module Todos.Forms.UserDetailsForm

open Suave.Form

type UserDetailsModel = {
    FirstName : string
    LastName : string
    Email : string
}

let UserDetailsForm : Form<UserDetailsModel> =
    Form ([ TextProp ((fun x -> <@ x.FirstName @>), []);
        TextProp ((fun x -> <@ x.LastName @>), []);
        TextProp ((fun x -> <@ x.Email @>), [])
    ], [])