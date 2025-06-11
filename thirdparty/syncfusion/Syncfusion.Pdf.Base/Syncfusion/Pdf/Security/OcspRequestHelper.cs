// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.OcspRequestHelper
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class OcspRequestHelper : X509ExtensionBase
{
  private RevocationListRequest m_request;

  public OcspRequestHelper(RevocationListRequest request) => this.m_request = request;

  protected override X509Extensions GetX509Extensions() => (X509Extensions) null;

  public byte[] GetEncoded() => this.m_request.GetEncoded();
}
