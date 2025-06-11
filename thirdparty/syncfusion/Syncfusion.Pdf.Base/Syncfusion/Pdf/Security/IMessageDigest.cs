// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.IMessageDigest
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

internal interface IMessageDigest
{
  string AlgorithmName { get; }

  int MessageDigestSize { get; }

  int ByteLength { get; }

  void Update(byte input);

  void Update(byte[] bytes, int offset, int length);

  void BlockUpdate(byte[] bytes, int offset, int length);

  int DoFinal(byte[] bytes, int offset);

  void Reset();
}
