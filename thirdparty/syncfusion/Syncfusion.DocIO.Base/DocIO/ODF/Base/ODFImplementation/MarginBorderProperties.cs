// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.MarginBorderProperties
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class MarginBorderProperties : BorderProperties
{
  private const byte MarginLeftKey = 0;
  private const byte MarginRightKey = 1;
  private const byte MarginTopKey = 2;
  private const byte MarginBottomKey = 3;
  private double m_marginLeft;
  private double m_marginRight;
  private double m_marginTop;
  private double m_marginBottom;
  internal byte m_marginFlag;

  internal double MarginLeft
  {
    get => this.m_marginLeft;
    set
    {
      this.m_marginFlag = (byte) ((int) this.m_marginFlag & 254 | 1);
      this.m_marginLeft = value;
    }
  }

  internal double MarginRight
  {
    get => this.m_marginRight;
    set
    {
      this.m_marginFlag = (byte) ((int) this.m_marginFlag & 253 | 2);
      this.m_marginRight = value;
    }
  }

  internal double MarginTop
  {
    get => this.m_marginTop;
    set
    {
      this.m_marginFlag = (byte) ((int) this.m_marginFlag & 251 | 4);
      this.m_marginTop = value;
    }
  }

  internal double MarginBottom
  {
    get => this.m_marginBottom;
    set
    {
      this.m_marginFlag = (byte) ((int) this.m_marginFlag & 247 | 8);
      this.m_marginBottom = value;
    }
  }
}
