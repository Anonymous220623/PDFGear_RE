// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WParagraph
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.Compression.Zip;
using Syncfusion.DocIO.DLS.Rendering;
using Syncfusion.DocIO.DLS.XML;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using Syncfusion.DocIO.ReaderWriter.DataStreamParser.OLEObject;
using Syncfusion.DocIO.Rendering;
using Syncfusion.Layouting;
using Syncfusion.Office;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WParagraph : 
  TextBodyItem,
  IWidgetContainer,
  IWidget,
  IWParagraph,
  ITextBodyItem,
  IStyleHolder,
  ICompositeEntity,
  IEntity
{
  private const string DEF_NORMAL_STYLE = "Normal";
  private const string DEF_DEFAULTPARAGRAPHFONT_STYLE_ID = "Default Paragraph Font";
  private const int DEF_LIST_STYLE_ID = 179;
  private const int DEF_USER_STYLE_ID = 4094;
  private string m_paraId = "";
  protected IWParagraphStyle m_style;
  private StringBuilder m_strTextBuilder = new StringBuilder(1);
  internal string m_liststring = string.Empty;
  protected WParagraphFormat m_prFormat;
  protected WListFormat m_listFormat;
  private byte m_bFlags;
  protected ParagraphItemCollection m_pItemColl;
  private ParagraphItemCollection m_pEmptyItemColl;
  private ParagraphItemCollection m_paragraphItems;
  private WCharacterFormat m_charFormat;
  private TextBodyItem m_ownerTextBodyItem;
  internal bool IsFloatingItemsLayouted;
  internal bool IsXpositionUpated;

  internal bool HasSDTInlineItem
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
  }

  internal bool SplitWidgetContainerDrawn
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal bool IsStyleApplied
  {
    get => ((int) this.m_bFlags & 4) >> 2 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 251 | (value ? 1 : 0) << 2);
  }

  internal bool IsNeedToMeasureBookMarkSize
  {
    get
    {
      if (this.SectionEndMark)
        return false;
      for (int index = 0; index < this.ChildEntities.Count; ++index)
      {
        if (!(this.ChildEntities[index] is BookmarkStart) && !(this.ChildEntities[index] is BookmarkEnd))
          return false;
      }
      return true;
    }
  }

  public override EntityType EntityType => EntityType.Paragraph;

  public EntityCollection ChildEntities => (EntityCollection) this.m_pItemColl;

  public string StyleName => this.m_style == null ? (string) null : this.m_style.Name;

  public string ListString => this.m_liststring;

  internal string ParaId
  {
    get => this.m_paraId;
    set => this.m_paraId = value;
  }

  public string Text
  {
    get => this.m_strTextBuilder.ToString();
    set
    {
      this.Items.Clear();
      this.AppendText(value).CharacterFormat.ImportContainer((FormatBase) this.BreakCharacterFormat);
    }
  }

  public bool EnableStyleSeparator
  {
    get => this.m_charFormat != null && this.m_charFormat.SpecVanish && this.m_charFormat.Hidden;
    set
    {
      if (this.m_charFormat == null)
        return;
      this.m_charFormat.SpecVanish = this.m_charFormat.Hidden = value;
    }
  }

  public ParagraphItem this[int index] => this.m_pItemColl[index];

  public ParagraphItemCollection Items => this.m_pItemColl;

  public WParagraphFormat ParagraphFormat => this.m_prFormat;

  public WCharacterFormat BreakCharacterFormat => this.m_charFormat;

  public WListFormat ListFormat
  {
    get
    {
      if (this.m_listFormat == null)
        this.m_listFormat = new WListFormat((IWParagraph) this);
      return this.m_listFormat;
    }
  }

  public bool IsInCell
  {
    get
    {
      if (this.Owner is WTableCell)
        return true;
      return this.Owner is WTextBody && (this.Owner as WTextBody).Owner is BlockContentControl && this.GetOwnerEntity() is WTableCell;
    }
  }

  public bool IsEndOfSection => this.Owner.Owner is WSection && this.NextSibling == null;

  public bool IsEndOfDocument
  {
    get => this.IsEndOfSection && (this.Owner.Owner as WSection).NextSibling == null;
  }

  internal bool IsLastItem
  {
    get => ((int) this.m_bFlags & 32 /*0x20*/) >> 5 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 223 | (value ? 1 : 0) << 5);
  }

  internal bool IsNeedToSkip
  {
    get => ((int) this.m_bFlags & 64 /*0x40*/) >> 6 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 191 | (value ? 1 : 0) << 6);
  }

  internal bool IsTopMarginValueUpdated
  {
    get => ((int) this.m_bFlags & 128 /*0x80*/) >> 7 != 0;
    set
    {
      this.m_bFlags = (byte) ((int) this.m_bFlags & (int) sbyte.MaxValue | (value ? 1 : 0) << 7);
    }
  }

  int IWidgetContainer.Count => this.WidgetCollection.Count;

  EntityCollection IWidgetContainer.WidgetInnerCollection
  {
    get => this.WidgetCollection as EntityCollection;
  }

  IWidget IWidgetContainer.this[int index]
  {
    get => (this.WidgetCollection as ParagraphItemCollection).GetCurrentWidget(index);
  }

  protected IEntityCollectionBase WidgetCollection
  {
    get
    {
      if (this.m_pItemColl.Count == 0)
        return (IEntityCollectionBase) this.m_pEmptyItemColl;
      if (this.HasSDTInlineItem)
      {
        ParagraphItemCollection paragraphItems = this.GetParagraphItems();
        if (paragraphItems.Count == 0)
          return (IEntityCollectionBase) this.m_pEmptyItemColl;
        if (this.IsNeedToAddEmptyTextRangeForBreakItem(paragraphItems[paragraphItems.Count - 1] as Break) || this.IsContainFloatingItems())
          paragraphItems.AddToInnerList((Entity) this.m_pEmptyItemColl[0]);
        return (IEntityCollectionBase) paragraphItems;
      }
      if (!this.IsNeedToAddEmptyTextRangeForBreakItem(this.m_pItemColl[this.m_pItemColl.Count - 1] as Break) && !this.IsContainFloatingItems())
        return (IEntityCollectionBase) this.m_pItemColl;
      if (this.m_paragraphItems == null || this.m_pItemColl.Owner != this.m_paragraphItems.Owner || this.m_pItemColl.Count != this.m_paragraphItems.Count - 1)
      {
        this.m_paragraphItems = this.GetParagraphItems();
        this.m_paragraphItems.AddToInnerList((Entity) this.m_pEmptyItemColl[0]);
      }
      return (IEntityCollectionBase) this.m_paragraphItems;
    }
  }

  private bool IsNeedToAddEmptyTextRangeForBreakItem(Break breakItem)
  {
    if (breakItem == null || breakItem.CharacterFormat.Hidden)
      return false;
    return breakItem.BreakType == BreakType.LineBreak || breakItem.BreakType == BreakType.PageBreak && (this == this.Document.LastParagraph && this.NextSibling == null || this.Document.Settings.CompatibilityOptions[CompatibilityOption.SplitPgBreakAndParaMark] || (this.Document.ActualFormatType == FormatType.Doc ? (this.Document.WordVersion <= (ushort) 268 ? 1 : 0) : 0) != 0) || breakItem.BreakType == BreakType.ColumnBreak;
  }

  internal bool IsContainDinOffcFont()
  {
    bool isDinFontOccur = false;
    string fontName = "";
    for (int index = 0; index < this.ChildEntities.Count; ++index)
    {
      if (this.ChildEntities[index] is WTextRange)
      {
        fontName = (this.ChildEntities[index] as WTextRange).CharacterFormat.GetFontNameToRender((this.ChildEntities[index] as WTextRange).ScriptType);
        if (!(fontName == "DIN Offc") && !(fontName == "DIN OT"))
          return false;
        isDinFontOccur = true;
      }
    }
    return this.IsDinFontCreatable(fontName, isDinFontOccur);
  }

  internal bool IsDinFontCreatable(string fontName, bool isDinFontOccur)
  {
    return isDinFontOccur && this.CreateFont(fontName, 10f, FontStyle.Regular).Name == fontName;
  }

  private Font CreateFont(string fontName, float fontSize, FontStyle fontStyle)
  {
    try
    {
      return new Font(fontName, fontSize, fontStyle);
    }
    catch
    {
      FontFamily fontFamily = new FontFamily(fontName);
      if (fontFamily.IsStyleAvailable(FontStyle.Bold))
        fontStyle |= FontStyle.Bold;
      if (fontFamily.IsStyleAvailable(FontStyle.Italic))
        fontStyle |= FontStyle.Italic;
      if (fontFamily.IsStyleAvailable(FontStyle.Underline))
        fontStyle |= FontStyle.Underline;
      if (fontFamily.IsStyleAvailable(FontStyle.Strikeout))
        fontStyle |= FontStyle.Strikeout;
      return new Font(fontName, fontSize, fontStyle);
    }
  }

  internal bool IsContainFloatingItems()
  {
    for (int index = 0; index < this.ChildEntities.Count; ++index)
    {
      IEntity childEntity = (IEntity) this.ChildEntities[index];
      int num;
      switch (childEntity)
      {
        case WTextBox _:
          num = (int) (childEntity as WTextBox).TextBoxFormat.TextWrappingStyle;
          break;
        case WPicture _:
          num = (int) (childEntity as WPicture).TextWrappingStyle;
          break;
        case Shape _:
          num = (int) (childEntity as Shape).WrapFormat.TextWrappingStyle;
          break;
        case GroupShape _:
          num = (int) (childEntity as GroupShape).WrapFormat.TextWrappingStyle;
          break;
        case WOleObject _ when (childEntity as WOleObject).OlePicture != null:
          num = (int) (childEntity as WOleObject).OlePicture.TextWrappingStyle;
          break;
        case WChart _:
          num = (int) (childEntity as WChart).WrapFormat.TextWrappingStyle;
          break;
        default:
          num = 0;
          break;
      }
      TextWrappingStyle textWrappingStyle = (TextWrappingStyle) num;
      switch (childEntity)
      {
        case BookmarkStart _:
        case BookmarkEnd _:
        case WFieldMark _:
          continue;
        default:
          if (textWrappingStyle == TextWrappingStyle.Inline)
            return false;
          continue;
      }
    }
    return true;
  }

  internal bool RemoveEmpty
  {
    get => ((int) this.m_bFlags & 16 /*0x10*/) >> 4 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 239 | (value ? 1 : 0) << 4);
  }

  internal ParagraphItem LastItem => this[this.m_pItemColl.Count - 1];

  internal IWParagraphStyle ParaStyle
  {
    get => this.m_style;
    set
    {
      if (this.m_style is WParagraphStyle style && !style.IsRemoving && style.IsCustom && style.RangeCollection.Contains((Entity) this))
        style.RangeCollection.Remove((Entity) this);
      if (value is WParagraphStyle wparagraphStyle && wparagraphStyle.IsCustom)
        wparagraphStyle.RangeCollection.Add((Entity) this);
      this.m_style = value;
      if (!this.ParagraphFormat.IsFormattingChange)
        return;
      this.ParagraphFormat.SetPropertyValue(47, (object) this.m_style.Name);
    }
  }

  internal bool SectionEndMark => this.IsSectionEndMark();

  internal bool IsTextReplaced
  {
    get => ((int) this.m_bFlags & 8) >> 3 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 247 | (value ? 1 : 0) << 3);
  }

  public WParagraph(IWordDocument doc)
    : base((WordDocument) doc)
  {
    this.m_pItemColl = new ParagraphItemCollection(this);
    this.m_charFormat = new WCharacterFormat((IWordDocument) this.Document);
    this.m_prFormat = new WParagraphFormat((IWordDocument) this.Document);
    this.m_listFormat = new WListFormat((IWParagraph) this);
    this.m_charFormat.SetOwner((OwnerHolder) this);
    this.m_prFormat.SetOwner((OwnerHolder) this);
    this.m_listFormat.SetOwner((OwnerHolder) this);
    this.ApplyStyle("Normal", false);
    this.CreateEmptyParagraph();
  }

  internal int GetHeadingLevel(WParagraphStyle style, WParagraph currParagraph)
  {
    WParagraphStyle wparagraphStyle = style;
    if (!currParagraph.ParagraphFormat.PropertiesHash.ContainsKey(56))
    {
      for (; wparagraphStyle != null; wparagraphStyle = wparagraphStyle.BaseStyle)
      {
        if (wparagraphStyle.ParagraphFormat.IsBuiltInHeadingStyle(wparagraphStyle.Name))
        {
          string name = wparagraphStyle.Name;
          for (int index = 1; index < 10; ++index)
          {
            if (Style.BuiltinStyleLoader.BuiltinStyleNames[index] == name)
              return index - 1;
          }
        }
        else if (wparagraphStyle.ParagraphFormat.PropertiesHash.ContainsKey(56))
          return Convert.ToInt32(wparagraphStyle.ParagraphFormat.PropertiesHash[56]);
      }
    }
    else
    {
      if (!wparagraphStyle.ParagraphFormat.IsBuiltInHeadingStyle(wparagraphStyle.Name))
        return Convert.ToInt32(currParagraph.ParagraphFormat.PropertiesHash[56]);
      string name = wparagraphStyle.Name;
      for (int index = 1; index < 10; ++index)
      {
        if (Style.BuiltinStyleLoader.BuiltinStyleNames[index] == name)
          return index - 1;
      }
    }
    return 9;
  }

  private void MoveParagraphItems(
    WParagraph targetParagraph,
    WParagraph sourceParagraph,
    int startIndex)
  {
    int num = sourceParagraph.Items.Count - startIndex;
    for (int index = 0; index < num; ++index)
    {
      Entity childEntity = sourceParagraph.ChildEntities[startIndex];
      switch (childEntity)
      {
        case WTextRange _ when this.ModifyText((childEntity as WTextRange).Text).IndexOf('\r') != -1:
          return;
        case WField _:
        case WFormField _:
          targetParagraph.ChildEntities.AddToInnerList(childEntity);
          childEntity.SetOwner((OwnerHolder) targetParagraph);
          sourceParagraph.ChildEntities.RemoveFromInnerList(startIndex);
          (childEntity as ParagraphItem).StartPos = (targetParagraph.ChildEntities.InnerList[targetParagraph.ChildEntities.InnerList.Count - 2] as ParagraphItem).EndPos;
          break;
        default:
          targetParagraph.ChildEntities.Add((IEntity) childEntity);
          break;
      }
    }
  }

  internal bool IsNeedToFitSymbol(WParagraph ownerParagraph)
  {
    Entity ownerEntity = ownerParagraph.GetOwnerEntity();
    int num1;
    switch (ownerEntity)
    {
      case ChildShape _:
        num1 = (int) (ownerEntity as ChildShape).AutoShapeType;
        break;
      case Shape _:
        num1 = (int) (ownerEntity as Shape).AutoShapeType;
        break;
      default:
        num1 = 224 /*0xE0*/;
        break;
    }
    AutoShapeType autoShapeType = (AutoShapeType) num1;
    int num2;
    switch (autoShapeType)
    {
      case AutoShapeType.RoundedRectangle:
      case AutoShapeType.Oval:
        num2 = 1;
        break;
      default:
        num2 = autoShapeType == AutoShapeType.IsoscelesTriangle ? 1 : 0;
        break;
    }
    bool flag = num2 != 0;
    return ownerEntity is WTextBox || flag;
  }

  internal bool HasInlineItem(int endIndex)
  {
    for (int index = endIndex; index >= 0; --index)
    {
      if (!this.Items[index].IsFloatingItem(false))
        return true;
    }
    return false;
  }

  internal void SplitTextRange()
  {
    if (!this.ModifyText(this.Text).Contains(ControlChar.CarriegeReturn))
      return;
    int num = 0;
    WParagraph targetParagraph = (WParagraph) null;
    bool flag = false;
    WCharacterFormat format = new WCharacterFormat((IWordDocument) this.Document);
    format.ImportContainer((FormatBase) this.BreakCharacterFormat);
    format.CopyProperties((FormatBase) this.BreakCharacterFormat);
    for (int index1 = 0; index1 < this.ChildEntities.Count; ++index1)
    {
      if (this.ChildEntities[index1].EntityType == EntityType.TextRange)
      {
        WTextRange childEntity = this.ChildEntities[index1] as WTextRange;
        string str = this.ModifyText(childEntity.Text);
        if (str.IndexOf('\r') != -1 && childEntity.Owner is WParagraph)
        {
          if (num > 0 && targetParagraph != null)
          {
            targetParagraph.ChildEntities.Add((IEntity) childEntity);
            --index1;
          }
          string[] strArray = str.Split('\r');
          childEntity.Text = strArray[0];
          int inOwnerCollection = this.GetIndexInOwnerCollection();
          WTextRange wtextRange = (WTextRange) null;
          for (int index2 = 1; index2 < strArray.Length; ++index2)
          {
            targetParagraph = new WParagraph((IWordDocument) this.Document);
            wtextRange = new WTextRange((IWordDocument) this.Document);
            WTextBody owner = childEntity.OwnerParagraph.Owner as WTextBody;
            targetParagraph.ParagraphFormat.ImportContainer((FormatBase) this.ParagraphFormat);
            targetParagraph.ParagraphFormat.CopyProperties((FormatBase) this.ParagraphFormat);
            targetParagraph.ListFormat.ImportContainer((FormatBase) this.ListFormat);
            owner.ChildEntities.Insert(inOwnerCollection + index2 + num, (IEntity) targetParagraph);
            wtextRange.Text = strArray[index2];
            wtextRange.CharacterFormat.ImportContainer((FormatBase) childEntity.CharacterFormat);
            wtextRange.CharacterFormat.CopyProperties((FormatBase) childEntity.CharacterFormat);
            targetParagraph.Items.Insert(0, (IEntity) wtextRange);
            if (this.StyleName != null)
              targetParagraph.ApplyStyle(this.StyleName, false);
            if (index2 == strArray.Length - 1)
            {
              targetParagraph.BreakCharacterFormat.ImportContainer((FormatBase) format);
              targetParagraph.BreakCharacterFormat.CopyProperties((FormatBase) format);
            }
            else
            {
              targetParagraph.BreakCharacterFormat.ImportContainer((FormatBase) wtextRange.CharacterFormat);
              targetParagraph.BreakCharacterFormat.CopyProperties((FormatBase) wtextRange.CharacterFormat);
            }
          }
          num += strArray.Length - 1;
          this.MoveParagraphItems(targetParagraph, this, index1 + 1);
          if (!flag)
          {
            flag = true;
            this.BreakCharacterFormat.ClearFormatting();
            this.BreakCharacterFormat.ImportContainer((FormatBase) childEntity.CharacterFormat);
            this.BreakCharacterFormat.CopyProperties((FormatBase) childEntity.CharacterFormat);
          }
          if (index1 < this.ChildEntities.Count - 1)
          {
            targetParagraph.BreakCharacterFormat.ImportContainer((FormatBase) wtextRange.CharacterFormat);
            targetParagraph.BreakCharacterFormat.CopyProperties((FormatBase) wtextRange.CharacterFormat);
          }
        }
      }
    }
  }

  internal void InsertBreak(BreakType breakType)
  {
    if (this.Owner == null)
      return;
    switch (breakType)
    {
      case BreakType.PageBreak:
        this.ParagraphFormat.PageBreakAfter = false;
        break;
      case BreakType.ColumnBreak:
        this.ParagraphFormat.ColumnBreakAfter = false;
        break;
    }
    if (this.NextSibling is WParagraph)
    {
      (this.NextSibling as WParagraph).Items.Insert(0, (IEntity) new Break((IWordDocument) this.Document, breakType));
    }
    else
    {
      int inOwnerCollection = this.GetIndexInOwnerCollection();
      WParagraph wparagraph = new WParagraph((IWordDocument) this.Document);
      wparagraph.AppendBreak(breakType);
      wparagraph.AppendText(" ");
      ICompositeEntity owner = this.Owner as ICompositeEntity;
      if (owner.ChildEntities.Count == inOwnerCollection + 1)
        owner.ChildEntities.Add((IEntity) wparagraph);
      else
        owner.ChildEntities.Insert(inOwnerCollection + 1, (IEntity) wparagraph);
    }
  }

  public void ApplyStyle(string styleName) => this.ApplyStyle(styleName, true);

  internal void ApplyStyle(string styleName, bool isDomChanges)
  {
    this.IsStyleApplied = true;
    IStyle style1 = (IStyle) (this.Document.Styles.FindByName(styleName, StyleType.ParagraphStyle) as IWParagraphStyle);
    if (style1 == null && styleName == "Normal")
      style1 = Style.CreateBuiltinStyle(BuiltinStyle.Normal, this.Document);
    if (style1 != null)
    {
      this.ApplyStyle((IWParagraphStyle) (style1 as WParagraphStyle), isDomChanges);
    }
    else
    {
      IStyle style2 = (IStyle) (this.Document.Styles.FindByName(styleName, StyleType.CharacterStyle) as WCharacterStyle);
      if (style2 == null && styleName == "Default Paragraph Font")
        style2 = Style.CreateBuiltinCharacterStyle(BuiltinStyle.DefaultParagraphFont, this.Document);
      if (style2 != null)
        this.ApplyCharacterStyle(style2 as IWCharacterStyle);
      if (style2 == null)
      {
        style2 = this.Document.Styles.FindByName(styleName, StyleType.NumberingStyle);
        if (style2 != null)
        {
          this.ApplyStyle("List Paragraph", isDomChanges);
          this.ListFormat.ApplyStyle((style2 as WNumberingStyle).ListFormat.CurrentListStyle.Name);
          if (!string.IsNullOrEmpty((style2 as WNumberingStyle).ListFormat.LFOStyleName))
            this.ListFormat.LFOStyleName = (style2 as WNumberingStyle).ListFormat.LFOStyleName;
        }
      }
      if (style2 == null)
        throw new ArgumentException("Specified style does not exist in the document style collection");
    }
  }

  public void ApplyStyle(BuiltinStyle builtinStyle) => this.ApplyStyle(builtinStyle, true);

  internal void ApplyStyle(BuiltinStyle builtinStyle, bool isDomChanges)
  {
    this.IsStyleApplied = true;
    bool flag = Style.IsListStyle(builtinStyle);
    this.CheckNormalStyle();
    if (flag)
    {
      this.ApplyListStyle(builtinStyle);
    }
    else
    {
      string name = Style.BuiltInToName(builtinStyle);
      if (this.IsBuiltInCharacterStyle(builtinStyle))
      {
        IStyle style = (IStyle) (this.Document.Styles.FindByName(name, StyleType.CharacterStyle) as WCharacterStyle);
        if (style == null)
        {
          style = Style.CreateBuiltinCharacterStyle(builtinStyle, this.Document);
          if ((style as WCharacterStyle).StyleId > 10)
            (style as WCharacterStyle).StyleId = 4094;
          (style as Style).UnhideWhenUsed = true;
          this.Document.Styles.Add(style);
          (style as WCharacterStyle).ApplyBaseStyle("Default Paragraph Font");
        }
        this.ApplyCharacterStyle((IWCharacterStyle) (style as WCharacterStyle));
      }
      else
      {
        IStyle style = (IStyle) (this.Document.Styles.FindByName(name, StyleType.ParagraphStyle) as IWParagraphStyle);
        if (style == null)
        {
          style = Style.CreateBuiltinStyle(builtinStyle, this.Document);
          if ((style as WParagraphStyle).StyleId > 10)
            (style as WParagraphStyle).StyleId = 4094;
          this.Document.Styles.Add(style);
          if (builtinStyle != BuiltinStyle.MacroText && builtinStyle != BuiltinStyle.CommentSubject)
            (style as WParagraphStyle).ApplyBaseStyle("Normal");
          this.Document.UpdateNextStyle(style as Style);
        }
        this.ApplyStyle(style as IWParagraphStyle, isDomChanges);
      }
    }
  }

  public IWParagraphStyle GetStyle() => this.m_style;

  public void RemoveAbsPosition()
  {
    if (this.m_prFormat == null)
      return;
    this.m_prFormat.RemovePositioning();
  }

  internal bool IsExactlyRowHeight(WTableCell ownerTableCell, ref float rowHeight)
  {
    if ((((IWidget) ownerTableCell).LayoutInfo as CellLayoutInfo).IsRowMergeStart)
    {
      WTableRow ownerRow = ownerTableCell.GetOwnerRow(ownerTableCell);
      if (ownerRow != null)
      {
        rowHeight = ownerRow.Height;
        return ownerRow.HeightType == TableRowHeightType.Exactly;
      }
    }
    else
    {
      if (ownerTableCell == null || ownerTableCell.OwnerRow == null || ownerTableCell.OwnerRow.HeightType == TableRowHeightType.Exactly || ownerTableCell.OwnerRow.OwnerTable == null || !ownerTableCell.OwnerRow.OwnerTable.IsInCell)
        return ownerTableCell.OwnerRow.HeightType == TableRowHeightType.Exactly;
      ownerTableCell = ownerTableCell.OwnerRow.OwnerTable.GetOwnerTableCell();
      if (ownerTableCell != null)
      {
        rowHeight = ownerTableCell.OwnerRow.Height;
        return this.IsExactlyRowHeight(ownerTableCell, ref rowHeight);
      }
    }
    return false;
  }

  internal bool IsExactlyRowHeight()
  {
    if (!this.IsInCell)
      return false;
    float rowHeight = 0.0f;
    return this.IsExactlyRowHeight(this.GetOwnerEntity() as WTableCell, ref rowHeight);
  }

  public IWTextRange AppendText(string text)
  {
    IWTextRange wtextRange = this.AppendItem(ParagraphItemType.TextRange) as IWTextRange;
    wtextRange.Text = text;
    return wtextRange;
  }

  public IInlineContentControl AppendInlineContentControl(ContentControlType controlType)
  {
    switch (controlType)
    {
      case ContentControlType.BuildingBlockGallery:
      case ContentControlType.Group:
      case ContentControlType.RepeatingSection:
        throw new NotImplementedException($"Creating a content control for the {(object) controlType}type is not implemented");
      default:
        InlineContentControl inlineContentControl = this.AppendItem(ParagraphItemType.InlineContentControl) as InlineContentControl;
        inlineContentControl.ContentControlProperties.Type = controlType;
        if (controlType == ContentControlType.CheckBox)
        {
          inlineContentControl.ContentControlProperties.CheckedState.Value = '☒'.ToString();
          inlineContentControl.ContentControlProperties.CheckedState.Font = "MS Gothic";
          inlineContentControl.ContentControlProperties.UncheckedState.Value = '☐'.ToString();
          inlineContentControl.ContentControlProperties.UncheckedState.Font = "MS Gothic";
        }
        this.HasSDTInlineItem = true;
        return (IInlineContentControl) inlineContentControl;
    }
  }

  public IWPicture AppendPicture(byte[] imageBytes)
  {
    IWPicture wpicture = (IWPicture) this.AppendItem(ParagraphItemType.Picture);
    wpicture.LoadImage(imageBytes);
    this.Document.HasPicture = true;
    return wpicture;
  }

  public IWPicture AppendPicture(byte[] svgData, byte[] imageBytes)
  {
    WPicture.EvaluateSVGImageBytes(svgData);
    WPicture wpicture = this.AppendItem(ParagraphItemType.Picture) as WPicture;
    wpicture.LoadImage(svgData, imageBytes);
    this.Document.HasPicture = true;
    return (IWPicture) wpicture;
  }

  public WChart AppendChart(object[][] data, float width, float height)
  {
    WChart wchart = this.AppendItem(ParagraphItemType.Chart) as WChart;
    wchart.OfficeChart.SetChartData(data);
    wchart.Width = width;
    wchart.Height = height;
    return wchart;
  }

  public WChart AppendChart(float width, float height)
  {
    WChart wchart = this.AppendItem(ParagraphItemType.Chart) as WChart;
    wchart.Width = width;
    wchart.Height = height;
    return wchart;
  }

  public WChart AppendChart(
    Stream excelStream,
    int sheetNumber,
    string dataRange,
    float width,
    float height)
  {
    this.DetectExcelFileFromStream(excelStream);
    if (sheetNumber <= 0)
      throw new ArgumentOutOfRangeException("Sheet number should be greater than zero");
    if (string.IsNullOrEmpty(dataRange))
      throw new ArgumentNullException("Data range should not be null");
    WChart wchart = this.AppendItem(ParagraphItemType.Chart) as WChart;
    wchart.Workbook.DataHolder.ParseDocument(excelStream);
    wchart.Width = width;
    wchart.Height = height;
    wchart.SetDataRange(sheetNumber, dataRange);
    return wchart;
  }

  private void DetectExcelFileFromStream(Stream stream)
  {
    long num = stream != null ? stream.Position : throw new ArgumentNullException("Excelstream should not be null");
    if (ZipArchive.ReadInt32(stream) != 67324752)
      throw new ArgumentException("Excel stream should be *.xlsx format");
    stream.Position = num;
  }

  public IWField AppendField(string fieldName, FieldType fieldType)
  {
    if (fieldName == null)
      throw new ArgumentNullException(nameof (fieldName));
    WField wfield;
    switch (fieldType)
    {
      case FieldType.FieldIndexEntry:
        return this.AppendIndexEntry(fieldName);
      case FieldType.FieldIf:
        wfield = (WField) new WIfField((IWordDocument) this.Document);
        break;
      case FieldType.FieldSequence:
        wfield = (WField) new WSeqField((IWordDocument) this.Document);
        break;
      case FieldType.FieldMergeField:
        return (IWField) this.AppendMergeField(fieldName);
      case FieldType.FieldFormTextInput:
        return (IWField) this.AppendTextFormField(fieldName, fieldName);
      case FieldType.FieldFormCheckBox:
        return (IWField) this.AppendCheckBox(fieldName, false);
      case FieldType.FieldFormDropDown:
        return (IWField) this.AppendDropDownFormField(fieldName);
      default:
        wfield = new WField((IWordDocument) this.Document);
        break;
    }
    wfield.SetFieldTypeValue(fieldType);
    if (wfield.FieldType == FieldType.FieldFormula)
    {
      fieldName = fieldName.Replace(" ", string.Empty);
      fieldName = fieldName.Replace("\"", string.Empty);
      fieldName = fieldName.Replace("=", string.Empty);
    }
    WTextRange wtextRange1 = new WTextRange((IWordDocument) this.m_doc);
    string str1 = FieldTypeDefiner.GetFieldCode(fieldType) + " ";
    string str2 = fieldName.TrimStart();
    if (str2.StartsWith(str1))
      str2 = str2.Remove(0, str1.Length);
    wfield.m_fieldValue = str2.IndexOf(' ') == -1 || wfield.FieldType == FieldType.FieldIndex ? str2 : $"\"{str2}\"";
    wtextRange1.Text = str1 + wfield.m_fieldValue;
    if (!str2.Contains(" \\* MERGEFORMAT"))
    {
      switch (wfield.FieldType)
      {
        case FieldType.FieldTitle:
        case FieldType.FieldSubject:
        case FieldType.FieldDocVariable:
          wtextRange1.Text += " \\* MERGEFORMAT";
          break;
      }
    }
    if (wfield.AddFormattingString())
      wfield.m_formattingString = "\\* MERGEFORMAT";
    this.m_pItemColl.Add((IEntity) wfield);
    wtextRange1.ApplyCharacterFormat(wfield.CharacterFormat);
    this.m_pItemColl.Add((IEntity) wtextRange1);
    if (!wfield.IsFieldWithoutSeparator)
    {
      wfield.FieldSeparator = this.AppendFieldMark(FieldMarkType.FieldSeparator);
      if (fieldType != FieldType.FieldSequence)
      {
        WTextRange wtextRange2 = new WTextRange((IWordDocument) this.Document);
        wtextRange2.Text = fieldName;
        this.m_pItemColl.Add((IEntity) wtextRange2);
        if (fieldType == FieldType.FieldHyperlink)
        {
          wtextRange2.CharacterFormat.TextColor = Color.Blue;
          wtextRange2.CharacterFormat.UnderlineStyle = UnderlineStyle.Single;
        }
      }
    }
    WFieldMark wfieldMark = new WFieldMark((IWordDocument) this.Document, FieldMarkType.FieldEnd);
    this.m_pItemColl.Add((IEntity) wfieldMark);
    wfield.FieldEnd = wfieldMark;
    return (IWField) wfield;
  }

  public IWField AppendHyperlink(string link, string text, HyperlinkType type)
  {
    return this.AppendHyperlink(link, text, (WPicture) null, type);
  }

  public IWField AppendHyperlink(string link, WPicture picture, HyperlinkType type)
  {
    return this.AppendHyperlink(link, (string) null, picture, type);
  }

  public BookmarkStart AppendBookmarkStart(string name)
  {
    BookmarkStart bookmarkStart = new BookmarkStart((IWordDocument) this.Document, name);
    this.Items.Add((IEntity) bookmarkStart);
    return bookmarkStart;
  }

  public BookmarkEnd AppendBookmarkEnd(string name)
  {
    BookmarkEnd bookmarkEnd = new BookmarkEnd((IWordDocument) this.Document, name);
    this.Items.Add((IEntity) bookmarkEnd);
    return bookmarkEnd;
  }

  internal EditableRangeEnd AppendEditableRangeEnd(string id)
  {
    EditableRangeEnd editableRangeEnd = new EditableRangeEnd((IWordDocument) this.Document, id);
    this.Items.Add((IEntity) editableRangeEnd);
    return editableRangeEnd;
  }

  public WComment AppendComment(string text)
  {
    WComment wcomment = (WComment) this.AppendItem(ParagraphItemType.Comment);
    wcomment.TextBody.AddParagraph().AppendText(text);
    return wcomment;
  }

  public WFootnote AppendFootnote(FootnoteType type)
  {
    WFootnote wfootnote = (WFootnote) this.AppendItem(ParagraphItemType.Footnote);
    wfootnote.FootnoteType = type;
    wfootnote.EnsureFtnStyle();
    return wfootnote;
  }

  public IWTextBox AppendTextBox(float width, float height)
  {
    IWTextBox wtextBox = this.AppendItem(ParagraphItemType.TextBox) as IWTextBox;
    wtextBox.TextBoxFormat.Width = width;
    wtextBox.TextBoxFormat.Height = height;
    return wtextBox;
  }

  public WCheckBox AppendCheckBox()
  {
    return this.AppendCheckBox(("Check_" + Guid.NewGuid().ToString().Replace("-", "_")).Substring(0, 20), false);
  }

  public WCheckBox AppendCheckBox(string checkBoxName, bool defaultCheckBoxValue)
  {
    WCheckBox paragraphItem = this.Document.CreateParagraphItem(ParagraphItemType.CheckBox) as WCheckBox;
    paragraphItem.Name = checkBoxName;
    paragraphItem.DefaultCheckBoxValue = defaultCheckBoxValue;
    this.Items.Add((IEntity) paragraphItem);
    return paragraphItem;
  }

  public WTextFormField AppendTextFormField(string defaultText)
  {
    return this.AppendTextFormField(("Text_" + Guid.NewGuid().ToString().Replace("-", "_")).Substring(0, 20), defaultText);
  }

  public WTextFormField AppendTextFormField(string formFieldName, string defaultText)
  {
    WTextFormField paragraphItem = this.Document.CreateParagraphItem(ParagraphItemType.TextFormField) as WTextFormField;
    paragraphItem.Name = formFieldName;
    this.Items.Add((IEntity) paragraphItem);
    if (string.IsNullOrEmpty(defaultText))
    {
      paragraphItem.DefaultText = "     ";
      paragraphItem.Text = "     ";
    }
    else
    {
      paragraphItem.DefaultText = defaultText;
      paragraphItem.Text = defaultText;
    }
    return paragraphItem;
  }

  public WDropDownFormField AppendDropDownFormField()
  {
    return this.AppendDropDownFormField(("Drop_" + Guid.NewGuid().ToString().Replace("-", "_")).Substring(0, 20));
  }

  public WDropDownFormField AppendDropDownFormField(string dropDropDownName)
  {
    WDropDownFormField paragraphItem = this.Document.CreateParagraphItem(ParagraphItemType.DropDownFormField) as WDropDownFormField;
    paragraphItem.Name = dropDropDownName;
    this.Items.Add((IEntity) paragraphItem);
    return paragraphItem;
  }

  public WSymbol AppendSymbol(byte characterCode)
  {
    WSymbol wsymbol = (WSymbol) this.AppendItem(ParagraphItemType.Symbol);
    wsymbol.CharacterCode = characterCode;
    return wsymbol;
  }

  public Break AppendBreak(BreakType breakType)
  {
    Break @break = new Break((IWordDocument) this.Document, breakType);
    this.Items.Add((IEntity) @break);
    return @break;
  }

  public Shape AppendShape(AutoShapeType autoShapeType, float width, float height)
  {
    Shape shape = new Shape((IWordDocument) this.Document, autoShapeType);
    shape.Width = width;
    shape.Height = height;
    this.Items.Add((IEntity) shape);
    return shape;
  }

  public TableOfContent AppendTOC(int lowerHeadingLevel, int upperHeadingLevel)
  {
    TableOfContent tableOfContent = this.AppendItem(ParagraphItemType.TOC) as TableOfContent;
    this.AppendText("TOC ");
    tableOfContent.LowerHeadingLevel = lowerHeadingLevel;
    tableOfContent.UpperHeadingLevel = upperHeadingLevel;
    tableOfContent.TOCField.FieldSeparator = this.AppendFieldMark(FieldMarkType.FieldSeparator);
    this.AppendText("TOC");
    tableOfContent.TOCField.FieldEnd = this.AppendFieldMark(FieldMarkType.FieldEnd);
    if (!this.Document.TOC.ContainsKey(tableOfContent.TOCField))
      this.Document.TOC.Add(tableOfContent.TOCField, tableOfContent);
    tableOfContent.UpdateTOCFieldCode();
    return tableOfContent;
  }

  public void AppendCrossReference(
    ReferenceType referenceType,
    ReferenceKind referenceKind,
    Entity referenceEntity,
    bool insertAsHyperlink,
    bool includePosition,
    bool separatorNumber,
    string separatorString)
  {
    string empty1 = string.Empty;
    string empty2 = string.Empty;
    FieldType fieldType;
    string str;
    string fieldName;
    if (referenceKind == ReferenceKind.PageNumber)
    {
      fieldType = FieldType.FieldPageRef;
      str = "PAGEREF ";
      fieldName = "PageRef field";
    }
    else
    {
      fieldType = FieldType.FieldRef;
      str = "Ref ";
      fieldName = "Ref field";
    }
    if (referenceEntity is BookmarkStart)
      str = $"{str}{(referenceEntity as BookmarkStart).Name} ";
    switch (referenceKind)
    {
      case ReferenceKind.NumberFullContext:
        str += "\\w ";
        break;
      case ReferenceKind.NumberNoContext:
        str += "\\n ";
        break;
      case ReferenceKind.NumberRelativeContext:
        str += "\\r ";
        break;
      case ReferenceKind.AboveBelow:
        str += "\\p ";
        break;
    }
    if (includePosition)
      str += "\\p ";
    if (insertAsHyperlink)
      str += "\\h ";
    if (separatorNumber)
      str += "\\d ";
    if (separatorNumber && separatorString != null)
      str += separatorString;
    ((this.AppendField(fieldName, fieldType) as WField).NextSibling as WTextRange).Text = str;
  }

  public IWPicture AppendPicture(Image image)
  {
    WPicture wpicture = this.AppendItem(ParagraphItemType.Picture) as WPicture;
    wpicture.LoadImage(image);
    this.Document.HasPicture = true;
    return (IWPicture) wpicture;
  }

  public void AppendHTML(string html)
  {
    this.Document.IsOpening = true;
    string lower = html.ToLower();
    if (!this.StartsWithExt(lower, "<html") && !this.StartsWithExt(lower, "<body") && !this.StartsWithExt(lower, "<!doctype") && !this.StartsWithExt(lower, "<?xml"))
      html = $"<html><head><title/></head><body>{html}</body></html>";
    IHtmlConverter instance = HtmlConverterFactory.GetInstance();
    (instance as HTMLConverterImpl).HtmlImportSettings = this.Document.HTMLImportSettings;
    if (this.IsStyleApplied)
      instance.AppendToTextBody((ITextBody) this.OwnerTextBody, html, this.GetIndexInOwnerCollection(), this.Items.Count, this.ParaStyle, this.ListFormat.CurrentListStyle);
    else
      instance.AppendToTextBody((ITextBody) this.OwnerTextBody, html, this.GetIndexInOwnerCollection(), this.Items.Count, (IWParagraphStyle) null, this.ListFormat.CurrentListStyle);
    this.Document.IsOpening = false;
  }

  public WOleObject AppendOleObject(string pathToFile, WPicture olePicture, OleObjectType type)
  {
    byte[] nativeData = File.Exists(pathToFile) ? File.ReadAllBytes(pathToFile) : throw new ArgumentException("File does not exists, please use valid file path.");
    WOleObject woleObject = new WOleObject(this.m_doc);
    this.Items.Add((IEntity) woleObject);
    woleObject.SetOlePicture(olePicture);
    woleObject.SetLinkType(OleLinkType.Embed);
    woleObject.ObjectType = OleTypeConvertor.ToString(type, false);
    woleObject.OleObjectType = type;
    woleObject.CreateOleObjContainer(nativeData, Path.GetFileName(pathToFile));
    woleObject.Field.FieldType = FieldType.FieldEmbed;
    woleObject.AddFieldCodeText();
    WFieldMark wfieldMark = new WFieldMark((IWordDocument) this.m_doc);
    wfieldMark.Type = FieldMarkType.FieldSeparator;
    this.Items.Add((IEntity) wfieldMark);
    this.Items.Add((IEntity) woleObject.OlePicture);
    woleObject.Field.FieldSeparator = wfieldMark;
    woleObject.Field.FieldEnd = this.AppendFieldMark(FieldMarkType.FieldEnd);
    return woleObject;
  }

  public WOleObject AppendOleObject(string pathToFile, WPicture olePicture)
  {
    return this.AppendOleObject(pathToFile, olePicture, OleObjectType.Package);
  }

  public WOleObject AppendOleObject(Stream oleStream, WPicture olePicture, OleObjectType type)
  {
    if (oleStream == null || oleStream.Length == 0L)
      return (WOleObject) null;
    if (type == OleObjectType.Package)
      throw new ArgumentException("Please use AppendOleObject(Stream oleStream, WPicture olePicture, string fileExtension) method.  Package type is invalid in this context.");
    oleStream.Position = 0L;
    WOleObject woleObject = this.AppendItem(ParagraphItemType.OleObject) as WOleObject;
    woleObject.SetOlePicture(olePicture);
    woleObject.SetLinkType(OleLinkType.Embed);
    woleObject.ObjectType = OleTypeConvertor.ToString(type, false);
    woleObject.OleObjectType = type;
    woleObject.ParseOleStream(oleStream);
    woleObject.Field.FieldType = FieldType.FieldEmbed;
    woleObject.AddFieldCodeText();
    WFieldMark wfieldMark = new WFieldMark((IWordDocument) this.m_doc);
    wfieldMark.Type = FieldMarkType.FieldSeparator;
    this.Items.Add((IEntity) wfieldMark);
    this.Items.Add((IEntity) woleObject.OlePicture);
    woleObject.Field.FieldSeparator = wfieldMark;
    woleObject.Field.FieldEnd = this.AppendFieldMark(FieldMarkType.FieldEnd);
    return woleObject;
  }

  public WOleObject AppendOleObject(byte[] oleBytes, WPicture olePicture, OleObjectType type)
  {
    if (oleBytes == null || oleBytes.Length == 0)
      return (WOleObject) null;
    if (type == OleObjectType.Package)
      throw new ArgumentException("Please use AppendOleObject(byte[] oleBytes, WPicture olePicture, string fileExtension) method.  Package type is invalid in this context.");
    return this.AppendOleObject((Stream) new MemoryStream(oleBytes), olePicture, type);
  }

  public WOleObject AppendOleObject(Stream oleStream, WPicture olePicture, OleLinkType oleLinkType)
  {
    WOleObject woleObject = this.AppendItem(ParagraphItemType.OleObject) as WOleObject;
    woleObject.SetOlePicture(olePicture);
    woleObject.SetLinkType(oleLinkType);
    oleStream.Position = 0L;
    woleObject.ParseOleStream(oleStream);
    woleObject.Field.FieldType = oleLinkType != OleLinkType.Embed ? FieldType.FieldLink : FieldType.FieldEmbed;
    woleObject.AddFieldCodeText();
    WFieldMark wfieldMark = new WFieldMark((IWordDocument) this.m_doc);
    wfieldMark.Type = FieldMarkType.FieldSeparator;
    this.Items.Add((IEntity) wfieldMark);
    this.Items.Add((IEntity) woleObject.OlePicture);
    woleObject.Field.FieldSeparator = wfieldMark;
    woleObject.Field.FieldEnd = this.AppendFieldMark(FieldMarkType.FieldEnd);
    return woleObject;
  }

  public WOleObject AppendOleObject(byte[] oleBytes, WPicture olePicture, OleLinkType oleLinkType)
  {
    return this.AppendOleObject((Stream) new MemoryStream(oleBytes), olePicture, oleLinkType);
  }

  public WOleObject AppendOleObject(byte[] oleBytes, WPicture olePicture, string fileExtension)
  {
    WOleObject woleObject = new WOleObject(this.m_doc);
    this.Items.Add((IEntity) woleObject);
    woleObject.SetOlePicture(olePicture);
    woleObject.SetLinkType(OleLinkType.Embed);
    woleObject.ObjectType = OleTypeConvertor.ToString(OleObjectType.Package, false);
    woleObject.OleObjectType = OleObjectType.Package;
    string dataPath = "Package." + fileExtension.Replace(".", string.Empty);
    woleObject.CreateOleObjContainer(oleBytes, dataPath);
    woleObject.Field.FieldType = FieldType.FieldEmbed;
    woleObject.AddFieldCodeText();
    WFieldMark wfieldMark = new WFieldMark((IWordDocument) this.m_doc);
    wfieldMark.Type = FieldMarkType.FieldSeparator;
    this.Items.Add((IEntity) wfieldMark);
    this.Items.Add((IEntity) woleObject.OlePicture);
    woleObject.Field.FieldSeparator = wfieldMark;
    woleObject.Field.FieldEnd = this.AppendFieldMark(FieldMarkType.FieldEnd);
    return woleObject;
  }

  public WOleObject AppendOleObject(Stream oleStream, WPicture olePicture, string fileExtension)
  {
    oleStream.Position = 0L;
    byte[] numArray = new byte[oleStream.Length];
    oleStream.Read(numArray, 0, numArray.Length);
    return this.AppendOleObject(numArray, olePicture, fileExtension);
  }

  public WMath AppendMath() => this.AppendItem(ParagraphItemType.Math) as WMath;

  internal Entity GetOwnerEntity()
  {
    Entity owner = this.Owner;
    while (true)
    {
      switch (owner)
      {
        case WTextBox _:
        case Shape _:
        case WSection _:
        case WTableCell _:
        case ChildShape _:
          goto label_4;
        case null:
          goto label_1;
        default:
          owner = owner.Owner;
          continue;
      }
    }
label_1:
    return owner;
label_4:
    return owner;
  }

  internal bool OmitHeadingStyles()
  {
    Entity owner = this.Owner;
    while (true)
    {
      switch (owner)
      {
        case HeaderFooter _:
        case WFootnote _:
          goto label_4;
        case null:
          goto label_1;
        default:
          owner = owner.Owner;
          continue;
      }
    }
label_1:
    return false;
label_4:
    return true;
  }

  internal bool IsInHeaderFooter()
  {
    Entity owner = this.Owner;
    while (true)
    {
      switch (owner)
      {
        case WSection _:
          goto label_5;
        case null:
          goto label_1;
        case HeaderFooter _:
          goto label_2;
        default:
          owner = owner.Owner;
          continue;
      }
    }
label_1:
    return false;
label_2:
    return true;
label_5:
    return false;
  }

  internal WParagraph GetFirstParagraphInOwnerTextbody(WTextBody textbody)
  {
    WParagraph paragraphInOwnerTextbody = (WParagraph) null;
    WTable ownerTable = !(textbody is WTableCell wtableCell) || wtableCell.OwnerRow == null || wtableCell.OwnerRow.OwnerTable == null ? (WTable) null : wtableCell.OwnerRow.OwnerTable;
    if (ownerTable != null && ownerTable.Rows.Count > 0 && ownerTable.Rows[0].Cells.Count > 0)
    {
      WTableCell cell = ownerTable.Rows[0].Cells[0];
      if (cell.Items.Count > 0 && cell.Items[0] is WParagraph)
        return cell.Paragraphs[0].ParagraphFormat.IsFrame ? cell.Paragraphs[0] : (WParagraph) null;
      if (cell.Items.Count > 0 && cell.Items[0] is WTable && cell.Tables[0].Rows.Count > 0 && cell.Tables[0].Rows[0].Cells.Count > 0)
        paragraphInOwnerTextbody = this.GetFirstParagraphInOwnerTextbody((WTextBody) cell.Tables[0].Rows[0].Cells[0]);
    }
    return paragraphInOwnerTextbody;
  }

  internal WFieldMark AppendFieldMark(FieldMarkType type)
  {
    WFieldMark wfieldMark = (WFieldMark) this.AppendItem(ParagraphItemType.FieldMark);
    wfieldMark.Type = type;
    return wfieldMark;
  }

  internal Break AppendLineBreak(string lineBreakText)
  {
    Break @break = new Break((IWordDocument) this.Document, BreakType.LineBreak);
    @break.TextRange.Text = lineBreakText;
    this.Items.Add((IEntity) @break);
    return @break;
  }

  internal IWField AppendHyperlink(string link, string text, WPicture pict, HyperlinkType type)
  {
    WField hyperlink1 = new WField((IWordDocument) this.Document);
    hyperlink1.FieldType = FieldType.FieldHyperlink;
    this.Items.Add((IEntity) hyperlink1);
    IWTextRange wtextRange1 = this.AppendText(string.Empty);
    hyperlink1.FieldSeparator = this.AppendFieldMark(FieldMarkType.FieldSeparator);
    if (text != null)
    {
      IWTextRange wtextRange2 = this.AppendText(text);
      if (this.m_doc.Styles.FindByName(BuiltinStyle.Hyperlink.ToString()) == null)
        this.m_doc.Styles.Add(Style.CreateBuiltinStyle(BuiltinStyle.Hyperlink, StyleType.CharacterStyle, this.m_doc));
      wtextRange2.CharacterFormat.CharStyleName = BuiltinStyle.Hyperlink.ToString();
    }
    else if (pict != null)
      this.Items.Add((IEntity) pict);
    else
      this.AppendText("Hyperlink");
    WFieldMark wfieldMark = new WFieldMark((IWordDocument) this.Document, FieldMarkType.FieldEnd);
    this.Items.Add((IEntity) wfieldMark);
    hyperlink1.FieldEnd = wfieldMark;
    Hyperlink hyperlink2 = new Hyperlink(hyperlink1);
    hyperlink2.Type = type;
    if (type == HyperlinkType.WebLink || type == HyperlinkType.EMailLink)
      hyperlink2.Uri = link;
    else if (hyperlink2.Type == HyperlinkType.Bookmark)
      hyperlink2.BookmarkName = link;
    else if (hyperlink2.Type == HyperlinkType.FileLink)
      hyperlink2.FilePath = link;
    else
      wtextRange1.Text = $"HYPERLINK {hyperlink1.FieldValue} {hyperlink1.LocalReference}";
    return (IWField) hyperlink1;
  }

  internal void LoadPicture(WPicture picture, ImageRecord imageRecord)
  {
    picture.LoadImage(imageRecord);
    this.Document.HasPicture = true;
  }

  internal IWField AppendIndexEntry(string entryToMark)
  {
    WField wfield = new WField((IWordDocument) this.Document);
    wfield.SetFieldTypeValue(FieldType.FieldIndexEntry);
    wfield.CharacterFormat.FieldVanish = true;
    this.Items.Add((IEntity) wfield);
    this.AppendText($"XE \"{entryToMark}\"").CharacterFormat.FieldVanish = true;
    WFieldMark wfieldMark = this.AppendFieldMark(FieldMarkType.FieldEnd);
    wfieldMark.CharacterFormat.FieldVanish = true;
    wfield.FieldEnd = wfieldMark;
    return (IWField) wfield;
  }

  internal WMergeField AppendMergeField(string fieldName)
  {
    WMergeField wmergeField = new WMergeField((IWordDocument) this.Document);
    if (!this.StartsWithExt(fieldName.ToUpper().TrimStart(), "MERGEFIELD "))
      fieldName = "MERGEFIELD " + fieldName;
    this.Items.Add((IEntity) wmergeField);
    wmergeField.FieldCode = fieldName;
    return wmergeField;
  }

  internal WListFormat GetListFormatValue()
  {
    WListFormat listFormatValue = (WListFormat) null;
    WParagraphStyle wparagraphStyle = this.ParaStyle as WParagraphStyle;
    if (this.ListFormat.ListType != ListType.NoList)
      listFormatValue = this.ListFormat;
    else if (!this.ListFormat.IsEmptyList)
    {
      for (; wparagraphStyle != null; wparagraphStyle = wparagraphStyle.BaseStyle)
      {
        if (wparagraphStyle.ListFormat.ListType != ListType.NoList || wparagraphStyle.ListFormat.IsEmptyList)
        {
          listFormatValue = wparagraphStyle.ListFormat;
          break;
        }
      }
    }
    return listFormatValue;
  }

  internal WListFormat GetListFormat(
    ref WListLevel listLevel,
    ref int tabLevelIndex,
    ref float? leftIndent,
    ref float? firstLineIndent)
  {
    WListFormat listFormat = (WListFormat) null;
    if (this.ListFormat.ListType != ListType.NoList || this.ListFormat.IsEmptyList)
      listFormat = this.ListFormat;
    int? nullable = new int?();
    if (this.ListFormat.HasKey(0))
      nullable = new int?(this.ListFormat.ListLevelNumber);
    if (this.ParagraphFormat.HasValue(2))
      leftIndent = new float?(this.ParagraphFormat.LeftIndent);
    if (this.ParagraphFormat.HasValue(5))
      firstLineIndent = new float?(this.ParagraphFormat.FirstLineIndent);
    for (WParagraphStyle wparagraphStyle = this.ParaStyle as WParagraphStyle; (listFormat == null || !nullable.HasValue) && wparagraphStyle != null; wparagraphStyle = wparagraphStyle.BaseStyle)
    {
      if (listFormat == null && !nullable.HasValue && wparagraphStyle.ParagraphFormat != null)
      {
        if (!leftIndent.HasValue && wparagraphStyle.ParagraphFormat.HasValue(2))
          leftIndent = new float?(wparagraphStyle.ParagraphFormat.LeftIndent);
        if (!firstLineIndent.HasValue && wparagraphStyle.ParagraphFormat.HasValue(5))
          firstLineIndent = new float?(wparagraphStyle.ParagraphFormat.FirstLineIndent);
      }
      if (listFormat == null && (wparagraphStyle.ListFormat.ListType != ListType.NoList || wparagraphStyle.ListFormat.IsEmptyList))
        listFormat = wparagraphStyle.ListFormat;
      if (!nullable.HasValue && wparagraphStyle.ListFormat.HasKey(0))
      {
        nullable = new int?(wparagraphStyle.ListFormat.ListLevelNumber);
        if (wparagraphStyle.ParagraphFormat.Tabs.Count > 0)
          ++tabLevelIndex;
      }
    }
    if (listFormat == null || listFormat.CurrentListStyle == null)
      return listFormat;
    if (!nullable.HasValue)
      nullable = new int?(0);
    ListStyle currentListStyle = listFormat.CurrentListStyle;
    listLevel = currentListStyle.GetNearLevel(nullable.Value);
    ListOverrideStyle listOverrideStyle = (ListOverrideStyle) null;
    if (listFormat.LFOStyleName != null && listFormat.LFOStyleName.Length > 0)
      listOverrideStyle = this.Document.ListOverrides.FindByName(listFormat.LFOStyleName);
    if (listOverrideStyle != null && listOverrideStyle.OverrideLevels.HasOverrideLevel(nullable.Value) && listOverrideStyle.OverrideLevels[nullable.Value].OverrideFormatting)
      listLevel = listOverrideStyle.OverrideLevels[nullable.Value].OverrideListLevel;
    nullable = new int?();
    if (!leftIndent.HasValue && listLevel.ParagraphFormat.HasValue(2))
      leftIndent = new float?(listLevel.ParagraphFormat.LeftIndent);
    if (!firstLineIndent.HasValue && listLevel.ParagraphFormat.HasValue(5))
      firstLineIndent = new float?(listLevel.ParagraphFormat.FirstLineIndent);
    return listFormat;
  }

  internal WListLevel GetListLevel(WListFormat listFormat, ref int tabLevelIndex)
  {
    ListStyle currentListStyle = listFormat.CurrentListStyle;
    WParagraphStyle wparagraphStyle = this.ParaStyle as WParagraphStyle;
    int levelNumber = 0;
    if (this.ListFormat.HasKey(0))
    {
      levelNumber = this.ListFormat.ListLevelNumber;
    }
    else
    {
      for (; wparagraphStyle != null; wparagraphStyle = wparagraphStyle.BaseStyle)
      {
        if (wparagraphStyle.ListFormat.HasKey(0))
        {
          levelNumber = wparagraphStyle.ListFormat.ListLevelNumber;
          if (wparagraphStyle.ParagraphFormat.Tabs.Count > 0)
          {
            ++tabLevelIndex;
            break;
          }
          break;
        }
      }
    }
    WListLevel listLevel = currentListStyle.GetNearLevel(levelNumber);
    ListOverrideStyle listOverrideStyle = (ListOverrideStyle) null;
    if (listFormat.LFOStyleName != null && listFormat.LFOStyleName.Length > 0)
      listOverrideStyle = this.Document.ListOverrides.FindByName(listFormat.LFOStyleName);
    if (listOverrideStyle != null && listOverrideStyle.OverrideLevels.HasOverrideLevel(levelNumber) && listOverrideStyle.OverrideLevels[levelNumber].OverrideFormatting)
      listLevel = listOverrideStyle.OverrideLevels[levelNumber].OverrideListLevel;
    return listLevel;
  }

  internal string GetListText(bool isFromTextConverter, ref bool isPicBullet)
  {
    string listText = string.Empty;
    WListFormat listFormatValue = this.GetListFormatValue();
    if (listFormatValue != null && listFormatValue.CurrentListStyle != null && listFormatValue.ListType != ListType.NoList)
    {
      WListLevel listLevel = this.GetListLevel(listFormatValue);
      if (listLevel.PatternType == ListPatternType.Bullet)
      {
        listText = "* ";
        if (isFromTextConverter && listLevel.PicBullet != null)
          isPicBullet = true;
      }
      else
        listText = this.Document.UpdateListValue(this, listFormatValue, listLevel) + "  ";
    }
    return listText;
  }

  internal WListLevel GetListLevel(WListFormat listFormat)
  {
    int tabLevelIndex = 0;
    return this.GetListLevel(listFormat, ref tabLevelIndex);
  }

  internal float[] GetLeftRightMargindAndFirstLineIndent(
    WListFormat listformat,
    WListLevel level,
    WParagraphStyle paraStyle)
  {
    float[] andFirstLineIndent = new float[3];
    float num1 = 0.0f;
    float num2 = 0.0f;
    float num3 = 0.0f;
    if (level != null && level.ParagraphFormat.HasValue(2))
      num1 = level.ParagraphFormat.LeftIndent;
    if (this.ListFormat.ListType == ListType.NoList && paraStyle != null && paraStyle.ParagraphFormat.HasValue(2))
      num1 = paraStyle.ParagraphFormat.LeftIndent;
    if (this.ParagraphFormat.HasValue(2))
      num1 = this.ParagraphFormat.LeftIndent;
    if (level != null && level.ParagraphFormat.HasValue(3))
      num2 = level.ParagraphFormat.RightIndent;
    if (this.ListFormat.ListType == ListType.NoList && paraStyle != null && paraStyle.ParagraphFormat.HasValue(3))
      num2 = paraStyle.ParagraphFormat.RightIndent;
    if (this.ParagraphFormat.HasValue(3))
      num2 = this.ParagraphFormat.RightIndent;
    if (level != null && level.ParagraphFormat.HasValue(5))
      num3 = level.ParagraphFormat.FirstLineIndent;
    if (this.ListFormat.ListType == ListType.NoList && paraStyle != null && paraStyle.ParagraphFormat.HasValue(5))
      num3 = paraStyle.ParagraphFormat.FirstLineIndent;
    if (this.ParagraphFormat.HasValue(5))
      num3 = this.ParagraphFormat.FirstLineIndent;
    andFirstLineIndent[0] = num1;
    andFirstLineIndent[1] = num2;
    andFirstLineIndent[2] = num3;
    return andFirstLineIndent;
  }

  internal bool IsZeroAutoLineSpace()
  {
    return (double) this.ParagraphFormat.LineSpacing == 0.0 && this.ParagraphFormat.LineSpacingRule == LineSpacingRule.Multiple;
  }

  internal bool IsLineNumbersEnabled()
  {
    WSection ownerSection = this.GetOwnerSection();
    return ownerSection != null && ownerSection.LineNumbersEnabled() && !this.ParagraphFormat.SuppressLineNumbers && (!this.SectionEndMark || this.ChildEntities.Count != 0);
  }

  internal bool IsContainsInLineImage()
  {
    foreach (ParagraphItem paragraphItem in (CollectionImpl) this.Items)
    {
      if (paragraphItem is WPicture && (paragraphItem as WPicture).TextWrappingStyle == TextWrappingStyle.Inline)
        return true;
    }
    return false;
  }

  public override TextSelection Find(Regex pattern)
  {
    List<TextSelection> textSelectionList = !FindUtils.IsPatternEmpty(pattern) ? (List<TextSelection>) TextFinder.Instance.Find(this, pattern, true) : throw new ArgumentException("Search string cannot be empty");
    return textSelectionList.Count <= 0 ? (TextSelection) null : textSelectionList[0];
  }

  public TextSelection Find(string given, bool caseSensitive, bool wholeWord)
  {
    return this.Find(FindUtils.StringToRegex(given, caseSensitive, wholeWord));
  }

  public override int Replace(Regex pattern, string replace)
  {
    if (FindUtils.IsPatternEmpty(pattern))
      throw new ArgumentException("Search string cannot be empty");
    return TextReplacer.Instance.Replace(this, pattern, replace);
  }

  public override int Replace(string given, string replace, bool caseSensitive, bool wholeWord)
  {
    return this.Replace(FindUtils.StringToRegex(given, caseSensitive, wholeWord), replace);
  }

  public override int Replace(Regex pattern, TextSelection textSelection)
  {
    return this.Replace(pattern, textSelection, false);
  }

  public override int Replace(Regex pattern, TextSelection textSelection, bool saveFormatting)
  {
    if (FindUtils.IsPatternEmpty(pattern))
      throw new ArgumentException("Search string cannot be empty");
    textSelection.CacheRanges();
    TextSelectionList all = this.FindAll(pattern);
    if (all == null)
      return 0;
    foreach (TextSelection textSelection1 in (List<TextSelection>) all)
    {
      WCharacterFormat srcFormat = (WCharacterFormat) null;
      if (textSelection1.StartTextRange != null && saveFormatting)
        srcFormat = textSelection1.StartTextRange.CharacterFormat;
      Entity entity = textSelection1.OwnerParagraph != null || textSelection1.Count <= 0 ? (Entity) textSelection1.OwnerParagraph : textSelection1.StartTextRange.Owner;
      int startIndex = textSelection1.SplitAndErase();
      if (entity is WParagraph)
      {
        WParagraph ownerParagraph = textSelection1.OwnerParagraph;
        textSelection.CopyTo(ownerParagraph, startIndex, saveFormatting, srcFormat);
      }
      else if (entity is InlineContentControl)
        textSelection.CopyTo(entity as InlineContentControl, startIndex, saveFormatting, srcFormat);
    }
    return all.Count;
  }

  public int Replace(
    string given,
    TextSelection textSelection,
    bool caseSensitive,
    bool wholeWord)
  {
    return this.Replace(FindUtils.StringToRegex(given, caseSensitive, wholeWord), textSelection, false);
  }

  public int Replace(
    string given,
    TextSelection textSelection,
    bool caseSensitive,
    bool wholeWord,
    bool saveFormatting)
  {
    return this.Replace(FindUtils.StringToRegex(given, caseSensitive, wholeWord), textSelection, saveFormatting);
  }

  internal int ReplaceFirst(string given, string replace, bool caseSensitive, bool wholeWord)
  {
    return this.ReplaceFirst(FindUtils.StringToRegex(given, caseSensitive, wholeWord), replace);
  }

  internal int ReplaceFirst(Regex pattern, string replace)
  {
    if (FindUtils.IsPatternEmpty(pattern))
      throw new ArgumentException("Search string cannot be empty");
    TextReplacer instance = TextReplacer.Instance;
    bool replaceFirst = this.Document.ReplaceFirst;
    this.Document.ReplaceFirst = true;
    int num = instance.Replace(this, pattern, replace);
    this.Document.ReplaceFirst = replaceFirst;
    return num;
  }

  public WSection InsertSectionBreak() => this.InsertSectionBreak(SectionBreakCode.NewPage);

  public WSection InsertSectionBreak(SectionBreakCode breakType)
  {
    WSection ownerSection = this.GetOwnerSection();
    if (ownerSection == null)
      throw new Exception("Owner section cannot be null.");
    if (this.m_ownerTextBodyItem.Owner is HeaderFooter)
      throw new NotSupportedException("Cannot insert section break for header footer items.");
    int inOwnerCollection1 = ownerSection.GetIndexInOwnerCollection();
    WSection wsection = ownerSection.CloneWithoutBodyItems();
    this.Document.Sections.Insert(inOwnerCollection1 + 1, (IEntity) wsection);
    wsection.BreakCode = breakType;
    int inOwnerCollection2 = this.m_ownerTextBodyItem.GetIndexInOwnerCollection();
    for (int index = ownerSection.Body.Items.Count - 1; index >= inOwnerCollection2 + 1; --index)
      wsection.Body.Items.Insert(0, (IEntity) ownerSection.Body.ChildEntities[index]);
    return wsection;
  }

  internal WSection GetOwnerSection()
  {
    Entity ownerSection;
    for (ownerSection = (Entity) this; !(ownerSection is WSection); ownerSection = ownerSection.Owner)
    {
      if (ownerSection is TextBodyItem)
        this.m_ownerTextBodyItem = ownerSection as TextBodyItem;
      if (ownerSection.Owner == null)
        break;
    }
    return ownerSection as WSection;
  }

  internal ParagraphItemCollection GetParagraphItems()
  {
    ParagraphItemCollection paraItems = new ParagraphItemCollection(this);
    for (int index = 0; index < this.m_pItemColl.Count; ++index)
    {
      if (this.m_pItemColl[index] is InlineContentControl)
        (this.m_pItemColl[index] as InlineContentControl).CopyItemsTo(paraItems);
      else
        paraItems.InnerList.Add((object) this.m_pItemColl[index]);
    }
    return paraItems;
  }

  internal void ClearItems()
  {
    this.Items.InnerList.Clear();
    this.m_strTextBuilder = new StringBuilder(1);
  }

  internal override TextSelectionList FindAll(Regex pattern)
  {
    return TextFinder.Instance.Find(this, pattern, false);
  }

  internal TextSelectionList FindFirst(Regex pattern)
  {
    return TextFinder.Instance.Find(this, pattern, true);
  }

  internal void RemoveItems(int startIndex, bool toEnd)
  {
    if (toEnd)
    {
      while (startIndex < this.Items.Count)
        this.Items.RemoveAt(this.Items.Count - 1);
    }
    else
    {
      for (; startIndex > -1; --startIndex)
        this.Items.RemoveAt(startIndex);
    }
  }

  internal WParagraph CloneWithoutItems() => this.CloneParagraph(false);

  internal IParagraphItem AppendItem(ParagraphItemType itemType)
  {
    IParagraphItem paragraphItem = (IParagraphItem) this.Document.CreateParagraphItem(itemType);
    this.Items.Add((IEntity) paragraphItem);
    return paragraphItem;
  }

  internal void UpdateText(WTextRange pItem, string newText, bool isRemove)
  {
    this.UpdateText((ParagraphItem) pItem, pItem.TextLength, newText, isRemove);
  }

  internal void UpdateText(
    ParagraphItem pItem,
    int removeTextLength,
    string newText,
    bool isRemove)
  {
    if (isRemove)
      this.m_strTextBuilder.Remove(pItem.StartPos, removeTextLength);
    this.m_strTextBuilder.Insert(pItem.StartPos, newText);
    int offset = isRemove ? newText.Length - removeTextLength : newText.Length;
    if (pItem.Owner is InlineContentControl)
    {
      this.Document.UpdateStartPosOfInlineContentControlItems(pItem.Owner as InlineContentControl, pItem.Index + 1, offset);
      Entity pItem1 = (Entity) pItem;
      while (!(pItem1 is WParagraph) && pItem1 != null)
      {
        pItem1 = pItem1.Owner;
        if (pItem1.Owner is WParagraph)
        {
          this.UpdateStartPosOfParaItems(pItem1 as ParagraphItem, offset);
          break;
        }
        if (pItem1.Owner is InlineContentControl)
          this.Document.UpdateStartPosOfInlineContentControlItems(pItem1.Owner as InlineContentControl, pItem1.Index + 1, offset);
      }
    }
    else
      this.UpdateStartPosOfParaItems(pItem, offset);
  }

  private void UpdateStartPosOfParaItems(ParagraphItem pItem, int offset)
  {
    int num = this.m_pItemColl.IndexOf((IEntity) pItem);
    if (!(pItem.Owner is InlineContentControl) && num < 0)
      throw new InvalidOperationException("pItem haven't found in paragraph items");
    int index = num + 1;
    for (int count = this.m_pItemColl.Count; index < count; ++index)
      this.Document.UpdateStartPos(this.m_pItemColl[index], offset);
  }

  internal void ApplyStyle(IWParagraphStyle style, bool isDomChanges)
  {
    if (style == null)
      throw new ArgumentNullException("Specified Character style not found");
    if (isDomChanges && this.ParagraphFormat.m_unParsedSprms != null && this.ParagraphFormat.m_unParsedSprms.Contain(17920))
      this.ParagraphFormat.m_unParsedSprms.RemoveValue(17920);
    this.ParaStyle = style;
    this.ApplyBaseStyleFormats();
  }

  internal void ApplyCharacterStyle(IWCharacterStyle style)
  {
    this.BreakCharacterFormat.CharStyleName = style != null ? style.Name : throw new ArgumentNullException("Specified character style not found");
    for (int index = 0; index < this.Items.Count; ++index)
    {
      ParagraphItem paragraphItem = this.Items[index];
      paragraphItem.GetCharFormat().CharStyleName = style.Name;
      switch (paragraphItem)
      {
        case Break _:
          (paragraphItem as Break).CharacterFormat.CharStyleName = style.Name;
          break;
        case InlineContentControl _:
          (paragraphItem as InlineContentControl).ApplyBaseFormatForCharacterStyle(style);
          break;
      }
    }
  }

  internal void ReplaceWithoutCorrection(int start, int length, string replacement)
  {
    this.m_strTextBuilder.Remove(start, length);
    this.m_strTextBuilder.Insert(start, replacement);
    this.IsTextReplaced = true;
  }

  internal override void AddSelf()
  {
    foreach (Entity entity in (CollectionImpl) this.Items)
      entity.AddSelf();
  }

  private void ApplyDuplicateStyleFormatting(WordDocument destDocument)
  {
    if (this.ParaStyle == null || this.ListFormat == null || this.ListFormat.CurrentListStyle == null || this.ListFormat.CurrentListLevel == null || this.ListFormat.ListType == ListType.NoList)
      return;
    IWParagraphStyle byName = (IWParagraphStyle) (destDocument.Styles.FindByName(this.StyleName) as WParagraphStyle);
    if (byName == null)
      return;
    List<int> formatProperties = this.GetListParaFormatProperties();
    this.ParagraphFormat.UpdateSourceFormat(byName.ParagraphFormat);
    foreach (int key in formatProperties)
    {
      if (this.ParagraphFormat.PropertiesHash.ContainsKey(key))
        this.ParagraphFormat.PropertiesHash.Remove(key);
    }
    formatProperties.Clear();
  }

  private List<int> GetListParaFormatProperties()
  {
    List<int> formatProperties = new List<int>();
    int[] array = new int[this.ListFormat.CurrentListLevel.ParagraphFormat.PropertiesHash.Count];
    this.ListFormat.CurrentListLevel.ParagraphFormat.PropertiesHash.Keys.CopyTo(array, 0);
    for (int index = 0; index < array.Length; ++index)
    {
      int key = array[index];
      if (!this.ParagraphFormat.PropertiesHash.ContainsKey(key))
        formatProperties.Add(key);
    }
    return formatProperties;
  }

  internal override void CloneRelationsTo(WordDocument doc, OwnerHolder nextOwner)
  {
    base.CloneRelationsTo(doc, nextOwner);
    if ((doc.ImportOptions & ImportOptions.UseDestinationStyles) != (ImportOptions) 0)
    {
      if (doc != this.Document && doc.Sections.Count == 0)
      {
        if (this.Document != null && this.Document.DefCharFormat != null)
        {
          if (doc.DefCharFormat == null)
            doc.DefCharFormat = new WCharacterFormat((IWordDocument) doc);
          doc.DefCharFormat.ImportContainer((FormatBase) this.Document.DefCharFormat);
        }
        if (this.Document != null && this.Document.m_defParaFormat != null)
        {
          if (doc.DefParaFormat == null)
            doc.DefParaFormat = new WParagraphFormat((IWordDocument) doc);
          doc.DefParaFormat.ImportContainer((FormatBase) this.Document.DefParaFormat);
        }
      }
      if (doc.UpdateAlternateChunk)
        this.ApplyDuplicateStyleFormatting(doc);
      this.CloneStyleRelations(doc);
    }
    else if ((doc.ImportOptions & ImportOptions.MergeFormatting) != (ImportOptions) 0)
      this.UpdateMergeFormatting(doc);
    else
      this.UpdateSourceFormatting(doc);
    this.ListFormat.CloneListRelationsTo(doc, (string) null);
    Entity ownerTextBody = this.GetOwnerTextBody(nextOwner as Entity);
    int index = 0;
    for (int count = this.Items.Count; index < count; ++index)
    {
      ParagraphItem paragraphItem = this.Items[index];
      if (doc.UpdateAlternateChunk && this.IsNeedToRemoveItems(ownerTextBody, paragraphItem))
      {
        paragraphItem.RemoveSelf();
      }
      else
      {
        paragraphItem.CloneRelationsTo(doc, nextOwner);
        if (this.IsNeedToAddFloatingItemsCollection((Entity) paragraphItem))
          doc.FloatingItems.Add((Entity) paragraphItem);
      }
      if (paragraphItem is TableOfContent tableOfContent && !doc.TOC.ContainsKey(tableOfContent.TOCField))
        doc.TOC.Add(tableOfContent.TOCField, paragraphItem as TableOfContent);
    }
    if ((doc.ImportOptions & ImportOptions.UseDestinationStyles) != (ImportOptions) 0)
      return;
    this.ApplyStyle(this.m_style, false);
  }

  private bool IsNeedToRemoveItems(Entity baseEntity, ParagraphItem item)
  {
    switch (baseEntity)
    {
      case WComment _:
      case HeaderFooter _:
      case WFootnote _:
        switch (item)
        {
          case WFootnote _:
          case WComment _:
            return true;
        }
        break;
    }
    return false;
  }

  private bool IsNeedToAddFloatingItemsCollection(Entity entity)
  {
    switch (entity.EntityType)
    {
      case EntityType.Picture:
      case EntityType.TextBox:
      case EntityType.XmlParaItem:
      case EntityType.Chart:
      case EntityType.OleObject:
        return true;
      case EntityType.Shape:
      case EntityType.AutoShape:
        return entity is Shape;
      default:
        return false;
    }
  }

  private void UpdateMergeFormatting(WordDocument doc)
  {
    WParagraph wparagraph = doc.LastParagraph ?? new WParagraph((IWordDocument) doc);
    this.ParagraphFormat.ImportContainer((FormatBase) wparagraph.ParagraphFormat);
    this.ParagraphFormat.CopyProperties((FormatBase) wparagraph.ParagraphFormat);
    this.BreakCharacterFormat.MergeFormat(wparagraph.BreakCharacterFormat);
    this.ParaStyle = wparagraph.m_style;
  }

  private void UpdateSourceFormatting(WordDocument doc)
  {
    if (!(doc.Styles.FindByName("Normal", StyleType.ParagraphStyle) is WParagraphStyle wparagraphStyle))
    {
      wparagraphStyle = (WParagraphStyle) Style.CreateBuiltinStyle(BuiltinStyle.Normal, doc);
      if (doc.Styles.FindByName("Normal", StyleType.ParagraphStyle) == null)
        doc.Styles.Add((IStyle) wparagraphStyle);
    }
    if ((doc.ImportOptions & ImportOptions.KeepSourceFormatting) != (ImportOptions) 0)
    {
      if (!doc.UpdateAlternateChunk || !this.IsLastItem || this.m_style == null || !(this.m_style.Name == "Normal"))
      {
        this.ParagraphFormat.UpdateSourceFormat(wparagraphStyle.ParagraphFormat);
        if (this.ParaStyle != null)
        {
          if (this.ParaStyle.ParagraphFormat.Tabs != null && this.ParaStyle.ParagraphFormat.Tabs.Count > 0)
            this.ParaStyle.ParagraphFormat.Tabs.UpdateTabs(this.ParagraphFormat.Tabs);
          if (this.ListFormat != null && !this.ListFormat.IsEmptyList && !this.ListFormat.HasKey(2) && this.ParaStyle is WParagraphStyle && (this.ParaStyle as WParagraphStyle).ListFormat != null && (this.ParaStyle as WParagraphStyle).ListFormat.CurrentListStyle != null)
          {
            this.ListFormat.ApplyStyle((this.ParaStyle as WParagraphStyle).ListFormat.CurrentListStyle.Name);
            this.ListFormat.PropertiesHash[0] = (object) (this.ParaStyle as WParagraphStyle).ListFormat.ListLevelNumber;
          }
        }
        if (this.ParagraphFormat.Tabs != null && this.ParagraphFormat.Tabs.Count > 0 && wparagraphStyle.ParagraphFormat.Tabs != null && wparagraphStyle.ParagraphFormat.Tabs.Count > 0)
          wparagraphStyle.ParagraphFormat.Tabs.UpdateSourceFormatting(this.ParagraphFormat.Tabs);
      }
      this.BreakCharacterFormat.UpdateSourceFormat(wparagraphStyle.CharacterFormat);
    }
    this.ParaStyle = (IWParagraphStyle) wparagraphStyle;
    this.IsLastItem = false;
  }

  private void CloneStyleRelations(WordDocument doc)
  {
    IStyle style = (this.m_style as Style).ImportStyleTo(doc, true);
    if (!(style is WParagraphStyle))
      return;
    if (doc == this.Document)
    {
      this.ApplyStyle((IWParagraphStyle) (style as WParagraphStyle), false);
    }
    else
    {
      this.ParaStyle = (IWParagraphStyle) (style as WParagraphStyle);
      this.m_charFormat.ApplyBase((FormatBase) this.ParaStyle.CharacterFormat);
      this.m_prFormat.ApplyBase((FormatBase) this.ParaStyle.ParagraphFormat);
    }
  }

  internal void ImportStyle(IWParagraphStyle style)
  {
    IStyle style1 = (style as Style).ImportStyleTo(this.Document, false);
    if (!(style1 is WParagraphStyle))
      return;
    this.ApplyStyle((IWParagraphStyle) (style1 as WParagraphStyle), false);
  }

  protected override object CloneImpl() => (object) this.CloneParagraph(true);

  private WParagraph CloneParagraph(bool cloneItems)
  {
    WParagraph owner = (WParagraph) base.CloneImpl();
    owner.m_strTextBuilder = new StringBuilder(this.Text);
    owner.m_pItemColl = new ParagraphItemCollection(owner);
    if (cloneItems)
      this.m_pItemColl.CloneItemsTo(owner.m_pItemColl);
    if (this.m_charFormat != null)
    {
      owner.m_charFormat = new WCharacterFormat((IWordDocument) this.Document);
      owner.m_charFormat.ImportContainer((FormatBase) this.m_charFormat);
      owner.m_charFormat.CopyProperties((FormatBase) this.m_charFormat);
      owner.m_charFormat.SetOwner((OwnerHolder) owner);
      owner.m_charFormat.CopyCharacterFormatRevision((FormatBase) this.m_charFormat);
    }
    if (this.m_prFormat != null)
    {
      owner.m_prFormat = new WParagraphFormat((IWordDocument) this.Document);
      owner.m_prFormat.ImportContainer((FormatBase) this.m_prFormat);
      owner.m_prFormat.CopyProperties((FormatBase) this.m_prFormat);
      owner.m_prFormat.SetOwner((OwnerHolder) owner);
    }
    if (this.m_listFormat != null)
    {
      owner.m_listFormat = new WListFormat((IWParagraph) this);
      owner.m_listFormat.ImportContainer((FormatBase) this.m_listFormat);
      owner.m_listFormat.SetOwner((OwnerHolder) owner);
    }
    IWParagraphStyle style = this.GetStyle();
    if (style != null)
      owner.ApplyStyle(style, false);
    owner.CreateEmptyParagraph();
    return owner;
  }

  internal string GetParagraphText(bool isLastPargraph)
  {
    string text = this.GetText(0, this.m_pItemColl.Count - 1);
    return !isLastPargraph ? text + ControlChar.ParagraphBreak : text;
  }

  internal bool HasNonHiddenPara()
  {
    for (IEntity nextSibling = this.NextSibling; nextSibling != null; nextSibling = nextSibling.NextSibling)
    {
      if (!this.IsPerviousHiddenPara(nextSibling) || this.IsPreviousParagraphHasContent(nextSibling))
        return nextSibling is WParagraph;
    }
    return true;
  }

  internal bool IsPreviousParagraphMarkIsHidden()
  {
    bool flag = false;
    for (IEntity previousSibling = this.PreviousSibling; previousSibling != null && this.IsPerviousHiddenPara(previousSibling); previousSibling = previousSibling.PreviousSibling)
    {
      flag = this.IsPreviousParagraphHasContent(previousSibling);
      if (flag)
      {
        flag = !((previousSibling as WParagraph).LastItem is Break) || ((previousSibling as WParagraph).LastItem as Break).BreakType != BreakType.PageBreak;
        break;
      }
    }
    return flag;
  }

  internal bool IsPerviousHiddenPara(IEntity prevsibling)
  {
    return prevsibling is WParagraph && (prevsibling as WParagraph).BreakCharacterFormat.Hidden || prevsibling is BlockContentControl && (prevsibling as BlockContentControl).IsHiddenParagraphMarkIsInLastItemOfSDTContent();
  }

  internal bool IsPreviousParagraphMarkIsInDeletion()
  {
    bool flag = false;
    for (IEntity previousSibling = this.PreviousSibling; previousSibling != null && this.IsPreviousParaInDeletion(previousSibling); previousSibling = previousSibling.PreviousSibling)
      flag = this.IsPreviousParagraphHasContent(previousSibling);
    return flag;
  }

  internal bool IsPreviousParaInDeletion(IEntity prevsibling)
  {
    return prevsibling is WParagraph && (prevsibling as WParagraph).BreakCharacterFormat.IsDeleteRevision || prevsibling is BlockContentControl && (prevsibling as BlockContentControl).IsDeletionParagraphMarkIsInLastItemOfSDTContent();
  }

  private bool IsPreviousParagraphHasContent(IEntity prevsibling)
  {
    return !(prevsibling is WParagraph) || !(prevsibling as WParagraph).IsEmptyParagraph();
  }

  internal string GetText(int startIndex, int endIndex)
  {
    string text = string.Empty;
    bool isPicBullet = false;
    if (!this.ListFormat.IsEmptyList && this.ChildEntities.Count != 0 && !this.SectionEndMark)
      text = this.GetListText(false, ref isPicBullet);
    for (int index = startIndex; index <= endIndex; ++index)
    {
      ParagraphItem paragraphItem = this.m_pItemColl[index];
      switch (paragraphItem)
      {
        case WField _:
          text += (paragraphItem as WField).Text;
          if (this.Document.m_prevClonedEntity == null)
          {
            if ((paragraphItem as WField).FieldEnd != null && (paragraphItem as WField).FieldEnd.OwnerParagraph == this)
            {
              index = (paragraphItem as WField).FieldEnd.GetIndexInOwnerCollection();
              break;
            }
            break;
          }
          goto label_12;
        case WTextRange _:
          WTextRange wtextRange = paragraphItem as WTextRange;
          if (!wtextRange.CharacterFormat.Hidden)
          {
            text = wtextRange.CharacterFormat.AllCaps || wtextRange.CharacterFormat.SmallCaps ? text + wtextRange.Text.ToUpper() : text + wtextRange.Text;
            break;
          }
          break;
        case Break _:
          text += ControlChar.ParagraphBreak;
          break;
      }
    }
label_12:
    return text;
  }

  private void ApplyBaseStyleFormats()
  {
    if (this.m_style == null)
      return;
    this.m_charFormat.ApplyBase((FormatBase) this.m_style.CharacterFormat);
    this.m_prFormat.ApplyBase((FormatBase) this.m_style.ParagraphFormat);
    int index = 0;
    for (int count = this.m_pItemColl.Count; index < count; ++index)
    {
      ParagraphItem paragraphItem = this.m_pItemColl[index];
      paragraphItem.ParaItemCharFormat.ApplyBase((FormatBase) this.m_style.CharacterFormat);
      switch (paragraphItem)
      {
        case WMergeField _:
          (paragraphItem as WMergeField).ApplyBaseFormat();
          break;
        case Break _:
          (paragraphItem as Break).CharacterFormat.ApplyBase((FormatBase) this.m_style.CharacterFormat);
          break;
        case InlineContentControl _:
          (paragraphItem as InlineContentControl).ApplyBaseFormat();
          break;
        case WMath _:
          (paragraphItem as WMath).ApplyBaseFormat();
          break;
      }
    }
  }

  internal bool IsEmptyParagraph()
  {
    for (int index = 0; index < this.ChildEntities.Count; ++index)
    {
      if (this.ChildEntities[index] is WTextRange)
      {
        int num;
        if (!(this.ChildEntities[index] is WField))
          num = (this.ChildEntities[index] as WTextRange).Text.Trim(ControlChar.SpaceChar) == string.Empty ? 1 : 0;
        else
          num = 1;
        if (num != 0 && !((this.ChildEntities[index] as WTextRange).m_layoutInfo is TabsLayoutInfo))
          continue;
      }
      if (!(this.ChildEntities[index] is BookmarkStart) && !(this.ChildEntities[index] is BookmarkEnd) && !(this.ChildEntities[index] is WFieldMark))
        return false;
    }
    return true;
  }

  internal void CheckFormFieldName(string formFieldName)
  {
    Bookmark bookmark = this.Document.Bookmarks[formFieldName];
    if (bookmark == null)
      return;
    this.Document.Bookmarks.Remove(bookmark);
    foreach (WSection section in (CollectionImpl) this.Document.Sections)
    {
      if (section.Body.FormFields != null)
      {
        WFormField formField = section.Body.FormFields[formFieldName];
        if (formField != null)
          formField.Name = string.Empty;
      }
    }
  }

  private void ApplyListStyle(BuiltinStyle builtinStyle)
  {
    string name = Style.BuiltInToName(builtinStyle);
    ListStyle style1 = this.Document.ListStyles.FindByName(name);
    if (style1 == null)
    {
      style1 = (ListStyle) Style.CreateBuiltinStyle(builtinStyle, StyleType.OtherStyle, this.Document);
      this.Document.ListStyles.Add(style1);
    }
    if (!(this.Document.Styles.FindByName(style1.Name) is IWParagraphStyle style2))
    {
      style2 = (IWParagraphStyle) new WParagraphStyle((IWordDocument) this.Document);
      style2.Name = name;
      (style2 as WParagraphStyle).ApplyBaseStyle("Normal");
      this.Document.Styles.Add((IStyle) style2);
      this.Document.UpdateNextStyle(style2 as Style);
    }
    this.ApplyStyle(style2, false);
    this.ListFormat.ApplyStyle(style1.Name);
  }

  private void CheckNormalStyle()
  {
    if ((IStyle) (this.Document.Styles.FindByName("Normal", StyleType.ParagraphStyle) as WParagraphStyle) == null)
      this.Document.Styles.Add(Style.CreateBuiltinStyle(BuiltinStyle.Normal, this.Document));
    if ((IStyle) (this.Document.Styles.FindByName("Default Paragraph Font", StyleType.CharacterStyle) as WCharacterStyle) != null)
      return;
    IStyle builtinCharacterStyle = Style.CreateBuiltinCharacterStyle(BuiltinStyle.DefaultParagraphFont, this.Document);
    (builtinCharacterStyle as Style).IsSemiHidden = true;
    (builtinCharacterStyle as Style).UnhideWhenUsed = true;
    this.Document.Styles.Add(builtinCharacterStyle);
  }

  internal override void Close()
  {
    if (this.m_pItemColl != null)
    {
      this.m_pItemColl.Close();
      this.m_pItemColl = (ParagraphItemCollection) null;
    }
    if (this.m_paragraphItems != null)
    {
      this.m_paragraphItems.Close();
      this.m_paragraphItems = (ParagraphItemCollection) null;
    }
    if (this.m_prFormat != null)
    {
      this.m_prFormat.Close();
      this.m_prFormat = (WParagraphFormat) null;
    }
    if (this.m_charFormat != null)
    {
      this.m_charFormat.Close();
      this.m_charFormat = (WCharacterFormat) null;
    }
    if (this.m_style != null)
      this.m_style = (IWParagraphStyle) null;
    if (this.m_listFormat != null)
    {
      this.m_listFormat.Close();
      this.m_listFormat = (WListFormat) null;
    }
    if (this.m_pEmptyItemColl != null)
    {
      this.m_pEmptyItemColl.Close();
      this.m_pEmptyItemColl = (ParagraphItemCollection) null;
    }
    if (this.m_strTextBuilder != null)
      this.m_strTextBuilder = (StringBuilder) null;
    if (TextFinder.Instance.SingleLinePCol.Contains(this))
      TextFinder.Instance.SingleLinePCol.Remove(this);
    base.Close();
  }

  internal void ApplyListParaStyle()
  {
    if (this.ParaStyle != null && (this.ParaStyle as WParagraphStyle).StyleId == 179)
      return;
    if (!((this.Document.Styles as StyleCollection).FindById(179) is WParagraphStyle wparagraphStyle))
    {
      wparagraphStyle = new WParagraphStyle((IWordDocument) this.Document);
      wparagraphStyle.StyleId = 179;
      wparagraphStyle.Name = "List Paragraph";
      wparagraphStyle.NextStyle = "List Paragraph";
      this.Document.Styles.Add((IStyle) wparagraphStyle);
    }
    this.ParaStyle = (IWParagraphStyle) wparagraphStyle;
  }

  internal FieldType GetLastFieldType()
  {
    IEntity entity = (IEntity) this.Items.LastItem;
    while (true)
    {
      switch (entity)
      {
        case null:
        case WField _:
          goto label_3;
        default:
          entity = entity.PreviousSibling;
          continue;
      }
    }
label_3:
    return entity != null && entity is WField ? (entity as WField).FieldType : FieldType.FieldUnknown;
  }

  internal WField GetLastField()
  {
    IEntity entity = (IEntity) this.Items.LastItem;
    while (true)
    {
      switch (entity)
      {
        case null:
        case WField _:
          goto label_3;
        default:
          entity = entity.PreviousSibling;
          continue;
      }
    }
label_3:
    return entity != null && entity is WField ? entity as WField : (WField) null;
  }

  internal WTableCell GetOwnerTableCell(WTextBody Owner)
  {
    if (Owner is WTableCell)
      return Owner as WTableCell;
    for (BlockContentControl owner = Owner == null || !(Owner.Owner is BlockContentControl) ? (BlockContentControl) null : Owner.Owner as BlockContentControl; owner != null; owner = (owner.Owner as WTextBody).Owner as BlockContentControl)
    {
      if (owner.Owner is WTableCell)
        return owner.Owner as WTableCell;
      if (!(owner.Owner is WTextBody) || !((owner.Owner as WTextBody).Owner is BlockContentControl))
        return (WTableCell) null;
    }
    return (WTableCell) null;
  }

  private void CreateEmptyParagraph()
  {
    this.m_pEmptyItemColl = new ParagraphItemCollection(this);
    WTextRange paragraphItem = (WTextRange) this.Document.CreateParagraphItem(ParagraphItemType.TextRange);
    paragraphItem.Text = " ";
    paragraphItem.CharacterFormat.ApplyBase((FormatBase) this.m_charFormat);
    this.m_pEmptyItemColl.UnsafeAdd((ParagraphItem) paragraphItem);
  }

  internal bool IsParagraphHasSectionBreak()
  {
    if (this != null && this.NextSibling == null && this.OwnerTextBody != null && !(this.OwnerTextBody is HeaderFooter))
    {
      if (this.OwnerTextBody.Owner != null && this.OwnerTextBody.Owner is WSection)
        return true;
      if (this.OwnerTextBody.Owner != null && this.OwnerTextBody.Owner is BlockContentControl)
      {
        BlockContentControl owner1 = this.OwnerTextBody.Owner as BlockContentControl;
        if (owner1.Owner != null && !(owner1.Owner is HeaderFooter))
        {
          WTextBody owner2 = owner1.Owner as WTextBody;
          if (owner2.Owner is WSection && owner1 == owner2.Items.LastItem)
            return true;
        }
      }
    }
    return false;
  }

  private bool IsSectionEndMark()
  {
    if (this != null && this.NextSibling == null && this.OwnerTextBody != null && !(this.OwnerTextBody is HeaderFooter))
    {
      if (this.OwnerTextBody.Owner != null && this.OwnerTextBody.Owner is WSection)
      {
        if (this.OwnerTextBody.Owner is WSection owner1 && owner1.NextSibling != null)
        {
          string str = this.ModifyText(this.Text);
          if (owner1 != null && !str.Contains('\r'.ToString()) && (this.ChildEntities.Count == 0 || this.IsEmptyParagraph()))
            return true;
        }
      }
      else if (this.OwnerTextBody.Owner != null && this.OwnerTextBody.Owner is BlockContentControl)
      {
        BlockContentControl owner2 = this.OwnerTextBody.Owner as BlockContentControl;
        if (owner2.Owner != null && !(owner2.Owner is HeaderFooter))
        {
          WTextBody owner3 = owner2.Owner as WTextBody;
          if (owner3.Owner is WSection && owner2 == owner3.Items.LastItem)
          {
            WSection owner4 = owner3.Owner as WSection;
            string str = this.ModifyText(this.Text);
            if (owner4 != null && !str.Contains('\r'.ToString()) && (this.ChildEntities.Count == 0 || this.IsEmptyParagraph()))
              return true;
          }
        }
      }
    }
    return false;
  }

  private string ModifyText(string text)
  {
    char newChar = '\r';
    char oldChar = '\n';
    text = text.Replace(Environment.NewLine, newChar.ToString());
    text = text.Replace(oldChar, newChar);
    text = text.Replace('\a'.ToString(), string.Empty);
    text = text.Replace('\b'.ToString(), string.Empty);
    return text;
  }

  internal void UpdateBookmarkEnd(ParagraphItem item, WParagraph paragraphStart, bool isAddItem)
  {
    int index = -1;
    WParagraph wparagraph = (WParagraph) null;
    if (item is BookmarkStart && !this.StartsWithExt((item as BookmarkStart).Name, "_"))
    {
      Bookmark byName = paragraphStart.Document.Bookmarks.FindByName((item as BookmarkStart).Name);
      if (byName != null && byName.BookmarkEnd != null)
      {
        index = byName.BookmarkEnd.Index;
        wparagraph = byName.BookmarkEnd.OwnerParagraph;
      }
    }
    if (isAddItem)
    {
      paragraphStart.Items.Add((IEntity) item);
    }
    else
    {
      paragraphStart.Items.Insert(0, (IEntity) item);
      if (paragraphStart != null && wparagraph != null && paragraphStart == wparagraph)
        ++index;
    }
    if (wparagraph == null)
      return;
    Bookmark bookmark = (Bookmark) null;
    if (isAddItem && paragraphStart.LastItem is BookmarkStart)
      bookmark = paragraphStart.Document.Bookmarks.FindByName((paragraphStart.LastItem as BookmarkStart).Name);
    else if (!isAddItem && paragraphStart.Items[0] is BookmarkStart)
      bookmark = paragraphStart.Document.Bookmarks.FindByName((paragraphStart.Items[0] as BookmarkStart).Name);
    if (bookmark == null || bookmark.BookmarkEnd != null || wparagraph.Items.Count <= index || !(wparagraph.Items[index] is BookmarkEnd))
      return;
    bookmark.SetEnd(wparagraph.Items[index] as BookmarkEnd);
  }

  internal override void MakeChanges(bool acceptChanges)
  {
    if (acceptChanges && this.m_listFormat != null)
    {
      if (this.m_listFormat.OldPropertiesHash != null && this.m_listFormat.OldPropertiesHash.Count > 0)
        this.m_listFormat.OldPropertiesHash.Clear();
      if (this.ParagraphFormat.m_unParsedSprms != null && this.ParagraphFormat.m_unParsedSprms.Count > 0)
      {
        if (this.ParagraphFormat.m_unParsedSprms[50757] != null)
          this.ParagraphFormat.m_unParsedSprms.RemoveValue(50757);
        if (this.ParagraphFormat.m_unParsedSprms[9283] != null)
          this.ParagraphFormat.m_unParsedSprms.RemoveValue(9283);
      }
    }
    this.BreakCharacterFormat.AcceptChanges();
    for (int index = 0; index < this.m_pItemColl.Count; ++index)
    {
      ParagraphItem paragraphItem = this.m_pItemColl[index];
      if (paragraphItem.IsDeleteRevision && acceptChanges || paragraphItem.IsInsertRevision && !acceptChanges)
      {
        this.m_pItemColl.RemoveAt(index);
        --index;
      }
      else
      {
        if (paragraphItem.IsChangedCFormat && !acceptChanges)
          paragraphItem.RemoveChanges();
        if (paragraphItem is Break)
        {
          if (acceptChanges && paragraphItem is Break && (paragraphItem as Break).CharacterFormat.IsDeleteRevision || !acceptChanges && paragraphItem is Break && (paragraphItem as Break).CharacterFormat.IsInsertRevision)
          {
            this.m_pItemColl.RemoveAt(index);
            --index;
            continue;
          }
          (paragraphItem as Break).TextRange.AcceptChanges();
        }
        paragraphItem.AcceptChanges();
        if (paragraphItem is WTextBox)
          (paragraphItem as WTextBox).TextBoxBody.MakeChanges(acceptChanges);
        else if (paragraphItem is WFootnote)
          (paragraphItem as WFootnote).TextBody.MakeChanges(acceptChanges);
      }
    }
  }

  internal override void RemoveCFormatChanges()
  {
    if (this.m_charFormat == null)
      return;
    this.m_charFormat.RemoveChanges();
  }

  internal override void RemovePFormatChanges()
  {
    if (this.m_prFormat == null)
      return;
    this.m_prFormat.RemoveChanges();
  }

  internal override void AcceptCChanges()
  {
    if (this.m_charFormat == null)
      return;
    this.m_charFormat.AcceptChanges();
  }

  internal override void AcceptPChanges()
  {
    if (this.m_prFormat == null)
      return;
    this.m_prFormat.AcceptChanges();
  }

  internal override bool CheckChangedPFormat()
  {
    return this.m_prFormat != null && this.m_prFormat.IsChangedFormat;
  }

  internal override bool CheckInsertRev()
  {
    return this.m_charFormat != null && this.m_charFormat.IsInsertRevision;
  }

  internal override bool CheckDeleteRev()
  {
    return this.m_charFormat != null && this.m_charFormat.IsDeleteRevision;
  }

  internal override bool CheckChangedCFormat()
  {
    return this.m_charFormat != null && this.m_charFormat.IsChangedFormat;
  }

  internal bool CheckOnRemove()
  {
    foreach (ParagraphItem paragraphItem in (CollectionImpl) this.m_pItemColl)
    {
      if (!paragraphItem.IsDeleteRevision)
        return false;
    }
    return true;
  }

  internal override bool HasTrackedChanges()
  {
    if (this.IsInsertRevision || this.IsDeleteRevision || this.IsChangedCFormat || this.IsChangedPFormat)
      return true;
    foreach (ParagraphItem paragraphItem in (CollectionImpl) this.m_pItemColl)
    {
      if (paragraphItem.HasTrackedChanges())
        return true;
    }
    return false;
  }

  internal override void SetDeleteRev(bool check)
  {
    if (this.m_charFormat == null)
      return;
    this.m_charFormat.IsDeleteRevision = check;
  }

  internal override void SetInsertRev(bool check)
  {
    if (this.m_charFormat == null)
      return;
    this.m_charFormat.IsInsertRevision = check;
  }

  internal override void SetChangedCFormat(bool check)
  {
    if (this.m_charFormat == null)
      return;
    this.m_charFormat.IsChangedFormat = check;
  }

  internal override void SetChangedPFormat(bool check)
  {
    if (this.m_prFormat == null)
      return;
    this.m_prFormat.IsChangedFormat = check;
  }

  internal bool IsOnlyHasSpaces()
  {
    if (!this.IsParagraphContainsOnlyTextRange())
      return false;
    return string.IsNullOrEmpty(this.Text.Trim(' ', ' ', ' ', '　'));
  }

  private bool IsParagraphContainsOnlyTextRange()
  {
    foreach (Entity childEntity in (CollectionImpl) this.ChildEntities)
    {
      if (!(childEntity is WTextRange))
        return false;
    }
    return true;
  }

  internal override TextBodyItem GetNextTextBodyItemValue()
  {
    if (this.NextSibling != null)
      return this.NextSibling as TextBodyItem;
    Entity ownerEntity = this.GetOwnerEntity();
    if (this.IsInCell)
      return (ownerEntity as WTableCell).GetNextTextBodyItem();
    if (this.Owner is WTextBody)
    {
      if (ownerEntity is WTextBox)
        return (ownerEntity as WTextBox).GetNextTextBodyItem();
      if (this.OwnerTextBody.Owner is WSection)
        return this.GetNextInSection(this.OwnerTextBody.Owner as WSection);
    }
    return (TextBodyItem) null;
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    writer.WriteValue("type", "Paragraph");
  }

  protected override void InitXDLSHolder()
  {
    this.XDLSHolder.AddRefElement("style", (object) this.GetStyle());
    this.XDLSHolder.AddElement("paragraph-format", (object) this.m_prFormat);
    this.XDLSHolder.AddElement("character-format", (object) this.m_charFormat);
    this.XDLSHolder.AddElement("list-format", (object) this.ListFormat);
    this.XDLSHolder.AddElement("items", (object) this.m_pItemColl);
  }

  protected override void RestoreReference(string name, int index)
  {
    if (!(name == "style") || index <= -1)
      return;
    this.ParaStyle = this.Document.Styles[index] as IWParagraphStyle;
    this.ApplyBaseStyleFormats();
  }

  internal bool HasNoRenderableItem()
  {
    if (this.ChildEntities.Count == 0)
      return true;
    Entity entity = (Entity) this.LastItem;
label_3:
    entity = entity is InlineContentControl ? (entity as InlineContentControl).ParagraphItems.LastItem : entity;
    while (true)
    {
      switch (entity)
      {
        case BookmarkStart _:
        case BookmarkEnd _:
        case EditableRangeStart _:
        case EditableRangeEnd _:
          entity = entity.PreviousSibling as Entity;
          continue;
        case InlineContentControl _:
          goto label_3;
        case null:
          goto label_7;
        default:
          goto label_6;
      }
    }
label_6:
    return false;
label_7:
    return true;
  }

  internal bool IsFirstLine(LayoutedWidget ltWidget)
  {
    IWidget widget = ltWidget.Widget;
    if (widget is SplitWidgetContainer)
      widget = (widget as SplitWidgetContainer).m_currentChild;
    if (widget is SplitStringWidget)
    {
      if ((widget as SplitStringWidget).StartIndex > 0)
        return false;
      widget = (IWidget) (widget as SplitStringWidget).RealStringWidget;
    }
    IWidget previousSibling;
    for (previousSibling = this.GetPreviousSibling(widget); previousSibling != null; previousSibling = this.GetPreviousSibling(previousSibling))
    {
      if (!previousSibling.LayoutInfo.IsSkip && !previousSibling.LayoutInfo.IsSkipBottomAlign)
      {
        switch (previousSibling)
        {
          case InlineShapeObject _:
          case WFieldMark _:
          case BookmarkStart _:
          case BookmarkEnd _:
            continue;
          case Break _:
            if ((previousSibling as Break).BreakType == BreakType.LineBreak)
              goto label_12;
            continue;
          default:
            goto label_12;
        }
      }
    }
label_12:
    return previousSibling == null;
  }

  internal IWidget GetPreviousSibling(IWidget widget)
  {
    IList innerList = (this.WidgetCollection as ParagraphItemCollection).InnerList;
    int num = innerList.IndexOf((object) widget);
    switch (num)
    {
      case -1:
      case 0:
        return (IWidget) null;
      default:
        return innerList[num - 1] as IWidget;
    }
  }

  internal bool IsLastLine(LayoutedWidget ltWidget)
  {
    IWidget widget = ltWidget.Widget;
    if (widget is SplitStringWidget)
    {
      SplitStringWidget splitStringWidget = widget as SplitStringWidget;
      int num = -1;
      if (!string.IsNullOrEmpty(splitStringWidget.SplittedText))
        num = splitStringWidget.SplittedText.Length;
      if (splitStringWidget.StartIndex + num != (splitStringWidget.RealStringWidget as WTextRange).Text.Length)
        return false;
      widget = (IWidget) (widget as SplitStringWidget).RealStringWidget;
    }
    IWidget nextSibling = this.GetNextSibling(widget);
    if (nextSibling != null && ltWidget.Widget is Break && (ltWidget.Widget as Break).BreakType == BreakType.LineBreak)
      return false;
    for (; nextSibling != null; nextSibling = this.GetNextSibling(nextSibling))
    {
      if (!nextSibling.LayoutInfo.IsSkip && !nextSibling.LayoutInfo.IsSkipBottomAlign)
      {
        switch (nextSibling)
        {
          case InlineShapeObject _:
          case WFieldMark _:
          case BookmarkStart _:
          case BookmarkEnd _:
            continue;
          default:
            goto label_12;
        }
      }
    }
label_12:
    return nextSibling == null;
  }

  internal IWidget GetNextSibling(IWidget widget)
  {
    IList innerList = (this.WidgetCollection as ParagraphItemCollection).InnerList;
    int num = innerList.IndexOf((object) widget);
    return num < 0 || num > innerList.Count - 2 ? (IWidget) null : innerList[num + 1] as IWidget;
  }

  void IWidget.InitLayoutInfo()
  {
    if (this.m_layoutInfo is ParagraphLayoutInfo)
      (this.m_layoutInfo as ParagraphLayoutInfo).InitLayoutInfo();
    this.m_layoutInfo = (ILayoutInfo) null;
  }

  void IWidget.InitLayoutInfo(IWidget widget)
  {
  }

  protected override void CreateLayoutInfo()
  {
    this.m_layoutInfo = (ILayoutInfo) new WParagraph.LayoutParagraphInfoImpl(this);
    WParagraphFormat paragraphFormat = this.ParagraphFormat;
    if (this.IsHiddenParagraph() && !this.IsParagraphHasSectionBreak() && this.BreakCharacterFormat.Hidden && (!this.IsInCell || this.NextSibling != null) || this.IsParagraphContainsOnlyBookMarks() && this.IsSkipCellMark())
      this.m_layoutInfo.IsSkip = true;
    if (this.IsVerticalText())
      this.m_layoutInfo.IsVerticalText = true;
    if ((double) paragraphFormat.LineSpacing == 0.0 && paragraphFormat.LineSpacingRule == LineSpacingRule.Multiple && (double) paragraphFormat.BeforeSpacing == 0.0 && (double) paragraphFormat.AfterSpacing == 0.0 && (double) paragraphFormat.AfterLines == 0.0 && (double) paragraphFormat.BeforeLines == 0.0 && !paragraphFormat.SpaceAfterAuto && !paragraphFormat.SpaceBeforeAuto)
      this.m_layoutInfo.IsSkip = true;
    if (this.Text == string.Empty && this.RemoveEmpty)
      this.m_layoutInfo.IsSkip = true;
    if (this.BreakCharacterFormat.IsDeleteRevision && this.IsDeletionParagraph())
      this.m_layoutInfo.IsSkip = true;
    this.SplitByLineBreak(this.Items);
    TextSplitter splitter = new TextSplitter();
    this.SplitTextRangeByScriptType(this.Items, splitter);
    this.SplitLtrAndRtlText(this.Items, splitter);
    this.CombineconsecutiveRTL(this.Items);
  }

  internal bool IsDeletionParagraph()
  {
    bool flag = false;
    for (int index = 0; index < this.Items.Count; ++index)
    {
      flag = !(this.Items[index] is InlineContentControl) ? (this.Items[index] is Break ? (this.Items[index] as Break).CharacterFormat.IsDeleteRevision : this.Items[index].ParaItemCharFormat.IsDeleteRevision) : (this.Items[index] as InlineContentControl).IsDeletion();
      if (!flag)
        break;
    }
    if (this.IsEmptyParagraph())
      flag = true;
    return flag;
  }

  private void CombineconsecutiveRTL(ParagraphItemCollection paraItems)
  {
    for (int index = 0; index <= paraItems.Count - 2; ++index)
    {
      if (paraItems[index] is InlineContentControl paraItem1)
        this.CombineconsecutiveRTL(paraItem1.ParagraphItems);
      if (paraItems[index] is WTextRange && paraItems[index + 1] is WTextRange)
      {
        if (paraItems[index] is WField)
        {
          IWidget paraItem2 = (IWidget) paraItems[index];
          if (paraItem2 != null && paraItem2.LayoutInfo.IsSkip)
            continue;
        }
        WTextRange paraItem3 = paraItems[index] as WTextRange;
        WTextRange paraItem4 = paraItems[index + 1] as WTextRange;
        if (paraItem3.CharacterRange == CharacterRangeType.RTL && paraItem4.CharacterRange == CharacterRangeType.RTL && !(paraItem3 is WField) && !(paraItem4 is WField) && paraItem3.Text.Length > 0 && paraItem4.Text.Length > 0 && !TextSplitter.IsWordSplitChar(paraItem3.Text[paraItem3.Text.Length - 1]) && !TextSplitter.IsWordSplitChar(paraItem4.Text[0]) && paraItem3.CharacterFormat.Compare(paraItem4.CharacterFormat))
        {
          paraItem3.Text += paraItem4.Text;
          paraItems.RemoveAt(index + 1);
          --index;
        }
      }
    }
  }

  private bool IsParagraphContainsOnlyBookMarks()
  {
    for (int index = 0; index < this.ChildEntities.Count; ++index)
    {
      if (!(this.ChildEntities[index] is BookmarkStart) && !(this.ChildEntities[index] is BookmarkEnd))
        return false;
    }
    return true;
  }

  private void SplitTextRangeByScriptType(ParagraphItemCollection paraItems, TextSplitter splitter)
  {
    int num;
    for (int index1 = 0; index1 < paraItems.Count; index1 += num)
    {
      num = 1;
      if (paraItems[index1] is InlineContentControl paraItem1)
        this.SplitTextRangeByScriptType(paraItem1.ParagraphItems, splitter);
      WTextRange paraItem2 = paraItems[index1] as WTextRange;
      if (paraItems[index1] is WFieldMark && (paraItems[index1] as WFieldMark).Type == FieldMarkType.FieldEnd && (paraItems[index1] as WFieldMark).ParentField != null)
        (paraItems[index1] as WFieldMark).ParentField.IsFieldRangeUpdated = false;
      if (paraItems[index1] is WField)
      {
        IWidget paraItem3 = (IWidget) paraItems[index1];
        if (paraItem3 != null && paraItem3.LayoutInfo.IsSkip)
          continue;
      }
      if (paraItem2 != null && !(paraItems[index1] is WField) && (paraItem2.m_layoutInfo == null || !paraItem2.m_layoutInfo.IsSkip))
      {
        int idctHint = (int) paraItem2.CharacterFormat.IdctHint;
        List<FontScriptType> fontScriptTypes = new List<FontScriptType>();
        string[] strArray = splitter.SplitTextByFontScriptType(paraItem2.Text, ref fontScriptTypes);
        if (strArray.Length > 1)
        {
          for (int index2 = 0; index2 < strArray.Length; ++index2)
          {
            string str = strArray[index2];
            if (index2 > 0)
            {
              WTextRange wtextRange = paraItem2.Clone() as WTextRange;
              wtextRange.Text = str;
              wtextRange.ScriptType = fontScriptTypes[index2];
              paraItems.Insert(index1 + index2, (IEntity) wtextRange);
              ++num;
            }
            else
            {
              paraItem2.Text = str;
              paraItem2.ScriptType = fontScriptTypes[index2];
            }
          }
        }
        else if (strArray.Length > 0)
          paraItem2.ScriptType = fontScriptTypes[0];
        fontScriptTypes.Clear();
      }
    }
  }

  private void SplitLtrAndRtlText(ParagraphItemCollection paraItems, TextSplitter splitter)
  {
    bool? isPrevLTRText = new bool?();
    bool hasRTLCharacter = false;
    List<CharacterRangeType> characterRangeTypes = new List<CharacterRangeType>();
    int num;
    for (int index1 = 0; index1 < paraItems.Count; index1 += num)
    {
      num = 1;
      if (paraItems[index1] is InlineContentControl paraItem1)
        this.SplitLtrAndRtlText(paraItem1.ParagraphItems, splitter);
      WTextRange paraItem2 = paraItems[index1] as WTextRange;
      if (paraItems[index1] is WFieldMark && (paraItems[index1] as WFieldMark).Type == FieldMarkType.FieldEnd && (paraItems[index1] as WFieldMark).ParentField != null)
        (paraItems[index1] as WFieldMark).ParentField.IsFieldRangeUpdated = false;
      if (paraItems[index1] is WField)
      {
        IWidget paraItem3 = (IWidget) paraItems[index1];
        if (paraItem3 != null && paraItem3.LayoutInfo.IsSkip)
          continue;
      }
      if (paraItem2 != null && !(paraItems[index1] is WField) && (paraItem2.m_layoutInfo == null || !paraItem2.m_layoutInfo.IsSkip))
      {
        string text = paraItem2.Text;
        bool bidi = paraItem2.CharacterFormat.Bidi;
        bool isRTLLang = false;
        int count = characterRangeTypes.Count;
        if (bidi && paraItem2.CharacterFormat.HasValueWithParent(75))
          isRTLLang = this.IsRightToLeftLang(paraItem2.CharacterFormat.LocaleIdBidi);
        string[] strArray = splitter.SplitTextByConsecutiveLtrAndRtl(text, bidi, isRTLLang, ref characterRangeTypes, ref isPrevLTRText, ref hasRTLCharacter);
        if (strArray.Length > 1)
        {
          for (int index2 = 0; index2 < strArray.Length; ++index2)
          {
            string str = strArray[index2];
            if (index2 > 0)
            {
              WTextRange wtextRange = paraItem2.Clone() as WTextRange;
              wtextRange.Text = str;
              wtextRange.CharacterRange = characterRangeTypes[index2 + count];
              paraItems.Insert(index1 + index2, (IEntity) wtextRange);
              ++num;
            }
            else
            {
              paraItem2.Text = str;
              paraItem2.CharacterRange = characterRangeTypes[count];
            }
          }
        }
        else if (strArray.Length > 0)
          paraItem2.CharacterRange = characterRangeTypes[count];
      }
    }
    characterRangeTypes.Clear();
  }

  private bool IsRightToLeftLang(short id)
  {
    LocaleIDs localeIds = (LocaleIDs) id;
    switch (localeIds)
    {
      case LocaleIDs.ar_SA:
      case LocaleIDs.ar_IQ:
      case LocaleIDs.ar_EG:
      case LocaleIDs.ar_LY:
      case LocaleIDs.ar_DZ:
      case LocaleIDs.ar_TN:
      case LocaleIDs.ar_OM:
      case LocaleIDs.ar_YE:
      case LocaleIDs.ar_SY:
      case LocaleIDs.ar_JO:
      case LocaleIDs.ar_LB:
      case LocaleIDs.ar_KW:
      case LocaleIDs.ar_AE:
      case LocaleIDs.ar_BH:
      case LocaleIDs.ar_QA:
        return true;
      default:
        return localeIds == LocaleIDs.fa_IR;
    }
  }

  internal string GetDisplayText(ParagraphItemCollection ParagraphItems)
  {
    string displayText = "";
    for (int index = 0; index < ParagraphItems.Count; ++index)
    {
      IWidget paragraphItem = (IWidget) ParagraphItems[index];
      if (!(paragraphItem is WField) && paragraphItem is WTextRange && !paragraphItem.LayoutInfo.IsSkip)
        displayText = !((paragraphItem as WTextRange).Text == "") || !(paragraphItem.LayoutInfo is TabsLayoutInfo) ? displayText + (paragraphItem as WTextRange).Text : displayText + "\t";
      else if (paragraphItem is InlineContentControl)
        displayText += this.GetDisplayText((paragraphItem as InlineContentControl).ParagraphItems);
    }
    return displayText;
  }

  internal override void InitLayoutInfo(Entity entity, ref bool isLastTOCEntry)
  {
    this.m_layoutInfo = (ILayoutInfo) null;
    foreach (Entity childEntity in (CollectionImpl) this.ChildEntities)
    {
      childEntity.InitLayoutInfo(entity, ref isLastTOCEntry);
      if (isLastTOCEntry)
        return;
    }
    if (this != entity)
      return;
    isLastTOCEntry = true;
  }

  internal bool IsVerticalText()
  {
    Entity ownerEntity = this.GetOwnerEntity();
    if (this.IsInCell && ownerEntity is WTableCell && ((ownerEntity as WTableCell).CellFormat.TextDirection == TextDirection.VerticalTopToBottom || (ownerEntity as WTableCell).CellFormat.TextDirection == TextDirection.VerticalBottomToTop) || ownerEntity is Shape && (ownerEntity as Shape).m_layoutInfo.IsVerticalText || ownerEntity is ChildShape && !(ownerEntity is ChildGroupShape) && (ownerEntity as ChildShape).m_layoutInfo.IsVerticalText)
      return true;
    return ownerEntity is WTextBox && (ownerEntity as WTextBox).m_layoutInfo.IsVerticalText;
  }

  private bool IsHiddenParagraph()
  {
    bool flag = false;
    for (int index = 0; index < this.Items.Count; ++index)
    {
      flag = !(this.Items[index] is InlineContentControl) ? (this.Items[index] is Break ? (this.Items[index] as Break).CharacterFormat.Hidden : this.Items[index].ParaItemCharFormat.Hidden) : (this.Items[index] as InlineContentControl).IsHidden();
      if (!flag)
        break;
    }
    if (this.IsEmptyParagraph())
      flag = true;
    return flag;
  }

  private void SplitByLineBreak(ParagraphItemCollection paraItems)
  {
    if (!this.Text.Contains(ControlChar.LineBreak))
      return;
    for (int index1 = 0; index1 < paraItems.Count; ++index1)
    {
      if (paraItems[index1] is InlineContentControl paraItem1)
        this.SplitByLineBreak(paraItem1.ParagraphItems);
      if (paraItems[index1] is WTextRange paraItem2 && paraItem2.Text.Contains(ControlChar.LineBreak))
      {
        IWParagraphStyle wparagraphStyle = (IWParagraphStyle) null;
        if (paraItem2.Owner is InlineContentControl && paraItem2.OwnerParagraph != null && paraItem2.OwnerParagraph.ParaStyle != null)
          wparagraphStyle = paraItem2.OwnerParagraph.ParaStyle;
        string[] strArray1 = paraItem2.Text.Split(ControlChar.LineBreakChar);
        paraItem2.Text = strArray1[0];
        int index2 = paraItem2.Index + 1;
        for (int index3 = 1; index3 < strArray1.Length; ++index3)
        {
          Break @break = new Break((IWordDocument) this.Document, BreakType.LineBreak);
          @break.TextRange.Text = ControlChar.LineBreak;
          @break.TextRange.CharacterFormat.ImportContainer((FormatBase) paraItem2.CharacterFormat);
          @break.TextRange.CharacterFormat.CopyProperties((FormatBase) paraItem2.CharacterFormat);
          paraItems.Insert(index2, (IEntity) @break);
          if (wparagraphStyle != null)
            @break.CharacterFormat.ApplyBase((FormatBase) wparagraphStyle.CharacterFormat);
          WTextRange wtextRange1 = paraItem2.Clone() as WTextRange;
          wtextRange1.Text = strArray1[index3];
          string[] strArray2 = wtextRange1.Text.Split(ControlChar.TabChar);
          if (wtextRange1.Text != string.Empty)
          {
            if (strArray1.Length > 1)
              wtextRange1.Text = strArray2[0];
            paraItems.Insert(index2 + 1, (IEntity) wtextRange1);
            if (wparagraphStyle != null)
              wtextRange1.CharacterFormat.ApplyBase((FormatBase) wparagraphStyle.CharacterFormat);
            index2 = paraItems.IndexOf((IEntity) wtextRange1) + 1;
          }
          else
            ++index2;
          for (int index4 = 1; index4 < strArray2.Length; ++index4)
          {
            WTextRange wtextRange2 = paraItem2.Clone() as WTextRange;
            wtextRange2.Text = ControlChar.Tab;
            paraItems.Insert(index2, (IEntity) wtextRange2);
            if (wparagraphStyle != null)
              wtextRange2.CharacterFormat.ApplyBase((FormatBase) wparagraphStyle.CharacterFormat);
            index2 = paraItems.IndexOf((IEntity) wtextRange2) + 1;
            if (strArray2[index4] != string.Empty)
            {
              WTextRange wtextRange3 = paraItem2.Clone() as WTextRange;
              wtextRange3.Text = strArray2[index4];
              paraItems.Insert(index2, (IEntity) wtextRange3);
              if (wparagraphStyle != null)
                wtextRange3.CharacterFormat.ApplyBase((FormatBase) wparagraphStyle.CharacterFormat);
              index2 = paraItems.IndexOf((IEntity) wtextRange3) + 1;
            }
          }
        }
      }
    }
  }

  private Entity GetOwnerBaseEntity(Entity entity)
  {
    Entity ownerBaseEntity = entity;
    while (ownerBaseEntity.Owner != null)
    {
      ownerBaseEntity = ownerBaseEntity.Owner;
      switch (ownerBaseEntity)
      {
        case WSection _:
        case BlockContentControl _:
        case WTextBox _:
        case Shape _:
        case GroupShape _:
          return ownerBaseEntity;
        default:
          continue;
      }
    }
    return ownerBaseEntity;
  }

  internal bool IsFirstParagraphOfOwnerTextBody()
  {
    return !this.IsInCell && this.OwnerTextBody != null && this.IsFirstParagraphOfTextBody(this.GetOwnerBaseEntity((Entity) this), (Entity) this);
  }

  internal bool IsFirstParagraphOfTextBody(Entity ownerBaseEntity, Entity childEntity)
  {
    bool flag = false;
    switch (ownerBaseEntity.EntityType)
    {
      case EntityType.Section:
        flag = (childEntity.Owner as WTextBody).Items[0] == childEntity && this.GetOwnerSection(childEntity).Index == 0;
        break;
      case EntityType.BlockContentControl:
        childEntity = (Entity) (ownerBaseEntity as BlockContentControl);
        ownerBaseEntity = this.GetOwnerBaseEntity(childEntity);
        flag = this.IsFirstParagraphOfTextBody(ownerBaseEntity, childEntity);
        break;
      case EntityType.Shape:
      case EntityType.AutoShape:
        flag = (ownerBaseEntity as Shape).TextBody.Items[0] == childEntity;
        break;
      case EntityType.TextBox:
        flag = (ownerBaseEntity as WTextBox).TextBoxBody.Items[0] == childEntity;
        break;
      case EntityType.ChildShape:
        flag = (ownerBaseEntity as ChildShape).TextBody.Items[0] == childEntity;
        break;
    }
    return flag;
  }

  private bool IsSkipCellMark()
  {
    WTableCell wtableCell = (WTableCell) null;
    if (this.IsInCell)
      wtableCell = this.GetOwnerEntity() as WTableCell;
    if (wtableCell != null && wtableCell.OwnerRow.ContentControl != null && wtableCell.Index == wtableCell.OwnerRow.ChildEntities.Count - 1)
      return false;
    if (wtableCell == null || wtableCell.LastParagraph == null || !wtableCell.LastParagraph.Equals((object) this) || this.PreviousSibling == null)
      return false;
    WSection ownerSection = this.GetOwnerSection((Entity) wtableCell) as WSection;
    float num = ownerSection.PageSetup.PageSize.Height - (ownerSection.PageSetup.HeaderDistance + ownerSection.PageSetup.FooterDistance);
    if (this.PreviousSibling is WTable && (this.PreviousSibling as WTable).Rows.Count > 0 && (double) (this.PreviousSibling as WTable).LastRow.Height < (double) num || this.PreviousSibling is BlockContentControl && (this.PreviousSibling as BlockContentControl).LastChildEntity is WTable && ((this.PreviousSibling as BlockContentControl).LastChildEntity as WTable).Rows.Count > 0 && (double) ((this.PreviousSibling as BlockContentControl).LastChildEntity as WTable).LastRow.Height < (double) num)
      return true;
    if (!wtableCell.CellFormat.HideMark)
      return false;
    return this.ListFormat == null || this.ListFormat.CurrentListLevel == null;
  }

  public float GetHeight(WParagraph paragraph, ParagraphItem paraItem)
  {
    float height = DocumentLayouter.m_dc.MeasureString(" ", paragraph.BreakCharacterFormat.GetFontToRender(FontScriptType.English), (StringFormat) null, paragraph.BreakCharacterFormat, false).Height;
    if (paraItem != null)
    {
      paraItem = paragraph.GetNextSibling((IWidget) paraItem) as ParagraphItem;
      while (true)
      {
        int num;
        switch (paraItem)
        {
          case null:
            goto label_11;
          case WPicture _:
            num = (paraItem as WPicture).TextWrappingStyle == TextWrappingStyle.Inline ? 1 : 0;
            break;
          case Shape _:
            num = (paraItem as Shape).WrapFormat.TextWrappingStyle == TextWrappingStyle.Inline ? 1 : 0;
            break;
          case WTextBox _:
            num = (paraItem as WTextBox).TextBoxFormat.TextWrappingStyle == TextWrappingStyle.Inline ? 1 : 0;
            break;
          case WOleObject _:
            num = (paraItem as WOleObject).OlePicture.TextWrappingStyle == TextWrappingStyle.Inline ? 1 : 0;
            break;
          default:
            num = 1;
            break;
        }
        bool flag = num != 0;
        if (paraItem is BookmarkStart || paraItem is BookmarkEnd || !flag)
          paraItem = paragraph.GetNextSibling((IWidget) paraItem) as ParagraphItem;
        else
          break;
      }
      return paragraph.GetSize(paraItem, height);
    }
label_11:
    return height;
  }

  public float GetSize(ParagraphItem paraItem, float height)
  {
    switch (paraItem)
    {
      case WTextRange _:
        return DocumentLayouter.m_dc.MeasureString(" ", (paraItem as WTextRange).CharacterFormat.GetFontToRender((paraItem as WTextRange).ScriptType), (StringFormat) null, (paraItem as WTextRange).CharacterFormat, false).Height;
      case WSymbol _:
        return DocumentLayouter.m_dc.MeasureString(" ", (paraItem as WSymbol).CharacterFormat.GetFontToRender(FontScriptType.English), (StringFormat) null, (paraItem as WSymbol).CharacterFormat, false).Height;
      case WFootnote _:
        return DocumentLayouter.m_dc.MeasureString(" ", (paraItem as WFootnote).MarkerCharacterFormat.GetFontToRender(FontScriptType.English), (StringFormat) null, (paraItem as WFootnote).MarkerCharacterFormat, false).Height;
      case WPicture _:
        return DocumentLayouter.m_dc.MeasureImage(paraItem as WPicture).Height;
      case WTextBox _:
        return (paraItem as WTextBox).TextBoxFormat.Height;
      case Shape _:
        return (paraItem as Shape).Height;
      case WOleObject _:
        return (paraItem as WOleObject).OlePicture.Height;
      default:
        return height;
    }
  }

  internal bool IsAdjacentParagraphHaveSameBorders(
    WParagraph adjacentParagraph,
    float currentParaLeftIndent)
  {
    return this.IsSameLeftIndent(adjacentParagraph, currentParaLeftIndent) && (double) this.ParagraphFormat.RightIndent == (double) adjacentParagraph.ParagraphFormat.RightIndent && (this.ParagraphFormat.IsFrame && adjacentParagraph.ParagraphFormat.IsFrame && this.ParagraphFormat.IsInSameFrame(adjacentParagraph.ParagraphFormat) || !this.ParagraphFormat.IsFrame && !adjacentParagraph.ParagraphFormat.IsFrame) && this.IsSameAdjacentBorder(this.ParagraphFormat.Borders.Bottom, adjacentParagraph.ParagraphFormat.Borders.Bottom) && this.IsSameAdjacentBorder(this.ParagraphFormat.Borders.Top, adjacentParagraph.ParagraphFormat.Borders.Top) && this.IsSameAdjacentBorder(this.ParagraphFormat.Borders.Right, adjacentParagraph.ParagraphFormat.Borders.Right) && this.IsSameAdjacentBorder(this.ParagraphFormat.Borders.Left, adjacentParagraph.ParagraphFormat.Borders.Left);
  }

  internal bool IsSameAdjacentBorder(Border border, Border adjacentBorder)
  {
    return border.BorderType == adjacentBorder.BorderType && (double) border.LineWidth == (double) adjacentBorder.LineWidth && (double) border.Space == (double) adjacentBorder.Space && border.Color == adjacentBorder.Color;
  }

  internal bool IsSameLeftIndent(WParagraph adjacentParagrph, float firstParaLeftPosition)
  {
    float num1 = (double) adjacentParagrph.ParagraphFormat.FirstLineIndent > 0.0 ? 0.0f : adjacentParagrph.ParagraphFormat.FirstLineIndent;
    float num2 = adjacentParagrph.ParagraphFormat.LeftIndent + num1;
    if (adjacentParagrph.ListFormat.IsEmptyList || this.IsSectionEndMark())
      return (double) firstParaLeftPosition == (double) num2;
    WListFormat listFormatValue = adjacentParagrph.GetListFormatValue();
    WParagraphStyle paraStyle = adjacentParagrph.ParaStyle as WParagraphStyle;
    if (listFormatValue != null && listFormatValue.CurrentListStyle != null)
    {
      ListStyle currentListStyle = listFormatValue.CurrentListStyle;
      WListLevel listLevel = adjacentParagrph.GetListLevel(listFormatValue);
      if (listLevel.ParagraphFormat.HasValue(2))
        num2 = listLevel.ParagraphFormat.LeftIndent;
      if (paraStyle != null && adjacentParagrph.ListFormat.ListType == ListType.NoList && paraStyle.ParagraphFormat.HasValue(2))
        num2 = paraStyle.ParagraphFormat.LeftIndent;
      if (adjacentParagrph.ParagraphFormat.HasValue(2))
        num2 = adjacentParagrph.ParagraphFormat.LeftIndent;
      if (listLevel.ParagraphFormat.HasValue(5))
        num1 = listLevel.ParagraphFormat.FirstLineIndent;
      if (paraStyle != null && adjacentParagrph.ListFormat.ListType == ListType.NoList && paraStyle.ParagraphFormat.HasValue(5))
        num1 = paraStyle.ParagraphFormat.FirstLineIndent;
      if (adjacentParagrph.ParagraphFormat.HasValue(5))
        num1 = adjacentParagrph.ParagraphFormat.FirstLineIndent;
      if ((double) num1 < 0.0 && (double) num2 == 0.0 && !adjacentParagrph.ParagraphFormat.HasValue(2))
        num2 = Math.Abs(num1);
      num2 += num1;
    }
    return (double) firstParaLeftPosition == (double) num2;
  }

  internal bool IsParagraphBeforeSpacingNeedToSkip()
  {
    foreach (ParagraphItem childEntity in (CollectionImpl) this.ChildEntities)
    {
      switch (childEntity)
      {
        case BookmarkStart _:
        case BookmarkEnd _:
          continue;
        case Break _:
          if ((childEntity as Break).BreakType == BreakType.ColumnBreak)
            return true;
          break;
      }
      return false;
    }
    return false;
  }

  internal double GetDefaultTabWidth()
  {
    Entity owner = this.Owner;
    while (!(owner is WSection) && owner.Owner != null)
      owner = owner.Owner;
    return owner is WSection ? (double) (owner as WSection).PageSetup.DefaultTabWidth : 36.0;
  }

  internal bool StartsWithExt(string text, string value) => text.StartsWithExt(value);

  internal void GetMinimumAndMaximumWordWidth(
    ref float minimumWordWidth,
    ref float maximumWordWidth,
    ref float paragraphWidth,
    bool needtoCalculateParaWidth)
  {
    float maximumWordWidthInPara = 0.0f;
    float minumWordWidthInPara = 0.0f;
    (this.ChildEntities as ParagraphItemCollection).GetMinimumAndMaximumWordWidthInPara(ref maximumWordWidthInPara, ref minumWordWidthInPara);
    if ((double) maximumWordWidthInPara > 0.0)
      maximumWordWidthInPara += (float) (((double) this.ParagraphFormat.LeftIndent > 0.0 ? (double) this.ParagraphFormat.LeftIndent : 0.0) + ((double) this.ParagraphFormat.RightIndent > 0.0 ? (double) this.ParagraphFormat.RightIndent : 0.0));
    if ((double) minimumWordWidth == 0.0 || (double) minumWordWidthInPara < (double) minimumWordWidth)
      minimumWordWidth = minumWordWidthInPara;
    if ((double) maximumWordWidth == 0.0 || (double) maximumWordWidthInPara > (double) maximumWordWidth)
      maximumWordWidth = maximumWordWidthInPara;
    if (!needtoCalculateParaWidth)
      return;
    float paragraphWidth1 = this.GetParagraphWidth();
    if ((double) paragraphWidth1 <= (double) paragraphWidth)
      return;
    paragraphWidth = paragraphWidth1;
  }

  internal float GetParagraphWidth()
  {
    float paraItemsWidth = this.GetParaItemsWidth(this.ChildEntities as ParagraphItemCollection);
    if ((double) paraItemsWidth > 0.0)
      paraItemsWidth += (float) (((double) this.ParagraphFormat.LeftIndent > 0.0 ? (double) this.ParagraphFormat.LeftIndent : 0.0) + ((double) this.ParagraphFormat.RightIndent > 0.0 ? (double) this.ParagraphFormat.RightIndent : 0.0));
    return paraItemsWidth;
  }

  private float GetParaItemsWidth(ParagraphItemCollection paraItems)
  {
    float paraItemsWidth = 0.0f;
    DrawingContext drawingContext = DocumentLayouter.DrawingContext;
    for (int index = 0; index < paraItems.Count; ++index)
    {
      Entity entity = (Entity) paraItems[index];
      switch (entity)
      {
        case WTextRange _ when !string.IsNullOrEmpty((entity as WTextRange).Text) && !(entity is WField):
          paraItemsWidth += drawingContext.MeasureString((entity as WTextRange).Text, (entity as WTextRange).CharacterFormat.Font, (StringFormat) null).Width;
          break;
        case WField _:
        case WOleObject _:
          WField wfield = entity is WField ? entity as WField : ((entity as WOleObject).Field != null ? (entity as WOleObject).Field : (WField) null);
          if (wfield != null && wfield.FieldEnd != null)
            entity = wfield.FieldSeparator == null ? (Entity) wfield.FieldEnd : (Entity) wfield.FieldSeparator;
          index = paraItems.IndexOf((IEntity) entity);
          if (index != -1)
            break;
          goto label_10;
        case WPicture _ when (entity as WPicture).TextWrappingStyle != TextWrappingStyle.InFrontOfText && (entity as WPicture).TextWrappingStyle != TextWrappingStyle.Behind:
          paraItemsWidth += (entity as WPicture).Width;
          break;
        case InlineContentControl _ when (entity as InlineContentControl).ParagraphItems.Count > 0:
          paraItemsWidth += this.GetParaItemsWidth((entity as InlineContentControl).ParagraphItems);
          break;
        case Break _ when (entity as Break).BreakType == BreakType.LineBreak:
          goto label_10;
      }
    }
label_10:
    return paraItemsWidth;
  }

  internal class LayoutParagraphInfoImpl : ParagraphLayoutInfo
  {
    private WParagraph m_paragraph;
    private IEntity m_nextSibling;
    private IEntity m_prevSibling;

    protected IWordDocument Document => (IWordDocument) this.m_paragraph.Document;

    public LayoutParagraphInfoImpl(WParagraph paragraph)
      : base(ChildrenLayoutDirection.Horizontal)
    {
      this.IsLineContainer = true;
      this.m_paragraph = paragraph;
      this.m_nextSibling = paragraph.NextSibling;
      this.m_prevSibling = paragraph.PreviousSibling;
      this.IsSectionEndMark = paragraph.SectionEndMark;
      this.Size = this.GetEmptyTextRangeSize();
      this.UpdateLayoutInfoIsClipped();
      this.InitFormat();
      this.InitPageBreaks();
      this.InitListFormat();
      this.InitBorders();
    }

    private SizeF GetEmptyTextRangeSize()
    {
      return DocumentLayouter.DrawingContext.MeasureString(" ", this.m_paragraph.BreakCharacterFormat.GetFontToRender(FontScriptType.English), (StringFormat) null, this.m_paragraph.BreakCharacterFormat, false);
    }

    private void UpdateLayoutInfoIsClipped()
    {
      if (this.m_paragraph.IsInCell)
      {
        WTableCell ownerEntity = this.m_paragraph.GetOwnerEntity() as WTableCell;
        float rowHeight = (ownerEntity.Owner as WTableRow).Height;
        if (this.m_paragraph.IsExactlyRowHeight(ownerEntity, ref rowHeight))
        {
          if ((double) rowHeight < 0.0)
            rowHeight = -rowHeight;
          if ((double) rowHeight > 1.0)
            this.IsClipped = true;
        }
        if (ownerEntity.CellFormat.TextDirection == TextDirection.VerticalBottomToTop || ownerEntity.CellFormat.TextDirection == TextDirection.VerticalTopToBottom)
          this.IsClipped = true;
        WParagraph paragraphInOwnerTextbody = this.m_paragraph.GetFirstParagraphInOwnerTextbody(this.m_paragraph.OwnerTextBody);
        if (paragraphInOwnerTextbody == null || !paragraphInOwnerTextbody.ParagraphFormat.IsFrame || (double) paragraphInOwnerTextbody.ParagraphFormat.FrameHeight == 0.0 || ((int) (ushort) ((double) paragraphInOwnerTextbody.ParagraphFormat.FrameHeight * 20.0) & 32768 /*0x8000*/) != 0)
          return;
        this.IsClipped = true;
      }
      else if (this.m_paragraph != null && this.m_paragraph.Owner != null && (this.GetBaseEntity((Entity) this.m_paragraph) is WTextBox || this.GetBaseEntity((Entity) this.m_paragraph) is Shape || this.GetBaseEntity((Entity) this.m_paragraph) is ChildShape))
      {
        this.IsClipped = true;
      }
      else
      {
        if (this.m_paragraph == null || !this.m_paragraph.ParagraphFormat.IsFrame || (double) this.m_paragraph.ParagraphFormat.FrameHeight == 0.0 || ((int) (ushort) ((double) this.m_paragraph.ParagraphFormat.FrameHeight * 20.0) & 32768 /*0x8000*/) != 0)
          return;
        this.IsClipped = true;
      }
    }

    private Entity GetBaseEntity(Entity entity)
    {
      if (entity == null)
        return (Entity) null;
      Entity baseEntity = entity;
      while (true)
      {
        switch (baseEntity)
        {
          case WSection _:
          case WTextBox _:
          case Shape _:
          case ChildShape _:
            goto label_6;
          default:
            if (baseEntity.Owner != null)
            {
              baseEntity = baseEntity.Owner;
              continue;
            }
            goto label_6;
        }
      }
label_6:
      return baseEntity;
    }

    private void InitListFormat()
    {
      if (this.m_paragraph.ListFormat.IsEmptyList || this.IsSectionEndMark)
        return;
      int tabLevelIndex = 0;
      if (this.m_paragraph.ParagraphFormat.Tabs.Count > 0)
        ++tabLevelIndex;
      WListLevel listLevel = (WListLevel) null;
      float? leftIndent = new float?();
      float? firstLineIndent = new float?();
      WListFormat listFormat = this.m_paragraph.GetListFormat(ref listLevel, ref tabLevelIndex, ref leftIndent, ref firstLineIndent);
      WParagraphStyle paraStyle = this.m_paragraph.ParaStyle as WParagraphStyle;
      if (listFormat == null || listLevel == null)
        return;
      ListStyle currentListStyle = listFormat.CurrentListStyle;
      this.LevelNumber = listLevel.LevelNumber;
      if (currentListStyle.ListType == ListType.Numbered || currentListStyle.ListType == ListType.Bulleted)
      {
        if (leftIndent.HasValue)
          this.Margins.Left = leftIndent.Value;
        if (firstLineIndent.HasValue)
          this.FirstLineIndent = firstLineIndent.Value;
        if ((double) this.FirstLineIndent < 0.0 && (double) this.Margins.Left == 0.0 && !this.m_paragraph.ParagraphFormat.HasValue(2) && !listLevel.ParagraphFormat.HasValue(2) && paraStyle != null && !paraStyle.ParagraphFormat.HasValue(2))
          this.Margins.Left = Math.Abs(this.FirstLineIndent);
      }
      this.ListValue = (this.Document as WordDocument).UpdateListValue(this.m_paragraph, listFormat, listLevel);
      this.CurrentListType = listFormat.ListType;
      if (listLevel.PatternType == ListPatternType.Bullet)
        this.CurrentListType = ListType.Bulleted;
      else if (listFormat.ListType == ListType.Bulleted && listLevel.PatternType != ListPatternType.Bullet)
        this.CurrentListType = ListType.Numbered;
      this.CharacterFormat = new WCharacterFormat(this.Document);
      this.CharacterFormat.ImportContainer((FormatBase) this.m_paragraph.BreakCharacterFormat);
      this.CharacterFormat.CopyProperties((FormatBase) this.m_paragraph.BreakCharacterFormat);
      this.CharacterFormat.ApplyBase(this.m_paragraph.BreakCharacterFormat.BaseFormat);
      if (this.CharacterFormat.PropertiesHash.ContainsKey(7))
      {
        this.CharacterFormat.UnderlineStyle = UnderlineStyle.None;
        this.CharacterFormat.PropertiesHash.Remove(7);
      }
      else if (this.CharacterFormat.CharStyle != null && this.CharacterFormat.CharStyle.CharacterFormat.PropertiesHash.ContainsKey(7))
        this.CharacterFormat.UnderlineStyle = UnderlineStyle.None;
      this.CopyCharacterFormatting(listLevel.CharacterFormat, this.CharacterFormat);
      if (this.CurrentListType == ListType.Numbered && this.m_paragraph.ParagraphFormat.Bidi && (this.m_paragraph.BreakCharacterFormat.LocaleIdBidi == (short) 1037 || this.m_paragraph.BreakCharacterFormat.LocaleIdBidi == (short) 1085))
        this.CharacterFormat.FontName = this.CharacterFormat.GetFontNameBidiToRender(FontScriptType.English);
      this.ListFont = new SyncFont(this.CharacterFormat.GetFontToRender(FontScriptType.English));
      if (listLevel.FollowCharacter == FollowCharacterType.Tab)
        this.UpdateListTab(listLevel, tabLevelIndex);
      else
        this.UpdateListWidth(listLevel);
      this.ListAlignment = listLevel.NumberAlignment;
      if (this.m_paragraph.Document.DOP.Dop2000.Copts.DontUseHTMLParagraphAutoSpacing)
        return;
      this.UpdateListParagraphSpacing(listFormat);
    }

    private void UpdateListParagraphSpacing(WListFormat currentListFormat)
    {
      if (this.m_nextSibling != null && this.m_nextSibling is WParagraph && this.m_paragraph.ParagraphFormat.SpaceAfterAuto)
      {
        WParagraph nextSibling = this.m_nextSibling as WParagraph;
        IWParagraphStyle paraStyle = nextSibling.ParaStyle;
        WListFormat listFormatValue = nextSibling.GetListFormatValue();
        if (!nextSibling.ListFormat.IsEmptyList && listFormatValue != null && listFormatValue.CurrentListStyle != null && listFormatValue.CurrentListStyle == currentListFormat.CurrentListStyle)
          this.Margins.Bottom = 0.0f;
      }
      if (this.m_prevSibling == null || !(this.m_prevSibling is WParagraph) || !this.m_paragraph.ParagraphFormat.SpaceBeforeAuto)
        return;
      WParagraph prevSibling = this.m_prevSibling as WParagraph;
      IWParagraphStyle paraStyle1 = prevSibling.ParaStyle;
      WListFormat listFormatValue1 = prevSibling.GetListFormatValue();
      if (prevSibling.ListFormat.IsEmptyList || listFormatValue1 == null || listFormatValue1.CurrentListStyle == null || listFormatValue1.CurrentListStyle != currentListFormat.CurrentListStyle)
        return;
      this.Margins.Top = 0.0f;
    }

    private SizeF GetListValueSize(WListLevel level)
    {
      DrawingContext drawingContext = DocumentLayouter.DrawingContext;
      Font font = this.ListFont.GetFont(level.Document);
      return this.CurrentListType == ListType.Bulleted && level != null && level.PicBullet != null ? drawingContext.MeasurePictureBulletSize(level.PicBullet, font) : drawingContext.MeasureString(this.ListValue, font, (StringFormat) null, this.CharacterFormat, false);
    }

    private void UpdateListWidth(WListLevel level)
    {
      DrawingContext drawingContext = DocumentLayouter.DrawingContext;
      SizeF listValueSize = this.GetListValueSize(level);
      float num = 0.0f;
      if (level.NumberAlignment == ListNumberAlignment.Left)
        num = listValueSize.Width;
      else if (level.NumberAlignment == ListNumberAlignment.Center)
        num = listValueSize.Width / 2f;
      if (level.FollowCharacter == FollowCharacterType.Space)
        this.ListTab = num + drawingContext.MeasureString(" ", this.ListFont.GetFont(level.Document), (StringFormat) null).Width;
      else
        this.ListTab = num;
    }

    private void UpdateListTab(WListLevel level, int tabLevelIndex)
    {
      WParagraph.ListTabs tabs = new WParagraph.ListTabs(this.m_paragraph);
      tabs.SortParagraphTabsCollection(this.m_paragraph.ParagraphFormat, level.ParagraphFormat.Tabs, tabLevelIndex);
      SizeF listValueSize = this.GetListValueSize(level);
      if (level.NumberAlignment == ListNumberAlignment.Left)
        this.UpdateTabWidth(level, tabs, listValueSize.Width);
      else if (level.NumberAlignment == ListNumberAlignment.Center)
        this.UpdateTabWidth(level, tabs, listValueSize.Width / 2f);
      else
        this.UpdateTabWidth(level, tabs, 0.0f);
      this.ListTabStop = tabs.m_currTab;
    }

    private void UpdateTabWidth(WListLevel level, WParagraph.ListTabs tabs, float width)
    {
      if ((double) this.ListTab > (double) width && (this.Document as WordDocument).UseHangingIndentAsListTab)
        return;
      float position = width + this.Margins.Left + this.FirstLineIndent;
      float nextTabPosition = (float) tabs.GetNextTabPosition((double) position);
      if ((double) this.FirstLineIndent < 0.0 && (double) width <= (double) Math.Abs(this.FirstLineIndent) && (this.Document as WordDocument).UseHangingIndentAsListTab)
      {
        if ((double) tabs.m_currTab.Position != 0.0)
        {
          this.ListTab = Math.Min(width + nextTabPosition, Math.Abs(this.FirstLineIndent));
          if ((double) Math.Abs(this.FirstLineIndent) < (double) tabs.m_currTab.Position)
            tabs.m_currTab.TabLeader = Syncfusion.Layouting.TabLeader.NoLeader;
        }
        else
          this.ListTab = Math.Abs(this.FirstLineIndent);
      }
      else if (level.Word6Legacy)
      {
        float val1 = (float) (level.LegacyIndent / 20);
        if ((double) width + (double) (level.LegacySpace / 20) > (double) val1)
          val1 = width + (float) (level.LegacySpace / 20);
        this.ListTab = Math.Max(val1, (double) this.FirstLineIndent < 0.0 ? Math.Abs(this.FirstLineIndent) : 0.0f);
      }
      else if ((double) this.FirstLineIndent < 0.0 && (double) tabs.m_currTab.Position == 0.0 && (double) width <= (double) Math.Abs(this.FirstLineIndent))
        this.ListTab = Math.Min(width + nextTabPosition, Math.Abs(this.FirstLineIndent));
      else
        this.ListTab = width + nextTabPosition;
      if ((double) width == 0.0 && (double) this.ListTab == 0.0 && (double) this.FirstLineIndent == 0.0)
        this.ListTab = nextTabPosition;
      this.ListTabWidth = nextTabPosition;
    }

    private void CopyCharacterFormatting(WCharacterFormat sourceFormat, WCharacterFormat destFormat)
    {
      if (sourceFormat.HasValue(6))
        destFormat.Strikeout = sourceFormat.Strikeout;
      if (sourceFormat.HasValue(90))
        destFormat.UnderlineColor = sourceFormat.UnderlineColor;
      if (sourceFormat.HasValue(3))
        destFormat.SetPropertyValue(3, (object) sourceFormat.FontSize);
      if (sourceFormat.HasValue(1))
        destFormat.TextColor = sourceFormat.TextColor;
      if (sourceFormat.HasValue(2))
        destFormat.FontName = sourceFormat.FontName;
      if (sourceFormat.HasValue(4))
        destFormat.Bold = sourceFormat.Bold;
      if (sourceFormat.HasValue(5))
        destFormat.Italic = sourceFormat.Italic;
      if (sourceFormat.HasValue(7))
        destFormat.UnderlineStyle = sourceFormat.UnderlineStyle;
      if (sourceFormat.HasValue(63 /*0x3F*/))
        destFormat.HighlightColor = sourceFormat.HighlightColor;
      if (sourceFormat.HasValue(50))
        destFormat.Shadow = sourceFormat.Shadow;
      if (sourceFormat.HasValue(18))
        destFormat.SetPropertyValue(18, (object) sourceFormat.CharacterSpacing);
      if (sourceFormat.HasValue(14))
        destFormat.DoubleStrike = sourceFormat.DoubleStrike;
      if (sourceFormat.HasValue(51))
        destFormat.Emboss = sourceFormat.Emboss;
      if (sourceFormat.HasValue(52))
        destFormat.Engrave = sourceFormat.Engrave;
      if (sourceFormat.HasValue(10))
        destFormat.SubSuperScript = sourceFormat.SubSuperScript;
      destFormat.TextBackgroundColor = sourceFormat.TextBackgroundColor;
      if (sourceFormat.HasValue(54))
        destFormat.AllCaps = sourceFormat.AllCaps;
      if (sourceFormat.Bidi)
        destFormat.Bidi = true;
      else if (!sourceFormat.Bidi && destFormat.Bidi)
        destFormat.Bidi = false;
      if (sourceFormat.HasValue(59))
        destFormat.BoldBidi = sourceFormat.BoldBidi;
      if (sourceFormat.HasValue(60))
        destFormat.ItalicBidi = sourceFormat.ItalicBidi;
      if (sourceFormat.HasValue(61))
        destFormat.FontNameBidi = sourceFormat.FontNameBidi;
      if (sourceFormat.HasValue(62))
        destFormat.SetPropertyValue(62, (object) sourceFormat.FontSizeBidi);
      if (sourceFormat.HasValue(109))
        destFormat.FieldVanish = sourceFormat.FieldVanish;
      if (sourceFormat.HasValue(53))
        destFormat.Hidden = sourceFormat.Hidden;
      if (sourceFormat.HasValue(24))
        destFormat.SpecVanish = sourceFormat.SpecVanish;
      if (sourceFormat.HasValue(55))
        destFormat.SmallCaps = sourceFormat.SmallCaps;
      if (sourceFormat.HasValue(72))
        destFormat.IdctHint = sourceFormat.IdctHint;
      if (sourceFormat.HasValue((int) sbyte.MaxValue))
        destFormat.Scaling = sourceFormat.Scaling;
      if (sourceFormat.HasValue(0))
        destFormat.Font = sourceFormat.Font;
      if (sourceFormat.HasValue(17))
        destFormat.Position = sourceFormat.Position;
      if (sourceFormat.HasValue(20))
        destFormat.LineBreak = sourceFormat.LineBreak;
      if (sourceFormat.HasValue(68))
        destFormat.FontNameAscii = sourceFormat.FontNameAscii;
      if (sourceFormat.HasValue(69))
        destFormat.FontNameFarEast = sourceFormat.FontNameFarEast;
      if (sourceFormat.HasValue(70))
        destFormat.FontNameNonFarEast = sourceFormat.FontNameNonFarEast;
      if (sourceFormat.HasValue(71))
        destFormat.OutLine = sourceFormat.OutLine;
      if (sourceFormat.HasValue(73))
        destFormat.LocaleIdASCII = sourceFormat.LocaleIdASCII;
      if (sourceFormat.HasValue(74))
        destFormat.LocaleIdFarEast = sourceFormat.LocaleIdFarEast;
      if (sourceFormat.HasValue(75))
        destFormat.LocaleIdBidi = sourceFormat.LocaleIdBidi;
      if (sourceFormat.HasValue(76))
        destFormat.NoProof = sourceFormat.NoProof;
      if (sourceFormat.HasValue(78))
        destFormat.TextureStyle = sourceFormat.TextureStyle;
      if (sourceFormat.HasValue(77))
        destFormat.ForeColor = sourceFormat.ForeColor;
      if (sourceFormat.HasValue(79))
        destFormat.EmphasisType = sourceFormat.EmphasisType;
      if (sourceFormat.HasValue(80 /*0x50*/))
        destFormat.TextEffect = sourceFormat.TextEffect;
      if (sourceFormat.HasValue(81))
        destFormat.SnapToGrid = sourceFormat.SnapToGrid;
      if (sourceFormat.HasValue(91))
        destFormat.CharStyleName = sourceFormat.CharStyleName;
      if (sourceFormat.HasValue(92))
        destFormat.WebHidden = sourceFormat.WebHidden;
      if (sourceFormat.HasValue(99))
        destFormat.ComplexScript = sourceFormat.ComplexScript;
      if (sourceFormat.HasValue(103))
        destFormat.IsInsertRevision = sourceFormat.IsInsertRevision;
      if (sourceFormat.HasValue(104))
        destFormat.IsDeleteRevision = sourceFormat.IsDeleteRevision;
      if (sourceFormat.HasValue(105))
        destFormat.IsChangedFormat = sourceFormat.IsChangedFormat;
      if (sourceFormat.HasValue(106))
        destFormat.Special = sourceFormat.Special;
      if (sourceFormat.HasValue(107))
        destFormat.ListPictureIndex = sourceFormat.ListPictureIndex;
      if (sourceFormat.HasValue(108))
        destFormat.ListHasPicture = sourceFormat.ListHasPicture;
      if (sourceFormat.HasValue(120))
        destFormat.UseContextualAlternates = sourceFormat.UseContextualAlternates;
      if (sourceFormat.HasValue(121))
        destFormat.Ligatures = sourceFormat.Ligatures;
      if (sourceFormat.HasValue(122))
        destFormat.NumberForm = sourceFormat.NumberForm;
      if (sourceFormat.HasValue(123))
        destFormat.NumberSpacing = sourceFormat.NumberSpacing;
      if (sourceFormat.HasValue(124))
        destFormat.StylisticSet = sourceFormat.StylisticSet;
      if (sourceFormat.HasValue(125))
        destFormat.Kern = sourceFormat.Kern;
      if (sourceFormat.HasValue(126))
        destFormat.BreakClear = sourceFormat.BreakClear;
      if (sourceFormat.HasValue(8))
        destFormat.AuthorName = sourceFormat.AuthorName;
      if (sourceFormat.HasValue(12))
        destFormat.FormatChangeAuthorName = sourceFormat.FormatChangeAuthorName;
      if (sourceFormat.HasValue(15))
        destFormat.FormatChangeDateTime = sourceFormat.FormatChangeDateTime;
      if (!sourceFormat.HasValue(128 /*0x80*/))
        return;
      destFormat.RevisionName = sourceFormat.RevisionName;
    }

    private void InitBorders()
    {
      Borders borders = this.m_paragraph.ParagraphFormat.Borders;
      if (!borders.NoBorder && !this.IsSectionEndMark || this.m_paragraph.ParagraphFormat.HasBorder())
      {
        if (borders.Left.BorderType != BorderStyle.None)
          this.Paddings.Left -= borders.Left.Space + borders.Left.GetLineWidthValue() / 2f;
        else
          this.SkipLeftBorder = true;
        if (borders.Right.BorderType != BorderStyle.None)
          this.Paddings.Right -= borders.Right.Space + borders.Right.GetLineWidthValue() / 2f;
        else
          this.SkipRightBorder = true;
        if (borders.Top.BorderType != BorderStyle.None)
          this.Paddings.Top += borders.Top.Space + borders.Top.GetLineWidthValue();
        else
          this.SkipTopBorder = true;
        if (borders.Bottom.BorderType != BorderStyle.None)
          this.Paddings.Bottom += borders.Bottom.Space + borders.Bottom.GetLineWidthValue();
        else
          this.SkipBottomBorder = true;
        if (this.m_prevSibling is WParagraph prevSibling && prevSibling.ParagraphFormat.Borders.Horizontal.IsBorderDefined)
        {
          if (!this.m_paragraph.IsAdjacentParagraphHaveSameBorders(prevSibling, this.Margins.Left + ((double) this.FirstLineIndent > 0.0 ? 0.0f : this.FirstLineIndent)))
          {
            this.SkipHorizonatalBorder = true;
          }
          else
          {
            this.SkipTopBorder = true;
            this.Paddings.Top = prevSibling.ParagraphFormat.Borders.Horizontal.Space + prevSibling.ParagraphFormat.Borders.Horizontal.GetLineWidthValue();
          }
        }
        else
          this.SkipHorizonatalBorder = true;
        WParagraph adjacentParagraph = this.m_nextSibling as WParagraph;
        if (this.m_paragraph.IsEndOfSection && !this.m_paragraph.IsEndOfDocument)
          adjacentParagraph = (this.m_paragraph.GetOwnerSection().NextSibling as WSection).GetFirstParagraph();
        if (adjacentParagraph != null && adjacentParagraph.ParagraphFormat.HasBorder() && this.m_paragraph.IsAdjacentParagraphHaveSameBorders(adjacentParagraph, this.Margins.Left + ((double) this.FirstLineIndent > 0.0 ? 0.0f : this.FirstLineIndent)))
        {
          this.SkipBottomBorder = true;
          this.Paddings.Bottom = borders.Horizontal.Space;
        }
        if (!this.SkipTopBorder && this.m_prevSibling != null && this.m_prevSibling is WParagraph && !((WParagraph) this.m_prevSibling).ParagraphFormat.Borders.NoBorder && !((WParagraph) this.m_prevSibling).SectionEndMark && this.m_paragraph.IsAdjacentParagraphHaveSameBorders((WParagraph) this.m_prevSibling, this.Margins.Left + ((double) this.FirstLineIndent > 0.0 ? 0.0f : this.FirstLineIndent)))
        {
          this.Paddings.Top = 0.0f;
          this.SkipTopBorder = true;
        }
        if (this.SkipBottomBorder || this.m_nextSibling == null || !(this.m_nextSibling is WParagraph) || ((WParagraph) this.m_nextSibling).ParagraphFormat.Borders.NoBorder || !this.m_paragraph.IsAdjacentParagraphHaveSameBorders((WParagraph) this.m_nextSibling, this.Margins.Left + ((double) this.FirstLineIndent > 0.0 ? 0.0f : this.FirstLineIndent)))
          return;
        this.Paddings.Bottom = 0.0f;
        this.SkipBottomBorder = true;
      }
      else
        this.SkipBottomBorder = this.SkipLeftBorder = this.SkipRightBorder = this.SkipTopBorder = this.SkipHorizonatalBorder = true;
    }

    private void InitPageBreaks()
    {
      IEntity owner = (IEntity) this.m_paragraph.Owner;
      while (owner != null && owner.EntityType != EntityType.TextBody)
        owner = (IEntity) owner.Owner;
      WTextBody wtextBody = owner as WTextBody;
      bool flag1 = false;
      bool flag2 = false;
      WParagraph wparagraph = (WParagraph) null;
      bool flag3 = this.m_paragraph.Text.Contains(ControlChar.LineFeed) || this.m_paragraph.Text.Contains(ControlChar.CarriegeReturn) || this.m_paragraph.Text.Contains(ControlChar.CrLf);
      if (wtextBody != null)
      {
        bool flag4 = this.IsLastParagraphInTextBody();
        IWSection wsection = (IWSection) null;
        WSection ownerSection = this.m_paragraph.GetOwnerSection();
        if (ownerSection != null)
        {
          int num = this.Document.Sections.IndexOf((IWSection) ownerSection);
          wsection = num + 1 < this.m_paragraph.Document.Sections.Count ? (IWSection) this.Document.Sections[num + 1] : (IWSection) null;
        }
        if (wsection != null)
          flag1 = flag4 && (wsection.BreakCode == SectionBreakCode.NewPage || wsection.BreakCode == SectionBreakCode.NewColumn || wsection.BreakCode == SectionBreakCode.Oddpage || wsection.BreakCode == SectionBreakCode.EvenPage || wsection.BreakCode == SectionBreakCode.NoBreak);
        if (!flag4 && (this.m_paragraph.OwnerTextBody.Owner is WSection || this.m_paragraph.OwnerTextBody.Owner is BlockContentControl))
        {
          if (this.m_nextSibling is WTable && (this.m_nextSibling as WTable).Rows.Count > 0 && (this.m_nextSibling as WTable).Rows[0].Cells.Count > 0 && (this.m_nextSibling as WTable).Rows[0].Cells[0].Paragraphs.Count > 0)
            wparagraph = (this.m_nextSibling as WTable).Rows[0].Cells[0].Paragraphs[0];
          else if (this.m_nextSibling is WParagraph)
            wparagraph = this.m_nextSibling as WParagraph;
          if (wparagraph != null && !wparagraph.IsSectionEndMark())
            flag2 = wparagraph.ParagraphFormat.PageBreakBefore;
        }
      }
      WParagraphFormat paragraphFormat = this.m_paragraph.ParagraphFormat;
      this.IsPageBreak = !flag3 && (paragraphFormat.PageBreakAfter || flag2 || paragraphFormat.ColumnBreakAfter || flag1 && this.m_paragraph.IsParagraphHasSectionBreak());
      if (!flag2)
        return;
      DocumentLayouter.IsEndPage = true;
    }

    private bool IsLastParagraphInTextBody()
    {
      entity = (Entity) this.m_paragraph;
      while (!(entity.NextSibling is WParagraph))
      {
        if (!(entity.NextSibling is Entity entity))
          return true;
      }
      return false;
    }

    private void InitFormat()
    {
      WParagraphFormat paragraphFormat = this.m_paragraph.ParagraphFormat;
      WSection ownerSection = this.m_paragraph.GetOwnerSection();
      if (!(this.m_paragraph.GetStyle() is WParagraphStyle))
      {
        WParagraphStyle byName = this.m_paragraph.Document.Styles.FindByName("Normal") as WParagraphStyle;
      }
      this.Margins.Left = paragraphFormat.LeftIndent;
      if (ownerSection != null && ownerSection.PageSetup != null && (double) paragraphFormat.RightIndent > (double) ownerSection.PageSetup.ClientWidth)
        this.Margins.Right = 0.0f;
      else
        this.Margins.Right = paragraphFormat.RightIndent;
      WTableCell wtableCell = (WTableCell) null;
      if (this.m_paragraph.IsInCell)
        wtableCell = this.m_paragraph.GetOwnerEntity() as WTableCell;
      this.Margins.Top = (float) paragraphFormat.GetParagraphFormat(8);
      this.Margins.Bottom = (float) paragraphFormat.GetParagraphFormat(9);
      if (!this.m_paragraph.Document.DOP.Dop2000.Copts.DontUseHTMLParagraphAutoSpacing)
        this.UpdateParagraphSpacing();
      if (this.m_paragraph.IsInCell && !this.m_paragraph.Document.DOP.Dop2000.Copts.DontUseHTMLParagraphAutoSpacing)
      {
        if (this.m_nextSibling == null && this.m_paragraph.ParagraphFormat.SpaceAfterAuto)
        {
          WListFormat listFormatValue = this.m_paragraph.GetListFormatValue();
          if (listFormatValue == null || listFormatValue.ListType == ListType.NoList)
            this.Margins.Bottom = 0.0f;
          else
            this.Margins.Bottom = 14f;
        }
        if (this.m_prevSibling == null && this.m_paragraph.ParagraphFormat.SpaceBeforeAuto)
          this.Margins.Top = 0.0f;
      }
      if (paragraphFormat.ContextualSpacing && (this.m_paragraph.IsInCell ? (!this.m_paragraph.Document.DOP.Dop2000.Copts.AllowSpaceOfSameStyleInTable ? 1 : (this.m_paragraph.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013 ? 1 : 0)) : 1) != 0)
      {
        if (this.m_paragraph.IsInCell)
        {
          if (this.m_paragraph.Index == 0)
            this.UpdateFirstParagraphMarginsBasedOnContextualSpacing();
          else if (this.m_paragraph.Index == wtableCell.ChildEntities.Count - 1)
            this.UpdateLastParagraphMarginsBasedOnContextualSpacing();
        }
        IEntity prevSibling = this.GetPrevSibling();
        IEntity nextSibling = this.GetNextSibling();
        if (this.IsSameStyle(this.m_paragraph, prevSibling as WParagraph))
          this.Margins.Top = 0.0f;
        if (this.IsSameStyle(this.m_paragraph, nextSibling as WParagraph))
          this.Margins.Bottom = 0.0f;
        WTextBody ownerTextBody = this.m_paragraph.OwnerTextBody;
        if (ownerTextBody != null && (this.m_paragraph.Index != 0 && this.m_paragraph.Index != ownerTextBody.ChildEntities.Count - 1 || ownerTextBody.Owner is BlockContentControl))
          this.UpdateParaMarginsBasedOnContextualSpacing(prevSibling, nextSibling);
      }
      this.IsKeepTogether = paragraphFormat.Keep;
      this.IsKeepWithNext = paragraphFormat.KeepFollow;
      this.FirstLineIndent = paragraphFormat.FirstLineIndent;
      this.Justification = (HAlignment) paragraphFormat.GetAlignmentToRender();
      if (!this.IsSectionEndMark)
        return;
      if (this.UpdateSectionBreakSpacing())
      {
        this.Margins.Bottom = this.m_paragraph.ParagraphFormat.AfterSpacing + this.Size.Height;
        this.Margins.Top = this.m_paragraph.ParagraphFormat.BeforeSpacing;
      }
      else
      {
        this.Margins.Bottom = 0.0f;
        this.Margins.Top = 0.0f;
      }
    }

    internal bool UpdateSectionBreakSpacing()
    {
      return (this.m_prevSibling is WTable || this.m_prevSibling is WParagraph && (this.m_prevSibling as WParagraph).BreakCharacterFormat.Hidden) && this.m_paragraph.Document.Settings.CompatibilityMode == CompatibilityMode.Word2013;
    }

    private void UpdateFirstParagraphMarginsBasedOnContextualSpacing()
    {
      WTableCell ownerEntity = this.m_paragraph.GetOwnerEntity() as WTableCell;
      WTableRow ownerRow = ownerEntity.OwnerRow;
      WTable ownerTable = ownerRow.OwnerTable;
      IEntity paragraphNextsibling = this.m_paragraph.NextSibling == null ? (ownerEntity.NextSibling != null ? (IEntity) (ownerEntity.NextSibling as WTableCell).Items[0] : (IEntity) null) : this.m_paragraph.NextSibling;
      IEntity nextOrPrevSibling1 = this.m_paragraph.PreviousSibling == null ? (ownerEntity.PreviousSibling != null ? (IEntity) (ownerEntity.PreviousSibling as WTableCell).Items[0] : (IEntity) null) : this.m_paragraph.PreviousSibling;
      if (ownerEntity.Index == 0 && this.m_paragraph.Index == 0)
      {
        if (ownerRow.Index == 0)
        {
          if (ownerTable.IsInCell && ownerTable.Index == 0)
          {
            this.CheckOwnerTablePrevItem(ownerTable);
          }
          else
          {
            IEntity nextOrPrevSibling2 = ownerTable.PreviousSibling;
            if (nextOrPrevSibling2 is BlockContentControl)
              nextOrPrevSibling2 = (IEntity) (nextOrPrevSibling2 as BlockContentControl).GetLastParagraphOfSDTContent();
            if (this.IsSameStyle(this.m_paragraph, nextOrPrevSibling2 as WParagraph) || nextOrPrevSibling2 == null && this.m_paragraph.StyleName == "Normal" && !this.IsParaHaveNumberingOrBullets())
              this.Margins.Top = 0.0f;
            this.UpdateAfterSpacing(paragraphNextsibling as WParagraph);
          }
        }
        else
        {
          if (this.IsSameStyle(this.m_paragraph, nextOrPrevSibling1 as WParagraph) || nextOrPrevSibling1 == null && this.m_paragraph.StyleName == "Normal" && !this.IsParaHaveNumberingOrBullets())
            this.Margins.Top = 0.0f;
          this.UpdateAfterSpacing(paragraphNextsibling as WParagraph);
        }
      }
      else
      {
        if (this.m_paragraph.Index != 0)
          return;
        WTableCell childEntity = ownerRow.ChildEntities[ownerEntity.Index - 1] as WTableCell;
        IEntity nextOrPrevSibling3 = (IEntity) childEntity.ChildEntities[childEntity.ChildEntities.Count - 1];
        if (nextOrPrevSibling3 is BlockContentControl)
          nextOrPrevSibling3 = (IEntity) (nextOrPrevSibling3 as BlockContentControl).GetLastParagraphOfSDTContent();
        if (nextOrPrevSibling3 is WTable && this.m_paragraph.StyleName == "Normal" && !this.IsParaHaveNumberingOrBullets())
        {
          this.Margins.Top = 0.0f;
          this.Margins.Bottom = 0.0f;
        }
        else
        {
          if (this.IsSameStyle(this.m_paragraph, nextOrPrevSibling3 as WParagraph))
            this.Margins.Top = 0.0f;
          this.UpdateAfterSpacing(paragraphNextsibling as WParagraph);
        }
      }
    }

    private void UpdateLastParagraphMarginsBasedOnContextualSpacing()
    {
      WTableCell ownerEntity = this.m_paragraph.GetOwnerEntity() as WTableCell;
      WTableRow ownerRow = ownerEntity.OwnerRow;
      if (ownerEntity.Index == ownerRow.Cells.Count - 1 && this.m_paragraph.Index == ownerEntity.ChildEntities.Count - 1)
      {
        if (!(this.m_paragraph.StyleName == "Normal") || this.IsParaHaveNumberingOrBullets())
          return;
        this.Margins.Bottom = 0.0f;
      }
      else
      {
        if (this.m_paragraph.Index != ownerEntity.ChildEntities.Count - 1)
          return;
        IEntity nextOrPrevSibling = (IEntity) (ownerRow.ChildEntities[ownerEntity.Index + 1] as WTableCell).ChildEntities[0];
        while (nextOrPrevSibling is WTable)
          nextOrPrevSibling = (IEntity) (nextOrPrevSibling as WTable).Rows[0].Cells[0].ChildEntities[0];
        if (nextOrPrevSibling is BlockContentControl)
          nextOrPrevSibling = (IEntity) (nextOrPrevSibling as BlockContentControl).GetFirstParagraphOfSDTContent();
        if (!this.IsSameStyle(this.m_paragraph, nextOrPrevSibling as WParagraph))
          return;
        this.Margins.Bottom = 0.0f;
      }
    }

    private bool IsParaHaveNumberingOrBullets()
    {
      WListFormat listFormatValue = this.m_paragraph.GetListFormatValue();
      return listFormatValue != null && listFormatValue.ListType != ListType.NoList;
    }

    private void UpdateParaMarginsBasedOnContextualSpacing(IEntity prevSibling, IEntity nextSibling)
    {
      if (prevSibling is WTable && this.m_paragraph.StyleName == "Normal" && !this.IsParaHaveNumberingOrBullets())
        this.Margins.Top = 0.0f;
      if (!(nextSibling is WTable))
        return;
      WParagraph innerTableFirstPara = this.GetInnerTableFirstPara(nextSibling as WTable);
      if ((innerTableFirstPara != null || !(this.m_paragraph.StyleName == "Normal")) && !this.IsSameStyle(this.m_paragraph, innerTableFirstPara))
        return;
      this.Margins.Bottom = 0.0f;
    }

    private WParagraph GetInnerTableFirstPara(WTable table)
    {
      if (table.Rows.Count == 0 || table.Rows[0].Cells.Count == 0 || table.Rows[0].Cells[0].ChildEntities.Count == 0)
        return (WParagraph) null;
      IEntity table1 = (IEntity) table.Rows[0].Cells[0].ChildEntities[0];
      if (table1 is WTable)
        table1 = (IEntity) this.GetInnerTableFirstPara(table1 as WTable);
      if (table1 is BlockContentControl)
        table1 = (IEntity) (table1 as BlockContentControl).GetFirstParagraphOfSDTContent();
      return table1 as WParagraph;
    }

    private void CheckOwnerTablePrevItem(WTable ownerTable)
    {
      WTableRow ownerRow = ownerTable.GetOwnerTableCell().OwnerRow;
      if (ownerRow.Index > 0)
      {
        if (!(this.m_paragraph.StyleName == "Normal") || this.IsParaHaveNumberingOrBullets())
          return;
        this.Margins.Top = 0.0f;
      }
      else if (ownerRow.OwnerTable.IsInCell && ownerRow.OwnerTable.Index == 0)
      {
        this.CheckOwnerTablePrevItem(ownerRow.OwnerTable);
      }
      else
      {
        IEntity nextOrPrevSibling = ownerRow.OwnerTable.PreviousSibling;
        if (nextOrPrevSibling is BlockContentControl)
          nextOrPrevSibling = (IEntity) (nextOrPrevSibling as BlockContentControl).GetLastParagraphOfSDTContent();
        if (!this.IsSameStyle(this.m_paragraph, nextOrPrevSibling as WParagraph))
          return;
        this.Margins.Top = 0.0f;
      }
    }

    private void UpdateParagraphSpacing()
    {
      if (this.m_paragraph.ParagraphFormat.SpaceBeforeAuto)
        this.Margins.Top = 14f;
      if (this.m_paragraph.ParagraphFormat.SpaceAfterAuto)
        this.Margins.Bottom = 14f;
      float num = 0.0f;
      IEntity entity = this.GetNextSibling();
      if (entity != null && entity is WParagraph)
        entity = ((WParagraph) entity).IsParagraphBeforeSpacingNeedToSkip() ? (IEntity) null : entity;
      IEntity prevSibling = this.GetPrevSibling();
      if (entity != null && entity is WParagraph && !(entity as WParagraph).SectionEndMark && (!(entity as WParagraph).ParagraphFormat.SpaceBeforeAuto && (double) (num = (float) (entity as WParagraph).ParagraphFormat.GetParagraphFormat(8)) > (double) this.Margins.Bottom || (entity as WParagraph).ParagraphFormat.SpaceBeforeAuto && (double) this.Margins.Bottom < 14.0) && (!(entity as WParagraph).ParagraphFormat.SpaceBeforeAuto || !this.IsParagraphContainsListtype(this.m_paragraph) || !this.IsParagraphContainsListtype(entity as WParagraph)) && (!(entity as WParagraph).ParagraphFormat.ContextualSpacing || !this.IsSameStyle(this.m_paragraph, entity as WParagraph)))
      {
        if ((entity as WParagraph).ParagraphFormat.SpaceBeforeAuto)
          this.Margins.Bottom = 14f;
        else
          this.Margins.Bottom = num;
      }
      if (prevSibling != null && prevSibling is WParagraph && (prevSibling as WParagraph).m_layoutInfo != null && (double) ((prevSibling as WParagraph).m_layoutInfo as ParagraphLayoutInfo).Margins.Bottom >= (double) this.Margins.Top && (this.m_paragraph.ParagraphFormat.IsPreviousParagraphInSameFrame() || !(prevSibling as WParagraph).ParagraphFormat.IsInFrame() && !this.m_paragraph.ParagraphFormat.IsInFrame()))
        this.Margins.Top = 0.0f;
      if (prevSibling == null && !this.m_paragraph.Document.DOP.Dop2000.Copts.DontUseHTMLParagraphAutoSpacing)
        this.UpdateTopMargin();
      if (!this.m_paragraph.IsFirstParagraphOfOwnerTextBody() || !this.m_paragraph.ParagraphFormat.SpaceBeforeAuto)
        return;
      this.Margins.Top = 0.0f;
    }

    private bool IsSameStyle(WParagraph currParagraph, WParagraph nextOrPrevSibling)
    {
      return currParagraph != null && nextOrPrevSibling != null && currParagraph.StyleName == nextOrPrevSibling.StyleName;
    }

    private void UpdateAfterSpacing(WParagraph paragraphNextsibling)
    {
      if (!this.IsSameStyle(this.m_paragraph, paragraphNextsibling) && (paragraphNextsibling != null || !(this.m_paragraph.StyleName == "Normal") || this.IsParaHaveNumberingOrBullets()))
        return;
      this.Margins.Bottom = 0.0f;
    }

    private void UpdateTopMargin()
    {
      WSection ownerSection = this.m_paragraph.GetOwnerSection();
      if (ownerSection == null || ownerSection.Index <= 0)
        return;
      Entity entity = ownerSection.Body.ChildEntities.FirstItem;
      if (entity is BlockContentControl)
        entity = (Entity) (entity as BlockContentControl).GetFirstParagraphOfSDTContent();
      if (entity != this.m_paragraph || !(this.GetLastItemOfPreviousSection(ownerSection.Index) is WParagraph ofPreviousSection))
        return;
      float num1 = this.m_paragraph.ParagraphFormat.SpaceBeforeAuto ? 14f : this.m_paragraph.ParagraphFormat.BeforeSpacing;
      float num2 = ofPreviousSection.ParagraphFormat.SpaceAfterAuto ? 14f : ofPreviousSection.ParagraphFormat.AfterSpacing;
      this.Margins.Top = (double) num1 > (double) num2 ? num1 - num2 : 0.0f;
      this.m_paragraph.IsTopMarginValueUpdated = true;
    }

    private IEntity GetPrevSibling()
    {
      IEntity prevSibling = this.m_prevSibling;
      if (prevSibling == null && this.m_paragraph.OwnerTextBody.Owner is BlockContentControl)
        prevSibling = (this.m_paragraph.OwnerTextBody.Owner as BlockContentControl).PreviousSibling;
      if (prevSibling is BlockContentControl)
        prevSibling = (IEntity) (prevSibling as BlockContentControl).GetLastParagraphOfSDTContent();
      return prevSibling;
    }

    private IEntity GetNextSibling()
    {
      IEntity nextSibling = this.m_nextSibling;
      if (nextSibling == null && this.m_paragraph.OwnerTextBody.Owner is BlockContentControl)
        nextSibling = (this.m_paragraph.OwnerTextBody.Owner as BlockContentControl).NextSibling;
      if (nextSibling is BlockContentControl)
        nextSibling = (IEntity) (nextSibling as BlockContentControl).GetFirstParagraphOfSDTContent();
      return nextSibling;
    }

    private IEntity GetLastItemOfPreviousSection(int currentSectionIndex)
    {
      IEntity ofPreviousSection = (IEntity) this.m_paragraph.Document.Sections[currentSectionIndex - 1].Body.ChildEntities.LastItem;
      if (ofPreviousSection == null && this.m_paragraph.OwnerTextBody.Owner is BlockContentControl)
        ofPreviousSection = (this.m_paragraph.OwnerTextBody.Owner as BlockContentControl).PreviousSibling;
      if (ofPreviousSection is BlockContentControl)
        ofPreviousSection = (IEntity) (ofPreviousSection as BlockContentControl).GetLastParagraphOfSDTContent();
      return ofPreviousSection;
    }

    private bool IsParagraphContainsListtype(WParagraph para)
    {
      WParagraphStyle wparagraphStyle = para.ParaStyle as WParagraphStyle;
      if (para.ListFormat.ListType != ListType.NoList)
        return true;
      for (; wparagraphStyle != null; wparagraphStyle = wparagraphStyle.BaseStyle)
      {
        if (wparagraphStyle.ListFormat.ListType != ListType.NoList || wparagraphStyle.ListFormat.IsEmptyList)
          return true;
      }
      return false;
    }
  }

  internal class ListTabs : TabsLayoutInfo
  {
    public ListTabs(WParagraph paragrath)
      : base(ChildrenLayoutDirection.Horizontal)
    {
      this.m_defaultTabWidth = paragrath.GetDefaultTabWidth();
    }
  }
}
