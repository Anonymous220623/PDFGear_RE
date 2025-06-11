// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Images.Metafiles.GradientBrushFlags
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Images.Metafiles;

[Flags]
internal enum GradientBrushFlags
{
  Default = 0,
  Matrix = 2,
  ColorBlend = 4,
  Blend = 8,
  FocusScales = 64, // 0x00000040
  GammaCorrection = 128, // 0x00000080
}
