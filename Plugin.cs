using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Reflection;

// Namespace matching the game (adjust if needed, e.g., Pigeon).
using Pigeon;

namespace DescriptionOverrideMod
{
    [BepInPlugin("com.yourname.descriptionoverride", "DescriptionOverride", "1.0.0")]
    [MycoMod(null, ModFlags.IsClientSide)]
    public class DescriptionOverridePlugin : BaseUnityPlugin
    {
        internal static ConfigEntry<bool> EnableOverride;
        internal static ConfigEntry<bool> EnableHashetty;
        internal static new ManualLogSource Logger;

        private Harmony harmony;

        private void Awake()
        {
            Logger = base.Logger;

            EnableOverride = Config.Bind("General", "EnableDescriptionOverride", true, "If true, uses the serialized _description field instead of TextBlocks.");
            EnableHashetty = Config.Bind("General", "EnableHashettyOverride", true, "If true, applies Hashetty font toggles in descriptions.");

            var harmony = new Harmony("com.yourname.descriptionoverride");
            harmony.PatchAll();

            Logger.LogInfo($"{harmony.Id} loaded!");
        }

        private void OnDestroy()
        {
            harmony?.UnpatchSelf();
        }
    }

    [HarmonyPatch(typeof(Upgrade), nameof(Upgrade.Description), MethodType.Getter)]
    public static class DescriptionPatch
    {
        static bool Prefix(Upgrade __instance, ref string __result)
        {
            if (!DescriptionOverridePlugin.EnableOverride.Value)
                return true;

            string rawDesc = null;
            FieldInfo descField = null;

            try
            {
                if (__instance is GearUpgrade gearUpgrade)
                {
                    descField = AccessTools.Field(typeof(GearUpgrade), "_description");
                    if (descField != null)
                        rawDesc = (string)descField.GetValue(gearUpgrade);
                }
                else if (__instance is PlayerUpgrade playerUpgrade)
                {
                    descField = AccessTools.Field(typeof(PlayerUpgrade), "_description");
                    if (descField != null)
                        rawDesc = (string)descField.GetValue(playerUpgrade);
                }
            }
            catch (Exception ex)
            {
                DescriptionOverridePlugin.Logger.LogWarning($"DescriptionPatch: Failed to get raw desc for {__instance.APIName}: {ex.Message}");
            }

            if (!string.IsNullOrEmpty(rawDesc))
            {
                __result = rawDesc;
                return false;
            }

            return true;
        }
    }
}