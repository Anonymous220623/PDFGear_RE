// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.FilePassStrongBlock
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[CLSCompliant(false)]
internal class FilePassStrongBlock
{
  private uint m_uiOptions;
  private uint m_uiReserved;
  private uint m_uiStreamEncryption;
  private uint m_uiPassword;
  private uint m_uiHashKeyLength;
  private uint m_uiCryptographicProvider;
  private byte[] m_arrUnknown = new byte[8];
  private string m_strProviderName;
  private byte[] m_arrDocumentId;
  private byte[] m_arrEncryptedDocumentId;
  private byte[] m_arrDigest;

  public uint Options
  {
    get => this.m_uiOptions;
    set => this.m_uiOptions = value;
  }

  public uint Reserved
  {
    get => this.m_uiReserved;
    set => this.m_uiReserved = value;
  }

  public uint StreamEncryption
  {
    get => this.m_uiStreamEncryption;
    set => this.m_uiStreamEncryption = value;
  }

  public uint Password
  {
    get => this.m_uiPassword;
    set => this.m_uiPassword = value;
  }

  public uint HashKeyLength
  {
    get => this.m_uiHashKeyLength;
    set => this.m_uiHashKeyLength = value;
  }

  public uint CryptographicProvider
  {
    get => this.m_uiCryptographicProvider;
    set => this.m_uiCryptographicProvider = value;
  }

  public byte[] UnknownData => this.m_arrUnknown;

  public string ProviderName
  {
    get => this.m_strProviderName;
    set => this.m_strProviderName = value;
  }

  public byte[] Digest => this.m_arrDigest;

  public int ParseStructure(DataProvider provider, int iOffset, int iLength)
  {
    this.m_uiOptions = provider != null ? provider.ReadUInt32(iOffset) : throw new ArgumentNullException(nameof (provider));
    iOffset += 4;
    uint num1 = provider.ReadUInt32(iOffset) + 4U;
    int num2 = iOffset;
    this.m_uiOptions = provider.ReadUInt32(iOffset);
    iOffset += 4;
    this.m_uiReserved = provider.ReadUInt32(iOffset);
    iOffset += 4;
    this.m_uiStreamEncryption = provider.ReadUInt32(iOffset);
    iOffset += 4;
    this.m_uiPassword = provider.ReadUInt32(iOffset);
    iOffset += 4;
    this.m_uiHashKeyLength = provider.ReadUInt32(iOffset);
    iOffset += 4;
    this.m_uiCryptographicProvider = provider.ReadUInt32(iOffset);
    iOffset += 4;
    iOffset = provider.ReadArray(iOffset, this.m_arrUnknown);
    iOffset = num2 + (int) num1;
    uint length = provider.ReadUInt32(iOffset) + 4U;
    this.m_arrDocumentId = new byte[(IntPtr) length];
    this.m_arrEncryptedDocumentId = new byte[(IntPtr) length];
    iOffset = provider.ReadArray(iOffset, this.m_arrDocumentId);
    iOffset = provider.ReadArray(iOffset, this.m_arrEncryptedDocumentId);
    uint num3 = provider.ReadUInt32(iOffset) + 4U;
    throw new NotSupportedException("Strong encryption algorithms are not supported.");
  }
}
