// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.VarianceP
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

public class VarianceP : SummaryBase
{
  internal Type type;
  internal double? doubelSumX2 = new double?();
  internal double? doubleSumX = new double?();
  internal int? intSumX2 = new int?();
  internal int? intSumX = new int?();
  internal Decimal? decimalSumX2 = new Decimal?();
  internal Decimal? decimalSumX = new Decimal?();
  internal float? singleSumX2 = new float?();
  internal float? singleSumX = new float?();
  internal int? count = new int?();

  public override string ToString() => "VarP";

  public override void Combine(object other)
  {
    this.type = other.GetType();
    this.doubelSumX2 = new double?(this.doubelSumX2 ?? 0.0);
    this.doubleSumX = new double?(this.doubleSumX ?? 0.0);
    this.intSumX2 = new int?(this.intSumX2 ?? 0);
    this.intSumX = new int?(this.intSumX ?? 0);
    this.decimalSumX2 = new Decimal?(this.decimalSumX2 ?? 0M);
    this.decimalSumX = new Decimal?(this.decimalSumX ?? 0M);
    this.singleSumX2 = new float?((float) ((double) this.singleSumX2 ?? 0.0));
    this.singleSumX = new float?((float) ((double) this.singleSumX ?? 0.0));
    this.count = new int?(this.count ?? 0);
    num4 = 0.0;
    int num1 = 0;
    num3 = 0M;
    num2 = 0.0f;
    if (other is double num4)
    {
      VarianceP varianceP = this;
      int? count = varianceP.count;
      varianceP.count = count.HasValue ? new int?(count.GetValueOrDefault() + 1) : new int?();
    }
    else if (other is Decimal num3)
    {
      VarianceP varianceP = this;
      int? count = varianceP.count;
      varianceP.count = count.HasValue ? new int?(count.GetValueOrDefault() + 1) : new int?();
    }
    else if (this.type.ToString().Contains("Int"))
    {
      num1 = (int) Convert.ChangeType(other, typeof (int));
      VarianceP varianceP = this;
      int? count = varianceP.count;
      varianceP.count = count.HasValue ? new int?(count.GetValueOrDefault() + 1) : new int?();
    }
    else
    {
      switch (other)
      {
        case string _:
          double result;
          if (double.TryParse(other.ToString(), out result))
            num4 = result;
          VarianceP varianceP1 = this;
          int? count1 = varianceP1.count;
          varianceP1.count = count1.HasValue ? new int?(count1.GetValueOrDefault() + 1) : new int?();
          break;
        case DBNull _:
        case null:
          if (this.ShowNullAsBlank)
          {
            double? doubelSumX2 = this.doubelSumX2;
            if ((doubelSumX2.GetValueOrDefault() != 0.0 ? 0 : (doubelSumX2.HasValue ? 1 : 0)) != 0)
              this.doubelSumX2 = new double?();
            double? doubleSumX = this.doubleSumX;
            if ((doubleSumX.GetValueOrDefault() != 0.0 ? 0 : (doubleSumX.HasValue ? 1 : 0)) != 0)
              this.doubleSumX = new double?();
            Decimal? decimalSumX2 = this.decimalSumX2;
            if ((!(decimalSumX2.GetValueOrDefault() == 0M) ? 0 : (decimalSumX2.HasValue ? 1 : 0)) != 0)
              this.decimalSumX2 = new Decimal?();
            Decimal? decimalSumX = this.decimalSumX;
            if ((!(decimalSumX.GetValueOrDefault() == 0M) ? 0 : (decimalSumX.HasValue ? 1 : 0)) != 0)
              this.decimalSumX = new Decimal?();
            int? intSumX2 = this.intSumX2;
            if ((intSumX2.GetValueOrDefault() != 0 ? 0 : (intSumX2.HasValue ? 1 : 0)) != 0)
              this.intSumX2 = new int?();
            int? intSumX = this.intSumX;
            if ((intSumX.GetValueOrDefault() != 0 ? 0 : (intSumX.HasValue ? 1 : 0)) != 0)
              this.intSumX = new int?();
            this.count = new int?();
            break;
          }
          break;
        case float num2:
          VarianceP varianceP2 = this;
          int? count2 = varianceP2.count;
          varianceP2.count = count2.HasValue ? new int?(count2.GetValueOrDefault() + 1) : new int?();
          break;
      }
    }
    VarianceP varianceP3 = this;
    double? doubelSumX2_1 = varianceP3.doubelSumX2;
    double num5 = num4 * num4;
    varianceP3.doubelSumX2 = doubelSumX2_1.HasValue ? new double?(doubelSumX2_1.GetValueOrDefault() + num5) : new double?();
    VarianceP varianceP4 = this;
    double? doubleSumX1 = varianceP4.doubleSumX;
    double num6 = num4;
    varianceP4.doubleSumX = doubleSumX1.HasValue ? new double?(doubleSumX1.GetValueOrDefault() + num6) : new double?();
    VarianceP varianceP5 = this;
    int? intSumX2_1 = varianceP5.intSumX2;
    int num7 = num1 * num1;
    varianceP5.intSumX2 = intSumX2_1.HasValue ? new int?(intSumX2_1.GetValueOrDefault() + num7) : new int?();
    VarianceP varianceP6 = this;
    int? intSumX1 = varianceP6.intSumX;
    int num8 = num1;
    varianceP6.intSumX = intSumX1.HasValue ? new int?(intSumX1.GetValueOrDefault() + num8) : new int?();
    this.decimalSumX2 = new Decimal?(num3 * num3);
    this.decimalSumX = new Decimal?(num3);
    VarianceP varianceP7 = this;
    float? singleSumX2 = varianceP7.singleSumX2;
    float num9 = num2 * num2;
    varianceP7.singleSumX2 = singleSumX2.HasValue ? new float?(singleSumX2.GetValueOrDefault() + num9) : new float?();
    VarianceP varianceP8 = this;
    float? singleSumX = varianceP8.singleSumX;
    float num10 = num2;
    varianceP8.singleSumX = singleSumX.HasValue ? new float?(singleSumX.GetValueOrDefault() + num10) : new float?();
  }

  public override void CombineSummary(SummaryBase other)
  {
    this.doubelSumX2 = new double?(this.doubelSumX2 ?? 0.0);
    this.doubleSumX = new double?(this.doubleSumX ?? 0.0);
    this.intSumX2 = new int?(this.intSumX2 ?? 0);
    this.intSumX = new int?(this.intSumX ?? 0);
    this.decimalSumX2 = new Decimal?(this.decimalSumX2 ?? 0M);
    this.decimalSumX = new Decimal?(this.decimalSumX ?? 0M);
    this.singleSumX2 = new float?((float) ((double) this.singleSumX2 ?? 0.0));
    this.singleSumX = new float?((float) ((double) this.singleSumX ?? 0.0));
    this.count = new int?(this.count ?? 0);
    if (other.GetResult() == null)
    {
      VarianceP varianceP1 = this;
      double? doubelSumX2 = varianceP1.doubelSumX2;
      varianceP1.doubelSumX2 = doubelSumX2.HasValue ? new double?(doubelSumX2.GetValueOrDefault() + 0.0) : new double?();
      VarianceP varianceP2 = this;
      double? doubleSumX = varianceP2.doubleSumX;
      varianceP2.doubleSumX = doubleSumX.HasValue ? new double?(doubleSumX.GetValueOrDefault() + 0.0) : new double?();
      VarianceP varianceP3 = this;
      int? intSumX1 = varianceP3.intSumX;
      varianceP3.intSumX = intSumX1.HasValue ? new int?(intSumX1.GetValueOrDefault()) : new int?();
      VarianceP varianceP4 = this;
      int? intSumX2 = varianceP4.intSumX;
      varianceP4.intSumX = intSumX2.HasValue ? new int?(intSumX2.GetValueOrDefault()) : new int?();
      VarianceP varianceP5 = this;
      Decimal? decimalSumX2 = varianceP5.decimalSumX2;
      varianceP5.decimalSumX2 = decimalSumX2.HasValue ? new Decimal?(decimalSumX2.GetValueOrDefault()) : new Decimal?();
      VarianceP varianceP6 = this;
      Decimal? decimalSumX = varianceP6.decimalSumX;
      varianceP6.decimalSumX = decimalSumX.HasValue ? new Decimal?(decimalSumX.GetValueOrDefault()) : new Decimal?();
      VarianceP varianceP7 = this;
      float? singleSumX2 = varianceP7.singleSumX2;
      varianceP7.singleSumX2 = singleSumX2.HasValue ? new float?(singleSumX2.GetValueOrDefault() + 0.0f) : new float?();
      VarianceP varianceP8 = this;
      float? singleSumX = varianceP8.singleSumX;
      varianceP8.singleSumX = singleSumX.HasValue ? new float?(singleSumX.GetValueOrDefault() + 0.0f) : new float?();
      VarianceP varianceP9 = this;
      int? count = varianceP9.count;
      varianceP9.count = count.HasValue ? new int?(count.GetValueOrDefault()) : new int?();
    }
    else
    {
      VarianceP varianceP10 = (VarianceP) other;
      VarianceP varianceP11 = this;
      double? doubelSumX2_1 = varianceP11.doubelSumX2;
      double? doubelSumX2_2 = varianceP10.doubelSumX2;
      varianceP11.doubelSumX2 = doubelSumX2_1.HasValue & doubelSumX2_2.HasValue ? new double?(doubelSumX2_1.GetValueOrDefault() + doubelSumX2_2.GetValueOrDefault()) : new double?();
      VarianceP varianceP12 = this;
      double? doubleSumX1 = varianceP12.doubleSumX;
      double? doubleSumX2 = varianceP10.doubleSumX;
      varianceP12.doubleSumX = doubleSumX1.HasValue & doubleSumX2.HasValue ? new double?(doubleSumX1.GetValueOrDefault() + doubleSumX2.GetValueOrDefault()) : new double?();
      VarianceP varianceP13 = this;
      int? intSumX2_1 = varianceP13.intSumX2;
      int? intSumX2_2 = varianceP10.intSumX2;
      varianceP13.intSumX2 = intSumX2_1.HasValue & intSumX2_2.HasValue ? new int?(intSumX2_1.GetValueOrDefault() + intSumX2_2.GetValueOrDefault()) : new int?();
      VarianceP varianceP14 = this;
      int? intSumX3 = varianceP14.intSumX;
      int? intSumX4 = varianceP10.intSumX;
      varianceP14.intSumX = intSumX3.HasValue & intSumX4.HasValue ? new int?(intSumX3.GetValueOrDefault() + intSumX4.GetValueOrDefault()) : new int?();
      VarianceP varianceP15 = this;
      Decimal? decimalSumX2_1 = varianceP15.decimalSumX2;
      Decimal? decimalSumX2_2 = varianceP10.decimalSumX2;
      varianceP15.decimalSumX2 = decimalSumX2_1.HasValue & decimalSumX2_2.HasValue ? new Decimal?(decimalSumX2_1.GetValueOrDefault() + decimalSumX2_2.GetValueOrDefault()) : new Decimal?();
      VarianceP varianceP16 = this;
      Decimal? decimalSumX1 = varianceP16.decimalSumX;
      Decimal? decimalSumX2 = varianceP10.decimalSumX;
      varianceP16.decimalSumX = decimalSumX1.HasValue & decimalSumX2.HasValue ? new Decimal?(decimalSumX1.GetValueOrDefault() + decimalSumX2.GetValueOrDefault()) : new Decimal?();
      VarianceP varianceP17 = this;
      float? singleSumX2_1 = varianceP17.singleSumX2;
      float? singleSumX2_2 = varianceP10.singleSumX2;
      varianceP17.singleSumX2 = singleSumX2_1.HasValue & singleSumX2_2.HasValue ? new float?(singleSumX2_1.GetValueOrDefault() + singleSumX2_2.GetValueOrDefault()) : new float?();
      VarianceP varianceP18 = this;
      float? singleSumX1 = varianceP18.singleSumX;
      float? singleSumX2 = varianceP10.singleSumX;
      varianceP18.singleSumX = singleSumX1.HasValue & singleSumX2.HasValue ? new float?(singleSumX1.GetValueOrDefault() + singleSumX2.GetValueOrDefault()) : new float?();
      VarianceP varianceP19 = this;
      int? count1 = varianceP19.count;
      int? count2 = varianceP10.count;
      varianceP19.count = count1.HasValue & count2.HasValue ? new int?(count1.GetValueOrDefault() + count2.GetValueOrDefault()) : new int?();
    }
  }

  public override void Reset()
  {
    this.doubelSumX2 = new double?();
    this.doubleSumX = new double?();
    this.intSumX2 = new int?();
    this.intSumX = new int?();
    this.decimalSumX2 = new Decimal?();
    this.decimalSumX = new Decimal?();
    this.singleSumX2 = new float?();
    this.singleSumX = new float?();
    this.count = new int?();
  }

  public override object GetResult()
  {
    object result = (object) null;
    if (this.intSumX2.HasValue && this.intSumX.HasValue)
    {
      int? intSumX2 = this.intSumX2;
      if ((intSumX2.GetValueOrDefault() != 0 ? 1 : (!intSumX2.HasValue ? 1 : 0)) != 0)
      {
        int? intSumX = this.intSumX;
        if ((intSumX.GetValueOrDefault() != 0 ? 1 : (!intSumX.HasValue ? 1 : 0)) != 0)
          result = (object) Math.Round((Convert.ToDouble((object) this.intSumX2) - Convert.ToDouble((object) this.intSumX) * Convert.ToDouble((object) this.intSumX) / Convert.ToDouble((object) this.count)) / Convert.ToDouble((object) this.count), 0, MidpointRounding.AwayFromZero);
      }
    }
    if (this.decimalSumX2.HasValue && this.decimalSumX.HasValue)
    {
      Decimal? decimalSumX2 = this.decimalSumX2;
      if ((decimalSumX2.GetValueOrDefault() != 0M ? 1 : (!decimalSumX2.HasValue ? 1 : 0)) != 0)
      {
        Decimal? decimalSumX = this.decimalSumX;
        if ((decimalSumX.GetValueOrDefault() != 0M ? 1 : (!decimalSumX.HasValue ? 1 : 0)) != 0)
          result = (object) Math.Round((Convert.ToDouble((object) this.decimalSumX2) - Convert.ToDouble((object) this.decimalSumX) * Convert.ToDouble((object) this.decimalSumX) / Convert.ToDouble((object) this.count)) / Convert.ToDouble((object) this.count), 0, MidpointRounding.AwayFromZero);
      }
    }
    if (this.doubelSumX2.HasValue && this.doubleSumX.HasValue)
    {
      double? doubelSumX2 = this.doubelSumX2;
      if ((doubelSumX2.GetValueOrDefault() != 0.0 ? 1 : (!doubelSumX2.HasValue ? 1 : 0)) != 0)
      {
        double? doubleSumX = this.doubleSumX;
        if ((doubleSumX.GetValueOrDefault() != 0.0 ? 1 : (!doubleSumX.HasValue ? 1 : 0)) != 0)
          result = (object) Math.Round((Convert.ToDouble((object) this.doubelSumX2) - Convert.ToDouble((object) this.doubleSumX) * Convert.ToDouble((object) this.doubleSumX) / Convert.ToDouble((object) this.count)) / Convert.ToDouble((object) this.count), 0, MidpointRounding.AwayFromZero);
      }
    }
    if (this.singleSumX2.HasValue && this.singleSumX.HasValue)
    {
      float? singleSumX2 = this.singleSumX2;
      if (((double) singleSumX2.GetValueOrDefault() != 0.0 ? 1 : (!singleSumX2.HasValue ? 1 : 0)) != 0)
      {
        float? singleSumX = this.singleSumX;
        if (((double) singleSumX.GetValueOrDefault() != 0.0 ? 1 : (!singleSumX.HasValue ? 1 : 0)) != 0)
          result = (object) Math.Round((Convert.ToDouble((object) this.singleSumX2) - Convert.ToDouble((object) this.singleSumX) * Convert.ToDouble((object) this.singleSumX) / Convert.ToDouble((object) this.count)) / Convert.ToDouble((object) this.count), 0, MidpointRounding.AwayFromZero);
      }
    }
    int? count = this.count;
    if ((count.GetValueOrDefault() >= 2 ? 0 : (count.HasValue ? 1 : 0)) != 0)
      return (object) double.NaN;
    switch (result)
    {
      case double _:
        return result;
      case Decimal _:
        return (object) Convert.ToDecimal(result);
      case int _:
        return (object) Convert.ToInt32(result);
      default:
        return (object) null;
    }
  }

  public override SummaryBase GetInstance() => (SummaryBase) new VarianceP();
}
