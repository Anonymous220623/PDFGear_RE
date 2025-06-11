// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.X509CertificateHelper
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class X509CertificateHelper
{
  private byte[] m_id;
  private X509Name m_issuer;
  private Number m_serialNumber;

  internal byte[] KeyIdentifier
  {
    get => this.m_id != null ? (byte[]) this.m_id.Clone() : (byte[]) null;
    set => this.m_id = value == null ? (byte[]) null : (byte[]) value.Clone();
  }

  internal X509Name Issuer
  {
    get => this.m_issuer;
    set => this.m_issuer = value;
  }

  internal Number SerialNumber
  {
    get => this.m_serialNumber;
    set => this.m_serialNumber = value;
  }
}
