// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WParagraphFormat
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using Syncfusion.DocIO.ReaderWriter;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WParagraphFormat : FormatBase
{
  internal const short HrAlignmentKey = 0;
  internal const short LeftIndentKey = 2;
  internal const short RightIndentKey = 3;
  internal const short FirstLineIndentKey = 5;
  internal const short KeepKey = 6;
  internal const short BeforeSpacingKey = 8;
  internal const short AfterSpacingKey = 9;
  internal const short KeepFollowKey = 10;
  internal const short WidowControlKey = 11;
  internal const short BeforeLinesKey = 90;
  internal const short AfterLinesKey = 91;
  internal const short PageBreakBeforeKey = 12;
  internal const short PageBreakAfterKey = 13;
  internal const short BordersKey = 20;
  internal const short BackColorKey = 21;
  internal const short BackGroundColorKey = 23;
  internal const short ColumnBreakAfterKey = 22;
  internal const short TabsKey = 30;
  internal const short BidiKey = 31 /*0x1F*/;
  internal const short ForeColorKey = 32 /*0x20*/;
  internal const short TextureStyleKey = 33;
  internal const short DataKey = 50;
  internal const short AdjustRightIndentKey = 80 /*0x50*/;
  internal const short AutoSpaceDEKey = 81;
  internal const short AutoSpaceDNKey = 82;
  internal const short LineSpacingKey = 52;
  internal const short LineSpacingRuleKey = 53;
  internal const short SpacingBeforeAutoKey = 54;
  internal const short SpacingAfterAutoKey = 55;
  internal const short OutlineLevelKey = 56;
  internal const short LeftBorderKey = 57;
  internal const short RightBorderKey = 58;
  internal const short TopBorderKey = 59;
  internal const short BottomBorderKey = 60;
  internal const short LeftBorderNewKey = 61;
  internal const short RightBorderNewKey = 62;
  internal const short TopBorderNewKey = 63 /*0x3F*/;
  internal const short BottomBorderNewKey = 64 /*0x40*/;
  internal const short ChangedFormatKey = 65;
  internal const short BetweenBorderKey = 66;
  internal const short BarBorderKey = 67;
  internal const short BetweenBorderNewKey = 93;
  internal const short BarBorderNewKey = 94;
  internal const short ContextualSpacingKey = 92;
  internal const short FrameHorizontalPositionKey = 71;
  internal const short FrameVerticalPositionKey = 72;
  internal const short FrameXKey = 73;
  internal const short FrameYKey = 74;
  internal const short FrameWidthKey = 76;
  internal const short FrameHeightKey = 77;
  internal const short FrameHorizontalDistanceFromTextKey = 83;
  internal const short FrameVerticalDistanceFromTextKey = 84;
  internal const short WrapFrameAroundKey = 88;
  internal const short SuppressAutoHyphensKey = 78;
  internal const short MirrorIndentsKey = 75;
  internal const short LeftIndentCharsKey = 85;
  internal const short FirstLineIndentCharsKey = 86;
  internal const short RightIndentCharsKey = 87;
  internal const short WordWrapKey = 89;
  internal const short BaseLineAlignmentKey = 34;
  internal const short SnapToGridKey = 35;
  internal const short SuppressOverlapKey = 36;
  internal const short TextBoxTightWrapKey = 37;
  internal const short SuppressLineNumbersKey = 38;
  internal const short LockFrameAnchorKey = 39;
  internal const short KinsokuKey = 40;
  internal const short OverflowPunctuationKey = 41;
  internal const short TopLinePunctuationKey = 42;
  internal const short DropCapKey = 43;
  internal const short DropCapLinesKey = 44;
  internal const short FrameTextDirectionKey = 48 /*0x30*/;
  internal const short FormatChangeDateTimeKey = 45;
  internal const short FormatChangeAuthorNameKey = 46;
  internal const short ParagraphStyleNameKey = 47;
  private WParagraphFormat m_tableStyleParagraphFormat;
  private byte m_bFlags;
  internal WAbsoluteTab m_absoluteTab;
  private Dictionary<string, Stream> m_xmlProps;

  private bool CancelOnChange
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal bool WordWrap
  {
    get => (bool) this.GetPropertyValue(89);
    set => this.SetPropertyValue(89, (object) value);
  }

  internal WAbsoluteTab AbsoluteTab
  {
    get
    {
      if (this.m_absoluteTab != null && this.m_absoluteTab.Owner == null)
        this.m_absoluteTab.SetOwner((OwnerHolder) this);
      return this.m_absoluteTab;
    }
    set => this.m_absoluteTab = value;
  }

  internal float FirstLineIndentChars
  {
    get => (float) this.GetPropertyValue(86);
    set => this.SetPropertyValue(86, (object) value);
  }

  internal float LeftIndentChars
  {
    get => (float) this.GetPropertyValue(85);
    set => this.SetPropertyValue(85, (object) value);
  }

  internal float RightIndentChars
  {
    get => (float) this.GetPropertyValue(87);
    set => this.SetPropertyValue(87, (object) value);
  }

  public bool Bidi
  {
    get => (bool) this.GetPropertyValue(31 /*0x1F*/);
    set => this.SetPropertyValue(31 /*0x1F*/, (object) value);
  }

  public TabCollection Tabs
  {
    get
    {
      if (!this.HasValue(30) || this.IsFormattingChange && this.OldPropertiesHash != null && !this.OldPropertiesHash.ContainsKey(30))
        this.CreateTabsCol();
      return this.IsFormattingChange && this.OldPropertiesHash.ContainsKey(30) ? (TabCollection) this.OldPropertiesHash[30] : (TabCollection) this.GetPropertyValue(30);
    }
  }

  public bool Keep
  {
    get => (bool) this.GetPropertyValue(6);
    set => this.SetPropertyValue(6, (object) value);
  }

  public bool KeepFollow
  {
    get => (bool) this.GetPropertyValue(10);
    set => this.SetPropertyValue(10, (object) value);
  }

  public bool PageBreakBefore
  {
    get => (bool) this.GetPropertyValue(12);
    set => this.SetPropertyValue(12, (object) value);
  }

  public bool PageBreakAfter
  {
    get => this[13] != null && (bool) this[13];
    set => this[13] = (object) value;
  }

  public bool WidowControl
  {
    get => (bool) this.GetPropertyValue(11);
    set => this.SetPropertyValue(11, (object) value);
  }

  internal bool AutoSpaceDN
  {
    get => (bool) this.GetPropertyValue(82);
    set => this.SetPropertyValue(82, (object) value);
  }

  internal bool AutoSpaceDE
  {
    get => (bool) this.GetPropertyValue(81);
    set => this.SetPropertyValue(81, (object) value);
  }

  internal bool AdjustRightIndent
  {
    get => (bool) this.GetPropertyValue(80 /*0x50*/);
    set => this.SetPropertyValue(80 /*0x50*/, (object) value);
  }

  public HorizontalAlignment HorizontalAlignment
  {
    get
    {
      HorizontalAlignment logicalJustification = this.LogicalJustification;
      if (this.Bidi)
      {
        if (logicalJustification == HorizontalAlignment.Left)
          return HorizontalAlignment.Right;
        if (logicalJustification == HorizontalAlignment.Right)
          return HorizontalAlignment.Left;
      }
      return logicalJustification;
    }
    set
    {
      HorizontalAlignment horizontalAlignment = value;
      if (this.Bidi)
      {
        switch (horizontalAlignment)
        {
          case HorizontalAlignment.Left:
            horizontalAlignment = HorizontalAlignment.Right;
            break;
          case HorizontalAlignment.Right:
            horizontalAlignment = HorizontalAlignment.Left;
            break;
        }
      }
      this.LogicalJustification = horizontalAlignment;
    }
  }

  internal HorizontalAlignment LogicalJustification
  {
    get => (HorizontalAlignment) this.GetPropertyValue(0);
    set => this.SetPropertyValue(0, (object) value);
  }

  public float LeftIndent
  {
    get => (float) this.GetPropertyValue(2);
    set
    {
      if ((double) value < -1584.0 || (double) value > 1584.0)
        throw new ArgumentException("LeftIndent must be between -1584 pt and 1584 pt.");
      this.SetPropertyValue(2, (object) value);
    }
  }

  public float RightIndent
  {
    get => (float) this.GetPropertyValue(3);
    set
    {
      if ((double) value < -1584.0 || (double) value > 1584.0)
        throw new ArgumentException("RightIndent must be between -1584 pt and 1584 pt.");
      this.SetPropertyValue(3, (object) value);
    }
  }

  public float FirstLineIndent
  {
    get => (float) this.GetPropertyValue(5);
    set
    {
      if ((double) value < -1584.0 || (double) value > 1584.0)
        throw new ArgumentException("FirstLineIndent must be between -1584 pt and 1584 pt.");
      this.SetPropertyValue(5, (object) value);
    }
  }

  public float BeforeSpacing
  {
    get => (float) this.GetPropertyValue(8);
    set
    {
      if ((double) value < 0.0 || (double) value > 1584.0)
        throw new ArgumentException("BeforeSpacing must be between 0 pt and 1584 pt.");
      if (this.SpaceBeforeAuto && !this.Document.IsOpening)
        this.SpaceBeforeAuto = false;
      this.SetPropertyValue(8, (object) value);
    }
  }

  internal float BeforeLines
  {
    get => (float) this.GetPropertyValue(90);
    set => this.SetPropertyValue(90, (object) value);
  }

  internal float AfterLines
  {
    get => (float) this.GetPropertyValue(91);
    set => this.SetPropertyValue(91, (object) value);
  }

  public float AfterSpacing
  {
    get => (float) this.GetPropertyValue(9);
    set
    {
      if ((double) value < 0.0 || (double) value > 1584.0)
        throw new ArgumentException("AfterSpacing must be between 0 pt and 1584 pt.");
      if (this.SpaceAfterAuto && !this.Document.IsOpening)
        this.SpaceAfterAuto = false;
      this.SetPropertyValue(9, (object) value);
    }
  }

  public Borders Borders => this.GetPropertyValue(20) as Borders;

  public Color BackColor
  {
    get => (Color) this.GetPropertyValue(21);
    set => this.SetPropertyValue(21, (object) value);
  }

  internal Color BackGroundColor
  {
    get => (Color) this.GetPropertyValue(23);
    set => this.SetPropertyValue(23, (object) value);
  }

  public bool ColumnBreakAfter
  {
    get => this[22] != null && (bool) this[22];
    set => this[22] = (object) value;
  }

  public float LineSpacing
  {
    get => (float) this.GetPropertyValue(52);
    set
    {
      if ((double) value < 0.7 || (double) value > 1584.0)
        throw new ArgumentException("LineSpacing must be between 0.7 pt and 1584 pt.");
      this.SetPropertyValue(52, (object) value);
    }
  }

  public LineSpacingRule LineSpacingRule
  {
    get => (LineSpacingRule) this.GetPropertyValue(53);
    set => this.SetPropertyValue(53, (object) value);
  }

  internal Color ForeColor
  {
    get => (Color) this.GetPropertyValue(32 /*0x20*/);
    set => this.SetPropertyValue(32 /*0x20*/, (object) value);
  }

  internal TextureStyle TextureStyle
  {
    get => (TextureStyle) this.GetPropertyValue(33);
    set => this.SetPropertyValue(33, (object) value);
  }

  public bool SpaceBeforeAuto
  {
    get => (bool) this.GetPropertyValue(54);
    set => this.SetPropertyValue(54, (object) value);
  }

  public bool SpaceAfterAuto
  {
    get => (bool) this.GetPropertyValue(55);
    set => this.SetPropertyValue(55, (object) value);
  }

  public OutlineLevel OutlineLevel
  {
    get
    {
      WParagraphStyle wparagraphStyle = (WParagraphStyle) null;
      if (this.OwnerBase is WParagraph)
        wparagraphStyle = (this.OwnerBase as WParagraph).ParaStyle as WParagraphStyle;
      else if (this.OwnerBase is WParagraphStyle)
        wparagraphStyle = this.OwnerBase as WParagraphStyle;
      if (wparagraphStyle != null && wparagraphStyle.BuiltInStyleIdentifier != BuiltinStyle.User && this.IsBuiltInHeadingStyle(wparagraphStyle.Name))
        return this.GetOutLineLevelForHeadingStyle(wparagraphStyle.Name);
      byte propertyValue = (byte) this.GetPropertyValue(56);
      switch (propertyValue)
      {
        case 0:
        case 1:
        case 2:
        case 3:
        case 4:
        case 5:
        case 6:
        case 7:
        case 8:
        case 9:
          return (OutlineLevel) Enum.ToObject(typeof (OutlineLevel), propertyValue);
        default:
          return OutlineLevel.BodyText;
      }
    }
    set => this.SetPropertyValue(56, (object) (byte) value);
  }

  internal bool IsFrame
  {
    get
    {
      return (double) this.FrameWidth != 0.0 || (double) this.FrameHeight != 0.0 || (double) this.FrameX != 0.0 || (double) this.FrameY != 0.0 || this.FrameHorizontalPos != (byte) 0 || this.WrapFrameAround != FrameWrapMode.Auto || this.HasValue(72) && this.FrameVerticalPos == (byte) 2;
    }
  }

  internal byte FrameVerticalPos
  {
    get => (byte) this.GetPropertyValue(72);
    set => this.SetPropertyValue(72, (object) value);
  }

  internal byte FrameVerticalAnchor
  {
    get
    {
      if (this.Document != null && this.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013 || this.HasValue(74))
        return this.FrameVerticalPos;
      return this.OwnerBase is WParagraph && (this.OwnerBase as WParagraph).ParaStyle != null && (this.OwnerBase as WParagraph).ParaStyle.ParagraphFormat.HasValue(74) ? (this.OwnerBase as WParagraph).ParaStyle.ParagraphFormat.FrameVerticalPos : (byte) 2;
    }
  }

  internal byte FrameHorizontalPos
  {
    get => (byte) this.GetPropertyValue(71);
    set => this.SetPropertyValue(71, (object) value);
  }

  internal float FrameX
  {
    get => (float) this.GetPropertyValue(73);
    set => this.SetPropertyValue(73, (object) value);
  }

  internal float FrameY
  {
    get => (float) this.GetPropertyValue(74);
    set => this.SetPropertyValue(74, (object) value);
  }

  internal float FrameWidth
  {
    get => (float) this.GetPropertyValue(76);
    set => this.SetPropertyValue(76, (object) value);
  }

  internal float FrameHeight
  {
    get => (float) this.GetPropertyValue(77);
    set => this.SetPropertyValue(77, (object) value);
  }

  internal float FrameHorizontalDistanceFromText
  {
    get => (float) this.GetPropertyValue(83);
    set => this.SetPropertyValue(83, (object) value);
  }

  internal float FrameVerticalDistanceFromText
  {
    get => (float) this.GetPropertyValue(84);
    set => this.SetPropertyValue(84, (object) value);
  }

  internal FrameWrapMode WrapFrameAround
  {
    get => (FrameWrapMode) this.GetPropertyValue(88);
    set => this.SetPropertyValue(88, (object) value);
  }

  internal bool IsChangedFormat
  {
    get => (bool) this.GetPropertyValue(65);
    set
    {
      if (!value)
        return;
      this.SetPropertyValue(65, (object) value);
    }
  }

  public bool ContextualSpacing
  {
    get => (bool) this.GetPropertyValue(92);
    set => this.SetPropertyValue(92, (object) value);
  }

  internal WParagraphFormat TableStyleParagraphFormat
  {
    get => this.m_tableStyleParagraphFormat;
    set => this.m_tableStyleParagraphFormat = value;
  }

  public bool MirrorIndents
  {
    get => (bool) this.GetPropertyValue(75);
    set => this.SetPropertyValue(75, (object) value);
  }

  public bool SuppressAutoHyphens
  {
    get => (bool) this.GetPropertyValue(78);
    set => this.SetPropertyValue(78, (object) value);
  }

  internal BaseLineAlignment BaseLineAlignment
  {
    get => (BaseLineAlignment) this.GetPropertyValue(34);
    set => this.SetPropertyValue(34, (object) value);
  }

  internal bool SnapToGrid
  {
    get => (bool) this.GetPropertyValue(35);
    set => this.SetPropertyValue(35, (object) value);
  }

  internal bool SuppressOverlap
  {
    get => (bool) this.GetPropertyValue(36);
    set => this.SetPropertyValue(36, (object) value);
  }

  internal TextboxTightWrapOptions TextboxTightWrap
  {
    get => (TextboxTightWrapOptions) this.GetPropertyValue(37);
    set => this.SetPropertyValue(37, (object) value);
  }

  internal bool SuppressLineNumbers
  {
    get => (bool) this.GetPropertyValue(38);
    set => this.SetPropertyValue(38, (object) value);
  }

  internal bool LockFrameAnchor
  {
    get => (bool) this.GetPropertyValue(39);
    set => this.SetPropertyValue(39, (object) value);
  }

  internal bool Kinsoku
  {
    get => (bool) this.GetPropertyValue(40);
    set => this.SetPropertyValue(40, (object) value);
  }

  internal bool OverflowPunctuation
  {
    get => (bool) this.GetPropertyValue(41);
    set => this.SetPropertyValue(41, (object) value);
  }

  internal bool TopLinePunctuation
  {
    get => (bool) this.GetPropertyValue(41);
    set => this.SetPropertyValue(41, (object) value);
  }

  internal DropCapType DropCap
  {
    get => (DropCapType) this.GetPropertyValue(43);
    set => this.SetPropertyValue(43, (object) value);
  }

  internal int DropCapLines
  {
    get => (int) this.GetPropertyValue(44);
    set => this.SetPropertyValue(44, (object) value);
  }

  internal byte TextDirection
  {
    get => (byte) this.GetPropertyValue(48 /*0x30*/);
    set => this.SetPropertyValue(48 /*0x30*/, (object) value);
  }

  internal Dictionary<string, Stream> XmlProps
  {
    get
    {
      if (this.m_xmlProps == null)
        this.m_xmlProps = new Dictionary<string, Stream>();
      return this.m_xmlProps;
    }
  }

  internal DateTime FormatChangeDateTime
  {
    get => (DateTime) this.GetPropertyValue(45);
    set => this.SetPropertyValue(45, (object) value);
  }

  internal string FormatChangeAuthorName
  {
    get => (string) this.GetPropertyValue(46);
    set => this.SetPropertyValue(46, (object) value);
  }

  internal string ParagraphStyleName
  {
    get => (string) this.GetPropertyValue(47);
    set => this.SetPropertyValue(47, (object) value);
  }

  public WParagraphFormat()
  {
  }

  public WParagraphFormat(IWordDocument document)
    : base(document)
  {
  }

  internal bool IsContainFrameKey()
  {
    if (this.PropertiesHash.Count <= 0)
      return false;
    return this.PropertiesHash.ContainsKey(72) && this.PropertiesHash.ContainsKey(71) || this.PropertiesHash.ContainsKey(88) || this.PropertiesHash.ContainsKey(73) || this.PropertiesHash.ContainsKey(74) || this.PropertiesHash.ContainsKey(77) || this.PropertiesHash.ContainsKey(76) || this.PropertiesHash.ContainsKey(83) || this.PropertiesHash.ContainsKey(84);
  }

  internal bool IsInFrame()
  {
    if (this.PropertiesHash.Count <= 0)
      return false;
    if (this.PropertiesHash.ContainsKey(71) && this.FrameHorizontalPos != (byte) 0 || this.PropertiesHash.ContainsKey(72) && this.FrameVerticalPos != (byte) 0 || this.PropertiesHash.ContainsKey(88) && this.WrapFrameAround != FrameWrapMode.Auto || this.PropertiesHash.ContainsKey(74) && (double) this.FrameY != 0.0 || this.PropertiesHash.ContainsKey(73) && (double) this.FrameX != 0.0)
      return true;
    return this.PropertiesHash.ContainsKey(76) && (double) this.FrameWidth != 0.0;
  }

  internal bool IsFrameXAlign(float xPosition)
  {
    return (double) xPosition == 0.0 || (double) xPosition == -4.0 || (double) xPosition == -8.0 || (double) xPosition == -12.0 || (double) xPosition == -16.0;
  }

  internal bool IsFrameYAlign(float yPosition)
  {
    return (double) yPosition == 0.0 || (double) yPosition == -4.0 || (double) yPosition == -8.0 || (double) yPosition == -12.0 || (double) yPosition == -16.0 || (double) yPosition == -20.0;
  }

  private object GetPropertyValue(int propKey) => this[propKey];

  private bool ContainsBordersSprm()
  {
    if (this.m_sprms != null)
    {
      int[] array = new int[12]
      {
        25636,
        50766,
        25638,
        50768,
        25639,
        50769,
        25637,
        50767,
        25640,
        50770,
        26153,
        50771
      };
      for (int index = 0; index < this.m_sprms.Modifiers.Count; ++index)
      {
        SinglePropertyModifierRecord modifier = this.m_sprms.Modifiers[index];
        if (modifier != null && Array.IndexOf<int>(array, modifier.TypedOptions) != -1)
          return true;
      }
    }
    return false;
  }

  internal object GetSpacingValue(int key)
  {
    WParagraphFormat wparagraphFormat = this;
    int fullKey = this.GetFullKey(key);
    if (wparagraphFormat.PropertiesHash.ContainsKey(fullKey))
      return wparagraphFormat.PropertiesHash[fullKey];
    FormatBase baseFormat = wparagraphFormat.BaseFormat;
    while (baseFormat != null && baseFormat.PropertiesHash != null && baseFormat != this.m_doc.DefParaFormat)
    {
      if (baseFormat.PropertiesHash.ContainsKey(fullKey))
        return baseFormat.PropertiesHash[fullKey];
      if (baseFormat.BaseFormat == null && this.TableStyleParagraphFormat != null)
      {
        if (this.TableStyleParagraphFormat.PropertiesHash.ContainsKey(fullKey))
          return this.TableStyleParagraphFormat.PropertiesHash[key];
        baseFormat = this.TableStyleParagraphFormat.BaseFormat;
      }
      else
        baseFormat = baseFormat.BaseFormat;
    }
    return (object) 0.0f;
  }

  internal object GetParagraphFormat(int key)
  {
    object paragraphFormat = this[key];
    WParagraphFormat wparagraphFormat = this;
    for (int fullKey = this.GetFullKey(key); !wparagraphFormat.PropertiesHash.ContainsKey(fullKey); wparagraphFormat = wparagraphFormat.BaseFormat as WParagraphFormat)
    {
      WListFormat wlistFormat = (WListFormat) null;
      if (wparagraphFormat.OwnerBase is WParagraph)
        wlistFormat = (wparagraphFormat.OwnerBase as WParagraph).ListFormat;
      else if (wparagraphFormat.OwnerBase is WParagraphStyle)
        wlistFormat = (wparagraphFormat.OwnerBase as WParagraphStyle).ListFormat;
      else if (wparagraphFormat.OwnerBase is WTableStyle)
        wlistFormat = (wparagraphFormat.OwnerBase as WTableStyle).ListFormat;
      else if (wparagraphFormat.OwnerBase is WNumberingStyle)
        wlistFormat = (wparagraphFormat.OwnerBase as WNumberingStyle).ListFormat;
      if (wlistFormat != null && wlistFormat.CurrentListLevel != null && wlistFormat.CurrentListLevel.ParagraphFormat.HasValue(key))
        return wlistFormat.CurrentListLevel.ParagraphFormat.PropertiesHash[key];
      if (wparagraphFormat.BaseFormat == null)
        return this.TableStyleParagraphFormat != null ? this.TableStyleParagraphFormat[key] : paragraphFormat;
    }
    return paragraphFormat;
  }

  internal void SetPropertyValue(int propKey, object value) => this[propKey] = value;

  internal void ChangeTabs(TabCollection tabs) => this[30] = (object) tabs;

  internal void CreateTabsCol()
  {
    this[30] = (object) new TabCollection(this.m_doc, (FormatBase) this);
  }

  internal bool ContainsValue(int key)
  {
    if (this.PropertiesHash.ContainsKey(key) || this.BaseFormat is WParagraphFormat && (this.BaseFormat as WParagraphFormat).ContainsValue(key))
      return true;
    if (this.OwnerBase is WParagraph && (this.OwnerBase as WParagraph).IsInCell)
    {
      WTableCell ownerEntity = (this.OwnerBase as WParagraph).GetOwnerEntity() as WTableCell;
      if (ownerEntity.Owner is WTableRow && ownerEntity.Owner.Owner is WTable && (ownerEntity.Owner.Owner as WTable).GetStyle() is WTableStyle style && style.ParagraphFormat.PropertiesHash.ContainsKey(key))
        return true;
    }
    return false;
  }

  internal bool IsPreviousParagraphInSameFrame()
  {
    if (!(this.OwnerBase is WParagraph))
      return false;
    WParagraph ownerBase = this.OwnerBase as WParagraph;
    IEntity previousSibling1 = ownerBase.PreviousSibling;
    if (ownerBase.IsInCell && !(previousSibling1 is WParagraph))
    {
      IEntity previousSibling2 = ownerBase.GetOwnerTableCell(ownerBase.OwnerTextBody).OwnerRow.OwnerTable.PreviousSibling;
      return previousSibling2 is WParagraph && this.IsInSameFrame((previousSibling2 as WParagraph).ParagraphFormat);
    }
    switch (previousSibling1)
    {
      case WParagraph _:
        return this.IsInSameFrame((previousSibling1 as WParagraph).ParagraphFormat);
      case WTable _ when (previousSibling1 as WTable).IsFrame:
        return this.IsInSameFrame((previousSibling1 as WTable).Rows[0].Cells[0].Paragraphs[0].ParagraphFormat);
      default:
        return false;
    }
  }

  internal bool IsNextParagraphInSameFrame()
  {
    return this.OwnerBase is WParagraph ownerBase && ownerBase.NextSibling is WParagraph && this.IsInSameFrame(((WParagraph) ownerBase.NextSibling).ParagraphFormat);
  }

  internal bool IsInSameFrame(WParagraphFormat paraFormat)
  {
    float num1 = (float) ((int) (ushort) Math.Round((double) paraFormat.FrameHeight * 20.0) & (int) short.MaxValue);
    float num2 = (float) ((int) (ushort) Math.Round((double) this.FrameHeight * 20.0) & (int) short.MaxValue);
    return paraFormat.IsFrame && (double) paraFormat.FrameX == (double) this.FrameX && (double) paraFormat.FrameWidth == (double) this.FrameWidth && (double) num1 == (double) num2 && (int) paraFormat.FrameHorizontalPos == (int) this.FrameHorizontalPos && (int) paraFormat.FrameVerticalPos == (int) this.FrameVerticalPos && (double) paraFormat.FrameY == (double) this.FrameY && paraFormat.WrapFrameAround == this.WrapFrameAround;
  }

  internal void SetDefaultProperties()
  {
    this.PropertiesHash.Add(21, (object) Color.Empty);
    this.PropertiesHash.Add(8, (object) 0.0f);
    this.PropertiesHash.Add(9, (object) 0.0f);
    this.PropertiesHash.Add(31 /*0x1F*/, (object) false);
    this.PropertiesHash.Add(22, (object) false);
    this.PropertiesHash.Add(92, (object) false);
    this.PropertiesHash.Add(5, (object) 0.0f);
    this.PropertiesHash.Add(32 /*0x20*/, (object) Color.Empty);
    this.PropertiesHash.Add(0, (object) HorizontalAlignment.Left);
    this.PropertiesHash.Add(6, (object) false);
    this.PropertiesHash.Add(10, (object) false);
    this.PropertiesHash.Add(2, (object) 0.0f);
    this.PropertiesHash.Add(52, (object) 12f);
    this.PropertiesHash.Add(53, (object) LineSpacingRule.Multiple);
    this.PropertiesHash.Add(56, (object) (byte) 9);
    this.PropertiesHash.Add(13, (object) false);
    this.PropertiesHash.Add(12, (object) false);
    this.PropertiesHash.Add(3, (object) 0.0f);
    this.PropertiesHash.Add(55, (object) false);
    this.PropertiesHash.Add(54, (object) false);
    this.PropertiesHash.Add(33, (object) TextureStyle.TextureNone);
    this.PropertiesHash.Add(11, (object) false);
    this.PropertiesHash.Add(89, (object) true);
    this.Borders.SetDefaultProperties();
  }

  internal bool IsBuiltInHeadingStyle(string styleName)
  {
    return styleName == "Heading 1" || styleName == "Heading 2" || styleName == "Heading 3" || styleName == "Heading 4" || styleName == "Heading 5" || styleName == "Heading 6" || styleName == "Heading 7" || styleName == "Heading 8" || styleName == "Heading 9";
  }

  private OutlineLevel GetOutLineLevelForHeadingStyle(string styleName)
  {
    switch (styleName)
    {
      case "Heading 1":
        return OutlineLevel.Level1;
      case "Heading 2":
        return OutlineLevel.Level2;
      case "Heading 3":
        return OutlineLevel.Level3;
      case "Heading 4":
        return OutlineLevel.Level4;
      case "Heading 5":
        return OutlineLevel.Level5;
      case "Heading 6":
        return OutlineLevel.Level6;
      case "Heading 7":
        return OutlineLevel.Level7;
      case "Heading 8":
        return OutlineLevel.Level8;
      case "Heading 9":
        return OutlineLevel.Level9;
      default:
        return OutlineLevel.BodyText;
    }
  }

  protected internal override void EnsureComposites() => this.EnsureComposites(20);

  protected override object GetDefValue(int key)
  {
    if (this.Document != null && this.Document.m_defParaFormat != null && this.Document.m_defParaFormat != this)
      return this.Document.m_defParaFormat[key];
    switch (key)
    {
      case 0:
        return (object) HorizontalAlignment.Left;
      case 2:
      case 3:
      case 5:
      case 85:
      case 86:
      case 87:
        return (object) 0.0f;
      case 6:
      case 10:
      case 12:
      case 13:
      case 22:
      case 31 /*0x1F*/:
      case 65:
        return (object) false;
      case 8:
      case 9:
      case 90:
      case 91:
        return (object) 0.0f;
      case 11:
      case 89:
        return (object) true;
      case 21:
      case 23:
      case 32 /*0x20*/:
        return (object) Color.Empty;
      case 30:
        return (object) new TabCollection(this.Document, (FormatBase) this);
      case 33:
        return (object) TextureStyle.TextureNone;
      case 34:
        return (object) BaseLineAlignment.Auto;
      case 35:
      case 41:
        return (object) true;
      case 36:
      case 38:
      case 39:
      case 40:
      case 42:
        return (object) false;
      case 37:
        return (object) TextboxTightWrapOptions.None;
      case 43:
        return (object) DropCapType.None;
      case 44:
        return (object) 1;
      case 45:
        return (object) DateTime.MinValue;
      case 46:
        return (object) string.Empty;
      case 47:
        return (object) string.Empty;
      case 48 /*0x30*/:
        return (object) 0;
      case 50:
        return (object) null;
      case 52:
        return (object) 12f;
      case 53:
        return (object) LineSpacingRule.Multiple;
      case 54:
      case 55:
      case 75:
      case 78:
      case 80 /*0x50*/:
      case 81:
      case 82:
      case 92:
        return (object) false;
      case 56:
        return (object) byte.MaxValue;
      case 71:
      case 72:
        return (object) (byte) 0;
      case 73:
      case 74:
      case 76:
      case 77:
      case 83:
      case 84:
        return (object) 0.0f;
      case 88:
        return (object) FrameWrapMode.Auto;
      default:
        throw new ArgumentException("key has invalid value");
    }
  }

  protected override FormatBase GetDefComposite(int key)
  {
    return key == 20 ? this.GetDefComposite(20, (FormatBase) new Borders((FormatBase) this, 20)) : (FormatBase) null;
  }

  protected internal new void ImportContainer(FormatBase format)
  {
    base.ImportContainer(format);
    if (!(format is WParagraphFormat format1))
      return;
    this.ImportXmlProps(format1);
  }

  private void ImportXmlProps(WParagraphFormat format)
  {
    if (format.m_xmlProps == null || format.m_xmlProps.Count <= 0)
      return;
    format.Document.CloneProperties(format.XmlProps, ref this.m_xmlProps);
  }

  protected override void ImportMembers(FormatBase format)
  {
    base.ImportMembers(format);
    if (!(format is WParagraphFormat wparagraphFormat))
      return;
    this.CopyFormat(format);
    if (wparagraphFormat.HasValue(13))
      this[13] = (object) wparagraphFormat.PageBreakAfter;
    if (!wparagraphFormat.HasValue(22))
      return;
    this[22] = (object) wparagraphFormat.ColumnBreakAfter;
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    base.ReadXmlAttributes(reader);
    if (reader.HasAttribute("Bidi"))
      this.Bidi = reader.ReadBoolean("Bidi");
    if (reader.HasAttribute("HrAlignment"))
      this.HorizontalAlignment = (HorizontalAlignment) reader.ReadEnum("HrAlignment", typeof (HorizontalAlignment));
    if (reader.HasAttribute("LeftIndent"))
      this.SetPropertyValue(2, (object) reader.ReadFloat("LeftIndent"));
    if (reader.HasAttribute("RightIndent"))
      this.SetPropertyValue(3, (object) reader.ReadFloat("RightIndent"));
    if (reader.HasAttribute("FirstLineIndent"))
      this.SetPropertyValue(5, (object) reader.ReadFloat("FirstLineIndent"));
    if (reader.HasAttribute("Keep"))
      this.Keep = reader.ReadBoolean("Keep");
    if (reader.HasAttribute("BeforeSpacing"))
      this.SetPropertyValue(8, (object) reader.ReadFloat("BeforeSpacing"));
    if (reader.HasAttribute("AfterSpacing"))
      this.SetPropertyValue(9, (object) reader.ReadFloat("AfterSpacing"));
    if (reader.HasAttribute("KeepFollow"))
      this.KeepFollow = reader.ReadBoolean("KeepFollow");
    if (reader.HasAttribute("WidowControl"))
      this.WidowControl = reader.ReadBoolean("WidowControl");
    if (reader.HasAttribute("PageBreakBefore"))
      this.PageBreakBefore = reader.ReadBoolean("PageBreakBefore");
    if (reader.HasAttribute("PageBreakAfter"))
      this.PageBreakAfter = reader.ReadBoolean("PageBreakAfter");
    if (reader.HasAttribute("BackColor"))
      this.BackColor = reader.ReadColor("BackColor");
    if (reader.HasAttribute("ColumnBreakAfter"))
      this.ColumnBreakAfter = reader.ReadBoolean("ColumnBreakAfter");
    if (reader.HasAttribute("LineSpacing"))
      this.SetPropertyValue(52, (object) reader.ReadFloat("LineSpacing"));
    if (reader.HasAttribute("LineSpacingRule"))
      this.LineSpacingRule = (LineSpacingRule) reader.ReadEnum("LineSpacingRule", typeof (LineSpacingRule));
    if (reader.HasAttribute("ForeColor"))
      this.ForeColor = reader.ReadColor("ForeColor");
    if (!reader.HasAttribute("Texture"))
      return;
    this.TextureStyle = (TextureStyle) reader.ReadEnum("Texture", typeof (TextureStyle));
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    if (this.HasKey(13))
      writer.WriteValue("PageBreakAfter", this.PageBreakAfter);
    if (this.HasKey(22))
      writer.WriteValue("ColumnBreakAfter", this.ColumnBreakAfter);
    if (this.m_sprms != null)
      return;
    if (this.HasValue(31 /*0x1F*/))
      writer.WriteValue("Bidi", this.Bidi);
    if (this.HasValue(0))
      writer.WriteValue("HrAlignment", (Enum) this.HorizontalAlignment);
    if (this.HasValue(2))
      writer.WriteValue("LeftIndent", this.LeftIndent);
    if (this.HasValue(3))
      writer.WriteValue("RightIndent", this.RightIndent);
    if (this.HasValue(5))
      writer.WriteValue("FirstLineIndent", this.FirstLineIndent);
    if (this.HasValue(6))
      writer.WriteValue("Keep", this.Keep);
    if (this.HasValue(8))
      writer.WriteValue("BeforeSpacing", this.BeforeSpacing);
    if (this.HasValue(9))
      writer.WriteValue("AfterSpacing", this.AfterSpacing);
    if (this.HasValue(10))
      writer.WriteValue("KeepFollow", this.KeepFollow);
    if (this.HasValue(11))
      writer.WriteValue("WidowControl", this.WidowControl);
    if (this.HasValue(12))
      writer.WriteValue("PageBreakBefore", this.PageBreakBefore);
    if (!this.BackColor.IsEmpty)
      writer.WriteValue("BackColor", this.BackColor);
    if (this.HasValue(52))
      writer.WriteValue("LineSpacing", this.LineSpacing);
    if (this.HasValue(53))
      writer.WriteValue("LineSpacingRule", (Enum) this.LineSpacingRule);
    if (this.ForeColor != Color.Empty)
      writer.WriteValue("ForeColor", this.ForeColor);
    if (this.TextureStyle == TextureStyle.TextureNone)
      return;
    writer.WriteValue("Texture", (Enum) this.TextureStyle);
  }

  protected override void WriteXmlContent(IXDLSContentWriter writer)
  {
    base.WriteXmlContent(writer);
    if (this.m_sprms == null)
      return;
    byte[] arrData = new byte[this.m_sprms.Length];
    this.m_sprms.Save(arrData, 0);
    writer.WriteChildBinaryElement("internal-data", arrData);
  }

  protected override bool ReadXmlContent(IXDLSContentReader reader)
  {
    bool flag = base.ReadXmlContent(reader);
    if (reader.TagName == "internal-data")
    {
      SinglePropertyModifierArray sprms = new SinglePropertyModifierArray(reader.ReadChildBinaryElement());
      flag = true;
      ParagraphPropertiesConverter.SprmsToFormat(sprms, this, (Dictionary<int, string>) null, (WordStyleSheet) null);
      sprms.Clear();
    }
    return flag;
  }

  protected override void InitXDLSHolder()
  {
    if (this.m_sprms != null)
      return;
    this.XDLSHolder.AddElement("borders", (object) this.Borders);
    this.XDLSHolder.AddElement("Tabs", (object) this.Tabs);
  }

  internal override void AcceptChanges()
  {
    this[65] = (object) false;
    if (this.OldPropertiesHash == null || this.OldPropertiesHash.Count <= 0)
      return;
    this.OldPropertiesHash.Clear();
    base.AcceptChanges();
  }

  internal override void RemovePositioning()
  {
    if (this.m_sprms == null || this.m_sprms.Count <= 0)
      return;
    this.m_sprms.RemoveValue(9755);
    this.m_sprms.RemoveValue(9251);
    this.m_sprms.RemoveValue(33816);
    this.m_sprms.RemoveValue(33817);
    this.m_sprms.RemoveValue(33839);
    this.m_sprms.RemoveValue(17954);
    this.m_sprms.RemoveValue(33838);
    this.m_sprms.RemoveValue(33818);
    this.m_sprms.RemoveValue(17451);
  }

  internal override void ApplyBase(FormatBase baseFormat)
  {
    base.ApplyBase(baseFormat);
    if (baseFormat == null)
      this.Borders.ApplyBase((FormatBase) null);
    else
      this.Borders.ApplyBase((FormatBase) (baseFormat as WParagraphFormat).Borders);
  }

  internal override void Close()
  {
    base.Close();
    if (this.m_tableStyleParagraphFormat != null)
    {
      this.m_tableStyleParagraphFormat.Close();
      this.m_tableStyleParagraphFormat = (WParagraphFormat) null;
    }
    if (this.m_xmlProps != null)
    {
      this.m_xmlProps.Clear();
      this.m_xmlProps = (Dictionary<string, Stream>) null;
    }
    if (this.Borders == null)
      return;
    this.Borders.Close();
  }

  internal override bool HasValue(int propertyKey)
  {
    if (this.HasKey(propertyKey))
      return true;
    if (propertyKey == 0 || propertyKey != 31 /*0x1F*/)
      ;
    return false;
  }

  internal bool HasValueWithParent(int propertyKey)
  {
    bool flag = this.HasValue(propertyKey);
    return !flag && this.BaseFormat is WParagraphFormat ? (this.BaseFormat as WParagraphFormat).HasValueWithParent(propertyKey) : flag;
  }

  internal bool HasBorder()
  {
    return this.Borders.Horizontal.IsBorderDefined || this.Borders.Left.IsBorderDefined || this.Borders.Right.IsBorderDefined || this.Borders.Top.IsBorderDefined || this.Borders.Bottom.IsBorderDefined;
  }

  internal HorizontalAlignment GetAlignmentToRender()
  {
    HorizontalAlignment alignmentToRender = this.HorizontalAlignment;
    switch (alignmentToRender)
    {
      case HorizontalAlignment.JustifyMedium:
      case HorizontalAlignment.JustifyHigh:
      case HorizontalAlignment.JustifyLow:
      case HorizontalAlignment.ThaiJustify:
        alignmentToRender = HorizontalAlignment.Justify;
        break;
      case HorizontalAlignment.Right | HorizontalAlignment.Distribute:
        alignmentToRender = this.Bidi ? HorizontalAlignment.Right : HorizontalAlignment.Left;
        break;
    }
    return alignmentToRender;
  }

  internal bool HasShading()
  {
    return this.PropertiesHash.ContainsKey(21) || this.PropertiesHash.ContainsKey(33) || this.PropertiesHash.ContainsKey(32 /*0x20*/);
  }

  internal override int GetSprmOption(int propertyKey)
  {
    switch (propertyKey)
    {
      case 0:
        return 9219;
      case 2:
        return 33807;
      case 3:
        return 33806;
      case 5:
        return 33809;
      case 6:
        return 9221;
      case 8:
        return 42003;
      case 9:
        return 42004;
      case 10:
        return 9222;
      case 11:
        return 9265;
      case 12:
        return 9223;
      case 31 /*0x1F*/:
        return 9281;
      case 34:
        return 17465;
      case 35:
        return 9287;
      case 36:
        return 9314;
      case 37:
        return 9329;
      case 38:
        return 9228;
      case 39:
        return 9264;
      case 40:
        return 9267;
      case 41:
        return 9269;
      case 42:
        return 9270;
      case 43:
      case 44:
        return 17452;
      case 48 /*0x30*/:
        return 17466;
      case 52:
      case 53:
        return 25618;
      case 54:
        return 9307;
      case 55:
        return 9308;
      case 56:
        return 9792;
      case 57:
        return 25637;
      case 58:
        return 25639;
      case 59:
        return 25636;
      case 60:
        return 25638;
      case 61:
        return 50767;
      case 62:
        return 50769;
      case 63 /*0x3F*/:
        return 50766;
      case 64 /*0x40*/:
        return 50768;
      case 65:
        return 9828;
      case 66:
        return 25640;
      case 67:
        return 26153;
      case 71:
      case 72:
        return 9755;
      case 73:
        return 33816;
      case 74:
        return 33817;
      case 75:
        return 9328;
      case 76:
        return 33818;
      case 77:
        return 17451;
      case 78:
        return 9258;
      case 80 /*0x50*/:
        return 9288;
      case 81:
        return 9271;
      case 82:
        return 9272;
      case 83:
        return 33839;
      case 84:
        return 33838;
      case 85:
        return 17494;
      case 86:
        return 17495;
      case 87:
        return 17493;
      case 88:
        return 9251;
      case 89:
        return 9268;
      case 90:
        return 17496;
      case 91:
        return 17497;
      case 92:
        return 9325;
      case 93:
        return 50770;
      case 94:
        return 50771;
      default:
        return int.MaxValue;
    }
  }

  internal void UpdateJustification(
    SinglePropertyModifierArray sprms,
    SinglePropertyModifierRecord sprmPJc)
  {
    if (this.Document == null || this.Document.WordVersion == (ushort) 0 || this.Document.WordVersion > (ushort) 217)
      return;
    SinglePropertyModifierRecord propertyModifierRecord = sprmPJc;
    if (propertyModifierRecord != null && (this.Document.WordVersion == (ushort) 193 || !this.Bidi))
    {
      this[0] = (object) propertyModifierRecord.ByteValue;
    }
    else
    {
      if (this.Document.WordVersion == (ushort) 193 || !this.Bidi)
        return;
      byte byteValue = propertyModifierRecord.ByteValue;
      if (byteValue == (byte) 5)
      {
        sprms.SetByteValue(9313, (byte) 5);
        sprms.SetIntValue(9219, 4);
      }
      if (this.Document.WordVersion == (ushort) 217)
      {
        if ((!(this.OwnerBase is WParagraph) || propertyModifierRecord != null) && byteValue == (byte) 0)
        {
          this[0] = (object) HorizontalAlignment.Right;
        }
        else
        {
          if (propertyModifierRecord == null)
            return;
          this[0] = (object) (HorizontalAlignment) (byteValue == (byte) 2 ? 0 : (int) byteValue);
        }
      }
      else if (this.Document.WordVersion == (ushort) 192 /*0xC0*/ && propertyModifierRecord != null)
      {
        this[0] = (object) (HorizontalAlignment) byteValue;
      }
      else
      {
        if (propertyModifierRecord == null)
          return;
        if (this.Bidi)
        {
          int num;
          switch (byteValue)
          {
            case 0:
              num = 2;
              break;
            case 2:
              num = 0;
              break;
            default:
              num = (int) byteValue;
              break;
          }
          this[0] = (object) (HorizontalAlignment) num;
        }
        else
          this[0] = (object) (HorizontalAlignment) byteValue;
      }
    }
  }

  internal void UpdateBiDi(bool value)
  {
    if (this.Document != null && this.Document.WordVersion == (ushort) 193)
      return;
    this[31 /*0x1F*/] = (object) value;
  }

  internal void UpdateTabs(SinglePropertyModifierArray sprms)
  {
    bool flag = false;
    SinglePropertyModifierRecord record = this.IsFormattingChange ? sprms.GetOldSprm(50701, 9828) : sprms.GetNewSprm(50701, 9828);
    if (record == null)
    {
      record = this.IsFormattingChange ? sprms.GetOldSprm(50709, 9828) : sprms.GetNewSprm(50709, 9828);
      flag = true;
    }
    if (record == null)
      return;
    TabsInfo info = flag ? new TabsInfo(record) : new TabsInfo(record);
    TabCollection tabs = new TabCollection(this.Document, (FormatBase) this);
    tabs.CancelOnChangeEvent = true;
    ParagraphPropertiesConverter.ExportTabs(info, tabs);
    this[30] = (object) tabs;
    tabs.CancelOnChangeEvent = false;
  }

  internal void UpdateOldFormatBorders(ref Borders borders)
  {
    if (!this.IsFormattingChange || borders != null)
      return;
    borders = new Borders();
    this.SetPropertyValue(20, (object) borders);
  }

  internal BorderCode GetBorder(SinglePropertyModifierRecord record)
  {
    byte[] byteArray = record.ByteArray;
    BorderCode border;
    if (byteArray.Length == 4)
    {
      border = new BorderCode(record.ByteArray, 0);
    }
    else
    {
      border = new BorderCode();
      border.ParseNewBrc(byteArray, 0);
    }
    return border;
  }

  internal void UpdateSourceFormat(WParagraphFormat destBaseFormat)
  {
    WParagraphFormat format = new WParagraphFormat((IWordDocument) destBaseFormat.Document);
    format.ImportContainer((FormatBase) this);
    format.CopyProperties((FormatBase) this);
    format.ApplyBase((FormatBase) destBaseFormat);
    this.UpdateSourceFormatting(format);
    this.ImportContainer((FormatBase) format);
    this.CopyProperties((FormatBase) format);
    format.Close();
  }

  internal void UpdateSourceFormatting(WParagraphFormat format)
  {
    if (format.AdjustRightIndent != this.AdjustRightIndent)
      format.AdjustRightIndent = this.AdjustRightIndent;
    if ((double) format.AfterSpacing != (double) this.AfterSpacing)
      format.SetPropertyValue(9, (object) this.AfterSpacing);
    if (format.AutoSpaceDE != this.AutoSpaceDE)
      format.AutoSpaceDE = this.AutoSpaceDE;
    if (format.AutoSpaceDN != this.AutoSpaceDN)
      format.AutoSpaceDN = this.AutoSpaceDN;
    if (format.BackColor != this.BackColor)
      format.BackColor = this.BackColor;
    if ((double) format.BeforeSpacing != (double) this.BeforeSpacing)
      format.SetPropertyValue(8, (object) this.BeforeSpacing);
    if (format.Bidi != this.Bidi)
      format.Bidi = this.Bidi;
    if (format.ColumnBreakAfter != this.ColumnBreakAfter)
      format.ColumnBreakAfter = this.ColumnBreakAfter;
    if (format.ContextualSpacing != this.ContextualSpacing)
      format.ContextualSpacing = this.ContextualSpacing;
    if ((double) format.FirstLineIndent != (double) this.FirstLineIndent)
      format.SetPropertyValue(5, (object) this.FirstLineIndent);
    if (format.ForeColor != this.ForeColor)
      format.ForeColor = this.ForeColor;
    if ((double) format.FrameHeight != (double) this.FrameHeight)
      format.FrameHeight = this.FrameHeight;
    if ((double) format.FrameHorizontalDistanceFromText != (double) this.FrameHorizontalDistanceFromText)
      format.FrameHorizontalDistanceFromText = this.FrameHorizontalDistanceFromText;
    if ((int) format.FrameHorizontalPos != (int) this.FrameHorizontalPos)
      format.FrameHorizontalPos = this.FrameHorizontalPos;
    if ((double) format.FrameVerticalDistanceFromText != (double) this.FrameVerticalDistanceFromText)
      format.FrameVerticalDistanceFromText = this.FrameVerticalDistanceFromText;
    if ((int) format.FrameVerticalPos != (int) this.FrameVerticalPos)
      format.FrameVerticalPos = this.FrameVerticalPos;
    if ((double) format.FrameWidth != (double) this.FrameWidth)
      format.FrameWidth = this.FrameWidth;
    if ((double) format.FrameX != (double) this.FrameX)
      format.FrameX = this.FrameX;
    if ((double) format.FrameY != (double) this.FrameY)
      format.FrameY = this.FrameY;
    if (format.HorizontalAlignment != this.HorizontalAlignment)
      format.HorizontalAlignment = this.HorizontalAlignment;
    if (format.Keep != this.Keep)
      format.Keep = this.Keep;
    if (format.KeepFollow != this.KeepFollow)
      format.KeepFollow = this.KeepFollow;
    if ((double) format.LeftIndent != (double) this.LeftIndent)
      format.SetPropertyValue(2, (object) this.LeftIndent);
    if ((double) format.LineSpacing != (double) this.LineSpacing)
      format.SetPropertyValue(52, (object) this.LineSpacing);
    if (format.LineSpacingRule != this.LineSpacingRule)
    {
      format.LineSpacingRule = this.LineSpacingRule;
      if (!format.HasValue(52))
        format.SetPropertyValue(52, (object) this.LineSpacing);
    }
    if (format.MirrorIndents != this.MirrorIndents)
      format.MirrorIndents = this.MirrorIndents;
    if (format.OutlineLevel != this.OutlineLevel)
      format.OutlineLevel = this.OutlineLevel;
    if (format.PageBreakAfter != this.PageBreakAfter)
      format.PageBreakAfter = this.PageBreakAfter;
    if (format.PageBreakBefore != this.PageBreakBefore)
      format.PageBreakBefore = this.PageBreakBefore;
    if ((double) format.RightIndent != (double) this.RightIndent)
      format.SetPropertyValue(3, (object) this.RightIndent);
    if (format.SpaceAfterAuto != this.SpaceAfterAuto)
      format.SpaceAfterAuto = this.SpaceAfterAuto;
    if (format.SpaceBeforeAuto != this.SpaceBeforeAuto)
      format.SpaceBeforeAuto = this.SpaceBeforeAuto;
    if (format.SuppressAutoHyphens != this.SuppressAutoHyphens)
      format.SuppressAutoHyphens = this.SuppressAutoHyphens;
    if (format.TextureStyle != this.TextureStyle)
      format.TextureStyle = this.TextureStyle;
    if (format.WidowControl != this.WidowControl)
      format.WidowControl = this.WidowControl;
    if (format.WrapFrameAround != this.WrapFrameAround)
      format.WrapFrameAround = this.WrapFrameAround;
    if ((double) format.LeftIndentChars != (double) this.LeftIndentChars)
      format.LeftIndentChars = this.LeftIndentChars;
    if ((double) format.FirstLineIndentChars != (double) this.FirstLineIndentChars)
      format.FirstLineIndentChars = this.FirstLineIndentChars;
    if ((double) format.RightIndentChars != (double) this.RightIndentChars)
      format.RightIndentChars = this.RightIndentChars;
    this.Borders.UpdateSourceFormatting(format.Borders);
    this.CompareListFormat(format);
  }

  private void CompareListFormat(WParagraphFormat format)
  {
    if (!(this.OwnerBase is WParagraph) || (this.OwnerBase as WParagraph).ListFormat.ListType == ListType.NoList || (this.OwnerBase as WParagraph).ListFormat.CurrentListLevel == null)
      return;
    int[] array = new int[format.PropertiesHash.Count];
    format.PropertiesHash.Keys.CopyTo(array, 0);
    foreach (int key in array)
    {
      if (!this.PropertiesHash.ContainsKey(key) && (this.OwnerBase as WParagraph).ListFormat.CurrentListLevel.ParagraphFormat.IsValueDefined(key))
        format.RemoveValue(key);
    }
  }

  internal void NestedParaFormatting(WParagraphFormat format)
  {
    if (!format.HasKey(80 /*0x50*/) && this.HasKey(80 /*0x50*/))
      format.AdjustRightIndent = this.AdjustRightIndent;
    if (!format.HasKey(9) && this.HasKey(9))
      format.SetPropertyValue(9, (object) this.AfterSpacing);
    if (!format.HasKey(81) && this.HasKey(81))
      format.AutoSpaceDE = this.AutoSpaceDE;
    if (!format.HasKey(82) && this.HasKey(82))
      format.AutoSpaceDN = this.AutoSpaceDN;
    if (!format.HasKey(21) && this.HasKey(21))
      format.BackColor = this.BackColor;
    if (!format.HasKey(8) && this.HasKey(8))
      format.SetPropertyValue(8, (object) this.BeforeSpacing);
    if (!format.HasKey(31 /*0x1F*/) && this.HasKey(31 /*0x1F*/))
      format.Bidi = this.Bidi;
    if (!format.HasKey(22) && this.HasKey(22))
      format.ColumnBreakAfter = this.ColumnBreakAfter;
    if (!format.HasKey(92) && this.HasKey(92))
      format.ContextualSpacing = this.ContextualSpacing;
    if (!format.HasKey(5) && this.HasKey(5))
      format.SetPropertyValue(5, (object) this.FirstLineIndent);
    if (!format.HasKey(32 /*0x20*/) && this.HasKey(32 /*0x20*/))
      format.ForeColor = this.ForeColor;
    if (!format.HasKey(77) && this.HasKey(77))
      format.FrameHeight = this.FrameHeight;
    if (!format.HasKey(83) && this.HasKey(83))
      format.FrameHorizontalDistanceFromText = this.FrameHorizontalDistanceFromText;
    if (!format.HasKey(71) && this.HasKey(71))
      format.FrameHorizontalPos = this.FrameHorizontalPos;
    if (!format.HasKey(84) && this.HasKey(84))
      format.FrameVerticalDistanceFromText = this.FrameVerticalDistanceFromText;
    if (!format.HasKey(72) && this.HasKey(72))
      format.FrameVerticalPos = this.FrameVerticalPos;
    if (!format.HasKey(76) && this.HasKey(76))
      format.FrameWidth = this.FrameWidth;
    if (!format.HasKey(73) && this.HasKey(73))
      format.FrameX = this.FrameX;
    if (!format.HasKey(74) && this.HasKey(74))
      format.FrameY = this.FrameY;
    if (!format.HasKey(0) && this.HasKey(0))
      format.HorizontalAlignment = this.HorizontalAlignment;
    if (!format.HasKey(6) && this.HasKey(6))
      format.Keep = this.Keep;
    if (!format.HasKey(10) && this.HasKey(10))
      format.KeepFollow = this.KeepFollow;
    if (!format.HasKey(2) && this.HasKey(2))
      format.SetPropertyValue(2, (object) this.LeftIndent);
    if (!format.HasKey(52) && this.HasKey(52))
      format.SetPropertyValue(52, (object) this.LineSpacing);
    if (!format.HasKey(53) && this.HasKey(53))
    {
      format.LineSpacingRule = this.LineSpacingRule;
      if (!format.HasValue(52) && this.HasKey(52))
        format.SetPropertyValue(52, (object) this.LineSpacing);
    }
    if (!format.HasKey(75) && this.HasKey(75))
      format.MirrorIndents = this.MirrorIndents;
    if (!format.HasKey(56) && this.HasKey(56))
      format.OutlineLevel = this.OutlineLevel;
    if (!format.HasKey(13) && this.HasKey(13))
      format.PageBreakAfter = this.PageBreakAfter;
    if (!format.HasKey(12) && this.HasKey(12))
      format.PageBreakBefore = this.PageBreakBefore;
    if (!format.HasKey(3) && this.HasKey(3))
      format.SetPropertyValue(3, (object) this.RightIndent);
    if (!format.HasKey(55) && this.HasKey(55))
      format.SpaceAfterAuto = this.SpaceAfterAuto;
    if (!format.HasKey(54) && this.HasKey(54))
      format.SpaceBeforeAuto = this.SpaceBeforeAuto;
    if (!format.HasKey(78) && this.HasKey(78))
      format.SuppressAutoHyphens = this.SuppressAutoHyphens;
    if (!format.HasKey(33) && this.HasKey(33))
      format.TextureStyle = this.TextureStyle;
    if (!format.HasKey(11) && this.HasKey(11))
      format.WidowControl = this.WidowControl;
    if (!format.HasKey(88) && this.HasKey(88))
      format.WrapFrameAround = this.WrapFrameAround;
    if (!format.HasKey(85) && this.HasKey(85))
      format.LeftIndentChars = this.LeftIndentChars;
    if (!format.HasKey(86) && this.HasKey(86))
      format.FirstLineIndentChars = this.FirstLineIndentChars;
    if (!format.HasKey(87) && this.HasKey(87))
      format.RightIndentChars = this.RightIndentChars;
    if (!format.Borders.NoBorder || this.Borders.NoBorder)
      return;
    this.Borders.UpdateSourceFormatting(format.Borders);
  }

  private bool IsValueDefined(int key)
  {
    while (key > (int) byte.MaxValue)
      key >>= 8;
    return this.HasValue(key);
  }

  private void RemoveValue(int key) => this.PropertiesHash.Remove(key);

  public override void ClearFormatting()
  {
    if (this.m_propertiesHash != null)
      this.m_propertiesHash.Clear();
    if (this.m_sprms != null)
      this.m_sprms.Clear();
    if (this.m_xmlProps == null)
      return;
    this.m_xmlProps.Clear();
  }

  internal void SetFrameHorizontalDistanceFromTextValue(float value)
  {
    if ((double) value < 0.0 || (double) value > 1638.0)
      value = 0.0f;
    this.FrameHorizontalDistanceFromText = value;
  }

  internal void SetFrameVerticalDistanceFromTextValue(float value)
  {
    if ((double) value < 0.0 || (double) value > 1638.0)
      value = 0.0f;
    this.FrameVerticalDistanceFromText = value;
  }

  internal void SetFrameYValue(float value)
  {
    if ((double) value < -1584.0 && (double) value > 1584.0)
      value = 0.0f;
    this.FrameY = value;
  }

  internal void SetFrameXValue(float value)
  {
    if ((double) value < -1584.0 && (double) value > 1584.0)
      value = 0.0f;
    this.FrameX = value;
  }

  internal void SetFrameWidthValue(float value)
  {
    if ((double) value < 0.05 || (double) value > 1584.0)
      value = 0.0f;
    this.FrameWidth = value;
  }

  internal bool Compare(WParagraphFormat paragraphFormat)
  {
    return this.Compare(91, (FormatBase) paragraphFormat) && this.Compare(9, (FormatBase) paragraphFormat) && this.Compare(81, (FormatBase) paragraphFormat) && this.Compare(82, (FormatBase) paragraphFormat) && this.Compare(21, (FormatBase) paragraphFormat) && this.Compare(90, (FormatBase) paragraphFormat) && this.Compare(8, (FormatBase) paragraphFormat) && this.Compare(31 /*0x1F*/, (FormatBase) paragraphFormat) && this.Compare(22, (FormatBase) paragraphFormat) && this.Compare(92, (FormatBase) paragraphFormat) && this.Compare(5, (FormatBase) paragraphFormat) && this.Compare(86, (FormatBase) paragraphFormat) && this.Compare(32 /*0x20*/, (FormatBase) paragraphFormat) && this.Compare(77, (FormatBase) paragraphFormat) && this.Compare(83, (FormatBase) paragraphFormat) && this.Compare(71, (FormatBase) paragraphFormat) && this.Compare(84, (FormatBase) paragraphFormat) && this.Compare(72, (FormatBase) paragraphFormat) && this.Compare(76, (FormatBase) paragraphFormat) && this.Compare(73, (FormatBase) paragraphFormat) && this.Compare(74, (FormatBase) paragraphFormat) && this.Compare(65, (FormatBase) paragraphFormat) && this.Compare(32 /*0x20*/, (FormatBase) paragraphFormat) && this.Compare(77, (FormatBase) paragraphFormat) && this.Compare(6, (FormatBase) paragraphFormat) && this.Compare(10, (FormatBase) paragraphFormat) && this.HorizontalAlignment == paragraphFormat.HorizontalAlignment && this.IsFrame == paragraphFormat.IsFrame && this.Compare(2, (FormatBase) paragraphFormat) && this.Compare(85, (FormatBase) paragraphFormat) && this.Compare(52, (FormatBase) paragraphFormat) && this.Compare(53, (FormatBase) paragraphFormat) && this.Compare(0, (FormatBase) paragraphFormat) && this.Compare(75, (FormatBase) paragraphFormat) && this.Compare(56, (FormatBase) paragraphFormat) && this.Compare(13, (FormatBase) paragraphFormat) && this.Compare(12, (FormatBase) paragraphFormat) && this.Compare(3, (FormatBase) paragraphFormat) && this.Compare(87, (FormatBase) paragraphFormat) && this.Compare(55, (FormatBase) paragraphFormat) && this.Compare(54, (FormatBase) paragraphFormat) && this.Compare(78, (FormatBase) paragraphFormat) && this.Compare(33, (FormatBase) paragraphFormat) && this.Compare(11, (FormatBase) paragraphFormat) && this.Compare(89, (FormatBase) paragraphFormat) && this.Compare(88, (FormatBase) paragraphFormat) && this.Tabs.Compare(paragraphFormat.Tabs) && (this.Borders == null || paragraphFormat.Borders == null || this.Borders.Compare(paragraphFormat.Borders));
  }
}
