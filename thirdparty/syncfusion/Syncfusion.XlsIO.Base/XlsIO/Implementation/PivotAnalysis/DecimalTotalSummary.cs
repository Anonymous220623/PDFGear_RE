// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.DecimalTotalSummary
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

public class DecimalTotalSummary : SummaryBase, IAdjustable
{
  internal Decimal? total = new Decimal?();

  public override string ToString() => "DecimalTotal";

  public override void Combine(object other)
  {
    this.total = new Decimal?(this.total ?? 0M);
    switch (other)
    {
      case DBNull _:
      case null:
        if (this.ShowNullAsBlank)
        {
          Decimal? total = this.total;
          if ((!(total.GetValueOrDefault() == 0M) ? 0 : (total.HasValue ? 1 : 0)) != 0)
          {
            this.total = new Decimal?();
            goto label_11;
          }
          goto label_11;
        }
        goto label_11;
      case Decimal num4:
        DecimalTotalSummary decimalTotalSummary1 = this;
        Decimal? total1 = decimalTotalSummary1.total;
        Decimal num1 = num4;
        decimalTotalSummary1.total = total1.HasValue ? new Decimal?(total1.GetValueOrDefault() + num1) : new Decimal?();
        goto label_11;
      case double _:
        if (!other.Equals((object) double.NaN))
          break;
        goto default;
      default:
        switch (other)
        {
          case int _:
            break;
          case string _:
            Decimal result;
            if (Decimal.TryParse(other.ToString(), out result))
            {
              DecimalTotalSummary decimalTotalSummary2 = this;
              Decimal? total2 = decimalTotalSummary2.total;
              Decimal num2 = result;
              decimalTotalSummary2.total = total2.HasValue ? new Decimal?(total2.GetValueOrDefault() + num2) : new Decimal?();
              goto label_11;
            }
            goto label_11;
          case float _:
            DecimalTotalSummary decimalTotalSummary3 = this;
            Decimal? total3 = decimalTotalSummary3.total;
            Decimal num3 = Convert.ToDecimal(other);
            decimalTotalSummary3.total = total3.HasValue ? new Decimal?(total3.GetValueOrDefault() + num3) : new Decimal?();
            goto label_11;
          default:
            goto label_11;
        }
        break;
    }
    DecimalTotalSummary decimalTotalSummary4 = this;
    Decimal? total4 = decimalTotalSummary4.total;
    Decimal num5 = (Decimal) Convert.ChangeType(other, typeof (Decimal));
    decimalTotalSummary4.total = total4.HasValue ? new Decimal?(total4.GetValueOrDefault() + num5) : new Decimal?();
label_11:
    if (other == null || !other.Equals((object) double.NaN))
      return;
    this.total = new Decimal?();
  }

  public override void CombineSummary(SummaryBase other)
  {
    this.total = new Decimal?(this.total ?? 0M);
    if (other != null && other.GetResult() == null)
    {
      DecimalTotalSummary decimalTotalSummary = this;
      Decimal? total = decimalTotalSummary.total;
      decimalTotalSummary.total = total.HasValue ? new Decimal?(total.GetValueOrDefault()) : new Decimal?();
    }
    else
      this.Combine((object) ((DecimalTotalSummary) other).total);
  }

  public override void Reset() => this.total = new Decimal?();

  public override object GetResult() => (object) this.total;

  public override SummaryBase GetInstance() => (SummaryBase) new DecimalTotalSummary();

  public void AdjustForNewContribution(object newContribution)
  {
    this.total = new Decimal?(this.total ?? 0M);
    switch (newContribution)
    {
      case Decimal num3:
        DecimalTotalSummary decimalTotalSummary1 = this;
        Decimal? total1 = decimalTotalSummary1.total;
        Decimal num1 = num3;
        decimalTotalSummary1.total = total1.HasValue ? new Decimal?(total1.GetValueOrDefault() + num1) : new Decimal?();
        break;
      case double _:
      case int _:
        DecimalTotalSummary decimalTotalSummary2 = this;
        Decimal? total2 = decimalTotalSummary2.total;
        Decimal num2 = (Decimal) Convert.ChangeType(newContribution, typeof (Decimal));
        decimalTotalSummary2.total = total2.HasValue ? new Decimal?(total2.GetValueOrDefault() + num2) : new Decimal?();
        break;
    }
  }

  public void AdjustForOldContribution(object oldContribution)
  {
    this.total = new Decimal?(this.total ?? 0M);
    switch (oldContribution)
    {
      case Decimal num3:
        DecimalTotalSummary decimalTotalSummary1 = this;
        Decimal? total1 = decimalTotalSummary1.total;
        Decimal num1 = num3;
        decimalTotalSummary1.total = total1.HasValue ? new Decimal?(total1.GetValueOrDefault() - num1) : new Decimal?();
        break;
      case double _:
      case int _:
        DecimalTotalSummary decimalTotalSummary2 = this;
        Decimal? total2 = decimalTotalSummary2.total;
        Decimal num2 = (Decimal) Convert.ChangeType(oldContribution, typeof (Decimal));
        decimalTotalSummary2.total = total2.HasValue ? new Decimal?(total2.GetValueOrDefault() - num2) : new Decimal?();
        break;
    }
  }
}
