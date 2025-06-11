// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ExcelAutoFormatOptions
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO;

[Flags]
public enum ExcelAutoFormatOptions
{
  Number = 1,
  Border = 2,
  Font = 4,
  Patterns = 8,
  Alignment = 16, // 0x00000010
  Width_Height = 32, // 0x00000020
  None = 0,
  All = Width_Height | Alignment | Patterns | Font | Border | Number, // 0x0000003F
}
