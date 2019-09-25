using Projector.Domain;
using System;
using System.Collections.Generic;
using static Projector.Domain.InventoryEvent;

namespace Projector
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var repo = new Repo();

            var eventId = new Guid("439263a8-95be-499f-b0d5-926d972bce79");
            var events = new List<InventoryEvent> { new Created("Item 1"),
                new Stocked(10),
                new Activated(),
                new Sold(5)
            };

            foreach (var @event in events)
            {
                InventoryItem inventoryItem = repo.Get(eventId);
                InventoryItem newState = InventoryItem.Apply(@event, inventoryItem);
                repo.Upsert(newState);
            }

            foreach (var inventoryItem in repo.GetAll())
            {
                Console.WriteLine(inventoryItem);
            }
        }
    }
}
