// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.WorksheetConditionalFormats
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections;

public class WorksheetConditionalFormats(IApplication application, object parent) : 
  CollectionBaseEx<ConditionalFormats>(application, parent),
  ICloneParent
{
  private Dictionary<ConditionalFormats, ConditionalFormats> m_hash = new Dictionary<ConditionalFormats, ConditionalFormats>();
  internal Dictionary<int, CFExRecord> m_arrCFExRecords = new Dictionary<int, CFExRecord>();

  public ConditionalFormats Find(Rectangle[] arrRanges)
  {
    ConditionalFormats outputCF = new ConditionalFormats(this.Application, (object) this);
    if (arrRanges == null)
      return (ConditionalFormats) null;
    if (arrRanges.Length == 0)
      return (ConditionalFormats) null;
    int i = 0;
    for (int count = this.Count; i < count; ++i)
    {
      ConditionalFormats AddCf = this[i];
      if (AddCf.Contains(arrRanges))
        outputCF = this.AddCF(outputCF, AddCf);
    }
    return outputCF.Count == 0 ? (ConditionalFormats) null : outputCF;
  }

  private ConditionalFormats AddCF(ConditionalFormats outputCF, ConditionalFormats AddCf)
  {
    ConditionalFormats conditionalFormats = outputCF;
    for (int i = 0; i < AddCf.Count; ++i)
      outputCF.Add(AddCf[i]);
    return conditionalFormats;
  }

  public ConditionalFormats Contains(ConditionalFormats formats)
  {
    if (formats == null)
      throw new ArgumentNullException(nameof (formats));
    ConditionalFormats conditionalFormats;
    this.m_hash.TryGetValue(formats, out conditionalFormats);
    return conditionalFormats;
  }

  public ConditionalFormats Add(ConditionalFormats formats)
  {
    if (formats == null)
      throw new ArgumentNullException(nameof (formats));
    ConditionalFormats conditionalFormats;
    if (this.m_hash.TryGetValue(formats, out conditionalFormats))
    {
      if (conditionalFormats.CondFMTRecord != null && conditionalFormats.CondFMTRecord.CFNumber > (ushort) 0)
        conditionalFormats.AddCells(formats);
      else
        conditionalFormats.AddCellsCondFMT12(formats);
    }
    else
    {
      base.Add(formats);
      this.m_hash.Add(formats, formats);
    }
    return conditionalFormats ?? formats;
  }

  public void Remove(Rectangle[] arrRanges) => this.Remove(arrRanges, false);

  internal void Remove(Rectangle[] arrRanges, bool isCF)
  {
    if (arrRanges == null || arrRanges.Length == 0)
      return;
    int count1 = 0;
    System.Collections.Generic.List<ConditionalFormats> innerList = this.InnerList;
    int num = 0;
    for (int count2 = this.Count; num < count2; ++num)
    {
      ConditionalFormats key = this[num];
      key.Remove(arrRanges, isCF);
      if (key.IsEmpty)
      {
        int index = count2 - 1;
        if (index != num)
        {
          ConditionalFormats conditionalFormats = innerList[index];
          innerList[index] = key;
          innerList[num] = conditionalFormats;
        }
        ++count1;
        --count2;
        --num;
        this.m_hash.Remove(key);
      }
    }
    if (count1 <= 0)
      return;
    innerList.RemoveRange(this.Count - count1, count1);
  }

  public void CopyFrom(WorksheetConditionalFormats arrSourceFormats)
  {
    if (arrSourceFormats == null)
      throw new ArgumentNullException(nameof (arrSourceFormats));
    int i = 0;
    for (int count = arrSourceFormats.Count; i < count; ++i)
    {
      ConditionalFormats arrSourceFormat = arrSourceFormats[i];
      this.Add(new ConditionalFormats(this.Application, (object) this, arrSourceFormat, true)
      {
        Pivot = arrSourceFormat.Pivot
      });
    }
  }

  public override object Clone(object parent)
  {
    WorksheetConditionalFormats parent1 = (WorksheetConditionalFormats) base.Clone(parent);
    parent1.m_hash = CloneUtils.CloneHash<ConditionalFormats, ConditionalFormats>(this.m_hash, (object) parent1);
    return (object) parent1;
  }

  public void RemoveItem(ConditionalFormats formats)
  {
    this.Remove(formats);
    this.m_hash.Remove(formats);
  }

  public void MarkUsedReferences(bool[] usedItems)
  {
    System.Collections.Generic.List<ConditionalFormats> innerList = this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].MarkUsedReferences(usedItems);
  }

  public void UpdateReferenceIndexes(int[] arrUpdatedIndexes)
  {
    System.Collections.Generic.List<ConditionalFormats> innerList = this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].UpdateReferenceIndexes(arrUpdatedIndexes);
  }

  public void Dispose()
  {
    if (this.m_hash != null)
    {
      this.m_hash.Clear();
      this.m_hash = (Dictionary<ConditionalFormats, ConditionalFormats>) null;
    }
    if (this.m_arrCFExRecords == null)
      return;
    this.m_arrCFExRecords.Clear();
    this.m_hash = (Dictionary<ConditionalFormats, ConditionalFormats>) null;
  }

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    int priority = 1;
    ushort index = 1;
    int i = 0;
    for (int count = this.Count; i < count; ++i)
    {
      priority = this[i].Serialize(records, index, priority);
      this.m_arrCFExRecords = this[i].sheet.m_dictCFExRecords;
      ++index;
    }
    for (int key = 0; key < this.m_arrCFExRecords.Count; ++key)
    {
      if (this.m_arrCFExRecords[key].IsCF12Extends != (byte) 1)
      {
        records.Add((IBiffStorage) this.m_arrCFExRecords[key]);
      }
      else
      {
        CFExRecord arrCfExRecord = this.m_arrCFExRecords[key];
        records.Add((IBiffStorage) arrCfExRecord);
        records.Add((IBiffStorage) arrCfExRecord.CF12RecordIfExtends);
      }
    }
    this.m_arrCFExRecords.Clear();
  }

  internal void UpdateCFExProperties(ConditionalFormats conditionalFormats, CFExRecord extRecord)
  {
    for (int i = 0; i < conditionalFormats.Count; ++i)
    {
      ConditionalFormatImpl conditionalFormat = (ConditionalFormatImpl) conditionalFormats[i];
      if (extRecord != null && i == (int) extRecord.CFIndex)
      {
        conditionalFormat.SetCFExRecord(extRecord);
        if (extRecord.Template == ConditionalFormatTemplate.UniqueValues)
          conditionalFormat.FormatType = ExcelCFType.Unique;
        if (extRecord.Template == ConditionalFormatTemplate.DuplicateValues)
          conditionalFormat.FormatType = ExcelCFType.Duplicate;
        if (extRecord.Template == ConditionalFormatTemplate.Filter)
          conditionalFormat.FormatType = ExcelCFType.TopBottom;
        if (extRecord.Template == ConditionalFormatTemplate.AboveAverage || extRecord.Template == ConditionalFormatTemplate.AboveOrEqualToAverage || extRecord.Template == ConditionalFormatTemplate.BelowAverage || extRecord.Template == ConditionalFormatTemplate.BelowOrEqualToAverage)
          conditionalFormat.FormatType = ExcelCFType.AboveBelowAverage;
      }
    }
  }

  private Color ConvertRGBToColor(uint rgb)
  {
    if (rgb == 4278190080U /*0xFF000000*/)
      return ColorExtension.Empty;
    byte red = (byte) (rgb & (uint) byte.MaxValue);
    byte green = (byte) ((rgb & 65280U) >> 8);
    byte blue = (byte) ((rgb & 16711680U /*0xFF0000*/) >> 16 /*0x10*/);
    return Color.FromArgb((int) (byte) ~((rgb & (uint) byte.MaxValue) >> 24), (int) red, (int) green, (int) blue);
  }

  public void UpdateFormula(
    int iCurIndex,
    int iSourceIndex,
    Rectangle sourceRect,
    int iDestIndex,
    Rectangle destRect)
  {
    int i = 0;
    for (int count = this.Count; i < count; ++i)
      this[i].UpdateFormula(iCurIndex, iSourceIndex, sourceRect, iDestIndex, destRect);
  }
}
