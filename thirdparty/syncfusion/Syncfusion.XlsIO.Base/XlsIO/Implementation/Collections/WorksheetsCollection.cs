// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.WorksheetsCollection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Security;
using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections;

public class WorksheetsCollection : 
  CollectionBaseEx<IWorksheet>,
  IWorksheets,
  IEnumerable<IWorksheet>,
  IEnumerable,
  ICloneParent
{
  private const int MaxSheetNameLength = 31 /*0x1F*/;
  private Dictionary<string, IWorksheet> m_list = new Dictionary<string, IWorksheet>((IEqualityComparer<string>) System.StringComparer.CurrentCultureIgnoreCase);
  private WorkbookImpl m_book;
  private bool m_bUseHash = true;
  internal static string[] sheetNameValidators = new string[7]
  {
    "*",
    "?",
    ":",
    "/",
    "[",
    "]",
    "\\"
  };

  public WorksheetsCollection(IApplication application, object parent)
    : base(application, parent)
  {
    this.m_book = this.FindParent(typeof (WorkbookImpl)) as WorkbookImpl;
    if (this.m_book == null)
      throw new ArgumentNullException("Can't find parent workbook.");
    this.m_book.Objects.TabSheetMoved += new TabSheetMovedEventHandler(this.Objects_TabSheetMoved);
  }

  public new IWorksheet this[int Index] => this.InnerList[Index];

  public IWorksheet this[string sheetName]
  {
    get
    {
      IWorksheet worksheet1 = (IWorksheet) null;
      if (this.m_bUseHash)
      {
        this.m_list.TryGetValue(sheetName, out worksheet1);
      }
      else
      {
        System.Collections.Generic.List<IWorksheet> innerList = this.InnerList;
        System.StringComparer cultureIgnoreCase = System.StringComparer.CurrentCultureIgnoreCase;
        int index = 0;
        for (int count = innerList.Count; index < count; ++index)
        {
          IWorksheet worksheet2 = innerList[index];
          if (cultureIgnoreCase.Compare(worksheet2.Name, sheetName) == 0)
          {
            worksheet1 = worksheet2;
            break;
          }
        }
      }
      return worksheet1;
    }
  }

  public bool UseRangesCache
  {
    get
    {
      if (this.Count == 0)
        return false;
      System.Collections.Generic.List<IWorksheet> innerList = this.InnerList;
      bool useRangesCache = innerList[0].UseRangesCache;
      int index = 1;
      for (int count = innerList.Count; index < count; ++index)
      {
        if (innerList[index].UseRangesCache != useRangesCache)
          return false;
      }
      return useRangesCache;
    }
    set
    {
      System.Collections.Generic.List<IWorksheet> innerList = this.InnerList;
      int index = 0;
      for (int count = innerList.Count; index < count; ++index)
        innerList[index].UseRangesCache = value;
    }
  }

  public bool UseHashForWorksheetLookup
  {
    get => this.m_bUseHash;
    set
    {
      if (this.m_bUseHash == value)
        return;
      this.m_bUseHash = value;
      if (value)
      {
        System.Collections.Generic.List<IWorksheet> innerList = this.InnerList;
        int index = 0;
        for (int count = this.Count; index < count; ++index)
        {
          IWorksheet worksheet = innerList[index];
          this.m_list.Add(worksheet.Name, worksheet);
        }
      }
      else
        this.m_list.Clear();
    }
  }

  internal IWorksheet Add(IWorksheet sheet)
  {
    if (this.m_book.HasMacros && !this.m_book.Loading)
      (sheet as WorksheetImpl).AddVbaModule();
    this.m_book.Objects.Add((ISerializableNamedObject) sheet);
    base.Add(sheet);
    return sheet;
  }

  protected internal void RemoveLocal(string name) => base.Remove(this[name]);

  public void Move(int iOldIndex, int iNewIndex)
  {
    if (iOldIndex == iNewIndex)
      return;
    int count = this.InnerList.Count;
    if (iOldIndex < 0 || iOldIndex >= count)
      throw new ArgumentOutOfRangeException(nameof (iOldIndex));
    if (iNewIndex < 0 || iNewIndex >= count)
      throw new ArgumentOutOfRangeException(nameof (iNewIndex));
    this.m_book.Objects.Move(iOldIndex, iNewIndex);
    WorksheetImpl worksheetImpl = this[iOldIndex] as WorksheetImpl;
    this.InnerList.RemoveAt(iOldIndex);
    this.InnerList.Insert(iNewIndex, (IWorksheet) worksheetImpl);
    int num1 = Math.Min(iNewIndex, iOldIndex);
    int num2 = Math.Max(iNewIndex, iOldIndex);
    for (int Index = num1; Index <= num2; ++Index)
      (this[Index] as WorksheetImpl).Index = Index;
  }

  public void UpdateSheetIndex(WorksheetImpl sheet, int iOldRealIndex)
  {
    int num1 = sheet != null ? sheet.RealIndex : throw new ArgumentNullException(nameof (sheet));
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
        throw new NotImplementedException("Worksheet wasn't moved at all");
      num3 = num1 - 1;
      num4 = -1;
    }
    ITabSheet tabSheet1 = (ITabSheet) null;
    int index = num3;
    ITabSheet tabSheet2;
    while (true)
    {
      tabSheet2 = tabSheets[index];
      if (!(tabSheet2 is WorksheetImpl))
      {
        if (index != num2)
          index += num4;
        else
          goto label_12;
      }
      else
        break;
    }
    tabSheet1 = tabSheet2;
label_12:
    if (tabSheet1 == null)
      return;
    WorksheetImpl worksheetImpl = (WorksheetImpl) tabSheet1;
    this.MoveInternal(sheet.Index, worksheetImpl.Index);
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
    WorksheetImpl worksheetImpl = this[iOldSheetIndex] as WorksheetImpl;
    this.InnerList.RemoveAt(iOldSheetIndex);
    this.InnerList.Insert(iNewSheetIndex, (IWorksheet) worksheetImpl);
    int num1 = Math.Min(iNewSheetIndex, iOldSheetIndex);
    int num2 = Math.Max(iNewSheetIndex, iOldSheetIndex);
    for (int Index = num1; Index <= num2; ++Index)
      (this[Index] as WorksheetImpl).Index = Index;
  }

  public IRange FindFirst(string findValue, ExcelFindType flags)
  {
    return this.FindFirst(findValue, flags, ExcelFindOptions.None);
  }

  public IRange FindFirst(string findValue, ExcelFindType flags, ExcelFindOptions findOptions)
  {
    if (findValue == null)
      return (IRange) null;
    bool flag1 = (flags & ExcelFindType.Formula) == ExcelFindType.Formula;
    bool flag2 = (flags & ExcelFindType.Text) == ExcelFindType.Text;
    bool flag3 = (flags & ExcelFindType.FormulaStringValue) == ExcelFindType.FormulaStringValue;
    bool flag4 = (flags & ExcelFindType.Error) == ExcelFindType.Error;
    bool flag5 = (flags & ExcelFindType.Comments) == ExcelFindType.Comments;
    if (!flag1 && !flag2 && !flag3 && !flag4 && !flag5)
      throw new ArgumentException("Parameter flag is not valid.", nameof (flags));
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
    {
      IRange first = innerList[index].FindFirst(findValue, flags, findOptions);
      if (first != null)
        return first;
    }
    return (IRange) null;
  }

  public IRange FindFirst(double findValue, ExcelFindType flags)
  {
    bool flag1 = (flags & ExcelFindType.FormulaValue) == ExcelFindType.FormulaValue;
    bool flag2 = (flags & ExcelFindType.Number) == ExcelFindType.Number;
    if (!flag1 && !flag2)
      throw new ArgumentException("Parameter flag is not valid.", nameof (flags));
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
    {
      IRange first = innerList[index].FindFirst(findValue, flags);
      if (first != null)
        return first;
    }
    return (IRange) null;
  }

  public IRange FindFirst(bool findValue)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
    {
      IRange first = innerList[index].FindFirst(findValue);
      if (first != null)
        return first;
    }
    return (IRange) null;
  }

  public IRange FindFirst(DateTime findValue)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
    {
      IRange first = innerList[index].FindFirst(findValue);
      if (first != null)
        return first;
    }
    return (IRange) null;
  }

  public IRange FindFirst(TimeSpan findValue)
  {
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
    {
      IRange first = innerList[index].FindFirst(findValue);
      if (first != null)
        return first;
    }
    return (IRange) null;
  }

  public IRange[] FindAll(string findValue, ExcelFindType flags)
  {
    return this.FindAll(findValue, flags, ExcelFindOptions.None);
  }

  public IRange[] FindAll(string findValue, ExcelFindType flags, ExcelFindOptions findOptions)
  {
    if (findValue == null)
      return (IRange[]) null;
    bool flag1 = (flags & ExcelFindType.Formula) == ExcelFindType.Formula;
    bool flag2 = (flags & ExcelFindType.Text) == ExcelFindType.Text;
    bool flag3 = (flags & ExcelFindType.FormulaStringValue) == ExcelFindType.FormulaStringValue;
    bool flag4 = (flags & ExcelFindType.Error) == ExcelFindType.Error;
    bool flag5 = (flags & ExcelFindType.Values) == ExcelFindType.Values;
    if (!flag1 && !flag2 && !flag3 && !flag4 && !flag5)
      throw new ArgumentException("Parameter flag is not valid.", nameof (flags));
    System.Collections.Generic.List<IRange> rangeList = new System.Collections.Generic.List<IRange>();
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
    {
      IRange[] all = innerList[index].FindAll(findValue, flags, findOptions);
      if (all != null)
        rangeList.AddRange((IEnumerable<IRange>) all);
    }
    return rangeList.Count == 0 ? (IRange[]) null : rangeList.ToArray();
  }

  public IRange[] FindAll(double findValue, ExcelFindType flags)
  {
    bool flag1 = (flags & ExcelFindType.FormulaValue) == ExcelFindType.FormulaValue;
    bool flag2 = (flags & ExcelFindType.Number) == ExcelFindType.Number;
    if (!flag1 && !flag2)
      throw new ArgumentException("Parameter flag is not valid.", nameof (flags));
    System.Collections.Generic.List<IRange> rangeList = new System.Collections.Generic.List<IRange>();
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
    {
      IRange[] all = innerList[index].FindAll(findValue, flags);
      if (all != null)
        rangeList.AddRange((IEnumerable<IRange>) all);
    }
    return rangeList.Count == 0 ? (IRange[]) null : rangeList.ToArray();
  }

  public IRange[] FindAll(bool findValue)
  {
    System.Collections.Generic.List<IRange> rangeList = new System.Collections.Generic.List<IRange>();
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
    {
      IRange[] all = innerList[index].FindAll(findValue);
      if (all != null)
        rangeList.AddRange((IEnumerable<IRange>) all);
    }
    return rangeList.Count == 0 ? (IRange[]) null : rangeList.ToArray();
  }

  public IRange[] FindAll(DateTime findValue)
  {
    System.Collections.Generic.List<IRange> rangeList = new System.Collections.Generic.List<IRange>();
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
    {
      IRange[] all = innerList[index].FindAll(findValue);
      if (all != null)
        rangeList.AddRange((IEnumerable<IRange>) all);
    }
    return rangeList.Count == 0 ? (IRange[]) null : rangeList.ToArray();
  }

  public IRange[] FindAll(TimeSpan findValue)
  {
    System.Collections.Generic.List<IRange> rangeList = new System.Collections.Generic.List<IRange>();
    IList<IWorksheet> innerList = (IList<IWorksheet>) this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
    {
      IRange[] all = innerList[index].FindAll(findValue);
      if (all != null)
        rangeList.AddRange((IEnumerable<IRange>) all);
    }
    return rangeList.Count == 0 ? (IRange[]) null : rangeList.ToArray();
  }

  [CLSCompliant(false)]
  public IWorksheet Add(
    BiffReader reader,
    ExcelParseOptions options,
    bool bSkipParsing,
    Dictionary<int, int> hashNewXFormatIndexes,
    IDecryptor decryptor)
  {
    return this.Add((IWorksheet) this.AppImplementation.CreateWorksheet((object) this, reader, options, bSkipParsing, hashNewXFormatIndexes, decryptor));
  }

  public IWorksheet Add(string sheetName)
  {
    IWorksheet worksheet = (IWorksheet) this.AppImplementation.CreateWorksheet((object) this);
    ((WorksheetBaseImpl) worksheet).RealIndex = this.m_book.ObjectCount;
    worksheet.Name = sheetName;
    return this.Add(worksheet);
  }

  public IWorksheet AddCopy(int sheetIndex)
  {
    return this.AddCopy(sheetIndex, ExcelWorksheetCopyFlags.CopyAll);
  }

  public IWorksheet AddCopy(int sheetIndex, ExcelWorksheetCopyFlags flags)
  {
    return this.AddCopy(this[sheetIndex], flags, true);
  }

  public IWorksheet AddCopy(IWorksheet sheet)
  {
    return this.AddCopy(sheet, ExcelWorksheetCopyFlags.CopyAll);
  }

  public IWorksheet AddCopy(IWorksheet sheet, ExcelWorksheetCopyFlags flags)
  {
    return this.AddCopy(sheet, flags, false);
  }

  internal string GenerateDefaultName(string sheetName)
  {
    return CollectionBaseEx<IWorksheet>.GenerateDefaultName((ICollection<IWorksheet>) this.List, sheetName + "_");
  }

  private IWorksheet AddCopy(IWorksheet sheet, ExcelWorksheetCopyFlags flags, bool isLocal)
  {
    WorksheetImpl worksheetImpl1 = (WorksheetImpl) sheet;
    WorksheetVisibility visibility = worksheetImpl1.Visibility;
    if (isLocal || sheet.Workbook.Worksheets == this)
    {
      WorksheetImpl worksheet = this.AppImplementation.CreateWorksheet((object) this);
      Dictionary<string, string> hashWorksheetNames = new Dictionary<string, string>(1);
      worksheet.Name = CollectionBaseEx<IWorksheet>.GenerateDefaultName((ICollection<IWorksheet>) this.List, worksheetImpl1.Name + "_");
      hashWorksheetNames.Add(worksheetImpl1.Name, worksheet.Name);
      this.Add((IWorksheet) worksheet);
      worksheet.StandardWidth = worksheetImpl1.StandardWidth;
      worksheet.StandardHeight = worksheetImpl1.StandardHeight;
      worksheet.BaseColumnWidth = worksheetImpl1.BaseColumnWidth;
      worksheet.DefaultColumnWidth = worksheetImpl1.DefaultColumnWidth;
      worksheet.DefaultRowHeight = worksheetImpl1.DefaultRowHeight;
      worksheet.CopyFrom(worksheetImpl1, new Dictionary<string, string>(), hashWorksheetNames, (Dictionary<int, int>) null, flags);
      return (IWorksheet) worksheet;
    }
    int version1 = (int) sheet.Workbook.Version;
    ExcelVersion version2 = worksheetImpl1.Version;
    if ((worksheetImpl1.Workbook as WorkbookImpl).IsConverted && this.m_book.IsCreated && !this.m_book.IsConverted)
      worksheetImpl1.Workbook.Version = ExcelVersion.Excel97to2003;
    Dictionary<int, int> dicFontIndexes;
    Dictionary<int, int> hashExtFormatIndexes;
    Dictionary<string, string> hashStyleNames = this.m_book.InnerStyles.Merge(sheet.Workbook, ExcelStyleMergeOptions.CreateDiffName, out dicFontIndexes, out hashExtFormatIndexes);
    WorkbookImpl workbook = (WorkbookImpl) worksheetImpl1.Workbook;
    if ((flags & ExcelWorksheetCopyFlags.CopyPalette) != ExcelWorksheetCopyFlags.None)
      workbook.CopyPaletteColorTo(this.m_book);
    if (workbook.DefaultThemeVersion != null)
      this.m_book.DefaultThemeVersion = workbook.DefaultThemeVersion;
    Dictionary<string, string> hashWorksheetNames1 = new Dictionary<string, string>();
    Dictionary<int, int> hashNameIndexes = new Dictionary<int, int>();
    Dictionary<int, int> hashSubBooks = this.m_book.ExternWorkbooks.AddCopy(workbook.ExternWorkbooks);
    Dictionary<int, int> hashExternSheets = this.m_book.CopyExternSheets(workbook.ExternSheet, hashSubBooks);
    WorksheetImpl worksheetImpl2 = this.AddWorksheet(sheet.Name, hashWorksheetNames1);
    worksheetImpl2.CopyStyles(sheet, hashExtFormatIndexes);
    worksheetImpl2.CopyFrom(worksheetImpl1, hashStyleNames, hashWorksheetNames1, dicFontIndexes, flags, hashExtFormatIndexes, hashNameIndexes, hashExternSheets);
    if (flags == ExcelWorksheetCopyFlags.CopyShapes)
      this.CopyControlsData(worksheetImpl1, flags);
    WorksheetsCollection.CopyOleObjects(workbook, worksheetImpl2.Workbook as WorkbookImpl);
    ExcelVersion version3 = worksheetImpl2.Version;
    ExcelVersion version4 = this.m_book.Version;
    if (version3 != version4)
      worksheetImpl2.Version = version4;
    if (visibility != worksheetImpl2.Visibility)
      worksheetImpl2.Visibility = visibility;
    if (sheet.Workbook.Version != version2)
      sheet.Workbook.Version = version2;
    return (IWorksheet) worksheetImpl2;
  }

  private static void CopyOleObjects(WorkbookImpl sourceBook, WorkbookImpl destBook)
  {
    if (!sourceBook.HasOleObjects)
      return;
    OleStorageCollection storageCollection1 = sourceBook.OleStorageCollection;
    OleStorageCollection storageCollection2 = destBook.OleStorageCollection;
    foreach (string oleStoragesName in storageCollection1.OleStoragesNames)
    {
      if (!storageCollection2.OleStoragesNames.Contains(oleStoragesName))
      {
        OleStorage storage = storageCollection1.OpenStorage(oleStoragesName);
        storageCollection2.Add(storage);
        destBook.HasOleObjects = true;
        destBook.IsOleObjectCopied = true;
      }
    }
    foreach (string arrayStreamName in storageCollection1.ArrayStreamNames)
    {
      if (!storageCollection2.ArrayStreamNames.Contains(arrayStreamName))
      {
        MemoryStream stream = storageCollection1.OpenStream(arrayStreamName);
        storageCollection2.Add(arrayStreamName, stream);
      }
    }
  }

  private void CopyControlsData(WorksheetImpl oldSheet, ExcelWorksheetCopyFlags flags)
  {
    if ((flags & ExcelWorksheetCopyFlags.CopyShapes) == ExcelWorksheetCopyFlags.None)
      return;
    WorkbookImpl workbook = oldSheet.Workbook as WorkbookImpl;
    if (!this.ContainsActiveX((IWorksheet) oldSheet))
      return;
    Stream controlsStream1 = workbook.ControlsStream;
    if (controlsStream1 == null)
      return;
    Stream controlsStream2 = this.m_book.ControlsStream;
    if (controlsStream2 != null)
    {
      controlsStream1.Position = 0L;
      controlsStream2.Position = controlsStream2.Length;
      UtilityMethods.CopyStreamTo(controlsStream1, controlsStream2);
    }
    else
      this.m_book.ControlsStream = controlsStream1;
  }

  private bool ContainsActiveX(IWorksheet sheet)
  {
    foreach (ShapeImpl shape in (CollectionBase<IShape>) (sheet.Shapes as ShapesCollection))
    {
      if (shape.IsActiveX)
        return true;
    }
    return false;
  }

  public void AddCopy(IWorksheets worksheets)
  {
    this.AddCopy(worksheets, ExcelWorksheetCopyFlags.CopyAll);
  }

  public void AddCopy(IWorksheets worksheets, ExcelWorksheetCopyFlags flags)
  {
    int length = worksheets != null ? worksheets.Count : throw new ArgumentNullException(nameof (worksheets));
    if (length == 0)
      return;
    WorkbookImpl book = ((WorksheetsCollection) worksheets).m_book;
    if (worksheets == this)
    {
      for (int sheetIndex = 0; sheetIndex < length; ++sheetIndex)
        this.AddCopy(sheetIndex);
    }
    else
    {
      WorksheetImpl[] worksheetImplArray = new WorksheetImpl[length];
      Dictionary<string, string> hashWorksheetNames = new Dictionary<string, string>();
      Dictionary<int, int> hashNameIndexes = new Dictionary<int, int>();
      Dictionary<int, int> hashSubBooks = this.m_book.ExternWorkbooks.AddCopy(book.ExternWorkbooks);
      Dictionary<int, int> hashExternSheets = this.m_book.CopyExternSheets(book.ExternSheet, hashSubBooks);
      Dictionary<int, int> dicFontIndexes;
      Dictionary<int, int> hashExtFormatIndexes;
      Dictionary<string, string> hashStyleNames = this.m_book.InnerStyles.Merge(worksheets[0].Workbook, ExcelStyleMergeOptions.CreateDiffName, out dicFontIndexes, out hashExtFormatIndexes);
      for (int Index = 0; Index < length; ++Index)
        worksheetImplArray[Index] = this.AddWorksheet(worksheets[Index].Name, hashWorksheetNames);
      if ((flags & ExcelWorksheetCopyFlags.CopyNames) != ExcelWorksheetCopyFlags.None)
      {
        for (int Index = 0; Index < length; ++Index)
          worksheetImplArray[Index].CopyFrom((WorksheetImpl) worksheets[Index], hashStyleNames, hashWorksheetNames, dicFontIndexes, ExcelWorksheetCopyFlags.CopyNames, hashExtFormatIndexes, hashNameIndexes, hashExternSheets);
        flags &= ~ExcelWorksheetCopyFlags.CopyNames;
      }
      for (int Index = 0; Index < length; ++Index)
      {
        worksheetImplArray[Index].CopyStyles(worksheets[Index], hashExtFormatIndexes);
        worksheetImplArray[Index].CopyFrom((WorksheetImpl) worksheets[Index], hashStyleNames, hashWorksheetNames, dicFontIndexes, flags, hashExtFormatIndexes, hashNameIndexes, hashExternSheets);
      }
      this.CopyConnections(book, this.m_book);
      WorksheetsCollection.CopyOleObjects(book, this.m_book);
    }
  }

  private void CopyConnections(WorkbookImpl sourceBook, WorkbookImpl destBook)
  {
    ExternalConnectionCollection connections1 = (ExternalConnectionCollection) sourceBook.Connections;
    ExternalConnectionCollection connections2 = (ExternalConnectionCollection) destBook.Connections;
    for (int i = 0; i < connections1.Count; ++i)
    {
      ExternalConnection externalConnection = (ExternalConnection) connections1[i];
      externalConnection.Clone(destBook, externalConnection.Name);
    }
    ExternalConnectionCollection deletedConnections1 = (ExternalConnectionCollection) sourceBook.DeletedConnections;
    ExternalConnectionCollection deletedConnections2 = (ExternalConnectionCollection) destBook.DeletedConnections;
    for (int i = 0; i < deletedConnections1.Count; ++i)
    {
      ExternalConnection externalConnection = (ExternalConnection) deletedConnections1[i];
      externalConnection.Clone(destBook, externalConnection.Name);
    }
  }

  private WorksheetImpl AddWorksheet(
    string strSuggestedName,
    Dictionary<string, string> hashWorksheetNames)
  {
    WorksheetImpl worksheet = this.AppImplementation.CreateWorksheet((object) this);
    string str = strSuggestedName;
    if (hashWorksheetNames == null)
      hashWorksheetNames = new Dictionary<string, string>();
    if (this[strSuggestedName] != null)
      str = CollectionBaseEx<IWorksheet>.GenerateDefaultName((ICollection<IWorksheet>) this.List, str + "_");
    hashWorksheetNames.Add(strSuggestedName, str);
    worksheet.Name = str;
    this.Add((IWorksheet) worksheet);
    return worksheet;
  }

  public IWorksheet AddCopyBefore(IWorksheet toCopy) => this.AddCopyBefore(toCopy, toCopy);

  public IWorksheet AddCopyBefore(IWorksheet toCopy, IWorksheet sheetAfter)
  {
    if (toCopy == null)
      throw new ArgumentNullException(nameof (toCopy));
    int iNewIndex = sheetAfter != null ? sheetAfter.Index : throw new ArgumentNullException(nameof (sheetAfter));
    IWorksheet worksheet = this.AddCopy(toCopy);
    worksheet.Move(iNewIndex);
    worksheet.Activate();
    return worksheet;
  }

  public IWorksheet AddCopyAfter(IWorksheet toCopy) => this.AddCopyAfter(toCopy, toCopy);

  public IWorksheet AddCopyAfter(IWorksheet toCopy, IWorksheet sheetBefore)
  {
    if (toCopy == null)
      throw new ArgumentNullException(nameof (toCopy));
    int num = sheetBefore != null ? sheetBefore.Index : throw new ArgumentNullException(nameof (sheetBefore));
    IWorksheet worksheet = this.AddCopy(toCopy);
    worksheet.Move(num + 1);
    worksheet.Activate();
    return worksheet;
  }

  protected override void OnInsertComplete(int index, IWorksheet value)
  {
    (value as WorksheetImpl).NameChanged += new ValueChangedEventHandler(this.sheet_NameChanged);
    if (this.m_bUseHash)
      this.m_list[value.Name] = value;
    base.OnInsertComplete(index, value);
  }

  protected override void OnSetComplete(int index, IWorksheet oldValue, IWorksheet newValue)
  {
    (oldValue as WorksheetImpl).NameChanged -= new ValueChangedEventHandler(this.sheet_NameChanged);
    if (this.m_bUseHash)
    {
      this.m_list.Remove(oldValue.Name);
      this.m_list[newValue.Name] = newValue;
    }
    base.OnSetComplete(index, oldValue, newValue);
  }

  protected override void OnRemoveComplete(int index, IWorksheet value)
  {
    (value as WorksheetImpl).NameChanged -= new ValueChangedEventHandler(this.sheet_NameChanged);
    if (this.m_bUseHash)
      this.m_list.Remove(value.Name);
    base.OnRemoveComplete(index, value);
  }

  protected override void OnClearComplete()
  {
    base.OnClearComplete();
    this.m_list.Clear();
  }

  private void sheet_NameChanged(object sender, ValueChangedEventArgs e)
  {
    string newValue = (string) e.newValue;
    string oldValue = (string) e.oldValue;
    if (!this.m_bUseHash)
      return;
    if (oldValue.ToUpper() != newValue.ToUpper() && this.m_list.ContainsKey(newValue))
      throw new NotSupportedException("Name of worksheet must be unique in a workbook.");
    this.m_list.Remove(oldValue);
    this.m_list[newValue] = (IWorksheet) sender;
  }

  public IWorksheet Create(string name)
  {
    if (name == null)
      throw new ArgumentNullException(nameof (name));
    if (name.Length < 1 || WorksheetsCollection.ValidateSheetName(name))
      throw new ArgumentException("Sheet Name is InValid");
    if (name.Length > 31 /*0x1F*/)
    {
      name = name.Substring(0, 31 /*0x1F*/);
      IWorksheet worksheet = this[name];
      if (worksheet != null)
        return worksheet;
    }
    IWorksheet worksheet1 = (IWorksheet) this.AppImplementation.CreateWorksheet((object) this);
    worksheet1.Name = name;
    if (worksheet1.Workbook.Worksheets.Count == 0)
      worksheet1.Activate();
    return this.Add(worksheet1);
  }

  public IWorksheet Create()
  {
    IWorksheet worksheet = (IWorksheet) this.AppImplementation.CreateWorksheet((object) this);
    int count = this.InnerList.Count;
    if (count == 0)
      ++count;
    string sheetName;
    for (sheetName = "Sheet" + (object) count; this[sheetName] != null; sheetName = "Sheet" + (object) count)
      ++count;
    worksheet.Name = sheetName;
    if (worksheet.Workbook.Worksheets.Count == 0)
      worksheet.Activate();
    return this.Add(worksheet);
  }

  public void Remove(IWorksheet sheet)
  {
    if (!this.InnerList.Contains(sheet))
      throw new ArgumentOutOfRangeException("Worksheets collection does not contain specified worksheet.");
    if (this.m_book.Objects.Count == 1)
      throw new ArgumentException("Workbook must contains at least one worksheet. You cannot remove last worksheet.", nameof (sheet));
    sheet.Remove();
  }

  public void Remove(string sheetName) => this.Remove(this[sheetName]);

  public void Remove(int index) => this.Remove(this[index]);

  public void UpdateStringIndexes(System.Collections.Generic.List<int> arrNewIndexes)
  {
    System.Collections.Generic.List<IWorksheet> innerList = this.InnerList;
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      ((WorksheetImpl) innerList[index]).UpdateStringIndexes(arrNewIndexes);
  }

  public void InnerRemove(int index)
  {
    int count1 = this.Count;
    if (index < 0 || index > count1 - 1)
      throw new ArgumentOutOfRangeException(nameof (index), "Value cannot be less than 0 and greater than Count - 1.");
    if (this.m_book.Objects.Count == 1)
      throw new ArgumentException("Workbook at least must contains one worksheet. You cannot remove last worksheet.", "sheet");
    IWorksheet worksheet = this[index];
    int realIndex = ((ISerializableNamedObject) worksheet).RealIndex;
    this.RemoveAt(index);
    WorkbookObjectsCollection objects = this.m_book.Objects;
    objects.RemoveAt(realIndex);
    int index1 = realIndex;
    for (int count2 = objects.Count; index1 < count2; ++index1)
      objects[index1].RealIndex = index1;
    if (this.m_book.ActiveSheet == worksheet)
      this.m_book.SetActiveWorksheet(this[0] as WorksheetBaseImpl);
    for (int index2 = index + 1; index2 < count1; ++index2)
      ((WorksheetBaseImpl) this[index2 - 1]).Index = index2 - 1;
  }

  public void InnerAdd(IWorksheet sheet)
  {
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    base.Add(sheet);
  }

  private static bool ValidateSheetName(string sheetName)
  {
    foreach (string sheetNameValidator in WorksheetsCollection.sheetNameValidators)
    {
      if (sheetName.Contains(sheetNameValidator))
        return true;
    }
    return false;
  }

  public bool IsRightToLeft
  {
    get
    {
      System.Collections.Generic.List<IWorksheet> innerList = this.InnerList;
      bool isRightToLeft = innerList[0].IsRightToLeft;
      int index = 1;
      for (int count = innerList.Count; index < count; ++index)
      {
        if (innerList[index].IsRightToLeft != isRightToLeft || !isRightToLeft)
          return false;
      }
      return isRightToLeft;
    }
    set
    {
      System.Collections.Generic.List<IWorksheet> innerList = this.InnerList;
      int index = 0;
      for (int count = innerList.Count; index < count; ++index)
        innerList[index].IsRightToLeft = value;
    }
  }

  private void Objects_TabSheetMoved(object sender, TabSheetMovedEventArgs args)
  {
    if (!(((ITabSheets) sender)[args.NewIndex] is WorksheetImpl sheet))
      return;
    int oldIndex = args.OldIndex;
    this.UpdateSheetIndex(sheet, oldIndex);
  }
}
