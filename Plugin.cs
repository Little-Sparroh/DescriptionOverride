using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Reflection;

[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
[MycoMod(null, ModFlags.IsClientSide)]
public class SparrohPlugin : BaseUnityPlugin
{
    public const string PluginGUID = "sparroh.descriptionoverride";
    public const string PluginName = "DescriptionOverride";
    public const string PluginVersion = "1.0.2";

    internal static ConfigEntry<bool> EnableOverride;
    internal static new ManualLogSource Logger;

    private Harmony harmony;

    private void Awake()
    {
        Logger = base.Logger;

        try
        {
            EnableOverride = Config.Bind("General", "EnableDescriptionOverride", true, "If true, uses the serialized _description field instead of TextBlocks.");

            harmony = new Harmony(PluginGUID);
            harmony.PatchAll();
            Logger.LogInfo($"{PluginName} loaded successfully.");
        }
        catch (Exception ex)
        {
            Logger.LogError($"Failed to initialize {PluginName}: {ex.Message}");
            Logger.LogDebug(ex.ToString());
        }
    }

    private void OnDestroy()
    {
        try
        {
            harmony?.UnpatchSelf();
        }
        catch (Exception ex)
        {
            Logger.LogError($"Failed to unpatch {PluginName}: {ex.Message}");
            Logger.LogDebug(ex.ToString());
        }
    }
}

[HarmonyPatch(typeof(Upgrade), nameof(Upgrade.Description), MethodType.Getter)]
public static class DescriptionPatch
{
    static bool Prefix(Upgrade __instance, ref string __result)
    {
        try
        {
            if (!SparrohPlugin.EnableOverride.Value)
                return true;
        }
        catch (Exception ex)
        {
            SparrohPlugin.Logger.LogError($"Failed to access config value: {ex.Message}");
            SparrohPlugin.Logger.LogDebug(ex.ToString());
            return true;
        }

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
            SparrohPlugin.Logger.LogError($"Failed to access description field for {__instance.GetType().Name}: {ex.Message}");
            SparrohPlugin.Logger.LogDebug(ex.ToString());
        }

        if (!string.IsNullOrEmpty(rawDesc))
        {
            __result = rawDesc;
            return false;
        }

        return true;
    }
}
