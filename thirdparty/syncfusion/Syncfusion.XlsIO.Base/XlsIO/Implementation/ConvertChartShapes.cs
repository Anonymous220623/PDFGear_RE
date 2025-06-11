// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.ConvertChartShapes
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.Drawing;
using Syncfusion.XlsIO.Implementation.Charts;
using Syncfusion.XlsIO.Implementation.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class ConvertChartShapes
{
  private readonly PointF[] _arrowPoints = new PointF[4]
  {
    new PointF(0.0f, 0.0f),
    new PointF(-5f, -10f),
    new PointF(5f, -10f),
    new PointF(0.0f, 0.0f)
  };
  private readonly PointF[] _arrowOpenPoints = new PointF[3]
  {
    new PointF(-5f, -11f),
    new PointF(0.0f, -2f),
    new PointF(5f, -11f)
  };
  private readonly PointF[] _arrowStealthPoints = new PointF[5]
  {
    new PointF(0.0f, -6f),
    new PointF(5f, -10f),
    new PointF(0.0f, 0.0f),
    new PointF(-5f, -10f),
    new PointF(0.0f, -6f)
  };
  private readonly PointF[] _arrowDiamondPoints = new PointF[5]
  {
    new PointF(-5f, 0.0f),
    new PointF(0.0f, -5f),
    new PointF(5f, 0.0f),
    new PointF(0.0f, 5f),
    new PointF(-5f, 0.0f)
  };
  private StringFormat _stringFormat;
  private Graphics m_graphics;
  private RectangleF _currentCellRect;
  private WorkbookImpl m_workbook;
  private ChartImpl m_chartImpl;

  internal StringFormat StringFormt
  {
    get
    {
      StringFormat stringFormt = new StringFormat(StringFormat.GenericTypographic);
      stringFormt.FormatFlags &= ~StringFormatFlags.LineLimit;
      stringFormt.FormatFlags |= StringFormatFlags.NoClip;
      return stringFormt;
    }
  }

  internal ConvertChartShapes(WorkbookImpl workbook, ChartImpl chartImpl)
  {
    this.m_workbook = workbook;
    this.m_chartImpl = chartImpl;
  }

  internal void DrawChartShapes(Stream imageAsStream, int width, int height)
  {
    if (this.m_chartImpl.Shapes.Count <= 0)
      return;
    IShape[] shapes = new IShape[this.m_chartImpl.Shapes.Count];
    for (int index = 0; index < this.m_chartImpl.Shapes.Count; ++index)
      shapes[index] = this.m_chartImpl.Shapes[index];
    Image image = Image.FromStream(imageAsStream);
    int width1 = image.Width;
    int height1 = image.Height;
    double scaleWidth = (double) width1 / (double) width;
    double scaleHeight = (double) height1 / (double) height;
    Bitmap bitmap = new Bitmap(image.Width, image.Height);
    bitmap.SetResolution(96f, 96f);
    Graphics graphics = Graphics.FromImage((Image) bitmap);
    graphics.PageUnit = GraphicsUnit.Pixel;
    graphics.CompositingMode = CompositingMode.SourceCopy;
    graphics.CompositingQuality = CompositingQuality.HighQuality;
    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
    graphics.SmoothingMode = SmoothingMode.HighQuality;
    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
    graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height));
    this.DrawShapesCollection(shapes, graphics, scaleWidth, scaleHeight);
    imageAsStream.Position = 0L;
    imageAsStream.SetLength(0L);
    bitmap.Save(imageAsStream, image.RawFormat);
  }

  internal void DrawShapesCollection(
    IShape[] shapes,
    Graphics graphics,
    double scaleWidth,
    double scaleHeight)
  {
    foreach (IShape shape in shapes)
    {
      if (shape.IsShapeVisible)
      {
        switch (shape.ShapeType)
        {
          case ExcelShapeType.AutoShape:
          case ExcelShapeType.TextBox:
            this.DrawShape(shape as ShapeImpl, graphics, scaleWidth, scaleHeight);
            continue;
          case ExcelShapeType.Group:
            if ((shape as GroupShapeImpl).Group == null)
              (shape as GroupShapeImpl).LayoutGroupShape(true);
            this.DrawGroupShape(shape as IGroupShape, graphics, scaleWidth, scaleHeight);
            continue;
          case ExcelShapeType.Picture:
            this.DrawImage(shape as ShapeImpl, (shape as IPictureShape).Picture, graphics, scaleWidth, scaleHeight);
            continue;
          default:
            continue;
        }
      }
    }
  }

  internal void DrawGroupShape(
    IGroupShape groupShape,
    Graphics graphics,
    double scaledWidth,
    double scaledHeight)
  {
    this.DrawShapesCollection(groupShape.Items, graphics, scaledWidth, scaledHeight);
  }

  internal void DrawImage(
    ShapeImpl shape,
    Image image,
    Graphics graphics,
    double scaleWidth,
    double scaleHeight)
  {
    float x;
    float y;
    float width;
    float height;
    if (shape.GroupFrame != null)
    {
      x = (float) (ApplicationImpl.ConvertToPixels((double) shape.GroupFrame.OffsetX, MeasureUnits.EMU) * scaleWidth);
      y = (float) (ApplicationImpl.ConvertToPixels((double) shape.GroupFrame.OffsetY, MeasureUnits.EMU) * scaleHeight);
      width = (float) (ApplicationImpl.ConvertToPixels((double) shape.GroupFrame.OffsetCX, MeasureUnits.EMU) * scaleWidth);
      height = (float) (ApplicationImpl.ConvertToPixels((double) shape.GroupFrame.OffsetCY, MeasureUnits.EMU) * scaleHeight);
    }
    else
    {
      x = (float) ApplicationImpl.ConvertFromPixel(shape.ChartShapeX * scaleWidth, MeasureUnits.Pixel);
      y = (float) ApplicationImpl.ConvertFromPixel(shape.ChartShapeY * scaleHeight, MeasureUnits.Pixel);
      width = (float) ApplicationImpl.ConvertFromPixel(shape.ChartShapeWidth * scaleWidth, MeasureUnits.Pixel);
      height = (float) ApplicationImpl.ConvertFromPixel(shape.ChartShapeHeight * scaleHeight, MeasureUnits.Pixel);
    }
    RectangleF rectangleF = new RectangleF(x, y, width, height);
    if ((double) rectangleF.Width == 0.0)
      rectangleF.Width = 0.1f;
    if ((double) rectangleF.Height == 0.0)
      rectangleF.Height = 0.1f;
    graphics.ResetTransform();
    ImageAttributes imageAttributes1 = (ImageAttributes) null;
    if (shape is BitmapShapeImpl)
    {
      BitmapShapeImpl bitmapShapeImpl = shape as BitmapShapeImpl;
      if (bitmapShapeImpl.GrayScale || bitmapShapeImpl.Threshold > 0)
        image = (Image) this.ApplyRecolor(bitmapShapeImpl, image);
      if (bitmapShapeImpl.ColorChange != null && bitmapShapeImpl.ColorChange.Count == 2)
      {
        if (bitmapShapeImpl.ColorChange[1].GetRGB((IWorkbook) this.m_workbook).A == (byte) 0)
        {
          Bitmap bitmap = new Bitmap(image);
          Color pixel = bitmap.GetPixel(1, 1);
          bitmap.MakeTransparent(pixel);
          MemoryStream memoryStream = new MemoryStream();
          bitmap.Save((Stream) memoryStream, ImageFormat.Png);
          image = Image.FromStream((Stream) memoryStream);
        }
        else
        {
          ImageAttributes imageAttributes2 = new ImageAttributes();
          imageAttributes1 = this.ColorChange(bitmapShapeImpl, imageAttributes2);
        }
      }
      if (bitmapShapeImpl.DuoTone != null && bitmapShapeImpl.DuoTone.Count == 2)
        image = this.ApplyDuoTone(image, bitmapShapeImpl.DuoTone);
      double transparency = 1.0;
      if (bitmapShapeImpl.Amount / 100000 < 1)
      {
        if (transparency < 0.0)
          transparency = 0.0;
        if (imageAttributes1 == null)
          imageAttributes1 = new ImageAttributes();
        this.ApplyImageTransparency(imageAttributes1, (float) transparency);
      }
      if (imageAttributes1 != null)
      {
        Bitmap bitmap = new Bitmap(image.Width, image.Height);
        Graphics graphics1 = Graphics.FromImage((Image) bitmap);
        graphics1.DrawImage(image, new Rectangle(0, 0, bitmap.Width, bitmap.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttributes1);
        image = (Image) bitmap;
        graphics1.Dispose();
      }
    }
    MemoryStream memoryStream1 = new MemoryStream();
    if (shape is BitmapShapeImpl)
    {
      BitmapShapeImpl bitmapShapeImpl = shape as BitmapShapeImpl;
      double leftOffset = (double) bitmapShapeImpl.CropLeftOffset / 1000.0;
      double topOffset = (double) (bitmapShapeImpl.CropTopOffset / 1000);
      double rightOffset = (double) (bitmapShapeImpl.CropRightOffset / 1000);
      double bottomOffset = (double) (bitmapShapeImpl.CropBottomOffset / 1000);
      bool flag = image.RawFormat.Equals((object) ImageFormat.Png);
      if ((double) rectangleF.Height < (double) image.Size.Height && (double) rectangleF.Width < (double) image.Width && bitmapShapeImpl.CropLeftOffset > 0 && bitmapShapeImpl.CropTopOffset > 0 && bitmapShapeImpl.CropRightOffset > 0 && bitmapShapeImpl.CropLeftOffset > 0)
        image = ConvertChartShapes.CropHFImage(image, leftOffset, topOffset, rightOffset, bottomOffset, bitmapShapeImpl.HasTransparency);
      if (bitmapShapeImpl.HasTransparency || flag)
        image.Save((Stream) memoryStream1, ImageFormat.Png);
    }
    graphics.ResetTransform();
    if (shape is BitmapShapeImpl)
      this.Rotate(graphics, (ShapeImpl) (shape as BitmapShapeImpl), rectangleF);
    graphics.DrawImage(image, rectangleF);
    if (shape is BitmapShapeImpl && shape.Line.Visible)
    {
      Pen pen = this.CreatePen(shape, scaleWidth);
      graphics.DrawRectangle(pen, rectangleF.X - pen.Width / 2f, rectangleF.Y - pen.Width / 2f, rectangleF.Width + pen.Width, rectangleF.Height + pen.Width);
    }
    graphics.ResetTransform();
  }

  public static Image CropHFImage(
    Image cropableImage,
    double leftOffset,
    double topOffset,
    double rightOffset,
    double bottomOffset,
    bool isTransparent)
  {
    double width = (double) cropableImage.Width;
    double height = (double) cropableImage.Height;
    leftOffset = width * (leftOffset / 100.0);
    topOffset = height * (topOffset / 100.0);
    rightOffset = width * (rightOffset / 100.0);
    bottomOffset = height * (bottomOffset / 100.0);
    RectangleF rect = new RectangleF((float) -leftOffset, (float) -topOffset, (float) width, (float) height);
    Bitmap bitmap1 = new Bitmap((int) (width - leftOffset - rightOffset), (int) (height - topOffset - bottomOffset));
    bitmap1.SetResolution(cropableImage.VerticalResolution, cropableImage.HorizontalResolution);
    Graphics graphics = Graphics.FromImage((Image) bitmap1);
    graphics.Clear(Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
    if (isTransparent)
    {
      graphics.CompositingMode = CompositingMode.SourceCopy;
      graphics.CompositingQuality = CompositingQuality.HighQuality;
      graphics.SmoothingMode = SmoothingMode.AntiAlias;
      graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
      graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
    }
    graphics.DrawImage(cropableImage, rect);
    MemoryStream memoryStream = new MemoryStream();
    bitmap1.Save((Stream) memoryStream, ImageFormat.Png);
    Bitmap bitmap2 = Image.FromStream((Stream) memoryStream) as Bitmap;
    memoryStream.Dispose();
    return (Image) bitmap2;
  }

  private ImageAttributes ColorChange(BitmapShapeImpl pictureImpl, ImageAttributes imageAttributes)
  {
    List<ColorObject> colorChange = pictureImpl.ColorChange;
    ColorObject colorObject1 = colorChange[0];
    ColorObject colorObject2 = colorChange[1];
    ColorMap[] map = new ColorMap[1]{ new ColorMap() };
    map[0].OldColor = colorObject1.GetRGB((IWorkbook) this.m_workbook);
    map[0].NewColor = !pictureImpl.IsUseAlpha ? Color.FromArgb((int) colorObject2.GetRGB((IWorkbook) this.m_workbook).R, (int) colorObject2.GetRGB((IWorkbook) this.m_workbook).G, (int) colorObject2.GetRGB((IWorkbook) this.m_workbook).B) : colorObject2.GetRGB((IWorkbook) this.m_workbook);
    imageAttributes.SetRemapTable(map);
    return imageAttributes;
  }

  private Image ApplyDuoTone(Image image, List<ColorObject> duotone)
  {
    if (duotone.Count != 2)
      return image;
    ColorObject colorObject1 = new ColorObject(ColorExtension.Empty);
    ColorObject colorObject2 = new ColorObject(ColorExtension.Empty);
    Bitmap bitmap1 = image as Bitmap;
    Bitmap bitmap2 = new Bitmap(bitmap1.Width, bitmap1.Height, bitmap1.PixelFormat);
    ColorObject colorObject3 = duotone[1];
    ColorObject colorObject4 = duotone[0];
    Color color1 = Color.FromArgb((int) byte.MaxValue - (int) colorObject4.GetRGB((IWorkbook) this.m_workbook).A, colorObject4.GetRGB((IWorkbook) this.m_workbook));
    Color color2 = Color.FromArgb((int) byte.MaxValue - (int) colorObject3.GetRGB((IWorkbook) this.m_workbook).A, colorObject3.GetRGB((IWorkbook) this.m_workbook));
    Rectangle rect = new Rectangle(0, 0, bitmap1.Width, bitmap1.Height);
    BitmapData bitmapdata1 = bitmap1.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
    BitmapData bitmapdata2 = bitmap2.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
    int length1 = Math.Abs(bitmapdata1.Stride) * bitmap1.Height;
    byte[] destination = new byte[length1];
    Marshal.Copy(bitmapdata1.Scan0, destination, 0, length1);
    int length2 = Math.Abs(bitmapdata2.Stride) * bitmap2.Height;
    byte[] numArray = new byte[length2];
    Marshal.Copy(bitmapdata2.Scan0, numArray, 0, length2);
    for (int index = 0; index < length2; index += 4)
    {
      Color inputPixelColor = Color.FromArgb((int) destination[index + 3], (int) destination[index + 2], (int) destination[index + 1], (int) destination[index]);
      float num = (float) Math.Sqrt(0.299 * (double) inputPixelColor.R * (double) inputPixelColor.R + 0.587 * (double) inputPixelColor.G * (double) inputPixelColor.G + 0.114 * (double) inputPixelColor.B * (double) inputPixelColor.B);
      float factor = num / (float) byte.MaxValue;
      Color empty = Color.Empty;
      Color color3 = (double) num == (double) byte.MaxValue || (double) num == 0.0 ? ((double) num != (double) byte.MaxValue ? Color.FromArgb((int) inputPixelColor.A, color1) : Color.FromArgb((int) inputPixelColor.A, color2)) : this.ExecuteLinearInterpolation(color1, color2, inputPixelColor, factor);
      numArray[index] = color3.B;
      numArray[index + 1] = color3.G;
      numArray[index + 2] = color3.R;
      numArray[index + 3] = color3.A;
    }
    Marshal.Copy(numArray, 0, bitmapdata2.Scan0, length2);
    bitmap1.UnlockBits(bitmapdata1);
    bitmap2.UnlockBits(bitmapdata2);
    bitmap1.Dispose();
    return (Image) bitmap2;
  }

  private Color ExecuteLinearInterpolation(
    Color firstColor,
    Color secondColor,
    Color inputPixelColor,
    float factor)
  {
    int red = (int) ((1.0 - (double) factor) * (double) firstColor.R + (double) factor * (double) secondColor.R);
    int green = (int) ((1.0 - (double) factor) * (double) firstColor.G + (double) factor * (double) secondColor.G);
    int blue = (int) ((1.0 - (double) factor) * (double) firstColor.B + (double) factor * (double) secondColor.B);
    return Color.FromArgb((int) inputPixelColor.A, red, green, blue);
  }

  private Bitmap CreateNonIndexedImage(Image sourceImage)
  {
    using (Graphics graphics = Graphics.FromImage((Image) new Bitmap(sourceImage.Width, sourceImage.Height, PixelFormat.Format32bppArgb)))
      graphics.DrawImage(sourceImage, 0, 0);
    return sourceImage as Bitmap;
  }

  private void ApplyImageTransparency(ImageAttributes imgAttribute, float transparency)
  {
    imgAttribute.SetColorMatrix(new ColorMatrix()
    {
      Matrix33 = transparency
    }, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
  }

  private Bitmap ApplyRecolor(BitmapShapeImpl picture, Image image)
  {
    Bitmap bitmap = image.PixelFormat.ToString().ToLower().Contains("rgb") ? (Bitmap) image : this.CreateNonIndexedImage(image);
    for (int x = 0; x < bitmap.Width; ++x)
    {
      for (int y = 0; y < bitmap.Height; ++y)
      {
        Color pixel = bitmap.GetPixel(x, y);
        if (picture.GrayScale)
        {
          byte num = (byte) (0.299 * (double) pixel.R + 0.587 * (double) pixel.G + 0.114 * (double) pixel.B);
          bitmap.SetPixel(x, y, Color.FromArgb((int) pixel.A, (int) num, (int) num, (int) num));
        }
        else
        {
          int maxValue = (0.299 * (double) pixel.R + 0.587 * (double) pixel.G + 0.114 * (double) pixel.B) / 2.5 >= (double) (picture.Threshold / 1000) ? (int) byte.MaxValue : 0;
          bitmap.SetPixel(x, y, Color.FromArgb((int) pixel.A, maxValue, maxValue, maxValue));
        }
      }
    }
    return bitmap;
  }

  internal void DrawShape(
    ShapeImpl shape,
    Graphics graphics,
    double scaleWidth,
    double scaleHeight)
  {
    if (!shape.IsShapeVisible)
      return;
    float x;
    float y;
    float width;
    float height;
    if (shape.GroupFrame != null)
    {
      x = (float) (ApplicationImpl.ConvertToPixels((double) shape.GroupFrame.OffsetX, MeasureUnits.EMU) * scaleWidth);
      y = (float) (ApplicationImpl.ConvertToPixels((double) shape.GroupFrame.OffsetY, MeasureUnits.EMU) * scaleHeight);
      width = (float) (ApplicationImpl.ConvertToPixels((double) shape.GroupFrame.OffsetCX, MeasureUnits.EMU) * scaleWidth);
      height = (float) (ApplicationImpl.ConvertToPixels((double) shape.GroupFrame.OffsetCY, MeasureUnits.EMU) * scaleHeight);
    }
    else
    {
      x = (float) ApplicationImpl.ConvertFromPixel(shape.ChartShapeX * scaleWidth, MeasureUnits.Pixel);
      y = (float) ApplicationImpl.ConvertFromPixel(shape.ChartShapeY * scaleHeight, MeasureUnits.Pixel);
      width = (float) ApplicationImpl.ConvertFromPixel(shape.ChartShapeWidth * scaleWidth, MeasureUnits.Pixel);
      height = (float) ApplicationImpl.ConvertFromPixel(shape.ChartShapeHeight * scaleHeight, MeasureUnits.Pixel);
    }
    RectangleF rectangleF = new RectangleF(x, y, width, height);
    if ((double) rectangleF.Width == 0.0)
      rectangleF.Width = 0.1f;
    if ((double) rectangleF.Height == 0.0)
      rectangleF.Height = 0.1f;
    graphics.ResetTransform();
    rectangleF = shape.UpdateShapeBounds(rectangleF, shape.GetShapeRotation());
    this.Rotate(graphics, shape, rectangleF);
    Pen pen = this.CreatePen(shape, shape.Line, scaleWidth);
    GraphicsPath graphicsPath;
    if (shape is TextBoxShapeImpl)
    {
      graphicsPath = new GraphicsPath();
      graphicsPath.AddRectangle(rectangleF);
    }
    else
      graphicsPath = this.GetGraphicsPath(rectangleF, ref pen, graphics, shape as AutoShapeImpl);
    this.DrawShapeFillAndLine(graphicsPath, shape, pen, graphics, rectangleF);
    IRichTextString richText = (IRichTextString) null;
    switch (shape)
    {
      case TextBoxShapeImpl _:
        richText = (shape as TextBoxShapeImpl).RichText;
        break;
      case CommentShapeImpl _:
        richText = (shape as CommentShapeImpl).RichText;
        break;
      case AutoShapeImpl _:
        richText = (shape as AutoShapeImpl).TextFrame.TextRange.RichText;
        break;
    }
    graphics.CompositingMode = CompositingMode.SourceOver;
    this.DrawShapeRTFText(richText, shape, rectangleF, graphics, scaleWidth, scaleHeight);
    graphics.CompositingMode = CompositingMode.SourceCopy;
    graphics.ResetTransform();
  }

  private void RotateText(RectangleF bounds, TextDirection textDirectionType, Graphics graphics)
  {
    switch (textDirectionType)
    {
      case TextDirection.RotateAllText90:
        graphics.TranslateTransform(bounds.X + bounds.Y + bounds.Height, bounds.Y - bounds.X);
        graphics.RotateTransform(90f);
        break;
      case TextDirection.RotateAllText270:
        graphics.TranslateTransform(bounds.X - bounds.Y, bounds.X + bounds.Y + bounds.Width);
        graphics.RotateTransform(270f);
        break;
    }
  }

  private void ApplyRotation(
    ShapeImpl shape,
    RectangleF bounds,
    float rotationAngle,
    Graphics graphics)
  {
    bool flag1 = false;
    bool flag2 = false;
    if (shape is AutoShapeImpl)
    {
      flag1 = (shape as AutoShapeImpl).ShapeExt.FlipVertical;
      flag2 = (shape as AutoShapeImpl).ShapeExt.FlipHorizontal;
    }
    else if (shape is TextBoxShapeImpl)
    {
      flag1 = (shape as TextBoxShapeImpl).FlipVertical;
      flag2 = (shape as TextBoxShapeImpl).FlipHorizontal;
    }
    if (shape.Group != null && (this.IsGroupFlipH(shape.Group) || this.IsGroupFlipV(shape.Group)))
    {
      int flipVcount = this.GetFlipVCount(shape.Group, flag1 ? 1 : 0);
      int flipHcount = this.GetFlipHCount(shape.Group, flag2 ? 1 : 0);
      rotationAngle = (float) shape.GetShapeRotation();
      if (flipVcount % 2 != 0)
      {
        graphics.Transform = this.GetTransformMatrix(bounds, rotationAngle, true, true);
      }
      else
      {
        if (flipHcount % 2 == 0)
          return;
        graphics.Transform = this.GetTransformMatrix(bounds, rotationAngle, false, false);
      }
    }
    else if (flag1)
    {
      graphics.Transform = this.GetTransformMatrix(bounds, rotationAngle, true, true);
    }
    else
    {
      if (!flag2)
        return;
      graphics.Transform = this.GetTransformMatrix(bounds, rotationAngle, false, false);
    }
  }

  private void UpdateShapeBoundsToLayoutTextBody(
    ref RectangleF layoutRect,
    RectangleF shapeBounds,
    ShapeImpl shape,
    double scaledWidth,
    double scaledHeight)
  {
    float num1 = 7.2f;
    float num2 = 7.2f;
    float num3 = 3.6f;
    float num4 = 3.6f;
    float num5;
    float num6;
    float num7;
    float num8;
    switch (shape)
    {
      case AutoShapeImpl _:
        layoutRect.Height -= layoutRect.Y;
        layoutRect.Y += shapeBounds.Y;
        layoutRect.Width -= layoutRect.X;
        layoutRect.X += shapeBounds.X;
        num5 = (float) ((shape.TextFrame as TextFrame).TextBodyProperties.LeftMarginPt * scaledWidth);
        num6 = (float) ((shape.TextFrame as TextFrame).TextBodyProperties.RightMarginPt * scaledWidth);
        num7 = (float) ((shape.TextFrame as TextFrame).TextBodyProperties.TopMarginPt * scaledHeight);
        num8 = (float) ((shape.TextFrame as TextFrame).TextBodyProperties.BottomMarginPt * scaledHeight);
        break;
      case TextBoxShapeImpl _:
        num5 = (float) ((shape as TextBoxShapeImpl).TextBodyPropertiesHolder.LeftMarginPt * scaledWidth);
        num6 = (float) ((shape as TextBoxShapeImpl).TextBodyPropertiesHolder.RightMarginPt * scaledWidth);
        num7 = (float) ((shape as TextBoxShapeImpl).TextBodyPropertiesHolder.TopMarginPt * scaledHeight);
        num8 = (float) ((shape as TextBoxShapeImpl).TextBodyPropertiesHolder.BottomMarginPt * scaledHeight);
        break;
      case CommentShapeImpl _:
        num5 = (float) (shape as CommentShapeImpl).TextBodyPropertiesHolder.LeftMarginPt;
        num6 = (float) (shape as CommentShapeImpl).TextBodyPropertiesHolder.RightMarginPt;
        num7 = (float) (shape as CommentShapeImpl).TextBodyPropertiesHolder.TopMarginPt;
        num8 = (float) (shape as CommentShapeImpl).TextBodyPropertiesHolder.BottomMarginPt;
        break;
      default:
        num5 = num1 * (float) scaledWidth;
        num6 = num2 * (float) scaledWidth;
        num7 = num3 * (float) scaledHeight;
        num8 = num4 * (float) scaledHeight;
        break;
    }
    layoutRect.X += num5;
    layoutRect.Y += num7;
    layoutRect.Width -= num5 + num6;
    layoutRect.Height -= num7 + num8;
  }

  private StringAlignment GetVerticalAlignmentFromShape(IShape shape)
  {
    StringAlignment alignmentFromShape = StringAlignment.Center;
    switch (shape)
    {
      case AutoShapeImpl _:
        switch (shape.TextFrame.VerticalAlignment)
        {
          case ExcelVerticalAlignment.Middle:
          case ExcelVerticalAlignment.MiddleCentered:
            alignmentFromShape = StringAlignment.Center;
            break;
          case ExcelVerticalAlignment.Bottom:
          case ExcelVerticalAlignment.BottomCentered:
            alignmentFromShape = StringAlignment.Far;
            break;
          default:
            alignmentFromShape = StringAlignment.Near;
            break;
        }
        break;
      case TextBoxShapeImpl _:
        switch ((shape as TextBoxShapeImpl).VAlignment)
        {
          case ExcelCommentVAlign.Center:
            alignmentFromShape = StringAlignment.Center;
            break;
          case ExcelCommentVAlign.Bottom:
            alignmentFromShape = StringAlignment.Far;
            break;
          default:
            alignmentFromShape = StringAlignment.Near;
            break;
        }
        break;
    }
    return alignmentFromShape;
  }

  private float GetRotationAngle(TextDirection textDirection)
  {
    float rotationAngle = 0.0f;
    switch (textDirection)
    {
      case TextDirection.RotateAllText90:
        rotationAngle = 90f;
        break;
      case TextDirection.RotateAllText270:
        rotationAngle = 270f;
        break;
    }
    return rotationAngle;
  }

  private StringAlignment GetTextAlignmentFromShape(IShape shape)
  {
    StringAlignment alignmentFromShape = StringAlignment.Near;
    switch (shape)
    {
      case AutoShapeImpl _:
        switch (shape.TextFrame.HorizontalAlignment)
        {
          case ExcelHorizontalAlignment.Left:
            alignmentFromShape = StringAlignment.Near;
            break;
          case ExcelHorizontalAlignment.Center:
            alignmentFromShape = StringAlignment.Center;
            break;
          case ExcelHorizontalAlignment.Right:
            alignmentFromShape = StringAlignment.Far;
            break;
        }
        break;
      case TextBoxShapeImpl _:
        switch ((shape as TextBoxShapeImpl).HAlignment)
        {
          case ExcelCommentHAlign.Left:
            alignmentFromShape = StringAlignment.Near;
            break;
          case ExcelCommentHAlign.Center:
            alignmentFromShape = StringAlignment.Center;
            break;
          case ExcelCommentHAlign.Right:
            alignmentFromShape = StringAlignment.Far;
            break;
        }
        break;
    }
    return alignmentFromShape;
  }

  private void DrawShapeRTFText(
    IRichTextString richText,
    ShapeImpl shape,
    RectangleF rect,
    Graphics graphics,
    double scaledWidth,
    double scaledHeight)
  {
    bool isWrapText = true;
    bool isVerticalTextOverflow = false;
    bool isHorizontalTextOverflow = false;
    if (string.IsNullOrEmpty(richText.Text))
      return;
    this._currentCellRect = this.GetBoundsToLayoutShapeTextBody(shape as AutoShapeImpl, rect);
    this.UpdateShapeBoundsToLayoutTextBody(ref this._currentCellRect, rect, shape, scaledWidth, scaledHeight);
    if (shape is TextBoxShapeImpl)
    {
      string textVerticalType = ((Excel2007TextRotation) (shape as TextBoxShapeImpl).TextRotation).ToString();
      (shape as TextBoxShapeImpl).TextBodyPropertiesHolder.TextDirection = Helper.SetTextDirection(textVerticalType);
    }
    TextDirection textDirection = shape is AutoShapeImpl ? shape.TextFrame.TextDirection : (shape is CommentShapeImpl ? (shape as CommentShapeImpl).TextBodyPropertiesHolder.TextDirection : (shape as TextBoxShapeImpl).TextBodyPropertiesHolder.TextDirection);
    switch (textDirection)
    {
      case TextDirection.RotateAllText90:
      case TextDirection.RotateAllText270:
        float width = this._currentCellRect.Width;
        this._currentCellRect.Width = this._currentCellRect.Height;
        this._currentCellRect.Height = width;
        break;
    }
    float shapeRotation = (float) shape.ShapeRotation;
    this.ApplyRotation(shape, rect, shapeRotation, graphics);
    this.RotateText(this._currentCellRect, textDirection, graphics);
    IFont font = richText.GetFont(0);
    StringFormat stringFormt = this.StringFormt;
    stringFormt.Alignment = this.GetTextAlignmentFromShape((IShape) shape);
    this.GetFont(font, font.FontName, (int) font.Size);
    stringFormt.LineAlignment = this.GetVerticalAlignmentFromShape((IShape) shape);
    if ((double) this.GetRotationAngle(textDirection) > 0.0)
      this.UpdateAlignment(stringFormt, (int) this.GetRotationAngle(textDirection), shape);
    stringFormt.Trimming = StringTrimming.Word;
    stringFormt.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
    this._stringFormat = stringFormt;
    List<IFont> richTextFonts = new List<IFont>();
    List<string> drawString = this.m_workbook.GetDrawString(richText.Text, richText as RichTextString, out richTextFonts, richText.GetFont(0));
    if (drawString.Count > 1 && drawString[drawString.Count - 1].Equals("\n"))
      drawString.RemoveAt(drawString.Count - 1);
    if (shape is AutoShapeImpl)
    {
      for (int index = 0; index < richTextFonts.Count; ++index)
      {
        if (this.FontColorNeedsUpdation(richTextFonts[index]))
        {
          if (richTextFonts[index] is FontWrapper parent)
          {
            richTextFonts[index] = (IFont) new FontWrapper(parent.Font.Clone((object) parent), false, false)
            {
              RGBColor = shape.GetDefaultColor(PreservedFlag.RichText, "fontRef")
            };
          }
          else
          {
            FontImpl fontImpl = (richTextFonts[index] as FontImpl).Clone() as FontImpl;
            fontImpl.RGBColor = shape.GetDefaultColor(PreservedFlag.RichText, "fontRef");
            richTextFonts[index] = (IFont) fontImpl;
          }
        }
      }
    }
    if (shape is AutoShapeImpl)
    {
      AutoShapeImpl autoShapeImpl = shape as AutoShapeImpl;
      if (autoShapeImpl.TextFrameInternal != null && autoShapeImpl.TextFrameInternal.TextBodyProperties != null)
      {
        isWrapText = autoShapeImpl.TextFrameInternal.TextBodyProperties.WrapTextInShape;
        if (!isWrapText)
          isWrapText = false;
        if (autoShapeImpl.TextFrameInternal.TextBodyProperties.TextVertOverflowType == TextVertOverflowType.OverFlow)
          isVerticalTextOverflow = true;
        if (autoShapeImpl.TextFrameInternal.TextBodyProperties.TextHorzOverflowType == TextHorzOverflowType.OverFlow)
          isHorizontalTextOverflow = true;
      }
    }
    else
    {
      string empty = string.Empty;
      TextBoxShapeBase textBoxShapeBase = shape as TextBoxShapeBase;
      if (textBoxShapeBase.UnknownBodyProperties != null)
      {
        textBoxShapeBase.UnknownBodyProperties.TryGetValue("wrap", out empty);
        if (empty == "none")
          isWrapText = false;
        textBoxShapeBase.UnknownBodyProperties.TryGetValue("vertOverflow", out empty);
        if (empty == "overflow" || string.IsNullOrEmpty(empty))
          isVerticalTextOverflow = true;
        textBoxShapeBase.UnknownBodyProperties.TryGetValue("horzOverflow", out empty);
        if (empty == "overflow" || string.IsNullOrEmpty(empty))
          isHorizontalTextOverflow = true;
      }
    }
    List<IFont> richTextFont = new List<IFont>();
    if (richTextFonts.Count > 0)
    {
      foreach (FontImpl baseFont in richTextFonts)
      {
        double num = baseFont.Size * scaledWidth;
        richTextFont.Add((IFont) new FontImpl((IFont) baseFont)
        {
          Size = num
        });
      }
    }
    this.DrawRTFText(this._currentCellRect, this._currentCellRect, graphics, richTextFont, drawString, true, isWrapText, isHorizontalTextOverflow, isVerticalTextOverflow);
  }

  private StringFormat UpdateAlignment(StringFormat format, int rotationAngle, ShapeImpl shape)
  {
    ExcelHorizontalAlignment horizontalAlignment = ExcelHorizontalAlignment.Left;
    ExcelVerticalAlignment verticalAlignment = ExcelVerticalAlignment.Bottom;
    TextBoxShapeImpl textBoxShapeImpl = shape as TextBoxShapeImpl;
    if (shape is AutoShapeImpl || textBoxShapeImpl != null && textBoxShapeImpl.IsCreated)
    {
      if (rotationAngle == 270)
      {
        switch (format.Alignment)
        {
          case StringAlignment.Near:
            verticalAlignment = ExcelVerticalAlignment.Top;
            break;
          case StringAlignment.Center:
            verticalAlignment = ExcelVerticalAlignment.Middle;
            break;
          case StringAlignment.Far:
            verticalAlignment = ExcelVerticalAlignment.Bottom;
            break;
        }
        switch (format.LineAlignment)
        {
          case StringAlignment.Near:
            horizontalAlignment = ExcelHorizontalAlignment.Left;
            break;
          case StringAlignment.Center:
            horizontalAlignment = ExcelHorizontalAlignment.Center;
            break;
          case StringAlignment.Far:
            horizontalAlignment = ExcelHorizontalAlignment.Right;
            break;
        }
      }
      else
      {
        switch (format.Alignment)
        {
          case StringAlignment.Near:
            verticalAlignment = ExcelVerticalAlignment.Bottom;
            break;
          case StringAlignment.Center:
            verticalAlignment = ExcelVerticalAlignment.Middle;
            break;
          case StringAlignment.Far:
            verticalAlignment = ExcelVerticalAlignment.Top;
            break;
        }
        switch (format.LineAlignment)
        {
          case StringAlignment.Near:
            horizontalAlignment = ExcelHorizontalAlignment.Right;
            break;
          case StringAlignment.Center:
            horizontalAlignment = ExcelHorizontalAlignment.Center;
            break;
          case StringAlignment.Far:
            horizontalAlignment = ExcelHorizontalAlignment.Left;
            break;
        }
      }
    }
    StringFormat stringFormat1 = format;
    int num1;
    switch (horizontalAlignment)
    {
      case ExcelHorizontalAlignment.Left:
        num1 = 0;
        break;
      case ExcelHorizontalAlignment.Center:
        num1 = 1;
        break;
      case ExcelHorizontalAlignment.Right:
        num1 = 2;
        break;
      default:
        num1 = (int) format.Alignment;
        break;
    }
    stringFormat1.Alignment = (StringAlignment) num1;
    StringFormat stringFormat2 = format;
    int num2;
    switch (verticalAlignment)
    {
      case ExcelVerticalAlignment.Top:
        num2 = 0;
        break;
      case ExcelVerticalAlignment.Middle:
        num2 = 1;
        break;
      case ExcelVerticalAlignment.Bottom:
        num2 = 2;
        break;
      default:
        num2 = (int) format.LineAlignment;
        break;
    }
    stringFormat2.LineAlignment = (StringAlignment) num2;
    return format;
  }

  private void DrawRTFText(
    RectangleF cellRect,
    RectangleF adjacentRect,
    Graphics graphics,
    List<IFont> richTextFont,
    List<string> drawString,
    bool isShape,
    bool isWrapText,
    bool isHorizontalTextOverflow,
    bool isVerticalTextOverflow)
  {
    new GDIRenderer(graphics, this._stringFormat, richTextFont, drawString, this.m_workbook, 1).DrawRTFText(cellRect, adjacentRect, isShape, isWrapText, isHorizontalTextOverflow, isVerticalTextOverflow, true, false);
  }

  private Font GetFont(IFont font, string fontName, int size)
  {
    Font font1 = new Font(fontName, (float) size);
    FontStyle fontStyle1 = !font.Bold ? FontStyle.Regular : FontStyle.Bold;
    FontStyle fontStyle2 = FontStyle.Regular;
    if (font.Italic)
      fontStyle2 = FontStyle.Italic;
    FontStyle fontStyle3 = FontStyle.Regular;
    if (font.Underline.ToString() == "Single")
      fontStyle3 = FontStyle.Underline;
    return new Font(fontName, (float) size, fontStyle1 | fontStyle2 | fontStyle3);
  }

  private bool FontColorNeedsUpdation(IFont font)
  {
    return font.Color.ToString() == "32767" || font is FontImpl && (font as FontImpl).ColorObject.Value == 8;
  }

  private RectangleF GetBoundsToLayoutShapeTextBody(AutoShapeImpl shapeImpl, RectangleF bounds)
  {
    if (shapeImpl == null)
      return bounds;
    Dictionary<string, float> shapeFormula = new GDIShapePath(bounds, shapeImpl.ShapeExt.ShapeGuide).ParseShapeFormula(shapeImpl.ShapeExt.AutoShapeType);
    switch (shapeImpl.ShapeExt.AutoShapeType)
    {
      case AutoShapeType.Parallelogram:
      case AutoShapeType.Hexagon:
      case AutoShapeType.Cross:
      case AutoShapeType.SmileyFace:
      case AutoShapeType.NoSymbol:
      case AutoShapeType.FlowChartTerminator:
      case AutoShapeType.FlowChartSummingJunction:
      case AutoShapeType.FlowChartOr:
      case AutoShapeType.Star16Point:
      case AutoShapeType.Star24Point:
      case AutoShapeType.Star32Point:
      case AutoShapeType.Wave:
      case AutoShapeType.OvalCallout:
      case AutoShapeType.SnipSameSideCornerRectangle:
      case AutoShapeType.Teardrop:
        return new RectangleF(shapeFormula["il"], shapeFormula["it"], shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.Trapezoid:
        return new RectangleF(shapeFormula["il"], shapeFormula["it"], shapeFormula["ir"], bounds.Height);
      case AutoShapeType.Diamond:
      case AutoShapeType.FlowChartDecision:
      case AutoShapeType.FlowChartCollate:
        return new RectangleF(bounds.Width / 4f, bounds.Height / 4f, shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.RoundedRectangle:
      case AutoShapeType.Octagon:
      case AutoShapeType.Plaque:
      case AutoShapeType.RoundedRectangularCallout:
      case AutoShapeType.SnipDiagonalCornerRectangle:
        return new RectangleF(shapeFormula["il"], shapeFormula["il"], shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.IsoscelesTriangle:
        return new RectangleF(shapeFormula["x1"], bounds.Height / 2f, shapeFormula["x3"], bounds.Height);
      case AutoShapeType.RightTriangle:
        return new RectangleF(bounds.Width / 12f, shapeFormula["it"], shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.Oval:
      case AutoShapeType.Donut:
      case AutoShapeType.BlockArc:
      case AutoShapeType.Arc:
      case AutoShapeType.CircularArrow:
      case AutoShapeType.FlowChartConnector:
      case AutoShapeType.FlowChartSequentialAccessStorage:
      case AutoShapeType.DoubleWave:
      case AutoShapeType.CloudCallout:
      case AutoShapeType.Chord:
      case AutoShapeType.Cloud:
        return new RectangleF(shapeFormula["il"], shapeFormula["it"], shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.RegularPentagon:
        return new RectangleF(shapeFormula["x2"], shapeFormula["it"], shapeFormula["x3"], shapeFormula["y2"]);
      case AutoShapeType.Can:
        return new RectangleF(0.0f, shapeFormula["y2"], bounds.Width, shapeFormula["y3"]);
      case AutoShapeType.Cube:
        return new RectangleF(0.0f, shapeFormula["y1"], shapeFormula["x4"], bounds.Height);
      case AutoShapeType.Bevel:
        return new RectangleF(shapeFormula["x1"], shapeFormula["x1"], shapeFormula["x2"], shapeFormula["y2"]);
      case AutoShapeType.FoldedCorner:
        return new RectangleF(0.0f, 0.0f, bounds.Width, shapeFormula["y2"]);
      case AutoShapeType.Heart:
        return new RectangleF(shapeFormula["il"], bounds.Height / 4f, shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.LightningBolt:
        return new RectangleF(shapeFormula["x4"], shapeFormula["y4"], shapeFormula["x9"], shapeFormula["y10"]);
      case AutoShapeType.Sun:
        return new RectangleF(shapeFormula["x9"], shapeFormula["y9"], shapeFormula["x8"], shapeFormula["y8"]);
      case AutoShapeType.Moon:
        return new RectangleF(shapeFormula["g12w"], shapeFormula["g15h"], shapeFormula["g0w"], shapeFormula["g16h"]);
      case AutoShapeType.DoubleBracket:
      case AutoShapeType.DoubleBrace:
      case AutoShapeType.FlowChartAlternateProcess:
        return new RectangleF(shapeFormula["il"], shapeFormula["il"], shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.LeftBracket:
      case AutoShapeType.LeftBrace:
        return new RectangleF(shapeFormula["il"], shapeFormula["it"], bounds.Width, shapeFormula["ib"]);
      case AutoShapeType.RightBracket:
      case AutoShapeType.RightBrace:
        return new RectangleF(0.0f, shapeFormula["it"], shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.RightArrow:
        return new RectangleF(0.0f, shapeFormula["y1"], shapeFormula["x2"], shapeFormula["y2"]);
      case AutoShapeType.LeftArrow:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y1"], bounds.Width, shapeFormula["y2"]);
      case AutoShapeType.UpArrow:
      case AutoShapeType.MathEqual:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y1"], shapeFormula["x2"], bounds.Height);
      case AutoShapeType.DownArrow:
        return new RectangleF(shapeFormula["x1"], 0.0f, shapeFormula["x2"], shapeFormula["y2"]);
      case AutoShapeType.LeftRightArrow:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y1"], shapeFormula["x4"], shapeFormula["y2"]);
      case AutoShapeType.UpDownArrow:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y1"], shapeFormula["x2"], shapeFormula["y4"]);
      case AutoShapeType.QuadArrow:
        return new RectangleF(shapeFormula["il"], shapeFormula["y3"], shapeFormula["ir"], shapeFormula["y4"]);
      case AutoShapeType.LeftRightUpArrow:
        return new RectangleF(shapeFormula["il"], shapeFormula["y3"], shapeFormula["ir"], shapeFormula["y5"]);
      case AutoShapeType.UTurnArrow:
      case AutoShapeType.FlowChartProcess:
      case AutoShapeType.RectangularCallout:
      case AutoShapeType.StraightConnector:
        return new RectangleF(0.0f, 0.0f, bounds.Width, bounds.Height);
      case AutoShapeType.LeftUpArrow:
        return new RectangleF(shapeFormula["il"], shapeFormula["y3"], shapeFormula["x4"], shapeFormula["y5"]);
      case AutoShapeType.BentUpArrow:
        return new RectangleF(0.0f, shapeFormula["y2"], shapeFormula["x4"], bounds.Height);
      case AutoShapeType.StripedRightArrow:
        return new RectangleF(shapeFormula["x4"], shapeFormula["y1"], shapeFormula["x6"], shapeFormula["y2"]);
      case AutoShapeType.NotchedRightArrow:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y1"], shapeFormula["x3"], shapeFormula["y2"]);
      case AutoShapeType.Pentagon:
      case AutoShapeType.RoundSingleCornerRectangle:
        return new RectangleF(0.0f, 0.0f, shapeFormula["ir"], bounds.Height);
      case AutoShapeType.Chevron:
        return new RectangleF(shapeFormula["il"], 0.0f, shapeFormula["ir"], bounds.Height);
      case AutoShapeType.RightArrowCallout:
        return new RectangleF(0.0f, 0.0f, shapeFormula["x2"], bounds.Height);
      case AutoShapeType.LeftArrowCallout:
        return new RectangleF(shapeFormula["x2"], 0.0f, bounds.Width, bounds.Height);
      case AutoShapeType.UpArrowCallout:
        return new RectangleF(0.0f, shapeFormula["y2"], bounds.Width, bounds.Height);
      case AutoShapeType.DownArrowCallout:
        return new RectangleF(0.0f, 0.0f, bounds.Width, shapeFormula["y2"]);
      case AutoShapeType.LeftRightArrowCallout:
        return new RectangleF(shapeFormula["x2"], 0.0f, shapeFormula["x3"], bounds.Height);
      case AutoShapeType.UpDownArrowCallout:
        return new RectangleF(0.0f, shapeFormula["y2"], bounds.Width, shapeFormula["y3"]);
      case AutoShapeType.QuadArrowCallout:
        return new RectangleF(shapeFormula["x2"], shapeFormula["y2"], shapeFormula["x7"], shapeFormula["y7"]);
      case AutoShapeType.FlowChartData:
        return new RectangleF(bounds.Width / 5f, 0.0f, shapeFormula["x5"], bounds.Height);
      case AutoShapeType.FlowChartPredefinedProcess:
        return new RectangleF(bounds.Width / 8f, 0.0f, shapeFormula["x2"], bounds.Height);
      case AutoShapeType.FlowChartInternalStorage:
        return new RectangleF(bounds.Width / 8f, bounds.Height / 8f, bounds.Width, bounds.Height);
      case AutoShapeType.FlowChartDocument:
        return new RectangleF(0.0f, 0.0f, bounds.Width, shapeFormula["y1"]);
      case AutoShapeType.FlowChartMultiDocument:
        return new RectangleF(0.0f, shapeFormula["y2"], shapeFormula["x5"], shapeFormula["y8"]);
      case AutoShapeType.FlowChartPreparation:
        return new RectangleF(bounds.Width / 5f, 0.0f, shapeFormula["x2"], bounds.Height);
      case AutoShapeType.FlowChartManualInput:
      case AutoShapeType.FlowChartCard:
        return new RectangleF(0.0f, bounds.Height / 5f, bounds.Width, bounds.Height);
      case AutoShapeType.FlowChartManualOperation:
        return new RectangleF(bounds.Width / 5f, 0.0f, shapeFormula["x3"], bounds.Height);
      case AutoShapeType.FlowChartOffPageConnector:
        return new RectangleF(0.0f, 0.0f, bounds.Width, shapeFormula["y1"]);
      case AutoShapeType.FlowChartPunchedTape:
        return new RectangleF(0.0f, bounds.Height / 5f, bounds.Width, shapeFormula["ib"]);
      case AutoShapeType.FlowChartSort:
        return new RectangleF(bounds.Width / 4f, bounds.Height / 4f, shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.FlowChartExtract:
        return new RectangleF(bounds.Width / 4f, bounds.Height / 2f, shapeFormula["x2"], bounds.Height);
      case AutoShapeType.FlowChartMerge:
        return new RectangleF(bounds.Width / 4f, 0.0f, shapeFormula["x2"], bounds.Height / 2f);
      case AutoShapeType.FlowChartStoredData:
        return new RectangleF(bounds.Width / 6f, 0.0f, shapeFormula["x2"], bounds.Height);
      case AutoShapeType.FlowChartDelay:
        return new RectangleF(0.0f, shapeFormula["it"], shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.FlowChartMagneticDisk:
        return new RectangleF(0.0f, bounds.Height / 3f, bounds.Width, shapeFormula["y3"]);
      case AutoShapeType.FlowChartDirectAccessStorage:
        return new RectangleF(bounds.Width / 6f, 0.0f, shapeFormula["x2"], bounds.Height);
      case AutoShapeType.FlowChartDisplay:
        return new RectangleF(bounds.Width / 6f, 0.0f, shapeFormula["x2"], bounds.Height);
      case AutoShapeType.Explosion1:
        return new RectangleF(shapeFormula["x5"], shapeFormula["y3"], shapeFormula["x21"], shapeFormula["y9"]);
      case AutoShapeType.Explosion2:
        return new RectangleF(shapeFormula["x5"], shapeFormula["y3"], shapeFormula["x19"], shapeFormula["y17"]);
      case AutoShapeType.Star4Point:
        return new RectangleF(shapeFormula["sx1"], shapeFormula["sy1"], shapeFormula["sx2"], shapeFormula["sy2"]);
      case AutoShapeType.Star5Point:
        return new RectangleF(shapeFormula["sx1"], shapeFormula["sy1"], shapeFormula["sx4"], shapeFormula["sy3"]);
      case AutoShapeType.Star8Point:
        return new RectangleF(shapeFormula["sx1"], shapeFormula["sy1"], shapeFormula["sx4"], shapeFormula["sy4"]);
      case AutoShapeType.UpRibbon:
        return new RectangleF(shapeFormula["x2"], 0.0f, shapeFormula["x9"], shapeFormula["y2"]);
      case AutoShapeType.DownRibbon:
        return new RectangleF(shapeFormula["x2"], shapeFormula["y2"], shapeFormula["x9"], bounds.Height);
      case AutoShapeType.CurvedUpRibbon:
        return new RectangleF(shapeFormula["x2"], shapeFormula["y6"], shapeFormula["x5"], shapeFormula["rh"]);
      case AutoShapeType.CurvedDownRibbon:
        return new RectangleF(shapeFormula["x2"], shapeFormula["q1"], shapeFormula["x5"], shapeFormula["y6"]);
      case AutoShapeType.VerticalScroll:
        return new RectangleF(shapeFormula["ch"], shapeFormula["ch"], shapeFormula["x6"], shapeFormula["y4"]);
      case AutoShapeType.HorizontalScroll:
        return new RectangleF(shapeFormula["ch"], shapeFormula["ch"], shapeFormula["x4"], shapeFormula["y6"]);
      case AutoShapeType.DiagonalStripe:
        return new RectangleF(0.0f, 0.0f, shapeFormula["x3"], shapeFormula["y3"]);
      case AutoShapeType.Pie:
        return new RectangleF(shapeFormula["il"], shapeFormula["it"], shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.Decagon:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y2"], shapeFormula["x4"], shapeFormula["y3"]);
      case AutoShapeType.Heptagon:
        return new RectangleF(shapeFormula["x2"], shapeFormula["y1"], shapeFormula["x5"], shapeFormula["ib"]);
      case AutoShapeType.Dodecagon:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y1"], shapeFormula["x4"], shapeFormula["y4"]);
      case AutoShapeType.Star6Point:
        return new RectangleF(shapeFormula["sx1"], shapeFormula["sy1"], shapeFormula["sx4"], shapeFormula["sy2"]);
      case AutoShapeType.Star7Point:
        return new RectangleF(shapeFormula["sx2"], shapeFormula["sy1"], shapeFormula["sx5"], shapeFormula["sy3"]);
      case AutoShapeType.Star10Point:
        return new RectangleF(shapeFormula["sx2"], shapeFormula["sy2"], shapeFormula["sx5"], shapeFormula["sy3"]);
      case AutoShapeType.Star12Point:
        return new RectangleF(shapeFormula["sx2"], shapeFormula["sy2"], shapeFormula["sx5"], shapeFormula["sy5"]);
      case AutoShapeType.RoundSameSideCornerRectangle:
        return new RectangleF(shapeFormula["il"], shapeFormula["tdx"], shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.RoundDiagonalCornerRectangle:
        return new RectangleF(shapeFormula["dx"], shapeFormula["dx"], shapeFormula["ir"], shapeFormula["ib"]);
      case AutoShapeType.SnipAndRoundSingleCornerRectangle:
        return new RectangleF(shapeFormula["il"], shapeFormula["il"], shapeFormula["ir"], bounds.Height);
      case AutoShapeType.SnipSingleCornerRectangle:
        return new RectangleF(0.0f, shapeFormula["it"], shapeFormula["ir"], bounds.Height);
      case AutoShapeType.Frame:
        return new RectangleF(shapeFormula["x1"], shapeFormula["x1"], shapeFormula["x4"], shapeFormula["y4"]);
      case AutoShapeType.L_Shape:
        return new RectangleF(0.0f, shapeFormula["it"], shapeFormula["ir"], bounds.Height);
      case AutoShapeType.MathPlus:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y2"], shapeFormula["x4"], shapeFormula["y3"]);
      case AutoShapeType.MathMinus:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y1"], shapeFormula["x2"], shapeFormula["y2"]);
      case AutoShapeType.MathMultiply:
        return new RectangleF(shapeFormula["xA"], shapeFormula["yB"], shapeFormula["xE"], shapeFormula["yH"]);
      case AutoShapeType.MathDivision:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y3"], shapeFormula["x3"], shapeFormula["y4"]);
      case AutoShapeType.MathNotEqual:
        return new RectangleF(shapeFormula["x1"], shapeFormula["y1"], shapeFormula["x8"], shapeFormula["y4"]);
      default:
        return new RectangleF(0.0f, 0.0f, bounds.Width, bounds.Height);
    }
  }

  private void DrawShapeFillAndLine(
    GraphicsPath graphicsPath,
    ShapeImpl shape,
    Pen pen,
    Graphics graphics,
    RectangleF bounds)
  {
    AutoShapeImpl autoShapeImpl = shape as AutoShapeImpl;
    if (graphicsPath.PointCount <= 0)
      return;
    if ((shape.Fill.Visible || shape is AutoShapeImpl && (shape as AutoShapeImpl).ShapeExt.AutoShapeType == AutoShapeType.Unknown) && this.IsShapeNeedToBeFill(shape))
    {
      IFill fill = shape.Fill;
      this.FillBackground(graphics, shape, graphicsPath, fill);
    }
    if (!shape.Line.Visible && (!(shape is AutoShapeImpl) || (shape as AutoShapeImpl).ShapeExt.AutoShapeType != AutoShapeType.Unknown && (!autoShapeImpl.ShapeExt.IsCreated || !autoShapeImpl.ShapeExt.Logger.GetPreservedItem(PreservedFlag.Line) || !autoShapeImpl.Line.Visible) && (!autoShapeImpl.ShapeExt.IsCreated || autoShapeImpl.ShapeExt.Logger.GetPreservedItem(PreservedFlag.Line))))
      return;
    graphics.DrawPath(pen, graphicsPath);
  }

  internal void FillBackground(
    Graphics pdfGraphics,
    ShapeImpl shape,
    GraphicsPath path,
    IFill format)
  {
    if (!format.Visible)
      return;
    switch (format.FillType)
    {
      case ExcelFillType.SolidColor:
        Color color = this.NormalizeColor(shape.GetFillColor());
        pdfGraphics.FillPath((Brush) new SolidBrush(color), path);
        break;
      case ExcelFillType.Pattern:
        HatchBrush hatchBrush = this.GetHatchBrush(format);
        pdfGraphics.FillPath((Brush) hatchBrush, path);
        break;
    }
  }

  internal Image CreateImage(RectangleF bounds, MemoryStream stream)
  {
    using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
    {
      bounds.Width = (float) (int) ((double) bounds.Width / 96.0 * (double) graphics.DpiX);
      bounds.Height = (float) (int) ((double) bounds.Height / 96.0 * (double) graphics.DpiY);
    }
    Image image = (Image) null;
    if ((double) bounds.Width == 0.0)
      bounds.Width = 1f;
    if ((double) bounds.Height == 0.0)
      bounds.Height = 1f;
    using (Bitmap bitmap = new Bitmap((int) bounds.Width, (int) bounds.Height))
    {
      using (Graphics graphics = Graphics.FromImage((Image) bitmap))
      {
        bitmap.SetResolution(graphics.DpiX, graphics.DpiY);
        IntPtr hdc = graphics.GetHdc();
        Rectangle frameRect = new Rectangle(0, 0, (int) bounds.Width, (int) bounds.Height);
        image = (Image) new Metafile((Stream) stream, hdc, frameRect, MetafileFrameUnit.Pixel, EmfType.EmfPlusOnly);
        graphics.ReleaseHdc();
      }
    }
    return image;
  }

  private bool IsLine(ShapeImpl shape)
  {
    if (shape is AutoShapeImpl)
    {
      switch ((shape as AutoShapeImpl).ShapeExt.AutoShapeType)
      {
        case AutoShapeType.FlowChartConnector:
        case AutoShapeType.FlowChartOffPageConnector:
        case AutoShapeType.Line:
        case AutoShapeType.StraightConnector:
        case AutoShapeType.ElbowConnector:
        case AutoShapeType.CurvedConnector:
        case AutoShapeType.BentConnector2:
        case AutoShapeType.BentConnector4:
        case AutoShapeType.CurvedConnector2:
        case AutoShapeType.CurvedConnector4:
        case AutoShapeType.CurvedConnector5:
          return true;
      }
    }
    return false;
  }

  private bool IsShapeNeedToBeFill(ShapeImpl shape)
  {
    if (!(shape is AutoShapeImpl))
      return true;
    switch ((shape as AutoShapeImpl).ShapeExt.AutoShapeType)
    {
      case AutoShapeType.Line:
      case AutoShapeType.StraightConnector:
      case AutoShapeType.ElbowConnector:
      case AutoShapeType.CurvedConnector:
      case AutoShapeType.BentConnector2:
      case AutoShapeType.BentConnector4:
      case AutoShapeType.BentConnector5:
      case AutoShapeType.CurvedConnector2:
      case AutoShapeType.CurvedConnector4:
      case AutoShapeType.CurvedConnector5:
        return false;
      default:
        return true;
    }
  }

  private void Rotate(Graphics graphics, ShapeImpl shapeImpl, RectangleF rectangleF)
  {
    float shapeRotation1 = (float) shapeImpl.ShapeRotation;
    if ((double) shapeRotation1 > 360.0)
      shapeRotation1 %= 360f;
    bool flipV1 = false;
    bool flipH = false;
    switch (shapeImpl)
    {
      case AutoShapeImpl _:
        flipV1 = (shapeImpl as AutoShapeImpl).ShapeExt.FlipVertical;
        flipH = (shapeImpl as AutoShapeImpl).ShapeExt.FlipHorizontal;
        break;
      case TextBoxShapeImpl _:
        flipV1 = (shapeImpl as TextBoxShapeImpl).FlipVertical;
        flipH = (shapeImpl as TextBoxShapeImpl).FlipHorizontal;
        break;
      case BitmapShapeImpl _:
        flipV1 = (shapeImpl as BitmapShapeImpl).FlipVertical;
        flipH = (shapeImpl as BitmapShapeImpl).FlipHorizontal;
        break;
    }
    if (shapeImpl.Group != null)
    {
      float shapeRotation2 = (float) shapeImpl.GetShapeRotation();
      if (this.IsGroupFlipH(shapeImpl.Group) || this.IsGroupFlipV(shapeImpl.Group))
      {
        int flipVcount = this.GetFlipVCount(shapeImpl.Group, flipV1 ? 1 : 0);
        int flipHcount = this.GetFlipHCount(shapeImpl.Group, flipH ? 1 : 0);
        graphics.Transform = this.GetTransformMatrix(rectangleF, shapeRotation2, flipVcount % 2 != 0, flipHcount % 2 != 0);
      }
      else if ((double) shapeRotation2 != 0.0 || flipV1 || flipH)
        graphics.Transform = this.GetTransformMatrix(rectangleF, shapeRotation2, flipV1, flipH);
    }
    else if ((double) shapeRotation1 != 0.0 || flipV1 || flipH)
      graphics.Transform = this.GetTransformMatrix(rectangleF, shapeRotation1, flipV1, flipH);
    if (!(shapeImpl is AutoShapeImpl) || !(shapeImpl as AutoShapeImpl).ShapeExt.PreservedElements.ContainsKey("Scene3d"))
      return;
    bool flip = false;
    bool flipV2 = false;
    float latFromScene3D = this.GetLatFromScene3D(shapeImpl as AutoShapeImpl, out flip);
    if ((double) latFromScene3D == 0.0)
      return;
    graphics.Transform = this.GetTransformMatrix(rectangleF, latFromScene3D, flipV2, flip);
  }

  private float GetLatFromScene3D(AutoShapeImpl shapeImpl, out bool flip)
  {
    float latFromScene3D = 0.0f;
    int num = 0;
    XmlReader reader = UtilityMethods.CreateReader(shapeImpl.ShapeExt.PreservedElements["Scene3d"], "rot");
    if (reader.MoveToAttribute("lat"))
      latFromScene3D = (float) (int) (Convert.ToInt64(reader.Value) / 60000L);
    if (reader.MoveToAttribute("lon"))
      num = (int) (Convert.ToInt64(reader.Value) / 60000L);
    flip = num != 180;
    return latFromScene3D;
  }

  private bool IsGroupFlipV(GroupShapeImpl group)
  {
    for (; group != null; group = group.Group)
    {
      if (group.FlipVertical)
        return true;
    }
    return false;
  }

  private bool IsGroupFlipH(GroupShapeImpl group)
  {
    for (; group != null; group = group.Group)
    {
      if (group.FlipHorizontal)
        return true;
    }
    return false;
  }

  private int GetFlipHCount(GroupShapeImpl group, int count)
  {
    for (; group != null; group = group.Group)
    {
      if (group.FlipHorizontal)
        ++count;
    }
    return count;
  }

  private int GetFlipVCount(GroupShapeImpl group, int count)
  {
    for (; group != null; group = group.Group)
    {
      if (group.FlipVertical)
        ++count;
    }
    return count;
  }

  internal GraphicsPath GetGraphicsPath(
    RectangleF bounds,
    ref Pen pen,
    Graphics graphics,
    AutoShapeImpl shapeImpl)
  {
    this.m_graphics = graphics;
    GDIShapePath gdiShapePath = new GDIShapePath(bounds, shapeImpl.ShapeExt.ShapeGuide);
    GraphicsPath path1 = new GraphicsPath();
    Color empty = Color.Empty;
    switch (shapeImpl.ShapeExt.AutoShapeType)
    {
      case AutoShapeType.Rectangle:
      case AutoShapeType.FlowChartProcess:
        path1.AddRectangle(bounds);
        return path1;
      case AutoShapeType.Parallelogram:
      case AutoShapeType.FlowChartData:
        return gdiShapePath.GetParallelogramPath();
      case AutoShapeType.Trapezoid:
        return gdiShapePath.GetTrapezoidPath();
      case AutoShapeType.Diamond:
      case AutoShapeType.FlowChartDecision:
        PointF[] points1 = new PointF[4]
        {
          new PointF(bounds.X, bounds.Y + bounds.Height / 2f),
          new PointF(bounds.X + bounds.Width / 2f, bounds.Y),
          new PointF(bounds.Right, bounds.Y + bounds.Height / 2f),
          new PointF(bounds.X + bounds.Width / 2f, bounds.Bottom)
        };
        path1.AddLines(points1);
        path1.CloseFigure();
        break;
      case AutoShapeType.RoundedRectangle:
        return gdiShapePath.GetRoundedRectanglePath();
      case AutoShapeType.Octagon:
        return gdiShapePath.GetOctagonPath();
      case AutoShapeType.IsoscelesTriangle:
        return gdiShapePath.GetTrianglePath();
      case AutoShapeType.RightTriangle:
        PointF[] points2 = new PointF[3]
        {
          new PointF(bounds.X, bounds.Bottom),
          new PointF(bounds.X, bounds.Y),
          new PointF(bounds.Right, bounds.Bottom)
        };
        path1.AddLines(points2);
        path1.CloseFigure();
        return path1;
      case AutoShapeType.Oval:
        path1.AddEllipse(bounds);
        return path1;
      case AutoShapeType.Hexagon:
        return gdiShapePath.GetHexagonPath();
      case AutoShapeType.Cross:
        return gdiShapePath.GetCrossPath();
      case AutoShapeType.RegularPentagon:
        return gdiShapePath.GetRegularPentagonPath();
      case AutoShapeType.Can:
        return gdiShapePath.GetCanPath();
      case AutoShapeType.Cube:
        return gdiShapePath.GetCubePath();
      case AutoShapeType.Bevel:
        return gdiShapePath.GetBevelPath();
      case AutoShapeType.FoldedCorner:
        return gdiShapePath.GetFoldedCornerPath();
      case AutoShapeType.SmileyFace:
        for (int index = 0; index < 2; ++index)
        {
          bool isDrawEye = index == 1;
          GraphicsPath[] smileyFacePath = gdiShapePath.GetSmileyFacePath(isDrawEye);
          IFill fill = shapeImpl.Fill;
          Color color = Color.Empty;
          if (fill.FillType == ExcelFillType.SolidColor)
            color = this.NormalizeColor(shapeImpl.GetFillColor());
          if (fill.FillType == ExcelFillType.Gradient)
            color = this.NormalizeColor(fill.ForeColor);
          if (isDrawEye)
            color = this.GetDarkerColor(color, 80f);
          Brush brush = (Brush) new SolidBrush(color);
          if (fill.FillType == ExcelFillType.Pattern)
            brush = (Brush) this.GetHatchBrush(fill);
          foreach (GraphicsPath path2 in smileyFacePath)
          {
            if (color != Color.Empty)
              this.m_graphics.FillPath(brush, path2);
            if (shapeImpl.ShapeExt.IsCreated && !shapeImpl.ShapeExt.Logger.GetPreservedItem(PreservedFlag.Line) || shapeImpl.ShapeExt.IsCreated && shapeImpl.ShapeExt.Logger.GetPreservedItem(PreservedFlag.Line) && shapeImpl.ShapeExt.Line.Visible || !shapeImpl.ShapeExt.IsCreated && shapeImpl.ShapeExt.Line.Visible)
              this.m_graphics.DrawPath(pen, path2);
          }
        }
        break;
      case AutoShapeType.Donut:
        return gdiShapePath.GetDonutPath();
      case AutoShapeType.NoSymbol:
        return gdiShapePath.GetNoSymbolPath();
      case AutoShapeType.BlockArc:
        return gdiShapePath.GetBlockArcPath();
      case AutoShapeType.Heart:
        return gdiShapePath.GetHeartPath();
      case AutoShapeType.LightningBolt:
        return gdiShapePath.GetLightningBoltPath();
      case AutoShapeType.Sun:
        return gdiShapePath.GetSunPath();
      case AutoShapeType.Moon:
        return gdiShapePath.GetMoonPath();
      case AutoShapeType.Arc:
        GraphicsPath[] arcPath = gdiShapePath.GetArcPath();
        IFill fill1 = shapeImpl.Fill;
        Color color1 = ColorExtension.Empty;
        Color color2 = Color.Empty;
        if (fill1.FillType == ExcelFillType.SolidColor)
        {
          color1 = shapeImpl.GetFillColor();
          color2 = this.NormalizeColor(color1);
        }
        Brush brush1 = (Brush) new SolidBrush(color2);
        if (fill1.FillType == ExcelFillType.Pattern)
          brush1 = (Brush) this.GetHatchBrush(fill1);
        if (color2 != Color.Empty && color1 != ColorExtension.Empty)
          this.m_graphics.FillPath(brush1, arcPath[1]);
        if (shapeImpl.ShapeExt.IsCreated && !shapeImpl.ShapeExt.Logger.GetPreservedItem(PreservedFlag.Line) || shapeImpl.ShapeExt.IsCreated && shapeImpl.ShapeExt.Logger.GetPreservedItem(PreservedFlag.Line) && shapeImpl.ShapeExt.Line.Visible || !shapeImpl.ShapeExt.IsCreated && shapeImpl.ShapeExt.Line.Visible)
        {
          this.m_graphics.DrawPath(pen, arcPath[0]);
          break;
        }
        break;
      case AutoShapeType.DoubleBracket:
        return gdiShapePath.GetDoubleBracketPath();
      case AutoShapeType.DoubleBrace:
        return gdiShapePath.GetDoubleBracePath();
      case AutoShapeType.Plaque:
        return gdiShapePath.GetPlaquePath();
      case AutoShapeType.LeftBracket:
        return gdiShapePath.GetLeftBracketPath();
      case AutoShapeType.RightBracket:
        return gdiShapePath.GetRightBracketPath();
      case AutoShapeType.LeftBrace:
        return gdiShapePath.GetLeftBracePath();
      case AutoShapeType.RightBrace:
        return gdiShapePath.GetRightBracePath();
      case AutoShapeType.RightArrow:
        return gdiShapePath.GetRightArrowPath();
      case AutoShapeType.LeftArrow:
        return gdiShapePath.GetLeftArrowPath();
      case AutoShapeType.UpArrow:
        return gdiShapePath.GetUpArrowPath();
      case AutoShapeType.DownArrow:
        return gdiShapePath.GetDownArrowPath();
      case AutoShapeType.LeftRightArrow:
        return gdiShapePath.GetLeftRightArrowPath();
      case AutoShapeType.UpDownArrow:
        return gdiShapePath.GetUpDownArrowPath();
      case AutoShapeType.QuadArrow:
        return gdiShapePath.GetQuadArrowPath();
      case AutoShapeType.LeftRightUpArrow:
        return gdiShapePath.GetLeftRightUpArrowPath();
      case AutoShapeType.BentArrow:
        return gdiShapePath.GetBentArrowPath();
      case AutoShapeType.UTurnArrow:
        return gdiShapePath.GetUTrunArrowPath();
      case AutoShapeType.LeftUpArrow:
        return gdiShapePath.GetLeftUpArrowPath();
      case AutoShapeType.BentUpArrow:
        return gdiShapePath.GetBentUpArrowPath();
      case AutoShapeType.CurvedRightArrow:
        return gdiShapePath.GetCurvedRightArrowPath();
      case AutoShapeType.CurvedLeftArrow:
        return gdiShapePath.GetCurvedLeftArrowPath();
      case AutoShapeType.CurvedUpArrow:
        return gdiShapePath.GetCurvedUpArrowPath();
      case AutoShapeType.CurvedDownArrow:
        return gdiShapePath.GetCurvedDownArrowPath();
      case AutoShapeType.StripedRightArrow:
        return gdiShapePath.GetStripedRightArrowPath();
      case AutoShapeType.NotchedRightArrow:
        return gdiShapePath.GetNotchedRightArrowPath();
      case AutoShapeType.Pentagon:
        return gdiShapePath.GetPentagonPath();
      case AutoShapeType.Chevron:
        return gdiShapePath.GetChevronPath();
      case AutoShapeType.RightArrowCallout:
        return gdiShapePath.GetRightArrowCalloutPath();
      case AutoShapeType.LeftArrowCallout:
        return gdiShapePath.GetLeftArrowCalloutPath();
      case AutoShapeType.UpArrowCallout:
        return gdiShapePath.GetUpArrowCalloutPath();
      case AutoShapeType.DownArrowCallout:
        return gdiShapePath.GetDownArrowCalloutPath();
      case AutoShapeType.LeftRightArrowCallout:
        return gdiShapePath.GetLeftRightArrowCalloutPath();
      case AutoShapeType.QuadArrowCallout:
        return gdiShapePath.GetQuadArrowCalloutPath();
      case AutoShapeType.CircularArrow:
        return gdiShapePath.GetCircularArrowPath();
      case AutoShapeType.FlowChartAlternateProcess:
        return gdiShapePath.GetFlowChartAlternateProcessPath();
      case AutoShapeType.FlowChartPredefinedProcess:
        return gdiShapePath.GetFlowChartPredefinedProcessPath();
      case AutoShapeType.FlowChartInternalStorage:
        return gdiShapePath.GetFlowChartInternalStoragePath();
      case AutoShapeType.FlowChartDocument:
        return gdiShapePath.GetFlowChartDocumentPath();
      case AutoShapeType.FlowChartMultiDocument:
        return gdiShapePath.GetFlowChartMultiDocumentPath();
      case AutoShapeType.FlowChartTerminator:
        return gdiShapePath.GetFlowChartTerminatorPath();
      case AutoShapeType.FlowChartPreparation:
        return gdiShapePath.GetFlowChartPreparationPath();
      case AutoShapeType.FlowChartManualInput:
        return gdiShapePath.GetFlowChartManualInputPath();
      case AutoShapeType.FlowChartManualOperation:
        return gdiShapePath.GetFlowChartManualOperationPath();
      case AutoShapeType.FlowChartConnector:
        return gdiShapePath.GetFlowChartConnectorPath();
      case AutoShapeType.FlowChartOffPageConnector:
        return gdiShapePath.GetFlowChartOffPageConnectorPath();
      case AutoShapeType.FlowChartCard:
        return gdiShapePath.GetFlowChartCardPath();
      case AutoShapeType.FlowChartPunchedTape:
        return gdiShapePath.GetFlowChartPunchedTapePath();
      case AutoShapeType.FlowChartSummingJunction:
        return gdiShapePath.GetFlowChartSummingJunctionPath();
      case AutoShapeType.FlowChartOr:
        return gdiShapePath.GetFlowChartOrPath();
      case AutoShapeType.FlowChartCollate:
        return gdiShapePath.GetFlowChartCollatePath();
      case AutoShapeType.FlowChartSort:
        return gdiShapePath.GetFlowChartSortPath();
      case AutoShapeType.FlowChartExtract:
        return gdiShapePath.GetFlowChartExtractPath();
      case AutoShapeType.FlowChartMerge:
        return gdiShapePath.GetFlowChartMergePath();
      case AutoShapeType.FlowChartStoredData:
        return gdiShapePath.GetFlowChartOnlineStoragePath();
      case AutoShapeType.FlowChartDelay:
        return gdiShapePath.GetFlowChartDelayPath();
      case AutoShapeType.FlowChartSequentialAccessStorage:
        return gdiShapePath.GetFlowChartSequentialAccessStoragePath();
      case AutoShapeType.FlowChartMagneticDisk:
        return gdiShapePath.GetFlowChartMagneticDiskPath();
      case AutoShapeType.FlowChartDirectAccessStorage:
        return gdiShapePath.GetFlowChartDirectAccessStoragePath();
      case AutoShapeType.FlowChartDisplay:
        return gdiShapePath.GetFlowChartDisplayPath();
      case AutoShapeType.Explosion1:
        return gdiShapePath.GetExplosion1();
      case AutoShapeType.Explosion2:
        return gdiShapePath.GetExplosion2();
      case AutoShapeType.Star4Point:
        return gdiShapePath.GetStar4Point();
      case AutoShapeType.Star5Point:
        return gdiShapePath.GetStar5Point();
      case AutoShapeType.Star8Point:
        return gdiShapePath.GetStar8Point();
      case AutoShapeType.Star16Point:
        return gdiShapePath.GetStar16Point();
      case AutoShapeType.Star24Point:
        return gdiShapePath.GetStar24Point();
      case AutoShapeType.Star32Point:
        return gdiShapePath.GetStar32Point();
      case AutoShapeType.UpRibbon:
        return gdiShapePath.GetUpRibbon();
      case AutoShapeType.DownRibbon:
        return gdiShapePath.GetDownRibbon();
      case AutoShapeType.CurvedUpRibbon:
        return gdiShapePath.GetCurvedUpRibbon();
      case AutoShapeType.CurvedDownRibbon:
        return gdiShapePath.GetCurvedDownRibbon();
      case AutoShapeType.VerticalScroll:
        return gdiShapePath.GetVerticalScroll();
      case AutoShapeType.HorizontalScroll:
        GraphicsPath[] horizontalScroll = gdiShapePath.GetHorizontalScroll();
        Color color3 = Color.Empty;
        IFill fill2 = shapeImpl.Fill;
        if (fill2.FillType == ExcelFillType.SolidColor)
          color3 = this.NormalizeColor(shapeImpl.GetFillColor());
        if (fill2.FillType == ExcelFillType.Gradient)
          color3 = this.NormalizeColor(fill2.ForeColor);
        Brush brush2 = (Brush) new SolidBrush(color3);
        if (fill2.FillType == ExcelFillType.Pattern)
          brush2 = (Brush) this.GetHatchBrush(fill2);
        foreach (GraphicsPath path3 in horizontalScroll)
        {
          if (color3 != Color.Empty)
            this.m_graphics.FillPath(brush2, path1);
          if (shapeImpl.ShapeExt.IsCreated && !shapeImpl.ShapeExt.Logger.GetPreservedItem(PreservedFlag.Line) || shapeImpl.ShapeExt.IsCreated && shapeImpl.ShapeExt.Logger.GetPreservedItem(PreservedFlag.Line) && shapeImpl.ShapeExt.Line.Visible || !shapeImpl.ShapeExt.IsCreated && shapeImpl.ShapeExt.Line.Visible)
            this.m_graphics.DrawPath(pen, path3);
        }
        break;
      case AutoShapeType.Wave:
        return gdiShapePath.GetWave();
      case AutoShapeType.DoubleWave:
        return gdiShapePath.GetDoubleWave();
      case AutoShapeType.RectangularCallout:
        return gdiShapePath.GetRectangularCalloutPath();
      case AutoShapeType.RoundedRectangularCallout:
        return gdiShapePath.GetRoundedRectangularCalloutPath();
      case AutoShapeType.OvalCallout:
        return gdiShapePath.GetOvalCalloutPath();
      case AutoShapeType.CloudCallout:
        return gdiShapePath.GetCloudCalloutPath();
      case AutoShapeType.LineCallout1:
      case AutoShapeType.LineCallout1NoBorder:
        return gdiShapePath.GetLineCallout1Path();
      case AutoShapeType.LineCallout2:
      case AutoShapeType.LineCallout2NoBorder:
        return gdiShapePath.GetLineCallout2Path();
      case AutoShapeType.LineCallout3:
      case AutoShapeType.LineCallout3NoBorder:
        return gdiShapePath.GetLineCallout3Path();
      case AutoShapeType.LineCallout1AccentBar:
      case AutoShapeType.LineCallout1BorderAndAccentBar:
        return gdiShapePath.GetLineCallout1AccentBarPath();
      case AutoShapeType.LineCallout2AccentBar:
      case AutoShapeType.LineCallout2BorderAndAccentBar:
        return gdiShapePath.GetLineCallout2AccentBarPath();
      case AutoShapeType.LineCallout3AccentBar:
      case AutoShapeType.LineCallout3BorderAndAccentBar:
        return gdiShapePath.GetLineCallout3AccentBarPath();
      case AutoShapeType.DiagonalStripe:
        return gdiShapePath.GetDiagonalStripePath();
      case AutoShapeType.Pie:
        return gdiShapePath.GetPiePath();
      case AutoShapeType.Decagon:
        return gdiShapePath.GetDecagonPath();
      case AutoShapeType.Heptagon:
        return gdiShapePath.GetHeptagonPath();
      case AutoShapeType.Dodecagon:
        return gdiShapePath.GetDodecagonPath();
      case AutoShapeType.Star6Point:
        return gdiShapePath.GetStar6Point();
      case AutoShapeType.Star7Point:
        return gdiShapePath.GetStar7Point();
      case AutoShapeType.Star10Point:
        return gdiShapePath.GetStar10Point();
      case AutoShapeType.Star12Point:
        return gdiShapePath.GetStar12Point();
      case AutoShapeType.RoundSingleCornerRectangle:
        return gdiShapePath.GetRoundSingleCornerRectanglePath();
      case AutoShapeType.RoundSameSideCornerRectangle:
        return gdiShapePath.GetRoundSameSideCornerRectanglePath();
      case AutoShapeType.RoundDiagonalCornerRectangle:
        return gdiShapePath.GetRoundDiagonalCornerRectanglePath();
      case AutoShapeType.SnipAndRoundSingleCornerRectangle:
        return gdiShapePath.GetSnipAndRoundSingleCornerRectanglePath();
      case AutoShapeType.SnipSingleCornerRectangle:
        return gdiShapePath.GetSnipSingleCornerRectanglePath();
      case AutoShapeType.SnipSameSideCornerRectangle:
        return gdiShapePath.GetSnipSameSideCornerRectanglePath();
      case AutoShapeType.SnipDiagonalCornerRectangle:
        return gdiShapePath.GetSnipDiagonalCornerRectanglePath();
      case AutoShapeType.Frame:
        return gdiShapePath.GetFramePath();
      case AutoShapeType.HalfFrame:
        return gdiShapePath.GetHalfFramePath();
      case AutoShapeType.Teardrop:
        return gdiShapePath.GetTearDropPath();
      case AutoShapeType.Chord:
        return gdiShapePath.GetChordPath();
      case AutoShapeType.L_Shape:
        return gdiShapePath.GetL_ShapePath();
      case AutoShapeType.MathPlus:
        return gdiShapePath.GetMathPlusPath();
      case AutoShapeType.MathMinus:
        return gdiShapePath.GetMathMinusPath();
      case AutoShapeType.MathMultiply:
        return gdiShapePath.GetMathMultiplyPath();
      case AutoShapeType.MathDivision:
        return gdiShapePath.GetMathDivisionPath();
      case AutoShapeType.MathEqual:
        return gdiShapePath.GetMathEqualPath();
      case AutoShapeType.MathNotEqual:
        return gdiShapePath.GetMathNotEqualPath();
      case AutoShapeType.Cloud:
        return gdiShapePath.GetCloudPath();
      case AutoShapeType.Line:
        path1.AddLine(bounds.X, bounds.Y, bounds.Right, bounds.Bottom);
        return path1;
      case AutoShapeType.StraightConnector:
        path1.AddLine(bounds.X, bounds.Y, bounds.Right, bounds.Bottom);
        return path1;
      case AutoShapeType.ElbowConnector:
        return gdiShapePath.GetBentConnectorPath();
      case AutoShapeType.CurvedConnector:
        return gdiShapePath.GetCurvedConnectorPath();
      case AutoShapeType.BentConnector2:
        return gdiShapePath.GetBentConnector2Path();
      case AutoShapeType.BentConnector4:
        return gdiShapePath.GetBentConnector4Path();
      case AutoShapeType.BentConnector5:
        return gdiShapePath.GetBentConnector5Path();
      case AutoShapeType.CurvedConnector2:
        return gdiShapePath.GetCurvedConnector2Path();
      case AutoShapeType.CurvedConnector4:
        return gdiShapePath.GetCurvedConnector4Path();
      case AutoShapeType.CurvedConnector5:
        return gdiShapePath.GetCurvedConnector5Path();
      default:
        if (shapeImpl.ShapeExt.IsCustomGeometry)
          return this.GetCustomGeomentryPath(bounds, path1, (ShapeImpl) shapeImpl);
        break;
    }
    return path1;
  }

  private GraphicsPath GetCustomGeomentryPath(
    RectangleF bounds,
    GraphicsPath path,
    ShapeImpl shapeImpl)
  {
    foreach (Path2D path2D in (shapeImpl as AutoShapeImpl).ShapeExt.Path2DList)
    {
      double width = path2D.Width;
      double height = path2D.Height;
      this.GetGeomentryPath(path, path2D.PathElements, width, height, bounds);
    }
    return path;
  }

  private void GetGeomentryPath(
    GraphicsPath path,
    List<double> pathElements,
    double pathWidth,
    double pathHeight,
    RectangleF bounds)
  {
    PointF pt1 = PointF.Empty;
    double num = 0.0;
    for (int index = 0; index < pathElements.Count; index = index + ((int) num + 1) + 1)
    {
      switch ((ushort) pathElements[index])
      {
        case 1:
          path.CloseFigure();
          pt1 = PointF.Empty;
          num = 0.0;
          break;
        case 2:
          path.CloseFigure();
          num = pathElements[index + 1] * 2.0;
          pt1 = new PointF(this.GetGeomentryPathXValue(pathWidth, pathElements[index + 2], bounds), this.GetGeomentryPathYValue(pathHeight, pathElements[index + 3], bounds));
          break;
        case 3:
          num = pathElements[index + 1] * 2.0;
          PointF pt2 = new PointF(this.GetGeomentryPathXValue(pathWidth, pathElements[index + 2], bounds), this.GetGeomentryPathYValue(pathHeight, pathElements[index + 3], bounds));
          path.AddLine(pt1, pt2);
          pt1 = pt2;
          break;
        case 4:
          num = pathElements[index + 1] * 2.0;
          RectangleF rect = new RectangleF();
          rect.X = bounds.X;
          rect.Y = bounds.Y;
          rect.Width = this.EmuToPoint((int) pathElements[index + 2]) * 2f;
          rect.Height = this.EmuToPoint((int) pathElements[index + 3]) * 2f;
          float startAngle = (float) pathElements[index + 4] / 60000f;
          float sweepAngle = (float) pathElements[index + 5] / 60000f;
          path.AddArc(rect, startAngle, sweepAngle);
          pt1 = path.PathPoints[path.PathPoints.Length - 1];
          break;
        case 5:
          num = pathElements[index + 1] * 2.0;
          PointF[] points1 = new PointF[3]
          {
            pt1,
            new PointF(this.GetGeomentryPathXValue(pathWidth, pathElements[index + 2], bounds), this.GetGeomentryPathYValue(pathHeight, pathElements[index + 3], bounds)),
            new PointF(this.GetGeomentryPathXValue(pathWidth, pathElements[index + 4], bounds), this.GetGeomentryPathYValue(pathHeight, pathElements[index + 5], bounds))
          };
          path.AddBeziers(points1);
          pt1 = points1[2];
          break;
        case 6:
          num = pathElements[index + 1] * 2.0;
          PointF[] points2 = new PointF[4]
          {
            pt1,
            new PointF(this.GetGeomentryPathXValue(pathWidth, pathElements[index + 2], bounds), this.GetGeomentryPathYValue(pathHeight, pathElements[index + 3], bounds)),
            new PointF(this.GetGeomentryPathXValue(pathWidth, pathElements[index + 4], bounds), this.GetGeomentryPathYValue(pathHeight, pathElements[index + 5], bounds)),
            new PointF(this.GetGeomentryPathXValue(pathWidth, pathElements[index + 6], bounds), this.GetGeomentryPathYValue(pathHeight, pathElements[index + 7], bounds))
          };
          path.AddBeziers(points2);
          pt1 = points2[3];
          break;
      }
    }
  }

  private float EmuToPoint(int emu) => (float) Convert.ToDouble((double) emu / 12700.0);

  private float GetGeomentryPathXValue(double pathWidth, double x, RectangleF bounds)
  {
    if (pathWidth == 0.0)
      return bounds.X + this.EmuToPoint((int) x);
    double num = x * 100.0 / pathWidth;
    return (float) ((double) bounds.Width * num / 100.0) + bounds.X;
  }

  private float GetGeomentryPathYValue(double pathHeight, double y, RectangleF bounds)
  {
    if (pathHeight == 0.0)
      return bounds.Y + this.EmuToPoint((int) y);
    double num = y * 100.0 / pathHeight;
    return (float) ((double) bounds.Height * num / 100.0) + bounds.Y;
  }

  internal Matrix GetTransformMatrix(RectangleF bounds, float ang, bool flipV, bool flipH)
  {
    Matrix transformMatrix = new Matrix();
    Matrix matrix1 = new Matrix(1f, 0.0f, 0.0f, -1f, 0.0f, 0.0f);
    Matrix matrix2 = new Matrix(-1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
    PointF point = new PointF(bounds.X + bounds.Width / 2f, bounds.Y + bounds.Height / 2f);
    if (flipV)
    {
      transformMatrix.Multiply(matrix1, MatrixOrder.Append);
      transformMatrix.Translate(0.0f, point.Y * 2f, MatrixOrder.Append);
    }
    if (flipH)
    {
      transformMatrix.Multiply(matrix2, MatrixOrder.Append);
      transformMatrix.Translate(point.X * 2f, 0.0f, MatrixOrder.Append);
    }
    transformMatrix.RotateAt(ang, point, MatrixOrder.Append);
    return transformMatrix;
  }

  private Color GetDarkerColor(Color color, float correctionfactory)
  {
    return Color.FromArgb((int) ((double) color.R / 100.0 * (double) correctionfactory), (int) ((double) color.G / 100.0 * (double) correctionfactory), (int) ((double) color.B / 100.0 * (double) correctionfactory));
  }

  private HatchBrush GetHatchBrush(IFill format)
  {
    return new HatchBrush(this.GetHatchStyle(format.Pattern), this.NormalizeColor(format.ForeColor), this.NormalizeColor(format.BackColor));
  }

  private Pen CreatePen(ShapeImpl shape, double scaledWidth)
  {
    if (shape == null)
      return (Pen) null;
    Color color = shape.Line.ForeColor;
    if (shape.Line.ForeColor.A == (byte) 0)
      color = Color.FromArgb((int) byte.MaxValue, (int) color.R, (int) color.G, (int) color.B);
    return new Pen(color, (float) (shape.GetBorderThickness() * scaledWidth))
    {
      DashStyle = this.GetDashStyle(shape.Line)
    };
  }

  private Color NormalizeColor(Color color)
  {
    return color.A == (byte) 0 ? Color.FromArgb((int) byte.MaxValue, (int) color.R, (int) color.G, (int) color.B) : color;
  }

  private Pen CreatePen(ShapeImpl shape, IShapeLineFormat lineFormat, double scaledWidth)
  {
    Pen pen = new Pen(this.NormalizeColor(shape.GetBorderColor()), (float) (shape.GetBorderThickness() * scaledWidth));
    switch (lineFormat.DashStyle)
    {
      case ExcelShapeDashLineStyle.Solid:
        pen.DashStyle = DashStyle.Solid;
        break;
      case ExcelShapeDashLineStyle.Dotted:
        pen.DashStyle = DashStyle.Dot;
        break;
      case ExcelShapeDashLineStyle.Dotted_Round:
      case ExcelShapeDashLineStyle.Dash_Dot:
        pen.DashStyle = DashStyle.DashDot;
        break;
      case ExcelShapeDashLineStyle.Dashed:
      case ExcelShapeDashLineStyle.Medium_Dashed:
        pen.DashStyle = DashStyle.Dash;
        break;
      case ExcelShapeDashLineStyle.Medium_Dash_Dot:
        pen.DashPattern = new float[2]{ 1f, 0.5f };
        break;
      case ExcelShapeDashLineStyle.Dash_Dot_Dot:
        pen.DashStyle = DashStyle.DashDotDot;
        break;
    }
    switch (lineFormat.Style)
    {
      case ExcelShapeLineStyle.Line_Thin_Thin:
        pen.CompoundArray = new float[4]
        {
          0.0f,
          0.3333333f,
          0.6666667f,
          1f
        };
        break;
      case ExcelShapeLineStyle.Line_Thin_Thick:
        pen.CompoundArray = new float[4]
        {
          0.0f,
          0.16666f,
          0.3f,
          1f
        };
        break;
      case ExcelShapeLineStyle.Line_Thick_Thin:
        pen.CompoundArray = new float[4]
        {
          0.0f,
          0.6f,
          0.73333f,
          1f
        };
        break;
      case ExcelShapeLineStyle.Line_Thick_Between_Thin:
        pen.CompoundArray = new float[6]
        {
          0.0f,
          0.1666667f,
          0.3333333f,
          0.6666667f,
          0.8333333f,
          1f
        };
        break;
    }
    CustomLineCap gdiCustomLineCap1 = this.GetGDICustomLineCap(lineFormat.BeginArrowHeadStyle, lineFormat.BeginArrowheadLength, lineFormat.BeginArrowheadWidth);
    CustomLineCap gdiCustomLineCap2 = this.GetGDICustomLineCap(lineFormat.EndArrowHeadStyle, lineFormat.EndArrowheadLength, lineFormat.EndArrowheadWidth);
    if (gdiCustomLineCap1 != null)
    {
      gdiCustomLineCap1.WidthScale *= (float) scaledWidth;
      pen.CustomStartCap = gdiCustomLineCap1;
    }
    if (gdiCustomLineCap2 != null)
    {
      gdiCustomLineCap2.WidthScale *= (float) scaledWidth;
      pen.CustomEndCap = gdiCustomLineCap2;
    }
    return pen;
  }

  private CustomLineCap GetGDICustomLineCap(
    ExcelShapeArrowStyle arrowheadStyle,
    ExcelShapeArrowLength arrowheadLength,
    ExcelShapeArrowWidth arrowheadWidth)
  {
    float baseInset;
    GraphicsPath lineGapGraphicsPath = this.GetGDICustomLineGapGraphicsPath(arrowheadStyle, arrowheadLength, arrowheadWidth, out baseInset);
    if (lineGapGraphicsPath == null)
      return (CustomLineCap) null;
    CustomLineCap gdiCustomLineCap;
    if (arrowheadStyle == ExcelShapeArrowStyle.LineArrowOpen)
    {
      gdiCustomLineCap = new CustomLineCap((GraphicsPath) null, lineGapGraphicsPath, LineCap.Round, baseInset);
      gdiCustomLineCap.SetStrokeCaps(LineCap.Round, LineCap.Round);
    }
    else
      gdiCustomLineCap = new CustomLineCap(lineGapGraphicsPath, (GraphicsPath) null, LineCap.Triangle, baseInset);
    return gdiCustomLineCap;
  }

  private GraphicsPath GetGDICustomLineGapGraphicsPath(
    ExcelShapeArrowStyle arrowheadStyle,
    ExcelShapeArrowLength arrowheadLength,
    ExcelShapeArrowWidth arrowheadWidth,
    out float baseInset)
  {
    baseInset = 0.0f;
    if (arrowheadStyle == ExcelShapeArrowStyle.LineNoArrow)
      return (GraphicsPath) null;
    GraphicsPath graphicsPath = new GraphicsPath(FillMode.Winding);
    float styleValue;
    if (this.GetGDIArrowheadStyleValue(arrowheadStyle, graphicsPath, out styleValue))
      return (GraphicsPath) null;
    float num1 = (float) (2 + arrowheadLength);
    if (arrowheadLength == ExcelShapeArrowLength.ArrowHeadLong)
      ++num1;
    float num2 = (float) (2 + arrowheadWidth);
    if (arrowheadWidth == ExcelShapeArrowWidth.ArrowHeadWide)
      ++num2;
    baseInset = num1 * styleValue;
    Matrix matrix = new Matrix();
    matrix.Scale(num2 / 10f, num1 / 10f);
    graphicsPath.Transform(matrix);
    return graphicsPath;
  }

  private bool GetGDIArrowheadStyleValue(
    ExcelShapeArrowStyle arrowheadStyle,
    GraphicsPath graphicsPath,
    out float styleValue)
  {
    switch (arrowheadStyle)
    {
      case ExcelShapeArrowStyle.LineArrow:
        graphicsPath.AddLines(this._arrowPoints);
        graphicsPath.CloseFigure();
        styleValue = 1f;
        break;
      case ExcelShapeArrowStyle.LineArrowStealth:
        graphicsPath.AddLines(this._arrowStealthPoints);
        graphicsPath.CloseFigure();
        styleValue = 0.55f;
        break;
      case ExcelShapeArrowStyle.LineArrowDiamond:
        graphicsPath.AddLines(this._arrowDiamondPoints);
        graphicsPath.CloseFigure();
        styleValue = 0.4f;
        break;
      case ExcelShapeArrowStyle.LineArrowOval:
        graphicsPath.AddEllipse(-5f, -5f, 10f, 10f);
        graphicsPath.CloseFigure();
        styleValue = 0.4f;
        break;
      case ExcelShapeArrowStyle.LineArrowOpen:
        graphicsPath.AddLines(this._arrowOpenPoints);
        styleValue = 0.3f;
        break;
      default:
        styleValue = 0.0f;
        return true;
    }
    return false;
  }

  private HatchStyle GetHatchStyle(ExcelGradientPattern pattern)
  {
    HatchStyle hatchStyle = HatchStyle.Horizontal;
    switch (pattern)
    {
      case ExcelGradientPattern.Pat_5_Percent:
        hatchStyle = HatchStyle.Percent05;
        break;
      case ExcelGradientPattern.Pat_10_Percent:
        hatchStyle = HatchStyle.Percent10;
        break;
      case ExcelGradientPattern.Pat_20_Percent:
        hatchStyle = HatchStyle.Percent20;
        break;
      case ExcelGradientPattern.Pat_25_Percent:
        hatchStyle = HatchStyle.Percent25;
        break;
      case ExcelGradientPattern.Pat_30_Percent:
        hatchStyle = HatchStyle.Percent30;
        break;
      case ExcelGradientPattern.Pat_40_Percent:
        hatchStyle = HatchStyle.Percent40;
        break;
      case ExcelGradientPattern.Pat_50_Percent:
        hatchStyle = HatchStyle.Percent50;
        break;
      case ExcelGradientPattern.Pat_60_Percent:
        hatchStyle = HatchStyle.Percent60;
        break;
      case ExcelGradientPattern.Pat_70_Percent:
        hatchStyle = HatchStyle.Percent70;
        break;
      case ExcelGradientPattern.Pat_75_Percent:
        hatchStyle = HatchStyle.Percent75;
        break;
      case ExcelGradientPattern.Pat_80_Percent:
        hatchStyle = HatchStyle.Percent80;
        break;
      case ExcelGradientPattern.Pat_90_Percent:
        hatchStyle = HatchStyle.Percent90;
        break;
      case ExcelGradientPattern.Pat_Dark_Horizontal:
        hatchStyle = HatchStyle.DarkHorizontal;
        break;
      case ExcelGradientPattern.Pat_Dark_Vertical:
        hatchStyle = HatchStyle.DarkVertical;
        break;
      case ExcelGradientPattern.Pat_Dark_Downward_Diagonal:
        hatchStyle = HatchStyle.DarkDownwardDiagonal;
        break;
      case ExcelGradientPattern.Pat_Dark_Upward_Diagonal:
        hatchStyle = HatchStyle.DarkUpwardDiagonal;
        break;
      case ExcelGradientPattern.Pat_Small_Checker_Board:
        hatchStyle = HatchStyle.SmallCheckerBoard;
        break;
      case ExcelGradientPattern.Pat_Trellis:
        hatchStyle = HatchStyle.Trellis;
        break;
      case ExcelGradientPattern.Pat_Light_Horizontal:
        hatchStyle = HatchStyle.LightHorizontal;
        break;
      case ExcelGradientPattern.Pat_Light_Vertical:
        hatchStyle = HatchStyle.LightVertical;
        break;
      case ExcelGradientPattern.Pat_Light_Downward_Diagonal:
        hatchStyle = HatchStyle.LightDownwardDiagonal;
        break;
      case ExcelGradientPattern.Pat_Light_Upward_Diagonal:
        hatchStyle = HatchStyle.LightUpwardDiagonal;
        break;
      case ExcelGradientPattern.Pat_Small_Grid:
        hatchStyle = HatchStyle.SmallGrid;
        break;
      case ExcelGradientPattern.Pat_Dotted_Diamond:
        hatchStyle = HatchStyle.DottedDiamond;
        break;
      case ExcelGradientPattern.Pat_Wide_Downward_Diagonal:
        hatchStyle = HatchStyle.WideDownwardDiagonal;
        break;
      case ExcelGradientPattern.Pat_Wide_Upward_Diagonal:
        hatchStyle = HatchStyle.WideUpwardDiagonal;
        break;
      case ExcelGradientPattern.Pat_Dashed_Upward_Diagonal:
        hatchStyle = HatchStyle.DashedUpwardDiagonal;
        break;
      case ExcelGradientPattern.Pat_Dashed_Downward_Diagonal:
        hatchStyle = HatchStyle.DashedDownwardDiagonal;
        break;
      case ExcelGradientPattern.Pat_Narrow_Vertical:
        hatchStyle = HatchStyle.NarrowVertical;
        break;
      case ExcelGradientPattern.Pat_Narrow_Horizontal:
        hatchStyle = HatchStyle.NarrowHorizontal;
        break;
      case ExcelGradientPattern.Pat_Dashed_Vertical:
        hatchStyle = HatchStyle.DashedVertical;
        break;
      case ExcelGradientPattern.Pat_Dashed_Horizontal:
        hatchStyle = HatchStyle.DashedHorizontal;
        break;
      case ExcelGradientPattern.Pat_Large_Confetti:
        hatchStyle = HatchStyle.LargeConfetti;
        break;
      case ExcelGradientPattern.Pat_Large_Grid:
        hatchStyle = HatchStyle.Cross;
        break;
      case ExcelGradientPattern.Pat_Horizontal_Brick:
        hatchStyle = HatchStyle.HorizontalBrick;
        break;
      case ExcelGradientPattern.Pat_Large_Checker_Board:
        hatchStyle = HatchStyle.LargeCheckerBoard;
        break;
      case ExcelGradientPattern.Pat_Small_Confetti:
        hatchStyle = HatchStyle.SmallConfetti;
        break;
      case ExcelGradientPattern.Pat_Zig_Zag:
        hatchStyle = HatchStyle.ZigZag;
        break;
      case ExcelGradientPattern.Pat_Solid_Diamond:
        hatchStyle = HatchStyle.SolidDiamond;
        break;
      case ExcelGradientPattern.Pat_Diagonal_Brick:
        hatchStyle = HatchStyle.DiagonalBrick;
        break;
      case ExcelGradientPattern.Pat_Outlined_Diamond:
        hatchStyle = HatchStyle.OutlinedDiamond;
        break;
      case ExcelGradientPattern.Pat_Plaid:
        hatchStyle = HatchStyle.Plaid;
        break;
      case ExcelGradientPattern.Pat_Sphere:
        hatchStyle = HatchStyle.Sphere;
        break;
      case ExcelGradientPattern.Pat_Weave:
        hatchStyle = HatchStyle.Weave;
        break;
      case ExcelGradientPattern.Pat_Dotted_Grid:
        hatchStyle = HatchStyle.DottedGrid;
        break;
      case ExcelGradientPattern.Pat_Divot:
        hatchStyle = HatchStyle.Divot;
        break;
      case ExcelGradientPattern.Pat_Shingle:
        hatchStyle = HatchStyle.Shingle;
        break;
      case ExcelGradientPattern.Pat_Wave:
        hatchStyle = HatchStyle.Wave;
        break;
    }
    return hatchStyle;
  }

  private DashStyle GetDashStyle(IShapeLineFormat lineFormat)
  {
    DashStyle dashStyle = DashStyle.Solid;
    switch (lineFormat.DashStyle)
    {
      case ExcelShapeDashLineStyle.Solid:
        dashStyle = DashStyle.Solid;
        break;
      case ExcelShapeDashLineStyle.Dotted:
      case ExcelShapeDashLineStyle.Dotted_Round:
        dashStyle = DashStyle.Dot;
        break;
      case ExcelShapeDashLineStyle.Dashed:
      case ExcelShapeDashLineStyle.Medium_Dashed:
        dashStyle = DashStyle.Dash;
        break;
      case ExcelShapeDashLineStyle.Dash_Dot:
      case ExcelShapeDashLineStyle.Medium_Dash_Dot:
        dashStyle = DashStyle.DashDot;
        break;
      case ExcelShapeDashLineStyle.Dash_Dot_Dot:
        dashStyle = DashStyle.DashDotDot;
        break;
    }
    return dashStyle;
  }
}
