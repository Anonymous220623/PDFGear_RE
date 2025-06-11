// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.PasswordUtility
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class PasswordUtility
{
  private const string m_pkcs12 = "Pkcs12";
  private readonly IDictionary m_algorithms = (IDictionary) new Hashtable();
  private readonly IDictionary m_type = (IDictionary) new Hashtable();
  private readonly IDictionary m_ids = (IDictionary) new Hashtable();

  internal PasswordUtility()
  {
    this.m_algorithms[(object) "PBEWITHSHAAND40BITRC4"] = (object) "PBEwithSHA-1and40bitRC4";
    this.m_algorithms[(object) "PBEWITHSHA1AND40BITRC4"] = (object) "PBEwithSHA-1and40bitRC4";
    this.m_algorithms[(object) "PBEWITHSHA-1AND40BITRC4"] = (object) "PBEwithSHA-1and40bitRC4";
    this.m_algorithms[(object) PKCSOIDs.PbeWithShaAnd40BitRC4.ID] = (object) "PBEwithSHA-1and40bitRC4";
    this.m_algorithms[(object) "PBEWITHSHAAND3-KEYDESEDE-CBC"] = (object) "PBEwithSHA-1and3-keyDESEDE-CBC";
    this.m_algorithms[(object) "PBEWITHSHAAND3-KEYTRIPLEDES-CBC"] = (object) "PBEwithSHA-1and3-keyDESEDE-CBC";
    this.m_algorithms[(object) "PBEWITHSHA1AND3-KEYDESEDE-CBC"] = (object) "PBEwithSHA-1and3-keyDESEDE-CBC";
    this.m_algorithms[(object) "PBEWITHSHA1AND3-KEYTRIPLEDES-CBC"] = (object) "PBEwithSHA-1and3-keyDESEDE-CBC";
    this.m_algorithms[(object) "PBEWITHSHA-1AND3-KEYDESEDE-CBC"] = (object) "PBEwithSHA-1and3-keyDESEDE-CBC";
    this.m_algorithms[(object) "PBEWITHSHA-1AND3-KEYTRIPLEDES-CBC"] = (object) "PBEwithSHA-1and3-keyDESEDE-CBC";
    this.m_algorithms[(object) PKCSOIDs.PbeWithShaAnd3KeyTripleDesCbc.ID] = (object) "PBEwithSHA-1and3-keyDESEDE-CBC";
    this.m_algorithms[(object) "PBEWITHSHAAND40BITRC2-CBC"] = (object) "PBEwithSHA-1and40bitRC2-CBC";
    this.m_algorithms[(object) "PBEWITHSHA1AND40BITRC2-CBC"] = (object) "PBEwithSHA-1and40bitRC2-CBC";
    this.m_algorithms[(object) "PBEWITHSHA-1AND40BITRC2-CBC"] = (object) "PBEwithSHA-1and40bitRC2-CBC";
    this.m_algorithms[(object) PKCSOIDs.PbewithShaAnd40BitRC2Cbc.ID] = (object) "PBEwithSHA-1and40bitRC2-CBC";
    this.m_algorithms[(object) "PBEWITHSHAAND128BITAES-CBC-BC"] = (object) "PBEwithSHA-1and128bitAES-CBC-BC";
    this.m_algorithms[(object) "PBEWITHSHA1AND128BITAES-CBC-BC"] = (object) "PBEwithSHA-1and128bitAES-CBC-BC";
    this.m_algorithms[(object) "PBEWITHSHA-1AND128BITAES-CBC-BC"] = (object) "PBEwithSHA-1and128bitAES-CBC-BC";
    this.m_algorithms[(object) "PBEWITHSHAAND192BITAES-CBC-BC"] = (object) "PBEwithSHA-1and192bitAES-CBC-BC";
    this.m_algorithms[(object) "PBEWITHSHA1AND192BITAES-CBC-BC"] = (object) "PBEwithSHA-1and192bitAES-CBC-BC";
    this.m_algorithms[(object) "PBEWITHSHA-1AND192BITAES-CBC-BC"] = (object) "PBEwithSHA-1and192bitAES-CBC-BC";
    this.m_algorithms[(object) "PBEWITHSHAAND256BITAES-CBC-BC"] = (object) "PBEwithSHA-1and256bitAES-CBC-BC";
    this.m_algorithms[(object) "PBEWITHSHA1AND256BITAES-CBC-BC"] = (object) "PBEwithSHA-1and256bitAES-CBC-BC";
    this.m_algorithms[(object) "PBEWITHSHA-1AND256BITAES-CBC-BC"] = (object) "PBEwithSHA-1and256bitAES-CBC-BC";
    this.m_algorithms[(object) "PBEWITHSHA256AND128BITAES-CBC-BC"] = (object) "PBEwithSHA-256and128bitAES-CBC-BC";
    this.m_algorithms[(object) "PBEWITHSHA-256AND128BITAES-CBC-BC"] = (object) "PBEwithSHA-256and128bitAES-CBC-BC";
    this.m_algorithms[(object) "PBEWITHSHA256AND192BITAES-CBC-BC"] = (object) "PBEwithSHA-256and192bitAES-CBC-BC";
    this.m_algorithms[(object) "PBEWITHSHA-256AND192BITAES-CBC-BC"] = (object) "PBEwithSHA-256and192bitAES-CBC-BC";
    this.m_algorithms[(object) "PBEWITHSHA256AND256BITAES-CBC-BC"] = (object) "PBEwithSHA-256and256bitAES-CBC-BC";
    this.m_algorithms[(object) "PBEWITHSHA-256AND256BITAES-CBC-BC"] = (object) "PBEwithSHA-256and256bitAES-CBC-BC";
    this.m_type[(object) "Pkcs12"] = (object) "Pkcs12";
    this.m_type[(object) "PBEwithSHA-1and128bitRC4"] = (object) "Pkcs12";
    this.m_type[(object) "PBEwithSHA-1and40bitRC4"] = (object) "Pkcs12";
    this.m_type[(object) "PBEwithSHA-1and3-keyDESEDE-CBC"] = (object) "Pkcs12";
    this.m_type[(object) "PBEwithSHA-1and2-keyDESEDE-CBC"] = (object) "Pkcs12";
    this.m_type[(object) "PBEwithSHA-1and128bitRC2-CBC"] = (object) "Pkcs12";
    this.m_type[(object) "PBEwithSHA-1and40bitRC2-CBC"] = (object) "Pkcs12";
    this.m_type[(object) "PBEwithSHA-1and256bitAES-CBC-BC"] = (object) "Pkcs12";
    this.m_type[(object) "PBEwithSHA-256and128bitAES-CBC-BC"] = (object) "Pkcs12";
    this.m_type[(object) "PBEwithSHA-256and192bitAES-CBC-BC"] = (object) "Pkcs12";
    this.m_type[(object) "PBEwithSHA-256and256bitAES-CBC-BC"] = (object) "Pkcs12";
    this.m_ids[(object) "PBEwithSHA-1and128bitRC4"] = (object) PKCSOIDs.PbeWithShaAnd128BitRC4;
    this.m_ids[(object) "PBEwithSHA-1and40bitRC4"] = (object) PKCSOIDs.PbeWithShaAnd40BitRC4;
    this.m_ids[(object) "PBEwithSHA-1and3-keyDESEDE-CBC"] = (object) PKCSOIDs.PbeWithShaAnd3KeyTripleDesCbc;
    this.m_ids[(object) "PBEwithSHA-1and2-keyDESEDE-CBC"] = (object) PKCSOIDs.PbeWithShaAnd2KeyTripleDesCbc;
    this.m_ids[(object) "PBEwithSHA-1and128bitRC2-CBC"] = (object) PKCSOIDs.PbeWithShaAnd128BitRC2Cbc;
    this.m_ids[(object) "PBEwithSHA-1and40bitRC2-CBC"] = (object) PKCSOIDs.PbewithShaAnd40BitRC2Cbc;
  }

  internal PasswordGenerator GetEncoder(
    string type,
    IMessageDigest digest,
    byte[] key,
    byte[] salt,
    int iterationCount)
  {
    if (!type.Equals("Pkcs12"))
      throw new ArgumentException("Invalid Password Based Encryption type: " + type, nameof (type));
    PasswordGenerator encoder = (PasswordGenerator) new PKCS12AlgorithmGenerator(digest);
    encoder.Init(key, salt, iterationCount);
    return encoder;
  }

  internal bool IsPkcs12(string algorithm)
  {
    string algorithm1 = (string) this.m_algorithms[(object) algorithm.ToUpperInvariant()];
    return algorithm1 != null && "Pkcs12".Equals(this.m_type[(object) algorithm1]);
  }

  internal ICipherParam GenerateCipherParameters(
    DerObjectID algorithmOid,
    char[] password,
    bool isWrong,
    Asn1Encode parameters)
  {
    return this.GenerateCipherParameters(algorithmOid.ID, password, isWrong, parameters);
  }

  internal ICipherParam GenerateCipherParameters(Algorithms algID, char[] password, bool isWrong)
  {
    return this.GenerateCipherParameters(algID.ObjectID.ID, password, isWrong, algID.Parameters);
  }

  internal ICipherParam GenerateCipherParameters(
    string algorithm,
    char[] password,
    bool isWrong,
    Asn1Encode pbeParameters)
  {
    string algorithm1 = (string) this.m_algorithms[(object) algorithm.ToUpperInvariant()];
    byte[] key = (byte[]) null;
    byte[] salt = (byte[]) null;
    int iterationCount = 0;
    if (this.IsPkcs12(algorithm1))
    {
      PKCS12PasswordParameter pbeParameter = PKCS12PasswordParameter.GetPBEParameter((object) pbeParameters);
      salt = pbeParameter.Octets;
      iterationCount = pbeParameter.Iterations.IntValue;
      key = PasswordGenerator.ToBytes(password, isWrong);
    }
    ICipherParam parameters = (ICipherParam) null;
    if (algorithm1.StartsWith("PBEwithSHA-1"))
    {
      PasswordGenerator encoder = this.GetEncoder((string) this.m_type[(object) algorithm1], (IMessageDigest) new SHA1MessageDigest(), key, salt, iterationCount);
      switch (algorithm1)
      {
        case "PBEwithSHA-1and128bitAES-CBC-BC":
          parameters = encoder.GenerateParam("AES", 128 /*0x80*/, 128 /*0x80*/);
          break;
        case "PBEwithSHA-1and192bitAES-CBC-BC":
          parameters = encoder.GenerateParam("AES", 192 /*0xC0*/, 128 /*0x80*/);
          break;
        case "PBEwithSHA-1and256bitAES-CBC-BC":
          parameters = encoder.GenerateParam("AES", 256 /*0x0100*/, 128 /*0x80*/);
          break;
        case "PBEwithSHA-1and128bitRC4":
          parameters = encoder.GenerateParam("RC4", 128 /*0x80*/);
          break;
        case "PBEwithSHA-1and40bitRC4":
          parameters = encoder.GenerateParam("RC4", 40);
          break;
        case "PBEwithSHA-1and3-keyDESEDE-CBC":
          parameters = encoder.GenerateParam("DESEDE", 192 /*0xC0*/, 64 /*0x40*/);
          break;
        case "PBEwithSHA-1and2-keyDESEDE-CBC":
          parameters = encoder.GenerateParam("DESEDE", 128 /*0x80*/, 64 /*0x40*/);
          break;
        case "PBEwithSHA-1and128bitRC2-CBC":
          parameters = encoder.GenerateParam("RC2", 128 /*0x80*/, 64 /*0x40*/);
          break;
        case "PBEwithSHA-1and40bitRC2-CBC":
          parameters = encoder.GenerateParam("RC2", 40, 64 /*0x40*/);
          break;
        case "PBEwithSHA-1andDES-CBC":
          parameters = encoder.GenerateParam("DES", 64 /*0x40*/, 64 /*0x40*/);
          break;
        case "PBEwithSHA-1andRC2-CBC":
          parameters = encoder.GenerateParam("RC2", 64 /*0x40*/, 64 /*0x40*/);
          break;
      }
    }
    else if (algorithm1.StartsWith("PBEwithSHA-256"))
    {
      PasswordGenerator encoder = this.GetEncoder((string) this.m_type[(object) algorithm1], (IMessageDigest) new SHA256MessageDigest(), key, salt, iterationCount);
      switch (algorithm1)
      {
        case "PBEwithSHA-256and128bitAES-CBC-BC":
          parameters = encoder.GenerateParam("AES", 128 /*0x80*/, 128 /*0x80*/);
          break;
        case "PBEwithSHA-256and192bitAES-CBC-BC":
          parameters = encoder.GenerateParam("AES", 192 /*0xC0*/, 128 /*0x80*/);
          break;
        case "PBEwithSHA-256and256bitAES-CBC-BC":
          parameters = encoder.GenerateParam("AES", 256 /*0x0100*/, 128 /*0x80*/);
          break;
      }
    }
    else if (algorithm1.StartsWith("PBEwithHmac"))
    {
      IMessageDigest digest = new MessageDigestFinder().GetDigest(algorithm1.Substring("PBEwithHmac".Length));
      parameters = this.GetEncoder((string) this.m_type[(object) algorithm1], digest, key, salt, iterationCount).GenerateParam(digest.MessageDigestSize * 8);
    }
    Array.Clear((Array) key, 0, key.Length);
    return this.FixDataEncryptionParity(algorithm1, parameters);
  }

  internal object CreateEncoder(DerObjectID algorithmOid) => this.CreateEncoder(algorithmOid.ID);

  internal object CreateEncoder(Algorithms algId) => this.CreateEncoder(algId.ObjectID.ID);

  internal object CreateEncoder(string algorithm)
  {
    string algorithm1 = (string) this.m_algorithms[(object) algorithm.ToUpperInvariant()];
    if (algorithm1.StartsWith("PBEwithMD2") || algorithm1.StartsWith("PBEwithMD5") || algorithm1.StartsWith("PBEwithSHA-1") || algorithm1.StartsWith("PBEwithSHA-256"))
    {
      if (algorithm1.EndsWith("AES-CBC-BC") || algorithm1.EndsWith("AES-CBC-OPENSSL"))
        return (object) CipherUtils.GetCipher("AES/CBC");
      if (algorithm1.EndsWith("DES-CBC"))
        return (object) CipherUtils.GetCipher("DES/CBC");
      if (algorithm1.EndsWith("DESEDE-CBC"))
        return (object) CipherUtils.GetCipher("DESEDE/CBC");
      if (algorithm1.EndsWith("RC2-CBC"))
        return (object) CipherUtils.GetCipher("RC2/CBC");
      if (algorithm1.EndsWith("RC4"))
        return (object) CipherUtils.GetCipher("RC4");
    }
    return (object) null;
  }

  private ICipherParam FixDataEncryptionParity(string mechanism, ICipherParam parameters)
  {
    if (!mechanism.EndsWith("DES-CBC") & !mechanism.EndsWith("DESEDE-CBC"))
      return parameters;
    if (parameters is InvalidParameter)
    {
      InvalidParameter invalidParameter = (InvalidParameter) parameters;
      return (ICipherParam) new InvalidParameter(this.FixDataEncryptionParity(mechanism, invalidParameter.Parameters), invalidParameter.InvalidBytes);
    }
    byte[] keys = ((KeyParameter) parameters).Keys;
    for (int index = 0; index < keys.Length; ++index)
    {
      int num = (int) keys[index];
      keys[index] = (byte) (num & 254 | (num >> 1 ^ num >> 2 ^ num >> 3 ^ num >> 4 ^ num >> 5 ^ num >> 6 ^ num >> 7 ^ 1) & 1);
    }
    return (ICipherParam) new KeyParameter(keys);
  }
}
