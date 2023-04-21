using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronellium.EventSystem;

public class HoldObjectTest : Interactable
{
    private bool isPickedUp;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.InvokeEvent(CommonEventCollection.ObjectPickedUp);
    }

    // Update is called once per frame
    void Update()
    {
        TryInteract();
    }

    public override void Interact() 
    {
        if (!isPickedUp)
        {
            EventManager.InvokeEvent(CommonEventCollection.ObjectPickedUp);
            isPickedUp = true;
        }
        else 
        {
            EventManager.InvokeEvent(CommonEventCollection.ObjectPutDown);    
        }
    }
}
