// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.DocIO.Net.FileHeader
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.CompoundFile.DocIO.Net;

internal class FileHeader
{
  public const int HeaderSize = 512 /*0x0200*/;
  private const int SignatureSize = 8;
  internal const int ShortSize = 2;
  internal const int IntSize = 4;
  private static readonly byte[] DefaultSignature = new byte[8]
  {
    (byte) 208 /*0xD0*/,
    (byte) 207,
    (byte) 17,
    (byte) 224 /*0xE0*/,
    (byte) 161,
    (byte) 177,
    (byte) 26,
    (byte) 225
  };
  private byte[] m_arrSignature = new byte[8];
  private Guid m_classId = new Guid();
  private ushort m_usMinorVersion = 62;
  private ushort m_usDllVersion = 3;
  private ushort m_usByteOrder = 65534;
  private ushort m_usSectorShift = 9;
  private ushort m_usMiniSectorShift = 6;
  private ushort m_usReserved;
  private uint m_uiReserved1;
  private uint m_uiReserved2;
  private int m_iFatSectorsNumber;
  private int m_iDirectorySectorStart = -1;
  private int m_iSignature;
  private uint m_uiMiniSectorCutoff = 4096 /*0x1000*/;
  private int m_iMiniFastStart = -2;
  private int m_iMiniFatNumber;
  private int m_iDifStart = -2;
  private int m_iDifNumber;
  private int[] m_arrFatStart = new int[109];

  public FileHeader()
  {
    Buffer.BlockCopy((Array) FileHeader.DefaultSignature, 0, (Array) this.m_arrSignature, 0, 8);
  }

  public FileHeader(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    if (stream.Length < 512L /*0x0200*/)
      throw new CompoundFileException();
    byte[] numArray1 = new byte[512 /*0x0200*/];
    stream.Read(numArray1, 0, 512 /*0x0200*/);
    Buffer.BlockCopy((Array) numArray1, 0, (Array) this.m_arrSignature, 0, 8);
    this.CheckSignature();
    int srcOffset1 = 8;
    byte[] numArray2 = new byte[16 /*0x10*/];
    Buffer.BlockCopy((Array) numArray1, srcOffset1, (Array) numArray2, 0, 16 /*0x10*/);
    int startIndex1 = srcOffset1 + 16 /*0x10*/;
    this.m_classId = new Guid(numArray2);
    this.m_usMinorVersion = BitConverter.ToUInt16(numArray1, startIndex1);
    int startIndex2 = startIndex1 + 2;
    this.m_usDllVersion = BitConverter.ToUInt16(numArray1, startIndex2);
    int startIndex3 = startIndex2 + 2;
    this.m_usByteOrder = BitConverter.ToUInt16(numArray1, startIndex3);
    int startIndex4 = startIndex3 + 2;
    this.m_usSectorShift = BitConverter.ToUInt16(numArray1, startIndex4);
    int startIndex5 = startIndex4 + 2;
    this.m_usMiniSectorShift = BitConverter.ToUInt16(numArray1, startIndex5);
    int startIndex6 = startIndex5 + 2;
    this.m_usReserved = BitConverter.ToUInt16(numArray1, startIndex6);
    int startIndex7 = startIndex6 + 2;
    this.m_uiReserved1 = BitConverter.ToUInt32(numArray1, startIndex7);
    int startIndex8 = startIndex7 + 4;
    this.m_uiReserved2 = BitConverter.ToUInt32(numArray1, startIndex8);
    int startIndex9 = startIndex8 + 4;
    this.m_iFatSectorsNumber = BitConverter.ToInt32(numArray1, startIndex9);
    int startIndex10 = startIndex9 + 4;
    this.m_iDirectorySectorStart = BitConverter.ToInt32(numArray1, startIndex10);
    int startIndex11 = startIndex10 + 4;
    this.m_iSignature = BitConverter.ToInt32(numArray1, startIndex11);
    int startIndex12 = startIndex11 + 4;
    this.m_uiMiniSectorCutoff = BitConverter.ToUInt32(numArray1, startIndex12);
    int startIndex13 = startIndex12 + 4;
    this.m_iMiniFastStart = BitConverter.ToInt32(numArray1, startIndex13);
    int startIndex14 = startIndex13 + 4;
    this.m_iMiniFatNumber = BitConverter.ToInt32(numArray1, startIndex14);
    int startIndex15 = startIndex14 + 4;
    this.m_iDifStart = BitConverter.ToInt32(numArray1, startIndex15);
    int startIndex16 = startIndex15 + 4;
    this.m_iDifNumber = BitConverter.ToInt32(numArray1, startIndex16);
    int srcOffset2 = startIndex16 + 4;
    Buffer.BlockCopy((Array) numArray1, srcOffset2, (Array) this.m_arrFatStart, 0, this.m_arrFatStart.Length * 4);
  }

  public void Serialize(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    byte[] numArray = new byte[512 /*0x0200*/];
    Buffer.BlockCopy((Array) this.m_arrSignature, 0, (Array) numArray, 0, 8);
    int dstOffset1 = 8;
    Buffer.BlockCopy((Array) this.m_classId.ToByteArray(), 0, (Array) numArray, dstOffset1, 16 /*0x10*/);
    int offset1 = dstOffset1 + 16 /*0x10*/;
    this.WriteUInt16(numArray, offset1, this.m_usMinorVersion);
    int offset2 = offset1 + 2;
    this.WriteUInt16(numArray, offset2, this.m_usDllVersion);
    int offset3 = offset2 + 2;
    this.WriteUInt16(numArray, offset3, this.m_usByteOrder);
    int offset4 = offset3 + 2;
    this.WriteUInt16(numArray, offset4, this.m_usSectorShift);
    int offset5 = offset4 + 2;
    this.WriteUInt16(numArray, offset5, this.m_usMiniSectorShift);
    int offset6 = offset5 + 2;
    this.WriteUInt16(numArray, offset6, this.m_usReserved);
    int offset7 = offset6 + 2;
    this.WriteUInt32(numArray, offset7, this.m_uiReserved1);
    int offset8 = offset7 + 4;
    this.WriteUInt32(numArray, offset8, this.m_uiReserved2);
    int offset9 = offset8 + 4;
    this.WriteInt32(numArray, offset9, this.m_iFatSectorsNumber);
    int offset10 = offset9 + 4;
    this.WriteInt32(numArray, offset10, this.m_iDirectorySectorStart);
    int offset11 = offset10 + 4;
    this.WriteInt32(numArray, offset11, this.m_iSignature);
    int offset12 = offset11 + 4;
    this.WriteUInt32(numArray, offset12, this.m_uiMiniSectorCutoff);
    int offset13 = offset12 + 4;
    this.WriteInt32(numArray, offset13, this.m_iMiniFastStart);
    int offset14 = offset13 + 4;
    this.WriteInt32(numArray, offset14, this.m_iMiniFatNumber);
    int offset15 = offset14 + 4;
    this.WriteInt32(numArray, offset15, this.m_iDifStart);
    int offset16 = offset15 + 4;
    this.WriteInt32(numArray, offset16, this.m_iDifNumber);
    int dstOffset2 = offset16 + 4;
    Buffer.BlockCopy((Array) this.m_arrFatStart, 0, (Array) numArray, dstOffset2, this.m_arrFatStart.Length * 4);
    stream.Write(numArray, 0, 512 /*0x0200*/);
  }

  public static bool CheckSignature(Stream stream)
  {
    bool flag = false;
    if (stream != null)
    {
      byte[] numArray = new byte[8];
      long position = stream.Position;
      if (stream.Read(numArray, 0, 8) == 8)
        flag = FileHeader.CheckSignature(numArray);
      stream.Position = position;
    }
    return flag;
  }

  private void CheckSignature()
  {
    if (!FileHeader.CheckSignature(this.m_arrSignature))
      throw new CompoundFileException("Wrong signature");
  }

  private static bool CheckSignature(byte[] arrSignature)
  {
    bool flag = false;
    if (arrSignature != null && arrSignature.Length == 8)
    {
      flag = true;
      for (int index = 0; index < 8; ++index)
      {
        if ((int) arrSignature[index] != (int) FileHeader.DefaultSignature[index])
        {
          flag = false;
          break;
        }
      }
    }
    return flag;
  }

  private void WriteUInt16(byte[] buffer, int offset, ushort value)
  {
    if (buffer == null)
      throw new ArgumentNullException(nameof (buffer));
    buffer[offset] = (byte) ((uint) value & (uint) byte.MaxValue);
    buffer[offset + 1] = (byte) (((int) value & 65280) >> 8);
  }

  private void WriteUInt32(byte[] buffer, int offset, uint value)
  {
    if (buffer == null)
      throw new ArgumentNullException(nameof (buffer));
    buffer[offset] = (byte) (value & (uint) byte.MaxValue);
    value >>= 8;
    buffer[offset + 1] = (byte) (value & (uint) byte.MaxValue);
    value >>= 8;
    buffer[offset + 2] = (byte) (value & (uint) byte.MaxValue);
    value >>= 8;
    buffer[offset + 3] = (byte) (value & (uint) byte.MaxValue);
    value >>= 8;
  }

  private void WriteInt32(byte[] buffer, int offset, int value)
  {
    if (buffer == null)
      throw new ArgumentNullException(nameof (buffer));
    buffer[offset] = (byte) (value & (int) byte.MaxValue);
    value >>= 8;
    buffer[offset + 1] = (byte) (value & (int) byte.MaxValue);
    value >>= 8;
    buffer[offset + 2] = (byte) (value & (int) byte.MaxValue);
    value >>= 8;
    buffer[offset + 3] = (byte) (value & (int) byte.MaxValue);
    value >>= 8;
  }

  public int SectorSize => 1 << (int) this.m_usSectorShift;

  public ushort MinorVersion => this.m_usMinorVersion;

  public ushort DllVersion => this.m_usDllVersion;

  public ushort ByteOrder => this.m_usByteOrder;

  public ushort SectorShift => this.m_usSectorShift;

  public ushort MiniSectorShift => this.m_usMiniSectorShift;

  public ushort Reserved => this.m_usReserved;

  public uint Reserved1 => this.m_uiReserved1;

  public uint Reserved2 => this.m_uiReserved2;

  public int FatSectorsNumber
  {
    get => this.m_iFatSectorsNumber;
    set => this.m_iFatSectorsNumber = value;
  }

  public int DirectorySectorStart
  {
    get => this.m_iDirectorySectorStart;
    set => this.m_iDirectorySectorStart = value;
  }

  public int Signature => this.m_iSignature;

  public uint MiniSectorCutoff => this.m_uiMiniSectorCutoff;

  public int MiniFastStart
  {
    get => this.m_iMiniFastStart;
    set => this.m_iMiniFastStart = value;
  }

  public int MiniFatNumber
  {
    get => this.m_iMiniFatNumber;
    set => this.m_iMiniFatNumber = value;
  }

  public int DifStart
  {
    get => this.m_iDifStart;
    set => this.m_iDifStart = value;
  }

  public int DifNumber
  {
    get => this.m_iDifNumber;
    set => this.m_iDifNumber = value;
  }

  public int[] FatStart => this.m_arrFatStart;

  internal void Write(Stream stream)
  {
    byte[] numArray = new byte[512 /*0x0200*/];
    Buffer.BlockCopy((Array) this.m_arrSignature, 0, (Array) numArray, 0, 8);
    int dstOffset1 = 8;
    Buffer.BlockCopy((Array) this.m_classId.ToByteArray(), 0, (Array) numArray, dstOffset1, 16 /*0x10*/);
    int dstOffset2 = dstOffset1 + 16 /*0x10*/;
    Buffer.BlockCopy((Array) BitConverter.GetBytes(this.m_usMinorVersion), 0, (Array) numArray, dstOffset2, 2);
    int dstOffset3 = dstOffset2 + 2;
    Buffer.BlockCopy((Array) BitConverter.GetBytes(this.m_usDllVersion), 0, (Array) numArray, dstOffset3, 2);
    int dstOffset4 = dstOffset3 + 2;
    Buffer.BlockCopy((Array) BitConverter.GetBytes(this.m_usByteOrder), 0, (Array) numArray, dstOffset4, 2);
    int dstOffset5 = dstOffset4 + 2;
    Buffer.BlockCopy((Array) BitConverter.GetBytes(this.m_usSectorShift), 0, (Array) numArray, dstOffset5, 2);
    int dstOffset6 = dstOffset5 + 2;
    Buffer.BlockCopy((Array) BitConverter.GetBytes(this.m_usMiniSectorShift), 0, (Array) numArray, dstOffset6, 2);
    int dstOffset7 = dstOffset6 + 2;
    Buffer.BlockCopy((Array) BitConverter.GetBytes(this.m_usReserved), 0, (Array) numArray, dstOffset7, 2);
    int dstOffset8 = dstOffset7 + 2;
    Buffer.BlockCopy((Array) BitConverter.GetBytes(this.m_uiReserved1), 0, (Array) numArray, dstOffset8, 4);
    int dstOffset9 = dstOffset8 + 4;
    Buffer.BlockCopy((Array) BitConverter.GetBytes(this.m_uiReserved2), 0, (Array) numArray, dstOffset9, 4);
    int dstOffset10 = dstOffset9 + 4;
    Buffer.BlockCopy((Array) BitConverter.GetBytes(this.m_iFatSectorsNumber), 0, (Array) numArray, dstOffset10, 4);
    int dstOffset11 = dstOffset10 + 4;
    Buffer.BlockCopy((Array) BitConverter.GetBytes(this.m_iDirectorySectorStart), 0, (Array) numArray, dstOffset11, 4);
    int dstOffset12 = dstOffset11 + 4;
    Buffer.BlockCopy((Array) BitConverter.GetBytes(this.m_iSignature), 0, (Array) numArray, dstOffset12, 4);
    int dstOffset13 = dstOffset12 + 4;
    Buffer.BlockCopy((Array) BitConverter.GetBytes(this.m_uiMiniSectorCutoff), 0, (Array) numArray, dstOffset13, 4);
    int dstOffset14 = dstOffset13 + 4;
    Buffer.BlockCopy((Array) BitConverter.GetBytes(this.m_iMiniFastStart), 0, (Array) numArray, dstOffset14, 4);
    int dstOffset15 = dstOffset14 + 4;
    Buffer.BlockCopy((Array) BitConverter.GetBytes(this.m_iMiniFatNumber), 0, (Array) numArray, dstOffset15, 4);
    int dstOffset16 = dstOffset15 + 4;
    Buffer.BlockCopy((Array) BitConverter.GetBytes(this.m_iDifStart), 0, (Array) numArray, dstOffset16, 4);
    int dstOffset17 = dstOffset16 + 4;
    Buffer.BlockCopy((Array) BitConverter.GetBytes(this.m_iDifNumber), 0, (Array) numArray, dstOffset17, 4);
    int dstOffset18 = dstOffset17 + 4;
    Buffer.BlockCopy((Array) this.m_arrFatStart, 0, (Array) numArray, dstOffset18, this.m_arrFatStart.Length * 4);
    stream.Position = 0L;
    stream.Write(numArray, 0, 512 /*0x0200*/);
  }

  internal long GetSectorOffset(int sectorIndex)
  {
    return (long) ((sectorIndex << (int) this.m_usSectorShift) + 512 /*0x0200*/);
  }

  internal long GetSectorOffset(int sectorIndex, int headerSize)
  {
    return (long) ((sectorIndex << (int) this.m_usSectorShift) + headerSize);
  }
}
