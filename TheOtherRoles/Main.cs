global using Il2CppInterop.Runtime;
global using Il2CppInterop.Runtime.Attributes;
global using Il2CppInterop.Runtime.InteropTypes;
global using Il2CppInterop.Runtime.InteropTypes.Arrays;
global using Il2CppInterop.Runtime.Injection;

using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Hazel;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using TheOtherRoles.Modules;
using TheOtherRoles.Players;
using TheOtherRoles.Utilities;
using Il2CppSystem.Security.Cryptography;
using Il2CppSystem.Text;
using Reactor.Networking.Attributes;
using AmongUs.Data;
using TheOtherRoles.Modules.CustomHats;
using static TheOtherRoles.Modules.ModUpdater;
using System.Data;

namespace TheOtherRoles
{
    [BepInPlugin(Id, "The Other Roles", TORRVersionString)]
    [BepInDependency(SubmergedCompatibility.SUBMERGED_GUID, BepInDependency.DependencyFlags.SoftDependency)]
    [BepInProcess("Among Us.exe")]
    [ReactorModFlags(Reactor.Networking.ModFlags.RequireOnAllClients)]
    
    public class TheOtherRolesPlugin : BasePlugin
    {


        public const string Id = "me.eisbison.theotherroles";
        public const string VersionString = "4.6.0";
        public const string TORRVersionString = "1.0.2";
        public static uint betaDays = 0;  // amount of days for the build to be usable (0 for infinite!)

        public static Version Version = Version.Parse(TORRVersionString);
        internal static BepInEx.Logging.ManualLogSource Logger;
         
        public Harmony Harmony { get; } = new Harmony(Id);
        public static TheOtherRolesPlugin Instance;

        public static int optionsPage = 2;

        public static ConfigEntry<string> DebugMode { get; private set; }
        public static ConfigEntry<bool> GhostsSeeInformation { get; set; }
        public static ConfigEntry<bool> GhostsSeeRoles { get; set; }
        public static ConfigEntry<bool> GhostsSeeModifier { get; set; }
        public static ConfigEntry<bool> GhostsSeeVotes{ get; set; }
        public static ConfigEntry<bool> ShowRoleSummary { get; set; }
        public static ConfigEntry<bool> ShowLighterDarker { get; set; }
        public static ConfigEntry<bool> EnableSoundEffects { get; set; }
        public static ConfigEntry<bool> EnableHorseMode { get; set; }
        public static ConfigEntry<bool> ShowVentsOnMap { get; set; }
        public static ConfigEntry<bool> ShowChatNotifications { get; set; }
        public static ConfigEntry<bool> AddServer { get; set; }
        public static ConfigEntry<string> Ip { get; set; }
        public static ConfigEntry<ushort> Port { get; set; }
        public static ConfigEntry<string> ShowPopUpVersion { get; set; }

        public static Sprite ModStamp;

        public static IRegionInfo[] defaultRegions;


        // This is part of the Mini.RegionInstaller, Licensed under GPLv3
        // file="RegionInstallPlugin.cs" company="miniduikboot">
        public static void UpdateRegions() {
            ServerManager serverManager = FastDestroyableSingleton<ServerManager>.Instance;
            var regions = new IRegionInfo[] {
                new StaticHttpRegionInfo("Custom", StringNames.NoTranslation, Ip.Value, new Il2CppReferenceArray<ServerInfo>(new ServerInfo[1] { new ServerInfo("Custom", Ip.Value, Port.Value, false) })).CastFast<IRegionInfo>()
            };
            
            IRegionInfo currentRegion = serverManager.CurrentRegion;
            Logger.LogInfo($"Adding {regions.Length} regions");
            foreach (IRegionInfo region in regions) {
                if (region == null) 
                    Logger.LogError("Could not add region");
                else {
                    if (currentRegion != null && region.Name.Equals(currentRegion.Name, StringComparison.OrdinalIgnoreCase)) 
                        currentRegion = region;               
                    serverManager.AddOrUpdateRegion(region);
                }
            }

            // AU remembers the previous region that was set, so we need to restore it
            if (currentRegion != null) {
                Logger.LogDebug("Resetting previous region");
                serverManager.SetRegion(currentRegion);
            }


        }

        public override void Load() {

            ModTranslation.Load();
            TheOtherRolesPlugin.Logger.LogInfo("TORR ModTranslation was Loaded\nTORR ModTranslation已被加载");
  
            _ = Helpers.checkBeta(); // Exit if running an expired beta
            _ = Patches.CredentialsPatch.MOTD.loadMOTDs();

            DebugMode = Config.Bind("Costom", "启用Debug模式", "false");
            GhostsSeeInformation = Config.Bind("Costom", "鬼魂可见剩余任务数量", true);
            GhostsSeeRoles = Config.Bind("Costom", "鬼魂可见职业", true);
            GhostsSeeModifier = Config.Bind("Costom", "鬼魂可见附加职业", true);
            GhostsSeeVotes = Config.Bind("Costom", "鬼魂非匿名投票", true);
            ShowRoleSummary = Config.Bind("Costom", "显示复盘结果", true);
            ShowLighterDarker = Config.Bind("Costom", "显示 亮/暗", true);
            EnableSoundEffects = Config.Bind("Costom", "启用音效", true);
            EnableHorseMode = Config.Bind("Costom", "启用马模式", false);
            ShowPopUpVersion = Config.Bind("Costom", "启用多服装模式", "0");
            ShowVentsOnMap = Config.Bind("Costom", "在小地图上显示通风口位置", false);
            ShowChatNotifications = Config.Bind("Costom", "显示聊天通知", true);
            AddServer = Config.Bind("Costom", "添加私服", false);

            Ip = Config.Bind("Costom", "自定义服务器IP", "127.0.0.1");
            Port = Config.Bind("Costom", "自定义服务器Port", (ushort)22023);
            defaultRegions = ServerManager.DefaultRegions;
            // Removes vanilla Servers
            ServerManager.DefaultRegions = new Il2CppReferenceArray<IRegionInfo>(new IRegionInfo[0]);
            UpdateRegions();

            DebugMode = Config.Bind("Costom", "Enable Debug Mode", "false");
            Harmony.PatchAll();
            
            CustomOptionHolder.Load();
            CustomColors.Load();
            CustomHatManager.LoadHats();
            if (BepInExUpdater.UpdateRequired)
            {
                AddComponent<BepInExUpdater>();
                return;
            }

            AddComponent<ModUpdater>();

            EventUtility.Load();
            SubmergedCompatibility.Initialize();
            MainMenuPatch.addSceneChangeCallbacks();
            _ = RoleInfo.loadReadme();
            AddToKillDistanceSetting.addKillDistance();
            TheOtherRolesPlugin.Logger.LogInfo("加载TORR完成！");
        }
    }

    // Deactivate bans, since I always leave my local testing game and ban myself
    [HarmonyPatch(typeof(StatsManager), nameof(StatsManager.AmBanned), MethodType.Getter)]
    public static class AmBannedPatch
    {
        public static void Postfix(out bool __result)
        {
            __result = false;
        }
    }
    [HarmonyPatch(typeof(ChatController), nameof(ChatController.Awake))]
    public static class ChatControllerAwakePatch {
        private static void Prefix() {
            if (!EOSManager.Instance.isKWSMinor) {
                DataManager.Settings.Multiplayer.ChatMode = InnerNet.QuickChatModes.FreeChatOrQuickChat;
            }
        }
    }
    
    // Debugging tools
    [HarmonyPatch(typeof(KeyboardJoystick), nameof(KeyboardJoystick.Update))]
    public static class DebugManager
    {
        private static readonly string passwordHash = "d1f51dfdfd8d38027fd2ca9dfeb299399b5bdee58e6c0b3b5e9a45cd4e502848";
        private static readonly System.Random random = new System.Random((int)DateTime.Now.Ticks);
        private static List<PlayerControl> bots = new List<PlayerControl>();

        public static void Postfix(KeyboardJoystick __instance)
        {
            // Check if debug mode is active.
            StringBuilder builder = new StringBuilder();
            SHA256 sha = SHA256Managed.Create();
            Byte[] hashed = sha.ComputeHash(Encoding.UTF8.GetBytes(TheOtherRolesPlugin.DebugMode.Value));
            foreach (var b in hashed) {
                builder.Append(b.ToString("x2"));
            }
            string enteredHash = builder.ToString();
            if (enteredHash != passwordHash) return;


            // Spawn dummys
            /*if (Input.GetKeyDown(KeyCode.F)) {
                var playerControl = UnityEngine.Object.Instantiate(AmongUsClient.Instance.PlayerPrefab);
                var i = playerControl.PlayerId = (byte) GameData.Instance.GetAvailableId();

                bots.Add(playerControl);
                GameData.Instance.AddPlayer(playerControl, new InnerNet.ClientData(0));
                AmongUsClient.Instance.Spawn(playerControl, -2, InnerNet.SpawnFlags.None);
                
                playerControl.transform.position = CachedPlayer.LocalPlayer.transform.position;
                playerControl.GetComponent<DummyBehaviour>().enabled = true;
                playerControl.NetTransform.enabled = false;
                playerControl.SetName(RandomString(10));
                playerControl.SetColor((byte) random.Next(Palette.PlayerColors.Length));
                playerControl.Data.RpcSetTasks(new byte[0]);
            }*/

            // Terminate round
            if(Input.GetKeyDown(KeyCode.L)) {
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.ForceEnd, Hazel.SendOption.Reliable, -1);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.forceEnd();
            }
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
