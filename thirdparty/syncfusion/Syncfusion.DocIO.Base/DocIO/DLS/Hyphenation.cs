// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.Hyphenation
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class Hyphenation
{
  private IWordDocument m_document;

  public bool AutoHyphenation
  {
    get => (this.m_document as WordDocument).DOP.AutoHyphen;
    set => (this.m_document as WordDocument).DOP.AutoHyphen = value;
  }

  public bool HyphenateCaps
  {
    get => (this.m_document as WordDocument).DOP.HyphCapitals;
    set => (this.m_document as WordDocument).DOP.HyphCapitals = value;
  }

  public float HyphenationZone
  {
    get => (float) ((this.m_document as WordDocument).DOP.DxaHotZ / 20);
    set
    {
      if ((double) value < 0.05 || (double) value > 1584.0)
        throw new ArgumentOutOfRangeException("Hyphenation zone must be between 0.05 pt and 1584 pt.");
      (this.m_document as WordDocument).DOP.DxaHotZ = (int) ((double) value * 20.0);
    }
  }

  public int ConsecutiveHyphensLimit
  {
    get => (this.m_document as WordDocument).DOP.ConsecHypLim;
    set
    {
      (this.m_document as WordDocument).DOP.ConsecHypLim = value >= 0 && value <= (int) short.MaxValue ? value : throw new ArgumentOutOfRangeException("Consecutive hyphens limit must be between 0 and 32767.");
    }
  }

  internal Hyphenation(IWordDocument document) => this.m_document = document;

  internal void Close() => this.m_document = (IWordDocument) null;
}
