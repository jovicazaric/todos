module Todos.Views.Shared.Nodes


open Todos.Views.Shared.Attributes
open Suave.Html

let cssLink href = link ["href", href; "rel", "stylesheet"; "type", "text/css"]

let scriptLink src = script ["src", src; "type", "text/javascript"] []

let bold text = tag "b" [] [Text text]

let label text =  tag "label" [] [(bold text)]

let h2 text = tag "h2" [] [Text text]

let h3 text = tag "h3" [] [Text text]

let h4 text = tag "h4" [] [Text text]

let textP text = p [] [Text text]

let ul attributes nodes = tag "ul" attributes nodes

let li attributes nodes = tag "li" attributes nodes

let errorSpan text = span [classAttr "error"] [Text text] 

let form attributes content = tag "form" attributes content