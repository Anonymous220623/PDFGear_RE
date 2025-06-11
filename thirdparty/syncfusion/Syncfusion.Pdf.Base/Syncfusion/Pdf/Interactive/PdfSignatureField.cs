// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfSignatureField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using Syncfusion.Pdf.Security;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfSignatureField : PdfSignatureAppearanceField
{
  private PdfSignature m_signature;
  internal bool m_fieldAutoNaming = true;
  internal bool m_SkipKidsCertificate;

  public PdfSignatureField(PdfPageBase page, string name)
    : base(page, name)
  {
    PdfDocumentBase pdfDocumentBase = (PdfDocumentBase) null;
    if (page is PdfPage)
      pdfDocumentBase = (PdfDocumentBase) (page as PdfPage).Document;
    else if (page is PdfLoadedPage)
      pdfDocumentBase = (page as PdfLoadedPage).Document;
    if (pdfDocumentBase == null)
      return;
    pdfDocumentBase.ObtainForm().SignatureFlags = SignatureFlags.SignaturesExists | SignatureFlags.AppendOnly;
  }

  internal PdfSignatureField()
  {
  }

  public new PdfAppearance Appearance => this.Widget.Appearance;

  public PdfSignature Signature
  {
    get => this.m_signature;
    set => this.m_signature = value;
  }

  protected override void Initialize()
  {
    base.Initialize();
    if (this.m_fieldAutoNaming)
      this.Widget.Dictionary.SetProperty("FT", (IPdfPrimitive) new PdfName("Sig"));
    else
      this.Dictionary.SetProperty("FT", (IPdfPrimitive) new PdfName("Sig"));
  }

  internal override void Save()
  {
    base.Save();
    if (this.m_signature == null)
      return;
    PdfSignatureDictionary signatureDictionary = (PdfSignatureDictionary) null;
    if (this.Page is PdfPage)
    {
      this.Widget.Dictionary.SetProperty("V", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) new PdfSignatureDictionary((PdfDocumentBase) ((PdfPage) this.Page).Document, this.m_signature, this.m_signature.Certificate)));
    }
    else
    {
      if (!(this.Page is PdfLoadedPage))
        return;
      if (this.m_signature.Certificate == null)
      {
        signatureDictionary = new PdfSignatureDictionary(((PdfLoadedPage) this.Page).Document, this.m_signature);
      }
      else
      {
        ((PdfLoadedPage) this.Page).Document.Catalog.BeginSave += new SavePdfPrimitiveEventHandler(this.Catalog_BeginSave);
        this.Dictionary.BeginSave += new SavePdfPrimitiveEventHandler(this.Dictionary_BeginSave);
        PdfSignatureDictionary wrapper = new PdfSignatureDictionary(((PdfLoadedPage) this.Page).Document, this.m_signature, this.m_signature.Certificate);
        ((PdfLoadedPage) this.Page).Document.PdfObjects.Add(((IPdfWrapper) wrapper).Element);
        if (!((PdfLoadedPage) this.Page).Document.CrossTable.IsMerging)
          ((IPdfWrapper) wrapper).Element.Position = -1;
        this.Widget.Dictionary.SetProperty("V", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) wrapper));
        this.Widget.Dictionary.SetProperty("Ff", (IPdfPrimitive) new PdfNumber(0));
        wrapper.Archive = false;
      }
    }
  }

  private void Catalog_BeginSave(object sender, SavePdfPrimitiveEventArgs ars)
  {
    if (!this.m_signature.Certificated)
      return;
    if (!(PdfCrossTable.Dereference(((PdfLoadedPage) this.Page).Document.Catalog["Perms"]) is PdfDictionary pdfDictionary))
    {
      ((PdfLoadedPage) this.Page).Document.Catalog["Perms"] = (IPdfPrimitive) new PdfDictionary()
      {
        ["DocMDP"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_signature.m_signatureDictionary)
      };
    }
    else
    {
      if (pdfDictionary.ContainsKey("DocMDP"))
        return;
      pdfDictionary.SetProperty("DocMDP", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_signature.m_signatureDictionary));
    }
  }

  private void Dictionary_BeginSave(object sender, SavePdfPrimitiveEventArgs ars)
  {
    if (this != null && this.Page is PdfPage)
    {
      this.Dictionary.Encrypt = ((PdfPage) this.Page).Document.Security.Enabled;
    }
    else
    {
      if (this == null || !(this.Page is PdfLoadedPage))
        return;
      this.Dictionary.Encrypt = ((PdfLoadedPage) this.Page).Document.Security.Enabled;
    }
  }

  internal override void Draw()
  {
    base.Draw();
    if (this.Widget.ObtainAppearance() == null)
      return;
    this.Page.Graphics.DrawPdfTemplate(this.Appearance.Normal, this.Location);
  }

  protected override void DrawAppearance(PdfTemplate template)
  {
    base.DrawAppearance(template);
    PaintParams paintParams = new PaintParams(new RectangleF(PointF.Empty, this.Size), this.BackBrush, (PdfBrush) null, this.BorderPen, this.BorderStyle, this.m_containsBW ? this.BorderWidth : 0.0f, this.ShadowBrush, this.RotationAngle);
    FieldPainter.DrawSignature(template.Graphics, paintParams);
  }
}
