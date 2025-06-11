// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.SSTDictionary
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class SSTDictionary : IParseable, IDisposable
{
  private const int DEF_RESERVE_SPACE = 20;
  public const int DEF_EMPTY_STRING_INDEX = -1;
  private const int DEF_EMPTY_COUNT = 2;
  private const int MaxCellLength = 32767 /*0x7FFF*/;
  private Dictionary<object, int> m_hashKeyToIndex = new Dictionary<object, int>(20);
  internal List<object> m_arrStrings = new List<object>(20);
  private SortedList<int, int> m_arrFreeIndexes = new SortedList<int, int>(20);
  private WorkbookImpl m_book;
  private SSTRecord m_sstOriginal;
  private bool m_bParsed = true;
  private TextWithFormat m_tempString = new TextWithFormat(0);
  private List<int> newRefCount;
  private bool m_bUseHash = true;

  public int this[TextWithFormat key]
  {
    get
    {
      this.Parse();
      return this.Find(key);
    }
  }

  public TextWithFormat this[int index]
  {
    get
    {
      object sstContentByIndex = this.GetSSTContentByIndex(index);
      if (!(sstContentByIndex is TextWithFormat textWithFormat))
      {
        this.m_tempString.Text = sstContentByIndex.ToString();
        textWithFormat = this.m_tempString;
      }
      return textWithFormat;
    }
  }

  public object[] Keys
  {
    get
    {
      int count = this.Count;
      object[] keys = new object[count];
      for (int index = 0; index < count; ++index)
        keys[index] = this.m_arrStrings[index];
      return keys;
    }
  }

  public int Count
  {
    get
    {
      return !this.m_bParsed ? (int) this.m_sstOriginal.NumberOfUniqueStrings : this.m_arrStrings.Count;
    }
  }

  public WorkbookImpl Workbook => this.m_book;

  [CLSCompliant(false)]
  public SSTRecord OriginalSST
  {
    get => this.m_sstOriginal;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (OriginalSST));
      this.m_bParsed = false;
      this.m_sstOriginal = value;
      int numberOfUniqueStrings = (int) this.m_sstOriginal.NumberOfUniqueStrings;
    }
  }

  public bool UseHashForSearching
  {
    get => this.m_bUseHash;
    set
    {
      if (this.m_bUseHash == value)
        return;
      this.m_bUseHash = value;
      if (!value)
      {
        this.m_hashKeyToIndex.Clear();
      }
      else
      {
        if (!this.m_bParsed)
          return;
        this.FillHash();
      }
    }
  }

  public int ActiveCount
  {
    get
    {
      return !this.m_bParsed ? (int) this.m_sstOriginal.NumberOfUniqueStrings : this.m_arrStrings.Count - this.m_arrFreeIndexes.Count;
    }
  }

  internal Dictionary<object, int> HashKeyToIndex
  {
    get => this.m_hashKeyToIndex;
    set => this.m_hashKeyToIndex = value;
  }

  public SSTDictionary(WorkbookImpl book)
  {
    this.m_book = book;
    this.newRefCount = new List<int>();
  }

  public object GetSSTContentByIndex(int index)
  {
    if (index < 0 || index >= this.Count)
      throw new ArgumentOutOfRangeException(nameof (index));
    return this.m_bParsed ? this.m_arrStrings[index] : this.m_sstOriginal.Strings[index];
  }

  public void Clear()
  {
    if (this.m_book == null)
      return;
    if (this.m_bParsed)
    {
      this.m_arrStrings.Clear();
      this.m_hashKeyToIndex.Clear();
    }
    else
    {
      this.m_sstOriginal = (SSTRecord) null;
      this.m_bParsed = true;
    }
  }

  public Dictionary<int, object> GetStringIndexes(string value)
  {
    Dictionary<int, object> stringIndexes = new Dictionary<int, object>();
    object[] strings = this.m_bParsed ? (object[]) null : this.m_sstOriginal.Strings;
    int index = 0;
    for (int count = this.Count; index < count; ++index)
    {
      if (!this.m_arrFreeIndexes.ContainsKey(index))
      {
        object obj = this.m_bParsed ? this.m_arrStrings[index] : strings[index];
        if ((obj is TextWithFormat textWithFormat ? textWithFormat.Text : (string) obj).IndexOf(value, 0, StringComparison.CurrentCultureIgnoreCase) != -1)
          stringIndexes.Add(index, (object) null);
      }
    }
    return stringIndexes;
  }

  public void AddIncrease(int index)
  {
    if (index == -1)
      return;
    int refCount = this.GetRefCount(index);
    this.SetRefCount(index, refCount + 1);
  }

  public int AddIncrease(object key) => this.AddIncrease(key, true);

  public int AddIncrease(object key, bool bIncrease)
  {
    if (bIncrease)
      this.Parse();
    int count1;
    if (this.m_hashKeyToIndex.TryGetValue(key, out count1))
    {
      count1 = this.m_hashKeyToIndex[key];
      if (bIncrease)
        this.AddIncrease(count1);
      else
        this.m_arrStrings.Add(key);
    }
    else
    {
      this.CheckLength(key);
      int count2 = bIncrease ? 1 : 0;
      if (this.m_arrFreeIndexes.Count == 0)
      {
        count1 = this.m_arrStrings.Count;
        if (key.ToString().Length > (int) short.MaxValue && (this.m_book.Application.ExcludeAdditionalCharacters || !(this.m_book.Application as ApplicationImpl).m_isExplicitlySet))
          this.m_arrStrings.Add((object) key.ToString().Substring(0, (int) short.MaxValue));
        else
          this.m_arrStrings.Add(key);
        if (this.m_bUseHash)
          this.m_hashKeyToIndex[key] = count1;
        if (bIncrease)
          this.SetRefCount(count1, count2);
      }
      else
      {
        count1 = this.m_arrFreeIndexes.Values[0];
        this.m_arrStrings[count1] = key;
        if (this.m_bUseHash)
          this.m_hashKeyToIndex[key] = count1;
        this.m_arrFreeIndexes.RemoveAt(0);
        this.SetRefCount(count1, count2);
      }
    }
    return count1;
  }

  private void CheckLength(object key)
  {
    string str = key as string;
    int num = 0;
    if (str != null)
      num = str.Length;
    else if (key != null)
      num = (key as TextWithFormat).Text.Length;
    if (num > (int) short.MaxValue && !this.m_book.Application.ExcludeAdditionalCharacters && !(this.m_book.Application as ApplicationImpl).m_isExplicitlySet)
      throw new ArgumentOutOfRangeException("Text length cannot be more than " + (object) (int) short.MaxValue);
  }

  public void RemoveDecrease(object key)
  {
    this.Parse();
    int iIndex = this.Find(key);
    if (iIndex == -1)
      throw new ArgumentException($"Dictionary does not contain specified string: '{key}'");
    this.RemoveDecrease(iIndex);
  }

  public void RemoveDecrease(int iIndex)
  {
    if (iIndex < 0 || iIndex >= this.Count)
      throw new ArgumentOutOfRangeException(nameof (iIndex));
    int count = this.GetRefCount(iIndex) - 1;
    this.SetRefCount(iIndex, count);
    if (count > 0)
      return;
    this.Parse();
    if (this.m_bUseHash)
      this.m_hashKeyToIndex.Remove(this.m_arrStrings[iIndex]);
    this.m_arrFreeIndexes[iIndex] = iIndex;
    this.m_arrStrings[iIndex] = (object) 0;
  }

  public void DecreaseOnly(int index)
  {
    if (index < 0 || index > this.m_arrStrings.Count)
      throw new ArgumentOutOfRangeException(nameof (index));
    int refCount = this.GetRefCount(index);
    this.SetRefCount(index, refCount - 1);
  }

  public bool Contains(object key)
  {
    this.Parse();
    return this.Find(key) != -1;
  }

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records)
  {
    if (this.m_bParsed)
      this.Defragment();
    this.SaveIntoRecords(records);
  }

  public int GetStringCount(int index)
  {
    if (index == -1)
      return 2;
    this.Parse();
    return this.GetRefCount(index);
  }

  public TextWithFormat GetStringByIndex(int index)
  {
    return index != -1 ? this[index] : throw new NotImplementedException();
  }

  public int AddCopy(int index, SSTDictionary sourceSST, Dictionary<int, int> dicFontIndexes)
  {
    if (sourceSST == null)
      throw new ArgumentNullException(nameof (sourceSST));
    this.Parse();
    sourceSST.Parse();
    object key = sourceSST.m_arrStrings[index];
    if (key is TextWithFormat textWithFormat)
      key = (object) textWithFormat.Clone(dicFontIndexes);
    return this.AddIncrease(key, true);
  }

  public List<int> StartWith(string strStart)
  {
    switch (strStart)
    {
      case null:
        throw new ArgumentNullException(nameof (strStart));
      case "":
        throw new ArgumentException("strStart - string cannot be empty.");
      default:
        List<int> intList = new List<int>();
        int index = 0;
        for (int count = this.Count; index < count; ++index)
        {
          if (this[index].Text.StartsWith(strStart))
            intList.Add(index);
        }
        return intList;
    }
  }

  public object Clone(WorkbookImpl book)
  {
    SSTDictionary sstDictionary = (SSTDictionary) this.MemberwiseClone();
    sstDictionary.m_book = book;
    sstDictionary.m_hashKeyToIndex = CloneUtils.CloneHash(this.m_hashKeyToIndex);
    sstDictionary.m_arrStrings = CloneUtils.CloneCloneable<object>(this.m_arrStrings);
    if (this.m_arrFreeIndexes != null)
      sstDictionary.m_arrFreeIndexes = new SortedList<int, int>((IDictionary<int, int>) this.m_arrFreeIndexes);
    sstDictionary.m_sstOriginal = (SSTRecord) CloneUtils.CloneCloneable((ICloneable) this.m_sstOriginal);
    sstDictionary.newRefCount = CloneUtils.CloneCloneable<int>(this.newRefCount);
    return (object) sstDictionary;
  }

  public void UpdateRefCounts()
  {
    int count = this.Count;
    for (int index = 0; index < count; ++index)
      this.newRefCount.Add(0);
  }

  public void RemoveUnnecessaryStrings()
  {
    this.Parse();
    this.m_book.ReAddAllStrings();
    int num = 0;
    for (int count = this.Count; num < count; ++num)
    {
      if (this.GetRefCount(num) == 0)
      {
        object arrString = this.m_arrStrings[num];
        if (arrString != null)
        {
          if (this.m_bUseHash)
            this.m_hashKeyToIndex.Remove(arrString);
          this.m_arrStrings[num] = (object) null;
          this.m_arrFreeIndexes[num] = num;
        }
      }
    }
    this.Defragment();
  }

  private void MoveStrings(
    int iStartIndex,
    int iEndIndex,
    int iDecreaseValue,
    List<int> arrNewIndexes)
  {
    for (int index1 = iStartIndex + 1; index1 < iEndIndex; ++index1)
    {
      object arrString = this.m_arrStrings[index1];
      int index2 = index1 - iDecreaseValue;
      this.m_arrStrings[index2] = arrString;
      if (this.m_bUseHash)
        this.m_hashKeyToIndex[arrString] = index2;
      arrNewIndexes[index1] = index2;
    }
  }

  private void Defragment()
  {
    int count1 = this.Count;
    int count2 = this.m_arrFreeIndexes.Count;
    if (count2 <= 0)
      return;
    int iStartIndex = this.m_arrFreeIndexes.Values[0];
    List<int> arrNewIndexes = new List<int>(count1 + 1);
    for (int index = 0; index < count1; ++index)
      arrNewIndexes.Add(index);
    IList<int> values = this.m_arrFreeIndexes.Values;
    int num = 1;
    for (int count3 = this.m_arrFreeIndexes.Count; num < count3; ++num)
    {
      int iEndIndex = values[num];
      this.MoveStrings(iStartIndex, iEndIndex, num, arrNewIndexes);
      iStartIndex = iEndIndex;
    }
    this.MoveStrings(iStartIndex, count1, this.m_arrFreeIndexes.Count, arrNewIndexes);
    this.m_arrStrings.RemoveRange(count1 - count2, count2);
    this.m_arrFreeIndexes.Clear();
    this.m_book.UpdateStringIndexes(arrNewIndexes);
  }

  private void SaveIntoRecords(OffsetArrayList records)
  {
    SSTRecord sstRecord;
    int num1;
    if (this.m_bParsed)
    {
      sstRecord = (SSTRecord) BiffRecordFactory.GetRecord(TBIFFRecord.SST);
      sstRecord.Strings = this.Keys;
      num1 = this.m_arrStrings.Count;
      sstRecord.NumberOfStrings = (uint) num1;
    }
    else
    {
      sstRecord = this.m_sstOriginal;
      num1 = (int) sstRecord.NumberOfStrings;
    }
    records.Add((IBiffStorage) sstRecord);
    int num2 = num1 / 126;
    int num3 = num2 < 8 ? 8 : num2;
    ExtSSTRecord record = (ExtSSTRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ExtSST);
    record.StringPerBucket = (ushort) num3;
    record.SSTInfo = new ExtSSTInfoSubRecord[(long) sstRecord.NumberOfUniqueStrings / (long) num3 + 1L];
    record.SST = sstRecord;
    for (int index = 0; index < record.SSTInfo.Length; ++index)
      record.SSTInfo[index] = new ExtSSTInfoSubRecord();
    records.Add((IBiffStorage) record);
  }

  private void SetRefCount(int index, int count)
  {
    while (index >= this.newRefCount.Count)
      this.newRefCount.Add(0);
    this.newRefCount[index] = count;
  }

  private int GetRefCount(int index)
  {
    return this.newRefCount.Count > index ? this.newRefCount[index] : 0;
  }

  private int Find(TextWithFormat key)
  {
    int num = -1;
    if (this.m_bUseHash)
    {
      if (this.m_hashKeyToIndex.ContainsKey((object) key))
        num = this.m_hashKeyToIndex[(object) key];
      else if (key.FormattingRunsCount == 0 && this.m_hashKeyToIndex.ContainsKey((object) key.Text))
        num = this.m_hashKeyToIndex[(object) key.Text];
    }
    else
    {
      int index = 0;
      for (int count = this.m_arrStrings.Count; index < count; ++index)
      {
        object arrString = this.m_arrStrings[index];
        if (arrString != null && (key.FormattingRunsCount == 0 && arrString is string && key.Text == (string) arrString || key.CompareTo(arrString) == 0))
        {
          num = index;
          break;
        }
      }
    }
    return num;
  }

  private int Find(object key)
  {
    int num = -1;
    if (this.m_bUseHash)
    {
      if (this.m_hashKeyToIndex.ContainsKey(key))
        num = this.m_hashKeyToIndex[key];
    }
    else
    {
      int index = 0;
      for (int count = this.m_arrStrings.Count; index < count; ++index)
      {
        object arrString = this.m_arrStrings[index];
        if (arrString != null && arrString.Equals(key))
        {
          num = index;
          break;
        }
      }
    }
    return num;
  }

  private void FillHash()
  {
    int index = 0;
    for (int count = this.m_arrStrings.Count; index < count; ++index)
    {
      object arrString = this.m_arrStrings[index];
      if (arrString != null)
        this.m_hashKeyToIndex[arrString] = index;
    }
  }

  public void Parse()
  {
    if (this.m_bParsed)
      return;
    Dictionary<int, int> dictUpdatedIndexes = new Dictionary<int, int>();
    object[] strings = this.m_sstOriginal.Strings;
    for (int key = 0; key < strings.Length; ++key)
    {
      int num = this.AddIncrease(strings[key], false);
      if (key != num)
        dictUpdatedIndexes.Add(key, num);
    }
    if (dictUpdatedIndexes.Count > 0)
      this.UpdateLabelSSTIndexes(dictUpdatedIndexes);
    this.m_bParsed = true;
    this.m_sstOriginal = (SSTRecord) null;
  }

  internal void UpdateLabelSSTIndexes(Dictionary<int, int> dictUpdatedIndexes)
  {
    IWorksheets worksheets = this.m_book.Worksheets;
    int Index = 0;
    for (int count = worksheets.Count; Index < count; ++Index)
      ((WorksheetImpl) worksheets[Index]).UpdateLabelSSTIndexes(dictUpdatedIndexes, new IncreaseIndex(this.AddIncrease));
  }

  internal int GetLabelSSTCount()
  {
    int labelSstCount = 0;
    if (this.m_bParsed)
    {
      foreach (KeyValuePair<object, int> keyValuePair in this.m_hashKeyToIndex)
        labelSstCount += this.GetRefCount(keyValuePair.Value);
    }
    return labelSstCount;
  }

  public void Dispose()
  {
    if (this.m_book == null)
      return;
    this.m_hashKeyToIndex = (Dictionary<object, int>) null;
    this.m_arrStrings = (List<object>) null;
    this.m_arrFreeIndexes = (SortedList<int, int>) null;
    this.m_book = (WorkbookImpl) null;
    this.m_sstOriginal = (SSTRecord) null;
    this.m_tempString = (TextWithFormat) null;
    this.newRefCount.Clear();
    this.newRefCount = (List<int>) null;
    GC.SuppressFinalize((object) this);
  }

  ~SSTDictionary() => this.Dispose();
}
