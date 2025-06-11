// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Security.DocxProtection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Security;

[CLSCompliant(false)]
internal class DocxProtection
{
  internal const int SpinCount = 100000;
  internal const string CryptographicType = "rsaFull";
  internal const string CryptographicAlgorithmClass = "hash";
  internal const string CryptographicAlgorithmType = "typeAny";
  internal const int CryptographicAlgorithmId = 4;

  internal DocxProtection()
  {
  }

  internal byte[] ComputeHash(byte[] salt, uint encryptedPassword)
  {
    byte[] numArray = new byte[4];
    for (int index = 0; index < 4; ++index)
      numArray[index] = Convert.ToByte((uint) ((ulong) encryptedPassword & (ulong) ((int) byte.MaxValue << index * 8)) >> index * 8);
    string empty = string.Empty;
    for (int index = 0; index < 4; ++index)
      empty += numArray[index].ToString("X2");
    byte[] bytes = Encoding.Unicode.GetBytes(empty.Trim().Trim('\uFEFF').ToUpper());
    byte[] buffer1 = this.CombineByteArrays(salt, bytes);
    SHA1 shA1 = (SHA1) new SHA1CryptoServiceProvider();
    byte[] hash = shA1.ComputeHash(buffer1);
    byte[] buffer2 = new byte[24];
    for (int index1 = 0; index1 < 100000; ++index1)
    {
      hash.CopyTo((Array) buffer2, 0);
      int num = index1;
      for (int index2 = 0; index2 < 4; ++index2)
      {
        buffer2[hash.Length + index2] = (byte) num;
        num >>= 8;
      }
      hash = shA1.ComputeHash(buffer2);
    }
    return hash;
  }

  private byte[] CombineByteArrays(byte[] array1, byte[] array2)
  {
    byte[] dst = new byte[array1.Length + array2.Length];
    Buffer.BlockCopy((Array) array1, 0, (Array) dst, 0, array1.Length);
    Buffer.BlockCopy((Array) array2, 0, (Array) dst, array1.Length, array2.Length);
    return dst;
  }

  internal byte[] CreateSalt(int length)
  {
    byte[] salt = length > 0 ? new byte[length] : throw new ArgumentOutOfRangeException(nameof (length));
    Random random = new Random((int) DateTime.Now.Ticks);
    int maxValue = 256 /*0x0100*/;
    for (int index = 0; index < length; ++index)
      salt[index] = (byte) random.Next(maxValue);
    return salt;
  }
}
