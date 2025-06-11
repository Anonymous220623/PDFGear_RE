// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Native.GDI_PITCH_AND_FAMILY
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Native;

internal enum GDI_PITCH_AND_FAMILY
{
  FF_DONTCARE = 0,
  TMPF_FIXED_PITCH = 1,
  TMPF_VECTOR = 2,
  TMPF_TRUETYPE = 4,
  TMPF_DEVICE = 8,
  FF_ROMAN = 16, // 0x00000010
  FF_SWISS = 32, // 0x00000020
  FF_MODERN = 48, // 0x00000030
  FF_SCRIPT = 64, // 0x00000040
  FF_DECORATIVE = 80, // 0x00000050
}
