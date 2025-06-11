// Decompiled with JetBrains decompiler
// Type: Syncfusion.Compression.Zip.ZipCrypto
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.Compression.Zip;

internal class ZipCrypto
{
  private Stream m_dataStream;
  private string m_password;
  private uint m_iCrc;
  private uint[] m_Keys;
  private ZipCrc32 m_crc32;

  internal ZipCrypto(Stream dataStream, string password, uint crc)
  {
    this.m_dataStream = dataStream;
    this.m_iCrc = crc;
    this.UpdatePassword(password);
  }

  internal ZipCrypto(string password, uint crc)
  {
    this.UpdatePassword(password);
    this.m_iCrc = crc;
  }

  private void Initialize()
  {
    this.m_Keys = new uint[3]
    {
      305419896U,
      591751049U,
      878082192U
    };
    this.m_crc32 = new ZipCrc32();
  }

  private void UpdatePassword(string password)
  {
    this.m_password = password;
    this.Initialize();
  }

  private void UpdateKeys(byte byteVal)
  {
    this.m_Keys[0] = this.m_crc32.ComputeCrc(this.m_Keys[0], (uint) byteVal);
    this.m_Keys[1] = (uint) (((int) this.m_Keys[1] + ((int) this.m_Keys[0] & (int) byte.MaxValue)) * 134775813 + 1);
    this.m_Keys[2] = this.m_crc32.ComputeCrc(this.m_Keys[2], this.m_Keys[1] >> 24);
  }

  internal void InitiateCipher(string passphrase)
  {
    byte[] bytes = Encoding.UTF8.GetBytes(passphrase);
    for (int index = 0; index < passphrase.Length; ++index)
      this.UpdateKeys(bytes[index]);
  }

  internal void Write(string password)
  {
    if (password == null)
      throw new ArgumentNullException(nameof (password));
    this.InitiateCipher(password);
  }

  internal byte[] EncryptMessage(byte[] plainData)
  {
    long length = (long) plainData.Length;
    if (plainData == null)
      throw new ArgumentNullException(nameof (plainData));
    byte[] numArray = new byte[length];
    for (int index = 0; (long) index < length; ++index)
    {
      byte byteVal = plainData[index];
      numArray[index] = (byte) ((uint) plainData[index] ^ (uint) this.GetCryptoByte());
      this.UpdateKeys(byteVal);
    }
    return numArray;
  }

  internal byte[] DecryptMessage(byte[] cipherData)
  {
    long length = (long) cipherData.Length;
    if (cipherData == null)
      throw new ArgumentNullException(nameof (cipherData));
    byte[] numArray = new byte[length];
    for (int index = 0; (long) index < length; ++index)
    {
      byte byteVal = (byte) ((uint) cipherData[index] ^ (uint) this.GetCryptoByte());
      this.UpdateKeys(byteVal);
      numArray[index] = byteVal;
    }
    return numArray;
  }

  internal byte[] Decrypt(byte[] data)
  {
    int length = 12;
    byte[] numArray1 = new byte[length];
    Buffer.BlockCopy((Array) data, 0, (Array) numArray1, 0, length);
    byte[] numArray2 = new byte[data.Length - length];
    Buffer.BlockCopy((Array) data, length, (Array) numArray2, 0, numArray2.Length);
    this.InitiateCipher(this.m_password);
    if ((int) this.DecryptMessage(numArray1)[11] != ((int) (this.m_iCrc >> 24) & (int) byte.MaxValue))
      throw new Exception("Password is Incorrect");
    return this.DecryptMessage(numArray2);
  }

  internal byte[] Encrypt(byte[] data)
  {
    this.Write(this.m_password);
    byte[] random = ZipArchiveItem.CreateRandom(12);
    this.m_dataStream.Position = 0L;
    random[11] = (byte) (this.m_iCrc >> 24 & (uint) byte.MaxValue);
    byte[] src1 = this.EncryptMessage(random);
    byte[] src2 = this.EncryptMessage(data);
    byte[] dst = new byte[data.Length + random.Length];
    Buffer.BlockCopy((Array) src1, 0, (Array) dst, 0, src1.Length);
    Buffer.BlockCopy((Array) src2, 0, (Array) dst, src1.Length, src2.Length);
    return dst;
  }

  private byte GetCryptoByte()
  {
    ushort num = (ushort) ((int) this.m_Keys[2] & (int) ushort.MaxValue | 2);
    return (byte) ((int) num * ((int) num ^ 1) >> 8);
  }
}
