// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.FilePassStandardBlock
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

internal class FilePassStandardBlock
{
  public const int StoreSize = 48 /*0x30*/;
  private byte[] m_arrDocumentID = new byte[16 /*0x10*/];
  private byte[] m_arrEncyptedDocumentID = new byte[16 /*0x10*/];
  private byte[] m_arrDigest = new byte[16 /*0x10*/];

  public byte[] DocumentID => this.m_arrDocumentID;

  public byte[] EncyptedDocumentID => this.m_arrEncyptedDocumentID;

  public byte[] Digest => this.m_arrDigest;

  public void ParseStructure(DataProvider provider, int iOffset, int iLength)
  {
    if (provider == null)
      throw new ArgumentNullException(nameof (provider));
    iOffset = provider.ReadArray(iOffset, this.m_arrDocumentID);
    iOffset = provider.ReadArray(iOffset, this.m_arrEncyptedDocumentID);
    iOffset = provider.ReadArray(iOffset, this.m_arrDigest);
  }

  public void InfillInternalData(DataProvider provider, int iOffset, int iLength)
  {
    if (provider == null)
      throw new ArgumentNullException(nameof (provider));
    int length1 = this.m_arrDocumentID.Length;
    provider.WriteBytes(iOffset, this.m_arrDocumentID, 0, length1);
    iOffset += length1;
    int length2 = this.m_arrEncyptedDocumentID.Length;
    provider.WriteBytes(iOffset, this.m_arrEncyptedDocumentID, 0, length2);
    iOffset += length2;
    int length3 = this.m_arrDigest.Length;
    provider.WriteBytes(iOffset, this.m_arrDigest, 0, length3);
    iOffset += length3;
  }

  public static int GetStoreSize(OfficeVersion version) => 48 /*0x30*/;
}
