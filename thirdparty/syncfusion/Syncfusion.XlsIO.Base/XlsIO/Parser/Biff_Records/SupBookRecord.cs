// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.SupBookRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Exceptions;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.SupBook)]
public class SupBookRecord : BiffRecordWithContinue
{
  public const ushort INTERNAL_REFERENCE = 1025;
  public const ushort ADDIN_FUNCTION = 14849;
  private bool m_bIsInternal;
  private bool m_bIsAddInFunction;
  [BiffRecordPos(0, 2)]
  private ushort m_usSheetNumber;
  [BiffRecordPos(2, 2)]
  private ushort m_usUrlLength;
  private string m_strUrl;
  private List<string> m_arrSheetNames;
  private string m_strOriginalURL;

  public bool IsInternalReference
  {
    get => this.m_bIsInternal;
    set => this.m_bIsInternal = value;
  }

  public override int MinimumRecordSize => 4;

  public override int MaximumRecordSize => this.m_bIsInternal ? 4 : base.MaximumRecordSize;

  public string URL
  {
    get => this.m_strUrl;
    set
    {
      this.m_strUrl = value;
      this.m_usUrlLength = this.m_strUrl != null ? (ushort) value.Length : (ushort) 0;
    }
  }

  public string OriginalURL
  {
    get => this.m_strOriginalURL;
    set => this.m_strOriginalURL = value;
  }

  public List<string> SheetNames
  {
    get => this.m_arrSheetNames;
    set => this.m_arrSheetNames = value;
  }

  public ushort SheetNumber
  {
    get
    {
      if (!this.IsInternalReference)
        this.m_usSheetNumber = this.m_arrSheetNames != null ? (ushort) this.m_arrSheetNames.Count : (ushort) 0;
      return this.m_usSheetNumber;
    }
    set
    {
      this.m_usSheetNumber = value;
      if (this.IsInternalReference)
        return;
      this.m_usSheetNumber = this.m_arrSheetNames != null ? (ushort) this.m_arrSheetNames.Count : (ushort) 0;
    }
  }

  public bool IsAddInFunctions
  {
    get => this.m_bIsAddInFunction;
    set => this.m_bIsAddInFunction = value;
  }

  public override void ParseStructure()
  {
    this.m_usSheetNumber = this.m_provider.ReadUInt16(0);
    this.m_usUrlLength = this.m_provider.ReadUInt16(2);
    this.m_arrSheetNames = new List<string>((int) this.m_usSheetNumber);
    this.m_bIsInternal = this.m_iLength == 4 && this.m_usUrlLength == (ushort) 1025;
    if (this.m_bIsInternal || (this.m_bIsAddInFunction = this.m_iLength == 4 && this.m_usUrlLength == (ushort) 14849))
      return;
    int iOffset1 = 2;
    int iFullLength;
    this.m_strOriginalURL = this.m_strUrl = this.m_provider.ReadString16Bit(iOffset1, out iFullLength);
    int iOffset2 = iOffset1 + iFullLength;
    this.m_arrSheetNames = new List<string>((int) this.m_usSheetNumber);
    for (int index = 0; index < (int) this.m_usSheetNumber; ++index)
    {
      this.m_arrSheetNames.Add(this.m_provider.ReadString16BitUpdateOffset(ref iOffset2));
      if (iOffset2 > this.m_iLength)
        throw new WrongBiffRecordDataException();
    }
    if (iOffset2 != this.m_iLength)
      throw new WrongBiffRecordDataException();
  }

  public override void InfillInternalData(ExcelVersion version)
  {
    this.PrognoseRecordSize();
    this.m_arrContinuePos.Clear();
    if (this.m_bIsInternal)
      this.m_usUrlLength = (ushort) 1025;
    else if (this.m_bIsAddInFunction)
      this.m_usUrlLength = (ushort) 14849;
    else if (this.m_strOriginalURL != null)
      this.m_usUrlLength = (ushort) this.m_strOriginalURL.Length;
    this.m_iLength = 0;
    IntPtrContinueRecordBuilder continueRecordBuilder = new IntPtrContinueRecordBuilder((BiffRecordWithContinue) this, 4);
    continueRecordBuilder.AppendUInt16(this.SheetNumber);
    continueRecordBuilder.AppendUInt16(this.m_usUrlLength);
    if (!this.m_bIsInternal && !this.m_bIsAddInFunction)
    {
      byte[] arrBuffer = (byte[]) null;
      int iSize = (int) this.m_usUrlLength * 2 + 3;
      SSTRecord.EnsureSize(ref arrBuffer, iSize);
      int offset1 = 0;
      string str = this.m_strOriginalURL != null ? this.m_strOriginalURL : this.m_strUrl;
      BiffRecordRaw.SetString16BitUpdateOffset(arrBuffer, ref offset1, str);
      continueRecordBuilder.AppendBytes(arrBuffer, 2, iSize - 2);
      if (this.m_arrSheetNames != null)
      {
        int index = 0;
        for (int count = this.m_arrSheetNames.Count; index < count; ++index)
        {
          string arrSheetName = this.m_arrSheetNames[index];
          int num = arrSheetName.Length * 2 + 3;
          SSTRecord.EnsureSize(ref arrBuffer, num);
          int offset2 = 0;
          BiffRecordRaw.SetString16BitUpdateOffset(arrBuffer, ref offset2, arrSheetName);
          if (continueRecordBuilder.FreeSpace < num)
            continueRecordBuilder.StartContinueRecord();
          continueRecordBuilder.AppendBytes(arrBuffer, 0, num);
        }
      }
    }
    this.m_iLength = continueRecordBuilder.Total;
    this.m_iFirstLength = continueRecordBuilder.FirstRecordLength;
    continueRecordBuilder.Dispose();
  }

  private void PrognoseRecordSize()
  {
    int size = 4;
    if (!this.m_bIsInternal && !this.m_bIsAddInFunction)
    {
      size += 3 + (int) this.m_usUrlLength * 2;
      int num1 = size;
      if (this.m_arrSheetNames != null)
      {
        int index = 0;
        for (int count = this.m_arrSheetNames.Count; index < count; ++index)
        {
          int num2 = this.m_arrSheetNames[index].Length * 2 + 3;
          if (num1 + num2 > 8224)
          {
            size += 4;
            num1 = 0;
          }
          size += num2;
          num1 += num2;
        }
      }
    }
    this.m_provider.EnsureCapacity(size);
  }

  public override int GetStoreSize(ExcelVersion version)
  {
    if (this.NeedInfill)
    {
      this.InfillInternalData(version);
      this.NeedInfill = false;
    }
    return this.m_iLength;
  }

  internal new object Clone()
  {
    SupBookRecord supBookRecord = (SupBookRecord) base.Clone();
    supBookRecord.m_arrSheetNames = new List<string>();
    for (int index = 0; index < this.m_arrSheetNames.Count; ++index)
      supBookRecord.m_arrSheetNames.Add(this.m_arrSheetNames[index]);
    return (object) supBookRecord;
  }
}
