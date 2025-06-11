// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODFConverter.Base.ODFImplementation.TabStops
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ODF.Base;

#nullable disable
namespace Syncfusion.DocIO.ODFConverter.Base.ODFImplementation;

internal class TabStops
{
  private TextAlign m_textAlignType;
  private double m_textPosition;
  private TabStopLeader m_tabLeader;

  internal double TextPosition
  {
    get => this.m_textPosition;
    set => this.m_textPosition = value;
  }

  internal TextAlign TextAlignType
  {
    get => this.m_textAlignType;
    set => this.m_textAlignType = value;
  }

  internal TabStopLeader TabStopLeader
  {
    get => this.m_tabLeader;
    set => this.m_tabLeader = value;
  }
}
