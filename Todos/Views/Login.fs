module Todos.Views.Login

open Suave.Html
open Todos.Forms.LoginForm
open Todos.Views.Layout
open Todos.Views.Shared
open Todos

let private insertErrorMessage = function
    | Some x -> span [Attributes.classAttr "error"] [Text x] 
    | _ -> Text ""

let private mainContent errorMessage = 
    div [] [
        div [Attributes.classAttr "row"] [
            div [Attributes.classAttr "offset-md-4 col-md-4"] [
                Nodes.h2 "Log in"
                Nodes.textP "Please enter your credentials to start using application"
            ]
        ]
        div [Attributes.classAttr "row"] [
            div [Attributes.classAttr "offset-md-4 col-md-4"] [
                Nodes.form [Attributes.methodAttr "post"] [
                    div [Attributes.classAttr "form-group"] [
                        Nodes.label "Email *"
                        Suave.Form.input (fun x -> <@ x.Email @>) [Attributes.classAttr "form-control"] Form
                    ]
                    div [Attributes.classAttr "form-group"] [
                        Nodes.label "Password *"
                        Suave.Form.input (fun x -> <@ x.Password @>) [Attributes.classAttr "form-control"] Form
                    ]
                    insertErrorMessage errorMessage
                    div [Attributes.classAttr "form-group"] [
                        Suave.Html.input [Attributes.classAttr "form-control btn btn-success"; Attributes.typeAttr "submit"; Attributes.valueAttr "Log in"]
                    ]
                ]
                p [] [
                    Text "If you do not have account, please "
                    a Paths.Pages.registration [] [
                        Text "register"
                    ]
                ]
            ]
        ]  
    ]
    
let content errorMessage = 
    { Title = "Login"; Content = mainContent errorMessage }