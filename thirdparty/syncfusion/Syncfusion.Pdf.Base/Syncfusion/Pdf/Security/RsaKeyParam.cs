// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.RsaKeyParam
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class RsaKeyParam : CipherParameter
{
  private Number m_modulus;
  private Number m_exponent;

  internal RsaKeyParam(bool isPrivate, Number modulus, Number exponent)
    : base(isPrivate)
  {
    this.m_modulus = modulus;
    this.m_exponent = exponent;
  }

  internal Number Modulus => this.m_modulus;

  internal Number Exponent => this.m_exponent;

  public override bool Equals(object obj)
  {
    return obj is RsaKeyParam rsaKeyParam && rsaKeyParam.IsPrivate == this.IsPrivate && rsaKeyParam.Modulus.Equals((object) this.m_modulus) && rsaKeyParam.Exponent.Equals((object) this.m_exponent);
  }

  public override int GetHashCode()
  {
    return this.m_modulus.GetHashCode() ^ this.m_exponent.GetHashCode() ^ this.IsPrivate.GetHashCode();
  }
}
