module Domain

open System

type InventoryItem =
    { Id: Guid
      Name: string
      Count: int
      Active: bool
      SoldOut: bool }
    with static member Initialize id = 
            { Id = id
              Name = null
              Count = 0
              Active = false
              SoldOut = true }

type InventoryEvent =
    | Created of name: string
    | Stocked of count: int
    | Sold of count: int
    | Deactivated
    | Activated

let Apply (state: InventoryItem) event =
    match event with
    | Created name
        -> { state with Name = name; }
    | Stocked count
        -> { state with Count = state.Count + count; } // SoldOut = false }
    | Sold count when state.Count - count > 0
        -> { state with Count = state.Count - count; }
    // TODO: Implement SoldOut
    | Deactivated
        -> { state with Active = false; }
    | Activated
        -> { state with Active = true; }
    | _ -> state