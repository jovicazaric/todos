module Todos.Views.Layout

open Suave.Html
open Todos.Views.Shared
open Todos.Views.Shared.Attributes
open Todos
open System


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

let private navigation = 
    tag "nav" [classAttr "navbar navbar-expand-lg navbar-dark bg-dark fixed-top"] [
        div [classAttr "container"] [
            a "/" [classAttr "navbar-brand"] [Text "Todos"]
            div [classAttr "collapse navbar-collapse"] [
                tag "ul" [classAttr "navbar-nav ml-auto"] [
                    tag "li" [classAttr "nav-item"] [
                        a "#" [classAttr "nav-link"] [Text "Logout"]
                    ]
                    tag "li" [classAttr "nav-item"] [
                        a "#" [classAttr "nav-link"; titleAttr "Account settings"] [Text "Hi j.zaric"]
                    ]
                ]
            ]
        ]
    ]

let private footer = 
    tag "footer" [classAttr "py-3 bg-dark"] [
        div [classAttr "container"] [
            p [classAttr "m-0 text-right text-white"] [Text "Copyright © Todos WebApp By Jovica Zaric 2018"]
        ]
    ]
let buildPage pageTitle mainContent additionalHead additionalScripts =
    html [] [
        head [] [
            meta ["charset", "utf-8"]
            meta ["name", "viewport"; "content", "width=device-width, initial-scale=1, shrink-to-fit=no"]
            composeTitle pageTitle
            insertAdditionalHead additionalHead

            Nodes.cssLink Paths.Assets.CSS.bootstrap
            Nodes.cssLink Paths.Assets.CSS.style
        ]
        body [] [
            navigation
            mainContent
            footer

            Nodes.scriptLink Paths.Assets.JS.Lib.jQuery
            Nodes.scriptLink Paths.Assets.JS.main
            insertAdditionalScripts additionalScripts
        ]
    ]
    |> htmlToString



