// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.CondFormatCollectionWrapper
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections;

public class CondFormatCollectionWrapper : 
  CommonWrapper,
  IConditionalFormats,
  IEnumerable,
  IParentApplication,
  IOptimizedUpdate
{
  private ICombinedRange m_range;
  private ConditionalFormats m_condFormats;
  private List<IConditionalFormat> m_arrConditions = new List<IConditionalFormat>();

  private CondFormatCollectionWrapper()
  {
  }

  public CondFormatCollectionWrapper(ICombinedRange range)
  {
    this.m_range = range != null ? range : throw new ArgumentNullException(nameof (range));
  }

  public override void BeginUpdate()
  {
    base.BeginUpdate();
    if (this.BeginCallsCount != 1)
      return;
    this.CreateReadOnlyFormats();
    if (this.m_range is RangeImpl)
      (this.m_range as RangeImpl).ClearConditionalFormats(true);
    else if (this.m_range is RangesCollection)
      (this.m_range as RangesCollection).ClearConditionalFormats(true);
    this.CreateWriteableFormats();
  }

  public override void EndUpdate()
  {
    base.EndUpdate();
    if (this.BeginCallsCount != 0)
      return;
    this.m_condFormats = this.SheetFormats.Add(this.m_condFormats);
  }

  public int Count
  {
    get
    {
      this.CreateReadOnlyFormats();
      return this.m_condFormats.Count;
    }
  }

  public IConditionalFormat this[int index]
  {
    get
    {
      this.CreateReadOnlyFormats();
      ((ConditionalFormatWrapper) this.m_arrConditions[index]).Range = (IRange) this.m_range;
      return this.m_arrConditions[index];
    }
  }

  public IConditionalFormat AddCondition()
  {
    this.BeginUpdate();
    try
    {
      this.m_condFormats.AddCondition();
      IConditionalFormat conditionalFormat = (IConditionalFormat) new ConditionalFormatWrapper(this, this.Count - 1);
      ((ConditionalFormatWrapper) conditionalFormat).Range = (IRange) this.m_range;
      this.m_arrConditions.Add(conditionalFormat);
      return conditionalFormat;
    }
    finally
    {
      this.EndUpdate();
    }
  }

  public void Remove()
  {
    if (this.m_range.ConditionalFormats == null)
      throw new ArgumentNullException("Conditional Format");
    ((WorksheetImpl) this.m_range.Worksheet).ConditionalFormats.Remove(this.m_range.GetRectangles());
  }

  public void RemoveAt(int index)
  {
    if (this.m_range.ConditionalFormats == null)
      throw new ArgumentNullException("Conditional Format");
    ((WorksheetImpl) this.m_range.Worksheet).ConditionalFormats.RemoveAt(index);
  }

  public IEnumerator GetEnumerator() => (IEnumerator) this.m_arrConditions.GetEnumerator();

  public IApplication Application => this.m_range.Application;

  public object Parent => (object) this.m_range;

  private void CreateReadOnlyFormats()
  {
    if (this.m_condFormats != null)
      return;
    WorksheetConditionalFormats sheetFormats = this.SheetFormats;
    this.m_condFormats = sheetFormats.Find(this.m_range.GetRectangles());
    if (this.m_condFormats == null)
      this.m_condFormats = new ConditionalFormats(this.Application, (object) sheetFormats);
    this.CreateConditionWrappers();
  }

  private void CreateWriteableFormats()
  {
    if (this.m_condFormats == null)
      this.CreateReadOnlyFormats();
    this.m_condFormats = new ConditionalFormats(this.Application, (object) this.m_range, this.m_condFormats);
    this.m_condFormats.ClearCells();
    this.m_condFormats.AddRange((IRange) this.m_range);
    this.m_condFormats.EnclosedRange = new TAddr(this.m_range.Row - 1, this.m_range.Column - 1, this.m_range.LastRow - 1, this.m_range.LastColumn - 1);
  }

  private void CreateConditionWrappers()
  {
    int iIndex = 0;
    for (int count = this.m_condFormats.Count; iIndex < count; ++iIndex)
      this.m_arrConditions.Add((IConditionalFormat) new ConditionalFormatWrapper(this, iIndex));
  }

  public ConditionalFormatImpl GetCondition(int iCondition)
  {
    return this.m_condFormats[iCondition] as ConditionalFormatImpl;
  }

  private WorksheetConditionalFormats SheetFormats
  {
    get => ((WorksheetImpl) this.m_range.Worksheet).ConditionalFormats;
  }

  internal ConditionalFormats ConditionalFormats => this.m_condFormats;

  internal IRange Range => (IRange) this.m_range;
}
