// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Grid.PdfGridCell
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Graphics.Images;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Grid;

public class PdfGridCell
{
  private float m_width = float.MinValue;
  private float m_height = float.MinValue;
  private int m_rowSpan;
  private int m_colSpan;
  private PdfGridRow m_row;
  private PdfGridCellStyle m_style;
  private object m_value;
  private PdfStringFormat m_format;
  private bool m_bIsCellMergeStart;
  private bool m_bIsCellMergeContinue;
  private bool m_bIsRowMergeStart;
  private bool m_bIsRowMergeContinue;
  private bool m_finsh = true;
  private string m_remainingString;
  internal bool present;
  private PdfGridCell m_parent;
  private float rowSpanRemainingHeight;
  private bool isHtmlText;
  internal int m_pageCount;
  private bool m_isImageDrawn;
  private float m_outerCellWidth = float.MinValue;
  private PdfGridImagePosition m_imagePosition = PdfGridImagePosition.Stretch;
  private PdfGridStretchOption m_pdfGridStretchOption = PdfGridStretchOption.None;
  internal float m_rowSpanRemainingHeight;
  internal RectangleF layoutBounds;
  internal PdfLayoutResult layoutResult;
  internal float tempHeight;
  private PdfTag m_tag;
  internal bool cellBorderCuttOffX;
  internal bool cellBorderCuttOffY;
  internal RectangleF parentLayoutBounds = new RectangleF();

  public float Width
  {
    get
    {
      if ((double) this.m_width == -3.4028234663852886E+38 || this.Row.Grid.isComplete)
        this.m_width = this.MeasureWidth();
      return (float) Math.Round((double) this.m_width, 4);
    }
    internal set => this.m_width = value;
  }

  public float Height
  {
    get
    {
      if ((double) this.m_height == -3.4028234663852886E+38)
        this.m_height = this.MeasureHeight();
      return this.m_height;
    }
    internal set => this.m_height = value;
  }

  public int RowSpan
  {
    get => this.m_rowSpan;
    set
    {
      if (value < 1)
        throw new ArgumentException("Invalid span specified, must be greater than or equal to 1");
      if (value <= 1)
        return;
      this.m_rowSpan = value;
      this.Row.RowSpanExists = true;
      this.Row.Grid.m_hasRowSpanSpan = true;
    }
  }

  public int ColumnSpan
  {
    get => this.m_colSpan;
    set
    {
      if (value < 1)
        throw new ArgumentException("Invalid span specified, must be greater than or equal to 1");
      if (value <= 1)
        return;
      this.m_colSpan = value;
      this.Row.ColumnSpanExists = true;
      this.Row.Grid.m_hasColumnSpan = true;
    }
  }

  public PdfGridCellStyle Style
  {
    get
    {
      if (this.m_style == null)
        this.m_style = new PdfGridCellStyle();
      return this.m_style;
    }
    set => this.m_style = value;
  }

  public bool IsHtmlText
  {
    get => this.isHtmlText;
    set
    {
      this.isHtmlText = value;
      this.Row.Grid.m_hasHTMLText = value;
    }
  }

  public object Value
  {
    get
    {
      if (this.m_value != null && this.IsHtmlText && !string.IsNullOrEmpty(this.m_value.ToString()) && !(this.m_value is PdfHTMLTextElement))
      {
        PdfFont font = this.m_style.Font == null ? PdfDocument.DefaultFont : this.m_style.Font;
        PdfBrush brush = this.m_style.TextBrush == null ? PdfBrushes.Black : this.m_style.TextBrush;
        this.m_value = (object) new PdfHTMLTextElement(this.m_value.ToString(), font, brush);
      }
      return this.m_value;
    }
    set
    {
      this.m_value = value;
      if (!(this.m_value is PdfGrid))
        return;
      this.Row.Grid.isSignleGrid = false;
      PdfGrid pdfGrid = this.m_value as PdfGrid;
      pdfGrid.ParentCell = this;
      (this.m_value as PdfGrid).IsChildGrid = true;
      foreach (PdfGridRow row in (List<PdfGridRow>) pdfGrid.Rows)
      {
        foreach (PdfGridCell cell in row.Cells)
          cell.m_parent = this;
      }
    }
  }

  public PdfStringFormat StringFormat
  {
    get
    {
      if (this.m_format == null)
        this.m_format = new PdfStringFormat();
      return this.m_format;
    }
    set => this.m_format = value;
  }

  internal PdfGridRow Row
  {
    get => this.m_row;
    set => this.m_row = value;
  }

  internal bool IsCellMergeContinue
  {
    get => this.m_bIsCellMergeContinue;
    set => this.m_bIsCellMergeContinue = value;
  }

  internal bool IsCellMergeStart
  {
    get => this.m_bIsCellMergeStart;
    set => this.m_bIsCellMergeStart = value;
  }

  internal bool IsRowMergeStart
  {
    get => this.m_bIsRowMergeStart;
    set => this.m_bIsRowMergeStart = value;
  }

  internal bool IsRowMergeContinue
  {
    get => this.m_bIsRowMergeContinue;
    set => this.m_bIsRowMergeContinue = value;
  }

  internal PdfGridCell NextCell => this.ObtainNextCell();

  internal string RemainingString
  {
    get => this.m_remainingString;
    set => this.m_remainingString = value;
  }

  internal bool FinishedDrawingCell
  {
    get => this.m_finsh;
    set => this.m_finsh = value;
  }

  public PdfGridImagePosition ImagePosition
  {
    get => this.m_imagePosition;
    set => this.m_imagePosition = value;
  }

  internal PdfGridStretchOption StretchOption
  {
    get => this.m_pdfGridStretchOption;
    set => this.m_pdfGridStretchOption = value;
  }

  public PdfTag PdfTag
  {
    get => this.m_tag;
    set => this.m_tag = value;
  }

  public PdfGridCell()
  {
    this.m_rowSpan = 1;
    this.m_colSpan = 1;
  }

  public PdfGridCell(PdfGridRow row)
    : this()
  {
    this.m_row = row;
  }

  internal PdfStringLayoutResult Draw(
    PdfGraphics graphics,
    RectangleF bounds,
    bool cancelSubsequentSpans)
  {
    PdfDictionary pdfDictionary = new PdfDictionary();
    pdfDictionary.SetName("O", "Table");
    pdfDictionary.SetNumber("RowSpan", this.RowSpan);
    pdfDictionary.SetNumber("ColSpan", this.ColumnSpan);
    graphics.TableSpan = pdfDictionary;
    bool flag = false;
    if (!this.Row.Grid.isSignleGrid)
    {
      if (this.m_remainingString != null || PdfGridLayouter.m_repeatRowIndex != -1)
        this.DrawParentCells(graphics, bounds, true);
      else if (this.Row.Grid.Rows.Count > 1)
      {
        for (int index = 0; index < this.Row.Grid.Rows.Count; ++index)
        {
          if (this.Row == this.Row.Grid.Rows[index])
          {
            if ((double) this.Row.Grid.Rows[index].RowBreakHeight > 0.0)
              flag = true;
            if (index > 0 && flag)
              this.DrawParentCells(graphics, bounds, false);
          }
        }
      }
    }
    PdfStringLayoutResult stringLayoutResult = (PdfStringLayoutResult) null;
    if (cancelSubsequentSpans)
    {
      int num = this.Row.Cells.IndexOf(this);
      for (int index = num + 1; index <= num + this.m_colSpan; ++index)
      {
        if (index < this.Row.Cells.Count)
        {
          this.Row.Cells[index].IsCellMergeContinue = false;
          this.Row.Cells[index].IsRowMergeContinue = false;
        }
      }
      this.m_colSpan = 1;
    }
    if ((this.m_bIsCellMergeContinue || this.m_bIsRowMergeContinue) && (!this.m_bIsCellMergeContinue || !this.Row.Grid.Style.AllowHorizontalOverflow || this.Row.RowOverflowIndex > 0 && this.Row.Cells.IndexOf(this) != this.Row.RowOverflowIndex + 1 || this.Row.RowOverflowIndex == 0 && this.m_bIsCellMergeContinue))
      return stringLayoutResult;
    bounds = this.AdjustOuterLayoutArea(bounds, graphics);
    this.DrawCellBackground(ref graphics, bounds);
    PdfPen textPen = this.GetTextPen();
    PdfBrush textBrush = this.GetTextBrush();
    PdfFont textFont = this.GetTextFont();
    PdfStringFormat stringFormat = this.ObtainStringFormat();
    graphics.Tag = this.PdfTag;
    RectangleF bounds1 = bounds;
    if ((double) bounds1.Height >= (double) graphics.ClientSize.Height)
    {
      if (this.Row.Grid.AllowRowBreakAcrossPages)
      {
        bounds1.Height -= bounds1.Y;
        bounds.Height -= bounds.Y;
        if (this.Row.Grid.IsChildGrid)
          bounds1.Height -= this.Row.Grid.ParentCell.Row.Grid.Style.CellPadding.Bottom;
      }
      else
      {
        bounds1.Height = graphics.ClientSize.Height;
        bounds.Height = graphics.ClientSize.Height;
      }
    }
    if (this.Value is PdfGrid && this.cellBorderCuttOffX)
      bounds1.Width -= this.Style.Borders.Left.Width / 2f;
    if (this.Value is PdfGrid && this.cellBorderCuttOffY)
      bounds1.Height -= this.Style.Borders.Top.Width / 2f;
    RectangleF rectangleF = this.AdjustContentLayoutArea(bounds1);
    if (this.Value is PdfGrid)
    {
      graphics.Save();
      graphics.SetClip(new RectangleF(rectangleF.X, rectangleF.Y, bounds.Width, bounds.Height));
      double width1 = (double) (this.Value as PdfGrid).Size.Width;
      double width2 = (double) rectangleF.Size.Width;
      if (PdfCatalog.StructTreeRoot != null)
        PdfCatalog.StructTreeRoot.m_isChildGrid = true;
      PdfGrid grid = this.Value as PdfGrid;
      grid.IsChildGrid = true;
      grid.ParentCell = this;
      grid.m_listOfNavigatePages = new List<int>();
      PdfGridLayouter pdfGridLayouter = new PdfGridLayouter(grid);
      PdfLayoutFormat pdfLayoutFormat = (PdfLayoutFormat) new PdfGridLayoutFormat();
      if (this.Row.Grid.LayoutFormat != null)
        pdfLayoutFormat = this.Row.Grid.LayoutFormat;
      else
        pdfLayoutFormat.Layout = PdfLayoutType.Paginate;
      if (graphics.Layer != null)
      {
        PdfLayoutParams pdfLayoutParams = new PdfLayoutParams();
        pdfLayoutParams.Page = graphics.Page as PdfPage;
        grid.ParentCell.parentLayoutBounds = bounds;
        pdfLayoutParams.Bounds = rectangleF;
        pdfLayoutParams.Format = pdfLayoutFormat;
        grid.SetSpan();
        PdfLayoutResult pdfLayoutResult = pdfGridLayouter.Layout(pdfLayoutParams);
        this.Value = (object) grid;
        if (pdfLayoutParams.Page != pdfLayoutResult.Page)
        {
          this.Row.NestedGridLayoutResult = pdfLayoutResult;
          bounds.Height = graphics.ClientSize.Height - bounds.Y;
        }
      }
      else
      {
        grid.SetSpan();
        new PdfGridLayouter(this.Value as PdfGrid).Layout(graphics, rectangleF);
      }
      graphics.Restore();
    }
    else if (this.Value is PdfTextElement)
    {
      PdfTextElement pdfTextElement = this.Value as PdfTextElement;
      PdfPage page = graphics.Page as PdfPage;
      pdfTextElement.ispdfTextElement = true;
      string text = pdfTextElement.Text;
      PdfTextLayoutResult textLayoutResult;
      if (this.m_finsh)
      {
        textLayoutResult = pdfTextElement.Draw(page, rectangleF) as PdfTextLayoutResult;
      }
      else
      {
        pdfTextElement.Text = this.RemainingString;
        textLayoutResult = pdfTextElement.Draw(page, rectangleF) as PdfTextLayoutResult;
      }
      if (textLayoutResult.Remainder != null && textLayoutResult.Remainder != string.Empty)
      {
        this.RemainingString = textLayoutResult.Remainder;
        this.FinishedDrawingCell = false;
      }
      else if ((double) this.Row.RowBreakHeight <= 0.0)
      {
        this.RemainingString = (string) null;
        this.FinishedDrawingCell = true;
      }
      else
        this.RemainingString = string.Empty;
      pdfTextElement.Text = text;
    }
    else if (this.Value is PdfDocumentLinkAnnotation)
    {
      PdfPage page = graphics.Page as PdfPage;
      PdfDocumentLinkAnnotation annotation = this.Value as PdfDocumentLinkAnnotation;
      annotation.Bounds = bounds;
      page.Annotations.Add((PdfAnnotation) annotation);
      this.Value = (object) annotation.Text;
      graphics.DrawString(this.Value.ToString(), textFont, textBrush, rectangleF, stringFormat);
    }
    else if (this.Value is PdfUriAnnotation)
    {
      PdfPage page = graphics.Page as PdfPage;
      PdfUriAnnotation annotation = this.Value as PdfUriAnnotation;
      annotation.Bounds = bounds;
      page.Annotations.Add((PdfAnnotation) annotation);
      this.Value = (object) annotation.Text;
      graphics.DrawString(this.Value.ToString(), textFont, textBrush, rectangleF, stringFormat);
    }
    else if (this.Value is PdfFileLinkAnnotation)
    {
      PdfPage page = graphics.Page as PdfPage;
      PdfFileLinkAnnotation annotation = this.Value as PdfFileLinkAnnotation;
      annotation.Bounds = bounds;
      page.Annotations.Add((PdfAnnotation) annotation);
      this.Value = (object) annotation.Text;
      graphics.DrawString(this.Value.ToString(), textFont, textBrush, rectangleF, stringFormat);
    }
    else if (this.Value is PdfHTMLTextElement)
    {
      PdfPage page = graphics.Page as PdfPage;
      PdfMetafileLayoutFormat format = new PdfMetafileLayoutFormat();
      format.Break = PdfLayoutBreakType.FitPage;
      format.SplitTextLines = false;
      textLayoutResult = (PdfTextLayoutResult) null;
      PdfHTMLTextElement pdfHtmlTextElement = this.Value as PdfHTMLTextElement;
      pdfHtmlTextElement.m_bottomCellpadding = this.Style.CellPadding == null ? this.m_row.Grid.Style.CellPadding.Bottom : this.Style.CellPadding.Bottom;
      if (this.layoutResult == null)
      {
        pdfHtmlTextElement.shapeBounds = RectangleF.Empty;
        pdfHtmlTextElement.m_isPdfGrid = true;
        this.layoutResult = pdfHtmlTextElement.Draw(page, rectangleF.Location, rectangleF.Width, format);
        this.tempHeight = this.layoutResult.Bounds.Height;
        if (this.layoutResult is PdfTextLayoutResult textLayoutResult && textLayoutResult.Remainder == null && (double) this.tempHeight > 0.0 && this.Row != null && (double) this.Row.RowBreakHeight > 0.0)
        {
          this.Value = (object) string.Empty;
          this.RemainingString = string.Empty;
        }
      }
      else
      {
        if (!this.Row.IsHeaderRow || this.Row.IsHeaderRow && (double) pdfHtmlTextElement.Height > (double) graphics.ClientSize.Height)
          pdfHtmlTextElement.shapeBounds = new RectangleF(this.layoutResult.Bounds.X, this.tempHeight, 0.0f, 0.0f);
        this.layoutResult = pdfHtmlTextElement.Draw(page, rectangleF.Location, rectangleF.Width, format);
        this.tempHeight += this.layoutResult.Bounds.Height;
      }
      this.FinishedDrawingCell = (double) pdfHtmlTextElement.Height == (double) this.tempHeight || textLayoutResult != null && textLayoutResult.Remainder == null && (double) this.tempHeight > 0.0;
    }
    else if (this.Value is string || this.m_remainingString != null)
    {
      RectangleF layoutRectangle = !((Decimal) rectangleF.Height < (Decimal) textFont.Height) ? rectangleF : new RectangleF(rectangleF.X, rectangleF.Y, rectangleF.Width, textFont.Height);
      if ((double) rectangleF.Height < (double) textFont.Height && this.Row.Grid.IsChildGrid && this.Row.Grid.ParentCell != null)
      {
        float num = layoutRectangle.Height - this.Row.Grid.ParentCell.Row.Grid.Style.CellPadding.Bottom - this.Row.Grid.Style.CellPadding.Bottom;
        if ((double) num > 0.0 && (double) num < (double) textFont.Height)
          layoutRectangle.Height = num;
        else if ((double) num + (double) this.Row.Grid.Style.CellPadding.Bottom > 0.0 && (double) num + (double) this.Row.Grid.Style.CellPadding.Bottom < (double) textFont.Height)
          layoutRectangle.Height = num + this.Row.Grid.Style.CellPadding.Bottom;
        else if ((double) bounds.Height < (double) textFont.Height)
          layoutRectangle.Height = bounds.Height;
        else if ((double) bounds.Height - (double) this.Row.Grid.ParentCell.Row.Grid.Style.CellPadding.Bottom < (double) textFont.Height)
          layoutRectangle.Height = bounds.Height - this.Row.Grid.ParentCell.Row.Grid.Style.CellPadding.Bottom;
      }
      if (this.Row.Grid.AllowRowBreakAcrossPages && this.Row.Grid.IsChildGrid && (double) this.Row.RowBreakHeight > 0.0)
        layoutRectangle.Height -= this.Row.Grid.ParentCell.Row.Grid.Style.CellPadding.Bottom / 2f;
      if (this.Style.CellPadding != null && (double) this.Style.CellPadding.Bottom == 0.0 && (double) this.Style.CellPadding.Left == 0.0 && (double) this.Style.CellPadding.Right == 0.0 && (double) this.Style.CellPadding.Top == 0.0)
        layoutRectangle.Width -= this.Style.Borders.Left.Width + this.Style.Borders.Right.Width;
      if (this.m_finsh)
      {
        string s = this.m_remainingString == string.Empty ? this.m_remainingString : (string) this.Value;
        graphics.DrawString(s, textFont, textPen, textBrush, layoutRectangle, stringFormat);
      }
      else
        graphics.DrawString(this.m_remainingString, textFont, textPen, textBrush, layoutRectangle, stringFormat);
      stringLayoutResult = graphics.StringLayoutResult;
      if (stringLayoutResult != null && this.Row.Grid.IsChildGrid && (double) this.Row.RowBreakHeight > 0.0)
        bounds.Height -= this.Row.Grid.ParentCell.Row.Grid.Style.CellPadding.Bottom;
    }
    else if (this.m_value is PdfImage)
    {
      if (this.m_imagePosition == PdfGridImagePosition.Stretch)
      {
        if (this.Style.CellPadding != null && this.Style.CellPadding != new PdfPaddings())
        {
          PdfPaddings cellPadding = this.Style.CellPadding;
          bounds = new RectangleF(bounds.X + cellPadding.Left, bounds.Y + cellPadding.Top, bounds.Width - (cellPadding.Left + cellPadding.Right), bounds.Height - (cellPadding.Top + cellPadding.Bottom));
        }
        else if (this.Row.Grid.Style.CellPadding != null && this.Row.Grid.Style.CellPadding != new PdfPaddings())
        {
          PdfPaddings cellPadding = this.Row.Grid.Style.CellPadding;
          bounds = new RectangleF(bounds.X + cellPadding.Left, bounds.Y + cellPadding.Top, bounds.Width - (cellPadding.Left + cellPadding.Right), bounds.Height - (cellPadding.Top + cellPadding.Bottom));
        }
        graphics.IsTemplateGraphics = false;
        PdfImage image1 = this.m_value as PdfImage;
        float width = (float) image1.Width;
        float height = (float) image1.Height;
        if (this.m_pdfGridStretchOption == PdfGridStretchOption.Uniform || this.m_pdfGridStretchOption == PdfGridStretchOption.UniformToFill)
        {
          if ((double) width > (double) bounds.Width)
          {
            float num = width / bounds.Width;
            width = bounds.Width;
            height /= num;
          }
          if ((double) height > (double) bounds.Height)
          {
            float num = height / bounds.Height;
            height = bounds.Height;
            width /= num;
          }
          if ((double) width < (double) bounds.Width && (double) height < (double) bounds.Height)
          {
            if ((double) (bounds.Width - width) < (double) (bounds.Height - height))
            {
              float num = width / bounds.Width;
              width = bounds.Width;
              height /= num;
            }
            else
            {
              float num = height / bounds.Height;
              height = bounds.Height;
              width /= num;
            }
          }
        }
        if (this.m_pdfGridStretchOption == PdfGridStretchOption.Fill || this.m_pdfGridStretchOption == PdfGridStretchOption.None)
        {
          width = bounds.Width;
          height = bounds.Height;
        }
        if (this.m_pdfGridStretchOption == PdfGridStretchOption.UniformToFill)
        {
          if ((double) width == (double) bounds.Width && (double) height < (double) bounds.Height)
          {
            float num = height / bounds.Height;
            height = bounds.Height;
            width /= num;
          }
          if ((double) height == (double) bounds.Height && (double) width < (double) bounds.Width)
          {
            float num = width / bounds.Width;
            width = bounds.Width;
            height /= num;
          }
          PdfBitmap image2 = new PdfBitmap(image1.InternalImage);
          PdfPage page = graphics.Page as PdfPage;
          PdfGraphicsState state = page.Graphics.Save();
          page.Graphics.SetClip(new RectangleF(bounds.X, bounds.Y, bounds.Width, bounds.Height));
          page.Graphics.DrawImage((PdfImage) image2, bounds.X, bounds.Y, width, height);
          page.Graphics.Restore(state);
        }
        else
        {
          float x = bounds.X;
          float y = bounds.Y;
          graphics.DrawImage(image1, x, y, width, height);
        }
        this.m_isImageDrawn = true;
      }
      graphics.Save();
      PdfPage page1 = graphics.Page as PdfPage;
      PdfImage pdfImage = this.m_value as PdfImage;
      PdfMetafileLayoutFormat format = new PdfMetafileLayoutFormat();
      format.Layout = PdfLayoutType.Paginate;
      int count = page1.Section.Count;
      if (!this.m_isImageDrawn)
      {
        PdfLayoutResult pdfLayoutResult = pdfImage.Draw(page1, new PointF(bounds.X, bounds.Y), (PdfLayoutFormat) format);
        this.m_isImageDrawn = true;
        if ((double) pdfImage.Height > (double) bounds.Height)
        {
          if (this.Row.m_noOfPageCount < page1.Section.Count - count)
            this.Row.m_noOfPageCount = page1.Section.Count - count;
          this.Row.RowBreakHeight = pdfLayoutResult.Bounds.Height;
        }
      }
    }
    if (this.Style.Borders != null && this.Style.Borders.Left != null)
    {
      if (graphics.Tag != null)
        graphics.Tag = (PdfTag) new PdfArtifact();
      this.DrawCellBorders(ref graphics, bounds);
      this.layoutBounds = bounds;
    }
    graphics.Tag = (PdfTag) null;
    return stringLayoutResult;
  }

  private void DrawParentCells(PdfGraphics graphics, RectangleF bounds, bool b)
  {
    PointF location = new PointF();
    if ((double) bounds.Height < (double) graphics.ClientSize.Height && b)
      bounds.Height += bounds.Y - location.Y;
    RectangleF bounds1 = new RectangleF(location, new SizeF(bounds.Width, bounds.Height));
    if (!b)
    {
      bounds1.Y = bounds.Y;
      bounds1.Height = bounds.Height;
    }
    PdfGridCellStyle pdfGridCellStyle = this.Style.Clone() as PdfGridCellStyle;
    this.MemberwiseClone();
    PdfGridCell pdfGridCell = this;
    if (this.m_parent != null)
    {
      if (pdfGridCell.Row.Grid.Rows.Count == 1 && pdfGridCell.Row.Grid.Rows[0].Cells.Count == 1)
      {
        pdfGridCell.Row.Grid.Rows[0].Cells[0].present = true;
      }
      else
      {
        foreach (PdfGridRow row in (List<PdfGridRow>) pdfGridCell.Row.Grid.Rows)
        {
          if (row == pdfGridCell.Row)
          {
            foreach (PdfGridCell cell in this.Row.Cells)
            {
              if (cell == pdfGridCell)
              {
                cell.present = true;
                break;
              }
            }
          }
        }
      }
      while (pdfGridCell.m_parent != null)
      {
        pdfGridCell = pdfGridCell.m_parent;
        pdfGridCell.present = true;
        bounds1.X += pdfGridCell.Row.Grid.Style.CellPadding.Left;
        if (pdfGridCell.Row.Cells.Count > 0)
          bounds1.X += pdfGridCell.Row.Cells[0].Style.Borders.Left.Width;
      }
    }
    if ((double) bounds.X >= (double) bounds1.X)
    {
      bounds1.X -= bounds.X;
      if ((double) bounds1.X < 0.0)
        bounds1.X = bounds.X;
    }
    PdfGrid grid = pdfGridCell.Row.Grid;
    for (int index1 = 0; index1 < grid.Rows.Count; ++index1)
    {
      for (int index2 = 0; index2 < grid.Rows[index1].Cells.Count; ++index2)
      {
        if (grid.Rows[index1].Cells[index2].present)
        {
          int num1 = 0;
          if (grid.Rows[index1].Style.BackgroundBrush != null)
          {
            this.Style.BackgroundBrush = grid.Rows[index1].Style.BackgroundBrush;
            float num2 = 0.0f;
            if (index2 > 0)
            {
              for (int index3 = 0; index3 < index2; ++index3)
                num2 += grid.Columns[index3].Width;
            }
            bounds1.Width = grid.Rows[index1].Width - num2;
            if (grid.Rows[index1].Cells[index2].Value is PdfGrid pdfGrid)
            {
              for (int index4 = 0; index4 < pdfGrid.Rows.Count; ++index4)
              {
                for (int index5 = 0; index5 < pdfGrid.Rows[index4].Cells.Count; ++index5)
                {
                  if (pdfGrid.Rows[index4].Cells[index5].present && index5 > 0)
                  {
                    bounds1.Width = pdfGrid.Rows[index4].Cells[index5].Width;
                    num1 = index5;
                  }
                }
              }
            }
            this.DrawCellBackground(ref graphics, bounds1);
          }
          grid.Rows[index1].Cells[index2].present = false;
          if (grid.Rows[index1].Cells[index2].Style.BackgroundBrush != null)
          {
            this.Style.BackgroundBrush = grid.Rows[index1].Cells[index2].Style.BackgroundBrush;
            if (num1 == 0)
              bounds1.Width = grid.Columns[index2].Width;
            if ((double) bounds1.X == 0.0 && grid.Rows[index1].Cells[index2].Style != null && grid.Rows[index1].Cells[index2].Style.Borders != null && grid.Rows[index1].Cells[index2].Style.Borders.Left != null)
            {
              bounds1.X = grid.Rows[index1].Cells[index2].Style.Borders.Left.Width / 2f;
              bounds1.Width -= grid.Rows[index1].Cells[index2].Style.Borders.Left.Width / 2f;
            }
            if ((double) bounds1.Y == 0.0 && grid.Rows[index1].Cells[index2].Style != null && grid.Rows[index1].Cells[index2].Style.Borders != null && grid.Rows[index1].Cells[index2].Style.Borders.Top != null)
            {
              bounds1.Y = grid.Rows[index1].Cells[index2].Style.Borders.Top.Width / 2f;
              if (grid.Rows[index1].m_paginatedGridRow)
                bounds1.Height += grid.Rows[index1].Cells[index2].Style.Borders.Top.Width / 2f;
              else
                bounds1.Height -= grid.Rows[index1].Cells[index2].Style.Borders.Top.Width / 2f;
            }
            this.DrawCellBackground(ref graphics, bounds1);
          }
          if (grid.Rows[index1].Cells[index2].Value is PdfGrid)
          {
            if (grid.Style != null && !grid.Rows[index1].isrowFinish && num1 == 0)
              bounds1.X += grid.Style.CellPadding.Left;
            grid = grid.Rows[index1].Cells[index2].Value as PdfGrid;
            if (grid.Style.BackgroundBrush != null)
            {
              this.Style.BackgroundBrush = grid.Style.BackgroundBrush;
              if (num1 == 0 && index2 < grid.Columns.Count)
                bounds1.Width = grid.Columns[index2].Width;
              this.DrawCellBackground(ref graphics, bounds1);
            }
            index1 = -1;
            break;
          }
        }
      }
    }
    if ((double) bounds.Height < (double) graphics.ClientSize.Height)
      bounds.Height -= bounds.Y - location.Y;
    this.Style = pdfGridCellStyle;
  }

  internal PdfGridCell Clone(PdfGridCell gridcell) => (PdfGridCell) gridcell.MemberwiseClone();

  private float MeasureWidth()
  {
    float num = 0.0f;
    PdfStringLayouter pdfStringLayouter = new PdfStringLayouter();
    if (this.Value is string)
    {
      float width = float.MaxValue;
      if (this.m_parent != null)
      {
        width = this.GetColumnWidth();
        if (this.Row.Grid.LayoutFormat == null)
        {
          if (this.Style.CellPadding != null)
            width -= this.Style.CellPadding.Left + this.Style.CellPadding.Right;
          width -= this.Row.Grid.Style.CellSpacing;
        }
      }
      PdfStringLayoutResult stringLayoutResult = pdfStringLayouter.Layout((string) this.Value, this.GetTextFont(), this.StringFormat, new SizeF(width, float.MaxValue));
      num += stringLayoutResult.ActualSize.Width;
      if (this.Style.Borders != null && this.Style.Borders.Left != null && this.Style.Borders.Right != null)
        num += (float) (((double) this.Style.Borders.Left.Width + (double) this.Style.Borders.Right.Width) * 2.0);
    }
    else if (this.Value is PdfGrid)
      num = (this.Value as PdfGrid).Size.Width;
    else if (this.Value is PdfHTMLTextElement)
    {
      if (!this.Row.Grid.Style.AllowHorizontalOverflow)
      {
        num = this.Row.Width / (float) this.Row.Cells.Count;
      }
      else
      {
        PdfHTMLTextElement pdfHtmlTextElement = this.Value as PdfHTMLTextElement;
        RichTextBoxExt richTextBoxExt = new RichTextBoxExt();
        richTextBoxExt.RenderHTML(pdfHtmlTextElement.HTMLText, pdfHtmlTextElement.Font, pdfHtmlTextElement.Brush);
        PdfStringLayoutResult stringLayoutResult = pdfStringLayouter.Layout(richTextBoxExt.Text, pdfHtmlTextElement.Font ?? this.GetTextFont(), this.StringFormat, new SizeF(float.MaxValue, float.MaxValue));
        num += stringLayoutResult.ActualSize.Width;
      }
    }
    else if (this.Value is PdfTextElement)
    {
      float width = float.MaxValue;
      if (this.m_parent != null)
        width = this.GetColumnWidth();
      PdfTextElement pdfTextElement = this.Value as PdfTextElement;
      string text = pdfTextElement.Text;
      if (!this.m_finsh)
        text = !string.IsNullOrEmpty(this.m_remainingString) ? this.m_remainingString : (string) this.Value;
      PdfStringLayoutResult stringLayoutResult = pdfStringLayouter.Layout(text, pdfTextElement.Font ?? this.GetTextFont(), pdfTextElement.StringFormat ?? this.StringFormat, new SizeF(width, float.MaxValue));
      num = num + stringLayoutResult.ActualSize.Width + (float) (((double) this.Style.Borders.Left.Width + (double) this.Style.Borders.Right.Width) * 2.0);
    }
    return (this.Style.CellPadding == null ? num + (this.Row.Grid.Style.CellPadding.Left + this.Row.Grid.Style.CellPadding.Right) : num + (this.Style.CellPadding.Left + this.Style.CellPadding.Right)) + this.Row.Grid.Style.CellSpacing;
  }

  internal float MeasureHeight()
  {
    float width1 = this.CalculateWidth();
    float width2 = this.Style.CellPadding != null ? width1 - (this.Style.CellPadding.Right + this.Style.CellPadding.Left) - (this.Style.Borders.Left.Width + this.Style.Borders.Right.Width) : width1 - (this.m_row.Grid.Style.CellPadding.Right + this.m_row.Grid.Style.CellPadding.Left);
    this.m_outerCellWidth = width2;
    float num1 = 0.0f;
    PdfStringLayouter pdfStringLayouter = new PdfStringLayouter();
    if (this.Value is PdfHTMLTextElement)
    {
      PdfHTMLTextElement pdfHtmlTextElement = this.Value as PdfHTMLTextElement;
      string htmlText = pdfHtmlTextElement.HTMLText;
      pdfHtmlTextElement.HTMLText = htmlText;
      PdfUnitConvertor pdfUnitConvertor = new PdfUnitConvertor();
      RichTextBoxExt richTextBoxExt = new RichTextBoxExt();
      richTextBoxExt.RenderHTML(pdfHtmlTextElement.HTMLText, pdfHtmlTextElement.Font, pdfHtmlTextElement.Brush);
      richTextBoxExt.SelectAll();
      richTextBoxExt.SelectionAlignment = pdfHtmlTextElement.TextAlign;
      float num2 = width2 + (this.Style.Borders.Left.Width + this.Style.Borders.Right.Width);
      float width3 = pdfUnitConvertor.ConvertUnits(num2, PdfGraphicsUnit.Point, PdfGraphicsUnit.Pixel);
      if (richTextBoxExt.Text != string.Empty)
      {
        Image image = RtfToImage.ConvertToImage(richTextBoxExt.Rtf, width3, -1f, PdfImageType.Metafile);
        richTextBoxExt.Dispose();
        num1 = pdfUnitConvertor.ConvertFromPixels((float) image.Height, PdfGraphicsUnit.Point) + (float) (((double) this.Style.Borders.Top.Width + (double) this.Style.Borders.Bottom.Width) * 2.0);
      }
    }
    if (this.Value is PdfTextElement)
    {
      PdfTextElement pdfTextElement = this.Value as PdfTextElement;
      string text = pdfTextElement.Text;
      if (!this.m_finsh)
        text = !string.IsNullOrEmpty(this.m_remainingString) ? this.m_remainingString : (string) this.Value;
      PdfStringLayoutResult stringLayoutResult = pdfStringLayouter.Layout(text, pdfTextElement.Font ?? this.GetTextFont(), pdfTextElement.StringFormat ?? this.StringFormat, new SizeF(width2, float.MaxValue));
      num1 = num1 + stringLayoutResult.ActualSize.Height + (float) (((double) this.Style.Borders.Top.Width + (double) this.Style.Borders.Bottom.Width) * 2.0);
    }
    else if (this.Value is string || this.m_remainingString != null)
    {
      string text = (string) this.Value;
      if (!this.m_finsh)
        text = !string.IsNullOrEmpty(this.m_remainingString) ? this.m_remainingString : (string) this.Value;
      PdfStringLayoutResult stringLayoutResult = pdfStringLayouter.Layout(text, this.GetTextFont(), this.StringFormat, new SizeF(width2, float.MaxValue));
      num1 = num1 + stringLayoutResult.ActualSize.Height + (float) (((double) this.Style.Borders.Top.Width + (double) this.Style.Borders.Bottom.Width) * 2.0);
      if (stringLayoutResult.LineCount > 1 && this.Style.StringFormat != null && (double) this.Style.StringFormat.LineSpacing != 0.0)
        num1 += (float) (stringLayoutResult.LineCount - 1) * this.Style.StringFormat.LineSpacing;
    }
    else if (this.Value is PdfGrid)
      num1 = (this.Value as PdfGrid).Size.Height + (float) ((double) this.Style.Borders.Top.Width / 2.0 + (double) this.Style.Borders.Bottom.Width / 2.0);
    else if (this.m_value is PdfImage)
      num1 = new PdfUnitConvertor().ConvertFromPixels((float) (this.m_value as PdfImage).Height, PdfGraphicsUnit.Point);
    return (this.Style.CellPadding != null ? num1 + (this.Style.CellPadding.Top + this.Style.CellPadding.Bottom) : num1 + (this.Row.Grid.Style.CellPadding.Top + this.Row.Grid.Style.CellPadding.Bottom)) + this.Row.Grid.Style.CellSpacing;
  }

  private RectangleF AdjustOuterLayoutArea(RectangleF bounds, PdfGraphics g)
  {
    bool flag = false;
    float cellSpacing = this.Row.Grid.Style.CellSpacing;
    if ((double) cellSpacing > 0.0)
      bounds = new RectangleF(bounds.X + cellSpacing, bounds.Y + cellSpacing, bounds.Width - cellSpacing, bounds.Height - cellSpacing);
    int index1 = this.Row.Cells.IndexOf(this);
    if (this.ColumnSpan > 1 || this.Row.RowOverflowIndex > 0 && index1 == this.Row.RowOverflowIndex + 1 && this.m_bIsCellMergeContinue)
    {
      int columnSpan = this.ColumnSpan;
      if (columnSpan == 1 && this.m_bIsCellMergeContinue)
      {
        for (int index2 = index1 + 1; index2 < this.Row.Grid.Columns.Count && this.Row.Cells[index2].m_bIsCellMergeContinue; ++index2)
          ++columnSpan;
      }
      float num1 = 0.0f;
      for (int index3 = index1; index3 < index1 + columnSpan; ++index3)
      {
        if (this.Row.Grid.Style.AllowHorizontalOverflow)
        {
          float num2 = (double) this.Row.Grid.Size.Width < (double) g.ClientSize.Width ? this.Row.Grid.Size.Width : g.ClientSize.Width;
          if (((double) this.Row.Grid.Size.Width <= (double) g.ClientSize.Width ? (double) (num1 + this.Row.Grid.Columns[index3].Width) : (double) (bounds.X + num1 + this.Row.Grid.Columns[index3].Width)) > (double) num2)
            break;
        }
        num1 += this.Row.Grid.Columns[index3].Width;
      }
      float num3 = num1 - this.Row.Grid.Style.CellSpacing;
      bounds.Width = num3;
      if ((double) bounds.Width > 0.0 && this.cellBorderCuttOffX)
        bounds.Width -= this.Style.Borders.Left.Width / 2f;
    }
    if (this.RowSpan > 1 || this.Row.RowSpanExists)
    {
      int rowSpan = this.RowSpan;
      int num4 = this.Row.Grid.Rows.IndexOf(this.Row);
      if (num4 == -1)
      {
        num4 = this.Row.Grid.Headers.IndexOf(this.Row);
        if (num4 != -1)
          flag = true;
      }
      if (rowSpan == 1 && this.m_bIsCellMergeContinue)
      {
        for (int index4 = num4 + 1; index4 < this.Row.Grid.Rows.Count && (flag ? this.Row.Grid.Headers[index4].Cells[index1].m_bIsCellMergeContinue : this.Row.Grid.Rows[index4].Cells[index1].m_bIsCellMergeContinue); ++index4)
          ++rowSpan;
      }
      float num5 = 0.0f;
      float num6 = 0.0f;
      if (flag)
      {
        for (int index5 = num4; index5 < num4 + rowSpan; ++index5)
          num5 += this.Row.Grid.Headers[index5].Height;
        float num7 = num5 - this.Row.Grid.Style.CellSpacing;
        bounds.Height = num7;
      }
      else
      {
        for (int index6 = num4; index6 < num4 + rowSpan; ++index6)
        {
          if (!this.Row.Grid.Rows[index6].m_isRowSpanRowHeightSet)
            this.Row.Grid.Rows[index6].m_isRowHeightSet = false;
          num5 += flag ? this.Row.Grid.Headers[index6].Height : this.Row.Grid.Rows[index6].Height;
          PdfGridRow row = this.Row.Grid.Rows[index6];
          int num8 = this.Row.Grid.Rows.IndexOf(row);
          if (this.RowSpan > 1)
          {
            foreach (PdfGridCell cell in row.Cells)
            {
              if (cell.RowSpan > 1)
              {
                float num9 = 0.0f;
                for (int index7 = index6; index7 < index6 + cell.RowSpan; ++index7)
                {
                  if (!this.Row.Grid.Rows[index7].m_isRowSpanRowHeightSet)
                    this.Row.Grid.Rows[index7].m_isRowHeightSet = false;
                  num9 += this.Row.Grid.Rows[index7].Height;
                  if (!this.Row.Grid.Rows[index7].m_isRowSpanRowHeightSet)
                    this.Row.Grid.Rows[index7].m_isRowHeightSet = true;
                }
                if ((double) cell.Height > (double) num9 && (double) num6 < (double) cell.Height - (double) num9)
                {
                  num6 = cell.Height - num9;
                  if ((double) this.rowSpanRemainingHeight != 0.0 && (double) num6 > (double) this.rowSpanRemainingHeight)
                    num6 += this.rowSpanRemainingHeight;
                  int index8 = row.Cells.IndexOf(cell);
                  this.Row.Grid.Rows[num8 + cell.RowSpan - 1].Cells[index8].m_rowSpanRemainingHeight = num6;
                  this.rowSpanRemainingHeight = this.Row.Grid.Rows[num8 + cell.RowSpan - 1].Cells[index8].m_rowSpanRemainingHeight;
                }
              }
            }
          }
          if (!this.Row.Grid.Rows[index6].m_isRowSpanRowHeightSet)
            this.Row.Grid.Rows[index6].m_isRowHeightSet = true;
        }
        int index9 = this.Row.Cells.IndexOf(this);
        float num10 = num5 - this.Row.Grid.Style.CellSpacing;
        if ((double) this.Row.Cells[index9].Height > (double) num10 && !this.Row.Grid.Rows[num4 + rowSpan - 1].m_isRowHeightSet)
        {
          this.Row.Grid.Rows[num4 + rowSpan - 1].Cells[index9].m_rowSpanRemainingHeight = this.Row.Cells[index9].Height - num10;
          num10 = this.Row.Cells[index9].Height;
          bounds.Height = num10;
        }
        else
          bounds.Height = num10;
        if (!this.Row.RowMergeComplete)
          bounds.Height = num10;
      }
      if ((double) bounds.Height > 0.0 && this.cellBorderCuttOffY)
        bounds.Height -= this.Style.Borders.Top.Width / 2f;
    }
    return bounds;
  }

  private RectangleF AdjustContentLayoutArea(RectangleF bounds)
  {
    if (this.Value is PdfGrid)
    {
      SizeF size = (this.Value as PdfGrid).Size;
      if (this.Style.CellPadding == null)
      {
        bounds.Width -= this.m_row.Grid.Style.CellPadding.Right + this.m_row.Grid.Style.CellPadding.Left;
        bounds.Height -= this.m_row.Grid.Style.CellPadding.Bottom + this.m_row.Grid.Style.CellPadding.Top;
        if (this.StringFormat.Alignment == PdfTextAlignment.Center)
        {
          bounds.X += this.m_row.Grid.Style.CellPadding.Left + (float) (((double) bounds.Width - (double) size.Width) / 2.0);
          bounds.Y += this.m_row.Grid.Style.CellPadding.Top + (float) (((double) bounds.Height - (double) size.Height) / 2.0);
        }
        else if (this.StringFormat.Alignment == PdfTextAlignment.Left)
        {
          bounds.X += this.m_row.Grid.Style.CellPadding.Left;
          bounds.Y += this.m_row.Grid.Style.CellPadding.Top;
        }
        else if (this.StringFormat.Alignment == PdfTextAlignment.Right)
        {
          bounds.X += this.m_row.Grid.Style.CellPadding.Left + (bounds.Width - size.Width);
          bounds.Y += this.m_row.Grid.Style.CellPadding.Top;
          bounds.Width = size.Width;
        }
      }
      else
      {
        bounds.Width -= this.Style.CellPadding.Right + this.Style.CellPadding.Left;
        bounds.Height -= this.Style.CellPadding.Bottom + this.Style.CellPadding.Top;
        if (this.StringFormat.Alignment == PdfTextAlignment.Center)
        {
          bounds.X += this.Style.CellPadding.Left + (float) (((double) bounds.Width - (double) size.Width) / 2.0);
          bounds.Y += this.Style.CellPadding.Top + (float) (((double) bounds.Height - (double) size.Height) / 2.0);
        }
        else if (this.StringFormat.Alignment == PdfTextAlignment.Left)
        {
          bounds.X += this.Style.CellPadding.Left;
          bounds.Y += this.Style.CellPadding.Top;
        }
        else if (this.StringFormat.Alignment == PdfTextAlignment.Right)
        {
          bounds.X += this.Style.CellPadding.Left + (bounds.Width - size.Width);
          bounds.Y += this.Style.CellPadding.Top;
          bounds.Width = size.Width;
        }
      }
    }
    else if (this.Style.CellPadding == null)
    {
      bounds.X += this.m_row.Grid.Style.CellPadding.Left;
      bounds.Y += this.m_row.Grid.Style.CellPadding.Top;
      bounds.Width -= this.m_row.Grid.Style.CellPadding.Right + this.m_row.Grid.Style.CellPadding.Left;
      bounds.Height -= this.m_row.Grid.Style.CellPadding.Bottom + this.m_row.Grid.Style.CellPadding.Top;
      if ((double) this.Row.RowBreakHeight > 0.0 && this.Value is PdfHTMLTextElement)
        this.Row.RowBreakHeight += this.m_row.Grid.Style.CellPadding.Bottom + this.m_row.Grid.Style.CellPadding.Top;
    }
    else
    {
      bounds.X += this.Style.CellPadding.Left;
      bounds.Y += this.Style.CellPadding.Top;
      bounds.Width -= this.Style.CellPadding.Right + this.Style.CellPadding.Left;
      bounds.Height -= this.Style.CellPadding.Bottom + this.Style.CellPadding.Top;
      if ((double) this.Row.RowBreakHeight > 0.0 && this.Value is PdfHTMLTextElement)
        this.Row.RowBreakHeight += this.Style.CellPadding.Bottom + this.Style.CellPadding.Top;
    }
    return bounds;
  }

  internal void DrawCellBorders(ref PdfGraphics graphics, RectangleF bounds)
  {
    bool flag1 = graphics.Tag != null;
    if (this.Row.RowSpanExists && this.Row.Grid.AllowRowBreakAcrossPages)
    {
      float num = 0.0f;
      bool flag2 = false;
      if (this.Row.Grid.m_listOfNavigatePages.Count > 0 && this.Row.Grid.LayoutFormat != null)
      {
        PdfLayoutFormat layoutFormat = this.Row.Grid.LayoutFormat;
        if (layoutFormat.UsePaginateBounds && layoutFormat.Break == PdfLayoutBreakType.FitPage && layoutFormat.Layout == PdfLayoutType.Paginate)
        {
          RectangleF paginateBounds = layoutFormat.PaginateBounds;
          if ((double) layoutFormat.PaginateBounds.Height > 0.0)
          {
            num = layoutFormat.PaginateBounds.Bottom;
            flag2 = true;
          }
        }
      }
      if (flag2 && (double) bounds.Bottom > (double) num && this.Row.RowIndex + 1 < this.Row.Grid.Rows.Count)
      {
        PdfGridRow row = this.Row.Grid.Rows[this.Row.RowIndex + 1];
        if ((double) num - (double) bounds.Y > (double) this.Row.Height)
        {
          row.m_borderReminingHeight = bounds.Height - this.Row.Height;
          bounds.Height = this.Row.Height;
        }
        else
        {
          row.m_borderReminingHeight = bounds.Height - (num - bounds.Y);
          bounds.Height = num - bounds.Y;
        }
        row.m_drawCellBroders = true;
      }
    }
    if (this.Row.Grid.Style.BorderOverlapStyle == PdfBorderOverlapStyle.Inside)
    {
      bounds.X += this.Style.Borders.Left.Width;
      bounds.Y += this.Style.Borders.Top.Width;
      bounds.Width -= this.Style.Borders.Right.Width;
      bounds.Height -= this.Style.Borders.Bottom.Width;
    }
    graphics.IsTemplateGraphics = true;
    if (this.Style.Borders.IsAll)
    {
      if ((double) bounds.X + (double) bounds.Width > (double) graphics.ClientSize.Width - (double) this.m_style.Borders.Left.Width / 2.0)
        bounds.Width -= this.m_style.Borders.Left.Width / 2f;
      if ((double) bounds.Y + (double) bounds.Height > (double) graphics.ClientSize.Width - (double) this.m_style.Borders.Bottom.Width / 2.0)
        bounds.Height -= this.m_style.Borders.Bottom.Width / 2f;
      this.SetTransparency(ref graphics, this.m_style.Borders.Left);
      if ((double) this.m_style.Borders.Left.Width != 0.0 && this.m_style.Borders.Left.Color.A != (byte) 0)
        graphics.DrawRectangle(this.m_style.Borders.Left, bounds);
      graphics.Restore();
    }
    else
    {
      PointF point1_1 = new PointF(bounds.X, bounds.Y + bounds.Height);
      PointF point2 = bounds.Location;
      PdfPen pen1 = this.m_style.Borders.Left;
      if (pen1.IsImmutable)
        pen1 = new PdfPen(this.m_style.Borders.Left.Color, this.m_style.Borders.Left.Width);
      if ((double) pen1.Width != 0.0 && pen1.Color.A != (byte) 0)
      {
        if (this.m_style.Borders.Left.DashStyle == PdfDashStyle.Solid)
          pen1.LineCap = PdfLineCap.Square;
        this.SetTransparency(ref graphics, pen1);
        if (graphics.Tag == null)
        {
          if (!flag1)
          {
            int num = graphics.IsTaggedPdf ? 1 : 0;
            if (!graphics.IsTaggedPdf)
              goto label_29;
          }
          graphics.Tag = (PdfTag) new PdfArtifact();
        }
label_29:
        graphics.DrawLine(pen1, point1_1, point2);
        graphics.Restore();
      }
      if (graphics.Tag == null)
      {
        if (!flag1)
        {
          int num = graphics.IsTaggedPdf ? 1 : 0;
          if (!graphics.IsTaggedPdf)
            goto label_34;
        }
        graphics.Tag = (PdfTag) new PdfArtifact();
      }
label_34:
      point1_1 = new PointF(bounds.X + bounds.Width, bounds.Y);
      point2 = new PointF(bounds.X + bounds.Width, bounds.Y + bounds.Height);
      PdfPen pen2 = this.m_style.Borders.Right;
      if ((double) bounds.X + (double) bounds.Width > (double) graphics.ClientSize.Width - (double) pen2.Width / 2.0)
      {
        point1_1 = new PointF(graphics.ClientSize.Width - pen2.Width / 2f, bounds.Y);
        point2 = new PointF(graphics.ClientSize.Width - pen2.Width / 2f, bounds.Y + bounds.Height);
      }
      if (pen2.IsImmutable)
        pen2 = new PdfPen(this.m_style.Borders.Right.Color, this.m_style.Borders.Right.Width);
      if ((double) pen2.Width != 0.0 && pen2.Color.A != (byte) 0)
      {
        if (this.m_style.Borders.Right.DashStyle == PdfDashStyle.Solid)
          pen2.LineCap = PdfLineCap.Square;
        this.SetTransparency(ref graphics, pen2);
        graphics.DrawLine(pen2, point1_1, point2);
        graphics.Restore();
      }
      if (graphics.Tag == null)
      {
        if (!flag1)
        {
          int num = graphics.IsTaggedPdf ? 1 : 0;
          if (!graphics.IsTaggedPdf)
            goto label_46;
        }
        graphics.Tag = (PdfTag) new PdfArtifact();
      }
label_46:
      PointF point1_2 = bounds.Location;
      point2 = new PointF(bounds.X + bounds.Width, bounds.Y);
      PdfPen pen3 = this.m_style.Borders.Top;
      if (pen3.IsImmutable)
        pen3 = new PdfPen(this.m_style.Borders.Top.Color, this.m_style.Borders.Top.Width);
      if ((double) pen3.Width != 0.0 && pen3.Color.A != (byte) 0)
      {
        if (this.m_style.Borders.Top.DashStyle == PdfDashStyle.Solid)
          pen3.LineCap = PdfLineCap.Square;
        this.SetTransparency(ref graphics, pen3);
        graphics.DrawLine(pen3, point1_2, point2);
        graphics.Restore();
      }
      if (graphics.Tag == null)
      {
        if (!flag1)
        {
          int num = graphics.IsTaggedPdf ? 1 : 0;
          if (!graphics.IsTaggedPdf)
            goto label_56;
        }
        graphics.Tag = (PdfTag) new PdfArtifact();
      }
label_56:
      point1_2 = new PointF(bounds.X + bounds.Width, bounds.Y + bounds.Height);
      point2 = new PointF(bounds.X, bounds.Y + bounds.Height);
      PdfPen pen4 = this.m_style.Borders.Bottom;
      if ((double) bounds.Y + (double) bounds.Height > (double) graphics.ClientSize.Height - (double) pen4.Width / 2.0)
      {
        point1_2 = new PointF(bounds.X + bounds.Width, graphics.ClientSize.Height - pen4.Width / 2f);
        point2 = new PointF(bounds.X, graphics.ClientSize.Height - pen4.Width / 2f);
      }
      if (pen4.IsImmutable)
        pen4 = new PdfPen(this.m_style.Borders.Bottom.Color, this.m_style.Borders.Bottom.Width);
      if ((double) pen4.Width != 0.0 && pen4.Color.A != (byte) 0)
      {
        if (this.m_style.Borders.Bottom.DashStyle == PdfDashStyle.Solid)
          pen4.LineCap = PdfLineCap.Square;
        this.SetTransparency(ref graphics, pen4);
        graphics.DrawLine(pen4, point1_2, point2);
        graphics.Restore();
      }
    }
    graphics.IsTemplateGraphics = false;
  }

  private void SetTransparency(ref PdfGraphics graphics, PdfPen pen)
  {
    float alpha = (float) pen.Color.A / (float) byte.MaxValue;
    graphics.Save();
    graphics.SetTransparency(alpha);
  }

  private PdfGridCell ObtainNextCell()
  {
    int num = this.m_row.Cells.IndexOf(this);
    return num + 1 <= this.m_row.Cells.Count ? this.m_row.Cells[num + 1] : (PdfGridCell) null;
  }

  internal void DrawCellBackground(ref PdfGraphics graphics, RectangleF bounds)
  {
    PdfBrush backgroundBrush = this.GetBackgroundBrush();
    graphics.IsTemplateGraphics = true;
    if (graphics.Tag == null)
    {
      int num = graphics.IsTaggedPdf ? 1 : 0;
      if (graphics.IsTaggedPdf)
        graphics.Tag = (PdfTag) new PdfArtifact();
    }
    if (backgroundBrush != null)
    {
      graphics.Save();
      graphics.DrawRectangle(backgroundBrush, bounds);
      graphics.Restore();
    }
    if (this.Style.BackgroundImage != null)
    {
      if (this.Style.CellPadding != null && this.Style.CellPadding != new PdfPaddings())
      {
        PdfPaddings cellPadding = this.Style.CellPadding;
        bounds = new RectangleF(bounds.X + cellPadding.Left, bounds.Y + cellPadding.Top, bounds.Width - (cellPadding.Left + cellPadding.Right), bounds.Height - (cellPadding.Top + cellPadding.Bottom));
      }
      else if (this.Row.Grid.Style.CellPadding != null && this.Row.Grid.Style.CellPadding != new PdfPaddings())
      {
        PdfPaddings cellPadding = this.Row.Grid.Style.CellPadding;
        bounds = new RectangleF(bounds.X + cellPadding.Left, bounds.Y + cellPadding.Top, bounds.Width - (cellPadding.Left + cellPadding.Right), bounds.Height - (cellPadding.Top + cellPadding.Bottom));
      }
      graphics.IsTemplateGraphics = false;
      PdfImage backgroundImage = this.Style.BackgroundImage;
      if (this.m_imagePosition == PdfGridImagePosition.Stretch)
        graphics.DrawImage(this.Style.BackgroundImage, bounds);
      else if (this.m_imagePosition == PdfGridImagePosition.Center)
      {
        float x = bounds.X + bounds.Width / 4f;
        float y = bounds.Y + bounds.Height / 4f;
        graphics.DrawImage(backgroundImage, x, y, bounds.Width / 2f, bounds.Height / 2f);
      }
      else if (this.m_imagePosition == PdfGridImagePosition.Fit)
      {
        float width = backgroundImage.PhysicalDimension.Width;
        if ((double) backgroundImage.PhysicalDimension.Height > (double) width)
        {
          float y = bounds.Y;
          float x = bounds.X + bounds.Width / 4f;
          graphics.DrawImage(backgroundImage, x, y, bounds.Width / 2f, bounds.Height);
        }
        else
        {
          float x = bounds.X;
          float y = bounds.Y + bounds.Height / 4f;
          graphics.DrawImage(backgroundImage, x, y, bounds.Width, bounds.Height / 2f);
        }
      }
      else if (this.m_imagePosition == PdfGridImagePosition.Tile)
      {
        float x1 = bounds.X;
        float y = bounds.Y;
        double width = (double) bounds.Width;
        PdfUnitConvertor pdfUnitConvertor = new PdfUnitConvertor();
        for (; (double) y < (double) bounds.Bottom; y += backgroundImage.PhysicalDimension.Height)
        {
          for (float x2 = x1; (double) x2 < (double) bounds.Right; x2 += backgroundImage.PhysicalDimension.Width)
          {
            if ((double) x2 + (double) backgroundImage.PhysicalDimension.Width > (double) bounds.Right && (double) y + (double) backgroundImage.PhysicalDimension.Height > (double) bounds.Bottom)
            {
              Rectangle rect = new Rectangle(0, 0, (int) pdfUnitConvertor.ConvertToPixels(bounds.Right - x2, PdfGraphicsUnit.Point), (int) pdfUnitConvertor.ConvertToPixels(bounds.Bottom - y, PdfGraphicsUnit.Point));
              Bitmap bitmap1 = new Bitmap(backgroundImage.InternalImage, new Size(backgroundImage.Width, backgroundImage.Height));
              Bitmap bitmap2 = bitmap1.Clone(rect, bitmap1.PixelFormat);
              MemoryStream memoryStream = new MemoryStream();
              if (Image.IsAlphaPixelFormat(backgroundImage.InternalImage.PixelFormat) || backgroundImage.InternalImage is Metafile)
                bitmap2.Save((Stream) memoryStream, ImageFormat.Png);
              else
                bitmap2.Save((Stream) memoryStream, ImageFormat.Jpeg);
              PdfBitmap image = new PdfBitmap((Stream) memoryStream);
              graphics.DrawImage((PdfImage) image, x2, y);
              memoryStream.Dispose();
              bitmap1.Dispose();
              bitmap2.Dispose();
              image.Dispose();
            }
            else if ((double) x2 + (double) backgroundImage.PhysicalDimension.Width > (double) bounds.Right)
            {
              Rectangle rect = new Rectangle(0, 0, (int) pdfUnitConvertor.ConvertToPixels(bounds.Right - x2, PdfGraphicsUnit.Point), backgroundImage.Height);
              Bitmap bitmap3 = new Bitmap(backgroundImage.InternalImage, new Size(backgroundImage.Width, backgroundImage.Height));
              Bitmap bitmap4 = bitmap3.Clone(rect, bitmap3.PixelFormat);
              MemoryStream memoryStream = new MemoryStream();
              if (Image.IsAlphaPixelFormat(backgroundImage.InternalImage.PixelFormat) || backgroundImage.InternalImage is Metafile)
                bitmap4.Save((Stream) memoryStream, ImageFormat.Png);
              else
                bitmap4.Save((Stream) memoryStream, ImageFormat.Jpeg);
              PdfBitmap image = new PdfBitmap((Stream) memoryStream);
              graphics.DrawImage((PdfImage) image, x2, y);
              memoryStream.Dispose();
              bitmap3.Dispose();
              bitmap4.Dispose();
              image.Dispose();
            }
            else if ((double) y + (double) backgroundImage.PhysicalDimension.Height > (double) bounds.Bottom)
            {
              float pixels = pdfUnitConvertor.ConvertToPixels(bounds.Bottom - y, PdfGraphicsUnit.Point);
              Rectangle rect = new Rectangle(0, 0, backgroundImage.Width, (int) pixels);
              Bitmap bitmap5 = new Bitmap(backgroundImage.InternalImage, new Size(backgroundImage.Width, backgroundImage.Height));
              Bitmap bitmap6 = bitmap5.Clone(rect, bitmap5.PixelFormat);
              MemoryStream memoryStream = new MemoryStream();
              if (Image.IsAlphaPixelFormat(backgroundImage.InternalImage.PixelFormat) || backgroundImage.InternalImage is Metafile)
                bitmap6.Save((Stream) memoryStream, ImageFormat.Png);
              else
                bitmap6.Save((Stream) memoryStream, ImageFormat.Jpeg);
              PdfBitmap image = new PdfBitmap((Stream) memoryStream);
              graphics.DrawImage((PdfImage) image, x2, y);
              memoryStream.Dispose();
              bitmap5.Dispose();
              bitmap6.Dispose();
              image.Dispose();
            }
            else
              graphics.DrawImage(backgroundImage, new PointF(x2, y));
          }
        }
      }
    }
    graphics.IsTemplateGraphics = false;
  }

  private PdfStringFormat ObtainStringFormat() => this.Style.StringFormat ?? this.StringFormat;

  private PdfFont GetTextFont()
  {
    return this.Style.Font ?? this.Row.Style.Font ?? this.Row.Grid.Style.Font ?? PdfDocument.DefaultFont;
  }

  private PdfBrush GetTextBrush()
  {
    return this.Style.TextBrush ?? this.Row.Style.TextBrush ?? this.Row.Grid.Style.TextBrush ?? PdfBrushes.Black;
  }

  private PdfPen GetTextPen()
  {
    return this.Style.TextPen ?? this.Row.Style.TextPen ?? this.Row.Grid.Style.TextPen;
  }

  private PdfBrush GetBackgroundBrush()
  {
    return this.Style.BackgroundBrush ?? this.Row.Style.BackgroundBrush ?? this.Row.Grid.Style.BackgroundBrush;
  }

  private float CalculateWidth()
  {
    int num1 = this.Row.Cells.IndexOf(this);
    int columnSpan = this.ColumnSpan;
    float width = 0.0f;
    for (int index = 0; index < columnSpan; ++index)
      width += this.Row.Grid.Columns[num1 + index].Width;
    if (this.m_parent != null && (double) this.m_parent.Row.Width > 0.0 && this.Row.Grid.IsChildGrid && (double) this.Row.Width > (double) this.m_parent.Row.Width)
    {
      float num2 = 0.0f;
      for (int index = 0; index < this.m_parent.ColumnSpan; ++index)
        num2 += this.m_parent.Row.Grid.Columns[index].Width;
      width = num2 / (float) this.Row.Cells.Count;
    }
    else if (this.m_parent != null && this.Row.Grid.IsChildGrid && (double) width == -3.4028234663852886E+38)
    {
      width = this.FindGridColumnWidth(this.m_parent);
      if (columnSpan <= 1)
        width /= (float) this.Row.Cells.Count;
    }
    return width;
  }

  private float GetColumnWidth()
  {
    float width = this.m_parent.CalculateWidth();
    if (this.m_parent != null && this.Row.Grid != null && this.Row.Grid.IsChildGrid && this.m_parent.Row != null && this.m_parent.Row.Grid != null && this.m_parent.Row.Grid.LayoutFormat != null && this.m_parent.Row.Grid.LayoutFormat.Break == PdfLayoutBreakType.FitElement && this.m_parent.Row.Grid.Style.CellPadding != null)
    {
      width -= this.m_parent.Row.Grid.Style.CellPadding.Left + this.m_parent.Row.Grid.Style.CellPadding.Right;
      if (this.Row.Grid.Style.CellPadding != null)
        width -= this.Row.Grid.Style.CellPadding.Left + this.Row.Grid.Style.CellPadding.Right;
      if (this.Style.Borders != null && this.Style.Borders.Left != null && this.Style.Borders.Right != null)
        width -= (float) (((double) this.Style.Borders.Left.Width + (double) this.Style.Borders.Right.Width) * 2.0);
    }
    float columnWidth = width / (float) this.Row.Grid.Columns.Count;
    if ((double) columnWidth <= 0.0)
      columnWidth = float.MaxValue;
    return columnWidth;
  }

  private float FindGridColumnWidth(PdfGridCell pdfGridCell)
  {
    float gridColumnWidth = float.MinValue;
    if (pdfGridCell.m_parent != null && (double) pdfGridCell.m_outerCellWidth == -3.4028234663852886E+38)
      gridColumnWidth = this.FindGridColumnWidth(pdfGridCell.m_parent) / (float) pdfGridCell.Row.Cells.Count;
    else if (pdfGridCell.m_parent == null && (double) pdfGridCell.m_outerCellWidth > 0.0)
      gridColumnWidth = pdfGridCell.m_outerCellWidth;
    return gridColumnWidth;
  }
}
