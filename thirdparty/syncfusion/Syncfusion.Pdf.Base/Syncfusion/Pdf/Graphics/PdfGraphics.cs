// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfGraphics
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.ColorSpace;
using Syncfusion.Pdf.Graphics.Fonts;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public sealed class PdfGraphics
{
  private const int PathTypesValuesMask = 15;
  internal bool m_isEMF;
  internal bool m_isEMFPlus;
  internal bool m_isUseFontSize;
  internal bool m_isBaselineFormat = true;
  internal float m_DpiY;
  private PdfStreamWriter m_streamWriter;
  private PdfGraphics.GetResources m_getResources;
  private SizeF m_canvasSize;
  internal RectangleF m_clipBounds;
  internal bool m_bStateSaved;
  private PdfPen m_currentPen;
  private PdfBrush m_currentBrush;
  private PdfFont m_currentFont;
  private PdfColorSpace m_currentColorSpace;
  private bool m_istransparencySet;
  private bool m_bCSInitialized;
  private bool m_CIEColors;
  private bool m_isItalic;
  private bool m_isRestoreGraphics;
  internal float m_cellBorderMaxHeight;
  private PdfGraphicsState gState;
  private Stack<PdfGraphicsState> m_graphicsState;
  private PdfTransformationMatrix m_matrix;
  private TextRenderingMode m_previousTextRenderingMode;
  private float m_previousCharacterSpacing;
  private float m_previousWordSpacing;
  private float m_previousTextScaling = 100f;
  private Dictionary<PdfGraphics.TransparencyData, PdfTransparency> m_trasparencies;
  private PdfStringFormat m_currentStringFormat;
  private PdfPageLayer m_layer;
  private PdfLayer m_documentLayer;
  private PdfAutomaticFieldInfoCollection m_automaticFields;
  private PdfStringLayoutResult m_stringLayoutResult;
  private float m_split;
  private bool m_isTransparentBrush;
  private static bool m_transparencyObject = false;
  private static object s_transparencyLock = new object();
  private static object s_syncLockTemplate = new object();
  private static object s_rtlRenderLock = new object();
  internal SizeF m_emfScalingFactor = SizeF.Empty;
  internal bool m_isEmfTextScaled;
  internal bool isImageOptimized;
  internal bool isStandardUnicode;
  internal PdfDocument m_pdfdocument;
  private float m_mediaBoxUpperRightBound;
  internal PdfArray m_cropBox;
  internal bool m_isNormalRender = true;
  private PdfColorSpace lastDocumentCS;
  private PdfColorSpace lastGraphicsCS;
  private bool colorSpaceChanged;
  private TextRenderingMode m_textRenderingMode;
  private bool m_isTextRenderingSet;
  private PdfTag m_tag;
  internal bool isBoldFontWeight;
  private bool m_isTaggedPdf;
  private string m_currentTagType;
  private bool isTemplateGraphics;
  internal bool isEmptyLayer;
  private PdfStringLayoutResult m_layoutResult;
  private PdfDictionary m_tableSpan;
  internal bool customTag;
  internal System.Drawing.Graphics NativeGraphics;
  private Hashtable m_graphicsStates;
  private RectangleF RealClip = new RectangleF();
  private bool m_stateChanged;
  private bool m_bFirstTransform = true;
  private bool m_stateRestored;
  private bool m_bFirstCall = true;
  private System.Drawing.Drawing2D.Matrix m_multiplyTransform;
  private RectangleF m_textClip;
  private bool m_clipPath;
  private bool m_isTranslate;
  private SizeF m_translateTransform;
  private RectangleF m_clipBoundsDirectPDF = new RectangleF();
  private RectangleF m_DocIOPageBounds = new RectangleF();
  private bool m_isDirectPDF;
  private bool m_optimizeIdenticalImages;
  private bool m_isXPStoken;
  private Dictionary<string, string> m_replaceCharacter;

  internal event PdfGraphics.StructElementEventHandler StructElementChanged;

  public SizeF Size => this.m_canvasSize;

  internal float MediaBoxUpperRightBound
  {
    get => this.m_mediaBoxUpperRightBound;
    set => this.m_mediaBoxUpperRightBound = value;
  }

  public SizeF ClientSize => this.m_clipBounds.Size;

  public PdfColorSpace ColorSpace
  {
    get => this.m_currentColorSpace;
    set
    {
      if (PdfDocument.ConformanceLevel == PdfConformanceLevel.Pdf_X1A2001)
        return;
      this.m_currentColorSpace = value;
    }
  }

  internal PdfStreamWriter StreamWriter
  {
    get => this.m_streamWriter;
    set => this.m_streamWriter = value;
  }

  internal PdfTransformationMatrix Matrix
  {
    get
    {
      if (this.m_matrix == null)
        this.m_matrix = new PdfTransformationMatrix();
      return this.m_matrix;
    }
  }

  internal PdfPageLayer Layer => this.m_layer;

  internal PdfPageBase Page
  {
    get
    {
      if (this.m_documentLayer != null)
        return this.m_documentLayer.Page;
      return this.m_layer == null ? (PdfPageBase) null : this.m_layer.Page;
    }
  }

  internal PdfAutomaticFieldInfoCollection AutomaticFields
  {
    get
    {
      if (this.m_automaticFields == null)
        this.m_automaticFields = new PdfAutomaticFieldInfoCollection();
      return this.m_automaticFields;
    }
  }

  internal PdfStringLayoutResult StringLayoutResult => this.m_stringLayoutResult;

  internal float Split
  {
    get => this.m_split;
    set => this.m_split = value;
  }

  internal static bool TransparencyObject => PdfGraphics.m_transparencyObject;

  internal PdfTag Tag
  {
    get => this.m_tag;
    set => this.m_tag = value;
  }

  internal bool IsTaggedPdf
  {
    get
    {
      if (this.Layer != null && this.Page is PdfPage && (this.Page as PdfPage).Section.ParentDocument is PdfDocument && ((this.Page as PdfPage).Section.ParentDocument as PdfDocument).AutoTag)
        this.m_isTaggedPdf = true;
      return this.m_isTaggedPdf;
    }
  }

  internal string CurrentTagType
  {
    get => this.m_currentTagType;
    set
    {
      this.m_currentTagType = value;
      this.OnStructElementChanged(this.Tag);
      this.StructElementChanged = (PdfGraphics.StructElementEventHandler) null;
    }
  }

  internal bool IsTemplateGraphics
  {
    get => this.isTemplateGraphics;
    set => this.isTemplateGraphics = value;
  }

  internal PdfStringLayoutResult LayoutResult
  {
    get => this.m_layoutResult;
    set => this.m_layoutResult = value;
  }

  internal System.Drawing.Drawing2D.Matrix Transform
  {
    get => this.NativeGraphics.Transform;
    set
    {
      this.InternalResetClip();
      this.NativeGraphics.Transform = value;
      PdfTransformationMatrix matrix = this.PrepareMatrix(value, this.PageScale);
      this.PutComment("Transform property");
      this.SetTransform();
      this.MultiplyTransform(matrix);
      this.Matrix.Multiply(matrix);
    }
  }

  private float PageScale
  {
    get => this.NativeGraphics.PageScale;
    set
    {
      this.NativeGraphics.PageScale = value;
      this.PutComment("PageScale property");
      this.SetTransform();
      this.ScaleTransform(value, value);
    }
  }

  internal GraphicsUnit PageUnit
  {
    get => this.NativeGraphics.PageUnit;
    set
    {
      this.NativeGraphics.PageUnit = value;
      float sx = 1f;
      float sy = 1f;
      if (value != GraphicsUnit.Display)
      {
        PdfUnitConvertor pdfUnitConvertor1 = new PdfUnitConvertor(this.NativeGraphics);
        PdfUnitConvertor pdfUnitConvertor2 = new PdfUnitConvertor(this.NativeGraphics);
        sx = pdfUnitConvertor1.ConvertUnits(sx, this.GraphicsToPrintUnits(value), PdfGraphicsUnit.Pixel);
        sy = pdfUnitConvertor2.ConvertUnits(sy, this.GraphicsToPrintUnits(value), PdfGraphicsUnit.Pixel);
      }
      this.PutComment("PageUnit property");
      this.NativeGraphics.ScaleTransform(sx, sy, MatrixOrder.Prepend);
    }
  }

  internal RectangleF ClipBounds
  {
    get => this.m_textClip;
    set => this.m_textClip = value;
  }

  internal RectangleF DocIOPageBounds
  {
    get => this.m_DocIOPageBounds;
    set
    {
      this.m_DocIOPageBounds = value;
      this.m_clipBoundsDirectPDF = value;
    }
  }

  internal bool IsDirectPDF
  {
    get => this.m_isDirectPDF;
    set
    {
      this.m_isDirectPDF = value;
      if (value && PdfDocument.ConformanceLevel == PdfConformanceLevel.Pdf_X1A2001)
      {
        if (this.Page != null && this.Page is PdfPage page)
          page.Document.ColorSpace = PdfColorSpace.CMYK;
        this.ColorSpace = PdfColorSpace.CMYK;
      }
      if (this.Page == null || !(this.Page is PdfPage))
        return;
      PdfPage page1 = this.Page as PdfPage;
      if (page1.Document == null)
        return;
      page1.Document.m_WordtoPDFTagged = true;
    }
  }

  internal bool OptimizeIdenticalImages
  {
    get => this.m_optimizeIdenticalImages;
    set => this.m_optimizeIdenticalImages = value;
  }

  internal bool XPSToken
  {
    get => this.m_isXPStoken;
    set => this.m_isXPStoken = value;
  }

  internal Dictionary<string, string> XPSReplaceCharacter
  {
    get
    {
      if (this.m_replaceCharacter == null)
        this.m_replaceCharacter = new Dictionary<string, string>();
      return this.m_replaceCharacter;
    }
  }

  internal PdfDictionary TableSpan
  {
    get => this.m_tableSpan;
    set => this.m_tableSpan = value;
  }

  internal PdfGraphics(SizeF size, PdfGraphics.GetResources resources, PdfStreamWriter writer)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (resources == null)
      throw new ArgumentNullException(nameof (resources));
    this.m_streamWriter = writer;
    this.m_getResources = resources;
    this.m_canvasSize = size;
    this.Initialize();
  }

  internal PdfGraphics(SizeF size, PdfGraphics.GetResources resources, PdfStream stream)
    : this(size, resources, new PdfStreamWriter(stream))
  {
  }

  public void DrawLine(PdfPen pen, PointF point1, PointF point2)
  {
    if (this.IsDirectPDF)
    {
      this.OnDrawPrimitive();
      if (pen.Color.A == (byte) 0)
        pen = new PdfPen(Color.White);
    }
    this.DrawLine(pen, point1.X, point1.Y, point2.X, point2.Y);
  }

  public void DrawLine(PdfPen pen, float x1, float y1, float x2, float y2)
  {
    this.BeginMarkContent();
    bool flag = false;
    if (!this.IsTemplateGraphics && (this.Tag != null || this.IsTaggedPdf) || this.IsTaggedPdf && !this.m_isEMF)
    {
      if (this.Tag == null)
        this.Tag = (PdfTag) new PdfStructureElement(PdfTagType.Figure);
      PdfPath pdfPath = new PdfPath();
      pdfPath.AddLine(x1, y1, x2, y2);
      this.Tag.Bounds = pdfPath.GetBounds();
      this.StructElementChanged += new PdfGraphics.StructElementEventHandler(this.ApplyTag);
      this.CurrentTagType = "Figure";
      this.Tag = (PdfTag) null;
      flag = true;
    }
    this.StateControl(pen, (PdfBrush) null, (PdfFont) null);
    this.CapControl(pen, x1, y1, x2, y2);
    this.CapControl(pen, x2, y2, x1, y1);
    PdfStreamWriter streamWriter = this.StreamWriter;
    streamWriter.BeginPath(x1, y1);
    streamWriter.AppendLineSegment(x2, y2);
    streamWriter.StrokePath();
    if (flag)
      this.StreamWriter.Write("EMC" + Environment.NewLine);
    this.EndMarkContent();
    this.m_getResources().RequireProcSet("PDF");
  }

  public void DrawRectangle(PdfPen pen, RectangleF rectangle)
  {
    this.DrawRectangle(pen, (PdfBrush) null, rectangle);
  }

  public void DrawRectangle(PdfPen pen, float x, float y, float width, float height)
  {
    this.DrawRectangle(pen, (PdfBrush) null, x, y, width, height);
  }

  public void DrawRectangle(PdfBrush brush, RectangleF rectangle)
  {
    if (this.IsDirectPDF)
    {
      this.OnDrawPrimitive();
      if (brush is PdfHatchBrush)
      {
        PdfHatchBrush pdfHatchBrush = brush as PdfHatchBrush;
        if (!pdfHatchBrush.BackColor.IsEmpty && pdfHatchBrush.BackColor.A != (byte) 0)
          this.DrawRectangle((PdfBrush) new PdfSolidBrush(pdfHatchBrush.BackColor), rectangle);
      }
    }
    this.DrawRectangle((PdfPen) null, brush, rectangle);
  }

  public void DrawRectangle(PdfBrush brush, float x, float y, float width, float height)
  {
    if (this.IsDirectPDF)
      this.OnDrawPrimitive();
    this.DrawRectangle((PdfPen) null, brush, x, y, width, height);
  }

  public void DrawRectangle(PdfPen pen, PdfBrush brush, RectangleF rectangle)
  {
    this.DrawRectangle(pen, brush, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
  }

  public void DrawRectangle(
    PdfPen pen,
    PdfBrush brush,
    float x,
    float y,
    float width,
    float height)
  {
    this.BeginMarkContent();
    bool flag = false;
    if (!this.IsTemplateGraphics && (this.Tag != null || this.IsTaggedPdf) && !this.m_isEMF || this.IsTaggedPdf && !this.m_isEMF || this.customTag)
    {
      if (this.Tag == null)
        this.Tag = (PdfTag) new PdfStructureElement(PdfTagType.Figure);
      this.Tag.Bounds = new RectangleF(x, y, width, height);
      this.StructElementChanged += new PdfGraphics.StructElementEventHandler(this.ApplyTag);
      this.CurrentTagType = "Figure";
      this.Tag = (PdfTag) null;
      flag = true;
    }
    if (brush is PdfSolidBrush && (brush as PdfSolidBrush).Color.A == (byte) 0)
    {
      this.m_isTransparentBrush = true;
      lock (PdfGraphics.s_transparencyLock)
        PdfGraphics.m_transparencyObject = true;
    }
    if (brush is PdfTilingBrush)
    {
      this.m_bCSInitialized = false;
      float x1 = this.m_matrix.OffsetX + x;
      float y1 = this.Layer == null || this.Layer.Page == null ? this.ClientSize.Height - this.m_matrix.OffsetY + y : this.Layer.Page.Size.Height - this.m_matrix.OffsetY + y;
      (brush as PdfTilingBrush).Location = new PointF(x1, y1);
      (brush as PdfTilingBrush).Graphics.ColorSpace = this.ColorSpace;
    }
    else if (brush is PdfGradientBrush)
    {
      this.m_bCSInitialized = false;
      (brush as PdfGradientBrush).ColorSpace = this.ColorSpace;
    }
    if (brush is PdfSolidBrush && (brush as PdfSolidBrush).Color.IsEmpty)
      brush = (PdfBrush) null;
    this.StateControl(pen, brush, (PdfFont) null);
    this.StreamWriter.AppendRectangle(x, y, width, height);
    this.DrawPath(pen, brush, false);
    if (flag)
      this.StreamWriter.Write("EMC" + Environment.NewLine);
    this.EndMarkContent();
  }

  internal void DrawRoundedRectangle(RectangleF bounds, int radius, PdfPen pen, PdfBrush brush)
  {
    this.DrawRoundedRectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height, radius, pen, brush);
  }

  internal void DrawRoundedRectangle(
    float x,
    float y,
    float width,
    float height,
    int radius,
    PdfPen pen,
    PdfBrush brush)
  {
    if (pen == null)
      throw new ArgumentNullException(nameof (pen));
    if (brush == null)
      throw new ArgumentNullException(nameof (brush));
    RectangleF rectangle1 = new RectangleF(x, y, width, height);
    int num = radius * 2;
    SizeF size = new SizeF((float) num, (float) num);
    RectangleF rectangle2 = new RectangleF(rectangle1.Location, size);
    PdfPath path = new PdfPath();
    if (radius == 0)
    {
      path.AddRectangle(rectangle1);
      this.DrawPath(pen, brush, path);
    }
    else
    {
      path.AddArc(rectangle2, 180f, 90f);
      rectangle2.X = rectangle1.Right - (float) num;
      path.AddArc(rectangle2, 270f, 90f);
      rectangle2.Y = rectangle1.Bottom - (float) num;
      path.AddArc(rectangle2, 0.0f, 90f);
      rectangle2.X = rectangle1.Left;
      path.AddArc(rectangle2, 90f, 90f);
      path.CloseFigure();
      this.DrawPath(pen, brush, path);
    }
  }

  public void DrawEllipse(PdfPen pen, RectangleF rectangle)
  {
    this.DrawEllipse(pen, (PdfBrush) null, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
  }

  public void DrawEllipse(PdfPen pen, float x, float y, float width, float height)
  {
    this.DrawEllipse(pen, (PdfBrush) null, x, y, width, height);
  }

  public void DrawEllipse(PdfBrush brush, RectangleF rectangle)
  {
    this.DrawEllipse((PdfPen) null, brush, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
  }

  public void DrawEllipse(PdfBrush brush, float x, float y, float width, float height)
  {
    this.DrawEllipse((PdfPen) null, brush, x, y, width, height);
  }

  public void DrawEllipse(PdfPen pen, PdfBrush brush, RectangleF rectangle)
  {
    this.DrawEllipse(pen, brush, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
  }

  public void DrawEllipse(
    PdfPen pen,
    PdfBrush brush,
    float x,
    float y,
    float width,
    float height)
  {
    this.BeginMarkContent();
    bool flag = false;
    if (!this.IsTemplateGraphics && (this.Tag != null || this.IsTaggedPdf))
    {
      if (this.Tag == null)
        this.Tag = (PdfTag) new PdfStructureElement(PdfTagType.Figure);
      this.Tag.Bounds = new RectangleF(x, y, width, height);
      this.StructElementChanged += new PdfGraphics.StructElementEventHandler(this.ApplyTag);
      this.CurrentTagType = "Figure";
      this.Tag = (PdfTag) null;
      flag = true;
    }
    if (brush is PdfTilingBrush)
    {
      this.m_bCSInitialized = true;
      float x1 = this.m_matrix.OffsetX + x;
      float y1 = this.Layer == null || this.Layer.Page == null ? this.ClientSize.Height - this.m_matrix.OffsetY + y : this.Layer.Page.Size.Height - this.m_matrix.OffsetY + y;
      (brush as PdfTilingBrush).Location = new PointF(x1, y1);
      (brush as PdfTilingBrush).Graphics.ColorSpace = this.ColorSpace;
    }
    this.StateControl(pen, brush, (PdfFont) null);
    this.ConstructArcPath(x, y, x + width, y + height, 0.0f, 360f);
    this.DrawPath(pen, brush, true);
    if (flag)
      this.StreamWriter.Write("EMC" + Environment.NewLine);
    this.EndMarkContent();
  }

  public void DrawArc(PdfPen pen, RectangleF rectangle, float startAngle, float sweepAngle)
  {
    bool flag = false;
    if (!this.IsTemplateGraphics && (this.Tag != null || this.IsTaggedPdf))
    {
      if (this.Tag == null)
        this.Tag = (PdfTag) new PdfStructureElement(PdfTagType.Figure);
      this.Tag.Bounds = rectangle;
      this.StructElementChanged += new PdfGraphics.StructElementEventHandler(this.ApplyTag);
      this.CurrentTagType = "Figure";
      flag = true;
    }
    GraphicsPath graphicsPath = new GraphicsPath();
    graphicsPath.AddArc(rectangle, startAngle, sweepAngle);
    PointF[] pathPoints = graphicsPath.PathPoints;
    byte[] pathTypes = graphicsPath.PathTypes;
    PdfPath path = new PdfPath(pen, pathPoints, pathTypes);
    this.DrawPath(pen, path);
    graphicsPath.Dispose();
    if (!flag)
      return;
    this.Tag = (PdfTag) null;
  }

  public void DrawArc(
    PdfPen pen,
    float x,
    float y,
    float width,
    float height,
    float startAngle,
    float sweepAngle)
  {
    this.BeginMarkContent();
    bool flag = false;
    if (!this.IsTemplateGraphics && (this.Tag != null || this.IsTaggedPdf))
    {
      if (this.Tag == null)
        this.Tag = (PdfTag) new PdfStructureElement(PdfTagType.Figure);
      this.Tag.Bounds = new RectangleF(x, y, width, height);
      this.StructElementChanged += new PdfGraphics.StructElementEventHandler(this.ApplyTag);
      this.CurrentTagType = "Figure";
      flag = true;
    }
    if ((double) sweepAngle != 0.0)
    {
      this.StateControl(pen, (PdfBrush) null, (PdfFont) null);
      this.ConstructArcPath(x, y, x + width, y + height, startAngle, sweepAngle);
      this.DrawPath(pen, (PdfBrush) null, false);
    }
    if (flag)
      this.StreamWriter.Write("EMC" + Environment.NewLine);
    this.EndMarkContent();
  }

  public void DrawPie(PdfPen pen, RectangleF rectangle, float startAngle, float sweepAngle)
  {
    this.DrawPie(pen, (PdfBrush) null, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, startAngle, sweepAngle);
  }

  public void DrawPie(
    PdfPen pen,
    float x,
    float y,
    float width,
    float height,
    float startAngle,
    float sweepAngle)
  {
    this.DrawPie(pen, (PdfBrush) null, x, y, width, height, startAngle, sweepAngle);
  }

  public void DrawPie(PdfBrush brush, RectangleF rectangle, float startAngle, float sweepAngle)
  {
    bool flag = false;
    if (!this.IsTemplateGraphics && (this.Tag != null || this.IsTaggedPdf))
    {
      if (this.Tag == null)
        this.Tag = (PdfTag) new PdfStructureElement(PdfTagType.Figure);
      this.Tag.Bounds = rectangle;
      this.StructElementChanged += new PdfGraphics.StructElementEventHandler(this.ApplyTag);
      this.CurrentTagType = "Figure";
      flag = true;
    }
    GraphicsPath graphicsPath = new GraphicsPath();
    graphicsPath.AddPie(new Rectangle((int) rectangle.X, (int) rectangle.Y, (int) rectangle.Width, (int) rectangle.Height), startAngle, sweepAngle);
    PdfPath path = new PdfPath(graphicsPath.PathPoints, graphicsPath.PathTypes);
    this.DrawPath(brush, path);
    graphicsPath.Dispose();
    if (!flag)
      return;
    this.Tag = (PdfTag) null;
  }

  public void DrawPie(
    PdfBrush brush,
    float x,
    float y,
    float width,
    float height,
    float startAngle,
    float sweepAngle)
  {
    this.DrawPie((PdfPen) null, brush, x, y, width, height, startAngle, sweepAngle);
  }

  public void DrawPie(
    PdfPen pen,
    PdfBrush brush,
    RectangleF rectangle,
    float startAngle,
    float sweepAngle)
  {
    this.DrawPie(pen, brush, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, startAngle, sweepAngle);
  }

  public void DrawPie(
    PdfPen pen,
    PdfBrush brush,
    float x,
    float y,
    float width,
    float height,
    float startAngle,
    float sweepAngle)
  {
    this.BeginMarkContent();
    if (!this.IsTemplateGraphics && (this.Tag != null || this.IsTaggedPdf))
    {
      if (this.Tag == null)
        this.Tag = (PdfTag) new PdfStructureElement(PdfTagType.Figure);
      this.Tag.Bounds = new RectangleF(x, y, width, height);
      this.StructElementChanged += new PdfGraphics.StructElementEventHandler(this.ApplyTag);
      this.CurrentTagType = "Figure";
    }
    if ((double) sweepAngle == 0.0)
      return;
    switch (brush)
    {
      case PdfTilingBrush _:
        this.m_bCSInitialized = false;
        float x1 = this.m_matrix.OffsetX + x;
        float y1 = this.Layer == null || this.Layer.Page == null ? this.ClientSize.Height - this.m_matrix.OffsetY + y : this.Layer.Page.Size.Height - this.m_matrix.OffsetY + y;
        (brush as PdfTilingBrush).Location = new PointF(x1, y1);
        (brush as PdfTilingBrush).Graphics.ColorSpace = this.ColorSpace;
        break;
      case PdfGradientBrush _:
        this.m_bCSInitialized = false;
        (brush as PdfGradientBrush).ColorSpace = this.ColorSpace;
        break;
    }
    this.StateControl(pen, brush, (PdfFont) null);
    this.ConstructArcPath(x, y, x + width, y + height, startAngle, sweepAngle);
    this.m_streamWriter.AppendLineSegment(x + width / 2f, y + height / 2f);
    this.DrawPath(pen, brush, true);
    if (!this.IsTemplateGraphics && (this.Tag != null || this.IsTaggedPdf))
      this.StreamWriter.Write("EMC" + Environment.NewLine);
    this.EndMarkContent();
  }

  public void DrawPolygon(PdfPen pen, PointF[] points)
  {
    this.DrawPolygon(pen, (PdfBrush) null, points);
  }

  public void DrawPolygon(PdfBrush brush, PointF[] points)
  {
    this.DrawPolygon((PdfPen) null, brush, points);
  }

  public void DrawPolygon(PdfPen pen, PdfBrush brush, PointF[] points)
  {
    this.BeginMarkContent();
    bool flag = false;
    if (!this.IsTemplateGraphics && (this.Tag != null || this.IsTaggedPdf))
    {
      if (this.Tag == null)
        this.Tag = (PdfTag) new PdfStructureElement(PdfTagType.Figure);
      PdfPath pdfPath = new PdfPath();
      pdfPath.AddLines(points);
      this.Tag.Bounds = pdfPath.GetBounds();
      this.StructElementChanged += new PdfGraphics.StructElementEventHandler(this.ApplyTag);
      this.CurrentTagType = "Figure";
      this.Tag = (PdfTag) null;
      flag = true;
    }
    switch (brush)
    {
      case PdfTilingBrush _:
        this.m_bCSInitialized = false;
        (brush as PdfTilingBrush).Graphics.ColorSpace = this.ColorSpace;
        break;
      case PdfGradientBrush _:
        this.m_bCSInitialized = false;
        (brush as PdfGradientBrush).ColorSpace = this.ColorSpace;
        break;
    }
    int length = points.Length;
    if (length <= 0)
      return;
    this.StateControl(pen, brush, (PdfFont) null);
    this.m_streamWriter.BeginPath(points[0]);
    for (int index = 1; index < length; ++index)
      this.m_streamWriter.AppendLineSegment(points[index]);
    this.DrawPath(pen, brush, true);
    if (flag)
      this.StreamWriter.Write("EMC" + Environment.NewLine);
    this.EndMarkContent();
  }

  public void DrawBezier(
    PdfPen pen,
    PointF startPoint,
    PointF firstControlPoint,
    PointF secondControlPoint,
    PointF endPoint)
  {
    this.DrawBezier(pen, startPoint.X, startPoint.Y, firstControlPoint.X, firstControlPoint.Y, secondControlPoint.X, secondControlPoint.Y, endPoint.X, endPoint.Y);
  }

  public void DrawBezier(
    PdfPen pen,
    float startPointX,
    float startPointY,
    float firstControlPointX,
    float firstControlPointY,
    float secondControlPointX,
    float secondControlPointY,
    float endPointX,
    float endPointY)
  {
    this.BeginMarkContent();
    bool flag = false;
    if (!this.IsTemplateGraphics && (this.Tag != null || this.IsTaggedPdf))
    {
      if (this.Tag == null)
        this.Tag = (PdfTag) new PdfStructureElement(PdfTagType.Figure);
      PdfPath pdfPath = new PdfPath();
      pdfPath.AddLine(startPointX, startPointY, endPointX, endPointY);
      this.Tag.Bounds = pdfPath.GetBounds();
      this.StructElementChanged += new PdfGraphics.StructElementEventHandler(this.ApplyTag);
      this.CurrentTagType = "Figure";
      this.Tag = (PdfTag) null;
      flag = true;
    }
    this.StateControl(pen, (PdfBrush) null, (PdfFont) null);
    this.CapControl(pen, secondControlPointX, secondControlPointY, endPointX, endPointY);
    this.CapControl(pen, firstControlPointX, firstControlPointY, secondControlPointX, startPointY);
    PdfStreamWriter streamWriter = this.StreamWriter;
    streamWriter.BeginPath(startPointX, startPointY);
    streamWriter.AppendBezierSegment(firstControlPointX, firstControlPointY, secondControlPointX, secondControlPointY, endPointX, endPointY);
    streamWriter.StrokePath();
    if (flag)
      this.StreamWriter.Write("EMC" + Environment.NewLine);
    this.EndMarkContent();
  }

  public void DrawPath(PdfPen pen, PdfPath path) => this.DrawPath(pen, (PdfBrush) null, path);

  public void DrawPath(PdfBrush brush, PdfPath path) => this.DrawPath((PdfPen) null, brush, path);

  public void DrawPath(PdfPen pen, PdfBrush brush, PdfPath path)
  {
    this.BeginMarkContent();
    if (this.Tag == null || path.PdfTag != null)
      this.Tag = path.PdfTag;
    if (!this.IsTemplateGraphics && (this.Tag != null || this.IsTaggedPdf) || this.IsTaggedPdf && !this.m_isEMF)
    {
      if (this.Tag == null)
        this.Tag = (PdfTag) new PdfStructureElement(PdfTagType.Figure);
      this.Tag.Bounds = path.GetBounds();
      this.StructElementChanged += new PdfGraphics.StructElementEventHandler(this.ApplyTag);
      this.CurrentTagType = "Figure";
    }
    switch (brush)
    {
      case PdfTilingBrush _:
        this.m_bCSInitialized = false;
        (brush as PdfTilingBrush).Graphics.ColorSpace = this.ColorSpace;
        break;
      case PdfGradientBrush _:
        this.m_bCSInitialized = false;
        (brush as PdfGradientBrush).ColorSpace = this.ColorSpace;
        break;
    }
    this.StateControl(pen, brush, (PdfFont) null);
    this.BuildUpPath(path);
    this.DrawPath(pen, brush, path.FillMode, false);
    if (!this.IsTemplateGraphics && (this.Tag != null || this.IsTaggedPdf))
      this.StreamWriter.Write("EMC" + Environment.NewLine);
    this.EndMarkContent();
  }

  public void DrawImage(PdfImage image, PointF point)
  {
    if (image == null)
      throw new ArgumentNullException(nameof (image));
    this.DrawImage(image, point.X, point.Y);
  }

  public void DrawImage(PdfImage image, float x, float y)
  {
    SizeF sizeF = image != null ? image.PhysicalDimension : throw new ArgumentNullException(nameof (image));
    this.DrawImage(image, x, y, sizeF.Width, sizeF.Height);
  }

  public void DrawImage(PdfImage image, RectangleF rectangle)
  {
    if (image == null)
      throw new ArgumentNullException(nameof (image));
    if (this.IsDirectPDF)
      this.OnDrawPrimitive();
    this.DrawImage(image, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
  }

  public void DrawImage(PdfImage image, PointF point, SizeF size)
  {
    if (image == null)
      throw new ArgumentNullException(nameof (image));
    this.DrawImage(image, point.X, point.Y, size.Width, size.Height);
  }

  public void DrawImage(PdfImage image, float x, float y, float width, float height)
  {
    this.BeginMarkContent();
    bool flag = false;
    if (image == null)
      throw new ArgumentNullException(nameof (image));
    if ((double) this.ClientSize.Height < 0.0)
      y += this.ClientSize.Height;
    if (!this.isImageOptimized)
    {
      if (this.m_layer != null && this.Page != null)
      {
        if (this.Page is PdfPage && (this.Page as PdfPage).Document != null)
          image.m_tiffCompressionLevel = (this.Page as PdfPage).Document.Compression;
        else if (this.Page is PdfLoadedPage && (this.Page as PdfLoadedPage).Document != null)
          image.m_tiffCompressionLevel = (this.Page as PdfLoadedPage).Document.Compression;
      }
      image.Save();
    }
    this.isImageOptimized = false;
    if (this.Layer != null && this.Page != null && this.Page is PdfPage)
    {
      if ((this.Page as PdfPage).Section.ParentDocument is PdfDocument)
      {
        PdfStructTreeRoot structTreeRoot = PdfCatalog.StructTreeRoot;
        if (structTreeRoot != null)
        {
          structTreeRoot.m_isImage = true;
          RectangleF bounds = new RectangleF(x, y, width, height);
          if (image.PdfTag != null && image.PdfTag is PdfStructureElement)
          {
            int num = structTreeRoot.Add(image.PdfTag as PdfStructureElement, this.Page, bounds);
            this.StreamWriter.WriteTag(string.Format($"/{{0}} <</E ({(image.PdfTag as PdfStructureElement).Abbrevation}) /MCID {{1}} >>BDC", (object) structTreeRoot.ConvertToEquivalentTag((image.PdfTag as PdfStructureElement).TagType), (object) num));
            flag = true;
          }
          else if (image.PdfTag != null && image.PdfTag is PdfArtifact)
          {
            this.StreamWriter.Write(string.Format("/Artifact << {0} >>BDC" + Environment.NewLine, (object) this.SetArtifact(this.Tag as PdfArtifact)));
            flag = true;
          }
          else if (this.IsTaggedPdf)
          {
            this.m_streamWriter.WriteTag($"/{"Figure"} <</MCID {structTreeRoot.Add(new PdfStructureElement(PdfTagType.Figure), this.Page, bounds)} >>BDC");
            flag = true;
          }
        }
      }
    }
    else
    {
      PdfStructTreeRoot structTreeRoot = PdfCatalog.StructTreeRoot;
      if (structTreeRoot != null)
      {
        structTreeRoot.m_isImage = true;
        if (this.Tag != null)
          image.PdfTag = this.Tag;
        if (image.PdfTag != null && image.PdfTag is PdfArtifact)
        {
          this.StreamWriter.Write(string.Format("/Artifact << {0} >>BDC" + Environment.NewLine, (object) this.SetArtifact(this.Tag as PdfArtifact)));
          flag = true;
        }
        else if (!this.isTemplateGraphics)
        {
          this.StreamWriter.WriteTag($"/{"Figure"} <</MCID {structTreeRoot.Add("Figure", "Image", RectangleF.Empty)} >>BDC");
          flag = true;
        }
      }
    }
    if (this.m_isTransparentBrush && !this.m_istransparencySet)
    {
      float num = 1f;
      PdfBlendMode blendMode = PdfBlendMode.Normal;
      this.SetTransparency(num, num, blendMode);
    }
    PdfGraphicsState state = this.Save();
    PdfTransformationMatrix transformationMatrix = new PdfTransformationMatrix();
    this.GetTranslateTransform(x, y + height, transformationMatrix);
    if (image.InternalImage is Metafile)
      this.GetScaleTransform(width / (float) image.Width, height / (float) image.Height, transformationMatrix);
    else
      this.GetScaleTransform(width, height, transformationMatrix);
    this.m_streamWriter.ModifyCTM(transformationMatrix);
    PdfResources res = this.m_getResources();
    if (this.m_layer != null && this.Page is PdfPage && image.InternalImage is Bitmap && (this.Page as PdfPage).Document != null)
      res.Document = (this.Page as PdfPage).Document;
    PdfName name = res.GetName((IPdfWrapper) image);
    if (this.m_layer != null)
    {
      if (res.Document == null)
      {
        this.Page.SetResources(res);
      }
      else
      {
        PdfDictionary pdfDictionary1 = new PdfDictionary();
        pdfDictionary1[name] = (IPdfPrimitive) new PdfReferenceHolder(((IPdfWrapper) image).Element);
        if (res.ContainsKey("XObject"))
        {
          if (!(res[new PdfName("XObject")] is PdfDictionary pdfDictionary2) && (object) (res["XObject"] as PdfReferenceHolder) != null)
            pdfDictionary2 = (res["XObject"] as PdfReferenceHolder).Object as PdfDictionary;
          if (!pdfDictionary2.Items.ContainsKey(name))
            pdfDictionary2.Items.Add(name, pdfDictionary1[name]);
        }
        else
          res["XObject"] = (IPdfPrimitive) pdfDictionary1;
        this.Page.SetResources(res);
      }
    }
    this.m_streamWriter.ExecuteObject(name);
    lock (PdfGraphics.s_transparencyLock)
    {
      if (image.SoftMask)
        PdfGraphics.m_transparencyObject = true;
      if (PdfGraphics.m_transparencyObject)
      {
        if (this.Layer != null)
        {
          if (!this.Page.Dictionary.ContainsKey("Group"))
            this.SetTransparencyGroup(this.Page);
        }
      }
    }
    this.Restore(state);
    if (flag)
      this.m_streamWriter.WriteTag("EMC");
    this.EndMarkContent();
    this.m_getResources().RequireProcSet("ImageB");
    this.m_getResources().RequireProcSet("ImageC");
    this.m_getResources().RequireProcSet("ImageI");
    this.m_getResources().RequireProcSet("Text");
  }

  private IPdfPrimitive SetPageXobject(PdfDictionary pageDictionary, PdfName name, PdfImage image)
  {
    PdfDictionary pdfDictionary1 = pageDictionary["XObject"] as PdfDictionary;
    PdfReferenceHolder page = pageDictionary["XObject"] as PdfReferenceHolder;
    PdfDictionary pdfDictionary2 = new PdfDictionary();
    if (pdfDictionary1 == null && page != (PdfReferenceHolder) null)
      pdfDictionary2 = page.Object as PdfDictionary;
    if (pdfDictionary1 == null)
    {
      pdfDictionary1 = new PdfDictionary();
      pageDictionary["XObject"] = (IPdfPrimitive) pdfDictionary1;
      foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in pdfDictionary2.Items)
        pdfDictionary1[keyValuePair.Key] = (IPdfPrimitive) (keyValuePair.Value as PdfReferenceHolder);
    }
    pdfDictionary1[name] = (IPdfPrimitive) new PdfReferenceHolder(((IPdfWrapper) image).Element);
    return pdfDictionary1[name];
  }

  private IPdfPrimitive GetPageXObject(PdfDictionary pageDictionary, PdfName name, PdfImage image)
  {
    PdfDictionary page = pageDictionary["Resources"] as PdfDictionary;
    IPdfPrimitive pageXobject = (IPdfPrimitive) null;
    foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in page.Items)
    {
      if (keyValuePair.Key.Value == "XObject")
      {
        PdfDictionary pdfDictionary = page[keyValuePair.Key] as PdfDictionary;
        if (pdfDictionary.ContainsKey(name))
          pageXobject = pdfDictionary[name];
      }
    }
    return pageXobject;
  }

  internal void SetTransparencyGroup(PdfPageBase page)
  {
    PdfDictionary pdfDictionary = new PdfDictionary();
    pdfDictionary.SetName("CS", "DeviceRGB");
    pdfDictionary.SetBoolean("K", false);
    pdfDictionary.SetName("S", "Transparency");
    pdfDictionary.SetBoolean("I", false);
    page.Dictionary["Group"] = (IPdfPrimitive) pdfDictionary;
  }

  public void DrawString(string s, PdfFont font, PdfBrush brush, PointF point)
  {
    this.DrawString(s, font, brush, point, (PdfStringFormat) null);
  }

  public void DrawString(
    string s,
    PdfFont font,
    PdfBrush brush,
    PointF point,
    PdfStringFormat format)
  {
    this.DrawString(s, font, brush, point.X, point.Y, format);
  }

  public void DrawString(string s, PdfFont font, PdfBrush brush, float x, float y)
  {
    this.DrawString(s, font, brush, x, y, (PdfStringFormat) null);
  }

  public void DrawString(
    string s,
    PdfFont font,
    PdfBrush brush,
    float x,
    float y,
    PdfStringFormat format)
  {
    this.DrawString(s, font, (PdfPen) null, brush, x, y, format);
  }

  public void DrawString(string s, PdfFont font, PdfPen pen, PointF point)
  {
    this.DrawString(s, font, pen, point, (PdfStringFormat) null);
  }

  public void DrawString(
    string s,
    PdfFont font,
    PdfPen pen,
    PointF point,
    PdfStringFormat format)
  {
    this.DrawString(s, font, pen, point.X, point.Y, format);
  }

  public void DrawString(string s, PdfFont font, PdfPen pen, float x, float y)
  {
    this.DrawString(s, font, pen, x, y, (PdfStringFormat) null);
  }

  public void DrawString(
    string s,
    PdfFont font,
    PdfPen pen,
    float x,
    float y,
    PdfStringFormat format)
  {
    this.DrawString(s, font, pen, (PdfBrush) null, x, y, format);
  }

  public void DrawString(string s, PdfFont font, PdfPen pen, PdfBrush brush, PointF point)
  {
    this.DrawString(s, font, pen, brush, point, (PdfStringFormat) null);
  }

  public void DrawString(
    string s,
    PdfFont font,
    PdfPen pen,
    PdfBrush brush,
    PointF point,
    PdfStringFormat format)
  {
    this.DrawString(s, font, pen, brush, point.X, point.Y, format);
  }

  public void DrawString(
    string s,
    PdfFont font,
    PdfPen pen,
    PdfBrush brush,
    float x,
    float y,
    PdfStringFormat format)
  {
    RectangleF layoutRectangle = new RectangleF(x, y, 0.0f, 0.0f);
    this.DrawString(s, font, pen, brush, layoutRectangle, format);
  }

  public void DrawString(string s, PdfFont font, PdfPen pen, PdfBrush brush, float x, float y)
  {
    this.DrawString(s, font, pen, brush, x, y, (PdfStringFormat) null);
  }

  public void DrawString(string s, PdfFont font, PdfBrush brush, RectangleF layoutRectangle)
  {
    this.DrawString(s, font, brush, layoutRectangle, (PdfStringFormat) null);
  }

  public void DrawString(
    string s,
    PdfFont font,
    PdfBrush brush,
    RectangleF layoutRectangle,
    PdfStringFormat format)
  {
    this.DrawString(s, font, (PdfPen) null, brush, layoutRectangle, format);
  }

  internal void DrawString(
    string s,
    PdfFont font,
    PdfBrush brush,
    RectangleF layoutRectangle,
    PdfStringFormat format,
    double maxRowFontSize,
    PdfFont maxPdfFont,
    PdfStringFormat maxPdfFormat)
  {
    this.DrawString(s, font, (PdfPen) null, brush, layoutRectangle, format, maxRowFontSize, maxPdfFont, maxPdfFormat);
  }

  public void DrawString(string s, PdfFont font, PdfPen pen, RectangleF layoutRectangle)
  {
    this.DrawString(s, font, pen, layoutRectangle, (PdfStringFormat) null);
  }

  public void DrawString(
    string s,
    PdfFont font,
    PdfPen pen,
    RectangleF layoutRectangle,
    PdfStringFormat format)
  {
    this.DrawString(s, font, pen, (PdfBrush) null, layoutRectangle, format);
  }

  public void DrawString(
    string s,
    PdfFont font,
    PdfPen pen,
    PdfBrush brush,
    RectangleF layoutRectangle,
    PdfStringFormat format)
  {
    if (s == null)
      throw new ArgumentNullException(nameof (s));
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    if (!this.isStandardUnicode)
      s = PdfGraphics.NormalizeText(font, s);
    if (this.IsDirectPDF && format != null && format.TextDirection != PdfTextDirection.RightToLeft && PdfString.IsUnicode(s))
    {
      foreach (char input in s.ToCharArray())
      {
        if (this.IsRTLChar(input))
        {
          format.TextDirection = PdfTextDirection.RightToLeft;
          format.isCustomRendering = true;
          break;
        }
      }
    }
    PdfStringLayouter pdfStringLayouter = new PdfStringLayouter();
    PdfStringLayoutResult result = this.LayoutResult != null ? this.LayoutResult : pdfStringLayouter.Layout(s, font, format, layoutRectangle.Size);
    if (!result.Empty || this.IsTaggedPdf)
    {
      RectangleF rectangleF = this.CheckCorrectLayoutRectangle(result.ActualSize, layoutRectangle.X, layoutRectangle.Y, format);
      if ((double) layoutRectangle.Width <= 0.0)
      {
        layoutRectangle.X = rectangleF.X;
        layoutRectangle.Width = rectangleF.Width;
      }
      if ((double) layoutRectangle.Height <= 0.0)
      {
        layoutRectangle.Y = rectangleF.Y;
        layoutRectangle.Height = rectangleF.Height;
      }
      if (font.Name.ToLower().Contains("calibri") && this.m_isNormalRender && font.Style == PdfFontStyle.Regular && (format == null || format.LineAlignment != PdfVerticalAlignment.Bottom))
        this.m_isUseFontSize = true;
      if ((double) this.ClientSize.Height < 0.0)
        layoutRectangle.Y += this.ClientSize.Height;
      this.DrawStringLayoutResult(result, font, pen, brush, layoutRectangle, format);
      this.m_isEmfTextScaled = false;
      this.m_emfScalingFactor = SizeF.Empty;
    }
    this.m_getResources().RequireProcSet("Text");
    this.m_isNormalRender = true;
    this.m_stringLayoutResult = result;
    if (!this.IsDirectPDF)
      return;
    this.m_isUseFontSize = false;
  }

  internal void DrawString(
    string s,
    PdfFont font,
    PdfPen pen,
    PdfBrush brush,
    RectangleF layoutRectangle,
    PdfStringFormat format,
    double maxRowFontSize,
    PdfFont maxPdfFont,
    PdfStringFormat maxPdfFormat)
  {
    if (s == null)
      throw new ArgumentNullException(nameof (s));
    s = font != null ? PdfGraphics.NormalizeText(font, s) : throw new ArgumentNullException(nameof (font));
    PdfStringLayouter pdfStringLayouter = new PdfStringLayouter();
    PdfStringLayoutResult result = this.LayoutResult != null ? this.LayoutResult : pdfStringLayouter.Layout(s, font, format, layoutRectangle.Size);
    if (!result.Empty)
    {
      RectangleF rectangleF = this.CheckCorrectLayoutRectangle(result.ActualSize, layoutRectangle.X, layoutRectangle.Y, format);
      if ((double) layoutRectangle.Width <= 0.0)
      {
        layoutRectangle.X = rectangleF.X;
        layoutRectangle.Width = rectangleF.Width;
      }
      if ((double) layoutRectangle.Height <= 0.0)
      {
        layoutRectangle.Y = rectangleF.Y;
        layoutRectangle.Height = rectangleF.Height;
      }
      if (font.Name.ToLower().Contains("calibri") && this.m_isNormalRender && font.Style == PdfFontStyle.Regular && (format == null || format.LineAlignment != PdfVerticalAlignment.Bottom))
        this.m_isUseFontSize = true;
      if ((double) this.ClientSize.Height < 0.0)
        layoutRectangle.Y += this.ClientSize.Height;
      this.DrawStringLayoutResult(result, font, pen, brush, layoutRectangle, format, maxRowFontSize, maxPdfFont, maxPdfFormat);
    }
    this.m_getResources().RequireProcSet("Text");
    this.m_isNormalRender = true;
    this.m_stringLayoutResult = result;
  }

  internal bool IsRTLChar(char input)
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

  public void TranslateTransform(float offsetX, float offsetY)
  {
    PdfTransformationMatrix transformationMatrix = new PdfTransformationMatrix();
    this.GetTranslateTransform(offsetX, offsetY, transformationMatrix);
    this.m_streamWriter.ModifyCTM(transformationMatrix);
    this.Matrix.Multiply(transformationMatrix);
  }

  public void ScaleTransform(float scaleX, float scaleY)
  {
    PdfTransformationMatrix transformationMatrix = new PdfTransformationMatrix();
    this.GetScaleTransform(scaleX, scaleY, transformationMatrix);
    this.m_streamWriter.ModifyCTM(transformationMatrix);
    this.Matrix.Multiply(transformationMatrix);
  }

  public void RotateTransform(float angle)
  {
    PdfTransformationMatrix transformationMatrix = new PdfTransformationMatrix();
    this.GetRotateTransform(angle, transformationMatrix);
    this.m_streamWriter.ModifyCTM(transformationMatrix);
    this.Matrix.Multiply(transformationMatrix);
  }

  public void SkewTransform(float angleX, float angleY)
  {
    PdfTransformationMatrix transformationMatrix = new PdfTransformationMatrix();
    this.GetSkewTransform(angleX, angleY, transformationMatrix);
    this.m_streamWriter.ModifyCTM(transformationMatrix);
    this.Matrix.Multiply(transformationMatrix);
  }

  internal void MultiplyTransform(PdfTransformationMatrix matrix)
  {
    this.m_streamWriter.ModifyCTM(matrix);
  }

  public void DrawPdfTemplate(PdfTemplate template, PointF location)
  {
    if (template == null)
      throw new ArgumentNullException(nameof (template));
    this.DrawPdfTemplate(template, location, template.Size);
  }

  public void DrawPdfTemplate(PdfTemplate template, PointF location, SizeF size)
  {
    lock (PdfGraphics.s_syncLockTemplate)
    {
      PdfCrossTable crossTable = (PdfCrossTable) null;
      this.BeginMarkContent();
      if (this.m_layer != null || this.m_documentLayer != null)
      {
        bool flag = false;
        if (this.Page is PdfLoadedPage)
        {
          crossTable = (this.Page as PdfLoadedPage).Document.CrossTable;
          flag = (this.Page as PdfLoadedPage).Document.EnableMemoryOptimization;
        }
        else if (this.Page is PdfPage)
        {
          crossTable = (this.Page as PdfPage).Section.ParentDocument.CrossTable;
          flag = (this.Page as PdfPage).Section.ParentDocument.EnableMemoryOptimization;
        }
        if (template.ReadOnly && flag || template.isLoadedPageTemplate)
          template.CloneResources(crossTable);
      }
      if (template == null)
        throw new ArgumentNullException(nameof (template));
      float num1 = (double) template.Width > 0.0 ? size.Width / template.Width : 1f;
      float num2 = (double) template.Height > 0.0 ? size.Height / template.Height : 1f;
      bool flag1 = (double) num1 != 1.0 || (double) num2 != 1.0;
      if ((this.m_layer != null || this.m_documentLayer != null) && this.Page != null && this.Page.Dictionary.ContainsKey("CropBox") && this.Page.Dictionary.ContainsKey("MediaBox"))
      {
        PdfArray pdfArray1 = (object) (this.Page.Dictionary["CropBox"] as PdfReferenceHolder) == null ? this.Page.Dictionary["CropBox"] as PdfArray : (this.Page.Dictionary["CropBox"] as PdfReferenceHolder).Object as PdfArray;
        PdfArray pdfArray2 = (object) (this.Page.Dictionary["MediaBox"] as PdfReferenceHolder) == null ? this.Page.Dictionary["MediaBox"] as PdfArray : (this.Page.Dictionary["MediaBox"] as PdfReferenceHolder).Object as PdfArray;
        float floatValue1 = (pdfArray2[0] as PdfNumber).FloatValue;
        float floatValue2 = (pdfArray2[1] as PdfNumber).FloatValue;
        float floatValue3 = (pdfArray1[0] as PdfNumber).FloatValue;
        float floatValue4 = (pdfArray1[3] as PdfNumber).FloatValue;
        if ((double) floatValue3 > 0.0 && (double) floatValue4 > 0.0 && (double) floatValue1 < 0.0 && (double) floatValue2 < 0.0)
        {
          this.TranslateTransform(floatValue3, -floatValue4);
          location.X = -floatValue3;
          location.Y = floatValue4;
        }
      }
      PdfGraphicsState state = this.Save();
      PdfTransformationMatrix transformationMatrix = new PdfTransformationMatrix();
      if (this.m_layer != null || this.m_documentLayer != null && this.Page != null)
      {
        bool flag2 = false;
        if (this.Page.Dictionary.ContainsKey("CropBox") && this.Page.Dictionary.ContainsKey("MediaBox"))
        {
          PdfArray pdfArray3 = (object) (this.Page.Dictionary["CropBox"] as PdfReferenceHolder) == null ? this.Page.Dictionary["CropBox"] as PdfArray : (this.Page.Dictionary["CropBox"] as PdfReferenceHolder).Object as PdfArray;
          PdfArray pdfArray4 = (object) (this.Page.Dictionary["MediaBox"] as PdfReferenceHolder) == null ? this.Page.Dictionary["MediaBox"] as PdfArray : (this.Page.Dictionary["MediaBox"] as PdfReferenceHolder).Object as PdfArray;
          if (pdfArray3 != null && pdfArray4 != null && pdfArray3.ToRectangle() == pdfArray4.ToRectangle())
            flag2 = true;
        }
        if (this.Page.Dictionary.ContainsKey("MediaBox"))
        {
          PdfArray pdfArray = (object) (this.Page.Dictionary["MediaBox"] as PdfReferenceHolder) == null ? this.Page.Dictionary["MediaBox"] as PdfArray : (this.Page.Dictionary["MediaBox"] as PdfReferenceHolder).Object as PdfArray;
          if (pdfArray != null && (double) (pdfArray[3] as PdfNumber).FloatValue == 0.0)
            flag2 = true;
        }
        if ((double) this.Page.Origin.X >= 0.0 && (double) this.Page.Origin.Y >= 0.0 || flag2)
          this.GetTranslateTransform(location.X, location.Y + size.Height, transformationMatrix);
        else if ((double) this.Page.Origin.X >= 0.0 && (double) this.Page.Origin.Y <= 0.0)
          this.GetTranslateTransform(location.X, location.Y + size.Height, transformationMatrix);
        else
          this.GetTranslateTransform(location.X, location.Y + 0.0f, transformationMatrix);
      }
      else
        this.GetTranslateTransform(location.X, location.Y + size.Height, transformationMatrix);
      if (flag1)
      {
        if (template.IsAnnotationTemplate && template.NeedScaling)
        {
          bool flag3 = false;
          if (template.m_content != null && template.m_content.ContainsKey("Matrix") && template.m_content.ContainsKey("BBox"))
          {
            PdfArray pdfArray5 = PdfCrossTable.Dereference(template.m_content["Matrix"]) as PdfArray;
            PdfArray pdfArray6 = PdfCrossTable.Dereference(template.m_content["BBox"]) as PdfArray;
            if (pdfArray5 != null && pdfArray6 != null && pdfArray5.Count > 5 && pdfArray6.Count > 3)
            {
              float num3 = -(pdfArray5[1] as PdfNumber).FloatValue;
              float floatValue5 = (pdfArray5[2] as PdfNumber).FloatValue;
              float floatValue6 = (pdfArray5[4] as PdfNumber).FloatValue;
              float floatValue7 = (pdfArray5[5] as PdfNumber).FloatValue;
              float num4 = (float) Math.Round((double) num3, 2);
              float num5 = (float) Math.Round((double) floatValue5, 2);
              float num6 = (float) Math.Round((double) num1, 2);
              float num7 = (float) Math.Round((double) num2, 2);
              if ((double) num6 == (double) num4 && (double) num7 == (double) num5 && (double) (pdfArray6[2] as PdfNumber).FloatValue == (double) template.Size.Width && (double) (pdfArray6[3] as PdfNumber).FloatValue == (double) template.Size.Height)
              {
                transformationMatrix = new PdfTransformationMatrix();
                this.GetTranslateTransform(location.X - floatValue6, location.Y + floatValue7, transformationMatrix);
                this.GetScaleTransform(1f, 1f, transformationMatrix);
                flag3 = true;
              }
            }
          }
          if (!flag3)
            this.GetScaleTransform(num2, num1, transformationMatrix);
        }
        else
          this.GetScaleTransform(num1, num2, transformationMatrix);
      }
      if (template.Graphics != null && template.Graphics.Tag != null || this.IsTaggedPdf)
      {
        PdfStructTreeRoot structTreeRoot = PdfCatalog.StructTreeRoot;
        RectangleF bounds = new RectangleF(location.X, location.Y, size.Width, size.Height);
        if (structTreeRoot != null && template.Graphics.Tag is PdfStructureElement)
        {
          int num8 = structTreeRoot.Add(template.Graphics.Tag as PdfStructureElement, this.Page, bounds);
          this.StreamWriter.WriteTag(string.Format($"/{{0}} <</E ({(template.Graphics.Tag as PdfStructureElement).Abbrevation}) /MCID {{1}} >>BDC", (object) structTreeRoot.ConvertToEquivalentTag((template.Graphics.Tag as PdfStructureElement).TagType), (object) num8));
        }
        else
        {
          if (!this.Page.Dictionary.ContainsKey("Tabs"))
            this.Page.Dictionary["Tabs"] = (IPdfPrimitive) new PdfName("S");
          if (structTreeRoot != null && template.Graphics.Tag is PdfArtifact)
            this.StreamWriter.Write(string.Format("/Artifact << {0} >>BDC" + Environment.NewLine, (object) this.SetArtifact(template.Graphics.Tag as PdfArtifact)));
          else
            this.StreamWriter.WriteTag($"/{"Figure"} <</MCID {structTreeRoot.Add(new PdfStructureElement(PdfTagType.Figure), this.Page, bounds)} >>BDC");
        }
      }
      this.m_streamWriter.ModifyCTM(transformationMatrix);
      PdfResources pdfResources1 = this.m_getResources();
      pdfResources1.OriginalFontName = (string) null;
      this.m_streamWriter.ExecuteObject(pdfResources1.GetName((IPdfWrapper) template));
      if (template.Graphics != null && template.Graphics.Tag != null || this.IsTaggedPdf)
        this.StreamWriter.Write("EMC" + Environment.NewLine);
      this.EndMarkContent();
      this.Restore(state);
      PdfGraphics graphics = template.Graphics;
      if (graphics != null)
      {
        foreach (PdfAutomaticFieldInfo automaticField in (PdfCollection) graphics.AutomaticFields)
        {
          PointF location1 = new PointF(automaticField.Location.X + location.X, automaticField.Location.Y + location.Y);
          float scalingX = (double) template.Size.Width == 0.0 ? 0.0f : size.Width / template.Size.Width;
          float scalingY = (double) template.Size.Height == 0.0 ? 0.0f : size.Height / template.Size.Height;
          this.AutomaticFields.Add(new PdfAutomaticFieldInfo(automaticField.Field, location1, scalingX, scalingY));
          this.Page.Dictionary.Modify();
        }
      }
      this.m_getResources().RequireProcSet("ImageB");
      this.m_getResources().RequireProcSet("ImageC");
      this.m_getResources().RequireProcSet("ImageI");
      this.m_getResources().RequireProcSet("Text");
      if (!(this.Page is PdfPage page) || page.Document == null || !template.isLoadedPageTemplate)
        return;
      PdfResources pdfResources2 = this.m_getResources();
      page.DestinationDocument = (PdfDocumentBase) page.Document;
      page.templateResource = true;
      page.RemoveIdenticalResources(pdfResources2, page);
      page.Dictionary["Resources"] = (IPdfPrimitive) pdfResources2;
      page.SetResources(pdfResources2);
      page.templateResource = false;
    }
  }

  public void Flush()
  {
    if (!this.m_bStateSaved)
      return;
    this.m_streamWriter.RestoreGraphicsState();
    this.m_bStateSaved = false;
  }

  public PdfGraphicsState Save()
  {
    PdfGraphicsState pdfGraphicsState = new PdfGraphicsState(this, this.Matrix.Clone());
    pdfGraphicsState.Brush = this.m_currentBrush;
    pdfGraphicsState.Pen = this.m_currentPen;
    pdfGraphicsState.Font = this.m_currentFont;
    pdfGraphicsState.ColorSpace = this.m_currentColorSpace;
    pdfGraphicsState.CharacterSpacing = this.m_previousCharacterSpacing;
    pdfGraphicsState.WordSpacing = this.m_previousWordSpacing;
    pdfGraphicsState.TextScaling = this.m_previousTextScaling;
    pdfGraphicsState.TextRenderingMode = this.m_previousTextRenderingMode;
    this.m_graphicsState.Push(pdfGraphicsState);
    if (this.m_bStateSaved)
    {
      this.m_streamWriter.RestoreGraphicsState();
      this.m_bStateSaved = false;
    }
    this.m_streamWriter.SaveGraphicsState();
    return pdfGraphicsState;
  }

  public void Restore()
  {
    if (this.m_graphicsState.Count <= 0)
      return;
    this.DoRestoreState();
  }

  public void Restore(PdfGraphicsState state)
  {
    if (state == null)
      throw new ArgumentNullException(nameof (state));
    if (state.Graphics != this)
      throw new ArgumentException("The GraphicsState belongs to another Graphics object.", nameof (state));
    if (!this.m_graphicsState.Contains(state))
      return;
    do
      ;
    while (this.m_graphicsState.Count != 0 && this.DoRestoreState() != state);
  }

  public void SetClip(RectangleF rectangle) => this.SetClip(rectangle, PdfFillMode.Winding);

  public void SetClip(RectangleF rectangle, PdfFillMode mode)
  {
    this.m_streamWriter.AppendRectangle(rectangle);
    this.m_streamWriter.ClipPath(mode == PdfFillMode.Alternate);
  }

  public void SetClip(PdfPath path)
  {
    if (path == null)
      throw new ArgumentNullException(nameof (path));
    this.SetClip(path, path.FillMode);
  }

  public void SetClip(PdfPath path, PdfFillMode mode)
  {
    if (path == null)
      throw new ArgumentNullException(nameof (path));
    this.BuildUpPath(path);
    this.m_streamWriter.ClipPath(mode == PdfFillMode.Alternate);
  }

  public void SetTransparency(float alpha)
  {
    this.m_istransparencySet = true;
    this.SetTransparency(alpha, alpha, PdfBlendMode.Normal);
  }

  public void SetTransparency(float alphaPen, float alphaBrush)
  {
    this.SetTransparency(alphaPen, alphaBrush, PdfBlendMode.Normal);
  }

  public void SetTransparency(float alphaPen, float alphaBrush, PdfBlendMode blendMode)
  {
    if (this.m_trasparencies == null)
      this.m_trasparencies = new Dictionary<PdfGraphics.TransparencyData, PdfTransparency>();
    PdfTransparency pdfTransparency = (PdfTransparency) null;
    PdfGraphics.TransparencyData key = new PdfGraphics.TransparencyData(alphaPen, alphaBrush, blendMode);
    if (this.m_trasparencies.ContainsKey(key))
      pdfTransparency = this.m_trasparencies[key];
    if (pdfTransparency == null)
    {
      pdfTransparency = new PdfTransparency(alphaPen, alphaBrush, blendMode);
      this.m_trasparencies[key] = pdfTransparency;
    }
    this.StreamWriter.SetGraphicsState(this.m_getResources().GetName((IPdfWrapper) pdfTransparency));
  }

  internal void SetTextRenderingMode(TextRenderingMode mode)
  {
    this.m_textRenderingMode = mode;
    this.m_isTextRenderingSet = true;
  }

  internal static string NormalizeText(PdfFont font, string text)
  {
    PdfTrueTypeFont pdfTrueTypeFont = font as PdfTrueTypeFont;
    if (font is PdfStandardFont || pdfTrueTypeFont != null && !pdfTrueTypeFont.Unicode)
      text = !(font is PdfStandardFont pdfStandardFont) || pdfStandardFont.fontEncoding == null ? PdfStandardFont.Convert(text) : PdfStandardFont.Convert(text, pdfStandardFont.fontEncoding);
    return text;
  }

  internal void TranslateTransform(float offsetX, float offsetY, bool value)
  {
    PdfTransformationMatrix transformationMatrix = new PdfTransformationMatrix(value);
    this.GetTranslateTransform(offsetX, offsetY, transformationMatrix);
    this.m_streamWriter.ModifyCTM(transformationMatrix);
    this.Matrix.Multiply(transformationMatrix);
  }

  private void Initialize()
  {
    this.m_bStateSaved = false;
    this.m_currentPen = (PdfPen) null;
    this.m_currentBrush = (PdfBrush) null;
    this.m_currentFont = (PdfFont) null;
    this.m_currentColorSpace = PdfDocument.ConformanceLevel != PdfConformanceLevel.Pdf_X1A2001 ? PdfColorSpace.RGB : PdfColorSpace.CMYK;
    this.m_bCSInitialized = false;
    this.m_matrix = (PdfTransformationMatrix) null;
    this.m_previousTextRenderingMode = ~TextRenderingMode.Fill;
    this.m_previousCharacterSpacing = -1f;
    this.m_previousWordSpacing = -1f;
    this.m_previousTextScaling = -100f;
    this.m_trasparencies = (Dictionary<PdfGraphics.TransparencyData, PdfTransparency>) null;
    this.m_currentStringFormat = (PdfStringFormat) null;
    this.m_clipBounds = new RectangleF(PointF.Empty, this.Size);
    this.m_graphicsState = new Stack<PdfGraphicsState>();
    this.m_getResources().RequireProcSet("PDF");
  }

  internal void SetLayer(PdfPageLayer layer)
  {
    this.m_layer = layer;
    if (layer.Page is PdfPage page)
      page.BeginSave += new EventHandler(this.PageSave);
    else
      (layer.Page as PdfLoadedPage).BeginSave += new EventHandler(this.PageSave);
  }

  internal void SetLayer(PdfLayer layer)
  {
    this.m_documentLayer = layer;
    if (layer.Page is PdfPage page)
      page.BeginSave += new EventHandler(this.PageSave);
    else
      (layer.Page as PdfLoadedPage).BeginSave += new EventHandler(this.PageSave);
  }

  internal void EndMarkContent()
  {
    if (this.m_documentLayer == null)
      return;
    if (this.m_documentLayer.m_isEndState && this.m_documentLayer.m_parentLayer.Count != 0)
    {
      for (int index = 0; index < this.m_documentLayer.m_parentLayer.Count; ++index)
        this.StreamWriter.Write("EMC" + Environment.NewLine);
    }
    if (!this.m_documentLayer.m_isEndState)
      return;
    this.StreamWriter.Write("EMC" + Environment.NewLine);
  }

  private void BeginMarkContent()
  {
    if (this.m_documentLayer == null)
      return;
    this.m_documentLayer.BeginLayer(this);
  }

  private void PageSave(object sender, EventArgs e)
  {
    if (this.m_automaticFields == null)
      return;
    foreach (PdfAutomaticFieldInfo automaticField in (PdfCollection) this.m_automaticFields)
      automaticField.Field.PerformDraw(this, automaticField.Location, automaticField.ScalingX, automaticField.ScalingY);
  }

  internal static float UpdateY(float y) => -y;

  internal void PutComment(string comment) => this.m_streamWriter.WriteComment(comment);

  internal void Reset(SizeF size)
  {
    this.m_canvasSize = size;
    this.m_streamWriter.Clear();
    this.Initialize();
    this.InitializeCoordinates();
  }

  private PdfGraphicsState DoRestoreState()
  {
    PdfGraphicsState pdfGraphicsState = this.m_graphicsState.Pop();
    this.m_matrix = pdfGraphicsState.Matrix;
    this.m_currentBrush = pdfGraphicsState.Brush;
    this.m_currentPen = pdfGraphicsState.Pen;
    this.m_currentFont = pdfGraphicsState.Font;
    this.m_currentColorSpace = pdfGraphicsState.ColorSpace;
    this.m_previousCharacterSpacing = pdfGraphicsState.CharacterSpacing;
    this.m_previousWordSpacing = pdfGraphicsState.WordSpacing;
    this.m_previousTextScaling = pdfGraphicsState.TextScaling;
    this.m_previousTextRenderingMode = pdfGraphicsState.TextRenderingMode;
    this.m_streamWriter.RestoreGraphicsState();
    return pdfGraphicsState;
  }

  private void StateControl(PdfPen pen, PdfBrush brush, PdfFont font)
  {
    this.StateControl(pen, brush, font, (PdfStringFormat) null);
  }

  private void StateControl(PdfPen pen, PdfBrush brush, PdfFont font, PdfStringFormat format)
  {
    if ((pen != null && pen.Color.A == (byte) 0 || brush != null && brush is PdfSolidBrush && (brush as PdfSolidBrush).Color.A == (byte) 0) && this.Layer != null && !this.Layer.Page.Dictionary.ContainsKey("Group"))
      this.SetTransparencyGroup(this.Layer.Page);
    if (brush is PdfGradientBrush)
    {
      this.m_bCSInitialized = false;
      (brush as PdfGradientBrush).ColorSpace = this.ColorSpace;
    }
    if (brush is PdfTilingBrush)
    {
      this.m_bCSInitialized = false;
      (brush as PdfTilingBrush).Graphics.ColorSpace = this.ColorSpace;
    }
    bool saveState = false;
    if (brush != null)
    {
      if (brush is PdfSolidBrush pdfSolidBrush)
      {
        if (pdfSolidBrush.Colorspaces != null)
        {
          this.ColorSpaceControl(pdfSolidBrush.Colorspaces.ColorSpace);
        }
        else
        {
          if (this.m_layer != null)
          {
            if (this.m_layer.Page is PdfPage && (this.m_layer.Page as PdfPage).Section.ParentDocument.GetType().Name != "PdfLoadedDocument")
            {
              if (!this.colorSpaceChanged)
              {
                this.lastDocumentCS = (this.m_layer.Page as PdfPage).Document.ColorSpace;
                this.lastGraphicsCS = (this.m_layer.Page as PdfPage).Graphics.ColorSpace;
                if ((this.m_layer.Page as PdfPage).Document.ColorSpace == (this.m_layer.Page as PdfPage).Graphics.ColorSpace)
                {
                  this.ColorSpace = (this.m_layer.Page as PdfPage).Document.ColorSpace;
                  this.m_currentColorSpace = (this.m_layer.Page as PdfPage).Document.ColorSpace;
                }
                else if ((this.m_layer.Page as PdfPage).Document.ColorSpace != (this.m_layer.Page as PdfPage).Graphics.ColorSpace)
                {
                  this.ColorSpace = (this.m_layer.Page as PdfPage).Graphics.ColorSpace;
                  this.m_currentColorSpace = (this.m_layer.Page as PdfPage).Graphics.ColorSpace;
                }
                this.colorSpaceChanged = true;
              }
              else if ((this.m_layer.Page as PdfPage).Document.ColorSpace != this.lastDocumentCS)
              {
                this.ColorSpace = (this.m_layer.Page as PdfPage).Document.ColorSpace;
                this.m_currentColorSpace = (this.m_layer.Page as PdfPage).Document.ColorSpace;
                this.lastDocumentCS = (this.m_layer.Page as PdfPage).Document.ColorSpace;
              }
              else if ((this.m_layer.Page as PdfPage).Graphics.ColorSpace != this.lastGraphicsCS)
              {
                this.ColorSpace = (this.m_layer.Page as PdfPage).Graphics.ColorSpace;
                this.m_currentColorSpace = (this.m_layer.Page as PdfPage).Graphics.ColorSpace;
                this.lastGraphicsCS = (this.m_layer.Page as PdfPage).Graphics.ColorSpace;
              }
            }
            else if (this.m_layer.Page is PdfLoadedPage)
            {
              this.ColorSpace = ((this.m_layer.Page as PdfLoadedPage).Document as PdfLoadedDocument).ColorSpace;
              this.m_currentColorSpace = ((this.m_layer.Page as PdfLoadedPage).Document as PdfLoadedDocument).ColorSpace;
            }
          }
          this.InitCurrentColorSpace(this.m_currentColorSpace);
        }
      }
      else
      {
        if (this.m_layer != null)
        {
          if (this.m_layer.Page is PdfPage && (this.m_layer.Page as PdfPage).Section.ParentDocument.GetType().Name != "PdfLoadedDocument")
          {
            this.ColorSpace = (this.m_layer.Page as PdfPage).Document.ColorSpace;
            this.m_currentColorSpace = (this.m_layer.Page as PdfPage).Document.ColorSpace;
          }
          else if (this.m_layer.Page is PdfLoadedPage)
          {
            this.ColorSpace = ((this.m_layer.Page as PdfLoadedPage).Document as PdfLoadedDocument).ColorSpace;
            this.m_currentColorSpace = ((this.m_layer.Page as PdfLoadedPage).Document as PdfLoadedDocument).ColorSpace;
          }
        }
        this.InitCurrentColorSpace(this.m_currentColorSpace);
      }
    }
    else if (pen != null)
    {
      PdfPen pdfPen = pen;
      if (pdfPen != null)
      {
        if (pdfPen.Colorspaces != null)
        {
          this.ColorSpaceControl(pdfPen.Colorspaces.ColorSpace);
        }
        else
        {
          if (this.m_layer != null)
          {
            if (this.m_layer.Page is PdfPage && (this.m_layer.Page as PdfPage).Section.ParentDocument.GetType().Name != "PdfLoadedDocument")
            {
              this.ColorSpace = (this.m_layer.Page as PdfPage).Document.ColorSpace;
              this.m_currentColorSpace = (this.m_layer.Page as PdfPage).Document.ColorSpace;
            }
            else if (this.m_layer.Page is PdfLoadedPage)
            {
              this.ColorSpace = ((this.m_layer.Page as PdfLoadedPage).Document as PdfLoadedDocument).ColorSpace;
              this.m_currentColorSpace = ((this.m_layer.Page as PdfLoadedPage).Document as PdfLoadedDocument).ColorSpace;
            }
          }
          this.InitCurrentColorSpace(this.m_currentColorSpace);
        }
      }
      else
      {
        if (this.m_layer != null)
        {
          if (this.m_layer.Page is PdfPage && (this.m_layer.Page as PdfPage).Section.ParentDocument.GetType().Name != "PdfLoadedDocument")
          {
            this.ColorSpace = (this.m_layer.Page as PdfPage).Document.ColorSpace;
            this.m_currentColorSpace = (this.m_layer.Page as PdfPage).Document.ColorSpace;
          }
          else if (this.m_layer.Page is PdfLoadedPage)
          {
            this.ColorSpace = ((this.m_layer.Page as PdfLoadedPage).Document as PdfLoadedDocument).ColorSpace;
            this.m_currentColorSpace = ((this.m_layer.Page as PdfLoadedPage).Document as PdfLoadedDocument).ColorSpace;
          }
        }
        this.InitCurrentColorSpace(this.m_currentColorSpace);
      }
    }
    if (saveState)
    {
      if (this.m_bStateSaved)
        this.m_streamWriter.RestoreGraphicsState();
      this.m_streamWriter.SaveGraphicsState();
      this.m_bStateSaved = true;
    }
    this.PenControl(pen, saveState);
    this.BrushControl(brush, saveState);
    this.FontControl(font, format, saveState);
  }

  private void FontControl(PdfFont font, PdfStringFormat format, bool saveState)
  {
    if (font == null)
      return;
    PdfSubSuperScript pdfSubSuperScript1 = format != null ? format.SubSuperScript : PdfSubSuperScript.None;
    PdfSubSuperScript pdfSubSuperScript2 = this.m_currentStringFormat != null ? this.m_currentStringFormat.SubSuperScript : PdfSubSuperScript.None;
    bool flag = false;
    PdfResources pdfResources1 = this.m_getResources();
    float size = font.Metrics.GetSize(format);
    if (saveState || font != this.m_currentFont || pdfSubSuperScript1 != pdfSubSuperScript2)
    {
      this.m_currentFont = font;
      this.m_currentStringFormat = format;
      if (this.m_isEmfTextScaled && font.Metrics.LineGap == 0)
      {
        float num = (double) this.m_emfScalingFactor.Width > (double) this.m_emfScalingFactor.Height ? this.m_emfScalingFactor.Width : this.m_emfScalingFactor.Height;
        if ((double) size > (double) num)
          size /= num;
        else
          this.m_isEmfTextScaled = false;
      }
      else
        this.m_isEmfTextScaled = false;
      if (this.m_layer != null && this.Page is PdfLoadedPage)
      {
        if (!pdfResources1.Items.ContainsKey(new PdfName("Font")))
        {
          if (!(this.Page.Dictionary["Parent"] is PdfDictionary pdfDictionary1))
            pdfDictionary1 = (this.Page.Dictionary["Parent"] as PdfReferenceHolder).Object as PdfDictionary;
          while (pdfDictionary1 != null)
          {
            PdfDictionary pdfDictionary2 = new PdfDictionary();
            if (pdfDictionary1.Items.ContainsKey(new PdfName("Resources")))
            {
              if (!(pdfDictionary1.Items[new PdfName("Resources")] is PdfDictionary pdfDictionary3))
                pdfDictionary3 = (pdfDictionary1.Items[new PdfName("Resources")] as PdfReferenceHolder).Object as PdfDictionary;
              if (pdfDictionary3.Items.ContainsKey(new PdfName("Font")))
              {
                PdfDictionary pdfDictionary4 = pdfDictionary3.Items[new PdfName("Font")] as PdfDictionary;
                PdfName pdfName = new PdfName(Guid.NewGuid().ToString());
                PdfDictionary element = ((IPdfWrapper) font).Element as PdfDictionary;
                if (pdfDictionary4 == null)
                {
                  PdfDictionary pdfDictionary5 = new PdfDictionary();
                  pdfDictionary5.Items.Add(pdfName, (IPdfPrimitive) element);
                  this.m_streamWriter.SetFont(font, pdfName, size);
                  flag = true;
                  PdfResources pdfResources2 = this.Page.Dictionary.Items[new PdfName("Resources")] as PdfResources;
                  if (pdfDictionary3 != null)
                  {
                    pdfResources2.Items.Add(new PdfName("Font"), (IPdfPrimitive) pdfDictionary5);
                    break;
                  }
                  break;
                }
                pdfDictionary4.Items.Add(pdfName, (IPdfPrimitive) element);
                this.m_streamWriter.SetFont(font, pdfName, size);
                this.Page.Dictionary.Remove(new PdfName("Resources"));
                flag = true;
                break;
              }
              if (pdfDictionary1.Items.ContainsKey(new PdfName("Parent")))
              {
                PdfDictionary pdfDictionary6 = pdfDictionary1.Items[new PdfName("Parent")] as PdfDictionary;
                if (pdfDictionary1 == null)
                  pdfDictionary6 = (pdfDictionary1.Items[new PdfName("Parent")] as PdfReferenceHolder).Object as PdfDictionary;
                pdfDictionary1 = pdfDictionary6;
              }
              else
              {
                PdfDictionary primitive = new PdfDictionary();
                PdfName name = pdfResources1.GetName((IPdfWrapper) font);
                PdfDictionary element = ((IPdfWrapper) font).Element as PdfDictionary;
                primitive.Items.Add(name, (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) element));
                this.m_streamWriter.SetFont(font, name, size);
                flag = true;
                PdfDictionary pdfDictionary7 = PdfCrossTable.Dereference(this.Page.Dictionary["Resources"]) as PdfDictionary;
                pdfResources1?.SetProperty(new PdfName("Font"), (IPdfPrimitive) primitive);
                if (pdfDictionary7 != null)
                {
                  pdfDictionary7.SetProperty(new PdfName("Font"), (IPdfPrimitive) primitive);
                  break;
                }
                break;
              }
            }
            else
            {
              if (pdfDictionary1.Items.ContainsKey(new PdfName("Parent")))
                pdfDictionary2 = pdfDictionary1.Items[new PdfName("Parent")] as PdfDictionary;
              if (pdfDictionary2 == null)
                pdfDictionary2 = (pdfDictionary1.Items[new PdfName("Parent")] as PdfReferenceHolder).Object as PdfDictionary;
              pdfDictionary1 = pdfDictionary2;
              if (pdfDictionary1 != null && pdfDictionary1.Items.Count == 0)
              {
                PdfName name = pdfResources1.GetName((IPdfWrapper) font);
                this.m_streamWriter.SetFont(font, name, size);
                flag = true;
                break;
              }
            }
          }
        }
        else
        {
          PdfName name = pdfResources1.GetName((IPdfWrapper) font);
          this.m_streamWriter.SetFont(font, name, size);
          flag = true;
        }
      }
      else
      {
        PdfName name = pdfResources1.GetName((IPdfWrapper) font);
        this.m_streamWriter.SetFont(font, name, size);
        flag = true;
      }
    }
    if (flag)
      return;
    PdfName name1 = pdfResources1.GetName((IPdfWrapper) font);
    this.m_streamWriter.SetFont(font, name1, size);
  }

  private void ColorSpaceControl(PdfColorSpaces colorspace)
  {
    if (colorspace == null)
      return;
    PdfName name = this.m_getResources().GetName((IPdfWrapper) colorspace);
    this.m_streamWriter.SetColorSpace(colorspace, name);
  }

  private void BrushControl(PdfBrush brush, bool saveState)
  {
    if (brush == null)
      return;
    bool flag1 = false;
    bool flag2 = false;
    PdfBrush pdfBrush = brush.Clone();
    if (pdfBrush is PdfGradientBrush pdfGradientBrush)
    {
      PdfTransformationMatrix matrix1 = pdfGradientBrush.Matrix;
      PdfTransformationMatrix matrix2 = this.Matrix.Clone();
      if (matrix1 != null)
      {
        matrix1.Multiply(matrix2);
        matrix2 = matrix1;
      }
      pdfGradientBrush.Matrix = matrix2;
    }
    if (brush is PdfSolidBrush pdfSolidBrush)
    {
      if (pdfSolidBrush.Colorspaces != null)
      {
        if (pdfSolidBrush.Colorspaces is PdfCalRGBColor)
          this.ColorSpace = PdfColorSpace.RGB;
        else if (pdfSolidBrush.Colorspaces is PdfCalGrayColor)
          this.ColorSpace = PdfColorSpace.GrayScale;
        else if (pdfSolidBrush.Colorspaces is PdfICCColor)
        {
          flag1 = true;
          PdfICCColor colorspaces = pdfSolidBrush.Colorspaces as PdfICCColor;
          if (colorspaces.ColorSpaces.AlternateColorSpace != null)
          {
            if (colorspaces.ColorSpaces.AlternateColorSpace is PdfCalGrayColorSpace)
              this.ColorSpace = PdfColorSpace.GrayScale;
            else if (colorspaces.ColorSpaces.AlternateColorSpace is PdfCalRGBColorSpace)
              this.ColorSpace = PdfColorSpace.RGB;
            else if (colorspaces.ColorSpaces.AlternateColorSpace is PdfLabColorSpace)
              this.ColorSpace = PdfColorSpace.RGB;
            else if (colorspaces.ColorSpaces.AlternateColorSpace is PdfDeviceColorSpace)
            {
              switch ((colorspaces.ColorSpaces.AlternateColorSpace as PdfDeviceColorSpace).DeviceColorSpaceType.ToString())
              {
                case "RGB":
                  this.ColorSpace = PdfColorSpace.RGB;
                  break;
                case "GrayScale":
                  this.ColorSpace = PdfColorSpace.GrayScale;
                  break;
                case "CMYK":
                  this.ColorSpace = PdfColorSpace.CMYK;
                  break;
              }
            }
          }
          else
            this.ColorSpace = PdfColorSpace.RGB;
        }
        else if (pdfSolidBrush.Colorspaces is PdfSeparationColor)
        {
          flag1 = true;
          this.ColorSpace = PdfColorSpace.GrayScale;
        }
        else if (pdfSolidBrush.Colorspaces is PdfIndexedColor)
        {
          flag2 = true;
          this.ColorSpace = PdfColorSpace.GrayScale;
        }
        else if (pdfSolidBrush.Colorspaces is PdfLabColor)
          this.ColorSpace = PdfColorSpace.RGB;
        if (!flag1 ? (!flag2 ? pdfBrush.MonitorChanges(this.m_currentBrush, this.m_streamWriter, this.m_getResources, saveState, this.ColorSpace, true) : pdfBrush.MonitorChanges(this.m_currentBrush, this.m_streamWriter, this.m_getResources, saveState, this.ColorSpace, true, true, true)) : pdfBrush.MonitorChanges(this.m_currentBrush, this.m_streamWriter, this.m_getResources, saveState, this.ColorSpace, true, true))
          this.m_currentBrush = pdfBrush;
      }
      else if (pdfBrush.MonitorChanges(this.m_currentBrush, this.m_streamWriter, this.m_getResources, saveState, this.ColorSpace))
        this.m_currentBrush = pdfBrush;
    }
    else if (pdfBrush.MonitorChanges(this.m_currentBrush, this.m_streamWriter, this.m_getResources, saveState, this.ColorSpace))
      this.m_currentBrush = pdfBrush;
    brush = (PdfBrush) null;
  }

  private void InitCurrentColorSpace()
  {
    if (this.m_bCSInitialized)
      return;
    this.m_streamWriter.SetColorSpace("DeviceRGB", true);
    this.m_streamWriter.SetColorSpace("DeviceRGB", false);
    this.m_bCSInitialized = true;
  }

  private void InitCurrentColorSpace(PdfColorSpace colorspace)
  {
    PdfResources pdfResources = this.m_getResources();
    if (this.m_bCSInitialized)
      return;
    if (this.m_currentColorSpace != PdfColorSpace.GrayScale)
    {
      this.m_streamWriter.SetColorSpace("Device" + this.m_currentColorSpace.ToString(), true);
      this.m_streamWriter.SetColorSpace("Device" + this.m_currentColorSpace.ToString(), false);
      this.m_bCSInitialized = true;
    }
    else
    {
      this.m_streamWriter.SetColorSpace("DeviceGray", true);
      this.m_streamWriter.SetColorSpace("DeviceGray", false);
      this.m_bCSInitialized = true;
    }
  }

  private void PenControl(PdfPen pen, bool saveState)
  {
    if (pen == null)
      return;
    bool flag1 = false;
    bool flag2 = false;
    PdfPen pdfPen = pen;
    if (pdfPen != null && pdfPen.Colorspaces != null)
    {
      if (pdfPen.Colorspaces is PdfCalRGBColor)
        this.ColorSpace = PdfColorSpace.RGB;
      else if (pdfPen.Colorspaces is PdfCalGrayColor)
        this.ColorSpace = PdfColorSpace.GrayScale;
      else if (pdfPen.Colorspaces is PdfICCColor)
      {
        flag1 = true;
        PdfICCColor colorspaces = pdfPen.Colorspaces as PdfICCColor;
        if (colorspaces.ColorSpaces.AlternateColorSpace != null)
        {
          if (colorspaces.ColorSpaces.AlternateColorSpace is PdfCalGrayColorSpace)
            this.ColorSpace = PdfColorSpace.GrayScale;
          else if (colorspaces.ColorSpaces.AlternateColorSpace is PdfCalRGBColorSpace)
            this.ColorSpace = PdfColorSpace.RGB;
          else if (colorspaces.ColorSpaces.AlternateColorSpace is PdfLabColorSpace)
            this.ColorSpace = PdfColorSpace.RGB;
          else if (colorspaces.ColorSpaces.AlternateColorSpace is PdfDeviceColorSpace)
          {
            switch ((colorspaces.ColorSpaces.AlternateColorSpace as PdfDeviceColorSpace).DeviceColorSpaceType.ToString())
            {
              case "RGB":
                this.ColorSpace = PdfColorSpace.RGB;
                break;
              case "GrayScale":
                this.ColorSpace = PdfColorSpace.GrayScale;
                break;
              case "CMYK":
                this.ColorSpace = PdfColorSpace.CMYK;
                break;
            }
          }
        }
        else
          this.ColorSpace = PdfColorSpace.RGB;
      }
      else if (pdfPen.Colorspaces is PdfSeparationColor)
      {
        flag1 = true;
        this.ColorSpace = PdfColorSpace.GrayScale;
      }
      else if (pdfPen.Colorspaces is PdfIndexedColor)
      {
        flag2 = true;
        this.ColorSpace = PdfColorSpace.GrayScale;
      }
    }
    if (!(flag1 || flag2 ? (!flag2 ? pen.MonitorChanges(this.m_currentPen, this.m_streamWriter, this.m_getResources, saveState, this.ColorSpace, this.Matrix.Clone(), true) : pen.MonitorChanges(this.m_currentPen, this.m_streamWriter, this.m_getResources, saveState, this.ColorSpace, this.Matrix.Clone(), true)) : pen.MonitorChanges(this.m_currentPen, this.m_streamWriter, this.m_getResources, saveState, this.ColorSpace, this.Matrix.Clone())))
      return;
    this.m_currentPen = pen.Clone();
  }

  private void CapControl(PdfPen pen, float x2, float y2, float x1, float y1)
  {
  }

  private void DrawPath(PdfPen pen, PdfBrush brush, bool needClosing)
  {
    this.DrawPath(pen, brush, PdfFillMode.Winding, needClosing);
  }

  private void DrawPath(PdfPen pen, PdfBrush brush, PdfFillMode fillMode, bool needClosing)
  {
    bool flag1 = pen != null;
    bool flag2 = brush != null;
    bool useEvenOddRule = fillMode == PdfFillMode.Alternate;
    if (flag1 && flag2)
    {
      if (needClosing)
        this.StreamWriter.CloseFillStrokePath(useEvenOddRule);
      else
        this.StreamWriter.FillStrokePath(useEvenOddRule);
    }
    else if (!flag1 && !flag2)
      this.StreamWriter.EndPath();
    else if (flag1)
    {
      if (needClosing)
        this.StreamWriter.CloseStrokePath();
      else
        this.StreamWriter.StrokePath();
    }
    else
    {
      if (!flag2)
        throw new PdfException("Internal CLR error.");
      if (needClosing)
        this.StreamWriter.CloseFillPath(useEvenOddRule);
      else
        this.StreamWriter.FillPath(useEvenOddRule);
    }
  }

  internal static List<float[]> GetBezierArcPoints(
    float x1,
    float y1,
    float x2,
    float y2,
    float startAng,
    float extent)
  {
    if ((double) x1 > (double) x2)
    {
      float num = x1;
      x1 = x2;
      x2 = num;
    }
    if ((double) y2 > (double) y1)
    {
      float num = y1;
      y1 = y2;
      y2 = num;
    }
    float num1;
    int num2;
    if ((double) Math.Abs(extent) <= 90.0)
    {
      num1 = extent;
      num2 = 1;
    }
    else
    {
      num2 = (int) Math.Ceiling((double) Math.Abs(extent) / 90.0);
      num1 = extent / (float) num2;
    }
    float num3 = (float) (((double) x1 + (double) x2) / 2.0);
    float num4 = (float) (((double) y1 + (double) y2) / 2.0);
    float num5 = (float) (((double) x2 - (double) x1) / 2.0);
    float num6 = (float) (((double) y2 - (double) y1) / 2.0);
    float num7 = (float) ((double) num1 * Math.PI / 360.0);
    float num8 = (float) Math.Abs(4.0 / 3.0 * (1.0 - Math.Cos((double) num7)) / Math.Sin((double) num7));
    List<float[]> bezierArcPoints = new List<float[]>();
    for (int index = 0; index < num2; ++index)
    {
      float num9 = (float) (((double) startAng + (double) index * (double) num1) * Math.PI / 180.0);
      float num10 = (float) (((double) startAng + (double) (index + 1) * (double) num1) * Math.PI / 180.0);
      float num11 = (float) Math.Cos((double) num9);
      float num12 = (float) Math.Cos((double) num10);
      float num13 = (float) Math.Sin((double) num9);
      float num14 = (float) Math.Sin((double) num10);
      if ((double) num1 > 0.0)
        bezierArcPoints.Add(new float[8]
        {
          num3 + num5 * num11,
          num4 - num6 * num13,
          num3 + num5 * (num11 - num8 * num13),
          num4 - num6 * (num13 + num8 * num11),
          num3 + num5 * (num12 + num8 * num14),
          num4 - num6 * (num14 - num8 * num12),
          num3 + num5 * num12,
          num4 - num6 * num14
        });
      else
        bezierArcPoints.Add(new float[8]
        {
          num3 + num5 * num11,
          num4 - num6 * num13,
          num3 + num5 * (num11 + num8 * num13),
          num4 - num6 * (num13 - num8 * num11),
          num3 + num5 * (num12 - num8 * num14),
          num4 - num6 * (num14 + num8 * num12),
          num3 + num5 * num12,
          num4 - num6 * num14
        });
    }
    return bezierArcPoints;
  }

  private void ConstructArcPath(
    float x1,
    float y1,
    float x2,
    float y2,
    float startAng,
    float sweepAngle)
  {
    List<float[]> bezierArcPoints = PdfGraphics.GetBezierArcPoints(x1, y1, x2, y2, startAng, sweepAngle);
    if (bezierArcPoints.Count == 0)
      return;
    float[] numArray1 = bezierArcPoints[0];
    this.m_streamWriter.BeginPath(numArray1[0], numArray1[1]);
    for (int index = 0; index < bezierArcPoints.Count; ++index)
    {
      float[] numArray2 = bezierArcPoints[index];
      this.m_streamWriter.AppendBezierSegment(numArray2[2], numArray2[3], numArray2[4], numArray2[5], numArray2[6], numArray2[7]);
    }
  }

  private void BuildUpPath(PdfPath path) => this.BuildUpPath(path.PathPoints, path.PathTypes);

  private void GetBezierPoints(
    PointF[] points,
    byte[] types,
    ref int i,
    out PointF p2,
    out PointF p3)
  {
    ++i;
    p2 = ((int) types[i] & 15) == 3 ? points[i] : throw new ArgumentException("Malforming path.");
    ++i;
    p3 = ((int) types[i] & 15) == 3 ? points[i] : throw new ArgumentException("Malforming path.");
  }

  private void BuildUpPath(PointF[] points, byte[] types)
  {
    int i = 0;
    for (int length = points.Length; i < length; ++i)
    {
      byte type = types[i];
      PointF point = points[i];
      switch ((int) type & 15)
      {
        case 0:
          this.m_streamWriter.BeginPath(point);
          break;
        case 1:
          this.m_streamWriter.AppendLineSegment(point);
          break;
        case 3:
          PointF p2;
          PointF p3;
          this.GetBezierPoints(points, types, ref i, out p2, out p3);
          this.m_streamWriter.AppendBezierSegment(point, p2, p3);
          break;
        default:
          throw new ArithmeticException("Incorrect path formation.");
      }
      this.CheckFlags(types[i]);
    }
  }

  private void CheckFlags(byte type)
  {
    if (((int) type & 128 /*0x80*/) != 128 /*0x80*/)
      return;
    this.m_streamWriter.ClosePath();
  }

  private TextRenderingMode GetTextRenderingMode(
    PdfPen pen,
    PdfBrush brush,
    PdfStringFormat format)
  {
    TextRenderingMode textRenderingMode = TextRenderingMode.None;
    if (this.m_isTextRenderingSet)
    {
      textRenderingMode = this.m_textRenderingMode;
      this.m_isTextRenderingSet = false;
    }
    else
    {
      if (pen != null && brush != null)
        textRenderingMode = TextRenderingMode.FillStroke;
      else if (pen != null)
        textRenderingMode = TextRenderingMode.Stroke;
      else if (brush != null)
        textRenderingMode = TextRenderingMode.Fill;
      if (format != null && format.ClipPath)
        textRenderingMode |= TextRenderingMode.ClipFlag;
    }
    return textRenderingMode;
  }

  internal void ClipTranslateMargins(
    float x,
    float y,
    float left,
    float top,
    float right,
    float bottom)
  {
    RectangleF rect = new RectangleF(left, top, this.Size.Width - left - right, this.Size.Height - top - bottom);
    this.m_clipBounds = rect;
    this.m_streamWriter.WriteComment("Clip margins.");
    this.m_streamWriter.AppendRectangle(rect);
    this.m_streamWriter.ClosePath();
    this.m_streamWriter.ClipPath(false);
    this.m_streamWriter.WriteComment("Translate co-ordinate system.");
    this.TranslateTransform(x, y);
  }

  internal void ClipTranslateMargins(RectangleF clipBounds)
  {
    this.m_clipBounds = clipBounds;
    this.m_streamWriter.WriteComment("Clip margins.");
    this.m_streamWriter.AppendRectangle(clipBounds);
    this.m_streamWriter.ClosePath();
    this.m_streamWriter.ClipPath(false);
    this.m_streamWriter.WriteComment("Translate co-ordinate system.");
    this.TranslateTransform(clipBounds.X, clipBounds.Y);
  }

  internal void InitializeCoordinates()
  {
    this.m_streamWriter.WriteComment("Change co-ordinate system to left/top.");
    if ((double) this.MediaBoxUpperRightBound == -(double) this.Size.Height)
      return;
    if (this.m_cropBox == null)
    {
      if ((double) this.MediaBoxUpperRightBound == (double) this.Size.Height || (double) this.MediaBoxUpperRightBound == 0.0)
        this.TranslateTransform(0.0f, PdfGraphics.UpdateY(this.Size.Height));
      else
        this.TranslateTransform(0.0f, PdfGraphics.UpdateY(this.MediaBoxUpperRightBound));
    }
    else
    {
      if (this.m_cropBox == null)
        return;
      if ((double) (this.m_cropBox[0] as PdfNumber).FloatValue > 0.0 || (double) (this.m_cropBox[1] as PdfNumber).FloatValue > 0.0 || (double) this.Size.Width == (double) (this.m_cropBox[2] as PdfNumber).FloatValue || (double) this.Size.Height == (double) (this.m_cropBox[3] as PdfNumber).FloatValue)
        this.TranslateTransform((this.m_cropBox[0] as PdfNumber).FloatValue, PdfGraphics.UpdateY((this.m_cropBox[3] as PdfNumber).FloatValue));
      else if ((double) this.MediaBoxUpperRightBound == (double) this.Size.Height || (double) this.MediaBoxUpperRightBound == 0.0)
        this.TranslateTransform(0.0f, PdfGraphics.UpdateY(this.Size.Height));
      else
        this.TranslateTransform(0.0f, PdfGraphics.UpdateY(this.MediaBoxUpperRightBound));
    }
  }

  internal void InitializeCoordinates(PdfPageBase page)
  {
    PointF empty = PointF.Empty;
    PdfDictionary dictionary = page.Dictionary;
    bool flag = false;
    PdfArray pdfArray1 = (PdfArray) null;
    if (page.Dictionary.ContainsKey("CropBox") && page.Dictionary.ContainsKey("MediaBox"))
    {
      pdfArray1 = page.Dictionary["CropBox"] as PdfArray;
      PdfArray pdfArray2 = page.Dictionary["MediaBox"] as PdfArray;
      if (pdfArray1.ToRectangle() == pdfArray2.ToRectangle())
        flag = true;
      if ((double) (pdfArray1[0] as PdfNumber).FloatValue > 0.0 && (double) (pdfArray1[3] as PdfNumber).FloatValue > 0.0 && (double) (pdfArray2[0] as PdfNumber).FloatValue < 0.0 && (double) (pdfArray2[1] as PdfNumber).FloatValue < 0.0)
      {
        this.TranslateTransform((pdfArray1[0] as PdfNumber).FloatValue, -(pdfArray1[3] as PdfNumber).FloatValue);
        empty.X = -(pdfArray1[0] as PdfNumber).FloatValue;
        empty.Y = (pdfArray1[3] as PdfNumber).FloatValue;
      }
    }
    else if (!page.Dictionary.ContainsKey("CropBox"))
      flag = true;
    if (flag)
    {
      this.m_streamWriter.WriteComment("Change co-ordinate system to left/top.");
      if (this.m_cropBox == null)
      {
        if (-(double) page.Origin.Y < (double) this.MediaBoxUpperRightBound || (double) this.MediaBoxUpperRightBound == 0.0)
          this.TranslateTransform(0.0f, PdfGraphics.UpdateY(this.Size.Height));
        else
          this.TranslateTransform(0.0f, PdfGraphics.UpdateY(this.MediaBoxUpperRightBound));
      }
      else
        this.TranslateTransform((pdfArray1[0] as PdfNumber).FloatValue, PdfGraphics.UpdateY((this.m_cropBox[3] as PdfNumber).FloatValue));
    }
    else
    {
      PdfTransformationMatrix input = new PdfTransformationMatrix();
      this.GetTranslateTransform(empty.X, empty.Y + 0.0f, input);
    }
  }

  private void FlipHorizontal()
  {
    PdfTransformationMatrix matrix = new PdfTransformationMatrix();
    matrix.Translate(0.0f, this.Size.Height);
    matrix.Scale(1f, -1f);
    this.m_streamWriter.ModifyCTM(matrix);
  }

  private void FlipVertical()
  {
    PdfTransformationMatrix matrix = new PdfTransformationMatrix();
    matrix.Translate(this.Size.Width, 0.0f);
    matrix.Scale(-1f, 1f);
    this.m_streamWriter.ModifyCTM(matrix);
  }

  private PdfTransformationMatrix GetTranslateTransform(
    float x,
    float y,
    PdfTransformationMatrix input)
  {
    if (input == null)
      input = new PdfTransformationMatrix();
    input.Translate(x, PdfGraphics.UpdateY(y));
    return input;
  }

  private PdfTransformationMatrix GetScaleTransform(
    float x,
    float y,
    PdfTransformationMatrix input)
  {
    if (input == null)
      input = new PdfTransformationMatrix();
    input.Scale(x, y);
    return input;
  }

  private PdfTransformationMatrix GetRotateTransform(float angle, PdfTransformationMatrix input)
  {
    if (input == null)
      input = new PdfTransformationMatrix();
    input.Rotate(PdfGraphics.UpdateY(angle));
    return input;
  }

  private PdfTransformationMatrix GetSkewTransform(
    float angleX,
    float angleY,
    PdfTransformationMatrix input)
  {
    if (input == null)
      input = new PdfTransformationMatrix();
    input.Skew(PdfGraphics.UpdateY(angleX), PdfGraphics.UpdateY(angleY));
    return input;
  }

  private void DrawCjkString(
    LineInfo lineInfo,
    RectangleF layoutRectangle,
    PdfFont font,
    PdfStringFormat format)
  {
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    double num = (double) this.JustifyLine(lineInfo, layoutRectangle.Width, format);
    this.m_streamWriter.ShowNextLineText(this.GetCjkString(lineInfo.Text), false);
  }

  private byte[] GetCjkString(string line)
  {
    return line != null ? PdfString.EscapeSymbols(PdfString.ToUnicodeArray(line, false)) : throw new ArgumentNullException(nameof (line));
  }

  private void DrawAsciiLine(
    LineInfo lineInfo,
    RectangleF layoutRectangle,
    PdfFont font,
    PdfStringFormat format)
  {
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    double num = (double) this.JustifyLine(lineInfo, layoutRectangle.Width, format);
    this.m_streamWriter.ShowNextLineText(this.GetAsciiString(lineInfo.Text));
  }

  private PdfString GetAsciiString(string token)
  {
    PdfString asciiString = token != null ? new PdfString(token) : throw new ArgumentNullException(nameof (token));
    if (this.m_currentFont is PdfStandardFont)
    {
      PdfStandardFont currentFont = this.m_currentFont as PdfStandardFont;
      if (currentFont.Name != currentFont.FontFamily.ToString() && this.CheckFontEncoding(currentFont.FontInternal as PdfDictionary) == "MacRomanEncoding")
        asciiString.EncodedBytes = this.GetMacRomanEncodedByte(token);
    }
    asciiString.Encode = PdfString.ForceEncoding.ASCII;
    return asciiString;
  }

  private byte[] GetMacRomanEncodedByte(string token)
  {
    Encoding encoding;
    try
    {
      encoding = Encoding.GetEncoding("macintosh");
    }
    catch (Exception ex)
    {
      encoding = Encoding.UTF8;
    }
    return encoding.GetBytes(token);
  }

  private string CheckFontEncoding(PdfDictionary fontDictionary)
  {
    PdfName pdfName = new PdfName();
    string empty = string.Empty;
    if (fontDictionary.ContainsKey("Encoding"))
    {
      PdfName font = fontDictionary["Encoding"] as PdfName;
      if (font == (PdfName) null)
      {
        Type type = fontDictionary["Encoding"].GetType();
        pdfDictionary = new PdfDictionary();
        if (type.Name == "PdfDictionary")
        {
          if (!(fontDictionary["Encoding"] is PdfDictionary pdfDictionary))
            empty = ((fontDictionary["Encoding"] as PdfReferenceHolder).Object as PdfName).Value;
        }
        else if (type.Name == "PdfReferenceHolder")
          pdfDictionary = (fontDictionary["Encoding"] as PdfReferenceHolder).Object as PdfDictionary;
        if (pdfDictionary != null && pdfDictionary.ContainsKey("Type"))
          empty = (pdfDictionary["Type"] as PdfName).Value;
        if (pdfDictionary != null && pdfDictionary.ContainsKey("BaseEncoding"))
          empty = (pdfDictionary["BaseEncoding"] as PdfName).Value;
      }
      else
        empty = font.Value;
    }
    return empty;
  }

  private void DrawUnicodeLine(
    LineInfo lineInfo,
    RectangleF layoutRectangle,
    PdfFont font,
    PdfStringFormat format)
  {
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    string text = lineInfo.Text;
    double width = (double) lineInfo.Width;
    bool isRTL = format != null && format.RightToLeft;
    bool wordSpace = format != null && ((double) format.WordSpacing != 0.0 || format.Alignment == PdfTextAlignment.Justify);
    PdfTrueTypeFont pdfTrueTypeFont = font as PdfTrueTypeFont;
    float wordSpacing = this.JustifyLine(lineInfo, layoutRectangle.Width, format);
    if (isRTL || format != null && format.TextDirection != PdfTextDirection.None)
    {
      lock (PdfGraphics.s_rtlRenderLock)
      {
        bool rtl = format != null && format.Alignment == PdfTextAlignment.Right;
        if (lineInfo.OpenTypeGlyphList == null)
        {
          string[] blocks = format == null || format.TextDirection == PdfTextDirection.None ? RtlRenderer.Layout(text, pdfTrueTypeFont, rtl, wordSpace, format) : RtlRenderer.Layout(text, pdfTrueTypeFont, format.TextDirection == PdfTextDirection.RightToLeft, wordSpace, format);
          string[] words;
          if (blocks.Length > 1)
            words = format == null || format.TextDirection == PdfTextDirection.None ? RtlRenderer.SplitLayout(text, pdfTrueTypeFont, rtl, wordSpace, format) : RtlRenderer.SplitLayout(text, pdfTrueTypeFont, format.TextDirection == PdfTextDirection.RightToLeft, wordSpace, format);
          else
            words = new string[1]{ text };
          this.DrawUnicodeBlocks(blocks, words, font, format, wordSpacing);
        }
        else
        {
          Bidi bidi = new Bidi();
          if (format.TextDirection == PdfTextDirection.RightToLeft)
            isRTL = true;
          OtfGlyphInfo[] logicalToVisualGlyphs = bidi.GetLogicalToVisualGlyphs(lineInfo.OpenTypeGlyphList.Glyphs, isRTL, pdfTrueTypeFont.TtfReader);
          OtfGlyphInfoList otfGlyphInfoList = new OtfGlyphInfoList(logicalToVisualGlyphs, 0, logicalToVisualGlyphs.Length);
          OtfGlyphTokenizer tokenizer = new OtfGlyphTokenizer();
          if (wordSpace)
            this.DrawOpenTypeStringBlocks(tokenizer.SplitGlyphs(otfGlyphInfoList.Glyphs), pdfTrueTypeFont, format, wordSpacing, tokenizer);
          else
            this.DrawOpenTypeStringBlocks(new List<OtfGlyphInfo[]>()
            {
              otfGlyphInfoList.Glyphs.ToArray()
            }.ToArray(), pdfTrueTypeFont, format, wordSpacing, tokenizer);
        }
      }
    }
    else if (wordSpace)
    {
      if (lineInfo.OpenTypeGlyphList != null && pdfTrueTypeFont.InternalFont is UnicodeTrueTypeFont)
      {
        OtfGlyphTokenizer tokenizer = new OtfGlyphTokenizer();
        this.DrawOpenTypeStringBlocks(tokenizer.SplitGlyphs(lineInfo.OpenTypeGlyphList.Glyphs), pdfTrueTypeFont, format, wordSpacing, tokenizer);
      }
      else
      {
        string[] words = (string[]) null;
        this.DrawUnicodeBlocks(this.BreakUnicodeLine(text, pdfTrueTypeFont, out words, format), words, font, format, wordSpacing);
      }
    }
    else if (lineInfo.OpenTypeGlyphList != null && pdfTrueTypeFont.InternalFont is UnicodeTrueTypeFont)
    {
      if (lineInfo.OpenTypeGlyphList.HasYPlacement())
        this.DrawOpenTypeStringBlocks(lineInfo.OpenTypeGlyphList, pdfTrueTypeFont, format, 0.0f);
      else
        this.DrawOpenTypeString(lineInfo.OpenTypeGlyphList, pdfTrueTypeFont);
    }
    else
      this.m_streamWriter.ShowNextLineText(this.GetUnicodeString(this.ConvertToUnicode(RtlRenderer.TrimLRM(text), pdfTrueTypeFont, format)));
  }

  private void DrawOpenTypeString(OtfGlyphInfoList glyphInfoList, PdfTrueTypeFont ttfFont)
  {
    ITrueTypeFont internalFont = ttfFont.InternalFont;
    int count = glyphInfoList.Glyphs.Count;
    (ttfFont.InternalFont as UnicodeTrueTypeFont).SetSymbols(glyphInfoList);
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("[");
    stringBuilder.Append("<");
    int num1 = 0;
    for (int index = 0; index < glyphInfoList.Glyphs.Count; ++index)
    {
      bool flag = false;
      OtfGlyphInfo glyph = glyphInfoList.Glyphs[index];
      int charCode = glyph.CharCode;
      if (glyph.CharCode > -1)
        flag = LanguageUtil.IsDiscardGlyph(glyph.CharCode);
      if (!flag)
      {
        if (glyph.leadingX != 0 || glyph.leadingY != 0)
        {
          if ((double) glyph.Width == 0.0 && index - 1 >= 0 && (double) glyphInfoList.Glyphs[index - 1].Width != 0.0)
          {
            stringBuilder.Append(">");
            int num2 = (int) ((double) glyphInfoList.Glyphs[index - 1].Width - (double) glyph.leadingX);
            stringBuilder.Append(num2.ToString());
            stringBuilder.Append("<");
            num1 = num2;
          }
          else if ((double) glyph.Width == 0.0 && glyph.CharCode == -1 && (double) glyphInfoList.Glyphs[index - 1].Width == 0.0 && glyph.leadingX > 0)
          {
            stringBuilder.Append(">");
            stringBuilder.Append((-glyph.leadingX).ToString());
            stringBuilder.Append("<");
            num1 = -glyph.leadingX;
          }
          else if ((double) glyph.Width == 0.0 && glyph.CharCode != -1 && index - 2 >= 0 && (double) glyphInfoList.Glyphs[index - 1].Width == 0.0 && glyph.leadingX > 0 && (double) glyphInfoList.Glyphs[index - 2].Width != 0.0)
          {
            stringBuilder.Append(">");
            int num3 = (int) ((double) glyphInfoList.Glyphs[index - 2].Width - (double) glyph.leadingX);
            stringBuilder.Append(num3.ToString());
            stringBuilder.Append("<");
            num1 = num3;
          }
        }
        else if ((double) glyph.Width != 0.0 && num1 != 0)
        {
          stringBuilder.Append(">");
          stringBuilder.Append((-num1).ToString());
          stringBuilder.Append("<");
          num1 = 0;
        }
        byte[] encode = PdfString.ToEncode(glyphInfoList.Glyphs[index].Index);
        stringBuilder.Append(PdfString.BytesToHex(encode));
      }
    }
    stringBuilder.Append(">");
    stringBuilder.Append("]");
    this.m_streamWriter.ShowFormatedText(stringBuilder.ToString());
  }

  private void DrawOpenTypeString(
    OtfGlyphInfo[] glyphs,
    PdfTrueTypeFont ttfFont,
    PdfStringFormat format,
    float spaceWidth,
    out bool skipNextLine)
  {
    skipNextLine = false;
    OtfGlyphInfoList glyphInfoList = new OtfGlyphInfoList(glyphs, 0, glyphs.Length);
    if (glyphInfoList.HasYPlacement())
    {
      this.DrawOpenTypeStringBlocks(glyphInfoList, ttfFont, format, spaceWidth);
      skipNextLine = true;
    }
    else
      this.DrawOpenTypeString(glyphInfoList, ttfFont);
  }

  private void DrawOpenTypeStringBlocks(
    OtfGlyphInfoList glyphInfoList,
    PdfTrueTypeFont ttfFont,
    PdfStringFormat format,
    float spaceWidth)
  {
    ITrueTypeFont internalFont = ttfFont.InternalFont;
    int count = glyphInfoList.Glyphs.Count;
    (ttfFont.InternalFont as UnicodeTrueTypeFont).SetSymbols(glyphInfoList);
    float size = ttfFont.Metrics.GetSize(format);
    this.m_streamWriter.StartNextLine();
    float x1 = 0.0f;
    float num1 = 0.0f;
    float num2 = 0.0f;
    float num3 = 0.0f;
    float num4 = 0.0f;
    float x2 = 0.0f;
    bool flag1 = false;
    bool flag2 = false;
    for (int index = 0; index < glyphInfoList.Glyphs.Count; ++index)
    {
      bool flag3 = false;
      OtfGlyphInfo glyph = glyphInfoList.Glyphs[index];
      int charCode = glyph.CharCode;
      if (glyph.CharCode > -1)
        flag3 = LanguageUtil.IsDiscardGlyph(glyph.CharCode);
      if (!flag3)
      {
        float num5 = glyph.Width * (1f / 1000f * size);
        if (glyph.leadingX != 0 || glyph.leadingY != 0)
        {
          if (glyph.leadingX != 0 && (double) glyph.Width == 0.0)
          {
            x2 = (float) glyph.leadingX * (1f / 1000f * size);
            flag1 = true;
          }
          if (glyph.leadingY != 0 && (double) glyph.Width == 0.0)
          {
            num4 = (float) glyph.leadingY * (1f / 1000f * size);
            flag1 = true;
          }
        }
        if (index != 0)
        {
          if (flag1 && (double) glyph.Width == 0.0)
          {
            if (index - 1 >= 0 && (double) glyphInfoList.Glyphs[index - 1].Width == 0.0)
            {
              if (glyph.CharCode != -1)
                x2 = 0.0f;
              else
                flag2 = true;
            }
            if ((double) num5 == 0.0 && (double) x2 != 0.0 && (double) x2 < 0.0 && glyph.CharCode != -1)
              this.m_streamWriter.StartNextLine(num1 - num2, (float) -((double) num4 - (double) num3));
            else
              this.m_streamWriter.StartNextLine(x2, -num4);
            if ((double) num2 != 0.0 || (double) num3 != 0.0)
            {
              num2 += x2;
              num3 += num4;
            }
            else
            {
              num2 = x2;
              num3 = num4;
            }
            x2 = 0.0f;
            num4 = 0.0f;
          }
          else if ((double) glyph.Width == 0.0)
          {
            if ((double) num1 != 0.0 && (double) num1 == (double) x1)
              this.m_streamWriter.StartNextLine(x1, 0.0f);
            else
              this.m_streamWriter.StartNextLine(0.0f, 0.0f);
          }
          else if ((double) num2 != 0.0 || (double) num3 != 0.0)
          {
            if (flag2 && (glyph.CharCode != 32 /*0x20*/ || glyph.CharCode == 32 /*0x20*/ && (double) x2 == 0.0))
              this.m_streamWriter.StartNextLine(-x2, (float) -((double) num4 - (double) num3));
            else if (!flag1 && (double) num2 < 0.0 && index - 1 >= 0 && (double) glyphInfoList.Glyphs[index - 1].Width == 0.0 && glyphInfoList.Glyphs[index - 1].CharCode != -1)
              this.m_streamWriter.StartNextLine(0.0f, (float) -((double) num4 - (double) num3));
            else
              this.m_streamWriter.StartNextLine(num1 - num2, (float) -((double) num4 - (double) num3));
            num2 = x2;
            num3 = num4;
            x2 = 0.0f;
            flag2 = false;
          }
          else
            this.m_streamWriter.StartNextLine(x1, 0.0f);
          flag1 = false;
        }
        this.m_streamWriter.ShowText(PdfString.BytesToHex(PdfString.ToEncode(glyph.Index)), true);
        x1 = num5;
        if ((double) num5 != 0.0)
          num1 = num5;
      }
    }
    this.m_streamWriter.StartNextLine(num1 - num2 + spaceWidth, (float) -((double) num4 - (double) num3));
  }

  private void DrawOpenTypeStringBlocks(
    OtfGlyphInfo[][] blocks,
    PdfTrueTypeFont ttfFont,
    PdfStringFormat format,
    float wordSpacing,
    OtfGlyphTokenizer tokenizer)
  {
    this.m_streamWriter.StartNextLine();
    float x = 0.0f;
    float num1 = 0.0f;
    float num2 = 0.0f;
    float num3 = 0.0f;
    try
    {
      if (format != null)
      {
        num2 = format.FirstLineIndent;
        num3 = format.ParagraphIndent;
        format.FirstLineIndent = 0.0f;
        format.ParagraphIndent = 0.0f;
      }
      float num4 = ttfFont.GetCharWidth(' ', format) + wordSpacing;
      float num5 = format != null ? format.CharacterSpacing : 0.0f;
      float num6 = format == null || (double) wordSpacing != 0.0 ? 0.0f : format.WordSpacing;
      float spaceWidth = num4 + (num5 + num6);
      bool skipNextLine = false;
      int index = 0;
      for (int length = blocks.Length; index < length; ++index)
      {
        OtfGlyphInfo[] block = blocks[index];
        float num7 = 0.0f;
        if ((double) x != 0.0 && !skipNextLine)
          this.m_streamWriter.StartNextLine(x, 0.0f);
        skipNextLine = false;
        if (block.Length > 0)
        {
          num7 = num7 + tokenizer.GetLineWidth(block, ttfFont, format) + num5;
          this.DrawOpenTypeString(block, ttfFont, format, spaceWidth, out skipNextLine);
        }
        if (index != length - 1)
        {
          x = num7 + spaceWidth;
          num1 += x;
        }
      }
      if ((double) num1 <= 0.0)
        return;
      this.m_streamWriter.StartNextLine(-num1, 0.0f);
    }
    finally
    {
      if (format != null)
      {
        format.FirstLineIndent = num2;
        format.ParagraphIndent = num3;
      }
    }
  }

  private PdfString GetUnicodeString(string token)
  {
    return token != null ? new PdfString(token)
    {
      Converted = true,
      Encode = PdfString.ForceEncoding.ASCII
    } : throw new ArgumentNullException(nameof (token));
  }

  private string[] BreakUnicodeLine(
    string line,
    PdfTrueTypeFont ttfFont,
    out string[] words,
    PdfStringFormat format)
  {
    if (line == null)
      throw new ArgumentNullException(nameof (line));
    if (ttfFont == null)
      throw new ArgumentNullException(nameof (ttfFont));
    words = line.Split((char[]) null);
    string[] strArray = new string[words.Length];
    int index = 0;
    for (int length = words.Length; index < length; ++index)
    {
      string unicode = this.ConvertToUnicode(words[index], ttfFont, format);
      strArray[index] = unicode;
    }
    return strArray;
  }

  private string ConvertToUnicode(string text, PdfTrueTypeFont ttfFont, PdfStringFormat format)
  {
    string unicode = (string) null;
    if (text == null)
      throw new ArgumentNullException(nameof (text));
    if (ttfFont == null)
      throw new ArgumentNullException(nameof (ttfFont));
    if (ttfFont.InternalFont is UnicodeTrueTypeFont)
    {
      TtfReader ttfReader = (ttfFont.InternalFont as UnicodeTrueTypeFont).TtfReader;
      UnicodeTrueTypeFont internalFont = ttfFont.InternalFont as UnicodeTrueTypeFont;
      if (format != null && format.ComplexScript || this.IsDirectPDF && ttfReader.isOTFFont())
        internalFont.SetSymbols(text, true);
      else
        ttfFont.SetSymbols(text);
      string str = ttfReader.ConvertString(text);
      if (this.XPSToken && this.XPSReplaceCharacter.ContainsKey(text))
        str = this.XPSReplaceCharacter[text];
      unicode = PdfString.ByteToString(PdfString.ToUnicodeArray(str, false));
    }
    else if (ttfFont.InternalFont is TrueTypeFont)
    {
      TtfReader ttfReader = (ttfFont.InternalFont as TrueTypeFont).TtfReader;
      ttfFont.SetSymbols(text);
      unicode = PdfString.ByteToString(PdfString.ToUnicodeArray(ttfReader.ConvertString(text), false));
    }
    return unicode;
  }

  private void DrawUnicodeBlocks(
    string[] blocks,
    string[] words,
    PdfFont font,
    PdfStringFormat format,
    float wordSpacing)
  {
    if (blocks == null)
      throw new ArgumentNullException(nameof (blocks));
    if (words == null)
      throw new ArgumentNullException(nameof (words));
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    this.m_streamWriter.StartNextLine();
    float x = 0.0f;
    float num1 = 0.0f;
    float num2 = 0.0f;
    float num3 = 0.0f;
    try
    {
      if (format != null)
      {
        num2 = format.FirstLineIndent;
        num3 = format.ParagraphIndent;
        format.FirstLineIndent = 0.0f;
        format.ParagraphIndent = 0.0f;
      }
      float num4 = font.GetCharWidth(' ', format) + wordSpacing;
      float num5 = format != null ? format.CharacterSpacing : 0.0f;
      float num6 = format == null || (double) wordSpacing != 0.0 ? 0.0f : format.WordSpacing;
      float num7 = num4 + (num5 + num6);
      int index = 0;
      for (int length = blocks.Length; index < length; ++index)
      {
        string block = blocks[index];
        string word = words[index];
        float num8 = 0.0f;
        if ((double) x != 0.0)
          this.m_streamWriter.StartNextLine(x, 0.0f);
        if (word.Length > 0)
        {
          num8 = num8 + font.MeasureString(word, format).Width + num5;
          this.m_streamWriter.ShowText(this.GetUnicodeString(block));
        }
        if (index != length - 1)
        {
          x = num8 + num7;
          num1 += x;
        }
      }
      if ((double) num1 <= 0.0)
        return;
      this.m_streamWriter.StartNextLine(-num1, 0.0f);
    }
    finally
    {
      if (format != null)
      {
        format.FirstLineIndent = num2;
        format.ParagraphIndent = num3;
      }
    }
  }

  private string[] GetTextLines(string text)
  {
    MatchCollection matchCollection = new Regex("[^\r\n]*").Matches(text);
    int count = matchCollection.Count;
    List<string> stringList = new List<string>();
    bool flag = true;
    for (int i = 0; i < count; ++i)
    {
      string str = matchCollection[i].Value;
      if (str == string.Empty && !flag)
      {
        flag = true;
      }
      else
      {
        if (str != string.Empty)
          flag = false;
        stringList.Add(str);
      }
    }
    return stringList.ToArray();
  }

  private void ApplyStringSettings(
    PdfFont font,
    PdfPen pen,
    PdfBrush brush,
    PdfStringFormat format,
    RectangleF bounds)
  {
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    switch (brush)
    {
      case PdfTilingBrush _:
        this.m_bCSInitialized = false;
        (brush as PdfTilingBrush).Graphics.ColorSpace = this.ColorSpace;
        break;
      case PdfGradientBrush _:
        this.m_bCSInitialized = false;
        (brush as PdfGradientBrush).ColorSpace = this.ColorSpace;
        break;
    }
    bool flag1 = false;
    TextRenderingMode renderingMode = this.GetTextRenderingMode(pen, brush, format);
    if (font is PdfTrueTypeFont && (font as PdfTrueTypeFont).Unicode)
    {
      PdfTrueTypeFont pdfTrueTypeFont = font as PdfTrueTypeFont;
      string postScriptName = pdfTrueTypeFont.m_fontInternal.Metrics.PostScriptName;
      bool flag2 = false;
      if (postScriptName != null && postScriptName.ToLower().Contains("bold"))
        flag2 = true;
      if (pdfTrueTypeFont.m_fontInternal.Metrics.IsBold != font.Bold && font.Bold && !flag2)
      {
        if (pen == null && brush != null)
          pen = new PdfPen(brush);
        renderingMode = TextRenderingMode.FillStroke;
        flag1 = true;
      }
    }
    this.m_streamWriter.BeginText();
    this.StateControl(pen, brush, font, format);
    if (this.Layer != null && this.Page != null && this.Page is PdfPage)
    {
      if ((this.Page as PdfPage).Section.ParentDocument is PdfDocument)
      {
        PdfStructTreeRoot structTreeRoot = PdfCatalog.StructTreeRoot;
        if (structTreeRoot != null)
        {
          if (this.Tag != null && this.Tag is PdfStructureElement)
          {
            if (this.IsDirectPDF)
            {
              if (!(this.Tag as PdfStructureElement).IsAdded)
              {
                int num = structTreeRoot.Add(this.Tag as PdfStructureElement, this.Page, bounds);
                this.StreamWriter.WriteTag(string.Format($"/{{0}} <</E ({(this.Tag as PdfStructureElement).Abbrevation}) /MCID {{1}} >>BDC", (object) structTreeRoot.ConvertToEquivalentTag((this.Tag as PdfStructureElement).TagType), (object) num));
                (this.Tag as PdfStructureElement).IsAdded = true;
              }
            }
            else
            {
              int num = structTreeRoot.Add(this.Tag as PdfStructureElement, this.Page, bounds);
              this.StreamWriter.WriteTag(string.Format($"/{{0}} <</E ({(this.Tag as PdfStructureElement).Abbrevation}) /MCID {{1}} >>BDC", (object) structTreeRoot.ConvertToEquivalentTag((this.Tag as PdfStructureElement).TagType), (object) num));
            }
          }
          else if (this.Tag != null && this.Tag is PdfArtifact)
            this.StreamWriter.Write(string.Format("/Artifact << {0} >>BDC" + Environment.NewLine, (object) this.SetArtifact(this.Tag as PdfArtifact)));
          else if (this.IsTaggedPdf)
            this.StreamWriter.WriteTag($"/{"P"} <</MCID {structTreeRoot.Add(new PdfStructureElement(PdfTagType.Paragraph), this.Page, bounds)} >>BDC");
        }
      }
    }
    else
    {
      PdfStructTreeRoot structTreeRoot = PdfCatalog.StructTreeRoot;
      if (structTreeRoot != null)
      {
        if (this.Tag != null && this.Tag is PdfArtifact)
          this.StreamWriter.Write(string.Format("/Artifact << {0} >>BDC" + Environment.NewLine, (object) this.SetArtifact(this.Tag as PdfArtifact)));
        else if (this.Layer != null && this.Page != null && this.Page is PdfLoadedPage && (this.Page as PdfLoadedPage).Document.FileStructure.TaggedPdf)
          this.StreamWriter.WriteTag($"/{"P"} <</MCID {structTreeRoot.Add("P", "", this.Page, bounds)} >>BDC");
        else if (!this.IsTemplateGraphics)
          this.StreamWriter.WriteTag($"/{"P"} <</MCID {structTreeRoot.Add("P", "", bounds)} >>BDC");
      }
    }
    if (flag1)
      this.m_streamWriter.SetLineWidth(font.Size / 30f);
    if (renderingMode != this.m_previousTextRenderingMode)
    {
      this.m_streamWriter.SetTextRenderingMode(renderingMode);
      this.m_previousTextRenderingMode = renderingMode;
    }
    float charSpacing = format != null ? format.CharacterSpacing : 0.0f;
    if ((double) charSpacing != (double) this.m_previousCharacterSpacing && !this.m_isEmfTextScaled)
    {
      this.m_streamWriter.SetCharacterSpacing(charSpacing);
      this.m_previousCharacterSpacing = charSpacing;
    }
    float wordSpacing = format != null ? format.WordSpacing : 0.0f;
    if ((double) wordSpacing == (double) this.m_previousWordSpacing)
      return;
    this.m_streamWriter.SetWordSpacing(wordSpacing);
    this.m_previousWordSpacing = wordSpacing;
  }

  private float GetHorizontalAlignShift(float lineWidth, float boundsWidth, PdfStringFormat format)
  {
    float horizontalAlignShift = 0.0f;
    if ((double) boundsWidth >= 0.0 && format != null && format.Alignment != PdfTextAlignment.Left)
    {
      switch (format.Alignment)
      {
        case PdfTextAlignment.Center:
          horizontalAlignShift = (float) (((double) boundsWidth - (double) lineWidth) / 2.0);
          break;
        case PdfTextAlignment.Right:
          horizontalAlignShift = boundsWidth - lineWidth;
          break;
      }
    }
    return horizontalAlignShift;
  }

  internal float GetTextVerticalAlignShift(
    float textHeight,
    float boundsHeight,
    PdfStringFormat format)
  {
    float verticalAlignShift = 0.0f;
    if ((double) boundsHeight >= 0.0 && format != null && format.LineAlignment != PdfVerticalAlignment.Top)
    {
      switch (format.LineAlignment)
      {
        case PdfVerticalAlignment.Middle:
          verticalAlignShift = (float) (((double) boundsHeight - (double) textHeight) / 2.0);
          break;
        case PdfVerticalAlignment.Bottom:
          verticalAlignShift = boundsHeight - textHeight;
          break;
      }
    }
    return verticalAlignShift;
  }

  private float JustifyLine(LineInfo lineInfo, float boundsWidth, PdfStringFormat format)
  {
    string text = lineInfo.Text;
    float width = lineInfo.Width;
    bool flag1 = this.ShouldJustify(lineInfo, boundsWidth, format);
    bool flag2 = format != null && (double) format.WordSpacing != 0.0;
    char[] spaces = StringTokenizer.Spaces;
    int charsCount = StringTokenizer.GetCharsCount(text, spaces);
    float wordSpacing = 0.0f;
    if (flag1)
    {
      if (flag2)
        width -= (float) charsCount * format.WordSpacing;
      wordSpacing = (boundsWidth - width) / (float) charsCount;
      this.m_streamWriter.SetWordSpacing(wordSpacing);
    }
    else if (format != null && format.Alignment == PdfTextAlignment.Justify)
      this.m_streamWriter.SetWordSpacing(0.0f);
    return wordSpacing;
  }

  private bool ShouldJustify(LineInfo lineInfo, float boundsWidth, PdfStringFormat format)
  {
    string text = lineInfo.Text;
    float width = lineInfo.Width;
    bool flag1 = format != null && format.Alignment == PdfTextAlignment.Justify;
    bool flag2 = (double) boundsWidth >= 0.0 && (double) width < (double) boundsWidth;
    char[] spaces = StringTokenizer.Spaces;
    bool flag3 = StringTokenizer.GetCharsCount(text, spaces) > 0 && text[0] != ' ';
    bool flag4 = (lineInfo.LineType & LineType.LayoutBreak) > LineType.None || format != null && format.WordWrap == PdfWordWrapType.None;
    return flag1 && flag2 && flag3 && flag4;
  }

  private bool CheckCorrectLayoutRectangle(ref RectangleF layoutRectangle) => true;

  internal RectangleF CheckCorrectLayoutRectangle(
    SizeF textSize,
    float x,
    float y,
    PdfStringFormat format)
  {
    RectangleF rectangleF = new RectangleF(x, y, textSize.Width, textSize.Width);
    if (format != null)
    {
      switch (format.Alignment)
      {
        case PdfTextAlignment.Center:
          rectangleF.X -= rectangleF.Width / 2f;
          break;
        case PdfTextAlignment.Right:
          rectangleF.X -= rectangleF.Width;
          break;
      }
      switch (format.LineAlignment)
      {
        case PdfVerticalAlignment.Middle:
          rectangleF.Y -= rectangleF.Height / 2f;
          break;
        case PdfVerticalAlignment.Bottom:
          rectangleF.Y -= rectangleF.Height;
          break;
      }
    }
    return rectangleF;
  }

  private void UnderlineStrikeoutText(
    PdfPen pen,
    PdfBrush brush,
    PdfStringLayoutResult result,
    PdfFont font,
    RectangleF layoutRectangle,
    PdfStringFormat format)
  {
    if (result == null)
      throw new ArgumentNullException(nameof (result));
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    if (!font.Underline && !font.Strikeout)
      return;
    PdfPen underlineStikeoutPen = this.CreateUnderlineStikeoutPen(pen, brush, font, format);
    if (underlineStikeoutPen == null)
      return;
    float verticalAlignShift = this.GetTextVerticalAlignShift(result.ActualSize.Height, layoutRectangle.Height, format);
    if (format != null && format.SubSuperScript == PdfSubSuperScript.SubScript)
      verticalAlignShift += font.Height - font.Metrics.GetHeight(format);
    float num1 = this.m_isEMFPlus || this.m_isEMF || this.IsDirectPDF ? (float) ((double) layoutRectangle.Y + (double) verticalAlignShift + (double) this.GetAscent(font) + 1.5 * (double) underlineStikeoutPen.Width) : (float) (-((double) layoutRectangle.Y + (double) font.Height) - ((double) font.Metrics.GetDescent(format) > 0.0 ? -(double) font.Metrics.GetDescent(format) : (double) font.Metrics.GetDescent(format)) - 1.5 * (double) underlineStikeoutPen.Width) - verticalAlignShift;
    float num2 = (float) ((double) layoutRectangle.Y + (double) verticalAlignShift + (double) font.Metrics.GetHeight(format) / 2.0 + 1.5 * (double) underlineStikeoutPen.Width);
    LineInfo[] lines = result.Lines;
    int index = 0;
    for (int lineCount = result.LineCount; index < lineCount; ++index)
    {
      LineInfo lineInfo = lines[index];
      string text = lineInfo.Text;
      float width = lineInfo.Width;
      float horizontalAlignShift = this.GetHorizontalAlignShift(width, layoutRectangle.Width, format);
      float lineIndent = this.GetLineIndent(lineInfo, format, layoutRectangle, index == 0);
      float num3 = horizontalAlignShift + (!this.RightToLeft(format) ? lineIndent : 0.0f);
      float x1 = layoutRectangle.X + num3;
      float x2 = !this.ShouldJustify(lineInfo, layoutRectangle.Width, format) ? x1 + width - lineIndent : x1 + layoutRectangle.Width - lineIndent;
      if (font.Underline)
      {
        float num4 = num1;
        if (this.m_isEMFPlus || this.m_isEMF || this.IsDirectPDF)
        {
          if (this.Tag != null || this.IsTaggedPdf)
            this.isTemplateGraphics = true;
          this.DrawLine(underlineStikeoutPen, x1, num4, x2, num4);
          if (this.Tag != null || this.IsTaggedPdf)
            this.isTemplateGraphics = false;
          num1 += result.LineHeight;
        }
        else
        {
          if (this.Tag != null || this.IsTaggedPdf)
            this.isTemplateGraphics = true;
          this.DrawLine(underlineStikeoutPen, x1, -num4, x2, -num4);
          if (this.Tag != null || this.IsTaggedPdf)
            this.isTemplateGraphics = false;
          num1 -= result.LineHeight;
        }
      }
      if (font.Strikeout)
      {
        float num5 = num2;
        this.DrawLine(underlineStikeoutPen, x1, num5, x2, num5);
        num2 += result.LineHeight;
      }
    }
  }

  private PdfPen CreateUnderlineStikeoutPen(
    PdfPen pen,
    PdfBrush brush,
    PdfFont font,
    PdfStringFormat format)
  {
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    float width = font.Metrics.GetSize(format) / 20f;
    PdfPen underlineStikeoutPen = (PdfPen) null;
    if (pen != null)
      underlineStikeoutPen = new PdfPen(pen.Color, width);
    else if (brush != null)
      underlineStikeoutPen = new PdfPen(brush, width);
    return underlineStikeoutPen;
  }

  private void DrawLayoutResult(
    PdfStringLayoutResult result,
    PdfFont font,
    PdfStringFormat format,
    RectangleF layoutRectangle)
  {
    if (result == null)
      throw new ArgumentNullException(nameof (result));
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    LineInfo[] lines = result.Lines;
    float num = format == null || (double) format.LineSpacing == 0.0 ? font.Height : format.LineSpacing + font.Height;
    bool flag = font is PdfTrueTypeFont pdfTrueTypeFont && pdfTrueTypeFont.Unicode;
    bool embed = pdfTrueTypeFont != null && pdfTrueTypeFont.Embed;
    int index = 0;
    for (int length = lines.Length; index < length; ++index)
    {
      LineInfo lineInfo = lines[index];
      string text = lineInfo.Text;
      float width = lineInfo.Width;
      if (text == null || text.Length == 0)
      {
        float verticalAlignShift = this.GetTextVerticalAlignShift(result.ActualSize.Height, layoutRectangle.Height, format);
        PdfTransformationMatrix matrix = new PdfTransformationMatrix();
        float offsetY = (float) -((double) layoutRectangle.Y + (double) font.Height) - font.Metrics.GetDescent(format) - verticalAlignShift - num * (float) (index + 1);
        matrix.Translate(layoutRectangle.X, offsetY);
        this.m_streamWriter.ModifyTM(matrix);
      }
      else
      {
        float horizontalAlignShift = this.GetHorizontalAlignShift(width, layoutRectangle.Width, format);
        float lineIndent = this.GetLineIndent(lineInfo, format, layoutRectangle, index == 0);
        float x = horizontalAlignShift + (!this.RightToLeft(format) ? lineIndent : 0.0f);
        if ((double) x != 0.0 && !this.m_isEmfTextScaled)
          this.m_streamWriter.StartNextLine(x, 0.0f);
        if (font is PdfCjkStandardFont)
          this.DrawCjkString(lineInfo, layoutRectangle, font, format);
        else if (flag)
          this.DrawUnicodeLine(lineInfo, layoutRectangle, font, format);
        else if (embed)
          this.DrawAsciiLine(lineInfo, layoutRectangle, font, format, embed);
        else
          this.DrawAsciiLine(lineInfo, layoutRectangle, font, format);
        if (index + 1 != length)
        {
          float verticalAlignShift = this.GetTextVerticalAlignShift(result.ActualSize.Height, layoutRectangle.Height, format);
          PdfTransformationMatrix matrix = new PdfTransformationMatrix();
          float offsetY = (float) -((double) layoutRectangle.Y + (double) font.Height) - font.Metrics.GetDescent(format) - verticalAlignShift - num * (float) (index + 1);
          matrix.Translate(layoutRectangle.X, offsetY);
          this.m_streamWriter.ModifyTM(matrix);
        }
      }
    }
    this.m_getResources().RequireProcSet("Text");
  }

  private void DrawAsciiLine(
    LineInfo lineInfo,
    RectangleF layoutRectangle,
    PdfFont font,
    PdfStringFormat format,
    bool embed)
  {
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    double num = (double) this.JustifyLine(lineInfo, layoutRectangle.Width, format);
    string text = lineInfo.Text;
    this.m_streamWriter.ShowNextLineText(this.GetAsciiString(text));
    PdfTrueTypeFont ttfFont = font as PdfTrueTypeFont;
    this.ConvertToUnicode(text, ttfFont, format);
  }

  internal void DrawStringLayoutResult(
    PdfStringLayoutResult result,
    PdfFont font,
    PdfPen pen,
    PdfBrush brush,
    RectangleF layoutRectangle,
    PdfStringFormat format,
    double maxRowFontSize,
    PdfFont maxPdfFont,
    PdfStringFormat maxPdfFormat)
  {
    if (result == null)
      throw new ArgumentNullException(nameof (result));
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    if (result.Empty)
      return;
    bool flag1 = format != null && !format.LineLimit;
    bool flag2 = format == null || !format.NoClip;
    bool flag3 = flag1 && flag2;
    PdfGraphicsState state = (PdfGraphicsState) null;
    this.BeginMarkContent();
    if (flag3)
    {
      state = this.Save();
      RectangleF rectangle = new RectangleF(layoutRectangle.Location, result.ActualSize);
      if ((double) layoutRectangle.Width > 0.0)
        rectangle.Width = layoutRectangle.Width;
      if (format.LineAlignment == PdfVerticalAlignment.Middle)
        rectangle.Y += (float) (((double) layoutRectangle.Height - (double) rectangle.Height) / 2.0);
      else if (format.LineAlignment == PdfVerticalAlignment.Bottom)
        rectangle.Y += layoutRectangle.Height - rectangle.Height;
      this.SetClip(rectangle);
    }
    this.ApplyStringSettings(font, pen, brush, format, layoutRectangle);
    float textScaling = format != null ? format.HorizontalScalingFactor : 100f;
    if ((double) textScaling != (double) this.m_previousTextScaling)
    {
      this.m_streamWriter.SetTextScaling(textScaling);
      this.m_previousTextScaling = textScaling;
    }
    float num1 = this.GetTextVerticalAlignShift(result.ActualSize.Height, layoutRectangle.Height, format);
    float num2 = format == null || (double) format.LineSpacing == 0.0 ? font.Height : format.LineSpacing;
    bool flag4 = format != null && format.SubSuperScript == PdfSubSuperScript.SubScript;
    float num3 = 0.0f;
    float num4 = this.m_isEMFPlus || this.m_isUseFontSize ? (!this.m_isUseFontSize ? (format.LineAlignment != PdfVerticalAlignment.Bottom || (double) num3 != 0.0 ? (flag4 ? num2 - (font.Height + font.Metrics.GetDescent(format)) : num2 - font.Metrics.GetAscent(format)) : (flag4 ? maxPdfFont.Height - (maxPdfFont.Height + maxPdfFont.Metrics.GetDescent(maxPdfFormat)) : maxPdfFont.Height - maxPdfFont.Metrics.GetAscent(maxPdfFormat))) : (format.LineAlignment != PdfVerticalAlignment.Bottom || (double) num3 != 0.0 ? (flag4 ? num2 - (font.Height + font.Metrics.GetDescent(format)) : num2 - font.Size) : (flag4 ? maxPdfFont.Height - (maxPdfFont.Height + maxPdfFont.Metrics.GetDescent(maxPdfFormat)) : maxPdfFont.Height - maxPdfFont.Size))) : (format.LineAlignment != PdfVerticalAlignment.Bottom || (double) num3 != 0.0 ? (flag4 ? num2 - (font.Height + font.Metrics.GetDescent(format)) : num2 - font.Metrics.GetAscent(format)) : (flag4 ? maxPdfFont.Height - (maxPdfFont.Height + maxPdfFont.Metrics.GetDescent(maxPdfFormat)) : maxPdfFont.Height - maxPdfFont.Metrics.GetAscent(maxPdfFormat)));
    if (this.m_isEMF && this.m_isBaselineFormat && format != null && format.Alignment != PdfTextAlignment.Right)
      num4 = 0.0f;
    PdfTransformationMatrix matrix = new PdfTransformationMatrix();
    if (this.m_isEMF && this.m_isBaselineFormat && format != null && format.Alignment != PdfTextAlignment.Right)
      matrix.Translate(layoutRectangle.X, -layoutRectangle.Y);
    else if (this.m_isEMF || this.m_isEMFPlus || this.IsDirectPDF)
      matrix.Translate(layoutRectangle.X, (float) -((double) layoutRectangle.Y + (double) this.GetAscent(font)) - num1);
    else
      matrix.Translate(layoutRectangle.X, (float) (-((double) layoutRectangle.Y + (double) font.Height) - ((double) font.Metrics.GetDescent(format) > 0.0 ? -(double) font.Metrics.GetDescent(format) : (double) font.Metrics.GetDescent(format))) - num1);
    this.m_streamWriter.ModifyTM(matrix);
    if ((double) layoutRectangle.Height < (double) font.Size && (double) result.ActualSize.Height - (double) layoutRectangle.Height < (double) font.Size / 2.0 - 1.0)
      num1 = 0.0f;
    if (this.m_isEMF && (double) result.ActualSize.Height - (double) layoutRectangle.Height > (double) font.Size / 2.0 - 1.0)
      num1 = this.GetTextVerticalAlignShift(result.ActualSize.Height, font.Height, format);
    this.DrawLayoutResult(result, font, format, layoutRectangle);
    if ((double) num1 != 0.0)
      this.m_streamWriter.StartNextLine(0.0f, (float) -((double) num1 - (double) result.LineHeight));
    if (this.Layer != null && this.Page != null && this.Page is PdfPage && (this.Page as PdfPage).Section.ParentDocument is PdfDocument && (this.Page as PdfPage).Section.ParentDocument.FileStructure.TaggedPdf || PdfCatalog.StructTreeRoot != null && !this.IsTemplateGraphics && (this.Tag != null || this.IsTaggedPdf))
      this.m_streamWriter.WriteTag("EMC");
    this.m_streamWriter.EndText();
    this.UnderlineStrikeoutText(pen, brush, result, font, layoutRectangle, format);
    this.m_isEMFPlus = false;
    this.m_isUseFontSize = false;
    if (flag3)
      this.Restore(state);
    this.EndMarkContent();
  }

  internal void DrawStringLayoutResult(
    PdfStringLayoutResult result,
    PdfFont font,
    PdfPen pen,
    PdfBrush brush,
    RectangleF layoutRectangle,
    PdfStringFormat format)
  {
    if (result == null)
      throw new ArgumentNullException(nameof (result));
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    if (result.Empty && !this.IsTaggedPdf)
      return;
    bool flag1 = format != null && !format.LineLimit;
    bool flag2 = format == null || !format.NoClip;
    bool flag3 = flag1 && flag2;
    PdfGraphicsState state = (PdfGraphicsState) null;
    this.BeginMarkContent();
    if (flag3)
    {
      state = this.Save();
      RectangleF rectangle = new RectangleF(layoutRectangle.Location, result.ActualSize);
      if ((double) layoutRectangle.Width > 0.0)
        rectangle.Width = layoutRectangle.Width;
      if (format.LineAlignment == PdfVerticalAlignment.Middle)
        rectangle.Y += (float) (((double) layoutRectangle.Height - (double) rectangle.Height) / 2.0);
      else if (format.LineAlignment == PdfVerticalAlignment.Bottom)
        rectangle.Y += layoutRectangle.Height - rectangle.Height;
      this.SetClip(rectangle);
    }
    if (font.Name.Equals("MS PMincho") || font.Name.Equals("Arial Unicode MS") || font.Name.Equals("MS Mincho") || font.Name.Equals("MS PGothic"))
    {
      this.gState = this.Save();
      this.m_isRestoreGraphics = true;
    }
    this.ApplyStringSettings(font, pen, brush, format, layoutRectangle);
    float textScaling = format != null ? format.HorizontalScalingFactor : 100f;
    if ((double) textScaling != (double) this.m_previousTextScaling && !this.m_isEmfTextScaled)
    {
      this.m_streamWriter.SetTextScaling(textScaling);
      this.m_previousTextScaling = textScaling;
    }
    float num1 = this.GetTextVerticalAlignShift(result.ActualSize.Height, layoutRectangle.Height, format);
    float leading = format == null || (double) format.LineSpacing == 0.0 ? font.Height : format.LineSpacing + font.Height;
    bool flag4 = format != null && format.SubSuperScript == PdfSubSuperScript.SubScript;
    float num2 = this.m_isEMFPlus || this.m_isUseFontSize ? (!this.m_isUseFontSize ? (flag4 ? leading - (font.Height + font.Metrics.GetDescent(format)) : leading - font.Metrics.GetAscent(format)) : (flag4 ? leading - (font.Height + font.Metrics.GetDescent(format)) : leading - font.Size)) : (flag4 ? leading - (font.Height + font.Metrics.GetDescent(format)) : leading - font.Metrics.GetAscent(format));
    if (this.m_isEMF && this.m_isBaselineFormat && format != null && format.Alignment != PdfTextAlignment.Right)
      num2 = 0.0f;
    if (format != null && format.LineAlignment == PdfVerticalAlignment.Bottom && this.m_isBaselineFormat && !this.m_isEMFPlus && (double) layoutRectangle.Height - (double) result.ActualSize.Height != 0.0 && (double) layoutRectangle.Height - (double) result.ActualSize.Height < (double) font.Size / 2.0 - 1.0 && Math.Round((double) layoutRectangle.Height, 2) <= Math.Round((double) font.Height, 2))
      num2 = (float) -((double) leading / (double) font.Size);
    if ((font.Name.Equals("MS PMincho") || font.Name.Equals("Arial Unicode MS") || font.Name.Equals("MS Mincho") || font.Name.Equals("MS PGothic")) && font.Italic)
    {
      this.TranslateTransform(layoutRectangle.X + font.Size / 5f, layoutRectangle.Y - num2);
      this.SkewTransform(0.0f, -11f);
      this.m_isItalic = true;
    }
    if (format != null && format.EnableBaseline)
    {
      float num3 = font.Metrics.GetDescent(format);
      if ((double) num3 < 0.0)
        num3 = -num3;
      if (format.LineAlignment == PdfVerticalAlignment.Bottom)
        layoutRectangle.Y += num3;
      if (format.LineAlignment == PdfVerticalAlignment.Top)
        layoutRectangle.Y -= num3;
    }
    if (this.m_isEMF && this.m_isEmfTextScaled)
    {
      PdfTransformationMatrix matrix = new PdfTransformationMatrix();
      if ((double) leading > (double) font.Size && !this.m_isBaselineFormat)
        matrix.Translate(layoutRectangle.X, (float) -((double) layoutRectangle.Y + (double) leading - (double) num2));
      else
        matrix.Translate(layoutRectangle.X, (float) -((double) layoutRectangle.Y - (double) num2));
      matrix.Scale(this.m_emfScalingFactor.Width, this.m_emfScalingFactor.Height);
      this.m_streamWriter.ModifyTM(matrix);
    }
    else if (!this.m_isItalic)
    {
      PdfTransformationMatrix matrix = new PdfTransformationMatrix();
      if (this.m_isEMF && this.m_isBaselineFormat && format != null && format.Alignment != PdfTextAlignment.Right)
        matrix.Translate(layoutRectangle.X, -layoutRectangle.Y);
      else if (format != null && format.SubSuperScript == PdfSubSuperScript.SuperScript)
      {
        if (this.m_isEMF || this.m_isEMFPlus || this.IsDirectPDF)
          matrix.Translate(layoutRectangle.X, (float) (-((double) layoutRectangle.Y + (double) this.GetAscent(font)) + (double) font.Height / 1.5));
        else
          matrix.Translate(layoutRectangle.X, (float) (-((double) layoutRectangle.Y + (double) font.Height) + ((double) font.Metrics.GetDescent(format) > 0.0 ? -(double) font.Metrics.GetDescent(format) : (double) font.Metrics.GetDescent(format)) + (double) font.Height / 1.5));
      }
      else if (this.m_isEMF || this.m_isEMFPlus || this.IsDirectPDF)
        matrix.Translate(layoutRectangle.X, (float) -((double) layoutRectangle.Y + (double) this.GetAscent(font)) - num1);
      else
        matrix.Translate(layoutRectangle.X, (float) (-((double) layoutRectangle.Y + (double) font.Height) - ((double) font.Metrics.GetDescent(format) > 0.0 ? -(double) font.Metrics.GetDescent(format) : (double) font.Metrics.GetDescent(format))) - num1);
      this.m_streamWriter.ModifyTM(matrix);
    }
    else
      this.m_streamWriter.StartNextLine(0.0f, 0.0f);
    if (this.m_isEMF && this.m_isBaselineFormat && format != null && format.Alignment == PdfTextAlignment.Right)
    {
      if ((double) leading > (double) font.Size && this.m_isItalic)
        this.m_streamWriter.SetLeading(leading);
    }
    else if (!this.m_isEMF || !this.m_isBaselineFormat && !this.m_isEmfTextScaled)
    {
      if ((double) leading >= (double) font.Size && this.m_isItalic)
        this.m_streamWriter.SetLeading(leading);
    }
    else
      this.m_streamWriter.SetLeading(0.0f);
    if ((double) layoutRectangle.Height < (double) font.Size && (double) result.ActualSize.Height - (double) layoutRectangle.Height < (double) font.Size / 2.0 - 1.0)
      num1 = 0.0f;
    if (this.m_isEMF && (double) result.ActualSize.Height - (double) layoutRectangle.Height > (double) font.Size / 2.0 - 1.0)
      num1 = this.GetTextVerticalAlignShift(result.ActualSize.Height, font.Height, format);
    if ((double) num1 != 0.0 && !this.m_isEmfTextScaled && format != null && format.LineAlignment == PdfVerticalAlignment.Bottom && this.m_isBaselineFormat && !this.m_isEMFPlus && (double) layoutRectangle.Height - (double) result.ActualSize.Height != 0.0 && (double) layoutRectangle.Height - (double) result.ActualSize.Height > (double) font.Size / 2.0 - 1.0)
      num1 -= (float) (((double) num2 - (flag4 ? (double) leading - ((double) font.Height + (double) font.Metrics.GetDescent(format)) : (double) leading - (double) font.Size)) / 2.0);
    this.DrawLayoutResult(result, font, format, layoutRectangle);
    if ((double) num1 != 0.0 && !this.m_isEmfTextScaled)
      this.m_streamWriter.StartNextLine(0.0f, (float) -((double) num1 - (double) result.LineHeight));
    if (this.Layer != null && this.Page != null && this.Page is PdfPage && (this.Page as PdfPage).Section.ParentDocument is PdfDocument && (this.Page as PdfPage).Section.ParentDocument.FileStructure.TaggedPdf || PdfCatalog.StructTreeRoot != null && !this.IsTemplateGraphics && (this.Tag != null || this.IsTaggedPdf))
    {
      if (this.IsDirectPDF)
      {
        if (this.Tag is PdfStructureElement && !(this.Tag as PdfStructureElement).IsAdded)
          this.m_streamWriter.WriteTag("EMC");
      }
      else
        this.m_streamWriter.WriteTag("EMC");
    }
    if (PdfCatalog.StructTreeRoot != null && this.Layer != null && this.Page != null && this.Page is PdfLoadedPage && (this.Page as PdfLoadedPage).Document.FileStructure.TaggedPdf)
      this.m_streamWriter.WriteTag("EMC");
    this.m_streamWriter.EndText();
    if (this.m_isItalic || this.m_isRestoreGraphics)
    {
      if (this.gState != null)
        this.Restore(this.gState);
      this.m_isItalic = false;
    }
    this.UnderlineStrikeoutText(pen, brush, result, font, layoutRectangle, format);
    this.m_isEMFPlus = false;
    this.m_isUseFontSize = false;
    if (flag3)
      this.Restore(state);
    this.EndMarkContent();
  }

  private float GetAscent(PdfFont pdfFont)
  {
    float ascent;
    switch (pdfFont)
    {
      case PdfTrueTypeFont _ when (pdfFont as PdfTrueTypeFont).InternalFont.Font != null:
        PdfTrueTypeFont pdfTrueTypeFont1 = pdfFont as PdfTrueTypeFont;
        Font font = pdfTrueTypeFont1.InternalFont.Font;
        int emHeight1 = font.FontFamily.GetEmHeight(font.Style);
        int cellAscent1 = font.FontFamily.GetCellAscent(font.Style);
        ascent = pdfTrueTypeFont1.Size * (float) cellAscent1 / (float) emHeight1;
        break;
      case PdfTrueTypeFont pdfTrueTypeFont2 when pdfTrueTypeFont2.TtfReader != null && pdfTrueTypeFont2.TtfReader.m_CellAscent != 0 && pdfTrueTypeFont2.TtfReader.m_EmHeight != 0 && pdfTrueTypeFont2.Name != null && pdfTrueTypeFont2.Name != "Cambria Math":
        int emHeight2 = pdfTrueTypeFont2.TtfReader.m_EmHeight;
        int cellAscent2 = pdfTrueTypeFont2.TtfReader.m_CellAscent;
        ascent = pdfTrueTypeFont2.Size * (float) cellAscent2 / (float) emHeight2;
        break;
      default:
        ascent = pdfFont.Metrics.GetAscent((PdfStringFormat) null);
        break;
    }
    return ascent;
  }

  private float GetLineIndent(
    LineInfo lineInfo,
    PdfStringFormat format,
    RectangleF layoutBounds,
    bool firstLine)
  {
    float lineIndent = 0.0f;
    bool flag = (lineInfo.LineType & LineType.FirstParagraphLine) > LineType.None;
    if (format != null && flag)
    {
      float val2 = firstLine ? format.FirstLineIndent : format.ParagraphIndent;
      lineIndent = (double) layoutBounds.Width > 0.0 ? Math.Min(layoutBounds.Width, val2) : val2;
    }
    return lineIndent;
  }

  private bool RightToLeft(PdfStringFormat format)
  {
    bool left = format != null && format.RightToLeft;
    if (format != null && format.TextDirection != PdfTextDirection.None)
      left = true;
    return left;
  }

  internal RectangleF GetLineBounds(
    int lineIndex,
    PdfStringLayoutResult result,
    PdfFont font,
    RectangleF layoutRectangle,
    PdfStringFormat format)
  {
    if (result == null)
      throw new ArgumentNullException(nameof (result));
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    RectangleF lineBounds = RectangleF.Empty;
    if (!result.Empty && lineIndex < result.LineCount && lineIndex >= 0)
    {
      LineInfo line = result.Lines[lineIndex];
      float y = (float) ((double) this.GetTextVerticalAlignShift(result.ActualSize.Height, layoutRectangle.Height, format) + (double) layoutRectangle.Y + (double) result.LineHeight * (double) lineIndex);
      float width1 = line.Width;
      float horizontalAlignShift = this.GetHorizontalAlignShift(width1, layoutRectangle.Width, format);
      float lineIndent = this.GetLineIndent(line, format, layoutRectangle, lineIndex == 0);
      float num = horizontalAlignShift + (!this.RightToLeft(format) ? lineIndent : 0.0f);
      float x = layoutRectangle.X + num;
      float width2 = !this.ShouldJustify(line, layoutRectangle.Width, format) ? width1 - lineIndent : layoutRectangle.Width - lineIndent;
      float lineHeight = result.LineHeight;
      lineBounds = new RectangleF(x, y, width2, lineHeight);
    }
    return lineBounds;
  }

  internal void SetBBox(RectangleF bounds)
  {
    this.m_streamWriter.GetStream()["BBox"] = (IPdfPrimitive) PdfArray.FromRectangle(bounds);
  }

  private string SetArtifact(PdfArtifact artifact)
  {
    StringBuilder stringBuilder = new StringBuilder();
    if (artifact.ArtifactType != PdfArtifactType.None)
      stringBuilder.Append($"/Type /{artifact.ArtifactType.ToString()} ");
    if (artifact.BoundingBox != RectangleF.Empty)
      stringBuilder.Append($"/BBox [{(object) artifact.BoundingBox.X} {(object) artifact.BoundingBox.Y} {(object) artifact.BoundingBox.Width} {(object) artifact.BoundingBox.Height}] ");
    if (artifact.Attached != null)
      stringBuilder.Append($"/Attached [{this.GetEdges(artifact.Attached)}]");
    if (artifact.SubType != PdfArtifactSubType.None)
      stringBuilder.Append("/Subtype /" + artifact.SubType.ToString());
    return stringBuilder.ToString();
  }

  private string GetEdges(PdfAttached attached)
  {
    StringBuilder stringBuilder = new StringBuilder();
    if (attached.Bottom)
      stringBuilder.Append("/Bottom ");
    if (attached.Top)
      stringBuilder.Append("/Top ");
    if (attached.Left)
      stringBuilder.Append("/Left ");
    if (attached.Right)
      stringBuilder.Append("/Right ");
    return stringBuilder.ToString();
  }

  protected void OnStructElementChanged(PdfTag tag)
  {
    PdfGraphics.StructElementEventHandler structElementChanged = this.StructElementChanged;
    if (structElementChanged == null)
      return;
    structElementChanged(tag);
  }

  internal void ApplyTag(PdfTag tag)
  {
    PdfStructTreeRoot structTreeRoot = PdfCatalog.StructTreeRoot;
    switch (tag)
    {
      case PdfStructureElement _:
        int num = structTreeRoot.Add(tag as PdfStructureElement, this.Page, tag.Bounds);
        this.StreamWriter.WriteTag(string.Format($"/{{0}} <</E ({(this.Tag as PdfStructureElement).Abbrevation}) /MCID {{1}} >>BDC", (object) structTreeRoot.ConvertToEquivalentTag((this.Tag as PdfStructureElement).TagType), (object) num));
        break;
      case PdfArtifact _:
        this.StreamWriter.Write(string.Format("/Artifact << {0} >>BDC" + Environment.NewLine, (object) this.SetArtifact(tag as PdfArtifact)));
        break;
    }
  }

  internal void DrawString(
    string text,
    PdfFont font,
    PdfBrush brush,
    RectangleF rect,
    PdfStringFormat format,
    bool directConversion)
  {
    if (text == null)
      throw new ArgumentNullException(nameof (text));
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    if (brush == null)
      throw new ArgumentNullException(nameof (brush));
    if (format == null)
      throw new ArgumentNullException(nameof (format));
    if (font is PdfTrueTypeFont && PdfDocument.ConformanceLevel != PdfConformanceLevel.None && font is PdfTrueTypeFont pdfTrueTypeFont && pdfTrueTypeFont.Font != null)
      font = (PdfFont) new PdfTrueTypeFont(pdfTrueTypeFont.Font, font.Size, true, false, pdfTrueTypeFont.Embed);
    this.m_isUseFontSize = font.Name.ToLower().Contains("calibri");
    if (PdfString.IsUnicode(text))
    {
      PdfTrueTypeFont font1 = font as PdfTrueTypeFont;
      int style = (int) font.Style;
      if (!font1.Unicode)
      {
        font = (PdfFont) new PdfTrueTypeFont(new Font(font.Name, font.Size, (FontStyle) style), true, false, font1.Embed);
        font1 = font as PdfTrueTypeFont;
      }
      if (font1 != null && font1.Font != null)
      {
        bool flag = this.IsContainFont(font1, text, format);
        if (format != null)
        {
          if (!format.RightToLeft)
          {
            if (format.TextDirection != PdfTextDirection.None)
            {
              if (!format.isCustomRendering)
                goto label_20;
            }
            else
              goto label_20;
          }
          if (!flag)
          {
            font = (PdfFont) new PdfTrueTypeFont(new Font("Times New Roman", font.Size, (FontStyle) style), true, false, font1.Embed);
            flag = this.IsContainFont(font as PdfTrueTypeFont, text, format);
          }
        }
label_20:
        try
        {
          if (!flag)
            font = (PdfFont) new PdfTrueTypeFont(new Font("Arial Unicode MS", font.Size, (FontStyle) font.Style), true, false, font1.Embed);
        }
        catch
        {
          if (font.Name.ToLower().Equals("microsoft sans serif"))
            font = (PdfFont) new PdfTrueTypeFont(new Font("Arial Unicode MS", font.Size, (FontStyle) font.Style), true, false, font1.Embed);
          else if (font.Name.ToLower().Equals("arial"))
            font = (PdfFont) new PdfTrueTypeFont(new Font("Arial", font.Size, (FontStyle) font.Style), true, false, font1.Embed);
        }
      }
    }
    SizeF textSize = rect.Size;
    float num = this.ScaleText(text, font, rect, out textSize, format);
    this.m_isEMFPlus = true;
    if ((double) num != 1.0)
    {
      if (format == null)
        format = new PdfStringFormat();
      format.HorizontalScalingFactor = num * 100f;
    }
    this.OnDrawPrimitive();
    rect.Width /= num;
    if ((double) rect.Width == 0.0)
      rect.Width = textSize.Width;
    else if ((double) rect.Height == 0.0)
      rect.Height = textSize.Height;
    if (font.Name.ToLower().Contains("calibri"))
      this.m_isUseFontSize = true;
    format.LineLimit = false;
    format.NoClip = true;
    if ((double) rect.Width <= 0.0)
      return;
    this.DrawString(text, font, brush, rect, format);
  }

  private bool IsContainFont(PdfTrueTypeFont font, string text, PdfStringFormat format)
  {
    if (this.IsOpenTypeFont(format, font))
    {
      OtfGlyphInfoList glyphList = (OtfGlyphInfoList) null;
      ScriptTags[] tags = this.ObtainTags(text);
      double lineWidth = (double) font.GetLineWidth(text, format, out glyphList, tags);
    }
    else
    {
      double lineWidth1 = (double) font.GetLineWidth(text, (PdfStringFormat) null);
    }
    return font.IsContainsFont;
  }

  private bool IsOpenTypeFont(PdfStringFormat format, PdfTrueTypeFont font)
  {
    bool flag = false;
    if (format != null && format.ComplexScript && font.InternalFont is UnicodeTrueTypeFont)
      flag = font.TtfReader.isOTFFont();
    return flag;
  }

  private ScriptTags[] ObtainTags(string line)
  {
    Dictionary<ScriptTags, int> dictionary = new Dictionary<ScriptTags, int>();
    LanguageUtil languageUtil = new LanguageUtil();
    for (int index = 0; index < line.Length; ++index)
    {
      ScriptTags language = languageUtil.GetLanguage(line[index]);
      if (language != (ScriptTags) 0)
      {
        if (dictionary.ContainsKey(language))
          ++dictionary[language];
        else
          dictionary.Add(language, 1);
      }
    }
    ScriptTags[] array = new ScriptTags[dictionary.Count];
    dictionary.Keys.CopyTo(array, 0);
    return array;
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
    float num = 1f;
    textSize = rect.Size;
    if (text.Length > 0)
    {
      if (format == null)
        format = new PdfStringFormat();
      if (text.EndsWith(" "))
        format.MeasureTrailingSpaces = true;
      textSize = pdfFont.MeasureString(text, format);
      if ((double) rect.Width > 0.0 && (double) rect.Width < 2147483648.0 && (double) textSize.Width > (double) rect.Width && (double) textSize.Width > 0.0)
        num = rect.Width / textSize.Width;
    }
    return num;
  }

  internal void ResetClip()
  {
    this.ClipBounds = this.DocIOPageBounds;
    this.NativeGraphics.ResetClip();
    this.m_stateChanged = true;
    this.m_textClip = RectangleF.Empty;
    this.m_clipPath = false;
  }

  private PdfTransformationMatrix PrepareMatrix(System.Drawing.Drawing2D.Matrix matrix, float pageScale)
  {
    PdfTransformationMatrix transformationMatrix = new PdfTransformationMatrix();
    PdfTransformationMatrix matrix1 = new PdfTransformationMatrix();
    matrix1.Matrix = matrix;
    transformationMatrix.Scale(pageScale, -pageScale);
    transformationMatrix.Multiply(matrix1);
    transformationMatrix.Scale(1f, -1f);
    return transformationMatrix;
  }

  internal void SetClip(RectangleF rect, CombineMode mode)
  {
    this.NativeGraphics.SetClip(rect, mode);
    this.RealClip = rect;
    this.m_textClip = rect;
    this.SetClip();
  }

  private void OnDrawPrimitive()
  {
    if (!this.m_stateChanged)
      return;
    this.InternalResetClip();
    this.m_bFirstCall = false;
    this.PutComment(nameof (OnDrawPrimitive));
    this.Save();
    this.SetPdfClipPath();
    this.m_stateChanged = false;
  }

  internal PdfStringFormat ConvertFormat(StringFormat format) => this.ConvertFormat(format, false);

  internal PdfStringFormat ConvertFormat(StringFormat format, bool isComplexScript)
  {
    PdfStringFormat pdfStringFormat = (PdfStringFormat) null;
    if (format != null)
    {
      this.PutComment($"String Format Flags: {(object) format.FormatFlags}({(object) (int) format.FormatFlags})");
      this.PutComment("Alignment: " + (object) format.Alignment);
      this.PutComment("Line Alignment: " + (object) format.LineAlignment);
      pdfStringFormat = new PdfStringFormat();
      pdfStringFormat.LineLimit = false;
      pdfStringFormat.Alignment = this.ConvertAlingnmet(format.Alignment);
      pdfStringFormat.LineAlignment = this.ConvertLineAlignment(format.LineAlignment);
      pdfStringFormat.ComplexScript = isComplexScript;
      format.GetTabStops(out float _);
      pdfStringFormat.NoClip = true;
      pdfStringFormat.TextDirection = (format.FormatFlags & StringFormatFlags.DirectionRightToLeft) != (StringFormatFlags) 0 ? PdfTextDirection.RightToLeft : PdfTextDirection.None;
      pdfStringFormat.isCustomRendering = true;
      if (pdfStringFormat.NoClip)
        pdfStringFormat.LineLimit = false;
      pdfStringFormat.WordWrap = this.GetWrapType(format.FormatFlags);
    }
    return pdfStringFormat;
  }

  private PdfVerticalAlignment ConvertLineAlignment(StringAlignment stringAlignment)
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

  private PdfWordWrapType GetWrapType(StringFormatFlags stringFormatFlags)
  {
    PdfWordWrapType wrapType = PdfWordWrapType.Word;
    if ((stringFormatFlags & StringFormatFlags.NoWrap) != (StringFormatFlags) 0)
      wrapType = PdfWordWrapType.None;
    return wrapType;
  }

  private PdfTextAlignment ConvertAlingnmet(StringAlignment stringAlignment)
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

  private void TranslateTransform(float dx, float dy, MatrixOrder order)
  {
    this.PutComment(nameof (TranslateTransform));
    if (this.NativeGraphics != null)
      this.NativeGraphics.TranslateTransform(dx, dy, order);
    this.m_isTranslate = true;
    if (order == MatrixOrder.Append)
    {
      if ((double) dy < 0.0)
      {
        this.TranslateTransform(dx, dy);
        this.m_translateTransform = new SizeF(-dx, -dy);
      }
      else if ((double) dx > (double) dy)
      {
        this.TranslateTransform(dx, dy);
        this.m_translateTransform = new SizeF(dx, dy);
      }
      else if ((double) dx < 0.0 && (double) dx < (double) dy)
      {
        this.TranslateTransform(dx, dy);
        this.m_translateTransform = new SizeF(dx, PdfGraphics.UpdateY(dy));
      }
      else
        this.TranslateTransform(dx, PdfGraphics.UpdateY(dy));
    }
    else
      this.Transform = this.Transform;
  }

  internal void RotateTransform(float angle, MatrixOrder order)
  {
    this.PutComment(nameof (RotateTransform));
    this.NativeGraphics.RotateTransform(angle, order);
    if (order == MatrixOrder.Append)
      this.RotateTransform(angle);
    else
      this.Transform = this.Transform;
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

  private void InternalResetClip()
  {
    if (this.m_bFirstCall)
      return;
    this.PutComment(nameof (InternalResetClip));
    this.m_bFirstCall = true;
    this.Restore();
    this.m_stateChanged = true;
  }

  private void SetTransform()
  {
    this.InternalResetClip();
    this.PutComment(nameof (SetTransform));
    this.m_clipPath = false;
    if (!this.m_bFirstTransform && !this.m_stateRestored)
    {
      this.Restore();
    }
    else
    {
      this.m_bFirstTransform = false;
      this.m_stateRestored = false;
    }
    this.Save();
  }

  internal void ResetTransform()
  {
    this.PutComment(nameof (ResetTransform));
    this.m_clipPath = false;
    this.InternalResetClip();
    this.SetTransform();
    this.NativeGraphics.ResetTransform();
  }

  private void SetClip() => this.m_stateChanged = true;

  private void SetPdfClipPath()
  {
    GraphicsPath clipPath = this.GetClipPath();
    if (clipPath == null)
      return;
    this.SetClip(new PdfPath(clipPath.PathPoints, clipPath.PathTypes), this.GetPathFillMode(clipPath));
  }

  private GraphicsPath GetClipPath()
  {
    GraphicsPath clipPath = (GraphicsPath) null;
    Region clip = this.NativeGraphics.Clip;
    RectangleF clipBounds = this.NativeGraphics.ClipBounds;
    if (!clip.IsEmpty(this.NativeGraphics) && !clip.IsInfinite(this.NativeGraphics))
    {
      clipPath = new GraphicsPath(FillMode.Winding);
      if ((double) this.RealClip.X != 0.0 || (double) this.RealClip.Y != 0.0 || (double) this.RealClip.Width != 0.0 || (double) this.RealClip.Height != 0.0)
      {
        if ((int) this.RealClip.Width == (int) clipBounds.Width || (int) this.RealClip.Height == (int) clipBounds.Height)
        {
          clipPath.AddRectangle(this.RealClip);
          this.RealClip = RectangleF.Empty;
        }
        else
        {
          clipPath.AddRectangle(clipBounds);
          this.RealClip = RectangleF.Empty;
        }
      }
      else
      {
        RectangleF[] regionScans = clip.GetRegionScans(new System.Drawing.Drawing2D.Matrix());
        if (regionScans.Length > 1)
          clipPath.AddRectangles(regionScans);
        else
          clipPath.AddRectangle(clipBounds);
      }
    }
    return clipPath;
  }

  private PdfFillMode GetPathFillMode(GraphicsPath path)
  {
    if (path == null)
      throw new ArgumentNullException(nameof (path));
    return path.FillMode == FillMode.Winding ? PdfFillMode.Winding : PdfFillMode.Alternate;
  }

  private void InternalResetTransformation()
  {
    PdfTransformationMatrix transformationMatrix = new PdfTransformationMatrix();
    if (this.m_bFirstTransform)
      return;
    this.PutComment(nameof (InternalResetTransformation));
    if (this.m_multiplyTransform != null)
    {
      if ((double) this.m_multiplyTransform.Elements[0] != (double) transformationMatrix.Matrix.Elements[0] || (double) this.m_multiplyTransform.Elements[1] != (double) transformationMatrix.Matrix.Elements[1] || (double) this.m_multiplyTransform.Elements[2] != (double) transformationMatrix.Matrix.Elements[2] || (double) this.m_multiplyTransform.Elements[3] != (double) transformationMatrix.Matrix.Elements[3] || !this.m_isTranslate)
        this.Restore();
    }
    else if (this.m_stateChanged && this.m_bFirstCall && !this.m_stateRestored)
      this.Restore();
    this.m_bFirstTransform = true;
  }

  internal GraphicsState DirectPDFSave()
  {
    this.PutComment("Save");
    this.InternalResetClip();
    this.InternalResetTransformation();
    PdfGraphicsState pdfGraphicsState = this.Save();
    GraphicsState key = this.NativeGraphics.Save();
    if (this.m_graphicsStates == null)
      this.m_graphicsStates = new Hashtable();
    this.m_graphicsStates[(object) key] = (object) pdfGraphicsState;
    return key;
  }

  internal void Restore(GraphicsState gState)
  {
    this.PutComment(nameof (Restore));
    if (this.m_graphicsStates == null)
      this.m_graphicsStates = new Hashtable();
    if (this.m_graphicsStates[(object) gState] is PdfGraphicsState graphicsState)
      this.Restore(graphicsState);
    this.NativeGraphics.Restore(gState);
    this.m_stateChanged = true;
    this.m_bFirstCall = true;
    this.m_bFirstTransform = false;
    this.m_stateRestored = true;
  }

  internal void Clear(PdfColor color)
  {
    this.DrawRectangle((PdfBrush) new PdfSolidBrush(color), this.m_clipBounds);
  }

  internal void Clear() => this.DrawRectangle(PdfBrushes.White, this.m_clipBounds);

  internal void SetTag(PdfStructureElement element) => this.Tag = (PdfTag) element;

  internal void ReSetTag()
  {
    if (this.Tag == null)
      return;
    (this.Tag as PdfStructureElement).IsAdded = false;
    this.Tag = (PdfTag) null;
    this.m_streamWriter.WriteTag("EMC");
  }

  internal PdfImage GetImage(Image image, PdfDocument document)
  {
    PdfImage image1;
    if (image is Metafile)
      image1 = (PdfImage) new PdfMetafile(image as Metafile);
    else if (this.OptimizeIdenticalImages)
    {
      bool flag = false;
      PdfPage page = this.Page as PdfPage;
      string key = (string) null;
      if (page != null && page.Document != null)
      {
        key = page.Document.GetImageHash(image);
        flag = page.Document.ImageCollection.ContainsKey(key);
      }
      if (!string.IsNullOrEmpty(key))
      {
        if (flag)
        {
          image1 = document.ImageCollection[key];
        }
        else
        {
          image1 = (PdfImage) new PdfBitmap(image);
          document.ImageCollection.Add(key, image1);
        }
      }
      else
        image1 = (PdfImage) new PdfBitmap(image);
    }
    else
      image1 = (PdfImage) new PdfBitmap(image);
    return image1;
  }

  internal PdfImage GetImage(Stream stream, PdfDocument document)
  {
    return this.GetImage((stream != null ? Image.FromStream(PdfImage.CheckStreamExistance(stream)) : throw new ArgumentNullException(nameof (stream))) ?? throw new ArgumentNullException("image"), document);
  }

  internal delegate PdfResources GetResources();

  internal delegate void StructElementEventHandler(PdfTag tag);

  private struct TransparencyData
  {
    internal float AlphaPen;
    internal float AlphaBrush;
    internal PdfBlendMode BlendMode;

    internal TransparencyData(float alphaPen, float alphaBrush, PdfBlendMode blendMode)
    {
      this.AlphaPen = alphaPen;
      this.AlphaBrush = alphaBrush;
      this.BlendMode = blendMode;
    }

    public override bool Equals(object obj)
    {
      bool flag = false;
      if (obj != null && obj is PdfGraphics.TransparencyData transparencyData)
        flag = true & (double) this.AlphaBrush == (double) transparencyData.AlphaBrush & (double) this.AlphaPen == (double) transparencyData.AlphaPen & this.BlendMode == transparencyData.BlendMode;
      return flag;
    }

    public override int GetHashCode() => base.GetHashCode();
  }
}
