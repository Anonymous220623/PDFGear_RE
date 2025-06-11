// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WTableCell
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using Syncfusion.Layouting;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WTableCell : WTextBody, ICompositeEntity, IEntity, IWidget
{
  private CellFormat m_cellFormat;
  private WCharacterFormat m_charFormat;
  internal TextureStyle m_textureStyle;
  internal Color m_foreColor = Color.Empty;
  internal CellFormat m_trackCellFormat;
  private short m_gridStartIndex;
  private CellContentControl m_CellContentControl;
  private float m_cellStartPosition = float.MinValue;
  private float m_cellEndPosition = float.MinValue;
  private ColumnSizeInfo m_sizeInfo;
  private short m_gridSpan = 1;

  public short GridSpan
  {
    get => this.m_gridSpan;
    internal set => this.m_gridSpan = value;
  }

  internal short GridColumnStartIndex
  {
    get => this.m_gridStartIndex;
    set => this.m_gridStartIndex = value;
  }

  internal CellContentControl ContentControl
  {
    get => this.m_CellContentControl;
    set
    {
      this.m_CellContentControl = value;
      this.m_CellContentControl.OwnerCell = this;
    }
  }

  internal bool IsChangedFormat => this.OwnerRow != null && this.OwnerRow.RowFormat.IsChangedFormat;

  public override EntityType EntityType => EntityType.TableCell;

  public WTableRow OwnerRow => this.Owner as WTableRow;

  public CellFormat CellFormat => this.m_cellFormat;

  public float Width
  {
    get => this.CellFormat.CellWidth;
    set
    {
      float cellWidth = this.CellFormat.CellWidth;
      this.CellFormat.CellWidth = value;
      if (this.Document.IsOpening || this.OwnerRow == null || this.OwnerRow.OwnerTable == null)
        return;
      this.OwnerRow.OwnerTable.m_tableWidth = float.MinValue;
      this.OwnerRow.OwnerTable.IsTableGridUpdated = false;
      this.OwnerRow.OwnerTable.IsTableGridVerified = false;
      this.UpdateTablePreferredWidth(cellWidth, value);
    }
  }

  internal Color ForeColor
  {
    get => this.CellFormat.ForeColor;
    set => this.CellFormat.ForeColor = value;
  }

  internal TextureStyle TextureStyle
  {
    get => this.CellFormat.TextureStyle;
    set => this.CellFormat.TextureStyle = value;
  }

  internal WCharacterFormat CharacterFormat => this.m_charFormat;

  internal bool IsFixedWidth => (double) this.Width > -1.0;

  internal CellFormat TrackCellFormat
  {
    get
    {
      if (this.m_trackCellFormat == null)
      {
        this.m_trackCellFormat = new CellFormat();
        this.m_trackCellFormat.SetOwner((OwnerHolder) this);
      }
      return this.m_trackCellFormat;
    }
  }

  internal PreferredWidthInfo PreferredWidth => this.CellFormat.PreferredWidth;

  internal float CellStartPosition
  {
    get
    {
      if ((double) this.m_cellStartPosition == -3.4028234663852886E+38)
        this.m_cellStartPosition = this.GetCellStartPositionValue();
      return this.m_cellStartPosition;
    }
  }

  internal float CellEndPosition
  {
    get
    {
      if ((double) this.m_cellEndPosition == -3.4028234663852886E+38)
        this.m_cellEndPosition = this.GetCellEndPositionValue();
      return this.m_cellEndPosition;
    }
  }

  internal ColumnSizeInfo SizeInfo
  {
    get
    {
      if (this.m_sizeInfo == null)
        this.m_sizeInfo = new ColumnSizeInfo();
      return this.m_sizeInfo;
    }
  }

  public WTableCell(IWordDocument document)
    : base((WordDocument) document, (Entity) null)
  {
    this.m_cellFormat = new CellFormat();
    this.m_cellFormat.SetOwner((OwnerHolder) this);
    this.m_charFormat = new WCharacterFormat((IWordDocument) this.Document);
    this.m_charFormat.SetOwner((OwnerHolder) this);
  }

  public new Entity Clone() => (Entity) this.CloneImpl();

  public int GetCellIndex()
  {
    if (this.ContentControl == null || this.ContentControl.MappedCell == null)
      return this.GetIndexInOwnerCollection();
    int inOwnerCollection = this.GetIndexInOwnerCollection();
    return inOwnerCollection <= 0 ? 0 : inOwnerCollection;
  }

  protected override object CloneImpl()
  {
    WTableCell owner = (WTableCell) base.CloneImpl();
    owner.m_cellFormat = new CellFormat();
    owner.m_cellFormat.SetOwner((OwnerHolder) owner);
    owner.m_cellFormat.ImportContainer((FormatBase) this.m_cellFormat);
    owner.m_cellFormat.Paddings.ImportPaddings(this.m_cellFormat.Paddings);
    owner.m_charFormat = new WCharacterFormat((IWordDocument) this.Document);
    owner.m_charFormat.ImportContainer((FormatBase) this.CharacterFormat);
    owner.m_charFormat.SetOwner((OwnerHolder) owner);
    return (object) owner;
  }

  protected override void InitXDLSHolder()
  {
    base.InitXDLSHolder();
    this.XDLSHolder.AddElement("cell-format", (object) this.CellFormat);
    this.XDLSHolder.AddElement("character-format", (object) this.CharacterFormat);
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    if (this.m_cellFormat.OwnerRowFormat.HasSprms())
      return;
    if (this.IsFixedWidth)
      writer.WriteValue("Width", this.Width);
    if (this.ForeColor != Color.Empty)
      writer.WriteValue("ForeColor", this.ForeColor);
    writer.WriteValue("Texture", (Enum) this.TextureStyle);
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    base.ReadXmlAttributes(reader);
    if (reader.HasAttribute("Width"))
      this.Width = reader.ReadFloat("Width");
    if (reader.HasAttribute("ForeColor"))
      this.ForeColor = reader.ReadColor("ForeColor");
    if (!reader.HasAttribute("Texture"))
      return;
    this.TextureStyle = (TextureStyle) reader.ReadEnum("Texture", typeof (TextureStyle));
  }

  internal bool IsCellWidthZero()
  {
    return (this.PreferredWidth.WidthType < FtsWidth.Percentage || (double) this.PreferredWidth.Width == 0.0) && (double) this.Width == 0.0;
  }

  private void UpdateTablePreferredWidth(float prevWidth, float newValue)
  {
    if ((byte) this.OwnerRow.OwnerTable.PreferredTableWidth.WidthType > (byte) 1)
    {
      float num1 = (float) Math.Round((double) this.OwnerRow.GetRowWidth(), 2);
      if (this.OwnerRow.OwnerTable.PreferredTableWidth.WidthType == FtsWidth.Point && (double) this.OwnerRow.OwnerTable.PreferredTableWidth.Width < (double) num1)
        this.OwnerRow.OwnerTable.PreferredTableWidth.Width += newValue - prevWidth;
      else if (this.OwnerRow.OwnerTable.PreferredTableWidth.WidthType == FtsWidth.Percentage)
      {
        float ownerWidth = this.OwnerRow.OwnerTable.GetOwnerWidth();
        float num2 = (float) ((double) ownerWidth * (double) this.OwnerRow.OwnerTable.PreferredTableWidth.Width / 100.0);
        if ((double) num2 < (double) num1)
          this.OwnerRow.OwnerTable.PreferredTableWidth.Width = (float) (((double) num2 + (double) newValue - (double) prevWidth) / (double) ownerWidth * 100.0);
      }
    }
    if (this.CellFormat.HorizontalMerge == CellMerge.Start)
    {
      this.PreferredWidth.WidthType = FtsWidth.None;
      this.PreferredWidth.Width = 0.0f;
    }
    else if (this.PreferredWidth.WidthType == FtsWidth.Percentage)
    {
      float tableClientWidth = this.OwnerRow.OwnerTable.GetTableClientWidth(this.OwnerRow.OwnerTable.GetOwnerWidth());
      this.PreferredWidth.Width = (float) ((double) newValue / (double) tableClientWidth * 100.0);
    }
    else
    {
      this.PreferredWidth.WidthType = FtsWidth.Point;
      this.PreferredWidth.Width = newValue;
    }
  }

  internal void ApplyTableStyleBaseFormats(
    CellFormat cellFormat,
    WParagraphFormat paraFormat,
    WCharacterFormat charFormat,
    BodyItemCollection items)
  {
    this.CellFormat.ApplyBase((FormatBase) cellFormat);
    bool isOverrideFontSize = false;
    bool isOverrideFontSizeBidi = false;
    this.IsOverrideStyleHierarchy(ref isOverrideFontSize, ref isOverrideFontSizeBidi);
    for (int index1 = 0; index1 < items.Count; ++index1)
    {
      if (items[index1] is WParagraph)
      {
        WParagraph wparagraph = items[index1] as WParagraph;
        wparagraph.ParagraphFormat.TableStyleParagraphFormat = paraFormat;
        wparagraph.BreakCharacterFormat.TableStyleCharacterFormat = charFormat;
        if (isOverrideFontSize || isOverrideFontSizeBidi)
          this.IsFontSizeDefinedInStyle(wparagraph.ParaStyle, ref isOverrideFontSize, ref isOverrideFontSizeBidi);
        this.SetDocDefaultFontSize(wparagraph.BreakCharacterFormat, isOverrideFontSizeBidi, isOverrideFontSize);
        this.SetDocDefaultFontSize(wparagraph.BreakCharacterFormat.TableStyleCharacterFormat, isOverrideFontSizeBidi, isOverrideFontSize);
        for (int index2 = 0; index2 < wparagraph.Items.Count; ++index2)
        {
          if (wparagraph.Items[index2] != null)
          {
            WCharacterFormat paraItemCharFormat = wparagraph.Items[index2].ParaItemCharFormat;
            paraItemCharFormat.TableStyleCharacterFormat = charFormat;
            this.SetDocDefaultFontSize(paraItemCharFormat, isOverrideFontSizeBidi, isOverrideFontSize);
            this.SetDocDefaultFontSize(paraItemCharFormat.TableStyleCharacterFormat, isOverrideFontSizeBidi, isOverrideFontSize);
            if (wparagraph.Items[index2] is InlineContentControl)
              this.ApplyTableStyleBaseFormatsInlineCC(wparagraph.Items[index2] as InlineContentControl, charFormat, isOverrideFontSizeBidi, isOverrideFontSize);
          }
        }
      }
      else if (items[index1] is BlockContentControl)
      {
        BodyItemCollection items1 = (items[index1] as BlockContentControl).TextBody.Items;
        this.ApplyTableStyleBaseFormats(cellFormat, paraFormat, charFormat, items1);
      }
    }
  }

  private void ApplyTableStyleBaseFormatsInlineCC(
    InlineContentControl inlineControl,
    WCharacterFormat charFormat,
    bool isOverrideFontSizeBidi,
    bool isOverrideFontSize)
  {
    foreach (ParagraphItem paragraphItem in (CollectionImpl) inlineControl.ParagraphItems)
    {
      paragraphItem.ParaItemCharFormat.TableStyleCharacterFormat = charFormat;
      this.SetDocDefaultFontSize(paragraphItem.ParaItemCharFormat, isOverrideFontSizeBidi, isOverrideFontSize);
      if (paragraphItem is InlineContentControl)
      {
        inlineControl = paragraphItem as InlineContentControl;
        this.ApplyTableStyleBaseFormatsInlineCC(inlineControl, charFormat, isOverrideFontSizeBidi, isOverrideFontSize);
      }
    }
  }

  private bool IsTableStyleHasFormatting()
  {
    return !((this.Owner.Owner as WTable).GetStyle() is WTableStyle style) || style.ParagraphFormat == null || !this.IsNotEmptyParaFormat(style.ParagraphFormat) || style.CharacterFormat.HasKey(3) || style.CharacterFormat.HasKey(62);
  }

  private bool IsNotEmptyParaFormat(WParagraphFormat paraFormat)
  {
    return paraFormat.PropertiesHash.Count != 0 && !new WParagraphFormat((IWordDocument) this.Document).Compare(paraFormat);
  }

  private void IsOverrideStyleHierarchy(
    ref bool isOverrideFontSize,
    ref bool isOverrideFontSizeBidi)
  {
    if (this.Document.DefCharFormat != null && !this.IsTableStyleHasFormatting() && !this.Document.Settings.CompatibilityOptions[CompatibilityOption.overrideTableStyleFontSizeAndJustification] && !this.IsNormalStyleFontSizeNotInLimit(ref isOverrideFontSize, ref isOverrideFontSizeBidi))
      return;
    isOverrideFontSize = false;
    isOverrideFontSizeBidi = false;
  }

  private void IsFontSizeDefinedInStyle(
    IWParagraphStyle paraStyle,
    ref bool isOverrideFontSize,
    ref bool isOverrideFontSizeBidi)
  {
    for (; paraStyle is WParagraphStyle && !(paraStyle.Name == "Normal"); paraStyle = (IWParagraphStyle) (paraStyle as WParagraphStyle).BaseStyle)
    {
      if (paraStyle.CharacterFormat.HasKey(3))
        isOverrideFontSize = false;
      if (paraStyle.CharacterFormat.HasKey(62))
        isOverrideFontSizeBidi = false;
      if (!isOverrideFontSize || !isOverrideFontSizeBidi)
        break;
    }
  }

  private bool IsNormalStyleFontSizeNotInLimit(
    ref bool isOverrideFontSize,
    ref bool isOverrideFontSizeBidi)
  {
    WCharacterFormat characterFormat = this.Document.Styles.FindByName("Normal") is WParagraphStyle byName ? byName.CharacterFormat : (WCharacterFormat) null;
    if (characterFormat == null)
      return true;
    if (characterFormat.HasKey(3) && (double) (float) characterFormat.PropertiesHash[3] == 12.0)
      isOverrideFontSize = true;
    if (characterFormat.HasKey(62) && (double) (float) characterFormat.PropertiesHash[62] == 12.0)
      isOverrideFontSizeBidi = true;
    return !isOverrideFontSize && !isOverrideFontSizeBidi;
  }

  private void SetDocDefaultFontSize(
    WCharacterFormat charFormat,
    bool isOverrideFontSizeBidi,
    bool isOverrideFontSize)
  {
    if (isOverrideFontSize && this.UseDocDefaultFontSize(charFormat, (short) 3))
      charFormat.FontSize = charFormat.Document.DefCharFormat.FontSize;
    if (!isOverrideFontSizeBidi || !this.UseDocDefaultFontSize(charFormat, (short) 62))
      return;
    charFormat.FontSizeBidi = charFormat.Document.DefCharFormat.FontSizeBidi;
  }

  private bool UseDocDefaultFontSize(WCharacterFormat charFormat, short key)
  {
    return !charFormat.HasKey((int) key) && charFormat.Document.DefCharFormat.HasKey((int) key);
  }

  internal Entity CloneCell()
  {
    WTableCell owner = (WTableCell) base.CloneImpl();
    owner.m_cellFormat = new CellFormat();
    owner.m_cellFormat.SetOwner((OwnerHolder) owner);
    owner.m_cellFormat.ImportContainer((FormatBase) this.m_cellFormat);
    owner.m_cellFormat.Paddings.ImportPaddings(this.m_cellFormat.Paddings);
    owner.m_charFormat = new WCharacterFormat((IWordDocument) this.Document);
    owner.m_charFormat.ImportContainer((FormatBase) this.CharacterFormat);
    owner.m_charFormat.SetOwner((OwnerHolder) owner);
    return (Entity) owner;
  }

  internal TextBodyItem GetNextTextBodyItem()
  {
    if (this.NextSibling == null)
    {
      if (this.OwnerRow == null)
        return (TextBodyItem) null;
      if (this.OwnerRow.NextSibling == null)
        return this.OwnerRow.OwnerTable != null ? this.OwnerRow.OwnerTable.GetNextTextBodyItemValue() : (TextBodyItem) null;
      WTableRow wtableRow = this.OwnerRow;
      while (wtableRow.NextSibling != null)
      {
        wtableRow = wtableRow.NextSibling as WTableRow;
        foreach (WTableCell cell in (CollectionImpl) wtableRow.Cells)
        {
          if (cell.Items.Count > 0)
            return cell.Items[0];
        }
      }
      return this.OwnerRow.OwnerTable.GetNextTextBodyItemValue();
    }
    WTableCell nextSibling = this.NextSibling as WTableCell;
    return nextSibling.Items.Count > 0 ? nextSibling.Items[0] : nextSibling.GetNextTextBodyItem();
  }

  internal WTableCell GetVerticalMergeStartCell()
  {
    WTableCell verticalMergeStartCell = (WTableCell) null;
    int rowIndex = this.OwnerRow.GetRowIndex();
    int cellIndex = this.GetCellIndex();
    if (rowIndex > 0)
    {
      WTableRow row = this.OwnerRow.OwnerTable.Rows[rowIndex - 1];
      if (cellIndex == 0)
      {
        verticalMergeStartCell = row.Cells[0];
      }
      else
      {
        float num1 = 0.0f;
        float num2 = 0.0f;
        for (int index = 0; cellIndex > 0 && index <= cellIndex - 1; ++index)
          num1 += this.OwnerRow.Cells[index].Width;
        for (int index = 0; index < row.Cells.Count; ++index)
        {
          num2 += row.Cells[index].Width;
          if ((double) num1 == (double) num2 && index + 1 < row.Cells.Count)
          {
            verticalMergeStartCell = row.Cells[index + 1];
            break;
          }
        }
      }
      if (verticalMergeStartCell != null && verticalMergeStartCell.CellFormat.VerticalMerge == CellMerge.Continue)
        verticalMergeStartCell = verticalMergeStartCell.GetVerticalMergeStartCell();
      if (verticalMergeStartCell == null)
        verticalMergeStartCell = this;
    }
    return verticalMergeStartCell;
  }

  internal override void Close()
  {
    if (this.m_charFormat != null)
    {
      this.m_charFormat.Close();
      this.m_charFormat = (WCharacterFormat) null;
    }
    if (this.m_cellFormat != null)
    {
      this.m_cellFormat.Close();
      this.m_cellFormat = (CellFormat) null;
    }
    if (this.m_trackCellFormat != null)
    {
      this.m_trackCellFormat.Close();
      this.m_trackCellFormat = (CellFormat) null;
    }
    if (this.m_CellContentControl != null)
    {
      this.m_CellContentControl.Close();
      this.m_CellContentControl = (CellContentControl) null;
    }
    base.Close();
  }

  internal float GetCellWidth()
  {
    float width = this.Width;
    if (this.CellFormat.HorizontalMerge == CellMerge.Start)
    {
      int cellIndex = this.GetCellIndex();
      int mergeEndCellIndex = this.GetHorizontalMergeEndCellIndex();
      for (int index = cellIndex + 1; index <= mergeEndCellIndex; ++index)
        width += this.OwnerRow.Cells[index].Width;
    }
    return width;
  }

  internal float GetCellLayoutingWidth()
  {
    float cellLayoutingWidth = this.Width;
    if ((double) cellLayoutingWidth == 0.0)
    {
      float leftPadding = this.GetLeftPadding();
      float rightPadding = this.GetRightPadding();
      int inOwnerCollection = this.GetIndexInOwnerCollection();
      Border border1 = this.CellFormat.Borders.Left;
      if (!border1.IsBorderDefined || border1.IsBorderDefined && border1.BorderType == BorderStyle.None && (double) border1.LineWidth == 0.0 && border1.Color.IsEmpty)
        border1 = this.GetIndexInOwnerCollection() != 0 ? this.OwnerRow.RowFormat.Borders.Vertical : this.OwnerRow.RowFormat.Borders.Left;
      if (!border1.IsBorderDefined)
        border1 = this.GetIndexInOwnerCollection() != 0 ? this.OwnerRow.OwnerTable.TableFormat.Borders.Vertical : this.OwnerRow.OwnerTable.TableFormat.Borders.Left;
      Border border2 = this.CellFormat.Borders.Right;
      int num = this.OwnerRow.Cells.Count - 1;
      if (!border2.IsBorderDefined || border2.IsBorderDefined && border2.BorderType == BorderStyle.None && (double) border2.LineWidth == 0.0 && border2.Color.IsEmpty)
        border2 = inOwnerCollection != num ? this.OwnerRow.RowFormat.Borders.Vertical : this.OwnerRow.RowFormat.Borders.Right;
      if (!border2.IsBorderDefined)
        border2 = inOwnerCollection != num ? this.OwnerRow.OwnerTable.TableFormat.Borders.Vertical : this.OwnerRow.OwnerTable.TableFormat.Borders.Right;
      if (border1.IsBorderDefined && border1.BorderType != BorderStyle.None && border1.BorderType != BorderStyle.Cleared)
        leftPadding += border1.GetLineWidthValue() / 2f;
      if (border2.IsBorderDefined && border2.BorderType != BorderStyle.Cleared && border2.BorderType != BorderStyle.None)
        rightPadding += border2.GetLineWidthValue() / 2f;
      cellLayoutingWidth = leftPadding + rightPadding;
    }
    return cellLayoutingWidth;
  }

  private float GetCellStartPositionValue()
  {
    return this.GetCellIndex() == 0 || !(this.PreviousSibling is WTableCell) ? 0.0f : (this.PreviousSibling as WTableCell).CellEndPosition;
  }

  private float GetCellEndPositionValue()
  {
    int cellIndex = this.GetCellIndex();
    if (this.CellFormat.HorizontalMerge == CellMerge.Continue)
    {
      for (int index = cellIndex - 1; index >= 0; --index)
      {
        if (this.OwnerRow.Cells[index].CellFormat.HorizontalMerge == CellMerge.Start)
          return this.OwnerRow.Cells[index].CellEndPosition;
      }
    }
    float endPositionValue = this.CellStartPosition + this.GetCellLayoutingWidth();
    if (this.CellFormat.HorizontalMerge == CellMerge.Start)
    {
      int mergeEndCellIndex = this.GetHorizontalMergeEndCellIndex();
      for (int index = cellIndex + 1; index <= mergeEndCellIndex; ++index)
        endPositionValue += this.OwnerRow.Cells[index].GetCellLayoutingWidth();
    }
    return endPositionValue;
  }

  internal int GetHorizontalMergeEndCellIndex()
  {
    int cellIndex = this.GetCellIndex();
    int mergeEndCellIndex = cellIndex;
    for (int index = cellIndex + 1; index < this.OwnerRow.Cells.Count && this.OwnerRow.Cells[index].CellFormat.HorizontalMerge == CellMerge.Continue; ++index)
      ++mergeEndCellIndex;
    return mergeEndCellIndex;
  }

  internal WTableCell GetPreviousCell()
  {
    int index = this.Index - 1;
    WTableCell cell;
    for (cell = this.OwnerRow.Cells[index]; cell.CellFormat.HorizontalMerge == CellMerge.Continue && index != 0; cell = this.OwnerRow.Cells[index])
      --index;
    return cell;
  }

  internal float GetLeftPadding()
  {
    float leftPadding = this.CellFormat.Paddings.Left;
    if (!this.CellFormat.Paddings.HasKey(1) || (double) leftPadding == -0.05000000074505806)
      leftPadding = !this.OwnerRow.RowFormat.Paddings.HasKey(1) ? (!this.OwnerRow.OwnerTable.TableFormat.Paddings.HasKey(1) ? (this.Document.ActualFormatType != FormatType.Doc ? (!(this.OwnerRow.OwnerTable.GetStyle() is WTableStyle style) || !style.TableProperties.Paddings.HasKey(1) ? 5.4f : style.TableProperties.Paddings.Left) : 0.0f) : this.OwnerRow.OwnerTable.TableFormat.Paddings.Left) : this.OwnerRow.RowFormat.Paddings.Left;
    return leftPadding;
  }

  internal float GetRightPadding()
  {
    float rightPadding = this.CellFormat.Paddings.Right;
    if (!this.CellFormat.Paddings.HasKey(4) || (double) rightPadding == -0.05000000074505806)
      rightPadding = !this.OwnerRow.RowFormat.Paddings.HasKey(4) ? (!this.OwnerRow.OwnerTable.TableFormat.Paddings.HasKey(4) ? (this.Document.ActualFormatType != FormatType.Doc ? (!(this.OwnerRow.OwnerTable.GetStyle() is WTableStyle style) || !style.TableProperties.Paddings.HasKey(4) ? 5.4f : style.TableProperties.Paddings.Right) : 0.0f) : this.OwnerRow.OwnerTable.TableFormat.Paddings.Right) : this.OwnerRow.RowFormat.Paddings.Right;
    return rightPadding;
  }

  internal float GetTopPadding()
  {
    float topPadding = this.CellFormat.Paddings.Top;
    if ((double) this.CellFormat.Paddings.Top == -0.05000000074505806 || (double) this.CellFormat.Paddings.Top == 0.0 && !this.CellFormat.Paddings.HasKey(2))
      topPadding = !this.OwnerRow.RowFormat.Paddings.HasKey(2) ? (!this.OwnerRow.OwnerTable.TableFormat.Paddings.HasKey(2) ? (this.Document.ActualFormatType == FormatType.Doc || !(this.OwnerRow.OwnerTable.GetStyle() is WTableStyle style) || !style.TableProperties.Paddings.HasKey(2) ? 0.0f : style.TableProperties.Paddings.Top) : this.OwnerRow.OwnerTable.TableFormat.Paddings.Top) : this.OwnerRow.RowFormat.Paddings.Top;
    return topPadding;
  }

  internal float GetBottomPadding()
  {
    float bottomPadding = this.CellFormat.Paddings.Bottom;
    if ((double) this.CellFormat.Paddings.Bottom == -0.05000000074505806 || (double) this.CellFormat.Paddings.Bottom == 0.0 && !this.CellFormat.Paddings.HasKey(3))
      bottomPadding = !this.OwnerRow.RowFormat.Paddings.HasKey(3) ? (!this.OwnerRow.OwnerTable.TableFormat.Paddings.HasKey(3) ? (this.Document.ActualFormatType == FormatType.Doc || !(this.OwnerRow.OwnerTable.GetStyle() is WTableStyle style) || !style.TableProperties.Paddings.HasKey(3) ? 0.0f : style.TableProperties.Paddings.Bottom) : this.OwnerRow.OwnerTable.TableFormat.Paddings.Bottom) : this.OwnerRow.RowFormat.Paddings.Bottom;
    return bottomPadding;
  }

  internal ColumnSizeInfo GetSizeInfo(
    bool isAutoFit,
    bool isAutoWidth,
    bool needtoCalculateParaWidth)
  {
    bool flag1 = true;
    bool flag2 = true;
    float minimumWordWidth = 0.0f;
    float maximumWordWidth = 0.0f;
    float paragraphWidth = 0.0f;
    if (!this.SizeInfo.HasMinimumWidth || flag2)
      this.SizeInfo.MinimumWidth = this.GetMinimumPreferredWidth();
    if ((isAutoFit || isAutoWidth) && (!this.SizeInfo.HasMinimumMaximumWordWidth || flag1))
    {
      this.GetMinimumAndMaximumWordWidth(this.Items, ref minimumWordWidth, ref maximumWordWidth, ref paragraphWidth, needtoCalculateParaWidth);
      float cellSpacing = this.OwnerRow.OwnerTable.TableFormat.GetCellSpacing();
      this.SizeInfo.MinimumWordWidth = (float) ((double) minimumWordWidth + (double) this.SizeInfo.MinimumWidth + (double) cellSpacing * 3.0);
      this.SizeInfo.MaximumWordWidth = (float) ((double) maximumWordWidth + (double) this.SizeInfo.MinimumWidth + (double) cellSpacing * 3.0);
      this.SizeInfo.MaxParaWidth = (float) ((double) paragraphWidth + (double) this.SizeInfo.MinimumWidth + (double) cellSpacing * 3.0);
    }
    ColumnSizeInfo sizeInfo = new ColumnSizeInfo();
    sizeInfo.MinimumWidth = this.SizeInfo.MinimumWidth;
    sizeInfo.MinimumWordWidth = this.SizeInfo.MinimumWordWidth;
    if ((this.CellFormat.TextDirection == TextDirection.HorizontalFarEast || this.CellFormat.TextDirection == TextDirection.Horizontal) && this.CellFormat.HorizontalMerge == CellMerge.None)
    {
      sizeInfo.MaximumWordWidth = this.SizeInfo.MaximumWordWidth;
      sizeInfo.HasMaximumWordWidth = (double) maximumWordWidth > 0.0;
      this.SizeInfo.HasMaximumWordWidth = (double) maximumWordWidth > 0.0;
    }
    sizeInfo.MaxParaWidth = this.SizeInfo.MaxParaWidth;
    return sizeInfo;
  }

  private void GetMinimumAndMaximumWordWidth(
    BodyItemCollection bodyItemCollection,
    ref float minimumWordWidth,
    ref float maximumWordWidth,
    ref float paragraphWidth,
    bool needtoCalculateParaWidth)
  {
    for (int index = 0; index < bodyItemCollection.Count; ++index)
      this.GetMinimumAndMaximumWordWidth(bodyItemCollection[index], ref minimumWordWidth, ref maximumWordWidth, ref paragraphWidth, needtoCalculateParaWidth);
  }

  private void GetMinimumAndMaximumWordWidth(
    TextBodyItem textBodyItem,
    ref float minimumWordWidth,
    ref float maximumWordWidth,
    ref float paragraphWidth,
    bool needtoCalculateParaWidth)
  {
    switch (textBodyItem.EntityType)
    {
      case EntityType.Paragraph:
        (textBodyItem as WParagraph).GetMinimumAndMaximumWordWidth(ref minimumWordWidth, ref maximumWordWidth, ref paragraphWidth, needtoCalculateParaWidth);
        break;
      case EntityType.BlockContentControl:
        this.GetMinimumAndMaximumWordWidth((textBodyItem as BlockContentControl).TextBody.Items, ref minimumWordWidth, ref maximumWordWidth, ref paragraphWidth, needtoCalculateParaWidth);
        break;
      case EntityType.Table:
        (textBodyItem as WTable).GetMinimumAndMaximumWordWidth(ref minimumWordWidth, ref maximumWordWidth);
        break;
    }
  }

  internal float GetMinimumPreferredWidth()
  {
    float num = this.GetLeftPadding() + this.GetRightPadding() + this.OwnerRow.OwnerTable.TableFormat.GetCellSpacing();
    float lineWidthValue1 = this.CellFormat.Borders.Left.GetLineWidthValue();
    if ((double) lineWidthValue1 < (double) this.OwnerRow.OwnerTable.TableFormat.Borders.Left.LineWidth)
      lineWidthValue1 = this.OwnerRow.OwnerTable.TableFormat.Borders.Left.GetLineWidthValue();
    else if ((double) lineWidthValue1 < (double) this.OwnerRow.OwnerTable.TableFormat.Borders.Vertical.LineWidth)
      lineWidthValue1 = this.OwnerRow.OwnerTable.TableFormat.Borders.Vertical.GetLineWidthValue();
    else if ((double) lineWidthValue1 < (double) this.OwnerRow.RowFormat.Borders.Left.LineWidth)
      lineWidthValue1 = this.OwnerRow.RowFormat.Borders.Left.GetLineWidthValue();
    else if ((double) lineWidthValue1 < (double) this.OwnerRow.RowFormat.Borders.Vertical.LineWidth)
      lineWidthValue1 = this.OwnerRow.RowFormat.Borders.Vertical.GetLineWidthValue();
    float lineWidthValue2 = this.CellFormat.Borders.Right.GetLineWidthValue();
    if ((double) lineWidthValue2 < (double) this.OwnerRow.OwnerTable.TableFormat.Borders.Right.LineWidth)
      lineWidthValue2 = this.OwnerRow.OwnerTable.TableFormat.Borders.Right.GetLineWidthValue();
    else if ((double) lineWidthValue2 < (double) this.OwnerRow.OwnerTable.TableFormat.Borders.Vertical.LineWidth)
      lineWidthValue2 = this.OwnerRow.OwnerTable.TableFormat.Borders.Vertical.GetLineWidthValue();
    else if ((double) lineWidthValue2 < (double) this.OwnerRow.RowFormat.Borders.Right.LineWidth)
      lineWidthValue2 = this.OwnerRow.RowFormat.Borders.Right.GetLineWidthValue();
    else if ((double) lineWidthValue2 < (double) this.OwnerRow.RowFormat.Borders.Vertical.LineWidth)
      lineWidthValue2 = this.OwnerRow.RowFormat.Borders.Vertical.GetLineWidthValue();
    return num + (float) ((double) lineWidthValue1 / 2.0 + (double) lineWidthValue2 / 2.0);
  }

  internal bool IsFitAsPerMaximumWordWidth(float width, float maxWordWidth)
  {
    WTable ownerTable = this.OwnerRow.OwnerTable;
    WSection ownerSection = this.GetOwnerSection((Entity) this) as WSection;
    return (this.m_doc.ActualFormatType == FormatType.Docx || this.m_doc.ActualFormatType == FormatType.Word2013) && ownerTable != null && !ownerTable.IsInCell && !ownerTable.TableFormat.WrapTextAround && ownerTable.TableFormat.IsAutoResized && ownerTable.PreferredTableWidth.WidthType == FtsWidth.Point && (double) ownerTable.PreferredTableWidth.Width > (double) ownerSection.PageSetup.ClientWidth && this.GridSpan == (short) 1 && this.SizeInfo.HasMaximumWordWidth && (double) maxWordWidth > (double) width && this.CellFormat.VerticalMerge == CellMerge.None || (this.m_doc.ActualFormatType == FormatType.Docx || this.m_doc.ActualFormatType == FormatType.Word2013) && ownerSection != null && (double) maxWordWidth < (double) ownerSection.PageSetup.ClientWidth && ownerTable.Rows.Count == 1 && ownerTable.Rows[0].Cells.Count == 1 && ownerTable.PreferredTableWidth.WidthType == FtsWidth.Auto && this.PreferredWidth.WidthType == FtsWidth.Auto && ownerTable.TableFormat.IsAutoResized && !ownerTable.IsInCell && !ownerTable.TableFormat.WrapTextAround;
  }

  protected override IEntityCollectionBase WidgetCollection
  {
    get
    {
      if (this.m_CellContentControl == null || this.m_CellContentControl.MappedCell == null || this.m_CellContentControl.MappedCell.ChildEntities.Count <= 0)
        return (IEntityCollectionBase) this.m_bodyItems;
      this.m_CellContentControl.MappedCell.SetOwner((OwnerHolder) this.OwnerRow);
      this.m_CellContentControl.MappedCell.OwnerRow.SetOwner((OwnerHolder) this.OwnerRow.OwnerTable);
      this.m_CellContentControl.MappedCell.CellFormat.SetOwner((OwnerHolder) this);
      this.m_CellContentControl.MappedCell.CharacterFormat.SetOwner((OwnerHolder) this);
      if (this.m_CellContentControl.Paragraphs.Count > 0 && this.Paragraphs[0].ChildEntities[0] is ParagraphItem)
        (this.m_CellContentControl.Paragraphs[0].Items[0] as WTextRange).ApplyCharacterFormat((this.Paragraphs[0].ChildEntities[0] as ParagraphItem).ParaItemCharFormat);
      return (IEntityCollectionBase) this.m_CellContentControl.MappedCell.m_bodyItems;
    }
  }

  protected override void CreateLayoutInfo()
  {
    this.m_layoutInfo = (ILayoutInfo) new CellLayoutInfo(this);
    this.CheckFootNoteInTextBody((WTextBody) this);
    if ((this.m_layoutInfo as CellLayoutInfo).IsColumnMergeContinue)
      this.m_layoutInfo.IsSkip = true;
    if (!(this.m_layoutInfo as CellLayoutInfo).IsVerticalText)
      return;
    this.m_layoutInfo.IsClipped = true;
  }

  internal CellLayoutInfo CreateCellLayoutInfo()
  {
    this.CreateLayoutInfo();
    return this.m_layoutInfo as CellLayoutInfo;
  }

  internal void InitCellLayoutInfo()
  {
    if (this.m_layoutInfo is CellLayoutInfo)
      (this.m_layoutInfo as CellLayoutInfo).InitLayoutInfo();
    this.m_layoutInfo = (ILayoutInfo) null;
  }

  internal override void InitLayoutInfo(Entity entity, ref bool isLastTOCEntry)
  {
    if (this.m_layoutInfo is CellLayoutInfo)
      (this.m_layoutInfo as CellLayoutInfo).InitLayoutInfo();
    this.m_layoutInfo = (ILayoutInfo) null;
    base.InitLayoutInfo(entity, ref isLastTOCEntry);
    int num = isLastTOCEntry ? 1 : 0;
  }

  internal WTableRow GetOwnerRow(WTableCell ownerTableCell)
  {
    if (!(((IWidget) ownerTableCell).LayoutInfo as CellLayoutInfo).IsRowMergeStart)
      return ownerTableCell.OwnerRow;
    WTable ownerTable = ownerTableCell.OwnerRow.OwnerTable;
    float cellStartPosition = ownerTableCell.CellStartPosition;
    for (int index1 = ownerTableCell.OwnerRow.Index; index1 < ownerTable.Rows.Count; ++index1)
    {
      WTableRow row = ownerTable.Rows[index1];
      float num = 0.0f;
      if ((((IWidget) row).LayoutInfo as RowLayoutInfo).IsRowHasVerticalMergeEndCell)
      {
        for (int index2 = 0; index2 < row.Cells.Count; ++index2)
        {
          if ((((IWidget) row.Cells[index2]).LayoutInfo as CellLayoutInfo).IsRowMergeEnd && Math.Round((double) num, 2) == Math.Round((double) cellStartPosition, 2))
            return row.Cells[index2].OwnerRow;
          num += ownerTable.Rows[index1].Cells[index2].Width;
        }
      }
    }
    return (WTableRow) null;
  }

  private void CheckFootNoteInTextBody(WTextBody textBody)
  {
    for (int index1 = 0; index1 < textBody.Items.Count; ++index1)
    {
      if (textBody.Items[index1] is WParagraph)
        this.CheckFootNoteInParagraph(textBody.Items[index1] as WParagraph);
      else if (textBody.Items[index1] is WTable)
      {
        WTable wtable = textBody.Items[index1] as WTable;
        for (int index2 = 0; index2 < wtable.Rows.Count; ++index2)
        {
          WTableRow row = wtable.Rows[index2];
          for (int index3 = 0; index3 < row.Cells.Count; ++index3)
            this.CheckFootNoteInTextBody((WTextBody) row.Cells[index3]);
        }
      }
    }
  }

  private void CheckFootNoteInParagraph(WParagraph paragraph)
  {
    for (int index = 0; index < paragraph.Items.Count; ++index)
    {
      if (paragraph.Items[index] is WFootnote)
      {
        ILayoutInfo layoutInfo = ((IWidget) paragraph.Items[index]).LayoutInfo;
        if (paragraph.Items[index] is WFootnote)
        {
          if ((paragraph.Items[index] as WFootnote).FootnoteType == FootnoteType.Endnote)
            (this.m_layoutInfo as CellLayoutInfo).IsCellHasEndNote = true;
          if ((paragraph.Items[index] as WFootnote).FootnoteType == FootnoteType.Footnote)
            (this.m_layoutInfo as CellLayoutInfo).IsCellHasFootNote = true;
        }
      }
    }
  }

  void IWidget.InitLayoutInfo()
  {
    if (this.m_layoutInfo is CellLayoutInfo)
      (this.m_layoutInfo as CellLayoutInfo).InitLayoutInfo();
    this.m_layoutInfo = (ILayoutInfo) null;
  }

  void IWidget.InitLayoutInfo(IWidget widget)
  {
  }

  internal IWParagraph AddPrevParagraph()
  {
    return this.m_bodyItems[this.m_bodyItems.Add((IEntity) this.m_doc.LastParagraph)] as IWParagraph;
  }
}
