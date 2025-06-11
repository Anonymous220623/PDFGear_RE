// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures.ByteConverter
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;

internal static class ByteConverter
{
  internal static short ReadInt16(byte[] arrData, ref int iOffset)
  {
    short int16 = BitConverter.ToInt16(arrData, iOffset);
    iOffset += 2;
    return int16;
  }

  internal static int ReadInt32(byte[] arrData, ref int iOffset)
  {
    int int32 = BitConverter.ToInt32(arrData, iOffset);
    iOffset += 4;
    return int32;
  }

  internal static long ReadInt64(byte[] arrData, ref int iOffset)
  {
    long int64 = BitConverter.ToInt64(arrData, iOffset);
    iOffset += 8;
    return int64;
  }

  internal static ushort ReadUInt16(byte[] arrData, ref int iOffset)
  {
    ushort uint16 = BitConverter.ToUInt16(arrData, iOffset);
    iOffset += 2;
    return uint16;
  }

  internal static uint ReadUInt32(byte[] arrData, ref int iOffset)
  {
    uint uint32 = BitConverter.ToUInt32(arrData, iOffset);
    iOffset += 4;
    return uint32;
  }

  internal static byte[] ReadBytes(byte[] arrData, int length, ref int iOffset)
  {
    byte[] numArray = new byte[length];
    for (int index = 0; index < length && iOffset + index < arrData.Length; ++index)
      numArray[index] = arrData[iOffset + index];
    iOffset += length;
    return numArray;
  }

  internal static void WriteInt16(byte[] destination, ref int iOffset, short val)
  {
    byte[] bytes = BitConverter.GetBytes(val);
    ByteConverter.WriteBytes(destination, ref iOffset, bytes);
  }

  internal static void WriteUInt16(byte[] destination, ref int iOffset, ushort val)
  {
    byte[] bytes = BitConverter.GetBytes(val);
    ByteConverter.WriteBytes(destination, ref iOffset, bytes);
  }

  internal static void WriteInt32(byte[] destination, ref int iOffset, int val)
  {
    byte[] bytes = BitConverter.GetBytes(val);
    ByteConverter.WriteBytes(destination, ref iOffset, bytes);
  }

  internal static void WriteInt64(byte[] destination, ref int iOffset, long val)
  {
    byte[] bytes = BitConverter.GetBytes(val);
    ByteConverter.WriteBytes(destination, ref iOffset, bytes);
  }

  internal static void WriteUInt32(byte[] destination, ref int iOffset, uint val)
  {
    byte[] bytes = BitConverter.GetBytes(val);
    ByteConverter.WriteBytes(destination, ref iOffset, bytes);
  }

  internal static void WriteBytes(byte[] destination, ref int iOffset, byte[] bytes)
  {
    int length = bytes.Length;
    for (int index = 0; index < length; ++index)
      destination[iOffset + index] = bytes[index];
    iOffset += length;
  }

  internal static void CopyMemory(byte[] destination, byte[] source, int length)
  {
    for (int index = 0; index < length; ++index)
      destination[index] = source[index];
  }
}
