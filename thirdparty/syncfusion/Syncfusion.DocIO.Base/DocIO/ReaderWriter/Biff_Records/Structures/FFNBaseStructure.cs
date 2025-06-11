// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures.FFNBaseStructure
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;

[CLSCompliant(false)]
[StructLayout(LayoutKind.Sequential)]
internal class FFNBaseStructure : IDataStructure
{
  private const int DEF_RECORD_LENGTH = 40;
  private const int DEF_PANOSE_SIZE = 10;
  private const int DEF_FONTSIGNATURE_SIZE = 24;
  private byte m_btTotalLength;
  private byte m_btOptions;
  private short m_wWeight;
  private byte m_btCharacterSetId;
  private byte m_btAlternateFontIndex;
  [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
  private byte[] m_PANOSE = new byte[10];
  [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
  internal byte[] m_FONTSIGNATURE = new byte[24];

  internal byte TotalLengthM1
  {
    get => this.m_btTotalLength;
    set
    {
      if ((int) value == (int) this.m_btTotalLength)
        return;
      this.m_btTotalLength = value;
    }
  }

  internal byte Options
  {
    get => this.m_btOptions;
    set
    {
      if ((int) value == (int) this.m_btOptions)
        return;
      this.m_btOptions = value;
    }
  }

  internal short Weight
  {
    get => this.m_wWeight;
    set
    {
      if ((int) value == (int) this.m_wWeight)
        return;
      this.m_wWeight = value;
    }
  }

  internal byte CharacterSetId
  {
    get => this.m_btCharacterSetId;
    set
    {
      if ((int) value == (int) this.m_btCharacterSetId)
        return;
      this.m_btCharacterSetId = value;
    }
  }

  internal byte AlternateFontIndex
  {
    get => this.m_btAlternateFontIndex;
    set
    {
      if ((int) value == (int) this.m_btAlternateFontIndex)
        return;
      this.m_btAlternateFontIndex = value;
    }
  }

  public int Length => 40;

  internal void Close()
  {
    this.m_FONTSIGNATURE = (byte[]) null;
    this.m_PANOSE = (byte[]) null;
  }

  public void Parse(byte[] arrData, int iOffset)
  {
    this.m_btTotalLength = arrData[iOffset];
    ++iOffset;
    this.m_btOptions = arrData[iOffset];
    ++iOffset;
    this.m_wWeight = ByteConverter.ReadInt16(arrData, ref iOffset);
    this.m_btCharacterSetId = arrData[iOffset];
    ++iOffset;
    this.m_btAlternateFontIndex = arrData[iOffset];
    ++iOffset;
    this.m_PANOSE = ByteConverter.ReadBytes(arrData, 10, ref iOffset);
    this.m_FONTSIGNATURE = ByteConverter.ReadBytes(arrData, 24, ref iOffset);
  }

  public int Save(byte[] arrData, int iOffset)
  {
    arrData[iOffset] = this.m_btTotalLength;
    ++iOffset;
    arrData[iOffset] = this.m_btOptions;
    ++iOffset;
    ByteConverter.WriteInt16(arrData, ref iOffset, this.m_wWeight);
    arrData[iOffset] = this.m_btCharacterSetId;
    ++iOffset;
    arrData[iOffset] = this.m_btAlternateFontIndex;
    ++iOffset;
    ByteConverter.WriteBytes(arrData, ref iOffset, this.m_PANOSE);
    ByteConverter.WriteBytes(arrData, ref iOffset, this.m_FONTSIGNATURE);
    return 40;
  }
}
