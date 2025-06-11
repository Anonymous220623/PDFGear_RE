// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WarningInfo
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WarningInfo
{
  private string m_description;
  private WarningType m_warningType;

  public string Description => this.m_description;

  public WarningType WarningType => this.m_warningType;

  internal WarningInfo(string description, WarningType warningType)
  {
    this.m_description = description;
    this.m_warningType = warningType;
  }
}
