// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.SubjectKeyID
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class SubjectKeyID : Asn1Encode
{
  private byte[] m_bytes;

  internal static SubjectKeyID GetIdentifier(object obj)
  {
    switch (obj)
    {
      case SubjectKeyID _:
        return (SubjectKeyID) obj;
      case PublicKeyInformation _:
        return new SubjectKeyID((PublicKeyInformation) obj);
      case Asn1Octet _:
        return new SubjectKeyID((Asn1Octet) obj);
      case X509Extension _:
        return SubjectKeyID.GetIdentifier((object) X509Extension.ConvertValueToObject((X509Extension) obj));
      default:
        throw new ArgumentException("Invalid entry");
    }
  }

  internal SubjectKeyID(Asn1Octet keyID) => this.m_bytes = keyID.GetOctets();

  internal SubjectKeyID(PublicKeyInformation publicKey)
  {
    this.m_bytes = SubjectKeyID.GetDigest(publicKey);
  }

  internal byte[] GetKeyIdentifier() => this.m_bytes;

  public override Asn1 GetAsn1() => (Asn1) new DerOctet(this.m_bytes);

  internal static PublicKeyInformation CreateSubjectKeyID(CipherParameter publicKey)
  {
    RsaKeyParam rsaKeyParam = publicKey is RsaKeyParam ? (RsaKeyParam) publicKey : throw new Exception("Invalid Key");
    return new PublicKeyInformation(new Algorithms(PKCSOIDs.RsaEncryption, (Asn1Encode) DerNull.Value), (Asn1Encode) new RSAPublicKey(rsaKeyParam.Modulus, rsaKeyParam.Exponent).GetAsn1());
  }

  private static byte[] GetDigest(PublicKeyInformation publicKey)
  {
    IMessageDigest messageDigest = (IMessageDigest) new SHA1MessageDigest();
    byte[] bytes1 = new byte[messageDigest.MessageDigestSize];
    byte[] bytes2 = publicKey.PublicKey.GetBytes();
    messageDigest.Update(bytes2, 0, bytes2.Length);
    messageDigest.DoFinal(bytes1, 0);
    return bytes1;
  }
}
