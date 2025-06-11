// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.DocPartItem
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class DocPartItem
{
  private string m_docPartCategory;
  private string m_docPartGallery;
  private byte m_bFlags;

  internal string DocPartCategory
  {
    get => this.m_docPartCategory;
    set => this.m_docPartCategory = value;
  }

  internal string DocPartGallery
  {
    get => this.m_docPartGallery;
    set => this.m_docPartGallery = value;
  }

  internal bool IsDocPartUnique
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }
}
