// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.Grouping.ConditionalFormatsGroup
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;
using System;
using System.Collections;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections.Grouping;

public class ConditionalFormatsGroup(IApplication application, object parent) : 
  CommonObject(application, parent),
  IConditionalFormats,
  IEnumerable,
  IParentApplication,
  IOptimizedUpdate
{
  private RangeGroup m_range;

  private void FindParents()
  {
    this.m_range = this.FindParent(typeof (RangeGroup)) as RangeGroup;
    if (this.m_range == null)
      throw new ArgumentOutOfRangeException("parent", "Can't find parent range group.");
  }

  private void SynchronizeCollection() => throw new NotImplementedException();

  public IConditionalFormats this[int index] => this.m_range[index].ConditionalFormats;

  public int Count => this.m_range.Count;

  int IConditionalFormats.Count => 0;

  IConditionalFormat IConditionalFormats.this[int index] => throw new NotImplementedException();

  public IConditionalFormat AddCondition()
  {
    this[0].AddCondition();
    int index = this[0].Count - 1;
    this.SynchronizeCollection();
    return ((IConditionalFormats) this)[index];
  }

  public void Remove() => throw new NotImplementedException();

  public void RemoveAt(int index) => throw new NotImplementedException();

  public IEnumerator GetEnumerator() => (IEnumerator) null;

  public void BeginUpdate() => throw new NotImplementedException();

  public void EndUpdate() => throw new NotImplementedException();
}
