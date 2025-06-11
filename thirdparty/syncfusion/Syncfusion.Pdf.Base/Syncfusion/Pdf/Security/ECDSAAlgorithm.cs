// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.ECDSAAlgorithm
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class ECDSAAlgorithm : IDSASigner
{
  private EllipticKeyParam ecKey;
  private SecureRandomAlgorithm ecRandomNumber;

  public string AlgorithmName => "ECDSA";

  public void Initialize(bool isSigning, ICipherParam parameters)
  {
    if (isSigning)
    {
      if (parameters is SecureParamNumber)
      {
        SecureParamNumber secureParamNumber = (SecureParamNumber) parameters;
        this.ecRandomNumber = secureParamNumber.Random;
        parameters = secureParamNumber.Parameters;
      }
      else
        this.ecRandomNumber = new SecureRandomAlgorithm();
      this.ecKey = parameters is ECPrivateKey ? (EllipticKeyParam) parameters : throw new Exception("EC private key required for signing");
    }
    else
      this.ecKey = parameters is ECPublicKeyParam ? (EllipticKeyParam) parameters : throw new Exception("EC public key required for verification");
  }

  public Number[] GenerateSignature(byte[] data)
  {
    Number numberX = this.ecKey.Parameters.NumberX;
    Number messageBit = this.CalculateMessageBit(numberX, data);
    Number val;
    Number number1;
    do
    {
      Number number2;
      do
      {
        do
        {
          number2 = new Number(numberX.BitLength, this.ecRandomNumber);
        }
        while (number2.SignValue == 0 || number2.CompareTo(numberX) >= 0);
        val = this.ecKey.Parameters.PointG.Multiply(number2).PointX.ToIntValue().Mod(numberX);
      }
      while (val.SignValue == 0);
      Number key = ((ECPrivateKey) this.ecKey).Key;
      number1 = number2.ModInverse(numberX).Multiply(messageBit.Add(key.Multiply(val).Mod(numberX))).Mod(numberX);
    }
    while (number1.SignValue == 0);
    return new Number[2]{ val, number1 };
  }

  public bool ValidateSignature(byte[] message, Number number3, Number number4)
  {
    Number numberX = this.ecKey.Parameters.NumberX;
    if (number3.SignValue < 1 || number4.SignValue < 1 || number3.CompareTo(numberX) >= 0 || number4.CompareTo(numberX) >= 0)
      return false;
    Number messageBit = this.CalculateMessageBit(numberX, message);
    Number val = number4.ModInverse(numberX);
    Number number = messageBit.Multiply(val).Mod(numberX);
    Number number1 = number3.Multiply(val).Mod(numberX);
    EllipticPoint pointG = this.ecKey.Parameters.PointG;
    EllipticPoint pointQ = ((ECPublicKeyParam) this.ecKey).PointQ;
    EllipticPoint ellipticPoint = ECMath.AddCurve(pointG, number, pointQ, number1);
    return !ellipticPoint.IsInfinity && ellipticPoint.PointX.ToIntValue().Mod(numberX).Equals((object) number3);
  }

  private Number CalculateMessageBit(Number number1, byte[] data)
  {
    int num = data.Length * 8;
    Number messageBit = new Number(1, data);
    if (number1.BitLength < num)
      messageBit = messageBit.ShiftRight(num - number1.BitLength);
    return messageBit;
  }
}
