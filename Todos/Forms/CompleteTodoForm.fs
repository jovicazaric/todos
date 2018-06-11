module Todos.Forms.CompleteTodoForm

open Suave.Form

type CompleteTodoModel = {
    Id : string
}

[<Literal>]
let private ErrorMessage = "Id is missing or in invalid format."

let private isIdValid =
    (fun x -> x.Id <> ""), ErrorMessage

let Form : Form<CompleteTodoModel> =
    Form ([ TextProp  ((fun x -> <@ x.Id @>), [])
    ], [isIdValid])