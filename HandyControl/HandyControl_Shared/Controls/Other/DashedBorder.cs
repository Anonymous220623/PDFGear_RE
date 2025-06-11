// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.DashedBorder
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Expression.Drawing;
using HandyControl.Tools;
using HandyControl.Tools.Extension;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Controls;

public class DashedBorder : Decorator
{
  private bool _useComplexRenderCodePath;
  public static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register(nameof (BorderThickness), typeof (Thickness), typeof (DashedBorder), (PropertyMetadata) new FrameworkPropertyMetadata((object) new Thickness(), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(DashedBorder.OnClearPenCache)));
  public static readonly DependencyProperty BorderDashThicknessProperty = DependencyProperty.Register(nameof (BorderDashThickness), typeof (double), typeof (DashedBorder), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.Double0Box, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(DashedBorder.OnClearPenCache)));
  public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register(nameof (Padding), typeof (Thickness), typeof (DashedBorder), (PropertyMetadata) new FrameworkPropertyMetadata((object) new Thickness(), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof (CornerRadius), typeof (CornerRadius), typeof (DashedBorder), (PropertyMetadata) new FrameworkPropertyMetadata((object) new CornerRadius(), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty BorderBrushProperty = DependencyProperty.Register(nameof (BorderBrush), typeof (Brush), typeof (DashedBorder), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender, new PropertyChangedCallback(DashedBorder.OnClearPenCache)));
  public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(nameof (Background), typeof (Brush), typeof (DashedBorder), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender, new PropertyChangedCallback(DashedBorder.OnClearPenCache)));
  public static readonly DependencyProperty BorderDashArrayProperty = DependencyProperty.Register(nameof (BorderDashArray), typeof (DoubleCollection), typeof (DashedBorder), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(DashedBorder.OnClearPenCache)));
  public static readonly DependencyProperty BorderDashCapProperty = DependencyProperty.Register(nameof (BorderDashCap), typeof (PenLineCap), typeof (DashedBorder), (PropertyMetadata) new FrameworkPropertyMetadata((object) PenLineCap.Flat, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(DashedBorder.OnClearPenCache)));
  public static readonly DependencyProperty BorderDashOffsetProperty = DependencyProperty.Register(nameof (BorderDashOffset), typeof (double), typeof (DashedBorder), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.Double0Box, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(DashedBorder.OnClearPenCache)));

  private Pen GeometryPenCache { get; set; }

  private Pen LeftPenCache { get; set; }

  private Pen RightPenCache { get; set; }

  private Pen TopPenCache { get; set; }

  private Pen BottomPenCache { get; set; }

  private StreamGeometry BackgroundGeometryCache { get; set; }

  private StreamGeometry BorderGeometryCache { get; set; }

  private static void OnClearPenCache(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    DashedBorder dashedBorder = (DashedBorder) d;
    dashedBorder.LeftPenCache = (Pen) null;
    dashedBorder.RightPenCache = (Pen) null;
    dashedBorder.TopPenCache = (Pen) null;
    dashedBorder.BottomPenCache = (Pen) null;
    dashedBorder.GeometryPenCache = (Pen) null;
  }

  public Thickness BorderThickness
  {
    get => (Thickness) this.GetValue(DashedBorder.BorderThicknessProperty);
    set => this.SetValue(DashedBorder.BorderThicknessProperty, (object) value);
  }

  public double BorderDashThickness
  {
    get => (double) this.GetValue(DashedBorder.BorderDashThicknessProperty);
    set => this.SetValue(DashedBorder.BorderDashThicknessProperty, (object) value);
  }

  public Thickness Padding
  {
    get => (Thickness) this.GetValue(DashedBorder.PaddingProperty);
    set => this.SetValue(DashedBorder.PaddingProperty, (object) value);
  }

  public CornerRadius CornerRadius
  {
    get => (CornerRadius) this.GetValue(DashedBorder.CornerRadiusProperty);
    set => this.SetValue(DashedBorder.CornerRadiusProperty, (object) value);
  }

  public Brush BorderBrush
  {
    get => (Brush) this.GetValue(DashedBorder.BorderBrushProperty);
    set => this.SetValue(DashedBorder.BorderBrushProperty, (object) value);
  }

  public Brush Background
  {
    get => (Brush) this.GetValue(DashedBorder.BackgroundProperty);
    set => this.SetValue(DashedBorder.BackgroundProperty, (object) value);
  }

  public DoubleCollection BorderDashArray
  {
    get => (DoubleCollection) this.GetValue(DashedBorder.BorderDashArrayProperty);
    set => this.SetValue(DashedBorder.BorderDashArrayProperty, (object) value);
  }

  public PenLineCap BorderDashCap
  {
    get => (PenLineCap) this.GetValue(DashedBorder.BorderDashCapProperty);
    set => this.SetValue(DashedBorder.BorderDashCapProperty, (object) value);
  }

  public double BorderDashOffset
  {
    get => (double) this.GetValue(DashedBorder.BorderDashOffsetProperty);
    set => this.SetValue(DashedBorder.BorderDashOffsetProperty, (object) value);
  }

  private static Size ConvertThickness2Size(Thickness th)
  {
    return new Size(th.Left + th.Right, th.Top + th.Bottom);
  }

  private static Rect DeflateRect(Rect rt, Thickness thick)
  {
    return new Rect(rt.Left + thick.Left, rt.Top + thick.Top, Math.Max(0.0, rt.Width - thick.Left - thick.Right), Math.Max(0.0, rt.Height - thick.Top - thick.Bottom));
  }

  private static bool AreUniformCorners(CornerRadius borderRadii)
  {
    double topLeft = borderRadii.TopLeft;
    return MathHelper.AreClose(topLeft, borderRadii.TopRight) && MathHelper.AreClose(topLeft, borderRadii.BottomLeft) && MathHelper.AreClose(topLeft, borderRadii.BottomRight);
  }

  private static void GenerateGeometry(
    StreamGeometryContext ctx,
    Rect rect,
    in DashedBorder.Radii radii)
  {
    Point point1 = new Point(radii.LeftTop, 0.0);
    Point point2 = new Point(rect.Width - radii.RightTop, 0.0);
    Point point3 = new Point(rect.Width, radii.TopRight);
    Point point4 = new Point(rect.Width, rect.Height - radii.BottomRight);
    Point point5 = new Point(rect.Width - radii.RightBottom, rect.Height);
    Point point6 = new Point(radii.LeftBottom, rect.Height);
    Point point7 = new Point(0.0, rect.Height - radii.BottomLeft);
    Point point8 = new Point(0.0, radii.TopLeft);
    if (point1.X > point2.X)
    {
      double num = radii.LeftTop / (radii.LeftTop + radii.RightTop) * rect.Width;
      point1.X = num;
      point2.X = num;
    }
    if (point3.Y > point4.Y)
    {
      double num = radii.TopRight / (radii.TopRight + radii.BottomRight) * rect.Height;
      point3.Y = num;
      point4.Y = num;
    }
    if (point5.X < point6.X)
    {
      double num = radii.LeftBottom / (radii.LeftBottom + radii.RightBottom) * rect.Width;
      point5.X = num;
      point6.X = num;
    }
    if (point7.Y < point8.Y)
    {
      double num = radii.TopLeft / (radii.TopLeft + radii.BottomLeft) * rect.Height;
      point7.Y = num;
      point8.Y = num;
    }
    Vector vector;
    ref Vector local = ref vector;
    Point point9 = rect.TopLeft;
    double x1 = point9.X;
    point9 = rect.TopLeft;
    double y1 = point9.Y;
    local = new Vector(x1, y1);
    point1 += vector;
    point2 += vector;
    point3 += vector;
    point4 += vector;
    point5 += vector;
    point6 += vector;
    point7 += vector;
    point8 += vector;
    ctx.BeginFigure(point1, true, true);
    ctx.LineTo(point2, true, false);
    point9 = rect.TopRight;
    double width1 = point9.X - point2.X;
    double y2 = point3.Y;
    point9 = rect.TopRight;
    double y3 = point9.Y;
    double height1 = y2 - y3;
    if (!MathHelper.IsZero(width1) || !MathHelper.IsZero(height1))
      ctx.ArcTo(point3, new Size(width1, height1), 0.0, false, SweepDirection.Clockwise, true, false);
    ctx.LineTo(point4, true, false);
    point9 = rect.BottomRight;
    double width2 = point9.X - point5.X;
    point9 = rect.BottomRight;
    double height2 = point9.Y - point4.Y;
    if (!MathHelper.IsZero(width2) || !MathHelper.IsZero(height2))
      ctx.ArcTo(point5, new Size(width2, height2), 0.0, false, SweepDirection.Clockwise, true, false);
    ctx.LineTo(point6, true, false);
    double x2 = point6.X;
    point9 = rect.BottomLeft;
    double x3 = point9.X;
    double width3 = x2 - x3;
    point9 = rect.BottomLeft;
    double height3 = point9.Y - point7.Y;
    if (!MathHelper.IsZero(width3) || !MathHelper.IsZero(height3))
      ctx.ArcTo(point7, new Size(width3, height3), 0.0, false, SweepDirection.Clockwise, true, false);
    ctx.LineTo(point8, true, false);
    double x4 = point1.X;
    point9 = rect.TopLeft;
    double x5 = point9.X;
    double width4 = x4 - x5;
    double y4 = point8.Y;
    point9 = rect.TopLeft;
    double y5 = point9.Y;
    double height4 = y4 - y5;
    if (MathHelper.IsZero(width4) && MathHelper.IsZero(height4))
      return;
    ctx.ArcTo(point1, new Size(width4, height4), 0.0, false, SweepDirection.Clockwise, true, false);
  }

  protected override Size MeasureOverride(Size constraint)
  {
    UIElement child = this.Child;
    Thickness th = this.BorderThickness;
    Thickness padding = this.Padding;
    if (this.UseLayoutRounding)
    {
      double deviceDpiX = DpiHelper.DeviceDpiX;
      double deviceDpiY = DpiHelper.DeviceDpiY;
      th = new Thickness(DpiHelper.RoundLayoutValue(th.Left, deviceDpiX), DpiHelper.RoundLayoutValue(th.Top, deviceDpiY), DpiHelper.RoundLayoutValue(th.Right, deviceDpiX), DpiHelper.RoundLayoutValue(th.Bottom, deviceDpiY));
    }
    Size size1 = DashedBorder.ConvertThickness2Size(th);
    Size size2 = DashedBorder.ConvertThickness2Size(padding);
    Size size3 = new Size();
    if (child != null)
    {
      Size size4 = new Size(size1.Width + size2.Width, size1.Height + size2.Height);
      Size availableSize = new Size(Math.Max(0.0, constraint.Width - size4.Width), Math.Max(0.0, constraint.Height - size4.Height));
      child.Measure(availableSize);
      Size desiredSize = child.DesiredSize;
      size3.Width = desiredSize.Width + size4.Width;
      size3.Height = desiredSize.Height + size4.Height;
    }
    else
      size3 = new Size(size1.Width + size2.Width, size1.Height + size2.Height);
    return size3;
  }

  protected override Size ArrangeOverride(Size arrangeSize)
  {
    Thickness thickness = this.BorderThickness;
    if (this.UseLayoutRounding)
    {
      double deviceDpiX = DpiHelper.DeviceDpiX;
      double deviceDpiY = DpiHelper.DeviceDpiY;
      thickness = new Thickness(DpiHelper.RoundLayoutValue(thickness.Left, deviceDpiX), DpiHelper.RoundLayoutValue(thickness.Top, deviceDpiY), DpiHelper.RoundLayoutValue(thickness.Right, deviceDpiX), DpiHelper.RoundLayoutValue(thickness.Bottom, deviceDpiY));
    }
    Rect rect1 = new Rect(arrangeSize);
    Rect rect2 = DashedBorder.DeflateRect(rect1, thickness);
    UIElement child = this.Child;
    if (child != null)
    {
      Thickness padding = this.Padding;
      Rect finalRect = DashedBorder.DeflateRect(rect2, padding);
      child.Arrange(finalRect);
    }
    CornerRadius cornerRadius = this.CornerRadius;
    Brush borderBrush = this.BorderBrush;
    this._useComplexRenderCodePath = !DashedBorder.AreUniformCorners(cornerRadius);
    if (!this._useComplexRenderCodePath && borderBrush != null)
      this._useComplexRenderCodePath = !MathHelper.IsZero(cornerRadius.TopLeft) && !thickness.IsUniform();
    if (this._useComplexRenderCodePath)
    {
      DashedBorder.Radii radii1 = new DashedBorder.Radii(cornerRadius, thickness, false);
      StreamGeometry streamGeometry1 = (StreamGeometry) null;
      if (!MathHelper.IsZero(rect2.Width) && !MathHelper.IsZero(rect2.Height))
      {
        streamGeometry1 = new StreamGeometry();
        using (StreamGeometryContext ctx = streamGeometry1.Open())
          DashedBorder.GenerateGeometry(ctx, rect2, in radii1);
        streamGeometry1.Freeze();
        this.BackgroundGeometryCache = streamGeometry1;
      }
      else
        this.BackgroundGeometryCache = (StreamGeometry) null;
      if (!MathHelper.IsZero(rect1.Width) && !MathHelper.IsZero(rect1.Height))
      {
        DashedBorder.Radii radii2 = new DashedBorder.Radii(cornerRadius, thickness, true);
        StreamGeometry streamGeometry2 = new StreamGeometry();
        using (StreamGeometryContext ctx = streamGeometry2.Open())
        {
          DashedBorder.GenerateGeometry(ctx, rect1, in radii2);
          if (streamGeometry1 != null)
            DashedBorder.GenerateGeometry(ctx, rect2, in radii1);
        }
        streamGeometry2.Freeze();
        this.BorderGeometryCache = streamGeometry2;
      }
      else
        this.BorderGeometryCache = (StreamGeometry) null;
    }
    else
    {
      this.BackgroundGeometryCache = (StreamGeometry) null;
      this.BorderGeometryCache = (StreamGeometry) null;
    }
    return arrangeSize;
  }

  protected override void OnRender(DrawingContext drawingContext)
  {
    Brush background = this.Background;
    Brush borderBrush = this.BorderBrush;
    bool useLayoutRounding = this.UseLayoutRounding;
    if (this._useComplexRenderCodePath)
    {
      StreamGeometry borderGeometryCache = this.BorderGeometryCache;
      if (borderGeometryCache != null && borderBrush != null)
      {
        Pen pen = this.GeometryPenCache;
        if (pen == null)
        {
          pen = new Pen()
          {
            Brush = borderBrush,
            Thickness = this.BorderDashThickness,
            DashCap = this.BorderDashCap,
            DashStyle = new DashStyle((IEnumerable<double>) this.BorderDashArray, this.BorderDashOffset)
          };
          if (borderBrush.IsFrozen)
            pen.Freeze();
          this.GeometryPenCache = pen;
        }
        drawingContext.DrawGeometry((Brush) null, pen, (Geometry) borderGeometryCache);
      }
      StreamGeometry backgroundGeometryCache = this.BackgroundGeometryCache;
      if (backgroundGeometryCache == null || background == null)
        return;
      drawingContext.DrawGeometry(background, (Pen) null, (Geometry) backgroundGeometryCache);
    }
    else
    {
      double deviceDpiX = DpiHelper.DeviceDpiX;
      double deviceDpiY = DpiHelper.DeviceDpiY;
      Thickness borderThickness = this.BorderThickness;
      CornerRadius cornerRadius = this.CornerRadius;
      double topLeft1 = cornerRadius.TopLeft;
      bool flag = !MathHelper.IsZero(topLeft1);
      if (!borderThickness.IsZero() && borderBrush != null)
      {
        Pen pen1 = this.LeftPenCache;
        if (pen1 == null)
        {
          pen1 = new Pen()
          {
            Brush = borderBrush,
            Thickness = useLayoutRounding ? DpiHelper.RoundLayoutValue(borderThickness.Left, deviceDpiX) : borderThickness.Left,
            DashCap = this.BorderDashCap,
            DashStyle = new DashStyle((IEnumerable<double>) this.BorderDashArray, this.BorderDashOffset)
          };
          if (borderBrush.IsFrozen)
            pen1.Freeze();
          this.LeftPenCache = pen1;
        }
        Size renderSize = this.RenderSize;
        if (borderThickness.IsUniform())
        {
          double num = pen1.Thickness * 0.5;
          Rect rectangle = new Rect(new Point(num, num), new Point(renderSize.Width - num, renderSize.Height - num));
          if (flag)
            drawingContext.DrawRoundedRectangle((Brush) null, pen1, rectangle, topLeft1, topLeft1);
          else
            drawingContext.DrawRectangle((Brush) null, pen1, rectangle);
        }
        else
        {
          if (MathHelper.GreaterThan(borderThickness.Left, 0.0))
          {
            double x = pen1.Thickness * 0.5;
            drawingContext.DrawLine(pen1, new Point(x, 0.0), new Point(x, renderSize.Height));
          }
          if (MathHelper.GreaterThan(borderThickness.Right, 0.0))
          {
            Pen pen2 = this.RightPenCache;
            if (pen2 == null)
            {
              pen2 = new Pen()
              {
                Brush = borderBrush,
                Thickness = useLayoutRounding ? DpiHelper.RoundLayoutValue(borderThickness.Right, deviceDpiX) : borderThickness.Right,
                DashCap = this.BorderDashCap,
                DashStyle = new DashStyle((IEnumerable<double>) this.BorderDashArray, this.BorderDashOffset)
              };
              if (borderBrush.IsFrozen)
                pen2.Freeze();
              this.RightPenCache = pen2;
            }
            double num = pen2.Thickness * 0.5;
            drawingContext.DrawLine(pen2, new Point(renderSize.Width - num, 0.0), new Point(renderSize.Width - num, renderSize.Height));
          }
          if (MathHelper.GreaterThan(borderThickness.Top, 0.0))
          {
            Pen pen3 = this.TopPenCache;
            if (pen3 == null)
            {
              pen3 = new Pen()
              {
                Brush = borderBrush,
                Thickness = useLayoutRounding ? DpiHelper.RoundLayoutValue(borderThickness.Top, deviceDpiY) : borderThickness.Top,
                DashCap = this.BorderDashCap,
                DashStyle = new DashStyle((IEnumerable<double>) this.BorderDashArray, this.BorderDashOffset)
              };
              if (borderBrush.IsFrozen)
                pen3.Freeze();
              this.TopPenCache = pen3;
            }
            double y = pen3.Thickness * 0.5;
            drawingContext.DrawLine(pen3, new Point(0.0, y), new Point(renderSize.Width, y));
          }
          if (MathHelper.GreaterThan(borderThickness.Bottom, 0.0))
          {
            Pen pen4 = this.BottomPenCache;
            if (pen4 == null)
            {
              pen4 = new Pen()
              {
                Brush = borderBrush,
                Thickness = useLayoutRounding ? DpiHelper.RoundLayoutValue(borderThickness.Bottom, deviceDpiY) : borderThickness.Bottom,
                DashCap = this.BorderDashCap,
                DashStyle = new DashStyle((IEnumerable<double>) this.BorderDashArray, this.BorderDashOffset)
              };
              if (borderBrush.IsFrozen)
                pen4.Freeze();
              this.BottomPenCache = pen4;
            }
            double num = pen4.Thickness * 0.5;
            drawingContext.DrawLine(pen4, new Point(0.0, renderSize.Height - num), new Point(renderSize.Width, renderSize.Height - num));
          }
        }
      }
      if (background == null)
        return;
      Point point1;
      Point point2;
      if (useLayoutRounding)
      {
        point1 = new Point(DpiHelper.RoundLayoutValue(borderThickness.Left, deviceDpiX), DpiHelper.RoundLayoutValue(borderThickness.Top, deviceDpiY));
        ref Point local = ref point2;
        Size renderSize = this.RenderSize;
        double x = renderSize.Width - DpiHelper.RoundLayoutValue(borderThickness.Right, deviceDpiX);
        renderSize = this.RenderSize;
        double y = renderSize.Height - DpiHelper.RoundLayoutValue(borderThickness.Bottom, deviceDpiY);
        local = new Point(x, y);
      }
      else
      {
        point1 = new Point(borderThickness.Left, borderThickness.Top);
        point2 = new Point(this.RenderSize.Width - borderThickness.Right, this.RenderSize.Height - borderThickness.Bottom);
      }
      if (point2.X <= point1.X || point2.Y <= point1.Y)
        return;
      if (flag)
      {
        double topLeft2 = new DashedBorder.Radii(cornerRadius, borderThickness, false).TopLeft;
        drawingContext.DrawRoundedRectangle(background, (Pen) null, new Rect(point1, point2), topLeft2, topLeft2);
      }
      else
        drawingContext.DrawRectangle(background, (Pen) null, new Rect(point1, point2));
    }
  }

  private readonly struct Radii
  {
    internal readonly double LeftTop;
    internal readonly double TopLeft;
    internal readonly double TopRight;
    internal readonly double RightTop;
    internal readonly double RightBottom;
    internal readonly double BottomRight;
    internal readonly double BottomLeft;
    internal readonly double LeftBottom;

    internal Radii(CornerRadius radii, Thickness borders, bool outer)
    {
      double num1 = 0.5 * borders.Left;
      double num2 = 0.5 * borders.Top;
      double num3 = 0.5 * borders.Right;
      double num4 = 0.5 * borders.Bottom;
      if (outer)
      {
        if (MathHelper.IsZero(radii.TopLeft))
        {
          this.LeftTop = this.TopLeft = 0.0;
        }
        else
        {
          this.LeftTop = radii.TopLeft + num1;
          this.TopLeft = radii.TopLeft + num2;
        }
        if (MathHelper.IsZero(radii.TopRight))
        {
          this.TopRight = this.RightTop = 0.0;
        }
        else
        {
          this.TopRight = radii.TopRight + num2;
          this.RightTop = radii.TopRight + num3;
        }
        if (MathHelper.IsZero(radii.BottomRight))
        {
          this.RightBottom = this.BottomRight = 0.0;
        }
        else
        {
          this.RightBottom = radii.BottomRight + num3;
          this.BottomRight = radii.BottomRight + num4;
        }
        if (MathHelper.IsZero(radii.BottomLeft))
        {
          this.BottomLeft = this.LeftBottom = 0.0;
        }
        else
        {
          this.BottomLeft = radii.BottomLeft + num4;
          this.LeftBottom = radii.BottomLeft + num1;
        }
      }
      else
      {
        this.LeftTop = Math.Max(0.0, radii.TopLeft - num1);
        this.TopLeft = Math.Max(0.0, radii.TopLeft - num2);
        this.TopRight = Math.Max(0.0, radii.TopRight - num2);
        this.RightTop = Math.Max(0.0, radii.TopRight - num3);
        this.RightBottom = Math.Max(0.0, radii.BottomRight - num3);
        this.BottomRight = Math.Max(0.0, radii.BottomRight - num4);
        this.BottomLeft = Math.Max(0.0, radii.BottomLeft - num4);
        this.LeftBottom = Math.Max(0.0, radii.BottomLeft - num1);
      }
    }
  }
}
