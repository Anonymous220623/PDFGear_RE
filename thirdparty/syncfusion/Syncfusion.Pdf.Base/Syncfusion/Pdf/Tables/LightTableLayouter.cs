// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Tables.LightTableLayouter
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Tables;

internal class LightTableLayouter : ElementLayouter
{
  private string[] m_row;
  private PdfStringLayoutResult[] m_latestTextResults;
  private float[] m_cellWidths;
  private PdfPage m_currentPage;
  private SizeF m_currentPageBounds;
  private PdfGraphics m_currentGraphics;
  private RectangleF m_currentBounds;
  private float m_cellSpacing;
  private int[] m_spanMap;
  private int m_dropIndex;
  private int m_startColumn;
  private int m_endColumn;
  private int m_previousRowIndex = -1;
  private int m_currentRowIndex;
  private int m_currentCellIndex;

  public PdfLightTable Table => this.Element as PdfLightTable;

  internal LightTableLayouter(PdfLightTable table)
    : base((PdfLayoutElement) table)
  {
  }

  public void Layout(PdfGraphics graphics, PointF location)
  {
    RectangleF boundaries = new RectangleF(location, SizeF.Empty);
    this.Layout(graphics, boundaries);
  }

  public void Layout(PdfGraphics graphics, RectangleF boundaries)
  {
    if (graphics == null)
      throw new ArgumentNullException(nameof (graphics));
    if ((double) graphics.ClientSize.Height < 0.0)
      boundaries.Y += graphics.ClientSize.Height;
    double width = (double) graphics.ClientSize.Width;
    double x = (double) boundaries.X;
    PdfLayoutParams pdfLayoutParams = new PdfLayoutParams();
    pdfLayoutParams.Bounds = boundaries;
    this.m_currentGraphics = graphics;
    this.LayoutInternal(pdfLayoutParams);
  }

  protected override PdfLayoutResult LayoutInternal(PdfLayoutParams param)
  {
    PdfLightTableLayoutFormat tableLayoutFormat = param != null ? this.GetFormat(param.Format) : throw new ArgumentNullException(nameof (param));
    if (tableLayoutFormat != null)
    {
      this.m_startColumn = tableLayoutFormat.StartColumnIndex;
      this.m_endColumn = tableLayoutFormat.EndColumnIndex;
    }
    if (this.m_endColumn == 0)
      this.m_endColumn = this.Table.Columns.Count - 1;
    if (this.m_endColumn < this.m_startColumn)
      throw new PdfLightTableException("End column index is less than start column index.");
    int count = this.Table.Columns.Count;
    if (this.m_startColumn < 0 || this.m_startColumn >= count || this.m_endColumn >= count || this.m_endColumn - this.m_startColumn > count)
      throw new PdfLightTableException("The selected columns are out of the existing range.");
    this.m_dropIndex = 0;
    this.m_row = (string[]) null;
    this.m_latestTextResults = (PdfStringLayoutResult[]) null;
    this.m_currentPage = param.Page;
    this.m_currentPageBounds = this.m_currentPage != null ? this.m_currentPage.GetClientSize() : this.m_currentGraphics.ClientSize;
    this.m_currentBounds = param.Bounds;
    LightTableLayouter.PageLayoutResult pageResult = (LightTableLayouter.PageLayoutResult) null;
    if ((double) this.m_currentBounds.Width <= 0.0)
    {
      float num = this.m_currentPageBounds.Width - this.m_currentBounds.X;
      this.m_currentBounds.Width = (double) num >= 0.0 ? num : throw new PdfLightTableException("Can't draw table outside of the page.");
    }
    param.Bounds = this.m_currentBounds;
    PdfLightTableStyle style = this.Table.Style;
    this.m_cellSpacing = style.CellSpacing;
    int currentRow = style.HeaderSource == PdfHeaderSource.Rows ? style.HeaderRowCount : 0;
    this.m_cellWidths = this.GetWidths(param.Bounds);
    bool isPageFirst = true;
    while (true)
    {
      do
      {
        bool flag = this.RaiseBeforePageLayout(this.m_currentPage, ref this.m_currentBounds, ref currentRow);
        LightTableEndPageLayoutEventArgs pageLayoutEventArgs = (LightTableEndPageLayoutEventArgs) null;
        if (!flag)
        {
          pageResult = this.LayoutOnPage(currentRow, param, isPageFirst);
          pageLayoutEventArgs = this.RaisePageLayouted(pageResult);
          flag = pageLayoutEventArgs != null && pageLayoutEventArgs.Cancel;
        }
        if (flag || pageResult.Finish)
          return (PdfLayoutResult) this.GetLayoutResult(pageResult);
        this.m_currentPage = pageLayoutEventArgs == null || pageLayoutEventArgs.NextPage == null ? this.GetNextPage(this.m_currentPage) : pageLayoutEventArgs.NextPage;
        this.m_currentPageBounds = this.m_currentPage != null ? this.m_currentPage.GetClientSize() : this.m_currentGraphics.ClientSize;
        isPageFirst = false;
        currentRow = pageResult.LastRowIndex;
        this.m_currentBounds = this.GetPaginateBounds(param);
      }
      while ((double) this.m_currentBounds.Height != 0.0);
      this.m_currentBounds.Y = 0.0f;
    }
  }

  private PdfLightTableLayoutFormat GetFormat(PdfLayoutFormat format)
  {
    PdfLightTableLayoutFormat format1 = format as PdfLightTableLayoutFormat;
    if (format != null && format1 == null)
      format1 = new PdfLightTableLayoutFormat(format);
    return format1;
  }

  private PdfLightTableLayoutResult GetLayoutResult(LightTableLayouter.PageLayoutResult pageResult)
  {
    PdfPage page = pageResult != null ? pageResult.Page : this.m_currentPage;
    RectangleF bounds = pageResult != null ? pageResult.Bounds : RectangleF.Empty;
    if (pageResult == null)
      pageResult = new LightTableLayouter.PageLayoutResult();
    return new PdfLightTableLayoutResult(page, bounds, pageResult.LastRowIndex, this.m_latestTextResults);
  }

  private LightTableLayouter.PageLayoutResult LayoutOnPage(
    int startRowIndex,
    PdfLayoutParams param,
    bool isPageFirst)
  {
    int rowIndex = startRowIndex;
    RectangleF rectangleF1 = this.m_currentBounds;
    if ((double) rectangleF1.Height == 0.0 && this.m_currentPage != null)
      rectangleF1.Height = this.m_currentPageBounds.Height - rectangleF1.Y;
    RectangleF rectangleF2 = rectangleF1;
    PdfLightTableStyle style = this.Table.Style;
    PdfPen borderPen = style.BorderPen;
    if (borderPen != null)
      rectangleF1 = LightTableLayouter.PreserveForBorder(rectangleF1, borderPen, style.BorderOverlapStyle);
    rectangleF1.Height -= this.m_cellSpacing;
    rectangleF1.Width -= this.m_cellSpacing;
    float rowHeight = 0.0f;
    LightTableLayouter.PageLayoutResult pageLayoutResult = new LightTableLayouter.PageLayoutResult();
    bool flag1 = false;
    bool isHeader = style.ShowHeader && (isPageFirst || style.RepeatHeader);
    bool flag2 = style.HeaderSource != PdfHeaderSource.Rows;
    int headerRowCount = style.HeaderRowCount;
    if (isHeader && !flag2)
    {
      if (headerRowCount > 0)
        rowIndex = 0;
      else
        isHeader = false;
    }
    string[] row1 = this.m_row;
    if (isHeader)
      this.m_row = (string[]) null;
    PdfGraphics pdfGraphics = this.m_currentPage != null ? this.m_currentPage.Graphics : this.m_currentGraphics;
    while (true)
    {
      do
      {
        string[] row2;
        if (isHeader && flag2)
        {
          if (!this.Table.isCustomDataSource)
          {
            rowIndex = -1;
            this.m_previousRowIndex = -2;
          }
          string[] columnCaptions = this.Table.GetColumnCaptions();
          if (columnCaptions == null)
          {
            isHeader = false;
            rowIndex = startRowIndex;
            this.m_row = row1;
            continue;
          }
          row2 = this.CropRow(columnCaptions);
        }
        else
          row2 = this.GetRow(rowIndex, param);
        bool stop = row2 == null;
        if (row2 != null)
        {
          rectangleF1.Y += this.m_cellSpacing;
          rectangleF1.Height -= this.m_cellSpacing;
          bool flag3 = this.DrawRow(param, ref rowIndex, row2, rectangleF1, out rowHeight, isHeader, out stop);
          if (!flag3)
          {
            rectangleF1.Y += rowHeight;
            rectangleF1.Height -= rowHeight;
          }
          else if ((double) rectangleF1.Height < (double) rowHeight || (double) rectangleF1.Y + (double) this.m_cellSpacing < (double) rectangleF2.Height)
          {
            rectangleF1.Y -= this.m_cellSpacing;
            rectangleF1.Height += this.m_cellSpacing;
          }
          stop |= flag3;
          flag1 = ((flag1 ? 1 : 0) | ((double) rowHeight > 0.0 ? 0 : (startRowIndex == rowIndex ? 1 : (isHeader ? 1 : 0)))) != 0;
          stop |= flag1;
        }
        else
          pageLayoutResult.Finish = true;
        if (stop)
        {
          if ((double) rowHeight > 0.0)
            rectangleF1.Y += this.m_cellSpacing;
          if (borderPen != null)
            rectangleF1.Y += borderPen.Width;
          if (isHeader)
            rowIndex = startRowIndex;
          pageLayoutResult.Page = this.m_currentPage;
          pageLayoutResult.FirstRowIndex = startRowIndex;
          pageLayoutResult.LastRowIndex = rowIndex;
          pageLayoutResult.Bounds = rectangleF2;
          pageLayoutResult.Bounds.Height = rectangleF1.Y - rectangleF2.Y;
          RectangleF rectangleF3 = pageLayoutResult.Bounds;
          if (borderPen != null)
          {
            if (style.BorderOverlapStyle == PdfBorderOverlapStyle.Overlap)
              rectangleF3.Height -= borderPen.Width / 2f;
            rectangleF3 = LightTableLayouter.PreserveForBorder(rectangleF3, borderPen, PdfBorderOverlapStyle.Overlap);
          }
          if (borderPen != null && (double) rectangleF3.Bottom < (double) this.m_currentPageBounds.Height)
          {
            float alpha = (float) borderPen.Color.A / (float) byte.MaxValue;
            pdfGraphics.Save();
            pdfGraphics.SetTransparency(alpha);
            pdfGraphics.DrawRectangle(borderPen, rectangleF3);
            pdfGraphics.Restore();
            goto label_42;
          }
          goto label_42;
        }
      }
      while (!isHeader || !flag2 && rowIndex < headerRowCount);
      isHeader = false;
      if (!this.Table.isCustomDataSource)
        rowIndex = startRowIndex;
      this.m_row = row1;
    }
label_42:
    bool flag4 = param.Format == null || param.Format.Layout == PdfLayoutType.OnePage;
    pageLayoutResult.Finish |= flag4;
    if ((double) param.Bounds.Y == (double) rectangleF1.Y && isHeader)
      isHeader = false;
    if (flag1 || this.m_row != null && isHeader)
      throw new PdfLightTableException("Can't draw table, because there is not enough space for it.");
    return pageLayoutResult;
  }

  private string[] CropRow(string[] row)
  {
    string[] destinationArray = row;
    if (row != null && (this.m_endColumn != 0 || this.m_startColumn != 0))
    {
      int length = this.m_endColumn - this.m_startColumn + 1;
      destinationArray = new string[length];
      Array.Copy((Array) row, this.m_startColumn, (Array) destinationArray, 0, length);
    }
    return destinationArray;
  }

  private static RectangleF PreserveForBorder(
    RectangleF bounds,
    PdfPen pen,
    PdfBorderOverlapStyle overlapStyle)
  {
    if (pen != null)
    {
      float width = pen.Width;
      if (overlapStyle == PdfBorderOverlapStyle.Overlap)
      {
        float num = width / 2f;
        bounds.X += num;
        bounds.Y += num;
        bounds.Width -= width;
        bounds.Height -= width;
      }
      else
      {
        if (overlapStyle != PdfBorderOverlapStyle.Inside)
          throw new ArgumentException("Unsupported overlap style.");
        float num = width * 2f;
        bounds.X += width;
        bounds.Y += width;
        bounds.Width -= num;
        bounds.Height -= num;
      }
    }
    return bounds;
  }

  private PdfFont CreateBoldFont(PdfFont font)
  {
    PdfFont boldFont;
    if (font is PdfStandardFont)
    {
      boldFont = (PdfFont) new PdfStandardFont(font as PdfStandardFont, font.Size, PdfFontStyle.Bold);
    }
    else
    {
      PdfTrueTypeFont pdfTrueTypeFont = font as PdfTrueTypeFont;
      boldFont = (PdfFont) new PdfTrueTypeFont(new Font(font.Name, font.Size, FontStyle.Bold), pdfTrueTypeFont.Unicode);
    }
    return boldFont;
  }

  private PdfFont CreateRegularFont(PdfFont font)
  {
    PdfFont regularFont;
    if (font is PdfStandardFont)
    {
      regularFont = (PdfFont) new PdfStandardFont(font as PdfStandardFont, font.Size, PdfFontStyle.Regular);
    }
    else
    {
      PdfTrueTypeFont pdfTrueTypeFont = font as PdfTrueTypeFont;
      regularFont = (PdfFont) new PdfTrueTypeFont(new Font(font.Name, font.Size, FontStyle.Regular), pdfTrueTypeFont.Unicode);
    }
    return regularFont;
  }

  private PdfFont CreateItalicFont(PdfFont font)
  {
    PdfFont italicFont;
    if (font is PdfStandardFont)
    {
      italicFont = (PdfFont) new PdfStandardFont(font as PdfStandardFont, font.Size, PdfFontStyle.Italic);
    }
    else
    {
      PdfTrueTypeFont pdfTrueTypeFont = font as PdfTrueTypeFont;
      italicFont = (PdfFont) new PdfTrueTypeFont(new Font(font.Name, font.Size, FontStyle.Italic), pdfTrueTypeFont.Unicode);
    }
    return italicFont;
  }

  private bool DrawRow(
    PdfLayoutParams param,
    ref int rowIndex,
    string[] row,
    RectangleF rowBouds,
    out float rowHeight,
    bool isHeader,
    out bool stop)
  {
    int length = this.m_cellWidths.Length;
    PdfStringLayoutResult[] results = (PdfStringLayoutResult[]) null;
    bool hasOwnStyle;
    PdfCellStyle pdfCellStyle = this.GetCellStyle(rowIndex, isHeader, out hasOwnStyle);
    BeginRowLayoutEventArgs rowLayoutEventArgs = this.RaiseBeforeRowLayout(rowIndex, pdfCellStyle);
    bool flag1 = false;
    this.m_spanMap = (int[]) null;
    rowHeight = 0.0f;
    if (rowLayoutEventArgs != null)
    {
      stop = rowLayoutEventArgs.Cancel;
      flag1 = rowLayoutEventArgs.Skip;
      this.m_spanMap = rowLayoutEventArgs.ColumnSpanMap;
      pdfCellStyle = rowLayoutEventArgs.CellStyle;
      this.ValidateSpanMap();
      rowHeight = Math.Max(rowLayoutEventArgs.MinimalHeight, rowHeight);
    }
    else
      stop = false;
    if (!stop)
    {
      float rowHeight1 = this.DetermineRowHeight(param, rowIndex, row, rowBouds, out results, pdfCellStyle);
      rowHeight = (double) rowHeight1 <= 0.0 ? rowHeight1 : Math.Max(rowHeight1, rowHeight);
      this.m_latestTextResults = results;
    }
    if ((double) rowHeight <= 0.0 || stop)
      return this.IsIncomplete(results) | (double) this.m_currentPageBounds.Height - (double) rowBouds.Y <= 0.0;
    rowBouds.Height = rowHeight;
    if ((double) (rowBouds.Y + rowBouds.Height) > (double) this.m_currentPageBounds.Height && this.m_currentPage != null)
      return true;
    bool flag2 = false;
    RectangleF bounds = rowBouds;
    PdfGraphics graphics = this.m_currentPage != null ? this.m_currentPage.Graphics : this.m_currentGraphics;
    int num = 0;
    if (!flag1)
    {
      for (int index = 0; index < length; ++index)
      {
        bounds.Width = this.GetCellWidth(index);
        bool flag3 = this.m_spanMap != null && this.m_spanMap[index] < 0;
        string str = row[index];
        bool flag4 = false;
        if (!flag3)
        {
          bounds.X += this.m_cellSpacing;
          if (!flag1 && !flag4 && !results[index].Empty)
          {
            BeginCellLayoutEventArgs cellLayoutEventArgs = this.RaiseBeforeCellLayout(graphics, rowIndex, index, bounds, str);
            if (cellLayoutEventArgs != null)
              flag4 = cellLayoutEventArgs.Skip;
            if (flag4)
              ++num;
          }
        }
        PdfStringLayoutResult layoutResult = results[index];
        if (!flag1 && !flag4 && !layoutResult.Empty)
        {
          bool ignoreColumnFormat = false;
          if (rowLayoutEventArgs != null)
            ignoreColumnFormat = rowLayoutEventArgs.IgnoreColumnFormat;
          if (isHeader && hasOwnStyle && this.Table.Columns[index] != null && (this.Table.Style.HeaderStyle != null && this.Table.Style.HeaderStyle.StringFormat != null || this.Table.Columns[index].StringFormat == null))
            ignoreColumnFormat = true;
          if (this.Table.isBuiltinStyle && this.Table.m_lightTableBuiltinStyle != PdfLightTableBuiltinStyle.TableGrid)
          {
            this.m_currentRowIndex = rowIndex;
            this.m_currentCellIndex = index;
            this.ApplyStyle(this.Table.m_lightTableBuiltinStyle);
            pdfCellStyle = this.Table.Style.DefaultStyle;
          }
          layoutResult = this.DrawCell(layoutResult, bounds, rowIndex, index, pdfCellStyle, ignoreColumnFormat);
          if (this.Table.isBuiltinStyle && isHeader && this.Table.m_headerStyle)
          {
            if (pdfCellStyle.Font.Style == PdfFontStyle.Regular)
              pdfCellStyle.Font = this.CreateBoldFont(pdfCellStyle.Font);
            else if (pdfCellStyle.Font.Style == PdfFontStyle.Bold)
              pdfCellStyle.Font = this.CreateRegularFont(pdfCellStyle.Font);
          }
        }
        if (!flag3)
          this.RaiseAfterCellLayout(graphics, rowIndex, index, bounds, str);
        string remainder = layoutResult.Remainder;
        if (remainder != null && remainder != string.Empty)
          flag2 = true;
        row[index] = remainder;
        if (!flag3)
          bounds.X += bounds.Width;
      }
    }
    else
      rowHeight = 0.0f;
    if (!flag2)
    {
      this.m_row = (string[]) null;
      ++rowIndex;
    }
    else if (!isHeader)
      this.m_row = row;
    stop = this.RaiseAfterRowLayout(rowIndex, !flag2, rowBouds);
    return flag2;
  }

  private void DrawBorder(RectangleF bounds, PdfGraphics graphics, PdfCellStyle style)
  {
    PointF point1_1 = new PointF(bounds.X, bounds.Y + bounds.Height);
    PointF point2 = bounds.Location;
    PdfPen pen1 = style.Borders.Left;
    if (pen1.IsImmutable)
      pen1 = new PdfPen(style.Borders.Left.Color, style.Borders.Left.Width);
    pen1.LineCap = PdfLineCap.Square;
    this.SetTransparency(ref graphics, pen1);
    graphics.DrawLine(pen1, point1_1, point2);
    graphics.Restore();
    point1_1 = new PointF(bounds.X + bounds.Width, bounds.Y);
    point2 = new PointF(bounds.X + bounds.Width, bounds.Y + bounds.Height);
    PdfPen pen2 = style.Borders.Right;
    if ((double) bounds.X + (double) bounds.Width > (double) graphics.ClientSize.Width - (double) pen2.Width / 2.0)
    {
      point1_1 = new PointF(graphics.ClientSize.Width - pen2.Width / 2f, bounds.Y);
      point2 = new PointF(graphics.ClientSize.Width - pen2.Width / 2f, bounds.Y + bounds.Height);
    }
    if (pen2.IsImmutable)
      pen2 = new PdfPen(style.Borders.Right.Color, style.Borders.Right.Width);
    pen2.LineCap = PdfLineCap.Square;
    this.SetTransparency(ref graphics, pen2);
    graphics.DrawLine(pen2, point1_1, point2);
    graphics.Restore();
    PointF point1_2 = bounds.Location;
    point2 = new PointF(bounds.X + bounds.Width, bounds.Y);
    PdfPen pen3 = style.Borders.Top;
    if (pen3.IsImmutable)
      pen3 = new PdfPen(style.Borders.Top.Color, style.Borders.Top.Width);
    pen3.LineCap = PdfLineCap.Square;
    this.SetTransparency(ref graphics, pen3);
    graphics.DrawLine(pen3, point1_2, point2);
    graphics.Restore();
    point1_2 = new PointF(bounds.X + bounds.Width, bounds.Y + bounds.Height);
    point2 = new PointF(bounds.X, bounds.Y + bounds.Height);
    PdfPen pen4 = style.Borders.Bottom;
    if ((double) bounds.Y + (double) bounds.Height > (double) graphics.ClientSize.Height - (double) pen4.Width / 2.0)
    {
      point1_2 = new PointF(bounds.X + bounds.Width, graphics.ClientSize.Height - pen4.Width / 2f);
      point2 = new PointF(bounds.X, graphics.ClientSize.Height - pen4.Width / 2f);
    }
    if (pen4.IsImmutable)
      pen4 = new PdfPen(style.Borders.Bottom.Color, style.Borders.Bottom.Width);
    pen4.LineCap = PdfLineCap.Square;
    this.SetTransparency(ref graphics, pen4);
    graphics.DrawLine(pen4, point1_2, point2);
    graphics.Restore();
  }

  private void SetTransparency(ref PdfGraphics graphics, PdfPen pen)
  {
    float alpha = (float) pen.Color.A / (float) byte.MaxValue;
    graphics.Save();
    graphics.SetTransparency(alpha);
  }

  private void ValidateSpanMap()
  {
    if (this.m_spanMap == null)
      return;
    int length = this.m_spanMap.Length;
    for (int index1 = 0; index1 < length; ++index1)
    {
      int span = this.m_spanMap[index1];
      if (span > 1)
      {
        int num = span + index1;
        int index2;
        for (index2 = index1 + 1; index2 < num && index2 < length; ++index2)
          this.m_spanMap[index2] = -1;
        index1 = index2 - 1;
      }
      else if (span < 0)
        throw new PdfLightTableException("Invalid span map.");
    }
  }

  private bool IsIncomplete(PdfStringLayoutResult[] results)
  {
    bool flag = false;
    if (results != null)
    {
      foreach (PdfStringLayoutResult result in results)
      {
        if (result.Remainder != null && result.Remainder != string.Empty)
        {
          flag = true;
          break;
        }
      }
    }
    else
      flag = true;
    return flag;
  }

  private float DetermineRowHeight(
    PdfLayoutParams param,
    int rowIndex,
    string[] row,
    RectangleF rowBouds,
    out PdfStringLayoutResult[] results,
    PdfCellStyle cs)
  {
    int length = row.Length;
    float height = 0.0f;
    if (this.m_currentPage != null)
      height = Math.Min(this.m_currentPageBounds.Height - rowBouds.Y, rowBouds.Height);
    SizeF size = new SizeF(this.m_cellWidths[0], height);
    float width = cs.BorderPen.Width;
    float cellPadding = this.Table.Style.CellPadding;
    bool overlapped = this.Table.Style.BorderOverlapStyle == PdfBorderOverlapStyle.Overlap;
    float rowHeight = 0.0f;
    size.Height = LightTableLayouter.ApplyBordersToHeight(size.Height, width, overlapped);
    if ((double) cellPadding > 0.0)
      size.Height = LightTableLayouter.ApplyBordersToHeight(size.Height, cellPadding, false);
    results = new PdfStringLayoutResult[length];
    PdfColumnCollection columns = this.Table.Columns;
    for (int index = 0; index < length; ++index)
    {
      PdfStringLayoutResult stringLayoutResult;
      if (this.m_spanMap != null && this.m_spanMap[index] < 0)
      {
        stringLayoutResult = new PdfStringLayoutResult();
        stringLayoutResult.m_actualSize = SizeF.Empty;
      }
      else
      {
        string text = row[index];
        size.Width = this.GetCellWidth(index);
        size.Width = LightTableLayouter.ApplyBordersToHeight(size.Width, width, overlapped);
        if ((double) cellPadding > 0.0)
          size.Width = LightTableLayouter.ApplyBordersToHeight(size.Width, cellPadding, false);
        if (text != null)
        {
          if (text.Equals(string.Empty))
            text = " ";
          if (this.m_previousRowIndex != rowIndex)
            text = PdfGraphics.NormalizeText(cs.Font, text);
        }
        else
          text = " ";
        PdfStringLayouter pdfStringLayouter = new PdfStringLayouter();
        PdfStringFormat format = columns[index].StringFormat ?? cs.StringFormat;
        if (this.Table.m_lightTableBuiltinStyle == PdfLightTableBuiltinStyle.PlainTable3 && (rowIndex == -1 && this.Table.m_headerStyle || rowIndex == this.Table.Rows.Count - 1 && this.Table.m_totalRowStyle) || (index == 0 && this.Table.m_firstColumnStyle || index == this.Table.Columns.Count - 1 && this.Table.m_lastColumnStyle) && rowIndex != -1)
          text = text.ToUpper();
        stringLayoutResult = pdfStringLayouter.Layout(text, cs.Font, format, size);
        bool flag = param.Format != null && param.Format.Break == PdfLayoutBreakType.FitElement;
        string remainder = stringLayoutResult.Remainder;
        if ((!this.Table.AllowRowBreakAcrossPages ? !string.IsNullOrEmpty(remainder) : flag & !string.IsNullOrEmpty(remainder)) && this.m_dropIndex != rowIndex)
        {
          this.DropToNextPage(results, length, row);
          this.m_dropIndex = rowIndex;
          rowHeight = 0.0f;
          break;
        }
        if ((double) size.Height > 0.0 || this.m_currentPage == null)
        {
          rowHeight = Math.Max(stringLayoutResult.ActualSize.Height, rowHeight);
        }
        else
        {
          stringLayoutResult = new PdfStringLayoutResult();
          stringLayoutResult.m_remainder = text;
          stringLayoutResult.m_actualSize = SizeF.Empty;
        }
      }
      results[index] = stringLayoutResult;
    }
    this.m_previousRowIndex = rowIndex;
    if ((double) rowHeight >= 0.0)
    {
      if (this.m_currentPage != null && (double) rowBouds.Height > 0.0)
        rowHeight = Math.Min(rowBouds.Height, rowHeight);
      if ((double) cellPadding > 0.0)
        rowHeight = LightTableLayouter.ApplyBordersToHeight(rowHeight, -cellPadding, false);
      rowHeight = LightTableLayouter.ApplyBordersToHeight(rowHeight, -width, overlapped);
    }
    return rowHeight;
  }

  private void DropToNextPage(PdfStringLayoutResult[] results, int count, string[] row)
  {
    for (int index = 0; index < count; ++index)
      results[index] = new PdfStringLayoutResult()
      {
        m_remainder = row[index],
        m_actualSize = SizeF.Empty
      };
  }

  private float GetCellWidth(int cellIndex)
  {
    float cellWidth = this.m_cellWidths[cellIndex];
    if (this.m_spanMap != null && this.m_spanMap.Length == this.m_cellWidths.Length)
    {
      int span = this.m_spanMap[cellIndex];
      if (span > 1)
      {
        int length = this.m_spanMap.Length;
        int num = span + cellIndex;
        float cellSpacing = this.Table.Style.CellSpacing;
        for (int index = cellIndex + 1; index < num && index < length; ++index)
        {
          cellWidth += this.m_cellWidths[index] + cellSpacing;
          this.m_spanMap[index] = -1;
        }
      }
    }
    return cellWidth;
  }

  private static float ApplyBordersToHeight(float height, float borderWidth, bool overlapped)
  {
    if (overlapped)
      height -= borderWidth;
    else
      height -= borderWidth * 2f;
    if ((double) height < 0.0)
      height = 0.0f;
    return height;
  }

  private PdfStringLayoutResult DrawCell(
    PdfStringLayoutResult layoutResult,
    RectangleF bounds,
    int rowIndex,
    int cellIndex,
    PdfCellStyle cs,
    bool ignoreColumnFormat)
  {
    PdfGraphics graphics = this.m_currentPage != null ? this.m_currentPage.Graphics : this.m_currentGraphics;
    bool flag = this.Table.Style.BorderOverlapStyle == PdfBorderOverlapStyle.Overlap;
    float cellPadding = this.Table.Style.CellPadding;
    PdfPen borderPen = cs.BorderPen;
    PdfBrush backgroundBrush = cs.BackgroundBrush;
    PdfBorders borders = cs.Borders;
    if (this.m_spanMap != null && this.m_spanMap[cellIndex] == -1)
      return new PdfStringLayoutResult();
    if (!flag)
      bounds = LightTableLayouter.PreserveForBorder(bounds, borderPen, PdfBorderOverlapStyle.Overlap);
    if (backgroundBrush != null)
    {
      float alpha = this.GetAlpha(backgroundBrush);
      graphics.Save();
      graphics.SetTransparency(alpha);
      graphics.DrawRectangle((PdfPen) null, backgroundBrush, bounds);
      graphics.Restore();
    }
    if (borders == null)
    {
      if (borderPen != null)
      {
        float alpha = (float) borderPen.Color.A / (float) byte.MaxValue;
        graphics.Save();
        graphics.SetTransparency(alpha);
        graphics.DrawRectangle(borderPen, (PdfBrush) null, bounds);
        graphics.Restore();
      }
    }
    else
      this.DrawBorder(bounds, graphics, cs);
    bounds = LightTableLayouter.PreserveForBorder(bounds, borderPen, PdfBorderOverlapStyle.Overlap);
    if ((double) cellPadding > 0.0)
    {
      bounds.X += cellPadding;
      bounds.Y += cellPadding;
      bounds.Width -= cellPadding * 2f;
      bounds.Height -= cellPadding * 2f;
    }
    if (!layoutResult.Empty)
    {
      PdfColumn column = this.Table.Columns[cellIndex];
      PdfStringFormat format = (ignoreColumnFormat ? cs.StringFormat : column.StringFormat) ?? cs.StringFormat;
      RectangleF layoutRectangle = bounds;
      RectangleF rectangleF = graphics.CheckCorrectLayoutRectangle(layoutResult.ActualSize, layoutRectangle.X, layoutRectangle.Y, format);
      if ((double) layoutRectangle.Width <= 0.0)
      {
        layoutRectangle.X = rectangleF.X;
        layoutRectangle.Width = rectangleF.Width;
      }
      if ((double) layoutRectangle.Height <= 0.0)
      {
        layoutRectangle.Y = rectangleF.Y;
        layoutRectangle.Height = rectangleF.Height;
      }
      graphics.DrawStringLayoutResult(layoutResult, cs.Font, cs.TextPen, cs.TextBrush, layoutRectangle, format);
    }
    return layoutResult;
  }

  private PdfCellStyle GetCellStyle(int rowIndex, bool isHeader, out bool hasOwnStyle)
  {
    PdfLightTableStyle style = this.Table.Style;
    hasOwnStyle = false;
    PdfCellStyle cellStyle;
    if (isHeader)
    {
      cellStyle = style.HeaderStyle;
      hasOwnStyle = true;
    }
    else
      cellStyle = (rowIndex & 1) <= 0 ? style.DefaultStyle : style.AlternateStyle;
    if (cellStyle == null)
    {
      cellStyle = style.DefaultStyle;
      hasOwnStyle = false;
    }
    return cellStyle;
  }

  private float[] GetWidths(RectangleF bounds)
  {
    int num1 = this.m_endColumn - this.m_startColumn + 1;
    PdfLightTableStyle style = this.Table.Style;
    PdfPen borderPen = style.BorderPen;
    float num2 = borderPen == null ? 0.0f : borderPen.Width;
    if (style.BorderOverlapStyle == PdfBorderOverlapStyle.Inside)
      num2 *= 2f;
    return this.Table.Columns.GetWidths(bounds.Width - style.CellSpacing * (float) (num1 + 1) - num2, this.m_startColumn, this.m_endColumn, this.Table.ColumnProportionalSizing);
  }

  private string[] GetRow(int startRowIndex, PdfLayoutParams param)
  {
    return this.m_row == null ? this.CropRow(this.Table.GetNextRow(ref startRowIndex)) : this.m_row;
  }

  private float GetAlpha(PdfBrush brush)
  {
    PdfSolidBrush pdfSolidBrush = brush as PdfSolidBrush;
    PdfLinearGradientBrush linearGradientBrush = brush as PdfLinearGradientBrush;
    float alpha = 1f;
    if (pdfSolidBrush != null)
      alpha = (float) pdfSolidBrush.Color.A / (float) byte.MaxValue;
    else if (linearGradientBrush != null)
    {
      PdfColor pdfColor1 = new PdfColor((byte) 0, (byte) 0, (byte) 0);
      PdfColor pdfColor2 = new PdfColor((byte) 0, (byte) 0, (byte) 0);
      PdfColor[] linearColors = linearGradientBrush.LinearColors;
      if (linearColors != null)
      {
        pdfColor1 = linearColors[0];
        pdfColor2 = linearColors[1];
      }
      if (pdfColor1.IsEmpty && pdfColor2.IsEmpty || pdfColor1.A == (byte) 0 && pdfColor2.A == (byte) 0)
        pdfColor1 = linearGradientBrush.InterpolationColors.Colors[0];
      alpha = (float) pdfColor1.A / (float) byte.MaxValue;
    }
    return alpha;
  }

  private bool RaiseBeforePageLayout(
    PdfPage currentPage,
    ref RectangleF currentBounds,
    ref int currentRow)
  {
    bool flag = false;
    if (this.Element.RaiseBeginPageLayout)
    {
      LightTableBeginPageLayoutEventArgs e = new LightTableBeginPageLayoutEventArgs(currentBounds, currentPage, currentRow);
      this.Element.OnBeginPageLayout((BeginPageLayoutEventArgs) e);
      flag = e.Cancel;
      currentBounds = e.Bounds;
      currentRow = e.StartRowIndex;
    }
    return flag;
  }

  private LightTableEndPageLayoutEventArgs RaisePageLayouted(
    LightTableLayouter.PageLayoutResult pageResult)
  {
    LightTableEndPageLayoutEventArgs e = (LightTableEndPageLayoutEventArgs) null;
    if (this.Element.RaiseEndPageLayout)
    {
      PdfLightTableLayoutResult layoutResult = this.GetLayoutResult(pageResult);
      int endRow = pageResult.LastRowIndex - 1;
      e = new LightTableEndPageLayoutEventArgs(layoutResult, pageResult.FirstRowIndex, endRow);
      this.Element.OnEndPageLayout((EndPageLayoutEventArgs) e);
    }
    return e;
  }

  private BeginRowLayoutEventArgs RaiseBeforeRowLayout(int rowIndex, PdfCellStyle cellStyle)
  {
    BeginRowLayoutEventArgs args = (BeginRowLayoutEventArgs) null;
    if (this.Table.RaiseBeginRowLayout)
    {
      args = new BeginRowLayoutEventArgs(rowIndex, cellStyle);
      this.Table.OnBeginRowLayout(args);
    }
    return args;
  }

  private bool RaiseAfterRowLayout(int rowIndex, bool isComplete, RectangleF rowBouds)
  {
    bool flag = false;
    if (this.Table.RaiseEndRowLayout)
    {
      EndRowLayoutEventArgs args = new EndRowLayoutEventArgs(rowIndex, isComplete, rowBouds);
      this.Table.OnEndRowLayout(args);
      flag = args.Cancel;
    }
    return flag;
  }

  private BeginCellLayoutEventArgs RaiseBeforeCellLayout(
    PdfGraphics graphics,
    int rowIndex,
    int cellIndex,
    RectangleF bounds,
    string value)
  {
    BeginCellLayoutEventArgs args = (BeginCellLayoutEventArgs) null;
    if (this.Table.RaiseBeginCellLayout)
    {
      args = new BeginCellLayoutEventArgs(graphics, rowIndex, cellIndex, bounds, value);
      this.Table.OnBeginCellLayout(args);
    }
    return args;
  }

  private void RaiseAfterCellLayout(
    PdfGraphics graphics,
    int rowIndex,
    int cellIndex,
    RectangleF bounds,
    string value)
  {
    if (!this.Table.RaiseEndCellLayout)
      return;
    this.Table.OnEndCellLayout(new EndCellLayoutEventArgs(graphics, rowIndex, cellIndex, bounds, value));
  }

  private void ApplyStyle(PdfLightTableBuiltinStyle tableStyle)
  {
    switch (tableStyle)
    {
      case PdfLightTableBuiltinStyle.PlainTable1:
        this.ApplyPlainTable1(Color.FromArgb((int) byte.MaxValue, 191, 191, 191), Color.FromArgb((int) byte.MaxValue, 242, 242, 242));
        break;
      case PdfLightTableBuiltinStyle.PlainTable2:
        this.ApplyPlainTable2(Color.FromArgb((int) byte.MaxValue, (int) sbyte.MaxValue, (int) sbyte.MaxValue, (int) sbyte.MaxValue));
        break;
      case PdfLightTableBuiltinStyle.PlainTable3:
        this.ApplyPlainTable3(Color.FromArgb((int) byte.MaxValue, (int) sbyte.MaxValue, (int) sbyte.MaxValue, (int) sbyte.MaxValue), Color.FromArgb((int) byte.MaxValue, 242, 242, 242));
        break;
      case PdfLightTableBuiltinStyle.PlainTable4:
        this.ApplyPlainTable4(Color.FromArgb((int) byte.MaxValue, 242, 242, 242));
        break;
      case PdfLightTableBuiltinStyle.PlainTable5:
        this.ApplyPlainTable5(Color.FromArgb((int) byte.MaxValue, (int) sbyte.MaxValue, (int) sbyte.MaxValue, (int) sbyte.MaxValue), Color.FromArgb((int) byte.MaxValue, 242, 242, 242));
        break;
      case PdfLightTableBuiltinStyle.GridTable1Light:
        this.ApplyGridTable1Light(Color.FromArgb((int) byte.MaxValue, 153, 153, 153), Color.FromArgb((int) byte.MaxValue, 102, 102, 102));
        break;
      case PdfLightTableBuiltinStyle.GridTable1LightAccent1:
        this.ApplyGridTable1Light(Color.FromArgb((int) byte.MaxValue, 189, 214, 238), Color.FromArgb((int) byte.MaxValue, 156, 194, 229));
        break;
      case PdfLightTableBuiltinStyle.GridTable1LightAccent2:
        this.ApplyGridTable1Light(Color.FromArgb((int) byte.MaxValue, 247, 202, 172), Color.FromArgb((int) byte.MaxValue, 244, 176 /*0xB0*/, 131));
        break;
      case PdfLightTableBuiltinStyle.GridTable1LightAccent3:
        this.ApplyGridTable1Light(Color.FromArgb((int) byte.MaxValue, 219, 219, 219), Color.FromArgb((int) byte.MaxValue, 201, 201, 201));
        break;
      case PdfLightTableBuiltinStyle.GridTable1LightAccent4:
        this.ApplyGridTable1Light(Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 229, 153), Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 217, 102));
        break;
      case PdfLightTableBuiltinStyle.GridTable1LightAccent5:
        this.ApplyGridTable1Light(Color.FromArgb((int) byte.MaxValue, 180, 198, 231), Color.FromArgb((int) byte.MaxValue, 142, 170, 219));
        break;
      case PdfLightTableBuiltinStyle.GridTable1LightAccent6:
        this.ApplyGridTable1Light(Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 224 /*0xE0*/, 179), Color.FromArgb((int) byte.MaxValue, 168, 208 /*0xD0*/, 141));
        break;
      case PdfLightTableBuiltinStyle.GridTable2:
        this.ApplyGridTable2(Color.FromArgb((int) byte.MaxValue, 102, 102, 102), Color.FromArgb((int) byte.MaxValue, 204, 204, 204));
        break;
      case PdfLightTableBuiltinStyle.GridTable2Accent1:
        this.ApplyGridTable2(Color.FromArgb((int) byte.MaxValue, 156, 194, 229), Color.FromArgb((int) byte.MaxValue, 222, 234, 246));
        break;
      case PdfLightTableBuiltinStyle.GridTable2Accent2:
        this.ApplyGridTable2(Color.FromArgb((int) byte.MaxValue, 244, 176 /*0xB0*/, 131), Color.FromArgb((int) byte.MaxValue, 251, 228, 213));
        break;
      case PdfLightTableBuiltinStyle.GridTable2Accent3:
        this.ApplyGridTable2(Color.FromArgb((int) byte.MaxValue, 201, 201, 201), Color.FromArgb((int) byte.MaxValue, 237, 237, 237));
        break;
      case PdfLightTableBuiltinStyle.GridTable2Accent4:
        this.ApplyGridTable2(Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 217, 102), Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 242, 204));
        break;
      case PdfLightTableBuiltinStyle.GridTable2Accent5:
        this.ApplyGridTable2(Color.FromArgb((int) byte.MaxValue, 142, 170, 219), Color.FromArgb((int) byte.MaxValue, 217, 226, 243));
        break;
      case PdfLightTableBuiltinStyle.GridTable2Accent6:
        this.ApplyGridTable2(Color.FromArgb((int) byte.MaxValue, 168, 208 /*0xD0*/, 141), Color.FromArgb((int) byte.MaxValue, 226, 239, 217));
        break;
      case PdfLightTableBuiltinStyle.GridTable3:
        this.ApplyGridTable3(Color.FromArgb((int) byte.MaxValue, 102, 102, 102), Color.FromArgb((int) byte.MaxValue, 204, 204, 204));
        break;
      case PdfLightTableBuiltinStyle.GridTable3Accent1:
        this.ApplyGridTable3(Color.FromArgb((int) byte.MaxValue, 156, 194, 229), Color.FromArgb((int) byte.MaxValue, 222, 234, 246));
        break;
      case PdfLightTableBuiltinStyle.GridTable3Accent2:
        this.ApplyGridTable3(Color.FromArgb((int) byte.MaxValue, 244, 176 /*0xB0*/, 131), Color.FromArgb((int) byte.MaxValue, 251, 228, 213));
        break;
      case PdfLightTableBuiltinStyle.GridTable3Accent3:
        this.ApplyGridTable3(Color.FromArgb((int) byte.MaxValue, 201, 201, 201), Color.FromArgb((int) byte.MaxValue, 237, 237, 237));
        break;
      case PdfLightTableBuiltinStyle.GridTable3Accent4:
        this.ApplyGridTable3(Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 217, 102), Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 242, 204));
        break;
      case PdfLightTableBuiltinStyle.GridTable3Accent5:
        this.ApplyGridTable3(Color.FromArgb((int) byte.MaxValue, 142, 170, 219), Color.FromArgb((int) byte.MaxValue, 217, 226, 243));
        break;
      case PdfLightTableBuiltinStyle.GridTable3Accent6:
        this.ApplyGridTable3(Color.FromArgb((int) byte.MaxValue, 168, 208 /*0xD0*/, 141), Color.FromArgb((int) byte.MaxValue, 226, 239, 217));
        break;
      case PdfLightTableBuiltinStyle.GridTable4:
        this.ApplyGridTable4(Color.FromArgb((int) byte.MaxValue, 102, 102, 102), Color.FromArgb((int) byte.MaxValue, 204, 204, 204), Color.FromArgb((int) byte.MaxValue, 0, 0, 0));
        break;
      case PdfLightTableBuiltinStyle.GridTable4Accent1:
        this.ApplyGridTable4(Color.FromArgb((int) byte.MaxValue, 156, 194, 229), Color.FromArgb((int) byte.MaxValue, 222, 234, 246), Color.FromArgb((int) byte.MaxValue, 91, 155, 213));
        break;
      case PdfLightTableBuiltinStyle.GridTable4Accent2:
        this.ApplyGridTable4(Color.FromArgb((int) byte.MaxValue, 244, 176 /*0xB0*/, 131), Color.FromArgb((int) byte.MaxValue, 251, 228, 213), Color.FromArgb((int) byte.MaxValue, 237, 125, 49));
        break;
      case PdfLightTableBuiltinStyle.GridTable4Accent3:
        this.ApplyGridTable4(Color.FromArgb((int) byte.MaxValue, 201, 201, 201), Color.FromArgb((int) byte.MaxValue, 237, 237, 237), Color.FromArgb((int) byte.MaxValue, 165, 165, 165));
        break;
      case PdfLightTableBuiltinStyle.GridTable4Accent4:
        this.ApplyGridTable4(Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 217, 102), Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 242, 204), Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192 /*0xC0*/, 0));
        break;
      case PdfLightTableBuiltinStyle.GridTable4Accent5:
        this.ApplyGridTable4(Color.FromArgb((int) byte.MaxValue, 142, 170, 219), Color.FromArgb((int) byte.MaxValue, 217, 226, 243), Color.FromArgb((int) byte.MaxValue, 68, 114, 196));
        break;
      case PdfLightTableBuiltinStyle.GridTable4Accent6:
        this.ApplyGridTable4(Color.FromArgb((int) byte.MaxValue, 168, 208 /*0xD0*/, 141), Color.FromArgb((int) byte.MaxValue, 226, 239, 217), Color.FromArgb((int) byte.MaxValue, 112 /*0x70*/, 173, 71));
        break;
      case PdfLightTableBuiltinStyle.GridTable5Dark:
        this.ApplyGridTable5Dark(Color.FromArgb((int) byte.MaxValue, 0, 0, 0), Color.FromArgb((int) byte.MaxValue, 153, 153, 153), Color.FromArgb((int) byte.MaxValue, 204, 204, 204));
        break;
      case PdfLightTableBuiltinStyle.GridTable5DarkAccent1:
        this.ApplyGridTable5Dark(Color.FromArgb((int) byte.MaxValue, 91, 155, 213), Color.FromArgb((int) byte.MaxValue, 189, 214, 238), Color.FromArgb((int) byte.MaxValue, 222, 234, 246));
        break;
      case PdfLightTableBuiltinStyle.GridTable5DarkAccent2:
        this.ApplyGridTable5Dark(Color.FromArgb((int) byte.MaxValue, 237, 125, 49), Color.FromArgb((int) byte.MaxValue, 247, 202, 172), Color.FromArgb((int) byte.MaxValue, 251, 228, 213));
        break;
      case PdfLightTableBuiltinStyle.GridTable5DarkAccent3:
        this.ApplyGridTable5Dark(Color.FromArgb((int) byte.MaxValue, 165, 165, 165), Color.FromArgb((int) byte.MaxValue, 219, 219, 219), Color.FromArgb((int) byte.MaxValue, 237, 237, 237));
        break;
      case PdfLightTableBuiltinStyle.GridTable5DarkAccent4:
        this.ApplyGridTable5Dark(Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192 /*0xC0*/, 0), Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 229, 153), Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 242, 204));
        break;
      case PdfLightTableBuiltinStyle.GridTable5DarkAccent5:
        this.ApplyGridTable5Dark(Color.FromArgb((int) byte.MaxValue, 68, 114, 196), Color.FromArgb((int) byte.MaxValue, 180, 198, 231), Color.FromArgb((int) byte.MaxValue, 217, 226, 243));
        break;
      case PdfLightTableBuiltinStyle.GridTable5DarkAccent6:
        this.ApplyGridTable5Dark(Color.FromArgb((int) byte.MaxValue, 112 /*0x70*/, 171, 71), Color.FromArgb((int) byte.MaxValue, 197, 224 /*0xE0*/, 179), Color.FromArgb((int) byte.MaxValue, 226, 239, 217));
        break;
      case PdfLightTableBuiltinStyle.GridTable6Colorful:
        this.ApplyGridTable6Colorful(Color.FromArgb((int) byte.MaxValue, 102, 102, 102), Color.FromArgb((int) byte.MaxValue, 204, 204, 204), Color.FromArgb((int) byte.MaxValue, 0, 0, 0));
        break;
      case PdfLightTableBuiltinStyle.GridTable6ColorfulAccent1:
        this.ApplyGridTable6Colorful(Color.FromArgb((int) byte.MaxValue, 156, 194, 229), Color.FromArgb((int) byte.MaxValue, 222, 234, 246), Color.FromArgb((int) byte.MaxValue, 46, 116, 181));
        break;
      case PdfLightTableBuiltinStyle.GridTable6ColorfulAccent2:
        this.ApplyGridTable6Colorful(Color.FromArgb((int) byte.MaxValue, 244, 176 /*0xB0*/, 131), Color.FromArgb((int) byte.MaxValue, 251, 228, 213), Color.FromArgb((int) byte.MaxValue, 196, 89, 17));
        break;
      case PdfLightTableBuiltinStyle.GridTable6ColorfulAccent3:
        this.ApplyGridTable6Colorful(Color.FromArgb((int) byte.MaxValue, 201, 201, 201), Color.FromArgb((int) byte.MaxValue, 237, 237, 237), Color.FromArgb((int) byte.MaxValue, 123, 123, 123));
        break;
      case PdfLightTableBuiltinStyle.GridTable6ColorfulAccent4:
        this.ApplyGridTable6Colorful(Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 217, 102), Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 242, 204), Color.FromArgb((int) byte.MaxValue, 191, 143, 0));
        break;
      case PdfLightTableBuiltinStyle.GridTable6ColorfulAccent5:
        this.ApplyGridTable6Colorful(Color.FromArgb((int) byte.MaxValue, 142, 170, 219), Color.FromArgb((int) byte.MaxValue, 217, 226, 243), Color.FromArgb((int) byte.MaxValue, 47, 84, 150));
        break;
      case PdfLightTableBuiltinStyle.GridTable6ColorfulAccent6:
        this.ApplyGridTable6Colorful(Color.FromArgb((int) byte.MaxValue, 168, 208 /*0xD0*/, 141), Color.FromArgb((int) byte.MaxValue, 226, 239, 217), Color.FromArgb((int) byte.MaxValue, 83, 129, 53));
        break;
      case PdfLightTableBuiltinStyle.GridTable7Colorful:
        this.ApplyGridTable7Colorful(Color.FromArgb((int) byte.MaxValue, 102, 102, 102), Color.FromArgb((int) byte.MaxValue, 204, 204, 204), Color.FromArgb((int) byte.MaxValue, 0, 0, 0));
        break;
      case PdfLightTableBuiltinStyle.GridTable7ColorfulAccent1:
        this.ApplyGridTable7Colorful(Color.FromArgb((int) byte.MaxValue, 156, 194, 229), Color.FromArgb((int) byte.MaxValue, 222, 234, 246), Color.FromArgb((int) byte.MaxValue, 46, 116, 181));
        break;
      case PdfLightTableBuiltinStyle.GridTable7ColorfulAccent2:
        this.ApplyGridTable7Colorful(Color.FromArgb((int) byte.MaxValue, 244, 176 /*0xB0*/, 131), Color.FromArgb((int) byte.MaxValue, 251, 228, 213), Color.FromArgb((int) byte.MaxValue, 196, 89, 17));
        break;
      case PdfLightTableBuiltinStyle.GridTable7ColorfulAccent3:
        this.ApplyGridTable7Colorful(Color.FromArgb((int) byte.MaxValue, 201, 201, 201), Color.FromArgb((int) byte.MaxValue, 237, 237, 237), Color.FromArgb((int) byte.MaxValue, 123, 123, 123));
        break;
      case PdfLightTableBuiltinStyle.GridTable7ColorfulAccent4:
        this.ApplyGridTable7Colorful(Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 217, 102), Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 242, 204), Color.FromArgb((int) byte.MaxValue, 191, 143, 0));
        break;
      case PdfLightTableBuiltinStyle.GridTable7ColorfulAccent5:
        this.ApplyGridTable7Colorful(Color.FromArgb((int) byte.MaxValue, 142, 170, 219), Color.FromArgb((int) byte.MaxValue, 217, 226, 243), Color.FromArgb((int) byte.MaxValue, 47, 84, 150));
        break;
      case PdfLightTableBuiltinStyle.GridTable7ColorfulAccent6:
        this.ApplyGridTable7Colorful(Color.FromArgb((int) byte.MaxValue, 168, 208 /*0xD0*/, 141), Color.FromArgb((int) byte.MaxValue, 226, 239, 217), Color.FromArgb((int) byte.MaxValue, 83, 129, 53));
        break;
      case PdfLightTableBuiltinStyle.ListTable1Light:
        this.ApplyListTable1Light(Color.FromArgb((int) byte.MaxValue, 102, 102, 102), Color.FromArgb((int) byte.MaxValue, 204, 204, 204));
        break;
      case PdfLightTableBuiltinStyle.ListTable1LightAccent1:
        this.ApplyListTable1Light(Color.FromArgb((int) byte.MaxValue, 156, 194, 229), Color.FromArgb((int) byte.MaxValue, 222, 234, 246));
        break;
      case PdfLightTableBuiltinStyle.ListTable1LightAccent2:
        this.ApplyListTable1Light(Color.FromArgb((int) byte.MaxValue, 244, 176 /*0xB0*/, 131), Color.FromArgb((int) byte.MaxValue, 251, 228, 213));
        break;
      case PdfLightTableBuiltinStyle.ListTable1LightAccent3:
        this.ApplyListTable1Light(Color.FromArgb((int) byte.MaxValue, 201, 201, 201), Color.FromArgb((int) byte.MaxValue, 237, 237, 237));
        break;
      case PdfLightTableBuiltinStyle.ListTable1LightAccent4:
        this.ApplyListTable1Light(Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 217, 102), Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 242, 204));
        break;
      case PdfLightTableBuiltinStyle.ListTable1LightAccent5:
        this.ApplyListTable1Light(Color.FromArgb((int) byte.MaxValue, 142, 170, 219), Color.FromArgb((int) byte.MaxValue, 217, 226, 243));
        break;
      case PdfLightTableBuiltinStyle.ListTable1LightAccent6:
        this.ApplyListTable1Light(Color.FromArgb((int) byte.MaxValue, 168, 208 /*0xD0*/, 141), Color.FromArgb((int) byte.MaxValue, 226, 239, 217));
        break;
      case PdfLightTableBuiltinStyle.ListTable2:
        this.ApplyListTable2(Color.FromArgb((int) byte.MaxValue, 102, 102, 102), Color.FromArgb((int) byte.MaxValue, 204, 204, 204));
        break;
      case PdfLightTableBuiltinStyle.ListTable2Accent1:
        this.ApplyListTable2(Color.FromArgb((int) byte.MaxValue, 156, 194, 229), Color.FromArgb((int) byte.MaxValue, 222, 234, 246));
        break;
      case PdfLightTableBuiltinStyle.ListTable2Accent2:
        this.ApplyListTable2(Color.FromArgb((int) byte.MaxValue, 244, 176 /*0xB0*/, 131), Color.FromArgb((int) byte.MaxValue, 251, 228, 213));
        break;
      case PdfLightTableBuiltinStyle.ListTable2Accent3:
        this.ApplyListTable2(Color.FromArgb((int) byte.MaxValue, 201, 201, 201), Color.FromArgb((int) byte.MaxValue, 237, 237, 237));
        break;
      case PdfLightTableBuiltinStyle.ListTable2Accent4:
        this.ApplyListTable2(Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 217, 102), Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 242, 204));
        break;
      case PdfLightTableBuiltinStyle.ListTable2Accent5:
        this.ApplyListTable2(Color.FromArgb((int) byte.MaxValue, 142, 170, 219), Color.FromArgb((int) byte.MaxValue, 217, 226, 243));
        break;
      case PdfLightTableBuiltinStyle.ListTable2Accent6:
        this.ApplyListTable2(Color.FromArgb((int) byte.MaxValue, 168, 208 /*0xD0*/, 141), Color.FromArgb((int) byte.MaxValue, 226, 239, 217));
        break;
      case PdfLightTableBuiltinStyle.ListTable3:
        this.ApplyListTable3(Color.FromArgb((int) byte.MaxValue, 0, 0, 0));
        break;
      case PdfLightTableBuiltinStyle.ListTable3Accent1:
        this.ApplyListTable3(Color.FromArgb((int) byte.MaxValue, 91, 155, 213));
        break;
      case PdfLightTableBuiltinStyle.ListTable3Accent2:
        this.ApplyListTable3(Color.FromArgb((int) byte.MaxValue, 237, 125, 49));
        break;
      case PdfLightTableBuiltinStyle.ListTable3Accent3:
        this.ApplyListTable3(Color.FromArgb((int) byte.MaxValue, 165, 165, 165));
        break;
      case PdfLightTableBuiltinStyle.ListTable3Accent4:
        this.ApplyListTable3(Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192 /*0xC0*/, 0));
        break;
      case PdfLightTableBuiltinStyle.ListTable3Accent5:
        this.ApplyListTable3(Color.FromArgb((int) byte.MaxValue, 68, 114, 196));
        break;
      case PdfLightTableBuiltinStyle.ListTable3Accent6:
        this.ApplyListTable3(Color.FromArgb((int) byte.MaxValue, 112 /*0x70*/, 171, 71));
        break;
      case PdfLightTableBuiltinStyle.ListTable4:
        this.ApplyListTable4(Color.FromArgb((int) byte.MaxValue, 102, 102, 102), Color.FromArgb((int) byte.MaxValue, 0, 0, 0), Color.FromArgb((int) byte.MaxValue, 204, 204, 204));
        break;
      case PdfLightTableBuiltinStyle.ListTable4Accent1:
        this.ApplyListTable4(Color.FromArgb((int) byte.MaxValue, 156, 194, 229), Color.FromArgb((int) byte.MaxValue, 91, 155, 213), Color.FromArgb((int) byte.MaxValue, 222, 234, 246));
        break;
      case PdfLightTableBuiltinStyle.ListTable4Accent2:
        this.ApplyListTable4(Color.FromArgb((int) byte.MaxValue, 244, 176 /*0xB0*/, 131), Color.FromArgb((int) byte.MaxValue, 237, 125, 49), Color.FromArgb((int) byte.MaxValue, 251, 228, 213));
        break;
      case PdfLightTableBuiltinStyle.ListTable4Accent3:
        this.ApplyListTable4(Color.FromArgb((int) byte.MaxValue, 201, 201, 201), Color.FromArgb((int) byte.MaxValue, 165, 165, 165), Color.FromArgb((int) byte.MaxValue, 237, 237, 237));
        break;
      case PdfLightTableBuiltinStyle.ListTable4Accent4:
        this.ApplyListTable4(Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 217, 102), Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192 /*0xC0*/, 0), Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 242, 204));
        break;
      case PdfLightTableBuiltinStyle.ListTable4Accent5:
        this.ApplyListTable4(Color.FromArgb((int) byte.MaxValue, 142, 170, 219), Color.FromArgb((int) byte.MaxValue, 68, 114, 196), Color.FromArgb((int) byte.MaxValue, 217, 226, 243));
        break;
      case PdfLightTableBuiltinStyle.ListTable4Accent6:
        this.ApplyListTable4(Color.FromArgb((int) byte.MaxValue, 168, 208 /*0xD0*/, 141), Color.FromArgb((int) byte.MaxValue, 112 /*0x70*/, 173, 71), Color.FromArgb((int) byte.MaxValue, 226, 239, 217));
        break;
      case PdfLightTableBuiltinStyle.ListTable5Dark:
        this.ApplyListTable5Dark(Color.FromArgb((int) byte.MaxValue, 0, 0, 0));
        break;
      case PdfLightTableBuiltinStyle.ListTable5DarkAccent1:
        this.ApplyListTable5Dark(Color.FromArgb((int) byte.MaxValue, 91, 155, 213));
        break;
      case PdfLightTableBuiltinStyle.ListTable5DarkAccent2:
        this.ApplyListTable5Dark(Color.FromArgb((int) byte.MaxValue, 237, 125, 49));
        break;
      case PdfLightTableBuiltinStyle.ListTable5DarkAccent3:
        this.ApplyListTable5Dark(Color.FromArgb((int) byte.MaxValue, 165, 165, 165));
        break;
      case PdfLightTableBuiltinStyle.ListTable5DarkAccent4:
        this.ApplyListTable5Dark(Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192 /*0xC0*/, 0));
        break;
      case PdfLightTableBuiltinStyle.ListTable5DarkAccent5:
        this.ApplyListTable5Dark(Color.FromArgb((int) byte.MaxValue, 68, 114, 196));
        break;
      case PdfLightTableBuiltinStyle.ListTable5DarkAccent6:
        this.ApplyListTable5Dark(Color.FromArgb((int) byte.MaxValue, 112 /*0x70*/, 173, 71));
        break;
      case PdfLightTableBuiltinStyle.ListTable6Colorful:
        this.ApplyListTable6Colorful(Color.FromArgb((int) byte.MaxValue, 102, 102, 102), Color.FromArgb((int) byte.MaxValue, 204, 204, 204), Color.FromArgb((int) byte.MaxValue, 0, 0, 0));
        break;
      case PdfLightTableBuiltinStyle.ListTable6ColorfulAccent1:
        this.ApplyListTable6Colorful(Color.FromArgb((int) byte.MaxValue, 91, 155, 213), Color.FromArgb((int) byte.MaxValue, 222, 234, 246), Color.FromArgb((int) byte.MaxValue, 46, 116, 181));
        break;
      case PdfLightTableBuiltinStyle.ListTable6ColorfulAccent2:
        this.ApplyListTable6Colorful(Color.FromArgb((int) byte.MaxValue, 237, 125, 49), Color.FromArgb((int) byte.MaxValue, 251, 228, 213), Color.FromArgb((int) byte.MaxValue, 196, 89, 17));
        break;
      case PdfLightTableBuiltinStyle.ListTable6ColorfulAccent3:
        this.ApplyListTable6Colorful(Color.FromArgb((int) byte.MaxValue, 165, 165, 165), Color.FromArgb((int) byte.MaxValue, 237, 237, 237), Color.FromArgb((int) byte.MaxValue, 123, 123, 123));
        break;
      case PdfLightTableBuiltinStyle.ListTable6ColorfulAccent4:
        this.ApplyListTable6Colorful(Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192 /*0xC0*/, 0), Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 242, 204), Color.FromArgb((int) byte.MaxValue, 191, 143, 0));
        break;
      case PdfLightTableBuiltinStyle.ListTable6ColorfulAccent5:
        this.ApplyListTable6Colorful(Color.FromArgb((int) byte.MaxValue, 68, 114, 196), Color.FromArgb((int) byte.MaxValue, 217, 226, 243), Color.FromArgb((int) byte.MaxValue, 47, 84, 150));
        break;
      case PdfLightTableBuiltinStyle.ListTable6ColorfulAccent6:
        this.ApplyListTable6Colorful(Color.FromArgb((int) byte.MaxValue, 112 /*0x70*/, 173, 71), Color.FromArgb((int) byte.MaxValue, 226, 239, 217), Color.FromArgb((int) byte.MaxValue, 83, 129, 53));
        break;
      case PdfLightTableBuiltinStyle.ListTable7Colorful:
        this.ApplyListTable7Colorful(Color.FromArgb((int) byte.MaxValue, 102, 102, 102), Color.FromArgb((int) byte.MaxValue, 204, 204, 204), Color.FromArgb((int) byte.MaxValue, 0, 0, 0));
        break;
      case PdfLightTableBuiltinStyle.ListTable7ColorfulAccent1:
        this.ApplyListTable7Colorful(Color.FromArgb((int) byte.MaxValue, 91, 155, 213), Color.FromArgb((int) byte.MaxValue, 222, 234, 246), Color.FromArgb((int) byte.MaxValue, 46, 116, 181));
        break;
      case PdfLightTableBuiltinStyle.ListTable7ColorfulAccent2:
        this.ApplyListTable7Colorful(Color.FromArgb((int) byte.MaxValue, 237, 125, 49), Color.FromArgb((int) byte.MaxValue, 251, 228, 213), Color.FromArgb((int) byte.MaxValue, 196, 89, 17));
        break;
      case PdfLightTableBuiltinStyle.ListTable7ColorfulAccent3:
        this.ApplyListTable7Colorful(Color.FromArgb((int) byte.MaxValue, 165, 165, 165), Color.FromArgb((int) byte.MaxValue, 237, 237, 237), Color.FromArgb((int) byte.MaxValue, 123, 123, 123));
        break;
      case PdfLightTableBuiltinStyle.ListTable7ColorfulAccent4:
        this.ApplyListTable7Colorful(Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192 /*0xC0*/, 0), Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 242, 204), Color.FromArgb((int) byte.MaxValue, 191, 143, 0));
        break;
      case PdfLightTableBuiltinStyle.ListTable7ColorfulAccent5:
        this.ApplyListTable7Colorful(Color.FromArgb((int) byte.MaxValue, 68, 114, 196), Color.FromArgb((int) byte.MaxValue, 217, 226, 243), Color.FromArgb((int) byte.MaxValue, 47, 84, 150));
        break;
      case PdfLightTableBuiltinStyle.ListTable7ColorfulAccent6:
        this.ApplyListTable7Colorful(Color.FromArgb((int) byte.MaxValue, 112 /*0x70*/, 173, 71), Color.FromArgb((int) byte.MaxValue, 226, 239, 217), Color.FromArgb((int) byte.MaxValue, 83, 129, 53));
        break;
      case PdfLightTableBuiltinStyle.TableGridLight:
        this.ApplyTableGridLight(Color.FromArgb((int) byte.MaxValue, 191, 191, 191));
        break;
    }
  }

  private PdfFont ChangeFontStyle(PdfFont font)
  {
    PdfFont pdfFont = (PdfFont) null;
    if (font.Style == PdfFontStyle.Regular)
      pdfFont = this.CreateBoldFont(font);
    else if (font.Style == PdfFontStyle.Bold)
      pdfFont = this.CreateRegularFont(font);
    return pdfFont;
  }

  private PdfBrush ApplyBandedColStyle(bool firstColumn, Color backColor, int cellIndex)
  {
    PdfBrush pdfBrush = (PdfBrush) null;
    if (!firstColumn)
    {
      if (cellIndex % 2 == 0)
        pdfBrush = (PdfBrush) new PdfSolidBrush((PdfColor) backColor);
    }
    else if (cellIndex % 2 != 0)
      pdfBrush = (PdfBrush) new PdfSolidBrush((PdfColor) backColor);
    return pdfBrush;
  }

  private PdfBrush ApplyBandedRowStyle(bool headerRow, Color backColor, int rowIndex)
  {
    PdfBrush pdfBrush = (PdfBrush) null;
    if (!headerRow)
    {
      if (rowIndex % 2 != 0)
        pdfBrush = (PdfBrush) new PdfSolidBrush((PdfColor) backColor);
    }
    else if (rowIndex % 2 == 0)
      pdfBrush = (PdfBrush) new PdfSolidBrush((PdfColor) backColor);
    return pdfBrush;
  }

  private void ApplyTableGridLight(Color borderColor)
  {
    PdfCellStyle pdfCellStyle = new PdfCellStyle();
    pdfCellStyle.Borders = PdfBorders.Default;
    this.Table.Style.DefaultStyle = pdfCellStyle;
    PdfPen pdfPen = new PdfPen(borderColor, 0.5f);
    pdfCellStyle.Borders.All = pdfPen;
  }

  private void ApplyPlainTable1(Color borderColor, Color backColor)
  {
    PdfCellStyle pdfCellStyle = new PdfCellStyle();
    if (this.m_currentRowIndex < 0 && this.Table.m_headerStyle && (this.m_currentCellIndex != 0 || !this.Table.m_firstColumnStyle) && (this.m_currentCellIndex != this.Table.Columns.Count - 1 || !this.Table.m_lastColumnStyle))
      pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
    pdfCellStyle.Borders = PdfBorders.Default;
    this.Table.Style.DefaultStyle = pdfCellStyle;
    PdfPen pdfPen = new PdfPen(borderColor, 0.5f);
    pdfCellStyle.BorderPen = pdfPen;
    pdfCellStyle.Borders.All = pdfPen;
    PdfSolidBrush pdfSolidBrush = new PdfSolidBrush((PdfColor) backColor);
    if (this.Table.m_bandedColStyle && this.Table.m_bandedRowStyle)
    {
      if (!this.Table.m_lastColumnStyle || this.m_currentCellIndex != this.Table.Columns.Count - 1)
        pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, backColor, this.m_currentCellIndex);
      if (pdfCellStyle.BackgroundBrush == null)
        pdfCellStyle.BackgroundBrush = this.ApplyBandedRowStyle(this.Table.m_headerStyle, backColor, this.m_currentRowIndex);
    }
    else
    {
      if (this.Table.m_bandedColStyle && (!this.Table.m_lastColumnStyle || this.m_currentCellIndex != this.Table.Columns.Count - 1))
        pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, backColor, this.m_currentCellIndex);
      if (this.Table.m_bandedRowStyle)
        pdfCellStyle.BackgroundBrush = this.m_currentRowIndex >= 0 || this.Table.m_headerStyle ? this.ApplyBandedRowStyle(this.Table.m_headerStyle, backColor, this.m_currentRowIndex) : (PdfBrush) pdfSolidBrush;
    }
    if (this.m_currentRowIndex == this.Table.Rows.Count - 1 && this.Table.m_totalRowStyle)
    {
      pdfCellStyle.BackgroundBrush = (PdfBrush) null;
      if (this.Table.m_bandedColStyle && (!this.Table.m_lastColumnStyle || this.m_currentCellIndex != this.Table.Columns.Count - 1))
        pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, backColor, this.m_currentCellIndex);
      pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
      pdfCellStyle.Borders.Top = new PdfPen(borderColor);
    }
    if (this.Table.m_firstColumnStyle && this.m_currentCellIndex == 0 && (!this.Table.m_totalRowStyle || this.m_currentRowIndex != this.Table.Rows.Count - 1))
      pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
    if (!this.Table.m_lastColumnStyle || this.m_currentCellIndex != this.Table.Columns.Count - 1 || this.Table.m_totalRowStyle && this.m_currentRowIndex == this.Table.Rows.Count - 1)
      return;
    pdfCellStyle.BackgroundBrush = (PdfBrush) null;
    if (this.Table.m_bandedRowStyle)
      pdfCellStyle.BackgroundBrush = this.ApplyBandedRowStyle(this.Table.m_headerStyle, backColor, this.m_currentRowIndex);
    pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
  }

  private void ApplyPlainTable2(Color borderColor)
  {
    PdfCellStyle pdfCellStyle = new PdfCellStyle();
    pdfCellStyle.Borders = PdfBorders.Default;
    this.Table.Style.DefaultStyle = pdfCellStyle;
    PdfPen pdfPen1 = new PdfPen(borderColor, 0.5f);
    PdfPen pdfPen2 = new PdfPen(Color.Empty);
    pdfCellStyle.Borders.All = pdfPen2;
    pdfCellStyle.Borders.Top = pdfPen1;
    if (this.m_currentRowIndex < 0 && this.Table.m_headerStyle && (this.m_currentCellIndex != 0 || !this.Table.m_firstColumnStyle) && (this.m_currentCellIndex != this.Table.Columns.Count - 1 || !this.Table.m_lastColumnStyle))
    {
      pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
      pdfCellStyle.Borders.All = pdfPen1;
      pdfCellStyle.Borders.Left = pdfPen2;
      pdfCellStyle.Borders.Right = pdfPen2;
      if (this.Table.m_bandedColStyle)
      {
        pdfCellStyle.Borders.Left = pdfPen1;
        pdfCellStyle.Borders.Right = pdfPen1;
      }
      if (this.Table.m_firstColumnStyle && this.m_currentCellIndex == 0)
        pdfCellStyle.Borders.Left = pdfPen2;
      if (this.Table.m_lastColumnStyle && this.m_currentCellIndex == this.Table.Columns.Count - 1)
        pdfCellStyle.Borders.Right = pdfPen2;
    }
    pdfCellStyle.Borders.All = pdfPen2;
    if (this.Table.m_bandedRowStyle)
    {
      pdfCellStyle.Borders.Top = pdfPen1;
      pdfCellStyle.Borders.Bottom = pdfPen1;
    }
    if (this.Table.m_bandedColStyle)
    {
      pdfCellStyle.Borders.Left = pdfPen1;
      pdfCellStyle.Borders.Right = pdfPen1;
    }
    if (this.m_currentRowIndex == this.Table.Rows.Count - 1)
    {
      pdfCellStyle.Borders.Bottom = pdfPen1;
      if (this.Table.m_totalRowStyle)
      {
        pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
        pdfCellStyle.Borders.Top = pdfPen1;
      }
    }
    if (this.Table.m_firstColumnStyle && this.m_currentCellIndex == 0)
    {
      if (!this.Table.m_totalRowStyle || this.m_currentRowIndex != this.Table.Rows.Count - 1)
        pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
      pdfCellStyle.Borders.Left = pdfPen2;
    }
    if (!this.Table.m_lastColumnStyle || this.m_currentCellIndex != this.Table.Columns.Count - 1)
      return;
    if (!this.Table.m_totalRowStyle || this.m_currentRowIndex != this.Table.Rows.Count - 1)
      pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
    pdfCellStyle.Borders.Right = pdfPen2;
  }

  private void ApplyPlainTable3(Color borderColor, Color backColor)
  {
    PdfCellStyle pdfCellStyle = new PdfCellStyle();
    pdfCellStyle.Borders = PdfBorders.Default;
    this.Table.Style.DefaultStyle = pdfCellStyle;
    PdfPen pdfPen1 = new PdfPen(borderColor, 0.5f);
    PdfPen pdfPen2 = new PdfPen(Color.Empty, 0.5f);
    PdfBrush pdfBrush = (PdfBrush) new PdfSolidBrush((PdfColor) backColor);
    PdfPen pdfPen3 = new PdfPen(backColor, 0.5f);
    pdfCellStyle.Borders.All = pdfPen2;
    if (this.m_currentRowIndex < 0)
    {
      if (this.Table.m_bandedColStyle)
      {
        pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, backColor, this.m_currentCellIndex);
        if (pdfCellStyle.BackgroundBrush != null)
          pdfCellStyle.Borders.All = pdfPen3;
      }
      if (this.Table.m_headerStyle)
      {
        if (this.Table.m_headerStyle && (this.m_currentCellIndex != 0 || !this.Table.m_firstColumnStyle) && (this.m_currentCellIndex != this.Table.Columns.Count - 1 || !this.Table.m_lastColumnStyle))
          pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
        pdfCellStyle.Borders.Bottom = pdfPen1;
      }
    }
    if (this.Table.m_bandedColStyle && this.Table.m_bandedRowStyle)
    {
      pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, backColor, this.m_currentCellIndex);
      if (pdfCellStyle.BackgroundBrush == null)
        pdfCellStyle.BackgroundBrush = this.ApplyBandedRowStyle(this.Table.m_headerStyle, backColor, this.m_currentRowIndex);
      if (this.Table.m_firstColumnStyle && this.m_currentCellIndex == 1)
        pdfCellStyle.Borders.Left = pdfPen1;
      else
        pdfCellStyle.Borders.All = pdfPen2;
      if (this.Table.m_headerStyle && this.m_currentRowIndex == 0)
        pdfCellStyle.Borders.Top = pdfPen1;
    }
    else
    {
      if (this.Table.m_bandedColStyle)
      {
        pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, backColor, this.m_currentCellIndex);
        if (pdfCellStyle.BackgroundBrush != null)
        {
          if (this.Table.m_firstColumnStyle && this.m_currentCellIndex == 1)
            pdfCellStyle.Borders.Left = pdfPen1;
          else
            pdfCellStyle.Borders.All = pdfPen2;
          if (this.Table.m_headerStyle && this.m_currentRowIndex == 0)
            pdfCellStyle.Borders.Top = pdfPen1;
        }
      }
      if (this.Table.m_bandedRowStyle)
      {
        pdfCellStyle.BackgroundBrush = this.m_currentRowIndex >= 0 || this.Table.m_headerStyle ? this.ApplyBandedRowStyle(this.Table.m_headerStyle, backColor, this.m_currentRowIndex) : pdfBrush;
        if (this.Table.m_firstColumnStyle && this.m_currentCellIndex == 1)
          pdfCellStyle.Borders.Left = pdfPen1;
        else
          pdfCellStyle.Borders.All = pdfPen2;
        if (this.Table.m_headerStyle && this.m_currentRowIndex == 0)
          pdfCellStyle.Borders.Top = pdfPen1;
      }
    }
    if (this.m_currentRowIndex == this.Table.Rows.Count - 1 && this.Table.m_totalRowStyle)
    {
      pdfCellStyle.BackgroundBrush = (PdfBrush) null;
      pdfCellStyle.Borders.All = pdfPen2;
      if (this.Table.m_bandedColStyle)
      {
        if (!this.Table.m_lastColumnStyle || this.m_currentCellIndex != this.Table.Columns.Count - 1)
          pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, backColor, this.m_currentCellIndex);
        if (pdfCellStyle.BackgroundBrush != null)
        {
          if (this.Table.m_firstColumnStyle && this.m_currentCellIndex == 1)
            pdfCellStyle.Borders.Left = pdfPen1;
          else
            pdfCellStyle.Borders.All = pdfPen3;
        }
      }
      pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
    }
    if (this.Table.m_firstColumnStyle && this.m_currentCellIndex == 0)
    {
      if (!this.Table.m_totalRowStyle || this.m_currentRowIndex != this.Table.Rows.Count - 1)
        pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
      pdfCellStyle.Borders.Right = pdfPen1;
    }
    if (!this.Table.m_lastColumnStyle || this.m_currentCellIndex != this.Table.Columns.Count - 1 || this.Table.m_totalRowStyle && this.m_currentRowIndex == this.Table.Rows.Count - 1)
      return;
    pdfCellStyle.BackgroundBrush = (PdfBrush) null;
    pdfCellStyle.Borders.All = pdfPen2;
    pdfCellStyle.BackgroundBrush = this.m_currentRowIndex >= 0 || this.Table.m_headerStyle ? this.ApplyBandedRowStyle(this.Table.m_headerStyle, backColor, this.m_currentRowIndex) : pdfBrush;
    if (pdfCellStyle.BackgroundBrush != null)
      pdfCellStyle.Borders.All = pdfPen3;
    pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
    if (this.m_currentRowIndex != 0 || !this.Table.m_headerStyle)
      return;
    pdfCellStyle.Borders.Top = pdfPen1;
  }

  private void ApplyPlainTable4(Color backColor)
  {
    PdfCellStyle pdfCellStyle = new PdfCellStyle();
    pdfCellStyle.Borders = PdfBorders.Default;
    this.Table.Style.DefaultStyle = pdfCellStyle;
    PdfBrush pdfBrush = (PdfBrush) new PdfSolidBrush((PdfColor) backColor);
    PdfPen pdfPen1 = new PdfPen(backColor, 0.5f);
    PdfPen pdfPen2 = new PdfPen(Color.Empty);
    pdfCellStyle.Borders.All = pdfPen2;
    if (this.m_currentRowIndex < 0 && this.Table.m_headerStyle)
    {
      if (this.Table.m_headerStyle && (this.m_currentCellIndex != 0 || !this.Table.m_firstColumnStyle) && (this.m_currentCellIndex != this.Table.Columns.Count - 1 || !this.Table.m_lastColumnStyle))
        pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
      if (this.Table.m_bandedColStyle)
      {
        pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, backColor, this.m_currentCellIndex);
        if (pdfCellStyle.BackgroundBrush != null)
          pdfCellStyle.Borders.All = pdfPen1;
      }
    }
    if (this.Table.m_bandedColStyle && this.Table.m_bandedRowStyle)
    {
      pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, backColor, this.m_currentCellIndex);
      if (pdfCellStyle.BackgroundBrush == null)
        pdfCellStyle.BackgroundBrush = this.ApplyBandedRowStyle(this.Table.m_headerStyle, backColor, this.m_currentRowIndex);
      if (pdfCellStyle.BackgroundBrush != null)
        pdfCellStyle.Borders.All = pdfPen1;
    }
    else
    {
      if (this.Table.m_bandedColStyle)
      {
        pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, backColor, this.m_currentCellIndex);
        if (pdfCellStyle.BackgroundBrush != null)
          pdfCellStyle.Borders.All = pdfPen1;
      }
      if (this.Table.m_bandedRowStyle)
      {
        pdfCellStyle.BackgroundBrush = this.m_currentRowIndex >= 0 || this.Table.m_headerStyle ? this.ApplyBandedRowStyle(this.Table.m_headerStyle, backColor, this.m_currentRowIndex) : pdfBrush;
        if (pdfCellStyle.BackgroundBrush != null)
          pdfCellStyle.Borders.All = pdfPen1;
      }
    }
    if (this.Table.m_totalRowStyle && this.m_currentRowIndex == this.Table.Rows.Count - 1)
    {
      pdfCellStyle.BackgroundBrush = (PdfBrush) null;
      pdfCellStyle.Borders.All = pdfPen2;
      if (this.Table.m_bandedColStyle)
      {
        if (!this.Table.m_lastColumnStyle || this.m_currentCellIndex != this.Table.Columns.Count - 1)
          pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, backColor, this.m_currentCellIndex);
        if (pdfCellStyle.BackgroundBrush != null)
          pdfCellStyle.Borders.All = pdfPen1;
      }
      pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
    }
    if (this.Table.m_firstColumnStyle && this.m_currentCellIndex == 0 && (!this.Table.m_totalRowStyle || this.m_currentRowIndex != this.Table.Rows.Count - 1))
      pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
    if (!this.Table.m_lastColumnStyle || this.m_currentCellIndex != this.Table.Columns.Count - 1 || this.Table.m_totalRowStyle && this.m_currentRowIndex == this.Table.Rows.Count - 1)
      return;
    pdfCellStyle.BackgroundBrush = (PdfBrush) null;
    pdfCellStyle.Borders.All = pdfPen2;
    if (this.Table.m_bandedRowStyle)
    {
      pdfCellStyle.BackgroundBrush = this.m_currentRowIndex >= 0 || this.Table.m_headerStyle ? this.ApplyBandedRowStyle(this.Table.m_headerStyle, backColor, this.m_currentRowIndex) : pdfBrush;
      if (pdfCellStyle.BackgroundBrush != null)
        pdfCellStyle.Borders.All = pdfPen1;
    }
    pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
  }

  private void ApplyPlainTable5(Color borderColor, Color backColor)
  {
    PdfCellStyle pdfCellStyle = new PdfCellStyle();
    pdfCellStyle.Borders = PdfBorders.Default;
    this.Table.Style.DefaultStyle = pdfCellStyle;
    PdfBrush pdfBrush = (PdfBrush) new PdfSolidBrush((PdfColor) backColor);
    PdfPen pdfPen1 = new PdfPen(backColor, 0.5f);
    PdfPen pdfPen2 = new PdfPen(borderColor, 0.5f);
    PdfPen pdfPen3 = new PdfPen(Color.Empty);
    pdfCellStyle.Borders.All = pdfPen3;
    if (this.m_currentRowIndex < 0 && this.Table.m_headerStyle)
    {
      if (this.Table.m_headerStyle && (this.m_currentCellIndex != 0 || !this.Table.m_firstColumnStyle) && (this.m_currentCellIndex != this.Table.Columns.Count - 1 || !this.Table.m_lastColumnStyle))
        pdfCellStyle.Font = this.CreateItalicFont(this.Table.Style.DefaultStyle.Font);
      pdfCellStyle.Borders.Bottom = pdfPen2;
    }
    if (this.Table.m_bandedColStyle && this.Table.m_bandedRowStyle)
    {
      if (!this.Table.m_headerStyle || this.m_currentRowIndex >= 0)
        pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, backColor, this.m_currentCellIndex);
      if (pdfCellStyle.BackgroundBrush == null)
        pdfCellStyle.BackgroundBrush = this.ApplyBandedRowStyle(this.Table.m_headerStyle, backColor, this.m_currentRowIndex);
      if (pdfCellStyle.BackgroundBrush != null)
      {
        if (this.Table.m_firstColumnStyle && this.m_currentCellIndex == 1)
          pdfCellStyle.Borders.Left = pdfPen2;
        else
          pdfCellStyle.Borders.All = pdfPen3;
        if (this.Table.m_headerStyle && this.m_currentRowIndex == 0)
          pdfCellStyle.Borders.Top = pdfPen2;
      }
    }
    else
    {
      if (this.Table.m_bandedColStyle)
      {
        if (!this.Table.m_headerStyle || this.m_currentRowIndex >= 0)
          pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, backColor, this.m_currentCellIndex);
        if (pdfCellStyle.BackgroundBrush != null)
        {
          if (this.Table.m_firstColumnStyle && this.m_currentCellIndex == 1)
            pdfCellStyle.Borders.Left = pdfPen2;
          else
            pdfCellStyle.Borders.All = pdfPen3;
          if (this.Table.m_headerStyle && this.m_currentRowIndex == 0)
            pdfCellStyle.Borders.Top = pdfPen2;
        }
      }
      if (this.Table.m_bandedRowStyle)
      {
        pdfCellStyle.BackgroundBrush = this.m_currentRowIndex >= 0 || this.Table.m_headerStyle ? this.ApplyBandedRowStyle(this.Table.m_headerStyle, backColor, this.m_currentRowIndex) : pdfBrush;
        if (pdfCellStyle.BackgroundBrush != null)
        {
          if (this.Table.m_firstColumnStyle && this.m_currentCellIndex == 1)
            pdfCellStyle.Borders.Left = pdfPen2;
          else
            pdfCellStyle.Borders.All = pdfPen3;
          if (this.Table.m_headerStyle && this.m_currentRowIndex == 0)
            pdfCellStyle.Borders.Top = pdfPen2;
        }
      }
    }
    if (this.Table.m_totalRowStyle && this.m_currentRowIndex == this.Table.Rows.Count - 1)
    {
      pdfCellStyle.BackgroundBrush = (PdfBrush) null;
      pdfCellStyle.Borders.All = pdfPen3;
      pdfCellStyle.Font = this.CreateItalicFont(this.Table.Style.DefaultStyle.Font);
      pdfCellStyle.Borders.Top = pdfPen2;
    }
    if (this.Table.m_firstColumnStyle && this.m_currentCellIndex == 0 && (!this.Table.m_totalRowStyle || this.m_currentRowIndex != this.Table.Rows.Count - 1))
    {
      pdfCellStyle.Borders.All = pdfPen3;
      pdfCellStyle.BackgroundBrush = (PdfBrush) null;
      pdfCellStyle.Font = this.CreateItalicFont(this.Table.Style.DefaultStyle.Font);
      pdfCellStyle.Borders.Right = pdfPen2;
      if (this.m_currentRowIndex == 0)
        pdfCellStyle.Borders.Top = pdfPen2;
    }
    if (!this.Table.m_lastColumnStyle || this.m_currentCellIndex != this.Table.Columns.Count - 1 || this.Table.m_totalRowStyle && this.m_currentRowIndex == this.Table.Rows.Count - 1)
      return;
    pdfCellStyle.BackgroundBrush = (PdfBrush) null;
    pdfCellStyle.Font = this.CreateItalicFont(this.Table.Style.DefaultStyle.Font);
    pdfCellStyle.Borders.Left = pdfPen2;
  }

  private void ApplyGridTable1Light(Color borderColor, Color headerBottomColor)
  {
    PdfCellStyle pdfCellStyle = new PdfCellStyle();
    pdfCellStyle.Borders = PdfBorders.Default;
    this.Table.Style.DefaultStyle = pdfCellStyle;
    pdfCellStyle.Borders.All = new PdfPen(borderColor, 0.5f);
    if (this.m_currentRowIndex < 0 && this.Table.m_headerStyle)
    {
      if (this.Table.m_headerStyle && (this.m_currentCellIndex != 0 || !this.Table.m_firstColumnStyle) && (this.m_currentCellIndex != this.Table.Columns.Count - 1 || !this.Table.m_lastColumnStyle))
        pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
      pdfCellStyle.Borders.Bottom = new PdfPen(headerBottomColor);
    }
    if (this.Table.m_totalRowStyle && this.m_currentRowIndex == this.Table.Rows.Count - 1)
    {
      pdfCellStyle.Borders.Top = new PdfPen(headerBottomColor);
      pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
    }
    if (this.Table.m_firstColumnStyle && this.m_currentCellIndex == 0 && (!this.Table.m_totalRowStyle || this.m_currentRowIndex != this.Table.Rows.Count - 1))
      pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
    if (!this.Table.m_lastColumnStyle || this.m_currentCellIndex != this.Table.Columns.Count - 1 || this.Table.m_totalRowStyle && this.m_currentRowIndex == this.Table.Rows.Count - 1)
      return;
    pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
  }

  private void ApplyGridTable2(Color borderColor, Color backColor)
  {
    PdfCellStyle pdfCellStyle = new PdfCellStyle();
    pdfCellStyle.Borders = PdfBorders.Default;
    this.Table.Style.DefaultStyle = pdfCellStyle;
    pdfCellStyle.Borders.All = new PdfPen(borderColor, 0.5f);
    PdfPen pdfPen = new PdfPen(Color.Empty);
    PdfBrush pdfBrush = (PdfBrush) new PdfSolidBrush((PdfColor) backColor);
    if (this.m_currentCellIndex == 0)
      pdfCellStyle.Borders.Left = pdfPen;
    else if (this.m_currentCellIndex == this.Table.Columns.Count - 1)
      pdfCellStyle.Borders.Right = pdfPen;
    if (this.m_currentRowIndex < 0 && this.Table.m_headerStyle)
    {
      if (this.Table.m_headerStyle && (this.m_currentCellIndex != 0 || !this.Table.m_firstColumnStyle) && (this.m_currentCellIndex != this.Table.Columns.Count - 1 || !this.Table.m_lastColumnStyle))
        pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
      pdfCellStyle.Borders.All = pdfPen;
      pdfCellStyle.Borders.Bottom = new PdfPen(borderColor);
    }
    if (this.Table.m_bandedColStyle && this.Table.m_bandedRowStyle)
    {
      if (!this.Table.m_headerStyle || this.m_currentRowIndex >= 0)
        pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, backColor, this.m_currentCellIndex);
      if (pdfCellStyle.BackgroundBrush == null)
        pdfCellStyle.BackgroundBrush = this.ApplyBandedRowStyle(this.Table.m_headerStyle, backColor, this.m_currentRowIndex);
    }
    else
    {
      if (this.Table.m_bandedColStyle && (!this.Table.m_headerStyle || this.m_currentRowIndex >= 0))
        pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, backColor, this.m_currentCellIndex);
      if (this.Table.m_bandedRowStyle)
        pdfCellStyle.BackgroundBrush = this.m_currentRowIndex >= 0 || this.Table.m_headerStyle ? this.ApplyBandedRowStyle(this.Table.m_headerStyle, backColor, this.m_currentRowIndex) : pdfBrush;
    }
    if (this.Table.m_totalRowStyle && this.m_currentRowIndex == this.Table.Rows.Count - 1)
    {
      pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
      pdfCellStyle.Borders.All = pdfPen;
      pdfCellStyle.Borders.Top = new PdfPen(borderColor);
      pdfCellStyle.BackgroundBrush = (PdfBrush) null;
    }
    if (this.Table.m_firstColumnStyle && this.m_currentCellIndex == 0 && (!this.Table.m_totalRowStyle || this.m_currentRowIndex != this.Table.Rows.Count - 1))
      pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
    if (!this.Table.m_lastColumnStyle || this.m_currentCellIndex != this.Table.Columns.Count - 1 || this.Table.m_totalRowStyle && this.m_currentRowIndex == this.Table.Rows.Count - 1)
      return;
    pdfCellStyle.BackgroundBrush = (PdfBrush) null;
    pdfCellStyle.Borders.All = pdfPen;
    if (this.Table.m_bandedRowStyle)
      pdfCellStyle.BackgroundBrush = this.m_currentRowIndex >= 0 || this.Table.m_headerStyle ? this.ApplyBandedRowStyle(this.Table.m_headerStyle, backColor, this.m_currentRowIndex) : pdfBrush;
    pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
  }

  private void ApplyGridTable3(Color borderColor, Color backColor)
  {
    PdfCellStyle pdfCellStyle = new PdfCellStyle();
    pdfCellStyle.Borders = PdfBorders.Default;
    this.Table.Style.DefaultStyle = pdfCellStyle;
    PdfPen pdfPen1 = new PdfPen(borderColor, 0.5f);
    pdfCellStyle.Borders.All = pdfPen1;
    PdfPen pdfPen2 = new PdfPen(Color.Empty);
    PdfBrush pdfBrush = (PdfBrush) new PdfSolidBrush((PdfColor) backColor);
    pdfCellStyle.Borders.All = pdfPen1;
    if (this.m_currentRowIndex < 0 && this.Table.m_headerStyle)
    {
      pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
      pdfCellStyle.Borders.All = pdfPen2;
    }
    if (this.Table.m_bandedRowStyle && this.Table.m_bandedColStyle)
    {
      pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, backColor, this.m_currentCellIndex);
      if (pdfCellStyle.BackgroundBrush == null)
        pdfCellStyle.BackgroundBrush = this.ApplyBandedRowStyle(this.Table.m_headerStyle, backColor, this.m_currentRowIndex);
      if (this.Table.m_headerStyle && this.m_currentRowIndex < 0)
        pdfCellStyle.BackgroundBrush = (PdfBrush) null;
    }
    else
    {
      if (this.Table.m_bandedColStyle && (!this.Table.m_headerStyle || this.m_currentRowIndex >= 0))
        pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, backColor, this.m_currentCellIndex);
      if (this.Table.m_bandedRowStyle)
        pdfCellStyle.BackgroundBrush = this.m_currentRowIndex >= 0 || this.Table.m_headerStyle ? this.ApplyBandedRowStyle(this.Table.m_headerStyle, backColor, this.m_currentRowIndex) : pdfBrush;
    }
    if (this.Table.m_totalRowStyle && this.m_currentRowIndex == this.Table.Rows.Count - 1)
    {
      pdfCellStyle.Borders.All = pdfPen2;
      pdfCellStyle.BackgroundBrush = (PdfBrush) null;
      pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
    }
    if (this.Table.m_firstColumnStyle && this.m_currentCellIndex == 0)
    {
      if (!this.Table.m_totalRowStyle || this.m_currentRowIndex != this.Table.Rows.Count - 1)
      {
        if (!this.Table.m_headerStyle || this.m_currentRowIndex >= 0)
          pdfCellStyle.Font = this.CreateItalicFont(this.Table.Style.DefaultStyle.Font);
        pdfCellStyle.BackgroundBrush = (PdfBrush) null;
        pdfCellStyle.Borders.All = pdfPen2;
        if (this.m_currentRowIndex == 0)
          pdfCellStyle.Borders.Top = pdfPen1;
      }
      else
        pdfCellStyle.Borders.Top = pdfPen1;
    }
    if (!this.Table.m_lastColumnStyle || this.m_currentCellIndex != this.Table.Columns.Count - 1)
      return;
    if (!this.Table.m_totalRowStyle || this.m_currentRowIndex != this.Table.Rows.Count - 1)
    {
      pdfCellStyle.BackgroundBrush = (PdfBrush) null;
      pdfCellStyle.Borders.All = pdfPen2;
      if (!this.Table.m_headerStyle || this.m_currentRowIndex >= 0)
        pdfCellStyle.Font = this.CreateItalicFont(this.Table.Style.DefaultStyle.Font);
      if (this.m_currentRowIndex != 0)
        return;
      pdfCellStyle.Borders.Top = pdfPen1;
    }
    else
      pdfCellStyle.Borders.Top = pdfPen1;
  }

  private void ApplyGridTable4(Color borderColor, Color backColor, Color headerColor)
  {
    PdfCellStyle pdfCellStyle = new PdfCellStyle();
    pdfCellStyle.Borders = PdfBorders.Default;
    this.Table.Style.DefaultStyle = pdfCellStyle;
    PdfPen pdfPen1 = new PdfPen(borderColor, 0.5f);
    PdfPen pdfPen2 = new PdfPen(Color.Empty);
    PdfPen pdfPen3 = new PdfPen(headerColor, 0.5f);
    PdfBrush pdfBrush1 = (PdfBrush) new PdfSolidBrush((PdfColor) backColor);
    PdfBrush pdfBrush2 = (PdfBrush) new PdfSolidBrush((PdfColor) headerColor);
    PdfBrush pdfBrush3 = (PdfBrush) new PdfSolidBrush((PdfColor) Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
    pdfCellStyle.Borders.All = pdfPen1;
    if (this.m_currentRowIndex < 0 && this.Table.m_headerStyle)
    {
      if (this.Table.m_headerStyle && (this.m_currentCellIndex != 0 || !this.Table.m_firstColumnStyle) && (this.m_currentCellIndex != this.Table.Columns.Count - 1 || !this.Table.m_lastColumnStyle))
        pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
      pdfCellStyle.BackgroundBrush = pdfBrush2;
      pdfCellStyle.TextBrush = pdfBrush3;
      pdfCellStyle.Borders.Left = pdfPen3;
      pdfCellStyle.Borders.Right = pdfPen3;
    }
    if (this.Table.m_firstColumnStyle && this.m_currentCellIndex == 0)
    {
      if (!this.Table.m_totalRowStyle || this.m_currentRowIndex != this.Table.Rows.Count - 1)
        pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
      else
        pdfCellStyle.Borders.Top = pdfPen1;
    }
    if (this.Table.m_bandedColStyle && this.Table.m_bandedRowStyle)
    {
      pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, backColor, this.m_currentCellIndex);
      if (pdfCellStyle.BackgroundBrush == null)
        pdfCellStyle.BackgroundBrush = this.ApplyBandedRowStyle(this.Table.m_headerStyle, backColor, this.m_currentRowIndex);
      if (this.Table.m_headerStyle && this.m_currentRowIndex < 0)
        pdfCellStyle.BackgroundBrush = pdfBrush2;
    }
    else
    {
      if (this.Table.m_bandedColStyle && (!this.Table.m_headerStyle || this.m_currentRowIndex >= 0))
        pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, backColor, this.m_currentCellIndex);
      if (this.Table.m_bandedRowStyle)
        pdfCellStyle.BackgroundBrush = this.m_currentRowIndex >= 0 ? this.ApplyBandedRowStyle(this.Table.m_headerStyle, backColor, this.m_currentRowIndex) : (!this.Table.m_headerStyle ? pdfBrush1 : pdfBrush2);
    }
    if (this.Table.m_totalRowStyle && this.m_currentRowIndex == this.Table.Rows.Count - 1)
    {
      pdfCellStyle.BackgroundBrush = (PdfBrush) null;
      pdfCellStyle.Borders.All = pdfPen1;
      if (this.Table.m_bandedColStyle)
      {
        if (!this.Table.m_lastColumnStyle || this.m_currentCellIndex != this.Table.Columns.Count - 1)
          pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, backColor, this.m_currentCellIndex);
        if (pdfCellStyle.BackgroundBrush != null)
          pdfCellStyle.Borders.All = pdfPen1;
      }
      pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
      pdfCellStyle.Borders.Left = pdfPen3;
      pdfCellStyle.Borders.Right = pdfPen3;
      pdfCellStyle.Borders.Top = new PdfPen(borderColor);
    }
    if (!this.Table.m_lastColumnStyle || this.m_currentCellIndex != this.Table.Columns.Count - 1 || this.Table.m_totalRowStyle && this.m_currentRowIndex == this.Table.Rows.Count - 1)
      return;
    pdfCellStyle.BackgroundBrush = (PdfBrush) null;
    if (this.Table.m_bandedRowStyle)
      pdfCellStyle.BackgroundBrush = this.ApplyBandedRowStyle(this.Table.m_headerStyle, backColor, this.m_currentRowIndex);
    if (this.m_currentRowIndex < 0)
      pdfCellStyle.BackgroundBrush = !this.Table.m_headerStyle ? pdfBrush1 : pdfBrush2;
    pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
  }

  private void ApplyGridTable5Dark(Color headerColor, Color oddRowColor, Color evenRowColor)
  {
    PdfCellStyle pdfCellStyle = new PdfCellStyle();
    pdfCellStyle.Borders = PdfBorders.Default;
    this.Table.Style.DefaultStyle = pdfCellStyle;
    PdfPen pdfPen1 = new PdfPen(Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), 0.5f);
    PdfPen pdfPen2 = new PdfPen(headerColor, 0.5f);
    PdfBrush pdfBrush1 = (PdfBrush) new PdfSolidBrush((PdfColor) headerColor);
    PdfBrush pdfBrush2 = (PdfBrush) new PdfSolidBrush((PdfColor) oddRowColor);
    PdfBrush pdfBrush3 = (PdfBrush) new PdfSolidBrush((PdfColor) evenRowColor);
    PdfBrush pdfBrush4 = (PdfBrush) new PdfSolidBrush((PdfColor) Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
    pdfCellStyle.Borders.All = pdfPen1;
    pdfCellStyle.BackgroundBrush = pdfBrush3;
    pdfCellStyle.BorderPen = pdfPen1;
    if (this.m_currentRowIndex < 0 && this.Table.m_headerStyle)
    {
      pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
      pdfCellStyle.BackgroundBrush = pdfBrush1;
      pdfCellStyle.TextBrush = pdfBrush4;
      pdfCellStyle.Borders.Left = pdfPen2;
      pdfCellStyle.Borders.Right = pdfPen2;
    }
    if (this.Table.m_bandedRowStyle && this.Table.m_bandedColStyle)
    {
      pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, oddRowColor, this.m_currentCellIndex);
      if (pdfCellStyle.BackgroundBrush == null)
        pdfCellStyle.BackgroundBrush = this.ApplyBandedRowStyle(this.Table.m_headerStyle, oddRowColor, this.m_currentRowIndex);
      if (this.m_currentRowIndex < 0 && this.Table.m_headerStyle)
        pdfCellStyle.BackgroundBrush = pdfBrush1;
      if (pdfCellStyle.BackgroundBrush == null)
        pdfCellStyle.BackgroundBrush = pdfBrush3;
    }
    else
    {
      if (this.Table.m_bandedColStyle)
      {
        pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, oddRowColor, this.m_currentCellIndex);
        if (this.m_currentRowIndex < 0 && this.Table.m_headerStyle)
          pdfCellStyle.BackgroundBrush = pdfBrush1;
        if (pdfCellStyle.BackgroundBrush == null)
          pdfCellStyle.BackgroundBrush = pdfBrush3;
      }
      if (this.Table.m_bandedRowStyle)
      {
        if (this.m_currentRowIndex < 0 && !this.Table.m_headerStyle)
        {
          pdfCellStyle.BackgroundBrush = pdfBrush2;
        }
        else
        {
          pdfCellStyle.BackgroundBrush = this.ApplyBandedRowStyle(this.Table.m_headerStyle, oddRowColor, this.m_currentRowIndex);
          if (this.m_currentRowIndex < 0 && this.Table.m_headerStyle)
            pdfCellStyle.BackgroundBrush = pdfBrush1;
          if (pdfCellStyle.BackgroundBrush == null)
            pdfCellStyle.BackgroundBrush = pdfBrush3;
        }
      }
    }
    if (this.Table.m_totalRowStyle && this.m_currentRowIndex == this.Table.Rows.Count - 1)
    {
      pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
      pdfCellStyle.BackgroundBrush = pdfBrush1;
      pdfCellStyle.TextBrush = pdfBrush4;
      pdfCellStyle.Borders.Left = pdfPen2;
      pdfCellStyle.Borders.Right = pdfPen2;
    }
    if (this.Table.m_firstColumnStyle && this.m_currentCellIndex == 0 && (!this.Table.m_totalRowStyle || this.m_currentRowIndex != this.Table.Rows.Count - 1))
    {
      pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
      pdfCellStyle.BackgroundBrush = pdfBrush1;
      pdfCellStyle.TextBrush = pdfBrush4;
    }
    if (!this.Table.m_lastColumnStyle || this.m_currentCellIndex != this.Table.Columns.Count - 1 || this.Table.m_totalRowStyle && this.m_currentRowIndex == this.Table.Rows.Count - 1)
      return;
    pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
    pdfCellStyle.BackgroundBrush = pdfBrush1;
    pdfCellStyle.TextBrush = pdfBrush4;
  }

  private void ApplyGridTable6Colorful(Color borderColor, Color backColor, Color textColor)
  {
    PdfCellStyle pdfCellStyle = new PdfCellStyle();
    pdfCellStyle.Borders = PdfBorders.Default;
    this.Table.Style.DefaultStyle = pdfCellStyle;
    PdfPen pdfPen = new PdfPen(borderColor, 0.5f);
    PdfBrush pdfBrush1 = (PdfBrush) new PdfSolidBrush((PdfColor) textColor);
    PdfBrush pdfBrush2 = (PdfBrush) new PdfSolidBrush((PdfColor) backColor);
    pdfCellStyle.Borders.All = pdfPen;
    pdfCellStyle.TextBrush = pdfBrush1;
    if (this.m_currentRowIndex < 0 && this.Table.m_headerStyle)
    {
      if (this.Table.m_headerStyle && (this.m_currentCellIndex != 0 || !this.Table.m_firstColumnStyle) && (this.m_currentCellIndex != this.Table.Columns.Count - 1 || !this.Table.m_lastColumnStyle))
        pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
      pdfCellStyle.Borders.Bottom = new PdfPen(borderColor);
      if (this.Table.m_bandedColStyle)
        pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, backColor, this.m_currentCellIndex);
    }
    if (this.Table.m_bandedColStyle && this.Table.m_bandedRowStyle)
    {
      pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, backColor, this.m_currentCellIndex);
      if (pdfCellStyle.BackgroundBrush == null)
        pdfCellStyle.BackgroundBrush = this.ApplyBandedRowStyle(this.Table.m_headerStyle, backColor, this.m_currentRowIndex);
    }
    else
    {
      if (this.Table.m_bandedColStyle)
        pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, backColor, this.m_currentCellIndex);
      if (this.Table.m_bandedRowStyle)
        pdfCellStyle.BackgroundBrush = this.m_currentRowIndex >= 0 || this.Table.m_headerStyle ? this.ApplyBandedRowStyle(this.Table.m_headerStyle, backColor, this.m_currentRowIndex) : pdfBrush2;
    }
    if (this.Table.m_firstColumnStyle && this.m_currentCellIndex == 0 && (!this.Table.m_totalRowStyle || this.m_currentRowIndex != this.Table.Rows.Count - 1))
      pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
    if (this.Table.m_lastColumnStyle && this.m_currentCellIndex == this.Table.Columns.Count - 1)
    {
      if (!this.Table.m_totalRowStyle || this.m_currentRowIndex != this.Table.Rows.Count - 1)
      {
        pdfCellStyle.BackgroundBrush = (PdfBrush) null;
        if (this.Table.m_bandedRowStyle)
          pdfCellStyle.BackgroundBrush = this.m_currentRowIndex >= 0 || this.Table.m_headerStyle ? this.ApplyBandedRowStyle(this.Table.m_headerStyle, backColor, this.m_currentRowIndex) : pdfBrush2;
        pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
        if (this.m_currentRowIndex == 0)
          pdfCellStyle.Borders.Top = pdfPen;
      }
      else
        pdfCellStyle.Borders.Top = pdfPen;
    }
    if (!this.Table.m_totalRowStyle || this.m_currentRowIndex != this.Table.Rows.Count - 1)
      return;
    pdfCellStyle.BackgroundBrush = (PdfBrush) null;
    pdfCellStyle.Borders.All = pdfPen;
    if (this.Table.m_bandedColStyle)
    {
      if (!this.Table.m_lastColumnStyle || this.m_currentCellIndex != this.Table.Columns.Count - 1)
        pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, backColor, this.m_currentCellIndex);
      if (pdfCellStyle.BackgroundBrush != null)
        pdfCellStyle.Borders.All = pdfPen;
    }
    pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
    pdfCellStyle.Borders.Top = new PdfPen(borderColor);
  }

  private void ApplyGridTable7Colorful(Color borderColor, Color backColor, Color textColor)
  {
    PdfCellStyle pdfCellStyle = new PdfCellStyle();
    pdfCellStyle.Borders = PdfBorders.Default;
    this.Table.Style.DefaultStyle = pdfCellStyle;
    PdfPen pdfPen1 = new PdfPen(borderColor, 0.5f);
    PdfPen pdfPen2 = new PdfPen(Color.Empty);
    PdfBrush pdfBrush1 = (PdfBrush) new PdfSolidBrush((PdfColor) textColor);
    PdfBrush pdfBrush2 = (PdfBrush) new PdfSolidBrush((PdfColor) backColor);
    pdfCellStyle.Borders.All = pdfPen1;
    pdfCellStyle.TextBrush = pdfBrush1;
    if (this.m_currentRowIndex < 0 && this.Table.m_headerStyle)
    {
      pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
      pdfCellStyle.Borders.Bottom = new PdfPen(borderColor);
      pdfCellStyle.Borders.All = new PdfPen(Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), 0.5f);
    }
    if (this.Table.m_bandedColStyle && this.Table.m_bandedRowStyle)
    {
      if (!this.Table.m_headerStyle || this.m_currentRowIndex >= 0)
        pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, backColor, this.m_currentCellIndex);
      if (pdfCellStyle.BackgroundBrush == null)
        pdfCellStyle.BackgroundBrush = this.ApplyBandedRowStyle(this.Table.m_headerStyle, backColor, this.m_currentRowIndex);
    }
    else
    {
      if (this.Table.m_bandedColStyle && (!this.Table.m_headerStyle || this.m_currentRowIndex >= 0))
        pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, backColor, this.m_currentCellIndex);
      if (this.Table.m_bandedRowStyle)
        pdfCellStyle.BackgroundBrush = this.m_currentRowIndex >= 0 || this.Table.m_headerStyle ? this.ApplyBandedRowStyle(this.Table.m_headerStyle, backColor, this.m_currentRowIndex) : pdfBrush2;
    }
    if (this.Table.m_totalRowStyle && this.m_currentRowIndex == this.Table.Rows.Count - 1)
    {
      pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
      pdfCellStyle.Borders.All = new PdfPen(Color.Empty);
      pdfCellStyle.BackgroundBrush = (PdfBrush) null;
    }
    if (this.Table.m_firstColumnStyle && this.m_currentCellIndex == 0)
    {
      if (!this.Table.m_totalRowStyle || this.m_currentRowIndex != this.Table.Rows.Count - 1)
      {
        if (!this.Table.m_headerStyle || this.m_currentRowIndex >= 0)
          pdfCellStyle.Font = this.CreateItalicFont(this.Table.Style.DefaultStyle.Font);
        pdfCellStyle.BackgroundBrush = (PdfBrush) null;
        pdfCellStyle.Borders.All = pdfPen2;
        if (this.m_currentRowIndex == 0)
          pdfCellStyle.Borders.Top = pdfPen1;
      }
      else
        pdfCellStyle.Borders.Top = pdfPen1;
    }
    if (!this.Table.m_lastColumnStyle || this.m_currentCellIndex != this.Table.Columns.Count - 1)
      return;
    if (!this.Table.m_totalRowStyle || this.m_currentRowIndex != this.Table.Rows.Count - 1)
    {
      pdfCellStyle.BackgroundBrush = (PdfBrush) null;
      pdfCellStyle.Borders.All = pdfPen2;
      if (!this.Table.m_headerStyle || this.m_currentRowIndex >= 0)
        pdfCellStyle.Font = this.CreateItalicFont(this.Table.Style.DefaultStyle.Font);
      if (this.m_currentRowIndex != 0)
        return;
      pdfCellStyle.Borders.Top = pdfPen1;
    }
    else
      pdfCellStyle.Borders.Top = pdfPen1;
  }

  private void ApplyListTable1Light(Color borderColor, Color backColor)
  {
    PdfCellStyle pdfCellStyle = new PdfCellStyle();
    pdfCellStyle.Borders = PdfBorders.Default;
    this.Table.Style.DefaultStyle = pdfCellStyle;
    PdfPen pdfPen1 = new PdfPen(Color.Empty);
    PdfPen pdfPen2 = new PdfPen(borderColor);
    PdfBrush pdfBrush = (PdfBrush) new PdfSolidBrush((PdfColor) backColor);
    pdfCellStyle.Borders.All = pdfPen1;
    pdfCellStyle.BorderPen = pdfPen1;
    if (this.m_currentRowIndex < 0 && this.Table.m_headerStyle)
    {
      if (this.Table.m_headerStyle && (this.m_currentCellIndex != 0 || !this.Table.m_firstColumnStyle) && (this.m_currentCellIndex != this.Table.Columns.Count - 1 || !this.Table.m_lastColumnStyle))
        pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
      pdfCellStyle.Borders.Bottom = pdfPen2;
      if (this.Table.m_bandedColStyle)
        pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, backColor, this.m_currentCellIndex);
    }
    if (this.Table.m_bandedRowStyle && this.Table.m_bandedColStyle)
    {
      pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, backColor, this.m_currentCellIndex);
      if (pdfCellStyle.BackgroundBrush == null)
        pdfCellStyle.BackgroundBrush = this.ApplyBandedRowStyle(this.Table.m_headerStyle, backColor, this.m_currentRowIndex);
    }
    else
    {
      if (this.Table.m_bandedColStyle)
        pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, backColor, this.m_currentCellIndex);
      if (this.Table.m_bandedRowStyle)
        pdfCellStyle.BackgroundBrush = this.m_currentRowIndex >= 0 || this.Table.m_headerStyle ? this.ApplyBandedRowStyle(this.Table.m_headerStyle, backColor, this.m_currentRowIndex) : pdfBrush;
    }
    if (this.Table.m_totalRowStyle && this.m_currentRowIndex == this.Table.Rows.Count - 1)
    {
      pdfCellStyle.BackgroundBrush = (PdfBrush) null;
      pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
      pdfCellStyle.Borders.Top = pdfPen2;
      if (this.Table.m_bandedColStyle && (!this.Table.m_lastColumnStyle || this.m_currentCellIndex != this.Table.Columns.Count - 1))
        pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, backColor, this.m_currentCellIndex);
    }
    if (this.Table.m_firstColumnStyle && this.m_currentCellIndex == 0 && (!this.Table.m_totalRowStyle || this.m_currentRowIndex != this.Table.Rows.Count - 1))
      pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
    if (!this.Table.m_lastColumnStyle || this.m_currentCellIndex != this.Table.Columns.Count - 1 || this.Table.m_totalRowStyle && this.m_currentRowIndex == this.Table.Rows.Count - 1)
      return;
    pdfCellStyle.BackgroundBrush = (PdfBrush) null;
    if (this.Table.m_bandedRowStyle)
      pdfCellStyle.BackgroundBrush = this.m_currentRowIndex >= 0 || this.Table.m_headerStyle ? this.ApplyBandedRowStyle(this.Table.m_headerStyle, backColor, this.m_currentRowIndex) : pdfBrush;
    pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
  }

  private void ApplyListTable2(Color borderColor, Color backColor)
  {
    PdfCellStyle pdfCellStyle = new PdfCellStyle();
    pdfCellStyle.Borders = PdfBorders.Default;
    this.Table.Style.DefaultStyle = pdfCellStyle;
    PdfPen pdfPen1 = new PdfPen(Color.Empty);
    PdfPen pdfPen2 = new PdfPen(borderColor, 0.5f);
    PdfBrush pdfBrush = (PdfBrush) new PdfSolidBrush((PdfColor) backColor);
    pdfCellStyle.Borders.All = pdfPen2;
    pdfCellStyle.Borders.Left = pdfPen1;
    pdfCellStyle.Borders.Right = pdfPen1;
    if (this.m_currentRowIndex < 0 && this.Table.m_headerStyle)
    {
      pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
      if (this.Table.m_bandedColStyle)
        pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, backColor, this.m_currentCellIndex);
    }
    if (this.Table.m_bandedColStyle && this.Table.m_bandedRowStyle)
    {
      pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, backColor, this.m_currentCellIndex);
      if (pdfCellStyle.BackgroundBrush == null)
        pdfCellStyle.BackgroundBrush = this.ApplyBandedRowStyle(this.Table.m_headerStyle, backColor, this.m_currentRowIndex);
    }
    else
    {
      if (this.Table.m_bandedColStyle)
        pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, backColor, this.m_currentCellIndex);
      if (this.Table.m_bandedRowStyle)
        pdfCellStyle.BackgroundBrush = this.m_currentRowIndex >= 0 || this.Table.m_headerStyle ? this.ApplyBandedRowStyle(this.Table.m_headerStyle, backColor, this.m_currentRowIndex) : pdfBrush;
    }
    if (this.Table.m_totalRowStyle && this.m_currentRowIndex == this.Table.Rows.Count - 1)
    {
      pdfCellStyle.BackgroundBrush = (PdfBrush) null;
      if (this.Table.m_bandedColStyle)
        pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, backColor, this.m_currentCellIndex);
      pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
    }
    if (this.Table.m_firstColumnStyle && this.m_currentCellIndex == 0 && (!this.Table.m_totalRowStyle || this.m_currentRowIndex != this.Table.Rows.Count - 1))
      pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
    if (!this.Table.m_lastColumnStyle || this.m_currentCellIndex != this.Table.Columns.Count - 1 || this.Table.m_totalRowStyle && this.m_currentRowIndex == this.Table.Rows.Count - 1)
      return;
    pdfCellStyle.BackgroundBrush = (PdfBrush) null;
    if (this.Table.m_bandedRowStyle)
      pdfCellStyle.BackgroundBrush = this.m_currentRowIndex >= 0 || this.Table.m_headerStyle ? this.ApplyBandedRowStyle(this.Table.m_headerStyle, backColor, this.m_currentRowIndex) : pdfBrush;
    pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
  }

  private void ApplyListTable3(Color backColor)
  {
    PdfCellStyle pdfCellStyle = new PdfCellStyle();
    pdfCellStyle.Borders = PdfBorders.Default;
    this.Table.Style.DefaultStyle = pdfCellStyle;
    PdfPen pdfPen1 = new PdfPen(backColor, 0.5f);
    PdfBrush pdfBrush1 = (PdfBrush) new PdfSolidBrush((PdfColor) backColor);
    PdfBrush pdfBrush2 = (PdfBrush) new PdfSolidBrush((PdfColor) Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
    PdfPen pdfPen2 = new PdfPen(Color.Empty);
    pdfCellStyle.Borders.All = pdfPen2;
    if (this.m_currentCellIndex == 0)
      pdfCellStyle.Borders.Left = pdfPen1;
    else if (this.m_currentCellIndex == this.Table.Columns.Count - 1)
      pdfCellStyle.Borders.Right = pdfPen1;
    if (this.Table.Style.ShowHeader)
    {
      if (this.m_currentRowIndex < 0)
        pdfCellStyle.Borders.Top = pdfPen1;
    }
    else if (this.m_currentRowIndex == 0)
      pdfCellStyle.Borders.Top = pdfPen1;
    if (this.m_currentRowIndex == this.Table.Rows.Count - 1)
      pdfCellStyle.Borders.Bottom = pdfPen1;
    if (this.m_currentRowIndex < 0 && this.Table.m_headerStyle)
    {
      pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
      pdfCellStyle.Borders.Top = pdfPen1;
      pdfCellStyle.BackgroundBrush = pdfBrush1;
      pdfCellStyle.TextBrush = pdfBrush2;
    }
    if (this.Table.m_bandedRowStyle)
      pdfCellStyle.Borders.Top = pdfPen1;
    if (this.Table.m_bandedColStyle)
      pdfCellStyle.Borders.Left = pdfPen1;
    if (this.Table.m_totalRowStyle && this.m_currentRowIndex == this.Table.Rows.Count - 1)
    {
      pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
      pdfCellStyle.Borders.Top = new PdfPen(backColor);
    }
    if (this.Table.m_firstColumnStyle && this.m_currentCellIndex == 0 && (!this.Table.m_totalRowStyle || this.m_currentRowIndex != this.Table.Rows.Count - 1))
    {
      if (this.Table.m_headerStyle)
      {
        if (this.m_currentRowIndex >= 0)
          pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
      }
      else
        pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
    }
    if (!this.Table.m_lastColumnStyle || this.m_currentCellIndex != this.Table.Columns.Count - 1 || this.Table.m_totalRowStyle && this.m_currentRowIndex == this.Table.Rows.Count - 1)
      return;
    if (this.Table.m_headerStyle)
    {
      if (this.m_currentRowIndex < 0)
        return;
      pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
    }
    else
      pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
  }

  private void ApplyListTable4(Color borderColor, Color headerBackColor, Color bandedRowColor)
  {
    PdfCellStyle pdfCellStyle = new PdfCellStyle();
    pdfCellStyle.Borders = PdfBorders.Default;
    this.Table.Style.DefaultStyle = pdfCellStyle;
    PdfPen pdfPen1 = new PdfPen(borderColor, 0.5f);
    PdfBrush pdfBrush1 = (PdfBrush) new PdfSolidBrush((PdfColor) headerBackColor);
    PdfBrush pdfBrush2 = (PdfBrush) new PdfSolidBrush((PdfColor) Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
    PdfPen pdfPen2 = new PdfPen(Color.Empty);
    PdfBrush pdfBrush3 = (PdfBrush) new PdfSolidBrush((PdfColor) bandedRowColor);
    pdfCellStyle.Borders.All = pdfPen2;
    if (this.m_currentCellIndex == 0)
      pdfCellStyle.Borders.Left = pdfPen1;
    else if (this.m_currentCellIndex == this.Table.Columns.Count - 1)
      pdfCellStyle.Borders.Right = pdfPen1;
    pdfCellStyle.Borders.Top = pdfPen1;
    pdfCellStyle.Borders.Bottom = pdfPen1;
    if (this.m_currentRowIndex < 0 && this.Table.m_headerStyle)
    {
      pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
      pdfCellStyle.Borders.All = new PdfPen(headerBackColor, 0.5f);
      pdfCellStyle.BackgroundBrush = pdfBrush1;
      pdfCellStyle.TextBrush = pdfBrush2;
    }
    if (this.Table.m_bandedColStyle && this.Table.m_bandedRowStyle)
    {
      if (!this.Table.m_lastColumnStyle || this.m_currentCellIndex != this.Table.Columns.Count - 1)
        pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, bandedRowColor, this.m_currentCellIndex);
      if (this.m_currentRowIndex < 0 && this.Table.m_headerStyle)
        pdfCellStyle.BackgroundBrush = pdfBrush1;
      if (pdfCellStyle.BackgroundBrush == null)
        pdfCellStyle.BackgroundBrush = this.ApplyBandedRowStyle(this.Table.m_headerStyle, bandedRowColor, this.m_currentRowIndex);
    }
    else
    {
      if (this.Table.m_bandedColStyle && (!this.Table.m_lastColumnStyle || this.m_currentCellIndex != this.Table.Columns.Count - 1))
        pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, bandedRowColor, this.m_currentCellIndex);
      if (this.Table.m_bandedRowStyle)
        pdfCellStyle.BackgroundBrush = this.m_currentRowIndex >= 0 || this.Table.m_headerStyle ? this.ApplyBandedRowStyle(this.Table.m_headerStyle, bandedRowColor, this.m_currentRowIndex) : pdfBrush3;
      if (this.m_currentRowIndex < 0 && this.Table.m_headerStyle)
        pdfCellStyle.BackgroundBrush = pdfBrush1;
    }
    if (this.Table.m_totalRowStyle && this.m_currentRowIndex == this.Table.Rows.Count - 1)
    {
      pdfCellStyle.BackgroundBrush = (PdfBrush) null;
      pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
      pdfCellStyle.Borders.Top = new PdfPen(borderColor);
      if (this.Table.m_bandedColStyle && (!this.Table.m_lastColumnStyle || this.m_currentCellIndex != this.Table.Columns.Count - 1))
        pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, bandedRowColor, this.m_currentCellIndex);
    }
    if (this.Table.m_firstColumnStyle && this.m_currentCellIndex == 0 && (!this.Table.m_totalRowStyle || this.m_currentRowIndex != this.Table.Rows.Count - 1))
    {
      if (this.Table.m_headerStyle)
      {
        if (this.m_currentRowIndex >= 0)
          pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
      }
      else
        pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
    }
    if (!this.Table.m_lastColumnStyle || this.m_currentCellIndex != this.Table.Columns.Count - 1 || this.Table.m_totalRowStyle && this.m_currentRowIndex == this.Table.Rows.Count - 1)
      return;
    if (this.Table.m_headerStyle)
    {
      if (this.m_currentRowIndex < 0)
        return;
      pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
    }
    else
      pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
  }

  private void ApplyListTable5Dark(Color backColor)
  {
    PdfCellStyle pdfCellStyle = new PdfCellStyle();
    pdfCellStyle.Borders = PdfBorders.Default;
    this.Table.Style.DefaultStyle = pdfCellStyle;
    PdfBrush pdfBrush1 = (PdfBrush) new PdfSolidBrush((PdfColor) backColor);
    PdfBrush pdfBrush2 = (PdfBrush) new PdfSolidBrush((PdfColor) Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
    PdfPen pdfPen1 = new PdfPen(Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), 0.5f);
    PdfPen pdfPen2 = new PdfPen(Color.Empty, 0.5f);
    pdfCellStyle.Borders.All = new PdfPen(Color.Empty);
    pdfCellStyle.BackgroundBrush = pdfBrush1;
    pdfCellStyle.TextBrush = pdfBrush2;
    if (this.m_currentRowIndex < 0 && this.Table.m_headerStyle)
    {
      pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
      pdfCellStyle.Borders.Bottom = new PdfPen(Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), 2f);
      if (this.Table.m_bandedColStyle)
      {
        pdfCellStyle.Borders.Left = pdfPen1;
        pdfCellStyle.Borders.Right = pdfPen1;
      }
    }
    if (this.Table.m_firstColumnStyle && (!this.Table.m_totalRowStyle || this.m_currentRowIndex != this.Table.Rows.Count - 1))
    {
      if (this.m_currentCellIndex == 0)
        pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
      else if (this.m_currentCellIndex == 1)
        pdfCellStyle.Borders.Left = pdfPen1;
    }
    if (this.Table.m_lastColumnStyle && this.m_currentCellIndex == this.Table.Columns.Count - 1 && (!this.Table.m_totalRowStyle || this.m_currentRowIndex != this.Table.Rows.Count - 1))
    {
      pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
      pdfCellStyle.Borders.Left = pdfPen1;
    }
    if (this.Table.m_bandedRowStyle)
    {
      pdfCellStyle.Borders.Top = pdfPen1;
      pdfCellStyle.Borders.Bottom = pdfPen1;
    }
    if (this.Table.m_bandedColStyle)
    {
      pdfCellStyle.Borders.Left = pdfPen1;
      pdfCellStyle.Borders.Right = pdfPen1;
    }
    if (!this.Table.m_totalRowStyle || this.m_currentRowIndex != this.Table.Rows.Count - 1)
      return;
    pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
    pdfCellStyle.Borders.Top = pdfPen1;
    if (this.Table.m_headerStyle)
      return;
    if (this.Table.m_firstColumnStyle && this.m_currentCellIndex == 0)
    {
      pdfCellStyle.Borders.Top = pdfPen2;
    }
    else
    {
      if (!this.Table.m_lastColumnStyle || this.m_currentCellIndex != this.Table.Columns.Count - 1)
        return;
      pdfCellStyle.Borders.Top = pdfPen2;
    }
  }

  private void ApplyListTable6Colorful(Color borderColor, Color backColor, Color textColor)
  {
    PdfCellStyle pdfCellStyle = new PdfCellStyle();
    pdfCellStyle.Borders = PdfBorders.Default;
    this.Table.Style.DefaultStyle = pdfCellStyle;
    PdfPen pdfPen1 = new PdfPen(Color.Empty, 0.5f);
    PdfPen pdfPen2 = new PdfPen(borderColor, 0.5f);
    PdfBrush pdfBrush1 = (PdfBrush) new PdfSolidBrush((PdfColor) textColor);
    PdfBrush pdfBrush2 = (PdfBrush) new PdfSolidBrush((PdfColor) backColor);
    pdfCellStyle.Borders.All = pdfPen1;
    pdfCellStyle.TextBrush = pdfBrush1;
    if (this.m_currentRowIndex == this.Table.Rows.Count - 1)
      pdfCellStyle.Borders.Bottom = pdfPen2;
    if (this.Table.Style.ShowHeader)
    {
      if (this.m_currentRowIndex < 0)
        pdfCellStyle.Borders.Top = pdfPen2;
    }
    else if (this.m_currentRowIndex == 0)
      pdfCellStyle.Borders.Top = pdfPen2;
    if (this.m_currentRowIndex < 0 && this.Table.m_headerStyle)
    {
      pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
      pdfCellStyle.Borders.Bottom = pdfPen2;
      if (this.Table.m_bandedColStyle)
        pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, backColor, this.m_currentCellIndex);
    }
    if (this.Table.m_bandedRowStyle && this.Table.m_bandedColStyle)
    {
      pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, backColor, this.m_currentCellIndex);
      if (pdfCellStyle.BackgroundBrush == null)
        pdfCellStyle.BackgroundBrush = this.ApplyBandedRowStyle(this.Table.m_headerStyle, backColor, this.m_currentRowIndex);
    }
    else
    {
      if (this.Table.m_bandedColStyle)
      {
        pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, backColor, this.m_currentCellIndex);
        if (pdfCellStyle.BackgroundBrush != null && this.m_currentRowIndex == 0)
          pdfCellStyle.Borders.Top = pdfPen2;
      }
      if (this.Table.m_bandedRowStyle)
      {
        pdfCellStyle.BackgroundBrush = this.m_currentRowIndex >= 0 || this.Table.m_headerStyle ? this.ApplyBandedRowStyle(this.Table.m_headerStyle, backColor, this.m_currentRowIndex) : pdfBrush2;
        if (pdfCellStyle.BackgroundBrush != null && this.m_currentRowIndex == 0)
          pdfCellStyle.Borders.Top = pdfPen2;
      }
    }
    if (this.Table.m_totalRowStyle && this.m_currentRowIndex == this.Table.Rows.Count - 1)
    {
      pdfCellStyle.BackgroundBrush = (PdfBrush) null;
      pdfCellStyle.Borders.All = pdfPen1;
      if (this.Table.m_bandedColStyle)
        pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, backColor, this.m_currentCellIndex);
      pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
      pdfCellStyle.Borders.Top = new PdfPen(borderColor);
    }
    if (this.Table.m_firstColumnStyle && this.m_currentCellIndex == 0 && (!this.Table.m_totalRowStyle || this.m_currentRowIndex != this.Table.Rows.Count - 1))
      pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
    if (!this.Table.m_lastColumnStyle || this.m_currentCellIndex != this.Table.Columns.Count - 1 || this.Table.m_totalRowStyle && this.m_currentRowIndex == this.Table.Rows.Count - 1)
      return;
    pdfCellStyle.BackgroundBrush = (PdfBrush) null;
    if (this.Table.m_bandedRowStyle)
      pdfCellStyle.BackgroundBrush = this.m_currentRowIndex >= 0 || this.Table.m_headerStyle ? this.ApplyBandedRowStyle(this.Table.m_headerStyle, backColor, this.m_currentRowIndex) : pdfBrush2;
    pdfCellStyle.Font = this.ChangeFontStyle(this.Table.Style.DefaultStyle.Font);
  }

  private void ApplyListTable7Colorful(Color borderColor, Color backColor, Color textColor)
  {
    PdfCellStyle pdfCellStyle = new PdfCellStyle();
    pdfCellStyle.Borders = PdfBorders.Default;
    this.Table.Style.DefaultStyle = pdfCellStyle;
    PdfPen pdfPen1 = new PdfPen(Color.Empty);
    PdfBrush pdfBrush1 = (PdfBrush) new PdfSolidBrush((PdfColor) textColor);
    PdfPen pdfPen2 = new PdfPen(borderColor, 0.5f);
    PdfBrush pdfBrush2 = (PdfBrush) new PdfSolidBrush((PdfColor) backColor);
    pdfCellStyle.Borders.All = pdfPen1;
    pdfCellStyle.TextBrush = pdfBrush1;
    if (this.m_currentRowIndex < 0 && this.Table.m_headerStyle)
    {
      pdfCellStyle.Font = this.CreateItalicFont(this.Table.Style.DefaultStyle.Font);
      pdfCellStyle.Borders.Bottom = pdfPen2;
    }
    if (this.Table.m_bandedColStyle && this.Table.m_bandedRowStyle)
    {
      pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, backColor, this.m_currentCellIndex);
      if (pdfCellStyle.BackgroundBrush == null)
        pdfCellStyle.BackgroundBrush = this.ApplyBandedRowStyle(this.Table.m_headerStyle, backColor, this.m_currentRowIndex);
    }
    else
    {
      if (this.Table.m_bandedColStyle)
      {
        if (!this.Table.m_headerStyle || this.m_currentRowIndex >= 0)
          pdfCellStyle.BackgroundBrush = this.ApplyBandedColStyle(this.Table.m_firstColumnStyle, backColor, this.m_currentCellIndex);
        if (pdfCellStyle.BackgroundBrush != null)
        {
          if (this.Table.m_firstColumnStyle && this.m_currentCellIndex == 1)
            pdfCellStyle.Borders.Left = pdfPen2;
          else
            pdfCellStyle.Borders.All = pdfPen1;
          if (this.Table.m_headerStyle && this.m_currentRowIndex == 0)
            pdfCellStyle.Borders.Top = pdfPen2;
        }
      }
      if (this.Table.m_bandedRowStyle)
      {
        pdfCellStyle.BackgroundBrush = this.m_currentRowIndex >= 0 || this.Table.m_headerStyle ? this.ApplyBandedRowStyle(this.Table.m_headerStyle, backColor, this.m_currentRowIndex) : pdfBrush2;
        if (this.Table.m_firstColumnStyle && this.m_currentCellIndex == 1)
          pdfCellStyle.Borders.Left = pdfPen2;
        else
          pdfCellStyle.Borders.All = pdfPen1;
        if (this.Table.m_headerStyle && this.m_currentRowIndex == 0)
          pdfCellStyle.Borders.Top = pdfPen2;
      }
    }
    if (this.Table.m_firstColumnStyle && this.m_currentRowIndex >= 0 && (!this.Table.m_totalRowStyle || this.m_currentRowIndex != this.Table.Rows.Count - 1) && this.m_currentCellIndex == 0)
    {
      pdfCellStyle.BackgroundBrush = (PdfBrush) null;
      pdfCellStyle.Font = this.CreateItalicFont(this.Table.Style.DefaultStyle.Font);
      pdfCellStyle.Borders.Right = pdfPen2;
    }
    if (this.Table.m_totalRowStyle && this.m_currentRowIndex == this.Table.Rows.Count - 1)
    {
      pdfCellStyle.Font = this.CreateItalicFont(this.Table.Style.DefaultStyle.Font);
      pdfCellStyle.BackgroundBrush = (PdfBrush) null;
      pdfCellStyle.Borders.All = pdfPen1;
      pdfCellStyle.Borders.Top = pdfPen2;
    }
    if (!this.Table.m_lastColumnStyle || this.m_currentRowIndex < 0 || this.Table.m_totalRowStyle && this.m_currentRowIndex == this.Table.Rows.Count - 1 || this.m_currentCellIndex != this.Table.Columns.Count - 1)
      return;
    pdfCellStyle.BackgroundBrush = (PdfBrush) null;
    pdfCellStyle.Font = this.CreateItalicFont(this.Table.Style.DefaultStyle.Font);
    pdfCellStyle.Borders.Left = pdfPen2;
  }

  private class PageLayoutResult
  {
    public PdfPage Page;
    public RectangleF Bounds;
    public bool Finish;
    public int FirstRowIndex;
    public int LastRowIndex;
  }
}
