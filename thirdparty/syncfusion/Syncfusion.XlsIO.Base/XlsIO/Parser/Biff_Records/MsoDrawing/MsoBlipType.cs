// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing.MsoBlipType
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing;

public enum MsoBlipType
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
