// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.HtmlToPdf.HtmlToPdfResult
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Graphics.Images.Metafiles;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.Parsing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

#nullable disable
namespace Syncfusion.Pdf.HtmlToPdf;

public class HtmlToPdfResult : IDisposable
{
  private const int m_splitOffset = 4;
  private const string TEXTBOX = "text";
  private const string INPUT = "input";
  private const string TEXTAREA = "textarea";
  private const string SUBMIT = "submit";
  private const string BUTTON = "button";
  private const string CHECKBOX = "checkbox";
  private const string RADIOBUTTON = "radio";
  private const string SELECTBOX = "select";
  private const string READONLY = "readonly";
  private const string CHECKED = "checked";
  private const string SELECTED = "selected";
  private const string PASSWORD = "password";
  private const string NUMBER = "number";
  private const string TEL = "tel";
  private const string EMAIL = "email";
  private const string FORMBEGIN = "formbegin";
  private const string FORMEND = "formend";
  private HtmlToPdfToc m_toc = new HtmlToPdfToc();
  private bool m_enableBookmark;
  private bool m_enableToc;
  private SinglePageLayout m_singlePageLayout;
  private bool m_isTempDir;
  private ArrayList m_pageBreakCollection;
  private ArrayList m_anchorsCollection;
  private ArrayList m_WebKitHyperlinkCollection = new ArrayList();
  private ArrayList m_documentLinkCollection;
  private ArrayList m_inputElemenetCollection;
  private ArrayList m_selectElementCollection;
  private ArrayList m_buttonElementCollection;
  private List<HtmlInternalLink> m_WebKitInternalLinkCollection = new List<HtmlInternalLink>();
  private List<HtmlInternalLink> m_internalLinkDestination = new List<HtmlInternalLink>();
  private List<HtmlToPdfAutoCreateForms> m_webkitAutoCreateForms = new List<HtmlToPdfAutoCreateForms>();
  private Image[] m_images;
  private PointF m_location;
  private float m_metafileTransparency;
  private long m_quality = 100;
  private Stream m_docStream;
  private bool m_Completed = true;
  private bool m_enableForm;
  private float m_height;
  private float m_remHeight;
  private PdfLayoutResult m_layoutResult;
  private bool m_isImagePath;
  internal string WebKitFilePath = string.Empty;
  internal string RenderEngine = string.Empty;
  internal string baseURL = string.Empty;
  internal bool m_enableDirectLayout;
  private PdfLayoutResult[] layoutDetails;
  private PdfDocument singlePdfDoc;

  public HtmlToPdfResult(
    Image[] image,
    ArrayList pageBreaks,
    ArrayList anchors,
    ArrayList documentLinks,
    ArrayList inputElements,
    ArrayList selectElements,
    ArrayList buttonElements)
  {
    this.m_images = image;
    this.m_anchorsCollection = anchors;
    this.m_pageBreakCollection = pageBreaks;
    this.m_documentLinkCollection = documentLinks;
    this.m_inputElemenetCollection = inputElements;
    this.m_selectElementCollection = selectElements;
    this.m_buttonElementCollection = buttonElements;
  }

  public HtmlToPdfResult(
    Image[] image,
    ArrayList pageBreaks,
    ArrayList anchors,
    ArrayList documentLinks)
  {
    this.m_images = image;
    this.m_anchorsCollection = anchors;
    this.m_pageBreakCollection = pageBreaks;
    this.m_documentLinkCollection = documentLinks;
  }

  public HtmlToPdfResult(Stream docStream) => this.m_docStream = docStream;

  public HtmlToPdfResult()
  {
  }

  internal HtmlToPdfResult(
    Image[] image,
    ArrayList pageBreaks,
    ArrayList anchors,
    ArrayList documentLinks,
    ArrayList inputElements,
    float remHeight,
    ArrayList selectElements,
    ArrayList buttonElements)
    : this(image, pageBreaks, anchors, documentLinks, inputElements, selectElements, buttonElements)
  {
    this.m_remHeight = remHeight;
  }

  internal HtmlToPdfResult(
    Image[] image,
    ArrayList pageBreaks,
    ArrayList anchors,
    ArrayList documentLinks,
    float remHeight)
    : this(image, pageBreaks, anchors, documentLinks)
  {
    this.m_remHeight = remHeight;
  }

  internal bool EnableBookmark
  {
    get => this.m_enableBookmark;
    set => this.m_enableBookmark = value;
  }

  internal bool EnableToc
  {
    get => this.m_enableToc;
    set => this.m_enableToc = value;
  }

  internal HtmlToPdfToc Toc
  {
    get => this.m_toc;
    set => this.m_toc = value;
  }

  internal bool IsImagePath
  {
    get => this.m_isImagePath;
    set => this.m_isImagePath = value;
  }

  internal ArrayList PageBreakCollection => this.m_pageBreakCollection;

  internal ArrayList AnchorsCollection => this.m_anchorsCollection;

  internal bool Completed => this.m_Completed;

  internal bool EnableForms
  {
    get => this.m_enableForm;
    set => this.m_enableForm = value;
  }

  internal float Height => this.m_height;

  public Image RenderedImage => this.m_images[0];

  public Image[] Images => this.m_images;

  public long Quality
  {
    set => this.m_quality = value;
  }

  public PointF Location
  {
    get => this.m_location;
    set => this.m_location = value;
  }

  public float MetafileTransparency
  {
    get => this.m_metafileTransparency;
    set
    {
      this.m_metafileTransparency = (double) value > 0.0 && (double) value <= 1.0 ? value : throw new PdfException("Value can only be greater than 0 and less than or equal to 1");
    }
  }

  internal PdfLayoutResult LayoutResult => this.m_layoutResult;

  internal SinglePageLayout SinglePageLayout
  {
    get => this.m_singlePageLayout;
    set => this.m_singlePageLayout = value;
  }

  internal bool IsTempDirectory
  {
    get => this.m_isTempDir;
    set => this.m_isTempDir = value;
  }

  private void DeleteFile(string filePath)
  {
    if (!File.Exists(filePath))
      return;
    File.Delete(filePath);
  }

  public void Render(PdfDocument document)
  {
    if (this.RenderEngine == "WebKit")
    {
      PdfMetafileLayoutFormat format = new PdfMetafileLayoutFormat();
      this.RenderWebKit((PdfPageBase) document.Pages[0], (PdfLayoutFormat) format);
    }
    else
    {
      PdfMetafileLayoutFormat format = new PdfMetafileLayoutFormat();
      this.Render((PdfPageBase) document.Pages.Add(), (PdfLayoutFormat) format);
    }
  }

  internal PdfDocument Render(PdfDocument document, PdfMetafileLayoutFormat metafileFormat)
  {
    this.RenderWebKit((PdfPageBase) document.Pages[0], (PdfLayoutFormat) metafileFormat);
    return this.singlePdfDoc == null ? document : this.singlePdfDoc;
  }

  public void Render(PdfPageBase page, PdfLayoutFormat format)
  {
    if (page == null)
      throw new PdfException("Page cannot be null.");
    format = format == null ? new PdfLayoutFormat() : format;
    if (this.RenderEngine == "WebKit")
    {
      this.RenderWebKit(page, format);
    }
    else
    {
      if (this.m_images == null)
        throw new PdfException("Image cannot be null.");
      PdfLayoutResult pdfLayoutResult = (PdfLayoutResult) null;
      PdfMetafileLayoutFormat metafileLayoutFormat = format as PdfMetafileLayoutFormat;
      if (this.m_enableDirectLayout && metafileLayoutFormat != null)
        metafileLayoutFormat.m_enableDirectLayout = true;
      foreach (Image image in this.m_images)
      {
        if (pdfLayoutResult != null && (double) pdfLayoutResult.Bounds.Size.Height <= (double) page.Size.Height)
        {
          page = (PdfPageBase) pdfLayoutResult.Page;
          if (this.PageBreakCollection.Count > 1 && this.m_images.Length > 1)
          {
            if (this.m_enableDirectLayout)
            {
              page = (PdfPageBase) pdfLayoutResult.Page.Section.Pages.Add();
              pdfLayoutResult = image is Metafile ? this.DrawMetaFile((Metafile) image, page, RectangleF.Empty, format, this.m_quality) : this.DrawBitmap((Bitmap) image, page, RectangleF.Empty, format);
            }
            else
              pdfLayoutResult = image is Metafile ? this.DrawMetaFile((Metafile) image, page, new RectangleF(0.0f, pdfLayoutResult.Bounds.Size.Height, page.Size.Width, 0.0f), format, this.m_quality) : this.DrawBitmap((Bitmap) image, page, new RectangleF(0.0f, pdfLayoutResult.Bounds.Size.Height, page.Size.Width, 0.0f), format);
          }
          else if (!this.m_enableDirectLayout)
          {
            pdfLayoutResult = image is Metafile ? this.DrawMetaFile((Metafile) image, page, new RectangleF(0.0f, pdfLayoutResult.Bounds.Size.Height - 4f, page.Size.Width, 0.0f), format, this.m_quality) : this.DrawBitmap((Bitmap) image, page, new RectangleF(0.0f, pdfLayoutResult.Bounds.Size.Height - 4f, page.Size.Width, 0.0f), format);
          }
          else
          {
            page = (PdfPageBase) pdfLayoutResult.Page.Section.Pages.Add();
            pdfLayoutResult = image is Metafile ? this.DrawMetaFile((Metafile) image, page, RectangleF.Empty, format, this.m_quality) : this.DrawBitmap((Bitmap) image, page, RectangleF.Empty, format);
          }
        }
        else if (image is Metafile && pdfLayoutResult == null)
        {
          PdfGraphicsState state = (PdfGraphicsState) null;
          if (page is PdfPage && (page as PdfPage).Document.FileStructure.TaggedPdf)
          {
            state = page.Graphics.Save();
            page.Graphics.ScaleTransform(0.75f, 0.75f);
          }
          pdfLayoutResult = this.DrawMetaFile((Metafile) image, page, RectangleF.Empty, format, this.m_quality);
          if (page is PdfPage && (page as PdfPage).Document.FileStructure.TaggedPdf && state != null)
          {
            page.Graphics.ScaleTransform(1f, 1f);
            page.Graphics.Restore(state);
          }
        }
        else
          pdfLayoutResult = this.DrawBitmap((Bitmap) image, page, RectangleF.Empty, format);
      }
      if (page is PdfPage && (page as PdfPage).Section.ParentDocument is PdfDocument && (page as PdfPage).Section.ParentDocument.FileStructure.TaggedPdf)
      {
        if ((double) this.m_remHeight > 0.0)
        {
          this.m_height = pdfLayoutResult.Bounds.Height;
          this.m_Completed = false;
        }
        else
          this.m_Completed = true;
        this.m_layoutResult = pdfLayoutResult;
      }
      Dictionary<int, List<PdfUriAnnotation>> dictionary = new Dictionary<int, List<PdfUriAnnotation>>();
      PdfDocument document = (page as PdfPage).Document;
      if (document != null && this.m_documentLinkCollection != null && this.m_documentLinkCollection.Count > 0)
      {
        foreach (PdfPage page1 in document.Pages)
        {
          for (int index1 = 0; index1 < page1.Annotations.Count; ++index1)
          {
            if (page1.Annotations[index1] is PdfDocumentLinkAnnotation)
            {
              PdfDocumentLinkAnnotation annotation1 = page1.Annotations[index1] as PdfDocumentLinkAnnotation;
              if (annotation1.Destination == null)
              {
                float num = 0.0f;
                for (int index2 = 0; index2 < document.Pages.Count && annotation1.Destination == null; ++index2)
                {
                  PdfPage page2 = document.Pages[index2];
                  for (int index3 = page2.Annotations.Count - 1; index3 >= 0; --index3)
                  {
                    foreach (KeyValuePair<int, List<PdfUriAnnotation>> keyValuePair in dictionary)
                    {
                      bool flag = false;
                      if (keyValuePair.Key == index3)
                      {
                        foreach (PdfUriAnnotation pdfUriAnnotation in keyValuePair.Value)
                        {
                          if (pdfUriAnnotation.Text == annotation1.Text)
                          {
                            PdfDestination pdfDestination = new PdfDestination((PdfPageBase) pdfUriAnnotation.Page, pdfUriAnnotation.Location);
                            annotation1.Destination = pdfDestination;
                            flag = true;
                            break;
                          }
                        }
                        if (flag)
                          break;
                      }
                      else if (flag)
                        break;
                    }
                    if (page2.Annotations[index3] is PdfUriAnnotation)
                    {
                      PdfUriAnnotation annotation2 = page2.Annotations[index3] as PdfUriAnnotation;
                      if (annotation2.Text == annotation1.Text)
                      {
                        PointF location = annotation2.Location;
                        if ((double) location.Y > (double) num)
                          location.Y -= num;
                        PdfDestination pdfDestination = new PdfDestination((PdfPageBase) page2, location);
                        annotation1.Destination = pdfDestination;
                        if (!dictionary.ContainsKey(index2))
                          dictionary.Add(index2, new List<PdfUriAnnotation>());
                        dictionary[index2].Add(annotation2);
                        page2.Annotations.RemoveAt(index3);
                        break;
                      }
                    }
                  }
                  num += page2.Graphics.ClientSize.Height;
                }
              }
            }
          }
        }
      }
      dictionary.Clear();
      foreach (PdfPage page3 in document.Pages)
      {
        int count = page3.Annotations.Count;
        try
        {
          for (int index = 0; index < count; ++index)
          {
            PdfAnnotation annotation = page3.Annotations[index];
            if (annotation is PdfUriAnnotation && (double) annotation.Border.Width > 0.0)
            {
              page3.Annotations.Remove(annotation);
              --index;
            }
          }
        }
        catch
        {
        }
      }
    }
  }

  public void Render(PdfPageBase page, PdfLayoutFormat format, out PdfLayoutResult result)
  {
    if (page == null)
      throw new PdfException("Page cannot be null.");
    result = (PdfLayoutResult) null;
    format = format == null ? new PdfLayoutFormat() : format;
    if (this.RenderEngine == "WebKit")
    {
      result = this.RenderWebKit(page, format);
    }
    else
    {
      if (this.m_images == null)
        throw new PdfException("Image cannot be null.");
      ArrayList pageBreakCollection = this.m_pageBreakCollection;
      result = (PdfLayoutResult) null;
      PdfMetafileLayoutFormat metafileLayoutFormat = format as PdfMetafileLayoutFormat;
      if (this.m_enableDirectLayout && metafileLayoutFormat != null)
        metafileLayoutFormat.m_enableDirectLayout = true;
      foreach (Image image in this.m_images)
      {
        if (result != null && (double) result.Bounds.Size.Height <= (double) page.Size.Height)
        {
          page = (PdfPageBase) result.Page;
          if (this.PageBreakCollection.Count > 1 && this.m_images.Length > 1)
          {
            if (this.m_enableDirectLayout)
            {
              page = (PdfPageBase) result.Page.Section.Pages.Add();
              result = image is Metafile ? this.DrawMetaFile((Metafile) image, page, RectangleF.Empty, format, this.m_quality) : this.DrawBitmap((Bitmap) image, page, RectangleF.Empty, format);
            }
            else
              result = image is Metafile ? this.DrawMetaFile((Metafile) image, page, new RectangleF(0.0f, result.Bounds.Size.Height, page.Size.Width, 0.0f), format, this.m_quality) : this.DrawBitmap((Bitmap) image, page, new RectangleF(0.0f, result.Bounds.Size.Height, page.Size.Width, 0.0f), format);
          }
          else
            result = image is Metafile ? this.DrawMetaFile((Metafile) image, page, new RectangleF(0.0f, result.Bounds.Size.Height, page.Size.Width, 0.0f), format, this.m_quality) : this.DrawBitmap((Bitmap) image, page, new RectangleF(0.0f, result.Bounds.Size.Height, page.Size.Width, 0.0f), format);
        }
        else
        {
          if (result == null)
          {
            PointF location = this.Location;
            if (image is Metafile)
            {
              result = this.DrawMetaFile((Metafile) image, page, new RectangleF(this.Location.X, this.Location.Y, page.Size.Width, 0.0f), format, this.m_quality);
              if (page == result.Page)
              {
                result = new PdfLayoutResult(result.Page, new RectangleF(result.Bounds.X, result.Bounds.Y, result.Bounds.Width, result.Bounds.Height + this.Location.Y));
                continue;
              }
              continue;
            }
          }
          result = !(image is Metafile) || result != null ? this.DrawBitmap((Bitmap) image, page, RectangleF.Empty, format) : this.DrawMetaFile((Metafile) image, page, RectangleF.Empty, format, this.m_quality);
        }
      }
    }
  }

  private PdfLayoutResult RenderWebKit(PdfPageBase page, PdfLayoutFormat format)
  {
    CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
    Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
    PdfLayoutResult pdfLayoutResult1 = new PdfLayoutResult(page as PdfPage, RectangleF.Empty);
    PdfMetafileLayoutFormat metafileLayoutFormat = format as PdfMetafileLayoutFormat;
    try
    {
      PdfLoadedDocument pdfLoadedDocument = File.Exists(this.WebKitFilePath + ".pdf") && File.Exists(this.WebKitFilePath + ".txt") ? new PdfLoadedDocument(File.ReadAllBytes(this.WebKitFilePath + ".pdf")) : throw new PdfException("Failed to convert the webpage");
      string str1 = string.Empty;
      using (StreamReader streamReader = new StreamReader(this.WebKitFilePath + ".txt"))
        str1 = streamReader.ReadToEnd();
      this.DeleteFile(this.WebKitFilePath + ".txt");
      string[] strArray1 = new string[0];
      if (this.EnableForms)
      {
        int startIndex = str1.IndexOf("formbegin", StringComparison.CurrentCultureIgnoreCase);
        int num = str1.IndexOf("formend", StringComparison.CurrentCultureIgnoreCase);
        strArray1 = str1.Substring(startIndex, num - startIndex).Split(new string[1]
        {
          "forms"
        }, StringSplitOptions.None);
        str1 = str1.Remove(startIndex, num + "formend".Length - startIndex);
      }
      string[] strArray2 = str1.Split(new string[1]{ "\n" }, StringSplitOptions.RemoveEmptyEntries);
      float num1 = float.Parse(strArray2[1].Trim(), (IFormatProvider) CultureInfo.InvariantCulture);
      int num2 = 0;
      List<TextRegionManager> textRegionManagerList = new List<TextRegionManager>();
      TextRegionManager textRegionManager1 = new TextRegionManager();
      ImageRegionManager imageRegionManager1 = new ImageRegionManager();
      List<ImageRegionManager> imageRegionManagerList1 = new List<ImageRegionManager>();
      ImageRegionManager imageRegionManager2 = new ImageRegionManager();
      List<ImageRegionManager> imageRegionManagerList2 = new List<ImageRegionManager>();
      float num3 = float.Parse(strArray2[0].Split(new string[1]
      {
        ","
      }, StringSplitOptions.RemoveEmptyEntries)[1].Trim(), (IFormatProvider) CultureInfo.InvariantCulture);
      float num4 = 0.0f;
      float num5 = 0.0f;
      float num6 = 0.0f;
      Dictionary<string, RectangleF> dictionary = new Dictionary<string, RectangleF>();
      for (int index1 = 2; index1 < strArray2.Length; ++index1)
      {
        string[] strArray3 = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))").Split(strArray2[index1]);
        if (strArray3.Length > 4)
        {
          switch (strArray3[0])
          {
            case "text":
              try
              {
                num6 = float.Parse(strArray3[1], (IFormatProvider) CultureInfo.InvariantCulture);
                float dx1 = float.Parse(strArray3[2], (IFormatProvider) CultureInfo.InvariantCulture);
                float dy1 = float.Parse(strArray3[3], (IFormatProvider) CultureInfo.InvariantCulture);
                float num7 = float.Parse(strArray3[4], (IFormatProvider) CultureInfo.InvariantCulture);
                float num8 = float.Parse(strArray3[5], (IFormatProvider) CultureInfo.InvariantCulture);
                float dx2 = float.Parse(strArray3[6], (IFormatProvider) CultureInfo.InvariantCulture);
                float dy2 = float.Parse(strArray3[7], (IFormatProvider) CultureInfo.InvariantCulture);
                Matrix matrix = new Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
                matrix.Multiply(new Matrix(num1, 0.0f, 0.0f, num1, 0.0f, 0.0f));
                matrix.Multiply(new Matrix(num6, 0.0f, 0.0f, num6, dx1, dy1));
                matrix.Multiply(new Matrix(1f, 0.0f, 0.0f, 1f, dx2, dy2));
                num4 = num1 * num6;
                if ((double) num4 < 0.0)
                  num4 = Math.Abs(num4);
                PdfUnitConvertor pdfUnitConvertor = new PdfUnitConvertor(96f);
                TextRegion region = new TextRegion(pdfUnitConvertor.ConvertToPixels(matrix.OffsetY - (float) ((double) num7 * (double) num4 - (double) num8 * (double) num4), PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertToPixels(num7 * num4, PdfGraphicsUnit.Point));
                textRegionManager1.Add(region);
                continue;
              }
              catch (Exception ex)
              {
                continue;
              }
            case "Hyperlink":
              try
              {
                string relativeUri = strArray3[1].Substring(1, strArray3[1].Length - 2);
                int num9 = int.Parse(strArray3[2], (IFormatProvider) CultureInfo.InvariantCulture);
                float num10 = float.Parse(strArray3[3], (IFormatProvider) CultureInfo.InvariantCulture);
                float num11 = float.Parse(strArray3[4], (IFormatProvider) CultureInfo.InvariantCulture);
                float num12 = float.Parse(strArray3[5], (IFormatProvider) CultureInfo.InvariantCulture);
                float num13 = float.Parse(strArray3[6], (IFormatProvider) CultureInfo.InvariantCulture);
                if (num9 != -1)
                {
                  Uri result1;
                  Uri.TryCreate(this.baseURL, UriKind.RelativeOrAbsolute, out result1);
                  Uri result2;
                  Uri.TryCreate(result1, relativeUri, out result2);
                  this.m_WebKitHyperlinkCollection.Add((object) new HtmlHyperLink()
                  {
                    Bounds = new RectangleF(num10 * num4, num11 * num4, num12 * num4, num13 * num4),
                    Href = result2.OriginalString,
                    Name = num9.ToString()
                  });
                  continue;
                }
                continue;
              }
              catch (Exception ex)
              {
                continue;
              }
            case "Internallink":
              try
              {
                int num14 = int.Parse(strArray3[2], (IFormatProvider) CultureInfo.InvariantCulture);
                float num15 = float.Parse(strArray3[3], (IFormatProvider) CultureInfo.InvariantCulture);
                float num16 = float.Parse(strArray3[4], (IFormatProvider) CultureInfo.InvariantCulture);
                float num17 = float.Parse(strArray3[5], (IFormatProvider) CultureInfo.InvariantCulture);
                float num18 = float.Parse(strArray3[6], (IFormatProvider) CultureInfo.InvariantCulture);
                HtmlHyperLink htmlHyperLink = new HtmlHyperLink();
                HtmlInternalLink htmlInternalLink = new HtmlInternalLink();
                if (num14 != -1)
                {
                  Uri result3;
                  Uri.TryCreate(this.baseURL, UriKind.RelativeOrAbsolute, out result3);
                  Uri result4;
                  Uri.TryCreate(result3, strArray3[1], out result4);
                  htmlHyperLink.Bounds = new RectangleF(num15 * num4, num16 * num4, num17 * num4, num18 * num4);
                  htmlHyperLink.Href = result4.OriginalString;
                  htmlHyperLink.Name = num14.ToString();
                  htmlInternalLink.SourcePageNumber = num14.ToString();
                  htmlInternalLink.Bounds = new RectangleF(num15 * num4, num16 * num4, num17 * num4, num18 * num4);
                  if (strArray3[1].Contains("#"))
                  {
                    string[] strArray4 = strArray3[1].Split('#');
                    if (strArray3[1] == "#")
                    {
                      this.m_WebKitHyperlinkCollection.Add((object) htmlHyperLink);
                      continue;
                    }
                    if (strArray4[0] == this.baseURL || strArray4[0] == string.Empty || strArray4[0].Substring(0, 1) == "/")
                    {
                      if (strArray4[1] != string.Empty)
                      {
                        bool flag = true;
                        for (int index2 = 0; index2 < strArray4[1].Length; ++index2)
                        {
                          if (char.IsLetter(strArray4[1][index2]))
                            flag = false;
                        }
                        if (flag)
                        {
                          this.m_WebKitHyperlinkCollection.Add((object) htmlHyperLink);
                          continue;
                        }
                        htmlInternalLink.Href = strArray4[1];
                        this.m_WebKitInternalLinkCollection.Add(htmlInternalLink);
                        continue;
                      }
                      continue;
                    }
                    this.m_WebKitHyperlinkCollection.Add((object) htmlHyperLink);
                    continue;
                  }
                  string str2 = strArray3[1];
                  int num19 = int.Parse(strArray3[2], (IFormatProvider) CultureInfo.InvariantCulture);
                  float num20 = float.Parse(strArray3[3], (IFormatProvider) CultureInfo.InvariantCulture);
                  float num21 = float.Parse(strArray3[4], (IFormatProvider) CultureInfo.InvariantCulture);
                  this.m_internalLinkDestination.Add(new HtmlInternalLink()
                  {
                    ID = str2,
                    DestinationPageNumber = num19,
                    Destination = new PointF(num20 * num4, num21 * num4)
                  });
                  continue;
                }
                continue;
              }
              catch
              {
                continue;
              }
            case "Header":
              try
              {
                HtmlInternalLink htmlInternalLink = new HtmlInternalLink();
                htmlInternalLink.HeaderTagLevel = strArray3[1];
                htmlInternalLink.ID = strArray3[2];
                string str3 = strArray3[3].Substring(1, strArray3[3].Length - 2);
                htmlInternalLink.HeaderContent = str3;
                htmlInternalLink.DestinationPageNumber = this.SinglePageLayout != SinglePageLayout.None ? 0 : int.Parse(strArray3[4], (IFormatProvider) CultureInfo.InvariantCulture);
                float num22 = float.Parse(strArray3[5], (IFormatProvider) CultureInfo.InvariantCulture);
                float num23 = float.Parse(strArray3[6], (IFormatProvider) CultureInfo.InvariantCulture);
                if (htmlInternalLink.DestinationPageNumber >= 1)
                {
                  if ((double) num22 >= 0.0)
                  {
                    if ((double) num23 >= 0.0)
                    {
                      bool flag = false;
                      foreach (HtmlInternalLink webKitInternalLink in this.m_WebKitInternalLinkCollection)
                      {
                        if (webKitInternalLink.Href == htmlInternalLink.ID && webKitInternalLink.Href != "")
                        {
                          webKitInternalLink.HeaderTagLevel = htmlInternalLink.HeaderTagLevel;
                          webKitInternalLink.ID = htmlInternalLink.ID;
                          webKitInternalLink.HeaderContent = htmlInternalLink.HeaderContent;
                          webKitInternalLink.DestinationPageNumber = htmlInternalLink.DestinationPageNumber;
                          webKitInternalLink.Destination = new PointF(num22 * num4, num23 * num4);
                          flag = true;
                          break;
                        }
                      }
                      if (!flag)
                      {
                        htmlInternalLink.Destination = new PointF(num22 * num4, num23 * num4);
                        this.m_WebKitInternalLinkCollection.Add(htmlInternalLink);
                        continue;
                      }
                      continue;
                    }
                    continue;
                  }
                  continue;
                }
                continue;
              }
              catch
              {
                continue;
              }
            default:
              try
              {
                num6 = float.Parse(strArray3[1], (IFormatProvider) CultureInfo.InvariantCulture);
                float dx3 = float.Parse(strArray3[2], (IFormatProvider) CultureInfo.InvariantCulture);
                float dy3 = float.Parse(strArray3[3], (IFormatProvider) CultureInfo.InvariantCulture);
                float dx4 = float.Parse(strArray3[4], (IFormatProvider) CultureInfo.InvariantCulture);
                float dy4 = float.Parse(strArray3[5], (IFormatProvider) CultureInfo.InvariantCulture);
                double num24 = (double) float.Parse(strArray3[6], (IFormatProvider) CultureInfo.InvariantCulture);
                float num25 = float.Parse(strArray3[7], (IFormatProvider) CultureInfo.InvariantCulture);
                Matrix matrix = new Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
                matrix.Multiply(new Matrix(num1, 0.0f, 0.0f, num1, 0.0f, 0.0f));
                matrix.Multiply(new Matrix(num6, 0.0f, 0.0f, num6, dx3, dy3));
                matrix.Multiply(new Matrix(1f, 0.0f, 0.0f, 1f, dx4, dy4));
                num4 = num1 * num6;
                PdfUnitConvertor pdfUnitConvertor = new PdfUnitConvertor(96f);
                ImageRegion region = new ImageRegion(pdfUnitConvertor.ConvertToPixels(matrix.OffsetY, PdfGraphicsUnit.Point), pdfUnitConvertor.ConvertToPixels(num25 * num4, PdfGraphicsUnit.Point));
                imageRegionManager1.Add(region);
                continue;
              }
              catch (Exception ex)
              {
                continue;
              }
          }
        }
        else if (strArray3.Length == 2)
        {
          try
          {
            if (strArray3[0] == "shape")
              num5 = num1 * float.Parse(strArray3[1].Trim(), (IFormatProvider) CultureInfo.InvariantCulture);
            else
              num3 = float.Parse(strArray3[1].Trim(), (IFormatProvider) CultureInfo.InvariantCulture);
          }
          catch (Exception ex)
          {
          }
        }
        else
        {
          try
          {
            ++num2;
            if (strArray3.Length == 1)
            {
              num1 = float.Parse(strArray3[0].Trim(), (IFormatProvider) CultureInfo.InvariantCulture);
              if (!metafileLayoutFormat.SplitTextLines)
                textRegionManagerList.Add(textRegionManager1);
              textRegionManager1 = new TextRegionManager();
              if (!metafileLayoutFormat.SplitImages)
                imageRegionManagerList1.Add(imageRegionManager1);
              imageRegionManager1 = new ImageRegionManager();
            }
          }
          catch (Exception ex)
          {
          }
        }
      }
      this.updateInternalLink();
      if (!metafileLayoutFormat.SplitTextLines)
        textRegionManagerList.Add(textRegionManager1);
      TextRegionManager textRegionManager2 = new TextRegionManager();
      if (!metafileLayoutFormat.SplitImages)
        imageRegionManagerList1.Add(imageRegionManager1);
      ImageRegionManager imageRegionManager3 = new ImageRegionManager();
      for (int index = 1; index < strArray1.Length; ++index)
      {
        num4 = (double) num5 > 0.0 ? num5 : num4;
        string[] strArray5 = Regex.Split(strArray1[index], ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
        string id = strArray5[1];
        string str4 = strArray5[2];
        if (str4.StartsWith("\"") && str4.EndsWith("\""))
        {
          str4 = str4.Substring(1, str4.Length - 2);
          if (str4.StartsWith("\"\"") && str4.EndsWith("\"\""))
            str4 = str4.Replace("\"\"", "\"");
        }
        string str5 = str4;
        bool isReadonly = false;
        if (strArray5[3] == "readonly")
          isReadonly = true;
        bool selected = false;
        string str6 = strArray5[4];
        if (str6 == "checked" || str6 == "selected")
          selected = true;
        string optionValue = strArray5[5];
        string type = strArray5[6];
        int pageNo = int.Parse(strArray5[7], (IFormatProvider) CultureInfo.InvariantCulture) - 1;
        int num26 = int.Parse(strArray5[8], (IFormatProvider) CultureInfo.InvariantCulture);
        int num27 = int.Parse(strArray5[9], (IFormatProvider) CultureInfo.InvariantCulture);
        int num28 = int.Parse(strArray5[10], (IFormatProvider) CultureInfo.InvariantCulture);
        int num29 = int.Parse(strArray5[11], (IFormatProvider) CultureInfo.InvariantCulture);
        RectangleF bounds = new RectangleF((float) num26 * num4, (float) num27 * num4, (float) num28 * num4, (float) num29 * num4);
        this.m_webkitAutoCreateForms.Add(new HtmlToPdfAutoCreateForms(id, str5, isReadonly, selected, type, pageNo, bounds, optionValue));
      }
      for (int index = 0; index < pdfLoadedDocument.Pages.Count; ++index)
      {
        foreach (HtmlToPdfAutoCreateForms webkitAutoCreateForm in this.m_webkitAutoCreateForms)
        {
          if (webkitAutoCreateForm.ElementPageNo == index)
          {
            float dx = webkitAutoCreateForm.ElementBounds.X / num4;
            float dy = webkitAutoCreateForm.ElementBounds.Y / num4;
            RectangleF elementBounds = webkitAutoCreateForm.ElementBounds;
            double num30 = (double) elementBounds.Width / (double) num4;
            elementBounds = webkitAutoCreateForm.ElementBounds;
            float height = elementBounds.Height / num4;
            Matrix matrix = new Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
            matrix.Multiply(new Matrix(num1, 0.0f, 0.0f, num1, 0.0f, 0.0f));
            matrix.Multiply(new Matrix(num6, 0.0f, 0.0f, num6, -1f, -1f));
            matrix.Multiply(new Matrix(1f, 0.0f, 0.0f, 1f, dx, dy));
            ImageRegion region = new ImageRegion(new PdfUnitConvertor(96f).ConvertToPixels(matrix.OffsetY, PdfGraphicsUnit.Point), height);
            imageRegionManager2.Add(region);
          }
        }
        imageRegionManagerList2.Add(imageRegionManager2);
        imageRegionManager2 = new ImageRegionManager();
      }
      PdfLayoutResult pdfLayoutResult2 = (PdfLayoutResult) null;
      PdfPage page1 = (PdfPage) null;
      double num31 = 0.0;
      if (this.EnableForms)
        this.layoutDetails = new PdfLayoutResult[pdfLoadedDocument.PageCount];
      if (this.EnableBookmark || this.EnableToc)
      {
        this.m_WebKitInternalLinkCollection.Sort((Comparison<HtmlInternalLink>) ((Header1, Header2) => Header1.Destination.Y.CompareTo(Header2.Destination.Y)));
        List<HtmlInternalLink> htmlInternalLinkList = new List<HtmlInternalLink>();
        for (int index = 1; index <= pdfLoadedDocument.Pages.Count; ++index)
        {
          foreach (HtmlInternalLink webKitInternalLink in this.m_WebKitInternalLinkCollection)
          {
            if (webKitInternalLink.DestinationPageNumber == index)
              htmlInternalLinkList.Add(webKitInternalLink);
          }
        }
        this.m_WebKitInternalLinkCollection = htmlInternalLinkList;
      }
      PdfPage page2 = page as PdfPage;
      if (this.EnableToc && this.m_WebKitInternalLinkCollection.Count != 0)
      {
        this.Toc.TocPageCount = this.Toc.GetRectangleHeightAndTocPageCount(page, this.m_WebKitInternalLinkCollection);
        PdfMargins pdfMargins = new PdfMargins();
        pdfMargins.All = 0.0f;
        for (int index3 = 0; index3 < this.Toc.TocPageCount; ++index3)
        {
          PdfLoadedPageCollection pages = pdfLoadedDocument.Pages;
          int index4 = index3;
          SizeF clientSize = page2.GetClientSize();
          double width = (double) clientSize.Width;
          clientSize = page2.GetClientSize();
          double height = (double) clientSize.Height;
          SizeF size = new SizeF((float) width, (float) height);
          PdfMargins margins = pdfMargins;
          pages.Insert(index4, size, margins);
        }
      }
      PdfPageBase pdfPageBase1 = page;
      if (this.SinglePageLayout != SinglePageLayout.None && pdfLoadedDocument.PageCount > 1)
      {
        this.singlePdfDoc = new PdfDocument();
        float num32 = (float) (int) Math.Ceiling((double) num3 * (double) num4);
        PdfDocument document = (page as PdfPage).Document;
        float height = num32 + document.PageSettings.Margins.Top + document.PageSettings.Margins.Bottom;
        if (this.SinglePageLayout == SinglePageLayout.FitWidth)
        {
          if (document.Template.Top != null)
            height += document.Template.Top.Height;
          if (document.Template.Bottom != null)
            height += document.Template.Bottom.Height;
          this.singlePdfDoc.Template = document.Template;
        }
        this.singlePdfDoc.PageSettings.Orientation = (double) document.PageSettings.Size.Width >= (double) height ? PdfPageOrientation.Landscape : PdfPageOrientation.Portrait;
        this.singlePdfDoc.PageSettings.Margins = document.PageSettings.Margins;
        this.singlePdfDoc.PageSettings.Rotate = document.PageSettings.Rotate;
        this.singlePdfDoc.PageSettings.Size = new SizeF(document.PageSettings.Size.Width, height);
        page = (PdfPageBase) this.singlePdfDoc.Pages.Add();
      }
      SizeF sizeF;
      for (int index = 0; index < pdfLoadedDocument.Pages.Count; ++index)
      {
        PdfTemplate template = pdfLoadedDocument.Pages[index].CreateTemplate();
        HtmlToPdfFormat htmlToPdfFormat = new HtmlToPdfFormat();
        htmlToPdfFormat.SplitTextLines = metafileLayoutFormat.SplitTextLines;
        htmlToPdfFormat.SplitImages = metafileLayoutFormat.SplitImages;
        htmlToPdfFormat.Layout = PdfLayoutType.Paginate;
        ArrayList arrayList = new ArrayList();
        if (index >= this.Toc.TocPageCount)
        {
          foreach (HtmlHyperLink webKitHyperlink in this.m_WebKitHyperlinkCollection)
          {
            if (webKitHyperlink.Name == (index - this.Toc.TocPageCount + 1).ToString())
            {
              if (pdfLayoutResult2 != null && index > this.Toc.TocPageCount)
              {
                double height1 = (double) pdfLayoutResult2.Bounds.Height;
                sizeF = (page as PdfPage).GetClientSize();
                double height2 = (double) (int) sizeF.Height;
                if (height1 < height2)
                  webKitHyperlink.Bounds = new RectangleF(webKitHyperlink.Bounds.X, webKitHyperlink.Bounds.Y + (float) pdfLayoutResult2.TotalPageSize, webKitHyperlink.Bounds.Width, webKitHyperlink.Bounds.Height);
              }
              arrayList.Add((object) webKitHyperlink);
            }
          }
          foreach (HtmlInternalLink webKitInternalLink in this.m_WebKitInternalLinkCollection)
          {
            if (webKitInternalLink.SourcePageNumber == (index - this.Toc.TocPageCount + 1).ToString() && webKitInternalLink.DestinationPageNumber > 0)
            {
              if (pdfLayoutResult2 != null && index > this.Toc.TocPageCount)
              {
                HtmlInternalLink htmlInternalLink = webKitInternalLink;
                double x = (double) webKitInternalLink.Bounds.X;
                double y1 = (double) webKitInternalLink.Bounds.Y;
                RectangleF bounds = pdfLayoutResult2.Bounds;
                double height3 = (double) bounds.Height;
                double y2 = y1 + height3;
                bounds = webKitInternalLink.Bounds;
                double width = (double) bounds.Width;
                bounds = webKitInternalLink.Bounds;
                double height4 = (double) bounds.Height;
                RectangleF rectangleF = new RectangleF((float) x, (float) y2, (float) width, (float) height4);
                htmlInternalLink.Bounds = rectangleF;
              }
              webKitInternalLink.Destination = new PointF(webKitInternalLink.Destination.X, webKitInternalLink.Destination.Y);
              webKitInternalLink.DestinationPage = this.SinglePageLayout != SinglePageLayout.None ? pdfLoadedDocument.Pages[0] : pdfLoadedDocument.Pages[webKitInternalLink.DestinationPageNumber + this.Toc.TocPageCount - 1];
              htmlToPdfFormat.HtmlInternalLinksCollection.Add(webKitInternalLink);
            }
          }
        }
        htmlToPdfFormat.HtmlHyperlinksCollection = arrayList;
        if (index >= this.Toc.TocPageCount)
        {
          if (!htmlToPdfFormat.SplitTextLines && index < textRegionManagerList.Count + this.Toc.TocPageCount)
            htmlToPdfFormat.TextRegionManager = textRegionManagerList[index - this.Toc.TocPageCount];
          if (!htmlToPdfFormat.SplitImages && index < textRegionManagerList.Count + this.Toc.TocPageCount)
            htmlToPdfFormat.ImageRegionManager = imageRegionManagerList1[index - this.Toc.TocPageCount];
          if (this.EnableForms)
          {
            this.layoutDetails[index - this.Toc.TocPageCount] = pdfLayoutResult1;
            htmlToPdfFormat.FormRegionManager = imageRegionManagerList2[index - this.Toc.TocPageCount];
          }
        }
        htmlToPdfFormat.TotalPageLayoutSize = num3 * num4;
        htmlToPdfFormat.PageNumber = index + 1;
        htmlToPdfFormat.PageCount = pdfLoadedDocument.Pages.Count;
        htmlToPdfFormat.TotalPageSize = num31;
        if (pdfLayoutResult2 != null)
        {
          double height5 = (double) pdfLayoutResult2.Bounds.Height;
          sizeF = page.Size;
          double height6 = (double) sizeF.Height;
          if (height5 <= height6)
          {
            PdfTemplate pdfTemplate = template;
            PdfPage page3 = pdfLayoutResult2.Page;
            HtmlToPdfFormat format1 = htmlToPdfFormat;
            double x = (double) this.Location.X;
            double bottom = (double) pdfLayoutResult2.Bounds.Bottom;
            sizeF = page1.GetClientSize();
            double width = (double) sizeF.Width;
            sizeF = page1.GetClientSize();
            double height7 = (double) sizeF.Height;
            RectangleF layoutRectangle = new RectangleF((float) x, (float) bottom, (float) width, (float) height7);
            pdfLayoutResult2 = pdfTemplate.Draw(page3, format1, layoutRectangle);
            goto label_144;
          }
        }
        page1 = page as PdfPage;
        PdfTemplate pdfTemplate1 = template;
        PdfPage page4 = page1;
        HtmlToPdfFormat format2 = htmlToPdfFormat;
        double x1 = (double) this.Location.X;
        double y = (double) this.Location.Y;
        sizeF = page1.GetClientSize();
        double width1 = (double) sizeF.Width;
        sizeF = page1.GetClientSize();
        double height = (double) sizeF.Height;
        RectangleF layoutRectangle1 = new RectangleF((float) x1, (float) y, (float) width1, (float) height);
        pdfLayoutResult2 = pdfTemplate1.Draw(page4, format2, layoutRectangle1);
label_144:
        if (index >= this.Toc.TocPageCount)
          num31 = pdfLayoutResult2.TotalPageSize;
        pdfLayoutResult1 = pdfLayoutResult2;
      }
      PdfDocument document1 = (page as PdfPage).Document;
      if (this.EnableBookmark && this.m_WebKitInternalLinkCollection.Count != 0)
        new HtmlInternalLink()
        {
          TocPageCount = this.Toc.TocPageCount
        }.AddBookmark(page1, document1, this.m_WebKitInternalLinkCollection);
      if (this.EnableToc && this.m_WebKitInternalLinkCollection.Count != 0)
        this.Toc.DrawTable(document1, page2, this.m_WebKitInternalLinkCollection);
      if (document1 != null && this.m_WebKitInternalLinkCollection != null && this.m_WebKitInternalLinkCollection.Count > 0)
      {
        foreach (PdfPage page5 in document1.Pages)
        {
          for (int index = 0; index < page5.Annotations.Count; ++index)
          {
            if (page5.Annotations[index] is PdfDocumentLinkAnnotation)
            {
              PdfDocumentLinkAnnotation annotation = page5.Annotations[index] as PdfDocumentLinkAnnotation;
              if (annotation.Destination == null)
              {
                foreach (HtmlInternalLink webKitInternalLink in this.m_WebKitInternalLinkCollection)
                {
                  if (annotation.Text == webKitInternalLink.Href)
                  {
                    PdfPage page6 = document1.Pages[webKitInternalLink.DestinationPageNumber + this.Toc.TocPageCount - 1];
                    annotation.Destination = new PdfDestination((PdfPageBase) page6);
                    annotation.Destination.Location = webKitInternalLink.Destination;
                    break;
                  }
                }
              }
            }
          }
        }
      }
      if (this.EnableForms)
        this.createPdfForms((page as PdfPage).Document);
      double num33 = (double) num3 * (double) num4 - pdfLayoutResult2.TotalPageSize;
      RectangleF bounds1 = pdfLayoutResult2.Bounds;
      double height8 = (double) bounds1.Height;
      double num34 = num33 + height8;
      PdfPage page7 = pdfLayoutResult1.Page;
      bounds1 = pdfLayoutResult1.Bounds;
      double x2 = (double) bounds1.X;
      bounds1 = pdfLayoutResult1.Bounds;
      double y3 = (double) bounds1.Y;
      bounds1 = pdfLayoutResult1.Bounds;
      double width2 = (double) bounds1.Width;
      double height9 = num34;
      RectangleF bounds2 = new RectangleF((float) x2, (float) y3, (float) width2, (float) height9);
      pdfLayoutResult1 = new PdfLayoutResult(page7, bounds2);
      if (pdfLoadedDocument.PageCount == 1)
      {
        double totalPageSize = pdfLayoutResult2.TotalPageSize;
        sizeF = pdfLayoutResult1.Page.Size;
        double height10 = (double) sizeF.Height;
        if (totalPageSize > height10)
        {
          PdfPage pdfPage = page as PdfPage;
          pdfPage.Document.Pages.Remove(pdfLayoutResult1.Page);
          PdfPage page8 = pdfPage;
          sizeF = pdfPage.GetClientSize();
          double width3 = (double) sizeF.Width;
          sizeF = pdfPage.GetClientSize();
          double height11 = (double) sizeF.Height;
          RectangleF bounds3 = new RectangleF(0.0f, 0.0f, (float) width3, (float) height11);
          pdfLayoutResult1 = new PdfLayoutResult(page8, bounds3);
        }
      }
      if (this.SinglePageLayout == SinglePageLayout.FitHeight)
      {
        if (pdfLoadedDocument.PageCount > 1)
        {
          PdfDocument document2 = (pdfPageBase1 as PdfPage).Document;
          PdfTemplate template1 = page.CreateTemplate();
          double height12 = (double) template1.Height;
          sizeF = pdfPageBase1.Size;
          double height13 = (double) sizeF.Height;
          float num35 = (float) (height12 / height13);
          this.singlePdfDoc = new PdfDocument();
          this.singlePdfDoc.PageSettings.Margins = document2.PageSettings.Margins;
          this.singlePdfDoc.PageSettings.Rotate = document2.PageSettings.Rotate;
          float width4 = document2.PageSettings.Width / num35;
          this.singlePdfDoc.PageSettings.Orientation = (double) width4 >= (double) document2.PageSettings.Height ? PdfPageOrientation.Landscape : PdfPageOrientation.Portrait;
          this.singlePdfDoc.PageSettings.Size = new SizeF(width4, document2.PageSettings.Height);
          PdfPageBase pdfPageBase2 = (PdfPageBase) this.singlePdfDoc.Pages.Add();
          PdfGraphics graphics = pdfPageBase2.Graphics;
          PdfTemplate template2 = template1;
          PointF location = new PointF(0.0f, 0.0f);
          sizeF = pdfPageBase2.Size;
          double width5 = (double) sizeF.Width;
          sizeF = pdfPageBase2.Size;
          double height14 = (double) sizeF.Height;
          SizeF size = new SizeF((float) width5, (float) height14);
          graphics.DrawPdfTemplate(template2, location, size);
        }
      }
    }
    finally
    {
      if (this.IsTempDirectory)
        Directory.Delete(new DirectoryInfo(this.WebKitFilePath).Parent.FullName, true);
      this.DeleteFile(this.WebKitFilePath + ".txt");
      this.DeleteFile(this.WebKitFilePath + ".pdf");
    }
    Thread.CurrentThread.CurrentCulture = currentCulture;
    return pdfLayoutResult1;
  }

  private PdfLayoutResult DrawMetaFile(
    Metafile metafile,
    PdfPageBase page,
    RectangleF bounds,
    PdfLayoutFormat format,
    long quality)
  {
    PdfMetafile pdfMetafile = new PdfMetafile(metafile);
    pdfMetafile.Quality = quality;
    if ((double) this.m_metafileTransparency > 0.0)
      pdfMetafile.SetTransparency(this.m_metafileTransparency, this.m_metafileTransparency, PdfBlendMode.Normal, true);
    PdfMetafileLayoutFormat format1 = format is PdfMetafileLayoutFormat ? format as PdfMetafileLayoutFormat : new PdfMetafileLayoutFormat();
    float[] pageOffsets = new float[this.m_pageBreakCollection.Count];
    format = format == null ? (PdfLayoutFormat) new PdfMetafileLayoutFormat() : format;
    if (format1.SplitImages && format1.SplitTextLines)
      pdfMetafile.IsDirectImageRendering = true;
    if (this.m_enableDirectLayout)
      format1.m_enableDirectLayout = true;
    this.m_pageBreakCollection.CopyTo((System.Array) pageOffsets);
    pdfMetafile.HtmlHyperlinksCollection = this.m_anchorsCollection;
    pdfMetafile.DocumentLinksCollection = this.m_documentLinkCollection;
    pdfMetafile.InputElementCollection = this.m_inputElemenetCollection;
    pdfMetafile.SelectElementCollection = this.m_selectElementCollection;
    pdfMetafile.ButtonElementCollection = this.m_buttonElementCollection;
    pdfMetafile.IsImagePath = this.IsImagePath;
    pdfMetafile.m_isHtmlToTaggedPdf = (page as PdfPage).Document.FileStructure.TaggedPdf;
    return pdfMetafile.Draw((PdfPage) page, bounds, pageOffsets, (PdfLayoutFormat) format1);
  }

  private PdfLayoutResult DrawBitmap(
    Bitmap bitmap,
    PdfPageBase page,
    RectangleF bounds,
    PdfLayoutFormat format)
  {
    PdfBitmap pdfBitmap = new PdfBitmap((Image) bitmap);
    format = format == null ? new PdfLayoutFormat() : format;
    return pdfBitmap.Draw((PdfPage) page, bounds.Location, format);
  }

  internal void UpdateFormBounds(SizeF size)
  {
    for (int index = 0; index < this.layoutDetails.Length; ++index)
    {
      foreach (HtmlToPdfAutoCreateForms webkitAutoCreateForm in this.m_webkitAutoCreateForms)
      {
        if (webkitAutoCreateForm.ElementPageNo == index && (double) this.layoutDetails[index].Bounds.Height < (double) size.Height)
        {
          webkitAutoCreateForm.ElementBounds = new RectangleF(webkitAutoCreateForm.ElementBounds.X, webkitAutoCreateForm.ElementBounds.Y + this.layoutDetails[index].Bounds.Height, webkitAutoCreateForm.ElementBounds.Width, webkitAutoCreateForm.ElementBounds.Height);
          if ((double) webkitAutoCreateForm.ElementBounds.Y + (double) webkitAutoCreateForm.ElementBounds.Height > (double) size.Height)
          {
            ++webkitAutoCreateForm.ElementPageNo;
            webkitAutoCreateForm.ElementBounds = new RectangleF(webkitAutoCreateForm.ElementBounds.X, webkitAutoCreateForm.ElementBounds.Y - size.Height - this.layoutDetails[index].Bounds.Height, webkitAutoCreateForm.ElementBounds.Width, webkitAutoCreateForm.ElementBounds.Height);
          }
        }
      }
    }
  }

  internal void updateInternalLink()
  {
    foreach (HtmlInternalLink htmlInternalLink in this.m_internalLinkDestination)
    {
      foreach (HtmlInternalLink webKitInternalLink in this.m_WebKitInternalLinkCollection)
      {
        if (webKitInternalLink.Href == htmlInternalLink.ID)
        {
          webKitInternalLink.DestinationPageNumber = htmlInternalLink.DestinationPageNumber;
          webKitInternalLink.Destination = htmlInternalLink.Destination;
        }
      }
    }
  }

  internal void createPdfForms(PdfDocument lDoc)
  {
    SizeF size = (SizeF) Size.Empty;
    if (lDoc != null && lDoc.Pages.Count >= 0)
      size = lDoc.Pages[0].GetClientSize();
    this.UpdateFormBounds(size);
    for (int index = 0; index < this.m_webkitAutoCreateForms.Count; ++index)
    {
      HtmlToPdfAutoCreateForms webkitAutoCreateForm1 = this.m_webkitAutoCreateForms[index];
      webkitAutoCreateForm1.ElementPageNo += this.Toc.TocPageCount;
      if (webkitAutoCreateForm1.ElementPageNo >= 0)
      {
        if (webkitAutoCreateForm1.ElementType == "text" || webkitAutoCreateForm1.ElementType == "textarea" || webkitAutoCreateForm1.ElementType == "input" || webkitAutoCreateForm1.ElementType == "password" || webkitAutoCreateForm1.ElementType == "number" || webkitAutoCreateForm1.ElementType == "email")
        {
          if (webkitAutoCreateForm1.ElementId == string.Empty)
            webkitAutoCreateForm1.ElementId = "text_" + (object) Guid.NewGuid();
          PdfTextBoxField field = new PdfTextBoxField((PdfPageBase) lDoc.Pages[webkitAutoCreateForm1.ElementPageNo], webkitAutoCreateForm1.ElementId);
          field.Bounds = webkitAutoCreateForm1.ElementBounds;
          field.Text = webkitAutoCreateForm1.ElementValue;
          field.ReadOnly = webkitAutoCreateForm1.IsReadOnly;
          if (webkitAutoCreateForm1.ElementType == "textarea")
          {
            field.Multiline = true;
            field.Scrollable = true;
          }
          lDoc.Form.Fields.Add((PdfField) field);
        }
        else if (webkitAutoCreateForm1.ElementType == "checkbox")
        {
          if (webkitAutoCreateForm1.ElementId == string.Empty)
            webkitAutoCreateForm1.ElementId = "checkbox_" + (object) Guid.NewGuid();
          PdfCheckBoxField field = new PdfCheckBoxField((PdfPageBase) lDoc.Pages[webkitAutoCreateForm1.ElementPageNo], webkitAutoCreateForm1.ElementId);
          field.Bounds = webkitAutoCreateForm1.ElementBounds;
          field.Checked = webkitAutoCreateForm1.IsSelected;
          field.ReadOnly = webkitAutoCreateForm1.IsReadOnly;
          lDoc.Form.Fields.Add((PdfField) field);
        }
        else if (webkitAutoCreateForm1.ElementType.Equals("submit", StringComparison.CurrentCultureIgnoreCase) || webkitAutoCreateForm1.ElementType.Equals("button", StringComparison.CurrentCultureIgnoreCase) && webkitAutoCreateForm1.ElementValue != "")
        {
          if (webkitAutoCreateForm1.ElementId == string.Empty)
            webkitAutoCreateForm1.ElementId = "submit_" + (object) Guid.NewGuid();
          PdfButtonField field = new PdfButtonField((PdfPageBase) lDoc.Pages[webkitAutoCreateForm1.ElementPageNo], webkitAutoCreateForm1.ElementId);
          field.Bounds = webkitAutoCreateForm1.ElementBounds;
          field.Text = webkitAutoCreateForm1.ElementValue;
          field.ReadOnly = webkitAutoCreateForm1.IsReadOnly;
          lDoc.Form.Fields.Add((PdfField) field);
        }
        else if (webkitAutoCreateForm1.ElementType == "radio")
        {
          int num = 0;
          bool flag = true;
          PdfRadioButtonListField field = new PdfRadioButtonListField((PdfPageBase) lDoc.Pages[webkitAutoCreateForm1.ElementPageNo], webkitAutoCreateForm1.ElementId);
          foreach (HtmlToPdfAutoCreateForms webkitAutoCreateForm2 in this.m_webkitAutoCreateForms)
          {
            if (webkitAutoCreateForm2.ElementPageNo >= 0 && webkitAutoCreateForm1.ElementId == webkitAutoCreateForm2.ElementId && webkitAutoCreateForm1.ElementPageNo == webkitAutoCreateForm2.ElementPageNo)
            {
              PdfRadioButtonListItem radioButtonListItem = new PdfRadioButtonListItem(webkitAutoCreateForm2.ElementValue);
              radioButtonListItem.Bounds = webkitAutoCreateForm2.ElementBounds;
              radioButtonListItem.ReadOnly = webkitAutoCreateForm2.IsReadOnly;
              if (!webkitAutoCreateForm2.IsSelected && flag)
                ++num;
              else
                flag = false;
              field.Items.Add(radioButtonListItem);
            }
          }
          if (!flag && field.Items.Count != 0)
            field.SelectedIndex = num;
          lDoc.Form.Fields.Add((PdfField) field);
        }
        else if (webkitAutoCreateForm1.ElementType == "select")
        {
          int num = 0;
          bool flag = true;
          PdfComboBoxField pdfComboBoxField;
          PdfComboBoxField field = pdfComboBoxField = new PdfComboBoxField((PdfPageBase) lDoc.Pages[webkitAutoCreateForm1.ElementPageNo], webkitAutoCreateForm1.ElementId);
          foreach (HtmlToPdfAutoCreateForms webkitAutoCreateForm3 in this.m_webkitAutoCreateForms)
          {
            if (webkitAutoCreateForm1.ElementBounds == webkitAutoCreateForm3.ElementBounds && webkitAutoCreateForm3.ElementBounds != this.m_webkitAutoCreateForms[index - 1].ElementBounds)
            {
              PdfListFieldItem pdfListFieldItem = new PdfListFieldItem(webkitAutoCreateForm3.ElementValue, webkitAutoCreateForm3.OptionValue);
              field.Bounds = webkitAutoCreateForm3.ElementBounds;
              if (webkitAutoCreateForm3.IsSelected)
                field.ReadOnly = webkitAutoCreateForm3.IsReadOnly;
              if (!webkitAutoCreateForm3.IsSelected && flag)
                ++num;
              else
                flag = false;
              field.Items.Add(pdfListFieldItem);
            }
          }
          if (!flag)
          {
            if (field.Items.Count != 0)
              field.SelectedIndex = num;
          }
          else if (field.Items.Count != 0)
            field.SelectedIndex = 0;
          lDoc.Form.Fields.Add((PdfField) field);
        }
      }
    }
  }

  void IDisposable.Dispose()
  {
    if (this.m_images != null)
    {
      for (int index = 0; index < this.m_images.Length; ++index)
        this.m_images[index].Dispose();
    }
    this.m_images = (Image[]) null;
    this.m_pageBreakCollection = (ArrayList) null;
  }
}
