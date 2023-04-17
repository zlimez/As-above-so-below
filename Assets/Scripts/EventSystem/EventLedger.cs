using System.Collections.Generic;
using System;
using System.Linq;
using Chronellium.Utils;
using DataStructures;

namespace Chronellium.EventSystem
{
    /// <summary>
    /// Stores a list of events that have occurred thus far for the flattened time travel inclusive timeline and the commonly defined past.
    /// </summary>
    public class EventLedger : StaticInstance<EventLedger>
    {
        /// <summary>
        /// A dictionary of normal past events with their occurrence count.
        /// </summary>
        public Dictionary<GameEvent, int> NormalPastEvents { get; private set; }

        /// <summary>
        /// A dictionary of looped past events with their occurrence count.
        /// </summary>
        public Dictionary<GameEvent, int> LoopedPastEvents { get; private set; }

        /// <summary>
        /// A dictionary containing the recency of events.
        /// </summary>
        public Dictionary<GameEvent, int> EventRecencyTable { get; private set; }

        /// <summary>
        /// A counter for labeling events in sequence for "normal past events".
        /// Resets to 0 when rewinds, strictly increasing.
        /// </summary>
        public int Counter { get; private set; }

        /// <summary>
        /// A queue containing the most recent events.
        /// </summary>
        private MyQueue<GameEvent> recentEvents;

        /// <summary>
        /// The maximum size of the recentEvents queue.
        /// </summary>
        private const int RecentEventsSize = 5;

        protected override void Awake()
        {
            base.Awake();
            NormalPastEvents = new Dictionary<GameEvent, int>();
            LoopedPastEvents = new Dictionary<GameEvent, int>();
            EventRecencyTable = new Dictionary<GameEvent, int>();
            recentEvents = new MyQueue<GameEvent>();
            Counter = 0;
        }

        /// <summary>
        /// Clears the recent event cache.
        /// </summary>
        public void ClearRecentEventCache()
        {
            recentEvents.RemoveAll();
        }

        /// <summary>
        /// Checks if a given event is the most recent event.
        /// </summary>
        /// <param name="gameEvent">The game event to check.</param>
        /// <returns>True if the given event is the most recent event, otherwise false.</returns>
        public bool IsMostRecent(GameEvent gameEvent)
        {
            GameEvent latestEvent = recentEvents.Last();
            return latestEvent != GameEvent.NoEvent && latestEvent == gameEvent;
        }

        public bool IsMostRecent(StaticEvent gameEvent)
        {
            return IsMostRecent(new GameEvent(gameEvent.ToString()));
        }

        /// <summary>
        /// Checks if a given event is recent.
        /// </summary>
        /// <param name="gameEvent">The game event to check.</param>
        /// <returns>True if the given event is recent, otherwise false.</returns>
        public bool IsRecent(GameEvent gameEvent)
        {
            return recentEvents.Contains(gameEvent);
        }

        public bool IsRecent(StaticEvent gameEvent)
        {
            return IsRecent(new GameEvent(gameEvent.ToString()));
        }

        /// <summary>
        /// Adds an event to the recent events queue.
        /// </summary>
        /// <param name="gameEvent">The game event to add.</param>
        public void AddToRecent(GameEvent gameEvent)
        {
            recentEvents.Enqueue(gameEvent);
            if (recentEvents.Count() > RecentEventsSize)
            {
                recentEvents.Dequeue();
            }
        }

        public void AddToRecent(StaticEvent gameEvent)
        {
            AddToRecent(new GameEvent(gameEvent.ToString()));
        }

        /// <summary>
        /// Gets the most recent event out of the given array of events.
        /// </summary>
        /// <param name="events">An array of game events to compare.</param>
        /// <returns>The most recent game event out of the given array.</returns>
        public GameEvent GetMostRecentEvent(params GameEvent[] events)
        {
            GameEvent mostRecentEvent = GameEvent.NoEvent;
            int mostRecentTime = -1;
            foreach (GameEvent gameEvent in events)
            {
                if (EventRecencyTable.ContainsKey(gameEvent) && EventRecencyTable[gameEvent] > mostRecentTime)
                {
                    mostRecentEvent = gameEvent;
                    mostRecentTime = EventRecencyTable[gameEvent];
                }
            }
            return mostRecentEvent;
        }

        public StaticEvent GetMostRecentEvent(params StaticEvent[] events)
        {
            string eventName = GetMostRecentEvent(events.Select(s => new GameEvent(s.ToString())).ToArray()).EventName;

            return Parser.getStaticEventFromText(eventName);
        }

        /// <summary>
        /// Records the occurrence of a game event.
        /// </summary>
        /// <param name="gameEvent">The game event to record.</param>
        /// <param name="isSilent">Set to False to invoke the event after recording.</param>
        public void RecordEvent(GameEvent gameEvent, bool isSilent = true)
        {
            IncrementEventCount(NormalPastEvents, gameEvent);
            IncrementEventCount(LoopedPastEvents, gameEvent);

            AddToRecent(gameEvent);
            EventRecencyTable[gameEvent] = Counter;
            Counter++;

            if (!isSilent)
            {
                EventManager.InvokeEvent(gameEvent);
            }
        }

        public void RecordEvent(StaticEvent gameEvent, bool isSilent = false)
        {
            RecordEvent(new GameEvent(gameEvent.ToString()), isSilent);
        }

        /// <summary>
        /// Reset the NormalPastEvents and LoopedPastEvents to the given event.
        /// </summary>
        /// <param name="normalPastEvents">The normal past events to set.</param>
        /// <param name="loopedPastEvents">The looped past events to set.</param>
        /// <param name="forceReload">Set to True to force reload looped past events.</param>
        public void LoadEvents(Dictionary<GameEvent, int> normalPastEvents, Dictionary<GameEvent, int> loopedPastEvents, bool forceReload = false)
        {
            EventLedger.Instance.Counter = 0;
            EventLedger.Instance.EventRecencyTable.Clear();

            EventLedger.Instance.NormalPastEvents = normalPastEvents;

            if (forceReload || SnapshotManager.Instance.IsForInit)
            {
                // In the rewind case, loopedPastEvents should be strictly increasing, hence only assignment on init.
                EventLedger.Instance.LoopedPastEvents = loopedPastEvents;
            }
        }

        /// <summary>
        /// Removes a game event from all data structures.
        /// </summary>
        /// <param name="gameEvent">The game event to remove.</param>
        public void RemoveEvent(GameEvent gameEvent)
        {
            NormalPastEvents.Remove(gameEvent);
            LoopedPastEvents.Remove(gameEvent);
            EventRecencyTable.Remove(gameEvent);
            recentEvents.Remove(gameEvent);
        }

        public void RemoveEvent(StaticEvent gameEvent)
        {
            RemoveEvent(new GameEvent(gameEvent.ToString()));
        }

        /// <summary>
        /// Returns the number of times a game event has occurred in the past.
        /// </summary>
        /// <param name="gameEvent">The game event to check.</param>
        /// <returns>The occurrence count of the game event.</returns>
        public int GetEventCountInPast(GameEvent gameEvent)
        {
            return GetEventCount(NormalPastEvents, gameEvent);
        }

        public int GetEventCountInPast(StaticEvent gameEvent)
        {
            return GetEventCountInPast(new GameEvent(gameEvent.ToString()));
        }

        /// <summary>
        /// Returns the number of times a game event has occurred in the looped past.
        /// </summary>
        /// <param name="gameEvent">The game event to check.</param>
        /// <returns>The occurrence count of the game event.</returns>
        public int GetEventCountInLoopedPast(GameEvent gameEvent)
        {
            return GetEventCount(LoopedPastEvents, gameEvent);
        }

        public int GetEventCountInLoopedPast(StaticEvent gameEvent)
        {
            return GetEventCountInLoopedPast(new GameEvent(gameEvent.ToString()));
        }

        /// <summary>
        /// Checks if a game event has occurred in the past.
        /// </summary>
        /// <param name="gameEvent">The game event to check.</param>
        /// <returns>True if the game event has occurred in the past, otherwise false.</returns>
        public bool HasEventOccurredInPast(GameEvent gameEvent)
        {
            return GetEventCountInPast(gameEvent) > 0;
        }

        public bool HasEventOccurredInPast(StaticEvent gameEvent)
        {
            return HasEventOccurredInPast(new GameEvent(gameEvent.ToString()));
        }

        /// <summary>
        /// Checks if a game event has occurred in the looped past.
        /// </summary>
        /// <param name="gameEvent">The game event to check.</param>
        /// <returns>True if the game event has occurred in the looped past, otherwise false.</returns>
        public bool HasEventOccurredInLoopedPast(GameEvent gameEvent)
        {
            return GetEventCountInLoopedPast(gameEvent) > 0;
        }

        public bool HasEventOccurredInLoopedPast(StaticEvent gameEvent)
        {
            return HasEventOccurredInLoopedPast(new GameEvent(gameEvent.ToString()));
        }


        /// <summary>
        /// Increments the occurrence count of a game event in the given dictionary.
        /// </summary>
        /// <param name="eventDict">The dictionary containing the event count.</param>
        /// <param name="gameEvent">The game event to increment.</param>
        private void IncrementEventCount(Dictionary<GameEvent, int> eventDict, GameEvent gameEvent)
        {
            if (eventDict.ContainsKey(gameEvent))
            {
                eventDict[gameEvent]++;
            }
            else
            {
                eventDict.Add(gameEvent, 1);
            }
        }

        public void IncrementEventCount(Dictionary<StaticEvent, int> eventDict, StaticEvent gameEvent)
        {
            IncrementEventCount(eventDict.ToDictionary(kvp => new GameEvent(kvp.Key.ToString()), kvp => kvp.Value), new GameEvent(gameEvent.ToString()));
        }

        /// <summary>
        /// Returns the occurrence count of a game event from the given dictionary.
        /// </summary>
        /// <param name="eventDict">The dictionary containing the event count.</param>
        /// <param name="gameEvent">The game event to get the count for.</param>
        /// <returns>The occurrence count of the game event.</returns>
        private int GetEventCount(Dictionary<GameEvent, int> eventDict, GameEvent gameEvent)
        {
            return eventDict.ContainsKey(gameEvent) ? eventDict[gameEvent] : 0;
        }

        public int GetEventCount(Dictionary<StaticEvent, int> eventDict, StaticEvent gameEvent)
        {
            return GetEventCount(eventDict.ToDictionary(kvp => new GameEvent(kvp.Key.ToString()), kvp => kvp.Value), new GameEvent(gameEvent.ToString()));
        }

    }
}