// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.OneTimeResponse
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class OneTimeResponse : X509ExtensionBase
{
  private OneTimeResponseHelper m_helper;

  internal OneTimeResponse(OneTimeResponseHelper helper) => this.m_helper = helper;

  internal GeneralizedTime NextUpdate => this.m_helper.NextUpdate;

  internal GeneralizedTime CurrentUpdate => this.m_helper.CurrentUpdate;

  internal CertificateIdentityHelper CertificateID => this.m_helper.CertificateIdentification;

  internal object CertificateStatus
  {
    get
    {
      OcspStatus status = this.m_helper.Status;
      return status.TagNumber == 0 ? (object) null : (object) status;
    }
  }

  protected override X509Extensions GetX509Extensions() => (X509Extensions) null;
}
