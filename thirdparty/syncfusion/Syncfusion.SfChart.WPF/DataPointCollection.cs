// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.DataPointCollection
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class DataPointCollection
{
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public List<double> XValues { get; set; }

  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public List<double> YValues { get; set; }

  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public List<double> ZValues { get; set; }

  internal bool IsDataAvailable
  {
    get => this.XValues.Count > 0 || this.YValues.Count > 0 || this.ZValues.Count > 0;
  }

  public DataPointCollection()
  {
    this.XValues = new List<double>();
    this.YValues = new List<double>();
    this.ZValues = new List<double>();
  }

  public void AddPoints(double x, double y, double z)
  {
    this.XValues.Add(x);
    this.YValues.Add(y);
    this.ZValues.Add(z);
  }
}
