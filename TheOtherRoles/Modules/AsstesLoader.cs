using System.Reflection;
using Reactor.Utilities.Extensions;
using UnityEngine;

namespace TheOtherRoles.Modules
{
    public static class ZipsLoad
    {
        public static readonly Assembly dll = Assembly.GetExecutingAssembly();
        public static bool flag = false;

        public static void Load()
        {
            if (flag) return;
            flag = true;
            LoadTORCommunityEditionZips();
        }

        public static void LoadTORCommunityEditionZips()
        {
            var resourceAudioZipBundleStream = dll.GetManifestResourceStream("TheOtherRoles.Resources.7_zip");
            var ZipBundleStream = AssetBundle.LoadFromMemory(resourceAudioZipBundleStream.ReadFully());
            //音频有需要了再添加     
            //Sprite
            CustomMain.customZips.RightPanelCloseButton = ZipBundleStream.LoadAsset<Sprite>("RightPanelCloseButton.png").DontUnload();
            CustomMain.customZips.Alert = ZipBundleStream.LoadAsset<Sprite>("Alert.png").DontUnload();
            CustomMain.customZips.NoAlert = ZipBundleStream.LoadAsset<Sprite>("NoAlert.png").DontUnload();
            CustomMain.customZips.BlackmailerBlackmailButton = ZipBundleStream.LoadAsset<Sprite>("BlackmailerBlackmailButton.png").DontUnload();
            CustomMain.customZips.BlackmailerLetter = ZipBundleStream.LoadAsset<Sprite>("BlackmailerLetter.png").DontUnload();
            CustomMain.customZips.BlackmailerOverlay = ZipBundleStream.LoadAsset<Sprite>("BlackmailerOverlay.png").DontUnload();
            CustomMain.customZips.Bomb_Button_Defuse = ZipBundleStream.LoadAsset<Sprite>("Bomb_Button_Defuse.png").DontUnload();
            CustomMain.customZips.Bomb_Button_Plant = ZipBundleStream.LoadAsset<Sprite>("Bomb_Button_Plant.png").DontUnload();
            CustomMain.customZips.Bomber = ZipBundleStream.LoadAsset<Sprite>("Bomber.png").DontUnload();
            CustomMain.customZips.CamoButton = ZipBundleStream.LoadAsset<Sprite>("CamoButton.png").DontUnload();
            CustomMain.customZips.CleanButton = ZipBundleStream.LoadAsset<Sprite>("CleanButton.png").DontUnload();
            CustomMain.customZips.CloseVentButton = ZipBundleStream.LoadAsset<Sprite>("CloseVentButton.png").DontUnload();
            CustomMain.customZips.CurseButton = ZipBundleStream.LoadAsset<Sprite>("CurseButton.png").DontUnload();
            CustomMain.customZips.CurseKillButton = ZipBundleStream.LoadAsset<Sprite>("CurseKillButton.png").DontUnload();
            CustomMain.customZips.DeputyHandcuffButton = ZipBundleStream.LoadAsset<Sprite>("DeputyHandcuffButton.png").DontUnload();
            CustomMain.customZips.DeputyHandcuffedEN = ZipBundleStream.LoadAsset<Sprite>("DeputyHandcuffedEN.png").DontUnload();
            CustomMain.customZips.DeputyHandcuffedCN = ZipBundleStream.LoadAsset<Sprite>("DeputyHandcuffedCN.png").DontUnload();
            CustomMain.customZips.Disperse = ZipBundleStream.LoadAsset<Sprite>("Disperse.png").DontUnload();
            CustomMain.customZips.DoomsayerButton = ZipBundleStream.LoadAsset<Sprite>("DoomsayerButton.png").DontUnload();
            CustomMain.customZips.DouseButton = ZipBundleStream.LoadAsset<Sprite>("DouseButton.png").DontUnload();
            CustomMain.customZips.EraserButton = ZipBundleStream.LoadAsset<Sprite>("EraserButton.png").DontUnload();
            CustomMain.customZips.FindButton = ZipBundleStream.LoadAsset<Sprite>("FindButton.png").DontUnload();
            CustomMain.customZips.GarlicButton = ZipBundleStream.LoadAsset<Sprite>("GarlicButton.png").DontUnload();
            CustomMain.customZips.HackerButton = ZipBundleStream.LoadAsset<Sprite>("HackerButton.png").DontUnload();
            CustomMain.customZips.HideNSeekArrowButton = ZipBundleStream.LoadAsset<Sprite>("HideNSeekArrowButton.png").DontUnload();
            CustomMain.customZips.IgniteButton = ZipBundleStream.LoadAsset<Sprite>("IgniteButton.png").DontUnload();
            CustomMain.customZips.InvisButton = ZipBundleStream.LoadAsset<Sprite>("InvisButton.png").DontUnload();
            CustomMain.customZips.LighterButton = ZipBundleStream.LoadAsset<Sprite>("LighterButton.png").DontUnload();
            CustomMain.customZips.LightsOutButton = ZipBundleStream.LoadAsset<Sprite>("LightsOutButton.png").DontUnload();
            CustomMain.customZips.MediumButton = ZipBundleStream.LoadAsset<Sprite>("MediumButton.png").DontUnload();
            CustomMain.customZips.Mine = ZipBundleStream.LoadAsset<Sprite>("Mine.png").DontUnload();
            CustomMain.customZips.MorphButton = ZipBundleStream.LoadAsset<Sprite>("MorphButton.png").DontUnload();
            CustomMain.customZips.NinjaAssassinateButton = ZipBundleStream.LoadAsset<Sprite>("NinjaAssassinateButton.png").DontUnload();
            CustomMain.customZips.NinjaMarkButton = ZipBundleStream.LoadAsset<Sprite>("NinjaMarkButton.png").DontUnload();
            CustomMain.customZips.PathfindButton = ZipBundleStream.LoadAsset<Sprite>("PathfindButton.png").DontUnload();
            CustomMain.customZips.PlaceCameraButton = ZipBundleStream.LoadAsset<Sprite>("PlaceCameraButton.png").DontUnload();
            CustomMain.customZips.PlaceJackInTheBoxButton = ZipBundleStream.LoadAsset<Sprite>("PlaceJackInTheBoxButton.png").DontUnload();
            CustomMain.customZips.PlacePortalButton = ZipBundleStream.LoadAsset<Sprite>("PlacePortalButton.png").DontUnload();
            CustomMain.customZips.PursuerButton = ZipBundleStream.LoadAsset<Sprite>("PursuerButton.png").DontUnload();
            CustomMain.customZips.Rampage = ZipBundleStream.LoadAsset<Sprite>("Rampage.png").DontUnload();
            CustomMain.customZips.Remember = ZipBundleStream.LoadAsset<Sprite>("Remember.png").DontUnload();
            CustomMain.customZips.RepairButton = ZipBundleStream.LoadAsset<Sprite>("RepairButton.png").DontUnload();
            CustomMain.customZips.Reveal = ZipBundleStream.LoadAsset<Sprite>("Reveal.png").DontUnload();
            CustomMain.customZips.SampleButton = ZipBundleStream.LoadAsset<Sprite>("SampleButton.png").DontUnload();
            CustomMain.customZips.Shield = ZipBundleStream.LoadAsset<Sprite>("Shield.png").DontUnload();
            CustomMain.customZips.ShieldButton = ZipBundleStream.LoadAsset<Sprite>("ShieldButton.png").DontUnload();
            CustomMain.customZips.ShiftButton = ZipBundleStream.LoadAsset<Sprite>("ShiftButton.png").DontUnload();
            CustomMain.customZips.SidekickButton = ZipBundleStream.LoadAsset<Sprite>("SidekickButton.png").DontUnload();
            CustomMain.customZips.SpeedboostButton = ZipBundleStream.LoadAsset<Sprite>("SpeedboostButton.png").DontUnload();
            CustomMain.customZips.SpellButton = ZipBundleStream.LoadAsset<Sprite>("SpellButton.png").DontUnload();
            CustomMain.customZips.Swoop = ZipBundleStream.LoadAsset<Sprite>("Swoop.png").DontUnload();
            CustomMain.customZips.TimeShieldButton = ZipBundleStream.LoadAsset<Sprite>("TimeShieldButton.png").DontUnload();
            CustomMain.customZips.TrackerButton = ZipBundleStream.LoadAsset<Sprite>("TrackerButton.png").DontUnload();
            CustomMain.customZips.Trapper_Place_Button = ZipBundleStream.LoadAsset<Sprite>("Trapper_Place_Button.png").DontUnload();
            CustomMain.customZips.TricksterVentButton = ZipBundleStream.LoadAsset<Sprite>("TricksterVentButton.png").DontUnload();
            CustomMain.customZips.UndertakerDragButton = ZipBundleStream.LoadAsset<Sprite>("UndertakerDragButton.png").DontUnload();
            CustomMain.customZips.UnStuck = ZipBundleStream.LoadAsset<Sprite>("UnStuck.png").DontUnload();
            CustomMain.customZips.UsePortalButton = ZipBundleStream.LoadAsset<Sprite>("UsePortalButton.png").DontUnload();
            CustomMain.customZips.VampireButton = ZipBundleStream.LoadAsset<Sprite>("VampireButton.png").DontUnload();
            CustomMain.customZips.VultureButton = ZipBundleStream.LoadAsset<Sprite>("VultureButton.png").DontUnload();
            CustomMain.customZips.Watch = ZipBundleStream.LoadAsset<Sprite>("Watch.png").DontUnload();
            CustomMain.customZips.YoyoBlinkButtonSprite = ZipBundleStream.LoadAsset<Sprite>("YoyoBlinkButtonSprite.png").DontUnload();
            CustomMain.customZips.YoyoMarkButtonSprite = ZipBundleStream.LoadAsset<Sprite>("YoyoMarkButtonSprite.png").DontUnload();
            CustomMain.customZips.TOR_Logo = ZipBundleStream.LoadAsset<Sprite>("TOR_Logo.png").DontUnload();
            CustomMain.customZips.VestButton = ZipBundleStream.LoadAsset<Sprite>("VestButton.png").DontUnload();
            CustomMain.customZips.SheriffKillButton = ZipBundleStream.LoadAsset<Sprite>("SheriffKillButton.png").DontUnload();
            CustomMain.customZips.CupidButton = ZipBundleStream.LoadAsset<Sprite>("CupidButton.png").DontUnload();
            CustomMain.customZips.InfectButton = ZipBundleStream.LoadAsset<Sprite>("InfectButton.png").DontUnload();
        }
    }
}