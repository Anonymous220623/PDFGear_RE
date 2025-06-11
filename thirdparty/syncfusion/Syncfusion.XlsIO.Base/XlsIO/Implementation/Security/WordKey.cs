// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Security.WordKey
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Security;

public class WordKey
{
  private byte[] m_baState = new byte[256 /*0x0100*/];
  private byte m_bX;
  private byte m_bY;

  public byte[] Status
  {
    get => this.m_baState;
    set
    {
      if (this.m_baState == value)
        return;
      this.m_baState = value;
    }
  }

  public byte X
  {
    get => this.m_bX;
    set
    {
      if ((int) this.m_bX == (int) value)
        return;
      this.m_bX = value;
    }
  }

  public byte Y
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
