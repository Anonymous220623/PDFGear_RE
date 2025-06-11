// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.PdfUsedFont
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

public class PdfUsedFont
{
  private string m_name;
  private float m_size;
  private PdfFontStyle m_style;
  private PdfFontType m_type;
  private PdfFont m_internalFont;
  private PdfLoadedPage m_lpage;
  private string m_actualFontName;
  private PdfDictionary resourceDictionary;
  private bool replace;
  private bool xobject;

  internal PdfFont InternalFont => this.m_internalFont;

  public string Name => this.m_name;

  public float Size => this.m_size;

  public PdfFontStyle Style => this.m_style;

  public PdfFontType Type => this.m_type;

  internal string ActualFontName
  {
    get
    {
      if (this.m_actualFontName == null)
        this.m_actualFontName = this.GetActualFontName();
      return this.GetActualFontName();
    }
  }

  public PdfUsedFont(PdfFont font, PdfLoadedPage page) => this.InitializeInternals(font, page);

  public void Replace(PdfFont fontToReplace)
  {
    this.CheckPreambula();
    PdfLoadedPage lpage = this.m_lpage;
    PdfFont font1 = fontToReplace;
    if (this.m_lpage.Document is PdfLoadedDocument document)
    {
      foreach (PdfLoadedPage page in document.Pages)
      {
        this.m_lpage = page;
        PdfResources resources = this.m_lpage.GetResources();
        if (fontToReplace is PdfTrueTypeFont)
        {
          Font font2 = (fontToReplace as PdfTrueTypeFont).Font;
          string fontFile = (fontToReplace as PdfTrueTypeFont).FontFile;
          font1 = font2 != null || fontFile == null ? (PdfFont) new PdfTrueTypeFont(font2, true, true, false) : (PdfFont) new PdfTrueTypeFont(fontFile, fontToReplace.Size, true);
        }
        IPdfPrimitive xobject = this.m_lpage.GetXObject(resources);
        if (xobject != null && xobject is PdfDictionary)
        {
          Dictionary<PdfName, IPdfPrimitive> items = ((PdfDictionary) xobject).Items;
          if (items != null)
          {
            foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in items)
            {
              if ((object) (keyValuePair.Value as PdfReferenceHolder) != null)
              {
                PdfStream pdfStream = PdfCrossTable.Dereference(((PdfReferenceHolder) keyValuePair.Value).Object) as PdfStream;
                PdfDictionary pdfDictionary = (PdfDictionary) null;
                if (pdfStream != null)
                  pdfDictionary = PdfCrossTable.Dereference((IPdfPrimitive) pdfStream) as PdfDictionary;
                if (pdfDictionary != null && pdfDictionary.ContainsKey("Resources"))
                  this.resourceDictionary = PdfCrossTable.Dereference(pdfDictionary["Resources"]) as PdfDictionary;
              }
            }
          }
        }
        if (resources != null && font1 != null)
        {
          PdfName name = resources.GetName(this.ActualFontName);
          if (this.xobject && this.resourceDictionary != null)
            resources = new PdfResources(this.resourceDictionary);
          resources.RemoveFont(name.Value);
          resources.Add(font1, name);
          this.xobject = false;
        }
      }
    }
    this.m_lpage = lpage;
  }

  private void InitializeInternals(PdfFont font, PdfLoadedPage page)
  {
    this.m_lpage = page;
    this.m_internalFont = font;
    this.m_name = font.Name;
    this.m_size = font.Size;
    this.m_style = font.Style;
    switch (font)
    {
      case PdfStandardFont _:
        this.m_type = PdfFontType.Standard;
        return;
      case PdfTrueTypeFont _:
        if (!(font as PdfTrueTypeFont).Unicode)
        {
          this.m_type = PdfFontType.TrueType;
          return;
        }
        break;
    }
    this.m_type = PdfFontType.TrueTypeEmbedded;
  }

  private string GetActualFontName()
  {
    string empty1 = string.Empty;
    string empty2 = string.Empty;
    PdfResources resources = this.m_lpage.GetResources();
    if (resources.ContainsKey("Font"))
    {
      foreach (KeyValuePair<IPdfPrimitive, PdfName> name in resources.ObtainNames())
      {
        string key = (name.Key as PdfDictionary).ContainsKey("BaseFont") ? "BaseFont" : "Name";
        string str = (name.Key as PdfDictionary)[key].ToString().TrimStart('/');
        if (str == this.Name || str == this.InternalFont.InternalFontName)
        {
          empty1 = name.Value.ToString();
          break;
        }
      }
    }
    PdfDictionary baseDictionary = (PdfDictionary) null;
    IPdfPrimitive xobject = this.m_lpage.GetXObject(resources);
    if (xobject != null && xobject is PdfDictionary)
    {
      Dictionary<PdfName, IPdfPrimitive> items = ((PdfDictionary) xobject).Items;
      if (items != null)
      {
        foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in items)
        {
          if ((object) (keyValuePair.Value as PdfReferenceHolder) != null)
          {
            PdfStream pdfStream = PdfCrossTable.Dereference(((PdfReferenceHolder) keyValuePair.Value).Object) as PdfStream;
            PdfDictionary pdfDictionary = PdfCrossTable.Dereference((IPdfPrimitive) pdfStream) as PdfDictionary;
            if (pdfStream != null)
              pdfDictionary = PdfCrossTable.Dereference((IPdfPrimitive) pdfStream) as PdfDictionary;
            if (pdfDictionary != null && pdfDictionary.ContainsKey("Resources"))
              baseDictionary = PdfCrossTable.Dereference(pdfDictionary["Resources"]) as PdfDictionary;
          }
        }
      }
    }
    if (baseDictionary != null && baseDictionary.ContainsKey("Font"))
    {
      foreach (KeyValuePair<IPdfPrimitive, PdfName> name in new PdfResources(baseDictionary).ObtainNames())
      {
        string str = (name.Key as PdfDictionary)["BaseFont"].ToString().TrimStart('/');
        if (str == this.Name || str == this.InternalFont.InternalFontName)
        {
          empty1 = name.Value.ToString();
          this.xobject = true;
          break;
        }
      }
    }
    return empty1.TrimStart('/');
  }

  private void CheckPreambula()
  {
    if (this.Type == PdfFontType.TrueTypeEmbedded)
      throw new PdfException("Can't replace font,  the font is already embedded");
  }
}
