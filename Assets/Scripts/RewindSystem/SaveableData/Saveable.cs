using System;
using System.Collections.Generic;
using UnityEngine;
using Chronellium.Utils;
using Chronellium.EventSystem;

// Should be consist of structs by value
namespace Saveable
{
    // Transform in Unity Engine is by reference
    public struct SaveableTransform
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        public SaveableTransform(Transform transform)
        {
            position = transform.position;
            rotation = transform.rotation;
            scale = transform.localScale;
        }

        // Used only for scientist
        public SaveableTransform(Transform transform, bool isFlipX)
        {
            position = transform.position;
            rotation = transform.rotation;
            scale = isFlipX ? VectorUtils.horizontalFlipped : Vector3.one;
        }
    }

    public struct SaveableCollection
    {
        public SaveableCountableItem[] items;

        public SaveableCollection(Collection collection)
        {
            items = new SaveableCountableItem[collection.items.Count];
            for (int i = 0; i < collection.items.Count; i++)
            {
                items[i] = new SaveableCountableItem(collection.items[i].Data.itemName, collection.items[i].Count);
            }
        }

        public SaveableCollection(Dictionary<GameEvent, int> eventTable)
        {
            items = new SaveableCountableItem[eventTable.Count];
            int i = 0;
            foreach (KeyValuePair<GameEvent, int> eventRep in eventTable)
            {
                StaticEvent parsedEvent = Parser.getStaticEventFromText(eventRep.Key.EventName);

                if (CommonEventCollection.UtilityEvents.Contains(parsedEvent)) continue;
                if (CoreEventCollection.UtilityEvents.Contains(parsedEvent)) continue;

                items[i] = new SaveableCountableItem(eventRep.Key.EventName, eventRep.Value);
                i++;
            }
        }
    }

    public struct SaveableCountableItem
    {
        public int count;
        public string itemName;

        public SaveableCountableItem(string itemName, int count)
        {
            this.count = count;
            this.itemName = itemName;
        }
    }
}