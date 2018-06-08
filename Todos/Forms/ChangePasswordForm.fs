module Todos.Forms.ChangePasswordForm

open Suave.Form

type ChangePasswordModel = {
    CurrentPassword : Password
    NewPassword : Password
    ConfirmNewPassword : Password
}

let ChangePasswordForm : Form<ChangePasswordModel> =
    Form ([ PasswordProp ((fun x -> <@ x.CurrentPassword @>), []);
        PasswordProp ((fun x -> <@ x.NewPassword @>), []);
        PasswordProp ((fun x -> <@ x.ConfirmNewPassword @>), [])
    ], [])