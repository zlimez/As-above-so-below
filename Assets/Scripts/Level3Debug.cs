using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Chronellium.EventSystem;
using Chronellium.SceneSystem;

public class Level3Debug : MonoBehaviour
{
    public Item pimpItem;
    public Item[] items;
    public Item drink;
    public TextMeshProUGUI podText;

    public static int pod = 3;

    public void givePimpItem()
    {
        Inventory.Instance.AddTo(true, pimpItem, 1);
        Inventory.Instance.AddTo(true, items[1], 1);
        Inventory.Instance.AddTo(true, items[0], 1);
        // Inventory.Instance.AddTo(false, pimpItem, 1);
    }

    public void giveDrinkItem()
    {
        Inventory.Instance.AddTo(false, drink, 1);
    }

    public void decreasePOD()
    {
        Debug.Log("down");
        pod--;
    }

    public void increasePOD()
    {
        Debug.Log("increase");
        pod++;
    }

    void Update()
    {
        // podText.text = pod.ToString();
    }

    public void SetEventMcHaleCalled()
    {
        EventLedger.Instance.RecordEvent(DynamicEvent.Convo_CommandFixDoor);
        SceneLoader.Instance.PrepLoadWithMaster(ChronelliumScene.BrothelLevel2);
    }
    public void SetEventGingerChosen()
    {
        EventLedger.Instance.RecordEvent(StaticEvent.LobbyEvents_OnGingerChosen);
    }
    public void SetEventLockBroken()
    {
        EventLedger.Instance.RecordEvent(StaticEvent.Level2Events_GingerLockBroken);
        EventManager.QueueEvent(StaticEvent.Level2Events_GingerLockBroken);
        EventManager.InvokeQueueEvents();
    }
}