// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Security.SecurityHelper2010
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Security;

internal sealed class SecurityHelper2010
{
  internal const string SHA1Algorithm = "SHA-1";
  internal const string SHA256Alogrithm = "SHA-256";
  internal const string SHA512Alogrithm = "SHA-512";

  internal static byte[] Encrypt(byte[] arrPlainData, byte[] arrKey, byte[] arrIV, int blockSize)
  {
    int num = 0;
    Aes aes = new Aes(Aes.KeySize.Bits128, arrKey);
    byte[] dst = new byte[arrPlainData.Length];
    byte[] numArray1 = new byte[blockSize];
    Buffer.BlockCopy((Array) arrPlainData, 0, (Array) numArray1, 0, numArray1.Length);
    byte[] input1 = SecurityHelper2010.XOR(numArray1, arrIV);
    byte[] numArray2 = new byte[blockSize];
    aes.Cipher(input1, numArray2);
    Buffer.BlockCopy((Array) numArray2, 0, (Array) dst, 0, numArray2.Length);
    for (int index = num + blockSize; index < arrPlainData.Length; index += blockSize)
    {
      byte[] numArray3 = new byte[blockSize];
      Buffer.BlockCopy((Array) arrPlainData, index, (Array) numArray3, 0, numArray1.Length);
      byte[] input2 = SecurityHelper2010.XOR(numArray3, numArray2);
      aes.Cipher(input2, numArray2);
      Buffer.BlockCopy((Array) numArray2, 0, (Array) dst, index, numArray2.Length);
    }
    return dst;
  }

  internal static byte[] Decrypt(
    byte[] arrCipherData,
    byte[] arrKey,
    byte[] arrIV,
    int keySize,
    int actualLength)
  {
    int num1 = 0;
    byte[] numArray1 = new byte[arrCipherData.Length];
    Aes.KeySize keySize1 = Aes.KeySize.Bits128;
    if (arrKey.Length == 32 /*0x20*/)
      keySize1 = Aes.KeySize.Bits256;
    Aes aes = new Aes(keySize1, arrKey);
    byte[] numArray2 = new byte[keySize];
    Buffer.BlockCopy((Array) arrCipherData, 0, (Array) numArray2, 0, numArray2.Length);
    byte[] numArray3 = new byte[keySize];
    aes.InvCipher(numArray2, numArray3);
    byte[] src1 = SecurityHelper2010.XOR(numArray3, arrIV);
    Buffer.BlockCopy((Array) src1, 0, (Array) numArray1, 0, src1.Length);
    int num2 = num1 + src1.Length;
    int num3 = arrCipherData.Length - num2;
    while (num2 <= num3)
    {
      byte[] numArray4 = new byte[keySize];
      Buffer.BlockCopy((Array) arrCipherData, num2, (Array) numArray4, 0, numArray4.Length);
      byte[] numArray5 = new byte[keySize];
      aes.InvCipher(numArray4, numArray5);
      byte[] src2 = SecurityHelper2010.XOR(numArray5, numArray2);
      Buffer.BlockCopy((Array) src2, 0, (Array) numArray1, num2, src2.Length);
      num2 += src2.Length;
      numArray2 = numArray4;
    }
    SecurityHelper2010.TryPadOrTruncate(numArray1, actualLength, (byte) 0);
    return numArray1;
  }

  internal static byte[] CreateKey(
    byte[] arrPassword,
    byte[] arrSalt,
    byte[] arrBlock_Key,
    int spinCount,
    int keySize,
    string hashAlgorithm)
  {
    if (hashAlgorithm == "SHA512")
    {
      SHA512CryptoServiceProvider cryptoServiceProvider = new SHA512CryptoServiceProvider();
      byte[] buffer1 = SecurityHelper.CombineArray(arrSalt, arrPassword);
      byte[] hash = cryptoServiceProvider.ComputeHash(buffer1);
      for (uint index = 0; (long) index < (long) spinCount; ++index)
      {
        byte[] buffer2 = SecurityHelper.CombineArray(BitConverter.GetBytes(index), hash);
        hash = cryptoServiceProvider.ComputeHash(buffer2);
      }
      byte[] buffer3 = SecurityHelper.CombineArray(hash, arrBlock_Key);
      return SecurityHelper2010.TryPadOrTruncate(cryptoServiceProvider.ComputeHash(buffer3), keySize, (byte) 54);
    }
    SHA1CryptoServiceProvider cryptoServiceProvider1 = new SHA1CryptoServiceProvider();
    byte[] buffer4 = SecurityHelper.CombineArray(arrSalt, arrPassword);
    byte[] hash1 = cryptoServiceProvider1.ComputeHash(buffer4);
    for (uint index = 0; (long) index < (long) spinCount; ++index)
    {
      byte[] buffer5 = SecurityHelper.CombineArray(BitConverter.GetBytes(index), hash1);
      hash1 = cryptoServiceProvider1.ComputeHash(buffer5);
    }
    byte[] buffer6 = SecurityHelper.CombineArray(hash1, arrBlock_Key);
    return SecurityHelper2010.TryPadOrTruncate(cryptoServiceProvider1.ComputeHash(buffer6), keySize, (byte) 54);
  }

  internal static byte[] GenerateVector(byte[] arrSalt, uint segmentNumber, string hashAlgorithm)
  {
    if (hashAlgorithm == "SHA512")
    {
      SHA512CryptoServiceProvider cryptoServiceProvider = new SHA512CryptoServiceProvider();
      byte[] bytes = BitConverter.GetBytes(segmentNumber);
      byte[] buffer = SecurityHelper.CombineArray(arrSalt, bytes);
      byte[] hash = cryptoServiceProvider.ComputeHash(buffer);
      byte[] dst = new byte[16 /*0x10*/];
      Buffer.BlockCopy((Array) hash, 0, (Array) dst, 0, dst.Length);
      return dst;
    }
    SHA1CryptoServiceProvider cryptoServiceProvider1 = new SHA1CryptoServiceProvider();
    byte[] bytes1 = BitConverter.GetBytes(segmentNumber);
    byte[] buffer1 = SecurityHelper.CombineArray(arrSalt, bytes1);
    byte[] hash1 = cryptoServiceProvider1.ComputeHash(buffer1);
    byte[] dst1 = new byte[16 /*0x10*/];
    Buffer.BlockCopy((Array) hash1, 0, (Array) dst1, 0, dst1.Length);
    return dst1;
  }

  internal static byte[] GenerateVector(byte[] arrSalt, byte[] blockKey, string hashAlgorithm)
  {
    if (hashAlgorithm == "SHA512")
    {
      byte[] hash = new SHA512CryptoServiceProvider().ComputeHash(SecurityHelper.CombineArray(arrSalt, blockKey));
      byte[] dst = new byte[16 /*0x10*/];
      Buffer.BlockCopy((Array) hash, 0, (Array) dst, 0, dst.Length);
      return dst;
    }
    byte[] hash1 = new SHA1CryptoServiceProvider().ComputeHash(SecurityHelper.CombineArray(arrSalt, blockKey));
    byte[] dst1 = new byte[16 /*0x10*/];
    Buffer.BlockCopy((Array) hash1, 0, (Array) dst1, 0, dst1.Length);
    return dst1;
  }

  internal static byte[] Hash(byte[] input, string hashAlgorithm)
  {
    return hashAlgorithm == "SHA512" ? new SHA512CryptoServiceProvider().ComputeHash(input) : new SHA1CryptoServiceProvider().ComputeHash(input);
  }

  internal static byte[] XOR(byte[] fByte, byte[] sByte)
  {
    byte[] numArray = new byte[fByte.Length];
    for (int index = 0; index < numArray.Length; ++index)
      numArray[index] = (byte) ((uint) fByte[index] ^ (uint) sByte[index]);
    return numArray;
  }

  internal static byte[] TryPadOrTruncate(byte[] arrData, int length, byte padValue)
  {
    byte[] dst = new byte[length];
    int num = length - arrData.Length;
    if (num == 0)
      return arrData;
    if (num < 0)
    {
      Buffer.BlockCopy((Array) arrData, 0, (Array) dst, 0, dst.Length);
    }
    else
    {
      byte[] src = new byte[1]{ padValue };
      Buffer.BlockCopy((Array) arrData, 0, (Array) dst, 0, arrData.Length);
      for (int length1 = arrData.Length; length1 < length; length1 += src.Length)
        Buffer.BlockCopy((Array) src, 0, (Array) dst, length1, src.Length);
    }
    return dst;
  }

  internal static HashAlgorithm GetAlgorithm(string algorithmName)
  {
    switch (algorithmName)
    {
      case "SHA-1":
        return (HashAlgorithm) new SHA1CryptoServiceProvider();
      case "SHA-256":
        return (HashAlgorithm) new SHA256CryptoServiceProvider();
      case "SHA-512":
        return (HashAlgorithm) new SHA512CryptoServiceProvider();
      default:
        return (HashAlgorithm) new SHA1CryptoServiceProvider();
    }
  }

  internal static bool VerifyPassword(
    string password,
    string algorithmName,
    byte[] saltValue,
    byte[] hashValue,
    uint spinCount)
  {
    HashAlgorithm hashAlgorithm = algorithmName != null ? SecurityHelper2010.GetAlgorithm(algorithmName) : throw new ArgumentNullException("Alogithm not found");
    byte[] bytes1 = Encoding.Unicode.GetBytes(password);
    byte[] buffer1 = SecurityHelper.CombineArray(saltValue, bytes1);
    byte[] hash = hashAlgorithm.ComputeHash(buffer1);
    for (uint index = 0; index < spinCount; ++index)
    {
      byte[] bytes2 = BitConverter.GetBytes(index);
      byte[] buffer2 = SecurityHelper.CombineArray(hash, bytes2);
      hash = hashAlgorithm.ComputeHash(buffer2);
    }
    return BiffRecordRaw.CompareArrays(hash, 0, hashValue, 0, hashValue.Length);
  }
}
