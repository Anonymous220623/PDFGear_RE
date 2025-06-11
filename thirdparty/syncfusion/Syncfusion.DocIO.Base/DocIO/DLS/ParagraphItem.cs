// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.ParagraphItem
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.Layouting;
using Syncfusion.Office;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public abstract class ParagraphItem(WordDocument doc) : 
  WidgetBase(doc, (Entity) null),
  IParagraphItem,
  IEntity,
  IOfficeRun
{
  private int m_startIndex;
  protected WCharacterFormat m_charFormat;
  private byte m_bFlags = 4;
  private IOfficeMathRunElement m_ownerMathRunElement;

  internal bool SkipDocxItem
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal bool IsMappedItem
  {
    get => ((int) this.m_bFlags & 32 /*0x20*/) >> 5 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 223 | (value ? 1 : 0) << 5);
  }

  internal bool IsCloned
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
  }

  public WParagraph OwnerParagraph
  {
    get
    {
      return this.OwnerMathRunElement != null ? this.GetBaseWMath(this.OwnerMathRunElement)?.OwnerParagraph : (this.Owner is InlineContentControl ? this.GetOwnerParagraphValue() : this.Owner as WParagraph);
    }
  }

  internal WMath GetBaseWMath(IOfficeMathRunElement OwnerMathRunElement)
  {
    IOfficeMathEntity officeMathEntity = (IOfficeMathEntity) OwnerMathRunElement;
    while (true)
    {
      switch (officeMathEntity)
      {
        case IOfficeMathParagraph _:
        case null:
          goto label_3;
        default:
          officeMathEntity = officeMathEntity.OwnerMathEntity;
          continue;
      }
    }
label_3:
    return officeMathEntity == null || !(officeMathEntity is IOfficeMathParagraph) ? (WMath) null : (officeMathEntity as IOfficeMathParagraph).Owner as WMath;
  }

  public void ApplyStyle(string styleName)
  {
    IWCharacterStyle style = (IWCharacterStyle) (this.Document.Styles.FindByName(styleName, StyleType.CharacterStyle) as WCharacterStyle);
    if (style == null && styleName == "Default Paragraph Font")
      style = (IWCharacterStyle) Style.CreateBuiltinCharacterStyle(BuiltinStyle.DefaultParagraphFont, this.Document);
    if (style == null)
      throw new ArgumentException("Specified Character style not found");
    this.ApplyCharacterStyle(style);
  }

  internal void ApplyCharacterStyle(IWCharacterStyle style)
  {
    this.m_charFormat.CharStyleName = style != null ? style.Name : throw new ArgumentNullException("newStyle");
    switch (this)
    {
      case Break _:
        (this as Break).CharacterFormat.CharStyleName = style.Name;
        break;
      case InlineContentControl _:
        (this as InlineContentControl).ApplyBaseFormatForCharacterStyle(style);
        break;
    }
  }

  internal WParagraph GetOwnerParagraphValue()
  {
    if (this.OwnerMathRunElement != null)
      return this.GetBaseWMath(this.OwnerMathRunElement)?.OwnerParagraph;
    Entity owner = this.Owner;
    while (true)
    {
      switch (owner)
      {
        case WParagraph _:
        case null:
          goto label_7;
        default:
          owner = owner.Owner;
          continue;
      }
    }
label_7:
    return owner as WParagraph;
  }

  public bool IsInsertRevision => this.m_charFormat != null && this.m_charFormat.IsInsertRevision;

  internal string AuthorName
  {
    get => this.m_charFormat != null ? this.m_charFormat.AuthorName : string.Empty;
  }

  internal DateTime RevDateTime
  {
    get => this.m_charFormat != null ? this.m_charFormat.RevDateTime : DateTime.MinValue;
  }

  internal void SetInsertRev(bool value, string authorName, DateTime dt)
  {
    if (this is Break)
    {
      (this as Break).CharacterFormat.IsInsertRevision = value;
      if (!string.IsNullOrEmpty(authorName))
        (this as Break).CharacterFormat.AuthorName = authorName;
      if (dt.Year <= 1900)
        return;
      (this as Break).CharacterFormat.RevDateTime = dt;
    }
    else
    {
      this.ParaItemCharFormat.IsInsertRevision = value;
      if (!string.IsNullOrEmpty(authorName))
        this.ParaItemCharFormat.AuthorName = authorName;
      if (dt.Year <= 1900)
        return;
      this.ParaItemCharFormat.RevDateTime = dt;
    }
  }

  public bool IsDeleteRevision => this.m_charFormat != null && this.m_charFormat.IsDeleteRevision;

  internal void SetDeleteRev(bool value, string authorName, DateTime dt)
  {
    if (this is Break)
    {
      (this as Break).CharacterFormat.IsDeleteRevision = value;
      if (!string.IsNullOrEmpty(authorName))
        (this as Break).CharacterFormat.AuthorName = authorName;
      if (dt.Year <= 1900)
        return;
      (this as Break).CharacterFormat.RevDateTime = dt;
    }
    else
    {
      this.ParaItemCharFormat.IsDeleteRevision = value;
      if (!string.IsNullOrEmpty(authorName))
        this.ParaItemCharFormat.AuthorName = authorName;
      if (dt.Year <= 1900)
        return;
      this.ParaItemCharFormat.RevDateTime = dt;
    }
  }

  internal bool IsChangedCFormat
  {
    get => this.m_charFormat != null && this.m_charFormat.IsChangedFormat;
    set
    {
      if (this.m_charFormat == null)
        return;
      this.m_charFormat.IsChangedFormat = value;
    }
  }

  internal int StartPos
  {
    get => this.m_startIndex;
    set
    {
      if (this.m_startIndex != value)
        this.IsDetachedTextChanged = true;
      this.m_startIndex = value;
    }
  }

  internal bool IsDetachedTextChanged
  {
    get => ((int) this.m_bFlags & 4) >> 2 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 251 | (value ? 1 : 0) << 2);
  }

  internal virtual int EndPos => this.StartPos;

  internal bool ItemDetached => this.OwnerBase == null;

  internal WCharacterFormat ParaItemCharFormat
  {
    get
    {
      if (this.m_charFormat == null)
        this.m_charFormat = this.Owner is InlineContentControl || this.Owner is XmlParagraphItem ? this.GetOwnerParagraphValue().BreakCharacterFormat : this.OwnerParagraph.BreakCharacterFormat;
      return this.m_charFormat;
    }
  }

  public IOfficeMathRunElement OwnerMathRunElement
  {
    get => this.m_ownerMathRunElement;
    set
    {
      this.m_ownerMathRunElement = value;
      if (this.OwnerParagraph == null || this.OwnerParagraph.BreakCharacterFormat == null || this.OwnerParagraph.BreakCharacterFormat.BaseFormat == null || this.ParaItemCharFormat == null)
        return;
      this.ParaItemCharFormat.ApplyBase(this.OwnerParagraph.BreakCharacterFormat.BaseFormat);
      this.ParaItemCharFormat.FontName = "Cambria Math";
      if (!(this is WOleObject))
        return;
      (this as WOleObject).OlePicture.OwnerMathRunElement = value;
    }
  }

  internal virtual void AttachToParagraph(WParagraph owner, int itemPos)
  {
    if (owner == null && !(this.Owner is InlineContentControl))
      throw new ArgumentNullException(nameof (owner));
    if (owner != this.OwnerParagraph && !(this.Owner is InlineContentControl))
      throw new InvalidOperationException();
    if (this.ItemDetached)
      throw new InvalidOperationException();
    if (this is InlineContentControl && this.OwnerParagraph.BreakCharacterFormat.BaseFormat != null)
    {
      foreach (ParagraphItem paragraphItem in (CollectionImpl) (this as InlineContentControl).ParagraphItems)
        paragraphItem.ParaItemCharFormat.ApplyBase(this.OwnerParagraph.BreakCharacterFormat.BaseFormat);
    }
    if (this.Owner is WParagraph && this.OwnerParagraph.BreakCharacterFormat.BaseFormat != null)
      this.ParaItemCharFormat.ApplyBase(this.OwnerParagraph.BreakCharacterFormat.BaseFormat);
    else if (this.Owner is InlineContentControl && this.OwnerParagraph.BreakCharacterFormat.BaseFormat != null)
      this.ParaItemCharFormat.ApplyBase(this.OwnerParagraph.BreakCharacterFormat.BaseFormat);
    if (this is WMath)
      (this as WMath).ApplyBaseFormat();
    if (this is InlineContentControl)
      this.Document.UpdateStartPos((ParagraphItem) (this as InlineContentControl), itemPos - this.StartPos);
    else
      this.StartPos = itemPos;
  }

  internal virtual void Detach()
  {
    if (this.ItemDetached && !this.Document.IsClosing)
      throw new InvalidOperationException();
  }

  internal void AcceptChanges()
  {
    if (this.m_charFormat == null)
      return;
    this.m_charFormat.AcceptChanges();
  }

  internal void RemoveChanges()
  {
    if (this.m_charFormat == null)
      return;
    this.m_charFormat.RemoveChanges();
  }

  internal bool HasTrackedChanges()
  {
    return this.IsInsertRevision || this.IsDeleteRevision || this.IsChangedCFormat;
  }

  internal WCharacterFormat GetCharFormat() => this.m_charFormat;

  internal void ApplyTableStyleFormatting(WParagraph source, WParagraph clonedParagraph)
  {
    if (source.BreakCharacterFormat.TableStyleCharacterFormat != null)
      clonedParagraph.BreakCharacterFormat.TableStyleCharacterFormat = source.BreakCharacterFormat.TableStyleCharacterFormat;
    if (source.ParagraphFormat.TableStyleParagraphFormat == null)
      return;
    clonedParagraph.ParagraphFormat.TableStyleParagraphFormat = source.ParagraphFormat.TableStyleParagraphFormat;
  }

  internal void SetParagraphItemCharacterFormat(WCharacterFormat charFormat)
  {
    if (this.m_charFormat != null)
      this.m_charFormat.SetOwner((WordDocument) null, (OwnerHolder) null);
    this.m_charFormat = new WCharacterFormat((IWordDocument) this.Document, (Entity) this);
    this.m_charFormat.ImportContainer((FormatBase) charFormat);
    this.m_charFormat.CopyProperties((FormatBase) charFormat);
    if (charFormat.BaseFormat != null && !this.Document.IsOpening && !this.Document.IsCloning && !this.Document.IsMailMerge)
      this.m_charFormat.ApplyBase(charFormat.BaseFormat);
    if (this.m_charFormat == null)
      return;
    this.m_charFormat.SetOwner((OwnerHolder) this);
  }

  internal bool IsNotFieldShape()
  {
    if (this.PreviousSibling is WPicture || this.PreviousSibling is Shape)
    {
      Entity previousSibling1 = this.PreviousSibling as Entity;
      if (previousSibling1.PreviousSibling is WFieldMark)
      {
        WFieldMark previousSibling2 = previousSibling1.PreviousSibling as WFieldMark;
        if (previousSibling2.Type == FieldMarkType.FieldSeparator && previousSibling2.ParentField != null && previousSibling2.ParentField.FieldType == FieldType.FieldShape)
          return false;
      }
    }
    return true;
  }

  internal override void Close()
  {
    if (this.m_charFormat != null)
    {
      this.m_charFormat.Close();
      this.m_charFormat = (WCharacterFormat) null;
    }
    base.Close();
  }

  public IOfficeRun CloneRun() => (IOfficeRun) this.CloneImpl();

  public void Dispose() => this.Close();

  protected override object CloneImpl()
  {
    ParagraphItem owner = (ParagraphItem) base.CloneImpl();
    if (this.m_charFormat != null)
    {
      owner.m_charFormat = new WCharacterFormat((IWordDocument) this.Document, (Entity) owner);
      owner.m_charFormat.ImportContainer((FormatBase) this.m_charFormat);
      owner.m_charFormat.CopyProperties((FormatBase) this.m_charFormat);
    }
    owner.m_ownerMathRunElement = this.m_ownerMathRunElement;
    if (this.m_revisions != null)
    {
      owner.m_revisions = new List<Revision>();
      foreach (Revision revision in this.m_revisions)
        owner.m_revisions.Add(revision.Clone());
    }
    owner.IsCloned = true;
    return (object) owner;
  }

  internal override void CloneRelationsTo(WordDocument doc, OwnerHolder nextOwner)
  {
    if (doc != this.Document && (doc.ImportOptions & ImportOptions.UseDestinationStyles) != (ImportOptions) 0 && doc.Sections.Count == 0 && this.Document != null && this.Document.DefCharFormat != null)
    {
      if (doc.DefCharFormat == null)
        doc.DefCharFormat = new WCharacterFormat((IWordDocument) doc);
      doc.DefCharFormat.ImportContainer((FormatBase) this.Document.DefCharFormat);
    }
    base.CloneRelationsTo(doc, nextOwner);
    if (this.m_charFormat != null)
      this.m_charFormat.CloneRelationsTo(doc, nextOwner);
    if (doc == this.Document || (doc.ImportOptions & ImportOptions.UseDestinationStyles) == (ImportOptions) 0)
      return;
    IWParagraphStyle wparagraphStyle = (IWParagraphStyle) null;
    if (this.OwnerParagraph != null)
      wparagraphStyle = this.OwnerParagraph.GetStyle();
    if (wparagraphStyle == null)
      return;
    this.ParaItemCharFormat.ApplyBase((FormatBase) wparagraphStyle.CharacterFormat);
    switch (this)
    {
      case Break _:
        (this as Break).CharacterFormat.ApplyBase((FormatBase) wparagraphStyle.CharacterFormat);
        break;
      case InlineContentControl _:
        (this as InlineContentControl).ApplyBaseFormat();
        break;
      case WMath _:
        (this as WMath).ApplyBaseFormat();
        break;
    }
  }

  internal TextWrappingStyle GetTextWrappingStyle()
  {
    switch (this)
    {
      case Shape _:
        return (this as Shape).WrapFormat.TextWrappingStyle;
      case WChart _:
        return (this as WChart).WrapFormat.TextWrappingStyle;
      case WTextBox _:
        return (this as WTextBox).TextBoxFormat.TextWrappingStyle;
      case GroupShape _:
        return (this as GroupShape).WrapFormat.TextWrappingStyle;
      default:
        return (this as WPicture).TextWrappingStyle;
    }
  }

  internal bool IsWrappingBoundsAdded()
  {
    switch (this)
    {
      case Shape _:
        return (this as Shape).WrapFormat.IsWrappingBoundsAdded;
      case WChart _:
        return (this as WChart).WrapFormat.IsWrappingBoundsAdded;
      case WTextBox _:
        return (this as WTextBox).TextBoxFormat.IsWrappingBoundsAdded;
      case GroupShape _:
        return (this as GroupShape).WrapFormat.IsWrappingBoundsAdded;
      default:
        return (this as WPicture).IsWrappingBoundsAdded;
    }
  }

  internal void SetIsWrappingBoundsAdded(bool boolean)
  {
    switch (this)
    {
      case Shape _:
        (this as Shape).WrapFormat.IsWrappingBoundsAdded = boolean;
        break;
      case WChart _:
        (this as WChart).WrapFormat.IsWrappingBoundsAdded = boolean;
        break;
      case WTextBox _:
        (this as WTextBox).TextBoxFormat.IsWrappingBoundsAdded = boolean;
        break;
      case GroupShape _:
        (this as GroupShape).WrapFormat.IsWrappingBoundsAdded = boolean;
        break;
      default:
        (this as WPicture).IsWrappingBoundsAdded = boolean;
        break;
    }
  }

  internal bool GetLayOutInCell()
  {
    switch (this)
    {
      case Shape _:
        return (this as Shape).LayoutInCell;
      case WTextBox _:
        return (this as WTextBox).TextBoxFormat.AllowInCell;
      case WChart _:
        return (this as WChart).LayoutInCell;
      case GroupShape _:
        return (this as GroupShape).LayoutInCell;
      default:
        return (this as WPicture).LayoutInCell;
    }
  }

  internal VerticalOrigin GetVerticalOrigin()
  {
    switch (this)
    {
      case Shape _:
        return (this as Shape).VerticalOrigin;
      case WChart _:
        return (this as WChart).VerticalOrigin;
      case WTextBox _:
        return (this as WTextBox).TextBoxFormat.VerticalOrigin;
      case GroupShape _:
        return (this as GroupShape).VerticalOrigin;
      default:
        return (this as WPicture).VerticalOrigin;
    }
  }

  internal ShapeVerticalAlignment GetShapeVerticalAlignment()
  {
    switch (this)
    {
      case Shape _:
        return (this as Shape).VerticalAlignment;
      case WChart _:
        return (this as WChart).VerticalAlignment;
      case WTextBox _:
        return (this as WTextBox).TextBoxFormat.VerticalAlignment;
      case GroupShape _:
        return (this as GroupShape).VerticalAlignment;
      default:
        return (this as WPicture).VerticalAlignment;
    }
  }

  internal ShapeHorizontalAlignment GetShapeHorizontalAlignment()
  {
    switch (this)
    {
      case Shape _:
        return (this as Shape).HorizontalAlignment;
      case WChart _:
        return (this as WChart).HorizontalAlignment;
      case WTextBox _:
        return (this as WTextBox).TextBoxFormat.HorizontalAlignment;
      case GroupShape _:
        return (this as GroupShape).HorizontalAlignment;
      default:
        return (this as WPicture).HorizontalAlignment;
    }
  }

  internal HorizontalOrigin GetHorizontalOrigin()
  {
    switch (this)
    {
      case Shape _:
        return (this as Shape).HorizontalOrigin;
      case WChart _:
        return (this as WChart).HorizontalOrigin;
      case WTextBox _:
        return (this as WTextBox).TextBoxFormat.HorizontalOrigin;
      case GroupShape _:
        return (this as GroupShape).HorizontalOrigin;
      default:
        return (this as WPicture).HorizontalOrigin;
    }
  }

  internal float GetHorizontalPosition()
  {
    switch (this)
    {
      case Shape _:
        return (this as Shape).HorizontalPosition;
      case WChart _:
        return (this as WChart).HorizontalPosition;
      case WTextBox _:
        return (this as WTextBox).TextBoxFormat.HorizontalPosition;
      case GroupShape _:
        return (this as GroupShape).HorizontalPosition;
      default:
        return (this as WPicture).HorizontalPosition;
    }
  }

  internal float GetVerticalPosition()
  {
    switch (this)
    {
      case Shape _:
        return (this as Shape).VerticalPosition;
      case WChart _:
        return (this as WChart).VerticalPosition;
      case WTextBox _:
        return (this as WTextBox).TextBoxFormat.VerticalPosition;
      case GroupShape _:
        return (this as GroupShape).VerticalPosition;
      default:
        return (this as WPicture).VerticalPosition;
    }
  }

  internal bool GetAllowOverlap()
  {
    switch (this)
    {
      case Shape _:
        return (this as Shape).WrapFormat.AllowOverlap;
      case WChart _:
        return (this as WChart).WrapFormat.AllowOverlap;
      case WTextBox _:
        return (this as WTextBox).TextBoxFormat.AllowOverlap;
      case GroupShape _:
        return (this as GroupShape).WrapFormat.AllowOverlap;
      default:
        return (this as WPicture).AllowOverlap;
    }
  }

  internal void GetEffectExtentValues(
    out float leftEdgeExtent,
    out float rightEgeExtent,
    out float topEdgeExtent,
    out float bottomEdgeExtent)
  {
    leftEdgeExtent = 0.0f;
    rightEgeExtent = 0.0f;
    topEdgeExtent = 0.0f;
    bottomEdgeExtent = 0.0f;
    switch (this)
    {
      case WTextBox _ when (this as WTextBox).GetTextWrappingStyle() == TextWrappingStyle.Inline && (this as WTextBox).Shape != null && (double) (this as WTextBox).TextBoxFormat.Rotation == 0.0:
        Shape shape1 = (this as WTextBox).Shape;
        leftEdgeExtent = shape1.LeftEdgeExtent;
        rightEgeExtent = shape1.RightEdgeExtent;
        topEdgeExtent = shape1.TopEdgeExtent;
        bottomEdgeExtent = shape1.BottomEdgeExtent;
        break;
      case Shape _ when (this as Shape).GetTextWrappingStyle() == TextWrappingStyle.Inline && (double) (this as Shape).Rotation == 0.0:
        Shape shape2 = this as Shape;
        leftEdgeExtent = shape2.LeftEdgeExtent;
        rightEgeExtent = shape2.RightEdgeExtent;
        topEdgeExtent = shape2.TopEdgeExtent;
        bottomEdgeExtent = shape2.BottomEdgeExtent;
        break;
      case GroupShape _ when (this as GroupShape).GetTextWrappingStyle() == TextWrappingStyle.Inline && (double) (this as GroupShape).Rotation == 0.0:
        GroupShape groupShape = this as GroupShape;
        leftEdgeExtent = groupShape.LeftEdgeExtent;
        rightEgeExtent = groupShape.RightEdgeExtent;
        topEdgeExtent = groupShape.TopEdgeExtent;
        bottomEdgeExtent = groupShape.BottomEdgeExtent;
        break;
    }
  }

  private float GetLeftMargin(WSection section) => Layouter.GetLeftMargin(section);

  private float GetRightMargin(WSection section) => Layouter.GetRightMargin(section);

  private new Entity GetBaseEntity(Entity entity)
  {
    Entity baseEntity = entity;
    while (baseEntity != null && baseEntity.Owner != null)
    {
      baseEntity = baseEntity.Owner;
      if (baseEntity is WSection)
        return baseEntity;
    }
    return baseEntity;
  }

  internal float GetWidthRelativeToPercent(bool isDocToPdf)
  {
    Entity baseEntity = this.GetBaseEntity((Entity) this);
    if (baseEntity is WSection)
    {
      WPageSetup pageSetup = (baseEntity as WSection).PageSetup;
      WTextBox wtextBox = this as WTextBox;
      WidthOrigin widthOrigin = WidthOrigin.Page;
      float num1 = 0.0f;
      float num2 = 0.0f;
      if (wtextBox == null)
      {
        if (this is Shape shape)
        {
          widthOrigin = shape.TextFrame.WidthOrigin;
          num1 = shape.TextFrame.WidthRelativePercent;
          if (isDocToPdf && shape.LineFormat.Line)
            num2 = shape.LineFormat.Weight;
        }
      }
      else
      {
        widthOrigin = wtextBox.TextBoxFormat.WidthOrigin;
        num1 = wtextBox.TextBoxFormat.WidthRelativePercent;
        if (isDocToPdf && !wtextBox.TextBoxFormat.NoLine)
          num2 = (double) wtextBox.TextBoxFormat.LineWidth >= 2.0 ? wtextBox.TextBoxFormat.LineWidth : 2f;
      }
      float relativeToPercent;
      switch (widthOrigin)
      {
        case WidthOrigin.Page:
          relativeToPercent = pageSetup.PageSize.Width * (num1 / 100f);
          break;
        case WidthOrigin.LeftMargin:
        case WidthOrigin.InsideMargin:
          relativeToPercent = (isDocToPdf ? this.GetLeftMargin(baseEntity as WSection) : pageSetup.Margins.Left + (baseEntity.Document.DOP.GutterAtTop ? 0.0f : pageSetup.Margins.Gutter)) * (num1 / 100f);
          break;
        case WidthOrigin.RightMargin:
        case WidthOrigin.OutsideMargin:
          relativeToPercent = (float) ((isDocToPdf ? (double) this.GetRightMargin(baseEntity as WSection) : (double) pageSetup.Margins.Right) * ((double) num1 / 100.0));
          break;
        default:
          relativeToPercent = pageSetup.ClientWidth * (num1 / 100f);
          break;
      }
      if ((double) num2 != 0.0)
        relativeToPercent -= num2;
      return relativeToPercent;
    }
    return !(this is Shape) ? (this as WTextBox).TextBoxFormat.Width : (this as Shape).Width;
  }

  internal float GetHeightRelativeToPercent(bool isDocToPdf)
  {
    Entity baseEntity = this.GetBaseEntity((Entity) this);
    if (baseEntity is WSection)
    {
      WPageSetup pageSetup = (baseEntity as WSection).PageSetup;
      WTextBox wtextBox = this as WTextBox;
      HeightOrigin heightOrigin = HeightOrigin.Page;
      float num1 = 0.0f;
      float num2 = 0.0f;
      if (wtextBox == null)
      {
        if (this is Shape shape)
        {
          heightOrigin = shape.TextFrame.HeightOrigin;
          num1 = shape.TextFrame.HeightRelativePercent;
          if (isDocToPdf && shape.LineFormat.Line)
            num2 = shape.LineFormat.Weight;
        }
      }
      else
      {
        heightOrigin = wtextBox.TextBoxFormat.HeightOrigin;
        num1 = wtextBox.TextBoxFormat.HeightRelativePercent;
        if (isDocToPdf && !wtextBox.TextBoxFormat.NoLine)
          num2 = (double) wtextBox.TextBoxFormat.LineWidth >= 2.0 ? wtextBox.TextBoxFormat.LineWidth : 2f;
      }
      float relativeToPercent;
      switch (heightOrigin)
      {
        case HeightOrigin.Page:
          relativeToPercent = pageSetup.PageSize.Height * (num1 / 100f);
          break;
        case HeightOrigin.TopMargin:
        case HeightOrigin.InsideMargin:
          relativeToPercent = (float) (((double) pageSetup.Margins.Top + (baseEntity.Document.DOP.GutterAtTop ? (double) pageSetup.Margins.Gutter : 0.0)) * ((double) num1 / 100.0));
          break;
        case HeightOrigin.BottomMargin:
        case HeightOrigin.OutsideMargin:
          relativeToPercent = pageSetup.Margins.Bottom * (num1 / 100f);
          break;
        default:
          relativeToPercent = (float) (((double) pageSetup.PageSize.Height - (double) pageSetup.Margins.Top - (baseEntity.Document.DOP.GutterAtTop ? (double) pageSetup.Margins.Gutter : 0.0) - (double) pageSetup.Margins.Bottom) * ((double) num1 / 100.0));
          break;
      }
      if ((double) num2 != 0.0)
        relativeToPercent -= num2;
      return relativeToPercent;
    }
    return !(this is Shape) ? (this as WTextBox).TextBoxFormat.Height : (this as Shape).Height;
  }

  internal int GetWrapCollectionIndex()
  {
    switch (this)
    {
      case Shape _:
        return (this as Shape).WrapFormat.WrapCollectionIndex;
      case WChart _:
        return (this as WChart).WrapFormat.WrapCollectionIndex;
      case WTextBox _:
        return (int) (this as WTextBox).TextBoxFormat.WrapCollectionIndex;
      case GroupShape _:
        return (this as GroupShape).WrapFormat.WrapCollectionIndex;
      default:
        return (int) (this as WPicture).WrapCollectionIndex;
    }
  }

  internal List<Path2D> Parse2007CustomShapePoints(string path)
  {
    if (string.IsNullOrEmpty(path))
      return (List<Path2D>) null;
    List<char> VMLCommands = new List<char>();
    VMLCommands.Add('m');
    VMLCommands.Add('l');
    VMLCommands.Add('x');
    VMLCommands.Add('e');
    VMLCommands.Add('t');
    VMLCommands.Add('r');
    List<char> VMLSeparators = new List<char>();
    VMLSeparators.Add(',');
    VMLSeparators.Add(' ');
    List<char> VMLOperators = new List<char>();
    VMLOperators.Add('-');
    VMLOperators.Add('+');
    if (this.IsValidVMLPath(path, VMLCommands, VMLSeparators, VMLOperators))
    {
      List<Path2D> vmlPath = this.ParseVMLPath(path, VMLCommands, VMLSeparators, VMLOperators);
      if (vmlPath.Count > 0)
        return vmlPath;
    }
    return (List<Path2D>) null;
  }

  private List<Path2D> ParseVMLPath(
    string path,
    List<char> VMLCommands,
    List<char> VMLSeparators,
    List<char> VMLOperators)
  {
    List<Path2D> vmlPath = new List<Path2D>();
    int index1 = 0;
    while (index1 < path.Length)
    {
      this.SkipWhiteSpaces(path, ref index1);
      if (index1 >= path.Length || vmlPath.Count == 0 && !VMLCommands.Contains(path[index1]))
        return vmlPath;
      string pathCommandType = path[index1].ToString();
      int startIndex = index1 + 1;
      int indexOfPathCommand = this.GetNextIndexOfPathCommand(path, startIndex, VMLCommands);
      string path1 = path.Substring(startIndex, indexOfPathCommand - startIndex);
      List<PointF> pathPoints = new List<PointF>();
      int index2 = 0;
      while (index2 < path1.Length)
      {
        this.SkipWhiteSpaces(path1, ref index2);
        float pathPoint1 = this.GetPathPoint(path1, ref index2, VMLOperators);
        ++index2;
        this.SkipWhiteSpaces(path1, ref index2);
        float pathPoint2 = this.GetPathPoint(path1, ref index2, VMLOperators);
        this.SkipWhiteSpaces(path1, ref index2);
        PointF pointF = new PointF(pathPoint1, pathPoint2);
        pathPoints.Add(pointF);
        if (index2 < path1.Length && VMLSeparators.Contains(path1[index2]))
          ++index2;
      }
      index1 = startIndex + path1.Length;
      vmlPath.Add(new Path2D(pathCommandType, pathPoints));
    }
    return vmlPath;
  }

  private void SkipWhiteSpaces(string path, ref int index)
  {
    while (index < path.Length && char.IsWhiteSpace(path[index]))
      ++index;
  }

  private float GetPathPoint(string path, ref int index, List<char> VMLOperators)
  {
    float result = 0.0f;
    string s = (string) null;
    while (index < path.Length && (char.IsNumber(path[index]) || VMLOperators.Contains(path[index])))
    {
      s += (string) (object) path[index];
      ++index;
    }
    if (s != null && float.TryParse(s, out result))
      result = float.Parse(s);
    return result;
  }

  private int GetNextIndexOfPathCommand(string path, int startIndex, List<char> VMLCommands)
  {
    for (int index = startIndex; index < path.Length; ++index)
    {
      if (VMLCommands.Contains(path[index]))
        return index;
    }
    return path.Length;
  }

  private bool IsValidVMLPath(
    string path,
    List<char> VMLCommands,
    List<char> VMLSeparators,
    List<char> VMLOperators)
  {
    foreach (char c in path)
    {
      if (!char.IsNumber(c) && !VMLCommands.Contains(c) && !VMLSeparators.Contains(c) && !VMLOperators.Contains(c))
        return false;
    }
    return true;
  }

  internal void UpdateVMLPathPoints(
    RectangleF bounds,
    string path,
    PointF coordinateOrgin,
    string coordinateSize,
    List<Path2D> vmlPoints,
    bool isUpdated)
  {
    if (isUpdated)
      vmlPoints = this.Parse2007CustomShapePoints(path);
    SizeF sizeF1 = new SizeF(1000f, 1000f);
    if (coordinateSize != null)
    {
      string[] strArray = coordinateSize.Split(new char[2]
      {
        ',',
        ' '
      }, StringSplitOptions.RemoveEmptyEntries);
      if (strArray.Length == 2)
      {
        float result;
        if (float.TryParse(strArray[0].Trim(), out result))
        {
          float num = float.Parse(strArray[0].Trim());
          if ((double) num > 0.0)
            sizeF1.Width = num;
        }
        if (float.TryParse(strArray[1].Trim(), out result))
        {
          float num = float.Parse(strArray[1].Trim());
          if ((double) num > 0.0)
            sizeF1.Height = num;
        }
      }
    }
    SizeF sizeF2 = new SizeF();
    sizeF2.Width = bounds.Width / sizeF1.Width;
    sizeF2.Height = bounds.Height / sizeF1.Height;
    PointF pointF1 = new PointF();
    pointF1.X = bounds.X - sizeF2.Width * coordinateOrgin.X;
    pointF1.Y = bounds.Y - sizeF2.Height * coordinateOrgin.Y;
    PointF pointF2 = new PointF();
    foreach (Path2D vmlPoint in vmlPoints)
    {
      switch (vmlPoint.PathCommandType)
      {
        case "l":
          if (vmlPoint.PathPoints.Count > 0)
          {
            pointF2 = vmlPoint.PathPoints[vmlPoint.PathPoints.Count - 1];
            continue;
          }
          continue;
        case "r":
        case "t":
          if (vmlPoint.PathPoints.Count > 0)
          {
            for (int index = 0; index < vmlPoint.PathPoints.Count; ++index)
            {
              float x = vmlPoint.PathPoints[index].X + pointF2.X;
              float y = vmlPoint.PathPoints[index].Y + pointF2.Y;
              vmlPoint.PathPoints[index] = new PointF(x, y);
              pointF2 = vmlPoint.PathPoints[index];
            }
            continue;
          }
          continue;
        case "m":
          if (vmlPoint.PathPoints.Count > 0)
          {
            pointF2 = vmlPoint.PathPoints[vmlPoint.PathPoints.Count - 1];
            continue;
          }
          continue;
        default:
          continue;
      }
    }
    foreach (Path2D vmlPoint in vmlPoints)
    {
      List<PointF> pathPoints = vmlPoint.PathPoints;
      for (int index = 0; index < pathPoints.Count; ++index)
      {
        PointF pointF3 = pathPoints[index];
        pathPoints[index] = new PointF()
        {
          X = pointF3.X * sizeF2.Width + pointF1.X,
          Y = pointF3.Y * sizeF2.Height + pointF1.Y
        };
      }
    }
  }

  internal void ReUpdateVMLPathPoints(float xOffset, float yOffset, List<Path2D> vmlPoints)
  {
    foreach (Path2D vmlPoint in vmlPoints)
    {
      List<PointF> pathPoints = vmlPoint.PathPoints;
      for (int index = 0; index < pathPoints.Count; ++index)
      {
        PointF pointF = pathPoints[index];
        pathPoints[index] = new PointF()
        {
          X = pointF.X + xOffset,
          Y = pointF.Y + yOffset
        };
      }
    }
  }
}
