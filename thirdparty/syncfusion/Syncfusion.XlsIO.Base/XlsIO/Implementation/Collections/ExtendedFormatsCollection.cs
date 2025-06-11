// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.ExtendedFormatsCollection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections;

public class ExtendedFormatsCollection(IApplication application, object parent) : 
  CollectionBaseEx<ExtendedFormatImpl>(application, parent)
{
  private const int DEF_DEFAULT_COUNT = 21;
  private Dictionary<ExtendedFormatImpl, ExtendedFormatImpl> m_hashFormats = new Dictionary<ExtendedFormatImpl, ExtendedFormatImpl>();

  public new ExtendedFormatImpl this[int index]
  {
    get
    {
      return index >= 0 && index < this.Count ? this.InnerList[index] : throw new ArgumentOutOfRangeException(nameof (index));
    }
  }

  public WorkbookImpl ParentWorkbook => this[0].Workbook;

  public ExtendedFormatImpl Add(ExtendedFormatImpl format) => this.Add(format, 0);

  internal ExtendedFormatImpl Add(ExtendedFormatImpl format, int count)
  {
    if (format == null)
      throw new ArgumentNullException(nameof (format));
    bool flag1 = true;
    if (this.m_hashFormats.ContainsKey(format))
    {
      ExtendedFormatImpl extendedFormatImpl = format;
      format = this.m_hashFormats[format];
      if (this.ParentWorkbook.Version == ExcelVersion.Excel97to2003 && format.Index != 0 && format.Index < 15)
        format = extendedFormatImpl;
      else
        flag1 = false;
      if (this.ParentWorkbook.Loading && this.ParentWorkbook.Options == ExcelParseOptions.ParseWorksheetsOnDemand)
      {
        format = extendedFormatImpl;
        flag1 = true;
      }
      if (!flag1)
      {
        bool flag2 = true;
        foreach (KeyValuePair<ExtendedFormatImpl, ExtendedFormatImpl> hashFormat in this.m_hashFormats)
        {
          if (extendedFormatImpl == hashFormat.Key)
          {
            flag2 = false;
            break;
          }
        }
        if (flag2)
          extendedFormatImpl.Clear();
      }
    }
    else
    {
      int count1 = this.Count;
      int num = this.Count - count >= 0 ? this.Count - count : this.Count;
      WorkbookImpl workbookImpl = count1 > 0 ? this.ParentWorkbook : format.Workbook;
      if (num >= workbookImpl.MaxXFCount && this.Count >= workbookImpl.MaxXFCount)
      {
        workbookImpl.m_bisUnusedXFRemoved = false;
        if (workbookImpl.InnerExtFormats.Count >= workbookImpl.MaxXFCount)
          throw new ApplicationException("Maximum number of extended formats exceeded.");
      }
      this.m_hashFormats.Add(format, format);
    }
    if (flag1)
    {
      format.Index = (int) (ushort) this.List.Count;
      base.Add(format);
      if (!format.Workbook.m_xfCellCount.ContainsKey(format.Index))
      {
        format.Workbook.m_xfCellCount.Add(format.Index, 1);
      }
      else
      {
        int num;
        if (format.Workbook.m_xfCellCount.TryGetValue(format.Index, out num))
          format.Workbook.m_xfCellCount[format.Index] = num + 1;
      }
    }
    else
    {
      int num;
      if (format.Workbook.m_xfCellCount.ContainsKey(format.Index) && format.Workbook.m_xfCellCount.TryGetValue(format.Index, out num))
        format.Workbook.m_xfCellCount[format.Index] = num + 1;
    }
    return format;
  }

  public ExtendedFormatImpl ForceAdd(ExtendedFormatImpl format)
  {
    if (format == null)
      throw new ArgumentNullException(nameof (format));
    if (!this.m_hashFormats.ContainsKey(format))
      this.m_hashFormats.Add(format, format);
    format.Index = (int) (ushort) this.List.Count;
    base.Add(format);
    return format;
  }

  public int Import(ExtendedFormatImpl format, Dictionary<int, int> hashExtFormatIndexes)
  {
    if (format == null)
      throw new ArgumentNullException(nameof (format));
    if (hashExtFormatIndexes == null)
      throw new ArgumentNullException(nameof (hashExtFormatIndexes));
    int index = format.Index;
    if (this.Count > index && this[index] == format)
      return index;
    ExtendedFormatImpl format1 = format.TypedClone((object) this);
    int key = format1.ParentIndex;
    if (hashExtFormatIndexes.ContainsKey(key))
      key = hashExtFormatIndexes[key];
    format1.ParentIndex = key;
    return this.Add(format1).Index;
  }

  public Dictionary<int, int> Merge(
    IList<ExtendedFormatImpl> arrXFormats,
    out Dictionary<int, int> dicFontIndexes)
  {
    if (arrXFormats == null)
      throw new ArgumentNullException(nameof (arrXFormats));
    dicFontIndexes = (Dictionary<int, int>) null;
    if (arrXFormats == this)
      return (Dictionary<int, int>) null;
    if (arrXFormats.Count == 0)
      return (Dictionary<int, int>) null;
    WorkbookImpl workbook = arrXFormats[0].Workbook;
    if (!(this.FindParent(typeof (WorkbookImpl)) is WorkbookImpl parent))
      throw new ArgumentNullException("Can't find destination workbook.");
    dicFontIndexes = parent.InnerFonts.AddRange(workbook.InnerFonts);
    Dictionary<int, int> dicFormatIndexes = parent.InnerFormats.Merge(workbook.InnerFormats);
    this.MarkUsedFormats(arrXFormats, workbook);
    return this.Merge(arrXFormats, dicFontIndexes, dicFormatIndexes);
  }

  public Dictionary<int, int> Merge(IList<ExtendedFormatImpl> arrXFormats)
  {
    Dictionary<int, int> dicFontIndexes = arrXFormats != null ? this.GetFontIndexes(arrXFormats) : throw new ArgumentNullException(nameof (arrXFormats));
    Dictionary<int, int> formatIndexes = this.GetFormatIndexes(arrXFormats);
    return this.Merge(arrXFormats, dicFontIndexes, formatIndexes);
  }

  public void AddIndex(
    Dictionary<int, object> hashToAdd,
    IList<ExtendedFormatImpl> arrXFormats,
    int index)
  {
    if (hashToAdd == null)
      throw new ArgumentNullException(nameof (hashToAdd));
    if (arrXFormats == null)
      throw new ArgumentNullException(nameof (arrXFormats));
    if (hashToAdd.ContainsKey(index))
      return;
    hashToAdd.Add(index, (object) null);
    ExtendedFormatImpl extendedFormatImpl = this[index];
    arrXFormats.Add(extendedFormatImpl);
    if (!extendedFormatImpl.HasParent)
      return;
    this.AddIndex(hashToAdd, arrXFormats, extendedFormatImpl.ParentIndex);
  }

  public ExtendedFormatImpl GatherTwoFormats(int iFirstXF, int iEndXF)
  {
    if (iFirstXF >= this.Count || iFirstXF < 0)
      throw new ArgumentOutOfRangeException(nameof (iFirstXF));
    if (iEndXF >= this.Count || iEndXF < 0)
      throw new ArgumentOutOfRangeException(nameof (iEndXF));
    if (iFirstXF == iEndXF)
      return (ExtendedFormatImpl) this[iFirstXF].Clone();
    ExtendedFormatImpl extendedFormatImpl1 = this[iFirstXF];
    ExtendedFormatImpl extendedFormatImpl2 = this[iEndXF];
    ExtendedFormatImpl extendedFormatImpl3 = (ExtendedFormatImpl) extendedFormatImpl1.Clone();
    IBorder border1;
    IBorder border2;
    if (extendedFormatImpl2.IncludeBorder)
    {
      border1 = extendedFormatImpl2.Borders[ExcelBordersIndex.EdgeRight];
      border2 = extendedFormatImpl2.Borders[ExcelBordersIndex.EdgeBottom];
    }
    else
    {
      ExtendedFormatImpl extendedFormatImpl4 = this[extendedFormatImpl2.ParentIndex];
      border1 = extendedFormatImpl4.Borders[ExcelBordersIndex.EdgeRight];
      border2 = extendedFormatImpl4.Borders[ExcelBordersIndex.EdgeBottom];
    }
    IBorder border3 = extendedFormatImpl3.Borders[ExcelBordersIndex.EdgeRight];
    border3.ColorObject.CopyFrom(border1.ColorObject, true);
    border3.LineStyle = border1.LineStyle;
    IBorder border4 = extendedFormatImpl3.Borders[ExcelBordersIndex.EdgeBottom];
    border4.ColorObject.CopyFrom(border2.ColorObject, true);
    border4.LineStyle = border2.LineStyle;
    return extendedFormatImpl3;
  }

  public Dictionary<int, int> RemoveAt(int xfIndex)
  {
    int count1 = this.Count;
    if (xfIndex < 0 || xfIndex > count1)
      return (Dictionary<int, int>) null;
    SortedList<int, ExtendedFormatImpl> sortedList = new SortedList<int, ExtendedFormatImpl>();
    this.m_hashFormats.Remove(this[xfIndex]);
    Dictionary<int, int> dictionary = new Dictionary<int, int>();
    this.InnerList[xfIndex] = (ExtendedFormatImpl) null;
    dictionary[xfIndex] = 0;
    for (int index = 0; index < count1; ++index)
    {
      ExtendedFormatImpl key = this[index];
      if (key != null && key.ParentIndex == xfIndex)
      {
        this.m_hashFormats.Remove(key);
        key.ParentIndex = 0;
        key.SynchronizeWithParent();
        sortedList.Add(index, (ExtendedFormatImpl) null);
      }
    }
    int count2 = sortedList.Count;
    IList<int> keys1 = sortedList.Keys;
    for (int index = 0; index < count2; ++index)
    {
      int num = keys1[index];
      ExtendedFormatImpl key = this[num];
      if (key != null)
      {
        ExtendedFormatImpl extendedFormatImpl;
        if (this.m_hashFormats.TryGetValue(key, out extendedFormatImpl))
        {
          sortedList[num] = extendedFormatImpl;
          this.InnerList[num] = (ExtendedFormatImpl) null;
        }
        else
        {
          this.m_hashFormats.Add(key, key);
          sortedList[num] = key;
        }
      }
    }
    int count3 = this.Count;
    int num1 = 0;
    for (int index = 0; index < count3; ++index)
    {
      ExtendedFormatImpl extendedFormatImpl = this[index];
      if (extendedFormatImpl != null)
      {
        if ((this.Parent as WorkbookImpl).m_xfCellCount.ContainsKey(index))
        {
          int num2 = 0;
          if ((this.Parent as WorkbookImpl).m_xfCellCount.TryGetValue(index, out num2))
          {
            int key = index - num1;
            (this.Parent as WorkbookImpl).m_xfCellCount.Remove(index);
            (this.Parent as WorkbookImpl).m_xfCellCount.Add(key, num2);
          }
        }
        int num3 = index - num1;
        dictionary.Add(index, num3);
        extendedFormatImpl.Index = (int) (ushort) num3;
      }
      else
      {
        int num4 = 0;
        if ((this.Parent as WorkbookImpl).m_xfCellCount.TryGetValue(index, out num4))
          (this.Parent as WorkbookImpl).m_xfCellCount.Remove(index);
        ++num1;
      }
    }
    IList<int> keys2 = sortedList.Keys;
    for (int index = 0; index < count2; ++index)
    {
      int key = keys2[index];
      ExtendedFormatImpl extendedFormatImpl = sortedList[key];
      dictionary[key] = extendedFormatImpl.Index;
    }
    for (int index = 0; index < count3; ++index)
    {
      ExtendedFormatImpl extendedFormatImpl = this[index];
      if (extendedFormatImpl != null && extendedFormatImpl.HasParent)
      {
        int parentIndex = extendedFormatImpl.ParentIndex;
        if (parentIndex != 4095 /*0x0FFF*/ && dictionary.ContainsKey(index))
          extendedFormatImpl.ParentIndex = (int) (ushort) dictionary[parentIndex];
      }
    }
    for (int index = 0; index < count3; ++index)
    {
      if (this[index] == null)
      {
        this.InnerList.RemoveAt(index);
        --count3;
        --index;
      }
    }
    return dictionary;
  }

  public override object Clone(object parent)
  {
    ExtendedFormatsCollection formatsCollection = parent != null ? (ExtendedFormatsCollection) base.Clone(parent) : throw new ArgumentNullException(nameof (parent));
    System.Collections.Generic.List<ExtendedFormatImpl> innerList = formatsCollection.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
    {
      ExtendedFormatImpl key = innerList[index];
      if (!this.m_hashFormats.ContainsKey(key))
        this.m_hashFormats.Add(key, key);
      key.Index = index;
    }
    return (object) formatsCollection;
  }

  public void SetMaxCount(int maxCount)
  {
    if (this.Count <= 0)
      return;
    WorkbookImpl workbook = this[0].Workbook;
    int maxXfCount = workbook.MaxXFCount;
    System.Collections.Generic.List<ExtendedFormatImpl> innerList = this.InnerList;
    int count1 = innerList.Count;
    if (count1 >= maxCount)
      innerList.RemoveRange(maxCount - 1, count1 - maxCount);
    int index = 0;
    for (int count2 = this.Count; index < count2; ++index)
    {
      ExtendedFormatImpl extendedFormatImpl = innerList[index];
      if (extendedFormatImpl.ParentIndex == maxXfCount)
        extendedFormatImpl.ParentIndex = maxCount;
    }
    workbook.UpdateXFIndexes(maxCount);
  }

  internal void SetXF(int iXFIndex, ExtendedFormatImpl format)
  {
    if (iXFIndex >= this.Count)
      throw new ArgumentOutOfRangeException(nameof (iXFIndex));
    if (format == null)
      throw new ArgumentNullException(nameof (format));
    this.m_hashFormats.Remove(this[iXFIndex]);
    this.InnerList[iXFIndex] = format;
    this.m_hashFormats.Add(format, format);
  }

  private Dictionary<int, int> Merge(
    IList<ExtendedFormatImpl> arrXFormats,
    Dictionary<int, int> dicFontIndexes,
    Dictionary<int, int> dicFormatIndexes)
  {
    if (arrXFormats == null)
      throw new ArgumentNullException(nameof (arrXFormats));
    if (dicFontIndexes == null)
      throw new ArgumentNullException(nameof (dicFontIndexes));
    if (dicFormatIndexes == null)
      throw new ArgumentNullException(nameof (dicFormatIndexes));
    int count = arrXFormats.Count;
    Dictionary<int, int> hashResult = new Dictionary<int, int>(count);
    for (int index1 = 0; index1 < count; ++index1)
    {
      ExtendedFormatImpl arrXformat = arrXFormats[index1];
      int index2 = arrXformat.Index;
      if (!hashResult.ContainsKey(index2))
        this.Merge(arrXformat, hashResult, dicFontIndexes, dicFormatIndexes);
    }
    return hashResult;
  }

  private void Merge(
    ExtendedFormatImpl format,
    Dictionary<int, int> hashResult,
    Dictionary<int, int> dicFontIndexes,
    Dictionary<int, int> dicFormatIndexes)
  {
    if (format == null)
      throw new ArgumentNullException(nameof (format));
    if (hashResult == null)
      throw new ArgumentNullException(nameof (hashResult));
    ExtendedFormatsCollection parentCollection = format.ParentCollection;
    int parentIndex = format.ParentIndex;
    bool flag1 = format.Index <= 0 || format.HasParent || !format.HasParent && format.m_bisValid;
    if (format.HasParent)
    {
      if (!hashResult.ContainsKey(parentIndex))
        this.Merge(parentCollection[parentIndex], hashResult, dicFontIndexes, dicFormatIndexes);
      parentIndex = hashResult[parentIndex];
    }
    if (!flag1)
      return;
    int index = format.Index;
    bool flag2 = false;
    if (!format.HasParent && index > 0)
    {
      StyleImpl byXfIndex = parentCollection.ParentWorkbook.InnerStyles.GetByXFIndex(format.Index);
      if (byXfIndex != null)
        flag2 = !this.ParentWorkbook.Styles.Contains(byXfIndex.Name);
    }
    ExtendedFormatImpl format1 = format.TypedClone((object) this);
    int formatIndex = (int) format.Record.FormatIndex;
    int key = (int) format.Record.FontIndex;
    if (format.HasParent)
      format1.ParentIndex = parentIndex;
    if (key >= dicFontIndexes.Count)
      key = 0;
    format1.Record.FontIndex = (ushort) dicFontIndexes[key];
    if (dicFormatIndexes != null && dicFormatIndexes.ContainsKey(formatIndex))
    {
      int dicFormatIndex = dicFormatIndexes[formatIndex];
      format1.Record.FormatIndex = (ushort) dicFormatIndex;
    }
    ExtendedFormatImpl extendedFormatImpl = !flag2 ? this.Add(format1) : this.ForceAdd(format1);
    hashResult.Add(index, extendedFormatImpl.Index);
  }

  private void MarkUsedFormats(IList<ExtendedFormatImpl> allFormats, WorkbookImpl sourceBook)
  {
    StylesCollection innerStyles = sourceBook.InnerStyles;
    for (int index = 0; index < allFormats.Count; ++index)
    {
      ExtendedFormatImpl allFormat = allFormats[index];
      if (allFormat.HasParent)
      {
        int parentIndex = allFormat.ParentIndex;
        if (parentIndex >= 0)
        {
          sourceBook.InnerExtFormats[parentIndex].m_bisValid = true;
          StyleImpl byXfIndex = innerStyles.GetByXFIndex(parentIndex);
          if (byXfIndex != null && !byXfIndex.m_bisUsed)
            byXfIndex.m_bisUsed = true;
        }
      }
    }
  }

  private Dictionary<int, int> GetFontIndexes(IList<ExtendedFormatImpl> arrXFormats)
  {
    if (arrXFormats == null)
      throw new ArgumentNullException(nameof (arrXFormats));
    Dictionary<int, object> dictionary = new Dictionary<int, object>();
    int index = 0;
    for (int count = arrXFormats.Count; index < count; ++index)
    {
      ExtendedFormatImpl arrXformat = arrXFormats[index];
      dictionary[arrXformat.FontIndex] = (object) -1;
    }
    WorkbookImpl workbook = arrXFormats[0].Workbook;
    return ((WorkbookImpl) this.FindParent(typeof (WorkbookImpl))).InnerFonts.AddRange((ICollection<int>) dictionary.Keys, workbook.InnerFonts);
  }

  private Dictionary<int, int> GetFormatIndexes(IList<ExtendedFormatImpl> arrXFormats)
  {
    int num = arrXFormats != null ? arrXFormats.Count : throw new ArgumentNullException(nameof (arrXFormats));
    Dictionary<int, int> dicIndexes = new Dictionary<int, int>();
    if (num == 0)
      return dicIndexes;
    int index = 0;
    for (int count = arrXFormats.Count; index < count; ++index)
    {
      ExtendedFormatImpl arrXformat = arrXFormats[index];
      dicIndexes[arrXformat.NumberFormatIndex] = -1;
    }
    WorkbookImpl workbook = arrXFormats[0].Workbook;
    return ((WorkbookImpl) this.FindParent(typeof (WorkbookImpl))).InnerFormats.AddRange((IDictionary) dicIndexes, workbook.InnerFormats);
  }

  protected override void OnClearComplete() => this.m_hashFormats.Clear();

  internal void Dispose()
  {
    foreach (ExtendedFormatImpl inner in this.InnerList)
      inner.ClearAll();
    this.InnerList.Clear();
  }
}
