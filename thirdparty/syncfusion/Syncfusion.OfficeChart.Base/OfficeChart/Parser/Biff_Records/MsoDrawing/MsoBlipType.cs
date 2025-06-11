// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing.MsoBlipType
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing;

internal enum MsoBlipType
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
