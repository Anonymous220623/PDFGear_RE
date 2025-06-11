// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.IconSetImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class IconSetImpl : IIconSet, ICloneable
{
  private Stream m_iconSetStream;
  private IConditionValue[] m_arrCriteria;
  private ExcelIconSetType m_iconSet;
  private bool m_bPercentileValues;
  private bool m_bReverseOrder;
  private bool m_hasCustomIconSet;
  private bool m_bShowIconOnly;

  public IconSetImpl Clone()
  {
    IconSetImpl iconSetImpl = (IconSetImpl) this.MemberwiseClone();
    if (this.m_arrCriteria != null)
    {
      int length = this.m_arrCriteria.Length;
      iconSetImpl.m_arrCriteria = (IConditionValue[]) new ConditionValue[length];
      for (int index = 0; index < length; ++index)
        iconSetImpl.m_arrCriteria[index] = (IConditionValue) (this.m_arrCriteria[index] as ConditionValue).Clone();
    }
    return iconSetImpl;
  }

  public static bool operator ==(IconSetImpl first, IconSetImpl second)
  {
    if ((object) first == null && (object) second == null)
      return true;
    if ((object) first == null || (object) second == null)
      return false;
    IConditionValue[] arrCriteria1 = first.m_arrCriteria;
    IConditionValue[] arrCriteria2 = second.m_arrCriteria;
    bool flag = first.m_iconSet == second.m_iconSet && first.m_bPercentileValues == second.m_bPercentileValues && first.m_bReverseOrder == second.m_bReverseOrder && first.m_bShowIconOnly == second.m_bShowIconOnly && first.m_iconSetStream == second.m_iconSetStream && (arrCriteria1 != null ? arrCriteria1.Length : 0) == (arrCriteria2 != null ? arrCriteria2.Length : 0);
    if (flag && arrCriteria1 != null && arrCriteria2 != null && arrCriteria1.Length == arrCriteria2.Length)
    {
      for (int index = 0; index < arrCriteria1.Length; ++index)
        flag = arrCriteria1[index].Equals((object) arrCriteria2[index]);
    }
    return flag;
  }

  public static bool operator !=(IconSetImpl first, IconSetImpl second) => !(first == second);

  object ICloneable.Clone() => (object) this.Clone();

  public IList<IConditionValue> IconCriteria
  {
    get
    {
      if (this.m_arrCriteria == null)
        this.UpdateCriteria();
      return (IList<IConditionValue>) this.m_arrCriteria;
    }
  }

  public ExcelIconSetType IconSet
  {
    get => this.m_iconSet;
    set
    {
      if (this.m_iconSet != value)
      {
        this.m_iconSet = value;
        this.UpdateCriteria();
      }
      this.m_hasCustomIconSet = false;
    }
  }

  public bool PercentileValues
  {
    get => this.m_bPercentileValues;
    set => this.m_bPercentileValues = value;
  }

  public bool ReverseOrder
  {
    get => this.m_bReverseOrder;
    set => this.m_bReverseOrder = value;
  }

  public bool ShowIconOnly
  {
    get => this.m_bShowIconOnly;
    set => this.m_bShowIconOnly = value;
  }

  private void UpdateCriteria()
  {
    string str = this.m_iconSet.ToString();
    int length;
    if (str.StartsWith("Three"))
      length = 3;
    else if (str.StartsWith("Four"))
    {
      length = 4;
    }
    else
    {
      if (!str.StartsWith("Five"))
        throw new InvalidOperationException();
      length = 5;
    }
    this.m_arrCriteria = (IConditionValue[]) new ConditionValue[length];
    for (int index = 0; index < length; ++index)
    {
      int num = (int) Math.Round((double) (index * 100) / (double) length);
      IconConditionValue iconConditionValue = new IconConditionValue(this.m_iconSet, index, ConditionValueType.Percent, num.ToString());
      this.m_arrCriteria[index] = (IConditionValue) iconConditionValue;
    }
  }

  internal bool IsCustom
  {
    get
    {
      if (this.m_hasCustomIconSet)
        return this.m_hasCustomIconSet;
      for (int index = 0; index < this.IconCriteria.Count; ++index)
      {
        IconConditionValue arrCriterion = this.m_arrCriteria[index] as IconConditionValue;
        if (arrCriterion.IconSet != this.m_iconSet || arrCriterion.Index != index)
          return true;
      }
      return false;
    }
  }

  internal Stream IconSetStream
  {
    get => this.m_iconSetStream;
    set => this.m_iconSetStream = value;
  }

  internal void SetCustomIcon(bool value) => this.m_hasCustomIconSet = value;

  internal void ClearAll() => this.m_arrCriteria = (IConditionValue[]) null;
}
