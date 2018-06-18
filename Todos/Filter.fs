module Todos.Filter

open Todos.Database
open Todos.Forms.TodoFilterForm

let private filterTodosByFrom fromDate todos =
    match fromDate with 
        | Some f -> List.where (fun x -> x.HappeningAt >= f) todos
        | None -> todos

let private filterTodosByTo toDate todos =
    match toDate with 
        | Some t -> List.where (fun x -> x.HappeningAt <= t) todos
        | None -> todos
 
let private filterTodosByCompleted completed todos =
    match completed with 
        | 1M -> List.where (fun x -> x.IsCompleted) todos
        | _ -> List.where (fun x -> not x.IsCompleted) todos
    

let filterTodos todos filter =
    todos 
    |> filterTodosByFrom filter.TFMFrom 
    |> filterTodosByTo filter.TFMTo
    |> filterTodosByCompleted filter.TFMIsCompleted