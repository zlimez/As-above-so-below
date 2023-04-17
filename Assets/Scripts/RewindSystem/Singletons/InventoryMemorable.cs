using System.Collections;
using System;
using System.Collections.Generic;
using Saveable;
using Tuples;
using UnityEngine;


// TODO: not on the same level of abstraction as other memorable. perhaps it is better to further derive a collection memorable

public class InventoryMemorable : Memorable<Pair<SaveableCollection, SaveableCollection>>
{
    [SerializeField] private ItemMapper itemMapper;

    public Collection Convert(SaveableCollection saveableCollection, Func<Collection, Item, int, bool> addRule)
    {
        List<Countable<Item>> countedItems = new List<Countable<Item>>();
        foreach (SaveableCountableItem countedItem in saveableCollection.items)
        {
            countedItems.Add(new Countable<Item>(itemMapper.GetItem(countedItem.itemName), countedItem.count));
        }
        return new Collection(addRule, countedItems, countedItems.Count);
    }

    public override void TakeSnapshot(Snapshot snapshot)
    {
        memory.Push(new Pair<SaveableCollection, SaveableCollection>(new SaveableCollection(Inventory.Instance.NormalCollection), new SaveableCollection(Inventory.Instance.ScannedCollection)));
    }

    public override void LoadSnapshot(int offset)
    {
        if (!HasSufficientMemory(offset)) LoadActiveSnapshot();
        // Lazily assert normal collection and scanned collection memory have same length
        base.LoadSnapshot(offset);

        Collection normalCollection = Convert(memory.Peek().head, Inventory.Instance.normalAddRule);
        Collection scannedCollection = Convert(memory.Peek().tail, Inventory.Instance.scanAddRule);
        Debug.Log($"Snapshot inventory normal collection {normalCollection.Size()} scanned collection {scannedCollection.Size()}");
        // Convert saveable collection to pastInventory
        if (SnapshotManager.Instance.IsForInit)
        {
            Inventory.AssignNewInventory(new Inventory(normalCollection, scannedCollection));
        }
        else
        {
            Debug.Log("Merging current inventory with snapshot inventory");
            Inventory.AssignNewInventory(Inventory.Merge(new Inventory(normalCollection, scannedCollection), Inventory.Instance));
        }
    }

    protected override void LoadSnapshot(Snapshot snapshot)
    {
        // TODO: Get from db
    }
}
