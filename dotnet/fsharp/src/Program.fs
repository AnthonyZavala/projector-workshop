// Learn more about F# at http://fsharp.org

open System
open Domain

let UpsertInventoryItem id event =
    let inventoryItem = id |> Repo.Get
    let newState = event |> Domain.Apply inventoryItem
    newState |> Repo.Upsert

[<EntryPoint>]
let main argv =
    let eventId = Guid.Parse("439263a8-95be-499f-b0d5-926d972bce79")
    let events = [ Created "Item 1";
                    Stocked 10;
                    Activated;
                    Sold 5 ]

    events |> List.iter (UpsertInventoryItem eventId)
    
    Repo.GetAll () |> printfn "%A"

    0 // return an integer exit code
