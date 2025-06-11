// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.CipherUtils
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal sealed class CipherUtils
{
  private static readonly IDictionary m_algorithms = (IDictionary) new Hashtable();
  private static readonly IDictionary m_ids = (IDictionary) new Hashtable();

  internal static ICollection Algorithms => CipherUtils.m_ids.Keys;

  static CipherUtils()
  {
    ((CipherUtils.Algorithm) Enums.GetArbitraryValue(typeof (CipherUtils.Algorithm))).ToString();
    ((CipherUtils.Mode) Enums.GetArbitraryValue(typeof (CipherUtils.Mode))).ToString();
    ((CipherUtils.PaddingType) Enums.GetArbitraryValue(typeof (CipherUtils.PaddingType))).ToString();
  }

  internal static IBufferedCipher GetCipher(string algorithm)
  {
    algorithm = algorithm != null ? algorithm.ToUpperInvariant() : throw new ArgumentNullException(nameof (algorithm));
    string str1 = (string) null;
    if (CipherUtils.m_algorithms.Count > 0)
      str1 = (string) CipherUtils.m_algorithms[(object) algorithm];
    if (str1 != null)
      algorithm = str1;
    string[] strArray = algorithm.Split('/');
    ICipher cipher1 = (ICipher) null;
    ICipherBlock cipher2 = (ICipherBlock) null;
    string str2 = strArray[0];
    if (CipherUtils.m_algorithms.Count > 0)
      str1 = (string) CipherUtils.m_algorithms[(object) str2];
    if (str1 != null)
      str2 = str1;
    CipherUtils.Algorithm enumValue;
    try
    {
      enumValue = (CipherUtils.Algorithm) Enums.GetEnumValue(typeof (CipherUtils.Algorithm), str2);
    }
    catch (ArgumentException ex)
    {
      throw new Exception("Invalid cipher " + algorithm);
    }
    switch (enumValue)
    {
      case CipherUtils.Algorithm.DES:
        cipher1 = (ICipher) new DataEncryption();
        break;
      case CipherUtils.Algorithm.DESEDE:
        cipher1 = (ICipher) new DesEdeAlogorithm();
        break;
      case CipherUtils.Algorithm.RC2:
        cipher1 = (ICipher) new RC2Algorithm();
        break;
      case CipherUtils.Algorithm.RSA:
        cipher2 = (ICipherBlock) new RSAAlgorithm();
        break;
      default:
        throw new Exception("Invalid cipher " + algorithm);
    }
    bool flag = true;
    IPadding padding = (IPadding) null;
    if (strArray.Length > 2)
    {
      string s = strArray[2];
      CipherUtils.PaddingType paddingType;
      switch (s)
      {
        case "":
          paddingType = CipherUtils.PaddingType.RAW;
          break;
        case "X9.23PADDING":
          paddingType = CipherUtils.PaddingType.X923PADDING;
          break;
        default:
          try
          {
            paddingType = (CipherUtils.PaddingType) Enums.GetEnumValue(typeof (CipherUtils.PaddingType), s);
            break;
          }
          catch (ArgumentException ex)
          {
            throw new Exception("Invalid cipher " + algorithm);
          }
      }
      switch (paddingType)
      {
        case CipherUtils.PaddingType.NOPADDING:
          flag = false;
          break;
        case CipherUtils.PaddingType.RAW:
        case CipherUtils.PaddingType.WITHCipherTextStealing:
          break;
        case CipherUtils.PaddingType.PKCS1:
        case CipherUtils.PaddingType.PKCS1PADDING:
          ICipherBlock cipherBlock = (ICipherBlock) new Pkcs1Encoding(cipher2);
          break;
        case CipherUtils.PaddingType.PKCS5:
        case CipherUtils.PaddingType.PKCS5PADDING:
        case CipherUtils.PaddingType.PKCS7:
        case CipherUtils.PaddingType.PKCS7PADDING:
          padding = (IPadding) new Pkcs7Padding();
          break;
        default:
          throw new Exception("Invalid cipher " + algorithm);
      }
    }
    if (strArray.Length > 1)
    {
      string str3 = strArray[1];
      int length = -1;
      for (int index = 0; index < str3.Length; ++index)
      {
        if (char.IsDigit(str3[index]))
        {
          length = index;
          break;
        }
      }
      string s = length >= 0 ? str3.Substring(0, length) : str3;
      try
      {
        switch (s == "" ? 1 : (int) Enums.GetEnumValue(typeof (CipherUtils.Mode), s))
        {
          case 0:
          case 1:
            break;
          case 2:
            cipher1 = (ICipher) new CipherBlockChainingMode(cipher1);
            break;
          case 3:
            cipher1 = (ICipher) new CipherBlockChainingMode(cipher1);
            break;
          default:
            throw new Exception("Invalid cipher " + algorithm);
        }
      }
      catch (ArgumentException ex)
      {
        throw new Exception("Invalid cipher " + algorithm);
      }
    }
    if (cipher1 == null)
      throw new Exception("Invalid cipher " + algorithm);
    if (padding != null)
      return (IBufferedCipher) new BufferedBlockPadding(cipher1, padding);
    return !flag || cipher1.IsBlock ? (IBufferedCipher) new BufferedCipher(cipher1) : (IBufferedCipher) new BufferedBlockPadding(cipher1);
  }

  private enum Algorithm
  {
    DES,
    DESEDE,
    RC2,
    RSA,
  }

  private enum Mode
  {
    ECB,
    NONE,
    CBC,
    CTS,
  }

  private enum PaddingType
  {
    NOPADDING,
    RAW,
    PKCS1,
    PKCS1PADDING,
    PKCS5,
    PKCS5PADDING,
    PKCS7,
    PKCS7PADDING,
    WITHCipherTextStealing,
    X923PADDING,
  }
}
