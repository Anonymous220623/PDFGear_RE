// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Native.PT_POINT_TYPE
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Native;

[Flags]
internal enum PT_POINT_TYPE : byte
{
  PT_CLOSEFIGURE = 1,
  PT_LINETO = 2,
  PT_BEZIERTO = 4,
  PT_MOVETO = PT_BEZIERTO | PT_LINETO, // 0x06
}
