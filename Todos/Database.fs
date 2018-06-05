module Todos.Database

open System
open Suave.Logging
open System.Collections.Generic

type Todo = {
    Id: string
    Title: string
    Description: string
    HappeningAt: DateTime
    mutable IsCompleted: bool
}

let todoItems = new List<Todo>();

todoItems.Add(
    { 
        Id = "A16247C4-E638-405F-8CB9-04DF26F3B3A1";
        Title = "Pellentesque malesuada laoreet"; 
        Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam imperdiet mollis tellus nec lobortis. "; 
        HappeningAt = DateTime.Now.AddDays(2.0).AddHours(4.0);
        IsCompleted = true
    });

todoItems.Add(
    { 
        Id = "B4ED9212-C630-40B4-8B34-093FCA298739";
        Title = "Lorem ipsum dolor"; 
        Description = "Vivamus semper ipsum sit amet nulla venenatis laoreet."; 
        HappeningAt = DateTime.Now.AddDays(-4.0).AddHours(7.0).AddMinutes(8.0);
        IsCompleted = false
    });

todoItems.Add(
    {
        Id = "FDB1884B-F15E-4C51-AD82-2DAE73BE3ACB"; 
        Title = "Sed eu congue"; 
        Description = "Nunc magna turpis"; 
        HappeningAt = DateTime.Now.AddHours(1.0).AddMinutes(8.0);
        IsCompleted = false
    });

todoItems.Add(
    {
        Id = "BE9AB565-FBC5-43C2-BC5E-E8F814F1A179";
        Title = "Morbi tristique lobortis nisl"; 
        Description = "Morbi tristique lobortis nisl, vitae lobortis odio vulputate eget. Donec ut arcu sed nisl dictum congue vitae eu ligula."; 
        HappeningAt = DateTime.Now.AddHours(1.2).AddMinutes(27.1);
        IsCompleted = false
    });

todoItems.Add(
    {
        Id = "86922177-6E1E-42BA-AF76-706EEAC7CECC";
        Title = "Mauris et diam "; 
        Description = "Morbi tristique lobortis nisl, vitae lobortis odio vulputate eget. Donec ut arcu sed nisl dictum congue vitae eu ligula."; 
        HappeningAt = DateTime.Now.AddHours(-7.3).AddMinutes(2.1);
        IsCompleted = true
    });

let TryFindTodo id = List.tryFind (fun x -> x.Id = id) (List.ofSeq todoItems)

let MarkAsCompleted id = 
    let todo = todoItems.Find (fun x -> x.Id = id)
    todo.IsCompleted <- true
    // let completedTodo = {todo with IsCompleted = true}
    // todoItems.Remove todo |> ignore
    // todoItems.Add completedTodo
    printfn "Mark as completed."
    printfn "%A" todoItems
    |> ignore
