// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.WarningInfo
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO;

public class WarningInfo
{
  private string m_description;
  private WarningType m_type;

  public string Description
  {
    get => this.m_description;
    internal set => this.m_description = value;
  }

  public WarningType Type
  {
    get => this.m_type;
    internal set => this.m_type = value;
  }

  internal WarningInfo()
  {
  }
}
