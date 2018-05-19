module Todos.Forms.LoginForm

open Suave.Form

type Login = {
    Email : string
    Password : Password
}

let requiredFieldsErrorMessage = "Both email and password inputs are required"

let emailNotEmpty =
    (fun x -> x.Email <> ""), requiredFieldsErrorMessage

let passwordNotEmpty =
    (fun x -> x.Password <>  Password("")), requiredFieldsErrorMessage

let Form : Form<Login> =
    Form ([ TextProp ((fun x -> <@ x.Email @>), [])
            PasswordProp ((fun x -> <@ x.Password @>), [])
    ], [emailNotEmpty; passwordNotEmpty])