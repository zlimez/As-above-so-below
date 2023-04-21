namespace Chronellium.EventSystem
{
    // WARNING: Do not change the order of the enum values or their assigned integer values
    // after development has started, as it may cause issues with saved data and other parts
    // of the code that rely on these values. If you need to add new values, append them to
    // the end of the list and assign them new unique integer values.
    public enum StaticEvent
    {
        NoEvent = 0,
        Core_SwitchToOtherWorld = 1,
        Core_SwitchToRealWorld = 2,
        Core_GameManagerReady = 3,
        Core_TransitionWithMaster = 5,
        Core_TransitionWithMasterCompleted = 6,
        Core_Transition = 7,
        Core_InteractableEntered = 8,
        Common_CurtainFullyDrawn = 9,
        Common_PrepToTeleport = 10,
        Common_OpenInventory = 11,
        Common_CurtainFullyOpen = 12,
        Common_ItemUsed = 13,
        Common_DialogStarted = 15,
        Common_PlayerPositionMoved = 56,
    }


    public static class DynamicEvent
    {
        
    }
}
