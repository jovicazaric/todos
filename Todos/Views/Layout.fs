module Todos.Views.Layout

open Suave.Html
open Todos.Views.Shared
open Todos


let private composeTitle pageTitle =
    title [] (sprintf "%s | Todos" pageTitle)

let private insertAdditionalHead aHead =
    match aHead with 
        | Some x -> x
        | None -> Text ""

let private insertAdditionalScripts aScripts =
    match aScripts with 
        | Some x -> x
        | None -> Text ""


let buildPage pageTitle mainContent additionalHead additionalScripts =
    html [] [
        head [] [
            composeTitle pageTitle
            insertAdditionalHead additionalHead

            Nodes.cssLink Paths.Assets.CSS.style
        ]
        body [] [
            mainContent
            Nodes.scriptLink Paths.Assets.JS.Lib.jQuery
            Nodes.scriptLink Paths.Assets.JS.main
            insertAdditionalScripts additionalScripts
        ]
    ]
    |> htmlToString



