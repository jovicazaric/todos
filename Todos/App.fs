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
    choose [
        GET >=> Authentication.sessionBasedActions (buildPage OK (Views.Home.content (List.ofSeq Database.todoItems))) (Redirection.FOUND Paths.Pages.Login)
    ]
    
let login = 
    choose [
        GET >=> Authentication.sessionBasedActions (Redirection.FOUND Paths.Pages.Home) (buildPage OK (Views.Login.content None))
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
    Redirection.FOUND Paths.Pages.Login

let registration = 
    choose [
        GET >=> buildPage OK (Views.Registration.content None)
        POST >=> bindToForm RegistrationForm.Form (fun _form ->  buildPage OK (Views.Registration.content None)) (Views.Registration.content >> buildPage BAD_REQUEST)
    ]

let todo = 
    choose [
        GET >=> Authentication.sessionBasedActions 
            (request (fun req ->
                match req.queryParam "id" with 
                    | Choice1Of2 id -> 
                        match Database.TryFindTodo id with 
                            | Some t -> buildPage OK (Views.Todo.content (Some (t))  None)
                            | None -> NOT_FOUND (sprintf "Todo with id %s not found" id)  
                    | Choice2Of2 _ -> buildPage OK (Views.Todo.content None None) 
            )) 
            (Redirection.FOUND Paths.Pages.Login)
    ]

let completeTodo =
    choose [
        POST >=> bindToForm CompleteTodoForm.Form (fun form -> 
                let id = form.Id
                let todo = Database.TryFindTodo id

                match todo with 
                    | Some x -> 
                        if (x.IsCompleted) then
                            BAD_REQUEST (sprintf "Todo with id %s has been already completed." x.Id)
                        else
                            Database.MarkAsCompleted x.Id
                            Redirection.FOUND Paths.Pages.Home
                    | None -> BAD_REQUEST (sprintf "Todo with id %s not found." id)
            )
            (fun error -> BAD_REQUEST error.Value)
    ]

let resultWebPart = 
    choose [
        path Paths.Pages.Home >=> home
        path Paths.Pages.Login >=> login
        path Paths.Actions.Logout >=> logout
        path Paths.Pages.Registration >=> registration
        path Paths.Pages.Todo >=> todo
        path Paths.Actions.CompleteTodo >=> completeTodo
        pathRegex "(.*)\.css" >=> Files.browseHome
        pathRegex "(.*)\.js" >=> Files.browseHome
    ]
startWebServer defaultConfig resultWebPart
