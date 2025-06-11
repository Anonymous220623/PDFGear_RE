// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Native.FormatMessageFlags
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Native;

[Flags]
internal enum FormatMessageFlags
{
  AllocateBuffer = 256, // 0x00000100
  IgnoreInserts = 512, // 0x00000200
  FromString = 1024, // 0x00000400
  FromHmodule = 2048, // 0x00000800
  FromSystem = 4096, // 0x00001000
  ArgumentArray = 8192, // 0x00002000
}
