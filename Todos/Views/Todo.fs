module Todos.Views.Todo

open Suave.Html
//open Todos.Forms.LoginForm
open Todos.Views.Layout
open Todos.Views.Shared
open Todos.Database

let private insertErrorMessage = function
    | Some x -> span [Attributes.classAttr "error"] [Text x] 
    | _ -> Text ""

let private title = function 
    | Some x -> sprintf "Edit - %s" x.Title
    | None -> "New todo"

let submitButtonText = function 
    | Some _ -> "Edit"
    | None -> "Create"

let private mainContent todo errorMessage = 
    div [] [
        div [Attributes.classAttr "row"] [
            div [Attributes.classAttr "offset-md-4 col-md-4"] [
                Nodes.h2 (title todo)
            ]
        ]
        div [Attributes.classAttr "row"] [
            div [Attributes.classAttr "offset-md-4 col-md-4"] [
                Nodes.form [Attributes.methodAttr "post"] [
                    div [Attributes.classAttr "form-group"] [
                        Nodes.label "Title *"
                        //Suave.Form.input (fun x -> <@ x.Email @>) [Attributes.classAttr "form-control"] Form
                    ]
                    div [Attributes.classAttr "form-group"] [
                        Nodes.label "Description *"
                        //Suave.Form.input (fun x -> <@ x.Password @>) [Attributes.classAttr "form-control"] Form
                    ]
                    //insertErrorMessage errorMessage
                    div [Attributes.classAttr "form-group"] [
                        Suave.Html.input [Attributes.classAttr "form-control btn btn-success"; Attributes.typeAttr "submit"; Attributes.valueAttr (submitButtonText todo)]
                    ]
                ]
            ]
        ]  
    ]
    
let content todo errorMessage = 
    { Title = (title todo); Content = mainContent todo errorMessage }