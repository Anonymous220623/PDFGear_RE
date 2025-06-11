// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.DoubleVarianceSummary
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

public class DoubleVarianceSummary : SummaryBase
{
  internal double? sumX2 = new double?();
  internal double? sumX = new double?();
  internal int? n = new int?();

  public override string ToString() => "DoubleVariance";

  public override void Combine(object other)
  {
    this.sumX2 = new double?(this.sumX2 ?? 0.0);
    this.sumX = new double?(this.sumX ?? 0.0);
    this.n = new int?(this.n ?? 0);
    num1 = 0.0;
    switch (other)
    {
      case double num1:
        DoubleVarianceSummary doubleVarianceSummary1 = this;
        int? n1 = doubleVarianceSummary1.n;
        doubleVarianceSummary1.n = n1.HasValue ? new int?(n1.GetValueOrDefault() + 1) : new int?();
        break;
      case int _:
        num1 = (double) Convert.ChangeType(other, typeof (double));
        DoubleVarianceSummary doubleVarianceSummary2 = this;
        int? n2 = doubleVarianceSummary2.n;
        doubleVarianceSummary2.n = n2.HasValue ? new int?(n2.GetValueOrDefault() + 1) : new int?();
        break;
      case string _:
        double result;
        if (double.TryParse(other.ToString(), out result))
          num1 = result;
        DoubleVarianceSummary doubleVarianceSummary3 = this;
        int? n3 = doubleVarianceSummary3.n;
        doubleVarianceSummary3.n = n3.HasValue ? new int?(n3.GetValueOrDefault() + 1) : new int?();
        break;
      case DBNull _:
      case null:
        if (this.ShowNullAsBlank)
        {
          double? sumX2 = this.sumX2;
          if ((sumX2.GetValueOrDefault() != 0.0 ? 0 : (sumX2.HasValue ? 1 : 0)) != 0)
            this.sumX2 = new double?();
          double? sumX = this.sumX;
          if ((sumX.GetValueOrDefault() != 0.0 ? 0 : (sumX.HasValue ? 1 : 0)) != 0)
            this.sumX = new double?();
          this.n = new int?();
          break;
        }
        break;
      case float _:
        num1 = Convert.ToDouble(other);
        DoubleVarianceSummary doubleVarianceSummary4 = this;
        int? n4 = doubleVarianceSummary4.n;
        doubleVarianceSummary4.n = n4.HasValue ? new int?(n4.GetValueOrDefault() + 1) : new int?();
        break;
      case DateTime _:
        num1 = (other as DateTime?).Value.ToOADate();
        break;
    }
    DoubleVarianceSummary doubleVarianceSummary5 = this;
    double? sumX2_1 = doubleVarianceSummary5.sumX2;
    double num2 = num1 * num1;
    doubleVarianceSummary5.sumX2 = sumX2_1.HasValue ? new double?(sumX2_1.GetValueOrDefault() + num2) : new double?();
    DoubleVarianceSummary doubleVarianceSummary6 = this;
    double? sumX1 = doubleVarianceSummary6.sumX;
    double num3 = num1;
    doubleVarianceSummary6.sumX = sumX1.HasValue ? new double?(sumX1.GetValueOrDefault() + num3) : new double?();
  }

  public override void CombineSummary(SummaryBase other)
  {
    this.sumX2 = new double?(this.sumX2 ?? 0.0);
    this.sumX = new double?(this.sumX ?? 0.0);
    this.n = new int?(this.n ?? 0);
    if (other != null && other.GetResult() == null)
    {
      DoubleVarianceSummary doubleVarianceSummary1 = this;
      double? sumX2 = doubleVarianceSummary1.sumX2;
      doubleVarianceSummary1.sumX2 = sumX2.HasValue ? new double?(sumX2.GetValueOrDefault() + 0.0) : new double?();
      DoubleVarianceSummary doubleVarianceSummary2 = this;
      double? sumX = doubleVarianceSummary2.sumX;
      doubleVarianceSummary2.sumX = sumX.HasValue ? new double?(sumX.GetValueOrDefault() + 0.0) : new double?();
      DoubleVarianceSummary doubleVarianceSummary3 = this;
      int? n = doubleVarianceSummary3.n;
      doubleVarianceSummary3.n = n.HasValue ? new int?(n.GetValueOrDefault()) : new int?();
    }
    else
    {
      DoubleVarianceSummary doubleVarianceSummary4 = (DoubleVarianceSummary) other;
      DoubleVarianceSummary doubleVarianceSummary5 = this;
      double? sumX2_1 = doubleVarianceSummary5.sumX2;
      double? sumX2_2 = doubleVarianceSummary4.sumX2;
      doubleVarianceSummary5.sumX2 = sumX2_1.HasValue & sumX2_2.HasValue ? new double?(sumX2_1.GetValueOrDefault() + sumX2_2.GetValueOrDefault()) : new double?();
      DoubleVarianceSummary doubleVarianceSummary6 = this;
      double? sumX1 = doubleVarianceSummary6.sumX;
      double? sumX2 = doubleVarianceSummary4.sumX;
      doubleVarianceSummary6.sumX = sumX1.HasValue & sumX2.HasValue ? new double?(sumX1.GetValueOrDefault() + sumX2.GetValueOrDefault()) : new double?();
      DoubleVarianceSummary doubleVarianceSummary7 = this;
      int? n1 = doubleVarianceSummary7.n;
      int? n2 = doubleVarianceSummary4.n;
      doubleVarianceSummary7.n = n1.HasValue & n2.HasValue ? new int?(n1.GetValueOrDefault() + n2.GetValueOrDefault()) : new int?();
    }
  }

  public override void Reset()
  {
    this.sumX2 = new double?();
    this.sumX = new double?();
    this.n = new int?();
  }

  public override object GetResult()
  {
    int? n1 = this.n;
    if ((n1.GetValueOrDefault() >= 2 ? 0 : (n1.HasValue ? 1 : 0)) != 0)
      return (object) double.NaN;
    double? sumX2 = this.sumX2;
    double? sumX1 = this.sumX;
    double? sumX3 = this.sumX;
    double? nullable1 = sumX1.HasValue & sumX3.HasValue ? new double?(sumX1.GetValueOrDefault() * sumX3.GetValueOrDefault()) : new double?();
    int? n2 = this.n;
    double? nullable2 = nullable1.HasValue & n2.HasValue ? new double?(nullable1.GetValueOrDefault() / (double) n2.GetValueOrDefault()) : new double?();
    double? nullable3 = sumX2.HasValue & nullable2.HasValue ? new double?(sumX2.GetValueOrDefault() - nullable2.GetValueOrDefault()) : new double?();
    int? n3 = this.n;
    int? nullable4 = n3.HasValue ? new int?(n3.GetValueOrDefault() - 1) : new int?();
    return (object) (nullable3.HasValue & nullable4.HasValue ? new double?(nullable3.GetValueOrDefault() / (double) nullable4.GetValueOrDefault()) : new double?());
  }

  public override SummaryBase GetInstance() => (SummaryBase) new DoubleVarianceSummary();
}
