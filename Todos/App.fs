module Todos.App

open Suave
open Suave.Filters     
open Suave.Operators 
open Suave.RequestErrors
open Suave.Successful
open Suave.Form
open Suave.Model.Binding
open Todos.Views
open Todos.Forms

let bindToForm form success error =
    request (fun req ->
        match bindForm form req with
            | Choice1Of2 f -> success f
            | Choice2Of2 msg -> error (Some msg)
    )

let home = OK Views.Home.content

let login = 
    choose [
        GET >=> OK (Views.Login.content None)
        POST >=> bindToForm LoginForm.Form (fun _ -> OK (Views.Home.content)) (Views.Login.content >> BAD_REQUEST)
    ]

let registration = 
    choose [
        GET >=> OK (Views.Registration.content None)
        POST >=> bindToForm RegistrationForm.Form (fun _form -> OK (Views.Registration.content None)) (Views.Registration.content >> BAD_REQUEST)
    ]
    
let resultWebPart = 
    choose [
        path Paths.Pages.home >=> home
        path Paths.Pages.login >=> login
        path Paths.Pages.registration >=> registration
        pathRegex "(.*)\.css" >=> Files.browseHome
        pathRegex "(.*)\.js" >=> Files.browseHome
    ]
startWebServer defaultConfig resultWebPart
