// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.PdfSignatureDictionary
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class PdfSignatureDictionary : IPdfWrapper
{
  private const string c_Type = "Sig";
  private const string ts_Type = "DocTimeStamp";
  private const string c_cmsFilterType = "adbe.pkcs7.detached";
  internal const string CadasFilterType = "ETSI.CAdES.detached";
  private const string rfc_FilterType = "ETSI.RFC3161";
  internal const string StoreFilterType = "adbe.pkcs7.sha1";
  private const string c_DocMdp = "DocMDP";
  private const string c_TransParam = "TransformParams";
  private uint c_EstimatedSize = 8192 /*0x2000*/;
  private PdfDocumentBase m_doc;
  private PdfSignature m_sig;
  private PdfCertificate m_cert;
  private int m_firstRangeLength;
  private int m_secondRangeIndex;
  private int m_startPositionByteRange;
  private int m_docDigestPosition;
  private int m_fieldsDigestPosition;
  private PdfDictionary m_dictionary = new PdfDictionary();
  private bool m_isEndCertOnly = true;
  private Stream m_stream;
  private long[] m_range = new long[4];

  public bool Archive
  {
    get => this.m_dictionary.Archive;
    set => this.m_dictionary.Archive = value;
  }

  internal PdfSignatureDictionary(PdfDocumentBase doc, PdfSignature sig, PdfCertificate cert)
  {
    if (doc == null)
      throw new ArgumentNullException(nameof (doc));
    if (sig == null)
      throw new ArgumentNullException(nameof (sig));
    this.m_doc = doc;
    this.m_sig = sig;
    this.m_cert = cert;
    doc.DocumentSaved += new PdfDocumentBase.DocumentSavedEventHandler(this.DocumentSaved);
    this.m_dictionary.BeginSave += new SavePdfPrimitiveEventHandler(this.Dictionary_BeginSave);
  }

  internal PdfSignatureDictionary(PdfDocumentBase doc, PdfDictionary dic)
  {
    this.m_doc = doc != null ? doc : throw new ArgumentNullException(nameof (doc));
    this.m_dictionary = dic;
    doc.DocumentSaved += new PdfDocumentBase.DocumentSavedEventHandler(this.DocumentSaved);
    this.m_dictionary.BeginSave += new SavePdfPrimitiveEventHandler(this.Dictionary_BeginSave);
  }

  internal PdfSignatureDictionary(PdfDocumentBase doc, PdfSignature sig)
  {
    if (doc == null)
      throw new ArgumentNullException(nameof (doc));
    if (sig == null)
      throw new ArgumentNullException(nameof (sig));
    this.m_doc = doc;
    this.m_sig = sig;
    doc.DocumentSaved += new PdfDocumentBase.DocumentSavedEventHandler(this.DocumentSaved);
    this.m_dictionary.BeginSave += new SavePdfPrimitiveEventHandler(this.Dictionary_BeginSave);
  }

  private void AddRequiredItems()
  {
    if (this.m_sig.Certificated && this.AllowMDP())
      this.AddReference();
    this.AddType();
    this.AddDate();
    this.AddFilter();
    this.AddSubFilter();
  }

  private void AddOptionalItems()
  {
    this.AddReason();
    this.AddLocation();
    this.AddContactInfo();
    this.AddSignedName();
  }

  private void AddLocation()
  {
    if (this.m_sig.LocationInfo == null)
      return;
    this.m_dictionary.SetProperty("Location", (IPdfPrimitive) new PdfString(this.m_sig.LocationInfo));
  }

  private void AddContactInfo()
  {
    if (this.m_sig.ContactInfo == null)
      return;
    this.m_dictionary.SetProperty("ContactInfo", (IPdfPrimitive) new PdfString(this.m_sig.ContactInfo));
  }

  private void AddType()
  {
    if (this.m_cert != null)
      this.m_dictionary.SetName("Type", "Sig");
    else if (this.m_sig.TimeStampServer != null && this.m_sig.isTimeStampOnly)
      this.m_dictionary.SetName("Type", "DocTimeStamp");
    else
      this.m_dictionary.SetName("Type", "Sig");
  }

  private void AddSignedName()
  {
    if (this.m_sig == null || this.m_sig.m_signedName == null)
      return;
    this.m_dictionary.SetString("Name", this.m_sig.m_signedName);
    PdfDictionary pdfDictionary1 = new PdfDictionary();
    PdfDictionary pdfDictionary2 = new PdfDictionary();
    pdfDictionary1.SetName("Name", this.m_sig.m_signedName);
    pdfDictionary2.SetProperty("App", (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary1));
    this.m_dictionary.SetProperty(new PdfName("Prop_Build"), (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary2));
  }

  private void AddDate()
  {
    string str1 = $"D:{DateTime.Now:yyyyMMddHHmmss}";
    string str2 = DateTimeOffset.Now.ToString();
    char[] charArray = str2.Substring(str2.Length - 6).ToCharArray();
    this.m_dictionary.SetProperty("M", (IPdfPrimitive) new PdfString(str1 + (object) charArray[0] + (object) charArray[1] + (object) charArray[2] + "'" + (object) charArray[4] + (object) charArray[5] + "'"));
  }

  private void AddReason()
  {
    if (this.m_sig.Reason == null)
      return;
    this.m_dictionary.SetProperty("Reason", (IPdfPrimitive) new PdfString(this.m_sig.Reason));
  }

  private void AddFilter() => this.m_dictionary.SetName("Filter", "Adobe.PPKLite");

  private void AddSubFilter()
  {
    if (this.m_cert != null)
    {
      if (this.m_cert.isStore)
      {
        if (!this.m_sig.Settings.HasChanged)
          this.m_sig.Settings.DigestAlgorithm = DigestAlgorithm.SHA1;
        if (this.m_sig.Settings.DigestAlgorithm != DigestAlgorithm.SHA1 && this.CheckExportable() || this.m_sig.TimeStampServer != null || this.m_sig.Settings.CryptographicStandard == CryptographicStandard.CADES)
          this.m_dictionary.SetName("SubFilter", this.m_sig.Settings.CryptographicStandard == CryptographicStandard.CADES ? "ETSI.CAdES.detached" : "adbe.pkcs7.detached");
        else
          this.m_dictionary.SetName("SubFilter", this.m_sig.Settings.CryptographicStandard == CryptographicStandard.CADES ? "ETSI.CAdES.detached" : "adbe.pkcs7.sha1");
      }
      else
        this.m_dictionary.SetName("SubFilter", this.m_sig.Settings.CryptographicStandard == CryptographicStandard.CADES ? "ETSI.CAdES.detached" : "adbe.pkcs7.detached");
    }
    else if (this.m_sig.TimeStampServer != null && this.m_sig.isTimeStampOnly)
      this.m_dictionary.SetName("SubFilter", "ETSI.RFC3161");
    else
      this.m_dictionary.SetName("SubFilter", this.m_sig.Settings.CryptographicStandard == CryptographicStandard.CADES ? "ETSI.CAdES.detached" : "adbe.pkcs7.detached");
  }

  private void AddContents(IPdfWriter writer)
  {
    writer.Write("/Contents ");
    this.m_firstRangeLength = (int) writer.Position;
    if (this.m_sig != null && this.m_sig.EstimatedSignatureSize != 0U)
      this.c_EstimatedSize = this.m_sig.EstimatedSignatureSize;
    uint num = this.c_EstimatedSize * 2U;
    if (this.m_cert != null && this.m_sig != null && this.m_sig.TimeStampServer == null)
      num = this.c_EstimatedSize;
    if (this.m_sig != null && this.m_sig.ExternalSigner != null)
    {
      if (this.m_sig.CRLBytes != null)
      {
        foreach (byte[] crlByte in (IEnumerable<byte[]>) this.m_sig.CRLBytes)
          num += (uint) (crlByte.Length + 10);
      }
      if (this.m_sig.OCSP != null)
        num += 4192U;
      num += 4192U;
    }
    byte[] data = new byte[(IntPtr) (uint) ((int) num * 2 + 2)];
    writer.Write(data);
    this.m_secondRangeIndex = (int) writer.Position;
    writer.Write("\r\n");
  }

  private void AddRange(IPdfWriter writer)
  {
    writer.Write("/ByteRange [");
    this.m_startPositionByteRange = (int) writer.Position;
    for (int index = 0; index < 32 /*0x20*/; ++index)
      writer.Write(" ");
    writer.Write("]\r\n");
  }

  private bool CheckExportable()
  {
    try
    {
      return (this.m_cert.X509Certificate.PrivateKey as RSACryptoServiceProvider).CspKeyContainerInfo.Exportable;
    }
    catch
    {
      return false;
    }
  }

  private bool AllowMDP()
  {
    return this.m_dictionary.Equals((object) PdfCrossTable.Dereference((PdfCrossTable.Dereference(this.m_doc.Catalog["Perms"]) as PdfDictionary)["DocMDP"]));
  }

  private void AddDigest(IPdfWriter writer)
  {
    if (!this.AllowMDP())
      return;
    PdfDictionary catalog = (PdfDictionary) writer.Document.Catalog;
    writer.Write((IPdfPrimitive) new PdfName("Reference"));
    writer.Write("[");
    writer.Write("<<");
    writer.Write("/TransformParams");
    PdfDictionary pdfObject1 = new PdfDictionary();
    int documentPermissions = (int) this.m_sig.DocumentPermissions;
    pdfObject1["V"] = (IPdfPrimitive) new PdfName("1.2");
    pdfObject1["P"] = (IPdfPrimitive) new PdfNumber(documentPermissions);
    pdfObject1["Type"] = (IPdfPrimitive) new PdfName("TransformParams");
    writer.Write((IPdfPrimitive) pdfObject1);
    writer.Write((IPdfPrimitive) new PdfName("TransformMethod"));
    writer.Write((IPdfPrimitive) new PdfName("DocMDP"));
    writer.Write((IPdfPrimitive) new PdfName("Type"));
    writer.Write((IPdfPrimitive) new PdfName("SigRef"));
    writer.Write((IPdfPrimitive) new PdfName("DigestValue"));
    int position1 = (int) writer.Position;
    this.m_docDigestPosition = position1;
    writer.Write((IPdfPrimitive) new PdfString(new byte[16 /*0x10*/]));
    PdfArray pdfObject2 = new PdfArray();
    pdfObject2.Add((IPdfPrimitive) new PdfNumber(position1));
    pdfObject2.Add((IPdfPrimitive) new PdfNumber(34));
    writer.Write((IPdfPrimitive) new PdfName("DigestLocation"));
    writer.Write((IPdfPrimitive) pdfObject2);
    writer.Write((IPdfPrimitive) new PdfName("DigestMethod"));
    writer.Write((IPdfPrimitive) new PdfName("MD5"));
    writer.Write((IPdfPrimitive) new PdfName("Data"));
    PdfReferenceHolder pdfObject3 = new PdfReferenceHolder((IPdfPrimitive) catalog);
    writer.Write(" ");
    writer.Write((IPdfPrimitive) pdfObject3);
    writer.Write(">>");
    writer.Write("<<");
    writer.Write((IPdfPrimitive) new PdfName("TransformParams"));
    writer.Write((IPdfPrimitive) new PdfDictionary()
    {
      ["V"] = (IPdfPrimitive) new PdfName("1.2"),
      ["Fields"] = (IPdfPrimitive) new PdfArray()
      {
        (IPdfPrimitive) new PdfString(this.m_sig.Field.Name)
      },
      ["Type"] = (IPdfPrimitive) new PdfName("TransformParams"),
      ["Action"] = (IPdfPrimitive) new PdfName("Include")
    });
    writer.Write((IPdfPrimitive) new PdfName("TransformMethod"));
    writer.Write((IPdfPrimitive) new PdfName("FieldMDP"));
    writer.Write((IPdfPrimitive) new PdfName("Type"));
    writer.Write((IPdfPrimitive) new PdfName("SigRef"));
    writer.Write((IPdfPrimitive) new PdfName("DigestValue"));
    int position2 = (int) writer.Position;
    this.m_fieldsDigestPosition = position2;
    writer.Write((IPdfPrimitive) new PdfString(new byte[16 /*0x10*/]));
    PdfArray pdfObject4 = new PdfArray();
    pdfObject4.Add((IPdfPrimitive) new PdfNumber(position2));
    pdfObject4.Add((IPdfPrimitive) new PdfNumber(34));
    writer.Write((IPdfPrimitive) new PdfName("DigestLocation"));
    writer.Write((IPdfPrimitive) pdfObject4);
    writer.Write((IPdfPrimitive) new PdfName("DigestMethod"));
    writer.Write((IPdfPrimitive) new PdfName("MD5"));
    writer.Write((IPdfPrimitive) new PdfName("Data"));
    writer.Write(" ");
    writer.Write((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) catalog));
    writer.Write(">>");
    writer.Write("]");
    writer.Write(" ");
  }

  private void DocumentSaved(object sender, DocumentSavedEventArgs e)
  {
    if (sender == null)
      throw new ArgumentNullException(nameof (sender));
    if (e == null)
      throw new ArgumentNullException(nameof (e));
    bool enabled = this.m_doc.Security.Enabled;
    this.m_doc.Security.Enabled = false;
    PdfWriter writer = e.Writer;
    byte[] buffer1 = new byte[this.m_firstRangeLength];
    int length1 = (int) e.Writer.Length - this.m_secondRangeIndex;
    byte[] buffer2 = new byte[length1];
    string str1 = "0 ";
    string str2 = this.m_firstRangeLength.ToString() + " ";
    string str3 = this.m_secondRangeIndex.ToString() + " ";
    string str4 = length1.ToString();
    int startPosition1 = this.SaveRangeItem(writer, str1, this.m_startPositionByteRange);
    int startPosition2 = this.SaveRangeItem(writer, str2, startPosition1);
    int startPosition3 = this.SaveRangeItem(writer, str3, startPosition2);
    this.SaveRangeItem(e.Writer, str4, startPosition3);
    this.m_range = new long[4]
    {
      0L,
      (long) int.Parse(str2),
      (long) int.Parse(str3),
      (long) int.Parse(str4)
    };
    this.m_stream = writer.ObtainStream();
    writer.Position = 0L;
    this.m_stream.Read(buffer1, 0, buffer1.Length);
    writer.Position = (long) this.m_secondRangeIndex;
    this.m_stream.Read(buffer2, 0, buffer2.Length);
    byte[][] dataBlocks = new byte[2][]{ buffer1, buffer2 };
    PdfSignatureEventArgs args = (PdfSignatureEventArgs) null;
    if (this.m_sig != null && this.m_sig.ExternalSigner != null)
    {
      string hex = PdfString.BytesToHex(this.GetPKCS7Content());
      e.Writer.Position = (long) this.m_firstRangeLength;
      e.Writer.Write("<");
      e.Writer.Write(hex);
      int length2 = (this.m_secondRangeIndex - (int) e.Writer.Position) / 2;
      e.Writer.Write(PdfString.BytesToHex(new byte[length2]));
      e.Writer.Write(">");
      byte[] numArray = new byte[buffer1.Length + buffer2.Length];
      buffer1.CopyTo((Array) numArray, 0);
      buffer2.CopyTo((Array) numArray, buffer1.Length);
    }
    else if (this.m_sig != null && this.m_sig.Certificate == null)
    {
      byte[] documentData = new byte[buffer1.Length + buffer2.Length];
      buffer1.CopyTo((Array) documentData, 0);
      buffer2.CopyTo((Array) documentData, buffer1.Length);
      int.Parse(str2);
      int.Parse(str3);
      int.Parse(str4);
      args = new PdfSignatureEventArgs(documentData);
      this.m_sig.OnComputeHash(args);
    }
    if (args != null && args.SignedData != null)
    {
      string hex = PdfString.BytesToHex(args.SignedData);
      e.Writer.Position = (long) this.m_firstRangeLength;
      e.Writer.Write("<");
      e.Writer.Write(hex);
      int length3 = (this.m_secondRangeIndex - (int) e.Writer.Position) / 2;
      e.Writer.Write(PdfString.BytesToHex(new byte[length3]));
      e.Writer.Write(">");
      byte[] numArray = new byte[buffer1.Length + buffer2.Length];
      buffer1.CopyTo((Array) numArray, 0);
      buffer2.CopyTo((Array) numArray, buffer1.Length);
    }
    else if (this.m_sig.ExternalSigner == null)
    {
      if (this.m_sig != null && this.m_sig.Certificated && this.AllowMDP())
      {
        byte[] numArray1 = new PdfSignatureDigest().HashDocument(e.Writer.Document);
        e.Writer.Position = (long) this.m_docDigestPosition;
        e.Writer.Write((IPdfPrimitive) new PdfString(numArray1));
        byte[] numArray2 = new PdfSignatureDigest().HashSignatureFields(this.m_sig.Field.Page as PdfPage);
        e.Writer.Position = (long) this.m_fieldsDigestPosition;
        e.Writer.Write((IPdfPrimitive) new PdfString(numArray2));
      }
      if (this.m_cert != null)
      {
        string empty = string.Empty;
        string text = !this.m_cert.isStore ? (this.m_cert.isPkcs7Certificate ? PdfString.BytesToHex(this.GetPKCS7Content()) : PdfString.BytesToHex(new PdfString(this.m_cert.GetSignatureValue(dataBlocks)).Bytes)) : (this.m_sig.Settings.DigestAlgorithm != DigestAlgorithm.SHA1 && this.CheckExportable() || this.m_sig.TimeStampServer != null || this.m_sig.Settings.CryptographicStandard == CryptographicStandard.CADES ? PdfString.BytesToHex(this.GetPKCS7Content()) : this.GetStoreCertificate());
        e.Writer.Position = (long) this.m_firstRangeLength;
        e.Writer.Write("<");
        e.Writer.Write(text);
        int length4 = (this.m_secondRangeIndex - (int) e.Writer.Position) / 2;
        e.Writer.Write(PdfString.BytesToHex(new byte[length4]));
        e.Writer.Write(">");
      }
      else if (this.m_cert == null && this.m_sig != null)
      {
        if (this.m_sig.TimeStampServer != null)
        {
          string empty = string.Empty;
          string hex = PdfString.BytesToHex(this.GetPKCS7TimeStampContent());
          e.Writer.Position = (long) this.m_firstRangeLength;
          e.Writer.Write("<");
          e.Writer.Write(hex);
          int length5 = (this.m_secondRangeIndex - (int) e.Writer.Position) / 2;
          e.Writer.Write(PdfString.BytesToHex(new byte[length5]));
          e.Writer.Write(">");
        }
      }
      else if (this.m_dictionary.ContainsKey("Contents"))
      {
        PdfString pdfString = this.m_dictionary["Contents"] as PdfString;
        e.Writer.Position = (long) this.m_firstRangeLength;
        e.Writer.Write(pdfString.PdfEncode(writer.Document));
      }
      byte[] numArray = new byte[buffer1.Length + buffer2.Length];
      buffer1.CopyTo((Array) numArray, 0);
      buffer2.CopyTo((Array) numArray, buffer1.Length);
    }
    this.m_doc.Security.Enabled = enabled;
  }

  private string GetStoreCertificate()
  {
    HashAlgorithm hashAlgorithm = (HashAlgorithm) new SHA1CryptoServiceProvider();
    this.m_stream.Position = 0L;
    byte[] buffer = new byte[this.m_stream.Length];
    this.m_stream.Read(buffer, 0, (int) this.m_stream.Length);
    this.m_stream.Position = 0L;
    int[] range = new int[4];
    for (int index = 0; index < 4; ++index)
      range[index] = (int) this.m_range[index];
    PdfSignatureDictionary.SignatureRangeStream signatureRangeStream = new PdfSignatureDictionary.SignatureRangeStream((FileStream) null, buffer, range);
    byte[] numArray1 = new byte[8192 /*0x2000*/];
    int inputCount;
    while ((inputCount = signatureRangeStream.Read(numArray1, 0, 8192 /*0x2000*/)) > 0)
      hashAlgorithm.TransformBlock(numArray1, 0, inputCount, numArray1, 0);
    hashAlgorithm.TransformFinalBlock(numArray1, 0, 0);
    byte[] sourceArray = this.SignCertificate(hashAlgorithm.Hash, this.m_cert.X509Certificate, false);
    byte[] numArray2 = new byte[sourceArray.Length];
    Array.Copy((Array) sourceArray, 0, (Array) numArray2, 0, sourceArray.Length);
    return PdfString.BytesToHex(numArray2);
  }

  private byte[] SignCertificate(byte[] message, X509Certificate2 cert, bool detached)
  {
    SignedCms signedCms = new SignedCms(new ContentInfo(message), detached);
    CmsSigner signer = new CmsSigner(cert);
    X509Chain x509Chain = new X509Chain();
    x509Chain.Build(cert);
    signer.IncludeOption = x509Chain.ChainElements.Count <= 1 ? X509IncludeOption.EndCertOnly : X509IncludeOption.WholeChain;
    if (this.m_sig.TimeStampServer != null)
    {
      Asn1 asn1 = (new Asn1Stream(this.m_sig.TimeStampServer.GetTimeStampResponse(new TimeStampRequestCreator(true).GetAsnEncodedTimestampRequest(message))).ReadAsn1() as Asn1Sequence)[1] as Asn1;
      MemoryStream memoryStream = new MemoryStream();
      DerStream derOut = new DerStream((Stream) memoryStream);
      asn1.Encode(derOut);
      byte[] array = memoryStream.ToArray();
      derOut.m_stream.Dispose();
      memoryStream.Dispose();
      AsnEncodedData asnEncodedData = (AsnEncodedData) new Pkcs9AttributeObject(new Oid("1.2.840.113549.1.9.16.2.14"), array);
      signer.UnsignedAttributes.Add(asnEncodedData);
    }
    signedCms.ComputeSignature(signer, false);
    return signedCms.Encode();
  }

  private byte[] GetPKCS7Content()
  {
    string empty = string.Empty;
    SignaturePrivateKey signaturePrivateKey1 = (SignaturePrivateKey) null;
    ICollection<byte[]> numArrays = (ICollection<byte[]>) null;
    byte[] ocsp = (byte[]) null;
    List<X509Certificate2> x509Certificate2List = (List<X509Certificate2>) null;
    IPdfExternalSigner externalSigner = this.m_sig.ExternalSigner;
    if (externalSigner != null)
      x509Certificate2List = this.m_sig.ExternalCertificates;
    X509CertificateToSignature certificateToSignature = (X509CertificateToSignature) null;
    ICollection<X509Certificate> x509Certificates1 = (ICollection<X509Certificate>) new List<X509Certificate>();
    string hashAlgorithm1;
    if (externalSigner != null && x509Certificate2List != null)
    {
      X509CertificateParser certificateParser = new X509CertificateParser();
      foreach (X509Certificate2 x509Certificate2 in x509Certificate2List)
        x509Certificates1.Add(certificateParser.ReadCertificate(x509Certificate2.RawData));
      List<X509Certificate> x509CertificateList = new List<X509Certificate>((IEnumerable<X509Certificate>) x509Certificates1);
      EncryptionAlgorithms encryptionAlgorithms = new EncryptionAlgorithms();
      string encryptionAlgorithm = (string) null;
      for (int index = 0; index < x509CertificateList.Count; ++index)
      {
        if (encryptionAlgorithms.GetAlgorithm(x509CertificateList[index].SigAlgOid) == "ECDSA")
        {
          encryptionAlgorithm = "ECDSA";
          break;
        }
      }
      SignaturePrivateKey signaturePrivateKey2 = new SignaturePrivateKey(externalSigner.HashAlgorithm == null || !string.IsNullOrEmpty(externalSigner.HashAlgorithm) ? externalSigner.HashAlgorithm : (this.m_sig.Settings != null ? this.GetDigestAlgorithm(this.m_sig.Settings.DigestAlgorithm) : "SHA-256"), encryptionAlgorithm);
      hashAlgorithm1 = signaturePrivateKey2.GetHashAlgorithm();
      signaturePrivateKey1 = signaturePrivateKey2;
    }
    else if (!this.m_cert.isStore)
    {
      string key1 = "";
      foreach (string key2 in this.m_cert.m_pkcs7Certificate.KeyEnumerable)
      {
        if (this.m_cert.m_pkcs7Certificate.IsKey(key2) && this.m_cert.m_pkcs7Certificate.GetKey(key2).Key.IsPrivate)
        {
          key1 = key2;
          break;
        }
      }
      KeyEntry key3 = this.m_cert.m_pkcs7Certificate.GetKey(key1);
      foreach (Syncfusion.Pdf.Security.X509Certificates x509Certificates2 in this.m_cert.m_pkcs7Certificate.GetCertificateChain(key1))
        x509Certificates1.Add(x509Certificates2.Certificate);
      if (x509Certificates1.Count > 1)
        this.m_isEndCertOnly = false;
      string hashAlgorithm2 = this.m_sig.Settings != null ? this.GetDigestAlgorithm(this.m_sig.Settings.DigestAlgorithm) : "SHA-256";
      SignaturePrivateKey signaturePrivateKey3 = key3.Key as RsaPrivateKeyParam == null ? new SignaturePrivateKey((ICipherParam) (key3.Key as ECPrivateKey), hashAlgorithm2) : new SignaturePrivateKey((ICipherParam) (key3.Key as RsaPrivateKeyParam), hashAlgorithm2);
      hashAlgorithm1 = signaturePrivateKey3.GetHashAlgorithm();
      signaturePrivateKey1 = signaturePrivateKey3;
    }
    else
    {
      for (int index = 0; index < this.m_cert.Chains.Length; ++index)
        x509Certificates1.Add(this.m_cert.Chains[index]);
      certificateToSignature = new X509CertificateToSignature(this.m_cert.X509Certificate, this.m_sig.Settings == null || !this.CheckExportable() ? "SHA-1" : this.GetDigestAlgorithm(this.m_sig.Settings.DigestAlgorithm));
      hashAlgorithm1 = certificateToSignature.GetHashAlgorithm();
    }
    PdfCmsSigner pdfCmsSigner = new PdfCmsSigner((ICipherParam) null, x509Certificates1, hashAlgorithm1, false);
    IRandom underlyingSource = this.GetUnderlyingSource();
    IRandom[] sources = new IRandom[this.m_range.Length / 2];
    for (int index = 0; index < this.m_range.Length; index += 2)
      sources[index / 2] = (IRandom) new WindowRandom(underlyingSource, this.m_range[index], this.m_range[index + 1]);
    byte[] secondDigest = new MessageDigestAlgorithms().Digest((Stream) new RandomStream((IRandom) new RandomGroup((ICollection<IRandom>) sources)), hashAlgorithm1);
    byte[] timeStampResponse = (byte[]) null;
    byte[] sequenceData = pdfCmsSigner.GetSequenceData(secondDigest, ocsp, numArrays, this.m_sig.Settings.CryptographicStandard);
    if (externalSigner != null)
    {
      byte[] digest = externalSigner.Sign(sequenceData, out timeStampResponse);
      if (digest != null)
      {
        pdfCmsSigner.SetSignedData(digest, (byte[]) null, signaturePrivateKey1.GetEncryptionAlgorithm());
      }
      else
      {
        byte[] destinationArray = new byte[(IntPtr) this.c_EstimatedSize];
        byte[] sourceArray = new byte[0];
        Array.Copy((Array) sourceArray, 0, (Array) destinationArray, 0, sourceArray.Length);
        return destinationArray;
      }
    }
    else if (!this.m_cert.isStore)
    {
      byte[] digest = signaturePrivateKey1.Sign(sequenceData);
      pdfCmsSigner.SetSignedData(digest, (byte[]) null, signaturePrivateKey1.GetEncryptionAlgorithm());
    }
    else
    {
      byte[] digest = certificateToSignature.Sign(sequenceData);
      pdfCmsSigner.SetSignedData(digest, (byte[]) null, certificateToSignature.GetEncryptionAlgorithm());
    }
    byte[] sourceArray1 = pdfCmsSigner.Sign(secondDigest, this.m_sig.TimeStampServer, timeStampResponse, ocsp, numArrays, this.m_sig.Settings.CryptographicStandard, hashAlgorithm1);
    byte[] destinationArray1 = new byte[sourceArray1.Length];
    Array.Copy((Array) sourceArray1, 0, (Array) destinationArray1, 0, sourceArray1.Length);
    return destinationArray1;
  }

  private string GetDigestAlgorithm(DigestAlgorithm digest)
  {
    string digestAlgorithm;
    switch (digest)
    {
      case DigestAlgorithm.SHA1:
        digestAlgorithm = "SHA-1";
        break;
      case DigestAlgorithm.SHA384:
        digestAlgorithm = "SHA-384";
        break;
      case DigestAlgorithm.SHA512:
        digestAlgorithm = "SHA-512";
        break;
      case DigestAlgorithm.RIPEMD160:
        digestAlgorithm = "RIPEMD160";
        break;
      default:
        digestAlgorithm = "SHA-256";
        break;
    }
    return digestAlgorithm;
  }

  private byte[] GetPKCS7TimeStampContent()
  {
    SignaturePrivateKey signaturePrivateKey = new SignaturePrivateKey(this.m_sig.Settings != null ? this.GetDigestAlgorithm(this.m_sig.Settings.DigestAlgorithm) : "SHA-256", (string) null);
    string hashAlgorithm = signaturePrivateKey.GetHashAlgorithm();
    PdfCmsSigner pdfCmsSigner = new PdfCmsSigner(hashAlgorithm, false);
    IRandom underlyingSource = this.GetUnderlyingSource();
    IRandom[] sources = new IRandom[this.m_range.Length / 2];
    for (int index = 0; index < this.m_range.Length; index += 2)
      sources[index / 2] = (IRandom) new WindowRandom(underlyingSource, this.m_range[index], this.m_range[index + 1]);
    byte[] numArray = new MessageDigestAlgorithms().Digest((Stream) new RandomStream((IRandom) new RandomGroup((ICollection<IRandom>) sources)), hashAlgorithm);
    pdfCmsSigner.SetSignedData(numArray, (byte[]) null, signaturePrivateKey.GetEncryptionAlgorithm());
    byte[] encodedTimestamp = pdfCmsSigner.GetEncodedTimestamp(numArray, this.m_sig.TimeStampServer);
    byte[] destinationArray = new byte[encodedTimestamp.Length];
    Array.Copy((Array) encodedTimestamp, 0, (Array) destinationArray, 0, encodedTimestamp.Length);
    return destinationArray;
  }

  private IRandom GetUnderlyingSource()
  {
    byte[] numArray = new byte[this.m_stream.Length];
    this.m_stream.Position = 0L;
    this.m_stream.Read(numArray, 0, (int) this.m_stream.Length);
    return (IRandom) new RandomArray(numArray);
  }

  private int SaveRangeItem(PdfWriter writer, string str, int startPosition)
  {
    byte[] bytes = Encoding.UTF8.GetBytes(str);
    writer.Position = (long) startPosition;
    writer.ObtainStream().Write(bytes, 0, bytes.Length);
    return startPosition + str.Length;
  }

  private void AddReference()
  {
    PdfDictionary pdfDictionary = new PdfDictionary();
    PdfDictionary element = new PdfDictionary();
    PdfArray primitive = new PdfArray();
    int documentPermissions = (int) this.m_sig.DocumentPermissions;
    pdfDictionary["V"] = (IPdfPrimitive) new PdfName("1.2");
    pdfDictionary["P"] = (IPdfPrimitive) new PdfNumber(documentPermissions);
    pdfDictionary["Type"] = (IPdfPrimitive) new PdfName("TransformParams");
    element["TransformMethod"] = (IPdfPrimitive) new PdfName("DocMDP");
    element["Type"] = (IPdfPrimitive) new PdfName("SigRef");
    element["TransformParams"] = (IPdfPrimitive) pdfDictionary;
    primitive.Add((IPdfPrimitive) element);
    this.m_dictionary.SetProperty("Reference", (IPdfPrimitive) primitive);
  }

  private int CreateAsn1TspRequest(byte[] sha1Hash, Stream input)
  {
    byte[] buffer1 = new byte[18]
    {
      (byte) 48 /*0x30*/,
      (byte) 39,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 48 /*0x30*/,
      (byte) 31 /*0x1F*/,
      (byte) 48 /*0x30*/,
      (byte) 7,
      (byte) 6,
      (byte) 5,
      (byte) 43,
      (byte) 14,
      (byte) 3,
      (byte) 2,
      (byte) 26,
      (byte) 4,
      (byte) 20
    };
    byte[] buffer2 = new byte[3]
    {
      (byte) 1,
      (byte) 1,
      byte.MaxValue
    };
    input.Write(buffer1, 0, buffer1.Length);
    input.Write(sha1Hash, 0, sha1Hash.Length);
    input.Write(buffer2, 0, buffer2.Length);
    return buffer1.Length + sha1Hash.Length + buffer2.Length;
  }

  private void Dictionary_BeginSave(object sender, SavePdfPrimitiveEventArgs args)
  {
    bool enabled = this.m_doc.Security.Enabled;
    this.m_dictionary.Encrypt = enabled;
    if (this.m_cert != null)
    {
      this.AddRequiredItems();
      this.AddOptionalItems();
    }
    else if (this.m_sig != null)
    {
      if (this.m_sig.TimeStampServer != null && this.m_sig.isTimeStampOnly)
      {
        this.AddRequiredItems();
        this.AddOptionalItems();
      }
      else
      {
        this.AddRequiredItems();
        this.AddOptionalItems();
      }
    }
    this.m_doc.Security.Enabled = false;
    this.AddContents(args.Writer);
    this.AddRange(args.Writer);
    if (this.m_sig != null && this.m_sig.Certificated)
      this.AddDigest(args.Writer);
    this.m_doc.Security.Enabled = enabled;
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_dictionary;

  private class SignatureRangeStream : Stream
  {
    private byte[] m_data = new byte[1];
    private FileStream m_fStream;
    private byte[] m_buffer;
    private int[] m_range;
    private int m_rangePosition;

    internal SignatureRangeStream(FileStream stream, byte[] buffer, int[] range)
    {
      this.m_fStream = stream;
      this.m_buffer = buffer;
      this.m_range = range;
    }

    public override int ReadByte()
    {
      return this.Read(this.m_data, 0, 1) != 1 ? -1 : (int) this.m_data[0] & (int) byte.MaxValue;
    }

    public override int Read(byte[] b, int off, int len)
    {
      if (b == null)
        throw new ArgumentNullException();
      if (off < 0 || off > b.Length || len < 0 || off + len > b.Length || off + len < 0)
        throw new ArgumentOutOfRangeException();
      if (len == 0)
        return 0;
      if (this.m_rangePosition >= this.m_range[this.m_range.Length - 2] + this.m_range[this.m_range.Length - 1])
        return -1;
      for (int index = 0; index < this.m_range.Length; index += 2)
      {
        int num1 = this.m_range[index];
        int num2 = num1 + this.m_range[index + 1];
        if (this.m_rangePosition < num1)
          this.m_rangePosition = num1;
        if (this.m_rangePosition >= num1 && this.m_rangePosition < num2)
        {
          int num3 = Math.Min(len, num2 - this.m_rangePosition);
          if (this.m_fStream == null)
          {
            Array.Copy((Array) this.m_buffer, this.m_rangePosition, (Array) b, off, num3);
          }
          else
          {
            this.m_fStream.Seek((long) this.m_rangePosition, SeekOrigin.Begin);
            this.ReadFully(b, off, num3);
          }
          this.m_rangePosition += num3;
          return num3;
        }
      }
      return -1;
    }

    private void ReadFully(byte[] b, int offset, int count)
    {
      while (count > 0)
      {
        int num = this.m_fStream.Read(b, offset, count);
        if (num <= 0)
          throw new IOException("insufficient data");
        count -= num;
        offset += num;
      }
    }

    public override bool CanRead => true;

    public override bool CanSeek => false;

    public override bool CanWrite => false;

    public override long Length => 0;

    public override long Position
    {
      get => 0;
      set
      {
      }
    }

    public override void Flush()
    {
    }

    public override long Seek(long offset, SeekOrigin origin) => 0;

    public override void SetLength(long value)
    {
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
    }

    public override void WriteByte(byte value)
    {
    }
  }
}
