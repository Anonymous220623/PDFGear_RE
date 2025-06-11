// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Security.AgileDecryptor
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.CompoundFile.DocIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Security;

[CLSCompliant(false)]
internal class AgileDecryptor
{
  private const int SegmentSize = 4096 /*0x1000*/;
  private DataSpaceMap m_dataSpaceMap;
  private AgileEncryptionInfo m_info;
  private ICompoundStorage m_storage;
  private byte[] m_intermediateKey;
  private SecurityHelper m_securityHelper = new SecurityHelper();
  private HashAlgorithm m_hashAlgorithm = (HashAlgorithm) new SHA1CryptoServiceProvider();
  private HMAC m_hmacSha = (HMAC) new HMACSHA1();

  internal Stream Decrypt()
  {
    if (this.m_intermediateKey == null)
      throw new InvalidOperationException("Incorrect password.");
    MemoryStream memoryStream = new MemoryStream();
    KeyData keyData = this.m_info.XmlEncryptionDescriptor.KeyData;
    using (CompoundStream compoundStream = this.m_storage.OpenStream("EncryptedPackage"))
    {
      byte[] numArray1 = new byte[8];
      compoundStream.Read(numArray1, 0, 8);
      int int32 = BitConverter.ToInt32(numArray1, 0);
      int num1 = int32 % keyData.BlockSize;
      int count1 = num1 > 0 ? int32 + keyData.BlockSize - num1 : int32;
      byte[] numArray2 = new byte[count1];
      compoundStream.Read(numArray2, 0, count1);
      if (!this.CheckEncryptedPackage(this.m_securityHelper.CombineArray(numArray1, numArray2)))
        throw new Exception("Encrypted package is invalid");
      byte[] numArray3 = new byte[int32];
      int num2 = count1 % 4096 /*0x1000*/ == 0 ? count1 / 4096 /*0x1000*/ : count1 / 4096 /*0x1000*/ + 1;
      for (int index = 0; index < num2; ++index)
      {
        int length = Math.Min(4096 /*0x1000*/, count1 - index * 4096 /*0x1000*/);
        byte[] numArray4 = new byte[length];
        byte[] numArray5 = new byte[length];
        Buffer.BlockCopy((Array) numArray2, index * 4096 /*0x1000*/, (Array) numArray4, 0, length);
        byte[] hash = this.m_hashAlgorithm.ComputeHash(this.m_securityHelper.CombineArray(keyData.Salt, BitConverter.GetBytes(index)));
        byte[] src = this.Decrypt(numArray4, keyData.BlockSize, this.m_intermediateKey, hash, length);
        int count2 = index == num2 - 1 ? length - (count1 - int32) : length;
        Buffer.BlockCopy((Array) src, 0, (Array) numArray3, index * 4096 /*0x1000*/, count2);
      }
      memoryStream.Write(numArray3, 0, int32);
      memoryStream.Position = 0L;
    }
    return (Stream) memoryStream;
  }

  internal void Initialize(ICompoundStorage storage)
  {
    this.m_storage = storage != null ? storage : throw new ArgumentNullException(nameof (storage));
    using (Stream stream = (Stream) storage.OpenStream("EncryptionInfo"))
    {
      this.m_info = new AgileEncryptionInfo(stream);
      if (this.m_info.XmlEncryptionDescriptor.KeyEncryptors.EncryptedKey.HashAlgorithm == "SHA512")
      {
        this.m_hashAlgorithm = (HashAlgorithm) new SHA512CryptoServiceProvider();
        this.m_hmacSha = (HMAC) new HMACSHA512();
      }
    }
    using (ICompoundStorage dataSpaces = storage.OpenStorage("\u0006DataSpaces"))
    {
      this.ParseDataSpaceMap(dataSpaces);
      this.ParseTransform(dataSpaces);
    }
  }

  internal bool CheckPassword(string password)
  {
    KeyData keyData = this.m_info.XmlEncryptionDescriptor.KeyData;
    EncryptedKey encryptedKey = this.m_info.XmlEncryptionDescriptor.KeyEncryptors.EncryptedKey;
    byte[] blockKey1 = new byte[8]
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
    byte[] agileEncryptionKey1 = this.m_securityHelper.CreateAgileEncryptionKey(this.m_hashAlgorithm, password, encryptedKey.Salt, blockKey1, encryptedKey.KeyBits >> 3, encryptedKey.SpinCount);
    byte[] buffer = this.Decrypt(encryptedKey.EncryptedVerifierHashInput, encryptedKey.BlockSize, agileEncryptionKey1, encryptedKey.Salt, encryptedKey.SaltSize);
    byte[] blockKey2 = new byte[8]
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
    byte[] agileEncryptionKey2 = this.m_securityHelper.CreateAgileEncryptionKey(this.m_hashAlgorithm, password, encryptedKey.Salt, blockKey2, encryptedKey.KeyBits >> 3, encryptedKey.SpinCount);
    bool flag = this.m_securityHelper.CompareArray(this.Decrypt(encryptedKey.EncryptedVerifierHashValue, encryptedKey.BlockSize, agileEncryptionKey2, encryptedKey.Salt, encryptedKey.HashSize), this.m_hashAlgorithm.ComputeHash(buffer));
    byte[] blockKey3 = new byte[8]
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
    byte[] agileEncryptionKey3 = this.m_securityHelper.CreateAgileEncryptionKey(this.m_hashAlgorithm, password, encryptedKey.Salt, blockKey3, encryptedKey.KeyBits >> 3, encryptedKey.SpinCount);
    this.m_intermediateKey = this.Decrypt(encryptedKey.EncryptedKeyValue, encryptedKey.BlockSize, agileEncryptionKey3, encryptedKey.Salt, keyData.KeyBits / 8);
    return flag;
  }

  private bool CheckEncryptedPackage(byte[] encryptedPackage)
  {
    KeyData keyData = this.m_info.XmlEncryptionDescriptor.KeyData;
    DataIntegrity dataIntegrity = this.m_info.XmlEncryptionDescriptor.DataIntegrity;
    byte[] buffer2_1 = new byte[8]
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
    byte[] IV1 = this.m_securityHelper.CorrectSize(this.m_hashAlgorithm.ComputeHash(this.m_securityHelper.CombineArray(keyData.Salt, buffer2_1)), keyData.BlockSize, (byte) 0);
    this.m_hmacSha.Key = this.m_securityHelper.CorrectSize(this.Decrypt(dataIntegrity.EncryptedHmacKey, keyData.BlockSize, this.m_intermediateKey, IV1, keyData.HashSize), keyData.HashSize, (byte) 0);
    byte[] hash = this.m_hmacSha.ComputeHash(encryptedPackage);
    byte[] buffer2_2 = new byte[8]
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
    byte[] IV2 = this.m_securityHelper.CorrectSize(this.m_hashAlgorithm.ComputeHash(this.m_securityHelper.CombineArray(keyData.Salt, buffer2_2)), keyData.BlockSize, (byte) 0);
    byte[] buffer2_3 = this.Decrypt(dataIntegrity.EncryptedHmacValue, keyData.BlockSize, this.m_intermediateKey, IV2, keyData.HashSize);
    return this.m_securityHelper.CompareArray(hash, buffer2_3);
  }

  private byte[] Decrypt(byte[] data, int blockSize, byte[] arrKey, byte[] IV, int actualLength)
  {
    int size = data.Length;
    byte[] numArray1 = new byte[size];
    byte[] numArray2 = new byte[blockSize];
    byte[] numArray3 = new byte[blockSize];
    byte[] numArray4 = new byte[blockSize];
    Aes.KeySize keySize = Aes.KeySize.Bits128;
    if (arrKey.Length == 32 /*0x20*/)
      keySize = Aes.KeySize.Bits256;
    Aes aes = new Aes(keySize, arrKey);
    int num = 0;
    byte[] src;
    if (size % blockSize != 0)
    {
      size = (size / blockSize + 1) * blockSize;
      src = this.m_securityHelper.CorrectSize(data, size, (byte) 0);
    }
    else
      src = data;
    for (; num < size; num += blockSize)
    {
      if (num == 0)
        Buffer.BlockCopy((Array) IV, 0, (Array) numArray4, 0, blockSize);
      else
        Buffer.BlockCopy((Array) numArray2, 0, (Array) numArray4, 0, blockSize);
      Buffer.BlockCopy((Array) src, num, (Array) numArray2, 0, blockSize);
      aes.InvCipher(numArray2, numArray3);
      numArray3 = this.m_securityHelper.ConcatenateIV(numArray3, numArray4);
      Buffer.BlockCopy((Array) numArray3, 0, (Array) numArray1, num, blockSize);
    }
    if (numArray1.Length > actualLength)
    {
      byte[] dst = new byte[actualLength];
      Buffer.BlockCopy((Array) numArray1, 0, (Array) dst, 0, actualLength);
      numArray1 = dst;
    }
    return numArray1;
  }

  private void ParseTransform(ICompoundStorage dataSpaces)
  {
    List<DataSpaceMapEntry> mapEntries = this.m_dataSpaceMap.MapEntries;
    string streamName = mapEntries.Count == 1 ? mapEntries[0].DataSpaceName : throw new Exception("Invalid data");
    string storageName = (string) null;
    using (ICompoundStorage compoundStorage = dataSpaces.OpenStorage("DataSpaceInfo"))
    {
      using (Stream stream = (Stream) compoundStorage.OpenStream(streamName))
      {
        List<string> transformRefs = new DataSpaceDefinition(stream).TransformRefs;
        storageName = transformRefs.Count == 1 ? transformRefs[0] : throw new Exception("Invalid data");
      }
    }
    using (ICompoundStorage compoundStorage = dataSpaces.OpenStorage("TransformInfo"))
    {
      using (ICompoundStorage transformStorage = compoundStorage.OpenStorage(storageName))
        this.ParseTransformInfo(transformStorage);
    }
  }

  private void ParseDataSpaceMap(ICompoundStorage dataSpaces)
  {
    if (dataSpaces == null)
      throw new ArgumentNullException(nameof (dataSpaces));
    using (CompoundStream compoundStream = dataSpaces.OpenStream("DataSpaceMap"))
      this.m_dataSpaceMap = new DataSpaceMap((Stream) compoundStream);
  }

  private void ParseTransformInfo(ICompoundStorage transformStorage)
  {
    using (Stream stream = (Stream) transformStorage.OpenStream("\u0006Primary"))
    {
      TransformInfoHeader transformInfoHeader = new TransformInfoHeader(stream);
      EncryptionTransformInfo encryptionTransformInfo = new EncryptionTransformInfo(stream);
    }
  }
}
