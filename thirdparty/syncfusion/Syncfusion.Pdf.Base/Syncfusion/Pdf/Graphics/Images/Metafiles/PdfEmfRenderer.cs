// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Images.Metafiles.PdfEmfRenderer
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Microsoft.Win32;
using Syncfusion.Pdf.Native;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Images.Metafiles;

internal class PdfEmfRenderer : IDisposable
{
  private static Image s_bmp = (Image) new Bitmap(1, 1);
  private float TextAngleLocal;
  private PdfGraphics m_graphics;
  private System.Drawing.Graphics m_grCache;
  private PdfTransformationMatrix m_bounds;
  private bool m_stateChanged;
  private bool m_bFirstCall = true;
  private PdfUnitConvertor m_convertX;
  private PdfUnitConvertor m_convertY;
  internal float m_horizontalResolution;
  internal float m_verticalResolution;
  private Dictionary<GraphicsState, PdfGraphicsState> m_graphicsStates = new Dictionary<GraphicsState, PdfGraphicsState>();
  private Dictionary<GraphicsContainer, PdfGraphicsState> m_graphicsContainer = new Dictionary<GraphicsContainer, PdfGraphicsState>();
  private PdfGraphicsState m_startState;
  private bool m_bFirstTransform = true;
  private long m_quality = 100;
  private int m_imageResolution;
  private bool m_bPageTransformed;
  private bool m_stateRestored;
  private bool m_embedFonts;
  private bool m_isembedFonts;
  private bool m_isembedCompleteEmbedFonts;
  private float m_alphaPen = 1f;
  private float m_alphaBrush = 1f;
  private Font m_fontInfo;
  private bool m_bIsTransparency;
  private bool m_isTranslate;
  private Matrix m_multiplyTransform;
  private PdfBlendMode m_blendMode;
  private bool m_CloseShape;
  internal bool m_taggedPDF;
  private object m_context;
  private RectangleF m_realClip;
  internal bool m_EMFState;
  internal EmfPlusRecordType m_recordType;
  internal EmfPlusRecordType m_previousRecordtype;
  private RectangleF m_textClip;
  internal CustomLineCapArrowData m_customLineCapArrowData;
  private bool m_isIntersectClipRect;
  internal GraphicsPath m_graphicsPath;
  private string m_hashValue = " ";
  internal bool m_isClippedPath;
  private bool m_isRTLFormat;
  internal PdfDocument m_document;
  private bool negativeRegion;
  internal bool m_optimizeIdenticalImages;
  internal bool m_multiplyTransformed = true;
  private Dictionary<string, PdfTrueTypeFont> m_fontCollection = new Dictionary<string, PdfTrueTypeFont>();
  private SizeF m_translateTransform;
  private bool m_isUnicodeString;
  private RectangleF m_brushRectangle;
  private GraphicsPath m_fillPath;
  internal bool m_clipPath;
  private bool m_boundTransformation;
  internal int m_customLineDataFlag;
  internal CustomLineCapData m_customLineCapData;
  internal PointF m_innerImageStartingPosition;
  internal bool isPositiveLogicPoints;
  internal bool m_isBoldStyle;
  private CustomFont m_customFontCollection;
  private Matrix pageUnitScale;
  internal bool isinnerEMF;
  private Matrix m_multiplyTransformMatrix;
  private bool m_onDrawPrimitiveMultiplyTransform;
  private bool m_restoreState;
  private int m_emfWidth;
  private bool m_complexScript;
  private bool m_isDrawLine;
  private PointF m_clipRegionPoint = new PointF();
  private bool m_recreateInnerEMF;
  internal bool m_skipInnerScale;
  internal bool m_nonScaledRegion;
  private bool m_gradientPen;
  internal PointF m_originalLocation;
  internal PointF[] previousClipBounds;
  internal bool m_transparentBackMode;

  internal bool ComplexScript
  {
    get => this.m_complexScript;
    set => this.m_complexScript = value;
  }

  internal CustomFont CustomFontCollection
  {
    get => this.m_customFontCollection;
    set
    {
      if (value == null)
        return;
      this.m_customFontCollection = value;
    }
  }

  private Font FontInfo
  {
    get => this.m_fontInfo;
    set => this.m_fontInfo = value;
  }

  internal bool IsTranparency
  {
    get => this.m_bIsTransparency;
    set => this.m_bIsTransparency = value;
  }

  internal float AlphaPen
  {
    get => this.m_alphaPen;
    set => this.m_alphaPen = value;
  }

  internal float AlphaBrush
  {
    get => this.m_alphaBrush;
    set => this.m_alphaBrush = value;
  }

  internal PdfBlendMode BlendMode
  {
    get => this.m_blendMode;
    set => this.m_blendMode = value;
  }

  internal bool OptimizeIdenticalImages
  {
    get => this.m_optimizeIdenticalImages;
    set => this.m_optimizeIdenticalImages = value;
  }

  internal PdfDocument Document
  {
    get => this.m_document;
    set => this.m_document = value;
  }

  public PdfGraphics Graphics => this.m_graphics;

  public System.Drawing.Graphics NativeGraphics => this.m_grCache;

  private RectangleF ClipBounds => RectangleF.Empty;

  public Matrix Transform
  {
    get => this.NativeGraphics.Transform;
    set
    {
      this.InternalResetClip();
      this.NativeGraphics.Transform = value;
      PdfTransformationMatrix matrix = PdfEmfRenderer.PrepareMatrix(value, this.PageScale);
      this.Graphics.PutComment("Transform property");
      this.SetTransform();
      this.Graphics.MultiplyTransform(matrix);
    }
  }

  public float PageScale
  {
    get => this.NativeGraphics.PageScale;
    set
    {
      this.NativeGraphics.PageScale = value;
      this.Graphics.PutComment("PageScale property");
      this.SetTransform();
      this.Graphics.ScaleTransform(value, value);
    }
  }

  public GraphicsUnit PageUnit
  {
    get => this.NativeGraphics.PageUnit;
    set
    {
      this.NativeGraphics.PageUnit = value;
      float num1 = 1f;
      float num2 = 1f;
      if (value != GraphicsUnit.Display)
      {
        num1 = this.ConvertX.ConvertUnits(num1, this.GraphicsToPrintUnits(value), PdfGraphicsUnit.Pixel);
        num2 = this.ConvertY.ConvertUnits(num2, this.GraphicsToPrintUnits(value), PdfGraphicsUnit.Pixel);
      }
      this.Graphics.PutComment("PageUnit property");
      this.NativeGraphics.ScaleTransform(num1, num2, MatrixOrder.Prepend);
      this.Graphics.ScaleTransform(num1, num2);
      this.pageUnitScale = this.NativeGraphics.Transform;
    }
  }

  private PdfUnitConvertor ConvertX
  {
    get
    {
      if (this.m_convertX == null)
        this.m_convertX = new PdfUnitConvertor(this.NativeGraphics);
      return this.m_convertX;
    }
  }

  private PdfUnitConvertor ConvertY
  {
    get
    {
      if (this.m_convertY == null)
        this.m_convertY = new PdfUnitConvertor(this.NativeGraphics);
      return this.m_convertY;
    }
  }

  public bool PageTransformed
  {
    get => this.m_bPageTransformed;
    set => this.m_bPageTransformed = value;
  }

  internal bool EmbedFonts => this.m_embedFonts;

  internal bool IsEmbedFonts
  {
    get => this.m_isembedFonts;
    set => this.m_isembedFonts = value;
  }

  internal bool IsEmbedCompleteFonts
  {
    get => this.m_isembedCompleteEmbedFonts;
    set => this.m_isembedCompleteEmbedFonts = value;
  }

  private TextRegionManager TextRegions => this.Context as TextRegionManager;

  internal object Context
  {
    get => this.m_context;
    set
    {
      if (this.m_context == value)
        return;
      this.m_context = value;
    }
  }

  private RectangleF RealClip
  {
    get => this.m_realClip;
    set => this.m_realClip = value;
  }

  internal bool StateChanged
  {
    get => this.m_stateChanged;
    set => this.m_stateChanged = value;
  }

  internal int EmfWidth
  {
    get => this.m_emfWidth;
    set => this.m_emfWidth = value;
  }

  internal bool RecreateInnerEMF
  {
    get => this.m_recreateInnerEMF;
    set => this.m_recreateInnerEMF = value;
  }

  public PdfEmfRenderer(PdfGraphics graphics)
  {
    this.m_graphics = graphics != null ? graphics : throw new ArgumentNullException(nameof (graphics));
  }

  internal PdfEmfRenderer(PdfGraphics graphics, PointF location, bool tagged)
    : this(graphics)
  {
    this.m_taggedPDF = tagged;
    this.Context = (object) new TextRegionManager();
  }

  public PdfEmfRenderer(PdfGraphics graphics, long quality, bool embedFonts)
    : this(graphics)
  {
    this.m_quality = quality;
    this.m_embedFonts = embedFonts;
  }

  public PdfEmfRenderer(PdfGraphics graphics, int imageResolution, bool embedFonts)
    : this(graphics)
  {
    this.m_imageResolution = imageResolution;
    this.m_embedFonts = embedFonts;
  }

  public GraphicsContainer BeginContainer()
  {
    this.InternalResetClip();
    this.InternalResetTransformation();
    this.Graphics.PutComment("BegingContainer");
    PdfGraphicsState pdfGraphicsState = this.Graphics.Save();
    GraphicsContainer key = this.NativeGraphics.BeginContainer();
    this.m_graphicsContainer[key] = pdfGraphicsState;
    this.m_bFirstTransform = true;
    return key;
  }

  public GraphicsContainer BeginContainer(
    RectangleF destRect,
    RectangleF srcRect,
    GraphicsUnit unit)
  {
    this.InternalResetClip();
    this.InternalResetTransformation();
    this.Graphics.PutComment("BegingContainer");
    PdfGraphicsState pdfGraphicsState = this.Graphics.Save();
    GraphicsContainer key = this.NativeGraphics.BeginContainer(destRect, srcRect, unit);
    this.m_graphicsContainer[key] = pdfGraphicsState;
    return key;
  }

  public void Clear(System.Drawing.Color color)
  {
    this.NativeGraphics.Clear(color);
    using (Brush brush = (Brush) new SolidBrush(color))
    {
      RectangleF[] rects = new RectangleF[1]
      {
        this.NativeGraphics.ClipBounds
      };
      this.FillRectangles(brush, rects);
    }
  }

  public void DrawArc(Pen pen, RectangleF rect, float startAngle, float sweepAngle)
  {
    if (pen == null)
      throw new ArgumentNullException(nameof (pen));
    this.OnDrawPrimitive();
    this.Graphics.DrawArc(this.ConvertPen(pen), rect, startAngle, sweepAngle);
  }

  public void DrawBeziers(Pen pen, PointF[] points)
  {
    if (pen == null)
      throw new ArgumentNullException(nameof (pen));
    if (points == null)
      throw new ArgumentNullException(nameof (points));
    if (points.Length < 4)
      throw new ArgumentException("Incorrect size of array", nameof (points));
    this.OnDrawPrimitive();
    PdfPen pen1 = this.ConvertPen(pen);
    int num = 3;
    int index1 = 0;
    PointF startPoint = points[index1];
    int index2 = index1 + 1;
    while (index2 + num <= points.Length)
    {
      PointF point1 = points[index2];
      int index3 = index2 + 1;
      PointF point2 = points[index3];
      int index4 = index3 + 1;
      PointF point3 = points[index4];
      index2 = index4 + 1;
      this.Graphics.DrawBezier(pen1, startPoint, point1, point2, point3);
      startPoint = point3;
    }
  }

  public void DrawClosedCurve(Pen pen, PointF[] points, float tension, PdfFillMode fillMode)
  {
    if (pen == null)
      throw new ArgumentNullException(nameof (pen));
    this.Graphics.DrawPolygon(this.ConvertPen(pen), points);
  }

  public void DrawCurve(
    Pen pen,
    PointF[] points,
    PointF[] penPoints,
    int offset,
    int numSegments,
    float tension)
  {
    GraphicsPath path = new GraphicsPath();
    if (!this.IsLine(points))
      path.AddCurve(points, tension);
    else
      path.AddLines(points);
    this.DrawPath(pen, path);
    this.DrawCustomCap(pen, points, penPoints, false);
  }

  public void DrawEllipse(Pen pen, RectangleF rect)
  {
    if (pen == null)
      throw new ArgumentNullException(nameof (pen));
    this.OnDrawPrimitive();
    this.Graphics.DrawEllipse(this.ConvertPen(pen), rect);
  }

  public void DrawImage(Image image, RectangleF destRect, RectangleF srcRect, GraphicsUnit units)
  {
    if (image == null)
      throw new ArgumentNullException(nameof (image));
    if (this.m_taggedPDF && !this.CheckPdfPage(destRect, true))
      return;
    this.OnDrawPrimitive();
    PdfImage image1 = PdfImage.FromImage(image);
    if (image1 is PdfMetafile)
    {
      PdfMetafile pdfMetafile = image1 as PdfMetafile;
      pdfMetafile.CustomFont = this.CustomFontCollection;
      pdfMetafile.ComplexScript = this.ComplexScript;
      pdfMetafile.m_isinnerEMF = true;
    }
    if (image is Bitmap)
    {
      if (this.m_imageResolution > 0)
      {
        Image image2 = this.ChangeResolution(this.m_imageResolution, image);
        if (image2 != null)
          this.Graphics.DrawImage((PdfImage) new PdfBitmap(image2), destRect);
        else
          this.Graphics.DrawImage((PdfImage) new PdfBitmap(image), destRect);
      }
      else
      {
        (image1 as PdfBitmap).Quality = this.m_quality;
        this.Graphics.DrawImage(image1, destRect);
      }
    }
    else
      this.Graphics.DrawImage(image1, destRect);
  }

  public void DrawImage(Image image, PointF[] points, RectangleF srcRect, GraphicsUnit units)
  {
    if (image == null)
      throw new ArgumentNullException(nameof (image));
    if (points.Length < 0 || points.Length > 3)
      throw new ArgumentOutOfRangeException(nameof (points), (object) points, "Value can not be less 0 and greater 3");
    RectangleF rectangleF = new RectangleF(Math.Min(points[0].X, points[2].X), points[0].Y, points[1].X - points[0].X, points[2].Y - points[0].Y);
    if (this.m_taggedPDF && !this.CheckPdfPage(rectangleF, true))
      return;
    this.OnDrawPrimitive();
    bool flag = false;
    if (image is Metafile)
      flag = true;
    if (image is Bitmap && this.m_imageResolution > 0)
      image = this.ChangeResolution(this.m_imageResolution, image);
    PdfImage image1;
    if (this.OptimizeIdenticalImages && this.Document != null && !flag)
    {
      if (this.CompareImage(image))
      {
        image1 = this.Document.ImageCollection[this.m_hashValue];
        this.Graphics.isImageOptimized = true;
      }
      else
      {
        image1 = PdfImage.FromImage(image);
        this.Document.ImageCollection.Add(this.m_hashValue, image1);
      }
    }
    else
    {
      if (this.RecreateInnerEMF && image is Metafile)
      {
        Metafile metafile1 = image as Metafile;
        Rectangle frameRect = new Rectangle(0, 0, image.Width, image.Height);
        System.Drawing.Graphics graphics1 = System.Drawing.Graphics.FromImage((Image) new Bitmap(1, 1));
        IntPtr hdc = graphics1.GetHdc();
        MemoryStream memoryStream = new MemoryStream();
        Metafile metafile2 = new Metafile((Stream) memoryStream, hdc, frameRect, MetafileFrameUnit.Pixel, EmfType.EmfOnly);
        graphics1.Dispose();
        System.Drawing.Graphics graphics2 = System.Drawing.Graphics.FromImage((Image) metafile2);
        graphics2.DrawImage((Image) metafile1, 0, 0, image.Width, image.Height);
        graphics2.Dispose();
        memoryStream.Dispose();
        image = (Image) metafile2;
        this.m_skipInnerScale = true;
      }
      image1 = PdfImage.FromImage(image);
    }
    if (image is Bitmap)
    {
      if (this.m_imageResolution > 0)
      {
        this.Graphics.DrawImage(image1, rectangleF);
      }
      else
      {
        (image1 as PdfBitmap).Quality = this.m_quality;
        if (this.Graphics.Layer == null)
          this.Graphics.m_pdfdocument = this.Document;
        this.Graphics.DrawImage(image1, rectangleF);
      }
    }
    else
    {
      if (this.Graphics.Layer == null)
        this.Graphics.m_pdfdocument = this.Document;
      PdfMetafile image2 = image1 as PdfMetafile;
      image2.m_isinnerEMF = true;
      image2.IsEmbedCompleteFonts = this.m_isembedCompleteEmbedFonts;
      image2.CustomFont = this.CustomFontCollection;
      image2.ComplexScript = this.ComplexScript;
      image2.m_skipScaling = this.m_skipInnerScale;
      this.Graphics.DrawImage((PdfImage) image2, rectangleF);
    }
  }

  private bool CompareImage(Image img)
  {
    this.m_hashValue = this.Document.GetImageHash(img);
    return this.Document.ImageCollection.ContainsKey(this.m_hashValue);
  }

  private Image ChangeResolution(int value, Image image)
  {
    Bitmap bitmap = image as Bitmap;
    bitmap.SetResolution((float) value, (float) value);
    Stream stream = (Stream) new MemoryStream();
    ImageFormat rawFormat = image.RawFormat;
    if (rawFormat.Equals((object) ImageFormat.Jpeg) || rawFormat.Equals((object) ImageFormat.Gif))
      bitmap.Save(stream, ImageFormat.Jpeg);
    else if (rawFormat.Equals((object) ImageFormat.Png))
      bitmap.Save(stream, ImageFormat.Png);
    else if (rawFormat.Equals((object) ImageFormat.Bmp))
      bitmap.Save(stream, ImageFormat.Bmp);
    else if (rawFormat.Equals((object) ImageFormat.MemoryBmp))
    {
      try
      {
        bitmap.Save(stream, ImageFormat.MemoryBmp);
      }
      catch
      {
        bitmap.Save(stream, ImageFormat.Png);
      }
    }
    return stream.Length > 0L ? Image.FromStream(stream) : (Image) bitmap;
  }

  public void DrawImage(
    Image image,
    Brush brush,
    RectangleF destRect,
    RectangleF srcRect,
    uint dwRop)
  {
    this.Graphics.PutComment(nameof (DrawImage));
    this.OnDrawPrimitive();
    this.ConvertBrush(brush);
    PdfBitmap image1 = (PdfBitmap) null;
    if (image != null)
    {
      image1 = (PdfBitmap) PdfImage.FromImage(image);
      image1.Quality = this.m_quality;
    }
    switch ((RASTER_CODE) dwRop)
    {
      case RASTER_CODE.SRCINVERT:
        if (image == null)
          break;
        this.Graphics.Save();
        this.Graphics.SetTransparency(0.1f);
        this.Graphics.DrawImage((PdfImage) image1, destRect);
        this.Graphics.Restore();
        break;
      case RASTER_CODE.SRCAND:
      case RASTER_CODE.PSDPXAX:
        if (image == null)
          break;
        this.Graphics.Save();
        this.Graphics.SetTransparency(1.1f, 1.1f, PdfBlendMode.Multiply);
        this.Graphics.DrawImage((PdfImage) image1, destRect);
        this.Graphics.Restore();
        break;
      case RASTER_CODE.SRCANDDST:
        PdfBrush brush1 = this.ConvertBrush(brush);
        this.Graphics.Save();
        this.Graphics.SetTransparency(0.1f);
        this.Graphics.DrawRectangle(brush1, destRect);
        this.Graphics.Restore();
        break;
      case RASTER_CODE.SRCCOPY:
        if (image == null)
          break;
        this.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);
        break;
      case RASTER_CODE.SRCPAINT:
        if (image == null)
          break;
        this.Graphics.Save();
        Bitmap bitmap = new Bitmap(image);
        bitmap.MakeTransparent(System.Drawing.Color.Black);
        MemoryStream memoryStream = new MemoryStream();
        bitmap.Save((Stream) memoryStream, ImageFormat.Png);
        PdfImage image2 = PdfImage.FromStream((Stream) memoryStream);
        memoryStream.Dispose();
        this.Graphics.SetTransparency(this.AlphaPen, this.AlphaBrush, PdfBlendMode.Lighten);
        this.Graphics.DrawImage(image2, destRect);
        this.Graphics.Restore();
        break;
      case RASTER_CODE.PATCOPY:
        if (this.m_transparentBackMode)
          break;
        this.FillRectangles(brush, new RectangleF[1]
        {
          destRect
        });
        break;
      default:
        if (image != null)
        {
          this.Graphics.Save();
          this.Graphics.SetTransparency(1f);
          this.Graphics.DrawImage((PdfImage) image1, destRect);
          this.Graphics.Restore();
          break;
        }
        PdfBrush brush2 = this.ConvertBrush(brush);
        this.Graphics.Save();
        this.Graphics.SetTransparency(0.5f);
        this.Graphics.DrawRectangle(brush2, destRect);
        this.Graphics.Restore();
        break;
    }
  }

  internal void DrawLines(Pen pen, PointF[] points, bool closeShape)
  {
    this.m_CloseShape = closeShape;
    this.DrawLines(pen, points);
    this.m_CloseShape = false;
  }

  public void DrawLines(Pen pen, PointF[] points)
  {
    this.m_isDrawLine = true;
    if (pen == null)
      throw new ArgumentNullException(nameof (pen));
    int num = points != null ? points.Length : throw new ArgumentNullException(nameof (points));
    if (num < 2)
      throw new ArgumentException("Incorrect size of array", nameof (points));
    this.OnDrawPrimitive();
    bool rotate = false;
    if (points.Length > 1 && pen.CompoundArray.Length != 0 && (double) points[0].X == (double) points[1].X)
      rotate = true;
    float alpha;
    PdfPen pdfPen = this.ConvertToPen(pen, out alpha);
    if ((double) alpha == 0.0)
      pdfPen = new PdfPen(PdfBrushes.White);
    else if (this.IsTranparency)
      this.Graphics.SetTransparency(this.AlphaPen, this.AlphaBrush, this.BlendMode);
    else
      this.Graphics.SetTransparency(alpha, alpha, PdfBlendMode.Normal);
    PointF point1 = points[0];
    if (pen.CompoundArray.Length != 0)
    {
      this.DrawCompoundLine(pen, points, rotate, pdfPen);
    }
    else
    {
      for (int index = 1; index < num; ++index)
      {
        PointF point = points[index];
        this.Graphics.DrawLine(pdfPen, point1, point);
        point1 = point;
      }
    }
    if (this.m_CloseShape)
      this.Graphics.DrawLine(pdfPen, points[num - 1], points[0]);
    PdfBrush brushFromPen = this.GetBrushFromPen(pdfPen);
    float width = pen.Width / 2f;
    LineCap endCap = pen.EndCap;
    if (num > 1 || endCap == LineCap.Round)
      this.DrawCap(endCap, points, num - 2, num - 1, width, brushFromPen);
    LineCap startCap = pen.StartCap;
    if (num > 1 || startCap == LineCap.Round)
      this.DrawCap(startCap, points, 0, 1, width, brushFromPen);
    this.m_isDrawLine = false;
    this.m_clipRegionPoint = PointF.Empty;
  }

  private void DrawCompoundLine(Pen pen, PointF[] points, bool rotate, PdfPen pdfPen)
  {
    float num = 0.0f;
    for (int index = 0; index < pen.CompoundArray.Length; index += 2)
    {
      float width = pen.Width;
      pdfPen.Width = (pen.CompoundArray[index + 1] - pen.CompoundArray[index]) * width;
      if (!rotate)
        this.Graphics.DrawLine(pdfPen, points[0].X, points[0].Y + num, points[1].X, points[1].Y + num);
      else
        this.Graphics.DrawLine(pdfPen, points[0].X + num, points[0].Y, points[1].X + num, points[1].Y);
      if (index + 1 < pen.CompoundArray.Length - 1)
        num += (pen.CompoundArray[index + 2] - pen.CompoundArray[index + 1]) * width + pdfPen.Width;
    }
  }

  private PdfPen ConvertToPen(Pen pen, out float alpha)
  {
    PdfPen pen1;
    try
    {
      pen1 = new PdfPen(pen.Color);
    }
    catch (ArgumentException ex)
    {
      pen1 = new PdfPen(System.Drawing.Color.Empty);
    }
    pen1.DashStyle = PdfEmfRenderer.ConvertDashStyle(pen.DashStyle);
    if (pen1.DashStyle != PdfDashStyle.Solid)
    {
      pen1.DashOffset = pen.DashOffset;
      pen1.DashPattern = pen.DashPattern;
    }
    pen1.LineCap = PdfEmfRenderer.ConvertCaps(pen.StartCap);
    pen1.LineCap = PdfEmfRenderer.ConvertCaps(pen.EndCap);
    pen1.LineJoin = PdfEmfRenderer.ConvertJoin(pen.LineJoin);
    pen1.MiterLimit = pen.MiterLimit;
    pen1.Width = pen.Width;
    try
    {
      alpha = (float) pen.Color.A / (float) byte.MaxValue;
    }
    catch (ArgumentException ex)
    {
      alpha = 0.0f;
    }
    if (pen.Brush != null)
    {
      if (pen.Brush is LinearGradientBrush)
        this.m_gradientPen = true;
      float alpha1;
      PdfBrush pdfBrush = this.ConvertBrush(pen.Brush, out alpha1);
      if (pdfBrush is PdfSolidBrush pdfSolidBrush && pdfSolidBrush.Color.A != (byte) 0 || pdfSolidBrush == null)
      {
        pen1.Brush = pdfBrush;
        alpha = alpha1;
      }
      this.m_gradientPen = false;
    }
    return pen1;
  }

  public void DrawPath(Pen pen, GraphicsPath path)
  {
    if (pen == null)
      throw new ArgumentNullException(nameof (pen));
    if (path == null)
      throw new ArgumentNullException(nameof (path));
    this.OnDrawPrimitive();
    PdfPen pen1 = this.ConvertPen(pen);
    if ((!(pen1.Brush is PdfTilingBrush) || path.FillMode != FillMode.Alternate) && path.FillMode == FillMode.Alternate && pen.Color.A == (byte) 0)
      return;
    PdfPath path1 = new PdfPath(path.PathPoints, path.PathTypes);
    if (path.FillMode == FillMode.Alternate)
      path1.FillMode = PdfFillMode.Alternate;
    this.Graphics.DrawPath(pen1, path1);
  }

  public void DrawPolygon(Pen pen, PointF[] points)
  {
    if (pen == null)
      throw new ArgumentNullException(nameof (pen));
    if (points == null)
      throw new ArgumentNullException(nameof (points));
    this.OnDrawPrimitive();
    this.Graphics.DrawPolygon(this.ConvertPen(pen), points);
  }

  public void DrawPie(Pen pen, RectangleF rect, float startAngle, float sweepAngle)
  {
    if (pen == null)
      throw new ArgumentNullException(nameof (pen));
    this.OnDrawPrimitive();
    this.Graphics.DrawPie(this.ConvertPen(pen), rect, startAngle, sweepAngle);
  }

  public void DrawRectangles(Pen pen, RectangleF[] rects)
  {
    if (pen == null)
      throw new ArgumentNullException(nameof (pen));
    if (rects == null)
      throw new ArgumentNullException(nameof (rects));
    this.OnDrawPrimitive();
    PdfPen pen1 = this.ConvertPen(pen);
    for (int index = 0; index < rects.Length; ++index)
    {
      RectangleF rect = rects[index];
      bool isEmf = this.Graphics.m_isEMF;
      if ((double) rect.Width == (double) this.EmfWidth)
        this.Graphics.m_isEMF = true;
      this.Graphics.DrawRectangle(pen1, rect);
      this.Graphics.m_isEMF = isEmf;
    }
  }

  public void DrawString(string text, Font font, Brush brush, RectangleF rect)
  {
    if (text == null)
      throw new ArgumentNullException(nameof (text));
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    if (brush == null)
      throw new ArgumentNullException(nameof (brush));
    this.OnDrawPrimitive();
    PdfBrush brush1 = this.ConvertBrush(brush);
    PdfFont pdfFont;
    try
    {
      pdfFont = this.GetPdfFont(text, font);
    }
    catch (Exception ex)
    {
      pdfFont = (PdfFont) new PdfTrueTypeFont(this.GetInstalledFontLocation(font), font.Size, (PdfFontStyle) font.Style, false, this.IsEmbedCompleteFonts, true);
    }
    SizeF textSize;
    float num = this.ScaleText(text, pdfFont, rect, out textSize, (PdfStringFormat) null);
    if (this.m_taggedPDF && !this.CheckPdfPage(new RectangleF(rect.Location, new SizeF(rect.Width, Math.Max(rect.Height, pdfFont.Height))), false))
      return;
    if ((double) num != 1.0)
    {
      PdfStringFormat format = new PdfStringFormat();
      format.HorizontalScalingFactor = num * 100f;
      rect.Width /= num;
      format.ComplexScript = this.ComplexScript;
      if ((double) rect.Width == 0.0 && (double) rect.Height == 0.0)
      {
        PointF point = this.CorrectLocation(rect.Location, rect.Size, textSize, format);
        this.Graphics.DrawString(text, pdfFont, brush1, point, format);
      }
      else
      {
        if ((double) rect.Height == 0.0)
          rect.Height = (float) font.Height;
        this.Graphics.DrawString(text, pdfFont, brush1, rect, format);
      }
    }
    else
    {
      PdfStringFormat format = new PdfStringFormat();
      format.ComplexScript = this.ComplexScript;
      if ((double) rect.Width == 0.0 && (double) rect.Height == 0.0)
      {
        PointF point = this.CorrectLocation(rect.Location, rect.Size, textSize, new PdfStringFormat());
        this.Graphics.DrawString(text, pdfFont, brush1, point, format);
      }
      else
      {
        if ((double) rect.Height == 0.0)
          rect.Height = (float) font.Height;
        this.Graphics.DrawString(text, pdfFont, brush1, rect, format);
      }
    }
  }

  public void DrawString(
    string text,
    Font font,
    Brush brush,
    RectangleF rect,
    StringFormat format)
  {
    if (text == null)
      throw new ArgumentNullException(nameof (text));
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    if (brush == null)
      throw new ArgumentNullException(nameof (brush));
    if (format == null)
      throw new ArgumentNullException(nameof (format));
    if (this.m_recordType == EmfPlusRecordType.EmfExtTextOutW && this.m_EMFState)
    {
      if ((double) this.Transform.Elements[3] == -1.0)
        this.Graphics.TranslateTransform(0.0f, 0.0f, this.m_EMFState);
      this.m_EMFState = false;
    }
    this.FontInfo = font;
    this.OnDrawPrimitive();
    PdfBrush brush1 = this.ConvertBrush(brush);
    PdfStringFormat format1 = this.ConvertFormat(format);
    if (format1 != null)
      format1.ComplexScript = this.ComplexScript;
    this.m_isRTLFormat = format1.TextDirection == PdfTextDirection.RightToLeft;
    PdfFont pdfFont;
    try
    {
      if (format1.RightToLeft && !PdfString.IsUnicode(text) && !this.EmbedFonts)
      {
        pdfFont = this.GetPdfFont(font, true);
        format1.TextDirection = PdfTextDirection.RightToLeft;
      }
      else
        pdfFont = this.GetPdfFont(text, font);
    }
    catch (Exception ex)
    {
      pdfFont = (PdfFont) new PdfTrueTypeFont(this.GetInstalledFontLocation(font), font.Size, (PdfFontStyle) font.Style, false, this.IsEmbedCompleteFonts, true);
    }
    PdfTrueTypeFont pdfTrueTypeFont1 = pdfFont as PdfTrueTypeFont;
    if (pdfTrueTypeFont1.Unicode && text.Length > 1 && text.Contains(' '.ToString()))
    {
      char[] charArray = text.ToCharArray();
      for (int index = 0; index < charArray.Length; ++index)
      {
        if (charArray[index] == ' ')
          charArray[index] = ' ';
      }
      text = new string(charArray);
    }
    if (pdfTrueTypeFont1 != null && pdfTrueTypeFont1.TtfReader != null && pdfTrueTypeFont1.TtfReader.isOTFFont() && format1 != null)
      format1.ComplexScript = true;
    SizeF textSize = rect.Size;
    if (format1 != null && format1.Alignment.Equals((object) PdfTextAlignment.Right) && !this.IsRTLCharacters(text) && this.IsEnglish(text))
      rect.X -= font.Size;
    float num1 = this.ScaleText(text, pdfFont, rect, out textSize, format1);
    if ((double) num1 != 1.0)
    {
      if (format1 == null)
      {
        format1 = new PdfStringFormat();
        format1.ComplexScript = this.ComplexScript;
      }
      if (this.m_isIntersectClipRect)
      {
        if (this.m_previousRecordtype != EmfPlusRecordType.EmfIntersectClipRect)
          format1.HorizontalScalingFactor = num1 * 100f;
        this.m_isIntersectClipRect = false;
      }
      else
        format1.HorizontalScalingFactor = num1 * 100f;
    }
    StringFormatFlags stringFormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
    bool flag1 = (format.FormatFlags & stringFormatFlags) == stringFormatFlags && format.Trimming == StringTrimming.None && format.Alignment == StringAlignment.Near && format.LineAlignment == StringAlignment.Near;
    if ((double) rect.Width > (double) pdfFont.MeasureString(text, format1).Width && text.Split((char[]) null).Length > 0)
    {
      char[] spaces = StringTokenizer.Spaces;
      StringTokenizer.GetCharsCount(text, spaces);
      string str1 = text.Trim();
      string str2 = text.Substring(str1.Length);
      bool flag2 = false;
      int num2 = 160 /*0xA0*/;
      foreach (char ch in str2)
      {
        if (num2 == (int) ch)
        {
          flag2 = true;
          break;
        }
      }
      bool flag3 = flag2 && font.Name == "PMingLiU";
      float num3 = 0.0f;
      float num4 = 0.0f;
      if (pdfFont is PdfTrueTypeFont pdfTrueTypeFont2 && pdfTrueTypeFont2.Unicode)
      {
        string postScriptName = pdfTrueTypeFont2.m_fontInternal.Metrics.PostScriptName;
        bool flag4 = false;
        if (postScriptName != null && postScriptName.ToLower().Contains("bold"))
          flag4 = true;
        if (pdfTrueTypeFont2.m_fontInternal.Metrics.IsBold != font.Bold && font.Bold && !flag4)
        {
          SizeF sizeF = this.m_grCache.MeasureString(text, font, new PointF(0.0f, 0.0f), format);
          Font font1 = new Font(font.Name, font.Size, FontStyle.Regular);
          num4 = this.m_grCache.MeasureString(text, font1, new PointF(0.0f, 0.0f), format).Width;
          num3 = (sizeF.Width - num4) / (float) text.Length;
        }
      }
      if (format1.Alignment == PdfTextAlignment.Justify || format1.LineAlignment == PdfVerticalAlignment.Middle || flag1)
      {
        num4 = pdfFont.MeasureString(text, format1).Width;
        num3 = (rect.Width - num4) / (float) text.Length;
      }
      if ((double) num3 < (double) num4 && !flag3)
      {
        format1.CharacterSpacing = num3;
        if ((double) pdfFont.GetLineWidth(text, format1) > (double) rect.Width)
          format1.CharacterSpacing = 0.0f;
      }
    }
    if (this.m_taggedPDF && !this.CheckPdfPage(new RectangleF(rect.Location, new SizeF(rect.Width, Math.Max(rect.Height, pdfFont.Height))), false))
      return;
    if ((double) rect.Width == 0.0 && (double) rect.Height == 0.0 || flag1 && !this.Graphics.m_isBaselineFormat)
    {
      PointF point = this.CorrectLocation(rect.Location, rect.Size, textSize, format1);
      this.Graphics.DrawString(text, pdfFont, brush1, point, format1);
    }
    else
    {
      rect.Width /= num1;
      if ((double) rect.Width == 0.0)
        rect.Width = textSize.Width;
      else if ((double) rect.Height == 0.0)
        rect.Height = textSize.Height;
      if ((double) rect.Width <= 0.0)
        return;
      if ((double) this.m_textClip.X != 0.0 && (double) this.m_textClip.Y != 0.0 && (double) this.m_textClip.Width != 0.0 && (double) this.m_textClip.Height != 0.0)
      {
        float num5 = (double) this.m_textClip.Y > (double) rect.Y ? this.m_textClip.Y - rect.Y : rect.Y - this.m_textClip.Y;
        float num6 = (double) this.m_textClip.Height > (double) rect.Height ? this.m_textClip.Height - rect.Height : rect.Height - this.m_textClip.Height;
        if ((double) this.m_textClip.Width < (double) rect.Width / 2.0 && ((double) this.m_textClip.X >= (double) rect.X || (double) num5 == (double) num6))
          return;
        this.Graphics.DrawString(text, pdfFont, brush1, rect, format1);
        this.m_textClip = RectangleF.Empty;
      }
      else
      {
        if (this.Graphics.m_isEMF && !this.Graphics.m_emfScalingFactor.IsEmpty && !this.m_isUnicodeString)
          this.Graphics.m_isEmfTextScaled = true;
        this.Graphics.DrawString(text, pdfFont, brush1, rect, format1);
      }
    }
  }

  private bool IsEnglish(string word)
  {
    char ch = word.Length > 0 ? word[0] : char.MinValue;
    return ch >= char.MinValue && ch < 'ÿ';
  }

  private bool IsRTLCharacters(string text)
  {
    bool flag = false;
    foreach (char input in text.ToCharArray())
    {
      if (this.IsRTLChar(input))
      {
        flag = true;
        break;
      }
    }
    return flag;
  }

  private bool IsRTLChar(char input)
  {
    bool flag = false;
    int num = (int) input;
    if (num >= 1424 && num <= 1535 /*0x05FF*/)
      flag = true;
    else if (num >= 1536 /*0x0600*/ && num <= 1791 /*0x06FF*/ || num >= 1872 && num <= 1919 || num >= 2208 && num <= 2303 /*0x08FF*/ || num >= 64336 && num <= 65279 || num >= 126464 && num <= 126719)
      flag = true;
    else if (num >= 67648 && num <= 67679)
      flag = true;
    else if (num >= 66464 && num <= 66527)
      flag = true;
    return flag;
  }

  private bool CheckPdfPage(RectangleF rect, bool image)
  {
    if (this.m_taggedPDF && this.Graphics.Page is PdfPage)
    {
      float height = (this.Graphics.Page as PdfPage).GetClientSize().Height;
      float width = (this.Graphics.Page as PdfPage).GetClientSize().Width;
      PdfUnitConvertor pdfUnitConvertor = new PdfUnitConvertor();
      float pixels = pdfUnitConvertor.ConvertToPixels(height, PdfGraphicsUnit.Point);
      pdfUnitConvertor.ConvertToPixels(width, PdfGraphicsUnit.Point);
      if (!image)
        this.TextRegions.Add(new TextRegion(rect.Location.Y, rect.Height));
      if ((double) pixels < (double) rect.Bottom && (!image || (double) rect.Height <= (double) pixels))
      {
        this.m_graphics.Split = (double) this.m_graphics.Split <= 0.0 ? pdfUnitConvertor.ConvertFromPixels(rect.Y, PdfGraphicsUnit.Point) : Math.Min(this.m_graphics.Split, pdfUnitConvertor.ConvertFromPixels(rect.Y, PdfGraphicsUnit.Point));
        return false;
      }
    }
    return true;
  }

  public void DrawString(
    string text,
    Font font,
    Brush brush,
    RectangleF rect,
    StringFormat format,
    float textAngle)
  {
    this.TextAngleLocal = textAngle;
    if (text == null)
      throw new ArgumentNullException(nameof (text));
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    if (brush == null)
      throw new ArgumentNullException(nameof (brush));
    if (format == null)
      throw new ArgumentNullException(nameof (format));
    this.OnDrawPrimitive();
    PdfBrush brush1 = this.ConvertBrush(brush);
    PdfStringFormat format1 = this.ConvertFormat(format);
    if (format1 != null)
      format1.ComplexScript = this.ComplexScript;
    PdfFont pdfFont;
    try
    {
      pdfFont = this.GetPdfFont(text, font);
    }
    catch (Exception ex)
    {
      pdfFont = (PdfFont) new PdfTrueTypeFont(this.GetInstalledFontLocation(font), font.Size, (PdfFontStyle) font.Style, false, this.IsEmbedCompleteFonts, true);
    }
    SizeF textSize = rect.Size;
    float num1 = this.ScaleText(text, pdfFont, rect, out textSize, format1);
    if ((double) num1 != 1.0)
    {
      if (format1 == null)
        format1 = new PdfStringFormat();
      format1.HorizontalScalingFactor = num1 * 100f;
    }
    StringFormatFlags stringFormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
    bool flag = (format.FormatFlags & stringFormatFlags) == stringFormatFlags && format.Trimming == StringTrimming.None && format.Alignment == StringAlignment.Near && format.LineAlignment == StringAlignment.Near;
    if ((double) rect.Width > (double) pdfFont.MeasureString(text, format1).Width && text.Split((char[]) null).Length > 1)
    {
      char[] spaces = StringTokenizer.Spaces;
      StringTokenizer.GetCharsCount(text, spaces);
      float width = pdfFont.MeasureString(text, format1).Width;
      float num2 = (rect.Width - width) / (float) text.Length;
      format1.CharacterSpacing = num2;
    }
    if (this.m_taggedPDF && !this.CheckPdfPage(new RectangleF(rect.Location, new SizeF(rect.Width, Math.Max(rect.Height, pdfFont.Height))), false))
      return;
    if ((double) rect.Width == 0.0 && (double) rect.Height == 0.0 || flag)
    {
      PointF pointF = this.CorrectLocation(rect.Location, rect.Size, textSize, format1);
      if (this.EmbedFonts)
        format1.RightToLeft = false;
      if ((double) this.m_translateTransform.Width > (double) pointF.X)
      {
        pointF.X = this.m_translateTransform.Width;
        if ((double) textAngle == 90.0)
        {
          textAngle = -textAngle;
          pointF.X = -pointF.X;
        }
      }
      this.Graphics.Save();
      this.Graphics.TranslateTransform(pointF.X, pointF.Y);
      this.Graphics.RotateTransform(textAngle);
      this.Graphics.DrawString(text, pdfFont, brush1, PointF.Empty);
      this.Graphics.Restore();
    }
    else
    {
      rect.Width /= num1;
      if ((double) rect.Width == 0.0)
        rect.Width = textSize.Width;
      else if ((double) rect.Height == 0.0)
        rect.Height = textSize.Height;
      PointF pointF = this.CorrectLocation(rect.Location, rect.Size, textSize, format1);
      if ((double) textAngle == 90.0)
        textAngle = -textAngle;
      if ((double) textAngle == -90.0)
      {
        pointF.X += (float) (-(double) pdfFont.Metrics.GetDescent(format1) / 2.0);
        if (this.isPositiveLogicPoints)
          pointF.Y -= (float) (-(double) pdfFont.Metrics.GetDescent(format1) / 2.0);
      }
      if ((double) rect.Width <= 0.0)
        return;
      this.Graphics.Save();
      if ((double) textAngle == 0.0 || (double) textAngle == 360.0)
      {
        this.Graphics.DrawString(text, pdfFont, brush1, rect, format1);
      }
      else
      {
        this.Graphics.TranslateTransform(pointF.X, pointF.Y);
        this.Graphics.RotateTransform(textAngle);
        this.Graphics.DrawString(text, pdfFont, brush1, PointF.Empty);
      }
      this.Graphics.Restore();
    }
  }

  public void DrawString(string text, Font font, Brush brush, RectangleF rect, float textAngle)
  {
    this.TextAngleLocal = textAngle;
    if (text == null)
      throw new ArgumentNullException(nameof (text));
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    if (brush == null)
      throw new ArgumentNullException(nameof (brush));
    this.OnDrawPrimitive();
    PdfBrush brush1 = this.ConvertBrush(brush);
    PdfFont font1;
    try
    {
      font1 = this.GetPdfFont(text, font);
    }
    catch (Exception ex)
    {
      font1 = (PdfFont) new PdfTrueTypeFont(this.GetInstalledFontLocation(font), font.Size, (PdfFontStyle) font.Style, false, this.IsEmbedCompleteFonts, true);
    }
    SizeF size = rect.Size;
    if (this.m_taggedPDF && !this.CheckPdfPage(new RectangleF(rect.Location, new SizeF(rect.Width, Math.Max(rect.Height, font1.Height))), false))
      return;
    if ((double) rect.Width == 0.0 && (double) rect.Height == 0.0)
    {
      PointF location = rect.Location;
      if ((double) this.m_translateTransform.Width > (double) location.X)
        location.X = this.m_translateTransform.Width;
      this.Graphics.Save();
      this.Graphics.TranslateTransform(location.X, location.Y);
      this.Graphics.RotateTransform(textAngle);
      this.Graphics.DrawString(text, font1, brush1, PointF.Empty);
      this.Graphics.Restore();
    }
    else
    {
      if ((double) rect.Width == 0.0)
        rect.Width = size.Width;
      else if ((double) rect.Height == 0.0)
        rect.Height = size.Height;
      PointF location = rect.Location;
      if ((double) textAngle == 90.0)
        textAngle = -textAngle;
      if ((double) rect.Width <= 0.0)
        return;
      this.Graphics.Save();
      this.Graphics.TranslateTransform(location.X, location.Y);
      this.Graphics.RotateTransform(textAngle);
      this.Graphics.DrawString(text, font1, brush1, PointF.Empty);
      this.Graphics.Restore();
    }
  }

  private PointF CorrectLocation(
    PointF location,
    SizeF size,
    SizeF realSize,
    PdfStringFormat format)
  {
    PointF pointF = location;
    if ((double) this.TextAngleLocal == 0.0)
    {
      switch (format.Alignment)
      {
        case PdfTextAlignment.Center:
          pointF.X += size.Width / 2f;
          break;
        case PdfTextAlignment.Right:
          if ((double) size.Width > (double) realSize.Width)
          {
            pointF.X += size.Width - realSize.Width;
            break;
          }
          break;
      }
      switch (format.LineAlignment)
      {
        case PdfVerticalAlignment.Middle:
          pointF.Y += size.Height / 2f;
          break;
        case PdfVerticalAlignment.Bottom:
          if ((double) size.Height > (double) realSize.Height)
          {
            pointF.Y += size.Height - realSize.Height;
            break;
          }
          break;
      }
    }
    return pointF;
  }

  public void EndContainer(GraphicsContainer container)
  {
    if (container == null)
      throw new ArgumentNullException(nameof (container));
    this.Graphics.PutComment(nameof (EndContainer));
    PdfGraphicsState state = this.m_graphicsContainer[container];
    if (state != null)
      this.Graphics.Restore(state);
    this.NativeGraphics.EndContainer(container);
    this.m_stateChanged = true;
    this.m_bFirstCall = true;
    this.m_bFirstTransform = true;
    this.Transform = this.Transform;
  }

  public void FillClosedCurve(Brush brush, PointF[] points, FillMode fillMode, float tension)
  {
    if (brush == null)
      throw new ArgumentNullException(nameof (brush));
    this.Graphics.DrawPolygon(this.ConvertBrush(brush), points);
  }

  public void FillEllipse(Brush brush, RectangleF rect)
  {
    if (brush == null)
      throw new ArgumentNullException(nameof (brush));
    this.m_brushRectangle = rect;
    this.OnDrawPrimitive();
    this.Graphics.DrawEllipse(this.ConvertBrush(brush), rect);
  }

  public void FillPath(Brush brush, GraphicsPath path)
  {
    if (brush == null)
      throw new ArgumentNullException(nameof (brush));
    if (path == null)
      throw new ArgumentNullException(nameof (path));
    bool empty = false;
    this.IsEmptyBrushColor(brush, out empty);
    if (empty)
      return;
    this.m_fillPath = path;
    this.OnDrawPrimitive();
    byte[] pathTypes = path.PathTypes;
    float x = path.PathPoints[0].X;
    float y = path.PathPoints[0].Y;
    for (int index = 1; index < pathTypes.Length - 1; ++index)
    {
      if (pathTypes[index] == (byte) 1 && pathTypes[index - 1] == (byte) 3 && pathTypes[index + 1] == (byte) 3 && (double) x > 0.0 && (double) y < 0.0 && (double) x > (double) path.PathPoints[index].X && (double) y > (double) path.PathPoints[index].Y)
        pathTypes[index] = (byte) 0;
    }
    RectangleF bounds = path.GetBounds();
    if ((double) bounds.X > 0.0 && (double) bounds.Y > 0.0 && (double) bounds.Width <= (double) this.Graphics.ClientSize.Width && (double) bounds.Height <= (double) this.Graphics.ClientSize.Height)
    {
      this.m_brushRectangle = bounds;
    }
    else
    {
      this.negativeRegion = true;
      this.m_brushRectangle = RectangleF.Empty;
    }
    PdfBrush brush1 = this.ConvertBrush(brush);
    PdfPath path1 = new PdfPath(path.PathPoints, pathTypes);
    if (path.FillMode == FillMode.Alternate)
      path1.FillMode = PdfFillMode.Alternate;
    this.Graphics.DrawPath(brush1, path1);
    this.negativeRegion = false;
  }

  public void FillPie(
    Brush brush,
    float x,
    float y,
    float width,
    float height,
    float startAngle,
    float sweepAngle)
  {
    if (brush == null)
      throw new ArgumentNullException(nameof (brush));
    this.OnDrawPrimitive();
    RectangleF rectangle = new RectangleF(x, y, width, height);
    this.Graphics.DrawPie(this.ConvertBrush(brush), rectangle, startAngle, sweepAngle);
  }

  public void FillPolygon(Brush brush, PointF[] points)
  {
    if (brush == null)
      throw new ArgumentNullException(nameof (brush));
    if (points == null)
      throw new ArgumentNullException(nameof (points));
    this.OnDrawPrimitive();
    byte[] types = new byte[points.Length];
    for (int index = 0; index < types.Length; ++index)
      types[index] = (byte) 1;
    this.m_fillPath = new GraphicsPath(points, types);
    RectangleF bounds = this.m_fillPath.GetBounds();
    if ((double) bounds.X > 0.0 && (double) bounds.Y > 0.0 && (double) bounds.Width <= (double) this.Graphics.ClientSize.Width && (double) bounds.Height <= (double) this.Graphics.ClientSize.Height)
    {
      this.m_brushRectangle = bounds;
    }
    else
    {
      this.negativeRegion = true;
      this.m_brushRectangle = RectangleF.Empty;
    }
    this.Graphics.DrawPolygon(this.ConvertBrush(brush), points);
    this.negativeRegion = false;
  }

  public void FillRectangles(Brush brush, RectangleF[] rects)
  {
    if (brush == null)
      throw new ArgumentNullException(nameof (brush));
    this.m_brushRectangle = rects != null ? rects[0] : throw new ArgumentNullException(nameof (rects));
    bool flag = false;
    if (!this.StateChanged && this.NativeGraphics.ClipBounds == RectangleF.Empty && this.previousClipBounds != null)
    {
      flag = true;
      GraphicsPath path = new GraphicsPath();
      path.AddLines(this.previousClipBounds);
      this.Graphics.SetClip(new PdfPath(path.PathPoints, path.PathTypes), this.GetPathFillMode(path));
      path.Dispose();
    }
    this.OnDrawPrimitive();
    PdfBrush brush1 = this.ConvertBrush(brush);
    if (!this.m_isClippedPath || !(brush1 is PdfLinearGradientBrush))
    {
      for (int index = 0; index < rects.Length; ++index)
      {
        RectangleF rect = rects[index];
        bool isEmf = this.Graphics.m_isEMF;
        if ((double) rect.Width == (double) this.EmfWidth)
          this.Graphics.m_isEMF = true;
        this.Graphics.DrawRectangle(brush1, rect);
        this.Graphics.m_isEMF = isEmf;
      }
    }
    else if (this.m_isClippedPath && this.m_graphicsPath != null && brush1 is PdfLinearGradientBrush)
    {
      this.ResetTransform();
      PdfPath path = new PdfPath(this.m_graphicsPath.PathPoints, this.m_graphicsPath.PathTypes);
      this.Graphics.DrawPath(brush1, path);
      this.m_isClippedPath = false;
    }
    if (!flag)
      return;
    this.StateChanged = false;
    this.NativeGraphics.Clip = new Region(RectangleF.Empty);
  }

  public void FillRegion(Brush brush, Region region)
  {
    if (brush == null)
      throw new ArgumentNullException(nameof (brush));
    if (region == null)
      throw new ArgumentNullException(nameof (region));
    this.OnDrawPrimitive();
    Matrix transform = this.NativeGraphics.Transform;
    RectangleF[] regionScans = region.GetRegionScans(transform);
    this.FillRectangles(brush, regionScans);
  }

  public void MultiplyTransform(Matrix matrix, MatrixOrder order)
  {
    if (matrix == null)
      throw new ArgumentNullException(nameof (matrix));
    this.NativeGraphics.MultiplyTransform(matrix, order);
    if (this.isinnerEMF && this.m_previousRecordtype == EmfPlusRecordType.Save)
    {
      this.Graphics.Restore();
      this.Graphics.Restore();
    }
    this.Graphics.PutComment("MuliplyTransform");
    this.m_multiplyTransform = matrix;
    if (order == MatrixOrder.Append)
      this.Graphics.MultiplyTransform(PdfEmfRenderer.PrepareMatrix(matrix));
    else
      this.Transform = this.NativeGraphics.Transform;
    this.m_multiplyTransformMatrix = this.NativeGraphics.Transform;
    this.m_stateChanged = true;
  }

  internal void SetSecondPageScalling(float value)
  {
    this.Graphics.PutComment("PageScale property");
    this.SetTransform();
    this.Graphics.ScaleTransform(value, value);
  }

  public void TranslateClip(float dx, float dy)
  {
    this.NativeGraphics.TranslateClip(dx, dy);
    this.m_stateChanged = true;
  }

  public void ResetClip()
  {
    this.NativeGraphics.ResetClip();
    this.m_stateChanged = true;
    this.m_textClip = RectangleF.Empty;
    this.m_clipPath = false;
  }

  public void ResetTransform()
  {
    this.Graphics.PutComment(nameof (ResetTransform));
    this.m_clipPath = false;
    this.InternalResetClip();
    this.SetTransform();
    this.NativeGraphics.ResetTransform();
  }

  public void RotateTransform(float angle, MatrixOrder order)
  {
    this.Graphics.PutComment(nameof (RotateTransform));
    this.NativeGraphics.RotateTransform(angle, order);
    if (order == MatrixOrder.Append)
      this.Graphics.RotateTransform(angle);
    else
      this.Transform = this.Transform;
  }

  public GraphicsState Save()
  {
    this.Graphics.PutComment(nameof (Save));
    this.InternalResetClip();
    this.InternalResetTransformation();
    PdfGraphicsState pdfGraphicsState = this.Graphics.Save();
    GraphicsState key = this.NativeGraphics.Save();
    this.m_graphicsStates[key] = pdfGraphicsState;
    return key;
  }

  public void Restore(GraphicsState gState)
  {
    if (this.m_graphicsStates.Count <= 0)
      return;
    this.Graphics.PutComment(nameof (Restore));
    if (!this.m_graphicsStates.ContainsKey(gState))
      gState = (GraphicsState) new ArrayList((ICollection) this.m_graphicsStates.Keys)[0];
    PdfGraphicsState graphicsState = this.m_graphicsStates[gState];
    if (this.m_onDrawPrimitiveMultiplyTransform && this.m_previousRecordtype == EmfPlusRecordType.Restore)
    {
      this.m_onDrawPrimitiveMultiplyTransform = false;
      this.m_restoreState = true;
    }
    bool flag = false;
    if (this.m_previousRecordtype == EmfPlusRecordType.SetWorldTransform)
      flag = true;
    if (graphicsState != null && !flag)
      this.Graphics.Restore(graphicsState);
    this.NativeGraphics.Restore(gState);
    this.m_stateChanged = true;
    this.m_bFirstCall = true;
    this.m_bFirstTransform = false;
    this.m_stateRestored = true;
    this.m_graphicsStates.Remove(gState);
  }

  public void ScaleTransform(float sx, float sy, MatrixOrder order)
  {
    this.Graphics.PutComment(nameof (ScaleTransform));
    this.NativeGraphics.ScaleTransform(sx, sy, order);
    if (order == MatrixOrder.Append)
    {
      if ((double) this.Transform.OffsetX == 0.0 && (double) this.Transform.OffsetY == 0.0)
        this.Graphics.ScaleTransform(sx, sy);
      else
        this.Transform = this.Transform;
    }
    else
      this.Transform = this.Transform;
  }

  public void SetClip(GraphicsPath path, CombineMode mode)
  {
    this.NativeGraphics.SetClip(path, mode);
    this.SetClip();
  }

  public void SetClip(RectangleF rect, CombineMode mode)
  {
    this.NativeGraphics.SetClip(rect, mode);
    this.RealClip = rect;
    this.m_textClip = rect;
    this.SetClip();
  }

  public void SetClip(Region region, CombineMode mode)
  {
    this.RealClip = RectangleF.Empty;
    this.m_clipRegionPoint = region.GetBounds(this.m_grCache).Location;
    this.NativeGraphics.SetClip(region, mode);
    this.SetClip();
  }

  public void ExcludeClip(Rectangle rect)
  {
    this.NativeGraphics.ExcludeClip(rect);
    this.SetClip();
  }

  public void ExcludeClip(Region region)
  {
    this.NativeGraphics.ExcludeClip(region);
    this.SetClip();
  }

  public void IntersectClip(RectangleF rect)
  {
    this.NativeGraphics.IntersectClip(rect);
    this.m_isIntersectClipRect = true;
    this.SetClip();
  }

  public void IntersectClip(Region region)
  {
    this.NativeGraphics.IntersectClip(region);
    this.SetClip();
  }

  public void TransformPoints(CoordinateSpace destSpace, CoordinateSpace srcSpace, PointF[] pts)
  {
    this.NativeGraphics.TransformPoints(destSpace, srcSpace, pts);
  }

  public void SetRenderingOrigin(Point origin)
  {
    this.Graphics.PutComment(nameof (SetRenderingOrigin));
    this.NativeGraphics.RenderingOrigin = origin;
    if (this.NativeGraphics.TextRenderingHint == TextRenderingHint.SystemDefault)
      return;
    this.Graphics.TranslateTransform((float) -origin.X, (float) -origin.Y);
    this.SetTransform();
  }

  public void SetTransform(Matrix matrix)
  {
    if (matrix == null)
      throw new ArgumentNullException(nameof (matrix));
    this.Graphics.PutComment("SetTransform( matrix )");
    this.InternalResetClip();
    this.Transform = matrix;
  }

  public void TranslateTransform(float dx, float dy, MatrixOrder order)
  {
    this.Graphics.PutComment(nameof (TranslateTransform));
    if (this.NativeGraphics != null)
      this.NativeGraphics.TranslateTransform(dx, dy, order);
    this.m_isTranslate = true;
    if (order == MatrixOrder.Append)
      this.Graphics.TranslateTransform(dx, dy);
    else
      this.Transform = this.Transform;
  }

  public void BeforeStart()
  {
    lock (PdfEmfRenderer.s_bmp)
      this.m_grCache = System.Drawing.Graphics.FromImage(PdfEmfRenderer.s_bmp);
    this.Graphics.PutComment(nameof (BeforeStart));
    this.m_startState = this.Graphics.Save();
    if (this.m_bounds == null || !this.m_multiplyTransformed || !this.isinnerEMF && !this.m_nonScaledRegion)
      return;
    this.m_boundTransformation = true;
    this.Graphics.MultiplyTransform(this.m_bounds);
  }

  public void BeforeEnd()
  {
    if (this.m_startState != null)
    {
      this.Graphics.PutComment(nameof (BeforeEnd));
      this.Graphics.Restore(this.m_startState);
    }
    if (this.m_grCache == null)
      return;
    this.m_grCache.Dispose();
  }

  public void OnError(Exception ex) => this.BeforeEnd();

  public void Dispose()
  {
    this.m_graphics = (PdfGraphics) null;
    this.m_bFirstCall = true;
    this.m_grCache = (System.Drawing.Graphics) null;
    this.m_fontCollection.Clear();
    if (this.Document == null)
      return;
    this.Document.FontCollection.Clear();
  }

  internal int GetCustomFontIndex(string fontName)
  {
    int customFontIndex = -1;
    if (this.CustomFontCollection != null && this.CustomFontCollection.FontCollection != null && fontName != null)
    {
      FontFamily[] families = this.CustomFontCollection.FontCollection.Families;
      for (int index = 0; index < families.Length; ++index)
      {
        if (families[index].Name.ToLower() == fontName.ToLower())
        {
          customFontIndex = index;
          break;
        }
      }
    }
    return customFontIndex;
  }

  internal void SetBounds(PointF location, SizeF size)
  {
    this.m_bounds = new PdfTransformationMatrix();
    if (!size.IsEmpty)
      this.m_bounds.Scale(size);
    if (location.IsEmpty)
      return;
    this.m_bounds.Translate(location.X, -location.Y);
  }

  internal void SetBBox(RectangleF bounds) => this.m_graphics.SetBBox(bounds);

  private void SetTransform()
  {
    this.InternalResetClip();
    this.Graphics.PutComment(nameof (SetTransform));
    this.m_clipPath = false;
    if (!this.m_bFirstTransform && !this.m_stateRestored)
    {
      this.Graphics.Restore();
    }
    else
    {
      this.m_bFirstTransform = false;
      this.m_stateRestored = false;
    }
    this.Graphics.Save();
  }

  private void SetClip() => this.m_stateChanged = true;

  private void SetPdfClipPath()
  {
    GraphicsPath clipPath = this.GetClipPath();
    if (clipPath == null)
      return;
    PointF[] pathPoints = clipPath.PathPoints;
    byte[] pathTypes = clipPath.PathTypes;
    PdfFillMode pathFillMode = this.GetPathFillMode(clipPath);
    if (this.FontInfo != null && (this.FontInfo.Name.Equals("MS PMincho") || this.FontInfo.Name.Equals("Arial Unicode MS")) && this.FontInfo.Italic)
    {
      PointF pointF1 = pathPoints[0];
      pointF1.X -= this.FontInfo.Size / 10f;
      pointF1.Y = pointF1.Y;
      PointF pointF2 = pathPoints[3];
      pointF2.X -= this.FontInfo.Size / 10f;
      pointF2.Y = pointF2.Y;
      for (int index = 0; index < pathPoints.Length; ++index)
      {
        switch (index)
        {
          case 0:
            pathPoints[0] = pointF1;
            break;
          case 3:
            pathPoints[3] = pointF2;
            break;
        }
      }
    }
    this.Graphics.SetClip(new PdfPath(pathPoints, pathTypes), pathFillMode);
  }

  private PdfFillMode GetPathFillMode(GraphicsPath path)
  {
    if (path == null)
      throw new ArgumentNullException(nameof (path));
    return path.FillMode == FillMode.Winding ? PdfFillMode.Winding : PdfFillMode.Alternate;
  }

  private GraphicsPath GetClipPath()
  {
    GraphicsPath clipPath = (GraphicsPath) null;
    Region clip = this.NativeGraphics.Clip;
    RectangleF rect = this.NativeGraphics.ClipBounds;
    if (this.m_isDrawLine && this.m_clipRegionPoint != PointF.Empty && ((double) rect.X > (double) this.m_clipRegionPoint.X || (double) rect.Y > (double) this.m_clipRegionPoint.Y))
      rect = new RectangleF(this.m_clipRegionPoint, rect.Size);
    if (!clip.IsEmpty(this.NativeGraphics) && !clip.IsInfinite(this.NativeGraphics))
    {
      this.m_clipPath = true;
      clipPath = new GraphicsPath(FillMode.Winding);
      if ((double) this.RealClip.X != 0.0 || (double) this.RealClip.Y != 0.0 || (double) this.RealClip.Width != 0.0 || (double) this.RealClip.Height != 0.0)
      {
        if ((int) this.RealClip.Width == (int) rect.Width || (int) this.RealClip.Height == (int) rect.Height)
        {
          clipPath.AddRectangle(this.RealClip);
          this.RealClip = RectangleF.Empty;
        }
        else
        {
          clipPath.AddRectangle(rect);
          this.RealClip = RectangleF.Empty;
        }
      }
      else
      {
        RectangleF[] rects = (double) rect.X >= 0.0 || (double) rect.Y >= 0.0 ? clip.GetRegionScans(new Matrix()) : clip.GetRegionScans(this.NativeGraphics.Transform);
        if (rects.Length > 1)
          clipPath.AddRectangles(rects);
        else
          clipPath.AddRectangle(rect);
      }
    }
    return clipPath;
  }

  private string GetFontStyle(Font font)
  {
    string fontStyle = string.Empty;
    if (font != null)
    {
      switch (font.Style)
      {
        case FontStyle.Regular:
        case FontStyle.Underline:
        case FontStyle.Strikeout:
          fontStyle = FontStyle.Regular.ToString();
          break;
        case FontStyle.Bold:
          fontStyle = FontStyle.Bold.ToString();
          break;
        case FontStyle.Italic:
          fontStyle = FontStyle.Italic.ToString();
          break;
        case FontStyle.Bold | FontStyle.Italic:
        case FontStyle.Bold | FontStyle.Italic | FontStyle.Underline:
        case FontStyle.Bold | FontStyle.Italic | FontStyle.Strikeout:
        case FontStyle.Bold | FontStyle.Italic | FontStyle.Underline | FontStyle.Strikeout:
          fontStyle = FontStyle.Bold.ToString() + FontStyle.Italic.ToString();
          break;
        default:
          fontStyle = FontStyle.Regular.ToString();
          break;
      }
    }
    return fontStyle;
  }

  private PdfFont GetPdfFont(string text, Font font)
  {
    if (text == null)
      throw new ArgumentNullException(nameof (text));
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    bool flag = PdfDocument.ConformanceLevel != PdfConformanceLevel.None || this.IsEmbedFonts || PdfString.IsUnicode(font.Name) || PdfString.IsUnicode(text);
    if (this.ComplexScript || this.m_isRTLFormat)
      flag = true;
    float size = font.Size;
    if (font.Name == "Wingdings" || font.Name == "Symbol" || font.Name.ToLower() == "latha" || font.Name.ToLower() == "shruti" || font.Name.ToLower() == "mangal" || font.Name.ToLower() == "tunga" || font.Name.ToLower() == "vrinda" || font.Name.ToLower() == "swis721 th bt" || font.Name.ToLower() == "swis721 ltex bt" || font.Name.Contains("Helvetica") || font.Name.ToLower() == "frutiger light")
      flag = true;
    if (this.IsEmbedCompleteFonts)
      flag = true;
    PdfFont pdfFont;
    if (this.CustomFontCollection != null && this.CustomFontCollection.EmbeddedFonts != null && this.CustomFontCollection.EmbeddedFonts.ContainsKey($"{font.Name}_{this.GetFontStyle(font)}".ToLower()))
    {
      Stream embeddedFont = this.CustomFontCollection.EmbeddedFonts[$"{font.Name}_{this.GetFontStyle(font)}".ToLower()];
      embeddedFont.Position = 0L;
      pdfFont = (PdfFont) new PdfTrueTypeFont(embeddedFont, size, true, (PdfFontStyle) font.Style);
    }
    else if (!PdfDocument.EnableCache)
    {
      if (this.Document != null)
      {
        string fontKey = this.GetFontKey(font, flag);
        if (this.Document.FontCollection.ContainsKey(fontKey))
        {
          pdfFont = (PdfFont) new PdfTrueTypeFont(this.Document.FontCollection[fontKey], font, true, this.IsEmbedCompleteFonts);
        }
        else
        {
          pdfFont = (PdfFont) new PdfTrueTypeFont(font, size, flag, false, this.IsEmbedCompleteFonts, true);
          this.Document.FontCollection.Add(fontKey, pdfFont as PdfTrueTypeFont);
        }
      }
      else
      {
        string fontKey = this.GetFontKey(font, flag);
        if (this.m_fontCollection.ContainsKey(fontKey))
        {
          pdfFont = (PdfFont) new PdfTrueTypeFont(this.m_fontCollection[fontKey], font, true, this.IsEmbedCompleteFonts);
        }
        else
        {
          pdfFont = (PdfFont) new PdfTrueTypeFont(font, size, flag, false, this.IsEmbedCompleteFonts, true);
          this.m_fontCollection.Add(fontKey, pdfFont as PdfTrueTypeFont);
        }
      }
    }
    else
      pdfFont = (PdfFont) new PdfTrueTypeFont(font, size, flag, false, this.IsEmbedCompleteFonts, true);
    if (flag)
      this.m_isUnicodeString = true;
    return pdfFont;
  }

  internal PdfFont GetPdfFont(Font font, bool isUnicode)
  {
    PdfFont pdfFont;
    if (this.CustomFontCollection != null && this.CustomFontCollection.EmbeddedFonts != null && this.CustomFontCollection.EmbeddedFonts.ContainsKey($"{font.Name}_{this.GetFontStyle(font)}".ToLower()))
    {
      Stream embeddedFont = this.CustomFontCollection.EmbeddedFonts[$"{font.Name}_{this.GetFontStyle(font)}".ToLower()];
      embeddedFont.Position = 0L;
      pdfFont = (PdfFont) new PdfTrueTypeFont(embeddedFont, font.Size, true, (PdfFontStyle) font.Style);
    }
    else if (!PdfDocument.EnableCache)
    {
      if (this.Document != null)
      {
        string fontKey = this.GetFontKey(font, isUnicode);
        if (this.Document.FontCollection.ContainsKey(fontKey))
        {
          pdfFont = (PdfFont) new PdfTrueTypeFont(this.Document.FontCollection[fontKey], font, true, this.IsEmbedCompleteFonts);
        }
        else
        {
          pdfFont = (PdfFont) new PdfTrueTypeFont(font, font.Size, isUnicode, false, this.IsEmbedCompleteFonts, true);
          this.Document.FontCollection.Add(fontKey, pdfFont as PdfTrueTypeFont);
        }
      }
      else
      {
        string fontKey = this.GetFontKey(font, isUnicode);
        if (this.m_fontCollection.ContainsKey(fontKey))
        {
          pdfFont = (PdfFont) new PdfTrueTypeFont(this.m_fontCollection[fontKey], font, true, this.IsEmbedCompleteFonts);
        }
        else
        {
          pdfFont = (PdfFont) new PdfTrueTypeFont(font, font.Size, isUnicode, false, this.IsEmbedCompleteFonts, true);
          this.m_fontCollection.Add(fontKey, pdfFont as PdfTrueTypeFont);
        }
      }
    }
    else
      pdfFont = (PdfFont) new PdfTrueTypeFont(font, font.Size, isUnicode, false, this.IsEmbedCompleteFonts, true);
    return pdfFont;
  }

  private string GetFontKey(Font font, bool isUnicode)
  {
    string empty = string.Empty;
    return font.Style.ToString().ToLower().Contains("underline") ? this.GetUnderlineFontKey(font.Name, font.Style, isUnicode) : font.Name + (object) font.Style + (object) isUnicode;
  }

  private string GetUnderlineFontKey(string fontName, FontStyle style, bool isUnicode)
  {
    string underlineFontKey = string.Empty;
    switch (style)
    {
      case FontStyle.Underline:
        underlineFontKey = fontName + (object) FontStyle.Regular + (object) isUnicode;
        break;
      case FontStyle.Bold | FontStyle.Underline:
        underlineFontKey = fontName + (object) FontStyle.Bold + (object) isUnicode;
        break;
      case FontStyle.Italic | FontStyle.Underline:
        underlineFontKey = fontName + (object) FontStyle.Italic + (object) isUnicode;
        break;
      case FontStyle.Bold | FontStyle.Italic | FontStyle.Underline:
        underlineFontKey = fontName + (object) FontStyle.Bold + (object) FontStyle.Italic + (object) isUnicode;
        break;
    }
    return underlineFontKey;
  }

  private bool IsChineseString(string text) => Regex.IsMatch(text, "[一-龥]");

  private void OnDrawPrimitive()
  {
    if (!this.m_stateChanged)
      return;
    this.InternalResetClip();
    if (object.Equals((object) this.NativeGraphics.Transform, (object) this.m_multiplyTransformMatrix) && this.isinnerEMF || this.m_restoreState && this.m_fillPath != null)
      this.Graphics.Restore();
    this.m_bFirstCall = false;
    this.Graphics.PutComment(nameof (OnDrawPrimitive));
    this.Graphics.Save();
    if (object.Equals((object) this.NativeGraphics.Transform, (object) this.m_multiplyTransformMatrix) && this.isinnerEMF && !this.NativeGraphics.Transform.Equals((object) this.pageUnitScale) || this.m_restoreState && this.m_fillPath != null)
    {
      this.m_onDrawPrimitiveMultiplyTransform = true;
      this.Graphics.MultiplyTransform(new PdfTransformationMatrix()
      {
        Matrix = new Matrix(this.NativeGraphics.Transform.Elements[0], this.NativeGraphics.Transform.Elements[1], this.NativeGraphics.Transform.Elements[2], this.NativeGraphics.Transform.Elements[3], this.NativeGraphics.Transform.Elements[4], -this.NativeGraphics.Transform.Elements[5])
      });
    }
    this.SetPdfClipPath();
    this.m_stateChanged = false;
  }

  private void OnChangeState() => this.m_stateChanged = true;

  private void DrawCap(
    LineCap cap,
    PointF[] points,
    int startPointIndex,
    int endPointIndex,
    float width,
    PdfBrush brush)
  {
    switch (cap)
    {
      case LineCap.Round:
        SizeF size = new SizeF(width, width);
        RectangleF rectangle = new RectangleF(points[endPointIndex], size);
        this.Graphics.DrawEllipse(brush, rectangle);
        break;
      case LineCap.Triangle:
        PointF point1 = points[endPointIndex];
        PointF point2 = points[startPointIndex];
        float x1 = point1.X;
        float y1 = point1.Y;
        float num1 = x1 - point2.X;
        float num2 = y1 - point2.Y;
        float num3 = (float) Math.Sqrt((double) num1 * (double) num1 + (double) num2 * (double) num2);
        float num4 = num1 / num3;
        float num5 = num2 / num3 * width;
        float num6 = num4 * width;
        float x2 = x1 - num5;
        float y2 = y1 + num6;
        float x3 = x1 + num5;
        float y3 = y1 - num6;
        float x4 = x1 + num6;
        float y4 = y1 + num5;
        PointF pointF1 = new PointF(x3, y3);
        PointF pointF2 = new PointF(x2, y2);
        PointF pointF3 = new PointF(x4, y4);
        PointF[] points1 = new PointF[3]
        {
          pointF1,
          pointF3,
          pointF2
        };
        this.Graphics.DrawPolygon(brush, points1);
        break;
      case LineCap.Custom:
        if ((double) this.m_customLineCapArrowData.width == 0.0)
          break;
        this.DrawCustomLineCapArrow(cap, points, startPointIndex, endPointIndex, width, brush);
        this.m_customLineCapArrowData.Reset();
        break;
    }
  }

  private void DrawCustomLineCapArrow(
    LineCap cap,
    PointF[] points,
    int startPointIndex,
    int endPointIndex,
    float width,
    PdfBrush brush)
  {
    PointF point1 = points[endPointIndex];
    PointF point2 = points[startPointIndex];
    double num1 = Math.Cos(Math.PI / 6.0);
    double num2 = Math.Sin(Math.PI / 6.0);
    float x = point1.X;
    float y = point1.Y;
    float num3 = x - point2.X;
    float num4 = y - point2.Y;
    float num5 = (float) Math.Sqrt((double) num3 * (double) num3 + (double) num4 * (double) num4);
    float num6 = num3 / num5;
    float num7 = num4 / num5 * this.m_customLineCapArrowData.width;
    float num8 = num6 * this.m_customLineCapArrowData.width;
    PointF point2_1 = new PointF(x - (float) ((double) num8 * num1 + (double) num7 * -num2), y - (float) ((double) num8 * num2 + (double) num7 * num1));
    PointF point2_2 = new PointF(x - (float) ((double) num8 * num1 + (double) num7 * num2), y - (float) ((double) num8 * -num2 + (double) num7 * num1));
    if (this.m_customLineCapArrowData.fillState == 0)
    {
      PdfPen pen = new PdfPen(brush);
      this.Graphics.DrawLine(pen, new PointF(x, y), point2_1);
      this.Graphics.DrawLine(pen, new PointF(x, y), point2_2);
    }
    else
    {
      if (this.m_customLineCapArrowData.fillState != 1)
        return;
      PointF[] points1 = new PointF[3]
      {
        new PointF(x, y),
        point2_1,
        point2_2
      };
      this.Graphics.DrawPolygon(brush, points1);
    }
  }

  private float ScaleText(
    string text,
    PdfFont pdfFont,
    RectangleF rect,
    out SizeF textSize,
    PdfStringFormat format)
  {
    if (text == null)
      throw new ArgumentNullException(nameof (text));
    if (pdfFont == null)
      throw new ArgumentNullException(nameof (pdfFont));
    float num1 = 1f;
    textSize = rect.Size;
    if (text.Length > 0)
    {
      if (format == null)
        format = new PdfStringFormat();
      if (text.EndsWith(" "))
        format.MeasureTrailingSpaces = format.TextDirection != PdfTextDirection.RightToLeft || text.Length <= 0 || PdfString.IsUnicode(text) || !Regex.IsMatch(text, "[a-z]", RegexOptions.IgnoreCase);
      textSize = pdfFont.MeasureString(text, format);
      if ((double) rect.Width > 0.0 && (double) rect.Width < 2147483648.0 && (double) textSize.Width > (double) rect.Width && (double) textSize.Width > 0.0)
      {
        bool flag = false;
        if (this.NativeGraphics != null && this.NativeGraphics.ClipBounds != RectangleF.Empty && (double) this.NativeGraphics.ClipBounds.Width > 0.0)
        {
          float num2 = (double) rect.X <= (double) this.NativeGraphics.ClipBounds.X ? this.NativeGraphics.ClipBounds.X - rect.X : rect.X - this.NativeGraphics.ClipBounds.X;
          if (format != null && format.MeasureTrailingSpaces && format.WordWrap == PdfWordWrapType.None && (double) textSize.Width > (double) this.NativeGraphics.ClipBounds.Width + (double) num2)
            flag = true;
        }
        if (!flag)
          num1 = rect.Width / textSize.Width;
      }
    }
    return num1;
  }

  private PdfGraphicsUnit GraphicsToPrintUnits(GraphicsUnit gUnits)
  {
    switch (gUnits)
    {
      case GraphicsUnit.Display:
        return PdfGraphicsUnit.Pixel;
      case GraphicsUnit.Pixel:
        return PdfGraphicsUnit.Pixel;
      case GraphicsUnit.Point:
        return PdfGraphicsUnit.Point;
      case GraphicsUnit.Inch:
        return PdfGraphicsUnit.Inch;
      case GraphicsUnit.Document:
        return PdfGraphicsUnit.Document;
      case GraphicsUnit.Millimeter:
        return PdfGraphicsUnit.Millimeter;
      default:
        return PdfGraphicsUnit.Point;
    }
  }

  private PdfPen ConvertPen(Pen pen, out float alpha, bool rotate)
  {
    PdfPen pdfPen;
    try
    {
      pdfPen = new PdfPen(pen.Color);
    }
    catch (ArgumentException ex)
    {
      pdfPen = new PdfPen(System.Drawing.Color.Empty);
    }
    pdfPen.DashStyle = PdfEmfRenderer.ConvertDashStyle(pen.DashStyle);
    if (pdfPen.DashStyle != PdfDashStyle.Solid)
    {
      pdfPen.DashOffset = pen.DashOffset;
      pdfPen.DashPattern = pen.DashPattern;
    }
    pdfPen.LineCap = PdfEmfRenderer.ConvertCaps(pen.StartCap);
    pdfPen.LineCap = PdfEmfRenderer.ConvertCaps(pen.EndCap);
    pdfPen.LineJoin = PdfEmfRenderer.ConvertJoin(pen.LineJoin);
    pdfPen.MiterLimit = pen.MiterLimit;
    pdfPen.Width = pen.Width;
    try
    {
      alpha = (float) pen.Color.A / (float) byte.MaxValue;
    }
    catch (ArgumentException ex)
    {
      alpha = 0.0f;
    }
    if (pen.Brush != null)
    {
      if (pen.Brush is LinearGradientBrush)
        this.m_gradientPen = true;
      float alpha1;
      PdfBrush brush = this.ConvertBrush(pen.Brush, out alpha1);
      if (brush is PdfSolidBrush pdfSolidBrush && pdfSolidBrush.Color.A != (byte) 0 || pdfSolidBrush == null)
      {
        pdfPen.Brush = brush;
        alpha = alpha1;
      }
      if (pen.CompoundArray.Length != 0)
      {
        double width1 = (double) pen.Width;
        float width2 = pen.Width;
        float num = (float) Math.Pow(2.0, Math.Round(Math.Log((double) pen.Width, 2.0)) + 1.0);
        PdfTilingBrush pdfTilingBrush = new PdfTilingBrush(new SizeF(num, num));
        PdfPen pen1 = new PdfPen(brush);
        pen1.Width = (pen.CompoundArray[1] - pen.CompoundArray[0]) * width2;
        if (!rotate)
          pdfTilingBrush.Graphics.DrawLine(pen1, 0.0f, pen1.Width, num, pen1.Width);
        else
          pdfTilingBrush.Graphics.DrawLine(pen1, 0.0f, pen1.Width, 0.0f, num);
        pen1.Width = (pen.CompoundArray[1] - pen.CompoundArray[0]) * width2;
        if (!rotate)
        {
          pdfTilingBrush.Graphics.TranslateTransform(0.0f, pen.Width);
          pdfTilingBrush.Graphics.DrawLine(pen1, 0.0f, pen1.Width / 2f, num, pen1.Width / 2f);
        }
        else
        {
          pdfTilingBrush.Graphics.TranslateTransform(pen.Width, 0.0f);
          pdfTilingBrush.Graphics.DrawLine(pen1, pen1.Width / 2f, pen1.Width, pen1.Width / 2f, num);
        }
        pdfPen.Brush = (PdfBrush) pdfTilingBrush;
      }
      this.m_gradientPen = false;
    }
    if (this.m_brushRectangle != RectangleF.Empty)
      this.m_brushRectangle = RectangleF.Empty;
    if (this.m_fillPath != null)
      this.m_fillPath = (GraphicsPath) null;
    return pdfPen;
  }

  private PdfBrush ConvertBrush(Brush brush, out float alpha)
  {
    PdfBrush pdfBrush = (PdfBrush) null;
    SolidBrush solidBrush = brush as SolidBrush;
    TextureBrush textureBrush = brush as TextureBrush;
    LinearGradientBrush linearGradientBrush1 = brush as LinearGradientBrush;
    HatchBrush hatchBrush = brush as HatchBrush;
    PathGradientBrush pathGradientBrush1 = brush as PathGradientBrush;
    alpha = 1f;
    if (solidBrush != null)
    {
      pdfBrush = (PdfBrush) new PdfSolidBrush((PdfColor) solidBrush.Color);
      alpha = (float) solidBrush.Color.A / (float) byte.MaxValue;
    }
    else if (textureBrush != null)
    {
      Image image1 = textureBrush.Image;
      PdfImage image2 = PdfImage.FromImage(image1);
      PdfTilingBrush pdfTilingBrush = new PdfTilingBrush((SizeF) image1.Size);
      if (image2 is PdfBitmap)
      {
        Bitmap bitmap = image1 as Bitmap;
        PdfBitmap image3 = image2 as PdfBitmap;
        PdfMask pdfMask = this.CheckAlpha(bitmap);
        if (pdfMask != null)
          image3.Mask = pdfMask;
        pdfTilingBrush.Graphics.DrawImage((PdfImage) image3, PointF.Empty, (SizeF) image1.Size);
      }
      else
        pdfTilingBrush.Graphics.DrawImage(image2, PointF.Empty, (SizeF) image1.Size);
      pdfBrush = (PdfBrush) pdfTilingBrush;
    }
    else if (linearGradientBrush1 != null)
    {
      RectangleF rect = linearGradientBrush1.Rectangle;
      float element1 = linearGradientBrush1.Transform.Elements[0];
      float element2 = linearGradientBrush1.Transform.Elements[1];
      float element3 = linearGradientBrush1.Transform.Elements[2];
      float element4 = linearGradientBrush1.Transform.Elements[3];
      float angle = Convert.ToSingle(Math.Round(180.0 / Math.PI * Math.Atan2((double) (element3 - element2), (double) (element1 + element4))));
      if ((double) angle > 0.0)
        angle = 360f - angle;
      if ((double) angle < 0.0)
        angle = (double) angle >= -90.0 ? -angle : (float) -((double) angle + 1.0);
      if (!this.m_clipPath)
      {
        RectangleF rectangleF = new RectangleF();
        if (this.m_brushRectangle != RectangleF.Empty)
        {
          if ((double) this.m_brushRectangle.X == 0.0 && (double) this.m_brushRectangle.Y == 0.0)
            rectangleF = this.m_brushRectangle;
        }
        else if (this.m_fillPath != null)
        {
          float num1 = this.m_fillPath.PathPoints[0].Y;
          float val2_1 = this.m_fillPath.PathPoints[0].Y;
          float num2 = this.m_fillPath.PathPoints[0].X;
          float val2_2 = this.m_fillPath.PathPoints[0].X;
          foreach (PointF pathPoint in this.m_fillPath.PathPoints)
          {
            num1 = Math.Min(pathPoint.Y, num1);
            val2_1 = Math.Max(pathPoint.Y, val2_1);
            num2 = Math.Min(pathPoint.X, num2);
            val2_2 = Math.Max(pathPoint.X, val2_2);
          }
          rectangleF = new RectangleF(num2, num1, val2_2 - num2, val2_1 - num1);
        }
        if ((double) rect.Width < (double) rectangleF.Width)
          rect = rectangleF;
      }
      System.Drawing.Color color1 = System.Drawing.Color.Empty;
      System.Drawing.Color color2 = System.Drawing.Color.Empty;
      System.Drawing.Color[] linearColors = linearGradientBrush1.LinearColors;
      ColorBlend colorBlend = (ColorBlend) null;
      if (linearColors != null)
      {
        color1 = linearColors[0];
        color2 = linearColors[1];
      }
      if (color1.R == (byte) 0 && color1.G == (byte) 0 && color1.B == (byte) 0 && linearGradientBrush1.WrapMode == WrapMode.TileFlipX)
      {
        color1 = linearColors[1];
        color2 = linearColors[0];
      }
      bool flag = false;
      try
      {
        if (linearGradientBrush1.Blend == null)
        {
          colorBlend = linearGradientBrush1.InterpolationColors;
          if (colorBlend != null)
          {
            int num3 = 0;
            int num4 = 0;
            int length = colorBlend.Colors.Length;
            for (int index = 0; index < length; ++index)
            {
              System.Drawing.Color color = colorBlend.Colors[index];
              if (color.A == (byte) 0 && color.R != (byte) 0 && color.G != (byte) 0 && color.B != (byte) 0)
              {
                ++num3;
                colorBlend.Colors[index] = System.Drawing.Color.Transparent;
                if (num3 > 1 && num4 > num3)
                  flag = true;
              }
              else
                ++num4;
            }
            color1 = colorBlend.Colors[0];
            color2 = colorBlend.Colors[length - 1];
          }
        }
      }
      catch
      {
      }
      float[,] numArray1 = new float[3, 3];
      PdfLinearGradientBrush linearGradientBrush2 = new PdfLinearGradientBrush(rect, (PdfColor) color1, (PdfColor) color2, angle);
      PdfTransformationMatrix transformationMatrix = new PdfTransformationMatrix();
      transformationMatrix.Matrix = this.m_graphics.Matrix.Matrix;
      float[,] numArray2 = new float[3, 3];
      float[,] numArray3 = new float[3, 3];
      float[,] numArray4 = new float[3, 3];
      numArray1[0, 0] = transformationMatrix.Matrix.Elements[0];
      numArray1[0, 1] = transformationMatrix.Matrix.Elements[1];
      numArray1[1, 0] = transformationMatrix.Matrix.Elements[2];
      numArray1[1, 1] = transformationMatrix.Matrix.Elements[3];
      numArray1[2, 0] = transformationMatrix.Matrix.Elements[4];
      numArray1[2, 1] = transformationMatrix.Matrix.Elements[5];
      numArray1[0, 2] = 0.0f;
      numArray1[1, 2] = 0.0f;
      numArray1[2, 2] = 1f;
      new PdfTransformationMatrix().Matrix = this.NativeGraphics.Transform;
      numArray3[0, 0] = this.NativeGraphics.Transform.Elements[0];
      numArray3[0, 1] = this.NativeGraphics.Transform.Elements[1];
      numArray3[1, 0] = this.NativeGraphics.Transform.Elements[2];
      numArray3[1, 1] = this.NativeGraphics.Transform.Elements[3];
      numArray3[2, 0] = this.NativeGraphics.Transform.Elements[4];
      numArray3[2, 1] = -this.NativeGraphics.Transform.Elements[5];
      numArray3[0, 2] = 0.0f;
      numArray3[1, 2] = 0.0f;
      numArray3[2, 2] = 1f;
      if (this.m_bounds != null && this.m_boundTransformation)
      {
        float[,] numArray5 = new float[3, 3];
        numArray2[0, 0] = this.m_bounds.Matrix.Elements[0];
        numArray2[0, 1] = this.m_bounds.Matrix.Elements[1];
        numArray2[1, 0] = this.m_bounds.Matrix.Elements[2];
        numArray2[1, 1] = this.m_bounds.Matrix.Elements[3];
        numArray2[0, 2] = 0.0f;
        numArray2[1, 2] = 0.0f;
        numArray2[2, 2] = 1f;
        numArray2[2, 0] = this.m_bounds.Matrix.Elements[4];
        numArray2[2, 1] = this.m_bounds.Matrix.Elements[5];
        if (this.NativeGraphics.Transform != this.m_bounds.Matrix)
        {
          for (int index1 = 0; index1 < 3; ++index1)
          {
            for (int index2 = 0; index2 < 3; ++index2)
            {
              for (int index3 = 0; index3 < 3; ++index3)
                numArray4[index1, index2] += numArray3[index1, index3] * numArray2[index3, index2];
            }
          }
        }
        for (int index4 = 0; index4 < 3; ++index4)
        {
          for (int index5 = 0; index5 < 3; ++index5)
          {
            for (int index6 = 0; index6 < 3; ++index6)
              numArray5[index4, index5] += numArray4[index4, index6] * numArray1[index6, index5];
          }
        }
        Matrix matrix = new Matrix(numArray5[0, 0], numArray5[0, 1], numArray5[1, 0], numArray5[1, 1], numArray5[2, 0], numArray5[2, 1]);
        transformationMatrix.Matrix = matrix;
        linearGradientBrush2.Matrix = transformationMatrix;
      }
      else
      {
        for (int index7 = 0; index7 < 3; ++index7)
        {
          for (int index8 = 0; index8 < 3; ++index8)
          {
            for (int index9 = 0; index9 < 3; ++index9)
              numArray4[index7, index8] += numArray3[index7, index9] * numArray1[index9, index8];
          }
        }
        RectangleF rectangleF = this.NativeGraphics.ClipBounds;
        double height = (double) rectangleF.Height;
        rectangleF = this.NativeGraphics.ClipBounds;
        double y = (double) rectangleF.Y;
        float num5 = (float) (height + y);
        float num6 = num5 / numArray4[2, 1];
        if (this.m_fillPath != null)
        {
          float val2 = this.m_fillPath.PathPoints[0].Y;
          foreach (PointF pathPoint in this.m_fillPath.PathPoints)
            val2 = Math.Min(pathPoint.Y, val2);
          float num7 = (num5 - val2) / num6;
          numArray4[2, 1] = num7;
        }
        else if ((double) this.m_brushRectangle.Y >= 0.0)
        {
          float num8 = (num5 - this.m_brushRectangle.Y) / num6;
          numArray4[2, 1] = num8;
        }
        if (this.m_clipPath)
        {
          rectangleF = this.NativeGraphics.ClipBounds;
          float width1 = rectangleF.Width;
          if ((double) numArray4[2, 0] > 0.0)
          {
            float num9 = width1 / numArray4[2, 0];
            if (this.m_fillPath != null)
            {
              float val2 = this.m_fillPath.PathPoints[0].X;
              foreach (PointF pathPoint in this.m_fillPath.PathPoints)
                val2 = Math.Min(pathPoint.X, val2);
              float num10 = (width1 - val2) / num9;
              numArray4[2, 0] = num10;
            }
            else if ((double) this.m_brushRectangle.X >= 0.0)
            {
              float num11 = (width1 - this.m_brushRectangle.X) / num9;
              numArray4[2, 0] = num11;
            }
          }
          else if (this.NativeGraphics != null)
          {
            if (this.m_fillPath != null)
            {
              rectangleF = this.NativeGraphics.VisibleClipBounds;
              if ((double) rectangleF.Width > 0.0)
              {
                float val2 = this.m_fillPath.PathPoints[0].X;
                foreach (PointF pathPoint in this.m_fillPath.PathPoints)
                  val2 = Math.Min(pathPoint.X, val2);
                float[,] numArray6 = numArray4;
                double num12 = (double) val2;
                rectangleF = this.NativeGraphics.VisibleClipBounds;
                double width2 = (double) rectangleF.Width;
                double num13 = num12 / width2;
                numArray6[2, 0] = (float) num13;
                goto label_98;
              }
            }
            if ((double) this.m_brushRectangle.X > 0.0)
            {
              rectangleF = this.NativeGraphics.VisibleClipBounds;
              if ((double) rectangleF.Width > 0.0)
              {
                float[,] numArray7 = numArray4;
                double x = (double) this.m_brushRectangle.X;
                rectangleF = this.NativeGraphics.VisibleClipBounds;
                double width3 = (double) rectangleF.Width;
                double num14 = x / width3;
                numArray7[2, 0] = (float) num14;
                goto label_98;
              }
            }
            numArray4[2, 0] = 0.0f;
          }
        }
label_98:
        Matrix matrix = new Matrix(numArray4[0, 0], numArray4[0, 1], numArray4[1, 0], numArray4[1, 1], numArray4[2, 0], numArray4[2, 1]);
        transformationMatrix.Matrix = matrix;
        if (float.IsInfinity(matrix.Elements[5]))
          transformationMatrix.Matrix = this.m_graphics.Matrix.Matrix;
        if (this.m_brushRectangle != RectangleF.Empty || this.negativeRegion)
          linearGradientBrush2.Matrix = transformationMatrix;
      }
      if (colorBlend != null)
      {
        System.Drawing.Color[] colors = colorBlend.Colors;
        PdfColorBlend pdfColorBlend = new PdfColorBlend(colors.Length);
        pdfColorBlend.Colors = PdfEmfRenderer.ConvertColors(colors);
        pdfColorBlend.Positions = colorBlend.Positions;
        linearGradientBrush2.InterpolationColors = pdfColorBlend;
      }
      else if (linearGradientBrush1.Blend != null && !this.m_gradientPen)
      {
        Blend blend = linearGradientBrush1.Blend;
        if (blend != null)
        {
          PdfBlend pdfBlend = new PdfBlend();
          pdfBlend.Factors = blend.Factors;
          pdfBlend.Positions = blend.Positions;
          linearGradientBrush2.Blend = pdfBlend;
        }
      }
      if (linearGradientBrush1.WrapMode == WrapMode.Tile || linearGradientBrush1.WrapMode == WrapMode.TileFlipX)
        linearGradientBrush2.Extend = PdfExtend.Both;
      alpha = !flag ? (float) color1.A / (float) byte.MaxValue : (float) (((double) color1.A + (double) color2.A) / 510.0);
      if ((double) alpha == 0.0)
      {
        alpha = (float) color2.A / (float) byte.MaxValue;
        alpha = (double) alpha > 0.0 ? alpha : 1f;
      }
      pdfBrush = (PdfBrush) linearGradientBrush2;
    }
    else if (hatchBrush != null)
    {
      pdfBrush = this.ConvertHatchBrush(hatchBrush, out alpha);
    }
    else
    {
      if (pathGradientBrush1 == null)
        throw new ArgumentException("Unsupported brush type: " + (object) brush, nameof (brush));
      RectangleF empty = RectangleF.Empty;
      if (brush is PathGradientBrush)
      {
        RectangleF rect = RectangleF.Empty;
        PathGradientBrush pathGradientBrush2 = brush as PathGradientBrush;
        float[,] numArray8 = new float[3, 3];
        float[,] numArray9 = new float[3, 3];
        PdfTransformationMatrix transformationMatrix = new PdfTransformationMatrix();
        Matrix matrix1 = new Matrix(0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f);
        bool flag = false;
        transformationMatrix.Matrix = this.m_graphics.Matrix.Matrix;
        float[,] numArray10 = new float[3, 3];
        float[,] numArray11 = new float[3, 3];
        float[,] numArray12 = new float[3, 3];
        numArray8[0, 0] = transformationMatrix.Matrix.Elements[0];
        numArray8[0, 1] = transformationMatrix.Matrix.Elements[1];
        numArray8[1, 0] = transformationMatrix.Matrix.Elements[2];
        numArray8[1, 1] = transformationMatrix.Matrix.Elements[3];
        numArray8[2, 0] = transformationMatrix.Matrix.Elements[4];
        numArray8[2, 1] = transformationMatrix.Matrix.Elements[5];
        numArray8[0, 2] = 0.0f;
        numArray8[1, 2] = 0.0f;
        numArray8[2, 2] = 1f;
        new PdfTransformationMatrix().Matrix = this.NativeGraphics.Transform;
        numArray11[0, 0] = this.NativeGraphics.Transform.Elements[0];
        numArray11[0, 1] = this.NativeGraphics.Transform.Elements[1];
        numArray11[1, 0] = this.NativeGraphics.Transform.Elements[2];
        numArray11[1, 1] = this.NativeGraphics.Transform.Elements[3];
        numArray11[2, 0] = this.NativeGraphics.Transform.Elements[4];
        numArray11[2, 1] = -this.NativeGraphics.Transform.Elements[5];
        numArray11[0, 2] = 0.0f;
        numArray11[1, 2] = 0.0f;
        numArray11[2, 2] = 1f;
        Matrix matrix2;
        if (this.m_bounds != null && this.m_boundTransformation)
        {
          numArray10[0, 0] = this.m_bounds.Matrix.Elements[0];
          numArray10[0, 1] = this.m_bounds.Matrix.Elements[1];
          numArray10[1, 0] = this.m_bounds.Matrix.Elements[2];
          numArray10[1, 1] = this.m_bounds.Matrix.Elements[3];
          numArray10[0, 2] = 0.0f;
          numArray10[1, 2] = 0.0f;
          numArray10[2, 2] = 1f;
          numArray10[2, 0] = this.m_bounds.Matrix.Elements[4];
          numArray10[2, 1] = this.m_bounds.Matrix.Elements[5];
          if (this.NativeGraphics.Transform != this.m_bounds.Matrix)
          {
            for (int index10 = 0; index10 < 3; ++index10)
            {
              for (int index11 = 0; index11 < 3; ++index11)
              {
                for (int index12 = 0; index12 < 3; ++index12)
                  numArray12[index10, index11] += numArray11[index10, index12] * numArray10[index12, index11];
              }
            }
          }
          for (int index13 = 0; index13 < 3; ++index13)
          {
            for (int index14 = 0; index14 < 3; ++index14)
            {
              for (int index15 = 0; index15 < 3; ++index15)
                numArray9[index13, index14] += numArray12[index13, index15] * numArray8[index15, index14];
            }
          }
          matrix2 = new Matrix(numArray9[0, 0], numArray9[0, 1], numArray9[1, 0], numArray9[1, 1], numArray9[2, 0], numArray9[2, 1]);
        }
        else
        {
          for (int index16 = 0; index16 < 3; ++index16)
          {
            for (int index17 = 0; index17 < 3; ++index17)
            {
              for (int index18 = 0; index18 < 3; ++index18)
                numArray12[index16, index17] += numArray11[index16, index18] * numArray8[index18, index17];
            }
          }
          flag = true;
          matrix2 = new Matrix(numArray12[0, 0], numArray12[0, 1], numArray12[1, 0], numArray12[1, 1], numArray12[2, 0], numArray12[2, 1]);
        }
        SizeF size1 = this.Graphics.Size;
        int width4 = (int) ((double) size1.Width + 1.0);
        size1 = this.Graphics.Size;
        int height1 = (int) ((double) size1.Height + 1.0);
        Bitmap bitmap1 = new Bitmap(width4, height1, PixelFormat.Format32bppArgb);
        System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage((Image) bitmap1);
        graphics.SmoothingMode = SmoothingMode.HighQuality;
        if ((double) matrix2.Elements[0] != 0.0 && (double) matrix2.Elements[3] != 0.0)
        {
          if (flag)
            graphics.ScaleTransform(matrix2.Elements[0], matrix2.Elements[3]);
          else
            graphics.ScaleTransform(matrix2.Elements[0], -matrix2.Elements[3]);
        }
        graphics.TranslateTransform(this.NativeGraphics.Transform.Elements[4], this.NativeGraphics.Transform.Elements[5]);
        RectangleF rectangle;
        if (this.m_fillPath != null)
        {
          rectangle = this.m_fillPath.GetBounds();
          graphics.FillPath((Brush) pathGradientBrush2, this.m_fillPath);
        }
        else
        {
          rectangle = this.m_brushRectangle;
          graphics.FillRectangle((Brush) pathGradientBrush2, this.m_brushRectangle);
        }
        if ((double) matrix2.Elements[0] != 0.0 && (double) matrix2.Elements[3] != 0.0)
        {
          rect = ((double) pathGradientBrush2.Rectangle.Width != (double) rectangle.Width || (double) pathGradientBrush2.Rectangle.Height != (double) rectangle.Height ? new RectangleF(0.0f, 0.0f, matrix2.Elements[0] * rectangle.Width, matrix2.Elements[3] * rectangle.Height) : new RectangleF(0.0f, 0.0f, matrix2.Elements[0] * pathGradientBrush2.Rectangle.Width, matrix2.Elements[3] * pathGradientBrush2.Rectangle.Height)) with
          {
            Location = (double) rectangle.X == 0.0 || (double) rectangle.Y == 0.0 ? new PointF(this.NativeGraphics.Transform.Elements[4] * matrix2.Elements[0], this.NativeGraphics.Transform.Elements[5] * matrix2.Elements[3]) : new PointF((rectangle.X + this.NativeGraphics.Transform.Elements[4]) * matrix2.Elements[0], (rectangle.Y + this.NativeGraphics.Transform.Elements[5]) * matrix2.Elements[3])
          };
          if ((double) rect.Location.Y == 0.0 && (double) rect.Width > (double) rectangle.Width && (double) rect.Height > (double) rectangle.Height)
            rect = rectangle;
        }
        else
          rect = rectangle;
        double width5 = (double) bitmap1.Size.Width;
        double x1 = (double) rect.Location.X;
        size1 = rect.Size;
        double width6 = (double) size1.Width;
        double num15 = x1 + width6;
        Size size2;
        PointF location;
        if (width5 < num15)
        {
          ref RectangleF local = ref rect;
          size2 = bitmap1.Size;
          double width7 = (double) size2.Width;
          location = rect.Location;
          double x2 = (double) location.X;
          double num16 = width7 - x2;
          local.Width = (float) num16;
        }
        size2 = bitmap1.Size;
        double height2 = (double) size2.Height;
        location = rect.Location;
        double y1 = (double) location.Y;
        size1 = rect.Size;
        double height3 = (double) size1.Height;
        double num17 = y1 + height3;
        if (height2 < num17)
        {
          ref RectangleF local = ref rect;
          size2 = bitmap1.Size;
          double height4 = (double) size2.Height;
          location = rect.Location;
          double y2 = (double) location.Y;
          double num18 = height4 - y2;
          local.Height = (float) num18;
        }
        Bitmap bitmap2 = (Bitmap) null;
        if ((double) rect.Width != 0.0 && (double) rect.Height != 0.0)
          bitmap2 = bitmap1.Clone(rect, bitmap1.PixelFormat);
        bitmap1.Dispose();
        if (bitmap2 != null)
        {
          MemoryStream memoryStream = new MemoryStream();
          ImageCodecInfo encoderInfo = this.GetEncoderInfo("image/png");
          EncoderParameters encoderParams = new EncoderParameters(1);
          encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, (byte) 100);
          if ((double) rectangle.Y < 0.0)
            bitmap2.RotateFlip(RotateFlipType.Rotate180FlipX);
          bitmap2.Save((Stream) memoryStream, encoderInfo, encoderParams);
          bitmap2.Dispose();
          this.Graphics.DrawImage(PdfImage.FromStream((Stream) memoryStream), rectangle);
        }
        else
        {
          if (pathGradientBrush1.InterpolationColors.Colors.Length > 0)
          {
            int length = pathGradientBrush1.InterpolationColors.Colors.Length;
            pdfBrush = (PdfBrush) new PdfSolidBrush((PdfColor) pathGradientBrush1.InterpolationColors.Colors[length - 1]);
          }
          else
            pdfBrush = (PdfBrush) new PdfSolidBrush((PdfColor) System.Drawing.Color.Black);
          alpha = (float) pathGradientBrush1.CenterColor.A / (float) byte.MaxValue;
        }
      }
    }
    if (this.m_brushRectangle != RectangleF.Empty)
      this.m_brushRectangle = RectangleF.Empty;
    if (this.m_fillPath != null)
      this.m_fillPath = (GraphicsPath) null;
    return pdfBrush;
  }

  private ImageCodecInfo GetEncoderInfo(string mimeType)
  {
    ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
    for (int index = 0; index < imageEncoders.Length; ++index)
    {
      if (imageEncoders[index].MimeType == mimeType)
        return imageEncoders[index];
    }
    return (ImageCodecInfo) null;
  }

  internal PdfMask CheckAlpha(Bitmap bitmap)
  {
    PdfMask pdfMask = (PdfMask) null;
    switch (bitmap.PixelFormat)
    {
      case PixelFormat.Format1bppIndexed:
      case PixelFormat.Format4bppIndexed:
      case PixelFormat.Format8bppIndexed:
        System.Drawing.Color[] entries = bitmap.Palette.Entries;
        pdfMask = this.CheckAlpha(bitmap.Palette.Flags, (Image) bitmap, entries);
        break;
      case PixelFormat.Format32bppPArgb:
      case PixelFormat.Format32bppArgb:
        pdfMask = (PdfMask) new PdfImageMask(new PdfBitmap((Image) PdfBitmap.CreateMaskFromARGBImage((Image) bitmap)));
        break;
    }
    return pdfMask;
  }

  private PdfMask CheckAlpha(int flags, Image bitmap, System.Drawing.Color[] array)
  {
    PdfMask pdfMask = (PdfMask) null;
    bool flag = false;
    int index = 0;
    for (int length = array.Length; index < length; ++index)
    {
      if (flags == 1 && array[index].A < byte.MaxValue)
        flag = true;
    }
    if (flag)
      pdfMask = (PdfMask) new PdfImageMask(new PdfBitmap(PdfBitmap.CreateMaskFromIndexedImage(bitmap)));
    return pdfMask;
  }

  private PdfBrush ConvertHatchBrush(HatchBrush hatchBrush, out float alpha)
  {
    System.Drawing.Color foregroundColor = hatchBrush.ForegroundColor;
    System.Drawing.Color backgroundColor = hatchBrush.BackgroundColor;
    SizeF sizeF = new SizeF(8f, 8f);
    PdfTilingBrush pdfTilingBrush = new PdfTilingBrush(sizeF);
    PdfGraphics graphics = pdfTilingBrush.Graphics;
    PdfPen pen = new PdfPen(foregroundColor, 1f);
    alpha = (float) foregroundColor.A / (float) byte.MaxValue;
    if (!backgroundColor.IsEmpty && backgroundColor.A != (byte) 0)
      graphics.DrawRectangle((PdfBrush) new PdfSolidBrush((PdfColor) backgroundColor), new RectangleF(PointF.Empty, sizeF));
    switch (hatchBrush.HatchStyle)
    {
      case HatchStyle.Horizontal:
        PdfEmfRenderer.DrawHorizontal(graphics, pen, sizeF);
        goto case HatchStyle.LargeConfetti;
      case HatchStyle.Vertical:
        PdfEmfRenderer.DrawVertical(graphics, pen, sizeF);
        goto case HatchStyle.LargeConfetti;
      case HatchStyle.ForwardDiagonal:
        PdfEmfRenderer.DrawForwardDiagonal(graphics, pen, sizeF);
        goto case HatchStyle.LargeConfetti;
      case HatchStyle.BackwardDiagonal:
        PdfEmfRenderer.DrawBackwardDiagonal(graphics, pen, sizeF);
        goto case HatchStyle.LargeConfetti;
      case HatchStyle.Cross:
        PdfEmfRenderer.DrawCross(graphics, pen, sizeF);
        goto case HatchStyle.LargeConfetti;
      case HatchStyle.DiagonalCross:
        PdfEmfRenderer.DrawForwardDiagonal(graphics, pen, sizeF);
        PdfEmfRenderer.DrawBackwardDiagonal(graphics, pen, sizeF);
        goto case HatchStyle.LargeConfetti;
      case HatchStyle.LightDownwardDiagonal:
        PdfEmfRenderer.DrawDownwardDiagonal(graphics, pen, sizeF);
        goto case HatchStyle.LargeConfetti;
      case HatchStyle.LightUpwardDiagonal:
        PdfEmfRenderer.DrawUpwardDiagonal(graphics, pen, sizeF);
        goto case HatchStyle.LargeConfetti;
      case HatchStyle.DarkDownwardDiagonal:
        pen.Width = 2f;
        PdfEmfRenderer.DrawDownwardDiagonal(graphics, pen, sizeF);
        goto case HatchStyle.LargeConfetti;
      case HatchStyle.DarkUpwardDiagonal:
        pen.Width = 2f;
        PdfEmfRenderer.DrawUpwardDiagonal(graphics, pen, sizeF);
        goto case HatchStyle.LargeConfetti;
      case HatchStyle.LightVertical:
        PdfEmfRenderer.DrawVertical(graphics, pen, sizeF);
        goto case HatchStyle.LargeConfetti;
      case HatchStyle.LightHorizontal:
        PdfEmfRenderer.DrawHorizontal(graphics, pen, sizeF);
        goto case HatchStyle.LargeConfetti;
      case HatchStyle.DarkVertical:
        pen.Width = 2f;
        PdfEmfRenderer.DrawVertical(graphics, pen, sizeF);
        goto case HatchStyle.LargeConfetti;
      case HatchStyle.DarkHorizontal:
        pen.Width = 2f;
        PdfEmfRenderer.DrawHorizontal(graphics, pen, sizeF);
        goto case HatchStyle.LargeConfetti;
      case HatchStyle.DashedDownwardDiagonal:
        pen.DashStyle = PdfDashStyle.Dash;
        PdfEmfRenderer.DrawDownwardDiagonal(graphics, pen, sizeF);
        goto case HatchStyle.LargeConfetti;
      case HatchStyle.DashedUpwardDiagonal:
        pen.DashStyle = PdfDashStyle.Dash;
        PdfEmfRenderer.DrawUpwardDiagonal(graphics, pen, sizeF);
        goto case HatchStyle.LargeConfetti;
      case HatchStyle.DashedHorizontal:
        pen.DashStyle = PdfDashStyle.Dash;
        PdfEmfRenderer.DrawHorizontal(graphics, pen, sizeF);
        goto case HatchStyle.LargeConfetti;
      case HatchStyle.DashedVertical:
        pen.DashStyle = PdfDashStyle.Dash;
        PdfEmfRenderer.DrawVertical(graphics, pen, sizeF);
        goto case HatchStyle.LargeConfetti;
      case HatchStyle.LargeConfetti:
      case HatchStyle.Divot:
        return (PdfBrush) pdfTilingBrush ?? (PdfBrush) new PdfSolidBrush((PdfColor) foregroundColor);
      case HatchStyle.DiagonalBrick:
        PdfEmfRenderer.DrawForwardDiagonal(graphics, pen, sizeF);
        PdfEmfRenderer.DrawBrickTails(graphics, pen, sizeF);
        goto case HatchStyle.LargeConfetti;
      case HatchStyle.HorizontalBrick:
        PdfEmfRenderer.DrawHorizontalBrick(graphics, pen, sizeF);
        goto case HatchStyle.LargeConfetti;
      case HatchStyle.Weave:
        this.DrawWeave(graphics, pen, sizeF);
        goto case HatchStyle.LargeConfetti;
      case HatchStyle.DottedGrid:
        pen.DashStyle = PdfDashStyle.Dot;
        PdfEmfRenderer.DrawCross(graphics, pen, sizeF);
        goto case HatchStyle.LargeConfetti;
      case HatchStyle.DottedDiamond:
        pen.DashStyle = PdfDashStyle.Dot;
        PdfEmfRenderer.DrawForwardDiagonal(graphics, pen, sizeF);
        PdfEmfRenderer.DrawBackwardDiagonal(graphics, pen, sizeF);
        goto case HatchStyle.LargeConfetti;
      case HatchStyle.LargeCheckerBoard:
        PdfEmfRenderer.DrawCheckerBoard(graphics, pen, sizeF, 4);
        goto case HatchStyle.LargeConfetti;
      default:
        alpha = 0.5f;
        pdfTilingBrush = (PdfTilingBrush) null;
        goto case HatchStyle.LargeConfetti;
    }
  }

  internal static PdfColor[] ConvertColors(System.Drawing.Color[] colors)
  {
    int length = colors.Length;
    PdfColor[] pdfColorArray = new PdfColor[length];
    for (int index = 0; index < length; ++index)
    {
      System.Drawing.Color color = colors[index];
      pdfColorArray[index] = (PdfColor) color;
    }
    return pdfColorArray;
  }

  private PdfBrush GetBrushFromPen(PdfPen pdfPen)
  {
    return pdfPen.Brush ?? (PdfBrush) new PdfSolidBrush(pdfPen.Color);
  }

  private PdfStringFormat ConvertFormat(StringFormat format)
  {
    PdfStringFormat pdfStringFormat = (PdfStringFormat) null;
    if (format != null)
    {
      this.Graphics.PutComment($"String Format Flags: {(object) format.FormatFlags}({(object) (int) format.FormatFlags})");
      this.Graphics.PutComment("Alignment: " + (object) format.Alignment);
      this.Graphics.PutComment("Line Alignment: " + (object) format.LineAlignment);
      pdfStringFormat = new PdfStringFormat();
      pdfStringFormat.LineLimit = false;
      pdfStringFormat.Alignment = PdfEmfRenderer.ConvertAlingnmet(format.Alignment);
      pdfStringFormat.LineAlignment = PdfEmfRenderer.CovertLineAlignment(format.LineAlignment);
      format.GetTabStops(out float _);
      pdfStringFormat.NoClip = true;
      if ((format.FormatFlags & StringFormatFlags.DirectionRightToLeft) != (StringFormatFlags) 0)
      {
        pdfStringFormat.TextDirection = PdfTextDirection.RightToLeft;
        pdfStringFormat.isCustomRendering = true;
      }
      if (pdfStringFormat.NoClip)
        pdfStringFormat.LineLimit = false;
      pdfStringFormat.WordWrap = this.GetWrapType(format.FormatFlags);
    }
    return pdfStringFormat;
  }

  private PdfWordWrapType GetWrapType(StringFormatFlags stringFormatFlags)
  {
    PdfWordWrapType wrapType = PdfWordWrapType.Word;
    if ((stringFormatFlags & StringFormatFlags.NoWrap) != (StringFormatFlags) 0)
      wrapType = PdfWordWrapType.None;
    return wrapType;
  }

  internal static PdfVerticalAlignment CovertLineAlignment(StringAlignment stringAlignment)
  {
    PdfVerticalAlignment verticalAlignment;
    switch (stringAlignment)
    {
      case StringAlignment.Near:
        verticalAlignment = PdfVerticalAlignment.Top;
        break;
      case StringAlignment.Far:
        verticalAlignment = PdfVerticalAlignment.Bottom;
        break;
      default:
        verticalAlignment = PdfVerticalAlignment.Middle;
        break;
    }
    return verticalAlignment;
  }

  internal static PdfTextAlignment ConvertAlingnmet(StringAlignment stringAlignment)
  {
    PdfTextAlignment pdfTextAlignment;
    switch (stringAlignment)
    {
      case StringAlignment.Center:
        pdfTextAlignment = PdfTextAlignment.Center;
        break;
      case StringAlignment.Far:
        pdfTextAlignment = PdfTextAlignment.Right;
        break;
      default:
        pdfTextAlignment = PdfTextAlignment.Left;
        break;
    }
    return pdfTextAlignment;
  }

  private PdfPen ConvertPen(Pen pen) => this.ConvertPen(pen, false);

  private PdfPen ConvertPen(Pen pen, bool rotate)
  {
    float alpha;
    PdfPen pdfPen = this.ConvertPen(pen, out alpha, rotate);
    if ((double) alpha == 0.0)
      pdfPen = new PdfPen(PdfBrushes.White);
    else if (this.IsTranparency)
      this.Graphics.SetTransparency(this.AlphaPen, this.AlphaBrush, this.BlendMode);
    else
      this.Graphics.SetTransparency(alpha, alpha, PdfBlendMode.Normal);
    return pdfPen;
  }

  private PdfBrush ConvertBrush(Brush brush)
  {
    PdfBlendMode blendMode = PdfBlendMode.Normal;
    float alpha;
    PdfBrush pdfBrush = this.ConvertBrush(brush, out alpha);
    if (this.IsTranparency)
      this.Graphics.SetTransparency(this.AlphaPen, this.AlphaBrush, this.BlendMode);
    else
      this.Graphics.SetTransparency(alpha, alpha, blendMode);
    return pdfBrush;
  }

  private void InternalResetClip()
  {
    if (this.m_bFirstCall)
      return;
    this.Graphics.PutComment(nameof (InternalResetClip));
    this.m_bFirstCall = true;
    this.Graphics.Restore();
    this.m_stateChanged = true;
  }

  private void InternalResetTransformation()
  {
    PdfTransformationMatrix transformationMatrix = new PdfTransformationMatrix();
    if (this.m_bFirstTransform)
      return;
    this.Graphics.PutComment(nameof (InternalResetTransformation));
    if (this.m_multiplyTransform != null)
    {
      if ((double) this.m_multiplyTransform.Elements[0] != (double) transformationMatrix.Matrix.Elements[0] || (double) this.m_multiplyTransform.Elements[1] != (double) transformationMatrix.Matrix.Elements[1] || (double) this.m_multiplyTransform.Elements[2] != (double) transformationMatrix.Matrix.Elements[2] || (double) this.m_multiplyTransform.Elements[3] != (double) transformationMatrix.Matrix.Elements[3] || !this.m_isTranslate)
        this.Graphics.Restore();
    }
    else if (this.m_stateChanged && this.m_bFirstCall && !this.m_stateRestored)
      this.Graphics.Restore();
    this.m_bFirstTransform = true;
  }

  internal void DrawCustomCap(Pen pen, PointF[] points, PointF[] penPoints, bool isStartCap)
  {
    if (penPoints == null)
      return;
    PdfGraphicsState state = this.Graphics.Save();
    PointF empty1 = PointF.Empty;
    PointF empty2 = PointF.Empty;
    PointF point1;
    PointF point2;
    if (isStartCap)
    {
      point1 = points[1];
      point2 = points[0];
    }
    else
    {
      point1 = points[points.Length - 2];
      point2 = points[points.Length - 1];
      if (point1 == point2)
      {
        for (int length = points.Length; length > 0; --length)
        {
          if (point1 != points[length - 1])
          {
            point1 = points[length - 1];
            break;
          }
        }
      }
    }
    float num1 = point1.X - point2.X;
    float num2 = point1.Y - point2.Y;
    float num3 = (float) Math.Sqrt((double) num1 * (double) num1 + (double) num2 * (double) num2);
    double num4 = (double) num1 / (double) num2;
    float num5 = (float) Math.Atan((double) (num2 / num1));
    if ((double) (num1 / num3) <= 0.0)
      num5 += 3.14159274f;
    this.Graphics.TranslateTransform(point2.X, point2.Y);
    this.Graphics.RotateTransform((float) ((double) num5 * 180.0 / Math.PI) + 90f);
    PointF[] pointFArray = new PointF[penPoints.Length];
    GraphicsPath path = new GraphicsPath();
    float num6 = pen.Width * this.m_customLineCapData.widthScale;
    pointFArray[0].X = penPoints[0].X * num6;
    pointFArray[0].Y = penPoints[0].Y * num6;
    for (int index = 1; index < penPoints.Length; ++index)
    {
      pointFArray[index].X = penPoints[index].X * num6;
      pointFArray[index].Y = penPoints[index].Y * num6;
      path.AddLine(pointFArray[index - 1], pointFArray[index]);
    }
    if (this.m_customLineDataFlag == 1)
      this.FillPath(pen.Brush, path);
    else
      this.DrawPath(pen, path);
    this.Graphics.Restore(state);
  }

  private bool IsLine(PointF[] points)
  {
    int length = points.Length;
    float x = points[0].X;
    bool flag = false;
    for (int index = 1; index < length; ++index)
    {
      flag = false;
      if ((double) x == (double) points[index].X)
        flag = true;
      else
        break;
    }
    if (!flag)
    {
      float y = points[0].Y;
      for (int index = 1; index < length; ++index)
      {
        flag = false;
        if ((double) y == (double) points[index].Y)
          flag = true;
        else
          break;
      }
    }
    if (!flag)
    {
      float num1 = points[1].X - points[0].X;
      float num2 = (points[1].Y - points[0].Y) / num1;
      for (int index = 2; index < length; ++index)
      {
        float num3 = points[index].X - points[index - 1].X;
        float num4 = (points[index].Y - points[index - 1].Y) / num3;
        flag = false;
        if ((double) Math.Abs(num2 - num4) <= 1.4012984643248171E-45)
          flag = true;
        else
          break;
      }
    }
    return flag;
  }

  internal static PdfLineCap ConvertCaps(LineCap cap)
  {
    PdfLineCap pdfLineCap;
    switch (cap)
    {
      case LineCap.Square:
        pdfLineCap = PdfLineCap.Square;
        break;
      case LineCap.Round:
        pdfLineCap = PdfLineCap.Round;
        break;
      default:
        pdfLineCap = PdfLineCap.Flat;
        break;
    }
    return pdfLineCap;
  }

  internal static PdfLineJoin ConvertJoin(LineJoin join)
  {
    PdfLineJoin pdfLineJoin;
    switch (join)
    {
      case LineJoin.Bevel:
        pdfLineJoin = PdfLineJoin.Bevel;
        break;
      case LineJoin.Round:
        pdfLineJoin = PdfLineJoin.Round;
        break;
      default:
        pdfLineJoin = PdfLineJoin.Miter;
        break;
    }
    return pdfLineJoin;
  }

  internal static PdfDashStyle ConvertDashStyle(DashStyle dashStyle)
  {
    PdfDashStyle pdfDashStyle;
    switch (dashStyle)
    {
      case DashStyle.Dash:
        pdfDashStyle = PdfDashStyle.Dash;
        break;
      case DashStyle.Dot:
        pdfDashStyle = PdfDashStyle.Dot;
        break;
      case DashStyle.DashDot:
        pdfDashStyle = PdfDashStyle.DashDot;
        break;
      case DashStyle.DashDotDot:
        pdfDashStyle = PdfDashStyle.DashDotDot;
        break;
      case DashStyle.Custom:
        pdfDashStyle = PdfDashStyle.Custom;
        break;
      default:
        pdfDashStyle = PdfDashStyle.Solid;
        break;
    }
    return pdfDashStyle;
  }

  private static PdfTransformationMatrix PrepareMatrix(Matrix matrix)
  {
    PdfTransformationMatrix transformationMatrix = new PdfTransformationMatrix();
    PdfTransformationMatrix matrix1 = new PdfTransformationMatrix();
    matrix1.Matrix = matrix;
    transformationMatrix.Scale(1f, -1f);
    transformationMatrix.Multiply(matrix1);
    transformationMatrix.Scale(1f, -1f);
    return transformationMatrix;
  }

  private static PdfTransformationMatrix PrepareMatrix(Matrix matrix, float pageScale)
  {
    PdfTransformationMatrix transformationMatrix = new PdfTransformationMatrix();
    PdfTransformationMatrix matrix1 = new PdfTransformationMatrix();
    matrix1.Matrix = matrix;
    transformationMatrix.Scale(pageScale, -pageScale);
    transformationMatrix.Multiply(matrix1);
    transformationMatrix.Scale(1f, -1f);
    return transformationMatrix;
  }

  private static void DrawCross(PdfGraphics graphics, PdfPen pen, SizeF brushSize)
  {
    float num1 = brushSize.Width / 2f;
    float num2 = brushSize.Height / 2f;
    graphics.DrawLine(pen, num1, 0.0f, num1, brushSize.Height);
    graphics.DrawLine(pen, 0.0f, num2, brushSize.Width, num2);
  }

  private static void DrawBackwardDiagonal(PdfGraphics graphics, PdfPen pen, SizeF brushSize)
  {
    graphics.DrawLine(pen, brushSize.Width, 0.0f, 0.0f, brushSize.Height);
    graphics.DrawLine(pen, -1f, 1f, 1f, -1f);
    graphics.DrawLine(pen, brushSize.Width - 1f, brushSize.Height + 1f, brushSize.Width + 1f, brushSize.Height - 1f);
  }

  private static void DrawForwardDiagonal(PdfGraphics graphics, PdfPen pen, SizeF brushSize)
  {
    graphics.DrawLine(pen, 0.0f, 0.0f, brushSize.Width, brushSize.Height);
    graphics.DrawLine(pen, -1f, -1f, 1f, 1f);
    graphics.DrawLine(pen, brushSize.Width - 1f, brushSize.Height - 1f, brushSize.Width + 1f, brushSize.Height + 1f);
  }

  private static void DrawHorizontal(PdfGraphics graphics, PdfPen pen, SizeF brushSize)
  {
    float num1 = 0.0f;
    float num2 = brushSize.Height / 2f;
    float height = brushSize.Height;
    graphics.DrawLine(pen, 0.0f, num1, brushSize.Width, num1);
    graphics.DrawLine(pen, 0.0f, num2, brushSize.Width, num2);
    graphics.DrawLine(pen, 0.0f, height, brushSize.Width, height);
  }

  private static void DrawVertical(PdfGraphics graphics, PdfPen pen, SizeF brushSize)
  {
    float num1 = 0.0f;
    float num2 = brushSize.Height / 2f;
    float height = brushSize.Height;
    graphics.DrawLine(pen, num1, 0.0f, num1, brushSize.Height);
    graphics.DrawLine(pen, num2, 0.0f, num2, brushSize.Height);
    graphics.DrawLine(pen, height, 0.0f, height, brushSize.Height);
  }

  private static void DrawDownwardDiagonal(PdfGraphics graphics, PdfPen pen, SizeF brushSize)
  {
    float num1 = brushSize.Height / 2f;
    float num2 = brushSize.Width / 2f;
    graphics.DrawLine(pen, 0.0f, 0.0f, brushSize.Width, brushSize.Height);
    graphics.DrawLine(pen, 0.0f, num1, num2, brushSize.Height);
    graphics.DrawLine(pen, num2, 0.0f, brushSize.Width, num1);
    graphics.DrawLine(pen, -1f, -1f, 1f, 1f);
    graphics.DrawLine(pen, brushSize.Width - 1f, brushSize.Height - 1f, brushSize.Width + 1f, brushSize.Height + 1f);
  }

  private void DrawWeave(PdfGraphics g, PdfPen pen, SizeF brushSize)
  {
    g.TranslateTransform(-0.5f, -0.5f);
    g.DrawLine(pen, new PointF(0.0f, 0.0f), new PointF(0.5f, 0.5f));
    g.DrawLine(pen, new PointF(0.0f, 1f), new PointF(1f, 0.0f));
    g.DrawLine(pen, new PointF(0.0f, 5f), new PointF(5f, 0.0f));
    g.DrawLine(pen, new PointF(0.0f, 4f), new PointF(5f, 9f));
    g.DrawLine(pen, new PointF(2.5f, 2.5f), new PointF(9f, 9f));
    g.DrawLine(pen, new PointF(4f, 0.0f), new PointF(6.5f, 2.5f));
    g.DrawLine(pen, new PointF((float) (6.5 - Math.Sqrt(0.125)), (float) (2.5 + Math.Sqrt(0.125))), new PointF(9f, 0.0f));
    g.DrawLine(pen, new PointF(6.5f, 6.5f), new PointF(9f, 4f));
    g.DrawLine(pen, new PointF(2.5f, 6.5f), new PointF(0.5f, 8.5f));
  }

  private static void DrawUpwardDiagonal(PdfGraphics graphics, PdfPen pen, SizeF brushSize)
  {
    float num1 = brushSize.Height / 2f;
    float num2 = brushSize.Width / 2f;
    graphics.DrawLine(pen, brushSize.Width, 0.0f, 0.0f, brushSize.Height);
    graphics.DrawLine(pen, 0.0f, num1, num2, 0.0f);
    graphics.DrawLine(pen, num2, brushSize.Height, brushSize.Width, num1);
    graphics.DrawLine(pen, -1f, 1f, 1f, -1f);
    graphics.DrawLine(pen, brushSize.Width - 1f, brushSize.Height + 1f, brushSize.Width + 1f, brushSize.Height - 1f);
  }

  private static void DrawBrickTails(PdfGraphics graphics, PdfPen pen, SizeF brushSize)
  {
    float x1 = brushSize.Width / 2f;
    float y1 = brushSize.Height / 2f;
    graphics.DrawLine(pen, x1, y1, brushSize.Width, brushSize.Height);
  }

  private static void DrawHorizontalBrick(PdfGraphics graphics, PdfPen pen, SizeF brushSize)
  {
    float num1 = brushSize.Width / 2f;
    float num2 = brushSize.Height / 2f;
    graphics.DrawLine(pen, 0.0f, 0.0f, brushSize.Width, 0.0f);
    graphics.DrawLine(pen, 0.0f, brushSize.Height, brushSize.Width, brushSize.Height);
    graphics.DrawLine(pen, 0.0f, num2, brushSize.Width, num2);
    graphics.DrawLine(pen, num1, 0.0f, num1, num2);
    graphics.DrawLine(pen, 0.0f, num2, 0.0f, brushSize.Height);
    graphics.DrawLine(pen, brushSize.Width, num2, brushSize.Width, brushSize.Height);
  }

  private static void DrawCheckerBoard(
    PdfGraphics graphics,
    PdfPen pen,
    SizeF brushSize,
    int cellSize)
  {
    int num1 = (int) ((double) brushSize.Width / (double) cellSize);
    int num2 = (int) ((double) brushSize.Height / (double) cellSize);
    PdfSolidBrush brush = new PdfSolidBrush(pen.Color);
    for (int index1 = 0; index1 < num2; ++index1)
    {
      float y = (float) (index1 * cellSize);
      for (int index2 = 0; index2 < num1; ++index2)
      {
        float x = (float) (index2 * cellSize);
        graphics.DrawRectangle((PdfBrush) brush, x, y, (float) cellSize, (float) cellSize);
      }
    }
  }

  private string GetInstalledFontLocation(Font font)
  {
    string name = "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Fonts";
    RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(name, false);
    string[] valueNames = registryKey.GetValueNames();
    string environmentVariable = Environment.GetEnvironmentVariable("SystemRoot");
    string str1 = $"{font.Name} {this.GetFontSuffix(font.Style)}";
    int index = 0;
    for (int length = valueNames.Length; index < length; ++index)
    {
      if (valueNames[index].Contains(str1))
      {
        string str2 = registryKey.GetValue(valueNames[index]).ToString();
        return $"{environmentVariable}\\Fonts\\{str2}";
      }
    }
    return string.Empty;
  }

  private string GetFontSuffix(FontStyle fs)
  {
    string fontSuffix = "";
    switch (fs)
    {
      case FontStyle.Bold:
        fontSuffix = "Bold";
        break;
      case FontStyle.Italic:
        fontSuffix = "Italic";
        break;
      case FontStyle.Bold | FontStyle.Italic:
        fontSuffix = "Bold Italic";
        break;
    }
    return fontSuffix;
  }

  private void IsEmptyBrushColor(Brush brush, out bool empty)
  {
    empty = false;
    if (!(brush is SolidBrush solidBrush) || solidBrush.Color.A != (byte) 0)
      return;
    empty = true;
  }
}
