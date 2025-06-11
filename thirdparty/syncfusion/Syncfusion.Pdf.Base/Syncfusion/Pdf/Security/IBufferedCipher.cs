// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.IBufferedCipher
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

internal interface IBufferedCipher
{
  string AlgorithmName { get; }

  void Initialize(bool forEncryption, ICipherParam parameters);

  int BlockSize { get; }

  int GetOutputSize(int inputLen);

  int GetUpdateOutputSize(int inputLen);

  byte[] ProcessByte(byte input);

  int ProcessByte(byte input, byte[] output, int outOff);

  byte[] ProcessBytes(byte[] input);

  byte[] ProcessBytes(byte[] input, int inOff, int length);

  int ProcessBytes(byte[] input, byte[] output, int outOff);

  int ProcessBytes(byte[] input, int inOff, int length, byte[] output, int outOff);

  byte[] DoFinal();

  byte[] DoFinal(byte[] input);

  byte[] DoFinal(byte[] input, int inOff, int length);

  int DoFinal(byte[] output, int outOff);

  int DoFinal(byte[] input, byte[] output, int outOff);

  int DoFinal(byte[] input, int inOff, int length, byte[] output, int outOff);

  void Reset();
}
