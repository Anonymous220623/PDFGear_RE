// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2Statics
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Compression;

internal static class JBIG2Statics
{
  internal const int SelectWidth = 1;
  internal const int SelectHeight = 2;
  internal const int SelectIfEither = 3;
  internal const int SelectIfBoth = 4;
  internal const int Unknown = 0;
  internal const int Bmp = 1;
  internal const int JfifJpeg = 2;
  internal const int Png = 3;
  internal const int Tiff = 4;
  internal const int TiffPackBits = 5;
  internal const int TiffRle = 6;
  internal const int TiffG3 = 7;
  internal const int TiffG4 = 8;
  internal const int TiffLzw = 9;
  internal const int TiffZip = 10;
  internal const int TiffPnm = 11;
  internal const int TiffPs = 12;
  internal const int Gif = 13;
  internal const int Jp2 = 14;
  internal const int Webp = 15;
  internal const int Lpdf = 16 /*0x10*/;
  internal const int Default = 17;
  internal const int Spix = 18;
  internal const int CompressionNone = 1;
  internal const int CompressionCcittRle = 2;
  internal const int CompressionCcittFax3 = 3;
  internal const int CompressionCcittT4 = 3;
  internal const int CompressionCcittFax4 = 4;
  internal const int CompressionCcittT6 = 4;
  internal const int CompressionLzw = 5;
  internal const int CompressionOJpeg = 6;
  internal const int CompressionJpeg = 7;
  internal const int CompressionNext = 32766;
  internal const int CompressionCcittRlew = 32771;
  internal const int CompressionPackBits = 32773;
  internal const int CompressionDeflate = 32946;
  internal const int CompressionAdobeDeflate = 8;
  internal const float RedWeight = 0.3f;
  internal const float GreenWeight = 0.5f;
  internal const float BlueWeight = 0.2f;
  internal static int[] MatchOffset = new int[50]
  {
    0,
    0,
    0,
    1,
    -1,
    0,
    0,
    -1,
    1,
    0,
    -1,
    1,
    1,
    1,
    -1,
    -1,
    1,
    -1,
    0,
    -2,
    2,
    0,
    0,
    2,
    -2,
    0,
    -1,
    -2,
    1,
    -2,
    2,
    -1,
    2,
    1,
    1,
    2,
    -1,
    2,
    -2,
    1,
    -2,
    -1,
    -2,
    -2,
    2,
    -2,
    2,
    2,
    -2,
    2
  };
  internal static int PixSrc = 24;
  internal static int PixDst = 20;
  internal static int PixClr = 0;
  internal static int PixSet = 30;
  internal static int PixPaint = JBIG2Statics.PixSrc | JBIG2Statics.PixDst;
  internal static int PixMask = JBIG2Statics.PixSrc & JBIG2Statics.PixDst;
  internal static int PixSubtract = JBIG2Statics.PixDst & JBIG2Statics.PixNot(JBIG2Statics.PixSrc);
  internal static int PixXor = JBIG2Statics.PixSrc ^ JBIG2Statics.PixDst;
  internal static int SelDontCare = 0;
  internal static int SelHit = 1;
  internal static int SelMiss = 2;
  internal static int Undef = -1;
  internal static int ShiftLeft = 0;
  internal static int ShiftRight = 1;
  internal static int SelectIfLt = 1;
  internal static int SelectIfGt = 2;
  internal static int SelectIfLte = 3;
  internal static int SelectIfGte = 4;
  internal static uint[] RightMask = new uint[33]
  {
    0U,
    1U,
    3U,
    7U,
    15U,
    31U /*0x1F*/,
    63U /*0x3F*/,
    (uint) sbyte.MaxValue,
    (uint) byte.MaxValue,
    511U /*0x01FF*/,
    1023U /*0x03FF*/,
    2047U /*0x07FF*/,
    4095U /*0x0FFF*/,
    8191U /*0x1FFF*/,
    16383U /*0x3FFF*/,
    (uint) short.MaxValue,
    (uint) ushort.MaxValue,
    131071U /*0x01FFFF*/,
    262143U /*0x03FFFF*/,
    524287U /*0x07FFFF*/,
    1048575U /*0x0FFFFF*/,
    2097151U /*0x1FFFFF*/,
    4194303U /*0x3FFFFF*/,
    8388607U /*0x7FFFFF*/,
    16777215U /*0xFFFFFF*/,
    33554431U /*0x01FFFFFF*/,
    67108863U /*0x03FFFFFF*/,
    134217727U /*0x07FFFFFF*/,
    268435455U /*0x0FFFFFFF*/,
    536870911U /*0x1FFFFFFF*/,
    1073741823U /*0x3FFFFFFF*/,
    (uint) int.MaxValue,
    uint.MaxValue
  };
  internal static uint[] LeftMask = new uint[33]
  {
    0U,
    2147483648U /*0x80000000*/,
    3221225472U /*0xC0000000*/,
    3758096384U /*0xE0000000*/,
    4026531840U /*0xF0000000*/,
    4160749568U /*0xF8000000*/,
    4227858432U /*0xFC000000*/,
    4261412864U /*0xFE000000*/,
    4278190080U /*0xFF000000*/,
    4286578688U /*0xFF800000*/,
    4290772992U /*0xFFC00000*/,
    4292870144U /*0xFFE00000*/,
    4293918720U /*0xFFF00000*/,
    4294443008U,
    4294705152U,
    4294836224U,
    4294901760U,
    4294934528U,
    4294950912U,
    4294959104U,
    4294963200U,
    4294965248U,
    4294966272U,
    4294966784U,
    4294967040U,
    4294967168U,
    4294967232U,
    4294967264U,
    4294967280U,
    4294967288U,
    4294967292U,
    4294967294U,
    uint.MaxValue
  };

  internal static uint Htonl(object p)
  {
    byte[] numArray = (byte[]) null;
    if (p is int num1)
      numArray = BitConverter.GetBytes(num1);
    if (p is uint num2)
      numArray = BitConverter.GetBytes(num2);
    if (p is char ch)
      numArray = BitConverter.GetBytes(ch);
    if (p is float num3)
      numArray = BitConverter.GetBytes(num3);
    if (BitConverter.IsLittleEndian)
      Array.Reverse((Array) numArray);
    return BitConverter.ToUInt32(numArray, 0);
  }

  internal static byte[] Htonl(uint p)
  {
    byte[] bytes = BitConverter.GetBytes(p);
    if (BitConverter.IsLittleEndian)
      Array.Reverse((Array) bytes);
    return bytes;
  }

  internal static int PixNot(int op) => op ^ 30;

  internal static void ClearDataBit(ref uint[] line, int index, int n)
  {
    if (n > 31 /*0x1F*/)
    {
      index += n / 32 /*0x20*/;
      n %= 32 /*0x20*/;
    }
    int num = n / 32 /*0x20*/;
    string str = Convert.ToString((long) line[index], 2);
    while (str.Length < 32 /*0x20*/)
      str = "0" + str;
    char[] charArray = str.ToCharArray();
    charArray[n] = '0';
    uint uint32 = Convert.ToUInt32(new string(charArray), 2);
    line[index] = uint32;
  }

  internal static void ClearDataBit(ref uint line, int n)
  {
    if (n > 31 /*0x1F*/)
      return;
    string str = Convert.ToString((long) line, 2);
    while (str.Length < 32 /*0x20*/)
      str = "0" + str;
    char[] charArray = str.ToCharArray();
    charArray[n] = '0';
    uint uint32 = Convert.ToUInt32(new string(charArray), 2);
    line = uint32;
  }

  internal static uint GetDataBit(uint[] line, int index, int n)
  {
    int startIndex = n % 32 /*0x20*/;
    int index1 = index + n / 32 /*0x20*/;
    string str = Convert.ToString((long) line[index1], 2);
    while (str.Length < 32 /*0x20*/)
      str = "0" + str;
    return uint.Parse(str.Substring(startIndex, 1));
  }

  internal static uint GetDataDibit(uint line, int n) => line >> 2 * (15 - (n & 15)) & 3U;

  internal static uint GetDataQbit(uint line, int n) => line >> 4 * (7 - (n & 7)) & 15U;

  internal static void SetDataBit(ref uint line, int n) => line |= 2147483648U /*0x80000000*/ >> n;

  internal static void SetDataQbit(ref uint[] line, int n, uint val)
  {
    uint num = line[n] >> 3 | (uint) (((int) val & 15) << 28 - 4 * (n & 7));
    line[n] = num;
  }

  internal static void SetDataDibit(ref uint[] line, int n, uint val)
  {
    uint num = line[n] >> 4 | (uint) (((int) val & 3) << 30 - 2 * (n & 15));
    line[n] = num;
  }

  internal static uint GetDataByte(uint[] line, int n) => line[n];

  internal static uint GetDataByte(uint line, int n) => (uint) BitConverter.GetBytes(line)[n];

  internal static void SetDataByte(ref uint[] line, int n, uint val)
  {
    int num1 = n % 4;
    int index = n / 4;
    byte[] bytes = BitConverter.GetBytes(line[index]);
    byte num2 = bytes[3];
    byte num3 = bytes[2];
    byte num4 = bytes[1];
    byte num5 = bytes[0];
    switch (num1)
    {
      case 0:
        num2 = (byte) val;
        break;
      case 1:
        num3 = (byte) val;
        break;
      case 2:
        num4 = (byte) val;
        break;
      case 3:
        num5 = (byte) val;
        break;
    }
    byte[] numArray = new byte[4]{ num5, num4, num3, num2 };
    line[index] = BitConverter.ToUInt32(numArray, 0);
  }

  internal static void SetDataTwoBytes(ref uint[] line, int n, short val)
  {
    int num1 = n % 4;
    int index = n / 4;
    byte[] bytes = BitConverter.GetBytes(line[index]);
    byte num2 = bytes[3];
    byte num3 = bytes[2];
    byte num4 = bytes[1];
    byte num5 = bytes[0];
    BitConverter.GetBytes(val);
    switch (num1)
    {
      case 0:
        num2 = (byte) val;
        break;
      case 1:
        num3 = (byte) val;
        break;
      case 2:
        num4 = (byte) val;
        break;
      case 3:
        num5 = (byte) val;
        break;
    }
    byte[] numArray = new byte[4]{ num5, num4, num3, num2 };
    line[index] = BitConverter.ToUInt32(numArray, 0);
  }

  internal static short GetDataTwoBytes(uint[] line, int n) => (short) ((int) line[n] ^ 2);

  internal static Pixa CreatePixa(int n)
  {
    if (n <= 0)
      n = 20;
    Pixa pixa = new Pixa(n);
    pixa.Boxa = JBIG2Statics.CreateBoxa(n);
    return pixa.Boxa != null ? pixa : throw new Exception();
  }

  internal static Boxa CreateBoxa(int n)
  {
    if (n <= 0)
      n = 50;
    return new Boxa(n);
  }

  internal static Numa CreateNuma(int n)
  {
    int num = 50;
    if (n <= 0)
      n = num;
    return new Numa(n);
  }

  internal static L_Stack CreateLStack(int nalloc)
  {
    int num = 20;
    if (nalloc <= 0)
      nalloc = num;
    return new L_Stack(nalloc);
  }

  internal static Pta CreatePta(int n)
  {
    int num = 20;
    if (n <= 0)
      n = num;
    Pta pta = new Pta(n);
    pta.N = 0;
    pta.Nalloc = n;
    ++pta.RefCount;
    return pta;
  }

  internal static PixColormap CreatePixCmap(int depth)
  {
    PixColormap pixCmap = new PixColormap();
    int length = 1 << depth;
    if (depth != 1 && depth != 2 && depth != 4)
      ;
    pixCmap.Depth = depth;
    pixCmap.Nalloc = 1 << depth;
    pixCmap.Array = new RGBA_Quad[length];
    pixCmap.N = 0;
    return pixCmap;
  }

  internal static Sel CreateSel(int height, int weight, string name)
  {
    Sel sel = new Sel();
    sel.SY = height;
    sel.SX = weight;
    sel.Name = name;
    List<int[]> numArrayList = new List<int[]>(height);
    for (int index = 0; index < height; ++index)
      numArrayList.Add(new int[weight]);
    sel.Data = numArrayList;
    return sel;
  }
}
