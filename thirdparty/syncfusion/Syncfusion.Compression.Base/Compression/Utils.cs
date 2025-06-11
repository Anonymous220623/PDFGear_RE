// Decompiled with JetBrains decompiler
// Type: Syncfusion.Compression.Utils
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

#nullable disable
namespace Syncfusion.Compression;

public class Utils
{
  private static readonly byte[] DEF_REVERSE_BITS = new byte[16 /*0x10*/]
  {
    (byte) 0,
    (byte) 8,
    (byte) 4,
    (byte) 12,
    (byte) 2,
    (byte) 10,
    (byte) 6,
    (byte) 14,
    (byte) 1,
    (byte) 9,
    (byte) 5,
    (byte) 13,
    (byte) 3,
    (byte) 11,
    (byte) 7,
    (byte) 15
  };
  public static int[] DEF_HUFFMAN_DYNTREE_CODELENGTHS_ORDER = new int[19]
  {
    16 /*0x10*/,
    17,
    18,
    0,
    8,
    7,
    9,
    6,
    10,
    5,
    11,
    4,
    12,
    3,
    13,
    2,
    14,
    1,
    15
  };

  public static short BitReverse(int value)
  {
    return (short) ((int) Utils.DEF_REVERSE_BITS[value & 15] << 12 | (int) Utils.DEF_REVERSE_BITS[value >> 4 & 15] << 8 | (int) Utils.DEF_REVERSE_BITS[value >> 8 & 15] << 4 | (int) Utils.DEF_REVERSE_BITS[value >> 12]);
  }
}
