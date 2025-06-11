// Decompiled with JetBrains decompiler
// Type: Syncfusion.ExcelToPdfConverter.HeaderFooterImpl
// Assembly: Syncfusion.ExcelToPDFConverter.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 4304B189-CB46-46CF-B5C1-2287263DCC93
// Assembly location: C:\Program Files\PDFgear\Syncfusion.ExcelToPdfConverter.Base.dll

using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.XlsIO.Implementation;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.ExcelToPdfConverter;

internal class HeaderFooterImpl
{
  private PdfSection _pdfSection;
  private List<HeaderFooter> _headerFooterCollection;
  private PageSetupBaseImpl _pageSetup;
  private float _topMargin;
  private float _bottomMargin;
  private LayoutOptions _layoutOptions;
  private bool _isHeader;
  private bool _isFooter;
  private List<PdfTemplate> _PdfPageTemplateCollection;
  private float _footerMargin;
  private float _headerMargin;

  internal List<PdfTemplate> PdfPageTemplateCollection
  {
    get
    {
      if (this._PdfPageTemplateCollection == null)
        this._PdfPageTemplateCollection = new List<PdfTemplate>();
      return this._PdfPageTemplateCollection;
    }
  }

  internal bool IsHeader
  {
    get => this._isHeader;
    set => this._isHeader = value;
  }

  internal bool IsFooter
  {
    get => this._isFooter;
    set => this._isFooter = value;
  }

  internal LayoutOptions LayoutOptions
  {
    get => this._layoutOptions;
    set => this._layoutOptions = value;
  }

  internal PdfSection PdfSection
  {
    get => this._pdfSection;
    set => this._pdfSection = value;
  }

  internal List<HeaderFooter> HeaderFooterCollection
  {
    get
    {
      if (this._headerFooterCollection == null)
        this._headerFooterCollection = new List<HeaderFooter>();
      return this._headerFooterCollection;
    }
    set => this._headerFooterCollection = value;
  }

  internal PageSetupBaseImpl PageSetup
  {
    get => this._pageSetup;
    set => this._pageSetup = value;
  }

  internal float TopMargin
  {
    get => this._topMargin;
    set => this._topMargin = value;
  }

  internal float BottomMargin
  {
    get => this._bottomMargin;
    set => this._bottomMargin = value;
  }

  internal float FooterMargin
  {
    get => this._headerMargin;
    set => this._headerMargin = value;
  }

  internal float HeaderMargin
  {
    get => this._footerMargin;
    set => this._footerMargin = value;
  }
}
