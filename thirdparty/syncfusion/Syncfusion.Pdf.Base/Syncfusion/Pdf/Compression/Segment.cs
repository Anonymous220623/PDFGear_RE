// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.Segment
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Compression;

internal class Segment
{
  private uint m_length;
  private uint m_number;
  private int m_sType;
  private int deferred_non_retain;
  private int m_retainBits;
  private List<int> m_referredTo;
  private uint m_page;

  internal uint Number
  {
    get => this.m_number;
    set => this.m_number = value;
  }

  internal int SType
  {
    get => this.m_sType;
    set => this.m_sType = value;
  }

  internal int RetainBits
  {
    get => this.m_retainBits;
    set => this.m_retainBits = value;
  }

  internal List<int> ReferredTo
  {
    get => this.m_referredTo;
    set => this.m_referredTo = value;
  }

  internal uint Page
  {
    get => this.m_page;
    set => this.m_page = value;
  }

  internal uint Length
  {
    get => this.m_length;
    set => this.m_length = value;
  }

  internal Segment() => this.ReferredTo = new List<int>();

  private int ReferenceSize
  {
    get => this.Number > 256U /*0x0100*/ ? (this.Number > 65536U /*0x010000*/ ? 4 : 2) : 1;
  }

  private int PageSize => this.Page > (uint) byte.MaxValue ? 4 : 1;

  private uint Size
  {
    get
    {
      int referenceSize = this.ReferenceSize;
      int pageSize = this.PageSize;
      return 0;
    }
  }

  internal void Write(List<byte> buf)
  {
    JBIG2Segment jbiG2Segment = new JBIG2Segment();
    jbiG2Segment.Number = JBIG2Statics.Htonl((object) this.Number);
    jbiG2Segment.SType = (byte) this.SType;
    jbiG2Segment.DeferredNonRetain = (byte) this.deferred_non_retain;
    jbiG2Segment.RetainBits = (byte) this.RetainBits;
    jbiG2Segment.SegmentCount = (byte) this.ReferredTo.Count;
    int pageSize = this.PageSize;
    int referenceSize = this.ReferenceSize;
    if (pageSize == 4)
      jbiG2Segment.PageAssocSize = (byte) 1;
    buf.AddRange((IEnumerable<byte>) jbiG2Segment.Serialize());
    foreach (uint p in this.ReferredTo)
    {
      switch (referenceSize)
      {
        case 2:
          buf.Add((byte) JBIG2Statics.Htonl((object) p));
          continue;
        case 4:
          buf.AddRange((IEnumerable<byte>) JBIG2Statics.Htonl(p));
          continue;
        default:
          buf.Add((byte) p);
          continue;
      }
    }
    if (pageSize == 4)
      buf.AddRange((IEnumerable<byte>) JBIG2Statics.Htonl(this.Page));
    else
      buf.Add((byte) this.Page);
    buf.AddRange((IEnumerable<byte>) JBIG2Statics.Htonl(this.Length));
  }
}
