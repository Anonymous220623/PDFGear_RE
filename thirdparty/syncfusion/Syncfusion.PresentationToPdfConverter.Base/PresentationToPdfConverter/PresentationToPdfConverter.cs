// Decompiled with JetBrains decompiler
// Type: Syncfusion.PresentationToPdfConverter.PresentationToPdfConverter
// Assembly: Syncfusion.PresentationToPdfConverter.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 66FE5253-50B1-47E3-888F-DF2FAFB49C7E
// Assembly location: C:\Program Files\PDFgear\Syncfusion.PresentationToPdfConverter.Base.dll

using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.Xmp;
using Syncfusion.Presentation;
using Syncfusion.Presentation.Drawing;
using Syncfusion.Presentation.Rendering;
using Syncfusion.Presentation.SlideImplementation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;

#nullable disable
namespace Syncfusion.PresentationToPdfConverter;

public class PresentationToPdfConverter : IDisposable
{
  private PresentationToPdfConverterSettings _settings;
  private GDIRenderer _gdiRenderer;
  private float _x;
  private float _y;
  private float _width;
  private float _height;
  private float _slideNumber;
  private bool _isNewPage = true;
  private float _slideSpace;

  internal PresentationToPdfConverter(PresentationToPdfConverterSettings settings)
  {
    this._settings = settings;
  }

  internal GDIRenderer GdiRenderer => this._gdiRenderer ?? (this._gdiRenderer = new GDIRenderer());

  public static PdfDocument Convert(IPresentation presentation)
  {
    return Syncfusion.PresentationToPdfConverter.PresentationToPdfConverter.Convert(presentation, (PresentationToPdfConverterSettings) null);
  }

  public static PdfDocument Convert(IPresentation presentation, bool isPortableRendering)
  {
    return Syncfusion.PresentationToPdfConverter.PresentationToPdfConverter.Convert(presentation, new PresentationToPdfConverterSettings()
    {
      EnablePortableRendering = isPortableRendering
    });
  }

  public static PdfDocument Convert(
    IPresentation presentation,
    PresentationToPdfConverterSettings settings)
  {
    if (Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.IsAzureCompatible)
    {
      if (settings == null)
        settings = new PresentationToPdfConverterSettings();
      settings.EnablePortableRendering = true;
    }
    PdfDocument pdfDocument = settings == null || settings.PdfConformanceLevel == PdfConformanceLevel.None ? new PdfDocument() : new PdfDocument(settings.PdfConformanceLevel);
    using (Syncfusion.PresentationToPdfConverter.PresentationToPdfConverter presentationToPdfConverter = new Syncfusion.PresentationToPdfConverter.PresentationToPdfConverter(settings))
    {
      Syncfusion.Presentation.Presentation presentation1 = (Syncfusion.Presentation.Presentation) presentation;
      presentation1.IsPdfConversion = (byte) 1;
      if (settings != null && settings.PublishOptions == PublishOptions.NotesPages)
      {
        PdfPageSettings pageSettings = pdfDocument.PageSettings;
        NotesSize notesSize = presentation1.NotesSize as NotesSize;
        presentationToPdfConverter._width = pageSettings.Width = (float) Helper.EmuToPoint(notesSize.CX);
        presentationToPdfConverter._height = pageSettings.Height = (float) Helper.EmuToPoint(notesSize.CY);
        int width = 0;
        int height = 0;
        bool checkHasNotesSlide = presentation1.CheckHasNotesSlide;
        if (!checkHasNotesSlide)
          presentation1.Slides[0].AddNotesSlide();
        foreach (Shape shape in (IEnumerable<ISlideItem>) presentation1.NotesMaster.Shapes)
        {
          if (shape.GetPlaceholder() != null && shape.GetPlaceholder().Type == PlaceholderType.SlideImage)
          {
            width = Helper.PointToPixel(shape.Width);
            height = Helper.PointToPixel(shape.Height);
            break;
          }
        }
        foreach (ISlide slide in (IEnumerable<ISlide>) presentation.Slides)
        {
          INotesSlide notesSlide = slide.NotesSlide;
          if (notesSlide != null)
          {
            NotesSlide notes = (NotesSlide) notesSlide;
            if (checkHasNotesSlide)
            {
              System.Drawing.Image image1 = notes.ParentSlide.ConvertToImage(ImageType.Metafile);
              Stream stream = (Stream) new MemoryStream();
              image1.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
              System.Drawing.Image image2 = (System.Drawing.Image) new Bitmap(image1, new Size(width, height));
              notes.ThumbnailImage = image2;
            }
            else
              notes.ThumbnailImage = (System.Drawing.Image) new Bitmap(width, height);
            if (settings != null && settings.EnablePortableRendering)
              notes.EnablePortableRendering(settings.EnablePortableRendering);
            presentationToPdfConverter.DrawSlide(pdfDocument, notes, presentation);
          }
        }
      }
      else
      {
        presentationToPdfConverter.AddDocumentProperties(pdfDocument, presentation.BuiltInDocumentProperties as BuiltInDocumentProperties);
        PdfPageSettings pageSettings = pdfDocument.PageSettings;
        ISlideSize slideSize = presentation1.SlideSize;
        if (settings == null || settings.PublishOptions == PublishOptions.Slides)
        {
          presentationToPdfConverter._width = pageSettings.Width = (float) slideSize.Width;
          presentationToPdfConverter._height = pageSettings.Height = (float) slideSize.Height;
        }
        else
        {
          pageSettings.Width = 540f;
          pageSettings.Height = 720f;
          PdfMargins margins = pageSettings.Margins;
          switch (presentationToPdfConverter._settings.SlidesPerPage)
          {
            case SlidesPerPage.Six:
              presentationToPdfConverter._width = (float) (((double) pageSettings.Width - ((double) margins.Left + (double) margins.Right + (double) margins.Left / 2.0)) / 2.0);
              presentationToPdfConverter._height = presentationToPdfConverter._width / 1.77f;
              float num1 = presentationToPdfConverter._height * 3f;
              presentationToPdfConverter._slideSpace = (float) (((double) pageSettings.Height - (double) margins.Top - (double) margins.Bottom - (double) num1) / 6.0);
              break;
            case SlidesPerPage.One:
              presentationToPdfConverter._width = pageSettings.Width = (float) slideSize.Width;
              presentationToPdfConverter._height = pageSettings.Height = (float) slideSize.Height;
              break;
            case SlidesPerPage.Two:
              presentationToPdfConverter._width = pageSettings.Width - (margins.Left + margins.Right);
              presentationToPdfConverter._height = presentationToPdfConverter._width / 1.77f;
              float num2 = presentationToPdfConverter._height * 2f;
              presentationToPdfConverter._slideSpace = (float) (((double) pageSettings.Height - (double) margins.Top - (double) margins.Bottom - (double) num2) / 4.0);
              presentationToPdfConverter._width -= presentationToPdfConverter._slideSpace * 2f;
              break;
            case SlidesPerPage.Three:
              presentationToPdfConverter._width = (float) (((double) pageSettings.Width - ((double) margins.Left + (double) margins.Right + (double) margins.Left / 2.0)) / 2.0);
              presentationToPdfConverter._height = presentationToPdfConverter._width / 1.77f;
              float num3 = presentationToPdfConverter._height * 3f;
              presentationToPdfConverter._slideSpace = (float) (((double) pageSettings.Height - (double) margins.Top - (double) margins.Bottom - (double) num3) / 6.0);
              break;
            case SlidesPerPage.Four:
              presentationToPdfConverter._width = (float) (((double) pageSettings.Width - ((double) margins.Left + (double) margins.Right + (double) margins.Left / 2.0)) / 2.0);
              presentationToPdfConverter._height = presentationToPdfConverter._width / 1.33f;
              float num4 = presentationToPdfConverter._height * 2f;
              presentationToPdfConverter._slideSpace = (float) (((double) pageSettings.Height - (double) margins.Top - (double) margins.Bottom - (double) num4) / 4.0);
              break;
            case SlidesPerPage.Nine:
              presentationToPdfConverter._width = (float) (((double) pageSettings.Width - ((double) margins.Left + (double) margins.Right + (double) margins.Left)) / 3.0);
              presentationToPdfConverter._height = presentationToPdfConverter._width / 1.77f;
              float num5 = presentationToPdfConverter._height * 3f;
              presentationToPdfConverter._slideSpace = (float) (((double) pageSettings.Height - (double) margins.Top - (double) margins.Bottom - (double) num5) / 6.0);
              break;
          }
        }
        foreach (ISlide slide in (IEnumerable<ISlide>) presentation.Slides)
        {
          if (slide.Visible || presentationToPdfConverter._settings != null && presentationToPdfConverter._settings.ShowHiddenSlides)
          {
            ++presentationToPdfConverter._slideNumber;
            if (settings != null && settings.EnablePortableRendering)
              (slide as Slide).EnablePortableRendering(settings.EnablePortableRendering);
            presentationToPdfConverter.DrawSlide(pdfDocument, slide, presentation);
          }
        }
        presentationToPdfConverter.AddHyperlink(pdfDocument, (PdfPage) null, (presentation as Syncfusion.Presentation.Presentation).DocumentLinkHyperlinks);
      }
      presentation1.IsPdfConversion = (byte) 0;
    }
    return pdfDocument;
  }

  private void DrawSlide(PdfDocument pdfDocument, NotesSlide notes, IPresentation presentation)
  {
    PdfPage page = pdfDocument.Pages.Add();
    this.InitializePageMargins(page.Section.PageSettings.Margins);
    this.DrawHandoutMaster(pdfDocument, page, (HandoutMaster) null);
    PdfMargins margins = pdfDocument.PageSettings.Margins;
    this.CalculateImagePosition(margins);
    PdfGraphics graphics1 = page.Graphics;
    if (this._settings != null && this._settings.EnablePortableRendering)
    {
      graphics1.IsDirectPDF = true;
      graphics1.OptimizeIdenticalImages = this._settings.OptimizeIdenticalImages;
      graphics1.NativeGraphics = System.Drawing.Graphics.FromImage((System.Drawing.Image) new Bitmap(1, 1));
      MemoryStream stream = new MemoryStream();
      System.Drawing.Graphics graphics2 = System.Drawing.Graphics.FromImage(new Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter().CreateImage(notes, ImageType.Metafile, stream));
      graphics2.PageUnit = GraphicsUnit.Point;
      Syncfusion.Presentation.Presentation presentation1 = (Syncfusion.Presentation.Presentation) presentation;
      presentation1.Graphics = graphics2;
      PDFRenderer pdfRenderer = new PDFRenderer(graphics1, graphics2, GraphicsUnit.Point, pdfDocument);
      presentation1.Renderer = (RendererBase) pdfRenderer;
      pdfRenderer.EmbedFonts = this._settings.EmbedFonts;
      pdfRenderer.EmbedCompleteFonts = this._settings.EmbedCompleteFonts;
      pdfRenderer.ImageQuality = this._settings.ImageQuality;
      pdfRenderer.FontStreams = presentation.FontSettings.FontStreams;
      pdfRenderer.PrivateFonts = presentation.FontSettings.PrivateFonts;
      if (presentation.FontSettings.FallbackFonts.Count > 0)
        pdfRenderer.FallbackFonts = presentation.FontSettings.FallbackFonts;
      notes.Layout();
      pdfRenderer.DrawSlide(notes);
      graphics1.NativeGraphics.Dispose();
      graphics1.IsDirectPDF = false;
    }
    else
    {
      PdfMetafile image = new PdfMetafile(notes.ConvertToImage(Syncfusion.Drawing.ImageFormat.Emf));
      if (this._settings != null)
      {
        image.IsEmbedFonts = this._settings.EmbedFonts;
        image.IsEmbedCompleteFonts = this._settings.EmbedCompleteFonts;
        if (this._settings._imageResolution > 0)
          image.ImageResolution = this._settings._imageResolution;
        else
          image.Quality = (long) this._settings.ImageQuality;
        if (this._settings.OptimizeIdenticalImages)
        {
          image.OptimizeIdenticalImages = this._settings.OptimizeIdenticalImages;
          image.Document = pdfDocument;
        }
      }
      if (presentation.FontSettings.PrivateFonts != null && presentation.FontSettings.FontStreams != null)
      {
        image.CustomFont = new CustomFont();
        image.CustomFont.EmbeddedFonts = presentation.FontSettings.FontStreams;
        image.CustomFont.FontCollection = presentation.FontSettings.PrivateFonts;
      }
      if (this._settings == null || this._settings.SlidesPerPage == SlidesPerPage.One || this._settings.PublishOptions != PublishOptions.Handouts)
        graphics1.DrawImage((PdfImage) image, new RectangleF(this._x, this._y, this._width, this._height));
      else
        graphics1.DrawImage((PdfImage) image, new RectangleF(this._x + margins.Left, this._y + margins.Top, this._width, this._height));
    }
  }

  public static PdfDocument Convert(ISlide slide)
  {
    PdfDocument pdfDocument = new PdfDocument();
    PresentationToPdfConverterSettings settings = new PresentationToPdfConverterSettings();
    if (Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.IsAzureCompatible)
      settings.EnablePortableRendering = true;
    using (Syncfusion.PresentationToPdfConverter.PresentationToPdfConverter presentationToPdfConverter = new Syncfusion.PresentationToPdfConverter.PresentationToPdfConverter(settings))
    {
      Slide slide1 = slide as Slide;
      presentationToPdfConverter.AddDocumentProperties(pdfDocument, slide1.Presentation.BuiltInDocumentProperties as BuiltInDocumentProperties);
      presentationToPdfConverter.AddDocumentProperties(pdfDocument, slide1.Presentation.CustomDocumentProperties as CustomDocumentProperties);
      PdfPageSettings pageSettings = pdfDocument.PageSettings;
      ISlideSize slideSize = slide1.Presentation.SlideSize;
      presentationToPdfConverter._width = pageSettings.Width = (float) slideSize.Width;
      presentationToPdfConverter._height = pageSettings.Height = (float) slideSize.Height;
      if (settings.EnablePortableRendering)
        (slide as Slide).EnablePortableRendering(settings.EnablePortableRendering);
      presentationToPdfConverter.DrawSlide(pdfDocument, slide, (IPresentation) slide1.Presentation);
    }
    return pdfDocument;
  }

  private void AddDocumentProperties(
    PdfDocument pdfDocument,
    CustomDocumentProperties customDocumentProperties)
  {
    CustomSchema customSchema = new CustomSchema(pdfDocument.DocumentInformation.XmpMetadata, "custom", "http://www.syncfusion.com");
    foreach (DocumentPropertyImpl documentProperty in customDocumentProperties.GetDocumentPropertyList())
      customSchema[documentProperty.Name] = documentProperty.Value.ToString();
  }

  private void AddDocumentProperties(
    PdfDocument pdfDocument,
    BuiltInDocumentProperties docProperties)
  {
    PdfDocumentInformation documentInformation = pdfDocument.DocumentInformation;
    documentInformation.Author = docProperties.Author;
    documentInformation.CreationDate = docProperties.CreationDate;
    documentInformation.Creator = docProperties.Company;
    documentInformation.Keywords = docProperties.Keywords;
    documentInformation.Producer = docProperties.Company;
    documentInformation.Subject = docProperties.Subject;
    documentInformation.Title = docProperties.Title;
    documentInformation.ModificationDate = docProperties.LastSaveDate;
  }

  private void DrawSlide(PdfDocument pdfDocument, ISlide slide, IPresentation presentation)
  {
    PdfPage page;
    if (this._settings == null || this._settings.SlidesPerPage == SlidesPerPage.One || this._isNewPage)
    {
      page = pdfDocument.Pages.Add();
      this.InitializePageMargins(page.Section.PageSettings.Margins);
      this.DrawHandoutMaster(pdfDocument, page, (slide as Slide).Presentation.GetHandoutMaster());
    }
    else
      page = pdfDocument.Pages[pdfDocument.Pages.Count - 1];
    PdfMargins margins = pdfDocument.PageSettings.Margins;
    this.CalculateImagePosition(margins);
    PdfGraphics graphics1 = page.Graphics;
    if (this._settings != null && this._settings.EnablePortableRendering)
    {
      graphics1.IsDirectPDF = true;
      graphics1.OptimizeIdenticalImages = this._settings.OptimizeIdenticalImages;
      graphics1.NativeGraphics = System.Drawing.Graphics.FromImage((System.Drawing.Image) new Bitmap(1, 1));
      MemoryStream stream = new MemoryStream();
      System.Drawing.Graphics graphics2 = System.Drawing.Graphics.FromImage(new Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter().CreateImage(slide as Slide, ImageType.Metafile, stream));
      graphics2.PageUnit = GraphicsUnit.Point;
      Syncfusion.Presentation.Presentation presentation1 = (Syncfusion.Presentation.Presentation) presentation;
      presentation1.Graphics = graphics2;
      PDFRenderer pdfRenderer = new PDFRenderer(graphics1, graphics2, GraphicsUnit.Point, pdfDocument);
      presentation1.Renderer = (RendererBase) pdfRenderer;
      pdfRenderer.EmbedFonts = this._settings.EmbedFonts;
      pdfRenderer.EmbedCompleteFonts = this._settings.EmbedCompleteFonts;
      pdfRenderer.ImageQuality = this._settings.ImageQuality;
      pdfRenderer.FontStreams = presentation.FontSettings.FontStreams;
      pdfRenderer.PrivateFonts = presentation.FontSettings.PrivateFonts;
      if (presentation.FontSettings.FallbackFonts.Count > 0)
        pdfRenderer.FallbackFonts = presentation.FontSettings.FallbackFonts;
      (slide as Slide).Layout();
      pdfRenderer.DrawSlide((Slide) slide);
      this.AddHyperlink(pdfDocument, page, ((Slide) slide).UriHyperlinks);
      graphics1.NativeGraphics.Dispose();
      graphics1.IsDirectPDF = false;
    }
    else
    {
      System.Drawing.Image image1 = slide.ConvertToImage(ImageType.Metafile);
      this.AddHyperlink(pdfDocument, page, ((Slide) slide).UriHyperlinks);
      PdfMetafile image2 = new PdfMetafile(image1 as Metafile);
      if (this._settings != null)
      {
        image2.IsEmbedFonts = this._settings.EmbedFonts;
        image2.IsEmbedCompleteFonts = this._settings.EmbedCompleteFonts;
        if (this._settings._imageResolution > 0)
          image2.ImageResolution = this._settings._imageResolution;
        else
          image2.Quality = (long) this._settings.ImageQuality;
        if (this._settings.OptimizeIdenticalImages)
        {
          image2.OptimizeIdenticalImages = this._settings.OptimizeIdenticalImages;
          image2.Document = pdfDocument;
        }
      }
      if (presentation.FontSettings.PrivateFonts != null && presentation.FontSettings.FontStreams != null)
      {
        image2.CustomFont = new CustomFont();
        image2.CustomFont.EmbeddedFonts = presentation.FontSettings.FontStreams;
        image2.CustomFont.FontCollection = presentation.FontSettings.PrivateFonts;
      }
      if (this._settings == null || this._settings.PublishOptions == PublishOptions.Slides || this._settings.SlidesPerPage == SlidesPerPage.One)
        graphics1.DrawImage((PdfImage) image2, new RectangleF(this._x, this._y, this._width, this._height));
      else
        graphics1.DrawImage((PdfImage) image2, new RectangleF(this._x + margins.Left, this._y + margins.Top, this._width, this._height));
    }
  }

  private void AddHyperlink(
    PdfDocument pdfDocument,
    PdfPage page,
    List<Dictionary<string, RectangleF>> hyperlinks)
  {
    for (int index1 = 0; index1 < hyperlinks.Count; ++index1)
    {
      foreach (KeyValuePair<string, RectangleF> keyValuePair in hyperlinks[index1])
      {
        RectangleF rectangle = keyValuePair.Value;
        string key = keyValuePair.Key;
        if (!key.Equals(string.Empty))
        {
          if (page == null)
          {
            PdfPage page1 = (PdfPage) null;
            string[] strArray = key.Split(' ');
            int index2 = int.Parse(strArray[1]);
            int index3 = int.Parse(strArray[3]);
            if (index2 < pdfDocument.Pages.count)
              page = pdfDocument.Pages[index2];
            if (index3 < pdfDocument.Pages.count)
              page1 = pdfDocument.Pages[index3];
            if (page != null && page1 != null)
            {
              PdfDocumentLinkAnnotation annotation = new PdfDocumentLinkAnnotation(rectangle);
              annotation.Border.Width = 0.0f;
              annotation.Destination = new PdfDestination((PdfPageBase) page1);
              page.Annotations.Add((PdfAnnotation) annotation);
              page = (PdfPage) null;
            }
          }
          else
          {
            PdfUriAnnotation annotation = new PdfUriAnnotation(rectangle);
            annotation.Uri = key;
            annotation.Border.Width = 0.0f;
            page.Annotations.Add((PdfAnnotation) annotation);
          }
        }
      }
    }
  }

  private void DrawHandoutMaster(
    PdfDocument pdfDocument,
    PdfPage page,
    HandoutMaster handoutMaster)
  {
    if (this._settings == null || this._settings.PublishOptions != PublishOptions.Handouts || this._settings.SlidesPerPage == SlidesPerPage.One)
      return;
    if (handoutMaster != null)
    {
      MemoryStream stream = new MemoryStream();
      if (this._settings.EnablePortableRendering)
        return;
      using (this.GdiRenderer.Graphics = handoutMaster.Presentation.Graphics = System.Drawing.Graphics.FromImage(this.CreateImage(page.Size, stream)))
      {
        handoutMaster.Presentation.Renderer = (RendererBase) this.GdiRenderer;
        handoutMaster.Presentation.Graphics.PageUnit = GraphicsUnit.Point;
        GraphicsPath path = new GraphicsPath();
        path.AddRectangle(new RectangleF(0.0f, 0.0f, page.Size.Width, page.Size.Height));
        this.GdiRenderer.FillBackground((IShape) null, path, handoutMaster.Background.Fill);
        foreach (IShape shape in (IEnumerable<ISlideItem>) handoutMaster.Shapes)
        {
          Shape shapeImpl = shape as Shape;
          shapeImpl.Layout();
          if (shapeImpl.PlaceholderFormat != null)
          {
            switch (shapeImpl.PlaceholderFormat.Type)
            {
              case PlaceholderType.SlideNumber:
                ITextPart textPart = shapeImpl.TextBody.Paragraphs[0].TextParts[0];
                string text = textPart.Text;
                textPart.Text = "  " + pdfDocument.Pages.Count.ToString((IFormatProvider) CultureInfo.InvariantCulture);
                this.GdiRenderer.DrawShape(shapeImpl);
                textPart.Text = text;
                break;
              case PlaceholderType.Header:
                this.GdiRenderer.DrawShape(shapeImpl);
                break;
              case PlaceholderType.Footer:
                this.GdiRenderer.DrawShape(shapeImpl);
                break;
              case PlaceholderType.Date:
                this.GdiRenderer.DrawShape(shapeImpl);
                break;
            }
          }
          else
            this.GdiRenderer.DrawShape(shapeImpl);
        }
      }
      stream.Position = 0L;
      PdfMetafile pdfMetafile = new PdfMetafile((Stream) stream);
      pdfMetafile.IsEmbedFonts = this._settings.EmbedFonts;
      pdfMetafile.IsEmbedCompleteFonts = this._settings.EmbedCompleteFonts;
      PdfGraphics graphics = page.Graphics;
      PdfMetafile image = pdfMetafile;
      SizeF size = page.Size;
      double width = (double) size.Width;
      size = page.Size;
      double height = (double) size.Height;
      graphics.DrawImage((PdfImage) image, 0.0f, 0.0f, (float) width, (float) height);
      stream.Dispose();
      pdfMetafile.Dispose();
    }
    else
    {
      PdfFont font1 = (PdfFont) new PdfTrueTypeFont(new Font("Calibri", 12f));
      PdfStringFormat pdfStringFormat = new PdfStringFormat();
      pdfStringFormat.Alignment = PdfTextAlignment.Right;
      pdfStringFormat.LineAlignment = PdfVerticalAlignment.Top;
      RectangleF layoutRectangle1 = new RectangleF(300f, 5f, 234f, 36f);
      PdfGraphics graphics = page.Graphics;
      DateTime dateTime = DateTime.Now;
      dateTime = dateTime.Date;
      string shortDateString = dateTime.ToShortDateString();
      PdfFont font2 = font1;
      PdfBrush black = PdfBrushes.Black;
      RectangleF layoutRectangle2 = layoutRectangle1;
      PdfStringFormat format1 = pdfStringFormat;
      graphics.DrawString(shortDateString, font2, black, layoutRectangle2, format1);
      PdfStringFormat format2 = new PdfStringFormat();
      format2.Alignment = PdfTextAlignment.Right;
      format2.LineAlignment = PdfVerticalAlignment.Bottom;
      layoutRectangle1 = new RectangleF(300f, 683f, 234f, 36f);
      page.Graphics.DrawString(pdfDocument.Pages.Count.ToString((IFormatProvider) CultureInfo.InvariantCulture), font1, PdfBrushes.Black, layoutRectangle1, format2);
    }
  }

  private void CalculateImagePosition(PdfMargins margins)
  {
    if (this._settings == null || this._settings.SlidesPerPage == SlidesPerPage.One || this._settings.PublishOptions != PublishOptions.Handouts)
      return;
    switch (this._settings.SlidesPerPage)
    {
      case SlidesPerPage.Six:
        int num1 = (int) ((double) this._slideNumber % 6.0);
        if (num1 != 0)
        {
          if (num1 % 2 != 0)
          {
            this._x = 0.0f;
            this._y = (float) ((double) this._height * (double) ((num1 + 1) / 2 - 1) + (double) this._slideSpace * (double) num1);
          }
          else
          {
            this._x = this._width + margins.Left / 2f;
            this._y = (float) ((double) this._height * (double) (num1 / 2 - 1) + (double) this._slideSpace * (double) (num1 - 1));
          }
          this._isNewPage = false;
          break;
        }
        this._x = this._width + margins.Left / 2f;
        this._y = (float) ((double) this._height * 2.0 + (double) this._slideSpace * 5.0);
        this._isNewPage = true;
        break;
      case SlidesPerPage.Two:
        if ((double) this._slideNumber % 2.0 != 0.0)
        {
          this._x = this._y = this._slideSpace;
          this._isNewPage = false;
          break;
        }
        this._y += this._height + this._slideSpace * 2f;
        this._isNewPage = true;
        break;
      case SlidesPerPage.Three:
        this._x = 0.0f;
        int num2 = (int) ((double) this._slideNumber % 3.0);
        switch (num2)
        {
          case 0:
            this._y = (float) ((double) this._height * 2.0 + (double) this._slideSpace * 5.0);
            this._isNewPage = true;
            return;
          case 1:
            this._y = this._slideSpace;
            break;
          default:
            this._y = (float) ((double) this._height * (double) (num2 - 1) + (double) this._slideSpace * 3.0);
            break;
        }
        this._isNewPage = false;
        break;
      case SlidesPerPage.Four:
        int num3 = (int) ((double) this._slideNumber % 4.0);
        if (num3 != 0)
        {
          if (num3 % 2 != 0)
          {
            this._x = 0.0f;
            this._y = num3 != 1 ? this._height + this._slideSpace * 3f : this._slideSpace;
          }
          else
          {
            this._x = this._width + margins.Left / 2f;
            this._y = num3 != 2 ? this._height + this._slideSpace * 3f : this._slideSpace;
          }
          this._isNewPage = false;
          break;
        }
        this._x = this._width + margins.Left / 2f;
        this._y = this._height + this._slideSpace * 3f;
        this._isNewPage = true;
        break;
      case SlidesPerPage.Nine:
        int num4 = (int) ((double) this._slideNumber % 9.0);
        if (num4 != 0)
        {
          if (num4 % 3 == 1)
          {
            this._x = 0.0f;
            this._y = (float) ((double) this._height * (double) ((num4 + 2) / 3 - 1) + (double) this._slideSpace * (double) ((num4 - 1) / 3 * 2 + 1));
          }
          else if (num4 % 3 == 2)
          {
            this._x = this._width + margins.Left / 2f;
            this._y = (float) ((double) this._height * (double) ((num4 + 1) / 3 - 1) + (double) this._slideSpace * (double) ((num4 - 1) / 3 * 2 + 1));
          }
          else
          {
            this._x = (float) (((double) this._width + (double) margins.Left / 2.0) * 2.0);
            this._y = (float) ((double) this._height * (double) (num4 / 3 - 1) + (double) this._slideSpace * (double) ((num4 - 1) / 3 * 2 + 1));
          }
          this._isNewPage = false;
          break;
        }
        this._x = (float) (((double) this._width + (double) margins.Left / 2.0) * 2.0);
        this._y = (float) ((double) this._height * 2.0 + (double) this._slideSpace * 5.0);
        this._isNewPage = true;
        break;
    }
  }

  private System.Drawing.Image CreateImage(SizeF size, MemoryStream stream)
  {
    int pixel1 = Helper.PointToPixel((double) size.Width);
    int pixel2 = Helper.PointToPixel((double) size.Height);
    System.Drawing.Image image = (System.Drawing.Image) null;
    using (Bitmap bitmap = new Bitmap(pixel1, pixel2))
    {
      using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage((System.Drawing.Image) bitmap))
      {
        bitmap.SetResolution(graphics.DpiX, graphics.DpiY);
        IntPtr hdc = graphics.GetHdc();
        Rectangle frameRect = new Rectangle(0, 0, pixel1, pixel2);
        image = (System.Drawing.Image) new Metafile((Stream) stream, hdc, frameRect, MetafileFrameUnit.Pixel, EmfType.EmfPlusOnly);
        graphics.ReleaseHdc();
      }
    }
    return image;
  }

  private void InitializePageMargins(PdfMargins margin)
  {
    margin.Left = 0.0f;
    margin.Right = 0.0f;
    margin.Top = 0.0f;
    margin.Bottom = 0.0f;
  }

  internal void Close()
  {
    if (this._gdiRenderer != null)
      this._gdiRenderer = (GDIRenderer) null;
    if (this._settings == null)
      return;
    this._settings = (PresentationToPdfConverterSettings) null;
  }

  public void Dispose() => this.Close();
}
