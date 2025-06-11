// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.IconSetWrapper
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class IconSetWrapper : IIconSet, IOptimizedUpdate
{
  private ConditionalFormatWrapper m_format;
  private List<IConditionValue> m_arrConditions = new List<IConditionValue>();
  private IList<IConditionValue> m_readOnly;

  public IList<IConditionValue> IconCriteria
  {
    get
    {
      if (this.m_readOnly == null)
        this.m_readOnly = (IList<IConditionValue>) this.m_arrConditions.AsReadOnly();
      return this.m_readOnly;
    }
  }

  public ExcelIconSetType IconSet
  {
    get => this.GetIconSet().IconSet;
    set
    {
      this.BeginUpdate();
      this.GetIconSet().IconSet = value;
      this.EndUpdate();
    }
  }

  public bool PercentileValues
  {
    get => this.GetIconSet().PercentileValues;
    set
    {
      this.BeginUpdate();
      this.GetIconSet().PercentileValues = value;
      this.EndUpdate();
    }
  }

  public bool ReverseOrder
  {
    get => this.GetIconSet().ReverseOrder;
    set
    {
      this.BeginUpdate();
      this.GetIconSet().ReverseOrder = value;
      this.EndUpdate();
    }
  }

  public bool ShowIconOnly
  {
    get => this.GetIconSet().ShowIconOnly;
    set
    {
      this.BeginUpdate();
      this.GetIconSet().ShowIconOnly = value;
      this.EndUpdate();
    }
  }

  public void BeginUpdate() => this.UpdateCollection(this.GetIconSet().IconCriteria);

  public void EndUpdate()
  {
    this.m_format.EndUpdate();
    this.UpdateCollection(this.GetIconSet().IconCriteria);
  }

  public IconSetWrapper(ConditionalFormatWrapper format)
  {
    this.m_format = format;
    this.UpdateCollection(this.GetIconSet().IconCriteria);
  }

  private void UpdateCollection(IList<IConditionValue> arrSource)
  {
    int count1 = arrSource != null ? arrSource.Count : 0;
    int count2 = this.m_arrConditions.Count;
    if (count1 > count2)
      this.Add(count1 - count2, arrSource);
    else if (count2 > count1)
      this.Remove(count2 - count1);
    this.Update(Math.Min(count1, count2));
  }

  private void Add(int count, IList<IConditionValue> arrSource)
  {
    int count1 = this.m_arrConditions.Count;
    for (int index = 0; index < count; ++index)
      this.m_arrConditions.Add((IConditionValue) new IconConditionValueWrapper(arrSource[index], (IOptimizedUpdate) this));
  }

  private void Update(int count)
  {
    IList<IConditionValue> iconCriteria = this.GetIconSet().IconCriteria;
    for (int index = 0; index < count; ++index)
      (this.m_arrConditions[index] as ConditionValueWrapper).Wrapped = iconCriteria[index];
  }

  private void Remove(int count)
  {
    this.m_arrConditions.RemoveRange(this.m_arrConditions.Count - count, count);
  }

  private IconSetImpl GetIconSet() => this.m_format.GetCondition().IconSet as IconSetImpl;
}
