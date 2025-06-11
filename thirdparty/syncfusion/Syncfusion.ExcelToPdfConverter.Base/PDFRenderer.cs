// Decompiled with JetBrains decompiler
// Type: Syncfusion.ExcelToPdfConverter.PDFRenderer
// Assembly: Syncfusion.ExcelToPDFConverter.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 4304B189-CB46-46CF-B5C1-2287263DCC93
// Assembly location: C:\Program Files\PDFgear\Syncfusion.ExcelToPdfConverter.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Primitives;
using Syncfusion.XlsIO;
using Syncfusion.XlsIO.Implementation;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.ExcelToPdfConverter;

internal class PDFRenderer : RendererBase
{
  private PdfStringFormat _pdfStringFormat;
  private Syncfusion.ExcelToPdfConverter.ExcelToPdfConverter _excelToPdfConverter;
  private PdfGraphics _pdfGraphics;
  private WorkbookImpl _workbookImpl;
  private static StringFormat _stringFormat;

  internal override bool IsHfRtfProcess
  {
    get => this._excelToPdfConverter.IsHFRTFProcess;
    set => this._excelToPdfConverter.IsHFRTFProcess = value;
  }

  internal override RectangleF HfImageBounds
  {
    get => this._excelToPdfConverter.HfImageBounds;
    set => this._excelToPdfConverter.HfImageBounds = value;
  }

  internal PDFRenderer(
    IRange cell,
    PdfGraphics pdfGraphics,
    Syncfusion.ExcelToPdfConverter.ExcelToPdfConverter excelToPdfConverter,
    PdfStringFormat pdfStringFormat,
    List<IFont> fonts,
    List<string> drawString,
    WorkbookImpl workbook)
    : base(cell, PDFRenderer.GetStrigFormat(pdfStringFormat), fonts, drawString, workbook)
  {
    this._pdfGraphics = pdfGraphics;
    this._excelToPdfConverter = excelToPdfConverter;
    this._pdfStringFormat = pdfStringFormat;
    this._pdfStringFormat.MeasureTrailingSpaces = true;
    this._pdfStringFormat.NoClip = true;
    this._workbookImpl = workbook;
    this.IsHfRtfProcess = excelToPdfConverter.IsHFRTFProcess;
    this.SetWidth(excelToPdfConverter.LeftWidth, excelToPdfConverter.RightWidth);
  }

  internal override void InitializeStringFormat()
  {
    bool rightToLeft = this._pdfStringFormat.RightToLeft;
    PdfTextDirection textDirection = this._pdfStringFormat.TextDirection;
    this._pdfStringFormat = new PdfStringFormat();
    this._pdfStringFormat.ComplexScript = this._excelToPdfConverter.ExcelToPdfSettings.AutoDetectComplexScript;
    this._pdfStringFormat.RightToLeft = rightToLeft;
    this._pdfStringFormat.TextDirection = textDirection;
  }

  private static StringFormat GetStrigFormat(PdfStringFormat pdfStringFormat)
  {
    PDFRenderer._stringFormat = new StringFormat();
    switch (pdfStringFormat.LineAlignment)
    {
      case PdfVerticalAlignment.Top:
        PDFRenderer._stringFormat.LineAlignment = StringAlignment.Near;
        break;
      case PdfVerticalAlignment.Middle:
        PDFRenderer._stringFormat.LineAlignment = StringAlignment.Center;
        break;
      case PdfVerticalAlignment.Bottom:
        PDFRenderer._stringFormat.LineAlignment = StringAlignment.Far;
        break;
    }
    switch (pdfStringFormat.Alignment)
    {
      case PdfTextAlignment.Left:
        PDFRenderer._stringFormat.Alignment = StringAlignment.Near;
        break;
      case PdfTextAlignment.Center:
        PDFRenderer._stringFormat.Alignment = StringAlignment.Center;
        break;
      case PdfTextAlignment.Right:
        PDFRenderer._stringFormat.Alignment = StringAlignment.Far;
        break;
    }
    switch (pdfStringFormat.WordWrap)
    {
      case PdfWordWrapType.Word:
      case PdfWordWrapType.WordOnly:
        PDFRenderer._stringFormat.Trimming = StringTrimming.Word;
        break;
    }
    return PDFRenderer._stringFormat;
  }

  private static StringFormat UpdateLineAlignment(
    StringFormat stringFormat,
    PdfStringFormat pdfStringFormat)
  {
    switch (pdfStringFormat.LineAlignment)
    {
      case PdfVerticalAlignment.Top:
        stringFormat.LineAlignment = StringAlignment.Near;
        break;
      case PdfVerticalAlignment.Middle:
        stringFormat.LineAlignment = StringAlignment.Center;
        break;
      case PdfVerticalAlignment.Bottom:
        stringFormat.Alignment = StringAlignment.Far;
        break;
    }
    return stringFormat;
  }

  private static StringFormat UpdateAlignment(
    StringFormat stringFormat,
    PdfStringFormat pdfStringFormat)
  {
    switch (pdfStringFormat.Alignment)
    {
      case PdfTextAlignment.Left:
        stringFormat.Alignment = StringAlignment.Near;
        break;
      case PdfTextAlignment.Center:
        stringFormat.Alignment = StringAlignment.Center;
        break;
      case PdfTextAlignment.Right:
        stringFormat.Alignment = StringAlignment.Far;
        break;
    }
    return stringFormat;
  }

  internal override string CheckPdfFont(Font sysFont, string testString)
  {
    string str = sysFont.Name;
    if (!(this._excelToPdfConverter.GetPdfFont(sysFont, true, (Stream) null) as PdfTrueTypeFont).TtfReader.IsFontContainsString(testString))
      str = "Arial Unicode MS";
    return str;
  }

  private PdfFont GetAlternatePdfFont(Font systemFont, string text)
  {
    Stream stream = (Stream) null;
    string fontStream = (this._workbookImpl.Application as ApplicationImpl).TryGetFontStream(systemFont.OriginalFontName, systemFont.Size, systemFont.Style, out stream);
    PdfFont pdfFont;
    if (stream != null && stream.Length > 0L)
    {
      systemFont = this._excelToPdfConverter.GetFont(fontStream, (int) systemFont.Size, systemFont.Underline, systemFont.Strikeout, systemFont);
      this._excelToPdfConverter.ExcelToPdfSettings.EmbedFonts = true;
      pdfFont = this._excelToPdfConverter.GetPdfFont(systemFont, true, stream);
    }
    else
      pdfFont = this._excelToPdfConverter.GetPdfFont(systemFont, this._excelToPdfConverter.ExcelToPdfSettings.EmbedFonts || PdfString.IsUnicode(text), (Stream) null);
    return pdfFont;
  }

  internal override void DrawString(TextInfoImpl textInfo, StringFormat stringFormat)
  {
    PdfFont alternatePdfFont = this.GetAlternatePdfFont(this.GetSystemFont(textInfo), textInfo.Text);
    this._pdfGraphics.DrawString(textInfo.Text, alternatePdfFont, (PdfBrush) new PdfSolidBrush((PdfColor) this.NormalizeColor(textInfo.Font.RGBColor)), textInfo.Bounds, this._pdfStringFormat, this._excelToPdfConverter.MaxRowFontSize, this._excelToPdfConverter.MaxPdfFont ?? alternatePdfFont, this._excelToPdfConverter.MaxRowPdfTextformat ?? this._pdfStringFormat);
  }

  internal override void DrawTextTemplate(
    RectangleF bounds,
    List<LineInfoImpl> lineInfoCollection,
    float y)
  {
    this.DrawTextTemplate(bounds, this._pdfGraphics, lineInfoCollection, y);
  }

  internal override SizeF MeasureString(string text, Font sysFont)
  {
    return this.GetAlternatePdfFont(sysFont, text).MeasureString(text, this._pdfStringFormat);
  }

  private void DrawTextTemplate(
    RectangleF bounds,
    PdfGraphics graphics,
    List<LineInfoImpl> lineInfoCollection,
    float y)
  {
    PdfTemplate template = new PdfTemplate(bounds.Width, bounds.Height);
    foreach (LineInfoImpl lineInfo in lineInfoCollection)
    {
      foreach (TextInfoImpl textInfo in lineInfo.TextInfoCollection)
      {
        textInfo.X -= bounds.X;
        textInfo.Y = textInfo.Y - bounds.Y + y;
        if (this.IsHfRtfProcess && textInfo.Text.Contains("HeaderFooterImage"))
        {
          this.HfImageBounds = textInfo.Bounds;
        }
        else
        {
          PdfFont pdfTrueTypeFont = (PdfFont) this._excelToPdfConverter.GetPdfTrueTypeFont(this.GetSystemFont(textInfo), this._excelToPdfConverter.ExcelToPdfSettings.EmbedFonts || Encoding.UTF8.GetByteCount(textInfo.Text) != textInfo.Text.Length, (Stream) null);
          template.Graphics.DrawString(textInfo.Text, pdfTrueTypeFont, (PdfBrush) new PdfSolidBrush((PdfColor) this.NormalizeColor(textInfo.Font.RGBColor)), textInfo.Bounds, this._pdfStringFormat);
        }
      }
    }
    this._pdfGraphics.DrawPdfTemplate(template, bounds.Location, template.Size);
  }

  internal override string SwitchFonts(string text, byte charSet, string fontName)
  {
    return this._excelToPdfConverter.SwitchFonts(text, charSet, fontName);
  }

  internal override bool CheckUnicode(string text) => this._excelToPdfConverter.CheckUnicode(text);

  internal override bool IsJustify() => this._pdfStringFormat.Alignment == PdfTextAlignment.Justify;

  internal float GetAscent(Font font)
  {
    return this._excelToPdfConverter.GetPdfTrueTypeFont(font, false, (Stream) null).Metrics.GetAscent(this._pdfStringFormat);
  }
}
