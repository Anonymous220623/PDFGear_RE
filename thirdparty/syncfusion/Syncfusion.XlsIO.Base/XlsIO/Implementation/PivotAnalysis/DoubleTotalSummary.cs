// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.DoubleTotalSummary
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

public class DoubleTotalSummary : SummaryBase, IAdjustable
{
  internal double? total = new double?();

  public override string ToString() => "DoubleTotal";

  public override void Combine(object other)
  {
    this.total = new double?(this.total ?? 0.0);
    switch (other)
    {
      case double num5:
        DoubleTotalSummary doubleTotalSummary1 = this;
        double? total1 = doubleTotalSummary1.total;
        double num1 = num5;
        doubleTotalSummary1.total = total1.HasValue ? new double?(total1.GetValueOrDefault() + num1) : new double?();
        break;
      case int _:
        DoubleTotalSummary doubleTotalSummary2 = this;
        double? total2 = doubleTotalSummary2.total;
        double num2 = (double) Convert.ChangeType(other, typeof (double));
        doubleTotalSummary2.total = total2.HasValue ? new double?(total2.GetValueOrDefault() + num2) : new double?();
        break;
      case DBNull _:
      case null:
        if (!this.ShowNullAsBlank)
          break;
        double? total3 = this.total;
        if ((total3.GetValueOrDefault() != 0.0 ? 0 : (total3.HasValue ? 1 : 0)) == 0)
          break;
        this.total = new double?();
        break;
      case string _:
        double result;
        if (!double.TryParse(other.ToString(), out result))
          break;
        DoubleTotalSummary doubleTotalSummary3 = this;
        double? total4 = doubleTotalSummary3.total;
        double num3 = result;
        doubleTotalSummary3.total = total4.HasValue ? new double?(total4.GetValueOrDefault() + num3) : new double?();
        break;
      case float _:
        DoubleTotalSummary doubleTotalSummary4 = this;
        double? total5 = doubleTotalSummary4.total;
        double num4 = Convert.ToDouble(other);
        doubleTotalSummary4.total = total5.HasValue ? new double?(total5.GetValueOrDefault() + num4) : new double?();
        break;
      case DateTime _:
        DoubleTotalSummary doubleTotalSummary5 = this;
        double? total6 = doubleTotalSummary5.total;
        double oaDate = (other as DateTime?).Value.ToOADate();
        doubleTotalSummary5.total = total6.HasValue ? new double?(total6.GetValueOrDefault() + oaDate) : new double?();
        break;
    }
  }

  public override void CombineSummary(SummaryBase other)
  {
    if (other != null && other.GetResult() == null)
    {
      DoubleTotalSummary doubleTotalSummary = this;
      double? total = doubleTotalSummary.total;
      doubleTotalSummary.total = total.HasValue ? new double?(total.GetValueOrDefault() + 0.0) : new double?();
    }
    else
      this.Combine((object) ((DoubleTotalSummary) other).total);
  }

  public override void Reset() => this.total = new double?();

  public override object GetResult() => (object) this.total;

  public override SummaryBase GetInstance() => (SummaryBase) new DoubleTotalSummary();

  public void AdjustForNewContribution(object newContribution)
  {
    this.total = new double?(this.total ?? 0.0);
    DoubleTotalSummary doubleTotalSummary = this;
    double? total = doubleTotalSummary.total;
    double num = (double) Convert.ChangeType(newContribution, typeof (double));
    doubleTotalSummary.total = total.HasValue ? new double?(total.GetValueOrDefault() + num) : new double?();
  }

  public void AdjustForOldContribution(object oldContribution)
  {
    this.total = new double?(this.total ?? 0.0);
    if (oldContribution == null || !(oldContribution.GetType().Name != "DBNull"))
      return;
    DoubleTotalSummary doubleTotalSummary = this;
    double? total = doubleTotalSummary.total;
    double num = (double) Convert.ChangeType(oldContribution, typeof (double));
    doubleTotalSummary.total = total.HasValue ? new double?(total.GetValueOrDefault() - num) : new double?();
  }
}
