// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Security.Excel2007Decryptor
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.CompoundFile.XlsIO;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Security;

public class Excel2007Decryptor
{
  private int BlockSize = 16 /*0x10*/;
  private DataSpaceMap m_dataSpaceMap;
  protected EncryptionInfo m_info;
  private ICompoundStorage m_storage;
  protected byte[] m_arrKey;

  protected ICompoundStorage Storage => this.m_storage;

  public virtual Stream Decrypt()
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
      byte[] buffer2 = Excel2007Decryptor.Decrypt(numArray, this.m_arrKey);
      memoryStream.Write(buffer2, 0, int32);
      memoryStream.Position = 0L;
    }
    return (Stream) memoryStream;
  }

  public void Initialize(ICompoundStorage storage)
  {
    this.m_storage = storage != null ? storage : throw new ArgumentNullException(nameof (storage));
    using (Stream stream = (Stream) storage.OpenStream("EncryptionInfo"))
    {
      stream.Position = 0L;
      this.m_info = new EncryptionInfo(stream);
    }
    using (ICompoundStorage dataSpaces = storage.OpenStorage("\u0006DataSpaces"))
    {
      this.ParseDataSpaceMap(dataSpaces);
      this.ParseTransfrom(dataSpaces);
    }
  }

  public static bool CheckEncrypted(ICompoundStorage storage)
  {
    return storage.ContainsStream("EncryptionInfo") && storage.ContainsStorage("\u0006DataSpaces");
  }

  public virtual bool CheckPassword(string password)
  {
    EncryptionVerifier verifier = this.m_info.Verifier;
    this.m_arrKey = this.VerifyPassword(password, verifier);
    return this.m_arrKey != null;
  }

  private byte[] VerifyPassword(string password, EncryptionVerifier verifier)
  {
    byte[] salt = verifier.Salt;
    byte[] key = SecurityHelper.CreateKey(password, salt, 16 /*0x10*/);
    byte[] buffer = Excel2007Decryptor.Decrypt(verifier.EncryptedVerifier, key);
    byte[] array2 = Excel2007Decryptor.Decrypt(verifier.EncryptedVerifierHash, key);
    byte[] hash = new SHA1CryptoServiceProvider().ComputeHash(buffer);
    if (!BiffRecordRaw.CompareArrays(hash, 0, array2, 0, hash.Length))
      key = (byte[]) null;
    return key;
  }

  private static byte[] Decrypt(byte[] data, byte[] key)
  {
    Aes aes = new Aes(Aes.KeySize.Bits128, key);
    return SecurityHelper.EncryptDecrypt(data, new SecurityHelper.EncryptionMethod(aes.InvCipher), key.Length);
  }

  private void ParseTransfrom(ICompoundStorage dataSpaces)
  {
    List<DataSpaceMapEntry> mapEntries = this.m_dataSpaceMap.MapEntries;
    string streamName = mapEntries.Count == 1 ? mapEntries[0].DataSpaceName : throw new InvalidDataException();
    string storageName = (string) null;
    using (ICompoundStorage compoundStorage = dataSpaces.OpenStorage("DataSpaceInfo"))
    {
      using (Stream stream = (Stream) compoundStorage.OpenStream(streamName))
      {
        List<string> transformRefs = new DataSpaceDefinition(stream).TransformRefs;
        storageName = transformRefs.Count == 1 ? transformRefs[0] : throw new InvalidDataException();
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
