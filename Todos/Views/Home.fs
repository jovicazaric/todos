module Todos.Views.Home

open Suave.Html
open System
open Todos.Views.Layout
open Todos.Views.Shared
open Todos

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
    div [Attributes.classAttr ("col-md-3 todo-item todo-item-" + (getTodoItemCssClass todo))] [
        Nodes.h4 todo.Title
        p [] [Text todo.Description]
        p [Attributes.classAttr "time"] [Text (sprintf "Time: %s" (todo.HappeningAt.ToString "dd. MMMM yyyy. HH:mm" ))]
        renderControls todo
    ]

let private renderTodoItems (todos : Database.Todo list) =  
    List.map renderTodoItem todos

let private mainContent todos = 
    div[] [
        div [Attributes.classAttr "row"] [
            div [Attributes.classAttr "col-md-3"] [
                a Paths.Pages.Todo [Attributes.classAttr "btn btn-success"] [
                    Text "+ New todo"
                ]
                // Nodes.ul [] [
                //     Nodes.li [] [
                //         a Paths.Pages.Registration [] [
                //             Text "All"
                //         ]
                //     ]
                //     Nodes.li [] [
                //         a Paths.Pages.Registration [] [
                //             Text "Upcomming"
                //         ]
                //     ]
                //     Nodes.li [] [
                //         a Paths.Pages.Registration [] [
                //             Text "Completed"
                //         ]
                //     ]
                //     Nodes.li [] [
                //         a Paths.Pages.Registration [] [
                //             Text "Incompleted"
                //         ]
                //     ]
                // ]
            ]
            div [Attributes.classAttr "col-md-9"] [
                Nodes.h2 "Todos"
                p [] [Text "Try using filter options on the left side panel."]
                Text "Legend: "
                Nodes.ul [] [
                    Nodes.li [] [
                        Text "Upcomming"
                        div [Attributes.classAttr "legend-item todo-item-upcomming"] []
                    ]
                    Nodes.li [] [
                        Text "Completed"
                        div [Attributes.classAttr "legend-item todo-item-completed"] []
                    ]
                    Nodes.li [] [
                        Text "Incompleted"
                        div [Attributes.classAttr "legend-item todo-item-incompleted"] []
                    ]
                ]
            ]
        ]
        div [Attributes.classAttr "row"] [
            div [Attributes.classAttr "offset-md-3 col-md-12"] [
                div [Attributes.classAttr "row flex-row"] (renderTodoItems todos)
            ]
        ]
    ]

let content todos =
    { Title = "Home"; Content = mainContent todos}