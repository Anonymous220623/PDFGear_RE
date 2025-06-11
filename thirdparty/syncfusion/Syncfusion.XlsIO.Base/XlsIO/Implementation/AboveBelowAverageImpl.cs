// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.AboveBelowAverageImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class AboveBelowAverageImpl : IAboveBelowAverage
{
  private ExcelCFAverageType m_averageType;
  private int m_stdDevValue = 1;

  public AboveBelowAverageImpl Clone() => (AboveBelowAverageImpl) this.MemberwiseClone();

  public static bool operator ==(AboveBelowAverageImpl first, AboveBelowAverageImpl second)
  {
    if ((object) first == null && (object) second == null)
      return true;
    return (object) first != null && (object) second != null && first.m_averageType == second.m_averageType && first.m_stdDevValue == second.m_stdDevValue;
  }

  public static bool operator !=(AboveBelowAverageImpl first, AboveBelowAverageImpl second)
  {
    return !(first == second);
  }

  public ExcelCFAverageType AverageType
  {
    get => this.m_averageType;
    set
    {
      if (this.m_averageType != value)
        this.m_stdDevValue = 1;
      this.m_averageType = value;
    }
  }

  public int StdDevValue
  {
    get => this.m_stdDevValue;
    set
    {
      this.m_stdDevValue = value >= 1 && value <= 3 ? value : throw new ArgumentException("NumStd must be between 1 and 3");
    }
  }
}
