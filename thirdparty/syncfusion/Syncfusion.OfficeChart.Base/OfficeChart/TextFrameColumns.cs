// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.TextFrameColumns
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

#nullable disable
namespace Syncfusion.OfficeChart;

internal struct TextFrameColumns
{
  private int number;
  private int spacingPt;

  public int Number
  {
    get => this.number;
    set => this.number = value;
  }

  public int SpacingPt
  {
    get => this.spacingPt;
    set => this.spacingPt = value;
  }
}
