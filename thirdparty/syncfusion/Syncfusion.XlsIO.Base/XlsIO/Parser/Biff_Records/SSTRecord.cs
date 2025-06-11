// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.SSTRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.SST)]
public class SSTRecord : BiffRecordWithContinue
{
  private const int DEF_OPTIONS_OFFET = 2;
  [BiffRecordPos(0, 4)]
  private uint m_uiNumberOfStrings;
  [BiffRecordPos(4, 4)]
  private uint m_uiNumberOfUniqueStrings;
  private object[] m_arrStrings;
  private int[] m_arrStringsPos;
  private int[] m_arrStringOffset;
  private bool m_bAutoAttach = true;

  public uint NumberOfStrings
  {
    get => this.m_uiNumberOfStrings;
    set => this.m_uiNumberOfStrings = value;
  }

  public uint NumberOfUniqueStrings => this.m_uiNumberOfUniqueStrings;

  public object[] Strings
  {
    get => this.m_arrStrings;
    set
    {
      this.m_arrStrings = value != null ? value : throw new ArgumentNullException(nameof (value));
      this.m_uiNumberOfUniqueStrings = (uint) this.m_arrStrings.Length;
    }
  }

  public int[] StringsStreamPos => this.m_arrStringsPos;

  public int[] StringsOffsets => this.m_arrStringOffset;

  public bool AutoAttachContinue
  {
    get => this.m_bAutoAttach;
    set => this.m_bAutoAttach = value;
  }

  public override bool NeedDataArray => true;

  public override void ParseStructure()
  {
    this.m_uiNumberOfStrings = this.m_provider.ReadUInt32(0);
    this.m_uiNumberOfUniqueStrings = this.m_provider.ReadUInt32(4);
    this.m_arrStrings = new object[(IntPtr) this.m_uiNumberOfUniqueStrings];
    int num = 8;
    int count = this.m_arrContinuePos.Count;
    int iBreakIndex = 0;
    TextWithFormat textWithFormat1 = new TextWithFormat(0);
    for (int index = 0; (long) index < (long) this.m_uiNumberOfUniqueStrings; ++index)
    {
      if (3 + num > this.m_iLength)
        throw new WrongBiffRecordDataException(nameof (SSTRecord));
      int length;
      byte[] rich;
      string unkTypeString = this.GetUnkTypeString(num, (IList<int>) this.m_arrContinuePos, count, ref iBreakIndex, out length, out rich, out byte[] _);
      object obj;
      if (rich != null && rich.Length > 0)
      {
        TextWithFormat textWithFormat2 = textWithFormat1.TypedClone();
        textWithFormat2.Text = unkTypeString;
        textWithFormat2.ParseFormattingRuns(rich);
        obj = (object) textWithFormat2;
      }
      else
        obj = (object) unkTypeString;
      this.m_arrStrings[index] = obj;
      num += length;
    }
    this.InternalDataIntegrityCheck(num);
  }

  public override void InfillInternalData(ExcelVersion version)
  {
    this.m_arrContinuePos.Clear();
    this.PrognoseRecordSize();
    this.m_uiNumberOfStrings = this.m_uiNumberOfUniqueStrings;
    this.m_provider.WriteUInt32(0, this.m_uiNumberOfStrings);
    this.m_provider.WriteUInt32(4, this.m_uiNumberOfUniqueStrings);
    this.m_iLength = 8;
    this.m_arrStringsPos = new int[(IntPtr) this.m_uiNumberOfUniqueStrings];
    this.m_arrStringOffset = new int[(IntPtr) this.m_uiNumberOfUniqueStrings];
    byte[] arrBuffer1 = (byte[]) null;
    byte[] arrBuffer2 = (byte[]) null;
    TextWithFormat textWithFormat1 = new TextWithFormat(0);
    IntPtrContinueRecordBuilder builder = new IntPtrContinueRecordBuilder((BiffRecordWithContinue) this, 4);
    for (int index = 0; (long) index < (long) this.m_uiNumberOfUniqueStrings; ++index)
    {
      object arrString = this.m_arrStrings[index];
      TextWithFormat textWithFormat2 = arrString as TextWithFormat;
      int num = 0;
      TextWithFormat.StringType options = TextWithFormat.StringType.Unicode;
      Encoding encoding = Encoding.Unicode;
      string str;
      if (textWithFormat2 == null)
      {
        str = (string) arrString;
      }
      else
      {
        str = textWithFormat2.Text;
        num = textWithFormat2.GetFormattingSize();
        options = textWithFormat2.GetOptions();
      }
      if (BiffRecordRawWithArray.IsAsciiString(str))
      {
        options &= ~TextWithFormat.StringType.Unicode;
        encoding = Encoding.UTF8;
      }
      ushort length = (ushort) str.Length;
      int byteCount = encoding.GetByteCount(str);
      int iSize = byteCount + 3;
      SSTRecord.EnsureSize(ref arrBuffer1, iSize);
      SSTRecord.EnsureSize(ref arrBuffer2, num);
      encoding.GetBytes(str, 0, (int) length, arrBuffer1, 0);
      if (num > 0)
        textWithFormat2.SerializeFormatting(arrBuffer2, 0, false);
      this.m_arrStringsPos[index] = builder.Position + 4;
      this.m_arrStringOffset[index] = builder.Offset;
      if (builder.FreeSpace < 20)
        builder.StartContinueRecord();
      builder.AppendUInt16(length);
      this.InfillText(builder, options, num, byteCount, arrBuffer1);
      this.InfillFormatting(builder, arrBuffer2, num);
    }
    this.m_iLength = builder.Total;
    this.m_iFirstLength = builder.FirstRecordLength;
    builder.Dispose();
  }

  private void InfillText(
    IntPtrContinueRecordBuilder builder,
    TextWithFormat.StringType options,
    int iFormattingSize,
    int totalLen,
    byte[] arrStringData)
  {
    int start = 0;
    do
    {
      builder.AppendByte((byte) options);
      if (start == 0 && iFormattingSize > 0)
        builder.AppendUInt16((ushort) (iFormattingSize / 4));
      int length = (options & TextWithFormat.StringType.Unicode) != TextWithFormat.StringType.NonUnicode ? Math.Min(builder.FreeSpace, totalLen - start) / 2 * 2 : Math.Min(builder.FreeSpace, totalLen - start);
      builder.AppendBytes(arrStringData, start, length);
      start += length;
      if (start < totalLen)
        builder.StartContinueRecord();
      options = (options & TextWithFormat.StringType.Unicode) == TextWithFormat.StringType.NonUnicode ? TextWithFormat.StringType.NonUnicode : TextWithFormat.StringType.Unicode;
    }
    while (start < totalLen);
  }

  private void InfillFormatting(
    IntPtrContinueRecordBuilder builder,
    byte[] arrFormattingData,
    int iFormattingSize)
  {
    int start = 0;
    if (iFormattingSize <= 0)
      return;
    do
    {
      int length = Math.Min(builder.FreeSpace, iFormattingSize - start) / 4 * 4;
      builder.AppendBytes(arrFormattingData, start, length);
      builder.UpdateContinueRecordSize();
      start += length;
      if (start < iFormattingSize)
        builder.StartContinueRecord();
    }
    while (start < iFormattingSize);
  }

  private void PrognoseRecordSize()
  {
    int num = 0;
    for (int index = 0; (long) index < (long) this.m_uiNumberOfUniqueStrings; ++index)
    {
      object arrString = this.m_arrStrings[index];
      string str;
      if (!(arrString is TextWithFormat textWithFormat))
      {
        str = (string) arrString;
      }
      else
      {
        str = textWithFormat.Text;
        if (textWithFormat.FormattingRunsCount > 0)
          num += textWithFormat.FormattingRunsCount * 4 + 2;
      }
      num += str.Length * 2 + 3;
    }
    this.m_provider.EnsureCapacity(8 + (num + num / 1000));
  }

  public static void EnsureSize(ref byte[] arrBuffer, int iSize)
  {
    if ((arrBuffer == null ? 0 : arrBuffer.Length) >= iSize)
      return;
    arrBuffer = new byte[iSize];
  }

  private void InternalDataIntegrityCheck(int iCurPos)
  {
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
}
