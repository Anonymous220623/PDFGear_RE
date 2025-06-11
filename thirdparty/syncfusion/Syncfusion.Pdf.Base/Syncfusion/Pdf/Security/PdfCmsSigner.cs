// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.PdfCmsSigner
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class PdfCmsSigner
{
  private const string rfc_FilterType = "ETSI.RFC3161";
  private int m_version = 1;
  private int m_signerVersion = 1;
  private string m_digestAlgorithmOid;
  private IMessageDigest m_messageDigest;
  private Dictionary<string, object> m_digestOid;
  private string m_encryptionAlgorithmOid;
  private byte[] m_signedData;
  private byte[] m_signedRSAData;
  private ISigner m_signer;
  private byte[] m_digest;
  private byte[] m_rsaData;
  private List<X509Certificate> m_certificates;
  private X509Certificate m_signCert;
  private string m_encryptionAlgorithm;
  private bool m_isTimeStamp;
  private byte[] m_digestAttributeData;
  private byte[] m_sigAttribute;
  private byte[] m_sigAttributeDer;
  private IMessageDigest m_encodedMessageDigest;
  private MessageDigestAlgorithms m_digestAlgorithm = new MessageDigestAlgorithms();
  private MessageDigestFinder m_digestFinder = new MessageDigestFinder();
  private X509RevocationResponse m_basicOcspResponse;
  private TimeStampToken m_timeStampToken;
  private readonly string m_ocspID = "1.3.6.1.5.5.7.48.1.1";
  private string m_hashAlgorithm;

  internal List<X509Certificate> CertificateList => this.m_certificates;

  internal X509Certificate SignerCertificate => this.m_signCert;

  internal PdfCmsSigner(
    ICipherParam privateKey,
    ICollection<X509Certificate> certChain,
    string hashAlgorithm,
    bool hasRSAdata)
  {
    this.m_digestAlgorithmOid = this.m_digestAlgorithm.GetAllowedDigests(hashAlgorithm);
    if (this.m_digestAlgorithmOid == null)
      throw new ArgumentException("Unknown Hash Algorithm", hashAlgorithm);
    this.m_version = this.m_signerVersion = 1;
    this.m_certificates = new List<X509Certificate>((IEnumerable<X509Certificate>) certChain);
    this.m_digestOid = new Dictionary<string, object>();
    this.m_digestOid[this.m_digestAlgorithmOid] = (object) null;
    if (this.m_certificates.Count > 0)
      this.m_signCert = this.m_certificates[0];
    if (privateKey != null)
    {
      if (!(privateKey is RsaKeyParam))
        throw new ArgumentException("Unknown key algorithm ", privateKey.ToString());
      this.m_encryptionAlgorithmOid = "1.2.840.113549.1.1.1";
    }
    if (!hasRSAdata)
      return;
    this.m_rsaData = new byte[0];
    this.m_messageDigest = this.MessageDigest;
  }

  internal PdfCmsSigner(string hashAlgorithm, bool hasRSAdata)
  {
    this.m_digestAlgorithmOid = new MessageDigestAlgorithms().GetAllowedDigests(hashAlgorithm);
    if (this.m_digestAlgorithmOid == null)
      throw new ArgumentException("Unknown Hash Algorithm", hashAlgorithm);
    this.m_version = this.m_signerVersion = 1;
    this.m_digestOid = new Dictionary<string, object>();
    this.m_digestOid[this.m_digestAlgorithmOid] = (object) null;
    this.m_encryptionAlgorithmOid = "1.2.840.113549.1.1.1";
    if (!hasRSAdata)
      return;
    this.m_rsaData = new byte[0];
    this.m_messageDigest = this.MessageDigest;
  }

  internal PdfCmsSigner(byte[] contentByte, byte[] certBytes)
  {
    ICollection collection = new X509CertificateParser().ReadCertificates(certBytes);
    Asn1 asn1 = new Asn1Stream(contentByte).ReadAsn1();
    IList<X509Certificate> certs = (IList<X509Certificate>) new List<X509Certificate>();
    if (asn1 != null && asn1 is Asn1Octet)
    {
      foreach (X509Certificate x509Certificate in (IEnumerable) collection)
      {
        if (this.m_signCert == null)
          this.m_signCert = x509Certificate;
        certs.Add(x509Certificate);
      }
    }
    if (this.m_signCert == null)
      return;
    this.m_certificates = this.GetCertificateChain(this.m_signCert, certs);
    ISigner signer = new SignerUtilities().GetSigner("SHA-1withRSA");
    signer.Initialize(false, (ICipherParam) this.m_signCert.GetPublicKey());
    this.m_digestAlgorithmOid = "1.2.840.10040.4.3";
    this.m_encryptionAlgorithmOid = "1.3.36.3.3.1.2";
    this.m_digest = (asn1 as Asn1Octet).GetOctets();
    this.m_signer = signer;
    certs.Clear();
  }

  internal PdfCmsSigner(byte[] bytes, string subFilter)
  {
    this.m_digestAlgorithm = new MessageDigestAlgorithms();
    this.m_digestFinder = new MessageDigestFinder();
    this.m_isTimeStamp = subFilter == "ETSI.RFC3161";
    int num1 = subFilter == "ETSI.CAdES.detached" ? 1 : 0;
    Asn1 asn1 = new Asn1Stream(bytes).ReadAsn1();
    if (asn1 == null || !(asn1 is Asn1Sequence))
      return;
    Asn1Sequence asn1Sequence1 = asn1 as Asn1Sequence;
    if (!(((DerObjectID) asn1Sequence1[0]).ID == "1.2.840.113549.1.7.2"))
      return;
    Asn1Sequence asn1Sequence2 = (Asn1Sequence) ((Asn1Tag) asn1Sequence1[1]).GetObject();
    int intValue = ((DerInteger) asn1Sequence2[0]).Value.IntValue;
    Dictionary<string, object> dictionary = new Dictionary<string, object>();
    foreach (Asn1Sequence asn1Sequence3 in (Asn1Set) asn1Sequence2[1])
    {
      DerObjectID derObjectId = (DerObjectID) asn1Sequence3[0];
      dictionary[derObjectId.ID] = (object) null;
    }
    X509CertificateParser certificateParser = new X509CertificateParser();
    IList<X509Certificate> certs = (IList<X509Certificate>) new List<X509Certificate>();
    foreach (X509Certificate readCertificate in (IEnumerable) certificateParser.ReadCertificates(bytes))
      certs.Add(readCertificate);
    Asn1Sequence asn1Sequence4 = (Asn1Sequence) asn1Sequence2[2];
    if (asn1Sequence4.Count > 1)
      this.m_rsaData = ((Asn1Octet) ((Asn1Tag) asn1Sequence4[1]).GetObject()).GetOctets();
    int index1 = 3;
    while (asn1Sequence2[index1] is Asn1Tag)
      ++index1;
    Asn1Sequence asn1Sequence5 = (Asn1Sequence) ((Asn1Set) asn1Sequence2[index1])[0];
    this.m_signerVersion = ((DerInteger) asn1Sequence5[0]).Value.IntValue;
    Asn1Sequence asn1Sequence6 = (Asn1Sequence) asn1Sequence5[1];
    X509Name name = X509Name.GetName((object) asn1Sequence6[0]);
    Number number = ((DerInteger) asn1Sequence6[1]).Value;
    foreach (X509Certificate x509Certificate in (IEnumerable<X509Certificate>) certs)
    {
      if (name.Equivalent(x509Certificate.IssuerDN) && number.Equals((object) x509Certificate.SerialNumber))
      {
        this.m_signCert = x509Certificate;
        break;
      }
    }
    if (this.m_signCert == null)
      return;
    this.m_certificates = this.GetCertificateChain(this.m_signCert, certs);
    this.m_digestAlgorithmOid = ((DerObjectID) ((Asn1Sequence) asn1Sequence5[2])[0]).ID;
    int index2 = 3;
    if (asn1Sequence5[index2] is Asn1Tag)
    {
      Asn1Set asn1Set = Asn1Set.GetAsn1Set((Asn1Tag) asn1Sequence5[index2], false);
      this.m_sigAttribute = asn1Set.GetEncoded();
      this.m_sigAttributeDer = asn1Set.GetEncoded("DER");
      for (int index3 = 0; index3 < asn1Set.Count; ++index3)
      {
        Asn1Sequence asn1Sequence7 = (Asn1Sequence) asn1Set[index3];
        string id = ((DerObjectID) asn1Sequence7[0]).ID;
        if (id.Equals(PKCSOIDs.Pkcs9AtMessageDigest.ID))
          this.m_digestAttributeData = ((Asn1Octet) ((Asn1Set) asn1Sequence7[1])[0]).GetOctets();
        else if (id.Equals(PKCSOIDs.AdobeRevocation.ID))
        {
          Asn1Sequence asn1Sequence8 = (Asn1Sequence) ((Asn1Set) asn1Sequence7[1])[0];
          for (int index4 = 0; index4 < asn1Sequence8.Count; ++index4)
          {
            Asn1Tag asn1Tag = (Asn1Tag) asn1Sequence8[index4];
            if (asn1Tag.TagNumber == 1)
              this.GetOcsp((Asn1Sequence) asn1Tag.GetObject());
          }
        }
      }
      ++index2;
    }
    Asn1Sequence asn1Sequence9 = asn1Sequence5;
    int index5 = index2;
    int num2 = index5 + 1;
    this.m_encryptionAlgorithmOid = ((DerObjectID) ((Asn1Sequence) asn1Sequence9[index5])[0]).ID;
    Asn1Sequence asn1Sequence10 = asn1Sequence5;
    int index6 = num2;
    int index7 = index6 + 1;
    this.m_digest = ((Asn1Octet) asn1Sequence10[index6]).GetOctets();
    if (index7 < asn1Sequence5.Count && asn1Sequence5[index7] is DerTag)
    {
      TimeStampElement timeStampElement = new TimeStampElements(Asn1Set.GetAsn1Set((Asn1Tag) asn1Sequence5[index7], false))[PKCSOIDs.Pkcs9SignatureTimeStamp];
      if (timeStampElement != null && timeStampElement.Values.Count > 0)
        this.m_timeStampToken = new TimeStampToken(new CmsSignedDetails(ContentInformation.GetInformation((object) Asn1Sequence.GetSequence((object) timeStampElement.Values[0]))));
    }
    if (this.m_isTimeStamp)
    {
      this.m_timeStampToken = new TimeStampToken(new CmsSignedDetails(ContentInformation.GetInformation((object) asn1Sequence1)));
      this.m_messageDigest = this.m_digestFinder.GetDigest(this.m_timeStampToken.TimeStampInformation.MessageImprintAlgOid);
    }
    else
    {
      if (this.m_rsaData != null || this.m_digestAttributeData != null)
      {
        this.m_messageDigest = this.m_digestFinder.GetDigest(this.HashAlgorithm);
        this.m_encodedMessageDigest = this.MessageDigest;
      }
      this.m_signer = this.InitializeSignature();
    }
  }

  internal IMessageDigest MessageDigest => this.m_digestFinder.GetDigest(this.HashAlgorithm);

  internal string HashAlgorithm
  {
    get
    {
      if (this.m_hashAlgorithm == null)
        this.m_hashAlgorithm = new MessageDigestAlgorithms().GetDigest(this.m_digestAlgorithmOid);
      return this.m_hashAlgorithm;
    }
  }

  internal void SetSignedData(byte[] digest, byte[] RSAdata, string digestEncryptionAlgorithm)
  {
    this.m_signedData = digest;
    this.m_signedRSAData = RSAdata;
    switch (digestEncryptionAlgorithm)
    {
      case null:
        break;
      case "RSA":
        this.m_encryptionAlgorithmOid = "1.2.840.113549.1.1.1";
        break;
      case "DSA":
        this.m_encryptionAlgorithmOid = "1.2.840.10040.4.1";
        break;
      case "ECDSA":
        this.m_encryptionAlgorithmOid = "1.2.840.10045.2.1";
        break;
      default:
        throw new ArgumentException("Invalid entry");
    }
  }

  internal byte[] Sign(
    byte[] secondDigest,
    TimeStampServer server,
    byte[] timeStampResponse,
    byte[] ocsp,
    ICollection<byte[]> crls,
    CryptographicStandard sigtype,
    string hashAlgorithm)
  {
    if (this.m_signedData != null)
    {
      this.m_digest = this.m_signedData;
      if (this.m_rsaData != null)
        this.m_rsaData = this.m_signedRSAData;
    }
    else if (this.m_signedRSAData != null && this.m_rsaData != null)
    {
      this.m_rsaData = this.m_signedRSAData;
      this.m_signer.BlockUpdate(this.m_rsaData, 0, this.m_rsaData.Length);
      this.m_digest = this.m_signer.GenerateSignature();
    }
    else
    {
      if (this.m_rsaData != null)
      {
        this.m_rsaData = new byte[this.m_messageDigest.MessageDigestSize];
        this.m_messageDigest.DoFinal(this.m_rsaData, 0);
        this.m_signer.BlockUpdate(this.m_rsaData, 0, this.m_rsaData.Length);
      }
      this.m_digest = this.m_signer.GenerateSignature();
    }
    Asn1EncodeCollection collection1 = new Asn1EncodeCollection(new Asn1Encode[0]);
    foreach (string key in this.m_digestOid.Keys)
      collection1.Add((Asn1Encode) new DerSequence(new Asn1EncodeCollection(new Asn1Encode[0])
      {
        new Asn1Encode[1]{ (Asn1Encode) new DerObjectID(key) },
        new Asn1Encode[1]{ (Asn1Encode) DerNull.Value }
      }));
    Asn1EncodeCollection collection2 = new Asn1EncodeCollection(new Asn1Encode[0]);
    collection2.Add((Asn1Encode) new DerObjectID("1.2.840.113549.1.7.1"));
    if (this.m_rsaData != null)
      collection2.Add((Asn1Encode) new DerTag(0, (Asn1Encode) new DerOctet(this.m_rsaData)));
    DerSequence derSequence = new DerSequence(collection2);
    Asn1EncodeCollection collection3 = new Asn1EncodeCollection(new Asn1Encode[0]);
    foreach (X509Certificate certificate in this.m_certificates)
    {
      Asn1Stream asn1Stream = new Asn1Stream((Stream) new MemoryStream(certificate.GetEncoded()));
      collection3.Add((Asn1Encode) asn1Stream.ReadAsn1());
    }
    DerSet asn1_1 = new DerSet(collection3);
    Asn1EncodeCollection collection4 = new Asn1EncodeCollection(new Asn1Encode[0]);
    collection4.Add((Asn1Encode) new DerInteger(this.m_signerVersion));
    collection4.Add((Asn1Encode) new DerSequence(new Asn1EncodeCollection(new Asn1Encode[0])
    {
      new Asn1Encode[1]
      {
        (Asn1Encode) this.GetIssuer(this.m_signCert.GetTbsCertificate())
      },
      new Asn1Encode[1]
      {
        (Asn1Encode) new DerInteger(this.m_signCert.SerialNumber)
      }
    }));
    collection4.Add((Asn1Encode) new DerSequence(new Asn1EncodeCollection(new Asn1Encode[0])
    {
      new Asn1Encode[1]
      {
        (Asn1Encode) new DerObjectID(this.m_digestAlgorithmOid)
      },
      new Asn1Encode[1]{ (Asn1Encode) DerNull.Value }
    }));
    if (secondDigest != null)
      collection4.Add((Asn1Encode) new DerTag(false, 0, (Asn1Encode) this.GetSequenceDataSet(secondDigest, ocsp, crls, sigtype)));
    collection4.Add((Asn1Encode) new DerSequence(new Asn1EncodeCollection(new Asn1Encode[0])
    {
      new Asn1Encode[1]
      {
        (Asn1Encode) new DerObjectID(this.m_encryptionAlgorithmOid)
      },
      new Asn1Encode[1]{ (Asn1Encode) DerNull.Value }
    }));
    collection4.Add((Asn1Encode) new DerOctet(this.m_digest));
    if (timeStampResponse == null && server != null)
    {
      byte[] timestampRequest = new TimeStampRequestCreator(true).GetAsnEncodedTimestampRequest(new MessageDigestAlgorithms().Digest((Stream) new MemoryStream(this.m_digest), "SHA256"));
      timeStampResponse = server.GetTimeStampResponse(timestampRequest);
      Asn1 asn1_2 = (new Asn1Stream(timeStampResponse).ReadAsn1() as Asn1Sequence)[1] as Asn1;
      MemoryStream memoryStream = new MemoryStream();
      DerStream derOut = new DerStream((Stream) memoryStream);
      asn1_2.Encode(derOut);
      timeStampResponse = memoryStream.ToArray();
      derOut.m_stream.Dispose();
      memoryStream.Dispose();
    }
    if (timeStampResponse != null)
    {
      Asn1EncodeCollection attributes = this.GetAttributes(timeStampResponse);
      if (attributes != null)
        collection4.Add((Asn1Encode) new DerTag(false, 1, (Asn1Encode) new DerSet(attributes)));
    }
    Asn1EncodeCollection collection5 = new Asn1EncodeCollection(new Asn1Encode[0]);
    collection5.Add((Asn1Encode) new DerInteger(this.m_version));
    collection5.Add((Asn1Encode) new DerSet(collection1));
    collection5.Add((Asn1Encode) derSequence);
    collection5.Add((Asn1Encode) new DerTag(false, 0, (Asn1Encode) asn1_1));
    collection5.Add((Asn1Encode) new DerSet(new Asn1Encode[1]
    {
      (Asn1Encode) new DerSequence(collection4)
    }));
    Asn1EncodeCollection collection6 = new Asn1EncodeCollection(new Asn1Encode[0]);
    collection6.Add((Asn1Encode) new DerObjectID("1.2.840.113549.1.7.2"));
    collection6.Add((Asn1Encode) new DerTag(0, (Asn1Encode) new DerSequence(collection5)));
    MemoryStream memoryStream1 = new MemoryStream();
    Asn1DerStream asn1DerStream = new Asn1DerStream((Stream) memoryStream1);
    asn1DerStream.WriteObject((object) new DerSequence(collection6));
    ((DerStream) asn1DerStream).m_stream.Close();
    return memoryStream1.ToArray();
  }

  internal byte[] GetEncodedTimestamp(byte[] secondDigest, TimeStampServer server)
  {
    byte[] encodedTimestamp = (byte[]) null;
    if (server != null)
    {
      byte[] timestampRequest = new TimeStampRequestCreator(true).GetAsnEncodedTimestampRequest(secondDigest);
      Asn1 asn1 = (new Asn1Stream(server.GetTimeStampResponse(timestampRequest)).ReadAsn1() as Asn1Sequence)[1] as Asn1;
      MemoryStream memoryStream = new MemoryStream();
      DerStream derOut = new DerStream((Stream) memoryStream);
      derOut.WriteObject((object) asn1);
      asn1.Encode(derOut);
      encodedTimestamp = memoryStream.ToArray();
      derOut.m_stream.Dispose();
      memoryStream.Dispose();
    }
    return encodedTimestamp;
  }

  private Asn1EncodeCollection GetAttributes(byte[] timeStampToken)
  {
    if (timeStampToken == null)
      return (Asn1EncodeCollection) null;
    Asn1Stream asn1Stream = new Asn1Stream((Stream) new MemoryStream(timeStampToken));
    Asn1EncodeCollection attributes = new Asn1EncodeCollection(new Asn1Encode[0]);
    Asn1EncodeCollection collection = new Asn1EncodeCollection(new Asn1Encode[0]);
    collection.Add((Asn1Encode) new DerObjectID("1.2.840.113549.1.9.16.2.14"));
    Asn1Sequence asn1Sequence = (Asn1Sequence) asn1Stream.ReadAsn1();
    collection.Add((Asn1Encode) new DerSet(new Asn1Encode[1]
    {
      (Asn1Encode) asn1Sequence
    }));
    attributes.Add((Asn1Encode) new DerSequence(collection));
    return attributes;
  }

  private Asn1 GetIssuer(byte[] data)
  {
    Asn1Sequence asn1Sequence = (Asn1Sequence) new Asn1Stream((Stream) new MemoryStream(data)).ReadAsn1();
    return (Asn1) asn1Sequence[asn1Sequence[0] is Asn1Tag ? 3 : 2];
  }

  internal byte[] GetSequenceData(
    byte[] secondDigest,
    byte[] ocsp,
    ICollection<byte[]> crlBytes,
    CryptographicStandard sigtype)
  {
    return this.GetSequenceDataSet(secondDigest, ocsp, crlBytes, sigtype).GetEncoded("DER");
  }

  private DerSet GetSequenceDataSet(
    byte[] secondDigest,
    byte[] ocsp,
    ICollection<byte[]> crlBytes,
    CryptographicStandard sigtype)
  {
    Asn1EncodeCollection collection1 = new Asn1EncodeCollection(new Asn1Encode[0]);
    collection1.Add((Asn1Encode) new DerSequence(new Asn1EncodeCollection(new Asn1Encode[0])
    {
      new Asn1Encode[1]
      {
        (Asn1Encode) new DerObjectID("1.2.840.113549.1.9.3")
      },
      new Asn1Encode[1]
      {
        (Asn1Encode) new DerSet(new Asn1Encode[1]
        {
          (Asn1Encode) new DerObjectID("1.2.840.113549.1.7.1")
        })
      }
    }));
    collection1.Add((Asn1Encode) new DerSequence(new Asn1EncodeCollection(new Asn1Encode[0])
    {
      new Asn1Encode[1]
      {
        (Asn1Encode) new DerObjectID("1.2.840.113549.1.9.4")
      },
      new Asn1Encode[1]
      {
        (Asn1Encode) new DerSet(new Asn1Encode[1]
        {
          (Asn1Encode) new DerOctet(secondDigest)
        })
      }
    }));
    bool flag = false;
    if (crlBytes != null)
    {
      foreach (byte[] crlByte in (IEnumerable<byte[]>) crlBytes)
      {
        if (crlByte != null)
        {
          flag = true;
          break;
        }
      }
    }
    if (ocsp != null || flag)
    {
      Asn1EncodeCollection collection2 = new Asn1EncodeCollection(new Asn1Encode[0]);
      collection2.Add((Asn1Encode) new DerObjectID("1.2.840.113583.1.1.8"));
      Asn1EncodeCollection collection3 = new Asn1EncodeCollection(new Asn1Encode[0]);
      if (flag)
      {
        Asn1EncodeCollection collection4 = new Asn1EncodeCollection(new Asn1Encode[0]);
        foreach (byte[] crlByte in (IEnumerable<byte[]>) crlBytes)
        {
          if (crlByte != null)
          {
            Asn1Stream asn1Stream = new Asn1Stream(crlByte);
            collection4.Add((Asn1Encode) asn1Stream.ReadAsn1());
          }
        }
        collection3.Add((Asn1Encode) new DerTag(true, 0, (Asn1Encode) new DerSequence(collection4)));
      }
      if (ocsp != null)
      {
        DerOctet derOctet = new DerOctet(ocsp);
        Asn1EncodeCollection collection5 = new Asn1EncodeCollection(new Asn1Encode[0]);
        Asn1EncodeCollection collection6 = new Asn1EncodeCollection(new Asn1Encode[0]);
        collection6.Add((Asn1Encode) OcspConstants.OcspBasic);
        collection6.Add((Asn1Encode) derOctet);
        DerCatalogue derCatalogue = new DerCatalogue(0);
        collection5.Add((Asn1Encode) new DerSequence(new Asn1EncodeCollection(new Asn1Encode[0])
        {
          new Asn1Encode[1]{ (Asn1Encode) derCatalogue },
          new Asn1Encode[1]
          {
            (Asn1Encode) new DerTag(true, 0, (Asn1Encode) new DerSequence(collection6))
          }
        }));
        collection3.Add((Asn1Encode) new DerTag(true, 1, (Asn1Encode) new DerSequence(collection5)));
      }
      collection2.Add((Asn1Encode) new DerSet(new Asn1Encode[1]
      {
        (Asn1Encode) new DerSequence(collection3)
      }));
      collection1.Add((Asn1Encode) new DerSequence(collection2));
    }
    if (this.m_signCert != null && sigtype == CryptographicStandard.CADES)
    {
      Asn1EncodeCollection collection7 = new Asn1EncodeCollection(new Asn1Encode[0]);
      collection7.Add((Asn1Encode) new DerObjectID("1.2.840.113549.1.9.16.2.47"));
      Asn1EncodeCollection collection8 = new Asn1EncodeCollection(new Asn1Encode[0]);
      MessageDigestAlgorithms digestAlgorithms = new MessageDigestAlgorithms();
      if (!digestAlgorithms.GetAllowedDigests("SHA-256").Equals(this.m_digestAlgorithmOid))
      {
        Algorithms algorithms = new Algorithms(new DerObjectID(this.m_digestAlgorithmOid));
        collection8.Add((Asn1Encode) algorithms);
      }
      byte[] bytes = digestAlgorithms.Digest(this.HashAlgorithm, this.m_signCert.GetEncoded());
      collection8.Add((Asn1Encode) new DerOctet(bytes));
      collection7.Add((Asn1Encode) new DerSet(new Asn1Encode[1]
      {
        (Asn1Encode) new DerSequence((Asn1Encode) new DerSequence((Asn1Encode) new DerSequence(collection8)))
      }));
      collection1.Add((Asn1Encode) new DerSequence(collection7));
    }
    return new DerSet(collection1);
  }

  internal string MessageDigestAlgorithm => $"{this.HashAlgorithm}with{this.EncryptionAlgorithm}";

  public string EncryptionAlgorithm
  {
    get
    {
      if (this.m_encryptionAlgorithm == null)
        this.m_encryptionAlgorithm = new EncryptionAlgorithms().GetAlgorithm(this.m_encryptionAlgorithmOid);
      return this.m_encryptionAlgorithm;
    }
  }

  internal void Update(byte[] bytes, int offset, int length)
  {
    if (this.m_rsaData != null || this.m_digestAttributeData != null || this.m_isTimeStamp)
      this.m_messageDigest.BlockUpdate(bytes, offset, length);
    else
      this.m_signer.BlockUpdate(bytes, offset, length);
  }

  private List<X509Certificate> GetCertificateChain(
    X509Certificate signCert,
    IList<X509Certificate> certs)
  {
    List<X509Certificate> certificateChain = new List<X509Certificate>();
    certificateChain.Add(signCert);
    List<X509Certificate> x509CertificateList = new List<X509Certificate>((IEnumerable<X509Certificate>) certs);
    for (int index = 0; index < x509CertificateList.Count; ++index)
    {
      if (signCert.Equals((object) x509CertificateList[index]))
      {
        x509CertificateList.RemoveAt(index);
        --index;
      }
    }
    bool flag = true;
    while (flag)
    {
      X509Certificate x509Certificate1 = certificateChain[certificateChain.Count - 1];
      flag = false;
      for (int index = 0; index < x509CertificateList.Count; ++index)
      {
        X509Certificate x509Certificate2 = x509CertificateList[index];
        try
        {
          x509Certificate1.Verify(x509Certificate2.GetPublicKey());
          flag = true;
          certificateChain.Add(x509Certificate2);
          x509CertificateList.RemoveAt(index);
          break;
        }
        catch
        {
        }
      }
    }
    return certificateChain;
  }

  internal bool ValidateCertificateWithCollection(
    List<X509Certificate> collection,
    DateTime signDate,
    PdfSignatureValidationResult signatureResult)
  {
    List<PdfSignatureValidationException> validationExceptionList = new List<PdfSignatureValidationException>();
    if (this.m_certificates != null && this.m_certificates.Count > 0)
    {
      for (int index1 = 0; index1 < this.m_certificates.Count; ++index1)
      {
        X509Certificate certificate1 = this.m_certificates[index1];
        foreach (X509Certificate certificate2 in collection)
        {
          if (this.ValidateCertificates(certificate2, signDate))
          {
            try
            {
              certificate1.Verify(certificate2.GetPublicKey());
              return true;
            }
            catch (Exception ex)
            {
            }
          }
        }
        int index2;
        for (index2 = 0; index2 < this.m_certificates.Count; ++index2)
        {
          if (index2 != index1)
          {
            X509Certificate certificate3 = this.m_certificates[index2];
            try
            {
              certificate1.Verify(certificate3.GetPublicKey());
              break;
            }
            catch
            {
            }
          }
        }
        if (index2 == this.m_certificates.Count)
          signatureResult.SignatureValidationErrors.Add(new PdfSignatureValidationException("Cannot be verified against the KeyStore or the certificate chain"));
      }
    }
    return false;
  }

  internal List<string> GetSupportedOids()
  {
    return new List<string>()
    {
      X509Extensions.KeyUsage.ID,
      X509Extensions.CertificatePolicies.ID,
      X509Extensions.PolicyMappings.ID,
      X509Extensions.InhibitAnyPolicy.ID,
      X509Extensions.CrlDistributionPoints.ID,
      X509Extensions.IssuingDistributionPoint.ID,
      X509Extensions.DeltaCrlIndicator.ID,
      X509Extensions.PolicyConstraints.ID,
      X509Extensions.BasicConstraints.ID,
      X509Extensions.SubjectAlternativeName.ID,
      X509Extensions.NameConstraints.ID
    };
  }

  private bool IsSupportedOid(List<string> oids, X509Certificate certificate)
  {
    string str = "1.3.6.1.5.5.7.3.8";
    List<string> supportedOids = this.GetSupportedOids();
    if (oids != null && supportedOids != null)
    {
      foreach (string oid in oids)
      {
        if (!supportedOids.Contains(oid) && (oid != X509Extensions.ExtendedKeyUsage.ID || !certificate.GetExtendedKeyUsage().Contains((object) str)))
          return false;
      }
    }
    return true;
  }

  private bool ValidateCertificates(X509Certificate certificate, DateTime signDate)
  {
    return this.IsSupportedOid(certificate.GetOids(true), certificate) && certificate.IsValid(signDate.ToUniversalTime());
  }

  internal X509Certificate2Collection GetCertificates()
  {
    X509Certificate2Collection certificates = new X509Certificate2Collection();
    if (this.m_certificates != null && this.m_certificates.Count > 0)
    {
      foreach (X509Certificate certificate in this.m_certificates)
        certificates.Add(new X509Certificate2(certificate.GetEncoded()));
    }
    return certificates;
  }

  internal bool ValidateChecksum()
  {
    bool flag1;
    if (!this.m_isTimeStamp)
    {
      if (this.m_sigAttribute != null || this.m_sigAttributeDer != null)
      {
        byte[] numArray1 = new byte[this.m_messageDigest.MessageDigestSize];
        this.m_messageDigest.DoFinal(numArray1, 0);
        bool flag2 = true;
        bool flag3 = false;
        if (this.m_rsaData != null)
        {
          flag2 = object.Equals((object) numArray1, (object) this.m_rsaData);
          this.m_encodedMessageDigest.BlockUpdate(this.m_rsaData, 0, this.m_rsaData.Length);
          byte[] numArray2 = new byte[this.m_encodedMessageDigest.MessageDigestSize];
          this.m_encodedMessageDigest.DoFinal(numArray2, 0);
          flag3 = object.Equals((object) numArray2, (object) this.m_digestAttributeData);
        }
        int index = 0;
        bool flag4 = true;
        if (numArray1.Length == this.m_digestAttributeData.Length)
        {
          for (; index < numArray1.Length; ++index)
          {
            if ((int) numArray1[index] != (int) this.m_digestAttributeData[index])
            {
              flag4 = false;
              break;
            }
          }
        }
        else
          flag4 = false;
        flag1 = (flag4 || flag3) && (this.ValidateAttributes(this.m_sigAttribute) || this.ValidateAttributes(this.m_sigAttributeDer)) && flag2;
      }
      else
      {
        if (this.m_rsaData != null)
        {
          byte[] bytes = new byte[this.m_messageDigest.MessageDigestSize];
          this.m_messageDigest.DoFinal(bytes, 0);
          this.m_signer.BlockUpdate(bytes, 0, bytes.Length);
        }
        flag1 = this.m_signer.ValidateSignature(this.m_digest);
      }
    }
    else
      flag1 = true;
    return flag1;
  }

  internal RevocationResult CheckRevocation(
    DateTime signedDate,
    PdfSignatureValidationResult validationResult)
  {
    if (this.m_certificates == null)
      return (RevocationResult) null;
    RevocationResult result = new RevocationResult();
    X509Certificate certificate1 = this.m_certificates[0];
    X509Certificate certificate2 = this.m_certificates.Count > 1 ? this.m_certificates[1] : (X509Certificate) null;
    List<X509RevocationResponse> responses = new List<X509RevocationResponse>();
    if (this.m_basicOcspResponse != null)
      responses.Add(this.m_basicOcspResponse);
    OcspValidator ocspValidator = new OcspValidator(responses);
    result.OcspRevocationStatus = ocspValidator.Validate(certificate1, certificate2, signedDate);
    if (result.OcspRevocationStatus == RevocationStatus.Revoked)
      validationResult.SignatureValidationErrors.Add(new PdfSignatureValidationException("The certificate is considered invalid because it has been revoked as verified using Online Certificate Status Protocol that was embedded in the document"));
    RevocationListValidator revocationListValidator = new RevocationListValidator(certificate1, result);
    result.IsRevokedCRL = revocationListValidator.Validate(certificate1, certificate2, signedDate);
    if (result.IsRevokedCRL)
      validationResult.SignatureValidationErrors.Add(new PdfSignatureValidationException("The certificate is considered invalid because it has been revoked as verified using CRL that was embedded in the document"));
    if (result.OcspRevocationStatus == RevocationStatus.Good || ocspValidator.m_ocspResposne || !ocspValidator.m_ocspResposne && !result.IsRevokedCRL)
      validationResult.m_isValidOCSPorCRLtimeValidation = true;
    return result;
  }

  private bool ValidateAttributes(byte[] attr)
  {
    ISigner signer = this.InitializeSignature();
    signer.BlockUpdate(attr, 0, attr.Length);
    return signer.ValidateSignature(this.m_digest);
  }

  internal TimeStampInformation ValidateTimestamp()
  {
    if (this.m_timeStampToken == null)
      return (TimeStampInformation) null;
    TimeStampInformation stampInformation1 = new TimeStampInformation();
    TimeStampTokenInformation stampInformation2 = this.m_timeStampToken.TimeStampInformation;
    stampInformation1.MessageImprintAlgorithmId = stampInformation2.MessageImprintAlgOid;
    stampInformation1.TimeStampPolicyId = stampInformation2.Policy;
    stampInformation1.Time = stampInformation2.GeneralizedTime;
    stampInformation1.SignerInformation = (object) stampInformation2.TimeStampData;
    MessageStamp messageImprint = stampInformation2.TimeStampData.MessageImprint;
    byte[] numArray = this.m_digestAlgorithm.Digest(stampInformation2.MessageImprintAlgOid, this.m_digest);
    byte[] hashedMessage = messageImprint.HashedMessage;
    if (numArray == hashedMessage)
    {
      stampInformation1.IsValid = true;
      return stampInformation1;
    }
    if (numArray == null || hashedMessage == null)
    {
      stampInformation1.IsValid = false;
      return stampInformation1;
    }
    int length = numArray.Length;
    if (length != hashedMessage.Length)
    {
      stampInformation1.IsValid = false;
      return stampInformation1;
    }
    while (length != 0)
    {
      --length;
      if ((int) numArray[length] != (int) hashedMessage[length])
      {
        stampInformation1.IsValid = false;
        return stampInformation1;
      }
    }
    return stampInformation1;
  }

  private ISigner InitializeSignature()
  {
    ISigner signer = new SignerUtilities().GetSigner(this.MessageDigestAlgorithm);
    signer.Initialize(false, (ICipherParam) this.m_signCert.GetPublicKey());
    return signer;
  }

  private void GetOcsp(Asn1Sequence sequence)
  {
    this.m_basicOcspResponse = (X509RevocationResponse) null;
    while (!(sequence[0] is DerObjectID) || !(((DerObjectID) sequence[0]).ID == this.m_ocspID))
    {
      bool flag = true;
      for (int index = 0; index < sequence.Count; ++index)
      {
        if (sequence[index] is Asn1Sequence)
        {
          sequence = (Asn1Sequence) sequence[0];
          flag = false;
          break;
        }
        if (sequence[index] is Asn1Tag)
        {
          Asn1Tag asn1Tag = (Asn1Tag) sequence[index];
          if (!(asn1Tag.GetObject() is Asn1Sequence))
            return;
          sequence = (Asn1Sequence) asn1Tag.GetObject();
          flag = false;
          break;
        }
      }
      if (flag)
        return;
    }
    this.m_basicOcspResponse = new X509RevocationResponse(new OcspHelper().GetOcspStructure((object) new Asn1Stream(((Asn1Octet) sequence[1]).GetOctets()).ReadAsn1()));
  }
}
