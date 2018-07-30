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

let session handler =
    statefulForSession >=>
    context (fun ctx ->
        match HttpContext.state ctx with 
            | Some state -> 
                match (state.get "userId", state.get "userFullName")  with 
                    | Some(id), Some(fullName) -> (LoggedUserSession {Id = id; FullName = fullName} |> handler )
                    | _ -> handler NoSession
            | None -> handler NoSession)

let stateStore handler = context (fun ctx ->
    match HttpContext.state ctx with
        | Some state -> handler  state
        | None -> never
    )

let sessionBasedActions userLogged userNotLogged =
    session (fun session -> 
        match session with    
            | LoggedUserSession _ -> userLogged
            | NoSession -> userNotLogged)
