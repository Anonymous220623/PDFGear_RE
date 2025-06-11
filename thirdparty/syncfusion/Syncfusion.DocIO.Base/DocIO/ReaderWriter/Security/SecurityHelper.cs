// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Security.SecurityHelper
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.CompoundFile.DocIO;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Security;

[CLSCompliant(false)]
internal sealed class SecurityHelper
{
  private const int PasswordIterationCount = 50000;
  internal const string EncryptionInfoStream = "EncryptionInfo";
  internal const string DataSpacesStorage = "\u0006DataSpaces";
  internal const string DataSpaceMapStream = "DataSpaceMap";
  internal const string TransformPrimaryStream = "\u0006Primary";
  internal const string DataSpaceInfoStorage = "DataSpaceInfo";
  internal const string TransformInfoStorage = "TransformInfo";
  internal const string EncryptedPackageStream = "EncryptedPackage";
  internal const string StrongEncryptionDataSpaceStream = "StrongEncryptionDataSpace";
  internal const string StrongEncryptionTransformStream = "StrongEncryptionTransform";
  internal const string VersionStream = "Version";

  internal SecurityHelper.EncrytionType GetEncryptionType(ICompoundStorage storage)
  {
    SecurityHelper.EncrytionType encryptionType = SecurityHelper.EncrytionType.None;
    if (storage.ContainsStream("EncryptionInfo") && storage.ContainsStorage("\u0006DataSpaces"))
    {
      using (Stream stream = (Stream) storage.OpenStream("EncryptionInfo"))
      {
        byte[] buffer = new byte[4];
        int num = this.ReadInt32(stream, buffer);
        stream.Position = 0L;
        switch (num)
        {
          case 131075 /*0x020003*/:
          case 131076 /*0x020004*/:
            encryptionType = SecurityHelper.EncrytionType.Standard;
            break;
          case 262148 /*0x040004*/:
            encryptionType = SecurityHelper.EncrytionType.Agile;
            break;
        }
      }
    }
    return encryptionType;
  }

  internal int ReadInt32(Stream stream, byte[] buffer)
  {
    return stream.Read(buffer, 0, 4) == 4 ? BitConverter.ToInt32(buffer, 0) : throw new Exception("Invalid data");
  }

  internal string ReadUnicodeString(Stream stream)
  {
    byte[] buffer = new byte[4];
    int count = this.ReadInt32(stream, buffer);
    byte[] numArray = new byte[count];
    if (stream.Read(numArray, 0, count) != count)
      throw new Exception("Invalid data");
    string str = Encoding.Unicode.GetString(numArray, 0, numArray.Length);
    int num = count % 4;
    if (num != 0)
      stream.Position += (long) (4 - num);
    return str;
  }

  internal string ReadUnicodeStringZero(Stream stream)
  {
    StringBuilder stringBuilder = new StringBuilder();
    byte[] numArray = new byte[2];
    while (stream.Read(numArray, 0, 2) > 0)
    {
      string str = Encoding.Unicode.GetString(numArray, 0, numArray.Length);
      if (str[0] != char.MinValue)
        stringBuilder.Append(str);
      else
        break;
    }
    return stringBuilder.ToString();
  }

  internal void WriteInt32(Stream stream, int value)
  {
    byte[] bytes = BitConverter.GetBytes(value);
    stream.Write(bytes, 0, 4);
  }

  internal void WriteUnicodeString(Stream stream, string value)
  {
    byte[] bytes = Encoding.Unicode.GetBytes(value);
    int length = bytes.Length;
    this.WriteInt32(stream, length);
    stream.Write(bytes, 0, length);
    int num1 = length % 4;
    if (num1 == 0)
      return;
    int num2 = 0;
    for (int index = 4 - num1; num2 < index; ++num2)
      stream.WriteByte((byte) 0);
  }

  internal void WriteUnicodeStringZero(Stream stream, string value)
  {
    int length = value.Length;
    byte[] bytes = Encoding.Unicode.GetBytes(value);
    stream.Write(bytes, 0, bytes.Length);
    if (length != 0 && value[length - 1] == char.MinValue)
      return;
    stream.WriteByte((byte) 0);
    stream.WriteByte((byte) 0);
  }

  internal byte[] CreateKey(string password, byte[] salt, int keyLength)
  {
    SHA1 shA1 = (SHA1) new SHA1CryptoServiceProvider();
    byte[] bytes1 = Encoding.Unicode.GetBytes(password);
    byte[] numArray1 = new byte[salt.Length + bytes1.Length];
    Buffer.BlockCopy((Array) salt, 0, (Array) numArray1, 0, salt.Length);
    Buffer.BlockCopy((Array) bytes1, 0, (Array) numArray1, salt.Length, bytes1.Length);
    byte[] hash1 = shA1.ComputeHash(numArray1);
    byte[] numArray2 = new byte[hash1.Length + 4];
    byte[] src = hash1;
    for (int index = 0; index < 50000; ++index)
    {
      byte[] bytes2 = BitConverter.GetBytes(index);
      Buffer.BlockCopy((Array) bytes2, 0, (Array) numArray2, 0, bytes2.Length);
      Buffer.BlockCopy((Array) src, 0, (Array) numArray2, bytes2.Length, src.Length);
      src = shA1.ComputeHash(numArray2);
    }
    byte[] bytes3 = BitConverter.GetBytes(0);
    Buffer.BlockCopy((Array) src, 0, (Array) numArray2, 0, src.Length);
    Buffer.BlockCopy((Array) bytes3, 0, (Array) numArray2, src.Length, bytes3.Length);
    byte[] hash2 = shA1.ComputeHash(numArray2);
    byte[] buffer = new byte[64 /*0x40*/];
    for (int index = 0; index < 64 /*0x40*/; ++index)
      buffer[index] = (byte) 54;
    int index1 = 0;
    for (int length = hash2.Length; index1 < length; ++index1)
      buffer[index1] ^= hash2[index1];
    byte[] hash3 = shA1.ComputeHash(buffer);
    for (int index2 = 0; index2 < 64 /*0x40*/; ++index2)
      buffer[index2] = (byte) 92;
    int index3 = 0;
    for (int length = hash2.Length; index3 < length; ++index3)
      buffer[index3] ^= hash2[index3];
    shA1.ComputeHash(buffer);
    if (keyLength > hash3.Length)
      throw new NotImplementedException();
    byte[] dst = new byte[keyLength];
    Buffer.BlockCopy((Array) hash3, 0, (Array) dst, 0, keyLength);
    return dst;
  }

  internal byte[] CreateAgileEncryptionKey(
    HashAlgorithm hashAlgorithm,
    string password,
    byte[] salt,
    byte[] blockKey,
    int keyLength,
    int iterationCount)
  {
    byte[] bytes = Encoding.Unicode.GetBytes(password);
    byte[] hash = hashAlgorithm.ComputeHash(this.CombineArray(salt, bytes));
    for (int index = 0; index < iterationCount; ++index)
      hash = hashAlgorithm.ComputeHash(this.CombineArray(BitConverter.GetBytes(index), hash));
    return this.CorrectSize(hashAlgorithm.ComputeHash(this.CombineArray(hash, blockKey)), keyLength, (byte) 54);
  }

  internal byte[] EncryptDecrypt(
    byte[] data,
    SecurityHelper.EncryptionMethod method,
    int blockSize)
  {
    int length = data.Length;
    byte[] dst = new byte[length];
    byte[] numArray1 = new byte[blockSize];
    byte[] numArray2 = new byte[blockSize];
    for (int index = 0; index < length; index += blockSize)
    {
      int count = Math.Min(length - index, blockSize);
      Buffer.BlockCopy((Array) data, index, (Array) numArray1, 0, count);
      method(numArray1, numArray2);
      Buffer.BlockCopy((Array) numArray2, 0, (Array) dst, index, count);
    }
    return dst;
  }

  internal byte[] CombineArray(byte[] buffer1, byte[] buffer2)
  {
    int length1 = buffer1.Length;
    int length2 = buffer2.Length;
    byte[] dst = new byte[length1 + length2];
    Buffer.BlockCopy((Array) buffer1, 0, (Array) dst, 0, length1);
    Buffer.BlockCopy((Array) buffer2, 0, (Array) dst, length1, length2);
    return dst;
  }

  internal byte[] CorrectSize(byte[] data, int size, byte padding)
  {
    byte[] dst = new byte[size];
    if (data.Length < size)
    {
      Buffer.BlockCopy((Array) data, 0, (Array) dst, 0, data.Length);
      for (int length = data.Length; length < size; ++length)
        dst[length] = padding;
    }
    else if (data.Length >= size)
      Buffer.BlockCopy((Array) data, 0, (Array) dst, 0, size);
    return dst;
  }

  internal byte[] ConcatenateIV(byte[] data, byte[] IV)
  {
    byte[] numArray = new byte[data.Length];
    for (int index = 0; index < numArray.Length; ++index)
      numArray[index] = (byte) ((uint) data[index] ^ (uint) IV[index]);
    return numArray;
  }

  internal bool CompareArray(byte[] buffer1, byte[] buffer2)
  {
    bool flag = true;
    for (int index = 0; index < buffer1.Length; ++index)
    {
      if ((int) buffer1[index] != (int) buffer2[index])
      {
        flag = false;
        break;
      }
    }
    return flag;
  }

  internal enum EncrytionType
  {
    Standard,
    Agile,
    None,
  }

  internal delegate void EncryptionMethod(byte[] buffer1, byte[] buffer2);
}
