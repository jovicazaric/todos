module Todos.Forms.UserDetailsForm

open Suave.Form

type UserDetailsModel = {
    UDMFirstName : string
    UDMLastName : string
    UDMEmail : string
}
let private firstNameNotEmpty =
    (fun x -> x.UDMFirstName <> ""), "First name is requred"

let private lastNameNotEmpty =
    (fun x -> x.UDMLastName <> ""), "Last name is required"

let private emailNotEmpty = 
    (fun x -> x.UDMEmail <> ""), "Email is required"

let UserDetailsForm : Form<UserDetailsModel> =
    Form ([ TextProp ((fun x -> <@ x.UDMFirstName @>), [])
            TextProp ((fun x -> <@ x.UDMLastName @>), [])
            TextProp ((fun x -> <@ x.UDMEmail @>), [])
    ], [firstNameNotEmpty; lastNameNotEmpty; emailNotEmpty])