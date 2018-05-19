module Todos.Views.Registration

open Suave.Html
open Todos.Forms.RegistrationForm
open Todos.Views.Shared.Attributes
open Todos.Views.Shared
open Todos.Views.Layout
open Todos

let private insertErrorMessage = function
    | Some x -> Nodes.errorSpan x
    | _ -> Text ""

let private mainContent error = 
    div [] [
        div [classAttr "row"] [
            div [classAttr "offset-md-4 col-md-4"] [
                Nodes.h2 "Registration"
                Nodes.textP "Please enter your personal details to register"
            ]
        ]
        div [classAttr "row"] [
            div [classAttr "offset-md-4 col-md-4"] [
                tag "form" [methodAttr "post"] [
                    div [classAttr "form-group"] [
                        Nodes.label "First name *"
                        Suave.Form.input (fun x -> <@ x.FirstName @>) [classAttr "form-control"] Form
                    ]
                    div [classAttr "form-group"] [
                        Nodes.label "Last name *"
                        Suave.Form.input (fun x -> <@ x.LastName @>) [classAttr "form-control"] Form
                    ]
                    div [classAttr "form-group"] [
                        Nodes.label "Email *"
                        Suave.Form.input (fun x -> <@ x.Email @>) [classAttr "form-control"] Form
                    ]
                    div [classAttr "form-group"] [
                        Nodes.label "Password *"
                        Suave.Form.input (fun x -> <@ x.Password @>) [classAttr "form-control"] Form
                    ]
                    div [classAttr "form-group"] [
                        Nodes.label "Confirm password *"
                        Suave.Form.input (fun x -> <@ x.ConfirmPassword @>) [classAttr "form-control"] Form
                    ]
                    insertErrorMessage error
                    div [classAttr "form-group"] [
                        Suave.Html.input [classAttr "form-control btn btn-success"; typeAttr "submit"; valueAttr "Register"]
                    ]
                ]
                p [] [
                    Text "If you have already registered, please "
                    a Paths.Pages.login [] [
                        Text "log in"
                    ]
                ]
            ]
        ]  
    ]
    
let content error =
   buildPage "Registration" (mainContent error) None None