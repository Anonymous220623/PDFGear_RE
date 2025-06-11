// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.FontDescriptorFlags
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Graphics;

internal enum FontDescriptorFlags
{
  FixedPitch = 1,
  Serif = 2,
  Symbolic = 4,
  Script = 8,
  Nonsymbolic = 32, // 0x00000020
  Italic = 64, // 0x00000040
  ForceBold = 262144, // 0x00040000
}
