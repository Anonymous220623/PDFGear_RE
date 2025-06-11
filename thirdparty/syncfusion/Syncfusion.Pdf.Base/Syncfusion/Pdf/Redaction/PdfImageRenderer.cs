// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Redaction.PdfImageRenderer
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using Syncfusion.PdfViewer.Base;
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
namespace Syncfusion.Pdf.Redaction;

internal class PdfImageRenderer
{
  private const int PathTypesValuesMask = 15;
  internal PdfLoadedPage m_loadedPage;
  internal List<PageURL> URLDictonary = new List<PageURL>();
  internal List<PdfPath> PdfPaths = new List<PdfPath>();
  private float pt = 1.3333f;
  private bool m_isTrEntry;
  private bool m_isExtendedGraphicStateContainsSMask;
  internal bool IsExtendedGraphicsState;
  internal bool IsGraphicsState;
  internal GraphicsPath m_graphicspathtoclip;
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
  private Stack<GraphicsState> m_graphicsState = new Stack<GraphicsState>();
  internal Stack<GraphicObjectData> m_objects = new Stack<GraphicObjectData>();
  private float m_textScaling = 100f;
  private bool textMatrix;
  private float m_textElementWidth;
  private string[] m_backupColorElements;
  private string m_backupColorSpace;
  private PointF m_endTextPosition;
  private bool m_isCurrentPositionChanged;
  internal bool isFindText;
  private Brush transperentStrokingBrush;
  private Brush transperentNonStrokingBrush;
  private PdfViewerExceptions exception = new PdfViewerExceptions();
  private Syncfusion.PdfViewer.Base.DeviceCMYK decodecmykColor;
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
  internal char findpath;
  private bool isScaledText;
  private float currentPageHeight;
  internal bool isBlack;
  private Dictionary<SystemFontFontDescriptor, SystemFontOpenTypeFontSource> testdict = new Dictionary<SystemFontFontDescriptor, SystemFontOpenTypeFontSource>();
  private PdfDictionary m_inlineParameters = new PdfDictionary();
  private List<string> BIparameter = new List<string>();
  internal List<Syncfusion.PdfViewer.Base.Glyph> imageRenderGlyphList;
  internal float pageRotation;
  internal float zoomFactor = 1f;
  private bool isNextFill;
  private bool isWinAnsiEncoding;
  private int m_tilingType;
  internal List<int[]> elementRange = new List<int[]>();
  internal List<RectangleF> RedactionBounds = new List<RectangleF>();
  private List<string> decodedStringData = new List<string>();
  private FontStructure fontStructure;
  internal bool isExportAsImage;
  internal Dictionary<string, string> substitutedFontsList = new Dictionary<string, string>();
  private double det = 6.7500003375002535;
  private double m03 = 4.0 / 9.0;
  private double m12 = -0.22222218888888887;
  private double pow1 = 1.0 / 27.0;
  private double pow2 = 8.0 / 27.0;
  private int recordCount;
  private Syncfusion.PdfViewer.Base.Matrix XFormsMatrix;
  private Syncfusion.PdfViewer.Base.Matrix m_d1Matrix;
  private Matrixx m_d0Matrix;
  private Syncfusion.PdfViewer.Base.Matrix m_type3TextLineMatrix;
  private float m_type3FontScallingFactor;
  private PdfRecordCollection m_type3RecordCollection;
  private bool m_isType3Font;
  private Syncfusion.Pdf.TransformationStack m_transformations;
  private FontStructure m_type3FontStruct;
  private float m_spacingWidth;
  private string m_type3GlyphID;
  private float m_type3WhiteSpaceWidth;
  private Dictionary<string, List<List<int>>> m_type3GlyphPath;
  private bool m_istype3FontContainCTRM;
  private MemoryStream m_outStream;
  private bool isContainsRedactionText;
  private bool isNotUpdated;
  private bool removePathLines;
  private RectangleF rectValue = new RectangleF();
  private bool isContainsImages;
  private PathGeometry CurrentGeometry = new PathGeometry();
  private PathGeometry BackupCurrentGeometry = new PathGeometry();
  private PathFigure m_currentPath;
  private bool containsImage;
  internal int xobjectGraphicsCount;
  internal bool isXGraphics;
  private string[] clipRectShape;
  private bool isRect;
  internal float m_characterSpacing;
  internal bool m_selectablePrintDocument;
  private float m_textAngle;
  internal float m_pageHeight;
  internal bool m_isPrintSelected;

  internal PdfImageRenderer(
    PdfRecordCollection contentElements,
    PdfPageResources resources,
    System.Drawing.Graphics g,
    bool newPage,
    float pageBottom,
    float left,
    Syncfusion.PdfViewer.Base.DeviceCMYK cmyk)
  {
    GraphicObjectData graphicObjectData = new GraphicObjectData();
    graphicObjectData.Ctm = Syncfusion.PdfViewer.Base.Matrix.Identity;
    graphicObjectData.Ctm.Translate((double) (g.Transform.Elements[4] / 1.333f), (double) (g.Transform.Elements[5] / 1.333f));
    graphicObjectData.drawing2dMatrixCTM = new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
    graphicObjectData.drawing2dMatrixCTM.Translate(g.Transform.Elements[4] / 1.333f, g.Transform.Elements[5] / 1.333f);
    System.Drawing.Drawing2D.Matrix transform = g.Transform;
    graphicObjectData.documentMatrix = new Syncfusion.PdfViewer.Base.Matrix(4.0 / 3.0 * ((double) g.DpiX / 96.0) * (double) transform.Elements[0], 0.0, 0.0, -4.0 / 3.0 * ((double) g.DpiX / 96.0) * (double) transform.Elements[3], 0.0, (double) pageBottom * (double) transform.Elements[3]);
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
    this.imageRenderGlyphList = new List<Syncfusion.PdfViewer.Base.Glyph>();
  }

  private PdfImageRenderer(
    PdfRecordCollection contentElements,
    PdfPageResources resources,
    System.Drawing.Graphics g,
    bool newPage,
    Syncfusion.PdfViewer.Base.DeviceCMYK cmyk,
    float pageHeight)
  {
    GraphicObjectData graphicObjectData = new GraphicObjectData();
    graphicObjectData.m_mitterLength = 1f;
    graphicObjectData.Ctm = Syncfusion.PdfViewer.Base.Matrix.Identity;
    graphicObjectData.Ctm.Translate((double) (g.Transform.Elements[4] / 1.333f), (double) (g.Transform.Elements[5] / 1.333f));
    graphicObjectData.drawing2dMatrixCTM = new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
    graphicObjectData.drawing2dMatrixCTM.Translate(g.Transform.Elements[4] / 1.333f, g.Transform.Elements[5] / 1.333f);
    System.Drawing.Drawing2D.Matrix transform = g.Transform;
    graphicObjectData.documentMatrix = new Syncfusion.PdfViewer.Base.Matrix(4.0 / 3.0 * ((double) g.DpiX / 96.0) * (double) transform.Elements[0], 0.0, 0.0, -4.0 / 3.0 * ((double) g.DpiX / 96.0) * (double) transform.Elements[3], 0.0, (double) pageHeight * (double) transform.Elements[3]);
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
    this.imageRenderGlyphList = new List<Syncfusion.PdfViewer.Base.Glyph>();
    this.m_type3GlyphPath = new Dictionary<string, List<List<int>>>();
  }

  internal PdfImageRenderer(int recordCount) => this.recordCount = recordCount;

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

  private Syncfusion.PdfViewer.Base.Matrix CTRM
  {
    get => this.Objects.Ctm;
    set => this.Objects.Ctm = value;
  }

  private Syncfusion.PdfViewer.Base.Matrix TextLineMatrix
  {
    get => this.Objects.textLineMatrix;
    set => this.Objects.textLineMatrix = value;
  }

  private Syncfusion.PdfViewer.Base.Matrix TextMatrix
  {
    get => this.Objects.textMatrix;
    set => this.Objects.textMatrix = value;
  }

  private Syncfusion.PdfViewer.Base.Matrix DocumentMatrix
  {
    get => this.Objects.documentMatrix;
    set => this.Objects.documentMatrix = value;
  }

  private Syncfusion.PdfViewer.Base.Matrix TextMatrixUpdate
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

  private Syncfusion.PdfViewer.Base.Matrix TransformMatrixTM
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
    this.CTRM = new Syncfusion.PdfViewer.Base.Matrix(a, b, c, d, e, f) * this.m_objects.ToArray()[0].Ctm;
    return new System.Drawing.Drawing2D.Matrix((float) this.CTRM.M11, (float) this.CTRM.M12, (float) this.CTRM.M21, (float) this.CTRM.M22, (float) this.CTRM.OffsetX, (float) this.CTRM.OffsetY);
  }

  private void SetTextMatrix(double a, double b, double c, double d, double e, double f)
  {
    this.TextLineMatrix = this.TextMatrix = new Syncfusion.PdfViewer.Base.Matrix()
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

  private Syncfusion.PdfViewer.Base.Matrix GetTextRenderingMatrix(bool isPath)
  {
    return new Syncfusion.PdfViewer.Base.Matrix()
    {
      M11 = (double) this.FontSize * ((double) this.HorizontalScaling / 100.0),
      M22 = (isPath ? (double) this.FontSize : -(double) this.FontSize),
      OffsetY = (isPath ? (double) this.Rise : (double) this.FontSize + (double) this.Rise)
    } * this.TextLineMatrix * this.CTRM;
  }

  private Syncfusion.PdfViewer.Base.Matrix GetTextRenderingMatrix()
  {
    return new Syncfusion.PdfViewer.Base.Matrix()
    {
      M11 = (double) this.FontSize * ((double) this.HorizontalScaling / 100.0),
      M22 = -(double) this.FontSize,
      OffsetY = ((double) this.FontSize + (double) this.Rise)
    } * this.CTRM;
  }

  private void MoveToNextLine(double tx, double ty)
  {
    this.TextLineMatrix = this.TextMatrix = new Syncfusion.PdfViewer.Base.Matrix()
    {
      M11 = 1.0,
      M12 = 0.0,
      OffsetX = tx,
      M21 = 0.0,
      M22 = 1.0,
      OffsetY = ty
    } * this.TextLineMatrix;
  }

  internal MemoryStream RenderAsImage()
  {
    PdfStream stream = new PdfStream();
    PdfRecordCollection recordCollection = !this.m_isType3Font ? this.m_contentElements : this.m_type3RecordCollection;
    if (recordCollection != null)
    {
      this.recordCount = recordCollection.RecordCollection.Count;
      bool flag1 = false;
      for (int index = 0; index < recordCollection.RecordCollection.Count; ++index)
      {
        string tokenType = recordCollection.RecordCollection[index].OperatorName;
        string[] operands = recordCollection.RecordCollection[index].Operands;
        string updatedText = (string) null;
        bool isSkip = false;
        bool changeOperator = false;
        foreach (char symbolChar in this.m_symbolChars)
        {
          if (tokenType.Contains(symbolChar.ToString()))
            tokenType = tokenType.Replace(symbolChar.ToString(), "");
        }
        switch (tokenType.Trim())
        {
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
            }
            if (this.isXGraphics)
              ++this.xobjectGraphicsCount;
            this.m_objects.Push(graphicObjectData1);
            this.m_graphicsState.Push(this.m_graphics.Save());
            break;
          case "Q":
            if (this.isXGraphics)
              --this.xobjectGraphicsCount;
            this.m_objects.Pop();
            if (this.m_graphicsState.Count > 0)
              this.m_graphics.Restore(this.m_graphicsState.Pop());
            this.m_graphicspathtoclip = (GraphicsPath) null;
            this.IsGraphicsState = false;
            this.IsTransparentText = false;
            break;
          case "Tm":
            float num1 = float.Parse(operands[0]);
            float b = float.Parse(operands[1]);
            float c = float.Parse(operands[2]);
            float num2 = float.Parse(operands[3]);
            float num3 = float.Parse(operands[4]);
            float num4 = float.Parse(operands[5]);
            this.SetTextMatrix((double) num1, (double) b, (double) c, (double) num2, (double) num3, (double) num4);
            if (this.textMatrix)
              this.m_graphics.Restore(this.m_graphicsState.Pop());
            this.m_graphicsState.Push(this.m_graphics.Save());
            this.m_graphics.MultiplyTransform(new System.Drawing.Drawing2D.Matrix(num1, -b, -c, num2, num3, -num4));
            this.CurrentLocation = (PointF) new System.Drawing.Point(0, 0);
            this.textMatrix = true;
            if (this.isFoundText(new PointF(num3, num4)))
            {
              this.isContainsRedactionText = true;
              this.isNotUpdated = true;
            }
            if (recordCollection.RecordCollection.Count != index + 1 && !this.isContainsRedactionText)
            {
              string operatorName = recordCollection.RecordCollection[index + 1].OperatorName;
              if (operatorName == "TJ" || operatorName == "Tj" || operatorName == "'")
              {
                this.isContainsRedactionText = true;
                this.isNotUpdated = true;
              }
            }
            if (!this.isContainsRedactionText && (double) this.m_loadedPage.Size.Height == (double) num4)
            {
              this.isContainsRedactionText = true;
              this.isNotUpdated = true;
              break;
            }
            break;
          case "cm":
            float num5 = float.Parse(operands[0]);
            float num6 = float.Parse(operands[1]);
            float num7 = float.Parse(operands[2]);
            float num8 = float.Parse(operands[3]);
            float num9 = float.Parse(operands[4]);
            float num10 = float.Parse(operands[5]);
            if (this.isFoundText(new PointF(num9, num10)))
            {
              this.isContainsRedactionText = true;
              this.isNotUpdated = true;
            }
            this.Drawing2dMatrixCTM = this.SetMatrix((double) num5, (double) num6, (double) num7, (double) num8, (double) num9, (double) num10);
            this.m_imageCommonMatrix = new System.Drawing.Drawing2D.Matrix(num5, num6, num7, num8, num9, num10);
            break;
          case "BT":
            this.TextLineMatrix = this.TextMatrix = Syncfusion.PdfViewer.Base.Matrix.Identity;
            this.m_beginText = true;
            this.CurrentLocation = PointF.Empty;
            break;
          case "ET":
            this.CurrentLocation = PointF.Empty;
            if (this.isScaledText)
            {
              this.isScaledText = false;
              this.m_graphics.Restore(this.m_graphicsState.Pop());
            }
            if (this.textMatrix)
            {
              this.m_graphics.Restore(this.m_graphicsState.Pop());
              this.textMatrix = false;
            }
            if (this.RenderingMode != 3)
              this.RenderingMode = 0;
            this.isContainsRedactionText = false;
            this.isNotUpdated = false;
            break;
          case "T*":
            this.MoveToNextLineWithCurrentTextLeading();
            this.DrawNewLine();
            break;
          case "TJ":
            try
            {
              if (this.isContainsRedactionText)
              {
                this.isNotUpdated = false;
                this.decodedStringData = new List<string>();
                this.imageRenderGlyphList.Clear();
                this.RenderTextElementWithSpacing(operands, tokenType);
                updatedText = this.ReplaceText(string.Join("", operands), this.imageRenderGlyphList, out isSkip, out changeOperator);
                if (updatedText != null)
                {
                  if (operands[0].Equals(updatedText))
                  {
                    this.isNotUpdated = true;
                    break;
                  }
                  break;
                }
                break;
              }
              break;
            }
            catch (Exception ex)
            {
              break;
            }
          case "Tj":
            try
            {
              if (this.isContainsRedactionText)
              {
                this.isNotUpdated = false;
                this.decodedStringData = new List<string>();
                this.imageRenderGlyphList.Clear();
                this.RenderTextElement(operands, tokenType);
                updatedText = this.ReplaceText(string.Join("", operands), this.imageRenderGlyphList, out isSkip, out changeOperator);
                if (updatedText != null)
                {
                  if (operands[0].Equals(updatedText))
                  {
                    this.isNotUpdated = true;
                    break;
                  }
                  break;
                }
                break;
              }
              break;
            }
            catch (Exception ex)
            {
              break;
            }
          case "'":
            try
            {
              if (this.isContainsRedactionText)
              {
                this.isNotUpdated = false;
                this.decodedStringData = new List<string>();
                this.MoveToNextLineWithCurrentTextLeading();
                this.TextMatrixUpdate = this.GetTextRenderingMatrix(false);
                Syncfusion.PdfViewer.Base.Matrix documentMatrix = this.DocumentMatrix;
                if ((double) this.TextScaling != 100.0)
                {
                  this.m_graphicsState.Push(this.m_graphics.Save());
                  this.m_graphics.ScaleTransform(this.TextScaling / 100f, 1f);
                  this.isScaledText = true;
                  this.CurrentLocation = new PointF(this.CurrentLocation.X / (this.TextScaling / 100f), this.CurrentLocation.Y);
                }
                this.imageRenderGlyphList.Clear();
                this.RenderTextElementWithLeading(operands, tokenType);
                updatedText = this.ReplaceText(string.Join("", operands), this.imageRenderGlyphList, out isSkip, out changeOperator);
                if (updatedText != null)
                {
                  if (operands[0].Equals(updatedText))
                  {
                    this.isNotUpdated = true;
                    break;
                  }
                  break;
                }
                break;
              }
              break;
            }
            catch (Exception ex)
            {
              break;
            }
          case "Tf":
            this.RenderFont(operands);
            break;
          case "TD":
            this.CurrentLocation = new PointF(this.CurrentLocation.X + float.Parse(operands[0]), this.CurrentLocation.Y - float.Parse(operands[1]));
            this.MoveToNextLineWithLeading(operands);
            if (this.isFoundText(this.CurrentLocation))
            {
              this.isContainsRedactionText = true;
              this.isNotUpdated = true;
            }
            if (recordCollection.RecordCollection.Count != index + 1 && !this.isContainsRedactionText)
            {
              string operatorName = recordCollection.RecordCollection[index + 1].OperatorName;
              if (operatorName == "TJ" || operatorName == "Tj" || operatorName == "'")
              {
                this.isContainsRedactionText = true;
                this.isNotUpdated = true;
                break;
              }
              break;
            }
            break;
          case "Td":
            this.CurrentLocation = new PointF(this.CurrentLocation.X + float.Parse(operands[0]), this.CurrentLocation.Y - float.Parse(operands[1]));
            this.MoveToNextLine((double) float.Parse(operands[0]), (double) float.Parse(operands[1]));
            if (this.isFoundText(this.CurrentLocation))
            {
              this.isContainsRedactionText = true;
              this.isNotUpdated = true;
            }
            if (recordCollection.RecordCollection.Count != index + 1 && !this.isContainsRedactionText)
            {
              string operatorName = recordCollection.RecordCollection[index + 1].OperatorName;
              if (operatorName == "TJ" || operatorName == "Tj" || operatorName == "'")
              {
                this.isContainsRedactionText = true;
                this.isNotUpdated = true;
                break;
              }
              break;
            }
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
          case "Do":
            this.isContainsImages = true;
            if (this.m_isType3Font)
            {
              this.GetType3XObject(operands);
              break;
            }
            if (this.m_resources.ContainsKey(operands[0].Replace("/", "")))
            {
              if (this.m_resources[operands[0].Replace("/", "")] is XObjectElement)
              {
                string empty = string.Empty;
                XObjectElement resource = this.m_resources[operands[0].Replace("/", "")] as XObjectElement;
                if (resource.ObjectType == "Form")
                {
                  PdfStream xobjectDictionary1 = resource.XObjectDictionary as PdfStream;
                  xobjectDictionary1.Decompress();
                  PdfRecordCollection contentElements = new ContentParser(xobjectDictionary1.InternalStream.ToArray()).ReadContent();
                  PageResourceLoader pageResourceLoader = new PageResourceLoader();
                  PdfDictionary pdfDictionary = new PdfDictionary();
                  PdfDictionary xobjectDictionary2 = resource.XObjectDictionary;
                  PdfPageResources pdfPageResources = new PdfPageResources();
                  if (xobjectDictionary2.ContainsKey("Resources"))
                  {
                    pdfDictionary = (object) (xobjectDictionary2["Resources"] as PdfReference) == null ? ((object) (xobjectDictionary2["Resources"] as PdfReferenceHolder) == null ? xobjectDictionary2["Resources"] as PdfDictionary : (xobjectDictionary2["Resources"] as PdfReferenceHolder).Object as PdfDictionary) : (xobjectDictionary2["Resources"] as PdfReferenceHolder).Object as PdfDictionary;
                    if (pdfDictionary.ContainsKey("Pattern"))
                    {
                      foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in (PdfCrossTable.Dereference(pdfDictionary["Pattern"]) as PdfDictionary).Items)
                      {
                        if (keyValuePair.Value != null && PdfCrossTable.Dereference(keyValuePair.Value) is PdfStream pdfStream && pdfStream.isSkip)
                          pdfStream.isSkip = false;
                      }
                    }
                    Dictionary<string, PdfMatrix> commonMatrix = new Dictionary<string, PdfMatrix>();
                    PdfPageResources pageResources1 = pageResourceLoader.UpdatePageResources(pdfPageResources, pageResourceLoader.GetImageResources(pdfDictionary, (PdfPageBase) null, ref commonMatrix));
                    PdfPageResources pageResources2 = pageResourceLoader.UpdatePageResources(pageResources1, pageResourceLoader.GetFontResources(pdfDictionary));
                    PdfPageResources pageResources3 = pageResourceLoader.UpdatePageResources(pageResources2, pageResourceLoader.GetExtendedGraphicResources(pdfDictionary));
                    PdfPageResources pageResources4 = pageResourceLoader.UpdatePageResources(pageResources3, pageResourceLoader.GetColorSpaceResource(pdfDictionary));
                    PdfPageResources pageResources5 = pageResourceLoader.UpdatePageResources(pageResources4, pageResourceLoader.GetShadingResource(pdfDictionary));
                    pdfPageResources = pageResourceLoader.UpdatePageResources(pageResources5, pageResourceLoader.GetPatternResource(pdfDictionary));
                  }
                  Syncfusion.PdfViewer.Base.Matrix matrix1 = Syncfusion.PdfViewer.Base.Matrix.Identity;
                  if (xobjectDictionary2.ContainsKey("Matrix"))
                  {
                    PdfArray pdfArray1 = new PdfArray();
                    if (xobjectDictionary2["Matrix"] is PdfArray && xobjectDictionary2["Matrix"] is PdfArray pdfArray2)
                    {
                      float floatValue1 = (pdfArray2[0] as PdfNumber).FloatValue;
                      float floatValue2 = (pdfArray2[1] as PdfNumber).FloatValue;
                      float floatValue3 = (pdfArray2[2] as PdfNumber).FloatValue;
                      float floatValue4 = (pdfArray2[3] as PdfNumber).FloatValue;
                      float floatValue5 = (pdfArray2[4] as PdfNumber).FloatValue;
                      float floatValue6 = (pdfArray2[5] as PdfNumber).FloatValue;
                      matrix1 = new Syncfusion.PdfViewer.Base.Matrix((double) floatValue1, (double) floatValue2, (double) floatValue3, (double) floatValue4, (double) floatValue5, (double) floatValue6);
                      if ((double) floatValue5 != 0.0 || (double) floatValue6 != 0.0)
                        this.m_graphics.TranslateTransform(floatValue5, -floatValue6);
                      if ((double) floatValue1 != 0.0 || (double) floatValue4 != 0.0)
                        this.m_graphics.ScaleTransform(floatValue1, floatValue4);
                      double d1 = Math.Round(180.0 / Math.PI * Math.Acos((double) floatValue1));
                      double d2 = Math.Round(180.0 / Math.PI * Math.Asin((double) floatValue2));
                      if (d1 == d2)
                        this.m_graphics.RotateTransform(-(float) d1);
                      else if (!double.IsNaN(d2))
                        this.m_graphics.RotateTransform(-(float) d2);
                      else if (!double.IsNaN(d1))
                        this.m_graphics.RotateTransform(-(float) d1);
                    }
                  }
                  if (pdfDictionary != null)
                  {
                    Syncfusion.PdfViewer.Base.DeviceCMYK cmyk = new Syncfusion.PdfViewer.Base.DeviceCMYK();
                    if (pdfPageResources.fontCollection.Count == 0 && this.m_resources.fontCollection.Count > 0)
                    {
                      foreach (KeyValuePair<string, FontStructure> font in this.m_resources.fontCollection)
                      {
                        pdfPageResources.Resources.Add(font.Key, (object) font.Value);
                        pdfPageResources.fontCollection.Add(font.Key, font.Value);
                      }
                    }
                    PdfImageRenderer pdfImageRenderer = new PdfImageRenderer(contentElements, pdfPageResources, this.m_graphics, false, cmyk, this.currentPageHeight);
                    pdfImageRenderer.RedactionBounds = this.RedactionBounds;
                    pdfImageRenderer.m_loadedPage = this.m_loadedPage;
                    pdfImageRenderer.m_objects = this.m_objects;
                    GraphicObjectData graphicObjectData3 = new GraphicObjectData();
                    if (pdfImageRenderer.m_objects.Count > 0)
                    {
                      GraphicObjectData graphicObjectData4 = pdfImageRenderer.m_objects.ToArray()[0];
                      graphicObjectData3.Ctm = graphicObjectData4.Ctm;
                      graphicObjectData3.m_mitterLength = graphicObjectData4.m_mitterLength;
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
                    pdfImageRenderer.m_objects.Push(graphicObjectData3);
                    GraphicsState graphicsState = this.m_graphics.Save();
                    pdfImageRenderer.m_graphicsState.Push(graphicsState);
                    Syncfusion.PdfViewer.Base.Matrix ctm = this.m_objects.ToArray()[0].Ctm;
                    Syncfusion.PdfViewer.Base.Matrix matrix2 = matrix1 * ctm;
                    this.m_objects.ToArray()[0].drawing2dMatrixCTM = new System.Drawing.Drawing2D.Matrix((float) matrix2.M11, (float) matrix2.M12, (float) matrix2.M21, (float) matrix2.M22, (float) matrix2.OffsetX, (float) matrix2.OffsetY);
                    this.m_objects.ToArray()[0].Ctm = matrix2;
                    if (empty != resource.ObjectName && xobjectDictionary2.ContainsKey("BBox"))
                    {
                      PdfArray pdfArray3 = new PdfArray();
                      if (xobjectDictionary2["BBox"] is PdfArray)
                      {
                        PdfArray pdfArray4 = xobjectDictionary2["BBox"] as PdfArray;
                        float floatValue7 = (pdfArray4[0] as PdfNumber).FloatValue;
                        float floatValue8 = (pdfArray4[1] as PdfNumber).FloatValue;
                        float width = (pdfArray4[2] as PdfNumber).FloatValue - floatValue7;
                        float height = (pdfArray4[3] as PdfNumber).FloatValue - floatValue8;
                        RectangleF rect = new RectangleF(floatValue7, floatValue8, width, height);
                        Syncfusion.PdfViewer.Base.Matrix documentMatrix = this.m_objects.ToArray()[0].documentMatrix;
                        Syncfusion.PdfViewer.Base.Matrix matrix3 = matrix1 * ctm * documentMatrix;
                        System.Drawing.Drawing2D.Matrix transform = this.m_graphics.Transform;
                        int pageUnit = (int) this.m_graphics.PageUnit;
                        this.m_graphics.PageUnit = GraphicsUnit.Pixel;
                        this.m_graphics.Transform = new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
                        this.m_graphics.Transform = new System.Drawing.Drawing2D.Matrix((float) matrix3.M11, (float) matrix3.M12, (float) matrix3.M21, (float) matrix3.M22, (float) matrix3.OffsetX, (float) matrix3.OffsetY);
                        this.m_graphics.SetClip(rect, CombineMode.Intersect);
                        string objectName = resource.ObjectName;
                      }
                    }
                    pdfImageRenderer.m_selectablePrintDocument = this.m_isPrintSelected;
                    pdfImageRenderer.m_pageHeight = this.m_pageHeight;
                    pdfImageRenderer.isXGraphics = true;
                    pdfImageRenderer.substitutedFontsList = this.substitutedFontsList;
                    pdfImageRenderer.zoomFactor = this.zoomFactor;
                    MemoryStream memoryStream = pdfImageRenderer.RenderAsImage();
                    pdfImageRenderer.m_objects.Pop();
                    if (pdfImageRenderer.m_graphicsState.Count > 0)
                      this.m_graphics.Restore(pdfImageRenderer.m_graphicsState.Pop());
                    pdfImageRenderer.m_graphicspathtoclip = (GraphicsPath) null;
                    pdfImageRenderer.IsGraphicsState = false;
                    pdfImageRenderer.isXGraphics = false;
                    for (; pdfImageRenderer.xobjectGraphicsCount > 0; --pdfImageRenderer.xobjectGraphicsCount)
                      this.m_objects.Pop();
                    this.imageRenderGlyphList.AddRange((IEnumerable<Syncfusion.PdfViewer.Base.Glyph>) pdfImageRenderer.imageRenderGlyphList);
                    this.m_objects.ToArray()[0].Ctm = ctm;
                    PdfStream pdfStream = new PdfStream();
                    pdfStream.Data = memoryStream.ToArray();
                    pdfStream.Compress = true;
                    memoryStream.Dispose();
                    xobjectDictionary1.Clear();
                    xobjectDictionary1.Items.Remove(new PdfName("Length"));
                    xobjectDictionary1.Data = pdfStream.Data;
                    xobjectDictionary1.Compress = true;
                    xobjectDictionary1.Modify();
                    break;
                  }
                  break;
                }
                break;
              }
              this.GetXObject(operands);
              break;
            }
            break;
          case "re":
            if (this.RenderingMode == 3 && this.transperentStrokingBrush != null && this.transperentStrokingBrush != null)
            {
              this.Objects.NonStrokingBrush = this.transperentNonStrokingBrush;
              this.Objects.StrokingBrush = this.transperentStrokingBrush;
            }
            if (index < recordCollection.RecordCollection.Count && recordCollection.RecordCollection[index + 1].OperatorName == "f")
              this.isNextFill = true;
            float x = float.Parse(operands[0]);
            float y = float.Parse(operands[1]);
            float width1 = float.Parse(operands[2]);
            float height1 = float.Parse(operands[3]);
            if (this.isFoundText(new PointF(x, y)))
            {
              this.isContainsRedactionText = true;
              this.isNotUpdated = true;
            }
            if (!flag1)
            {
              this.rectValue = new RectangleF(x, y, width1, height1);
              flag1 = true;
            }
            this.GetClipRectangle(operands);
            break;
          case "d":
            if (operands[0] != "[]" && !operands[0].Contains("\n"))
            {
              this.m_dashedLine = operands;
              break;
            }
            break;
          case "d0":
            this.m_d0Matrix = new Matrixx((double) float.Parse(operands[0]), (double) float.Parse(operands[1]));
            break;
          case "d1":
            this.m_d1Matrix = new Syncfusion.PdfViewer.Base.Matrix((double) float.Parse(operands[0]), (double) float.Parse(operands[1]), (double) float.Parse(operands[2]), (double) float.Parse(operands[3]), (double) float.Parse(operands[4]), (double) float.Parse(operands[5]));
            break;
          case "gs":
            this.IsTransparentText = false;
            if (this.m_resources.ContainsKey(operands[0].Substring(1)))
            {
              int num11 = 0;
              string str = (string) null;
              bool flag2 = true;
              PdfDictionary xobjectDictionary = (this.m_resources[operands[0].Substring(1)] as XObjectElement).XObjectDictionary;
              if (xobjectDictionary.ContainsKey("OPM"))
                num11 = (xobjectDictionary["OPM"] as PdfNumber).IntValue;
              if (xobjectDictionary.ContainsKey("SMask"))
                this.m_isExtendedGraphicStateContainsSMask = true;
              if (xobjectDictionary.ContainsKey("AIS"))
                flag2 = (xobjectDictionary["AIS"] as PdfBoolean).Value;
              bool flag3 = false;
              if ((double) this.Objects.m_nonStrokingOpacity == 0.0 || (double) this.Objects.m_strokingOpacity == 0.0)
                flag3 = true;
              if (xobjectDictionary.ContainsKey("HT"))
              {
                PdfName pdfName1 = xobjectDictionary["HT"] as PdfName;
                if (pdfName1 != (PdfName) null)
                {
                  str = pdfName1.Value;
                }
                else
                {
                  PdfName pdfName2 = (xobjectDictionary["HT"] as PdfReferenceHolder).Object as PdfName;
                  if (pdfName2 != (PdfName) null)
                    str = pdfName2.Value;
                }
              }
              else if (xobjectDictionary.ContainsKey("CA") || xobjectDictionary.ContainsKey("ca"))
              {
                if (xobjectDictionary.ContainsKey("CA"))
                  this.Objects.m_nonStrokingOpacity = (xobjectDictionary["CA"] as PdfNumber).FloatValue;
                if (xobjectDictionary.ContainsKey("ca"))
                {
                  if (flag2)
                    this.Objects.m_strokingOpacity = (xobjectDictionary["ca"] as PdfNumber).FloatValue;
                  else if (!this.isXGraphics)
                    this.Objects.m_strokingOpacity = (xobjectDictionary["ca"] as PdfNumber).FloatValue;
                }
                if ((double) this.Objects.m_nonStrokingOpacity == 0.0 || (double) this.Objects.m_strokingOpacity == 0.0)
                  flag3 = true;
                if (flag3)
                {
                  if (this.StrokingBrush != null && this.m_backupColorElements != null && this.m_backupColorSpace != null)
                  {
                    this.m_opacity = this.Objects.m_strokingOpacity;
                    this.SetStrokingColor(this.GetColor(this.m_backupColorElements, "Stroking", this.m_backupColorSpace));
                  }
                  if (this.NonStrokingBrush != null && this.m_backupColorElements != null && this.m_backupColorSpace != null)
                  {
                    this.m_opacity = this.Objects.m_nonStrokingOpacity;
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
              if (num11 == 1 && str == "Default")
              {
                this.IsExtendedGraphicsState = true;
                break;
              }
              break;
            }
            break;
          case "n":
            this.BackupCurrentGeometry = this.CurrentGeometry;
            this.CurrentGeometry = new PathGeometry();
            this.m_currentPath = (PathFigure) null;
            break;
          case "J":
            this.m_lineCap = float.Parse(operands[0]);
            break;
          case "w":
            this.MitterLength = float.Parse(operands[0]);
            break;
          case "W":
            if (!this.m_isType3Font)
            {
              this.m_clippingPath = tokenType;
              Syncfusion.PdfViewer.Base.Matrix transform1 = new Syncfusion.PdfViewer.Base.Matrix((double) this.Drawing2dMatrixCTM.Elements[0], (double) this.Drawing2dMatrixCTM.Elements[1], (double) this.Drawing2dMatrixCTM.Elements[2], (double) this.Drawing2dMatrixCTM.Elements[3], (double) this.Drawing2dMatrixCTM.OffsetX, (double) this.Drawing2dMatrixCTM.OffsetY) * this.DocumentMatrix;
              System.Drawing.Drawing2D.Matrix matrix = new System.Drawing.Drawing2D.Matrix((float) Math.Round(transform1.M11, 5, MidpointRounding.ToEven), (float) Math.Round(transform1.M12, 5, MidpointRounding.ToEven), (float) Math.Round(transform1.M21, 5, MidpointRounding.ToEven), (float) Math.Round(transform1.M22, 5, MidpointRounding.ToEven), (float) Math.Round(transform1.OffsetX, 5, MidpointRounding.ToEven), (float) Math.Round(transform1.OffsetY, 5, MidpointRounding.ToEven));
              System.Drawing.Drawing2D.Matrix transform2 = this.m_graphics.Transform;
              int pageUnit = (int) this.m_graphics.PageUnit;
              this.m_graphics.PageUnit = GraphicsUnit.Pixel;
              this.m_graphics.Transform = new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
              foreach (PathFigure figure in this.CurrentGeometry.Figures)
              {
                figure.IsClosed = true;
                figure.IsFilled = true;
              }
              this.CurrentGeometry.FillRule = FillRule.Nonzero;
              GraphicsPath geometry = this.GetGeometry(this.CurrentGeometry, transform1);
              bool flag4 = true;
              foreach (PointF pathPoint in geometry.PathPoints)
              {
                if ((double) pathPoint.X < 0.0 || (double) pathPoint.Y < 0.0)
                {
                  flag4 = false;
                  break;
                }
              }
              if (geometry.PointCount != 0 && flag4)
              {
                this.m_graphics.SetClip(geometry, CombineMode.Intersect);
                break;
              }
              break;
            }
            break;
          case "W*":
            this.m_clippingPath = tokenType;
            Syncfusion.PdfViewer.Base.Matrix transform3 = new Syncfusion.PdfViewer.Base.Matrix((double) this.Drawing2dMatrixCTM.Elements[0], (double) this.Drawing2dMatrixCTM.Elements[1], (double) this.Drawing2dMatrixCTM.Elements[2], (double) this.Drawing2dMatrixCTM.Elements[3], (double) this.Drawing2dMatrixCTM.OffsetX, (double) this.Drawing2dMatrixCTM.OffsetY) * this.DocumentMatrix;
            System.Drawing.Drawing2D.Matrix matrix4 = new System.Drawing.Drawing2D.Matrix((float) Math.Round(transform3.M11, 5, MidpointRounding.ToEven), (float) Math.Round(transform3.M12, 5, MidpointRounding.ToEven), (float) Math.Round(transform3.M21, 5, MidpointRounding.ToEven), (float) Math.Round(transform3.M22, 5, MidpointRounding.ToEven), (float) Math.Round(transform3.OffsetX, 5, MidpointRounding.ToEven), (float) Math.Round(transform3.OffsetY, 5, MidpointRounding.ToEven));
            System.Drawing.Drawing2D.Matrix transform4 = this.m_graphics.Transform;
            int pageUnit1 = (int) this.m_graphics.PageUnit;
            this.m_graphics.PageUnit = GraphicsUnit.Pixel;
            this.m_graphics.Transform = new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
            foreach (PathFigure figure in this.CurrentGeometry.Figures)
            {
              figure.IsClosed = true;
              figure.IsFilled = true;
            }
            this.CurrentGeometry.FillRule = FillRule.EvenOdd;
            GraphicsPath geometry1 = this.GetGeometry(this.CurrentGeometry, transform3);
            if (geometry1.PointCount != 0)
            {
              this.m_graphics.SetClip(geometry1, CombineMode.Intersect);
              break;
            }
            break;
          case "m":
            this.BackupCurrentGeometry = this.CurrentGeometry;
            this.CurrentGeometry = new PathGeometry();
            this.m_currentPath = (PathFigure) null;
            if (this.FindRedactpath(recordCollection, index))
            {
              this.removePathLines = true;
              break;
            }
            break;
        }
        this.recordCount = recordCollection.RecordCollection.Count;
        if (!changeOperator)
          updatedText = (string) null;
        if (!isSkip && !this.removePathLines)
          this.OptimizeContent(recordCollection, index, updatedText, stream);
        if (this.removePathLines && (tokenType.Trim() == "h" || tokenType.Trim() == "f" || tokenType.Trim() == "f*"))
        {
          string operatorName = recordCollection.RecordCollection[index + 1].OperatorName;
          if (tokenType.Trim() == "h" && (operatorName != "f" || operatorName != "f*"))
            this.removePathLines = false;
          else if (tokenType.Trim() == "f")
            this.removePathLines = false;
        }
      }
      stream.Write("\r\n");
      this.m_isType3Font = false;
    }
    MemoryStream memoryStream1 = new MemoryStream();
    byte[] buffer = new byte[4096 /*0x1000*/];
    stream.InternalStream.Position = 0L;
    int count;
    while ((count = stream.InternalStream.Read(buffer, 0, buffer.Length)) > 0)
      memoryStream1.Write(buffer, 0, count);
    stream.Clear();
    stream.InternalStream.Dispose();
    stream.InternalStream.Close();
    stream.InternalStream = (MemoryStream) null;
    return memoryStream1;
  }

  private void AddLine(string[] line)
  {
    this.CurrentLocation = new PointF(this.FloatParse(line[0]), this.FloatParse(line[1]));
    this.m_currentPath.Segments.Add((PathSegment) new LineSegment()
    {
      Point = (Syncfusion.PdfViewer.Base.Point) this.CurrentLocation
    });
  }

  private void BeginPath(string[] point)
  {
    this.CurrentLocation = new PointF(this.FloatParse(point[0]), this.FloatParse(point[1]));
    if (this.m_currentPath != null && this.m_currentPath.Segments.Count == 0)
      this.CurrentGeometry.Figures.Remove(this.CurrentPath);
    this.m_currentPath = new PathFigure();
    this.m_currentPath.StartPoint = (Syncfusion.PdfViewer.Base.Point) this.CurrentLocation;
    this.CurrentGeometry.Figures.Add(this.m_currentPath);
  }

  private void AddBezierCurve(string[] curve)
  {
    this.m_currentPath.Segments.Add((PathSegment) new BezierSegment()
    {
      Point1 = (Syncfusion.PdfViewer.Base.Point) new PointF(this.FloatParse(curve[0]), this.FloatParse(curve[1])),
      Point2 = (Syncfusion.PdfViewer.Base.Point) new PointF(this.FloatParse(curve[2]), this.FloatParse(curve[3])),
      Point3 = (Syncfusion.PdfViewer.Base.Point) new PointF(this.FloatParse(curve[4]), this.FloatParse(curve[5]))
    });
    this.CurrentLocation = new PointF(this.FloatParse(curve[4]), this.FloatParse(curve[5]));
  }

  private void AddBezierCurve2(string[] curve)
  {
    this.m_currentPath.Segments.Add((PathSegment) new BezierSegment()
    {
      Point1 = (Syncfusion.PdfViewer.Base.Point) this.CurrentLocation,
      Point2 = (Syncfusion.PdfViewer.Base.Point) new PointF(this.FloatParse(curve[0]), this.FloatParse(curve[1])),
      Point3 = (Syncfusion.PdfViewer.Base.Point) new PointF(this.FloatParse(curve[2]), this.FloatParse(curve[3]))
    });
    this.CurrentLocation = new PointF(this.FloatParse(curve[2]), this.FloatParse(curve[3]));
  }

  private void AddBezierCurve3(string[] curve)
  {
    this.m_currentPath.Segments.Add((PathSegment) new BezierSegment()
    {
      Point1 = (Syncfusion.PdfViewer.Base.Point) new PointF(this.FloatParse(curve[0]), this.FloatParse(curve[1])),
      Point2 = (Syncfusion.PdfViewer.Base.Point) new PointF(this.FloatParse(curve[2]), this.FloatParse(curve[3])),
      Point3 = (Syncfusion.PdfViewer.Base.Point) new PointF(this.FloatParse(curve[2]), this.FloatParse(curve[3]))
    });
    this.CurrentLocation = new PointF(this.FloatParse(curve[2]), this.FloatParse(curve[3]));
  }

  private void EndPathLine()
  {
    if (this.m_currentPath == null)
      return;
    this.m_currentPath.IsClosed = true;
  }

  private float FloatParse(string textString)
  {
    return float.Parse(textString, (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat);
  }

  internal bool FindRedactpath(PdfRecordCollection recordCollection, int i)
  {
    bool redactpath = false;
    for (int index = i; index < recordCollection.RecordCollection.Count; ++index)
    {
      string str = recordCollection.RecordCollection[index].OperatorName;
      string[] operands = recordCollection.RecordCollection[index].Operands;
      foreach (char symbolChar in this.m_symbolChars)
      {
        if (str.Contains(symbolChar.ToString()))
          str = str.Replace(symbolChar.ToString(), "");
      }
      switch (str.Trim())
      {
        case "h":
          Syncfusion.PdfViewer.Base.Matrix transform = new Syncfusion.PdfViewer.Base.Matrix((double) this.Drawing2dMatrixCTM.Elements[0], (double) this.Drawing2dMatrixCTM.Elements[1], (double) this.Drawing2dMatrixCTM.Elements[2], (double) this.Drawing2dMatrixCTM.Elements[3], (double) this.Drawing2dMatrixCTM.OffsetX, (double) this.Drawing2dMatrixCTM.OffsetY) * this.DocumentMatrix;
          System.Drawing.Drawing2D.Matrix matrix = new System.Drawing.Drawing2D.Matrix((float) Math.Round(transform.M11, 5, MidpointRounding.ToEven), (float) Math.Round(transform.M12, 5, MidpointRounding.ToEven), (float) Math.Round(transform.M21, 5, MidpointRounding.ToEven), (float) Math.Round(transform.M22, 5, MidpointRounding.ToEven), (float) Math.Round(transform.OffsetX, 5, MidpointRounding.ToEven), (float) Math.Round(transform.OffsetY, 5, MidpointRounding.ToEven));
          GraphicsPath geometry = this.GetGeometry(this.CurrentGeometry, transform);
          RectangleF bounds = geometry.GetBounds();
          bounds.X /= this.pt;
          bounds.Y /= this.pt;
          bounds.Width /= this.pt;
          bounds.Height /= this.pt;
          RectangleF empty = RectangleF.Empty;
          foreach (PdfRedaction redaction in this.m_loadedPage.Redactions)
          {
            RectangleF rectangle = RectangleF.Intersect(new RectangleF(redaction.Bounds.X, redaction.Bounds.Y, redaction.Bounds.Width, redaction.Bounds.Height), bounds);
            if (rectangle != RectangleF.Empty)
            {
              if (rectangle.Contains(bounds) || rectangle.Equals((object) bounds) || (int) rectangle.X == (int) bounds.X && (int) rectangle.Y == (int) bounds.Y && (int) rectangle.Width == (int) bounds.Width && (int) rectangle.Height == (int) bounds.Height)
                return true;
              PdfPath pdfPath = new PdfPath();
              redaction.m_success = true;
              pdfPath.AddRectangle(rectangle);
              this.PdfPaths.Add(pdfPath);
            }
          }
          if (geometry.PointCount != 0 && this.Path != null)
          {
            this.Path.CloseAllFigures();
            this.m_subPaths.Add(this.Path);
            this.m_tempSubPaths.Clear();
            this.Path = new GraphicsPath();
          }
          if (this.m_currentPath != null)
            this.m_currentPath.IsClosed = true;
          return false;
        case "m":
          this.BeginPath(operands);
          break;
        case "c":
          this.AddBezierCurve(operands);
          break;
        case "l":
          this.AddLine(operands);
          break;
        case "v":
          this.AddBezierCurve2(operands);
          break;
        case "y":
          this.AddBezierCurve3(operands);
          break;
      }
    }
    return redactpath;
  }

  internal void OptimizeContent(
    PdfRecordCollection recordCollection,
    int i,
    string updatedText,
    PdfStream stream)
  {
    PdfRecord record = recordCollection.RecordCollection[i];
    if (record.Operands != null && record.Operands.Length >= 1)
    {
      if (record.OperatorName == "ID")
      {
        StringBuilder stringBuilder = new StringBuilder();
        for (int index = 0; index < record.Operands.Length; ++index)
        {
          if (index + 1 < record.Operands.Length && record.Operands[index].Contains("/") && record.Operands[index + 1].Contains("/"))
          {
            stringBuilder.Append(record.Operands[index]);
            stringBuilder.Append(" ");
            stringBuilder.Append(record.Operands[index + 1]);
            stringBuilder.Append("\r\n");
            ++index;
          }
          else if (index + 1 < record.Operands.Length && record.Operands[index].Contains("/"))
          {
            stringBuilder.Append(record.Operands[index]);
            stringBuilder.Append(" ");
            stringBuilder.Append(record.Operands[index + 1]);
            stringBuilder.Append("\r\n");
            ++index;
          }
          else
          {
            stringBuilder.Append(record.Operands[index]);
            stringBuilder.Append("\r\n");
          }
        }
        byte[] bytes = Encoding.Default.GetBytes(stringBuilder.ToString());
        stream.Write(bytes);
      }
      else
      {
        for (int index = 0; index < record.Operands.Length; ++index)
        {
          string str = record.Operands[index];
          if ((record.OperatorName == "Tj" || record.OperatorName == "'" || record.OperatorName == "\"" || record.OperatorName == "TJ") && updatedText != null)
          {
            str = updatedText;
            if (record.OperatorName == "'")
            {
              stream.Write("T*");
              stream.Write(" ");
            }
            record.OperatorName = "TJ";
          }
          PdfString pdfString = new PdfString(str);
          stream.Write(pdfString.Bytes);
          if (record.OperatorName != "Tj" && record.OperatorName != "'" && record.OperatorName != "\"" && record.OperatorName != "TJ")
            stream.Write(" ");
        }
      }
    }
    else if (record.Operands == null && record.InlineImageBytes != null)
    {
      byte[] bytes = Encoding.Default.GetBytes(Encoding.Default.GetString(record.InlineImageBytes));
      stream.Write(bytes);
      stream.Write(" ");
    }
    stream.Write(record.OperatorName);
    if (i + 1 >= this.recordCount)
      return;
    if (record.OperatorName == "ID")
      stream.Write("\n");
    else if (i + 1 < this.recordCount && (record.OperatorName == "W" || record.OperatorName == "W*") && recordCollection.RecordCollection[i + 1].OperatorName == "n")
      stream.Write(" ");
    else if (record.OperatorName == "w" || record.OperatorName == "EI")
      stream.Write(" ");
    else
      stream.Write("\r\n");
  }

  private List<StringMapping> MapString(
    string[] mainTextCollection,
    List<Syncfusion.PdfViewer.Base.Glyph> imgGlyph,
    FontStructure structure)
  {
    List<StringMapping> stringMappingList = new List<StringMapping>();
    Syncfusion.PdfViewer.Base.Glyph[] array = imgGlyph.ToArray();
    int sourceIndex = 0;
    for (int index1 = 0; index1 < mainTextCollection.Length; ++index1)
    {
      int index2 = mainTextCollection[index1].Length - 1;
      if (mainTextCollection[index1][0] != '(' && mainTextCollection[index1][index2] != ')')
        stringMappingList.Add(new StringMapping()
        {
          text = mainTextCollection[index1]
        });
      else if (structure.FontEncoding == "Identity-H" || structure.FontEncoding == "" && structure.CharacterMapTable != null && structure.CharacterMapTable.Count > 0)
      {
        StringMapping stringMapping = new StringMapping();
        stringMapping.text = mainTextCollection[index1];
        string mainText = mainTextCollection[index1];
        string str1 = mainText.Substring(1, mainText.Length - 2);
        int length = str1.Length;
        int num = 0;
        if (length > 1)
        {
          int index3 = sourceIndex;
          while (num < str1.Length)
          {
            string toUnicode = array[index3].ToUnicode;
            if (toUnicode != null)
            {
              char[] charArray = toUnicode.ToCharArray();
              if (structure.CharacterMapTable.ContainsKey((double) charArray[0]))
              {
                string str2 = structure.CharacterMapTable[(double) charArray[0]];
                num += str2.Length;
                ++index3;
              }
              else
              {
                num += toUnicode.Length;
                ++index3;
              }
            }
            else
              ++index3;
          }
          length = index3 - sourceIndex;
        }
        else if (array[sourceIndex].ToUnicode == null)
          ++length;
        stringMapping.glyph = new Syncfusion.PdfViewer.Base.Glyph[length];
        System.Array.Copy((System.Array) array, sourceIndex, (System.Array) stringMapping.glyph, 0, length);
        sourceIndex += length;
        stringMappingList.Add(stringMapping);
      }
      else
      {
        StringMapping stringMapping = new StringMapping();
        stringMapping.text = mainTextCollection[index1];
        string mainText = mainTextCollection[index1];
        bool flag1 = mainText.Length >= 2;
        bool flag2 = mainText.StartsWith("(");
        bool flag3 = mainText.EndsWith(")");
        string str;
        if (flag1 && flag2 && !flag3)
          str = mainText.Substring(1, mainText.Length - 1);
        else if (flag1 && !flag2 && flag3)
          str = mainText.Substring(0, mainText.Length - 1);
        else if (flag1)
          str = mainText.Substring(1, mainText.Length - 2);
        else
          continue;
        int length = str.Length;
        stringMapping.glyph = new Syncfusion.PdfViewer.Base.Glyph[length];
        System.Array.Copy((System.Array) array, sourceIndex, (System.Array) stringMapping.glyph, 0, length);
        sourceIndex += length;
        stringMappingList.Add(stringMapping);
      }
    }
    return stringMappingList;
  }

  private string ReplaceText(
    string text,
    List<Syncfusion.PdfViewer.Base.Glyph> imageGlyph,
    out bool isSkip,
    out bool changeOperator)
  {
    if (imageGlyph.Count == 0)
    {
      isSkip = false;
      changeOperator = false;
      return text;
    }
    bool flag1 = false;
    bool flag2 = false;
    isSkip = false;
    changeOperator = false;
    bool isHex = false;
    if (this.m_resources.ContainsKey(this.CurrentFont))
    {
      FontStructure resource = this.m_resources[this.CurrentFont] as FontStructure;
      if (resource.CharacterMapTable != null && resource.CharacterMapTable.Count > 0)
      {
        IPdfPrimitive font = resource.FontDictionary["ToUnicode"];
        ((object) (font as PdfReferenceHolder) == null ? (PdfDictionary) (font as PdfStream) : (PdfDictionary) ((font as PdfReferenceHolder).Object as PdfStream)).isSkip = false;
      }
      int num = text.IndexOf('<');
      text.IndexOf('>');
      if (num >= 0)
        isHex = true;
      resource.IsSameFont = this.m_resources.isSameFont();
      if ((double) resource.FontSize != (double) this.FontSize)
        resource.FontSize = this.FontSize;
      if (resource.FontGlyphWidths == null || resource.FontGlyphWidths.Count != 0)
      {
        if (resource.FontEncoding == "MacRomanEncoding" && text.Length != imageGlyph.Count)
        {
          if (text[0] == '(' || text[0] == '<')
            text = $"({resource.Decode(text, true)})";
          else if (text[0] == '[')
          {
            string decodedString = string.Empty;
            this.DecodeTextTJ(resource, text, true, out decodedString);
            text = decodedString;
          }
        }
        else if (resource.FontEncoding == "Identity-H" && resource.CharacterMapTable.Count > 0)
        {
          if (text[0] == '(')
            text = $"({resource.DecodeTextExtraction(text, true)})";
          else if (text[0] == '[')
          {
            List<string> stringList = this.AddEscapeSymbols(resource.DecodeTextExtractionTJ(text, true));
            string str = "[";
            for (int index = 0; index < stringList.Count; ++index)
              str = stringList[index].Length == 0 || stringList[index][stringList[index].Length - 1] != 's' ? str + stringList[index] : $"{str}({stringList[index].Substring(0, stringList[index].Length - 1)})";
            text = str + "]";
          }
        }
        else if (text[0] == '(' || text[0] == '<')
          text = $"({resource.DecodeTextExtraction(text, true)})";
        else if (text[0] == '[')
        {
          List<string> stringList = resource.DecodeTextExtractionTJ(text, true);
          string str = "[";
          for (int index = 0; index < stringList.Count; ++index)
            str = stringList[index].Length == 0 || stringList[index][stringList[index].Length - 1] != 's' ? str + stringList[index] : $"{str}({stringList[index].Substring(0, stringList[index].Length - 1)})";
          text = str + "]";
        }
      }
    }
    foreach (Syncfusion.PdfViewer.Base.Glyph glyph in imageGlyph)
    {
      if (this.isFoundRect((RectangleF) glyph.BoundingRect))
      {
        flag1 = true;
        glyph.IsReplace = true;
      }
      else
      {
        flag2 = true;
        glyph.IsReplace = false;
      }
    }
    if (!flag1 && flag2)
    {
      changeOperator = false;
      return text;
    }
    string[] mainTextCollection = (string[]) null;
    this.fontStructure = this.m_resources[this.CurrentFont] as FontStructure;
    if (text[0] == '(')
    {
      if (this.fontStructure.FontName == "ZapfDingbats" && !this.fontStructure.isEmbedded)
        mainTextCollection = this.GetSplittedString($"({this.fontStructure.Decode(text, true)})");
      else
        mainTextCollection = new string[1]{ text };
    }
    else if (text[0] == '[')
    {
      text = text.TrimStart('[');
      text = text.TrimEnd(']');
      mainTextCollection = this.GetSplittedString(text);
      if (this.fontStructure.FontName == "ZapfDingbats" && !this.fontStructure.isEmbedded)
      {
        for (int index = 0; index < mainTextCollection.Length; ++index)
        {
          if (mainTextCollection[index].StartsWith("(") || mainTextCollection[index].StartsWith("<"))
            mainTextCollection[index] = $"({this.fontStructure.Decode(mainTextCollection[index], true)})";
        }
      }
    }
    else if (text[0] == '<')
      mainTextCollection = new string[1]
      {
        $"({this.fontStructure.Decode(text, true)})"
      };
    if ((this.fontStructure.FontEncoding == "Identity-H" || this.fontStructure.FontEncoding == "Encoding" || this.fontStructure.FontEncoding == "WinAnsiEncoding") && mainTextCollection.Length == this.decodedStringData.Count)
    {
      for (int index = 0; index < mainTextCollection.Length; ++index)
      {
        if (mainTextCollection[index][0] == '(')
          mainTextCollection[index] = $"({this.decodedStringData[index]})";
      }
    }
    List<StringMapping> stringMappingList = (List<StringMapping>) null;
    try
    {
      stringMappingList = this.MapString(mainTextCollection, imageGlyph, this.fontStructure);
    }
    catch (Exception ex)
    {
    }
    changeOperator = true;
    string empty = string.Empty;
    foreach (StringMapping stringMapping in stringMappingList)
      empty += stringMapping.GetText(this.fontStructure, isHex);
    string str1 = empty.Insert(0, "[");
    return str1.Insert(str1.Length, "]");
  }

  private List<string> DecodeTextTJ(
    FontStructure structure,
    string textToDecode,
    bool isSameFont,
    out string decodedString)
  {
    decodedString = string.Empty;
    string str1 = string.Empty;
    string str2 = textToDecode;
    structure.IsSameFont = isSameFont;
    List<string> stringList = new List<string>();
    structure.IsHexaDecimalString = false;
    switch (str2[0])
    {
      case '(':
        if (str2.Contains("\\\n"))
        {
          StringBuilder stringBuilder = new StringBuilder(str2);
          stringBuilder.Replace("\\\n", "");
          str2 = stringBuilder.ToString();
        }
        string encodedText = str2.Substring(1, str2.Length - 2);
        string literalString = structure.GetLiteralString(encodedText);
        str1 = structure.SkipEscapeSequence(literalString);
        if (structure.FontDictionary.ContainsKey("Encoding") && (object) (structure.FontDictionary["Encoding"] as PdfName) != null && (structure.FontDictionary["Encoding"] as PdfName).Value == "Identity-H")
        {
          List<byte> byteList = new List<byte>();
          foreach (char ch in str1)
            byteList.Add((byte) ch);
          str1 = Encoding.BigEndianUnicode.GetString(byteList.ToArray());
        }
        if (structure.FontName == "ZapfDingbats" && !structure.isEmbedded)
        {
          decodedString = $"({structure.MapZapf(str1)})";
          break;
        }
        break;
      case '<':
        string hexEncodedText = str2.Substring(1, str2.Length - 2);
        str1 = structure.GetHexaDecimalString(hexEncodedText);
        if (structure.FontName == "ZapfDingbats" && !structure.isEmbedded)
          structure.MapZapf(str1);
        ref string local1 = ref decodedString;
        local1 = $"{local1}({str1})";
        break;
      case '[':
        if (str2.Contains("\\\n"))
        {
          StringBuilder stringBuilder = new StringBuilder(str2);
          stringBuilder.Replace("\\\n", "");
          str2 = stringBuilder.ToString();
        }
        string str3 = str2.Substring(1, str2.Length - 2);
        decodedString += "[";
        int num1;
        for (; str3.Length > 0; str3 = str3.Substring(num1 + 1, str3.Length - num1 - 1))
        {
          bool flag = false;
          int length = str3.IndexOf('(');
          num1 = str3.IndexOf(')');
          for (int index = num1 + 1; index < str3.Length && str3[index] != '('; ++index)
          {
            if (str3[index] == ')')
            {
              num1 = index;
              break;
            }
          }
          int num2 = str3.IndexOf('<');
          int num3 = str3.IndexOf('>');
          if (num2 < length && num2 > -1)
          {
            length = num2;
            num1 = num3;
            flag = true;
          }
          if (length < 0)
          {
            length = str3.IndexOf('<');
            num1 = str3.IndexOf('>');
            if (length >= 0)
            {
              flag = true;
            }
            else
            {
              string str4 = str3;
              decodedString += str4;
              stringList.Add(str4);
              break;
            }
          }
          if (num1 < 0 && str3.Length > 0)
          {
            string str5 = str3;
            decodedString += str5;
            stringList.Add(str5);
            break;
          }
          if (num1 > 0)
          {
            while (str3[num1 - 1] == '\\' && (num1 - 1 <= 0 || str3[num1 - 2] != '\\') && str3.IndexOf(')', num1 + 1) >= 0)
              num1 = str3.IndexOf(')', num1 + 1);
          }
          if (length != 0)
          {
            string str6 = str3.Substring(0, length);
            decodedString += str6;
            stringList.Add(str6);
          }
          string str7 = str3.Substring(length + 1, num1 - length - 1);
          string str8;
          if (flag)
          {
            str8 = structure.GetHexaDecimalString(str7);
            if (str8.Contains("\\"))
              str8 = str8.Replace("\\", "\\\\");
            if (structure.FontName == "ZapfDingbats" && !structure.isEmbedded)
              str8 = structure.MapZapf(str8);
            ref string local2 = ref decodedString;
            local2 = $"{local2}({str8})";
            str1 += str8;
          }
          else
          {
            str8 = structure.GetLiteralString(str7);
            if (structure.FontName == "ZapfDingbats" && !structure.isEmbedded)
              str8 = structure.MapZapf(str8);
            ref string local3 = ref decodedString;
            local3 = $"{local3}({str8})";
            str1 += str8;
          }
          if (str8.Contains("\\000"))
            str8 = str8.Replace("\\000", "");
          if (str8.Contains("\0") && (!structure.CharacterMapTable.ContainsKey(0.0) || structure.CharacterMapTable.ContainsValue("\0")) && (structure.CharacterMapTable.Count > 0 || structure.IsCID && !structure.FontDictionary.ContainsKey("ToUnicode")))
            str8 = str8.Replace("\0", "");
          if (!structure.IsTextExtraction)
            str8 = structure.SkipEscapeSequence(str8);
          if (structure.CidToGidMap != null)
            str8 = structure.MapCidToGid(str8);
          if (str8.Length > 0)
          {
            if (str8[0] >= '\u0E00' && str8[0] <= '\u0E7F' && stringList.Count > 0)
            {
              string str9 = stringList[0];
              string str10 = str9.Remove(str9.Length - 1) + str8;
              stringList[0] = str10 + "s";
            }
            else if ((str8[0] == ' ' || str8[0] == '/') && str8.Length > 1)
            {
              if (str8[1] >= '\u0E00' && str8[1] <= '\u0E7F' && stringList.Count > 0)
              {
                string str11 = stringList[0];
                string str12 = str11.Remove(str11.Length - 1) + str8;
                stringList[0] = str12 + "s";
              }
              else
              {
                string str13 = str8 + "s";
                stringList.Add(str13);
              }
            }
            else
            {
              string str14 = str8 + "s";
              stringList.Add(str14);
            }
          }
          else
          {
            string str15 = str8 + "s";
            stringList.Add(str15);
          }
        }
        decodedString += "]";
        break;
    }
    structure.SkipEscapeSequence(str1);
    return stringList;
  }

  private List<string> AddEscapeSymbols(List<string> decodedList)
  {
    List<string> stringList = new List<string>();
    foreach (string decoded in decodedList)
    {
      if (decoded.Length > 0 && decoded[decoded.Length - 1] == 's')
      {
        MemoryStream memoryStream = new MemoryStream();
        for (int index = 0; index < decoded.Length; ++index)
        {
          char ch = decoded[index];
          switch ((byte) ch)
          {
            case 13:
              memoryStream.WriteByte((byte) 92);
              memoryStream.WriteByte((byte) 112 /*0x70*/);
              break;
            case 40:
            case 41:
            case 92:
              memoryStream.WriteByte((byte) 92);
              memoryStream.WriteByte((byte) ch);
              break;
            default:
              memoryStream.WriteByte((byte) ch);
              break;
          }
        }
        if (memoryStream.Length > 0L)
          stringList.Add(Encoding.Default.GetString(memoryStream.ToArray()));
      }
      else
        stringList.Add(decoded);
    }
    return stringList;
  }

  private string[] GetSplittedString(string text)
  {
    List<string> stringList = new List<string>();
    string empty1 = string.Empty;
    for (int index = 0; index < text.Length; ++index)
    {
      char ch = text[index];
      if (ch != '\\')
      {
        if (ch == '(' && index + 1 < text.Length && index + 2 < text.Length && (text[index + 1] == '(' || text[index + 1] == ')') && text[index + 2] == ')')
        {
          if (empty1 != string.Empty)
            stringList.Add(empty1);
          string empty2 = string.Empty;
          string str = ch.ToString() + (object) text[index + 1] + (object) text[index + 2];
          stringList.Add(str);
          empty1 = string.Empty;
          index += 2;
        }
        else if ((ch == '(' || ch == '<') && index - 1 >= 0 && text[index - 1] != '\\')
        {
          if (empty1 != string.Empty)
            stringList.Add(empty1);
          empty1 = ch.ToString();
        }
        else if ((ch == ')' || ch == '>') && index - 1 >= 0 && text[index - 1] != '\\')
        {
          if (empty1 != string.Empty)
          {
            string str = empty1 + ch.ToString();
            stringList.Add(str);
            empty1 = string.Empty;
          }
        }
        else
          empty1 += ch.ToString();
      }
    }
    if (empty1 != string.Empty)
      stringList.Add(empty1);
    return stringList.ToArray();
  }

  private bool isFoundRect(RectangleF rect)
  {
    bool flag = false;
    rect = this.GetRelativeBounds(rect);
    foreach (PdfRedaction redaction in this.m_loadedPage.Redactions)
    {
      PointF pt1 = new PointF(rect.X, rect.Y);
      PointF pt2 = new PointF(rect.X + rect.Width, rect.Y);
      PointF pt3 = new PointF(rect.X, rect.Y + rect.Height);
      PointF pt4 = new PointF(rect.X + rect.Width, rect.Y + rect.Height);
      RectangleF rectangleF = new RectangleF(redaction.Bounds.X, redaction.Bounds.Y, redaction.Bounds.Width, redaction.Bounds.Height);
      PointF pt5 = new PointF(rect.X + rect.Width / 2f, rect.Y + rect.Height / 2f);
      if (rectangleF.Contains(pt5) && (rectangleF.Contains(rect) || rectangleF.IntersectsWith(rect) || rectangleF.Contains(pt1) || rectangleF.Contains(pt2) || rectangleF.Contains(pt3) || rectangleF.Contains(pt4)))
      {
        redaction.m_success = true;
        flag = true;
        break;
      }
      if (this.isContainsRedactionText && !this.isNotUpdated)
        redaction.m_success = true;
    }
    return flag;
  }

  private bool isFoundText(PointF location)
  {
    bool flag = false;
    if ((double) location.Y < 0.0)
      location.Y = -location.Y;
    location = this.GetRelativelocation(location);
    int num1 = (int) location.Y;
    double x = (double) location.X;
    foreach (PdfRedaction redaction in this.m_loadedPage.Redactions)
    {
      RectangleF bounds = redaction.Bounds;
      if (this.isContainsImages)
      {
        flag = true;
        break;
      }
      if ((int) bounds.Y == num1 || (int) bounds.Y == num1 - 1 || (int) bounds.Y == num1 + 1)
      {
        flag = true;
        break;
      }
      int y = (int) location.Y;
      if ((double) bounds.Y >= (double) y && (double) y >= (double) bounds.Y - (double) bounds.Height || (double) bounds.Y <= (double) y && (double) y <= (double) bounds.Y + (double) bounds.Height)
      {
        flag = true;
        break;
      }
      num1 = (int) ((double) this.m_loadedPage.Size.Height - (double) location.Y);
      if ((double) bounds.Y >= (double) num1 && (double) num1 >= (double) bounds.Y - (double) bounds.Height || (double) bounds.Y <= (double) num1 && (double) num1 <= (double) bounds.Y + (double) bounds.Height)
      {
        flag = true;
        break;
      }
      if ((double) this.rectValue.Y != 0.0)
      {
        if ((double) this.rectValue.Y < 0.0)
          this.rectValue.Y = -this.rectValue.Y;
        int num2 = (int) ((double) location.Y + (double) this.rectValue.Y);
        if ((double) bounds.Y >= (double) num2 && (double) num2 >= (double) bounds.Y - (double) bounds.Height || (double) bounds.Y <= (double) num2 && (double) num2 <= (double) bounds.Y + (double) bounds.Height)
        {
          flag = true;
          break;
        }
        if ((double) this.rectValue.Height < 0.0)
          this.rectValue.Height = -this.rectValue.Height;
        num1 = (int) ((double) this.rectValue.Height - (double) num2);
        if ((double) bounds.Y >= (double) num1 && (double) num1 >= (double) bounds.Y - (double) bounds.Height || (double) bounds.Y <= (double) num1 && (double) num1 <= (double) bounds.Y + (double) bounds.Height)
        {
          flag = true;
          break;
        }
      }
    }
    return flag;
  }

  private PointF GetRelativelocation(PointF location)
  {
    SizeF size = this.m_loadedPage.Size;
    PointF relativelocation = location;
    if (this.m_loadedPage.Rotation == PdfPageRotateAngle.RotateAngle90)
    {
      relativelocation.X = size.Height - location.Y;
      relativelocation.Y = location.X;
    }
    else if (this.m_loadedPage.Rotation == PdfPageRotateAngle.RotateAngle270)
    {
      relativelocation.Y = size.Width - location.X;
      relativelocation.X = location.Y;
    }
    return relativelocation;
  }

  private RectangleF GetRelativeBounds(RectangleF bounds)
  {
    SizeF size = this.m_loadedPage.Size;
    RectangleF relativeBounds = bounds;
    if (this.m_loadedPage.Rotation == PdfPageRotateAngle.RotateAngle90)
    {
      relativeBounds.X = size.Height - (bounds.Y + bounds.Height);
      relativeBounds.Y = bounds.X;
      relativeBounds.Width = bounds.Height;
      relativeBounds.Height = bounds.Width;
    }
    else if (this.m_loadedPage.Rotation == PdfPageRotateAngle.RotateAngle270)
    {
      relativeBounds.Y = size.Width - (bounds.X + bounds.Width);
      relativeBounds.X = bounds.Y;
      relativeBounds.Width = bounds.Height;
      relativeBounds.Height = bounds.Width;
    }
    else if (this.m_loadedPage.Rotation == PdfPageRotateAngle.RotateAngle180)
    {
      relativeBounds.X = size.Width - (bounds.X + bounds.Width);
      relativeBounds.Y = size.Height - (bounds.Y + bounds.Height);
    }
    return relativeBounds;
  }

  private void SetStrokingColor(Color color) => this.Objects.StrokingBrush = new Pen(color).Brush;

  private void SetNonStrokingColor(Color color)
  {
    this.Objects.NonStrokingBrush = new Pen(color).Brush;
  }

  private void MoveToNextLineWithLeading(string[] element)
  {
    this.SetTextLeading(-float.Parse(element[1]));
    this.MoveToNextLine((double) float.Parse(element[0]), (double) float.Parse(element[1]));
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
    this.FontSize = float.Parse(fontElements[index + 1]);
  }

  private void RenderTextElement(string[] textElements, string tokenType)
  {
    string str1 = string.Join("", textElements);
    if (!this.m_resources.ContainsKey(this.CurrentFont))
      return;
    (this.m_resources[this.CurrentFont] as FontStructure).IsSameFont = this.m_resources.isSameFont();
    if ((double) (this.m_resources[this.CurrentFont] as FontStructure).FontSize != (double) this.FontSize)
      (this.m_resources[this.CurrentFont] as FontStructure).FontSize = this.FontSize;
    FontStructure structure = this.m_resources[this.CurrentFont] as FontStructure;
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
    textElement.FontStyle = structure.FontStyle;
    textElement.EncodedTextBytes = dictionary;
    textElement.FontName = structure.FontName;
    textElement.Font = structure.CurrentFont;
    textElement.FontSize = this.FontSize;
    textElement.TextScaling = this.m_textScaling;
    textElement.FontEncoding = structure.FontEncoding;
    textElement.FontGlyphWidths = structure.FontGlyphWidths;
    textElement.DefaultGlyphWidth = structure.DefaultGlyphWidth;
    textElement.isNegativeFont = this.isNegativeFont;
    textElement.UnicodeCharMapTable = structure.UnicodeCharMapTable;
    textElement.IsFindText = this.isFindText;
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
    textElement.PathBrush = this.StrokingBrush == null ? new Pen(Color.Black).Brush : (textElement.RenderingMode != 3 ? this.StrokingBrush : new Pen(Color.Transparent).Brush);
    textElement.PathNonStrokeBrush = this.NonStrokingBrush == null ? new Pen(Color.Black).Brush : this.NonStrokingBrush;
    textElement.WordSpacing = this.Objects.WordSpacing;
    textElement.CharacterSpacing = this.Objects.CharacterSpacing;
    if (this.m_beginText)
      this.m_beginText = false;
    Syncfusion.PdfViewer.Base.Matrix txtMatrix = new Syncfusion.PdfViewer.Base.Matrix();
    if (structure.fontType.Value != "Type3")
    {
      if (this.m_isCurrentPositionChanged)
      {
        this.m_isCurrentPositionChanged = false;
        this.m_endTextPosition = this.CurrentLocation;
        if (textElement.structure.IsMappingDone && textElement.structure.FontEncoding == "Identity-H" && textElement.structure.CharacterMapTable.Count > 0)
          textElement.structure.IsMappingDone = false;
        this.m_textElementWidth = textElement.Render(this.m_graphics, new PointF(this.m_endTextPosition.X, this.m_endTextPosition.Y - this.FontSize), (double) this.m_textScaling, fontGlyphWidths, (double) structure.Type1GlyphHeight, structure.differenceTable, structure.DifferencesDictionary, structure.differenceEncoding, out txtMatrix);
        this.decodedStringData.Add(this.GetTextFromGlyph(textElement.textElementGlyphList, 0));
        this.imageRenderGlyphList.AddRange((IEnumerable<Syncfusion.PdfViewer.Base.Glyph>) textElement.textElementGlyphList);
        textElement.textElementGlyphList.Clear();
      }
      else
      {
        this.m_endTextPosition = new PointF(this.m_endTextPosition.X + this.m_textElementWidth, this.m_endTextPosition.Y);
        this.m_textElementWidth = textElement.Render(this.m_graphics, new PointF(this.m_endTextPosition.X, this.m_endTextPosition.Y - this.FontSize), (double) this.m_textScaling, fontGlyphWidths, (double) structure.Type1GlyphHeight, structure.differenceTable, structure.DifferencesDictionary, structure.differenceEncoding, out txtMatrix);
        this.decodedStringData.Add(this.GetTextFromGlyph(textElement.textElementGlyphList, 0));
        this.imageRenderGlyphList.AddRange((IEnumerable<Syncfusion.PdfViewer.Base.Glyph>) textElement.textElementGlyphList);
        textElement.textElementGlyphList.Clear();
      }
    }
    else
    {
      Syncfusion.PdfViewer.Base.Matrix textLineMatrix = this.TextLineMatrix;
      FontStructure fontStructure = structure;
      this.m_graphics.Transform = new System.Drawing.Drawing2D.Matrix((float) this.DocumentMatrix.M11, (float) this.DocumentMatrix.M12, (float) this.DocumentMatrix.M21, (float) this.DocumentMatrix.M22, (float) this.DocumentMatrix.OffsetX, (float) this.DocumentMatrix.OffsetY);
      this.m_graphics.TranslateTransform(0.0f, (float) this.TextLineMatrix.OffsetY);
      this.m_spacingWidth = 0.0f;
      this.RenderType3GlyphImages(structure, str2);
      this.TextLineMatrix = textLineMatrix;
      structure = fontStructure;
      txtMatrix = Syncfusion.PdfViewer.Base.Matrix.Identity;
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
    Syncfusion.PdfViewer.Base.Matrix matrix1 = this.CTRM * this.TextLineMatrix;
    float num = new PdfUnitConvertor().ConvertFromPixels(this.currentPageHeight, PdfGraphicsUnit.Point);
    if (!str3.Contains("www") && !str3.Contains("http") && !this.IsValidEmail(str3) && !str3.Contains(".com"))
      return;
    float emSize = 0.0f;
    Syncfusion.PdfViewer.Base.Matrix matrix2 = new Syncfusion.PdfViewer.Base.Matrix()
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
    SizeF sizeF = this.m_graphics.MeasureString(str3, font);
    double textLeading = (double) this.TextLeading;
    Syncfusion.PdfViewer.Base.Matrix matrix3 = this.CTRM * this.TextLineMatrix;
    if (this.TextLineMatrix.M12 != 0.0 && this.TextLineMatrix.M21 != 0.0)
    {
      matrix3.OffsetX = this.TextLineMatrix.OffsetY;
      matrix3.OffsetY = -this.TextLineMatrix.OffsetX;
    }
    else
    {
      matrix3.OffsetX = matrix2.OffsetX;
      matrix3.OffsetY = matrix2.OffsetY;
      matrix3 *= new Syncfusion.PdfViewer.Base.Matrix(1.0, 0.0, 0.0, 1.0, 0.0, -(double) num);
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
    Syncfusion.PdfViewer.Base.Point point1 = this.Objects.textLineMatrix.Transform(new Syncfusion.PdfViewer.Base.Point(0.0, 0.0));
    Syncfusion.PdfViewer.Base.Point point2 = this.Objects.textLineMatrix.Transform(new Syncfusion.PdfViewer.Base.Point(x, 0.0));
    if (point1.X != point2.X)
      this.Objects.textLineMatrix.OffsetX = point2.X;
    else
      this.Objects.textLineMatrix.OffsetY = point2.Y;
    this.m_spacingWidth += (float) x;
  }

  private string GetTextFromGlyph(List<Syncfusion.PdfViewer.Base.Glyph> glyph, int startIndex)
  {
    string empty = string.Empty;
    for (int index = startIndex; index < glyph.Count; ++index)
      empty += glyph[index].ToUnicode;
    return empty;
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
            pdfStream.Decompress();
            pdfStream.InternalStream.WriteTo((Stream) memoryStream);
            memoryStream.Write(buffer, 0, buffer.Length);
            bool flag = false;
            Syncfusion.PdfViewer.Base.Matrix ctrm = this.CTRM;
            System.Drawing.Drawing2D.Matrix drawing2dMatrixCtm = this.Drawing2dMatrixCTM;
            Syncfusion.PdfViewer.Base.Matrix matrix = this.TextLineMatrix * this.CTRM;
            Stack<GraphicObjectData> objects = this.m_objects;
            if (structure.FontDictionary.ContainsKey("FontMatrix"))
            {
              PdfArray font = structure.FontDictionary["FontMatrix"] as PdfArray;
              this.XFormsMatrix = new Syncfusion.PdfViewer.Base.Matrix((double) (font[0] as PdfNumber).FloatValue, (double) (font[1] as PdfNumber).FloatValue, (double) (font[2] as PdfNumber).FloatValue, (double) (font[3] as PdfNumber).FloatValue, (double) (font[4] as PdfNumber).FloatValue, (double) (font[5] as PdfNumber).FloatValue);
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
              this.m_transformations = new Syncfusion.Pdf.TransformationStack(this.DocumentMatrix);
              this.m_type3FontStruct = structure;
              this.m_isType3Font = true;
              if (flag)
              {
                this.CTRM = new Syncfusion.PdfViewer.Base.Matrix(matrix.M11, matrix.M12, matrix.M21, matrix.M22, matrix.OffsetX, matrix.OffsetY);
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
      if (structure.Type3FontCharProcsDict != null && structure.Type3FontCharProcsDict.ContainsKey(str1))
      {
        this.m_type3GlyphID = str1 + structure.FontRefNumber;
        MemoryStream memoryStream = new MemoryStream();
        PdfStream pdfStream = structure.Type3FontCharProcsDict[str1];
        byte[] buffer = PdfString.StringToByte("\r\n");
        pdfStream.Decompress();
        pdfStream.InternalStream.WriteTo((Stream) memoryStream);
        memoryStream.Write(buffer, 0, buffer.Length);
        bool flag = false;
        Syncfusion.PdfViewer.Base.Matrix ctrm = this.CTRM;
        System.Drawing.Drawing2D.Matrix drawing2dMatrixCtm = this.Drawing2dMatrixCTM;
        Syncfusion.PdfViewer.Base.Matrix matrix = this.TextLineMatrix * this.CTRM;
        Stack<GraphicObjectData> objects = this.m_objects;
        if (structure.FontDictionary.ContainsKey("FontMatrix"))
        {
          PdfArray font = structure.FontDictionary["FontMatrix"] as PdfArray;
          this.XFormsMatrix = new Syncfusion.PdfViewer.Base.Matrix((double) (font[0] as PdfNumber).FloatValue, (double) (font[1] as PdfNumber).FloatValue, (double) (font[2] as PdfNumber).FloatValue, (double) (font[3] as PdfNumber).FloatValue, (double) (font[4] as PdfNumber).FloatValue, (double) (font[5] as PdfNumber).FloatValue);
          matrix = this.XFormsMatrix * matrix;
          this.m_type3FontScallingFactor = (float) matrix.M11;
        }
        if (memoryStream != null)
        {
          this.m_type3RecordCollection = new ContentParser(memoryStream.ToArray()).ReadContent();
          for (int index2 = 0; index2 < this.m_type3RecordCollection.RecordCollection.Count; ++index2)
          {
            string str2 = this.m_type3RecordCollection.RecordCollection[index2].OperatorName;
            if (str2 == "cm")
              this.m_istype3FontContainCTRM = true;
            foreach (char symbolChar in this.m_symbolChars)
            {
              if (str2.Contains(symbolChar.ToString()))
                str2 = str2.Replace(symbolChar.ToString(), "");
            }
            if (str2.Trim() == "BI" || str2.Trim() == "Do")
              flag = true;
          }
          PdfPageResources resources = this.m_resources;
          this.m_resources = structure.Type3FontGlyphImages;
          this.m_transformations = new Syncfusion.Pdf.TransformationStack(this.DocumentMatrix);
          this.m_type3FontStruct = structure;
          this.m_isType3Font = true;
          if (flag)
          {
            this.CTRM = new Syncfusion.PdfViewer.Base.Matrix(matrix.M11, matrix.M12, matrix.M21, matrix.M22, matrix.OffsetX, matrix.OffsetY);
            this.Drawing2dMatrixCTM = new System.Drawing.Drawing2D.Matrix((float) matrix.M11, (float) matrix.M12, (float) matrix.M21, (float) matrix.M22, (float) matrix.OffsetX, (float) matrix.OffsetY);
          }
          this.RenderAsImage();
          this.m_resources = resources;
          if (flag)
            this.TextLineMatrix = this.m_type3TextLineMatrix;
        }
        if (str1 == "space")
          this.TextLineMatrix = this.CalculateType3TextMatrixupdate(this.TextLineMatrix, (float) this.m_d0Matrix.M11 * this.m_type3FontScallingFactor, true);
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
    textElement.FontStyle = structure.FontStyle;
    textElement.Font = structure.CurrentFont;
    textElement.FontName = structure.FontName;
    Font currentFont = structure.CurrentFont;
    textElement.FontSize = this.FontSize;
    textElement.TextScaling = this.m_textScaling;
    textElement.FontEncoding = structure.FontEncoding;
    textElement.FontGlyphWidths = structure.FontGlyphWidths;
    textElement.DefaultGlyphWidth = structure.DefaultGlyphWidth;
    textElement.Text = str1;
    textElement.IsFindText = this.isFindText;
    textElement.IsTransparentText = this.IsTransparentText;
    textElement.UnicodeCharMapTable = structure.UnicodeCharMapTable;
    Dictionary<int, int> fontGlyphWidths = structure.FontGlyphWidths;
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
      if ((double) this.Objects.m_strokingOpacity != 1.0)
      {
        this.m_opacity = this.Objects.m_strokingOpacity;
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
    Syncfusion.PdfViewer.Base.Matrix txtMatrix = new Syncfusion.PdfViewer.Base.Matrix();
    if (structure.fontType.Value != "Type3")
    {
      if (this.m_isCurrentPositionChanged)
      {
        this.m_isCurrentPositionChanged = false;
        this.m_endTextPosition = this.CurrentLocation;
        if (textElement.structure.IsMappingDone && textElement.structure.FontEncoding == "Identity-H" && textElement.structure.CharacterMapTable.Count > 0)
          textElement.structure.IsMappingDone = false;
        this.m_textElementWidth = textElement.Render(this.m_graphics, new PointF(this.m_endTextPosition.X, this.m_endTextPosition.Y + (float) (-(double) this.TextLeading / 4.0)), (double) this.m_textScaling, fontGlyphWidths, (double) structure.Type1GlyphHeight, structure.differenceTable, structure.DifferencesDictionary, structure.differenceEncoding, out txtMatrix);
        this.decodedStringData.Add(this.GetTextFromGlyph(textElement.textElementGlyphList, 0));
        this.imageRenderGlyphList.AddRange((IEnumerable<Syncfusion.PdfViewer.Base.Glyph>) textElement.textElementGlyphList);
        textElement.textElementGlyphList.Clear();
      }
      else
      {
        this.m_endTextPosition = new PointF(this.m_endTextPosition.X + this.m_textElementWidth, this.m_endTextPosition.Y);
        this.m_textElementWidth = textElement.Render(this.m_graphics, new PointF(this.m_endTextPosition.X, this.m_endTextPosition.Y + (float) (-(double) this.TextLeading / 4.0)), (double) this.m_textScaling, fontGlyphWidths, (double) structure.Type1GlyphHeight, structure.differenceTable, structure.DifferencesDictionary, structure.differenceEncoding, out txtMatrix);
        this.decodedStringData.Add(this.GetTextFromGlyph(textElement.textElementGlyphList, 0));
        this.imageRenderGlyphList.AddRange((IEnumerable<Syncfusion.PdfViewer.Base.Glyph>) textElement.textElementGlyphList);
        textElement.textElementGlyphList.Clear();
      }
    }
    else
    {
      Syncfusion.PdfViewer.Base.Matrix textLineMatrix = this.TextLineMatrix;
      FontStructure fontStructure = structure;
      this.m_graphics.Transform = new System.Drawing.Drawing2D.Matrix((float) this.DocumentMatrix.M11, (float) this.DocumentMatrix.M12, (float) this.DocumentMatrix.M21, (float) this.DocumentMatrix.M22, (float) this.DocumentMatrix.OffsetX, (float) this.DocumentMatrix.OffsetY);
      this.m_graphics.TranslateTransform(0.0f, (float) this.TextLineMatrix.OffsetY);
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
    Syncfusion.PdfViewer.Base.Matrix matrix1 = this.CTRM * this.TextLineMatrix;
    float num = new PdfUnitConvertor().ConvertFromPixels(this.currentPageHeight, PdfGraphicsUnit.Point);
    this.DrawNewLine();
    if (!str2.Contains("www") && !str2.Contains("http") && !this.IsValidEmail(str2) && !str2.Contains(".com"))
      return;
    float emSize = (float) this.TextLineMatrix.M11 * this.FontSize;
    Font font = new Font(this.CurrentFont, emSize);
    SizeF sizeF = this.m_graphics.MeasureString(str2, font);
    double textLeading = (double) this.TextLeading;
    Syncfusion.PdfViewer.Base.Matrix matrix2 = this.CTRM * this.TextLineMatrix * new Syncfusion.PdfViewer.Base.Matrix(1.0, 0.0, 0.0, 1.0, 0.0, -(double) num);
    this.currentTransformLocation = new PointF((float) matrix2.OffsetX, (float) -(matrix2.OffsetY + (double) emSize));
    Regex regex = new Regex("\\b(?:http://|https://|www\\.)\\S+\\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    string URI = string.Empty;
    foreach (Capture match in regex.Matches(str2))
      URI = match.Value;
    if (this.IsValidEmail(textElement.renderedText) || str2.Contains("@"))
      URI = "mailto:" + str2;
    this.URLDictonary.Add(new PageURL(this.transformMatrix, URI, new PointF(this.currentTransformLocation.X, this.currentTransformLocation.Y), sizeF.Width, sizeF.Height));
  }

  private void RenderTextElementWithSpacing(string[] textElements, string tokenType)
  {
    List<string> stringList = new List<string>();
    string textToDecode = string.Join("", textElements);
    if (!this.m_resources.ContainsKey(this.CurrentFont))
      return;
    (this.m_resources[this.CurrentFont] as FontStructure).IsSameFont = this.m_resources.isSameFont();
    if ((double) (this.m_resources[this.CurrentFont] as FontStructure).FontSize != (double) this.FontSize)
      (this.m_resources[this.CurrentFont] as FontStructure).FontSize = this.FontSize;
    FontStructure structure = this.m_resources[this.CurrentFont] as FontStructure;
    List<float> characterSpacings = (List<float>) null;
    string decodedString;
    List<string> decodedCollection;
    if (structure.FontName == "ZapfDingbats" && !structure.isEmbedded)
      decodedCollection = this.DecodeTextTJ(structure, textToDecode, this.m_resources.isSameFont(), out decodedString);
    else if (structure.FontEncoding == "Identity-H" || structure.FontName == "MinionPro" && structure.FontEncoding == "Encoding")
    {
      structure.IsMappingDone = false;
      decodedCollection = structure.DecodeTextTJ(textToDecode, this.m_resources.isSameFont());
      string str = "[";
      for (int index = 0; index < decodedCollection.Count; ++index)
      {
        if (decodedCollection[index].Length != 0 && decodedCollection[index][decodedCollection[index].Length - 1] == 's')
        {
          string empty = string.Empty;
          string decodedText = structure.CharacterMapTable == null || structure.CharacterMapTable.Count <= 0 ? (structure.DifferencesDictionary == null || structure.DifferencesDictionary.Count <= 0 ? decodedCollection[index].Substring(0, decodedCollection[index].Length - 1) : structure.MapDifferences(empty)) : structure.MapCharactersFromTable(decodedCollection[index].Substring(0, decodedCollection[index].Length - 1));
          if (structure.CidToGidMap != null)
            decodedText = structure.MapCidToGid(decodedText);
          str = $"{str}({decodedText})";
        }
        else
          str += decodedCollection[index];
      }
      decodedString = str + "]";
    }
    else
    {
      decodedCollection = structure.DecodeTextTJ(textToDecode, this.m_resources.isSameFont());
      decodedString = textToDecode;
    }
    TextElement textElement = new TextElement(decodedString, this.DocumentMatrix);
    textElement.FontStyle = structure.FontStyle;
    textElement.Font = structure.CurrentFont;
    textElement.FontName = structure.FontName;
    textElement.FontSize = this.FontSize;
    textElement.TextScaling = this.m_textScaling;
    textElement.FontEncoding = structure.FontEncoding;
    textElement.FontGlyphWidths = structure.FontGlyphWidths;
    textElement.DefaultGlyphWidth = structure.DefaultGlyphWidth;
    textElement.RenderingMode = this.RenderingMode;
    textElement.IsFindText = this.isFindText;
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
        textElement.PathBrush = new Pen(Color.Transparent).Brush;
      }
      else
      {
        textElement.PathBrush = this.StrokingBrush;
        if (this.isXGraphics)
        {
          Color color = ((SolidBrush) this.StrokingBrush).Color;
          float num = Math.Max(0.0f, Math.Min(1f, this.Objects.m_strokingOpacity));
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
    Syncfusion.PdfViewer.Base.Matrix textmatrix = new Syncfusion.PdfViewer.Base.Matrix();
    if (structure.fontType.Value != "Type3")
    {
      if (this.m_isCurrentPositionChanged)
      {
        this.m_isCurrentPositionChanged = false;
        this.m_endTextPosition = this.CurrentLocation;
        this.m_textElementWidth = 0.0f;
        int startIndex = 0;
        string fontName = textElement.FontName;
        textElement.m_isRectation = true;
        foreach (string str in decodedCollection)
        {
          List<string> decodedList = new List<string>();
          decodedList.Add(str);
          if (fontName == "Zapdingbats" && !this.fontStructure.isEmbedded)
            textElement.FontName = fontName;
          this.m_textElementWidth += textElement.RenderWithSpace(this.m_graphics, new PointF(this.m_endTextPosition.X, this.m_endTextPosition.Y - this.FontSize), decodedList, characterSpacings, (double) this.m_textScaling, fontGlyphWidths, (double) structure.Type1GlyphHeight, structure.differenceTable, structure.DifferencesDictionary, structure.differenceEncoding, out textmatrix);
          this.decodedStringData.Add(this.GetTextFromGlyph(textElement.textElementGlyphList, startIndex));
          startIndex = textElement.textElementGlyphList.Count;
        }
        this.imageRenderGlyphList.AddRange((IEnumerable<Syncfusion.PdfViewer.Base.Glyph>) textElement.textElementGlyphList);
        textElement.textElementGlyphList.Clear();
        textElement.m_isRectation = false;
      }
      else
      {
        this.m_endTextPosition = new PointF(this.m_endTextPosition.X + this.m_textElementWidth, this.m_endTextPosition.Y);
        this.m_textElementWidth = 0.0f;
        int startIndex = 0;
        textElement.m_isRectation = true;
        foreach (string str in decodedCollection)
        {
          this.m_textElementWidth = textElement.RenderWithSpace(this.m_graphics, new PointF(this.m_endTextPosition.X, this.m_endTextPosition.Y - this.FontSize), new List<string>()
          {
            str
          }, characterSpacings, (double) this.m_textScaling, fontGlyphWidths, (double) structure.Type1GlyphHeight, structure.differenceTable, structure.DifferencesDictionary, structure.differenceEncoding, out textmatrix);
          this.decodedStringData.Add(this.GetTextFromGlyph(textElement.textElementGlyphList, startIndex));
          startIndex = textElement.textElementGlyphList.Count;
        }
        this.imageRenderGlyphList.AddRange((IEnumerable<Syncfusion.PdfViewer.Base.Glyph>) textElement.textElementGlyphList);
        textElement.textElementGlyphList.Clear();
        textElement.m_isRectation = false;
      }
    }
    else
    {
      Syncfusion.PdfViewer.Base.Matrix textLineMatrix = this.TextLineMatrix;
      FontStructure fontStructure = structure;
      this.m_graphics.Transform = new System.Drawing.Drawing2D.Matrix((float) this.DocumentMatrix.M11, (float) this.DocumentMatrix.M12, (float) this.DocumentMatrix.M21, (float) this.DocumentMatrix.M22, (float) this.DocumentMatrix.OffsetX, (float) this.DocumentMatrix.OffsetY);
      this.m_graphics.TranslateTransform(0.0f, (float) this.TextLineMatrix.OffsetY);
      this.m_spacingWidth = 0.0f;
      this.RenderType3GlyphImagesTJ(structure, decodedCollection);
      this.m_textElementWidth = 0.0f;
      int startIndex = 0;
      textElement.m_isRectation = true;
      foreach (string str in decodedCollection)
      {
        this.m_textElementWidth = textElement.RenderWithSpace(this.m_graphics, new PointF(this.m_endTextPosition.X, this.m_endTextPosition.Y - this.FontSize), new List<string>()
        {
          str
        }, characterSpacings, (double) this.m_textScaling, fontGlyphWidths, (double) structure.Type1GlyphHeight, structure.differenceTable, structure.DifferencesDictionary, structure.differenceEncoding, out textmatrix);
        this.decodedStringData.Add(this.GetTextFromGlyph(textElement.textElementGlyphList, startIndex));
        startIndex = textElement.textElementGlyphList.Count;
      }
      this.imageRenderGlyphList.AddRange((IEnumerable<Syncfusion.PdfViewer.Base.Glyph>) textElement.textElementGlyphList);
      textElement.textElementGlyphList.Clear();
      this.TextLineMatrix = textLineMatrix;
      structure = fontStructure;
      textElement.m_isRectation = false;
    }
    this.TextMatrix = textmatrix;
    string str1 = textElement.renderedText;
    if (!structure.IsMappingDone)
    {
      if (structure.CharacterMapTable != null && structure.CharacterMapTable.Count > 0)
        str1 = structure.MapCharactersFromTable(str1);
      else if (structure.DifferencesDictionary != null && structure.DifferencesDictionary.Count > 0)
        str1 = structure.MapDifferences(str1);
    }
    Syncfusion.PdfViewer.Base.Matrix matrix1 = this.CTRM * this.TextLineMatrix;
    float num1 = new PdfUnitConvertor().ConvertFromPixels(this.currentPageHeight, PdfGraphicsUnit.Point);
    if (!str1.Contains("www") && !str1.Contains("http") && !this.IsValidEmail(str1) && !str1.Contains(".com"))
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
    SizeF sizeF = this.m_graphics.MeasureString(str1, font);
    double textLeading = (double) this.TextLeading;
    Syncfusion.PdfViewer.Base.Matrix matrix2 = this.CTRM * this.TextLineMatrix;
    if (this.TextLineMatrix.M12 != 0.0 && this.TextLineMatrix.M21 != 0.0)
    {
      matrix2.OffsetX = this.TextLineMatrix.OffsetY;
      matrix2.OffsetY = -this.TextLineMatrix.OffsetX;
    }
    else
    {
      matrix2.OffsetX = this.TextLineMatrix.OffsetX;
      matrix2.OffsetY = this.TextLineMatrix.OffsetY;
      matrix2 *= new Syncfusion.PdfViewer.Base.Matrix(1.0, 0.0, 0.0, 1.0, 0.0, -(double) num1);
    }
    this.currentTransformLocation = new PointF((float) matrix2.OffsetX, (float) -(matrix2.OffsetY + (double) emSize));
    Regex regex = new Regex("\\b(?:http://|https://|www\\.)\\S+\\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    string URI = string.Empty;
    foreach (Capture match in regex.Matches(str1))
      URI = match.Value;
    if (this.IsValidEmail(textElement.renderedText) || str1.Contains("@"))
      URI = "mailto:" + str1;
    this.URLDictonary.Add((double) this.m_endTextPosition.Y + (-(double) this.TextLeading - (double) this.FontSize) == 0.0 ? new PageURL(this.transformMatrix, URI, new PointF(this.m_endTextPosition.X, this.m_endTextPosition.Y), this.m_textElementWidth, this.FontSize) : new PageURL(this.transformMatrix, URI, new PointF(this.currentTransformLocation.X, this.currentTransformLocation.Y), sizeF.Width, sizeF.Height));
  }

  private bool IsValidEmail(string email)
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
      num1 = float.Parse(colorElement[0]);
      num2 = float.Parse(colorElement[1]);
      num3 = float.Parse(colorElement[2]);
      num4 = this.m_opacity;
    }
    else if (colorSpace == "Gray" && colorElement.Length == 1)
    {
      double num5;
      num3 = (float) (num5 = !colorElement[0].Contains("/") ? (!this.isBlack ? (double) float.Parse(colorElement[0]) : 0.0) : 0.0);
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
    float num1 = (float) ((double) byte.MaxValue * (1.0 - (double) c) * (1.0 - (double) k));
    float num2 = (float) ((double) byte.MaxValue * (1.0 - (double) m) * (1.0 - (double) k));
    float num3 = (float) ((double) byte.MaxValue * (1.0 - (double) y) * (1.0 - (double) k));
    return Color.FromArgb((int) byte.MaxValue, (double) num1 > (double) byte.MaxValue ? (int) byte.MaxValue : ((double) num1 < 0.0 ? 0 : (int) num1), (double) num2 > (double) byte.MaxValue ? (int) byte.MaxValue : ((double) num2 < 0.0 ? 0 : (int) num2), (double) num3 > (double) byte.MaxValue ? (int) byte.MaxValue : ((double) num3 < 0.0 ? 0 : (int) num3));
  }

  private void DrawNewLine()
  {
    this.m_isCurrentPositionChanged = true;
    FontStructure resource = this.m_resources[this.CurrentFont] as FontStructure;
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
    this.Objects.WordSpacing = float.Parse(spacing[0]);
  }

  private void GetCharacterSpacing(string[] spacing)
  {
    this.Objects.CharacterSpacing = float.Parse(spacing[0]);
    this.m_characterSpacing = this.Objects.CharacterSpacing;
  }

  private void GetScalingFactor(string[] scaling)
  {
    this.m_textScaling = float.Parse(scaling[0]);
    this.HorizontalScaling = float.Parse(scaling[0]);
  }

  private void writePoint(PdfStream stream, PointF point)
  {
    stream.Write(PdfNumber.FloatToString(point.X));
    stream.Write(" ");
    stream.Write(PdfNumber.FloatToString(point.Y));
    stream.Write(" ");
  }

  private Syncfusion.PdfViewer.Base.Matrix CalculateTextMatrixupdate(
    Syncfusion.PdfViewer.Base.Matrix m,
    float Width,
    bool isImage)
  {
    return new Syncfusion.PdfViewer.Base.Matrix(1.0, 0.0, 0.0, 1.0, !isImage ? (double) Width * (double) this.FontSize * 0.001 : (double) Width * 1.0, 0.0) * m;
  }

  private System.Drawing.Drawing2D.Matrix GetTransformationMatrix(Syncfusion.PdfViewer.Base.Matrix transform)
  {
    return new System.Drawing.Drawing2D.Matrix((float) transform.M11, (float) transform.M12, (float) transform.M21, (float) transform.M22, (float) transform.OffsetX, (float) transform.OffsetY);
  }

  private void BeginPath(float x, float y)
  {
    this.CurrentLocation = new PointF(x, y);
    if (this.m_currentPath != null && this.m_currentPath.Segments.Count == 0)
      this.CurrentGeometry.Figures.Remove(this.CurrentPath);
    this.m_currentPath = new PathFigure();
    this.m_currentPath.StartPoint = (Syncfusion.PdfViewer.Base.Point) this.CurrentLocation;
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
      Point = (Syncfusion.PdfViewer.Base.Point) this.CurrentLocation
    });
  }

  private GraphicsPath GetGeometry(PathGeometry geometry, Syncfusion.PdfViewer.Base.Matrix transform)
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
      if (this.m_selectablePrintDocument)
      {
        (this.m_resources[xobjectElement[0].Replace("/", "")] as XObjectElement).m_isPrintSelected = this.m_selectablePrintDocument;
        (this.m_resources[xobjectElement[0].Replace("/", "")] as XObjectElement).m_pageHeight = (double) this.m_pageHeight;
      }
      List<Syncfusion.PdfViewer.Base.Glyph> glyphList;
      this.m_graphicsState = (this.m_resources[xobjectElement[0].Replace("/", "")] as XObjectElement).Render(this.m_graphics, this.m_resources, this.m_graphicsState, this.m_objects, out glyphList, this.zoomFactor, this.isExportAsImage, this.substitutedFontsList);
      this.imageRenderGlyphList.AddRange((IEnumerable<Syncfusion.PdfViewer.Base.Glyph>) glyphList);
      glyphList.Clear();
    }
    else
    {
      if (!(this.m_resources[xobjectElement[0].Replace("/", "")] is ImageStructure))
        return;
      ImageStructure resource = this.m_resources[xobjectElement[0].Replace("/", "")] as ImageStructure;
      resource.m_isExtGStateContainsSMask = this.m_isExtendedGraphicStateContainsSMask;
      Image image = resource.EmbeddedImage;
      System.Drawing.Drawing2D.Matrix transform = this.m_graphics.Transform;
      GraphicsUnit pageUnit = this.m_graphics.PageUnit;
      this.m_graphics.PageUnit = GraphicsUnit.Pixel;
      this.m_graphics.Transform = new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
      Syncfusion.PdfViewer.Base.Matrix matrix1 = new Syncfusion.PdfViewer.Base.Matrix((double) this.Drawing2dMatrixCTM.Elements[0], (double) this.Drawing2dMatrixCTM.Elements[1], (double) this.Drawing2dMatrixCTM.Elements[2], (double) this.Drawing2dMatrixCTM.Elements[3], (double) this.Drawing2dMatrixCTM.OffsetX, (double) this.Drawing2dMatrixCTM.OffsetY);
      matrix1.Scale(1.0, -1.0, 0.0, 1.0);
      Syncfusion.PdfViewer.Base.Matrix matrix2 = matrix1 * this.DocumentMatrix;
      this.m_graphics.Transform = new System.Drawing.Drawing2D.Matrix((float) matrix2.M11, (float) matrix2.M12, (float) matrix2.M21, (float) matrix2.M22, (float) matrix2.OffsetX, (float) Math.Round(matrix2.OffsetY, 5));
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
        InterpolationMode interpolationMode = this.m_graphics.InterpolationMode;
        PixelOffsetMode pixelOffsetMode = this.m_graphics.PixelOffsetMode;
        this.m_graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        this.m_graphics.PixelOffsetMode = PixelOffsetMode.None;
        using (ImageAttributes imageAttr = new ImageAttributes())
        {
          if (resource.ImageDictionary != null && resource.ImageDictionary.ContainsKey("Intent") && (resource.ImageDictionary["Intent"] as PdfName).Value == "RelativeColorimetric")
          {
            ColorMatrix newColorMatrix = new ColorMatrix();
            newColorMatrix.Matrix33 = this.Objects.m_strokingOpacity;
            ImageAttributes imageAttributes = new ImageAttributes();
            imageAttr.SetColorMatrix(newColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
          }
          imageAttr.SetWrapMode(WrapMode.TileFlipXY);
          this.m_graphics.DrawImage(image, new Rectangle(0, 0, 1, 1), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttr);
        }
        this.m_graphics.PixelOffsetMode = pixelOffsetMode;
        this.m_graphics.InterpolationMode = interpolationMode;
      }
      this.m_graphics.Transform = transform;
      this.m_graphics.PageUnit = pageUnit;
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
    textElement.Font = type3FontStruct.CurrentFont;
    textElement.type3GlyphImage = image;
    textElement.FontSize = this.FontSize;
    textElement.TextScaling = this.m_textScaling;
    textElement.isNegativeFont = this.isNegativeFont;
    textElement.UnicodeCharMapTable = type3FontStruct.UnicodeCharMapTable;
    Dictionary<int, int> fontGlyphWidths = type3FontStruct.FontGlyphWidths;
    textElement.structure = type3FontStruct;
    textElement.Ctm = this.CTRM;
    textElement.textLineMatrix = new Syncfusion.PdfViewer.Base.Matrix(1.0, 0.0, 0.0, 1.0, 0.0, 0.0);
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
    Syncfusion.PdfViewer.Base.Matrix txtMatrix = new Syncfusion.PdfViewer.Base.Matrix();
    double num = (double) textElement.Render(this.m_graphics, new PointF(this.m_endTextPosition.X, this.m_endTextPosition.Y - this.FontSize), (double) this.m_textScaling, fontGlyphWidths, (double) type3FontStruct.Type1GlyphHeight, type3FontStruct.differenceTable, type3FontStruct.DifferencesDictionary, type3FontStruct.differenceEncoding, out txtMatrix);
    this.TextLineMatrix = this.CalculateTextMatrixupdate(this.TextLineMatrix, (float) this.m_d1Matrix.M11, true);
    this.m_type3TextLineMatrix = this.TextLineMatrix;
  }

  private Syncfusion.PdfViewer.Base.Matrix CalculateType3TextMatrixupdate(
    Syncfusion.PdfViewer.Base.Matrix m,
    float Width,
    bool isImage)
  {
    return new Syncfusion.PdfViewer.Base.Matrix(1.0, 0.0, 0.0, 1.0, ((double) Width * (double) this.FontSize + (double) this.Objects.CharacterSpacing + (double) this.m_type3WhiteSpaceWidth) * ((double) this.HorizontalScaling / 100.0), 0.0) * m;
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
    MemoryStream memoryStream = new MemoryStream();
    bitmap.Save((Stream) memoryStream, ImageFormat.Png);
    Image maskImagefromStream = Image.FromStream((Stream) memoryStream);
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
    this.DrawType3FontAsShape(this.m_graphics, outlines, width, height);
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
      Syncfusion.PdfViewer.Base.Glyph glyph = new Syncfusion.PdfViewer.Base.Glyph();
      glyph.FontSize = (double) this.FontSize;
      glyph.Stroke = this.StrokingBrush == null ? new Pen(Color.Black).Brush : this.StrokingBrush;
      glyph.TransformMatrix = this.GetTextRenderingMatrix();
      glyph.HorizontalScaling = (double) this.TextScaling;
      glyph.CharSpacing = (double) this.Objects.CharacterSpacing;
      Syncfusion.PdfViewer.Base.Matrix identity = Syncfusion.PdfViewer.Base.Matrix.Identity;
      this.m_transformations = new Syncfusion.Pdf.TransformationStack(this.DocumentMatrix);
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
      graphicsPathList.Add(this.GetGeometry(this.CurrentGeometry, new Syncfusion.PdfViewer.Base.Matrix(1.0, 0.0, 0.0, 1.0, 0.0, 0.0)));
      this.CurrentGeometry = new PathGeometry();
    }
    GraphicsPath path = new GraphicsPath();
    for (int index3 = 0; index3 < graphicsPathList.Count; ++index3)
      path.AddPath(graphicsPathList[index3], false);
    graphics.FillPath(pen.Brush, path);
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
}
