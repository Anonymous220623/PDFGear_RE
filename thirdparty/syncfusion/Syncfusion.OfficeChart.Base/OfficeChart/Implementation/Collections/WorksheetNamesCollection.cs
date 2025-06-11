// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Collections.WorksheetNamesCollection
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Interfaces;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using Syncfusion.OfficeChart.Parser.Biff_Records.Formula;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Collections;

internal class WorksheetNamesCollection : CollectionBaseEx<IName>, INames, IEnumerable
{
  private Dictionary<string, IName> m_hashNameToIName = new Dictionary<string, IName>();
  private WorkbookImpl m_book;
  private WorksheetImpl m_worksheet;

  public WorksheetNamesCollection(IApplication application, object parent)
    : base(application, parent)
  {
    this.SetParents();
  }

  public IName this[string name]
  {
    get
    {
      IName name1;
      this.m_hashNameToIName.TryGetValue(name, out name1);
      if (name1 == null)
        return (IName) null;
      return !(name1 as NameImpl).IsDeleted ? name1 : (IName) null;
    }
  }

  public IWorksheet ParentWorksheet => (IWorksheet) this.m_worksheet;

  public void Rename(IName name, string strOldName)
  {
    if (!this.Contains(strOldName))
      return;
    this.m_hashNameToIName.Remove(strOldName);
    this.m_hashNameToIName.Add(name.Name, name);
  }

  public IName Add(string name)
  {
    IName name1;
    this.m_hashNameToIName.TryGetValue(name, out name1);
    if (name1 != null)
    {
      (name1 as NameImpl).IsDeleted = false;
      return name1;
    }
    IName name2 = (IName) new NameImpl(this.Application, (object) this, name, this.Count, true);
    this.Add(name2);
    return name2;
  }

  public IName Add(string name, IRange namedRange)
  {
    NameImpl nameImpl = new NameImpl(this.Application, (object) this, name, namedRange, this.Count, true);
    this.Add((IName) nameImpl);
    return (IName) nameImpl;
  }

  public IName Add(IName name) => this.Add(name, true);

  public IName Add(IName name, bool bAddInGlobalNamesHash)
  {
    if (name == null)
      throw new ArgumentNullException(nameof (name));
    base.Add(name);
    ((WorkbookNamesCollection) this.m_book.Names).AddLocal(name, bAddInGlobalNamesHash);
    return name;
  }

  public void Remove(string name)
  {
    IName name1;
    if (!this.m_hashNameToIName.TryGetValue(name, out name1))
      return;
    name1.Delete();
  }

  public new void Clear()
  {
    for (int index = this.Count - 1; index >= 0; --index)
      this.Remove(this[index].Name);
  }

  public bool Contains(string name)
  {
    return this.m_hashNameToIName.ContainsKey(name) && !(this.m_hashNameToIName[name] as NameImpl).IsDeleted;
  }

  int INames.Count => this.GetKnownNamedCount();

  private int GetKnownNamedCount()
  {
    int knownNamedCount = 0;
    foreach (IName name in (IEnumerable<IName>) this.List)
    {
      if (name.RefersToRange != null)
        ++knownNamedCount;
    }
    return knownNamedCount;
  }

  public new IName this[int index]
  {
    get
    {
      if (index < 0 || index >= this.List.Count)
        throw new ArgumentOutOfRangeException($"index is {index}, Count is {this.List.Count}");
      int num = 0;
      for (int index1 = 0; index1 < this.List.Count; ++index1)
      {
        NameImpl nameImpl = this.List[index1] as NameImpl;
        if (!nameImpl.IsDeleted)
        {
          if (num == index)
            return (IName) nameImpl;
          ++num;
        }
      }
      return (IName) null;
    }
  }

  public IName AddLocal(IName name)
  {
    if (name == null)
      throw new ArgumentNullException(nameof (name));
    base.Add(name);
    return name;
  }

  [CLSCompliant(false)]
  public IName Add(NameRecord name) => this.Add(name, true);

  [CLSCompliant(false)]
  public IName Add(NameRecord name, bool bAddInGlobalNamesHash)
  {
    if (name == null)
      throw new ArgumentNullException(nameof (name));
    NameImpl nameImpl = new NameImpl(this.Application, (object) this, name.Name, this.Count);
    nameImpl.Parse(name);
    ((IParseable) nameImpl).Parse();
    this.Add((IName) nameImpl, bAddInGlobalNamesHash);
    return (IName) nameImpl;
  }

  [CLSCompliant(false)]
  public void AddRange(NameRecord[] names)
  {
    if (names == null)
      throw new ArgumentNullException(nameof (names));
    string str = $"'{this.m_worksheet.Name}'";
    FormulaUtil formulaUtil = this.m_book.FormulaUtil;
    int index = 0;
    for (int length = names.Length; index < length; ++index)
    {
      NameRecord name = names[index];
      if (formulaUtil.ParsePtgArray(name.FormulaTokens, 0, 0, false, false).StartsWith(str))
        this.Add(name);
    }
  }

  internal void FillFrom(
    WorksheetNamesCollection sourceNames,
    IDictionary hashNewWorksheetNames,
    Dictionary<int, int> dicNewNameIndexes,
    ExcelNamesMergeOptions option,
    Dictionary<int, int> hashExternSheetIndexes)
  {
    if (sourceNames == null)
      throw new ArgumentNullException(nameof (sourceNames));
    if (hashExternSheetIndexes == null)
      throw new ArgumentNullException(nameof (hashExternSheetIndexes));
    WorkbookImpl book = sourceNames.m_book;
    int index = 0;
    for (int count = sourceNames.Count; index < count; ++index)
    {
      NameImpl inner = (NameImpl) sourceNames.InnerList[index];
      NameRecord name1 = (NameRecord) inner.Record.Clone();
      name1.IndexOrGlobal = (ushort) (this.m_worksheet.RealIndex + 1);
      WorksheetNamesCollection.UpdateReferenceIndexes(name1, book, hashNewWorksheetNames, hashExternSheetIndexes, this.m_book);
      IName name2 = this.Add(name1);
      dicNewNameIndexes.Add(inner.Index, (name2 as NameImpl).Index);
    }
  }

  internal static void UpdateReferenceIndexes(
    NameRecord name,
    WorkbookImpl oldBook,
    IDictionary hashNewWorksheetNames,
    Dictionary<int, int> hashExternSheetIndexes,
    WorkbookImpl newBook)
  {
    if (hashExternSheetIndexes == null)
      return;
    Ptg[] formulaTokens = name.FormulaTokens;
    if (formulaTokens == null || formulaTokens.Length == 0)
      return;
    if (oldBook == null)
      throw new ArgumentException(nameof (oldBook));
    if (newBook == null)
      throw new ArgumentNullException(nameof (newBook));
    int index = 0;
    for (int length = formulaTokens.Length; index < length; ++index)
    {
      if (formulaTokens[index] is IReference reference)
      {
        int refIndex = (int) reference.RefIndex;
        string str = oldBook.GetSheetNameByReference(refIndex) ?? "#REF";
        if (str != null && hashNewWorksheetNames.Contains((object) str))
          str = (string) hashNewWorksheetNames[(object) str];
        if (str == "#REF")
        {
          if (hashExternSheetIndexes.ContainsKey(refIndex))
          {
            int externSheetIndex = hashExternSheetIndexes[refIndex];
            reference.RefIndex = (ushort) externSheetIndex;
          }
        }
        else
        {
          int num = newBook.AddSheetReference(str);
          reference.RefIndex = (ushort) num;
        }
      }
    }
  }

  public void SetSheetIndex(int iSheetIndex)
  {
    for (int index = this.Count - 1; index >= 0; --index)
      ((NameImpl) this.InnerList[index]).SetSheetIndex(iSheetIndex);
  }

  public NameImpl GetOrCreateName(string strName)
  {
    switch (strName)
    {
      case null:
        throw new ArgumentNullException(nameof (strName));
      case "":
        throw new ArgumentException("strName - string cannot be empty.");
      default:
        if (!(this[strName] is NameImpl name))
          name = (NameImpl) this.Add(strName);
        return name;
    }
  }

  public void ConvertFullRowColumnNames(OfficeVersion version)
  {
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      ((NameImpl) this.GetNameByIndex(index)).ConvertFullRowColumnName(version);
  }

  internal IName GetNameByIndex(int index) => this.List[index];

  private void SetParents()
  {
    this.m_worksheet = this.FindParent(typeof (WorksheetImpl)) as WorksheetImpl;
    this.m_book = this.m_worksheet != null ? this.m_worksheet.ParentWorkbook : throw new ArgumentNullException("WorksheetNamesCollection has no parent Worksheet.");
  }

  protected override void OnInsertComplete(int index, IName value)
  {
    string name = ((NameImpl) value).Name;
    if (!this.m_book.IsWorkbookOpening || !this.m_hashNameToIName.ContainsKey(name))
      this.m_hashNameToIName[name] = value;
    base.OnInsertComplete(index, value);
  }
}
