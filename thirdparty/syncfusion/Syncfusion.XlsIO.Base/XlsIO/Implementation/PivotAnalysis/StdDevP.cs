// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.StdDevP
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

public class StdDevP : SummaryBase
{
  internal Type type;
  internal double? doubelSumX2 = new double?();
  internal double? doubleSumX = new double?();
  internal int? intSumX2 = new int?();
  internal int? intSumX = new int?();
  internal Decimal? decimalSumX2 = new Decimal?();
  internal Decimal? decimalSumX = new Decimal?();
  internal int? count = new int?();
  internal float? singleSumX2 = new float?();
  internal float? singleSumX = new float?();

  public override string ToString() => nameof (StdDevP);

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
      StdDevP stdDevP = this;
      int? count = stdDevP.count;
      stdDevP.count = count.HasValue ? new int?(count.GetValueOrDefault() + 1) : new int?();
    }
    else if (other is Decimal num3)
    {
      StdDevP stdDevP = this;
      int? count = stdDevP.count;
      stdDevP.count = count.HasValue ? new int?(count.GetValueOrDefault() + 1) : new int?();
    }
    else if (this.type.ToString().Contains("Int"))
    {
      num1 = (int) Convert.ChangeType(other, typeof (int));
      StdDevP stdDevP = this;
      int? count = stdDevP.count;
      stdDevP.count = count.HasValue ? new int?(count.GetValueOrDefault() + 1) : new int?();
    }
    else
    {
      switch (other)
      {
        case string _:
          double result;
          if (double.TryParse(other.ToString(), out result))
            num4 = result;
          StdDevP stdDevP1 = this;
          int? count1 = stdDevP1.count;
          stdDevP1.count = count1.HasValue ? new int?(count1.GetValueOrDefault() + 1) : new int?();
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
          StdDevP stdDevP2 = this;
          int? count2 = stdDevP2.count;
          stdDevP2.count = count2.HasValue ? new int?(count2.GetValueOrDefault() + 1) : new int?();
          break;
      }
    }
    StdDevP stdDevP3 = this;
    double? doubelSumX2_1 = stdDevP3.doubelSumX2;
    double num5 = num4 * num4;
    stdDevP3.doubelSumX2 = doubelSumX2_1.HasValue ? new double?(doubelSumX2_1.GetValueOrDefault() + num5) : new double?();
    StdDevP stdDevP4 = this;
    double? doubleSumX1 = stdDevP4.doubleSumX;
    double num6 = num4;
    stdDevP4.doubleSumX = doubleSumX1.HasValue ? new double?(doubleSumX1.GetValueOrDefault() + num6) : new double?();
    StdDevP stdDevP5 = this;
    int? intSumX2_1 = stdDevP5.intSumX2;
    int num7 = num1 * num1;
    stdDevP5.intSumX2 = intSumX2_1.HasValue ? new int?(intSumX2_1.GetValueOrDefault() + num7) : new int?();
    StdDevP stdDevP6 = this;
    int? intSumX1 = stdDevP6.intSumX;
    int num8 = num1;
    stdDevP6.intSumX = intSumX1.HasValue ? new int?(intSumX1.GetValueOrDefault() + num8) : new int?();
    this.decimalSumX2 = new Decimal?(num3 * num3);
    this.decimalSumX = new Decimal?(num3);
    StdDevP stdDevP7 = this;
    float? singleSumX2 = stdDevP7.singleSumX2;
    float num9 = num2 * num2;
    stdDevP7.singleSumX2 = singleSumX2.HasValue ? new float?(singleSumX2.GetValueOrDefault() + num9) : new float?();
    StdDevP stdDevP8 = this;
    float? singleSumX = stdDevP8.singleSumX;
    float num10 = num2;
    stdDevP8.singleSumX = singleSumX.HasValue ? new float?(singleSumX.GetValueOrDefault() + num10) : new float?();
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
      StdDevP stdDevP1 = this;
      double? doubelSumX2 = stdDevP1.doubelSumX2;
      stdDevP1.doubelSumX2 = doubelSumX2.HasValue ? new double?(doubelSumX2.GetValueOrDefault() + 0.0) : new double?();
      StdDevP stdDevP2 = this;
      double? doubleSumX = stdDevP2.doubleSumX;
      stdDevP2.doubleSumX = doubleSumX.HasValue ? new double?(doubleSumX.GetValueOrDefault() + 0.0) : new double?();
      StdDevP stdDevP3 = this;
      int? intSumX1 = stdDevP3.intSumX;
      stdDevP3.intSumX = intSumX1.HasValue ? new int?(intSumX1.GetValueOrDefault()) : new int?();
      StdDevP stdDevP4 = this;
      int? intSumX2 = stdDevP4.intSumX;
      stdDevP4.intSumX = intSumX2.HasValue ? new int?(intSumX2.GetValueOrDefault()) : new int?();
      StdDevP stdDevP5 = this;
      Decimal? decimalSumX2 = stdDevP5.decimalSumX2;
      stdDevP5.decimalSumX2 = decimalSumX2.HasValue ? new Decimal?(decimalSumX2.GetValueOrDefault()) : new Decimal?();
      StdDevP stdDevP6 = this;
      Decimal? decimalSumX = stdDevP6.decimalSumX;
      stdDevP6.decimalSumX = decimalSumX.HasValue ? new Decimal?(decimalSumX.GetValueOrDefault()) : new Decimal?();
      StdDevP stdDevP7 = this;
      int? count = stdDevP7.count;
      stdDevP7.count = count.HasValue ? new int?(count.GetValueOrDefault()) : new int?();
      StdDevP stdDevP8 = this;
      float? singleSumX2 = stdDevP8.singleSumX2;
      stdDevP8.singleSumX2 = singleSumX2.HasValue ? new float?(singleSumX2.GetValueOrDefault() + 0.0f) : new float?();
      StdDevP stdDevP9 = this;
      float? singleSumX = stdDevP9.singleSumX;
      stdDevP9.singleSumX = singleSumX.HasValue ? new float?(singleSumX.GetValueOrDefault() + 0.0f) : new float?();
    }
    else
    {
      StdDevP stdDevP10 = (StdDevP) other;
      StdDevP stdDevP11 = this;
      double? doubelSumX2_1 = stdDevP11.doubelSumX2;
      double? doubelSumX2_2 = stdDevP10.doubelSumX2;
      stdDevP11.doubelSumX2 = doubelSumX2_1.HasValue & doubelSumX2_2.HasValue ? new double?(doubelSumX2_1.GetValueOrDefault() + doubelSumX2_2.GetValueOrDefault()) : new double?();
      StdDevP stdDevP12 = this;
      double? doubleSumX1 = stdDevP12.doubleSumX;
      double? doubleSumX2 = stdDevP10.doubleSumX;
      stdDevP12.doubleSumX = doubleSumX1.HasValue & doubleSumX2.HasValue ? new double?(doubleSumX1.GetValueOrDefault() + doubleSumX2.GetValueOrDefault()) : new double?();
      StdDevP stdDevP13 = this;
      int? intSumX2_1 = stdDevP13.intSumX2;
      int? intSumX2_2 = stdDevP10.intSumX2;
      stdDevP13.intSumX2 = intSumX2_1.HasValue & intSumX2_2.HasValue ? new int?(intSumX2_1.GetValueOrDefault() + intSumX2_2.GetValueOrDefault()) : new int?();
      StdDevP stdDevP14 = this;
      int? intSumX3 = stdDevP14.intSumX;
      int? intSumX4 = stdDevP10.intSumX;
      stdDevP14.intSumX = intSumX3.HasValue & intSumX4.HasValue ? new int?(intSumX3.GetValueOrDefault() + intSumX4.GetValueOrDefault()) : new int?();
      StdDevP stdDevP15 = this;
      Decimal? decimalSumX2_1 = stdDevP15.decimalSumX2;
      Decimal? decimalSumX2_2 = stdDevP10.decimalSumX2;
      stdDevP15.decimalSumX2 = decimalSumX2_1.HasValue & decimalSumX2_2.HasValue ? new Decimal?(decimalSumX2_1.GetValueOrDefault() + decimalSumX2_2.GetValueOrDefault()) : new Decimal?();
      StdDevP stdDevP16 = this;
      Decimal? decimalSumX1 = stdDevP16.decimalSumX;
      Decimal? decimalSumX2 = stdDevP10.decimalSumX;
      stdDevP16.decimalSumX = decimalSumX1.HasValue & decimalSumX2.HasValue ? new Decimal?(decimalSumX1.GetValueOrDefault() + decimalSumX2.GetValueOrDefault()) : new Decimal?();
      StdDevP stdDevP17 = this;
      int? count1 = stdDevP17.count;
      int? count2 = stdDevP10.count;
      stdDevP17.count = count1.HasValue & count2.HasValue ? new int?(count1.GetValueOrDefault() + count2.GetValueOrDefault()) : new int?();
      StdDevP stdDevP18 = this;
      float? singleSumX2_1 = stdDevP18.singleSumX2;
      float? singleSumX2_2 = stdDevP10.singleSumX2;
      stdDevP18.singleSumX2 = singleSumX2_1.HasValue & singleSumX2_2.HasValue ? new float?(singleSumX2_1.GetValueOrDefault() + singleSumX2_2.GetValueOrDefault()) : new float?();
      StdDevP stdDevP19 = this;
      float? singleSumX1 = stdDevP19.singleSumX;
      float? singleSumX2 = stdDevP10.singleSumX;
      stdDevP19.singleSumX = singleSumX1.HasValue & singleSumX2.HasValue ? new float?(singleSumX1.GetValueOrDefault() + singleSumX2.GetValueOrDefault()) : new float?();
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
    this.count = new int?();
    this.singleSumX2 = new float?();
    this.singleSumX = new float?();
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
          result = (object) Math.Round(Math.Sqrt((Convert.ToDouble((object) this.intSumX2) - Convert.ToDouble((object) this.intSumX) * Convert.ToDouble((object) this.intSumX) / Convert.ToDouble((object) this.count)) / Convert.ToDouble((object) this.count)), 0, MidpointRounding.AwayFromZero);
      }
    }
    if (this.decimalSumX2.HasValue && this.decimalSumX.HasValue)
    {
      Decimal? decimalSumX2 = this.decimalSumX2;
      if ((decimalSumX2.GetValueOrDefault() != 0M ? 1 : (!decimalSumX2.HasValue ? 1 : 0)) != 0)
      {
        Decimal? decimalSumX = this.decimalSumX;
        if ((decimalSumX.GetValueOrDefault() != 0M ? 1 : (!decimalSumX.HasValue ? 1 : 0)) != 0)
          result = (object) Math.Round(Math.Sqrt((Convert.ToDouble((object) this.decimalSumX2) - Convert.ToDouble((object) this.decimalSumX) * Convert.ToDouble((object) this.decimalSumX) / Convert.ToDouble((object) this.count)) / Convert.ToDouble((object) this.count)), 0, MidpointRounding.AwayFromZero);
      }
    }
    if (this.doubelSumX2.HasValue && this.doubleSumX.HasValue)
    {
      double? doubelSumX2 = this.doubelSumX2;
      if ((doubelSumX2.GetValueOrDefault() != 0.0 ? 1 : (!doubelSumX2.HasValue ? 1 : 0)) != 0)
      {
        double? doubleSumX = this.doubleSumX;
        if ((doubleSumX.GetValueOrDefault() != 0.0 ? 1 : (!doubleSumX.HasValue ? 1 : 0)) != 0)
          result = (object) Math.Round(Math.Sqrt((Convert.ToDouble((object) this.doubelSumX2) - Convert.ToDouble((object) this.doubleSumX) * Convert.ToDouble((object) this.doubleSumX) / Convert.ToDouble((object) this.count)) / Convert.ToDouble((object) this.count)), 0, MidpointRounding.AwayFromZero);
      }
    }
    if (this.singleSumX2.HasValue && this.singleSumX.HasValue)
    {
      float? singleSumX2 = this.singleSumX2;
      if (((double) singleSumX2.GetValueOrDefault() != 0.0 ? 1 : (!singleSumX2.HasValue ? 1 : 0)) != 0)
      {
        float? singleSumX = this.singleSumX;
        if (((double) singleSumX.GetValueOrDefault() != 0.0 ? 1 : (!singleSumX.HasValue ? 1 : 0)) != 0)
          result = (object) Math.Round(Math.Sqrt((Convert.ToDouble((object) this.singleSumX2) - Convert.ToDouble((object) this.singleSumX) * Convert.ToDouble((object) this.singleSumX) / Convert.ToDouble((object) this.count)) / Convert.ToDouble((object) this.count)), 0, MidpointRounding.AwayFromZero);
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

  public override SummaryBase GetInstance() => (SummaryBase) new StdDevP();
}
