namespace Chronellium.EventSystem
{
    // WARNING: Do not change the order of the enum values or their assigned integer values
    // after development has started, as it may cause issues with saved data and other parts
    // of the code that rely on these values. If you need to add new values, append them to
    // the end of the list and assign them new unique integer values.
    public enum StaticEvent
    {
        NoEvent = 0,
        Core_SnapshotManagerReady = 1,
        Core_GameManagerReady = 2,
        Core_SnapshotTaken = 3,
        Core_SnapshotLoaded = 4,
        Core_TransitionWithMaster = 5,
        Core_TransitionWithMasterCompleted = 6,
        Core_Transition = 7,
        Core_InteractableEntered = 8,
        Common_CurtainFullyDrawn = 9,
        Common_PrepToTeleport = 10,
        Common_OpenInventory = 11,
        Common_CurtainFullyOpen = 12,
        Common_ItemUsed = 13,
        Common_ForcedRewind = 14,
        Common_DialogStarted = 15,
        Common_PlayerPositionMoved = 56,
        BrothelRewindEvents_FirstIterationCompleted = 16,
        BrothelRewindEvents_SecondIterationCompleted = 17,
        BrothelSnapshotEvents_AtCityFloor = 18,
        BrothelSnapshotEvents_AtEntrance = 19,
        BrothelSnapshotEvents_AfterGingerLock = 20,
        BrothelSnapshotEvents_AfterNaraTalk = 21,
        BrothelSnapshotEvents_AfterPimpKnock = 22,
        LobbyEvents_OnBrothelEnter = 23,
        LobbyEvents_OnCandyChosen = 24,
        LobbyEvents_OnGingerChosen = 25,
        LobbyEvents_TeleportedAfterScan = 26,
        LobbyEvents_HectorAtCounter = 27,
        LobbyEvents_LeaveCue = 28,
        LobbyEvents_SawVendingMachine = 29,
        LobbyEvents_InteractedWithVendingMachine = 30,
        LobbyEvents_BoughtItem = 31,
        LobbyEvents_NoItemBought = 32,
        LobbyEvents_McHaleStands = 33,
        Level2Events_GingerLockBroken = 34,
        Level2Events_GingerLockUnlocked = 35,
        Level2Events_OnDoorBashed = 36,
        Level2Events_EnteredGingerRoom = 37,
        Level2Events_ExploredGingersRoom = 38,
        Level2Events_EnterCue = 39,
        Level3Events_OnTalkCherry = 40,
        Level3Events_OnCorrectCherryNameGuess = 41,
        Level3Events_OnCherryTalkComplete = 42,
        Level4Events_TeaDrugged = 43,
        Level4Events_CandyLockUnlocked = 44,
        Level4Events_CheckDrinkSpike = 45,
        CommonEvents_ElevatorTaken = 46,
        CommonEvents_RewindTutorialTriggered = 47,
        CommonEvents_InventoryTutorialTriggered = 48,
        CommonEvents_SkipTutorialTriggered = 49,
        AkaMeeting_FirstIterationCompleted = 50,
        AkaMeeting_CarSirensSound = 51,
        AkaMeeting_CatIsThreatened = 52,
        AkaMeeting_CatIsKilled = 53,
        AkaMeeting_PolicemanLeaving = 54,
        AkaMeeting_StopsHiding = 55,
        BrothelSnapshotEvents_MchaleDoorBreak = 57,
        AkaMeeting_WatchedWindowScene = 58,
        AkaMeeting_AkaFreed = 59,
        AkaMeeting_ExitAkaPlace = 60,
        AkaMeeting_HectorLooksInFromWindow = 61,
        AkaMeeting_HectorStopsLookingInWindow = 62,
        AkaMeeting_UrgentHidingNeeded = 63,
        AkaMeeting_PolicemanLeft = 64,
        AkaSnapshotEvents_CityRooftop = 65,
        AkaMeeting_PolicemanNotComingBackToPlace = 66,
        AkaMeeting_PolicemanAtCar = 67,
        AkaMeeting_PolicemanBackToLab = 68,
        AkaMeeting_HideAndLeave = 69, 
        AkaMeeting_TookScrewdriver = 120,
        WarehouseSnapshotEvents_Entrance = 200,
        Warehouse_HackedWarehouseDoor = 201,
        Warehouse_StartedHackingWarehouseDoor = 1049
    }


    public static class DynamicEvent
    {
        // Lobby
        public static GameEvent Convo_FaceScannedGinger = new GameEvent(Conversation.EventPrefix + "FaceScanGinger");
        public static GameEvent Convo_FaceScannedCandy = new GameEvent(Conversation.EventPrefix + "FaceScanCandy");
        public static GameEvent Convo_CherryArgCompleted = new GameEvent(Conversation.EventPrefix + "CherryArg");
        public static GameEvent Convo_FinishGreetingHector = new GameEvent(Conversation.EventPrefix + "SugarGreeting");
        public static GameEvent Convo_FinishGreetingHector2 = new GameEvent(Conversation.EventPrefix + "SugarGreeting 2");
        public static GameEvent Convo_GingerSelectedConvo = new GameEvent(Conversation.EventPrefix + "GingerChosen");
        public static GameEvent Convo_CandySelectedConvo = new GameEvent(Conversation.EventPrefix + "CandyChosen");
        public static GameEvent Convo_VendingMachineThought = new GameEvent(Conversation.EventPrefix + "VendingThought");
        public static GameEvent Convo_VendingMachineRemark = new GameEvent(Conversation.EventPrefix + "VendingRemark");
        public static GameEvent Convo_CommandFixDoor = new GameEvent(Conversation.EventPrefix + "FixDoor");
        public static GameEvent Convo_FirstIterationPostThought = new GameEvent(Conversation.EventPrefix + "firstIterationPostThought");

        // Level 3
        public static GameEvent Convo_CherryGuessedName = new GameEvent(Conversation.EventPrefix + "Cherry_4_Item");

        // Level 4
        public static GameEvent Convo_CandyReassured = new GameEvent(Conversation.EventPrefix + "Candy_reassureCandyConvo");
        public static GameEvent Convo_HectorGreetedCandy = new GameEvent(Conversation.EventPrefix + "Candy_greetCandyConvo");
        public static GameEvent Convo_CandyOpenedDoorConvoCompleted = new GameEvent(Conversation.EventPrefix + "Candy_candyOpenedDoorConvo");
        public static GameEvent Convo_GlassInspectRemark = new GameEvent(Conversation.EventPrefix + "PimpGlass");
        public static GameEvent Convo_FoundItemConvoCompleted = new GameEvent(Conversation.EventPrefix + "PimpPainting_foundItemConvo");
        public static GameEvent Convo_FoundItemBeforeConvoCompleted = new GameEvent(Conversation.EventPrefix + "PimpPainting_foundItemBeforeConvo");
        public static GameEvent Convo_PimpReturnedConvoCompleted = new GameEvent(Conversation.EventPrefix + "PimpReturn");
        public static GameEvent Convo_CallCandyRealNameConvoCompleted = new GameEvent(Conversation.EventPrefix + "Room2Door_callCandyRealNameConvo");
        public static GameEvent Convo_AskQuestionConvoCompleted = new GameEvent(Conversation.EventPrefix + "Candy_askQuestionConvo");
        public static GameEvent Convo_MeetingPimpConvo = new GameEvent(Conversation.EventPrefix + "PimpMeeting1");
        public static GameEvent Convo_PostMurderThought = new GameEvent(Conversation.EventPrefix + "PostMurderThought");
        public static GameEvent Item_YellowsUsed = new GameEvent(Item.itemUsedPrefix + "Yellow(Drugs)");
        public static GameEvent Choice_HectorKnockedOnPimpDoor = new GameEvent(Choice.EventPrefix + "Knock on the door");

        // Murder Cutscene
        public static GameEvent Convo_MurderFirstTime = new GameEvent(Conversation.EventPrefix + "MurderConvo1-1");
        public static GameEvent Convo_MurderSecondTime = new GameEvent(Conversation.EventPrefix + "MurderConvo1-2");
        public static GameEvent Convo_MurderThirdTime = new GameEvent(Conversation.EventPrefix + "MurderConvo1-3");

        // Brothel Ending
        public static GameEvent Convo_BrothelEndingEvidence = new GameEvent(Conversation.EventPrefix + "Ending_Evidence");
        public static GameEvent Convo_BrothelEndingCandyConvo = new GameEvent(Conversation.EventPrefix + "Ending_CandyConvo");
        
        // Aka Meeting
        public static GameEvent Convo_AkaMeeting_AkaWarnHector = new GameEvent(Conversation.EventPrefix + "AkaWarning");
        public static GameEvent Convo_AkaMeeting_LurgeConvo = new GameEvent(Conversation.EventPrefix + "AkaChoice-AboutLurge");
    
        // Warehouse ServerRoom
        public static GameEvent Convo_WiringFirstTime = new GameEvent(Conversation.EventPrefix + "WiringPanel_firstConvo");
        public static GameEvent Convo_ServerRoomMachineryDoorFirstTime = new GameEvent(Conversation.EventPrefix + "ServerRoomMachineryDoor_firstConvo");
        public static GameEvent Convo_DisableStinger = new GameEvent(Conversation.EventPrefix + "ServerRoomEmma_DisableStinger");
        public static GameEvent Convo_DeliveryAndroid = new GameEvent(Conversation.EventPrefix + "ServerRoomEmma_TurnOnDeliveryAndroid");
        public static GameEvent Convo_ServerRoomEmmaSurprised = new GameEvent(Conversation.EventPrefix + "ServerRoomEmma_Surprised");
    }

}
