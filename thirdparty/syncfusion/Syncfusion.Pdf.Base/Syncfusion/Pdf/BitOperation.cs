// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.BitOperation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class BitOperation
{
  public const int LEFT_SHIFT = 0;
  public const int RIGHT_SHIFT = 1;

  public int GetInt32(short[] number)
  {
    return (int) number[0] << 24 | (int) number[1] << 16 /*0x10*/ | (int) number[2] << 8 | (int) number[3];
  }

  public int GetInt16(short[] number) => (int) number[0] << 8 | (int) number[1];

  public long Bit32Shift(long number, int shift, int direction)
  {
    if (direction == 0)
      number <<= shift;
    else
      number >>= shift;
    long maxValue = (long) uint.MaxValue;
    return number & maxValue;
  }

  public int Bit8Shift(int number, int shift, int direction)
  {
    if (direction == 0)
      number <<= shift;
    else
      number >>= shift;
    int maxValue = (int) byte.MaxValue;
    return number & maxValue;
  }
}
