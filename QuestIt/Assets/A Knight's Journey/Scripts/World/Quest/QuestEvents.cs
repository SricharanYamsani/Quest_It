﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RPG.Control;
using RPG.NPCs;

namespace RPG.QuestSystem
{
    public static class QuestEvents 
    {
        public static System.Action InteractionStarted;
        public static System.Action InteractionFinished;

        public static System.Action<QuestTask> TrackTask;
        public static System.Action TaskUpdated;
        public static System.Action<Quest> QuestCompleted;
       
        public static System.Action<NPC> AddNPCLocation;

        public static System.Action<List<string>, bool, int> StartDialogue;
        public static System.Action<int> QuestAccepted;
    }
}
