using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
// using static Obeliskial_Essentials.Essentials;
using System;
using static BetterVisualization.Plugin;
using static BetterVisualization.CustomFunctions;
using static BetterVisualization.BetterVisualizationFunctions;
using System.Collections.Generic;
using static Functions;
using UnityEngine;
// using Photon.Pun;
using TMPro;
using System.Linq;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Diagnostics;
// using Unity.TextMeshPro;

// Make sure your namespace is the same everywhere
namespace BetterVisualization
{

    [HarmonyPatch] // DO NOT REMOVE/CHANGE - This tells your plugin that this is part of the mod

    public class BetterVisualizationPatches
    {
        public static bool devMode = false; //DevMode.Value;
        public static bool bSelectingPerk = false;
        public static bool IsHost()
        {
            return GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster();
        }




        // [HarmonyPatch(typeof(PerformanceRecorderSystem), nameof(PerformanceRecorderSystem.StaticRecordingEnabled), MethodType.Setter)]
        // [HarmonyPostfix]
        // static void SetterPostfix()
        // {
        //     // Existence of this method causes startup error during patching.
        //     // Even if we patch "set_StaticRecordingEnabled" and don't specify a method type,
        //     // harmony decides there's supposed to be a param named "value" in the patched method
        // }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(CardItem), nameof(CardItem.CardSize), MethodType.Getter)]
        public static void CardItemGetterPostfix(ref float __result)
        {
            // This patch works and the value is correctly logged.
            //
            // example code (ran elsewhere) that triggers this:
            // var wasEnabled = PerformanceRecorderSystem.StaticRecordingEnabled;
            //
            // If it was a field rather than a property, I don't think that would be the case.
            LogDebug($"CardItemGetterPostfix as: {__result}");

        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(CardItem), "Start")]
        public static void StartPrefix(ref float ___cardSize, ref float ___cardSizeTable, ref float ___cardSizeAmplified)
        {
            LogDebug($"StartPrefix ");

            ___cardSize = CardSizeMultiplier.Value;
            ___cardSizeTable = CardSizeMultiplier.Value * 1.2f;
            ___cardSizeAmplified = CardSizeMultiplier.Value * 1.4f;

        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PopupNode), "Start")]
        public static void PopupNodeStartPrefix(ref PopupNode __instance)
        {
            LogDebug($"PopupNodeStartPrefix");
            __instance.transform.localScale = new Vector3(__instance.transform.localScale.x * NodePopupSizeMultiplier.Value,
                                                          __instance.transform.localScale.y * NodePopupSizeMultiplier.Value,
                                                          __instance.transform.localScale.z);


        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(GameManager), nameof(GameManager.Resize))]
        public static bool ResizePrefix()
        {
            Globals.Instance.sizeW = Camera.main.orthographicSize * 2f * Camera.main.aspect * GlobalSizeMultiplier.Value;
            Globals.Instance.sizeH = Camera.main.orthographicSize * 2f * GlobalSizeMultiplier.Value;
            Globals.Instance.multiplierX = 0.9999999f;
            Globals.Instance.multiplierY = 1f;
            Globals.Instance.scale = 1920f * (float)Screen.height / (1080f * (float)Screen.width);
            Globals.Instance.scaleV = new Vector3(Globals.Instance.scale, Globals.Instance.scale, 1f);
            if (MainMenuManager.Instance != null)
            {
                MainMenuManager.Instance.Resize();
            }
            else if (HeroSelectionManager.Instance != null)
            {
                HeroSelectionManager.Instance.Resize();
            }
            else if (MapManager.Instance != null)
            {
                MapManager.Instance.Resize();
            }
            else if (ChallengeSelectionManager.Instance != null)
            {
                ChallengeSelectionManager.Instance.Resize();
            }
            else if (TownManager.Instance != null)
            {
                TownManager.Instance.Resize();
            }
            else if (MatchManager.Instance != null)
            {
                MatchManager.Instance.Resize();
            }
            if (OptionsManager.Instance != null)
            {
                OptionsManager.Instance.Resize();
            }
            if (PlayerUIManager.Instance != null)
            {
                PlayerUIManager.Instance.Resize();
            }
            if (CardCraftManager.Instance != null)
            {
                CardCraftManager.Instance.Resize();
            }
            if (SettingsManager.Instance != null)
            {
                SettingsManager.Instance.Resize();
            }
            if (CardScreenManager.Instance != null)
            {
                CardScreenManager.Instance.Resize();
            }
            if (PopupManager.Instance != null)
            {
                PopupManager.Instance.Resize();
            }
            return false;
        }

    }
}