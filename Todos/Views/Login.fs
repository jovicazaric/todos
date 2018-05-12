module Todos.Views.Login

open Suave.Html
open Todos.Views
open Todos.Views.Shared.Attributes

let private insertErrorMessage = function
    | Some x -> span [classAttr "error"] [Text x] 
    | _ -> Text ""

let private mainContent errorMessage = 
    div [] [
        div [classAttr "row"] [
            div [classAttr "offset-md-4 col-md-4"] [
                tag "h2" [] [Text "Log in"]
                p [] [Text "Please enter your credentials to start using application"]
            ]
        ]
        div [classAttr "row"] [
            div [classAttr "offset-md-4 col-md-4"] [
                tag "form" [methodAttr "post"] [
                    div [classAttr "form-group"] [
                        input [classAttr "form-control"; typeAttr "text"; nameAttr "email"; placeholderAttr "Email"]
                    ]
                    div [classAttr "form-group"] [
                        input [classAttr "form-control"; typeAttr "password"; nameAttr "password"; placeholderAttr "Password"]
                    ]
                    insertErrorMessage errorMessage
                    div [classAttr "form-group"] [
                        input [classAttr "form-control btn btn-success"; typeAttr "submit"; valueAttr "Log in"]
                    ]
                ]
            ]
        ]  
    ]
    
let content errorMessage = 
    Layout.buildPage "Login" (mainContent errorMessage) None None