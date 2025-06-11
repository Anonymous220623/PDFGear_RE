// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Security.StandardDecryptor
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.CompoundFile.Presentation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

#nullable disable
namespace Syncfusion.Presentation.Security;

[CLSCompliant(false)]
internal class StandardDecryptor
{
  private int BlockSize = 16 /*0x10*/;
  private DataSpaceMap m_dataSpaceMap;
  private StandardEncryptionInfo m_info;
  private ICompoundStorage m_storage;
  private byte[] m_arrKey;
  private SecurityHelper m_securityHelper = new SecurityHelper();

  internal Stream Decrypt()
  {
    if (this.m_arrKey == null)
      throw new InvalidOperationException("Incorrect password.");
    MemoryStream memoryStream = new MemoryStream();
    using (CompoundStream compoundStream = this.m_storage.OpenStream("EncryptedPackage"))
    {
      byte[] buffer1 = new byte[8];
      compoundStream.Read(buffer1, 0, 8);
      int int32 = BitConverter.ToInt32(buffer1, 0);
      int num = int32 % this.BlockSize;
      int count = num > 0 ? int32 + this.BlockSize - num : int32;
      byte[] numArray = new byte[count];
      compoundStream.Read(numArray, 0, count);
      byte[] buffer2 = this.Decrypt(numArray, this.m_arrKey);
      memoryStream.Write(buffer2, 0, int32);
      memoryStream.Position = 0L;
    }
    return (Stream) memoryStream;
  }

  internal void Initialize(ICompoundStorage storage)
  {
    this.m_storage = storage != null ? storage : throw new ArgumentNullException(nameof (storage));
    using (Stream stream = (Stream) storage.OpenStream("EncryptionInfo"))
      this.m_info = new StandardEncryptionInfo(stream);
    using (ICompoundStorage dataSpaces = storage.OpenStorage("\u0006DataSpaces"))
    {
      this.ParseDataSpaceMap(dataSpaces);
      this.ParseTransfrom(dataSpaces);
    }
  }

  internal bool CheckPassword(string password)
  {
    EncryptionVerifier verifier = this.m_info.Verifier;
    this.m_arrKey = this.m_securityHelper.CreateKey(password, verifier.Salt, 16 /*0x10*/);
    byte[] hash = new SHA1Managed().ComputeHash(this.Decrypt(verifier.EncryptedVerifier, this.m_arrKey));
    byte[] src = this.Decrypt(verifier.EncryptedVerifierHash, this.m_arrKey);
    byte[] numArray = new byte[verifier.VerifierHashSize];
    Buffer.BlockCopy((Array) src, 0, (Array) numArray, 0, numArray.Length);
    return this.m_securityHelper.CompareArray(numArray, hash);
  }

  private byte[] Decrypt(byte[] data, byte[] key)
  {
    Aes aes = new Aes(Aes.KeySize.Bits128, key);
    return this.m_securityHelper.EncryptDecrypt(data, new SecurityHelper.EncryptionMethod(aes.InvCipher), key.Length);
  }

  private void ParseTransfrom(ICompoundStorage dataSpaces)
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
