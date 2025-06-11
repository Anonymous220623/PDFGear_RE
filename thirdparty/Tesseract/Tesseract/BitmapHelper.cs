// Decompiled with JetBrains decompiler
// Type: Tesseract.BitmapHelper
// Assembly: Tesseract, Version=4.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: C5D5562D-D917-402B-968F-9F8B28C3D951
// Assembly location: D:\PDFGear\bin\Tesseract.dll

#nullable disable
namespace Tesseract;

public static class BitmapHelper
{
  public static unsafe byte GetDataBit(byte* data, int index)
  {
    return (byte) ((int) data[index >> 3] >> (index & 7) & 1);
  }

  public static unsafe void SetDataBit(byte* data, int index, byte value)
  {
    byte* numPtr = data + (index >> 3);
    *numPtr = (byte) ((uint) *numPtr & (uint) (byte) ~(128 /*0x80*/ >> (index & 7)));
    *numPtr = (byte) ((uint) *numPtr | (uint) (byte) (((int) value & 1) << 7 - (index & 7)));
  }

  public static unsafe byte GetDataQBit(byte* data, int index)
  {
    return (byte) ((int) data[index >> 1] >> 4 * (index & 1) & 15);
  }

  public static unsafe void SetDataQBit(byte* data, int index, byte value)
  {
    byte* numPtr = data + (index >> 1);
    *numPtr = (byte) ((uint) *numPtr & (uint) (byte) ~(240 /*0xF0*/ >> 4 * (index & 1)));
    *numPtr = (byte) ((uint) *numPtr | (uint) (byte) (((int) value & 15) << 4 - 4 * (index & 1)));
  }

  public static unsafe byte GetDataByte(byte* data, int index) => data[index];

  public static unsafe void SetDataByte(byte* data, int index, byte value) => data[index] = value;

  public static unsafe ushort GetDataUInt16(ushort* data, int index) => data[index];

  public static unsafe void SetDataUInt16(ushort* data, int index, ushort value)
  {
    data[index] = value;
  }

  public static unsafe uint GetDataUInt32(uint* data, int index) => data[index];

  public static unsafe void SetDataUInt32(uint* data, int index, uint value) => data[index] = value;

  public static uint ConvertRgb555ToRGBA(uint val)
  {
    uint num1 = (val & 31744U) >> 10;
    uint num2 = (val & 992U) >> 5;
    uint num3 = val & 31U /*0x1F*/;
    return (uint) (((int) num1 << 3 | (int) (num1 >> 2)) << 24 | ((int) num2 << 3 | (int) (num2 >> 2)) << 16 /*0x10*/ | ((int) num3 << 3 | (int) (num3 >> 2)) << 8 | (int) byte.MaxValue);
  }

  public static uint ConvertRgb565ToRGBA(uint val)
  {
    uint num1 = (val & 63488U) >> 11;
    uint num2 = (val & 2016U) >> 5;
    uint num3 = val & 31U /*0x1F*/;
    return (uint) (((int) num1 << 3 | (int) (num1 >> 2)) << 24 | ((int) num2 << 2 | (int) (num2 >> 4)) << 16 /*0x10*/ | ((int) num3 << 3 | (int) (num3 >> 2)) << 8 | (int) byte.MaxValue);
  }

  public static uint ConvertArgb1555ToRGBA(uint val)
  {
    uint num1 = (val & 32768U /*0x8000*/) >> 15;
    uint num2 = (val & 31744U) >> 10;
    uint num3 = (val & 992U) >> 5;
    uint num4 = val & 31U /*0x1F*/;
    return (uint) (((int) num2 << 3 | (int) (num2 >> 2)) << 24 | ((int) num3 << 3 | (int) (num3 >> 2)) << 16 /*0x10*/ | ((int) num4 << 3 | (int) (num4 >> 2)) << 8 | ((int) num1 << 8) - (int) num1);
  }

  public static uint EncodeAsRGBA(byte red, byte green, byte blue, byte alpha)
  {
    return (uint) ((int) red << 24 | (int) green << 16 /*0x10*/ | (int) blue << 8) | (uint) alpha;
  }
}
