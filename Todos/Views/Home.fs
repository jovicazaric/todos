module Todos.Views.Home

open Suave.Html
open Todos.Views.Layout

let private mainContent = 

    div [("id", "header"); ("class", "header")] [
        a "/" ["target", "blank"] [
            tag "h1" [] [Text "Todos"]
        ]
    ]

let content =
    { Title = "Home"; Content = mainContent }