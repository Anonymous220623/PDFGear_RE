// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.X509Certificate
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class X509Certificate : X509ExtensionBase
{
  private X509CertificateStructure m_c;
  private BaseConstraints m_basicConstraints;
  private bool[] m_keyUsage;
  private bool m_hashValueSet;
  private int m_hashValue;

  protected X509Certificate()
  {
  }

  internal X509Certificate(X509CertificateStructure c)
  {
    this.m_c = c;
    try
    {
      Asn1Octet extension = this.GetExtension(new DerObjectID("2.5.29.19"));
      if (extension != null)
        this.m_basicConstraints = BaseConstraints.GetConstraints((object) Asn1.FromByteArray(extension.GetOctets()));
    }
    catch (Exception ex)
    {
      throw new Exception("cannot construct BasicConstraints: " + (object) ex);
    }
    try
    {
      Asn1Octet extension = this.GetExtension(new DerObjectID("2.5.29.15"));
      if (extension != null)
      {
        DerBitString derBitString = DerBitString.GetString((object) Asn1.FromByteArray(extension.GetOctets()));
        byte[] bytes = derBitString.GetBytes();
        int num = bytes.Length * 8 - derBitString.ExtraBits;
        this.m_keyUsage = new bool[num < 9 ? 9 : num];
        for (int index = 0; index != num; ++index)
          this.m_keyUsage[index] = ((int) bytes[index / 8] & 128 /*0x80*/ >> index % 8) != 0;
      }
      else
        this.m_keyUsage = (bool[]) null;
    }
    catch (Exception ex)
    {
      throw new Exception("cannot construct KeyUsage: " + (object) ex);
    }
  }

  internal virtual X509CertificateStructure CertificateStructure => this.m_c;

  public virtual bool IsValidNow => this.IsValid(DateTime.UtcNow);

  public virtual bool IsValid(DateTime time)
  {
    return time.CompareTo(this.NotBefore) >= 0 && time.CompareTo(this.NotAfter) <= 0;
  }

  public virtual void CheckValidity() => this.CheckValidity(DateTime.UtcNow);

  public virtual void CheckValidity(DateTime time)
  {
    if (time.CompareTo(this.NotAfter) > 0)
      throw new Exception("certificate expired on " + this.m_c.EndDate.GetTime());
    if (time.CompareTo(this.NotBefore) < 0)
      throw new Exception("certificate not valid until " + this.m_c.StartDate.GetTime());
  }

  public virtual int Version => this.m_c.Version;

  public virtual Number SerialNumber => this.m_c.SerialNumber.Value;

  public virtual X509Name IssuerDN => this.m_c.Issuer;

  public virtual X509Name SubjectDN => this.m_c.Subject;

  public virtual DateTime NotBefore => this.m_c.StartDate.ToDateTime();

  public virtual DateTime NotAfter => this.m_c.EndDate.ToDateTime();

  public virtual byte[] GetTbsCertificate() => this.m_c.TbsCertificate.GetDerEncoded();

  public virtual byte[] GetSignature() => this.m_c.Signature.GetBytes();

  public virtual string SigAlgName
  {
    get => new SignerUtilities().GetEncoding(this.m_c.SignatureAlgorithm.ObjectID);
  }

  public virtual string SigAlgOid => this.m_c.SignatureAlgorithm.ObjectID.ID;

  public virtual byte[] GetSigAlgParams()
  {
    return this.m_c.SignatureAlgorithm.Parameters != null ? this.m_c.SignatureAlgorithm.Parameters.GetDerEncoded() : (byte[]) null;
  }

  public virtual DerBitString IssuerUniqueID => this.m_c.TbsCertificate.IssuerUniqueID;

  public virtual DerBitString SubjectUniqueID => this.m_c.TbsCertificate.SubjectUniqueID;

  public virtual bool[] GetKeyUsage()
  {
    return this.m_keyUsage != null ? (bool[]) this.m_keyUsage.Clone() : (bool[]) null;
  }

  public virtual IList GetExtendedKeyUsage()
  {
    Asn1Octet extension = this.GetExtension(new DerObjectID("2.5.29.37"));
    if (extension == null)
      return (IList) null;
    try
    {
      Asn1Sequence sequence = Asn1Sequence.GetSequence((object) Asn1.FromByteArray(extension.GetOctets()));
      IList extendedKeyUsage = (IList) new ArrayList();
      foreach (DerObjectID derObjectId in sequence)
        extendedKeyUsage.Add((object) derObjectId.ID);
      return extendedKeyUsage;
    }
    catch (Exception ex)
    {
      throw new Exception("error processing extended key usage extension", ex);
    }
  }

  internal List<string> GetOids(bool critical)
  {
    X509Extensions x509Extensions = this.GetX509Extensions();
    if (x509Extensions == null)
      return (List<string>) null;
    List<string> oids = new List<string>();
    foreach (DerObjectID extensionOid in x509Extensions.ExtensionOids)
    {
      if (x509Extensions.GetExtension(extensionOid).IsCritical == critical)
        oids.Add(extensionOid.ID);
    }
    return oids;
  }

  public virtual int GetBasicConstraints()
  {
    if (this.m_basicConstraints == null || !this.m_basicConstraints.IsCertificate)
      return -1;
    return this.m_basicConstraints.PathLenConstraint == null ? int.MaxValue : this.m_basicConstraints.PathLenConstraint.IntValue;
  }

  public virtual ICollection GetSubjectAlternativeNames() => this.GetAlternativeNames("2.5.29.17");

  public virtual ICollection GetIssuerAlternativeNames() => this.GetAlternativeNames("2.5.29.18");

  protected virtual ICollection GetAlternativeNames(string oid)
  {
    return this.GetExtension(new DerObjectID(oid)) == null ? (ICollection) null : (ICollection) new ArrayList();
  }

  protected override X509Extensions GetX509Extensions()
  {
    return this.m_c.Version != 3 ? (X509Extensions) null : this.m_c.TbsCertificate.Extensions;
  }

  public virtual CipherParameter GetPublicKey() => this.CreateKey(this.m_c.SubjectPublicKeyInfo);

  internal CipherParameter CreateKey(PublicKeyInformation keyInfo)
  {
    Algorithms algorithm = keyInfo.Algorithm;
    DerObjectID objectId = algorithm.ObjectID;
    if (objectId.ID.Equals(PKCSOIDs.RsaEncryption.ID) || objectId.ID.Equals(X509Objects.IdEARsa.ID))
    {
      RSAPublicKey publicKey = RSAPublicKey.GetPublicKey((object) keyInfo.GetPublicKey());
      return (CipherParameter) new RsaKeyParam(false, publicKey.Modulus, publicKey.PublicExponent);
    }
    if (!objectId.Equals((object) ECDSAOIDs.IdECPublicKey))
      throw new Exception("algorithm identifier in key not recognised: " + (object) objectId);
    ECX962Params ecX962Params = new ECX962Params(algorithm.Parameters.GetAsn1());
    ECX9Field ecX9Field = !ecX962Params.IsNamedCurve ? new ECX9Field((Asn1Sequence) ecX962Params.Parameters) : EllipicCryptoKeyGen.GetECCurveByObjectID((DerObjectID) ecX962Params.Parameters);
    Asn1Octet sequence = (Asn1Octet) new DerOctet(keyInfo.PublicKey.GetBytes());
    EllipticPoint point = new ECx9Point(ecX9Field.Curve, sequence).Point;
    if (ecX962Params.IsNamedCurve)
      return (CipherParameter) new ECPublicKeyParam("EC", point, (DerObjectID) ecX962Params.Parameters);
    EllipticCurveParams parameters = new EllipticCurveParams(ecX9Field.Curve, ecX9Field.PointG, ecX9Field.NumberX, ecX9Field.NumberY, ecX9Field.Seed());
    return (CipherParameter) new ECPublicKeyParam(point, parameters);
  }

  public virtual byte[] GetEncoded() => this.m_c.GetDerEncoded();

  public override bool Equals(object obj)
  {
    if (obj == this)
      return true;
    return obj is X509Certificate x509Certificate && this.m_c.Equals((object) x509Certificate.m_c);
  }

  public override int GetHashCode()
  {
    lock (this)
    {
      if (!this.m_hashValueSet)
      {
        this.m_hashValue = this.m_c.GetHashCode();
        this.m_hashValueSet = true;
      }
    }
    return this.m_hashValue;
  }

  public override string ToString()
  {
    StringBuilder stringBuilder = new StringBuilder();
    string newLine = Environment.NewLine;
    stringBuilder.Append("  [0]         Version: ").Append(this.Version).Append(newLine);
    stringBuilder.Append("         SerialNumber: ").Append((object) this.SerialNumber).Append(newLine);
    stringBuilder.Append("             IssuerDN: ").Append((object) this.IssuerDN).Append(newLine);
    stringBuilder.Append("           Start Date: ").Append((object) this.NotBefore).Append(newLine);
    stringBuilder.Append("           Final Date: ").Append((object) this.NotAfter).Append(newLine);
    stringBuilder.Append("            SubjectDN: ").Append((object) this.SubjectDN).Append(newLine);
    stringBuilder.Append("           Public Key: ").Append((object) this.GetPublicKey()).Append(newLine);
    stringBuilder.Append("  Signature Algorithm: ").Append(this.SigAlgName).Append(newLine);
    this.GetSignature();
    X509Extensions extensions = this.m_c.TbsCertificate.Extensions;
    if (extensions != null)
    {
      IEnumerator enumerator = extensions.ExtensionOids.GetEnumerator();
      if (enumerator.MoveNext())
        stringBuilder.Append("       Extensions: \n");
      do
      {
        DerObjectID current = (DerObjectID) enumerator.Current;
        X509Extension extension = extensions.GetExtension(current);
        if (extension.Value != null)
        {
          Asn1 asn1 = Asn1.FromByteArray(extension.Value.GetOctets());
          stringBuilder.Append("                       critical(").Append(extension.IsCritical).Append(") ");
          try
          {
            if (current.Equals((object) X509Extensions.BasicConstraints))
              stringBuilder.Append((object) BaseConstraints.GetConstraints((object) asn1));
          }
          catch (Exception ex)
          {
            stringBuilder.Append(current.ID);
            stringBuilder.Append(" value = ").Append("*****");
          }
        }
        stringBuilder.Append(newLine);
      }
      while (enumerator.MoveNext());
    }
    return stringBuilder.ToString();
  }

  public virtual void Verify(CipherParameter key)
  {
    ISigner signer = new SignerUtilities().GetSigner(this.m_c.SignatureAlgorithm.ObjectID.ID);
    this.CheckSignature(key, signer);
  }

  protected virtual void CheckSignature(CipherParameter publicKey, ISigner signature)
  {
    if (!X509Certificate.IsAlgIDEqual(this.m_c.SignatureAlgorithm, this.m_c.TbsCertificate.Signature))
      throw new Exception("signature algorithm in TBS cert not same as outer cert");
    Asn1Encode parameters = this.m_c.SignatureAlgorithm.Parameters;
    signature.Initialize(false, (ICipherParam) publicKey);
    byte[] tbsCertificate = this.GetTbsCertificate();
    signature.BlockUpdate(tbsCertificate, 0, tbsCertificate.Length);
    byte[] signature1 = this.GetSignature();
    if (!signature.ValidateSignature(signature1))
      throw new Exception("Public key presented not for certificate signature");
  }

  private static bool IsAlgIDEqual(Algorithms id1, Algorithms id2)
  {
    if (!id1.ObjectID.Equals((object) id2.ObjectID))
      return false;
    Asn1Encode parameters1 = id1.Parameters;
    Asn1Encode parameters2 = id2.Parameters;
    if (parameters1 == null == (parameters2 == null))
      return object.Equals((object) parameters1, (object) parameters2);
    return parameters1 != null ? parameters1.GetAsn1() is Asn1Null : parameters2.GetAsn1() is Asn1Null;
  }
}
