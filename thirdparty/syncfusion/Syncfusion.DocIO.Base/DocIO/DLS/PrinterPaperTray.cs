// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.PrinterPaperTray
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS;

public enum PrinterPaperTray
{
  DefaultBin = 0,
  OnlyBin = 1,
  LowerBin = 2,
  MiddleBin = 3,
  ManualFeed = 4,
  EnvelopeFeed = 5,
  ManualEnvelopeFeed = 6,
  AutomaticSheetFeed = 7,
  TractorFeed = 8,
  SmallFormatBin = 9,
  LargeFormatBin = 10, // 0x0000000A
  LargeCapacityBin = 11, // 0x0000000B
  PaperCassette = 14, // 0x0000000E
  FormSource = 15, // 0x0000000F
}
