// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.JPXImageCoordinates
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class JPXImageCoordinates
{
  public int x;
  public int y;

  public JPXImageCoordinates()
  {
  }

  public JPXImageCoordinates(int x, int y)
  {
    this.x = x;
    this.y = y;
  }

  public JPXImageCoordinates(JPXImageCoordinates c)
  {
    this.x = c.x;
    this.y = c.y;
  }

  public override string ToString() => $"({(object) this.x},{(object) this.y})";
}
