// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Security.Excel2010Encryptor
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.CompoundFile.XlsIO;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Security;

internal class Excel2010Encryptor : Excel2007Encryptor
{
  internal const int DefaultSegmentSize = 4096 /*0x1000*/;
  internal const int Excel2010Version = 262148 /*0x040004*/;
  internal const int DefaultFlag = 64 /*0x40*/;
  private byte[] m_arrKey;

  public override void Encrypt(Stream data, string password, ICompoundStorage root)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    EncryptionInfo info = password != null && password.Length != 0 ? this.PrepareEncryptionInfo2010(root, password) : throw new ArgumentOutOfRangeException(nameof (password));
    this.PrepareDataSpaces(root);
    using (CompoundStream stream1 = root.CreateStream("EncryptedPackage"))
    {
      byte[] bytes = BitConverter.GetBytes(data.Length);
      stream1.Write(bytes, 0, 8);
      Stream stream2 = this.Encrypt(data, this.m_arrKey, info.DataEncryption.SaltValue, info.KeyInfo.HashAlgorithm);
      byte[] buffer = new byte[stream2.Length];
      stream2.Read(buffer, 0, buffer.Length);
      stream1.Write(buffer, 0, buffer.Length);
      stream2.Close();
      stream1.Position = 0L;
      this.PrepareDataIntegrity((Stream) stream1, this.m_arrKey, info.DataEncryption.SaltValue, info.KeyInfo.HashSize, info);
    }
    using (CompoundStream stream = root.CreateStream("EncryptionInfo"))
      info.Serialize((Stream) stream);
  }

  internal Stream Encrypt(
    Stream stream,
    byte[] arrIntermediateKey,
    byte[] arrKeyData_SaltValue,
    string hashAlgorithm)
  {
    byte[] numArray1 = stream != null ? new byte[stream.Length] : throw new ArgumentNullException("Stream");
    stream.Read(numArray1, 0, numArray1.Length);
    int length1 = numArray1.Length;
    int num1 = length1 % 16 /*0x10*/;
    if (num1 > 0)
    {
      int length2 = length1 + (16 /*0x10*/ - num1);
      numArray1 = SecurityHelper2010.TryPadOrTruncate(numArray1, length2, (byte) 0);
    }
    int num2 = numArray1.Length / 4096 /*0x1000*/;
    uint segmentNumber = 0;
    byte[] numArray2;
    if (num2 == 0)
    {
      byte[] vector = SecurityHelper2010.GenerateVector(arrKeyData_SaltValue, segmentNumber, hashAlgorithm);
      numArray2 = SecurityHelper2010.Encrypt(numArray1, arrIntermediateKey, vector, 16 /*0x10*/);
    }
    else
    {
      numArray2 = new byte[numArray1.Length];
      byte[] numArray3 = new byte[4096 /*0x1000*/];
      int num3 = 0;
      for (; (long) segmentNumber < (long) num2; ++segmentNumber)
      {
        Buffer.BlockCopy((Array) numArray1, num3, (Array) numArray3, 0, numArray3.Length);
        byte[] vector = SecurityHelper2010.GenerateVector(arrKeyData_SaltValue, segmentNumber, hashAlgorithm);
        byte[] src = SecurityHelper2010.Encrypt(numArray3, arrIntermediateKey, vector, 16 /*0x10*/);
        Buffer.BlockCopy((Array) src, 0, (Array) numArray2, num3, src.Length);
        num3 += 4096 /*0x1000*/;
      }
      int length3 = numArray1.Length % 4096 /*0x1000*/;
      if (length3 > 0)
      {
        byte[] numArray4 = new byte[length3];
        Buffer.BlockCopy((Array) numArray1, num3, (Array) numArray4, 0, numArray4.Length);
        byte[] vector = SecurityHelper2010.GenerateVector(arrKeyData_SaltValue, segmentNumber, hashAlgorithm);
        byte[] src = SecurityHelper2010.Encrypt(numArray4, arrIntermediateKey, vector, 16 /*0x10*/);
        Buffer.BlockCopy((Array) src, 0, (Array) numArray2, num3, src.Length);
      }
    }
    return (Stream) new MemoryStream(numArray2);
  }

  protected void PrepareDataSpaces(ICompoundStorage root)
  {
    if (root == null)
      throw new ArgumentNullException(nameof (root));
    using (ICompoundStorage storage = root.CreateStorage("\u0006DataSpaces"))
    {
      this.SerializeDataSpaceInfo(storage);
      this.SerializeTransformInfo(storage);
      this.SerializeVersion(storage);
      this.SerializeDataSpaceMap(storage);
    }
  }

  internal byte[] CreateIntermediateKey(
    byte[] arrPassword,
    byte[] arrSalt,
    int spinCount,
    string hashAlgorithm)
  {
    byte[] salt = this.CreateSalt(16 /*0x10*/);
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
    byte[] key = this.CreateKey(arrPassword, arrSalt, arrBlock_Key, spinCount, arrSalt.Length, hashAlgorithm);
    this.m_arrKey = salt;
    return SecurityHelper2010.Encrypt(salt, key, arrSalt, key.Length);
  }

  internal byte[] CreateVerifierHashInput(
    byte[] random,
    byte[] arrPassword,
    byte[] arrSalt,
    int spinCount,
    string hashAlgorithm)
  {
    byte[] arrPlainData = random;
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
    byte[] key = this.CreateKey(arrPassword, arrSalt, arrBlock_Key, spinCount, 16 /*0x10*/, hashAlgorithm);
    return SecurityHelper2010.Encrypt(arrPlainData, key, arrSalt, key.Length);
  }

  internal byte[] CreateVerifierHashValue(
    byte[] random,
    byte[] arrPassword,
    byte[] arrSalt,
    int spinCount,
    string hashAlgorithm)
  {
    byte[] arrPlainData = SecurityHelper2010.TryPadOrTruncate(SecurityHelper2010.Hash(random, hashAlgorithm), 32 /*0x20*/, (byte) 0);
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
    byte[] key = this.CreateKey(arrPassword, arrSalt, arrBlock_Key, spinCount, arrSalt.Length, hashAlgorithm);
    return SecurityHelper2010.Encrypt(arrPlainData, key, arrSalt, key.Length);
  }

  internal byte[] CreateKey(
    byte[] arrPassword,
    byte[] arrSalt,
    byte[] arrBlock_Key,
    int spinCount,
    int keySize,
    string algorithm)
  {
    if (algorithm == "SHA512")
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

  protected EncryptionInfo PrepareEncryptionInfo2010(ICompoundStorage root, string password)
  {
    if (root == null)
      throw new ArgumentNullException(nameof (root));
    EncryptionInfo encryptionInfo = new EncryptionInfo();
    encryptionInfo.VersionInfo = 262148 /*0x040004*/;
    encryptionInfo.Flags = 64 /*0x40*/;
    int spinCount = encryptionInfo.KeyInfo.SpinCount;
    byte[] bytes = Encoding.Unicode.GetBytes(password);
    byte[] salt1 = this.CreateSalt(16 /*0x10*/);
    encryptionInfo.KeyInfo.KeyValue = this.CreateIntermediateKey(bytes, salt1, spinCount, encryptionInfo.KeyInfo.HashAlgorithm);
    byte[] salt2 = this.CreateSalt(16 /*0x10*/);
    encryptionInfo.KeyInfo.VerifierHashInput = this.CreateVerifierHashInput(salt2, bytes, salt1, spinCount, encryptionInfo.KeyInfo.HashAlgorithm);
    encryptionInfo.KeyInfo.VerifierHashValue = this.CreateVerifierHashValue(salt2, bytes, salt1, spinCount, encryptionInfo.KeyInfo.HashAlgorithm);
    encryptionInfo.KeyInfo.SaltValue = salt1;
    byte[] numArray = encryptionInfo.DataEncryption.SaltValue = this.CreateSalt(16 /*0x10*/);
    return encryptionInfo;
  }

  internal void PrepareDataIntegrity(
    Stream encryptedPackage,
    byte[] arrKey,
    byte[] arrSalt,
    int hashSize,
    EncryptionInfo info)
  {
    if (encryptedPackage == null)
      throw new ArgumentNullException("encrypted Package");
    byte[] salt = this.CreateSalt(hashSize);
    byte[] blockKey1 = new byte[8]
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
    byte[] vector1 = SecurityHelper2010.GenerateVector(arrSalt, blockKey1, info.KeyInfo.HashAlgorithm);
    byte[] arrPlainData1 = SecurityHelper2010.TryPadOrTruncate(salt, 2 * arrKey.Length, (byte) 0);
    byte[] numArray = SecurityHelper2010.Encrypt(arrPlainData1, arrKey, vector1, arrKey.Length);
    info.DataIntegrity.HMacKey = numArray;
    HMACSHA1 hmacshA1 = new HMACSHA1();
    hmacshA1.Key = arrPlainData1;
    byte[] buffer = new byte[encryptedPackage.Length];
    encryptedPackage.Read(buffer, 0, buffer.Length);
    byte[] arrPlainData2 = SecurityHelper2010.TryPadOrTruncate(hmacshA1.ComputeHash(buffer), 2 * arrKey.Length, (byte) 0);
    byte[] blockKey2 = new byte[8]
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
    byte[] vector2 = SecurityHelper2010.GenerateVector(arrSalt, blockKey2, info.KeyInfo.HashAlgorithm);
    info.DataIntegrity.HMacValue = SecurityHelper2010.Encrypt(arrPlainData2, arrKey, vector2, arrKey.Length);
  }
}
