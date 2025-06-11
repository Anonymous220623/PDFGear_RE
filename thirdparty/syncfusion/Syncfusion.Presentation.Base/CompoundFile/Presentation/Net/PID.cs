// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.Presentation.Net.PID
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System;

#nullable disable
namespace Syncfusion.CompoundFile.Presentation.Net;

[CLSCompliant(false)]
internal enum PID : uint
{
  PID_DICTIONARY = 0,
  PID_CODEPAGE = 1,
  PID_FIRST_USABLE = 2,
  PID_FIRST_NAME_DEFAULT = 4095, // 0x00000FFF
  PID_LOCALE = 2147483648, // 0x80000000
  PID_MIN_READONLY = 2147483648, // 0x80000000
  PID_MODIFY_TIME = 2147483649, // 0x80000001
  PID_SECURITY = 2147483650, // 0x80000002
  PID_BEHAVIOR = 2147483651, // 0x80000003
  PID_MAX_READONLY = 3221225471, // 0xBFFFFFFF
  PID_ILLEGAL = 4294967295, // 0xFFFFFFFF
}
