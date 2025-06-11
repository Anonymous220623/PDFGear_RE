// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Security.Excel2010Decryptor
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.CompoundFile.XlsIO;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Security;

internal class Excel2010Decryptor : Excel2007Decryptor
{
  public override Stream Decrypt()
  {
    if (this.m_arrKey == null)
      throw new InvalidOperationException("Incorrect password.");
    MemoryStream memoryStream = new MemoryStream();
    int blockSize = this.m_info.KeyInfo.BlockSize;
    using (CompoundStream EncryptedPackage = this.Storage.OpenStream("EncryptedPackage"))
    {
      if (!this.CheckDataIntegrity((Stream) EncryptedPackage, this.m_arrKey, this.m_info.DataEncryption.SaltValue, this.m_info.DataIntegrity.HMacKey, this.m_info.DataIntegrity.HMacValue, this.m_info.KeyInfo.HashAlgorithm))
        throw new InvalidOperationException("Not a valid encrypted file.");
      EncryptedPackage.Position = 0L;
      byte[] buffer = new byte[8];
      EncryptedPackage.Read(buffer, 0, 8);
      int int32 = BitConverter.ToInt32(buffer, 0);
      int actualLength = 4096 /*0x1000*/;
      byte[] numArray1 = new byte[EncryptedPackage.Length - 8L];
      EncryptedPackage.Read(numArray1, 0, numArray1.Length);
      int length = numArray1.Length;
      int num1 = length / actualLength;
      int num2 = length % actualLength;
      if (num2 > 0)
      {
        ++num1;
        byte[] numArray2 = new byte[length + actualLength - num2];
        Buffer.BlockCopy((Array) numArray1, 0, (Array) numArray2, 0, numArray1.Length);
        SecurityHelper2010.TryPadOrTruncate(numArray2, numArray2.Length, (byte) 0);
        numArray1 = numArray2;
      }
      byte[] numArray3 = new byte[numArray1.Length];
      byte[] numArray4 = new byte[actualLength];
      int num3 = 0;
      for (uint segmentNumber = 0; (long) segmentNumber < (long) num1; ++segmentNumber)
      {
        Buffer.BlockCopy((Array) numArray1, num3, (Array) numArray4, 0, numArray4.Length);
        byte[] vector = SecurityHelper2010.GenerateVector(this.m_info.DataEncryption.SaltValue, segmentNumber, this.m_info.KeyInfo.HashAlgorithm);
        byte[] src = SecurityHelper2010.Decrypt(numArray4, this.m_arrKey, vector, 16 /*0x10*/, actualLength);
        Buffer.BlockCopy((Array) src, 0, (Array) numArray3, num3, src.Length);
        num3 += actualLength;
      }
      return (Stream) new MemoryStream(SecurityHelper2010.TryPadOrTruncate(numArray3, int32, (byte) 0));
    }
  }

  public override bool CheckPassword(string password)
  {
    EncryptedKeyInfo keyInfo = this.m_info.KeyInfo;
    this.m_arrKey = this.VerifyPassword(password, keyInfo);
    return this.m_arrKey != null;
  }

  private byte[] VerifyPassword(string password, EncryptedKeyInfo verifier)
  {
    if (verifier == null)
      throw new ArgumentNullException("Key Verifier");
    byte[] numArray = (byte[]) null;
    byte[] saltValue = verifier.SaltValue;
    byte[] bytes = Encoding.Unicode.GetBytes(password);
    byte[] verifierHashInput = this.GetVerifierHashInput(verifier.VerifierHashInput, bytes, saltValue, verifier.SpinCount, verifier.KeyBits, verifier.BlockSize);
    byte[] verifierHashValue = this.GetVerifierHashValue(verifier.VerifierHashValue, bytes, saltValue, verifier.SpinCount, verifier.KeyBits);
    byte[] array1 = SecurityHelper2010.Hash(verifierHashInput, this.m_info.KeyInfo.HashAlgorithm);
    if (BiffRecordRaw.CompareArrays(array1, 0, verifierHashValue, 0, array1.Length))
      numArray = this.GetIntermediateKey(verifier.KeyValue, bytes, saltValue, verifier.SpinCount, verifier.KeyBits);
    return numArray;
  }

  internal byte[] GetIntermediateKey(
    byte[] arrEncryptedKeyValue,
    byte[] arrPassword,
    byte[] arrSalt,
    int spinCount,
    int keySize)
  {
    byte[] arrBlock_Key = new byte[8]
    {
      (byte) 20,
      (byte) 110,
      (byte) 11,
      (byte) 231,
      (byte) 171,
      (byte) 172,
      (byte) 208 /*0xD0*/,
      (byte) 214
    };
    byte[] key = SecurityHelper2010.CreateKey(arrPassword, arrSalt, arrBlock_Key, spinCount, keySize, this.m_info.KeyInfo.HashAlgorithm);
    return SecurityHelper2010.Decrypt(arrEncryptedKeyValue, key, arrSalt, arrSalt.Length, this.m_info.KeyInfo.BlockSize);
  }

  private byte[] GetVerifierHashInput(
    byte[] arrVerifierHashInput,
    byte[] arrPassword,
    byte[] arrSalt,
    int spinCount,
    int keySize,
    int blockSize)
  {
    byte[] arrBlock_Key = new byte[8]
    {
      (byte) 254,
      (byte) 167,
      (byte) 210,
      (byte) 118,
      (byte) 59,
      (byte) 75,
      (byte) 158,
      (byte) 121
    };
    byte[] key = SecurityHelper2010.CreateKey(arrPassword, arrSalt, arrBlock_Key, spinCount, keySize, this.m_info.KeyInfo.HashAlgorithm);
    return SecurityHelper2010.Decrypt(arrVerifierHashInput, key, arrSalt, arrSalt.Length, blockSize);
  }

  internal byte[] GetVerifierHashValue(
    byte[] arrVerifierHashValue,
    byte[] arrPassword,
    byte[] arrSalt,
    int spinCount,
    int keySize)
  {
    byte[] arrBlock_Key = new byte[8]
    {
      (byte) 215,
      (byte) 170,
      (byte) 15,
      (byte) 109,
      (byte) 48 /*0x30*/,
      (byte) 97,
      (byte) 52,
      (byte) 78
    };
    byte[] key = SecurityHelper2010.CreateKey(arrPassword, arrSalt, arrBlock_Key, spinCount, keySize, this.m_info.KeyInfo.HashAlgorithm);
    return SecurityHelper2010.Decrypt(arrVerifierHashValue, key, arrSalt, arrSalt.Length, this.m_info.KeyInfo.BlockSize);
  }

  internal byte[] GetHMacKey(
    byte[] arrEncryptedHmacKey,
    byte[] arrKey,
    byte[] arrSalt,
    int hashSize)
  {
    byte[] blockKey = new byte[8]
    {
      (byte) 95,
      (byte) 178,
      (byte) 173,
      (byte) 1,
      (byte) 12,
      (byte) 185,
      (byte) 225,
      (byte) 246
    };
    byte[] vector = SecurityHelper2010.GenerateVector(arrSalt, blockKey, this.m_info.KeyInfo.HashAlgorithm);
    return SecurityHelper2010.TryPadOrTruncate(SecurityHelper2010.Decrypt(arrEncryptedHmacKey, arrKey, vector, vector.Length, arrKey.Length), hashSize, (byte) 0);
  }

  internal byte[] GetHmacValue(byte[] arrEncryptedHmacValue, byte[] arrKey, byte[] arrSalt)
  {
    byte[] blockKey = new byte[8]
    {
      (byte) 160 /*0xA0*/,
      (byte) 103,
      (byte) 127 /*0x7F*/,
      (byte) 2,
      (byte) 178,
      (byte) 44,
      (byte) 132,
      (byte) 51
    };
    byte[] vector = SecurityHelper2010.GenerateVector(arrSalt, blockKey, this.m_info.KeyInfo.HashAlgorithm);
    return SecurityHelper2010.Decrypt(arrEncryptedHmacValue, arrKey, vector, vector.Length, arrKey.Length);
  }

  internal bool CheckDataIntegrity(
    Stream EncryptedPackage,
    byte[] arrKey,
    byte[] arrSalt,
    byte[] arrEncryptedHmacKey,
    byte[] arrEncryptedHmacValue,
    string hashAlgorithm)
  {
    byte[] hmacKey = this.GetHMacKey(arrEncryptedHmacKey, arrKey, arrSalt, this.m_info.KeyInfo.HashSize);
    if (hashAlgorithm == "SHA512")
    {
      HMACSHA512 hmacshA512 = new HMACSHA512();
      hmacshA512.Key = hmacKey;
      byte[] buffer = new byte[EncryptedPackage.Length];
      EncryptedPackage.Read(buffer, 0, buffer.Length);
      byte[] hash = hmacshA512.ComputeHash(buffer);
      return BiffRecordRaw.CompareArrays(SecurityHelper2010.TryPadOrTruncate(this.GetHmacValue(arrEncryptedHmacValue, arrKey, arrSalt), hash.Length, (byte) 0), hash);
    }
    HMACSHA1 hmacshA1 = new HMACSHA1();
    hmacshA1.Key = hmacKey;
    byte[] buffer1 = new byte[EncryptedPackage.Length];
    EncryptedPackage.Read(buffer1, 0, buffer1.Length);
    byte[] hash1 = hmacshA1.ComputeHash(buffer1);
    return BiffRecordRaw.CompareArrays(SecurityHelper2010.TryPadOrTruncate(this.GetHmacValue(arrEncryptedHmacValue, arrKey, arrSalt), hash1.Length, (byte) 0), hash1);
  }
}
