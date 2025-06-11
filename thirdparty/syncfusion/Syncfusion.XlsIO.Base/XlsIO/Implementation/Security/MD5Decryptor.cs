// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Security.MD5Decryptor
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Security;

[CLSCompliant(false)]
public class MD5Decryptor : IDecryptor, IEncryptor
{
  private const int DEF_READ_LENGTH = 16 /*0x10*/;
  private const int DEF_PAS_LEN = 64 /*0x40*/;
  private const int DEF_BLOCK_SIZE = 1024 /*0x0400*/;
  private const int DEF_START_POS = 0;
  private const int DEF_INC_BYTE_MAXVAL = 256 /*0x0100*/;
  private byte[] m_baDocumentID;
  private byte[] m_baPoint;
  private byte[] m_baHash;
  private byte[] m_baPassword = new byte[64 /*0x40*/];
  private MD5Context m_valContext;
  private long m_lLastStreamPosition;
  private ByteArrayDataProvider m_provider;

  public bool CheckPassword(string password)
  {
    this.PreparePassword(password);
    return this.VerifyPassword();
  }

  public MemoryStream Decrypt(Stream stream)
  {
    if (stream == null)
      return (MemoryStream) null;
    MemoryStream memoryStream = new MemoryStream();
    byte[] numArray = new byte[16 /*0x10*/];
    ByteArrayDataProvider provider = new ByteArrayDataProvider(numArray);
    long length = stream.Length;
    if (length == 0L)
      return memoryStream;
    long position = stream.Position;
    uint block = 0;
    WordKey key = new WordKey();
    this.MakeKey(key, block, this.m_valContext);
    while (position < length)
    {
      for (int index = stream.Read(numArray, 0, 16 /*0x10*/); index < 16 /*0x10*/; ++index)
        numArray[index] = (byte) 1;
      this.DecryptBuffer((DataProvider) provider, 0, 16 /*0x10*/, key);
      memoryStream.Write(numArray, 0, 16 /*0x10*/);
      position += 16L /*0x10*/;
      if (position % 1024L /*0x0400*/ == 0L)
      {
        ++block;
        this.MakeKey(key, block, this.m_valContext);
      }
    }
    memoryStream.Position = 0L;
    return memoryStream;
  }

  public void Decrypt(DataProvider provider, int offset, int length, long streamPos)
  {
    if (provider == null)
      return;
    if (offset < 0)
      throw new ArgumentOutOfRangeException(nameof (offset));
    if (length < 0)
      throw new ArgumentOutOfRangeException(nameof (length));
    this.CheckPrepared();
    long num = streamPos / 1024L /*0x0400*/ * 1024L /*0x0400*/;
    int val1 = (int) (1024L /*0x0400*/ - streamPos + num);
    int startOffset = offset;
    for (int length1 = Math.Min(val1, length); length1 > 0; length1 = Math.Min(1024 /*0x0400*/, length))
    {
      WordKey key = this.PrepareKey(streamPos);
      this.DecryptBuffer(provider, startOffset, length1, key);
      startOffset += length1;
      length -= length1;
      streamPos += (long) length1;
    }
  }

  public void Decrypt(byte[] buffer, int offset, int length) => throw new NotImplementedException();

  private void PreparePassword(string password)
  {
    int index = 0;
    for (int length = password.Length; index < 16 /*0x10*/ && index < length; ++index)
    {
      ushort num = (ushort) password[index];
      this.m_baPassword[2 * index] = (byte) ((uint) num & (uint) byte.MaxValue);
      this.m_baPassword[2 * index + 1] = (byte) ((int) num >> 8 & (int) byte.MaxValue);
    }
    this.m_baPassword[2 * index] = (byte) 128 /*0x80*/;
    this.m_baPassword[56] = (byte) (index << 4);
  }

  private void Swap(ref byte a, ref byte b)
  {
    byte num = a;
    a = b;
    b = num;
  }

  private void PrepareKey(WordKey key, byte[] data, byte length)
  {
    byte index1 = 0;
    byte index2 = 0;
    byte[] status = key.Status;
    for (int index3 = 0; index3 < 256 /*0x0100*/; ++index3)
      status[index3] = (byte) index3;
    for (int index4 = 0; index4 < 256 /*0x0100*/; ++index4)
    {
      index2 = (byte) (((int) data[(int) index1] + (int) status[index4] + (int) index2) % 256 /*0x0100*/);
      this.Swap(ref status[index4], ref status[(int) index2]);
      index1 = (byte) (((uint) index1 + 1U) % (uint) length);
    }
  }

  private void MakeKey(WordKey key, uint block, MD5Context valContext)
  {
    MD5Context md5Context = new MD5Context();
    byte[] numArray = new byte[64 /*0x40*/];
    Buffer.BlockCopy((Array) valContext.Digest, 0, (Array) numArray, 0, 5);
    numArray[5] = (byte) (block & (uint) byte.MaxValue);
    numArray[6] = (byte) (block >> 8 & (uint) byte.MaxValue);
    numArray[7] = (byte) (block >> 16 /*0x10*/ & (uint) byte.MaxValue);
    numArray[8] = (byte) (block >> 24 & (uint) byte.MaxValue);
    numArray[9] = (byte) 128 /*0x80*/;
    numArray[56] = (byte) 72;
    md5Context.Update(numArray, 64U /*0x40*/);
    md5Context.StoreDigest();
    this.PrepareKey(key, md5Context.Digest, (byte) 16 /*0x10*/);
  }

  private bool CompareMemory(byte[] block1, byte[] block2, int length)
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
    WordKey key = new WordKey();
    this.MakeKey(key, 0U, this.m_valContext);
    ByteArrayDataProvider provider = new ByteArrayDataProvider(this.m_baPoint);
    this.DecryptBuffer((DataProvider) provider, 0, 16 /*0x10*/, key);
    provider.SetBuffer(this.m_baHash);
    this.DecryptBuffer((DataProvider) provider, 0, 16 /*0x10*/, key);
    this.m_baPoint[16 /*0x10*/] = (byte) 128 /*0x80*/;
    MD5Decryptor.SetByte(this.m_baPoint, 17, 47, (byte) 0);
    this.m_baPoint[56] = (byte) 128 /*0x80*/;
    MD5Context md5Context = new MD5Context();
    md5Context.Update(this.m_baPoint, 64U /*0x40*/);
    md5Context.StoreDigest();
    return this.CompareMemory(md5Context.Digest, this.m_baHash, 16 /*0x10*/);
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

  private static void SetByte(byte[] arrData, int iOffset, int iLength, byte btValue)
  {
    if (arrData == null)
      throw new ArgumentNullException();
    for (int index = iOffset; index < iLength; ++index)
      arrData[index] = btValue;
  }

  private void DecryptBuffer(DataProvider provider, int startOffset, int length, WordKey key)
  {
    byte index1 = key.X;
    byte index2 = key.Y;
    byte[] status = key.Status;
    int num1 = 0;
    while (num1 < length)
    {
      index1 = (byte) (((int) index1 + 1) % 256 /*0x0100*/);
      index2 = (byte) (((int) status[(int) index1] + (int) index2) % 256 /*0x0100*/);
      this.Swap(ref status[(int) index1], ref status[(int) index2]);
      byte index3 = (byte) (((int) status[(int) index1] + (int) status[(int) index2]) % 256 /*0x0100*/);
      if (provider != null)
      {
        byte num2 = (byte) ((uint) provider.ReadByte(startOffset) ^ (uint) status[(int) index3]);
        provider.WriteByte(startOffset, num2);
      }
      ++num1;
      ++startOffset;
    }
    key.Status = status;
    key.X = index1;
    key.Y = index2;
  }

  private void CheckPrepared()
  {
    if (this.m_baDocumentID == null)
      throw new ApplicationException("Decryption wasn't prepared.");
  }

  private WordKey PrepareKey(long position)
  {
    WordKey key = new WordKey();
    int num = (int) (position - this.m_lLastStreamPosition);
    int block = (int) (position / 1024L /*0x0400*/);
    this.MakeKey(key, (uint) block, this.m_valContext);
    this.DecryptBuffer((DataProvider) null, 0, (int) (position % 1024L /*0x0400*/), key);
    this.m_lLastStreamPosition = position;
    return key;
  }

  public bool SetDecryptionInfo(
    byte[] docId,
    byte[] encryptedDocId,
    byte[] digest,
    string password)
  {
    if (docId == null)
      throw new ArgumentNullException(nameof (docId));
    if (encryptedDocId == null)
      throw new ArgumentNullException(nameof (encryptedDocId));
    if (digest == null)
      throw new ArgumentNullException(nameof (digest));
    if (password == null)
      throw new ArgumentNullException(nameof (password));
    if (16 /*0x10*/ != docId.Length)
      throw new ArgumentOutOfRangeException(nameof (docId));
    if (16 /*0x10*/ != encryptedDocId.Length)
      throw new ArgumentOutOfRangeException(nameof (encryptedDocId));
    if (16 /*0x10*/ != digest.Length)
      throw new ArgumentOutOfRangeException(nameof (digest));
    this.m_valContext = new MD5Context();
    this.m_baDocumentID = new byte[16 /*0x10*/];
    this.m_baPoint = new byte[64 /*0x40*/];
    this.m_baHash = new byte[16 /*0x10*/];
    Buffer.BlockCopy((Array) docId, 0, (Array) this.m_baDocumentID, 0, 16 /*0x10*/);
    Buffer.BlockCopy((Array) encryptedDocId, 0, (Array) this.m_baPoint, 0, 16 /*0x10*/);
    Buffer.BlockCopy((Array) digest, 0, (Array) this.m_baHash, 0, 16 /*0x10*/);
    return this.CheckPassword(password);
  }

  public void SetEncryptionInfo(byte[] docId, string password)
  {
    if (password == null)
      throw new ArgumentNullException(nameof (password));
    if (docId == null || docId.Length != 16 /*0x10*/)
      throw new ArgumentOutOfRangeException(nameof (docId));
    this.m_valContext = new MD5Context();
    this.m_baDocumentID = docId;
    this.PreparePassword(password);
    this.PrepareValContext();
    this.m_baPoint = new byte[64 /*0x40*/];
    Buffer.BlockCopy((Array) docId, 0, (Array) this.m_baPoint, 0, 16 /*0x10*/);
    this.m_baPoint[16 /*0x10*/] = (byte) 128 /*0x80*/;
    MD5Decryptor.SetByte(this.m_baPoint, 17, 47, (byte) 0);
    this.m_baPoint[56] = (byte) 128 /*0x80*/;
    MD5Context md5Context = new MD5Context();
    md5Context.Update(this.m_baPoint, 64U /*0x40*/);
    md5Context.StoreDigest();
    this.m_baHash = new byte[16 /*0x10*/];
    Buffer.BlockCopy((Array) md5Context.Digest, 0, (Array) this.m_baHash, 0, 16 /*0x10*/);
    WordKey key = new WordKey();
    this.MakeKey(key, 0U, this.m_valContext);
    ByteArrayDataProvider provider = new ByteArrayDataProvider(this.m_baPoint);
    this.DecryptBuffer((DataProvider) provider, 0, 16 /*0x10*/, key);
    provider.SetBuffer(this.m_baHash);
    this.DecryptBuffer((DataProvider) provider, 0, 16 /*0x10*/, key);
  }

  public void Encrypt(DataProvider provider, int offset, int length, long streamPosition)
  {
    this.Decrypt(provider, offset, length, streamPosition);
  }

  public void Encrypt(byte[] data, int offset, int length, long streamPosition)
  {
    if (this.m_provider == null)
      this.m_provider = new ByteArrayDataProvider(data);
    else
      this.m_provider.SetBuffer(data);
    this.Encrypt((DataProvider) this.m_provider, offset, length, streamPosition);
  }

  public FilePassRecord GetFilePassRecord()
  {
    FilePassRecord record = (FilePassRecord) BiffRecordFactory.GetRecord(TBIFFRecord.FilePass);
    record.IsWeakEncryption = false;
    record.Key = record.Hash = (ushort) 1;
    record.CreateStandardBlock();
    FilePassStandardBlock standardBlock = record.StandardBlock;
    Buffer.BlockCopy((Array) this.m_baDocumentID, 0, (Array) standardBlock.DocumentID, 0, 16 /*0x10*/);
    Buffer.BlockCopy((Array) this.m_baPoint, 0, (Array) standardBlock.EncyptedDocumentID, 0, 16 /*0x10*/);
    Buffer.BlockCopy((Array) this.m_baHash, 0, (Array) standardBlock.Digest, 0, 16 /*0x10*/);
    return record;
  }
}
