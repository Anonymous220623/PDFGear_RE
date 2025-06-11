// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.TextRenderingMode
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

[Flags]
internal enum TextRenderingMode
{
  Fill = 0,
  Stroke = 1,
  FillStroke = 2,
  None = FillStroke | Stroke, // 0x00000003
  ClipFlag = 4,
  ClipFill = ClipFlag, // 0x00000004
  ClipStroke = ClipFill | Stroke, // 0x00000005
  ClipFillStroke = ClipFill | FillStroke, // 0x00000006
  Clip = ClipFillStroke | Stroke, // 0x00000007
}
