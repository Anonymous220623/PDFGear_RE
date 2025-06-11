// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.Encrypt.PasswordHelper
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.BasicTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace PDFKit.Utils.Encrypt;

public static class PasswordHelper
{
  private static readonly byte[] kDefaultPasscode = new byte[32 /*0x20*/]
  {
    (byte) 40,
    (byte) 191,
    (byte) 78,
    (byte) 94,
    (byte) 78,
    (byte) 117,
    (byte) 138,
    (byte) 65,
    (byte) 100,
    (byte) 0,
    (byte) 78,
    (byte) 86,
    byte.MaxValue,
    (byte) 250,
    (byte) 1,
    (byte) 8,
    (byte) 46,
    (byte) 46,
    (byte) 0,
    (byte) 182,
    (byte) 208 /*0xD0*/,
    (byte) 104,
    (byte) 62,
    (byte) 128 /*0x80*/,
    (byte) 47,
    (byte) 12,
    (byte) 169,
    (byte) 254,
    (byte) 100,
    (byte) 83,
    (byte) 105,
    (byte) 122
  };
  private const uint kRequiredOkeyLength = 32 /*0x20*/;

  public static bool TryGetUserPassword(
    PdfDocument doc,
    string ownerPassword,
    out string userPassword)
  {
    userPassword = string.Empty;
    if (doc == null || string.IsNullOrEmpty(ownerPassword))
      return false;
    try
    {
      PdfTypeBase pdfTypeBase1;
      PdfTypeBase pdfTypeBase2;
      int keylen;
      if (doc.Trailer.TryGetValue("Encrypt", out pdfTypeBase1) && pdfTypeBase1.Is<PdfTypeDictionary>() && pdfTypeBase1.As<PdfTypeDictionary>().TryGetValue("O", out pdfTypeBase2) && pdfTypeBase2.Is<PdfTypeString>() && PasswordHelper.LoadCryptInfo(doc, out keylen, out PasswordHelper.FXCIPHER _))
      {
        byte[] hexStringBytes = PasswordHelper.GetHexStringBytes(pdfTypeBase2.As<PdfTypeString>());
        if (hexStringBytes.Length >= 32 /*0x20*/)
        {
          byte[] numArray1 = PasswordHelper.MD5Generate(PasswordHelper.GetPassCode(ownerPassword));
          if (doc.SecurityRevision >= 3)
          {
            for (int index = 0; index < 50; ++index)
              numArray1 = PasswordHelper.MD5Generate(numArray1);
          }
          byte[] numArray2 = new byte[32 /*0x20*/];
          int length = Math.Min(keylen, numArray1.Length);
          Array.Copy((Array) numArray1, (Array) numArray2, length);
          byte[] numArray3;
          if (doc.SecurityRevision == 2)
          {
            numArray3 = PasswordHelper.ArcFourCrypt(hexStringBytes, ((IEnumerable<byte>) numArray2).Take<byte>(keylen).ToArray<byte>());
          }
          else
          {
            numArray3 = ((IEnumerable<byte>) hexStringBytes).ToArray<byte>();
            for (int index1 = 19; index1 >= 0; --index1)
            {
              byte[] source = new byte[32 /*0x20*/];
              for (int index2 = 0; index2 < keylen; ++index2)
                source[index2] = (byte) ((uint) numArray2[index2] ^ (uint) (byte) index1);
              numArray3 = PasswordHelper.ArcFourCrypt(numArray3, ((IEnumerable<byte>) source).Take<byte>(keylen).ToArray<byte>());
            }
          }
          userPassword = PasswordHelper.GetKeyString(numArray3);
          return true;
        }
      }
      return false;
    }
    catch
    {
    }
    return false;
  }

  private static bool LoadCryptInfo(
    PdfDocument doc,
    out int keylen,
    out PasswordHelper.FXCIPHER cipher)
  {
    cipher = PasswordHelper.FXCIPHER.NONE;
    keylen = 0;
    PdfTypeBase pdfTypeBase1;
    if (doc == null || !doc.Trailer.TryGetValue("Encrypt", out pdfTypeBase1) || !pdfTypeBase1.Is<PdfTypeDictionary>())
      return false;
    PdfTypeDictionary dict1 = pdfTypeBase1.As<PdfTypeDictionary>();
    int intValue1 = dict1.TryGetIntValue("V", 0);
    if (intValue1 >= 4)
    {
      string key = "";
      PdfTypeBase pdfTypeBase2;
      PdfTypeBase pdfTypeBase3;
      if (dict1.TryGetValue("StmF", out pdfTypeBase2) && dict1.TryGetValue("StrF", out pdfTypeBase3) && pdfTypeBase2.Is<PdfTypeName>() && pdfTypeBase3.Is<PdfTypeName>())
      {
        key = pdfTypeBase3.As<PdfTypeName>().Value;
        if (key != pdfTypeBase2.As<PdfTypeName>().Value)
          return false;
      }
      PdfTypeBase pdfTypeBase4;
      if (!dict1.TryGetValue("CF", out pdfTypeBase4) || !pdfTypeBase4.Is<PdfTypeDictionary>())
        return false;
      PdfTypeDictionary pdfTypeDictionary = pdfTypeBase4.As<PdfTypeDictionary>();
      if (key != "Identity")
      {
        PdfTypeBase pdfTypeBase5;
        if (!pdfTypeDictionary.TryGetValue(key, out pdfTypeBase5) || !pdfTypeBase5.Is<PdfTypeDictionary>())
          return false;
        PdfTypeDictionary dict2 = pdfTypeBase5.As<PdfTypeDictionary>();
        int intValue2;
        if (intValue1 == 4)
        {
          intValue2 = dict2.TryGetIntValue("Length", 0);
          if (intValue2 == 0)
            intValue2 = dict1.TryGetIntValue("Length", 128 /*0x80*/);
        }
        else
          intValue2 = dict1.TryGetIntValue("Length", 256 /*0x0100*/);
        if (intValue2 < 0)
          return false;
        if (intValue2 < 40)
          intValue2 *= 8;
        keylen = intValue2 / 8;
        PdfTypeBase pdfTypeBase6;
        if (dict2.TryGetValue("CFM", out pdfTypeBase6) && pdfTypeBase6.Is<PdfTypeName>())
        {
          string str = pdfTypeBase6.As<PdfTypeName>().Value;
          if (str == "AESV2" || str == "AESV3")
            cipher = PasswordHelper.FXCIPHER.AES;
        }
      }
    }
    else
      keylen = intValue1 > 1 ? dict1.TryGetIntValue("Length", 40) / 8 : 5;
    return true;
  }

  private static int TryGetIntValue(this PdfTypeDictionary dict, string key, int defaultValue)
  {
    PdfTypeBase pdfTypeBase;
    return dict == null || string.IsNullOrEmpty(key) || !dict.TryGetValue(key, out pdfTypeBase) || !pdfTypeBase.Is<PdfTypeNumber>() ? defaultValue : pdfTypeBase.As<PdfTypeNumber>().IntValue;
  }

  private static string GetKeyString(byte[] keyWithPadding)
  {
    int count1 = 32 /*0x20*/;
    if (keyWithPadding.Length != count1)
      return string.Empty;
    for (int count2 = 0; count2 < count1; ++count2)
    {
      if (((IEnumerable<byte>) keyWithPadding).Skip<byte>(count2).SequenceEqual<byte>(((IEnumerable<byte>) PasswordHelper.kDefaultPasscode).Take<byte>(count1 - count2)))
        return count2 == 0 ? string.Empty : Encoding.ASCII.GetString(keyWithPadding, 0, count2);
    }
    return Encoding.ASCII.GetString(keyWithPadding, 0, count1);
  }

  private static byte[] GetPassCode(string pwd)
  {
    int count = 32 /*0x20*/ - pwd.Length;
    List<byte> list = ((IEnumerable<byte>) Encoding.ASCII.GetBytes(pwd)).ToList<byte>();
    list.AddRange(((IEnumerable<byte>) PasswordHelper.kDefaultPasscode).Take<byte>(count));
    return list.ToArray();
  }

  private static byte[] MD5Generate(byte[] passCode)
  {
    using (MD5 md5 = MD5.Create())
      return md5.ComputeHash(passCode, 0, passCode.Length);
  }

  private static byte[] ArcFourCrypt(byte[] data, byte[] key)
  {
    return RC4(key, data);

    static byte[] RC4(byte[] _pwd, byte[] _data)
    {
      int[] numArray1 = new int[256 /*0x0100*/];
      int[] numArray2 = new int[256 /*0x0100*/];
      byte[] numArray3 = new byte[_data.Length];
      for (int index = 0; index < 256 /*0x0100*/; ++index)
      {
        numArray1[index] = (int) _pwd[index % _pwd.Length];
        numArray2[index] = index;
      }
      int index1;
      for (int index2 = index1 = 0; index1 < 256 /*0x0100*/; ++index1)
      {
        index2 = (index2 + numArray2[index1] + numArray1[index1]) % 256 /*0x0100*/;
        int num = numArray2[index1];
        numArray2[index1] = numArray2[index2];
        numArray2[index2] = num;
      }
      int num1;
      int index3 = num1 = 0;
      int index4 = num1;
      int index5 = num1;
      for (; index3 < _data.Length; ++index3)
      {
        index5 = (index5 + 1) % 256 /*0x0100*/;
        index4 = (index4 + numArray2[index5]) % 256 /*0x0100*/;
        int num2 = numArray2[index5];
        numArray2[index5] = numArray2[index4];
        numArray2[index4] = num2;
        int num3 = numArray2[(numArray2[index5] + numArray2[index4]) % 256 /*0x0100*/];
        numArray3[index3] = (byte) ((uint) _data[index3] ^ (uint) num3);
      }
      return numArray3;
    }
  }

  private static byte[] GetHexStringBytes(PdfTypeString pdfStr)
  {
    if (pdfStr == null || !pdfStr.IsHex)
      return (byte[]) null;
    using (PdfDocument pdfDocument = PdfDocument.CreateNew(new PdfForms()))
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        pdfDocument.Root["PDFXSTR"] = pdfStr.Clone();
        pdfDocument.Save((Stream) memoryStream, SaveFlags.NoIncremental);
        memoryStream.Seek(0L, SeekOrigin.Begin);
        long length = memoryStream.Length;
        byte[] numArray = new byte[length];
        memoryStream.Read(numArray, 0, (int) length);
        Match match = Regex.Match(Encoding.UTF8.GetString(numArray), "/PDFXSTR<(.+?)>");
        if (match.Success)
        {
          string str = match.Groups[1].Value;
          if (str.Length % 2 == 1)
            str += "0";
          byte[] hexStringBytes = new byte[str.Length / 2];
          for (int index = 0; index < str.Length; index += 2)
            hexStringBytes[index / 2] = (byte) ((uint) ToNumber(str[index]) * 16U /*0x10*/ + (uint) ToNumber(str[index + 1]));
          return hexStringBytes;
        }
      }
    }
    return (byte[]) null;

    static byte ToNumber(char _c)
    {
      if (_c >= '0' && _c <= '9')
        return (byte) ((uint) _c - 48U /*0x30*/);
      if (_c >= 'A' && _c <= 'F')
        return (byte) ((int) _c - 65 + 10);
      if (_c >= 'a' && _c <= 'f')
        return (byte) ((int) _c - 97 + 10);
      throw new ArgumentException(nameof (_c));
    }
  }

  private enum FXCIPHER
  {
    NONE,
    RC4,
    AES,
    AES2,
  }
}
