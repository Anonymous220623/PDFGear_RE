// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WTable
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using Syncfusion.Layouting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WTable : TextBodyItem, IWTable, ICompositeEntity, IEntity, ITableWidget, IWidget
{
  private const string DEF_NORMAL_STYLE = "Normal Table";
  private const int DEF_USER_STYLE_ID = 4094;
  private WRowCollection m_rows;
  private RowFormat m_initTableFormat;
  internal float m_tableWidth = float.MinValue;
  private WTableColumnCollection m_tableGrid;
  private XmlTableFormat m_xmlTblFormat;
  internal XmlTableFormat m_trackTblFormat;
  internal WTableColumnCollection m_trackTableGrid;
  private IWTableStyle m_style;
  private string m_title;
  private string m_description;
  private short m_bFlags = 21;
  private byte m_bFlags1 = 144 /*0x90*/;
  internal List<WTable> m_recalculateTables;

  public override EntityType EntityType => EntityType.Table;

  public WRowCollection Rows => this.m_rows;

  public RowFormat TableFormat
  {
    get
    {
      if (this.m_initTableFormat == null)
      {
        this.m_initTableFormat = new RowFormat();
        this.m_initTableFormat.SetOwner((OwnerHolder) this);
      }
      if (this.m_initTableFormat.IsDefault && this.Rows != null && this.Rows.Count > 0)
      {
        this.m_initTableFormat.ImportContainer((FormatBase) this.FirstRow.RowFormat);
        this.m_initTableFormat.RemoveRowSprms();
      }
      return this.m_initTableFormat;
    }
  }

  internal PreferredWidthInfo PreferredTableWidth => this.TableFormat.PreferredWidth;

  public string StyleName => this.m_style == null ? (string) null : this.m_style.Name;

  public WTableCell LastCell
  {
    get
    {
      WTableRow lastRow = this.LastRow;
      if (lastRow == null)
        return (WTableCell) null;
      int count = lastRow.Cells.Count;
      return count <= 0 ? (WTableCell) null : lastRow.Cells[count - 1];
    }
  }

  public WTableRow FirstRow => this.Rows.FirstItem as WTableRow;

  public WTableRow LastRow => this.Rows.LastItem as WTableRow;

  public WTableCell this[int row, int column] => this.Rows[row].Cells[column];

  public float Width
  {
    get
    {
      if ((double) this.m_tableWidth == -3.4028234663852886E+38)
        this.m_tableWidth = this.UpdateWidth();
      return this.m_tableWidth;
    }
  }

  public EntityCollection ChildEntities => (EntityCollection) this.m_rows;

  internal WTableColumnCollection TableGrid
  {
    get
    {
      if (!this.IsTableGridUpdated)
        this.m_tableGrid = (WTableColumnCollection) null;
      else if (this.m_rows != null && this.m_rows.Count > 0 && !this.Document.IsOpening && !this.Document.IsCloning && !this.Document.IsMailMerge && (this.Document.ActualFormatType.ToString().Contains("Docx") || this.Document.ActualFormatType.ToString().Contains("Word")) && (!this.IsTableGridVerified || this.IsTableGridCorrupted))
        this.CheckTableGrid();
      if (this.m_tableGrid == null)
      {
        if (this.m_rows != null && this.m_rows.Count > 0)
          this.UpdateTableGrid(false, true);
        else
          this.m_tableGrid = new WTableColumnCollection(this.Document);
        this.IsTableGridUpdated = true;
      }
      return this.m_tableGrid;
    }
  }

  internal void ChangeTrackTableGrid()
  {
    this.TableGrid.Close();
    this.m_tableGrid = new WTableColumnCollection(this.Document);
    foreach (WTableColumn wtableColumn in (CollectionImpl) this.TrackTableGrid)
      this.m_tableGrid.AddColumns(wtableColumn.EndOffset);
    this.m_trackTableGrid = (WTableColumnCollection) null;
  }

  public float IndentFromLeft
  {
    get => this.TableFormat.LeftIndent;
    set
    {
      if ((double) value > 1080.0 || (double) value < -1080.0)
        throw new ArgumentOutOfRangeException(nameof (IndentFromLeft), "Table Indent must be between -1080 and 1080");
      foreach (WTableRow row in (CollectionImpl) this.Rows)
        row.RowFormat.LeftIndent = value;
      this.TableFormat.LeftIndent = value;
    }
  }

  internal XmlTableFormat DocxTableFormat
  {
    get
    {
      if (this.m_xmlTblFormat == null)
        this.m_xmlTblFormat = new XmlTableFormat(this);
      return this.m_xmlTblFormat;
    }
    set => this.m_xmlTblFormat = value;
  }

  internal XmlTableFormat TrackTblFormat
  {
    get
    {
      if (this.m_trackTblFormat == null)
        this.m_trackTblFormat = new XmlTableFormat(this);
      return this.m_trackTblFormat;
    }
  }

  internal WTableColumnCollection TrackTableGrid
  {
    get
    {
      if (this.m_trackTableGrid == null)
        this.m_trackTableGrid = new WTableColumnCollection(this.Document);
      return this.m_trackTableGrid;
    }
  }

  public bool ApplyStyleForHeaderRow
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (short) ((int) this.m_bFlags & 65534 | (value ? 1 : 0));
  }

  public bool ApplyStyleForLastRow
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (short) ((int) this.m_bFlags & 65533 | (value ? 1 : 0) << 1);
  }

  public bool ApplyStyleForFirstColumn
  {
    get => ((int) this.m_bFlags & 4) >> 2 != 0;
    set => this.m_bFlags = (short) ((int) this.m_bFlags & 65531 | (value ? 1 : 0) << 2);
  }

  public bool ApplyStyleForLastColumn
  {
    get => ((int) this.m_bFlags & 8) >> 3 != 0;
    set => this.m_bFlags = (short) ((int) this.m_bFlags & 65527 | (value ? 1 : 0) << 3);
  }

  public bool ApplyStyleForBandedRows
  {
    get => ((int) this.m_bFlags & 16 /*0x10*/) >> 4 != 0;
    set => this.m_bFlags = (short) ((int) this.m_bFlags & 65519 | (value ? 1 : 0) << 4);
  }

  public bool ApplyStyleForBandedColumns
  {
    get => ((int) this.m_bFlags & 32 /*0x20*/) >> 5 != 0;
    set => this.m_bFlags = (short) ((int) this.m_bFlags & 65503 | (value ? 1 : 0) << 5);
  }

  public string Title
  {
    get => this.m_title;
    set => this.m_title = value;
  }

  public string Description
  {
    get => this.m_description;
    set => this.m_description = value;
  }

  internal bool IsFrame
  {
    get
    {
      return this.Rows.Count > 0 && this.Rows[0].Cells.Count > 0 && this.Rows[0].Cells[0].Paragraphs.Count > 0 && this.Rows[0].Cells[0].Paragraphs[0].ParagraphFormat.IsFrame;
    }
  }

  internal bool IsCompleteFrame
  {
    get
    {
      bool isCompleteFrame = false;
      foreach (WTableRow row in (CollectionImpl) this.Rows)
      {
        if (row.Cells.Count > 0 && row.Cells[0].Paragraphs.Count > 0)
          isCompleteFrame = row.Cells[0].Paragraphs[0].ParagraphFormat.IsFrame;
      }
      return isCompleteFrame;
    }
  }

  internal bool IsTableGridUpdated
  {
    get => ((int) this.m_bFlags & 64 /*0x40*/) >> 6 != 0;
    set => this.m_bFlags = (short) ((int) this.m_bFlags & 65471 | (value ? 1 : 0) << 6);
  }

  internal bool IsTableGridVerified
  {
    get => ((int) this.m_bFlags & 256 /*0x0100*/) >> 8 != 0;
    set => this.m_bFlags = (short) ((int) this.m_bFlags & 65279 | (value ? 1 : 0) << 8);
  }

  internal bool IsTableGridCorrupted
  {
    get => ((int) this.m_bFlags & 128 /*0x80*/) >> 7 != 0;
    set => this.m_bFlags = (short) ((int) this.m_bFlags & 65407 | (value ? 1 : 0) << 7);
  }

  internal bool IsTableCellWidthDefined
  {
    get => ((int) this.m_bFlags & 512 /*0x0200*/) >> 9 != 0;
    set => this.m_bFlags = (short) ((int) this.m_bFlags & 65023 | (value ? 1 : 0) << 9);
  }

  internal bool IsUpdateCellWidthByPartitioning
  {
    get => ((int) this.m_bFlags & 1024 /*0x0400*/) >> 10 != 0;
    set
    {
      bool flag = value && this.IsCellWidthZero();
      if (flag)
      {
        this.IsTableGridUpdated = false;
        this.IsTableGridVerified = false;
      }
      this.m_bFlags = (short) ((int) this.m_bFlags & 64511 | (flag ? 1 : 0) << 10);
    }
  }

  internal bool UsePreferredCellWidth
  {
    get => ((int) this.m_bFlags & 2048 /*0x0800*/) >> 11 != 0;
    set => this.m_bFlags = (short) ((int) this.m_bFlags & 63487 | (value ? 1 : 0) << 11);
  }

  internal bool IsInCell
  {
    get
    {
      if (this.Owner is WTableCell)
        return true;
      return this.Owner is WTextBody && (this.Owner as WTextBody).Owner is BlockContentControl && this.IsSDTInTableCell((this.Owner as WTextBody).Owner as BlockContentControl);
    }
  }

  internal bool IsAllCellsHavePointWidth
  {
    get => ((int) this.m_bFlags & 4096 /*0x1000*/) >> 12 != 0;
    set => this.m_bFlags = (short) ((int) this.m_bFlags & 61439 /*0xEFFF*/ | (value ? 1 : 0) << 12);
  }

  internal bool HasOnlyParagraphs
  {
    get => ((int) this.m_bFlags & 8192 /*0x2000*/) >> 13 != 0;
    set => this.m_bFlags = (short) ((int) this.m_bFlags & 57343 /*0xDFFF*/ | (value ? 1 : 0) << 13);
  }

  internal bool IsRecalulateBasedOnLastCol
  {
    get => ((int) this.m_bFlags & 16384 /*0x4000*/) >> 14 != 0;
    set => this.m_bFlags = (short) ((int) this.m_bFlags & 49151 /*0xBFFF*/ | (value ? 1 : 0) << 14);
  }

  internal bool HasPercentPreferredCellWidth
  {
    get => ((int) this.m_bFlags1 & 2) >> 1 != 0;
    set => this.m_bFlags1 = (byte) ((int) this.m_bFlags1 & 253 | (value ? 1 : 0) << 1);
  }

  internal bool HasAutoPreferredCellWidth
  {
    get => ((int) this.m_bFlags1 & 4) >> 2 != 0;
    set => this.m_bFlags1 = (byte) ((int) this.m_bFlags1 & 251 | (value ? 1 : 0) << 2);
  }

  internal bool HasNonePreferredCellWidth
  {
    get => ((int) this.m_bFlags1 & 8) >> 3 != 0;
    set => this.m_bFlags1 = (byte) ((int) this.m_bFlags1 & 247 | (value ? 1 : 0) << 3);
  }

  internal bool HasPointPreferredCellWidth
  {
    get => ((int) this.m_bFlags1 & 1) != 0;
    set => this.m_bFlags1 = (byte) ((int) this.m_bFlags1 & 254 | (value ? 1 : 0));
  }

  internal bool IsNeedToRecalculate
  {
    get => ((int) this.m_bFlags1 & 32 /*0x20*/) >> 5 != 0;
    set => this.m_bFlags1 = (byte) ((int) this.m_bFlags1 & 223 | (value ? 1 : 0) << 5);
  }

  internal List<WTable> RecalculateTables
  {
    get
    {
      if (this.m_recalculateTables == null)
        this.m_recalculateTables = new List<WTable>();
      return this.m_recalculateTables;
    }
  }

  public WTable(IWordDocument doc)
    : this(doc, false)
  {
    if (this.Document.IsOpening || this.Document.IsCloning || this.Document.IsMailMerge)
      return;
    IStyleCollection styles = this.Document.Styles;
    IStyle style = styles.FindByName(nameof (TableGrid));
    if (style != null && style.StyleType != StyleType.TableStyle)
      this.RenameDupliateStyle(style, styles);
    if (!(style is WTableStyle))
    {
      style = styles.FindByName("Table Grid");
      if (style != null && style.StyleType != StyleType.TableStyle)
        this.RenameDupliateStyle(style, styles);
    }
    if (!(style is WTableStyle))
      style = (IStyle) this.CreateTableGridStyle(styles);
    if (!(style is WTableStyle))
      return;
    this.ApplyStyle(style.Name, false);
  }

  private void RenameDupliateStyle(IStyle style, IStyleCollection docStyles)
  {
    int num = 0;
    string name;
    for (name = $"{style.Name}_{(object) num}"; docStyles.FindByName(name) != null; name = $"{style.Name}_{(object) num}")
      ++num;
    style.Name = name;
  }

  private WTableStyle CreateTableGridStyle(IStyleCollection docStyles)
  {
    WTableStyle tableGridStyle = new WTableStyle((IWordDocument) this.Document);
    tableGridStyle.Name = "Table Grid";
    string[] strArray = new string[4]
    {
      "Normal Table",
      "NormalTable",
      "Table Normal",
      "TableNormal"
    };
    IStyle style = (IStyle) null;
    foreach (string name in strArray)
    {
      style = docStyles.FindByName(name, StyleType.TableStyle);
      if (style != null)
        break;
    }
    if (style == null)
    {
      foreach (string name in strArray)
      {
        style = docStyles.FindByName(name);
        if (style != null)
        {
          if (style.StyleType != StyleType.TableStyle)
            this.RenameDupliateStyle(style, docStyles);
          else
            break;
        }
      }
      if (!(style is WTableStyle))
        style = (IStyle) this.CreateTableNormalStyle(docStyles);
    }
    if (style is WTableStyle)
      tableGridStyle.ApplyBaseStyle((Style) (style as WTableStyle));
    tableGridStyle.TableProperties.LeftIndent = 0.0f;
    tableGridStyle.TableProperties.Borders.BorderType = BorderStyle.Single;
    tableGridStyle.TableProperties.Borders.Color = Color.Black;
    docStyles.Add((IStyle) tableGridStyle);
    this.Document.StyleNameIds.Add("TableGrid", tableGridStyle.Name);
    return tableGridStyle;
  }

  private WTableStyle CreateTableNormalStyle(IStyleCollection styleCollection)
  {
    WTableStyle tableNormalStyle = new WTableStyle((IWordDocument) this.Document);
    tableNormalStyle.Name = "Normal Table";
    tableNormalStyle.IsSemiHidden = true;
    tableNormalStyle.UnhideWhenUsed = true;
    tableNormalStyle.IsPrimaryStyle = true;
    tableNormalStyle.UIPriority = 99;
    tableNormalStyle.TableProperties.LeftIndent = 0.0f;
    tableNormalStyle.TableProperties.Paddings.Top = 0.0f;
    tableNormalStyle.TableProperties.Paddings.Left = 5.4f;
    tableNormalStyle.TableProperties.Paddings.Right = 5.4f;
    tableNormalStyle.TableProperties.Paddings.Bottom = 0.0f;
    styleCollection.Add((IStyle) tableNormalStyle);
    return tableNormalStyle;
  }

  public WTable(IWordDocument doc, bool showBorder)
    : base((WordDocument) doc)
  {
    this.m_rows = new WRowCollection(this);
    if (!showBorder)
      return;
    this.TableFormat.Borders.BorderType = BorderStyle.Single;
    this.TableFormat.Borders.Color = Color.Black;
    this.TableFormat.Borders.LineWidth = 1f;
  }

  public WTable Clone() => (WTable) this.CloneImpl();

  public void ResetCells(int rowsNum, int columnsNum)
  {
    if (rowsNum <= 0 || columnsNum <= 0)
      throw new ArgumentException("Table should have atleast 1 row and 1 column");
    if (columnsNum > 63 /*0x3F*/)
      throw new ArgumentException("The number of cells must be between 1 and 63.");
    float cellWidth = this.GetOwnerWidth() / (float) columnsNum;
    this.ResetCells(rowsNum, columnsNum, (RowFormat) null, cellWidth);
  }

  public void ResetCells(int rowsNum, int columnsNum, RowFormat format, float cellWidth)
  {
    if (rowsNum <= 0 || columnsNum <= 0)
      throw new ArgumentException("Table should have atleast 1 row and 1 column");
    if (columnsNum > 63 /*0x3F*/)
      throw new ArgumentException("Not supported more than 63 cells.");
    if (format != null)
    {
      this.TableFormat.ClearFormatting();
      this.TableFormat.ImportContainer((FormatBase) format);
    }
    this.m_rows.Clear();
    if (rowsNum <= 0)
      return;
    WTableRow wtableRow = this.AddRow();
    --rowsNum;
    for (; columnsNum > 0; --columnsNum)
      wtableRow.Cells.Add(new WTableCell((IWordDocument) this.Document)
      {
        Width = cellWidth,
        PreferredWidth = {
          Width = cellWidth,
          WidthType = FtsWidth.Point
        }
      });
    for (; rowsNum > 0; --rowsNum)
      this.AddRow();
  }

  public void ApplyStyle(BuiltinTableStyle builtinTableStyle)
  {
    this.ApplyStyle(builtinTableStyle, true);
  }

  internal void ApplyStyle(BuiltinTableStyle builtinTableStyle, bool isClearCellShading)
  {
    this.CheckNormalStyle();
    IStyle style = (IStyle) (this.Document.Styles.FindByName(Style.BuiltInToName(builtinTableStyle), StyleType.TableStyle) as IWTableStyle);
    if (style == null)
    {
      style = Style.CreateBuiltinStyle(builtinTableStyle, this.Document);
      if ((style as WTableStyle).StyleId > 10)
        (style as WTableStyle).StyleId = 4094;
      this.Document.Styles.Add(style);
      this.Document.StyleNameIds.Add(style.Name.Replace("Accent", "-Accent").Replace(" ", ""), style.Name);
      (style as WTableStyle).ApplyBaseStyle("Normal Table");
    }
    this.ApplyStyle(style as IWTableStyle, isClearCellShading);
  }

  public WTableRow AddRow() => this.AddRow(true, true);

  public WTableRow AddRow(bool isCopyFormat) => this.AddRow(isCopyFormat, true);

  public WTableRow AddRow(bool isCopyFormat, bool autoPopulateCells)
  {
    WTableRow row = new WTableRow((IWordDocument) this.Document);
    if (autoPopulateCells)
    {
      WTableRow lastRow = this.LastRow;
      if (lastRow != null)
      {
        int index = 0;
        for (int count = lastRow.Cells.Count; index < count; ++index)
        {
          WTableCell cell1 = lastRow.Cells[index];
          WTableCell cell2 = new WTableCell((IWordDocument) this.Document);
          row.Cells.Add(cell2);
          cell2.Width = cell1.Width;
          if (isCopyFormat)
            cell2.CellFormat.ImportContainer((FormatBase) cell1.CellFormat);
        }
        row.Height = lastRow.Height;
      }
    }
    if (isCopyFormat)
    {
      if (this.LastRow != null)
        row.RowFormat.ImportContainer((FormatBase) this.LastRow.RowFormat);
      else
        row.RowFormat.ImportContainer((FormatBase) this.TableFormat);
    }
    this.Rows.Add(row);
    return row;
  }

  public override int Replace(Regex pattern, string replace)
  {
    int num = 0;
    foreach (WTableRow row in (CollectionImpl) this.Rows)
    {
      foreach (WTextBody cell in (CollectionImpl) row.Cells)
      {
        foreach (TextBodyItem childEntity in (CollectionImpl) cell.ChildEntities)
        {
          num += childEntity.Replace(pattern, replace);
          if (this.Document.ReplaceFirst && num > 0)
            return num;
        }
      }
    }
    return num;
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
    textSelection.CacheRanges();
    int num = 0;
    foreach (WTableRow row in (CollectionImpl) this.m_rows)
    {
      foreach (WTableCell cell in (CollectionImpl) row.Cells)
      {
        num += cell.Replace(pattern, textSelection, saveFormatting);
        if (this.Document.ReplaceFirst && num > 0)
          return num;
      }
    }
    return num;
  }

  public override TextSelection Find(Regex pattern)
  {
    foreach (WTableRow row in (CollectionImpl) this.m_rows)
    {
      foreach (WTextBody cell in (CollectionImpl) row.Cells)
      {
        TextSelection textSelection = cell.Find(pattern);
        if (textSelection != null && textSelection.Count > 0)
          return textSelection;
      }
    }
    return (TextSelection) null;
  }

  public void ApplyVerticalMerge(int columnIndex, int startRowIndex, int endRowIndex)
  {
    if (this.m_rows == null || this.m_rows.Count == 0)
      throw new Exception("Table rows are not initialized.");
    if (startRowIndex < 0 || startRowIndex >= this.m_rows.Count)
      throw new ArgumentOutOfRangeException(nameof (startRowIndex), "Row with specified row index doesn't exist");
    if (endRowIndex < 0 || endRowIndex >= this.m_rows.Count)
      throw new ArgumentOutOfRangeException(nameof (endRowIndex), "Row with specified row index doesn't exist");
    if (startRowIndex > endRowIndex)
      throw new Exception("Start row index is greater than end row index.");
    if (columnIndex < 0)
      throw new ArgumentOutOfRangeException(nameof (columnIndex), "Column with specified column index doesn't exist");
    for (int index = startRowIndex; index <= endRowIndex; ++index)
    {
      if (columnIndex >= this.m_rows[index].Cells.Count)
        throw new ArgumentOutOfRangeException(nameof (columnIndex), "Column with specified column index doesn't exist");
    }
    this.RemoveStartCellEmptyParagraph(columnIndex, startRowIndex, endRowIndex);
    this.m_rows[startRowIndex].Cells[columnIndex].CellFormat.VerticalMerge = CellMerge.Start;
    for (int index = startRowIndex + 1; index <= endRowIndex; ++index)
    {
      this.RemoveContinueCellEmptyParagraph(columnIndex, index, endRowIndex);
      this.m_rows[index].Cells[columnIndex].CellFormat.VerticalMerge = CellMerge.Continue;
      this.m_rows[index].Cells[columnIndex].Items.CloneTo((EntityCollection) this.m_rows[startRowIndex].Cells[columnIndex].Items);
      this.m_rows[index].Cells[columnIndex].Items.Clear();
    }
  }

  private void RemoveStartCellEmptyParagraph(int columnIndex, int startRowIndex, int endRowIndex)
  {
    bool flag = false;
    WTableCell cell = this.m_rows[startRowIndex].Cells[columnIndex];
    if (cell.Items.Count <= 0 || cell.LastParagraph == null)
      return;
    if (cell.LastParagraph.PreviousSibling != null && cell.LastParagraph.PreviousSibling is WTable)
      flag = true;
    if (this.HasRenderableItem(cell.LastParagraph) || flag || this.IsFollowingCellsEmpty(columnIndex, startRowIndex, endRowIndex))
      return;
    cell.Items.Remove((IEntity) cell.LastParagraph);
  }

  private void RemoveContinueCellEmptyParagraph(int columnIndex, int rowIndex, int endRowIndex)
  {
    bool flag = false;
    WTableCell cell1 = this.m_rows[rowIndex].Cells[columnIndex];
    if (cell1.Items.Count > 1 && cell1.LastParagraph != null && rowIndex != endRowIndex)
    {
      WTableCell cell2 = this.m_rows[endRowIndex].Cells[columnIndex];
      if (cell1.LastParagraph.PreviousSibling != null && cell1.LastParagraph.PreviousSibling is WTable)
        flag = true;
      if (this.HasRenderableItem(cell1.LastParagraph) || flag || rowIndex == endRowIndex - 1 && cell2.Items.Count <= 1 && (cell2.Items.Count != 1 || cell2.LastParagraph == null || cell2.LastParagraph.Items.Count <= 0 || !this.HasRenderableItem(cell2.LastParagraph)))
        return;
      cell1.Items.Remove((IEntity) cell1.LastParagraph);
    }
    else
    {
      if (cell1.Items.Count != 1 || cell1.LastParagraph == null || this.HasRenderableItem(cell1.LastParagraph))
        return;
      cell1.Items.Remove((IEntity) cell1.LastParagraph);
    }
  }

  private bool IsFollowingCellsEmpty(int columnIndex, int startRowIndex, int endRowIndex)
  {
    for (int index = startRowIndex + 1; index <= endRowIndex; ++index)
    {
      WTableCell cell = this.m_rows[index].Cells[columnIndex];
      if (cell.Items.Count > 1 || cell.Items.Count == 1 && cell.LastParagraph != null && cell.LastParagraph.Items.Count > 0 && this.HasRenderableItem(cell.LastParagraph))
        return false;
    }
    return true;
  }

  internal bool HasRenderableItem(IWParagraph paragraph)
  {
    for (int index = 0; index < paragraph.ChildEntities.Count; ++index)
    {
      switch (paragraph.ChildEntities[index])
      {
        case BookmarkStart _:
        case BookmarkEnd _:
        case EditableRangeStart _:
        case EditableRangeEnd _:
          continue;
        default:
          return true;
      }
    }
    return false;
  }

  public void ApplyHorizontalMerge(int rowIndex, int startCellIndex, int endCellIndex)
  {
    if (this.m_rows == null || this.m_rows.Count == 0)
      throw new Exception("Table rows are not initialized.");
    if (rowIndex < 0 || rowIndex >= this.m_rows.Count)
      throw new ArgumentOutOfRangeException(nameof (rowIndex), "Row with specified row index doesn't exist");
    WCellCollection cells = this.m_rows[rowIndex].Cells;
    if (cells == null || cells.Count == 0)
      throw new Exception("Table row cells are not initialized.");
    if (startCellIndex < 0 || startCellIndex > cells.Count - 1)
      throw new ArgumentOutOfRangeException(nameof (startCellIndex), "Cell with specified start cell index doesn't exist");
    if (endCellIndex < 0 || endCellIndex > cells.Count - 1)
      throw new ArgumentOutOfRangeException(nameof (endCellIndex), "Cell with specified end cell index doesn't exist");
    if (startCellIndex > endCellIndex)
      throw new Exception("Start cell index is greater than end cell index.");
    this.UpdateStartcell(cells, startCellIndex, endCellIndex);
    cells[startCellIndex].CellFormat.HorizontalMerge = CellMerge.Start;
    for (int index = startCellIndex + 1; index <= endCellIndex; ++index)
    {
      this.UpdateContinuecell(cells, index, endCellIndex);
      cells[index].CellFormat.HorizontalMerge = CellMerge.Continue;
      cells[index].Items.CloneToImpl((CollectionImpl) cells[startCellIndex].Items);
      cells[index].Items.Clear();
    }
  }

  internal void UpdateStartcell(WCellCollection cells, int startCellIndex, int endCellIndex)
  {
    bool isEmpty = false;
    bool isNotConsider = false;
    if (this.ChecksLastPara(cells[startCellIndex].Items, ref isEmpty, ref isNotConsider) && startCellIndex + 1 <= endCellIndex)
    {
      this.UpdateBookmark(cells, startCellIndex + 1, startCellIndex);
    }
    else
    {
      if (startCellIndex + 1 > endCellIndex)
        return;
      this.UpdateParaItems(cells, startCellIndex + 1, startCellIndex, isEmpty);
    }
  }

  internal void UpdateContinuecell(WCellCollection cells, int cellIndex, int endCellIndex)
  {
    bool flag = false;
    bool isEmpty = false;
    bool isNotConsider = false;
    if (cellIndex != endCellIndex || cells[cellIndex].Items.Count < 2)
      flag = this.ChecksLastPara(cells[cellIndex].Items, ref isEmpty, ref isNotConsider);
    if (flag && cellIndex + 1 <= endCellIndex)
      this.UpdateBookmark(cells, cellIndex + 1, cellIndex);
    else if (flag && cellIndex == endCellIndex)
    {
      this.UpdateBookmark(cells, cellIndex, cellIndex - 1);
    }
    else
    {
      if (cellIndex + 1 > endCellIndex)
        return;
      this.UpdateParaItems(cells, cellIndex + 1, cellIndex, isEmpty);
    }
  }

  internal void UpdateBookmark(WCellCollection cells, int currCellIndex, int prevCellIndex)
  {
    if (cells[currCellIndex].Items.FirstItem is WParagraph && cells[prevCellIndex].Items.LastItem is WParagraph)
    {
      EntityCollection childEntities1 = (cells[currCellIndex].Items.FirstItem as WParagraph).ChildEntities;
      EntityCollection childEntities2 = (cells[prevCellIndex].Items.LastItem as WParagraph).ChildEntities;
      int count = childEntities2.Count;
      for (int index = 0; index < count; ++index)
        childEntities1.Insert(0, (IEntity) childEntities2[childEntities2.Count - 1]);
      cells[prevCellIndex].Items.LastItem.RemoveSelf();
    }
    else
    {
      if (!(cells[currCellIndex].Items.FirstItem is WTable))
        return;
      WTable firstItem = cells[currCellIndex].Items.FirstItem as WTable;
      if (firstItem.Rows.Count <= 0 || firstItem.Rows[0].Cells.Count <= 0 || firstItem.Rows[0].Cells[0].ChildEntities.Count <= 0 || !(firstItem.Rows[0].Cells[0].ChildEntities.FirstItem is WParagraph) || !(cells[prevCellIndex].Items.LastItem is WParagraph))
        return;
      EntityCollection childEntities3 = (firstItem.Rows[0].Cells[0].ChildEntities.FirstItem as WParagraph).ChildEntities;
      EntityCollection childEntities4 = (cells[prevCellIndex].Items.LastItem as WParagraph).ChildEntities;
      int count = childEntities4.Count;
      for (int index = 0; index < count; ++index)
        childEntities3.Insert(0, (IEntity) childEntities4[childEntities4.Count - 1]);
      cells[prevCellIndex].Items.LastItem.RemoveSelf();
    }
  }

  internal void UpdateParaItems(
    WCellCollection cells,
    int currCellIndex,
    int prevCellIndex,
    bool isPrevCellEmpty)
  {
    if (cells[currCellIndex].Items.Count == 1 && cells[currCellIndex].Items[0] is WParagraph)
    {
      bool isEmpty = false;
      bool isNotConsider = false;
      bool flag = this.ChecksLastPara(cells[currCellIndex].Items, ref isEmpty, ref isNotConsider);
      if (isEmpty || flag || !flag && isNotConsider)
      {
        if (!(cells[prevCellIndex].Items.LastItem is WParagraph) || !(cells[currCellIndex].Items.FirstItem is WParagraph))
          return;
        EntityCollection childEntities1 = (cells[currCellIndex].Items.FirstItem as WParagraph).ChildEntities;
        EntityCollection childEntities2 = (cells[prevCellIndex].Items.LastItem as WParagraph).ChildEntities;
        int count = childEntities2.Count;
        for (int index = 0; index < count; ++index)
          childEntities1.Insert(0, (IEntity) childEntities2[childEntities2.Count - 1]);
        cells[prevCellIndex].Items.LastItem.RemoveSelf();
      }
      else
      {
        if (!isPrevCellEmpty)
          return;
        cells[prevCellIndex].Items.LastItem.RemoveSelf();
      }
    }
    else
    {
      if (!isPrevCellEmpty)
        return;
      cells[prevCellIndex].Items.LastItem.RemoveSelf();
    }
  }

  private bool ChecksLastPara(BodyItemCollection items, ref bool isEmpty, ref bool isNotConsider)
  {
    bool flag = false;
    if (items.LastItem is WParagraph && (items.LastItem.PreviousSibling != null && items.LastItem.PreviousSibling.EntityType != EntityType.Table || items.LastItem.PreviousSibling == null))
    {
      WParagraph lastItem = items.LastItem as WParagraph;
      if (lastItem.ChildEntities.Count > 0)
      {
        foreach (ParagraphItem childEntity in (CollectionImpl) lastItem.ChildEntities)
        {
          switch (childEntity)
          {
            case BookmarkStart _:
              if ((childEntity as BookmarkStart).Name != "_GoBack")
              {
                flag = true;
                isNotConsider = false;
                continue;
              }
              isNotConsider = true;
              continue;
            case BookmarkEnd _:
              if ((childEntity as BookmarkEnd).Name != "_GoBack")
              {
                flag = true;
                isNotConsider = false;
                continue;
              }
              isNotConsider = true;
              continue;
            default:
              flag = false;
              isNotConsider = false;
              goto label_17;
          }
        }
      }
      else
        isEmpty = true;
    }
label_17:
    return flag;
  }

  public void RemoveAbsPosition()
  {
    foreach (WTableRow row in (CollectionImpl) this.m_rows)
    {
      foreach (WTextBody cell in (CollectionImpl) row.Cells)
      {
        foreach (TextBodyItem textBodyItem in (CollectionImpl) cell.Items)
        {
          if (textBodyItem is WParagraph)
            (textBodyItem as WParagraph).RemoveAbsPosition();
          else if (textBodyItem is WTable)
            (textBodyItem as WTable).RemoveAbsPosition();
        }
      }
      if (row.RowFormat != null)
        row.RowFormat.RemovePositioning();
    }
  }

  public void AutoFit(AutoFitType autoFitType) => this.AutoFitTable(autoFitType);

  internal void GetMinimumAndMaximumWordWidth(
    ref float minimumWordWidth,
    ref float maximumWordWidth)
  {
    float ownerWidth = this.GetOwnerWidth();
    bool flag = this.IsTableBasedOnContent(this);
    this.AutoFitColumns(false);
    if (flag)
      this.CheckToRecalculatAgain();
    float totalWidth = this.m_tableGrid.GetTotalWidth((byte) 0);
    if ((double) ownerWidth < (double) totalWidth && !this.IsSkipToResizeNestedTable(totalWidth) || this.TableFormat.PreferredWidth.WidthType == FtsWidth.Percentage)
    {
      float num = totalWidth / ownerWidth;
      totalWidth *= num;
    }
    if ((double) totalWidth > (double) minimumWordWidth)
      minimumWordWidth = totalWidth;
    if ((double) totalWidth <= (double) maximumWordWidth)
      return;
    maximumWordWidth = totalWidth;
  }

  private void CheckToRecalculatAgain()
  {
    if (!this.IsInCell)
      return;
    WTableCell owner = this.Owner as WTableCell;
    WTable ownerTable = this.GetOwnerTable(this.Owner) as WTable;
    bool flag = owner != null && owner.Index == (owner.Owner as WTableRow).Cells.Count - 1;
    ownerTable.IsRecalulateBasedOnLastCol = flag && this.IsTableBasedOnContent(ownerTable);
  }

  private bool IsSkipToResizeNestedTable(float tableWidth)
  {
    WTable wtable = (WTable) null;
    WTableCell wtableCell = (WTableCell) null;
    WSection ownerSection = this.GetOwnerSection();
    if (this.IsInCell)
    {
      wtableCell = this.GetOwnerTableCell();
      wtable = wtableCell.OwnerRow != null ? wtableCell.OwnerRow.OwnerTable : (WTable) null;
    }
    return (this.m_doc.ActualFormatType == FormatType.Docx || this.m_doc.ActualFormatType == FormatType.Word2013) && wtableCell != null && wtable != null && wtable.TableFormat.IsAutoResized && (double) tableWidth < (double) ownerSection.PageSetup.ClientWidth && (wtable.Rows.Count == 1 && wtable.Rows[0].Cells.Count == 1 && wtable.PreferredTableWidth.WidthType == FtsWidth.Auto && wtableCell.PreferredWidth.WidthType == FtsWidth.Auto && !wtable.IsInCell && !wtable.TableFormat.WrapTextAround || wtable.IsInCell || wtable.IsNeedToRecalculate);
  }

  internal void AutoFitTable(AutoFitType autoFitType)
  {
    this.TableFormat.IsAutoResized = autoFitType != AutoFitType.FixedColumnWidth;
    if (autoFitType == AutoFitType.FitToContent)
      this.ClearPreferredWidths(true);
    this.AutoFitColumns(autoFitType == AutoFitType.FitToContent);
    this.UpdatePreferredWidthProperties(this.TableFormat.IsAutoResized, autoFitType);
  }

  internal bool IsSDTInTableCell(BlockContentControl sdtBlock)
  {
    for (; sdtBlock != null; sdtBlock = (sdtBlock.Owner as WTextBody).Owner as BlockContentControl)
    {
      if (sdtBlock.Owner is WTableCell)
        return true;
      if (!(sdtBlock.Owner is WTextBody) || !((sdtBlock.Owner as WTextBody).Owner is BlockContentControl))
        return false;
    }
    return false;
  }

  internal WTableCell GetOwnerTableCell()
  {
    if (this.Owner is WTableCell)
      return this.Owner as WTableCell;
    for (BlockContentControl owner = !(this.Owner is WTextBody) || !((this.Owner as WTextBody).Owner is BlockContentControl) ? (BlockContentControl) null : (this.Owner as WTextBody).Owner as BlockContentControl; owner != null; owner = (owner.Owner as WTextBody).Owner as BlockContentControl)
    {
      if (owner.Owner is WTableCell)
        return owner.Owner as WTableCell;
      if (!(owner.Owner is WTextBody) || !((owner.Owner as WTextBody).Owner is BlockContentControl))
        return (WTableCell) null;
    }
    return (WTableCell) null;
  }

  internal string GetTableText()
  {
    string tableText = string.Empty;
    foreach (WTableRow row in (CollectionImpl) this.Rows)
    {
      foreach (WTableCell cell in (CollectionImpl) row.Cells)
      {
        for (int index = 0; index < cell.Items.Count; ++index)
        {
          if (cell.Items[index] is WParagraph)
            tableText += (cell.Items[index] as WParagraph).GetParagraphText(false);
          else if (cell.Items[index] is WTable)
            tableText += (cell.Items[index] as WTable).GetTableText();
          if (this.Document.m_prevClonedEntity != null && this.Document.m_prevClonedEntity.OwnerTextBody == cell)
          {
            index = this.Document.m_prevClonedEntity.GetIndexInOwnerCollection();
            this.Document.m_prevClonedEntity = (TextBodyItem) null;
          }
          if (index == cell.Items.Count - 1 && cell.CellFormat.CurCellIndex < row.Cells.Count - 1)
          {
            tableText = tableText.Substring(0, tableText.Length - 1);
            tableText += ControlChar.Tab;
          }
        }
      }
    }
    return tableText;
  }

  private void CheckNormalStyle()
  {
    if (this.Document.Styles.FindByName("Normal Table", StyleType.TableStyle) is WTableStyle)
      return;
    WTableStyle builtinStyle = (WTableStyle) Style.CreateBuiltinStyle(BuiltinTableStyle.TableNormal, this.Document);
    this.Document.Styles.Add((IStyle) builtinStyle);
    this.Document.StyleNameIds.Add("TableNormal", builtinStyle.Name);
  }

  public IWTableStyle GetStyle() => this.m_style;

  private void ApplyStyle(IWTableStyle style, bool isClearCellShading)
  {
    this.m_style = style != null ? style : throw new ArgumentNullException("newStyle");
    this.DocxTableFormat.StyleName = this.m_style.Name;
    if (!isClearCellShading)
      return;
    this.RemoveCellBackGroundColor();
  }

  private void RemoveCellBackGroundColor()
  {
    foreach (WTableRow row in (CollectionImpl) this.Rows)
    {
      foreach (WTableCell cell in (CollectionImpl) row.Cells)
      {
        CellFormat cellFormat = cell.CellFormat;
        if (cellFormat != null)
        {
          if (cellFormat.PropertiesHash.ContainsKey(4))
            cellFormat.PropertiesHash.Remove(4);
          if (cellFormat.PropertiesHash.ContainsKey(7))
            cellFormat.PropertiesHash.Remove(7);
        }
      }
    }
  }

  public void ApplyStyle(string styleName)
  {
    if (!(this.Document.Styles.FindByName(styleName, StyleType.TableStyle) is IWTableStyle byName))
      throw new ArgumentNullException("newStyle");
    this.ApplyStyle(byName, true);
  }

  internal void ApplyStyle(string styleName, bool isClearCellShading)
  {
    if (!(this.Document.Styles.FindByName(styleName, StyleType.TableStyle) is IWTableStyle byName))
      throw new ArgumentNullException("newStyle");
    this.ApplyStyle(byName, isClearCellShading);
  }

  internal void ApplyBaseStyleFormats()
  {
    if (this.m_style == null)
      return;
    this.TableFormat.ApplyBase((this.m_style as WTableStyle).TableProperties.GetAsTableFormat());
    bool flag1 = false;
    foreach (ConditionalFormattingStyle conditionalFormattingStyle in (CollectionImpl) (this.m_style as WTableStyle).ConditionalFormattingStyles)
    {
      if (conditionalFormattingStyle.ConditionalFormattingType == ConditionalFormattingType.FirstRow)
      {
        flag1 = true;
        break;
      }
    }
    bool flag2 = this.Rows.Count > 1 && this.Rows[1].RowFormat.IsHeaderRow;
    int num1 = 1;
    for (int index1 = 0; index1 < this.Rows.Count; ++index1)
    {
      if (!flag1 || !flag2 || (index1 != 0 ? (this.Rows[index1].RowFormat.IsHeaderRow ? 1 : 0) : 1) == 0)
        flag2 = false;
      else if (index1 != 0)
        ++num1;
      int num2 = index1 + 1;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (ConditionalFormattingStyle) null;
      long rowStripe = (this.m_style as WTableStyle).TableProperties.RowStripe;
      bool flag3 = rowStripe == 0L || (!this.ApplyStyleForHeaderRow || !flag1 ? (long) num2 / rowStripe % 2L == 1L : index1 != 0 && ((long) (index1 - num1) / rowStripe + 1L) % 2L == 1L);
      bool flag4 = false;
      foreach (ConditionalFormattingStyle conditionalFormattingStyle2 in (CollectionImpl) (this.m_style as WTableStyle).ConditionalFormattingStyles)
      {
        if (conditionalFormattingStyle2.ConditionalFormattingType == ConditionalFormattingType.LastRow)
        {
          flag4 = true;
          break;
        }
      }
      foreach (ConditionalFormattingStyle conditionalFormattingStyle3 in (CollectionImpl) (this.m_style as WTableStyle).ConditionalFormattingStyles)
      {
        switch (conditionalFormattingStyle3.ConditionalFormattingType)
        {
          case ConditionalFormattingType.FirstRow:
            if (index1 == 0 && this.ApplyStyleForHeaderRow || flag2)
            {
              conditionalFormattingStyle1 = conditionalFormattingStyle3;
              continue;
            }
            continue;
          case ConditionalFormattingType.LastRow:
            if (index1 != 0 && !flag2 && conditionalFormattingStyle1 == null && index1 == this.Rows.Count - 1 && this.ApplyStyleForLastRow)
            {
              conditionalFormattingStyle1 = conditionalFormattingStyle3;
              continue;
            }
            continue;
          case ConditionalFormattingType.OddRowBanding:
            if (conditionalFormattingStyle1 == null && (index1 != this.Rows.Count - 1 || !this.ApplyStyleForLastRow || !flag4) && flag3 && rowStripe != 0L && this.ApplyStyleForBandedRows)
            {
              conditionalFormattingStyle1 = conditionalFormattingStyle3;
              continue;
            }
            continue;
          case ConditionalFormattingType.EvenRowBanding:
            if (conditionalFormattingStyle1 == null && index1 != 0 && (index1 != this.Rows.Count - 1 || !this.ApplyStyleForLastRow || !flag4) && !flag3 && this.ApplyStyleForBandedRows)
            {
              conditionalFormattingStyle1 = conditionalFormattingStyle3;
              continue;
            }
            continue;
          default:
            continue;
        }
      }
      if (conditionalFormattingStyle1 != null)
        this.Rows[index1].RowFormat.ApplyBase(conditionalFormattingStyle1.RowProperties.GetAsRowFormat());
      else
        this.Rows[index1].RowFormat.ApplyBase((this.m_style as WTableStyle).RowProperties.GetAsRowFormat());
      for (int index2 = 0; index2 < this.Rows[index1].Cells.Count; ++index2)
      {
        int num3 = index2 + 1;
        long columnStripe = (this.m_style as WTableStyle).TableProperties.ColumnStripe;
        bool flag5 = false;
        foreach (ConditionalFormattingStyle conditionalFormattingStyle4 in (CollectionImpl) (this.m_style as WTableStyle).ConditionalFormattingStyles)
        {
          if (conditionalFormattingStyle4.ConditionalFormattingType == ConditionalFormattingType.FirstColumn)
          {
            flag5 = true;
            break;
          }
        }
        bool flag6 = columnStripe == 0L || (!this.ApplyStyleForFirstColumn || !flag5 ? (long) num3 / columnStripe % 2L == 1L : index2 != 0 && ((long) (index2 - 1) / columnStripe + 1L) % 2L == 1L);
        WParagraphFormat wparagraphFormat = new WParagraphFormat((IWordDocument) this.Document);
        if (this.m_style.ParagraphFormat.BaseFormat != null)
          wparagraphFormat.ApplyBase(this.m_style.ParagraphFormat.BaseFormat);
        WCharacterFormat wcharacterFormat = new WCharacterFormat((IWordDocument) this.Document);
        CellFormat cellFormat = new CellFormat();
        wparagraphFormat.CopyFormat((FormatBase) this.m_style.ParagraphFormat);
        wcharacterFormat.CopyFormat((FormatBase) (this.m_style as WTableStyle).CharacterFormat);
        for (WTableStyle baseStyle = (this.m_style as WTableStyle).BaseStyle; baseStyle != null && baseStyle.IsCustom; baseStyle = baseStyle.BaseStyle)
          this.BaseStyleFormatCopy(baseStyle, wcharacterFormat, wparagraphFormat);
        cellFormat.UpdateCellFormat((this.m_style as WTableStyle).CellProperties);
        if (conditionalFormattingStyle1 != null)
        {
          cellFormat.UpdateCellFormat(conditionalFormattingStyle1.CellProperties);
          this.UpdateRowBorders(cellFormat.Borders, conditionalFormattingStyle1.CellProperties.Borders, this.TableFormat.Borders, index2, this.Rows[index1].Cells.Count, true);
          wparagraphFormat.CopyFormat((FormatBase) conditionalFormattingStyle1.ParagraphFormat);
          wcharacterFormat.CopyFormat((FormatBase) conditionalFormattingStyle1.CharacterFormat);
        }
        ConditionalFormattingStyle conditionalFormattingStyle5 = (ConditionalFormattingStyle) null;
        foreach (ConditionalFormattingStyle conditionalFormattingStyle6 in (CollectionImpl) (this.m_style as WTableStyle).ConditionalFormattingStyles)
        {
          switch (conditionalFormattingStyle6.ConditionalFormattingType)
          {
            case ConditionalFormattingType.FirstColumn:
              if (index2 == 0 && this.ApplyStyleForFirstColumn)
              {
                conditionalFormattingStyle5 = conditionalFormattingStyle6;
                break;
              }
              break;
            case ConditionalFormattingType.LastColumn:
              if (index2 != 0 && index2 == this.Rows[index1].Cells.Count - 1 && this.ApplyStyleForLastColumn)
              {
                conditionalFormattingStyle5 = conditionalFormattingStyle6;
                break;
              }
              break;
            case ConditionalFormattingType.OddColumnBanding:
              if (index2 != (this.Rows[index1].Cells.Count > 1 ? this.Rows[index1].Cells.Count - 1 : (int) this.Rows[index1].Cells[index2].GridSpan - 1) && flag6 && columnStripe != 0L && this.ApplyStyleForBandedColumns)
              {
                conditionalFormattingStyle5 = conditionalFormattingStyle6;
                break;
              }
              break;
            case ConditionalFormattingType.EvenColumnBanding:
              if (index2 != 0 && index2 != this.Rows[index1].Cells.Count - 1 && !flag6 && this.ApplyStyleForBandedColumns)
              {
                conditionalFormattingStyle5 = conditionalFormattingStyle6;
                break;
              }
              break;
            case ConditionalFormattingType.FirstRowLastCell:
              if (index1 == 0 && index2 != 0 && index2 == this.Rows[index1].Cells.Count - 1 && this.ApplyStyleForHeaderRow && this.ApplyStyleForLastColumn)
              {
                conditionalFormattingStyle5 = conditionalFormattingStyle6;
                break;
              }
              break;
            case ConditionalFormattingType.FirstRowFirstCell:
              if (index1 == 0 && index2 == 0 && this.ApplyStyleForHeaderRow && this.ApplyStyleForFirstColumn)
              {
                conditionalFormattingStyle5 = conditionalFormattingStyle6;
                break;
              }
              break;
            case ConditionalFormattingType.LastRowLastCell:
              if (index1 != 0 && index1 == this.Rows.Count - 1 && index2 != 0 && index2 == this.Rows[index1].Cells.Count - 1 && this.ApplyStyleForLastRow && this.ApplyStyleForLastColumn)
              {
                conditionalFormattingStyle5 = conditionalFormattingStyle6;
                break;
              }
              break;
            case ConditionalFormattingType.LastRowFirstCell:
              if (index1 != 0 && index1 == this.Rows.Count - 1 && index2 == 0 && this.ApplyStyleForLastRow && this.ApplyStyleForFirstColumn)
              {
                conditionalFormattingStyle5 = conditionalFormattingStyle6;
                break;
              }
              break;
          }
          if (conditionalFormattingStyle5 != null)
          {
            cellFormat.UpdateCellFormat(conditionalFormattingStyle5.CellProperties);
            this.UpdateColumnBorders(cellFormat.Borders, conditionalFormattingStyle5.CellProperties.Borders, this.TableFormat.Borders, index1, this.Rows.Count);
            wparagraphFormat.CopyFormat((FormatBase) conditionalFormattingStyle5.ParagraphFormat);
            wcharacterFormat.CopyFormat((FormatBase) conditionalFormattingStyle5.CharacterFormat);
            if (conditionalFormattingStyle1 != null)
            {
              if (conditionalFormattingStyle1.ConditionalFormattingType == ConditionalFormattingType.FirstRow)
              {
                wparagraphFormat.CopyFormat((FormatBase) conditionalFormattingStyle1.ParagraphFormat);
                wcharacterFormat.CopyFormat((FormatBase) conditionalFormattingStyle1.CharacterFormat);
              }
              cellFormat.UpdateCellFormat(conditionalFormattingStyle1.CellProperties);
              this.UpdateRowBorders(cellFormat.Borders, conditionalFormattingStyle1.CellProperties.Borders, this.TableFormat.Borders, index2, this.Rows[index1].Cells.Count, conditionalFormattingStyle5.ConditionalFormattingType != ConditionalFormattingType.OddColumnBanding && conditionalFormattingStyle5.ConditionalFormattingType != ConditionalFormattingType.EvenColumnBanding);
            }
            switch (conditionalFormattingStyle5.ConditionalFormattingType)
            {
              case ConditionalFormattingType.FirstColumn:
                if (conditionalFormattingStyle1 == null || conditionalFormattingStyle1.ConditionalFormattingType != ConditionalFormattingType.FirstRow)
                {
                  cellFormat.UpdateCellFormat(conditionalFormattingStyle5.CellProperties);
                  this.UpdateColumnBorders(cellFormat.Borders, conditionalFormattingStyle5.CellProperties.Borders, this.TableFormat.Borders, index1, this.Rows.Count);
                  continue;
                }
                continue;
              case ConditionalFormattingType.FirstRowLastCell:
              case ConditionalFormattingType.FirstRowFirstCell:
              case ConditionalFormattingType.LastRowLastCell:
              case ConditionalFormattingType.LastRowFirstCell:
                cellFormat.CopyFormat((FormatBase) conditionalFormattingStyle5.CellProperties);
                wparagraphFormat.CopyFormat((FormatBase) conditionalFormattingStyle5.ParagraphFormat);
                wcharacterFormat.CopyFormat((FormatBase) conditionalFormattingStyle5.CharacterFormat);
                continue;
              default:
                continue;
            }
          }
        }
        this.Rows[index1].Cells[index2].ApplyTableStyleBaseFormats(cellFormat, wparagraphFormat, wcharacterFormat, this.Rows[index1].Cells[index2].Items);
      }
    }
  }

  private void BaseStyleFormatCopy(
    WTableStyle baseStyle,
    WCharacterFormat characterFormat,
    WParagraphFormat paragraphFormat)
  {
    foreach (KeyValuePair<int, object> keyValuePair in baseStyle.CharacterFormat.PropertiesHash)
    {
      if (!characterFormat.PropertiesHash.ContainsKey(keyValuePair.Key))
        characterFormat.PropertiesHash.Add(keyValuePair.Key, keyValuePair.Value);
    }
    foreach (KeyValuePair<int, object> keyValuePair in baseStyle.ParagraphFormat.PropertiesHash)
    {
      if (!paragraphFormat.PropertiesHash.ContainsKey(keyValuePair.Key))
        paragraphFormat.PropertiesHash.Add(keyValuePair.Key, keyValuePair.Value);
    }
  }

  internal CellFormat GetCellFormatFromStyle(int rowIndex, int cellIndex)
  {
    if (this.m_style == null)
      return (CellFormat) null;
    int num1 = rowIndex + 1;
    ConditionalFormattingStyle conditionalFormattingStyle1 = (ConditionalFormattingStyle) null;
    long rowStripe = (this.m_style as WTableStyle).TableProperties.RowStripe;
    bool flag1 = false;
    foreach (ConditionalFormattingStyle conditionalFormattingStyle2 in (CollectionImpl) (this.m_style as WTableStyle).ConditionalFormattingStyles)
    {
      if (conditionalFormattingStyle2.ConditionalFormattingType == ConditionalFormattingType.FirstRow)
      {
        flag1 = true;
        break;
      }
    }
    bool flag2 = rowStripe == 0L || (!this.ApplyStyleForHeaderRow || !flag1 ? (long) num1 / rowStripe % 2L == 1L : rowIndex != 0 && ((long) (rowIndex - 1) / rowStripe + 1L) % 2L == 1L);
    foreach (ConditionalFormattingStyle conditionalFormattingStyle3 in (CollectionImpl) (this.m_style as WTableStyle).ConditionalFormattingStyles)
    {
      switch (conditionalFormattingStyle3.ConditionalFormattingType)
      {
        case ConditionalFormattingType.FirstRow:
          if (rowIndex == 0 && this.ApplyStyleForHeaderRow)
          {
            conditionalFormattingStyle1 = conditionalFormattingStyle3;
            continue;
          }
          continue;
        case ConditionalFormattingType.LastRow:
          if (rowIndex != 0 && rowIndex == this.Rows.Count - 1 && this.ApplyStyleForLastRow)
          {
            conditionalFormattingStyle1 = conditionalFormattingStyle3;
            continue;
          }
          continue;
        case ConditionalFormattingType.OddRowBanding:
          if ((rowIndex != this.Rows.Count - 1 || !this.ApplyStyleForLastRow) && flag2 && rowStripe != 0L && this.ApplyStyleForBandedRows)
          {
            conditionalFormattingStyle1 = conditionalFormattingStyle3;
            continue;
          }
          continue;
        case ConditionalFormattingType.EvenRowBanding:
          if (rowIndex != 0 && (rowIndex != this.Rows.Count - 1 || !this.ApplyStyleForLastRow) && !flag2 && this.ApplyStyleForBandedRows)
          {
            conditionalFormattingStyle1 = conditionalFormattingStyle3;
            continue;
          }
          continue;
        default:
          continue;
      }
    }
    int num2 = cellIndex + 1;
    long columnStripe = (this.m_style as WTableStyle).TableProperties.ColumnStripe;
    bool flag3 = false;
    foreach (ConditionalFormattingStyle conditionalFormattingStyle4 in (CollectionImpl) (this.m_style as WTableStyle).ConditionalFormattingStyles)
    {
      if (conditionalFormattingStyle4.ConditionalFormattingType == ConditionalFormattingType.FirstColumn)
      {
        flag3 = true;
        break;
      }
    }
    bool flag4 = columnStripe == 0L || (!this.ApplyStyleForFirstColumn || !flag3 ? (long) num2 / columnStripe % 2L == 1L : cellIndex != 0 && ((long) (cellIndex - 1) / columnStripe + 1L) % 2L == 1L);
    CellFormat cellFormatFromStyle = new CellFormat();
    cellFormatFromStyle.UpdateCellFormat((this.m_style as WTableStyle).CellProperties);
    if (conditionalFormattingStyle1 != null)
      cellFormatFromStyle.UpdateCellFormat(conditionalFormattingStyle1.CellProperties);
    ConditionalFormattingStyle conditionalFormattingStyle5 = (ConditionalFormattingStyle) null;
    foreach (ConditionalFormattingStyle conditionalFormattingStyle6 in (CollectionImpl) (this.m_style as WTableStyle).ConditionalFormattingStyles)
    {
      switch (conditionalFormattingStyle6.ConditionalFormattingType)
      {
        case ConditionalFormattingType.FirstColumn:
          if (cellIndex == 0 && this.ApplyStyleForFirstColumn)
          {
            conditionalFormattingStyle5 = conditionalFormattingStyle6;
            break;
          }
          break;
        case ConditionalFormattingType.LastColumn:
          if (cellIndex != 0 && cellIndex == this.Rows[rowIndex].Cells.Count - 1 && this.ApplyStyleForLastColumn)
          {
            conditionalFormattingStyle5 = conditionalFormattingStyle6;
            break;
          }
          break;
        case ConditionalFormattingType.OddColumnBanding:
          if (cellIndex != this.Rows[rowIndex].Cells.Count - 1 && flag4 && columnStripe != 0L && this.ApplyStyleForBandedColumns)
          {
            conditionalFormattingStyle5 = conditionalFormattingStyle6;
            break;
          }
          break;
        case ConditionalFormattingType.EvenColumnBanding:
          if (cellIndex != 0 && cellIndex != this.Rows[rowIndex].Cells.Count - 1 && !flag4 && this.ApplyStyleForBandedColumns)
          {
            conditionalFormattingStyle5 = conditionalFormattingStyle6;
            break;
          }
          break;
        case ConditionalFormattingType.FirstRowLastCell:
          if (rowIndex == 0 && cellIndex != 0 && cellIndex == this.Rows[rowIndex].Cells.Count - 1 && this.ApplyStyleForHeaderRow && this.ApplyStyleForLastColumn)
          {
            conditionalFormattingStyle5 = conditionalFormattingStyle6;
            break;
          }
          break;
        case ConditionalFormattingType.FirstRowFirstCell:
          if (rowIndex == 0 && cellIndex == 0 && this.ApplyStyleForHeaderRow && this.ApplyStyleForFirstColumn)
          {
            conditionalFormattingStyle5 = conditionalFormattingStyle6;
            break;
          }
          break;
        case ConditionalFormattingType.LastRowLastCell:
          if (rowIndex != 0 && rowIndex == this.Rows.Count - 1 && cellIndex != 0 && cellIndex == this.Rows[rowIndex].Cells.Count - 1 && this.ApplyStyleForLastRow && this.ApplyStyleForLastColumn)
          {
            conditionalFormattingStyle5 = conditionalFormattingStyle6;
            break;
          }
          break;
        case ConditionalFormattingType.LastRowFirstCell:
          if (rowIndex != 0 && rowIndex == this.Rows.Count - 1 && cellIndex == 0 && this.ApplyStyleForLastRow && this.ApplyStyleForFirstColumn)
          {
            conditionalFormattingStyle5 = conditionalFormattingStyle6;
            break;
          }
          break;
      }
      if (conditionalFormattingStyle5 != null)
      {
        cellFormatFromStyle.UpdateCellFormat(conditionalFormattingStyle5.CellProperties);
        if (conditionalFormattingStyle1 != null)
          cellFormatFromStyle.UpdateCellFormat(conditionalFormattingStyle1.CellProperties);
        switch (conditionalFormattingStyle5.ConditionalFormattingType)
        {
          case ConditionalFormattingType.FirstColumn:
            if (conditionalFormattingStyle1 == null || conditionalFormattingStyle1.ConditionalFormattingType != ConditionalFormattingType.FirstRow)
            {
              cellFormatFromStyle.UpdateCellFormat(conditionalFormattingStyle5.CellProperties);
              continue;
            }
            continue;
          case ConditionalFormattingType.FirstRowLastCell:
          case ConditionalFormattingType.FirstRowFirstCell:
          case ConditionalFormattingType.LastRowLastCell:
          case ConditionalFormattingType.LastRowFirstCell:
            cellFormatFromStyle.CopyFormat((FormatBase) conditionalFormattingStyle5.CellProperties);
            continue;
          default:
            continue;
        }
      }
    }
    return cellFormatFromStyle;
  }

  private void UpdateRowBorders(
    Borders dest,
    Borders src,
    Borders tableBorders,
    int index,
    int count,
    bool isLeftRightWidthapplicable)
  {
    if (src.NoBorder)
      return;
    dest.Top.CopyBorderFormatting(src.Top);
    dest.Bottom.CopyBorderFormatting(src.Bottom);
    if (index == 0 && isLeftRightWidthapplicable)
      dest.Left.CopyBorderFormatting(src.Left);
    if (index == count - 1 && isLeftRightWidthapplicable)
      dest.Right.CopyBorderFormatting(src.Right);
    if (!src.Vertical.HasKey(2))
      return;
    if (index < count - 1)
      dest.Right.CopyBorderFormatting(src.Vertical);
    if (index <= 0 || index >= count)
      return;
    dest.Left.CopyBorderFormatting(src.Vertical);
  }

  private void UpdateColumnBorders(
    Borders dest,
    Borders src,
    Borders tableBorders,
    int index,
    int count)
  {
    if (src.NoBorder)
      return;
    dest.Left.CopyBorderFormatting(src.Left);
    dest.Right.CopyBorderFormatting(src.Right);
    if (index == 0)
      dest.Top.CopyBorderFormatting(src.Top);
    if (index == count - 1)
      dest.Bottom.CopyBorderFormatting(src.Bottom);
    if (!src.Horizontal.HasValue(2))
      return;
    if (index < count - 1)
    {
      dest.Bottom.CopyBorderFormatting(src.Horizontal);
      if (src.Horizontal.BorderType == BorderStyle.Cleared && tableBorders.Horizontal.BorderType != BorderStyle.None)
        dest.Bottom.CopyBorderFormatting(tableBorders.Vertical);
    }
    if (index <= 0 || index >= count)
      return;
    dest.Top.CopyBorderFormatting(src.Horizontal);
    if (src.Horizontal.BorderType != BorderStyle.Cleared || tableBorders.Horizontal.BorderType == BorderStyle.None)
      return;
    dest.Top.CopyBorderFormatting(tableBorders.Vertical);
  }

  internal override TextSelectionList FindAll(Regex pattern)
  {
    TextSelectionList all1 = (TextSelectionList) null;
    foreach (WTableRow row in (CollectionImpl) this.m_rows)
    {
      foreach (WTextBody cell in (CollectionImpl) row.Cells)
      {
        TextSelectionList all2 = cell.FindAll(pattern);
        if (all2 != null && all2.Count > 0)
        {
          if (all1 == null)
            all1 = all2;
          else
            all1.AddRange((IEnumerable<TextSelection>) all2);
        }
      }
    }
    return all1;
  }

  internal override void AddSelf()
  {
    foreach (Entity row in (CollectionImpl) this.Rows)
      row.AddSelf();
  }

  protected override object CloneImpl()
  {
    WTable wtable = (WTable) base.CloneImpl();
    wtable.m_rows = new WRowCollection(wtable);
    wtable.m_initTableFormat = (RowFormat) null;
    if (this.m_tableGrid != null)
      wtable.m_tableGrid = this.m_tableGrid.Clone();
    wtable.TableFormat.ImportContainer((FormatBase) this.TableFormat);
    wtable.TableFormat.SetOwner((OwnerHolder) wtable);
    if (this.m_xmlTblFormat != null)
      wtable.m_xmlTblFormat = this.m_xmlTblFormat.Clone(wtable);
    this.Rows.CloneTo((EntityCollection) wtable.m_rows);
    foreach (WTableRow row1 in (CollectionImpl) this.Rows)
    {
      foreach (WTableRow row2 in (CollectionImpl) wtable.m_rows)
        row2.RowFormat.CopyRowFormatRevisions((FormatBase) row1.RowFormat);
    }
    if (this.m_style != null)
      wtable.ApplyStyle(this.m_style.Clone() as IWTableStyle, false);
    return (object) wtable;
  }

  internal override void CloneRelationsTo(WordDocument doc, OwnerHolder nextOwner)
  {
    base.CloneRelationsTo(doc, nextOwner);
    if (doc.ImportStyles || doc.UpdateAlternateChunk)
      this.CloneStyleTo(doc);
    int index = 0;
    for (int count = this.ChildEntities.Count; index < count; ++index)
      this.ChildEntities[index].CloneRelationsTo(doc, nextOwner);
  }

  private void CloneStyleTo(WordDocument doc)
  {
    if (this.m_style == null)
      return;
    IStyle style = (this.m_style as Style).ImportStyleTo(doc, false);
    if (!(style is WTableStyle))
      return;
    this.ApplyStyle((IWTableStyle) (style as WTableStyle), false);
  }

  private void CheckTableGrid()
  {
    if (this.m_tableGrid != null && (this.PreferredTableWidth.WidthType >= FtsWidth.Percentage || this.IsTableCellWidthDefined))
    {
      float num1 = (float) Math.Round((double) this.GetTableClientWidth(this.GetOwnerWidth()) * 20.0);
      float num2 = 0.0f;
      if (this.m_tableGrid.Count != 0)
        num2 = this.m_tableGrid[this.m_tableGrid.Count - 1].EndOffset;
      if ((double) num1 == (double) num2)
        return;
      this.UpdateTableGrid(false, true);
      this.IsTableGridVerified = true;
    }
    else
    {
      if (!this.IsTableGridCorrupted)
        return;
      this.UpdateTableGrid(false, true);
      this.IsTableGridCorrupted = false;
    }
  }

  private bool IsCellWidthZero()
  {
    foreach (WTableRow row in (CollectionImpl) this.Rows)
    {
      if (!row.IsCellWidthZero())
        return false;
    }
    return true;
  }

  internal void UpdateCellWidthByPartitioning(float tableWidth, ref bool isTableGridMissMatch)
  {
    this.IsUpdateCellWidthByPartitioning = false;
    bool isSkipToEqualPartition = false;
    foreach (WTableRow row in (CollectionImpl) this.Rows)
    {
      isSkipToEqualPartition = isSkipToEqualPartition ? isSkipToEqualPartition : this.IsSkipToUpdateByEqualPartition(row);
      row.UpdateCellWidthByPartitioning(tableWidth, isSkipToEqualPartition);
    }
    if (this.m_doc.ActualFormatType != FormatType.Docx || !this.IsInCell || this.PreferredTableWidth.WidthType != FtsWidth.None)
      return;
    foreach (WTableRow row in (CollectionImpl) this.Rows)
    {
      foreach (WTableCell cell in (CollectionImpl) row.Cells)
      {
        if (cell.PreferredWidth.WidthType != FtsWidth.Percentage)
        {
          isTableGridMissMatch = false;
          break;
        }
      }
      if (row.Cells.Count > this.m_tableGrid.Count)
      {
        isTableGridMissMatch = true;
        break;
      }
    }
  }

  private bool IsSkipToUpdateByEqualPartition(WTableRow row)
  {
    foreach (WTableCell cell in (CollectionImpl) row.Cells)
    {
      if (cell.PreferredWidth.WidthType == FtsWidth.Percentage || cell.PreferredWidth.WidthType == FtsWidth.Point)
        return true;
    }
    return false;
  }

  internal void UpdateUnDefinedCellWidth()
  {
    float ownerWidth = this.GetOwnerWidth();
    float width = this.Width;
    foreach (WTableRow row in (CollectionImpl) this.Rows)
      row.UpdateUnDefinedCellWidth((double) ownerWidth < (double) width ? ownerWidth : width);
  }

  internal void UpdateTableGrid(bool isTableGridMissMatch, bool isgridafter)
  {
    this.m_tableGrid = new WTableColumnCollection(this.Document);
    float ownerWidth = this.GetOwnerWidth();
    float tableClientWidth = this.GetTableClientWidth(ownerWidth);
    float maxRowWidth = this.GetMaxRowWidth(ownerWidth);
    int num = this.TableFormat.IsAutoResized ? 1 : 0;
    int widthType = (int) this.TableFormat.PreferredWidth.WidthType;
    bool isSkiptoCalculateCellWidth = false;
    if (this.IsUpdateCellWidthByPartitioning || !this.CheckCellWidth() && !this.Document.IsOpening)
    {
      isTableGridMissMatch = false;
      this.UpdateCellWidthByPartitioning(tableClientWidth, ref isTableGridMissMatch);
    }
    if (this.PreferredTableWidth.WidthType >= FtsWidth.Percentage && (double) this.PreferredTableWidth.Width > 0.0 || this.IsTableCellWidthDefined)
      isSkiptoCalculateCellWidth = this.IsNeedtoResizeCell(tableClientWidth);
    foreach (WTableRow row in (CollectionImpl) this.Rows)
    {
      float currOffset = 0.0f;
      if (this.PreferredTableWidth.WidthType >= FtsWidth.Percentage && (double) this.PreferredTableWidth.Width > 0.0 || this.IsTableCellWidthDefined)
        this.UpdateCellWidth(row, ownerWidth, tableClientWidth, maxRowWidth, isSkiptoCalculateCellWidth, isgridafter);
      if ((double) row.RowFormat.BeforeWidth > 0.0)
      {
        currOffset += this.GetGridBeforeAfter(row, ownerWidth, false, tableClientWidth, currOffset, maxRowWidth, isTableGridMissMatch);
        this.UpdateTableGrid(currOffset);
      }
      foreach (WTableCell cell in (CollectionImpl) row.Cells)
      {
        currOffset += this.GetCellWidth(cell, ownerWidth, tableClientWidth, currOffset, maxRowWidth, isTableGridMissMatch);
        this.UpdateTableGrid(currOffset);
      }
      if ((double) row.RowFormat.AfterWidth > 0.0)
        this.UpdateTableGrid(currOffset + this.GetGridBeforeAfter(row, ownerWidth, true, tableClientWidth, currOffset, maxRowWidth, isTableGridMissMatch));
    }
    if (!isTableGridMissMatch)
      return;
    this.m_tableGrid.ValidateColumnWidths();
  }

  internal bool CheckCellWidth()
  {
    bool flag = true;
    foreach (WTableRow row in (CollectionImpl) this.Rows)
    {
      foreach (WTableCell cell in (CollectionImpl) row.Cells)
      {
        if ((double) cell.CellFormat.CellWidth != 0.0)
          return true;
        flag = false;
      }
    }
    return flag;
  }

  internal void AutoFitColumns(bool forceAutoFitToContent)
  {
    bool isTableGridCorrupts = this.IsTableGridCorrupted && this.m_doc.IsDOCX();
    bool needtoCalculateParaWidth = this.m_doc.IsDOCX() && !this.IsInCell && this.TableFormat.IsAutoResized && this.HasOnlyParagraphs && (isTableGridCorrupts && this.IsTableBasedOnContent(this) || !this.HasPercentPreferredCellWidth && !this.HasPointPreferredCellWidth && !this.HasAutoPreferredCellWidth && this.HasNonePreferredCellWidth && this.TableFormat.PreferredWidth.WidthType == FtsWidth.Percentage);
    if (this.TableGrid.Count == 0)
      this.UpdateTableGrid(false, true);
    this.UpdateGridSpan();
    List<WTableCell> wtableCellList = new List<WTableCell>();
    bool isAutoResized = this.TableFormat.IsAutoResized;
    float ownerWidth = this.GetOwnerWidth();
    float tableClientWidth = this.GetTableClientWidth(ownerWidth);
    bool isAutoWidth = this.IsAutoWidth(ownerWidth, tableClientWidth);
    Dictionary<int, float> preferredWidths = new Dictionary<int, float>();
    bool flag1 = !this.IsInCell && this.Document != null && this.Document.ActualFormatType == FormatType.Docx && !this.TableFormat.IsAutoResized && this.PreferredTableWidth.WidthType == FtsWidth.Point && (double) this.PreferredTableWidth.Width > 0.0;
    float currentRowWidthBasedOnCells = 0.0f;
    for (int index1 = 0; index1 < this.Rows.Count; ++index1)
    {
      float num1 = 0.0f;
      WTableRow row = this.Rows[index1];
      ColumnSizeInfo sizeInfo1 = new ColumnSizeInfo();
      short currentColumnIndex = 0;
      float num2 = 0.0f;
      if (row.RowFormat.GridBefore > (short) 0)
      {
        float cellWidth = this.GetCellWidth(row.RowFormat.GridBeforeWidth.Width, this.PreferredTableWidth.WidthType, tableClientWidth, (WTableCell) null);
        sizeInfo1.MinimumWidth = cellWidth;
        this.TableGrid.UpdateColumns((int) currentColumnIndex, (int) (currentColumnIndex = row.RowFormat.GridBefore), num2 += cellWidth, sizeInfo1, preferredWidths);
      }
      for (int index2 = 0; index2 < row.Cells.Count; ++index2)
      {
        WTableCell cell = row.Cells[index2];
        flag1 = !flag1 ? flag1 : cell.CellFormat.VerticalMerge == CellMerge.None && (double) cell.PreferredWidth.Width > 0.0 && cell.PreferredWidth.WidthType == FtsWidth.Point;
        for (int index3 = 0; index3 < wtableCellList.Count; ++index3)
        {
          WTableCell wtableCell = wtableCellList[index3];
          if ((int) wtableCell.GridColumnStartIndex >= (int) currentColumnIndex)
          {
            bool flag2 = true;
            int rowSpan = this.GetRowSpan(wtableCell);
            if ((int) wtableCell.GridColumnStartIndex > (int) currentColumnIndex)
            {
              flag2 = false;
            }
            else
            {
              float cellWidth = this.GetCellWidth(wtableCell.CellFormat.PreferredWidth.Width, wtableCell.CellFormat.PreferredWidth.WidthType, tableClientWidth, wtableCell);
              ColumnSizeInfo sizeInfo2 = wtableCell.GetSizeInfo(isAutoResized, isAutoWidth, needtoCalculateParaWidth);
              short num3 = 1;
              wtableCell.GridSpan = wtableCell.GridSpan == (short) 0 ? num3 : wtableCell.GridSpan;
              this.TableGrid.UpdateColumns((int) currentColumnIndex, (int) currentColumnIndex + (int) wtableCell.GridSpan, num2 += cellWidth, sizeInfo2, preferredWidths);
            }
            if (!flag2 && index2 == row.Cells.Count - 1)
              flag2 = true;
            if (flag2 && index1 - wtableCell.OwnerRow.Index == rowSpan - 1)
            {
              wtableCellList.RemoveAt(index3);
              --index3;
            }
          }
        }
        if (this.GetRowSpan(cell) > 1)
        {
          if (wtableCellList.Count == 0 || (int) wtableCellList[wtableCellList.Count - 1].GridColumnStartIndex <= (int) currentColumnIndex)
          {
            wtableCellList.Add(cell);
          }
          else
          {
            int index4 = 0;
            for (int count = wtableCellList.Count; count > 0; --count)
            {
              if ((int) wtableCellList[count - 1].GridColumnStartIndex > (int) currentColumnIndex)
                index4 = count - 1;
            }
            wtableCellList.Insert(index4, cell);
          }
        }
        sizeInfo1 = cell.GetSizeInfo(isAutoResized, isAutoWidth, needtoCalculateParaWidth);
        float cellWidth1 = this.GetCellWidth(cell.CellFormat.PreferredWidth.Width, cell.CellFormat.PreferredWidth.WidthType, tableClientWidth, cell);
        num1 += cellWidth1;
        this.TableGrid.UpdateColumns((int) currentColumnIndex, (int) (currentColumnIndex += cell.GridSpan), num2 += cellWidth1, sizeInfo1, preferredWidths);
      }
      if (row.RowFormat.GridAfter > (short) 0)
      {
        float cellWidth = this.GetCellWidth(row.RowFormat.GridAfterWidth.Width, row.RowFormat.GridAfterWidth.WidthType, tableClientWidth, (WTableCell) null);
        sizeInfo1.MinimumWidth = cellWidth;
        short num4;
        float num5;
        this.TableGrid.UpdateColumns((int) currentColumnIndex, (int) (num4 = (short) ((int) currentColumnIndex + (int) row.RowFormat.GridAfter)), num5 = num2 + cellWidth, sizeInfo1, preferredWidths);
      }
      flag1 = !flag1 ? flag1 : (double) num1 <= (double) this.PreferredTableWidth.Width;
      currentRowWidthBasedOnCells = (double) currentRowWidthBasedOnCells < (double) num1 ? num1 : currentRowWidthBasedOnCells;
    }
    this.UsePreferredCellWidth = flag1;
    this.TableGrid.UpdatePreferredWidhToColumns(preferredWidths);
    bool autoFit = this.CheckNeedToAutoFit();
    if (forceAutoFitToContent || autoFit && isAutoResized && (double) this.TableFormat.PreferredWidth.Width == 0.0)
    {
      this.TableGrid.AutoFitColumns(ownerWidth, tableClientWidth, isAutoWidth, forceAutoFitToContent);
    }
    else
    {
      bool skipGridValue = this.CheckIsNeedToSkipGridValue(ownerWidth, isAutoWidth, currentRowWidthBasedOnCells);
      if (skipGridValue && this.IsAutoTableSkipTableGrid())
        this.ResizeAutoTableColumnWidth();
      if (!this.UsePreferredCellWidth && !skipGridValue)
        this.TableGrid.ValidateColumnWidths();
      this.TableGrid.FitColumns(ownerWidth, tableClientWidth, isAutoWidth, this);
    }
    this.SetNewWidthToCells(isTableGridCorrupts);
  }

  private bool CheckNeedToAutoFit()
  {
    int num = 0;
    foreach (WTableColumn wtableColumn in (CollectionImpl) this.TableGrid)
    {
      if ((double) wtableColumn.PreferredWidth == 0.0)
        ++num;
    }
    return num == this.TableGrid.Count;
  }

  private bool CheckIsNeedToSkipGridValue(
    float containerWidth,
    bool isAutoWidth,
    float currentRowWidthBasedOnCells)
  {
    if (!this.IsInCell && (this.Document.ActualFormatType == FormatType.Word2013 || this.Document.ActualFormatType == FormatType.Docx) && isAutoWidth && this.IsAllCellsHavePointWidth && (double) this.PreferredTableWidth.Width == 0.0 && Math.Round((double) currentRowWidthBasedOnCells, 2) <= Math.Round((double) containerWidth, 2) && Math.Round((double) this.TableFormat.LeftIndent + (double) currentRowWidthBasedOnCells, 2) <= Math.Round((double) containerWidth, 2) && (double) this.TableFormat.CellSpacing <= 0.0)
    {
      int num = 0;
      bool flag = this.HasMergeCellsInTable();
      for (int index = 0; index < this.TableGrid.InnerList.Count; ++index)
      {
        WTableColumn inner = this.TableGrid.InnerList[index] as WTableColumn;
        if ((double) inner.PreferredWidth != 0.0 && (double) inner.MaximumWordWidth <= (double) inner.PreferredWidth)
          ++num;
      }
      if (num == this.TableGrid.InnerList.Count && !flag)
        return true;
    }
    else if (this.IsAutoTableSkipTableGrid())
      return true;
    return false;
  }

  private bool HasMergeCellsInTable()
  {
    for (int index1 = 0; index1 < this.Rows.Count; ++index1)
    {
      WTableRow row = this.Rows[index1];
      for (int index2 = 0; index2 < row.Cells.Count; ++index2)
      {
        WTableCell cell = row.Cells[index2];
        if (cell.GridSpan > (short) 1 || cell.CellFormat.HorizontalMerge != CellMerge.None || cell.CellFormat.VerticalMerge != CellMerge.None)
          return true;
      }
    }
    return false;
  }

  private bool IsAutoTableSkipTableGrid()
  {
    return this.Document.ActualFormatType == FormatType.Word2010 && this.IsAutoTableExceedsClientWidth() && this.IsAllCellsHavePointWidth && (double) this.TableFormat.CellSpacing <= 0.0 && !this.HasMergeCellsInTable() && this.MaximumCellCount() == this.TableGrid.Count;
  }

  private void ResizeAutoTableColumnWidth()
  {
    float num1 = 0.0f;
    for (int index = 0; index < this.TableGrid.InnerList.Count; ++index)
    {
      WTableColumn inner = this.TableGrid.InnerList[index] as WTableColumn;
      num1 += (double) inner.PreferredWidth > (double) inner.MaximumWordWidth ? inner.PreferredWidth : inner.MaximumWordWidth;
    }
    if (Math.Round((double) num1, 2) <= Math.Round((double) this.PreferredTableWidth.Width, 2))
      return;
    for (int index = 0; index < this.TableGrid.InnerList.Count; ++index)
    {
      int num2 = -1;
      WTableColumn inner = this.TableGrid.InnerList[index] as WTableColumn;
      if ((double) inner.PreferredWidth < (double) inner.MaximumWordWidth)
      {
        inner.PreferredWidth = inner.MaximumWordWidth;
        num2 = index;
      }
      float num3 = this.PreferredTableWidth.Width / num1;
      if (num2 != index && (double) inner.PreferredWidth > (double) inner.MaximumWordWidth)
        inner.PreferredWidth = (float) Math.Round((double) num3, 2) * inner.PreferredWidth;
    }
  }

  internal void UpdateGridSpan()
  {
    if (this.TableGrid.Count <= 0)
      return;
    foreach (WTableRow childEntity1 in (CollectionImpl) this.ChildEntities)
    {
      foreach (WTableCell childEntity2 in (CollectionImpl) childEntity1.ChildEntities)
        childEntity2.GridSpan = childEntity1.RowFormat.GetGridCount(childEntity2.Index);
    }
  }

  internal void UpdateGridSpan(WTable table)
  {
    if (table.m_tableGrid == null || table.m_tableGrid.Count <= 0)
      return;
    foreach (WTableRow childEntity1 in (CollectionImpl) this.ChildEntities)
    {
      foreach (WTableCell childEntity2 in (CollectionImpl) childEntity1.ChildEntities)
        childEntity2.GridSpan = childEntity1.RowFormat.GetGridCount(childEntity2.Index);
    }
  }

  private bool IsAutoWidth(float parentCellWidth, float tableWidth)
  {
    bool flag = this.TableFormat.PreferredWidth.WidthType == FtsWidth.Auto || this.TableFormat.PreferredWidth.WidthType == FtsWidth.None;
    if (this.IsInCell && ((double) parentCellWidth < (double) tableWidth || this.TableFormat.PreferredWidth.WidthType == FtsWidth.Percentage))
      flag = false;
    return flag;
  }

  private int GetRowSpan(WTableCell wCell)
  {
    int rowSpan = 1;
    if (wCell.OwnerRow.NextSibling != null && wCell.CellFormat.VerticalMerge == CellMerge.Start)
    {
      WTableRow ownerRow = wCell.OwnerRow;
      int num1 = (int) ownerRow.RowFormat.GridBefore;
      if (num1 < 0)
        num1 = 0;
      foreach (WTableCell cell in (CollectionImpl) ownerRow.Cells)
      {
        if (cell != wCell)
          num1 += (int) cell.GridSpan;
        else
          break;
      }
      for (int index = ownerRow.GetRowIndex() + 1; index < this.Rows.Count; ++index)
      {
        WTableRow row = this.Rows[index];
        int num2 = (int) row.RowFormat.GridBefore;
        if (num2 < 0)
          num2 = 0;
        WTableCell wtableCell = (WTableCell) null;
        foreach (WTableCell cell in (CollectionImpl) row.Cells)
        {
          if (num1 == num2)
          {
            wtableCell = cell;
            break;
          }
          num2 += (int) cell.GridSpan;
        }
        if (wtableCell != null && wtableCell.CellFormat.VerticalMerge == CellMerge.Continue)
          ++rowSpan;
        else
          break;
      }
    }
    return rowSpan;
  }

  internal float GetCellWidth(
    float preferredWidth,
    FtsWidth preferredWidthType,
    float containerWidth,
    WTableCell cell)
  {
    float cellWidth = preferredWidth;
    switch (preferredWidthType)
    {
      case FtsWidth.Percentage:
        cellWidth = (float) ((double) preferredWidth * (double) containerWidth / 100.0);
        break;
      case FtsWidth.Point:
        cellWidth = preferredWidth;
        break;
      default:
        if (cell != null)
        {
          cellWidth = this.GetMinimumPreferredWidth(cell);
          break;
        }
        break;
    }
    return cellWidth;
  }

  internal float GetMinimumPreferredWidth(WTableCell cell)
  {
    return (double) cell.CellFormat.PreferredWidth.Width != 0.0 || (double) cell.CellFormat.CellWidth == 0.0 ? cell.GetMinimumPreferredWidth() : cell.CellFormat.CellWidth;
  }

  private void SetNewWidthToCells(bool isTableGridCorrupts)
  {
    WTableColumnCollection tableGrid = this.TableGrid;
    if (isTableGridCorrupts && !this.IsInCell && this.IsTableBasedOnContent(this) && this.HasOnlyParagraphs)
    {
      this.SetParagraphWidthToCells();
      this.UpdateRowBeforeAfter(this);
      tableGrid.UpdateEndOffset();
      this.m_tableWidth = this.UpdateWidth();
    }
    else
    {
      float totalWidth1 = tableGrid.GetTotalWidth((byte) 0);
      float ownerWidth = this.GetOwnerWidth();
      WSection ownerSection = this.GetOwnerSection();
      float num1 = 0.0f;
      bool flag1 = false;
      foreach (WTableRow row in (CollectionImpl) this.Rows)
      {
        float rowPreferredWidth = row.GetRowPreferredWidth(this.m_tableWidth);
        if ((double) rowPreferredWidth > (double) num1)
          num1 = rowPreferredWidth;
      }
      if (this.m_doc.IsDOCX() && (Math.Round((double) totalWidth1, 2) < Math.Round((double) ownerWidth, 2) || Math.Round((double) totalWidth1, 2) > Math.Round((double) ownerWidth, 2) && (double) totalWidth1 < (double) ownerSection.PageSetup.PageSize.Width) && !this.IsInCell && this.TableFormat.IsAutoResized && this.PreferredTableWidth.WidthType == FtsWidth.Auto && Math.Round((double) num1 / 20.0) <= Math.Round((double) totalWidth1) && this.MaximumCellCount() == tableGrid.Count && this.TableFormat.Positioning.AllowOverlap)
      {
        float totalWidth2 = tableGrid.GetTotalWidth((byte) 2);
        float totalWidth3 = tableGrid.GetTotalWidth((byte) 3);
        bool flag2 = (double) totalWidth3 <= (double) ownerWidth;
        bool flag3 = (double) totalWidth2 > (double) ownerWidth || !flag2 || (double) totalWidth2 > (double) totalWidth3;
        if (isTableGridCorrupts && (double) this.PreferredTableWidth.Width == 0.0 && this.IsAllCellsHaveAutoZeroWidth() && this.IsRecalulateBasedOnLastCol && this.IsColumnNotHaveEnoughWidth(ownerWidth) && (flag2 || flag3))
        {
          flag1 = true;
          if (flag3)
          {
            for (int index = 0; index < tableGrid.Count; ++index)
              tableGrid[index].PreferredWidth = tableGrid[index].MaximumWordWidth;
            float num2 = ownerWidth - totalWidth2;
            if ((double) num2 > 0.0)
            {
              float widthFromLastColumn = this.GetMaxNestedTableWidthFromLastColumn();
              WTableColumn wtableColumn = tableGrid[tableGrid.Count - 1];
              if ((double) wtableColumn.PreferredWidth < (double) widthFromLastColumn && (double) widthFromLastColumn - (double) wtableColumn.PreferredWidth <= (double) num2)
                wtableColumn.PreferredWidth = widthFromLastColumn;
            }
          }
          else if (flag2)
          {
            for (int index = 0; index < tableGrid.Count; ++index)
              tableGrid[index].PreferredWidth = tableGrid[index].MaxParaWidth;
            float num3 = ownerWidth - totalWidth3;
            if ((double) num3 > 0.0)
            {
              float widthFromLastColumn = this.GetMaxNestedTableWidthFromLastColumn();
              WTableColumn wtableColumn = tableGrid[tableGrid.Count - 1];
              if ((double) wtableColumn.PreferredWidth < (double) widthFromLastColumn && (double) widthFromLastColumn - (double) wtableColumn.PreferredWidth <= (double) num3)
                wtableColumn.PreferredWidth = widthFromLastColumn;
            }
          }
        }
        else
        {
          float num4 = this.TableFormat.HorizontalAlignment != RowAlignment.Right ? this.IndentFromLeft : 0.0f;
          float num5 = (double) totalWidth1 < (double) ownerWidth ? ownerWidth - (totalWidth1 + num4) : ownerSection.PageSetup.PageSize.Width - totalWidth1;
          for (int index = 0; index < tableGrid.Count; ++index)
          {
            float num6 = tableGrid[index].MaximumWordWidth - tableGrid[index].PreferredWidth;
            if ((double) tableGrid[index].MaximumWordWidth > 0.0 && tableGrid[index].HasMaximumWordWidth && (double) tableGrid[index].MaximumWordWidth > (double) tableGrid[index].PreferredWidth && (double) num6 > 0.0 && (double) num5 - (double) num6 >= 0.0)
            {
              tableGrid[index].PreferredWidth = tableGrid[index].MaximumWordWidth;
              num5 -= num6;
            }
          }
        }
      }
      if (this.m_doc.IsDOCX() && this.IsTableGridCorrupted && !this.IsInCell && !this.TableFormat.IsAutoResized && this.IsAllCellsHaveAutoZeroWidth() && this.TableFormat.PreferredWidth.WidthType == FtsWidth.Percentage)
      {
        float num7 = this.GetTableClientWidth(ownerWidth) / 100f * this.TableFormat.PreferredWidth.Width / (float) tableGrid.Count;
        for (int index = 0; index < tableGrid.Count; ++index)
          tableGrid[index].PreferredWidth = num7;
      }
      foreach (WTableRow row in (CollectionImpl) this.Rows)
      {
        short gridBefore = row.RowFormat.GridBefore;
        short gridAfter = row.RowFormat.GridAfter;
        if (gridBefore > (short) 0)
          row.RowFormat.BeforeWidth = tableGrid.GetCellWidth(0, (int) gridBefore);
        for (int index = 0; index < row.Cells.Count; ++index)
        {
          WTableCell cell = row.Cells[index];
          float cellWidth = tableGrid.GetCellWidth((int) cell.GridColumnStartIndex, (int) cell.GridSpan);
          if (cell.GridSpan > (short) 0 && cell.IsFitAsPerMaximumWordWidth(cellWidth, tableGrid[(int) cell.GridColumnStartIndex].MaximumWordWidth) && this.HasSpaceToConsiderMaxWordWidth(cell.GridColumnStartIndex))
          {
            tableGrid[(int) cell.GridColumnStartIndex].PreferredWidth = tableGrid[(int) cell.GridColumnStartIndex].MaximumWordWidth;
            cell.CellFormat.CellWidth = tableGrid[(int) cell.GridColumnStartIndex].MaximumWordWidth;
          }
          else
            cell.CellFormat.CellWidth = cellWidth;
        }
        if (gridAfter > (short) 0)
          row.RowFormat.AfterWidth = tableGrid.GetCellWidth(row.Cells.Count, (int) gridAfter);
      }
      tableGrid.UpdateEndOffset();
      if (flag1 || isTableGridCorrupts && (double) this.PreferredTableWidth.Width == 0.0 && this.IsAllCellsHaveAutoZeroWidth() && !this.IsInCell && this.TableFormat.IsAutoResized && this.PreferredTableWidth.WidthType == FtsWidth.Auto)
        this.m_tableWidth = this.UpdateWidth();
      if (!this.IsInCell || !isTableGridCorrupts)
        return;
      this.SetNewWidthToNestedTableCells();
    }
  }

  private float GetMaxNestedTableWidthFromLastColumn()
  {
    float widthFromLastColumn = 0.0f;
    foreach (WTableRow row in (CollectionImpl) this.Rows)
    {
      foreach (WTable table in (IEnumerable) (row.Cells.LastItem as WTableCell).Tables)
        widthFromLastColumn = (double) table.Width > (double) widthFromLastColumn ? table.Width : widthFromLastColumn;
    }
    return widthFromLastColumn;
  }

  private void SetNewWidthToNestedTableCells()
  {
    WTable ownerTable = this.GetOwnerTable(this.Owner) as WTable;
    WTableColumnCollection tableGrid = this.TableGrid;
    if (!this.IsTableBasedOnContent(ownerTable) || ownerTable.IsInCell || !this.IsTableBasedOnContent(this) || !this.HasOnlyParagraphs || (double) tableGrid.GetTotalWidth((byte) 2) >= (double) tableGrid.GetTotalWidth((byte) 0))
      return;
    for (int index = 0; index < tableGrid.Count; ++index)
      tableGrid[index].PreferredWidth = tableGrid[index].MaximumWordWidth;
    this.SetCellWidthAsColumnPreferredWidth(this, tableGrid);
    this.UpdateRowBeforeAfter(this);
    tableGrid.UpdateEndOffset();
  }

  internal void SetCellWidthAsColumnPreferredWidth(WTable table, WTableColumnCollection columns)
  {
    for (int index1 = 0; index1 < table.Rows.Count; ++index1)
    {
      WTableRow row = this.Rows[index1];
      for (int index2 = 0; index2 < this.Rows[index1].Cells.Count; ++index2)
        row.Cells[index2].Width = columns[index2].PreferredWidth;
    }
  }

  internal void UpdateRowBeforeAfter(WTable table)
  {
    WTableColumnCollection tableGrid = table.TableGrid;
    foreach (WTableRow row in (CollectionImpl) table.Rows)
    {
      short gridBefore = row.RowFormat.GridBefore;
      short gridAfter = row.RowFormat.GridAfter;
      if (gridBefore > (short) 0)
        row.RowFormat.BeforeWidth = tableGrid.GetCellWidth(0, (int) gridBefore);
      if (gridAfter > (short) 0)
        row.RowFormat.AfterWidth = tableGrid.GetCellWidth(row.Cells.Count, (int) gridAfter);
    }
  }

  internal bool IsColumnNotHaveEnoughWidth(float clientWidth)
  {
    WTableColumnCollection tableGrid = this.TableGrid;
    if ((double) tableGrid.GetTotalWidth((byte) 2) <= (double) clientWidth)
    {
      foreach (WTableColumn wtableColumn in (CollectionImpl) tableGrid)
      {
        if ((double) wtableColumn.PreferredWidth < (double) wtableColumn.MaximumWordWidth)
          return true;
      }
    }
    return false;
  }

  private bool IsTableBasedOnContent(WTable table)
  {
    WTableColumnCollection tableGrid = table.TableGrid;
    return this.m_doc.IsDOCX() && table.TableFormat.IsAutoResized && table.PreferredTableWidth.WidthType == FtsWidth.Auto && (double) table.PreferredTableWidth.Width == 0.0 && table.MaximumCellCount() == tableGrid.Count && table.IsAllCellsHaveAutoZeroWidth();
  }

  private void SetParagraphWidthToCells()
  {
    WTableColumnCollection tableGrid = this.TableGrid;
    float ownerWidth = this.GetOwnerWidth();
    float num1 = 0.0f;
    foreach (WTableColumn wtableColumn in (CollectionImpl) tableGrid)
      num1 += wtableColumn.MaxParaWidth;
    if ((double) num1 <= (double) ownerWidth)
    {
      foreach (WTableColumn wtableColumn in (CollectionImpl) tableGrid)
        wtableColumn.PreferredWidth = wtableColumn.MaxParaWidth;
    }
    else
    {
      float num2 = 0.0f;
      foreach (WTableColumn wtableColumn in (CollectionImpl) tableGrid)
      {
        wtableColumn.PreferredWidth = wtableColumn.MaximumWordWidth;
        num2 += wtableColumn.PreferredWidth;
      }
      float num3 = num1 - num2;
      float num4 = ownerWidth - num2;
      foreach (WTableColumn wtableColumn in (CollectionImpl) tableGrid)
      {
        float num5 = (wtableColumn.MaxParaWidth - wtableColumn.PreferredWidth) / num3 * num4;
        wtableColumn.PreferredWidth += num5;
      }
    }
    for (int index1 = 0; index1 < this.Rows.Count; ++index1)
    {
      WTableRow row = this.Rows[index1];
      for (int index2 = 0; index2 < row.Cells.Count; ++index2)
        row.Cells[index2].CellFormat.CellWidth = tableGrid[index2].PreferredWidth;
    }
  }

  private bool IsAllCellsHaveAutoZeroWidth()
  {
    foreach (WTableRow row in (CollectionImpl) this.Rows)
    {
      foreach (WTableCell cell in (CollectionImpl) row.Cells)
      {
        if ((double) cell.PreferredWidth.Width != 0.0 || cell.PreferredWidth.WidthType != FtsWidth.Auto)
          return false;
      }
    }
    return true;
  }

  internal int MaximumCellCount()
  {
    int num = 0;
    foreach (WTableRow row in (CollectionImpl) this.Rows)
    {
      if (row.Cells.Count > num)
        num = row.Cells.Count;
    }
    return num;
  }

  private bool HasSpaceToConsiderMaxWordWidth(short gridColumnStartIndex)
  {
    WTableColumnCollection tableGrid = this.TableGrid;
    float totalWidth = tableGrid.GetTotalWidth((byte) 0);
    WSection ownerSection = this.GetOwnerSection();
    float num1 = !ownerSection.PageSetup.Borders.NoBorder ? ownerSection.PageSetup.PageSize.Width - (ownerSection.PageSetup.Borders.Left.Space + ownerSection.PageSetup.Borders.Right.Space) - totalWidth : ownerSection.PageSetup.PageSize.Width - totalWidth;
    float num2 = tableGrid[(int) gridColumnStartIndex].MaximumWordWidth - tableGrid[(int) gridColumnStartIndex].PreferredWidth;
    return (double) num2 > 0.0 && (double) num1 - (double) num2 > 0.0;
  }

  private void UpdatePreferredWidthProperties(bool updateAllowAutoFit, AutoFitType autoFittype)
  {
    if (updateAllowAutoFit)
      this.TableFormat.IsAutoResized = autoFittype != AutoFitType.FixedColumnWidth;
    switch (autoFittype)
    {
      case AutoFitType.FitToWindow:
        float totalWidth = this.TableGrid.GetTotalWidth((byte) 0);
        this.TableFormat.PreferredWidth.Width = 100f;
        this.TableFormat.PreferredWidth.WidthType = FtsWidth.Percentage;
        IEnumerator enumerator1 = this.Rows.GetEnumerator();
        try
        {
          while (enumerator1.MoveNext())
          {
            foreach (WTableCell cell in (CollectionImpl) ((WTableRow) enumerator1.Current).Cells)
            {
              if (cell.CellFormat.PreferredWidth.WidthType != FtsWidth.Percentage)
              {
                cell.CellFormat.PreferredWidth.WidthType = FtsWidth.Percentage;
                cell.CellFormat.PreferredWidth.Width = (float) ((double) cell.CellFormat.CellWidth / (double) totalWidth * 100.0);
              }
            }
          }
          break;
        }
        finally
        {
          if (enumerator1 is IDisposable disposable)
            disposable.Dispose();
        }
      case AutoFitType.FixedColumnWidth:
        this.TableFormat.ClearPreferredWidthPropertyValue(12);
        this.TableFormat.ClearPreferredWidthPropertyValue(11);
        IEnumerator enumerator2 = this.Rows.GetEnumerator();
        try
        {
          while (enumerator2.MoveNext())
          {
            foreach (WTableCell cell in (CollectionImpl) ((WTableRow) enumerator2.Current).Cells)
            {
              cell.CellFormat.PreferredWidth.WidthType = FtsWidth.Point;
              cell.CellFormat.PreferredWidth.Width = cell.CellFormat.CellWidth;
            }
          }
          break;
        }
        finally
        {
          if (enumerator2 is IDisposable disposable)
            disposable.Dispose();
        }
      default:
        this.ClearPreferredWidths(false);
        break;
    }
  }

  private void ClearPreferredWidths(bool beforeAutoFit)
  {
    this.TableFormat.ClearPreferredWidthPropertyValue(12);
    this.TableFormat.ClearPreferredWidthPropertyValue(11);
    foreach (WTableRow row in (CollectionImpl) this.Rows)
    {
      foreach (WTableCell cell in (CollectionImpl) row.Cells)
      {
        cell.CellFormat.ClearPreferredWidthPropertyValue(14);
        cell.CellFormat.ClearPreferredWidthPropertyValue(13);
      }
    }
    if (!beforeAutoFit)
      return;
    foreach (WTableColumn wtableColumn in (CollectionImpl) this.TableGrid)
      wtableColumn.PreferredWidth = 0.0f;
  }

  private bool IsTablesAnyOneOfRowsCellWidthsDefined(WTableCell tableCell)
  {
    bool flag = false;
    foreach (WTableRow row in (CollectionImpl) tableCell.OwnerRow.OwnerTable.Rows)
    {
      foreach (WTableCell cell in (CollectionImpl) row.Cells)
      {
        if ((double) cell.PreferredWidth.Width != 0.0 || (double) cell.CellFormat.CellWidth != 0.0)
        {
          flag = true;
        }
        else
        {
          flag = false;
          break;
        }
      }
      if (flag)
        return true;
    }
    return false;
  }

  private float GetMaxRowWidth(float clientWidth)
  {
    float maxRowWidth = clientWidth;
    foreach (WTableRow row in (CollectionImpl) this.Rows)
    {
      float widthToResizeCells = row.GetWidthToResizeCells(clientWidth);
      if ((double) widthToResizeCells > (double) maxRowWidth)
        maxRowWidth = widthToResizeCells;
    }
    return maxRowWidth;
  }

  private void UpdateCellWidth(
    WTableRow row,
    float clientWidth,
    float tableWidth,
    float maxRowWidth,
    bool isSkiptoCalculateCellWidth,
    bool isGridafter)
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    float num3 = 0.0f;
    if ((double) row.RowFormat.GridBeforeWidth.Width > 0.0)
      num2 = this.GetGridBeforeAfter(row.RowFormat.GridBeforeWidth, tableWidth);
    if ((double) row.RowFormat.GridAfterWidth.Width > 0.0 && isGridafter)
      num3 = this.GetGridBeforeAfter(row.RowFormat.GridAfterWidth, tableWidth);
    if ((double) row.RowFormat.BeforeWidth > 0.0)
      num1 += row.RowFormat.BeforeWidth * 20f;
    else if ((double) num2 > 0.0)
    {
      num1 += num2;
      row.RowFormat.BeforeWidth = num2 / 20f;
    }
    if ((double) row.RowFormat.AfterWidth > 0.0 && isGridafter)
      num1 += row.RowFormat.AfterWidth * 20f;
    else if ((double) num3 > 0.0 && isGridafter)
    {
      num1 += num3;
      row.RowFormat.AfterWidth = num3 / 20f;
    }
    float num4 = row.GetRowWidth() * 20f;
    float a = num1 + num4;
    float num5 = row.RowFormat.AfterWidth;
    if ((double) num5 > 0.0 && !isGridafter)
      num5 = 0.0f;
    if (!this.TableFormat.IsAutoResized && (this.Document.ActualFormatType != FormatType.Doc || row.Cells.Count <= 0))
      a += (float) ((double) row.GetRowPreferredWidth(tableWidth) - (double) num4 + (double) num2 - (double) row.RowFormat.BeforeWidth * 20.0 + (double) num3 - (double) num5 * 20.0);
    if ((double) a > 0.0 && Math.Round((double) a) != Math.Round((double) tableWidth * 20.0) || this.IsAutoTableExceedsClientWidth() && Math.Round((double) maxRowWidth * 20.0) != Math.Round((double) a))
    {
      float num6 = tableWidth * 20f / a;
      if (this.TableFormat.IsAutoResized)
      {
        if ((double) a <= (double) clientWidth * 20.0 && ((this.PreferredTableWidth.WidthType != FtsWidth.Percentage || (double) this.PreferredTableWidth.Width <= 0.0) && !this.IsTableCellWidthDefined || (double) num6 <= 1.0) || Math.Round((double) maxRowWidth * 20.0) == Math.Round((double) a))
          return;
        if ((double) a > (double) clientWidth * 20.0)
          num6 = (float) (((double) tableWidth > (double) maxRowWidth ? (double) tableWidth : (double) maxRowWidth) * 20.0) / a;
        row.RowFormat.BeforeWidth *= num6;
        row.RowFormat.AfterWidth *= num6;
        foreach (WTableCell cell in (CollectionImpl) row.Cells)
          cell.CellFormat.CellWidth *= num6;
      }
      else
      {
        if (row.RowFormat.GridBeforeWidth.WidthType >= FtsWidth.Percentage)
          row.RowFormat.BeforeWidth = num2 / 20f * num6;
        if (row.RowFormat.GridAfterWidth.WidthType >= FtsWidth.Percentage)
          row.RowFormat.AfterWidth = num3 / 20f * num6;
        if (isSkiptoCalculateCellWidth)
          return;
        if (!this.Document.IsOpening && !this.Document.IsCloning && this.Document.IsDOCX() && !this.IsInCell && (double) this.PreferredTableWidth.Width > 0.0 && this.PreferredTableWidth.WidthType == FtsWidth.Point && !this.HasPercentPreferredCellWidth && !this.HasAutoPreferredCellWidth && this.HasPointPreferredCellWidth && this.HasNonePreferredCellWidth)
        {
          int defaultPrefCellWidth = 18;
          float preferredWidthFromPoint = row.GetRowPreferredWidthFromPoint(defaultPrefCellWidth);
          float num7 = tableWidth / preferredWidthFromPoint;
          if (row.RowFormat.GridBeforeWidth.WidthType >= FtsWidth.Percentage)
            row.RowFormat.BeforeWidth = num2 / 20f * num7;
          if (row.RowFormat.GridAfterWidth.WidthType >= FtsWidth.Percentage)
            row.RowFormat.AfterWidth = num3 / 20f * num7;
          float totalWidthToShrink = 0.0f;
          float totalWidthToExpand = 0.0f;
          if ((double) preferredWidthFromPoint >= (double) this.PreferredTableWidth.Width)
            totalWidthToShrink = preferredWidthFromPoint - this.PreferredTableWidth.Width;
          else
            totalWidthToExpand = this.PreferredTableWidth.Width - preferredWidthFromPoint;
          foreach (WTableCell cell in (CollectionImpl) row.Cells)
          {
            if (cell.CellFormat.VerticalMerge == CellMerge.Continue)
              this.CalculateCellWidthFixedTable(cell.GetVerticalMergeStartCell() ?? cell, preferredWidthFromPoint, defaultPrefCellWidth, totalWidthToShrink, totalWidthToExpand);
            else
              this.CalculateCellWidthFixedTable(cell, preferredWidthFromPoint, defaultPrefCellWidth, totalWidthToShrink, totalWidthToExpand);
          }
        }
        else
        {
          foreach (WTableCell cell in (CollectionImpl) row.Cells)
          {
            if (cell.CellFormat.VerticalMerge == CellMerge.Continue)
            {
              WTableCell wtableCell = cell.GetVerticalMergeStartCell() ?? cell;
              if (wtableCell.PreferredWidth.WidthType == FtsWidth.Point)
                cell.CellFormat.CellWidth = wtableCell.PreferredWidth.Width * num6;
              else if (wtableCell.PreferredWidth.WidthType == FtsWidth.Percentage)
                cell.CellFormat.CellWidth = (float) ((double) tableWidth * (double) wtableCell.PreferredWidth.Width / 100.0) * num6;
            }
            else if ((double) cell.PreferredWidth.Width != 0.0)
            {
              if (cell.PreferredWidth.WidthType == FtsWidth.Point && !this.UsePreferredCellWidth)
                cell.CellFormat.CellWidth = cell.PreferredWidth.Width * num6;
              else if (cell.PreferredWidth.WidthType == FtsWidth.Percentage)
                cell.CellFormat.CellWidth = (float) ((double) tableWidth * (double) cell.PreferredWidth.Width / 100.0) * num6;
            }
          }
        }
      }
    }
    else
    {
      if (this.TableFormat.IsAutoResized || this.Document.ActualFormatType == FormatType.Doc && row.Cells.Count > 0)
        return;
      if (row.RowFormat.GridBeforeWidth.WidthType >= FtsWidth.Percentage)
        row.RowFormat.BeforeWidth = num2 / 20f;
      if (row.RowFormat.GridAfterWidth.WidthType >= FtsWidth.Percentage)
        row.RowFormat.AfterWidth = num3 / 20f;
      foreach (WTableCell cell in (CollectionImpl) row.Cells)
      {
        if (cell.CellFormat.VerticalMerge == CellMerge.Continue)
        {
          WTableCell wtableCell = cell.GetVerticalMergeStartCell() ?? cell;
          if (wtableCell.PreferredWidth.WidthType == FtsWidth.Point)
            cell.CellFormat.CellWidth = wtableCell.PreferredWidth.Width;
          else if (wtableCell.PreferredWidth.WidthType == FtsWidth.Percentage)
            cell.CellFormat.CellWidth = (float) ((double) tableWidth * (double) wtableCell.PreferredWidth.Width / 100.0);
        }
        else if ((double) cell.PreferredWidth.Width != 0.0)
        {
          if (cell.PreferredWidth.WidthType == FtsWidth.Point)
            cell.CellFormat.CellWidth = cell.PreferredWidth.Width;
          else if (cell.PreferredWidth.WidthType == FtsWidth.Percentage)
            cell.CellFormat.CellWidth = (float) ((double) tableWidth * (double) cell.PreferredWidth.Width / 100.0);
        }
      }
    }
  }

  private void CalculateCellWidthFixedTable(
    WTableCell cell,
    float rowWidth,
    int defaultPrefCellWidth,
    float totalWidthToShrink,
    float totalWidthToExpand)
  {
    if (cell.PreferredWidth.WidthType == FtsWidth.Point)
    {
      float num = cell.PreferredWidth.Width / rowWidth;
      cell.CellFormat.CellWidth = (double) rowWidth >= (double) this.PreferredTableWidth.Width ? cell.PreferredWidth.Width - num * totalWidthToShrink : cell.PreferredWidth.Width + num * totalWidthToExpand;
    }
    else
    {
      if (cell.PreferredWidth.WidthType != FtsWidth.None)
        return;
      float num = (float) defaultPrefCellWidth / rowWidth;
      cell.CellFormat.CellWidth = (double) rowWidth >= (double) this.PreferredTableWidth.Width ? (float) defaultPrefCellWidth - num * totalWidthToShrink : (float) defaultPrefCellWidth + num * totalWidthToExpand;
    }
  }

  internal bool IsAutoTableExceedsClientWidth()
  {
    WSection ownerSection = this.GetOwnerSection((Entity) this) as WSection;
    return this.m_doc.IsDOCX() && !this.IsInCell && this.TableFormat.IsAutoResized && !this.TableFormat.WrapTextAround && this.PreferredTableWidth.WidthType == FtsWidth.Point && ownerSection != null && (double) this.PreferredTableWidth.Width > (double) ownerSection.PageSetup.ClientWidth;
  }

  private bool IsNeedtoResizeCell(float tableWidth)
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    if (this.Document.ActualFormatType != FormatType.Doc || this.TableFormat.IsAutoResized)
      return false;
    foreach (WTableRow row in (CollectionImpl) this.Rows)
    {
      foreach (WTableCell cell in (CollectionImpl) row.Cells)
      {
        if (cell.PreferredWidth.WidthType != FtsWidth.Point && cell.PreferredWidth.WidthType != FtsWidth.Percentage || (double) cell.PreferredWidth.Width <= 0.0)
          return false;
        num1 += cell.PreferredWidth.WidthType == FtsWidth.Percentage ? (float) ((double) cell.PreferredWidth.Width * (double) tableWidth / 100.0) : cell.PreferredWidth.Width;
        num2 += cell.CellFormat.CellWidth;
      }
      if ((double) num1 > (double) num2)
        return false;
    }
    return true;
  }

  private float GetGridBeforeAfter(
    WTableRow row,
    float clientWidth,
    bool isAfterWidth,
    float tableWidth,
    float currOffset,
    float maxRowWidth,
    bool isTableGridMissMatch)
  {
    float gridBeforeAfter = (float) ((isAfterWidth ? (double) row.RowFormat.AfterWidth : (double) row.RowFormat.BeforeWidth) * 20.0);
    PreferredWidthInfo preferredWidthInfo = isAfterWidth ? row.RowFormat.GridAfterWidth : row.RowFormat.GridBeforeWidth;
    float num = 0.0f;
    if (!this.TableFormat.IsAutoResized && (this.PreferredTableWidth.WidthType < FtsWidth.Percentage || (double) this.PreferredTableWidth.Width == 0.0) || isTableGridMissMatch)
    {
      if (preferredWidthInfo.WidthType == FtsWidth.Point)
        num = preferredWidthInfo.Width * 20f;
      else if (preferredWidthInfo.WidthType == FtsWidth.Percentage)
        num = (float) ((double) tableWidth * (double) preferredWidthInfo.Width / 5.0);
    }
    if ((double) gridBeforeAfter < (double) num || isTableGridMissMatch)
    {
      if (isAfterWidth)
        row.RowFormat.AfterWidth = num / 20f;
      else
        row.RowFormat.BeforeWidth = num / 20f;
      gridBeforeAfter = num;
    }
    if (this.PreferredTableWidth.WidthType >= FtsWidth.Percentage && (double) this.PreferredTableWidth.Width > 0.0 && Math.Round((double) tableWidth * 20.0) < Math.Round((double) currOffset + (double) gridBeforeAfter))
    {
      if (this.TableFormat.IsAutoResized)
      {
        if ((double) currOffset + (double) gridBeforeAfter > (double) clientWidth * 20.0 && Math.Round((double) maxRowWidth * 20.0) != Math.Round((double) currOffset + (double) gridBeforeAfter))
        {
          gridBeforeAfter = clientWidth * 20f - currOffset;
          if (isAfterWidth)
            row.RowFormat.AfterWidth = gridBeforeAfter / 20f;
          else
            row.RowFormat.BeforeWidth = gridBeforeAfter / 20f;
        }
      }
      else
      {
        gridBeforeAfter = tableWidth * 20f - currOffset;
        if (isAfterWidth)
          row.RowFormat.AfterWidth = gridBeforeAfter / 20f;
        else
          row.RowFormat.BeforeWidth = gridBeforeAfter / 20f;
      }
    }
    return gridBeforeAfter;
  }

  private float GetGridBeforeAfter(PreferredWidthInfo widthInfo, float tableWidth)
  {
    float gridBeforeAfter = 0.0f;
    if (widthInfo.WidthType == FtsWidth.Point)
      gridBeforeAfter = widthInfo.Width * 20f;
    else if (widthInfo.WidthType == FtsWidth.Percentage)
      gridBeforeAfter = (float) ((double) tableWidth * (double) widthInfo.Width / 5.0);
    return gridBeforeAfter;
  }

  private float GetCellWidth(
    WTableCell cell,
    float clientWidth,
    float tableWidth,
    float currOffset,
    float maxRowWidth,
    bool isTableGridMissMatch)
  {
    float cellWidth = cell.Width * 20f;
    float num1 = 0.0f;
    if (!this.TableFormat.IsAutoResized && (this.PreferredTableWidth.WidthType < FtsWidth.Percentage || (double) this.PreferredTableWidth.Width == 0.0) || isTableGridMissMatch)
    {
      if (cell.CellFormat.VerticalMerge == CellMerge.Continue)
      {
        WTableCell wtableCell = cell.GetVerticalMergeStartCell() ?? cell;
        if (wtableCell.PreferredWidth.WidthType == FtsWidth.Point)
          num1 = wtableCell.PreferredWidth.Width * 20f;
        else if (wtableCell.PreferredWidth.WidthType == FtsWidth.Percentage)
          num1 = (float) ((double) tableWidth * (double) wtableCell.PreferredWidth.Width / 5.0);
      }
      else if (cell.PreferredWidth.WidthType == FtsWidth.Point)
        num1 = cell.PreferredWidth.Width * 20f;
      else if (cell.PreferredWidth.WidthType == FtsWidth.Percentage)
      {
        float preferredWidth = 0.0f;
        cell.OwnerRow.GetRowWidth(ref preferredWidth, tableWidth);
        float num2 = (double) preferredWidth == 0.0 ? 1f : 100f / preferredWidth;
        num1 = tableWidth * (cell.PreferredWidth.Width / 5f * num2);
      }
      else if (cell.PreferredWidth.WidthType == FtsWidth.Auto && !this.IsTablesAnyOneOfRowsCellWidthsDefined(cell))
        num1 = (float) (((double) tableWidth > 0.0 ? (double) tableWidth : (double) clientWidth) / (double) cell.OwnerRow.Cells.Count * 20.0);
    }
    bool flag = this.Document != null && this.Document.IsOpening && (this.Document.ActualFormatType.ToString().Contains("Docx") || this.Document.ActualFormatType.ToString().Contains("Word")) && !this.IsInCell && !isTableGridMissMatch && !this.TableFormat.IsAutoResized && (this.PreferredTableWidth.WidthType < FtsWidth.Percentage || (double) this.PreferredTableWidth.Width == 0.0);
    if ((double) num1 != 0.0 && ((double) cellWidth < (double) num1 || isTableGridMissMatch || flag))
    {
      cell.CellFormat.CellWidth = num1 / 20f;
      cellWidth = num1;
    }
    if (this.PreferredTableWidth.WidthType >= FtsWidth.Percentage && (double) this.PreferredTableWidth.Width > 0.0 && Math.Round((double) tableWidth * 20.0) < Math.Round((double) currOffset + (double) cellWidth))
    {
      if (this.TableFormat.IsAutoResized)
      {
        if ((double) currOffset + (double) cellWidth > (double) clientWidth * 20.0 && Math.Round((double) maxRowWidth * 20.0) != Math.Round((double) currOffset + (double) cellWidth))
        {
          cellWidth = clientWidth * 20f - currOffset;
          cell.CellFormat.CellWidth = cellWidth / 20f;
        }
      }
      else
      {
        cellWidth = tableWidth * 20f - currOffset;
        cell.CellFormat.CellWidth = cellWidth / 20f;
      }
    }
    return cellWidth;
  }

  internal float GetOwnerWidth()
  {
    float ownerWidth = 0.0f;
    if (this.IsInCell)
    {
      WTableCell ownerTableCell = this.GetOwnerTableCell();
      float width = ownerTableCell.Width;
      if (ownerTableCell.CellFormat.HorizontalMerge == CellMerge.Start)
      {
        for (WTableCell nextSibling = ownerTableCell.NextSibling as WTableCell; nextSibling != null && nextSibling.CellFormat.HorizontalMerge == CellMerge.Continue; nextSibling = nextSibling.NextSibling as WTableCell)
          width += nextSibling.Width;
      }
      Paddings paddings = ownerTableCell.OwnerRow == null || ownerTableCell.OwnerRow.OwnerTable == null ? (ownerTableCell.OwnerRow != null ? ownerTableCell.OwnerRow.RowFormat.Paddings : ownerTableCell.CellFormat.Paddings) : ownerTableCell.OwnerRow.OwnerTable.TableFormat.Paddings;
      float num1 = ownerTableCell.OwnerRow == null || ownerTableCell.OwnerRow.OwnerTable == null ? (ownerTableCell.OwnerRow != null ? ownerTableCell.OwnerRow.RowFormat.CellSpacing : 0.0f) : ownerTableCell.OwnerRow.OwnerTable.TableFormat.CellSpacing;
      float num2 = ownerTableCell.CellFormat.Paddings.Left;
      if (ownerTableCell.CellFormat.SamePaddingsAsTable)
      {
        if (paddings.HasKey(1))
          num2 = paddings.Left;
        else if (this.Document.ActualFormatType != FormatType.Doc || !this.Document.IsOpening)
          num2 = 5.4f;
      }
      float num3 = ownerTableCell.CellFormat.Paddings.Right;
      if (ownerTableCell.CellFormat.SamePaddingsAsTable)
      {
        if (paddings.HasKey(4))
          num3 = paddings.Right;
        else if (this.Document.ActualFormatType != FormatType.Doc || !this.Document.IsOpening)
          num3 = 5.4f;
      }
      if ((double) num1 > 0.0)
      {
        num2 += num1 * 2f + this.TableFormat.Borders.Left.GetLineWidthValue();
        num3 += num1 * 2f + this.TableFormat.Borders.Right.GetLineWidthValue();
      }
      ownerWidth = width - (num2 + num3);
    }
    else if (this.Owner != null && this.Owner.OwnerBase is WTextBox)
    {
      WTextBox ownerBase = this.Owner.OwnerBase as WTextBox;
      float width = ownerBase.TextBoxFormat.Width;
      ownerWidth = !ownerBase.TextBoxFormat.NoLine ? width - (ownerBase.TextBoxFormat.LineWidth + ownerBase.TextBoxFormat.InternalMargin.Left + ownerBase.TextBoxFormat.InternalMargin.Right) : (!this.m_doc.IsDOCX() || this.m_doc.Settings.CompatibilityMode != CompatibilityMode.Word2007 || !this.TableFormat.IsAutoResized || ownerBase.TextBoxFormat.AutoFit || ownerBase.IsShape && ownerBase.Shape.TextFrame.ShapeAutoFit ? width - (ownerBase.TextBoxFormat.InternalMargin.Left + ownerBase.TextBoxFormat.InternalMargin.Right) : width - ownerBase.TextBoxFormat.InternalMargin.Left + 7.2f);
    }
    else
    {
      WSection ownerSection = this.GetOwnerSection();
      if (ownerSection != null)
        ownerWidth = (float) ((ownerSection.Columns.Count <= 1 || this.OwnerTextBody is HeaderFooter ? (double) ownerSection.PageSetup.ClientWidth : (double) ownerSection.Columns[0].Width) - (this.Document.DOP.GutterAtTop ? 0.0 : (double) ownerSection.PageSetup.Margins.Gutter));
      else if (this.Document.LastSection != null)
        ownerWidth = this.Document.LastSection.PageSetup.ClientWidth - (this.Document.DOP.GutterAtTop ? 0.0f : this.Document.LastSection.PageSetup.Margins.Gutter);
    }
    if ((double) ownerWidth < 0.0)
      ownerWidth = 0.0f;
    return ownerWidth;
  }

  internal float GetTableClientWidth(float clientWidth)
  {
    float tableClientWidth;
    if (this.PreferredTableWidth.WidthType == FtsWidth.Point && (double) this.PreferredTableWidth.Width > 0.0)
    {
      tableClientWidth = this.PreferredTableWidth.Width;
    }
    else
    {
      tableClientWidth = clientWidth;
      if (this.PreferredTableWidth.WidthType == FtsWidth.Percentage && (double) this.PreferredTableWidth.Width > 0.0)
      {
        tableClientWidth = (float) ((double) tableClientWidth * (double) this.PreferredTableWidth.Width / 100.0);
        if (this.IsInCell)
        {
          float lineWidthValue1 = this.Rows[0].Cells[0].CellFormat.Borders.Left.GetLineWidthValue();
          if ((double) lineWidthValue1 < (double) this.TableFormat.Borders.Left.LineWidth)
            lineWidthValue1 = this.TableFormat.Borders.Left.GetLineWidthValue();
          float lineWidthValue2 = this.Rows[0].Cells[this.Rows[0].Cells.Count - 1].CellFormat.Borders.Right.GetLineWidthValue();
          if ((double) lineWidthValue2 < (double) this.TableFormat.Borders.Right.LineWidth)
            lineWidthValue2 = this.TableFormat.Borders.Right.GetLineWidthValue();
          tableClientWidth -= (float) ((double) lineWidthValue1 / 2.0 + (double) lineWidthValue2 / 2.0);
        }
        else if (this.Rows[0].Cells.Count > 0)
        {
          if (this.m_doc.IsDOCX() && !this.TableFormat.IsAutoResized && this.m_doc.Settings.CompatibilityMode == CompatibilityMode.Word2013)
          {
            float lineWidthValue3 = this.Rows[0].Cells[0].CellFormat.Borders.Left.GetLineWidthValue();
            if ((double) lineWidthValue3 < (double) this.TableFormat.Borders.Left.LineWidth)
              lineWidthValue3 = this.TableFormat.Borders.Left.GetLineWidthValue();
            float lineWidthValue4 = this.Rows[0].Cells[this.Rows[0].Cells.Count - 1].CellFormat.Borders.Right.GetLineWidthValue();
            if ((double) lineWidthValue4 < (double) this.TableFormat.Borders.Right.LineWidth)
              lineWidthValue4 = this.TableFormat.Borders.Right.GetLineWidthValue();
            tableClientWidth -= (float) ((double) lineWidthValue3 / 2.0 + (double) lineWidthValue4 / 2.0);
          }
          else
          {
            float leftPad = 0.0f;
            float rightPad = 0.0f;
            bool flag1 = !this.TableFormat.Paddings.HasKey(1) && (this.m_style == null || !(this.m_style as WTableStyle).TableProperties.Paddings.HasKey(1));
            bool flag2 = !this.TableFormat.Paddings.HasKey(4) && (this.m_style == null || !(this.m_style as WTableStyle).TableProperties.Paddings.HasKey(4));
            this.CalculatePaddingOfTableWidth(ref leftPad, ref rightPad);
            if (this.Document == null || !this.Document.IsDOCX() || !this.TableFormat.IsAutoResized || this.Document.Settings.CompatibilityMode != CompatibilityMode.Word2013 || (double) this.PreferredTableWidth.Width != 100.0 || !flag1 || !flag2)
              tableClientWidth += leftPad + rightPad;
            if ((double) this.TableFormat.CellSpacing > 0.0)
              tableClientWidth = tableClientWidth + (this.TableFormat.CellSpacing * 2f + this.TableFormat.Borders.Left.GetLineWidthValue()) + (this.TableFormat.CellSpacing * 2f + this.TableFormat.Borders.Right.GetLineWidthValue());
          }
        }
        if ((double) tableClientWidth > (double) clientWidth && this.Owner != null && this.Owner.OwnerBase is WTextBox)
          tableClientWidth = (float) ((double) clientWidth * (double) this.PreferredTableWidth.Width / 100.0);
      }
    }
    if ((double) tableClientWidth < 0.0)
      tableClientWidth = 0.0f;
    return tableClientWidth;
  }

  internal void CalculatePaddingOfTableWidth(ref float leftPad, ref float rightPad)
  {
    leftPad = this.Rows[0].Cells[0].CellFormat.Paddings.Left;
    if (this.Rows[0].Cells[0].CellFormat.SamePaddingsAsTable)
    {
      if (this.TableFormat.Paddings.HasKey(1))
        leftPad = this.TableFormat.Paddings.Left;
      else if (this.m_style != null && (this.m_style as WTableStyle).TableProperties.Paddings.HasKey(1))
        leftPad = (this.m_style as WTableStyle).TableProperties.Paddings.Left;
      else if (this.Document.ActualFormatType != FormatType.Doc)
        leftPad = 5.4f;
    }
    rightPad = this.Rows[0].Cells[this.Rows[0].Cells.Count - 1].CellFormat.Paddings.Right;
    if (!this.Rows[0].Cells[this.Rows[0].Cells.Count - 1].CellFormat.SamePaddingsAsTable)
      return;
    if (this.TableFormat.Paddings.HasKey(4))
      rightPad = this.TableFormat.Paddings.Right;
    else if (this.m_style != null && (this.m_style as WTableStyle).TableProperties.Paddings.HasKey(4))
    {
      rightPad = (this.m_style as WTableStyle).TableProperties.Paddings.Right;
    }
    else
    {
      if (this.Document.ActualFormatType == FormatType.Doc)
        return;
      rightPad = 5.4f;
    }
  }

  private void UpdateTableGrid(float currOffset)
  {
    currOffset = (float) Math.Round((double) currOffset);
    if (this.m_tableGrid.IndexOf(currOffset) >= 0 || (double) currOffset == 0.0)
      return;
    if (this.m_tableGrid.Count > 0)
    {
      int num = 0;
      for (int count = this.m_tableGrid.Count; num < count; ++num)
      {
        if ((double) this.m_tableGrid[num].EndOffset > (double) currOffset)
        {
          this.m_tableGrid.InsertColumn(num, currOffset);
          break;
        }
        if (count == num + 1)
          this.m_tableGrid.AddColumns(currOffset);
      }
    }
    else
      this.m_tableGrid.AddColumns(currOffset);
  }

  internal override TextBodyItem GetNextTextBodyItemValue()
  {
    if (this.NextSibling != null)
      return this.NextSibling as TextBodyItem;
    if (this.IsInCell)
      this.GetOwnerTableCell().GetNextTextBodyItem();
    else if (this.OwnerTextBody != null)
      this.GetNextInSection(this.OwnerTextBody.Owner as WSection);
    return (TextBodyItem) null;
  }

  internal void UpdateFormat(FormatBase format, int propKey)
  {
    switch (format)
    {
      case RowFormat _:
        IEnumerator enumerator1 = this.Rows.GetEnumerator();
        try
        {
          while (enumerator1.MoveNext())
            ((WTableRow) enumerator1.Current).RowFormat.SetPropertyValue(propKey, this.TableFormat.GetPropertyValue(propKey));
          break;
        }
        finally
        {
          if (enumerator1 is IDisposable disposable)
            disposable.Dispose();
        }
      case Border _:
      case Borders _:
        IEnumerator enumerator2 = this.Rows.GetEnumerator();
        try
        {
          while (enumerator2.MoveNext())
            ((WTableRow) enumerator2.Current).RowFormat.Borders.ImportContainer((FormatBase) this.TableFormat.Borders);
          break;
        }
        finally
        {
          if (enumerator2 is IDisposable disposable)
            disposable.Dispose();
        }
      case Paddings _:
        IEnumerator enumerator3 = this.Rows.GetEnumerator();
        try
        {
          while (enumerator3.MoveNext())
            ((WTableRow) enumerator3.Current).RowFormat.Paddings.ImportContainer((FormatBase) this.TableFormat.Paddings);
          break;
        }
        finally
        {
          if (enumerator3 is IDisposable disposable)
            disposable.Dispose();
        }
    }
  }

  internal override void Close()
  {
    if (this.m_rows != null && this.m_rows.Count > 0)
    {
      int count = this.m_rows.Count;
      WTableRow wtableRow = (WTableRow) null;
      for (int index = 0; index < count; ++index)
      {
        this.m_rows[index].Close();
        wtableRow = (WTableRow) null;
      }
      this.m_rows.Close();
      this.m_rows = (WRowCollection) null;
    }
    if (this.m_initTableFormat != null)
    {
      this.m_initTableFormat.Close();
      this.m_initTableFormat = (RowFormat) null;
    }
    if (this.m_tableGrid != null)
    {
      this.m_tableGrid.Close();
      this.m_tableGrid = (WTableColumnCollection) null;
    }
    if (this.m_xmlTblFormat != null)
    {
      this.m_xmlTblFormat.Close();
      this.m_xmlTblFormat = (XmlTableFormat) null;
    }
    if (this.m_style != null)
    {
      this.m_style.Close();
      this.m_style = (IWTableStyle) null;
    }
    if (this.m_trackTableGrid != null)
    {
      this.m_trackTableGrid.Close();
      this.m_trackTableGrid = (WTableColumnCollection) null;
    }
    if (this.m_recalculateTables != null)
    {
      this.m_recalculateTables.Clear();
      this.m_recalculateTables = (List<WTable>) null;
    }
    base.Close();
  }

  internal float UpdateWidth()
  {
    float num1 = 0.0f;
    if (this.Rows.Count > 0 && this.TableGrid != null)
    {
      int index1 = 0;
      for (int count1 = this.Rows.Count; index1 < count1; ++index1)
      {
        float num2 = 0.0f;
        int num3 = 0;
        int index2 = 0;
        for (int count2 = this.Rows[index1].Cells.Count; index2 < count2; ++index2)
        {
          WTableCell cell = this.Rows[index1].Cells[index2];
          if ((double) cell.Width > 1584.0)
          {
            if (num3 + (int) cell.GridSpan - 1 < this.m_tableGrid.Count)
              num2 += (float) ((num3 == 0 ? (double) this.m_tableGrid[num3 + (int) cell.GridSpan - 1].EndOffset : (double) this.m_tableGrid[num3 + (int) cell.GridSpan - 1].EndOffset - (double) this.m_tableGrid[num3 - 1].EndOffset) / 20.0);
            else
              num2 += 1584f;
          }
          else
            num2 += cell.Width;
          num3 += (int) cell.GridSpan;
        }
        if (this.Document != null && this.Document.GrammarSpellingData == null && this.Rows[index1].RowFormat.HorizontalAlignment == RowAlignment.Left)
          num2 += Math.Abs(this.Rows[index1].RowFormat.LeftIndent);
        if ((double) num2 > (double) num1)
          num1 = num2;
      }
    }
    return num1;
  }

  private WSection GetOwnerSection()
  {
    for (IEntity ownerSection = (IEntity) this; ownerSection != null; ownerSection = (IEntity) ownerSection.Owner)
    {
      if (ownerSection.EntityType == EntityType.Section)
        return ownerSection as WSection;
    }
    return (WSection) null;
  }

  internal override void MakeChanges(bool acceptChanges)
  {
    for (int index = 0; index < this.m_rows.Count; ++index)
    {
      WTableRow row = this.m_rows[index];
      foreach (WTableCell cell in (CollectionImpl) row.Cells)
      {
        cell.MakeChanges(acceptChanges);
        if (acceptChanges)
        {
          cell.m_trackCellFormat = (CellFormat) null;
          if (cell.CellFormat.OldPropertiesHash.Count > 0)
            cell.CellFormat.OldPropertiesHash.Clear();
        }
        else if (cell.m_trackCellFormat != null)
        {
          cell.CellFormat.ClearFormatting();
          cell.CellFormat.ImportContainer((FormatBase) cell.TrackCellFormat);
          cell.m_trackCellFormat = (CellFormat) null;
        }
      }
      if (acceptChanges)
      {
        row.m_trackRowFormat = (RowFormat) null;
        if (row.RowFormat.OldPropertiesHash.Count > 0)
          row.RowFormat.OldPropertiesHash.Clear();
      }
      else if (row.m_trackRowFormat != null)
      {
        row.RowFormat.ClearFormatting();
        row.m_trackRowFormat = (RowFormat) null;
      }
      if (row.IsDeleteRevision && acceptChanges || row.IsInsertRevision && !acceptChanges)
      {
        this.m_rows.RemoveAt(index);
        --index;
      }
      if (row.IsInsertRevision && acceptChanges)
        row.IsInsertRevision = false;
    }
    if (this.Rows.Count != 0 || this.OwnerTextBody == null)
      return;
    this.OwnerTextBody.ChildEntities.Remove((IEntity) this);
  }

  internal override void RemoveCFormatChanges()
  {
    foreach (WTableRow row in (CollectionImpl) this.m_rows)
    {
      row.CharacterFormat.RemoveChanges();
      foreach (WTableCell cell in (CollectionImpl) row.Cells)
        cell.CharacterFormat.RemoveChanges();
    }
  }

  internal override void RemovePFormatChanges()
  {
    foreach (WTableRow row in (CollectionImpl) this.m_rows)
      row.RowFormat.RemoveChanges();
  }

  internal override void AcceptCChanges()
  {
    foreach (WTableRow row in (CollectionImpl) this.m_rows)
    {
      row.CharacterFormat.AcceptChanges();
      foreach (WTableCell cell in (CollectionImpl) row.Cells)
        cell.CharacterFormat.AcceptChanges();
    }
  }

  internal override void AcceptPChanges()
  {
    foreach (WTableRow row in (CollectionImpl) this.m_rows)
      row.RowFormat.AcceptChanges();
  }

  internal override bool CheckChangedPFormat()
  {
    foreach (WTableRow row in (CollectionImpl) this.m_rows)
    {
      if (row.RowFormat.IsChangedFormat)
        return true;
    }
    return false;
  }

  internal override bool CheckDeleteRev()
  {
    foreach (WTableRow row in (CollectionImpl) this.m_rows)
    {
      if (row.CharacterFormat.IsDeleteRevision)
        return true;
    }
    return false;
  }

  internal override bool CheckInsertRev()
  {
    foreach (WTableRow row in (CollectionImpl) this.m_rows)
    {
      if (row.CharacterFormat.IsInsertRevision)
        return true;
    }
    return false;
  }

  internal override bool CheckChangedCFormat()
  {
    foreach (WTableRow row in (CollectionImpl) this.m_rows)
    {
      if (row.CharacterFormat.IsChangedFormat)
        return true;
      foreach (WTableCell cell in (CollectionImpl) row.Cells)
      {
        if (cell.CharacterFormat.IsChangedFormat)
          return true;
      }
    }
    return false;
  }

  internal override bool HasTrackedChanges()
  {
    if (this.IsDeleteRevision || this.IsInsertRevision || this.IsChangedCFormat || this.IsChangedPFormat)
      return true;
    foreach (WTableRow row in (CollectionImpl) this.m_rows)
    {
      foreach (WTextBody cell in (CollectionImpl) row.Cells)
      {
        foreach (TextBodyItem textBodyItem in (CollectionImpl) cell.Items)
        {
          if (textBodyItem.HasTrackedChanges())
            return true;
        }
      }
    }
    return false;
  }

  internal bool RemoveChangedTable(bool acceptChanges)
  {
    for (int index = 0; index < this.m_rows.Count; ++index)
    {
      WTableRow row = this.m_rows[index];
      if (row.IsDeleteRevision)
      {
        if (this.m_rows.Count <= 1)
          return true;
        if (acceptChanges)
        {
          this.m_rows.Remove(row);
          --index;
        }
      }
    }
    return false;
  }

  internal override void SetDeleteRev(bool check)
  {
    RowFormat format = this.DocxTableFormat.Format;
    string str = format.HasValue(123) ? format.FormatChangeAuthorName : string.Empty;
    DateTime dateTime = format.HasValue(124) ? format.FormatChangeDateTime : DateTime.MinValue;
    foreach (WTableRow row in (CollectionImpl) this.m_rows)
    {
      row.CharacterFormat.IsDeleteRevision = check;
      if (!string.IsNullOrEmpty(str))
        row.RowFormat.FormatChangeAuthorName = str;
      if (dateTime != DateTime.MinValue)
        row.RowFormat.FormatChangeDateTime = dateTime;
    }
  }

  internal override void SetInsertRev(bool check)
  {
    RowFormat format = this.DocxTableFormat.Format;
    string str = format.HasValue(123) ? format.FormatChangeAuthorName : string.Empty;
    DateTime dateTime = format.HasValue(124) ? format.FormatChangeDateTime : DateTime.MinValue;
    foreach (WTableRow row in (CollectionImpl) this.m_rows)
    {
      row.CharacterFormat.IsInsertRevision = check;
      if (!string.IsNullOrEmpty(str))
        row.RowFormat.FormatChangeAuthorName = str;
      if (dateTime != DateTime.MinValue)
        row.RowFormat.FormatChangeDateTime = dateTime;
    }
  }

  internal override void SetChangedCFormat(bool check)
  {
    foreach (WTableRow row in (CollectionImpl) this.m_rows)
    {
      row.CharacterFormat.IsChangedFormat = check;
      foreach (WTableCell cell in (CollectionImpl) row.Cells)
        cell.CharacterFormat.IsChangedFormat = check;
    }
  }

  internal override void SetChangedPFormat(bool check)
  {
    foreach (WTableRow row in (CollectionImpl) this.m_rows)
      row.RowFormat.IsChangedFormat = check;
  }

  protected override void InitXDLSHolder()
  {
    this.XDLSHolder.AddElement("rows", (object) this.Rows);
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    writer.WriteValue("type", "Table");
  }

  protected override void CreateLayoutInfo()
  {
    this.m_layoutInfo = (ILayoutInfo) new TableLayoutInfo(this);
    IEntity nextSibling = this.NextSibling;
    if (nextSibling is WParagraph)
      this.m_layoutInfo.IsPageBreakItem = (nextSibling as WParagraph).ParagraphFormat.PageBreakBefore;
    else if (nextSibling is WTable && (nextSibling as WTable).Rows.Count > 0 && (nextSibling as WTable).Rows[0].Cells.Count > 0 && (nextSibling as WTable).Rows[0].Cells[0].Paragraphs.Count > 0)
      this.m_layoutInfo.IsPageBreakItem = (nextSibling as WTable).Rows[0].Cells[0].Paragraphs[0].ParagraphFormat.PageBreakBefore;
    if (this.Rows.Count >= 1 && !this.IsHiddenTable(this))
      return;
    this.m_layoutInfo.IsSkip = true;
  }

  internal override void InitLayoutInfo(Entity entity, ref bool isLastTOCEntry)
  {
    this.m_layoutInfo = (ILayoutInfo) null;
    if (this.m_rows == null || this.m_rows.Count <= 0)
      return;
    int count = this.m_rows.Count;
    for (int index = 0; index < count; ++index)
    {
      this.m_rows[index].InitLayoutInfo(entity, ref isLastTOCEntry);
      if (isLastTOCEntry)
        break;
    }
  }

  internal bool IsHiddenRow(int rowIndex, WTable table)
  {
    bool flag = false;
    if (table.Rows[rowIndex].RowFormat.Hidden || table.Rows[rowIndex].CharacterFormat.Hidden)
    {
      for (int index1 = 0; index1 < table.Rows[rowIndex].Cells.Count; ++index1)
      {
        for (int index2 = 0; index2 < table.Rows[rowIndex].Cells[index1].ChildEntities.Count; ++index2)
        {
          if (table.Rows[rowIndex].Cells[index1].ChildEntities[index2] is WParagraph)
            flag = this.IsHiddenParagraph(table.Rows[rowIndex].Cells[index1].ChildEntities[index2] as WParagraph);
          else if (table.Rows[rowIndex].Cells[index1].ChildEntities[index2] is WTable)
            flag = this.IsHiddenTable(table.Rows[rowIndex].Cells[index1].ChildEntities[index2] as WTable);
        }
      }
    }
    return flag;
  }

  internal bool IsHiddenParagraph(WParagraph paragraph)
  {
    bool flag = false;
    for (int index = 0; index < paragraph.ChildEntities.Count; ++index)
    {
      if ((paragraph.ChildEntities[index] is InlineContentControl ? ((paragraph.ChildEntities[index] as InlineContentControl).IsHidden() ? 1 : 0) : ((paragraph.ChildEntities[index] as ParagraphItem).ParaItemCharFormat.Hidden ? 1 : 0)) != 0)
      {
        flag = true;
      }
      else
      {
        flag = false;
        break;
      }
    }
    return paragraph.ChildEntities.Count == 0 && paragraph.BreakCharacterFormat.Hidden || flag;
  }

  internal bool IsHiddenTable(WTable table)
  {
    bool flag = false;
    for (int rowIndex = 0; rowIndex < table.Rows.Count; ++rowIndex)
    {
      if (!this.IsHiddenRow(rowIndex, table))
        return false;
      flag = true;
    }
    return flag;
  }

  int ITableWidget.MaxRowIndex
  {
    get
    {
      int num = 0;
      int maxRowIndex = 0;
      for (int index = 0; index < this.m_rows.Count; ++index)
      {
        if (num < this.m_rows[index].Cells.Count)
        {
          num = this.m_rows[index].Cells.Count;
          maxRowIndex = index;
        }
      }
      return maxRowIndex;
    }
  }

  int ITableWidget.RowsCount => this.m_rows.Count;

  IWidgetContainer ITableWidget.GetCellWidget(int row, int column)
  {
    return (IWidgetContainer) this.m_rows[row].Cells[column];
  }

  IWidget ITableWidget.GetRowWidget(int row) => (IWidget) this.m_rows[row];

  void IWidget.InitLayoutInfo() => this.m_layoutInfo = (ILayoutInfo) null;

  void IWidget.InitLayoutInfo(IWidget widget)
  {
  }
}
