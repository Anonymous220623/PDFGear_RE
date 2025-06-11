// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.TextLayouter
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

internal class TextLayouter(PdfTextElement element) : ElementLayouter((PdfLayoutElement) element)
{
  private PdfStringFormat m_format;
  private float m_maxValue;

  public PdfTextElement Element => base.Element as PdfTextElement;

  protected override PdfLayoutResult LayoutInternal(PdfLayoutParams param)
  {
    if (param == null)
      throw new ArgumentNullException(nameof (param));
    this.m_format = this.Element.StringFormat != null ? (PdfStringFormat) this.Element.StringFormat.Clone() : (PdfStringFormat) null;
    PdfPage currentPage = param.Page;
    RectangleF currentBounds = param.Bounds;
    string remainder = this.Element.Value;
    this.m_maxValue = currentBounds.Height;
    TextLayouter.TextPageLayoutResult pageResult = new TextLayouter.TextPageLayoutResult();
    pageResult.Page = currentPage;
    pageResult.Remainder = remainder;
    while (true)
    {
      bool flag = this.RaiseBeforePageLayout(currentPage, ref currentBounds);
      EndTextPageLayoutEventArgs pageLayoutEventArgs = (EndTextPageLayoutEventArgs) null;
      if (!flag)
      {
        pageResult = this.LayoutOnPage(remainder, currentPage, currentBounds, param);
        pageLayoutEventArgs = this.RaisePageLayouted(pageResult);
        flag = pageLayoutEventArgs != null && pageLayoutEventArgs.Cancel;
      }
      if (!pageResult.End && !flag)
      {
        if (!this.Element.ispdfTextElement)
        {
          currentBounds = this.GetPaginateBounds(param);
          if (this.Element.m_pdfHtmlTextElement && param.Format.UsePaginateBounds && (double) currentBounds.Height > (double) param.Bounds.Height)
            currentBounds.Height = param.Bounds.Height;
          remainder = pageResult.Remainder;
          currentPage = pageLayoutEventArgs == null || pageLayoutEventArgs.NextPage == null ? this.GetNextPage(currentPage) : pageLayoutEventArgs.NextPage;
        }
        else
          break;
      }
      else
        goto label_11;
    }
    PdfTextLayoutResult layoutResult = this.GetLayoutResult(pageResult);
    goto label_12;
label_11:
    layoutResult = this.GetLayoutResult(pageResult);
label_12:
    return (PdfLayoutResult) layoutResult;
  }

  private PdfTextLayoutResult GetLayoutResult(TextLayouter.TextPageLayoutResult pageResult)
  {
    return new PdfTextLayoutResult(pageResult.Page, pageResult.Bounds, pageResult.Remainder, pageResult.LastLineBounds);
  }

  private TextLayouter.TextPageLayoutResult LayoutOnPage(
    string text,
    PdfPage currentPage,
    RectangleF currentBounds,
    PdfLayoutParams param)
  {
    if (text == null)
      throw new ArgumentNullException(nameof (text));
    if (currentPage == null)
      throw new ArgumentNullException(nameof (currentPage));
    if (param == null)
      throw new ArgumentNullException(nameof (param));
    TextLayouter.TextPageLayoutResult pageLayoutResult = new TextLayouter.TextPageLayoutResult();
    pageLayoutResult.Remainder = text;
    pageLayoutResult.Page = currentPage;
    currentBounds = this.CheckCorrectBounds(currentPage, currentBounds);
    if ((double) currentBounds.Height < 0.0)
    {
      currentPage = this.GetNextPage(currentPage);
      PdfMargins margins = currentPage.Section.PageSettings.Margins;
      pageLayoutResult.Page = currentPage;
      currentBounds = new RectangleF(currentBounds.X, 0.0f, currentBounds.Width, currentBounds.Height);
    }
    PdfStringLayoutResult stringLayoutResult = new PdfStringLayouter().Layout(text, this.Element.Font, this.m_format, currentBounds, currentPage.GetClientSize().Height);
    bool flag1 = stringLayoutResult.Remainder == null || stringLayoutResult.Remainder.Length == 0;
    bool flag2 = (param.Format.Break != PdfLayoutBreakType.FitElement || currentPage != param.Page || flag1) && !stringLayoutResult.Empty;
    if (flag2)
    {
      PdfGraphics graphics = currentPage.Graphics;
      if (this.Element.PdfTag != null)
        graphics.Tag = this.Element.PdfTag;
      graphics.DrawStringLayoutResult(stringLayoutResult, this.Element.Font, this.Element.Pen, this.Element.ObtainBrush(), currentBounds, this.m_format);
      LineInfo line = stringLayoutResult.Lines[stringLayoutResult.LineCount - 1];
      pageLayoutResult.LastLineBounds = graphics.GetLineBounds(stringLayoutResult.LineCount - 1, stringLayoutResult, this.Element.Font, currentBounds, this.m_format);
      pageLayoutResult.Bounds = this.GetTextPageBounds(currentPage, currentBounds, stringLayoutResult);
      pageLayoutResult.Remainder = stringLayoutResult.Remainder;
      this.CheckCorectStringFormat(line);
    }
    else
      pageLayoutResult.Bounds = this.GetTextPageBounds(currentPage, currentBounds, stringLayoutResult);
    bool flag3 = stringLayoutResult.Empty && param.Format.Break != PdfLayoutBreakType.FitElement && param.Format.Layout != PdfLayoutType.Paginate && flag2 || param.Format.Break == PdfLayoutBreakType.FitElement && currentPage != param.Page;
    pageLayoutResult.End = flag1 || flag3 || param.Format.Layout == PdfLayoutType.OnePage;
    if (!pageLayoutResult.End && this.Element.m_pdfHtmlTextElement)
    {
      if ((double) pageLayoutResult.Bounds.Height != 0.0 && (double) this.m_maxValue < (double) pageLayoutResult.Bounds.Height + (double) this.Element.Font.Height)
      {
        pageLayoutResult.End = true;
      }
      else
      {
        param.Bounds = new RectangleF(currentBounds.X, currentBounds.Y, currentBounds.Width, this.m_maxValue - pageLayoutResult.Bounds.Height);
        this.m_maxValue = param.Bounds.Height;
      }
    }
    if (!pageLayoutResult.End && this.Element.m_pdfHtmlTextElement && (double) pageLayoutResult.Bounds.Height == 0.0)
    {
      currentBounds = new RectangleF(currentBounds.X, currentBounds.Y, currentBounds.Width, currentPage.GetClientSize().Height);
      pageLayoutResult = this.LayoutOnPage(text, currentPage, currentBounds, param);
    }
    return pageLayoutResult;
  }

  private RectangleF CheckCorrectBounds(PdfPage currentPage, RectangleF currentBounds)
  {
    if (currentPage == null)
      throw new ArgumentNullException(nameof (currentPage));
    SizeF clientSize = currentPage.Graphics.ClientSize;
    currentBounds.Height = (double) currentBounds.Height > 0.0 ? currentBounds.Height : clientSize.Height - currentBounds.Y;
    return currentBounds;
  }

  private RectangleF GetTextPageBounds(
    PdfPage currentPage,
    RectangleF currentBounds,
    PdfStringLayoutResult stringResult)
  {
    if (currentPage == null)
      throw new ArgumentNullException(nameof (currentPage));
    SizeF textSize = stringResult != null ? stringResult.ActualSize : throw new ArgumentNullException(nameof (stringResult));
    float x = currentBounds.X;
    float y1 = currentBounds.Y;
    float width = (double) currentBounds.Width > 0.0 ? currentBounds.Width : textSize.Width;
    float height = textSize.Height;
    RectangleF rectangleF = currentPage.Graphics.CheckCorrectLayoutRectangle(textSize, currentBounds.X, currentBounds.Y, this.m_format);
    if ((double) currentBounds.Width <= 0.0)
      x = rectangleF.X;
    if ((double) currentBounds.Height <= 0.0)
      y1 = rectangleF.Y;
    float verticalAlignShift = currentPage.Graphics.GetTextVerticalAlignShift(textSize.Height, currentBounds.Height, this.m_format);
    float y2 = y1 + verticalAlignShift;
    return new RectangleF(x, y2, width, height);
  }

  private EndTextPageLayoutEventArgs RaisePageLayouted(TextLayouter.TextPageLayoutResult pageResult)
  {
    EndTextPageLayoutEventArgs e = (EndTextPageLayoutEventArgs) null;
    if (this.Element.RaiseEndPageLayout)
    {
      e = new EndTextPageLayoutEventArgs(this.GetLayoutResult(pageResult));
      this.Element.OnEndPageLayout((EndPageLayoutEventArgs) e);
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

  private void CheckCorectStringFormat(LineInfo lineInfo)
  {
    if (this.m_format == null)
      return;
    this.m_format.FirstLineIndent = (lineInfo.LineType & LineType.NewLineBreak) > LineType.None ? this.Element.StringFormat.FirstLineIndent : 0.0f;
  }

  private struct TextPageLayoutResult
  {
    public PdfPage Page;
    public RectangleF Bounds;
    public bool End;
    public string Remainder;
    public RectangleF LastLineBounds;
  }
}
