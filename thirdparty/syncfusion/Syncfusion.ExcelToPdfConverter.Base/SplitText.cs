// Decompiled with JetBrains decompiler
// Type: Syncfusion.ExcelToPdfConverter.SplitText
// Assembly: Syncfusion.ExcelToPDFConverter.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 4304B189-CB46-46CF-B5C1-2287263DCC93
// Assembly location: C:\Program Files\PDFgear\Syncfusion.ExcelToPdfConverter.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.XlsIO;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.ExcelToPdfConverter;

public class SplitText
{
  private int _adjacentColumn;
  private PdfBrush _brush;
  private PdfStringFormat _format;
  private RectangleF _originRect;
  private PdfTemplate _pageTemplate;
  private PdfFont _pdfFont;
  private int _row;
  private IWorksheet _sheet;
  private string _text;
  private List<IFont> _richTextFont;
  private List<string> _richText;

  internal RectangleF OriginRect
  {
    get => this._originRect;
    set => this._originRect = value;
  }

  internal IWorksheet Sheet
  {
    get => this._sheet;
    set => this._sheet = value;
  }

  internal string Text
  {
    get => this._text;
    set => this._text = value;
  }

  internal PdfFont TextFont
  {
    get => this._pdfFont;
    set => this._pdfFont = value;
  }

  internal PdfBrush Brush
  {
    get => this._brush;
    set => this._brush = value;
  }

  internal PdfStringFormat Format
  {
    get => this._format;
    set => this._format = value;
  }

  internal int Row
  {
    get => this._row;
    set => this._row = value;
  }

  internal int AdjacentColumn
  {
    get => this._adjacentColumn;
    set => this._adjacentColumn = value;
  }

  internal PdfTemplate PageTemplate
  {
    get => this._pageTemplate;
    set => this._pageTemplate = value;
  }

  internal List<IFont> RichTextFont
  {
    get => this._richTextFont;
    set => this._richTextFont = value;
  }

  internal List<string> RichText
  {
    get => this._richText;
    set => this._richText = value;
  }
}
