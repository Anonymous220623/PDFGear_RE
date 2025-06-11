// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.EllipicCryptoKeyGen
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class EllipicCryptoKeyGen : ICipherKeyGen
{
  private readonly string algorithm;
  private EllipticCurveParams parameters;
  private DerObjectID publicCyptoKey;
  private SecureRandomAlgorithm randomNumber;

  internal EllipicCryptoKeyGen()
    : this("EC")
  {
  }

  internal EllipicCryptoKeyGen(string algorithm)
  {
    this.algorithm = algorithm != null ? EllipticKeyParam.VerifyAlgorithmName(algorithm) : throw new ArgumentNullException(nameof (algorithm));
  }

  public void Init(KeyGenParam parameters)
  {
    if (parameters is EllipticCryptoParam)
    {
      EllipticCryptoParam ellipticCryptoParam = (EllipticCryptoParam) parameters;
      this.publicCyptoKey = ellipticCryptoParam.PublicKeyParamSet;
      this.parameters = ellipticCryptoParam.DomainParameters;
    }
    else
    {
      DerObjectID objectIds;
      switch (parameters.Strength)
      {
        case 192 /*0xC0*/:
          objectIds = ECDSAOIDs.ECPC192v1;
          break;
        case 224 /*0xE0*/:
          objectIds = ECSecIDs.ECSECP224r1;
          break;
        case 239:
          objectIds = ECDSAOIDs.ECPC239v1;
          break;
        case 256 /*0x0100*/:
          objectIds = ECDSAOIDs.ECPC256v1;
          break;
        case 384:
          objectIds = ECSecIDs.ECSECP384r1;
          break;
        case 521:
          objectIds = ECSecIDs.ECSECP521r1;
          break;
        default:
          throw new PdfException("unknown key size.");
      }
      ECX9Field ecCurveByObjectId = EllipicCryptoKeyGen.GetECCurveByObjectID(objectIds);
      this.parameters = new EllipticCurveParams(ecCurveByObjectId.Curve, ecCurveByObjectId.PointG, ecCurveByObjectId.NumberX, ecCurveByObjectId.NumberY, ecCurveByObjectId.Seed());
    }
    this.randomNumber = parameters.Random;
  }

  public ECCipherKeyParam GenerateKeyPair()
  {
    Number numberX = this.parameters.NumberX;
    Number number;
    do
    {
      number = new Number(numberX.BitLength, this.randomNumber);
    }
    while (number.SignValue == 0 || number.CompareTo(numberX) >= 0);
    EllipticPoint pointQ = this.parameters.PointG.Multiply(number);
    return this.publicCyptoKey != null ? new ECCipherKeyParam((CipherParameter) new ECPublicKeyParam(this.algorithm, pointQ, this.publicCyptoKey), (CipherParameter) new ECPrivateKey(this.algorithm, number, this.publicCyptoKey)) : new ECCipherKeyParam((CipherParameter) new ECPublicKeyParam(this.algorithm, pointQ, this.parameters), (CipherParameter) new ECPrivateKey(this.algorithm, number, this.parameters));
  }

  internal static ECX9Field GetECCurveByObjectID(DerObjectID objectIds)
  {
    return ECX962Curves.GetByOid(objectIds) ?? SECGCurves.GetByOid(objectIds) ?? ECNamedCurves.GetByOid(objectIds) ?? ECBrainpoolAlgorithm.GetByOid(objectIds);
  }
}
