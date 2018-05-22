module Todos.Views.Layout

open Suave.Html
open Todos.Views.Shared
open Todos
open Todos.Authentication

type PageData = {
    Title : string
    Content : Node
}

let private composeTitle pageTitle =
    title [] (sprintf "%s | Todos" pageTitle)

let navigationActions user  = 
    match user with
        | Some user ->
            div [Attributes.classAttr "collapse navbar-collapse"] [
                tag "ul" [Attributes.classAttr "navbar-nav ml-auto"] [
                    tag "li" [Attributes.classAttr "navbar-text"] [
                        Text (sprintf "Hi, %s" user.Email)
                    ]
                    tag "li" [Attributes.classAttr "nav-item"] [
                        a "#" [Attributes.classAttr "nav-link"] [Text "Account"]
                    ]
                    tag "li" [Attributes.classAttr "nav-item"] [
                        a Paths.Pages.Logout [Attributes.classAttr "nav-link"] [Text "Logout"]
                    ]
                ]
            ]
        | None -> Text ""                   

let private navigation user = 
    tag "nav" [Attributes.classAttr "navbar navbar-expand-lg navbar-dark bg-dark fixed-top"] [
        div [Attributes.classAttr "container"] [
            a "/" [Attributes.classAttr "navbar-brand"] [Text "Todos"]
            navigationActions user
        ]
    ]

let buildPage pageData user =
    html [] [
        head [] [
            meta [Attributes.charsetAttr "utf-8"]
            meta [Attributes.nameAttr "viewport"; Attributes.contentAttr "width=device-width, initial-scale=1, shrink-to-fit=no"]
            composeTitle pageData.Title

            Nodes.cssLink Paths.Assets.CSS.Bootstrap
            Nodes.cssLink Paths.Assets.CSS.Style
        ]
        body [] [
            navigation user
            div [Attributes.classAttr "container main-content"] [
                pageData.Content
            ]
            
            tag "footer" [Attributes.classAttr "py-3 bg-dark"] [
                div [Attributes.classAttr "container"] [
                    p [Attributes.classAttr "m-0 text-right text-white"] [Text "Copyright © Todos WebApp By Jovica Zaric 2018"]
                ]
            ]

            Nodes.scriptLink Paths.Assets.JS.Lib.JQuery
            Nodes.scriptLink Paths.Assets.JS.Main
        ]
    ]
    |> htmlToString



