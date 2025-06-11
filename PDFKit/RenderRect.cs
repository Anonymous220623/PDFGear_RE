// Decompiled with JetBrains decompiler
// Type: PDFKit.RenderRect
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

#nullable disable
namespace PDFKit;

internal struct RenderRect(double x, double y, double width, double height, bool isChecked)
{
  public bool IsChecked { get; set; } = isChecked;

  public double X { get; set; } = x;

  public double Y { get; set; } = y;

  public double Left => this.X;

  public double Top => this.Y;

  public double Right => this.X + this.Width;

  public double Bottom => this.Y + this.Height;

  public double Width { get; set; } = width;

  public double Height { get; set; } = height;

  internal bool Contains(double x, double y)
  {
    return new System.Windows.Rect(this.X, this.Y, this.Width, this.Height).Contains(x, y);
  }

  internal System.Windows.Rect Rect() => new System.Windows.Rect(this.X, this.Y, this.Width, this.Height);
}
