// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.XmlSerialization.Charts.ChartAxisScale
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Charts;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.XmlSerialization.Charts;

internal class ChartAxisScale
{
  public bool? LogScale = new bool?();
  public bool? Reversed;
  public double? MaximumValue = new double?();
  public double? MinimumValue = new double?();

  public void CopyTo(IScalable axis)
  {
    if (this.LogScale.HasValue)
      axis.IsLogScale = this.LogScale.Value;
    if (this.Reversed.HasValue)
      axis.ReversePlotOrder = this.Reversed.Value;
    if (this.MaximumValue.HasValue)
      axis.MaximumValue = this.MaximumValue.Value;
    if (!this.MinimumValue.HasValue)
      return;
    axis.MinimumValue = this.MinimumValue.Value;
  }
}
