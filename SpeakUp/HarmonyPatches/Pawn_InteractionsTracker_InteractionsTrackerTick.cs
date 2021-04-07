﻿using HarmonyLib;
using RimWorld;
using System.Linq;
using Verse;

namespace SpeakUp
{
    //Fires a new line if scheduled.
    [HarmonyPatch(typeof(Pawn_InteractionsTracker), nameof(Pawn_InteractionsTracker.InteractionsTrackerTick))]
    public static class Pawn_InteractionsTracker_InteractionsTrackerTick
    {
        public static bool running = false;

        public static void Postfix(Pawn ___pawn)
        {
            if (___pawn.RaceProps.Humanlike && ___pawn.interactions != null)
            {
                var tick = GenTicks.TicksGame;
                var statement = DialogManager.Scheduled.Where(x => x.Timing <= tick && x.Emitter == ___pawn).FirstOrDefault();
                if (statement != null)
                {
                    running = true;
                    var intDef = statement.IntDef;
                    intDef.ignoreTimeSinceLastInteraction = true; //temporary, bc RW limit is 120 ticks
                    statement.Emitter.interactions.TryInteractWith(statement.Reciever, statement.IntDef);
                    DialogManager.Scheduled.Remove(statement);
                    running = false;
                }
            }
        }
    }
}