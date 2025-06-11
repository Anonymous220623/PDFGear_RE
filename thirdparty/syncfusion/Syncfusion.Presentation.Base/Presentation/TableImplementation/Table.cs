// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.TableImplementation.Table
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Drawing;
using Syncfusion.Presentation.Layouting;
using Syncfusion.Presentation.RichText;
using Syncfusion.Presentation.SlideImplementation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml;

#nullable disable
namespace Syncfusion.Presentation.TableImplementation;

internal class Table : Shape, ITable, ISlideItem
{
  private int _byte;
  private Syncfusion.Presentation.TableImplementation.Columns _columns;
  private Syncfusion.Presentation.TableImplementation.Rows _rows;
  private Dictionary<BuiltInTableStyle, string> _styleList;
  private BuiltInTableStyle _stylePreset;
  private List<long> _mergedCells;
  private string _id;
  private TableStyle _tableStyle;

  internal Table(BaseSlide slide)
    : base(ShapeType.GraphicFrame, slide)
  {
    this.DrawingType = DrawingType.Table;
    this._rows = new Syncfusion.Presentation.TableImplementation.Rows(this);
    this._columns = new Syncfusion.Presentation.TableImplementation.Columns(this);
    this._styleList = this.AddPresetStyles();
    this._stylePreset = BuiltInTableStyle.MediumStyle2Accent1;
    this._id = "";
  }

  public bool HasFirstColumn
  {
    get => (this._byte & 4) != 0;
    set
    {
      if (value)
        this._byte = (int) (byte) (this._byte | 4);
      else
        this._byte = (int) (byte) (this._byte & 251);
    }
  }

  public bool HasHeaderRow
  {
    get => (this._byte & 2) != 0;
    set
    {
      if (value)
        this._byte = (int) (byte) (this._byte | 2);
      else
        this._byte = (int) (byte) (this._byte & 253);
    }
  }

  public bool HasBandedRows
  {
    get => (this._byte & 32 /*0x20*/) != 0;
    set
    {
      if (value)
        this._byte = (int) (byte) (this._byte | 32 /*0x20*/);
      else
        this._byte = (int) (byte) (this._byte & 223);
    }
  }

  public bool HasLastColumn
  {
    get => (this._byte & 16 /*0x10*/) != 0;
    set
    {
      if (value)
        this._byte = (int) (byte) (this._byte | 16 /*0x10*/);
      else
        this._byte = (int) (byte) (this._byte & 239);
    }
  }

  public bool HasTotalRow
  {
    get => (this._byte & 8) != 0;
    set
    {
      if (value)
        this._byte = (int) (byte) (this._byte | 8);
      else
        this._byte = (int) (byte) (this._byte & 247);
    }
  }

  public bool RightToLeft
  {
    get => (this._byte & 1) != 0;
    set
    {
      if (value)
        this._byte = (int) (byte) (this._byte | 1);
      else
        this._byte = (int) (byte) (this._byte & 254);
    }
  }

  public IRows Rows => (IRows) this._rows;

  public IColumns Columns => (IColumns) this._columns;

  public BuiltInTableStyle BuiltInStyle
  {
    get => this._stylePreset;
    set
    {
      this._stylePreset = value;
      if (value == BuiltInTableStyle.None)
        return;
      this._id = this._styleList[value];
      this.BaseSlide.Presentation.IsStyleChanged = true;
      if (this.BaseSlide.Presentation.StyleList.ContainsKey(this._styleList[value]))
      {
        Dictionary<string, int> styleList;
        string style;
        (styleList = this.BaseSlide.Presentation.StyleList)[style = this._styleList[value]] = styleList[style] + 1;
      }
      else
        this.BaseSlide.Presentation.StyleList.Add(this._styleList[value], 1);
    }
  }

  public bool HasBandedColumns
  {
    get => (this._byte & 64 /*0x40*/) != 0;
    set
    {
      if (value)
        this._byte = (int) (byte) (this._byte | 64 /*0x40*/);
      else
        this._byte = (int) (byte) (this._byte & 191);
    }
  }

  public ICell this[int rowIndex, int columnIndex] => this._rows[rowIndex].Cells[columnIndex];

  public int ColumnsCount => this._columns.Count;

  internal List<long> MergedCells => this._mergedCells ?? (this._mergedCells = new List<long>());

  internal void AddColumn(Column column) => this._columns.AddTableColumn((IColumn) column);

  internal void InsertColumn(Column column, int index)
  {
    this._columns.InserTableColumn(index, (IColumn) column);
  }

  internal Syncfusion.Presentation.Drawing.LineFormat GetAdjacentCellBorder(
    long currentCellIndex,
    BorderType currentBorderType)
  {
    int rowIndex = Cell.GetRowIndex(currentCellIndex) - 1;
    int index = Cell.GetColumnIndex(currentCellIndex) - 1;
    if (currentBorderType == BorderType.Top && rowIndex != 0)
      return this.Rows[rowIndex - 1].Cells[index].CellBorders.BorderBottom as Syncfusion.Presentation.Drawing.LineFormat;
    return currentBorderType == BorderType.Left && index != 0 ? this.Rows[rowIndex].Cells[index - 1].CellBorders.BorderRight as Syncfusion.Presentation.Drawing.LineFormat : (Syncfusion.Presentation.Drawing.LineFormat) null;
  }

  public void InsertColumn(int index)
  {
    if (this.Rows.Count < 0)
      throw new Exception("The row count cannot be negative");
    if (index < 0)
      throw new Exception("The index position cannot be negative");
    if (this.Columns.Count < index)
      throw new Exception("The column value exceeds the column collection");
    double num1 = 0.0;
    Column column1 = new Column(this);
    column1.Width = index == 0 ? this.Rows[0].Cells[index].ColumnWidth : this.Rows[0].Cells[index - 1].ColumnWidth;
    this.InsertColumn(column1, index);
    foreach (Column column2 in (IEnumerable<IColumn>) this.Columns)
      num1 += column2.Width;
    double num2 = num1 / this.ShapeFrame.GetDefaultWidth();
    foreach (Row row in (IEnumerable<IRow>) this.Rows)
    {
      Cell cell = row.CreateCell(index + 1);
      ((Cells) row.Cells).Insert(index, (ICell) cell);
      for (int index1 = index + 1; index1 < row.Cells.Count; ++index1)
        ++((Cell) row.Cells[index1]).CellIndex;
      if (index >= 1)
      {
        int num3 = 0;
        int num4 = 0;
        for (int index2 = index - 1; index2 >= 0; --index2)
        {
          num3 = row.Cells[index2].ColumnSpan;
          ++num4;
          if (num3 > 0)
            break;
        }
        if (num3 >= 2 && index - num4 - 1 + num3 >= index)
          ((Cell) row.Cells[index - num4]).SetColumnSpan(num3 + 1, true);
      }
    }
    column1.Index = index;
    for (int columnIndex = index + 1; columnIndex < this.Columns.Count; ++columnIndex)
      ++(this.Columns[columnIndex] as Column).Index;
    foreach (Column column3 in (IEnumerable<IColumn>) this.Columns)
      column3.Width /= num2;
  }

  internal TableStyle TableStyle
  {
    get
    {
      this._tableStyle = this.GetTblStyleFromStyleList();
      if (this._tableStyle == null && !string.IsNullOrEmpty(this._id))
      {
        using (XmlReader reader = UtilityMethods.CreateReader((Stream) this.BaseSlide.Presentation.DataHolder.GetStreamFromTemplateZip()))
          Parser.ParseTableStyles(reader, this.BaseSlide.Presentation, this._id);
        this._tableStyle = this.GetTblStyleFromStyleList();
      }
      return this._tableStyle;
    }
  }

  private TableStyle GetTblStyleFromStyleList()
  {
    TableStyle styleFromStyleList = (TableStyle) null;
    if (this.BaseSlide.Presentation.TableStyleList != null && this.BaseSlide.Presentation.TableStyleList.ContainsKey(this._id))
      styleFromStyleList = this.BaseSlide.Presentation.TableStyleList[this._id];
    return styleFromStyleList;
  }

  internal IColumn AddColumn(int columnCount, int columnIndex)
  {
    int int32 = Convert.ToInt32(this.ShapeFrame.GetDefaultWidth() / (double) columnCount);
    Column column = new Column(this);
    column.Index = columnIndex;
    column.SetWidth((long) Helper.PointToEmu((double) int32));
    this.AddColumn(column);
    return (IColumn) column;
  }

  internal string Id
  {
    get => this._id;
    set => this._id = value;
  }

  internal void AddRow(Row tableRow) => this._rows.AddRow(tableRow);

  internal IRow AddRow(int rows, int cols)
  {
    Row tableRow = new Row(this);
    int point = Convert.ToInt32(this.ShapeFrame.GetDefaultHeight()) / rows;
    this.AddRow(tableRow);
    for (int index = 0; index < cols; ++index)
    {
      tableRow.AddCell(index + 1);
      tableRow.SetHeight((long) Helper.PointToEmu((double) point));
    }
    return (IRow) tableRow;
  }

  internal Dictionary<BuiltInTableStyle, string> GetStyleList() => this._styleList;

  internal override void Layout()
  {
    float usedHeight1 = 0.0f;
    float maxTextHeight = 0.0f;
    float defaultLeft = (float) this.ShapeFrame.GetDefaultLeft();
    float defaultTop = (float) this.ShapeFrame.GetDefaultTop();
    int count = this.Rows.Count;
    List<float> floatList = new List<float>(this.Rows.Count);
    for (int rowIndex = 0; rowIndex < count; ++rowIndex)
    {
      if (this.Rows[rowIndex] is Row row)
      {
        row.Layout(usedHeight1, defaultLeft, defaultTop, rowIndex, ref maxTextHeight);
        usedHeight1 = this.UpdateRowHeight(usedHeight1, ref maxTextHeight, row);
      }
      floatList.Add(maxTextHeight);
      maxTextHeight = 0.0f;
    }
    foreach (Row row in (IEnumerable<IRow>) this.Rows)
    {
      for (int index1 = 0; index1 < row.Cells.Count; ++index1)
      {
        Cell cell1 = row.Cells[index1] as Cell;
        if (cell1.IsVerticalMerge)
        {
          CellInfo cellInfo1 = cell1.CellInfo;
          int num1 = cell1.ObtainRowIndex() - 1;
          Column column = this.Columns[index1] as Column;
          for (int index2 = num1 - 1; index2 >= 0; --index2)
          {
            float num2 = 0.0f;
            Cell cell2 = column.Cells[index2] as Cell;
            if (cell2.RowSpan > 0 && cell2.RowSpan - 1 == num1 - index2)
            {
              for (int index3 = num1; index3 >= index2; --index3)
                num2 += floatList[index3];
              CellInfo cellInfo2 = cell2.CellInfo;
              if (cellInfo2 != null)
              {
                Syncfusion.Presentation.RichText.TextBody textBody = cell2.TextBody as Syncfusion.Presentation.RichText.TextBody;
                float num3 = cellInfo2.TotalTextHeight + ((float) textBody.GetDefaultTopMargin() + (float) textBody.GetDefaultBottomMargin());
                if ((double) num3 > (double) num2)
                {
                  float height = num3 - num2;
                  floatList[num1] += height;
                  this.UpdateHeightOfRow(num1, height);
                  if (num1 != this.Rows.Count - 1)
                  {
                    for (int rowIndex = num1 + 1; rowIndex < count; ++rowIndex)
                      this.UpdateTopOfRow(rowIndex, height);
                  }
                  num2 = num3;
                }
                cellInfo2.UpdateBoundsHeight(num2);
                cellInfo2.UpdateTextLayoutingBoundsHeight(num2 - ((float) textBody.GetDefaultTopMargin() + (float) textBody.GetDefaultBottomMargin()));
                if (textBody.ObatinTextDirection() == TextDirection.Vertical270)
                {
                  float maxWidth = 0.0f;
                  float usedHeight2 = 0.0f;
                  float defaultLeftMargin = (float) textBody.GetDefaultLeftMargin();
                  float defaultTopMargin = (float) textBody.GetDefaultTopMargin();
                  float defaultRightMargin = (float) textBody.GetDefaultRightMargin();
                  float defaultBottomMargin = (float) textBody.GetDefaultBottomMargin();
                  float left = cellInfo2.Bounds.Left;
                  float top = cellInfo2.Bounds.Top;
                  float width1 = cellInfo2.Bounds.Width;
                  float height1 = cellInfo2.Bounds.Height;
                  float x = left + defaultLeftMargin;
                  float y = top + defaultTopMargin;
                  float height2 = width1 - (defaultLeftMargin + defaultRightMargin);
                  float width2 = height1 - (defaultTopMargin + defaultBottomMargin);
                  cellInfo2.TextLayoutingBounds = new RectangleF(x, y, width2, height2);
                  foreach (Paragraph paragraph in (IEnumerable<IParagraph>) textBody.Paragraphs)
                    paragraph.Layout(cellInfo2.TextLayoutingBounds, ref usedHeight2, textBody.WrapText, ref maxWidth);
                }
                if ((double) num2 == (double) num3 || this.Rows[index2].Height < (double) floatList[index2])
                {
                  cell2.LayoutXYPosition(cellInfo2.TextLayoutingBounds.Height, cellInfo2.TextLayoutingBounds.Width, cellInfo2.TotalTextHeight, cell2.TextBody as Syncfusion.Presentation.RichText.TextBody, cellInfo2.MaxTextWidth);
                  break;
                }
                break;
              }
              break;
            }
          }
        }
      }
    }
  }

  internal void UpdateTopOfRow(int rowIndex, float height)
  {
    foreach (ICell cell1 in (IEnumerable<ICell>) (this.Rows[rowIndex] as Row).Cells)
    {
      Cell cell2 = cell1 as Cell;
      CellInfo cellInfo = cell2.CellInfo;
      if (cellInfo != null)
      {
        Syncfusion.Presentation.RichText.TextBody textBody = cell2.TextBody as Syncfusion.Presentation.RichText.TextBody;
        cellInfo.UpdateBoundsTop(height + cellInfo.Bounds.Y);
        cellInfo.UpdateTextLayoutingBoundsTop(cellInfo.Bounds.Y + (float) textBody.GetDefaultTopMargin());
        this.UpdateTopOfParagraph(textBody, height);
      }
    }
  }

  internal void UpdateTopOfParagraph(Syncfusion.Presentation.RichText.TextBody textBody, float height)
  {
    if (textBody == null)
      return;
    foreach (Paragraph paragraph in (IEnumerable<IParagraph>) textBody.Paragraphs)
    {
      if (paragraph.ParagraphInfo != null)
      {
        foreach (Syncfusion.Presentation.Layouting.LineInfo lineInfo in paragraph.ParagraphInfo.LineInfoCollection)
        {
          if (lineInfo != null)
          {
            foreach (TextInfo textInfo in lineInfo.TextInfoCollection)
            {
              if (textInfo != null)
                textInfo.Bounds = new RectangleF(textInfo.Bounds.X, textInfo.Bounds.Y + height, textInfo.Bounds.Width, textInfo.Bounds.Height);
            }
          }
        }
      }
    }
  }

  internal void UpdateHeightOfRow(int rowIndex, float height)
  {
    Row row = this.Rows[rowIndex] as Row;
    for (int index1 = 0; index1 < row.Cells.Count; ++index1)
    {
      Cell cell1 = row.Cells[index1] as Cell;
      CellInfo cellInfo1 = cell1.CellInfo;
      if (cellInfo1 == null)
      {
        Column column = this.Columns[index1] as Column;
        for (int index2 = rowIndex - 1; index2 >= 0; --index2)
        {
          Cell cell2 = column.Cells[index2] as Cell;
          if (cell2.RowSpan > 0 && cell2.RowSpan - 1 == rowIndex - index2)
          {
            CellInfo cellInfo2 = cell2.CellInfo;
            if (cellInfo2 != null)
            {
              Syncfusion.Presentation.RichText.TextBody textBody = cell2.TextBody as Syncfusion.Presentation.RichText.TextBody;
              cellInfo2.UpdateBoundsHeight(height + cellInfo2.Bounds.Height);
              cellInfo2.UpdateTextLayoutingBoundsHeight(cellInfo2.Bounds.Height - (float) (textBody.GetDefaultTopMargin() + textBody.GetDefaultBottomMargin()));
              cell2.LayoutXYPosition(cellInfo2.TextLayoutingBounds.Height, cellInfo2.TextLayoutingBounds.Width, cellInfo2.TotalTextHeight, cell2.TextBody as Syncfusion.Presentation.RichText.TextBody, cellInfo2.MaxTextWidth);
              break;
            }
            break;
          }
        }
      }
      else
      {
        Syncfusion.Presentation.RichText.TextBody textBody = cell1.TextBody as Syncfusion.Presentation.RichText.TextBody;
        cellInfo1.UpdateBoundsHeight(height + cellInfo1.Bounds.Height);
        cellInfo1.UpdateTextLayoutingBoundsHeight(cellInfo1.Bounds.Height - (float) (textBody.GetDefaultTopMargin() + textBody.GetDefaultBottomMargin()));
        cell1.LayoutXYPosition(cellInfo1.TextLayoutingBounds.Height, cellInfo1.TextLayoutingBounds.Width, cellInfo1.TotalTextHeight, cell1.TextBody as Syncfusion.Presentation.RichText.TextBody, cellInfo1.MaxTextWidth);
      }
    }
  }

  public float GetActualHeight()
  {
    using (Graphics graphics = this.BaseSlide.Presentation.InternalGraphics = Graphics.FromImage((Image) new Bitmap((int) this.BaseSlide.Presentation.SlideSize.Width, (int) this.BaseSlide.Presentation.SlideSize.Height)))
    {
      this.BaseSlide.Presentation.IsInternalGraphics = true;
      Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.SetGraphicsProperties(graphics);
      float usedHeight = 0.0f;
      float maxTextHeight = 0.0f;
      float defaultLeft = (float) this.ShapeFrame.GetDefaultLeft();
      float defaultTop = (float) this.ShapeFrame.GetDefaultTop();
      foreach (Row row in (IEnumerable<IRow>) this.Rows)
      {
        List<Cell> cellList = new List<Cell>();
        if (row != null)
        {
          int rowIndex = this.Rows.IndexOf((IRow) row);
          row.Layout(usedHeight, defaultLeft, defaultTop, rowIndex, ref maxTextHeight);
          usedHeight = this.UpdateRowHeight(usedHeight, ref maxTextHeight, row);
        }
        maxTextHeight = 0.0f;
      }
      if (this.BaseSlide.Presentation.InternalGraphics != null)
      {
        this.BaseSlide.Presentation.InternalGraphics.Dispose();
        this.BaseSlide.Presentation.InternalGraphics = (Graphics) null;
      }
      this.BaseSlide.Presentation.IsInternalGraphics = false;
      return usedHeight;
    }
  }

  private float UpdateRowHeight(float usedHeight, ref float maxTextHeight, Row row)
  {
    if (row.Height > (double) maxTextHeight)
    {
      foreach (ICell cell in (IEnumerable<ICell>) row.Cells)
      {
        CellInfo cellInfo = ((Cell) cell).CellInfo;
        if (cellInfo != null)
          ((Cell) cell).LayoutXYPosition(cellInfo.TextLayoutingBounds.Height, cellInfo.TextLayoutingBounds.Width, cellInfo.TotalTextHeight, cell.TextBody as Syncfusion.Presentation.RichText.TextBody, cellInfo.MaxTextWidth);
      }
      usedHeight += (float) row.Height;
      maxTextHeight = (float) row.Height;
    }
    else
    {
      foreach (ICell cell1 in (IEnumerable<ICell>) row.Cells)
      {
        Cell cell2 = cell1 as Cell;
        CellInfo cellInfo = cell2.CellInfo;
        if (cellInfo != null && cell1.RowSpan <= 1)
        {
          Syncfusion.Presentation.RichText.TextBody textBody = cell2.TextBody as Syncfusion.Presentation.RichText.TextBody;
          cellInfo.UpdateBoundsHeight(maxTextHeight);
          cellInfo.UpdateTextLayoutingBoundsHeight(maxTextHeight - ((float) textBody.GetDefaultTopMargin() + (float) textBody.GetDefaultBottomMargin()));
          cell2.LayoutXYPosition(cellInfo.TextLayoutingBounds.Height, cellInfo.TextLayoutingBounds.Width, cellInfo.TotalTextHeight, cell1.TextBody as Syncfusion.Presentation.RichText.TextBody, cellInfo.MaxTextWidth);
        }
      }
      usedHeight += maxTextHeight;
    }
    return usedHeight;
  }

  private Dictionary<BuiltInTableStyle, string> AddPresetStyles()
  {
    return new Dictionary<BuiltInTableStyle, string>()
    {
      {
        BuiltInTableStyle.NoStyleNoGrid,
        "{2D5ABB26-0587-4C30-8999-92F81FD0307C}"
      },
      {
        BuiltInTableStyle.ThemedStyle1Accent1,
        "{3C2FFA5D-87B4-456A-9821-1D502468CF0F}"
      },
      {
        BuiltInTableStyle.ThemedStyle1Accent2,
        "{284E427A-3D55-4303-BF80-6455036E1DE7}"
      },
      {
        BuiltInTableStyle.ThemedStyle1Accent3,
        "{69C7853C-536D-4A76-A0AE-DD22124D55A5}"
      },
      {
        BuiltInTableStyle.ThemedStyle1Accent4,
        "{775DCB02-9BB8-47FD-8907-85C794F793BA}"
      },
      {
        BuiltInTableStyle.ThemedStyle1Accent5,
        "{35758FB7-9AC5-4552-8A53-C91805E547FA}"
      },
      {
        BuiltInTableStyle.ThemedStyle1Accent6,
        "{08FB837D-C827-4EFA-A057-4D05807E0F7C}"
      },
      {
        BuiltInTableStyle.NoStyleTableGrid,
        "{5940675A-B579-460E-94D1-54222C63F5DA}"
      },
      {
        BuiltInTableStyle.ThemedStyle2Accent1,
        "{D113A9D2-9D6B-4929-AA2D-F23B5EE8CBE7}"
      },
      {
        BuiltInTableStyle.ThemedStyle2Accent2,
        "{18603FDC-E32A-4AB5-989C-0864C3EAD2B8}"
      },
      {
        BuiltInTableStyle.ThemedStyle2Accent3,
        "{306799F8-075E-4A3A-A7F6-7FBC6576F1A4}"
      },
      {
        BuiltInTableStyle.ThemedStyle2Accent4,
        "{E269D01E-BC32-4049-B463-5C60D7B0CCD2}"
      },
      {
        BuiltInTableStyle.ThemedStyle2Accent5,
        "{327F97BB-C833-4FB7-BDE5-3F7075034690}"
      },
      {
        BuiltInTableStyle.ThemedStyle2Accent6,
        "{638B1855-1B75-4FBE-930C-398BA8C253C6}"
      },
      {
        BuiltInTableStyle.LightStyle1,
        "{9D7B26C5-4107-4FEC-AEDC-1716B250A1EF}"
      },
      {
        BuiltInTableStyle.LightStyle1Accent1,
        "{3B4B98B0-60AC-42C2-AFA5-B58CD77FA1E5}"
      },
      {
        BuiltInTableStyle.LightStyle1Accent2,
        "{0E3FDE45-AF77-4B5C-9715-49D594BDF05E}"
      },
      {
        BuiltInTableStyle.LightStyle1Accent3,
        "{C083E6E3-FA7D-4D7B-A595-EF9225AFEA82}"
      },
      {
        BuiltInTableStyle.LightStyle1Accent4,
        "{D27102A9-8310-4765-A935-A1911B00CA55}"
      },
      {
        BuiltInTableStyle.LightStyle1Accent5,
        "{5FD0F851-EC5A-4D38-B0AD-8093EC10F338}"
      },
      {
        BuiltInTableStyle.LightStyle1Accent6,
        "{68D230F3-CF80-4859-8CE7-A43EE81993B5}"
      },
      {
        BuiltInTableStyle.LightStyle2,
        "{7E9639D4-E3E2-4D34-9284-5A2195B3D0D7}"
      },
      {
        BuiltInTableStyle.LightStyle2Accent2,
        "{72833802-FEF1-4C79-8D5D-14CF1EAF98D9}"
      },
      {
        BuiltInTableStyle.LightStyle2Accent1,
        "{69012ECD-51FC-41F1-AA8D-1B2483CD663E}"
      },
      {
        BuiltInTableStyle.LightStyle2Accent3,
        "{F2DE63D5-997A-4646-A377-4702673A728D}"
      },
      {
        BuiltInTableStyle.LightStyle2Accent4,
        "{17292A2E-F333-43FB-9621-5CBBE7FDCDCB}"
      },
      {
        BuiltInTableStyle.LightStyle2Accent5,
        "{5A111915-BE36-4E01-A7E5-04B1672EAD32}"
      },
      {
        BuiltInTableStyle.LightStyle2Accent6,
        "{912C8C85-51F0-491E-9774-3900AFEF0FD7}"
      },
      {
        BuiltInTableStyle.LightStyle3,
        "{616DA210-FB5B-4158-B5E0-FEB733F419BA}"
      },
      {
        BuiltInTableStyle.LightStyle3Accent1,
        "{BC89EF96-8CEA-46FF-86C4-4CE0E7609802}"
      },
      {
        BuiltInTableStyle.LightStyle3Accent2,
        "{5DA37D80-6434-44D0-A028-1B22A696006F}"
      },
      {
        BuiltInTableStyle.LightStyle3Accent3,
        "{8799B23B-EC83-4686-B30A-512413B5E67A}"
      },
      {
        BuiltInTableStyle.LightStyle3Accent4,
        "{ED083AE6-46FA-4A59-8FB0-9F97EB10719F}"
      },
      {
        BuiltInTableStyle.LightStyle3Accent5,
        "{BDBED569-4797-4DF1-A0F4-6AAB3CD982D8}"
      },
      {
        BuiltInTableStyle.LightStyle3Accent6,
        "{E8B1032C-EA38-4F05-BA0D-38AFFFC7BED3}"
      },
      {
        BuiltInTableStyle.MediumStyle1,
        "{793D81CF-94F2-401A-BA57-92F5A7B2D0C5}"
      },
      {
        BuiltInTableStyle.MediumStyle1Accent1,
        "{B301B821-A1FF-4177-AEE7-76D212191A09}"
      },
      {
        BuiltInTableStyle.MediumStyle1Accent2,
        "{9DCAF9ED-07DC-4A11-8D7F-57B35C25682E}"
      },
      {
        BuiltInTableStyle.MediumStyle1Accent3,
        "{1FECB4D8-DB02-4DC6-A0A2-4F2EBAE1DC90}"
      },
      {
        BuiltInTableStyle.MediumStyle1Accent4,
        "{1E171933-4619-4E11-9A3F-F7608DF75F80}"
      },
      {
        BuiltInTableStyle.MediumStyle1Accent5,
        "{FABFCF23-3B69-468F-B69F-88F6DE6A72F2}"
      },
      {
        BuiltInTableStyle.MediumStyle1Accent6,
        "{10A1B5D5-9B99-4C35-A422-299274C87663}"
      },
      {
        BuiltInTableStyle.MediumStyle2,
        "{073A0DAA-6AF3-43AB-8588-CEC1D06C72B9}"
      },
      {
        BuiltInTableStyle.MediumStyle2Accent1,
        "{5C22544A-7EE6-4342-B048-85BDC9FD1C3A}"
      },
      {
        BuiltInTableStyle.MediumStyle2Accent2,
        "{21E4AEA4-8DFA-4A89-87EB-49C32662AFE0}"
      },
      {
        BuiltInTableStyle.MediumStyle2Accent3,
        "{F5AB1C69-6EDB-4FF4-983F-18BD219EF322}"
      },
      {
        BuiltInTableStyle.MediumStyle2Accent4,
        "{00A15C55-8517-42AA-B614-E9B94910E393}"
      },
      {
        BuiltInTableStyle.MediumStyle2Accent5,
        "{7DF18680-E054-41AD-8BC1-D1AEF772440D}"
      },
      {
        BuiltInTableStyle.MediumStyle2Accent6,
        "{93296810-A885-4BE3-A3E7-6D5BEEA58F35}"
      },
      {
        BuiltInTableStyle.MediumStyle3,
        "{8EC20E35-A176-4012-BC5E-935CFFF8708E}"
      },
      {
        BuiltInTableStyle.MediumStyle3Accent1,
        "{6E25E649-3F16-4E02-A733-19D2CDBF48F0}"
      },
      {
        BuiltInTableStyle.MediumStyle3Accent2,
        "{85BE263C-DBD7-4A20-BB59-AAB30ACAA65A}"
      },
      {
        BuiltInTableStyle.MediumStyle3Accent3,
        "{EB344D84-9AFB-497E-A393-DC336BA19D2E}"
      },
      {
        BuiltInTableStyle.MediumStyle3Accent4,
        "{EB9631B5-78F2-41C9-869B-9F39066F8104}"
      },
      {
        BuiltInTableStyle.MediumStyle3Accent5,
        "{74C1A8A3-306A-4EB7-A6B1-4F7E0EB9C5D6}"
      },
      {
        BuiltInTableStyle.MediumStyle3Accent6,
        "{2A488322-F2BA-4B5B-9748-0D474271808F}"
      },
      {
        BuiltInTableStyle.MediumStyle4,
        "{D7AC3CCA-C797-4891-BE02-D94E43425B78}"
      },
      {
        BuiltInTableStyle.MediumStyle4Accent1,
        "{69CF1AB2-1976-4502-BF36-3FF5EA218861}"
      },
      {
        BuiltInTableStyle.MediumStyle4Accent2,
        "{8A107856-5554-42FB-B03E-39F5DBC370BA}"
      },
      {
        BuiltInTableStyle.MediumStyle4Accent3,
        "{0505E3EF-67EA-436B-97B2-0124C06EBD24}"
      },
      {
        BuiltInTableStyle.MediumStyle4Accent4,
        "{C4B1156A-380E-4F78-BDF5-A606A8083BF9}"
      },
      {
        BuiltInTableStyle.MediumStyle4Accent5,
        "{22838BEF-8BB2-4498-84A7-C5851F593DF1}"
      },
      {
        BuiltInTableStyle.MediumStyle4Accent6,
        "{16D9F66E-5EB9-4882-86FB-DCBF35E3C3E4}"
      },
      {
        BuiltInTableStyle.DarkStyle1,
        "{E8034E78-7F5D-4C2E-B375-FC64B27BC917}"
      },
      {
        BuiltInTableStyle.DarkStyle1Accent1,
        "{125E5076-3810-47DD-B79F-674D7AD40C01}"
      },
      {
        BuiltInTableStyle.DarkStyle1Accent2,
        "{37CE84F3-28C3-443E-9E96-99CF82512B78}"
      },
      {
        BuiltInTableStyle.DarkStyle1Accent3,
        "{D03447BB-5D67-496B-8E87-E561075AD55C}"
      },
      {
        BuiltInTableStyle.DarkStyle1Accent4,
        "{E929F9F4-4A8F-4326-A1B4-22849713DDAB}"
      },
      {
        BuiltInTableStyle.DarkStyle1Accent5,
        "{8FD4443E-F989-4FC4-A0C8-D5A2AF1F390B}"
      },
      {
        BuiltInTableStyle.DarkStyle1Accent6,
        "{AF606853-7671-496A-8E4F-DF71F8EC918B}"
      },
      {
        BuiltInTableStyle.DarkStyle2,
        "{5202B0CA-FC54-4496-8BCA-5EF66A818D29}"
      },
      {
        BuiltInTableStyle.DarkStyle2Accent1Accent2,
        "{0660B408-B3CF-4A94-85FC-2B1E0A45F4A2}"
      },
      {
        BuiltInTableStyle.DarkStyle2Accent3Accent4,
        "{91EBBBCC-DAD2-459C-BE2E-F6DE35CF9A28}"
      },
      {
        BuiltInTableStyle.DarkStyle2Accent5Accent6,
        "{46F890A9-2807-4EBB-B81D-B2AA78EC7F39}"
      }
    };
  }

  internal override void Close()
  {
    base.Close();
    this.ClearAll();
  }

  internal void ClearAll()
  {
    if (this._columns != null)
    {
      this._columns.Close();
      this._columns = (Syncfusion.Presentation.TableImplementation.Columns) null;
    }
    if (this._rows != null)
    {
      this._rows.Close();
      this._rows = (Syncfusion.Presentation.TableImplementation.Rows) null;
    }
    if (this._styleList != null)
    {
      this._styleList.Clear();
      this._styleList = (Dictionary<BuiltInTableStyle, string>) null;
    }
    if (this._mergedCells != null)
    {
      this._mergedCells.Clear();
      this._mergedCells = (List<long>) null;
    }
    if (this._tableStyle != null)
    {
      this._tableStyle.Close();
      this._tableStyle = (TableStyle) null;
    }
    base.Close();
  }

  public override ISlideItem Clone()
  {
    Table table = (Table) this.MemberwiseClone();
    this.Clone((Shape) table);
    table._columns = this._columns.Clone();
    table._columns.SetParent(table);
    if (this._mergedCells != null)
      table._mergedCells = Helper.CloneList(this._mergedCells);
    table._rows = this._rows.Clone();
    table._rows.SetParent(table);
    table._styleList = this.CloneBuiltInStyle();
    if (this._tableStyle != null)
      table._tableStyle = this._tableStyle.Clone();
    return (ISlideItem) table;
  }

  internal void SetTableStyleParent(BaseSlide newParent)
  {
    if (this._tableStyle == null)
      return;
    this._tableStyle.SetParent(newParent.Presentation);
  }

  internal override void SetParent(BaseSlide newParent)
  {
    base.SetParent(newParent);
    this._rows.SetParent(newParent);
    this._columns.SetParent(newParent);
  }

  private Dictionary<BuiltInTableStyle, string> CloneBuiltInStyle()
  {
    Dictionary<BuiltInTableStyle, string> dictionary = new Dictionary<BuiltInTableStyle, string>();
    foreach (KeyValuePair<BuiltInTableStyle, string> style in this._styleList)
      dictionary.Add(style.Key, style.Value);
    return dictionary;
  }
}
