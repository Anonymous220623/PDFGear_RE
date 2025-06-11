// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.X509Certificates
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class X509Certificates
{
  private X509Certificate m_certificate;

  internal X509Certificates(X509Certificate certificates) => this.m_certificate = certificates;

  internal X509Certificate Certificate => this.m_certificate;

  public override int GetHashCode() => this.m_certificate.GetHashCode();
}
