// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Security.WordKey
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Security;

internal class WordKey
{
  private byte[] m_baState = new byte[256 /*0x0100*/];
  private byte m_bX;
  private byte m_bY;

  internal byte[] status
  {
    get => this.m_baState;
    set
    {
      if (this.m_baState == value)
        return;
      this.m_baState = value;
    }
  }

  internal byte x
  {
    get => this.m_bX;
    set
    {
      if ((int) this.m_bX == (int) value)
        return;
      this.m_bX = value;
    }
  }

  internal byte y
  {
    get => this.m_bY;
    set
    {
      if ((int) this.m_bY == (int) value)
        return;
      this.m_bY = value;
    }
  }
}
