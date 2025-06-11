// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.Min
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

public class Min : SummaryBase
{
  internal double? doubleMin = new double?();
  internal int? intMin = new int?();
  internal Decimal? decimalMin = new Decimal?();
  internal float? singleMin = new float?();
  internal Type type;

  public override string ToString() => nameof (Min);

  public override void Combine(object other)
  {
    this.type = other.GetType();
    this.doubleMin = new double?(this.doubleMin ?? double.MaxValue);
    this.intMin = new int?(this.intMin ?? int.MaxValue);
    this.decimalMin = new Decimal?(this.decimalMin ?? Decimal.MaxValue);
    this.singleMin = new float?((float) ((double) this.singleMin ?? 3.4028234663852886E+38));
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
              double? doubleMin = this.doubleMin;
              if ((doubleMin.GetValueOrDefault() != double.MaxValue ? 0 : (doubleMin.HasValue ? 1 : 0)) != 0)
              {
                this.doubleMin = new double?();
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
      double? doubleMin = this.doubleMin;
      double num4 = num2;
      if ((doubleMin.GetValueOrDefault() <= num4 ? 0 : (doubleMin.HasValue ? 1 : 0)) != 0)
      {
        this.doubleMin = new double?(num2);
        return;
      }
    }
    if (this.type != (Type) null && (this.type.ToString().Contains("Decimal") || this.type.ToString().Contains("Float")))
    {
      Decimal? decimalMin = this.decimalMin;
      Decimal num5 = num3;
      if ((!(decimalMin.GetValueOrDefault() > num5) ? 0 : (decimalMin.HasValue ? 1 : 0)) != 0)
      {
        this.decimalMin = new Decimal?(num3);
        return;
      }
    }
    if (this.type != (Type) null && this.type.ToString().Contains("Int"))
    {
      int? intMin = this.intMin;
      int num6 = num1;
      if ((intMin.GetValueOrDefault() <= num6 ? 0 : (intMin.HasValue ? 1 : 0)) != 0)
      {
        this.intMin = new int?(num1);
        return;
      }
    }
    if (!(this.type != (Type) null) || !this.type.ToString().Contains("Single"))
      return;
    float? singleMin = this.singleMin;
    float num7 = pattern_2;
    if (((double) singleMin.GetValueOrDefault() <= (double) num7 ? 0 : (singleMin.HasValue ? 1 : 0)) == 0)
      return;
    this.singleMin = new float?(pattern_2);
  }

  public override void CombineSummary(SummaryBase other)
  {
    this.doubleMin = new double?(this.doubleMin ?? double.MaxValue);
    this.intMin = new int?(this.intMin ?? int.MaxValue);
    this.decimalMin = new Decimal?(this.decimalMin ?? Decimal.MaxValue);
    this.singleMin = new float?((float) ((double) this.singleMin ?? 3.4028234663852886E+38));
    Min min = (Min) other;
    double? doubleMin1 = this.doubleMin;
    double? doubleMin2 = min.doubleMin;
    if ((doubleMin1.GetValueOrDefault() <= doubleMin2.GetValueOrDefault() ? 0 : (doubleMin1.HasValue & doubleMin2.HasValue ? 1 : 0)) != 0)
    {
      this.doubleMin = min.doubleMin;
    }
    else
    {
      int? intMin1 = this.intMin;
      int? intMin2 = min.intMin;
      if ((intMin1.GetValueOrDefault() <= intMin2.GetValueOrDefault() ? 0 : (intMin1.HasValue & intMin2.HasValue ? 1 : 0)) != 0)
      {
        this.intMin = min.intMin;
      }
      else
      {
        Decimal? decimalMin1 = this.decimalMin;
        Decimal? decimalMin2 = min.decimalMin;
        if ((!(decimalMin1.GetValueOrDefault() > decimalMin2.GetValueOrDefault()) ? 0 : (decimalMin1.HasValue & decimalMin2.HasValue ? 1 : 0)) != 0)
        {
          this.decimalMin = min.decimalMin;
        }
        else
        {
          float? singleMin1 = this.singleMin;
          float? singleMin2 = min.singleMin;
          if (((double) singleMin1.GetValueOrDefault() <= (double) singleMin2.GetValueOrDefault() ? 0 : (singleMin1.HasValue & singleMin2.HasValue ? 1 : 0)) == 0)
            return;
          this.singleMin = min.singleMin;
        }
      }
    }
  }

  public override void Reset()
  {
    this.doubleMin = new double?();
    this.intMin = new int?();
    this.decimalMin = new Decimal?();
    this.singleMin = new float?();
  }

  public override object GetResult()
  {
    double? doubleMin = this.doubleMin;
    if ((doubleMin.GetValueOrDefault() != double.MaxValue ? 1 : (!doubleMin.HasValue ? 1 : 0)) != 0)
      return (object) this.doubleMin;
    Decimal? decimalMin = this.decimalMin;
    if ((decimalMin.GetValueOrDefault() != Decimal.MaxValue ? 1 : (!decimalMin.HasValue ? 1 : 0)) != 0)
      return (object) this.decimalMin;
    int? intMin = this.intMin;
    if ((intMin.GetValueOrDefault() != int.MaxValue ? 1 : (!intMin.HasValue ? 1 : 0)) != 0)
      return (object) this.intMin;
    float? singleMin = this.singleMin;
    return ((double) singleMin.GetValueOrDefault() != 3.4028234663852886E+38 ? 1 : (!singleMin.HasValue ? 1 : 0)) != 0 ? (object) this.singleMin : (object) null;
  }

  public override SummaryBase GetInstance() => (SummaryBase) new Min();
}
