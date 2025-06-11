// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.DocIO.Native.VARTYPE
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.CompoundFile.DocIO.Native;

[Flags]
public enum VARTYPE
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
