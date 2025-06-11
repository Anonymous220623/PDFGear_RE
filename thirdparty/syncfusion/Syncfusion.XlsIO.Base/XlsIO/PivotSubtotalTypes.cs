// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.PivotSubtotalTypes
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO;

[Flags]
public enum PivotSubtotalTypes
{
  None = 0,
  Default = 1,
  Sum = 2,
  Counta = 4,
  Average = 8,
  Max = 16, // 0x00000010
  Min = 32, // 0x00000020
  Product = 64, // 0x00000040
  Count = 128, // 0x00000080
  Stdev = 256, // 0x00000100
  Stdevp = 512, // 0x00000200
  Var = 1024, // 0x00000400
  Varp = 2048, // 0x00000800
}
