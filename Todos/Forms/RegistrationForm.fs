module Todos.Forms.RegistrationForm

open Suave.Form

type Registration = {
    FirstName : string
    LastName : string
    Email : string
    Password : Password
    ConfirmPassword : Password
}

let private firstNameNotEmpty =
    (fun x -> x.FirstName <> ""), "First name is required"

let private lastNameNotEmpty =
    (fun x -> x.LastName <> ""), "Last name is required"

let private emailNotEmpty =
    (fun x -> x.Email <> ""), "Email is required"

let private passwordNotEmpty =
    (fun x -> x.Password <>  Password("")), "Password is required"

let private confirmPasswordNotEmpty =
    (fun x -> x.ConfirmPassword <> Password("")), "Confirm password is required"

let private checkPasswords =
    (fun x -> x.Password = x.ConfirmPassword), "Confirm password must match password"

let Form : Form<Registration> =
    Form ([ TextProp ((fun x -> <@ x.FirstName @>), [])
            TextProp ((fun x -> <@ x.LastName @>), [])
            TextProp ((fun x -> <@ x.Email @>), [])
            PasswordProp ((fun x -> <@ x.Password @>), [])
            PasswordProp ((fun x -> <@ x.ConfirmPassword @>), [])
    ], [firstNameNotEmpty; lastNameNotEmpty; emailNotEmpty; passwordNotEmpty; confirmPasswordNotEmpty; checkPasswords])