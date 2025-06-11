// Decompiled with JetBrains decompiler
// Type: Syncfusion.Licensing.crypto.parameters.RSAKeyParameters
// Assembly: Syncfusion.Licensing, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=632609b4d040f6b4
// MVID: 46253E3A-52AF-49F3-BF00-D811A33B7BC6
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Licensing.dll

using Syncfusion.Licensing.math;
using System.ComponentModel;

#nullable disable
namespace Syncfusion.Licensing.crypto.parameters;

[EditorBrowsable(EditorBrowsableState.Never)]
public class RSAKeyParameters : AsymmetricKeyParameter
{
  private BigInteger modulus;
  private BigInteger exponent;

  public RSAKeyParameters(bool isPrivate, BigInteger modulus, BigInteger exponent)
    : base(isPrivate)
  {
    this.modulus = modulus;
    this.exponent = exponent;
  }

  public BigInteger getModulus() => this.modulus;

  public BigInteger getExponent() => this.exponent;

  public override bool Equals(object obj)
  {
    if (obj is RSAKeyParameters)
    {
      RSAKeyParameters rsaKeyParameters = (RSAKeyParameters) obj;
      if (rsaKeyParameters.isPrivate() == this.isPrivate() && rsaKeyParameters.getModulus().Equals((object) this.modulus) && rsaKeyParameters.getExponent().Equals((object) this.exponent))
        return true;
    }
    return false;
  }

  public override int GetHashCode()
  {
    return this.getModulus().GetHashCode() ^ this.getExponent().GetHashCode() ^ (this.isPrivate() ? 1 : 0);
  }
}
