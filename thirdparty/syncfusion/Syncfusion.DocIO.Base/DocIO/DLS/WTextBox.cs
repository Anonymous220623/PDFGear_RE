// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WTextBox
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.Rendering;
using Syncfusion.DocIO.DLS.XML;
using Syncfusion.DocIO.Rendering;
using Syncfusion.Layouting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WTextBox : 
  ParagraphItem,
  IWTextBox,
  IParagraphItem,
  ICompositeEntity,
  IEntity,
  ILeafWidget,
  IWidget
{
  internal WTextBody m_textBody;
  protected WTextBoxFormat m_txbxFormat;
  private int m_txbxSpid;
  internal Dictionary<string, Stream> m_docxProps;
  private Shape m_shape;
  private RectangleF m_textLayoutingBounds;
  private byte m_bFlags = 1;

  public string Name
  {
    get
    {
      if (this.m_txbxFormat == null)
        return (string) null;
      if (this.m_txbxFormat != null && string.IsNullOrEmpty(this.m_txbxFormat.Name))
      {
        int num1 = this.Document.TextBoxes.InnerList.IndexOf((object) this);
        int num2;
        this.m_txbxFormat.Name = "Text Box " + (object) (num2 = num1 + 1);
      }
      return this.m_txbxFormat.Name;
    }
    set
    {
      if (this.m_txbxFormat == null)
        return;
      this.m_txbxFormat.Name = value;
    }
  }

  public bool Visible
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set
    {
      if (this.Document != null && !this.Document.IsOpening && this.IsShape)
        this.Shape.Visible = value;
      this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
    }
  }

  public EntityCollection ChildEntities => this.m_textBody.ChildEntities;

  public override EntityType EntityType => EntityType.TextBox;

  internal RectangleF TextLayoutingBounds
  {
    get => this.m_textLayoutingBounds;
    set => this.m_textLayoutingBounds = value;
  }

  internal bool IsShape
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
  }

  internal Dictionary<string, Stream> DocxProps
  {
    get
    {
      if (this.m_docxProps == null)
        this.m_docxProps = new Dictionary<string, Stream>();
      return this.m_docxProps;
    }
  }

  public WTextBoxFormat TextBoxFormat
  {
    get => this.m_txbxFormat;
    set => this.m_txbxFormat = value;
  }

  public WTextBody TextBoxBody => this.m_textBody;

  internal int TextBoxSpid
  {
    get => this.m_txbxSpid;
    set => this.m_txbxSpid = value;
  }

  internal WCharacterFormat CharacterFormat => this.m_charFormat;

  internal Shape Shape
  {
    get => this.m_shape;
    set => this.m_shape = value;
  }

  public WTextBox(IWordDocument doc)
    : base((WordDocument) doc)
  {
    this.m_charFormat = new WCharacterFormat(doc, (Entity) this);
    this.m_txbxFormat = new WTextBoxFormat(this.Document);
    this.m_txbxFormat.SetOwner((OwnerHolder) this);
    this.m_textBody = new WTextBody(this.Document, (Entity) this);
    if (this.Document == null || this.Document.IsOpening)
      return;
    this.IsShape = true;
    this.m_shape = new Shape((IWordDocument) this.Document, AutoShapeType.Rectangle);
    this.m_shape.WrapFormat.AllowOverlap = this.m_txbxFormat.AllowOverlap;
  }

  internal override void AddSelf()
  {
    if (this.m_textBody == null)
      return;
    this.m_textBody.AddSelf();
  }

  internal override void AttachToDocument()
  {
    this.Document.TextBoxes.Add(this);
    if (this.TextBoxFormat.TextWrappingStyle != TextWrappingStyle.Inline)
      this.Document.FloatingItems.Add((Entity) this);
    if (this.m_textBody == null)
      return;
    this.m_textBody.AttachToDocument();
  }

  internal override void CloneRelationsTo(WordDocument doc, OwnerHolder nextOwner)
  {
    base.CloneRelationsTo(doc, nextOwner);
    this.TextBoxBody.CloneRelationsTo(doc, nextOwner);
    if (nextOwner.OwnerBase is HeaderFooter || nextOwner is HeaderFooter)
      this.TextBoxFormat.IsHeaderTextBox = true;
    this.TextBoxFormat.CloneRelationsTo(doc, nextOwner);
    this.Document.CloneShapeEscher(doc, (IParagraphItem) this);
    this.IsCloned = false;
  }

  protected override object CloneImpl()
  {
    WTextBox owner = (WTextBox) base.CloneImpl();
    owner.m_textBody = (WTextBody) this.TextBoxBody.Clone();
    if (this.Shape != null)
      owner.Shape = (Shape) this.Shape.Clone();
    int index = 0;
    for (int count = owner.m_textBody.Items.Count; index < count; ++index)
      owner.m_textBody.Items[index].SetOwner((OwnerHolder) owner.m_textBody);
    owner.m_txbxFormat = this.TextBoxFormat.Clone();
    owner.m_textBody.SetOwner((OwnerHolder) owner);
    owner.m_txbxFormat.SetOwner((OwnerHolder) owner);
    owner.IsCloned = true;
    return (object) owner;
  }

  protected override void CreateLayoutInfo()
  {
    this.m_layoutInfo = (ILayoutInfo) new LayoutInfo(ChildrenLayoutDirection.Horizontal);
    if (this.TextBoxFormat.TextWrappingStyle != TextWrappingStyle.Inline)
      this.m_layoutInfo.IsSkipBottomAlign = true;
    if (Entity.IsVerticalTextDirection(this.TextBoxFormat.TextDirection))
      this.m_layoutInfo.IsVerticalText = true;
    if (!this.Visible && this.GetTextWrappingStyle() != TextWrappingStyle.Inline)
      this.m_layoutInfo.IsSkip = true;
    if (!this.IsDeleteRevision || this.Document.RevisionOptions.ShowDeletedText)
      return;
    this.m_layoutInfo.IsSkip = true;
  }

  internal override void InitLayoutInfo(Entity entity, ref bool isLastTOCEntry)
  {
    this.m_layoutInfo = (ILayoutInfo) null;
    if (this.m_textBody != null)
    {
      this.m_textBody.InitLayoutInfo(entity, ref isLastTOCEntry);
      if (isLastTOCEntry)
        return;
    }
    if (this != entity)
      return;
    isLastTOCEntry = true;
  }

  internal bool IsNoNeedToConsiderLineWidth()
  {
    if (this.TextBoxFormat.NoLine && !this.IsShape)
      return true;
    return this.IsShape && !this.TextBoxFormat.HasKey(11);
  }

  internal override void Detach()
  {
    this.Document.TextBoxes.Remove(this);
    this.Document.FloatingItems.Remove((Entity) this);
  }

  internal TextBodyItem GetNextTextBodyItem()
  {
    return this.Owner is WParagraph ? this.OwnerParagraph.GetNextTextBodyItemValue() : (TextBodyItem) null;
  }

  internal override void Close()
  {
    if (this.m_textBody != null)
    {
      this.m_textBody.Close();
      this.m_textBody = (WTextBody) null;
    }
    if (this.m_shape != null)
    {
      this.m_shape.Close();
      this.m_shape = (Shape) null;
    }
    if (this.m_txbxFormat != null)
    {
      this.m_txbxFormat.Close();
      this.m_txbxFormat = (WTextBoxFormat) null;
    }
    if (this.m_docxProps != null)
    {
      foreach (Stream stream in this.m_docxProps.Values)
        stream.Close();
      this.m_docxProps.Clear();
      this.m_docxProps = (Dictionary<string, Stream>) null;
    }
    base.Close();
  }

  internal void SetTextBody(WTextBody textBody) => this.m_textBody = textBody;

  internal WTable GetAsTable(int currPageIndex)
  {
    float leftMargin = 0.0f;
    float num1 = 0.0f;
    float num2 = 0.0f;
    float num3 = 0.0f;
    float num4 = 0.0f;
    float rightMargin = 0.0f;
    Color color1 = new Color();
    WTable asTable = new WTable((IWordDocument) this.Document, false);
    asTable.ResetCells(1, 1);
    asTable.TableFormat.HorizontalAlignment = this.GetHorAlign(this.TextBoxFormat.HorizontalAlignment);
    asTable.Rows[0].Cells[0].CellFormat.TextDirection = this.TextBoxFormat.TextDirection;
    Color color2 = this.TextBoxFormat.FillColor;
    float shapeWidth = this.TextBoxFormat.Width;
    float num5 = this.TextBoxFormat.Height;
    if ((double) this.TextBoxFormat.WidthRelativePercent != 0.0)
      shapeWidth = this.GetWidthRelativeToPercent(false);
    if ((double) this.TextBoxFormat.HeightRelativePercent != 0.0)
      num5 = this.GetHeightRelativeToPercent(false);
    float pageWidth = 0.0f;
    float num6 = 0.0f;
    float num7 = 0.0f;
    float num8 = 0.0f;
    WSection wsection1 = new WSection((IWordDocument) this.Document);
    if (this.Owner != null)
    {
      Entity owner = this.Owner;
      while (!(owner is WSection) && owner.Owner != null)
        owner = owner.Owner;
      if (owner is WSection)
      {
        WSection wsection2 = owner as WSection;
        leftMargin = wsection2.PageSetup.Margins.Left + (wsection2.Document.DOP.GutterAtTop ? 0.0f : wsection2.PageSetup.Margins.Gutter);
        rightMargin = wsection2.PageSetup.Margins.Right;
        num1 = (float) (((double) wsection2.PageSetup.Margins.Top > 0.0 ? (double) wsection2.PageSetup.Margins.Top : 36.0) + (wsection2.Document.DOP.GutterAtTop ? (double) wsection2.PageSetup.Margins.Gutter : 0.0));
        num2 = (double) wsection2.PageSetup.Margins.Bottom > 0.0 ? wsection2.PageSetup.Margins.Bottom : 36f;
        num6 = wsection2.PageSetup.PageSize.Height;
        pageWidth = wsection2.PageSetup.PageSize.Width;
        num7 = wsection2.PageSetup.ClientWidth;
        num8 = wsection2.PageSetup.PageSize.Height - (num3 + num4);
        float footerDistance = wsection2.PageSetup.FooterDistance;
        float headerDistance = wsection2.PageSetup.HeaderDistance;
      }
    }
    WTableRow row = asTable.Rows[0];
    WTableCell cell = row.Cells[0];
    row.Height = num5;
    if (!this.TextBoxFormat.NoLine)
    {
      float lineWidth = this.TextBoxFormat.LineWidth;
      if (this.TextBoxFormat.LineStyle == TextBoxLineStyle.Double)
        lineWidth /= 3f;
      else if (this.TextBoxFormat.LineStyle == TextBoxLineStyle.Triple)
        lineWidth /= 5f;
      row.RowFormat.Borders.LineWidth = lineWidth;
      cell.CellFormat.Borders.LineWidth = lineWidth;
      row.RowFormat.Borders.Color = this.TextBoxFormat.LineColor;
      cell.CellFormat.Borders.Color = this.TextBoxFormat.LineColor;
      row.RowFormat.Borders.BorderType = this.GetBordersStyle(this.TextBoxFormat.LineStyle);
      cell.CellFormat.Borders.BorderType = this.GetBordersStyle(this.TextBoxFormat.LineStyle);
    }
    else
    {
      cell.CellFormat.Borders.BorderType = BorderStyle.None;
      row.RowFormat.Borders.BorderType = BorderStyle.None;
    }
    if (this.TextBoxFormat.TextWrappingStyle != TextWrappingStyle.Inline)
    {
      switch (this.TextBoxFormat.VerticalOrigin)
      {
        case VerticalOrigin.Margin:
          asTable.TableFormat.Positioning.VertRelationTo = VerticalRelation.Margin;
          switch (this.TextBoxFormat.VerticalAlignment)
          {
            case ShapeVerticalAlignment.None:
              asTable.TableFormat.Positioning.VertPosition = (double) Math.Abs(this.TextBoxFormat.VerticalRelativePercent) > 1000.0 ? this.TextBoxFormat.VerticalPosition : num8 * (this.TextBoxFormat.VerticalRelativePercent / 100f);
              break;
            case ShapeVerticalAlignment.Top:
              asTable.TableFormat.Positioning.VertPosition = (double) Math.Abs(this.TextBoxFormat.VerticalRelativePercent) > 1000.0 ? this.TextBoxFormat.VerticalPosition - this.TextBoxFormat.InternalMargin.Top + num1 : (float) (((double) num1 - (double) this.TextBoxFormat.InternalMargin.Top) * ((double) this.TextBoxFormat.VerticalRelativePercent / 100.0));
              break;
            case ShapeVerticalAlignment.Center:
              asTable.TableFormat.Positioning.VertPosition = (double) Math.Abs(this.TextBoxFormat.VerticalRelativePercent) > 1000.0 ? (float) (((double) num8 - (double) num5) / 2.0) : (float) ((double) num8 / 2.0 * ((double) this.TextBoxFormat.VerticalRelativePercent / 100.0));
              break;
            case ShapeVerticalAlignment.Bottom:
              asTable.TableFormat.Positioning.VertPosition = (double) Math.Abs(this.TextBoxFormat.VerticalRelativePercent) > 1000.0 ? num8 - num5 - this.TextBoxFormat.InternalMargin.Bottom - num2 : (float) (((double) num8 - (double) this.TextBoxFormat.InternalMargin.Bottom - (double) num2) * ((double) this.TextBoxFormat.VerticalRelativePercent / 100.0));
              break;
          }
          break;
        case VerticalOrigin.Page:
        case VerticalOrigin.TopMargin:
          asTable.TableFormat.Positioning.VertRelationTo = VerticalRelation.Page;
          switch (this.TextBoxFormat.VerticalAlignment)
          {
            case ShapeVerticalAlignment.None:
              asTable.TableFormat.Positioning.VertPosition = (double) Math.Abs(this.TextBoxFormat.VerticalRelativePercent) > 1000.0 ? this.TextBoxFormat.VerticalPosition : num6 * (this.TextBoxFormat.VerticalRelativePercent / 100f);
              break;
            case ShapeVerticalAlignment.Top:
              asTable.TableFormat.Positioning.VertPosition -= this.TextBoxFormat.InternalMargin.Top;
              break;
            case ShapeVerticalAlignment.Center:
              asTable.TableFormat.Positioning.VertPosition = (float) (((double) num6 - (double) num5) / 2.0);
              break;
            case ShapeVerticalAlignment.Bottom:
              asTable.TableFormat.Positioning.VertPosition = num6 - num5 - this.TextBoxFormat.InternalMargin.Bottom;
              break;
          }
          break;
        case VerticalOrigin.Paragraph:
        case VerticalOrigin.Line:
          asTable.TableFormat.Positioning.VertRelationTo = VerticalRelation.Paragraph;
          asTable.TableFormat.Positioning.VertPosition = this.TextBoxFormat.VerticalPosition;
          break;
        default:
          if ((double) asTable.TableFormat.Positioning.VertPosition == 0.0)
          {
            asTable.TableFormat.Positioning.VertPosition = this.TextBoxFormat.VerticalPosition;
            break;
          }
          break;
      }
      switch (this.TextBoxFormat.HorizontalOrigin)
      {
        case HorizontalOrigin.Margin:
          asTable.TableFormat.Positioning.HorizRelationTo = HorizontalRelation.Margin;
          switch (this.TextBoxFormat.HorizontalAlignment)
          {
            case ShapeHorizontalAlignment.None:
              asTable.TableFormat.Positioning.HorizPosition = (double) Math.Abs(this.TextBoxFormat.HorizontalRelativePercent) > 1000.0 ? this.TextBoxFormat.HorizontalPosition : num7 * (this.TextBoxFormat.HorizontalRelativePercent / 100f);
              break;
            case ShapeHorizontalAlignment.Left:
              asTable.TableFormat.Positioning.HorizPosition = (double) Math.Abs(this.TextBoxFormat.HorizontalRelativePercent) > 1000.0 ? asTable.TableFormat.LeftIndent - this.TextBoxFormat.InternalMargin.Left : (float) (((double) leftMargin - (double) this.TextBoxFormat.InternalMargin.Left) * ((double) this.TextBoxFormat.HorizontalRelativePercent / 100.0));
              break;
            case ShapeHorizontalAlignment.Center:
              asTable.TableFormat.Positioning.HorizPosition = (double) Math.Abs(this.TextBoxFormat.HorizontalRelativePercent) > 1000.0 ? (float) (((double) num7 - (double) shapeWidth) / 2.0) : (float) ((double) num7 / 2.0 * ((double) this.TextBoxFormat.HorizontalRelativePercent / 100.0));
              break;
            case ShapeHorizontalAlignment.Right:
              asTable.TableFormat.Positioning.HorizPosition = (double) Math.Abs(this.TextBoxFormat.HorizontalRelativePercent) > 1000.0 ? num7 - shapeWidth - this.TextBoxFormat.InternalMargin.Right : (float) (((double) num7 - (double) this.TextBoxFormat.InternalMargin.Right) * ((double) this.TextBoxFormat.HorizontalRelativePercent / 100.0));
              break;
          }
          break;
        case HorizontalOrigin.Page:
          asTable.TableFormat.Positioning.HorizRelationTo = HorizontalRelation.Page;
          switch (this.TextBoxFormat.HorizontalAlignment)
          {
            case ShapeHorizontalAlignment.None:
              asTable.TableFormat.Positioning.HorizPosition = (double) Math.Abs(this.TextBoxFormat.HorizontalRelativePercent) > 1000.0 ? this.TextBoxFormat.HorizontalPosition : pageWidth * (this.TextBoxFormat.HorizontalRelativePercent / 100f);
              break;
            case ShapeHorizontalAlignment.Left:
              asTable.TableFormat.Positioning.HorizPosition -= this.TextBoxFormat.InternalMargin.Left;
              break;
            case ShapeHorizontalAlignment.Center:
              asTable.TableFormat.Positioning.HorizPosition = (float) (((double) pageWidth - (double) shapeWidth) / 2.0);
              break;
            case ShapeHorizontalAlignment.Right:
              asTable.TableFormat.Positioning.HorizPosition = pageWidth - shapeWidth - this.TextBoxFormat.InternalMargin.Right;
              break;
          }
          break;
        case HorizontalOrigin.Column:
          asTable.TableFormat.Positioning.HorizRelationTo = HorizontalRelation.Column;
          switch (this.TextBoxFormat.HorizontalAlignment)
          {
            case ShapeHorizontalAlignment.None:
              asTable.TableFormat.Positioning.HorizPosition = this.TextBoxFormat.HorizontalPosition;
              break;
            case ShapeHorizontalAlignment.Left:
              asTable.TableFormat.Positioning.HorizPosition = asTable.TableFormat.LeftIndent - this.TextBoxFormat.InternalMargin.Left;
              break;
            case ShapeHorizontalAlignment.Center:
              asTable.TableFormat.Positioning.HorizPosition = (float) (((double) num7 - (double) shapeWidth) / 2.0);
              break;
            case ShapeHorizontalAlignment.Right:
              asTable.TableFormat.Positioning.HorizPosition = num7 - shapeWidth - this.TextBoxFormat.InternalMargin.Right;
              break;
          }
          break;
        case HorizontalOrigin.LeftMargin:
          asTable.TableFormat.Positioning.HorizPosition = this.GetLeftMarginHorizPosition(leftMargin, this.TextBoxFormat.HorizontalAlignment, this.TextBoxFormat.HorizontalPosition, shapeWidth, this.TextBoxFormat.TextWrappingStyle);
          break;
        case HorizontalOrigin.RightMargin:
          asTable.TableFormat.Positioning.HorizPosition = this.GetRightMarginHorizPosition(pageWidth, rightMargin, this.TextBoxFormat.HorizontalAlignment, this.TextBoxFormat.HorizontalPosition, shapeWidth, this.TextBoxFormat.TextWrappingStyle);
          break;
        case HorizontalOrigin.InsideMargin:
          asTable.TableFormat.Positioning.HorizPosition = currPageIndex % 2 != 0 ? this.GetLeftMarginHorizPosition(leftMargin, this.TextBoxFormat.HorizontalAlignment, this.TextBoxFormat.HorizontalPosition, shapeWidth, this.TextBoxFormat.TextWrappingStyle) : this.GetRightMarginHorizPosition(pageWidth, rightMargin, this.TextBoxFormat.HorizontalAlignment, this.TextBoxFormat.HorizontalPosition, shapeWidth, this.TextBoxFormat.TextWrappingStyle);
          break;
        case HorizontalOrigin.OutsideMargin:
          asTable.TableFormat.Positioning.HorizPosition = currPageIndex % 2 != 0 ? this.GetRightMarginHorizPosition(pageWidth, rightMargin, this.TextBoxFormat.HorizontalAlignment, this.TextBoxFormat.HorizontalPosition, shapeWidth, this.TextBoxFormat.TextWrappingStyle) : this.GetLeftMarginHorizPosition(leftMargin, this.TextBoxFormat.HorizontalAlignment, this.TextBoxFormat.HorizontalPosition, shapeWidth, this.TextBoxFormat.TextWrappingStyle);
          break;
        default:
          if ((double) asTable.TableFormat.Positioning.VertPosition == 0.0)
          {
            asTable.TableFormat.Positioning.VertPosition = this.TextBoxFormat.VerticalPosition;
            break;
          }
          break;
      }
      if (this.TextBoxFormat.HorizontalOrigin != HorizontalOrigin.Page && this.TextBoxFormat.HorizontalOrigin != HorizontalOrigin.Column)
        asTable.TableFormat.Positioning.HorizPosition += leftMargin;
    }
    if (this.TextBoxFormat.FillEfects.Type == BackgroundType.NoBackground)
      color2 = this.TextBoxFormat.TextWrappingStyle != TextWrappingStyle.InFrontOfText ? Color.Transparent : this.TextBoxFormat.FillColor;
    else if (this.TextBoxFormat.FillEfects.Type == BackgroundType.Gradient)
    {
      color2 = this.TextBoxFormat.FillEfects.Gradient.Color2;
      cell.CellFormat.TextureStyle = TextureStyle.Texture30Percent;
    }
    asTable.TableFormat.BackColor = color2;
    asTable.TableFormat.ForeColor = this.TextBoxFormat.TextThemeColor;
    asTable.TableFormat.Paddings.Left = this.TextBoxFormat.InternalMargin.Left;
    asTable.TableFormat.Paddings.Right = this.TextBoxFormat.InternalMargin.Right;
    asTable.TableFormat.Paddings.Top = this.TextBoxFormat.InternalMargin.Top;
    asTable.TableFormat.Paddings.Bottom = this.TextBoxFormat.InternalMargin.Bottom;
    cell.Width = shapeWidth;
    cell.CellFormat.BackColor = color2;
    cell.CellFormat.VerticalAlignment = this.TextBoxFormat.TextVerticalAlignment;
    asTable.Rows[0].HeightType = TableRowHeightType.Exactly;
    if ((double) this.TextBoxFormat.LineWidth < 1.0)
      row.RowFormat.Borders.BorderType = BorderStyle.None;
    int index = 0;
    for (int count = this.TextBoxBody.Items.Count; index < count; ++index)
    {
      TextBodyItem textBodyItem = this.TextBoxBody.Items[index];
      cell.Items.Add((IEntity) textBodyItem.Clone());
    }
    return asTable;
  }

  private float GetRightMarginHorizPosition(
    float pageWidth,
    float rightMargin,
    ShapeHorizontalAlignment horzAlignment,
    float horzPosition,
    float shapeWidth,
    TextWrappingStyle textWrapStyle)
  {
    float num = pageWidth - rightMargin;
    float marginHorizPosition = num + horzPosition;
    switch (horzAlignment)
    {
      case ShapeHorizontalAlignment.Left:
        marginHorizPosition = num;
        break;
      case ShapeHorizontalAlignment.Center:
        marginHorizPosition = num + (float) (((double) rightMargin - (double) shapeWidth) / 2.0);
        break;
      case ShapeHorizontalAlignment.Right:
        marginHorizPosition = pageWidth - shapeWidth;
        break;
    }
    if (((double) marginHorizPosition < 0.0 || (double) marginHorizPosition + (double) shapeWidth > (double) pageWidth) && textWrapStyle != TextWrappingStyle.InFrontOfText && textWrapStyle != TextWrappingStyle.Behind)
      marginHorizPosition = pageWidth - shapeWidth;
    return marginHorizPosition;
  }

  private float GetLeftMarginHorizPosition(
    float leftMargin,
    ShapeHorizontalAlignment horzAlignment,
    float horzPosition,
    float shapeWidth,
    TextWrappingStyle textWrapStyle)
  {
    float marginHorizPosition = horzPosition;
    switch (horzAlignment)
    {
      case ShapeHorizontalAlignment.Left:
        marginHorizPosition = 0.0f;
        break;
      case ShapeHorizontalAlignment.Center:
        marginHorizPosition = (float) (((double) leftMargin - (double) shapeWidth) / 2.0);
        break;
      case ShapeHorizontalAlignment.Right:
        marginHorizPosition = leftMargin - shapeWidth;
        break;
    }
    if ((double) marginHorizPosition < 0.0 && textWrapStyle != TextWrappingStyle.InFrontOfText && textWrapStyle != TextWrappingStyle.Behind)
      marginHorizPosition = 0.0f;
    return marginHorizPosition;
  }

  private RowAlignment GetHorAlign(ShapeHorizontalAlignment shapeAlign)
  {
    switch (shapeAlign)
    {
      case ShapeHorizontalAlignment.Center:
        return RowAlignment.Center;
      case ShapeHorizontalAlignment.Right:
        return RowAlignment.Right;
      default:
        return RowAlignment.Left;
    }
  }

  internal BorderStyle GetBordersStyle(TextBoxLineStyle lineStyle)
  {
    switch (lineStyle)
    {
      case TextBoxLineStyle.Simple:
        return BorderStyle.Single;
      case TextBoxLineStyle.Double:
        return BorderStyle.Double;
      case TextBoxLineStyle.ThickThin:
        return BorderStyle.ThickThinMediumGap;
      case TextBoxLineStyle.ThinThick:
        return BorderStyle.ThinThickMediumGap;
      case TextBoxLineStyle.Triple:
        return BorderStyle.Triple;
      default:
        return BorderStyle.None;
    }
  }

  internal void InitializeVMLDefaultValues()
  {
    this.TextBoxFormat.TextWrappingStyle = TextWrappingStyle.Inline;
    this.TextBoxFormat.FillColor = Color.White;
  }

  protected override void InitXDLSHolder()
  {
    this.XDLSHolder.AddElement("body", (object) this.TextBoxBody);
    this.XDLSHolder.AddElement("textbox-format", (object) this.TextBoxFormat);
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    writer.WriteValue("type", (Enum) ParagraphItemType.TextBox);
  }

  ILayoutInfo IWidget.LayoutInfo
  {
    get
    {
      if (this.m_layoutInfo == null)
        this.CreateLayoutInfo();
      return this.m_layoutInfo;
    }
  }

  SizeF ILeafWidget.Measure(DrawingContext dc)
  {
    float width = this.TextBoxFormat.Width;
    float height = this.TextBoxFormat.Height;
    if ((double) this.TextBoxFormat.WidthRelativePercent != 0.0)
      width = this.GetWidthRelativeToPercent(true);
    if ((double) this.TextBoxFormat.HeightRelativePercent != 0.0)
      height = this.GetHeightRelativeToPercent(true);
    return new SizeF(width, height);
  }

  void IWidget.InitLayoutInfo()
  {
    this.m_layoutInfo = (ILayoutInfo) null;
    this.TextBoxFormat.IsWrappingBoundsAdded = false;
  }

  void IWidget.InitLayoutInfo(IWidget widget)
  {
  }

  internal byte[] GetAsImage()
  {
    try
    {
      DocumentLayouter documentLayouter = new DocumentLayouter();
      byte[] asImage = documentLayouter.ConvertAsImage((IWidget) this);
      documentLayouter.Close();
      return asImage;
    }
    catch
    {
      return (byte[]) null;
    }
  }
}
