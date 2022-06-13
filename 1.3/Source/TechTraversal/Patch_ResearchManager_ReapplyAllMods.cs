using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

using HarmonyLib;

namespace TechTraversal
{
    [HarmonyPatch(typeof(ResearchManager), "ReapplyAllMods")]
    public static class Patch_ResearchManager_ReapplyAllMods
    {
        [HarmonyPostfix]
        public static void Postfix(ResearchManager __instance)
        {
            FactionDef playerFactionDef = Faction.OfPlayer.def;

            // Restore Original Tech Level or if setting true start with neolithic
            if (TechTraversalMod.settings.alwaysLowestUnfinishedLevel)
            {
                playerFactionDef.techLevel = TechTraversalMod.settings.lowestTechLevel;
            }
            else
            {
                playerFactionDef.techLevel = TechTraversalMod.settings.factionTechMap.GetValueSafe(Faction.OfPlayer.def);
            }

            // Advance Tech Level as Necessary
            while (playerFactionDef.techLevel < TechLevel.Ultra && !AnyProjectAtLevelUnfinished(playerFactionDef.techLevel))
            {
                playerFactionDef.techLevel++;
                LogUtil.LogMessage("Upgraded player tech level to " + playerFactionDef.techLevel.ToStringHuman());
            }
        }

        public static bool AnyProjectAtLevelUnfinished(TechLevel techLevel)
        {
            return DefDatabase<ResearchProjectDef>.AllDefsListForReading.Find(rpd => rpd.techLevel == techLevel && !rpd.IsFinished) != null;
        }
    }
}
