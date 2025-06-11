// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ExcelFindType
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO;

[Flags]
public enum ExcelFindType
{
  Text = 1,
  Formula = 2,
  FormulaStringValue = 4,
  Error = 8,
  Number = 16, // 0x00000010
  FormulaValue = 32, // 0x00000020
  Values = 64, // 0x00000040
  Comments = 128, // 0x00000080
}
