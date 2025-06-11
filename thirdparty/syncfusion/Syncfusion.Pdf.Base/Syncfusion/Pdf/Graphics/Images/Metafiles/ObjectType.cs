// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Images.Metafiles.ObjectType
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Graphics.Images.Metafiles;

internal enum ObjectType
{
  Invalid = 0,
  Brush = 256, // 0x00000100
  Pen = 512, // 0x00000200
  Path = 768, // 0x00000300
  Region = 1024, // 0x00000400
  Image = 1280, // 0x00000500
  Font = 1536, // 0x00000600
  StringFormat = 1792, // 0x00000700
  ImageAttributes = 2048, // 0x00000800
  CustomLineCap = 2304, // 0x00000900
}
