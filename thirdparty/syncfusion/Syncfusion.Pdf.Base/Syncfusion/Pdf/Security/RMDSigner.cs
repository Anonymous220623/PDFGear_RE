// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.RMDSigner
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class RMDSigner : ISigner
{
  private ICipherBlock m_rsaEngine = (ICipherBlock) new Pkcs1Encoding((ICipherBlock) new RSAAlgorithm());
  private Algorithms m_id;
  private IMessageDigest m_digest;
  private bool m_isSigning;
  private static readonly IDictionary m_map = (IDictionary) new Hashtable();

  public string AlgorithmName => this.m_digest.AlgorithmName + "withRSA";

  static RMDSigner()
  {
    RMDSigner.m_map[(object) "SHA-1"] = (object) X509Objects.IdSha1;
    RMDSigner.m_map[(object) "SHA-256"] = (object) NISTOIDs.SHA256;
    RMDSigner.m_map[(object) "SHA-384"] = (object) NISTOIDs.SHA384;
    RMDSigner.m_map[(object) "SHA-512"] = (object) NISTOIDs.SHA512;
    RMDSigner.m_map[(object) "RIPEMD160"] = (object) NISTOIDs.RipeMD160;
  }

  internal RMDSigner(IMessageDigest digest)
  {
    this.m_digest = digest;
    if (digest.AlgorithmName.Equals("NULL"))
      this.m_id = (Algorithms) null;
    else
      this.m_id = new Algorithms((DerObjectID) RMDSigner.m_map[(object) digest.AlgorithmName], (Asn1Encode) DerNull.Value);
  }

  public void Initialize(bool isSigning, ICipherParam parameters)
  {
    this.m_isSigning = isSigning;
    CipherParameter cipherParameter = (CipherParameter) parameters;
    if (isSigning && !cipherParameter.IsPrivate)
      throw new Exception("Private key required.");
    if (!isSigning && cipherParameter.IsPrivate)
      throw new Exception("Public key required.");
    this.Reset();
    this.m_rsaEngine.Initialize(isSigning, parameters);
  }

  public void Update(byte input) => this.m_digest.Update(input);

  public void BlockUpdate(byte[] input, int inOff, int length)
  {
    this.m_digest.Update(input, inOff, length);
  }

  public byte[] GenerateSignature()
  {
    if (!this.m_isSigning)
      throw new InvalidOperationException("Invalid entry");
    byte[] numArray = new byte[this.m_digest.MessageDigestSize];
    this.m_digest.DoFinal(numArray, 0);
    byte[] bytes = this.DerEncode(numArray);
    return this.m_rsaEngine.ProcessBlock(bytes, 0, bytes.Length);
  }

  public bool ValidateSignature(byte[] signature)
  {
    if (this.m_isSigning)
      throw new InvalidOperationException("Invalid entry");
    byte[] numArray1 = new byte[this.m_digest.MessageDigestSize];
    this.m_digest.DoFinal(numArray1, 0);
    byte[] numArray2;
    byte[] numArray3;
    try
    {
      numArray2 = this.m_rsaEngine.ProcessBlock(signature, 0, signature.Length);
      numArray3 = this.DerEncode(numArray1);
    }
    catch (Exception ex)
    {
      return false;
    }
    if (numArray2.Length == numArray3.Length)
    {
      for (int index = 0; index < numArray2.Length; ++index)
      {
        if ((int) numArray2[index] != (int) numArray3[index])
          return false;
      }
    }
    else
    {
      if (numArray2.Length != numArray3.Length - 2)
        return false;
      int num1 = numArray2.Length - numArray1.Length - 2;
      int num2 = numArray3.Length - numArray1.Length - 2;
      numArray3[1] -= (byte) 2;
      numArray3[3] -= (byte) 2;
      for (int index = 0; index < numArray1.Length; ++index)
      {
        if ((int) numArray2[num1 + index] != (int) numArray3[num2 + index])
          return false;
      }
      for (int index = 0; index < num1; ++index)
      {
        if ((int) numArray2[index] != (int) numArray3[index])
          return false;
      }
    }
    return true;
  }

  public void Reset() => this.m_digest.Reset();

  private byte[] DerEncode(byte[] hash)
  {
    return this.m_id == null ? hash : new DigestInformation(this.m_id, hash).GetDerEncoded();
  }
}
