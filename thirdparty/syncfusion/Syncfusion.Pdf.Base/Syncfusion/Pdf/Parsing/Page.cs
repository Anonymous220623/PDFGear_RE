// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.Page
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Primitives;
using Syncfusion.PdfViewer.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Threading;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class Page
{
  private const int c_ShadowWidth = 4;
  private const int c_ShadowHeight = 4;
  private static Pen s_borderPen = new Pen(Color.Black);
  internal List<RectangleF> matchTextPositions = new List<RectangleF>();
  internal int pageId = -1;
  private string m_webLink = string.Empty;
  private string m_annotType = string.Empty;
  private float m_annotX;
  private float m_annotY;
  private float m_annotRectHeight;
  private float m_annotWidth;
  private float m_annotHeight;
  private float m_annotBorderWidth;
  private RectangleF m_tempRect;
  internal Dictionary<RectangleF, string> pageAnnotations = new Dictionary<RectangleF, string>();
  internal List<PageAnnotation> pageAnnotList = new List<PageAnnotation>();
  internal Dictionary<RectangleF, string> pageURLs = new Dictionary<RectangleF, string>();
  internal List<PageAnnotation> pageURLList = new List<PageAnnotation>();
  internal Dictionary<RectangleF, string> pageTextsDict = new Dictionary<RectangleF, string>();
  internal List<TextMatchRectangle> pagetxtList = new List<TextMatchRectangle>();
  private PdfUnitConvertor m_unitConverter = new PdfUnitConvertor();
  private PdfPageResources m_resources;
  private PdfRecordCollection m_recordCollection;
  private PdfPageBase m_page;
  private int m_actualWidth;
  private int m_actualHeight;
  private RectangleF m_bounds;
  private double m_rotation;
  private string errorText;
  private Bitmap m_pageImage;
  private System.Drawing.Graphics m_graphics;
  private float m_zoomFactor = 1f;
  private int m_currentLocation;
  private Syncfusion.PdfViewer.Base.DeviceCMYK m_cmyk = new Syncfusion.PdfViewer.Base.DeviceCMYK();
  private object locker = new object();
  private bool _isRotationInitialized;
  private bool _isCropboxInitialized;
  private bool _isMediaboxInitialized;
  private RectangleF _cropboxRectangle = RectangleF.Empty;
  private RectangleF _mediaboxRectangle = RectangleF.Empty;
  internal string searchstring;
  public int CurrentLeftLocation;

  public Page(PdfPageBase page) => this.m_page = page;

  public int ActualWidth => this.m_actualWidth;

  public int CurrentLocation
  {
    get => this.m_currentLocation;
    set => this.m_currentLocation = value;
  }

  public int ActualHeight => this.m_actualHeight;

  public System.Drawing.Graphics Graphics
  {
    get
    {
      this.m_pageImage = new Bitmap((int) this.Width, (int) this.Height);
      this.m_graphics = System.Drawing.Graphics.FromImage((Image) this.m_pageImage);
      return this.m_graphics;
    }
  }

  public RectangleF Bounds
  {
    get => this.m_bounds;
    set => this.m_bounds = value;
  }

  public float Width
  {
    get => this.m_bounds.Width;
    set => this.m_bounds.Width = value;
  }

  public float Height
  {
    get => this.m_bounds.Height;
    set => this.m_bounds.Height = value;
  }

  internal double Rotation
  {
    get
    {
      if (this._isRotationInitialized)
        return this.m_rotation;
      if (this.m_page != null)
      {
        if (this.m_page.Rotation == PdfPageRotateAngle.RotateAngle90)
          this.m_rotation = 90.0;
        else if (this.m_page.Rotation == PdfPageRotateAngle.RotateAngle180)
          this.m_rotation = 180.0;
        else if (this.m_page.Rotation == PdfPageRotateAngle.RotateAngle270)
          this.m_rotation = 270.0;
      }
      this._isRotationInitialized = true;
      return this.m_rotation;
    }
  }

  internal PdfPageResources Resources => this.m_resources;

  internal PdfRecordCollection RecordCollection => this.m_recordCollection;

  internal RectangleF CropBox
  {
    get
    {
      if (this._isCropboxInitialized)
        return this._cropboxRectangle;
      PdfDictionary dictionary = this.m_page.Dictionary;
      if (dictionary.ContainsKey(nameof (CropBox)) && dictionary.GetValue(this.m_page.Dictionary.CrossTable, nameof (CropBox), "Parent") is PdfArray pdfArray)
      {
        float floatValue = (pdfArray[2] as PdfNumber).FloatValue;
        float height = (double) (pdfArray[3] as PdfNumber).FloatValue != 0.0 ? (pdfArray[3] as PdfNumber).FloatValue : (pdfArray[1] as PdfNumber).FloatValue;
        this._cropboxRectangle = new RectangleF(new PointF((pdfArray[0] as PdfNumber).FloatValue, (pdfArray[1] as PdfNumber).FloatValue), new SizeF(floatValue, height));
      }
      this._isCropboxInitialized = true;
      return this._cropboxRectangle;
    }
  }

  internal RectangleF MediaBox
  {
    get
    {
      if (this._isMediaboxInitialized)
        return this._mediaboxRectangle;
      PdfDictionary dictionary = this.m_page.Dictionary;
      if (dictionary.ContainsKey(nameof (MediaBox)) && dictionary[nameof (MediaBox)] is PdfArray pdfArray)
      {
        float floatValue = (pdfArray[2] as PdfNumber).FloatValue;
        float height = (double) (pdfArray[3] as PdfNumber).FloatValue != 0.0 ? (pdfArray[3] as PdfNumber).FloatValue : (pdfArray[1] as PdfNumber).FloatValue;
        this._mediaboxRectangle = new RectangleF(new PointF((pdfArray[0] as PdfNumber).FloatValue, (pdfArray[1] as PdfNumber).FloatValue), new SizeF(floatValue, height));
      }
      this._isMediaboxInitialized = true;
      return this._mediaboxRectangle;
    }
  }

  internal bool IsTextSelectionStarted { get; set; }

  internal bool IsTextSelectionCompleted { get; set; }

  internal int TextSelectionStart { get; set; }

  internal int TextSelectionEnd { get; set; }

  internal void Initialize(PdfPageBase page, bool needParsing)
  {
    try
    {
      if (needParsing && this.m_recordCollection == null)
      {
        this.m_resources = PageResourceLoader.Instance.GetPageResources(page);
        using (MemoryStream memoryStream = new MemoryStream())
        {
          byte[] buffer = PdfString.StringToByte("\r\n");
          for (int index = 0; index < page.Contents.Count; ++index)
          {
            PdfStream pdfStream = (PdfStream) null;
            IPdfPrimitive content = page.Contents[index];
            if ((object) (content as PdfReferenceHolder) != null)
              pdfStream = (page.Contents[index] as PdfReferenceHolder).Object as PdfStream;
            else if (content is PdfStream)
              pdfStream = content as PdfStream;
            if (pdfStream != null)
            {
              pdfStream.Decompress();
              pdfStream.InternalStream.WriteTo((Stream) memoryStream);
              memoryStream.Write(buffer, 0, buffer.Length);
            }
          }
          memoryStream.Position = 0L;
          this.m_recordCollection = new ContentParser(memoryStream.ToArray()).ReadContent();
        }
      }
      SizeF sizeF = new SizeF(this.m_unitConverter.ConvertToPixels(page.Size.Width, PdfGraphicsUnit.Point), this.m_unitConverter.ConvertToPixels(page.Size.Height, PdfGraphicsUnit.Point));
      this.Width = sizeF.Width;
      this.Height = sizeF.Height;
      this.m_actualWidth = (int) this.Width;
      this.m_actualHeight = (int) this.Height;
    }
    catch (Exception ex)
    {
      this.errorText = ex.StackTrace;
    }
  }

  internal void Initialize(PdfPageBase page, bool needParsing, float zoomFactor)
  {
    Monitor.Enter(this.locker);
    try
    {
      if (needParsing && this.m_recordCollection == null)
      {
        this.m_resources = PageResourceLoader.Instance.GetPageResources(page);
        using (MemoryStream memoryStream = new MemoryStream())
        {
          page.Layers.CombineContent((Stream) memoryStream);
          memoryStream.Position = 0L;
          this.m_recordCollection = new ContentParser(memoryStream.ToArray()).ReadContent();
        }
      }
      if (this.m_resources.ContainsKey("Annotations") && this.m_resources["Annotations"] is PdfArray resource)
        this.GetCurrentPageAnnotations(resource, page, zoomFactor);
      SizeF sizeF = new SizeF(this.m_unitConverter.ConvertToPixels(page.Size.Width, PdfGraphicsUnit.Point), this.m_unitConverter.ConvertToPixels(page.Size.Height, PdfGraphicsUnit.Point));
      this.Width = sizeF.Width;
      this.Height = sizeF.Height;
      this.m_actualWidth = (int) this.Width;
      this.m_actualHeight = (int) this.Height;
    }
    catch (Exception ex)
    {
      this.errorText = ex.StackTrace;
    }
    finally
    {
      Monitor.Exit(this.locker);
    }
  }

  private void GetCurrentPageAnnotations(PdfArray annots, PdfPageBase page, float zoomFactor)
  {
    RectangleF rectangleF = new RectangleF();
    pageAnnotDestinations = new PdfArray();
    this.pageAnnotations.Clear();
    this.pageAnnotList.Clear();
    for (int index = 0; index < annots.Count; ++index)
    {
      PdfDictionary pdfDictionary1 = !(annots[index].GetType().ToString() == "Syncfusion.Pdf.Primitives.PdfDictionary") ? (annots[index] as PdfReferenceHolder).Object as PdfDictionary : annots[index] as PdfDictionary;
      if (pdfDictionary1.ContainsKey("Subtype"))
      {
        PdfName pdfName = pdfDictionary1["Subtype"] as PdfName;
        if (pdfName == (PdfName) null)
          pdfName = (pdfDictionary1["Subtype"] as PdfReferenceHolder).Object as PdfName;
        this.m_annotType = pdfName.Value;
      }
      if (pdfDictionary1.ContainsKey("A"))
      {
        if (!(pdfDictionary1["A"] is PdfDictionary pdfDictionary2))
          pdfDictionary2 = (pdfDictionary1["A"] as PdfReferenceHolder).Object as PdfDictionary;
        if (pdfDictionary2.ContainsKey("URI"))
          this.m_webLink = (pdfDictionary2["URI"] as PdfString).Value;
        if (pdfDictionary2.ContainsKey("D"))
        {
          if (!(pdfDictionary2["D"] is PdfArray pageAnnotDestinations))
          {
            PdfReferenceHolder pdfReferenceHolder = pdfDictionary2["D"] as PdfReferenceHolder;
            if (pdfReferenceHolder != (PdfReferenceHolder) null)
              pageAnnotDestinations = pdfReferenceHolder.Object as PdfArray;
          }
          if (pageAnnotDestinations == null)
            pageAnnotDestinations = ((this.m_page as PdfLoadedPage).Document as PdfLoadedDocument).GetNamedDestination(pdfDictionary2["D"] as PdfString);
        }
      }
      if (pdfDictionary1.ContainsKey("Rect"))
        rectangleF = (pdfDictionary1["Rect"] as PdfArray).ToRectangle();
      if (pdfDictionary1.ContainsKey("Dest"))
      {
        if (!(pdfDictionary1["Dest"] is PdfArray pageAnnotDestinations))
        {
          PdfReferenceHolder pdfReferenceHolder = pdfDictionary1["Dest"] as PdfReferenceHolder;
          if (pdfReferenceHolder != (PdfReferenceHolder) null)
            pageAnnotDestinations = pdfReferenceHolder.Object as PdfArray;
        }
        if (pageAnnotDestinations == null)
          pageAnnotDestinations = ((this.m_page as PdfLoadedPage).Document as PdfLoadedDocument).GetNamedDestination(pdfDictionary1["Dest"] as PdfString);
      }
      if (pdfDictionary1.ContainsKey("Border"))
        this.m_annotBorderWidth = (float) (pdfDictionary1["Border"] as PdfArray)[2].ObjectCollectionIndex;
      if (pdfDictionary1.ContainsKey("BS") && pdfDictionary1["BS"] is PdfDictionary pdfDictionary3 && pdfDictionary3.ContainsKey("W"))
        this.m_annotBorderWidth = (pdfDictionary3["W"] as PdfNumber).FloatValue;
      if (!this.pageAnnotations.ContainsKey(rectangleF))
        this.GetAnnotRectProperties(page, rectangleF, this.m_webLink, zoomFactor, this.m_annotBorderWidth, this.m_annotType, pageAnnotDestinations);
    }
  }

  private void GetAnnotRectProperties(
    PdfPageBase page,
    RectangleF annotRect,
    string uri,
    float zoomFactor,
    float border,
    string annotType,
    PdfArray pageAnnotDestinations)
  {
    float num = this.m_unitConverter.ConvertFromPixels((float) this.m_actualHeight * zoomFactor, PdfGraphicsUnit.Point);
    this.m_annotX = annotRect.X * zoomFactor;
    this.m_annotRectHeight = annotRect.Height;
    this.m_annotY = (float) ((double) num - (double) annotRect.Y * (double) zoomFactor - (double) this.m_annotRectHeight * (double) zoomFactor);
    this.m_annotWidth = annotRect.Width * zoomFactor;
    this.m_annotHeight = annotRect.Height * zoomFactor;
    this.m_tempRect = new RectangleF(this.m_annotX, this.m_annotY, this.m_annotWidth, this.m_annotHeight);
    this.pageAnnotList.Add(new PageAnnotation(this.m_tempRect, uri, border, annotType, pageAnnotDestinations));
    if (this.pageAnnotations.ContainsKey(this.m_tempRect))
      return;
    this.pageAnnotations.Add(this.m_tempRect, uri);
  }

  internal void GetURLProperties(Page page, PageAnnotation annotProperties)
  {
    this.pageURLList.Add(annotProperties);
    if (this.pageURLs.ContainsKey(annotProperties.Rect))
      return;
    this.pageURLs.Add(annotProperties.Rect, annotProperties.URI);
  }

  internal void GetTextProperties(Page page, TextMatchRectangle annotProperties)
  {
    this.pagetxtList.Add(annotProperties);
    if (this.pageTextsDict.ContainsKey(annotProperties.Rect))
      return;
    this.pageTextsDict.Add(annotProperties.Rect, annotProperties.Text);
  }

  internal RectangleF GetTextRectProperties(
    Page page,
    RectangleF annotRect,
    float zoomFactor,
    bool isDrawingPanel)
  {
    float num1 = this.m_unitConverter.ConvertFromPixels(page.Width * zoomFactor, PdfGraphicsUnit.Point);
    float num2 = this.m_unitConverter.ConvertFromPixels((float) this.m_actualHeight * zoomFactor, PdfGraphicsUnit.Point);
    this.m_annotX = annotRect.X * zoomFactor;
    this.m_annotRectHeight = annotRect.Height;
    this.m_annotWidth = annotRect.Width * zoomFactor;
    this.m_annotHeight = annotRect.Height * zoomFactor;
    if (!isDrawingPanel)
      this.m_annotY = num2 + annotRect.Y * zoomFactor;
    else if (page.Rotation == 90.0)
    {
      float annotX = this.m_annotX;
      this.m_annotX = num2 - annotX;
      this.m_annotX += (float) page.CurrentLeftLocation;
      this.m_annotY = annotRect.Y * zoomFactor + (float) page.CurrentLocation;
    }
    else if (page.Rotation == 180.0)
    {
      float annotX = this.m_annotX;
      this.m_annotX = num1 - annotX;
      this.m_annotX += (float) page.CurrentLeftLocation;
      this.m_annotY = (float) ((double) num2 - (double) annotRect.Y * (double) zoomFactor + (double) page.CurrentLocation - (double) this.m_annotHeight - 9.0);
    }
    else if (page.Rotation == 270.0)
    {
      this.m_annotX += (float) page.CurrentLeftLocation;
      this.m_annotY = (float) ((double) num1 - (double) annotRect.Y * (double) zoomFactor + (double) page.CurrentLocation - (double) this.m_annotHeight - 9.0);
    }
    else
    {
      this.m_annotX += (float) page.CurrentLeftLocation;
      this.m_annotY = annotRect.Y * zoomFactor + (float) page.CurrentLocation;
    }
    this.m_tempRect = new RectangleF(this.m_annotX, this.m_annotY, this.m_annotWidth, this.m_annotHeight);
    return this.m_tempRect;
  }

  public void Draw(System.Drawing.Graphics g, bool printing)
  {
    try
    {
      CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
      Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
      if (printing)
        g.TranslateTransform(1f, 1f);
      GraphicsState gstate = g.Save();
      g.TranslateTransform(this.Bounds.Left, 0.0f);
      this.DrawPageBorders(g);
      PdfUnitConvertor pdfUnitConvertor = new PdfUnitConvertor(g);
      g.SetClip(this.Bounds, CombineMode.Replace);
      if (this.m_recordCollection == null)
        this.Initialize(this.m_page, true);
      new ImageRenderer(this.m_recordCollection, this.m_resources, g, true, this.m_cmyk, g.VisibleClipBounds.Height).RenderAsImage();
      g.Restore(gstate);
      Thread.CurrentThread.CurrentCulture = currentCulture;
    }
    catch (Exception ex)
    {
      this.errorText = ex.StackTrace;
    }
  }

  private void DrawToImage()
  {
    this.DrawPageBorders(this.Graphics);
    ImageRenderer imageRenderer = new ImageRenderer(this.m_recordCollection, this.m_resources, this.Graphics, true, this.m_cmyk, this.Graphics.VisibleClipBounds.Height);
    CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
    Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
    imageRenderer.RenderAsImage();
    Thread.CurrentThread.CurrentCulture = currentCulture;
  }

  private void DrawPageBorders(System.Drawing.Graphics g)
  {
    RectangleF bounds = this.Bounds;
    bounds.Width -= (float) (int) Page.s_borderPen.Width + bounds.Left;
    bounds.Height -= (float) (int) Page.s_borderPen.Width;
    bounds.Width = (float) (int) ((double) bounds.Width * (double) this.m_zoomFactor);
    g.FillRectangle(Brushes.White, bounds);
    g.DrawRectangle(Page.s_borderPen, bounds.X, bounds.Y, bounds.Width, bounds.Height);
  }

  public void SetLeft(int left)
  {
    this.Bounds = new RectangleF((float) left, this.Bounds.Top, this.Bounds.Width, this.Bounds.Height);
  }

  internal void Clear()
  {
    if (this.m_graphics != null)
      this.m_graphics.Dispose();
    this.matchTextPositions.Clear();
    if (this.Resources != null && this.Resources.Resources != null)
      this.Resources.Resources.Clear();
    this.pageAnnotations.Clear();
    this.pageAnnotList.Clear();
    this.pageURLs.Clear();
    this.pageURLList.Clear();
    this.pageTextsDict.Clear();
    this.pagetxtList.Clear();
  }

  internal bool SearchText(int pageIndex, string searchText, out List<Syncfusion.PdfViewer.Base.Glyph> texts)
  {
    this.searchstring = searchText;
    texts = new List<Syncfusion.PdfViewer.Base.Glyph>();
    bool flag = false;
    ImageRenderer.textDictonary.Clear();
    double num = (double) this.Height;
    double width = (double) this.Width;
    if (this.Rotation == 90.0 || this.Rotation == 270.0)
    {
      width = (double) this.Height;
      num = (double) this.Width;
    }
    using (Image image = (Image) new Bitmap((int) width, (int) num))
    {
      using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image))
      {
        g.TranslateTransform(0.0f, 0.0f);
        g.FillRectangle(Brushes.White, new Rectangle(0, 0, (int) width, (int) num));
        if (this.m_recordCollection == null)
          this.Initialize(this.m_page, true);
        ImageRenderer imageRenderer = new ImageRenderer(this.m_recordCollection, this.m_resources, g, true, new Syncfusion.PdfViewer.Base.DeviceCMYK(), (float) num);
        CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        imageRenderer.RenderAsImage();
        Thread.CurrentThread.CurrentCulture = currentCulture;
        texts.AddRange((IEnumerable<Syncfusion.PdfViewer.Base.Glyph>) imageRenderer.imageRenderGlyphList);
      }
    }
    return flag;
  }
}
