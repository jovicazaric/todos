module Todos.Views.Shared.Attributes

let classAttr (value : string) = ("class", value)

let titleAttr (value : string) = ("title", value)

let typeAttr (value : string) = ("type", value) 

let placeholderAttr (value : string) = ("placeholder", value)

let nameAttr (value : string) = ("name", value)

let valueAttr (value : string) = ("value", value)

let checkedAttr = ("checked", "checked")

let methodAttr (value : string) = ("method", value)

let novalidateAttr = ("novalidate", "novalidate")

let actionAttr (value : string) = ("action", value)

let charsetAttr (value : string) = ("charset", value)

let contentAttr (value : string) = ("content", value)

let rowsAttr (value : int) = ("rows", value.ToString())