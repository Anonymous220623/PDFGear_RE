// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WarningType
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS;

public enum WarningType
{
  DateTime = 0,
  Annotation = 4,
  Comment = 6,
  CurrentSectionNumber = 7,
  CustomShape = 10, // 0x0000000A
  GroupShape = 15, // 0x0000000F
  LineNumber = 18, // 0x00000012
  Math = 19, // 0x00000013
  PageNumber = 24, // 0x00000018
  Shape = 25, // 0x00000019
  PrintMergeHelperField = 26, // 0x0000001A
  SmartArt = 28, // 0x0000001C
  TrackChanges = 30, // 0x0000001E
  WordArt = 31, // 0x0000001F
  OLEObject = 60, // 0x0000003C
  Textbox = 76, // 0x0000004C
  Watermark = 80, // 0x00000050
}
