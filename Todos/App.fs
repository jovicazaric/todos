module Todos.App

open Suave
open Suave.Filters     
open Suave.Operators 
open Suave.RequestErrors
open Suave.Successful


// let todoWebPart userId todoId = OK  (sprintf "Todo with id %d created by user %d" todoId userId)

// let resultWebPart = 
//     choose [
//         path "/" >=> (OK "Home")
//         pathScan "/user/%d/todo/%d" (fun (uId, tId) -> todoWebPart uId tId)
//         NOT_FOUND "404 Not found"
//     ]


let browse =
    request (fun req ->
        match req.queryParam "id" with
            | Choice1Of2 genre -> OK (sprintf "Todo with id %s" genre)
            | Choice2Of2 msg -> BAD_REQUEST msg
    )

let home = OK Views.Home.content

let login = 
    choose [
        GET >=> OK (Views.Login.content None)
        POST >=> BAD_REQUEST (Views.Login.content (Some "Invalid credentials. Please try again."))
    ]

let registration = 
    choose [
        GET >=> OK (Views.Registration.content Views.Registration.RegistrationForm)
        //POST >=> BAD_REQUEST (Views.Login.content (Some "Invalid credentials. Please try again."))
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
