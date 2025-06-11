// Decompiled with JetBrains decompiler
// Type: pdfconverter.Utils.EncryptUtils
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using System;
using System.IO;
using System.Security.Cryptography;

#nullable disable
namespace pdfconverter.Utils;

public static class EncryptUtils
{
  public static readonly byte[] key = new byte[16 /*0x10*/]
  {
    (byte) 114,
    (byte) 68,
    (byte) 134,
    (byte) 72,
    (byte) 70,
    (byte) 59,
    (byte) 165,
    (byte) 226,
    (byte) 82,
    (byte) 97,
    (byte) 150,
    (byte) 72,
    (byte) 128 /*0x80*/,
    (byte) 164,
    (byte) 78,
    (byte) 227
  };
  public static readonly byte[] iv = new byte[16 /*0x10*/]
  {
    (byte) 162,
    (byte) 52,
    (byte) 182,
    (byte) 120,
    (byte) 71,
    (byte) 75,
    (byte) 149,
    (byte) 228,
    (byte) 37,
    (byte) 98,
    (byte) 138,
    (byte) 44,
    (byte) 46,
    (byte) 180,
    (byte) 77,
    (byte) 179
  };

  public static string EncryptStringToBase64_Aes(string plainText)
  {
    return EncryptUtils.EncryptStringToBase64_Aes(plainText, EncryptUtils.key, EncryptUtils.iv);
  }

  public static string EncryptStringToBase64_Aes(string plainText, byte[] Key, byte[] IV)
  {
    if (plainText == null || plainText.Length <= 0)
      throw new ArgumentNullException(nameof (plainText));
    if (Key == null || Key.Length == 0)
      throw new ArgumentNullException(nameof (Key));
    if (IV == null || IV.Length == 0)
      throw new ArgumentNullException(nameof (IV));
    byte[] array;
    using (Aes aes = Aes.Create())
    {
      aes.Key = Key;
      aes.IV = IV;
      ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, encryptor, CryptoStreamMode.Write))
        {
          using (StreamWriter streamWriter = new StreamWriter((Stream) cryptoStream))
            streamWriter.Write(plainText);
          array = memoryStream.ToArray();
        }
      }
    }
    return Convert.ToBase64String(array);
  }
}
