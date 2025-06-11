// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.CSSStyleItem
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class CSSStyleItem
{
  private string m_styleName;
  private Dictionary<CSSStyleItem.TextFormatKey, object> m_propertiesHash;
  private Dictionary<CSSStyleItem.TextFormatImportantKey, object> m_importantPropertiesHash;
  private CSSStyleItem.CssStyleType m_styleType;

  internal string StyleName
  {
    get => this.m_styleName;
    set => this.m_styleName = value;
  }

  internal CSSStyleItem.CssStyleType StyleType
  {
    get => this.m_styleType;
    set => this.m_styleType = value;
  }

  internal Dictionary<CSSStyleItem.TextFormatKey, object> PropertiesHash
  {
    get
    {
      if (this.m_propertiesHash == null)
        this.m_propertiesHash = new Dictionary<CSSStyleItem.TextFormatKey, object>();
      return this.m_propertiesHash;
    }
  }

  internal Dictionary<CSSStyleItem.TextFormatImportantKey, object> ImportantPropertiesHash
  {
    get
    {
      if (this.m_importantPropertiesHash == null)
        this.m_importantPropertiesHash = new Dictionary<CSSStyleItem.TextFormatImportantKey, object>();
      return this.m_importantPropertiesHash;
    }
  }

  internal object this[CSSStyleItem.TextFormatKey key]
  {
    get => !this.PropertiesHash.ContainsKey(key) ? (object) null : this.PropertiesHash[key];
    set
    {
      if (this.PropertiesHash.ContainsKey(key))
        this.PropertiesHash[key] = value;
      else
        this.PropertiesHash.Add(key, value);
    }
  }

  internal object this[CSSStyleItem.TextFormatImportantKey key]
  {
    get
    {
      return !this.ImportantPropertiesHash.ContainsKey(key) ? (object) null : this.ImportantPropertiesHash[key];
    }
    set
    {
      if (this.ImportantPropertiesHash.ContainsKey(key))
        this.ImportantPropertiesHash[key] = value;
      else
        this.ImportantPropertiesHash.Add(key, value);
    }
  }

  internal void Close()
  {
    if (this.m_importantPropertiesHash != null)
    {
      this.m_importantPropertiesHash.Clear();
      this.m_importantPropertiesHash = (Dictionary<CSSStyleItem.TextFormatImportantKey, object>) null;
    }
    if (this.m_propertiesHash == null)
      return;
    this.m_propertiesHash.Clear();
    this.m_propertiesHash = (Dictionary<CSSStyleItem.TextFormatKey, object>) null;
  }

  internal enum CssStyleType
  {
    None,
    ElementSelector,
    IdSelector,
    ClassSelector,
    GroupingSelector,
    DescendantSelector,
    ChildSelector,
    AdjacentSiblingSelector,
    GeneralSiblingSelector,
  }

  internal enum TextFormatKey
  {
    FontSize,
    FontFamily,
    Bold,
    Underline,
    Italic,
    Strike,
    FontColor,
    BackColor,
    LineHeight,
    LineHeightNormal,
    TextAlign,
    TopMargin,
    LeftMargin,
    BottomMargin,
    RightMargin,
    TextIndent,
    SubSuperScript,
    PageBreakBefore,
    PageBreakAfter,
    LetterSpacing,
    AllCaps,
    WhiteSpace,
    WordWrap,
    Display,
    ContentAlign,
    ItemsAlign,
    SelfAlign,
    Animation,
    AniamtionDelay,
    AnimationDirection,
    AnimationDuration,
    AnimationFillMode,
    AnimationIterationCount,
    AnimationName,
    AnimationPlayState,
    AnimationTimingFunction,
    BackfaceVisibility,
    Background,
    BackgroundAttachment,
    BackgroundClip,
    BackgroundImage,
    BackgroundOrigin,
    BackgroundPosition,
    BackgroundRepeat,
    BackgroundSize,
    Border,
    BorderColor,
    BorderStyle,
    BorderWidth,
    LeftBorder,
    RightBorder,
    TopBorder,
    BottomBorder,
    BorderBottomColor,
    BorderBottomStyle,
    BorderBottomWidth,
    BorderLeftColor,
    BorderLeftStyle,
    BorderLeftWidth,
    BorderRightColor,
    BorderRightStyle,
    BorderRightWidth,
    BorderTopColor,
    BorderTopStyle,
    BorderTopWidth,
    BorderBottomLeftRadius,
    BorderBottomRightRadius,
    BorderCollapse,
    BorderImage,
    BorderImageOutset,
    BorderImageRepeat,
    BorderImageSlice,
    BorderImageSource,
    BorderImageWidth,
    BorderRadius,
    BorderSpacing,
    BorderTopLeftRadius,
    BorderTopRightRadius,
    Bottom,
    BoxShadow,
    BoxSizing,
    CaptionSide,
    Clear,
    Clip,
    ColumnCount,
    ColumnFill,
    ColumnGap,
    ColumnRule,
    ColumnRuleColor,
    ColumnRuleStyle,
    ColumnRuleWidth,
    ColumnSpan,
    ColumnWidth,
    Columns,
    Content,
    CounterIncrement,
    CounterReset,
    Cursor,
    Direction,
    EmptyCells,
    Flex,
    FlexBasis,
    FlexDirection,
    FlexFlow,
    FlexGrow,
    FlexShrink,
    FlexWrap,
    Float,
    Font,
    FontFace,
    FontSizeAdjust,
    FontStretch,
    FontStyleKey,
    FontVariant,
    FontWeight,
    HangingPunctuation,
    Height,
    Icon,
    JustifyContent,
    KeyFrames,
    Left,
    ListStyle,
    listStyleImage,
    listStylePosition,
    ListStyleType,
    Margin,
    MaxHeight,
    MaxWidth,
    MinHeight,
    MinWidth,
    NavDown,
    NavIndex,
    NavLeft,
    NavRight,
    NavUp,
    Opacity,
    Order,
    Outline,
    OutlineColor,
    OutlineOffset,
    OutlineStyle,
    OutlineWidth,
    OverFlow,
    Overflow_X,
    Overflow_Y,
    Padding,
    PaddingBottom,
    PaddingLeft,
    PaddingRight,
    PaddingTop,
    PageBreakInside,
    Perspective,
    PerspectiveOrigin,
    Position,
    Quotes,
    Resize,
    Right,
    TabSize,
    TableLayout,
    TextAlignLast,
    TextDecoration,
    TextDecorationColor,
    TextDecorationLine,
    TextJustify,
    TextOverflow,
    TextShadow,
    Top,
    Transform,
    TransformOrigin,
    TransformStyle,
    Transition,
    TransitionDelay,
    TransitionDuration,
    TransitionProperty,
    TransitionTimingFunction,
    UnicodeBidi,
    VerticalAlign,
    Visibility,
    Width,
    WordBreak,
    WordSpacing,
    Zindex,
    Capitalize,
    Lowercase,
  }

  internal enum TextFormatImportantKey
  {
    FontSize = 0,
    FontFamily = 1,
    Bold = 2,
    Underline = 3,
    Italic = 4,
    Strike = 5,
    FontColor = 6,
    BackColor = 7,
    LineHeight = 8,
    LineHeightNormal = 9,
    TextAlign = 10, // 0x0000000A
    TopMargin = 11, // 0x0000000B
    LeftMargin = 12, // 0x0000000C
    BottomMargin = 13, // 0x0000000D
    RightMargin = 14, // 0x0000000E
    TextIndent = 15, // 0x0000000F
    SubSuperScript = 16, // 0x00000010
    PageBreakBefore = 17, // 0x00000011
    PageBreakAfter = 18, // 0x00000012
    LetterSpacing = 19, // 0x00000013
    AllCaps = 20, // 0x00000014
    WhiteSpace = 21, // 0x00000015
    WordWrap = 22, // 0x00000016
    Display = 23, // 0x00000017
    BackgroundAttachment = 24, // 0x00000018
    BackgroundImage = 25, // 0x00000019
    Background = 26, // 0x0000001A
    BackgroundPosition = 27, // 0x0000001B
    BackgroundRepeat = 28, // 0x0000001C
    BackgroundSize = 29, // 0x0000001D
    BorderBottomColor = 30, // 0x0000001E
    BorderBottom = 32, // 0x00000020
    BorderBottomStyle = 33, // 0x00000021
    BorderBottomWidth = 34, // 0x00000022
    BorderCollapse = 35, // 0x00000023
    BorderColor = 36, // 0x00000024
    Border = 37, // 0x00000025
    BorderLeftColor = 38, // 0x00000026
    BorderLeftStyle = 39, // 0x00000027
    BorderLeftWidth = 40, // 0x00000028
    BorderRightColor = 41, // 0x00000029
    BorderRightStyle = 42, // 0x0000002A
    BorderRightWidth = 43, // 0x0000002B
    BorderSpacing = 43, // 0x0000002B
    BorderStyle = 44, // 0x0000002C
    BorderTopColor = 45, // 0x0000002D
    BorderTopStyle = 46, // 0x0000002E
    BorderTopWidth = 47, // 0x0000002F
    BorderWidth = 48, // 0x00000030
    CaptionSide = 49, // 0x00000031
    Cursor = 50, // 0x00000032
    Direction = 51, // 0x00000033
    EmptyCells = 52, // 0x00000034
    Font = 53, // 0x00000035
    FontSizeAdjust = 54, // 0x00000036
    FontStretch = 55, // 0x00000037
    FontStyle = 56, // 0x00000038
    FontVariant = 57, // 0x00000039
    FontWeight = 58, // 0x0000003A
    Height = 59, // 0x0000003B
    ListStyleImage = 60, // 0x0000003C
    ListStyle = 61, // 0x0000003D
    ListStylePosition = 62, // 0x0000003E
    ListStyleType = 63, // 0x0000003F
    MarginBottom = 64, // 0x00000040
    Margin = 65, // 0x00000041
    MarginLeft = 66, // 0x00000042
    MarginRight = 67, // 0x00000043
    MarginTop = 68, // 0x00000044
    MarkerOffset = 69, // 0x00000045
    MozOpacity = 70, // 0x00000046
    PaddingBottom = 71, // 0x00000047
    Padding = 72, // 0x00000048
    PaddingLeft = 73, // 0x00000049
    PaddingRight = 74, // 0x0000004A
    PaddingTop = 75, // 0x0000004B
    TableLayout = 76, // 0x0000004C
    TextDecoration = 77, // 0x0000004D
    TextShadow = 78, // 0x0000004E
    Width = 79, // 0x0000004F
    WordSpacing = 80, // 0x00000050
  }
}
