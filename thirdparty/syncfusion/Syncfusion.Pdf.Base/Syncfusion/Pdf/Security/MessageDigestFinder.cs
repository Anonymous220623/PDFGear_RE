// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.MessageDigestFinder
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class MessageDigestFinder
{
  private readonly IDictionary m_algorithms = (IDictionary) new Hashtable();
  private readonly IDictionary m_ids = (IDictionary) new Hashtable();

  internal MessageDigestFinder()
  {
    ((MessageDigestFinder.DigestAlgorithm) Enums.GetArbitraryValue(typeof (MessageDigestFinder.DigestAlgorithm))).ToString();
    this.m_algorithms[(object) "SHA1"] = (object) "SHA-1";
    this.m_algorithms[(object) new DerObjectID("1.3.14.3.2.26").ID] = (object) "SHA-1";
    this.m_algorithms[(object) "SHA256"] = (object) "SHA-256";
    this.m_algorithms[(object) NISTOIDs.SHA256.ID] = (object) "SHA-256";
    this.m_algorithms[(object) "SHA384"] = (object) "SHA-384";
    this.m_algorithms[(object) NISTOIDs.SHA384.ID] = (object) "SHA-384";
    this.m_algorithms[(object) "SHA512"] = (object) "SHA-512";
    this.m_algorithms[(object) NISTOIDs.SHA512.ID] = (object) "SHA-512";
    this.m_algorithms[(object) "MD5"] = (object) "MD5";
    this.m_algorithms[(object) PKCSOIDs.MD5.ID] = (object) "MD5";
    this.m_algorithms[(object) "RIPEMD-160"] = (object) "RIPEMD160";
    this.m_algorithms[(object) "RIPEMD160"] = (object) "RIPEMD160";
    this.m_algorithms[(object) NISTOIDs.RipeMD160.ID] = (object) "RIPEMD160";
    this.m_ids[(object) "SHA-1"] = (object) new DerObjectID("1.3.14.3.2.26");
    this.m_ids[(object) "SHA-256"] = (object) NISTOIDs.SHA256;
    this.m_ids[(object) "SHA-384"] = (object) NISTOIDs.SHA384;
    this.m_ids[(object) "SHA-512"] = (object) NISTOIDs.SHA512;
    this.m_ids[(object) "MD5"] = (object) PKCSOIDs.MD5;
    this.m_ids[(object) "RIPEMD160"] = (object) NISTOIDs.RipeMD160;
  }

  internal IMessageDigest GetMessageDigest(DerObjectID id) => this.GetDigest(id.ID);

  internal IMessageDigest GetDigest(string algorithm)
  {
    string upperInvariant = algorithm.ToUpperInvariant();
    switch ((MessageDigestFinder.DigestAlgorithm) Enums.GetEnumValue(typeof (MessageDigestFinder.DigestAlgorithm), (string) this.m_algorithms[(object) upperInvariant] ?? upperInvariant))
    {
      case MessageDigestFinder.DigestAlgorithm.SHA_1:
        return (IMessageDigest) new SHA1MessageDigest();
      case MessageDigestFinder.DigestAlgorithm.SHA_256:
        return (IMessageDigest) new SHA256MessageDigest();
      case MessageDigestFinder.DigestAlgorithm.MD5:
        return (IMessageDigest) new MessageDigest5();
      case MessageDigestFinder.DigestAlgorithm.SHA_384:
        return (IMessageDigest) new SHA384MessageDigest();
      case MessageDigestFinder.DigestAlgorithm.SHA_512:
        return (IMessageDigest) new SHA512MessageDigest();
      case MessageDigestFinder.DigestAlgorithm.RIPEMD160:
        return (IMessageDigest) new RIPEMD160MessageDigest();
      default:
        throw new Exception("Invalid message");
    }
  }

  internal byte[] CalculateDigest(string algorithm, byte[] bytes)
  {
    IMessageDigest digest = this.GetDigest(algorithm);
    digest.Update(bytes, 0, bytes.Length);
    return this.DoFinal(digest);
  }

  internal byte[] DoFinal(IMessageDigest digest)
  {
    byte[] bytes = new byte[digest.MessageDigestSize];
    digest.DoFinal(bytes, 0);
    return bytes;
  }

  private enum DigestAlgorithm
  {
    SHA_1,
    SHA_256,
    MD5,
    SHA_384,
    SHA_512,
    RIPEMD160,
  }
}
