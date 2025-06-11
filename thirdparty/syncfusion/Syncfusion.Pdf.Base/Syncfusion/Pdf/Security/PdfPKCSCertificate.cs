// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.PdfPKCSCertificate
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;
using System.Collections;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class PdfPKCSCertificate
{
  private readonly PdfPKCSCertificate.CertificateTable m_keys = new PdfPKCSCertificate.CertificateTable();
  private readonly PdfPKCSCertificate.CertificateTable m_certificates = new PdfPKCSCertificate.CertificateTable();
  private readonly IDictionary m_localIdentifiers = (IDictionary) new Hashtable();
  private readonly IDictionary m_chainCertificates = (IDictionary) new Hashtable();
  private readonly IDictionary m_keyCertificates = (IDictionary) new Hashtable();

  private static SubjectKeyID CreateSubjectKeyID(CipherParameter publicKey)
  {
    switch (publicKey)
    {
      case RsaKeyParam _:
        RsaKeyParam rsaKeyParam = (RsaKeyParam) publicKey;
        return new SubjectKeyID(new PublicKeyInformation(new Algorithms(PKCSOIDs.RsaEncryption, (Asn1Encode) DerNull.Value), (Asn1Encode) new RSAPublicKey(rsaKeyParam.Modulus, rsaKeyParam.Exponent).GetAsn1()));
      case ECPublicKeyParam _:
        ECPublicKeyParam ecPublicKeyParam = (ECPublicKeyParam) publicKey;
        if (ecPublicKeyParam.AlgorithmName == "ECGOST3410")
        {
          EllipticPoint ellipticPoint = ecPublicKeyParam.PublicKeyParamSet != null ? ecPublicKeyParam.PointQ : throw new Exception("Not a CryptoPro parameter set");
          Number intValue1 = ellipticPoint.PointX.ToIntValue();
          Number intValue2 = ellipticPoint.PointY.ToIntValue();
          byte[] numArray = new byte[64 /*0x40*/];
          PdfPKCSCertificate.DecompressBytes(numArray, 0, intValue1);
          PdfPKCSCertificate.DecompressBytes(numArray, 32 /*0x20*/, intValue2);
          ECGostAlgorithm ecGostAlgorithm = new ECGostAlgorithm(ecPublicKeyParam.PublicKeyParamSet, CRYPTOIDs.IDR3411X94);
          return new SubjectKeyID(new PublicKeyInformation(new Algorithms(CRYPTOIDs.IDR3410, (Asn1Encode) ecGostAlgorithm.GetAsn1()), (Asn1Encode) new DerOctet(numArray)));
        }
        ECX962Params ecX962Params;
        if (ecPublicKeyParam.PublicKeyParamSet == null)
        {
          EllipticCurveParams parameters = ecPublicKeyParam.Parameters;
          ecX962Params = new ECX962Params(new ECX9Field(parameters.Curve, parameters.PointG, parameters.NumberX, parameters.NumberY, parameters.ECSeed()));
        }
        else
          ecX962Params = new ECX962Params(ecPublicKeyParam.PublicKeyParamSet);
        Asn1Octet asn1 = (Asn1Octet) new ECx9Point(ecPublicKeyParam.PointQ).GetAsn1();
        return new SubjectKeyID(new PublicKeyInformation(new Algorithms(ECDSAOIDs.IdECPublicKey, (Asn1Encode) ecX962Params.GetAsn1()), asn1.GetOctets()));
      default:
        throw new Exception("Invalid Key");
    }
  }

  private static void DecompressBytes(byte[] encKey, int offset, Number byteBI)
  {
    byte[] byteArray = byteBI.ToByteArray();
    int num = (byteBI.BitLength + 7) / 8;
    for (int index = 0; index < num; ++index)
      encKey[offset + index] = byteArray[byteArray.Length - 1 - index];
  }

  internal PdfPKCSCertificate()
  {
  }

  public PdfPKCSCertificate(Stream input, char[] password)
    : this()
  {
    this.LoadCertificate(input, password);
  }

  internal void LoadCertificate(Stream input, char[] password)
  {
    if (input == null)
      throw new ArgumentNullException(nameof (input));
    if (password == null)
      throw new ArgumentNullException(nameof (password));
    ContentInformation contentInformation1 = new PfxData((Asn1Sequence) Asn1.FromStream(input)).ContentInformation;
    bool flag1 = false;
    bool flag2 = password.Length == 0;
    this.m_keys.Clear();
    this.m_localIdentifiers.Clear();
    IList list = (IList) new ArrayList();
    if (contentInformation1.ContentType.ID.Equals(PKCSOIDs.Data.ID))
    {
      Asn1Sequence asn1Sequence1 = (Asn1Sequence) Asn1.FromByteArray(((Asn1Octet) contentInformation1.Content).GetOctets());
      ContentInformation[] contentInformationArray = new ContentInformation[asn1Sequence1.Count];
      for (int index = 0; index != contentInformationArray.Length; ++index)
        contentInformationArray[index] = ContentInformation.GetInformation((object) asn1Sequence1[index]);
      foreach (ContentInformation contentInformation2 in (ContentInformation[]) contentInformationArray.Clone())
      {
        DerObjectID contentType = contentInformation2.ContentType;
        if (contentType.ID.Equals(PKCSOIDs.Data.ID))
        {
          foreach (Asn1Sequence fromByte in (Asn1Sequence) Asn1.FromByteArray(((Asn1Octet) contentInformation2.Content).GetOctets()))
          {
            Asn1SequenceCollection sequenceCollection = new Asn1SequenceCollection(fromByte);
            if (sequenceCollection.ID.ID.Equals(PKCSOIDs.Pkcs8ShroudedKeyBag.ID))
            {
              EncryptedPrivateKey privateKeyInformation = EncryptedPrivateKey.GetEncryptedPrivateKeyInformation((object) sequenceCollection.Value);
              KeyInformation privateKeyInfo = KeyInformationCollection.CreatePrivateKeyInfo(password, flag2, privateKeyInformation);
              RsaPrivateKeyParam rsaPrivateKeyParam = (RsaPrivateKeyParam) null;
              ECPrivateKey ecPrivateKey1 = (ECPrivateKey) null;
              if (privateKeyInfo.AlgorithmID.ObjectID.ID.Equals(PKCSOIDs.RsaEncryption.ID) || privateKeyInfo.AlgorithmID.ObjectID.ID.Equals(X509Objects.IdEARsa.ID))
              {
                RSAKey rsaKey = new RSAKey(Asn1Sequence.GetSequence((object) privateKeyInfo.PrivateKey));
                rsaPrivateKeyParam = new RsaPrivateKeyParam(rsaKey.Modulus, rsaKey.PublicExponent, rsaKey.PrivateExponent, rsaKey.Prime1, rsaKey.Prime2, rsaKey.Exponent1, rsaKey.Exponent2, rsaKey.Coefficient);
              }
              else if (privateKeyInfo.AlgorithmID.ObjectID.ID.Equals(ECDSAOIDs.IdECPublicKey.ID))
              {
                ECX962Params ecX962Params = new ECX962Params(privateKeyInfo.AlgorithmID.Parameters.GetAsn1());
                ECX9Field ecX9Field = !ecX962Params.IsNamedCurve ? new ECX9Field((Asn1Sequence) ecX962Params.Parameters) : EllipicCryptoKeyGen.GetECCurveByObjectID((DerObjectID) ecX962Params.Parameters);
                Number key = new ECPrivateKeyParam(Asn1Sequence.GetSequence((object) privateKeyInfo.PrivateKey)).GetKey();
                if (ecX962Params.IsNamedCurve)
                {
                  ECPrivateKey ecPrivateKey2 = new ECPrivateKey("EC", key, (DerObjectID) ecX962Params.Parameters);
                }
                EllipticCurveParams parameters = new EllipticCurveParams(ecX9Field.Curve, ecX9Field.PointG, ecX9Field.NumberX, ecX9Field.NumberY, ecX9Field.Seed());
                ecPrivateKey1 = new ECPrivateKey(key, parameters);
              }
              CipherParameter key1 = (CipherParameter) rsaPrivateKeyParam;
              if (key1 == null && ecPrivateKey1 != null)
                key1 = (CipherParameter) ecPrivateKey1;
              IDictionary attributes = (IDictionary) new Hashtable();
              KeyEntry keyEntry = new KeyEntry(key1, attributes);
              string key2 = (string) null;
              Asn1Octet asn1Octet = (Asn1Octet) null;
              if (sequenceCollection.Attributes != null)
              {
                foreach (Asn1Sequence attribute in sequenceCollection.Attributes)
                {
                  DerObjectID id = DerObjectID.GetID((object) attribute[0]);
                  Asn1Set asn1Set = (Asn1Set) attribute[1];
                  if (asn1Set.Count > 0)
                  {
                    Asn1Encode asn1Encode = asn1Set[0];
                    if (attributes.Contains((object) id.ID))
                    {
                      if (!attributes[(object) id.ID].Equals((object) asn1Encode))
                        throw new IOException("attempt to add existing attribute with different value");
                    }
                    else
                      attributes.Add((object) id.ID, (object) asn1Encode);
                    if (id.Equals((object) PKCSOIDs.Pkcs9AtFriendlyName))
                    {
                      key2 = ((DerString) asn1Encode).GetString();
                      this.m_keys[key2] = (object) keyEntry;
                    }
                    else if (id.Equals((object) PKCSOIDs.Pkcs9AtLocalKeyID))
                      asn1Octet = (Asn1Octet) asn1Encode;
                  }
                }
              }
              if (asn1Octet != null)
              {
                string hex = PdfString.BytesToHex(asn1Octet.GetOctets());
                if (key2 == null)
                  this.m_keys[hex] = (object) keyEntry;
                else
                  this.m_localIdentifiers[(object) key2] = (object) hex;
              }
              else
              {
                flag1 = true;
                this.m_keys["unmarked"] = (object) keyEntry;
              }
            }
            else if (sequenceCollection.ID.Equals((object) PKCSOIDs.CertBag))
              list.Add((object) sequenceCollection);
          }
        }
        else if (contentType.ID.Equals(PKCSOIDs.EncryptedData.ID))
        {
          Asn1Sequence content = contentInformation2.Content as Asn1Sequence;
          if (content.Count != 2)
            throw new ArgumentException("Invalid length of the sequence");
          if (((DerInteger) content[0]).Value.IntValue != 0)
            throw new ArgumentException("Invalid sequence version");
          Asn1Sequence asn1Sequence2 = (Asn1Sequence) content[1];
          Asn1Octet asn1Octet1 = (Asn1Octet) null;
          if (asn1Sequence2.Count == 3)
            asn1Octet1 = Asn1Octet.GetOctetString((Asn1Tag) asn1Sequence2[2], false);
          foreach (Asn1Sequence fromByte in (Asn1Sequence) Asn1.FromByteArray(PdfPKCSCertificate.GetCryptographicData(false, Algorithms.GetAlgorithms((object) asn1Sequence2[1]), password, flag2, asn1Octet1.GetOctets())))
          {
            Asn1SequenceCollection sequenceCollection = new Asn1SequenceCollection(fromByte);
            if (sequenceCollection.ID.ID.Equals(PKCSOIDs.CertBag.ID))
              list.Add((object) sequenceCollection);
            else if (sequenceCollection.ID.ID.Equals(PKCSOIDs.Pkcs8ShroudedKeyBag.ID))
            {
              EncryptedPrivateKey privateKeyInformation = EncryptedPrivateKey.GetEncryptedPrivateKeyInformation((object) sequenceCollection.Value);
              KeyInformation privateKeyInfo = KeyInformationCollection.CreatePrivateKeyInfo(password, flag2, privateKeyInformation);
              RsaPrivateKeyParam rsaPrivateKeyParam = (RsaPrivateKeyParam) null;
              if (privateKeyInfo.AlgorithmID.ObjectID.ID.Equals(PKCSOIDs.RsaEncryption.ID) || privateKeyInfo.AlgorithmID.ObjectID.ID.Equals(X509Objects.IdEARsa.ID))
              {
                RSAKey rsaKey = new RSAKey(Asn1Sequence.GetSequence((object) privateKeyInfo.PrivateKey));
                rsaPrivateKeyParam = new RsaPrivateKeyParam(rsaKey.Modulus, rsaKey.PublicExponent, rsaKey.PrivateExponent, rsaKey.Prime1, rsaKey.Prime2, rsaKey.Exponent1, rsaKey.Exponent2, rsaKey.Coefficient);
              }
              CipherParameter key3 = (CipherParameter) rsaPrivateKeyParam;
              IDictionary attributes = (IDictionary) new Hashtable();
              KeyEntry keyEntry = new KeyEntry(key3, attributes);
              string key4 = (string) null;
              Asn1Octet asn1Octet2 = (Asn1Octet) null;
              foreach (Asn1Sequence attribute in sequenceCollection.Attributes)
              {
                DerObjectID derObjectId = (DerObjectID) attribute[0];
                Asn1Set asn1Set = (Asn1Set) attribute[1];
                if (asn1Set.Count > 0)
                {
                  Asn1Encode asn1Encode = asn1Set[0];
                  if (attributes.Contains((object) derObjectId.ID))
                  {
                    if (!attributes[(object) derObjectId.ID].Equals((object) asn1Encode))
                      throw new IOException("attempt to add existing attribute with different value");
                  }
                  else
                    attributes.Add((object) derObjectId.ID, (object) asn1Encode);
                  if (derObjectId.Equals((object) PKCSOIDs.Pkcs9AtFriendlyName))
                  {
                    key4 = ((DerString) asn1Encode).GetString();
                    this.m_keys[key4] = (object) keyEntry;
                  }
                  else if (derObjectId.Equals((object) PKCSOIDs.Pkcs9AtLocalKeyID))
                    asn1Octet2 = (Asn1Octet) asn1Encode;
                }
              }
              string hex = PdfString.BytesToHex(asn1Octet2.GetOctets());
              if (key4 == null)
                this.m_keys[hex] = (object) keyEntry;
              else
                this.m_localIdentifiers[(object) key4] = (object) hex;
            }
            else if (sequenceCollection.ID.Equals((object) PKCSOIDs.KeyBag))
            {
              KeyInformation information = KeyInformation.GetInformation((object) sequenceCollection.Value);
              RsaPrivateKeyParam rsaPrivateKeyParam = (RsaPrivateKeyParam) null;
              if (information.AlgorithmID.ObjectID.ID.Equals(PKCSOIDs.RsaEncryption.ID) || information.AlgorithmID.ObjectID.ID.Equals(X509Objects.IdEARsa.ID))
              {
                RSAKey rsaKey = new RSAKey(Asn1Sequence.GetSequence((object) information.PrivateKey));
                rsaPrivateKeyParam = new RsaPrivateKeyParam(rsaKey.Modulus, rsaKey.PublicExponent, rsaKey.PrivateExponent, rsaKey.Prime1, rsaKey.Prime2, rsaKey.Exponent1, rsaKey.Exponent2, rsaKey.Coefficient);
              }
              CipherParameter key5 = (CipherParameter) rsaPrivateKeyParam;
              string key6 = (string) null;
              Asn1Octet asn1Octet3 = (Asn1Octet) null;
              IDictionary attributes = (IDictionary) new Hashtable();
              KeyEntry keyEntry = new KeyEntry(key5, attributes);
              foreach (Asn1Sequence attribute in sequenceCollection.Attributes)
              {
                DerObjectID derObjectId = (DerObjectID) attribute[0];
                Asn1Set asn1Set = (Asn1Set) attribute[1];
                if (asn1Set.Count > 0)
                {
                  Asn1Encode asn1Encode = asn1Set[0];
                  if (attributes.Contains((object) derObjectId.ID))
                  {
                    if (!attributes[(object) derObjectId.ID].Equals((object) asn1Encode))
                      throw new IOException("attempt to add existing attribute with different value");
                  }
                  else
                    attributes.Add((object) derObjectId.ID, (object) asn1Encode);
                  if (derObjectId.Equals((object) PKCSOIDs.Pkcs9AtFriendlyName))
                  {
                    key6 = ((DerString) asn1Encode).GetString();
                    this.m_keys[key6] = (object) keyEntry;
                  }
                  else if (derObjectId.Equals((object) PKCSOIDs.Pkcs9AtLocalKeyID))
                    asn1Octet3 = (Asn1Octet) asn1Encode;
                }
              }
              string hex = PdfString.BytesToHex(asn1Octet3.GetOctets());
              if (key6 == null)
                this.m_keys[hex] = (object) keyEntry;
              else
                this.m_localIdentifiers[(object) key6] = (object) hex;
            }
          }
        }
      }
    }
    this.m_certificates.Clear();
    this.m_chainCertificates.Clear();
    this.m_keyCertificates.Clear();
    foreach (Asn1SequenceCollection sequenceCollection in (IEnumerable) list)
    {
      X509Certificate certificates = new X509CertificateParser().ReadCertificate((Stream) new MemoryStream(((Asn1Octet) Asn1Tag.GetTag((object) ((Asn1Sequence) sequenceCollection.Value)[1]).GetObject()).GetOctets(), false));
      IDictionary dictionary = (IDictionary) new Hashtable();
      Asn1Octet asn1Octet = (Asn1Octet) null;
      string key7 = (string) null;
      if (sequenceCollection.Attributes != null)
      {
        foreach (Asn1Sequence attribute in sequenceCollection.Attributes)
        {
          DerObjectID id = DerObjectID.GetID((object) attribute[0]);
          Asn1Set asn1Set = (Asn1Set) attribute[1];
          if (asn1Set.Count > 0)
          {
            Asn1Encode asn1Encode = asn1Set[0];
            if (dictionary.Contains((object) id.ID))
            {
              if (!dictionary[(object) id.ID].Equals((object) asn1Encode))
                throw new IOException("attempt to add existing attribute with different value");
            }
            else
              dictionary.Add((object) id.ID, (object) asn1Encode);
            if (id.Equals((object) PKCSOIDs.Pkcs9AtFriendlyName))
              key7 = ((DerString) asn1Encode).GetString();
            else if (id.Equals((object) PKCSOIDs.Pkcs9AtLocalKeyID))
              asn1Octet = (Asn1Octet) asn1Encode;
          }
        }
      }
      PdfPKCSCertificate.CertificateIdentifier key8 = new PdfPKCSCertificate.CertificateIdentifier(certificates.GetPublicKey());
      X509Certificates x509Certificates = new X509Certificates(certificates);
      this.m_chainCertificates[(object) key8] = (object) x509Certificates;
      if (flag1)
      {
        if (this.m_keyCertificates.Count == 0)
        {
          string hex = PdfString.BytesToHex(key8.ID);
          this.m_keyCertificates[(object) hex] = (object) x509Certificates;
          object key9 = this.m_keys["unmarked"];
          this.m_keys.Remove("unmarked");
          this.m_keys[hex] = key9;
        }
      }
      else
      {
        if (asn1Octet != null)
          this.m_keyCertificates[(object) PdfString.BytesToHex(asn1Octet.GetOctets())] = (object) x509Certificates;
        if (key7 != null)
          this.m_certificates[key7] = (object) x509Certificates;
      }
    }
  }

  internal KeyEntry GetKey(string key)
  {
    return key != null ? (KeyEntry) this.m_keys[key] : throw new ArgumentNullException(nameof (key));
  }

  internal bool IsCertificate(string key)
  {
    if (key == null)
      throw new ArgumentNullException(nameof (key));
    return this.m_certificates[key] != null && this.m_keys[key] == null;
  }

  internal bool IsKey(string key)
  {
    if (key == null)
      throw new ArgumentNullException(nameof (key));
    return this.m_keys[key] != null;
  }

  private IDictionary GetContentTable()
  {
    IDictionary contentTable = (IDictionary) new Hashtable();
    foreach (string key in (IEnumerable) this.m_certificates.Keys)
      contentTable[(object) key] = (object) "cert";
    foreach (string key in (IEnumerable) this.m_keys.Keys)
    {
      if (contentTable[(object) key] == null)
        contentTable[(object) key] = (object) "key";
    }
    return contentTable;
  }

  internal IEnumerable KeyEnumerable
  {
    get => (IEnumerable) new EnumerableProxy((IEnumerable) this.GetContentTable().Keys);
  }

  internal X509Certificates GetCertificate(string key)
  {
    X509Certificates certificate = key != null ? (X509Certificates) this.m_certificates[key] : throw new ArgumentNullException(nameof (key));
    if (certificate == null)
    {
      string localIdentifier = (string) this.m_localIdentifiers[(object) key];
      certificate = localIdentifier == null ? (X509Certificates) this.m_keyCertificates[(object) key] : (X509Certificates) this.m_keyCertificates[(object) localIdentifier];
    }
    return certificate;
  }

  internal X509Certificates[] GetCertificateChain(string key)
  {
    if (key == null)
      throw new ArgumentNullException(nameof (key));
    if (!this.IsKey(key))
      return (X509Certificates[]) null;
    X509Certificates x509Certificates1 = this.GetCertificate(key);
    if (x509Certificates1 == null)
      return (X509Certificates[]) null;
    IList list = (IList) new ArrayList();
    X509Certificates x509Certificates2;
    for (; x509Certificates1 != null; x509Certificates1 = x509Certificates2 == x509Certificates1 ? (X509Certificates) null : x509Certificates2)
    {
      X509Certificate certificate1 = x509Certificates1.Certificate;
      x509Certificates2 = (X509Certificates) null;
      Asn1Octet extension = certificate1.GetExtension(X509Extensions.AuthorityKeyIdentifier);
      if (extension != null)
      {
        KeyIdentifier keyIdentifier = KeyIdentifier.GetKeyIdentifier((object) Asn1.FromByteArray(extension.GetOctets()));
        if (keyIdentifier.KeyID != null)
          x509Certificates2 = (X509Certificates) this.m_chainCertificates[(object) new PdfPKCSCertificate.CertificateIdentifier(keyIdentifier.KeyID)];
      }
      if (x509Certificates2 == null)
      {
        X509Name issuerDn = certificate1.IssuerDN;
        X509Name subjectDn = certificate1.SubjectDN;
        if (!object.Equals((object) issuerDn, (object) subjectDn))
        {
          foreach (PdfPKCSCertificate.CertificateIdentifier key1 in (IEnumerable) this.m_chainCertificates.Keys)
          {
            X509Certificates chainCertificate = (X509Certificates) this.m_chainCertificates[(object) key1];
            X509Certificate certificate2 = chainCertificate.Certificate;
            if (object.Equals((object) certificate2.SubjectDN, (object) issuerDn))
            {
              try
              {
                certificate1.Verify(certificate2.GetPublicKey());
                x509Certificates2 = chainCertificate;
                break;
              }
              catch (Exception ex)
              {
              }
            }
          }
        }
      }
      list.Add((object) x509Certificates1);
    }
    X509Certificates[] certificateChain = new X509Certificates[list.Count];
    for (int index = 0; index < list.Count; ++index)
      certificateChain[index] = (X509Certificates) list[index];
    return certificateChain;
  }

  private static byte[] GetCryptographicData(
    bool forEncryption,
    Algorithms id,
    char[] password,
    bool isZero,
    byte[] data)
  {
    PasswordUtility passwordUtility = new PasswordUtility();
    if (!(passwordUtility.CreateEncoder(id.ObjectID) is IBufferedCipher encoder))
      throw new Exception("Invalid encryption algorithm : " + (object) id.ObjectID);
    PKCS12PasswordParameter pbeParameter = PKCS12PasswordParameter.GetPBEParameter((object) id.Parameters);
    ICipherParam cipherParameters = passwordUtility.GenerateCipherParameters(id.ObjectID, password, isZero, (Asn1Encode) pbeParameter);
    encoder.Initialize(forEncryption, cipherParameters);
    return encoder.DoFinal(data);
  }

  internal class CertificateIdentifier
  {
    private readonly byte[] m_id;

    internal CertificateIdentifier(CipherParameter pubKey)
    {
      this.m_id = PdfPKCSCertificate.CreateSubjectKeyID(pubKey).GetKeyIdentifier();
    }

    internal CertificateIdentifier(byte[] id) => this.m_id = id;

    internal byte[] ID => this.m_id;

    public override int GetHashCode() => Asn1Constants.GetHashCode(this.m_id);

    public override bool Equals(object obj)
    {
      if (obj == this)
        return true;
      return obj is PdfPKCSCertificate.CertificateIdentifier certificateIdentifier && Asn1Constants.AreEqual(this.m_id, certificateIdentifier.m_id);
    }
  }

  private class CertificateTable : IEnumerable
  {
    private readonly IDictionary m_orig = (IDictionary) new Hashtable();
    private readonly IDictionary m_keys = (IDictionary) new Hashtable();

    internal void Clear()
    {
      this.m_orig.Clear();
      this.m_keys.Clear();
    }

    public IEnumerator GetEnumerator() => (IEnumerator) this.m_orig.GetEnumerator();

    internal ICollection Keys => this.m_orig.Keys;

    internal object Remove(string key)
    {
      string lowerInvariant = key.ToLowerInvariant();
      string key1 = (string) this.m_keys[(object) lowerInvariant];
      if (key1 == null)
        return (object) null;
      this.m_keys.Remove((object) lowerInvariant);
      object obj = this.m_orig[(object) key1];
      this.m_orig.Remove((object) key1);
      return obj;
    }

    internal object this[string key]
    {
      get
      {
        string key1 = (string) this.m_keys[(object) key.ToLowerInvariant()];
        return key1 == null ? (object) null : this.m_orig[(object) key1];
      }
      set
      {
        string lowerInvariant = key.ToLowerInvariant();
        string key1 = (string) this.m_keys[(object) lowerInvariant];
        if (key1 != null)
          this.m_orig.Remove((object) key1);
        this.m_keys[(object) lowerInvariant] = (object) key;
        this.m_orig[(object) key] = value;
      }
    }
  }
}
