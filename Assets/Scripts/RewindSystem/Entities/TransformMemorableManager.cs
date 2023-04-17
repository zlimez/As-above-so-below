using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saveable;
// TESTCODE
using KinematicCharacterController;
using Chronellium.Utils;

public class TransformMemorableManager : Singleton<TransformMemorableManager>
{
    // Should always be matching pairs starting from stack top with SnapshotManager.memorables
    // private Stack<SaveableTransform> memory = new Stack<SaveableTransform>();

    Dictionary<int, Stack<SaveableTransform>> memoryDictionary = new Dictionary<int, Stack<SaveableTransform>>();

    public Stack<SaveableTransform> GetMemory(int id)
    {
        Stack<SaveableTransform> memoryToGet = new Stack<SaveableTransform>();
        if (memoryDictionary.TryGetValue(id, out memoryToGet))
        {
            return memoryToGet;
        }
        Debug.LogWarning("memory cannot be loaded from TransformMemorableManager");
        return null;
    }

    public void PushToMemory(int id, SaveableTransform saveableTransform)
    {
        Stack<SaveableTransform> memoryToGet = new Stack<SaveableTransform>();
        if (memoryDictionary.TryGetValue(id, out memoryToGet))
        {
            // memory already exists
            memoryToGet.Push(saveableTransform);
            return;
        }

        // memory does not exists as it is not initialised
        memoryToGet = new Stack<SaveableTransform>();
        memoryToGet.Push(saveableTransform);
        memoryDictionary.Add(id, memoryToGet);
    }

    public void UpdateMemory(int id, Stack<SaveableTransform> memory)
    {
        memoryDictionary[id] = memory;
    }
}
