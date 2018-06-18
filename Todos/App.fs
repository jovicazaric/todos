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
open Todos.Views

let createTodo (form : TodoForm.TodoModel) userId =
    {
        Database.Todo.Id = Guid.NewGuid().ToString()
        Database.Todo.Title = form.TMTitle
        Database.Todo.Description = form.TMDescription
        Database.Todo.HappeningAt = form.TMHappeningAt
        Database.Todo.IsCompleted = false
        Database.Todo.UserId = userId
    }

let updateTodo (form : TodoForm.TodoModel) todo = 
    {
        todo with
            Database.Todo.Title = form.TMTitle
            Database.Todo.Description = form.TMDescription
            Database.Todo.HappeningAt = form.TMHappeningAt
    }

let createUser (form : RegistrationForm.Registration) =
    let (Password password) = form.Password
    {
        Database.User.Id = Guid.NewGuid().ToString()
        Database.User.FirstName = form.FirstName
        Database.User.LastName = form.LastName
        Database.User.Email = form.Email
        Database.User.Password = password
    }

let updateUserDetails (form : UserDetailsForm.UserDetailsModel)  user =
    { 
        user with 
            Database.User.FirstName = form.UDMFirstName
            Database.User.LastName = form.UDMLastName
            Database.User.Email = form.UDMEmail
    }

let updateUserPassword (form : ChangePasswordForm.ChangePasswordModel) user = 
    let (Password newPassword) = form.CPMNewPassword
    { 
        user with
            Database.User.Password = newPassword
    }

let bindToForm form success error = 
    request (fun req ->
                match bindForm form req with
                    | Choice1Of2 f -> success f
                    | Choice2Of2 msg -> 
                        Some msg 
                        |> error
            )

let buildPage responseType pageData = 
    let result = Layout.buildPage pageData 
    
    Authentication.Session (fun s ->
        match s with 
            | Authentication.LoggedUserSession user -> 
                Some user 
                |> result
                |> responseType
            | Authentication.NoSession ->
                result None 
                |> responseType
    )

let handleErrorOnTodoPost error = 
    request (fun req ->
        match req.formData "TMId" with
            | Choice1Of2 id when id <> "" -> 
                match Database.tryFindTodo id with 
                    | Some t -> 
                        Some t
                        |> Views.Todo.content <| error 
                        |> buildPage BAD_REQUEST
                    | None -> 
                        sprintf "Todo with id %s not found" id
                        |> BAD_REQUEST
            | _ -> 
                Views.Todo.content None error 
                |> buildPage BAD_REQUEST
    )

let handleUserDetailsUpdate (user : Database.User) =
    (bindToForm UserDetailsForm.UserDetailsForm 
        (fun form -> 
            match Database.isUserEmailValidOnUpdate user form.UDMEmail with
                | true -> 
                    updateUserDetails form user
                    |> Database.updateUser
                    Redirection.FOUND Paths.Actions.Logout
                | _ -> 
                    Some "User with specified email already exists"
                    |> Views.UserDetails.content user
                    |> buildPage BAD_REQUEST
        )
        (fun error -> 
            Views.UserDetails.content user error 
            |> buildPage BAD_REQUEST
        )
    )

let handleChangePassword (user : Database.User) =
    (bindToForm ChangePasswordForm.ChangePasswordForm 
        (fun form -> 
            let (Password providedCurrentPassword) = form.CPMCurrentPassword
            match user.Password = providedCurrentPassword with
                | true ->
                    updateUserPassword form user
                    |> Database.updateUser
                    Views.ChangePassword.content None 
                    |> buildPage OK
                | _ -> 
                    Some ("Current password is invalid")
                    |> Views.ChangePassword.content
                    |> buildPage BAD_REQUEST
        )
        (fun error -> 
            Views.ChangePassword.content error 
            |> buildPage BAD_REQUEST)
    )

let home =
    choose [
        GET >=> Authentication.Session
            (fun s ->
                match s with 
                    | Authentication.LoggedUserSession userData ->
                        Database.getTodos userData.Id
                        |> Views.Home.content <| None <| None
                        |> buildPage OK
                    | Authentication.NoSession -> Redirection.FOUND Paths.Pages.Login
            )
        POST >=> Authentication.Session
            (fun s ->
                match s with 
                    | Authentication.LoggedUserSession userData ->
                        (bindToForm TodoFilterForm.TodoFilterForm 
                            (fun form ->
                                printfn "%A" form
                                Database.getTodos userData.Id
                                |> Filter.filterTodos <| form 
                                |> Views.Home.content <| Some form <| None
                                |> buildPage OK
                            )
                            (fun error -> 
                                Database.getTodos userData.Id
                                |> Views.Home.content <| None <| error
                                |> buildPage BAD_REQUEST
                            )
                        )
                    | Authentication.NoSession -> FORBIDDEN "You must be authenticated to perform this action."
            )
    ]
    
let login = 
    choose [
        GET >=> Authentication.SessionBasedActions 
            (Redirection.FOUND Paths.Pages.Home) 
            (Views.Login.content None |> buildPage OK)
        POST >=> Authentication.SessionBasedActions 
            (Redirection.FOUND Paths.Pages.Home)
            (bindToForm LoginForm.Form 
                (fun form -> 
                    let (Password password) = form.Password
                    let user = Database.tryFindUserByEmailPassword form.Email password
                    match user with
                        | Some x ->
                            authenticated Cookie.CookieLife.Session false >=> 
                            Authentication.Session (fun _ -> succeed) >=>
                            Authentication.SessionStore (fun store -> store.set "userId" x.Id) >=>
                            Authentication.SessionStore (fun store -> store.set "userFullName" (sprintf "%s %s" x.FirstName x.LastName)) >=>
                            Redirection.FOUND Paths.Pages.Home
                        | None ->  
                            Some "Invalid email or password"
                            |> Views.Login.content
                            |> buildPage BAD_REQUEST
                )
                (fun error -> 
                    Views.Login.content error 
                    |> buildPage BAD_REQUEST
                )
            )
    ]

let logout =
    unsetPair SessionAuthCookie >=>
    unsetPair StateCookie >=>
    Redirection.FOUND Paths.Pages.Login

let registration = 
    choose [
        GET >=> Authentication.SessionBasedActions 
            (Redirection.FOUND Paths.Pages.Home)
            (Views.Registration.content None |> buildPage OK)
        POST >=> Authentication.SessionBasedActions 
            (Redirection.FOUND Paths.Pages.Home)
            (bindToForm RegistrationForm.Form 
                (fun form ->
                    match Database.tryFindUserByEmail form.Email with 
                        | Some x -> 
                            sprintf "User with %s email already registered. Please try using another email." x.Email
                            |> Some
                            |> Views.Registration.content
                            |> buildPage BAD_REQUEST
                        | None ->
                            createUser form 
                            |> Database.addUser
                            Redirection.FOUND Paths.Pages.Login
                ) 
                (fun error -> 
                    Views.Registration.content error 
                    |> buildPage BAD_REQUEST
                )
            )
    ]

let todo = 
    choose [
        GET >=> Authentication.SessionBasedActions 
            (request (fun req ->
                match req.queryParam "id" with 
                    | Choice1Of2 id -> 
                        match Database.tryFindTodo id with 
                            | Some t -> 
                                Some t
                                |> Views.Todo.content <| None
                                |> buildPage OK
                            | None -> 
                                sprintf "Todo with id %s not found" id
                                |> NOT_FOUND 
                    | Choice2Of2 _ -> 
                        Views.Todo.content None None 
                        |> buildPage OK 
            )) 
            (Redirection.FOUND Paths.Pages.Login)
        POST >=> Authentication.Session
            (fun s ->
                match s with 
                    | Authentication.LoggedUserSession userData ->
                        (bindToForm TodoForm.Form 
                            (fun form ->
                                let todo = Database.tryFindTodo form.TMId
                                match todo with
                                    | Some x -> 
                                        updateTodo form x 
                                        |> Database.updateTodo
                                    | None ->
                                        createTodo form userData.Id 
                                        |> Database.addTodo
                                Redirection.FOUND Paths.Pages.Home
                            )
                            handleErrorOnTodoPost
                        )
                    | Authentication.NoSession -> FORBIDDEN "You must be authenticated to perform this action."
            )
    ]

let completeTodo =
    choose [
        POST >=> Authentication.SessionBasedActions
            (bindToForm CompleteTodoForm.Form 
                (fun form -> 
                    let id = form.Id
                    let todo = Database.tryFindTodo id

                    match todo with 
                        | Some x -> 
                            if (x.IsCompleted) then
                                sprintf "Todo with id %s has been already completed." x.Id
                                |> BAD_REQUEST
                            else
                                Database.completeTodo x.Id
                                Redirection.FOUND Paths.Pages.Home
                        | None -> 
                            sprintf "Todo with id %s not found." id
                            |> BAD_REQUEST
                )
                (fun error -> error.Value |> BAD_REQUEST)
            )
            (FORBIDDEN "You must be authenticated to perform this action.")
    ]

let userDetails =
    choose [
        GET >=> Authentication.Session 
            (fun s ->
                match s with 
                    | Authentication.LoggedUserSession userData -> 
                        let user = Database.tryFindUserById userData.Id
                        match user with 
                            | Some x -> 
                                Views.UserDetails.content x None
                                |> buildPage OK
                            | None ->
                                sprintf "User with id %s not found." userData.Id
                                |> NOT_FOUND
                    | Authentication.NoSession -> Redirection.FOUND Paths.Pages.Login
            )
        POST >=> Authentication.Session
            (fun s ->
                match s with 
                    | Authentication.LoggedUserSession userData -> 
                        let user = Database.tryFindUserById userData.Id
                        match user with 
                            | Some user -> 
                                handleUserDetailsUpdate user
                            | None -> 
                                sprintf "User with id %s not found." userData.Id 
                                |> NOT_FOUND
                    | Authentication.NoSession -> (FORBIDDEN "You must be authenticated to perform this action.")
            )
    ]

let changePassword =
    choose [
        GET >=> Authentication.SessionBasedActions
            (Views.ChangePassword.content None |> buildPage OK)
            (Redirection.FOUND Paths.Pages.Home)
        POST >=> Authentication.Session
            (fun s ->
                match s with 
                    | Authentication.LoggedUserSession userData -> 
                        let user = Database.tryFindUserById userData.Id
                        match user with 
                            | Some user -> 
                                handleChangePassword user
                            | None -> 
                                sprintf "User with id %s not found." userData.Id
                                |> NOT_FOUND
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
