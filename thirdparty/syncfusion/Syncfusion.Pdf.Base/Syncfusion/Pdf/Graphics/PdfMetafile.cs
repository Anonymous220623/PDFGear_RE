// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfMetafile
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics.Images.Metafiles;
using Syncfusion.Pdf.HtmlToPdf;
using Syncfusion.Pdf.Native;
using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public class PdfMetafile : PdfImage, IPdfWrapper, IDisposable
{
  private Metafile m_image;
  private PdfTemplate m_template;
  private bool m_bSaved;
  private TextRegionManager m_textRegions;
  private ImageRegionManager m_imageRegions;
  private bool m_bDisposed;
  private Metafile m_originalImage;
  private long m_quality = 100;
  private int m_imageResolution;
  private ArrayList m_htmlHyperlinks = new ArrayList();
  private ArrayList m_documentLinks = new ArrayList();
  private ArrayList m_inputElements = new ArrayList();
  private ArrayList m_selectElements = new ArrayList();
  private ArrayList m_buttonElements = new ArrayList();
  private float m_pageScale = 1f;
  private GraphicsUnit m_pageUnit = GraphicsUnit.Display;
  private float m_alphaPen = 1f;
  private float m_alphaBrush = 1f;
  private bool m_bIsTransparency;
  private PdfBlendMode m_blendMode;
  private bool m_isImagePath;
  internal PdfDocument m_document;
  internal bool m_optimizeIdenticalImages = true;
  private bool m_isembedFonts;
  private bool m_isembedCompleteEmbedFonts;
  internal RectangleF shapeBounds = RectangleF.Empty;
  internal bool m_isPdfGrid;
  internal float m_bottomCellpadding;
  private bool m_isDirectImageRendering;
  internal bool m_isHtmlToTaggedPdf;
  private PdfGraphics m_currentGraphics;
  internal PointF m_innerImageStartingPostion;
  private CustomFont m_customFont;
  internal bool m_isinnerEMF;
  private bool m_complexScript;
  private bool m_adjustMetaFile;
  internal bool m_skipScaling;
  private bool m_wordToPDFRendering;

  internal bool ComplexScript
  {
    get => this.m_complexScript;
    set => this.m_complexScript = value;
  }

  internal CustomFont CustomFont
  {
    get => this.m_customFont;
    set
    {
      if (value == null)
        return;
      this.m_customFont = value;
    }
  }

  internal bool IsImagePath
  {
    get => this.m_isImagePath;
    set => this.m_isImagePath = value;
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

  internal override Image InternalImage => (Image) this.m_image;

  internal TextRegionManager TextRegions => this.m_textRegions;

  internal ImageRegionManager ImageRegions => this.m_imageRegions;

  internal PdfTemplate Template => this.m_template;

  internal PdfDocument Document
  {
    get => this.m_document;
    set => this.m_document = value;
  }

  internal bool OptimizeIdenticalImages
  {
    get => this.m_optimizeIdenticalImages;
    set => this.m_optimizeIdenticalImages = value;
  }

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

  public long Quality
  {
    get => this.m_quality;
    set => this.m_quality = value;
  }

  public int ImageResolution
  {
    get => this.m_imageResolution;
    set => this.m_imageResolution = value;
  }

  internal ArrayList HtmlHyperlinksCollection
  {
    get => this.m_htmlHyperlinks;
    set => this.m_htmlHyperlinks = value;
  }

  internal ArrayList DocumentLinksCollection
  {
    get => this.m_documentLinks;
    set => this.m_documentLinks = value;
  }

  internal ArrayList InputElementCollection
  {
    get => this.m_inputElements;
    set => this.m_inputElements = value;
  }

  internal ArrayList SelectElementCollection
  {
    get => this.m_selectElements;
    set => this.m_selectElements = value;
  }

  internal ArrayList ButtonElementCollection
  {
    get => this.m_buttonElements;
    set => this.m_buttonElements = value;
  }

  public float PageScale
  {
    get => this.m_pageScale;
    set => this.m_pageScale = value;
  }

  public GraphicsUnit PageUnit
  {
    get => this.m_pageUnit;
    set => this.m_pageUnit = value;
  }

  internal bool IsDirectImageRendering
  {
    get => this.m_isDirectImageRendering;
    set => this.m_isDirectImageRendering = value;
  }

  internal PdfGraphics CurrentPageGraphics
  {
    get => this.m_currentGraphics;
    set => this.m_currentGraphics = value;
  }

  internal bool AdjustMetaFile
  {
    get => this.m_adjustMetaFile;
    set => this.m_adjustMetaFile = value;
  }

  internal bool IsWordToPDF
  {
    get => this.m_wordToPDFRendering;
    set => this.m_wordToPDFRendering = value;
  }

  public PdfMetafile(Metafile metafile)
  {
    this.m_image = metafile != null ? PdfMetafile.AdjustMetafile(metafile) : throw new ArgumentNullException(nameof (metafile));
    this.m_originalImage = metafile;
    if ((double) PdfUnitConvertor.HorizontalResolution == 144.0 && (double) PdfUnitConvertor.VerticalResolution == 144.0)
      this.SetResolution(PdfUnitConvertor.HorizontalResolution, PdfUnitConvertor.VerticalResolution);
    else
      this.SetResolution(96f, 96f);
    this.m_template = new PdfTemplate((float) this.m_image.Width, (float) this.m_image.Height);
    this.SetContent(((IPdfWrapper) this.m_template).Element);
  }

  public PdfMetafile(string path)
    : this(new Metafile(Utils.CheckFilePath(path)))
  {
  }

  public PdfMetafile(System.IO.Stream stream)
    : this(Image.FromStream(PdfImage.CheckStreamExistance(stream)) as Metafile)
  {
  }

  ~PdfMetafile() => this.Dispose(false);

  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }

  private void Dispose(bool disposing)
  {
    if (this.m_bDisposed)
      return;
    if (disposing)
    {
      if (this.m_originalImage != null)
      {
        this.m_originalImage.Dispose();
        this.m_originalImage = (Metafile) null;
      }
      if (this.m_image != null)
      {
        this.m_image.Dispose();
        this.m_image = (Metafile) null;
      }
    }
    else if (this.m_originalImage != null && this.m_image != null && this.m_image != this.m_originalImage)
    {
      this.m_image.Dispose();
      this.m_image = (Metafile) null;
    }
    this.m_bDisposed = true;
  }

  internal override void Save()
  {
    if (this.IsWordToPDF)
      this.SetResolution(PdfUnitConvertor.HorizontalResolution, PdfUnitConvertor.VerticalResolution);
    if (this.m_bSaved && this.m_currentGraphics == null)
      return;
    PdfEmfRenderer renderer = this.ImageResolution <= 0 ? (this.m_currentGraphics != null ? new PdfEmfRenderer(this.m_currentGraphics, this.m_quality, this.EmbedFontResource) : new PdfEmfRenderer(this.m_template.Graphics, this.m_quality, this.EmbedFontResource)) : new PdfEmfRenderer(this.m_template.Graphics, this.m_imageResolution, this.EmbedFontResource);
    renderer.CustomFontCollection = this.CustomFont;
    renderer.ComplexScript = this.ComplexScript;
    renderer.m_innerImageStartingPosition = this.m_innerImageStartingPostion;
    renderer.isinnerEMF = this.m_isinnerEMF;
    renderer.RecreateInnerEMF = this.AdjustMetaFile;
    renderer.m_skipInnerScale = this.m_skipScaling;
    if (this.OptimizeIdenticalImages && this.Document != null)
    {
      renderer.Document = this.Document;
      renderer.OptimizeIdenticalImages = this.OptimizeIdenticalImages;
    }
    if (this.IsEmbedFonts)
      renderer.IsEmbedFonts = this.IsEmbedFonts;
    if (this.IsEmbedCompleteFonts)
      renderer.IsEmbedCompleteFonts = this.IsEmbedCompleteFonts;
    using (Metafile metaFile = this.m_image.Clone() as Metafile)
    {
      using (MetaRecordParser metaRecordParser = new MetaRecordParser(renderer, metaFile))
      {
        if (this.IsTranparency)
        {
          renderer.AlphaBrush = this.AlphaBrush;
          renderer.AlphaPen = this.AlphaPen;
          renderer.BlendMode = this.BlendMode;
          renderer.IsTranparency = this.IsTranparency;
        }
        metaRecordParser.Parser.PageScale = this.m_pageScale;
        metaRecordParser.Parser.PageUnit = this.m_pageUnit;
        metaRecordParser.Enumerate();
        this.m_textRegions = metaRecordParser.Context as TextRegionManager;
        this.m_imageRegions = metaRecordParser.ImageContext as ImageRegionManager;
      }
    }
    this.m_bSaved = true;
  }

  protected override PdfLayoutResult Layout(PdfLayoutParams param)
  {
    if (PdfDocument.ConformanceLevel == PdfConformanceLevel.Pdf_X1A2001)
      param.Page.Document.ColorSpace = PdfColorSpace.CMYK;
    this.m_template.Graphics.ColorSpace = param.Page.Document.ColorSpace;
    if (param == null)
      throw new ArgumentNullException(nameof (param));
    this.Document = param.Page.Document;
    if (param.Format != null && param.Format is PdfMetafileLayoutFormat && (param.Format as PdfMetafileLayoutFormat).UseImageResolution)
      this.SetResolution(this.InternalImage.HorizontalResolution, this.InternalImage.VerticalResolution);
    if (!param.Page.Document.FileStructure.TaggedPdf)
      this.Save();
    MetafileLayouter metafileLayouter = new MetafileLayouter(this);
    if (this.m_isPdfGrid)
    {
      ShapeLayouter shapeLayouter = (ShapeLayouter) metafileLayouter;
      shapeLayouter.m_isPdfGrid = true;
      shapeLayouter.shapeBounds = this.shapeBounds;
      shapeLayouter.m_bottomCellPadding = this.m_bottomCellpadding;
    }
    metafileLayouter.IsImagePath = this.IsImagePath;
    return metafileLayouter.Layout(param);
  }

  protected override PdfLayoutResult Layout(HtmlToPdfLayoutParams param)
  {
    if (param == null)
      throw new ArgumentNullException(nameof (param));
    if (param.Page.Document != null)
      this.Document = param.Page.Document;
    if (!param.Page.Document.FileStructure.TaggedPdf && !this.IsDirectImageRendering)
      this.Save();
    return new MetafileLayouter(this).Layout(param);
  }

  internal static Metafile AdjustMetafile(Metafile metafile)
  {
    Metafile metafile1 = metafile != null ? metafile : throw new ArgumentNullException(nameof (metafile));
    if (metafile1 != null && !metafile.GetMetafileHeader().IsEmfOrEmfPlus())
    {
      metafile1 = PdfMetafile.ConvertToEmf(metafile);
      if (metafile1 == null)
        throw new ArgumentException("Can't parse metafile. Format is unknown.");
    }
    return metafile1;
  }

  private static Metafile ConvertToEmf(Metafile image)
  {
    MetafileHeader metafileHeader = image != null ? image.GetMetafileHeader() : throw new ArgumentNullException(nameof (image));
    Metafile emf = (Metafile) null;
    if (!metafileHeader.IsEmfOrEmfPlus())
    {
      image = (Metafile) image.Clone();
      SizeF physicalDimension = image.PhysicalDimension;
      IntPtr henhmetafile1 = image.GetHenhmetafile();
      int metaFileBitsEx = GdiApi.GetMetaFileBitsEx(henhmetafile1, 0, (byte[]) null);
      if (metaFileBitsEx > 0)
      {
        byte[] numArray = new byte[metaFileBitsEx];
        if (GdiApi.GetMetaFileBitsEx(henhmetafile1, metaFileBitsEx, numArray) > 0)
        {
          IntPtr zero = IntPtr.Zero;
          IntPtr dc = GdiApi.CreateDC("DISPLAY", (string) null, (string) null, IntPtr.Zero);
          float num1 = (float) ((double) PdfUnitConvertor.PxHorizontalResolution / (double) PdfUnitConvertor.HorizontalSize * 25.399999618530273);
          float num2 = (float) ((double) PdfUnitConvertor.PxVerticalResolution / (double) PdfUnitConvertor.VerticalSize * 25.399999618530273);
          float num3 = PdfUnitConvertor.HorizontalResolution / num1;
          float num4 = PdfUnitConvertor.VerticalResolution / num2;
          IntPtr henhmetafile2 = GdiApi.SetWinMetaFileBits(metaFileBitsEx, numArray, dc, ref new METAFILEPICT()
          {
            xExt = (int) ((double) physicalDimension.Width * (double) num3),
            yExt = (int) ((double) physicalDimension.Height * (double) num4),
            mm = 8
          });
          if (henhmetafile2 != IntPtr.Zero)
            emf = new Metafile(henhmetafile2, true);
          GdiApi.DeleteDC(dc);
        }
      }
      GdiApi.DeleteEnhMetaFile(henhmetafile1);
      image.Dispose();
    }
    else if (metafileHeader.Type == MetafileType.EmfPlusDual)
    {
      Rectangle frameRect = new Rectangle(0, 0, image.Width, image.Height);
      System.Drawing.Graphics graphics1 = System.Drawing.Graphics.FromImage((Image) new Bitmap(1, 1));
      IntPtr hdc = graphics1.GetHdc();
      MemoryStream memoryStream = new MemoryStream();
      emf = new Metafile((System.IO.Stream) memoryStream, hdc, frameRect, MetafileFrameUnit.Pixel, EmfType.EmfOnly);
      graphics1.Dispose();
      System.Drawing.Graphics graphics2 = System.Drawing.Graphics.FromImage((Image) emf);
      Rectangle rectangle = frameRect;
      graphics2.DrawImage((Image) image, rectangle, rectangle, GraphicsUnit.Pixel);
      graphics2.Dispose();
      memoryStream.Dispose();
    }
    return emf;
  }

  public void SetTransparency(
    float alphaPen,
    float alphaBrush,
    PdfBlendMode blendMode,
    bool transparency)
  {
    this.m_alphaBrush = alphaBrush;
    this.m_alphaPen = alphaPen;
    this.m_blendMode = blendMode;
    this.m_bIsTransparency = transparency;
  }
}
