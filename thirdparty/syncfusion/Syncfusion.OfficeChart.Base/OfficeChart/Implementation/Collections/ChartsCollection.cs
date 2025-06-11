// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Collections.ChartsCollection
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Charts;
using Syncfusion.OfficeChart.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Collections;

internal class ChartsCollection : CollectionBaseEx<IOfficeChart>, ICharts, IEnumerable
{
  public const string DEF_CHART_NAME_START = "Chart";
  private Dictionary<string, IOfficeChart> m_hashNames = new Dictionary<string, IOfficeChart>((IEqualityComparer<string>) System.StringComparer.CurrentCultureIgnoreCase);
  private WorkbookImpl m_book;

  public ChartsCollection(IApplication application, object parent)
    : base(application, parent)
  {
    this.SetParents();
  }

  public IOfficeChart this[string name] => this.m_hashNames[name];

  public IOfficeChart Add(IOfficeChart chartToAdd)
  {
    this.AddInternal(chartToAdd);
    this.m_book.Objects.Add((ISerializableNamedObject) chartToAdd);
    return chartToAdd;
  }

  public IOfficeChart Add()
  {
    ChartImpl chartToAdd = new ChartImpl();
    chartToAdd.Name = CollectionBaseEx<IOfficeChart>.GenerateDefaultName((ICollection<IOfficeChart>) this.List, "Chart");
    return this.Add((IOfficeChart) chartToAdd);
  }

  public IOfficeChart Add(string name)
  {
    ChartImpl chartToAdd = new ChartImpl();
    chartToAdd.Name = name;
    return this.Add((IOfficeChart) chartToAdd);
  }

  public IOfficeChart Remove(string name)
  {
    if (!this.m_hashNames.ContainsKey(name))
      return (IOfficeChart) null;
    IOfficeChart hashName = this.m_hashNames[name];
    this.Remove(hashName);
    return hashName;
  }

  private void SetParents()
  {
    this.m_book = (WorkbookImpl) (this.FindParent(typeof (WorkbookImpl)) ?? throw new ArgumentNullException("Can't find parent workbook."));
  }

  public void Move(int iOldIndex, int iNewIndex)
  {
    if (iOldIndex == iNewIndex)
      return;
    int count = this.InnerList.Count;
    if (iOldIndex < 0 || iOldIndex >= count)
      throw new ArgumentOutOfRangeException(nameof (iOldIndex));
    if (iNewIndex < 0 || iNewIndex >= count)
      throw new ArgumentOutOfRangeException(nameof (iNewIndex));
    ChartImpl chartImpl = this[iOldIndex] as ChartImpl;
    this.InnerList.RemoveAt(iOldIndex);
    this.InnerList.Insert(iNewIndex, (IOfficeChart) chartImpl);
  }

  protected override void OnClear()
  {
    base.OnClear();
    this.m_hashNames.Clear();
  }

  private void MoveInternal(int iOldSheetIndex, int iNewSheetIndex)
  {
    if (iOldSheetIndex == iNewSheetIndex)
      return;
    int count = this.InnerList.Count;
    if (iOldSheetIndex < 0 || iOldSheetIndex >= count)
      throw new ArgumentOutOfRangeException("iOldIndex");
    if (iNewSheetIndex < 0 || iNewSheetIndex >= count)
      throw new ArgumentOutOfRangeException("iNewIndex");
    ChartImpl chartImpl = this[iOldSheetIndex] as ChartImpl;
    this.InnerList.RemoveAt(iOldSheetIndex);
    this.InnerList.Insert(iNewSheetIndex, (IOfficeChart) chartImpl);
    int num1 = Math.Min(iNewSheetIndex, iOldSheetIndex);
    int num2 = Math.Max(iNewSheetIndex, iOldSheetIndex);
    for (int i = num1; i <= num2; ++i)
      (this[i] as ChartImpl).Index = i;
  }

  public void AddInternal(IOfficeChart chartToAdd)
  {
    if (chartToAdd == null)
      throw new ArgumentNullException(nameof (chartToAdd));
    if ((chartToAdd as ChartImpl).Name == null || (chartToAdd as ChartImpl).Name.Length == 0)
      (chartToAdd as ChartImpl).Name = CollectionBaseEx<IOfficeChart>.GenerateDefaultName((ICollection<IOfficeChart>) this.List, "Chart");
    this.m_hashNames.Add((chartToAdd as ChartImpl).Name, chartToAdd);
    (chartToAdd as ChartImpl).Index = this.Count;
    base.Add(chartToAdd);
  }
}
