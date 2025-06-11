// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Collections.WorkbookNamesCollection
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Interfaces;
using Syncfusion.OfficeChart.Parser;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using Syncfusion.OfficeChart.Parser.Biff_Records.Formula;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Collections;

internal class WorkbookNamesCollection : CollectionBaseEx<IName>, INames, IEnumerable
{
  private char[] SpecialChars = new char[23]
  {
    '!',
    '@',
    '#',
    '$',
    '%',
    '^',
    '&',
    '*',
    '(',
    ')',
    '-',
    '=',
    '+',
    ']',
    '}',
    '[',
    '{',
    ';',
    ':',
    '/',
    '.',
    '>',
    '<'
  };
  private Dictionary<string, IName> m_hashNameToIName = new Dictionary<string, IName>();
  private WorkbookImpl m_book;
  private bool m_bWorkNamesChanged;

  public WorkbookNamesCollection(IApplication application, object parent)
    : base(application, parent)
  {
    this.SetParent();
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

  public IWorksheet ParentWorksheet => (IWorksheet) null;

  int INames.Count => this.GetKnownNamedCount();

  public new int Count => this.List.Count;

  public IName Add(string name)
  {
    switch (name)
    {
      case null:
        throw new ArgumentNullException(nameof (name));
      case "":
        throw new ArgumentException(nameof (name));
      default:
        if (!WorkbookNamesCollection.IsValidName(name, this.m_book) || char.IsNumber(name[0]))
          throw new ArgumentException("This is not a valid name. Name should not be same as the cell name.");
        IName name1;
        this.m_hashNameToIName.TryGetValue(name, out name1);
        if (name1 != null && !name1.IsLocal)
        {
          (name1 as NameImpl).IsDeleted = false;
          return name1;
        }
        NameImpl nameImpl = new NameImpl(this.Application, (object) this.m_book, name, this.List.Count);
        this.Add((IName) nameImpl);
        return (IName) nameImpl;
    }
  }

  private void CheckInvalidCharacters(string name)
  {
    string str = "?";
    if (name.IndexOfAny(this.SpecialChars) != -1 || name.Contains("\"") || name.StartsWith(str))
      throw new ArgumentException("Contains invalid characters");
    if (char.IsNumber(name[0]))
      throw new ArgumentException("Contains invalid characters");
    int num1 = 0;
    int num2 = 0;
    bool flag = false;
    int length = name.Length;
    foreach (char c in name)
    {
      if (char.IsLetter(c))
        ++num1;
      else if (char.IsNumber(c))
        ++num2;
    }
    if (char.IsLetter(name[length - 1]) || name.EndsWith(str))
      flag = true;
    if (num1 <= 3 && num2 > 0 && !flag)
      throw new ArgumentException("Contains invalid characters");
  }

  protected override void OnClearComplete() => this.m_hashNameToIName.Clear();

  public IName Add(string name, IRange namedRange)
  {
    if (name == null)
      throw new ArgumentNullException(nameof (name));
    if (namedRange == null)
      throw new ArgumentNullException(nameof (namedRange));
    if (name.Length == 0)
      throw new ArgumentException(nameof (name));
    if (!WorkbookNamesCollection.IsValidName(name, this.m_book))
      throw new ArgumentException("This is not a valid name. Name should not be same as the cell name.");
    NameImpl nameImpl = new NameImpl(this.Application, (object) this, name, namedRange, this.List.Count);
    this.Add((IName) nameImpl);
    return (IName) nameImpl;
  }

  internal static bool IsValidName(string name, WorkbookImpl book)
  {
    bool bR1C1 = FormulaUtil.IsR1C1(name);
    string strRow;
    string strColumn;
    bool flag = FormulaUtil.IsCell(name, bR1C1, out strRow, out strColumn);
    if (strRow == null || strColumn == null)
      return true;
    int columnIndex = RangeImpl.GetColumnIndex(strColumn);
    int int32 = Convert.ToInt32(strRow);
    return !flag || int32 >= book.MaxRowCount || columnIndex >= book.MaxColumnCount || int32 == 0;
  }

  internal void Validate()
  {
    foreach (NameImpl nameImpl in (CollectionBase<IName>) this)
    {
      if (!WorkbookNamesCollection.IsValidName(nameImpl.Name, this.m_book))
        throw new Exception($"Named Range {nameImpl.Name} is not supported in this version");
    }
  }

  public IName Add(IName name)
  {
    NameImpl nameImpl = name as NameImpl;
    bool isExternName = nameImpl.IsExternName;
    if (!this.m_book.IsWorkbookOpening && !isExternName && !nameImpl.IsLocal && this.m_hashNameToIName.ContainsKey(name.Name))
      throw new ArgumentException("Name of the Name object must be unique.");
    this.AddLocal(name);
    this.IsWorkbookNamesChanged = true;
    return name;
  }

  public void Remove(string name)
  {
    IName name1;
    if (!this.m_hashNameToIName.TryGetValue(name, out name1))
      return;
    name1.Delete();
  }

  public new void RemoveAt(int index)
  {
    IName name = index >= 0 && index <= this.Count - 1 ? this[index] : throw new ArgumentOutOfRangeException(nameof (index), "Value cannot be less than 0 and greater than Count - 1.");
    if (this.m_hashNameToIName.ContainsValue(name))
      this.m_hashNameToIName.Remove(name.Name);
    IList<IName> list = this.List;
    list.RemoveAt(index);
    this.IsWorkbookNamesChanged = true;
    Dictionary<int, int> dictionary = new Dictionary<int, int>();
    int index1 = index;
    for (int count = list.Count; index1 < count; ++index1)
    {
      NameImpl nameImpl = (NameImpl) list[index1];
      dictionary.Add(nameImpl.Index, index1);
      nameImpl.SetIndex(index1);
    }
  }

  public void Remove(int[] arrIndexes)
  {
    System.Collections.Generic.List<int> intList = new System.Collections.Generic.List<int>((IEnumerable<int>) arrIndexes);
    intList.Sort();
    int count1 = this.List.Count;
    for (int index1 = intList.Count - 1; index1 >= 0; --index1)
    {
      int index2 = intList[index1];
      if (index2 < 0 || index2 >= count1)
        throw new ArgumentOutOfRangeException("index");
      this.m_hashNameToIName.Remove(this.List[index2].Name);
      base.RemoveAt(index2);
      this.IsWorkbookNamesChanged = true;
    }
    Dictionary<int, int> dicNewIndex = new Dictionary<int, int>();
    int index = intList[0];
    for (int count2 = this.Count; index < count2; ++index)
    {
      NameImpl nameImpl = (NameImpl) this.List[index];
      dicNewIndex.Add(nameImpl.Index, index);
      nameImpl.SetIndex(index);
    }
    this.m_book.UpdateNamedRangeIndexes((IDictionary<int, int>) dicNewIndex);
  }

  public bool Contains(string name)
  {
    return this.m_hashNameToIName.ContainsKey(name) && !(this.m_hashNameToIName[name] as NameImpl).IsDeleted;
  }

  public void InsertRow(int iRowIndex, int iRowCount, string strSheetName)
  {
    this.InsertRemoveRowColumn(strSheetName, iRowIndex, false, true, iRowCount);
  }

  public void RemoveRow(int iRowIndex, string strSheetName)
  {
    this.InsertRemoveRowColumn(strSheetName, iRowIndex, true, true, 1);
  }

  public void RemoveRow(int iRowIndex, string strSheetName, int count)
  {
    this.InsertRemoveRowColumn(strSheetName, iRowIndex, true, true, count);
  }

  public void InsertColumn(int iColumnIndex, int iCount, string strSheetName)
  {
    this.InsertRemoveRowColumn(strSheetName, iColumnIndex, false, false, iCount);
  }

  public void RemoveColumn(int iColumnIndex, string strSheetName)
  {
    this.RemoveColumn(iColumnIndex, strSheetName, 1);
  }

  public void RemoveColumn(int iColumnIndex, string strSheetName, int count)
  {
    this.InsertRemoveRowColumn(strSheetName, iColumnIndex, true, false, count);
  }

  [CLSCompliant(false)]
  public IName Add(NameRecord name)
  {
    if (name == null)
      throw new ArgumentNullException(nameof (name));
    NameImpl nameImpl = new NameImpl(this.Application, (object) this, name.Name, this.List.Count);
    this.Add((IName) nameImpl);
    nameImpl.Parse(name);
    return (IName) nameImpl;
  }

  [CLSCompliant(false)]
  public void AddRange(NameRecord[] names)
  {
    if (names == null)
      throw new ArgumentNullException(nameof (names));
    int index = 0;
    for (int length = names.Length; index < length; ++index)
      this.Add(names[index]);
  }

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records)
  {
    if (this.Count == 0)
      return;
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    this.SortForSerialization();
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      ((NameImpl) this.InnerList[index]).Serialize(records);
  }

  public void AddLocal(IName name) => this.AddLocal(name, true);

  public void AddLocal(IName name, bool bAddInGlobalNamesHash)
  {
    ((NameImpl) name).SetIndex(this.Count);
    if (bAddInGlobalNamesHash)
      base.Add(name);
    else
      this.InnerList.Add(name);
    this.IsWorkbookNamesChanged = true;
  }

  internal void SortForSerialization()
  {
    if (!this.m_bWorkNamesChanged)
      return;
    this.GetSortedWorksheets();
    this.SetIndexesWithoutEvent();
    this.IsWorkbookNamesChanged = false;
  }

  private SortedList<string, object> GetSortedWorksheets()
  {
    IWorksheets worksheets = this.m_book.Worksheets;
    int count = worksheets.Count;
    SortedList<string, object> sortedWorksheets = new SortedList<string, object>(count);
    for (int Index = 0; Index < count; ++Index)
      sortedWorksheets.Add(worksheets[Index].Name, (object) null);
    return sortedWorksheets;
  }

  private int[] GetNewIndexes(SortedList<string, object> list)
  {
    if (list == null)
      throw new ArgumentNullException(nameof (list));
    int count1 = this.Count;
    System.Collections.Generic.List<IName> nameList = new System.Collections.Generic.List<IName>((IEnumerable<IName>) this.InnerList);
    int[] newIndexes = new int[count1];
    int index1 = 0;
    bool[] flagArray = new bool[count1];
    int count2 = list.Count;
    string strGlobalName = list.Keys[count2 - 1] + "_1";
    for (int index2 = 0; index2 < count1; ++index2)
    {
      if (!flagArray[index2])
      {
        NameImpl name = (NameImpl) nameList[index2];
        WorksheetImpl worksheet = name.Worksheet;
        SortedList<string, NameImpl> sameNames = this.FindSameNames(name, strGlobalName);
        int count3 = sameNames.Count;
        IList<NameImpl> values = sameNames.Values;
        for (int index3 = 0; index3 < count3; ++index3)
        {
          NameImpl nameImpl = values[index3];
          if (index1 >= this.InnerList.Count)
            throw new ApplicationException();
          this.InnerList[index1] = (IName) nameImpl;
          int index4 = nameImpl.Index;
          if (!flagArray[index4])
          {
            newIndexes[index4] = index1;
            flagArray[index4] = true;
            ++index1;
          }
        }
      }
    }
    return newIndexes;
  }

  private SortedList<string, NameImpl> FindSameNames(NameImpl name, string strGlobalName)
  {
    if (name == null)
      throw new ArgumentNullException(nameof (name));
    SortedList<string, NameImpl> list = new SortedList<string, NameImpl>();
    string name1 = name.Name;
    IWorksheets worksheets = this.m_book.Worksheets;
    int indexOrGlobal = (int) name.Record.IndexOrGlobal;
    IWorksheet worksheet1 = (IWorksheet) name.Worksheet;
    int Index = 0;
    for (int count = worksheets.Count; Index < count; ++Index)
    {
      IWorksheet worksheet2 = worksheets[Index];
      if (worksheet1 != worksheet2)
        this.AddNameToList(list, worksheet2.Names, name1, worksheet2.Name);
    }
    if (indexOrGlobal != 0)
    {
      this.AddNameToList(list, this.m_book.Names, name1, strGlobalName);
      strGlobalName = worksheet1.Name;
    }
    list.Add(strGlobalName, name);
    return list;
  }

  private void AddNameToList(
    SortedList<string, NameImpl> list,
    INames names,
    string strName,
    string strSheetName)
  {
    if (list == null)
      throw new ArgumentNullException(nameof (list));
    if (names == null)
      throw new ArgumentNullException(nameof (names));
    switch (strName)
    {
      case null:
        throw new ArgumentNullException(nameof (strName));
      case "":
        throw new ArgumentException("strName - string cannot be empty");
      default:
        NameImpl name = (NameImpl) names[strName];
        if (name == null)
          break;
        list.Add(strSheetName, name);
        break;
    }
  }

  private void UpdateIndexes(int[] arrNewIndex)
  {
    if (arrNewIndex == null)
      throw new ArgumentNullException(nameof (arrNewIndex));
    this.m_book.UpdateNamedRangeIndexes(arrNewIndex);
  }

  private void SetIndexesWithoutEvent()
  {
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      ((NameImpl) this.InnerList[index]).SetIndex(index, false);
  }

  public void ParseNames()
  {
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      ((IParseable) this[index]).Parse();
  }

  public int AddFunctions(string strFunctionName)
  {
    NameRecord record = (NameRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Name);
    record.IsFunctionOrCommandMacro = true;
    record.IsNameFunction = true;
    record.IsNameCommand = true;
    record.Name = strFunctionName;
    return (this.Add(record) as NameImpl).Index;
  }

  [CLSCompliant(false)]
  public NameRecord GetNameRecordByIndex(int index)
  {
    return index >= 0 && index < this.Count ? ((NameImpl) this[index]).Record : throw new ArgumentOutOfRangeException(nameof (index));
  }

  public IName AddCopy(
    IName nameToCopy,
    IWorksheet destSheet,
    Dictionary<int, int> hashExternSheetIndexes,
    IDictionary hashNewWorksheetNames)
  {
    if (nameToCopy == null)
      throw new ArgumentNullException(nameof (nameToCopy));
    if (destSheet == null)
      throw new ArgumentNullException(nameof (destSheet));
    string name1 = nameToCopy.Name;
    this.m_book.AddSheetReference(destSheet);
    NameImpl nameImpl = (NameImpl) nameToCopy;
    NameRecord name2 = nameImpl.Record.Clone() as NameRecord;
    WorkbookImpl workbook = nameImpl.Workbook;
    WorksheetNamesCollection.UpdateReferenceIndexes(name2, workbook, hashNewWorksheetNames, hashExternSheetIndexes, this.m_book);
    IName name3;
    if (this.Contains(name1))
    {
      name2.IndexOrGlobal = (ushort) (destSheet.Index + 1);
      name3 = ((WorksheetNamesCollection) destSheet.Names).Add(name2, false);
    }
    else
      name3 = this.Add(name2);
    return name3;
  }

  private void SetReferenceIndex(NameRecord name, int iRefIndex)
  {
    Ptg[] ptgArray = name != null ? name.FormulaTokens : throw new ArgumentNullException(nameof (name));
    int index = 0;
    for (int length = ptgArray.Length; index < length; ++index)
    {
      if (ptgArray[index] is IReference reference)
        reference.RefIndex = (ushort) iRefIndex;
    }
  }

  public override object Clone(object parent)
  {
    WorkbookNamesCollection workbookNamesCollection = parent != null ? (WorkbookNamesCollection) base.Clone(parent) : throw new ArgumentNullException(nameof (parent));
    workbookNamesCollection.m_bWorkNamesChanged = this.m_bWorkNamesChanged;
    return (object) workbookNamesCollection;
  }

  protected override void OnInsertComplete(int index, IName value)
  {
    NameImpl nameImpl = (NameImpl) value;
    base.OnInsertComplete(index, value);
    if (nameImpl.IsBuiltIn || nameImpl.IsExternName || nameImpl.IsLocal)
      return;
    this.m_hashNameToIName[nameImpl.Name] = (IName) nameImpl;
  }

  public void ConvertFullRowColumnNames(OfficeVersion version)
  {
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      ((NameImpl) this.GetNameByIndex(index)).ConvertFullRowColumnName(version);
  }

  internal IName GetNameByIndex(int index) => this.List[index];

  public bool IsWorkbookNamesChanged
  {
    get => this.m_bWorkNamesChanged;
    set
    {
      if (this.m_book.IsWorkbookOpening)
        return;
      this.m_bWorkNamesChanged = value;
    }
  }

  private void SetParent()
  {
    this.m_book = this.FindParent(typeof (WorkbookImpl)) as WorkbookImpl;
    if (this.m_book == null)
      throw new ArgumentNullException("NamesCollection has no parent Workbook.");
  }

  private void InsertRemoveRowColumn(
    string strSheetName,
    int index,
    bool bIsRemove,
    bool bIsRow,
    int iCount)
  {
    int num = bIsRow ? this.m_book.MaxRowCount : this.m_book.MaxColumnCount;
    if (index < 1 || index > num)
      throw new ArgumentOutOfRangeException(nameof (index));
    switch (strSheetName)
    {
      case null:
        throw new ArgumentNullException(nameof (strSheetName));
      case "":
        throw new ArgumentException(nameof (strSheetName));
      default:
        System.Collections.Generic.List<IName> innerList = this.InnerList;
        int count = innerList.Count;
        if (iCount == 0)
          break;
        this.IsWorkbookNamesChanged = true;
        --index;
        int index1 = 0;
        for (int index2 = count; index1 < index2; ++index1)
        {
          NameImpl nameImpl = (NameImpl) innerList[index1];
          NameRecord record = nameImpl.Record;
          Ptg[] formulaTokens = record.FormulaTokens;
          if (formulaTokens != null)
          {
            int index3 = 0;
            for (int length = formulaTokens.Length; index3 < length; ++index3)
            {
              if (formulaTokens[index3] is IRangeGetterToken token && (!(token is IReference reference) || !this.m_book.IsExternalReference((int) reference.RefIndex)))
              {
                if (nameImpl != null && nameImpl.Worksheet != null && (nameImpl.Worksheet.ParseDataOnDemand || nameImpl.Worksheet.ParseOnDemand))
                  nameImpl.Worksheet.ParseData((Dictionary<int, int>) null);
                Ptg ptg = this.InsertRemoveRow(token, strSheetName, index, bIsRemove, bIsRow, iCount, (IWorksheet) nameImpl.Worksheet) ?? token.ConvertToError();
                formulaTokens[index3] = ptg;
              }
            }
            record.FormulaTokens = formulaTokens;
          }
        }
        break;
    }
  }

  private Ptg InsertRemoveRow(
    IRangeGetterToken token,
    string strSheetName,
    int index,
    bool bIsRemove,
    bool bIsRow,
    int iCount,
    IWorksheet sheet)
  {
    Ptg ptg1 = token != null ? (Ptg) token : throw new ArgumentNullException(nameof (token));
    IRange range = token.GetRange((IWorkbook) this.m_book, sheet);
    if (range != null && range.Worksheet.Name == strSheetName)
    {
      Rectangle rectangle = token.GetRectangle();
      MergeCellsRecord.MergedRegion region = new MergeCellsRecord.MergedRegion(rectangle.Top, rectangle.Bottom, rectangle.Left, rectangle.Right);
      Ptg ptg2 = (Ptg) null;
      MergeCellsRecord.MergedRegion mergedRegion = bIsRow ? MergeCellsImpl.InsertRemoveRow(region, index, bIsRemove, iCount, (IWorkbook) this.m_book) : MergeCellsImpl.InsertRemoveColumn(region, index, bIsRemove, iCount, (IWorkbook) this.m_book);
      Ptg ptg3;
      if (mergedRegion == null)
        ptg3 = (Ptg) null;
      else
        ptg2 = ptg3 = token.UpdateRectangle(mergedRegion.GetRectangle());
      ptg1 = ptg3;
    }
    return ptg1;
  }

  public void MarkUsedReferences(bool[] usedItems)
  {
    System.Collections.Generic.List<IName> innerList = this.InnerList;
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      FormulaUtil.MarkUsedReferences(((NameImpl) innerList[index]).Record.FormulaTokens, usedItems);
  }

  public void UpdateReferenceIndexes(int[] arrUpdatedIndexes)
  {
    System.Collections.Generic.List<IName> innerList = this.InnerList;
    int index = 0;
    for (int count = this.Count; index < count; ++index)
    {
      NameRecord record = ((NameImpl) innerList[index]).Record;
      Ptg[] formulaTokens = record.FormulaTokens;
      if (FormulaUtil.UpdateReferenceIndexes(formulaTokens, arrUpdatedIndexes))
        record.FormulaTokens = formulaTokens;
    }
  }

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
}
