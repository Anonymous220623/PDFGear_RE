// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontBitsHelper
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal static class SystemFontBitsHelper
{
  internal static bool GetBit(int n, byte bit) => (n & 1 << (int) bit) != 0;

  internal static byte[] GetBits(byte bt)
  {
    byte[] bits = new byte[8];
    for (int index = 0; index < 8; ++index)
    {
      bits[index] = (byte) ((uint) bt % 2U);
      bt /= (byte) 2;
    }
    return bits;
  }

  internal static byte ToByte(byte[] bits, int offset, int count)
  {
    byte num1 = 0;
    int num2 = 1;
    for (int index = 0; index < count; ++index)
    {
      num1 += (byte) ((uint) bits[offset + index] * (uint) num2);
      num2 *= 2;
    }
    return num1;
  }
}
