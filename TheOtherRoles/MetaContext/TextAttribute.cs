using TMPro;
using UnityEngine;

namespace TheOtherRoles.MetaContext;

public class TextAttribute
{
    public static readonly TextAttribute TitleAttr = new()
    {
        Alignment = TextAlignmentOptions.Left,
        Styles = FontStyles.Bold,
        FontMaxSize = 3f,
        FontMinSize = 1f,
        FontSize = 3f
    };

    public static readonly TextAttribute ContentAttr = new()
    {
        Alignment = TextAlignmentOptions.TopLeft,
        Styles = FontStyles.Normal,
        FontMaxSize = 1.5f,
        FontMinSize = 0.5f,
        FontSize = 1.5f,
        Size = new Vector2(8f, 2f)
    };

    public static readonly TextAttribute NormalAttr = new()
    {
        Alignment = TextAlignmentOptions.Center,
        Styles = FontStyles.Normal,
        FontMaxSize = 2f,
        FontMinSize = 1f,
        FontSize = 1.8f,
        Size = new Vector2(1.7f, 0.3f)
    };

    public static readonly TextAttribute NormalAttrLeft = new(NormalAttr) { Alignment = TextAlignmentOptions.Left };

    public static readonly TextAttribute BoldAttr = new()
    {
        Alignment = TextAlignmentOptions.Center,
        Styles = FontStyles.Bold,
        FontMaxSize = 2f,
        FontMinSize = 1f,
        FontSize = 1.8f,
        Size = new Vector2(1.7f, 0.3f)
    };

    public static readonly TextAttribute BoldAttrLeft = new(BoldAttr) { Alignment = TextAlignmentOptions.Left };

    public TextAttribute()
    {
    }

    public TextAttribute(TextAttribute orig)
    {
        Color = orig.Color;
        Alignment = orig.Alignment;
        Styles = orig.Styles;
        FontSize = orig.FontSize;
        FontMinSize = orig.FontMinSize;
        FontMaxSize = orig.FontMaxSize;
        AllowAutoSizing = orig.AllowAutoSizing;
        Size = orig.Size;
        FontMaterial = orig.FontMaterial;
    }

    public Color Color { get; set; } = Color.white;
    public TextAlignmentOptions Alignment { get; set; } = TextAlignmentOptions.Center;
    public FontStyles Styles { get; set; } = FontStyles.Normal;
    public Material FontMaterial { get; set; }
    public TMP_FontAsset Font { get; set; } = null;
    public float FontMinSize { get; set; } = 0.6f;
    public float FontMaxSize { get; set; } = 2f;
    public float FontSize { get; set; } = 1.5f;
    public bool AllowAutoSizing { get; set; } = true;
    public Vector2 Size { get; set; } = new(3f, 0.5f);

    public TextAttribute EditFontSize(float size)
    {
        return EditFontSize(size, size, size);
    }

    public TextAttribute EditFontSize(float size, float min, float max)
    {
        FontMaxSize = max;
        FontMinSize = min;
        FontSize = size;
        return this;
    }

    public void Reflect(TextMeshPro text)
    {
        text.color = Color;
        text.alignment = Alignment;
        text.fontStyle = Styles;
        text.fontSize = FontSize;
        text.fontSizeMin = FontMinSize;
        text.fontSizeMax = FontMaxSize;
        text.enableAutoSizing = AllowAutoSizing;
        text.rectTransform.sizeDelta = Size;
        text.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        text.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        text.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        if (Font != null) text.font = Font;
        if (FontMaterial != null) text.fontMaterial = FontMaterial;
    }

    public TextAttribute AlterColor(Color color)
    {
        return new TextAttribute(this) { Color = color };
    }

    public TextAttribute AlterAutoSizing(bool allowAutoSizing)
    {
        return new TextAttribute(this) { AllowAutoSizing = allowAutoSizing };
    }
}