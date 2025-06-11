// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.DSASigner
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class DSASigner : ISigner
{
  private readonly IMessageDigest algorithm;
  private readonly IDSASigner signer;
  private bool isSign;

  internal DSASigner(IDSASigner signer, IMessageDigest digest)
  {
    this.algorithm = digest;
    this.signer = signer;
  }

  public string AlgorithmName => $"{this.algorithm.AlgorithmName}with{this.signer.AlgorithmName}";

  public void Initialize(bool isSigning, ICipherParam parameters)
  {
    this.isSign = isSigning;
    CipherParameter cipherParameter = !(parameters is SecureParamNumber) ? (CipherParameter) parameters : (CipherParameter) ((SecureParamNumber) parameters).Parameters;
    if (isSigning && !cipherParameter.IsPrivate)
      throw new Exception("Needs Private Key");
    if (!isSigning && cipherParameter.IsPrivate)
      throw new Exception("Needs Public Key.");
    this.Reset();
    this.signer.Initialize(isSigning, parameters);
  }

  public void Update(byte input) => this.algorithm.Update(input);

  public void BlockUpdate(byte[] input, int offset, int length)
  {
    this.algorithm.BlockUpdate(input, offset, length);
  }

  public byte[] GenerateSignature()
  {
    if (!this.isSign)
      throw new InvalidOperationException("DSASigner not initialised");
    byte[] numArray = new byte[this.algorithm.MessageDigestSize];
    this.algorithm.DoFinal(numArray, 0);
    Number[] signature = this.signer.GenerateSignature(numArray);
    return this.DerEncode(signature[0], signature[1]);
  }

  public bool ValidateSignature(byte[] signature)
  {
    if (this.isSign)
      throw new InvalidOperationException("DSASigner not initialised");
    byte[] numArray = new byte[this.algorithm.MessageDigestSize];
    this.algorithm.DoFinal(numArray, 0);
    try
    {
      Number[] numberArray = this.DerDecode(signature);
      return this.signer.ValidateSignature(numArray, numberArray[0], numberArray[1]);
    }
    catch (Exception ex)
    {
      return false;
    }
  }

  public void Reset() => this.algorithm.Reset();

  private byte[] DerEncode(Number number1, Number number2)
  {
    return new DerSequence(new Asn1Encode[2]
    {
      (Asn1Encode) new DerInteger(number1),
      (Asn1Encode) new DerInteger(number2)
    }).GetDerEncoded();
  }

  private Number[] DerDecode(byte[] encoding)
  {
    Asn1Sequence asn1Sequence = (Asn1Sequence) Asn1.FromByteArray(encoding);
    return new Number[2]
    {
      ((DerInteger) asn1Sequence[0]).Value,
      ((DerInteger) asn1Sequence[1]).Value
    };
  }
}
