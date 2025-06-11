// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.AAE
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System.IO;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace CommomLib.Commom;

internal class AAE
{
  private static byte[] _str = new byte[16 /*0x10*/]
  {
    (byte) 18,
    (byte) 52,
    (byte) 134,
    (byte) 56,
    (byte) 70,
    (byte) 171,
    (byte) 197,
    (byte) 239,
    (byte) 18,
    (byte) 100,
    (byte) 86,
    (byte) 120,
    (byte) 144 /*0x90*/,
    (byte) 171,
    (byte) 77,
    (byte) 226
  };
  public static byte[] s = new byte[16 /*0x10*/]
  {
    (byte) 18,
    (byte) 63 /*0x3F*/,
    (byte) 130,
    (byte) 168,
    (byte) 59,
    (byte) 162,
    (byte) 53,
    (byte) 86,
    (byte) 120,
    (byte) 144 /*0x90*/,
    (byte) 107,
    (byte) 77,
    (byte) 226,
    (byte) 239,
    (byte) 149,
    (byte) 108
  };

  public static byte[] AD(byte[] cipherText, string strKey)
  {
    SymmetricAlgorithm symmetricAlgorithm = (SymmetricAlgorithm) Rijndael.Create();
    symmetricAlgorithm.Key = Encoding.UTF8.GetBytes(strKey);
    symmetricAlgorithm.IV = AAE._str;
    byte[] buffer = new byte[cipherText.Length];
    MemoryStream memoryStream = new MemoryStream(cipherText);
    CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, symmetricAlgorithm.CreateDecryptor(), CryptoStreamMode.Read);
    cryptoStream.Read(buffer, 0, buffer.Length);
    cryptoStream.Close();
    memoryStream.Close();
    return buffer;
  }
}
