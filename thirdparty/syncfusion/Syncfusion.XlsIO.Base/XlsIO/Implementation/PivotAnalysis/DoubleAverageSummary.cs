// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.DoubleAverageSummary
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

public class DoubleAverageSummary : SummaryBase, IAdjustable
{
  internal double? total = new double?();
  internal int? count = new int?();

  public override string ToString() => "DoubleAverage";

  public override void Combine(object other)
  {
    this.total = new double?(this.total ?? 0.0);
    this.count = new int?(this.count ?? 0);
    switch (other)
    {
      case double num5:
        DoubleAverageSummary doubleAverageSummary1 = this;
        double? total1 = doubleAverageSummary1.total;
        double num1 = num5;
        doubleAverageSummary1.total = total1.HasValue ? new double?(total1.GetValueOrDefault() + num1) : new double?();
        DoubleAverageSummary doubleAverageSummary2 = this;
        int? count1 = doubleAverageSummary2.count;
        doubleAverageSummary2.count = count1.HasValue ? new int?(count1.GetValueOrDefault() + 1) : new int?();
        break;
      case int _:
        DoubleAverageSummary doubleAverageSummary3 = this;
        double? total2 = doubleAverageSummary3.total;
        double num2 = (double) Convert.ChangeType(other, typeof (double));
        doubleAverageSummary3.total = total2.HasValue ? new double?(total2.GetValueOrDefault() + num2) : new double?();
        DoubleAverageSummary doubleAverageSummary4 = this;
        int? count2 = doubleAverageSummary4.count;
        doubleAverageSummary4.count = count2.HasValue ? new int?(count2.GetValueOrDefault() + 1) : new int?();
        break;
      case string _:
        double result;
        if (double.TryParse(other.ToString(), out result))
        {
          DoubleAverageSummary doubleAverageSummary5 = this;
          double? total3 = doubleAverageSummary5.total;
          double num3 = result;
          doubleAverageSummary5.total = total3.HasValue ? new double?(total3.GetValueOrDefault() + num3) : new double?();
        }
        DoubleAverageSummary doubleAverageSummary6 = this;
        int? count3 = doubleAverageSummary6.count;
        doubleAverageSummary6.count = count3.HasValue ? new int?(count3.GetValueOrDefault() + 1) : new int?();
        break;
      case DBNull _:
      case null:
        if (!this.ShowNullAsBlank)
          break;
        double? total4 = this.total;
        if ((total4.GetValueOrDefault() != 0.0 ? 0 : (total4.HasValue ? 1 : 0)) != 0)
          this.total = new double?();
        int? count4 = this.count;
        if (((double) count4.GetValueOrDefault() != 0.0 ? 0 : (count4.HasValue ? 1 : 0)) == 0)
          break;
        this.count = new int?();
        break;
      case float _:
        DoubleAverageSummary doubleAverageSummary7 = this;
        double? total5 = doubleAverageSummary7.total;
        double num4 = Convert.ToDouble(other);
        doubleAverageSummary7.total = total5.HasValue ? new double?(total5.GetValueOrDefault() + num4) : new double?();
        DoubleAverageSummary doubleAverageSummary8 = this;
        int? count5 = doubleAverageSummary8.count;
        doubleAverageSummary8.count = count5.HasValue ? new int?(count5.GetValueOrDefault() + 1) : new int?();
        break;
      case DateTime _:
        DoubleAverageSummary doubleAverageSummary9 = this;
        double? total6 = doubleAverageSummary9.total;
        double oaDate = (other as DateTime?).Value.ToOADate();
        doubleAverageSummary9.total = total6.HasValue ? new double?(total6.GetValueOrDefault() + oaDate) : new double?();
        break;
    }
  }

  public override void CombineSummary(SummaryBase other)
  {
    this.total = new double?(this.total ?? 0.0);
    this.count = new int?(this.count ?? 0);
    if (other != null && other.GetResult() == null)
    {
      DoubleAverageSummary doubleAverageSummary1 = this;
      double? total = doubleAverageSummary1.total;
      doubleAverageSummary1.total = total.HasValue ? new double?(total.GetValueOrDefault() + 0.0) : new double?();
      DoubleAverageSummary doubleAverageSummary2 = this;
      int? count = doubleAverageSummary2.count;
      doubleAverageSummary2.count = count.HasValue ? new int?(count.GetValueOrDefault()) : new int?();
    }
    else
    {
      DoubleAverageSummary doubleAverageSummary3 = (DoubleAverageSummary) other;
      DoubleAverageSummary doubleAverageSummary4 = this;
      double? total1 = doubleAverageSummary4.total;
      double? total2 = doubleAverageSummary3.total;
      doubleAverageSummary4.total = total1.HasValue & total2.HasValue ? new double?(total1.GetValueOrDefault() + total2.GetValueOrDefault()) : new double?();
      DoubleAverageSummary doubleAverageSummary5 = this;
      int? count1 = doubleAverageSummary5.count;
      int? count2 = doubleAverageSummary3.count;
      doubleAverageSummary5.count = count1.HasValue & count2.HasValue ? new int?(count1.GetValueOrDefault() + count2.GetValueOrDefault()) : new int?();
    }
  }

  public override void Reset()
  {
    this.total = new double?();
    this.count = new int?();
  }

  public override object GetResult()
  {
    int? count1 = this.count;
    if ((count1.GetValueOrDefault() != 0 ? 0 : (count1.HasValue ? 1 : 0)) != 0)
      return (object) double.NaN;
    double? total = this.total;
    int? count2 = this.count;
    return (object) (total.HasValue & count2.HasValue ? new double?(total.GetValueOrDefault() / (double) count2.GetValueOrDefault()) : new double?());
  }

  public override SummaryBase GetInstance() => (SummaryBase) new DoubleAverageSummary();

  public void AdjustForNewContribution(object newContribution)
  {
    this.total = new double?(this.total ?? 0.0);
    if (newContribution == null || !(newContribution.GetType().Name != "DBNull"))
      return;
    DoubleAverageSummary doubleAverageSummary = this;
    double? total = doubleAverageSummary.total;
    double num = (double) Convert.ChangeType(newContribution, typeof (double));
    doubleAverageSummary.total = total.HasValue ? new double?(total.GetValueOrDefault() + num) : new double?();
  }

  public void AdjustForOldContribution(object oldContribution)
  {
    this.total = new double?(this.total ?? 0.0);
    if (oldContribution == null)
      return;
    DoubleAverageSummary doubleAverageSummary = this;
    double? total = doubleAverageSummary.total;
    double num = (double) Convert.ChangeType(oldContribution, typeof (double));
    doubleAverageSummary.total = total.HasValue ? new double?(total.GetValueOrDefault() - num) : new double?();
  }
}
