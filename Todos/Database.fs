module Todos.Database
open System
open Suave.Logging

type Todo = {
    Title: string
    Description: string
    HappeningAt: DateTime
    IsCompleted: bool
}

let getTodoItemCssClass todo =
    if todo.IsCompleted then
        "completed"
    else if todo.HappeningAt < DateTime.Now then
        "incompleted"
    else 
        "upcomming"

let Todos = [
    { 
        Title = "Pellentesque malesuada laoreet"; 
        Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam imperdiet mollis tellus nec lobortis. "; 
        HappeningAt = DateTime.Now.AddDays(2.0).AddHours(4.0);
        IsCompleted = true
    }
    { 
        Title = "Lorem ipsum dolor"; 
        Description = "Vivamus semper ipsum sit amet nulla venenatis laoreet."; 
        HappeningAt = DateTime.Now.AddDays(-4.0).AddHours(7.0).AddMinutes(8.0);
        IsCompleted = false
    }
    { 
        Title = "Sed eu congue"; 
        Description = "Nunc magna turpis"; 
        HappeningAt = DateTime.Now.AddHours(1.0).AddMinutes(8.0);
        IsCompleted = false
    }
    { 
        Title = "Morbi tristique lobortis nisl"; 
        Description = "Morbi tristique lobortis nisl, vitae lobortis odio vulputate eget. Donec ut arcu sed nisl dictum congue vitae eu ligula."; 
        HappeningAt = DateTime.Now.AddHours(1.2).AddMinutes(27.1);
        IsCompleted = false
    }
    { 
        Title = "Mauris et diam "; 
        Description = "Morbi tristique lobortis nisl, vitae lobortis odio vulputate eget. Donec ut arcu sed nisl dictum congue vitae eu ligula."; 
        HappeningAt = DateTime.Now.AddHours(-7.3).AddMinutes(2.1);
        IsCompleted = true
    }
]