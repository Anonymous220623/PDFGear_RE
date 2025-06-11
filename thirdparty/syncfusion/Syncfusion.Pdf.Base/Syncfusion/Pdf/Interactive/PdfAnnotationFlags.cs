// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfAnnotationFlags
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

[Flags]
public enum PdfAnnotationFlags
{
  Default = 0,
  Invisible = 1,
  Hidden = 2,
  Print = 4,
  NoZoom = 8,
  NoRotate = 16, // 0x00000010
  NoView = 32, // 0x00000020
  ReadOnly = 64, // 0x00000040
  Locked = 128, // 0x00000080
  ToggleNoView = 256, // 0x00000100
}
