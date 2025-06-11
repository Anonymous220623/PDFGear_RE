// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.ShapeLayouter
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics.Images.Metafiles;
using Syncfusion.Pdf.HtmlToPdf;
using Syncfusion.Pdf.Interactive;
using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

internal class ShapeLayouter(PdfShapeElement element) : ElementLayouter((PdfLayoutElement) element)
{
  private const int borderWidth = 0;
  internal int olderPdfForm;
  [ThreadStatic]
  private static int index;
  [ThreadStatic]
  private static float splitDiff;
  [ThreadStatic]
  private static bool last;
  internal bool m_isPdfGrid;
  internal RectangleF shapeBounds = RectangleF.Empty;
  internal float m_bottomCellPadding;
  private double TotalPageSize;
  private TextRegionManager m_textRegions = new TextRegionManager();
  private ImageRegionManager m_imageRegions = new ImageRegionManager();
  private ImageRegionManager m_formRegions = new ImageRegionManager();

  public PdfShapeElement Element => base.Element as PdfShapeElement;

  private TextRegionManager TextRegions
  {
    get => this.m_textRegions;
    set => this.m_textRegions = value;
  }

  private ImageRegionManager ImageRegions
  {
    get => this.m_imageRegions;
    set => this.m_imageRegions = value;
  }

  private ImageRegionManager FormRegions
  {
    get => this.m_formRegions;
    set => this.m_formRegions = value;
  }

  protected override PdfLayoutResult LayoutInternal(PdfLayoutParams param)
  {
    PdfPage currentPage = param != null ? param.Page : throw new ArgumentNullException(nameof (param));
    RectangleF currentBounds = param.Bounds;
    RectangleF rectangleF = new RectangleF();
    RectangleF shapeLayoutBounds = (!(this.Element is PdfBezierCurve) ? this.Element.GetBounds() : currentBounds) with
    {
      Location = PointF.Empty
    };
    if (this.m_isPdfGrid && this.shapeBounds != RectangleF.Empty)
      shapeLayoutBounds = this.shapeBounds;
    ShapeLayouter.ShapeLayoutResult pageResult = new ShapeLayouter.ShapeLayoutResult();
    pageResult.Page = currentPage;
    do
    {
      bool flag1 = this.RaiseBeforePageLayout(currentPage, ref currentBounds);
      EndPageLayoutEventArgs pageLayoutEventArgs = (EndPageLayoutEventArgs) null;
      if (!flag1)
      {
        pageResult = this.LayoutOnPage(currentPage, currentBounds, shapeLayoutBounds, param);
        pageLayoutEventArgs = this.RaiseEndPageLayout(pageResult);
        flag1 = pageLayoutEventArgs != null && pageLayoutEventArgs.Cancel;
      }
      PdfFormFieldCollection fields = pageResult.Page.Document.Form.Fields;
      for (int index1 = fields.Count - 1; index1 >= this.olderPdfForm; --index1)
      {
        if (fields[index1] is PdfRadioButtonListField)
        {
          PdfRadioButtonListField radioButtonListField1 = fields[index1] as PdfRadioButtonListField;
          int num = 0;
          for (int index2 = this.olderPdfForm - 1; index2 >= 0; --index2)
          {
            if (fields[index2] is PdfRadioButtonListField)
            {
              PdfRadioButtonListField radioButtonListField2 = fields[index2] as PdfRadioButtonListField;
              if (radioButtonListField1.Name.Contains(radioButtonListField2.Name))
                ++num;
            }
          }
          for (int index3 = radioButtonListField1.Items.Count - 1; index3 >= num && (double) currentPage.GetClientSize().Height >= (double) radioButtonListField1.Items[index3].Bounds.Height + (double) radioButtonListField1.Items[index3].Bounds.Y; --index3)
          {
            PdfRadioButtonListItem radioButtonListItem = radioButtonListField1.Items[index3];
            PdfSolidBrush brush = new PdfSolidBrush(radioButtonListItem.BackRectColor);
            currentPage.Graphics.DrawRectangle((PdfBrush) brush, radioButtonListItem.Bounds);
          }
        }
      }
      for (int index = fields.Count - 1; index >= this.olderPdfForm; --index)
      {
        if (fields[index] is PdfTextBoxField)
        {
          PdfTextBoxField pdfTextBoxField = fields[index] as PdfTextBoxField;
          PdfSolidBrush brush = new PdfSolidBrush(pdfTextBoxField.BackRectColor);
          currentPage.Graphics.DrawRectangle((PdfBrush) brush, pdfTextBoxField.Bounds);
        }
        else if (fields[index] is PdfCheckBoxField)
        {
          PdfCheckBoxField pdfCheckBoxField = fields[index] as PdfCheckBoxField;
          PdfSolidBrush brush = new PdfSolidBrush(pdfCheckBoxField.BackRectColor);
          currentPage.Graphics.DrawRectangle((PdfBrush) brush, pdfCheckBoxField.Bounds);
        }
        else if (fields[index] is PdfButtonField)
        {
          PdfButtonField pdfButtonField = fields[index] as PdfButtonField;
          PdfSolidBrush brush = new PdfSolidBrush(pdfButtonField.BackRectColor);
          currentPage.Graphics.DrawRectangle((PdfBrush) brush, pdfButtonField.Bounds);
        }
        else if (fields[index] is PdfListBoxField)
        {
          PdfListBoxField pdfListBoxField = fields[index] as PdfListBoxField;
          PdfSolidBrush brush = new PdfSolidBrush(pdfListBoxField.BackRectColor);
          currentPage.Graphics.DrawRectangle((PdfBrush) brush, pdfListBoxField.Bounds);
        }
        else if (fields[index] is PdfComboBoxField)
        {
          PdfComboBoxField pdfComboBoxField = fields[index] as PdfComboBoxField;
          PdfSolidBrush brush = new PdfSolidBrush(pdfComboBoxField.BackRectColor);
          currentPage.Graphics.DrawRectangle((PdfBrush) brush, pdfComboBoxField.Bounds);
        }
      }
      this.olderPdfForm = fields.Items.Count;
      bool flag2 = this.Element is PdfMetafile && (this.Element as PdfMetafile).m_isHtmlToTaggedPdf;
      if (pageResult.Page.Document.FileStructure.TaggedPdf && !pageResult.End && !flag1 && flag2)
        return new PdfLayoutResult(pageResult.Page, pageResult.Bounds);
      if (!pageResult.End && !flag1)
      {
        currentBounds = this.GetPaginateBounds(param);
        shapeLayoutBounds = this.GetNextShapeBounds(shapeLayoutBounds, pageResult);
        currentPage = pageLayoutEventArgs == null || pageLayoutEventArgs.NextPage == null ? this.GetNextPage(currentPage) : pageLayoutEventArgs.NextPage;
        currentPage.Graphics.Tag = pageResult.Page.Graphics.Tag;
      }
      else
        goto label_38;
    }
    while (!this.m_isPdfGrid);
    PdfLayoutResult layoutResult = this.GetLayoutResult(pageResult);
    goto label_39;
label_38:
    layoutResult = this.GetLayoutResult(pageResult);
label_39:
    return layoutResult;
  }

  protected RectangleF GetPaginateBounds(HtmlToPdfParams param)
  {
    if (param == null)
      throw new ArgumentNullException(nameof (param));
    return param.Format.UsePaginateBounds ? param.Format.PaginateBounds : new RectangleF(param.Bounds.X, 0.0f, param.Bounds.Width, param.Bounds.Height);
  }

  protected override PdfLayoutResult LayoutInternal(HtmlToPdfParams param)
  {
    PdfPage currentPage = param != null ? param.Page : throw new ArgumentNullException(nameof (param));
    RectangleF currentBounds = param.Bounds;
    RectangleF shapeLayoutBounds = this.Element.GetBounds() with
    {
      Location = PointF.Empty
    };
    this.TotalPageSize = param.Format.TotalPageSize;
    ShapeLayouter.ShapeLayoutResult pageResult = new ShapeLayouter.ShapeLayoutResult();
    pageResult.Page = currentPage;
    while (true)
    {
      bool flag = this.RaiseBeforePageLayout(currentPage, ref currentBounds);
      EndPageLayoutEventArgs pageLayoutEventArgs = (EndPageLayoutEventArgs) null;
      if (!flag)
      {
        if ((double) currentBounds.Y != 0.0 && (double) currentBounds.Height > (double) currentBounds.Y && (double) currentBounds.Height > (double) shapeLayoutBounds.Height)
          currentBounds.Height = shapeLayoutBounds.Height;
        if ((double) currentBounds.Y == (double) currentBounds.Height)
        {
          currentPage = this.GetNextPage(currentPage);
          currentBounds.Y = 0.0f;
        }
        pageResult = this.LayoutOnPage(currentPage, currentBounds, shapeLayoutBounds, param);
        this.TotalPageSize += (double) pageResult.Bounds.Height;
        pageLayoutEventArgs = this.RaiseEndPageLayout(pageResult);
        flag = pageLayoutEventArgs != null && pageLayoutEventArgs.Cancel;
        if (this.TotalPageSize > (double) param.Format.TotalPageLayoutSize)
          flag = true;
      }
      if (!pageResult.End && !flag)
      {
        currentBounds = this.GetPaginateBounds(param);
        shapeLayoutBounds = this.GetNextShapeBounds(shapeLayoutBounds, pageResult);
        currentPage = pageLayoutEventArgs == null || pageLayoutEventArgs.NextPage == null ? this.GetNextPage(currentPage) : pageLayoutEventArgs.NextPage;
      }
      else
        break;
    }
    PdfLayoutResult layoutResult = this.GetLayoutResult(pageResult);
    layoutResult.TotalPageSize = this.TotalPageSize;
    return layoutResult;
  }

  protected override PdfLayoutResult LayoutInternal(HtmlToPdfLayoutParams param)
  {
    if (param == null)
      throw new ArgumentNullException(nameof (param));
    PdfLayoutParams pdfLayoutParams = new PdfLayoutParams();
    pdfLayoutParams.Bounds = param.Bounds;
    pdfLayoutParams.Format = param.Format;
    pdfLayoutParams.Page = param.Page;
    if (param.VerticalOffsets.Length == 1 || (param.Format as PdfMetafileLayoutFormat).m_enableDirectLayout)
      return this.LayoutInternal(pdfLayoutParams);
    PdfPage currentPage = param.Page;
    RectangleF bounds = param.Bounds;
    RectangleF shapeLayoutBounds = this.Element.GetBounds() with
    {
      Location = PointF.Empty
    };
    PdfLayoutResult pdfLayoutResult = (PdfLayoutResult) null;
    ShapeLayouter.ShapeLayoutResult pageResult = new ShapeLayouter.ShapeLayoutResult();
    pageResult.Page = currentPage;
    if (param.Page.Section.Count == 1)
    {
      ShapeLayouter.last = false;
      ShapeLayouter.index = 0;
      ShapeLayouter.splitDiff = 0.0f;
    }
    float num1 = 0.0f;
    bool flag1 = false;
    int length = param.VerticalOffsets.Length;
    foreach (float verticalOffset in param.VerticalOffsets)
    {
      if (ShapeLayouter.index > length - 1 || (double) param.VerticalOffsets[ShapeLayouter.index] == (double) verticalOffset)
      {
        bool flag2 = false;
        while (!flag2)
        {
          float num2;
          if ((double) verticalOffset != 0.0)
          {
            num2 = Math.Min(currentPage.Graphics.ClientSize.Height, verticalOffset);
            if ((double) num1 + (double) num2 > (double) verticalOffset)
            {
              num2 = verticalOffset - num1;
              (pdfLayoutParams.Format as PdfMetafileLayoutFormat).IsHTMLPageBreak = true;
            }
          }
          else
            num2 = Math.Min(currentPage.Graphics.ClientSize.Height, shapeLayoutBounds.Height);
          RectangleF currentBounds = new RectangleF(0.0f, param.Bounds.Y, 0.0f, num2 - param.Bounds.Y);
          bool flag3 = this.RaiseBeforePageLayout(currentPage, ref currentBounds);
          EndPageLayoutEventArgs pageLayoutEventArgs = (EndPageLayoutEventArgs) null;
          bool flag4 = false;
          if (ShapeLayouter.index == length - 1)
            ShapeLayouter.last = true;
          if (!flag3)
          {
            pageResult = this.LayoutOnPage(currentPage, currentBounds, shapeLayoutBounds, pdfLayoutParams);
            pageLayoutEventArgs = this.RaiseEndPageLayout(pageResult);
            flag3 = pageLayoutEventArgs != null && pageLayoutEventArgs.Cancel;
            (pdfLayoutParams.Format as PdfMetafileLayoutFormat).IsHTMLPageBreak = false;
          }
          num1 += (double) pageResult.Bounds.Height > 0.0 ? pageResult.Bounds.Height : num2;
          if ((int) num1 == (int) verticalOffset)
          {
            flag4 = true;
            ++ShapeLayouter.index;
          }
          float num3;
          if (!pageResult.End && !flag3 && !flag4)
          {
            currentBounds = this.GetPaginateBounds(pdfLayoutParams);
            shapeLayoutBounds = this.GetNextShapeBounds(shapeLayoutBounds, pageResult);
            currentPage = pageLayoutEventArgs == null || pageLayoutEventArgs.NextPage == null ? this.GetNextPage(currentPage) : pageLayoutEventArgs.NextPage;
            if ((int) num1 == (int) verticalOffset)
            {
              num3 = 0.0f;
              break;
            }
          }
          else
          {
            if (!ShapeLayouter.last)
            {
              currentBounds = this.GetPaginateBounds(pdfLayoutParams);
              pageResult.Page = pageLayoutEventArgs == null || pageLayoutEventArgs.NextPage == null ? this.GetNextPage(currentPage) : pageLayoutEventArgs.NextPage;
              num3 = 0.0f;
              pageResult.Bounds = RectangleF.Empty;
            }
            pdfLayoutResult = this.GetLayoutResult(pageResult);
            pageResult.Bounds = RectangleF.Empty;
            flag1 = true;
            break;
          }
        }
        break;
      }
    }
    return pdfLayoutResult;
  }

  protected virtual RectangleF CheckCorrectCurrentBounds(
    PdfPage currentPage,
    RectangleF currentBounds,
    RectangleF shapeLayoutBounds,
    PdfLayoutParams param)
  {
    if (currentPage == null)
      throw new ArgumentNullException(nameof (currentPage));
    SizeF clientSize = currentPage.Graphics.ClientSize;
    currentBounds.Width = !(this.Element is PdfMetafile) ? ((double) currentBounds.Width > 0.0 ? currentBounds.Width : clientSize.Width - currentBounds.X) : ((double) currentBounds.Width > 0.0 ? currentBounds.Width : ((double) shapeLayoutBounds.Width > 0.0 ? shapeLayoutBounds.Width : clientSize.Width - currentBounds.X));
    currentBounds.Height = (double) currentBounds.Height > 0.0 ? currentBounds.Height : clientSize.Height - currentBounds.Y;
    if (this.m_isPdfGrid)
      currentBounds.Height -= this.m_bottomCellPadding;
    return currentBounds;
  }

  private PdfLayoutResult GetLayoutResult(ShapeLayouter.ShapeLayoutResult pageResult)
  {
    return new PdfLayoutResult(pageResult.Page, pageResult.Bounds);
  }

  protected virtual RectangleF CheckCorrectCurrentBounds(
    PdfPage currentPage,
    RectangleF currentBounds,
    RectangleF shapeLayoutBounds,
    HtmlToPdfParams param)
  {
    if (currentPage == null)
      throw new ArgumentNullException(nameof (currentPage));
    SizeF clientSize = currentPage.Graphics.ClientSize;
    currentBounds.Width = (double) currentBounds.Width > 0.0 ? currentBounds.Width : clientSize.Width - currentBounds.X;
    currentBounds.Height = (double) currentBounds.Height > 0.0 ? currentBounds.Height : clientSize.Height - currentBounds.Y;
    return currentBounds;
  }

  internal RectangleF GetPdfLayoutBounds(
    PdfPage currentPage,
    RectangleF currentBounds,
    RectangleF shapeLayoutBounds,
    HtmlToPdfParams param)
  {
    if (param == null)
      throw new ArgumentNullException(nameof (param));
    this.TextRegions = param.Format.TextRegionManager;
    this.ImageRegions = param.Format.ImageRegionManager;
    this.FormRegions = param.Format.FormRegionManager;
    RectangleF pdfLayoutBounds = this.CheckCorrectCurrentBounds(currentPage, currentBounds, shapeLayoutBounds, param);
    HtmlToPdfFormat format = param.Format;
    bool flag1 = format != null && format.SplitTextLines;
    bool flag2 = format != null && format.SplitImages;
    bool flag3 = false;
    if (!this.IsImagePath)
    {
      if (this.TextRegions != null && !flag1 && !flag3)
      {
        float num1 = shapeLayoutBounds.Y + pdfLayoutBounds.Height;
        PdfUnitConvertor pdfUnitConvertor = new PdfUnitConvertor(96f);
        float topCoordinate1 = this.TextRegions.GetTopCoordinate(pdfUnitConvertor.ConvertToPixels(num1, PdfGraphicsUnit.Point));
        if ((double) pdfLayoutBounds.Height > (double) currentPage.GetClientSize().Height)
          topCoordinate1 = this.TextRegions.GetTopCoordinate(topCoordinate1 - 2f);
        float num2 = pdfUnitConvertor.ConvertFromPixels(topCoordinate1, PdfGraphicsUnit.Point);
        float num3 = 0.0f;
        if ((double) num2 > (double) shapeLayoutBounds.Y)
          num3 = num2 - shapeLayoutBounds.Y;
        pdfLayoutBounds.Height = currentPage == null || (double) currentPage.GetClientSize().Height >= (double) num3 ? num3 : currentPage.GetClientSize().Height;
        if ((double) pdfLayoutBounds.Y != 0.0)
        {
          float num4 = pdfLayoutBounds.Y + num3;
          if ((double) num4 > (double) currentPage.GetClientSize().Height)
          {
            float height = currentPage.GetClientSize().Height;
            float num5 = num4 - height;
            pdfLayoutBounds.Height = num3 - num5;
            float num6 = shapeLayoutBounds.Y + pdfLayoutBounds.Height;
            float topCoordinate2 = this.TextRegions.GetTopCoordinate(pdfUnitConvertor.ConvertToPixels(num6, PdfGraphicsUnit.Point));
            float num7 = pdfUnitConvertor.ConvertFromPixels(topCoordinate2, PdfGraphicsUnit.Point);
            if ((double) num7 > (double) shapeLayoutBounds.Y)
              num3 = num7 - shapeLayoutBounds.Y;
            pdfLayoutBounds.Height = currentPage == null || (double) currentPage.GetClientSize().Height >= (double) num3 ? num3 : currentPage.GetClientSize().Height;
          }
        }
      }
      if (this.ImageRegions != null && !flag2 && !flag3)
      {
        float height = pdfLayoutBounds.Height;
        float num8 = shapeLayoutBounds.Y + pdfLayoutBounds.Height;
        PdfUnitConvertor pdfUnitConvertor = new PdfUnitConvertor(96f);
        float topCoordinate3 = this.ImageRegions.GetTopCoordinate(pdfUnitConvertor.ConvertToPixels(num8, PdfGraphicsUnit.Point));
        float num9 = pdfUnitConvertor.ConvertFromPixels(topCoordinate3, PdfGraphicsUnit.Point);
        if (Math.Round((double) num9) != Math.Round((double) shapeLayoutBounds.Y + (double) pdfLayoutBounds.Height))
          num9 = (float) Math.Floor((double) num9);
        float num10 = 0.0f;
        if ((double) num9 > (double) shapeLayoutBounds.Y)
        {
          num10 = num9 - shapeLayoutBounds.Y;
          pdfLayoutBounds.Height = num10;
        }
        if ((double) num10 == 0.0 || this.TextRegions.Count == 0)
        {
          pdfLayoutBounds.Height = height;
        }
        else
        {
          PdfPage page = param.Page;
          if ((double) shapeLayoutBounds.Height > (double) page.Size.Height)
            pdfLayoutBounds.Height = num10;
          if (this.TextRegions != null && !flag1)
          {
            float num11 = shapeLayoutBounds.Y + pdfLayoutBounds.Height;
            float topCoordinate4 = this.TextRegions.GetTopCoordinate(pdfUnitConvertor.ConvertToPixels(num11, PdfGraphicsUnit.Point));
            float num12 = pdfUnitConvertor.ConvertFromPixels(topCoordinate4, PdfGraphicsUnit.Point);
            if ((double) num12 > (double) shapeLayoutBounds.Y && (double) num12 < (double) pdfLayoutBounds.Height)
              num10 = num12 - shapeLayoutBounds.Y;
            pdfLayoutBounds.Height = currentPage == null || (double) currentPage.GetClientSize().Height >= (double) num10 ? num10 : currentPage.GetClientSize().Height;
            if ((double) pdfLayoutBounds.Height == 0.0 && (double) currentPage.GetClientSize().Height > (double) num10)
              pdfLayoutBounds.Height = height;
          }
          else
            pdfLayoutBounds.Height = num10;
        }
      }
      if (this.FormRegions != null)
      {
        float num13 = shapeLayoutBounds.Y + pdfLayoutBounds.Height;
        PdfUnitConvertor pdfUnitConvertor = new PdfUnitConvertor(96f);
        float topCoordinate5 = this.FormRegions.GetTopCoordinate(pdfUnitConvertor.ConvertToPixels(num13, PdfGraphicsUnit.Point));
        float num14 = pdfUnitConvertor.ConvertFromPixels(topCoordinate5, PdfGraphicsUnit.Point);
        float num15 = 0.0f;
        if ((double) num14 > (double) shapeLayoutBounds.Y)
          num15 = num14 - shapeLayoutBounds.Y;
        pdfLayoutBounds.Height = currentPage == null || (double) currentPage.GetClientSize().Height >= (double) num15 ? num15 : currentPage.GetClientSize().Height;
        if ((double) pdfLayoutBounds.Y != 0.0)
        {
          float num16 = pdfLayoutBounds.Y + num15;
          if ((double) num16 > (double) currentPage.GetClientSize().Height)
          {
            float height = currentPage.GetClientSize().Height;
            float num17 = num16 - height;
            pdfLayoutBounds.Height = num15 - num17;
            float num18 = shapeLayoutBounds.Y + pdfLayoutBounds.Height;
            float topCoordinate6 = this.FormRegions.GetTopCoordinate(pdfUnitConvertor.ConvertToPixels(num18, PdfGraphicsUnit.Point));
            float num19 = pdfUnitConvertor.ConvertFromPixels(topCoordinate6, PdfGraphicsUnit.Point);
            if ((double) num19 > (double) shapeLayoutBounds.Y)
              num15 = num19 - shapeLayoutBounds.Y;
            pdfLayoutBounds.Height = currentPage == null || (double) currentPage.GetClientSize().Height >= (double) num15 ? num15 : currentPage.GetClientSize().Height;
          }
        }
      }
    }
    ArrayList list = new ArrayList();
    float height1 = 0.0f;
    foreach (HtmlHyperLink htmlHyperlinks in format.HtmlHyperlinksCollection)
    {
      height1 = pdfLayoutBounds.Height + pdfLayoutBounds.Y;
      if ((double) height1 > (double) htmlHyperlinks.Bounds.Y)
      {
        if (string.IsNullOrEmpty(htmlHyperlinks.Hash))
        {
          PdfUriAnnotation annotation = new PdfUriAnnotation(htmlHyperlinks.Bounds, htmlHyperlinks.Href);
          annotation.Border.Width = 0.0f;
          currentPage.Annotations.Add((PdfAnnotation) annotation);
        }
        else
        {
          PdfDocumentLinkAnnotation annotation = new PdfDocumentLinkAnnotation(htmlHyperlinks.Bounds);
          annotation.Border.Width = 0.0f;
          annotation.ApplyText(htmlHyperlinks.Hash);
          currentPage.Annotations.Add((PdfAnnotation) annotation);
        }
        list.Add((object) htmlHyperlinks);
      }
    }
    foreach (HtmlInternalLink htmlInternalLinks in format.HtmlInternalLinksCollection)
    {
      height1 = pdfLayoutBounds.Height + pdfLayoutBounds.Y;
      float y = (double) height1 <= (double) htmlInternalLinks.Bounds.Y ? htmlInternalLinks.Bounds.Y - pdfLayoutBounds.Height : htmlInternalLinks.Bounds.Y;
      if (htmlInternalLinks.DestinationPage is PdfLoadedPage)
      {
        PdfDocumentLinkAnnotation annotation = new PdfDocumentLinkAnnotation(new RectangleF(htmlInternalLinks.Bounds.X, y, htmlInternalLinks.Bounds.Width, htmlInternalLinks.Bounds.Height));
        annotation.Border.Width = 0.0f;
        annotation.ApplyText(htmlInternalLinks.Href);
        currentPage.Annotations.Add((PdfAnnotation) annotation);
      }
      else
      {
        PdfDocumentLinkAnnotation annotation = new PdfDocumentLinkAnnotation(new RectangleF(htmlInternalLinks.Bounds.X, y, htmlInternalLinks.Bounds.Width, htmlInternalLinks.Bounds.Height), new PdfDestination(htmlInternalLinks.DestinationPage)
        {
          Location = htmlInternalLinks.Destination
        });
        annotation.Border.Width = 0.0f;
        currentPage.Annotations.Add((PdfAnnotation) annotation);
      }
    }
    this.RepositionLinks(list, height1, format);
    return pdfLayoutBounds;
  }

  internal void RepositionLinks(ArrayList list, float height, HtmlToPdfFormat format)
  {
    foreach (HtmlHyperLink htmlHyperLink in list)
      format.HtmlHyperlinksCollection.Remove((object) htmlHyperLink);
    list.Clear();
    list = format.HtmlHyperlinksCollection.Clone() as ArrayList;
    format.HtmlHyperlinksCollection.Clear();
    foreach (HtmlHyperLink htmlHyperLink in list)
    {
      float y = htmlHyperLink.Bounds.Y - height;
      htmlHyperLink.Bounds = new RectangleF(htmlHyperLink.Bounds.X, y, htmlHyperLink.Bounds.Width, htmlHyperLink.Bounds.Height);
      format.HtmlHyperlinksCollection.Add((object) htmlHyperLink);
    }
  }

  private ShapeLayouter.ShapeLayoutResult LayoutOnPage(
    PdfPage currentPage,
    RectangleF currentBounds,
    RectangleF shapeLayoutBounds,
    HtmlToPdfParams param)
  {
    if (currentPage == null)
      throw new ArgumentNullException(nameof (currentPage));
    if (param == null)
      throw new ArgumentNullException(nameof (param));
    ShapeLayouter.ShapeLayoutResult shapeLayoutResult = new ShapeLayouter.ShapeLayoutResult();
    currentBounds = this.GetPdfLayoutBounds(currentPage, currentBounds, shapeLayoutBounds, param);
    bool bounds = this.FitsToBounds(currentBounds, shapeLayoutBounds);
    bool flag1 = param.Format.Break != PdfLayoutBreakType.FitElement || bounds || currentPage != param.Page;
    bool flag2 = false;
    if (flag1)
    {
      RectangleF drawBounds = this.GetDrawBounds(currentBounds, shapeLayoutBounds);
      if (Math.Round((double) shapeLayoutBounds.Height) + 2.0 == Math.Round((double) currentPage.GetClientSize().Height))
        shapeLayoutBounds.Height = currentPage.GetClientSize().Height;
      this.DrawShape(currentPage.Graphics, currentBounds, drawBounds);
      shapeLayoutResult.Bounds = this.GetPageResultBounds(currentBounds, shapeLayoutBounds);
      flag2 = (int) currentBounds.Height >= (int) shapeLayoutBounds.Height;
    }
    shapeLayoutResult.End = flag2 || param.Format.Layout == PdfLayoutType.OnePage;
    shapeLayoutResult.Page = currentPage;
    return shapeLayoutResult;
  }

  private ShapeLayouter.ShapeLayoutResult LayoutOnPage(
    PdfPage currentPage,
    RectangleF currentBounds,
    RectangleF shapeLayoutBounds,
    PdfLayoutParams param)
  {
    if (currentPage == null)
      throw new ArgumentNullException(nameof (currentPage));
    if (param == null)
      throw new ArgumentNullException(nameof (param));
    ShapeLayouter.ShapeLayoutResult shapeLayoutResult = new ShapeLayouter.ShapeLayoutResult();
    currentBounds = this.CheckCorrectCurrentBounds(currentPage, currentBounds, shapeLayoutBounds, param);
    bool bounds = this.FitsToBounds(currentBounds, shapeLayoutBounds);
    bool flag1 = param.Format.Break != PdfLayoutBreakType.FitElement || bounds || currentPage != param.Page;
    bool flag2 = false;
    if (flag1)
    {
      RectangleF drawBounds = this.GetDrawBounds(currentBounds, shapeLayoutBounds);
      if (this.Element is PdfMetafile && currentPage != null && currentPage.Section.ParentDocument is PdfDocument && currentPage.Section.ParentDocument.FileStructure.TaggedPdf)
        this.DrawShape(ref currentPage, currentBounds, drawBounds, shapeLayoutBounds, true);
      else
        this.DrawShape(currentPage.Graphics, currentBounds, drawBounds);
      shapeLayoutResult.Bounds = this.GetPageResultBounds(currentBounds, shapeLayoutBounds);
      flag2 = (int) currentBounds.Height >= (int) shapeLayoutBounds.Height;
      if (this.Element is PdfMetafile && currentPage != null && currentPage.Section.ParentDocument is PdfDocument && currentPage.Section.ParentDocument.FileStructure.TaggedPdf)
      {
        bool flag3;
        if ((double) currentPage.Graphics.Split > 0.0)
        {
          shapeLayoutResult.End = false;
          flag3 = true;
          shapeLayoutResult.Page = currentPage;
          shapeLayoutResult.Bounds = currentBounds;
          shapeLayoutResult.Bounds.Height = Math.Min(currentBounds.Height, currentPage.Graphics.Split);
          currentPage.Graphics.Split = 0.0f;
          return shapeLayoutResult;
        }
        shapeLayoutResult.End = true;
        flag3 = true;
        shapeLayoutResult.Page = currentPage;
        return shapeLayoutResult;
      }
    }
    shapeLayoutResult.End = flag2 || param.Format.Layout == PdfLayoutType.OnePage;
    shapeLayoutResult.Page = currentPage;
    return shapeLayoutResult;
  }

  private void DrawShape(
    ref PdfPage pdfPage,
    RectangleF currentBounds,
    RectangleF drawRectangle,
    RectangleF shapeLayoutBounds,
    bool tagged)
  {
    PdfMetafile element = this.Element as PdfMetafile;
    using (Metafile metaFile = element.InternalImage.Clone() as Metafile)
    {
      using (PdfEmfRenderer renderer = new PdfEmfRenderer(pdfPage.Graphics, currentBounds.Location, true))
      {
        renderer.EmfWidth = element.Width;
        using (MetaRecordParser metaRecordParser = new MetaRecordParser(renderer, metaFile))
        {
          metaRecordParser.Parser.PageScale = element.PageScale;
          metaRecordParser.Parser.PageUnit = element.PageUnit;
          PdfGraphicsState state1 = pdfPage.Graphics.Save();
          pdfPage.Graphics.SetClip(currentBounds);
          PdfGraphicsState state2 = pdfPage.Graphics.Save();
          pdfPage.Graphics.TranslateTransform(currentBounds.X, currentBounds.Y);
          PdfGraphicsState state3 = pdfPage.Graphics.Save();
          PdfTransformationMatrix matrix = new PdfTransformationMatrix();
          float scaleX = shapeLayoutBounds.Width / (float) element.Width;
          float scaleY = shapeLayoutBounds.Height / (float) element.Height;
          matrix.Scale(scaleX, scaleY);
          pdfPage.Graphics.StreamWriter.ModifyCTM(matrix);
          metaRecordParser.Enumerate();
          PdfUnitConvertor pdfUnitConvertor = new PdfUnitConvertor();
          if ((double) pdfPage.Graphics.Split > 0.0)
          {
            TextRegionManager context = renderer.Context as TextRegionManager;
            ImageRegionManager imageContext = metaRecordParser.ImageContext as ImageRegionManager;
            float pixels1 = pdfUnitConvertor.ConvertToPixels(pdfPage.Graphics.Split, PdfGraphicsUnit.Point);
            float topCoordinate1 = context.GetTopCoordinate(pixels1);
            float topCoordinate2 = imageContext.GetTopCoordinate(topCoordinate1);
            float pixels2 = pdfUnitConvertor.ConvertToPixels(pdfPage.Graphics.ClientSize.Width, PdfGraphicsUnit.Point);
            float pixels3 = pdfUnitConvertor.ConvertToPixels(pdfPage.Graphics.ClientSize.Height, PdfGraphicsUnit.Point);
            pdfPage.Graphics.DrawRectangle(PdfBrushes.White, new RectangleF(0.0f, topCoordinate2, pixels2, pixels3 - topCoordinate2));
            pdfPage.Graphics.SetClip(new RectangleF(0.0f, topCoordinate2, pixels2, pixels3 - topCoordinate2));
            pdfPage.Graphics.Split = pdfUnitConvertor.ConvertFromPixels(topCoordinate2, PdfGraphicsUnit.Point);
          }
          pdfPage.Graphics.Restore(state3);
          pdfPage.Graphics.Restore(state2);
          pdfPage.Graphics.Restore(state1);
          pdfPage = renderer.Graphics.Page as PdfPage;
        }
      }
    }
  }

  private RectangleF GetNextShapeBounds(
    RectangleF shapeLayoutBounds,
    ShapeLayouter.ShapeLayoutResult pageResult)
  {
    RectangleF bounds = pageResult.Bounds;
    shapeLayoutBounds.Y += bounds.Height;
    shapeLayoutBounds.Height -= bounds.Height;
    return shapeLayoutBounds;
  }

  private bool FitsToBounds(RectangleF currentBounds, RectangleF shapeLayoutBounds)
  {
    return (double) shapeLayoutBounds.Height <= (double) currentBounds.Height;
  }

  private RectangleF GetDrawBounds(RectangleF currentBounds, RectangleF shapeLayoutBounds)
  {
    RectangleF drawBounds = currentBounds;
    drawBounds.Y -= shapeLayoutBounds.Y;
    drawBounds.Height += shapeLayoutBounds.Y;
    return drawBounds;
  }

  private RectangleF GetPageResultBounds(RectangleF currentBounds, RectangleF shapeLayoutBounds)
  {
    RectangleF pageResultBounds = currentBounds;
    pageResultBounds.Height = Math.Min(pageResultBounds.Height, shapeLayoutBounds.Height);
    return pageResultBounds;
  }

  private void DrawShape(PdfGraphics g, RectangleF currentBounds, RectangleF drawRectangle)
  {
    PdfGraphicsState state = g != null ? g.Save() : throw new ArgumentNullException(nameof (g));
    try
    {
      g.SetClip(currentBounds);
      if (this.Element is PdfMetafile element && (double) currentBounds.Width > 0.0 && (double) currentBounds.Width < (double) element.PhysicalDimension.Width)
        g.ScaleTransform(currentBounds.Width / element.PhysicalDimension.Width, 1f);
      this.Element.Draw(g, drawRectangle.Location);
    }
    finally
    {
      g.Restore(state);
    }
  }

  private EndPageLayoutEventArgs RaiseEndPageLayout(ShapeLayouter.ShapeLayoutResult pageResult)
  {
    EndPageLayoutEventArgs e = (EndPageLayoutEventArgs) null;
    if (this.Element.RaiseEndPageLayout)
    {
      e = new EndPageLayoutEventArgs(this.GetLayoutResult(pageResult));
      this.Element.OnEndPageLayout(e);
    }
    return e;
  }

  private bool RaiseBeforePageLayout(PdfPage currentPage, ref RectangleF currentBounds)
  {
    bool flag = false;
    if (this.Element.RaiseBeginPageLayout)
    {
      BeginPageLayoutEventArgs e = new BeginPageLayoutEventArgs(currentBounds, currentPage);
      this.Element.OnBeginPageLayout(e);
      flag = e.Cancel;
      currentBounds = e.Bounds;
    }
    return flag;
  }

  protected virtual float ToCorrectBounds(
    RectangleF currentBounds,
    RectangleF shapeLayoutBounds,
    PdfPage currentPage)
  {
    return currentBounds.Height;
  }

  private struct ShapeLayoutResult
  {
    public PdfPage Page;
    public RectangleF Bounds;
    public bool End;
  }
}
