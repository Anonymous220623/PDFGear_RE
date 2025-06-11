// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.Charts.ChartAxisScale
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Charts;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization.Charts;

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
      axis.IsReversed = this.Reversed.Value;
    if (this.MaximumValue.HasValue)
      axis.MaximumValue = this.MaximumValue.Value;
    if (!this.MinimumValue.HasValue)
      return;
    axis.MinimumValue = this.MinimumValue.Value;
  }
}
