using System;
using System.Collections.Generic;
using Saveable;
using Tuples;

namespace Chronellium.EventSystem
{
    /// <summary>
    /// Handles the storage and retrieval of memorable ledger events.
    /// </summary>
    public class LedgerMemorable : Memorable<Pair<SaveableCollection, SaveableCollection>>
    {
        /// <summary>
        /// Converts a SaveableCollection into a dictionary of GameEvents and their occurrence counts.
        /// </summary>
        /// <param name="events">The SaveableCollection to convert.</param>
        /// <returns>A dictionary containing GameEvents and their occurrence counts.</returns>
        public static Dictionary<GameEvent, int> ConvertToEventTable(SaveableCollection events)
        {
            Dictionary<GameEvent, int> eventTable = new Dictionary<GameEvent, int>();
            foreach (SaveableCountableItem eventRep in events.items)
            {
                eventTable.Add(new GameEvent(eventRep.itemName), eventRep.count);
            }
            return eventTable;
        }

        /// <summary>
        /// Takes a snapshot of the current state and stores it in memory.
        /// </summary>
        /// <param name="snapshot">The snapshot to take.</param>
        public override void TakeSnapshot(Snapshot snapshot)
        {
            memory.Push(new Pair<SaveableCollection, SaveableCollection>(
                new SaveableCollection(EventLedger.Instance.NormalPastEvents),
                new SaveableCollection(EventLedger.Instance.LoopedPastEvents)));
        }

        /// <summary>
        /// Loads a snapshot based on the specified offset.
        /// </summary>
        /// <param name="offset">The offset to load the snapshot from.</param>
        public override void LoadSnapshot(int offset)
        {
            if (!HasSufficientMemory(offset))
            {
                LoadActiveSnapshot();
            }
            base.LoadSnapshot(offset);

            EventLedger.Instance.LoadEvents(
                ConvertToEventTable(memory.Peek().head),
                ConvertToEventTable(memory.Peek().tail)
            );
        }

        /// <summary>
        /// Loads a snapshot from the specified Snapshot object.
        /// </summary>
        /// <param name="snapshot">The Snapshot object to load the snapshot from.</param>
        protected override void LoadSnapshot(Snapshot snapshot)
        {
            // TODO: Get from the database.
        }
    }
}