// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.DoubleMinSummary
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

public class DoubleMinSummary : SummaryBase
{
  internal double? min = new double?();

  public override string ToString() => "DoubleMinimum";

  public override void Combine(object other)
  {
    this.min = new double?(this.min ?? double.MaxValue);
    pattern_0 = double.MaxValue;
    switch (other)
    {
      case int _:
        pattern_0 = (double) Convert.ChangeType(other, typeof (double));
        break;
      case DBNull _:
      case null:
        if (this.ShowNullAsBlank)
        {
          double? min = this.min;
          if ((min.GetValueOrDefault() != double.MaxValue ? 0 : (min.HasValue ? 1 : 0)) != 0)
          {
            this.min = new double?();
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
    double? min1 = this.min;
    double num = pattern_0;
    if ((min1.GetValueOrDefault() <= num ? 0 : (min1.HasValue ? 1 : 0)) == 0)
      return;
    this.min = new double?(pattern_0);
  }

  public override void CombineSummary(SummaryBase other)
  {
    if (other.GetResult() != null)
      this.min = new double?(this.min ?? double.MaxValue);
    DoubleMinSummary doubleMinSummary = (DoubleMinSummary) other;
    double? min1 = this.min;
    double? min2 = doubleMinSummary.min;
    if ((min1.GetValueOrDefault() <= min2.GetValueOrDefault() ? 0 : (min1.HasValue & min2.HasValue ? 1 : 0)) == 0)
      return;
    this.min = doubleMinSummary.min;
  }

  public override void Reset() => this.min = new double?();

  public override object GetResult() => (object) this.min;

  public override SummaryBase GetInstance() => (SummaryBase) new DoubleMinSummary();
}
