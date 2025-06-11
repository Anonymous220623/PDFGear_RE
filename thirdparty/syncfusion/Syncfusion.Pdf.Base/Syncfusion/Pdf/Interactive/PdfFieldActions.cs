// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfFieldActions
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfFieldActions : IPdfWrapper
{
  private PdfAnnotationActions m_annotationActions;
  private PdfDictionary m_dictionary = new PdfDictionary();
  private PdfJavaScriptAction m_keyPressed;
  private PdfJavaScriptAction m_format;
  private PdfJavaScriptAction m_validate;
  private PdfJavaScriptAction m_calculate;

  public PdfFieldActions(PdfAnnotationActions annotationActions)
  {
    this.m_annotationActions = annotationActions != null ? annotationActions : throw new ArgumentNullException("annotationActrions");
  }

  public PdfJavaScriptAction KeyPressed
  {
    get => this.m_keyPressed;
    set
    {
      if (this.m_keyPressed == value)
        return;
      this.m_keyPressed = value;
      this.m_dictionary.SetProperty("K", (IPdfWrapper) this.m_keyPressed);
    }
  }

  public PdfJavaScriptAction Format
  {
    get => this.m_format;
    set
    {
      if (this.m_format == value)
        return;
      this.m_format = value;
      this.m_dictionary.SetProperty("F", (IPdfWrapper) this.m_format);
    }
  }

  public PdfJavaScriptAction Validate
  {
    get => this.m_validate;
    set
    {
      if (this.m_validate == value)
        return;
      this.m_validate = value;
      this.m_dictionary.SetProperty("V", (IPdfWrapper) this.m_validate);
    }
  }

  public PdfJavaScriptAction Calculate
  {
    get => this.m_calculate;
    set
    {
      if (this.m_calculate == value)
        return;
      this.m_calculate = value;
      this.m_dictionary.SetProperty("C", (IPdfWrapper) this.m_calculate);
    }
  }

  public PdfAction MouseEnter
  {
    get => this.m_annotationActions.MouseEnter;
    set => this.m_annotationActions.MouseEnter = value;
  }

  public PdfAction MouseLeave
  {
    get => this.m_annotationActions.MouseLeave;
    set => this.m_annotationActions.MouseLeave = value;
  }

  public PdfAction MouseUp
  {
    get => this.m_annotationActions.MouseUp;
    set => this.m_annotationActions.MouseUp = value;
  }

  public PdfAction MouseDown
  {
    get => this.m_annotationActions.MouseDown;
    set => this.m_annotationActions.MouseDown = value;
  }

  public PdfAction GotFocus
  {
    get => this.m_annotationActions.GotFocus;
    set => this.m_annotationActions.GotFocus = value;
  }

  public PdfAction LostFocus
  {
    get => this.m_annotationActions.LostFocus;
    set => this.m_annotationActions.LostFocus = value;
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_dictionary;
}
