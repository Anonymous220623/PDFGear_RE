// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.PdfCertificate
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Native;
using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Security;

public class PdfCertificate
{
  private const uint CryptographicUserKeyset = 4096 /*0x1000*/;
  private const uint SystemStoreCertificateCurrentUser = 65536 /*0x010000*/;
  private const uint CertificateStoreReadonlyFlag = 32768 /*0x8000*/;
  private const uint CertificateStoreOpenExistingFlag = 16384 /*0x4000*/;
  private const uint X509AsnEncoding = 1;
  private const uint PKCS7AsnEncoding = 65536 /*0x010000*/;
  private const uint EncodingType = 65537 /*0x010001*/;
  private const int X509Name = 7;
  private const string RelativeDistinguishedName = "2.5.4.3";
  private CryptoSignMessageParamerter m_signParameters;
  private int m_version;
  private byte[] m_serialNumber;
  private string m_issuerName;
  private string m_subjectName;
  private IntPtr m_certificate;
  private uint m_signatureLength;
  private DateTime m_validTo;
  private DateTime m_validFrom;
  private X509Certificate2 m_x509Certificate;
  internal PdfPKCSCertificate m_pkcs7Certificate;
  internal bool isPkcs7Certificate;
  internal bool isStore;
  internal Syncfusion.Pdf.Security.X509Certificate[] Chains;
  private Dictionary<string, Dictionary<string, string>> m_distinguishedNameCollection = new Dictionary<string, Dictionary<string, string>>();

  public int Version => this.m_version;

  public byte[] SerialNumber => this.m_serialNumber;

  public string IssuerName => this.m_issuerName;

  public string SubjectName => this.m_subjectName;

  public DateTime ValidTo => this.m_validTo;

  public DateTime ValidFrom => this.m_validFrom;

  internal IntPtr SysCertificate => this.m_certificate;

  internal X509Certificate2 X509Certificate => this.m_x509Certificate;

  public PdfCertificate(string pfxPath, string password)
  {
    if (pfxPath == null)
      throw new ArgumentNullException(nameof (pfxPath));
    if (password == null)
      throw new ArgumentNullException(nameof (password));
    try
    {
      this.InitializePkcs7Certificate(PdfCertificate.GetFileBytes(pfxPath), password.ToCharArray());
    }
    catch (Exception ex)
    {
      this.isPkcs7Certificate = false;
      this.m_x509Certificate = new X509Certificate2(pfxPath, password);
      this.Initialize(pfxPath, password);
    }
  }

  public PdfCertificate(Stream certificate, string password)
  {
    if (certificate == null)
      throw new ArgumentNullException("pfxPath");
    if (password == null)
      throw new ArgumentNullException(nameof (password));
    certificate.Position = 0L;
    try
    {
      this.InitializePkcs7Certificate(this.StreamToByteArray(certificate), password.ToCharArray());
    }
    catch (Exception ex)
    {
      this.isPkcs7Certificate = false;
      this.m_x509Certificate = new X509Certificate2(this.StreamToByteArray(certificate), password);
      certificate.Position = 0L;
      this.Initialize(certificate, password);
    }
  }

  internal PdfCertificate(PdfCmsSigner signer) => this.LoadDetails(signer.SignerCertificate);

  private byte[] StreamToByteArray(Stream stream)
  {
    stream.Position = 0L;
    byte[] buffer = new byte[stream.Length];
    stream.Read(buffer, 0, buffer.Length);
    return buffer;
  }

  private void InitializePkcs7Certificate(byte[] data, char[] password)
  {
    this.m_pkcs7Certificate = new PdfPKCSCertificate((Stream) new MemoryStream(data), password);
    string key1 = "";
    foreach (string key2 in this.m_pkcs7Certificate.KeyEnumerable)
    {
      if (this.m_pkcs7Certificate.IsKey(key2) && this.m_pkcs7Certificate.GetKey(key2).Key.IsPrivate)
      {
        key1 = key2;
        break;
      }
    }
    this.LoadDetails(this.m_pkcs7Certificate.GetCertificate(key1).Certificate);
  }

  private void LoadDetails(Syncfusion.Pdf.Security.X509Certificate certificate)
  {
    this.m_issuerName = this.GetDistinguishedAttributes(certificate.CertificateStructure.Issuer.ToString(), "CN");
    this.m_subjectName = this.GetDistinguishedAttributes(certificate.CertificateStructure.Subject.ToString(), "CN");
    this.m_validFrom = certificate.CertificateStructure.StartDate.ToDateTime();
    this.m_validTo = certificate.CertificateStructure.EndDate.ToDateTime();
    this.m_version = certificate.CertificateStructure.Version;
    byte[] numArray1 = new byte[certificate.CertificateStructure.SerialNumber.m_value.Length];
    byte[] numArray2 = (byte[]) certificate.CertificateStructure.SerialNumber.m_value.Clone();
    System.Array.Reverse((System.Array) numArray2, 0, numArray2.Length);
    this.m_serialNumber = numArray2;
    this.isPkcs7Certificate = true;
  }

  public string GetValue(string pdfCertificateDistinguishedName, PdfCertificateField field)
  {
    switch (field)
    {
      case PdfCertificateField.Subject:
        return this.GetDistinguishedName(pdfCertificateDistinguishedName, this.m_x509Certificate.Subject);
      case PdfCertificateField.Issuer:
        return this.GetDistinguishedName(pdfCertificateDistinguishedName, this.m_x509Certificate.Issuer);
      default:
        return string.Empty;
    }
  }

  private string GetDistinguishedName(string name, string distinguishedStringCollection)
  {
    string distinguishedName1 = string.Empty;
    if (!this.m_distinguishedNameCollection.ContainsKey(distinguishedStringCollection))
    {
      distinguishedName1 = this.GetDistinguishedAttributes(distinguishedStringCollection, name);
    }
    else
    {
      Dictionary<string, string> distinguishedName2 = this.m_distinguishedNameCollection[distinguishedStringCollection];
      if (name.Contains("="))
        name = name.Replace("=", "");
      if (distinguishedName2.ContainsKey(name))
        distinguishedName1 = distinguishedName2[name];
    }
    return distinguishedName1;
  }

  private string GetDistinguishedAttributes(string name, string key)
  {
    string empty = string.Empty;
    Dictionary<string, string> dictionary = new Dictionary<string, string>();
    if (key.Contains("="))
      key = key.Replace("=", "");
    if (!this.m_distinguishedNameCollection.ContainsKey(name))
    {
      StringBuilder stringBuilder = new StringBuilder();
      PdfCertificate.DistinguishedNameSeparator distinguishedNameSeparator = PdfCertificate.DistinguishedNameSeparator.InitialSeparator;
      for (int index = 0; index < name.Length; ++index)
      {
        switch (distinguishedNameSeparator)
        {
          case PdfCertificate.DistinguishedNameSeparator.InitialSeparator:
            if (name[index] == ',' || name[index] == ';' || name[index] == '+')
            {
              this.AddStringToDictionary(stringBuilder.ToString(), dictionary);
              stringBuilder.Length = 0;
              break;
            }
            stringBuilder.Append(name[index]);
            if (name[index] == '\\')
            {
              stringBuilder.Append(name[++index]);
              break;
            }
            if (name[index] == '"')
            {
              distinguishedNameSeparator = PdfCertificate.DistinguishedNameSeparator.QuoteSpearator;
              break;
            }
            break;
          case PdfCertificate.DistinguishedNameSeparator.QuoteSpearator:
            stringBuilder.Append(name[index]);
            if (name[index] == '\\')
            {
              stringBuilder.Append(name[++index]);
              break;
            }
            if (name[index] == '"')
            {
              distinguishedNameSeparator = PdfCertificate.DistinguishedNameSeparator.InitialSeparator;
              break;
            }
            break;
        }
      }
      this.AddStringToDictionary(stringBuilder.ToString(), dictionary);
      stringBuilder.Length = 0;
      this.m_distinguishedNameCollection.Add(name, dictionary);
      if (dictionary.ContainsKey(key))
        empty = dictionary[key];
    }
    else
    {
      Dictionary<string, string> distinguishedName = this.m_distinguishedNameCollection[name];
      if (distinguishedName.ContainsKey(key))
        empty = distinguishedName[key];
    }
    return empty;
  }

  private void AddStringToDictionary(string name, Dictionary<string, string> dictionary)
  {
    int length = name.IndexOf("=");
    if (length <= 0)
      return;
    string[] strArray = new string[2]
    {
      name.Substring(0, length).TrimStart().TrimEnd(),
      null
    };
    int startIndex = length + 1;
    strArray[1] = name.Substring(startIndex, name.Length - startIndex).TrimStart().TrimEnd();
    if (string.IsNullOrEmpty(strArray[0]) || string.IsNullOrEmpty(strArray[1]) || dictionary.ContainsKey(strArray[0]))
      return;
    dictionary.Add(strArray[0], strArray[1]);
  }

  private string GetName(string name, string key)
  {
    string[] strArray = name.Split(',');
    string name1 = string.Empty;
    for (int index = 0; index < strArray.Length; ++index)
    {
      if (strArray[index].Contains(key))
      {
        name1 = strArray[index].Replace(key, "").TrimStart(' ').TrimEnd('\\');
        if (strArray.Length > index + 1 && !strArray[index + 1].Contains("="))
        {
          name1 = $"{name1},{strArray[index + 1].ToString()}";
          if (name1.Contains("+"))
          {
            name1 = name1.Substring(0, name1.IndexOf('+')).TrimEnd();
            break;
          }
          break;
        }
        if (name1.Contains("+"))
        {
          name1 = name1.Substring(0, name1.IndexOf('+')).TrimEnd();
          break;
        }
        break;
      }
    }
    return name1;
  }

  public PdfCertificate(string pfxPath, string password, KeyStorageFlags storageFlag)
  {
    if (pfxPath == null)
      throw new ArgumentNullException(nameof (pfxPath));
    if (password == null)
      throw new ArgumentNullException(nameof (password));
    try
    {
      this.InitializePkcs7Certificate(PdfCertificate.GetFileBytes(pfxPath), password.ToCharArray());
    }
    catch (Exception ex)
    {
      this.isPkcs7Certificate = false;
      X509KeyStorageFlags keyStorageFlags = (X509KeyStorageFlags) Enum.Parse(typeof (X509KeyStorageFlags), storageFlag.ToString());
      this.m_x509Certificate = new X509Certificate2(pfxPath, password, keyStorageFlags);
      this.Initialize(pfxPath, password);
    }
  }

  public PdfCertificate(Stream certificate, string password, KeyStorageFlags storageFlag)
  {
    if (certificate == null)
      throw new ArgumentNullException("pfxPath");
    if (password == null)
      throw new ArgumentNullException(nameof (password));
    certificate.Position = 0L;
    try
    {
      this.InitializePkcs7Certificate(this.StreamToByteArray(certificate), password.ToCharArray());
    }
    catch (Exception ex)
    {
      this.isPkcs7Certificate = false;
      X509KeyStorageFlags keyStorageFlags = (X509KeyStorageFlags) Enum.Parse(typeof (X509KeyStorageFlags), storageFlag.ToString());
      this.m_x509Certificate = new X509Certificate2(this.StreamToByteArray(certificate), password, keyStorageFlags);
      certificate.Position = 0L;
      this.Initialize(certificate, password);
    }
  }

  internal PdfCertificate(IntPtr certificate)
  {
    if (certificate == IntPtr.Zero)
      throw new ArgumentNullException(nameof (certificate));
    this.Initialize(certificate);
  }

  public PdfCertificate(X509Certificate2 x509Certificate2)
  {
    this.isStore = true;
    this.m_x509Certificate = x509Certificate2;
    this.m_subjectName = this.GetValue("CN", PdfCertificateField.Subject);
    this.m_issuerName = this.GetValue("CN", PdfCertificateField.Issuer);
    this.m_validFrom = x509Certificate2.NotBefore;
    this.m_validTo = x509Certificate2.NotAfter;
    this.m_version = x509Certificate2.Version;
    this.m_serialNumber = x509Certificate2.GetSerialNumber();
    X509CertificateParser certificateParser = new X509CertificateParser();
    X509Chain x509Chain = new X509Chain();
    x509Chain.Build(x509Certificate2);
    this.Chains = new Syncfusion.Pdf.Security.X509Certificate[x509Chain.ChainElements.Count];
    for (int index = 0; index < x509Chain.ChainElements.Count; ++index)
      this.Chains[index] = certificateParser.ReadCertificate(x509Chain.ChainElements[index].Certificate.RawData);
  }

  public static PdfCertificate[] GetCertificates() => PdfCertificate.GetStoreCertificates();

  private static PdfCertificate[] GetStoreCertificates()
  {
    List<PdfCertificate> certificateList = new List<PdfCertificate>();
    PdfCertificate.GetStoreCertificates(StoreName.My, certificateList);
    PdfCertificate.GetStoreCertificates(StoreName.CertificateAuthority, certificateList);
    PdfCertificate.GetStoreCertificates(StoreName.Root, certificateList);
    return certificateList.ToArray();
  }

  private static void GetStoreCertificates(StoreName name, List<PdfCertificate> certificateList)
  {
    X509Store x509Store = new X509Store(name);
    x509Store.Open(OpenFlags.MaxAllowed);
    foreach (X509Certificate2 certificate in x509Store.Certificates)
    {
      PdfCertificate pdfCertificate = new PdfCertificate(certificate);
      certificateList.Add(pdfCertificate);
    }
  }

  public static PdfCertificate FindBySubject(StoreType type, string subject)
  {
    return PdfCertificate.FindBySubject(type, subject, StoreRegion.LocalMachine, false);
  }

  private static PdfCertificate FindBySubject(
    StoreType type,
    string subject,
    StoreRegion storeRegion,
    bool isRegion)
  {
    X509Store x509Store = !isRegion ? new X509Store(PdfCertificate.GetStoreNameFromType(type)) : new X509Store(PdfCertificate.GetStoreNameFromType(type), PdfCertificate.GetCertificateLocation(storeRegion));
    x509Store.Open(OpenFlags.OpenExistingOnly);
    X509Certificate2Collection certificate2Collection = x509Store.Certificates.Find(X509FindType.FindBySubjectName, (object) subject, false);
    x509Store.Close();
    return certificate2Collection.Count > 0 ? new PdfCertificate(certificate2Collection[0]) : (PdfCertificate) null;
  }

  public static PdfCertificate FindBySubject(
    StoreType type,
    string subject,
    StoreRegion storeRegion)
  {
    if (subject == null)
      throw new ArgumentNullException(nameof (subject));
    return PdfCertificate.FindBySubject(type, subject, storeRegion, true);
  }

  private static StoreLocation GetCertificateLocation(StoreRegion certificateLocation)
  {
    StoreLocation certificateLocation1 = StoreLocation.CurrentUser;
    if (certificateLocation == StoreRegion.LocalMachine)
      certificateLocation1 = StoreLocation.LocalMachine;
    return certificateLocation1;
  }

  private static StoreName GetStoreNameFromType(StoreType type)
  {
    StoreName storeNameFromType = StoreName.My;
    switch (type)
    {
      case StoreType.ROOT:
        storeNameFromType = StoreName.Root;
        break;
      case StoreType.CA:
        storeNameFromType = StoreName.CertificateAuthority;
        break;
      case StoreType.TrustedPeople:
        storeNameFromType = StoreName.TrustedPeople;
        break;
      case StoreType.TrustedPublisher:
        storeNameFromType = StoreName.TrustedPublisher;
        break;
      case StoreType.AuthRoot:
        storeNameFromType = StoreName.AuthRoot;
        break;
      case StoreType.AddressBook:
        storeNameFromType = StoreName.AddressBook;
        break;
    }
    return storeNameFromType;
  }

  public static PdfCertificate FindByIssuer(StoreType type, string issuer)
  {
    if (issuer == null)
      throw new ArgumentNullException(nameof (issuer));
    X509Store x509Store = new X509Store(PdfCertificate.GetStoreNameFromType(type));
    x509Store.Open(OpenFlags.OpenExistingOnly);
    X509Certificate2Collection certificate2Collection = x509Store.Certificates.Find(X509FindType.FindByIssuerName, (object) issuer, false);
    x509Store.Close();
    return certificate2Collection.Count > 0 ? new PdfCertificate(certificate2Collection[0]) : (PdfCertificate) null;
  }

  public static PdfCertificate FindBySerialId(StoreType type, byte[] certificateID)
  {
    if (certificateID == null)
      throw new ArgumentNullException(nameof (certificateID));
    IntPtr storeProvider = CryptoApi.CertOpenSystemStore(IntPtr.Zero, type.ToString());
    for (IntPtr index = CryptoApi.CertEnumCertificatesInStore(storeProvider, (IntPtr) 0); index != IntPtr.Zero; index = CryptoApi.CertEnumCertificatesInStore(storeProvider, index))
    {
      IntPtr num = CryptoApi.CertDuplicateCertificateContext(index);
      CertInformation certificateInformation = PdfCertificate.GetCertificateInformation(num);
      byte[] numArray = new byte[certificateInformation.SerialNumber.Length];
      Marshal.Copy(certificateInformation.SerialNumber.Data, numArray, 0, numArray.Length);
      if (PdfCertificate.Equals(numArray, certificateID))
      {
        if (storeProvider != IntPtr.Zero)
          CryptoApi.CertCloseStore(storeProvider, 0);
        return new PdfCertificate(num);
      }
    }
    if (storeProvider != IntPtr.Zero)
      CryptoApi.CertCloseStore(storeProvider, 0);
    return (PdfCertificate) null;
  }

  private void Initialize(string pfxFileName, string password)
  {
    IntPtr zero = IntPtr.Zero;
    byte[] source = File.Exists(pfxFileName) ? PdfCertificate.GetFileBytes(pfxFileName) : throw new PdfException("File is not found");
    CryptographicDataStore pPfx = new CryptographicDataStore();
    pPfx.BinaryContentLength = source.Length;
    pPfx.BinaryContentData = Marshal.AllocHGlobal(source.Length);
    Marshal.Copy(source, 0, pPfx.BinaryContentData, source.Length);
    IntPtr storeProvider = CryptoApi.PFXIsPFXBlob(ref pPfx) ? CryptoApi.PFXImportCertStore(ref pPfx, password, 4096U /*0x1000*/) : throw new ArgumentException("File has wrong format");
    if (storeProvider == IntPtr.Zero)
      throw new ArgumentException(new Win32Exception(Marshal.GetLastWin32Error()).Message);
    Marshal.FreeHGlobal(pPfx.BinaryContentData);
    IntPtr num = CryptoApi.CertEnumCertificatesInStore(storeProvider, (IntPtr) 0);
    string[] strArray = this.m_x509Certificate.SubjectName.Name.Split(',');
    string str1 = string.Empty;
    foreach (string str2 in strArray)
    {
      if (str2.Contains("CN="))
      {
        str1 = str2.Replace("CN=", "").TrimStart(' ');
        break;
      }
    }
    while (num != IntPtr.Zero)
    {
      this.Initialize(CryptoApi.CertDuplicateCertificateContext(num));
      num = CryptoApi.CertEnumCertificatesInStore(storeProvider, num);
      if (this.m_subjectName == str1)
        break;
    }
    if (!(storeProvider != IntPtr.Zero))
      return;
    CryptoApi.CertCloseStore(storeProvider, 0);
  }

  private void Initialize(Stream certificate, string password)
  {
    IntPtr zero = IntPtr.Zero;
    byte[] byteArray = this.StreamToByteArray(certificate);
    CryptographicDataStore pPfx = new CryptographicDataStore();
    pPfx.BinaryContentLength = byteArray.Length;
    pPfx.BinaryContentData = Marshal.AllocHGlobal(byteArray.Length);
    Marshal.Copy(byteArray, 0, pPfx.BinaryContentData, byteArray.Length);
    IntPtr storeProvider = CryptoApi.PFXIsPFXBlob(ref pPfx) ? CryptoApi.PFXImportCertStore(ref pPfx, password, 4096U /*0x1000*/) : throw new ArgumentException("File has wrong format");
    if (storeProvider == IntPtr.Zero)
      throw new ArgumentException(new Win32Exception(Marshal.GetLastWin32Error()).Message);
    Marshal.FreeHGlobal(pPfx.BinaryContentData);
    IntPtr num = CryptoApi.CertEnumCertificatesInStore(storeProvider, (IntPtr) 0);
    string[] strArray = new string[0];
    string str1 = string.Empty;
    foreach (string str2 in strArray)
    {
      if (str2.Contains("CN="))
      {
        str1 = str2.Replace("CN=", "").TrimStart(' ');
        break;
      }
    }
    while (num != IntPtr.Zero)
    {
      this.Initialize(CryptoApi.CertDuplicateCertificateContext(num));
      num = CryptoApi.CertEnumCertificatesInStore(storeProvider, num);
      if (this.m_subjectName == str1)
        break;
    }
    if (!(storeProvider != IntPtr.Zero))
      return;
    CryptoApi.CertCloseStore(storeProvider, 0);
  }

  private void Initialize(IntPtr certificate)
  {
    this.m_certificate = certificate;
    CertInformation structure = (CertInformation) Marshal.PtrToStructure(((CertificateContext) Marshal.PtrToStructure(certificate, typeof (CertificateContext))).CertificateInformation, typeof (CertInformation));
    this.m_version = structure.Version;
    this.m_serialNumber = new byte[structure.SerialNumber.Length];
    Marshal.Copy(structure.SerialNumber.Data, this.m_serialNumber, 0, this.m_serialNumber.Length);
    string pszObjId = structure.SignatureAlgorithm.pszObjId;
    this.m_issuerName = PdfCertificate.DecodeCryptographicObject(structure.Issuer);
    this.m_subjectName = PdfCertificate.DecodeCryptographicObject(structure.Subject);
    this.m_validFrom = this.ConvertTime(structure.NotBefore);
    this.m_validTo = this.ConvertTime(structure.NotAfter);
    this.m_signParameters = new CryptoSignMessageParamerter();
    this.m_signParameters.SizeInBytes = (uint) Marshal.SizeOf(typeof (CryptoSignMessageParamerter));
    this.m_signParameters.EncodingType = 65537U /*0x010001*/;
    this.m_signParameters.SigningCertPointer = this.m_certificate;
    this.m_signParameters.HashAlgorithm.pszObjId = pszObjId;
    this.m_signParameters.HashAlgorithm.Parameters.Length = 0;
    this.m_signParameters.HashAuxInfo = new IntPtr(0);
    this.m_signParameters.MessageCertificateCount = 1U;
    this.m_signParameters.MessageCertificate = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof (IntPtr)));
    Marshal.StructureToPtr<IntPtr>(this.m_certificate, this.m_signParameters.MessageCertificate, true);
    this.m_signParameters.MessageCrlCount = 0U;
    this.m_signParameters.MessageCrl = new IntPtr(0);
    this.m_signParameters.AuthenticatedAttributeCount = 0U;
    this.m_signParameters.AuthenticatedAttribute = new IntPtr(0);
    this.m_signParameters.UnauthenticatedAttributeCount = 0U;
    this.m_signParameters.UnauthenticatedAttribute = new IntPtr(0);
    this.m_signParameters.CrytographicSilentFlag = 0U;
    this.m_signParameters.InnerContentType = 0U;
  }

  internal uint GetSignatureLength()
  {
    if (this.m_signatureLength == 0U)
    {
      byte[] bytes = Encoding.UTF8.GetBytes(Environment.CurrentDirectory);
      int[] rgcbToBeSigned = new int[1]{ bytes.Length };
      IntPtr destination1 = Marshal.AllocCoTaskMem(bytes.Length);
      Marshal.Copy(bytes, 0, destination1, bytes.Length);
      if (!CryptoApi.CryptSignMessage(ref this.m_signParameters, true, 1U, new IntPtr[1]
      {
        destination1
      }, rgcbToBeSigned, IntPtr.Zero, ref this.m_signatureLength))
      {
        uint lastError = KernelApi.GetLastError();
        IntPtr num = Marshal.AllocHGlobal(4);
        uint length = KernelApi.FormatMessage(FormatMessageFlags.AllocateBuffer | FormatMessageFlags.FromSystem, (IntPtr) 0, lastError, 0U, num, 4U, (IntPtr) 0);
        byte[] destination2 = new byte[4];
        Marshal.Copy(num, destination2, 0, 4);
        int int32 = BitConverter.ToInt32(destination2, 0);
        Marshal.FreeHGlobal(num);
        num = new IntPtr(int32);
        byte[] numArray = new byte[(IntPtr) length];
        Marshal.Copy(num, numArray, 0, (int) length);
        Marshal.FreeHGlobal(num);
        throw new Exception(Encoding.UTF8.GetString(numArray));
      }
    }
    return this.m_signatureLength;
  }

  internal byte[] GetSignatureValue(byte[][] dataBlocks)
  {
    if (dataBlocks == null)
      throw new ArgumentNullException(nameof (dataBlocks));
    uint signatureLength = this.GetSignatureLength();
    int length = dataBlocks.Length;
    byte[] destination1 = new byte[(IntPtr) signatureLength];
    IntPtr[] rgpbToBeSigned = new IntPtr[length];
    int[] rgcbToBeSigned = new int[length];
    for (int index = 0; index < length; ++index)
    {
      byte[] dataBlock = dataBlocks[index];
      IntPtr destination2 = Marshal.AllocCoTaskMem(dataBlock.Length);
      Marshal.Copy(dataBlock, 0, destination2, dataBlock.Length);
      rgpbToBeSigned[index] = destination2;
      rgcbToBeSigned[index] = dataBlock.Length;
    }
    IntPtr num = Marshal.AllocCoTaskMem((int) signatureLength);
    CryptoApi.CryptSignMessage(ref this.m_signParameters, true, (uint) length, rgpbToBeSigned, rgcbToBeSigned, num, ref signatureLength);
    Marshal.Copy(num, destination1, 0, destination1.Length);
    return destination1;
  }

  private static byte[] GetFileBytes(string filename)
  {
    if (!File.Exists(filename))
      return (byte[]) null;
    using (FileStream fileStream = File.OpenRead(filename))
    {
      int length = (int) fileStream.Length;
      byte[] buffer = new byte[length];
      fileStream.Seek(0L, SeekOrigin.Begin);
      fileStream.Read(buffer, 0, length);
      return buffer;
    }
  }

  private DateTime ConvertTime(FileTime filetime)
  {
    SystemTime lpSystemTime = new SystemTime();
    IntPtr num = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof (IntPtr)));
    Marshal.StructureToPtr<FileTime>(filetime, num, true);
    KernelApi.FileTimeToSystemTime(num, ref lpSystemTime);
    return new DateTime((int) lpSystemTime.Year, (int) lpSystemTime.Month, (int) lpSystemTime.Day, (int) lpSystemTime.Hour, (int) lpSystemTime.Minute, (int) lpSystemTime.Second, (int) lpSystemTime.Milliseconds);
  }

  private static string DecodeCryptographicObject(CryptographicApiStore blob)
  {
    IntPtr zero = IntPtr.Zero;
    int cbStructInfo = 0;
    CryptoApi.CryptDecodeObject(65537U /*0x010001*/, 7U, blob.Data, blob.Length, 0U, zero, ref cbStructInfo);
    IntPtr num = Marshal.AllocHGlobal(cbStructInfo);
    CryptoApi.CryptDecodeObject(65537U /*0x010001*/, 7U, blob.Data, blob.Length, 0U, num, ref cbStructInfo);
    CertificateNameInformation structure1 = (CertificateNameInformation) Marshal.PtrToStructure(num, typeof (CertificateNameInformation));
    string str1 = string.Empty;
    Marshal.FreeHGlobal(num);
    IntPtr ptr = structure1.Pointers;
    CertificateRDNAttribute certificateRdnAttribute = new CertificateRDNAttribute();
    for (int index = 0; "2.5.4.3" != str1 && structure1.Length != index; ++index)
    {
      CertificateRelativeDistinguishedName structure2 = (CertificateRelativeDistinguishedName) Marshal.PtrToStructure(ptr, typeof (CertificateRelativeDistinguishedName));
      certificateRdnAttribute = (CertificateRDNAttribute) Marshal.PtrToStructure(structure2.Attribute, typeof (CertificateRDNAttribute));
      str1 = certificateRdnAttribute.ID;
      ptr = IntPtr.Size != 4 ? new IntPtr(ptr.ToInt64() + (long) Marshal.SizeOf<CertificateRelativeDistinguishedName>(structure2)) : new IntPtr(ptr.ToInt32() + Marshal.SizeOf<CertificateRelativeDistinguishedName>(structure2));
    }
    byte[] numArray = new byte[certificateRdnAttribute.Value.Length];
    if (numArray.Length == 0)
      return (string) null;
    string str2 = (string) null;
    Marshal.Copy(certificateRdnAttribute.Value.Data, numArray, 0, numArray.Length);
    if (certificateRdnAttribute.ValueType == 4 || certificateRdnAttribute.ValueType == 5)
      str2 = new string(Encoding.UTF8.GetChars(numArray));
    else if (certificateRdnAttribute.ValueType == 12 || certificateRdnAttribute.ValueType == 13)
      str2 = Encoding.Unicode.GetString(numArray, 0, numArray.Length);
    return str2;
  }

  private static CertInformation GetCertificateInformation(IntPtr hCertCtx)
  {
    return (CertInformation) Marshal.PtrToStructure(((CertificateContext) Marshal.PtrToStructure(hCertCtx, typeof (CertificateContext))).CertificateInformation, typeof (CertInformation));
  }

  private static bool Equals(byte[] arr1, byte[] arr2)
  {
    if (arr1 == null)
      throw new ArgumentNullException(nameof (arr1));
    if (arr2 == null)
      throw new ArgumentNullException(nameof (arr2));
    bool flag = arr1.Length == arr2.Length;
    if (flag)
    {
      for (int index = 0; index < arr1.Length; ++index)
      {
        if ((int) arr1[index] != (int) arr2[index])
        {
          flag = false;
          break;
        }
      }
    }
    return flag;
  }

  private enum DistinguishedNameSeparator
  {
    InitialSeparator,
    QuoteSpearator,
  }
}
