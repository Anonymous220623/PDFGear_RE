// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.ShadeType
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;

internal enum ShadeType
{
  msoshadeNone = 0,
  msoshadeGamma = 1,
  msoshadeSigma = 2,
  msoshadeBand = 4,
  msoshadeOneColor = 8,
  msoshadeParameterShift = 16, // 0x00000010
  msoshadeDefault = 1073741827, // 0x40000003
}
