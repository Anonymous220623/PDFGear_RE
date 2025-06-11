// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.AboveBelowAverageWrapper
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class AboveBelowAverageWrapper : IAboveBelowAverage, IOptimizedUpdate
{
  private AboveBelowAverageImpl m_wrapped;
  private ConditionalFormatWrapper m_format;

  public ExcelCFAverageType AverageType
  {
    get => this.m_wrapped.AverageType;
    set
    {
      this.BeginUpdate();
      this.m_wrapped.AverageType = value;
      this.EndUpdate();
    }
  }

  public int StdDevValue
  {
    get => this.m_wrapped.StdDevValue;
    set
    {
      this.BeginUpdate();
      this.m_wrapped.StdDevValue = value;
      this.EndUpdate();
    }
  }

  public void BeginUpdate()
  {
    this.m_format.BeginUpdate();
    this.m_wrapped = this.m_format.GetCondition().AboveBelowAverage as AboveBelowAverageImpl;
  }

  public void EndUpdate() => this.m_format.EndUpdate();

  public AboveBelowAverageWrapper(
    AboveBelowAverageImpl aboveAverage,
    ConditionalFormatWrapper format)
  {
    this.m_wrapped = aboveAverage;
    this.m_format = format;
  }
}
