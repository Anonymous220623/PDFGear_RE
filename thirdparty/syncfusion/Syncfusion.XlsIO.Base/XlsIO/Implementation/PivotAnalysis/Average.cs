// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.Average
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

public class Average : SummaryBase
{
  internal int? intTotal = new int?();
  internal double? doubleTotal = new double?();
  internal Decimal? decimalTotal = new Decimal?();
  internal int? count = new int?();
  internal Type type;
  internal float? singleTotal = new float?();

  public override string ToString() => nameof (Average);

  public override void Combine(object other)
  {
    this.intTotal = new int?(this.intTotal ?? 0);
    this.doubleTotal = new double?(this.doubleTotal ?? 0.0);
    this.decimalTotal = new Decimal?(this.decimalTotal ?? 0M);
    this.singleTotal = new float?((float) ((double) this.singleTotal ?? 0.0));
    this.count = new int?(this.count ?? 0);
    this.type = other.GetType();
    if (other is double num4)
    {
      Average average1 = this;
      double? doubleTotal = average1.doubleTotal;
      average1.doubleTotal = doubleTotal.HasValue ? new double?(doubleTotal.GetValueOrDefault() + num4) : new double?();
      Average average2 = this;
      int? count = average2.count;
      average2.count = count.HasValue ? new int?(count.GetValueOrDefault() + 1) : new int?();
    }
    else if (other is Decimal num3)
    {
      this.decimalTotal = new Decimal?(num3);
      Average average = this;
      int? count = average.count;
      average.count = count.HasValue ? new int?(count.GetValueOrDefault() + 1) : new int?();
    }
    else if (this.type.ToString().Contains("Int"))
    {
      Average average3 = this;
      int? intTotal = average3.intTotal;
      int num = (int) Convert.ChangeType(other, typeof (int));
      average3.intTotal = intTotal.HasValue ? new int?(intTotal.GetValueOrDefault() + num) : new int?();
      Average average4 = this;
      int? count = average4.count;
      average4.count = count.HasValue ? new int?(count.GetValueOrDefault() + 1) : new int?();
    }
    else
    {
      switch (other)
      {
        case string _:
          double result;
          if (double.TryParse(other.ToString(), out result))
          {
            Average average = this;
            double? doubleTotal = average.doubleTotal;
            double num = result;
            average.doubleTotal = doubleTotal.HasValue ? new double?(doubleTotal.GetValueOrDefault() + num) : new double?();
          }
          Average average5 = this;
          int? count1 = average5.count;
          average5.count = count1.HasValue ? new int?(count1.GetValueOrDefault() + 1) : new int?();
          break;
        case DBNull _:
        case null:
          if (!this.ShowNullAsBlank)
            break;
          int? intTotal1 = this.intTotal;
          if ((intTotal1.GetValueOrDefault() != 0 ? 0 : (intTotal1.HasValue ? 1 : 0)) != 0)
            this.intTotal = new int?();
          double? doubleTotal1 = this.doubleTotal;
          if ((doubleTotal1.GetValueOrDefault() != 0.0 ? 0 : (doubleTotal1.HasValue ? 1 : 0)) != 0)
            this.doubleTotal = new double?();
          Decimal? decimalTotal = this.decimalTotal;
          if ((!(decimalTotal.GetValueOrDefault() == 0M) ? 0 : (decimalTotal.HasValue ? 1 : 0)) != 0)
            this.decimalTotal = new Decimal?();
          int? count2 = this.count;
          if (((double) count2.GetValueOrDefault() != 0.0 ? 0 : (count2.HasValue ? 1 : 0)) == 0)
            break;
          this.count = new int?();
          break;
        case float num2:
          Average average6 = this;
          float? singleTotal = average6.singleTotal;
          float num1 = num2;
          average6.singleTotal = singleTotal.HasValue ? new float?(singleTotal.GetValueOrDefault() + num1) : new float?();
          Average average7 = this;
          int? count3 = average7.count;
          average7.count = count3.HasValue ? new int?(count3.GetValueOrDefault() + 1) : new int?();
          break;
      }
    }
  }

  public override void CombineSummary(SummaryBase other)
  {
    this.intTotal = new int?(this.intTotal ?? 0);
    this.doubleTotal = new double?(this.doubleTotal ?? 0.0);
    this.decimalTotal = new Decimal?(this.decimalTotal ?? 0M);
    this.singleTotal = new float?((float) ((double) this.singleTotal ?? 0.0));
    this.count = new int?(this.count ?? 0);
    if (other.GetResult() == null)
    {
      Average average1 = this;
      int? intTotal = average1.intTotal;
      average1.intTotal = intTotal.HasValue ? new int?(intTotal.GetValueOrDefault()) : new int?();
      Average average2 = this;
      double? doubleTotal = average2.doubleTotal;
      average2.doubleTotal = doubleTotal.HasValue ? new double?(doubleTotal.GetValueOrDefault() + 0.0) : new double?();
      Average average3 = this;
      Decimal? decimalTotal = average3.decimalTotal;
      average3.decimalTotal = decimalTotal.HasValue ? new Decimal?(decimalTotal.GetValueOrDefault()) : new Decimal?();
      Average average4 = this;
      int? count = average4.count;
      average4.count = count.HasValue ? new int?(count.GetValueOrDefault()) : new int?();
      Average average5 = this;
      float? singleTotal = average5.singleTotal;
      average5.singleTotal = singleTotal.HasValue ? new float?(singleTotal.GetValueOrDefault() + 0.0f) : new float?();
    }
    else
    {
      Average average6 = (Average) other;
      Average average7 = this;
      int? intTotal1 = average7.intTotal;
      int? intTotal2 = average6.intTotal;
      average7.intTotal = intTotal1.HasValue & intTotal2.HasValue ? new int?(intTotal1.GetValueOrDefault() + intTotal2.GetValueOrDefault()) : new int?();
      Average average8 = this;
      double? doubleTotal1 = average8.doubleTotal;
      double? doubleTotal2 = average6.doubleTotal;
      average8.doubleTotal = doubleTotal1.HasValue & doubleTotal2.HasValue ? new double?(doubleTotal1.GetValueOrDefault() + doubleTotal2.GetValueOrDefault()) : new double?();
      Average average9 = this;
      Decimal? decimalTotal1 = average9.decimalTotal;
      Decimal? decimalTotal2 = average6.decimalTotal;
      average9.decimalTotal = decimalTotal1.HasValue & decimalTotal2.HasValue ? new Decimal?(decimalTotal1.GetValueOrDefault() + decimalTotal2.GetValueOrDefault()) : new Decimal?();
      Average average10 = this;
      int? count1 = average10.count;
      int? count2 = average6.count;
      average10.count = count1.HasValue & count2.HasValue ? new int?(count1.GetValueOrDefault() + count2.GetValueOrDefault()) : new int?();
      Average average11 = this;
      float? singleTotal1 = average11.singleTotal;
      float? singleTotal2 = average6.singleTotal;
      average11.singleTotal = singleTotal1.HasValue & singleTotal2.HasValue ? new float?(singleTotal1.GetValueOrDefault() + singleTotal2.GetValueOrDefault()) : new float?();
    }
  }

  public override void Reset()
  {
    this.intTotal = new int?();
    this.doubleTotal = new double?();
    this.decimalTotal = new Decimal?();
    this.count = new int?();
    this.singleTotal = new float?();
  }

  public override object GetResult()
  {
    object result1 = (object) null;
    if (this.intTotal.HasValue && this.count.HasValue)
    {
      int? intTotal = this.intTotal;
      if ((intTotal.GetValueOrDefault() != 0 ? 1 : (!intTotal.HasValue ? 1 : 0)) != 0)
        result1 = (object) Math.Round(Convert.ToDouble((object) this.intTotal) / Convert.ToDouble((object) this.count), 0, MidpointRounding.AwayFromZero);
    }
    if (this.doubleTotal.HasValue && this.count.HasValue)
    {
      double? doubleTotal = this.doubleTotal;
      if ((doubleTotal.GetValueOrDefault() != 0.0 ? 1 : (!doubleTotal.HasValue ? 1 : 0)) != 0)
      {
        result1 = (object) Math.Round(Convert.ToDouble((object) this.doubleTotal) / Convert.ToDouble((object) this.count), 0, MidpointRounding.AwayFromZero);
        goto label_12;
      }
    }
    if (this.decimalTotal.HasValue && this.count.HasValue)
    {
      Decimal? decimalTotal = this.decimalTotal;
      if ((decimalTotal.GetValueOrDefault() != 0M ? 1 : (!decimalTotal.HasValue ? 1 : 0)) != 0)
      {
        result1 = (object) Math.Round(Convert.ToDouble((object) this.decimalTotal) / Convert.ToDouble((object) this.count), 0, MidpointRounding.AwayFromZero);
        goto label_12;
      }
    }
    if (this.singleTotal.HasValue && this.count.HasValue)
    {
      float? singleTotal = this.singleTotal;
      if (((double) singleTotal.GetValueOrDefault() != 0.0 ? 1 : (!singleTotal.HasValue ? 1 : 0)) != 0)
        result1 = (object) Math.Round(Convert.ToDouble((object) this.singleTotal) / Convert.ToDouble((object) this.count), 0, MidpointRounding.AwayFromZero);
    }
label_12:
    int? count1 = this.count;
    if ((count1.GetValueOrDefault() != 0 ? 0 : (count1.HasValue ? 1 : 0)) != 0)
      return (object) double.NaN;
    int? result2;
    switch (result1)
    {
      case double _:
        return result1;
      case Decimal _:
        return (object) Convert.ToDecimal(result1);
      case null:
        int? intTotal1 = this.intTotal;
        int? count2 = this.count;
        result2 = intTotal1.HasValue & count2.HasValue ? new int?(intTotal1.GetValueOrDefault() / count2.GetValueOrDefault()) : new int?();
        break;
      default:
        result2 = new int?(Convert.ToInt32(result1));
        break;
    }
    return (object) result2;
  }

  public override SummaryBase GetInstance() => (SummaryBase) new Average();
}
