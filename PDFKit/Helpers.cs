// Decompiled with JetBrains decompiler
// Type: PDFKit.Helpers
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;
using System.Drawing.Printing;
using System.Security;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace PDFKit;

internal class Helpers
{
  private static Color _emptyColor = Color.FromArgb((byte) 0, (byte) 0, (byte) 0, (byte) 0);

  public static double GetPixelSize(DependencyObject dependencyObject)
  {
    return (double) Helpers.GetDpi(dependencyObject) / 96.0;
  }

  public static int GetDpi(DependencyObject dependencyObject)
  {
    Window window = dependencyObject != null ? Window.GetWindow(dependencyObject) : throw new ArgumentException(nameof (dependencyObject));
    if (window == null)
      throw new ArgumentException("window");
    int num = (int) (PresentationSource.FromVisual((Visual) window).CompositionTarget.TransformToDevice.M11 * 96.0);
    return num == 0 ? 96 /*0x60*/ : num;
  }

  public static int UnitsToPixels(DependencyObject dependencyObject, double units)
  {
    return (int) (units * Helpers.GetPixelSize(dependencyObject));
  }

  public static double PixelsToUnits(DependencyObject dependencyObject, int pixels)
  {
    return (double) pixels / Helpers.GetPixelSize(dependencyObject);
  }

  public static Color ColorEmpty => Helpers._emptyColor;

  internal static Pen CreatePen(Brush brush, double thick = 1.0) => new Pen(brush, thick);

  internal static Pen CreatePen(Color color, double thick = 1.0)
  {
    return Helpers.CreatePen(Helpers.CreateBrush(color), thick);
  }

  internal static Brush CreateBrush(Color color) => (Brush) new SolidColorBrush(color);

  internal static int ToArgb(Color color)
  {
    return (int) color.A << 24 | (int) color.R << 16 /*0x10*/ | (int) color.G << 8 | (int) color.B;
  }

  internal static Size CreateSize(double nw, double nh)
  {
    if (nw < 0.0)
      nw = 0.0;
    if (nh < 0.0)
      nh = 0.0;
    return new Size(nw, nh);
  }

  internal static RenderRect CreateRenderRect(
    double x,
    double y,
    double w,
    double h,
    bool isChecked)
  {
    if (w < 0.0)
      w = 0.0;
    if (h < 0.0)
      h = 0.0;
    return new RenderRect(x, y, w, h, isChecked);
  }

  internal static Rect CreateRect(double x, double y, double w, double h)
  {
    if (w < 0.0)
      w = 0.0;
    if (h < 0.0)
      h = 0.0;
    return new Rect(x, y, w, h);
  }

  internal static Rect CreateRect(Point location, Size size)
  {
    if (size.Width < 0.0)
      size.Width = 0.0;
    if (size.Height < 0.0)
      size.Height = 0.0;
    return new Rect(location, size);
  }

  internal static double ThicknessHorizontal(Thickness pageMargin)
  {
    return pageMargin.Left + pageMargin.Right;
  }

  internal static double ThicknessVertical(Thickness pageMargin)
  {
    return pageMargin.Top + pageMargin.Bottom;
  }

  public static void DrawImageUnscaled(
    DependencyObject dependencyObject,
    DrawingContext drawingContext,
    WriteableBitmap wpfBmp,
    double x,
    double y)
  {
    drawingContext.DrawImage((ImageSource) wpfBmp, new Rect(x, y, Helpers.PixelsToUnits(dependencyObject, wpfBmp.PixelWidth), Helpers.PixelsToUnits(dependencyObject, wpfBmp.PixelHeight)));
  }

  public static void FillRectangle(DrawingContext drawingContext, Brush brush, Rect rect)
  {
    drawingContext.DrawRectangle(brush, (Pen) null, rect);
  }

  public static void DrawRectangle(DrawingContext drawingContext, Pen pen, Rect rect)
  {
    drawingContext.DrawRectangle((Brush) null, pen, rect);
  }

  public static Point GetPixelOffset(UIElement UI)
  {
    Point point1 = new Point();
    PresentationSource presentationSource = PresentationSource.FromVisual((Visual) UI);
    if (presentationSource != null)
    {
      Visual rootVisual = presentationSource.RootVisual;
      Point point2 = Helpers.ApplyVisualTransform(UI.TransformToAncestor(rootVisual).Transform(point1), rootVisual, false);
      point1 = presentationSource.CompositionTarget.TransformToDevice.Transform(point2);
      point1.X = Math.Round(point1.X);
      point1.Y = Math.Round(point1.Y);
      point1 = presentationSource.CompositionTarget.TransformFromDevice.Transform(point1);
      point1 = Helpers.ApplyVisualTransform(point1, rootVisual, true);
      GeneralTransform descendant = rootVisual.TransformToDescendant((Visual) UI);
      if (descendant != null)
        point1 = descendant.Transform(point1);
    }
    return point1;
  }

  private static Point ApplyVisualTransform(Point point, Visual v, bool inverse)
  {
    bool success = true;
    return Helpers.TryApplyVisualTransform(point, v, inverse, true, out success);
  }

  private static Point TryApplyVisualTransform(
    Point point,
    Visual v,
    bool inverse,
    bool throwOnError,
    out bool success)
  {
    success = true;
    if (v != null)
    {
      Matrix visualTransform = Helpers.GetVisualTransform(v);
      if (inverse)
      {
        if (!throwOnError && !visualTransform.HasInverse)
        {
          success = false;
          return new Point(0.0, 0.0);
        }
        visualTransform.Invert();
      }
      point = visualTransform.Transform(point);
    }
    return point;
  }

  private static Matrix GetVisualTransform(Visual v)
  {
    if (v == null)
      return Matrix.Identity;
    Matrix trans1 = Matrix.Identity;
    Transform transform = VisualTreeHelper.GetTransform(v);
    if (transform != null)
    {
      Matrix trans2 = transform.Value;
      trans1 = Matrix.Multiply(trans1, trans2);
    }
    Vector offset = VisualTreeHelper.GetOffset(v);
    trans1.Translate(offset.X, offset.Y);
    return trans1;
  }

  public static void SecurityAssert()
  {
    new PrintingPermission(PrintingPermissionLevel.DefaultPrinting).Assert();
  }

  internal static void SecurityRevert() => CodeAccessPermission.RevertAssert();

  public struct Int32Size(int width, int height)
  {
    public int Width = width;
    public int Height = height;

    public override bool Equals(object obj)
    {
      return obj is Helpers.Int32Size int32Size && this.Equals(int32Size);
    }

    public bool Equals(Helpers.Int32Size obj)
    {
      return obj.Width.Equals(this.Width) && obj.Height.Equals(this.Height);
    }

    public static bool operator ==(Helpers.Int32Size left, Helpers.Int32Size right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(Helpers.Int32Size left, Helpers.Int32Size right)
    {
      return !(left == right);
    }

    public override int GetHashCode()
    {
      return (17 * 23 + this.Width.GetHashCode()) * 23 + this.Height.GetHashCode();
    }
  }

  public struct Int32Point(int x, int y)
  {
    public int X = x;
    public int Y = y;

    public override bool Equals(object obj)
    {
      return obj is Helpers.Int32Point int32Point && this.Equals(int32Point);
    }

    public bool Equals(Helpers.Int32Point obj) => obj.X.Equals(this.X) && obj.Y.Equals(this.Y);

    public static bool operator ==(Helpers.Int32Point left, Helpers.Int32Point right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(Helpers.Int32Point left, Helpers.Int32Point right)
    {
      return !(left == right);
    }

    public override int GetHashCode()
    {
      return (17 * 23 + this.X.GetHashCode()) * 23 + this.Y.GetHashCode();
    }
  }
}
