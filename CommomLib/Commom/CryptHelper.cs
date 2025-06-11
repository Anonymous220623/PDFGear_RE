// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.CryptHelper
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace CommomLib.Commom;

public static class CryptHelper
{
  private static byte[] defaultIV;

  public static byte[] DefaultIV
  {
    get
    {
      byte[] defaultIv = CryptHelper.defaultIV;
      if (defaultIv != null)
        return defaultIv;
      return CryptHelper.defaultIV = new byte[16 /*0x10*/]
      {
        (byte) 97,
        (byte) 98,
        (byte) 99,
        (byte) 100,
        (byte) 101,
        (byte) 102,
        (byte) 103,
        (byte) 104,
        (byte) 105,
        (byte) 106,
        (byte) 107,
        (byte) 108,
        (byte) 109,
        (byte) 110,
        (byte) 111,
        (byte) 112 /*0x70*/
      };
    }
  }

  public static byte[] Encrypt(
    byte[] data,
    byte[] key,
    byte[] iv = null,
    CipherMode mode = CipherMode.ECB,
    PaddingMode padding = PaddingMode.PKCS7)
  {
    using (Aes aes = Aes.Create())
    {
      aes.Key = key;
      aes.IV = iv ?? CryptHelper.DefaultIV;
      aes.Mode = mode;
      aes.Padding = padding;
      using (ICryptoTransform encryptor = aes.CreateEncryptor())
        return encryptor.TransformFinalBlock(data, 0, data.Length);
    }
  }

  public static byte[] Decrypt(
    byte[] data,
    byte[] key,
    byte[] iv = null,
    CipherMode mode = CipherMode.ECB,
    PaddingMode padding = PaddingMode.PKCS7)
  {
    using (Aes aes = Aes.Create())
    {
      aes.Key = key;
      aes.IV = iv ?? CryptHelper.DefaultIV;
      aes.Mode = mode;
      aes.Padding = padding;
      using (ICryptoTransform decryptor = aes.CreateDecryptor())
        return decryptor.TransformFinalBlock(data, 0, data.Length);
    }
  }

  public static byte[] MD5(byte[] data)
  {
    using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
      return md5.ComputeHash(data);
  }

  public static byte[] EncryptText(
    string text,
    byte[] key,
    byte[] iv = null,
    CipherMode mode = CipherMode.ECB,
    PaddingMode padding = PaddingMode.PKCS7)
  {
    return CryptHelper.Encrypt(Encoding.UTF8.GetBytes(text), key, iv, mode, padding);
  }

  public static string DecryptText(
    byte[] data,
    byte[] key,
    byte[] iv = null,
    CipherMode mode = CipherMode.ECB,
    PaddingMode padding = PaddingMode.PKCS7)
  {
    return Encoding.UTF8.GetString(CryptHelper.Encrypt(data, key, iv, mode, padding));
  }

  public static string EncryptTextToHex(
    string text,
    byte[] key,
    byte[] iv = null,
    CipherMode mode = CipherMode.ECB,
    PaddingMode padding = PaddingMode.PKCS7)
  {
    return CryptHelper.ToHexString(CryptHelper.Encrypt(Encoding.UTF8.GetBytes(text), key, iv, mode, padding));
  }

  public static string DecryptTextFromHex(
    string hex,
    byte[] key,
    byte[] iv = null,
    CipherMode mode = CipherMode.ECB,
    PaddingMode padding = PaddingMode.PKCS7)
  {
    return Encoding.UTF8.GetString(CryptHelper.Encrypt(CryptHelper.FromHexString(hex), key, iv, mode, padding));
  }

  public static byte[] MD5Text(string text) => CryptHelper.MD5(Encoding.UTF8.GetBytes(text));

  public static string MD5ToHex(byte[] data) => CryptHelper.ToHexString(CryptHelper.MD5(data));

  public static string MD5TextToHex(string text)
  {
    return CryptHelper.ToHexString(CryptHelper.MD5Text(text));
  }

  public static string GetSign(SortedDictionary<string, string> dict, string aesKey)
  {
    foreach (string key in dict.Keys.ToArray<string>())
    {
      if (string.IsNullOrEmpty(dict[key]))
        dict.Remove(key);
    }
    return CryptHelper.MD5ToHex(CryptHelper.EncryptText(dict.OrderBy<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>) (c => c.Key)).Aggregate<KeyValuePair<string, string>, StringBuilder>(new StringBuilder(), (Func<StringBuilder, KeyValuePair<string, string>, StringBuilder>) ((s, c) => s.Append(c.Key).Append(c.Value))).ToString(), Encoding.UTF8.GetBytes(aesKey)));
  }

  private static string ToHexString(byte[] data)
  {
    return ((IEnumerable<byte>) data).Aggregate<byte, StringBuilder>(new StringBuilder(), (Func<StringBuilder, byte, StringBuilder>) ((s, c) => s.AppendFormat("{0:x2}", (object) c))).ToString();
  }

  private static byte[] FromHexString(string text)
  {
    byte[] numArray = new byte[text.Length / 2];
    char[] chArray = new char[2];
    for (int index = 1; index < text.Length; index += 2)
    {
      char ch1 = text[index - 1];
      char ch2 = text[index];
      numArray[index / 2] = byte.Parse($"{ch1}{ch2}", NumberStyles.HexNumber);
    }
    return numArray;
  }
}
