// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.SkipExtRecords
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO;

[Flags]
public enum SkipExtRecords
{
  None = 0,
  Macros = 1,
  Drawings = 2,
  SummaryInfo = 4,
  CopySubstreams = 16, // 0x00000010
  All = CopySubstreams | SummaryInfo | Drawings | Macros, // 0x00000017
}
