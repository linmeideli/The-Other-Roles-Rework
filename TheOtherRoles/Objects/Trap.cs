using System;
using System.Collections.Generic;
using System.Linq;
using Hazel;
using TheOtherRoles.Utilities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TheOtherRoles.Objects;

internal class Trap
{
    public static List<Trap> traps = new();
    public static Dictionary<byte, Trap> trapPlayerIdMap = new();

    private static int instanceCounter;

    private static Sprite trapSprite;
    private readonly Arrow arrow = new(Color.blue);
    private readonly int neededCount = Trapper.trapCountToReveal;
    public int instanceId;
    public bool revealed;
    public GameObject trap;
    public List<byte> trappedPlayer = new();
    public bool triggerable;
    private int usedCount;

    public Trap(Vector2 p)
    {
        trap = new GameObject("Trap") { layer = 11 };
        trap.AddSubmergedComponent(SubmergedCompatibility.Classes.ElevatorMover);
        var position = new Vector3(p.x, p.y, p.y / 1000 + 0.001f); // just behind player
        trap.transform.position = position;
        neededCount = Trapper.trapCountToReveal;

        var trapRenderer = trap.AddComponent<SpriteRenderer>();
        trapRenderer.sprite = getTrapSprite();
        trap.SetActive(false);
        if (PlayerControl.LocalPlayer.PlayerId == Trapper.trapper.PlayerId) trap.SetActive(true);
        trapRenderer.color = Color.white * new Vector4(1, 1, 1, 0.5f);
        instanceId = ++instanceCounter;
        traps.Add(this);
        arrow.Update(position);
        arrow.arrow.SetActive(false);
        FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(5, new Action<float>(x =>
        {
            if (x == 1f)
            {
                triggerable = true;
                trapRenderer.color = Color.white;
            }
        })));
    }

    public static Sprite getTrapSprite()
    {
        if (trapSprite) return trapSprite;
        trapSprite = Helpers.loadSpriteFromResources("Trapper_Trap_Ingame.png", 300f);
        return trapSprite;
    }

    public static void clearTraps()
    {
        foreach (var t in traps)
        {
            Object.Destroy(t.arrow.arrow);
            Object.Destroy(t.trap);
        }

        traps = new List<Trap>();
        trapPlayerIdMap = new Dictionary<byte, Trap>();
        instanceCounter = 0;
    }

    public static void clearRevealedTraps()
    {
        var trapsToClear = traps.FindAll(x => x.revealed);

        foreach (var t in trapsToClear)
        {
            traps.Remove(t);
            Object.Destroy(t.trap);
        }
    }

    public static void triggerTrap(byte playerId, byte trapId)
    {
        var t = traps.FirstOrDefault(x => x.instanceId == trapId);
        var player = Helpers.playerById(playerId);
        if (Trapper.trapper == null || t == null || player == null) return;
        var localIsTrapper = PlayerControl.LocalPlayer.PlayerId == Trapper.trapper.PlayerId;
        if (!trapPlayerIdMap.ContainsKey(playerId)) trapPlayerIdMap.Add(playerId, t);
        t.usedCount++;
        t.triggerable = false;
        if (playerId == PlayerControl.LocalPlayer.PlayerId || playerId == Trapper.trapper.PlayerId)
        {
            t.trap.SetActive(true);
            SoundEffectsManager.play("trapperTrap");
        }

        player.moveable = false;
        player.NetTransform.Halt();
        Trapper.playersOnMap.Add(player.PlayerId);
        if (localIsTrapper) t.arrow.arrow.SetActive(true);

        FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(Trapper.trapDuration,
            new Action<float>(p =>
            {
                if (p == 1f)
                {
                    player.moveable = true;
                    Trapper.playersOnMap.RemoveAll(x => x == player.PlayerId);
                    if (trapPlayerIdMap.ContainsKey(playerId)) trapPlayerIdMap.Remove(playerId);
                    t.arrow.arrow.SetActive(false);
                }
            })));

        if (t.usedCount == t.neededCount) t.revealed = true;

        t.trappedPlayer.Add(player.PlayerId);
        t.triggerable = true;
    }

    public static void Update()
    {
        if (Trapper.trapper == null) return;
        var player = PlayerControl.LocalPlayer;
        var vent = MapUtilities.CachedShipStatus.AllVents[0];
        var closestDistance = float.MaxValue;

        if (vent == null || player == null) return;
        var ud = vent.UsableDistance / 2;
        Trap target = null;
        foreach (var trap in traps)
        {
            if (trap.arrow.arrow.active) trap.arrow.Update();
            if (trap.revealed || !trap.triggerable || trap.trappedPlayer.Contains(player.PlayerId)) continue;
            if (player.inVent || !player.CanMove) continue;
            var distance = Vector2.Distance(trap.trap.transform.position, player.GetTruePosition());
            if (distance <= ud && distance < closestDistance)
            {
                closestDistance = distance;
                target = trap;
            }
        }

        if (target != null && player.PlayerId != Trapper.trapper.PlayerId && !player.Data.IsDead)
        {
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)CustomRPC.TriggerTrap, SendOption.Reliable);
            writer.Write(player.PlayerId);
            writer.Write(target.instanceId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            RPCProcedure.triggerTrap(player.PlayerId, (byte)target.instanceId);
        }


        if (!player.Data.IsDead || player.PlayerId == Trapper.trapper.PlayerId) return;
        foreach (var trap in traps)
            if (!trap.trap.active)
                trap.trap.SetActive(true);
    }
}