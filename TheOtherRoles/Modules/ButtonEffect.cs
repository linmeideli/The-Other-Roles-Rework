using System.Collections.Generic;
using Il2CppSystem;
using Rewired;
using TheOtherRoles.MetaContext;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TheOtherRoles.Modules;

public static class ButtonEffect
{
    public enum ActionIconType
    {
        NonClickAction,
        InfoAction
    }

    private static readonly Image keyBindBackgroundSprite =
        SpriteLoader.FromResource("TheOtherRoles.Resources.KeyBindBackground.png", 100f);

    private static readonly Image mouseDisableActionSprite =
        SpriteLoader.FromResource("TheOtherRoles.Resources.MouseActionDisableIcon.png", 100f);

    private static readonly Image infoActionSprite =
        SpriteLoader.FromResource("TheOtherRoles.Resources.ButtonInfoIcon.png", 100f);

    private static readonly IDividedSpriteLoader textureUsesIconsSprite =
        XOnlyDividedSpriteLoader.FromResource("TheOtherRoles.Resources.UsesIcon.png", 120f, 10);

    public static GameObject AddKeyGuide(GameObject button, KeyCode key, Vector2 pos, bool removeExistingGuide,
        string action = null)
    {
        if (removeExistingGuide)
            button.gameObject.ForEachChild((Action<GameObject>)(obj =>
            {
                if (obj.name == "HotKeyGuide") Object.Destroy(obj);
            }));

        Sprite numSprite = null;
        if (KeyCodeInfo.AllKeyInfo.ContainsKey(key)) numSprite = KeyCodeInfo.AllKeyInfo[key].Sprite;
        if (numSprite == null) return null;

        GameObject obj = new()
        {
            name = "HotKeyGuide"
        };
        obj.transform.SetParent(button.transform);
        obj.layer = button.layer;
        var renderer = obj.AddComponent<SpriteRenderer>();
        renderer.transform.localPosition = (Vector3)pos + new Vector3(0f, 0f, -10f);
        renderer.sprite = keyBindBackgroundSprite.GetSprite();

        GameObject numObj = new()
        {
            name = "HotKeyText"
        };
        numObj.transform.SetParent(obj.transform);
        numObj.layer = button.layer;
        renderer = numObj.AddComponent<SpriteRenderer>();
        renderer.transform.localPosition = new Vector3(0, 0, -1f);
        renderer.sprite = numSprite;

        SetHintOverlay(obj, key, action);

        return obj;
    }

    public static GameObject SetKeyGuide(GameObject button, KeyCode key, bool removeExistingGuide = true,
        string action = null)
    {
        return AddKeyGuide(button, key, new Vector2(0.48f, 0.48f), removeExistingGuide, action);
    }

    public static void SetHintOverlay(GameObject gameObj, KeyCode keyCode, string action = null)
    {
        var button = gameObj.SetUpButton();
        var collider = gameObj.AddComponent<CircleCollider2D>();
        collider.isTrigger = true;
        collider.radius = 0.125f;
        button.OnMouseOver.AddListener((System.Action)(() =>
        {
            var str = keyCode != KeyCode.None
                ? string.Format("buttonsDescription".Translate(), KeyCodeInfo.GetKeyDisplayName(keyCode))
                : "";
            if (action != null)
            {
                if (str.Length > 0) str += "<br>";
                str += "<line-indent=0.8em>" + action;
            }

            TORGUIManager.Instance.SetHelpContext(button, str);
        }));
        button.OnMouseOut.AddListener((System.Action)(() => TORGUIManager.Instance.HideHelpContextIf(button)));
    }

    public static GameObject SetMouseActionIcon(GameObject button, bool show, string action = "mouseClick",
        bool atBottom = true, ActionIconType actionType = ActionIconType.NonClickAction)
    {
        if (!show)
        {
            button.gameObject.ForEachChild((Action<GameObject>)(obj =>
            {
                if (obj.name == "MouseAction") Object.Destroy(obj);
            }));
            return null;
        }

        {
            GameObject obj = new()
            {
                name = "MouseAction"
            };
            obj.transform.SetParent(button.transform);
            obj.layer = button.layer;
            var renderer = obj.AddComponent<SpriteRenderer>();
            renderer.transform.localPosition = new Vector3(0.48f, atBottom ? -0.29f : 0.48f, -10f);
            renderer.sprite = actionType == ActionIconType.NonClickAction
                ? mouseDisableActionSprite.GetSprite()
                : infoActionSprite.GetSprite();

            if (action != null) SetHintOverlay(obj, KeyCode.None, action);

            return obj;
        }
    }

    public static GameObject ShowUsesIcon(this ActionButton button)
    {
        var template = HudManager.Instance.AbilityButton.transform.GetChild(2);
        var usesObject = Object.Instantiate(template.gameObject);
        usesObject.transform.SetParent(button.gameObject.transform);
        usesObject.transform.localScale = template.localScale;
        usesObject.transform.localPosition = template.localPosition * 1.2f;
        return usesObject;
    }

    public static GameObject ShowUsesIcon(this ActionButton button, int iconVariation, out TextMeshPro text)
    {
        var result = ShowUsesIcon(button);
        var renderer = result.GetComponent<SpriteRenderer>();
        renderer.sprite = textureUsesIconsSprite.GetSprite(iconVariation);
        text = result.transform.GetChild(0).GetComponent<TextMeshPro>();
        text.transform.localScale *= 0.85f;
        text.transform.SetLocalY(text.transform.localPosition.y - 0.01f);
        return result;
    }

    public static void ShowVanillaKeyGuide(this HudManager manager)
    {
        if (manager == null) return;

        //ボタンのガイドを表示
        var keyboardMap = ReInput.mapping.GetKeyboardMapInstanceSavedOrDefault(0, 0, 0);
        Il2CppReferenceArray<ActionElementMap> actionArray;
        ActionElementMap actionMap;

        //マップ
        actionArray = keyboardMap.GetButtonMapsWithAction(4);
        if (actionArray.Count > 0)
        {
            actionMap = actionArray[0];
            SetKeyGuide(manager.SabotageButton.gameObject, actionMap.keyCode);
        }

        //使用
        actionArray = keyboardMap.GetButtonMapsWithAction(6);
        if (actionArray.Count > 0)
        {
            actionMap = actionArray[0];
            SetKeyGuide(manager.UseButton.gameObject, actionMap.keyCode);
            SetKeyGuide(manager.PetButton.gameObject, actionMap.keyCode);
        }

        //レポート
        actionArray = keyboardMap.GetButtonMapsWithAction(7);
        if (actionArray.Count > 0)
        {
            actionMap = actionArray[0];
            SetKeyGuide(manager.ReportButton.gameObject, actionMap.keyCode);
        }

        //キル
        actionArray = keyboardMap.GetButtonMapsWithAction(8);
        if (actionArray.Count > 0)
        {
            actionMap = actionArray[0];
            SetKeyGuide(manager.KillButton.gameObject, actionMap.keyCode);
        }

        //ベント
        actionArray = keyboardMap.GetButtonMapsWithAction(50);
        if (actionArray.Count > 0)
        {
            actionMap = actionArray[0];
            SetKeyGuide(manager.ImpostorVentButton.gameObject, actionMap.keyCode);
        }
    }

    public class KeyCodeInfo
    {
        public static Dictionary<KeyCode, KeyCodeInfo> AllKeyInfo = new();

        static KeyCodeInfo()
        {
            DividedSpriteLoader spriteLoader;
            spriteLoader =
                DividedSpriteLoader.FromResource("TheOtherRoles.Resources.KeyBindCharacters0.png", 100f, 18, 19, true);
            new KeyCodeInfo(KeyCode.Tab, "Tab", spriteLoader, 0);
            new KeyCodeInfo(KeyCode.Space, "Space", spriteLoader, 1);
            new KeyCodeInfo(KeyCode.Comma, "<", spriteLoader, 2);
            new KeyCodeInfo(KeyCode.Period, ">", spriteLoader, 3);
            spriteLoader =
                DividedSpriteLoader.FromResource("TheOtherRoles.Resources.KeyBindCharacters1.png", 100f, 18, 19, true);
            for (var key = KeyCode.A; key <= KeyCode.Z; key++)
                new KeyCodeInfo(key, ((char)('A' + key - KeyCode.A)).ToString(), spriteLoader, key - KeyCode.A);
            spriteLoader =
                DividedSpriteLoader.FromResource("TheOtherRoles.Resources.KeyBindCharacters2.png", 100f, 18, 19, true);
            for (var i = 0; i < 15; i++)
                new KeyCodeInfo(KeyCode.F1 + i, "F" + (i + 1), spriteLoader, i);
            spriteLoader =
                DividedSpriteLoader.FromResource("TheOtherRoles.Resources.KeyBindCharacters3.png", 100f, 18, 19, true);
            new KeyCodeInfo(KeyCode.RightShift, "RShift", spriteLoader, 0);
            new KeyCodeInfo(KeyCode.LeftShift, "LShift", spriteLoader, 1);
            new KeyCodeInfo(KeyCode.RightControl, "RControl", spriteLoader, 2);
            new KeyCodeInfo(KeyCode.LeftControl, "LControl", spriteLoader, 3);
            new KeyCodeInfo(KeyCode.RightAlt, "RAlt", spriteLoader, 4);
            new KeyCodeInfo(KeyCode.LeftAlt, "LAlt", spriteLoader, 5);
            spriteLoader =
                DividedSpriteLoader.FromResource("TheOtherRoles.Resources.KeyBindCharacters4.png", 100f, 18, 19, true);
            for (var i = 0; i < 6; i++)
                new KeyCodeInfo(KeyCode.Mouse1 + i,
                    "Mouse " + (i == 0 ? "Right" : i == 1 ? "Middle" : (i + 1).ToString()), spriteLoader, i);
            spriteLoader =
                DividedSpriteLoader.FromResource("TheOtherRoles.Resources.KeyBindCharacters5.png", 100f, 18, 19, true);
            for (var i = 0; i < 10; i++)
                new KeyCodeInfo(KeyCode.Alpha0 + i, "0" + (i + 1), spriteLoader, i);
        }

        public KeyCodeInfo(KeyCode keyCode, string translationKey, DividedSpriteLoader spriteLoader, int num)
        {
            this.keyCode = keyCode;
            TranslationKey = translationKey;
            textureHolder = spriteLoader;
            this.num = num;

            AllKeyInfo.Add(keyCode, this);
        }

        public KeyCode keyCode { get; private set; }
        public DividedSpriteLoader textureHolder { get; }
        public int num { get; }
        public string TranslationKey { get; }

        public Sprite Sprite => textureHolder.GetSprite(num);

        public static string GetKeyDisplayName(KeyCode keyCode)
        {
            if (keyCode == KeyCode.Return)
                return "Return";
            if (AllKeyInfo.TryGetValue(keyCode, out var val)) return val.TranslationKey;
            return null;
        }
    }
}