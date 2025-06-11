// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.DataEncryption
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class DataEncryption : ICipher
{
  internal const int m_blockSize = 8;
  private static readonly short[] ByteBit = new short[8]
  {
    (short) 128 /*0x80*/,
    (short) 64 /*0x40*/,
    (short) 32 /*0x20*/,
    (short) 16 /*0x10*/,
    (short) 8,
    (short) 4,
    (short) 2,
    (short) 1
  };
  private static readonly int[] BigByte = new int[24]
  {
    8388608 /*0x800000*/,
    4194304 /*0x400000*/,
    2097152 /*0x200000*/,
    1048576 /*0x100000*/,
    524288 /*0x080000*/,
    262144 /*0x040000*/,
    131072 /*0x020000*/,
    65536 /*0x010000*/,
    32768 /*0x8000*/,
    16384 /*0x4000*/,
    8192 /*0x2000*/,
    4096 /*0x1000*/,
    2048 /*0x0800*/,
    1024 /*0x0400*/,
    512 /*0x0200*/,
    256 /*0x0100*/,
    128 /*0x80*/,
    64 /*0x40*/,
    32 /*0x20*/,
    16 /*0x10*/,
    8,
    4,
    2,
    1
  };
  private static readonly byte[] Pc1 = new byte[56]
  {
    (byte) 56,
    (byte) 48 /*0x30*/,
    (byte) 40,
    (byte) 32 /*0x20*/,
    (byte) 24,
    (byte) 16 /*0x10*/,
    (byte) 8,
    (byte) 0,
    (byte) 57,
    (byte) 49,
    (byte) 41,
    (byte) 33,
    (byte) 25,
    (byte) 17,
    (byte) 9,
    (byte) 1,
    (byte) 58,
    (byte) 50,
    (byte) 42,
    (byte) 34,
    (byte) 26,
    (byte) 18,
    (byte) 10,
    (byte) 2,
    (byte) 59,
    (byte) 51,
    (byte) 43,
    (byte) 35,
    (byte) 62,
    (byte) 54,
    (byte) 46,
    (byte) 38,
    (byte) 30,
    (byte) 22,
    (byte) 14,
    (byte) 6,
    (byte) 61,
    (byte) 53,
    (byte) 45,
    (byte) 37,
    (byte) 29,
    (byte) 21,
    (byte) 13,
    (byte) 5,
    (byte) 60,
    (byte) 52,
    (byte) 44,
    (byte) 36,
    (byte) 28,
    (byte) 20,
    (byte) 12,
    (byte) 4,
    (byte) 27,
    (byte) 19,
    (byte) 11,
    (byte) 3
  };
  private static readonly byte[] ToTrot = new byte[16 /*0x10*/]
  {
    (byte) 1,
    (byte) 2,
    (byte) 4,
    (byte) 6,
    (byte) 8,
    (byte) 10,
    (byte) 12,
    (byte) 14,
    (byte) 15,
    (byte) 17,
    (byte) 19,
    (byte) 21,
    (byte) 23,
    (byte) 25,
    (byte) 27,
    (byte) 28
  };
  private static readonly byte[] Pc2 = new byte[48 /*0x30*/]
  {
    (byte) 13,
    (byte) 16 /*0x10*/,
    (byte) 10,
    (byte) 23,
    (byte) 0,
    (byte) 4,
    (byte) 2,
    (byte) 27,
    (byte) 14,
    (byte) 5,
    (byte) 20,
    (byte) 9,
    (byte) 22,
    (byte) 18,
    (byte) 11,
    (byte) 3,
    (byte) 25,
    (byte) 7,
    (byte) 15,
    (byte) 6,
    (byte) 26,
    (byte) 19,
    (byte) 12,
    (byte) 1,
    (byte) 40,
    (byte) 51,
    (byte) 30,
    (byte) 36,
    (byte) 46,
    (byte) 54,
    (byte) 29,
    (byte) 39,
    (byte) 50,
    (byte) 44,
    (byte) 32 /*0x20*/,
    (byte) 47,
    (byte) 43,
    (byte) 48 /*0x30*/,
    (byte) 38,
    (byte) 55,
    (byte) 33,
    (byte) 52,
    (byte) 45,
    (byte) 41,
    (byte) 49,
    (byte) 35,
    (byte) 28,
    (byte) 31 /*0x1F*/
  };
  private static readonly uint[] Sp1 = new uint[64 /*0x40*/]
  {
    16843776U /*0x01010400*/,
    0U,
    65536U /*0x010000*/,
    16843780U,
    16842756U /*0x01010004*/,
    66564U,
    4U,
    65536U /*0x010000*/,
    1024U /*0x0400*/,
    16843776U /*0x01010400*/,
    16843780U,
    1024U /*0x0400*/,
    16778244U /*0x01000404*/,
    16842756U /*0x01010004*/,
    16777216U /*0x01000000*/,
    4U,
    1028U,
    16778240U /*0x01000400*/,
    16778240U /*0x01000400*/,
    66560U /*0x010400*/,
    66560U /*0x010400*/,
    16842752U /*0x01010000*/,
    16842752U /*0x01010000*/,
    16778244U /*0x01000404*/,
    65540U /*0x010004*/,
    16777220U /*0x01000004*/,
    16777220U /*0x01000004*/,
    65540U /*0x010004*/,
    0U,
    1028U,
    66564U,
    16777216U /*0x01000000*/,
    65536U /*0x010000*/,
    16843780U,
    4U,
    16842752U /*0x01010000*/,
    16843776U /*0x01010400*/,
    16777216U /*0x01000000*/,
    16777216U /*0x01000000*/,
    1024U /*0x0400*/,
    16842756U /*0x01010004*/,
    65536U /*0x010000*/,
    66560U /*0x010400*/,
    16777220U /*0x01000004*/,
    1024U /*0x0400*/,
    4U,
    16778244U /*0x01000404*/,
    66564U,
    16843780U,
    65540U /*0x010004*/,
    16842752U /*0x01010000*/,
    16778244U /*0x01000404*/,
    16777220U /*0x01000004*/,
    1028U,
    66564U,
    16843776U /*0x01010400*/,
    1028U,
    16778240U /*0x01000400*/,
    16778240U /*0x01000400*/,
    0U,
    65540U /*0x010004*/,
    66560U /*0x010400*/,
    0U,
    16842756U /*0x01010004*/
  };
  private static readonly uint[] Sp2 = new uint[64 /*0x40*/]
  {
    2148565024U,
    2147516416U /*0x80008000*/,
    32768U /*0x8000*/,
    1081376U,
    1048576U /*0x100000*/,
    32U /*0x20*/,
    2148532256U /*0x80100020*/,
    2147516448U /*0x80008020*/,
    2147483680U /*0x80000020*/,
    2148565024U,
    2148564992U /*0x80108000*/,
    2147483648U /*0x80000000*/,
    2147516416U /*0x80008000*/,
    1048576U /*0x100000*/,
    32U /*0x20*/,
    2148532256U /*0x80100020*/,
    1081344U /*0x108000*/,
    1048608U /*0x100020*/,
    2147516448U /*0x80008020*/,
    0U,
    2147483648U /*0x80000000*/,
    32768U /*0x8000*/,
    1081376U,
    2148532224U /*0x80100000*/,
    1048608U /*0x100020*/,
    2147483680U /*0x80000020*/,
    0U,
    1081344U /*0x108000*/,
    32800U,
    2148564992U /*0x80108000*/,
    2148532224U /*0x80100000*/,
    32800U,
    0U,
    1081376U,
    2148532256U /*0x80100020*/,
    1048576U /*0x100000*/,
    2147516448U /*0x80008020*/,
    2148532224U /*0x80100000*/,
    2148564992U /*0x80108000*/,
    32768U /*0x8000*/,
    2148532224U /*0x80100000*/,
    2147516416U /*0x80008000*/,
    32U /*0x20*/,
    2148565024U,
    1081376U,
    32U /*0x20*/,
    32768U /*0x8000*/,
    2147483648U /*0x80000000*/,
    32800U,
    2148564992U /*0x80108000*/,
    1048576U /*0x100000*/,
    2147483680U /*0x80000020*/,
    1048608U /*0x100020*/,
    2147516448U /*0x80008020*/,
    2147483680U /*0x80000020*/,
    1048608U /*0x100020*/,
    1081344U /*0x108000*/,
    0U,
    2147516416U /*0x80008000*/,
    32800U,
    2147483648U /*0x80000000*/,
    2148532256U /*0x80100020*/,
    2148565024U,
    1081344U /*0x108000*/
  };
  private static readonly uint[] Sp3 = new uint[64 /*0x40*/]
  {
    520U,
    134349312U /*0x08020200*/,
    0U,
    134348808U /*0x08020008*/,
    134218240U /*0x08000200*/,
    0U,
    131592U,
    134218240U /*0x08000200*/,
    131080U /*0x020008*/,
    134217736U /*0x08000008*/,
    134217736U /*0x08000008*/,
    131072U /*0x020000*/,
    134349320U,
    131080U /*0x020008*/,
    134348800U /*0x08020000*/,
    520U,
    134217728U /*0x08000000*/,
    8U,
    134349312U /*0x08020200*/,
    512U /*0x0200*/,
    131584U /*0x020200*/,
    134348800U /*0x08020000*/,
    134348808U /*0x08020008*/,
    131592U,
    134218248U /*0x08000208*/,
    131584U /*0x020200*/,
    131072U /*0x020000*/,
    134218248U /*0x08000208*/,
    8U,
    134349320U,
    512U /*0x0200*/,
    134217728U /*0x08000000*/,
    134349312U /*0x08020200*/,
    134217728U /*0x08000000*/,
    131080U /*0x020008*/,
    520U,
    131072U /*0x020000*/,
    134349312U /*0x08020200*/,
    134218240U /*0x08000200*/,
    0U,
    512U /*0x0200*/,
    131080U /*0x020008*/,
    134349320U,
    134218240U /*0x08000200*/,
    134217736U /*0x08000008*/,
    512U /*0x0200*/,
    0U,
    134348808U /*0x08020008*/,
    134218248U /*0x08000208*/,
    131072U /*0x020000*/,
    134217728U /*0x08000000*/,
    134349320U,
    8U,
    131592U,
    131584U /*0x020200*/,
    134217736U /*0x08000008*/,
    134348800U /*0x08020000*/,
    134218248U /*0x08000208*/,
    520U,
    134348800U /*0x08020000*/,
    131592U,
    8U,
    134348808U /*0x08020008*/,
    131584U /*0x020200*/
  };
  private static readonly uint[] Sp4 = new uint[64 /*0x40*/]
  {
    8396801U,
    8321U,
    8321U,
    128U /*0x80*/,
    8396928U,
    8388737U,
    8388609U /*0x800001*/,
    8193U,
    0U,
    8396800U /*0x802000*/,
    8396800U /*0x802000*/,
    8396929U,
    129U,
    0U,
    8388736U /*0x800080*/,
    8388609U /*0x800001*/,
    1U,
    8192U /*0x2000*/,
    8388608U /*0x800000*/,
    8396801U,
    128U /*0x80*/,
    8388608U /*0x800000*/,
    8193U,
    8320U,
    8388737U,
    1U,
    8320U,
    8388736U /*0x800080*/,
    8192U /*0x2000*/,
    8396928U,
    8396929U,
    129U,
    8388736U /*0x800080*/,
    8388609U /*0x800001*/,
    8396800U /*0x802000*/,
    8396929U,
    129U,
    0U,
    0U,
    8396800U /*0x802000*/,
    8320U,
    8388736U /*0x800080*/,
    8388737U,
    1U,
    8396801U,
    8321U,
    8321U,
    128U /*0x80*/,
    8396929U,
    129U,
    1U,
    8192U /*0x2000*/,
    8388609U /*0x800001*/,
    8193U,
    8396928U,
    8388737U,
    8193U,
    8320U,
    8388608U /*0x800000*/,
    8396801U,
    128U /*0x80*/,
    8388608U /*0x800000*/,
    8192U /*0x2000*/,
    8396928U
  };
  private static readonly uint[] Sp5 = new uint[64 /*0x40*/]
  {
    256U /*0x0100*/,
    34078976U /*0x02080100*/,
    34078720U /*0x02080000*/,
    1107296512U /*0x42000100*/,
    524288U /*0x080000*/,
    256U /*0x0100*/,
    1073741824U /*0x40000000*/,
    34078720U /*0x02080000*/,
    1074266368U /*0x40080100*/,
    524288U /*0x080000*/,
    33554688U /*0x02000100*/,
    1074266368U /*0x40080100*/,
    1107296512U /*0x42000100*/,
    1107820544U /*0x42080000*/,
    524544U /*0x080100*/,
    1073741824U /*0x40000000*/,
    33554432U /*0x02000000*/,
    1074266112U /*0x40080000*/,
    1074266112U /*0x40080000*/,
    0U,
    1073742080U /*0x40000100*/,
    1107820800U,
    1107820800U,
    33554688U /*0x02000100*/,
    1107820544U /*0x42080000*/,
    1073742080U /*0x40000100*/,
    0U,
    1107296256U /*0x42000000*/,
    34078976U /*0x02080100*/,
    33554432U /*0x02000000*/,
    1107296256U /*0x42000000*/,
    524544U /*0x080100*/,
    524288U /*0x080000*/,
    1107296512U /*0x42000100*/,
    256U /*0x0100*/,
    33554432U /*0x02000000*/,
    1073741824U /*0x40000000*/,
    34078720U /*0x02080000*/,
    1107296512U /*0x42000100*/,
    1074266368U /*0x40080100*/,
    33554688U /*0x02000100*/,
    1073741824U /*0x40000000*/,
    1107820544U /*0x42080000*/,
    34078976U /*0x02080100*/,
    1074266368U /*0x40080100*/,
    256U /*0x0100*/,
    33554432U /*0x02000000*/,
    1107820544U /*0x42080000*/,
    1107820800U,
    524544U /*0x080100*/,
    1107296256U /*0x42000000*/,
    1107820800U,
    34078720U /*0x02080000*/,
    0U,
    1074266112U /*0x40080000*/,
    1107296256U /*0x42000000*/,
    524544U /*0x080100*/,
    33554688U /*0x02000100*/,
    1073742080U /*0x40000100*/,
    524288U /*0x080000*/,
    0U,
    1074266112U /*0x40080000*/,
    34078976U /*0x02080100*/,
    1073742080U /*0x40000100*/
  };
  private static readonly uint[] Sp6 = new uint[64 /*0x40*/]
  {
    536870928U /*0x20000010*/,
    541065216U /*0x20400000*/,
    16384U /*0x4000*/,
    541081616U,
    541065216U /*0x20400000*/,
    16U /*0x10*/,
    541081616U,
    4194304U /*0x400000*/,
    536887296U /*0x20004000*/,
    4210704U,
    4194304U /*0x400000*/,
    536870928U /*0x20000010*/,
    4194320U /*0x400010*/,
    536887296U /*0x20004000*/,
    536870912U /*0x20000000*/,
    16400U,
    0U,
    4194320U /*0x400010*/,
    536887312U /*0x20004010*/,
    16384U /*0x4000*/,
    4210688U /*0x404000*/,
    536887312U /*0x20004010*/,
    16U /*0x10*/,
    541065232U /*0x20400010*/,
    541065232U /*0x20400010*/,
    0U,
    4210704U,
    541081600U /*0x20404000*/,
    16400U,
    4210688U /*0x404000*/,
    541081600U /*0x20404000*/,
    536870912U /*0x20000000*/,
    536887296U /*0x20004000*/,
    16U /*0x10*/,
    541065232U /*0x20400010*/,
    4210688U /*0x404000*/,
    541081616U,
    4194304U /*0x400000*/,
    16400U,
    536870928U /*0x20000010*/,
    4194304U /*0x400000*/,
    536887296U /*0x20004000*/,
    536870912U /*0x20000000*/,
    16400U,
    536870928U /*0x20000010*/,
    541081616U,
    4210688U /*0x404000*/,
    541065216U /*0x20400000*/,
    4210704U,
    541081600U /*0x20404000*/,
    0U,
    541065232U /*0x20400010*/,
    16U /*0x10*/,
    16384U /*0x4000*/,
    541065216U /*0x20400000*/,
    4210704U,
    16384U /*0x4000*/,
    4194320U /*0x400010*/,
    536887312U /*0x20004010*/,
    0U,
    541081600U /*0x20404000*/,
    536870912U /*0x20000000*/,
    4194320U /*0x400010*/,
    536887312U /*0x20004010*/
  };
  private static readonly uint[] Sp7 = new uint[64 /*0x40*/]
  {
    2097152U /*0x200000*/,
    69206018U /*0x04200002*/,
    67110914U /*0x04000802*/,
    0U,
    2048U /*0x0800*/,
    67110914U /*0x04000802*/,
    2099202U,
    69208064U /*0x04200800*/,
    69208066U,
    2097152U /*0x200000*/,
    0U,
    67108866U /*0x04000002*/,
    2U,
    67108864U /*0x04000000*/,
    69206018U /*0x04200002*/,
    2050U,
    67110912U /*0x04000800*/,
    2099202U,
    2097154U /*0x200002*/,
    67110912U /*0x04000800*/,
    67108866U /*0x04000002*/,
    69206016U /*0x04200000*/,
    69208064U /*0x04200800*/,
    2097154U /*0x200002*/,
    69206016U /*0x04200000*/,
    2048U /*0x0800*/,
    2050U,
    69208066U,
    2099200U /*0x200800*/,
    2U,
    67108864U /*0x04000000*/,
    2099200U /*0x200800*/,
    67108864U /*0x04000000*/,
    2099200U /*0x200800*/,
    2097152U /*0x200000*/,
    67110914U /*0x04000802*/,
    67110914U /*0x04000802*/,
    69206018U /*0x04200002*/,
    69206018U /*0x04200002*/,
    2U,
    2097154U /*0x200002*/,
    67108864U /*0x04000000*/,
    67110912U /*0x04000800*/,
    2097152U /*0x200000*/,
    69208064U /*0x04200800*/,
    2050U,
    2099202U,
    69208064U /*0x04200800*/,
    2050U,
    67108866U /*0x04000002*/,
    69208066U,
    69206016U /*0x04200000*/,
    2099200U /*0x200800*/,
    0U,
    2U,
    69208066U,
    0U,
    2099202U,
    69206016U /*0x04200000*/,
    2048U /*0x0800*/,
    67108866U /*0x04000002*/,
    67110912U /*0x04000800*/,
    2048U /*0x0800*/,
    2097154U /*0x200002*/
  };
  private static readonly uint[] Sp8 = new uint[64 /*0x40*/]
  {
    268439616U /*0x10001040*/,
    4096U /*0x1000*/,
    262144U /*0x040000*/,
    268701760U,
    268435456U /*0x10000000*/,
    268439616U /*0x10001040*/,
    64U /*0x40*/,
    268435456U /*0x10000000*/,
    262208U /*0x040040*/,
    268697600U /*0x10040000*/,
    268701760U,
    266240U /*0x041000*/,
    268701696U /*0x10041000*/,
    266304U,
    4096U /*0x1000*/,
    64U /*0x40*/,
    268697600U /*0x10040000*/,
    268435520U /*0x10000040*/,
    268439552U /*0x10001000*/,
    4160U,
    266240U /*0x041000*/,
    262208U /*0x040040*/,
    268697664U /*0x10040040*/,
    268701696U /*0x10041000*/,
    4160U,
    0U,
    0U,
    268697664U /*0x10040040*/,
    268435520U /*0x10000040*/,
    268439552U /*0x10001000*/,
    266304U,
    262144U /*0x040000*/,
    266304U,
    262144U /*0x040000*/,
    268701696U /*0x10041000*/,
    4096U /*0x1000*/,
    64U /*0x40*/,
    268697664U /*0x10040040*/,
    4096U /*0x1000*/,
    266304U,
    268439552U /*0x10001000*/,
    64U /*0x40*/,
    268435520U /*0x10000040*/,
    268697600U /*0x10040000*/,
    268697664U /*0x10040040*/,
    268435456U /*0x10000000*/,
    262144U /*0x040000*/,
    268439616U /*0x10001040*/,
    0U,
    268701760U,
    262208U /*0x040040*/,
    268435520U /*0x10000040*/,
    268697600U /*0x10040000*/,
    268439552U /*0x10001000*/,
    268439616U /*0x10001040*/,
    0U,
    268701760U,
    266240U /*0x041000*/,
    266240U /*0x041000*/,
    4160U,
    4160U,
    262208U /*0x040040*/,
    268435456U /*0x10000000*/,
    268701696U /*0x10041000*/
  };
  private int[] m_keys;

  internal virtual int[] Keys => this.m_keys;

  public virtual string AlgorithmName => "DES";

  public bool IsBlock => false;

  public virtual void Initialize(bool isEncryption, ICipherParam parameters)
  {
    if (!(parameters is KeyParameter))
      throw new ArgumentException("Invalid parameter");
    this.m_keys = DataEncryption.GenerateWorkingKey(isEncryption, ((KeyParameter) parameters).Keys);
  }

  public virtual int BlockSize => 8;

  public virtual int ProcessBlock(
    byte[] inputBytes,
    int inOffset,
    byte[] outputBytes,
    int outOffset)
  {
    if (this.m_keys == null)
      throw new InvalidOperationException("Data Encryption Standard engine not initialised");
    if (inOffset + 8 > inputBytes.Length)
      throw new Exception("Invalid length in input bytes");
    if (outOffset + 8 > outputBytes.Length)
      throw new Exception("Invalid length in output bytes");
    DataEncryption.EncryptData(this.m_keys, inputBytes, inOffset, outputBytes, outOffset);
    return 8;
  }

  public virtual void Reset()
  {
  }

  protected static int[] GenerateWorkingKey(bool isEncrypt, byte[] bytes)
  {
    int[] workingKey = new int[32 /*0x20*/];
    bool[] flagArray1 = new bool[56];
    bool[] flagArray2 = new bool[56];
    for (int index = 0; index < 56; ++index)
    {
      int num = (int) DataEncryption.Pc1[index];
      flagArray1[index] = ((int) bytes[(IntPtr) (uint) (num >>> 3)] & (int) DataEncryption.ByteBit[num & 7]) != 0;
    }
    for (int index1 = 0; index1 < 16 /*0x10*/; ++index1)
    {
      int index2 = !isEncrypt ? 15 - index1 << 1 : index1 << 1;
      int index3 = index2 + 1;
      workingKey[index2] = workingKey[index3] = 0;
      for (int index4 = 0; index4 < 28; ++index4)
      {
        int index5 = index4 + (int) DataEncryption.ToTrot[index1];
        flagArray2[index4] = index5 >= 28 ? flagArray1[index5 - 28] : flagArray1[index5];
      }
      for (int index6 = 28; index6 < 56; ++index6)
      {
        int index7 = index6 + (int) DataEncryption.ToTrot[index1];
        flagArray2[index6] = index7 >= 56 ? flagArray1[index7 - 28] : flagArray1[index7];
      }
      for (int index8 = 0; index8 < 24; ++index8)
      {
        if (flagArray2[(int) DataEncryption.Pc2[index8]])
          workingKey[index2] |= DataEncryption.BigByte[index8];
        if (flagArray2[(int) DataEncryption.Pc2[index8 + 24]])
          workingKey[index3] |= DataEncryption.BigByte[index8];
      }
    }
    for (int index = 0; index != 32 /*0x20*/; index += 2)
    {
      int num1 = workingKey[index];
      int num2 = workingKey[index + 1];
      workingKey[index] = (num1 & 16515072 /*0xFC0000*/) << 6 | (num1 & 4032) << 10 | (num2 & 16515072 /*0xFC0000*/) >>> 10 | (num2 & 4032) >>> 6;
      workingKey[index + 1] = (num1 & 258048 /*0x03F000*/) << 12 | (num1 & 63 /*0x3F*/) << 16 /*0x10*/ | (num2 & 258048 /*0x03F000*/) >>> 4 | num2 & 63 /*0x3F*/;
    }
    return workingKey;
  }

  internal static void EncryptData(
    int[] keys,
    byte[] inputBytes,
    int inOffset,
    byte[] outBytes,
    int outOffset)
  {
    uint uint32_1 = Asn1Constants.BeToUInt32(inputBytes, inOffset);
    uint uint32_2 = Asn1Constants.BeToUInt32(inputBytes, inOffset + 4);
    uint num1 = (uint) (((int) (uint32_1 >> 4) ^ (int) uint32_2) & 252645135);
    uint num2 = uint32_2 ^ num1;
    uint num3 = uint32_1 ^ num1 << 4;
    uint num4 = (uint) (((int) (num3 >> 16 /*0x10*/) ^ (int) num2) & (int) ushort.MaxValue);
    uint num5 = num2 ^ num4;
    uint num6 = num3 ^ num4 << 16 /*0x10*/;
    uint num7 = (uint) (((int) (num5 >> 2) ^ (int) num6) & 858993459 /*0x33333333*/);
    uint num8 = num6 ^ num7;
    uint num9 = num5 ^ num7 << 2;
    uint num10 = (uint) (((int) (num9 >> 8) ^ (int) num8) & 16711935);
    uint num11 = num8 ^ num10;
    uint num12 = num9 ^ num10 << 8;
    uint num13 = num12 << 1 | num12 >> 31 /*0x1F*/;
    uint num14 = (uint) (((int) num11 ^ (int) num13) & -1431655766 /*0xAAAAAAAA*/);
    uint num15 = num11 ^ num14;
    uint num16 = num13 ^ num14;
    uint num17 = num15 << 1 | num15 >> 31 /*0x1F*/;
    for (int index = 0; index < 8; ++index)
    {
      uint num18 = (num16 << 28 | num16 >> 4) ^ (uint) keys[index * 4];
      uint num19 = DataEncryption.Sp7[(IntPtr) (num18 & 63U /*0x3F*/)] | DataEncryption.Sp5[(IntPtr) (num18 >> 8 & 63U /*0x3F*/)] | DataEncryption.Sp3[(IntPtr) (num18 >> 16 /*0x10*/ & 63U /*0x3F*/)] | DataEncryption.Sp1[(IntPtr) (num18 >> 24 & 63U /*0x3F*/)];
      uint num20 = num16 ^ (uint) keys[index * 4 + 1];
      uint num21 = num19 | DataEncryption.Sp8[(IntPtr) (num20 & 63U /*0x3F*/)] | DataEncryption.Sp6[(IntPtr) (num20 >> 8 & 63U /*0x3F*/)] | DataEncryption.Sp4[(IntPtr) (num20 >> 16 /*0x10*/ & 63U /*0x3F*/)] | DataEncryption.Sp2[(IntPtr) (num20 >> 24 & 63U /*0x3F*/)];
      num17 ^= num21;
      uint num22 = (num17 << 28 | num17 >> 4) ^ (uint) keys[index * 4 + 2];
      uint num23 = DataEncryption.Sp7[(IntPtr) (num22 & 63U /*0x3F*/)] | DataEncryption.Sp5[(IntPtr) (num22 >> 8 & 63U /*0x3F*/)] | DataEncryption.Sp3[(IntPtr) (num22 >> 16 /*0x10*/ & 63U /*0x3F*/)] | DataEncryption.Sp1[(IntPtr) (num22 >> 24 & 63U /*0x3F*/)];
      uint num24 = num17 ^ (uint) keys[index * 4 + 3];
      uint num25 = num23 | DataEncryption.Sp8[(IntPtr) (num24 & 63U /*0x3F*/)] | DataEncryption.Sp6[(IntPtr) (num24 >> 8 & 63U /*0x3F*/)] | DataEncryption.Sp4[(IntPtr) (num24 >> 16 /*0x10*/ & 63U /*0x3F*/)] | DataEncryption.Sp2[(IntPtr) (num24 >> 24 & 63U /*0x3F*/)];
      num16 ^= num25;
    }
    uint num26 = num16 << 31 /*0x1F*/ | num16 >> 1;
    uint num27 = (uint) (((int) num17 ^ (int) num26) & -1431655766 /*0xAAAAAAAA*/);
    uint num28 = num17 ^ num27;
    uint num29 = num26 ^ num27;
    uint num30 = num28 << 31 /*0x1F*/ | num28 >> 1;
    uint num31 = (uint) (((int) (num30 >> 8) ^ (int) num29) & 16711935);
    uint num32 = num29 ^ num31;
    uint num33 = num30 ^ num31 << 8;
    uint num34 = (uint) (((int) (num33 >> 2) ^ (int) num32) & 858993459 /*0x33333333*/);
    uint num35 = num32 ^ num34;
    uint num36 = num33 ^ num34 << 2;
    uint num37 = (uint) (((int) (num35 >> 16 /*0x10*/) ^ (int) num36) & (int) ushort.MaxValue);
    uint num38 = num36 ^ num37;
    uint num39 = num35 ^ num37 << 16 /*0x10*/;
    uint num40 = (uint) (((int) (num39 >> 4) ^ (int) num38) & 252645135);
    uint n = num38 ^ num40;
    Asn1Constants.UInt32ToBe(num39 ^ num40 << 4, outBytes, outOffset);
    Asn1Constants.UInt32ToBe(n, outBytes, outOffset + 4);
  }
}
