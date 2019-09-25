namespace Domain

open System
open Xunit

module EventTests =

    let Given state:InventoryItem = state
    let When event state = Apply state event
    let Then (actualState: InventoryItem) (expectedState: InventoryItem) = Assert.Equal(expectedState, actualState)

    let rdm = Random();

    [<Fact>]
    let ``Given initial inventory item, When Created event, Then Created inventory item`` () =
        let id = Guid.NewGuid()
        let initialState = id |> InventoryItem.Initialize
        let name = "Test"

        Given initialState
        |> When (Created name)
        |> Then { initialState with Name = name }

    [<Fact>]
    let ``Given inventory item, When Stocked event, Then inventory count incremented and not sold out`` () =
        let initialState = { Id = Guid.NewGuid(); Name = "Test"; Count = 0; Active = false; SoldOut = true; }
        let stockedCount = rdm.Next()

        Given initialState
        |> When (Stocked stockedCount)
        |> Then { initialState with Count = stockedCount; SoldOut = false }

    [<Fact>]
    let ``Given inventory item, When Sold event, Then inventory count decremented`` () =
        let initialState = { Id = Guid.NewGuid(); Name = "Test"; Count = 10; Active = false; SoldOut = false; }
        let soldCount = rdm.Next(10)

        Given initialState
        |> When (Sold soldCount)
        |> Then { initialState with Count = (initialState.Count - soldCount) }

    [<Fact>]
    let ``Given inventory item, When Deactivated event, Then inventory item inactive`` () =
        let initialState = { Id = Guid.NewGuid(); Name = "Test"; Count = 0; Active = true; SoldOut = true; }

        Given initialState
        |> When (Deactivated)
        |> Then { initialState with Active = false }

    [<Fact>]
    let ``Given inventory item, When Activated event, Then inventory item active`` () =
        let initialState = { Id = Guid.NewGuid(); Name = "Test"; Count = 0; Active = false; SoldOut = true; }

        Given initialState
        |> When (Activated)
        |> Then { initialState with Active = true }

    [<Fact>]
    let ``Given inventory item, When Sold more than count event, Then inventory item sold out`` () =
        let initialState = { Id = Guid.NewGuid(); Name = "Test"; Count = 9; Active = true; SoldOut = false; }

        Given initialState
        |> When (Sold 10)
        |> Then { initialState with Count = 0; SoldOut = true}
