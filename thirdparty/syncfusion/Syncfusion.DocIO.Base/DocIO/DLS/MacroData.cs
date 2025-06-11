// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.MacroData
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class MacroData
{
  private string m_name;
  private string m_bEncrypt;
  private string m_cmg;

  internal string Name
  {
    get => this.m_name;
    set => this.m_name = value;
  }

  internal string Encrypt
  {
    get => this.m_bEncrypt;
    set => this.m_bEncrypt = value;
  }

  internal string Cmg
  {
    get => this.m_cmg;
    set => this.m_cmg = value;
  }

  internal MacroData()
  {
  }
}
