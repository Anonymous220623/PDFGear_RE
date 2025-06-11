// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.RC2Algorithm
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class RC2Algorithm : ICipher
{
  private const int m_blockSize = 8;
  private static readonly byte[] m_piTable = new byte[256 /*0x0100*/]
  {
    (byte) 217,
    (byte) 120,
    (byte) 249,
    (byte) 196,
    (byte) 25,
    (byte) 221,
    (byte) 181,
    (byte) 237,
    (byte) 40,
    (byte) 233,
    (byte) 253,
    (byte) 121,
    (byte) 74,
    (byte) 160 /*0xA0*/,
    (byte) 216,
    (byte) 157,
    (byte) 198,
    (byte) 126,
    (byte) 55,
    (byte) 131,
    (byte) 43,
    (byte) 118,
    (byte) 83,
    (byte) 142,
    (byte) 98,
    (byte) 76,
    (byte) 100,
    (byte) 136,
    (byte) 68,
    (byte) 139,
    (byte) 251,
    (byte) 162,
    (byte) 23,
    (byte) 154,
    (byte) 89,
    (byte) 245,
    (byte) 135,
    (byte) 179,
    (byte) 79,
    (byte) 19,
    (byte) 97,
    (byte) 69,
    (byte) 109,
    (byte) 141,
    (byte) 9,
    (byte) 129,
    (byte) 125,
    (byte) 50,
    (byte) 189,
    (byte) 143,
    (byte) 64 /*0x40*/,
    (byte) 235,
    (byte) 134,
    (byte) 183,
    (byte) 123,
    (byte) 11,
    (byte) 240 /*0xF0*/,
    (byte) 149,
    (byte) 33,
    (byte) 34,
    (byte) 92,
    (byte) 107,
    (byte) 78,
    (byte) 130,
    (byte) 84,
    (byte) 214,
    (byte) 101,
    (byte) 147,
    (byte) 206,
    (byte) 96 /*0x60*/,
    (byte) 178,
    (byte) 28,
    (byte) 115,
    (byte) 86,
    (byte) 192 /*0xC0*/,
    (byte) 20,
    (byte) 167,
    (byte) 140,
    (byte) 241,
    (byte) 220,
    (byte) 18,
    (byte) 117,
    (byte) 202,
    (byte) 31 /*0x1F*/,
    (byte) 59,
    (byte) 190,
    (byte) 228,
    (byte) 209,
    (byte) 66,
    (byte) 61,
    (byte) 212,
    (byte) 48 /*0x30*/,
    (byte) 163,
    (byte) 60,
    (byte) 182,
    (byte) 38,
    (byte) 111,
    (byte) 191,
    (byte) 14,
    (byte) 218,
    (byte) 70,
    (byte) 105,
    (byte) 7,
    (byte) 87,
    (byte) 39,
    (byte) 242,
    (byte) 29,
    (byte) 155,
    (byte) 188,
    (byte) 148,
    (byte) 67,
    (byte) 3,
    (byte) 248,
    (byte) 17,
    (byte) 199,
    (byte) 246,
    (byte) 144 /*0x90*/,
    (byte) 239,
    (byte) 62,
    (byte) 231,
    (byte) 6,
    (byte) 195,
    (byte) 213,
    (byte) 47,
    (byte) 200,
    (byte) 102,
    (byte) 30,
    (byte) 215,
    (byte) 8,
    (byte) 232,
    (byte) 234,
    (byte) 222,
    (byte) 128 /*0x80*/,
    (byte) 82,
    (byte) 238,
    (byte) 247,
    (byte) 132,
    (byte) 170,
    (byte) 114,
    (byte) 172,
    (byte) 53,
    (byte) 77,
    (byte) 106,
    (byte) 42,
    (byte) 150,
    (byte) 26,
    (byte) 210,
    (byte) 113,
    (byte) 90,
    (byte) 21,
    (byte) 73,
    (byte) 116,
    (byte) 75,
    (byte) 159,
    (byte) 208 /*0xD0*/,
    (byte) 94,
    (byte) 4,
    (byte) 24,
    (byte) 164,
    (byte) 236,
    (byte) 194,
    (byte) 224 /*0xE0*/,
    (byte) 65,
    (byte) 110,
    (byte) 15,
    (byte) 81,
    (byte) 203,
    (byte) 204,
    (byte) 36,
    (byte) 145,
    (byte) 175,
    (byte) 80 /*0x50*/,
    (byte) 161,
    (byte) 244,
    (byte) 112 /*0x70*/,
    (byte) 57,
    (byte) 153,
    (byte) 124,
    (byte) 58,
    (byte) 133,
    (byte) 35,
    (byte) 184,
    (byte) 180,
    (byte) 122,
    (byte) 252,
    (byte) 2,
    (byte) 54,
    (byte) 91,
    (byte) 37,
    (byte) 85,
    (byte) 151,
    (byte) 49,
    (byte) 45,
    (byte) 93,
    (byte) 250,
    (byte) 152,
    (byte) 227,
    (byte) 138,
    (byte) 146,
    (byte) 174,
    (byte) 5,
    (byte) 223,
    (byte) 41,
    (byte) 16 /*0x10*/,
    (byte) 103,
    (byte) 108,
    (byte) 186,
    (byte) 201,
    (byte) 211,
    (byte) 0,
    (byte) 230,
    (byte) 207,
    (byte) 225,
    (byte) 158,
    (byte) 168,
    (byte) 44,
    (byte) 99,
    (byte) 22,
    (byte) 1,
    (byte) 63 /*0x3F*/,
    (byte) 88,
    (byte) 226,
    (byte) 137,
    (byte) 169,
    (byte) 13,
    (byte) 56,
    (byte) 52,
    (byte) 27,
    (byte) 171,
    (byte) 51,
    byte.MaxValue,
    (byte) 176 /*0xB0*/,
    (byte) 187,
    (byte) 72,
    (byte) 12,
    (byte) 95,
    (byte) 185,
    (byte) 177,
    (byte) 205,
    (byte) 46,
    (byte) 197,
    (byte) 243,
    (byte) 219,
    (byte) 71,
    (byte) 229,
    (byte) 165,
    (byte) 156,
    (byte) 119,
    (byte) 10,
    (byte) 166,
    (byte) 32 /*0x20*/,
    (byte) 104,
    (byte) 254,
    (byte) 127 /*0x7F*/,
    (byte) 193,
    (byte) 173
  };
  private int[] m_Key;
  private bool m_isEncrypt;

  private int[] GenerateKey(byte[] key, int bits)
  {
    int[] numArray = new int[128 /*0x80*/];
    for (int index = 0; index != key.Length; ++index)
      numArray[index] = (int) key[index] & (int) byte.MaxValue;
    int length = key.Length;
    if (length < 128 /*0x80*/)
    {
      int num1 = 0;
      int num2 = numArray[length - 1];
      do
      {
        num2 = (int) RC2Algorithm.m_piTable[num2 + numArray[num1++] & (int) byte.MaxValue] & (int) byte.MaxValue;
        numArray[length++] = num2;
      }
      while (length < 128 /*0x80*/);
    }
    int num3 = bits + 7 >> 3;
    int num4 = (int) RC2Algorithm.m_piTable[numArray[128 /*0x80*/ - num3] & (int) byte.MaxValue >> (7 & -bits)] & (int) byte.MaxValue;
    numArray[128 /*0x80*/ - num3] = num4;
    for (int index = 128 /*0x80*/ - num3 - 1; index >= 0; --index)
    {
      num4 = (int) RC2Algorithm.m_piTable[num4 ^ numArray[index + num3]] & (int) byte.MaxValue;
      numArray[index] = num4;
    }
    int[] key1 = new int[64 /*0x40*/];
    for (int index = 0; index != key1.Length; ++index)
      key1[index] = numArray[2 * index] + (numArray[2 * index + 1] << 8);
    return key1;
  }

  public void Initialize(bool forEncryption, ICipherParam parameters)
  {
    this.m_isEncrypt = forEncryption;
    if (!(parameters is KeyParameter))
      return;
    byte[] keys = ((KeyParameter) parameters).Keys;
    this.m_Key = this.GenerateKey(keys, keys.Length * 8);
  }

  public void Reset()
  {
  }

  public string AlgorithmName => "RC2";

  public bool IsBlock => false;

  public int BlockSize => 8;

  public int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
  {
    if (this.m_isEncrypt)
      this.EncryptBlock(input, inOff, output, outOff);
    else
      this.DecryptBlock(input, inOff, output, outOff);
    return 8;
  }

  private int RotateWordLeft(int x, int y)
  {
    x &= (int) ushort.MaxValue;
    return x << y | x >> 16 /*0x10*/ - y;
  }

  private void EncryptBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
  {
    int num1 = (((int) input[inOff + 7] & (int) byte.MaxValue) << 8) + ((int) input[inOff + 6] & (int) byte.MaxValue);
    int num2 = (((int) input[inOff + 5] & (int) byte.MaxValue) << 8) + ((int) input[inOff + 4] & (int) byte.MaxValue);
    int num3 = (((int) input[inOff + 3] & (int) byte.MaxValue) << 8) + ((int) input[inOff + 2] & (int) byte.MaxValue);
    int num4 = (((int) input[inOff + 1] & (int) byte.MaxValue) << 8) + ((int) input[inOff] & (int) byte.MaxValue);
    for (int index = 0; index <= 16 /*0x10*/; index += 4)
    {
      num4 = this.RotateWordLeft(num4 + (num3 & ~num1) + (num2 & num1) + this.m_Key[index], 1);
      num3 = this.RotateWordLeft(num3 + (num2 & ~num4) + (num1 & num4) + this.m_Key[index + 1], 2);
      num2 = this.RotateWordLeft(num2 + (num1 & ~num3) + (num4 & num3) + this.m_Key[index + 2], 3);
      num1 = this.RotateWordLeft(num1 + (num4 & ~num2) + (num3 & num2) + this.m_Key[index + 3], 5);
    }
    int num5 = num4 + this.m_Key[num1 & 63 /*0x3F*/];
    int num6 = num3 + this.m_Key[num5 & 63 /*0x3F*/];
    int num7 = num2 + this.m_Key[num6 & 63 /*0x3F*/];
    int num8 = num1 + this.m_Key[num7 & 63 /*0x3F*/];
    for (int index = 20; index <= 40; index += 4)
    {
      num5 = this.RotateWordLeft(num5 + (num6 & ~num8) + (num7 & num8) + this.m_Key[index], 1);
      num6 = this.RotateWordLeft(num6 + (num7 & ~num5) + (num8 & num5) + this.m_Key[index + 1], 2);
      num7 = this.RotateWordLeft(num7 + (num8 & ~num6) + (num5 & num6) + this.m_Key[index + 2], 3);
      num8 = this.RotateWordLeft(num8 + (num5 & ~num7) + (num6 & num7) + this.m_Key[index + 3], 5);
    }
    int num9 = num5 + this.m_Key[num8 & 63 /*0x3F*/];
    int num10 = num6 + this.m_Key[num9 & 63 /*0x3F*/];
    int num11 = num7 + this.m_Key[num10 & 63 /*0x3F*/];
    int num12 = num8 + this.m_Key[num11 & 63 /*0x3F*/];
    for (int index = 44; index < 64 /*0x40*/; index += 4)
    {
      num9 = this.RotateWordLeft(num9 + (num10 & ~num12) + (num11 & num12) + this.m_Key[index], 1);
      num10 = this.RotateWordLeft(num10 + (num11 & ~num9) + (num12 & num9) + this.m_Key[index + 1], 2);
      num11 = this.RotateWordLeft(num11 + (num12 & ~num10) + (num9 & num10) + this.m_Key[index + 2], 3);
      num12 = this.RotateWordLeft(num12 + (num9 & ~num11) + (num10 & num11) + this.m_Key[index + 3], 5);
    }
    outBytes[outOff] = (byte) num9;
    outBytes[outOff + 1] = (byte) (num9 >> 8);
    outBytes[outOff + 2] = (byte) num10;
    outBytes[outOff + 3] = (byte) (num10 >> 8);
    outBytes[outOff + 4] = (byte) num11;
    outBytes[outOff + 5] = (byte) (num11 >> 8);
    outBytes[outOff + 6] = (byte) num12;
    outBytes[outOff + 7] = (byte) (num12 >> 8);
  }

  private void DecryptBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
  {
    int x1 = (((int) input[inOff + 7] & (int) byte.MaxValue) << 8) + ((int) input[inOff + 6] & (int) byte.MaxValue);
    int x2 = (((int) input[inOff + 5] & (int) byte.MaxValue) << 8) + ((int) input[inOff + 4] & (int) byte.MaxValue);
    int x3 = (((int) input[inOff + 3] & (int) byte.MaxValue) << 8) + ((int) input[inOff + 2] & (int) byte.MaxValue);
    int x4 = (((int) input[inOff + 1] & (int) byte.MaxValue) << 8) + ((int) input[inOff] & (int) byte.MaxValue);
    for (int index = 60; index >= 44; index -= 4)
    {
      x1 = this.RotateWordLeft(x1, 11) - ((x4 & ~x2) + (x3 & x2) + this.m_Key[index + 3]);
      x2 = this.RotateWordLeft(x2, 13) - ((x1 & ~x3) + (x4 & x3) + this.m_Key[index + 2]);
      x3 = this.RotateWordLeft(x3, 14) - ((x2 & ~x4) + (x1 & x4) + this.m_Key[index + 1]);
      x4 = this.RotateWordLeft(x4, 15) - ((x3 & ~x1) + (x2 & x1) + this.m_Key[index]);
    }
    int x5 = x1 - this.m_Key[x2 & 63 /*0x3F*/];
    int x6 = x2 - this.m_Key[x3 & 63 /*0x3F*/];
    int x7 = x3 - this.m_Key[x4 & 63 /*0x3F*/];
    int x8 = x4 - this.m_Key[x5 & 63 /*0x3F*/];
    for (int index = 40; index >= 20; index -= 4)
    {
      x5 = this.RotateWordLeft(x5, 11) - ((x8 & ~x6) + (x7 & x6) + this.m_Key[index + 3]);
      x6 = this.RotateWordLeft(x6, 13) - ((x5 & ~x7) + (x8 & x7) + this.m_Key[index + 2]);
      x7 = this.RotateWordLeft(x7, 14) - ((x6 & ~x8) + (x5 & x8) + this.m_Key[index + 1]);
      x8 = this.RotateWordLeft(x8, 15) - ((x7 & ~x5) + (x6 & x5) + this.m_Key[index]);
    }
    int x9 = x5 - this.m_Key[x6 & 63 /*0x3F*/];
    int x10 = x6 - this.m_Key[x7 & 63 /*0x3F*/];
    int x11 = x7 - this.m_Key[x8 & 63 /*0x3F*/];
    int x12 = x8 - this.m_Key[x9 & 63 /*0x3F*/];
    for (int index = 16 /*0x10*/; index >= 0; index -= 4)
    {
      x9 = this.RotateWordLeft(x9, 11) - ((x12 & ~x10) + (x11 & x10) + this.m_Key[index + 3]);
      x10 = this.RotateWordLeft(x10, 13) - ((x9 & ~x11) + (x12 & x11) + this.m_Key[index + 2]);
      x11 = this.RotateWordLeft(x11, 14) - ((x10 & ~x12) + (x9 & x12) + this.m_Key[index + 1]);
      x12 = this.RotateWordLeft(x12, 15) - ((x11 & ~x9) + (x10 & x9) + this.m_Key[index]);
    }
    outBytes[outOff] = (byte) x12;
    outBytes[outOff + 1] = (byte) (x12 >> 8);
    outBytes[outOff + 2] = (byte) x11;
    outBytes[outOff + 3] = (byte) (x11 >> 8);
    outBytes[outOff + 4] = (byte) x10;
    outBytes[outOff + 5] = (byte) (x10 >> 8);
    outBytes[outOff + 6] = (byte) x9;
    outBytes[outOff + 7] = (byte) (x9 >> 8);
  }
}
