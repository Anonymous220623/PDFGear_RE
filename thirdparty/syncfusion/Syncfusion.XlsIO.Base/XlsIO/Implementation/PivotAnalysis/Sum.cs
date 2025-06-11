// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.Sum
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

public class Sum : SummaryBase
{
  internal int? intTotal = new int?();
  internal double? doubleTotal = new double?();
  internal Decimal? decimalTotal = new Decimal?();
  internal float? singleTotal = new float?();
  internal Type type;

  public override string ToString() => nameof (Sum);

  public override void Combine(object other)
  {
    this.intTotal = new int?(this.intTotal ?? 0);
    this.doubleTotal = new double?(this.doubleTotal ?? 0.0);
    this.decimalTotal = new Decimal?(this.decimalTotal ?? 0M);
    this.singleTotal = new float?((float) ((double) this.singleTotal ?? 0.0));
    this.type = other.GetType();
    if (this.type.ToString().Contains("Int"))
    {
      Sum sum = this;
      int? intTotal = sum.intTotal;
      int num = (int) Convert.ChangeType(other, typeof (int));
      sum.intTotal = intTotal.HasValue ? new int?(intTotal.GetValueOrDefault() + num) : new int?();
    }
    else
    {
      switch (other)
      {
        case double _:
          Sum sum1 = this;
          double? doubleTotal = sum1.doubleTotal;
          double num1 = Convert.ToDouble(other);
          sum1.doubleTotal = doubleTotal.HasValue ? new double?(doubleTotal.GetValueOrDefault() + num1) : new double?();
          break;
        case Decimal _:
          Sum sum2 = this;
          Decimal? decimalTotal = sum2.decimalTotal;
          Decimal num2 = Convert.ToDecimal(other);
          sum2.decimalTotal = decimalTotal.HasValue ? new Decimal?(decimalTotal.GetValueOrDefault() + num2) : new Decimal?();
          break;
        case DBNull _:
        case null:
          if (!this.ShowNullAsBlank)
            break;
          int? intTotal1 = this.intTotal;
          if ((intTotal1.GetValueOrDefault() != 0 ? 0 : (intTotal1.HasValue ? 1 : 0)) == 0)
            break;
          this.intTotal = new int?();
          break;
        case float _:
          Sum sum3 = this;
          float? singleTotal = sum3.singleTotal;
          float single = Convert.ToSingle(other);
          sum3.singleTotal = singleTotal.HasValue ? new float?(singleTotal.GetValueOrDefault() + single) : new float?();
          break;
      }
    }
  }

  public override void CombineSummary(SummaryBase other)
  {
    Sum sum1 = (Sum) other;
    if (other.GetResult() == null)
    {
      Sum sum2 = this;
      int? intTotal = sum2.intTotal;
      sum2.intTotal = intTotal.HasValue ? new int?(intTotal.GetValueOrDefault()) : new int?();
      Sum sum3 = this;
      Decimal? decimalTotal = sum3.decimalTotal;
      sum3.decimalTotal = decimalTotal.HasValue ? new Decimal?(decimalTotal.GetValueOrDefault()) : new Decimal?();
      Sum sum4 = this;
      double? doubleTotal = sum4.doubleTotal;
      sum4.doubleTotal = doubleTotal.HasValue ? new double?(doubleTotal.GetValueOrDefault() + 0.0) : new double?();
      Sum sum5 = this;
      float? singleTotal = sum5.singleTotal;
      sum5.singleTotal = singleTotal.HasValue ? new float?(singleTotal.GetValueOrDefault() + 0.0f) : new float?();
    }
    else
    {
      if (sum1 == null)
        return;
      if (sum1.type.ToString().Contains("Double"))
        this.Combine((object) sum1.doubleTotal);
      if (sum1.type.ToString().Contains("Int"))
        this.Combine((object) sum1.intTotal);
      if (sum1.type.ToString().Contains("Decimal"))
        this.Combine((object) sum1.decimalTotal);
      if (!sum1.type.ToString().Contains("Single"))
        return;
      this.Combine((object) sum1.singleTotal);
    }
  }

  public override void Reset()
  {
    this.intTotal = new int?();
    this.doubleTotal = new double?();
    this.decimalTotal = new Decimal?();
    this.singleTotal = new float?();
  }

  public override object GetResult()
  {
    if (this.type != (Type) null && this.type.ToString().Contains("Int"))
      return (object) this.intTotal;
    if (this.type != (Type) null && this.type.ToString().Contains("Double"))
      return (object) this.doubleTotal;
    if (this.type != (Type) null && (this.type.ToString().Contains("Decimal") || this.type.ToString().Contains("Float")))
      return (object) this.decimalTotal;
    return this.type != (Type) null && this.type.ToString().Contains("Single") ? (object) this.singleTotal : (object) this.intTotal;
  }

  public override SummaryBase GetInstance() => (SummaryBase) new Sum();
}
