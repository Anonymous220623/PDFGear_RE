// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.CommonTableParaProperties
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class CommonTableParaProperties : MarginBorderProperties
{
  private const byte WritingModeKey = 0;
  private const byte BeforeBreakKey = 1;
  private const byte AfterBreakKey = 2;
  private const byte KeepWithNextKey = 3;
  private const byte ShadowTypeKey = 4;
  private const byte PageNumberKey = 5;
  private const byte BackgroundColorKey = 6;
  private string m_backgroundColor;
  private KeepTogether m_keepWithNext;
  private int m_pageNumber;
  private AfterBreak m_afterBreak;
  private BeforeBreak m_beforeBreak;
  private string m_shadowType;
  private WritingMode m_writingMode;
  internal byte m_CommonstyleFlags;

  internal WritingMode WritingMode
  {
    get => this.m_writingMode;
    set
    {
      this.m_CommonstyleFlags = (byte) ((int) this.m_CommonstyleFlags & 254 | 1);
      this.m_writingMode = value;
    }
  }

  internal BeforeBreak BeforeBreak
  {
    get => this.m_beforeBreak;
    set
    {
      this.m_CommonstyleFlags = (byte) ((int) this.m_CommonstyleFlags & 253 | 2);
      this.m_beforeBreak = value;
    }
  }

  internal AfterBreak AfterBreak
  {
    get => this.m_afterBreak;
    set
    {
      this.m_CommonstyleFlags = (byte) ((int) this.m_CommonstyleFlags & 251 | 4);
      this.m_afterBreak = value;
    }
  }

  internal KeepTogether KeepWithNext
  {
    get => this.m_keepWithNext;
    set
    {
      this.m_CommonstyleFlags = (byte) ((int) this.m_CommonstyleFlags & 247 | 8);
      this.m_keepWithNext = value;
    }
  }

  internal string ShadowType
  {
    get => this.m_shadowType;
    set
    {
      this.m_CommonstyleFlags = (byte) ((int) this.m_CommonstyleFlags & 239 | 16 /*0x10*/);
      this.m_shadowType = value;
    }
  }

  internal int PageNumber
  {
    get => this.m_pageNumber;
    set
    {
      this.m_CommonstyleFlags = (byte) ((int) this.m_CommonstyleFlags & 223 | 32 /*0x20*/);
      this.m_pageNumber = value;
    }
  }

  internal string BackgroundColor
  {
    get => this.m_backgroundColor;
    set
    {
      this.m_CommonstyleFlags = (byte) ((int) this.m_CommonstyleFlags & 191 | 64 /*0x40*/);
      this.m_backgroundColor = value;
    }
  }
}
