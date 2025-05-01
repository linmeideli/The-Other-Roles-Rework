using System;
using System.Linq;
using TMPro;
using UnityEngine;

namespace TheOtherRoles.MetaContext;

public enum FontAsset
{
    Gothic,
    GothicMasked,
    Oblong,
    OblongMasked,
    Prespawn,
    Barlow
}

public enum TextAlignment
{
    Left = TextAlignmentOptions.Left,
    Right = TextAlignmentOptions.Right,
    Center = TextAlignmentOptions.Center,
    TopLeft = TextAlignmentOptions.TopLeft,
    TopRight = TextAlignmentOptions.TopRight,
    Top = TextAlignmentOptions.Top,
    BottomLeft = TextAlignmentOptions.BottomLeft,
    BottomRight = TextAlignmentOptions.BottomRight,
    Bottom = TextAlignmentOptions.Bottom
}

[Flags]
internal enum AttributeTemplateFlag
{
    FontMask = 0b1111,
    FontStandard = 0b0001,
    FontOblong = 0b0010,

    AlignmentMask = 0b11 << 4,
    AlignmentLeft = 0b01 << 4,
    AlignmentCenter = 0b10 << 4,
    AlignmentRight = 0b11 << 4,

    MaterialMask = 0b1 << 6,
    MaterialNormal = 0b0 << 6,
    MaterialBared = 0b1 << 6,

    StyleMask = 0b1 << 7,
    StyleBold = 0b1 << 7,

    OtherSetingMask = 0b1 << 8,
    IsFlexible = 0b1 << 8
}

public enum AttributeParams
{
    StandardBared = AttributeTemplateFlag.FontStandard | AttributeTemplateFlag.AlignmentCenter |
                    AttributeTemplateFlag.MaterialBared | AttributeTemplateFlag.IsFlexible,

    StandardBaredBold = AttributeTemplateFlag.FontStandard | AttributeTemplateFlag.AlignmentCenter |
                        AttributeTemplateFlag.MaterialBared | AttributeTemplateFlag.IsFlexible |
                        AttributeTemplateFlag.StyleBold,

    StandardBaredLeft = AttributeTemplateFlag.FontStandard | AttributeTemplateFlag.AlignmentLeft |
                        AttributeTemplateFlag.MaterialBared | AttributeTemplateFlag.IsFlexible,

    StandardBaredBoldLeft = AttributeTemplateFlag.FontStandard | AttributeTemplateFlag.AlignmentLeft |
                            AttributeTemplateFlag.MaterialBared | AttributeTemplateFlag.IsFlexible |
                            AttributeTemplateFlag.StyleBold,

    Standard = AttributeTemplateFlag.FontStandard | AttributeTemplateFlag.AlignmentCenter |
               AttributeTemplateFlag.MaterialNormal | AttributeTemplateFlag.IsFlexible,

    StandardBold = AttributeTemplateFlag.FontStandard | AttributeTemplateFlag.AlignmentCenter |
                   AttributeTemplateFlag.MaterialNormal | AttributeTemplateFlag.IsFlexible |
                   AttributeTemplateFlag.StyleBold,

    StandardLeft = AttributeTemplateFlag.FontStandard | AttributeTemplateFlag.AlignmentLeft |
                   AttributeTemplateFlag.MaterialNormal | AttributeTemplateFlag.IsFlexible,

    StandardBoldLeft = AttributeTemplateFlag.FontStandard | AttributeTemplateFlag.AlignmentLeft |
                       AttributeTemplateFlag.MaterialNormal | AttributeTemplateFlag.IsFlexible |
                       AttributeTemplateFlag.StyleBold,

    StandardBaredNonFlexible = AttributeTemplateFlag.FontStandard | AttributeTemplateFlag.AlignmentCenter |
                               AttributeTemplateFlag.MaterialBared,

    StandardBaredBoldNonFlexible = AttributeTemplateFlag.FontStandard | AttributeTemplateFlag.AlignmentCenter |
                                   AttributeTemplateFlag.MaterialBared | AttributeTemplateFlag.StyleBold,

    StandardBaredLeftNonFlexible = AttributeTemplateFlag.FontStandard | AttributeTemplateFlag.AlignmentLeft |
                                   AttributeTemplateFlag.MaterialBared,

    StandardBaredBoldLeftNonFlexible = AttributeTemplateFlag.FontStandard | AttributeTemplateFlag.AlignmentLeft |
                                       AttributeTemplateFlag.MaterialBared | AttributeTemplateFlag.StyleBold,

    StandardNonFlexible = AttributeTemplateFlag.FontStandard | AttributeTemplateFlag.AlignmentCenter |
                          AttributeTemplateFlag.MaterialNormal,

    StandardBoldNonFlexible = AttributeTemplateFlag.FontStandard | AttributeTemplateFlag.AlignmentCenter |
                              AttributeTemplateFlag.MaterialNormal | AttributeTemplateFlag.StyleBold,

    StandardLeftNonFlexible = AttributeTemplateFlag.FontStandard | AttributeTemplateFlag.AlignmentLeft |
                              AttributeTemplateFlag.MaterialNormal,

    StandardBoldLeftNonFlexible = AttributeTemplateFlag.FontStandard | AttributeTemplateFlag.AlignmentLeft |
                                  AttributeTemplateFlag.MaterialNormal | AttributeTemplateFlag.StyleBold,

    OblongBared = AttributeTemplateFlag.FontOblong | AttributeTemplateFlag.AlignmentCenter |
                  AttributeTemplateFlag.MaterialBared,

    OblongBaredLeft = AttributeTemplateFlag.FontOblong | AttributeTemplateFlag.AlignmentLeft |
                      AttributeTemplateFlag.MaterialBared,

    Oblong = AttributeTemplateFlag.FontOblong | AttributeTemplateFlag.AlignmentCenter |
             AttributeTemplateFlag.MaterialNormal,

    OblongLeft = AttributeTemplateFlag.FontOblong | AttributeTemplateFlag.AlignmentLeft |
                 AttributeTemplateFlag.MaterialNormal
}

public interface CommunicableTextTag
{
    /// <summary>
    ///     翻訳キーを取得します。
    /// </summary>
    public string TranslationKey { get; }

    internal int Id { get; }
    internal string Text { get; }
}

public enum AttributeAsset
{
    /// <summary>
    ///     タイトル画面の縦長の文字です。実績確認画面のヘッダーなどで使われています。
    /// </summary>
    OblongHeader,

    /// <summary>
    ///     ボタン向けの固定サイズテキスト属性です。
    ///     Preset読み込み画面のReloadボタン、Save As...ボタンと同じ大きさです。
    /// </summary>
    StandardMediumMasked,

    /// <summary>
    ///     ボタン向けの固定サイズテキスト属性です。
    ///     Preset読み込み画面の各プリセットのボタンと同じ大きさです。
    /// </summary>
    StandardLargeWideMasked,

    /// <summary>
    ///     主にオーバーレイ向けの本文用可変サイズテキスト属性です。
    /// </summary>
    OverlayContent,

    /// <summary>
    ///     フリープレイの役職選択ボタンで使用されているテキスト属性です。
    /// </summary>
    MetaRoleButton,

    /// <summary>
    ///     主にオーバーレイ向けの見出し用可変サイズテキスト属性です。
    /// </summary>
    OverlayTitle,

    /// <summary>
    ///     SerializableDocumentのTextStyle"Title"で提供されているテキスト属性です。
    /// </summary>
    DocumentTitle,

    /// <summary>
    ///     SerializableDocumentのTextStyle"Standard"で提供されているテキスト属性です。
    /// </summary>
    DocumentStandard
}

[Flags]
public enum FontStyle
{
    Normal = FontStyles.Normal,
    Bold = FontStyles.Bold,
    Italic = FontStyles.Italic
}

public interface Font
{
    internal Material FontMaterial { get; }
    internal TMP_FontAsset Font { get; }
}

internal class StaticFont : Font
{
    public StaticFont(Material material, TMP_FontAsset font)
    {
        FontMaterial = material;
        Font = font;
    }

    public Material FontMaterial { get; init; }

    public TMP_FontAsset Font { get; init; }
}

public class FontSize
{
    public FontSize(float fontSize, float fontSizeMin, float fontSizeMax, bool allowAutoSizing = true)
    {
        FontSizeDefault = fontSize;
        FontSizeMin = fontSizeMin;
        FontSizeMax = fontSizeMax;
        AllowAutoSizing = allowAutoSizing;
    }

    public FontSize(float fontSize, bool allowAutoSizing = true) : this(fontSize, fontSize, fontSize, allowAutoSizing)
    {
    }

    internal float FontSizeDefault { get; private init; }
    internal float FontSizeMin { get; private init; }
    internal float FontSizeMax { get; private init; }
    internal bool AllowAutoSizing { get; private init; }
}

public class TextAttributes
{
    public TextAttributes(TextAlignment alignment, Font font, FontStyle style, FontSize fontSize, Size size,
        Color color, bool isFlexible)
    {
        Alignment = alignment;
        Font = font;
        Style = style;
        FontSize = fontSize;
        Size = size;
        Color = color;
        IsFlexible = isFlexible;
    }

    public TextAttributes(TextAttributes original)
    {
        Alignment = original.Alignment;
        Font = original.Font;
        Style = original.Style;
        FontSize = original.FontSize;
        Size = original.Size;
        Color = original.Color;
        IsFlexible = original.IsFlexible;
    }

    public TextAlignment Alignment { get; init; }
    public Font Font { get; init; }
    public FontStyle Style { get; init; }
    public FontSize FontSize { get; init; }
    public Size Size { get; init; }
    public Color Color { get; init; }
    public bool IsFlexible { get; init; }
}

public interface TextComponent
{
    public string GetString();
}

public class CombinedTextComponent : TextComponent
{
    private readonly TextComponent[] components;

    public CombinedTextComponent(params TextComponent[] components)
    {
        this.components = components;
    }

    public string GetString()
    {
        return string.Join<string>("", components.Select(c => c.GetString()));
    }
}

public struct Size
{
    public float Width;
    public float Height;

    public Size(float width, float height)
    {
        Width = width;
        Height = height;
    }

    internal Size(Vector2 size)
    {
        Width = size.x;
        Height = size.y;
    }

    internal Vector2 ToUnityVector()
    {
        return new Vector2(Width, Height);
    }
}

public struct FuzzySize
{
    public float? Width;
    public float? Height;

    public FuzzySize(float? width, float? height)
    {
        Width = width;
        Height = height;
        if (!Width.HasValue && !Height.HasValue) Width = 1f;
    }
}