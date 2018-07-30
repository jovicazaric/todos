module Todos.Database

open System

type Todo = {
    Id: string
    Title: string
    Description: string
    HappeningAt: DateTime
    IsCompleted: bool
    UserId : string
}

type User = {
    Id : string
    FirstName : string
    LastName : string
    Email : string
    Password : string
}

let mutable private users = [
    {
        Id = "A16247C4-E638-405F-8CB9-04DF26F3B3AA"
        FirstName = "Pero"
        LastName = "Peric"
        Email = "pero@peric.com"
        Password = "111"
    }
    {
        Id = "A16247C4-E638-405F-8CB9-04DF26F3B3AB"
        FirstName = "Ziko"
        LastName = "Zikic"
        Email = "ziko@zikic.com"
        Password = "111"
    }
]

let tryFindUserByEmailPassword email password : User option = List.tryFind (fun x -> x.Email = email && x.Password = password) users

let tryFindUserByEmail email : User option = List.tryFind (fun x -> x.Email = email) users

let tryFindUserById id : User option = List.tryFind (fun x -> x.Id = id) users

let addUser user =
    users <- List.append users [user]

let updateUser user =
    let otherUsers = List.filter (fun x -> x.Id <> user.Id) users
    users <- List.append otherUsers [user]

let isUserEmailValidOnUpdate user newEmail = 
    match user.Email = newEmail with 
        | false ->
            match tryFindUserByEmail newEmail with 
                | Some x -> x.Id = user.Id
                | None -> true
        | _ -> true

let mutable private todoItems = [
    { 
        Id = "A16247C4-E638-405F-8CB9-04DF26F3B3A1"
        Title = "Pellentesque malesuada laoreet"
        Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam imperdiet mollis tellus nec lobortis."
        HappeningAt = DateTime.Now.AddDays(2.0).AddHours(4.0)
        IsCompleted = true
        UserId = "A16247C4-E638-405F-8CB9-04DF26F3B3AA"
    }
    { 
        Id = "A31E6EE6-BC5E-4BAC-8DCA-BDC5F1E7529D"
        Title = "Nulla maximus porttitor"
        Description = "LPhasellus fermentum sem mattis risus egestas, non suscipit eros faucibus."
        HappeningAt = DateTime.Now.AddDays(-7.0).AddHours(-1.0).AddMinutes(47.0)
        IsCompleted = false
        UserId = "A16247C4-E638-405F-8CB9-04DF26F3B3AA"
    }
    {
        Id = "B4ED9212-C630-40B4-8B34-093FCA298739"
        Title = "Lorem ipsum dolor"
        Description = "Vivamus semper ipsum sit amet nulla venenatis laoreet."
        HappeningAt = DateTime.Now.AddDays(-4.0).AddHours(7.0).AddMinutes(8.0)
        IsCompleted = false
        UserId = "A16247C4-E638-405F-8CB9-04DF26F3B3AB"
    }
    {
        Id = "FDB1884B-F15E-4C51-AD82-2DAE73BE3ACB"
        Title = "Sed eu congue"
        Description = "Nunc magna turpis"
        HappeningAt = DateTime.Now.AddHours(1.0).AddMinutes(8.0)
        IsCompleted = false
        UserId = "A16247C4-E638-405F-8CB9-04DF26F3B3AA"
    }
    {
        Id = "BE9AB565-FBC5-43C2-BC5E-E8F814F1A179"
        Title = "Morbi tristique lobortis nisl"
        Description = "Morbi tristique lobortis nisl, vitae lobortis odio vulputate eget. Donec ut arcu sed nisl dictum congue vitae eu ligula."
        HappeningAt = DateTime.Now.AddHours(1.2).AddMinutes(27.1)
        IsCompleted = false
        UserId = "A16247C4-E638-405F-8CB9-04DF26F3B3AA"
    }
    {
        Id = "86922177-6E1E-42BA-AF76-706EEAC7CECC"
        Title = "Mauris et diam "
        Description = "Morbi tristique lobortis nisl, vitae lobortis odio vulputate eget. Donec ut arcu sed nisl dictum congue vitae eu ligula." 
        HappeningAt = DateTime.Now.AddHours(-7.3).AddMinutes(2.1)
        IsCompleted = true
        UserId = "A16247C4-E638-405F-8CB9-04DF26F3B3AA"
    }
]

let getTodos userId =
    List.filter (fun x -> x.UserId = userId) todoItems
    |> List.sortByDescending (fun x -> x.HappeningAt)

let tryFindTodo id : Todo option = List.tryFind (fun x -> x.Id = id) todoItems

let addTodo todo = 
    todoItems <- List.append todoItems [todo]

let updateTodo (todo : Todo)=
    let otherTodos : Todo list = List.filter (fun x -> x.Id <> todo.Id) todoItems
    todoItems <- List.append otherTodos [todo]

let completeTodo id = 
    let todo : Todo = List.find (fun x -> x.Id = id) todoItems
    let updated = { 
        todo with 
            IsCompleted = true
    }
    updateTodo updated