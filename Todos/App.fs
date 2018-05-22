module Todos.App

open Suave
open Suave.Filters     
open Suave.Operators 
open Suave.Successful
open Suave.Form
open Suave.RequestErrors
open Suave.State.CookieStateStore
open Suave.Authentication
open Todos.Views
open Todos.Forms
open Todos
open Suave.Cookie

let bindToForm form success error = 
    request (fun req ->
        match bindForm form req with
            | Choice1Of2 f -> success f
            | Choice2Of2 msg -> error (Some msg))

let buildPage responseType pageData = 
    let result = Layout.buildPage pageData 
    
    Authentication.session (fun s ->
        match s with 
            | Authentication.LoggedUserSession user -> responseType (result (Some user))
            | Authentication.NoSession -> responseType (result None)
    ) 

let returnPath = 
    Redirection.FOUND "/"

let validateUser email password =
    if (email = "jovica@zaric.com" && password = "111") then
        Some email
    else
        None

let home =
    Authentication.sessionBasedActions (buildPage OK Views.Home.content) (Redirection.FOUND Paths.Pages.login)

let login = 
    choose [
        GET >=> Authentication.sessionBasedActions (Redirection.FOUND Paths.Pages.home) (buildPage OK (Views.Login.content None))
        POST >=> bindToForm LoginForm.Form 
            (fun form -> 
                let (Password password) = form.Password
                match (validateUser form.Email password) with
                    | Some x ->  
                        authenticated Cookie.CookieLife.Session false >=> 
                        Authentication.session (fun _ -> succeed) >=>
                        Authentication.sessionStore (fun store -> store.set "email" x ) >=>
                        returnPath 
                    | None ->  (buildPage BAD_REQUEST (Views.Login.content (Some "Invalid email or password"))))
            (Views.Login.content >> buildPage BAD_REQUEST)
    ]

let logout =
    unsetPair SessionAuthCookie >=>
    unsetPair StateCookie >=>
    Redirection.FOUND Paths.Pages.login

let registration = 
    choose [
        GET >=> buildPage OK (Views.Registration.content None)
        POST >=> bindToForm RegistrationForm.Form (fun _form ->  buildPage OK (Views.Registration.content None)) (Views.Registration.content >> buildPage BAD_REQUEST)
    ]
    
let resultWebPart = 
    choose [
        path Paths.Pages.home >=> home
        path Paths.Pages.login >=> login
        path Paths.Pages.logout >=> logout
        path Paths.Pages.registration >=> registration
        pathRegex "(.*)\.css" >=> Files.browseHome
        pathRegex "(.*)\.js" >=> Files.browseHome
    ]
startWebServer defaultConfig resultWebPart
