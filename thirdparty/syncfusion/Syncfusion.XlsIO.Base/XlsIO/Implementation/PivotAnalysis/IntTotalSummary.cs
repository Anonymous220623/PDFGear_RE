// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.IntTotalSummary
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

public class IntTotalSummary : SummaryBase, IAdjustable
{
  internal int? total = new int?();

  public override string ToString() => "IntTotal";

  public override void Combine(object other)
  {
    this.total = new int?(this.total ?? 0);
    switch (other)
    {
      case int num5:
        IntTotalSummary intTotalSummary1 = this;
        int? total1 = intTotalSummary1.total;
        int num1 = num5;
        intTotalSummary1.total = total1.HasValue ? new int?(total1.GetValueOrDefault() + num1) : new int?();
        break;
      case double _:
      case Decimal _:
        IntTotalSummary intTotalSummary2 = this;
        int? total2 = intTotalSummary2.total;
        int num2 = (int) Convert.ChangeType(other, typeof (int));
        intTotalSummary2.total = total2.HasValue ? new int?(total2.GetValueOrDefault() + num2) : new int?();
        break;
      case DBNull _:
      case null:
        if (!this.ShowNullAsBlank)
          break;
        int? total3 = this.total;
        if ((total3.GetValueOrDefault() != 0 ? 0 : (total3.HasValue ? 1 : 0)) == 0)
          break;
        this.total = new int?();
        break;
      case string _:
        Decimal result;
        if (!Decimal.TryParse(other.ToString(), out result))
          break;
        IntTotalSummary intTotalSummary3 = this;
        int? total4 = intTotalSummary3.total;
        int num3 = (int) Convert.ChangeType((object) result, typeof (int));
        intTotalSummary3.total = total4.HasValue ? new int?(total4.GetValueOrDefault() + num3) : new int?();
        break;
      case float _:
        IntTotalSummary intTotalSummary4 = this;
        int? total5 = intTotalSummary4.total;
        int num4 = (int) Convert.ChangeType(other, typeof (int));
        intTotalSummary4.total = total5.HasValue ? new int?(total5.GetValueOrDefault() + num4) : new int?();
        break;
    }
  }

  public override void CombineSummary(SummaryBase other)
  {
    if (other != null && other.GetResult() == null)
    {
      IntTotalSummary intTotalSummary = this;
      int? total = intTotalSummary.total;
      intTotalSummary.total = total.HasValue ? new int?(total.GetValueOrDefault()) : new int?();
    }
    else
      this.Combine((object) ((IntTotalSummary) other).total);
  }

  public override void Reset() => this.total = new int?();

  public override object GetResult() => (object) this.total;

  public override SummaryBase GetInstance() => (SummaryBase) new IntTotalSummary();

  public void AdjustForNewContribution(object newContribution)
  {
    this.total = new int?(this.total ?? 0);
    if (newContribution == null || !(newContribution.GetType().Name != "DBNull"))
      return;
    IntTotalSummary intTotalSummary = this;
    int? total = intTotalSummary.total;
    int num = (int) Convert.ChangeType(newContribution, typeof (int));
    intTotalSummary.total = total.HasValue ? new int?(total.GetValueOrDefault() + num) : new int?();
  }

  public void AdjustForOldContribution(object oldContribution)
  {
    this.total = new int?(this.total ?? 0);
    if (oldContribution == null || !(oldContribution.GetType().Name != "DBNull"))
      return;
    IntTotalSummary intTotalSummary = this;
    int? total = intTotalSummary.total;
    int num = (int) Convert.ChangeType(oldContribution, typeof (int));
    intTotalSummary.total = total.HasValue ? new int?(total.GetValueOrDefault() - num) : new int?();
  }
}
