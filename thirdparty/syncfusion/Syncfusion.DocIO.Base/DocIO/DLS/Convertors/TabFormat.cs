// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.Convertors.TabFormat
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS.Convertors;

internal class TabFormat
{
  private float m_tabPosition = 36f;
  private TabJustification m_tabJustification;
  private TabLeader m_tabLeader;

  internal float TabPosition
  {
    get => this.m_tabPosition;
    set => this.m_tabPosition = value;
  }

  internal TabJustification TabJustification
  {
    get => this.m_tabJustification;
    set => this.m_tabJustification = value;
  }

  internal TabLeader TabLeader
  {
    get => this.m_tabLeader;
    set => this.m_tabLeader = value;
  }
}
