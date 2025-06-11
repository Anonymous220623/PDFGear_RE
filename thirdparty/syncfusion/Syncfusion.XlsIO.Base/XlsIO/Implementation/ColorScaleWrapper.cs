// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.ColorScaleWrapper
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class ColorScaleWrapper : IColorScale, IOptimizedUpdate
{
  private ConditionalFormatWrapper m_format;
  private List<IColorConditionValue> m_arrConditions = new List<IColorConditionValue>();
  private IList<IColorConditionValue> m_readOnly;

  public IList<IColorConditionValue> Criteria
  {
    get
    {
      if (this.m_readOnly == null)
        this.m_readOnly = (IList<IColorConditionValue>) this.m_arrConditions.AsReadOnly();
      return this.m_readOnly;
    }
  }

  public void SetConditionCount(int count)
  {
    this.BeginUpdate();
    this.GetWrapped().SetConditionCount(count);
    this.EndUpdate();
  }

  public void BeginUpdate()
  {
    this.m_format.BeginUpdate();
    this.UpdateCollection(this.GetWrapped().Criteria, (IOptimizedUpdate) this);
  }

  public void EndUpdate()
  {
    this.m_format.EndUpdate();
    this.UpdateCollection(this.GetWrapped().Criteria, (IOptimizedUpdate) this);
  }

  public ColorScaleWrapper(ConditionalFormatWrapper format)
  {
    this.m_format = format;
    this.UpdateCollection(this.GetWrapped().Criteria, (IOptimizedUpdate) this);
  }

  private void UpdateCollection(IList<IColorConditionValue> arrSource, IOptimizedUpdate parent)
  {
    int count1 = arrSource != null ? arrSource.Count : 0;
    int count2 = this.m_arrConditions.Count;
    if (count1 > count2)
      this.Add(count1 - count2, arrSource);
    else if (count2 > count1)
      this.Remove(count2 - count1);
    this.Update(Math.Min(count1, count2));
  }

  private void Add(int count, IList<IColorConditionValue> arrSource)
  {
    int count1 = this.m_arrConditions.Count;
    for (int index = 0; index < count; ++index)
      this.m_arrConditions.Add((ConditionValueWrapper) new ColorConditionValueWrapper((IConditionValue) arrSource[index], (IOptimizedUpdate) this) as IColorConditionValue);
  }

  private void Update(int count)
  {
    IList<IColorConditionValue> criteria = this.GetWrapped().Criteria;
    for (int index = 0; index < count; ++index)
      (this.m_arrConditions[index] as ConditionValueWrapper).Wrapped = (IConditionValue) criteria[index];
  }

  private void Remove(int count)
  {
    this.m_arrConditions.RemoveRange(this.m_arrConditions.Count - count, count);
  }

  private ColorScaleImpl GetWrapped() => this.m_format.GetCondition().ColorScale as ColorScaleImpl;
}
