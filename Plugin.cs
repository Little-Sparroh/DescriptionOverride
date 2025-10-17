using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Reflection;

[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
[MycoMod(null, ModFlags.IsClientSide)]
public class DescriptionOverridePlugin : BaseUnityPlugin
{
    public const string PluginGUID = "sparroh.descriptionoverride";
    public const string PluginName = "DescriptionOverride";
    public const string PluginVersion = "1.0.1";

    internal static ConfigEntry<bool> EnableOverride;
    internal static new ManualLogSource Logger;

    private Harmony harmony;

    private void Awake()
    {
        Logger = base.Logger;

        EnableOverride = Config.Bind("General", "EnableDescriptionOverride", true, "If true, uses the serialized _description field instead of TextBlocks.");

        harmony = new Harmony(PluginGUID);
        harmony.PatchAll();
        Logger.LogInfo($"{PluginName} loaded successfully.");
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
        }

        if (!string.IsNullOrEmpty(rawDesc))
        {
            __result = rawDesc;
            return false;
        }

        return true;
    }
}
