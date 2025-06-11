// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.BufferedBlockPaddingBase
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal abstract class BufferedBlockPaddingBase : IBufferedCipher
{
  protected static readonly byte[] EmptyBuffer = new byte[0];

  public abstract string AlgorithmName { get; }

  public abstract void Initialize(bool isEncryption, ICipherParam parameters);

  public abstract int BlockSize { get; }

  public abstract int GetOutputSize(int inputLen);

  public abstract int GetUpdateOutputSize(int inputLen);

  public abstract byte[] ProcessByte(byte input);

  public abstract void Reset();

  public abstract byte[] ProcessBytes(byte[] input, int inOff, int length);

  public abstract byte[] DoFinal();

  public abstract byte[] DoFinal(byte[] input, int inOff, int length);

  public virtual int ProcessByte(byte input, byte[] output, int outOff)
  {
    byte[] numArray = this.ProcessByte(input);
    if (numArray == null)
      return 0;
    if (outOff + numArray.Length > output.Length)
      throw new Exception("output buffer too short");
    numArray.CopyTo((Array) output, outOff);
    return numArray.Length;
  }

  public virtual byte[] ProcessBytes(byte[] input) => this.ProcessBytes(input, 0, input.Length);

  public virtual int ProcessBytes(byte[] input, byte[] output, int outOff)
  {
    return this.ProcessBytes(input, 0, input.Length, output, outOff);
  }

  public virtual int ProcessBytes(byte[] input, int inOff, int length, byte[] output, int outOff)
  {
    byte[] numArray = this.ProcessBytes(input, inOff, length);
    if (numArray == null)
      return 0;
    if (outOff + numArray.Length > output.Length)
      throw new Exception("output buffer too short");
    numArray.CopyTo((Array) output, outOff);
    return numArray.Length;
  }

  public virtual byte[] DoFinal(byte[] input) => this.DoFinal(input, 0, input.Length);

  public virtual int DoFinal(byte[] output, int outOff)
  {
    byte[] numArray = this.DoFinal();
    if (outOff + numArray.Length > output.Length)
      throw new Exception("output buffer too short");
    numArray.CopyTo((Array) output, outOff);
    return numArray.Length;
  }

  public virtual int DoFinal(byte[] input, byte[] output, int outOff)
  {
    return this.DoFinal(input, 0, input.Length, output, outOff);
  }

  public virtual int DoFinal(byte[] input, int inOff, int length, byte[] output, int outOff)
  {
    int num = this.ProcessBytes(input, inOff, length, output, outOff);
    return num + this.DoFinal(output, outOff + num);
  }
}
