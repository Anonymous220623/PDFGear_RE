// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Rendering.RendererBase
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Office;
using Syncfusion.Presentation.Charts;
using Syncfusion.Presentation.Drawing;
using Syncfusion.Presentation.Layouting;
using Syncfusion.Presentation.RichText;
using Syncfusion.Presentation.SlideImplementation;
using Syncfusion.Presentation.SmartArtImplementation;
using Syncfusion.Presentation.TableImplementation;
using Syncfusion.Presentation.Themes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.Presentation.Rendering;

internal abstract class RendererBase
{
  private const double _alfhaConstantSerializer = 100000.0;
  private Slide _slide;
  private Syncfusion.Presentation.Presentation _presentation;
  private NotesSlide _notesSlide;

  internal RendererBase()
  {
  }

  internal RendererBase(ISlide slide)
  {
    this._slide = slide as Slide;
    this._presentation = this._slide.Presentation;
  }

  internal abstract void ResetClip();

  internal abstract void SetPathClip(Shape shape, RectangleF bounds);

  internal abstract void DrawImage(MemoryStream stream, RectangleF bounds);

  internal abstract void DrawImage(
    System.Drawing.Image image,
    RectangleF bounds,
    ImageAttributes imageAttributes,
    Picture picture);

  internal abstract void DrawImage(System.Drawing.Image image, RectangleF bounds);

  internal abstract void DrawImageBorder(Picture picture, RectangleF bounds);

  internal abstract void ResetTransform();

  internal abstract void Transform(Matrix matrix);

  internal abstract void RotateTransform(float angle);

  internal abstract void TranslateTransform(float dx, float dy);

  internal abstract object GetBrush(Syncfusion.Presentation.RichText.Font font, RectangleF bounds);

  internal abstract object GetSolidBrush(IColor color);

  internal abstract void DrawPathString(
    TextPart textPart,
    Paragraph paragraphImpl,
    string text,
    System.Drawing.Font systemFont,
    object brush,
    RectangleF bounds);

  internal abstract void DrawBullet(
    Paragraph paragraphImpl,
    ListFormat listFormat,
    System.Drawing.Font systemFont,
    RectangleF bounds,
    TextCapsType capsType);

  internal abstract void DrawString(
    string text,
    System.Drawing.Font systemFont,
    object brush,
    RectangleF bounds,
    StringFormat stringFormat,
    float indent,
    float spacing,
    float width);

  internal abstract void DrawSingleShape(Shape shapeImpl, RectangleF bounds);

  internal abstract void FillSlideBackground(Slide slide);

  internal abstract void DrawCell(IShape shape, ICell cell);

  internal abstract SizeF MeasureString(
    string text,
    System.Drawing.Font font,
    StringFormat stringFormat,
    Graphics graphics);

  internal void BaseDrawSlide(NotesSlide notesSlide)
  {
    this._notesSlide = notesSlide;
    this._presentation = notesSlide.ParentSlide.Presentation;
    foreach (ISlideItem shape in (IEnumerable<ISlideItem>) this._notesSlide.Shapes)
      this.DrawShape(shape as Shape);
  }

  internal void BaseDrawSlide(Slide slide)
  {
    this._slide = slide;
    this._presentation = slide.Presentation;
    this.FillSlideBackground(slide);
    if (this._slide.ShowMasterShape)
    {
      string layoutIndex = this._slide.ObtainLayoutIndex();
      if (layoutIndex != null)
      {
        LayoutSlide layoutSlide = this._presentation.GetSlideLayout()[layoutIndex];
        if (layoutSlide.ShowMasterShape)
        {
          foreach (ISlideItem shape in (IEnumerable<ISlideItem>) layoutSlide.MasterSlide.Shapes)
          {
            if (this.CanDrawShape(shape as Shape))
              this.DrawShape(shape as Shape);
          }
        }
        foreach (ISlideItem shape in (IEnumerable<ISlideItem>) layoutSlide.Shapes)
        {
          if (this.CanDrawShape(shape as Shape))
            this.DrawShape(shape as Shape);
        }
      }
    }
    foreach (ISlideItem shape in (IEnumerable<ISlideItem>) this._slide.Shapes)
    {
      Shape shapeImpl = shape as Shape;
      if (shapeImpl.ShapeType == ShapeType.CxnSp && shape is Connector)
      {
        Connector connector = shape as Connector;
        if (connector.IsChanged)
          connector.Update();
      }
      this.DrawShape(shapeImpl);
    }
  }

  internal void DrawShape(Shape shapeImpl)
  {
    float x;
    float y;
    float width;
    float height;
    if (shapeImpl is SmartArt && ((SmartArt) shapeImpl).CreatedSmartArt)
    {
      x = 159.84f;
      y = 56.88f;
      width = 640f;
      height = 426.6666f;
    }
    else
    {
      x = (float) shapeImpl.ShapeFrame.GetDefaultLeft();
      y = (float) shapeImpl.ShapeFrame.GetDefaultTop();
      width = (float) shapeImpl.ShapeFrame.GetDefaultWidth();
      height = (float) shapeImpl.ShapeFrame.GetDefaultHeight();
    }
    RectangleF bounds = new RectangleF(x, y, width, height);
    this.DrawShape(shapeImpl, bounds);
  }

  private void DrawShape(Shape shapeImpl, RectangleF bounds)
  {
    if (shapeImpl.Hidden)
      return;
    if ((double) bounds.Height == 0.0)
      bounds.Height = 0.1f;
    if ((double) bounds.Width == 0.0)
      bounds.Width = 0.1f;
    switch (shapeImpl.ShapeType)
    {
      case ShapeType.Sp:
      case ShapeType.CxnSp:
        this.DrawSingleShape(shapeImpl, bounds);
        break;
      case ShapeType.GrpSp:
        this.DrawGroupShape(shapeImpl);
        break;
      case ShapeType.GraphicFrame:
        switch (shapeImpl)
        {
          case Table table:
            this.DrawTable(table, (IShape) shapeImpl);
            return;
          case OleObject _:
            OleObject oleObject = shapeImpl as OleObject;
            IPicture olePicture = (IPicture) oleObject.OlePicture;
            if (olePicture != null)
            {
              this.DrawImage(olePicture, bounds);
              return;
            }
            if (oleObject.VmlShape == null || oleObject.VmlShape.ImageData == null)
              return;
            System.Drawing.Image image = System.Drawing.Image.FromStream((Stream) new MemoryStream(oleObject.VmlShape.ImageData));
            if (image is Metafile && Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.IsAzureCompatible)
              return;
            this.DrawImage(image, bounds);
            return;
          case SmartArt _:
            SmartArt smartArt = (SmartArt) shapeImpl;
            if (smartArt.CreatedSmartArt)
            {
              float x = 159.84f;
              float y = 56.88f;
              float width = 640f;
              float height = 426.6666f;
              this.DrawSingleShape((Shape) smartArt, new RectangleF(x, y, width, height));
            }
            else
              this.DrawSingleShape((Shape) smartArt, new RectangleF((float) smartArt.ShapeFrame.GetDefaultLeft(), (float) smartArt.ShapeFrame.GetDefaultTop(), (float) smartArt.ShapeFrame.GetDefaultWidth(), (float) smartArt.ShapeFrame.GetDefaultHeight()));
            this.DrawSmartArt(smartArt, bounds);
            return;
          default:
            return;
        }
      case ShapeType.Pic:
        Picture picture = shapeImpl as Picture;
        if (this.IsCropAsShape(picture))
        {
          this.DrawSingleShape(shapeImpl, bounds);
          break;
        }
        this.DrawImage((IPicture) picture, bounds);
        break;
      case ShapeType.Chart:
        if (!(shapeImpl is PresentationChart chart))
          break;
        this.DrawChart(chart, bounds);
        break;
    }
  }

  private void DrawSmartArt(SmartArt smartArt, RectangleF bounds)
  {
    foreach (KeyValuePair<Guid, SmartArtShape> smartArtShape in smartArt.DataModel.SmartArtShapeCollection)
    {
      float x;
      float y;
      if (smartArt.CreatedSmartArt)
      {
        x = (float) (Syncfusion.Presentation.Drawing.Helper.EmuToPoint((int) smartArtShape.Value.ShapeFrame.OffsetX) + 159.83999633789063);
        y = (float) (Syncfusion.Presentation.Drawing.Helper.EmuToPoint((int) smartArtShape.Value.ShapeFrame.OffsetY) + 56.880001068115234);
      }
      else if ((double) bounds.X != Syncfusion.Presentation.Drawing.Helper.EmuToPoint((int) smartArt.ShapeFrame.OffsetX) || (double) bounds.Y != Syncfusion.Presentation.Drawing.Helper.EmuToPoint((int) smartArt.ShapeFrame.OffsetY))
      {
        x = (float) Syncfusion.Presentation.Drawing.Helper.EmuToPoint((int) smartArtShape.Value.ShapeFrame.OffsetX) + bounds.X;
        y = (float) Syncfusion.Presentation.Drawing.Helper.EmuToPoint((int) smartArtShape.Value.ShapeFrame.OffsetY) + bounds.Y;
        float num1 = (float) Syncfusion.Presentation.Drawing.Helper.EmuToPoint((int) smartArt.ShapeFrame.OffsetX) - bounds.X;
        float num2 = (float) Syncfusion.Presentation.Drawing.Helper.EmuToPoint((int) smartArt.ShapeFrame.OffsetY) - bounds.Y;
        foreach (IParagraph paragraph1 in (IEnumerable<IParagraph>) smartArtShape.Value.TextBody.Paragraphs)
        {
          if ((!(paragraph1 is Paragraph paragraph2) || paragraph2.ParagraphInfo != null) && paragraph2 != null)
          {
            foreach (LineInfo lineInfo in paragraph2.ParagraphInfo.LineInfoCollection)
            {
              List<Syncfusion.Presentation.Layouting.TextInfo> textInfoCollection = lineInfo.TextInfoCollection;
              for (int index = 0; index < textInfoCollection.Count; ++index)
              {
                Syncfusion.Presentation.Layouting.TextInfo textInfo = textInfoCollection[index];
                textInfo.X -= num1;
                textInfo.Y -= num2;
              }
            }
          }
        }
      }
      else
      {
        x = (float) (Syncfusion.Presentation.Drawing.Helper.EmuToPoint((int) smartArtShape.Value.ShapeFrame.OffsetX) + Syncfusion.Presentation.Drawing.Helper.EmuToPoint((int) smartArt.ShapeFrame.OffsetX));
        y = (float) (Syncfusion.Presentation.Drawing.Helper.EmuToPoint((int) smartArtShape.Value.ShapeFrame.OffsetY) + Syncfusion.Presentation.Drawing.Helper.EmuToPoint((int) smartArt.ShapeFrame.OffsetY));
      }
      float point1 = (float) Syncfusion.Presentation.Drawing.Helper.EmuToPoint((int) smartArtShape.Value.ShapeFrame.OffsetCX);
      float point2 = (float) Syncfusion.Presentation.Drawing.Helper.EmuToPoint((int) smartArtShape.Value.ShapeFrame.OffsetCY);
      this.DrawSingleShape((Shape) smartArtShape.Value, new RectangleF(x, y, point1, point2));
    }
  }

  private void DrawChart(PresentationChart chart, RectangleF bounds)
  {
    MemoryStream memoryStream = new MemoryStream();
    if (!chart.BaseSlide.Presentation.HasChartToImageConverter)
      return;
    chart.SaveAsImage((Stream) memoryStream);
    if (memoryStream.Length > 0L)
      this.DrawImage(memoryStream, bounds);
    memoryStream.Dispose();
  }

  private void DrawGroupShape(Shape shapeImpl)
  {
    if (!(shapeImpl is GroupShape))
      return;
    foreach (Shape groupedShape in ((GroupShape) shapeImpl).GetGroupedShapes())
    {
      float point1 = (float) Syncfusion.Presentation.Drawing.Helper.EmuToPoint((int) groupedShape.GroupFrame.OffsetX);
      float point2 = (float) Syncfusion.Presentation.Drawing.Helper.EmuToPoint((int) groupedShape.GroupFrame.OffsetY);
      float point3 = (float) Syncfusion.Presentation.Drawing.Helper.EmuToPoint((int) groupedShape.GroupFrame.OffsetCX);
      float point4 = (float) Syncfusion.Presentation.Drawing.Helper.EmuToPoint((int) groupedShape.GroupFrame.OffsetCY);
      int rotation = groupedShape.ShapeFrame.Rotation;
      bool flipVertical = groupedShape.ShapeFrame.FlipVertical;
      bool flipHorizontal = groupedShape.ShapeFrame.FlipHorizontal;
      groupedShape.ShapeFrame.Rotation = groupedShape.GroupFrame.Rotation;
      groupedShape.ShapeFrame.FlipVertical = groupedShape.GroupFrame.FlipVertical;
      groupedShape.ShapeFrame.FlipHorizontal = groupedShape.GroupFrame.FlipHorizontal;
      this.DrawShape(groupedShape, new RectangleF(point1, point2, point3, point4));
      groupedShape.ShapeFrame.Rotation = rotation;
      groupedShape.ShapeFrame.FlipVertical = flipVertical;
      groupedShape.ShapeFrame.FlipHorizontal = flipHorizontal;
    }
  }

  private bool IsCropAsShape(Picture picture)
  {
    return picture.AutoShapeType != AutoShapeType.Rectangle && picture.AutoShapeType != ~AutoShapeType.Unknown && picture.FormatPicture.LeftCrop == 0.0 && picture.FormatPicture.RightCrop == 0.0 && picture.FormatPicture.TopCrop == 0.0 && picture.FormatPicture.BottomCrop == 0.0;
  }

  private void DrawTable(Table table, IShape shape)
  {
    foreach (IRow row in (IEnumerable<IRow>) table.Rows)
    {
      foreach (ICell cell in (IEnumerable<ICell>) row.Cells)
      {
        if (!table.MergedCells.Contains(((Cell) cell).CellIndex))
          this.DrawCell(shape, cell);
      }
    }
  }

  private void DrawImage(IPicture picture, RectangleF bounds)
  {
    if (picture.ImageData == null || !(picture is Picture picture1))
      return;
    this.AddHyperLink(picture as Shape, (picture as Picture).Hyperlink, bounds);
    RectangleF bounds1 = bounds;
    if (System.Drawing.Image.FromStream((Stream) new MemoryStream(picture1.ImageData)) is Metafile && Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.IsAzureCompatible)
      return;
    MemoryStream memoryStream1 = new MemoryStream(picture1.ImageData);
    MemoryStream memoryStream2 = new MemoryStream(picture1.ImageData);
    System.Drawing.Image image1 = (System.Drawing.Image) null;
    FormatPicture formatPicture = picture1.FormatPicture;
    SizeF size = new SizeF(bounds.Width, bounds.Height);
    System.Drawing.Image image2 = (System.Drawing.Image) new Bitmap((Stream) memoryStream2);
    RectangleF rectangleF = this.CropPosition(formatPicture, image2);
    if ((double) rectangleF.Height <= 0.0 || (double) rectangleF.Width <= 0.0)
      return;
    if (formatPicture.LeftCrop != 0.0 || formatPicture.TopCrop != 0.0 || formatPicture.RightCrop != 0.0 || formatPicture.BottomCrop != 0.0)
    {
      System.Drawing.Image image3 = (System.Drawing.Image) new Bitmap((Stream) memoryStream1);
      RectangleF rect = this.CropPosition(formatPicture, image3);
      image1 = (System.Drawing.Image) (image3 as Bitmap).Clone(rect, PixelFormat.Format32bppArgb);
      if (formatPicture.LeftCrop < 0.0 || formatPicture.TopCrop < 0.0 || formatPicture.RightCrop < 0.0 || formatPicture.BottomCrop < 0.0)
        this.CropImageBounds(picture1, ref bounds, ref size);
    }
    bounds.Width = size.Width;
    bounds.Height = size.Height;
    this.ResetTransform();
    this.Rotate(picture1.ShapeFrame, bounds);
    if (picture1.GrayScale || picture1.Threshold > 0)
    {
      if (image1 == null)
        image1 = (System.Drawing.Image) new Bitmap((Stream) memoryStream1);
      image1 = (System.Drawing.Image) this.ApplyRecolor(picture1, image1);
    }
    if (picture1.DuoTone.Count == 2)
    {
      if (image1 == null)
        image1 = (System.Drawing.Image) new Bitmap((Stream) memoryStream1);
      image1 = this.ApplyDuoTone(image1, picture1.DuoTone);
    }
    else if (image1 == null)
      image1 = System.Drawing.Image.FromStream((Stream) memoryStream1);
    ImageAttributes imageAttributes1 = (ImageAttributes) null;
    if (picture1.ColorChange.Count > 0)
    {
      if (picture1.ColorChange[1].ColorTransFormCollection.GetColorModeValue(Syncfusion.Presentation.ColorMode.Alpha) == 0)
      {
        Bitmap bitmap = new Bitmap(image1);
        Color pixel = bitmap.GetPixel(1, 1);
        bitmap.MakeTransparent(pixel);
        MemoryStream memoryStream3 = new MemoryStream();
        bitmap.Save((Stream) memoryStream3, System.Drawing.Imaging.ImageFormat.Png);
        image1 = System.Drawing.Image.FromStream((Stream) memoryStream3);
      }
      else
      {
        ImageAttributes imageAttributes2 = new ImageAttributes();
        imageAttributes1 = this.ColorChange(picture1, imageAttributes2);
      }
    }
    double transparency = (double) picture1.Amount / 100000.0;
    if (transparency < 0.0)
      transparency = 0.0;
    else if (transparency > 1.0)
      transparency = 1.0;
    if (transparency != 1.0)
    {
      if (imageAttributes1 == null)
        imageAttributes1 = new ImageAttributes();
      this.ApplyImageTransparency(imageAttributes1, (float) transparency);
    }
    this.SetPathClip(picture as Shape, bounds1);
    this.DrawImage(image1, bounds, imageAttributes1, picture1);
    this.ResetClip();
    this.DrawImageBorder(picture1, bounds1);
    this.ResetTransform();
    imageAttributes1?.Dispose();
    memoryStream1.Dispose();
    image1.Dispose();
    image2.Dispose();
    memoryStream2.Dispose();
  }

  private Bitmap ApplyRecolor(Picture picture, System.Drawing.Image image)
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

  private Bitmap CreateNonIndexedImage(System.Drawing.Image sourceImage)
  {
    Bitmap nonIndexedImage = new Bitmap(sourceImage.Width, sourceImage.Height, PixelFormat.Format32bppArgb);
    using (Graphics graphics = Graphics.FromImage((System.Drawing.Image) nonIndexedImage))
      graphics.DrawImage(sourceImage, 0, 0);
    return nonIndexedImage;
  }

  internal System.Drawing.Image ApplyDuoTone(System.Drawing.Image image, List<ColorObject> duotone)
  {
    if (duotone.Count != 2)
      return image;
    ColorObject empty1 = ColorObject.Empty as ColorObject;
    ColorObject empty2 = ColorObject.Empty as ColorObject;
    Bitmap bitmap1 = image as Bitmap;
    Bitmap bitmap2 = new Bitmap(bitmap1.Width, bitmap1.Height, bitmap1.PixelFormat);
    ColorObject inputColor1;
    if (duotone[1].ThemeColorValue != null)
    {
      inputColor1 = this._slide.BaseTheme.GetThemeColor(duotone[1].ThemeColorValue) as ColorObject;
    }
    else
    {
      duotone[1].UpdateColorObject((object) this._presentation);
      inputColor1 = duotone[1];
    }
    if (duotone[1].ColorTransFormCollection.Count > 0)
      inputColor1 = this.ApplyColorTransform((List<ColorTransForm>) duotone[1].ColorTransFormCollection, inputColor1);
    ColorObject inputColor2;
    if (duotone[0].ThemeColorValue != null)
    {
      inputColor2 = this._slide.BaseTheme.GetThemeColor(duotone[0].ThemeColorValue) as ColorObject;
    }
    else
    {
      duotone[0].UpdateColorObject((object) this._presentation);
      inputColor2 = duotone[0];
    }
    if (duotone[0].ColorTransFormCollection.Count > 0)
      inputColor2 = this.ApplyColorTransform((List<ColorTransForm>) duotone[0].ColorTransFormCollection, inputColor2);
    Color color1 = Color.FromArgb((int) byte.MaxValue - (int) inputColor2.A, inputColor2.SystemColor);
    Color color2 = Color.FromArgb((int) byte.MaxValue - (int) inputColor1.A, inputColor1.SystemColor);
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
      Color empty3 = Color.Empty;
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
    return (System.Drawing.Image) bitmap2;
  }

  private ColorObject ApplyColorTransform(
    List<ColorTransForm> colorTransforms,
    ColorObject inputColor)
  {
    int num1 = inputColor.ToArgb();
    for (int index = 0; index < colorTransforms.Count; ++index)
    {
      ColorTransForm colorTransform = colorTransforms[index];
      double num2 = (double) colorTransforms[index].HexValue * 1E-05;
      switch (colorTransform.ColorMode)
      {
        case Syncfusion.Presentation.ColorMode.Tint:
          num1 = ThemeUtilities.ApplyTint(num1, num2);
          break;
        case Syncfusion.Presentation.ColorMode.Shade:
          num1 = ThemeUtilities.ApplyShade(num1, num2);
          break;
        case Syncfusion.Presentation.ColorMode.Alpha:
          num1 = ThemeUtilities.ApplyAlpha(num1, num2);
          break;
        case Syncfusion.Presentation.ColorMode.HueMod:
          num1 = ThemeUtilities.ApplyHueMod(num1, num2);
          break;
        case Syncfusion.Presentation.ColorMode.SatMod:
          num1 = ThemeUtilities.ApplySatMod(num1, num2);
          break;
        case Syncfusion.Presentation.ColorMode.LumMod:
          num1 = ThemeUtilities.ApplyLumMod(num1, num2);
          break;
      }
    }
    return ColorObject.FromArgb(num1) as ColorObject;
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

  private ImageAttributes ColorChange(Picture pictureImpl, ImageAttributes imageAttributes)
  {
    List<ColorObject> colorChange = pictureImpl.ColorChange;
    ColorObject colorObject1 = colorChange[0];
    ColorObject colorObject2 = colorChange[1];
    ColorMap[] map = new ColorMap[1]{ new ColorMap() };
    map[0].OldColor = Color.FromArgb((int) colorObject1.R, (int) colorObject1.G, (int) colorObject1.B);
    map[0].NewColor = !pictureImpl.IsUseAlpha ? Color.FromArgb((int) colorObject2.R, (int) colorObject2.G, (int) colorObject2.B) : Color.FromArgb(colorObject2.ColorTransFormCollection.GetColorModeValue(Syncfusion.Presentation.ColorMode.Alpha), (int) colorObject2.R, (int) colorObject2.G, (int) colorObject2.B);
    imageAttributes.SetRemapTable(map);
    return imageAttributes;
  }

  private Matrix GetTransformMatrix(RectangleF bounds, float ang, bool flipV, bool flipH)
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

  private RectangleF CropPosition(FormatPicture formatPicture, System.Drawing.Image image)
  {
    RectangleF rectangleF = new RectangleF();
    float num1 = 0.0f;
    float num2 = 0.0f;
    if (formatPicture.LeftCrop > 0.0)
      rectangleF.X = (float) formatPicture.LeftCrop * (float) image.Width / 100f;
    if (formatPicture.TopCrop > 0.0)
      rectangleF.Y = (float) formatPicture.TopCrop * (float) image.Height / 100f;
    if (formatPicture.RightCrop > 0.0)
      num1 = (float) formatPicture.RightCrop * (float) image.Width / 100f;
    if (formatPicture.BottomCrop > 0.0)
      num2 = (float) formatPicture.BottomCrop * (float) image.Height / 100f;
    rectangleF.Width = (float) image.Width - (rectangleF.X + num1);
    rectangleF.Height = (float) image.Height - (rectangleF.Y + num2);
    if ((double) rectangleF.Height < 0.0)
      rectangleF.Height = -rectangleF.Height;
    return rectangleF;
  }

  internal RectangleF CropPosition(PicFormatOption formatPicture, RectangleF bounds)
  {
    bounds.X += (float) formatPicture.Left * (bounds.Width / 100f);
    bounds.Y += (float) formatPicture.Top * (bounds.Height / 100f);
    bounds.Width -= (float) (formatPicture.Left + formatPicture.Right) * (bounds.Width / 100f);
    bounds.Height -= (float) (formatPicture.Top + formatPicture.Bottom) * (bounds.Height / 100f);
    return bounds;
  }

  private void CropImageBounds(Picture picture, ref RectangleF bounds, ref SizeF size)
  {
    FormatPicture formatPicture = picture.FormatPicture;
    float num1 = size.Height - (float) ((double) size.Height * (formatPicture.TopCrop + formatPicture.BottomCrop) / 100.0);
    float num2 = size.Width - (float) ((double) size.Width * (formatPicture.LeftCrop + formatPicture.RightCrop) / 100.0);
    float num3 = size.Height * 100f / num1;
    float num4 = (float) ((double) (size.Width * 100f / num2) * (double) size.Width / 100.0);
    float num5 = (float) ((double) num3 * (double) size.Height / 100.0);
    if (formatPicture.LeftCrop < 0.0 && formatPicture.RightCrop < 0.0)
    {
      bounds.X += (float) (formatPicture.LeftCrop / (formatPicture.LeftCrop + formatPicture.RightCrop) * ((double) size.Width - (double) num4));
      size.Width = num4;
    }
    else if (formatPicture.LeftCrop < 0.0)
    {
      bounds.X -= (float) (formatPicture.LeftCrop * (double) size.Width / 100.0);
      size.Width += (float) (formatPicture.LeftCrop * (double) size.Width / 100.0);
    }
    else if (formatPicture.RightCrop < 0.0)
      size.Width += (float) (formatPicture.RightCrop * (double) size.Width / 100.0);
    if (formatPicture.TopCrop < 0.0 && formatPicture.BottomCrop < 0.0)
    {
      bounds.Y += (float) (formatPicture.TopCrop / (formatPicture.TopCrop + formatPicture.BottomCrop) * ((double) size.Height - (double) num5));
      size.Height = num5;
    }
    else if (formatPicture.TopCrop < 0.0)
    {
      bounds.Y -= (float) (formatPicture.TopCrop * (double) size.Height / 100.0);
      size.Height += (float) (formatPicture.TopCrop * (double) size.Height / 100.0);
    }
    else
    {
      if (formatPicture.BottomCrop >= 0.0)
        return;
      size.Height += (float) (formatPicture.BottomCrop * (double) size.Height / 100.0);
    }
  }

  internal bool IsShapeNeedToBeRender(Shape shape)
  {
    return shape is SmartArtShape || shape.GetAutoShapeType() != AutoShapeType.PieWedge && shape.GetAutoShapeType() != AutoShapeType.Funnel && shape.GetAutoShapeType() != AutoShapeType.LeftRightRibbon && shape.GetAutoShapeType() != AutoShapeType.Gear6 && shape.GetAutoShapeType() != AutoShapeType.Gear9 && shape.GetAutoShapeType() != AutoShapeType.LeftCircularArrow && shape.GetAutoShapeType() != AutoShapeType.SwooshArrow;
  }

  internal void AddHyperLink(Shape shape, IHyperLink hyperLink, RectangleF bounds)
  {
    if (hyperLink == null)
      return;
    BaseSlide baseSlide = shape.BaseSlide;
    string key = (string) null;
    switch (hyperLink.Action)
    {
      case HyperLinkType.Hyperlink:
      case HyperLinkType.OpenFile:
        key = hyperLink.Url;
        break;
      case HyperLinkType.JumpFirstSlide:
        if (baseSlide is Slide)
        {
          int num = baseSlide.Presentation.Slides.IndexOf((ISlide) (baseSlide as Slide));
          if (num != 0)
          {
            key = $"Link {(object) num} to 0";
            break;
          }
          break;
        }
        break;
      case HyperLinkType.JumpPreviousSlide:
      case HyperLinkType.JumpLastViewedSlide:
        if (baseSlide is Slide)
        {
          int num = baseSlide.Presentation.Slides.IndexOf((ISlide) (baseSlide as Slide));
          if (num != 0)
          {
            key = $"Link {(object) num} to {(object) (num - 1)}";
            break;
          }
          break;
        }
        break;
      case HyperLinkType.JumpNextSlide:
        if (baseSlide is Slide)
        {
          int num1 = baseSlide.Presentation.Slides.IndexOf((ISlide) (baseSlide as Slide));
          int num2 = baseSlide.Presentation.Slides.Count - 1;
          if (num1 < num2)
          {
            key = $"Link {(object) num1} to {(object) (num1 + 1)}";
            break;
          }
          break;
        }
        break;
      case HyperLinkType.JumpLastSlide:
      case HyperLinkType.JumpEndShow:
        if (baseSlide is Slide)
        {
          int num3 = baseSlide.Presentation.Slides.IndexOf((ISlide) (baseSlide as Slide));
          int num4 = baseSlide.Presentation.Slides.Count - 1;
          if (num3 < num4)
          {
            key = $"Link {(object) num3} to {(object) num4}";
            break;
          }
          break;
        }
        break;
      case HyperLinkType.JumpSpecificSlide:
        if (baseSlide is Slide)
        {
          int num5 = baseSlide.Presentation.Slides.IndexOf((ISlide) (baseSlide as Slide));
          int num6 = baseSlide.Presentation.Slides.IndexOf(hyperLink.TargetSlide);
          if (num5 != num6 && hyperLink.TargetSlide != null)
          {
            key = $"Link {(object) num5} to {(object) num6}";
            break;
          }
          break;
        }
        break;
    }
    if (key == null)
      return;
    Dictionary<string, RectangleF> dictionary = new Dictionary<string, RectangleF>(1);
    dictionary.Add(key, bounds);
    if (hyperLink.Action == HyperLinkType.Hyperlink || hyperLink.Action == HyperLinkType.OpenFile)
      this._slide.UriHyperlinks.Add(dictionary);
    else
      baseSlide.Presentation.DocumentLinkHyperlinks.Add(dictionary);
  }

  internal void ApplyTextBodyRotation(RectangleF rect, Shape shape)
  {
    float ang1 = (float) (shape.ShapeFrame.GetDefaultRotation() / 60000);
    if (((Syncfusion.Presentation.RichText.TextBody) shape.TextBody).Rotation != 0)
    {
      float ang2 = ang1 + (float) ((Syncfusion.Presentation.RichText.TextBody) shape.TextBody).Rotation;
      if ((double) ang2 > 360.0)
        ang2 %= 360f;
      this.Transform(this.GetTransformMatrix(rect, ang2, shape.ShapeFrame.FlipVertical, shape.ShapeFrame.FlipVertical));
    }
    else if (shape.ShapeFrame.FlipVertical)
    {
      this.Transform(this.GetTransformMatrix(rect, ang1, true, true));
    }
    else
    {
      if (!shape.ShapeFrame.FlipHorizontal)
        return;
      this.Transform(this.GetTransformMatrix(rect, ang1, false, false));
    }
  }

  internal void RotateText(RectangleF bounds, TextDirection textDirectionType)
  {
    switch (textDirectionType)
    {
      case TextDirection.Vertical:
      case TextDirection.EastAsianVertical:
        this.TranslateTransform(bounds.X + bounds.Y + bounds.Height, bounds.Y - bounds.X);
        this.RotateTransform(90f);
        break;
      case TextDirection.Vertical270:
        this.TranslateTransform(bounds.X - bounds.Y, bounds.X + bounds.Y + bounds.Width);
        this.RotateTransform(270f);
        break;
    }
  }

  internal void DrawParagraphs(IEnumerable<IParagraph> paragraphCollection)
  {
    foreach (IParagraph paragraph in paragraphCollection)
    {
      if ((!(paragraph is Paragraph paragraphImpl) || paragraphImpl.ParagraphInfo != null) && paragraphImpl != null)
      {
        foreach (LineInfo lineInfo in paragraphImpl.ParagraphInfo.LineInfoCollection)
        {
          List<Syncfusion.Presentation.Layouting.TextInfo> textInfoCollection = lineInfo.TextInfoCollection;
          float fontSpace = this.GetFontSpace(textInfoCollection, paragraphImpl);
          for (int index = 0; index < textInfoCollection.Count; ++index)
          {
            Syncfusion.Presentation.Layouting.TextInfo textInfo = textInfoCollection[index];
            TextPart textPart = textInfo.TextPart as TextPart;
            textInfo.Y += fontSpace;
            if (textPart != null)
              this.DrawTextPart(paragraphImpl, textInfoCollection, index, textInfo, textPart);
            else
              this.DrawBullet(paragraphImpl, textInfo);
          }
        }
      }
    }
  }

  private float GetFontSpace(List<Syncfusion.Presentation.Layouting.TextInfo> textInfoCollection, Paragraph paragraphImpl)
  {
    float num1 = 0.0f;
    float fontSpace = 0.0f;
    foreach (Syncfusion.Presentation.Layouting.TextInfo textInfo in textInfoCollection)
    {
      RectangleF bounds = textInfo.Bounds;
      System.Drawing.Font font = textInfo.TextPart != null ? paragraphImpl.GetUpdatedFont(textInfo.TextPart.Font, (textInfo.TextPart as TextPart).ScriptType) : paragraphImpl.Presentation.FontSettings.GetFont(((ListFormat) paragraphImpl.ListFormat).GetDefaultBulletFontName(), ((ListFormat) paragraphImpl.ListFormat).GetDefaultBulletSize(), FontStyle.Regular);
      float num2 = paragraphImpl.GetDescentSpace(font) + bounds.Height * 0.1f;
      if ((double) num2 > (double) num1)
      {
        num1 = num2;
        fontSpace = bounds.Bottom - (num1 + paragraphImpl.GetAscentSpace(font)) - bounds.Y;
      }
    }
    return fontSpace;
  }

  private void DrawTextPart(
    Paragraph paragraphImpl,
    List<Syncfusion.Presentation.Layouting.TextInfo> textInfoCollection,
    int index,
    Syncfusion.Presentation.Layouting.TextInfo textInfo,
    TextPart textPart)
  {
    Syncfusion.Presentation.RichText.Font font = textPart.Font as Syncfusion.Presentation.RichText.Font;
    System.Drawing.Font systemFont = paragraphImpl.GetUpdatedFont((IFont) font, textPart.ScriptType);
    if (font != null && Syncfusion.Presentation.Drawing.Helper.IsSymbol(textInfo.Text) && font.GetSymbolFontName() != null)
      systemFont = paragraphImpl.Presentation.FontSettings.GetFont(font.GetSymbolFontName(), systemFont.Size, systemFont.Style);
    ListFormat listFormat = paragraphImpl.ListFormat as ListFormat;
    string text = textInfo.Text;
    if (textPart.Type == "slidenum" && this._slide != null)
      text = this._slide.SlideNumber.ToString((IFormatProvider) CultureInfo.InvariantCulture);
    if (index + 1 == textInfoCollection.Count && text.EndsWith(" "))
    {
      if (textInfoCollection.Count > 1)
      {
        if ((double) textInfoCollection[index].X < (double) textInfoCollection[index - 1].X)
        {
          if (!paragraphImpl.IsRTLText(textPart.Text) || paragraphImpl.IsRTLText(textPart.Text) && listFormat.GetDefaultListType() == ListType.Bulleted || paragraphImpl.IsRTLText(textPart.Text) && listFormat.GetDefaultListType() == ListType.Picture || paragraphImpl.IsRTLText(textPart.Text) && listFormat.GetDefaultListType() == ListType.Numbered)
            text = text.TrimEnd(' ');
        }
        else
          text = text.TrimEnd(' ');
      }
      else
        text = text.TrimEnd(' ');
    }
    if (font != null && font.GetDefaultBold())
    {
      switch (systemFont.Name)
      {
        case "Lucida Console":
        case "Arial Unicode MS":
          systemFont = paragraphImpl.Presentation.FontSettings.GetFont(systemFont.Name, systemFont.Size, systemFont.Style | FontStyle.Bold);
          break;
      }
    }
    IColor color = (IColor) null;
    if (textPart.Hyperlink is Hyperlink hyperlink)
    {
      object brush = (object) null;
      if (hyperlink.HyperLinkColorType == HyperLinkColor.Tx)
        brush = this.GetBrush(font, textInfo.Bounds);
      this.DrawHyperlink(textPart, color, text, font, paragraphImpl, textInfo, systemFont, brush);
    }
    else
    {
      object brush = this.GetBrush(font, textInfo.Bounds);
      if (font != null && (double) font.GetDefaultCharacterSpacing() == 0.0)
      {
        TextCapsType capsType = font.GetDefaultCapsType();
        switch (capsType)
        {
          case TextCapsType.None:
          case TextCapsType.All:
            this.DrawString(textPart, paragraphImpl, text, systemFont, brush, textInfo.Bounds, capsType);
            break;
          default:
            if (paragraphImpl.IsRTLText(text))
            {
              capsType = TextCapsType.None;
              goto case TextCapsType.None;
            }
            goto case TextCapsType.None;
        }
      }
      else
      {
        if (font.GetDefaultCapsType() == TextCapsType.All)
          text = text.ToUpper();
        this.DrawStringBasedOnCharacterSpacing(paragraphImpl, systemFont, brush, textInfo.Bounds, text, font);
      }
    }
  }

  private void DrawHyperlink(
    TextPart textPart,
    IColor color,
    string text,
    Syncfusion.Presentation.RichText.Font font,
    Paragraph paragraphImpl,
    Syncfusion.Presentation.Layouting.TextInfo textInfo,
    System.Drawing.Font systemFont,
    object brush)
  {
    this.AddHyperLink(paragraphImpl.BaseShape, textPart.Hyperlink, textInfo.Bounds);
    color = ((Hyperlink) textPart.Hyperlink).Color;
    if (color == null)
      return;
    if (brush == null)
      brush = this.GetSolidBrush(color);
    if (font != null && (double) font.GetDefaultCharacterSpacing() == 0.0)
      this.DrawString(textPart, paragraphImpl, text, systemFont, brush, textInfo.Bounds, font.GetDefaultCapsType());
    else
      this.DrawStringBasedOnCharacterSpacing(paragraphImpl, systemFont, brush, textInfo.Bounds, text, font);
  }

  private void DrawStringBasedOnCharacterSpacing(
    Paragraph paragraphImpl,
    System.Drawing.Font systemFont,
    object textBrush,
    RectangleF bounds,
    string text,
    Syncfusion.Presentation.RichText.Font font)
  {
    StringFormat stringFormat = paragraphImpl.StringFormt;
    if (Syncfusion.Presentation.Drawing.Helper.HasFlag((Enum) stringFormat.FormatFlags, (Enum) StringFormatFlags.DirectionRightToLeft) && !paragraphImpl.IsRTLText(text))
    {
      stringFormat = (StringFormat) stringFormat.Clone();
      stringFormat.FormatFlags &= ~StringFormatFlags.DirectionRightToLeft;
    }
    else if (!Syncfusion.Presentation.Drawing.Helper.HasFlag((Enum) stringFormat.FormatFlags, (Enum) StringFormatFlags.DirectionRightToLeft) && paragraphImpl.IsRTLText(text))
    {
      stringFormat = (StringFormat) stringFormat.Clone();
      stringFormat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
      char[] charArray = text.ToCharArray();
      Array.Reverse((Array) charArray);
      text = new string(charArray);
    }
    else if (paragraphImpl.IsRTLText(text))
    {
      char[] charArray = text.ToCharArray();
      Array.Reverse((Array) charArray);
      text = new string(charArray);
    }
    float num = 0.0f;
    foreach (char ch in text)
      num += paragraphImpl.MeasureString(ch.ToString(), systemFont, font.GetDefaultCapsType()).Width;
    float spacing = (bounds.Width - num) / (float) text.Length;
    float indent = 0.0f;
    foreach (char ch in text)
    {
      float width = paragraphImpl.MeasureString(ch.ToString(), systemFont, font.GetDefaultCapsType()).Width;
      this.DrawString(ch.ToString(), systemFont, textBrush, bounds, stringFormat, indent, spacing, width);
      indent += width + spacing;
    }
  }

  private void DrawBullet(Paragraph paragraphImpl, Syncfusion.Presentation.Layouting.TextInfo textInfo)
  {
    IListFormat listFormat = paragraphImpl.ListFormat;
    if (((ListFormat) listFormat).GetDefaultListType() == ListType.Picture)
    {
      Syncfusion.Drawing.Image bulletImage = (listFormat as ListFormat).GetBulletImage();
      if (bulletImage == null || float.IsNaN(textInfo.Bounds.Width) || float.IsNaN(textInfo.Bounds.Height))
        return;
      MemoryStream stream = new MemoryStream();
      bulletImage.Save(stream, bulletImage.Format);
      stream.Position = 0L;
      if (System.Drawing.Image.FromStream((Stream) new MemoryStream(bulletImage.ImageData)) is Metafile && Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.IsAzureCompatible)
        return;
      this.DrawImage(System.Drawing.Image.FromStream((Stream) stream), textInfo.Bounds);
    }
    else
    {
      FontStyle fontStyle = FontStyle.Regular;
      if (((ListFormat) listFormat).GetDefaultListType() == ListType.Numbered)
      {
        if (paragraphImpl.TextParts.Count != 0)
          fontStyle = paragraphImpl.GetFontStyle(paragraphImpl.TextParts[0].Font, FontScriptType.English);
        if (Syncfusion.Presentation.Drawing.Helper.HasFlag(fontStyle, FontStyle.Underline))
          fontStyle ^= FontStyle.Underline;
      }
      string fontName = !Syncfusion.Presentation.Drawing.Helper.IsGeometricShapesSymbol(((ListFormat) listFormat).GetDefaultBulletCharacter()) ? ((ListFormat) listFormat).GetDefaultBulletFontName() : "Segoe UI Symbol";
      float defaultBulletSize = ((ListFormat) listFormat).GetDefaultBulletSize();
      System.Drawing.Font font = paragraphImpl.Presentation.FontSettings.GetFont(fontName, defaultBulletSize, fontStyle);
      this.DrawBullet(paragraphImpl, (ListFormat) listFormat, font, textInfo.Bounds, TextCapsType.None);
    }
  }

  internal void DrawString(
    TextPart textPart,
    Paragraph paragraphImpl,
    string text,
    System.Drawing.Font systemFont,
    object brush,
    RectangleF bounds,
    TextCapsType capsType)
  {
    switch (capsType)
    {
      case TextCapsType.None:
        this.DrawPathString(textPart, paragraphImpl, text, systemFont, brush, bounds);
        break;
      case TextCapsType.Small:
        this.DrawSmallCapString(text, systemFont, brush, bounds, paragraphImpl, textPart);
        break;
      case TextCapsType.All:
        this.DrawPathString(textPart, paragraphImpl, text.ToUpper(), systemFont, brush, bounds);
        break;
    }
  }

  internal void Rotate(ShapeFrame shapeFrame, RectangleF rect)
  {
    float ang = (float) (shapeFrame.GetDefaultRotation() / 60000);
    if ((double) ang > 360.0)
      ang %= 360f;
    bool flipVertical = shapeFrame.FlipVertical;
    bool flipHorizontal = shapeFrame.FlipHorizontal;
    if ((double) ang == 0.0 && !flipVertical && !flipHorizontal)
      return;
    this.Transform(this.GetTransformMatrix(rect, ang, flipVertical, flipHorizontal));
  }

  private void DrawSmallCapString(
    string text,
    System.Drawing.Font font,
    object textBrush,
    RectangleF bounds,
    Paragraph paragraphImpl,
    TextPart textPart)
  {
    System.Drawing.Font font1 = paragraphImpl.Presentation.FontSettings.GetFont(font.Name, (double) font.Size * 0.8 > 3.0 ? font.Size * 0.8f : 2f, font.Style);
    float num = 0.0f;
    float ascent1 = paragraphImpl.FindAscent(font);
    float ascent2 = paragraphImpl.FindAscent(font1);
    string empty1 = string.Empty;
    string empty2 = string.Empty;
    List<char> charList = new List<char>();
    List<string> stringList = new List<string>();
    foreach (char c in text)
    {
      if (char.IsUpper(c) || !char.IsLetter(c) && !c.Equals(' '))
      {
        if (empty2.Length != 0)
        {
          charList.Add('s');
          stringList.Add(empty2.ToUpper());
          empty2 = string.Empty;
        }
        empty1 += c.ToString((IFormatProvider) CultureInfo.InvariantCulture);
      }
      else
      {
        if (empty1.Length != 0)
        {
          charList.Add('c');
          stringList.Add(empty1);
          empty1 = string.Empty;
        }
        empty2 += c.ToString((IFormatProvider) CultureInfo.InvariantCulture);
      }
    }
    if (empty2.Length != 0)
    {
      charList.Add('s');
      stringList.Add(empty2.ToUpper());
    }
    else if (empty1.Length != 0)
    {
      charList.Add('c');
      stringList.Add(empty1);
    }
    SizeF sizeF;
    for (int index = 0; index < charList.Count; ++index)
    {
      if (charList[index] == 'c')
      {
        sizeF = paragraphImpl.MeasureString(stringList[index], font, TextCapsType.None);
        this.DrawString(textPart, paragraphImpl, stringList[index], font, textBrush, new RectangleF(bounds.X + num, bounds.Y, bounds.Width, bounds.Height), TextCapsType.None);
        num += sizeF.Width;
      }
      else
      {
        sizeF = paragraphImpl.MeasureString(stringList[index].ToUpper(), font1, TextCapsType.None);
        this.DrawString(textPart, paragraphImpl, stringList[index], font1, textBrush, new RectangleF(bounds.X + num, bounds.Y + (ascent1 - ascent2), bounds.Width, bounds.Height), TextCapsType.None);
        num += sizeF.Width;
      }
    }
  }

  internal bool IsShapeNeedToBeFill(AutoShapeType shapeType)
  {
    switch (shapeType)
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

  private bool CanDrawShape(Shape masterShape)
  {
    return masterShape.DrawingType != DrawingType.PlaceHolder;
  }

  internal bool IsNeedToBeDrawn(RectangleF bounds, TextureFill textureFill)
  {
    return textureFill.BaseShape == null && textureFill.TileMode == TileMode.Stretch && textureFill.TilePicOption == null;
  }

  internal void ApplyImageTransparency(ImageAttributes imgAttribute, float transparency)
  {
    imgAttribute.SetColorMatrix(new ColorMatrix()
    {
      Matrix33 = transparency
    }, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
  }
}
