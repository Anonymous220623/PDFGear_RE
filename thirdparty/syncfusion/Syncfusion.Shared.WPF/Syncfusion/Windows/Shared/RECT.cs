// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.RECT
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Windows;

#nullable disable
namespace Syncfusion.Windows.Shared;

public struct RECT
{
  public int left;
  public int top;
  public int right;
  public int bottom;

  public RECT(Point lefttop, Point rightbottom)
    : this((int) lefttop.X, (int) lefttop.Y, (int) rightbottom.X, (int) rightbottom.Y)
  {
  }

  public RECT(int left, int top, int right, int bottom)
  {
    this.left = left;
    this.top = top;
    this.right = right;
    this.bottom = bottom;
  }

  public RECT(Rect r)
  {
    this.left = (int) r.Left;
    this.top = (int) r.Top;
    this.right = (int) r.Right;
    this.bottom = (int) r.Bottom;
  }

  public static RECT FromXYWH(int x, int y, int width, int height)
  {
    return new RECT(x, y, x + width, y + height);
  }

  public Size Size
  {
    get => new Size((double) (this.right - this.left), (double) (this.bottom - this.top));
  }
}
