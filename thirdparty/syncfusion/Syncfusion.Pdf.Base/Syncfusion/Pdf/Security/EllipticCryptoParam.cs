// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.EllipticCryptoParam
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class EllipticCryptoParam : KeyGenParam
{
  private readonly EllipticCurveParams domainParameters;
  private readonly DerObjectID publicCyptoKey;

  public EllipticCryptoParam(EllipticCurveParams domainParameters, SecureRandomAlgorithm random)
    : base(random, domainParameters.NumberX.BitLength)
  {
    this.domainParameters = domainParameters;
  }

  public EllipticCryptoParam(DerObjectID publicCyptoKey, SecureRandomAlgorithm random)
    : this(EllipticKeyParam.FindParameters(publicCyptoKey), random)
  {
    this.publicCyptoKey = publicCyptoKey;
  }

  public EllipticCurveParams DomainParameters => this.domainParameters;

  public DerObjectID PublicKeyParamSet => this.publicCyptoKey;
}
