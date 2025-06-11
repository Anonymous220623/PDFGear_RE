// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.X509ExtensionBase
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

internal abstract class X509ExtensionBase : IX509Extension
{
  protected abstract X509Extensions GetX509Extensions();

  public virtual Asn1Octet GetExtension(DerObjectID oid)
  {
    X509Extensions x509Extensions = this.GetX509Extensions();
    if (x509Extensions != null)
    {
      X509Extension extension = x509Extensions.GetExtension(oid);
      if (extension != null)
        return extension.Value;
    }
    return (Asn1Octet) null;
  }
}
