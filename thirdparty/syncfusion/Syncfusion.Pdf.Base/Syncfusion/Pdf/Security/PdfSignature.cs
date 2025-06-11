// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.PdfSignature
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Security;

public class PdfSignature
{
  internal PdfSignatureDictionary m_signatureDictionary;
  private PdfSignatureField m_field;
  private PdfLoadedSignatureField m_sigField;
  private PdfCertificate m_pdfCertificate;
  private string m_reason;
  private PdfPageBase m_page;
  private string m_location;
  private string m_contactInformation;
  private PdfArray m_byteRange;
  private bool m_certeficated;
  private PdfCertificationFlags m_documentPermission = PdfCertificationFlags.ForbidChanges;
  private TimeStampServer m_timeStampServer;
  private bool m_hasDocumentPermission;
  internal PdfDocumentBase m_document;
  private bool m_drawSignatureAppearance;
  private bool m_enabledValiadtionAppearance;
  private SizeF size;
  internal bool isTimeStampOnly;
  private PdfDictionary m_dssDictionary;
  private bool m_enableLTV;
  internal DateTime m_signedDate;
  internal string m_signedName;
  private PdfSignatureSettings m_signatureSettings = new PdfSignatureSettings();
  private IPdfExternalSigner m_externalSigner;
  private byte[] m_ocsp;
  private ICollection<byte[]> m_crlBytes;
  private bool m_isExternalOCSP;
  private List<X509Certificate2> m_externalRootCert;
  private List<X509Certificate> m_externalChain;
  private uint m_estimatedSize;

  public event PdfSignatureEventHandler ComputeHash;

  public PdfSignatureSettings Settings => this.m_signatureSettings;

  [Obsolete("Please use Appearance instead")]
  public PdfAppearance Appearence
  {
    get => this.m_field != null ? this.m_field.Appearance : this.m_sigField.Appearance;
  }

  public PdfAppearance Appearance
  {
    get => this.m_field != null ? this.m_field.Appearance : this.m_sigField.Appearance;
  }

  public PointF Location
  {
    get => this.m_field != null ? this.m_field.Location : this.m_sigField.Location;
    set
    {
      if (this.m_field != null)
        this.m_field.Location = value;
      else
        this.m_sigField.Location = value;
    }
  }

  public RectangleF Bounds
  {
    get => this.m_field != null ? this.m_field.Bounds : this.m_sigField.Bounds;
    set
    {
      if (this.m_field != null)
        this.m_field.Bounds = value;
      else
        this.m_sigField.Bounds = value;
    }
  }

  public string ContactInfo
  {
    get => this.m_contactInformation;
    set => this.m_contactInformation = value;
  }

  internal PdfArray ByteRange
  {
    get => this.m_byteRange;
    set => this.m_byteRange = value;
  }

  public string Reason
  {
    get => this.m_reason;
    set => this.m_reason = value;
  }

  public string LocationInfo
  {
    get => this.m_location;
    set => this.m_location = value;
  }

  public bool Certificated
  {
    get
    {
      if (!this.m_certeficated && this.m_document != null && this.m_document is PdfLoadedDocument && this.m_document.Catalog != null)
        this.m_certeficated = this.CheckCertificated();
      return this.m_certeficated;
    }
    set
    {
      if (this.CheckCertificated() && !this.RemoveUnusedDocMDP())
        throw new ArgumentException("The document may contain at most one author signature!");
      this.m_certeficated = value;
    }
  }

  public PdfCertificationFlags DocumentPermissions
  {
    get => this.m_documentPermission;
    set => this.m_documentPermission = value;
  }

  internal bool HasDocumentPermission
  {
    get => this.m_hasDocumentPermission;
    set => this.m_hasDocumentPermission = value;
  }

  public PdfCertificate Certificate
  {
    get => this.m_pdfCertificate;
    set => this.m_pdfCertificate = value;
  }

  public bool EnableLtv
  {
    get => this.m_enableLTV;
    set
    {
      this.m_enableLTV = value;
      if (this.m_enableLTV && this.Certificate != null)
      {
        string key1 = "";
        List<X509Certificate> x509CertificateList = new List<X509Certificate>();
        if (this.Certificate.m_pkcs7Certificate != null)
        {
          foreach (string key2 in this.Certificate.m_pkcs7Certificate.KeyEnumerable)
          {
            if (this.Certificate.m_pkcs7Certificate.IsKey(key2) && this.Certificate.m_pkcs7Certificate.GetKey(key2).Key.IsPrivate)
            {
              key1 = key2;
              break;
            }
          }
          foreach (Syncfusion.Pdf.Security.X509Certificates x509Certificates in this.Certificate.m_pkcs7Certificate.GetCertificateChain(key1))
            x509CertificateList.Add(x509Certificates.Certificate);
        }
        else
        {
          for (int index = 0; index < this.Certificate.Chains.Length; ++index)
            x509CertificateList.Add(this.Certificate.Chains[index]);
        }
        if (this.ExternalSigner != null)
          this.EnableExternalLTV();
        else
          this.GetLTVData(x509CertificateList, (List<byte[]>) null, (List<byte[]>) null);
      }
      else
      {
        if (this.ExternalSigner == null)
          return;
        this.EnableExternalLTV();
      }
    }
  }

  public bool Visible
  {
    get
    {
      this.size = this.m_field == null ? this.m_sigField.Size : this.m_field.Size;
      return (double) this.size.Height != 0.0 || (double) this.size.Width != 0.0;
    }
  }

  public TimeStampServer TimeStampServer
  {
    get => this.m_timeStampServer;
    set => this.m_timeStampServer = value;
  }

  internal PdfField Field
  {
    get => this.m_field == null ? (PdfField) this.m_sigField : (PdfField) this.m_field;
  }

  internal bool DrawFieldAppearance => this.m_drawSignatureAppearance;

  public bool EnableValidationAppearance
  {
    set
    {
      this.m_enabledValiadtionAppearance = value;
      if (!value)
        return;
      this.Appearance.Normal.IsSignatureAppearanceValidation = value;
    }
    get => this.m_enabledValiadtionAppearance;
  }

  public DateTime SignedDate => this.m_signedDate;

  public string SignedName
  {
    get => this.m_signedName;
    set => this.m_signedName = value;
  }

  internal bool IsLTVEnabled
  {
    get
    {
      if (this.m_enableLTV)
        return this.m_enableLTV;
      return this.m_document != null && this.m_document.Catalog != null && this.m_document.Catalog.ContainsKey("DSS");
    }
  }

  internal byte[] OCSP => this.m_ocsp;

  internal ICollection<byte[]> CRLBytes => this.m_crlBytes;

  internal IPdfExternalSigner ExternalSigner => this.m_externalSigner;

  internal List<X509Certificate2> ExternalCertificates => this.m_externalRootCert;

  public uint EstimatedSignatureSize
  {
    internal get => this.m_estimatedSize;
    set => this.m_estimatedSize = value;
  }

  [Obsolete("Please use PdfSignature(PdfPage page, PdfCertificate cert, string signatureName)instead")]
  public PdfSignature() => this.m_drawSignatureAppearance = true;

  public PdfSignature(PdfPage page, PdfCertificate cert, string signatureName)
  {
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    if (cert == null)
      throw new ArgumentNullException(nameof (cert));
    this.m_page = (PdfPageBase) page;
    this.m_pdfCertificate = cert;
    this.CheckAnnotationElementsContainsSignature((PdfPageBase) page, signatureName);
    this.m_field = new PdfSignatureField((PdfPageBase) page, signatureName);
    PdfDocument document = page.Document;
    this.m_document = (PdfDocumentBase) document;
    document.Form.Fields.Add((PdfField) this.m_field);
    document.Form.SignatureFlags = SignatureFlags.SignaturesExists | SignatureFlags.AppendOnly;
    document.Catalog.BeginSave += new SavePdfPrimitiveEventHandler(this.Catalog_BeginSave);
    this.m_field.Dictionary.BeginSave += new SavePdfPrimitiveEventHandler(this.Dictionary_BeginSave);
    if (!this.m_field.m_SkipKidsCertificate)
    {
      this.m_signatureDictionary = new PdfSignatureDictionary((PdfDocumentBase) document, this, cert);
      document.PdfObjects.Add(((IPdfWrapper) this.m_signatureDictionary).Element);
      if (!document.CrossTable.IsMerging)
        ((IPdfWrapper) this.m_signatureDictionary).Element.Position = -1;
      this.m_field.Widget.Dictionary.SetProperty("V", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_signatureDictionary));
      this.m_signatureDictionary.Archive = false;
    }
    this.m_field.Widget.Dictionary.SetProperty("Ff", (IPdfPrimitive) new PdfNumber(0));
  }

  public PdfSignature(PdfPage page, string signatureName)
  {
    this.m_page = page != null ? (PdfPageBase) page : throw new ArgumentNullException(nameof (page));
    this.CheckAnnotationElementsContainsSignature((PdfPageBase) page, signatureName);
    this.m_field = new PdfSignatureField((PdfPageBase) page, signatureName);
    this.isTimeStampOnly = true;
    PdfDocument document = page.Document;
    this.m_document = (PdfDocumentBase) document;
    document.Form.Fields.Add((PdfField) this.m_field);
    document.Form.SignatureFlags = SignatureFlags.SignaturesExists | SignatureFlags.AppendOnly;
    document.Catalog.BeginSave += new SavePdfPrimitiveEventHandler(this.Catalog_BeginSave);
    this.m_field.Dictionary.BeginSave += new SavePdfPrimitiveEventHandler(this.Dictionary_BeginSave);
    if (!this.m_field.m_SkipKidsCertificate)
    {
      this.m_signatureDictionary = new PdfSignatureDictionary((PdfDocumentBase) document, this);
      document.PdfObjects.Add(((IPdfWrapper) this.m_signatureDictionary).Element);
      if (!document.CrossTable.IsMerging)
        ((IPdfWrapper) this.m_signatureDictionary).Element.Position = -1;
      this.m_field.Widget.Dictionary.SetProperty("V", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_signatureDictionary));
      this.m_signatureDictionary.Archive = false;
    }
    this.m_field.Widget.Dictionary.SetProperty("Ff", (IPdfPrimitive) new PdfNumber(0));
  }

  public PdfSignature(PdfLoadedPage page, string signatureName)
  {
    this.m_page = page != null ? (PdfPageBase) page : throw new ArgumentNullException(nameof (page));
    this.CheckAnnotationElementsContainsSignature((PdfPageBase) page, signatureName);
    this.m_field = new PdfSignatureField((PdfPageBase) page, signatureName);
    this.isTimeStampOnly = true;
    PdfLoadedDocument document = page.Document as PdfLoadedDocument;
    this.m_document = (PdfDocumentBase) document;
    document.Form.Fields.Add((PdfField) this.m_field);
    document.Form.SignatureFlags = SignatureFlags.SignaturesExists | SignatureFlags.AppendOnly;
    document.Catalog.BeginSave += new SavePdfPrimitiveEventHandler(this.Catalog_BeginSave);
    this.m_field.Dictionary.BeginSave += new SavePdfPrimitiveEventHandler(this.Dictionary_BeginSave);
    if (!this.m_field.m_SkipKidsCertificate)
    {
      this.m_signatureDictionary = new PdfSignatureDictionary((PdfDocumentBase) document, this);
      document.PdfObjects.Add(((IPdfWrapper) this.m_signatureDictionary).Element);
      if (!document.CrossTable.IsMerging)
        ((IPdfWrapper) this.m_signatureDictionary).Element.Position = -1;
      this.m_field.Widget.Dictionary.SetProperty("V", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_signatureDictionary));
      this.m_signatureDictionary.Archive = false;
    }
    this.m_field.Widget.Dictionary.SetProperty("Ff", (IPdfPrimitive) new PdfNumber(0));
  }

  public PdfSignature(
    PdfDocumentBase document,
    PdfPageBase page,
    PdfCertificate certificate,
    string signatureName)
  {
    if (document == null)
      throw new ArgumentNullException(nameof (document));
    this.m_page = page != null ? page : throw new ArgumentNullException(nameof (page));
    this.m_pdfCertificate = certificate;
    this.m_document = document;
    this.CheckAnnotationElementsContainsSignature(page, signatureName);
    this.m_field = new PdfSignatureField(page, signatureName);
    if (this.m_field.Dictionary.ContainsKey("Kids"))
    {
      PdfArray pdfArray = this.m_field.Dictionary["Kids"] as PdfArray;
      for (int index = 0; index < pdfArray.Count; ++index)
      {
        if (PdfCrossTable.Dereference(pdfArray[index]) is PdfDictionary pdfDictionary)
          pdfDictionary.BeginSave += new SavePdfPrimitiveEventHandler(this.Dictionary_BeginSave);
      }
    }
    PdfForm form = document.ObtainForm();
    if (form is PdfLoadedForm pdfLoadedForm)
      pdfLoadedForm.Fields.Add((PdfField) this.m_field);
    else
      form.Fields.Add((PdfField) this.m_field);
    form.SignatureFlags = SignatureFlags.SignaturesExists | SignatureFlags.AppendOnly;
    document.Catalog.BeginSave += new SavePdfPrimitiveEventHandler(this.Catalog_BeginSave);
    this.m_field.Dictionary.BeginSave += new SavePdfPrimitiveEventHandler(this.Dictionary_BeginSave);
    if (!this.m_field.m_SkipKidsCertificate)
    {
      this.m_signatureDictionary = new PdfSignatureDictionary(document, this, certificate);
      document.PdfObjects.Add(((IPdfWrapper) this.m_signatureDictionary).Element);
      if (!document.CrossTable.IsMerging)
        ((IPdfWrapper) this.m_signatureDictionary).Element.Position = -1;
      this.m_field.Dictionary.SetProperty("V", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_signatureDictionary));
      this.m_signatureDictionary.Archive = false;
    }
    this.m_field.Widget.Dictionary.SetProperty("Ff", (IPdfPrimitive) new PdfNumber(0));
  }

  public PdfSignature(
    PdfDocumentBase document,
    PdfPageBase page,
    PdfCertificate certificate,
    string signatureName,
    PdfLoadedSignatureField loadedField)
  {
    if (document == null)
      throw new ArgumentNullException(nameof (document));
    this.m_page = page != null ? page : throw new ArgumentNullException(nameof (page));
    this.m_pdfCertificate = certificate;
    this.m_document = document;
    this.m_sigField = loadedField;
    if (document.ObtainForm() is PdfLoadedForm form && this.m_sigField.Form == null)
      form.Fields.Add((PdfField) this.m_sigField);
    form.SignatureFlags = SignatureFlags.SignaturesExists | SignatureFlags.AppendOnly;
    document.Catalog.BeginSave += new SavePdfPrimitiveEventHandler(this.Catalog_BeginSave);
    this.m_sigField.Dictionary.BeginSave += new SavePdfPrimitiveEventHandler(this.Dictionary_BeginSave);
    this.m_signatureDictionary = new PdfSignatureDictionary(document, this, certificate);
    document.PdfObjects.Add(((IPdfWrapper) this.m_signatureDictionary).Element);
    if (!document.CrossTable.IsMerging)
      ((IPdfWrapper) this.m_signatureDictionary).Element.Position = -1;
    this.m_sigField.Dictionary.SetProperty("V", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_signatureDictionary));
    this.m_sigField.Widget.Bounds = this.m_sigField.Bounds;
    this.m_sigField.Dictionary.SetProperty("Ff", (IPdfPrimitive) new PdfNumber(0));
    this.m_signatureDictionary.Archive = false;
  }

  internal void SetValidationApperance()
  {
    if (!this.EnableValidationAppearance)
      return;
    float num1 = 0.3f;
    SizeF size = this.Bounds.Size;
    PdfFont font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, 12f);
    float[] scale = this.findScale(size.Width, size.Height);
    float height = size.Height * num1;
    if (this.m_page != null && (this.m_page.Rotation == PdfPageRotateAngle.RotateAngle90 || this.m_page.Rotation == PdfPageRotateAngle.RotateAngle270))
      height = size.Width * num1;
    PdfTemplate template1 = new PdfTemplate(size, false);
    template1.CustomPdfTemplateName = "FRM";
    PdfTemplate template2 = new PdfTemplate(new SizeF(100f, 100f), false);
    template2.CustomPdfTemplateName = "n0";
    template2.Clear("% DSBlank");
    PdfGraphicsState state1 = template1.Graphics.Save();
    if (this.m_page != null && this.m_page.Rotation == PdfPageRotateAngle.RotateAngle90)
    {
      template1.Graphics.RotateTransform(-90f);
      template1.Graphics.DrawPdfTemplate(template2, new PointF(0.0f, 0.0f));
    }
    else
      template1.Graphics.DrawPdfTemplate(template2, new PointF(0.0f, -100f));
    template1.Graphics.Restore(state1);
    PdfTemplate template3 = new PdfTemplate(new SizeF(100f, 100f), false);
    template3.CustomPdfTemplateName = "n1";
    template3.Clear("% DSUnkown\n");
    string comment = "q\n1 G\n1 g\n0.1 0 0 0.1 9 0 cm\n0 J 0 j 4 M []0 d\n1 i \n0 g\n313 292 m\n313 404 325 453 432 529 c\n478 561 504 597 504 645 c\n504 736 440 760 391 760 c\n286 760 271 681 265 626 c\n265 625 l\n100 625 l\n100 828 253 898 381 898 c\n451 898 679 878 679 650 c\n679 555 628 499 538 435 c\n488 399 467 376 467 292 c\n313 292 l\nh\n308 214 170 -164 re\nf\n0.44 G\n1.2 w\n1 1 0 rg\n287 318 m\n287 430 299 479 406 555 c\n451 587 478 623 478 671 c\n478 762 414 786 365 786 c\n260 786 245 707 239 652 c\n239 651 l\n74 651 l\n74 854 227 924 355 924 c\n425 924 653 904 653 676 c\n653 581 602 525 512 461 c\n462 425 441 402 441 318 c\n287 318 l\nh\n282 240 170 -164 re\nB\nQ\n";
    template3.Graphics.StreamWriter.WriteComment(comment);
    PdfGraphicsState state2 = template1.Graphics.Save();
    float num2 = Math.Min(size.Width, size.Height);
    if (this.m_page != null && (this.m_page.Rotation == PdfPageRotateAngle.RotateAngle90 || this.m_page.Rotation == PdfPageRotateAngle.RotateAngle270))
    {
      template1.Graphics.RotateTransform(-90f);
      template1.Graphics.DrawPdfTemplate(template3, new PointF(0.0f, 0.0f), new SizeF(num2, num2));
    }
    else
      template1.Graphics.DrawPdfTemplate(template3, new PointF(size.Width - num2, -size.Height), new SizeF(num2, num2));
    template1.Graphics.Restore(state2);
    PdfGraphicsState state3 = template1.Graphics.Save();
    if (this.m_page != null && (this.m_page.Rotation == PdfPageRotateAngle.RotateAngle90 || this.m_page.Rotation == PdfPageRotateAngle.RotateAngle270))
    {
      template1.Graphics.RotateTransform(-90f);
      template1.Graphics.DrawPdfTemplate(this.Appearance.AppearanceLayer, new PointF(0.0f, 0.0f));
    }
    else
      template1.Graphics.DrawPdfTemplate(this.Appearance.AppearanceLayer, new PointF(0.0f, -size.Height));
    template1.Graphics.Restore(state3);
    PdfTemplate template4 = new PdfTemplate(new SizeF(100f, 100f), false);
    template4.CustomPdfTemplateName = "n3";
    template4.Clear("% DSBlank\n");
    PdfGraphicsState state4 = template1.Graphics.Save();
    template1.Graphics.ScaleTransform(scale[0], scale[0]);
    if (this.m_page != null && (this.m_page.Rotation == PdfPageRotateAngle.RotateAngle90 || this.m_page.Rotation == PdfPageRotateAngle.RotateAngle270))
    {
      template1.Graphics.RotateTransform(-90f);
      template1.Graphics.DrawPdfTemplate(template4, new PointF(0.0f, 0.0f));
    }
    else
      template1.Graphics.DrawPdfTemplate(template4, new PointF(scale[1], (float) -(100.0 + (double) scale[2])));
    template1.Graphics.Restore(state4);
    PdfTemplate template5 = new PdfTemplate(new RectangleF(0.0f, size.Height - height, size.Width, height));
    if (this.m_page != null && (this.m_page.Rotation == PdfPageRotateAngle.RotateAngle90 || this.m_page.Rotation == PdfPageRotateAngle.RotateAngle270))
      template5 = new PdfTemplate(0.0f, 0.0f, size.Height, size.Width);
    template5.CustomPdfTemplateName = "n4";
    template5.m_writeTransformation = false;
    string str = "Signature Not Verified";
    if (this.m_page != null && (this.m_page.Rotation == PdfPageRotateAngle.RotateAngle90 || this.m_page.Rotation == PdfPageRotateAngle.RotateAngle270))
    {
      this.SetFittingFontSize(new RectangleF(0.0f, 0.0f, size.Height, height), font, str);
      template5.Graphics.DrawString(str, font, PdfBrushes.Black, new RectangleF(0.0f, -size.Width, size.Height, height), new PdfStringFormat(PdfTextAlignment.Center));
    }
    else
    {
      this.SetFittingFontSize(new RectangleF(0.0f, 0.0f, size.Width, height), font, str);
      template5.Graphics.DrawString(str, font, PdfBrushes.Black, new RectangleF(0.0f, -size.Height, size.Width, height), new PdfStringFormat(PdfTextAlignment.Center));
    }
    PdfGraphicsState state5 = template1.Graphics.Save();
    if (this.m_page != null && this.m_page.Rotation == PdfPageRotateAngle.RotateAngle90 || this.m_page.Rotation == PdfPageRotateAngle.RotateAngle270)
    {
      template1.Graphics.RotateTransform(-90f);
      template1.Graphics.DrawPdfTemplate(template5, new PointF(0.0f, 0.0f));
    }
    else
      template1.Graphics.DrawPdfTemplate(template5, new PointF(0.0f, -height));
    template1.Graphics.Restore(state5);
    this.SetMatrix((PdfDictionary) template1.m_content);
    this.Appearance.IsCompletedValidationAppearance = true;
    this.Appearance.Normal = new PdfTemplate(size, false);
    this.Appearance.Normal.Graphics.DrawPdfTemplate(template1, new PointF(0.0f, -size.Height));
  }

  private void SetFittingFontSize(RectangleF rect, PdfFont font, string text)
  {
    float num1 = 0.0f;
    if (this.m_field != null)
      num1 = this.m_field.BorderWidth;
    else if (this.m_sigField != null)
      num1 = this.m_sigField.BorderWidth;
    float num2 = rect.Width - 4f * num1;
    float num3 = rect.Height - 2f * num1;
    float num4 = 0.248f;
    font.Style = PdfFontStyle.Bold;
    for (float num5 = 0.0f; (double) num5 <= (double) rect.Height; ++num5)
    {
      font.Size = num5;
      SizeF sizeF1 = font.MeasureString(text);
      if ((double) sizeF1.Width > (double) rect.Width || (double) sizeF1.Height > (double) num3)
      {
        float num6 = num5;
        do
        {
          num6 -= 1f / 1000f;
          font.Size = num6;
          float lineWidth = font.GetLineWidth(text, new PdfStringFormat());
          if ((double) num6 < (double) num4)
          {
            font.Size = num4;
            break;
          }
          SizeF sizeF2 = font.MeasureString(text);
          if ((double) lineWidth < (double) num2 && (double) sizeF2.Height < (double) num3)
          {
            font.Size = num6;
            break;
          }
        }
        while ((double) num6 > (double) num4);
        break;
      }
    }
  }

  private void SetMatrix(PdfDictionary template)
  {
    float[] numArray = new float[0];
    if (!(template["BBox"] is PdfArray pdfArray1) || this.m_page == null || this.m_page.Rotation != PdfPageRotateAngle.RotateAngle180 && this.m_page.Rotation != PdfPageRotateAngle.RotateAngle270)
      return;
    PdfArray pdfArray2 = new PdfArray(new float[6]
    {
      -1f,
      0.0f,
      0.0f,
      -1f,
      (pdfArray1[2] as PdfNumber).FloatValue,
      (pdfArray1[3] as PdfNumber).FloatValue
    });
    template["Matrix"] = (IPdfPrimitive) pdfArray2;
  }

  private float[] findScale(float m_templateWidth, float m_templateHeight)
  {
    float[] scale = new float[3];
    scale[0] = Math.Min(m_templateWidth, m_templateHeight) * 0.9f;
    scale[1] = (float) (((double) m_templateWidth - (double) scale[0]) / 2.0);
    scale[2] = (float) (((double) m_templateHeight - (double) scale[0]) / 2.0);
    scale[0] /= 100f;
    return scale;
  }

  private void CheckAnnotationElementsContainsSignature(PdfPageBase page, string signatureName)
  {
    if (page.Dictionary == null || !page.Dictionary.ContainsKey("Annots"))
      return;
    PdfArray pdfArray = PdfCrossTable.Dereference(page.Dictionary["Annots"]) as PdfArray;
    PdfDictionary pdfDictionary = (PdfDictionary) null;
    if (pdfArray != null && pdfArray.Elements.Count > 0)
      pdfDictionary = PdfCrossTable.Dereference(pdfArray[pdfArray.Elements.Count - 1]) as PdfDictionary;
    if (pdfDictionary == null || !pdfDictionary.ContainsKey("T"))
      return;
    PdfString pdfString = PdfCrossTable.Dereference(pdfDictionary["T"]) as PdfString;
    string empty = string.Empty;
    if (pdfString != null)
      empty = Encoding.Default.GetString(pdfString.Bytes);
    if (!(empty == signatureName) || pdfArray.Elements.Count <= 0)
      return;
    pdfArray.Elements.RemoveAt(pdfArray.Elements.Count - 1);
  }

  public void CreateLongTermValidity(List<X509Certificate2> certificates)
  {
    this.m_enableLTV = this.GetLTVData(this.GetX509CertificateList(certificates), (List<byte[]>) null, (List<byte[]>) null);
  }

  internal void CreateLongTermValidity(
    List<X509Certificate2> certificates,
    List<byte[]> ocspResponseData,
    List<byte[]> crlResponseData)
  {
    this.m_enableLTV = this.GetLTVData(this.GetX509CertificateList(certificates), ocspResponseData, crlResponseData);
  }

  private List<X509Certificate> GetX509CertificateList(List<X509Certificate2> certificates)
  {
    this.m_enableLTV = true;
    X509CertificateParser certificateParser = new X509CertificateParser();
    List<X509Certificate> x509CertificateList = new List<X509Certificate>();
    foreach (X509Certificate2 certificate in certificates)
      x509CertificateList.Add(certificateParser.ReadCertificate(certificate.RawData));
    return x509CertificateList;
  }

  private bool GetLTVData(
    List<X509Certificate> x509CertificateList,
    List<byte[]> ocspResponseData,
    List<byte[]> crlResponseData)
  {
    bool dssDetails;
    if (crlResponseData != null && crlResponseData.Count > 0 && ocspResponseData != null && ocspResponseData.Count > 0)
      dssDetails = this.GetDssDetails(crlResponseData, ocspResponseData);
    else if (ocspResponseData == null && crlResponseData != null && crlResponseData.Count > 0)
    {
      dssDetails = this.GetDssDetails(crlResponseData, this.GetOCSPData(new Ocsp(), x509CertificateList));
    }
    else
    {
      RevocationList crl = new RevocationList((ICollection<X509Certificate>) x509CertificateList);
      if (ocspResponseData != null && ocspResponseData.Count > 0)
      {
        dssDetails = this.GetDSSDetails(x509CertificateList, ocspResponseData, crl);
      }
      else
      {
        Ocsp ocsp = new Ocsp();
        dssDetails = this.GetDSSDetails(x509CertificateList, ocsp, crl, OCSPType.OCSP_CRL);
      }
    }
    if (this.m_dssDictionary != null)
    {
      this.m_document.Catalog["DSS"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) this.m_dssDictionary);
      this.m_dssDictionary.Modify();
    }
    x509CertificateList.Clear();
    return dssDetails;
  }

  public void AddExternalSigner(
    IPdfExternalSigner signer,
    List<X509Certificate2> publicCertificates,
    byte[] Ocsp)
  {
    this.m_externalSigner = signer;
    this.m_externalRootCert = publicCertificates;
    if (Ocsp != null)
      this.m_isExternalOCSP = true;
    this.m_ocsp = Ocsp;
    if (publicCertificates == null && this.m_pdfCertificate == null)
      throw new PdfException("The certificate should not null");
    if (this.ExternalCertificates == null && this.m_pdfCertificate != null)
    {
      this.m_externalRootCert = new List<X509Certificate2>();
      this.m_externalRootCert.Add(this.m_pdfCertificate.X509Certificate);
    }
    if (this.ExternalCertificates != null)
    {
      X509CertificateParser certificateParser = new X509CertificateParser();
      this.m_externalChain = new List<X509Certificate>();
      foreach (X509Certificate2 externalCertificate in this.ExternalCertificates)
        this.m_externalChain.Add(certificateParser.ReadCertificate(externalCertificate.RawData));
    }
    if (this.m_externalChain != null && this.OCSP != null && this.m_isExternalOCSP)
    {
      X509RevocationResponse responseObject = (X509RevocationResponse) new OcspResponseHelper((Stream) new MemoryStream(this.m_ocsp)).GetResponseObject();
      if (responseObject != null)
        this.m_ocsp = responseObject.EncodedBytes;
    }
    if (!this.EnableLtv)
      return;
    this.EnableExternalLTV();
  }

  internal void OnComputeHash(PdfSignatureEventArgs args)
  {
    if (this.ComputeHash == null)
      return;
    this.ComputeHash((object) this, args);
  }

  public static void ReplaceEmptySignature(
    Stream inputFileStream,
    string pdfPassword,
    Stream outputFileStream,
    string signatureName,
    IPdfExternalSigner externalSigner,
    List<X509Certificate2> publicCertificates)
  {
    PdfSignature.ReplaceEmptySignature(inputFileStream, pdfPassword, outputFileStream, signatureName, externalSigner, publicCertificates, true);
  }

  public static void ReplaceEmptySignature(
    Stream inputFileStream,
    string pdfPassword,
    Stream outputFileStream,
    string signatureName,
    IPdfExternalSigner externalSigner,
    List<X509Certificate2> publicCertificates,
    bool isEncodeSignature)
  {
    if (inputFileStream == null)
      throw new ArgumentNullException("inputFile is null");
    if (inputFileStream.Length == 0L)
      throw new PdfException("Contents of file stream is empty");
    if (!inputFileStream.CanRead || !inputFileStream.CanSeek)
      throw new ArgumentException("Can't use the specified stream.", "input stream");
    if (inputFileStream.Position != 0L)
      inputFileStream.Position = 0L;
    if (outputFileStream == null)
      throw new ArgumentNullException("outputFile is null");
    if (!outputFileStream.CanWrite || !outputFileStream.CanSeek)
      throw new ArgumentException("Can't use the specified stream to write", "output stream");
    Stream stream = outputFileStream;
    Stream sourceStream = inputFileStream;
    PdfLoadedDocument pdfLoadedDocument = new PdfLoadedDocument(inputFileStream, pdfPassword);
    try
    {
      PdfLoadedForm form = pdfLoadedDocument.Form;
      if (form == null)
        return;
      PdfLoadedField field;
      form.Fields.TryGetField(signatureName, out field);
      if (!(field is PdfLoadedSignatureField loadedSignatureField))
        throw new PdfException("Signature field name not found");
      if (loadedSignatureField.Dictionary == null || !loadedSignatureField.Dictionary.ContainsKey("V") || !(PdfCrossTable.Dereference(loadedSignatureField.Dictionary["V"]) is PdfDictionary pdfDictionary) || !pdfDictionary.ContainsKey("ByteRange") || !(PdfCrossTable.Dereference(pdfDictionary["ByteRange"]) is PdfArray pdfArray) || pdfArray.Count <= 3)
        return;
      long num1 = 0;
      long num2 = 0;
      long num3 = 0;
      long num4 = 0;
      if (pdfArray[0] is PdfNumber pdfNumber1)
        num1 = pdfNumber1.LongValue;
      if (pdfArray[1] is PdfNumber pdfNumber2)
        num2 = pdfNumber2.LongValue;
      if (pdfArray[2] is PdfNumber pdfNumber3)
        num3 = pdfNumber3.LongValue;
      if (pdfArray[3] is PdfNumber pdfNumber4)
        num4 = pdfNumber4.LongValue;
      long[] numArray1 = new long[4]
      {
        num1,
        num2,
        num3,
        num4
      };
      byte[] numArray2 = new byte[sourceStream.Length];
      sourceStream.Position = 0L;
      sourceStream.Read(numArray2, 0, (int) sourceStream.Length);
      IRandom source = (IRandom) new RandomArray(numArray2);
      IRandom[] sources = new IRandom[numArray1.Length / 2];
      for (int index = 0; index < numArray1.Length; index += 2)
        sources[index / 2] = (IRandom) new WindowRandom(source, numArray1[index], numArray1[index + 1]);
      Stream data = (Stream) new RandomStream((IRandom) new RandomGroup((ICollection<IRandom>) sources));
      byte[] numArray3 = (byte[]) null;
      if (isEncodeSignature)
      {
        byte[] secondDigest = new MessageDigestAlgorithms().Digest(data, externalSigner.HashAlgorithm);
        string empty = string.Empty;
        ICollection<X509Certificate> x509Certificates = (ICollection<X509Certificate>) new List<X509Certificate>();
        List<X509Certificate> x509CertificateList = new List<X509Certificate>((IEnumerable<X509Certificate>) x509Certificates);
        if (externalSigner != null && publicCertificates != null)
        {
          X509CertificateParser certificateParser = new X509CertificateParser();
          foreach (X509Certificate2 publicCertificate in publicCertificates)
            x509Certificates.Add(certificateParser.ReadCertificate(publicCertificate.RawData));
          string hashAlgorithm1 = externalSigner.HashAlgorithm;
          byte[] timeStampResponse = (byte[]) null;
          EncryptionAlgorithms encryptionAlgorithms = new EncryptionAlgorithms();
          string encryptionAlgorithm = (string) null;
          for (int index = 0; index < x509CertificateList.Count; ++index)
          {
            if (encryptionAlgorithms.GetAlgorithm(x509CertificateList[index].SigAlgOid) == "ECDSA")
            {
              encryptionAlgorithm = "ECDSA";
              break;
            }
          }
          SignaturePrivateKey signaturePrivateKey1 = new SignaturePrivateKey(hashAlgorithm1, encryptionAlgorithm);
          string hashAlgorithm2 = signaturePrivateKey1.GetHashAlgorithm();
          SignaturePrivateKey signaturePrivateKey2 = signaturePrivateKey1;
          PdfCmsSigner pdfCmsSigner = new PdfCmsSigner((ICipherParam) null, x509Certificates, hashAlgorithm2, false);
          byte[] sequenceData = pdfCmsSigner.GetSequenceData(secondDigest, (byte[]) null, (ICollection<byte[]>) null, loadedSignatureField.Signature.Settings.CryptographicStandard);
          byte[] digest = externalSigner.Sign(sequenceData, out timeStampResponse);
          pdfCmsSigner.SetSignedData(digest, (byte[]) null, signaturePrivateKey2.GetEncryptionAlgorithm());
          numArray3 = pdfCmsSigner.Sign(secondDigest, (TimeStampServer) null, timeStampResponse, (byte[]) null, (ICollection<byte[]>) null, loadedSignatureField.Signature.Settings.CryptographicStandard, hashAlgorithm2);
          byte[] destinationArray = new byte[numArray3.Length];
          System.Array.Copy((System.Array) numArray3, 0, (System.Array) destinationArray, 0, numArray3.Length);
        }
      }
      else if (externalSigner != null)
      {
        data.Position = 0L;
        byte[] numArray4 = new byte[data.Length];
        data.Read(numArray4, 0, numArray4.Length);
        byte[] timeStampResponse = (byte[]) null;
        numArray3 = externalSigner.Sign(numArray4, out timeStampResponse);
      }
      int num5 = (int) (numArray1[2] - numArray1[1]) - 2;
      if ((num5 & 1) != 0)
        throw new PdfException("Allocated space was not enough");
      int num6 = num5 / 2;
      if (num6 < numArray3.Length)
        throw new PdfException("Signature content space is not enough for signed bytes");
      PdfSignature.TransferBytes(sourceStream, 0L, numArray1[1] + 1L, stream);
      string hex = PdfString.BytesToHex(numArray3);
      StringBuilder stringBuilder = new StringBuilder(num6 * 2);
      stringBuilder.Append(hex);
      int num7 = (num6 - numArray3.Length) * 2;
      for (int index = 0; index < num7; ++index)
        stringBuilder.Append(0);
      string s = stringBuilder.ToString();
      MemoryStream memoryStream = new MemoryStream(s.Length);
      memoryStream.Write(Encoding.UTF8.GetBytes(s), 0, s.Length);
      memoryStream.WriteTo(stream);
      PdfSignature.TransferBytes(sourceStream, numArray1[2] - 1L, numArray1[3] + 1L, stream);
      memoryStream.Dispose();
    }
    catch (Exception ex)
    {
      throw ex;
    }
    finally
    {
      pdfLoadedDocument.Close(true);
    }
  }

  private static void TransferBytes(
    Stream sourceStream,
    long positionStart,
    long length,
    Stream destination)
  {
    if (length <= 0L)
      return;
    long currentPosition = positionStart;
    byte[] numArray = new byte[8192 /*0x2000*/];
    long location;
    for (; length > 0L; length -= location)
    {
      location = (long) PdfSignature.FindLocation(currentPosition, numArray, 0, (int) Math.Min((long) numArray.Length, length), sourceStream);
      if (location <= 0L)
        throw new EndOfStreamException();
      destination.Write(numArray, 0, (int) location);
      currentPosition += location;
    }
  }

  private static int FindLocation(
    long currentPosition,
    byte[] data,
    int offsetPosition,
    int length,
    Stream sourceStream)
  {
    if (sourceStream.Position != currentPosition)
      sourceStream.Seek(currentPosition, SeekOrigin.Begin);
    int num = sourceStream.Read(data, offsetPosition, length);
    return num != 0 ? num : -1;
  }

  private bool CheckCertificated()
  {
    bool flag = false;
    if (!(PdfCrossTable.Dereference(this.m_document.Catalog["Perms"]) is PdfDictionary pdfDictionary) || !pdfDictionary.ContainsKey("DocMDP"))
      return flag;
    PdfReferenceHolder pdfReferenceHolder1 = pdfDictionary["DocMDP"] as PdfReferenceHolder;
    if (this.m_signatureSettings != null && this.m_signatureSettings.SignatureField != null && this.m_signatureSettings.SignatureField != null && this.m_signatureSettings.SignatureField.Dictionary != null && this.m_signatureSettings.SignatureField.Dictionary.ContainsKey("V"))
    {
      PdfReferenceHolder pdfReferenceHolder2 = this.m_signatureSettings.SignatureField.Dictionary["V"] as PdfReferenceHolder;
      if (pdfReferenceHolder1 != (PdfReferenceHolder) null && pdfReferenceHolder2 != (PdfReferenceHolder) null && pdfReferenceHolder1.Reference != (PdfReference) null && pdfReferenceHolder2.Reference != (PdfReference) null && pdfReferenceHolder1.Reference.ObjNum == pdfReferenceHolder2.Reference.ObjNum)
        flag = true;
    }
    else if (this.m_signatureSettings != null && this.m_signatureSettings.SignatureField == null)
      flag = true;
    return flag;
  }

  private bool RemoveUnusedDocMDP()
  {
    bool flag1 = false;
    if (PdfCrossTable.Dereference(this.m_document.Catalog["Perms"]) is PdfDictionary pdfDictionary && pdfDictionary.ContainsKey("DocMDP"))
    {
      PdfReferenceHolder pdfReferenceHolder1 = pdfDictionary["DocMDP"] as PdfReferenceHolder;
      if (pdfReferenceHolder1 != (PdfReferenceHolder) null && pdfReferenceHolder1.Reference != (PdfReference) null)
      {
        bool flag2 = false;
        if (this.Field != null && this.Field.Form != null && this.Field.Form is PdfLoadedForm form && form.Fields != null && form.Fields != null)
        {
          foreach (IPdfWrapper field in (PdfCollection) form.Fields)
          {
            if (field is PdfSignatureField)
            {
              PdfSignatureField pdfSignatureField = field as PdfSignatureField;
              if (pdfSignatureField.Dictionary != null && pdfSignatureField.Dictionary.ContainsKey("V"))
              {
                PdfReferenceHolder pdfReferenceHolder2 = pdfSignatureField.Dictionary["V"] as PdfReferenceHolder;
                if (pdfReferenceHolder2 != (PdfReferenceHolder) null && pdfReferenceHolder2.Reference != (PdfReference) null && pdfReferenceHolder2.Reference.ObjNum == pdfReferenceHolder1.Reference.ObjNum)
                {
                  flag2 = true;
                  break;
                }
              }
            }
            else if (field is PdfLoadedSignatureField)
            {
              PdfLoadedSignatureField loadedSignatureField = field as PdfLoadedSignatureField;
              if (loadedSignatureField.Dictionary != null && loadedSignatureField.Dictionary.ContainsKey("V"))
              {
                PdfReferenceHolder pdfReferenceHolder3 = loadedSignatureField.Dictionary["V"] as PdfReferenceHolder;
                if (pdfReferenceHolder3 != (PdfReferenceHolder) null && pdfReferenceHolder3.Reference != (PdfReference) null && pdfReferenceHolder3.Reference.ObjNum == pdfReferenceHolder1.Reference.ObjNum)
                {
                  flag2 = true;
                  break;
                }
              }
            }
          }
        }
        if (!flag2)
        {
          pdfDictionary.Remove("DocMDP");
          flag1 = true;
        }
      }
    }
    return flag1;
  }

  private bool GetDSSDetails(
    List<X509Certificate> certificates,
    Ocsp ocsp,
    RevocationList crl,
    OCSPType level)
  {
    List<byte[]> crlCollection = new List<byte[]>();
    List<byte[]> ocspCollection = new List<byte[]>();
    for (int index = 0; index < certificates.Count; ++index)
    {
      X509Certificate certificate = certificates[index];
      byte[] BasicOCSPResponse = (byte[]) null;
      if (ocsp != null && level != OCSPType.CRL)
      {
        BasicOCSPResponse = ocsp.GetEncodedOcspRspnose(certificate, this.GetRoot(certificate, certificates), (string) null);
        if (BasicOCSPResponse != null)
        {
          if (this.m_ocsp == null)
            this.m_ocsp = BasicOCSPResponse;
          ocspCollection.Add(PdfSignature.BuildOCSPResponse(BasicOCSPResponse));
        }
      }
      if (crl != null)
      {
        switch (level)
        {
          case OCSPType.CRL:
          case OCSPType.OCSP_CRL:
            ICollection<byte[]> encoded = crl.GetEncoded(certificates[index], (string) null);
            if (encoded != null)
            {
              using (IEnumerator<byte[]> enumerator = encoded.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  byte[] current = enumerator.Current;
                  bool flag = false;
                  foreach (byte[] a in crlCollection)
                  {
                    if (Asn1Constants.AreEqual(a, current))
                    {
                      flag = true;
                      break;
                    }
                  }
                  if (!flag)
                    crlCollection.Add(current);
                }
                continue;
              }
            }
            continue;
          case OCSPType.OCSP_OPTIONAL_CRL:
            if (BasicOCSPResponse != null)
              continue;
            goto case OCSPType.CRL;
          default:
            continue;
        }
      }
    }
    if (this.ExternalSigner != null && this.CRLBytes == null && crlCollection.Count > 0)
      this.m_crlBytes = (ICollection<byte[]>) crlCollection;
    return this.GetDssDetails(crlCollection, ocspCollection);
  }

  private List<byte[]> GetOCSPData(Ocsp ocsp, List<X509Certificate> certificates)
  {
    List<byte[]> ocspData = new List<byte[]>();
    for (int index = 0; index < certificates.Count; ++index)
    {
      X509Certificate certificate = certificates[index];
      if (ocsp != null)
      {
        byte[] encodedOcspRspnose = ocsp.GetEncodedOcspRspnose(certificate, this.GetRoot(certificate, certificates), (string) null);
        if (encodedOcspRspnose != null)
          ocspData.Add(PdfSignature.BuildOCSPResponse(encodedOcspRspnose));
      }
    }
    return ocspData;
  }

  private bool GetDSSDetails(
    List<X509Certificate> certificates,
    List<byte[]> ocspCollection,
    RevocationList crl)
  {
    List<byte[]> crlCollection = new List<byte[]>();
    for (int index = 0; index < certificates.Count; ++index)
    {
      X509Certificate certificate = certificates[index];
      if (crl != null && certificate != null)
      {
        ICollection<byte[]> encoded = crl.GetEncoded(certificate, (string) null);
        if (encoded != null)
        {
          foreach (byte[] b in (IEnumerable<byte[]>) encoded)
          {
            bool flag = false;
            foreach (byte[] a in crlCollection)
            {
              if (Asn1Constants.AreEqual(a, b))
              {
                flag = true;
                break;
              }
            }
            if (!flag)
              crlCollection.Add(b);
          }
        }
      }
    }
    if (this.ExternalSigner != null && this.CRLBytes == null)
      this.m_crlBytes = (ICollection<byte[]>) crlCollection;
    return this.GetDssDetails(crlCollection, ocspCollection);
  }

  private bool GetDssDetails(List<byte[]> crlCollection, List<byte[]> ocspCollection)
  {
    if (crlCollection.Count == 0 && ocspCollection.Count == 0)
      return false;
    if (this.m_document != null && this.m_document.Catalog != null && this.m_document.Catalog.ContainsKey("DSS"))
      this.m_dssDictionary = PdfCrossTable.Dereference(this.m_document.Catalog["DSS"]) as PdfDictionary;
    if (this.m_dssDictionary == null)
      this.m_dssDictionary = new PdfDictionary();
    PdfArray pdfArray1 = new PdfArray();
    PdfArray pdfArray2 = new PdfArray();
    if (this.m_dssDictionary.ContainsKey("OCSPs") && PdfCrossTable.Dereference(this.m_dssDictionary["OCSPs"]) is PdfArray pdfArray3)
      pdfArray1 = pdfArray3;
    if (this.m_dssDictionary.ContainsKey("CRLs") && PdfCrossTable.Dereference(this.m_dssDictionary["CRLs"]) is PdfArray pdfArray4)
      pdfArray2 = pdfArray4;
    PdfDictionary pdfDictionary1 = new PdfDictionary();
    if (this.m_dssDictionary.ContainsKey("VRI") && PdfCrossTable.Dereference(this.m_dssDictionary["VRI"]) is PdfDictionary pdfDictionary2)
      pdfDictionary1 = pdfDictionary2;
    for (int index = 0; index < ocspCollection.Count; ++index)
    {
      PdfStream pdfStream = new PdfStream(new PdfDictionary(), ocspCollection[index]);
      pdfArray1.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfStream));
    }
    for (int index = 0; index < crlCollection.Count; ++index)
    {
      PdfStream pdfStream = new PdfStream(new PdfDictionary(), crlCollection[index]);
      pdfArray2.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfStream));
    }
    pdfDictionary1.Items.Add(new PdfName(this.GetVRIName().ToUpper()), (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) new PdfDictionary()
    {
      Items = {
        {
          new PdfName("OCSPs"),
          (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfArray1)
        },
        {
          new PdfName("CRLs"),
          (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfArray2)
        }
      }
    }));
    pdfDictionary1.Modify();
    this.m_dssDictionary.Items[new PdfName("OCSPs")] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfArray1);
    this.m_dssDictionary.Items[new PdfName("CRLs")] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfArray2);
    this.m_dssDictionary.Items[new PdfName("VRI")] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary1);
    return true;
  }

  private string GetVRIName()
  {
    byte[] bytes = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());
    byte[] hash = SHA1.Create().ComputeHash(bytes);
    StringBuilder stringBuilder = new StringBuilder();
    foreach (byte num in hash)
      stringBuilder.AppendFormat("{0:x2}", (object) num);
    return stringBuilder.ToString();
  }

  private static byte[] BuildOCSPResponse(byte[] BasicOCSPResponse)
  {
    DerOctet derOctet = new DerOctet(BasicOCSPResponse);
    return new DerSequence(new Asn1EncodeCollection(new Asn1Encode[0])
    {
      new Asn1Encode[1]{ (Asn1Encode) new DerCatalogue(0) },
      new Asn1Encode[1]
      {
        (Asn1Encode) new DerTag(true, 0, (Asn1Encode) new DerSequence(new Asn1EncodeCollection(new Asn1Encode[0])
        {
          new Asn1Encode[1]{ (Asn1Encode) OcspConstants.OcspBasic },
          new Asn1Encode[1]{ (Asn1Encode) derOctet }
        }))
      }
    }).GetEncoded();
  }

  private X509Certificate GetRoot(X509Certificate cert, List<X509Certificate> certs)
  {
    for (int index = 0; index < certs.Count; ++index)
    {
      X509Certificate cert1 = certs[index];
      if (cert.IssuerDN.Equals((object) cert1.SubjectDN))
      {
        try
        {
          cert.Verify(cert1.GetPublicKey());
          return cert1;
        }
        catch
        {
        }
      }
    }
    return (X509Certificate) null;
  }

  private byte[] GetEncoded(X509Certificate checkCert, X509Certificate rootCert, string url)
  {
    try
    {
      X509RevocationResponse basicOcspResponse = this.GetBasicOCSPResponse(checkCert, rootCert, url);
      if (basicOcspResponse != null)
      {
        OneTimeResponse[] responses = basicOcspResponse.Responses;
        if (responses.Length == 1)
        {
          if (responses[0].CertificateStatus == CerificateStatus.Good)
            return basicOcspResponse.EncodedBytes;
        }
      }
    }
    catch (Exception ex)
    {
      return (byte[]) null;
    }
    return (byte[]) null;
  }

  internal X509RevocationResponse GetBasicOCSPResponse(
    X509Certificate checkCert,
    X509Certificate rootCert,
    string url)
  {
    OcspResponseHelper ocspResponse = this.GetOcspResponse(checkCert, rootCert, url);
    if (ocspResponse == null)
      return (X509RevocationResponse) null;
    return ocspResponse.Status != 0 ? (X509RevocationResponse) null : (X509RevocationResponse) ocspResponse.GetResponseObject();
  }

  private OcspResponseHelper GetOcspResponse(
    X509Certificate checkCert,
    X509Certificate rootCert,
    string url)
  {
    if (checkCert == null || rootCert == null)
      return (OcspResponseHelper) null;
    if (url == null)
      url = new CertificateUtililty().GetOcspUrl(checkCert);
    if (url == null)
      return (OcspResponseHelper) null;
    byte[] encoded = PdfSignature.GenerateOCSPRequest(rootCert, checkCert.SerialNumber).GetEncoded();
    HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(url);
    httpWebRequest.ContentLength = (long) encoded.Length;
    httpWebRequest.ContentType = "application/ocsp-request";
    httpWebRequest.Accept = "application/ocsp-response";
    httpWebRequest.Method = "POST";
    try
    {
      Stream requestStream = httpWebRequest.GetRequestStream();
      requestStream.Write(encoded, 0, encoded.Length);
      requestStream.Close();
    }
    catch (Exception ex)
    {
      throw new Exception(ex.Message);
    }
    HttpWebResponse response = (HttpWebResponse) httpWebRequest.GetResponse();
    Stream responseStream = response.GetResponseStream();
    OcspResponseHelper ocspResponse = new OcspResponseHelper(responseStream);
    responseStream.Close();
    response.Close();
    return ocspResponse;
  }

  private static OcspRequestHelper GenerateOCSPRequest(
    X509Certificate issuerCert,
    Number serialNumber)
  {
    CertificateIdentity id = new CertificateIdentity("1.3.14.3.2.26", issuerCert, serialNumber);
    OcspRequestCreator ocspRequestCreator = new OcspRequestCreator();
    ocspRequestCreator.AddRequest(id);
    ocspRequestCreator.SetRequestExtensions(new X509Extensions((IDictionary) new Dictionary<DerObjectID, X509Extension>()
    {
      {
        OcspConstants.OcspNonce,
        new X509Extension(false, (Asn1Octet) new DerOctet(new DerOctet(PdfEncryption.CreateDocumentId()).GetEncoded()))
      }
    }));
    return ocspRequestCreator.Generate();
  }

  private static Asn1 GetExtensionValue(X509Certificate cert, string oid)
  {
    byte[] derEncoded = cert.GetExtension(new DerObjectID(oid)).GetDerEncoded();
    return derEncoded == null ? (Asn1) null : new Asn1Stream((Stream) new MemoryStream(((Asn1Octet) new Asn1Stream((Stream) new MemoryStream(derEncoded)).ReadAsn1()).GetOctets())).ReadAsn1();
  }

  internal void EnableExternalLTV()
  {
    if (this.m_externalChain == null)
      return;
    if (this.OCSP != null && this.m_isExternalOCSP)
      this.GetLTVData(this.m_externalChain, new List<byte[]>()
      {
        this.OCSP
      }, (List<byte[]>) null);
    else
      this.GetLTVData(this.m_externalChain, (List<byte[]>) null, (List<byte[]>) null);
  }

  private void Catalog_BeginSave(object sender, SavePdfPrimitiveEventArgs ars)
  {
    if (!this.m_certeficated)
      return;
    if (!(PdfCrossTable.Dereference(this.m_document.Catalog["Perms"]) is PdfDictionary pdfDictionary))
    {
      this.m_document.Catalog["Perms"] = (IPdfPrimitive) new PdfDictionary()
      {
        ["DocMDP"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_signatureDictionary)
      };
    }
    else
    {
      if (pdfDictionary.ContainsKey("DocMDP"))
        return;
      pdfDictionary.SetProperty("DocMDP", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_signatureDictionary));
    }
  }

  private void Dictionary_BeginSave(object sender, SavePdfPrimitiveEventArgs ars)
  {
    if (this.EnableValidationAppearance)
    {
      this.SetValidationApperance();
      if (this.m_field != null && !this.m_field.Dictionary.ContainsKey("Kids"))
        this.EnableValidationAppearance = false;
    }
    if (this.m_field != null)
      this.m_field.Dictionary.Encrypt = this.m_document.Security.Enabled;
    else
      this.m_sigField.Dictionary.Encrypt = this.m_document.Security.Enabled;
    if (this.m_sigField == null || this.Appearance == null || this.Appearance.GetNormalTemplate() == null)
      return;
    if (this.m_sigField.Dictionary.ContainsKey("Kids"))
    {
      if (!(PdfCrossTable.Dereference(this.m_sigField.Dictionary["Kids"]) is PdfArray pdfArray) || pdfArray.Count <= 0)
        return;
      for (int index = 0; index < pdfArray.Count; ++index)
      {
        if (PdfCrossTable.Dereference(pdfArray[index]) is PdfDictionary pdfDictionary)
          pdfDictionary.SetProperty("AP", (IPdfWrapper) this.Appearance);
      }
    }
    else
      this.m_sigField.Dictionary.SetProperty("AP", (IPdfWrapper) this.Appearance);
  }
}
