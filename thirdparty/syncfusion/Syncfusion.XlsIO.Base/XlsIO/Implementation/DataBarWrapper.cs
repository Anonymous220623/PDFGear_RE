// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.DataBarWrapper
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class DataBarWrapper : IDataBar, IOptimizedUpdate
{
  private DataBarImpl m_wrapped;
  private ConditionalFormatWrapper m_format;
  private ConditionValueWrapper m_minPoint;
  private ConditionValueWrapper m_maxPoint;

  public IConditionValue MinPoint => this.m_wrapped.MinPoint;

  public IConditionValue MaxPoint => this.m_wrapped.MaxPoint;

  public Color BarColor
  {
    get => this.m_wrapped.BarColor;
    set
    {
      this.BeginUpdate();
      this.m_wrapped.BarColor = value;
      this.EndUpdate();
    }
  }

  public int PercentMax
  {
    get => this.m_wrapped.PercentMax;
    set
    {
      this.BeginUpdate();
      this.m_wrapped.PercentMax = value;
      this.EndUpdate();
    }
  }

  public int PercentMin
  {
    get => this.m_wrapped.PercentMin;
    set
    {
      this.BeginUpdate();
      this.m_wrapped.PercentMin = value;
      this.EndUpdate();
    }
  }

  public bool ShowValue
  {
    get => this.m_wrapped.ShowValue;
    set
    {
      this.BeginUpdate();
      this.m_wrapped.ShowValue = value;
      this.EndUpdate();
    }
  }

  public Color BarAxisColor
  {
    get => this.m_wrapped.BarAxisColor;
    set
    {
      this.BeginUpdate();
      this.m_wrapped.BarAxisColor = value;
      this.EndUpdate();
    }
  }

  public Color BorderColor
  {
    get => this.m_wrapped.BorderColor;
    set
    {
      this.BeginUpdate();
      this.m_wrapped.BorderColor = value;
      this.EndUpdate();
    }
  }

  public bool HasBorder => this.m_wrapped.HasBorder;

  public bool HasGradientFill
  {
    get => this.m_wrapped.HasGradientFill;
    set => this.m_wrapped.HasGradientFill = value;
  }

  public DataBarDirection DataBarDirection
  {
    get => this.m_wrapped.DataBarDirection;
    set
    {
      this.BeginUpdate();
      this.m_wrapped.DataBarDirection = value;
      this.EndUpdate();
    }
  }

  public Color NegativeBorderColor
  {
    get => this.m_wrapped.NegativeBorderColor;
    set
    {
      this.BeginUpdate();
      this.m_wrapped.NegativeBorderColor = value;
      this.EndUpdate();
    }
  }

  public Color NegativeFillColor
  {
    get => this.m_wrapped.NegativeFillColor;
    set
    {
      this.BeginUpdate();
      this.m_wrapped.NegativeFillColor = value;
      this.EndUpdate();
    }
  }

  public DataBarAxisPosition DataBarAxisPosition
  {
    get => this.m_wrapped.DataBarAxisPosition;
    set
    {
      this.BeginUpdate();
      this.m_wrapped.DataBarAxisPosition = value;
      this.EndUpdate();
    }
  }

  public void BeginUpdate()
  {
    this.m_format.BeginUpdate();
    this.m_wrapped = this.m_format.GetCondition().DataBar as DataBarImpl;
  }

  public void EndUpdate() => this.m_format.EndUpdate();

  public DataBarWrapper(DataBarImpl dataBar, ConditionalFormatWrapper format)
  {
    this.m_wrapped = dataBar;
    this.m_format = format;
    this.m_minPoint = new ConditionValueWrapper(dataBar.MinPoint, (IOptimizedUpdate) format);
    this.m_maxPoint = new ConditionValueWrapper(dataBar.MaxPoint, (IOptimizedUpdate) format);
  }
}
