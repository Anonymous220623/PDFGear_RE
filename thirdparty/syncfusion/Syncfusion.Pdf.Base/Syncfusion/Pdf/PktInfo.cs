// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PktInfo
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class PktInfo
{
  public int packetIdx;
  public int layerIdx;
  public int cbOff;
  public int cbLength;
  public int[] segLengths;
  public int numTruncPnts;

  public PktInfo(int lyIdx, int pckIdx)
  {
    this.layerIdx = lyIdx;
    this.packetIdx = pckIdx;
  }

  public override string ToString()
  {
    return $"packet {(object) this.packetIdx} (lay:{(object) this.layerIdx}, off:{(object) this.cbOff}, len:{(object) this.cbLength}, numTruncPnts:{(object) this.numTruncPnts})\n";
  }
}
