// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.Max
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

public class Max : SummaryBase
{
  internal double? doubleMax = new double?();
  internal int? intMax = new int?();
  internal Decimal? decimalMax = new Decimal?();
  internal float? singleMax = new float?();
  internal Type type;

  public override string ToString() => nameof (Max);

  public override void Combine(object other)
  {
    this.type = other.GetType();
    this.doubleMax = new double?(this.doubleMax ?? double.MinValue);
    this.intMax = new int?(this.intMax ?? int.MinValue);
    this.decimalMax = new Decimal?(this.decimalMax ?? Decimal.MinValue);
    this.singleMax = new float?((float) ((double) this.singleMax ?? -3.4028234663852886E+38));
    num2 = 0.0;
    int num1 = 0;
    num3 = 0M;
    pattern_2 = 0.0f;
    if (!(other is double num2) && !(other is Decimal num3))
    {
      if (this.type.ToString().Contains("Int"))
      {
        num1 = (int) Convert.ChangeType(other, typeof (int));
      }
      else
      {
        switch (other)
        {
          case DBNull _:
          case null:
            if (this.ShowNullAsBlank)
            {
              double? doubleMax = this.doubleMax;
              if ((doubleMax.GetValueOrDefault() != double.MinValue ? 0 : (doubleMax.HasValue ? 1 : 0)) != 0)
              {
                this.doubleMax = new double?();
                break;
              }
              break;
            }
            break;
          case string _:
            double result;
            if (double.TryParse(other.ToString(), out result))
            {
              num2 = result;
              break;
            }
            break;
        }
      }
    }
    if (this.type != (Type) null && (this.type.ToString().Contains("Double") || this.type.ToString().Contains("String")))
    {
      double? doubleMax = this.doubleMax;
      double num4 = num2;
      if ((doubleMax.GetValueOrDefault() >= num4 ? 0 : (doubleMax.HasValue ? 1 : 0)) != 0)
      {
        this.doubleMax = new double?(num2);
        return;
      }
    }
    if (this.type != (Type) null && (this.type.ToString().Contains("Decimal") || this.type.ToString().Contains("Float")))
    {
      Decimal? decimalMax = this.decimalMax;
      Decimal num5 = num3;
      if ((!(decimalMax.GetValueOrDefault() < num5) ? 0 : (decimalMax.HasValue ? 1 : 0)) != 0)
      {
        this.decimalMax = new Decimal?(num3);
        return;
      }
    }
    if (this.type != (Type) null && this.type.ToString().Contains("Int"))
    {
      int? intMax = this.intMax;
      int num6 = num1;
      if ((intMax.GetValueOrDefault() >= num6 ? 0 : (intMax.HasValue ? 1 : 0)) != 0)
      {
        this.intMax = new int?(num1);
        return;
      }
    }
    if (!(this.type != (Type) null) || !this.type.ToString().Contains("Single"))
      return;
    float? singleMax = this.singleMax;
    float num7 = pattern_2;
    if (((double) singleMax.GetValueOrDefault() >= (double) num7 ? 0 : (singleMax.HasValue ? 1 : 0)) == 0)
      return;
    this.singleMax = new float?(pattern_2);
  }

  public override void CombineSummary(SummaryBase other)
  {
    this.doubleMax = new double?(this.doubleMax ?? double.MinValue);
    this.intMax = new int?(this.intMax ?? int.MinValue);
    this.decimalMax = new Decimal?(this.decimalMax ?? Decimal.MinValue);
    this.singleMax = new float?((float) ((double) this.singleMax ?? -3.4028234663852886E+38));
    Max max = (Max) other;
    double? doubleMax1 = this.doubleMax;
    double? doubleMax2 = max.doubleMax;
    if ((doubleMax1.GetValueOrDefault() >= doubleMax2.GetValueOrDefault() ? 0 : (doubleMax1.HasValue & doubleMax2.HasValue ? 1 : 0)) != 0)
    {
      this.doubleMax = max.doubleMax;
    }
    else
    {
      int? intMax1 = this.intMax;
      int? intMax2 = max.intMax;
      if ((intMax1.GetValueOrDefault() >= intMax2.GetValueOrDefault() ? 0 : (intMax1.HasValue & intMax2.HasValue ? 1 : 0)) != 0)
      {
        this.intMax = max.intMax;
      }
      else
      {
        Decimal? decimalMax1 = this.decimalMax;
        Decimal? decimalMax2 = max.decimalMax;
        if ((!(decimalMax1.GetValueOrDefault() < decimalMax2.GetValueOrDefault()) ? 0 : (decimalMax1.HasValue & decimalMax2.HasValue ? 1 : 0)) != 0)
        {
          this.decimalMax = max.decimalMax;
        }
        else
        {
          float? singleMax1 = this.singleMax;
          float? singleMax2 = max.singleMax;
          if (((double) singleMax1.GetValueOrDefault() >= (double) singleMax2.GetValueOrDefault() ? 0 : (singleMax1.HasValue & singleMax2.HasValue ? 1 : 0)) == 0)
            return;
          this.singleMax = max.singleMax;
        }
      }
    }
  }

  public override void Reset()
  {
    this.doubleMax = new double?();
    this.intMax = new int?();
    this.decimalMax = new Decimal?();
    this.singleMax = new float?();
  }

  public override object GetResult()
  {
    double? doubleMax = this.doubleMax;
    if ((doubleMax.GetValueOrDefault() != double.MinValue ? 1 : (!doubleMax.HasValue ? 1 : 0)) != 0)
      return (object) this.doubleMax;
    Decimal? decimalMax = this.decimalMax;
    if ((decimalMax.GetValueOrDefault() != Decimal.MinValue ? 1 : (!decimalMax.HasValue ? 1 : 0)) != 0)
      return (object) this.decimalMax;
    int? intMax = this.intMax;
    if ((intMax.GetValueOrDefault() != int.MinValue ? 1 : (!intMax.HasValue ? 1 : 0)) != 0)
      return (object) this.intMax;
    float? singleMax = this.singleMax;
    return ((double) singleMax.GetValueOrDefault() != -3.4028234663852886E+38 ? 1 : (!singleMax.HasValue ? 1 : 0)) != 0 ? (object) this.singleMax : (object) null;
  }

  public override SummaryBase GetInstance() => (SummaryBase) new Max();
}
