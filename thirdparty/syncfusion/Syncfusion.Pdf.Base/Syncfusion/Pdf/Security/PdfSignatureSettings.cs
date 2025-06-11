// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.PdfSignatureSettings
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Parsing;

#nullable disable
namespace Syncfusion.Pdf.Security;

public class PdfSignatureSettings
{
  private DigestAlgorithm m_digestAlgorithm = DigestAlgorithm.SHA256;
  private CryptographicStandard m_cryptoStandard;
  private bool m_hasChanged;
  private PdfLoadedSignatureField m_field;
  private bool m_digestUpdated;

  internal PdfLoadedSignatureField SignatureField
  {
    get => this.m_field;
    set => this.m_field = value;
  }

  public DigestAlgorithm DigestAlgorithm
  {
    get
    {
      if (!this.m_digestUpdated && this.m_field != null)
      {
        this.m_digestAlgorithm = this.GetDigestAlgorithm();
        this.m_digestUpdated = true;
      }
      return this.m_digestAlgorithm;
    }
    set
    {
      this.m_digestAlgorithm = value;
      this.m_hasChanged = true;
    }
  }

  public CryptographicStandard CryptographicStandard
  {
    get => this.m_cryptoStandard;
    set
    {
      this.m_cryptoStandard = value;
      this.m_hasChanged = true;
    }
  }

  internal bool HasChanged => this.m_hasChanged;

  internal PdfSignatureSettings()
  {
  }

  private DigestAlgorithm GetDigestAlgorithm()
  {
    PdfCmsSigner cmsSigner = this.m_field.CmsSigner;
    if (cmsSigner != null)
    {
      string hashAlgorithm = cmsSigner.HashAlgorithm;
      if (!string.IsNullOrEmpty(hashAlgorithm))
      {
        switch (hashAlgorithm)
        {
          case "SHA1":
            return DigestAlgorithm.SHA1;
          case "SHA256":
            return DigestAlgorithm.SHA256;
          case "SHA384":
            return DigestAlgorithm.SHA384;
          case "SHA512":
            return DigestAlgorithm.SHA512;
          case "RIPEMD160":
            return DigestAlgorithm.RIPEMD160;
        }
      }
    }
    return DigestAlgorithm.SHA256;
  }
}
