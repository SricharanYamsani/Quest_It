using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.QuestSystem
{
    public static class QuestEvents 
    {
        public static System.Action<QuestTask> TrackTask;
        public static System.Action TaskUpdated;
        public static System.Action TaskCompleted;
    }
}
