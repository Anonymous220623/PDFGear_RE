// Decompiled with JetBrains decompiler
// Type: Syncfusion.Licensing.crypto.parameters.RSAPrivateCrtKeyParameters
// Assembly: Syncfusion.Licensing, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=632609b4d040f6b4
// MVID: 46253E3A-52AF-49F3-BF00-D811A33B7BC6
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Licensing.dll

using Syncfusion.Licensing.math;
using System.ComponentModel;

#nullable disable
namespace Syncfusion.Licensing.crypto.parameters;

[EditorBrowsable(EditorBrowsableState.Never)]
public class RSAPrivateCrtKeyParameters : RSAKeyParameters
{
  private BigInteger e;
  private BigInteger p;
  private BigInteger q;
  private BigInteger dP;
  private BigInteger dQ;
  private BigInteger qInv;

  public RSAPrivateCrtKeyParameters(
    BigInteger modulus,
    BigInteger publicExponent,
    BigInteger privateExponent,
    BigInteger p,
    BigInteger q,
    BigInteger dP,
    BigInteger dQ,
    BigInteger qInv)
    : base(true, modulus, privateExponent)
  {
    this.e = publicExponent;
    this.p = p;
    this.q = q;
    this.dP = dP;
    this.dQ = dQ;
    this.qInv = qInv;
  }

  public BigInteger getPublicExponent() => this.e;

  public BigInteger getP() => this.p;

  public BigInteger getQ() => this.q;

  public BigInteger getDP() => this.dP;

  public BigInteger getDQ() => this.dQ;

  public BigInteger getQInv() => this.qInv;

  public override bool Equals(object obj)
  {
    return ((RSAPrivateCrtKeyParameters) obj).getDP().Equals((object) this.dP) && ((RSAPrivateCrtKeyParameters) obj).getDQ().Equals((object) this.dQ) && ((RSAKeyParameters) obj).getExponent().Equals((object) this.getExponent()) && ((RSAKeyParameters) obj).getModulus().Equals((object) this.getModulus()) && ((RSAPrivateCrtKeyParameters) obj).getP().Equals((object) this.p) && ((RSAPrivateCrtKeyParameters) obj).getQ().Equals((object) this.q) && ((RSAPrivateCrtKeyParameters) obj).getPublicExponent().Equals((object) this.e) && ((RSAPrivateCrtKeyParameters) obj).getQInv().Equals((object) this.qInv);
  }

  public override int GetHashCode()
  {
    return this.getDP().GetHashCode() ^ this.getDQ().GetHashCode() ^ this.getExponent().GetHashCode() ^ this.getModulus().GetHashCode() ^ this.getP().GetHashCode() ^ this.getQ().GetHashCode() ^ this.getPublicExponent().GetHashCode() ^ this.getQInv().GetHashCode();
  }
}
