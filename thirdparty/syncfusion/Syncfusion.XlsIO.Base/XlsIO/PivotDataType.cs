// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.PivotDataType
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO;

[Flags]
public enum PivotDataType
{
  Number = 1,
  Integer = 2,
  String = 4,
  Blank = 8,
  Date = 16, // 0x00000010
  Boolean = 32, // 0x00000020
  Float = 64, // 0x00000040
  LongText = 128, // 0x00000080
}
