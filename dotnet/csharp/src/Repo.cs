using Projector.Domain;
using System;
using System.Collections.Generic;

namespace Projector
{
    public class Repo
    {
        private readonly Dictionary<string, InventoryItem> persistence;

        public Repo() => persistence = new Dictionary<string, InventoryItem>();

        public InventoryItem Get(Guid id) => persistence.GetValueOrDefault(id.ToString()) ?? InventoryItem.Init(id);

        public void Upsert(InventoryItem inventoryItem) => persistence[inventoryItem.Id.ToString()] = inventoryItem;

        public IEnumerable<InventoryItem> GetAll() => persistence.Values;
    }
}
