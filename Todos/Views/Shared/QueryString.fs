module Todos.Views.Shared.QueryString

let buildParam name value = sprintf "%s=%s" name value

let addToUrl url name value = sprintf "%s?%s" url (buildParam name value)