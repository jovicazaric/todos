module Todos.Forms.CompleteTodoForm

open Suave.Form

type CompleteTodoModel = {
    Id : string
}

[<Literal>]
let ErrorMessage = "Id is missing or in invalid format."

let isIdValid =
    (fun x -> x.Id <> ""), ErrorMessage

let Form : Form<CompleteTodoModel> =
    Form ([ TextProp  ((fun x -> <@ x.Id @>), [])
    ], [isIdValid])