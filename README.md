# projector-workshop
A workshop designed around the Projector component type.

## What is a Projector?

A projector has an ingress of events and persists state externally. 

| Input/Output | Commands                    | Events                        | External       |
|--------------|-----------------------------|-------------------------------|----------------|
| Commands     | Controller (Translator/ACL) | Aggregator (Aggregate)        | Actuator       |
| Events       | Process Manager             | Event System (Translator/ACL) | **Projector** (Projection)     |
| External     | Actor                       | Sensor                        | Plant (System) |

## Prerequisites
- .NET
    - [Visual Studio Code](https://code.visualstudio.com/download) (or anything that can be used for dotnet development)
    - [.NET Core SDK 2.2 or later](https://www.microsoft.com/net/download/all)
    - [C# for Visual Studio Code version 1.17.1 or later](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp)
    - [F#: Ionide-fsharp](https://marketplace.visualstudio.com/items?itemName=Ionide.Ionide-fsharp)

## Workshop
In this workshop we will be working with the an inventory item example. Assume this example is for a store and a user can be create, stock, sell, activate, and deactivate inventory items.

### State
InventoryItem is a container that holds state.
```yaml
inventoryItem:
    id: e85c1ec3-f077-4625-adf2-5a593a293988
    name: "Item 1"
    count: 212
    active: true
    soldOut: false
```

### Events
InventoryEvent defines the events that can happen to an InventoryItem state.
```yaml
- Created:
    InventoryId: e85c1ec3-f077-4625-adf2-5a593a293988
    Name: "Item 1"

- Stocked:
    Count: 123

- Sold:
    Count: 43

- Activated

- Deactivated
```
