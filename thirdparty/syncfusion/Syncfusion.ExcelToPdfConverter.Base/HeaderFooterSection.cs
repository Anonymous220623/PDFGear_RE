// Decompiled with JetBrains decompiler
// Type: Syncfusion.ExcelToPdfConverter.HeaderFooterSection
// Assembly: Syncfusion.ExcelToPDFConverter.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 4304B189-CB46-46CF-B5C1-2287263DCC93
// Assembly location: C:\Program Files\PDFgear\Syncfusion.ExcelToPdfConverter.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.XlsIO.Implementation;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.ExcelToPdfConverter;

internal class HeaderFooterSection
{
  private Dictionary<string, HeaderFooterFontColorSettings> headerFooterCollections;
  private float headerFooterHeight;
  private string headerFooterSectionName;
  private float headerFooterWidth;
  private List<RichTextString> m_rtf;
  private PdfTextAlignment m_textAlignment;
  private bool _isPageCount;

  internal bool IsPageCount
  {
    get => this._isPageCount;
    set => this._isPageCount = value;
  }

  internal float Width
  {
    get => this.headerFooterWidth;
    set => this.headerFooterWidth = value;
  }

  internal float Height
  {
    get => this.headerFooterHeight;
    set => this.headerFooterHeight = value;
  }

  internal string SectionName
  {
    get => this.headerFooterSectionName;
    set => this.headerFooterSectionName = value;
  }

  internal Dictionary<string, HeaderFooterFontColorSettings> HeaderFooterCollections
  {
    get => this.headerFooterCollections;
    set => this.headerFooterCollections = value;
  }

  internal List<RichTextString> RTF
  {
    get => this.m_rtf;
    set => this.m_rtf = value;
  }

  internal PdfTextAlignment TextAlignment
  {
    get => this.m_textAlignment;
    set => this.m_textAlignment = value;
  }

  internal HeaderFooterSection Clone() => (HeaderFooterSection) this.MemberwiseClone();
}
