// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Rendering.GDIRenderer
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Drawing;
using Syncfusion.Presentation.Layouting;
using Syncfusion.Presentation.RichText;
using Syncfusion.Presentation.SlideImplementation;
using Syncfusion.Presentation.TableImplementation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;

#nullable disable
namespace Syncfusion.Presentation.Rendering;

internal class GDIRenderer : RendererBase
{
  private const double _alfhaConstantSerializer = 100000.0;
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
  private Graphics _graphics;

  internal GDIRenderer()
  {
  }

  internal GDIRenderer(ISlide slide)
    : base(slide)
  {
    this._graphics = (slide as Slide).Presentation.Graphics;
  }

  internal Graphics Graphics
  {
    get => this._graphics;
    set => this._graphics = value;
  }

  internal override void DrawImage(MemoryStream stream, RectangleF bounds)
  {
    this._graphics.DrawImage(Image.FromStream((Stream) stream), bounds);
  }

  internal override void ResetTransform() => this._graphics.ResetTransform();

  internal override void Transform(Matrix matrix) => this._graphics.Transform = matrix;

  internal override void RotateTransform(float angle) => this._graphics.RotateTransform(angle);

  internal override SizeF MeasureString(
    string text,
    System.Drawing.Font font,
    StringFormat stringFormat,
    Graphics graphics)
  {
    return graphics.MeasureString(text, font, new PointF(0.0f, 0.0f), stringFormat);
  }

  internal override void TranslateTransform(float dx, float dy)
  {
    this._graphics.TranslateTransform(dx, dy);
  }

  internal override void DrawImage(
    Image image,
    RectangleF bounds,
    ImageAttributes imageAttributes,
    Picture picture)
  {
    IFill fill = picture.Fill;
    if (fill != null && fill.FillType != FillType.None)
    {
      GraphicsPath path = new GraphicsPath();
      path.AddRectangle(bounds);
      this.FillBackground((IShape) picture, path, fill);
    }
    if (imageAttributes != null)
      this._graphics.DrawImage(image, Rectangle.Round(bounds), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttributes);
    else
      this._graphics.DrawImage(image, bounds);
  }

  internal override void DrawImage(Image image, RectangleF bounds)
  {
    this._graphics.DrawImage(image, bounds);
  }

  internal override void DrawBullet(
    Paragraph paragraphImpl,
    ListFormat bulletFormat,
    System.Drawing.Font systemFont,
    RectangleF bounds,
    TextCapsType capsType)
  {
    IColor defaultBulletColor = bulletFormat.GetDefaultBulletColor();
    SolidBrush brush = new SolidBrush(Color.FromArgb((int) defaultBulletColor.R, (int) defaultBulletColor.G, (int) defaultBulletColor.B));
    this.DrawString((TextPart) null, paragraphImpl, bulletFormat.GetDefaultBulletCharacter(), systemFont, (object) brush, bounds, TextCapsType.None);
  }

  internal override void DrawString(
    string text,
    System.Drawing.Font systemFont,
    object brush,
    RectangleF bounds,
    StringFormat stringFormat,
    float indent,
    float spacing,
    float width)
  {
    if ((double) bounds.X <= (double) bounds.X + (double) indent)
    {
      this._graphics.DrawString(text, systemFont, brush as Brush, new RectangleF(bounds.X + indent, bounds.Y, width, bounds.Height), stringFormat);
      if ((double) spacing <= 0.0)
        return;
      this._graphics.DrawString(" ", systemFont, brush as Brush, new RectangleF(bounds.X + indent + width, bounds.Y, spacing, bounds.Height), stringFormat);
    }
    else
      this._graphics.DrawString(text, systemFont, brush as Brush, new RectangleF(bounds.X, bounds.Y, width, bounds.Height), stringFormat);
  }

  internal override void FillSlideBackground(Slide slide)
  {
    GraphicsPath path = new GraphicsPath();
    RectangleF bounds = slide.SlideInfo.Bounds;
    path.AddRectangle(bounds);
    this.FillBackground((IShape) null, path, ((Background) slide.Background).GetDefaultFillFormat());
  }

  internal override object GetBrush(Syncfusion.Presentation.RichText.Font font, RectangleF bounds)
  {
    IFill defaultFillFormat = font.GetDefaultFillFormat();
    switch (defaultFillFormat.FillType)
    {
      case FillType.Automatic:
        defaultFillFormat.FillType = FillType.Solid;
        defaultFillFormat.SolidFill.Color = ColorObject.Black;
        return this.GetSolidBrush(font.GetDefaultColor());
      case FillType.Solid:
        return this.GetSolidBrush(font.GetDefaultColor());
      case FillType.Gradient:
        GradientFill gradientFill = defaultFillFormat.GradientFill as GradientFill;
        GraphicsPath path = new GraphicsPath();
        path.AddRectangle(bounds);
        bool hasTransparant = false;
        return (object) this.GetGradientBrush(path, gradientFill, ref hasTransparant);
      case FillType.Texture:
        TextureFill pictureFill = defaultFillFormat.PictureFill as TextureFill;
        Image image = (Image) new Bitmap((Stream) new MemoryStream(pictureFill.Data));
        ImageAttributes imgAttribute = new ImageAttributes();
        this.ApplyImageTransparency(imgAttribute, (float) pictureFill.ObtainTransparency() / 100000f);
        if (pictureFill.DuoTone.Count == 2)
          image = this.ApplyDuoTone(image, pictureFill.GetDefaultDuoTone());
        return (object) this.GetTextureBrush(image, imgAttribute, pictureFill, bounds);
      case FillType.Pattern:
        return (object) this.GetHatchBrush(defaultFillFormat.PatternFill as PatternFill);
      case FillType.None:
        IColor defaultColor = font.GetDefaultColor();
        return (object) new SolidBrush(Color.FromArgb(0, (int) defaultColor.R, (int) defaultColor.G, (int) defaultColor.B));
      default:
        return (object) null;
    }
  }

  internal override void DrawPathString(
    TextPart textPart,
    Paragraph paragraphImpl,
    string text,
    System.Drawing.Font systemFont,
    object brush,
    RectangleF bounds)
  {
    StringFormat format = paragraphImpl.StringFormt;
    bool flag = false;
    if (textPart != null && (textPart.Font as Syncfusion.Presentation.RichText.Font).Bidi)
      flag = true;
    if (Helper.HasFlag((Enum) format.FormatFlags, (Enum) StringFormatFlags.DirectionRightToLeft) && !paragraphImpl.IsRTLText(text) && textPart != null && !flag)
    {
      format = (StringFormat) format.Clone();
      format.FormatFlags &= ~StringFormatFlags.DirectionRightToLeft;
    }
    else if (!Helper.HasFlag((Enum) format.FormatFlags, (Enum) StringFormatFlags.DirectionRightToLeft) && (paragraphImpl.IsRTLText(text) || flag))
    {
      format = (StringFormat) format.Clone();
      format.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
    }
    this._graphics.DrawString(text, systemFont, brush as Brush, bounds, format);
  }

  internal override object GetSolidBrush(IColor color)
  {
    return (object) new SolidBrush(Color.FromArgb((int) byte.MaxValue - (int) color.A, (int) color.R, (int) color.G, (int) color.B));
  }

  internal override void SetPathClip(Shape shape, RectangleF bounds)
  {
    if ((shape.GetAutoShapeType() == AutoShapeType.Rectangle || shape.GetAutoShapeType() == ~AutoShapeType.Unknown) && (shape.PlaceholderFormat == null || shape.PlaceholderFormat.Type != PlaceholderType.Picture))
      return;
    Pen pen = new Pen(Color.Black);
    this._graphics.SetClip(Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.GetGraphicsPath(shape, bounds, ref pen, this)[0]);
  }

  internal override void ResetClip() => this._graphics.ResetClip();

  internal override void DrawImageBorder(Picture picture, RectangleF bounds)
  {
    ILineFormat lineFormat = picture.LineFormat;
    if (lineFormat == null)
      return;
    Pen pen = (Pen) null;
    GraphicsPath path;
    if (picture.AutoShapeType != AutoShapeType.Rectangle)
    {
      path = Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.GetGraphicsPath((Shape) picture, bounds, ref pen, this)[0];
    }
    else
    {
      float num = 0.0f;
      if (lineFormat != null)
        num = (float) ((LineFormat) lineFormat).GetDefaultWidth();
      if ((double) num <= 0.0)
        num = 1f;
      path = new GraphicsPath();
      path.AddRectangle(new RectangleF(bounds.X - num / 2f, bounds.Y - num / 2f, bounds.Width + num, bounds.Height + num));
    }
    pen = this.CreatePen((IShape) picture, lineFormat, path);
    if (pen == null || pen.Brush == null && pen.Color.ToArgb() == 0)
      return;
    if (picture.HasAdditionalGraphicsPath())
      path = Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.GetGraphicsPath((Shape) picture, bounds, ref pen, this)[0];
    this._graphics.DrawPath(pen, path);
  }

  internal override void DrawSingleShape(Shape shapeImpl, RectangleF bounds)
  {
    if (!this.IsShapeNeedToBeRender(shapeImpl))
      return;
    Pen pen1 = (Pen) null;
    GraphicsPath[] graphicsPath = Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.GetGraphicsPath(shapeImpl, bounds, ref pen1, this);
    Pen pen2 = this.CreatePen((IShape) shapeImpl, shapeImpl.LineFormat, graphicsPath[0]);
    this.AddHyperLink(shapeImpl, shapeImpl.Hyperlink, bounds);
    if (shapeImpl.HasAdditionalGraphicsPath())
      graphicsPath = Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.GetGraphicsPath(shapeImpl, bounds, ref pen2, this);
    this._graphics.ResetTransform();
    IFill format = (IFill) null;
    this.Rotate(shapeImpl.ShapeFrame, bounds);
    if (this.IsShapeNeedToBeFill(shapeImpl.GetAutoShapeType()))
    {
      if (shapeImpl is Picture && (shapeImpl as Picture).ImageData != null)
      {
        format = (IFill) new Fill(shapeImpl);
        format.FillType = FillType.Picture;
        TextureFill pictureFill = format.PictureFill as TextureFill;
        pictureFill.AddImageStream((Stream) new MemoryStream((shapeImpl as Picture).ImageData));
        pictureFill.AssignTransparency((shapeImpl as Picture).Amount);
        pictureFill.SetDueTone((shapeImpl as Picture).DuoTone);
      }
      else
        format = !shapeImpl.IsBgFill ? shapeImpl.GetDefaultFillFormat() : ((Background) shapeImpl.BaseSlide.Background).GetDefaultFillFormat();
    }
    for (int index = 0; index < graphicsPath.Length; ++index)
    {
      GraphicsPath path = graphicsPath[index];
      if (path.PointCount > 0)
      {
        PathFillMode pathFillMode = PathFillMode.Normal;
        if (!shapeImpl.GetCustomGeometry())
        {
          path.FillMode = FillMode.Winding;
        }
        else
        {
          List<Path2D> path2Dlist = shapeImpl.GetPath2DList();
          if (path2Dlist != null && path2Dlist.Count == graphicsPath.Length)
            pathFillMode = path2Dlist[index].FillMode;
        }
        if (format != null && pathFillMode != PathFillMode.None)
          this.FillBackground((IShape) shapeImpl, path, format, bounds);
      }
    }
    for (int index = 0; index < graphicsPath.Length; ++index)
    {
      GraphicsPath path = graphicsPath[index];
      if (path.PointCount > 0)
      {
        bool flag = true;
        if (shapeImpl.GetCustomGeometry())
        {
          List<Path2D> path2Dlist = shapeImpl.GetPath2DList();
          if (path2Dlist != null && path2Dlist.Count == graphicsPath.Length)
            flag = path2Dlist[index].IsStroke;
        }
        if (pen2 != null && flag && (pen2.Brush != null || pen2.Color.ToArgb() != 0))
          this._graphics.DrawPath(pen2, path);
      }
    }
    if (shapeImpl.ShapeInfo != null)
    {
      this.ApplyTextBodyRotation(shapeImpl.ShapeInfo.TextLayoutingBounds, shapeImpl);
      this.RotateText(shapeImpl.ShapeInfo.TextLayoutingBounds, ((TextBody) shapeImpl.TextBody).ObatinTextDirection());
      this.DrawParagraphs((IEnumerable<IParagraph>) shapeImpl.TextBody.Paragraphs);
    }
    this._graphics.ResetTransform();
  }

  internal override void DrawCell(IShape shape, ICell cell)
  {
    CellInfo cellInfo = ((Cell) cell).CellInfo;
    RectangleF bounds = cellInfo.Bounds;
    Pen pen1 = (Pen) null;
    GraphicsPath path = Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.GetGraphicsPath(shape as Shape, bounds, ref pen1, this)[0];
    Pen pen2 = this.CreatePen(shape, ((Cell) cell).GetDefaultLeftBorder(), path);
    Pen pen3 = this.CreatePen(shape, ((Cell) cell).GetDefaultTopBorder(), path);
    Pen pen4 = this.CreatePen(shape, ((Cell) cell).GetDefaultRightBorder(), path);
    Pen pen5 = this.CreatePen(shape, ((Cell) cell).GetDefaultBottomBorder(), path);
    Pen pen6 = this.CreatePen(shape, ((Cell) cell).GetDefaultDiagonalUpBorder(), path);
    Pen pen7 = this.CreatePen(shape, ((Cell) cell).GetDefaultDiagonalDownBorder(), path);
    this.FillBackground(shape, path, ((Cell) cell).GetDefaultFillFormat());
    this.DrawCellBorders(pen2, pen3, pen4, pen5, pen6, pen7, bounds);
    this._graphics.ResetTransform();
    this.RotateText(cellInfo.TextLayoutingBounds, (cell.TextBody as TextBody).ObatinTextDirection());
    this.DrawParagraphs((IEnumerable<IParagraph>) cell.TextBody.Paragraphs);
    this._graphics.ResetTransform();
  }

  internal void DrawSlide(Slide slide)
  {
    this._graphics = slide.Presentation.Graphics;
    this.BaseDrawSlide(slide);
  }

  internal void DrawSlide(NotesSlide notesSlide)
  {
    this._graphics = notesSlide.ParentSlide.Presentation.Graphics;
    this.BaseDrawSlide(notesSlide);
  }

  private Brush GetHatchBrush(PatternFill patternFill)
  {
    IColor foreColor1 = patternFill.ForeColor;
    IColor backColor1 = patternFill.BackColor;
    Color foreColor2 = Color.FromArgb((int) byte.MaxValue, (int) foreColor1.R, (int) foreColor1.G, (int) foreColor1.B);
    Color backColor2 = Color.FromArgb((int) byte.MaxValue - (int) backColor1.A, (int) backColor1.R, (int) backColor1.G, (int) backColor1.B);
    return (Brush) new HatchBrush(this.GetHatchStyle(patternFill.Pattern), foreColor2, backColor2);
  }

  private Brush GetTextureBrush(
    Image image,
    ImageAttributes imgAttribute,
    TextureFill textureFill,
    RectangleF bounds)
  {
    TextureBrush textureBrush;
    if (textureFill.TilePicOption != null)
    {
      textureBrush = new TextureBrush(image, new Rectangle(0, 0, image.Width, image.Height), imgAttribute);
      TilePicOption tilePicOption = textureFill.TilePicOption;
      float sx = (float) (tilePicOption.ScaleX / 100.0);
      float sy = (float) (tilePicOption.ScaleY / 100.0);
      float dx = (float) (tilePicOption.OffsetX / 12700.0);
      float dy = (float) (tilePicOption.OffsetY / 12700.0);
      switch (tilePicOption.MirrorType)
      {
        case MirrorType.None:
          textureBrush.WrapMode = WrapMode.Tile;
          break;
        case MirrorType.Horizonal:
          textureBrush.WrapMode = WrapMode.TileFlipX;
          break;
        case MirrorType.Vertical:
          textureBrush.WrapMode = WrapMode.TileFlipY;
          break;
        case MirrorType.Both:
          textureBrush.WrapMode = WrapMode.TileFlipXY;
          break;
      }
      textureBrush.TranslateTransform(dx, dy);
      textureBrush.ScaleTransform(sx, sy);
    }
    else
    {
      PicFormatOption picFormatOption = textureFill.PicFormatOption;
      if (picFormatOption.Left != 0.0 || picFormatOption.Top != 0.0 || picFormatOption.Right != 0.0 || picFormatOption.Bottom != 0.0)
        bounds = this.CropPosition(picFormatOption, bounds);
      textureBrush = new TextureBrush((Image) new Bitmap(image, (int) bounds.Width, (int) bounds.Height), new Rectangle(0, 0, (int) bounds.Width, (int) bounds.Height), imgAttribute);
      textureBrush.WrapMode = WrapMode.Clamp;
      textureBrush.TranslateTransform(bounds.X, bounds.Y);
    }
    return (Brush) textureBrush;
  }

  private Brush GetGradientBrush(
    GraphicsPath path,
    GradientFill gradientFill,
    ref bool hasTransparant)
  {
    RectangleF bounds = path.GetBounds();
    if ((double) bounds.Width == 0.0)
      bounds.Width = 0.1f;
    if ((double) bounds.Height == 0.0)
      bounds.Height = 0.1f;
    List<Color> gradientColors = new List<Color>();
    List<float> floatList1 = new List<float>();
    if (gradientFill.GradientStops.Count == 0)
    {
      gradientFill.GradientStops.Add(ColorObject.Black, 0.0f);
      gradientFill.GradientStops.Add(ColorObject.White, 100f);
    }
    bool flag1 = false;
    bool flag2 = false;
    for (int index = 0; index < gradientFill.GradientStops.Count; ++index)
    {
      float num = gradientFill.GradientStops[index].Position / 100f;
      IColor defaultColor = ((GradientStop) gradientFill.GradientStops[index]).GetDefaultColor();
      if ((int) byte.MaxValue - (int) defaultColor.A < (int) byte.MaxValue)
        hasTransparant = true;
      if (index != 0 && 1.0 - (double) num == (double) floatList1[index - 1])
        gradientColors.Insert(index - 1, Color.FromArgb((int) byte.MaxValue - (int) defaultColor.A, (int) defaultColor.R, (int) defaultColor.G, (int) defaultColor.B));
      else
        gradientColors.Add(Color.FromArgb((int) byte.MaxValue - (int) defaultColor.A, (int) defaultColor.R, (int) defaultColor.G, (int) defaultColor.B));
      floatList1.Add(1f - num);
      if (1.0 - (double) num == 0.0)
        flag1 = true;
      if (1.0 - (double) num == 1.0)
        flag2 = true;
    }
    for (int index1 = 1; index1 <= floatList1.Count; ++index1)
    {
      for (int index2 = 0; index2 < floatList1.Count - index1; ++index2)
      {
        if ((double) floatList1[index2] > (double) floatList1[index2 + 1])
        {
          float num = floatList1[index2];
          Color color = gradientColors[index2];
          floatList1[index2] = floatList1[index2 + 1];
          floatList1[index2 + 1] = num;
          gradientColors[index2] = gradientColors[index2 + 1];
          gradientColors[index2 + 1] = color;
        }
      }
    }
    List<Color> colorList = new List<Color>();
    List<float> floatList2 = new List<float>();
    if (!flag1)
    {
      floatList2.Add(0.0f);
      colorList.Add(gradientColors[0]);
    }
    for (int index = 0; index < gradientColors.Count; ++index)
    {
      floatList2.Add(floatList1[index]);
      colorList.Add(gradientColors[index]);
    }
    if (!flag2)
    {
      floatList2.Add(1f);
      colorList.Add(gradientColors[gradientColors.Count - 1]);
    }
    ColorBlend gradientColorBlend = new ColorBlend();
    gradientColorBlend.Colors = colorList.ToArray();
    gradientColorBlend.Positions = floatList2.ToArray();
    switch (gradientFill.Type)
    {
      case GradientFillType.Linear:
        return (Brush) this.GetLinearGradientBrush(gradientColors, gradientFill, bounds, gradientColorBlend);
      case GradientFillType.Radial:
        return (Brush) this.GetRadialGradientBrush(path, gradientFill, gradientColorBlend);
      case GradientFillType.Rectangle:
        return (Brush) this.GetRectangleGradientBrush(gradientFill, bounds, gradientColorBlend);
      case GradientFillType.Shape:
        return (Brush) new PathGradientBrush(path)
        {
          InterpolationColors = gradientColorBlend
        };
      default:
        return (Brush) null;
    }
  }

  private LinearGradientBrush GetLinearGradientBrush(
    List<Color> gradientColors,
    GradientFill gradientFill,
    RectangleF pathBounds,
    ColorBlend gradientColorBlend)
  {
    float num1 = 180f;
    Color gradientColor1 = gradientColors[0];
    Color gradientColor2 = gradientColors[gradientColors.Count - 1];
    float num2 = gradientFill.ShadeProperties is LineShadeImpl shadeProperties ? (float) (shadeProperties.Angle / 60000) : 0.0f;
    if ((double) num2 + (double) num1 == 180.0)
    {
      num1 = 0.0f;
      gradientColor1 = gradientColors[gradientColors.Count - 1];
      gradientColor2 = gradientColors[0];
      List<Color> colorList = new List<Color>();
      List<float> floatList = new List<float>();
      for (int index = gradientColorBlend.Colors.Length - 1; index >= 0; --index)
      {
        floatList.Add(1f - gradientColorBlend.Positions[index]);
        colorList.Add(gradientColorBlend.Colors[index]);
      }
      gradientColorBlend.Colors = colorList.ToArray();
      gradientColorBlend.Positions = floatList.ToArray();
    }
    return new LinearGradientBrush(pathBounds, gradientColor1, gradientColor2, num2 + num1, false)
    {
      InterpolationColors = gradientColorBlend
    };
  }

  private PathGradientBrush GetRectangleGradientBrush(
    GradientFill gradientFill,
    RectangleF pathBounds,
    ColorBlend gradientColorBlend)
  {
    PathShadeImpl shadeProperties = gradientFill.ShadeProperties as PathShadeImpl;
    GraphicsPath path = new GraphicsPath();
    path.AddRectangle(pathBounds);
    PathGradientBrush rectangleGradientBrush = new PathGradientBrush(path);
    if (shadeProperties.Left == 0 && shadeProperties.Bottom == 0)
      rectangleGradientBrush.CenterPoint = new PointF(pathBounds.Left, pathBounds.Bottom);
    else if (shadeProperties.Right == 0 && shadeProperties.Bottom == 0)
      rectangleGradientBrush.CenterPoint = new PointF(pathBounds.Right, pathBounds.Bottom);
    else if (shadeProperties.Right == 0 && shadeProperties.Top == 0)
      rectangleGradientBrush.CenterPoint = new PointF(pathBounds.Right, pathBounds.Top);
    else if (shadeProperties.Left == 0 && shadeProperties.Top == 0)
      rectangleGradientBrush.CenterPoint = new PointF(pathBounds.Left, pathBounds.Top);
    rectangleGradientBrush.InterpolationColors = gradientColorBlend;
    return rectangleGradientBrush;
  }

  private PathGradientBrush GetRadialGradientBrush(
    GraphicsPath path,
    GradientFill gradientFill,
    ColorBlend gradientColorBlend)
  {
    PathShadeImpl shadeProperties = gradientFill.ShadeProperties as PathShadeImpl;
    RectangleF bounds = path.GetBounds();
    GraphicsPath path1 = new GraphicsPath();
    RectangleF scaledRectangle = this.GetScaledRectangle(bounds, Convert.ToSingle(Math.Sqrt(3.0)));
    path1.AddEllipse(scaledRectangle);
    PathGradientBrush radialGradientBrush = new PathGradientBrush(path1);
    if (shadeProperties.Left == 0 && shadeProperties.Bottom == 0)
      radialGradientBrush.CenterPoint = new PointF(bounds.Left, bounds.Bottom);
    else if (shadeProperties.Right == 0 && shadeProperties.Bottom == 0)
      radialGradientBrush.CenterPoint = new PointF(bounds.Right, bounds.Bottom);
    else if (shadeProperties.Right == 0 && shadeProperties.Top == 0)
      radialGradientBrush.CenterPoint = new PointF(bounds.Right, bounds.Top);
    else if (shadeProperties.Left == 0 && shadeProperties.Top == 0)
      radialGradientBrush.CenterPoint = new PointF(bounds.Left, bounds.Top);
    radialGradientBrush.InterpolationColors = gradientColorBlend;
    return radialGradientBrush;
  }

  private RectangleF GetScaledRectangle(RectangleF rect, float scale)
  {
    float width = rect.Width * scale;
    float height = rect.Height * scale;
    float num1 = width - rect.Width;
    float x = rect.Left - num1 / 2f;
    float num2 = height - rect.Height;
    float y = rect.Top - num2 / 2f;
    return new RectangleF(x, y, width, height);
  }

  private HatchStyle GetHatchStyle(PatternFillType pattern)
  {
    HatchStyle hatchStyle = HatchStyle.Horizontal;
    switch (pattern)
    {
      case PatternFillType.Gray5:
        hatchStyle = HatchStyle.Percent05;
        break;
      case PatternFillType.Gray10:
        hatchStyle = HatchStyle.Percent10;
        break;
      case PatternFillType.Gray20:
        hatchStyle = HatchStyle.Percent20;
        break;
      case PatternFillType.Gray30:
        hatchStyle = HatchStyle.Percent30;
        break;
      case PatternFillType.Gray40:
        hatchStyle = HatchStyle.Percent40;
        break;
      case PatternFillType.Gray50:
        hatchStyle = HatchStyle.Percent50;
        break;
      case PatternFillType.Gray60:
        hatchStyle = HatchStyle.Percent60;
        break;
      case PatternFillType.Gray70:
        hatchStyle = HatchStyle.Percent70;
        break;
      case PatternFillType.Gray75:
        hatchStyle = HatchStyle.Percent75;
        break;
      case PatternFillType.Gray80:
        hatchStyle = HatchStyle.Percent80;
        break;
      case PatternFillType.Gray90:
        hatchStyle = HatchStyle.Percent90;
        break;
      case PatternFillType.Gray25:
        hatchStyle = HatchStyle.Percent25;
        break;
      case PatternFillType.LightDownwardDiagonal:
        hatchStyle = HatchStyle.LightDownwardDiagonal;
        break;
      case PatternFillType.LightUpwardDiagonal:
        hatchStyle = HatchStyle.LightUpwardDiagonal;
        break;
      case PatternFillType.DarkDownwardDiagonal:
        hatchStyle = HatchStyle.DarkDownwardDiagonal;
        break;
      case PatternFillType.DarkUpwardDiagonal:
        hatchStyle = HatchStyle.DarkUpwardDiagonal;
        break;
      case PatternFillType.WideDownwardDiagonal:
        hatchStyle = HatchStyle.WideDownwardDiagonal;
        break;
      case PatternFillType.WideUpwardDiagonal:
        hatchStyle = HatchStyle.WideUpwardDiagonal;
        break;
      case PatternFillType.LightVertical:
        hatchStyle = HatchStyle.LightVertical;
        break;
      case PatternFillType.LightHorizontal:
        hatchStyle = HatchStyle.LightHorizontal;
        break;
      case PatternFillType.NarrowVertical:
        hatchStyle = HatchStyle.NarrowVertical;
        break;
      case PatternFillType.NarrowHorizontal:
        hatchStyle = HatchStyle.NarrowHorizontal;
        break;
      case PatternFillType.DarkVertical:
        hatchStyle = HatchStyle.DarkVertical;
        break;
      case PatternFillType.DarkHorizontal:
        hatchStyle = HatchStyle.DarkHorizontal;
        break;
      case PatternFillType.DashedDownwardDiagonal:
        hatchStyle = HatchStyle.DashedDownwardDiagonal;
        break;
      case PatternFillType.DashedUpwardDiagonal:
        hatchStyle = HatchStyle.DashedUpwardDiagonal;
        break;
      case PatternFillType.DashedVertical:
        hatchStyle = HatchStyle.DashedVertical;
        break;
      case PatternFillType.DashedHorizontal:
        hatchStyle = HatchStyle.DashedHorizontal;
        break;
      case PatternFillType.SmallConfetti:
        hatchStyle = HatchStyle.SmallConfetti;
        break;
      case PatternFillType.LargeConfetti:
        hatchStyle = HatchStyle.LargeConfetti;
        break;
      case PatternFillType.ZigZag:
        hatchStyle = HatchStyle.ZigZag;
        break;
      case PatternFillType.Wave:
        hatchStyle = HatchStyle.Wave;
        break;
      case PatternFillType.DiagonalBrick:
        hatchStyle = HatchStyle.DiagonalBrick;
        break;
      case PatternFillType.HorizontalBrick:
        hatchStyle = HatchStyle.HorizontalBrick;
        break;
      case PatternFillType.Weave:
        hatchStyle = HatchStyle.Weave;
        break;
      case PatternFillType.Plaid:
        hatchStyle = HatchStyle.Plaid;
        break;
      case PatternFillType.Divot:
        hatchStyle = HatchStyle.Divot;
        break;
      case PatternFillType.DottedGrid:
        hatchStyle = HatchStyle.DottedGrid;
        break;
      case PatternFillType.DottedDiamond:
        hatchStyle = HatchStyle.DottedDiamond;
        break;
      case PatternFillType.Shingle:
        hatchStyle = HatchStyle.Shingle;
        break;
      case PatternFillType.Trellis:
        hatchStyle = HatchStyle.Trellis;
        break;
      case PatternFillType.Sphere:
        hatchStyle = HatchStyle.Sphere;
        break;
      case PatternFillType.SmallGrid:
        hatchStyle = HatchStyle.SmallGrid;
        break;
      case PatternFillType.LargeGrid:
        hatchStyle = HatchStyle.Cross;
        break;
      case PatternFillType.SmallCheckerBoard:
        hatchStyle = HatchStyle.SmallCheckerBoard;
        break;
      case PatternFillType.LargeCheckerBoard:
        hatchStyle = HatchStyle.LargeCheckerBoard;
        break;
      case PatternFillType.OutlinedDiamond:
        hatchStyle = HatchStyle.OutlinedDiamond;
        break;
      case PatternFillType.SolidDiamond:
        hatchStyle = HatchStyle.SolidDiamond;
        break;
    }
    return hatchStyle;
  }

  internal void FillBackground(IShape shape, GraphicsPath path, IFill format)
  {
    if (path == null)
      return;
    this.FillBackground(shape, path, format, path.GetBounds());
  }

  internal void FillBackground(IShape shape, GraphicsPath path, IFill format, RectangleF bounds)
  {
    switch (format.FillType)
    {
      case FillType.Solid:
        this.FillSolidBackground(shape, path, format);
        break;
      case FillType.Gradient:
        GradientFill gradientFill = format.GradientFill as GradientFill;
        bool hasTransparant = false;
        Brush gradientBrush = this.GetGradientBrush(path, gradientFill, ref hasTransparant);
        if (hasTransparant && (double) bounds.Width > 0.0 && (double) bounds.Height > 0.0 && (shape is Shape && !(shape as Shape).HasAdditionalGraphicsPath() && (shape as Shape).IsFitWithInBounds() || !(shape is Shape)))
        {
          int width = Helper.PointToPixel((double) bounds.Width);
          int height = Helper.PointToPixel((double) bounds.Height);
          using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
          {
            width = (int) ((double) width / 96.0 * (double) graphics.DpiX);
            height = (int) ((double) height / 96.0 * (double) graphics.DpiY);
          }
          Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppPArgb);
          Pen pen = (Pen) null;
          AutoShapeType autoShapeType = ~AutoShapeType.Unknown;
          GraphicsPath path1;
          if (shape != null && shape is Shape)
          {
            path1 = Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.GetGraphicsPath(shape as Shape, new RectangleF(0.0f, 0.0f, bounds.Width, bounds.Height), ref pen, this)[0];
            if (!(shape as Shape).GetCustomGeometry())
              path1.FillMode = FillMode.Winding;
            autoShapeType = (shape as Shape).GetAutoShapeType();
          }
          else
          {
            path1 = new GraphicsPath();
            path1.AddRectangle(new RectangleF(0.0f, 0.0f, bounds.Width, bounds.Height));
          }
          gradientBrush = this.GetGradientBrush(path1, gradientFill, ref hasTransparant);
          using (Graphics graphics = Graphics.FromImage((Image) bitmap))
          {
            graphics.PageUnit = GraphicsUnit.Point;
            bitmap.SetResolution(graphics.DpiX, graphics.DpiY);
            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            graphics.CompositingQuality = CompositingQuality.GammaCorrected;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            if (autoShapeType != AutoShapeType.Rectangle)
              graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.Clear(Color.Transparent);
            graphics.FillPath(gradientBrush, path1);
          }
          this._graphics.DrawImage((Image) bitmap, new PointF(bounds.X, bounds.Y));
        }
        else
          this._graphics.FillPath(gradientBrush, path);
        gradientBrush.Dispose();
        break;
      case FillType.Picture:
        TextureFill pictureFill = format.PictureFill as TextureFill;
        this.FillTextureBackground(path, pictureFill);
        break;
      case FillType.Pattern:
        Brush hatchBrush = this.GetHatchBrush(format.PatternFill as PatternFill);
        this._graphics.FillPath(hatchBrush, path);
        hatchBrush.Dispose();
        break;
      case FillType.None:
        if (shape != null)
          break;
        this._graphics.Clear(Color.White);
        break;
    }
  }

  private void FillSolidBackground(IShape shape, GraphicsPath path, IFill format)
  {
    if (shape != null)
    {
      switch (((Shape) shape).DrawingType)
      {
        case DrawingType.None:
        case DrawingType.Table:
        case DrawingType.SmartArt:
          this._graphics.FillPath(this.GetSolidBrush(format.SolidFill.Color) as Brush, path);
          break;
        case DrawingType.TextBox:
        case DrawingType.PlaceHolder:
          if (format.SolidFill == null)
            break;
          this._graphics.FillPath(this.GetSolidBrush(format.SolidFill.Color) as Brush, path);
          break;
      }
    }
    else
    {
      IColor color = format.SolidFill.Color;
      this._graphics.FillPath((Brush) new SolidBrush(Color.FromArgb((int) byte.MaxValue - (int) color.A, (int) color.R, (int) color.G, (int) color.B)), path);
    }
  }

  private void FillTextureBackground(GraphicsPath path, TextureFill textureFill)
  {
    if (textureFill.Data == null || Image.FromStream((Stream) new MemoryStream(textureFill.Data)) is Metafile && Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.IsAzureCompatible)
      return;
    MemoryStream memoryStream = new MemoryStream(textureFill.Data);
    Image image = (Image) new Bitmap((Stream) memoryStream);
    ImageAttributes imageAttributes = new ImageAttributes();
    this.ApplyImageTransparency(imageAttributes, (float) textureFill.ObtainTransparency() / 100000f);
    RectangleF bounds = path.GetBounds();
    if (textureFill.DuoTone.Count == 2)
      image = this.ApplyDuoTone(image, textureFill.GetDefaultDuoTone());
    if (this.IsNeedToBeDrawn(bounds, textureFill))
    {
      PicFormatOption picFormatOption = textureFill.PicFormatOption;
      if (picFormatOption.Left != 0.0 || picFormatOption.Top != 0.0 || picFormatOption.Right != 0.0 || picFormatOption.Bottom != 0.0)
        bounds = this.CropPosition(picFormatOption, bounds);
      this._graphics.DrawImage((Image) new Bitmap(image), Rectangle.Round(bounds), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttributes);
    }
    else
    {
      Brush textureBrush = this.GetTextureBrush(image, imageAttributes, textureFill, bounds);
      this._graphics.FillPath(textureBrush, path);
      textureBrush.Dispose();
    }
    image.Dispose();
    memoryStream.Dispose();
  }

  private void DrawCellBorders(
    Pen leftPen,
    Pen topPen,
    Pen rightPen,
    Pen bottomPen,
    Pen diagonalUp,
    Pen diagonalDown,
    RectangleF cellBounds)
  {
    if (leftPen != null && (leftPen.Brush != null || leftPen.Color.ToArgb() != 0))
      this._graphics.DrawLine(leftPen, cellBounds.X, cellBounds.Y - ((double) leftPen.Width <= (double) topPen.Width ? topPen.Width / 2f : 0.0f), cellBounds.X, cellBounds.Bottom + ((double) leftPen.Width <= (double) bottomPen.Width ? bottomPen.Width / 2f : 0.0f));
    if (topPen != null && (topPen.Brush != null || topPen.Color.ToArgb() != 0))
      this._graphics.DrawLine(topPen, cellBounds.X - (leftPen == null || (double) topPen.Width > (double) leftPen.Width ? 0.0f : leftPen.Width / 2f), cellBounds.Y, cellBounds.Right + ((double) topPen.Width <= (double) rightPen.Width ? rightPen.Width / 2f : 0.0f), cellBounds.Y);
    if (rightPen != null && (rightPen.Brush != null || rightPen.Color.ToArgb() != 0))
      this._graphics.DrawLine(rightPen, cellBounds.Right, cellBounds.Y - (topPen == null || (double) rightPen.Width > (double) topPen.Width ? 0.0f : topPen.Width / 2f), cellBounds.Right, cellBounds.Bottom + ((double) rightPen.Width <= (double) bottomPen.Width ? bottomPen.Width / 2f : 0.0f));
    if (bottomPen != null && (bottomPen.Brush != null || bottomPen.Color.ToArgb() != 0))
      this._graphics.DrawLine(bottomPen, cellBounds.X - (leftPen == null || (double) bottomPen.Width > (double) leftPen.Width ? 0.0f : leftPen.Width / 2f), cellBounds.Bottom, cellBounds.Right + (rightPen == null || (double) bottomPen.Width > (double) rightPen.Width ? 0.0f : rightPen.Width / 2f), cellBounds.Bottom);
    if (diagonalUp != null && (diagonalUp.Brush != null || diagonalUp.Color.ToArgb() != 0))
      this._graphics.DrawLine(diagonalUp, cellBounds.Left, cellBounds.Bottom, cellBounds.Right, cellBounds.Top);
    if (diagonalDown == null || diagonalDown.Brush == null && diagonalDown.Color.ToArgb() == 0)
      return;
    this._graphics.DrawLine(diagonalDown, cellBounds.Left, cellBounds.Top, cellBounds.Right, cellBounds.Bottom);
  }

  private Pen CreatePen(IShape shape, ILineFormat lineFormat, GraphicsPath path)
  {
    Color color1 = Color.Empty;
    if (lineFormat == null)
      return (Pen) null;
    Pen pen = (Pen) null;
    IFill defaultFillFormat = ((LineFormat) lineFormat).GetDefaultFillFormat();
    if (defaultFillFormat.FillType != FillType.None && defaultFillFormat.FillType != FillType.Automatic && defaultFillFormat.FillType != FillType.Gradient)
    {
      switch (((Shape) shape).DrawingType)
      {
        case DrawingType.None:
        case DrawingType.Table:
        case DrawingType.SmartArt:
          IColor color2 = defaultFillFormat.SolidFill.Color;
          color1 = Color.FromArgb((int) byte.MaxValue - (int) color2.A, (int) color2.R, (int) color2.G, (int) color2.B);
          break;
        case DrawingType.TextBox:
        case DrawingType.PlaceHolder:
          if (defaultFillFormat.SolidFill != null)
          {
            IColor color3 = defaultFillFormat.SolidFill.Color;
            color1 = Color.FromArgb((int) byte.MaxValue - (int) color3.A, (int) color3.R, (int) color3.G, (int) color3.B);
            break;
          }
          break;
      }
    }
    else if (defaultFillFormat.FillType == FillType.Gradient)
    {
      GradientFill gradientFill = defaultFillFormat.GradientFill as GradientFill;
      bool hasTransparant = false;
      pen = new Pen(this.GetGradientBrush(path, gradientFill, ref hasTransparant));
    }
    if (pen == null)
      pen = new Pen(color1);
    switch (((LineFormat) lineFormat).GetDefaultCapStyle())
    {
      case LineCapStyle.Round:
      case LineCapStyle.Flat:
        pen.DashCap = (DashCap) Enum.Parse(typeof (DashCap), ((LineFormat) lineFormat).GetDefaultCapStyle().ToString());
        break;
      case LineCapStyle.Square:
        pen.DashCap = DashCap.Triangle;
        break;
    }
    switch (((LineFormat) lineFormat).GetDefaultLineJoinType())
    {
      case LineJoinType.Miter:
        pen.LineJoin = LineJoin.Miter;
        break;
      case LineJoinType.Round:
        pen.LineJoin = LineJoin.Round;
        break;
      case LineJoinType.Bevel:
        pen.LineJoin = LineJoin.Bevel;
        break;
    }
    switch (lineFormat.DashStyle)
    {
      case LineDashStyle.Solid:
        pen.DashStyle = DashStyle.Solid;
        break;
      case LineDashStyle.Dash:
        pen.DashStyle = DashStyle.Dash;
        break;
      case LineDashStyle.DashDot:
        pen.DashStyle = DashStyle.DashDot;
        break;
      case LineDashStyle.DashDotDot:
        pen.DashPattern = new float[6]
        {
          8f,
          2f,
          1f,
          2f,
          1f,
          2f
        };
        break;
      case LineDashStyle.DashLongDash:
        pen.DashPattern = new float[2]{ 8f, 2f };
        break;
      case LineDashStyle.DashLongDashDot:
        pen.DashPattern = new float[4]{ 8f, 2f, 1f, 2f };
        break;
      case LineDashStyle.RoundDot:
        pen.DashStyle = DashStyle.Dot;
        break;
      case LineDashStyle.SquareDot:
        pen.DashPattern = new float[2]{ 3f, 0.5f };
        break;
    }
    switch (lineFormat.Style)
    {
      case LineStyle.ThickBetweenThin:
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
      case LineStyle.ThinThick:
        pen.CompoundArray = new float[4]
        {
          0.0f,
          0.16666f,
          0.3f,
          1f
        };
        break;
      case LineStyle.ThickThin:
        pen.CompoundArray = new float[4]
        {
          0.0f,
          0.6f,
          0.73333f,
          1f
        };
        break;
      case LineStyle.ThinThin:
        pen.CompoundArray = new float[4]
        {
          0.0f,
          0.3333333f,
          0.6666667f,
          1f
        };
        break;
    }
    CustomLineCap customLineCap1 = this.GetCustomLineCap(lineFormat.BeginArrowheadStyle, lineFormat.BeginArrowheadLength, lineFormat.BeginArrowheadWidth);
    CustomLineCap customLineCap2 = this.GetCustomLineCap(lineFormat.EndArrowheadStyle, lineFormat.EndArrowheadLength, lineFormat.EndArrowheadWidth);
    if (customLineCap1 != null)
    {
      pen.StartCap = LineCap.Custom;
      pen.CustomStartCap = customLineCap1;
    }
    if (customLineCap2 != null)
    {
      pen.EndCap = LineCap.Custom;
      pen.CustomEndCap = customLineCap2;
    }
    if (((LineFormat) lineFormat).GetDefaultWidth() > 0.0)
      pen.Width = (float) ((LineFormat) lineFormat).GetDefaultWidth();
    return pen;
  }

  private CustomLineCap GetCustomLineCap(
    ArrowheadStyle arrowheadStyle,
    ArrowheadLength arrowheadLength,
    ArrowheadWidth arrowheadWidth)
  {
    float baseInset;
    GraphicsPath lineGapGraphicsPath = this.GetCustomLineGapGraphicsPath(arrowheadStyle, arrowheadLength, arrowheadWidth, out baseInset);
    if (lineGapGraphicsPath == null)
      return (CustomLineCap) null;
    CustomLineCap customLineCap;
    if (arrowheadStyle == ArrowheadStyle.ArrowOpen)
    {
      customLineCap = new CustomLineCap((GraphicsPath) null, lineGapGraphicsPath, LineCap.Round, baseInset);
      customLineCap.SetStrokeCaps(LineCap.Round, LineCap.Round);
    }
    else
      customLineCap = new CustomLineCap(lineGapGraphicsPath, (GraphicsPath) null, LineCap.Triangle, baseInset);
    return customLineCap;
  }

  private GraphicsPath GetCustomLineGapGraphicsPath(
    ArrowheadStyle arrowheadStyle,
    ArrowheadLength arrowheadLength,
    ArrowheadWidth arrowheadWidth,
    out float baseInset)
  {
    baseInset = 0.0f;
    if (arrowheadStyle == ArrowheadStyle.None)
      return (GraphicsPath) null;
    GraphicsPath graphicsPath = new GraphicsPath(FillMode.Winding);
    float styleValue;
    if (this.GetArrowheadStyleValue(arrowheadStyle, graphicsPath, out styleValue))
      return (GraphicsPath) null;
    float num1 = (float) ((sbyte) 2 + arrowheadLength);
    if (arrowheadLength == ArrowheadLength.Long)
      ++num1;
    float num2 = (float) ((sbyte) 2 + arrowheadWidth);
    if (arrowheadWidth == ArrowheadWidth.Wide)
      ++num2;
    baseInset = num1 * styleValue;
    Matrix matrix = new Matrix();
    matrix.Scale(num2 / 10f, num1 / 10f);
    graphicsPath.Transform(matrix);
    return graphicsPath;
  }

  private bool GetArrowheadStyleValue(
    ArrowheadStyle arrowheadStyle,
    GraphicsPath graphicsPath,
    out float styleValue)
  {
    switch (arrowheadStyle)
    {
      case ArrowheadStyle.Arrow:
        graphicsPath.AddLines(this._arrowPoints);
        graphicsPath.CloseFigure();
        styleValue = 1f;
        break;
      case ArrowheadStyle.ArrowStealth:
        graphicsPath.AddLines(this._arrowStealthPoints);
        graphicsPath.CloseFigure();
        styleValue = 0.55f;
        break;
      case ArrowheadStyle.ArrowDiamond:
        graphicsPath.AddLines(this._arrowDiamondPoints);
        graphicsPath.CloseFigure();
        styleValue = 0.4f;
        break;
      case ArrowheadStyle.ArrowOval:
        graphicsPath.AddEllipse(-5f, -5f, 10f, 10f);
        graphicsPath.CloseFigure();
        styleValue = 0.4f;
        break;
      case ArrowheadStyle.ArrowOpen:
        graphicsPath.AddLines(this._arrowOpenPoints);
        styleValue = 0.3f;
        break;
      default:
        styleValue = 0.0f;
        return true;
    }
    return false;
  }
}
