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


let bindToError content error =
    BAD_REQUEST (content error)

let bindToForm1 form handler error =
    bindReq (bindForm form) handler BAD_REQUEST



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
        POST >=> BAD_REQUEST (Views.Login.content (Some "Invalid credentials. Please try again."))
    ]

let registration = 
    choose [
        GET >=> OK (Views.Registration.content None)
        POST >=> bindToForm RegistrationForm.Form (fun form -> OK (Views.Registration.content None)) (fun error -> BAD_REQUEST (Views.Registration.content error))
        //POST >=> bindToForm Registration.RegistrationForm (fun form -> home) (bindToError (Views.Registration.content Views.Registration.RegistrationForm))
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
