// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.ImagePointer
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class ImagePointer
{
  private int m_x;
  private int m_y;
  private int m_width;
  private int m_height;
  private JBIG2Image m_bitmap;

  internal ImagePointer(JBIG2Image bitmap)
  {
    this.m_bitmap = bitmap;
    this.m_height = bitmap.Height;
    this.m_width = bitmap.Width;
  }

  internal void SetPointer(int x, int y)
  {
    this.m_x = x;
    this.m_y = y;
  }

  internal int NextPixel()
  {
    if (this.m_y < 0 || this.m_y >= this.m_height || this.m_x >= this.m_width)
      return 0;
    if (this.m_x < 0)
    {
      ++this.m_x;
      return 0;
    }
    int pixel = this.m_bitmap.GetPixel(this.m_x, this.m_y);
    ++this.m_x;
    return pixel;
  }
}
