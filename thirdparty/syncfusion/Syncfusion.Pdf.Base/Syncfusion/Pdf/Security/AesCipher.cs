// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.AesCipher
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class AesCipher
{
  private BufferedCipher m_bp;

  public AesCipher(bool isEncryption, byte[] key, byte[] iv)
  {
    this.m_bp = new BufferedCipher((ICipher) new CipherBlockChainingMode((ICipher) new AesEngine()));
    InvalidParameter parameters = new InvalidParameter((ICipherParam) new KeyParameter(key), iv);
    this.m_bp.Initialize(isEncryption, (ICipherParam) parameters);
  }

  internal byte[] Update(byte[] inp, int inpOff, int inpLen)
  {
    int updateOutputSize = this.m_bp.GetUpdateOutputSize(inpLen);
    byte[] output = (byte[]) null;
    if (updateOutputSize > 0)
      output = new byte[updateOutputSize];
    this.m_bp.ProcessBytes(inp, inpOff, inpLen, output, 0);
    return output;
  }

  internal byte[] DoFinal()
  {
    byte[] numArray = new byte[this.m_bp.GetOutputSize(0)];
    int length;
    try
    {
      length = this.m_bp.DoFinal(numArray, 0);
    }
    catch
    {
      return numArray;
    }
    if (length == numArray.Length)
      return numArray;
    byte[] destinationArray = new byte[length];
    Array.Copy((Array) numArray, 0, (Array) destinationArray, 0, length);
    return destinationArray;
  }
}
