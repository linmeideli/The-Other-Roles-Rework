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
            var resourceAudioZipBundleStream2 = dll.GetManifestResourceStream("TheOtherRoles.Resources.IntroAnimation.intro");
            var resourceAudioZipBundleStream3 = dll.GetManifestResourceStream("TheOtherRoles.Resources.SoundEffects.toraudio");
            var ZipBundleStream = AssetBundle.LoadFromMemory(resourceAudioZipBundleStream.ReadFully());
            var ZipBundleStream2 = AssetBundle.LoadFromMemory(resourceAudioZipBundleStream2.ReadFully());
            var ZipBundleStream3 = AssetBundle.LoadFromMemory(resourceAudioZipBundleStream3.ReadFully());
            //音频有需要了再添加     
            //Sprite
            CustomMain.customZips.RightPanelCloseButton = ZipBundleStream.LoadAsset<Sprite>("RightPanelCloseButton.png").DontUnload();
            CustomMain.customZips.Alert = ZipBundleStream.LoadAsset<Sprite>("Alert.png").DontUnload();
            CustomMain.customZips.NoAlert = ZipBundleStream.LoadAsset<Sprite>("NoAlert.png").DontUnload();
           
          
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
            CustomMain.customZips.bombDefused = ZipBundleStream3.LoadAsset<AudioClip>("bombDefused.ogg").DontUnload();
            CustomMain.customZips.bombExplosion = ZipBundleStream3.LoadAsset<AudioClip>("bombExplosion.ogg").DontUnload();
            CustomMain.customZips.bombFuseBurning = ZipBundleStream3.LoadAsset<AudioClip>("bombFuseBurning.ogg").DontUnload();
            CustomMain.customZips.bombTick = ZipBundleStream3.LoadAsset<AudioClip>("bombTick.ogg").DontUnload(); CustomMain.customZips.arsonistDouse = ZipBundleStream3.LoadAsset<AudioClip>("arsonistDouse.ogg").DontUnload();
            CustomMain.customZips.cleanerClean = ZipBundleStream3.LoadAsset<AudioClip>("cleanerClean.ogg").DontUnload();
            CustomMain.customZips.deputyHandcuff = ZipBundleStream3.LoadAsset<AudioClip>("deputyHandcuff.ogg").DontUnload();
            CustomMain.customZips.engineerRepair = ZipBundleStream3.LoadAsset<AudioClip>("engineerRepair.ogg").DontUnload();
            CustomMain.customZips.eraserErase = ZipBundleStream3.LoadAsset<AudioClip>("eraserErase.ogg").DontUnload();
            CustomMain.customZips.fail = ZipBundleStream3.LoadAsset<AudioClip>("fail.ogg").DontUnload();
            CustomMain.customZips.garlic = ZipBundleStream3.LoadAsset<AudioClip>("garlic.ogg").DontUnload();
            CustomMain.customZips.hackerHack = ZipBundleStream3.LoadAsset<AudioClip>("hackerHack.ogg").DontUnload(); CustomMain.customZips.arsonistDouse = ZipBundleStream3.LoadAsset<AudioClip>("arsonistDouse.ogg").DontUnload();
            CustomMain.customZips.jackalSidekick = ZipBundleStream3.LoadAsset<AudioClip>("jackalSidekick.ogg").DontUnload();
            CustomMain.customZips.knockKnock = ZipBundleStream3.LoadAsset<AudioClip>("knockKnock.ogg").DontUnload();
            CustomMain.customZips.lighterLight = ZipBundleStream3.LoadAsset<AudioClip>("lighterLight.ogg").DontUnload();
            CustomMain.customZips.medicShield = ZipBundleStream3.LoadAsset<AudioClip>("medicShield.ogg").DontUnload(); CustomMain.customZips.arsonistDouse = ZipBundleStream3.LoadAsset<AudioClip>("arsonistDouse.ogg").DontUnload();
            CustomMain.customZips.mediumAsk = ZipBundleStream3.LoadAsset<AudioClip>("mediumAsk.ogg").DontUnload();
            CustomMain.customZips.morphlingMorph = ZipBundleStream3.LoadAsset<AudioClip>("morphlingMorph.ogg").DontUnload();
            CustomMain.customZips.morphlingSample = ZipBundleStream3.LoadAsset<AudioClip>("morphlingSample.ogg").DontUnload();
            CustomMain.customZips.portalUse = ZipBundleStream3.LoadAsset<AudioClip>("portalUse.ogg").DontUnload();
            CustomMain.customZips.pursuerBlank = ZipBundleStream3.LoadAsset<AudioClip>("pursuerBlank.ogg").DontUnload();
            CustomMain.customZips.securityGuardPlaceCam = ZipBundleStream3.LoadAsset<AudioClip>("securityGuardPlaceCam.ogg").DontUnload();
            CustomMain.customZips.shifterShift = ZipBundleStream3.LoadAsset<AudioClip>("shifterShift.ogg").DontUnload();
            CustomMain.customZips.timemasterShield = ZipBundleStream3.LoadAsset<AudioClip>("timemasterShield.ogg").DontUnload();
            CustomMain.customZips.trackerTrackCorpses = ZipBundleStream3.LoadAsset<AudioClip>("trackerTrackCorpses.ogg").DontUnload();
            CustomMain.customZips.trackerTrackPlayer = ZipBundleStream3.LoadAsset<AudioClip>("trackerTrackPlayer.ogg").DontUnload();
            CustomMain.customZips.trapperTrap = ZipBundleStream3.LoadAsset<AudioClip>("trapperTrap.ogg").DontUnload();
            CustomMain.customZips.tricksterPlaceBox = ZipBundleStream3.LoadAsset<AudioClip>("tricksterPlaceBox.ogg").DontUnload();
            CustomMain.customZips.tricksterUseBoxVent = ZipBundleStream3.LoadAsset<AudioClip>("tricksterUseBoxVent.ogg").DontUnload();
            CustomMain.customZips.vampireBite = ZipBundleStream3.LoadAsset<AudioClip>("vampireBite.ogg").DontUnload();
            CustomMain.customZips.vultureEat = ZipBundleStream3.LoadAsset<AudioClip>("vultureEat.ogg").DontUnload();
            CustomMain.customZips.warlockCurse = ZipBundleStream3.LoadAsset<AudioClip>("warlockCurse.ogg").DontUnload();
            CustomMain.customZips.witchSpell = ZipBundleStream3.LoadAsset<AudioClip>("witchSpell.ogg").DontUnload();
           
        }
        //广告位招租 CustomMain.customZips.招租 = ZipBundleStream2/3.LoadAsset<格式>("招租.格式").DontUnload();
    }
}