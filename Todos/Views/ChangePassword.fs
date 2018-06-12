module Todos.Views.ChangePassword

open Suave.Html
open Todos.Forms.ChangePasswordForm
open Todos.Views.Layout
open Todos.Views.Shared

let private insertErrorMessage = function
    | Some x -> span [Attributes.classAttr "error"] [Text x] 
    | _ -> Text ""

let private mainContent errorMessage = 
    div [] [
        div [Attributes.classAttr "row"] [
            div [Attributes.classAttr "offset-lg-4 col-lg-5 account-setting-section"] [
                Nodes.h2 "Change password"
                Nodes.form [Attributes.methodAttr "post"] [
                    div [Attributes.classAttr "form-group"] [
                        Nodes.label "Current password *"
                        Suave.Form.input (fun x -> <@ x.CPMCurrentPassword @>) [Attributes.classAttr "form-control"] ChangePasswordForm
                    ]
                    div [Attributes.classAttr "form-group"] [
                        Nodes.label "New password *"
                        Suave.Form.input (fun x -> <@ x.CPMNewPassword @>) [Attributes.classAttr "form-control"] ChangePasswordForm
                    ]
                    div [Attributes.classAttr "form-group"] [
                        Nodes.label "Confirm new password *"
                        Suave.Form.input (fun x -> <@ x.CPMConfirmNewPassword @>) [Attributes.classAttr "form-control"] ChangePasswordForm
                    ]
                    insertErrorMessage errorMessage
                    div [Attributes.classAttr "form-group"] [
                        Suave.Html.input [Attributes.classAttr "form-control btn btn-success"; Attributes.typeAttr "submit"; Attributes.valueAttr "Submit changes"]
                    ]
                ]
            ]
        ]
    ]

let content errorMessage = 
    { Title = "Change password"; Content = mainContent errorMessage }