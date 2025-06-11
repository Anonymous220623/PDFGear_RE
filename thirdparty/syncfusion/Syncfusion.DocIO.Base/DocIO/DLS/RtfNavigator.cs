// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.RtfNavigator
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal abstract class RtfNavigator
{
  internal const int c_two = 2;
  internal const int c_twentiethOfPoint = 20;
  internal const int c_quaterPoint = 4;
  internal const int c_fiftiethOfPoint = 50;
  internal const float c_thirtyfive = 35.5f;

  internal enum EmfToWmfBitsFlags
  {
    EmfToWmfBitsFlagsDefault = 0,
    EmfToWmfBitsFlagsEmbedEmf = 1,
    EmfToWmfBitsFlagsIncludePlaceable = 2,
    EmfToWmfBitsFlagsNoXORClip = 4,
  }
}
