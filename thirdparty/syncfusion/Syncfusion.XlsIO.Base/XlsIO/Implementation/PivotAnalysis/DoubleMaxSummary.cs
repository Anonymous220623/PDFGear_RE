// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.DoubleMaxSummary
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

public class DoubleMaxSummary : SummaryBase
{
  internal double? max = new double?();

  public override string ToString() => "DoubleMaximum";

  public override void Combine(object other)
  {
    this.max = new double?(this.max ?? double.MinValue);
    pattern_0 = 0.0;
    switch (other)
    {
      case int _:
        pattern_0 = (double) Convert.ChangeType(other, typeof (double));
        break;
      case DBNull _:
      case null:
        if (this.ShowNullAsBlank)
        {
          double? max = this.max;
          if ((max.GetValueOrDefault() != double.MinValue ? 0 : (max.HasValue ? 1 : 0)) != 0)
          {
            this.max = new double?();
            break;
          }
          break;
        }
        break;
      case string _:
        double result;
        if (double.TryParse(other.ToString(), out result))
        {
          pattern_0 = result;
          break;
        }
        break;
      case float _:
        pattern_0 = Convert.ToDouble(other);
        break;
      case DateTime _:
        pattern_0 = (other as DateTime?).Value.ToOADate();
        break;
    }
    double? max1 = this.max;
    double num = pattern_0;
    if ((max1.GetValueOrDefault() >= num ? 0 : (max1.HasValue ? 1 : 0)) == 0)
      return;
    this.max = new double?(pattern_0);
  }

  public override void CombineSummary(SummaryBase other)
  {
    if (other.GetResult() != null)
      this.max = new double?(this.max ?? double.MinValue);
    DoubleMaxSummary doubleMaxSummary = (DoubleMaxSummary) other;
    double? max1 = this.max;
    double? max2 = doubleMaxSummary.max;
    if ((max1.GetValueOrDefault() >= max2.GetValueOrDefault() ? 0 : (max1.HasValue & max2.HasValue ? 1 : 0)) == 0)
      return;
    this.max = doubleMaxSummary.max;
  }

  public override void Reset() => this.max = new double?();

  public override object GetResult() => (object) this.max;

  public override SummaryBase GetInstance() => (SummaryBase) new DoubleMaxSummary();
}
