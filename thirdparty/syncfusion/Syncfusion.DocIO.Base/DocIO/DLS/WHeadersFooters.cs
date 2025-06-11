// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WHeadersFooters
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using System;
using System.Collections;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WHeadersFooters : XDLSSerializableBase, IEnumerable
{
  private HeaderFooter m_evenHeader;
  private HeaderFooter m_oddFooter;
  private HeaderFooter m_oddHeader;
  private HeaderFooter m_evenFooter;
  private HeaderFooter m_firstPageHeader;
  private HeaderFooter m_firstPageFooter;

  public HeaderFooter Header => this.OddHeader;

  public HeaderFooter Footer => this.OddFooter;

  public HeaderFooter EvenHeader => this.m_evenHeader;

  public HeaderFooter OddHeader => this.m_oddHeader;

  public HeaderFooter EvenFooter => this.m_evenFooter;

  public HeaderFooter OddFooter => this.m_oddFooter;

  public HeaderFooter FirstPageHeader => this.m_firstPageHeader;

  public HeaderFooter FirstPageFooter => this.m_firstPageFooter;

  public bool IsEmpty
  {
    get
    {
      return this.m_evenHeader.ChildEntities.Count == 0 && this.m_evenFooter.ChildEntities.Count == 0 && this.m_oddFooter.ChildEntities.Count == 0 && this.m_oddHeader.ChildEntities.Count == 0 && this.m_firstPageFooter.ChildEntities.Count == 0 && this.m_firstPageHeader.ChildEntities.Count == 0;
    }
  }

  public HeaderFooter this[int index]
  {
    get
    {
      return index >= 0 && index <= 5 ? this[(HeaderFooterType) index] : throw new ArgumentOutOfRangeException(nameof (index), "index can't be less 0 or greater 5");
    }
  }

  public HeaderFooter this[HeaderFooterType hfType]
  {
    get
    {
      switch (hfType)
      {
        case HeaderFooterType.EvenHeader:
          return this.EvenHeader;
        case HeaderFooterType.OddHeader:
          return this.OddHeader;
        case HeaderFooterType.EvenFooter:
          return this.EvenFooter;
        case HeaderFooterType.OddFooter:
          return this.OddFooter;
        case HeaderFooterType.FirstPageHeader:
          return this.FirstPageHeader;
        case HeaderFooterType.FirstPageFooter:
          return this.FirstPageFooter;
        default:
          throw new ArgumentException("Invalid header/footer type", nameof (hfType));
      }
    }
    internal set
    {
      switch (hfType)
      {
        case HeaderFooterType.EvenHeader:
          this.m_evenHeader = value;
          break;
        case HeaderFooterType.OddHeader:
          this.m_oddHeader = value;
          break;
        case HeaderFooterType.EvenFooter:
          this.m_evenFooter = value;
          break;
        case HeaderFooterType.OddFooter:
          this.m_oddFooter = value;
          break;
        case HeaderFooterType.FirstPageHeader:
          this.m_firstPageHeader = value;
          break;
        case HeaderFooterType.FirstPageFooter:
          this.m_firstPageFooter = value;
          break;
        default:
          throw new ArgumentException("Invalid header/footer type", nameof (hfType));
      }
    }
  }

  public bool LinkToPrevious
  {
    get => this.GetLinkToPreviousValue();
    set
    {
      if (this.LinkToPrevious == value)
        return;
      this.UpdateLinkToPrevious(value);
    }
  }

  internal WHeadersFooters(WSection sec)
    : base(sec.Document, (Entity) sec)
  {
    this.m_evenHeader = new HeaderFooter(sec, HeaderFooterType.EvenHeader);
    this.m_oddHeader = new HeaderFooter(sec, HeaderFooterType.OddHeader);
    this.m_evenFooter = new HeaderFooter(sec, HeaderFooterType.EvenFooter);
    this.m_oddFooter = new HeaderFooter(sec, HeaderFooterType.OddFooter);
    this.m_firstPageFooter = new HeaderFooter(sec, HeaderFooterType.FirstPageFooter);
    this.m_firstPageHeader = new HeaderFooter(sec, HeaderFooterType.FirstPageHeader);
  }

  protected override void InitXDLSHolder()
  {
    this.XDLSHolder.AddElement("even-header", (object) this.EvenHeader);
    this.XDLSHolder.AddElement("odd-header", (object) this.OddHeader);
    this.XDLSHolder.AddElement("even-footer", (object) this.EvenFooter);
    this.XDLSHolder.AddElement("odd-footer", (object) this.OddFooter);
    this.XDLSHolder.AddElement("first-page-header", (object) this.FirstPageHeader);
    this.XDLSHolder.AddElement("first-page-footer", (object) this.FirstPageFooter);
  }

  internal WHeadersFooters Clone() => (WHeadersFooters) this.CloneImpl();

  protected override object CloneImpl()
  {
    WHeadersFooters wheadersFooters = (WHeadersFooters) base.CloneImpl();
    wheadersFooters.m_evenHeader = (HeaderFooter) this.m_evenHeader.Clone();
    wheadersFooters.m_oddHeader = (HeaderFooter) this.m_oddHeader.Clone();
    wheadersFooters.m_evenFooter = (HeaderFooter) this.m_evenFooter.Clone();
    wheadersFooters.m_oddFooter = (HeaderFooter) this.m_oddFooter.Clone();
    wheadersFooters.m_firstPageFooter = (HeaderFooter) this.m_firstPageFooter.Clone();
    wheadersFooters.m_firstPageHeader = (HeaderFooter) this.m_firstPageHeader.Clone();
    return (object) wheadersFooters;
  }

  internal new void Close()
  {
    HeaderFooter headerFooter = (HeaderFooter) null;
    for (int index = 0; index < 6; ++index)
    {
      this[index].Close();
      headerFooter = (HeaderFooter) null;
    }
    base.Close();
  }

  internal void InitLayoutInfo(Entity entity, ref bool isLastTOCEntry)
  {
    HeaderFooter headerFooter = (HeaderFooter) null;
    for (int index = 0; index < 6; ++index)
    {
      this[index].InitLayoutInfo(entity, ref isLastTOCEntry);
      headerFooter = (HeaderFooter) null;
      if (isLastTOCEntry)
        break;
    }
  }

  public IEnumerator GetEnumerator() => (IEnumerator) new WHeadersFooters.HFEnumerator(this);

  private bool GetLinkToPreviousValue()
  {
    if (!(this.OwnerBase is WSection ownerBase) || ownerBase.Index <= 0)
      return false;
    bool linkToPreviousValue = true;
    for (int index = 0; index < 6; ++index)
    {
      if (this[index].Items.Count > 0)
      {
        linkToPreviousValue = false;
        break;
      }
    }
    return linkToPreviousValue;
  }

  private void UpdateLinkToPrevious(bool linkToPrevious)
  {
    if (!(this.OwnerBase is WSection ownerBase) || ownerBase.GetIndexInOwnerCollection() <= 0)
      return;
    for (int index = 0; index < 6; ++index)
      this[index].LinkToPrevious = linkToPrevious;
  }

  internal class HFEnumerator : IEnumerator
  {
    private int m_index = -1;
    private WHeadersFooters m_hfs;

    internal HFEnumerator(WHeadersFooters hfs) => this.m_hfs = hfs;

    public object Current => this.m_index >= 0 ? (object) this.m_hfs[this.m_index] : (object) null;

    public bool MoveNext()
    {
      if (this.m_index >= 5)
        return false;
      ++this.m_index;
      return true;
    }

    public void Reset() => this.m_index = -1;
  }
}
