﻿using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using Verse;
using Verse.Grammar;

namespace SpeakUp
{
    //Insert custom rules
    [HarmonyPatch(typeof(GrammarResolver), nameof(GrammarResolver.Resolve))]
    class GrammarResolver_Resolve
    {
        private static FieldInfo rulesInfo = AccessTools.Field(typeof(GrammarRequest), "rules");

        public static void Prefix(object __instance, string rootKeyword, GrammarRequest request)
        {
            if (rootKeyword == "r_logentry")
            {
                List<Rule> rules = (List<Rule>)rulesInfo.GetValue(request);
                if (!rules.NullOrEmpty())
                {
                    rules.AddRange(ExtraGrammarUtility.ExtraRules());
                }
            }
        }
    }
}
