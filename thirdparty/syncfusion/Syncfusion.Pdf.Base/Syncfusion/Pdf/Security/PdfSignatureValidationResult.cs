// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.PdfSignatureValidationResult
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

#nullable disable
namespace Syncfusion.Pdf.Security;

public class PdfSignatureValidationResult
{
  private SignatureStatus m_signatureStatus;
  private bool m_isDocumentModified;
  private bool m_isCertificated;
  private bool m_isValidAtSignedTime;
  private bool m_isValidAtCurrentTime;
  private bool m_isValidAtTimeStampTime;
  private bool m_signerIdentity;
  private RevocationResult m_revocationResult;
  private CryptographicStandard m_cryptographicStandard;
  private DigestAlgorithm m_digestAlgorithm;
  private string m_signatureAlgorithm;
  private PAdESSignatureLevel m_signatureLevel;
  private X509Certificate2Collection m_certificates;
  private TimeStampInformation m_timeStampInfo;
  private List<PdfSignatureValidationException> m_signatureValidationErrors;
  private string m_signatureName;
  internal bool m_isValidOCSPorCRLtimeValidation;

  public string SignatureName
  {
    get => this.m_signatureName;
    internal set => this.m_signatureName = value;
  }

  public SignatureStatus SignatureStatus
  {
    get => this.m_signatureStatus;
    internal set => this.m_signatureStatus = value;
  }

  public RevocationResult RevocationResult
  {
    get => this.m_revocationResult;
    internal set => this.m_revocationResult = value;
  }

  public bool IsDocumentModified
  {
    get => this.m_isDocumentModified;
    internal set => this.m_isDocumentModified = value;
  }

  public bool IsCertificated
  {
    get => this.m_isCertificated;
    internal set => this.m_isCertificated = value;
  }

  internal bool IsValidAtSignedTime
  {
    get => this.m_isValidAtSignedTime;
    set => this.m_isValidAtSignedTime = value;
  }

  internal bool IsValidAtCurrentTime
  {
    get => this.m_isValidAtCurrentTime;
    set => this.m_isValidAtCurrentTime = value;
  }

  internal bool IsValidAtTimeStampTime
  {
    get => this.m_isValidAtTimeStampTime;
    set => this.m_isValidAtTimeStampTime = value;
  }

  public bool IsSignatureValid
  {
    get => this.m_signerIdentity;
    internal set => this.m_signerIdentity = value;
  }

  public CryptographicStandard CryptographicStandard
  {
    get => this.m_cryptographicStandard;
    internal set => this.m_cryptographicStandard = value;
  }

  public string SignatureAlgorithm
  {
    get => this.m_signatureAlgorithm;
    internal set => this.m_signatureAlgorithm = value;
  }

  public DigestAlgorithm DigestAlgorithm
  {
    get => this.m_digestAlgorithm;
    internal set => this.m_digestAlgorithm = value;
  }

  internal PAdESSignatureLevel PAdESSignatureLevel
  {
    get => this.m_signatureLevel;
    set => this.m_signatureLevel = value;
  }

  public X509Certificate2Collection Certificates
  {
    get
    {
      if (this.m_certificates == null)
        this.m_certificates = new X509Certificate2Collection();
      return this.m_certificates;
    }
    internal set => this.m_certificates = value;
  }

  public TimeStampInformation TimeStampInformation
  {
    get => this.m_timeStampInfo;
    internal set => this.m_timeStampInfo = value;
  }

  public List<PdfSignatureValidationException> SignatureValidationErrors
  {
    get
    {
      if (this.m_signatureValidationErrors == null)
        this.m_signatureValidationErrors = new List<PdfSignatureValidationException>();
      return this.m_signatureValidationErrors;
    }
    internal set => this.m_signatureValidationErrors = value;
  }
}
