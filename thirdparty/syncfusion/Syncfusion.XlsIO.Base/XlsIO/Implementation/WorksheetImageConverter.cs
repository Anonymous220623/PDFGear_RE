// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.WorksheetImageConverter
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.PivotTables;
using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class WorksheetImageConverter
{
  private CFApplier m_cfApplier = new CFApplier();
  private SortedList<long, ExtendedFormatImpl> m_sortedListCF;
  private Image result;
  private float rightWidth;
  private float leftWidth;
  private bool EnableRTL;
  private PivotTableImpl pivotImpl;
  private IRange pivotTableRange;

  internal StringFormat StringFormt
  {
    get
    {
      StringFormat stringFormt = new StringFormat(StringFormat.GenericTypographic);
      stringFormt.FormatFlags &= ~StringFormatFlags.LineLimit;
      stringFormt.FormatFlags |= StringFormatFlags.NoClip;
      return stringFormt;
    }
  }

  internal float LeftWidth => this.leftWidth;

  internal float RightWidth => this.rightWidth;

  public void ConvertToImage(
    WorksheetImpl sheet,
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    Stream outputStream)
  {
    this.ConvertToImage(sheet, firstRow, firstColumn, lastRow, lastColumn, ImageType.Bitmap, outputStream, EmfType.EmfOnly);
  }

  public Image ConvertToImage(
    WorksheetImpl sheet,
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn)
  {
    return this.ConvertToImage(sheet, firstRow, firstColumn, lastRow, lastColumn, ImageType.Bitmap, (Stream) null);
  }

  public Image ConvertToImage(
    WorksheetImpl sheet,
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    EmfType emfType,
    Stream outputStream)
  {
    return this.ConvertToImage(sheet, firstRow, firstColumn, lastRow, lastColumn, ImageType.Metafile, outputStream, emfType);
  }

  public Image ConvertToImage(
    WorksheetImpl sheet,
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    ImageType imageType,
    Stream outputStream)
  {
    return this.ConvertToImage(sheet, firstRow, firstColumn, lastRow, lastColumn, imageType, outputStream, EmfType.EmfOnly);
  }

  public Image ConvertToImage(
    WorksheetImpl sheet,
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    ImageType imageType,
    Stream outputStream,
    EmfType emfType)
  {
    ItemSizeHelper rowHeightGetter = new ItemSizeHelper(new ItemSizeHelper.SizeGetter(sheet.GetRowHeightInPixels));
    ItemSizeHelper columnWidthGetter = new ItemSizeHelper(new ItemSizeHelper.SizeGetter(sheet.GetColumnWidthInPixels));
    float total1 = rowHeightGetter.GetTotal(firstRow, lastRow);
    float total2 = columnWidthGetter.GetTotal(firstColumn, lastColumn);
    if (sheet.IsRightToLeft)
      this.EnableRTL = true;
    sheet.EnableSheetCalculations();
    if (sheet.PivotTables.Count > 0)
    {
      for (int index = 0; index < sheet.PivotTables.Count; ++index)
      {
        IPivotTable pivotTable = sheet.PivotTables[index];
        pivotTable.Layout();
        this.pivotImpl = pivotTable as PivotTableImpl;
        this.pivotTableRange = pivotTable.Location;
      }
    }
    this.result = this.CreateImage((int) total2, (int) total1, imageType, outputStream, emfType);
    this.ConvertToImage(this.result, sheet, firstRow, firstColumn, lastRow, lastColumn, rowHeightGetter, columnWidthGetter, (int) total2, (int) total1, outputStream);
    sheet.DisableSheetCalculations();
    if (imageType == ImageType.Bitmap && outputStream != null)
    {
      ImageFormat format = this.result.RawFormat.Equals((object) ImageFormat.MemoryBmp) ? ImageFormat.Bmp : this.result.RawFormat;
      this.result.Save(outputStream, format);
    }
    return this.result;
  }

  private void ConvertToImage(
    Image image,
    WorksheetImpl sheet,
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    ItemSizeHelper rowHeightGetter,
    ItemSizeHelper columnWidthGetter,
    int width,
    int height,
    Stream outputStream)
  {
    using (Graphics graphics = Graphics.FromImage(image))
    {
      graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, width, height));
      this.DrawBackgroundImage(sheet, graphics, firstRow, firstColumn, lastRow, lastColumn, rowHeightGetter, columnWidthGetter);
      this.DrawGridlines(sheet, firstRow, firstColumn, lastRow, lastColumn, graphics, rowHeightGetter, columnWidthGetter, width, height);
      if (sheet.ConditionalFormats.Count > 0)
        this.m_sortedListCF = sheet.ApplyCF();
      this.IterateCells(sheet, firstRow, firstColumn, lastRow, lastColumn, graphics, rowHeightGetter, columnWidthGetter, new WorksheetImageConverter.CellMethod(this.DrawBackground));
      this.IterateMerges(sheet, firstRow, firstColumn, lastRow, lastColumn, graphics, rowHeightGetter, columnWidthGetter, new WorksheetImageConverter.MergeMethod(this.DrawMergeBackground));
      this.DrawCells(sheet, firstRow, firstColumn, lastRow, lastColumn, graphics, rowHeightGetter, columnWidthGetter);
      this.IterateMerges(sheet, firstRow, firstColumn, lastRow, lastColumn, graphics, rowHeightGetter, columnWidthGetter, new WorksheetImageConverter.MergeMethod(this.DrawMerge));
      this.DrawShapes(sheet, graphics, firstRow, firstColumn, lastRow, lastColumn, rowHeightGetter, columnWidthGetter);
      if (!sheet.AppImplementation.EvalExpired)
        return;
      Font font = new Font("Arial", 10f, FontStyle.Bold | FontStyle.Italic);
      graphics.DrawString("Created with a trial version of Syncfusion Essential XlsIO", font, (Brush) new SolidBrush(Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 0, 0)), new RectangleF(0.0f, 0.0f, (float) width, 18f), new StringFormat(StringFormatFlags.FitBlackBox | StringFormatFlags.NoWrap | StringFormatFlags.NoClip));
    }
  }

  private void DrawShapes(
    WorksheetImpl sheet,
    Graphics graphics,
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    ItemSizeHelper rowHeightGetter,
    ItemSizeHelper columnWidthGetter)
  {
    foreach (IShape shape in (IEnumerable) sheet.Shapes)
    {
      if (shape.IsShapeVisible)
      {
        switch (shape.ShapeType)
        {
          case ExcelShapeType.Chart:
            this.DrawChart(shape as ChartShapeImpl, graphics, firstRow, firstColumn, lastRow, lastColumn, rowHeightGetter, columnWidthGetter);
            continue;
          case ExcelShapeType.Picture:
            this.DrawImage(shape, (shape as IPictureShape).Picture, graphics, firstRow, firstColumn, lastRow, lastColumn, rowHeightGetter, columnWidthGetter);
            continue;
          case ExcelShapeType.TextBox:
            this.DrawTextBox(shape as TextBoxShapeImpl, sheet, graphics, firstRow, firstColumn, lastRow, lastColumn, rowHeightGetter, columnWidthGetter);
            continue;
          default:
            continue;
        }
      }
    }
  }

  private void DrawChart(
    ChartShapeImpl chart,
    Graphics graphics,
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    ItemSizeHelper rowHeightGetter,
    ItemSizeHelper columnWidthGetter)
  {
    if (chart.ParentWorkbook.AppImplementation.ChartToImageConverter == null)
      return;
    MemoryStream imageAsStream = new MemoryStream();
    chart.SaveAsImage((Stream) imageAsStream);
    Image image = Image.FromStream((Stream) imageAsStream);
    if (imageAsStream.Length > 0L)
      this.DrawImage((IShape) chart, image, graphics, firstRow, firstColumn, lastRow, lastColumn, rowHeightGetter, columnWidthGetter);
    imageAsStream.Dispose();
    image.Dispose();
  }

  private void DrawTextBox(
    TextBoxShapeImpl shape,
    WorksheetImpl sheet,
    Graphics graphics,
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    ItemSizeHelper rowHeightGetter,
    ItemSizeHelper columnWidthGetter)
  {
    MigrantRangeImpl cell = new MigrantRangeImpl(sheet.Application, (IWorksheet) sheet);
    float total1 = rowHeightGetter.GetTotal(firstRow, lastRow);
    float total2 = columnWidthGetter.GetTotal(firstColumn, lastColumn);
    int num1 = 0;
    int num2 = 0;
    float num3 = (float) num1 + total2;
    float num4 = (float) num2 + total1;
    if (!shape.IsShapeVisible)
      return;
    float x = (float) shape.Left - (float) (shape.Left / (int) total2) * total2;
    float y = (float) shape.Top - (float) (shape.Top / (int) total1) * total1;
    float height = (float) shape.Height;
    float width = (float) shape.Width;
    if ((double) height == 0.0 || (double) width == 0.0)
      return;
    RectangleF layoutRectangle1 = new RectangleF(x, y, width, height);
    if ((double) shape.Top > (double) num4 || shape.Top < num2 || (double) shape.Left > (double) num3 || shape.Left < num1)
      return;
    Color foreColor = shape.Fill.ForeColor;
    if (shape.Fill.ForeColor.A == (byte) 0)
    {
      Color white = Color.White;
    }
    if (string.IsNullOrEmpty(shape.RichText.Text))
      return;
    cell.ResetRowColumn(firstRow, firstColumn);
    int extendedFormatIndex = (int) cell.ExtendedFormatIndex;
    ExtendedFormatImpl innerExtFormat = cell.Workbook.InnerExtFormats[extendedFormatIndex];
    StringFormatFlags options = innerExtFormat.WrapText ? (StringFormatFlags) 0 : StringFormatFlags.NoWrap;
    IFont font = shape.RichText.GetFont(0);
    StringFormat format = new StringFormat(options);
    format.Alignment = this.GetHorizontalAlignment((IExtendedFormat) innerExtFormat, (IRange) cell);
    format.LineAlignment = this.GetVerticalAlignment((IExtendedFormat) innerExtFormat);
    format.Trimming = StringTrimming.None;
    Font nativeFont = font.GenerateNativeFont();
    if (shape.ShapeRotation != 0)
    {
      GraphicsState gstate = graphics.Save();
      PointF pointF = new PointF(0.0f, 0.0f);
      pointF.X = layoutRectangle1.X + layoutRectangle1.Width / 2f;
      pointF.Y = layoutRectangle1.Y + layoutRectangle1.Height / 2f;
      graphics.TranslateTransform(pointF.X, pointF.Y);
      graphics.RotateTransform((float) shape.ShapeRotation);
      RectangleF layoutRectangle2 = new RectangleF(0.0f, 0.0f, layoutRectangle1.Height, layoutRectangle1.Width);
      Rectangle rect = new Rectangle(0, 0, (int) layoutRectangle1.Height, (int) layoutRectangle1.Width);
      if (shape.Line.Visible)
      {
        Pen pen = this.CreatePen((ShapeImpl) shape);
        graphics.DrawRectangle(pen, rect);
      }
      graphics.DrawString(shape.Text, nativeFont, (Brush) new SolidBrush(font.RGBColor), layoutRectangle2, format);
      graphics.Restore(gstate);
    }
    else
    {
      Rectangle rect = new Rectangle((int) layoutRectangle1.X, (int) layoutRectangle1.Y, (int) layoutRectangle1.Width, (int) layoutRectangle1.Height);
      if (shape.Line.Visible)
      {
        Pen pen = this.CreatePen((ShapeImpl) shape);
        graphics.DrawRectangle(pen, rect);
      }
      graphics.DrawString(shape.Text, nativeFont, (Brush) new SolidBrush(font.RGBColor), layoutRectangle1, format);
    }
  }

  private Pen CreatePen(ShapeImpl shape)
  {
    Color color = shape.Line.ForeColor;
    if (shape.Line.ForeColor.A == (byte) 0)
      color = Color.FromArgb((int) byte.MaxValue, (int) color.R, (int) color.G, (int) color.B);
    return new Pen(color, (float) shape.GetBorderThickness())
    {
      DashStyle = this.GetDashStyle(shape.Line)
    };
  }

  private DashStyle GetDashStyle(IShapeLineFormat lineFormat)
  {
    DashStyle dashStyle = DashStyle.Solid;
    switch (lineFormat.DashStyle)
    {
      case ExcelShapeDashLineStyle.Solid:
        dashStyle = DashStyle.Solid;
        break;
      case ExcelShapeDashLineStyle.Dotted:
      case ExcelShapeDashLineStyle.Dotted_Round:
        dashStyle = DashStyle.Dot;
        break;
      case ExcelShapeDashLineStyle.Dashed:
      case ExcelShapeDashLineStyle.Medium_Dashed:
        dashStyle = DashStyle.Dash;
        break;
      case ExcelShapeDashLineStyle.Dash_Dot:
      case ExcelShapeDashLineStyle.Medium_Dash_Dot:
        dashStyle = DashStyle.DashDot;
        break;
      case ExcelShapeDashLineStyle.Dash_Dot_Dot:
        dashStyle = DashStyle.DashDotDot;
        break;
    }
    return dashStyle;
  }

  private void DrawBackgroundImage(
    WorksheetImpl sheet,
    Graphics graphics,
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    ItemSizeHelper rowHeightGetter,
    ItemSizeHelper columnWidthGetter)
  {
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (sheet.PageSetup.BackgoundImage == null)
      return;
    Dictionary<PointF, SizeF> imageCoordinates = new Dictionary<PointF, SizeF>();
    Dictionary<PointF, SizeF> widthCoordinates = this.GetBackgroundWidthCoordinates(0.0f, 0.0f, (float) sheet.PageSetupBase.BackgoundImage.Width, (float) sheet.PageSetupBase.BackgoundImage.Height, imageCoordinates);
    if (widthCoordinates.Count != 0)
    {
      foreach (KeyValuePair<PointF, SizeF> keyValuePair in widthCoordinates)
        graphics.DrawImage((Image) sheet.PageSetup.BackgoundImage, firstRow, firstColumn, this.result.Width, this.result.Height);
    }
    else
      graphics.DrawImage((Image) sheet.PageSetup.BackgoundImage, 0, 0, this.result.Width, this.result.Height);
  }

  protected Dictionary<PointF, SizeF> GetBackgroundWidthCoordinates(
    float startX,
    float startY,
    float imageWidth,
    float imageHeight,
    Dictionary<PointF, SizeF> imageCoordinates)
  {
    if ((double) this.result.Width >= (double) startX)
    {
      if (!imageCoordinates.ContainsKey(new PointF(startX, startY)))
        imageCoordinates.Add(new PointF(startX, startY), new SizeF(imageWidth, imageHeight));
      imageCoordinates = this.GetBackgroundHeightCoordinates(startX, startY, imageWidth, imageHeight, imageCoordinates);
      this.GetBackgroundWidthCoordinates(startX + imageWidth, startY, imageWidth, imageHeight, imageCoordinates);
    }
    return imageCoordinates;
  }

  protected Dictionary<PointF, SizeF> GetBackgroundHeightCoordinates(
    float startX,
    float startY,
    float imageWidth,
    float imageHeight,
    Dictionary<PointF, SizeF> imageCoordinates)
  {
    if ((double) this.result.Height >= (double) startY)
    {
      if (!imageCoordinates.ContainsKey(new PointF(startX, startY)))
        imageCoordinates.Add(new PointF(startX, startY), new SizeF(imageWidth, imageHeight));
      this.GetBackgroundHeightCoordinates(startX, imageHeight + startY, imageWidth, imageHeight, imageCoordinates);
    }
    return imageCoordinates;
  }

  private void IterateMerges(
    WorksheetImpl sheet,
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    Graphics graphics,
    ItemSizeHelper rowHeightGetter,
    ItemSizeHelper columnWidthGetter,
    WorksheetImageConverter.MergeMethod method)
  {
    if (!sheet.HasMergedCells)
      return;
    List<MergeCellsRecord.MergedRegion> lstRegions = new List<MergeCellsRecord.MergedRegion>();
    sheet.MergeCells.CacheMerges(new Rectangle(firstColumn, firstRow, lastColumn - firstColumn, lastRow - firstRow), lstRegions);
    int index = 0;
    for (int count = lstRegions.Count; index < count; ++index)
      method(sheet, lstRegions[index], firstRow, firstColumn, graphics, rowHeightGetter, columnWidthGetter);
  }

  private void DrawMerge(
    WorksheetImpl sheet,
    MergeCellsRecord.MergedRegion mergedRegion,
    int firstRow,
    int firstColumn,
    Graphics graphics,
    ItemSizeHelper rowHeightGetter,
    ItemSizeHelper columnWidthGetter)
  {
    ExtendedFormatImpl format = sheet.MergeCells.GetFormat(mergedRegion);
    RectangleF mergeRectangle = this.GetMergeRectangle(sheet, mergedRegion, firstRow, firstColumn, rowHeightGetter, columnWidthGetter);
    if (this.EnableRTL)
    {
      mergeRectangle.X = (float) this.result.Width - mergeRectangle.X;
      mergeRectangle.X -= mergeRectangle.Width;
    }
    IRange cell = sheet[mergedRegion.RowFrom + 1, mergedRegion.ColumnFrom + 1];
    if ((double) mergeRectangle.Height <= 0.0 || (double) mergeRectangle.Width <= 0.0)
      return;
    this.DrawCell(format, cell, mergeRectangle, mergeRectangle, graphics);
  }

  private RectangleF GetMergeRectangle(
    WorksheetImpl sheet,
    MergeCellsRecord.MergedRegion mergedRegion,
    int firstRow,
    int firstColumn,
    ItemSizeHelper rowHeightGetter,
    ItemSizeHelper columnWidthGetter)
  {
    float total1 = rowHeightGetter.GetTotal(mergedRegion.RowFrom);
    float total2 = columnWidthGetter.GetTotal(mergedRegion.ColumnFrom);
    float y = total1 - rowHeightGetter.GetTotal(firstRow - 1);
    float x = total2 - columnWidthGetter.GetTotal(firstColumn - 1);
    float total3 = rowHeightGetter.GetTotal(mergedRegion.RowFrom + 1, mergedRegion.RowTo + 1);
    float total4 = columnWidthGetter.GetTotal(mergedRegion.ColumnFrom + 1, mergedRegion.ColumnTo + 1);
    IRange range = sheet[mergedRegion.RowFrom + 1, mergedRegion.ColumnFrom + 1];
    return new RectangleF(x, y, total4, total3);
  }

  private void DrawMergeBackground(
    WorksheetImpl sheet,
    MergeCellsRecord.MergedRegion mergedRegion,
    int firstRow,
    int firstColumn,
    Graphics graphics,
    ItemSizeHelper rowHeightGetter,
    ItemSizeHelper columnWidthGetter)
  {
    ExtendedFormatImpl format = sheet.MergeCells.GetFormat(mergedRegion);
    float total1 = rowHeightGetter.GetTotal(mergedRegion.RowFrom);
    float total2 = columnWidthGetter.GetTotal(mergedRegion.ColumnFrom);
    float y = total1 - rowHeightGetter.GetTotal(firstRow - 1);
    float x = total2 - columnWidthGetter.GetTotal(firstColumn - 1);
    if (this.EnableRTL)
      x = (float) this.result.Width - x;
    float total3 = rowHeightGetter.GetTotal(mergedRegion.RowFrom + 1, mergedRegion.RowTo + 1);
    float total4 = columnWidthGetter.GetTotal(mergedRegion.ColumnFrom + 1, mergedRegion.ColumnTo + 1);
    if (this.EnableRTL)
      x -= total4;
    IRange cell = sheet[mergedRegion.RowFrom + 1, mergedRegion.ColumnFrom + 1];
    Rectangle rect = Rectangle.Round(new RectangleF(x, y, total4, total3));
    if ((double) total3 <= 0.0 || (double) total4 <= 0.0)
      return;
    this.DrawBackground((IInternalExtendedFormat) format, rect, graphics, cell);
  }

  private void IterateCells(
    WorksheetImpl sheet,
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    Graphics graphics,
    ItemSizeHelper rowHeightGetter,
    ItemSizeHelper columnWidthGetter,
    WorksheetImageConverter.CellMethod method)
  {
    MigrantRangeImpl cell = new MigrantRangeImpl(sheet.Application, (IWorksheet) sheet);
    for (int index1 = firstRow; index1 <= lastRow; ++index1)
    {
      float total1 = rowHeightGetter.GetTotal(firstRow, index1 - 1);
      float size1 = rowHeightGetter.GetSize(index1);
      if ((double) size1 > 0.0)
      {
        for (int index2 = firstColumn; index2 <= lastColumn; ++index2)
        {
          float total2 = columnWidthGetter.GetTotal(firstColumn, index2 - 1);
          cell.ResetRowColumn(index1, index2);
          float size2 = columnWidthGetter.GetSize(index2);
          Rectangle rect = Rectangle.Round(new RectangleF(total2, total1, size2, size1));
          method((IRange) cell, rect, graphics);
        }
      }
    }
  }

  private void DrawCells(
    WorksheetImpl sheet,
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    Graphics graphics,
    ItemSizeHelper rowHeightGetter,
    ItemSizeHelper columnWidthGetter)
  {
    MigrantRangeImpl cell = new MigrantRangeImpl(sheet.Application, (IWorksheet) sheet);
    MigrantRangeImpl cell2 = new MigrantRangeImpl(sheet.Application, (IWorksheet) sheet);
    int maxColumnCount = cell.Workbook.MaxColumnCount;
    for (int index1 = firstRow; index1 <= lastRow; ++index1)
    {
      float total = rowHeightGetter.GetTotal(firstRow, index1 - 1);
      float size1 = rowHeightGetter.GetSize(index1);
      if ((double) size1 > 0.0)
      {
        for (int index2 = firstColumn; index2 <= lastColumn; ++index2)
        {
          float x = columnWidthGetter.GetTotal(firstColumn, index2 - 1);
          if (this.EnableRTL)
            x = (float) this.result.Width - x;
          cell.ResetRowColumn(index1, index2);
          if (!cell.IsMerged)
          {
            float size2 = columnWidthGetter.GetSize(index2);
            if (this.EnableRTL)
              x -= size2;
            if ((double) size2 > 0.0)
            {
              RectangleF rect = new RectangleF(x, total, size2, size1);
              RectangleF adjecentCells = this.GetAdjecentCells((IRange) cell, rect, columnWidthGetter, firstColumn, cell2);
              this.DrawCell(cell, rect, adjecentCells, graphics);
            }
          }
        }
      }
    }
  }

  private RectangleF GetAdjecentCells(
    IRange cell,
    RectangleF rect,
    ItemSizeHelper columnWidthGetter,
    int firstColumn,
    MigrantRangeImpl cell2)
  {
    RectangleF adjecentCells = rect;
    if (!cell.IsBlank && !cell.WrapText && (cell.HasString || cell.FormulaStringValue != null))
    {
      WorksheetImpl worksheet = cell.Worksheet as WorksheetImpl;
      SizeF sizeF = worksheet.MeasureCell(cell, false, false);
      float size = columnWidthGetter.GetSize(cell.Column);
      int width = (int) sizeF.Width;
      int iDelta1 = 1;
      if ((double) width > (double) size)
      {
        ExcelHAlign horizontalAlignment = cell.CellStyle.HorizontalAlignment;
        float newWidth = 0.0f;
        float num = 0.0f;
        int column = cell.Column;
        float x;
        if (horizontalAlignment == ExcelHAlign.HAlignCenter || horizontalAlignment == ExcelHAlign.HAlignCenterAcrossSelection)
        {
          num = this.GetUpdatedPosition(cell, rect, columnWidthGetter, firstColumn, cell2, iDelta1, worksheet, size, (int) ((double) size + ((double) width - (double) size) / 2.0), out newWidth);
          this.rightWidth = newWidth - size;
          int iDelta2 = -1;
          x = this.GetUpdatedPosition(cell, rect, columnWidthGetter, firstColumn, cell2, iDelta2, worksheet, size, (int) ((double) size + (double) ((int) ((double) width - (double) size) / 2)), out newWidth);
          this.leftWidth = newWidth - size;
          newWidth = size + this.leftWidth + this.rightWidth;
        }
        else
        {
          if (horizontalAlignment == ExcelHAlign.HAlignRight)
            iDelta1 = -1;
          x = this.GetUpdatedPosition(cell, rect, columnWidthGetter, firstColumn, cell2, iDelta1, worksheet, size, width, out newWidth);
        }
        if (this.EnableRTL)
          x = (float) this.result.Width - x - newWidth;
        adjecentCells = new RectangleF(x, rect.Y, newWidth, rect.Height);
      }
    }
    return adjecentCells;
  }

  private float GetUpdatedPosition(
    IRange cell,
    RectangleF rect,
    ItemSizeHelper columnWidthGetter,
    int firstColumn,
    MigrantRangeImpl cell2,
    int iDelta,
    WorksheetImpl sheet,
    float iCurrentWidth,
    int iRequiredWidth,
    out float newWidth)
  {
    int row = cell.Row;
    int column = cell.Column;
    int maxColumnCount = sheet.ParentWorkbook.MaxColumnCount;
    cell2.ResetRowColumn(row, column + iDelta);
    while (cell2.IsBlank && column < maxColumnCount && column > 0 && (double) iRequiredWidth > (double) iCurrentWidth)
    {
      int itemIndex = cell2.Column;
      if (itemIndex == 0)
        itemIndex = 1;
      iCurrentWidth += columnWidthGetter.GetSize(itemIndex);
      column += iDelta;
      cell2.ResetRowColumn(row, column + iDelta);
    }
    int rowStart = Math.Min(column, cell.Column);
    int rowEnd = Math.Max(column, cell.Column);
    if (rowStart == 0)
      rowStart = 1;
    newWidth = columnWidthGetter.GetTotal(rowStart, rowEnd);
    return iDelta != -1 || rowStart >= firstColumn ? columnWidthGetter.GetTotal(firstColumn, rowStart - 1) : -columnWidthGetter.GetTotal(rowStart, rowStart + firstColumn - rowStart - 1);
  }

  private void DrawGridlines(
    WorksheetImpl sheet,
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    Graphics graphics,
    ItemSizeHelper rowHeightGetter,
    ItemSizeHelper columnWidthGetter,
    int width,
    int height)
  {
    if (!sheet.IsGridLinesVisible)
      return;
    Pen pen = sheet.DefaultGridlineColor ? Pens.LightGray : new Pen(sheet.Workbook.GetPaletteColor(sheet.GridLineColor));
    graphics.DrawLine(pen, 0, 0, width, 0);
    int itemIndex1 = firstRow;
    int num1 = 0;
    for (; itemIndex1 <= lastRow; ++itemIndex1)
    {
      num1 += (int) rowHeightGetter.GetSize(itemIndex1);
      graphics.DrawLine(pen, 0, num1, width, num1);
    }
    graphics.DrawLine(pen, 0, 0, 0, height);
    int itemIndex2 = firstColumn;
    int num2 = 0;
    for (; itemIndex2 <= lastColumn; ++itemIndex2)
    {
      num2 += (int) columnWidthGetter.GetSize(itemIndex2);
      graphics.DrawLine(pen, num2, 0, num2, height);
    }
    if (sheet.DefaultGridlineColor)
      return;
    pen.Dispose();
  }

  private void DrawImage(
    IShape shape,
    Image image,
    Graphics graphics,
    int firstRow,
    int firstColumn,
    int lastRow,
    int lastColumn,
    ItemSizeHelper rowHeightGetter,
    ItemSizeHelper columnWidthGetter)
  {
    float total1 = columnWidthGetter.GetTotal(firstColumn - 1);
    float total2 = rowHeightGetter.GetTotal(firstRow - 1);
    float total3 = columnWidthGetter.GetTotal(lastColumn);
    float total4 = rowHeightGetter.GetTotal(lastRow);
    if ((double) shape.Top > (double) total4 || (double) (shape.Top + shape.Height) < (double) total2 || (double) shape.Left > (double) total3 || (double) (shape.Left + shape.Width) < (double) total1)
      return;
    Rectangle rect = new Rectangle(shape.Left, shape.Top, shape.Width, shape.Height);
    rect.Offset((int) -(double) total1, (int) -(double) total2);
    graphics.DrawImage(image, rect);
  }

  private void DrawCell(
    MigrantRangeImpl cell,
    RectangleF rect,
    RectangleF rect2,
    Graphics graphics)
  {
    if (cell == null)
      throw new ArgumentNullException(nameof (cell));
    if (graphics == null)
      throw new ArgumentNullException(nameof (graphics));
    int extendedFormatIndex = (int) cell.ExtendedFormatIndex;
    this.DrawCell(cell.Workbook.InnerExtFormats[extendedFormatIndex], (IRange) cell, rect, rect2, graphics);
  }

  private void DrawCell(
    ExtendedFormatImpl xf,
    IRange cell,
    RectangleF rect,
    RectangleF rect2,
    Graphics graphics)
  {
    int indentLevel = xf.IndentLevel;
    bool flag = false;
    int num1 = 1;
    int num2 = 1;
    for (int index = 0; index < cell.Worksheet.PivotTables.Count; ++index)
    {
      if (cell.Row == cell.Worksheet.PivotTables[index].Location.Row && cell.Column == cell.Worksheet.PivotTables[index].Location.Column)
      {
        this.pivotTableRange = cell.Worksheet.PivotTables[index].Location;
        this.pivotImpl = cell.Worksheet.PivotTables[index] as PivotTableImpl;
      }
    }
    if (this.pivotTableRange != null)
    {
      num1 = this.pivotTableRange.Row;
      num2 = this.pivotTableRange.Column;
    }
    PivotTableStyleRenderer tableStyleRenderer = new PivotTableStyleRenderer(cell.Worksheet);
    if (cell.Worksheet.PivotTables.Count != 0 && this.pivotImpl.PageFields.Count != 0)
    {
      if (num1 - 2 == cell.Row && num2 == cell.Column)
      {
        xf = tableStyleRenderer.GetPageFilterLabel(this.pivotImpl.BuiltInStyle);
        xf.VerticalAlignment = ExcelVAlign.VAlignTop;
      }
      if (num1 - 2 == cell.Row && num2 + 1 == cell.Column)
      {
        xf = tableStyleRenderer.GetPageFilterValue(this.pivotImpl.BuiltInStyle);
        xf.VerticalAlignment = ExcelVAlign.VAlignTop;
      }
    }
    if (cell.Worksheet.PivotTables.Count != 0 && this.pivotTableRange != null && this.pivotTableRange.Row <= cell.Row && this.pivotTableRange.LastRow >= cell.LastRow && this.pivotTableRange.Column <= cell.Column && this.pivotTableRange.LastColumn >= cell.LastColumn && this.pivotImpl.PivotLayout != null)
    {
      xf = this.pivotImpl.PivotLayout[cell.Row - num1, cell.Column - num2].XF;
      xf.VerticalAlignment = ExcelVAlign.VAlignTop;
      if (xf.HorizontalAlignment == ExcelHAlign.HAlignGeneral && xf.HorizontalAlignment != cell.CellStyle.HorizontalAlignment)
      {
        xf.HorizontalAlignment = cell.CellStyle.HorizontalAlignment;
        xf.IndentLevel = indentLevel;
      }
    }
    else
    {
      long cellIndex = RangeImpl.GetCellIndex(cell.Column, cell.Row);
      if (this.m_sortedListCF != null && this.m_sortedListCF.ContainsKey(cellIndex))
        this.m_sortedListCF.TryGetValue(cellIndex, out xf);
      if (xf != null && xf is ExtendedFormatStandAlone)
        flag = (xf as ExtendedFormatStandAlone).ShowIconOnly;
    }
    IFont font = xf.Font;
    string fontName = font.FontName;
    Brush brush = (Brush) new SolidBrush(this.NormalizeColor(font.RGBColor));
    StringFormat stringFormt = this.StringFormt;
    stringFormt.Alignment = this.GetHorizontalAlignment((IExtendedFormat) xf, cell);
    stringFormt.LineAlignment = this.GetVerticalAlignment((IExtendedFormat) xf);
    if (xf != null && xf.IndentLevel > 0)
    {
      float num3 = (float) xf.IndentLevel * 7.2f;
      switch (stringFormt.Alignment)
      {
        case StringAlignment.Near:
          rect2.X += num3;
          break;
        case StringAlignment.Far:
          rect2.Width -= num3;
          break;
      }
    }
    if (!xf.WrapText)
    {
      stringFormt.FormatFlags |= StringFormatFlags.NoWrap;
      stringFormt.Trimming = StringTrimming.Character;
    }
    else
      stringFormt.Trimming = StringTrimming.Word;
    string toBottomText = this.UpdateToToBottomText(cell.DisplayText, xf.Rotation);
    FontStyle fontStyle = this.GetFontStyle(font);
    Stream stream = (Stream) null;
    font.FontName = (cell.Worksheet.Workbook.Application as ApplicationImpl).TryGetFontStream(font.FontName, (float) font.Size, fontStyle, out stream);
    Font nativeFont = font.GenerateNativeFont();
    if (xf.Rotation != (int) byte.MaxValue && xf.Rotation != 0)
    {
      this.DrawRotatedText(rect, rect2, xf, cell, toBottomText, graphics, nativeFont, brush, stringFormt);
    }
    else
    {
      if (stringFormt.Alignment == StringAlignment.Near)
      {
        float num4 = xf.Borders[ExcelBordersIndex.EdgeLeft].LineStyle == ExcelLineStyle.Double ? 2f : 1f;
        float num5 = this.GetBorderWidth(xf.Borders[ExcelBordersIndex.EdgeLeft]) * 0.5f;
        float num6 = this.GetBorderWidth(xf.Borders[ExcelBordersIndex.EdgeRight]) * 0.5f;
        rect2.X += num5 + num4;
        rect2.Width -= num5 + num6 + num4;
      }
      else if (stringFormt.Alignment == StringAlignment.Far)
      {
        float num7 = xf.Borders[ExcelBordersIndex.EdgeRight].LineStyle == ExcelLineStyle.Double ? 2f : 1f;
        float num8 = this.GetBorderWidth(xf.Borders[ExcelBordersIndex.EdgeLeft]) * 0.5f;
        float num9 = this.GetBorderWidth(xf.Borders[ExcelBordersIndex.EdgeRight]) * 0.5f;
        rect2.X += num8;
        rect2.Width -= num8 + num9 + num7;
      }
      if (stringFormt.LineAlignment == StringAlignment.Near)
      {
        float num10 = xf.Borders[ExcelBordersIndex.EdgeTop].LineStyle == ExcelLineStyle.Double ? 2f : 1f;
        float num11 = this.GetBorderWidth(xf.Borders[ExcelBordersIndex.EdgeTop]) * 0.5f;
        float num12 = this.GetBorderWidth(xf.Borders[ExcelBordersIndex.EdgeBottom]) * 0.5f;
        rect2.Y += num11 + num10;
        rect2.Height -= num11 + num12 + num10;
      }
      else if (stringFormt.LineAlignment == StringAlignment.Far)
      {
        float num13 = xf.Borders[ExcelBordersIndex.EdgeBottom].LineStyle == ExcelLineStyle.Double ? 2f : 1f;
        float num14 = this.GetBorderWidth(xf.Borders[ExcelBordersIndex.EdgeTop]) * 0.5f;
        float num15 = this.GetBorderWidth(xf.Borders[ExcelBordersIndex.EdgeBottom]) * 0.5f;
        rect2.Y += num14;
        rect2.Height -= num14 + num15 + num13;
      }
      if (cell.HasRichText)
      {
        this.DrawRtfText(rect, rect2, cell, toBottomText, stringFormt, graphics);
      }
      else
      {
        this.DrawIconSet(graphics, xf, ref rect2, nativeFont, stringFormt);
        if (!flag)
          this.DrawString(toBottomText, nativeFont, brush, rect, rect2, stringFormt, graphics, cell);
      }
    }
    brush.Dispose();
    this.DrawBorders(xf.Borders, rect, graphics, cell);
    font.FontName = fontName;
  }

  private void DrawString(
    string value,
    Font nativeFont,
    Brush brush,
    RectangleF rect,
    RectangleF rect2,
    StringFormat format,
    Graphics graphics,
    IRange cell)
  {
    int scale = 1;
    SizeF sizeF = graphics.MeasureString(value, nativeFont, new PointF(0.0f, 0.0f), format);
    if (format.Alignment == StringAlignment.Center && !cell.WrapText && ((double) this.leftWidth != 0.0 || (double) this.rightWidth != 0.0))
    {
      RectangleF textRenderingBounds = new RectangleF(this.leftWidth + (float) (((double) new RectangleF(rect2.X + this.leftWidth, rect2.Y, rect2.Width - (this.leftWidth + this.rightWidth), rect2.Height).Width - (double) sizeF.Width) / 2.0), 0.0f, sizeF.Width, rect2.Height);
      this.DrawTextTemplate(graphics, value, nativeFont, brush, rect2, textRenderingBounds, format, (float) scale);
    }
    else if (cell.WrapText || (double) rect2.Width > (double) sizeF.Width)
    {
      float width = graphics.MeasureString(value, nativeFont, new PointF(0.0f, 0.0f), format).Width;
      if (format.Trimming == StringTrimming.Word || (double) rect2.Width >= (double) width)
      {
        float height1 = graphics.MeasureString(value.Replace("\n", " ").Replace("\r", " "), nativeFont, new PointF(0.0f, 0.0f), format).Height;
        float num1 = format.Trimming != StringTrimming.Word ? height1 : graphics.MeasureString(value, nativeFont, (int) rect2.Width, format).Height;
        if ((double) rect2.Height < (double) num1)
        {
          float height2 = rect2.Height;
          RowStorage rowStorage = (cell as RangeImpl).RowStorage;
          if (rowStorage != null && !rowStorage.IsBadFontHeight)
          {
            if (rowStorage.IsSpaceAboveRow)
              height2 -= 0.5f;
            if (rowStorage.IsSpaceBelowRow)
              height2 -= 0.5f;
          }
          float num2 = 0.0f;
          if ((double) height2 < (double) height1)
          {
            switch (format.LineAlignment)
            {
              case StringAlignment.Center:
                num2 = (float) (((double) height2 - (double) height1) / 2.0);
                break;
              case StringAlignment.Far:
                num2 = height2 - height1;
                break;
            }
            graphics.SetClip(rect2);
            graphics.DrawString(value, nativeFont, brush, new RectangleF(rect2.X, rect2.Y - Math.Abs(num2), rect2.Width, height1), format);
            graphics.ResetClip();
          }
          else if (Math.Floor((double) height2 * 100.0) / 100.0 % (Math.Floor((double) height1 * 100.0) / 100.0) >= 0.01)
          {
            float num3 = height1 * (float) ((int) ((double) height2 / (double) height1) + 1);
            switch (format.LineAlignment)
            {
              case StringAlignment.Center:
                num2 = (float) (((double) rect2.Height - (double) num3) / 2.0);
                break;
              case StringAlignment.Far:
                num2 = rect2.Height - num3;
                break;
            }
            graphics.SetClip(rect2);
            graphics.DrawString(value, nativeFont, brush, new RectangleF(rect2.X, rect2.Y - Math.Abs(num2), rect2.Width, (double) num2 != 0.0 ? num1 : height1), format);
            graphics.ResetClip();
          }
        }
        else
          graphics.DrawString(value, nativeFont, brush, rect2, format);
      }
    }
    else
    {
      switch ((cell.Worksheet as WorksheetImpl).GetCellType(cell.Row, cell.Column, true))
      {
        case WorksheetImpl.TRangeValueType.Number:
        case WorksheetImpl.TRangeValueType.Number | WorksheetImpl.TRangeValueType.Formula:
          if ((double) rect.Width < (double) sizeF.Width)
          {
            format.Alignment = StringAlignment.Near;
            break;
          }
          break;
      }
      graphics.SetClip(rect2);
      graphics.DrawString(value, nativeFont, brush, new RectangleF(rect2.X, rect2.Y, sizeF.Width, rect2.Height), format);
      graphics.ResetClip();
    }
    this.leftWidth = 0.0f;
    this.rightWidth = 0.0f;
  }

  private void DrawTextTemplate(
    Graphics graphics,
    string value,
    Font font,
    Brush brush,
    RectangleF cellRect,
    RectangleF textRenderingBounds,
    StringFormat format,
    float scale)
  {
    Bitmap bitmap = new Bitmap((int) ((double) cellRect.Width * (double) scale), (int) ((double) cellRect.Height * (double) scale));
    using (Graphics graphics1 = Graphics.FromImage((Image) bitmap))
    {
      graphics1.FillRectangle(Brushes.Transparent, new Rectangle(0, 0, (int) cellRect.Width, (int) cellRect.Height));
      graphics1.DrawString(value, font, brush, textRenderingBounds, format);
      graphics.DrawImage((Image) bitmap, cellRect);
    }
  }

  private FontStyle GetFontStyle(IFont font)
  {
    FontStyle fontStyle = FontStyle.Regular;
    if (font.Bold)
      fontStyle |= FontStyle.Bold;
    if (font.Italic)
      fontStyle |= FontStyle.Italic;
    if (font.Strikethrough)
      fontStyle |= FontStyle.Strikeout;
    if (font.Underline != ExcelUnderline.None)
      fontStyle |= FontStyle.Underline;
    return fontStyle;
  }

  private IRange GetAdjacentRange(IRange cell)
  {
    return (cell.Worksheet as WorksheetImpl)[cell.Row == 1 ? cell.Row : cell.Row - 1, cell.Column, cell.LastRow == 1 ? cell.LastRow : cell.LastRow - 1, cell.LastColumn];
  }

  private void DrawIconSet(
    Graphics graphics,
    ExtendedFormatImpl extendedFormatImpl,
    ref RectangleF adjacentRect,
    Font font,
    StringFormat format)
  {
    if (!(extendedFormatImpl is ExtendedFormatStandAlone formatStandAlone) || formatStandAlone.AdvancedCFIcon == null)
      return;
    int width = formatStandAlone.AdvancedCFIcon.Width;
    int height = formatStandAlone.AdvancedCFIcon.Height;
    if (font != null)
    {
      double num = (double) font.GetHeight() / (double) formatStandAlone.AdvancedCFIcon.Height;
      width = (int) ((double) formatStandAlone.AdvancedCFIcon.Width * num);
      height = (int) ((double) formatStandAlone.AdvancedCFIcon.Height * num);
    }
    float x = adjacentRect.X;
    float y = adjacentRect.Y;
    switch (format.LineAlignment)
    {
      case StringAlignment.Center:
        y += (float) (((double) adjacentRect.Height - (double) height) / 2.0);
        break;
      case StringAlignment.Far:
        y += adjacentRect.Height - (float) height;
        break;
    }
    graphics.DrawImage(formatStandAlone.AdvancedCFIcon, new RectangleF(x + 1f, y, (float) width, (float) height));
    if (format.Alignment != StringAlignment.Near)
      return;
    adjacentRect.X += (float) width;
    adjacentRect.Width -= (float) width;
  }

  private void DrawRtfText(
    RectangleF rect,
    RectangleF rect2,
    IRange cell,
    string value,
    StringFormat format,
    Graphics graphics)
  {
    if (value == string.Empty)
      throw new ArgumentNullException("text");
    int scale = 1;
    format.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
    List<string> drawString = new List<string>();
    List<IFont> richTextFonts = new List<IFont>();
    if (string.IsNullOrEmpty(cell.RichText.Text))
    {
      drawString.Add(cell.DisplayText);
      richTextFonts.Add(cell.CellStyle.Font);
    }
    else
      drawString = (cell.Worksheet.Workbook as WorkbookImpl).GetDrawString(cell.RichText.Text, cell.RichText as RichTextString, out richTextFonts, cell.RichText.GetFont(0));
    new GDIRenderer(cell, graphics, format, this, richTextFonts, drawString, cell.Worksheet.Workbook as WorkbookImpl, scale).DrawRTFText(rect, rect2, false, true, false, false, false, false);
    format.FormatFlags &= ~StringFormatFlags.MeasureTrailingSpaces;
  }

  private void DrawRotatedText(
    RectangleF rect,
    RectangleF rect2,
    ExtendedFormatImpl xf,
    IRange cell,
    string value,
    Graphics graphics,
    Font nativeFont,
    Brush brush,
    StringFormat format)
  {
    GraphicsState gstate = graphics.Save();
    Matrix matrix = new Matrix();
    float x1 = rect2.X;
    float y1 = rect2.Y;
    int clockwiseRotation1 = this.GetCounterClockwiseRotation(xf.Rotation);
    double num = (double) clockwiseRotation1 * Math.PI / 180.0;
    int clockwiseRotation2 = this.GetCounterClockwiseRotation(xf.Rotation);
    if (clockwiseRotation1 == 90 || clockwiseRotation1 == -90)
    {
      if (clockwiseRotation1 <= 0)
      {
        graphics.TranslateTransform(rect2.X, rect2.Bottom);
        if (cell.HorizontalAlignment == ExcelHAlign.HAlignGeneral)
          format.Alignment = StringAlignment.Far;
      }
      else
      {
        graphics.TranslateTransform(rect2.Right, rect2.Top);
        if (cell.HorizontalAlignment == ExcelHAlign.HAlignGeneral)
          format.Alignment = StringAlignment.Near;
      }
      format = this.UpdateAlignment(format, clockwiseRotation1);
      format.Trimming = StringTrimming.Word;
      graphics.RotateTransform((float) clockwiseRotation1);
      graphics.DrawString(value, nativeFont, brush, new RectangleF(0.0f, 0.0f, rect2.Height, rect2.Width), format);
    }
    else
    {
      SizeF size = (cell.Worksheet as WorksheetImpl).MeasureCell(cell, false, false);
      if (xf.Rotation <= 90)
      {
        ItemSizeHelper itemSizeHelper = new ItemSizeHelper(new ItemSizeHelper.SizeGetter(cell.Worksheet.GetRowHeightInPixels));
        rect2.Y = itemSizeHelper.GetTotal(1, cell.Row);
      }
      else if (xf.Rotation < 180)
        rect2.Y += size.Height;
      double height = (double) size.Height;
      double x2 = (double) rect2.X;
      double y2 = (double) rect2.Y;
      PointF[] pts = new PointF[1]
      {
        new PointF((float) x2, (float) y2)
      };
      matrix.RotateAt((float) clockwiseRotation2, new PointF((float) x2, (float) y2));
      matrix.TransformPoints(pts);
      float x3 = (float) Math.Round((double) pts[0].X);
      float y3 = (float) Math.Round((double) pts[0].Y);
      PointF pointF = this.AlignRectangle(xf.Rotation <= 90 || xf.Rotation > 180 ? this.RotateRectangle(size, xf.Rotation) : this.RotateRectangle(size, clockwiseRotation2), rect2, format.Alignment, format.LineAlignment);
      matrix.Translate(pointF.X, pointF.Y, MatrixOrder.Append);
      graphics.Transform = matrix;
      graphics.DrawString(value, nativeFont, brush, x3, y3, format);
    }
    graphics.Restore(gstate);
  }

  internal StringFormat UpdateAlignment(StringFormat format, int rotationAngle)
  {
    StringAlignment stringAlignment1 = StringAlignment.Near;
    StringAlignment stringAlignment2 = StringAlignment.Far;
    if (rotationAngle <= 0)
    {
      switch (format.Alignment)
      {
        case StringAlignment.Near:
          stringAlignment2 = StringAlignment.Near;
          break;
        case StringAlignment.Center:
          stringAlignment2 = StringAlignment.Center;
          break;
        case StringAlignment.Far:
          stringAlignment2 = StringAlignment.Far;
          break;
      }
      switch (format.LineAlignment)
      {
        case StringAlignment.Near:
          stringAlignment1 = StringAlignment.Far;
          break;
        case StringAlignment.Center:
          stringAlignment1 = StringAlignment.Center;
          break;
        case StringAlignment.Far:
          stringAlignment1 = StringAlignment.Near;
          break;
      }
    }
    else
    {
      switch (format.Alignment)
      {
        case StringAlignment.Near:
          stringAlignment2 = StringAlignment.Far;
          break;
        case StringAlignment.Center:
          stringAlignment2 = StringAlignment.Center;
          break;
        case StringAlignment.Far:
          stringAlignment2 = StringAlignment.Near;
          break;
      }
      switch (format.LineAlignment)
      {
        case StringAlignment.Near:
          stringAlignment1 = StringAlignment.Near;
          break;
        case StringAlignment.Center:
          stringAlignment1 = StringAlignment.Center;
          break;
        case StringAlignment.Far:
          stringAlignment1 = StringAlignment.Far;
          break;
      }
    }
    format.Alignment = stringAlignment1;
    format.LineAlignment = stringAlignment2;
    return format;
  }

  private PointF[] RotateRectangle(SizeF size, int angleDegrees)
  {
    Matrix matrix = new Matrix();
    PointF[] pts = new PointF[4]
    {
      new PointF(0.0f, 0.0f),
      new PointF(0.0f, size.Height),
      new PointF(size.Width, size.Height),
      new PointF(size.Width, 0.0f)
    };
    matrix.Rotate((float) angleDegrees);
    matrix.TransformPoints(pts);
    return pts;
  }

  private PointF AlignRectangle(
    PointF[] arrPoints,
    RectangleF rect2,
    StringAlignment horizontal,
    StringAlignment vertical)
  {
    float x = 0.0f;
    float y = 0.0f;
    float num1 = float.MaxValue;
    float num2 = float.MaxValue;
    float num3 = float.MinValue;
    float num4 = float.MinValue;
    int index = 0;
    for (int length = arrPoints.Length; index < length; ++index)
    {
      PointF arrPoint = arrPoints[index];
      if ((double) num1 > (double) arrPoint.X)
        num1 = arrPoint.X;
      if ((double) num3 < (double) arrPoint.X)
        num3 = arrPoint.X;
      if ((double) num2 > (double) arrPoint.Y)
        num2 = arrPoint.Y;
      if ((double) num4 < (double) arrPoint.Y)
        num4 = arrPoint.Y;
    }
    switch (horizontal)
    {
      case StringAlignment.Near:
        x = -num1;
        break;
      case StringAlignment.Center:
        x = (float) (((double) rect2.Width - (double) num3) / 2.0);
        break;
      case StringAlignment.Far:
        x = rect2.Width - num3;
        break;
    }
    switch (vertical)
    {
      case StringAlignment.Near:
        y = -num2;
        break;
      case StringAlignment.Center:
        y = (float) (((double) rect2.Height - (double) num4) / 2.0);
        break;
      case StringAlignment.Far:
        x = rect2.Height - num4;
        break;
    }
    return new PointF(x, y);
  }

  private int GetCounterClockwiseRotation(int rotationAngle)
  {
    if (rotationAngle > 90)
      rotationAngle -= 90;
    else
      rotationAngle = -rotationAngle;
    return rotationAngle;
  }

  private string UpdateToToBottomText(string value, int rotationAngle)
  {
    if (rotationAngle == (int) byte.MaxValue)
    {
      StringBuilder stringBuilder = new StringBuilder(value);
      int num = 0;
      int index = 1;
      int length = value.Length;
      while (num < length)
      {
        stringBuilder.Insert(index, '\n');
        ++num;
        index += 2;
      }
      value = stringBuilder.ToString();
    }
    return value;
  }

  private void DrawBackground(IRange cell, Rectangle rect, Graphics graphics)
  {
    ExtendedFormatImpl xf = (cell.CellStyle as CellStyle).Wrapped;
    int num1 = 1;
    int num2 = 1;
    for (int index = 0; index < cell.Worksheet.PivotTables.Count; ++index)
    {
      if (cell.Row == cell.Worksheet.PivotTables[index].Location.Row && cell.Column == cell.Worksheet.PivotTables[index].Location.Column)
      {
        this.pivotTableRange = cell.Worksheet.PivotTables[index].Location;
        this.pivotImpl = cell.Worksheet.PivotTables[index] as PivotTableImpl;
      }
    }
    if (this.pivotTableRange != null)
    {
      num1 = this.pivotTableRange.Row;
      num2 = this.pivotTableRange.Column;
    }
    PivotTableStyleRenderer tableStyleRenderer = new PivotTableStyleRenderer(cell.Worksheet);
    if (cell.Worksheet.PivotTables.Count != 0 && this.pivotImpl.PageFields.Count != 0)
    {
      if (num1 - 2 == cell.Row && num2 == cell.Column)
        xf = tableStyleRenderer.GetPageFilterLabel(this.pivotImpl.BuiltInStyle);
      if (num1 - 2 == cell.Row && num2 + 1 == cell.Column)
        xf = tableStyleRenderer.GetPageFilterValue(this.pivotImpl.BuiltInStyle);
    }
    if (this.pivotTableRange != null && this.pivotTableRange.Row <= cell.Row && this.pivotTableRange.LastRow >= cell.LastRow && this.pivotTableRange.Column <= cell.Column && this.pivotTableRange.LastColumn >= cell.LastColumn && this.pivotImpl.PivotLayout != null)
    {
      xf = this.pivotImpl.PivotLayout[cell.Row - num1, cell.Column - num2].XF;
    }
    else
    {
      long cellIndex = RangeImpl.GetCellIndex(cell.Column, cell.Row);
      if (this.m_sortedListCF != null && this.m_sortedListCF.ContainsKey(cellIndex))
        this.m_sortedListCF.TryGetValue(cellIndex, out xf);
    }
    this.DrawBackground((IInternalExtendedFormat) xf, rect, graphics, cell);
  }

  private void DrawBackground(
    IInternalExtendedFormat xf,
    Rectangle rect,
    Graphics graphics,
    IRange cell)
  {
    if (rect.Height <= 0 || rect.Width <= 0)
      return;
    if (xf.FillPattern == ExcelPattern.None)
    {
      rect.Offset(1, 1);
      --rect.Width;
      --rect.Height;
      IBorders borders = xf.Borders;
      rect = this.UpdateRectangleCoordinates(rect, borders);
      if (cell.Worksheet.PageSetup.BackgoundImage != null)
        return;
      Brush white = Brushes.White;
      graphics.FillRectangle(white, rect);
    }
    else
    {
      if (!(xf.Color != Color.White))
        return;
      Brush brush = this.GetBrush(xf);
      graphics.FillRectangle(brush, rect);
      brush.Dispose();
    }
  }

  private Rectangle UpdateRectangleCoordinates(Rectangle rect, IBorders borders)
  {
    if (borders[ExcelBordersIndex.EdgeLeft].LineStyle != ExcelLineStyle.None)
    {
      rect.Offset(-1, 0);
      ++rect.Width;
    }
    if (borders[ExcelBordersIndex.EdgeTop].LineStyle != ExcelLineStyle.None)
    {
      rect.Offset(0, -1);
      ++rect.Height;
    }
    if (borders[ExcelBordersIndex.EdgeBottom].LineStyle != ExcelLineStyle.None)
      ++rect.Height;
    if (borders[ExcelBordersIndex.EdgeRight].LineStyle != ExcelLineStyle.None)
      ++rect.Width;
    return rect;
  }

  private Brush GetBrush(IInternalExtendedFormat xf)
  {
    Brush brush;
    if (xf.FillPattern == ExcelPattern.Solid)
    {
      brush = (Brush) new SolidBrush(this.NormalizeColor(xf.Color));
    }
    else
    {
      HatchStyle hatchstyle = HatchStyle.Percent05;
      switch (xf.FillPattern)
      {
        case ExcelPattern.Percent50:
          hatchstyle = HatchStyle.Percent50;
          break;
        case ExcelPattern.Percent70:
          hatchstyle = HatchStyle.Percent70;
          break;
        case ExcelPattern.Percent25:
          hatchstyle = HatchStyle.Percent25;
          break;
        case ExcelPattern.DarkHorizontal:
          hatchstyle = HatchStyle.DarkHorizontal;
          break;
        case ExcelPattern.DarkVertical:
          hatchstyle = HatchStyle.DarkVertical;
          break;
        case ExcelPattern.DarkDownwardDiagonal:
          hatchstyle = HatchStyle.DarkDownwardDiagonal;
          break;
        case ExcelPattern.DarkUpwardDiagonal:
          hatchstyle = HatchStyle.DarkUpwardDiagonal;
          break;
        case ExcelPattern.ForwardDiagonal:
          hatchstyle = HatchStyle.ForwardDiagonal;
          break;
        case ExcelPattern.Percent75:
          hatchstyle = HatchStyle.Percent75;
          break;
        case ExcelPattern.Horizontal:
          hatchstyle = HatchStyle.Horizontal;
          break;
        case ExcelPattern.Vertical:
          hatchstyle = HatchStyle.Vertical;
          break;
        case ExcelPattern.LightDownwardDiagonal:
          hatchstyle = HatchStyle.LightDownwardDiagonal;
          break;
        case ExcelPattern.LightUpwardDiagonal:
          hatchstyle = HatchStyle.LightUpwardDiagonal;
          break;
        case ExcelPattern.Angle:
          hatchstyle = HatchStyle.SmallGrid;
          break;
        case ExcelPattern.Percent60:
          hatchstyle = HatchStyle.Percent60;
          break;
        case ExcelPattern.Percent10:
          hatchstyle = HatchStyle.Percent10;
          break;
        case ExcelPattern.Percent05:
          hatchstyle = HatchStyle.Percent05;
          break;
      }
      brush = (Brush) new HatchBrush(hatchstyle, xf.PatternColor, xf.Color);
    }
    return brush;
  }

  private Color NormalizeColor(Color color)
  {
    return Color.FromArgb((int) byte.MaxValue, (int) color.R, (int) color.G, (int) color.B);
  }

  private StringAlignment GetVerticalAlignment(IExtendedFormat style)
  {
    StringAlignment verticalAlignment;
    switch (style.VerticalAlignment)
    {
      case ExcelVAlign.VAlignTop:
        verticalAlignment = StringAlignment.Near;
        break;
      case ExcelVAlign.VAlignCenter:
        verticalAlignment = StringAlignment.Center;
        break;
      default:
        verticalAlignment = StringAlignment.Far;
        break;
    }
    return verticalAlignment;
  }

  private StringAlignment GetHorizontalAlignment(IExtendedFormat style, IRange cell)
  {
    StringAlignment horizontalAlignment;
    switch (style.HorizontalAlignment)
    {
      case ExcelHAlign.HAlignGeneral:
        horizontalAlignment = style.Rotation != (int) byte.MaxValue ? (cell.HasNumber || cell.HasFormula && cell.FormulaStringValue == null && !cell.HasFormulaErrorValue && !cell.HasFormulaBoolValue ? ((cell.Worksheet.Workbook as WorkbookImpl).InnerFormats[style.NumberFormatIndex].GetFormatType(cell.Number) == ExcelFormatType.Text ? StringAlignment.Near : StringAlignment.Far) : StringAlignment.Near) : StringAlignment.Center;
        break;
      case ExcelHAlign.HAlignCenter:
      case ExcelHAlign.HAlignCenterAcrossSelection:
        horizontalAlignment = StringAlignment.Center;
        break;
      case ExcelHAlign.HAlignRight:
        horizontalAlignment = StringAlignment.Far;
        break;
      default:
        horizontalAlignment = StringAlignment.Near;
        break;
    }
    return horizontalAlignment;
  }

  private void DrawBorders(IBorders borders, RectangleF rect, Graphics graphics, IRange cell)
  {
    IBorder border1 = borders[ExcelBordersIndex.EdgeLeft];
    if (this.EnableRTL)
      border1 = borders[ExcelBordersIndex.EdgeRight];
    this.DrawBorder(borders, border1, rect.Left, rect.Top, rect.Left, rect.Bottom, graphics, cell);
    IBorder border2 = borders[ExcelBordersIndex.EdgeRight];
    if (this.EnableRTL)
      border2 = borders[ExcelBordersIndex.EdgeLeft];
    this.DrawBorder(borders, border2, rect.Right, rect.Top, rect.Right, rect.Bottom, graphics, cell);
    IBorder border3 = borders[ExcelBordersIndex.EdgeTop];
    this.DrawBorder(borders, border3, rect.Left, rect.Top, rect.Right, rect.Top, graphics, cell);
    IBorder border4 = borders[ExcelBordersIndex.EdgeBottom];
    this.DrawBorder(borders, border4, rect.Left, rect.Bottom, rect.Right, rect.Bottom, graphics, cell);
    IBorder border5 = borders[ExcelBordersIndex.DiagonalDown];
    if (border5.ShowDiagonalLine)
      this.DrawBorder(borders, border5, rect.Left, rect.Top, rect.Right, rect.Bottom, graphics, cell);
    IBorder border6 = borders[ExcelBordersIndex.DiagonalUp];
    if (!border6.ShowDiagonalLine)
      return;
    this.DrawBorder(borders, border6, rect.Left, rect.Bottom, rect.Right, rect.Top, graphics, cell);
  }

  private void DrawBorder(
    IBorders borders,
    IBorder border,
    float x1,
    float y1,
    float x2,
    float y2,
    Graphics graphics,
    IRange cell)
  {
    if (border.LineStyle == ExcelLineStyle.Double)
      this.DrawDoubleBorder(borders, border, x1, y1, x2, y2, graphics, cell);
    else
      this.DrawOrdinaryBorder(border, x1, y1, x2, y2, graphics);
  }

  private void DrawDoubleBorder(
    IBorders borders,
    IBorder border,
    float x1,
    float y1,
    float x2,
    float y2,
    Graphics graphics,
    IRange cell)
  {
    Pen pen = this.CreatePen(border);
    ExcelBordersIndex borderIndex = (border as BorderImpl).BorderIndex;
    int xDelta;
    int yDelta;
    switch (borderIndex)
    {
      case ExcelBordersIndex.EdgeLeft:
        xDelta = -1;
        yDelta = 0;
        break;
      case ExcelBordersIndex.EdgeTop:
        xDelta = 0;
        yDelta = -1;
        break;
      case ExcelBordersIndex.EdgeBottom:
        xDelta = 0;
        yDelta = 1;
        break;
      case ExcelBordersIndex.EdgeRight:
        xDelta = 1;
        yDelta = 0;
        break;
      default:
        xDelta = 1;
        yDelta = 1;
        break;
    }
    this.DrawInnerLine(graphics, pen, borders, borderIndex, x1, y1, x2, y2, xDelta, yDelta, cell);
    this.DrawOuterLine(graphics, pen, borders, borderIndex, x1, y1, x2, y2, xDelta, yDelta, cell);
  }

  private void DrawOuterLine(
    Graphics graphics,
    Pen pen,
    IBorders borders,
    ExcelBordersIndex borderIndex,
    float x1,
    float y1,
    float x2,
    float y2,
    int xDelta,
    int yDelta,
    IRange cell)
  {
    ExcelBordersIndex start;
    ExcelBordersIndex end;
    this.GetStartEndBorderIndex(borderIndex, out start, out end);
    int xDelta1_1 = xDelta;
    int xDelta1_2 = xDelta;
    int yDelta1_1 = yDelta;
    int yDelta1_2 = yDelta;
    int row = cell.Row + yDelta;
    int column = cell.Column + xDelta;
    if (column == 0)
      column = 1;
    if (row == 0)
      row = 1;
    IBorders borders1 = cell.Worksheet[row, column].Borders;
    this.UpdateBorderDelta(cell.Worksheet, row, column, xDelta, yDelta, ref xDelta1_1, ref yDelta1_1, true, borders1, end, start, true);
    this.UpdateBorderDelta(cell.Worksheet, row, column, xDelta, yDelta, ref xDelta1_2, ref yDelta1_2, true, borders1, start, end, false);
    graphics.DrawLine(pen, x1 + (float) xDelta1_1, y1 + (float) yDelta1_1, x2 + (float) xDelta1_2, y2 + (float) yDelta1_2);
  }

  private void DrawInnerLine(
    Graphics graphics,
    Pen pen,
    IBorders borders,
    ExcelBordersIndex borderIndex,
    float x1,
    float y1,
    float x2,
    float y2,
    int xDelta,
    int yDelta,
    IRange cell)
  {
    ExcelBordersIndex start;
    ExcelBordersIndex end;
    this.GetStartEndBorderIndex(borderIndex, out start, out end);
    int xDelta1_1 = xDelta;
    int xDelta1_2 = xDelta;
    int yDelta1_1 = yDelta;
    int yDelta1_2 = yDelta;
    int row = cell.Row;
    int column = cell.Column;
    IWorksheet worksheet = cell.Worksheet;
    this.UpdateBorderDelta(worksheet, row, column, -xDelta, -yDelta, ref xDelta1_1, ref yDelta1_1, false, borders, end, start, true);
    this.UpdateBorderDelta(worksheet, row, column, -xDelta, -yDelta, ref xDelta1_2, ref yDelta1_2, false, borders, start, end, false);
    graphics.DrawLine(pen, x1 - (float) xDelta1_1, y1 - (float) yDelta1_1, x2 - (float) xDelta1_2, y2 - (float) yDelta1_2);
  }

  private void UpdateBorderDelta(
    IWorksheet sheet,
    int row,
    int column,
    int xDelta,
    int yDelta,
    ref int xDelta1,
    ref int yDelta1,
    bool isInvertCondition,
    IBorders borders,
    ExcelBordersIndex start,
    ExcelBordersIndex end,
    bool isLineStart)
  {
    int num1 = row + xDelta;
    int num2 = column + yDelta;
    IWorkbook workbook = sheet.Workbook;
    int maxRowCount = workbook.MaxRowCount;
    int maxColumnCount = workbook.MaxColumnCount;
    if (num1 <= 0 || num1 > maxRowCount || num2 <= 0 || num2 > maxColumnCount)
      return;
    IBorders borders1 = sheet[row + xDelta, column + yDelta].Borders;
    bool flag = borders[end].LineStyle == ExcelLineStyle.Double || borders1[start].LineStyle == ExcelLineStyle.Double;
    if (isInvertCondition)
      flag = !flag;
    int num3 = end == (ExcelBordersIndex) -1 || !flag ? (isLineStart ? 1 : -1) : (isLineStart ? -1 : 1);
    if (yDelta != 0)
    {
      xDelta1 = num3;
    }
    else
    {
      if (xDelta == 0)
        return;
      yDelta1 = num3;
    }
  }

  private void GetStartEndBorderIndex(
    ExcelBordersIndex borderIndex,
    out ExcelBordersIndex start,
    out ExcelBordersIndex end)
  {
    start = (ExcelBordersIndex) -1;
    end = (ExcelBordersIndex) -1;
    switch (borderIndex)
    {
      case ExcelBordersIndex.EdgeLeft:
      case ExcelBordersIndex.EdgeRight:
        start = ExcelBordersIndex.EdgeTop;
        end = ExcelBordersIndex.EdgeBottom;
        break;
      case ExcelBordersIndex.EdgeTop:
      case ExcelBordersIndex.EdgeBottom:
        start = ExcelBordersIndex.EdgeLeft;
        end = ExcelBordersIndex.EdgeRight;
        break;
    }
  }

  private void DrawOrdinaryBorder(
    IBorder border,
    float x1,
    float y1,
    float x2,
    float y2,
    Graphics graphics)
  {
    if (border.LineStyle == ExcelLineStyle.None)
      return;
    using (Pen pen = this.CreatePen(border))
      graphics.DrawLine(pen, x1, y1, x2, y2);
  }

  private Pen CreatePen(IBorder border)
  {
    if (border.LineStyle == ExcelLineStyle.Hair && (border.Color == ExcelKnownColors.Black || border.Color == ExcelKnownColors.BlackCustom))
      border.Color = ExcelKnownColors.Grey_25_percent;
    return new Pen(this.NormalizeColor(border.ColorRGB), this.GetBorderWidth(border))
    {
      DashStyle = this.GetDashStyle(border)
    };
  }

  private DashStyle GetDashStyle(IBorder border)
  {
    DashStyle dashStyle = DashStyle.Solid;
    switch (border.LineStyle)
    {
      case ExcelLineStyle.Thin:
      case ExcelLineStyle.Medium:
      case ExcelLineStyle.Thick:
      case ExcelLineStyle.Double:
      case ExcelLineStyle.Hair:
        dashStyle = DashStyle.Solid;
        break;
      case ExcelLineStyle.Dashed:
      case ExcelLineStyle.Medium_dashed:
        dashStyle = DashStyle.Dash;
        break;
      case ExcelLineStyle.Dotted:
        dashStyle = DashStyle.Dot;
        break;
      case ExcelLineStyle.Dash_dot:
      case ExcelLineStyle.Medium_dash_dot:
      case ExcelLineStyle.Slanted_dash_dot:
        dashStyle = DashStyle.DashDot;
        break;
      case ExcelLineStyle.Dash_dot_dot:
      case ExcelLineStyle.Medium_dash_dot_dot:
        dashStyle = DashStyle.DashDotDot;
        break;
    }
    return dashStyle;
  }

  private float GetBorderWidth(IBorder border)
  {
    float borderWidth = 0.0f;
    switch (border.LineStyle)
    {
      case ExcelLineStyle.None:
      case ExcelLineStyle.Hair:
        borderWidth = 0.5f;
        break;
      case ExcelLineStyle.Thin:
      case ExcelLineStyle.Dashed:
      case ExcelLineStyle.Dotted:
      case ExcelLineStyle.Double:
      case ExcelLineStyle.Dash_dot:
      case ExcelLineStyle.Dash_dot_dot:
      case ExcelLineStyle.Slanted_dash_dot:
        borderWidth = 1f;
        break;
      case ExcelLineStyle.Medium:
      case ExcelLineStyle.Medium_dashed:
      case ExcelLineStyle.Medium_dash_dot:
      case ExcelLineStyle.Medium_dash_dot_dot:
        borderWidth = 2f;
        break;
      case ExcelLineStyle.Thick:
        borderWidth = 3f;
        break;
    }
    return borderWidth;
  }

  private Image CreateImage(
    int width,
    int height,
    ImageType imageType,
    Stream outputStream,
    EmfType emfType)
  {
    int num = 1;
    Image image1;
    switch (imageType)
    {
      case ImageType.Bitmap:
        image1 = (Image) new Bitmap(width * num + 1, height * num + 1);
        break;
      case ImageType.Metafile:
        if (outputStream == null)
          outputStream = (Stream) new MemoryStream();
        using (Image image2 = (Image) new Bitmap(width, height))
        {
          using (Graphics graphics = Graphics.FromImage(image2))
          {
            IntPtr hdc = graphics.GetHdc();
            Rectangle frameRect = new Rectangle(0, 0, width * num + 1, height * num + 1);
            image1 = (Image) new Metafile(outputStream, hdc, frameRect, MetafileFrameUnit.Pixel, emfType);
            graphics.ReleaseHdc();
            break;
          }
        }
      default:
        throw new ArgumentOutOfRangeException(nameof (imageType));
    }
    return image1;
  }

  private delegate void MergeMethod(
    WorksheetImpl sheet,
    MergeCellsRecord.MergedRegion mergedRegion,
    int firstRow,
    int firstColumn,
    Graphics graphics,
    ItemSizeHelper rowHeightGetter,
    ItemSizeHelper columnWidthGetter);

  private delegate void CellMethod(IRange cell, Rectangle rect, Graphics graphics);
}
