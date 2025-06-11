// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.X509CertificateToSignature
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class X509CertificateToSignature
{
  private X509Certificate2 certificate;
  private string hashAlgorithm;
  private string encryptionAlgorithm;

  internal X509CertificateToSignature(X509Certificate2 certificate, string hashAlgorithm)
  {
    this.certificate = certificate;
    MessageDigestAlgorithms digestAlgorithms = new MessageDigestAlgorithms();
    this.hashAlgorithm = digestAlgorithms.GetDigest(digestAlgorithms.GetAllowedDigests(hashAlgorithm));
    if (certificate.PrivateKey is RSACryptoServiceProvider)
    {
      this.encryptionAlgorithm = "RSA";
    }
    else
    {
      if (!(certificate.PrivateKey is DSACryptoServiceProvider))
        throw new ArgumentException("Unknown encryption algorithm " + (object) certificate.PrivateKey);
      this.encryptionAlgorithm = "DSA";
    }
  }

  private bool CheckExportable(CspKeyContainerInfo info)
  {
    try
    {
      return info.Exportable;
    }
    catch
    {
      return false;
    }
  }

  public virtual byte[] Sign(byte[] message)
  {
    if (!(this.certificate.PrivateKey is RSACryptoServiceProvider))
      return ((DSACryptoServiceProvider) this.certificate.PrivateKey).SignData(message);
    RSACryptoServiceProvider privateKey = (RSACryptoServiceProvider) this.certificate.PrivateKey;
    if (privateKey == null || privateKey.CspKeyContainerInfo == null || !this.CheckExportable(privateKey.CspKeyContainerInfo))
      return privateKey.SignData(message, (object) this.hashAlgorithm);
    RSAParameters parameters = privateKey.ExportParameters(true);
    if (this.hashAlgorithm == "RIPEMD160")
      return new SignaturePrivateKey((ICipherParam) new RsaPrivateKeyParam(new Number(1, parameters.Modulus), new Number(1, parameters.Exponent), new Number(1, parameters.D), new Number(1, parameters.P), new Number(1, parameters.Q), new Number(1, parameters.DP), new Number(1, parameters.DQ), new Number(1, parameters.InverseQ)), this.hashAlgorithm).Sign(message);
    RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();
    cryptoServiceProvider.ImportParameters(parameters);
    return cryptoServiceProvider.SignData(message, (object) this.hashAlgorithm);
  }

  internal string GetHashAlgorithm() => this.hashAlgorithm;

  internal string GetEncryptionAlgorithm() => this.encryptionAlgorithm;
}
