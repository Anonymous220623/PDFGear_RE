// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Security.AgileEncryptor
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.CompoundFile.Presentation;
using System;
using System.IO;
using System.Security.Cryptography;

#nullable disable
namespace Syncfusion.Presentation.Security;

[CLSCompliant(false)]
internal class AgileEncryptor
{
  private const int DefaultVersion = 262148 /*0x040004*/;
  private const int Reserved = 64 /*0x40*/;
  private const int SegmentSize = 4096 /*0x1000*/;
  private SecurityHelper m_securityHelper = new SecurityHelper();
  private HashAlgorithm m_hashAlgorithm = (HashAlgorithm) new SHA1CryptoServiceProvider();
  private HMAC m_hmacSha = (HMAC) new HMACSHA1();
  private string m_hashAlgorithmName = "SHA1";
  private int m_keyBits = 128 /*0x80*/;
  private int m_hashSize = 20;

  internal AgileEncryptor()
  {
  }

  internal AgileEncryptor(string hashAlgorithm, int keyBits, int hashSize)
  {
    this.m_hashAlgorithmName = hashAlgorithm;
    this.m_keyBits = keyBits;
    this.m_hashSize = hashSize;
    if (!(this.m_hashAlgorithmName == "SHA512"))
      return;
    this.m_hashAlgorithm = (HashAlgorithm) new SHA512CryptoServiceProvider();
    this.m_hmacSha = (HMAC) new HMACSHA512();
  }

  internal void Encrypt(Stream data, string password, ICompoundStorage root)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    if (password == null || password.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (password));
    this.PrepareEncryptionInfo(data, root, password);
    this.PrepareDataSpaces(root);
  }

  private void PrepareEncryptionInfo(Stream data, ICompoundStorage root, string password)
  {
    using (CompoundStream stream = root.CreateStream("EncryptionInfo"))
    {
      AgileEncryptionInfo agileEncryptionInfo = new AgileEncryptionInfo();
      agileEncryptionInfo.VersionInfo = 262148 /*0x040004*/;
      agileEncryptionInfo.Reserved = 64 /*0x40*/;
      KeyData keyData = agileEncryptionInfo.XmlEncryptionDescriptor.KeyData;
      this.InitializeKeyData(keyData);
      keyData.Salt = this.CreateSalt(keyData.SaltSize);
      EncryptedKey encryptedKey = agileEncryptionInfo.XmlEncryptionDescriptor.KeyEncryptors.EncryptedKey;
      this.InitializeEncryptedKey(encryptedKey);
      encryptedKey.Salt = this.CreateSalt(encryptedKey.SaltSize);
      byte[] salt1 = this.CreateSalt(encryptedKey.SaltSize);
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
      encryptedKey.EncryptedVerifierHashInput = this.Encrypt(salt1, encryptedKey.BlockSize, agileEncryptionKey1, encryptedKey.Salt);
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
      encryptedKey.EncryptedVerifierHashValue = this.Encrypt(this.m_hashAlgorithm.ComputeHash(salt1), encryptedKey.BlockSize, agileEncryptionKey2, encryptedKey.Salt);
      byte[] salt2 = this.CreateSalt(keyData.KeyBits / 8);
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
      encryptedKey.EncryptedKeyValue = this.Encrypt(salt2, encryptedKey.BlockSize, agileEncryptionKey3, encryptedKey.Salt);
      DataIntegrity dataIntegrity = agileEncryptionInfo.XmlEncryptionDescriptor.DataIntegrity;
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
      byte[] salt3 = this.CreateSalt(keyData.HashSize);
      dataIntegrity.EncryptedHmacKey = this.Encrypt(salt3, keyData.BlockSize, salt2, IV1);
      byte[] buffer = this.PrepareEncryptedPackage(data, root, keyData, salt2);
      this.m_hmacSha.Key = this.m_securityHelper.CorrectSize(salt3, keyData.HashSize, (byte) 0);
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
      dataIntegrity.EncryptedHmacValue = this.Encrypt(this.m_hmacSha.ComputeHash(buffer), keyData.BlockSize, salt2, IV2);
      agileEncryptionInfo.Serialize((Stream) stream);
    }
  }

  private byte[] PrepareEncryptedPackage(
    Stream data,
    ICompoundStorage root,
    KeyData keyData,
    byte[] intermediateKey)
  {
    byte[] numArray1 = BitConverter.GetBytes(data.Length);
    using (CompoundStream stream = root.CreateStream("EncryptedPackage"))
    {
      int num = (int) (data.Length / 4096L /*0x1000*/);
      if (data.Length % 4096L /*0x1000*/ != 0L)
        ++num;
      for (int index = 0; index < num; ++index)
      {
        int count = Math.Min(4096 /*0x1000*/, (int) (data.Length - (long) (index * 4096 /*0x1000*/)));
        byte[] numArray2 = new byte[count];
        byte[] numArray3 = new byte[count];
        data.Read(numArray2, 0, count);
        byte[] hash = this.m_hashAlgorithm.ComputeHash(this.m_securityHelper.CombineArray(keyData.Salt, BitConverter.GetBytes(index)));
        byte[] buffer2 = this.Encrypt(numArray2, keyData.BlockSize, intermediateKey, hash);
        numArray1 = this.m_securityHelper.CombineArray(numArray1, buffer2);
      }
      stream.Write(numArray1, 0, numArray1.Length);
    }
    return numArray1;
  }

  private void InitializeKeyData(KeyData keyData)
  {
    keyData.SaltSize = 16 /*0x10*/;
    keyData.BlockSize = 16 /*0x10*/;
    keyData.KeyBits = this.m_keyBits;
    keyData.HashSize = this.m_hashSize;
    keyData.CipherAlgorithm = "AES";
    keyData.CipherChaining = "ChainingModeCBC";
    keyData.HashAlgorithm = this.m_hashAlgorithmName;
  }

  private void InitializeEncryptedKey(EncryptedKey key)
  {
    key.SpinCount = 100000;
    key.SaltSize = 16 /*0x10*/;
    key.BlockSize = 16 /*0x10*/;
    key.KeyBits = this.m_keyBits;
    key.HashSize = this.m_hashSize;
    key.CipherAlgorithm = "AES";
    key.CipherChaining = "ChainingModeCBC";
    key.HashAlgorithm = this.m_hashAlgorithmName;
  }

  private void PrepareDataSpaces(ICompoundStorage root)
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

  private void SerializeVersion(ICompoundStorage dataSpaces)
  {
    if (dataSpaces == null)
      throw new ArgumentNullException(nameof (dataSpaces));
    using (CompoundStream stream = dataSpaces.CreateStream("Version"))
      new VersionInfo().Serialize((Stream) stream);
  }

  private void SerializeTransformInfo(ICompoundStorage dataSpaces)
  {
    using (ICompoundStorage storage1 = dataSpaces.CreateStorage("TransformInfo"))
    {
      using (ICompoundStorage storage2 = storage1.CreateStorage("StrongEncryptionTransform"))
      {
        using (CompoundStream stream = storage2.CreateStream("\u0006Primary"))
        {
          new TransformInfoHeader()
          {
            TransformType = 1,
            TransformId = "{FF9A3F03-56EF-4613-BDD5-5A41C1D07246}",
            TransformName = "Microsoft.Container.EncryptionTransform",
            ReaderVersion = 1,
            UpdaterVersion = 1,
            WriterVersion = 1
          }.Serialize((Stream) stream);
          new EncryptionTransformInfo()
          {
            Name = string.Empty
          }.Serialize((Stream) stream);
        }
      }
    }
  }

  private void SerializeDataSpaceInfo(ICompoundStorage dataSpaces)
  {
    using (ICompoundStorage storage = dataSpaces.CreateStorage("DataSpaceInfo"))
    {
      using (CompoundStream stream = storage.CreateStream("StrongEncryptionDataSpace"))
        new DataSpaceDefinition()
        {
          TransformRefs = {
            "StrongEncryptionTransform"
          }
        }.Serialize((Stream) stream);
    }
  }

  private void SerializeDataSpaceMap(ICompoundStorage dataSpaces)
  {
    if (dataSpaces == null)
      throw new ArgumentNullException(nameof (dataSpaces));
    DataSpaceMap dataSpaceMap = new DataSpaceMap();
    DataSpaceMapEntry dataSpaceMapEntry = new DataSpaceMapEntry();
    DataSpaceReferenceComponent referenceComponent = new DataSpaceReferenceComponent(0, "EncryptedPackage");
    dataSpaceMap.MapEntries.Add(dataSpaceMapEntry);
    dataSpaceMapEntry.Components.Add(referenceComponent);
    dataSpaceMapEntry.DataSpaceName = "StrongEncryptionDataSpace";
    using (CompoundStream stream = dataSpaces.CreateStream("DataSpaceMap"))
      dataSpaceMap.Serialize((Stream) stream);
  }

  private byte[] CreateSalt(int length)
  {
    byte[] salt = length > 0 ? new byte[length] : throw new ArgumentOutOfRangeException(nameof (length));
    Random random = new Random((int) DateTime.Now.Ticks);
    int maxValue = 256 /*0x0100*/;
    for (int index = 0; index < length; ++index)
      salt[index] = (byte) random.Next(maxValue);
    return salt;
  }

  private byte[] Encrypt(byte[] data, int blockSize, byte[] key, byte[] IV)
  {
    int size = data.Length;
    byte[] src;
    if (size % blockSize != 0)
    {
      size = (size / blockSize + 1) * blockSize;
      src = this.m_securityHelper.CorrectSize(data, size, (byte) 0);
    }
    else
      src = data;
    byte[] dst = new byte[size];
    byte[] numArray1 = new byte[blockSize];
    byte[] numArray2 = new byte[blockSize];
    Aes.KeySize keySize = Aes.KeySize.Bits128;
    if (key.Length == 32 /*0x20*/)
      keySize = Aes.KeySize.Bits256;
    Aes aes = new Aes(keySize, key);
    for (int index = 0; index < size; index += blockSize)
    {
      Buffer.BlockCopy((Array) src, index, (Array) numArray1, 0, blockSize);
      numArray1 = index != 0 ? this.m_securityHelper.ConcatenateIV(numArray1, numArray2) : this.m_securityHelper.ConcatenateIV(numArray1, IV);
      aes.Cipher(numArray1, numArray2);
      Buffer.BlockCopy((Array) numArray2, 0, (Array) dst, index, blockSize);
    }
    return dst;
  }
}
