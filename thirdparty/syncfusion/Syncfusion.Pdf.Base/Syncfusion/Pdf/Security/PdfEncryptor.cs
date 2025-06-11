// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.PdfEncryptor
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class PdfEncryptor
{
  private const int c_newKeyOffset = 5;
  private const int c_key40 = 5;
  private const int c_key128 = 16 /*0x10*/;
  private const int c_key256 = 32 /*0x20*/;
  private const int c_40RevisionNumber = 2;
  private const int c_128RevisionNumber = 3;
  private const int c_bytesAmount = 256 /*0x0100*/;
  private const int c_randomBytesAmount = 16 /*0x10*/;
  private const int c_stringLength = 32 /*0x20*/;
  private const int c_ownerLoopNum = 50;
  private const int c_ownerLoopNum2 = 20;
  private const byte c_flagNum = 255 /*0xFF*/;
  internal const byte c_numBits = 8;
  private const int c_permissionSet = -3904;
  private const int c_permissionCleared = -4;
  private const int c_permissionRevisionTwoMask = 4095 /*0x0FFF*/;
  private bool m_hasComputedPasswordValues;
  private MD5CryptoServiceProvider m_provider;
  private IMessageDigest MD5;
  private byte[] m_customArray;
  private byte[] m_randomBytes;
  private string m_ownerPassword = string.Empty;
  private string m_userPassword = string.Empty;
  private byte[] m_ownerPasswordOut;
  private byte[] m_userPasswordOut;
  private byte[] m_encryptionKey;
  private PdfEncryptionKeySize m_keyLength = PdfEncryptionKeySize.Key128Bit;
  private PdfPermissionsFlags m_permission;
  private int m_revision;
  private bool m_bChanged;
  private static byte[] s_paddingString;
  private static object s_lockObject = new object();
  private bool m_encrypt;
  private int m_permissionValue;
  private static readonly byte[] salt = new byte[4]
  {
    (byte) 115,
    (byte) 65,
    (byte) 108,
    (byte) 84
  };
  private PdfEncryptionAlgorithm m_encryptionAlgorithm = PdfEncryptionAlgorithm.RC4;
  private byte[] m_userEncryptionKeyOut;
  private byte[] m_ownerEncryptionKeyOut;
  private byte[] m_permissionFlag;
  private byte[] m_fileEncryptionKey;
  private byte[] m_userRandomBytes;
  private byte[] m_ownerRandomBytes;
  private SecureRandomAlgorithm m_randomArray = new SecureRandomAlgorithm();
  private SHA256Managed m_hashComputer;
  private bool m_encryptMetadata = true;
  private int m_revisionNumberOut;
  private int m_versionNumberOut;
  private int keyLength;
  private string[] HashAlgorithms = new string[3]
  {
    "SHA-256",
    "SHA-384",
    "SHA-512"
  };
  private byte[] m_documentID;
  private bool m_encryptOnlyAttachment;

  private SHA256Managed HashComputer
  {
    get
    {
      try
      {
        if (this.m_hashComputer == null)
          this.m_hashComputer = new SHA256Managed();
      }
      catch
      {
      }
      return this.m_hashComputer;
    }
  }

  internal PdfArray FileID
  {
    get
    {
      PdfString pdfString = new PdfString(this.RandomBytes);
      return new PdfArray()
      {
        (IPdfPrimitive) pdfString,
        (IPdfPrimitive) pdfString
      };
    }
  }

  public string Filter => SecurityHandlers.Standard.ToString();

  public PdfEncryptionKeySize CryptographicAlgorithm
  {
    get => this.m_keyLength;
    set
    {
      if (this.m_keyLength == value)
        return;
      this.m_keyLength = value;
      this.m_bChanged = true;
      this.m_hasComputedPasswordValues = false;
    }
  }

  public PdfEncryptionAlgorithm EncryptionAlgorithm
  {
    get => this.m_encryptionAlgorithm;
    set => this.m_encryptionAlgorithm = value;
  }

  internal PdfPermissionsFlags Permissions
  {
    get => this.m_permission;
    set
    {
      this.m_bChanged = true;
      this.m_permission = value;
      this.m_permissionValue = (int) ((this.m_permission | (PdfPermissionsFlags) -3904) & (PdfPermissionsFlags) -4);
      if (this.RevisionNumber > 2)
        this.m_permissionValue &= 4095 /*0x0FFF*/;
      this.m_hasComputedPasswordValues = false;
    }
  }

  public int RevisionNumber
  {
    get
    {
      if (this.m_revision != 0)
        return this.m_revision;
      if (this.CryptographicAlgorithm != PdfEncryptionKeySize.Key40Bit)
        return 3;
      return this.m_revisionNumberOut <= 2 ? 2 : this.m_revisionNumberOut;
    }
  }

  internal string OwnerPassword
  {
    get => this.m_encryptOnlyAttachment ? string.Empty : this.m_ownerPassword;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (OwnerPassword));
      if (!(this.m_ownerPassword != value))
        return;
      this.m_bChanged = true;
      this.m_ownerPassword = value;
      this.m_hasComputedPasswordValues = false;
    }
  }

  internal string UserPassword
  {
    get => this.m_userPassword;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (UserPassword));
      if (!(this.m_userPassword != value))
        return;
      this.m_bChanged = true;
      this.m_userPassword = value;
      this.m_hasComputedPasswordValues = false;
    }
  }

  protected byte[] RandomBytes
  {
    get
    {
      if (this.m_randomBytes == null)
      {
        this.m_randomBytes = new byte[16 /*0x10*/];
        this.m_randomArray.NextBytes(this.m_randomBytes);
      }
      return this.m_randomBytes;
    }
  }

  internal bool EncryptOnlyAttachment
  {
    get => this.m_encryptOnlyAttachment;
    set
    {
      this.m_encryptOnlyAttachment = value;
      this.m_hasComputedPasswordValues = false;
    }
  }

  protected byte[] CustomArray
  {
    get => this.m_customArray;
    set
    {
      if (this.m_customArray == value)
        return;
      this.m_customArray = value;
    }
  }

  protected MD5CryptoServiceProvider Provider => this.m_provider;

  protected Encoding SecurityEncoding => Encoding.Default;

  protected byte[] EncryptionKey
  {
    get => this.m_encryptionKey;
    set
    {
      if (this.m_encryptionKey == value)
        return;
      this.m_encryptionKey = value;
    }
  }

  internal bool Encrypt
  {
    get
    {
      bool flag = this.Permissions != PdfPermissionsFlags.Default || this.m_userPassword.Length > 0 || this.m_ownerPassword.Length > 0;
      return this.m_encrypt && flag;
    }
    set => this.m_encrypt = value;
  }

  internal byte[] UserPasswordOut
  {
    get
    {
      this.InitializeData();
      return this.m_userPasswordOut;
    }
  }

  internal byte[] OwnerPasswordOut
  {
    get
    {
      this.InitializeData();
      return this.m_ownerPasswordOut;
    }
  }

  internal bool Changed => this.m_bChanged;

  internal bool EncryptMetaData
  {
    get => this.m_encryptMetadata;
    set
    {
      this.m_hasComputedPasswordValues = false;
      this.m_encryptMetadata = value;
    }
  }

  protected static byte[] PaddingString
  {
    get => PdfEncryptor.s_paddingString;
    set
    {
      lock (PdfEncryptor.s_lockObject)
      {
        if (PdfEncryptor.s_paddingString == value)
          return;
        PdfEncryptor.s_paddingString = value;
      }
    }
  }

  internal PdfEncryptor()
  {
    PdfEncryptor.PaddingString = new byte[32 /*0x20*/]
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
    this.CustomArray = new byte[256 /*0x0100*/];
    this.Encrypt = true;
    this.Permissions = PdfPermissionsFlags.Default;
    try
    {
      this.m_provider = new MD5CryptoServiceProvider();
    }
    catch
    {
      this.MD5 = new MessageDigestFinder().GetDigest(nameof (MD5));
    }
  }

  internal PdfEncryptor Clone()
  {
    PdfEncryptor pdfEncryptor = this.MemberwiseClone() as PdfEncryptor;
    pdfEncryptor.CryptographicAlgorithm = this.m_keyLength;
    pdfEncryptor.UserPassword = this.UserPassword;
    pdfEncryptor.OwnerPassword = this.OwnerPassword;
    pdfEncryptor.Permissions = this.Permissions;
    pdfEncryptor.m_randomBytes = this.m_randomBytes.Clone() as byte[];
    pdfEncryptor.m_customArray = this.m_customArray.Clone() as byte[];
    pdfEncryptor.m_revision = this.m_revision;
    if (this.m_encryptionKey != null)
      pdfEncryptor.m_encryptionKey = this.m_encryptionKey.Clone() as byte[];
    pdfEncryptor.m_customArray = this.m_customArray.Clone() as byte[];
    pdfEncryptor.m_ownerPasswordOut = this.m_ownerPasswordOut.Clone() as byte[];
    pdfEncryptor.m_userPasswordOut = this.m_userPasswordOut.Clone() as byte[];
    pdfEncryptor.m_hasComputedPasswordValues = this.m_hasComputedPasswordValues;
    pdfEncryptor.m_bChanged = this.m_bChanged;
    return pdfEncryptor;
  }

  internal void ReadFromDictionary(PdfDictionary dictionary)
  {
    PdfName pdfName1 = dictionary != null ? PdfCrossTable.Dereference(dictionary["Filter"]) as PdfName : throw new ArgumentNullException(nameof (dictionary));
    if (pdfName1.Value != "Standard")
      throw new PdfDocumentException("Invalid Format: Unsupported security filter: " + pdfName1.Value);
    this.m_permissionValue = dictionary.GetInt("P");
    this.m_permission = (PdfPermissionsFlags) (this.m_permissionValue & 3903);
    this.m_keyLength = (PdfEncryptionKeySize) dictionary.GetInt("V");
    this.m_revisionNumberOut = dictionary.GetInt("R");
    this.m_versionNumberOut = dictionary.GetInt("V");
    this.m_revision = this.m_revisionNumberOut;
    if (this.m_keyLength == PdfEncryptionKeySize.Key256BitRevision6 && this.m_keyLength != (PdfEncryptionKeySize) dictionary.GetInt("R"))
      throw new PdfDocumentException("Invalid Format: V and R entries of the Encryption dictionary doesn't match.");
    if (this.m_keyLength == (PdfEncryptionKeySize.Key40Bit | PdfEncryptionKeySize.Key256BitRevision6))
    {
      this.m_userEncryptionKeyOut = dictionary.GetString("UE").Bytes;
      this.m_ownerEncryptionKeyOut = dictionary.GetString("OE").Bytes;
      this.m_permissionFlag = dictionary.GetString("Perms").Bytes;
    }
    this.m_userPasswordOut = dictionary.GetString("U").Bytes;
    this.m_ownerPasswordOut = dictionary.GetString("O").Bytes;
    this.keyLength = !dictionary.ContainsKey("Length") ? (this.m_keyLength != PdfEncryptionKeySize.Key40Bit ? (this.m_keyLength != PdfEncryptionKeySize.Key128Bit ? 256 /*0x0100*/ : 128 /*0x80*/) : 40) : dictionary.GetInt("Length");
    if (this.keyLength == 128 /*0x80*/ && dictionary.GetInt("R") < 4)
    {
      this.m_keyLength = PdfEncryptionKeySize.Key128Bit;
      this.m_encryptionAlgorithm = PdfEncryptionAlgorithm.RC4;
    }
    else if ((this.keyLength == 128 /*0x80*/ || this.keyLength == 256 /*0x0100*/) && dictionary.GetInt("R") >= 4)
    {
      this.m_keyLength = this.keyLength != 128 /*0x80*/ ? PdfEncryptionKeySize.Key256Bit : PdfEncryptionKeySize.Key128Bit;
      PdfDictionary pdfDictionary = (dictionary["CF"] as PdfDictionary)["StdCF"] as PdfDictionary;
      PdfName pdfName2 = pdfDictionary[new PdfName("AuthEvent")] as PdfName;
      if (pdfName2 != (PdfName) null && pdfName2.Value == "EFOpen")
        this.EncryptOnlyAttachment = true;
      this.m_encryptionAlgorithm = !((pdfDictionary[new PdfName("CFM")] as PdfName).Value != "V2") ? PdfEncryptionAlgorithm.RC4 : PdfEncryptionAlgorithm.AES;
    }
    else if (this.keyLength == 40)
      this.m_keyLength = PdfEncryptionKeySize.Key40Bit;
    else if (this.keyLength <= 128 /*0x80*/ && this.keyLength > 40 && this.keyLength % 8 == 0 && dictionary.GetInt("R") < 4)
    {
      this.m_keyLength = PdfEncryptionKeySize.Key128Bit;
      this.m_encryptionAlgorithm = PdfEncryptionAlgorithm.RC4;
    }
    else
    {
      this.m_keyLength = PdfEncryptionKeySize.Key256Bit;
      this.m_encryptionAlgorithm = PdfEncryptionAlgorithm.AES;
    }
    if (this.m_revisionNumberOut == 6)
    {
      this.m_keyLength = PdfEncryptionKeySize.Key256BitRevision6;
      this.m_encryptionAlgorithm = PdfEncryptionAlgorithm.AES;
    }
    if (this.keyLength != 0 && this.keyLength % 8 != 0 && (this.m_keyLength == PdfEncryptionKeySize.Key40Bit || this.m_keyLength == PdfEncryptionKeySize.Key128Bit || this.m_keyLength == PdfEncryptionKeySize.Key256Bit))
      throw new PdfDocumentException("Invalid format: Invalid/Unsupported security dictionary.");
    this.m_hasComputedPasswordValues = true;
  }

  internal bool CheckPassword(string password, PdfString key, bool attachEncryption)
  {
    if (password == null)
      throw new ArgumentNullException(nameof (password));
    if (key == null)
      throw new ArgumentNullException(nameof (key));
    byte[] randomBytes = this.m_randomBytes;
    this.m_randomBytes = key.Bytes.Clone() as byte[];
    bool flag;
    if (this.AuthenticateOwnerPassword(password))
    {
      this.m_ownerPassword = password;
      flag = true;
    }
    else if (this.AuthenticateUserPassword(password))
    {
      this.m_userPassword = password;
      flag = true;
    }
    else if (!attachEncryption)
    {
      flag = true;
    }
    else
    {
      this.m_encryptionKey = (byte[]) null;
      flag = false;
    }
    if (!flag)
      this.m_randomBytes = randomBytes;
    return flag;
  }

  internal byte[] EncryptData(long currObjNumber, byte[] data, bool isEncryption)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    if (this.CryptographicAlgorithm == PdfEncryptionKeySize.Key256Bit || this.CryptographicAlgorithm == PdfEncryptionKeySize.Key256BitRevision6)
      return isEncryption ? this.EncryptData256(data) : this.DecryptData256(data);
    this.InitializeData();
    int num1 = 0;
    int length1;
    byte[] numArray1;
    if (this.EncryptionKey.Length == 5)
    {
      byte[] originalKey = new byte[this.EncryptionKey.Length + 5];
      int index = 0;
      for (int length2 = this.EncryptionKey.Length; index < length2; ++index)
        originalKey[index] = this.EncryptionKey[index];
      int num2 = this.EncryptionKey.Length - 1;
      int num3;
      originalKey[num3 = num2 + 1] = (byte) currObjNumber;
      int num4;
      originalKey[num4 = num3 + 1] = (byte) (currObjNumber >> 8);
      int num5;
      originalKey[num5 = num4 + 1] = (byte) (currObjNumber >> 16 /*0x10*/);
      int num6;
      originalKey[num6 = num5 + 1] = (byte) num1;
      int num7;
      originalKey[num7 = num6 + 1] = (byte) (num1 >> 8);
      length1 = originalKey.Length;
      numArray1 = this.PrepareKeyForEncryption(originalKey);
    }
    else
    {
      byte[] numArray2 = this.EncryptionAlgorithm != PdfEncryptionAlgorithm.AES ? new byte[this.EncryptionKey.Length + 5] : new byte[this.EncryptionKey.Length + 9];
      Array.Copy((Array) this.EncryptionKey, (Array) numArray2, this.EncryptionKey.Length);
      int num8 = this.EncryptionKey.Length - 1;
      int num9;
      numArray2[num9 = num8 + 1] = (byte) currObjNumber;
      int num10;
      numArray2[num10 = num9 + 1] = (byte) (currObjNumber >> 8);
      int num11;
      numArray2[num11 = num10 + 1] = (byte) (currObjNumber >> 16 /*0x10*/);
      int num12;
      numArray2[num12 = num11 + 1] = (byte) num1;
      int num13;
      numArray2[num13 = num12 + 1] = (byte) (num1 >> 8);
      if (this.EncryptionAlgorithm == PdfEncryptionAlgorithm.AES)
      {
        int num14;
        numArray2[num14 = num13 + 1] = PdfEncryptor.salt[0];
        int num15;
        numArray2[num15 = num14 + 1] = PdfEncryptor.salt[1];
        int num16;
        numArray2[num16 = num15 + 1] = PdfEncryptor.salt[2];
        int num17;
        numArray2[num17 = num16 + 1] = PdfEncryptor.salt[3];
      }
      if (this.Provider != null)
      {
        numArray1 = this.Provider.ComputeHash(numArray2);
      }
      else
      {
        this.MD5.Reset();
        this.MD5.Update(numArray2, 0, numArray2.Length);
        numArray1 = new byte[this.MD5.MessageDigestSize];
        this.MD5.DoFinal(numArray1, 0);
        this.MD5.Reset();
      }
      length1 = numArray1.Length;
    }
    int keyLen = Math.Min(length1, numArray1.Length);
    if (this.EncryptionAlgorithm != PdfEncryptionAlgorithm.AES)
      return this.EncryptDataByCustom(data, numArray1, keyLen);
    return isEncryption ? (this.EncryptOnlyAttachment ? this.AESEncrypt(data, this.EncryptionKey) : this.AESEncrypt(data, numArray1)) : (this.EncryptOnlyAttachment ? this.AESDecrypt(data, this.EncryptionKey) : this.AESDecrypt(data, numArray1));
  }

  internal void SaveToDictionary(PdfDictionary dictionary)
  {
    if (this.Changed)
    {
      this.m_revisionNumberOut = 0;
      this.m_versionNumberOut = 0;
      this.m_revision = 0;
      this.keyLength = 0;
    }
    dictionary.SetName("Filter", "Standard");
    dictionary.SetNumber("P", this.m_permissionValue);
    dictionary.SetProperty("U", (IPdfPrimitive) new PdfString(this.UserPasswordOut));
    dictionary.SetProperty("O", (IPdfPrimitive) new PdfString(this.OwnerPasswordOut));
    if (!dictionary.ContainsKey("Length"))
    {
      dictionary.SetNumber("Length", this.GetKeyLength() * 8);
    }
    else
    {
      this.keyLength = 0;
      dictionary.SetNumber("Length", this.GetKeyLength() * 8);
    }
    bool flag = false;
    if (dictionary.ContainsKey("CF"))
    {
      if ((((dictionary["CF"] as PdfDictionary)["StdCF"] as PdfDictionary)[new PdfName("CFM")] as PdfName).Value != "V2")
        flag = true;
      if (dictionary.ContainsKey("StmF") && dictionary.ContainsKey("StrF") && this.m_versionNumberOut == 0 && this.m_revisionNumberOut == 0)
      {
        this.m_versionNumberOut = 4;
        this.m_revisionNumberOut = 4;
      }
    }
    if (this.m_encryptOnlyAttachment && (this.EncryptionAlgorithm == PdfEncryptionAlgorithm.RC4 || this.CryptographicAlgorithm == PdfEncryptionKeySize.Key40Bit))
      throw new PdfException("Encrypt only attachment is supported in AES algorithm with 128, 256 and 256Revision6 encryptions only.");
    if (!this.EncryptMetaData && this.CryptographicAlgorithm == PdfEncryptionKeySize.Key40Bit)
      throw new PdfException("EncryptAllContentsExceptMetadata PdfEncryptionOptions does not supprot encrption key size key40");
    if (this.m_encryptionAlgorithm == PdfEncryptionAlgorithm.AES || this.CryptographicAlgorithm == PdfEncryptionKeySize.Key256Bit || this.CryptographicAlgorithm == PdfEncryptionKeySize.Key256BitRevision6 || this.CryptographicAlgorithm == PdfEncryptionKeySize.Key128Bit && !this.EncryptMetaData)
    {
      if (this.m_revisionNumberOut > 0 && flag)
        dictionary.SetNumber("R", this.m_revisionNumberOut);
      else
        dictionary.SetNumber("R", (int) (this.m_keyLength + 2));
      if (this.m_versionNumberOut > 0 && flag)
        dictionary.SetNumber("V", this.m_versionNumberOut);
      else
        dictionary.SetNumber("V", (int) (this.m_keyLength + 2));
      if (this.CryptographicAlgorithm == PdfEncryptionKeySize.Key256BitRevision6)
      {
        dictionary.SetNumber("V", 5);
        dictionary.SetNumber("R", 6);
      }
      else if (this.CryptographicAlgorithm == PdfEncryptionKeySize.Key256Bit)
      {
        dictionary.SetNumber("V", 5);
        dictionary.SetNumber("R", 5);
      }
      if (this.m_encryptOnlyAttachment)
      {
        dictionary.SetName("StmF", "Identity");
        dictionary.SetName("StrF", "Identity");
        dictionary.SetName("EFF", "StdCF");
        dictionary.SetBoolean("EncryptMetadata", this.m_encryptMetadata);
      }
      else
      {
        dictionary.SetName("StmF", "StdCF");
        dictionary.SetName("StrF", "StdCF");
        if (dictionary.ContainsKey("EFF"))
          dictionary.Remove("EFF");
      }
      if (!this.m_encryptMetadata)
      {
        if (!dictionary.ContainsKey(new PdfName("EncryptMetadata")))
          dictionary.SetBoolean("EncryptMetadata", this.m_encryptMetadata);
      }
      else if (!this.EncryptOnlyAttachment && dictionary.ContainsKey(new PdfName("EncryptMetadata")))
        dictionary.Remove("EncryptMetadata");
      dictionary.SetProperty("CF", (IPdfPrimitive) new PdfDictionary(this.AESDictionary()));
      if (this.CryptographicAlgorithm == PdfEncryptionKeySize.Key256Bit || this.CryptographicAlgorithm == PdfEncryptionKeySize.Key256BitRevision6)
      {
        dictionary.SetProperty("UE", (IPdfPrimitive) new PdfString(this.m_userEncryptionKeyOut));
        dictionary.SetProperty("OE", (IPdfPrimitive) new PdfString(this.m_ownerEncryptionKeyOut));
        dictionary.SetProperty("Perms", (IPdfPrimitive) new PdfString(this.m_permissionFlag));
      }
    }
    else
    {
      if (this.m_revisionNumberOut > 0 && !flag)
        dictionary.SetNumber("R", this.m_revisionNumberOut);
      else
        dictionary.SetNumber("R", (int) (this.m_keyLength + 1));
      if (this.m_versionNumberOut > 0 && !flag)
        dictionary.SetNumber("V", this.m_versionNumberOut);
      else
        dictionary.SetNumber("V", (int) this.m_keyLength);
    }
    dictionary.Archive = false;
  }

  private byte[] PadTrancateString(string source)
  {
    return source != null ? this.PadTrancateString(this.SecurityEncoding.GetBytes(source)) : throw new ArgumentNullException(nameof (source));
  }

  private byte[] PadTrancateString(byte[] sourceBytes)
  {
    if (sourceBytes == null)
      throw new ArgumentNullException(nameof (sourceBytes));
    byte[] destinationArray = new byte[32 /*0x20*/];
    int length = sourceBytes.Length;
    if (length > 0)
      Array.Copy((Array) sourceBytes, 0, (Array) destinationArray, 0, Math.Min(length, 32 /*0x20*/));
    if (length < 32 /*0x20*/)
      Array.Copy((Array) PdfEncryptor.PaddingString, 0, (Array) destinationArray, length, 32 /*0x20*/ - length);
    return destinationArray;
  }

  private byte[] EncryptDataByCustom(byte[] data, byte[] key)
  {
    return this.EncryptDataByCustom(data, key, key.Length);
  }

  private byte[] EncryptDataByCustom(byte[] data, byte[] key, int keyLen)
  {
    byte[] numArray = new byte[data.Length];
    this.RecreateCustomArray(key, keyLen);
    keyLen = data.Length;
    int index1 = 0;
    int index2 = 0;
    for (int index3 = 0; index3 < keyLen; ++index3)
    {
      index1 = (index1 + 1) % 256 /*0x0100*/;
      index2 = (index2 + (int) this.CustomArray[index1]) % 256 /*0x0100*/;
      byte custom1 = this.CustomArray[index1];
      this.CustomArray[index1] = this.CustomArray[index2];
      this.CustomArray[index2] = custom1;
      byte custom2 = this.CustomArray[((int) this.CustomArray[index1] + (int) this.CustomArray[index2]) % 256 /*0x0100*/];
      numArray[index3] = (byte) ((uint) data[index3] ^ (uint) custom2);
    }
    return numArray;
  }

  private byte[] AESEncrypt(byte[] data, byte[] key)
  {
    MemoryStream memoryStream = new MemoryStream();
    byte[] iv = this.GenerateIV();
    AesEncryptor aesEncryptor = new AesEncryptor(key, iv, true);
    byte[] numArray1 = new byte[aesEncryptor.GetBlockSize(data.Length)];
    aesEncryptor.ProcessBytes(data, 0, data.Length, numArray1, 0);
    memoryStream.Write(numArray1, 0, numArray1.Length);
    byte[] numArray2 = new byte[aesEncryptor.CalculateOutputSize()];
    aesEncryptor.Finalize(numArray2);
    memoryStream.Write(numArray2, 0, numArray2.Length);
    memoryStream.Dispose();
    return memoryStream.ToArray();
  }

  private byte[] AESDecrypt(byte[] data, byte[] key)
  {
    MemoryStream memoryStream = new MemoryStream();
    byte[] numArray1 = new byte[16 /*0x10*/];
    int length1 = data.Length;
    int destinationIndex = 0;
    int length2 = Math.Min(numArray1.Length - destinationIndex, length1);
    Array.Copy((Array) data, 0, (Array) numArray1, destinationIndex, length2);
    int length3 = length1 - length2;
    int inOff = destinationIndex + length2;
    if (inOff != numArray1.Length || length3 <= 0)
      return data;
    AesEncryptor aesEncryptor = new AesEncryptor(key, numArray1, false);
    byte[] numArray2 = new byte[aesEncryptor.GetBlockSize(length3)];
    aesEncryptor.ProcessBytes(data, inOff, length3, numArray2, 0);
    memoryStream.Write(numArray2, 0, numArray2.Length);
    byte[] numArray3 = new byte[aesEncryptor.CalculateOutputSize()];
    int length4 = aesEncryptor.Finalize(numArray3);
    if (numArray3.Length != length4)
    {
      byte[] numArray4 = new byte[length4];
      Array.Copy((Array) numArray3, 0, (Array) numArray4, 0, length4);
      memoryStream.Write(numArray4, 0, numArray4.Length);
    }
    else
      memoryStream.Write(numArray3, 0, numArray3.Length);
    memoryStream.Dispose();
    return memoryStream.ToArray();
  }

  private byte[] EncryptData256(byte[] data) => this.AESEncrypt(data, this.m_fileEncryptionKey);

  private byte[] DecryptData256(byte[] data) => this.AESDecrypt(data, this.m_fileEncryptionKey);

  private byte[] GenerateIV()
  {
    byte[] buffer = new byte[16 /*0x10*/];
    this.m_randomArray.NextBytes(buffer);
    return buffer;
  }

  private void RecreateCustomArray(byte[] key, int keyLen)
  {
    byte[] numArray = new byte[256 /*0x0100*/];
    for (int index = 0; index < 256 /*0x0100*/; ++index)
    {
      numArray[index] = key[index % keyLen];
      this.CustomArray[index] = (byte) index;
    }
    int index1 = 0;
    for (int index2 = 0; index2 < 256 /*0x0100*/; ++index2)
    {
      index1 = (index1 + (int) this.CustomArray[index2] + (int) numArray[index2]) % 256 /*0x0100*/;
      byte custom = this.CustomArray[index2];
      this.CustomArray[index2] = this.CustomArray[index1];
      this.CustomArray[index1] = custom;
    }
  }

  protected internal int GetKeyLength()
  {
    if (this.keyLength != 0)
      return this.keyLength / 8;
    if (this.CryptographicAlgorithm == PdfEncryptionKeySize.Key40Bit)
      return 5;
    return this.CryptographicAlgorithm == PdfEncryptionKeySize.Key128Bit ? 16 /*0x10*/ : 32 /*0x20*/;
  }

  private byte[] CreateOwnerPassword()
  {
    byte[] keyFromOwnerPass = this.GetKeyFromOwnerPass(this.OwnerPassword == null || this.OwnerPassword.Length == 0 ? this.UserPassword : this.OwnerPassword);
    byte[] data = this.EncryptDataByCustom(this.PadTrancateString(this.UserPassword), keyFromOwnerPass, keyFromOwnerPass.Length);
    if (this.RevisionNumber > 2)
    {
      for (byte index = 1; index < (byte) 20; ++index)
      {
        byte[] forOwnerPassStep7 = this.GetKeyForOwnerPassStep7(keyFromOwnerPass, index);
        data = this.EncryptDataByCustom(data, forOwnerPassStep7, forOwnerPassStep7.Length);
      }
    }
    return data;
  }

  private byte[] AcrobatXComputeHash(byte[] input, byte[] password, byte[] Key)
  {
    try
    {
      bool flag = false;
      byte[] numArray1;
      try
      {
        numArray1 = new SHA256Managed().ComputeHash(input);
      }
      catch
      {
        flag = true;
        SHA256MessageDigest a256MessageDigest = new SHA256MessageDigest();
        numArray1 = new byte[a256MessageDigest.MessageDigestSize];
        a256MessageDigest.Update(input, 0, input.Length);
        a256MessageDigest.DoFinal(numArray1, 0);
      }
      byte[] numArray2 = (byte[]) null;
      for (int index1 = 0; index1 < 64 /*0x40*/ || ((int) numArray2[numArray2.Length - 1] & (int) byte.MaxValue) > index1 - 32 /*0x20*/; ++index1)
      {
        byte[] numArray3 = Key == null || Key.Length < 48 /*0x30*/ ? new byte[64 /*0x40*/ * (password.Length + numArray1.Length)] : new byte[64 /*0x40*/ * (password.Length + numArray1.Length + 48 /*0x30*/)];
        int destinationIndex1 = 0;
        try
        {
          for (int index2 = 0; index2 < 64 /*0x40*/; ++index2)
          {
            Array.Copy((Array) password, 0, (Array) numArray3, destinationIndex1, password.Length);
            int destinationIndex2 = destinationIndex1 + password.Length;
            Array.Copy((Array) numArray1, 0, (Array) numArray3, destinationIndex2, numArray1.Length);
            destinationIndex1 = destinationIndex2 + numArray1.Length;
            if (Key != null && Key.Length >= 48 /*0x30*/)
            {
              Array.Copy((Array) Key, 0, (Array) numArray3, destinationIndex1, 48 /*0x30*/);
              destinationIndex1 += 48 /*0x30*/;
            }
          }
        }
        catch (Exception ex)
        {
          throw new Exception(ex.Message);
        }
        byte[] numArray4 = new byte[16 /*0x10*/];
        byte[] numArray5 = new byte[16 /*0x10*/];
        Array.Copy((Array) numArray1, 0, (Array) numArray4, 0, 16 /*0x10*/);
        Array.Copy((Array) numArray1, 16 /*0x10*/, (Array) numArray5, 0, 16 /*0x10*/);
        if (!flag)
        {
          Rijndael rijndael = Rijndael.Create();
          rijndael.Mode = CipherMode.CBC;
          rijndael.KeySize = 256 /*0x0100*/;
          rijndael.Key = numArray4;
          rijndael.IV = numArray5;
          rijndael.Padding = PaddingMode.None;
          MemoryStream memoryStream = new MemoryStream();
          CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, rijndael.CreateEncryptor(numArray4, numArray5), CryptoStreamMode.Write);
          cryptoStream.Write(numArray3, 0, numArray3.Length);
          cryptoStream.Close();
          memoryStream.Dispose();
          numArray2 = memoryStream.ToArray();
        }
        if (flag)
          numArray2 = new AesCipher(true, numArray4, numArray5).Update(numArray3, 0, numArray3.Length);
        byte[] numArray6 = new byte[16 /*0x10*/];
        Array.Copy((Array) numArray2, 0, (Array) numArray6, 0, 16 /*0x10*/);
        string hashAlgorithm = this.HashAlgorithms[Math.Abs((new SfBigInteger(numArray6) % new SfBigInteger(3L)).IntValue())];
        if (!flag)
        {
          switch (hashAlgorithm)
          {
            case "SHA-256":
              numArray1 = new SHA256Managed().ComputeHash(numArray2);
              continue;
            case "SHA-384":
              numArray1 = new SHA384Managed().ComputeHash(numArray2);
              continue;
            default:
              numArray1 = new SHA512Managed().ComputeHash(numArray2);
              continue;
          }
        }
        else
        {
          switch (hashAlgorithm)
          {
            case "SHA-256":
              SHA256MessageDigest a256MessageDigest = new SHA256MessageDigest();
              numArray1 = new byte[a256MessageDigest.MessageDigestSize];
              a256MessageDigest.Update(numArray2, 0, numArray2.Length);
              a256MessageDigest.DoFinal(numArray1, 0);
              continue;
            case "SHA-384":
              SHA384MessageDigest a384MessageDigest = new SHA384MessageDigest();
              numArray1 = new byte[a384MessageDigest.MessageDigestSize];
              a384MessageDigest.Update(numArray2, 0, numArray2.Length);
              a384MessageDigest.DoFinal(numArray1, 0);
              continue;
            default:
              SHA512MessageDigest a512MessageDigest = new SHA512MessageDigest();
              numArray1 = new byte[a512MessageDigest.MessageDigestSize];
              a512MessageDigest.Update(numArray2, 0, numArray2.Length);
              a512MessageDigest.DoFinal(numArray1, 0);
              continue;
          }
        }
      }
      if (numArray1.Length <= 32 /*0x20*/)
        return numArray1;
      byte[] destinationArray = new byte[32 /*0x20*/];
      Array.Copy((Array) numArray1, 0, (Array) destinationArray, 0, 32 /*0x20*/);
      return destinationArray;
    }
    catch (Exception ex)
    {
      throw new IOException(ex.Message);
    }
  }

  private byte[] Create256BitOwnerPassword()
  {
    byte[] numArray1 = new byte[8];
    byte[] numArray2 = new byte[8];
    this.m_ownerRandomBytes = new byte[16 /*0x10*/];
    this.m_randomArray.NextBytes(this.m_ownerRandomBytes);
    if (string.IsNullOrEmpty(this.OwnerPassword))
      this.OwnerPassword = this.UserPassword;
    byte[] bytes = Encoding.UTF8.GetBytes(this.OwnerPassword == null || this.OwnerPassword.Length == 0 ? this.UserPassword : this.OwnerPassword);
    Array.Copy((Array) this.m_ownerRandomBytes, 0, (Array) numArray1, 0, 8);
    Array.Copy((Array) this.m_ownerRandomBytes, 8, (Array) numArray2, 0, 8);
    byte[] numArray3 = new byte[bytes.Length + numArray1.Length + this.m_userPasswordOut.Length];
    Array.Copy((Array) bytes, 0, (Array) numArray3, 0, bytes.Length);
    Array.Copy((Array) numArray1, 0, (Array) numArray3, bytes.Length, numArray1.Length);
    Array.Copy((Array) this.m_userPasswordOut, 0, (Array) numArray3, bytes.Length + numArray1.Length, this.m_userPasswordOut.Length);
    byte[] numArray4;
    if (this.HashComputer != null)
    {
      numArray4 = this.HashComputer.ComputeHash(numArray3);
    }
    else
    {
      numArray4 = new byte[32 /*0x20*/];
      IMessageDigest digest = new MessageDigestFinder().GetDigest("SHA256");
      digest.Update(numArray3, 0, numArray3.Length);
      digest.DoFinal(numArray4, 0);
    }
    byte[] destinationArray = new byte[numArray4.Length + numArray1.Length + numArray2.Length];
    Array.Copy((Array) numArray4, 0, (Array) destinationArray, 0, numArray4.Length);
    Array.Copy((Array) numArray1, 0, (Array) destinationArray, numArray4.Length, numArray1.Length);
    Array.Copy((Array) numArray2, 0, (Array) destinationArray, numArray4.Length + numArray1.Length, numArray2.Length);
    return destinationArray;
  }

  private void CreateAcrobatX256BitOwnerPassword()
  {
    byte[] numArray1 = new byte[8];
    byte[] numArray2 = new byte[8];
    if (string.IsNullOrEmpty(this.OwnerPassword))
      this.OwnerPassword = this.UserPassword;
    byte[] bytes = Encoding.UTF8.GetBytes(this.OwnerPassword == null || this.OwnerPassword.Length == 0 ? this.UserPassword : this.OwnerPassword);
    this.m_randomArray.NextBytes(numArray1);
    this.m_randomArray.NextBytes(numArray2);
    byte[] numArray3 = new byte[bytes.Length + numArray1.Length + this.m_userPasswordOut.Length];
    Array.Copy((Array) bytes, 0, (Array) numArray3, 0, bytes.Length);
    Array.Copy((Array) numArray1, 0, (Array) numArray3, bytes.Length, numArray1.Length);
    Array.Copy((Array) this.m_userPasswordOut, 0, (Array) numArray3, bytes.Length + numArray1.Length, this.m_userPasswordOut.Length);
    byte[] sourceArray = this.AcrobatXComputeHash(numArray3, bytes, this.m_userPasswordOut);
    this.m_ownerPasswordOut = new byte[sourceArray.Length + numArray1.Length + numArray2.Length];
    Array.Copy((Array) sourceArray, 0, (Array) this.m_ownerPasswordOut, 0, sourceArray.Length);
    Array.Copy((Array) numArray1, 0, (Array) this.m_ownerPasswordOut, sourceArray.Length, numArray1.Length);
    Array.Copy((Array) numArray2, 0, (Array) this.m_ownerPasswordOut, sourceArray.Length + numArray1.Length, numArray2.Length);
    byte[] numArray4 = new byte[bytes.Length + numArray2.Length + this.m_userPasswordOut.Length];
    Array.Copy((Array) bytes, 0, (Array) numArray4, 0, bytes.Length);
    Array.Copy((Array) numArray2, 0, (Array) numArray4, bytes.Length, numArray2.Length);
    Array.Copy((Array) this.m_userPasswordOut, 0, (Array) numArray4, bytes.Length + numArray2.Length, this.m_userPasswordOut.Length);
    byte[] key = this.AcrobatXComputeHash(numArray4, bytes, this.m_userPasswordOut);
    try
    {
      Rijndael rijndael = Rijndael.Create();
      rijndael.Mode = CipherMode.CBC;
      rijndael.KeySize = 256 /*0x0100*/;
      rijndael.Key = key;
      rijndael.IV = new byte[16 /*0x10*/];
      rijndael.Padding = PaddingMode.None;
      MemoryStream memoryStream = new MemoryStream();
      CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, rijndael.CreateEncryptor(rijndael.Key, rijndael.IV), CryptoStreamMode.Write);
      cryptoStream.Write(this.m_fileEncryptionKey, 0, this.m_fileEncryptionKey.Length);
      cryptoStream.Close();
      memoryStream.Dispose();
      this.m_ownerEncryptionKeyOut = memoryStream.ToArray();
    }
    catch
    {
      this.m_ownerEncryptionKeyOut = new AesCipherNoPadding(true, key).ProcessBlock(this.m_fileEncryptionKey, 0, this.m_fileEncryptionKey.Length);
    }
  }

  private byte[] CreateOwnerEncryptionKey()
  {
    byte[] destinationArray = new byte[8];
    byte[] numArray1 = new byte[8];
    byte[] bytes = Encoding.UTF8.GetBytes(this.m_ownerPassword);
    Array.Copy((Array) this.m_ownerRandomBytes, 0, (Array) destinationArray, 0, 8);
    Array.Copy((Array) this.m_ownerRandomBytes, 8, (Array) numArray1, 0, 8);
    byte[] numArray2 = new byte[bytes.Length + destinationArray.Length + this.m_userPasswordOut.Length];
    Array.Copy((Array) bytes, 0, (Array) numArray2, 0, bytes.Length);
    Array.Copy((Array) numArray1, 0, (Array) numArray2, bytes.Length, destinationArray.Length);
    Array.Copy((Array) this.m_userPasswordOut, 0, (Array) numArray2, bytes.Length + destinationArray.Length, this.m_userPasswordOut.Length);
    if (this.HashComputer != null)
    {
      byte[] hash = this.HashComputer.ComputeHash(numArray2);
      Rijndael rijndael = Rijndael.Create();
      rijndael.Mode = CipherMode.CBC;
      rijndael.KeySize = 256 /*0x0100*/;
      rijndael.Key = hash;
      rijndael.IV = new byte[16 /*0x10*/];
      rijndael.Padding = PaddingMode.None;
      MemoryStream memoryStream = new MemoryStream();
      CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, rijndael.CreateEncryptor(), CryptoStreamMode.Write);
      cryptoStream.Write(this.m_fileEncryptionKey, 0, this.m_fileEncryptionKey.Length);
      cryptoStream.Close();
      memoryStream.Dispose();
      return memoryStream.ToArray();
    }
    byte[] numArray3 = new byte[32 /*0x20*/];
    IMessageDigest digest = new MessageDigestFinder().GetDigest("SHA256");
    digest.Update(numArray2, 0, numArray2.Length);
    digest.DoFinal(numArray3, 0);
    return new AesCipherNoPadding(true, numArray3).ProcessBlock(this.m_fileEncryptionKey, 0, this.m_fileEncryptionKey.Length);
  }

  private byte[] GetKeyFromOwnerPass(string password)
  {
    byte[] numArray1 = this.PadTrancateString(password);
    byte[] numArray2;
    if (this.Provider != null)
    {
      numArray2 = this.Provider.ComputeHash(numArray1);
      if (this.RevisionNumber > 2)
      {
        for (int index = 0; index < 50; ++index)
          numArray2 = this.Provider.ComputeHash(numArray2);
      }
    }
    else
    {
      MessageDigestAlgorithms digestAlgorithms = new MessageDigestAlgorithms();
      numArray2 = digestAlgorithms.Digest("MD5", numArray1);
      if (this.RevisionNumber > 2)
      {
        byte[] numArray3 = new byte[this.GetKeyLength()];
        IMessageDigest digest = new MessageDigestFinder().GetDigest("MD5");
        for (int index = 0; index < 50; ++index)
          numArray2 = digestAlgorithms.Digest(digest, numArray2, 0, numArray3.Length);
      }
    }
    byte[] destinationArray = new byte[this.GetKeyLength()];
    Array.Copy((Array) numArray2, (Array) destinationArray, destinationArray.Length);
    return destinationArray;
  }

  private void FindFileEncryptionKey(string password)
  {
    byte[] numArray1 = (byte[]) null;
    byte[] buffer = (byte[]) null;
    if (this.m_ownerRandomBytes != null)
    {
      byte[] destinationArray = new byte[8];
      byte[] numArray2 = new byte[8];
      byte[] bytes = Encoding.UTF8.GetBytes(password);
      byte[] numArray3 = new byte[48 /*0x30*/];
      Array.Copy((Array) this.m_userPasswordOut, 0, (Array) numArray3, 0, 48 /*0x30*/);
      Array.Copy((Array) this.m_ownerRandomBytes, 0, (Array) destinationArray, 0, 8);
      Array.Copy((Array) this.m_ownerRandomBytes, 8, (Array) numArray2, 0, 8);
      byte[] numArray4 = new byte[bytes.Length + destinationArray.Length + numArray3.Length];
      Array.Copy((Array) bytes, 0, (Array) numArray4, 0, bytes.Length);
      Array.Copy((Array) numArray2, 0, (Array) numArray4, bytes.Length, numArray2.Length);
      Array.Copy((Array) numArray3, 0, (Array) numArray4, bytes.Length + destinationArray.Length, numArray3.Length);
      numArray1 = this.HashComputer.ComputeHash(numArray4);
      buffer = this.m_ownerEncryptionKeyOut;
    }
    else if (this.m_userRandomBytes != null)
    {
      byte[] destinationArray = new byte[8];
      byte[] numArray5 = new byte[8];
      byte[] bytes = Encoding.UTF8.GetBytes(password);
      Array.Copy((Array) this.m_userRandomBytes, 0, (Array) destinationArray, 0, 8);
      Array.Copy((Array) this.m_userRandomBytes, 8, (Array) numArray5, 0, 8);
      byte[] numArray6 = new byte[bytes.Length + numArray5.Length];
      Array.Copy((Array) bytes, 0, (Array) numArray6, 0, bytes.Length);
      Array.Copy((Array) numArray5, 0, (Array) numArray6, bytes.Length, numArray5.Length);
      numArray1 = this.HashComputer.ComputeHash(numArray6);
      buffer = this.m_userEncryptionKeyOut;
    }
    Rijndael rijndael = Rijndael.Create();
    rijndael.Mode = CipherMode.CBC;
    rijndael.KeySize = 256 /*0x0100*/;
    rijndael.Key = numArray1;
    rijndael.IV = new byte[16 /*0x10*/];
    rijndael.Padding = PaddingMode.None;
    MemoryStream memoryStream = new MemoryStream();
    CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, rijndael.CreateDecryptor(), CryptoStreamMode.Write);
    cryptoStream.Write(buffer, 0, buffer.Length);
    cryptoStream.Close();
    memoryStream.Dispose();
    this.m_fileEncryptionKey = memoryStream.ToArray();
  }

  private void AcrobatXOwnerFileEncryptionKey(string password)
  {
    byte[] numArray1 = new byte[8];
    byte[] bytes = Encoding.UTF8.GetBytes(password);
    Array.Copy((Array) this.m_ownerPasswordOut, 40, (Array) numArray1, 0, 8);
    int length = 48 /*0x30*/;
    if (this.m_userPasswordOut.Length < 48 /*0x30*/)
      length = this.m_userPasswordOut.Length;
    byte[] numArray2 = new byte[bytes.Length + numArray1.Length + length];
    Array.Copy((Array) bytes, 0, (Array) numArray2, 0, bytes.Length);
    Array.Copy((Array) numArray1, 0, (Array) numArray2, bytes.Length, numArray1.Length);
    Array.Copy((Array) this.m_userPasswordOut, 0, (Array) numArray2, bytes.Length + numArray1.Length, length);
    byte[] numArray3 = this.AcrobatXComputeHash(numArray2, bytes, this.m_userPasswordOut);
    byte[] encryptionKeyOut = this.m_ownerEncryptionKeyOut;
    try
    {
      Rijndael rijndael = Rijndael.Create();
      rijndael.Mode = CipherMode.CBC;
      rijndael.KeySize = 256 /*0x0100*/;
      rijndael.Key = numArray3;
      rijndael.IV = new byte[16 /*0x10*/];
      rijndael.Padding = PaddingMode.None;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        new CryptoStream((Stream) memoryStream, rijndael.CreateDecryptor(rijndael.Key, rijndael.IV), CryptoStreamMode.Write).Write(encryptionKeyOut, 0, encryptionKeyOut.Length);
        this.m_fileEncryptionKey = memoryStream.ToArray();
      }
    }
    catch (Exception ex)
    {
      throw new Exception(ex.Message);
    }
  }

  private void AdvanceXUserFileEncryptionKey(string password)
  {
    byte[] numArray1 = new byte[8];
    Array.Copy((Array) this.m_userPasswordOut, 40, (Array) numArray1, 0, 8);
    byte[] bytes = Encoding.UTF8.GetBytes(password);
    byte[] numArray2 = new byte[bytes.Length + numArray1.Length];
    Array.Copy((Array) bytes, 0, (Array) numArray2, 0, bytes.Length);
    Array.Copy((Array) numArray1, 0, (Array) numArray2, bytes.Length, numArray1.Length);
    byte[] numArray3 = this.AcrobatXComputeHash(numArray2, bytes, (byte[]) null);
    byte[] encryptionKeyOut = this.m_userEncryptionKeyOut;
    try
    {
      Rijndael rijndael = Rijndael.Create();
      rijndael.Mode = CipherMode.CBC;
      rijndael.KeySize = 256 /*0x0100*/;
      rijndael.Key = numArray3;
      rijndael.IV = new byte[16 /*0x10*/];
      rijndael.Padding = PaddingMode.None;
      MemoryStream memoryStream = new MemoryStream();
      CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, rijndael.CreateDecryptor(rijndael.Key, rijndael.IV), CryptoStreamMode.Write);
      cryptoStream.Write(encryptionKeyOut, 0, encryptionKeyOut.Length);
      cryptoStream.Close();
      memoryStream.Dispose();
      this.m_fileEncryptionKey = memoryStream.ToArray();
    }
    catch (Exception ex)
    {
      throw new Exception(ex.Message);
    }
  }

  private byte[] GetKeyForOwnerPassStep7(byte[] originalKey, byte index)
  {
    byte[] forOwnerPassStep7 = originalKey != null ? new byte[originalKey.Length] : throw new ArgumentNullException(nameof (originalKey));
    int index1 = 0;
    for (int length = originalKey.Length; index1 < length; ++index1)
      forOwnerPassStep7[index1] = (byte) ((uint) originalKey[index1] ^ (uint) index);
    return forOwnerPassStep7;
  }

  private byte[] CreateEncryptionKey(string inputPass, byte[] ownerPass)
  {
    if (inputPass == null)
      throw new ArgumentNullException(nameof (inputPass));
    if (ownerPass == null)
      throw new ArgumentNullException(nameof (ownerPass));
    byte[] collection1 = this.PadTrancateString(inputPass);
    List<byte> byteList = new List<byte>();
    byteList.AddRange((IEnumerable<byte>) collection1);
    byteList.AddRange((IEnumerable<byte>) ownerPass);
    byte[] collection2 = new byte[4]
    {
      (byte) this.m_permissionValue,
      (byte) (this.m_permissionValue >> 8),
      (byte) (this.m_permissionValue >> 16 /*0x10*/),
      (byte) (this.m_permissionValue >> 24)
    };
    byteList.AddRange((IEnumerable<byte>) collection2);
    byteList.AddRange((IEnumerable<byte>) this.RandomBytes);
    if ((this.m_revision == 0 ? (int) (this.m_keyLength + 2) : this.RevisionNumber) > 3 && !this.EncryptMetaData)
    {
      byteList.Add(byte.MaxValue);
      byteList.Add(byte.MaxValue);
      byteList.Add(byte.MaxValue);
      byteList.Add(byte.MaxValue);
    }
    byte[] array = byteList.ToArray();
    byte[] numArray1;
    if (this.Provider != null)
    {
      numArray1 = this.Provider.ComputeHash(array);
      int keyLength = this.GetKeyLength();
      if (this.RevisionNumber > 2)
      {
        for (int index = 0; index < 50; ++index)
        {
          byte[] numArray2 = new byte[keyLength];
          Array.Copy((Array) numArray1, (Array) numArray2, numArray2.Length);
          numArray1 = this.Provider.ComputeHash(numArray2);
        }
      }
    }
    else
    {
      this.MD5.Reset();
      this.MD5.Update(array, 0, array.Length);
      numArray1 = new byte[this.MD5.MessageDigestSize];
      this.MD5.DoFinal(numArray1, 0);
      byte[] destinationArray = new byte[this.GetKeyLength()];
      Array.Copy((Array) numArray1, (Array) destinationArray, destinationArray.Length);
      this.MD5.Reset();
      if (this.RevisionNumber > 2)
      {
        MessageDigestAlgorithms digestAlgorithms = new MessageDigestAlgorithms();
        byte[] numArray3 = new byte[this.GetKeyLength()];
        IMessageDigest digest = new MessageDigestFinder().GetDigest("MD5");
        for (int index = 0; index < 50; ++index)
          numArray1 = digestAlgorithms.Digest(digest, numArray1, 0, numArray3.Length);
      }
    }
    this.EncryptionKey = new byte[this.GetKeyLength()];
    Array.Copy((Array) numArray1, (Array) this.EncryptionKey, this.EncryptionKey.Length);
    return this.EncryptionKey;
  }

  private void CreateFileEncryptionKey()
  {
    this.m_fileEncryptionKey = new byte[32 /*0x20*/];
    this.m_randomArray.NextBytes(this.m_fileEncryptionKey);
  }

  private byte[] CreateUserPassword()
  {
    return this.RevisionNumber != 2 ? this.Create128BitUserPassword() : this.Create40BitUserPassword();
  }

  private byte[] Create256BitUserPassword()
  {
    byte[] numArray1 = new byte[8];
    byte[] numArray2 = new byte[8];
    this.m_userRandomBytes = new byte[16 /*0x10*/];
    this.m_randomArray.NextBytes(this.m_userRandomBytes);
    byte[] bytes = Encoding.UTF8.GetBytes(this.m_userPassword);
    Array.Copy((Array) this.m_userRandomBytes, 0, (Array) numArray1, 0, 8);
    Array.Copy((Array) this.m_userRandomBytes, 8, (Array) numArray2, 0, 8);
    byte[] numArray3 = new byte[this.UserPassword.Length + numArray1.Length];
    Array.Copy((Array) bytes, 0, (Array) numArray3, 0, bytes.Length);
    Array.Copy((Array) numArray1, 0, (Array) numArray3, this.UserPassword.Length, numArray1.Length);
    byte[] numArray4;
    if (this.HashComputer != null)
    {
      numArray4 = this.HashComputer.ComputeHash(numArray3);
    }
    else
    {
      numArray4 = new byte[32 /*0x20*/];
      IMessageDigest digest = new MessageDigestFinder().GetDigest("SHA256");
      digest.Update(numArray3, 0, numArray3.Length);
      digest.DoFinal(numArray4, 0);
    }
    byte[] destinationArray = new byte[numArray4.Length + numArray1.Length + numArray2.Length];
    Array.Copy((Array) numArray4, 0, (Array) destinationArray, 0, numArray4.Length);
    Array.Copy((Array) numArray1, 0, (Array) destinationArray, numArray4.Length, numArray1.Length);
    Array.Copy((Array) numArray2, 0, (Array) destinationArray, numArray4.Length + numArray1.Length, numArray2.Length);
    return destinationArray;
  }

  private void CreateAcrobatX256BitUserPassword()
  {
    byte[] numArray1 = new byte[8];
    byte[] numArray2 = new byte[8];
    byte[] bytes = Encoding.UTF8.GetBytes(this.m_userPassword);
    this.m_randomArray.NextBytes(numArray1);
    this.m_randomArray.NextBytes(numArray2);
    byte[] numArray3 = new byte[bytes.Length + numArray2.Length];
    Array.Copy((Array) bytes, 0, (Array) numArray3, 0, bytes.Length);
    Array.Copy((Array) numArray1, 0, (Array) numArray3, bytes.Length, numArray1.Length);
    byte[] sourceArray = this.AcrobatXComputeHash(numArray3, bytes, (byte[]) null);
    this.m_userPasswordOut = new byte[sourceArray.Length + numArray1.Length + numArray2.Length];
    Array.Copy((Array) sourceArray, 0, (Array) this.m_userPasswordOut, 0, sourceArray.Length);
    Array.Copy((Array) numArray1, 0, (Array) this.m_userPasswordOut, sourceArray.Length, numArray1.Length);
    Array.Copy((Array) numArray2, 0, (Array) this.m_userPasswordOut, sourceArray.Length + numArray1.Length, numArray2.Length);
    byte[] numArray4 = new byte[bytes.Length + numArray2.Length];
    Array.Copy((Array) bytes, 0, (Array) numArray4, 0, bytes.Length);
    Array.Copy((Array) numArray2, 0, (Array) numArray4, bytes.Length, numArray2.Length);
    byte[] key = this.AcrobatXComputeHash(numArray4, bytes, (byte[]) null);
    try
    {
      Rijndael rijndael = Rijndael.Create();
      rijndael.Mode = CipherMode.CBC;
      rijndael.KeySize = 256 /*0x0100*/;
      rijndael.Key = key;
      rijndael.IV = new byte[16 /*0x10*/];
      rijndael.Padding = PaddingMode.None;
      MemoryStream memoryStream = new MemoryStream();
      CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, rijndael.CreateEncryptor(), CryptoStreamMode.Write);
      cryptoStream.Write(this.m_fileEncryptionKey, 0, this.m_fileEncryptionKey.Length);
      cryptoStream.Close();
      memoryStream.Dispose();
      this.m_userEncryptionKeyOut = memoryStream.ToArray();
    }
    catch
    {
      this.m_userEncryptionKeyOut = new AesCipherNoPadding(true, key).ProcessBlock(this.m_fileEncryptionKey, 0, this.m_fileEncryptionKey.Length);
    }
  }

  private byte[] CreateUserEncryptionKey()
  {
    byte[] destinationArray = new byte[8];
    byte[] numArray1 = new byte[8];
    byte[] bytes = Encoding.UTF8.GetBytes(this.m_userPassword);
    Array.Copy((Array) this.m_userRandomBytes, 0, (Array) destinationArray, 0, 8);
    Array.Copy((Array) this.m_userRandomBytes, 8, (Array) numArray1, 0, 8);
    byte[] numArray2 = new byte[bytes.Length + numArray1.Length];
    Array.Copy((Array) bytes, 0, (Array) numArray2, 0, bytes.Length);
    Array.Copy((Array) numArray1, 0, (Array) numArray2, bytes.Length, numArray1.Length);
    if (this.HashComputer != null)
    {
      byte[] hash = this.HashComputer.ComputeHash(numArray2);
      Rijndael rijndael = Rijndael.Create();
      rijndael.Mode = CipherMode.CBC;
      rijndael.KeySize = 256 /*0x0100*/;
      rijndael.Key = hash;
      rijndael.IV = new byte[16 /*0x10*/];
      rijndael.Padding = PaddingMode.None;
      MemoryStream memoryStream = new MemoryStream();
      CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, rijndael.CreateEncryptor(), CryptoStreamMode.Write);
      cryptoStream.Write(this.m_fileEncryptionKey, 0, this.m_fileEncryptionKey.Length);
      cryptoStream.Close();
      memoryStream.Dispose();
      return memoryStream.ToArray();
    }
    byte[] numArray3 = new byte[32 /*0x20*/];
    IMessageDigest digest = new MessageDigestFinder().GetDigest("SHA256");
    digest.Update(numArray2, 0, numArray2.Length);
    digest.DoFinal(numArray3, 0);
    return new AesCipherNoPadding(true, numArray3).ProcessBlock(this.m_fileEncryptionKey, 0, this.m_fileEncryptionKey.Length);
  }

  private byte[] CreatePermissionFlag()
  {
    byte[] numArray1 = new byte[16 /*0x10*/];
    byte[] sourceArray = new byte[4]
    {
      (byte) this.m_permissionValue,
      (byte) (this.m_permissionValue >> 8),
      (byte) (this.m_permissionValue >> 16 /*0x10*/),
      (byte) (this.m_permissionValue >> 24)
    };
    Array.Copy((Array) sourceArray, 0, (Array) numArray1, 0, sourceArray.Length);
    int length = sourceArray.Length;
    byte[] numArray2 = numArray1;
    int index1 = length;
    int num1 = index1 + 1;
    numArray2[index1] = byte.MaxValue;
    byte[] numArray3 = numArray1;
    int index2 = num1;
    int num2 = index2 + 1;
    numArray3[index2] = byte.MaxValue;
    byte[] numArray4 = numArray1;
    int index3 = num2;
    int num3 = index3 + 1;
    numArray4[index3] = byte.MaxValue;
    byte[] numArray5 = numArray1;
    int index4 = num3;
    int num4 = index4 + 1;
    numArray5[index4] = byte.MaxValue;
    int num5;
    if (this.EncryptMetaData)
    {
      byte[] numArray6 = numArray1;
      int index5 = num4;
      num5 = index5 + 1;
      numArray6[index5] = (byte) 84;
    }
    else
    {
      byte[] numArray7 = numArray1;
      int index6 = num4;
      num5 = index6 + 1;
      numArray7[index6] = (byte) 70;
    }
    byte[] numArray8 = numArray1;
    int index7 = num5;
    int num6 = index7 + 1;
    numArray8[index7] = (byte) 97;
    byte[] numArray9 = numArray1;
    int index8 = num6;
    int num7 = index8 + 1;
    numArray9[index8] = (byte) 100;
    byte[] numArray10 = numArray1;
    int index9 = num7;
    int num8 = index9 + 1;
    numArray10[index9] = (byte) 98;
    byte[] numArray11 = numArray1;
    int index10 = num8;
    int num9 = index10 + 1;
    numArray11[index10] = (byte) 98;
    byte[] numArray12 = numArray1;
    int index11 = num9;
    int num10 = index11 + 1;
    numArray12[index11] = (byte) 98;
    byte[] numArray13 = numArray1;
    int index12 = num10;
    int num11 = index12 + 1;
    numArray13[index12] = (byte) 98;
    byte[] numArray14 = numArray1;
    int index13 = num11;
    int num12 = index13 + 1;
    numArray14[index13] = (byte) 98;
    try
    {
      Rijndael rijndael = Rijndael.Create();
      rijndael.Mode = CipherMode.ECB;
      rijndael.KeySize = 256 /*0x0100*/;
      rijndael.Key = this.m_fileEncryptionKey;
      rijndael.IV = new byte[16 /*0x10*/];
      rijndael.Padding = PaddingMode.None;
      MemoryStream memoryStream = new MemoryStream();
      CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, rijndael.CreateEncryptor(), CryptoStreamMode.Write);
      cryptoStream.Write(numArray1, 0, numArray1.Length);
      cryptoStream.Close();
      memoryStream.Close();
      return memoryStream.ToArray();
    }
    catch
    {
      return new AesCipherNoPadding(true, this.m_fileEncryptionKey).ProcessBlock(numArray1, 0, numArray1.Length);
    }
  }

  private byte[] Create40BitUserPassword()
  {
    return this.EncryptionKey != null ? this.EncryptDataByCustom(this.PadTrancateString(string.Empty), this.EncryptionKey) : throw new ArgumentNullException("EncryptionKey");
  }

  private byte[] Create128BitUserPassword()
  {
    if (this.EncryptionKey == null)
      throw new ArgumentNullException("EncryptionKey");
    List<byte> byteList = new List<byte>();
    byte[] collection = this.PadTrancateString(string.Empty);
    byteList.AddRange((IEnumerable<byte>) collection);
    byteList.AddRange((IEnumerable<byte>) this.RandomBytes);
    byte[] array = byteList.ToArray();
    byte[] numArray1;
    if (this.Provider != null)
    {
      numArray1 = this.Provider.ComputeHash(array);
    }
    else
    {
      this.MD5.Update(array, 0, array.Length);
      numArray1 = new byte[this.MD5.MessageDigestSize];
      this.MD5.DoFinal(numArray1, 0);
      this.MD5.Reset();
    }
    byte[] numArray2 = new byte[16 /*0x10*/];
    Array.Copy((Array) numArray1, 0, (Array) numArray2, 0, numArray2.Length);
    byte[] numArray3 = this.EncryptDataByCustom(numArray2, this.EncryptionKey);
    byte[] encryptionKey = this.EncryptionKey;
    for (byte index = 1; index < (byte) 20; ++index)
    {
      byte[] forOwnerPassStep7 = this.GetKeyForOwnerPassStep7(this.EncryptionKey, index);
      numArray3 = this.EncryptDataByCustom(numArray3, forOwnerPassStep7, forOwnerPassStep7.Length);
    }
    return this.PadTrancateString(numArray3);
  }

  private void InitializeData()
  {
    if (this.m_hasComputedPasswordValues)
      return;
    if (this.CryptographicAlgorithm == PdfEncryptionKeySize.Key256Bit)
    {
      this.m_userPasswordOut = this.Create256BitUserPassword();
      this.m_ownerPasswordOut = this.Create256BitOwnerPassword();
      this.CreateFileEncryptionKey();
      this.m_userEncryptionKeyOut = this.CreateUserEncryptionKey();
      this.m_ownerEncryptionKeyOut = this.CreateOwnerEncryptionKey();
      this.m_permissionFlag = this.CreatePermissionFlag();
    }
    else if (this.CryptographicAlgorithm == PdfEncryptionKeySize.Key256BitRevision6)
    {
      this.CreateFileEncryptionKey();
      this.CreateAcrobatX256BitUserPassword();
      this.CreateAcrobatX256BitOwnerPassword();
      this.m_permissionFlag = this.CreatePermissionFlag();
    }
    else
    {
      this.m_ownerPasswordOut = this.CreateOwnerPassword();
      this.m_encryptionKey = this.CreateEncryptionKey(this.UserPassword, this.m_ownerPasswordOut);
      this.m_userPasswordOut = this.CreateUserPassword();
    }
    this.m_hasComputedPasswordValues = true;
  }

  private byte[] PrepareKeyForEncryption(byte[] originalKey)
  {
    int num = originalKey != null ? originalKey.Length : throw new ArgumentNullException(nameof (originalKey));
    byte[] numArray;
    if (this.Provider != null)
    {
      numArray = this.Provider.ComputeHash(originalKey);
    }
    else
    {
      this.MD5.Reset();
      this.MD5.Update(originalKey, 0, originalKey.Length);
      numArray = new byte[this.MD5.MessageDigestSize];
      this.MD5.DoFinal(numArray, 0);
      this.MD5.Reset();
    }
    byte[] destinationArray = numArray;
    if (num > 16 /*0x10*/)
    {
      int length = Math.Min(this.GetKeyLength() + 5, 16 /*0x10*/);
      destinationArray = new byte[length];
      Array.Copy((Array) numArray, 0, (Array) destinationArray, 0, length);
    }
    return destinationArray;
  }

  private bool AuthenticateUserPassword(string password)
  {
    if (this.m_keyLength == PdfEncryptionKeySize.Key256Bit || this.m_keyLength == PdfEncryptionKeySize.Key256BitRevision6)
      return this.Authenticate256BitUserPassword(password);
    this.m_encryptionKey = this.CreateEncryptionKey(password, this.m_ownerPasswordOut);
    byte[] userPassword = this.CreateUserPassword();
    return this.RevisionNumber != 2 ? this.CompareByteArrays(userPassword, this.m_userPasswordOut, 16 /*0x10*/) : this.CompareByteArrays(userPassword, this.m_userPasswordOut);
  }

  private bool Authenticate256BitUserPassword(string password)
  {
    byte[] numArray1 = new byte[8];
    byte[] destinationArray = new byte[8];
    byte[] numArray2 = new byte[32 /*0x20*/];
    this.m_userRandomBytes = new byte[16 /*0x10*/];
    if (this.m_keyLength == PdfEncryptionKeySize.Key256BitRevision6)
    {
      byte[] bytes = Encoding.UTF8.GetBytes(password);
      Array.Copy((Array) this.m_userPasswordOut, 0, (Array) numArray2, 0, 32 /*0x20*/);
      Array.Copy((Array) this.m_userPasswordOut, 32 /*0x20*/, (Array) numArray1, 0, 8);
      byte[] numArray3 = new byte[bytes.Length + numArray1.Length];
      Array.Copy((Array) bytes, 0, (Array) numArray3, 0, bytes.Length);
      Array.Copy((Array) numArray1, 0, (Array) numArray3, bytes.Length, numArray1.Length);
      byte[] array1 = this.AcrobatXComputeHash(numArray3, bytes, (byte[]) null);
      this.AdvanceXUserFileEncryptionKey(password);
      return this.CompareByteArrays(array1, numArray2);
    }
    byte[] bytes1 = Encoding.UTF8.GetBytes(password);
    Array.Copy((Array) this.m_userPasswordOut, 0, (Array) numArray2, 0, numArray2.Length);
    Array.Copy((Array) this.m_userPasswordOut, 32 /*0x20*/, (Array) this.m_userRandomBytes, 0, 16 /*0x10*/);
    Array.Copy((Array) this.m_userRandomBytes, 0, (Array) numArray1, 0, numArray1.Length);
    Array.Copy((Array) this.m_userRandomBytes, numArray1.Length, (Array) destinationArray, 0, destinationArray.Length);
    byte[] numArray4 = new byte[bytes1.Length + numArray1.Length];
    Array.Copy((Array) bytes1, 0, (Array) numArray4, 0, bytes1.Length);
    Array.Copy((Array) numArray1, 0, (Array) numArray4, bytes1.Length, numArray1.Length);
    byte[] numArray5 = new byte[32 /*0x20*/];
    byte[] hash = this.HashComputer.ComputeHash(numArray4);
    bool flag = false;
    if (hash.Length == numArray2.Length)
    {
      int index = 0;
      while (index < hash.Length && (int) hash[index] == (int) numArray2[index])
        ++index;
      if (index == hash.Length)
        flag = true;
    }
    if (flag)
      this.FindFileEncryptionKey(password);
    return flag;
  }

  private bool AuthenticateOwnerPassword(string password)
  {
    if (this.m_keyLength == PdfEncryptionKeySize.Key256Bit || this.m_keyLength == PdfEncryptionKeySize.Key256BitRevision6)
      return this.Authenticate256BitOwnerPassword(password);
    this.m_encryptionKey = this.GetKeyFromOwnerPass(password);
    byte[] numArray = this.m_ownerPasswordOut;
    if (this.RevisionNumber == 2)
      numArray = this.EncryptDataByCustom(numArray, this.m_encryptionKey);
    else if (this.RevisionNumber > 2)
    {
      numArray = this.m_ownerPasswordOut;
      for (int index = 0; index < 20; ++index)
      {
        byte[] forOwnerPassStep7 = this.GetKeyForOwnerPassStep7(this.m_encryptionKey, (byte) (20 - index - 1));
        numArray = this.EncryptDataByCustom(numArray, forOwnerPassStep7);
      }
    }
    this.m_encryptionKey = (byte[]) null;
    string password1 = this.ConvertToPassword(numArray);
    if (!this.AuthenticateUserPassword(password1))
      return false;
    this.m_userPassword = password1;
    this.m_ownerPassword = password;
    return true;
  }

  private bool Authenticate256BitOwnerPassword(string password)
  {
    byte[] numArray1 = new byte[8];
    byte[] destinationArray = new byte[8];
    byte[] numArray2 = new byte[32 /*0x20*/];
    this.m_ownerRandomBytes = new byte[16 /*0x10*/];
    if (this.m_keyLength == PdfEncryptionKeySize.Key256BitRevision6)
    {
      byte[] bytes = Encoding.UTF8.GetBytes(password);
      Array.Copy((Array) this.m_ownerPasswordOut, 0, (Array) numArray2, 0, 32 /*0x20*/);
      Array.Copy((Array) this.m_ownerPasswordOut, 32 /*0x20*/, (Array) numArray1, 0, 8);
      int length = 48 /*0x30*/;
      if (this.m_userPasswordOut.Length < 48 /*0x30*/)
        length = this.m_userPasswordOut.Length;
      byte[] numArray3 = new byte[bytes.Length + numArray1.Length + length];
      Array.Copy((Array) bytes, 0, (Array) numArray3, 0, bytes.Length);
      Array.Copy((Array) numArray1, 0, (Array) numArray3, bytes.Length, numArray1.Length);
      Array.Copy((Array) this.m_userPasswordOut, 0, (Array) numArray3, bytes.Length + numArray1.Length, length);
      byte[] array1 = this.AcrobatXComputeHash(numArray3, bytes, this.m_userPasswordOut);
      this.AcrobatXOwnerFileEncryptionKey(password);
      bool flag = this.CompareByteArrays(array1, numArray2);
      if (flag)
      {
        byte[] fileEncryptionKey = this.m_fileEncryptionKey;
        string password1 = password;
        this.m_ownerRandomBytes = (byte[]) null;
        if (this.AuthenticateUserPassword(password1))
        {
          this.m_userPassword = password1;
          this.m_ownerPassword = password;
        }
        else
          this.m_fileEncryptionKey = fileEncryptionKey;
      }
      else
        this.m_ownerRandomBytes = (byte[]) null;
      return flag;
    }
    byte[] numArray4 = new byte[48 /*0x30*/];
    Array.Copy((Array) this.m_userPasswordOut, 0, (Array) numArray4, 0, 48 /*0x30*/);
    byte[] bytes1 = Encoding.UTF8.GetBytes(password);
    Array.Copy((Array) this.m_ownerPasswordOut, 0, (Array) numArray2, 0, numArray2.Length);
    Array.Copy((Array) this.m_ownerPasswordOut, 32 /*0x20*/, (Array) this.m_ownerRandomBytes, 0, 16 /*0x10*/);
    Array.Copy((Array) this.m_ownerRandomBytes, 0, (Array) numArray1, 0, numArray1.Length);
    Array.Copy((Array) this.m_ownerRandomBytes, numArray1.Length, (Array) destinationArray, 0, destinationArray.Length);
    byte[] numArray5 = new byte[bytes1.Length + numArray1.Length + numArray4.Length];
    Array.Copy((Array) bytes1, 0, (Array) numArray5, 0, bytes1.Length);
    Array.Copy((Array) numArray1, 0, (Array) numArray5, bytes1.Length, numArray1.Length);
    Array.Copy((Array) numArray4, 0, (Array) numArray5, bytes1.Length + numArray1.Length, numArray4.Length);
    byte[] hash = this.HashComputer.ComputeHash(numArray5);
    bool flag1 = false;
    if (hash.Length == numArray2.Length)
    {
      int index = 0;
      while (index < hash.Length && (int) hash[index] == (int) numArray2[index])
        ++index;
      if (index == hash.Length)
        flag1 = true;
    }
    this.FindFileEncryptionKey(password);
    if (flag1)
    {
      this.m_ownerRandomBytes = (byte[]) null;
      string password2 = password;
      if (this.AuthenticateUserPassword(password2))
      {
        this.m_userPassword = password2;
        this.m_ownerPassword = password;
      }
    }
    else
      this.m_ownerRandomBytes = (byte[]) null;
    return flag1;
  }

  private string ConvertToPassword(byte[] array)
  {
    int length = array.Length;
    for (int index = 0; index < length; ++index)
    {
      if ((int) array[index] == (int) PdfEncryptor.s_paddingString[0] && index < length - 1 && (int) array[index + 1] == (int) PdfEncryptor.s_paddingString[1])
      {
        length = index;
        break;
      }
    }
    return PdfString.ByteToString(array, length);
  }

  private bool CompareByteArrays(byte[] array1, byte[] array2)
  {
    bool flag = true;
    if (array1 == null || array2 == null)
      flag = array1 == array2;
    else if (array1.Length != array2.Length)
    {
      flag = false;
    }
    else
    {
      int index = 0;
      for (int length = array1.Length; index < length; ++index)
      {
        if ((int) array1[index] != (int) array2[index])
        {
          flag = false;
          break;
        }
      }
    }
    return flag;
  }

  private bool CompareByteArrays(byte[] array1, byte[] array2, int size)
  {
    bool flag = true;
    if (array1 == null || array2 == null)
    {
      flag = array1 == array2;
    }
    else
    {
      if (array1.Length < size || array2.Length < size)
        throw new ArgumentException("Size of one of the arrays are less then requisted size.");
      if (array1.Length != array2.Length)
      {
        flag = false;
      }
      else
      {
        for (int index = 0; index < size; ++index)
        {
          if ((int) array1[index] != (int) array2[index])
          {
            flag = false;
            break;
          }
        }
      }
    }
    return flag;
  }

  private PdfDictionary AESDictionary()
  {
    PdfDictionary pdfDictionary1 = new PdfDictionary();
    PdfDictionary pdfDictionary2 = new PdfDictionary();
    if (!pdfDictionary2.ContainsKey(new PdfName("CFM")))
    {
      if (this.m_encryptOnlyAttachment)
      {
        pdfDictionary2[new PdfName("Type")] = (IPdfPrimitive) new PdfName("CryptFilter");
        pdfDictionary2[new PdfName("CFM")] = (IPdfPrimitive) new PdfName("AESV2");
      }
      else if (this.CryptographicAlgorithm == PdfEncryptionKeySize.Key256Bit)
        pdfDictionary2[new PdfName("CFM")] = (IPdfPrimitive) new PdfName("AESV3");
      else if (this.EncryptionAlgorithm == PdfEncryptionAlgorithm.RC4 && this.CryptographicAlgorithm == PdfEncryptionKeySize.Key128Bit)
        pdfDictionary2[new PdfName("CFM")] = (IPdfPrimitive) new PdfName("V2");
      else
        pdfDictionary2[new PdfName("CFM")] = (IPdfPrimitive) new PdfName("AESV2");
    }
    if (!pdfDictionary2.ContainsKey(new PdfName("AuthEvent")))
    {
      if (this.m_encryptOnlyAttachment)
        pdfDictionary2[new PdfName("AuthEvent")] = (IPdfPrimitive) new PdfName("EFOpen");
      else
        pdfDictionary2[new PdfName("AuthEvent")] = (IPdfPrimitive) new PdfName("DocOpen");
    }
    if (!pdfDictionary2.ContainsKey(new PdfName("Length")))
    {
      if (this.CryptographicAlgorithm == PdfEncryptionKeySize.Key256Bit)
        pdfDictionary2[new PdfName("Length")] = (IPdfPrimitive) new PdfNumber(32 /*0x20*/);
      else if (this.CryptographicAlgorithm == PdfEncryptionKeySize.Key128Bit)
        pdfDictionary2[new PdfName("Length")] = (IPdfPrimitive) new PdfNumber(16 /*0x10*/);
      else
        pdfDictionary2[new PdfName("Length")] = (IPdfPrimitive) new PdfNumber(128 /*0x80*/);
    }
    if (!pdfDictionary1.ContainsKey(new PdfName("StdCF")))
      pdfDictionary1[new PdfName("StdCF")] = (IPdfPrimitive) pdfDictionary2;
    return pdfDictionary1;
  }
}
