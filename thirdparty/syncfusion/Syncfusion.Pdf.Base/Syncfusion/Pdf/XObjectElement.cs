// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.XObjectElement
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using Syncfusion.PdfViewer.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

#nullable disable
namespace Syncfusion.Pdf;

internal class XObjectElement
{
  private string m_objectName;
  private string m_objectType;
  private PdfMatrix m_imageInfo;
  private PdfDictionary m_xObjectDictionary;
  private bool m_isExtractTextData;
  private bool m_isExtractLineCollection;
  internal bool IsPdfium;
  internal List<TextElement> m_extractTextElement = new List<TextElement>();
  internal bool m_isPrintSelected;
  internal double m_pageHeight;
  private string previousObjectName = string.Empty;

  internal bool IsExtractTextData
  {
    get => this.m_isExtractTextData;
    set => this.m_isExtractTextData = value;
  }

  internal bool IsExtractLineCollection
  {
    get => this.m_isExtractLineCollection;
    set => this.m_isExtractLineCollection = value;
  }

  internal List<TextElement> ExtractTextElements
  {
    get => this.m_extractTextElement;
    set => this.m_extractTextElement = value;
  }

  internal string ObjectName
  {
    get => this.m_objectName;
    set => this.m_objectName = value;
  }

  internal PdfMatrix ImageInfo
  {
    get => this.m_imageInfo;
    set => this.m_imageInfo = value;
  }

  internal PdfDictionary XObjectDictionary
  {
    get => this.m_xObjectDictionary;
    set => this.m_xObjectDictionary = value;
  }

  internal string ObjectType
  {
    get => this.m_objectType;
    set => this.m_objectType = value;
  }

  public XObjectElement(PdfDictionary xobjectDictionary, string name)
  {
    this.m_xObjectDictionary = xobjectDictionary;
    this.m_objectName = name;
    this.GetObjectType();
  }

  public XObjectElement(PdfDictionary xobjectDictionary, string name, PdfMatrix tm)
  {
    this.m_xObjectDictionary = xobjectDictionary;
    this.m_objectName = name;
    this.ImageInfo = tm;
    this.GetObjectType();
  }

  public PdfRecordCollection Render(PdfPageResources resources, Stack<GraphicsState> graphicsStates)
  {
    if (!(this.ObjectType == "Form"))
      return (PdfRecordCollection) null;
    PdfStream objectDictionary = this.m_xObjectDictionary as PdfStream;
    objectDictionary.Decompress();
    return new ContentParser(objectDictionary.InternalStream.ToArray()).ReadContent();
  }

  public Stack<GraphicsState> Render(
    Graphics g,
    PdfPageResources resources,
    Stack<GraphicsState> graphicsStates,
    Stack<GraphicObjectData> m_objects,
    out List<Glyph> glyphList,
    float zoomFactor,
    bool isExportAsImage,
    Dictionary<string, string> substitutedFontsList)
  {
    glyphList = new List<Glyph>();
    if (this.ObjectType == "Form")
    {
      PdfStream objectDictionary = this.m_xObjectDictionary as PdfStream;
      objectDictionary.Decompress();
      PdfRecordCollection contentElements = new ContentParser(objectDictionary.InternalStream.ToArray()).ReadContent();
      PageResourceLoader pageResourceLoader = new PageResourceLoader();
      PdfDictionary pdfDictionary = new PdfDictionary();
      PdfDictionary xobjectDictionary = this.XObjectDictionary;
      PdfPageResources pdfPageResources = new PdfPageResources();
      if (xobjectDictionary.ContainsKey("Resources"))
      {
        pdfDictionary = (object) (xobjectDictionary["Resources"] as PdfReference) == null ? ((object) (xobjectDictionary["Resources"] as PdfReferenceHolder) == null ? xobjectDictionary["Resources"] as PdfDictionary : (xobjectDictionary["Resources"] as PdfReferenceHolder).Object as PdfDictionary) : (xobjectDictionary["Resources"] as PdfReferenceHolder).Object as PdfDictionary;
        Dictionary<string, PdfMatrix> commonMatrix = new Dictionary<string, PdfMatrix>();
        PdfPageResources pageResources1 = pageResourceLoader.UpdatePageResources(pdfPageResources, pageResourceLoader.GetImageResources(pdfDictionary, (PdfPageBase) null, ref commonMatrix));
        PdfPageResources pageResources2 = pageResourceLoader.UpdatePageResources(pageResources1, pageResourceLoader.GetFontResources(pdfDictionary));
        PdfPageResources pageResources3 = pageResourceLoader.UpdatePageResources(pageResources2, pageResourceLoader.GetExtendedGraphicResources(pdfDictionary));
        PdfPageResources pageResources4 = pageResourceLoader.UpdatePageResources(pageResources3, pageResourceLoader.GetColorSpaceResource(pdfDictionary));
        PdfPageResources pageResources5 = pageResourceLoader.UpdatePageResources(pageResources4, pageResourceLoader.GetShadingResource(pdfDictionary));
        pdfPageResources = pageResourceLoader.UpdatePageResources(pageResources5, pageResourceLoader.GetPatternResource(pdfDictionary));
      }
      else if (resources.Resources.Count > 0)
        pdfPageResources = resources;
      Syncfusion.PdfViewer.Base.Matrix matrix1 = new Syncfusion.PdfViewer.Base.Matrix();
      Syncfusion.PdfViewer.Base.Matrix matrix2 = Syncfusion.PdfViewer.Base.Matrix.Identity;
      if (xobjectDictionary.ContainsKey("Matrix"))
      {
        PdfArray pdfArray1 = new PdfArray();
        if (xobjectDictionary["Matrix"] is PdfArray && xobjectDictionary["Matrix"] is PdfArray pdfArray2)
        {
          float floatValue1 = (pdfArray2[0] as PdfNumber).FloatValue;
          float floatValue2 = (pdfArray2[1] as PdfNumber).FloatValue;
          float floatValue3 = (pdfArray2[2] as PdfNumber).FloatValue;
          float floatValue4 = (pdfArray2[3] as PdfNumber).FloatValue;
          float floatValue5 = (pdfArray2[4] as PdfNumber).FloatValue;
          float floatValue6 = (pdfArray2[5] as PdfNumber).FloatValue;
          matrix2 = new Syncfusion.PdfViewer.Base.Matrix((double) floatValue1, (double) floatValue2, (double) floatValue3, (double) floatValue4, (double) floatValue5, (double) floatValue6);
          if ((double) floatValue5 != 0.0 || (double) floatValue6 != 0.0)
            g.TranslateTransform(floatValue5, -floatValue6);
          if ((double) floatValue1 != 0.0 || (double) floatValue4 != 0.0)
            g.ScaleTransform(floatValue1, floatValue4);
          double d1 = Math.Round(180.0 / Math.PI * Math.Acos((double) floatValue1));
          double d2 = Math.Round(180.0 / Math.PI * Math.Asin((double) floatValue2));
          if (d1 == d2)
            g.RotateTransform(-(float) d1);
          else if (!double.IsNaN(d2))
            g.RotateTransform(-(float) d2);
          else if (!double.IsNaN(d1))
            g.RotateTransform(-(float) d1);
        }
      }
      if (pdfDictionary != null)
      {
        DeviceCMYK cmyk = new DeviceCMYK();
        ImageRenderer renderer = new ImageRenderer(contentElements, pdfPageResources, g, false, cmyk, g.VisibleClipBounds.Height);
        renderer.IsExtractTextData = this.m_isExtractTextData;
        renderer.m_objects = m_objects;
        this.GraphicsSave(g, renderer);
        Syncfusion.PdfViewer.Base.Matrix ctm = m_objects.ToArray()[0].Ctm;
        Syncfusion.PdfViewer.Base.Matrix matrix3 = matrix2 * ctm;
        m_objects.ToArray()[0].drawing2dMatrixCTM = new System.Drawing.Drawing2D.Matrix((float) matrix3.M11, (float) matrix3.M12, (float) matrix3.M21, (float) matrix3.M22, (float) matrix3.OffsetX, (float) matrix3.OffsetY);
        m_objects.ToArray()[0].Ctm = matrix3;
        if (this.previousObjectName != this.ObjectName && xobjectDictionary.ContainsKey("BBox"))
        {
          PdfArray pdfArray3 = new PdfArray();
          if (xobjectDictionary["BBox"] is PdfArray)
          {
            PdfArray pdfArray4 = xobjectDictionary["BBox"] as PdfArray;
            float floatValue7 = (pdfArray4[0] as PdfNumber).FloatValue;
            float floatValue8 = (pdfArray4[1] as PdfNumber).FloatValue;
            float width = (pdfArray4[2] as PdfNumber).FloatValue - floatValue7;
            float height = (pdfArray4[3] as PdfNumber).FloatValue - floatValue8;
            RectangleF rect = new RectangleF(floatValue7, floatValue8, width, height);
            Syncfusion.PdfViewer.Base.Matrix documentMatrix = m_objects.ToArray()[0].documentMatrix;
            Syncfusion.PdfViewer.Base.Matrix matrix4 = matrix2 * ctm * documentMatrix;
            System.Drawing.Drawing2D.Matrix transform = g.Transform;
            int pageUnit = (int) g.PageUnit;
            g.PageUnit = GraphicsUnit.Pixel;
            g.Transform = new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
            g.Transform = new System.Drawing.Drawing2D.Matrix((float) matrix4.M11, (float) matrix4.M12, (float) matrix4.M21, (float) matrix4.M22, (float) matrix4.OffsetX, (float) matrix4.OffsetY);
            g.SetClip(rect, CombineMode.Intersect);
            this.previousObjectName = this.ObjectName;
          }
        }
        renderer.IsPdfium = this.IsPdfium;
        renderer.m_selectablePrintDocument = this.m_isPrintSelected;
        renderer.m_pageHeight = (float) this.m_pageHeight;
        renderer.isXGraphics = true;
        renderer.substitutedFontsList = substitutedFontsList;
        renderer.zoomFactor = zoomFactor;
        renderer.isExtractLineCollection = this.m_isExtractLineCollection;
        renderer.RenderAsImage();
        renderer.isExtractLineCollection = false;
        this.m_extractTextElement = renderer.extractTextElement;
        this.GraphicsRestore(g, renderer);
        renderer.isXGraphics = false;
        for (; renderer.xobjectGraphicsCount > 0; --renderer.xobjectGraphicsCount)
          m_objects.Pop();
        glyphList = renderer.imageRenderGlyphList;
        m_objects.ToArray()[0].Ctm = ctm;
      }
    }
    return graphicsStates;
  }

  private void GraphicsSave(Graphics g, ImageRenderer renderer)
  {
    GraphicObjectData graphicObjectData1 = new GraphicObjectData();
    if (renderer.m_objects.Count > 0)
    {
      GraphicObjectData graphicObjectData2 = renderer.m_objects.ToArray()[0];
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
    renderer.m_objects.Push(graphicObjectData1);
    GraphicsState graphicsState = g.Save();
    renderer.m_graphicsState.Push(graphicsState);
  }

  private void GraphicsRestore(Graphics g, ImageRenderer renderer)
  {
    renderer.m_objects.Pop();
    if (renderer.m_graphicsState.Count > 0)
      g.Restore(renderer.m_graphicsState.Pop());
    renderer.m_graphicspathtoclip = (GraphicsPath) null;
    renderer.IsGraphicsState = false;
  }

  private void GetObjectType()
  {
    if (this.m_xObjectDictionary == null || !this.m_xObjectDictionary.ContainsKey("Subtype"))
      return;
    this.m_objectType = (this.m_xObjectDictionary["Subtype"] as PdfName).Value;
  }
}
