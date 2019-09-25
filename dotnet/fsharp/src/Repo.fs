module Repo

open System
open Domain

let mutable private persistence: Map<string, InventoryItem> = Map.empty;

let Get (id: Guid) =
    persistence |> Map.tryFind (id.ToString())
    |> Option.defaultValue (id |> InventoryItem.Initialize)

let Upsert (inventoryItem: InventoryItem) =
    persistence <- persistence |> Map.add (inventoryItem.Id.ToString()) inventoryItem

let GetAll () =
    persistence |> Map.toSeq |> Seq.map snd