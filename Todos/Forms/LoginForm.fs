module Todos.Forms.LoginForm

open Suave.Form

type Login = {
    Email : string
    Password : Password
}

[<Literal>]
let private RequiredFieldsErrorMessage = "Both email and password inputs are required"

let private emailNotEmpty =
    (fun x -> x.Email <> ""), RequiredFieldsErrorMessage

let private passwordNotEmpty =
    (fun x -> x.Password <>  Password("")), RequiredFieldsErrorMessage

let Form : Form<Login> =
    Form ([ TextProp ((fun x -> <@ x.Email @>), [])
            PasswordProp ((fun x -> <@ x.Password @>), [])
    ], [emailNotEmpty; passwordNotEmpty])