module Todos.Views.Home

open Suave.Html
open Todos.Views
open Todos.Views.Shared
open Todos

let private mainContent = 

    div [("id", "header"); ("class", "header")] [
        a "/" ["target", "blank"] [
            tag "h1" [] [Text "Todos"]
        ]
    ]

let private scripts = 
    Some (Nodes.scriptLink Paths.Assets.JS.home)

let content = 
    Layout.buildPage "Home" mainContent None scripts