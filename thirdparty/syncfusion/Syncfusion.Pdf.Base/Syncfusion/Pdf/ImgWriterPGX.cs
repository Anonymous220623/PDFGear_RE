// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.ImgWriterPGX
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.IO;

#nullable disable
namespace Syncfusion.Pdf;

internal class ImgWriterPGX
{
  internal int maxVal;
  internal int minVal;
  internal int levShift;
  internal bool isSigned;
  private int bitDepth;
  private FileStream out_Renamed;
  private int offset;
  private DataBlockInt db = new DataBlockInt();
  private int fb;
  private int c;
  private int packBytes;
  private byte[] buf;
}
