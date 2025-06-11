// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Native.CertInformation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Native;

internal struct CertInformation
{
  public int Version;
  public CryptographicApiStore SerialNumber;
  public CRYPT_ALGORITHM_IDENTIFIER SignatureAlgorithm;
  public CryptographicApiStore Issuer;
  public FileTime NotBefore;
  public FileTime NotAfter;
  public CryptographicApiStore Subject;
  public CERT_PUBLIC_KEY_INFO SubjectPublicKeyInfo;
  public CryptographicApiStore IssuerUniqueId;
  public CryptographicApiStore SubjectUniqueId;
  public int ExtensionCount;
  public PCERT_EXTENSION Extension;
}
