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
open System

let bindToForm form success error = 
    request (fun req ->
        match bindForm form req with
            | Choice1Of2 f -> success f
            | Choice2Of2 msg -> error (Some msg))

let buildPage responseType pageData = 
    let result = Layout.buildPage pageData 
    
    Authentication.Session (fun s ->
        match s with 
            | Authentication.LoggedUserSession user -> responseType (result (Some user))
            | Authentication.NoSession -> responseType (result None)
    )

let handleErrorOnTodoPost error = 
    (request (fun req ->
        match req.formData "TMId" with
            | Choice1Of2 id when id <> "" -> 
                match Database.TryFindTodo id with 
                    | Some t -> (buildPage BAD_REQUEST (Views.Todo.content (Some(t)) error))
                    | None -> BAD_REQUEST (sprintf "Todo with id %s not found" id)
            | Choice1Of2 _ -> (buildPage BAD_REQUEST (Views.Todo.content None error))
            | Choice2Of2 _ -> (buildPage BAD_REQUEST (Views.Todo.content None error))
    ))



let handleUserDetailsUpdate (user : Database.User) =
    (bindToForm UserDetailsForm.UserDetailsForm 
        (fun form -> 
            match Database.IsUserEmailValidOnUpdate user form.UDMEmail with
                | true -> 
                    user.FirstName <- form.UDMFirstName
                    user.LastName <- form.UDMLastName
                    user.Email <- form.UDMEmail
                    Redirection.FOUND Paths.Actions.Logout
                | _ -> buildPage BAD_REQUEST (Views.UserDetails.content user (Some ("User with specified email already exists")))
        )
        (Views.UserDetails.content user >> buildPage BAD_REQUEST))

let handleChangePassword (user : Database.User) =
    (bindToForm ChangePasswordForm.ChangePasswordForm 
        (fun form -> 
            let (Password providedCurrentPassword) = form.CPMCurrentPassword
            match user.Password = providedCurrentPassword with
                | true ->
                    let (Password newPassword) = form.CPMNewPassword
                    user.Password <- newPassword
                    buildPage OK (Views.ChangePassword.content None)
                | _ -> buildPage BAD_REQUEST (Views.ChangePassword.content (Some ("Current password is invalid")))
        )
        (Views.ChangePassword.content >> buildPage BAD_REQUEST))
    

let home =
    choose [
        GET >=> Authentication.SessionBasedActions 
            (buildPage OK (Views.Home.content (List.ofSeq Database.TodoItems)))
            (Redirection.FOUND Paths.Pages.Login)
    ]
    
let login = 
    choose [
        GET >=> Authentication.SessionBasedActions 
            (Redirection.FOUND Paths.Pages.Home) 
            (buildPage OK (Views.Login.content None))
        POST >=> Authentication.SessionBasedActions 
                (Redirection.FOUND Paths.Pages.Home)
                (bindToForm LoginForm.Form 
                    (fun form -> 
                        let (Password password) = form.Password
                        let user = Database.TryFindUserByEmailPassword form.Email password
                        match user with
                            | Some x ->  
                                authenticated Cookie.CookieLife.Session false >=> 
                                Authentication.Session (fun _ -> succeed) >=>
                                Authentication.SessionStore (fun store -> store.set "userId" x.Id) >=>
                                Authentication.SessionStore (fun store -> store.set "userFullName" (sprintf "%s %s" x.FirstName x.LastName)) >=>
                                Redirection.FOUND Paths.Pages.Home 
                            | None ->  (buildPage BAD_REQUEST (Views.Login.content (Some "Invalid email or password"))))
                    (Views.Login.content >> buildPage BAD_REQUEST))
    ]

let logout =
    unsetPair SessionAuthCookie >=>
    unsetPair StateCookie >=>
    Redirection.FOUND Paths.Pages.Login

let registration = 
    choose [
        GET >=> Authentication.SessionBasedActions 
            (Redirection.FOUND Paths.Pages.Home)
            (buildPage OK (Views.Registration.content None))
        POST >=> Authentication.SessionBasedActions 
            (Redirection.FOUND Paths.Pages.Home)
            (bindToForm RegistrationForm.Form 
                (fun form ->
                    match Database.TryFindUserByEmail form.Email with 
                        | Some x -> buildPage BAD_REQUEST (Views.Registration.content (Some (sprintf "User with %s email already registered. Please try using another email." x.Email)))
                        | None ->
                            let (Password password) = form.Password
                            let newUser = {
                                Database.User.Id = Guid.NewGuid().ToString()
                                Database.User.FirstName = form.FirstName
                                Database.User.LastName = form.LastName
                                Database.User.Email = form.Email
                                Database.User.Password = password
                            }
                            Database.AddUser newUser
                            Redirection.FOUND Paths.Pages.Login
                ) 
                (Views.Registration.content >> buildPage BAD_REQUEST))
    ]

let todo = 
    choose [
        GET >=> Authentication.SessionBasedActions 
            (request (fun req ->
                match req.queryParam "id" with 
                    | Choice1Of2 id -> 
                        match Database.TryFindTodo id with 
                            | Some t -> buildPage OK (Views.Todo.content (Some (t))  None)
                            | None -> NOT_FOUND (sprintf "Todo with id %s not found" id)  
                    | Choice2Of2 _ -> buildPage OK (Views.Todo.content None None) 
            )) 
            (Redirection.FOUND Paths.Pages.Login)
        POST >=> Authentication.SessionBasedActions
            (bindToForm TodoForm.Form 
                (fun form ->
                    let todo = Database.TryFindTodo form.TMId
                    match todo with
                        | Some x -> 
                            x.Title <- form.TMTitle
                            x.Description <- form.TMDescription
                            x.HappeningAt <- form.TMHappeningAt
                        | None ->
                            let newTodo = {
                                Database.Todo.Id = Guid.NewGuid().ToString()
                                Database.Todo.Title = form.TMTitle
                                Database.Todo.Description = form.TMDescription
                                Database.Todo.HappeningAt = form.TMHappeningAt
                                Database.Todo.IsCompleted = false
                            }
                            Database.AddNewTodo newTodo
                    Redirection.FOUND Paths.Pages.Home
                )
                handleErrorOnTodoPost
            )
            (FORBIDDEN "You must be authenticated to perform this action.")          
    ]

let completeTodo =
    choose [
        POST >=> Authentication.SessionBasedActions
            (bindToForm CompleteTodoForm.Form 
                (fun form -> 
                    let id = form.Id
                    let todo = Database.TryFindTodo id

                    match todo with 
                        | Some x -> 
                            if (x.IsCompleted) then
                                BAD_REQUEST (sprintf "Todo with id %s has been already completed." x.Id)
                            else
                                Database.CompleteTodo x.Id
                                Redirection.FOUND Paths.Pages.Home
                        | None -> BAD_REQUEST (sprintf "Todo with id %s not found." id)
                )
                (fun error -> BAD_REQUEST error.Value)
            )
            (FORBIDDEN "You must be authenticated to perform this action.")
    ]

let userDetails =
    choose [
        GET >=> Authentication.Session 
            (fun s ->
                match s with 
                    | Authentication.LoggedUserSession userData -> 
                        let user = Database.TryFindUserById userData.Id
                        match user with 
                            | Some x -> buildPage OK (Views.UserDetails.content x None)
                            | None -> NOT_FOUND (sprintf "User with id %s not found." userData.Id)
                    | Authentication.NoSession -> Redirection.FOUND Paths.Pages.Login
            )
        POST >=> Authentication.Session
            (fun s ->
                match s with 
                    | Authentication.LoggedUserSession userData -> 
                        let user = Database.TryFindUserById userData.Id
                        match user with 
                            | Some user -> 
                                handleUserDetailsUpdate user
                            | None -> NOT_FOUND (sprintf "User with id %s not found." userData.Id)
                    | Authentication.NoSession -> (FORBIDDEN "You must be authenticated to perform this action.")
            )
    ]

let changePassword =
    choose [
        GET >=> Authentication.SessionBasedActions 
            (buildPage OK (Views.ChangePassword.content None))
            (Redirection.FOUND Paths.Pages.Home)
        POST >=> Authentication.Session
            (fun s ->
                match s with 
                    | Authentication.LoggedUserSession userData -> 
                        let user = Database.TryFindUserById userData.Id
                        match user with 
                            | Some user -> 
                                handleChangePassword user
                            | None -> NOT_FOUND (sprintf "User with id %s not found." userData.Id)
                    | Authentication.NoSession -> (FORBIDDEN "You must be authenticated to perform this action.")
            )

    ]

let resultWebPart = 
    choose [
        path Paths.Pages.Home >=> home
        path Paths.Pages.Login >=> login
        path Paths.Pages.Registration >=> registration
        path Paths.Pages.Todo >=> todo
        path Paths.Pages.UserDetails >=> userDetails
        path Paths.Pages.ChangePassword >=> changePassword
        path Paths.Actions.CompleteTodo >=> completeTodo
        path Paths.Actions.Logout >=> logout
        pathRegex "(.*)\.css" >=> Files.browseHome
        pathRegex "(.*)\.js" >=> Files.browseHome
    ]

startWebServer defaultConfig resultWebPart
