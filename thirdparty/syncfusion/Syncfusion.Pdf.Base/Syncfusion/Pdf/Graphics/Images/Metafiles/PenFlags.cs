// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Images.Metafiles.PenFlags
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Images.Metafiles;

[Flags]
internal enum PenFlags
{
  Default = 0,
  Transform = 1,
  StartCap = 2,
  EndCap = 4,
  LineJoin = 8,
  MiterLimit = 16, // 0x00000010
  DashStyle = 32, // 0x00000020
  DashCap = 64, // 0x00000040
  DashOffset = 128, // 0x00000080
  DashPattern = 256, // 0x00000100
  Alignment = 512, // 0x00000200
  CompoundArray = 1024, // 0x00000400
  CustomStartCap = 2048, // 0x00000800
  CustomEndCap = 4096, // 0x00001000
}
