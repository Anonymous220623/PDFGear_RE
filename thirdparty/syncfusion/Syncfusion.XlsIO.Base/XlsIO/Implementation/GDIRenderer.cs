// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.GDIRenderer
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class GDIRenderer : RendererBase
{
  private Graphics _graphics;
  private WorkbookImpl _workbookImpl;
  private int _scale;
  private StringFormat _stringFormat;

  internal override bool IsHfRtfProcess
  {
    get => false;
    set
    {
    }
  }

  internal override RectangleF HfImageBounds
  {
    get => new RectangleF();
    set
    {
    }
  }

  internal GDIRenderer(
    IRange cell,
    Graphics graphics,
    StringFormat stringFormat,
    WorksheetImageConverter converter,
    List<IFont> fonts,
    List<string> drawString,
    WorkbookImpl workbook,
    int scale)
    : base(cell, stringFormat, fonts, drawString, workbook)
  {
    this._graphics = graphics;
    this._stringFormat = stringFormat;
    this._workbookImpl = cell.Worksheet.Workbook as WorkbookImpl;
    this._scale = scale;
    this.SetWidth(converter.LeftWidth, converter.RightWidth);
  }

  internal GDIRenderer(
    Graphics graphics,
    StringFormat stringFormat,
    List<IFont> fonts,
    List<string> drawString,
    WorkbookImpl workbook,
    int scale)
    : base((IRange) null, stringFormat, fonts, drawString, workbook)
  {
    this._graphics = graphics;
    this._stringFormat = stringFormat;
    this._workbookImpl = workbook;
    this._scale = scale;
  }

  internal override string CheckPdfFont(Font sysFont, string testString) => sysFont.Name;

  internal override bool CheckUnicode(string text) => false;

  internal override void DrawString(TextInfoImpl textInfo, StringFormat stringFormat)
  {
    this._graphics.DrawString(textInfo.Text, this.GetSystemFont(textInfo), (Brush) new SolidBrush(this.NormalizeColor(textInfo.Font.RGBColor)), textInfo.Bounds, this._stringFormat);
  }

  internal override void DrawTextTemplate(
    RectangleF bounds,
    List<LineInfoImpl> lineInfoCollection,
    float y)
  {
    this.DrawTextTemplate(bounds, this._graphics, lineInfoCollection, y);
  }

  internal override float FindAscent(string text, Font font)
  {
    int emHeight = font.FontFamily.GetEmHeight(font.Style);
    int cellAscent = font.FontFamily.GetCellAscent(font.Style);
    return (float) ((double) font.SizeInPoints * 96.0 / 72.0) * (float) cellAscent / (float) emHeight;
  }

  private void DrawTextTemplate(
    RectangleF bounds,
    Graphics graphics,
    List<LineInfoImpl> lineInfoCollection,
    float y)
  {
    foreach (LineInfoImpl lineInfo in lineInfoCollection)
    {
      foreach (TextInfoImpl textInfo in lineInfo.TextInfoCollection)
      {
        textInfo.Y += y;
        this._graphics.SetClip(bounds);
        this._graphics.DrawString(textInfo.Text, this.GetSystemFont(textInfo), (Brush) new SolidBrush(this.NormalizeColor(textInfo.Font.RGBColor)), textInfo.Bounds, this._stringFormat);
        this._graphics.ResetClip();
      }
    }
  }

  internal override void InitializeStringFormat()
  {
    this._stringFormat = new StringFormat(StringFormat.GenericTypographic);
    this._stringFormat.FormatFlags &= ~StringFormatFlags.LineLimit;
    this._stringFormat.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
    this._stringFormat.FormatFlags |= StringFormatFlags.NoClip;
  }

  internal override SizeF MeasureString(string text, Font systemFont)
  {
    return this._graphics.MeasureString(text, systemFont, new PointF(0.0f, 0.0f), this._stringFormat);
  }

  internal override string SwitchFonts(string text, byte charSet, string fontName) => fontName;

  internal override bool IsJustify() => false;
}
