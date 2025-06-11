// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Grid.PdfGridLayouter
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Grid;

internal class PdfGridLayouter : ElementLayouter
{
  private PdfGraphics m_currentGraphics;
  private PdfPage m_currentPage;
  private SizeF m_currentPageBounds;
  private RectangleF m_currentBounds;
  private List<int[]> m_columnRanges = new List<int[]>();
  private int m_cellStartIndex;
  private int m_cellEndIndex;
  private int m_currentRowIndex;
  private PointF m_startLocation;
  private float m_newheight;
  internal static int m_repeatRowIndex = -1;
  private bool isChanged;
  private PointF m_currentLocation = PointF.Empty;
  private PdfHorizontalOverflowType m_hType;
  private bool flag = true;
  private float m_childHeight;
  private List<int> m_parentCellIndexList;
  private bool isChildGrid;
  private int m_rowBreakPageHeightCellIndex;
  private bool m_isHeader;
  private bool isPaginate;
  private float userHeight;
  private RectangleF m_cellEventBounds = new RectangleF();
  private bool m_GridPaginated;
  private PdfStringLayoutResult slr;
  private string remainderText;

  internal PdfGrid Grid => this.Element as PdfGrid;

  internal PdfGridLayouter(PdfGrid grid)
    : base((PdfLayoutElement) grid)
  {
  }

  public void Layout(PdfGraphics graphics, PointF location)
  {
    RectangleF bounds = new RectangleF(location, SizeF.Empty);
    this.Layout(graphics, bounds);
  }

  public void Layout(PdfGraphics graphics, RectangleF bounds)
  {
    if (graphics == null)
      throw new ArgumentNullException(nameof (graphics));
    double width = (double) graphics.ClientSize.Width;
    double x = (double) bounds.X;
    PdfLayoutParams pdfLayoutParams = new PdfLayoutParams();
    pdfLayoutParams.Bounds = bounds;
    if (pdfLayoutParams.Format == null)
    {
      PdfGridLayoutFormat gridLayoutFormat = new PdfGridLayoutFormat();
      pdfLayoutParams.Format = (PdfLayoutFormat) gridLayoutFormat;
    }
    this.m_currentGraphics = graphics;
    if (graphics.Layer != null)
    {
      int num = !(this.m_currentGraphics.Page is PdfPage) ? this.m_currentGraphics.Page.DefaultLayerIndex : (this.m_currentGraphics.Page as PdfPage).Section.IndexOf(this.m_currentGraphics.Page as PdfPage);
      if (!this.Grid.m_listOfNavigatePages.Contains(num))
        this.Grid.m_listOfNavigatePages.Add(num);
    }
    this.LayoutInternal(pdfLayoutParams);
  }

  protected override PdfLayoutResult LayoutInternal(PdfLayoutParams param)
  {
    PdfGridLayoutFormat gridLayoutFormat = param != null ? this.GetFormat(param.Format) : throw new ArgumentNullException(nameof (param));
    this.m_currentPage = param.Page;
    this.m_currentPageBounds = this.m_currentPage == null ? this.m_currentGraphics.ClientSize : ((double) this.m_currentPage.GetClientSize().Height > (double) this.m_currentPage.GetClientSize().Width || param.Page.Orientation == PdfPageOrientation.Landscape && gridLayoutFormat.Break == PdfLayoutBreakType.FitPage ? this.m_currentPage.GetClientSize() : this.m_currentPage.Size);
    this.m_currentGraphics = this.m_currentPage != null ? this.m_currentPage.Graphics : this.m_currentGraphics;
    if (this.m_currentGraphics.Layer != null)
    {
      int num = !(this.m_currentGraphics.Page is PdfPage) ? this.m_currentGraphics.Page.DefaultLayerIndex : (this.m_currentGraphics.Page as PdfPage).Section.IndexOf(this.m_currentGraphics.Page as PdfPage);
      if (!this.Grid.m_listOfNavigatePages.Contains(num))
        this.Grid.m_listOfNavigatePages.Add(num);
    }
    this.m_startLocation = param.Bounds.Location;
    this.userHeight = this.m_startLocation.Y;
    if (!this.Grid.IsChildGrid)
      this.m_hType = this.Grid.Style.HorizontalOverflowType;
    return (PdfLayoutResult) this.LayoutOnPage(param);
  }

  private PdfGridLayoutResult LayoutOnPage(PdfLayoutParams param)
  {
    PdfGridLayoutFormat format = this.GetFormat(param.Format);
    PdfGridEndPageLayoutEventArgs pageLayoutEventArgs = (PdfGridEndPageLayoutEventArgs) null;
    PdfGridLayoutResult gridLayoutResult = (PdfGridLayoutResult) null;
    Dictionary<PdfPage, int[]> layoutedPages = new Dictionary<PdfPage, int[]>();
    PdfPage page = param.Page;
    bool flag1 = false;
    List<float> floatList1 = new List<float>();
    if (param.Page != null && param.Page.Document != null && param.Page.Document.AutoTag || this.Grid.PdfTag != null)
    {
      if (PdfCatalog.StructTreeRoot != null)
        PdfCatalog.StructTreeRoot.isNewTable = true;
      if (this.Grid.PdfTag == null)
      {
        this.Grid.PdfTag = (PdfTag) new PdfStructureElement(PdfTagType.Table);
        if (this.Grid.IsChildGrid)
        {
          (this.Grid.PdfTag as PdfStructureElement).Parent = this.Grid.ParentCell.PdfTag as PdfStructureElement;
          (this.Grid.ParentCell.PdfTag as PdfStructureElement).Parent = (PdfStructureElement) null;
        }
      }
    }
    RectangleF rectangleF1 = new RectangleF();
    if (this.Grid.IsChildGrid)
    {
      rectangleF1 = param.Bounds;
      this.m_cellEventBounds = rectangleF1;
      this.m_currentBounds = param.Bounds;
      this.m_childHeight = this.m_currentBounds.Height;
    }
    else
    {
      RectangleF bounds;
      if (format != null && format.Break == PdfLayoutBreakType.FitColumnsToPage)
      {
        ref RectangleF local = ref rectangleF1;
        bounds = param.Bounds;
        PointF location = bounds.Location;
        SizeF size = new SizeF(this.Grid.Columns.Width, this.m_currentGraphics.ClientSize.Height);
        local = new RectangleF(location, size);
      }
      else
      {
        ref RectangleF local = ref rectangleF1;
        bounds = param.Bounds;
        PointF location = bounds.Location;
        SizeF clientSize = this.m_currentGraphics.ClientSize;
        local = new RectangleF(location, clientSize);
      }
      ref RectangleF local1 = ref this.m_currentBounds;
      bounds = param.Bounds;
      PointF location1 = bounds.Location;
      local1.Location = location1;
      bounds = param.Bounds;
      if ((double) bounds.Width > 0.0)
      {
        bounds = param.Bounds;
        if ((double) bounds.Width > (double) this.m_currentPageBounds.Width)
        {
          PdfLayoutParams pdfLayoutParams = param;
          bounds = param.Bounds;
          double x = (double) bounds.X;
          bounds = param.Bounds;
          double y = (double) bounds.Y;
          double width = (double) this.m_currentPageBounds.Width;
          bounds = param.Bounds;
          double height = (double) bounds.Height;
          RectangleF rectangleF2 = new RectangleF((float) x, (float) y, (float) width, (float) height);
          pdfLayoutParams.Bounds = rectangleF2;
        }
        ref RectangleF local2 = ref this.m_currentBounds;
        bounds = param.Bounds;
        double width1 = (double) bounds.Width;
        local2.Width = (float) width1;
      }
      else
      {
        this.m_currentBounds.Width = rectangleF1.Width - rectangleF1.X;
        rectangleF1.Width = this.m_currentBounds.Width;
      }
      bounds = param.Bounds;
      if ((double) bounds.Height > 0.0)
      {
        ref RectangleF local3 = ref this.m_currentBounds;
        bounds = param.Bounds;
        double height = (double) bounds.Height;
        local3.Height = (float) height;
      }
      else
      {
        this.m_currentBounds.Height = rectangleF1.Height;
        rectangleF1.Height = this.m_currentBounds.Height;
      }
    }
    if (!this.Grid.Style.AllowHorizontalOverflow)
    {
      if (this.Grid.IsChildGrid)
        this.Grid.MeasureColumnsWidth(this.m_currentBounds);
      else
        this.Grid.MeasureColumnsWidth(new RectangleF(this.m_currentBounds.X, this.m_currentBounds.Y, this.m_currentBounds.Width + this.m_currentBounds.X, this.m_currentBounds.Height));
      this.m_columnRanges.Add(new int[2]
      {
        0,
        this.Grid.Columns.Count - 1
      });
    }
    else
    {
      this.Grid.MeasureColumnsWidth();
      this.DetermineColumnDrawRanges();
    }
    if (this.Grid.m_hasRowSpanSpan)
    {
      for (int index = 0; index < this.Grid.Rows.Count; ++index)
      {
        double height = (double) this.Grid.Rows[index].Height;
        if (!this.Grid.Rows[index].m_isRowHeightSet)
          this.Grid.Rows[index].m_isRowHeightSet = true;
        else
          this.Grid.Rows[index].m_isRowSpanRowHeightSet = true;
      }
    }
    foreach (int[] columnRange in this.m_columnRanges)
    {
      this.m_cellStartIndex = columnRange[0];
      this.m_cellEndIndex = columnRange[1];
      RectangleF currentBounds = rectangleF1;
      if (this.Grid.IsChildGrid)
        currentBounds = param.Bounds;
      if (this.RaiseBeforePageLayout(this.m_currentPage, ref currentBounds, ref this.m_currentRowIndex))
      {
        gridLayoutResult = new PdfGridLayoutResult(this.m_currentPage, this.m_currentBounds);
        break;
      }
      if (this.Grid.isBuiltinStyle && this.Grid.ParentCell == null && this.Grid.m_gridBuiltinStyle != PdfGridBuiltinStyle.TableGrid)
        this.Grid.ApplyBuiltinStyles(this.Grid.m_gridBuiltinStyle);
      foreach (PdfGridRow header in this.Grid.Headers)
      {
        float y = this.m_currentBounds.Y;
        if (param.Page != null && param.Page.Document != null && param.Page.Document.AutoTag || header.PdfTag != null)
        {
          PdfCatalog.StructTreeRoot.isNewRow = true;
          if (header.PdfTag == null)
            header.PdfTag = (PdfTag) new PdfStructureElement(PdfTagType.TableRow);
          (header.PdfTag as PdfStructureElement).Parent = header.Grid.PdfTag as PdfStructureElement;
        }
        this.m_isHeader = true;
        if (page != this.m_currentPage)
        {
          for (int cellStartIndex = this.m_cellStartIndex; cellStartIndex <= this.m_cellEndIndex; ++cellStartIndex)
          {
            if (header.Cells[cellStartIndex].IsCellMergeContinue)
            {
              header.Cells[cellStartIndex].IsCellMergeContinue = false;
              header.Cells[cellStartIndex].Value = (object) "";
            }
          }
        }
        PdfGridLayouter.RowLayoutResult rowLayoutResult = this.DrawRow(header);
        bool flag2;
        if ((double) y == (double) this.m_currentBounds.Y)
        {
          flag2 = true;
          PdfGridLayouter.m_repeatRowIndex = this.Grid.Rows.IndexOf(header);
        }
        else
          flag2 = false;
        if (!rowLayoutResult.IsFinish && page != null && format.Layout != PdfLayoutType.OnePage && flag2)
        {
          this.m_startLocation.X = this.m_currentBounds.X;
          this.m_currentPage = this.GetNextPage((PdfLayoutFormat) format);
          this.m_startLocation.Y = this.m_currentBounds.Y;
          if (format.PaginateBounds == RectangleF.Empty)
            this.m_currentBounds.X += this.m_startLocation.X;
          this.DrawRow(header);
        }
        this.m_isHeader = false;
      }
      int num1 = 0;
      int count1 = this.Grid.Rows.Count;
      float num2 = 0.0f;
      bool flag3 = true;
      if (flag1)
      {
        this.m_cellEndIndex = this.m_cellStartIndex = this.Grid.parentCellIndex;
        this.m_parentCellIndexList = new List<int>();
        this.m_parentCellIndexList.Add(this.Grid.parentCellIndex);
        this.Grid.ParentCell.present = true;
        PdfGrid grid = this.Grid.ParentCell.Row.Grid;
        while (grid.ParentCell != null)
        {
          this.m_parentCellIndexList.Add(grid.parentCellIndex);
          this.m_cellEndIndex = grid.parentCellIndex;
          this.m_cellStartIndex = grid.parentCellIndex;
          grid.ParentCell.present = true;
          grid = grid.ParentCell.Row.Grid;
          if (grid.ParentCell == null)
            this.m_parentCellIndexList.RemoveAt(this.m_parentCellIndexList.Count - 1);
        }
        int num3 = this.m_currentPage.Section.IndexOf(this.m_currentPage);
        if (!grid.isDrawn || !grid.m_listOfNavigatePages.Contains(num3))
        {
          (this.m_currentGraphics.Page as PdfPage).Section.IndexOf(this.m_currentPage);
          grid.isDrawn = true;
          foreach (PdfGridRow row in (List<PdfGridRow>) grid.Rows)
          {
            PdfGridRow pdfGridRow = row.CloneGridRow();
            PdfGridCell pdfGridCell = pdfGridRow.Cells[this.m_cellStartIndex].Clone(pdfGridRow.Cells[this.m_cellStartIndex]);
            pdfGridCell.Value = (object) "";
            PointF location = new PointF(this.m_currentBounds.X, this.m_currentBounds.Y);
            float width = grid.Columns[this.m_cellStartIndex].Width;
            if ((double) width > (double) this.m_currentGraphics.ClientSize.Width)
              width = this.m_currentGraphics.ClientSize.Width - 2f * location.X;
            float height = pdfGridCell.Height;
            if ((double) pdfGridRow.Height > (double) pdfGridCell.Height)
              height = pdfGridRow.Height;
            pdfGridCell.Draw(this.m_currentGraphics, new RectangleF(location, new SizeF(width, height)), false);
            this.m_currentBounds.Y += height;
          }
          this.m_currentBounds.Y = 0.0f;
        }
        this.DrawParentGridRow(grid);
        this.m_cellStartIndex = columnRange[0];
        this.m_cellEndIndex = columnRange[1];
      }
      floatList1.Clear();
      foreach (PdfGridRow row1 in (List<PdfGridRow>) this.Grid.Rows)
      {
        ++num1;
        this.m_currentRowIndex = num1 - 1;
        float y1 = this.m_currentBounds.Y;
        page = this.m_currentPage;
        PdfGridLayouter.m_repeatRowIndex = -1;
        if (param.Page != null && param.Page.Document != null && param.Page.Document.AutoTag || row1.PdfTag != null)
        {
          PdfCatalog.StructTreeRoot.isNewRow = true;
          if (row1.PdfTag == null)
            row1.PdfTag = (PdfTag) new PdfStructureElement(PdfTagType.TableRow);
          (row1.PdfTag as PdfStructureElement).Parent = row1.Grid.PdfTag as PdfStructureElement;
        }
        if (flag3 && row1.Grid.IsChildGrid)
        {
          num2 = y1;
          flag3 = false;
        }
        if (row1.Grid.IsChildGrid && row1.Grid.ParentCell.RowSpan > 1 && (int) ((double) num2 + (double) this.m_childHeight) < (int) ((double) this.m_currentBounds.Y + (double) row1.Height) && this.Grid.Rows.Count > num1)
        {
          foreach (PdfGridRow row2 in (List<PdfGridRow>) row1.Grid.ParentCell.Row.Grid.Rows)
          {
            if (row2.Cells[row1.Grid.parentCellIndex] == row1.Grid.ParentCell && row2.Cells[row1.Grid.parentCellIndex].Value is PdfGrid pdfGrid)
              pdfGrid.Rows.RemoveRange(0, num1 - 1);
          }
        }
        if (row1.Grid.LayoutFormat == null)
          row1.Grid.LayoutFormat = param.Format;
        PdfGridLayouter.RowLayoutResult rowLayoutResult = this.DrawRow(row1);
        List<float> floatList2 = floatList1;
        RectangleF rectangleF3 = rowLayoutResult.Bounds;
        double width2 = (double) rectangleF3.Width;
        floatList2.Add((float) width2);
        if (row1.isRowBreaksNextPage)
        {
          float x = 0.0f;
          for (int index = 0; index < row1.Cells.Count; ++index)
          {
            bool flag4 = false;
            if ((double) row1.Height == (double) row1.Cells[index].Height)
            {
              PdfGrid pdfGrid = row1.Cells[index].Value as PdfGrid;
              for (int count2 = pdfGrid.Rows.Count; 0 < count2; --count2)
              {
                if ((double) pdfGrid.Rows[count2 - 1].RowBreakHeight > 0.0)
                {
                  flag4 = true;
                  break;
                }
                if (pdfGrid.Rows[count2 - 1].isRowBreaksNextPage)
                {
                  row1.rowBreakHeight = pdfGrid.Rows[count2 - 1].rowBreakHeight;
                  break;
                }
                row1.rowBreakHeight += pdfGrid.Rows[count2 - 1].Height;
              }
            }
            if (flag4)
              break;
          }
          for (int index1 = 0; index1 < row1.Cells.Count; ++index1)
          {
            if ((double) row1.Height > (double) row1.Cells[index1].Height)
            {
              row1.Cells[index1].Value = (object) string.Empty;
              PdfPage nextPage = this.GetNextPage(this.m_currentPage);
              PdfSection section = this.m_currentPage.Section;
              int num4 = section.IndexOf(nextPage);
              RectangleF bounds;
              for (int index2 = 0; index2 < section.Count - 1 - num4; ++index2)
              {
                bounds = new RectangleF(x, 0.0f, row1.Grid.Columns[index1].Width, nextPage.GetClientSize().Height);
                PdfGridLayouter.m_repeatRowIndex = -1;
                row1.Cells[index1].Draw(nextPage.Graphics, bounds, false);
                nextPage = this.GetNextPage(nextPage);
              }
              bounds = new RectangleF(x, 0.0f, row1.Grid.Columns[index1].Width, row1.rowBreakHeight);
            }
            x += row1.Grid.Columns[index1].Width;
          }
        }
        bool flag5;
        if ((double) y1 == (double) this.m_currentBounds.Y)
        {
          flag5 = true;
          PdfGridLayouter.m_repeatRowIndex = this.Grid.Rows.IndexOf(row1);
        }
        else
        {
          flag5 = false;
          PdfGridLayouter.m_repeatRowIndex = -1;
        }
        while (!rowLayoutResult.IsFinish && page != null)
        {
          PdfGridLayoutResult result = this.GetLayoutResult();
          if (page != this.m_currentPage)
          {
            if (row1.Grid.IsChildGrid && row1.Grid.ParentCell != null)
            {
              RectangleF rectangleF4;
              ref RectangleF local4 = ref rectangleF4;
              rectangleF3 = format.PaginateBounds;
              PointF location = rectangleF3.Location;
              rectangleF3 = param.Bounds;
              double width3 = (double) rectangleF3.Width;
              rectangleF3 = result.Bounds;
              double height = (double) rectangleF3.Height;
              SizeF size = new SizeF((float) width3, (float) height);
              local4 = new RectangleF(location, size);
              ref RectangleF local5 = ref rectangleF4;
              double x1 = (double) local5.X;
              rectangleF3 = param.Bounds;
              double x2 = (double) rectangleF3.X;
              local5.X = (float) (x1 + x2);
              if (row1.Grid.ParentCell.Row.Grid.Style.CellPadding != null)
              {
                rectangleF4.Y += row1.Grid.ParentCell.Row.Grid.Style.CellPadding.Top;
                if ((double) rectangleF4.Height > (double) this.m_currentPageBounds.Height)
                {
                  rectangleF4.Height = this.m_currentPageBounds.Height - rectangleF4.Y;
                  rectangleF4.Height -= row1.Grid.ParentCell.Row.Grid.Style.CellPadding.Bottom;
                }
              }
              PdfGridCell cell;
              for (int index = 0; index < row1.Cells.Count; index = index + (cell.ColumnSpan - 1) + 1)
              {
                cell = row1.Cells[index];
                float num5 = 0.0f;
                if (cell.ColumnSpan > 1)
                {
                  for (; index < cell.ColumnSpan; ++index)
                    num5 += row1.Grid.Columns[index].Width;
                }
                else
                  num5 = Math.Max(cell.Width, row1.Grid.Columns[index].Width);
                rectangleF4.X += num5;
              }
            }
          }
          else if (!row1.Grid.IsChildGrid && !row1.Grid.AllowRowBreakAcrossPages)
          {
            rectangleF3 = result.Bounds;
            if ((double) rectangleF3.Width == (double) this.m_currentPageBounds.Width)
            {
              rectangleF3 = result.Bounds;
              if ((double) rectangleF3.X > 0.0)
              {
                RectangleF bounds = result.Bounds;
                float num6 = 0.0f;
                float num7 = 0.0f;
                if (row1.Cells[0].Style.Borders != null)
                {
                  if (row1.Cells[0].Style.Borders.Bottom != null)
                    num6 = row1.Cells[0].Style.Borders.Bottom.Width;
                  if (row1.Cells[0].Style.Borders.Right != null)
                  {
                    double width4 = (double) row1.Cells[0].Style.Borders.Bottom.Width;
                  }
                  if (row1.Cells[0].Style.Borders.Top != null)
                    num7 = row1.Cells[0].Style.Borders.Bottom.Width;
                  if (row1.Cells[0].Style.Borders.Bottom != null)
                  {
                    double width5 = (double) row1.Cells[0].Style.Borders.Bottom.Width;
                  }
                  if ((double) bounds.X != 0.0)
                    bounds.X -= num6;
                  if ((double) bounds.Y != 0.0)
                    bounds.Y -= num7;
                  result = new PdfGridLayoutResult(this.m_currentPage, bounds);
                }
              }
            }
          }
          pageLayoutEventArgs = this.RaisePageLayouted((PdfLayoutResult) result);
          if (!pageLayoutEventArgs.Cancel && !flag5)
          {
            if (this.Grid.AllowRowBreakAcrossPages)
            {
              if (pageLayoutEventArgs.NextPage != null)
              {
                this.m_currentPage = pageLayoutEventArgs.NextPage;
                this.m_currentGraphics = this.m_currentPage.Graphics;
                int num8 = (this.m_currentGraphics.Page as PdfPage).Section.IndexOf(this.m_currentGraphics.Page as PdfPage);
                if (!this.Grid.m_listOfNavigatePages.Contains(num8))
                  this.Grid.m_listOfNavigatePages.Add(num8);
                this.m_currentBounds = new RectangleF(PointF.Empty, this.m_currentPage.GetClientSize());
                if (format.PaginateBounds != RectangleF.Empty)
                {
                  ref RectangleF local6 = ref this.m_currentBounds;
                  rectangleF3 = format.PaginateBounds;
                  double x = (double) rectangleF3.X;
                  local6.X = (float) x;
                  ref RectangleF local7 = ref this.m_currentBounds;
                  rectangleF3 = format.PaginateBounds;
                  double y2 = (double) rectangleF3.Y;
                  local7.Y = (float) y2;
                  ref RectangleF local8 = ref this.m_currentBounds;
                  rectangleF3 = format.PaginateBounds;
                  double height = (double) rectangleF3.Size.Height;
                  local8.Height = (float) height;
                }
              }
              else
                this.m_currentPage = this.GetNextPage((PdfLayoutFormat) format);
              if (format != null && format.PaginateBounds != RectangleF.Empty)
              {
                this.m_currentBounds = format.PaginateBounds;
              }
              else
              {
                if (!this.Grid.IsChildGrid)
                  this.m_currentBounds.Location = new PointF(this.m_startLocation.X, 0.0f);
                else if (row1.RowIndex != -1)
                {
                  RectangleF layoutBounds = row1.Grid.Rows[row1.RowIndex > 0 ? row1.RowIndex - 1 : 0].Cells[0].layoutBounds;
                  float y3 = 0.0f;
                  float num9 = 0.0f;
                  for (PdfGridCell parentCell = row1.Grid.ParentCell; parentCell != null; parentCell = parentCell.Row.Grid.ParentCell)
                  {
                    if (parentCell.Style != null && parentCell.Style.Borders != null && parentCell.Style.Borders != null)
                    {
                      if (this.Grid.Style.CellPadding != null && (double) parentCell.Row.Grid.Style.CellPadding.Left <= 1.0)
                        layoutBounds.X -= parentCell.Row.Grid.Style.CellPadding.Left;
                      if (parentCell.Style.Borders.Top != null)
                        y3 += parentCell.Style.Borders.Top.Width;
                      if (this.Grid.Style.CellPadding != null && (double) parentCell.Row.Grid.Style.CellPadding.Top > 1.0 && (double) row1.RowBreakHeight <= (double) this.m_currentPageBounds.Height)
                        y3 += parentCell.Row.Grid.Style.CellPadding.Top;
                      if (parentCell.Style.Borders.Top != null && parentCell.Style.Borders.Bottom != null)
                        num9 += parentCell.Style.Borders.Top.Width + parentCell.Style.Borders.Bottom.Width;
                      num9 += parentCell.Row.Grid.Style.CellPadding.Bottom;
                      if (parentCell.Style.Borders.Left != null && parentCell.Style.Borders.Right != null)
                        layoutBounds.Width += (float) (((double) parentCell.Style.Borders.Left.Width + (double) parentCell.Style.Borders.Right.Width) / 2.0);
                    }
                  }
                  this.m_currentBounds = new RectangleF(layoutBounds.X, y3, layoutBounds.Width, this.m_currentBounds.Height - num9);
                }
                if (!this.Grid.IsChildGrid && (double) this.m_currentBounds.X != 0.0)
                  this.m_currentBounds.Width -= this.m_currentBounds.X;
                if (row1.Grid.ParentCell != null)
                {
                  row1.Grid.ParentCell.Row.isRowBreaksNextPage = true;
                  row1.Grid.ParentCell.Row.rowBreakHeight = row1.RowBreakHeight + this.Grid.ParentCell.Row.Grid.Style.CellPadding.Top + this.Grid.ParentCell.Row.Grid.Style.CellPadding.Bottom;
                }
              }
              if (this.Grid.IsChildGrid)
                this.m_cellEventBounds = this.m_currentBounds;
              if ((param.Page != null && param.Page.Document != null && param.Page.Document.AutoTag || row1.PdfTag != null) && param.Page != null && param.Page.Document != null && param.Page.Document.SeparateTable)
              {
                PdfCatalog.StructTreeRoot.isNewTable = true;
                PdfGridRow pdfGridRow = row1;
                bool flag6 = false;
                if (pdfGridRow.Grid.IsChildGrid)
                {
                  for (; pdfGridRow.Grid.IsChildGrid; pdfGridRow = pdfGridRow.Grid.ParentCell.Row)
                  {
                    PdfGrid grid1 = pdfGridRow.Grid;
                    PdfGrid grid2 = pdfGridRow.Grid.ParentCell.Row.Grid;
                    if (!pdfGridRow.Grid.ParentCell.Row.Grid.IsChildGrid && flag6)
                    {
                      PdfStructureElement structureElement = new PdfStructureElement(PdfTagType.Table);
                      (grid1.PdfTag as PdfStructureElement).Parent = new PdfStructureElement(PdfTagType.TableDataCell)
                      {
                        Parent = new PdfStructureElement(PdfTagType.TableRow)
                        {
                          Parent = structureElement
                        }
                      };
                      (grid2.PdfTag as PdfStructureElement).Parent = grid1.PdfTag as PdfStructureElement;
                    }
                    else
                    {
                      PdfStructureElement structureElement1 = new PdfStructureElement(PdfTagType.Table);
                      PdfStructureElement structureElement2 = new PdfStructureElement(PdfTagType.TableRow);
                      structureElement2.Parent = structureElement1;
                      PdfStructureElement structureElement3 = new PdfStructureElement(PdfTagType.TableDataCell);
                      structureElement3.Parent = structureElement2;
                      PdfStructureElement structureElement4 = new PdfStructureElement(PdfTagType.Table);
                      structureElement4.Parent = structureElement3;
                      PdfStructureElement structureElement5 = new PdfStructureElement(PdfTagType.TableRow);
                      structureElement5.Parent = structureElement4;
                      new PdfStructureElement(PdfTagType.TableDataCell).Parent = structureElement5;
                      pdfGridRow.PdfTag = (PdfTag) structureElement5;
                      grid1.PdfTag = (PdfTag) structureElement4;
                      grid2.PdfTag = (PdfTag) structureElement1;
                      flag6 = true;
                    }
                  }
                  PdfCatalog.StructTreeRoot.m_SplitTable = true;
                }
              }
              if (!this.RaiseBeforePageLayout(this.m_currentPage, ref currentBounds, ref this.m_currentRowIndex))
              {
                if (row1.m_noOfPageCount > 1)
                {
                  float rowBreakHeight = row1.RowBreakHeight;
                  for (int index = 1; index < row1.m_noOfPageCount; ++index)
                  {
                    row1.RowBreakHeight = 0.0f;
                    row1.Height = (float) (row1.m_noOfPageCount - 1) * this.m_currentPage.GetClientSize().Height;
                    this.DrawRow(row1);
                    this.m_currentPage = this.GetNextPage((PdfLayoutFormat) format);
                    page = this.m_currentPage;
                  }
                  row1.RowBreakHeight = rowBreakHeight;
                  row1.m_noOfPageCount = 1;
                  rowLayoutResult = this.DrawRow(row1);
                }
                else
                  rowLayoutResult = this.DrawRow(row1);
              }
              else
                break;
            }
            else
            {
              if (!this.Grid.AllowRowBreakAcrossPages && num1 < count1)
              {
                this.m_currentPage = this.GetNextPage((PdfLayoutFormat) format);
                break;
              }
              if (num1 >= count1)
                break;
            }
          }
          else
            break;
        }
        if (!rowLayoutResult.IsFinish && page != null && format.Layout != PdfLayoutType.OnePage && flag5)
        {
          this.m_startLocation.X = this.m_currentBounds.X;
          bool flag7 = false;
          if (!this.Grid.isSignleGrid)
          {
            for (int index3 = 0; index3 < this.Grid.Rows.Count; ++index3)
            {
              bool flag8 = false;
              for (int index4 = 0; index4 < this.Grid.Rows[index3].Cells.Count; ++index4)
              {
                if ((double) this.Grid.Rows[index3].Cells[index4].Width > (double) this.m_currentPageBounds.Width)
                  flag8 = true;
              }
              if (flag8 && this.Grid.Rows[index3].Cells[this.m_rowBreakPageHeightCellIndex].m_pageCount > 0)
                flag7 = true;
            }
          }
          if (!this.Grid.m_isRearranged && flag7)
          {
            this.m_currentPage = this.m_currentPage.Section.Add();
            this.m_currentGraphics = this.m_currentPage.Graphics;
            this.m_currentBounds = new RectangleF(PointF.Empty, this.m_currentPage.GetClientSize());
            int num10 = (this.m_currentGraphics.Page as PdfPage).Section.IndexOf(this.m_currentGraphics.Page as PdfPage);
            if (!this.Grid.m_listOfNavigatePages.Contains(num10))
              this.Grid.m_listOfNavigatePages.Add(num10);
          }
          else if (pageLayoutEventArgs.NextPage == null)
          {
            this.m_currentPage = this.GetNextPage((PdfLayoutFormat) format);
          }
          else
          {
            this.m_currentPage = pageLayoutEventArgs.NextPage;
            this.m_currentGraphics = pageLayoutEventArgs.NextPage.Graphics;
            this.m_currentBounds = new RectangleF(PointF.Empty, pageLayoutEventArgs.NextPage.GetClientSize());
          }
          PdfSection section1 = this.m_currentPage.Section;
          PdfSection section2 = param.Page.Section;
          if (format != null && format.PaginateBounds != RectangleF.Empty)
          {
            this.m_currentBounds = format.PaginateBounds;
          }
          else
          {
            if (!this.Grid.IsChildGrid)
              this.m_currentBounds.Location = new PointF(this.m_startLocation.X, 0.0f);
            else if (row1.RowIndex != -1)
            {
              RectangleF layoutBounds = row1.Grid.Rows[row1.RowIndex > 0 ? row1.RowIndex - 1 : 0].Cells[0].layoutBounds;
              float y4 = 0.0f;
              float num11 = 0.0f;
              PdfGridCell parentCell = row1.Grid.ParentCell;
              PdfGridRow pdfGridRow = (PdfGridRow) null;
              for (; parentCell != null; parentCell = parentCell.Row.Grid.ParentCell)
              {
                if (parentCell.Style != null && parentCell.Style.Borders != null && parentCell.Style.Borders != null)
                {
                  if (this.Grid.Style.CellPadding != null)
                    layoutBounds.X -= this.Grid.Style.CellPadding.Left;
                  if (parentCell.Style.Borders.Top != null)
                    y4 += parentCell.Style.Borders.Top.Width / 2f + this.Grid.Style.CellPadding.Top;
                  if (parentCell.Style.Borders.Top != null && parentCell.Style.Borders.Bottom != null)
                    num11 += parentCell.Style.Borders.Top.Width + parentCell.Style.Borders.Bottom.Width;
                  if (parentCell.Style.Borders.Left != null && parentCell.Style.Borders.Right != null)
                    layoutBounds.Width += (float) (((double) parentCell.Style.Borders.Left.Width + (double) parentCell.Style.Borders.Right.Width) / 2.0);
                }
                pdfGridRow = parentCell.Row;
              }
              this.m_currentBounds = new RectangleF(layoutBounds.X, y4, layoutBounds.Width, this.m_currentBounds.Height - num11);
              if ((double) this.m_currentBounds.X != (double) this.m_startLocation.X)
                this.m_currentBounds.X = this.m_startLocation.X;
              if (row1.Cells[0].Value is PdfGrid)
              {
                this.m_currentBounds.Y = 0.0f;
                if (pdfGridRow != null)
                  pdfGridRow.m_paginatedGridRow = true;
              }
            }
            if (!this.Grid.IsChildGrid && (double) this.m_currentBounds.X != 0.0)
              this.m_currentBounds.Width -= this.m_currentBounds.X;
          }
          if (!this.RaiseBeforePageLayout(this.m_currentPage, ref this.m_currentBounds, ref this.m_currentRowIndex))
          {
            this.m_startLocation.Y = this.m_currentBounds.Y;
            if (this.Grid.IsChildGrid)
              this.m_cellEventBounds = this.m_currentBounds;
            if ((param.Page != null && param.Page.Document != null && param.Page.Document.AutoTag || row1.PdfTag != null) && param.Page != null && param.Page.Document != null && param.Page.Document.SeparateTable)
            {
              PdfCatalog.StructTreeRoot.isNewTable = true;
              PdfGridRow pdfGridRow = row1;
              if (pdfGridRow.Grid.IsChildGrid)
              {
                bool flag9 = false;
                for (; pdfGridRow.Grid.IsChildGrid; pdfGridRow = pdfGridRow.Grid.ParentCell.Row)
                {
                  PdfGrid grid3 = pdfGridRow.Grid;
                  PdfGrid grid4 = pdfGridRow.Grid.ParentCell.Row.Grid;
                  if (!pdfGridRow.Grid.ParentCell.Row.Grid.IsChildGrid && flag9)
                  {
                    PdfStructureElement structureElement = new PdfStructureElement(PdfTagType.Table);
                    (grid3.PdfTag as PdfStructureElement).Parent = new PdfStructureElement(PdfTagType.TableDataCell)
                    {
                      Parent = new PdfStructureElement(PdfTagType.TableRow)
                      {
                        Parent = structureElement
                      }
                    };
                    (grid4.PdfTag as PdfStructureElement).Parent = grid3.PdfTag as PdfStructureElement;
                  }
                  else
                  {
                    PdfStructureElement structureElement6 = new PdfStructureElement(PdfTagType.Table);
                    PdfStructureElement structureElement7 = new PdfStructureElement(PdfTagType.TableRow);
                    structureElement7.Parent = structureElement6;
                    PdfStructureElement structureElement8 = new PdfStructureElement(PdfTagType.TableDataCell);
                    structureElement8.Parent = structureElement7;
                    PdfStructureElement structureElement9 = new PdfStructureElement(PdfTagType.Table);
                    structureElement9.Parent = structureElement8;
                    PdfStructureElement structureElement10 = new PdfStructureElement(PdfTagType.TableRow);
                    structureElement10.Parent = structureElement9;
                    new PdfStructureElement(PdfTagType.TableDataCell).Parent = structureElement10;
                    pdfGridRow.PdfTag = (PdfTag) structureElement10;
                    grid3.PdfTag = (PdfTag) structureElement9;
                    grid4.PdfTag = (PdfTag) structureElement6;
                    flag9 = true;
                  }
                }
                PdfCatalog.StructTreeRoot.m_SplitTable = true;
              }
            }
            if (this.Grid.RepeatHeader)
            {
              this.m_isHeader = true;
              foreach (PdfGridRow header in this.Grid.Headers)
              {
                if (param.Page != null && param.Page.Document != null && param.Page.Document.AutoTag || row1.PdfTag != null)
                {
                  PdfCatalog.StructTreeRoot.isNewRow = true;
                  if (row1.PdfTag == null)
                    row1.PdfTag = (PdfTag) new PdfStructureElement(PdfTagType.TableRow);
                  (row1.PdfTag as PdfStructureElement).Parent = row1.Grid.PdfTag as PdfStructureElement;
                }
                this.DrawRow(header);
              }
              if (param.Page != null && param.Page.Document != null && param.Page.Document.AutoTag || row1.PdfTag != null)
              {
                PdfCatalog.StructTreeRoot.isNewRow = true;
                if (row1.PdfTag == null)
                  row1.PdfTag = (PdfTag) new PdfStructureElement(PdfTagType.TableRow);
                (row1.PdfTag as PdfStructureElement).Parent = row1.Grid.PdfTag as PdfStructureElement;
              }
              this.m_isHeader = false;
            }
            this.DrawRow(row1);
            if (this.m_currentPage != null && !layoutedPages.ContainsKey(this.m_currentPage))
              layoutedPages.Add(this.m_currentPage, columnRange);
          }
          else
            break;
        }
        if (row1.NestedGridLayoutResult != null)
        {
          this.m_currentPage = row1.NestedGridLayoutResult.Page;
          this.m_currentGraphics = this.m_currentPage.Graphics;
          rectangleF3 = row1.NestedGridLayoutResult.Bounds;
          this.m_startLocation = rectangleF3.Location;
          ref RectangleF local9 = ref this.m_currentBounds;
          rectangleF3 = row1.NestedGridLayoutResult.Bounds;
          double bottom = (double) rectangleF3.Bottom;
          local9.Y = (float) bottom;
          if (page != this.m_currentPage)
          {
            PdfSection section = this.m_currentPage.Section;
            int num12 = section.IndexOf(page) + 1;
            int num13 = section.IndexOf(this.m_currentPage);
            for (int index5 = num12; index5 < num13 + 1; ++index5)
            {
              PdfGraphics graphics = section[index5].Graphics;
              rectangleF3 = format.PaginateBounds;
              PointF location = rectangleF3.Location;
              if (location == PointF.Empty && (double) this.m_currentBounds.X > (double) location.X && !row1.Grid.IsChildGrid && row1.Grid.ParentCell == null)
                location.X = this.m_currentBounds.X;
              double num14;
              if (index5 != num13)
              {
                num14 = (double) this.m_currentBounds.Height - (double) location.Y;
              }
              else
              {
                rectangleF3 = row1.NestedGridLayoutResult.Bounds;
                double height = (double) rectangleF3.Height;
                rectangleF3 = param.Bounds;
                double y5 = (double) rectangleF3.Y;
                num14 = height - y5;
              }
              float height1 = (float) num14;
              if ((double) height1 <= (double) graphics.ClientSize.Height)
              {
                double num15 = (double) height1;
                rectangleF3 = param.Bounds;
                double y6 = (double) rectangleF3.Y;
                height1 = (float) (num15 + y6);
              }
              if (row1.Grid.IsChildGrid && row1.Grid.ParentCell != null)
              {
                ref PointF local10 = ref location;
                double x3 = (double) local10.X;
                rectangleF3 = param.Bounds;
                double x4 = (double) rectangleF3.X;
                local10.X = (float) (x3 + x4);
              }
              ref PointF local11 = ref location;
              double num16;
              if (format != null)
              {
                rectangleF3 = format.PaginateBounds;
                num16 = (double) rectangleF3.Location.Y;
              }
              else
                num16 = 0.0;
              local11.Y = (float) num16;
              float num17 = 0.0f;
              float num18 = 0.0f;
              for (int index6 = 0; index6 < row1.Cells.Count; ++index6)
              {
                PdfGridCell cell = row1.Cells[index6];
                PdfGridCell parentCell;
                for (parentCell = row1.Grid.ParentCell; parentCell != null && cell.Value is PdfGrid; parentCell = parentCell.Row.Grid.ParentCell)
                {
                  if (parentCell.Style != null && parentCell.Style.Borders != null && parentCell.Style.Borders.Top != null)
                    num18 += parentCell.Style.Borders.Top.Width;
                }
                if (parentCell == null && cell.Value is PdfGrid && cell.Style != null && cell.Style.Borders != null && cell.Style.Borders.Top != null)
                  num17 += cell.Style.Borders.Top.Width;
              }
              if ((double) num18 != 0.0)
              {
                float num19 = height1 + num18;
                if ((double) graphics.m_cellBorderMaxHeight < (double) num19)
                  graphics.m_cellBorderMaxHeight = num19;
              }
              else if ((double) graphics.m_cellBorderMaxHeight > 0.0)
              {
                if ((double) graphics.m_cellBorderMaxHeight > (double) this.m_currentPageBounds.Height)
                {
                  this.m_currentBounds.Y = this.m_currentPageBounds.Height - row1.Cells[0].Style.Borders.Top.Width / 2f;
                  height1 = this.m_currentPageBounds.Height - row1.Cells[0].Style.Borders.Top.Width / 2f;
                }
                else
                {
                  height1 = graphics.m_cellBorderMaxHeight;
                  if ((double) this.m_currentBounds.Y + (double) row1.Cells[0].Style.Borders.Top.Width < (double) this.m_currentPageBounds.Height)
                    height1 += num17 * 2f;
                  if (row1.Cells.Count > 0 && row1.Cells[0].Style != null && row1.Cells[0].Style.Borders != null && row1.Cells[0].Style.Borders.Top != null)
                    height1 += row1.Cells[0].Style.Borders.Top.Width;
                  this.m_currentBounds.Y = row1.Cells.Count <= 0 || row1.Cells[0].Style == null || row1.Cells[0].Style.Borders == null || row1.Cells[0].Style.Borders.Top == null ? height1 : height1 + row1.Cells[0].Style.Borders.Top.Width;
                }
              }
              else if ((double) height1 < (double) this.m_currentPageBounds.Height)
              {
                double num20;
                if (format != null)
                {
                  rectangleF3 = format.PaginateBounds;
                  num20 = (double) rectangleF3.Location.Y;
                }
                else
                  num20 = 0.0;
                float num21 = (float) num20;
                if ((double) num21 == 0.0 && row1.Grid.isSignleGrid)
                {
                  if (row1.Cells.Count > 0 && row1.Cells[0].Style != null && row1.Cells[0].Style.Borders != null && row1.Cells[0].Style.Borders.Top != null)
                    height1 += row1.Cells[0].Style.Borders.Top.Width;
                  this.m_currentBounds.Y = row1.Cells.Count <= 0 || row1.Cells[0].Style == null || row1.Cells[0].Style.Borders == null || row1.Cells[0].Style.Borders.Top == null ? height1 : height1 + row1.Cells[0].Style.Borders.Top.Width;
                }
                else if ((double) num21 == 0.0 && !row1.Grid.isSignleGrid)
                {
                  if ((double) row1.rowBreakHeight == 0.0 && row1.Cells.Count > 0 && row1.Cells[0].Style != null && row1.Cells[0].Style.Borders != null && row1.Cells[0].Style.Borders.Top != null)
                    height1 += row1.Cells[0].Style.Borders.Top.Width;
                  if (!row1.m_paginatedGridRow)
                  {
                    rectangleF3 = param.Bounds;
                    if ((double) rectangleF3.X > 0.0)
                    {
                      rectangleF3 = param.Bounds;
                      if ((double) rectangleF3.Y == 0.0)
                        goto label_329;
                    }
                    if (row1.Cells.Count > 0 && row1.Cells[0].Style != null && row1.Cells[0].Style.Borders != null && row1.Cells[0].Style.Borders.Top != null)
                    {
                      this.m_currentBounds.Y = height1 + row1.Cells[0].Style.Borders.Top.Width;
                      goto label_330;
                    }
                  }
label_329:
                  this.m_currentBounds.Y = height1;
                }
              }
label_330:
              PdfGridCell cell1;
              for (int index7 = 0; index7 < row1.Cells.Count; index7 = index7 + (cell1.ColumnSpan - 1) + 1)
              {
                cell1 = row1.Cells[index7];
                float width6 = 0.0f;
                if (cell1.ColumnSpan > 1)
                {
                  for (; index7 < cell1.ColumnSpan; ++index7)
                    width6 += row1.Grid.Columns[index7].Width;
                }
                else
                  width6 = this.Grid.isWidthSet ? (this.Grid.LastRow == null || (double) this.Grid.LastRow.Width != (double) row1.Grid.Columns[index7].Width ? Math.Min(cell1.Width, row1.Grid.Columns[index7].Width) : this.Grid.LastRow.Width) : Math.Max(cell1.Width, row1.Grid.Columns[index7].Width);
                float num22 = 0.0f;
                float num23 = 0.0f;
                float num24 = 0.0f;
                int index8 = index7;
                if (cell1.ColumnSpan > 1)
                  index8 = cell1.ColumnSpan - 1;
                if (row1.Cells[index8].Style.Borders != null)
                {
                  if (row1.Cells[index8].Style.Borders.Left != null)
                    num22 = row1.Cells[index8].Style.Borders.Left.Width;
                  if (row1.Cells[index8].Style.Borders.Right != null)
                    num23 = row1.Cells[index8].Style.Borders.Right.Width;
                  if (row1.Cells[index8].Style.Borders.Top != null)
                    num24 = row1.Cells[index8].Style.Borders.Top.Width;
                  if (row1.Cells[index8].Style.Borders.Bottom != null)
                  {
                    double width7 = (double) row1.Cells[index8].Style.Borders.Bottom.Width;
                  }
                }
                if ((double) location.X == 0.0)
                {
                  location.X += num22 / 2f;
                  width6 -= num22 / 2f;
                }
                if ((double) location.Y == 0.0)
                {
                  if ((double) cell1.layoutBounds.Y != 0.0 && row1.NestedGridLayoutResult != null && cell1.Value is PdfGrid)
                  {
                    location.Y = num18 + num24 / 2f;
                    if ((double) num18 != 0.0)
                      location.X += num22 / 2f;
                    if ((double) graphics.m_cellBorderMaxHeight > 0.0)
                      width6 -= (float) (((double) num22 + (double) num23) / 2.0);
                    float cellBorderMaxHeight = graphics.m_cellBorderMaxHeight;
                    if ((double) graphics.m_cellBorderMaxHeight > 0.0)
                      height1 = (double) cellBorderMaxHeight != (double) height1 + (double) num18 ? (float) ((double) cellBorderMaxHeight + (double) num18 + (double) num24 / 2.0) : cellBorderMaxHeight - num24 / 2f;
                    else
                      height1 += num24 / 2f;
                  }
                  else
                  {
                    location.Y = num18 + num24 / 2f;
                    if ((double) num18 != 0.0)
                    {
                      float cellBorderMaxHeight = graphics.m_cellBorderMaxHeight;
                      height1 = (double) cellBorderMaxHeight != (double) height1 + (double) num18 ? (float) ((double) cellBorderMaxHeight + (double) num18 + (double) num24 / 2.0) : cellBorderMaxHeight - num24 / 2f;
                    }
                    else
                      height1 += num24 / 2f;
                  }
                }
                cell1.DrawCellBorders(ref graphics, new RectangleF(location, new SizeF(width6, height1)));
                location.X += width6;
              }
            }
            page = this.m_currentPage;
          }
        }
        else if (page != this.m_currentPage && !rowLayoutResult.IsFinish && !this.Grid.AllowRowBreakAcrossPages)
        {
          PointF pointF = new PointF(PdfBorders.Default.Right.Width / 2f, PdfBorders.Default.Top.Width / 2f);
          if (format.PaginateBounds == RectangleF.Empty && this.m_startLocation == pointF)
          {
            this.m_currentBounds.X += this.m_startLocation.X;
            this.m_currentBounds.Y += this.m_startLocation.Y;
          }
        }
      }
      bool flag10 = false;
      float num25 = 0.0f;
      if (floatList1.Count > 0)
        num25 = floatList1[0];
      float[,] numArray = new float[1, 2];
      for (int index = 0; index < this.Grid.Rows.Count; ++index)
      {
        if (this.m_cellEndIndex != -1 && this.Grid.Rows[index].Cells[this.m_cellEndIndex].Value is PdfGrid)
        {
          PdfGrid pdfGrid = this.Grid.Rows[index].Cells[this.m_cellEndIndex].Value as PdfGrid;
          this.Grid.m_rowLayoutBoundswidth = pdfGrid.m_rowLayoutBoundswidth;
          flag10 = true;
          if ((double) numArray[0, 0] < (double) pdfGrid.m_listOfNavigatePages.Count)
          {
            numArray[0, 0] = (float) pdfGrid.m_listOfNavigatePages.Count;
            numArray[0, 1] = floatList1[index];
          }
          else if ((double) numArray[0, 0] == (double) pdfGrid.m_listOfNavigatePages.Count && (double) numArray[0, 1] < (double) floatList1[index])
            numArray[0, 1] = floatList1[index];
        }
      }
      if (!flag10 && floatList1.Count > 0)
      {
        for (int index = 0; index < num1 - 1; ++index)
        {
          if ((double) num25 < (double) floatList1[index])
            num25 = floatList1[index];
        }
        this.Grid.m_rowLayoutBoundswidth = num25;
      }
      else
        this.Grid.m_rowLayoutBoundswidth = numArray[0, 1];
      if (this.m_columnRanges.IndexOf(columnRange) < this.m_columnRanges.Count - 1 && page != null && format.Layout != PdfLayoutType.OnePage)
      {
        flag1 = this.Grid.IsChildGrid;
        if ((int) numArray[0, 0] != 0)
        {
          PdfSection section = this.m_currentPage.Section;
          int num26 = section.IndexOf(this.m_currentPage);
          this.m_currentPage = section.Count <= num26 + (int) numArray[0, 0] ? section.Add() : section[num26 + (int) numArray[0, 0]];
          this.m_currentGraphics = this.m_currentPage.Graphics;
          this.m_currentBounds = new RectangleF(PointF.Empty, this.m_currentPage.GetClientSize());
          int num27 = (this.m_currentGraphics.Page as PdfPage).Section.IndexOf(this.m_currentGraphics.Page as PdfPage);
          if (!this.Grid.m_listOfNavigatePages.Contains(num27))
            this.Grid.m_listOfNavigatePages.Add(num27);
        }
        else
          this.m_currentPage = this.GetNextPage((PdfLayoutFormat) format);
        PointF pointF = new PointF(PdfBorders.Default.Right.Width / 2f, PdfBorders.Default.Top.Width / 2f);
        if (format.PaginateBounds == RectangleF.Empty && this.m_startLocation == pointF)
        {
          this.m_currentBounds.X += this.m_startLocation.X;
          this.m_currentBounds.Y += this.m_startLocation.Y;
        }
      }
    }
    if (!this.Grid.IsChildGrid)
    {
      foreach (PdfGridRow row in (List<PdfGridRow>) this.Grid.Rows)
        row.RowBreakHeight = 0.0f;
    }
    PdfGridLayoutResult layoutResult = this.GetLayoutResult();
    if (this.Grid.Style.AllowHorizontalOverflow && this.Grid.Style.HorizontalOverflowType == PdfHorizontalOverflowType.NextPage)
      this.ReArrangePages(layoutedPages);
    if (PdfCatalog.StructTreeRoot != null && PdfCatalog.StructTreeRoot.m_isChildGrid)
    {
      PdfCatalog.StructTreeRoot.m_isChildGrid = false;
      PdfCatalog.StructTreeRoot.m_isNestedGridRendered = true;
    }
    if (!this.Grid.IsChildGrid)
    {
      PdfGridLayoutResult result = layoutResult;
      RectangleF bounds = layoutResult.Bounds;
      PdfGridRow pdfGridRow = (PdfGridRow) null;
      if (this.Grid.Rows.Count > 0)
        pdfGridRow = this.Grid.Rows[0];
      if (pdfGridRow != null && pdfGridRow.NestedGridLayoutResult != null)
      {
        float num28 = 0.0f;
        float num29 = 0.0f;
        if (pdfGridRow.Cells[0].Style.Borders != null)
        {
          if (pdfGridRow.Cells[0].Style.Borders.Bottom != null)
            num28 = pdfGridRow.Cells[0].Style.Borders.Bottom.Width;
          if (pdfGridRow.Cells[0].Style.Borders.Right != null)
          {
            double width8 = (double) pdfGridRow.Cells[0].Style.Borders.Bottom.Width;
          }
          if (pdfGridRow.Cells[0].Style.Borders.Top != null)
            num29 = pdfGridRow.Cells[0].Style.Borders.Bottom.Width;
          if (pdfGridRow.Cells[0].Style.Borders.Bottom != null)
          {
            double width9 = (double) pdfGridRow.Cells[0].Style.Borders.Bottom.Width;
          }
          if ((double) bounds.X != 0.0)
            bounds.X -= num28;
          if ((double) bounds.Y != 0.0)
            bounds.Y -= num29;
          result = new PdfGridLayoutResult(this.m_currentPage, bounds);
        }
      }
      this.RaisePageLayouted((PdfLayoutResult) result);
    }
    else
      this.RaisePageLayouted((PdfLayoutResult) layoutResult);
    return layoutResult;
  }

  private bool DrawParentGridRow(PdfGrid grid)
  {
    bool flag = false;
    grid.isDrawn = true;
    float y = this.m_currentBounds.Y;
    foreach (PdfGridRow row in (List<PdfGridRow>) grid.Rows)
    {
      PdfGridRow pdfGridRow = row.CloneGridRow();
      PdfGridCell pdfGridCell = pdfGridRow.Cells[this.m_cellStartIndex].Clone(pdfGridRow.Cells[this.m_cellStartIndex]);
      pdfGridCell.Value = (object) "";
      PointF location = new PointF(this.m_currentBounds.X, this.m_currentBounds.Y);
      float width = grid.Columns[this.m_cellStartIndex].Width;
      if ((double) width > (double) this.m_currentGraphics.ClientSize.Width)
        width = this.m_currentGraphics.ClientSize.Width - 2f * location.X;
      float height = pdfGridCell.Height;
      if ((double) pdfGridRow.Height > (double) pdfGridCell.Height)
        height = pdfGridRow.Height;
      if (this.isChildGrid)
        pdfGridCell.Draw(this.m_currentGraphics, new RectangleF(location, new SizeF(width, height)), false);
      this.m_currentBounds.Y += height;
    }
    for (int index1 = 0; index1 < grid.Rows.Count; ++index1)
    {
      if (grid.Rows[index1].Cells[this.m_cellStartIndex].present)
      {
        flag = true;
        if (grid.Rows[index1].Cells[this.m_cellStartIndex].Value is PdfGrid)
        {
          PdfGrid grid1 = grid.Rows[index1].Cells[this.m_cellStartIndex].Value as PdfGrid;
          grid.Rows[index1].Cells[this.m_cellStartIndex].present = false;
          if (grid1 == this.Grid)
          {
            if (!this.isChildGrid)
              this.m_currentBounds.Y = y;
            else if (index1 == 0)
            {
              this.m_currentBounds.Y -= grid.Size.Height;
            }
            else
            {
              for (int index2 = index1; index2 < grid.Rows.Count; ++index2)
                this.m_currentBounds.Y -= grid.Rows[index2].Height;
            }
            PdfGrid pdfGrid = grid1.Clone(grid1);
            pdfGrid.isDrawn = true;
            grid.Rows[index1].Cells[this.m_cellStartIndex].Value = (object) pdfGrid;
            this.m_currentBounds.X += grid.Style.CellPadding.Left + grid.Style.CellPadding.Right;
            this.m_currentBounds.Y += grid.Style.CellPadding.Top + grid.Style.CellPadding.Bottom;
            this.m_currentBounds.Width -= 2f * this.m_currentBounds.X;
            break;
          }
          this.isChildGrid = true;
          if (this.m_parentCellIndexList.Count > 0)
          {
            this.m_cellStartIndex = this.m_parentCellIndexList[this.m_parentCellIndexList.Count - 1];
            this.m_parentCellIndexList.RemoveAt(this.m_parentCellIndexList.Count - 1);
          }
          this.m_currentBounds.Y = y;
          this.m_currentBounds.X += grid.Style.CellPadding.Left + grid.Style.CellPadding.Right;
          this.m_currentBounds.Y += grid.Style.CellPadding.Top + grid.Style.CellPadding.Bottom;
          if (!this.DrawParentGridRow(grid1))
            this.m_currentBounds.Y -= grid1.Size.Height;
          this.isChildGrid = false;
          break;
        }
        y += grid.Rows[index1].Height;
      }
      else
        y += grid.Rows[index1].Height;
    }
    return flag;
  }

  private void ReArrangePages(Dictionary<PdfPage, int[]> layoutedPages)
  {
    PdfDocument document = this.m_currentPage.Document;
    List<PdfPage> pdfPageList = new List<PdfPage>();
    foreach (PdfPage key in layoutedPages.Keys)
    {
      key.Section = (PdfSection) null;
      pdfPageList.Add(key);
      document.Pages.Remove(key);
    }
    for (int index1 = 0; index1 < layoutedPages.Count; ++index1)
    {
      int index2 = index1;
      int num = layoutedPages.Count / this.m_columnRanges.Count;
      for (; index2 < layoutedPages.Count; index2 += num)
      {
        PdfPage page = pdfPageList[index2];
        if (document.Pages.IndexOf(page) == -1)
          document.Pages.Add(page);
      }
    }
  }

  private PdfGridLayouter.RowLayoutResult DrawRow(PdfGridRow row)
  {
    PdfGridLayouter.RowLayoutResult result = new PdfGridLayouter.RowLayoutResult();
    float num1 = 0.0f;
    float num2 = 0.0f;
    PointF empty1 = PointF.Empty;
    SizeF empty2 = SizeF.Empty;
    bool flag1 = false;
    bool flag2 = true;
    if (row.RowSpanExists)
    {
      int index1 = this.Grid.Rows.IndexOf(row);
      int maximumRowSpan = row.maximumRowSpan;
      if (index1 == -1)
      {
        index1 = this.Grid.Headers.IndexOf(row);
        if (index1 != -1)
          flag1 = true;
      }
      int num3 = 0;
      for (int index2 = index1; index2 < index1 + maximumRowSpan; ++index2)
      {
        num1 += flag1 ? this.Grid.Headers[index2].Height : this.Grid.Rows[index2].Height;
        if (num3 < 2)
          num2 = num1;
        ++num3;
      }
      if (((double) num1 > (double) this.m_currentBounds.Height || (double) num1 + (double) this.m_currentBounds.Y > (double) this.m_currentBounds.Height) && row.RowSpanExists && (this.Grid.LayoutFormat.Break == PdfLayoutBreakType.FitElement || !this.Grid.AllowRowBreakAcrossPages))
        flag2 = false;
      if (((double) num1 > (double) this.m_currentBounds.Height || (double) num1 + (double) this.m_currentBounds.Y > (double) this.m_currentBounds.Height) && !row.isPageBreakRowSpanApplied && flag2)
      {
        num1 = 0.0f;
        row.isPageBreakRowSpanApplied = true;
        foreach (PdfGridCell cell in row.Cells)
        {
          int rowSpan = cell.RowSpan;
          for (int index3 = index1; index3 < index1 + rowSpan; ++index3)
          {
            num1 += flag1 ? this.Grid.Headers[index3].Height : this.Grid.Rows[index3].Height;
            if ((double) this.m_currentBounds.Y + (double) num1 > (double) this.m_currentPageBounds.Height)
            {
              num1 -= flag1 ? this.Grid.Headers[index3].Height : this.Grid.Rows[index3].Height;
              for (int index4 = 0; index4 < this.Grid.Rows[index1].Cells.Count; ++index4)
              {
                int num4 = index3 - index1;
                if (!flag1 && this.Grid.Rows[index1].Cells[index4].RowSpan == rowSpan)
                {
                  this.Grid.Rows[index1].Cells[index4].RowSpan = num4 == 0 ? 1 : num4;
                  this.Grid.Rows[index1].maximumRowSpan = num4 == 0 ? 1 : num4;
                  this.Grid.Rows[index3].Cells[index4].RowSpan = rowSpan - num4;
                  PdfGrid pdfGrid = this.Grid.Rows[index1].Cells[index4].Value as PdfGrid;
                  this.Grid.Rows[index3].Cells[index4].StringFormat = this.Grid.Rows[index1].Cells[index4].StringFormat;
                  this.Grid.Rows[index3].Cells[index4].Style = (PdfGridCellStyle) this.Grid.Rows[index1].Cells[index4].Style.Clone();
                  this.Grid.Rows[index3].Cells[index4].Style.BackgroundImage = (PdfImage) null;
                  this.Grid.Rows[index3].Cells[index4].ColumnSpan = this.Grid.Rows[index1].Cells[index4].ColumnSpan;
                  if (pdfGrid != null && (double) this.m_currentBounds.Y + (double) pdfGrid.Size.Height + (double) this.Grid.Rows[index3].Height + (double) pdfGrid.Style.CellPadding.Top + (double) pdfGrid.Style.CellPadding.Bottom >= (double) this.m_currentBounds.Height)
                    this.Grid.Rows[index3].Cells[index4].Value = this.Grid.Rows[index1].Cells[index4].Value;
                  else if (pdfGrid == null)
                    this.Grid.Rows[index3].Cells[index4].Value = this.Grid.Rows[index1].Cells[index4].Value;
                  if (index3 > 0)
                    this.Grid.Rows[index3 - 1].RowSpanExists = true;
                  this.Grid.Rows[index3].Cells[index4].IsRowMergeContinue = false;
                  this.Grid.Rows[index3].Cells[index4].IsRowMergeStart = true;
                }
                else if (flag1 && this.Grid.Headers[index1].Cells[index4].RowSpan == rowSpan)
                {
                  this.Grid.Headers[index1].Cells[index4].RowSpan = num4 == 0 ? 1 : num4;
                  this.Grid.Headers[index3].Cells[index4].RowSpan = rowSpan - num4;
                  this.Grid.Headers[index3].Cells[index4].StringFormat = this.Grid.Headers[index1].Cells[index4].StringFormat;
                  this.Grid.Headers[index3].Cells[index4].Style = this.Grid.Headers[index1].Cells[index4].Style;
                  this.Grid.Headers[index3].Cells[index4].ColumnSpan = this.Grid.Headers[index1].Cells[index4].ColumnSpan;
                  this.Grid.Headers[index3].Cells[index4].Value = this.Grid.Headers[index1].Cells[index4].Value;
                  this.Grid.Headers[index3 - 1].RowSpanExists = false;
                  this.Grid.Headers[index3].Cells[index4].IsRowMergeContinue = false;
                  this.Grid.Headers[index3].Cells[index4].IsRowMergeStart = true;
                }
              }
              break;
            }
          }
          num1 = 0.0f;
        }
      }
    }
    float height = (double) row.RowBreakHeight > 0.0 ? row.RowBreakHeight : row.Height;
    this.userHeight = this.m_startLocation.Y;
    bool flag3 = false;
    if (this.Grid.AllowRowBreakAcrossPages && row.Cells.Count > 0 && row.Cells[0].Value is PdfGrid)
    {
      PdfGrid pdfGrid = row.Cells[0].Value as PdfGrid;
      float num5 = 0.0f;
      using (List<PdfGridRow>.Enumerator enumerator = pdfGrid.Rows.GetEnumerator())
      {
        if (enumerator.MoveNext())
        {
          PdfGridRow current = enumerator.Current;
          num5 += current.Height;
          num5 += pdfGrid.Style.CellSpacing;
          if (row.Cells[0].Style != null)
          {
            if (row.Cells[0].Style.Borders != null)
              num5 += row.Cells[0].Style.Borders.Bottom.Width;
          }
        }
      }
      if ((double) height > (double) num5)
        flag3 = true;
    }
    if (this.Grid.IsChildGrid && this.Grid.ParentCell != null)
    {
      if ((double) height + (double) this.Grid.ParentCell.Row.Grid.Style.CellPadding.Bottom + (double) this.Grid.ParentCell.Row.Grid.Style.CellPadding.Top > (double) this.m_currentPageBounds.Height)
      {
        if (this.Grid.AllowRowBreakAcrossPages)
        {
          result.IsFinish = true;
          if (this.Grid.IsChildGrid && (double) row.RowBreakHeight > 0.0)
          {
            if (this.Grid.ParentCell.Row.Grid.Style.CellPadding != null)
              this.m_currentBounds.Y += this.Grid.ParentCell.Row.Grid.Style.CellPadding.Top;
            this.m_currentBounds.X = this.m_startLocation.X;
          }
          result.Bounds = this.m_currentBounds;
          this.DrawRowWithBreak(ref result, row, height);
        }
        else
        {
          if (this.Grid.ParentCell.Row.Grid.Style.CellPadding != null)
          {
            this.m_currentBounds.Y += this.Grid.ParentCell.Row.Grid.Style.CellPadding.Top;
            height = this.m_currentBounds.Height - this.m_currentBounds.Y - this.Grid.ParentCell.Row.Grid.Style.CellPadding.Bottom;
          }
          result.IsFinish = false;
          this.DrawRow(ref result, row, height);
        }
      }
      else if ((double) this.m_currentBounds.Y + (double) this.Grid.ParentCell.Row.Grid.Style.CellPadding.Bottom + (double) height > (double) this.m_currentPageBounds.Height || !this.Grid.IsChildGrid && (double) this.m_currentBounds.Y + (double) this.Grid.ParentCell.Row.Grid.Style.CellPadding.Bottom + (double) height > (double) this.m_currentBounds.Height && !this.Grid.IsChildGrid || (double) this.m_currentBounds.Y + (double) this.Grid.ParentCell.Row.Grid.Style.CellPadding.Bottom + (double) num1 > (double) this.m_currentPageBounds.Height)
      {
        bool flag4 = false;
        if (this.Grid.AllowRowBreakAcrossPages && !row.ColumnSpanExists && !this.Grid.RepeatHeader && !row.m_isRowHeightSet && !row.RowMergeComplete)
          flag4 = this.IsFitToCell(this.m_currentPageBounds.Height - this.m_currentBounds.Y, this.Grid, row);
        if (PdfGridLayouter.m_repeatRowIndex > -1 && PdfGridLayouter.m_repeatRowIndex == row.RowIndex || flag4)
        {
          if (this.Grid.AllowRowBreakAcrossPages)
          {
            result.IsFinish = true;
            if (this.Grid.IsChildGrid && (double) row.RowBreakHeight > 0.0)
            {
              if (this.Grid.ParentCell.Row.Grid.Style.CellPadding != null)
                this.m_currentBounds.Y += this.Grid.ParentCell.Row.Grid.Style.CellPadding.Top;
              this.m_currentBounds.X = this.m_startLocation.X;
            }
            result.Bounds = this.m_currentBounds;
            this.DrawRowWithBreak(ref result, row, height);
          }
          else
          {
            result.IsFinish = false;
            this.DrawRow(ref result, row, height);
          }
        }
        else
          result.IsFinish = false;
      }
      else
      {
        result.IsFinish = true;
        if (this.Grid.IsChildGrid && (double) row.RowBreakHeight > 0.0 && this.Grid.ParentCell.Row.Grid.Style.CellPadding != null)
          height += this.Grid.ParentCell.Row.Grid.Style.CellPadding.Bottom;
        this.DrawRow(ref result, row, height);
      }
    }
    else if ((double) height > (double) this.m_currentPageBounds.Height)
    {
      if (this.Grid.AllowRowBreakAcrossPages)
      {
        result.IsFinish = true;
        this.DrawRowWithBreak(ref result, row, height);
      }
      else
      {
        result.IsFinish = false;
        this.DrawRow(ref result, row, height);
      }
    }
    else if (flag3 || (double) this.m_currentBounds.Y + (double) height > (double) this.m_currentPageBounds.Height || (double) this.m_currentBounds.Y + (double) height - (double) this.userHeight > (double) this.m_currentBounds.Height || (double) this.m_currentBounds.Y + (double) num2 > (double) this.m_currentPageBounds.Height || (double) this.m_currentBounds.Y + (double) num1 > (double) this.m_currentPageBounds.Height || !flag2)
    {
      bool flag5 = false;
      if (this.Grid.AllowRowBreakAcrossPages && !this.Grid.RepeatHeader && !row.m_isRowHeightSet && !row.RowMergeComplete)
      {
        flag5 = this.Grid.LayoutFormat == null || (double) this.Grid.LayoutFormat.PaginateBounds.Height <= 0.0 || (double) this.m_currentBounds.Y + (double) height - (double) this.userHeight <= (double) this.m_currentBounds.Height ? this.IsFitToCell(this.m_currentPageBounds.Height - this.m_currentBounds.Y, this.Grid, row) : this.IsFitToCell(this.m_currentBounds.Height + this.m_startLocation.Y - this.m_currentBounds.Y, this.Grid, row);
        if (flag5)
          this.isPaginate = true;
      }
      if (PdfGridLayouter.m_repeatRowIndex > -1 && PdfGridLayouter.m_repeatRowIndex == row.RowIndex || flag5 || flag3)
      {
        if (this.Grid.AllowRowBreakAcrossPages)
        {
          result.IsFinish = true;
          this.DrawRowWithBreak(ref result, row, height);
        }
        else
        {
          result.IsFinish = false;
          this.DrawRow(ref result, row, height);
        }
      }
      else
        result.IsFinish = false;
    }
    else
    {
      result.IsFinish = true;
      if (row.Grid.m_hasHTMLText)
      {
        for (int cellStartIndex = this.m_cellStartIndex; cellStartIndex <= this.m_cellEndIndex; ++cellStartIndex)
        {
          if (row.Cells[cellStartIndex].Value is PdfHTMLTextElement && (double) height != (double) row.Cells[cellStartIndex].Height - (double) row.Cells[cellStartIndex].tempHeight)
            height = (row.Cells[cellStartIndex].Style.CellPadding == null ? this.Grid.Style.CellPadding.Bottom + this.Grid.Style.CellPadding.Top : row.Cells[cellStartIndex].Style.CellPadding.Bottom + row.Cells[cellStartIndex].Style.CellPadding.Top) + (row.Cells[cellStartIndex].Height - row.Cells[cellStartIndex].tempHeight);
        }
      }
      this.DrawRow(ref result, row, height);
    }
    return result;
  }

  private void DrawRowWithBreak(
    ref PdfGridLayouter.RowLayoutResult result,
    PdfGridRow row,
    float height)
  {
    PointF location1 = this.m_currentBounds.Location;
    if (row.Grid.IsChildGrid && row.Grid.AllowRowBreakAcrossPages && (double) this.m_startLocation.X != (double) this.m_currentBounds.X)
      location1.X = this.m_startLocation.X;
    result.Bounds = new RectangleF(location1, SizeF.Empty);
    this.m_newheight = (double) this.m_currentBounds.Height >= (double) this.m_currentPageBounds.Height ? ((double) row.RowBreakHeight > 0.0 ? this.m_currentPageBounds.Height : 0.0f) : ((double) row.RowBreakHeight > 0.0 ? this.m_currentBounds.Height : 0.0f);
    if ((double) row.Grid.Style.CellPadding.Top + (double) this.m_currentBounds.Y + (double) row.Grid.Style.CellPadding.Bottom < (double) this.m_currentPageBounds.Height)
    {
      row.RowBreakHeight = this.m_currentBounds.Y + height - this.m_currentPageBounds.Height;
      foreach (PdfGridCell cell in row.Cells)
      {
        float num = cell.MeasureHeight();
        if ((double) num == (double) height && cell.Value is PdfGrid)
          row.RowBreakHeight = 0.0f;
        else if ((double) num == (double) height && !(cell.Value is PdfGrid))
          row.RowBreakHeight = (double) this.m_currentBounds.Height >= (double) this.m_currentPageBounds.Height || row.Grid.IsChildGrid ? this.m_currentBounds.Y + height - this.m_currentPageBounds.Height : this.m_currentBounds.Y + height - this.m_currentBounds.Height;
      }
      for (int cellStartIndex = this.m_cellStartIndex; cellStartIndex <= this.m_cellEndIndex; ++cellStartIndex)
      {
        float width1 = this.Grid.Columns[cellStartIndex].Width;
        bool cancelSubsequentSpans = row.Cells[cellStartIndex].ColumnSpan + cellStartIndex > this.m_cellEndIndex + 1 && row.Cells[cellStartIndex].ColumnSpan > 1;
        if (!cancelSubsequentSpans)
        {
          for (int index = 1; index < row.Cells[cellStartIndex].ColumnSpan; ++index)
          {
            row.Cells[cellStartIndex + index].IsCellMergeContinue = true;
            width1 += this.Grid.Columns[cellStartIndex + index].Width;
          }
        }
        if ((double) height < (double) this.m_currentPageBounds.Height)
        {
          if ((double) this.m_currentBounds.Y + (double) height < (double) this.m_currentPageBounds.Height)
          {
            if ((double) this.m_currentBounds.Y + (double) height - (double) this.m_startLocation.Y > (double) this.m_currentBounds.Height)
            {
              row.RowBreakHeight = this.m_currentBounds.Y + height - this.m_startLocation.Y - this.m_currentBounds.Height;
              this.m_newheight = height - row.RowBreakHeight;
            }
            else
              this.m_newheight = height;
          }
          else
            this.m_newheight = this.m_currentPageBounds.Height - this.m_currentBounds.Y;
        }
        SizeF size1 = new SizeF();
        size1 = (double) this.m_currentBounds.Height >= (double) this.m_currentPageBounds.Height ? new SizeF(width1, (double) this.m_newheight > 0.0 ? this.m_newheight : this.m_currentPageBounds.Height) : new SizeF(width1, (double) this.m_newheight > 0.0 ? this.m_newheight : this.m_currentBounds.Height);
        if ((double) size1.Width == 0.0)
          size1 = new SizeF(row.Cells[cellStartIndex].Width, size1.Height);
        if (!this.CheckIfDefaultFormat(this.Grid.Columns[cellStartIndex].Format) && this.CheckIfDefaultFormat(row.Cells[cellStartIndex].StringFormat))
          row.Cells[cellStartIndex].StringFormat = this.Grid.Columns[cellStartIndex].Format;
        PdfGridCellStyle style = row.Cells[cellStartIndex].Style;
        PdfGridBeginCellLayoutEventArgs cellLayoutEventArgs = (PdfGridBeginCellLayoutEventArgs) null;
        if (!row.Cells[cellStartIndex].IsCellMergeContinue)
          cellLayoutEventArgs = this.RaiseBeforeCellLayout(this.m_currentGraphics, this.m_currentRowIndex, cellStartIndex, new RectangleF(location1, size1), row.Cells[cellStartIndex].Value is string ? row.Cells[cellStartIndex].Value.ToString() : string.Empty, ref style, row.IsHeaderRow);
        row.Cells[cellStartIndex].Style = style;
        bool flag = false;
        if (cellLayoutEventArgs != null)
          flag = cellLayoutEventArgs.Skip;
        PdfStringLayoutResult stringLayoutResult = (PdfStringLayoutResult) null;
        if (!flag)
        {
          if (row.PdfTag != null)
          {
            if (row.Cells[cellStartIndex].PdfTag == null)
              row.Cells[cellStartIndex].PdfTag = this.m_isHeader ? (PdfTag) new PdfStructureElement(PdfTagType.TableHeader) : (PdfTag) new PdfStructureElement(PdfTagType.TableDataCell);
            (row.Cells[cellStartIndex].PdfTag as PdfStructureElement).Parent = row.PdfTag as PdfStructureElement;
          }
          PointF location2 = location1;
          SizeF size2 = size1;
          float num1 = 0.0f;
          float num2 = 0.0f;
          float num3 = 0.0f;
          PdfGridCell cell = row.Cells[cellStartIndex];
          if (cell.Style.Borders != null)
          {
            if (cell.Style.Borders.Left != null)
              num1 = cell.Style.Borders.Left.Width;
            if (cell.Style.Borders.Right != null)
              num2 = cell.Style.Borders.Right.Width;
            if (cell.Style.Borders.Top != null)
              num3 = cell.Style.Borders.Top.Width;
            if (cell.Style.Borders.Bottom != null)
            {
              double width2 = (double) cell.Style.Borders.Bottom.Width;
            }
            if ((double) location1.X == 0.0)
            {
              location2.X += num1 / 2f;
              size2.Width -= num1 / 2f;
              row.Cells[cellStartIndex].cellBorderCuttOffX = true;
            }
            if ((double) location1.Y == 0.0)
            {
              location2.Y += num3 / 2f;
              size2.Height -= num3 / 2f;
              row.Cells[cellStartIndex].cellBorderCuttOffY = true;
            }
            if (cell.Value is PdfGrid && (double) location1.X == 0.0)
              row.Cells[cellStartIndex].cellBorderCuttOffX = true;
            if (cell.Value is PdfGrid && (double) location1.Y == 0.0)
              row.Cells[cellStartIndex].cellBorderCuttOffY = true;
            if (this.Grid.IsChildGrid)
            {
              if (cellStartIndex == 0)
              {
                location2.X += num1 / 2f;
                size2.Width -= num1 / 2f;
              }
              if ((double) location1.Y == (double) this.m_cellEventBounds.Y)
              {
                location2.Y += num3 / 2f;
                size2.Height -= num3 / 2f;
              }
              if (cellStartIndex != 0 && cellStartIndex == row.Cells.Count - 1 || cellStartIndex == 0 && row.Cells.Count - 1 == 0)
              {
                if ((double) location1.X + (double) size2.Width + (double) this.Grid.ParentCell.Style.Borders.Left.Width > (double) this.m_currentPageBounds.Width)
                  size2.Width -= (float) ((double) this.Grid.ParentCell.Style.Borders.Left.Width / 2.0 + (double) num2 / 2.0);
                else
                  size2.Width -= num2 / 2f;
              }
            }
          }
          stringLayoutResult = row.Cells[cellStartIndex].Draw(this.m_currentGraphics, new RectangleF(location2, size2), cancelSubsequentSpans);
        }
        if ((double) row.RowBreakHeight > 0.0 || (double) row.RowBreakHeight < 0.0 && stringLayoutResult != null && stringLayoutResult.Remainder != null)
        {
          if ((double) row.RowBreakHeight > 0.0 && row.Grid.AllowRowBreakAcrossPages)
            row.Cells[cellStartIndex].FinishedDrawingCell = false;
          if (stringLayoutResult != null)
          {
            row.Cells[cellStartIndex].FinishedDrawingCell = false;
            row.Cells[cellStartIndex].RemainingString = stringLayoutResult.Remainder == null ? string.Empty : stringLayoutResult.Remainder;
            if (row.Grid.IsChildGrid && (double) row.Grid.ParentCell.Row.Grid.Style.CellPadding.Bottom > 1.0)
              row.RowBreakHeight = height - stringLayoutResult.ActualSize.Height;
          }
          else if (row.Cells[cellStartIndex].Value is PdfImage)
            row.Cells[cellStartIndex].FinishedDrawingCell = false;
          else if (row.Cells[cellStartIndex].Value is PdfHTMLTextElement && row.Cells[cellStartIndex].layoutResult != null)
          {
            float num4 = 0.0f + row.Cells[cellStartIndex].Style.Borders.Bottom.Width * 2f;
            float num5 = row.Cells[cellStartIndex].Style.CellPadding != null ? num4 + row.Cells[cellStartIndex].Style.CellPadding.Bottom : num4 + row.Grid.Style.CellPadding.Bottom;
            float num6 = row.Cells[cellStartIndex].layoutBounds.Height - (row.Cells[cellStartIndex].layoutResult.Bounds.Height + num5);
            if ((double) num6 > 0.0)
              row.RowBreakHeight += num6;
          }
        }
        result.IsFinish = !result.IsFinish ? result.IsFinish : row.Cells[cellStartIndex].FinishedDrawingCell;
        if (!cancelSubsequentSpans && !row.Cells[cellStartIndex].IsCellMergeContinue)
          this.RaiseAfterCellLayout(this.m_currentGraphics, this.m_currentRowIndex, cellStartIndex, new RectangleF(location1, size1), row.Cells[cellStartIndex].Value is string ? row.Cells[cellStartIndex].Value.ToString() : string.Empty, row.Cells[cellStartIndex].Style, row.IsHeaderRow);
        if (row.Cells[cellStartIndex].Value is PdfGrid)
        {
          PdfGrid pdfGrid = row.Cells[cellStartIndex].Value as PdfGrid;
          this.m_rowBreakPageHeightCellIndex = cellStartIndex;
          row.Cells[cellStartIndex].m_pageCount = pdfGrid.m_listOfNavigatePages.Count;
          foreach (int listOfNavigatePage in pdfGrid.m_listOfNavigatePages)
          {
            if (!this.Grid.m_listOfNavigatePages.Contains(listOfNavigatePage))
              this.Grid.m_listOfNavigatePages.Add(listOfNavigatePage);
          }
          if ((double) this.Grid.Columns[cellStartIndex].Width >= (double) this.m_currentGraphics.ClientSize.Width)
          {
            location1.X = pdfGrid.m_rowLayoutBoundswidth;
            location1.X += pdfGrid.Style.CellSpacing;
          }
          else
            location1.X += this.Grid.Columns[cellStartIndex].Width;
        }
        else
          location1.X += this.Grid.Columns[cellStartIndex].Width;
      }
      this.m_currentBounds.Y += (double) this.m_newheight > 0.0 ? this.m_newheight : height;
      result.Bounds = new RectangleF(result.Bounds.Location, new SizeF(location1.X, location1.Y));
    }
    else
    {
      row.RowBreakHeight = height;
      result.IsFinish = false;
    }
  }

  private void DrawRow(ref PdfGridLayouter.RowLayoutResult result, PdfGridRow row, float height)
  {
    bool flag = false;
    PointF location1 = this.m_currentBounds.Location;
    result.Bounds = new RectangleF(location1, SizeF.Empty);
    height = this.ReCalculateHeight(row, height);
    for (int cellStartIndex = this.m_cellStartIndex; cellStartIndex <= this.m_cellEndIndex; ++cellStartIndex)
    {
      float width1 = this.Grid.Columns[cellStartIndex].Width;
      bool cancelSubsequentSpans = cellStartIndex > this.m_cellEndIndex + 1 && row.Cells[cellStartIndex].ColumnSpan > 1;
      if (!cancelSubsequentSpans)
      {
        for (int index = 1; index < row.Cells[cellStartIndex].ColumnSpan; ++index)
        {
          row.Cells[cellStartIndex + index].IsCellMergeContinue = true;
          width1 += this.Grid.Columns[cellStartIndex + index].Width;
        }
      }
      if ((double) height > (double) this.m_currentPageBounds.Height && !this.Grid.AllowRowBreakAcrossPages)
        height = this.m_currentPageBounds.Height;
      SizeF size1 = new SizeF(width1, height);
      if ((double) size1.Width > (double) this.m_currentGraphics.ClientSize.Width)
        size1.Width = this.m_currentGraphics.ClientSize.Width;
      if (this.Grid.IsChildGrid && this.Grid.Style.AllowHorizontalOverflow && (double) size1.Width >= (double) this.m_currentGraphics.ClientSize.Width)
        size1.Width -= 2f * this.m_currentBounds.X;
      if (!this.CheckIfDefaultFormat(this.Grid.Columns[cellStartIndex].Format) && this.CheckIfDefaultFormat(row.Cells[cellStartIndex].StringFormat))
        row.Cells[cellStartIndex].StringFormat = this.Grid.Columns[cellStartIndex].Format;
      PdfGridCellStyle style = row.Cells[cellStartIndex].Style;
      PdfGridBeginCellLayoutEventArgs cellLayoutEventArgs = (PdfGridBeginCellLayoutEventArgs) null;
      if (!row.Cells[cellStartIndex].IsCellMergeContinue)
        cellLayoutEventArgs = this.RaiseBeforeCellLayout(this.m_currentGraphics, this.m_currentRowIndex, cellStartIndex, new RectangleF(location1, size1), row.Cells[cellStartIndex].Value is string ? row.Cells[cellStartIndex].Value.ToString() : string.Empty, ref style, row.IsHeaderRow);
      row.Cells[cellStartIndex].Style = style;
      if (cellLayoutEventArgs != null)
        flag = cellLayoutEventArgs.Skip;
      if (!flag)
      {
        if (row.Cells[cellStartIndex].Value is PdfGrid)
          (row.Cells[cellStartIndex].Value as PdfGrid).parentCellIndex = cellStartIndex;
        if (row.PdfTag != null)
        {
          if (row.Cells[cellStartIndex].PdfTag == null)
            row.Cells[cellStartIndex].PdfTag = this.m_isHeader ? (PdfTag) new PdfStructureElement(PdfTagType.TableHeader) : (PdfTag) new PdfStructureElement(PdfTagType.TableDataCell);
          (row.Cells[cellStartIndex].PdfTag as PdfStructureElement).Parent = row.PdfTag as PdfStructureElement;
          this.m_currentGraphics.customTag = true;
        }
        PointF location2 = location1;
        SizeF size2 = size1;
        float num1 = 0.0f;
        float num2 = 0.0f;
        float num3 = 0.0f;
        PdfGridCell cell = row.Cells[cellStartIndex];
        if (cell.Style.Borders != null)
        {
          if (cell.Style.Borders.Left != null)
            num1 = cell.Style.Borders.Left.Width;
          if (cell.Style.Borders.Right != null)
            num2 = cell.Style.Borders.Right.Width;
          if (cell.Style.Borders.Top != null)
            num3 = cell.Style.Borders.Top.Width;
          if (cell.Style.Borders.Bottom != null)
          {
            double width2 = (double) cell.Style.Borders.Bottom.Width;
          }
          if ((double) location1.X == 0.0)
          {
            location2.X += num1 / 2f;
            size2.Width -= num1 / 2f;
            row.Cells[cellStartIndex].cellBorderCuttOffX = true;
          }
          if ((double) location1.Y == 0.0)
          {
            location2.Y += num3 / 2f;
            size2.Height -= num3 / 2f;
            row.Cells[cellStartIndex].cellBorderCuttOffY = true;
          }
          if (cell.Value is PdfGrid && (double) location1.X == 0.0)
            row.Cells[cellStartIndex].cellBorderCuttOffX = true;
          if (cell.Value is PdfGrid && (double) location1.Y == 0.0)
            row.Cells[cellStartIndex].cellBorderCuttOffY = true;
          if (this.Grid.IsChildGrid)
          {
            if (cellStartIndex == 0)
            {
              location2.X += num1 / 2f;
              size2.Width -= num1 / 2f;
            }
            if ((double) location1.Y == (double) this.m_cellEventBounds.Y)
            {
              location2.Y += num3 / 2f;
              size2.Height -= num3 / 2f;
            }
            if (cellStartIndex != 0 && cellStartIndex == row.Cells.Count - 1 || cellStartIndex == 0 && row.Cells.Count - 1 == 0)
            {
              if ((double) location1.X + (double) size2.Width + (double) this.Grid.ParentCell.Style.Borders.Left.Width > (double) this.m_currentPageBounds.Width)
                size2.Width -= (float) ((double) this.Grid.ParentCell.Style.Borders.Left.Width / 2.0 + (double) num2 / 2.0);
              else
                size2.Width -= num2 / 2f;
            }
          }
        }
        PdfStringLayoutResult stringLayoutResult = row.Cells[cellStartIndex].Draw(this.m_currentGraphics, new RectangleF(location2, size2), cancelSubsequentSpans);
        this.m_currentGraphics.customTag = false;
        if (stringLayoutResult == null && row.m_drawCellBroders && row.RowSpanExists && row.Grid.LayoutFormat != null && row.Grid.LayoutFormat.Layout == PdfLayoutType.Paginate)
        {
          if (row.RowIndex - 1 >= 0)
            row.Cells[cellStartIndex].Style = row.Grid.Rows[row.RowIndex - 1].Cells[cellStartIndex].Style;
          row.Cells[cellStartIndex].DrawCellBorders(ref this.m_currentGraphics, new RectangleF(location1, new SizeF(size1.Width, row.m_borderReminingHeight)));
        }
        if (row.Grid.Style.AllowHorizontalOverflow && (row.Cells[cellStartIndex].ColumnSpan > this.m_cellEndIndex || cellStartIndex + row.Cells[cellStartIndex].ColumnSpan > this.m_cellEndIndex + 1) && this.m_cellEndIndex < row.Cells.Count - 1)
          row.RowOverflowIndex = this.m_cellEndIndex;
        if (row.Grid.Style.AllowHorizontalOverflow && row.RowOverflowIndex > 0 && (row.Cells[cellStartIndex].ColumnSpan > this.m_cellEndIndex || cellStartIndex + row.Cells[cellStartIndex].ColumnSpan > this.m_cellEndIndex + 1) && row.Cells[cellStartIndex].ColumnSpan - this.m_cellEndIndex + cellStartIndex - 1 > 0)
        {
          row.Cells[row.RowOverflowIndex + 1].Value = (object) stringLayoutResult?.m_remainder;
          row.Cells[row.RowOverflowIndex + 1].StringFormat = row.Cells[cellStartIndex].StringFormat;
          row.Cells[row.RowOverflowIndex + 1].Style = row.Cells[cellStartIndex].Style;
          row.Cells[row.RowOverflowIndex + 1].ColumnSpan = row.Cells[cellStartIndex].ColumnSpan - this.m_cellEndIndex + cellStartIndex - 1;
        }
      }
      if (row.Cells[cellStartIndex].Value is PdfHTMLTextElement)
        result.IsFinish = row.Cells[cellStartIndex].FinishedDrawingCell;
      if (!cancelSubsequentSpans && !row.Cells[cellStartIndex].IsCellMergeContinue)
        this.RaiseAfterCellLayout(this.m_currentGraphics, this.m_currentRowIndex, cellStartIndex, new RectangleF(location1, size1), row.Cells[cellStartIndex].Value is string ? row.Cells[cellStartIndex].Value.ToString() : string.Empty, row.Cells[cellStartIndex].Style, row.IsHeaderRow);
      if (row.Cells[cellStartIndex].Value is PdfGrid)
      {
        PdfGrid pdfGrid = row.Cells[cellStartIndex].Value as PdfGrid;
        row.Cells[cellStartIndex].m_pageCount = pdfGrid.m_listOfNavigatePages.Count;
        this.m_rowBreakPageHeightCellIndex = cellStartIndex;
        foreach (int listOfNavigatePage in pdfGrid.m_listOfNavigatePages)
        {
          if (!this.Grid.m_listOfNavigatePages.Contains(listOfNavigatePage))
            this.Grid.m_listOfNavigatePages.Add(listOfNavigatePage);
        }
        if ((double) this.Grid.Columns[cellStartIndex].Width >= (double) this.m_currentGraphics.ClientSize.Width)
        {
          location1.X = pdfGrid.m_rowLayoutBoundswidth;
          location1.X += pdfGrid.Style.CellSpacing;
        }
        else
          location1.X += this.Grid.Columns[cellStartIndex].Width;
      }
      else
        location1.X += this.Grid.Columns[cellStartIndex].Width;
    }
    if (!row.RowMergeComplete || row.m_isRowHeightSet)
      this.m_currentBounds.Y += height;
    result.Bounds = new RectangleF(result.Bounds.Location, new SizeF(location1.X, location1.Y));
    this.m_currentGraphics.customTag = false;
  }

  private float ReCalculateHeight(PdfGridRow row, float height)
  {
    float num = 0.0f;
    for (int cellStartIndex = this.m_cellStartIndex; cellStartIndex <= this.m_cellEndIndex; ++cellStartIndex)
    {
      if (!string.IsNullOrEmpty(row.Cells[cellStartIndex].RemainingString))
        num = Math.Max(num, row.Cells[cellStartIndex].MeasureHeight());
    }
    return Math.Max(height, num);
  }

  private bool RaiseBeforePageLayout(
    PdfPage currentPage,
    ref RectangleF currentBounds,
    ref int currentRow)
  {
    bool flag = false;
    if (this.Element.RaiseBeginPageLayout)
    {
      PdfGridBeginPageLayoutEventArgs e = new PdfGridBeginPageLayoutEventArgs(currentBounds, currentPage, currentRow);
      this.Element.OnBeginPageLayout((BeginPageLayoutEventArgs) e);
      if (currentBounds != e.Bounds)
      {
        this.isChanged = true;
        this.m_currentLocation = e.Bounds.Location;
        this.Grid.MeasureColumnsWidth(new RectangleF(e.Bounds.Location, new SizeF(e.Bounds.Width + e.Bounds.X, e.Bounds.Height)));
      }
      flag = e.Cancel;
      currentBounds = e.Bounds;
      currentRow = e.StartRowIndex;
    }
    return flag;
  }

  private PdfGridEndPageLayoutEventArgs RaisePageLayouted(PdfLayoutResult result)
  {
    PdfGridEndPageLayoutEventArgs e = new PdfGridEndPageLayoutEventArgs(result);
    if (this.Element.RaiseEndPageLayout)
      this.Element.OnEndPageLayout((EndPageLayoutEventArgs) e);
    return e;
  }

  private PdfGridBeginCellLayoutEventArgs RaiseBeforeCellLayout(
    PdfGraphics graphics,
    int rowIndex,
    int cellIndex,
    RectangleF bounds,
    string value,
    ref PdfGridCellStyle style,
    bool isHeaderRow)
  {
    PdfGridBeginCellLayoutEventArgs args = (PdfGridBeginCellLayoutEventArgs) null;
    if (this.Grid.RaiseBeginCellLayout)
    {
      args = new PdfGridBeginCellLayoutEventArgs(graphics, rowIndex, cellIndex, bounds, value, style, isHeaderRow);
      this.Grid.OnBeginCellLayout(args);
      style = args.Style;
    }
    return args;
  }

  private void RaiseAfterCellLayout(
    PdfGraphics graphics,
    int rowIndex,
    int cellIndex,
    RectangleF bounds,
    string value,
    PdfGridCellStyle cellstyle,
    bool isHeaderRow)
  {
    if (!this.Grid.RaiseEndCellLayout)
      return;
    this.Grid.OnEndCellLayout(new PdfGridEndCellLayoutEventArgs(graphics, rowIndex, cellIndex, bounds, value, cellstyle, isHeaderRow));
  }

  private bool CheckIfDefaultFormat(PdfStringFormat format)
  {
    PdfStringFormat pdfStringFormat = new PdfStringFormat();
    return format.Alignment == pdfStringFormat.Alignment && (double) format.CharacterSpacing == (double) pdfStringFormat.CharacterSpacing && format.ClipPath == pdfStringFormat.ClipPath && (double) format.FirstLineIndent == (double) pdfStringFormat.FirstLineIndent && (double) format.HorizontalScalingFactor == (double) pdfStringFormat.HorizontalScalingFactor && format.LineAlignment == pdfStringFormat.LineAlignment && format.LineLimit == pdfStringFormat.LineLimit && (double) format.LineSpacing == (double) pdfStringFormat.LineSpacing && format.MeasureTrailingSpaces == pdfStringFormat.MeasureTrailingSpaces && format.NoClip == pdfStringFormat.NoClip && (double) format.ParagraphIndent == (double) pdfStringFormat.ParagraphIndent && format.RightToLeft == pdfStringFormat.RightToLeft && format.SubSuperScript == pdfStringFormat.SubSuperScript && (double) format.WordSpacing == (double) pdfStringFormat.WordSpacing && format.WordWrap == pdfStringFormat.WordWrap;
  }

  private void DetermineColumnDrawRanges()
  {
    int num1 = 0;
    int num2 = 0;
    float num3 = 0.0f;
    float num4 = this.m_currentGraphics.ClientSize.Width - this.m_currentBounds.X;
    for (int index1 = 0; index1 < this.Grid.Columns.Count; ++index1)
    {
      num3 += this.Grid.Columns[index1].Width;
      if ((double) num3 >= (double) num4)
      {
        float num5 = 0.0f;
        for (int index2 = num1; index2 <= index1; ++index2)
        {
          num5 += this.Grid.Columns[index2].Width;
          if ((double) num5 <= (double) num4)
            num2 = index2;
          else
            break;
        }
        this.m_columnRanges.Add(new int[2]{ num1, num2 });
        num1 = ++num2;
        num3 = num2 <= index1 ? this.Grid.Columns[index1].Width : 0.0f;
      }
    }
    if (num1 == this.Grid.Columns.Count)
      return;
    this.m_columnRanges.Add(new int[2]
    {
      num1,
      this.Grid.Columns.Count - 1
    });
  }

  private void ReArrangePages(PdfPage page)
  {
    List<PdfPage> pdfPageList = new List<PdfPage>();
    int count1 = page.Document.Pages.count;
    int index1 = 0;
    int count2 = this.m_columnRanges.Count;
    if (count1 <= this.m_columnRanges.Count)
    {
      for (int index2 = 0; index2 < this.m_columnRanges.Count; ++index2)
      {
        page.Document.Pages.Add();
        if (page.Document.Pages.count > this.m_columnRanges.Count + 1)
          break;
      }
    }
    int count3 = page.Document.Pages.count;
    for (int index3 = 0; index3 < count3; ++index3)
    {
      if (index1 < count3 && pdfPageList.Count != count3)
      {
        PdfPage page1 = page.Document.Pages[index1];
        if (!pdfPageList.Contains(page1))
          pdfPageList.Add(page1);
      }
      if (count2 < count3 && pdfPageList.Count != count3)
      {
        PdfPage page2 = page.Document.Pages[count2];
        if (!pdfPageList.Contains(page2))
          pdfPageList.Add(page2);
      }
      if (pdfPageList.Count != count3)
      {
        ++index1;
        ++count2;
      }
      else
        break;
    }
    PdfDocument document = page.Document;
    for (int index4 = 0; index4 < pdfPageList.Count; ++index4)
    {
      PdfPage page3 = pdfPageList[index4];
      page3.Section = (PdfSection) null;
      document.Pages.Remove(page3);
    }
    for (int index5 = 0; index5 < pdfPageList.Count; ++index5)
      document.Pages.Add(pdfPageList[index5]);
  }

  public PdfPage GetNextPage(PdfLayoutFormat format)
  {
    PdfSection section = this.m_currentPage.Section;
    int num1 = section.IndexOf(this.m_currentPage);
    if (this.m_currentPage.Document.Pages.count > 1 && this.m_hType == PdfHorizontalOverflowType.NextPage && this.flag && this.m_columnRanges.Count > 1)
    {
      this.Grid.m_isRearranged = true;
      this.ReArrangePages(this.m_currentPage);
    }
    this.flag = false;
    PdfPage nextPage = num1 != section.Count - 1 ? section[num1 + 1] : section.Add();
    this.m_currentGraphics = nextPage.Graphics;
    int num2 = (this.m_currentGraphics.Page as PdfPage).Section.IndexOf(this.m_currentGraphics.Page as PdfPage);
    if (!this.Grid.m_listOfNavigatePages.Contains(num2))
      this.Grid.m_listOfNavigatePages.Add(num2);
    this.m_currentBounds = new RectangleF(PointF.Empty, nextPage.GetClientSize());
    if (format.PaginateBounds != RectangleF.Empty)
    {
      this.m_currentBounds.X = format.PaginateBounds.X;
      this.m_currentBounds.Y = format.PaginateBounds.Y;
      this.m_currentBounds.Height = format.PaginateBounds.Size.Height;
    }
    this.m_GridPaginated = true;
    return nextPage;
  }

  private PdfGridLayoutFormat GetFormat(PdfLayoutFormat format)
  {
    PdfGridLayoutFormat format1 = format as PdfGridLayoutFormat;
    if (format != null && format1 == null)
      format1 = new PdfGridLayoutFormat(format);
    return format1;
  }

  private PdfGridLayoutResult GetLayoutResult()
  {
    if (this.Grid.IsChildGrid && this.Grid.AllowRowBreakAcrossPages)
    {
      foreach (PdfGridRow row in (List<PdfGridRow>) this.Grid.Rows)
      {
        if ((double) row.RowBreakHeight > 0.0)
          this.m_startLocation.Y = this.m_currentPage.Origin.Y;
      }
    }
    return new PdfGridLayoutResult(this.m_currentPage, !this.isChanged ? (this.Grid.IsChildGrid || (double) this.m_currentBounds.Y <= (double) this.m_currentBounds.Height ? new RectangleF(this.m_startLocation, new SizeF(this.m_currentBounds.Width, this.m_currentBounds.Y - this.m_startLocation.Y)) : new RectangleF(this.m_startLocation, new SizeF(this.m_currentBounds.Width, this.m_currentBounds.Height))) : new RectangleF(this.m_currentLocation, new SizeF(this.m_currentBounds.Width, this.m_currentBounds.Y - this.m_currentLocation.Y)));
  }

  private bool IsFitToCell(float currentHeight, PdfGrid grid, PdfGridRow gridRow)
  {
    bool cell = false;
    PdfStringLayouter pdfStringLayouter = new PdfStringLayouter();
    for (int index = 0; index < gridRow.Cells.Count; ++index)
    {
      if (gridRow.Cells[index].Value is string && gridRow.Cells[index].Value != null || gridRow.Cells[index].Value is PdfTextElement && gridRow.Cells[index].Value != null)
      {
        PdfFont font = grid.Style.Font ?? gridRow.Style.Font ?? gridRow.Grid.Style.Font ?? PdfDocument.DefaultFont;
        this.remainderText = gridRow.Cells[index].Value as string;
        float width = gridRow.Cells[index].Width;
        if (grid.Columns[index].isCustomWidth && (double) gridRow.Cells[index].Width > (double) grid.Columns[index].Width)
          width = grid.Columns[index].Width;
        if (gridRow.Cells[index].Value is PdfTextElement)
        {
          string text = (gridRow.Cells[index].Value as PdfTextElement).Text;
          this.remainderText = text;
          this.slr = pdfStringLayouter.Layout(text, font, gridRow.Cells[index].StringFormat, new SizeF(width, currentHeight));
        }
        else
          this.slr = pdfStringLayouter.Layout(gridRow.Cells[index].Value as string, font, gridRow.Cells[index].StringFormat, new SizeF(width, currentHeight));
        float height = this.slr.ActualSize.Height;
        if ((double) height == 0.0)
        {
          cell = false;
          break;
        }
        if (gridRow.Cells[index].Style != null && gridRow.Cells[index].Style.Borders != null && gridRow.Cells[index].Style.Borders.Top != null && gridRow.Cells[index].Style.Borders.Bottom != null)
          height += (float) (((double) gridRow.Cells[index].Style.Borders.Top.Width + (double) gridRow.Cells[index].Style.Borders.Bottom.Width) * 2.0);
        if (this.slr.LineCount > 1 && gridRow.Cells[index].StringFormat != null && (double) gridRow.Cells[index].StringFormat.LineSpacing != 0.0)
          height += (float) (this.slr.LineCount - 1) * gridRow.Cells[index].Style.StringFormat.LineSpacing;
        float num = (gridRow.Cells[index].Style.CellPadding != null ? height + (grid.Style.CellPadding.Top + grid.Style.CellPadding.Bottom) : height + (grid.Style.CellPadding.Top + grid.Style.CellPadding.Bottom)) + grid.Style.CellSpacing;
        if ((double) currentHeight > (double) num || this.slr.m_remainder != null || (double) this.slr.ActualSize.Height > 0.0 && this.slr.Remainder == null && this.slr.LineCount > 1)
        {
          cell = true;
          break;
        }
      }
    }
    return cell;
  }

  internal class RowLayoutResult
  {
    private bool m_bIsFinished;
    private RectangleF m_layoutedBounds;

    public bool IsFinish
    {
      get => this.m_bIsFinished;
      set => this.m_bIsFinished = value;
    }

    public RectangleF Bounds
    {
      get => this.m_layoutedBounds;
      set => this.m_layoutedBounds = value;
    }

    public RowLayoutResult() => this.m_layoutedBounds = new RectangleF();
  }
}
