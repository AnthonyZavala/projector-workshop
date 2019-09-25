using Projector.Domain;
using System;
using Xunit;

namespace Domain
{
    public class EventTests
    {
        private static InventoryItem Given(InventoryItem state) => state;
        private static InventoryItem When(InventoryItem state, InventoryEvent @event) => InventoryItem.Apply(@event, state);
        private static void Then(InventoryItem expectedState, InventoryItem actualState) => Assert.Equal(expectedState, actualState);

        private static Random rdm => new Random();

        [Fact]
        public void Given_initial_inventory_item_When_Created_event_Then_Created_inventory_item()
        {
            var id = Guid.NewGuid();
            var initialState = InventoryItem.Init(id);
            string name = "Test";

            InventoryItem given = Given(initialState);
            InventoryItem when = When(given, new InventoryEvent.Created(name));
            Then(new InventoryItem(id, name, 0, false, true), when);
        }

        [Fact]
        public void Given_inventory_item_When_Stocked_event_Then_inventory_count_incremented_and_not_sold_out()
        {
            var initialState = new InventoryItem(Guid.NewGuid(), "Test", 0, false, true);
            int stockedCount = rdm.Next();

            InventoryItem given = Given(initialState);
            InventoryItem when = When(given, new InventoryEvent.Stocked(stockedCount));
            Then(new InventoryItem(initialState.Id, initialState.Name, stockedCount, false, false), when);
        }

        [Fact]
        public void Given_inventory_item_When_Sold_event_Then_inventory_count_decremented()
        {
            var initialState = new InventoryItem(Guid.NewGuid(), "Test", 10, false, false);
            int soldCount = rdm.Next(10);

            InventoryItem given = Given(initialState);
            InventoryItem when = When(given, new InventoryEvent.Sold(soldCount));
            Then(new InventoryItem(initialState.Id, initialState.Name, initialState.Count - soldCount, false, false), when);
        }

        [Fact]
        public void Given_inventory_item_When_Deactivated_event_Then_inventory_item_inactive()
        {
            var initialState = new InventoryItem(Guid.NewGuid(), "Test", 0, true, true);

            InventoryItem given = Given(initialState);
            InventoryItem when = When(given, new InventoryEvent.Deactivated());
            Then(new InventoryItem(initialState.Id, initialState.Name, initialState.Count, false, true), when);
        }

        [Fact]
        public void Given_inventory_item_When_Activated_event_Then_inventory_item_active()
        {
            var initialState = new InventoryItem(Guid.NewGuid(), "Test", 0, false, true);

            InventoryItem given = Given(initialState);
            InventoryItem when = When(given, new InventoryEvent.Activated());
            Then(new InventoryItem(initialState.Id, initialState.Name, initialState.Count, true, true), when);
        }

        [Fact]
        public void Given_inventory_item_When_Sold_more_than_count_event_Then_inventory_item_sold_out()
        {
            var initialState = new InventoryItem(Guid.NewGuid(), "Test", 9, true, false);

            InventoryItem given = Given(initialState);
            InventoryItem when = When(given, new InventoryEvent.Sold(10));
            Then(new InventoryItem(initialState.Id, initialState.Name, 0, true, true), when);
        }
    }
}