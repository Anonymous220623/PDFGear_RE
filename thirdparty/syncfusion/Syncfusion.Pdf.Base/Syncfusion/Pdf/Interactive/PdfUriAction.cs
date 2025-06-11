// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfUriAction
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfUriAction : PdfAction
{
  private string m_uri = string.Empty;

  public PdfUriAction()
  {
  }

  public PdfUriAction(string uri) => this.Uri = uri;

  public string Uri
  {
    get => this.m_uri;
    set
    {
      if (value == null)
        throw new ArgumentNullException("uri");
      if (!(this.m_uri != value))
        return;
      this.m_uri = value;
      this.Dictionary.SetString("URI", this.m_uri);
    }
  }

  protected override void Initialize()
  {
    base.Initialize();
    this.Dictionary.SetProperty("S", (IPdfPrimitive) new PdfName("URI"));
  }
}
