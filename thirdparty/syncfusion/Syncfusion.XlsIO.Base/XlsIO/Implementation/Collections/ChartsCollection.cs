// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.ChartsCollection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Charts;
using Syncfusion.XlsIO.Implementation.Security;
using Syncfusion.XlsIO.Implementation.XmlSerialization;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections;

public class ChartsCollection : CollectionBaseEx<IChart>, ICharts, IEnumerable, IParentApplication
{
  public const string DEF_CHART_NAME_START = "Chart";
  private Dictionary<string, IChart> m_hashNames = new Dictionary<string, IChart>((IEqualityComparer<string>) System.StringComparer.CurrentCultureIgnoreCase);
  private WorkbookImpl m_book;

  public ChartsCollection(IApplication application, object parent)
    : base(application, parent)
  {
    this.SetParents();
    this.m_book.Objects.TabSheetMoved += new TabSheetMovedEventHandler(this.Objects_TabSheetMoved);
  }

  public IChart this[string name] => this.m_hashNames[name];

  public IChart Add()
  {
    ChartImpl chartToAdd = new ChartImpl(this.Application, (object) this);
    chartToAdd.Name = CollectionBaseEx<IChart>.GenerateDefaultName((ICollection<IChart>) this.List, "Chart");
    return this.Add((IChart) chartToAdd);
  }

  public IChart Add(string name)
  {
    ChartImpl chartToAdd = new ChartImpl(this.Application, (object) this);
    chartToAdd.Name = name;
    return this.Add((IChart) chartToAdd);
  }

  public IChart Remove(string name)
  {
    if (!this.m_hashNames.ContainsKey(name))
      return (IChart) null;
    if (this.m_book.ObjectCount == 1)
      throw new ArgumentException("Can't remove last worksheet from the workbook.");
    IChart hashName = this.m_hashNames[name];
    this.Remove(hashName);
    this.m_book.Objects.RemoveAt(((ISerializableNamedObject) hashName).RealIndex);
    return hashName;
  }

  public IChart Add(IChart chartToAdd)
  {
    this.AddInternal(chartToAdd);
    this.m_book.Objects.Add((ISerializableNamedObject) chartToAdd);
    return chartToAdd;
  }

  [CLSCompliant(false)]
  public IChart Add(BiffReader reader)
  {
    return this.Add(reader, ExcelParseOptions.Default, false, (Dictionary<int, int>) null, (IDecryptor) null);
  }

  [CLSCompliant(false)]
  public IChart Add(
    BiffReader reader,
    ExcelParseOptions options,
    bool bSkip,
    Dictionary<int, int> hashNewXFormatIndexes,
    IDecryptor decryptor)
  {
    return this.Add((IChart) new ChartImpl(this.Application, (object) this, reader, options, bSkip, hashNewXFormatIndexes, decryptor));
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
    this.InnerList.Insert(iNewIndex, (IChart) chartImpl);
  }

  private void ChartsCollection_NameChanged(object sender, ValueChangedEventArgs e)
  {
    CollectionBaseEx<IChart>.ChangeName((IDictionary) this.m_hashNames, e);
  }

  protected override void OnClear()
  {
    base.OnClear();
    this.m_hashNames.Clear();
  }

  private void UpdateSheetIndex(ChartImpl chart, int iOldRealIndex)
  {
    int num1 = chart != null ? chart.RealIndex : throw new ArgumentNullException(nameof (chart));
    ITabSheets tabSheets = this.m_book.TabSheets;
    int num2 = iOldRealIndex;
    int num3;
    int num4;
    if (iOldRealIndex > num1)
    {
      num3 = num1 + 1;
      num4 = 1;
    }
    else
    {
      if (iOldRealIndex >= num1)
        throw new NotImplementedException("Chart wasn't moved at all");
      num3 = num1 - 1;
      num4 = -1;
    }
    ITabSheet tabSheet1 = (ITabSheet) null;
    for (int index = num3; index <= num2; index += num4)
    {
      ITabSheet tabSheet2 = tabSheets[index];
      if (tabSheet2 is ChartImpl)
      {
        tabSheet1 = tabSheet2;
        break;
      }
    }
    if (tabSheet1 == null)
      return;
    ChartImpl chartImpl = (ChartImpl) tabSheet1;
    this.MoveInternal(chart.Index, chartImpl.Index);
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
    this.InnerList.Insert(iNewSheetIndex, (IChart) chartImpl);
    int num1 = Math.Min(iNewSheetIndex, iOldSheetIndex);
    int num2 = Math.Max(iNewSheetIndex, iOldSheetIndex);
    for (int i = num1; i <= num2; ++i)
      (this[i] as ChartImpl).Index = i;
  }

  public void AddInternal(IChart chartToAdd)
  {
    if (chartToAdd == null)
      throw new ArgumentNullException(nameof (chartToAdd));
    if (chartToAdd.Name == null || chartToAdd.Name.Length == 0)
      chartToAdd.Name = CollectionBaseEx<IChart>.GenerateDefaultName((ICollection<IChart>) this.List, "Chart");
    this.m_hashNames.Add(chartToAdd.Name, chartToAdd);
    ChartImpl chartImpl = chartToAdd as ChartImpl;
    chartImpl.Index = this.Count;
    chartImpl.NameChanged += new ValueChangedEventHandler(this.ChartsCollection_NameChanged);
    base.Add(chartToAdd);
  }

  private void Objects_TabSheetMoved(object sender, TabSheetMovedEventArgs args)
  {
    if (!(((ITabSheets) sender)[args.NewIndex] is ChartImpl chart))
      return;
    int oldIndex = args.OldIndex;
    this.UpdateSheetIndex(chart, oldIndex);
  }

  public void AddCopy(IChart chartToCopy)
  {
    if (chartToCopy == null)
      throw new ArgumentNullException(nameof (chartToCopy));
    Dictionary<int, int> dicFontIndexes = new Dictionary<int, int>();
    Dictionary<int, int> hashExtFormatIndexes = new Dictionary<int, int>();
    Dictionary<string, string> hashNewNames = this.m_book.InnerStyles.Merge(chartToCopy.Workbook, ExcelStyleMergeOptions.CreateDiffName, out dicFontIndexes, out hashExtFormatIndexes);
    ChartImpl chartToAdd = (chartToCopy as ChartImpl).Clone(hashNewNames, (object) this, dicFontIndexes);
    if (this.m_book != chartToCopy.Workbook)
      chartToAdd.IsAddCopied = true;
    chartToAdd.ClearEvents();
    chartToAdd.Name = CollectionBaseEx<object>.GenerateDefaultName((ICollection<object>) this.m_book.Objects, chartToCopy.Name);
    chartToAdd.m_dataHolder = (WorksheetDataHolder) null;
    this.m_book.InnerCharts.Add((IChart) chartToAdd);
  }
}
