// Decompiled with JetBrains decompiler
// Type: Syncfusion.Compression.ChecksumCalculator
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System;

#nullable disable
namespace Syncfusion.Compression;

public class ChecksumCalculator
{
  private const int DEF_CHECKSUM_BIT_OFFSET = 16 /*0x10*/;
  private const int DEF_CHECKSUM_BASE = 65521;
  private const int DEF_CHECKSUM_ITERATIONSCOUNT = 3800;

  public static void ChecksumUpdate(ref long checksum, byte[] buffer, int offset, int length)
  {
    uint num1 = (uint) checksum;
    uint num2 = num1 & (uint) ushort.MaxValue;
    uint num3 = num1 >> 16 /*0x10*/;
    while (length > 0)
    {
      int num4 = Math.Min(length, 3800);
      length -= num4;
      while (--num4 >= 0)
      {
        num2 += (uint) buffer[offset++] & (uint) byte.MaxValue;
        num3 += num2;
      }
      num2 %= 65521U;
      num3 %= 65521U;
    }
    uint num5 = num3 << 16 /*0x10*/ | num2;
    checksum = (long) num5;
  }

  public static long ChecksumGenerate(byte[] buffer, int offset, int length)
  {
    long checksum = 1;
    ChecksumCalculator.ChecksumUpdate(ref checksum, buffer, offset, length);
    return checksum;
  }
}
