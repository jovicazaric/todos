module Todos.Authentication

open Suave    
open Suave.Operators 
open Suave.State.CookieStateStore
open Suave.Authentication

type LoggedUser = {
    Id : string
    FullName : string
}

type SessionType = 
    | LoggedUserSession of LoggedUser
    | NoSession

let Session f =
    statefulForSession >=>
    context (fun ctx ->
        match HttpContext.state ctx with 
            | Some state -> 
                match (state.get "userId", state.get "userFullName")  with 
                    | Some(id), Some(fullName) -> f (LoggedUserSession {Id = id; FullName = fullName})
                    | _ -> f NoSession
            | None -> f NoSession)

let SessionStore setterFunction = context (fun ctx ->
    match HttpContext.state ctx with
        | Some state -> setterFunction state
        | None -> never
    )

let SessionBasedActions userLogged userNotLogged =
    Session (fun s -> 
        match s with    
            | LoggedUserSession _ -> userLogged
            | NoSession -> userNotLogged)
