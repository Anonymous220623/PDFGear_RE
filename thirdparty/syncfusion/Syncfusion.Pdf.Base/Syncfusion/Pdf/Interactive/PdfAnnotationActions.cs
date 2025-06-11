// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfAnnotationActions
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfAnnotationActions : IPdfWrapper
{
  private PdfAction m_mouseEnter;
  private PdfAction m_mouseLeave;
  private PdfAction m_mouseDown;
  private PdfAction m_mouseUp;
  private PdfAction m_gotFocus;
  private PdfAction m_lostFocus;
  private PdfDictionary m_dictionary = new PdfDictionary();

  public PdfAction MouseEnter
  {
    get => this.m_mouseEnter;
    set
    {
      if (this.m_mouseEnter == value)
        return;
      this.m_mouseEnter = value;
      this.m_dictionary.SetProperty("E", (IPdfWrapper) this.m_mouseEnter);
    }
  }

  public PdfAction MouseLeave
  {
    get => this.m_mouseLeave;
    set
    {
      if (this.m_mouseLeave == value)
        return;
      this.m_mouseLeave = value;
      this.m_dictionary.SetProperty("X", (IPdfWrapper) this.m_mouseLeave);
    }
  }

  public PdfAction MouseDown
  {
    get => this.m_mouseDown;
    set
    {
      if (this.m_mouseDown == value)
        return;
      this.m_mouseDown = value;
      this.m_dictionary.SetProperty("D", (IPdfWrapper) this.m_mouseDown);
    }
  }

  public PdfAction MouseUp
  {
    get => this.m_mouseUp;
    set
    {
      if (this.m_mouseUp == value)
        return;
      this.m_mouseUp = value;
      this.m_dictionary.SetProperty("U", (IPdfWrapper) this.m_mouseUp);
    }
  }

  public PdfAction GotFocus
  {
    get => this.m_gotFocus;
    set
    {
      if (this.m_gotFocus == value)
        return;
      this.m_gotFocus = value;
      this.m_dictionary.SetProperty("Fo", (IPdfWrapper) this.m_gotFocus);
    }
  }

  public PdfAction LostFocus
  {
    get => this.m_lostFocus;
    set
    {
      if (this.m_lostFocus == value)
        return;
      this.m_lostFocus = value;
      this.m_dictionary.SetProperty("Bl", (IPdfWrapper) this.m_lostFocus);
    }
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_dictionary;
}
