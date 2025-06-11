// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.CipherBlockChainingMode
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class CipherBlockChainingMode : ICipher
{
  private byte[] m_bytes;
  private byte[] m_cbcBytes;
  private byte[] m_cbcNextBytes;
  private int m_size;
  private ICipher m_cipher;
  private bool m_isEncryption;

  internal ICipher Cipher => this.m_cipher;

  public string AlgorithmName => this.m_cipher.AlgorithmName + "/CBC";

  public bool IsBlock => false;

  internal CipherBlockChainingMode(ICipher cipher)
  {
    this.m_cipher = cipher;
    this.m_size = cipher.BlockSize;
    this.m_bytes = new byte[this.m_size];
    this.m_cbcBytes = new byte[this.m_size];
    this.m_cbcNextBytes = new byte[this.m_size];
  }

  public void Initialize(bool isEncryption, ICipherParam parameters)
  {
    bool isEncryption1 = this.m_isEncryption;
    this.m_isEncryption = isEncryption;
    if (parameters is InvalidParameter)
    {
      InvalidParameter invalidParameter = (InvalidParameter) parameters;
      byte[] invalidBytes = invalidParameter.InvalidBytes;
      if (invalidBytes.Length != this.m_size)
        throw new ArgumentException("Invalid size in block");
      Array.Copy((Array) invalidBytes, 0, (Array) this.m_bytes, 0, invalidBytes.Length);
      parameters = invalidParameter.Parameters;
    }
    this.Reset();
    if (parameters != null)
      this.m_cipher.Initialize(this.m_isEncryption, parameters);
    else if (isEncryption1 != this.m_isEncryption)
      throw new ArgumentException("cannot change encrypting state without providing key.");
  }

  public int BlockSize => this.m_cipher.BlockSize;

  public int ProcessBlock(byte[] inBytes, int inOffset, byte[] outBytes, int outOffset)
  {
    return !this.m_isEncryption ? this.DecryptBlock(inBytes, inOffset, outBytes, outOffset) : this.EncryptBlock(inBytes, inOffset, outBytes, outOffset);
  }

  public void Reset()
  {
    Array.Copy((Array) this.m_bytes, 0, (Array) this.m_cbcBytes, 0, this.m_bytes.Length);
    Array.Clear((Array) this.m_cbcNextBytes, 0, this.m_cbcNextBytes.Length);
    this.m_cipher.Reset();
  }

  private int EncryptBlock(byte[] inBytes, int inOffset, byte[] outBytes, int outOffset)
  {
    if (inOffset + this.m_size > inBytes.Length)
      throw new Exception("Invalid length in input bytes");
    for (int index = 0; index < this.m_size; ++index)
      this.m_cbcBytes[index] ^= inBytes[inOffset + index];
    int num = this.m_cipher.ProcessBlock(this.m_cbcBytes, 0, outBytes, outOffset);
    Array.Copy((Array) outBytes, outOffset, (Array) this.m_cbcBytes, 0, this.m_cbcBytes.Length);
    return num;
  }

  private int DecryptBlock(byte[] inBytes, int inOffset, byte[] outBytes, int outOffset)
  {
    if (inOffset + this.m_size > inBytes.Length)
      throw new Exception("Invalid length in input bytes");
    Array.Copy((Array) inBytes, inOffset, (Array) this.m_cbcNextBytes, 0, this.m_size);
    int num = this.m_cipher.ProcessBlock(inBytes, inOffset, outBytes, outOffset);
    for (int index = 0; index < this.m_size; ++index)
      outBytes[outOffset + index] ^= this.m_cbcBytes[index];
    byte[] cbcBytes = this.m_cbcBytes;
    this.m_cbcBytes = this.m_cbcNextBytes;
    this.m_cbcNextBytes = cbcBytes;
    return num;
  }
}
