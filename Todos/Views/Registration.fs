module Todos.Views.Registration

open Suave.Html
open Suave.Form
open Todos.Views.Shared.Attributes
open Todos.Views.Shared
open Todos.Views.Layout
open Todos

let formInput = Suave.Form.input
let suaveInput = Suave.Html.input

type Registration = {
    FirstName : string
    LastName : string
    Email : string
    Password : Password
    ConfirmPassword : Password
}

let checkPasswords  =
    (fun x -> x.Password = x.ConfirmPassword), "Confirm password must match password"

let RegistrationForm : Form<Registration> =
    Form ([ TextProp ((fun x -> <@ x.FirstName @>), [maxLength 50])
            TextProp ((fun x -> <@ x.LastName @>), [maxLength 50;])
            TextProp ((fun x -> <@ x.Email @>), [])
            PasswordProp ((fun x -> <@ x.Password @>), [])
            PasswordProp ((fun x -> <@ x.ConfirmPassword @>), [])
    ], [])

let private mainContent (form : Form<Registration>) = 
    div [] [
        div [classAttr "row"] [
            div [classAttr "offset-md-4 col-md-4"] [
                Nodes.h2 "Registration"
                Nodes.textP "Please enter your personal details to register"
            ]
        ]
        div [classAttr "row"] [
            div [classAttr "offset-md-4 col-md-4"] [
                tag "form" [methodAttr "post"] [
                    div [classAttr "form-group"] [
                        Nodes.label "First name *"
                        formInput (fun x -> <@ x.FirstName @>) [classAttr "form-control"] form
                    ]
                    div [classAttr "form-group"] [
                        Nodes.label "Last name *"
                        formInput (fun x -> <@ x.LastName @>) [classAttr "form-control"] form
                    ]
                    div [classAttr "form-group"] [
                        Nodes.label "Email *"
                        formInput (fun x -> <@ x.Email @>) [classAttr "form-control";] form
                    ]
                    div [classAttr "form-group"] [
                        Nodes.label "Password *"
                        formInput (fun x -> <@ x.Password @>) [classAttr "form-control"] form
                    ]
                    div [classAttr "form-group"] [
                        Nodes.label "Confirm password *"
                        formInput (fun x -> <@ x.ConfirmPassword @>) [classAttr "form-control"] form
                    ]
                    div [classAttr "form-group"] [
                        suaveInput [classAttr "form-control btn btn-success"; typeAttr "submit"; valueAttr "Register"]
                    ]
                ]
                p [] [
                    Text "If you have already registered, please log in."
                    br []
                    a Paths.Pages.login [] [
                        Text "Go to login page"
                    ]
                ]
            ]
        ]  
    ]
    
let content form =
   buildPage "Registration" (mainContent form) None None