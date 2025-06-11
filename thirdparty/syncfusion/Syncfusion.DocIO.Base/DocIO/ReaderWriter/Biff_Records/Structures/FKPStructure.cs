// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures.FKPStructure
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;

[StructLayout(LayoutKind.Sequential)]
internal class FKPStructure : IDataStructure
{
  internal const int DEF_RECORD_SIZE = 512 /*0x0200*/;
  [MarshalAs(UnmanagedType.ByValArray, SizeConst = 511 /*0x01FF*/)]
  private byte[] m_arrPageData = new byte[511 /*0x01FF*/];
  private byte m_btLength;

  internal FKPStructure()
  {
  }

  internal FKPStructure(Stream stream)
  {
    stream.Read(this.m_arrPageData, 0, 511 /*0x01FF*/);
    this.m_btLength = (byte) stream.ReadByte();
  }

  internal byte[] PageData => this.m_arrPageData;

  internal byte Count
  {
    get => this.m_btLength;
    set
    {
      if ((int) this.m_btLength == (int) value)
        return;
      this.m_btLength = value;
    }
  }

  public int Length => 512 /*0x0200*/;

  public void Parse(byte[] arrData, int iOffset)
  {
    this.m_arrPageData = ByteConverter.ReadBytes(arrData, this.m_arrPageData.Length, ref iOffset);
    this.m_btLength = arrData[iOffset];
    ++iOffset;
  }

  public int Save(byte[] arrData, int iOffset)
  {
    if (arrData == null)
      throw new ArgumentNullException(nameof (arrData));
    if (iOffset < 0 || iOffset + 512 /*0x0200*/ > arrData.Length)
      throw new ArgumentOutOfRangeException(nameof (iOffset));
    this.m_arrPageData.CopyTo((Array) arrData, iOffset);
    iOffset += this.m_arrPageData.Length;
    arrData[iOffset] = this.m_btLength;
    return ++iOffset;
  }

  internal int Save(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    stream.Write(this.m_arrPageData, 0, this.m_arrPageData.Length);
    stream.WriteByte(this.m_btLength);
    return (int) stream.Position;
  }

  internal void Close()
  {
    if (this.m_arrPageData == null)
      return;
    this.m_arrPageData = (byte[]) null;
  }
}
