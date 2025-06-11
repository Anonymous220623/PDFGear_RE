// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.FontSchemeStruct
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal struct FontSchemeStruct
{
  private string m_name;
  private string m_typeface;
  private byte m_charSet;
  private string m_panose;
  private byte m_pitchFamily;

  internal string Name
  {
    get => this.m_name;
    set => this.m_name = value;
  }

  internal string Typeface
  {
    get => this.m_typeface;
    set => this.m_typeface = value;
  }

  internal byte Charset
  {
    get => this.m_charSet;
    set => this.m_charSet = value;
  }

  internal string Panose
  {
    get => this.m_panose;
    set => this.m_panose = value;
  }

  internal byte PitchFamily
  {
    get => this.m_pitchFamily;
    set => this.m_pitchFamily = value;
  }
}
