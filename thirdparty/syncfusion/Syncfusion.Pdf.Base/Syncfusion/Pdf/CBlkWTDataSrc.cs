// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.CBlkWTDataSrc
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal interface CBlkWTDataSrc : ForwWTDataProps, ImageData
{
  int getFixedPoint(int c);

  int getDataType(int t, int c);

  CBlkWTData getNextCodeBlock(int c, CBlkWTData cblk);

  CBlkWTData getNextInternCodeBlock(int c, CBlkWTData cblk);
}
