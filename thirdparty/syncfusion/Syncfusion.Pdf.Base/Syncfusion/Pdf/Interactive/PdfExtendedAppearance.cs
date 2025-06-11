// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfExtendedAppearance
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfExtendedAppearance : IPdfWrapper
{
  private PdfAppearanceState m_normal;
  private PdfAppearanceState m_pressed;
  private PdfAppearanceState m_mouseHover;
  private PdfDictionary m_dictionary = new PdfDictionary();

  public PdfAppearanceState Normal
  {
    get
    {
      if (this.m_normal == null)
      {
        this.m_normal = new PdfAppearanceState();
        this.m_dictionary.SetProperty("N", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_normal));
      }
      return this.m_normal;
    }
  }

  public PdfAppearanceState MouseHover
  {
    get
    {
      if (this.m_mouseHover == null)
      {
        this.m_mouseHover = new PdfAppearanceState();
        this.m_dictionary.SetProperty("R", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_mouseHover));
      }
      return this.m_mouseHover;
    }
  }

  public PdfAppearanceState Pressed
  {
    get
    {
      if (this.m_pressed == null)
      {
        this.m_pressed = new PdfAppearanceState();
        this.m_dictionary.SetProperty("D", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_pressed));
      }
      return this.m_pressed;
    }
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_dictionary;
}
