// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Native.TA_TEXT_ALIGN
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Native;

[Flags]
internal enum TA_TEXT_ALIGN
{
  TA_NOUPDATECP = 0,
  TA_UPDATECP = 1,
  TA_LEFT = 0,
  TA_RIGHT = 2,
  TA_CENTER = 6,
  TA_TOP = 0,
  TA_BOTTOM = 8,
  TA_BASELINE = 24, // 0x00000018
  TA_RTLREADING = 256, // 0x00000100
}
