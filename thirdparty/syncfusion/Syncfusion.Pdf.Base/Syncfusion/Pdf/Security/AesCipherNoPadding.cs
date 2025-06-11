// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.AesCipherNoPadding
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class AesCipherNoPadding
{
  private ICipher m_cbc;

  public AesCipherNoPadding(bool isEncryption, byte[] key)
  {
    this.m_cbc = (ICipher) new CipherBlockChainingMode((ICipher) new AesEngine());
    KeyParameter parameters = new KeyParameter(key);
    this.m_cbc.Initialize(isEncryption, (ICipherParam) parameters);
  }

  internal byte[] ProcessBlock(byte[] inp, int inpOff, int inpLen)
  {
    if (inpLen % this.m_cbc.BlockSize != 0)
      throw new ArgumentException("Not multiple of block: " + (object) inpLen);
    byte[] outBytes = new byte[inpLen];
    int outOffset = 0;
    while (inpLen > 0)
    {
      this.m_cbc.ProcessBlock(inp, inpOff, outBytes, outOffset);
      inpLen -= this.m_cbc.BlockSize;
      outOffset += this.m_cbc.BlockSize;
      inpOff += this.m_cbc.BlockSize;
    }
    return outBytes;
  }
}
