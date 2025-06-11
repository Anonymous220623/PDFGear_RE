// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.BiffRecordWithContinue
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using Syncfusion.XlsIO.Implementation.Security;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[CLSCompliant(false)]
public abstract class BiffRecordWithContinue : BiffRecordRawWithDataProvider
{
  private int DEF_WORD_MASK = (int) ushort.MaxValue;
  internal List<int> m_arrContinuePos = new List<int>();
  protected int m_iFirstLength = -1;

  public virtual TBIFFRecord FirstContinueType => TBIFFRecord.Continue;

  protected virtual bool AddHeaderToProvider => false;

  public override int FillRecord(
    BinaryReader reader,
    DataProvider provider,
    IDecryptor decryptor,
    byte[] arrBuffer)
  {
    this.m_arrContinuePos = new List<int>();
    Stream baseStream = reader.BaseStream;
    long position = baseStream.Position;
    provider.Read(reader, 0, 4, arrBuffer);
    int num1 = provider.ReadInt32(0);
    this.m_iCode = num1 & this.DEF_WORD_MASK;
    this.m_iLength = num1 >> 16 /*0x10*/ & this.DEF_WORD_MASK;
    int num2 = this.m_iLength;
    int num3 = 0;
    int num4 = 0;
    int firstContinueType = (int) this.FirstContinueType;
    int num5;
    do
    {
      if (num4 > 0 && this.AddHeaderToProvider)
        num3 += 4;
      baseStream.Position += (long) num2;
      num3 += num2;
      ++num4;
      this.m_arrContinuePos.Add(num3);
      provider.Read(reader, 0, 4, arrBuffer);
      int num6 = provider.ReadInt32(0);
      num5 = num6 & this.DEF_WORD_MASK;
      num2 = num6 >> 16 /*0x10*/ & this.DEF_WORD_MASK;
    }
    while (num5 == 60 || num5 == firstContinueType);
    this.m_provider.EnsureCapacity(num3);
    baseStream.Position = position;
    int iOffset = 0;
    if (this.AddHeaderToProvider)
    {
      baseStream.Position += 4L;
      this.m_provider.Read(reader, 0, num3, arrBuffer);
      if (decryptor != null)
      {
        int offset = 0;
        long streamPosition = position + 4L;
        for (int index = 0; index < num4; ++index)
        {
          int arrContinuePo = this.m_arrContinuePos[index];
          int length = arrContinuePo - offset;
          decryptor.Decrypt(this.m_provider, offset, length, streamPosition);
          streamPosition += (long) (length + 4);
          offset = arrContinuePo + 4;
        }
      }
    }
    else
    {
      for (int index = 0; index < num4; ++index)
      {
        provider.Read(reader, 0, 4, arrBuffer);
        int iLength = (int) provider.ReadInt16(2);
        this.m_provider.Read(reader, iOffset, iLength, arrBuffer, decryptor);
        iOffset += iLength;
      }
    }
    this.m_iLength = num3;
    this.ParseStructure();
    return (int) (baseStream.Position - position);
  }

  public override int FillStream(
    BinaryWriter writer,
    DataProvider provider,
    IEncryptor encryptor,
    int streamPosition)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (this.NeedInfill)
      this.InfillInternalData(ExcelVersion.Excel97to2003);
    if (this.m_iLength < 0)
      throw new ApplicationException("Wrong Record data infill.");
    writer.Write((ushort) this.m_iCode);
    streamPosition += 2;
    if (this.m_iFirstLength < 0)
      writer.Write((ushort) this.m_iLength);
    else
      writer.Write((ushort) this.m_iFirstLength);
    streamPosition += 2;
    if (encryptor != null)
    {
      int startDecodingOffset = this.StartDecodingOffset;
      if (this.m_arrContinuePos.Count > 0)
      {
        int count1 = this.m_arrContinuePos.Count;
        if (!this.AddHeaderToProvider && this.m_arrContinuePos[count1 - 1] != this.m_iLength)
        {
          this.m_arrContinuePos.Add(this.m_iLength);
          int num = count1 + 1;
        }
        int offset = this.StartDecodingOffset;
        int index = 0;
        for (int count2 = this.m_arrContinuePos.Count; index < count2; ++index)
        {
          int arrContinuePo = this.m_arrContinuePos[index];
          int length = arrContinuePo - offset;
          encryptor.Encrypt(this.m_provider, offset, length, (long) streamPosition);
          streamPosition += length + 4;
          offset = arrContinuePo + 4;
        }
      }
      else
        encryptor.Encrypt(this.m_provider, startDecodingOffset, this.m_iLength - startDecodingOffset, (long) (streamPosition + startDecodingOffset));
    }
    byte[] internalBuffer = ((ByteArrayDataProvider) provider).InternalBuffer;
    this.m_provider.WriteInto(writer, 0, this.m_iLength, internalBuffer);
    if (!this.NeedDataArray)
      this.m_provider.Clear();
    this.NeedInfill = true;
    return this.m_iLength + 4;
  }

  public override object Clone()
  {
    BiffRecordWithContinue recordWithContinue = (BiffRecordWithContinue) base.Clone();
    recordWithContinue.m_arrContinuePos = CloneUtils.CloneCloneable<int>(this.m_arrContinuePos);
    if (this.m_provider != null)
    {
      recordWithContinue.m_provider = ApplicationImpl.CreateDataProvider();
      recordWithContinue.m_provider.EnsureCapacity(this.m_provider.Capacity);
      this.m_provider.CopyTo(0, recordWithContinue.m_provider, 0, this.m_provider.Capacity);
    }
    return (object) recordWithContinue;
  }
}
