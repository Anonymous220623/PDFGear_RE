// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.ExternWorkbookImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.Security;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class ExternWorkbookImpl : CommonObject, ICloneParent
{
  private SortedList<int, ExternWorksheetImpl> m_arrSheets = new SortedList<int, ExternWorksheetImpl>();
  private Dictionary<string, ExternWorksheetImpl> m_hashNameToSheet = new Dictionary<string, ExternWorksheetImpl>();
  private ExternNamesCollection m_externNames;
  private SupBookRecord m_supBook;
  private int m_iIndex;
  private bool m_isDdeLink;
  private WorkbookImpl m_book;
  private string m_strShortName;
  private string m_strProgramId;
  private bool m_isParsed;

  public ExternWorkbookImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.InitializeVariables();
  }

  private void InitializeVariables()
  {
    this.FindParents();
    this.m_externNames = new ExternNamesCollection(this.Application, (object) this);
    this.m_supBook = (SupBookRecord) BiffRecordFactory.GetRecord(TBIFFRecord.SupBook);
    this.m_supBook.SheetNames = new List<string>();
    this.InitShortName();
  }

  public void InsertDefaultWorksheet()
  {
    this.m_supBook.SheetNames.Add("Sheet1");
    this.m_arrSheets.Add(0, new ExternWorksheetImpl(this.Application, this)
    {
      Index = 0
    });
  }

  private void FindParents()
  {
    this.m_book = this.FindParent(typeof (WorkbookImpl)) as WorkbookImpl;
    if (this.m_book == null)
      throw new ArgumentNullException("m_book");
    this.m_supBook = (SupBookRecord) CloneUtils.CloneCloneable((ICloneable) this.m_supBook);
  }

  [CLSCompliant(false)]
  public int Parse(BiffRecordRaw[] arrData, int iOffset) => throw new NotImplementedException();

  [CLSCompliant(false)]
  public void Parse(BiffReader reader, IDecryptor decryptor)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    this.m_arrSheets.Clear();
    this.m_externNames.Clear();
    BiffRecordRaw record1 = reader.GetRecord(decryptor);
    record1.CheckTypeCode(TBIFFRecord.SupBook);
    this.m_supBook = (SupBookRecord) record1;
    string url = this.m_supBook.URL;
    if (!this.m_supBook.IsInternalReference && url != null & url != string.Empty)
      this.m_supBook.URL = this.m_book.DecodeName(url);
    int sheetNumber = (int) this.m_supBook.SheetNumber;
    TBIFFRecord tbiffRecord = reader.PeekRecordType();
    int iNameIndex = 0;
    for (; tbiffRecord == TBIFFRecord.ExternName; tbiffRecord = reader.PeekRecordType())
    {
      ExternNameRecord record2 = (ExternNameRecord) reader.GetRecord(decryptor);
      this.m_externNames.Add(record2);
      if (record2.FormulaSize == (ushort) 0 || this.m_supBook.IsAddInFunctions)
        this.m_book.InnerAddInFunctions.Add(this.m_iIndex, iNameIndex);
      ++iNameIndex;
    }
    List<string> sheetNames = this.m_supBook.SheetNames;
    if (this.m_arrSheets.Count == 0 && sheetNumber > 0 && sheetNames != null)
    {
      int index = 0;
      for (int count = sheetNames.Count; index < count; ++index)
        this.AddExternSheet(new ExternWorksheetImpl(this.Application, this)
        {
          Name = sheetNames[index],
          Index = index
        });
    }
    for (; tbiffRecord == TBIFFRecord.XCT; tbiffRecord = reader.PeekRecordType())
    {
      ExternWorksheetImpl sheet = new ExternWorksheetImpl(this.Application, this);
      sheet.Parse(reader, decryptor);
      this.AddExternSheet(sheet);
      if (sheet.Name == null && sheetNames.Count == this.m_arrSheets.Count && sheet.Index < sheetNames.Count)
        sheet.Name = sheetNames[sheet.Index];
    }
    this.InitShortName();
  }

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    SupBookRecord supBookRecord = this.m_supBook;
    if (!this.m_supBook.IsInternalReference && supBookRecord.URL != null)
    {
      supBookRecord = (SupBookRecord) this.m_supBook.Clone();
      supBookRecord.URL = this.m_book.EncodeName(this.m_supBook.URL);
    }
    records.Add((IBiffStorage) supBookRecord);
    this.m_externNames.Serialize(records);
    if (this.IsInternalReference)
      return;
    IList<ExternWorksheetImpl> values = this.m_arrSheets.Values;
    int index = 0;
    for (int count = this.m_arrSheets.Count; index < count; ++index)
      values[index].Serialize(records);
  }

  private void AddExternSheet(ExternWorksheetImpl sheet)
  {
    int num = sheet != null ? sheet.Index : throw new ArgumentNullException(nameof (sheet));
    this.m_arrSheets[num] = sheet;
    int count = this.m_supBook.SheetNames.Count;
    if (num >= count)
      return;
    this.m_hashNameToSheet[this.m_supBook.SheetNames[num]] = sheet;
  }

  public ExternNamesCollection ExternNames => this.m_externNames;

  public bool IsInternalReference
  {
    get => this.m_supBook.IsInternalReference;
    set => this.m_supBook.IsInternalReference = value;
  }

  public bool IsOleLink
  {
    get
    {
      return this.m_externNames != null && this.m_externNames.Count == 1 && this.m_externNames[0].Record.OleLink;
    }
  }

  public int SheetNumber
  {
    get => (int) this.m_supBook.SheetNumber;
    set => this.m_supBook.SheetNumber = (ushort) value;
  }

  public string URL
  {
    get => this.m_supBook.URL;
    set
    {
      string url = this.m_supBook.URL;
      this.m_supBook.URL = value;
      this.InitShortName();
      if (value != null && !(url != value))
        return;
      this.m_arrSheets.Clear();
      this.m_hashNameToSheet.Clear();
      if (value != null)
        return;
      this.m_supBook.SheetNames = (List<string>) null;
    }
  }

  public int Index
  {
    get => this.m_iIndex;
    set => this.m_iIndex = value;
  }

  public WorkbookImpl Workbook => this.m_book;

  internal bool IsDdeLink
  {
    get => this.m_isDdeLink;
    set => this.m_isDdeLink = value;
  }

  public string ShortName => this.m_strShortName;

  public bool IsAddInFunctions
  {
    get => this.m_supBook.IsAddInFunctions;
    set => this.m_supBook.IsAddInFunctions = value;
  }

  public SortedList<int, ExternWorksheetImpl> Worksheets => this.m_arrSheets;

  public string ProgramId
  {
    get => this.m_strProgramId;
    set => this.m_strProgramId = value;
  }

  internal bool IsParsed
  {
    get => this.m_isParsed;
    set => this.m_isParsed = value;
  }

  public int IndexOf(string strSheetName)
  {
    ExternWorksheetImpl externWorksheetImpl;
    return strSheetName == null || strSheetName.Length == 0 || !this.m_hashNameToSheet.TryGetValue(strSheetName, out externWorksheetImpl) ? -1 : externWorksheetImpl.Index;
  }

  public void saveAsHtml(string FileName)
  {
  }

  public int GetNewIndex(int iNameIndex) => this.m_externNames.GetNewIndex(iNameIndex);

  public object Clone(object parent)
  {
    ExternWorkbookImpl parent1 = (ExternWorkbookImpl) this.MemberwiseClone();
    parent1.SetParent(parent);
    parent1.FindParents();
    parent1.m_arrSheets = new SortedList<int, ExternWorksheetImpl>();
    IList<int> keys = this.m_arrSheets.Keys;
    IList<ExternWorksheetImpl> values = this.m_arrSheets.Values;
    int index = 0;
    for (int count = this.m_arrSheets.Count; index < count; ++index)
    {
      int num = keys[index];
      ExternWorksheetImpl sheet = values[index].Clone((object) parent1);
      parent1.AddExternSheet(sheet);
    }
    parent1.m_hashNameToSheet = new Dictionary<string, ExternWorksheetImpl>();
    foreach (KeyValuePair<string, ExternWorksheetImpl> keyValuePair in this.m_hashNameToSheet)
      parent1.m_hashNameToSheet.Add(keyValuePair.Key, keyValuePair.Value.Clone((object) parent1));
    parent1.m_externNames = (ExternNamesCollection) this.m_externNames.Clone((object) this);
    parent1.m_supBook = (SupBookRecord) this.m_supBook.Clone();
    return (object) parent1;
  }

  public string GetSheetName(int index)
  {
    if (index == (int) ushort.MaxValue)
      return "#REF";
    return index == 65534 ? string.Empty : this.m_supBook.SheetNames[index];
  }

  private void InitShortName()
  {
    this.m_strShortName = this.m_supBook.URL != null ? ExternWorkbookImpl.GetFileName(this.m_supBook.URL) : (string) null;
  }

  private static string GetFileName(string strUrl)
  {
    if (strUrl == null || strUrl.Length == 0)
      return strUrl;
    int num = strUrl.LastIndexOf('\\');
    int length = strUrl.Length;
    int startIndex = 0;
    if (num > 0)
      startIndex = num + 1;
    return strUrl.Substring(startIndex, length - startIndex);
  }

  private static string GetFileNameWithoutExtension(string strUrl)
  {
    if (strUrl == null || strUrl.Length == 0)
      return strUrl;
    int num1 = strUrl.LastIndexOf('\\');
    int num2 = strUrl.LastIndexOf('.');
    int startIndex = 0;
    int num3 = strUrl.Length;
    if (num1 > 0)
      startIndex = num1 + 1;
    if (num2 > startIndex)
      num3 = num2;
    return strUrl.Substring(startIndex, num3 - startIndex);
  }

  public void AddWorksheets(List<string> sheets)
  {
    int count = sheets != null ? sheets.Count : 0;
    if (count == 0)
      return;
    for (int index = 0; index < count; ++index)
      this.AddWorksheet(sheets[index]);
  }

  public void AddWorksheets(string[] sheets)
  {
    int length = sheets != null ? sheets.Length : 0;
    if (length == 0)
      return;
    for (int index = 0; index < length; ++index)
      this.AddWorksheet(sheets[index]);
  }

  public ExternWorksheetImpl AddWorksheet(string sheetName)
  {
    if (sheetName == null)
      throw new ArgumentOutOfRangeException(nameof (sheetName));
    ExternWorksheetImpl externWorksheetImpl = new ExternWorksheetImpl(this.Application, this);
    int count = this.m_arrSheets.Count;
    externWorksheetImpl.Index = count;
    externWorksheetImpl.Name = sheetName;
    this.m_arrSheets.Add(count, externWorksheetImpl);
    this.m_hashNameToSheet.Add(sheetName, externWorksheetImpl);
    this.m_supBook.SheetNames.Add(sheetName);
    return externWorksheetImpl;
  }

  public void AddNames(string[] names)
  {
    int length = names != null ? names.Length : 0;
    for (int index = 0; index < length; ++index)
      this.AddName(names[index]);
  }

  public void AddName(string name) => this.m_externNames.Add(name);

  internal int FindOrAddSheet(string sheetName)
  {
    if (sheetName == null || sheetName.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (sheetName));
    ExternWorksheetImpl externWorksheetImpl;
    if (!this.m_hashNameToSheet.TryGetValue(sheetName, out externWorksheetImpl))
      externWorksheetImpl = this.AddWorksheet(sheetName);
    return externWorksheetImpl.Index;
  }

  protected override void OnDispose()
  {
    if (this.m_bIsDisposed)
      return;
    if (this.m_arrSheets != null)
    {
      foreach (CommonObject commonObject in (IEnumerable<ExternWorksheetImpl>) this.m_arrSheets.Values)
        commonObject.Dispose();
      this.m_arrSheets.Clear();
      this.m_arrSheets = (SortedList<int, ExternWorksheetImpl>) null;
      if (this.m_hashNameToSheet != null)
      {
        this.m_hashNameToSheet.Clear();
        this.m_hashNameToSheet = (Dictionary<string, ExternWorksheetImpl>) null;
      }
    }
    base.OnDispose();
  }
}
