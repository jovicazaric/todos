module Todos.Views.Todo

open Suave.Html
open Todos.Forms.TodoForm
open Todos.Views.Layout
open Todos.Views.Shared
open Todos.Database
open Todos


let private insertErrorMessage = function
    | Some x -> span [Attributes.classAttr "error"] [Text x] 
    | _ -> Text ""

let private title = function 
    | Some x -> sprintf "Edit - %s" x.Title
    | None -> "New todo"

let private submitButtonText = function 
    | Some _ -> "Edit"
    | None -> "Create"


let private todoTitle = function 
    | Some x -> x.Title
    | None -> ""

let private todoDescription = function 
    | Some x -> x.Description
    | None -> ""

let private todoHappeningAt = function 
    | Some x -> x.HappeningAt.ToString "yyyy-MM-ddTHH:mm"
    | None -> ""

let private renderDescription todo = 
    let text = 
        match todo with
            | Some x -> x.Description
            | None -> ""

    Nodes.textArea [Attributes.classAttr "form-control"; Attributes.nameAttr "TMDescription"; Attributes.rowsAttr 5] text

let private renderHiddenId todo =
    let id = 
        match todo with 
            | Some x -> x.Id
            | None -> ""

    Suave.Html.input [Attributes.typeAttr "hidden"; Attributes.valueAttr id; Attributes.nameAttr "TMId"]

let private renderNewTodoLink = function 
    | Some _ -> 
        p [] [
            Text "Or "
            a Paths.Pages.Todo [] [
                Text "create new one."
            ]
        ]
    | None -> Text ""

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
                        Suave.Form.input (fun x -> <@ x.TMTitle @>) [Attributes.classAttr "form-control"; Attributes.valueAttr (todoTitle todo)] Form
                    ]
                    div [Attributes.classAttr "form-group"] [
                        Nodes.label "Description *"
                        renderDescription todo
                    ]
                    div [Attributes.classAttr "form-group"] [
                        Nodes.label "Happening at *"
                        Suave.Form.input (fun x -> <@ x.TMHappeningAt @>) [Attributes.classAttr "form-control"; Attributes.valueAttr (todoHappeningAt todo)] Form
                    ]
                    insertErrorMessage errorMessage
                    renderHiddenId todo
                    div [Attributes.classAttr "form-group"] [
                        Suave.Html.input [Attributes.classAttr "form-control btn btn-success"; Attributes.typeAttr "submit"; Attributes.valueAttr (submitButtonText todo)]
                    ]
                ]
                renderNewTodoLink todo
            ]
        ]  
    ]
    
let content todo errorMessage = 
    { Title = (title todo); Content = mainContent todo errorMessage }