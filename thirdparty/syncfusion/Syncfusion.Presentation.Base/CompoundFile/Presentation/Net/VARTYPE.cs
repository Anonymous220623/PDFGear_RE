// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.Presentation.Net.VARTYPE
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System;

#nullable disable
namespace Syncfusion.CompoundFile.Presentation.Net;

[Flags]
internal enum VARTYPE
{
  VT_EMPTY = 0,
  VT_I4 = 3,
  VT_DATE = 7,
  VT_BSTR = 8,
  VT_BOOL = VT_BSTR | VT_I4, // 0x0000000B
  VT_VARIANT = 12, // 0x0000000C
  VT_INT = 22, // 0x00000016
  VT_LPSTR = VT_INT | VT_BSTR, // 0x0000001E
  VT_LPWSTR = 31, // 0x0000001F
  VT_FILETIME = 64, // 0x00000040
  VT_VECTOR = 4096, // 0x00001000
}
