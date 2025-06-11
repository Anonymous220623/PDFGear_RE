// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Rendering.PDFRenderer
// Assembly: Syncfusion.PresentationToPdfConverter.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 66FE5253-50B1-47E3-888F-DF2FAFB49C7E
// Assembly location: C:\Program Files\PDFgear\Syncfusion.PresentationToPdfConverter.Base.dll

using Syncfusion.Office;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
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
using System.Text;
using System.Xml;

#nullable disable
namespace Syncfusion.Presentation.Rendering;

internal class PDFRenderer : RendererBase
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
  private PdfGraphics _pdfGraphics;
  private int _imageQuality;
  private System.Drawing.Graphics _graphics;
  private PdfDocument _pdfDocument;
  private byte _bFlag;
  private PrivateFontCollection m_privateFontCollection;
  private Dictionary<string, Stream> m_privateFontStream;
  private List<FallbackFont> m_fallbackFonts;

  internal PDFRenderer()
  {
  }

  internal PDFRenderer(
    PdfGraphics pdfGraphics,
    System.Drawing.Graphics graphics,
    GraphicsUnit pageUnit,
    PdfDocument pdfDocument)
  {
    this._pdfGraphics = pdfGraphics != null ? pdfGraphics : throw new ArgumentException("Graphics");
    this._pdfGraphics.PageUnit = pageUnit;
    this._graphics = graphics;
    this._graphics.PageUnit = pageUnit;
    this._pdfDocument = pdfDocument;
  }

  internal PdfGraphics PDFGraphics
  {
    get => this._pdfGraphics;
    set
    {
      if (this._pdfGraphics == value)
        return;
      this._pdfGraphics = value;
    }
  }

  internal int ImageQuality
  {
    get => this._imageQuality;
    set => this._imageQuality = value;
  }

  internal bool EmbedFonts
  {
    get => ((int) this._bFlag & 8) >> 3 != 0;
    set => this._bFlag = (byte) ((int) this._bFlag & 247 | (value ? 1 : 0) << 3);
  }

  internal bool EmbedCompleteFonts
  {
    get => ((int) this._bFlag & 16 /*0x10*/) >> 4 != 0;
    set => this._bFlag = (byte) ((int) this._bFlag & 239 | (value ? 1 : 0) << 4);
  }

  internal PrivateFontCollection PrivateFonts
  {
    get => this.m_privateFontCollection;
    set
    {
      if (value == null)
        return;
      this.m_privateFontCollection = value;
    }
  }

  internal List<FallbackFont> FallbackFonts
  {
    get => this.m_fallbackFonts;
    set => this.m_fallbackFonts = value;
  }

  internal Dictionary<string, Stream> FontStreams
  {
    get => this.m_privateFontStream;
    set
    {
      if (value == null)
        return;
      this.m_privateFontStream = value;
    }
  }

  internal override void SetPathClip(Shape shape, RectangleF bounds)
  {
    if ((shape.GetAutoShapeType() == AutoShapeType.Rectangle || shape.GetAutoShapeType() == ~AutoShapeType.Unknown) && (shape.PlaceholderFormat == null || shape.PlaceholderFormat.Type != PlaceholderType.Picture))
      return;
    PdfPen pen = new PdfPen(Color.Black);
    this._pdfGraphics.SetClip(this.GetGraphicsPath(bounds, ref pen, this._pdfGraphics, shape)[0]);
  }

  internal override void ResetClip() => this._pdfGraphics.ResetClip();

  internal override void DrawImage(MemoryStream stream, RectangleF bounds)
  {
    PdfImage pdfImage = this.GetPdfImage(stream);
    if (pdfImage == null)
      return;
    this._pdfGraphics.DrawImage(pdfImage, bounds);
  }

  internal override void ResetTransform() => this._pdfGraphics.ResetTransform();

  internal override void RotateTransform(float angle) => this._pdfGraphics.RotateTransform(angle);

  internal override void TranslateTransform(float dx, float dy)
  {
    this._pdfGraphics.TranslateTransform(dx, dy);
  }

  internal override void Transform(Matrix matrix) => this._pdfGraphics.Transform = matrix;

  internal override void DrawImage(
    Image image,
    RectangleF bounds,
    ImageAttributes imageAttributes,
    Picture picture)
  {
    IFill fill = picture.Fill;
    if (fill != null && fill.FillType != FillType.None)
    {
      PdfPath path = new PdfPath();
      path.AddRectangle(bounds);
      this.FillBackground((IShape) picture, path, fill);
    }
    PdfGraphicsState state = (PdfGraphicsState) null;
    float alpha = (float) picture.Amount / 100000f;
    if ((double) alpha != 1.0)
    {
      state = this._pdfGraphics.Save();
      this._pdfGraphics.SetTransparency(alpha);
    }
    this.DrawImage(image, bounds);
    if ((double) alpha == 1.0)
      return;
    this._pdfGraphics.Restore(state);
  }

  internal override SizeF MeasureString(
    string text,
    System.Drawing.Font font,
    StringFormat stringFormat,
    System.Drawing.Graphics graphics)
  {
    if (this.FallbackFonts != null)
    {
      bool flag = this.EmbedFonts || this.EmbedCompleteFonts || Encoding.UTF8.GetByteCount(text) != text.Length;
      PdfTrueTypeFont pdfFont = new PdfTrueTypeFont(font, flag, false, false);
      PdfStringFormat pdfStringFormat = this.PDFGraphics.ConvertFormat(stringFormat);
      PdfTrueTypeFont fallbackFont = this.GetFallbackFont(pdfFont, this.FallbackFonts, font, text, pdfStringFormat, flag, false);
      if (pdfFont.Name != fallbackFont.Name)
        font = new System.Drawing.Font(fallbackFont.Name, font.Size, font.Style);
    }
    return graphics.MeasureString(text, font, new PointF(0.0f, 0.0f), stringFormat);
  }

  internal override void DrawBullet(
    Paragraph paragraphImpl,
    ListFormat bulletFormat,
    System.Drawing.Font systemFont,
    RectangleF bounds,
    TextCapsType capsType)
  {
    IColor defaultBulletColor = bulletFormat.GetDefaultBulletColor();
    PdfSolidBrush brush = new PdfSolidBrush((PdfColor) Color.FromArgb((int) defaultBulletColor.R, (int) defaultBulletColor.G, (int) defaultBulletColor.B));
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
    bool isEmbedFont = this.EmbedFonts || this.EmbedCompleteFonts || Encoding.UTF8.GetByteCount(text) != text.Length;
    PdfStringFormat pdfStringFormat = this.PDFGraphics.ConvertFormat(stringFormat);
    PdfFont pdfTrueTypeFont = (PdfFont) this.GetPdfTrueTypeFont(systemFont, isEmbedFont, this.EmbedCompleteFonts, text, pdfStringFormat);
    if ((double) bounds.X <= (double) bounds.X + (double) indent)
    {
      this.PDFGraphics.DrawString(text, pdfTrueTypeFont, brush as PdfBrush, new RectangleF(bounds.X + indent, bounds.Y, width, bounds.Height), pdfStringFormat, true);
      if ((double) spacing <= 0.0)
        return;
      this.PDFGraphics.DrawString(" ", pdfTrueTypeFont, brush as PdfBrush, new RectangleF(bounds.X + indent + width, bounds.Y, spacing, bounds.Height), pdfStringFormat, true);
    }
    else
      this.PDFGraphics.DrawString(text, pdfTrueTypeFont, brush as PdfBrush, new RectangleF(bounds.X, bounds.Y, width, bounds.Height), pdfStringFormat);
  }

  internal override void DrawImage(Image image, RectangleF bounds)
  {
    PdfImage pdfImage = this.GetPdfImage(image);
    if (pdfImage == null)
      return;
    this._pdfGraphics.DrawImage(pdfImage, bounds);
  }

  internal override void DrawImageBorder(Picture picture, RectangleF bounds)
  {
    ILineFormat lineFormat = picture.LineFormat;
    if (lineFormat == null)
      return;
    PdfPen pen = this.CreatePen((IShape) picture, lineFormat);
    if (pen != null && pen.Color.A > (byte) 0)
    {
      if (picture.AutoShapeType != AutoShapeType.Rectangle)
      {
        PdfPath path = this.GetGraphicsPath(bounds, ref pen, this._pdfGraphics, (Shape) picture)[0];
        this._pdfGraphics.DrawPath(pen, path);
      }
      else
        this._pdfGraphics.DrawRectangle(pen, bounds.X - pen.Width / 2f, bounds.Y - pen.Width / 2f, bounds.Width + pen.Width, bounds.Height + pen.Width);
    }
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
        PdfPath path = new PdfPath();
        path.AddRectangle(bounds);
        float shapeRotation = 0.0f;
        if (font.Paragraph != null && font.Paragraph.BaseShape != null && font.Paragraph.BaseShape.ShapeFrame != null)
        {
          float num = (float) (font.Paragraph.BaseShape.ShapeFrame.GetDefaultRotation() / 60000);
          if ((double) num != 0.0)
            shapeRotation = num;
        }
        return (object) this.GetGradientBrush(path, gradientFill, shapeRotation);
      case FillType.Texture:
        TextureFill pictureFill = defaultFillFormat.PictureFill as TextureFill;
        return this.GetSolidBrush(font.GetDefaultColor());
      case FillType.None:
        IColor defaultColor = font.GetDefaultColor();
        return (object) new PdfSolidBrush((PdfColor) Color.FromArgb(0, (int) defaultColor.R, (int) defaultColor.G, (int) defaultColor.B));
      default:
        return (object) null;
    }
  }

  internal override void FillSlideBackground(Slide slide)
  {
    PdfPath path = new PdfPath();
    RectangleF bounds = slide.SlideInfo.Bounds;
    path.AddRectangle(bounds);
    this.FillBackground((IShape) null, path, ((Background) slide.Background).GetDefaultFillFormat());
  }

  internal override void DrawSingleShape(Shape shapeImpl, RectangleF bounds)
  {
    if (!this.IsShapeNeedToBeRender(shapeImpl))
      return;
    PdfPen pen = this.CreatePen((IShape) shapeImpl, shapeImpl.LineFormat);
    this.AddHyperLink(shapeImpl, shapeImpl.Hyperlink, bounds);
    PdfPath[] graphicsPath = this.GetGraphicsPath(bounds, ref pen, this._pdfGraphics, shapeImpl);
    this._pdfGraphics.ResetTransform();
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
      PdfPath path = graphicsPath[index];
      if (path.PointCount > 0)
      {
        PathFillMode pathFillMode = PathFillMode.Normal;
        if (!shapeImpl.GetCustomGeometry())
        {
          path.FillMode = PdfFillMode.Winding;
        }
        else
        {
          List<Path2D> path2Dlist = shapeImpl.GetPath2DList();
          if (path2Dlist != null && path2Dlist.Count == graphicsPath.Length)
            pathFillMode = path2Dlist[index].FillMode;
        }
        if (format != null && pathFillMode != PathFillMode.None)
          this.FillBackground((IShape) shapeImpl, path, format);
      }
    }
    for (int index = 0; index < graphicsPath.Length; ++index)
    {
      PdfPath path = graphicsPath[index];
      if (path.PointCount > 0)
      {
        bool flag = true;
        if (shapeImpl.GetCustomGeometry())
        {
          List<Path2D> path2Dlist = shapeImpl.GetPath2DList();
          if (path2Dlist != null && path2Dlist.Count == graphicsPath.Length)
            flag = path2Dlist[index].IsStroke;
        }
        if (pen != null && flag && pen.Color.A > (byte) 0)
          this._pdfGraphics.DrawPath(pen, path);
      }
    }
    if (shapeImpl.ShapeInfo != null)
    {
      this.ApplyTextBodyRotation(shapeImpl.ShapeInfo.TextLayoutingBounds, shapeImpl);
      this.RotateText(shapeImpl.ShapeInfo.TextLayoutingBounds, ((TextBody) shapeImpl.TextBody).ObatinTextDirection());
      this.DrawParagraphs((IEnumerable<IParagraph>) shapeImpl.TextBody.Paragraphs);
    }
    this._pdfGraphics.ResetTransform();
  }

  internal override void DrawCell(IShape shape, ICell cell)
  {
    PdfPen pen1 = this.CreatePen(shape, ((Cell) cell).GetDefaultLeftBorder());
    PdfPen pen2 = this.CreatePen(shape, ((Cell) cell).GetDefaultTopBorder());
    PdfPen pen3 = this.CreatePen(shape, ((Cell) cell).GetDefaultRightBorder());
    PdfPen pen4 = this.CreatePen(shape, ((Cell) cell).GetDefaultBottomBorder());
    PdfPen pen5 = this.CreatePen(shape, ((Cell) cell).GetDefaultDiagonalUpBorder());
    PdfPen pen6 = this.CreatePen(shape, ((Cell) cell).GetDefaultDiagonalDownBorder());
    CellInfo cellInfo = ((Cell) cell).CellInfo;
    RectangleF bounds = cellInfo.Bounds;
    PdfPath path = this.GetGraphicsPath(bounds, ref pen1, this._pdfGraphics, (Shape) shape)[0];
    this.FillBackground(shape, path, ((Cell) cell).GetDefaultFillFormat());
    this.DrawCellBorders(pen1, pen2, pen3, pen4, pen5, pen6, bounds);
    this._pdfGraphics.ResetTransform();
    this.RotateText(cellInfo.TextLayoutingBounds, (cell.TextBody as TextBody).ObatinTextDirection());
    this.DrawParagraphs((IEnumerable<IParagraph>) cell.TextBody.Paragraphs);
    this._pdfGraphics.ResetTransform();
  }

  internal override void DrawPathString(
    TextPart textPart,
    Paragraph paragraphImpl,
    string text,
    System.Drawing.Font systemFont,
    object brush,
    RectangleF bounds)
  {
    if (textPart != null && textPart.IsWordSplitCharacter() && (textPart.Font as Syncfusion.Presentation.RichText.Font).Bidi)
    {
      char[] charArray = text.ToCharArray();
      Array.Reverse((Array) charArray);
      for (int index = 0; index < charArray.Length; ++index)
      {
        char ch = charArray[index];
        charArray[index] = this.InverseCharacter(ch);
      }
      text = new string(charArray);
    }
    bool isEmbedFont = this.EmbedFonts || this.EmbedCompleteFonts || Encoding.UTF8.GetByteCount(text) != text.Length;
    PdfStringFormat pdfStringFormat = this.PDFGraphics.ConvertFormat(paragraphImpl.StringFormt);
    PdfFont pdfTrueTypeFont = (PdfFont) this.GetPdfTrueTypeFont(systemFont, isEmbedFont, this.EmbedCompleteFonts, text, pdfStringFormat);
    if (textPart != null && textPart.IsHindiScript())
      pdfStringFormat.ComplexScript = true;
    this.PDFGraphics.DrawString(text, pdfTrueTypeFont, brush as PdfBrush, bounds, pdfStringFormat, true);
  }

  internal override object GetSolidBrush(IColor color)
  {
    return (object) new PdfSolidBrush((PdfColor) Color.FromArgb((int) byte.MaxValue - (int) color.A, (int) color.R, (int) color.G, (int) color.B));
  }

  internal PdfTrueTypeFont GetFallbackFont(
    PdfTrueTypeFont pdfFont,
    List<FallbackFont> fallbackFonts,
    System.Drawing.Font systemFont,
    string inputText,
    PdfStringFormat pdfStringFormat,
    bool isEmbedFont,
    bool isEmbedCompleteFont)
  {
    if (!string.IsNullOrEmpty(inputText) && !this.IsContainFont(pdfFont, inputText, pdfStringFormat))
    {
      for (int index = 0; index < fallbackFonts.Count; ++index)
      {
        FallbackFont fallbackFont = fallbackFonts[index];
        if (!string.IsNullOrEmpty(fallbackFont.FontNames) && fallbackFont.IsWithInRange(inputText))
        {
          string fontNames = fallbackFont.FontNames;
          char[] chArray = new char[1]{ ',' };
          foreach (string str in fontNames.Split(chArray))
          {
            PdfTrueTypeFont pdfFont1 = new PdfTrueTypeFont(new System.Drawing.Font(str.Trim(), systemFont.Size, systemFont.Style), isEmbedFont, false, isEmbedCompleteFont);
            if (this.IsContainFont(pdfFont1, inputText, pdfStringFormat))
              return pdfFont1;
          }
        }
      }
    }
    return pdfFont;
  }

  private bool IsContainFont(
    PdfTrueTypeFont pdfFont,
    string inputText,
    PdfStringFormat pdfStringFormat)
  {
    pdfFont.MeasureString(inputText, pdfStringFormat);
    return pdfFont.IsContainsFont;
  }

  private char InverseCharacter(char ch)
  {
    switch (ch)
    {
      case '(':
        return ')';
      case ')':
        return '(';
      case '<':
        return '>';
      case '>':
        return '<';
      case '[':
        return ']';
      case ']':
        return '[';
      case '{':
        return '}';
      case '}':
        return '{';
      default:
        return ch;
    }
  }

  internal void DrawSlide(Slide slide) => this.BaseDrawSlide(slide);

  internal void DrawSlide(NotesSlide notesSlide) => this.BaseDrawSlide(notesSlide);

  private PdfBrush GetHatchBrush(PatternFill patternFill)
  {
    IColor foreColor1 = patternFill.ForeColor;
    IColor backColor1 = patternFill.BackColor;
    Color foreColor2 = Color.FromArgb((int) byte.MaxValue, (int) foreColor1.R, (int) foreColor1.G, (int) foreColor1.B);
    Color backColor2 = Color.FromArgb((int) byte.MaxValue - (int) backColor1.A, (int) backColor1.R, (int) backColor1.G, (int) backColor1.B);
    return (PdfBrush) new PdfHatchBrush(this.GetHatchStyle(patternFill.Pattern), (PdfColor) foreColor2, (PdfColor) backColor2);
  }

  private PdfBrush GetTextureBrush(
    Image image,
    float transparancy,
    TextureFill textureFill,
    RectangleF bounds,
    PdfPath path)
  {
    PdfTextureBrush textureBrush = (PdfTextureBrush) null;
    if (textureFill.TilePicOption != null)
    {
      PdfImage pdfImage = this.GetPdfImage(image);
      if (pdfImage != null)
      {
        textureBrush = new PdfTextureBrush(pdfImage, (RectangleF) new Rectangle(0, 0, image.Width, image.Height), transparancy);
        TilePicOption tilePicOption = textureFill.TilePicOption;
        float scaleX = (float) (tilePicOption.ScaleX / 100.0);
        float scaleY = (float) (tilePicOption.ScaleY / 100.0);
        float offsetX = (float) (tilePicOption.OffsetX / 12700.0);
        float offsetY = (float) (tilePicOption.OffsetY / 12700.0);
        PdfTransformationMatrix transformationMatrix = new PdfTransformationMatrix();
        transformationMatrix.Translate(offsetX, offsetY);
        transformationMatrix.Scale(scaleX, scaleY);
        textureBrush.TransformationMatrix = transformationMatrix;
      }
    }
    else
    {
      PicFormatOption picFormatOption = textureFill.PicFormatOption;
      if (picFormatOption.Left != 0.0 || picFormatOption.Top != 0.0 || picFormatOption.Right != 0.0 || picFormatOption.Bottom != 0.0)
        bounds = this.CropPosition(picFormatOption, bounds);
      PdfImage image1 = (double) image.Width >= (double) bounds.Width && (double) image.Height >= (double) bounds.Height ? this.GetPdfImage(image) : this.GetPdfImage((Image) new Bitmap(image, (int) bounds.Width, (int) bounds.Height));
      this._pdfGraphics.SetTransparency(transparancy);
      this._pdfGraphics.SetClip(new PdfPath(path.PathPoints, path.PathTypes));
      this._pdfGraphics.TranslateTransform(bounds.X, bounds.Y);
      if (image1 != null)
        this._pdfGraphics.DrawImage(image1, new RectangleF(PointF.Empty, new SizeF(bounds.Width, bounds.Height)));
      this._pdfGraphics.ResetTransform();
    }
    return (PdfBrush) textureBrush;
  }

  private PdfBrush GetGradientBrush(PdfPath path, GradientFill gradientFill, float shapeRotation)
  {
    RectangleF bounds = path.GetBounds();
    if ((double) bounds.Width == 0.0)
      bounds.Width = 0.1f;
    if ((double) bounds.Height == 0.0)
      bounds.Height = 0.1f;
    List<PdfColor> gradientColors = new List<PdfColor>();
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
      if (index != 0 && 1.0 - (double) num == (double) floatList1[index - 1])
        gradientColors.Insert(index - 1, (PdfColor) Color.FromArgb((int) byte.MaxValue - (int) defaultColor.A, (int) defaultColor.R, (int) defaultColor.G, (int) defaultColor.B));
      else
        gradientColors.Add((PdfColor) Color.FromArgb((int) byte.MaxValue - (int) defaultColor.A, (int) defaultColor.R, (int) defaultColor.G, (int) defaultColor.B));
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
          PdfColor pdfColor = gradientColors[index2];
          floatList1[index2] = floatList1[index2 + 1];
          floatList1[index2 + 1] = num;
          gradientColors[index2] = gradientColors[index2 + 1];
          gradientColors[index2 + 1] = pdfColor;
        }
      }
    }
    List<PdfColor> finalGradientColors = new List<PdfColor>();
    List<float> floatList2 = new List<float>();
    if (!flag1)
    {
      floatList2.Add(0.0f);
      finalGradientColors.Add(gradientColors[0]);
    }
    for (int index = 0; index < gradientColors.Count; ++index)
    {
      floatList2.Add(floatList1[index]);
      finalGradientColors.Add(gradientColors[index]);
    }
    if (!flag2)
    {
      floatList2.Add(1f);
      finalGradientColors.Add(gradientColors[gradientColors.Count - 1]);
    }
    PdfColorBlend gradientColorBlend = new PdfColorBlend();
    gradientColorBlend.Colors = finalGradientColors.ToArray();
    gradientColorBlend.Positions = floatList2.ToArray();
    switch (gradientFill.Type)
    {
      case GradientFillType.Linear:
        return (PdfBrush) this.GetLinearGradientBrush(gradientColors, gradientFill, bounds, gradientColorBlend, shapeRotation);
      case GradientFillType.Radial:
        return (PdfBrush) this.GetRadialGradientBrush(path, gradientFill, gradientColorBlend, finalGradientColors);
      case GradientFillType.Rectangle:
        return (PdfBrush) this.GetRectangleGradientBrush(gradientFill, bounds, gradientColorBlend, finalGradientColors);
      case GradientFillType.Shape:
        return (PdfBrush) new PdfRadialGradientBrush(new PointF(bounds.Height, bounds.Bottom), bounds.Width, new PointF(bounds.Right, bounds.Width), bounds.Width, finalGradientColors[0], finalGradientColors[finalGradientColors.Count - 1])
        {
          InterpolationColors = gradientColorBlend
        };
      default:
        return (PdfBrush) null;
    }
  }

  private PdfLinearGradientBrush GetLinearGradientBrush(
    List<PdfColor> gradientColors,
    GradientFill gradientFill,
    RectangleF pathBounds,
    PdfColorBlend gradientColorBlend,
    float shapeRotation)
  {
    float num1 = 180f;
    PdfColor gradientColor1 = gradientColors[0];
    PdfColor gradientColor2 = gradientColors[gradientColors.Count - 1];
    float num2 = gradientFill.ShadeProperties is LineShadeImpl shadeProperties ? (float) (shadeProperties.Angle / 60000) : 0.0f;
    return new PdfLinearGradientBrush(pathBounds, gradientColor1, gradientColor2, num2 + num1)
    {
      InterpolationColors = gradientColorBlend
    };
  }

  private PdfRadialGradientBrush GetRectangleGradientBrush(
    GradientFill gradientFill,
    RectangleF pathBounds,
    PdfColorBlend gradientColorBlend,
    List<PdfColor> finalGradientColors)
  {
    PathShadeImpl shadeProperties = gradientFill.ShadeProperties as PathShadeImpl;
    new PdfPath().AddRectangle(pathBounds);
    if (shadeProperties.Left == 0 && shadeProperties.Bottom == 0)
      return new PdfRadialGradientBrush(new PointF(pathBounds.Left, pathBounds.Right), 0.0f, new PointF(pathBounds.Right, pathBounds.Width), pathBounds.Width, finalGradientColors[0], finalGradientColors[finalGradientColors.Count - 1])
      {
        InterpolationColors = gradientColorBlend
      };
    if (shadeProperties.Right == 0 && shadeProperties.Bottom == 0)
      return new PdfRadialGradientBrush(new PointF(pathBounds.Bottom, pathBounds.Height), pathBounds.Right, new PointF(pathBounds.Bottom, pathBounds.Right), pathBounds.Right, finalGradientColors[0], finalGradientColors[finalGradientColors.Count - 1])
      {
        InterpolationColors = gradientColorBlend
      };
    if (shadeProperties.Right == 0 && shadeProperties.Top == 0)
      return new PdfRadialGradientBrush(new PointF(pathBounds.Height, pathBounds.Bottom), 0.0f, new PointF(pathBounds.Right, pathBounds.Width), pathBounds.Width, finalGradientColors[0], finalGradientColors[finalGradientColors.Count - 1])
      {
        InterpolationColors = gradientColorBlend
      };
    if (shadeProperties.Left == 0 && shadeProperties.Top == 0)
      return new PdfRadialGradientBrush(new PointF(pathBounds.Left, pathBounds.Top), 0.0f, new PointF(pathBounds.Right, pathBounds.Width), pathBounds.Right, finalGradientColors[0], finalGradientColors[finalGradientColors.Count - 1])
      {
        InterpolationColors = gradientColorBlend
      };
    return new PdfRadialGradientBrush(new PointF(pathBounds.Right, pathBounds.Right), 0.0f, new PointF(pathBounds.Bottom, pathBounds.Right), pathBounds.Right, finalGradientColors[0], finalGradientColors[finalGradientColors.Count - 1])
    {
      InterpolationColors = gradientColorBlend
    };
  }

  private PdfRadialGradientBrush GetRadialGradientBrush(
    PdfPath path,
    GradientFill gradientFill,
    PdfColorBlend gradientColorBlend,
    List<PdfColor> finalGradientColors)
  {
    PathShadeImpl shadeProperties = gradientFill.ShadeProperties as PathShadeImpl;
    RectangleF bounds = path.GetBounds();
    this.GetScaledRectangle(bounds, Convert.ToSingle(Math.Sqrt(3.0)));
    return new PdfRadialGradientBrush(new PointF(bounds.Left, bounds.Right), 0.0f, new PointF(bounds.Bottom, bounds.Right), bounds.Height, finalGradientColors[0], finalGradientColors[finalGradientColors.Count - 1])
    {
      InterpolationColors = gradientColorBlend
    };
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

  private PdfHatchStyle GetHatchStyle(PatternFillType pattern)
  {
    PdfHatchStyle hatchStyle = PdfHatchStyle.Horizontal;
    switch (pattern)
    {
      case PatternFillType.Gray5:
        hatchStyle = PdfHatchStyle.Percent05;
        break;
      case PatternFillType.Gray10:
        hatchStyle = PdfHatchStyle.Percent10;
        break;
      case PatternFillType.Gray20:
        hatchStyle = PdfHatchStyle.Percent20;
        break;
      case PatternFillType.Gray30:
        hatchStyle = PdfHatchStyle.Percent30;
        break;
      case PatternFillType.Gray40:
        hatchStyle = PdfHatchStyle.Percent40;
        break;
      case PatternFillType.Gray50:
        hatchStyle = PdfHatchStyle.Percent50;
        break;
      case PatternFillType.Gray60:
        hatchStyle = PdfHatchStyle.Percent60;
        break;
      case PatternFillType.Gray70:
        hatchStyle = PdfHatchStyle.Percent70;
        break;
      case PatternFillType.Gray75:
        hatchStyle = PdfHatchStyle.Percent75;
        break;
      case PatternFillType.Gray80:
        hatchStyle = PdfHatchStyle.Percent80;
        break;
      case PatternFillType.Gray90:
        hatchStyle = PdfHatchStyle.Percent90;
        break;
      case PatternFillType.Gray25:
        hatchStyle = PdfHatchStyle.Percent25;
        break;
      case PatternFillType.LightDownwardDiagonal:
        hatchStyle = PdfHatchStyle.LightDownwardDiagonal;
        break;
      case PatternFillType.LightUpwardDiagonal:
        hatchStyle = PdfHatchStyle.LightUpwardDiagonal;
        break;
      case PatternFillType.DarkDownwardDiagonal:
        hatchStyle = PdfHatchStyle.DarkDownwardDiagonal;
        break;
      case PatternFillType.DarkUpwardDiagonal:
        hatchStyle = PdfHatchStyle.DarkUpwardDiagonal;
        break;
      case PatternFillType.WideDownwardDiagonal:
        hatchStyle = PdfHatchStyle.WideDownwardDiagonal;
        break;
      case PatternFillType.WideUpwardDiagonal:
        hatchStyle = PdfHatchStyle.WideUpwardDiagonal;
        break;
      case PatternFillType.LightVertical:
        hatchStyle = PdfHatchStyle.LightVertical;
        break;
      case PatternFillType.LightHorizontal:
        hatchStyle = PdfHatchStyle.LightHorizontal;
        break;
      case PatternFillType.NarrowVertical:
        hatchStyle = PdfHatchStyle.NarrowVertical;
        break;
      case PatternFillType.NarrowHorizontal:
        hatchStyle = PdfHatchStyle.NarrowHorizontal;
        break;
      case PatternFillType.DarkVertical:
        hatchStyle = PdfHatchStyle.DarkVertical;
        break;
      case PatternFillType.DarkHorizontal:
        hatchStyle = PdfHatchStyle.DarkHorizontal;
        break;
      case PatternFillType.DashedDownwardDiagonal:
        hatchStyle = PdfHatchStyle.DashedDownwardDiagonal;
        break;
      case PatternFillType.DashedUpwardDiagonal:
        hatchStyle = PdfHatchStyle.DashedUpwardDiagonal;
        break;
      case PatternFillType.DashedVertical:
        hatchStyle = PdfHatchStyle.DashedVertical;
        break;
      case PatternFillType.DashedHorizontal:
        hatchStyle = PdfHatchStyle.DashedHorizontal;
        break;
      case PatternFillType.SmallConfetti:
        hatchStyle = PdfHatchStyle.SmallConfetti;
        break;
      case PatternFillType.LargeConfetti:
        hatchStyle = PdfHatchStyle.LargeConfetti;
        break;
      case PatternFillType.ZigZag:
        hatchStyle = PdfHatchStyle.ZigZag;
        break;
      case PatternFillType.Wave:
        hatchStyle = PdfHatchStyle.Wave;
        break;
      case PatternFillType.DiagonalBrick:
        hatchStyle = PdfHatchStyle.DiagonalBrick;
        break;
      case PatternFillType.HorizontalBrick:
        hatchStyle = PdfHatchStyle.HorizontalBrick;
        break;
      case PatternFillType.Weave:
        hatchStyle = PdfHatchStyle.Weave;
        break;
      case PatternFillType.Plaid:
        hatchStyle = PdfHatchStyle.Plaid;
        break;
      case PatternFillType.Divot:
        hatchStyle = PdfHatchStyle.Divot;
        break;
      case PatternFillType.DottedGrid:
        hatchStyle = PdfHatchStyle.DottedGrid;
        break;
      case PatternFillType.DottedDiamond:
        hatchStyle = PdfHatchStyle.DottedDiamond;
        break;
      case PatternFillType.Shingle:
        hatchStyle = PdfHatchStyle.Shingle;
        break;
      case PatternFillType.Trellis:
        hatchStyle = PdfHatchStyle.Trellis;
        break;
      case PatternFillType.Sphere:
        hatchStyle = PdfHatchStyle.Sphere;
        break;
      case PatternFillType.SmallGrid:
        hatchStyle = PdfHatchStyle.SmallGrid;
        break;
      case PatternFillType.LargeGrid:
        hatchStyle = PdfHatchStyle.Cross;
        break;
      case PatternFillType.SmallCheckerBoard:
        hatchStyle = PdfHatchStyle.SmallCheckerBoard;
        break;
      case PatternFillType.LargeCheckerBoard:
        hatchStyle = PdfHatchStyle.LargeCheckerBoard;
        break;
      case PatternFillType.OutlinedDiamond:
        hatchStyle = PdfHatchStyle.OutlinedDiamond;
        break;
      case PatternFillType.SolidDiamond:
        hatchStyle = PdfHatchStyle.SolidDiamond;
        break;
    }
    return hatchStyle;
  }

  internal void FillBackground(IShape shape, PdfPath path, IFill format)
  {
    switch (format.FillType)
    {
      case FillType.Solid:
        this.FillSolidBackground(shape, path, format);
        break;
      case FillType.Gradient:
        GradientFill gradientFill = format.GradientFill as GradientFill;
        float shapeRotation = 0.0f;
        if (shape is Shape && (shape as Shape).ShapeFrame != null)
        {
          float num = (float) ((shape as Shape).ShapeFrame.GetDefaultRotation() / 60000);
          if ((double) num != 0.0)
            shapeRotation = num;
        }
        this._pdfGraphics.DrawPath(this.GetGradientBrush(path, gradientFill, shapeRotation), path);
        break;
      case FillType.Picture:
        TextureFill pictureFill = format.PictureFill as TextureFill;
        this.FillTextureBackground(path, pictureFill);
        break;
      case FillType.Pattern:
        this._pdfGraphics.DrawPath(this.GetHatchBrush(format.PatternFill as PatternFill), path);
        break;
      case FillType.None:
        if (shape != null)
          break;
        this._pdfGraphics.Clear((PdfColor) Color.White);
        break;
    }
  }

  private void FillSolidBackground(IShape shape, PdfPath path, IFill format)
  {
    if (shape != null)
    {
      switch (((Shape) shape).DrawingType)
      {
        case DrawingType.None:
        case DrawingType.Table:
        case DrawingType.SmartArt:
          IColor color1 = format.SolidFill.Color;
          PdfGraphicsState state1 = this.PDFGraphics.Save();
          this.PDFGraphics.SetTransparency((float) (1.0 - (double) color1.A / (double) byte.MaxValue));
          this._pdfGraphics.DrawPath(this.GetSolidBrush(color1) as PdfBrush, path);
          this.PDFGraphics.Restore(state1);
          break;
        case DrawingType.TextBox:
        case DrawingType.PlaceHolder:
          if (format.SolidFill == null)
            break;
          IColor color2 = format.SolidFill.Color;
          PdfGraphicsState state2 = this.PDFGraphics.Save();
          this.PDFGraphics.SetTransparency((float) (1.0 - (double) color2.A / (double) byte.MaxValue));
          this._pdfGraphics.DrawPath(this.GetSolidBrush(color2) as PdfBrush, path);
          this.PDFGraphics.Restore(state2);
          break;
      }
    }
    else
    {
      IColor color3 = format.SolidFill.Color;
      this._pdfGraphics.DrawPath((PdfBrush) new PdfSolidBrush((PdfColor) Color.FromArgb((int) byte.MaxValue - (int) color3.A, (int) color3.R, (int) color3.G, (int) color3.B)), path);
    }
  }

  private void FillTextureBackground(PdfPath path, TextureFill textureFill)
  {
    if (textureFill.Data == null)
      return;
    MemoryStream memoryStream = new MemoryStream(textureFill.Data);
    if (Image.FromStream((Stream) new MemoryStream(textureFill.Data)) is Metafile && Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.IsAzureCompatible)
      return;
    Image image = (Image) new Bitmap((Stream) memoryStream);
    this.ApplyImageTransparency(new ImageAttributes(), (float) textureFill.ObtainTransparency() / 100000f);
    RectangleF rectangleF = path.GetBounds();
    if (textureFill.DuoTone.Count == 2)
      image = this.ApplyDuoTone(image, textureFill.GetDefaultDuoTone());
    if (this.IsNeedToBeDrawn(rectangleF, textureFill))
    {
      PicFormatOption picFormatOption = textureFill.PicFormatOption;
      if (picFormatOption.Left != 0.0 || picFormatOption.Top != 0.0 || picFormatOption.Right != 0.0 || picFormatOption.Bottom != 0.0)
        rectangleF = this.CropPosition(picFormatOption, rectangleF);
      this._pdfGraphics.SetTransparency((float) textureFill.ObtainTransparency() / 100000f);
      PdfImage pdfImage = this.GetPdfImage(image);
      if (pdfImage != null)
        this._pdfGraphics.DrawImage(pdfImage, rectangleF);
      this._pdfGraphics.ResetTransform();
    }
    else
    {
      PdfBrush textureBrush = this.GetTextureBrush(image, (float) textureFill.ObtainTransparency() / 100000f, textureFill, rectangleF, path);
      if (textureBrush != null)
        this._pdfGraphics.DrawPath(textureBrush, path);
    }
    image.Dispose();
    memoryStream.Dispose();
  }

  private void DrawCellBorders(
    PdfPen leftPen,
    PdfPen topPen,
    PdfPen rightPen,
    PdfPen bottomPen,
    PdfPen diagonalUp,
    PdfPen diagonalDown,
    RectangleF cellBounds)
  {
    if (leftPen != null && leftPen.Color.A > (byte) 0)
      this._pdfGraphics.DrawLine(leftPen, cellBounds.X, cellBounds.Y - ((double) leftPen.Width <= (double) topPen.Width ? topPen.Width / 2f : 0.0f), cellBounds.X, cellBounds.Bottom + ((double) leftPen.Width <= (double) bottomPen.Width ? bottomPen.Width / 2f : 0.0f));
    if (topPen != null && topPen.Color.A > (byte) 0)
      this._pdfGraphics.DrawLine(topPen, cellBounds.X - (leftPen == null || (double) topPen.Width > (double) leftPen.Width ? 0.0f : leftPen.Width / 2f), cellBounds.Y, cellBounds.Right + ((double) topPen.Width <= (double) rightPen.Width ? rightPen.Width / 2f : 0.0f), cellBounds.Y);
    if (rightPen != null && rightPen.Color.A > (byte) 0)
      this._pdfGraphics.DrawLine(rightPen, cellBounds.Right, cellBounds.Y - (topPen == null || (double) rightPen.Width > (double) topPen.Width ? 0.0f : topPen.Width / 2f), cellBounds.Right, cellBounds.Bottom + ((double) rightPen.Width <= (double) bottomPen.Width ? bottomPen.Width / 2f : 0.0f));
    if (bottomPen != null && bottomPen.Color.A > (byte) 0)
      this._pdfGraphics.DrawLine(bottomPen, cellBounds.X - (leftPen == null || (double) bottomPen.Width > (double) leftPen.Width ? 0.0f : leftPen.Width / 2f), cellBounds.Bottom, cellBounds.Right + (rightPen == null || (double) bottomPen.Width > (double) rightPen.Width ? 0.0f : rightPen.Width / 2f), cellBounds.Bottom);
    if (diagonalUp != null && diagonalUp.Color.A > (byte) 0)
      this._pdfGraphics.DrawLine(diagonalUp, cellBounds.Left, cellBounds.Bottom, cellBounds.Right, cellBounds.Top);
    if (diagonalDown == null || diagonalDown.Color.A <= (byte) 0)
      return;
    this._pdfGraphics.DrawLine(diagonalDown, cellBounds.Left, cellBounds.Top, cellBounds.Right, cellBounds.Bottom);
  }

  private PdfPen CreatePen(IShape shape, ILineFormat lineFormat)
  {
    Color color1 = Color.Empty;
    if (lineFormat == null)
      return (PdfPen) null;
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
    PdfPen pen = new PdfPen(color1);
    if ((lineFormat.Fill.FillType == FillType.Automatic || lineFormat.Fill.FillType == FillType.Gradient) && ((Shape) shape).DrawingType == DrawingType.TextBox && lineFormat.Fill.FillType != FillType.Solid || lineFormat.Fill.FillType == FillType.None)
      pen = new PdfPen(new PdfColor(Color.Transparent));
    switch (((LineFormat) lineFormat).GetDefaultCapStyle())
    {
      case LineCapStyle.Round:
        pen.LineCap = PdfLineCap.Round;
        break;
      case LineCapStyle.Square:
        pen.LineCap = PdfLineCap.Square;
        break;
      case LineCapStyle.Flat:
        pen.LineCap = PdfLineCap.Flat;
        break;
    }
    switch (((LineFormat) lineFormat).GetDefaultLineJoinType())
    {
      case LineJoinType.Miter:
        pen.LineJoin = PdfLineJoin.Miter;
        break;
      case LineJoinType.Round:
        pen.LineJoin = PdfLineJoin.Round;
        break;
      case LineJoinType.Bevel:
        pen.LineJoin = PdfLineJoin.Bevel;
        break;
    }
    switch (lineFormat.DashStyle)
    {
      case LineDashStyle.Solid:
        pen.DashStyle = PdfDashStyle.Solid;
        break;
      case LineDashStyle.Dash:
        pen.DashStyle = PdfDashStyle.Dash;
        break;
      case LineDashStyle.DashDot:
        pen.DashStyle = PdfDashStyle.DashDot;
        break;
      case LineDashStyle.RoundDot:
        pen.DashStyle = PdfDashStyle.Dot;
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
    PdfCustomLineCap customLineCap1 = this.GetCustomLineCap(lineFormat.BeginArrowheadStyle, lineFormat.BeginArrowheadLength, lineFormat.BeginArrowheadWidth);
    PdfCustomLineCap customLineCap2 = this.GetCustomLineCap(lineFormat.EndArrowheadStyle, lineFormat.EndArrowheadLength, lineFormat.EndArrowheadWidth);
    if (customLineCap1 != null)
    {
      pen.StartCap = PdfLineCap.Flat;
      pen.CustomStartCap = customLineCap1;
    }
    if (customLineCap2 != null)
    {
      pen.EndCap = PdfLineCap.Flat;
      pen.CustomEndCap = customLineCap2;
    }
    if (((LineFormat) lineFormat).GetDefaultWidth() > 0.0)
      pen.Width = (float) ((LineFormat) lineFormat).GetDefaultWidth();
    return pen;
  }

  private PdfCustomLineCap GetCustomLineCap(
    ArrowheadStyle arrowheadStyle,
    ArrowheadLength arrowheadLength,
    ArrowheadWidth arrowheadWidth)
  {
    float baseInset;
    PdfPath lineGapGraphicsPath = this.GetCustomLineGapGraphicsPath(arrowheadStyle, arrowheadLength, arrowheadWidth, out baseInset);
    if (lineGapGraphicsPath == null)
      return (PdfCustomLineCap) null;
    PdfCustomLineCap customLineCap;
    if (arrowheadStyle == ArrowheadStyle.ArrowOpen)
    {
      customLineCap = new PdfCustomLineCap((PdfPath) null, lineGapGraphicsPath, PdfLineCap.Round, baseInset);
      customLineCap.SetStrokeCaps(PdfLineCap.Round, PdfLineCap.Round);
    }
    else
      customLineCap = new PdfCustomLineCap(lineGapGraphicsPath, (PdfPath) null, PdfLineCap.Flat, baseInset);
    return customLineCap;
  }

  private PdfPath GetCustomLineGapGraphicsPath(
    ArrowheadStyle arrowheadStyle,
    ArrowheadLength arrowheadLength,
    ArrowheadWidth arrowheadWidth,
    out float baseInset)
  {
    baseInset = 0.0f;
    if (arrowheadStyle == ArrowheadStyle.None)
      return (PdfPath) null;
    PdfPath graphicsPath = new PdfPath((PdfBrush) null, PdfFillMode.Winding);
    float styleValue;
    if (this.GetArrowheadStyleValue(arrowheadStyle, graphicsPath, out styleValue))
      return (PdfPath) null;
    float num1 = (float) ((sbyte) 2 + arrowheadLength);
    if (arrowheadLength == ArrowheadLength.Long)
      ++num1;
    float num2 = (float) ((sbyte) 2 + arrowheadWidth);
    if (arrowheadWidth == ArrowheadWidth.Wide)
      ++num2;
    baseInset = num1 * styleValue;
    new Matrix().Scale(num2 / 10f, num1 / 10f);
    return graphicsPath;
  }

  private bool GetArrowheadStyleValue(
    ArrowheadStyle arrowheadStyle,
    PdfPath graphicsPath,
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

  private void DrawArrowHead(
    Shape shapeImpl,
    PdfPen pen,
    RectangleF bounds,
    ref bool isArrowHeadExist,
    ref PdfPath path,
    PointF[] linePoints)
  {
    isArrowHeadExist = false;
    PointF endPoint = new PointF(0.0f, 0.0f);
    switch (shapeImpl.LineFormat.EndArrowheadStyle)
    {
      case ArrowheadStyle.Arrow:
        this.DrawCloseEndArrowHead(shapeImpl, pen, bounds, linePoints, ref endPoint, ref isArrowHeadExist, ref path);
        break;
      case ArrowheadStyle.ArrowOpen:
        this.DrawOpenEndArrowHead(shapeImpl, pen, bounds, linePoints, ref endPoint, ref isArrowHeadExist, ref path);
        break;
    }
    switch (shapeImpl.LineFormat.BeginArrowheadStyle)
    {
      case ArrowheadStyle.Arrow:
        this.DrawCloseBeginArrowHead(shapeImpl, pen, bounds, linePoints, ref endPoint, ref isArrowHeadExist, ref path);
        break;
      case ArrowheadStyle.ArrowOpen:
        this.DrawOpenBeginArrowHead(shapeImpl, pen, bounds, linePoints, ref endPoint, ref isArrowHeadExist, ref path);
        break;
    }
  }

  private void DrawCloseBeginArrowHead(
    Shape shape,
    PdfPen pen,
    RectangleF bounds,
    PointF[] linePoints,
    ref PointF endPoint,
    ref bool isArrowHeadExist,
    ref PdfPath path)
  {
    PointF[] arrowHeadPoints = this.FindArrowHeadPoints(shape, pen, bounds, linePoints, false, true);
    path.StartFigure();
    if ((double) endPoint.X == 0.0 && (double) endPoint.Y == 0.0)
      path.AddLine(linePoints[1].X, linePoints[1].Y, arrowHeadPoints[0].X, arrowHeadPoints[0].Y);
    else
      path.AddLine(endPoint.X, endPoint.Y, arrowHeadPoints[0].X, arrowHeadPoints[0].Y);
    this.AddCloseArrowHeadPoints(arrowHeadPoints, ref path, pen, shape.ShapeFrame);
    isArrowHeadExist = true;
  }

  private void DrawOpenBeginArrowHead(
    Shape shape,
    PdfPen pen,
    RectangleF bounds,
    PointF[] linePoints,
    ref PointF endPoint,
    ref bool isArrowHeadExist,
    ref PdfPath path)
  {
    PointF[] arrowHeadPoints = this.FindArrowHeadPoints(shape, pen, bounds, linePoints, true, true);
    path.StartFigure();
    if ((double) endPoint.X == 0.0 && (double) endPoint.Y == 0.0)
      path.AddLine(linePoints[1].X, linePoints[1].Y, arrowHeadPoints[0].X, arrowHeadPoints[0].Y);
    else
      path.AddLine(endPoint.X, endPoint.Y, arrowHeadPoints[0].X, arrowHeadPoints[0].Y);
    this.AddOpenArrowHeadPoints(arrowHeadPoints, ref path);
    isArrowHeadExist = true;
  }

  private void DrawCloseEndArrowHead(
    Shape shape,
    PdfPen pen,
    RectangleF bounds,
    PointF[] linePoints,
    ref PointF endPoint,
    ref bool isArrowHeadExist,
    ref PdfPath path)
  {
    PointF[] arrowHeadPoints = this.FindArrowHeadPoints(shape, pen, bounds, linePoints, false, false);
    if (shape.LineFormat.BeginArrowheadStyle != ArrowheadStyle.Arrow && shape.LineFormat.BeginArrowheadStyle != ArrowheadStyle.ArrowOpen)
      path.AddLine(linePoints[0].X, linePoints[0].Y, arrowHeadPoints[0].X, arrowHeadPoints[0].Y);
    else
      endPoint = arrowHeadPoints[0];
    this.AddCloseArrowHeadPoints(arrowHeadPoints, ref path, pen, shape.ShapeFrame);
    isArrowHeadExist = true;
  }

  private void AddCloseArrowHeadPoints(
    PointF[] points,
    ref PdfPath path,
    PdfPen pen,
    ShapeFrame shapeFrame)
  {
    PointF[] points1 = new PointF[3]
    {
      points[1],
      points[2],
      points[3]
    };
    path.AddPolygon(points1);
    path.CloseFigure();
    float num = (float) (shapeFrame.GetDefaultRotation() / 60000);
    if ((double) num > 360.0)
      num %= 360f;
    bool flipVertical = shapeFrame.FlipVertical;
    bool flipHorizontal = shapeFrame.FlipHorizontal;
    if ((double) num != 0.0 || flipVertical || flipHorizontal)
      return;
    this.PDFGraphics.DrawPolygon((PdfBrush) new PdfSolidBrush(pen.Color), points1);
  }

  private void DrawOpenEndArrowHead(
    Shape shape,
    PdfPen pen,
    RectangleF bounds,
    PointF[] linePoints,
    ref PointF endPoint,
    ref bool isArrowHeadExist,
    ref PdfPath path)
  {
    PointF[] arrowHeadPoints = this.FindArrowHeadPoints(shape, pen, bounds, linePoints, true, false);
    if (shape.LineFormat.BeginArrowheadStyle != ArrowheadStyle.Arrow && shape.LineFormat.BeginArrowheadStyle != ArrowheadStyle.ArrowOpen)
      path.AddLine(linePoints[0].X, linePoints[0].Y, arrowHeadPoints[0].X, arrowHeadPoints[0].Y);
    else
      endPoint = arrowHeadPoints[0];
    this.AddOpenArrowHeadPoints(arrowHeadPoints, ref path);
    isArrowHeadExist = true;
  }

  private void AddOpenArrowHeadPoints(PointF[] points, ref PdfPath path)
  {
    path.AddLine(points[1], points[2]);
    path.AddLine(points[2], points[3]);
  }

  private PointF[] FindArrowHeadPoints(
    Shape shape,
    PdfPen pen,
    RectangleF bounds,
    PointF[] linePoints,
    bool isFromOpenArrow,
    bool isFromBeginArrow)
  {
    PointF[] points = new PointF[4];
    float arrowLength = 0.0f;
    float arrowAngle = 0.0f;
    float adjustValue = 0.0f;
    this.GetArrowDefaultValues(shape, pen, ref arrowLength, ref arrowAngle, ref adjustValue, isFromOpenArrow, isFromBeginArrow);
    points[0] = this.FindBaseLineEndPoint(shape, linePoints, adjustValue, isFromBeginArrow);
    this.FindLeftRightHeadPoints(shape, linePoints, ref points, arrowAngle, arrowLength, isFromBeginArrow);
    return points;
  }

  private void GetArrowDefaultValues(
    Shape shape,
    PdfPen pen,
    ref float arrowLength,
    ref float arrowAngle,
    ref float adjustValue,
    bool isFromOpenArrow,
    bool isFromBeginArrow)
  {
    if (isFromOpenArrow)
      this.GetOpenArrowDefaultValues(shape, pen.Width, ref arrowLength, ref arrowAngle, ref adjustValue, isFromBeginArrow);
    else
      this.GetCloseArrowDefaultValues(shape, pen.Width, ref arrowLength, ref arrowAngle, ref adjustValue, isFromBeginArrow);
  }

  private void GetOpenArrowDefaultValues(
    Shape shape,
    float lineWidth,
    ref float arrowLength,
    ref float arrowAngle,
    ref float adjustValue,
    bool isFromBeginArrow)
  {
    ArrowheadWidth arrowheadWidth = shape.LineFormat.EndArrowheadWidth;
    ArrowheadLength endArrowheadLength = shape.LineFormat.EndArrowheadLength;
    if (shape.LineFormat.EndArrowheadWidth == ArrowheadWidth.None && (shape.LineFormat.EndArrowheadStyle == ArrowheadStyle.Arrow || shape.LineFormat.EndArrowheadStyle == ArrowheadStyle.ArrowOpen))
      arrowheadWidth = ArrowheadWidth.Medium;
    if (isFromBeginArrow)
    {
      arrowheadWidth = shape.LineFormat.BeginArrowheadWidth;
      if (shape.LineFormat.BeginArrowheadWidth == ArrowheadWidth.None && (shape.LineFormat.BeginArrowheadStyle == ArrowheadStyle.Arrow || shape.LineFormat.BeginArrowheadStyle == ArrowheadStyle.ArrowOpen))
        arrowheadWidth = ArrowheadWidth.Medium;
    }
    switch (arrowheadWidth)
    {
      case ArrowheadWidth.Narrow:
        endArrowheadLength = shape.LineFormat.EndArrowheadLength;
        this.GetOpenNarrowArrowDefaultValues(this.GetArrowHeadLength(shape, isFromBeginArrow), lineWidth, ref arrowLength, ref arrowAngle, ref adjustValue);
        break;
      case ArrowheadWidth.Medium:
        this.GetOpenMediumArrowDefaultValues(this.GetArrowHeadLength(shape, isFromBeginArrow), lineWidth, ref arrowLength, ref arrowAngle, ref adjustValue);
        break;
      case ArrowheadWidth.Wide:
        this.GetOpenWideArrowDefaultValues(this.GetArrowHeadLength(shape, isFromBeginArrow), lineWidth, ref arrowLength, ref arrowAngle, ref adjustValue);
        break;
    }
  }

  private void GetOpenNarrowArrowDefaultValues(
    ArrowheadLength arrowHeadLength,
    float lineWidth,
    ref float arrowLength,
    ref float arrowAngle,
    ref float adjustValue)
  {
    switch (arrowHeadLength)
    {
      case ArrowheadLength.Short:
        arrowLength = (double) lineWidth > 1.0 ? lineWidth * 2.8f : 5f;
        arrowAngle = 32f;
        adjustValue = lineWidth * 0.9f;
        break;
      case ArrowheadLength.Medium:
        arrowLength = (double) lineWidth > 1.0 ? (float) Math.Round((double) lineWidth * 3.5) : 7f;
        arrowAngle = 22f;
        adjustValue = lineWidth * 1.3f;
        break;
      case ArrowheadLength.Long:
        arrowLength = (double) lineWidth > 1.0 ? (float) Math.Round((double) lineWidth * 5.0) : 9.5f;
        arrowAngle = 15.5f;
        adjustValue = lineWidth * 1.83f;
        break;
    }
  }

  private void GetOpenMediumArrowDefaultValues(
    ArrowheadLength arrowHeadLength,
    float lineWidth,
    ref float arrowLength,
    ref float arrowAngle,
    ref float adjustValue)
  {
    switch (arrowHeadLength)
    {
      case ArrowheadLength.Short:
        arrowLength = (double) lineWidth > 1.0 ? lineWidth * 3f : 5.5f;
        arrowAngle = 41f;
        adjustValue = lineWidth * 0.75f;
        break;
      case ArrowheadLength.Medium:
        arrowLength = (double) lineWidth > 1.0 ? (float) Math.Round((double) lineWidth * 3.8) : 7f;
        arrowAngle = 30f;
        adjustValue = lineWidth;
        break;
      case ArrowheadLength.Long:
        arrowLength = (double) lineWidth > 1.0 ? (float) Math.Round((double) lineWidth * 5.0) : 10f;
        arrowAngle = 21f;
        adjustValue = lineWidth * 1.35f;
        break;
    }
  }

  private void GetOpenWideArrowDefaultValues(
    ArrowheadLength arrowHeadLength,
    float lineWidth,
    ref float arrowLength,
    ref float arrowAngle,
    ref float adjustValue)
  {
    switch (arrowHeadLength)
    {
      case ArrowheadLength.Short:
        arrowLength = (double) lineWidth > 1.0 ? lineWidth * 3.7f : 6.5f;
        arrowAngle = 52f;
        adjustValue = lineWidth * 0.65f;
        break;
      case ArrowheadLength.Medium:
        arrowLength = (double) lineWidth > 1.0 ? (float) Math.Round((double) lineWidth * 4.2) : 8f;
        arrowAngle = 40f;
        adjustValue = lineWidth;
        break;
      case ArrowheadLength.Long:
        arrowLength = (double) lineWidth > 1.0 ? (float) Math.Round((double) lineWidth * 5.6999998092651367) : 10.5f;
        arrowAngle = 29f;
        adjustValue = lineWidth;
        break;
    }
  }

  private void GetCloseArrowDefaultValues(
    Shape shape,
    float lineWidth,
    ref float arrowLength,
    ref float arrowAngle,
    ref float adjustValue,
    bool isFromBeginArrow)
  {
    ArrowheadWidth arrowheadWidth = shape.LineFormat.EndArrowheadWidth;
    ArrowheadLength endArrowheadLength = shape.LineFormat.EndArrowheadLength;
    if (shape.LineFormat.EndArrowheadWidth == ArrowheadWidth.None && (shape.LineFormat.EndArrowheadStyle == ArrowheadStyle.Arrow || shape.LineFormat.EndArrowheadStyle == ArrowheadStyle.ArrowOpen))
      arrowheadWidth = ArrowheadWidth.Medium;
    if (isFromBeginArrow)
    {
      arrowheadWidth = shape.LineFormat.BeginArrowheadWidth;
      if (shape.LineFormat.BeginArrowheadWidth == ArrowheadWidth.None && (shape.LineFormat.BeginArrowheadStyle == ArrowheadStyle.Arrow || shape.LineFormat.BeginArrowheadStyle == ArrowheadStyle.ArrowOpen))
        arrowheadWidth = ArrowheadWidth.Medium;
    }
    switch (arrowheadWidth)
    {
      case ArrowheadWidth.Narrow:
        endArrowheadLength = shape.LineFormat.EndArrowheadLength;
        this.GetCloseNarrowArrowDefaultValues(this.GetArrowHeadLength(shape, isFromBeginArrow), lineWidth, ref arrowLength, ref arrowAngle, ref adjustValue);
        break;
      case ArrowheadWidth.Medium:
        this.GetCloseMediumArrowDefaultValues(this.GetArrowHeadLength(shape, isFromBeginArrow), lineWidth, ref arrowLength, ref arrowAngle, ref adjustValue);
        break;
      case ArrowheadWidth.Wide:
        this.GetCloseWideArrowDefaultValues(this.GetArrowHeadLength(shape, isFromBeginArrow), lineWidth, ref arrowLength, ref arrowAngle, ref adjustValue);
        break;
    }
  }

  private void GetCloseNarrowArrowDefaultValues(
    ArrowheadLength arrowHeadLength,
    float lineWidth,
    ref float arrowLength,
    ref float arrowAngle,
    ref float adjustValue)
  {
    switch (arrowHeadLength)
    {
      case ArrowheadLength.Short:
        arrowLength = (double) lineWidth > 1.0 ? lineWidth * 0.37f : 2.7f;
        arrowAngle = 26f;
        adjustValue = lineWidth * 1.15f;
        break;
      case ArrowheadLength.Medium:
        arrowLength = (double) lineWidth > 1.0 ? (float) Math.Round((double) lineWidth * 0.97000002861022949) : 4.2f;
        arrowAngle = 18.5f;
        adjustValue = lineWidth * 1.59f;
        break;
      case ArrowheadLength.Long:
        arrowLength = (double) lineWidth > 1.0 ? (float) Math.Round((double) lineWidth * 2.0499999523162842) : 9f;
        arrowAngle = 11.3f;
        adjustValue = lineWidth * 2.52f;
        break;
    }
  }

  private void GetCloseMediumArrowDefaultValues(
    ArrowheadLength arrowHeadLength,
    float lineWidth,
    ref float arrowLength,
    ref float arrowAngle,
    ref float adjustValue)
  {
    switch (arrowHeadLength)
    {
      case ArrowheadLength.Short:
        arrowLength = (double) lineWidth > 1.0 ? lineWidth * 0.845f : 3.5f;
        arrowAngle = 37f;
        adjustValue = lineWidth * 0.83f;
        break;
      case ArrowheadLength.Medium:
        arrowLength = (double) lineWidth > 1.0 ? lineWidth * 1.5f : 5f;
        arrowAngle = 26.5f;
        adjustValue = lineWidth * 1.15f;
        break;
      case ArrowheadLength.Long:
        arrowLength = (double) lineWidth > 1.0 ? (float) Math.Round((double) lineWidth * 2.869999885559082) : 8f;
        arrowAngle = 16.65f;
        adjustValue = lineWidth * 1.75f;
        break;
    }
  }

  private void GetCloseWideArrowDefaultValues(
    ArrowheadLength arrowHeadLength,
    float lineWidth,
    ref float arrowLength,
    ref float arrowAngle,
    ref float adjustValue)
  {
    switch (arrowHeadLength)
    {
      case ArrowheadLength.Short:
        arrowLength = (double) lineWidth > 1.0 ? lineWidth * 1.36f : 4.5f;
        arrowAngle = 51.5f;
        adjustValue = lineWidth * 0.65f;
        break;
      case ArrowheadLength.Medium:
        arrowLength = (double) lineWidth > 1.0 ? lineWidth * 2.24f : 6.2f;
        arrowAngle = 39.7f;
        adjustValue = lineWidth * 0.78f;
        break;
      case ArrowheadLength.Long:
        arrowLength = (double) lineWidth > 1.0 ? (float) Math.Round((double) lineWidth * 3.7799999713897705) : 9.45f;
        arrowAngle = 26.55f;
        adjustValue = lineWidth * 1.13f;
        break;
    }
  }

  private PointF FindBaseLineEndPoint(
    Shape shape,
    PointF[] linePoints,
    float adjustValue,
    bool isFromBeginArrow)
  {
    float x = 0.0f;
    float y = 0.0f;
    double degree = this.RadianToDegree(this.FindAngleRadians(linePoints, true));
    this.GetEndPointForBaseLine(isFromBeginArrow, degree, Math.Sqrt(shape.Width * shape.Width + shape.Height * shape.Height), adjustValue, linePoints, ref x, ref y);
    return new PointF(x, y);
  }

  private double RadianToDegree(double angle) => angle * (180.0 / Math.PI);

  private double Degree2Radian(double a) => a * (Math.PI / 180.0);

  private void GetEndPointForBaseLine(
    bool isFromBeginArrow,
    double degree,
    double length,
    float adjustValue,
    PointF[] linePoints,
    ref float x,
    ref float y)
  {
    if (isFromBeginArrow)
    {
      degree -= 180.0;
      this.GetEndPoint(this.Degree2Radian(degree), (float) length - adjustValue, linePoints[1].X, linePoints[1].Y, ref x, ref y);
    }
    else
      this.GetEndPoint(this.Degree2Radian(degree), (float) length - adjustValue, linePoints[0].X, linePoints[0].Y, ref x, ref y);
  }

  private void GetEndPoint(
    double angle,
    float len,
    float start_x,
    float start_y,
    ref float end_x,
    ref float end_y)
  {
    end_x = start_x + len * (float) Math.Cos(angle);
    end_y = start_y + len * (float) Math.Sin(angle);
  }

  private double FindAngleRadians(PointF[] linePoints, bool isFromBottomToTop)
  {
    PointF linePoint1 = linePoints[0];
    PointF pointF1 = new PointF(linePoints[1].X, isFromBottomToTop ? linePoints[1].Y : linePoints[0].Y);
    PointF linePoint2 = linePoints[0];
    PointF pointF2 = new PointF(linePoints[1].X, isFromBottomToTop ? linePoints[0].Y : linePoints[1].Y);
    return Math.Atan2((double) pointF1.Y - (double) linePoint1.Y, (double) pointF1.X - (double) linePoint1.X) - Math.Atan2((double) pointF2.Y - (double) linePoint2.Y, (double) pointF2.X - (double) linePoint2.X);
  }

  private void FindLeftRightHeadPoints(
    Shape shape,
    PointF[] linePoints,
    ref PointF[] points,
    float arrowAngle,
    float arrowLength,
    bool isFromBeginArrow)
  {
    PointF point1 = new PointF();
    PointF point2 = new PointF();
    this.ConstrucBasetLine(isFromBeginArrow, points[0], linePoints, ref point1, ref point2);
    double andRightHeadPoint = this.FindAngleToLeftAndRightHeadPoint(shape, point1, point2, isFromBeginArrow);
    float end_x = 0.0f;
    float end_y = 0.0f;
    this.GetEndPoint(this.Degree2Radian(andRightHeadPoint - (double) arrowAngle), arrowLength, point2.X, point2.Y, ref end_x, ref end_y);
    points[1] = new PointF(end_x, end_y);
    points[2] = new PointF(point2.X, point2.Y);
    this.GetEndPoint(this.Degree2Radian(andRightHeadPoint + (double) arrowAngle), arrowLength, point2.X, point2.Y, ref end_x, ref end_y);
    points[3] = new PointF(end_x, end_y);
  }

  private void ConstrucBasetLine(
    bool isFromBeginArrow,
    PointF points,
    PointF[] linePoints,
    ref PointF point1,
    ref PointF point2)
  {
    if (isFromBeginArrow)
    {
      point1 = new PointF(linePoints[1].X, linePoints[1].Y);
      point2 = points;
    }
    else
    {
      point1 = new PointF(linePoints[0].X, linePoints[0].Y);
      point2 = points;
    }
  }

  private double FindAngleToLeftAndRightHeadPoint(
    Shape shape,
    PointF point1,
    PointF point2,
    bool isFromBeginArrow)
  {
    double andRightHeadPoint = 360.0 - this.RadianToDegree(this.FindArrowHeadAngleRadians(point1, point2, true));
    if (isFromBeginArrow && shape.Width != 0.0)
      andRightHeadPoint -= 180.0;
    return andRightHeadPoint;
  }

  private double FindArrowHeadAngleRadians(
    PointF point1,
    PointF point2,
    bool isFromSeparateOrientation)
  {
    PointF pointF1 = new PointF(isFromSeparateOrientation ? point1.X : 0.0f, point2.Y);
    PointF pointF2 = point2;
    PointF pointF3 = point2;
    PointF pointF4 = point1;
    return Math.Atan2((double) pointF2.Y - (double) pointF1.Y, (double) pointF2.X - (double) pointF1.X) - Math.Atan2((double) pointF4.Y - (double) pointF3.Y, (double) pointF4.X - (double) pointF3.X);
  }

  private ArrowheadLength GetArrowHeadLength(Shape shape, bool isFromBeginArrow)
  {
    ArrowheadLength arrowHeadLength = shape.LineFormat.EndArrowheadLength;
    if (shape.LineFormat.EndArrowheadLength == ArrowheadLength.None && (shape.LineFormat.EndArrowheadStyle == ArrowheadStyle.Arrow || shape.LineFormat.EndArrowheadStyle == ArrowheadStyle.ArrowOpen))
      arrowHeadLength = ArrowheadLength.Medium;
    if (isFromBeginArrow)
    {
      arrowHeadLength = shape.LineFormat.BeginArrowheadLength;
      if (shape.LineFormat.BeginArrowheadLength == ArrowheadLength.None && (shape.LineFormat.BeginArrowheadStyle == ArrowheadStyle.Arrow || shape.LineFormat.BeginArrowheadStyle == ArrowheadStyle.ArrowOpen))
        arrowHeadLength = ArrowheadLength.Medium;
    }
    return arrowHeadLength;
  }

  internal PdfPath[] GetGraphicsPath(
    RectangleF bounds,
    ref PdfPen pen,
    PdfGraphics gr,
    Shape shapeImpl)
  {
    PdfPath[] graphicsPath;
    if (shapeImpl.GetCustomGeometry())
    {
      graphicsPath = this.GetCustomGeomentryPath(bounds, shapeImpl);
      if (graphicsPath.Length == 0)
        graphicsPath = new PdfPath[1]{ new PdfPath() };
    }
    else
      graphicsPath = new PdfPath[1]
      {
        this.GetPresetGeomentryPath(bounds, ref pen, gr, shapeImpl)
      };
    return graphicsPath;
  }

  private PdfPath GetPresetGeomentryPath(
    RectangleF bounds,
    ref PdfPen pen,
    PdfGraphics gr,
    Shape shapeImpl)
  {
    this._pdfGraphics = gr;
    PDFShapePath pdfShapePath = new PDFShapePath(bounds, shapeImpl.ShapeGuide);
    PdfPath path1 = new PdfPath();
    switch (shapeImpl.GetAutoShapeType())
    {
      case AutoShapeType.Rectangle:
      case AutoShapeType.FlowChartProcess:
        path1.AddRectangle(bounds);
        return path1;
      case AutoShapeType.Parallelogram:
      case AutoShapeType.FlowChartData:
        return pdfShapePath.GetParallelogramPath();
      case AutoShapeType.Trapezoid:
        return pdfShapePath.GetTrapezoidPath();
      case AutoShapeType.Diamond:
      case AutoShapeType.FlowChartDecision:
        PointF[] linePoints1 = new PointF[4]
        {
          new PointF(bounds.X, bounds.Y + bounds.Height / 2f),
          new PointF(bounds.X + bounds.Width / 2f, bounds.Y),
          new PointF(bounds.Right, bounds.Y + bounds.Height / 2f),
          new PointF(bounds.X + bounds.Width / 2f, bounds.Bottom)
        };
        path1.AddLines(linePoints1);
        path1.CloseFigure();
        break;
      case AutoShapeType.RoundedRectangle:
        return pdfShapePath.GetRoundedRectanglePath();
      case AutoShapeType.Octagon:
        return pdfShapePath.GetOctagonPath();
      case AutoShapeType.IsoscelesTriangle:
        return pdfShapePath.GetTrianglePath();
      case AutoShapeType.RightTriangle:
        PointF[] linePoints2 = new PointF[3]
        {
          new PointF(bounds.X, bounds.Bottom),
          new PointF(bounds.X, bounds.Y),
          new PointF(bounds.Right, bounds.Bottom)
        };
        path1.AddLines(linePoints2);
        path1.CloseFigure();
        return path1;
      case AutoShapeType.Oval:
        path1.AddEllipse(bounds);
        return path1;
      case AutoShapeType.Hexagon:
        return pdfShapePath.GetHexagonPath();
      case AutoShapeType.Cross:
        return pdfShapePath.GetCrossPath();
      case AutoShapeType.RegularPentagon:
        return pdfShapePath.GetRegularPentagonPath();
      case AutoShapeType.Can:
        return pdfShapePath.GetCanPath();
      case AutoShapeType.Cube:
        return pdfShapePath.GetCubePath();
      case AutoShapeType.Bevel:
        return pdfShapePath.GetBevelPath();
      case AutoShapeType.FoldedCorner:
        return pdfShapePath.GetFoldedCornerPath();
      case AutoShapeType.SmileyFace:
        PdfPath[] smileyFacePath = pdfShapePath.GetSmileyFacePath();
        IFill defaultFillFormat1 = shapeImpl.GetDefaultFillFormat();
        IColor color1 = (IColor) null;
        if (defaultFillFormat1.FillType == FillType.Solid)
          color1 = defaultFillFormat1.SolidFill.Color;
        if (defaultFillFormat1.FillType == FillType.Gradient && shapeImpl.Fill.GradientFill.GradientStops.Count > 0)
          color1 = ((GradientStop) shapeImpl.Fill.GradientFill.GradientStops[shapeImpl.Fill.GradientFill.GradientStops.Count - 1]).Color;
        foreach (PdfPath path2 in smileyFacePath)
        {
          if (color1 != null && color1.SystemColor != Color.Empty)
            this.PDFGraphics.DrawPath((PdfBrush) new PdfSolidBrush((PdfColor) color1.SystemColor), path2);
          this.PDFGraphics.DrawPath(pen, path2);
        }
        if (shapeImpl is Picture)
        {
          path1 = smileyFacePath[0];
          break;
        }
        break;
      case AutoShapeType.Donut:
        return pdfShapePath.GetDonutPath();
      case AutoShapeType.NoSymbol:
        return pdfShapePath.GetNoSymbolPath();
      case AutoShapeType.BlockArc:
        return pdfShapePath.GetBlockArcPath();
      case AutoShapeType.Heart:
        return pdfShapePath.GetHeartPath();
      case AutoShapeType.LightningBolt:
        return pdfShapePath.GetLightningBoltPath();
      case AutoShapeType.Sun:
        return pdfShapePath.GetSunPath();
      case AutoShapeType.Moon:
        return pdfShapePath.GetMoonPath();
      case AutoShapeType.Arc:
        PdfPath[] arcPath = pdfShapePath.GetArcPath();
        IFill defaultFillFormat2 = shapeImpl.GetDefaultFillFormat();
        IColor color2 = (IColor) null;
        if (defaultFillFormat2.FillType == FillType.Solid)
          color2 = defaultFillFormat2.SolidFill.Color;
        if (defaultFillFormat2.FillType == FillType.Gradient && shapeImpl.Fill.GradientFill.GradientStops.Count > 0)
          color2 = ((GradientStop) shapeImpl.Fill.GradientFill.GradientStops[shapeImpl.Fill.GradientFill.GradientStops.Count - 1]).Color;
        if (color2 != null && color2.SystemColor != Color.Empty)
          this.PDFGraphics.DrawPath((PdfBrush) new PdfSolidBrush((PdfColor) color2.SystemColor), arcPath[1]);
        this.PDFGraphics.DrawPath(pen, arcPath[0]);
        if (shapeImpl is Picture)
        {
          path1 = arcPath[1];
          break;
        }
        break;
      case AutoShapeType.DoubleBracket:
        return pdfShapePath.GetDoubleBracketPath();
      case AutoShapeType.DoubleBrace:
        return pdfShapePath.GetDoubleBracePath();
      case AutoShapeType.Plaque:
        return pdfShapePath.GetPlaquePath();
      case AutoShapeType.LeftBracket:
        return pdfShapePath.GetLeftBracketPath();
      case AutoShapeType.RightBracket:
        return pdfShapePath.GetRightBracketPath();
      case AutoShapeType.LeftBrace:
        return pdfShapePath.GetLeftBracePath();
      case AutoShapeType.RightBrace:
        return pdfShapePath.GetRightBracePath();
      case AutoShapeType.RightArrow:
        return pdfShapePath.GetRightArrowPath();
      case AutoShapeType.LeftArrow:
        return pdfShapePath.GetLeftArrowPath();
      case AutoShapeType.UpArrow:
        return pdfShapePath.GetUpArrowPath();
      case AutoShapeType.DownArrow:
        return pdfShapePath.GetDownArrowPath();
      case AutoShapeType.LeftRightArrow:
        return pdfShapePath.GetLeftRightArrowPath();
      case AutoShapeType.UpDownArrow:
        return pdfShapePath.GetUpDownArrowPath();
      case AutoShapeType.QuadArrow:
        return pdfShapePath.GetQuadArrowPath();
      case AutoShapeType.LeftRightUpArrow:
        return pdfShapePath.GetLeftRightUpArrowPath();
      case AutoShapeType.BentArrow:
        return pdfShapePath.GetBentArrowPath();
      case AutoShapeType.UTurnArrow:
        return pdfShapePath.GetUTrunArrowPath();
      case AutoShapeType.LeftUpArrow:
        return pdfShapePath.GetLeftUpArrowPath();
      case AutoShapeType.BentUpArrow:
        return pdfShapePath.GetBentUpArrowPath();
      case AutoShapeType.CurvedRightArrow:
        return pdfShapePath.GetCurvedRightArrowPath();
      case AutoShapeType.CurvedLeftArrow:
        return pdfShapePath.GetCurvedLeftArrowPath();
      case AutoShapeType.CurvedUpArrow:
        return pdfShapePath.GetCurvedUpArrowPath();
      case AutoShapeType.CurvedDownArrow:
        return pdfShapePath.GetCurvedDownArrowPath();
      case AutoShapeType.StripedRightArrow:
        return pdfShapePath.GetStripedRightArrowPath();
      case AutoShapeType.NotchedRightArrow:
        return pdfShapePath.GetNotchedRightArrowPath();
      case AutoShapeType.Pentagon:
        return pdfShapePath.GetPentagonPath();
      case AutoShapeType.Chevron:
        return pdfShapePath.GetChevronPath();
      case AutoShapeType.RightArrowCallout:
        return pdfShapePath.GetRightArrowCalloutPath();
      case AutoShapeType.LeftArrowCallout:
        return pdfShapePath.GetLeftArrowCalloutPath();
      case AutoShapeType.UpArrowCallout:
        return pdfShapePath.GetUpArrowCalloutPath();
      case AutoShapeType.DownArrowCallout:
        return pdfShapePath.GetDownArrowCalloutPath();
      case AutoShapeType.LeftRightArrowCallout:
        return pdfShapePath.GetLeftRightArrowCalloutPath();
      case AutoShapeType.QuadArrowCallout:
        return pdfShapePath.GetQuadArrowCalloutPath();
      case AutoShapeType.CircularArrow:
        return pdfShapePath.GetCircularArrowPath();
      case AutoShapeType.FlowChartAlternateProcess:
        return pdfShapePath.GetFlowChartAlternateProcessPath();
      case AutoShapeType.FlowChartPredefinedProcess:
        return pdfShapePath.GetFlowChartPredefinedProcessPath();
      case AutoShapeType.FlowChartInternalStorage:
        return pdfShapePath.GetFlowChartInternalStoragePath();
      case AutoShapeType.FlowChartDocument:
        return pdfShapePath.GetFlowChartDocumentPath();
      case AutoShapeType.FlowChartMultiDocument:
        return pdfShapePath.GetFlowChartMultiDocumentPath();
      case AutoShapeType.FlowChartTerminator:
        return pdfShapePath.GetFlowChartTerminatorPath();
      case AutoShapeType.FlowChartPreparation:
        return pdfShapePath.GetFlowChartPreparationPath();
      case AutoShapeType.FlowChartManualInput:
        return pdfShapePath.GetFlowChartManualInputPath();
      case AutoShapeType.FlowChartManualOperation:
        return pdfShapePath.GetFlowChartManualOperationPath();
      case AutoShapeType.FlowChartConnector:
        return pdfShapePath.GetFlowChartConnectorPath();
      case AutoShapeType.FlowChartOffPageConnector:
        return pdfShapePath.GetFlowChartOffPageConnectorPath();
      case AutoShapeType.FlowChartCard:
        return pdfShapePath.GetFlowChartCardPath();
      case AutoShapeType.FlowChartPunchedTape:
        return pdfShapePath.GetFlowChartPunchedTapePath();
      case AutoShapeType.FlowChartSummingJunction:
        return pdfShapePath.GetFlowChartSummingJunctionPath();
      case AutoShapeType.FlowChartOr:
        return pdfShapePath.GetFlowChartOrPath();
      case AutoShapeType.FlowChartCollate:
        return pdfShapePath.GetFlowChartCollatePath();
      case AutoShapeType.FlowChartSort:
        return pdfShapePath.GetFlowChartSortPath();
      case AutoShapeType.FlowChartExtract:
        return pdfShapePath.GetFlowChartExtractPath();
      case AutoShapeType.FlowChartMerge:
        return pdfShapePath.GetFlowChartMergePath();
      case AutoShapeType.FlowChartStoredData:
        return pdfShapePath.GetFlowChartOnlineStoragePath();
      case AutoShapeType.FlowChartDelay:
        return pdfShapePath.GetFlowChartDelayPath();
      case AutoShapeType.FlowChartSequentialAccessStorage:
        return pdfShapePath.GetFlowChartSequentialAccessStoragePath();
      case AutoShapeType.FlowChartMagneticDisk:
        return pdfShapePath.GetFlowChartMagneticDiskPath();
      case AutoShapeType.FlowChartDirectAccessStorage:
        return pdfShapePath.GetFlowChartDirectAccessStoragePath();
      case AutoShapeType.FlowChartDisplay:
        return pdfShapePath.GetFlowChartDisplayPath();
      case AutoShapeType.Explosion1:
        return pdfShapePath.GetExplosion1();
      case AutoShapeType.Explosion2:
        return pdfShapePath.GetExplosion2();
      case AutoShapeType.Star4Point:
        return pdfShapePath.GetStar4Point();
      case AutoShapeType.Star5Point:
        return pdfShapePath.GetStar5Point();
      case AutoShapeType.Star8Point:
        return pdfShapePath.GetStar8Point();
      case AutoShapeType.Star16Point:
        return pdfShapePath.GetStar16Point();
      case AutoShapeType.Star24Point:
        return pdfShapePath.GetStar24Point();
      case AutoShapeType.Star32Point:
        return pdfShapePath.GetStar32Point();
      case AutoShapeType.UpRibbon:
        return pdfShapePath.GetUpRibbon();
      case AutoShapeType.DownRibbon:
        return pdfShapePath.GetDownRibbon();
      case AutoShapeType.CurvedUpRibbon:
        return pdfShapePath.GetCurvedUpRibbon();
      case AutoShapeType.CurvedDownRibbon:
        return pdfShapePath.GetCurvedDownRibbon();
      case AutoShapeType.VerticalScroll:
        return pdfShapePath.GetVerticalScroll();
      case AutoShapeType.HorizontalScroll:
        PdfPath[] horizontalScroll = pdfShapePath.GetHorizontalScroll();
        IColor color3 = (IColor) null;
        IFill defaultFillFormat3 = shapeImpl.GetDefaultFillFormat();
        if (defaultFillFormat3.FillType == FillType.Solid)
          color3 = defaultFillFormat3.SolidFill.Color;
        if (defaultFillFormat3.FillType == FillType.Gradient && shapeImpl.Fill.GradientFill.GradientStops.Count > 0)
          color3 = ((GradientStop) shapeImpl.Fill.GradientFill.GradientStops[shapeImpl.Fill.GradientFill.GradientStops.Count - 1]).Color;
        foreach (PdfPath path3 in horizontalScroll)
        {
          if (color3 != null && color3.SystemColor != Color.Empty)
            this.PDFGraphics.DrawPath((PdfBrush) new PdfSolidBrush((PdfColor) color3.SystemColor), path3);
          this.PDFGraphics.DrawPath(pen, path3);
        }
        if (shapeImpl is Picture)
        {
          path1 = horizontalScroll[0];
          break;
        }
        break;
      case AutoShapeType.Wave:
        return pdfShapePath.GetWave();
      case AutoShapeType.DoubleWave:
        return pdfShapePath.GetDoubleWave();
      case AutoShapeType.RectangularCallout:
        return pdfShapePath.GetRectangularCalloutPath();
      case AutoShapeType.RoundedRectangularCallout:
        return pdfShapePath.GetRoundedRectangularCalloutPath();
      case AutoShapeType.OvalCallout:
        return pdfShapePath.GetOvalCalloutPath();
      case AutoShapeType.CloudCallout:
        return pdfShapePath.GetCloudCalloutPath();
      case AutoShapeType.LineCallout1:
      case AutoShapeType.LineCallout1NoBorder:
        return pdfShapePath.GetLineCallout1Path();
      case AutoShapeType.LineCallout2:
      case AutoShapeType.LineCallout2NoBorder:
        return pdfShapePath.GetLineCallout2Path();
      case AutoShapeType.LineCallout3:
      case AutoShapeType.LineCallout3NoBorder:
        return pdfShapePath.GetLineCallout3Path();
      case AutoShapeType.LineCallout1AccentBar:
      case AutoShapeType.LineCallout1BorderAndAccentBar:
        return pdfShapePath.GetLineCallout1AccentBarPath();
      case AutoShapeType.LineCallout2AccentBar:
      case AutoShapeType.LineCallout2BorderAndAccentBar:
        return pdfShapePath.GetLineCallout2AccentBarPath();
      case AutoShapeType.LineCallout3AccentBar:
      case AutoShapeType.LineCallout3BorderAndAccentBar:
        return pdfShapePath.GetLineCallout3AccentBarPath();
      case AutoShapeType.LeftRightRibbon:
        return pdfShapePath.GetLeftRightRibbonPath();
      case AutoShapeType.DiagonalStripe:
        return pdfShapePath.GetDiagonalStripePath();
      case AutoShapeType.Pie:
        return pdfShapePath.GetPiePath();
      case AutoShapeType.Decagon:
        return pdfShapePath.GetDecagonPath();
      case AutoShapeType.Heptagon:
        return pdfShapePath.GetHeptagonPath();
      case AutoShapeType.Dodecagon:
        return pdfShapePath.GetDodecagonPath();
      case AutoShapeType.Star6Point:
        return pdfShapePath.GetStar6Point();
      case AutoShapeType.Star7Point:
        return pdfShapePath.GetStar7Point();
      case AutoShapeType.Star10Point:
        return pdfShapePath.GetStar10Point();
      case AutoShapeType.Star12Point:
        return pdfShapePath.GetStar12Point();
      case AutoShapeType.RoundSingleCornerRectangle:
        return pdfShapePath.GetRoundSingleCornerRectanglePath();
      case AutoShapeType.RoundSameSideCornerRectangle:
        return pdfShapePath.GetRoundSameSideCornerRectanglePath();
      case AutoShapeType.RoundDiagonalCornerRectangle:
        return pdfShapePath.GetRoundDiagonalCornerRectanglePath();
      case AutoShapeType.SnipAndRoundSingleCornerRectangle:
        return pdfShapePath.GetSnipAndRoundSingleCornerRectanglePath();
      case AutoShapeType.SnipSingleCornerRectangle:
        return pdfShapePath.GetSnipSingleCornerRectanglePath();
      case AutoShapeType.SnipSameSideCornerRectangle:
        return pdfShapePath.GetSnipSameSideCornerRectanglePath();
      case AutoShapeType.SnipDiagonalCornerRectangle:
        return pdfShapePath.GetSnipDiagonalCornerRectanglePath();
      case AutoShapeType.Frame:
        return pdfShapePath.GetFramePath();
      case AutoShapeType.HalfFrame:
        return pdfShapePath.GetHalfFramePath();
      case AutoShapeType.Teardrop:
        return pdfShapePath.GetTearDropPath();
      case AutoShapeType.Chord:
        return pdfShapePath.GetChordPath();
      case AutoShapeType.Corner:
        return pdfShapePath.GetL_ShapePath();
      case AutoShapeType.MathPlus:
        return pdfShapePath.GetMathPlusPath();
      case AutoShapeType.MathMinus:
        return pdfShapePath.GetMathMinusPath();
      case AutoShapeType.MathMultiply:
        return pdfShapePath.GetMathMultiplyPath();
      case AutoShapeType.MathDivision:
        return pdfShapePath.GetMathDivisionPath();
      case AutoShapeType.MathEqual:
        return pdfShapePath.GetMathEqualPath();
      case AutoShapeType.MathNotEqual:
        return pdfShapePath.GetMathNotEqualPath();
      case AutoShapeType.PieWedge:
        path1.AddPie(bounds.X, bounds.Y, bounds.Width * 2f, bounds.Height * 2f, 180f, 90f);
        return path1;
      case AutoShapeType.Gear6:
        this.SetCustomGeometry("<pathLst><path w=\"2167466\" h=\"2167466\"><moveTo><pt x=\"1621800\" y=\"548964\"/></moveTo><lnTo><pt x=\"1941574\" y=\"452590\"/></lnTo><lnTo><pt x=\"2059240\" y=\"656392\"/></lnTo><lnTo><pt x=\"1815890\" y=\"885138\"/></lnTo><cubicBezTo><pt x=\"1851165\" y=\"1015185\"/><pt x=\"1851165\" y=\"1152281\"/><pt x=\"1815890\" y=\"1282328\"/></cubicBezTo><lnTo><pt x=\"2059240\" y=\"1511074\"/></lnTo><lnTo><pt x=\"1941574\" y=\"1714876\"/></lnTo><lnTo><pt x=\"1621800\" y=\"1618502\"/></lnTo><cubicBezTo><pt x=\"1526813\" y=\"1714075\"/><pt x=\"1408085\" y=\"1782623\"/><pt x=\"1277823\" y=\"1817097\"/></cubicBezTo><lnTo><pt x=\"1201398\" y=\"2142217\"/></lnTo><lnTo><pt x=\"966068\" y=\"2142217\"/></lnTo><lnTo><pt x=\"889643\" y=\"1817097\"/></lnTo><cubicBezTo><pt x=\"759381\" y=\"1782622\"/><pt x=\"640653\" y=\"1714074\"/><pt x=\"545666\" y=\"1618502\"/></cubicBezTo><lnTo><pt x=\"225892\" y=\"1714876\"/></lnTo><lnTo><pt x=\"108226\" y=\"1511074\"/></lnTo><lnTo><pt x=\"351576\" y=\"1282328\"/></lnTo><cubicBezTo><pt x=\"316301\" y=\"1152281\"/><pt x=\"316301\" y=\"1015185\"/><pt x=\"351576\" y=\"885138\"/></cubicBezTo><lnTo><pt x=\"108226\" y=\"656392\"/></lnTo><lnTo><pt x=\"225892\" y=\"452590\"/></lnTo><lnTo><pt x=\"545666\" y=\"548964\"/></lnTo><cubicBezTo><pt x=\"640653\" y=\"453391\"/><pt x=\"759381\" y=\"384843\"/><pt x=\"889643\" y=\"350369\"/></cubicBezTo><lnTo><pt x=\"966068\" y=\"25249\"/></lnTo><lnTo><pt x=\"1201398\" y=\"25249\"/></lnTo><lnTo><pt x=\"1277823\" y=\"350369\"/></lnTo><cubicBezTo><pt x=\"1408085\" y=\"384844\"/><pt x=\"1526813\" y=\"453392\"/><pt x=\"1621800\" y=\"548964\"/></cubicBezTo><close/></path></pathLst>", shapeImpl);
        return this.GetCustomGeomentryPath(bounds, shapeImpl)[0];
      case AutoShapeType.Gear9:
        this.SetCustomGeometry("<pathLst><path w=\"2980266\" h=\"2980266\"><moveTo><pt x=\"2115406\" y=\"475169\"/></moveTo><lnTo><pt x=\"2347223\" y=\"280641\"/></lnTo><lnTo><pt x=\"2532418\" y=\"436038\"/></lnTo><lnTo><pt x=\"2381100\" y=\"698113\"/></lnTo><cubicBezTo><pt x=\"2488696\" y=\"819151\"/><pt x=\"2570502\" y=\"960843\"/><pt x=\"2621526\" y=\"1114543\"/></cubicBezTo><lnTo><pt x=\"2924149\" y=\"1114535\"/></lnTo><lnTo><pt x=\"2966129\" y=\"1352617\"/></lnTo><lnTo><pt x=\"2681754\" y=\"1456113\"/></lnTo><cubicBezTo><pt x=\"2686376\" y=\"1617995\"/><pt x=\"2657965\" y=\"1779121\"/><pt x=\"2598255\" y=\"1929659\"/></cubicBezTo><lnTo><pt x=\"2830082\" y=\"2124176\"/></lnTo><lnTo><pt x=\"2709205\" y=\"2333542\"/></lnTo><lnTo><pt x=\"2424835\" y=\"2230031\"/></lnTo><cubicBezTo><pt x=\"2324320\" y=\"2357010\"/><pt x=\"2198986\" y=\"2462178\"/><pt x=\"2056481\" y=\"2539116\"/></cubicBezTo><lnTo><pt x=\"2109039\" y=\"2837141\"/></lnTo><lnTo><pt x=\"1881863\" y=\"2919826\"/></lnTo><lnTo><pt x=\"1730559\" y=\"2657743\"/></lnTo><cubicBezTo><pt x=\"1571939\" y=\"2690405\"/><pt x=\"1408327\" y=\"2690405\"/><pt x=\"1249707\" y=\"2657743\"/></cubicBezTo><lnTo><pt x=\"1098403\" y=\"2919826\"/></lnTo><lnTo><pt x=\"871227\" y=\"2837141\"/></lnTo><lnTo><pt x=\"923785\" y=\"2539117\"/></lnTo><cubicBezTo><pt x=\"781280\" y=\"2462179\"/><pt x=\"655947\" y=\"2357011\"/><pt x=\"555431\" y=\"2230032\"/></cubicBezTo><lnTo><pt x=\"271061\" y=\"2333542\"/></lnTo><lnTo><pt x=\"150184\" y=\"2124176\"/></lnTo><lnTo><pt x=\"382011\" y=\"1929660\"/></lnTo><cubicBezTo><pt x=\"322301\" y=\"1779122\"/><pt x=\"293890\" y=\"1617995\"/><pt x=\"298512\" y=\"1456114\"/></cubicBezTo><lnTo><pt x=\"14137\" y=\"1352617\"/></lnTo><lnTo><pt x=\"56117\" y=\"1114535\"/></lnTo><lnTo><pt x=\"358740\" y=\"1114543\"/></lnTo><cubicBezTo><pt x=\"409764\" y=\"960843\"/><pt x=\"491570\" y=\"819151\"/><pt x=\"599166\" y=\"698113\"/></cubicBezTo><lnTo><pt x=\"447848\" y=\"436038\"/></lnTo><lnTo><pt x=\"633043\" y=\"280641\"/></lnTo><lnTo><pt x=\"864860\" y=\"475169\"/></lnTo><cubicBezTo><pt x=\"1002743\" y=\"390226\"/><pt x=\"1156488\" y=\"334267\"/><pt x=\"1316713\" y=\"310708\"/></cubicBezTo><lnTo><pt x=\"1369255\" y=\"12681\"/></lnTo><lnTo><pt x=\"1611011\" y=\"12681\"/></lnTo><lnTo><pt x=\"1663553\" y=\"310708\"/></lnTo><cubicBezTo><pt x=\"1823778\" y=\"334267\"/><pt x=\"1977523\" y=\"390226\"/><pt x=\"2115406\" y=\"475169\"/></cubicBezTo><close/></path></pathLst>", shapeImpl);
        return this.GetCustomGeomentryPath(bounds, shapeImpl)[0];
      case AutoShapeType.Funnel:
        return pdfShapePath.GetFunnelPath();
      case AutoShapeType.LeftCircularArrow:
        return pdfShapePath.GetLeftCircularArrowPath();
      case AutoShapeType.Cloud:
        return pdfShapePath.GetCloudPath();
      case AutoShapeType.SwooshArrow:
        return pdfShapePath.GetSwooshArrowPath();
      case AutoShapeType.Line:
        path1.AddLine(bounds.X, bounds.Y, bounds.Right, bounds.Bottom);
        return path1;
      case AutoShapeType.StraightConnector:
        if ((double) pen.Width < 1.0)
          pen.Width = 1f;
        bool isArrowHeadExist = false;
        PointF[] linePoints3 = new PointF[2]
        {
          new PointF(bounds.X, bounds.Y),
          new PointF(bounds.Right, bounds.Bottom)
        };
        this.DrawArrowHead(shapeImpl, pen, bounds, ref isArrowHeadExist, ref path1, linePoints3);
        if (!isArrowHeadExist)
          path1.AddLines(linePoints3);
        return path1;
      case AutoShapeType.ElbowConnector:
        return pdfShapePath.GetBentConnector3Path();
      case AutoShapeType.CurvedConnector:
        return pdfShapePath.GetCurvedConnector3Path();
      case AutoShapeType.BentConnector2:
        return pdfShapePath.GetBentConnector2Path();
      case AutoShapeType.BentConnector4:
        return pdfShapePath.GetBentConnector4Path();
      case AutoShapeType.BentConnector5:
        return pdfShapePath.GetBentConnector5Path();
      case AutoShapeType.CurvedConnector2:
        return pdfShapePath.GetCurvedConnector2Path();
      case AutoShapeType.CurvedConnector4:
        return pdfShapePath.GetCurvedConnector4Path();
      case AutoShapeType.CurvedConnector5:
        return pdfShapePath.GetCurvedConnector5Path();
      default:
        if (shapeImpl.GetPresetGeometry() || shapeImpl.DrawingType == DrawingType.PlaceHolder)
        {
          path1.AddRectangle(bounds);
          break;
        }
        break;
    }
    return path1;
  }

  private void SetCustomGeometry(string pathList, Shape shapeImpl)
  {
    Stream input = (Stream) new MemoryStream(Encoding.UTF8.GetBytes(pathList.Replace('\'', ' ')));
    input.Position = 0L;
    XmlReader reader = XmlReader.Create(input);
    shapeImpl.Path2DList = new List<Path2D>();
    DrawingParser.ParsePath2D(reader, shapeImpl, (Dictionary<string, string>) null);
  }

  private PdfPath[] GetCustomGeomentryPath(RectangleF bounds, Shape shapeImpl)
  {
    List<Path2D> path2Dlist = shapeImpl.GetPath2DList();
    bool flag = false;
    if (path2Dlist.Count > 1)
    {
      IFill fill = !shapeImpl.IsBgFill ? shapeImpl.GetDefaultFillFormat() : ((Background) shapeImpl.BaseSlide.Background).GetDefaultFillFormat();
      if (fill != null && fill.FillType == FillType.Solid)
        flag = true;
    }
    PdfPath[] customGeomentryPath;
    if (flag)
    {
      customGeomentryPath = new PdfPath[path2Dlist.Count];
      for (int index = 0; index < path2Dlist.Count; ++index)
      {
        Path2D path2D = path2Dlist[index];
        double width = path2D.Width;
        double height = path2D.Height;
        PdfPath path = new PdfPath();
        this.GetGeomentryPath(path, path2D.PathElements, width, height, bounds);
        customGeomentryPath[index] = path;
      }
    }
    else
    {
      customGeomentryPath = new PdfPath[1];
      PdfPath path = new PdfPath();
      foreach (Path2D path2D in shapeImpl.GetPath2DList())
      {
        double width = path2D.Width;
        double height = path2D.Height;
        this.GetGeomentryPath(path, path2D.PathElements, width, height, bounds);
      }
      customGeomentryPath[0] = path;
    }
    return customGeomentryPath;
  }

  private float GetGeomentryPathYValue(double pathHeight, double y, RectangleF bounds)
  {
    if (pathHeight == 0.0)
      return bounds.Y + (float) Helper.EmuToPoint(y);
    double num = y * 100.0 / pathHeight;
    return (float) ((double) bounds.Height * num / 100.0) + bounds.Y;
  }

  private float GetGeomentryPathXValue(double pathWidth, double x, RectangleF bounds)
  {
    if (pathWidth == 0.0)
      return bounds.X + (float) Helper.EmuToPoint(x);
    double num = x * 100.0 / pathWidth;
    return (float) ((double) bounds.Width * num / 100.0) + bounds.X;
  }

  private void GetGeomentryPath(
    PdfPath path,
    List<double> pathElements,
    double pathWidth,
    double pathHeight,
    RectangleF bounds)
  {
    PointF point1 = (PointF) Point.Empty;
    double num = 0.0;
    for (int index = 0; index < pathElements.Count; index = index + ((int) num + 1) + 1)
    {
      switch ((ushort) pathElements[index])
      {
        case 1:
          path.CloseFigure();
          point1 = (PointF) Point.Empty;
          num = 0.0;
          break;
        case 2:
          path.CloseFigure();
          num = pathElements[index + 1] * 2.0;
          point1 = new PointF(this.GetGeomentryPathXValue(pathWidth, pathElements[index + 2], bounds), this.GetGeomentryPathYValue(pathHeight, pathElements[index + 3], bounds));
          break;
        case 3:
          num = pathElements[index + 1] * 2.0;
          PointF point2 = new PointF(this.GetGeomentryPathXValue(pathWidth, pathElements[index + 2], bounds), this.GetGeomentryPathYValue(pathHeight, pathElements[index + 3], bounds));
          path.AddLine(point1, point2);
          point1 = point2;
          break;
        case 4:
          num = pathElements[index + 1] * 2.0;
          RectangleF rectangle = new RectangleF();
          rectangle.X = bounds.X;
          rectangle.Y = bounds.Y;
          rectangle.Width = (float) Helper.EmuToPoint(pathElements[index + 2]) * 2f;
          rectangle.Height = (float) Helper.EmuToPoint(pathElements[index + 3]) * 2f;
          float startAngle = (float) pathElements[index + 4] / 60000f;
          float sweepAngle = (float) pathElements[index + 5] / 60000f;
          path.AddArc(rectangle, startAngle, sweepAngle);
          point1 = path.PathPoints[path.PathPoints.Length - 1];
          break;
        case 5:
          num = pathElements[index + 1] * 2.0;
          PointF[] points1 = new PointF[3]
          {
            point1,
            new PointF(this.GetGeomentryPathXValue(pathWidth, pathElements[index + 2], bounds), this.GetGeomentryPathYValue(pathHeight, pathElements[index + 3], bounds)),
            new PointF(this.GetGeomentryPathXValue(pathWidth, pathElements[index + 4], bounds), this.GetGeomentryPathYValue(pathHeight, pathElements[index + 5], bounds))
          };
          path.AddBeziers(points1);
          point1 = points1[2];
          break;
        case 6:
          num = pathElements[index + 1] * 2.0;
          PointF[] points2 = new PointF[4]
          {
            point1,
            new PointF(this.GetGeomentryPathXValue(pathWidth, pathElements[index + 2], bounds), this.GetGeomentryPathYValue(pathHeight, pathElements[index + 3], bounds)),
            new PointF(this.GetGeomentryPathXValue(pathWidth, pathElements[index + 4], bounds), this.GetGeomentryPathYValue(pathHeight, pathElements[index + 5], bounds)),
            new PointF(this.GetGeomentryPathXValue(pathWidth, pathElements[index + 6], bounds), this.GetGeomentryPathYValue(pathHeight, pathElements[index + 7], bounds))
          };
          path.AddBeziers(points2);
          point1 = points2[3];
          break;
      }
    }
  }

  private PdfImage GetPdfImage(Image image)
  {
    PdfImage pdfImage = PdfImage.FromImage(image);
    switch (pdfImage)
    {
      case PdfBitmap _:
        (pdfImage as PdfBitmap).Quality = (long) this.ImageQuality;
        break;
      case PdfMetafile _:
        (pdfImage as PdfMetafile).IsEmbedCompleteFonts = this.EmbedCompleteFonts;
        break;
    }
    return pdfImage;
  }

  private PdfImage GetPdfImage(MemoryStream stream)
  {
    PdfImage pdfImage = PdfImage.FromStream((Stream) stream);
    switch (pdfImage)
    {
      case PdfBitmap _:
        (pdfImage as PdfBitmap).Quality = (long) this.ImageQuality;
        break;
      case PdfMetafile _:
        (pdfImage as PdfMetafile).IsEmbedCompleteFonts = this.EmbedCompleteFonts;
        break;
    }
    return pdfImage;
  }

  internal PdfTrueTypeFont GetPdfTrueTypeFont(
    System.Drawing.Font font,
    bool isEmbedFont,
    bool isEmbedCompleteFont,
    string text,
    PdfStringFormat pdfStringFormat)
  {
    string key = $"{font.Name}_{(object) font.Style}";
    string name = font.Name;
    PdfTrueTypeFont pdfFont;
    if (this.m_privateFontStream != null && (this.m_privateFontStream.ContainsKey(key) || this.m_privateFontStream.ContainsKey(name)))
    {
      Stream fontStream = (Stream) null;
      if (this.m_privateFontStream.ContainsKey(key))
        fontStream = this.m_privateFontStream[key];
      else if (this.m_privateFontStream.ContainsKey(name))
        fontStream = this.m_privateFontStream[name];
      if (fontStream != null && fontStream.CanRead)
      {
        fontStream.Position = 0L;
        pdfFont = new PdfTrueTypeFont(fontStream, font.Size, true, (PdfFontStyle) font.Style);
      }
      else
        pdfFont = new PdfTrueTypeFont(font, isEmbedFont, false, isEmbedCompleteFont);
    }
    else
      pdfFont = new PdfTrueTypeFont(font, isEmbedFont, false, isEmbedCompleteFont);
    if (this.FallbackFonts != null)
      pdfFont = this.GetFallbackFont(pdfFont, this.FallbackFonts, font, text, pdfStringFormat, isEmbedFont, isEmbedCompleteFont);
    return pdfFont;
  }
}
