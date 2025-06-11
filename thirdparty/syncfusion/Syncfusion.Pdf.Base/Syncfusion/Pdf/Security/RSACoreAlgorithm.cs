// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.RSACoreAlgorithm
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class RSACoreAlgorithm
{
  private RsaKeyParam m_key;
  private bool m_isEncryption;
  private int m_bitSize;

  internal void Initialize(bool isEncryption, ICipherParam parameters)
  {
    this.m_key = parameters is RsaKeyParam ? (RsaKeyParam) parameters : throw new Exception("Invalid RSA key");
    this.m_isEncryption = isEncryption;
    this.m_bitSize = this.m_key.Modulus.BitLength;
  }

  internal int InputBlockSize
  {
    get => this.m_isEncryption ? (this.m_bitSize - 1) / 8 : (this.m_bitSize + 7) / 8;
  }

  internal int OutputBlockSize
  {
    get => this.m_isEncryption ? (this.m_bitSize + 7) / 8 : (this.m_bitSize - 1) / 8;
  }

  internal Number ConvertInput(byte[] bytes, int offset, int length)
  {
    int num = (this.m_bitSize + 7) / 8;
    if (length > num)
      throw new Exception("Invalid length in inputs");
    Number number = new Number(1, bytes, offset, length);
    if (number.CompareTo(this.m_key.Modulus) >= 0)
      throw new Exception("Invalid length in inputs");
    return number;
  }

  internal byte[] ConvertOutput(Number result)
  {
    byte[] numArray1 = result.ToByteArrayUnsigned();
    if (this.m_isEncryption)
    {
      int outputBlockSize = this.OutputBlockSize;
      if (numArray1.Length < outputBlockSize)
      {
        byte[] numArray2 = new byte[outputBlockSize];
        numArray1.CopyTo((Array) numArray2, numArray2.Length - numArray1.Length);
        numArray1 = numArray2;
      }
    }
    return numArray1;
  }

  internal Number ProcessBlock(Number input)
  {
    if (!(this.m_key is RsaPrivateKeyParam))
      return input.ModPow(this.m_key.Exponent, this.m_key.Modulus);
    RsaPrivateKeyParam key = (RsaPrivateKeyParam) this.m_key;
    Number p = key.P;
    Number q = key.Q;
    Number dp = key.DP;
    Number dq = key.DQ;
    Number qinv = key.QInv;
    Number number = input.Remainder(p).ModPow(dp, p);
    Number n = input.Remainder(q).ModPow(dq, q);
    return number.Subtract(n).Multiply(qinv).Mod(p).Multiply(q).Add(n);
  }
}
