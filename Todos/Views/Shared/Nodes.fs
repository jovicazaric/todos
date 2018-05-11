module Todos.Views.Shared.Nodes

open Suave.Html

let cssLink href = link ["href", href; "rel", "stylesheet"; "type", "text/css"]

let scriptLink src = script ["src", src; "type", "text/javascript"] []