// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.KeyInformationCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class KeyInformationCollection
{
  internal static KeyInformation CreatePrivateKeyInfo(CipherParameter key)
  {
    if (!(key is RsaKeyParam))
      throw new ArgumentException("Invalid Key");
    Algorithms algorithms = new Algorithms(PKCSOIDs.RsaEncryption, (Asn1Encode) DerNull.Value);
    RSAKey rsaKey;
    if (key is RsaPrivateKeyParam)
    {
      RsaPrivateKeyParam rsaPrivateKeyParam = (RsaPrivateKeyParam) key;
      rsaKey = new RSAKey(rsaPrivateKeyParam.Modulus, rsaPrivateKeyParam.PublicExponent, rsaPrivateKeyParam.Exponent, rsaPrivateKeyParam.P, rsaPrivateKeyParam.Q, rsaPrivateKeyParam.DP, rsaPrivateKeyParam.DQ, rsaPrivateKeyParam.QInv);
    }
    else
    {
      RsaKeyParam rsaKeyParam = (RsaKeyParam) key;
      rsaKey = new RSAKey(rsaKeyParam.Modulus, Number.Zero, rsaKeyParam.Exponent, Number.Zero, Number.Zero, Number.Zero, Number.Zero, Number.Zero);
    }
    return new KeyInformation(algorithms, rsaKey.GetAsn1());
  }

  internal static KeyInformation CreatePrivateKeyInfo(
    char[] passPhrase,
    EncryptedPrivateKey encInfo)
  {
    return KeyInformationCollection.CreatePrivateKeyInfo(passPhrase, false, encInfo);
  }

  internal static KeyInformation CreatePrivateKeyInfo(
    char[] passPhrase,
    bool isPkcs12empty,
    EncryptedPrivateKey encInfo)
  {
    Algorithms encryptionAlgorithm = encInfo.EncryptionAlgorithm;
    PasswordUtility passwordUtility = new PasswordUtility();
    if (!(passwordUtility.CreateEncoder(encryptionAlgorithm) is IBufferedCipher encoder))
      throw new Exception("Unknown encryption algorithm");
    ICipherParam cipherParameters = passwordUtility.GenerateCipherParameters(encryptionAlgorithm, passPhrase, isPkcs12empty);
    encoder.Initialize(false, cipherParameters);
    return KeyInformation.GetInformation((object) encoder.DoFinal(encInfo.EncryptedData));
  }
}
