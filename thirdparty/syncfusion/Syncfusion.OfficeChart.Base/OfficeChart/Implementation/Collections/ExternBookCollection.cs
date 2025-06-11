// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Collections.ExternBookCollection
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Parser.Biff_Records;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Collections;

internal class ExternBookCollection : CollectionBaseEx<ExternWorkbookImpl>
{
  private const int StdDocumentOptions = 32746;
  private const int DEF_NO_SHEET_INDEX = 65534;
  internal const string DEF_WRONG_URL_NAME = " ";
  private WorkbookImpl m_book;
  private Dictionary<string, ExternWorkbookImpl> m_hashUrlToBook = new Dictionary<string, ExternWorkbookImpl>();
  private Dictionary<string, ExternWorkbookImpl> m_hashShortNameToBook = new Dictionary<string, ExternWorkbookImpl>();

  public ExternBookCollection(IApplication application, object parent)
    : base(application, parent)
  {
    this.SetParents();
  }

  public new ExternWorkbookImpl this[int index]
  {
    get
    {
      return index >= 0 && index <= this.Count ? this.List[index] : throw new ArgumentOutOfRangeException(nameof (index), "Value cannot be less than 0 and greater than Count");
    }
  }

  public ExternWorkbookImpl this[string strUrl]
  {
    get
    {
      if (strUrl == null || strUrl.Length == 0)
        return (ExternWorkbookImpl) null;
      ExternWorkbookImpl externWorkbookImpl;
      this.m_hashUrlToBook.TryGetValue(strUrl, out externWorkbookImpl);
      return externWorkbookImpl;
    }
  }

  public WorkbookImpl ParentWorkbook => this.m_book;

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      this.List[index].Serialize(records);
  }

  public int Add(ExternWorkbookImpl book)
  {
    book.Index = this.List.Count;
    base.Add(book);
    return this.Count - 1;
  }

  public int Add(string fileName) => this.Add(fileName, false);

  public int Add(string fileName, bool bAddInFunctions)
  {
    ExternWorkbookImpl book = new ExternWorkbookImpl(this.Application, (object) this);
    book.IsInternalReference = false;
    book.IsAddInFunctions = true;
    book.URL = fileName != null ? Path.GetFullPath(fileName) : fileName;
    int supIndex = this.Add(book);
    int sheetNumber = book.SheetNumber;
    int firstSheetIndex = sheetNumber == 0 ? 65534 : 0;
    int lastSheetIndex = sheetNumber == 0 ? 65534 : 0;
    this.m_book.AddSheetReference(supIndex, firstSheetIndex, lastSheetIndex);
    return supIndex;
  }

  public int Add(string filePath, string fileName, System.Collections.Generic.List<string> sheets, string[] names)
  {
    if (fileName == null || fileName.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (fileName));
    ExternWorkbookImpl book = new ExternWorkbookImpl(this.Application, (object) this);
    book.IsInternalReference = false;
    book.URL = filePath == null ? fileName : filePath + fileName;
    int num = this.Add(book);
    int count = sheets != null ? sheets.Count : 0;
    book.SheetNumber = count;
    book.AddWorksheets(sheets);
    book.AddNames(names);
    return num;
  }

  public int AddDDEFile(string fileName)
  {
    ExternWorkbookImpl book = new ExternWorkbookImpl(this.Application, (object) this);
    book.IsInternalReference = false;
    book.URL = fileName;
    int supIndex = this.Add(book);
    book.SheetNumber = 0;
    int firstSheetIndex = 65534;
    int lastSheetIndex = 65534;
    this.m_book.AddSheetReference(supIndex, firstSheetIndex, lastSheetIndex);
    ExternNamesCollection externNames = book.ExternNames;
    int index = externNames.Add("StdDocument");
    externNames[index].Record.Options = (ushort) 32746;
    return supIndex;
  }

  public int InsertSelfSupbook()
  {
    int index = 0;
    for (int count = this.Count; index < count; ++index)
    {
      ExternWorkbookImpl externWorkbookImpl = this[index];
    }
    base.Add(new ExternWorkbookImpl(this.Application, (object) this)
    {
      Index = this.List.Count,
      IsInternalReference = true
    });
    return this.Count - 1;
  }

  public bool ContainsExternName(string strName)
  {
    int index = 0;
    for (int count = this.Count; index < count; ++index)
    {
      if (this[index].ExternNames.Contains(strName))
        return true;
    }
    return false;
  }

  public bool ContainsExternName(string strName, ref int iBookIndex, ref int iNameIndex)
  {
    int index = 0;
    for (int count = this.Count; index < count; ++index)
    {
      ExternWorkbookImpl externWorkbookImpl = this[index];
      iNameIndex = externWorkbookImpl.ExternNames.GetNameIndex(strName);
      if (iNameIndex >= 0)
      {
        iBookIndex = this.m_book.AddSheetReference(externWorkbookImpl.Index, 65534, 65534);
        return true;
      }
    }
    return false;
  }

  public int GetNameIndexes(string strName, out int iRefIndex)
  {
    iRefIndex = -1;
    int index = 0;
    for (int count = this.Count; index < count; ++index)
    {
      int nameIndex = this[index].ExternNames.GetNameIndex(strName);
      if (nameIndex != -1)
      {
        iRefIndex = nameIndex;
        return index;
      }
    }
    return -1;
  }

  public ExternWorkbookImpl GetBookByShortName(string strShortName)
  {
    switch (strShortName)
    {
      case null:
        throw new ArgumentNullException(nameof (strShortName));
      case "":
        throw new ArgumentException("strShortName - string cannot be empty");
      default:
        ExternWorkbookImpl bookByShortName;
        this.m_hashShortNameToBook.TryGetValue(strShortName, out bookByShortName);
        return bookByShortName;
    }
  }

  private void SetParents()
  {
    this.m_book = this.FindParent(typeof (WorkbookImpl)) as WorkbookImpl;
    if (this.m_book == null)
      throw new ArgumentNullException("m_book");
  }

  public int GetFirstInternalIndex()
  {
    int index = 0;
    for (int count = this.List.Count; index < count; ++index)
    {
      if (this.List[index].IsInternalReference)
        return index;
    }
    return -1;
  }

  protected override void OnInsertComplete(int index, ExternWorkbookImpl value)
  {
    base.OnInsertComplete(index, value);
    ExternWorkbookImpl externWorkbookImpl = value;
    externWorkbookImpl.Index = this.List.Count - 1;
    if (externWorkbookImpl.IsInternalReference)
      return;
    string url = externWorkbookImpl.URL;
    if (url == null || !(url != " "))
      return;
    if (!this.m_hashUrlToBook.ContainsKey(url) || !this.m_book.IsWorkbookOpening)
      this.m_hashUrlToBook.Add(url, externWorkbookImpl);
    string shortName = externWorkbookImpl.ShortName;
    if (this.m_hashShortNameToBook.ContainsKey(shortName))
      return;
    this.m_hashShortNameToBook.Add(shortName, externWorkbookImpl);
  }

  public Dictionary<int, int> AddCopy(ExternBookCollection subBooks)
  {
    if (subBooks == null)
      throw new ArgumentNullException(nameof (subBooks));
    Dictionary<int, int> dictionary = new Dictionary<int, int>();
    int firstInternalIndex = this.GetFirstInternalIndex();
    int num1 = 0;
    for (int count = subBooks.Count; num1 < count; ++num1)
    {
      ExternWorkbookImpl subBook = subBooks[num1];
      ExternWorkbookImpl externWorkbookImpl = this[subBook.URL];
      int num2 = !subBook.IsInternalReference || firstInternalIndex < 0 ? (externWorkbookImpl != null ? externWorkbookImpl.Index : this.Add((ExternWorkbookImpl) subBook.Clone((object) this))) : firstInternalIndex;
      dictionary.Add(num1, num2);
    }
    return dictionary;
  }

  internal ExternWorkbookImpl FindOrAdd(string strBook, string strBookPath)
  {
    if (strBook == null || strBook.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (strBook));
    string key = strBookPath == null ? strBook : strBookPath + strBook;
    return !this.m_hashUrlToBook.ContainsKey(key) ? (strBook != null && strBook.Length != 0 || !this.m_hashShortNameToBook.ContainsKey(strBook) ? this[this.Add(strBookPath, strBook, (System.Collections.Generic.List<string>) null, (string[]) null)] : this.m_hashShortNameToBook[strBook]) : this.m_hashUrlToBook[key];
  }

  internal void Dispose()
  {
    for (int index = this.Count - 1; index >= 0; --index)
      this[index].Dispose();
    this.Clear();
  }

  internal int Add(string fileName, WorkbookImpl book, IRange sourceRange)
  {
    ExternWorkbookImpl book1 = new ExternWorkbookImpl(this.Application, (object) this);
    book1.IsInternalReference = false;
    book1.IsAddInFunctions = false;
    book1.URL = fileName != null ? Path.GetFullPath(fileName) : fileName;
    int supIndex = this.Add(book1);
    IWorksheets worksheets = sourceRange.Worksheet.Workbook.Worksheets;
    ExternWorksheetImpl externWorksheetImpl1 = (ExternWorksheetImpl) null;
    string name1 = sourceRange.Worksheet.Name;
    int Index = 0;
    for (int count = worksheets.Count; Index < count; ++Index)
    {
      string name2 = worksheets[Index].Name;
      ExternWorksheetImpl externWorksheetImpl2 = book1.AddWorksheet(name2);
      if (name2 == name1)
        externWorksheetImpl1 = externWorksheetImpl2;
    }
    int sheetNumber = book1.SheetNumber;
    int firstSheetIndex = sheetNumber == 0 ? 65534 : externWorksheetImpl1.Index;
    int lastSheetIndex = sheetNumber == 0 ? 65534 : externWorksheetImpl1.Index;
    externWorksheetImpl1.CacheValues(sourceRange);
    this.m_book.AddSheetReference(supIndex, firstSheetIndex, lastSheetIndex);
    return supIndex;
  }
}
