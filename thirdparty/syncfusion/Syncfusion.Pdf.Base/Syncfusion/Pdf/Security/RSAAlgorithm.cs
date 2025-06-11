// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.RSAAlgorithm
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class RSAAlgorithm : ICipherBlock
{
  private RSACoreAlgorithm m_rsaCoreEngine = new RSACoreAlgorithm();
  private RsaKeyParam m_key;
  private SecureRandomAlgorithm m_random;

  public string AlgorithmName => "RSA";

  public void Initialize(bool isEncryption, ICipherParam parameter)
  {
    this.m_rsaCoreEngine.Initialize(isEncryption, parameter);
    this.m_key = (RsaKeyParam) parameter;
    this.m_random = new SecureRandomAlgorithm();
  }

  public int InputBlock => this.m_rsaCoreEngine.InputBlockSize;

  public int OutputBlock => this.m_rsaCoreEngine.OutputBlockSize;

  public byte[] ProcessBlock(byte[] bytes, int offset, int length)
  {
    if (this.m_key == null)
      throw new InvalidOperationException("Invalid RSA engine");
    Number number = this.m_rsaCoreEngine.ConvertInput(bytes, offset, length);
    Number result;
    if (this.m_key is RsaPrivateKeyParam)
    {
      RsaPrivateKeyParam key = (RsaPrivateKeyParam) this.m_key;
      Number publicExponent = key.PublicExponent;
      if (publicExponent != null)
      {
        Number modulus = key.Modulus;
        Number randomInRange = this.CreateRandomInRange(Number.One, modulus.Subtract(Number.One), this.m_random);
        result = this.m_rsaCoreEngine.ProcessBlock(randomInRange.ModPow(publicExponent, modulus).Multiply(number).Mod(modulus)).Multiply(randomInRange.ModInverse(modulus)).Mod(modulus);
      }
      else
        result = this.m_rsaCoreEngine.ProcessBlock(number);
    }
    else
      result = this.m_rsaCoreEngine.ProcessBlock(number);
    return this.m_rsaCoreEngine.ConvertOutput(result);
  }

  internal Number CreateRandomInRange(Number minimum, Number maximum, SecureRandomAlgorithm random)
  {
    if (minimum.CompareTo(maximum) >= 0)
      return minimum;
    if (minimum.BitLength > maximum.BitLength / 2)
      return this.CreateRandomInRange(Number.Zero, maximum.Subtract(minimum), random).Add(minimum);
    for (int index = 0; index < 1000; ++index)
    {
      Number randomInRange = new Number(maximum.BitLength, random);
      if (randomInRange.CompareTo(minimum) >= 0 && randomInRange.CompareTo(maximum) <= 0)
        return randomInRange;
    }
    return new Number(maximum.Subtract(minimum).BitLength - 1, random).Add(minimum);
  }
}
