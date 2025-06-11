// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Native.ETO
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Native;

[Flags]
internal enum ETO
{
  OPAQUE = 2,
  CLIPPED = 4,
  GLYPH_INDEX = 16, // 0x00000010
  NUMERICSLATIN = 2048, // 0x00000800
  NUMERICSLOCAL = 1024, // 0x00000400
  RTLREADING = 128, // 0x00000080
  IGNORELANGUAGE = 4096, // 0x00001000
  PDY = 8192, // 0x00002000
}
