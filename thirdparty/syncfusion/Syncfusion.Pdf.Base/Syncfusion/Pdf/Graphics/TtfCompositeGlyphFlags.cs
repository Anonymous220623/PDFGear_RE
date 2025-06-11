// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.TtfCompositeGlyphFlags
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

[Flags]
internal enum TtfCompositeGlyphFlags : ushort
{
  ARG_1_AND_2_ARE_WORDS = 1,
  ARGS_ARE_XY_VALUES = 2,
  ROUND_XY_TO_GRID = 4,
  WE_HAVE_A_SCALE = 8,
  RESERVED = 16, // 0x0010
  MORE_COMPONENTS = 32, // 0x0020
  WE_HAVE_AN_X_AND_Y_SCALE = 64, // 0x0040
  WE_HAVE_A_TWO_BY_TWO = 128, // 0x0080
  WE_HAVE_INSTRUCTIONS = 256, // 0x0100
  USE_MY_METRICS = 512, // 0x0200
}
