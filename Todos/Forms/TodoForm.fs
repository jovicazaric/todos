module Todos.Forms.TodoForm

open Suave.Form
open System

type TodoModel = {
    TMId : string
    TMTitle : string
    TMDescription : string
    TMHappeningAt : DateTime
}

let titleNotEmpty =
    (fun x -> x.TMTitle <> ""), "Title is required"

let happeningAtInFuture = 
    (fun x -> x.TMHappeningAt > DateTime.Now), "Happening at must be in the future"

let Form : Form<TodoModel> = 
    Form ([ TextProp ((fun x -> <@ x.TMId @>), [])
            TextProp ((fun x -> <@ x.TMTitle @>), [])
            TextProp ((fun x -> <@ x.TMDescription @>), [])
            DateTimeProp ((fun x -> <@ x.TMHappeningAt @>), [])
    ], [titleNotEmpty; happeningAtInFuture])