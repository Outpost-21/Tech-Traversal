using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace TechTraversal
{
    public class TechTraversalSettings : ModSettings
    {
        public bool verboseLogging = false;

        public Dictionary<FactionDef, TechLevel> factionTechMap = new Dictionary<FactionDef, TechLevel>();

        /// <summary>
        /// If true, the players tech level will always be the lowest unfinished TechLevel. If false the players tech level will only raise above what it started at if it was lower than Ultratech.
        /// </summary>
        public bool alwaysLowestUnfinishedLevel = false;

        /// <summary>
        /// If alwaysLowestUnfinishedLevel is true, this can be used to change the lowest tech level.
        /// </summary>
        public TechLevel lowestTechLevel = TechLevel.Neolithic;

        /// <summary>
        /// If true, shows the tech level of a selected research and how many are completed out of the total for that tech level.
        /// </summary>
        public bool showTechCounter = true;

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look(ref alwaysLowestUnfinishedLevel, "alwaysLowestUnfinishedLevel", false);
            Scribe_Values.Look(ref lowestTechLevel, "lowestTechLevel", TechLevel.Neolithic);
            Scribe_Values.Look(ref showTechCounter, "showTechCounter", true);
        }

        public bool IsValidSetting(string input)
        {
            if (GetType().GetFields().Where(p => p.FieldType == typeof(bool)).Any(i => i.Name == input))
            {
                return true;
            }

            return false;
        }

        public IEnumerable<string> GetEnabledSettings
        {
            get
            {
                return GetType().GetFields().Where(p => p.FieldType == typeof(bool) && (bool)p.GetValue(this)).Select(p => p.Name);
            }
        }
    }
}
