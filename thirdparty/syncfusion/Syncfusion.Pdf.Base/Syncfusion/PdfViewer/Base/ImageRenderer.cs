// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.ImageRenderer
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class ImageRenderer
{
  private bool m_isNegative;
  internal List<PageURL> URLDictonary = new List<PageURL>();
  internal static List<PageText> textDictonary = new List<PageText>();
  private Dictionary<string, bool> m_layersVisibilityDictionary = new Dictionary<string, bool>();
  private bool m_skipRendering;
  private int m_inlayersCount;
  internal bool IsTextSearch;
  internal bool IsPdfium;
  private string[] m_rectangleValue;
  private bool m_isTrEntry;
  private bool m_isExtendedGraphicStateContainsSMask;
  public bool IsExtendedGraphicsState;
  public bool IsGraphicsState;
  public bool IsPdfiumRendering;
  public GraphicsPath m_graphicspathtoclip;
  internal System.Drawing.Drawing2D.Matrix transformMatrix = new System.Drawing.Drawing2D.Matrix();
  internal PointF currentTransformLocation = new PointF();
  private char[] m_symbolChars = new char[6]
  {
    '(',
    ')',
    '[',
    ']',
    '<',
    '>'
  };
  private char[] m_startText = new char[3]{ '(', '[', '<' };
  private char[] m_endText = new char[3]{ ')', ']', '>' };
  private PdfPageResources m_resources;
  private PdfRecordCollection m_contentElements;
  private PointF m_currentLocation = PointF.Empty;
  private bool m_beginText;
  private System.Drawing.Graphics m_graphics;
  private GraphicsPath m_path;
  private List<GraphicsPath> m_subPaths = new List<GraphicsPath>();
  private List<GraphicsPath> m_tempSubPaths = new List<GraphicsPath>();
  private RectangleF m_clipRectangle;
  private float m_mitterLength;
  internal Stack<GraphicsState> m_graphicsState = new Stack<GraphicsState>();
  internal Stack<GraphicObjectData> m_objects = new Stack<GraphicObjectData>();
  private float m_textScaling = 100f;
  private bool textMatrix;
  private float m_textElementWidth;
  private string[] m_backupColorElements;
  private string m_backupColorSpace;
  private PointF m_endTextPosition;
  private bool m_isCurrentPositionChanged;
  internal bool isFindText;
  internal bool isExportAsImage;
  internal bool isExtractLineCollection;
  private Brush transperentStrokingBrush;
  private Brush transperentNonStrokingBrush;
  private Colorspace m_currentColorspace;
  private PdfViewerExceptions exception = new PdfViewerExceptions();
  private DeviceCMYK decodecmykColor;
  private string[] m_dashedLine;
  private bool isNegativeFont;
  private string m_clippingPath;
  private float m_lineCap;
  private int RenderingMode;
  private float m_opacity;
  private List<RectangleF> m_clipRectangleList = new List<RectangleF>();
  private bool IsTransparentText;
  private List<CffGlyphs> m_glyphDataCollection = new List<CffGlyphs>();
  private System.Drawing.Drawing2D.Matrix m_imageCommonMatrix;
  public char findpath;
  private bool isScaledText;
  private float currentPageHeight;
  public bool isBlack;
  private Dictionary<SystemFontFontDescriptor, SystemFontOpenTypeFontSource> testdict = new Dictionary<SystemFontFontDescriptor, SystemFontOpenTypeFontSource>();
  private PdfDictionary m_inlineParameters = new PdfDictionary();
  internal Dictionary<string, string> substitutedFontsList = new Dictionary<string, string>();
  private List<string> BIparameter = new List<string>();
  internal List<Glyph> imageRenderGlyphList;
  internal List<TextElement> extractTextElement = new List<TextElement>();
  internal float pageRotation;
  internal float zoomFactor = 1f;
  private bool isNextFill;
  private bool isWinAnsiEncoding;
  private int m_tilingType;
  private bool m_isExtractTextData;
  private Matrix XFormsMatrix;
  private Matrix m_d1Matrix;
  private Matrixx m_d0Matrix;
  private Matrix m_type3TextLineMatrix;
  private float m_type3FontScallingFactor;
  private PdfRecordCollection m_type3RecordCollection;
  private bool m_isType3Font;
  private TransformationStack m_transformations;
  private FontStructure m_type3FontStruct;
  private float m_spacingWidth;
  private string m_type3GlyphID;
  private float m_type3WhiteSpaceWidth;
  private Dictionary<string, List<List<int>>> m_type3GlyphPath;
  private bool m_istype3FontContainCTRM;
  private MemoryStream m_outStream;
  internal PathGeometry CurrentGeometry = new PathGeometry();
  private PathGeometry BackupCurrentGeometry = new PathGeometry();
  private PathFigure m_currentPath;
  private bool containsImage;
  internal int xobjectGraphicsCount;
  internal bool isXGraphics;
  private string[] clipRectShape;
  private bool isRect;
  internal float m_characterSpacing;
  internal float m_wordSpacing;
  internal bool m_selectablePrintDocument;
  private float m_textAngle;
  internal float m_pageHeight;
  internal bool m_isPrintSelected;

  internal bool IsExtractTextData
  {
    get => this.m_isExtractTextData;
    set => this.m_isExtractTextData = value;
  }

  public ImageRenderer(
    PdfRecordCollection contentElements,
    PdfPageResources resources,
    System.Drawing.Graphics g,
    bool newPage,
    DeviceCMYK cmyk,
    float pageHeight)
  {
    GraphicObjectData graphicObjectData = new GraphicObjectData();
    graphicObjectData.m_mitterLength = 1f;
    graphicObjectData.Ctm = Matrix.Identity;
    graphicObjectData.Ctm.Translate((double) (g.Transform.Elements[4] / 1.333f), (double) (g.Transform.Elements[5] / 1.333f));
    graphicObjectData.drawing2dMatrixCTM = new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
    graphicObjectData.drawing2dMatrixCTM.Translate(g.Transform.Elements[4] / 1.333f, g.Transform.Elements[5] / 1.333f);
    System.Drawing.Drawing2D.Matrix transform = g.Transform;
    graphicObjectData.documentMatrix = new Matrix(4.0 / 3.0 * ((double) g.DpiX / 96.0) * (double) transform.Elements[0], 0.0, 0.0, -4.0 / 3.0 * ((double) g.DpiX / 96.0) * (double) transform.Elements[3], 0.0, (double) pageHeight * (double) transform.Elements[3]);
    this.m_objects.Push(graphicObjectData);
    this.m_objects.Push(graphicObjectData);
    this.m_contentElements = contentElements;
    this.m_resources = resources;
    this.m_graphics = g;
    this.m_graphics.SmoothingMode = SmoothingMode.AntiAlias;
    g.PageUnit = GraphicsUnit.Point;
    this.currentPageHeight = pageHeight;
    if (newPage)
      g.TranslateTransform(0.0f, g.VisibleClipBounds.Bottom);
    this.decodecmykColor = cmyk;
    this.imageRenderGlyphList = new List<Glyph>();
    this.m_type3GlyphPath = new Dictionary<string, List<List<int>>>();
  }

  public ImageRenderer(
    PdfRecordCollection contentElements,
    PdfPageResources resources,
    System.Drawing.Graphics g,
    bool newPage,
    float pageBottom,
    DeviceCMYK cmyk)
  {
    GraphicObjectData graphicObjectData = new GraphicObjectData();
    graphicObjectData.Ctm = Matrix.Identity;
    graphicObjectData.Ctm.Translate((double) (g.Transform.Elements[4] / 1.333f), (double) (g.Transform.Elements[5] / 1.333f));
    graphicObjectData.drawing2dMatrixCTM = new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
    graphicObjectData.drawing2dMatrixCTM.Translate(g.Transform.Elements[4] / 1.333f, g.Transform.Elements[5] / 1.333f);
    System.Drawing.Drawing2D.Matrix transform = g.Transform;
    graphicObjectData.documentMatrix = new Matrix(4.0 / 3.0 * ((double) g.DpiX / 96.0) * (double) transform.Elements[0], 0.0, 0.0, -4.0 / 3.0 * ((double) g.DpiX / 96.0) * (double) transform.Elements[3], 0.0, (double) pageBottom * (double) transform.Elements[3]);
    this.m_objects.Push(graphicObjectData);
    this.m_contentElements = contentElements;
    this.m_resources = resources;
    this.m_graphics = g;
    this.m_graphics.SmoothingMode = SmoothingMode.AntiAlias;
    g.PageUnit = GraphicsUnit.Point;
    if (newPage)
      g.TranslateTransform(0.0f, g.VisibleClipBounds.Top + pageBottom);
    this.decodecmykColor = cmyk;
    this.imageRenderGlyphList = new List<Glyph>();
  }

  public ImageRenderer(
    PdfRecordCollection contentElements,
    PdfPageResources resources,
    System.Drawing.Graphics g,
    bool newPage,
    float pageBottom,
    float left,
    DeviceCMYK cmyk)
  {
    GraphicObjectData graphicObjectData = new GraphicObjectData();
    graphicObjectData.Ctm = Matrix.Identity;
    graphicObjectData.Ctm.Translate((double) (g.Transform.Elements[4] / 1.333f), (double) (g.Transform.Elements[5] / 1.333f));
    graphicObjectData.drawing2dMatrixCTM = new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
    graphicObjectData.drawing2dMatrixCTM.Translate(g.Transform.Elements[4] / 1.333f, g.Transform.Elements[5] / 1.333f);
    System.Drawing.Drawing2D.Matrix transform = g.Transform;
    graphicObjectData.documentMatrix = new Matrix(4.0 / 3.0 * ((double) g.DpiX / 96.0) * (double) transform.Elements[0], 0.0, 0.0, -4.0 / 3.0 * ((double) g.DpiX / 96.0) * (double) transform.Elements[3], 0.0, (double) pageBottom * (double) transform.Elements[3]);
    this.m_objects.Push(graphicObjectData);
    this.m_objects.Push(graphicObjectData);
    this.m_contentElements = contentElements;
    this.m_resources = resources;
    this.m_graphics = g;
    this.m_graphics.SmoothingMode = SmoothingMode.AntiAlias;
    g.PageUnit = GraphicsUnit.Point;
    this.currentPageHeight = pageBottom;
    if (newPage)
      g.TranslateTransform(left, pageBottom);
    this.decodecmykColor = cmyk;
    this.imageRenderGlyphList = new List<Glyph>();
  }

  public ImageRenderer(
    PdfRecordCollection contentElements,
    PdfPageResources resources,
    System.Drawing.Graphics g,
    bool newPage,
    int height,
    DeviceCMYK cmyk)
  {
    GraphicObjectData graphicObjectData = new GraphicObjectData();
    graphicObjectData.Ctm = Matrix.Identity;
    graphicObjectData.Ctm.Translate((double) (g.Transform.Elements[4] / 1.333f), (double) (g.Transform.Elements[5] / 1.333f));
    graphicObjectData.drawing2dMatrixCTM = new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
    graphicObjectData.drawing2dMatrixCTM.Translate(g.Transform.Elements[4] / 1.333f, g.Transform.Elements[5] / 1.333f);
    System.Drawing.Drawing2D.Matrix transform = g.Transform;
    graphicObjectData.documentMatrix = new Matrix(4.0 / 3.0 * ((double) g.DpiX / 96.0) * (double) transform.Elements[0], 0.0, 0.0, -4.0 / 3.0 * ((double) g.DpiX / 96.0) * (double) transform.Elements[3], 0.0, (double) height * (double) transform.Elements[3]);
    this.m_objects.Push(graphicObjectData);
    this.m_objects.Push(graphicObjectData);
    this.m_contentElements = contentElements;
    this.m_resources = resources;
    this.m_graphics = g;
    this.m_graphics.SmoothingMode = SmoothingMode.AntiAlias;
    g.PageUnit = GraphicsUnit.Point;
    if (newPage)
      g.TranslateTransform(0.0f, (float) height);
    this.decodecmykColor = cmyk;
    this.imageRenderGlyphList = new List<Glyph>();
  }

  public System.Drawing.Graphics PageGraphics => this.m_graphics;

  internal Dictionary<string, bool> LayersVisibilityDictionary
  {
    get => this.m_layersVisibilityDictionary;
    set => this.m_layersVisibilityDictionary = value;
  }

  private PointF CurrentLocation
  {
    get => this.m_currentLocation;
    set
    {
      this.m_currentLocation = value;
      this.m_isCurrentPositionChanged = true;
    }
  }

  private Brush NonStrokingBrush
  {
    get
    {
      if (this.Objects.NonStrokingBrush != null)
        return this.Objects.NonStrokingBrush;
      foreach (GraphicObjectData graphicObjectData in this.m_objects)
      {
        if (graphicObjectData.NonStrokingBrush != null)
          return graphicObjectData.NonStrokingBrush;
      }
      return (Brush) null;
    }
  }

  private Brush StrokingBrush
  {
    get
    {
      if (this.Objects.StrokingBrush != null)
        return this.Objects.StrokingBrush;
      foreach (GraphicObjectData graphicObjectData in this.m_objects)
      {
        if (graphicObjectData.StrokingBrush != null)
          return graphicObjectData.StrokingBrush;
      }
      return (Brush) null;
    }
  }

  internal float StrokingOpacity
  {
    get => this.Objects.m_strokingOpacity;
    set => this.Objects.m_strokingOpacity = value;
  }

  internal float NonStrokingOpacity
  {
    get => this.Objects.m_nonStrokingOpacity;
    set => this.Objects.m_nonStrokingOpacity = value;
  }

  private RectangleF ClipRectangle
  {
    get => this.m_clipRectangle;
    set => this.m_clipRectangle = value;
  }

  private float MitterLength
  {
    get => this.m_objects.ToArray()[0].m_mitterLength;
    set => this.m_objects.ToArray()[0].m_mitterLength = value;
  }

  private float TextScaling
  {
    get => this.m_textScaling;
    set => this.m_textScaling = value;
  }

  private GraphicsPath Path
  {
    get => this.m_path;
    set => this.m_path = value;
  }

  private GraphicObjectData Objects => this.m_objects.Peek();

  private string CurrentFont
  {
    get
    {
      if (this.Objects.CurrentFont != null)
        return this.Objects.CurrentFont;
      string currentFont = "";
      foreach (GraphicObjectData graphicObjectData in this.m_objects)
      {
        if (graphicObjectData.CurrentFont != null)
        {
          currentFont = graphicObjectData.CurrentFont;
          break;
        }
      }
      return currentFont;
    }
    set => this.Objects.CurrentFont = value;
  }

  private float FontSize
  {
    get
    {
      if (this.Objects.CurrentFont != null)
        return this.Objects.FontSize;
      float fontSize = 0.0f;
      foreach (GraphicObjectData graphicObjectData in this.m_objects)
      {
        if (graphicObjectData.CurrentFont != null)
        {
          fontSize = graphicObjectData.FontSize;
          break;
        }
      }
      return fontSize;
    }
    set => this.Objects.FontSize = value;
  }

  private float TextLeading
  {
    get
    {
      if (this.Objects.CurrentFont != null)
        return this.Objects.TextLeading;
      float textLeading = 0.0f;
      foreach (GraphicObjectData graphicObjectData in this.m_objects)
      {
        if (graphicObjectData.CurrentFont != null)
          textLeading = graphicObjectData.TextLeading;
      }
      return textLeading;
    }
    set => this.Objects.TextLeading = value;
  }

  private PathFigure CurrentPath
  {
    get => this.m_currentPath;
    set => this.m_currentPath = value;
  }

  private Matrix CTRM
  {
    get => this.Objects.Ctm;
    set => this.Objects.Ctm = value;
  }

  private Matrix TextLineMatrix
  {
    get => this.Objects.textLineMatrix;
    set => this.Objects.textLineMatrix = value;
  }

  private Matrix TextMatrix
  {
    get => this.Objects.textMatrix;
    set => this.Objects.textMatrix = value;
  }

  private Matrix DocumentMatrix
  {
    get => this.Objects.documentMatrix;
    set => this.Objects.documentMatrix = value;
  }

  private Matrix TextMatrixUpdate
  {
    get => this.Objects.textMatrixUpdate;
    set => this.Objects.textMatrixUpdate = value;
  }

  private System.Drawing.Drawing2D.Matrix Drawing2dMatrixCTM
  {
    get => this.Objects.drawing2dMatrixCTM;
    set => this.Objects.drawing2dMatrixCTM = value;
  }

  private float HorizontalScaling
  {
    get => this.Objects.HorizontalScaling;
    set => this.Objects.HorizontalScaling = value;
  }

  private int Rise
  {
    get => this.Objects.Rise;
    set => this.Objects.Rise = value;
  }

  private Matrix TransformMatrixTM
  {
    get => this.Objects.transformMatrixTM;
    set => this.Objects.transformMatrixTM = value;
  }

  private System.Drawing.Drawing2D.Matrix SetMatrix(
    double a,
    double b,
    double c,
    double d,
    double e,
    double f)
  {
    if (this.m_isType3Font && this.m_isTrEntry || this.isWinAnsiEncoding && this.m_isType3Font)
    {
      if (e <= 0.0 || e > 1.0 || this.m_d1Matrix.OffsetY > 0.0)
      {
        e = Math.Round(this.m_d1Matrix.OffsetX / this.m_d1Matrix.M11);
        if (double.IsInfinity(e))
          e = Math.Round(this.m_d1Matrix.M11 / this.m_d1Matrix.OffsetX);
      }
      if (this.m_d1Matrix.OffsetY > 0.0)
        f /= this.m_d1Matrix.OffsetY;
      else if (this.m_d1Matrix.OffsetY < 0.0 && f % this.m_d1Matrix.OffsetY == 0.0)
        f = -(f / this.m_d1Matrix.OffsetY);
    }
    this.CTRM = new Matrix(a, b, c, d, e, f) * this.m_objects.ToArray()[0].Ctm;
    return new System.Drawing.Drawing2D.Matrix((float) this.CTRM.M11, (float) this.CTRM.M12, (float) this.CTRM.M21, (float) this.CTRM.M22, (float) this.CTRM.OffsetX, (float) this.CTRM.OffsetY);
  }

  private void SetTextMatrix(double a, double b, double c, double d, double e, double f)
  {
    this.TextLineMatrix = this.TextMatrix = new Matrix()
    {
      M11 = a,
      M12 = b,
      M21 = c,
      M22 = d,
      OffsetX = e,
      OffsetY = f
    };
  }

  private void MoveToNextLineWithCurrentTextLeading()
  {
    this.MoveToNextLine(0.0, (double) this.TextLeading);
  }

  private Matrix GetTextRenderingMatrix(bool isPath)
  {
    return new Matrix()
    {
      M11 = (double) this.FontSize * ((double) this.HorizontalScaling / 100.0),
      M22 = (isPath ? (double) this.FontSize : -(double) this.FontSize),
      OffsetY = (isPath ? (double) this.Rise : (double) this.FontSize + (double) this.Rise)
    } * this.TextLineMatrix * this.CTRM;
  }

  private Matrix GetTextRenderingMatrix()
  {
    return new Matrix()
    {
      M11 = (double) this.FontSize * ((double) this.HorizontalScaling / 100.0),
      M22 = -(double) this.FontSize,
      OffsetY = ((double) this.FontSize + (double) this.Rise)
    } * this.CTRM;
  }

  private void MoveToNextLine(double tx, double ty)
  {
    this.TextLineMatrix = this.TextMatrix = new Matrix()
    {
      M11 = 1.0,
      M12 = 0.0,
      OffsetX = tx,
      M21 = 0.0,
      M22 = 1.0,
      OffsetY = ty
    } * this.TextLineMatrix;
  }

  private float FloatParse(string textString)
  {
    return float.Parse(textString, (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat);
  }

  public void RenderAsImage()
  {
    if (!this.m_selectablePrintDocument)
    {
      try
      {
        bool flag1 = false;
        PdfRecordCollection recordCollection = !this.m_isType3Font ? this.m_contentElements : this.m_type3RecordCollection;
        if (recordCollection == null)
          return;
        for (int index1 = 0; index1 < recordCollection.RecordCollection.Count; ++index1)
        {
          bool IsCircle = false;
          bool IsRectangle = false;
          string RectangleWidth = (string) null;
          int num1 = 0;
          int index2 = 0;
          int index3 = 0;
          string str1 = (string) null;
          string tokenType = recordCollection.RecordCollection[index1].OperatorName;
          string[] operands = recordCollection.RecordCollection[index1].Operands;
          foreach (char symbolChar in this.m_symbolChars)
          {
            if (tokenType.Contains(symbolChar.ToString()))
              tokenType = tokenType.Replace(symbolChar.ToString(), "");
          }
          if (tokenType == "scn")
          {
            num1 = 0;
            foreach (string str2 in operands)
              ++num1;
            if (num1 == 1)
            {
              for (index2 = index1; index2 < recordCollection.RecordCollection.Count; ++index2)
              {
                if (recordCollection.RecordCollection[index2].OperatorName == "f")
                {
                  for (index3 = index1; index3 < index2; ++index3)
                  {
                    if (recordCollection.RecordCollection[index3].OperatorName == "re")
                    {
                      RectangleWidth = recordCollection.RecordCollection[index3].Operands[2];
                      IsRectangle = true;
                      break;
                    }
                    if (recordCollection.RecordCollection[index3].OperatorName == "c")
                    {
                      str1 = recordCollection.RecordCollection[index3].Operands[2];
                      IsCircle = true;
                      break;
                    }
                  }
                  break;
                }
              }
            }
          }
          PointF currentLocation;
          switch (tokenType.Trim())
          {
            case "BDC":
              if (operands.Length > 1)
              {
                string key = operands[1].Replace("/", "");
                if (this.m_layersVisibilityDictionary.ContainsKey(key) && !this.m_layersVisibilityDictionary[key])
                  this.m_skipRendering = true;
                if (this.m_skipRendering)
                {
                  ++this.m_inlayersCount;
                  break;
                }
                break;
              }
              break;
            case "EMC":
              if (this.m_inlayersCount > 0)
                --this.m_inlayersCount;
              if (this.m_inlayersCount <= 0)
              {
                this.m_skipRendering = false;
                break;
              }
              break;
            case "q":
              GraphicObjectData graphicObjectData1 = new GraphicObjectData();
              if (this.m_objects.Count > 0)
              {
                GraphicObjectData graphicObjectData2 = this.m_objects.ToArray()[0];
                graphicObjectData1.Ctm = graphicObjectData2.Ctm;
                graphicObjectData1.m_mitterLength = graphicObjectData2.m_mitterLength;
                graphicObjectData1.textLineMatrix = graphicObjectData2.textLineMatrix;
                graphicObjectData1.documentMatrix = graphicObjectData2.documentMatrix;
                graphicObjectData1.textMatrixUpdate = graphicObjectData2.textMatrixUpdate;
                graphicObjectData1.drawing2dMatrixCTM = graphicObjectData2.drawing2dMatrixCTM;
                graphicObjectData1.HorizontalScaling = graphicObjectData2.HorizontalScaling;
                graphicObjectData1.Rise = graphicObjectData2.Rise;
                graphicObjectData1.transformMatrixTM = graphicObjectData2.transformMatrixTM;
                graphicObjectData1.CharacterSpacing = graphicObjectData2.CharacterSpacing;
                graphicObjectData1.WordSpacing = graphicObjectData2.WordSpacing;
                graphicObjectData1.m_nonStrokingOpacity = graphicObjectData2.m_nonStrokingOpacity;
                graphicObjectData1.m_strokingOpacity = graphicObjectData2.m_strokingOpacity;
                graphicObjectData1.StrokingColorspace = graphicObjectData2.StrokingColorspace;
              }
              if (this.isXGraphics)
                ++this.xobjectGraphicsCount;
              this.m_objects.Push(graphicObjectData1);
              this.m_graphicsState.Push(this.PageGraphics.Save());
              break;
            case "Q":
              if (this.isXGraphics)
                --this.xobjectGraphicsCount;
              this.m_objects.Pop();
              if (this.m_graphicsState.Count > 0)
                this.PageGraphics.Restore(this.m_graphicsState.Pop());
              this.m_graphicspathtoclip = (GraphicsPath) null;
              this.IsGraphicsState = false;
              this.IsTransparentText = false;
              flag1 = false;
              break;
            case "Tr":
              this.RenderingMode = int.Parse(operands[0]);
              if (!this.IsPdfium)
              {
                if ((double) float.Parse(operands[0]) == 3.0)
                {
                  this.transperentNonStrokingBrush = this.Objects.NonStrokingBrush;
                  this.transperentStrokingBrush = this.Objects.StrokingBrush;
                  this.Objects.NonStrokingBrush = new Pen(Color.Transparent).Brush;
                  this.Objects.StrokingBrush = new Pen(Color.Transparent).Brush;
                  flag1 = true;
                  break;
                }
                if (this.Objects.NonStrokingBrush != null && !(this.Objects.NonStrokingBrush is TextureBrush) && !(this.Objects.NonStrokingBrush is LinearGradientBrush) && (new Pen(this.Objects.NonStrokingBrush).Color == Color.Transparent || new Pen(this.Objects.NonStrokingBrush).Color == Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue)))
                  this.Objects.NonStrokingBrush = this.transperentNonStrokingBrush != null ? this.transperentNonStrokingBrush : new Pen(Color.Black).Brush;
                if (this.Objects.StrokingBrush != null && !(this.Objects.StrokingBrush is TextureBrush) && !(this.Objects.StrokingBrush is LinearGradientBrush) && (new Pen(this.Objects.StrokingBrush).Color == Color.Transparent || new Pen(this.Objects.StrokingBrush).Color == Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue)))
                {
                  this.Objects.StrokingBrush = this.transperentStrokingBrush != null ? this.transperentStrokingBrush : new Pen(Color.Black).Brush;
                  break;
                }
                break;
              }
              break;
            case "Tm":
              float num2 = this.FloatParse(operands[0]);
              float b = this.FloatParse(operands[1]);
              float c = this.FloatParse(operands[2]);
              float num3 = this.FloatParse(operands[3]);
              float num4 = this.FloatParse(operands[4]);
              float f = this.FloatParse(operands[5]);
              this.SetTextMatrix((double) num2, (double) b, (double) c, (double) num3, (double) num4, (double) f);
              if (this.textMatrix)
                this.PageGraphics.Restore(this.m_graphicsState.Pop());
              this.m_graphicsState.Push(this.PageGraphics.Save());
              this.PageGraphics.MultiplyTransform(new System.Drawing.Drawing2D.Matrix(num2, -b, -c, num3, num4, -f));
              this.CurrentLocation = (PointF) new Point(0.0, 0.0);
              this.textMatrix = true;
              break;
            case "cm":
              float num5 = this.FloatParse(operands[0]);
              float num6 = this.FloatParse(operands[1]);
              float num7 = this.FloatParse(operands[2]);
              float num8 = this.FloatParse(operands[3]);
              float num9 = this.FloatParse(operands[4]);
              float num10 = this.FloatParse(operands[5]);
              this.Drawing2dMatrixCTM = this.SetMatrix((double) num5, (double) num6, (double) num7, (double) num8, (double) num9, (double) num10);
              this.m_imageCommonMatrix = new System.Drawing.Drawing2D.Matrix(num5, num6, num7, num8, num9, num10);
              break;
            case "BT":
              this.TextLineMatrix = this.TextMatrix = Matrix.Identity;
              this.m_beginText = true;
              this.CurrentLocation = PointF.Empty;
              break;
            case "ET":
              this.CurrentLocation = PointF.Empty;
              if (this.isScaledText)
              {
                this.isScaledText = false;
                this.PageGraphics.Restore(this.m_graphicsState.Pop());
              }
              if (this.textMatrix)
              {
                this.PageGraphics.Restore(this.m_graphicsState.Pop());
                this.textMatrix = false;
              }
              if (this.RenderingMode != 3 && this.RenderingMode == 2 && recordCollection.RecordCollection.Count > index1 + 1 && recordCollection.RecordCollection[index1 + 1].OperatorName != "q")
              {
                this.RenderingMode = 0;
                break;
              }
              break;
            case "T*":
              if (!this.m_skipRendering)
              {
                this.MoveToNextLineWithCurrentTextLeading();
                this.DrawNewLine();
                break;
              }
              break;
            case "TJ":
              if (!this.m_skipRendering && (double) this.FontSize != 0.0)
              {
                this.RenderTextElementWithSpacing(operands, tokenType);
                break;
              }
              break;
            case "Tj":
              if (!this.m_skipRendering)
              {
                this.RenderTextElement(operands, tokenType);
                break;
              }
              break;
            case "'":
              this.MoveToNextLineWithCurrentTextLeading();
              this.TextMatrixUpdate = this.GetTextRenderingMatrix(false);
              Matrix documentMatrix = this.DocumentMatrix;
              if ((double) this.TextScaling != 100.0)
              {
                this.m_graphicsState.Push(this.PageGraphics.Save());
                this.PageGraphics.ScaleTransform(this.TextScaling / 100f, 1f);
                this.isScaledText = true;
                currentLocation = this.CurrentLocation;
                double x = (double) currentLocation.X / ((double) this.TextScaling / 100.0);
                currentLocation = this.CurrentLocation;
                double y = (double) currentLocation.Y;
                this.CurrentLocation = new PointF((float) x, (float) y);
              }
              this.RenderTextElementWithLeading(operands, tokenType);
              break;
            case "Tf":
              this.RenderFont(operands);
              break;
            case "TD":
              currentLocation = this.CurrentLocation;
              double x1 = (double) currentLocation.X + (double) this.FloatParse(operands[0]);
              currentLocation = this.CurrentLocation;
              double y1 = (double) currentLocation.Y - (double) this.FloatParse(operands[1]);
              this.CurrentLocation = new PointF((float) x1, (float) y1);
              this.MoveToNextLineWithLeading(operands);
              break;
            case "Td":
              currentLocation = this.CurrentLocation;
              double x2 = (double) currentLocation.X + (double) this.FloatParse(operands[0]);
              currentLocation = this.CurrentLocation;
              double y2 = (double) currentLocation.Y - (double) this.FloatParse(operands[1]);
              this.CurrentLocation = new PointF((float) x2, (float) y2);
              this.MoveToNextLine((double) this.FloatParse(operands[0]), (double) this.FloatParse(operands[1]));
              break;
            case "TL":
              this.SetTextLeading(this.FloatParse(operands[0]));
              break;
            case "Tw":
              this.GetWordSpacing(operands);
              break;
            case "Tc":
              this.GetCharacterSpacing(operands);
              break;
            case "Tz":
              this.GetScalingFactor(operands);
              break;
            case "RG":
              if (!this.IsPdfium)
              {
                this.m_opacity = this.NonStrokingOpacity;
                if ((double) this.m_opacity == 0.0 && this.IsGraphicsState)
                  this.IsTransparentText = true;
                this.SetNonStrokingRGBColor(this.GetColor(operands, "nonstroking", "RGB"));
                this.m_backupColorElements = operands;
                this.m_backupColorSpace = "RGB";
                break;
              }
              break;
            case "SC":
            case "SCN":
              if (!this.IsPdfium)
              {
                this.m_opacity = this.NonStrokingOpacity;
                if ((double) this.m_opacity == 0.0 && this.IsGraphicsState)
                  this.IsTransparentText = true;
                if (this.Objects.NonStrokingColorspace != null)
                {
                  this.SetNonStrokingBrush(this.Objects.NonStrokingColorspace.GetBrush(operands, this.m_resources));
                  break;
                }
                if ((tokenType == "SCN" || tokenType == "SC") && operands.Length == 4)
                {
                  this.SetNonStrokingCMYKColor(this.GetColor(operands, "stroking", "DeviceCMYK"));
                  break;
                }
                if ((tokenType == "SCN" || tokenType == "SC") && operands.Length == 1)
                {
                  this.SetNonStrokingGrayColor(this.GetColor(operands, "stroking", "Gray"));
                  break;
                }
                if (!flag1)
                {
                  this.SetNonStrokingRGBColor(this.GetColor(operands, "stroking", "RGB"));
                  break;
                }
                break;
              }
              break;
            case "cs":
              if (!this.IsPdfium)
              {
                this.m_opacity = this.StrokingOpacity;
                if ((double) this.m_opacity == 0.0 && this.IsGraphicsState)
                  this.IsTransparentText = true;
                this.SetStrokingColorspace(this.GetColorspace(operands));
                break;
              }
              break;
            case "CS":
              if (!this.IsPdfium)
              {
                this.m_opacity = this.NonStrokingOpacity;
                if ((double) this.m_opacity == 0.0 && this.IsGraphicsState)
                  this.IsTransparentText = true;
                this.SetNonStrokingColorspace(this.GetColorspace(operands));
                break;
              }
              break;
            case "k":
              if (!this.IsPdfium)
              {
                this.m_opacity = this.StrokingOpacity;
                if ((double) this.m_opacity == 0.0 && this.IsGraphicsState)
                  this.IsTransparentText = true;
                this.SetStrokingCMYKColor(this.GetColor(operands, "stroking", "DeviceCMYK"));
                this.m_backupColorElements = operands;
                this.m_backupColorSpace = "DeviceCMYK";
                break;
              }
              break;
            case "K":
              if (!this.IsPdfium)
              {
                this.m_opacity = this.NonStrokingOpacity;
                if ((double) this.m_opacity == 0.0 && this.IsGraphicsState)
                  this.IsTransparentText = true;
                this.SetNonStrokingCMYKColor(this.GetColor(operands, "nonstroking", "DeviceCMYK"));
                this.m_backupColorElements = operands;
                this.m_backupColorSpace = "DeviceCMYK";
                break;
              }
              break;
            case "rg":
              if (!this.IsPdfium)
              {
                this.m_opacity = this.StrokingOpacity;
                if ((double) this.m_opacity == 0.0 && this.IsGraphicsState)
                  this.IsTransparentText = true;
                if (!flag1)
                  this.SetStrokingRGBColor(this.GetColor(operands, "stroking", "RGB"));
                this.m_backupColorElements = operands;
                this.m_backupColorSpace = "RGB";
                break;
              }
              break;
            case "sc":
            case "scn":
              if (!this.IsPdfium)
              {
                this.m_opacity = this.StrokingOpacity;
                if ((double) this.m_opacity == 0.0 && this.IsGraphicsState)
                  this.IsTransparentText = true;
                if (this.Objects.StrokingColorspace != null && num1 == 1 && index2 < recordCollection.RecordCollection.Count && index3 < recordCollection.RecordCollection.Count && recordCollection.RecordCollection[index2].OperatorName == "f" && (recordCollection.RecordCollection[index3].OperatorName == "re" || recordCollection.RecordCollection[index3].OperatorName == "c") && (RectangleWidth != null || str1 != null))
                  this.Objects.StrokingColorspace.SetOperatorValues(IsRectangle, IsCircle, RectangleWidth);
                if (this.Objects.StrokingColorspace != null)
                {
                  this.m_tilingType = 0;
                  this.SetStrokingBrush(this.Objects.StrokingColorspace.GetBrush(operands, this.m_resources));
                  if (this.Objects.StrokingColorspace is Syncfusion.Pdf.Parsing.Pattern)
                  {
                    Syncfusion.Pdf.Parsing.Pattern strokingColorspace = this.Objects.StrokingColorspace as Syncfusion.Pdf.Parsing.Pattern;
                    if (strokingColorspace.Type is TilingPattern)
                    {
                      this.m_tilingType = (strokingColorspace.Type as TilingPattern).TilingType;
                      break;
                    }
                    break;
                  }
                  break;
                }
                if ((tokenType == "scn" || tokenType == "sc") && operands.Length == 4)
                {
                  this.SetStrokingCMYKColor(this.GetColor(operands, "stroking", "DeviceCMYK"));
                  break;
                }
                if ((tokenType == "sc" || tokenType == "scn") && operands.Length == 1)
                {
                  this.SetStrokingGrayColor(this.GetColor(operands, "stroking", "Gray"));
                  break;
                }
                if (!flag1)
                {
                  this.SetStrokingRGBColor(this.GetColor(operands, "stroking", "RGB"));
                  break;
                }
                break;
              }
              break;
            case "g":
              if (!this.IsPdfium)
              {
                this.m_opacity = this.StrokingOpacity;
                if ((double) this.m_opacity == 0.0 && this.IsGraphicsState)
                  this.IsTransparentText = true;
                this.SetStrokingGrayColor(this.GetColor(operands, "stroking", "Gray"));
                this.m_backupColorElements = operands;
                this.m_backupColorSpace = "Gray";
                break;
              }
              break;
            case "G":
              this.m_opacity = this.NonStrokingOpacity;
              if (!this.IsPdfium)
              {
                if ((double) this.m_opacity == 0.0 && this.IsGraphicsState)
                  this.IsTransparentText = true;
                this.SetNonStrokingGrayColor(this.GetColor(operands, "nonstroking", "Gray"));
                this.m_backupColorElements = operands;
                this.m_backupColorSpace = "Gray";
                break;
              }
              break;
            case "Do":
              if (!this.m_skipRendering)
              {
                if (this.m_isType3Font)
                {
                  this.GetType3XObject(operands);
                  break;
                }
                this.GetXObject(operands);
                break;
              }
              break;
            case "re":
              if (!this.m_skipRendering && !this.IsPdfium)
              {
                if (this.RenderingMode == 3 && this.transperentStrokingBrush != null && this.transperentStrokingBrush != null)
                {
                  this.Objects.NonStrokingBrush = this.transperentNonStrokingBrush;
                  this.Objects.StrokingBrush = this.transperentStrokingBrush;
                }
                if (index1 < recordCollection.RecordCollection.Count && recordCollection.RecordCollection[index1 + 1].OperatorName == "f")
                  this.isNextFill = true;
                if ((double) this.Drawing2dMatrixCTM.Elements[0] != 0.0 || (double) this.Drawing2dMatrixCTM.Elements[2] != 0.0 || !this.isNextFill)
                {
                  this.GetClipRectangle(operands);
                  break;
                }
                break;
              }
              break;
            case "d":
              if (operands[0] != "[]" && !operands[0].Contains("\n"))
              {
                this.m_dashedLine = operands;
                break;
              }
              break;
            case "d0":
              this.m_d0Matrix = new Matrixx((double) this.FloatParse(operands[0]), (double) this.FloatParse(operands[1]));
              break;
            case "d1":
              this.m_d1Matrix = new Matrix((double) this.FloatParse(operands[0]), (double) this.FloatParse(operands[1]), (double) this.FloatParse(operands[2]), (double) this.FloatParse(operands[3]), (double) this.FloatParse(operands[4]), (double) this.FloatParse(operands[5]));
              break;
            case "gs":
              if (!this.IsPdfium)
              {
                this.IsTransparentText = false;
                if (this.m_resources.ContainsKey(operands[0].Substring(1)))
                {
                  int num11 = 0;
                  string str3 = (string) null;
                  bool flag2 = true;
                  PdfDictionary xobjectDictionary = (this.m_resources[operands[0].Substring(1)] as XObjectElement).XObjectDictionary;
                  if (xobjectDictionary.ContainsKey("OPM"))
                    num11 = (xobjectDictionary["OPM"] as PdfNumber).IntValue;
                  if (xobjectDictionary.ContainsKey("SMask"))
                    this.m_isExtendedGraphicStateContainsSMask = true;
                  if (xobjectDictionary.ContainsKey("AIS"))
                    flag2 = (xobjectDictionary["AIS"] as PdfBoolean).Value;
                  bool flag3 = false;
                  if ((double) this.NonStrokingOpacity == 0.0 || (double) this.StrokingOpacity == 0.0)
                    flag3 = true;
                  if (xobjectDictionary.ContainsKey("HT"))
                  {
                    PdfName pdfName1 = xobjectDictionary["HT"] as PdfName;
                    if (pdfName1 != (PdfName) null)
                    {
                      str3 = pdfName1.Value;
                    }
                    else
                    {
                      PdfName pdfName2 = (xobjectDictionary["HT"] as PdfReferenceHolder).Object as PdfName;
                      if (pdfName2 != (PdfName) null)
                        str3 = pdfName2.Value;
                    }
                  }
                  else if (xobjectDictionary.ContainsKey("CA") || xobjectDictionary.ContainsKey("ca"))
                  {
                    if (xobjectDictionary.ContainsKey("CA"))
                      this.NonStrokingOpacity = (xobjectDictionary["CA"] as PdfNumber).FloatValue;
                    if (xobjectDictionary.ContainsKey("ca"))
                    {
                      if (flag2)
                        this.StrokingOpacity = (xobjectDictionary["ca"] as PdfNumber).FloatValue;
                      else if (!this.isXGraphics)
                        this.StrokingOpacity = (xobjectDictionary["ca"] as PdfNumber).FloatValue;
                    }
                    if ((double) this.NonStrokingOpacity == 0.0 || (double) this.StrokingOpacity == 0.0)
                      flag3 = true;
                    if (flag3)
                    {
                      if (this.StrokingBrush != null && this.m_backupColorElements != null && this.m_backupColorSpace != null)
                      {
                        this.m_opacity = this.StrokingOpacity;
                        this.SetStrokingColor(this.GetColor(this.m_backupColorElements, "Stroking", this.m_backupColorSpace));
                      }
                      if (this.NonStrokingBrush != null && this.m_backupColorElements != null && this.m_backupColorSpace != null)
                      {
                        this.m_opacity = this.NonStrokingOpacity;
                        this.SetNonStrokingColor(this.GetColor(this.m_backupColorElements, "NonStroking", this.m_backupColorSpace));
                      }
                    }
                    this.IsGraphicsState = true;
                  }
                  else if (xobjectDictionary.ContainsKey("TR"))
                  {
                    if (xobjectDictionary["TR"].ToString().Replace("/", "") == "Identity" && num11 == 1)
                      this.m_isTrEntry = true;
                  }
                  else if (!xobjectDictionary.ContainsKey("TR") && num11 == 1 && xobjectDictionary.ContainsKey("Type") && xobjectDictionary["Type"].ToString().Replace("/", "") == "ExtGState" && num11 == 1)
                    this.m_isTrEntry = true;
                  if (num11 == 1 && str3 == "Default")
                  {
                    this.IsExtendedGraphicsState = true;
                    break;
                  }
                  break;
                }
                break;
              }
              break;
            case "b":
            case "b*":
              if (!this.m_skipRendering)
              {
                this.m_currentPath.IsClosed = true;
                Matrix matrix1 = new Matrix((double) this.Drawing2dMatrixCTM.Elements[0], (double) this.Drawing2dMatrixCTM.Elements[1], (double) this.Drawing2dMatrixCTM.Elements[2], (double) this.Drawing2dMatrixCTM.Elements[3], (double) this.Drawing2dMatrixCTM.OffsetX, (double) this.Drawing2dMatrixCTM.OffsetY) * this.DocumentMatrix;
                System.Drawing.Drawing2D.Matrix matrix2 = new System.Drawing.Drawing2D.Matrix((float) matrix1.M11, (float) matrix1.M12, (float) matrix1.M21, (float) matrix1.M22, (float) matrix1.OffsetX, (float) matrix1.OffsetY);
                System.Drawing.Drawing2D.Matrix matrix3 = new System.Drawing.Drawing2D.Matrix();
                matrix3.Multiply(matrix2);
                System.Drawing.Drawing2D.Matrix transform = this.PageGraphics.Transform;
                int pageUnit = (int) this.PageGraphics.PageUnit;
                this.PageGraphics.PageUnit = GraphicsUnit.Pixel;
                this.PageGraphics.Transform = new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
                this.PageGraphics.Transform = matrix3;
                GraphicsPath geometry = this.GetGeometry(this.CurrentGeometry, new Matrix(1.0, 0.0, 0.0, 1.0, 0.0, 0.0));
                Pen pen = new Pen(this.NonStrokingBrush == null ? new Pen(Color.Black).Brush : this.NonStrokingBrush);
                pen.Width = this.MitterLength;
                geometry.FillMode = !(tokenType.Trim() == "b") ? FillMode.Alternate : FillMode.Winding;
                if (this.StrokingBrush != null)
                  this.PageGraphics.FillPath(new Pen(this.StrokingBrush).Brush, geometry);
                if (geometry.FillMode == FillMode.Alternate)
                  this.FillPath("Alternate");
                else
                  this.FillPath("Winding");
                this.PageGraphics.DrawPath(pen, geometry);
                this.ClipRectangle = (RectangleF) Rectangle.Empty;
                this.CurrentGeometry = new PathGeometry();
                this.m_currentPath = (PathFigure) null;
                break;
              }
              break;
            case "B":
            case "B*":
              if (!this.m_skipRendering)
              {
                Pen pen = this.NonStrokingBrush == null ? new Pen(Color.Black) : new Pen(this.NonStrokingBrush);
                pen.Width = this.MitterLength;
                Matrix matrix4 = new Matrix((double) this.Drawing2dMatrixCTM.Elements[0], (double) this.Drawing2dMatrixCTM.Elements[1], (double) this.Drawing2dMatrixCTM.Elements[2], (double) this.Drawing2dMatrixCTM.Elements[3], (double) this.Drawing2dMatrixCTM.OffsetX, (double) this.Drawing2dMatrixCTM.OffsetY) * this.DocumentMatrix;
                System.Drawing.Drawing2D.Matrix matrix5 = new System.Drawing.Drawing2D.Matrix((float) matrix4.M11, (float) matrix4.M12, (float) matrix4.M21, (float) matrix4.M22, (float) matrix4.OffsetX, (float) matrix4.OffsetY);
                System.Drawing.Drawing2D.Matrix matrix6 = new System.Drawing.Drawing2D.Matrix();
                matrix6.Multiply(matrix5);
                System.Drawing.Drawing2D.Matrix transform = this.PageGraphics.Transform;
                GraphicsUnit pageUnit = this.PageGraphics.PageUnit;
                this.PageGraphics.PageUnit = GraphicsUnit.Pixel;
                this.PageGraphics.Transform = new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
                this.PageGraphics.Transform = matrix6;
                System.Drawing.Drawing2D.Matrix matrix7 = new System.Drawing.Drawing2D.Matrix();
                GraphicsPath geometry = this.GetGeometry(this.CurrentGeometry, new Matrix(1.0, 0.0, 0.0, 1.0, 0.0, 0.0));
                geometry.FillMode = !(tokenType.Trim() == "B") ? FillMode.Alternate : FillMode.Winding;
                if (this.StrokingBrush != null)
                  this.PageGraphics.FillPath(new Pen(this.StrokingBrush).Brush, geometry);
                else
                  this.PageGraphics.FillPath(new Pen(Color.Black).Brush, geometry);
                this.PageGraphics.DrawPath(pen, geometry);
                this.PageGraphics.PageUnit = pageUnit;
                this.PageGraphics.Transform = transform;
                this.CurrentGeometry = new PathGeometry();
                this.m_currentPath = (PathFigure) null;
                break;
              }
              break;
            case "n":
              this.BackupCurrentGeometry = this.CurrentGeometry;
              this.CurrentGeometry = new PathGeometry();
              this.m_currentPath = (PathFigure) null;
              break;
            case "J":
              this.m_lineCap = this.FloatParse(operands[0]);
              break;
            case "w":
              this.MitterLength = this.FloatParse(operands[0]);
              break;
            case "W":
              if (!this.m_isType3Font)
              {
                this.m_clippingPath = tokenType;
                Matrix transform1 = new Matrix((double) this.Drawing2dMatrixCTM.Elements[0], (double) this.Drawing2dMatrixCTM.Elements[1], (double) this.Drawing2dMatrixCTM.Elements[2], (double) this.Drawing2dMatrixCTM.Elements[3], (double) this.Drawing2dMatrixCTM.OffsetX, (double) this.Drawing2dMatrixCTM.OffsetY) * this.DocumentMatrix;
                System.Drawing.Drawing2D.Matrix matrix = new System.Drawing.Drawing2D.Matrix((float) Math.Round(transform1.M11, 5, MidpointRounding.ToEven), (float) Math.Round(transform1.M12, 5, MidpointRounding.ToEven), (float) Math.Round(transform1.M21, 5, MidpointRounding.ToEven), (float) Math.Round(transform1.M22, 5, MidpointRounding.ToEven), (float) Math.Round(transform1.OffsetX, 5, MidpointRounding.ToEven), (float) Math.Round(transform1.OffsetY, 5, MidpointRounding.ToEven));
                System.Drawing.Drawing2D.Matrix transform2 = this.PageGraphics.Transform;
                int pageUnit = (int) this.PageGraphics.PageUnit;
                this.PageGraphics.PageUnit = GraphicsUnit.Pixel;
                this.PageGraphics.Transform = new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
                foreach (PathFigure figure in this.CurrentGeometry.Figures)
                {
                  figure.IsClosed = true;
                  figure.IsFilled = true;
                }
                this.CurrentGeometry.FillRule = FillRule.Nonzero;
                GraphicsPath geometry = this.GetGeometry(this.CurrentGeometry, transform1);
                if (geometry.PointCount != 0 && !this.m_isNegative)
                  this.PageGraphics.SetClip(geometry, CombineMode.Intersect);
                this.m_isNegative = false;
                break;
              }
              break;
            case "W*":
              this.m_clippingPath = tokenType;
              Matrix transform3 = new Matrix((double) this.Drawing2dMatrixCTM.Elements[0], (double) this.Drawing2dMatrixCTM.Elements[1], (double) this.Drawing2dMatrixCTM.Elements[2], (double) this.Drawing2dMatrixCTM.Elements[3], (double) this.Drawing2dMatrixCTM.OffsetX, (double) this.Drawing2dMatrixCTM.OffsetY) * this.DocumentMatrix;
              System.Drawing.Drawing2D.Matrix matrix8 = new System.Drawing.Drawing2D.Matrix((float) Math.Round(transform3.M11, 5, MidpointRounding.ToEven), (float) Math.Round(transform3.M12, 5, MidpointRounding.ToEven), (float) Math.Round(transform3.M21, 5, MidpointRounding.ToEven), (float) Math.Round(transform3.M22, 5, MidpointRounding.ToEven), (float) Math.Round(transform3.OffsetX, 5, MidpointRounding.ToEven), (float) Math.Round(transform3.OffsetY, 5, MidpointRounding.ToEven));
              System.Drawing.Drawing2D.Matrix transform4 = this.PageGraphics.Transform;
              int pageUnit1 = (int) this.PageGraphics.PageUnit;
              this.PageGraphics.PageUnit = GraphicsUnit.Pixel;
              this.PageGraphics.Transform = new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
              foreach (PathFigure figure in this.CurrentGeometry.Figures)
              {
                figure.IsClosed = true;
                figure.IsFilled = true;
              }
              this.CurrentGeometry.FillRule = FillRule.EvenOdd;
              GraphicsPath geometry1 = this.GetGeometry(this.CurrentGeometry, transform3);
              if (geometry1.PointCount != 0)
              {
                this.PageGraphics.SetClip(geometry1, CombineMode.Intersect);
                break;
              }
              break;
            case "s":
            case "S":
              if (!this.m_skipRendering)
              {
                if (tokenType.Trim() == "s" && this.m_currentPath != null)
                  this.m_currentPath.IsClosed = true;
                if (!this.IsPdfium)
                {
                  this.DrawPath();
                  break;
                }
                break;
              }
              break;
            case "f*":
              if (!this.m_skipRendering)
              {
                this.FillPath("Alternate");
                this.CurrentLocation = PointF.Empty;
                break;
              }
              break;
            case "f":
              if (!this.m_skipRendering)
              {
                if ((double) this.Drawing2dMatrixCTM.Elements[0] != 0.0 || (double) this.Drawing2dMatrixCTM.Elements[2] != 0.0)
                  this.FillPath("Winding");
                this.CurrentLocation = PointF.Empty;
                break;
              }
              break;
            case "v":
              this.AddBezierCurve2(operands);
              break;
            case "y":
              this.AddBezierCurve3(operands);
              break;
            case "h":
              if (this.m_currentPath != null)
              {
                this.m_currentPath.IsClosed = true;
                break;
              }
              break;
            case "m":
              if (!this.m_skipRendering)
              {
                this.BeginPath(operands);
                break;
              }
              break;
            case "c":
              if (!this.m_skipRendering)
              {
                this.AddBezierCurve(operands);
                break;
              }
              break;
            case "l":
              if (!this.m_skipRendering)
              {
                this.AddLine(operands);
                break;
              }
              break;
            case "ID":
              this.BIparameter = new List<string>();
              foreach (string str4 in operands)
                this.BIparameter.Add(str4);
              this.GetInlineImageParameters(this.BIparameter);
              break;
            case "EI":
              if (!this.IsTextSearch && !this.IsPdfium)
              {
                MemoryStream memoryStream = new MemoryStream(recordCollection.RecordCollection[index1].InlineImageBytes);
                ImageStructure imageStructure = new ImageStructure((IPdfPrimitive) this.m_inlineParameters, new PdfMatrix());
                imageStructure.ImageStream = (Stream) memoryStream;
                if (this.m_isType3Font)
                {
                  this.GetType3XObject(imageStructure);
                  break;
                }
                Image image = imageStructure.EmbeddedImage;
                System.Drawing.Drawing2D.Matrix transform5 = this.PageGraphics.Transform;
                GraphicsUnit pageUnit2 = this.PageGraphics.PageUnit;
                this.PageGraphics.PageUnit = GraphicsUnit.Pixel;
                this.PageGraphics.Transform = new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
                Matrix matrix9 = new Matrix((double) this.Drawing2dMatrixCTM.Elements[0], (double) this.Drawing2dMatrixCTM.Elements[1], (double) this.Drawing2dMatrixCTM.Elements[2], (double) this.Drawing2dMatrixCTM.Elements[3], (double) this.Drawing2dMatrixCTM.OffsetX, (double) this.Drawing2dMatrixCTM.OffsetY);
                matrix9.Scale(1.0, -1.01, 0.0, 1.0);
                Matrix matrix10 = matrix9 * this.DocumentMatrix;
                this.PageGraphics.Transform = new System.Drawing.Drawing2D.Matrix((float) matrix10.M11, (float) matrix10.M12, (float) matrix10.M21, (float) matrix10.M22, (float) matrix10.OffsetX, (float) Math.Round(matrix10.OffsetY, 5));
                if (imageStructure.IsImageMask)
                {
                  if (image == null)
                    image = this.GetMaskImagefromStream(imageStructure);
                  else if (!imageStructure.m_isBlackIs1)
                    image = this.GetMaskImage(image);
                }
                if (image != null)
                {
                  PixelOffsetMode pixelOffsetMode = this.PageGraphics.PixelOffsetMode;
                  InterpolationMode interpolationMode = this.PageGraphics.InterpolationMode;
                  this.PageGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                  this.PageGraphics.PixelOffsetMode = PixelOffsetMode.None;
                  using (ImageAttributes imageAttr = new ImageAttributes())
                  {
                    imageAttr.SetWrapMode(WrapMode.TileFlipXY);
                    this.PageGraphics.DrawImage(image, new Rectangle(0, 0, 1, 1), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttr);
                  }
                  this.PageGraphics.PixelOffsetMode = pixelOffsetMode;
                  this.PageGraphics.InterpolationMode = interpolationMode;
                }
                this.PageGraphics.Transform = transform5;
                this.PageGraphics.PageUnit = pageUnit2;
                break;
              }
              break;
            case "sh":
              if (!this.IsTextSearch && !this.IsPdfium)
              {
                RectangleF rectangleF = RectangleF.Empty;
                if (this.GetColorspace(operands) is ShadingPattern colorspace)
                {
                  colorspace.PatternMatrix = this.GetShadingPatternMatrix(colorspace.PatternMatrix);
                  this.Objects.StrokingBrush = colorspace.CreateBrush();
                }
                LinearGradientBrush strokingBrush = this.StrokingBrush as LinearGradientBrush;
                if (colorspace.ShadingType is AxialShading)
                {
                  AxialShading shadingType = colorspace.ShadingType as AxialShading;
                  if (shadingType.AlternateColorspace is DeviceN)
                    rectangleF = shadingType.ShadingRectangle;
                }
                if (strokingBrush != null)
                {
                  float x3;
                  float y3;
                  float num12;
                  float num13;
                  if (rectangleF != RectangleF.Empty)
                  {
                    x3 = rectangleF.X;
                    y3 = rectangleF.Y;
                    num12 = rectangleF.Width;
                    num13 = rectangleF.Height;
                  }
                  else
                  {
                    RectangleF rectangle = strokingBrush.Rectangle;
                    x3 = rectangle.X;
                    rectangle = strokingBrush.Rectangle;
                    y3 = rectangle.Y;
                    rectangle = strokingBrush.Rectangle;
                    num12 = rectangle.Width;
                    rectangle = strokingBrush.Rectangle;
                    num13 = rectangle.Height;
                  }
                  if (colorspace.BBox != null)
                  {
                    x3 = (colorspace.BBox[0] as PdfNumber).FloatValue + x3;
                    y3 = (colorspace.BBox[1] as PdfNumber).FloatValue + y3;
                    num12 = (colorspace.BBox[2] as PdfNumber).FloatValue + num12;
                    num13 = (colorspace.BBox[3] as PdfNumber).FloatValue + num13;
                  }
                  if (this.BackupCurrentGeometry != null && this.BackupCurrentGeometry.Figures.Count != 0 && colorspace.BBox == null && (double) this.m_imageCommonMatrix.Elements[0] < 0.0 && (double) this.m_imageCommonMatrix.Elements[5] < 0.0)
                  {
                    this.CurrentGeometry = this.BackupCurrentGeometry;
                  }
                  else
                  {
                    this.BeginPath(x3, y3);
                    this.AddLine(x3 + num12, y3);
                    this.AddLine(x3 + num12, y3 + num13);
                    this.AddLine(x3, y3 + num13);
                    this.EndPath();
                  }
                  this.FillPath("Winding");
                  strokingBrush.Dispose();
                  break;
                }
                break;
              }
              break;
          }
        }
        this.m_isType3Font = false;
      }
      catch (Exception ex)
      {
        if (ex.Message.Contains(" does not supported") || ex.Message.Contains("Error in identifying the ImageFilter"))
          this.exception.Exceptions.Append($"\r\n\r\n{ex.Message}\r\n");
        else
          this.exception.Exceptions.Append($"\r\n\r\n{ex.Message}{ex.StackTrace}\r\n");
      }
    }
    else
    {
      try
      {
        this.DocumentMatrix = new Matrix(1.3333333730697632, 0.0, 0.0, -1.3333333730697632, 0.0, (double) this.m_pageHeight);
        bool flag4 = false;
        PdfRecordCollection recordCollection = !this.m_isType3Font ? this.m_contentElements : this.m_type3RecordCollection;
        if (recordCollection == null)
          return;
        for (int index = 0; index < recordCollection.RecordCollection.Count; ++index)
        {
          string tokenType = recordCollection.RecordCollection[index].OperatorName;
          string[] operands = recordCollection.RecordCollection[index].Operands;
          foreach (char symbolChar in this.m_symbolChars)
          {
            if (tokenType.Contains(symbolChar.ToString()))
              tokenType = tokenType.Replace(symbolChar.ToString(), "");
          }
          PointF currentLocation;
          switch (tokenType.Trim())
          {
            case "q":
              GraphicObjectData graphicObjectData3 = new GraphicObjectData();
              if (this.m_objects.Count > 0)
              {
                GraphicObjectData graphicObjectData4 = this.m_objects.ToArray()[0];
                graphicObjectData3.Ctm = graphicObjectData4.Ctm;
                graphicObjectData3.textLineMatrix = graphicObjectData4.textLineMatrix;
                graphicObjectData3.documentMatrix = graphicObjectData4.documentMatrix;
                graphicObjectData3.textMatrixUpdate = graphicObjectData4.textMatrixUpdate;
                graphicObjectData3.drawing2dMatrixCTM = graphicObjectData4.drawing2dMatrixCTM;
                graphicObjectData3.HorizontalScaling = graphicObjectData4.HorizontalScaling;
                graphicObjectData3.Rise = graphicObjectData4.Rise;
                graphicObjectData3.transformMatrixTM = graphicObjectData4.transformMatrixTM;
                graphicObjectData3.CharacterSpacing = graphicObjectData4.CharacterSpacing;
                graphicObjectData3.WordSpacing = graphicObjectData4.WordSpacing;
                graphicObjectData3.m_nonStrokingOpacity = graphicObjectData4.m_nonStrokingOpacity;
                graphicObjectData3.m_strokingOpacity = graphicObjectData4.m_strokingOpacity;
              }
              this.m_objects.Push(graphicObjectData3);
              this.m_graphicsState.Push(this.PageGraphics.Save());
              break;
            case "Q":
              this.m_objects.Pop();
              this.PageGraphics.Restore(this.m_graphicsState.Pop());
              this.m_graphicspathtoclip = (GraphicsPath) null;
              this.IsGraphicsState = false;
              this.IsTransparentText = false;
              flag4 = false;
              break;
            case "Tr":
              this.RenderingMode = int.Parse(operands[0]);
              if ((double) float.Parse(operands[0]) == 3.0)
              {
                this.transperentNonStrokingBrush = this.Objects.NonStrokingBrush;
                this.transperentStrokingBrush = this.Objects.StrokingBrush;
                this.Objects.NonStrokingBrush = new Pen(Color.Transparent).Brush;
                this.Objects.StrokingBrush = new Pen(Color.Transparent).Brush;
                flag4 = true;
                break;
              }
              if (this.Objects.NonStrokingBrush != null && (new Pen(this.Objects.NonStrokingBrush).Color == Color.Transparent || new Pen(this.Objects.NonStrokingBrush).Color == Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue)))
                this.Objects.NonStrokingBrush = this.transperentNonStrokingBrush != null ? this.transperentNonStrokingBrush : new Pen(Color.Black).Brush;
              if (this.Objects.StrokingBrush != null && (new Pen(this.Objects.StrokingBrush).Color == Color.Transparent || new Pen(this.Objects.StrokingBrush).Color == Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue)))
              {
                this.Objects.StrokingBrush = this.transperentStrokingBrush != null ? this.transperentStrokingBrush : new Pen(Color.Black).Brush;
                break;
              }
              break;
            case "Tm":
              try
              {
                float num14 = float.Parse(operands[0]);
                float num15 = float.Parse(operands[1]);
                float num16 = float.Parse(operands[2]);
                float sy = float.Parse(operands[3]);
                float dx = float.Parse(operands[4]);
                float num17 = float.Parse(operands[5]);
                if (this.textMatrix)
                  this.PageGraphics.Restore(this.m_graphicsState.Pop());
                this.m_graphicsState.Push(this.PageGraphics.Save());
                this.PageGraphics.TranslateTransform(dx, -num17);
                if (((double) num14 != 1.0 || (double) sy != 1.0) && (double) num14 != 0.0 && (double) sy != 0.0)
                  this.PageGraphics.ScaleTransform(num14, sy);
                if ((double) num15 != 0.0 || (double) num16 != 0.0)
                {
                  double num18 = Math.Round(180.0 / Math.PI * Math.Acos((double) num14));
                  double d = Math.Round(180.0 / Math.PI * Math.Asin((double) num15));
                  if (num18 == d)
                    this.m_textAngle = -(float) num18;
                  else if (double.IsNaN(d))
                  {
                    this.m_textAngle = -(float) num18;
                    this.PageGraphics.ScaleTransform(num15, -num16);
                  }
                  else
                    this.m_textAngle = -(float) d;
                  if (!double.IsNaN((double) this.m_textAngle))
                    this.PageGraphics.RotateTransform(this.m_textAngle);
                }
                this.CurrentLocation = (PointF) new Point(0.0, 0.0);
                this.textMatrix = true;
                break;
              }
              catch (Exception ex)
              {
                this.exception.Exceptions.Append($"\r\n\r\n{ex.Message}{ex.StackTrace}\r\n");
                break;
              }
            case "cm":
              float num19 = float.Parse(operands[0]);
              float num20 = float.Parse(operands[1]);
              float c = float.Parse(operands[2]);
              float num21 = float.Parse(operands[3]);
              float num22 = float.Parse(operands[4]);
              float f = float.Parse(operands[5]);
              this.Drawing2dMatrixCTM = this.SetMatrix((double) num19, (double) num20, (double) c, (double) num21, (double) num22, (double) f);
              if (((double) num19 != 1.0 || (double) num21 != 1.0) && (double) num19 != 0.0 && (double) num21 != 0.0)
              {
                if ((double) num19 == 0.0 && (double) num21 == 0.0)
                {
                  this.PageGraphics.TranslateTransform(c + num22, (float) -((double) f + (double) num21));
                  this.PageGraphics.ScaleTransform(-c, num20);
                }
                else if ((double) num22 == 0.0 && (double) f == 0.0)
                {
                  if ((double) num20 == 0.0 && (double) c == 0.0)
                  {
                    this.PageGraphics.TranslateTransform(num22, (float) -((double) f + (double) num21));
                    this.PageGraphics.ScaleTransform(num19, num21);
                  }
                  else
                    this.PageGraphics.TranslateTransform(num22, (float) -((double) f + (double) num21));
                }
                else
                {
                  this.PageGraphics.TranslateTransform(num22, (float) -((double) f + (double) num21));
                  if ((double) num20 <= 0.0)
                    this.PageGraphics.ScaleTransform(num19, num21);
                }
                this.CurrentLocation = PointF.Empty;
              }
              else
              {
                this.PageGraphics.TranslateTransform(num22, -f);
                this.CurrentLocation = PointF.Empty;
              }
              if (((double) num20 != (double) c || (double) num22 != (double) f || (double) c != (double) num22 || (double) c != 0.0) && ((double) num19 != (double) num21 || (double) num21 != 1.0 || (double) num22 != (double) f || (double) num22 != 0.0))
              {
                double d1 = Math.Round(180.0 / Math.PI * Math.Acos((double) num19));
                double d2 = Math.Round(180.0 / Math.PI * Math.Asin((double) num20));
                if (d1 == d2)
                {
                  this.PageGraphics.RotateTransform(-(float) d1);
                  break;
                }
                if (!double.IsNaN(d2))
                {
                  this.PageGraphics.RotateTransform(-(float) d2);
                  break;
                }
                if (!double.IsNaN(d1))
                {
                  this.PageGraphics.RotateTransform(-(float) d1);
                  break;
                }
                break;
              }
              break;
            case "BT":
              this.TextLineMatrix = this.TextMatrix = Matrix.Identity;
              this.m_beginText = true;
              this.CurrentLocation = PointF.Empty;
              break;
            case "ET":
              this.CurrentLocation = PointF.Empty;
              if (this.isScaledText)
              {
                this.isScaledText = false;
                this.PageGraphics.Restore(this.m_graphicsState.Pop());
              }
              if (this.textMatrix)
              {
                this.PageGraphics.Restore(this.m_graphicsState.Pop());
                this.textMatrix = false;
              }
              this.RenderingMode = 0;
              break;
            case "T*":
              if (!this.m_skipRendering)
              {
                this.MoveToNextLineWithCurrentTextLeading();
                this.DrawNewLine();
                break;
              }
              break;
            case "TJ":
              if (!this.m_skipRendering)
              {
                this.RenderTextElementWithSpacingPrintTextasText(operands, tokenType);
                break;
              }
              break;
            case "Tj":
              if (!this.m_skipRendering)
              {
                this.RenderTextElementPrintTextasText(operands, tokenType);
                break;
              }
              break;
            case "'":
              this.RenderTextElementWithLeadingPrintTextasText(operands, tokenType);
              break;
            case "Tf":
              this.RenderFont(operands);
              break;
            case "TD":
              currentLocation = this.CurrentLocation;
              double x4 = (double) currentLocation.X + (double) float.Parse(operands[0]);
              currentLocation = this.CurrentLocation;
              double y4 = (double) currentLocation.Y - (double) float.Parse(operands[1]);
              this.CurrentLocation = new PointF((float) x4, (float) y4);
              this.MoveToNextLineWithLeading(operands);
              break;
            case "Td":
              currentLocation = this.CurrentLocation;
              double x5 = (double) currentLocation.X + (double) float.Parse(operands[0]);
              currentLocation = this.CurrentLocation;
              double y5 = (double) currentLocation.Y - (double) float.Parse(operands[1]);
              this.CurrentLocation = new PointF((float) x5, (float) y5);
              this.MoveToNextLine((double) float.Parse(operands[0]), (double) float.Parse(operands[1]));
              break;
            case "TL":
              this.SetTextLeading(float.Parse(operands[0]));
              break;
            case "Tw":
              this.GetWordSpacing(operands);
              break;
            case "Tc":
              this.GetCharacterSpacing(operands);
              break;
            case "Tz":
              this.GetScalingFactor(operands);
              break;
            case "RG":
              this.m_opacity = this.NonStrokingOpacity;
              if ((double) this.m_opacity == 0.0 && this.IsGraphicsState)
                this.IsTransparentText = true;
              this.SetNonStrokingRGBColor(this.GetColor(operands, "nonstroking", "RGB"));
              this.m_backupColorElements = operands;
              this.m_backupColorSpace = "RGB";
              break;
            case "SC":
            case "SCN":
              this.m_opacity = this.NonStrokingOpacity;
              if ((double) this.m_opacity == 0.0 && this.IsGraphicsState)
                this.IsTransparentText = true;
              if (this.Objects.NonStrokingColorspace != null)
              {
                this.SetNonStrokingBrush(this.Objects.NonStrokingColorspace.GetBrush(operands, this.m_resources));
                break;
              }
              if ((tokenType == "SCN" || tokenType == "SC") && operands.Length == 4)
              {
                this.SetNonStrokingCMYKColor(this.GetColor(operands, "stroking", "DeviceCMYK"));
                break;
              }
              if ((tokenType == "SCN" || tokenType == "SC") && operands.Length == 1)
              {
                this.SetNonStrokingGrayColor(this.GetColor(operands, "stroking", "Gray"));
                break;
              }
              if (!flag4)
              {
                this.SetNonStrokingRGBColor(this.GetColor(operands, "stroking", "RGB"));
                break;
              }
              break;
            case "cs":
              this.m_opacity = this.StrokingOpacity;
              if ((double) this.m_opacity == 0.0 && this.IsGraphicsState)
                this.IsTransparentText = true;
              this.SetStrokingColorspace(this.GetColorspace(operands));
              break;
            case "CS":
              this.m_opacity = this.NonStrokingOpacity;
              if ((double) this.m_opacity == 0.0 && this.IsGraphicsState)
                this.IsTransparentText = true;
              this.SetNonStrokingColorspace(this.GetColorspace(operands));
              break;
            case "k":
              this.m_opacity = this.StrokingOpacity;
              if ((double) this.m_opacity == 0.0 && this.IsGraphicsState)
                this.IsTransparentText = true;
              this.SetStrokingCMYKColor(this.GetColor(operands, "stroking", "DeviceCMYK"));
              this.m_backupColorElements = operands;
              this.m_backupColorSpace = "DeviceCMYK";
              break;
            case "K":
              this.m_opacity = this.NonStrokingOpacity;
              if ((double) this.m_opacity == 0.0 && this.IsGraphicsState)
                this.IsTransparentText = true;
              this.SetNonStrokingCMYKColor(this.GetColor(operands, "nonstroking", "DeviceCMYK"));
              this.m_backupColorElements = operands;
              this.m_backupColorSpace = "DeviceCMYK";
              break;
            case "rg":
              this.m_opacity = this.StrokingOpacity;
              if ((double) this.m_opacity == 0.0 && this.IsGraphicsState)
                this.IsTransparentText = true;
              if (!flag4)
                this.SetStrokingRGBColor(this.GetColor(operands, "stroking", "RGB"));
              this.m_backupColorElements = operands;
              this.m_backupColorSpace = "RGB";
              break;
            case "sc":
            case "scn":
              this.m_opacity = this.StrokingOpacity;
              if ((double) this.m_opacity == 0.0 && this.IsGraphicsState)
                this.IsTransparentText = true;
              if (this.Objects.StrokingColorspace != null)
              {
                this.SetStrokingBrush(this.Objects.StrokingColorspace.GetBrush(operands, this.m_resources));
                break;
              }
              if ((tokenType == "scn" || tokenType == "sc") && operands.Length == 4)
              {
                this.SetStrokingCMYKColor(this.GetColor(operands, "stroking", "DeviceCMYK"));
                break;
              }
              if ((tokenType == "sc" || tokenType == "scn") && operands.Length == 1)
              {
                this.SetStrokingGrayColor(this.GetColor(operands, "stroking", "Gray"));
                break;
              }
              if (!flag4)
              {
                this.SetStrokingRGBColor(this.GetColor(operands, "stroking", "RGB"));
                break;
              }
              break;
            case "g":
              this.m_opacity = this.StrokingOpacity;
              if ((double) this.m_opacity == 0.0 && this.IsGraphicsState)
                this.IsTransparentText = true;
              this.SetStrokingGrayColor(this.GetColor(operands, "stroking", "Gray"));
              this.m_backupColorElements = operands;
              this.m_backupColorSpace = "Gray";
              break;
            case "G":
              this.m_opacity = this.NonStrokingOpacity;
              if ((double) this.m_opacity == 0.0 && this.IsGraphicsState)
                this.IsTransparentText = true;
              this.SetNonStrokingGrayColor(this.GetColor(operands, "nonstroking", "Gray"));
              this.m_backupColorElements = operands;
              this.m_backupColorSpace = "Gray";
              break;
            case "Do":
              if (!this.m_skipRendering)
              {
                if (this.m_isType3Font)
                {
                  this.GetType3XObject(operands);
                  break;
                }
                this.GetXObject(operands);
                break;
              }
              break;
            case "re":
              if (!this.m_skipRendering)
              {
                this.GetClipRectangle(operands);
                break;
              }
              break;
            case "d":
              if (operands[0] != "[]" && !operands[0].Contains("\n"))
              {
                this.m_dashedLine = operands;
                break;
              }
              break;
            case "d1":
              this.m_d1Matrix = new Matrix((double) float.Parse(operands[0]), (double) float.Parse(operands[1]), (double) float.Parse(operands[2]), (double) float.Parse(operands[3]), (double) float.Parse(operands[4]), (double) float.Parse(operands[5]));
              break;
            case "gs":
              this.IsTransparentText = false;
              if (this.m_resources.ContainsKey(operands[0].Substring(1)))
              {
                int num23 = 0;
                string str = (string) null;
                PdfDictionary xobjectDictionary = (this.m_resources[operands[0].Substring(1)] as XObjectElement).XObjectDictionary;
                if (xobjectDictionary.ContainsKey("OPM"))
                  num23 = (xobjectDictionary["OPM"] as PdfNumber).IntValue;
                bool flag5 = false;
                if ((double) this.NonStrokingOpacity == 0.0 || (double) this.StrokingOpacity == 0.0)
                  flag5 = true;
                if (xobjectDictionary.ContainsKey("HT"))
                {
                  PdfName pdfName3 = xobjectDictionary["HT"] as PdfName;
                  if (pdfName3 != (PdfName) null)
                  {
                    str = pdfName3.Value;
                  }
                  else
                  {
                    PdfName pdfName4 = (xobjectDictionary["HT"] as PdfReferenceHolder).Object as PdfName;
                    if (pdfName4 != (PdfName) null)
                      str = pdfName4.Value;
                  }
                }
                else if (xobjectDictionary.ContainsKey("CA") || xobjectDictionary.ContainsKey("ca"))
                {
                  if (xobjectDictionary.ContainsKey("CA"))
                    this.NonStrokingOpacity = (xobjectDictionary["CA"] as PdfNumber).FloatValue;
                  if (xobjectDictionary.ContainsKey("ca"))
                    this.StrokingOpacity = (xobjectDictionary["ca"] as PdfNumber).FloatValue;
                  if (flag5)
                  {
                    if (this.StrokingBrush != null && this.m_backupColorElements != null && this.m_backupColorSpace != null)
                    {
                      this.m_opacity = this.StrokingOpacity;
                      this.SetStrokingColor(this.GetColor(this.m_backupColorElements, "Stroking", this.m_backupColorSpace));
                    }
                    if (this.NonStrokingBrush != null && this.m_backupColorElements != null && this.m_backupColorSpace != null)
                    {
                      this.m_opacity = this.NonStrokingOpacity;
                      this.SetNonStrokingColor(this.GetColor(this.m_backupColorElements, "NonStroking", this.m_backupColorSpace));
                    }
                  }
                  this.IsGraphicsState = true;
                }
                if (num23 == 1 && str == "Default")
                {
                  this.IsExtendedGraphicsState = true;
                  break;
                }
                break;
              }
              break;
            case "b":
            case "b*":
              if (!this.m_skipRendering)
              {
                if (this.isRect)
                {
                  float x6 = float.Parse(this.clipRectShape[0]);
                  float y6 = float.Parse(this.clipRectShape[1]);
                  float num24 = float.Parse(this.clipRectShape[2]);
                  float num25 = float.Parse(this.clipRectShape[3]);
                  this.BeginPath(x6, y6);
                  this.AddLine(x6 + num24, y6);
                  this.AddLine(x6 + num24, y6 + num25);
                  this.AddLine(x6, y6 + num25);
                  this.EndPath();
                }
                Matrix matrix11 = new Matrix((double) this.Drawing2dMatrixCTM.Elements[0], (double) this.Drawing2dMatrixCTM.Elements[1], (double) this.Drawing2dMatrixCTM.Elements[2], (double) this.Drawing2dMatrixCTM.Elements[3], (double) this.Drawing2dMatrixCTM.OffsetX, (double) this.Drawing2dMatrixCTM.OffsetY) * this.DocumentMatrix;
                System.Drawing.Drawing2D.Matrix matrix12 = new System.Drawing.Drawing2D.Matrix((float) matrix11.M11, (float) matrix11.M12, (float) matrix11.M21, (float) matrix11.M22, (float) matrix11.OffsetX, (float) matrix11.OffsetY);
                System.Drawing.Drawing2D.Matrix matrix13 = new System.Drawing.Drawing2D.Matrix();
                matrix13.Multiply(matrix12);
                System.Drawing.Drawing2D.Matrix transform = this.PageGraphics.Transform;
                int pageUnit = (int) this.PageGraphics.PageUnit;
                this.PageGraphics.PageUnit = GraphicsUnit.Pixel;
                this.PageGraphics.Transform = new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
                this.PageGraphics.Transform = matrix13;
                foreach (PathFigure figure in this.CurrentGeometry.Figures)
                {
                  figure.IsClosed = true;
                  figure.IsFilled = true;
                }
                GraphicsPath geometry = this.GetGeometry(this.CurrentGeometry, new Matrix(1.0, 0.0, 0.0, 1.0, 0.0, 0.0));
                this.PageGraphics.DrawPath(new Pen(this.NonStrokingBrush == null ? new Pen(Color.Black).Brush : this.NonStrokingBrush)
                {
                  Width = this.MitterLength
                }, geometry);
                geometry.FillMode = !(tokenType.Trim() == "b") ? FillMode.Alternate : FillMode.Winding;
                if (this.StrokingBrush != null)
                  this.PageGraphics.FillPath(new Pen(this.StrokingBrush).Brush, geometry);
                if (geometry.FillMode == FillMode.Alternate)
                  this.FillPath("Alternate");
                else
                  this.FillPath("Winding");
                this.ClipRectangle = (RectangleF) Rectangle.Empty;
                this.CurrentGeometry = new PathGeometry();
                this.m_currentPath = (PathFigure) null;
                break;
              }
              break;
            case "B":
            case "B*":
              if (!this.m_skipRendering)
              {
                if (this.isRect)
                {
                  float x7 = float.Parse(this.clipRectShape[0]);
                  float y7 = float.Parse(this.clipRectShape[1]);
                  float num26 = float.Parse(this.clipRectShape[2]);
                  float num27 = float.Parse(this.clipRectShape[3]);
                  this.BeginPath(x7, y7);
                  this.AddLine(x7 + num26, y7);
                  this.AddLine(x7 + num26, y7 + num27);
                  this.AddLine(x7, y7 + num27);
                  this.EndPath();
                }
                Pen pen = this.NonStrokingBrush == null ? new Pen(Color.Black) : new Pen(this.NonStrokingBrush);
                pen.Width = this.MitterLength;
                Matrix matrix14 = new Matrix((double) this.Drawing2dMatrixCTM.Elements[0], (double) this.Drawing2dMatrixCTM.Elements[1], (double) this.Drawing2dMatrixCTM.Elements[2], (double) this.Drawing2dMatrixCTM.Elements[3], (double) this.Drawing2dMatrixCTM.OffsetX, (double) this.Drawing2dMatrixCTM.OffsetY) * this.DocumentMatrix;
                System.Drawing.Drawing2D.Matrix matrix15 = new System.Drawing.Drawing2D.Matrix((float) matrix14.M11, (float) matrix14.M12, (float) matrix14.M21, (float) matrix14.M22, (float) matrix14.OffsetX, (float) matrix14.OffsetY);
                System.Drawing.Drawing2D.Matrix matrix16 = new System.Drawing.Drawing2D.Matrix();
                matrix16.Multiply(matrix15);
                System.Drawing.Drawing2D.Matrix transform = this.PageGraphics.Transform;
                GraphicsUnit pageUnit = this.PageGraphics.PageUnit;
                this.PageGraphics.PageUnit = GraphicsUnit.Pixel;
                this.PageGraphics.Transform = new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
                this.PageGraphics.Transform = matrix16;
                System.Drawing.Drawing2D.Matrix matrix17 = new System.Drawing.Drawing2D.Matrix();
                foreach (PathFigure figure in this.CurrentGeometry.Figures)
                {
                  figure.IsClosed = true;
                  figure.IsFilled = true;
                }
                GraphicsPath geometry = this.GetGeometry(this.CurrentGeometry, new Matrix(1.0, 0.0, 0.0, 1.0, 0.0, 0.0));
                this.PageGraphics.DrawPath(pen, geometry);
                geometry.FillMode = !(tokenType.Trim() == "B") ? FillMode.Alternate : FillMode.Winding;
                this.PageGraphics.FillPath(new Pen(this.StrokingBrush).Brush, geometry);
                this.PageGraphics.PageUnit = pageUnit;
                this.PageGraphics.Transform = transform;
                this.CurrentGeometry = new PathGeometry();
                this.m_currentPath = (PathFigure) null;
                break;
              }
              break;
            case "n":
              try
              {
                if (this.ClipRectangle != RectangleF.Empty)
                {
                  if (this.m_clippingPath == null && !(this.m_clippingPath == "W"))
                  {
                    if (this.m_clippingPath == "W*")
                    {
                      if ((double) this.MitterLength == 0.0)
                        goto label_466;
                    }
                    else
                      goto label_466;
                  }
                  this.PageGraphics.SetClip(this.ClipRectangle);
                }
                else if (this.Path != null)
                {
                  if (this.Path.PointCount > 0)
                    this.m_graphicspathtoclip = this.Path;
                }
              }
              catch
              {
              }
label_466:
              this.m_clippingPath = (string) null;
              this.ClipRectangle = RectangleF.Empty;
              this.m_subPaths.Clear();
              this.m_tempSubPaths.Clear();
              break;
            case "w":
              this.MitterLength = float.Parse(operands[0]);
              break;
            case "W":
              this.m_clippingPath = tokenType;
              RectangleF clipBounds = this.PageGraphics.ClipBounds;
              if (this.ClipRectangle != RectangleF.Empty)
              {
                clipBounds.Intersect(this.ClipRectangle);
                if (clipBounds != (RectangleF) Rectangle.Empty)
                {
                  this.ClipRectangle = clipBounds;
                  break;
                }
                break;
              }
              break;
            case "W*":
              this.m_clippingPath = tokenType;
              break;
            case "s":
            case "S":
              if (!this.m_skipRendering)
              {
                if (this.isRect)
                {
                  float x8 = float.Parse(this.clipRectShape[0]);
                  float y8 = float.Parse(this.clipRectShape[1]);
                  float num28 = float.Parse(this.clipRectShape[2]);
                  float num29 = float.Parse(this.clipRectShape[3]);
                  this.BeginPath(x8, y8);
                  this.AddLine(x8 + num28, y8);
                  this.AddLine(x8 + num28, y8 + num29);
                  this.AddLine(x8, y8 + num29);
                  this.EndPath();
                }
                this.DrawPath();
                break;
              }
              break;
            case "f*":
              if (!this.m_skipRendering)
              {
                if (this.isRect)
                {
                  float x9 = float.Parse(this.clipRectShape[0]);
                  float y9 = float.Parse(this.clipRectShape[1]);
                  float num30 = float.Parse(this.clipRectShape[2]);
                  float num31 = float.Parse(this.clipRectShape[3]);
                  this.BeginPath(x9, y9);
                  this.AddLine(x9 + num30, y9);
                  this.AddLine(x9 + num30, y9 + num31);
                  this.AddLine(x9, y9 + num31);
                  this.EndPath();
                }
                this.FillPath("Alternate");
                this.CurrentLocation = PointF.Empty;
                break;
              }
              break;
            case "f":
              if (!this.m_skipRendering)
              {
                if (this.isRect)
                {
                  float x10 = float.Parse(this.clipRectShape[0]);
                  float y10 = float.Parse(this.clipRectShape[1]);
                  float num32 = float.Parse(this.clipRectShape[2]);
                  float num33 = float.Parse(this.clipRectShape[3]);
                  this.BeginPath(x10, y10);
                  this.AddLine(x10 + num32, y10);
                  this.AddLine(x10 + num32, y10 + num33);
                  this.AddLine(x10, y10 + num33);
                  this.EndPath();
                }
                this.FillPath("Winding");
                this.CurrentLocation = PointF.Empty;
                break;
              }
              break;
            case "v":
              this.AddBezierCurve2(operands);
              break;
            case "y":
              this.AddBezierCurve3(operands);
              break;
            case "h":
              if (this.Path != null)
              {
                this.Path.CloseAllFigures();
                this.m_subPaths.Add(this.Path);
                this.m_tempSubPaths.Clear();
                this.Path = new GraphicsPath();
              }
              if (this.m_currentPath != null)
              {
                this.m_currentPath.IsClosed = true;
                break;
              }
              break;
            case "m":
              if (!this.m_skipRendering)
              {
                this.BeginPath(operands);
                break;
              }
              break;
            case "c":
              if (!this.m_skipRendering)
              {
                this.isRect = false;
                this.AddBezierCurve(operands);
                break;
              }
              break;
            case "l":
              if (!this.m_skipRendering)
              {
                this.AddLine(operands);
                break;
              }
              break;
            case "ID":
              this.BIparameter = new List<string>();
              foreach (string str in operands)
                this.BIparameter.Add(str);
              this.GetInlineImageParameters(this.BIparameter);
              break;
            case "EI":
              MemoryStream memoryStream = new MemoryStream(recordCollection.RecordCollection[index].InlineImageBytes);
              ImageStructure imageStructure = new ImageStructure((IPdfPrimitive) this.m_inlineParameters, new PdfMatrix());
              imageStructure.ImageStream = (Stream) memoryStream;
              if (this.m_isType3Font)
              {
                this.GetType3XObject(imageStructure);
                break;
              }
              Image image = imageStructure.EmbeddedImage;
              System.Drawing.Drawing2D.Matrix transform6 = this.PageGraphics.Transform;
              GraphicsUnit pageUnit3 = this.PageGraphics.PageUnit;
              this.PageGraphics.PageUnit = GraphicsUnit.Pixel;
              this.PageGraphics.Transform = new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
              Matrix matrix18 = new Matrix((double) this.Drawing2dMatrixCTM.Elements[0], (double) this.Drawing2dMatrixCTM.Elements[1], (double) this.Drawing2dMatrixCTM.Elements[2], (double) this.Drawing2dMatrixCTM.Elements[3], (double) this.Drawing2dMatrixCTM.OffsetX, (double) this.Drawing2dMatrixCTM.OffsetY);
              matrix18.Scale(1.0, -1.01, 0.0, 1.0);
              Matrix matrix19 = matrix18 * this.DocumentMatrix;
              this.PageGraphics.Transform = new System.Drawing.Drawing2D.Matrix((float) matrix19.M11, (float) matrix19.M12, (float) matrix19.M21, (float) matrix19.M22, (float) matrix19.OffsetX, (float) Math.Round(matrix19.OffsetY, 5));
              if (imageStructure.IsImageMask)
              {
                if (image == null)
                  image = this.GetMaskImagefromStream(imageStructure);
                else if (!imageStructure.m_isBlackIs1)
                  image = this.GetMaskImage(image);
              }
              if (image != null)
                this.PageGraphics.DrawImage(image, new RectangleF(0.0f, 0.0f, 1f, 1f));
              this.PageGraphics.Transform = transform6;
              this.PageGraphics.PageUnit = pageUnit3;
              break;
            case "sh":
              if (this.GetColorspace(operands) is ShadingPattern colorspace)
                this.Objects.StrokingBrush = colorspace.CreateBrush();
              LinearGradientBrush strokingBrush = this.StrokingBrush as LinearGradientBrush;
              if (strokingBrush != null)
              {
                RectangleF rectangle = strokingBrush.Rectangle;
                float x11 = rectangle.X;
                rectangle = strokingBrush.Rectangle;
                float y11 = rectangle.Y;
                rectangle = strokingBrush.Rectangle;
                float num34 = rectangle.Width;
                rectangle = strokingBrush.Rectangle;
                float num35 = rectangle.Height;
                if (colorspace.BBox != null)
                {
                  x11 = (colorspace.BBox[0] as PdfNumber).FloatValue + x11;
                  y11 = (colorspace.BBox[1] as PdfNumber).FloatValue + y11;
                  num34 = (colorspace.BBox[2] as PdfNumber).FloatValue + num34;
                  num35 = (colorspace.BBox[3] as PdfNumber).FloatValue + num35;
                }
                this.BeginPath(x11, y11);
                this.AddLine(x11 + num34, y11);
                this.AddLine(x11 + num34, y11 + num35);
                this.AddLine(x11, y11 + num35);
                this.EndPath();
                this.FillPath("Winding");
                strokingBrush.Dispose();
                break;
              }
              break;
          }
        }
        this.m_isType3Font = false;
      }
      catch (Exception ex)
      {
        if (ex.Message.Contains(" does not supported") || ex.Message.Contains("Error in identifying the ImageFilter"))
          this.exception.Exceptions.Append($"\r\n\r\n{ex.Message}\r\n");
        else
          this.exception.Exceptions.Append($"\r\n\r\n{ex.Message}{ex.StackTrace}\r\n");
      }
    }
  }

  private void SetStrokingColorspace(Colorspace colorspace)
  {
    this.Objects.StrokingColorspace = colorspace;
  }

  private void SetNonStrokingColorspace(Colorspace colorspace)
  {
    this.Objects.NonStrokingColorspace = colorspace;
  }

  private void SetNonStrokingRGBColor(Color color)
  {
    this.Objects.NonStrokingColorspace = (Colorspace) new DeviceRGB();
    this.SetNonStrokingColor(color);
  }

  private void SetNonStrokingCMYKColor(Color color)
  {
    this.Objects.NonStrokingColorspace = (Colorspace) new Syncfusion.Pdf.Parsing.DeviceCMYK();
    this.SetNonStrokingColor(color);
  }

  private void SetNonStrokingGrayColor(Color color)
  {
    this.Objects.NonStrokingColorspace = (Colorspace) new DeviceGray();
    this.SetNonStrokingColor(color);
  }

  private void SetStrokingRGBColor(Color color)
  {
    this.Objects.StrokingColorspace = (Colorspace) new DeviceRGB();
    this.SetStrokingColor(color);
  }

  private void SetStrokingCMYKColor(Color color)
  {
    this.Objects.StrokingColorspace = (Colorspace) new Syncfusion.Pdf.Parsing.DeviceCMYK();
    this.SetStrokingColor(color);
  }

  private void SetStrokingGrayColor(Color color)
  {
    this.Objects.StrokingColorspace = (Colorspace) new DeviceGray();
    this.SetStrokingColor(color);
  }

  private void SetStrokingColor(Color color) => this.Objects.StrokingBrush = new Pen(color).Brush;

  private void SetStrokingBrush(Brush brush) => this.Objects.StrokingBrush = brush;

  private void SetNonStrokingBrush(Brush brush) => this.Objects.NonStrokingBrush = brush;

  private void SetNonStrokingColor(Color color)
  {
    this.Objects.NonStrokingBrush = new Pen(color).Brush;
  }

  private Colorspace GetColorspace(string[] colorspaceelement)
  {
    if (Colorspace.IsColorSpace(colorspaceelement[0].Replace("/", "")))
    {
      this.m_currentColorspace = Colorspace.CreateColorSpace(colorspaceelement[0].Replace("/", ""));
      if (colorspaceelement[0].Replace("/", "") == "Pattern")
      {
        object resource = this.m_resources[colorspaceelement[0].Replace("/", "")];
      }
    }
    else if (this.m_resources.ContainsKey(colorspaceelement[0].Replace("/", "")) && this.m_resources[colorspaceelement[0].Replace("/", "")] is ExtendColorspace)
    {
      ExtendColorspace resource = this.m_resources[colorspaceelement[0].Replace("/", "")] as ExtendColorspace;
      if (resource.ColorSpaceValueArray is PdfArray colorSpaceValueArray1)
        this.m_currentColorspace = Colorspace.CreateColorSpace((colorSpaceValueArray1[0] as PdfName).Value, (IPdfPrimitive) colorSpaceValueArray1);
      PdfName colorSpaceValueArray2 = resource.ColorSpaceValueArray as PdfName;
      if (colorSpaceValueArray2 != (PdfName) null)
        this.m_currentColorspace = Colorspace.CreateColorSpace(colorSpaceValueArray2.Value);
      if (resource.ColorSpaceValueArray is PdfDictionary colorSpaceValueArray3)
        this.m_currentColorspace = Colorspace.CreateColorSpace("Shading", (IPdfPrimitive) colorSpaceValueArray3);
    }
    return this.m_currentColorspace;
  }

  private PdfArray GetShadingPatternMatrix(PdfArray shadingPattern)
  {
    (shadingPattern[0] as PdfNumber).FloatValue = (float) this.CTRM.M11;
    (shadingPattern[1] as PdfNumber).FloatValue = (float) this.CTRM.M12;
    (shadingPattern[2] as PdfNumber).FloatValue = (float) this.CTRM.M21;
    (shadingPattern[3] as PdfNumber).FloatValue = (float) this.CTRM.M22;
    (shadingPattern[4] as PdfNumber).FloatValue = (float) this.CTRM.OffsetX;
    (shadingPattern[5] as PdfNumber).FloatValue = (float) this.CTRM.OffsetY;
    return shadingPattern;
  }

  private void GetInlineImageParameters(List<string> element)
  {
    int index1 = 0;
    Dictionary<string, string> dictionary = new Dictionary<string, string>();
    for (; index1 < element.Count; index1 += 2)
      dictionary.Add(element[index1].Substring(1, element[index1].Length - 1), element[index1 + 1]);
    this.m_inlineParameters = new PdfDictionary();
    foreach (KeyValuePair<string, string> keyValuePair in dictionary)
    {
      switch (keyValuePair.Key)
      {
        case "Height":
        case "H":
          this.m_inlineParameters.SetNumber("Height", this.FloatParse(keyValuePair.Value));
          continue;
        case "Width":
        case "W":
          this.m_inlineParameters.SetNumber("Width", this.FloatParse(keyValuePair.Value));
          continue;
        case "BitsPerComponent":
        case "BPC":
          this.m_inlineParameters.SetNumber("BitsPerComponent", this.FloatParse(keyValuePair.Value));
          continue;
        case "ImageMask":
        case "IM":
          if (keyValuePair.Value == "true")
          {
            this.m_inlineParameters.SetBoolean("ImageMask", true);
            continue;
          }
          this.m_inlineParameters.SetBoolean("ImageMask", false);
          continue;
        case "ColorSpace":
        case "CS":
          if (keyValuePair.Value.Substring(1) == "RGB" || keyValuePair.Value.Substring(1) == "DeviceRGB")
          {
            this.m_inlineParameters.SetName("ColorSpace", "DeviceRGB");
            continue;
          }
          if (keyValuePair.Value.Substring(1) == "G")
          {
            this.m_inlineParameters.SetName("ColorSpace", "DeviceGray");
            continue;
          }
          if (keyValuePair.Value.Substring(1) == "CMYK")
          {
            this.m_inlineParameters.SetName("ColorSpace", "DeviceCMYK");
            continue;
          }
          if (this.m_resources.ContainsKey(keyValuePair.Value.Replace("/", "")))
          {
            this.m_inlineParameters.SetProperty("ColorSpace", (IPdfPrimitive) ((this.m_resources[keyValuePair.Value.Replace("/", "")] as ExtendColorspace).ColorSpaceValueArray as PdfArray));
            continue;
          }
          continue;
        case "Decode":
        case "D":
          string[] strArray1 = keyValuePair.Value.Replace("[", string.Empty).Replace("]", string.Empty).Split(new char[2]
          {
            ' ',
            '\n'
          }, StringSplitOptions.RemoveEmptyEntries);
          float[] array = new float[strArray1.Length];
          for (int index2 = 0; index2 < strArray1.Length; ++index2)
            array[index2] = this.FloatParse(strArray1[index2]);
          this.m_inlineParameters.SetProperty("Decode", (IPdfPrimitive) new PdfArray(array));
          continue;
        case "DP":
          string blackis1 = "";
          int predictor;
          int columns;
          int colors;
          int k;
          this.GetDecodeParams(keyValuePair.Value, out predictor, out columns, out colors, out k, out blackis1);
          PdfDictionary primitive1 = new PdfDictionary();
          primitive1.SetNumber("Predictor", predictor);
          primitive1.SetNumber("Columns", columns);
          primitive1.SetNumber("Colors", colors);
          primitive1.SetNumber("K", k);
          if (keyValuePair.Value.Contains("BlackIs1"))
          {
            if (blackis1 == "true")
              primitive1.SetBoolean("BlackIs1", true);
            else
              primitive1.SetBoolean("BlackIs1", false);
          }
          this.m_inlineParameters.SetProperty("DecodeParms", (IPdfPrimitive) primitive1);
          continue;
        case "Filter":
        case "F":
          PdfArray primitive2 = new PdfArray();
          string[] strArray2 = new string[2];
          if (keyValuePair.Value.Contains(" "))
          {
            string[] strArray3 = keyValuePair.Value.Split(' ');
            for (int index3 = 0; index3 < strArray3.Length; ++index3)
            {
              if (index3 == 0)
                strArray3[index3] = strArray3[index3].Substring(2);
              else if (index3 == strArray3.Length - 1)
                strArray3[strArray3.Length - 1] = strArray3[strArray3.Length - 1].Substring(1, strArray3[strArray3.Length - 1].Length - 2);
              else
                strArray3[index3] = strArray3[index3].Substring(1);
              if (strArray3[index3] == "Fl")
                strArray3[index3] = "FlateDecode";
              PdfName element1 = new PdfName(strArray3[index3]);
              primitive2.Add((IPdfPrimitive) element1);
            }
          }
          else
          {
            strArray2[0] = keyValuePair.Value.Substring(1);
            strArray2[0] = this.RemoveUnwantedChar(strArray2[0]);
            if (strArray2[0] == "Fl")
              strArray2[0] = "FlateDecode";
            else if (strArray2[0] == "CCF")
              strArray2[0] = "CCITTFaxDecode";
            else if (strArray2[0] == "AHx")
              strArray2[0] = "ASCIIHex";
            else if (strArray2[0] == "A85")
              strArray2[0] = "ASCII85Decode";
            else if (strArray2[0] == "LZW")
              strArray2[0] = "LZWDecode";
            else if (strArray2[0] == "RL")
              strArray2[0] = "RunLengthDecode";
            else if (strArray2[0] == "DCT")
              strArray2[0] = "DCTDecode";
            PdfName element2 = new PdfName(strArray2[0]);
            primitive2.Add((IPdfPrimitive) element2);
          }
          this.m_inlineParameters.SetProperty("Filter", (IPdfPrimitive) primitive2);
          continue;
        default:
          continue;
      }
    }
  }

  private string RemoveUnwantedChar(string originalString)
  {
    char[] chArray = new char[2]{ '/', ']' };
    for (int index = 0; index < chArray.Length; ++index)
    {
      if (originalString.Contains(chArray[index].ToString()))
        originalString = originalString.Replace(chArray[index].ToString(), "");
    }
    return originalString;
  }

  private void GetDecodeParams(
    string decodeParam,
    out int predictor,
    out int columns,
    out int colors,
    out int k,
    out string blackis1)
  {
    predictor = 0;
    colors = 1;
    columns = 1;
    k = 0;
    blackis1 = "";
    decodeParam = decodeParam.Remove(0, 2);
    decodeParam = decodeParam.Remove(decodeParam.Length - 2, 2);
    char[] chArray = new char[2]{ ' ', '\n' };
    string[] strArray = decodeParam.Split(chArray);
    int index = 0;
    while (index < strArray.Length)
    {
      switch (strArray[index])
      {
        case "/Predictor":
          int.TryParse(strArray[++index], out predictor);
          continue;
        case "/K":
          int.TryParse(strArray[++index], out k);
          continue;
        case "/Columns":
          int.TryParse(strArray[++index], out columns);
          continue;
        case "/Colors":
          int.TryParse(strArray[++index], out colors);
          continue;
        case "/BlackIs1":
          blackis1 = strArray[++index];
          continue;
        default:
          ++index;
          continue;
      }
    }
  }

  private void MoveToNextLineWithLeading(string[] element)
  {
    this.SetTextLeading(-this.FloatParse(element[1]));
    this.MoveToNextLine((double) this.FloatParse(element[0]), (double) this.FloatParse(element[1]));
  }

  private void SetTextLeading(float txtLeading) => this.TextLeading = -txtLeading;

  private void RenderFont(string[] fontElements)
  {
    int index;
    for (index = 0; index < fontElements.Length; ++index)
    {
      if (fontElements[index].Contains("/"))
      {
        this.CurrentFont = fontElements[index].Replace("/", "");
        break;
      }
    }
    this.FontSize = this.FloatParse(fontElements[index + 1]);
  }

  private void RenderTextElement(string[] textElements, string tokenType)
  {
    string str1 = string.Join("", textElements);
    if (!this.m_resources.ContainsKey(this.CurrentFont) || !(this.m_resources[this.CurrentFont] is FontStructure structure))
      return;
    structure.IsSameFont = this.m_resources.isSameFont();
    if ((double) structure.FontSize != (double) this.FontSize)
      structure.FontSize = this.FontSize;
    byte[] bytes = Encoding.Unicode.GetBytes(structure.ToGetEncodedText(str1, this.m_resources.isSameFont()));
    Dictionary<int, int> dictionary = new Dictionary<int, int>();
    int key = 0;
    for (int index = 0; index < bytes.Length; index += 2)
    {
      dictionary.Add(key, (int) bytes[index]);
      ++key;
    }
    string str2 = structure.Decode(str1, this.m_resources.isSameFont());
    TextElement textElement = new TextElement(str2, this.DocumentMatrix);
    textElement.EncodedTextBytes = dictionary;
    textElement.FontName = structure.FontName;
    textElement.FontStyle = structure.FontStyle;
    textElement.Font = structure.CurrentFont;
    textElement.FontSize = this.FontSize;
    textElement.TextScaling = this.m_textScaling;
    textElement.FontEncoding = structure.FontEncoding;
    textElement.FontGlyphWidths = structure.FontGlyphWidths;
    textElement.DefaultGlyphWidth = structure.DefaultGlyphWidth;
    textElement.isNegativeFont = this.isNegativeFont;
    textElement.UnicodeCharMapTable = structure.UnicodeCharMapTable;
    textElement.IsFindText = this.isFindText;
    textElement.IsPdfium = this.IsPdfium;
    Dictionary<int, int> fontGlyphWidths = structure.FontGlyphWidths;
    textElement.IsType1Font = structure.IsType1Font;
    textElement.Is1C = structure.Is1C;
    textElement.IsCID = structure.IsCID;
    textElement.CharacterMapTable = structure.CharacterMapTable;
    textElement.CidToGidReverseMapTable = structure.CidToGidReverseMapTable;
    textElement.ReverseMapTable = structure.ReverseMapTable;
    textElement.Fontfile2Glyph = structure.GlyphFontFile2;
    textElement.structure = structure;
    textElement.Isembeddedfont = structure.isEmbedded;
    textElement.Ctm = this.CTRM;
    textElement.textLineMatrix = this.TextMatrix;
    textElement.IsExtractTextData = this.m_isExtractTextData;
    textElement.Rise = this.Rise;
    textElement.transformMatrix = this.DocumentMatrix;
    textElement.documentMatrix = this.DocumentMatrix;
    textElement.FontID = this.CurrentFont;
    textElement.OctDecMapTable = structure.OctDecMapTable;
    textElement.TextHorizontalScaling = this.HorizontalScaling;
    textElement.ZapfPostScript = structure.ZapfPostScript;
    textElement.LineWidth = this.MitterLength;
    textElement.RenderingMode = this.RenderingMode;
    textElement.testdict = this.testdict;
    textElement.pageRotation = this.pageRotation;
    textElement.zoomFactor = this.zoomFactor;
    textElement.SubstitutedFontsList = this.substitutedFontsList;
    if (structure.Flags != null)
      textElement.FontFlag = structure.Flags.IntValue;
    if (structure.IsType1Font)
    {
      textElement.IsType1Font = true;
      textElement.differenceTable = structure.differenceTable;
      textElement.differenceMappedTable = structure.DifferencesDictionary;
      textElement.m_cffGlyphs = structure.m_cffGlyphs;
      textElement.OctDecMapTable = structure.OctDecMapTable;
      if (!this.m_glyphDataCollection.Contains(textElement.m_cffGlyphs))
        this.m_glyphDataCollection.Add(textElement.m_cffGlyphs);
    }
    textElement.PathBrush = this.StrokingBrush == null ? new Pen(Color.Black).Brush : (textElement.RenderingMode != 3 ? this.StrokingBrush : this.StrokingBrush);
    textElement.PathNonStrokeBrush = this.NonStrokingBrush == null ? new Pen(Color.Black).Brush : this.NonStrokingBrush;
    textElement.WordSpacing = this.Objects.WordSpacing;
    textElement.CharacterSpacing = this.Objects.CharacterSpacing;
    if (this.m_beginText)
      this.m_beginText = false;
    Matrix txtMatrix = new Matrix();
    if (structure.fontType.Value != "Type3")
    {
      if (this.m_isCurrentPositionChanged)
      {
        this.m_isCurrentPositionChanged = false;
        this.m_endTextPosition = this.CurrentLocation;
        this.m_textElementWidth = textElement.Render(this.PageGraphics, new PointF(this.m_endTextPosition.X, this.m_endTextPosition.Y - this.FontSize), (double) this.m_textScaling, fontGlyphWidths, (double) structure.Type1GlyphHeight, structure.differenceTable, structure.DifferencesDictionary, structure.differenceEncoding, out txtMatrix);
        if (textElement.Fontfile2Glyph != null && textElement.Fontfile2Glyph.IsFontFile2)
          textElement.textElementGlyphList = this.ReplaceLigatureFromGlyphList(textElement.textElementGlyphList);
        this.imageRenderGlyphList.AddRange((IEnumerable<Glyph>) textElement.textElementGlyphList);
        if (this.isExtractLineCollection)
          this.extractTextElement.Add(textElement);
        textElement.textElementGlyphList.Clear();
      }
      else
      {
        this.m_endTextPosition = new PointF(this.m_endTextPosition.X + this.m_textElementWidth, this.m_endTextPosition.Y);
        this.m_textElementWidth = textElement.Render(this.PageGraphics, new PointF(this.m_endTextPosition.X, this.m_endTextPosition.Y - this.FontSize), (double) this.m_textScaling, fontGlyphWidths, (double) structure.Type1GlyphHeight, structure.differenceTable, structure.DifferencesDictionary, structure.differenceEncoding, out txtMatrix);
        if (textElement.Fontfile2Glyph != null && textElement.Fontfile2Glyph.IsFontFile2)
          textElement.textElementGlyphList = this.ReplaceLigatureFromGlyphList(textElement.textElementGlyphList);
        this.imageRenderGlyphList.AddRange((IEnumerable<Glyph>) textElement.textElementGlyphList);
        textElement.textElementGlyphList.Clear();
      }
    }
    else
    {
      Matrix textLineMatrix = this.TextLineMatrix;
      FontStructure fontStructure = structure;
      this.PageGraphics.Transform = new System.Drawing.Drawing2D.Matrix((float) this.DocumentMatrix.M11, (float) this.DocumentMatrix.M12, (float) this.DocumentMatrix.M21, (float) this.DocumentMatrix.M22, (float) this.DocumentMatrix.OffsetX, (float) this.DocumentMatrix.OffsetY);
      this.PageGraphics.TranslateTransform(0.0f, (float) this.TextLineMatrix.OffsetY);
      this.m_spacingWidth = 0.0f;
      this.RenderType3GlyphImages(structure, str2);
      this.TextLineMatrix = textLineMatrix;
      structure = fontStructure;
      txtMatrix = Matrix.Identity;
    }
    this.TextMatrix = txtMatrix;
    string str3 = textElement.renderedText;
    if (!structure.IsMappingDone)
    {
      if (structure.CharacterMapTable != null && structure.CharacterMapTable.Count > 0)
        str3 = structure.MapCharactersFromTable(str3);
      else if (structure.DifferencesDictionary != null && structure.DifferencesDictionary.Count > 0)
        str3 = structure.MapDifferences(str3);
    }
    Matrix matrix1 = this.CTRM * this.TextLineMatrix;
    float num = new PdfUnitConvertor().ConvertFromPixels(this.currentPageHeight, PdfGraphicsUnit.Point);
    if (!str3.Contains("www") && !str3.Contains("http") && !this.IsValidEmail(str3) && !str3.Contains(".com"))
      return;
    float emSize = 0.0f;
    Matrix matrix2 = new Matrix()
    {
      M11 = (double) this.FontSize * ((double) this.HorizontalScaling / 100.0),
      M22 = -(double) this.FontSize,
      OffsetY = ((double) this.FontSize + (double) this.Rise)
    } * this.TextLineMatrix * this.CTRM;
    if (matrix2.M11 != 0.0 && matrix2.M22 != 0.0)
      emSize = Math.Abs((float) matrix2.M11);
    else if (matrix2.M12 != 0.0 && matrix2.M21 != 0.0)
      emSize = Math.Abs((float) matrix2.M12);
    Font font = new Font(this.CurrentFont, emSize);
    SizeF sizeF = this.PageGraphics.MeasureString(str3, font);
    double textLeading = (double) this.TextLeading;
    Matrix matrix3 = this.CTRM * this.TextLineMatrix;
    if (this.TextLineMatrix.M12 != 0.0 && this.TextLineMatrix.M21 != 0.0)
    {
      matrix3.OffsetX = this.TextLineMatrix.OffsetY;
      matrix3.OffsetY = -this.TextLineMatrix.OffsetX;
    }
    else
    {
      matrix3.OffsetX = matrix2.OffsetX;
      matrix3.OffsetY = matrix2.OffsetY;
      matrix3 *= new Matrix(1.0, 0.0, 0.0, 1.0, 0.0, -(double) num);
    }
    this.currentTransformLocation = new PointF((float) matrix3.OffsetX, -(float) matrix3.OffsetY);
    Regex regex = new Regex("\\b(?:http://|https://|www\\.)\\S+\\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    string URI = string.Empty;
    foreach (Capture match in regex.Matches(str3))
      URI = match.Value;
    if (this.IsValidEmail(textElement.renderedText) || str3.Contains("@"))
      URI = "mailto:" + str3;
    this.URLDictonary.Add(new PageURL(this.transformMatrix, URI, new PointF(this.currentTransformLocation.X, this.currentTransformLocation.Y), sizeF.Width, sizeF.Height));
  }

  private void UpdateTextMatrix(double tj)
  {
    double x = -(tj * 0.001 * (double) this.FontSize * (double) this.HorizontalScaling / 100.0);
    Point point1 = this.Objects.textLineMatrix.Transform(new Point(0.0, 0.0));
    Point point2 = this.Objects.textLineMatrix.Transform(new Point(x, 0.0));
    if (point1.X != point2.X)
      this.Objects.textLineMatrix.OffsetX = point2.X;
    else
      this.Objects.textLineMatrix.OffsetY = point2.Y;
    this.m_spacingWidth += (float) x;
  }

  private void RenderType3GlyphImagesTJ(FontStructure structure, List<string> decodedCollection)
  {
    for (int index1 = 0; index1 < decodedCollection.Count; ++index1)
    {
      string decoded = decodedCollection[index1];
      double result;
      if (double.TryParse(decoded, out result))
      {
        this.UpdateTextMatrix(result);
        this.TextMatrix = this.TextLineMatrix;
      }
      else
      {
        string str1 = decoded.Remove(decoded.Length - 1, 1);
        for (int index2 = 0; index2 < str1.Length; ++index2)
        {
          this.m_type3WhiteSpaceWidth = str1[index2] != ' ' || (double) this.Objects.WordSpacing <= 0.0 ? 0.0f : this.Objects.WordSpacing;
          string str2 = structure.DecodeType3FontData(str1[index2].ToString());
          if (!structure.Type3FontCharProcsDict.ContainsKey(str2))
            str2 = FontStructure.GetCharCode(str2);
          if (structure.Type3FontCharProcsDict != null && structure.Type3FontCharProcsDict.ContainsKey(str2))
          {
            this.m_type3GlyphID = str2 + structure.FontRefNumber;
            MemoryStream memoryStream = new MemoryStream();
            PdfStream pdfStream = structure.Type3FontCharProcsDict[str2];
            byte[] buffer = PdfString.StringToByte("\r\n");
            pdfStream.isSkip = true;
            pdfStream.Decompress();
            pdfStream.InternalStream.WriteTo((Stream) memoryStream);
            memoryStream.Write(buffer, 0, buffer.Length);
            bool flag = false;
            Matrix ctrm = this.CTRM;
            System.Drawing.Drawing2D.Matrix drawing2dMatrixCtm = this.Drawing2dMatrixCTM;
            Matrix matrix = this.TextLineMatrix * this.CTRM;
            Stack<GraphicObjectData> objects = this.m_objects;
            if (structure.FontDictionary.ContainsKey("FontMatrix"))
            {
              PdfArray font = structure.FontDictionary["FontMatrix"] as PdfArray;
              this.XFormsMatrix = new Matrix((double) (font[0] as PdfNumber).FloatValue, (double) (font[1] as PdfNumber).FloatValue, (double) (font[2] as PdfNumber).FloatValue, (double) (font[3] as PdfNumber).FloatValue, (double) (font[4] as PdfNumber).FloatValue, (double) (font[5] as PdfNumber).FloatValue);
              matrix = this.XFormsMatrix * matrix;
              this.m_type3FontScallingFactor = (float) matrix.M11;
            }
            if (memoryStream != null)
            {
              this.m_type3RecordCollection = new ContentParser(memoryStream.ToArray()).ReadContent();
              for (int index3 = 0; index3 < this.m_type3RecordCollection.RecordCollection.Count; ++index3)
              {
                string str3 = this.m_type3RecordCollection.RecordCollection[index3].OperatorName;
                if (str3 == "cm")
                  this.m_istype3FontContainCTRM = true;
                foreach (char symbolChar in this.m_symbolChars)
                {
                  if (str3.Contains(symbolChar.ToString()))
                    str3 = str3.Replace(symbolChar.ToString(), "");
                }
                if (str3.Trim() == "BI" || str3.Trim() == "Do")
                  flag = true;
              }
              PdfPageResources resources = this.m_resources;
              this.m_resources = structure.Type3FontGlyphImages;
              this.m_transformations = new TransformationStack(this.DocumentMatrix);
              this.m_type3FontStruct = structure;
              this.m_isType3Font = true;
              if (flag)
              {
                this.CTRM = new Matrix(matrix.M11, matrix.M12, matrix.M21, matrix.M22, matrix.OffsetX, matrix.OffsetY);
                this.Drawing2dMatrixCTM = new System.Drawing.Drawing2D.Matrix((float) matrix.M11, (float) matrix.M12, (float) matrix.M21, (float) matrix.M22, (float) matrix.OffsetX, (float) matrix.OffsetY);
              }
              this.RenderAsImage();
              this.m_resources = resources;
              if (flag)
                this.TextLineMatrix = this.m_type3TextLineMatrix;
            }
            this.CTRM = ctrm;
            this.Drawing2dMatrixCTM = drawing2dMatrixCtm;
            this.m_objects = objects;
          }
          else
          {
            float num;
            if (this.m_d1Matrix.M11 == 0.0)
            {
              num = (float) (this.m_d1Matrix.M21 - this.m_d1Matrix.OffsetX);
              if ((double) num < 0.0)
                num = -num;
            }
            else
              num = (float) this.m_d1Matrix.M11;
            this.TextLineMatrix = this.CalculateType3TextMatrixupdate(this.TextLineMatrix, num * this.m_type3FontScallingFactor, true);
          }
        }
      }
    }
  }

  private void RenderType3GlyphImages(FontStructure structure, string renderingText)
  {
    for (int index1 = 0; index1 < renderingText.Length; ++index1)
    {
      string str1 = structure.DecodeType3FontData(renderingText[index1].ToString());
      if (structure.Type3FontCharProcsDict != null && !structure.Type3FontCharProcsDict.ContainsKey(str1))
        str1 = FontStructure.GetCharCode(str1);
      string str2 = string.Empty;
      string key = string.Empty;
      if (renderingText.Length == 1)
      {
        string differences = structure.DifferencesDictionary[((int) renderingText.ToCharArray()[0]).ToString()];
        if (differences.Length == 7 && differences.ToLowerInvariant().StartsWith("uni"))
        {
          key = differences;
          str2 = structure.DecodeToUnicode(differences);
        }
        else
          str2 = string.Empty;
      }
      if (structure.Type3FontCharProcsDict != null && (structure.Type3FontCharProcsDict.ContainsKey(str1) || str2 == str1))
      {
        this.m_type3GlyphID = str1 + structure.FontRefNumber;
        MemoryStream memoryStream = new MemoryStream();
        PdfStream pdfStream1 = new PdfStream();
        PdfStream pdfStream2 = !(str2 == str1) || !(key != string.Empty) ? structure.Type3FontCharProcsDict[str1] : structure.Type3FontCharProcsDict[key];
        byte[] buffer = PdfString.StringToByte("\r\n");
        pdfStream2.isSkip = true;
        pdfStream2.Decompress();
        pdfStream2.InternalStream.WriteTo((Stream) memoryStream);
        memoryStream.Write(buffer, 0, buffer.Length);
        bool flag = false;
        Matrix ctrm = this.CTRM;
        System.Drawing.Drawing2D.Matrix drawing2dMatrixCtm = this.Drawing2dMatrixCTM;
        Matrix matrix = this.TextLineMatrix * this.CTRM;
        Stack<GraphicObjectData> objects = this.m_objects;
        if (structure.FontDictionary.ContainsKey("FontMatrix"))
        {
          PdfArray font = structure.FontDictionary["FontMatrix"] as PdfArray;
          this.XFormsMatrix = new Matrix((double) (font[0] as PdfNumber).FloatValue, (double) (font[1] as PdfNumber).FloatValue, (double) (font[2] as PdfNumber).FloatValue, (double) (font[3] as PdfNumber).FloatValue, (double) (font[4] as PdfNumber).FloatValue, (double) (font[5] as PdfNumber).FloatValue);
          matrix = this.XFormsMatrix * matrix;
          this.m_type3FontScallingFactor = (float) matrix.M11;
        }
        if (memoryStream != null)
        {
          this.m_type3RecordCollection = new ContentParser(memoryStream.ToArray()).ReadContent();
          for (int index2 = 0; index2 < this.m_type3RecordCollection.RecordCollection.Count; ++index2)
          {
            string str3 = this.m_type3RecordCollection.RecordCollection[index2].OperatorName;
            if (str3 == "cm")
              this.m_istype3FontContainCTRM = true;
            foreach (char symbolChar in this.m_symbolChars)
            {
              if (str3.Contains(symbolChar.ToString()))
                str3 = str3.Replace(symbolChar.ToString(), "");
            }
            if (str3.Trim() == "BI" || str3.Trim() == "Do")
              flag = true;
          }
          PdfPageResources resources = this.m_resources;
          this.m_resources = structure.Type3FontGlyphImages;
          this.m_transformations = new TransformationStack(this.DocumentMatrix);
          this.m_type3FontStruct = structure;
          this.m_isType3Font = true;
          if (flag)
          {
            this.CTRM = new Matrix(matrix.M11, matrix.M12, matrix.M21, matrix.M22, matrix.OffsetX, matrix.OffsetY);
            this.Drawing2dMatrixCTM = new System.Drawing.Drawing2D.Matrix((float) matrix.M11, (float) matrix.M12, (float) matrix.M21, (float) matrix.M22, (float) matrix.OffsetX, (float) matrix.OffsetY);
          }
          this.RenderAsImage();
          this.m_resources = resources;
          if (flag)
            this.TextLineMatrix = this.m_type3TextLineMatrix;
        }
        if (str1 == "space")
          this.TextLineMatrix = this.CalculateType3TextMatrixupdate(this.TextLineMatrix, (float) this.m_d0Matrix.M11 * this.m_type3FontScallingFactor, true);
        else if (renderingText[index1].ToString() == " " && !flag)
          this.TextLineMatrix = this.CalculateType3TextMatrixupdate(this.TextLineMatrix, (float) this.m_d1Matrix.M11 * this.m_type3FontScallingFactor, true);
        this.CTRM = ctrm;
        this.Drawing2dMatrixCTM = drawing2dMatrixCtm;
        this.m_objects = objects;
      }
    }
  }

  private void RenderTextElementWithLeading(string[] textElements, string tokenType)
  {
    string textToDecode = string.Join("", textElements);
    if (!this.m_resources.ContainsKey(this.CurrentFont))
      return;
    (this.m_resources[this.CurrentFont] as FontStructure).IsSameFont = this.m_resources.isSameFont();
    if ((double) (this.m_resources[this.CurrentFont] as FontStructure).FontSize != (double) this.FontSize)
      (this.m_resources[this.CurrentFont] as FontStructure).FontSize = this.FontSize;
    FontStructure structure = this.m_resources[this.CurrentFont] as FontStructure;
    string str1 = structure.Decode(textToDecode, this.m_resources.isSameFont());
    TextElement textElement = new TextElement(str1, this.DocumentMatrix);
    textElement.FontName = structure.FontName;
    textElement.FontStyle = structure.FontStyle;
    textElement.Font = structure.CurrentFont;
    Font currentFont = structure.CurrentFont;
    textElement.FontSize = this.FontSize;
    textElement.TextScaling = this.m_textScaling;
    textElement.FontEncoding = structure.FontEncoding;
    textElement.FontGlyphWidths = structure.FontGlyphWidths;
    textElement.DefaultGlyphWidth = structure.DefaultGlyphWidth;
    textElement.Text = str1;
    textElement.IsFindText = this.isFindText;
    textElement.IsPdfium = this.IsPdfium;
    textElement.IsTransparentText = this.IsTransparentText;
    textElement.UnicodeCharMapTable = structure.UnicodeCharMapTable;
    Dictionary<int, int> fontGlyphWidths = structure.FontGlyphWidths;
    textElement.CidToGidReverseMapTable = structure.CidToGidReverseMapTable;
    textElement.IsType1Font = structure.IsType1Font;
    textElement.Is1C = structure.Is1C;
    textElement.IsCID = structure.IsCID;
    textElement.CharacterMapTable = structure.CharacterMapTable;
    textElement.ReverseMapTable = structure.ReverseMapTable;
    textElement.Fontfile2Glyph = structure.GlyphFontFile2;
    textElement.structure = structure;
    textElement.Isembeddedfont = structure.isEmbedded;
    textElement.Ctm = this.CTRM;
    textElement.textLineMatrix = this.TextMatrix;
    textElement.Rise = this.Rise;
    textElement.transformMatrix = this.DocumentMatrix;
    textElement.documentMatrix = this.DocumentMatrix;
    textElement.FontID = this.CurrentFont;
    textElement.OctDecMapTable = structure.OctDecMapTable;
    textElement.TextHorizontalScaling = this.HorizontalScaling;
    textElement.ZapfPostScript = structure.ZapfPostScript;
    textElement.LineWidth = this.MitterLength;
    textElement.RenderingMode = this.RenderingMode;
    textElement.testdict = this.testdict;
    textElement.pageRotation = this.pageRotation;
    textElement.zoomFactor = this.zoomFactor;
    textElement.SubstitutedFontsList = this.substitutedFontsList;
    if (structure.Flags != null)
      textElement.FontFlag = structure.Flags.IntValue;
    if (structure.IsType1Font)
    {
      textElement.IsType1Font = true;
      textElement.differenceTable = structure.differenceTable;
      textElement.differenceMappedTable = structure.DifferencesDictionary;
      textElement.m_cffGlyphs = structure.m_cffGlyphs;
      if (!this.m_glyphDataCollection.Contains(textElement.m_cffGlyphs))
        this.m_glyphDataCollection.Add(textElement.m_cffGlyphs);
    }
    if (this.StrokingBrush != null)
    {
      if ((double) this.StrokingOpacity != 1.0)
      {
        this.m_opacity = this.StrokingOpacity;
        this.SetStrokingColor(this.GetColor(this.m_backupColorElements, "Stroking", this.m_backupColorSpace));
        textElement.PathBrush = this.StrokingBrush;
      }
      else
        textElement.PathBrush = this.StrokingBrush;
    }
    else
      textElement.PathBrush = new Pen(Color.Black).Brush;
    if (this.NonStrokingBrush != null)
      textElement.PathNonStrokeBrush = this.NonStrokingBrush;
    textElement.WordSpacing = this.Objects.WordSpacing;
    textElement.CharacterSpacing = this.Objects.CharacterSpacing;
    if ((double) this.TextScaling != 100.0)
      textElement.Textscalingfactor = this.TextScaling / 100f;
    if (this.m_beginText)
      this.m_beginText = false;
    Matrix txtMatrix = new Matrix();
    if (structure.fontType.Value != "Type3")
    {
      if (this.m_isCurrentPositionChanged)
      {
        this.m_isCurrentPositionChanged = false;
        this.m_endTextPosition = this.CurrentLocation;
        this.m_textElementWidth = textElement.Render(this.PageGraphics, new PointF(this.m_endTextPosition.X, this.m_endTextPosition.Y + (float) (-(double) this.TextLeading / 4.0)), (double) this.m_textScaling, fontGlyphWidths, (double) structure.Type1GlyphHeight, structure.differenceTable, structure.DifferencesDictionary, structure.differenceEncoding, out txtMatrix);
        if (textElement.Fontfile2Glyph != null && textElement.Fontfile2Glyph.IsFontFile2)
          textElement.textElementGlyphList = this.ReplaceLigatureFromGlyphList(textElement.textElementGlyphList);
        this.imageRenderGlyphList.AddRange((IEnumerable<Glyph>) textElement.textElementGlyphList);
        if (this.isExtractLineCollection)
          this.extractTextElement.Add(textElement);
        textElement.textElementGlyphList.Clear();
      }
      else
      {
        this.m_endTextPosition = new PointF(this.m_endTextPosition.X + this.m_textElementWidth, this.m_endTextPosition.Y);
        this.m_textElementWidth = textElement.Render(this.PageGraphics, new PointF(this.m_endTextPosition.X, this.m_endTextPosition.Y + (float) (-(double) this.TextLeading / 4.0)), (double) this.m_textScaling, fontGlyphWidths, (double) structure.Type1GlyphHeight, structure.differenceTable, structure.DifferencesDictionary, structure.differenceEncoding, out txtMatrix);
        if (textElement.Fontfile2Glyph != null && textElement.Fontfile2Glyph.IsFontFile2)
          textElement.textElementGlyphList = this.ReplaceLigatureFromGlyphList(textElement.textElementGlyphList);
        this.imageRenderGlyphList.AddRange((IEnumerable<Glyph>) textElement.textElementGlyphList);
        textElement.textElementGlyphList.Clear();
      }
    }
    else
    {
      Matrix textLineMatrix = this.TextLineMatrix;
      FontStructure fontStructure = structure;
      this.PageGraphics.Transform = new System.Drawing.Drawing2D.Matrix((float) this.DocumentMatrix.M11, (float) this.DocumentMatrix.M12, (float) this.DocumentMatrix.M21, (float) this.DocumentMatrix.M22, (float) this.DocumentMatrix.OffsetX, (float) this.DocumentMatrix.OffsetY);
      this.PageGraphics.TranslateTransform(0.0f, (float) this.TextLineMatrix.OffsetY);
      this.m_spacingWidth = 0.0f;
      this.RenderType3GlyphImages(structure, str1);
      this.TextLineMatrix = textLineMatrix;
      structure = fontStructure;
    }
    this.TextMatrix = txtMatrix;
    string str2 = textElement.renderedText;
    if (!structure.IsMappingDone)
    {
      if (structure.CharacterMapTable != null && structure.CharacterMapTable.Count > 0)
        str2 = structure.MapCharactersFromTable(str2);
      else if (structure.DifferencesDictionary != null && structure.DifferencesDictionary.Count > 0)
        str2 = structure.MapDifferences(str2);
    }
    Matrix matrix1 = this.CTRM * this.TextLineMatrix;
    float num = new PdfUnitConvertor().ConvertFromPixels(this.currentPageHeight, PdfGraphicsUnit.Point);
    this.DrawNewLine();
    if (!str2.Contains("www") && !str2.Contains("http") && !this.IsValidEmail(str2) && !str2.Contains(".com"))
      return;
    float emSize = (float) this.TextLineMatrix.M11 * this.FontSize;
    Font font = new Font(this.CurrentFont, emSize);
    SizeF sizeF = this.PageGraphics.MeasureString(str2, font);
    double textLeading = (double) this.TextLeading;
    Matrix matrix2 = this.CTRM * this.TextLineMatrix * new Matrix(1.0, 0.0, 0.0, 1.0, 0.0, -(double) num);
    this.currentTransformLocation = new PointF((float) matrix2.OffsetX, (float) -(matrix2.OffsetY + (double) emSize));
    Regex regex = new Regex("\\b(?:http://|https://|www\\.)\\S+\\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    string URI = string.Empty;
    foreach (Capture match in regex.Matches(str2))
      URI = match.Value;
    if (this.IsValidEmail(textElement.renderedText) || str2.Contains("@"))
      URI = "mailto:" + str2;
    this.URLDictonary.Add(new PageURL(this.transformMatrix, URI, new PointF(this.currentTransformLocation.X, this.currentTransformLocation.Y), sizeF.Width, sizeF.Height));
  }

  private List<Glyph> ReplaceLigatureFromGlyphList(List<Glyph> textElementList)
  {
    for (int index = 0; index < textElementList.Count; ++index)
    {
      string str = textElementList[index].ToString();
      switch (str)
      {
        case null:
          continue;
        case "ﬀ":
          switch (str)
          {
            case "ﬀ":
              str = "ff";
              break;
          }
          if (str != null)
          {
            Rect boundingRect = textElementList[index].BoundingRect;
            char[] charArray = str.ToCharArray();
            Glyph textElement = textElementList[index];
            textElement.ToUnicode = charArray[0].ToString();
            textElement.BoundingRect = new Rect(boundingRect.X, boundingRect.Y, boundingRect.Width / 2.0, boundingRect.Height);
            Glyph glyph = new Glyph();
            glyph.FontStyle = textElementList[index].FontStyle;
            glyph.FontFamily = textElementList[index].FontFamily;
            glyph.FontSize = textElementList[index].FontSize;
            glyph.CharId = textElementList[index].CharId;
            glyph.ToUnicode = str[1].ToString();
            glyph.BoundingRect = new Rect(boundingRect.X + boundingRect.Width / 2.0, boundingRect.Y, boundingRect.Width / 2.0, boundingRect.Height);
            textElementList.RemoveAt(index);
            textElementList.Insert(index, textElement);
            if (textElementList.Count > index + 1 && textElementList[index + 1].ToUnicode == null)
              textElementList.RemoveAt(index + 1);
            textElementList.Insert(index + 1, glyph);
            ++index;
            continue;
          }
          continue;
        default:
          if (str.Length != 2)
            continue;
          goto case "ﬀ";
      }
    }
    return textElementList;
  }

  private void HTMLRenderTextElementWithLeading(string[] textElements, string tokenType)
  {
    string textToDecode = string.Join("", textElements);
    if (!this.m_resources.ContainsKey(this.CurrentFont))
      return;
    (this.m_resources[this.CurrentFont] as FontStructure).IsSameFont = this.m_resources.isSameFont();
    if ((double) (this.m_resources[this.CurrentFont] as FontStructure).FontSize != (double) this.FontSize)
      (this.m_resources[this.CurrentFont] as FontStructure).FontSize = this.FontSize;
    FontStructure resource = this.m_resources[this.CurrentFont] as FontStructure;
    string text = resource.Decode(textToDecode, this.m_resources.isSameFont());
    TextElement textElement = new TextElement(text, this.DocumentMatrix);
    textElement.FontName = resource.FontName;
    textElement.FontStyle = resource.FontStyle;
    textElement.FontSize = this.FontSize;
    textElement.TextScaling = this.m_textScaling;
    textElement.Font = resource.CurrentFont;
    textElement.FontEncoding = resource.FontEncoding;
    textElement.FontGlyphWidths = resource.FontGlyphWidths;
    textElement.DefaultGlyphWidth = resource.DefaultGlyphWidth;
    textElement.Text = text;
    textElement.IsTransparentText = this.IsTransparentText;
    textElement.UnicodeCharMapTable = resource.UnicodeCharMapTable;
    Dictionary<int, int> fontGlyphWidths = resource.FontGlyphWidths;
    Font currentFont = resource.CurrentFont;
    textElement.IsType1Font = resource.IsType1Font;
    textElement.Is1C = resource.Is1C;
    textElement.IsCID = resource.IsCID;
    textElement.CharacterMapTable = resource.CharacterMapTable;
    textElement.ReverseMapTable = resource.ReverseMapTable;
    textElement.Fontfile2Glyph = resource.GlyphFontFile2;
    textElement.structure = resource;
    textElement.Isembeddedfont = resource.isEmbedded;
    textElement.Ctm = this.CTRM;
    textElement.textLineMatrix = this.TextMatrix;
    textElement.Rise = this.Rise;
    textElement.transformMatrix = this.DocumentMatrix;
    textElement.documentMatrix = this.DocumentMatrix;
    textElement.FontID = this.CurrentFont;
    textElement.OctDecMapTable = resource.OctDecMapTable;
    textElement.TextHorizontalScaling = this.HorizontalScaling;
    textElement.ZapfPostScript = resource.ZapfPostScript;
    textElement.LineWidth = this.MitterLength;
    textElement.RenderingMode = this.RenderingMode;
    textElement.testdict = this.testdict;
    textElement.pageRotation = this.pageRotation;
    textElement.zoomFactor = this.zoomFactor;
    textElement.SubstitutedFontsList = this.substitutedFontsList;
    if (resource.Flags != null)
      textElement.FontFlag = resource.Flags.IntValue;
    if (resource.IsType1Font)
    {
      textElement.IsType1Font = true;
      textElement.differenceTable = resource.differenceTable;
      textElement.differenceMappedTable = resource.DifferencesDictionary;
      textElement.m_cffGlyphs = resource.m_cffGlyphs;
      if (!this.m_glyphDataCollection.Contains(textElement.m_cffGlyphs))
        this.m_glyphDataCollection.Add(textElement.m_cffGlyphs);
    }
    textElement.PathBrush = this.StrokingBrush == null ? new Pen(Color.Black).Brush : this.StrokingBrush;
    if (this.NonStrokingBrush != null)
      textElement.PathNonStrokeBrush = this.NonStrokingBrush;
    textElement.WordSpacing = this.Objects.WordSpacing;
    textElement.CharacterSpacing = this.Objects.CharacterSpacing;
    if ((double) this.TextScaling != 100.0)
      textElement.Textscalingfactor = this.TextScaling / 100f;
    if (this.m_beginText)
      this.m_beginText = false;
    Matrix txtMatrix = new Matrix();
    if (resource.fontType != (object) "Type3")
    {
      if (this.m_isCurrentPositionChanged)
      {
        this.m_isCurrentPositionChanged = false;
        this.m_endTextPosition = this.CurrentLocation;
        this.m_textElementWidth = textElement.Render(this.PageGraphics, new PointF(this.m_endTextPosition.X, this.m_endTextPosition.Y + (float) (-(double) this.TextLeading / 4.0)), (double) this.m_textScaling, fontGlyphWidths, (double) resource.Type1GlyphHeight, resource.differenceTable, resource.DifferencesDictionary, resource.differenceEncoding, out txtMatrix);
        if (textElement.Fontfile2Glyph != null && textElement.Fontfile2Glyph.IsFontFile2)
          textElement.textElementGlyphList = this.ReplaceLigatureFromGlyphList(textElement.textElementGlyphList);
        this.imageRenderGlyphList.AddRange((IEnumerable<Glyph>) textElement.textElementGlyphList);
        textElement.textElementGlyphList.Clear();
      }
      else
      {
        this.m_endTextPosition = new PointF(this.m_endTextPosition.X + this.m_textElementWidth, this.m_endTextPosition.Y);
        this.m_textElementWidth = textElement.Render(this.PageGraphics, new PointF(this.m_endTextPosition.X, this.m_endTextPosition.Y + (float) (-(double) this.TextLeading / 4.0)), (double) this.m_textScaling, fontGlyphWidths, (double) resource.Type1GlyphHeight, resource.differenceTable, resource.DifferencesDictionary, resource.differenceEncoding, out txtMatrix);
        if (textElement.Fontfile2Glyph != null && textElement.Fontfile2Glyph.IsFontFile2)
          textElement.textElementGlyphList = this.ReplaceLigatureFromGlyphList(textElement.textElementGlyphList);
        this.imageRenderGlyphList.AddRange((IEnumerable<Glyph>) textElement.textElementGlyphList);
        textElement.textElementGlyphList.Clear();
      }
    }
    else
    {
      string str1 = string.Join("", textElements);
      string str2 = str1.Substring(1, str1.Length - 2);
      Matrix textLineMatrix = this.TextLineMatrix;
      foreach (char ch in str2)
        this.RenderType3GlyphImages(resource, ch.ToString());
      this.TextLineMatrix = textLineMatrix;
    }
    this.TextMatrix = txtMatrix;
  }

  private void RenderTextElementWithSpacing(string[] textElements, string tokenType)
  {
    List<string> stringList1 = new List<string>();
    string str1 = string.Join("", textElements);
    if (!this.m_resources.ContainsKey(this.CurrentFont))
      return;
    (this.m_resources[this.CurrentFont] as FontStructure).IsSameFont = this.m_resources.isSameFont();
    if ((double) (this.m_resources[this.CurrentFont] as FontStructure).FontSize != (double) this.FontSize)
      (this.m_resources[this.CurrentFont] as FontStructure).FontSize = this.FontSize;
    FontStructure structure = this.m_resources[this.CurrentFont] as FontStructure;
    List<float> characterSpacings = (List<float>) null;
    List<string> stringList2 = structure.DecodeTextTJ(str1, this.m_resources.isSameFont());
    byte[] bytes = Encoding.Unicode.GetBytes(structure.ToGetEncodedText(str1, this.m_resources.isSameFont()));
    Dictionary<int, int> dictionary = new Dictionary<int, int>();
    int key = 0;
    for (int index = 0; index < bytes.Length; index += 2)
    {
      dictionary.Add(key, (int) bytes[index]);
      ++key;
    }
    TextElement textElement = new TextElement(str1, this.DocumentMatrix);
    textElement.IsExtractTextData = this.m_isExtractTextData;
    textElement.FontName = structure.FontName;
    textElement.FontStyle = structure.FontStyle;
    textElement.Font = structure.CurrentFont;
    textElement.FontSize = this.FontSize;
    textElement.TextScaling = this.m_textScaling;
    textElement.EncodedTextBytes = dictionary;
    textElement.FontEncoding = structure.FontEncoding;
    textElement.FontGlyphWidths = structure.FontGlyphWidths;
    textElement.DefaultGlyphWidth = structure.DefaultGlyphWidth;
    textElement.RenderingMode = this.RenderingMode;
    textElement.IsFindText = this.isFindText;
    textElement.IsPdfium = this.IsPdfium;
    textElement.UnicodeCharMapTable = structure.UnicodeCharMapTable;
    Dictionary<int, int> fontGlyphWidths = structure.FontGlyphWidths;
    textElement.CidToGidReverseMapTable = structure.CidToGidReverseMapTable;
    textElement.IsType1Font = structure.IsType1Font;
    textElement.Is1C = structure.Is1C;
    textElement.IsCID = structure.IsCID;
    textElement.CharacterMapTable = structure.CharacterMapTable;
    textElement.ReverseMapTable = structure.ReverseMapTable;
    textElement.GlyfDatapath = structure.Graphic;
    textElement.Fontfile2Glyph = structure.GlyphFontFile2;
    textElement.structure = structure;
    textElement.Isembeddedfont = structure.isEmbedded;
    textElement.Ctm = this.CTRM;
    textElement.textLineMatrix = this.TextMatrix;
    textElement.Rise = this.Rise;
    textElement.transformMatrix = this.DocumentMatrix;
    textElement.documentMatrix = this.DocumentMatrix;
    textElement.FontID = this.CurrentFont;
    textElement.OctDecMapTable = structure.OctDecMapTable;
    textElement.TextHorizontalScaling = this.HorizontalScaling;
    textElement.ZapfPostScript = structure.ZapfPostScript;
    textElement.LineWidth = this.MitterLength;
    textElement.RenderingMode = this.RenderingMode;
    textElement.testdict = this.testdict;
    textElement.pageRotation = this.pageRotation;
    textElement.zoomFactor = this.zoomFactor;
    textElement.SubstitutedFontsList = this.substitutedFontsList;
    if (structure.BaseFontEncoding == "WinAnsiEncoding")
      this.isWinAnsiEncoding = true;
    if (structure.Flags != null)
      textElement.FontFlag = structure.Flags.IntValue;
    if (structure.IsType1Font)
    {
      textElement.IsType1Font = true;
      textElement.differenceTable = structure.differenceTable;
      textElement.differenceMappedTable = structure.DifferencesDictionary;
      textElement.m_cffGlyphs = structure.m_cffGlyphs;
      if (!this.m_glyphDataCollection.Contains(textElement.m_cffGlyphs))
        this.m_glyphDataCollection.Add(textElement.m_cffGlyphs);
    }
    if (this.StrokingBrush != null)
    {
      if (textElement.RenderingMode == 3)
      {
        textElement.PathBrush = this.StrokingBrush;
      }
      else
      {
        textElement.PathBrush = this.StrokingBrush;
        if (this.isXGraphics)
        {
          Color color = ((SolidBrush) this.StrokingBrush).Color;
          float num = Math.Max(0.0f, Math.Min(1f, this.StrokingOpacity));
          int alpha = (int) Math.Floor((double) num == 1.0 ? (double) byte.MaxValue : (double) num * (double) byte.MaxValue);
          ((SolidBrush) textElement.PathBrush).Color = Color.FromArgb(alpha, (int) color.R, (int) color.G, (int) color.B);
        }
      }
    }
    else
      textElement.PathBrush = new Pen(Color.Black).Brush;
    if (this.NonStrokingBrush != null)
      textElement.PathNonStrokeBrush = this.NonStrokingBrush;
    textElement.WordSpacing = this.Objects.WordSpacing;
    textElement.CharacterSpacing = this.Objects.CharacterSpacing;
    if (this.m_beginText)
      this.m_beginText = false;
    Matrix textmatrix = new Matrix();
    if (structure.fontType.Value != "Type3")
    {
      if (this.m_isCurrentPositionChanged)
      {
        this.m_isCurrentPositionChanged = false;
        this.m_endTextPosition = this.CurrentLocation;
        this.m_textElementWidth = textElement.RenderWithSpace(this.PageGraphics, new PointF(this.m_endTextPosition.X, this.m_endTextPosition.Y - this.FontSize), stringList2, characterSpacings, (double) this.m_textScaling, fontGlyphWidths, (double) structure.Type1GlyphHeight, structure.differenceTable, structure.DifferencesDictionary, structure.differenceEncoding, out textmatrix);
        if (textElement.Fontfile2Glyph != null && textElement.Fontfile2Glyph.IsFontFile2)
          textElement.textElementGlyphList = this.ReplaceLigatureFromGlyphList(textElement.textElementGlyphList);
        this.imageRenderGlyphList.AddRange((IEnumerable<Glyph>) textElement.textElementGlyphList);
        if (this.isExtractLineCollection)
          this.extractTextElement.Add(textElement);
        textElement.textElementGlyphList.Clear();
      }
      else
      {
        this.m_endTextPosition = new PointF(this.m_endTextPosition.X + this.m_textElementWidth, this.m_endTextPosition.Y);
        this.m_textElementWidth = textElement.RenderWithSpace(this.PageGraphics, new PointF(this.m_endTextPosition.X, this.m_endTextPosition.Y - this.FontSize), stringList2, characterSpacings, (double) this.m_textScaling, fontGlyphWidths, (double) structure.Type1GlyphHeight, structure.differenceTable, structure.DifferencesDictionary, structure.differenceEncoding, out textmatrix);
        if (textElement.Fontfile2Glyph != null && textElement.Fontfile2Glyph.IsFontFile2)
          textElement.textElementGlyphList = this.ReplaceLigatureFromGlyphList(textElement.textElementGlyphList);
        this.imageRenderGlyphList.AddRange((IEnumerable<Glyph>) textElement.textElementGlyphList);
        textElement.textElementGlyphList.Clear();
      }
    }
    else
    {
      Matrix textLineMatrix = this.TextLineMatrix;
      FontStructure fontStructure = structure;
      this.PageGraphics.Transform = new System.Drawing.Drawing2D.Matrix((float) this.DocumentMatrix.M11, (float) this.DocumentMatrix.M12, (float) this.DocumentMatrix.M21, (float) this.DocumentMatrix.M22, (float) this.DocumentMatrix.OffsetX, (float) this.DocumentMatrix.OffsetY);
      this.PageGraphics.TranslateTransform(0.0f, (float) this.TextLineMatrix.OffsetY);
      this.m_spacingWidth = 0.0f;
      this.RenderType3GlyphImagesTJ(structure, stringList2);
      this.TextLineMatrix = textLineMatrix;
      structure = fontStructure;
    }
    this.TextMatrix = textmatrix;
    string str2 = textElement.renderedText;
    if (!structure.IsMappingDone)
    {
      if (structure.CharacterMapTable != null && structure.CharacterMapTable.Count > 0)
        str2 = structure.MapCharactersFromTable(str2);
      else if (structure.DifferencesDictionary != null && structure.DifferencesDictionary.Count > 0)
        str2 = structure.MapDifferences(str2);
    }
    Matrix matrix1 = this.CTRM * this.TextLineMatrix;
    float num1 = new PdfUnitConvertor().ConvertFromPixels(this.currentPageHeight, PdfGraphicsUnit.Point);
    if (!str2.Contains("www") && !str2.Contains("http") && !this.IsValidEmail(str2) && !str2.Contains(".com"))
      return;
    float emSize = 0.0f;
    if (this.TextLineMatrix.M11 != 0.0 && this.TextLineMatrix.M22 != 0.0)
    {
      emSize = (float) this.TextLineMatrix.M11 * this.FontSize;
      if (this.TextLineMatrix.M11 <= 0.0)
        emSize = this.FontSize;
    }
    else if (this.TextLineMatrix.M12 != 0.0 && this.TextLineMatrix.M21 != 0.0)
    {
      emSize = (float) this.TextLineMatrix.M12 * this.FontSize;
      if (this.TextLineMatrix.M12 <= 0.0)
        emSize = this.FontSize;
    }
    Font font = new Font(this.CurrentFont, emSize);
    SizeF sizeF = this.PageGraphics.MeasureString(str2, font);
    double textLeading = (double) this.TextLeading;
    Matrix matrix2 = this.CTRM * this.TextLineMatrix;
    if (this.TextLineMatrix.M12 != 0.0 && this.TextLineMatrix.M21 != 0.0)
    {
      matrix2.OffsetX = this.TextLineMatrix.OffsetY;
      matrix2.OffsetY = -this.TextLineMatrix.OffsetX;
    }
    else
    {
      matrix2.OffsetX = this.TextLineMatrix.OffsetX;
      matrix2.OffsetY = this.TextLineMatrix.OffsetY;
      matrix2 *= new Matrix(1.0, 0.0, 0.0, 1.0, 0.0, -(double) num1);
    }
    this.currentTransformLocation = new PointF((float) matrix2.OffsetX, (float) -(matrix2.OffsetY + (double) emSize));
    Regex regex = new Regex("\\b(?:http://|https://|www\\.)\\S+\\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    string URI = string.Empty;
    foreach (Capture match in regex.Matches(str2))
      URI = match.Value;
    if (this.IsValidEmail(textElement.renderedText) || str2.Contains("@"))
      URI = "mailto:" + str2;
    this.URLDictonary.Add((double) this.m_endTextPosition.Y + (-(double) this.TextLeading - (double) this.FontSize) == 0.0 ? new PageURL(this.transformMatrix, URI, new PointF(this.m_endTextPosition.X, this.m_endTextPosition.Y), this.m_textElementWidth, this.FontSize) : new PageURL(this.transformMatrix, URI, new PointF(this.currentTransformLocation.X, this.currentTransformLocation.Y), sizeF.Width, sizeF.Height));
  }

  public bool IsValidEmail(string email)
  {
    Regex regex = new Regex("^[-a-zA-Z0-9][-.a-zA-Z0-9]*@[-.a-zA-Z0-9]+(\\.[-.a-zA-Z0-9]+)*\\.(com|edu|info|gov|int|mil|net|org|biz|name|museum|coop|aero|pro|tv|[a-zA-Z]{2})$", RegexOptions.IgnorePatternWhitespace);
    return !string.IsNullOrEmpty(email) && regex.IsMatch(email);
  }

  private Color GetColor(string[] colorElement, string type, string colorSpace)
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    float num3 = 0.0f;
    float num4 = 1f;
    if (colorSpace == "RGB" && colorElement.Length == 3)
    {
      num1 = this.FloatParse(colorElement[0]);
      num2 = this.FloatParse(colorElement[1]);
      num3 = this.FloatParse(colorElement[2]);
      num4 = this.m_opacity;
    }
    else if (colorSpace == "Gray" && colorElement.Length == 1)
    {
      double num5;
      num3 = (float) (num5 = !colorElement[0].Contains("/") ? (!this.isBlack ? (double) this.FloatParse(colorElement[0]) : 0.0) : 0.0);
      num2 = (float) num5;
      num1 = (float) num5;
      num4 = this.m_opacity;
    }
    else if (colorSpace == "DeviceCMYK" && colorElement.Length == 4)
    {
      float result1;
      float.TryParse(colorElement[0], out result1);
      float result2;
      float.TryParse(colorElement[1], out result2);
      float result3;
      float.TryParse(colorElement[2], out result3);
      float result4;
      float.TryParse(colorElement[3], out result4);
      return this.ConvertCMYKtoRGB(result1, result2, result3, result4);
    }
    return Color.FromArgb((int) (byte) ((double) num4 * (double) byte.MaxValue), (int) (byte) ((double) num1 * (double) byte.MaxValue), (int) (byte) ((double) num2 * (double) byte.MaxValue), (int) (byte) ((double) num3 * (double) byte.MaxValue));
  }

  private Color ConvertCMYKtoRGB(float c, float m, float y, float k)
  {
    float num1 = (float) ((double) c * (-4.3873323846099881 * (double) c + 54.486151941891762 * (double) m + 18.822905021653021 * (double) y + 212.25662451639585 * (double) k - 285.2331026137004) + (double) m * (1.7149763477362134 * (double) m - 5.6096736904047315 * (double) y + -17.873870861415444 * (double) k - 5.4970064271963661) + (double) y * (-2.5217340131683033 * (double) y - 21.248923337353073 * (double) k + 17.5119270841813) + (double) k * (-21.86122147463605 * (double) k - 189.48180835922747) + (double) byte.MaxValue);
    float num2 = (float) ((double) c * (8.8410414220361488 * (double) c + 60.118027045597366 * (double) m + 6.8714255920490066 * (double) y + 31.159100130055922 * (double) k - 79.2970844816548) + (double) m * (-15.310361306967817 * (double) m + 17.575251261109482 * (double) y + 131.35250912493976 * (double) k - 190.9453302588951) + (double) y * (4.444339102852739 * (double) y + 9.8632861493405 * (double) k - 24.86741582555878) + (double) k * (-20.737325471181034 * (double) k - 187.80453709719578) + (double) byte.MaxValue);
    float num3 = (float) ((double) c * (0.88425224300032956 * (double) c + 8.0786775031129281 * (double) m + 30.89978309703729 * (double) y - 0.23883238689178934 * (double) k - 14.183576799673286) + (double) m * (10.49593273432072 * (double) m + 63.02378494754052 * (double) y + 50.606957656360734 * (double) k - 112.23884253719248) + (double) y * (0.032960411148732167 * (double) y + 115.60384449646641 * (double) k - 193.58209356861505) + (double) k * (-22.33816807309886 * (double) k - 180.12613974708367) + (double) byte.MaxValue);
    return Color.FromArgb((int) byte.MaxValue, (double) num1 > (double) byte.MaxValue ? (int) byte.MaxValue : ((double) num1 < 0.0 ? 0 : (int) num1), (double) num2 > (double) byte.MaxValue ? (int) byte.MaxValue : ((double) num2 < 0.0 ? 0 : (int) num2), (double) num3 > (double) byte.MaxValue ? (int) byte.MaxValue : ((double) num3 < 0.0 ? 0 : (int) num3));
  }

  private void DrawNewLine()
  {
    this.m_isCurrentPositionChanged = true;
    if (!(this.m_resources[this.CurrentFont] is FontStructure resource))
      return;
    Font font = new Font(TextElement.CheckFontName(resource.FontName), this.FontSize, resource.FontStyle);
    if (-(double) this.TextLeading != 0.0)
      this.m_currentLocation.Y = -(double) this.TextLeading < 0.0 ? this.m_currentLocation.Y - -this.TextLeading : this.m_currentLocation.Y + -this.TextLeading;
    else
      this.m_currentLocation.Y += font.Size;
    font.Dispose();
  }

  private void GetClipRectangle(string[] rectangle)
  {
    if (!this.m_selectablePrintDocument)
    {
      float x = float.Parse(rectangle[0]);
      float y = float.Parse(rectangle[1]);
      float width = float.Parse(rectangle[2]);
      float height = float.Parse(rectangle[3]);
      if ((double) x < 0.0 && (double) height < 0.0 && this.isNextFill && -(double) height >= (double) this.currentPageHeight)
      {
        x = 0.0f;
        height = 0.0f;
      }
      this.isNextFill = false;
      this.BeginPath(x, y);
      this.AddLine(x + width, y);
      this.AddLine(x + width, y + height);
      this.AddLine(x, y + height);
      this.EndPath();
      RectangleF rectangleF = new RectangleF(x, y, width, height);
    }
    else
    {
      this.clipRectShape = rectangle;
      this.isRect = true;
      float x = float.Parse(rectangle[0]);
      float y = -float.Parse(rectangle[1]);
      float width = float.Parse(rectangle[2]);
      float height = -float.Parse(rectangle[3]);
      RectangleF rect = new RectangleF(x, y, width, height);
      bool flag1 = false;
      bool flag2 = false;
      if ((double) height < 0.0)
      {
        height = -height;
        y -= height;
        flag1 = true;
      }
      if ((double) width < 0.0)
      {
        width = -width;
        x -= width;
        flag2 = true;
      }
      if (flag1 && flag2)
        rect = new RectangleF(x, y, width, height);
      if (this.ClipRectangle != RectangleF.Empty)
        this.m_clipRectangle.Intersect(rect);
      else
        this.m_clipRectangle = new RectangleF(x, y, width, height);
      this.Path = new GraphicsPath();
      GraphicsPath graphicsPath = new GraphicsPath();
      graphicsPath.AddRectangle(rect);
      this.m_tempSubPaths.Add(graphicsPath);
    }
  }

  private void GetWordSpacing(string[] spacing)
  {
    this.Objects.WordSpacing = this.FloatParse(spacing[0]);
  }

  private void GetCharacterSpacing(string[] spacing)
  {
    this.Objects.CharacterSpacing = this.FloatParse(spacing[0]);
    this.m_characterSpacing = this.Objects.CharacterSpacing;
  }

  private void GetScalingFactor(string[] scaling)
  {
    this.m_textScaling = this.FloatParse(scaling[0]);
    this.HorizontalScaling = this.FloatParse(scaling[0]);
  }

  private void DrawPath()
  {
    Pen pen = new Pen(this.NonStrokingBrush == null ? new Pen(Color.Black).Brush : this.NonStrokingBrush);
    Matrix matrix1 = new Matrix((double) this.Drawing2dMatrixCTM.Elements[0], (double) this.Drawing2dMatrixCTM.Elements[1], (double) this.Drawing2dMatrixCTM.Elements[2], (double) this.Drawing2dMatrixCTM.Elements[3], (double) this.Drawing2dMatrixCTM.OffsetX, (double) this.Drawing2dMatrixCTM.OffsetY) * this.DocumentMatrix;
    System.Drawing.Drawing2D.Matrix matrix2 = new System.Drawing.Drawing2D.Matrix((float) matrix1.M11, (float) matrix1.M12, (float) matrix1.M21, (float) matrix1.M22, (float) matrix1.OffsetX, (float) matrix1.OffsetY);
    System.Drawing.Drawing2D.Matrix matrix3 = new System.Drawing.Drawing2D.Matrix();
    matrix3.Multiply(matrix2);
    System.Drawing.Drawing2D.Matrix transform = this.PageGraphics.Transform;
    GraphicsUnit pageUnit = this.PageGraphics.PageUnit;
    this.PageGraphics.PageUnit = GraphicsUnit.Pixel;
    this.PageGraphics.Transform = new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
    this.PageGraphics.Transform = matrix3;
    GraphicsPath geometry = this.GetGeometry(this.CurrentGeometry, new Matrix(1.0, 0.0, 0.0, 1.0, 0.0, 0.0));
    if ((double) this.MitterLength != 0.0)
      pen.Width = this.MitterLength;
    if (this.m_dashedLine != null)
    {
      string str1 = this.m_dashedLine[0];
      string str2 = str1.Substring(1, str1.Length - 2).Trim();
      List<string> stringList = new List<string>();
      string str3 = str2;
      char[] chArray = new char[1]{ ' ' };
      foreach (string str4 in str3.Split(chArray))
      {
        if (str4 == "0")
          stringList.Add("0.000000001");
        else if (str4 != "")
          stringList.Add(str4);
      }
      float[] numArray = new float[stringList.Count];
      for (int index = 0; index < stringList.Count; ++index)
      {
        if (stringList[index] != "")
          numArray[index] = float.Parse(stringList[index]);
      }
      if (numArray.Length > 0 && (double) this.MitterLength < (double) numArray[0] && (double) this.MitterLength != 0.0)
      {
        for (int index = 0; index < numArray.Length; ++index)
          numArray[index] = numArray[index] / this.MitterLength;
      }
      if (numArray.Length > 0)
        pen.DashPattern = numArray;
      if ((double) this.m_lineCap == 1.0)
        pen.DashCap = DashCap.Round;
      this.m_dashedLine = (string[]) null;
    }
    geometry.FillMode = FillMode.Alternate;
    if (this.IsGraphicsState)
    {
      if ((double) this.NonStrokingOpacity == 0.0)
        this.PageGraphics.DrawPath(Pens.Transparent, geometry);
      else
        this.PageGraphics.DrawPath(pen, geometry);
    }
    else if ((double) this.NonStrokingOpacity == 0.0)
      this.PageGraphics.DrawPath(Pens.Transparent, geometry);
    else
      this.PageGraphics.DrawPath(pen, geometry);
    this.CurrentGeometry = new PathGeometry();
    this.m_currentPath = (PathFigure) null;
    this.PageGraphics.Transform = transform;
    this.PageGraphics.PageUnit = pageUnit;
  }

  private void FillPath(string mode)
  {
    Pen pen = (Pen) null;
    if (!(this.StrokingBrush is TextureBrush))
      pen = this.StrokingBrush == null ? new Pen(Color.Black) : new Pen(this.StrokingBrush);
    else if (this.StrokingBrush is TextureBrush && this.StrokingBrush != null && this.m_tilingType == 3)
      (this.StrokingBrush as TextureBrush).MultiplyTransform(new System.Drawing.Drawing2D.Matrix(this.Drawing2dMatrixCTM.Elements[0], this.Drawing2dMatrixCTM.Elements[1], this.Drawing2dMatrixCTM.Elements[2], this.Drawing2dMatrixCTM.Elements[3], 1f, 1f));
    System.Drawing.Drawing2D.Matrix transform;
    GraphicsUnit pageUnit;
    if (!this.m_isType3Font)
    {
      Matrix documentMatrix = this.DocumentMatrix;
      Matrix matrix1 = new Matrix((double) this.Drawing2dMatrixCTM.Elements[0], (double) this.Drawing2dMatrixCTM.Elements[1], (double) this.Drawing2dMatrixCTM.Elements[2], (double) this.Drawing2dMatrixCTM.Elements[3], (double) this.Drawing2dMatrixCTM.OffsetX, (double) this.Drawing2dMatrixCTM.OffsetY);
      if (matrix1.M11 < 0.0 && matrix1.OffsetY < 0.0)
        matrix1 = Matrix.Identity;
      Matrix matrix2 = matrix1 * documentMatrix;
      System.Drawing.Drawing2D.Matrix matrix3 = new System.Drawing.Drawing2D.Matrix((float) matrix2.M11, (float) matrix2.M12, (float) matrix2.M21, (float) matrix2.M22, (float) matrix2.OffsetX, (float) matrix2.OffsetY);
      System.Drawing.Drawing2D.Matrix matrix4 = new System.Drawing.Drawing2D.Matrix();
      matrix4.Multiply(matrix3);
      transform = this.PageGraphics.Transform;
      pageUnit = this.PageGraphics.PageUnit;
      this.PageGraphics.PageUnit = GraphicsUnit.Pixel;
      this.PageGraphics.Transform = new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
      this.PageGraphics.Transform = matrix4;
    }
    else if (!this.m_istype3FontContainCTRM)
    {
      Matrix matrix5 = new Matrix()
      {
        M11 = (double) this.FontSize,
        M22 = -(double) this.FontSize,
        OffsetY = (double) this.FontSize
      } * this.TextLineMatrix * this.CTRM;
      Matrix identity = Matrix.Identity;
      identity.Scale(0.01, 0.01, 0.0, 0.0);
      identity.Translate(0.0, 1.0);
      this.m_transformations.PushTransform(identity * matrix5);
      System.Drawing.Drawing2D.Matrix matrix6 = new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f).Clone();
      matrix6.Multiply(this.GetTransformationMatrix(this.m_transformations.CurrentTransform));
      transform = this.PageGraphics.Transform;
      pageUnit = this.PageGraphics.PageUnit;
      this.PageGraphics.PageUnit = GraphicsUnit.Pixel;
      this.PageGraphics.Transform = new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
      this.PageGraphics.Transform = matrix6;
    }
    else
    {
      Matrix matrix7 = new Matrix()
      {
        M11 = (double) this.FontSize,
        M22 = -(double) this.FontSize,
        OffsetY = (double) this.FontSize
      } * this.TextLineMatrix;
      Matrix identity = Matrix.Identity;
      identity.Scale(1.0, 1.0, 0.0, 0.0);
      this.m_transformations.PushTransform(identity * matrix7);
      System.Drawing.Drawing2D.Matrix matrix8 = new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f).Clone();
      matrix8.Multiply(this.GetTransformationMatrix(this.m_transformations.CurrentTransform));
      float element = this.PageGraphics.Transform.Elements[5];
      transform = this.PageGraphics.Transform;
      pageUnit = this.PageGraphics.PageUnit;
      this.PageGraphics.PageUnit = GraphicsUnit.Pixel;
      this.PageGraphics.Transform = new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
      this.PageGraphics.Transform = new System.Drawing.Drawing2D.Matrix(matrix8.Elements[0] * (float) this.CTRM.M11 * this.FontSize, matrix8.Elements[1], matrix8.Elements[2], matrix8.Elements[3] * (float) this.CTRM.M22 * this.FontSize, matrix8.Elements[4], element);
    }
    foreach (PathFigure figure in this.CurrentGeometry.Figures)
    {
      figure.IsClosed = true;
      figure.IsFilled = true;
    }
    this.CurrentGeometry.FillRule = mode == "Winding" ? FillRule.Nonzero : FillRule.EvenOdd;
    GraphicsPath path = !this.m_isType3Font ? this.GetGeometry(this.CurrentGeometry, new Matrix(1.0, 0.0, 0.0, 1.0, 0.0, 0.0)) : this.GetGeometry(this.CurrentGeometry, new Matrix(0.1, 0.0, 0.0, -0.1, 0.0, 0.0));
    path.FillMode = this.CurrentGeometry.FillRule == FillRule.Nonzero ? FillMode.Winding : FillMode.Alternate;
    if (this.IsGraphicsState)
    {
      if (this.StrokingBrush is TextureBrush)
        this.PageGraphics.FillPath(this.StrokingBrush, path);
      else if (this.StrokingBrush is LinearGradientBrush || this.StrokingBrush is PathGradientBrush)
      {
        this.PageGraphics.FillPath(pen.Brush, path);
      }
      else
      {
        Color color = pen.Color;
        float num = (double) this.m_opacity == (double) this.StrokingOpacity ? Math.Max(0.0f, Math.Min(1f, this.m_opacity)) : Math.Max(0.0f, Math.Min(1f, this.StrokingOpacity));
        int alpha = (int) Math.Floor((double) num == 1.0 ? (double) byte.MaxValue : (double) num * (double) byte.MaxValue);
        pen.Color = Color.FromArgb(alpha, (int) color.R, (int) color.G, (int) color.B);
        this.PageGraphics.FillPath(pen.Brush, path);
      }
    }
    else if (this.StrokingBrush is TextureBrush)
      this.PageGraphics.FillPath(this.StrokingBrush, path);
    else
      this.PageGraphics.FillPath(pen.Brush, path);
    this.CurrentGeometry = new PathGeometry();
    this.m_currentPath = (PathFigure) null;
    this.PageGraphics.Transform = transform;
    this.PageGraphics.PageUnit = pageUnit;
    float num1;
    if (this.m_d1Matrix.M11 == 0.0)
    {
      num1 = (float) (this.m_d1Matrix.M21 - this.m_d1Matrix.OffsetX);
      if ((double) num1 < 0.0)
        num1 = -num1;
    }
    else
      num1 = (float) this.m_d1Matrix.M11;
    if (!this.m_isType3Font)
      return;
    this.TextLineMatrix = this.CalculateType3TextMatrixupdate(this.TextLineMatrix, num1 * this.m_type3FontScallingFactor, true);
  }

  private Matrix CalculateTextMatrixupdate(Matrix m, float Width, bool isImage)
  {
    return new Matrix(1.0, 0.0, 0.0, 1.0, !isImage ? (double) Width * (double) this.FontSize * 0.001 : (double) Width * 1.0, 0.0) * m;
  }

  private System.Drawing.Drawing2D.Matrix GetTransformationMatrix(Matrix transform)
  {
    return new System.Drawing.Drawing2D.Matrix((float) transform.M11, (float) transform.M12, (float) transform.M21, (float) transform.M22, (float) transform.OffsetX, (float) transform.OffsetY);
  }

  private void AddLine(string[] line)
  {
    this.CurrentLocation = new PointF(this.FloatParse(line[0]), this.FloatParse(line[1]));
    this.m_currentPath.Segments.Add((PathSegment) new LineSegment()
    {
      Point = (Point) this.CurrentLocation
    });
  }

  private void AddBezierCurve(string[] curve)
  {
    this.m_currentPath.Segments.Add((PathSegment) new BezierSegment()
    {
      Point1 = (Point) new PointF(this.FloatParse(curve[0]), this.FloatParse(curve[1])),
      Point2 = (Point) new PointF(this.FloatParse(curve[2]), this.FloatParse(curve[3])),
      Point3 = (Point) new PointF(this.FloatParse(curve[4]), this.FloatParse(curve[5]))
    });
    this.CurrentLocation = new PointF(this.FloatParse(curve[4]), this.FloatParse(curve[5]));
  }

  private void AddBezierCurve2(string[] curve)
  {
    this.m_currentPath.Segments.Add((PathSegment) new BezierSegment()
    {
      Point1 = (Point) this.CurrentLocation,
      Point2 = (Point) new PointF(this.FloatParse(curve[0]), this.FloatParse(curve[1])),
      Point3 = (Point) new PointF(this.FloatParse(curve[2]), this.FloatParse(curve[3]))
    });
    this.CurrentLocation = new PointF(this.FloatParse(curve[2]), this.FloatParse(curve[3]));
  }

  private void AddBezierCurve3(string[] curve)
  {
    this.m_currentPath.Segments.Add((PathSegment) new BezierSegment()
    {
      Point1 = (Point) new PointF(this.FloatParse(curve[0]), this.FloatParse(curve[1])),
      Point2 = (Point) new PointF(this.FloatParse(curve[2]), this.FloatParse(curve[3])),
      Point3 = (Point) new PointF(this.FloatParse(curve[2]), this.FloatParse(curve[3]))
    });
    this.CurrentLocation = new PointF(this.FloatParse(curve[2]), this.FloatParse(curve[3]));
  }

  private void BeginPath(string[] point)
  {
    this.CurrentLocation = new PointF(this.FloatParse(point[0]), this.FloatParse(point[1]));
    if (this.m_currentPath != null && this.m_currentPath.Segments.Count == 0)
      this.CurrentGeometry.Figures.Remove(this.CurrentPath);
    this.m_currentPath = new PathFigure();
    this.m_currentPath.StartPoint = (Point) this.CurrentLocation;
    this.CurrentGeometry.Figures.Add(this.m_currentPath);
  }

  private void BeginPath(float x, float y)
  {
    this.CurrentLocation = new PointF(x, y);
    if (this.m_currentPath != null && this.m_currentPath.Segments.Count == 0)
      this.CurrentGeometry.Figures.Remove(this.CurrentPath);
    this.m_currentPath = new PathFigure();
    this.m_currentPath.StartPoint = (Point) this.CurrentLocation;
    this.CurrentGeometry.Figures.Add(this.m_currentPath);
  }

  private void EndPath()
  {
    if (this.m_currentPath == null)
      return;
    this.m_currentPath.IsClosed = true;
  }

  private void AddLine(float x, float y)
  {
    this.CurrentLocation = new PointF(x, y);
    this.m_currentPath.Segments.Add((PathSegment) new LineSegment()
    {
      Point = (Point) this.CurrentLocation
    });
  }

  public GraphicsPath GetGeometry(PathGeometry geometry, Matrix transform)
  {
    System.Drawing.Drawing2D.Matrix transformationMatrix = PdfElementsRenderer.GetTransformationMatrix(transform);
    GraphicsPath graphicsPath = new GraphicsPath();
    foreach (PathFigure figure in geometry.Figures)
    {
      graphicsPath.StartFigure();
      PointF pointF = new PointF((float) figure.StartPoint.X, (float) figure.StartPoint.Y);
      foreach (PathSegment segment in figure.Segments)
      {
        if (segment is LineSegment)
        {
          LineSegment lineSegment = (LineSegment) segment;
          PointF[] pts = new PointF[2]
          {
            pointF,
            new PointF((float) lineSegment.Point.X, (float) lineSegment.Point.Y)
          };
          transformationMatrix.TransformPoints(pts);
          graphicsPath.AddLine(pts[0], pts[1]);
          if ((double) pts[0].X < 0.0 || (double) pts[0].Y < 0.0 || (double) pts[1].X < 0.0 || (double) pts[1].Y < 0.0)
            this.m_isNegative = true;
          pointF = new PointF((float) lineSegment.Point.X, (float) lineSegment.Point.Y);
        }
        else if (segment is BezierSegment)
        {
          BezierSegment bezierSegment = segment as BezierSegment;
          PointF[] pts = new PointF[4]
          {
            pointF,
            new PointF((float) bezierSegment.Point1.X, (float) bezierSegment.Point1.Y),
            new PointF((float) bezierSegment.Point2.X, (float) bezierSegment.Point2.Y),
            new PointF((float) bezierSegment.Point3.X, (float) bezierSegment.Point3.Y)
          };
          transformationMatrix.TransformPoints(pts);
          graphicsPath.AddBezier(pts[0], pts[1], pts[2], pts[3]);
          if ((double) pts[0].X < 0.0 || (double) pts[0].Y < 0.0 || (double) pts[1].X < 0.0 || (double) pts[1].Y < 0.0 || (double) pts[2].X < 0.0 || (double) pts[2].Y < 0.0 || (double) pts[3].X < 0.0 || (double) pts[3].Y < 0.0)
            this.m_isNegative = true;
          pointF = new PointF((float) bezierSegment.Point3.X, (float) bezierSegment.Point3.Y);
        }
      }
      if (figure.IsClosed)
        graphicsPath.CloseFigure();
    }
    return (GraphicsPath) graphicsPath.Clone();
  }

  private void GetXObject(string[] xobjectElement)
  {
    if (!this.m_resources.ContainsKey(xobjectElement[0].Replace("/", "")))
      return;
    if (this.m_resources[xobjectElement[0].Replace("/", "")] is XObjectElement)
    {
      if (!(this.m_resources[xobjectElement[0].Replace("/", "")] is XObjectElement resource))
        return;
      if (this.m_selectablePrintDocument)
      {
        resource.m_isPrintSelected = this.m_selectablePrintDocument;
        resource.m_pageHeight = (double) this.m_pageHeight;
      }
      resource.IsExtractTextData = this.m_isExtractTextData;
      resource.IsExtractLineCollection = this.isExtractLineCollection;
      resource.IsPdfium = this.IsPdfium;
      List<Glyph> glyphList;
      this.m_graphicsState = resource.Render(this.PageGraphics, this.m_resources, this.m_graphicsState, this.m_objects, out glyphList, this.zoomFactor, this.isExportAsImage, this.substitutedFontsList);
      if (glyphList != null)
        glyphList = this.ReplaceLigatureFromGlyphList(glyphList);
      this.imageRenderGlyphList.AddRange((IEnumerable<Glyph>) glyphList);
      if (resource.IsExtractLineCollection)
      {
        if (this.extractTextElement.Count == 0)
        {
          this.extractTextElement = resource.ExtractTextElements;
        }
        else
        {
          for (int index = 0; index < resource.ExtractTextElements.Count; ++index)
            this.extractTextElement.Add(resource.ExtractTextElements[index]);
        }
      }
      glyphList.Clear();
    }
    else
    {
      if (!(this.m_resources[xobjectElement[0].Replace("/", "")] is ImageStructure) || this.IsTextSearch || this.IsPdfiumRendering || this.isExtractLineCollection)
        return;
      ImageStructure resource = this.m_resources[xobjectElement[0].Replace("/", "")] as ImageStructure;
      resource.m_isExtGStateContainsSMask = this.m_isExtendedGraphicStateContainsSMask;
      Image image = resource.EmbeddedImage;
      MemoryStream memoryStream = new MemoryStream();
      System.Drawing.Drawing2D.Matrix transform = this.PageGraphics.Transform;
      GraphicsUnit pageUnit = this.PageGraphics.PageUnit;
      this.PageGraphics.PageUnit = GraphicsUnit.Pixel;
      this.PageGraphics.Transform = new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
      Matrix matrix1 = new Matrix((double) this.Drawing2dMatrixCTM.Elements[0], (double) this.Drawing2dMatrixCTM.Elements[1], (double) this.Drawing2dMatrixCTM.Elements[2], (double) this.Drawing2dMatrixCTM.Elements[3], (double) this.Drawing2dMatrixCTM.OffsetX, (double) this.Drawing2dMatrixCTM.OffsetY);
      matrix1.Scale(1.0, -1.0, 0.0, 1.0);
      Matrix matrix2 = matrix1 * this.DocumentMatrix;
      this.PageGraphics.Transform = matrix2.M11 != 0.0 || matrix2.M12 != 0.0 || matrix2.M21 != 0.0 ? new System.Drawing.Drawing2D.Matrix((float) matrix2.M11, (float) matrix2.M12, (float) matrix2.M21, (float) matrix2.M22, (float) matrix2.OffsetX, (float) Math.Round(matrix2.OffsetY, 5)) : new System.Drawing.Drawing2D.Matrix(0.0001f, (float) matrix2.M12, (float) matrix2.M21, (float) matrix2.M22, (float) matrix2.OffsetX, (float) Math.Round(matrix2.OffsetY, 5));
      if (resource.IsImageMask)
      {
        if (image == null)
          image = this.GetMaskImagefromStream(resource);
        else if (!resource.m_isBlackIs1 && resource.ImageFilter[0] != "DCTDecode")
          image = this.GetMaskImage(image);
      }
      else
      {
        MemoryStream outStream = resource.outStream;
      }
      if (image != null)
      {
        InterpolationMode interpolationMode = this.PageGraphics.InterpolationMode;
        PixelOffsetMode pixelOffsetMode = this.PageGraphics.PixelOffsetMode;
        this.PageGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        this.PageGraphics.PixelOffsetMode = PixelOffsetMode.None;
        using (ImageAttributes imageAttr = new ImageAttributes())
        {
          if (resource.ImageDictionary != null && resource.ImageDictionary.ContainsKey("Intent") && (resource.ImageDictionary["Intent"] as PdfName).Value == "RelativeColorimetric")
          {
            ColorMatrix newColorMatrix = new ColorMatrix();
            newColorMatrix.Matrix33 = this.StrokingOpacity;
            ImageAttributes imageAttributes = new ImageAttributes();
            imageAttr.SetColorMatrix(newColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
          }
          imageAttr.SetWrapMode(WrapMode.TileFlipXY);
          this.PageGraphics.DrawImage(image, new Rectangle(0, 0, 1, 1), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttr);
        }
        this.PageGraphics.PixelOffsetMode = pixelOffsetMode;
        this.PageGraphics.InterpolationMode = interpolationMode;
      }
      this.PageGraphics.Transform = transform;
      this.PageGraphics.PageUnit = pageUnit;
    }
  }

  private void GetType3XObject(string[] xobjectElement)
  {
    if (!this.m_resources.ContainsKey(xobjectElement[0].Replace("/", "")) || !(this.m_resources[xobjectElement[0].Replace("/", "")] is ImageStructure))
      return;
    ImageStructure resource = this.m_resources[xobjectElement[0].Replace("/", "")] as ImageStructure;
    Image image = resource.EmbeddedImage;
    if (resource.IsImageMask)
      image = image != null ? this.GetType3MaskImage(image, resource) : this.GetType3MaskImagefromStream(resource);
    FontStructure type3FontStruct = this.m_type3FontStruct;
    TextElement textElement = new TextElement(image, this.DocumentMatrix);
    textElement.type3GlyphImage = image;
    textElement.FontSize = this.FontSize;
    textElement.TextScaling = this.m_textScaling;
    textElement.Font = type3FontStruct.CurrentFont;
    textElement.isNegativeFont = this.isNegativeFont;
    textElement.UnicodeCharMapTable = type3FontStruct.UnicodeCharMapTable;
    Dictionary<int, int> fontGlyphWidths = type3FontStruct.FontGlyphWidths;
    textElement.structure = type3FontStruct;
    textElement.Ctm = this.CTRM;
    textElement.textLineMatrix = new Matrix(1.0, 0.0, 0.0, 1.0, 0.0, 0.0);
    textElement.Rise = this.Rise;
    textElement.documentMatrix = this.DocumentMatrix;
    textElement.FontID = this.CurrentFont;
    textElement.OctDecMapTable = type3FontStruct.OctDecMapTable;
    textElement.TextHorizontalScaling = this.HorizontalScaling;
    textElement.ZapfPostScript = type3FontStruct.ZapfPostScript;
    textElement.LineWidth = this.MitterLength;
    textElement.RenderingMode = this.RenderingMode;
    textElement.testdict = this.testdict;
    textElement.PathBrush = this.StrokingBrush == null ? new Pen(Color.Black).Brush : this.StrokingBrush;
    textElement.WordSpacing = this.Objects.WordSpacing;
    textElement.CharacterSpacing = this.Objects.CharacterSpacing;
    Matrix txtMatrix = new Matrix();
    double num = (double) textElement.Render(this.PageGraphics, new PointF(this.m_endTextPosition.X, this.m_endTextPosition.Y - this.FontSize), (double) this.m_textScaling, fontGlyphWidths, (double) type3FontStruct.Type1GlyphHeight, type3FontStruct.differenceTable, type3FontStruct.DifferencesDictionary, type3FontStruct.differenceEncoding, out txtMatrix);
    this.TextLineMatrix = this.CalculateTextMatrixupdate(this.TextLineMatrix, (float) this.m_d1Matrix.M11, true);
    this.m_type3TextLineMatrix = this.TextLineMatrix;
  }

  private void GetType3XObject(ImageStructure xobjectElement)
  {
    Image image = xobjectElement.EmbeddedImage;
    if (xobjectElement.IsImageMask)
      image = image != null ? this.GetType3MaskImage(image, xobjectElement) : this.GetType3MaskImagefromStream(xobjectElement);
    FontStructure type3FontStruct = this.m_type3FontStruct;
    if (type3FontStruct == null)
      return;
    TextElement textElement = new TextElement(image, this.DocumentMatrix);
    textElement.type3GlyphImage = image;
    textElement.FontSize = this.FontSize;
    textElement.TextScaling = this.m_textScaling;
    textElement.Font = type3FontStruct.CurrentFont;
    textElement.isNegativeFont = this.isNegativeFont;
    textElement.UnicodeCharMapTable = type3FontStruct.UnicodeCharMapTable;
    textElement.Text = "";
    Dictionary<int, int> fontGlyphWidths = type3FontStruct.FontGlyphWidths;
    textElement.structure = type3FontStruct;
    textElement.Ctm = this.CTRM;
    textElement.textLineMatrix = new Matrix(1.0, 0.0, 0.0, 1.0, 0.0, 0.0);
    textElement.Rise = this.Rise;
    textElement.documentMatrix = this.DocumentMatrix;
    textElement.FontID = this.CurrentFont;
    textElement.OctDecMapTable = type3FontStruct.OctDecMapTable;
    textElement.TextHorizontalScaling = this.HorizontalScaling;
    textElement.ZapfPostScript = type3FontStruct.ZapfPostScript;
    textElement.LineWidth = this.MitterLength;
    textElement.RenderingMode = this.RenderingMode;
    textElement.testdict = this.testdict;
    textElement.PathBrush = this.StrokingBrush == null ? new Pen(Color.Black).Brush : this.StrokingBrush;
    textElement.WordSpacing = this.Objects.WordSpacing;
    textElement.CharacterSpacing = this.Objects.CharacterSpacing;
    Matrix txtMatrix = new Matrix();
    if (this.m_isTrEntry)
    {
      double num1 = (double) textElement.Render(this.PageGraphics, new PointF(this.m_endTextPosition.X, this.m_endTextPosition.Y - this.FontSize), (double) this.m_textScaling, fontGlyphWidths, (double) type3FontStruct.Type1GlyphHeight, type3FontStruct.differenceTable, type3FontStruct.DifferencesDictionary, type3FontStruct.differenceEncoding, out txtMatrix);
    }
    float num2;
    if (this.m_d1Matrix.M11 == 0.0)
    {
      num2 = (float) (this.m_d1Matrix.M21 - this.m_d1Matrix.OffsetX);
      if ((double) num2 < 0.0)
        num2 = -num2;
    }
    else
      num2 = (float) this.m_d1Matrix.M11;
    this.TextLineMatrix = this.CalculateType3TextMatrixupdate(this.TextLineMatrix, num2 * (float) this.XFormsMatrix.M11, true);
    this.PageGraphics.TranslateTransform(num2 * this.m_type3FontScallingFactor * this.FontSize, 0.0f);
    this.m_type3TextLineMatrix = this.TextLineMatrix;
    if (textElement.Fontfile2Glyph != null && textElement.Fontfile2Glyph.IsFontFile2)
      textElement.textElementGlyphList = this.ReplaceLigatureFromGlyphList(textElement.textElementGlyphList);
    this.imageRenderGlyphList.AddRange((IEnumerable<Glyph>) textElement.textElementGlyphList);
    textElement.textElementGlyphList.Clear();
  }

  private Matrix CalculateType3TextMatrixupdate(Matrix m, float Width, bool isImage)
  {
    return new Matrix(1.0, 0.0, 0.0, 1.0, ((double) Width * (double) this.FontSize + (double) this.Objects.CharacterSpacing + (double) this.m_type3WhiteSpaceWidth) * ((double) this.HorizontalScaling / 100.0), 0.0) * m;
  }

  private Image GetType3MaskImage(Image currentimage, ImageStructure structure)
  {
    Bitmap type3MaskImage = currentimage as Bitmap;
    if (this.StrokingBrush != null || this.NonStrokingBrush != null)
    {
      for (int y = 0; y < type3MaskImage.Height; ++y)
      {
        for (int x = 0; x < type3MaskImage.Width; ++x)
        {
          if (type3MaskImage.GetPixel(x, y).ToArgb() == Color.Black.ToArgb())
          {
            if (this.StrokingBrush != null)
              type3MaskImage.SetPixel(x, y, new Pen(this.StrokingBrush).Color);
            else if (this.NonStrokingBrush != null)
              type3MaskImage.SetPixel(x, y, new Pen(this.NonStrokingBrush).Color);
          }
        }
      }
    }
    MemoryStream memoryStream = new MemoryStream();
    type3MaskImage.Save((Stream) memoryStream, ImageFormat.Png);
    structure.outStream = memoryStream;
    if (!this.m_isTrEntry)
      this.ConvertType3GlyphToPath(structure);
    return (Image) type3MaskImage;
  }

  private Image GetType3MaskImagefromStream(ImageStructure structure)
  {
    byte[] array1 = structure.outStream.ToArray();
    Color color1 = this.StrokingBrush == null ? Color.Black : new Pen(this.StrokingBrush).Color;
    PdfArray pdfArray1 = (PdfArray) null;
    if (structure.ImageDictionary.ContainsKey("Decode"))
      pdfArray1 = structure.ImageDictionary["Decode"] as PdfArray;
    bool flag = false;
    PdfArray pdfArray2 = new PdfArray();
    if (pdfArray1 != null && pdfArray1[0] is PdfNumber && (pdfArray1[0] as PdfNumber).IntValue == 1 && structure.ImageFilter[0] == "FlateDecode")
      flag = true;
    int num1 = (int) color1.A << 24 | (int) color1.R << 16 /*0x10*/ | (int) color1.G << 8 | (int) color1.B;
    int num2 = 1;
    int index1 = 0;
    int num3 = 0;
    int num4 = (1 << num2) - 1;
    int num5 = num2;
    int num6 = 0;
    int num7 = 0;
    List<int> intList = new List<int>();
    for (int index2 = 0; index2 < (int) structure.Height; ++index2)
    {
      for (int index3 = 0; index3 < (int) structure.Width; ++index3)
      {
        if (num5 == 8)
          num6 = (int) array1[index1++];
        int num8;
        if (num5 != 16 /*0x10*/)
        {
          int num9 = (int) array1[index1] >> 8 - num3 - num2 & num4;
          num3 += num2;
          if (num3 == 8)
          {
            ++index1;
            num3 = 0;
          }
          num8 = num9;
        }
        else
        {
          byte[] numArray1 = array1;
          int index4 = index1;
          int num10 = index4 + 1;
          int num11 = (int) numArray1[index4] << 8;
          byte[] numArray2 = array1;
          int index5 = num10;
          index1 = index5 + 1;
          int num12 = (int) numArray2[index5];
          num8 = num11 | num12;
        }
        if (num7 == num8)
        {
          if (flag)
            intList.Add(0);
          else
            intList.Add(num1);
        }
        else if (flag && num8 == 1)
          intList.Add(-16777216 /*0xFF000000*/);
        else
          intList.Add(num8);
      }
      if (num3 != 0)
      {
        ++index1;
        num3 = 0;
      }
    }
    int[] array2 = intList.ToArray();
    Image maskImagefromStream;
    using (Bitmap bitmap = new Bitmap((int) structure.Width, (int) structure.Height, PixelFormat.Format32bppArgb))
    {
      BitmapData bitmapdata = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
      byte[] source = new byte[bitmapdata.Stride * bitmap.Height];
      for (int index6 = 0; index6 < array2.Length; ++index6)
      {
        Color color2 = Color.FromArgb(array2[index6]);
        int num13 = index6 % (int) structure.Width;
        int num14 = index6 / (int) structure.Width;
        source[bitmapdata.Stride * num14 + 4 * num13] = color2.B;
        source[bitmapdata.Stride * num14 + 4 * num13 + 1] = color2.G;
        source[bitmapdata.Stride * num14 + 4 * num13 + 2] = color2.R;
        source[bitmapdata.Stride * num14 + 4 * num13 + 3] = color2.A;
      }
      Marshal.Copy(source, 0, bitmapdata.Scan0, source.Length);
      bitmap.UnlockBits(bitmapdata);
      MemoryStream memoryStream = new MemoryStream();
      bitmap.Save((Stream) memoryStream, ImageFormat.Png);
      maskImagefromStream = Image.FromStream((Stream) memoryStream);
    }
    if (!this.m_isTrEntry)
      this.ConvertType3GlyphToPath(structure);
    return maskImagefromStream;
  }

  private void ConvertType3GlyphToPath(ImageStructure type3GlyphImageData)
  {
    float width = type3GlyphImageData.Width;
    float height = type3GlyphImageData.Height;
    List<List<int>> outlines;
    if (!this.m_type3GlyphPath.ContainsKey(this.m_type3GlyphID))
    {
      int num1 = 1000;
      float num2 = width + 1f;
      byte[] numArray1 = new byte[(int) ((double) num2 * ((double) height + 1.0))];
      byte[] numArray2 = new byte[16 /*0x10*/]
      {
        (byte) 0,
        (byte) 2,
        (byte) 4,
        (byte) 0,
        (byte) 1,
        (byte) 0,
        (byte) 5,
        (byte) 4,
        (byte) 8,
        (byte) 10,
        (byte) 0,
        (byte) 8,
        (byte) 0,
        (byte) 2,
        (byte) 1,
        (byte) 0
      };
      byte[] array = type3GlyphImageData.outStream.ToArray();
      int num3 = (int) ((double) width + 7.0) & -8;
      byte[] numArray3 = new byte[(int) ((double) num3 * (double) height)];
      int num4 = 0;
      byte maxValue = byte.MaxValue;
      int index1 = 0;
      for (int length = array.Length; index1 < length; ++index1)
      {
        byte num5 = 128 /*0x80*/;
        byte num6 = array[index1];
        for (; num5 > (byte) 0 && num4 < numArray3.Length; num5 >>= 1)
          numArray3[num4++] = ((int) num6 & (int) num5) != 0 ? (byte) 0 : maxValue;
      }
      int num7 = 0;
      int index2 = 0;
      if (numArray3[index2] != (byte) 0)
      {
        numArray1[0] = (byte) 1;
        ++num7;
      }
      int index3;
      for (index3 = 1; (double) index3 < (double) width; ++index3)
      {
        if ((int) numArray3[index2] != (int) numArray3[index2 + 1])
        {
          numArray1[index3] = numArray3[index2] != (byte) 0 ? (byte) 2 : (byte) 1;
          ++num7;
        }
        ++index2;
      }
      if (numArray3[index2] != (byte) 0)
      {
        numArray1[index3] = (byte) 2;
        ++num7;
      }
      int num8;
      for (num8 = 1; (double) num8 < (double) height; ++num8)
      {
        int index4 = num8 * num3;
        int index5 = (int) ((double) num8 * (double) num2);
        if ((int) numArray3[index4 - num3] != (int) numArray3[index4])
        {
          numArray1[index5] = numArray3[index4] != (byte) 0 ? (byte) 1 : (byte) 8;
          ++num7;
        }
        byte num9 = 8;
        byte num10 = 4;
        byte num11 = 0;
        byte num12 = 2;
        byte index6 = (byte) (((int) numArray3[index4] != (int) num11 ? (int) num10 : (int) num11) + ((int) numArray3[index4 - num3] != (int) num11 ? (int) num9 : (int) num11));
        int num13;
        for (num13 = 1; (double) num13 < (double) width; ++num13)
        {
          index6 = (byte) (((int) index6 >> (int) num12) + ((int) numArray3[index4 + 1] != (int) num11 ? (int) num10 : (int) num11) + (numArray3[index4 - num3 + 1] != (byte) 0 ? (int) num9 : (int) num11));
          if (numArray2[(int) index6] != (byte) 0)
          {
            numArray1[index5 + num13] = numArray2[(int) index6];
            ++num7;
          }
          ++index4;
        }
        if ((int) numArray3[index4 - num3] != (int) numArray3[index4])
        {
          numArray1[index5 + num13] = numArray3[index4] != (byte) 0 ? num12 : num10;
          ++num7;
        }
        if (num7 > num1)
        {
          this.m_isTrEntry = true;
          return;
        }
      }
      int index7 = num3 * (int) ((double) height - 1.0);
      int index8 = (int) ((double) num8 * (double) num2);
      if (numArray3[index7] != (byte) 0)
      {
        numArray1[index8] = (byte) 8;
        ++num7;
      }
      int num14;
      for (num14 = 1; (double) num14 < (double) width; ++num14)
      {
        if ((int) numArray3[index7] != (int) numArray3[index7 + 1])
        {
          numArray1[index8 + num14] = numArray3[index7] != (byte) 0 ? (byte) 4 : (byte) 8;
          ++num7;
        }
        ++index7;
      }
      if (numArray3[index7] != (byte) 0)
      {
        numArray1[index8 + num14] = (byte) 4;
        ++num7;
      }
      if (num7 > num1)
      {
        this.m_isTrEntry = true;
        return;
      }
      int[] numArray4 = new int[9];
      numArray4[1] = (int) num2;
      numArray4[2] = -1;
      numArray4[4] = (int) -(double) num2;
      numArray4[8] = 1;
      int[] numArray5 = numArray4;
      outlines = new List<List<int>>();
      for (int index9 = 0; num7 != 0 && (double) index9 <= (double) height; ++index9)
      {
        float index10 = (float) index9 * num2;
        float num15 = index10 + width;
        while ((double) index10 < (double) num15 && numArray1[(int) index10] == (byte) 0)
          ++index10;
        if ((double) index10 != (double) num15)
        {
          List<int> intList = new List<int>();
          intList.Add((int) ((double) index10 % (double) num2));
          intList.Add(index9);
          byte index11 = numArray1[(int) index10];
          float num16 = index10;
          do
          {
            int num17 = numArray5[(int) index11];
            do
            {
              index10 += (float) num17;
            }
            while (numArray1[(int) index10] == (byte) 0);
            byte num18 = numArray1[(int) index10];
            switch (num18)
            {
              case 5:
              case 10:
                index11 = (byte) ((uint) num18 & (uint) (51 * (int) index11 >> 4));
                numArray1[(int) index10] &= (byte) ((int) index11 >> 2 | (int) index11 << 2);
                break;
              default:
                index11 = num18;
                numArray1[(int) index10] = (byte) 0;
                break;
            }
            intList.Add((int) ((double) index10 % (double) num2));
            intList.Add((int) ((double) index10 / (double) num2));
            --num7;
          }
          while ((double) num16 != (double) index10);
          outlines.Add(intList);
          --index9;
        }
      }
      this.m_type3GlyphPath.Add(this.m_type3GlyphID, outlines);
    }
    else
      outlines = this.m_type3GlyphPath[this.m_type3GlyphID];
    if (outlines.Count <= 0)
      return;
    this.DrawType3FontAsShape(this.PageGraphics, outlines, width, height);
  }

  private void DrawType3FontAsShape(
    System.Drawing.Graphics graphics,
    List<List<int>> outlines,
    float width,
    float height)
  {
    Pen pen = this.StrokingBrush == null ? new Pen(Color.Black) : new Pen(this.StrokingBrush);
    if ((double) this.m_type3FontScallingFactor != 1.0)
    {
      int pageUnit = (int) graphics.PageUnit;
      System.Drawing.Drawing2D.Matrix transform = graphics.Transform;
      graphics.PageUnit = GraphicsUnit.Pixel;
      graphics.Transform = new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
      Glyph glyph = new Glyph();
      glyph.FontSize = (double) this.FontSize;
      glyph.Stroke = this.StrokingBrush == null ? new Pen(Color.Black).Brush : this.StrokingBrush;
      glyph.TransformMatrix = this.GetTextRenderingMatrix();
      glyph.HorizontalScaling = (double) this.TextScaling;
      glyph.CharSpacing = (double) this.Objects.CharacterSpacing;
      Matrix identity = Matrix.Identity;
      this.m_transformations = new TransformationStack(this.DocumentMatrix);
      this.m_transformations.PushTransform(identity * glyph.TransformMatrix);
      System.Drawing.Drawing2D.Matrix matrix = graphics.Transform.Clone();
      matrix.Multiply(this.GetTransformationMatrix(this.m_transformations.CurrentTransform));
      graphics.Transform = matrix;
      graphics.SmoothingMode = SmoothingMode.AntiAlias;
      graphics.ScaleTransform(1f / width, 1f / height);
    }
    else
    {
      if (this.CTRM.OffsetY != this.TextLineMatrix.OffsetY)
        graphics.TranslateTransform((float) (this.TextLineMatrix.OffsetX + (double) this.m_spacingWidth * (double) this.zoomFactor), this.m_imageCommonMatrix.OffsetY * this.FontSize);
      else
        graphics.TranslateTransform((float) (this.TextLineMatrix.OffsetX + (double) this.m_spacingWidth * (double) this.zoomFactor), 0.0f);
      if (this.CTRM.M11 == 0.0 && this.CTRM.M22 == 0.0)
        graphics.ScaleTransform((float) this.CTRM.M12 * this.FontSize, (float) this.CTRM.M21 * this.FontSize);
      else
        graphics.ScaleTransform((float) this.CTRM.M11 * this.FontSize, (float) this.CTRM.M22 * this.FontSize);
      graphics.ScaleTransform(1f / width, -1f / height);
      graphics.TranslateTransform(0.0f, -height);
    }
    List<int> outline1 = outlines[0];
    List<GraphicsPath> graphicsPathList = new List<GraphicsPath>();
    int index1 = 0;
    for (int count1 = outlines.Count; index1 < count1; ++index1)
    {
      this.BeginPath((float) outline1[0], (float) outline1[1]);
      List<int> outline2 = outlines[index1];
      this.AddLine((float) outline2[0], (float) outline2[1]);
      int index2 = 2;
      for (int count2 = outline2.Count; index2 < count2; index2 += 2)
        this.AddLine((float) outline2[index2], (float) outline2[index2 + 1]);
      this.EndPath();
      graphicsPathList.Add(this.GetGeometry(this.CurrentGeometry, new Matrix(1.0, 0.0, 0.0, 1.0, 0.0, 0.0)));
      this.CurrentGeometry = new PathGeometry();
    }
    GraphicsPath path = new GraphicsPath();
    for (int index3 = 0; index3 < graphicsPathList.Count; ++index3)
      path.AddPath(graphicsPathList[index3], false);
    graphics.FillPath(pen.Brush, path);
  }

  private void GetColorSpaceValue(string[] colorspaceelement)
  {
    if (!this.m_resources.ContainsKey(colorspaceelement[0].Replace("/", "")) || !(this.m_resources[colorspaceelement[0].Replace("/", "")] is ExtendColorspace))
      return;
    foreach (IPdfPrimitive colorSpaceValue in (this.m_resources[colorspaceelement[0].Replace("/", "")] as ExtendColorspace).ColorSpaceValueArray as PdfArray)
    {
      if ((object) (colorSpaceValue as PdfName) != null && (colorSpaceValue as PdfName).Value == "Black")
        this.isBlack = true;
    }
  }

  private Image GetMaskImagefromStream(ImageStructure structure)
  {
    try
    {
      byte[] array1 = structure.outStream.ToArray();
      Color color1 = this.StrokingBrush != null ? new Pen(this.StrokingBrush).Color : new Pen(Brushes.Black).Color;
      int num1 = (int) color1.A << 24 | (int) color1.R << 16 /*0x10*/ | (int) color1.G << 8 | (int) color1.B;
      int num2 = 1;
      int index1 = 0;
      int num3 = 0;
      int num4 = (1 << num2) - 1;
      int num5 = num2;
      int num6 = 0;
      int num7 = 0;
      PdfArray decodeArray = structure.DecodeArray;
      if (decodeArray != null)
        num7 = (decodeArray[0] as PdfNumber).IntValue;
      List<int> intList = new List<int>();
      for (int index2 = 0; index2 < (int) structure.Height; ++index2)
      {
        for (int index3 = 0; index3 < (int) structure.Width; ++index3)
        {
          if (num5 == 8)
            num6 = (int) array1[index1++];
          int num8;
          if (num5 != 16 /*0x10*/)
          {
            int num9 = (int) array1[index1] >> 8 - num3 - num2 & num4;
            num3 += num2;
            if (num3 == 8)
            {
              ++index1;
              num3 = 0;
            }
            num8 = num9;
          }
          else
          {
            byte[] numArray1 = array1;
            int index4 = index1;
            int num10 = index4 + 1;
            int num11 = (int) numArray1[index4] << 8;
            byte[] numArray2 = array1;
            int index5 = num10;
            index1 = index5 + 1;
            int num12 = (int) numArray2[index5];
            num8 = num11 | num12;
          }
          if (num7 == num8)
            intList.Add(num1);
          else
            intList.Add(num8);
        }
        if (num3 != 0)
        {
          ++index1;
          num3 = 0;
        }
      }
      int[] array2 = intList.ToArray();
      Bitmap bitmap = new Bitmap((int) structure.Width, (int) structure.Height, PixelFormat.Format32bppArgb);
      BitmapData bitmapdata = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
      byte[] source = new byte[bitmapdata.Stride * bitmap.Height];
      for (int index6 = 0; index6 < array2.Length; ++index6)
      {
        Color color2 = Color.FromArgb(array2[index6]);
        int num13 = index6 % (int) structure.Width;
        int num14 = index6 / (int) structure.Width;
        source[bitmapdata.Stride * num14 + 4 * num13] = color2.B;
        source[bitmapdata.Stride * num14 + 4 * num13 + 1] = color2.G;
        source[bitmapdata.Stride * num14 + 4 * num13 + 2] = color2.R;
        source[bitmapdata.Stride * num14 + 4 * num13 + 3] = color2.A;
      }
      Marshal.Copy(source, 0, bitmapdata.Scan0, source.Length);
      bitmap.UnlockBits(bitmapdata);
      this.m_outStream = new MemoryStream();
      bitmap.Save((Stream) this.m_outStream, ImageFormat.Png);
      return Image.FromStream((Stream) this.m_outStream);
    }
    catch
    {
      return (Image) null;
    }
  }

  private Image GetMaskImage(Image currentimage)
  {
    Bitmap maskImage = currentimage as Bitmap;
    if (this.StrokingBrush != null || this.NonStrokingBrush != null)
    {
      int height = maskImage.Height;
      int width = maskImage.Width;
      Color color1 = new Color();
      Color color2 = new Color();
      if (this.StrokingBrush != null)
        color1 = new Pen(this.StrokingBrush).Color;
      else if (this.NonStrokingBrush != null)
        color2 = new Pen(this.NonStrokingBrush).Color;
      for (int y = 0; y < height; ++y)
      {
        for (int x = 0; x < width; ++x)
        {
          if (maskImage.GetPixel(x, y).ToArgb() == Color.Black.ToArgb())
          {
            if (this.StrokingBrush != null)
              maskImage.SetPixel(x, y, color1);
            else if (this.NonStrokingBrush != null)
              maskImage.SetPixel(x, y, color2);
          }
        }
      }
    }
    return (Image) maskImage;
  }

  private float CalculateTextHeight()
  {
    float textHeight = 0.0f;
    if (this.m_resources[this.CurrentFont] is FontStructure resource)
    {
      Font font = (double) this.FontSize >= 0.0 ? new Font(TextElement.CheckFontName(resource.FontName), this.FontSize, resource.FontStyle) : new Font(TextElement.CheckFontName(resource.FontName), -this.FontSize, resource.FontStyle);
      textHeight = font.Size;
      font.Dispose();
    }
    return textHeight;
  }

  internal void Clear(bool IsSearchAllOccurrence)
  {
    if (this.URLDictonary.Count > 0)
      this.URLDictonary.Clear();
    if (this.testdict.Count > 0)
      this.testdict.Clear();
    if (!this.IsTextSearch)
    {
      if (this.m_resources != null)
        this.m_resources.Dispose();
      if (this.imageRenderGlyphList.Count > 0 && !IsSearchAllOccurrence)
        this.imageRenderGlyphList.Clear();
      if (this.m_contentElements != null && this.m_contentElements.RecordCollection.Count > 0)
        this.m_contentElements.RecordCollection.Clear();
    }
    if (this.m_subPaths.Count > 0)
      this.m_subPaths.Clear();
    if (this.m_tempSubPaths.Count > 0)
      this.m_tempSubPaths.Clear();
    if (FontStructure.unicodeCharMapTable != null)
      FontStructure.unicodeCharMapTable.Clear();
    if (this.m_clipRectangleList.Count > 0)
      this.m_clipRectangleList.Clear();
    if (this.m_glyphDataCollection.Count > 0)
      this.m_glyphDataCollection.Clear();
    if (this.m_inlineParameters != null)
      this.m_inlineParameters.Clear();
    if (this.m_objects.Count > 0)
      this.m_objects.Clear();
    if (this.m_graphicsState.Count > 0)
      this.m_graphicsState.Clear();
    if (this.m_graphics == null)
      return;
    this.m_graphics.Dispose();
  }

  private float CharacterSpacing
  {
    get => this.m_characterSpacing;
    set => this.m_characterSpacing = value;
  }

  private float WordSpacing
  {
    get => this.m_wordSpacing;
    set => this.m_wordSpacing = value;
  }

  private void RenderTextElementWithLeadingPrintTextasText(string[] textElements, string tokenType)
  {
    string textToDecode = string.Join("", textElements);
    if (!this.m_resources.ContainsKey(this.CurrentFont))
      return;
    (this.m_resources[this.CurrentFont] as FontStructure).IsSameFont = this.m_resources.isSameFont();
    if ((double) (this.m_resources[this.CurrentFont] as FontStructure).FontSize != (double) this.FontSize)
      (this.m_resources[this.CurrentFont] as FontStructure).FontSize = this.FontSize;
    FontStructure resource = this.m_resources[this.CurrentFont] as FontStructure;
    string text = resource.Decode(textToDecode, this.m_resources.isSameFont());
    TextElement textElement = new TextElement(text);
    textElement.FontName = resource.FontName;
    textElement.FontStyle = resource.FontStyle;
    textElement.FontSize = this.FontSize;
    textElement.TextScaling = this.m_textScaling;
    textElement.Font = resource.CurrentFont;
    textElement.FontEncoding = resource.FontEncoding;
    textElement.FontGlyphWidths = resource.FontGlyphWidths;
    textElement.CharacterMapTable = resource.CharacterMapTable;
    textElement.ReverseMapTable = resource.ReverseMapTable;
    textElement.Text = text;
    Color color = this.StrokingBrush == null ? Color.Empty : ((SolidBrush) this.StrokingBrush).Color;
    textElement.BrushColor = !(color != Color.Empty) ? Color.Black : color;
    textElement.WordSpacing = this.WordSpacing;
    textElement.CharacterSpacing = this.CharacterSpacing;
    if (this.m_beginText)
      this.m_beginText = false;
    if (this.m_isCurrentPositionChanged)
    {
      this.m_isCurrentPositionChanged = false;
      this.m_endTextPosition = this.CurrentLocation;
      this.m_textElementWidth = textElement.Render(this.PageGraphics, new PointF(this.m_endTextPosition.X, this.m_endTextPosition.Y + (float) (-(double) this.TextLeading / 4.0)));
    }
    else
    {
      this.m_endTextPosition = new PointF(this.m_endTextPosition.X + this.m_textElementWidth, this.m_endTextPosition.Y);
      this.m_textElementWidth = textElement.Render(this.PageGraphics, new PointF(this.m_endTextPosition.X, this.m_endTextPosition.Y + (float) (-(double) this.TextLeading / 4.0)));
    }
    string str = textElement.renderedText;
    if (!resource.IsMappingDone)
    {
      if (resource.CharacterMapTable != null && resource.CharacterMapTable.Count > 0)
        str = resource.MapCharactersFromTable(str);
      else if (resource.DifferencesDictionary != null && resource.DifferencesDictionary.Count > 0)
        str = resource.MapDifferences(str);
    }
    this.transformMatrix = this.PageGraphics.Transform;
    this.currentTransformLocation = new PointF(this.CurrentLocation.X, this.m_endTextPosition.Y);
    double textHeight1 = (double) this.CalculateTextHeight();
    PageText pageText = new PageText(this.transformMatrix, str, new PointF(this.m_endTextPosition.X, this.m_endTextPosition.Y + (this.TextLeading - this.FontSize)), this.m_textElementWidth, this.FontSize, textElement.textFont);
    ImageRenderer.textDictonary.Add(pageText);
    if (str.Contains("www") || textElement.renderedText.Contains("http") || this.IsValidEmail(str))
    {
      this.transformMatrix = this.PageGraphics.Transform;
      this.currentTransformLocation = new PointF(this.CurrentLocation.X, this.m_endTextPosition.Y);
      double textHeight2 = (double) this.CalculateTextHeight();
      Regex regex = new Regex("\\b(?:http://|https://|www\\.)\\S+\\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);
      string URI = string.Empty;
      foreach (Capture match in regex.Matches(str))
        URI = match.Value;
      if (this.IsValidEmail(textElement.renderedText))
        URI = "mailto:" + str;
      this.URLDictonary.Add(new PageURL(this.transformMatrix, URI, new PointF(this.m_endTextPosition.X, this.m_endTextPosition.Y + (this.TextLeading - this.FontSize)), this.m_textElementWidth, this.FontSize));
    }
    this.DrawNewLine();
  }

  private void RenderTextElementPrintTextasText(string[] textElements, string tokenType)
  {
    string textToDecode = string.Join("", textElements);
    if ((double) this.FontSize < 0.0)
    {
      this.FontSize = -this.FontSize;
      this.isNegativeFont = true;
    }
    if (!this.m_resources.ContainsKey(this.CurrentFont))
      return;
    (this.m_resources[this.CurrentFont] as FontStructure).IsSameFont = this.m_resources.isSameFont();
    if ((double) (this.m_resources[this.CurrentFont] as FontStructure).FontSize != (double) this.FontSize)
      (this.m_resources[this.CurrentFont] as FontStructure).FontSize = this.FontSize;
    FontStructure resource = this.m_resources[this.CurrentFont] as FontStructure;
    TextElement textElement = new TextElement(resource.Decode(textToDecode, this.m_resources.isSameFont()));
    textElement.FontName = resource.FontName;
    textElement.FontStyle = resource.FontStyle;
    textElement.FontSize = this.FontSize;
    textElement.TextScaling = this.m_textScaling;
    textElement.Font = resource.CurrentFont;
    textElement.FontEncoding = resource.FontEncoding;
    textElement.FontGlyphWidths = resource.FontGlyphWidths;
    textElement.isNegativeFont = this.isNegativeFont;
    Color color = this.StrokingBrush == null ? Color.Empty : ((SolidBrush) this.StrokingBrush).Color;
    textElement.BrushColor = !(color != Color.Empty) ? Color.Black : color;
    textElement.WordSpacing = this.WordSpacing;
    textElement.CharacterSpacing = this.CharacterSpacing;
    if (this.m_beginText)
      this.m_beginText = false;
    if (this.m_isCurrentPositionChanged)
    {
      this.m_isCurrentPositionChanged = false;
      this.m_endTextPosition = this.CurrentLocation;
      this.m_textElementWidth = textElement.Render(this.PageGraphics, new PointF(this.m_endTextPosition.X, this.m_endTextPosition.Y - this.FontSize));
    }
    else
    {
      this.m_endTextPosition = new PointF(this.m_endTextPosition.X + this.m_textElementWidth, this.m_endTextPosition.Y);
      this.m_textElementWidth = textElement.Render(this.PageGraphics, new PointF(this.m_endTextPosition.X, this.m_endTextPosition.Y - this.FontSize));
    }
    string str = textElement.renderedText;
    if (!resource.IsMappingDone)
    {
      if (resource.CharacterMapTable != null && resource.CharacterMapTable.Count > 0)
        str = resource.MapCharactersFromTable(str);
      else if (resource.DifferencesDictionary != null && resource.DifferencesDictionary.Count > 0)
        str = resource.MapDifferences(str);
    }
    this.transformMatrix = this.PageGraphics.Transform;
    this.currentTransformLocation = new PointF(this.CurrentLocation.X, this.m_endTextPosition.Y);
    double textHeight1 = (double) this.CalculateTextHeight();
    PageText pageText = new PageText(this.transformMatrix, str, new PointF(this.m_endTextPosition.X, this.m_endTextPosition.Y - this.FontSize), this.m_textElementWidth, this.FontSize, textElement.textFont);
    ImageRenderer.textDictonary.Add(pageText);
    if (!str.Contains("www") && !str.Contains("http") && !this.IsValidEmail(str))
      return;
    this.transformMatrix = this.PageGraphics.Transform;
    this.currentTransformLocation = new PointF(this.CurrentLocation.X, this.m_endTextPosition.Y);
    double textHeight2 = (double) this.CalculateTextHeight();
    Regex regex = new Regex("\\b(?:http://|https://|www\\.)\\S+\\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    string URI = string.Empty;
    foreach (Capture match in regex.Matches(str))
      URI = match.Value;
    if (this.IsValidEmail(textElement.renderedText))
      URI = "mailto:" + str;
    this.URLDictonary.Add(new PageURL(this.transformMatrix, URI, new PointF(this.m_endTextPosition.X, this.m_endTextPosition.Y - this.FontSize), this.m_textElementWidth, this.FontSize));
  }

  private void RenderTextElementWithSpacingPrintTextasText(string[] textElements, string tokenType)
  {
    List<string> stringList = new List<string>();
    string str1 = string.Join("", textElements);
    if (!this.m_resources.ContainsKey(this.CurrentFont))
      return;
    (this.m_resources[this.CurrentFont] as FontStructure).IsSameFont = this.m_resources.isSameFont();
    if ((double) (this.m_resources[this.CurrentFont] as FontStructure).FontSize != (double) this.FontSize)
      (this.m_resources[this.CurrentFont] as FontStructure).FontSize = this.FontSize;
    FontStructure resource = this.m_resources[this.CurrentFont] as FontStructure;
    List<float> characterSpacings = (List<float>) null;
    List<string> decodedList = resource.DecodeTextTJ(str1, this.m_resources.isSameFont());
    TextElement textElement = new TextElement(str1);
    textElement.FontName = resource.FontName;
    textElement.FontStyle = resource.FontStyle;
    textElement.FontSize = this.FontSize;
    textElement.TextScaling = this.m_textScaling;
    textElement.Font = resource.CurrentFont;
    textElement.FontEncoding = resource.FontEncoding;
    textElement.FontGlyphWidths = resource.FontGlyphWidths;
    textElement.CharacterMapTable = resource.CharacterMapTable;
    textElement.ReverseMapTable = resource.ReverseMapTable;
    Color color = this.StrokingBrush == null ? Color.Empty : ((SolidBrush) this.StrokingBrush).Color;
    textElement.BrushColor = !(color != Color.Empty) ? Color.Black : color;
    textElement.WordSpacing = this.WordSpacing;
    textElement.CharacterSpacing = this.CharacterSpacing;
    if (this.m_beginText)
      this.m_beginText = false;
    if (this.m_isCurrentPositionChanged)
    {
      this.m_isCurrentPositionChanged = false;
      this.m_endTextPosition = this.CurrentLocation;
      this.m_textElementWidth = textElement.RenderWithSpace(this.PageGraphics, new PointF(this.m_endTextPosition.X, this.m_endTextPosition.Y - this.FontSize), decodedList, characterSpacings);
    }
    else
    {
      this.m_endTextPosition = new PointF(this.m_endTextPosition.X + this.m_textElementWidth, this.m_endTextPosition.Y);
      this.m_textElementWidth = textElement.RenderWithSpace(this.PageGraphics, new PointF(this.m_endTextPosition.X, this.m_endTextPosition.Y - this.FontSize), decodedList, characterSpacings);
    }
    string str2 = textElement.renderedText;
    if (!resource.IsMappingDone)
    {
      if (resource.CharacterMapTable != null && resource.CharacterMapTable.Count > 0)
        str2 = resource.MapCharactersFromTable(str2);
      else if (resource.DifferencesDictionary != null && resource.DifferencesDictionary.Count > 0)
        str2 = resource.MapDifferences(str2);
    }
    this.transformMatrix = this.PageGraphics.Transform;
    this.currentTransformLocation = new PointF(this.CurrentLocation.X, this.m_endTextPosition.Y);
    double textHeight1 = (double) this.CalculateTextHeight();
    if (textElement.textFont == null)
    {
      textElement.CheckFontStyle(textElement.FontName);
      Font font = new Font(FontStructure.CheckFontName(textElement.FontName), textElement.FontSize, textElement.FontStyle);
    }
    PageText pageText;
    if ((double) this.m_endTextPosition.Y + ((double) this.TextLeading - (double) this.FontSize) != 0.0)
    {
      if (textElement.textFont == null)
      {
        textElement.CheckFontStyle(textElement.FontName);
        Font font = new Font(FontStructure.CheckFontName(textElement.FontName), textElement.FontSize, textElement.FontStyle);
        pageText = new PageText(this.transformMatrix, str2, new PointF(this.m_endTextPosition.X, this.m_endTextPosition.Y - this.FontSize), this.m_textElementWidth, this.FontSize, font);
      }
      else
        pageText = new PageText(this.transformMatrix, str2, new PointF(this.m_endTextPosition.X, this.m_endTextPosition.Y - this.FontSize), this.m_textElementWidth, this.FontSize, textElement.textFont);
    }
    else
      pageText = new PageText(this.transformMatrix, str2, new PointF(this.m_endTextPosition.X, this.m_endTextPosition.Y), this.m_textElementWidth, this.FontSize, textElement.textFont);
    ImageRenderer.textDictonary.Add(pageText);
    if (!str2.Contains("www") && !str2.Contains("http") && !this.IsValidEmail(str2))
      return;
    double textLeading = (double) this.TextLeading;
    this.transformMatrix = this.PageGraphics.Transform;
    this.currentTransformLocation = new PointF(this.CurrentLocation.X, this.m_endTextPosition.Y);
    double textHeight2 = (double) this.CalculateTextHeight();
    Regex regex = new Regex("\\b(?:http://|https://|www\\.)\\S+\\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    string URI = string.Empty;
    foreach (Capture match in regex.Matches(str2))
      URI = match.Value;
    if (this.IsValidEmail(textElement.renderedText))
      URI = "mailto:" + str2;
    this.URLDictonary.Add((double) this.m_endTextPosition.Y + ((double) this.TextLeading - (double) this.FontSize) == 0.0 ? new PageURL(this.transformMatrix, URI, new PointF(this.m_endTextPosition.X, this.m_endTextPosition.Y), this.m_textElementWidth, this.FontSize) : new PageURL(this.transformMatrix, URI, new PointF(this.m_endTextPosition.X, this.m_endTextPosition.Y - this.FontSize), this.m_textElementWidth, this.FontSize));
  }
}
