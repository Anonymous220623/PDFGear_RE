// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.ContentControlListItem
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class ContentControlListItem
{
  private string m_displayText;
  private string m_value;

  public string DisplayText
  {
    get => this.m_displayText;
    set => this.m_displayText = value;
  }

  public string Value
  {
    get => this.m_value;
    set => this.m_value = value;
  }
}
