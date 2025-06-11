// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.XPSToPdfConverter
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.XPS;

public class XPSToPdfConverter
{
  private PdfUnitConvertor m_unitConvertor;
  private bool isXpsFile = true;
  private XPSToPdfConverterSettings m_settings = new XPSToPdfConverterSettings();

  public XPSToPdfConverterSettings Settings
  {
    get => this.m_settings;
    set => this.m_settings = value;
  }

  public XPSToPdfConverter() => this.m_unitConvertor = new PdfUnitConvertor(96f);

  public XPSToPdfConverter(XPSToPdfConverterSettings settings)
  {
    this.m_unitConvertor = new PdfUnitConvertor(96f);
    this.Settings = settings;
  }

  public PdfDocument Convert(string fileName)
  {
    PdfDocument pdfDocument = new PdfDocument();
    pdfDocument.PageSettings.Margins.All = 0.0f;
    if (System.IO.Path.GetExtension(fileName) == ".oxps")
      this.isXpsFile = false;
    if (this.isXpsFile)
    {
      using (XPSDocumentReader reader = new XPSDocumentReader(fileName))
      {
        this.isXpsFile = reader.Read();
        if (this.isXpsFile)
        {
          foreach (FixedPage page in reader.Pages)
          {
            PdfSection pdfSection = pdfDocument.Sections.Add();
            pdfSection.PageSettings.Size = new SizeF(this.PixelsToPoints(page.Width), this.PixelsToPoints(page.Height));
            if (page.Width > page.Height)
              pdfSection.PageSettings.Orientation = PdfPageOrientation.Landscape;
            using (XPSRenderer renderer = new XPSRenderer(pdfSection.Pages.Add(), reader))
            {
              renderer.Document = pdfDocument;
              renderer.EmbedFont = this.m_settings;
              using (XPSParser xpsParser = new XPSParser(page, renderer))
                xpsParser.Enumerate();
            }
          }
        }
      }
    }
    if (!this.isXpsFile)
    {
      using (OXPSDocumentReader reader = new OXPSDocumentReader(fileName))
      {
        reader.Read();
        foreach (OXPSFixedPage page in reader.Pages)
        {
          PdfSection pdfSection = pdfDocument.Sections.Add();
          pdfSection.PageSettings.Size = new SizeF(this.PixelsToPoints(page.Width), this.PixelsToPoints(page.Height));
          if (page.Width > page.Height)
            pdfSection.PageSettings.Orientation = PdfPageOrientation.Landscape;
          using (OXPSRenderer renderer = new OXPSRenderer(pdfSection.Pages.Add(), reader))
          {
            renderer.EmbedFont = this.m_settings;
            using (OXPSParser oxpsParser = new OXPSParser(page, renderer))
              oxpsParser.Enumerate();
          }
        }
      }
    }
    return pdfDocument;
  }

  public PdfDocument Convert(Stream file)
  {
    PdfDocument pdfDocument = new PdfDocument();
    pdfDocument.PageSettings.Margins.All = 0.0f;
    using (XPSDocumentReader reader = new XPSDocumentReader(file))
    {
      this.isXpsFile = reader.Read();
      if (this.isXpsFile)
      {
        foreach (FixedPage page in reader.Pages)
        {
          PdfSection pdfSection = pdfDocument.Sections.Add();
          pdfSection.PageSettings.Size = new SizeF(this.PixelsToPoints(page.Width), this.PixelsToPoints(page.Height));
          if (page.Width > page.Height)
            pdfSection.PageSettings.Orientation = PdfPageOrientation.Landscape;
          using (XPSRenderer renderer = new XPSRenderer(pdfSection.Pages.Add(), reader))
          {
            renderer.Document = pdfDocument;
            using (XPSParser xpsParser = new XPSParser(page, renderer))
              xpsParser.Enumerate();
          }
        }
      }
    }
    if (!this.isXpsFile)
    {
      using (OXPSDocumentReader reader = new OXPSDocumentReader(file))
      {
        reader.Read();
        foreach (OXPSFixedPage page in reader.Pages)
        {
          PdfSection pdfSection = pdfDocument.Sections.Add();
          pdfSection.PageSettings.Size = new SizeF(this.PixelsToPoints(page.Width), this.PixelsToPoints(page.Height));
          if (page.Width > page.Height)
            pdfSection.PageSettings.Orientation = PdfPageOrientation.Landscape;
          using (OXPSRenderer renderer = new OXPSRenderer(pdfSection.Pages.Add(), reader))
          {
            renderer.EmbedFont = this.m_settings;
            using (OXPSParser oxpsParser = new OXPSParser(page, renderer))
              oxpsParser.Enumerate();
          }
        }
      }
    }
    return pdfDocument;
  }

  public PdfDocument Convert(byte[] file)
  {
    PdfDocument pdfDocument = new PdfDocument();
    pdfDocument.PageSettings.Margins.All = 0.0f;
    Stream stream = (Stream) new MemoryStream(file);
    using (XPSDocumentReader reader = new XPSDocumentReader(stream))
    {
      this.isXpsFile = reader.Read();
      if (this.isXpsFile)
      {
        foreach (FixedPage page in reader.Pages)
        {
          PdfSection pdfSection = pdfDocument.Sections.Add();
          pdfSection.PageSettings.Size = new SizeF(this.PixelsToPoints(page.Width), this.PixelsToPoints(page.Height));
          if (page.Width > page.Height)
            pdfSection.PageSettings.Orientation = PdfPageOrientation.Landscape;
          using (XPSRenderer renderer = new XPSRenderer(pdfSection.Pages.Add(), reader))
          {
            renderer.Document = pdfDocument;
            using (XPSParser xpsParser = new XPSParser(page, renderer))
              xpsParser.Enumerate();
          }
        }
      }
    }
    if (!this.isXpsFile)
    {
      using (OXPSDocumentReader reader = new OXPSDocumentReader(stream))
      {
        reader.Read();
        foreach (OXPSFixedPage page in reader.Pages)
        {
          PdfSection pdfSection = pdfDocument.Sections.Add();
          pdfSection.PageSettings.Size = new SizeF(this.PixelsToPoints(page.Width), this.PixelsToPoints(page.Height));
          if (page.Width > page.Height)
            pdfSection.PageSettings.Orientation = PdfPageOrientation.Landscape;
          using (OXPSRenderer renderer = new OXPSRenderer(pdfSection.Pages.Add(), reader))
          {
            using (OXPSParser oxpsParser = new OXPSParser(page, renderer))
              oxpsParser.Enumerate();
          }
        }
      }
    }
    return pdfDocument;
  }

  private float PixelsToPoints(double value)
  {
    return this.m_unitConvertor.ConvertFromPixels((float) value, PdfGraphicsUnit.Point);
  }
}
