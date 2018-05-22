module Todos.Views.Registration

open Suave.Html
open Todos.Forms.RegistrationForm
open Todos.Views.Shared
open Todos.Views.Layout
open Todos

let private insertErrorMessage = function
    | Some x -> Nodes.errorSpan x
    | _ -> Text ""

let private mainContent error = 
    div [] [
        div [Attributes.classAttr "row"] [
            div [Attributes.classAttr "offset-md-4 col-md-4"] [
                Nodes.h2 "Registration"
                Nodes.textP "Please enter your personal details to register"
            ]
        ]
        div [Attributes.classAttr "row"] [
            div [Attributes.classAttr "offset-md-4 col-md-4"] [
                Nodes.form [Attributes.methodAttr "post"] [
                    div [Attributes.classAttr "form-group"] [
                        Nodes.label "First name *"
                        Suave.Form.input (fun x -> <@ x.FirstName @>) [Attributes.classAttr "form-control"] Form
                    ]
                    div [Attributes.classAttr "form-group"] [
                        Nodes.label "Last name *"
                        Suave.Form.input (fun x -> <@ x.LastName @>) [Attributes.classAttr "form-control"] Form
                    ]
                    div [Attributes.classAttr "form-group"] [
                        Nodes.label "Email *"
                        Suave.Form.input (fun x -> <@ x.Email @>) [Attributes.classAttr "form-control"] Form
                    ]
                    div [Attributes.classAttr "form-group"] [
                        Nodes.label "Password *"
                        Suave.Form.input (fun x -> <@ x.Password @>) [Attributes.classAttr "form-control"] Form
                    ]
                    div [Attributes.classAttr "form-group"] [
                        Nodes.label "Confirm password *"
                        Suave.Form.input (fun x -> <@ x.ConfirmPassword @>) [Attributes.classAttr "form-control"] Form
                    ]
                    insertErrorMessage error
                    div [Attributes.classAttr "form-group"] [
                        Suave.Html.input [Attributes.classAttr "form-control btn btn-success"; Attributes.typeAttr "submit"; Attributes.valueAttr "Register"]
                    ]
                ]
                p [] [
                    Text "If you have already registered, please "
                    a Paths.Pages.Login [] [
                        Text "log in"
                    ]
                ]
            ]
        ]  
    ]
    
let content errorMessage =
    { Title = "Registration"; Content = mainContent errorMessage }