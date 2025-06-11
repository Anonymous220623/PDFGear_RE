// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.PivotSubtotalItems2007
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO;

[Flags]
public enum PivotSubtotalItems2007
{
  avg = 8,
  count = 128, // 0x00000080
  countA = 4,
  max = 16, // 0x00000010
  min = 32, // 0x00000020
  product = 64, // 0x00000040
  stdDev = 256, // 0x00000100
  stdDevP = 512, // 0x00000200
  sum = 2,
  var = 1024, // 0x00000400
  varP = 2048, // 0x00000800
}
