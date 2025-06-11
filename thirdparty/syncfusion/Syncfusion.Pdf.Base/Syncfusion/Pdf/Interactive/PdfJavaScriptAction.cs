// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfJavaScriptAction
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfJavaScriptAction : PdfAction
{
  private string m_javaScript = string.Empty;

  public PdfJavaScriptAction(string javaScript)
  {
    this.JavaScript = javaScript != null ? javaScript : throw new ArgumentNullException(nameof (javaScript));
  }

  public string JavaScript
  {
    get => this.m_javaScript;
    set
    {
      if (!(this.m_javaScript != value))
        return;
      this.m_javaScript = value;
      this.Dictionary.SetString("JS", this.m_javaScript);
    }
  }

  protected override void Initialize()
  {
    base.Initialize();
    this.Dictionary.SetProperty("S", (IPdfPrimitive) new PdfName("JavaScript"));
    this.Dictionary.SetProperty("JS", (IPdfPrimitive) new PdfString(this.m_javaScript));
  }
}
