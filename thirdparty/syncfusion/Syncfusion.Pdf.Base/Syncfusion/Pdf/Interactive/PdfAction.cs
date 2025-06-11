// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfAction
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public abstract class PdfAction : IPdfWrapper
{
  private PdfAction m_action;
  private PdfDictionary m_dictionary = new PdfDictionary();

  protected PdfAction() => this.Initialize();

  public PdfAction Next
  {
    get => this.m_action;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (Next));
      if (this.m_action == value)
        return;
      this.m_action = value;
      this.Dictionary.SetArray(nameof (Next), (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_action));
    }
  }

  internal PdfDictionary Dictionary => this.m_dictionary;

  protected virtual void Initialize()
  {
    this.Dictionary.SetProperty("Type", (IPdfPrimitive) new PdfName("Action"));
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_dictionary;
}
