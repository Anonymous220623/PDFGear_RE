// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.PSSSigner
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class PSSSigner : ISigner
{
  public const byte m_trailer = 188;
  private readonly IMessageDigest m_digest1;
  private readonly IMessageDigest m_digest2;
  private readonly IMessageDigest m_pssDigest;
  private readonly ICipherBlock m_cipher;
  private SecureRandomAlgorithm m_random;
  private int m_hashLength;
  private int m_digestLength;
  private int m_saltLength;
  private int m_emBits;
  private byte[] m_salt;
  private byte[] m_Dash;
  private byte[] m_blockCipher;
  private byte m_trailerBlock;

  public static PSSSigner CreateRawSigner(ICipherBlock m_cipher, IMessageDigest digest)
  {
    return new PSSSigner(m_cipher, (IMessageDigest) new NullMessageDigest(), digest, digest, digest.MessageDigestSize, (byte) 188);
  }

  public static PSSSigner CreateRawSigner(
    ICipherBlock m_cipher,
    IMessageDigest contentDigest,
    IMessageDigest m_pssDigest,
    int saltLen,
    byte m_trailerBlock)
  {
    return new PSSSigner(m_cipher, (IMessageDigest) new NullMessageDigest(), contentDigest, m_pssDigest, saltLen, m_trailerBlock);
  }

  public PSSSigner(ICipherBlock m_cipher, IMessageDigest digest)
    : this(m_cipher, digest, digest.MessageDigestSize)
  {
  }

  public PSSSigner(ICipherBlock m_cipher, IMessageDigest digest, int saltLen)
    : this(m_cipher, digest, saltLen, (byte) 188)
  {
  }

  public PSSSigner(
    ICipherBlock m_cipher,
    IMessageDigest contentDigest,
    IMessageDigest m_pssDigest,
    int saltLen)
    : this(m_cipher, contentDigest, m_pssDigest, saltLen, (byte) 188)
  {
  }

  public PSSSigner(ICipherBlock m_cipher, IMessageDigest digest, int saltLen, byte m_trailerBlock)
    : this(m_cipher, digest, digest, saltLen, (byte) 188)
  {
  }

  public PSSSigner(
    ICipherBlock m_cipher,
    IMessageDigest contentDigest,
    IMessageDigest m_pssDigest,
    int saltLen,
    byte m_trailerBlock)
    : this(m_cipher, contentDigest, contentDigest, m_pssDigest, saltLen, m_trailerBlock)
  {
  }

  private PSSSigner(
    ICipherBlock m_cipher,
    IMessageDigest m_digest1,
    IMessageDigest m_digest2,
    IMessageDigest m_pssDigest,
    int saltLen,
    byte m_trailerBlock)
  {
    this.m_cipher = m_cipher;
    this.m_digest1 = m_digest1;
    this.m_digest2 = m_digest2;
    this.m_pssDigest = m_pssDigest;
    this.m_hashLength = m_digest2.MessageDigestSize;
    this.m_digestLength = m_pssDigest.MessageDigestSize;
    this.m_saltLength = saltLen;
    this.m_salt = new byte[saltLen];
    this.m_Dash = new byte[8 + saltLen + this.m_hashLength];
    this.m_trailerBlock = m_trailerBlock;
  }

  public string AlgorithmName => this.m_pssDigest.AlgorithmName + "withRSAandMGF1";

  public void BlockUpdate(byte[] bytes, int offset, int length)
  {
    this.m_digest1.BlockUpdate(bytes, offset, length);
  }

  public byte[] GenerateSignature()
  {
    this.m_digest1.DoFinal(this.m_Dash, this.m_Dash.Length - this.m_hashLength - this.m_saltLength);
    if (this.m_saltLength != 0)
    {
      this.m_random.NextBytes(this.m_salt);
      this.m_salt.CopyTo((Array) this.m_Dash, this.m_Dash.Length - this.m_saltLength);
    }
    byte[] numArray = new byte[this.m_hashLength];
    this.m_digest2.BlockUpdate(this.m_Dash, 0, this.m_Dash.Length);
    this.m_digest2.DoFinal(numArray, 0);
    this.m_blockCipher[this.m_blockCipher.Length - this.m_saltLength - 1 - this.m_hashLength - 1] = (byte) 1;
    this.m_salt.CopyTo((Array) this.m_blockCipher, this.m_blockCipher.Length - this.m_saltLength - this.m_hashLength - 1);
    byte[] mask = this.ComputeMask(numArray, 0, numArray.Length, this.m_blockCipher.Length - this.m_hashLength - 1);
    for (int index = 0; index != mask.Length; ++index)
      this.m_blockCipher[index] ^= mask[index];
    this.m_blockCipher[0] &= (byte) ((int) byte.MaxValue >> this.m_blockCipher.Length * 8 - this.m_emBits);
    numArray.CopyTo((Array) this.m_blockCipher, this.m_blockCipher.Length - this.m_hashLength - 1);
    this.m_blockCipher[this.m_blockCipher.Length - 1] = this.m_trailerBlock;
    byte[] signature = this.m_cipher.ProcessBlock(this.m_blockCipher, 0, this.m_blockCipher.Length);
    this.ClearBlock(this.m_blockCipher);
    return signature;
  }

  public void Initialize(bool isSigning, ICipherParam parameters)
  {
    this.m_random = new SecureRandomAlgorithm();
    this.m_cipher.Initialize(isSigning, parameters);
    this.m_emBits = (parameters as RsaKeyParam).Modulus.BitLength - 1;
    if (this.m_emBits < 8 * this.m_hashLength + 8 * this.m_saltLength + 9)
      throw new ArgumentException("Small key is used for hash");
    this.m_blockCipher = new byte[(this.m_emBits + 7) / 8];
  }

  public void Reset() => this.m_digest1.Reset();

  public void Update(byte input) => this.m_digest1.Update(input);

  public bool ValidateSignature(byte[] signature)
  {
    this.m_digest1.DoFinal(this.m_Dash, this.m_Dash.Length - this.m_hashLength - this.m_saltLength);
    byte[] numArray = this.m_cipher.ProcessBlock(signature, 0, signature.Length);
    numArray.CopyTo((Array) this.m_blockCipher, this.m_blockCipher.Length - numArray.Length);
    if ((int) this.m_blockCipher[this.m_blockCipher.Length - 1] != (int) this.m_trailerBlock)
    {
      this.ClearBlock(this.m_blockCipher);
      return false;
    }
    byte[] mask = this.ComputeMask(this.m_blockCipher, this.m_blockCipher.Length - this.m_hashLength - 1, this.m_hashLength, this.m_blockCipher.Length - this.m_hashLength - 1);
    for (int index = 0; index != mask.Length; ++index)
      this.m_blockCipher[index] ^= mask[index];
    this.m_blockCipher[0] &= (byte) ((int) byte.MaxValue >> this.m_blockCipher.Length * 8 - this.m_emBits);
    for (int index = 0; index != this.m_blockCipher.Length - this.m_hashLength - this.m_saltLength - 2; ++index)
    {
      if (this.m_blockCipher[index] != (byte) 0)
      {
        this.ClearBlock(this.m_blockCipher);
        return false;
      }
    }
    if (this.m_blockCipher[this.m_blockCipher.Length - this.m_hashLength - this.m_saltLength - 2] != (byte) 1)
    {
      this.ClearBlock(this.m_blockCipher);
      return false;
    }
    Array.Copy((Array) this.m_blockCipher, this.m_blockCipher.Length - this.m_saltLength - this.m_hashLength - 1, (Array) this.m_Dash, this.m_Dash.Length - this.m_saltLength, this.m_saltLength);
    this.m_digest2.BlockUpdate(this.m_Dash, 0, this.m_Dash.Length);
    this.m_digest2.DoFinal(this.m_Dash, this.m_Dash.Length - this.m_hashLength);
    int index1 = this.m_blockCipher.Length - this.m_hashLength - 1;
    for (int index2 = this.m_Dash.Length - this.m_hashLength; index2 != this.m_Dash.Length; ++index2)
    {
      if (((int) this.m_blockCipher[index1] ^ (int) this.m_Dash[index2]) != 0)
      {
        this.ClearBlock(this.m_Dash);
        this.ClearBlock(this.m_blockCipher);
        return false;
      }
      ++index1;
    }
    this.ClearBlock(this.m_Dash);
    this.ClearBlock(this.m_blockCipher);
    return true;
  }

  private byte[] ComputeMask(byte[] maskD, int maskOff, int maskLen, int size)
  {
    byte[] destinationArray = new byte[size];
    byte[] numArray1 = new byte[this.m_digestLength];
    byte[] numArray2 = new byte[4];
    int m = 0;
    this.m_pssDigest.Reset();
    for (; m < size / this.m_digestLength; ++m)
    {
      this.ComputeItoOSP(m, numArray2);
      this.m_pssDigest.BlockUpdate(maskD, maskOff, maskLen);
      this.m_pssDigest.BlockUpdate(numArray2, 0, numArray2.Length);
      this.m_pssDigest.DoFinal(numArray1, 0);
      numArray1.CopyTo((Array) destinationArray, m * this.m_digestLength);
    }
    if (m * this.m_digestLength < size)
    {
      this.ComputeItoOSP(m, numArray2);
      this.m_pssDigest.BlockUpdate(maskD, maskOff, maskLen);
      this.m_pssDigest.BlockUpdate(numArray2, 0, numArray2.Length);
      this.m_pssDigest.DoFinal(numArray1, 0);
      Array.Copy((Array) numArray1, 0, (Array) destinationArray, m * this.m_digestLength, destinationArray.Length - m * this.m_digestLength);
    }
    return destinationArray;
  }

  private void ComputeItoOSP(int m, byte[] osp)
  {
    osp[0] = (byte) (m >>> 24);
    osp[1] = (byte) (m >>> 16 /*0x10*/);
    osp[2] = (byte) (m >>> 8);
    osp[3] = (byte) m;
  }

  private void ClearBlock(byte[] input) => Array.Clear((Array) input, 0, input.Length);
}
