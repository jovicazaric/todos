module Todos.Forms.TodoFilterForm

open Suave.Form
open System

type TodoFilterModel = {
    TFMFrom : DateTime option
    TFMTo : DateTime option
    TFMIsCompleted : Decimal option
}

let TodoFilterForm : Form<TodoFilterModel> =
    Form ([ DateTimeProp ((fun x -> <@ x.TFMFrom.Value @>), [])
            DateTimeProp ((fun x -> <@ x.TFMTo.Value @>), [])
            DecimalProp ((fun x -> <@ x.TFMIsCompleted.Value @>), [])
    ], [])