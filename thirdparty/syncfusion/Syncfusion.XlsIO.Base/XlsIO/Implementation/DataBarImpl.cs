// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.DataBarImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class DataBarImpl : IDataBar, ICloneable
{
  private static readonly Color DefaultColor = Color.FromArgb((int) byte.MaxValue, 99, 142, 198);
  private static readonly DataBarAxisPosition DefaultAxisPosition = DataBarAxisPosition.automatic;
  private static readonly DataBarDirection DefaultDataBarDirection = DataBarDirection.context;
  private static readonly Color DefaultNegativeFillColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 0, 0);
  private static readonly Color DefaultAxisColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
  private IConditionValue m_minPoint = (IConditionValue) new ConditionValue(ConditionValueType.Automatic, "0");
  private IConditionValue m_maxPoint = (IConditionValue) new ConditionValue(ConditionValueType.Automatic, "0");
  private Color m_barColor = DataBarImpl.DefaultColor;
  private int m_iPercentMax = 100;
  private int m_iPercentMin;
  private bool m_bShowValue = true;
  private Color m_axisColor = DataBarImpl.DefaultAxisColor;
  private Color m_borderColor;
  private Color m_negativeBorderColor;
  private Color m_negativeFillColor = DataBarImpl.DefaultNegativeFillColor;
  private bool m_bHasBorder;
  private DataBarDirection m_direction = DataBarImpl.DefaultDataBarDirection;
  private bool m_bHasGradientFill = true;
  private bool m_bHasDiffNegativeBarColor = true;
  private bool m_bHasDiffNegativeBarBorderColor;
  private DataBarAxisPosition m_axisPosition = DataBarImpl.DefaultAxisPosition;
  private bool m_hasExtensionList;
  internal string ST_GUID;

  public IConditionValue MinPoint
  {
    get => this.m_minPoint;
    set => this.m_minPoint = value;
  }

  public IConditionValue MaxPoint
  {
    get => this.m_maxPoint;
    set => this.m_maxPoint = value;
  }

  public Color BarColor
  {
    get => this.m_barColor;
    set
    {
      this.m_barColor = value;
      this.HasExtensionList = true;
    }
  }

  public int PercentMax
  {
    get => this.m_iPercentMax;
    set => this.m_iPercentMax = value;
  }

  public int PercentMin
  {
    get => this.m_iPercentMin;
    set => this.m_iPercentMin = value;
  }

  public bool ShowValue
  {
    get => this.m_bShowValue;
    set => this.m_bShowValue = value;
  }

  public Color BarAxisColor
  {
    get => this.m_axisColor;
    set
    {
      this.m_axisColor = value;
      this.HasExtensionList = true;
    }
  }

  public Color BorderColor
  {
    get => this.m_borderColor;
    set
    {
      this.m_borderColor = value;
      this.m_bHasBorder = true;
    }
  }

  public Color NegativeBorderColor
  {
    get
    {
      if (!this.m_bHasBorder)
        return Color.FromArgb(0, 0, 0, 0);
      return this.HasDiffNegativeBarBorderColor ? this.m_negativeBorderColor : this.m_borderColor;
    }
    set
    {
      this.m_negativeBorderColor = value;
      this.HasExtensionList = true;
      this.HasDiffNegativeBarBorderColor = true;
    }
  }

  public Color NegativeFillColor
  {
    get => this.HasDiffNegativeBarColor ? this.m_negativeFillColor : this.m_barColor;
    set
    {
      this.m_negativeFillColor = value;
      this.HasExtensionList = true;
      this.HasDiffNegativeBarColor = true;
    }
  }

  public bool HasBorder
  {
    get => this.m_bHasBorder;
    internal set => this.m_bHasBorder = value;
  }

  public DataBarDirection DataBarDirection
  {
    get => this.m_direction;
    set
    {
      this.m_direction = value;
      this.HasExtensionList = true;
    }
  }

  public bool HasGradientFill
  {
    get => this.m_bHasGradientFill;
    set
    {
      this.m_bHasGradientFill = value;
      this.HasExtensionList = true;
    }
  }

  internal bool HasDiffNegativeBarColor
  {
    get => this.m_bHasDiffNegativeBarColor;
    set => this.m_bHasDiffNegativeBarColor = value;
  }

  internal bool HasDiffNegativeBarBorderColor
  {
    get => this.m_bHasDiffNegativeBarBorderColor;
    set => this.m_bHasDiffNegativeBarBorderColor = value;
  }

  public DataBarAxisPosition DataBarAxisPosition
  {
    get => this.m_axisPosition;
    set
    {
      this.m_axisPosition = value;
      this.HasExtensionList = true;
    }
  }

  internal bool HasExtensionList
  {
    get => this.m_hasExtensionList;
    set
    {
      this.m_hasExtensionList = value;
      if (this.ST_GUID != null)
        return;
      this.ST_GUID = $"{{{Guid.NewGuid().ToString()}}}";
    }
  }

  internal DataBarImpl Clone()
  {
    DataBarImpl dataBarImpl = (DataBarImpl) this.MemberwiseClone();
    dataBarImpl.MaxPoint = (IConditionValue) (this.MaxPoint as ConditionValue).Clone();
    dataBarImpl.MinPoint = (IConditionValue) (this.MinPoint as ConditionValue).Clone();
    return dataBarImpl;
  }

  object ICloneable.Clone() => (object) this.Clone();

  public override int GetHashCode()
  {
    return base.GetHashCode() ^ this.m_minPoint.GetHashCode() ^ this.m_maxPoint.GetHashCode() ^ this.m_barColor.GetHashCode() ^ this.m_iPercentMax.GetHashCode() ^ this.m_iPercentMin.GetHashCode() ^ this.m_bShowValue.GetHashCode();
  }

  public override bool Equals(object obj)
  {
    DataBarImpl dataBarImpl = obj as DataBarImpl;
    return !(dataBarImpl == (DataBarImpl) null) && this == dataBarImpl;
  }

  public static bool operator ==(DataBarImpl first, DataBarImpl second)
  {
    if ((object) first == null && (object) second == null)
      return true;
    return (object) first != null && (object) second != null && first.m_minPoint.Equals((object) second.m_minPoint) && first.m_maxPoint.Equals((object) second.m_maxPoint) && first.m_barColor.ToArgb() == second.m_barColor.ToArgb() && first.m_iPercentMax == second.m_iPercentMax && first.m_iPercentMin == second.m_iPercentMin && first.m_bShowValue == second.m_bShowValue && first.m_negativeFillColor.ToArgb() == second.m_negativeFillColor.ToArgb() && first.m_bHasBorder == second.m_bHasBorder && first.m_borderColor.ToArgb() == second.m_borderColor.ToArgb() && first.m_negativeBorderColor.ToArgb() == second.m_negativeBorderColor.ToArgb() && first.m_axisColor.ToArgb() == second.m_axisColor.ToArgb() && first.m_axisPosition == second.m_axisPosition && first.m_direction == second.m_direction && first.m_bHasGradientFill == second.m_bHasGradientFill;
  }

  public static bool operator !=(DataBarImpl first, DataBarImpl second) => !(first == second);
}
