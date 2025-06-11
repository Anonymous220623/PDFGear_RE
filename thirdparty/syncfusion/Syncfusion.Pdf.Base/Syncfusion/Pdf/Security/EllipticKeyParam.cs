// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.EllipticKeyParam
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Globalization;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class EllipticKeyParam : CipherParameter
{
  private static readonly string[] algorithms = new string[6]
  {
    "EC",
    "ECDSA",
    "ECDH",
    "ECDHC",
    "ECGOST3410",
    "ECMQV"
  };
  private readonly string algorithm;
  private readonly EllipticCurveParams parameters;
  private readonly DerObjectID publicCyptoKey;

  protected EllipticKeyParam(string algorithm, bool isPrivate, EllipticCurveParams parameters)
    : base(isPrivate)
  {
    if (algorithm == null)
      throw new ArgumentNullException(nameof (algorithm));
    if (parameters == null)
      throw new ArgumentNullException(nameof (parameters));
    this.algorithm = EllipticKeyParam.VerifyAlgorithmName(algorithm);
    this.parameters = parameters;
  }

  protected EllipticKeyParam(string algorithm, bool isPrivate, DerObjectID publicCyptoKey)
    : base(isPrivate)
  {
    if (algorithm == null)
      throw new ArgumentNullException(nameof (algorithm));
    if (publicCyptoKey == null)
      throw new ArgumentNullException(nameof (publicCyptoKey));
    this.algorithm = EllipticKeyParam.VerifyAlgorithmName(algorithm);
    this.parameters = EllipticKeyParam.FindParameters(publicCyptoKey);
    this.publicCyptoKey = publicCyptoKey;
  }

  public string AlgorithmName => this.algorithm;

  public EllipticCurveParams Parameters => this.parameters;

  public DerObjectID PublicKeyParamSet => this.publicCyptoKey;

  public override bool Equals(object element)
  {
    if (element == this)
      return true;
    return element is EllipticCurveParams ellipticCurveParams && this.Equals((object) ellipticCurveParams);
  }

  protected bool Equals(EllipticKeyParam element)
  {
    return this.parameters.Equals((object) element.parameters) && this.Equals((CipherParameter) element);
  }

  public override int GetHashCode() => this.parameters.GetHashCode() ^ base.GetHashCode();

  internal static string VerifyAlgorithmName(string algorithm)
  {
    string upper = algorithm.ToUpper(CultureInfo.InvariantCulture);
    if (Array.IndexOf<string>(EllipticKeyParam.algorithms, algorithm, 0, EllipticKeyParam.algorithms.Length) < 0)
      throw new ArgumentException("unknown algorithm: " + algorithm);
    return upper;
  }

  internal static EllipticCurveParams FindParameters(DerObjectID publicCyptoKey)
  {
    EllipticCurveParams parameters = publicCyptoKey != null ? EllipticGOSTCurves.GetByOid(publicCyptoKey) : throw new ArgumentNullException(nameof (publicCyptoKey));
    if (parameters == null)
    {
      ECX9Field ecCurveByObjectId = EllipicCryptoKeyGen.GetECCurveByObjectID(publicCyptoKey);
      if (ecCurveByObjectId == null)
        throw new ArgumentException("OID is not valid");
      parameters = new EllipticCurveParams(ecCurveByObjectId.Curve, ecCurveByObjectId.PointG, ecCurveByObjectId.NumberX, ecCurveByObjectId.NumberY, ecCurveByObjectId.Seed());
    }
    return parameters;
  }
}
