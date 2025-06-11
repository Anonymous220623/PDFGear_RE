// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotTables.PivotArea
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotTables;

public class PivotArea
{
  private PivotAxisTypes m_axis;
  private bool m_iIsCacheIndex;
  private bool m_bIsSubtotal;
  private bool m_bIsDataOnly;
  private int m_iFieldIndex;
  private int m_iFieldPosition;
  private bool m_bHasColumnGrand;
  private bool m_bHasRowGrand;
  private bool m_bIsLableOnly;
  private IRange m_range;
  private bool m_bIsOutline;
  private PivotAreaType m_areaType;
  private PivotAreaReferences m_references;
  private bool m_isAutoSort;
  private bool m_bIsCollapsedLevelsAreSubtotals;
  private string m_offset;
  private List<List<InternalReference>> m_internalReferences;
  private PivotTableImpl m_pivotTable;

  public PivotAxisTypes Axis
  {
    get => this.m_axis;
    set => this.m_axis = value;
  }

  public bool IsCacheIndex
  {
    get => this.m_iIsCacheIndex;
    set => this.m_iIsCacheIndex = value;
  }

  public bool IsSubtotal
  {
    get => this.m_bIsSubtotal;
    set => this.m_bIsSubtotal = value;
  }

  public bool IsDataOnly
  {
    get => this.m_bIsDataOnly;
    set => this.m_bIsDataOnly = value;
  }

  public int FieldIndex
  {
    get => this.m_iFieldIndex;
    set => this.m_iFieldIndex = value;
  }

  public int FieldPosition
  {
    get => this.m_iFieldPosition;
    set => this.m_iFieldPosition = value;
  }

  public bool HasColumnGrand
  {
    get => this.m_bHasColumnGrand;
    set => this.m_bHasColumnGrand = value;
  }

  public bool HasRowGrand
  {
    get => this.m_bHasRowGrand;
    set => this.m_bHasRowGrand = value;
  }

  public bool IsLableOnly
  {
    get => this.m_bIsLableOnly;
    set => this.m_bIsLableOnly = value;
  }

  public IRange Range
  {
    get => this.m_range;
    set
    {
      this.m_range = value;
      if (this.m_range == null)
        return;
      this.m_offset = this.m_range.Address;
    }
  }

  public bool IsOutline
  {
    get => this.m_bIsOutline;
    set => this.m_bIsOutline = value;
  }

  public PivotAreaType AreaType
  {
    get => this.m_areaType;
    set => this.m_areaType = value;
  }

  internal PivotAreaReferences References => this.m_references;

  internal int FirstIndexReference
  {
    get => this.m_references != null ? this.m_references[0].FirstIndex : -1;
  }

  internal bool IsAutoSort
  {
    get => this.m_isAutoSort;
    set => this.m_isAutoSort = value;
  }

  internal bool CollapsedLevelsAreSubtotals
  {
    get => this.m_bIsCollapsedLevelsAreSubtotals;
    set => this.m_bIsCollapsedLevelsAreSubtotals = value;
  }

  internal string Offset
  {
    get => this.m_offset;
    set => this.m_offset = value;
  }

  internal List<List<InternalReference>> InternalReferences
  {
    get => this.m_internalReferences;
    set => this.m_internalReferences = value;
  }

  internal PivotTableImpl PivotTable => this.m_pivotTable;

  public PivotArea(PivotCacheFieldImpl cacheField)
  {
    this.m_references = new PivotAreaReferences();
    this.Axis = PivotAxisTypes.None;
    this.AreaType = PivotAreaType.None;
  }

  internal PivotArea(PivotTableImpl pivotTable)
  {
    this.m_pivotTable = pivotTable;
    this.m_references = new PivotAreaReferences();
    this.Axis = PivotAxisTypes.None;
    this.AreaType = PivotAreaType.None;
  }

  internal object Clone(object parent)
  {
    PivotArea pivotArea = (PivotArea) this.MemberwiseClone();
    if (this.m_references != null)
    {
      pivotArea.m_references = new PivotAreaReferences();
      for (int index = 0; index < this.m_references.Count; ++index)
        pivotArea.m_references.Add(this.m_references[index].Clone() as PivotAreaReference);
    }
    object parent1 = (object) null;
    if (parent is PivotFieldImpl)
      parent1 = CommonObject.FindParent((object) (parent as PivotFieldImpl).PivotTable, typeof (WorksheetImpl));
    else if (parent is PivotFormat)
      parent1 = CommonObject.FindParent((object) (parent as PivotFormat).PivotTable, typeof (WorksheetImpl));
    if (this.m_range != null && this.m_range is ICombinedRange)
      pivotArea.m_range = (this.m_range as ICombinedRange).Clone(parent1, (Dictionary<string, string>) null, (parent1 as WorksheetImpl).Workbook as WorkbookImpl);
    return (object) pivotArea;
  }

  internal new bool Equals(object obj)
  {
    return obj is PivotArea pivotArea && this.m_axis == pivotArea.m_axis && this.m_iIsCacheIndex == pivotArea.m_iIsCacheIndex && this.m_bIsSubtotal == pivotArea.m_bIsSubtotal && this.m_iFieldIndex == pivotArea.m_iFieldIndex && this.m_bIsDataOnly == pivotArea.m_bIsDataOnly && this.m_iFieldPosition == pivotArea.m_iFieldPosition && this.m_bHasColumnGrand == pivotArea.m_bHasColumnGrand && this.m_bHasRowGrand == pivotArea.m_bHasRowGrand && this.m_bIsLableOnly == pivotArea.m_bIsLableOnly && this.m_offset == pivotArea.m_offset && this.m_bIsOutline == pivotArea.m_bIsOutline && this.m_areaType == pivotArea.m_areaType && this.m_isAutoSort == pivotArea.m_isAutoSort && this.m_bIsCollapsedLevelsAreSubtotals == pivotArea.m_bIsCollapsedLevelsAreSubtotals;
  }
}
