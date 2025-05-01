using System.Collections.Generic;
using Reactor.Utilities.Extensions;
using UnityEngine;

namespace TheOtherRoles.Objects;

public class Silhouette
{
    public static List<Silhouette> silhouettes = new();


    private static Sprite SilhouetteSprite;
    private readonly SpriteRenderer renderer;
    private readonly bool visibleForEveryOne;
    public GameObject gameObject;
    public bool permanent;
    public float timeRemaining;

    public Silhouette(Vector3 p, float duration = 1f, bool visibleForEveryOne = true)
    {
        if (duration <= 0f) permanent = true;
        this.visibleForEveryOne = visibleForEveryOne;
        gameObject = new GameObject("Silhouette");
        gameObject.AddSubmergedComponent(SubmergedCompatibility.Classes.ElevatorMover);
        //Vector3 position = new Vector3(p.x, p.y, PlayerControl.LocalPlayer.transform.localPosition.z + 0.001f); // just behind player
        var position = new Vector3(p.x, p.y, p.y / 1000f + 0.01f);
        gameObject.transform.position = position;
        gameObject.transform.localPosition = position;

        renderer = gameObject.AddComponent<SpriteRenderer>();
        renderer.sprite = getSilhouetteSprite();

        timeRemaining = duration;

        renderer.color = renderer.color.SetAlpha(Yoyo.SilhouetteVisibility);

        var visible = visibleForEveryOne || PlayerControl.LocalPlayer == Yoyo.yoyo ||
                      PlayerControl.LocalPlayer.Data.IsDead;

        gameObject.SetActive(visible);
        silhouettes.Add(this);
    }

    public static Sprite getSilhouetteSprite()
    {
        if (SilhouetteSprite) return SilhouetteSprite;
        SilhouetteSprite = Helpers.loadSpriteFromResources("Silhouette.png", 225f);
        return SilhouetteSprite;
    }

    public static void clearSilhouettes()
    {
        foreach (var sil in silhouettes)
            sil.gameObject.Destroy();
        silhouettes = new List<Silhouette>();
    }

    public static void UpdateAll()
    {
        foreach (var current in new List<Silhouette>(silhouettes))
        {
            current.timeRemaining -= Time.fixedDeltaTime;
            var visible = current.visibleForEveryOne || PlayerControl.LocalPlayer == Yoyo.yoyo ||
                          PlayerControl.LocalPlayer.Data.IsDead;
            current.gameObject.SetActive(visible);

            if (visible && current.timeRemaining > 0 && current.timeRemaining < 0.5)
            {
                var alphaRatio = current.timeRemaining / 0.5f;
                current.renderer.color = current.renderer.color.SetAlpha(Yoyo.SilhouetteVisibility * alphaRatio);
            }

            if (current.timeRemaining < 0 && !current.permanent)
            {
                current.gameObject.SetActive(false);
                Object.Destroy(current.gameObject);
                silhouettes.Remove(current);
            }
        }
    }
}