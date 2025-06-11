// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.SignaturePrivateKey
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class SignaturePrivateKey
{
  private ICipherParam m_key;
  private string m_hashAlgorithm;
  private string m_encryptionAlgorithm;

  internal SignaturePrivateKey(ICipherParam key, string hashAlgorithm)
  {
    this.m_key = key;
    MessageDigestAlgorithms digestAlgorithms = new MessageDigestAlgorithms();
    this.m_hashAlgorithm = digestAlgorithms.GetDigest(digestAlgorithms.GetAllowedDigests(hashAlgorithm));
    switch (key)
    {
      case RsaKeyParam _:
        this.m_encryptionAlgorithm = "RSA";
        break;
      case EllipticKeyParam _:
        this.m_encryptionAlgorithm = "ECDSA";
        break;
      default:
        throw new ArgumentException("Invalid key");
    }
  }

  internal SignaturePrivateKey(string hashAlgorithm, string encryptionAlgorithm)
  {
    MessageDigestAlgorithms digestAlgorithms = new MessageDigestAlgorithms();
    this.m_hashAlgorithm = digestAlgorithms.GetDigest(digestAlgorithms.GetAllowedDigests(hashAlgorithm));
    if (encryptionAlgorithm == null)
      this.m_encryptionAlgorithm = "RSA";
    else
      this.m_encryptionAlgorithm = encryptionAlgorithm;
  }

  internal byte[] Sign(byte[] bytes)
  {
    ISigner signer = new SignerUtilities().GetSigner($"{this.m_hashAlgorithm}with{this.m_encryptionAlgorithm}");
    signer.Initialize(true, this.m_key);
    signer.BlockUpdate(bytes, 0, bytes.Length);
    return signer.GenerateSignature();
  }

  internal string GetHashAlgorithm() => this.m_hashAlgorithm;

  internal string GetEncryptionAlgorithm() => this.m_encryptionAlgorithm;
}
