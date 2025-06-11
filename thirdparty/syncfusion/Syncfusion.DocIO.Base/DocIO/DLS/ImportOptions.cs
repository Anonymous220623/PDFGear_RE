// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.ImportOptions
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.DLS;

[Flags]
public enum ImportOptions
{
  KeepSourceFormatting = 1,
  MergeFormatting = 2,
  KeepTextOnly = 4,
  UseDestinationStyles = 8,
  ListContinueNumbering = 16, // 0x00000010
  ListRestartNumbering = 32, // 0x00000020
}
