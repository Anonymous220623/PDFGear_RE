// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfFormAction
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfFormAction : PdfAction
{
  private bool m_include;
  private PdfFieldCollection m_fields;

  public virtual bool Include
  {
    get => this.m_include;
    set => this.m_include = value;
  }

  public PdfFieldCollection Fields
  {
    get
    {
      if (this.m_fields == null)
      {
        this.m_fields = new PdfFieldCollection();
        this.Dictionary.SetProperty(nameof (Fields), (IPdfWrapper) this.m_fields);
      }
      return this.m_fields;
    }
  }
}
