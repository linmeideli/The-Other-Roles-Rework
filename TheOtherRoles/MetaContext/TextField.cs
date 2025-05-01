using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using Twitch;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TheOtherRoles.MetaContext;

public class TextField : MonoBehaviour
{
    private static readonly List<TextField> allFields = new();

    private static TextField validField;

    public static readonly Predicate<char> TokenPredicate =
        c => ('a' <= c && c <= 'z') || ('A' <= c && c <= 'Z') || ('0' <= c && c <= '9');

    public static readonly Predicate<char> IdPredicate = c => TokenPredicate(c) || c is '.';
    public static readonly Predicate<char> NameSpacePredicate = c => TokenPredicate(c) || c is '.' || c is ':';
    public static readonly Predicate<char> IntegerPredicate = c => '0' <= c && c <= '9';
    public static readonly Predicate<char> NumberPredicate = c => ('0' <= c && c <= '9') || c is '.';
    public static readonly Predicate<char> JsonStringPredicate = c => !(c is '\\' or '"');

    public int MaxLines = 1;
    public bool AllowTab;
    private int cursor;
    private float cursorTimer;

    private bool dirtyFlag;

    //trueを返すとフォーカスを失います。
    public Predicate<string> EnterAction;
    private string hint;

    public Predicate<char> InputPredicate;

    private string lastCompoStr = "";

    //有効になっても操作できない時間
    private float lockedTime;
    public Action<string> LostFocusAction;
    private TextMeshPro myCursor = null!;

    private TextMeshPro myText = null!;
    private int selectingBegin = -1;
    public Action<string> UpdateAction;

    static TextField()
    {
        ClassInjector.RegisterTypeInIl2Cpp<TextField>();
    }

    public string Text { get; private set; } = "";

    public bool AllowMultiLine => MaxLines >= 2;

    public static bool AnyoneValid => validField?.IsValid ?? false;

    public bool IsSelecting => selectingBegin != -1;


    private bool PressingShift => Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

    public bool IsValid => validField == this && validField;

    public void Awake()
    {
        allFields.Add(this);

        myText = Instantiate(VanillaAsset.StandardTextPrefab, transform);
        myCursor = Instantiate(VanillaAsset.StandardTextPrefab, transform);

        myText.sortingOrder = 15;
        myCursor.sortingOrder = 15;

        myText.transform.localPosition = new Vector3(0, 0, -1f);
        myCursor.transform.localPosition = new Vector3(0, 0, -1f);

        myText.outlineWidth = 0f;
        myCursor.outlineWidth = 0f;

        myText.text = "";
        myCursor.text = "|";
        myCursor.ForceMeshUpdate();

        dirtyFlag = true;

        SetSize(new Vector2(4f, 0.5f), 2f);
    }

    public void Update()
    {
        if (this != validField)
        {
            myCursor.gameObject.SetActive(false);
            return;
        }

        if (lockedTime > 0f)
        {
            lockedTime -= Time.deltaTime;
            myCursor.gameObject.SetActive(false);
            return;
        }

        if (!AllowTab && Input.GetKeyDown(KeyCode.Tab))
        {
            ChangeField(!PressingShift);
            return;
        }

        if (!AllowMultiLine && Input.GetKeyDown(KeyCode.Return))
        {
            if (EnterAction?.Invoke(Text) ?? true) ChangeFocus(null);
            return;
        }

        var requireUpdate = InputText(Input.inputString);
        if (dirtyFlag)
        {
            requireUpdate = true;
            dirtyFlag = false;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveCursorLine(false, PressingShift);
            ShowCursor();
            requireUpdate = true;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveCursorLine(true, PressingShift);
            ShowCursor();
            requireUpdate = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveCursor(false, PressingShift);
            ShowCursor();
            requireUpdate = true;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveCursor(true, PressingShift);
            ShowCursor();
            requireUpdate = true;
        }

        if (Input.GetKeyDown(KeyCode.Home))
        {
            if (!IsSelecting && PressingShift) selectingBegin = cursor;
            while (cursor > 0 && Text[cursor - 1] != '\r') cursor--;
            ShowCursor();
            requireUpdate = true;
        }

        if (Input.GetKeyDown(KeyCode.End))
        {
            if (!IsSelecting && PressingShift) selectingBegin = cursor;
            while (cursor < Text.Length && Text[cursor] != '\r') cursor++;
            ShowCursor();
            requireUpdate = true;
        }

        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                selectingBegin = 0;
                cursor = Text.Length;
                requireUpdate = true;
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                CopyText();
                requireUpdate = true;
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                if (!IsSelecting)
                {
                    selectingBegin = 0;
                    cursor = Text.Length;
                }

                CopyText();
                RemoveCharacter(true);
                requireUpdate = true;
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                InputText(Helpers.GetClipboardString());
                requireUpdate = true;
            }
        }

        if (requireUpdate || lastCompoStr != Input.compositionString) UpdateTextMesh();

        cursorTimer -= Time.deltaTime;
        if (cursorTimer < 0f)
        {
            myCursor.gameObject.SetActive(!myCursor.gameObject.activeSelf);
            cursorTimer = 0.65f;
        }
    }

    public void OnDisable()
    {
        if (validField == this) ChangeFocus(null);
    }

    public void OnDestroy()
    {
        allFields.Remove(this);
    }

    public static TextField EditFirstField()
    {
        if (allFields.Count == 0) return null;
        var field = allFields.FirstOrDefault(f => f.gameObject.active);
        ChangeFocus(field);
        return field;
    }

    public static TextField EditLastField()
    {
        if (allFields.Count == 0) return null;
        var field = allFields.LastOrDefault(f => f.gameObject.active);
        ChangeFocus(field);
        return field;
    }

    public static TextField ChangeField(bool increament = true)
    {
        if (validField == null) return null;

        var index = allFields.IndexOf(validField);
        if (index == -1) return null;

        while (true)
        {
            index += increament ? 1 : -1;
            if (index < 0 || index >= allFields.Count) break;

            if (allFields[index].gameObject.active)
            {
                ChangeFocus(allFields[index]);
                return allFields[index];
            }
        }

        return null;
    }

    public TextField GainFocus()
    {
        ChangeFocus(this);
        return this;
    }

    private bool InputText(string input)
    {
        if (InputPredicate != null)
            input = new string(input.Where(c => InputPredicate.Invoke(c) || c is '\r' or (char)0x08).ToArray());

        if (input.Length == 0) return false;

        {
            var i = 0;
            while (true)
            {
                if (i == input.Length) return false;
                if (input[i] == 0x08)
                    RemoveCharacter(false);
                if (input[i] == 0xFF)
                    RemoveCharacter(true);
                else
                    break;

                i++;
            }


            if (!AllowMultiLine) input = input.Replace("\r", "");
            if (!AllowTab) input = input.Replace("\t", " ");
            input = input.Substring(i).Replace(((char)0x08).ToString(), "").Replace(((char)0xFF).ToString(), "")
                .Replace("\0", "").Replace("\n", "");
        }

        ShowCursor();

        if (IsSelecting)
        {
            var minIndex = Math.Min(cursor, selectingBegin);
            Text = Text.Remove(minIndex, Math.Abs(cursor - selectingBegin)).Insert(minIndex, input);
            selectingBegin = -1;
            cursor = minIndex + input.Length;
        }
        else
        {
            Text = Text.Insert(cursor, input);
            cursor += input.Length;
        }

        //改行文字を制限
        var strings = Text.Split('\r');
        Text = "";
        for (var i = 0; i < strings.Length; i++)
        {
            if (i > 0 && i < MaxLines) Text += '\r';
            Text += strings[i];
        }

        cursor = Math.Clamp(cursor, 0, Text.Length);

        UpdateAction?.Invoke(Text);

        return true;
    }

    private void RemoveAll()
    {
        Text = "";
        cursor = 0;
        selectingBegin = -1;
    }

    private void RemoveCharacter(bool isDelete)
    {
        if (IsSelecting)
        {
            Text = Text.Remove(Math.Min(cursor, selectingBegin), Math.Abs(cursor - selectingBegin));
            cursor = Math.Min(cursor, selectingBegin);
            selectingBegin = -1;
        }
        else
        {
            if (!isDelete && cursor > 0)
            {
                Text = Text.Remove(cursor - 1, 1);
                cursor--;
            }
            else if (isDelete && cursor < Text.Length)
            {
                Text = Text.Remove(cursor, 1);
            }
        }
    }

    private void MoveCursorLine(bool moveForward, bool shift)
    {
        try
        {
            var myLineBegining = cursor;
            while (myLineBegining > 0 && Text[myLineBegining - 1] != '\r') myLineBegining--;
            var targetLineBegining = moveForward ? cursor : myLineBegining - 1;
            while (targetLineBegining > 0 && targetLineBegining < Text.Length && Text[targetLineBegining - 1] != '\r')
                targetLineBegining += moveForward ? 1 : -1;

            var dis = cursor - myLineBegining;
            var result = targetLineBegining;
            for (var i = 0; i < dis; i++)
                if (Text[result] != '\r' && result + 1 < Text.Length)
                    result++;

            if (IsSelecting && !shift) selectingBegin = -1;
            if (shift && !IsSelecting) selectingBegin = cursor;
            cursor = result;
            if (selectingBegin == cursor) selectingBegin = -1;
        }
        catch
        {
        }
    }

    private void MoveCursor(bool moveForward, bool shift)
    {
        if (IsSelecting && !shift)
        {
            if (moveForward) cursor = Math.Max(cursor, selectingBegin);
            else cursor = Math.Min(cursor, selectingBegin);
            selectingBegin = -1;
        }
        else
        {
            if (shift && !IsSelecting) selectingBegin = cursor;
            cursor = Math.Clamp(cursor + (moveForward ? 1 : -1), 0, Text.Length);

            if (selectingBegin == cursor) selectingBegin = -1;
        }
    }

    private int ConsiderComposition(int index, string compStr)
    {
        if (index >= cursor) return index + compStr.Length;
        return index;
    }

    private int GetCursorLineNum(int index)
    {
        if (index >= myText.textInfo.characterInfo.Length) index = myText.textInfo.characterInfo.Length - 1;
        return myText.textInfo.characterInfo[index].lineNumber;
    }

    //改行文字を含むindex
    private float GetCursorX(int index)
    {
        //最初あるいは直前の文字と行が違う場合
        if (index <= 0 || (index < myText.textInfo.characterInfo.Count &&
                           myText.textInfo.characterInfo[index - 1].lineNumber !=
                           myText.textInfo.characterInfo[index].lineNumber))
            return myText.rectTransform.rect.min.x;
        return myText.textInfo.characterInfo[index - 1].xAdvance;
    }

    private void UpdateTextMesh()
    {
        lastCompoStr = Input.compositionString;
        var compStr = lastCompoStr;

        if (Text.Length > 0 || compStr.Length > 0)
        {
            var str = Text.Insert(cursor, compStr);
            if (IsSelecting)
                str = str.Insert(ConsiderComposition(Math.Max(cursor, selectingBegin), compStr), "\\EMK")
                    .Insert(ConsiderComposition(Math.Min(cursor, selectingBegin), compStr), "\\BMK");

            str = Regex.Replace(str, "[<>]", "<noparse>$0</noparse>").Replace("\\EMK", "</mark>")
                .Replace("\\BMK", "<mark=#5F74A5AA>").Replace("\r", "<br>");

            myText.text = str + " ";
        }
        else
        {
            myText.text = hint;
            cursor = 0;
        }

        myText.ForceMeshUpdate();

        var visualCursor = ConsiderComposition(cursor, compStr);
        var lineNum = GetCursorLineNum(visualCursor);
        myCursor.transform.localPosition = new Vector3(GetCursorX(visualCursor),
            Text.Length == 0 ? 0f : myText.textInfo.lineInfo[lineNum].baseline - myCursor.textInfo.lineInfo[0].baseline,
            -1f);


        Vector2 compoPos = Helpers.WorldToScreenPoint(transform.position + new Vector3(GetCursorX(cursor), 0.15f, 0f),
            LayerMask.NameToLayer("UI"));
        compoPos.y = Screen.height - compoPos.y;
        Input.compositionCursorPos = compoPos;
    }

    public void SetHint(string hint)
    {
        this.hint = hint;
        if (Text.Length == 0) UpdateTextMesh();
    }

    public void SetText(string text)
    {
        RemoveAll();
        InputText(text);
        ShowCursor();
        UpdateTextMesh();
    }

    private TextAttribute GenerateAttribute(Vector2 size, float fontSize, TextAlignmentOptions alignment)
    {
        return new TextAttribute
        {
            Alignment = alignment,
            AllowAutoSizing = false,
            Color = Color.white,
            FontSize = fontSize,
            Size = size,
            Styles = FontStyles.Normal
        };
    }


    public void SetSize(Vector2 size, float fontSize, int maxLines = 1)
    {
        MaxLines = maxLines;
        GenerateAttribute(size, fontSize, AllowMultiLine ? TextAlignmentOptions.TopLeft : TextAlignmentOptions.Left)
            .Reflect(myText);
        GenerateAttribute(new Vector2(0.3f, size.y), fontSize,
            AllowMultiLine ? TextAlignmentOptions.Top : TextAlignmentOptions.Center).Reflect(myCursor);
        myText.font = VanillaAsset.VersionFont;
        myCursor.font = VanillaAsset.VersionFont;

        UpdateTextMesh();
    }

    private void CopyText()
    {
        if (!IsSelecting) return;

        ClipboardHelper.PutClipboardString(Text.Substring(Math.Min(cursor, selectingBegin),
            Math.Abs(cursor - selectingBegin)));
    }

    private void ShowCursor()
    {
        myCursor.gameObject.SetActive(validField == this);
        cursorTimer = 0.8f;
    }

    public static void ChangeFocus(TextField field)
    {
        if (field == validField) return;
        if (validField != null) validField.LoseFocus();
        validField = field;
        field?.GetFocus();
    }

    private void LoseFocus()
    {
        LostFocusAction?.Invoke(Text);
        Input.imeCompositionMode = IMECompositionMode.Off;
    }

    private void GetFocus()
    {
        Input.imeCompositionMode = IMECompositionMode.On;
        lockedTime = 0.1f;
        cursor = Text.Length;
        UpdateTextMesh();
    }

    public void AsMaskedText()
    {
        myText.fontMaterial = VanillaAsset.StandardMaskedFontMaterial;
        myCursor.fontMaterial = VanillaAsset.StandardMaskedFontMaterial;
        myText.font = VanillaAsset.StandardTextPrefab.font;
        myCursor.font = VanillaAsset.StandardTextPrefab.font;
    }
}

public class VanillaAsset
{
    public static bool loaded;
    private static Material oblongMaskedFontMaterial;
    private static TMP_FontAsset preSpawnFont;
    private static Material standardMaskedFontMaterial;
    private static TMP_FontAsset versionFont;

    private static TMP_FontAsset brookFont;
    public static Sprite CloseButtonSprite { get; private set; } = null!;
    public static TextMeshPro StandardTextPrefab { get; private set; } = null!;
    public static PlayerCustomizationMenu PlayerOptionsMenuPrefab { get; private set; } = null!;
    public static Sprite PopUpBackSprite { get; private set; } = null!;
    public static Sprite FullScreenSprite { get; private set; } = null!;
    public static Sprite TextButtonSprite { get; private set; } = null!;
    public static AudioClip HoverClip { get; private set; } = null!;
    public static AudioClip SelectClip { get; private set; } = null!;

    public static Material OblongMaskedFontMaterial
    {
        get
        {
            if (oblongMaskedFontMaterial == null)
                oblongMaskedFontMaterial = Helpers.FindAsset<Material>("Brook Atlas Material Masked");
            return oblongMaskedFontMaterial!;
        }
    }

    public static TMP_FontAsset PreSpawnFont
    {
        get
        {
            if (preSpawnFont == null) preSpawnFont = Helpers.FindAsset<TMP_FontAsset>("DIN_Pro_Bold_700 SDF")!;
            return preSpawnFont;
        }
    }

    public static Material StandardMaskedFontMaterial
    {
        get
        {
            if (standardMaskedFontMaterial == null)
                standardMaskedFontMaterial = Helpers.FindAsset<Material>("LiberationSans SDF - BlackOutlineMasked")!;
            return standardMaskedFontMaterial!;
        }
    }

    public static TMP_FontAsset VersionFont
    {
        get
        {
            if (versionFont == null) versionFont = Helpers.FindAsset<TMP_FontAsset>("Barlow-Medium SDF");
            return versionFont!;
        }
    }

    public static TMP_FontAsset BrookFont
    {
        get
        {
            if (brookFont == null) brookFont = Helpers.FindAsset<TMP_FontAsset>("Brook SDF")!;
            return brookFont;
        }
    }

    public static void LoadAssetsOnTitle()
    {
        var twitchPopUp = TwitchManager.Instance.transform.GetChild(0);
        PopUpBackSprite = twitchPopUp.GetChild(3).GetComponent<SpriteRenderer>().sprite;
        FullScreenSprite = twitchPopUp.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        CloseButtonSprite = Helpers.FindAsset<Sprite>("closeButton")!;
        TextButtonSprite = twitchPopUp.GetChild(2).GetComponent<SpriteRenderer>().sprite;

        StandardTextPrefab = Object.Instantiate(twitchPopUp.GetChild(1).GetComponent<TextMeshPro>(), null);
        StandardTextPrefab.gameObject.hideFlags = HideFlags.HideAndDontSave;
        Object.Destroy(StandardTextPrefab.spriteAnimator);
        Object.DontDestroyOnLoad(StandardTextPrefab.gameObject);
    }

    public static void PlayHoverSE()
    {
        SoundManager.Instance.PlaySound(HoverClip, false, 0.8f);
    }

    public static void LoadAssetAtInitialize()
    {
        if (loaded) return;
        loaded = true;
        HoverClip = Helpers.FindAsset<AudioClip>("UI_Hover")!;
        SelectClip = Helpers.FindAsset<AudioClip>("UI_Select")!;
        PlayerOptionsMenuPrefab = Helpers.FindAsset<PlayerCustomizationMenu>("LobbyPlayerCustomizationMenu")!;
    }

    public static Scroller GenerateScroller(Vector2 size, Transform transform, Vector3 scrollBarLocalPos,
        Transform target, FloatRange bounds, float scrollerHeight)
    {
        var barBack =
            GameObject.Instantiate(
                PlayerOptionsMenuPrefab.transform.GetChild(4).FindChild("UI_ScrollbarTrack").gameObject, transform);
        var bar = GameObject.Instantiate(
            PlayerOptionsMenuPrefab.transform.GetChild(4).FindChild("UI_Scrollbar").gameObject, transform);
        barBack.transform.localPosition = scrollBarLocalPos + new Vector3(0.12f, 0f, 0f);
        bar.transform.localPosition = scrollBarLocalPos;

        var scrollBar = bar.GetComponent<Scrollbar>();

        var scroller = Helpers.CreateObject<Scroller>("Scroller", transform, new Vector3(0, 0, 5));
        scroller.gameObject.AddComponent<BoxCollider2D>().size = size;

        scrollBar.parent = scroller;
        scrollBar.graphic = bar.GetComponent<SpriteRenderer>();
        scrollBar.trackGraphic = barBack.GetComponent<SpriteRenderer>();
        scrollBar.trackGraphic.size = new Vector2(scrollBar.trackGraphic.size.x, scrollerHeight);

        var ratio = scrollerHeight / 3.88f;

        scroller.Inner = target;
        scroller.SetBounds(bounds, null);
        scroller.allowY = true;
        scroller.allowX = false;
        scroller.ScrollbarYBounds = new FloatRange(-1.8f * ratio + scrollBarLocalPos.y + 0.4f,
            1.8f * ratio + scrollBarLocalPos.y - 0.4f);
        scroller.ScrollbarY = scrollBar;
        scroller.active = true;
        //scroller.Colliders = new Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppReferenceArray<Collider2D>(new Collider2D[] { hitBox });

        scroller.ScrollToTop();

        return scroller;
    }
}

public class Reference<T>
{
    public T Value { get; set; }

    public Reference<T> Set(T value)
    {
        Value = value;
        return this;
    }

    public IEnumerator Wait()
    {
        while (Value == null) yield return null;
        yield break;
    }
}