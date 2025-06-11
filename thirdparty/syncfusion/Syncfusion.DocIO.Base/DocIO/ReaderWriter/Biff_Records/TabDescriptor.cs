// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.TabDescriptor
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

internal class TabDescriptor
{
  internal const int DEF_TAB_LENGTH = 1;
  private TabJustification m_jc;
  private TabLeader m_tlc;

  internal TabJustification Justification
  {
    get => this.m_jc;
    set
    {
      if (value == this.m_jc)
        return;
      this.m_jc = value;
    }
  }

  internal TabLeader TabLeader
  {
    get => this.m_tlc;
    set
    {
      if (value == this.m_tlc)
        return;
      this.m_tlc = value;
    }
  }

  internal TabDescriptor(byte options)
  {
    this.m_jc = (TabJustification) (byte) ((uint) options & 7U);
    this.m_tlc = (TabLeader) (byte) (((int) options & 56) >> 3);
  }

  internal TabDescriptor(TabJustification justification, TabLeader leader)
  {
    this.m_jc = justification;
    this.m_tlc = leader;
  }

  internal byte Save() => (byte) ((uint) (byte) this.m_tlc << 3 | (uint) (byte) this.m_jc);
}
