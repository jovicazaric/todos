module Todos.Views.Shared.Nodes


open Todos.Views.Shared.Attributes
open Suave.Html

let cssLink href = link ["href", href; "rel", "stylesheet"; "type", "text/css"]

let scriptLink src = script ["src", src; "type", "text/javascript"] []

let bold text = tag "b" [] [Text text]

let label text =  tag "label" [] [(bold text)]

let h2 text = tag "h2" [] [Text text]

let textP text = p [] [Text text]

let errorSpan text = span [classAttr "error"] [Text text] 