module Todos.Views.UserDetails

open Suave.Html
open Todos.Forms.UserDetailsForm
open Todos.Views.Layout
open Todos.Views.Shared
open Todos.Database
open Todos

let private insertErrorMessage = function
    | Some x -> span [Attributes.classAttr "error"] [Text x] 
    | _ -> Text ""

let private mainContent (user : User) errorMessage  = 
    div [] [
        div [Attributes.classAttr "row"] [
            div [Attributes.classAttr "offset-lg-4 col-lg-5"] [
                Nodes.h2 "User details"
            ]
        ]
        div [Attributes.classAttr "row"] [
            div [Attributes.classAttr "offset-lg-4 col-lg-5"] [
                Nodes.form [Attributes.methodAttr "post"; Attributes.actionAttr Paths.Pages.UserDetails] [
                    div [Attributes.classAttr "form-group"] [
                        Nodes.label "First name *"
                        Suave.Form.input (fun x -> <@ x.UDMFirstName @>) [Attributes.classAttr "form-control"; Attributes.valueAttr user.FirstName] UserDetailsForm
                    ]
                    div [Attributes.classAttr "form-group"] [
                        Nodes.label "Last name *"
                        Suave.Form.input (fun x -> <@ x.UDMLastName @>) [Attributes.classAttr "form-control"; Attributes.valueAttr user.LastName] UserDetailsForm
                    ]
                    div [Attributes.classAttr "form-group"] [
                        Nodes.label "Email *"
                        Suave.Form.input (fun x -> <@ x.UDMEmail @>) [Attributes.classAttr "form-control"; Attributes.valueAttr user.Email] UserDetailsForm
                    ]
                    insertErrorMessage errorMessage
                    div [Attributes.classAttr "form-group"] [
                        Suave.Html.input [Attributes.classAttr "form-control btn btn-success"; Attributes.typeAttr "submit"; Attributes.valueAttr "Submit changes"]
                    ]
                    p [] [
                        Text "After successfully updating your details, you will be requested to login again in order new changes to be applied."
                    ]
                ]
            ]
        ]
    ]
    
let content user errorMessage= 
    { Title = "User details"; Content = mainContent user errorMessage }