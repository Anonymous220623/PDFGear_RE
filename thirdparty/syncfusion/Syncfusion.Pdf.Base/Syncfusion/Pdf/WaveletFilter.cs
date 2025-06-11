// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.WaveletFilter
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal interface WaveletFilter
{
  int AnLowNegSupport { get; }

  int AnLowPosSupport { get; }

  int AnHighNegSupport { get; }

  int AnHighPosSupport { get; }

  int SynLowNegSupport { get; }

  int SynLowPosSupport { get; }

  int SynHighNegSupport { get; }

  int SynHighPosSupport { get; }

  int ImplType { get; }

  int DataType { get; }

  bool Reversible { get; }

  bool isSameAsFullWT(int tailOvrlp, int headOvrlp, int inLen);
}
