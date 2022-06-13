using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace TechTraversal
{
    public class TechTraversalMod : Mod
    {
        public static TechTraversalMod mod;
        public static TechTraversalSettings settings;

        internal static string VersionDir => Path.Combine(ModLister.GetActiveModWithIdentifier("Neronix17.TechTraversal").RootDir.FullName, "Version.txt");
        public static string CurrentVersion { get; private set; }

        public TechTraversalMod(ModContentPack content) : base(content)
        {
            mod = this;
            settings = GetSettings<TechTraversalSettings>();

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            CurrentVersion = $"{version.Major}.{version.Minor}.{version.Build}";

            LogUtil.LogMessage($"{CurrentVersion} ::");

            File.WriteAllText(VersionDir, CurrentVersion);

            Harmony OuterRimHarmony = new Harmony("Neronix17.TechTraversal.RimWorld");
            OuterRimHarmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        public override string SettingsCategory() => "Tech Traversal";

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);
            listing.CheckboxEnhanced("Always Lowest Level", "If checked, the players tech level will always be the lowest unfinished tech level. So if you start with the vanilla Crashlanded scenario, instead of starting Ultratech, you'd start Neolithic till you advance through everything.", ref settings.alwaysLowestUnfinishedLevel);
            listing.GapLine();
            listing.AddLabeledSlider<TechLevel>("Lowest Tech Level", ref settings.lowestTechLevel);
            GUI.color = Color.gray;
            listing.Note("If 'Always Lowest Level' is enabled, this will define what the lowest tech level is, by default this is set to Neolithic. Setting below Neolithic WILL break things, setting to Ultratech or higher makes the point of this mod worthless.", GameFont.Tiny);
            GUI.color = Color.white;
            listing.GapLine();
            listing.CheckboxEnhanced("Show Tech and Counter", "If checked, this enables a note on the Vanilla research screen which says what tech level the selected research is as well as a counter of how many techs from that level are completed.", ref settings.showTechCounter);
            listing.End();

            base.DoSettingsWindowContents(inRect);
        }
    }
}
