// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.DataStructure
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

[StructLayout(LayoutKind.Sequential)]
internal abstract class DataStructure
{
  public const int FileCharPosSize = 4;
  public const int DiskPageSize = 512 /*0x0200*/;

  internal abstract void Parse(byte[] arrData, int iOffset);

  internal abstract int Save(byte[] arrData, int iOffset);

  internal abstract int Length { get; }

  internal static short ReadInt16(byte[] arrData, ref int iOffset)
  {
    short int16 = BitConverter.ToInt16(arrData, iOffset);
    iOffset += 16 /*0x10*/;
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
    iOffset += 16 /*0x10*/;
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
    byte[] dst = new byte[length];
    Buffer.BlockCopy((Array) arrData, iOffset, (Array) dst, 0, length);
    iOffset += length;
    return dst;
  }

  internal static void WriteInt16(byte[] destination, ref int iOffset, short val)
  {
    byte[] bytes = BitConverter.GetBytes(val);
    DataStructure.WriteBytes(destination, ref iOffset, bytes);
  }

  internal static void WriteUInt16(byte[] destination, ref int iOffset, ushort val)
  {
    byte[] bytes = BitConverter.GetBytes(val);
    DataStructure.WriteBytes(destination, ref iOffset, bytes);
  }

  internal static void WriteInt32(byte[] destination, ref int iOffset, int val)
  {
    byte[] bytes = BitConverter.GetBytes(val);
    DataStructure.WriteBytes(destination, ref iOffset, bytes);
  }

  internal static void WriteInt64(byte[] destination, ref int iOffset, long val)
  {
    byte[] bytes = BitConverter.GetBytes(val);
    DataStructure.WriteBytes(destination, ref iOffset, bytes);
  }

  internal static void WriteUInt32(byte[] destination, ref int iOffset, uint val)
  {
    byte[] bytes = BitConverter.GetBytes(val);
    DataStructure.WriteBytes(destination, ref iOffset, bytes);
  }

  internal static void WriteBytes(byte[] destination, ref int iOffset, byte[] bytes)
  {
    int length = bytes.Length;
    Buffer.BlockCopy((Array) bytes, 0, (Array) destination, iOffset, length);
    iOffset += length;
  }
}
