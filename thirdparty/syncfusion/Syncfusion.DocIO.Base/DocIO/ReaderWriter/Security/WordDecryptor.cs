// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Security.WordDecryptor
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Security;

[CLSCompliant(false)]
internal class WordDecryptor
{
  private const int DEF_READ_LENGTH = 16 /*0x10*/;
  private const int DEF_PAS_LEN = 64 /*0x40*/;
  private const int DEF_BLOCK_SIZE = 512 /*0x0200*/;
  private const int DEF_START_POS = 0;
  private const int DEF_INC_BYTE_MAXVAL = 256 /*0x0100*/;
  private const uint DEF_PASSWORD_CONST = 52811;
  private static readonly ushort[] initCodeArr = new ushort[15]
  {
    (ushort) 57840,
    (ushort) 7439,
    (ushort) 52380,
    (ushort) 33984,
    (ushort) 4364,
    (ushort) 3600,
    (ushort) 61902,
    (ushort) 12606,
    (ushort) 6258,
    (ushort) 57657,
    (ushort) 54287,
    (ushort) 34041,
    (ushort) 10252,
    (ushort) 43370,
    (ushort) 20163
  };
  private static readonly ushort[,] encryptMatrix = new ushort[7, 15]
  {
    {
      (ushort) 44796,
      (ushort) 31585,
      (ushort) 17763,
      (ushort) 885,
      (ushort) 55369,
      (ushort) 28485,
      (ushort) 60195,
      (ushort) 18387,
      (ushort) 47201,
      (ushort) 17824,
      (ushort) 43601,
      (ushort) 30388,
      (ushort) 14128,
      (ushort) 13105,
      (ushort) 4129
    },
    {
      (ushort) 19929,
      (ushort) 63170,
      (ushort) 35526,
      (ushort) 1770,
      (ushort) 41139,
      (ushort) 56970,
      (ushort) 50791,
      (ushort) 36774,
      (ushort) 24803,
      (ushort) 35648,
      (ushort) 17539,
      (ushort) 60776,
      (ushort) 28256,
      (ushort) 26210,
      (ushort) 8258
    },
    {
      (ushort) 39858,
      (ushort) 64933,
      (ushort) 1453,
      (ushort) 3540,
      (ushort) 20807,
      (ushort) 44341,
      (ushort) 40175,
      (ushort) 3949,
      (ushort) 49606,
      (ushort) 1697,
      (ushort) 35078,
      (ushort) 51953,
      (ushort) 56512,
      (ushort) 52420,
      (ushort) 16516
    },
    {
      (ushort) 10053,
      (ushort) 60267,
      (ushort) 2906,
      (ushort) 7080,
      (ushort) 41614,
      (ushort) 19019,
      (ushort) 10751,
      (ushort) 7898,
      (ushort) 37805,
      (ushort) 3394,
      (ushort) 557,
      (ushort) 34243,
      (ushort) 43425,
      (ushort) 35241,
      (ushort) 33032
    },
    {
      (ushort) 20106,
      (ushort) 50935,
      (ushort) 5812,
      (ushort) 14160,
      (ushort) 21821,
      (ushort) 38038,
      (ushort) 21502,
      (ushort) 15796,
      (ushort) 14203,
      (ushort) 6788,
      (ushort) 1114,
      (ushort) 7079,
      (ushort) 17251,
      (ushort) 883,
      (ushort) 4657
    },
    {
      (ushort) 40212,
      (ushort) 40399,
      (ushort) 11624,
      (ushort) 28320,
      (ushort) 43642,
      (ushort) 14605,
      (ushort) 43004,
      (ushort) 31592,
      (ushort) 28406,
      (ushort) 13576,
      (ushort) 2228,
      (ushort) 14158,
      (ushort) 34502,
      (ushort) 1766,
      (ushort) 9314
    },
    {
      (ushort) 10761,
      (ushort) 11199,
      (ushort) 23248,
      (ushort) 56640,
      (ushort) 17621,
      (ushort) 29210,
      (ushort) 24537,
      (ushort) 63184,
      (ushort) 56812,
      (ushort) 27152,
      (ushort) 4456,
      (ushort) 28316,
      (ushort) 7597,
      (ushort) 3532,
      (ushort) 18628
    }
  };
  private byte[] m_baDocumentID = new byte[16 /*0x10*/];
  private byte[] m_baPoint = new byte[64 /*0x40*/];
  private byte[] m_baHash = new byte[16 /*0x10*/];
  private byte[] m_baPassword = new byte[64 /*0x40*/];
  private MD5Context m_valContext = new MD5Context();
  private MemoryStream m_tableStream;
  private MemoryStream m_dataStream;
  private MemoryStream m_mainStream;
  private Fib m_fib;
  private bool m_bIsComplexFile;
  private WordKey m_key;

  internal MemoryStream TableStream => this.m_tableStream;

  internal MemoryStream MainStream => this.m_mainStream;

  internal MemoryStream DataStream => this.m_dataStream;

  internal WordDecryptor()
  {
  }

  internal WordDecryptor(
    MemoryStream tableStream,
    MemoryStream mainStream,
    MemoryStream dataStream,
    Fib fib)
  {
    this.m_tableStream = tableStream;
    this.m_mainStream = mainStream;
    this.m_dataStream = dataStream;
    this.m_fib = fib;
    this.m_bIsComplexFile = fib.FComplex;
  }

  public void TestEncrypt(
    ref MemoryStream stream,
    string password,
    ref byte[] docID,
    ref byte[] point,
    ref byte[] hash)
  {
    this.m_baDocumentID = docID;
    this.ConvertPassword(password);
    this.PrepareValContext();
    Buffer.BlockCopy((Array) this.m_baDocumentID, 0, (Array) this.m_baPoint, 0, 16 /*0x10*/);
    this.m_baPoint[16 /*0x10*/] = (byte) 128 /*0x80*/;
    Array.Clear((Array) this.m_baPoint, 17, 47);
    this.m_baPoint[56] = (byte) 128 /*0x80*/;
    MD5Context md5Context = new MD5Context();
    md5Context.Update(this.m_baPoint, 64U /*0x40*/);
    md5Context.StoreDigest();
    Buffer.BlockCopy((Array) md5Context.Digest, 0, (Array) this.m_baHash, 0, 16 /*0x10*/);
    this.MakeKey(0U);
    this.DecryptBuffer(this.m_baPoint, 16 /*0x10*/);
    this.DecryptBuffer(this.m_baHash, 16 /*0x10*/);
    stream = this.DecryptStream(stream);
    docID = this.m_baDocumentID;
    point = this.m_baPoint;
    hash = this.m_baHash;
  }

  public void TestDecrypt(
    ref MemoryStream stream,
    string password,
    ref byte[] docid,
    ref byte[] point,
    ref byte[] hash)
  {
    Buffer.BlockCopy((Array) docid, 0, (Array) this.m_baDocumentID, 0, 16 /*0x10*/);
    Buffer.BlockCopy((Array) point, 0, (Array) this.m_baPoint, 0, 16 /*0x10*/);
    Buffer.BlockCopy((Array) hash, 0, (Array) this.m_baHash, 0, 16 /*0x10*/);
    this.ConvertPassword(password);
    this.VerifyPassword();
    stream = this.DecryptStream(stream);
  }

  internal bool CheckPassword(string password)
  {
    if (this.m_tableStream == null)
      throw new ArgumentNullException("Table Stream is null referenced.");
    this.m_tableStream.Position = 4L;
    this.m_tableStream.Read(this.m_baDocumentID, 0, 16 /*0x10*/);
    this.m_tableStream.Read(this.m_baPoint, 0, 16 /*0x10*/);
    this.m_tableStream.Read(this.m_baHash, 0, 16 /*0x10*/);
    this.ConvertPassword(password);
    return this.VerifyPassword();
  }

  internal void Decrypt()
  {
    this.m_tableStream = this.DecryptStream(this.m_tableStream);
    this.m_mainStream = this.DecryptStream(this.m_mainStream);
    if (this.m_dataStream == null)
      return;
    this.m_dataStream = this.DecryptStream(this.m_dataStream);
  }

  internal void Encrypt(string password)
  {
    this.m_baDocumentID = Guid.NewGuid().ToByteArray();
    this.ConvertPassword(password);
    this.PrepareValContext();
    Buffer.BlockCopy((Array) this.m_baDocumentID, 0, (Array) this.m_baPoint, 0, 16 /*0x10*/);
    this.m_baPoint[16 /*0x10*/] = (byte) 128 /*0x80*/;
    Array.Clear((Array) this.m_baPoint, 17, 47);
    this.m_baPoint[56] = (byte) 128 /*0x80*/;
    MD5Context md5Context = new MD5Context();
    md5Context.Update(this.m_baPoint, 64U /*0x40*/);
    md5Context.StoreDigest();
    Buffer.BlockCopy((Array) md5Context.Digest, 0, (Array) this.m_baHash, 0, 16 /*0x10*/);
    this.MakeKey(0U);
    this.DecryptBuffer(this.m_baPoint, 16 /*0x10*/);
    this.DecryptBuffer(this.m_baHash, 16 /*0x10*/);
    this.m_tableStream = this.DecryptStream(this.m_tableStream);
    this.m_tableStream.Position = 0L;
    this.m_tableStream.WriteByte((byte) 1);
    this.m_tableStream.WriteByte((byte) 0);
    this.m_tableStream.WriteByte((byte) 1);
    this.m_tableStream.WriteByte((byte) 0);
    this.m_tableStream.Write(this.m_baDocumentID, 0, 16 /*0x10*/);
    this.m_tableStream.Write(this.m_baPoint, 0, 16 /*0x10*/);
    this.m_tableStream.Write(this.m_baHash, 0, 16 /*0x10*/);
    this.m_mainStream = this.DecryptStream(this.m_mainStream);
    if (this.m_dataStream == null)
      return;
    this.m_dataStream = this.DecryptStream(this.m_dataStream);
  }

  private MemoryStream DecryptStream(MemoryStream stream)
  {
    byte[] numArray = new byte[16 /*0x10*/];
    MemoryStream memoryStream = new MemoryStream();
    long length = stream.Length;
    int num = 0;
    stream.Position = (long) num;
    uint block = 0;
    this.MakeKey(block);
    while ((long) num < length)
    {
      for (int index = stream.Read(numArray, 0, 16 /*0x10*/); index < 16 /*0x10*/; ++index)
        numArray[index] = (byte) 1;
      this.DecryptBuffer(numArray, 16 /*0x10*/);
      memoryStream.Write(numArray, 0, 16 /*0x10*/);
      num += 16 /*0x10*/;
      if (num % 512 /*0x0200*/ == 0)
      {
        ++block;
        this.MakeKey(block);
      }
    }
    memoryStream.Position = 0L;
    return memoryStream;
  }

  private void ConvertPassword(string password)
  {
    int index;
    for (index = 0; index < password.Length; ++index)
    {
      this.m_baPassword[2 * index] = (byte) ((uint) password[index] & (uint) byte.MaxValue);
      this.m_baPassword[2 * index + 1] = (byte) ((int) password[index] >> 8 & (int) byte.MaxValue);
    }
    this.m_baPassword[2 * index] = (byte) 128 /*0x80*/;
    this.m_baPassword[56] = (byte) (index << 4);
  }

  private void PrepareKey(byte[] data)
  {
    this.m_key = new WordKey();
    byte index1 = 0;
    byte index2 = 0;
    for (int index3 = 0; index3 < 256 /*0x0100*/; ++index3)
      this.m_key.status[index3] = (byte) index3;
    for (int index4 = 0; index4 < 256 /*0x0100*/; ++index4)
    {
      index2 = (byte) (((int) data[(int) index1] + (int) this.m_key.status[index4] + (int) index2) % 256 /*0x0100*/);
      byte statu = this.m_key.status[index4];
      this.m_key.status[index4] = this.m_key.status[(int) index2];
      this.m_key.status[(int) index2] = statu;
      index1 = (byte) (((int) index1 + 1) % 16 /*0x10*/);
    }
  }

  private void MakeKey(uint block)
  {
    MD5Context md5Context = new MD5Context();
    byte[] numArray = new byte[64 /*0x40*/];
    Buffer.BlockCopy((Array) this.m_valContext.Digest, 0, (Array) numArray, 0, 5);
    numArray[5] = (byte) (block & (uint) byte.MaxValue);
    numArray[6] = (byte) (block >> 8 & (uint) byte.MaxValue);
    numArray[7] = (byte) (block >> 16 /*0x10*/ & (uint) byte.MaxValue);
    numArray[8] = (byte) (block >> 24 & (uint) byte.MaxValue);
    numArray[9] = (byte) 128 /*0x80*/;
    numArray[56] = (byte) 72;
    md5Context.Update(numArray, 64U /*0x40*/);
    md5Context.StoreDigest();
    this.PrepareKey(md5Context.Digest);
  }

  private bool MemoryCompare(byte[] block1, byte[] block2, int length)
  {
    for (int index = 0; index < length; ++index)
    {
      if ((int) block1[index] != (int) block2[index])
        return false;
    }
    return true;
  }

  private bool VerifyPassword()
  {
    this.PrepareValContext();
    this.MakeKey(0U);
    this.DecryptBuffer(this.m_baPoint, 16 /*0x10*/);
    this.DecryptBuffer(this.m_baHash, 16 /*0x10*/);
    this.m_baPoint[16 /*0x10*/] = (byte) 128 /*0x80*/;
    Array.Clear((Array) this.m_baPoint, 17, 47);
    this.m_baPoint[56] = (byte) 128 /*0x80*/;
    MD5Context md5Context = new MD5Context();
    md5Context.Update(this.m_baPoint, 64U /*0x40*/);
    md5Context.StoreDigest();
    return this.MemoryCompare(md5Context.Digest, this.m_baHash, 16 /*0x10*/);
  }

  private void PrepareValContext()
  {
    MD5Context md5Context = new MD5Context();
    md5Context.Update(this.m_baPassword, 64U /*0x40*/);
    md5Context.StoreDigest();
    this.m_valContext = new MD5Context();
    int dstOffset1 = 0;
    int srcOffset = 0;
    int count = 5;
    while (dstOffset1 != 16 /*0x10*/)
    {
      if (64 /*0x40*/ - dstOffset1 < 5)
        count = 64 /*0x40*/ - dstOffset1;
      Buffer.BlockCopy((Array) md5Context.Digest, srcOffset, (Array) this.m_baPassword, dstOffset1, count);
      int dstOffset2 = dstOffset1 + count;
      if (dstOffset2 == 64 /*0x40*/)
      {
        this.m_valContext.Update(this.m_baPassword, 64U /*0x40*/);
        srcOffset = count;
        count = 5 - count;
        dstOffset1 = 0;
      }
      else
      {
        srcOffset = 0;
        count = 5;
        Buffer.BlockCopy((Array) this.m_baDocumentID, 0, (Array) this.m_baPassword, dstOffset2, 16 /*0x10*/);
        dstOffset1 = dstOffset2 + 16 /*0x10*/;
      }
    }
    this.m_baPassword[16 /*0x10*/] = (byte) 128 /*0x80*/;
    Array.Clear((Array) this.m_baPassword, 17, 47);
    this.m_baPassword[56] = (byte) 128 /*0x80*/;
    this.m_baPassword[57] = (byte) 10;
    this.m_valContext.Update(this.m_baPassword, 64U /*0x40*/);
    this.m_valContext.StoreDigest();
  }

  private void DecryptBuffer(byte[] data, int length)
  {
    for (int index1 = 0; index1 < length; ++index1)
    {
      this.m_key.x = (byte) (((int) this.m_key.x + 1) % 256 /*0x0100*/);
      this.m_key.y = (byte) (((int) this.m_key.status[(int) this.m_key.x] + (int) this.m_key.y) % 256 /*0x0100*/);
      byte statu = this.m_key.status[(int) this.m_key.x];
      this.m_key.status[(int) this.m_key.x] = this.m_key.status[(int) this.m_key.y];
      this.m_key.status[(int) this.m_key.y] = statu;
      byte index2 = (byte) (((int) this.m_key.status[(int) this.m_key.x] + (int) this.m_key.status[(int) this.m_key.y]) % 256 /*0x0100*/);
      data[index1] ^= this.m_key.status[(int) index2];
    }
  }

  internal static uint GetPasswordHash(string password)
  {
    if (string.IsNullOrEmpty(password))
      return 0;
    if (password.Length > 15)
      password = password.Substring(0, 15);
    ushort lowOrderHash = WordDecryptor.GetLowOrderHash(password);
    return (uint) WordDecryptor.GetHighOrderHash(password) << 16 /*0x10*/ | (uint) lowOrderHash;
  }

  private static uint RevertBytes(uint changeVal)
  {
    uint num = 0;
    for (int index = 0; index < 4; ++index)
    {
      num |= changeVal & (uint) byte.MaxValue;
      if (index < 3)
      {
        num <<= 8;
        changeVal >>= 8;
      }
    }
    return num;
  }

  private static ushort GetHighOrderHash(string password)
  {
    ushort highOrderHash = WordDecryptor.initCodeArr[password.Length - 1];
    int num = 15 - password.Length;
    int index1 = 0;
    for (int length = password.Length; index1 < length; ++index1)
    {
      bool[] charBits7 = WordDecryptor.GetCharBits7(password[index1]);
      for (int index2 = 0; index2 < 7; ++index2)
      {
        if (charBits7[index2])
          highOrderHash ^= WordDecryptor.encryptMatrix[index2, num + index1];
      }
    }
    return highOrderHash;
  }

  private static ushort GetLowOrderHash(string password)
  {
    if (password == null)
      return 0;
    ushort num = 0;
    int index = 0;
    for (int length = password.Length; index < length; ++index)
    {
      ushort uint16FromBits = WordDecryptor.GetUInt16FromBits(WordDecryptor.RotateBits(WordDecryptor.GetCharBits15(password[index]), index + 1));
      num ^= uint16FromBits;
    }
    return (ushort) ((ulong) ((int) num ^ password.Length) ^ 52811UL);
  }

  private static bool[] GetCharBits7(char charToConvert)
  {
    ushort num = 1;
    bool[] charBits7 = new bool[7];
    ushort uint16 = Convert.ToUInt16(charToConvert);
    if (((int) uint16 & (int) byte.MaxValue) == 0)
      uint16 >>= 8;
    for (int index = 0; index < 7; ++index)
    {
      charBits7[index] = ((int) uint16 & (int) num) == (int) num;
      num <<= 1;
    }
    return charBits7;
  }

  private static bool[] GetCharBits15(char charToConvert)
  {
    bool[] charBits15 = new bool[15];
    ushort uint16 = Convert.ToUInt16(charToConvert);
    ushort num = 1;
    for (int index = 0; index < 15; ++index)
    {
      charBits15[index] = ((int) uint16 & (int) num) == (int) num;
      num <<= 1;
    }
    return charBits15;
  }

  private static ushort GetUInt16FromBits(bool[] bits)
  {
    if (bits == null)
      throw new ArgumentNullException(nameof (bits));
    if (bits.Length > 16 /*0x10*/)
      throw new ArgumentOutOfRangeException("There can't be more than 16 bits");
    ushort uint16FromBits = 0;
    ushort num = 1;
    int index = 0;
    for (int length = bits.Length; index < length; ++index)
    {
      if (bits[index])
        uint16FromBits += num;
      num <<= 1;
    }
    return uint16FromBits;
  }

  private static bool[] RotateBits(bool[] bits, int count)
  {
    if (bits == null)
      throw new ArgumentNullException(nameof (bits));
    if (bits.Length == 0)
      return bits;
    if (count < 0)
      throw new ArgumentOutOfRangeException("count can't be less than zero");
    bool[] flagArray = new bool[bits.Length];
    int index1 = 0;
    for (int length = bits.Length; index1 < length; ++index1)
    {
      int index2 = (index1 + count) % length;
      flagArray[index2] = bits[index1];
    }
    return flagArray;
  }

  public static int Round(int value, int degree)
  {
    if (degree == 0)
      throw new ArgumentOutOfRangeException("degree can't be 0");
    int num = value % degree;
    return value - num + degree;
  }
}
