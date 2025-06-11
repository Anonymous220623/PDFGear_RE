// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.SignerUtilities
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class SignerUtilities
{
  internal IDictionary m_algms = (IDictionary) new Hashtable();
  internal IDictionary m_oids = (IDictionary) new Hashtable();

  internal SignerUtilities()
  {
    this.m_algms[(object) "MD2WITHRSA"] = (object) "MD2withRSA";
    this.m_algms[(object) "MD2WITHRSAENCRYPTION"] = (object) "MD2withRSA";
    this.m_algms[(object) PKCSOIDs.MD2WithRsaEncryption.ID] = (object) "MD2withRSA";
    this.m_algms[(object) PKCSOIDs.RsaEncryption.ID] = (object) "RSA";
    this.m_algms[(object) "SHA1WITHRSA"] = (object) "SHA-1withRSA";
    this.m_algms[(object) "SHA1WITHRSAENCRYPTION"] = (object) "SHA-1withRSA";
    this.m_algms[(object) PKCSOIDs.Sha1WithRsaEncryption.ID] = (object) "SHA-1withRSA";
    this.m_algms[(object) "SHA-1WITHRSA"] = (object) "SHA-1withRSA";
    this.m_algms[(object) "SHA256WITHRSA"] = (object) "SHA-256withRSA";
    this.m_algms[(object) "SHA256WITHRSAENCRYPTION"] = (object) "SHA-256withRSA";
    this.m_algms[(object) PKCSOIDs.Sha256WithRsaEncryption.ID] = (object) "SHA-256withRSA";
    this.m_algms[(object) "SHA-256WITHRSA"] = (object) "SHA-256withRSA";
    this.m_algms[(object) "SHA1WITHRSAANDMGF1"] = (object) "SHA-1withRSAandMGF1";
    this.m_algms[(object) "SHA-1WITHRSAANDMGF1"] = (object) "SHA-1withRSAandMGF1";
    this.m_algms[(object) "SHA1WITHRSA/PSS"] = (object) "SHA-1withRSAandMGF1";
    this.m_algms[(object) "SHA-1WITHRSA/PSS"] = (object) "SHA-1withRSAandMGF1";
    this.m_algms[(object) "SHA224WITHRSAANDMGF1"] = (object) "SHA-224withRSAandMGF1";
    this.m_algms[(object) "SHA-224WITHRSAANDMGF1"] = (object) "SHA-224withRSAandMGF1";
    this.m_algms[(object) "SHA224WITHRSA/PSS"] = (object) "SHA-224withRSAandMGF1";
    this.m_algms[(object) "SHA-224WITHRSA/PSS"] = (object) "SHA-224withRSAandMGF1";
    this.m_algms[(object) "SHA256WITHRSAANDMGF1"] = (object) "SHA-256withRSAandMGF1";
    this.m_algms[(object) "SHA-256WITHRSAANDMGF1"] = (object) "SHA-256withRSAandMGF1";
    this.m_algms[(object) "SHA256WITHRSA/PSS"] = (object) "SHA-256withRSAandMGF1";
    this.m_algms[(object) "SHA-256WITHRSA/PSS"] = (object) "SHA-256withRSAandMGF1";
    this.m_algms[(object) "SHA384WITHRSA"] = (object) "SHA-384withRSA";
    this.m_algms[(object) "SHA512WITHRSA"] = (object) "SHA-512withRSA";
    this.m_algms[(object) "SHA384WITHRSAENCRYPTION"] = (object) "SHA-384withRSA";
    this.m_algms[(object) PKCSOIDs.Sha384WithRsaEncryption.ID] = (object) "SHA-384withRSA";
    this.m_algms[(object) "SHA-384WITHRSA"] = (object) "SHA-384withRSA";
    this.m_algms[(object) "SHA-512WITHRSA"] = (object) "SHA-512withRSA";
    this.m_algms[(object) "SHA384WITHRSAANDMGF1"] = (object) "SHA-384withRSAandMGF1";
    this.m_algms[(object) "SHA-384WITHRSAANDMGF1"] = (object) "SHA-384withRSAandMGF1";
    this.m_algms[(object) "SHA384WITHRSA/PSS"] = (object) "SHA-384withRSAandMGF1";
    this.m_algms[(object) "SHA-384WITHRSA/PSS"] = (object) "SHA-384withRSAandMGF1";
    this.m_algms[(object) "SHA512WITHRSAANDMGF1"] = (object) "SHA-512withRSAandMGF1";
    this.m_algms[(object) "SHA-512WITHRSAANDMGF1"] = (object) "SHA-512withRSAandMGF1";
    this.m_algms[(object) "SHA512WITHRSA/PSS"] = (object) "SHA-512withRSAandMGF1";
    this.m_algms[(object) "SHA-512WITHRSA/PSS"] = (object) "SHA-512withRSAandMGF1";
    this.m_algms[(object) "DSAWITHSHA256"] = (object) "SHA-256withDSA";
    this.m_algms[(object) "DSAWITHSHA-256"] = (object) "SHA-256withDSA";
    this.m_algms[(object) "SHA256/DSA"] = (object) "SHA-256withDSA";
    this.m_algms[(object) "SHA-256/DSA"] = (object) "SHA-256withDSA";
    this.m_algms[(object) "SHA256WITHDSA"] = (object) "SHA-256withDSA";
    this.m_algms[(object) "SHA-256WITHDSA"] = (object) "SHA-256withDSA";
    this.m_algms[(object) NISTOIDs.DSAWithSHA256.ID] = (object) "SHA-256withDSA";
    this.m_algms[(object) "RIPEMD160WITHRSA"] = (object) "RIPEMD160withRSA";
    this.m_algms[(object) "RIPEMD160WITHRSAENCRYPTION"] = (object) "RIPEMD160withRSA";
    this.m_algms[(object) NISTOIDs.RsaSignatureWithRipeMD160.ID] = (object) "RIPEMD160withRSA";
    this.m_oids[(object) "SHA-1withRSA"] = (object) PKCSOIDs.Sha1WithRsaEncryption;
    this.m_oids[(object) "SHA-256withRSA"] = (object) PKCSOIDs.Sha256WithRsaEncryption;
    this.m_oids[(object) "SHA-384withRSA"] = (object) PKCSOIDs.Sha384WithRsaEncryption;
    this.m_oids[(object) "SHA-512withRSA"] = (object) PKCSOIDs.Sha512WithRsaEncryption;
    this.m_oids[(object) "RIPEMD160withRSA"] = (object) NISTOIDs.RsaSignatureWithRipeMD160;
    this.m_algms[(object) "NONEWITHECDSA"] = (object) "NONEwithECDSA";
    this.m_algms[(object) "ECDSAWITHNONE"] = (object) "NONEwithECDSA";
    this.m_algms[(object) "ECDSA"] = (object) "SHA-1withECDSA";
    this.m_algms[(object) "SHA1/ECDSA"] = (object) "SHA-1withECDSA";
    this.m_algms[(object) "SHA-1/ECDSA"] = (object) "SHA-1withECDSA";
    this.m_algms[(object) "ECDSAWITHSHA1"] = (object) "SHA-1withECDSA";
    this.m_algms[(object) "ECDSAWITHSHA-1"] = (object) "SHA-1withECDSA";
    this.m_algms[(object) "SHA1WITHECDSA"] = (object) "SHA-1withECDSA";
    this.m_algms[(object) "SHA-1WITHECDSA"] = (object) "SHA-1withECDSA";
    this.m_algms[(object) ECDSAOIDs.ECDSAwithSHA1.ID] = (object) "SHA-1withECDSA";
    this.m_algms[(object) ECBrainpoolIDs.EllipticSignSignWithSha1.ID] = (object) "SHA-1withECDSA";
    this.m_algms[(object) "SHA224/ECDSA"] = (object) "SHA-224withECDSA";
    this.m_algms[(object) "SHA-224/ECDSA"] = (object) "SHA-224withECDSA";
    this.m_algms[(object) "ECDSAWITHSHA224"] = (object) "SHA-224withECDSA";
    this.m_algms[(object) "ECDSAWITHSHA-224"] = (object) "SHA-224withECDSA";
    this.m_algms[(object) "SHA224WITHECDSA"] = (object) "SHA-224withECDSA";
    this.m_algms[(object) "SHA-224WITHECDSA"] = (object) "SHA-224withECDSA";
    this.m_algms[(object) ECDSAOIDs.ECDSAwithSHA224.ID] = (object) "SHA-224withECDSA";
    this.m_algms[(object) "SHA256/ECDSA"] = (object) "SHA-256withECDSA";
    this.m_algms[(object) "SHA-256/ECDSA"] = (object) "SHA-256withECDSA";
    this.m_algms[(object) "ECDSAWITHSHA256"] = (object) "SHA-256withECDSA";
    this.m_algms[(object) "ECDSAWITHSHA-256"] = (object) "SHA-256withECDSA";
    this.m_algms[(object) "SHA256WITHECDSA"] = (object) "SHA-256withECDSA";
    this.m_algms[(object) "SHA-256WITHECDSA"] = (object) "SHA-256withECDSA";
    this.m_algms[(object) ECDSAOIDs.ECDSAwithSHA256.ID] = (object) "SHA-256withECDSA";
    this.m_algms[(object) "SHA384/ECDSA"] = (object) "SHA-384withECDSA";
    this.m_algms[(object) "SHA-384/ECDSA"] = (object) "SHA-384withECDSA";
    this.m_algms[(object) "ECDSAWITHSHA384"] = (object) "SHA-384withECDSA";
    this.m_algms[(object) "ECDSAWITHSHA-384"] = (object) "SHA-384withECDSA";
    this.m_algms[(object) "SHA384WITHECDSA"] = (object) "SHA-384withECDSA";
    this.m_algms[(object) "SHA-384WITHECDSA"] = (object) "SHA-384withECDSA";
    this.m_algms[(object) ECDSAOIDs.ECDSAwithSHA384.ID] = (object) "SHA-384withECDSA";
    this.m_algms[(object) "SHA512/ECDSA"] = (object) "SHA-512withECDSA";
    this.m_algms[(object) "SHA-512/ECDSA"] = (object) "SHA-512withECDSA";
    this.m_algms[(object) "ECDSAWITHSHA512"] = (object) "SHA-512withECDSA";
    this.m_algms[(object) "ECDSAWITHSHA-512"] = (object) "SHA-512withECDSA";
    this.m_algms[(object) "SHA512WITHECDSA"] = (object) "SHA-512withECDSA";
    this.m_algms[(object) "SHA-512WITHECDSA"] = (object) "SHA-512withECDSA";
    this.m_algms[(object) ECDSAOIDs.ECDSAwithSHA512.ID] = (object) "SHA-512withECDSA";
    this.m_algms[(object) "RIPEMD160/ECDSA"] = (object) "RIPEMD160withECDSA";
    this.m_algms[(object) "ECDSAWITHRIPEMD160"] = (object) "RIPEMD160withECDSA";
    this.m_algms[(object) "RIPEMD160WITHECDSA"] = (object) "RIPEMD160withECDSA";
    this.m_algms[(object) ECBrainpoolIDs.EllipticSignWithRipeMD160.ID] = (object) "RIPEMD160withECDSA";
    this.m_oids[(object) "SHA-1withECDSA"] = (object) ECDSAOIDs.ECDSAwithSHA1;
    this.m_oids[(object) "SHA-224withECDSA"] = (object) ECDSAOIDs.ECDSAwithSHA224;
    this.m_oids[(object) "SHA-256withECDSA"] = (object) ECDSAOIDs.ECDSAwithSHA256;
    this.m_oids[(object) "SHA-384withECDSA"] = (object) ECDSAOIDs.ECDSAwithSHA384;
    this.m_oids[(object) "SHA-512withECDSA"] = (object) ECDSAOIDs.ECDSAwithSHA512;
  }

  internal DerObjectID GetOID(string mechanism)
  {
    mechanism = mechanism != null ? mechanism.ToUpperInvariant() : throw new ArgumentNullException(nameof (mechanism));
    string algm = (string) this.m_algms[(object) mechanism];
    if (algm != null)
      mechanism = algm;
    return (DerObjectID) this.m_oids[(object) mechanism];
  }

  internal ISigner GetSigner(string algorithm)
  {
    algorithm = algorithm != null ? algorithm.ToUpperInvariant() : throw new ArgumentNullException(nameof (algorithm));
    switch ((string) this.m_algms[(object) algorithm] ?? algorithm)
    {
      case "SHA-1withRSA":
        return (ISigner) new RMDSigner((IMessageDigest) new SHA1MessageDigest());
      case "SHA-256withRSA":
        return (ISigner) new RMDSigner((IMessageDigest) new SHA256MessageDigest());
      case "SHA-384withRSA":
        return (ISigner) new RMDSigner((IMessageDigest) new SHA384MessageDigest());
      case "SHA-512withRSA":
        return (ISigner) new RMDSigner((IMessageDigest) new SHA512MessageDigest());
      case "RIPEMD160withRSA":
        return (ISigner) new RMDSigner((IMessageDigest) new RIPEMD160MessageDigest());
      case "RAWRSASSA-PSS":
        return (ISigner) PSSSigner.CreateRawSigner((ICipherBlock) new RSAAlgorithm(), (IMessageDigest) new SHA1MessageDigest());
      case "PSSwithRSA":
        return (ISigner) new PSSSigner((ICipherBlock) new RSAAlgorithm(), (IMessageDigest) new SHA1MessageDigest());
      case "SHA-1withRSAandMGF1":
        return (ISigner) new PSSSigner((ICipherBlock) new RSAAlgorithm(), (IMessageDigest) new SHA1MessageDigest());
      case "SHA-256withRSAandMGF1":
        return (ISigner) new PSSSigner((ICipherBlock) new RSAAlgorithm(), (IMessageDigest) new SHA256MessageDigest());
      case "SHA-384withRSAandMGF1":
        return (ISigner) new PSSSigner((ICipherBlock) new RSAAlgorithm(), (IMessageDigest) new SHA384MessageDigest());
      case "SHA-512withRSAandMGF1":
        return (ISigner) new PSSSigner((ICipherBlock) new RSAAlgorithm(), (IMessageDigest) new SHA512MessageDigest());
      case "SHA-1withECDSA":
        return (ISigner) new DSASigner((IDSASigner) new ECDSAAlgorithm(), (IMessageDigest) new SHA1MessageDigest());
      case "SHA-256withECDSA":
        return (ISigner) new DSASigner((IDSASigner) new ECDSAAlgorithm(), (IMessageDigest) new SHA256MessageDigest());
      case "SHA-384withECDSA":
        return (ISigner) new DSASigner((IDSASigner) new ECDSAAlgorithm(), (IMessageDigest) new SHA384MessageDigest());
      case "SHA-512withECDSA":
        return (ISigner) new DSASigner((IDSASigner) new ECDSAAlgorithm(), (IMessageDigest) new SHA512MessageDigest());
      case "RIPEMD160withECDSA":
        return (ISigner) new DSASigner((IDSASigner) new ECDSAAlgorithm(), (IMessageDigest) new RIPEMD160MessageDigest());
      default:
        throw new Exception($"Signer {algorithm} not recognised.");
    }
  }

  internal string GetEncoding(DerObjectID oid) => (string) this.m_algms[(object) oid.ID];

  internal string GetAlgorithmName(DerObjectID oid)
  {
    return this.m_oids.Contains((object) oid) ? (string) this.m_oids[(object) oid] : oid.ID;
  }
}
