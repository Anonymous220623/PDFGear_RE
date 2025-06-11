// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfPathGradientBrush
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

internal class PdfPathGradientBrush : PdfBrush
{
  private PathGradientBrush m_pathBrush;
  private PdfImage m_image;
  private RectangleF m_bounds;
  private PdfPath m_path;

  public PdfPathGradientBrush(PathGradientBrush brush, PdfPath path)
  {
    RectangleF bounds = path.GetBounds();
    this.m_path = path;
    this.m_pathBrush = brush;
    if (this.m_image != null)
      return;
    this.m_bounds = brush.Rectangle;
    Bitmap bitmap1 = new Bitmap((int) ((double) bounds.Width + 1.0), (int) ((double) bounds.Height + 1.0), PixelFormat.Format32bppArgb);
    System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage((Image) bitmap1);
    graphics.SmoothingMode = SmoothingMode.HighQuality;
    RectangleF rectangle = brush.Rectangle;
    GraphicsPath path1 = new GraphicsPath(path.PathPoints, path.PathTypes);
    graphics.FillPath((Brush) brush, path1);
    Bitmap bitmap2 = bitmap1.Clone(rectangle, bitmap1.PixelFormat);
    bitmap1.Dispose();
    MemoryStream memoryStream = new MemoryStream();
    ImageCodecInfo encoderInfo = this.GetEncoderInfo("image/png");
    EncoderParameters encoderParams = new EncoderParameters(1);
    encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, (short) 1000);
    bitmap2.Save((Stream) memoryStream, encoderInfo, encoderParams);
    bitmap2.Dispose();
    this.m_image = PdfImage.FromStream((Stream) memoryStream);
  }

  private ImageCodecInfo GetEncoderInfo(string mimeType)
  {
    ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
    for (int index = 0; index < imageEncoders.Length; ++index)
    {
      if (imageEncoders[index].MimeType == mimeType)
        return imageEncoders[index];
    }
    return (ImageCodecInfo) null;
  }

  internal void DrawPath(PdfGraphics Graphics)
  {
    Graphics.DrawImage(this.m_image, new RectangleF(this.m_bounds.X, this.m_bounds.Y, this.m_bounds.Width, this.m_bounds.Height));
  }

  internal override bool MonitorChanges(
    PdfBrush brush,
    PdfStreamWriter streamWriter,
    PdfGraphics.GetResources getResources,
    bool saveChanges,
    PdfColorSpace currentColorSpace)
  {
    throw new NotImplementedException();
  }

  internal override bool MonitorChanges(
    PdfBrush brush,
    PdfStreamWriter streamWriter,
    PdfGraphics.GetResources getResources,
    bool saveChanges,
    PdfColorSpace currentColorSpace,
    bool check,
    bool iccbased,
    bool indexed)
  {
    throw new NotImplementedException();
  }

  internal override bool MonitorChanges(
    PdfBrush brush,
    PdfStreamWriter streamWriter,
    PdfGraphics.GetResources getResources,
    bool saveChanges,
    PdfColorSpace currentColorSpace,
    bool check)
  {
    throw new NotImplementedException();
  }

  internal override bool MonitorChanges(
    PdfBrush brush,
    PdfStreamWriter streamWriter,
    PdfGraphics.GetResources getResources,
    bool saveChanges,
    PdfColorSpace currentColorSpace,
    bool check,
    bool iccbased)
  {
    throw new NotImplementedException();
  }

  internal override void ResetChanges(PdfStreamWriter streamWriter)
  {
    throw new NotImplementedException();
  }

  public override PdfBrush Clone() => (PdfBrush) (this.MemberwiseClone() as PdfPathGradientBrush);

  internal void Dispose() => (this.m_image as PdfBitmap).Dispose();
}
