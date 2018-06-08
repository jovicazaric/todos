module Todos.Views.Account

open Suave.Html
open Todos.Forms.ChangePasswordForm
open Todos.Forms.UserDetailsForm
open Todos.Views.Layout
open Todos.Views.Shared

let private insertErrorMessage = function
    | Some x -> span [Attributes.classAttr "error"] [Text x] 
    | _ -> Text ""

let private mainContent errorMessageUserDetails errorMessageChangePassword = 
    div [] [
        div [Attributes.classAttr "row"] [
            div [Attributes.classAttr "offset-lg-4 col-lg-5"] [
                Nodes.h2 "Account settings"
            ]
        ]
        div [Attributes.classAttr "row"] [
            div [Attributes.classAttr "offset-lg-4 col-lg-5 account-setting-section"] [
                Nodes.h4 "Basic settings"
                Nodes.form [Attributes.methodAttr "post"] [
                    div [Attributes.classAttr "form-group"] [
                        Nodes.label "First name *"
                        Suave.Form.input (fun x -> <@ x.FirstName @>) [Attributes.classAttr "form-control"] UserDetailsForm
                    ]
                    div [Attributes.classAttr "form-group"] [
                        Nodes.label "Last name *"
                        Suave.Form.input (fun x -> <@ x.LastName @>) [Attributes.classAttr "form-control"] UserDetailsForm
                    ]
                    div [Attributes.classAttr "form-group"] [
                        Nodes.label "Email *"
                        Suave.Form.input (fun x -> <@ x.Email @>) [Attributes.classAttr "form-control"] UserDetailsForm
                    ]
                    insertErrorMessage errorMessageUserDetails
                    div [Attributes.classAttr "form-group"] [
                        Suave.Html.input [Attributes.classAttr "form-control btn btn-success"; Attributes.typeAttr "submit"; Attributes.valueAttr "Submit changes"]
                    ]
                ]
            ]
        ]
        div [Attributes.classAttr "row"] [
            div [Attributes.classAttr "offset-lg-4 col-lg-5 account-setting-section"] [
                Nodes.h4 "Security settings"
                Nodes.form [Attributes.methodAttr "post"] [
                    div [Attributes.classAttr "form-group"] [
                        Nodes.label "Current password *"
                        Suave.Form.input (fun x -> <@ x.CurrentPassword @>) [Attributes.classAttr "form-control"] ChangePasswordForm
                    ]
                    div [Attributes.classAttr "form-group"] [
                        Nodes.label "New password *"
                        Suave.Form.input (fun x -> <@ x.NewPassword @>) [Attributes.classAttr "form-control"] ChangePasswordForm
                    ]
                    div [Attributes.classAttr "form-group"] [
                        Nodes.label "Confirm new password *"
                        Suave.Form.input (fun x -> <@ x.ConfirmNewPassword @>) [Attributes.classAttr "form-control"] ChangePasswordForm
                    ]
                    insertErrorMessage errorMessageChangePassword
                    div [Attributes.classAttr "form-group"] [
                        Suave.Html.input [Attributes.classAttr "form-control btn btn-success"; Attributes.typeAttr "submit"; Attributes.valueAttr "Submit changes"]
                    ]
                ]
            ]
        ]
    ]
    
let content errorMessageUserDetails errorMessageChangePassword = 
    { Title = "Account"; Content = mainContent errorMessageUserDetails errorMessageChangePassword }