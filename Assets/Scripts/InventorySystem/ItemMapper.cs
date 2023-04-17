using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For Inventory snapshot loading name to item reference
[CreateAssetMenu(menuName = "Item Mapper")]
public sealed class ItemMapper : ScriptableObject
{
    [SerializeField]
    private Item[] allItems;
    private Dictionary<string, Item> itemTable = new Dictionary<string, Item>();

    void Awake() {
        itemTable.Clear();
        foreach (Item item in allItems) {
            itemTable.Add(item.itemName, item);
        }
    }

    public Item GetItem(string itemName) {
        return itemTable[itemName];
    }
}
