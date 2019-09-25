using System;

namespace Projector.Domain
{
    public class InventoryItem
    {
        public InventoryItem(Guid id, string name, int count, bool active, bool soldOut)
        {
            Id = id;
            Name = name;
            Count = count;
            Active = active;
            SoldOut = soldOut;
        }

        public Guid Id { get; }
        public string Name { get; }
        public int Count { get; }
        public bool Active { get; }
        public bool SoldOut { get; }

        public override string ToString() => $"{{Id = {Id}; Name = {Name}; Count = {Count}; Active = {Active}; SoldOut = {SoldOut}}}";
        public override bool Equals(object obj) => obj is InventoryItem item && Id.Equals(item.Id) && Name == item.Name && Count == item.Count && Active == item.Active && SoldOut == item.SoldOut;
        public override int GetHashCode() => HashCode.Combine(Id, Name, Count, Active, SoldOut);

        public static InventoryItem Init(Guid id) => new InventoryItem(id, null, 0, false, true);

        public static InventoryItem Apply(InventoryEvent @event, InventoryItem state)
        {
            switch (@event)
            {
                case InventoryEvent.Created created:
                    return new InventoryItem(state.Id, created.Name, state.Count, state.Active, state.SoldOut);
                case InventoryEvent.Stocked stocked:
                    return new InventoryItem(state.Id, state.Name, state.Count + stocked.Count, state.Active, state.SoldOut); // TODO: Set SoldOut to false
                case InventoryEvent.Sold sold when state.Count - sold.Count > 0:
                    return new InventoryItem(state.Id, state.Name, state.Count - sold.Count, state.Active, state.SoldOut);
                // TODO: Implement SoldOut
                case InventoryEvent.Deactivated _:
                    return new InventoryItem(state.Id, state.Name, state.Count, false, state.SoldOut);
                case InventoryEvent.Activated _:
                    return new InventoryItem(state.Id, state.Name, state.Count, true, state.SoldOut);
                default:
                    return state;
            }
        }
    }

    public abstract class InventoryEvent
    {
        public class Created : InventoryEvent
        {
            public Created(string name) => Name = name;

            public string Name { get; }

            public override bool Equals(object obj) => obj is Created created && Name == created.Name;
            public override int GetHashCode() => HashCode.Combine(Name);
        }

        public class Stocked : InventoryEvent
        {
            public Stocked(int count) => Count = count;

            public int Count { get; }

            public override bool Equals(object obj) => obj is Stocked stocked && Count == stocked.Count;
            public override int GetHashCode() => HashCode.Combine(Count);
        }

        public class Sold : InventoryEvent
        {
            public Sold(int count) => Count = count;

            public int Count { get; }

            public override bool Equals(object obj) => obj is Sold sold && Count == sold.Count;
            public override int GetHashCode() => HashCode.Combine(Count);
        }

        public class Activated : InventoryEvent
        {
            public Activated() { }
        }

        public class Deactivated : InventoryEvent
        {
            public Deactivated() { }
        }
    }
}
