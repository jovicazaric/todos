module Todos.Database

open System
open Suave.Logging
open System.Collections.Generic

type Todo = {
    Id: string
    mutable Title: string
    mutable Description: string
    mutable HappeningAt: DateTime
    mutable IsCompleted: bool
}

let mutable TodoItems = [
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

let AddNewTodo todo = 
    TodoItems <- List.append TodoItems [todo]
    printfn "Total todos %d" TodoItems.Length
    Seq.iter (fun x -> printfn "%A" x) TodoItems
    
let TryFindTodo id = List.tryFind (fun x -> x.Id = id) TodoItems

let MarkAsCompleted id = 
    let todo = List.find (fun x -> x.Id = id) TodoItems
    todo.IsCompleted <- true
    printfn "Mark as completed."
    Seq.iter (fun x -> printfn "%A" x) TodoItems
    |> ignore
