// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.HeaderFooter.HeadersFooters
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation.HeaderFooter;

internal class HeadersFooters : IHeadersFooters
{
  private Syncfusion.Presentation.HeaderFooter.HeaderFooter _header;
  private Syncfusion.Presentation.HeaderFooter.HeaderFooter _footer;
  private Syncfusion.Presentation.HeaderFooter.HeaderFooter _dateAndTime;
  private Syncfusion.Presentation.HeaderFooter.HeaderFooter _slideNumber;
  private IBaseSlide _baseSlide;

  internal HeadersFooters(IBaseSlide baseSlide)
  {
    this._baseSlide = baseSlide;
    if (this._baseSlide is INotesSlide)
      this._header = new Syncfusion.Presentation.HeaderFooter.HeaderFooter(this._baseSlide, HeaderFooterType.Header);
    this._footer = new Syncfusion.Presentation.HeaderFooter.HeaderFooter(this._baseSlide, HeaderFooterType.Footer);
    this._dateAndTime = new Syncfusion.Presentation.HeaderFooter.HeaderFooter(this._baseSlide, HeaderFooterType.DateAndTime);
    this._slideNumber = new Syncfusion.Presentation.HeaderFooter.HeaderFooter(this._baseSlide, HeaderFooterType.SlideNumber);
  }

  public IHeaderFooter Header => (IHeaderFooter) this._header;

  public IHeaderFooter Footer => (IHeaderFooter) this._footer;

  public IHeaderFooter DateAndTime => (IHeaderFooter) this._dateAndTime;

  public IHeaderFooter SlideNumber => (IHeaderFooter) this._slideNumber;

  internal IBaseSlide BaseSlide => this._baseSlide;

  internal IHeadersFooters Clone()
  {
    HeadersFooters headersFooters = (HeadersFooters) this.MemberwiseClone();
    if (this._header != null)
      headersFooters._header = this._header.Clone();
    headersFooters._footer = this._footer.Clone();
    headersFooters._slideNumber = this._slideNumber.Clone();
    headersFooters._dateAndTime = this._dateAndTime.Clone();
    return (IHeadersFooters) headersFooters;
  }

  internal void SetParent(IBaseSlide newParent)
  {
    if (this._header != null)
      this._header.SetParent(newParent);
    this._footer.SetParent(newParent);
    this._slideNumber.SetParent(newParent);
    this._dateAndTime.SetParent(newParent);
  }

  internal void Dispose()
  {
    if (this._header != null)
      this._header = (Syncfusion.Presentation.HeaderFooter.HeaderFooter) null;
    if (this._footer != null)
      this._footer = (Syncfusion.Presentation.HeaderFooter.HeaderFooter) null;
    if (this._slideNumber != null)
      this._slideNumber = (Syncfusion.Presentation.HeaderFooter.HeaderFooter) null;
    if (this._dateAndTime == null)
      return;
    this._dateAndTime = (Syncfusion.Presentation.HeaderFooter.HeaderFooter) null;
  }
}
