// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Escher.MSOBlipType
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Escher;

internal enum MSOBlipType
{
  msoblipERROR = 0,
  msoblipUNKNOWN = 1,
  msoblipEMF = 2,
  msoblipWMF = 3,
  msoblipPICT = 4,
  msoblipJPEG = 5,
  msoblipPNG = 6,
  msoblipDIB = 7,
  msoblipFirstClient = 32, // 0x00000020
  msoblipLastClient = 255, // 0x000000FF
}
