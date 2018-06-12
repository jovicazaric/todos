module Todos.Forms.ChangePasswordForm

open Suave.Form

type ChangePasswordModel = {
    CPMCurrentPassword : Password
    CPMNewPassword : Password
    CPMConfirmNewPassword : Password
}

let private currentPasswordNotEmpty =
    (fun x -> x.CPMCurrentPassword <>  Password("")), "Current password is required"

let private newPasswordNotEmpty =
    (fun x -> x.CPMNewPassword <> Password("")), "New password is required"

let private confirmNewPasswordNotEmpty =
    (fun x -> x.CPMConfirmNewPassword <> Password("")), "Confirm new password is required"

let private checkPasswords =
    (fun x -> x.CPMNewPassword = x.CPMConfirmNewPassword), "Confirm new password must match new password"

let ChangePasswordForm : Form<ChangePasswordModel> =
    Form ([ PasswordProp ((fun x -> <@ x.CPMCurrentPassword @>), [])
            PasswordProp ((fun x -> <@ x.CPMNewPassword @>), [])
            PasswordProp ((fun x -> <@ x.CPMConfirmNewPassword @>), [])
    ], [currentPasswordNotEmpty; newPasswordNotEmpty; confirmNewPasswordNotEmpty; checkPasswords])