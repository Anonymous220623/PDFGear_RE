// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.FilePassRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Exceptions;
using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[Biff(TBIFFRecord.FilePass)]
internal class FilePassRecord : BiffRecordRaw
{
  internal const int DEF_STANDARD_HASH = 1;
  internal const int DEF_STRONG_HASH = 2;
  private ushort m_usNotWeakEncryption;
  private ushort m_usKey;
  private ushort m_usHash;
  private FilePassStandardBlock m_standardBlock;
  private FilePassStrongBlock m_strongBlock;

  public FilePassRecord()
  {
  }

  public FilePassRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public FilePassRecord(int iReserve)
    : base(iReserve)
  {
  }

  public bool IsWeakEncryption
  {
    get => this.m_usNotWeakEncryption == (ushort) 0;
    set => this.m_usNotWeakEncryption = value ? (ushort) 0 : (ushort) 1;
  }

  [CLSCompliant(false)]
  public ushort Key
  {
    get => this.m_usKey;
    set => this.m_usKey = value;
  }

  [CLSCompliant(false)]
  public ushort Hash
  {
    get => this.m_usHash;
    set => this.m_usHash = value;
  }

  public FilePassStandardBlock StandardBlock => this.m_standardBlock;

  public void CreateStandardBlock() => this.m_standardBlock = new FilePassStandardBlock();

  public override bool NeedDecoding => false;

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_usNotWeakEncryption = provider != null ? provider.ReadUInt16(iOffset) : throw new ArgumentNullException(nameof (provider));
    iOffset += 2;
    this.m_usKey = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usHash = provider.ReadUInt16(iOffset);
    iOffset += 2;
    if (this.IsWeakEncryption)
      return;
    switch (this.m_usHash)
    {
      case 1:
        this.m_standardBlock = new FilePassStandardBlock();
        this.m_standardBlock.ParseStructure(provider, iOffset, iLength);
        break;
      case 2:
        this.m_strongBlock = new FilePassStrongBlock();
        this.m_strongBlock.ParseStructure(provider, iOffset, iLength);
        break;
      default:
        throw new ParseException("Cannot parse FilePass record");
    }
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteUInt16(iOffset, this.m_usNotWeakEncryption);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usKey);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usHash);
    iOffset += 2;
    if (this.IsWeakEncryption)
      return;
    if (this.m_usHash != (ushort) 1)
      throw new NotImplementedException();
    this.m_standardBlock.InfillInternalData(provider, iOffset, int.MaxValue);
  }

  public override int GetStoreSize(OfficeVersion version)
  {
    int storeSize = 6;
    if (!this.IsWeakEncryption)
    {
      if (this.m_usHash != (ushort) 1)
        throw new NotImplementedException();
      storeSize += FilePassStandardBlock.GetStoreSize(version);
    }
    return storeSize;
  }
}
