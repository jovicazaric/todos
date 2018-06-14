module Todos.Views.Home

open Suave.Html
open System
open Todos.Views.Layout
open Todos.Views.Shared
open Todos.Forms.TodoFilterForm
open Todos

let private insertErrorMessage = function
    | Some x -> span [Attributes.classAttr "error"] [Text x] 
    | _ -> Text ""

let getTodoItemCssClass (todo : Database.Todo) =
    if todo.IsCompleted then
        "completed"
    else if todo.HappeningAt < DateTime.Now then
        "incompleted"
    else 
        "upcomming"

let private renderControls (todo : Database.Todo) =
    match todo.IsCompleted with
    | true -> Text ""
    | false ->
        div [Attributes.classAttr "row todo-controls"] [
            Nodes.form [Attributes.methodAttr "post"; Attributes.actionAttr Paths.Actions.CompleteTodo] [
                Suave.Html.input [Attributes.typeAttr "hidden"; Attributes.valueAttr todo.Id; Attributes.nameAttr "Id"]
                Suave.Html.input [Attributes.classAttr "form-control btn btn-success btn-todo-complete"; Attributes.typeAttr "submit"; Attributes.valueAttr "Complete"]
            ]
            a (QueryString.addToUrl Paths.Pages.Todo "id" todo.Id) [Attributes.classAttr "btn btn-primary btn-todo-edit"] [Text "Edit"]   
        ]
 
let private renderTodoItem (todo : Database.Todo) =
    div [Attributes.classAttr ("col-md-4 todo-item todo-item-" + (getTodoItemCssClass todo))] [
        Nodes.h4 todo.Title
        p [] [Text todo.Description]
        p [Attributes.classAttr "time"] [Text (sprintf "Time: %s" (todo.HappeningAt.ToString "dd. MMMM yyyy. HH:mm" ))]
        renderControls todo
    ]

let private renderTodoItems (todos : Database.Todo list) =  
    List.map renderTodoItem todos

let private renderFilterFrom (filter : TodoFilterModel option) =
    let value = 
        if filter.IsSome && filter.Value.TFMFrom.IsSome 
        then filter.Value.TFMFrom.Value.ToString "yyyy-MM-ddTHH:mm" 
        else ""

    Suave.Form.input (fun x -> <@ x.TFMFrom @>) [Attributes.classAttr "form-control"; Attributes.valueAttr value] TodoFilterForm

let private renderFilterTo (filter : TodoFilterModel option) =
    let value = 
        if filter.IsSome && filter.Value.TFMTo.IsSome 
        then filter.Value.TFMTo.Value.ToString "yyyy-MM-ddTHH:mm" 
        else ""

    Suave.Form.input (fun x -> <@ x.TFMTo @>) [Attributes.classAttr "form-control"; Attributes.valueAttr value] TodoFilterForm
    
let private renderFilterForm (filter : TodoFilterModel option) errorMessage = 
    div [] [
        Nodes.h4 "Filter"
        Nodes.form [Attributes.methodAttr "post"; Attributes.novalidateAttr] [
            div [Attributes.classAttr "form-group"] [
                Nodes.label "From"
                renderFilterFrom filter
            ]
            div [Attributes.classAttr "form-group"] [
                Nodes.label "To"
                renderFilterTo filter
            ]
            div [Attributes.classAttr "form-group"] [
                Nodes.label "Completed"
                Suave.Html.input [Attributes.classAttr "form-control"; Attributes.typeAttr "checkbox"; Attributes.nameAttr "TFMIsCompleted"; Attributes.valueAttr "1"] 
                Suave.Html.input [Attributes.typeAttr "hidden"; Attributes.nameAttr "TFMIsCompleted"; Attributes.valueAttr "0"] 
            ]
            insertErrorMessage errorMessage
            div [Attributes.classAttr "form-group"] [
                Suave.Html.input [Attributes.classAttr "form-control btn btn-success"; Attributes.typeAttr "submit"; Attributes.valueAttr "Submit"]
            ]
        ]
    ]

let private mainContent todos filter errorMessage = 
    div [] [
        div [Attributes.classAttr "row"] [
            div [Attributes.classAttr "col-md-3"] [
                div [Attributes.classAttr "row"] [
                    a Paths.Pages.Todo [Attributes.classAttr "btn btn-success"] [
                        Text "+ New todo"
                    ]
                ]
            ]
            div [Attributes.classAttr "col-md-9"] [
                Nodes.h2 "Todos"
                p [] [Text "Try using filter options on the left side panel."]
                Text "Legend: "
                Nodes.ul [Attributes.classAttr "legend-list"] [
                    Nodes.li [] [
                        div [Attributes.classAttr "legend-item todo-item-upcomming"] []
                        Text "Upcomming"
                    ]
                    Nodes.li [] [
                        div [Attributes.classAttr "legend-item todo-item-completed"] []
                        Text "Completed"
                    ]
                    Nodes.li [] [
                        div [Attributes.classAttr "legend-item todo-item-incompleted"] []
                        Text "Incompleted"
                    ]
                ]
            ]
        ]
        div [Attributes.classAttr "row"] [
            div [Attributes.classAttr "col-md-3"] [
                div [Attributes.classAttr "row"] [
                    renderFilterForm filter errorMessage
                ]
            ]
            div [Attributes.classAttr "col-md-9"] [
                div [Attributes.classAttr "row flex-row"] (renderTodoItems todos)
            ]
        ]
    ]

let content todos filter errorMessage  =
    { Title = "Home"; Content = mainContent todos filter errorMessage}