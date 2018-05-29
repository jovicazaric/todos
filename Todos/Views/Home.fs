module Todos.Views.Home

open Suave.Html
open Todos.Views.Layout
open Todos.Views.Shared
open Todos

let private renderCompleteForm (todo : Database.Todo) =
    match todo.IsCompleted with
    | true -> Text ""
    | false -> 
        Nodes.form [Attributes.methodAttr "post"] [
            div [Attributes.classAttr "form-group"] [
                Suave.Html.input [Attributes.classAttr "form-control btn btn-success"; Attributes.typeAttr "submit"; Attributes.valueAttr "Mark as completed"]
            ]
        ]

let private renderTodoItem (todo : Database.Todo) =
    let cssClass = Database.getTodoItemCssClass todo
    div [Attributes.classAttr ("col-md-3 todo-item todo-item-" + cssClass)] [
        Nodes.h4 todo.Title
        p [] [Text todo.Description]
        p [Attributes.classAttr "time"] [Text (sprintf "Time: %s" (todo.HappeningAt.ToString "dd. MMMM yyyy. HH:mm" ))]
        renderCompleteForm todo
    ]

let private renderTodoItems (todos : Database.Todo list) =  
    List.map renderTodoItem todos

let private mainContent todos = 
    div[] [
        div [Attributes.classAttr "row"] [
            div [Attributes.classAttr "col-md-3"] [
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