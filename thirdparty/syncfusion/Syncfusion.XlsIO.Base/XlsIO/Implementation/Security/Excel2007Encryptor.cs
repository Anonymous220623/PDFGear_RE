// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Security.Excel2007Encryptor
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.CompoundFile.XlsIO;
using System;
using System.IO;
using System.Security.Cryptography;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Security;

internal class Excel2007Encryptor
{
  internal const int KeyLength = 16 /*0x10*/;
  private const int DefaultVersion = 131075 /*0x020003*/;
  private const int DefaultFlags = 36;
  private const int AES128AlgorithmId = 26126;
  private const int SHA1AlgorithmHash = 32772;
  private const int DefaultProviderType = 24;
  private const string DefaultCSPName = "Microsoft Enhanced RSA and AES Cryptographic Provider (Prototype)";

  public virtual void Encrypt(Stream data, string password, ICompoundStorage root)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    byte[] key = password != null && password.Length != 0 ? this.PrepareEncryptionInfo(root, password) : throw new ArgumentOutOfRangeException(nameof (password));
    this.PrepareDataSpaces(root);
    using (CompoundStream stream = root.CreateStream("EncryptedPackage"))
    {
      byte[] bytes = BitConverter.GetBytes(data.Length);
      stream.Write(bytes, 0, 8);
      this.Encrypt(data, key, (Stream) stream);
    }
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

  protected void SerializeVersion(ICompoundStorage dataSpaces)
  {
    if (dataSpaces == null)
      throw new ArgumentNullException(nameof (dataSpaces));
    using (CompoundStream stream = dataSpaces.CreateStream("Version"))
      new VersionInfo().Serialize((Stream) stream);
  }

  protected void SerializeTransformInfo(ICompoundStorage dataSpaces)
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
          new EncryptionTransformInfo() { Name = "AES128" }.Serialize((Stream) stream);
        }
      }
    }
  }

  protected void SerializeDataSpaceInfo(ICompoundStorage dataSpaces)
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

  protected void SerializeDataSpaceMap(ICompoundStorage dataSpaces)
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

  protected virtual byte[] PrepareEncryptionInfo(ICompoundStorage root, string password)
  {
    byte[] salt1 = this.CreateSalt(16 /*0x10*/);
    byte[] key = SecurityHelper.CreateKey(password, salt1, 16 /*0x10*/);
    byte[] salt2 = this.CreateSalt(16 /*0x10*/);
    SHA1 shA1 = (SHA1) new SHA1CryptoServiceProvider();
    using (CompoundStream stream = root.CreateStream("EncryptionInfo"))
    {
      EncryptionInfo encryptionInfo = new EncryptionInfo();
      encryptionInfo.VersionInfo = 131075 /*0x020003*/;
      encryptionInfo.Flags = 36;
      EncryptionHeader header = encryptionInfo.Header;
      header.Flags = 36;
      header.AlgorithmId = 26126;
      header.AlgorithmIdHash = 32772;
      header.KeySize = 128 /*0x80*/;
      header.ProviderType = 24;
      header.Reserved1 = 0;
      header.Reserved2 = 0;
      header.CSPName = "Microsoft Enhanced RSA and AES Cryptographic Provider (Prototype)";
      EncryptionVerifier verifier = encryptionInfo.Verifier;
      verifier.Salt = salt1;
      verifier.EncryptedVerifier = this.Encrypt(salt2, key);
      byte[] numArray = shA1.ComputeHash(salt2);
      int num = numArray.Length % 16 /*0x10*/;
      verifier.VerifierHashSize = numArray.Length;
      if (num != 0)
        numArray = SecurityHelper.CombineArray(numArray, new byte[16 /*0x10*/ - num]);
      verifier.EncryptedVerifierHash = this.Encrypt(numArray, key);
      encryptionInfo.Serialize((Stream) stream);
    }
    return key;
  }

  protected byte[] CreateSalt(int length)
  {
    byte[] salt = length > 0 ? new byte[length] : throw new ArgumentOutOfRangeException(nameof (length));
    Random random = new Random((int) DateTime.Now.Ticks);
    int maxValue = 256 /*0x0100*/;
    for (int index = 0; index < length; ++index)
      salt[index] = (byte) random.Next(maxValue);
    return salt;
  }

  private byte[] Encrypt(byte[] data, byte[] key)
  {
    Aes aes = new Aes(Aes.KeySize.Bits128, key);
    return SecurityHelper.EncryptDecrypt(data, new SecurityHelper.EncryptionMethod(aes.Cipher), key.Length);
  }

  private void Encrypt(Stream stream, byte[] key, Stream output)
  {
    Aes aes = new Aes(Aes.KeySize.Bits128, key);
    byte[] numArray1 = new byte[16 /*0x10*/];
    byte[] numArray2 = new byte[16 /*0x10*/];
    while (stream.Read(numArray1, 0, 16 /*0x10*/) > 0)
    {
      aes.Cipher(numArray1, numArray2);
      output.Write(numArray2, 0, 16 /*0x10*/);
    }
  }
}
