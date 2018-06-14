module Todos.Database

open System

type Todo = {
    Id: string
    mutable Title: string
    mutable Description: string
    mutable HappeningAt: DateTime
    mutable IsCompleted: bool
}

type User = {
    Id : string
    mutable FirstName : string
    mutable LastName : string
    mutable Email : string
    mutable Password : string
}

let mutable Users = [
    {
        Id = "A16247C4-E638-405F-8CB9-04DF26F3B3AA"
        FirstName = "Pero"
        LastName = "Peric"
        Email = "pero@peric.com"
        Password = "111"
    };
    {
        Id = "A16247C4-E638-405F-8CB9-04DF26F3B3AB"
        FirstName = "Ziko"
        LastName = "Zikic"
        Email = "ziko@zikic.com"
        Password = "111"
    }
]

let TryFindUserByEmailPassword email password : User option = List.tryFind (fun x -> x.Email = email && x.Password = password) Users

let TryFindUserByEmail email : User option = List.tryFind (fun x -> x.Email = email) Users

let TryFindUserById id : User option = List.tryFind (fun x -> x.Id = id) Users

let AddUser user =
    Users <- List.append Users [user]
    printfn "Total users %d" Users.Length
    Seq.iter (fun x -> printfn "%A" x) Users

let IsUserEmailValidOnUpdate user newEmail = 
    match user.Email = newEmail with 
        | false ->
            match TryFindUserByEmail newEmail with 
                | Some x -> x.Id = user.Id
                | None -> true
        | _ -> true

let mutable private todoItems = [
    { 
        Id = "A16247C4-E638-405F-8CB9-04DF26F3B3A1";
        Title = "Pellentesque malesuada laoreet"; 
        Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam imperdiet mollis tellus nec lobortis. "; 
        HappeningAt = DateTime.Now.AddDays(2.0).AddHours(4.0);
        IsCompleted = true
    };
    { 
        Id = "B4ED9212-C630-40B4-8B34-093FCA298739";
        Title = "Lorem ipsum dolor"; 
        Description = "Vivamus semper ipsum sit amet nulla venenatis laoreet."; 
        HappeningAt = DateTime.Now.AddDays(-4.0).AddHours(7.0).AddMinutes(8.0);
        IsCompleted = false
    };
    {
        Id = "FDB1884B-F15E-4C51-AD82-2DAE73BE3ACB"; 
        Title = "Sed eu congue"; 
        Description = "Nunc magna turpis"; 
        HappeningAt = DateTime.Now.AddHours(1.0).AddMinutes(8.0);
        IsCompleted = false
    };
    {
        Id = "BE9AB565-FBC5-43C2-BC5E-E8F814F1A179";
        Title = "Morbi tristique lobortis nisl"; 
        Description = "Morbi tristique lobortis nisl, vitae lobortis odio vulputate eget. Donec ut arcu sed nisl dictum congue vitae eu ligula."; 
        HappeningAt = DateTime.Now.AddHours(1.2).AddMinutes(27.1);
        IsCompleted = false
    };
    {
        Id = "86922177-6E1E-42BA-AF76-706EEAC7CECC";
        Title = "Mauris et diam "; 
        Description = "Morbi tristique lobortis nisl, vitae lobortis odio vulputate eget. Donec ut arcu sed nisl dictum congue vitae eu ligula."; 
        HappeningAt = DateTime.Now.AddHours(-7.3).AddMinutes(2.1);
        IsCompleted = true
    }
]

let GetTodos =
    List.sortByDescending (fun x -> x.HappeningAt) todoItems
let AddNewTodo todo = 
    todoItems <- List.append todoItems [todo]
    printfn "Total todos %d" todoItems.Length
    Seq.iter (fun x -> printfn "%A" x) todoItems
    
let TryFindTodo id : Todo option = List.tryFind (fun x -> x.Id = id) todoItems

let CompleteTodo id = 
    let todo : Todo = List.find (fun x -> x.Id = id) todoItems
    todo.IsCompleted <- true
    printfn "Mark as completed."
    Seq.iter (fun x -> printfn "%A" x) todoItems
    |> ignore


 
