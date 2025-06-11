// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2FileHeader
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Compression;

internal struct JBIG2FileHeader
{
  private byte[] m_id;
  private byte m_organisationType;
  private byte m_unknownNPages;
  private byte m_reserved;
  private uint m_nPages;

  internal byte[] Id
  {
    get => this.m_id;
    set => this.m_id = value;
  }

  internal byte OrganisationType
  {
    get => this.m_organisationType;
    set => this.m_organisationType = value;
  }

  internal byte UnknownNPages
  {
    get => this.m_unknownNPages;
    set => this.m_unknownNPages = value;
  }

  internal byte Reserved
  {
    get => this.m_reserved;
    set => this.m_reserved = value;
  }

  internal uint NPages
  {
    get => this.m_nPages;
    set => this.m_nPages = value;
  }
}
