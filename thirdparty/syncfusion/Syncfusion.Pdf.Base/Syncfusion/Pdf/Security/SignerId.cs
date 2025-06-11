// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.SignerId
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class SignerId : X509CertificateHelper
{
  public override int GetHashCode()
  {
    int hashCode = 0;
    if (this.KeyIdentifier != null)
    {
      int length = this.KeyIdentifier.Length;
      hashCode = length + 1;
      while (--length >= 0)
        hashCode = hashCode * 257 ^ (int) this.KeyIdentifier[length];
    }
    Number serialNumber = this.SerialNumber;
    if (serialNumber != null)
      hashCode ^= serialNumber.GetHashCode();
    X509Name issuer = this.Issuer;
    if (issuer != null)
      hashCode ^= issuer.GetHashCode();
    return hashCode;
  }

  public override bool Equals(object obj)
  {
    return obj != this && obj is SignerId signerId && this.AreEqual(this.KeyIdentifier, signerId.KeyIdentifier) && this.AreEqual(this.Issuer, signerId.Issuer);
  }

  private bool AreEqual(X509Name issuer1, X509Name issuer2)
  {
    return issuer1 != null ? issuer1.Equivalent(issuer1) : issuer2 == null;
  }

  private bool AreEqual(byte[] a, byte[] b)
  {
    if (a == b)
      return true;
    if (a == null || b == null)
      return false;
    int length = a.Length;
    if (length != b.Length)
      return false;
    while (length != 0)
    {
      --length;
      if ((int) a[length] != (int) b[length])
        return false;
    }
    return true;
  }
}
