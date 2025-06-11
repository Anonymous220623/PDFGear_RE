// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.PdfLoadedSignatureField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using Syncfusion.Pdf.Security;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security.Cryptography.X509Certificates;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

public class PdfLoadedSignatureField : PdfLoadedStyledField
{
  private PdfSignature m_signature;
  private PdfCmsSigner m_pdfCmsSigner;
  private bool m_isSigned;
  private string m_message = string.Empty;
  private List<long> m_skipedObjects;
  private PdfSignatureValidationResult result;
  private bool m_isVerified;

  internal PdfLoadedSignatureField(PdfDictionary dictionary, PdfCrossTable crossTable)
    : base(dictionary, crossTable)
  {
  }

  public bool IsSigned
  {
    get
    {
      if (!this.m_isSigned)
        this.CheckSigned();
      return this.m_isSigned;
    }
  }

  internal PdfCmsSigner CmsSigner
  {
    get
    {
      if (this.m_pdfCmsSigner == null)
        this.InitializeSigner();
      return this.m_pdfCmsSigner;
    }
  }

  public PdfSignature Signature
  {
    get
    {
      if (this.m_signature == null && this.Dictionary.ContainsKey("V"))
        this.SetSignature(this.Dictionary["V"]);
      return this.m_signature;
    }
    set
    {
      this.m_signature = value;
      this.Changed = true;
    }
  }

  internal PdfAppearance Appearance
  {
    get
    {
      if (this.Page != null && this.Widget != null && this.Widget.Page == null && this.Widget.LoadedPage == null)
        this.Widget.SetPage(this.Page);
      return this.Widget.Appearance;
    }
  }

  private void CheckSigned()
  {
    try
    {
      if (this.Signature == null)
        return;
      this.m_isSigned = true;
    }
    catch
    {
      this.m_message = "There are errors in the formatting or information contained in the signature";
    }
  }

  private void SetSignature(IPdfPrimitive signature)
  {
    PdfReferenceHolder pdfReferenceHolder1 = signature as PdfReferenceHolder;
    if (!(pdfReferenceHolder1 != (PdfReferenceHolder) null) || !(pdfReferenceHolder1.Object is PdfDictionary dic))
      return;
    this.m_signature = new PdfSignature();
    this.m_signature.Settings.SignatureField = this;
    this.m_signature.m_document = this.CrossTable.Document;
    string empty = string.Empty;
    if (dic.ContainsKey("SubFilter"))
    {
      PdfName pdfName = PdfCrossTable.Dereference(dic["SubFilter"]) as PdfName;
      if (pdfName != (PdfName) null)
        empty = pdfName.Value;
      if (empty == "ETSI.CAdES.detached")
        this.m_signature.Settings.CryptographicStandard = CryptographicStandard.CADES;
    }
    if (this.CrossTable.Document is PdfDocument)
    {
      if (dic.ContainsKey("Reference") && dic["Reference"] is PdfArray pdfArray && pdfArray.Elements[0] is PdfDictionary element && element.ContainsKey("Data"))
      {
        PdfMainObjectCollection pdfObjects = this.CrossTable.Document.PdfObjects;
        PdfReferenceHolder pdfReferenceHolder2 = element["Data"] as PdfReferenceHolder;
        if (pdfReferenceHolder2 != (PdfReferenceHolder) null && !pdfObjects.ContainsReference(pdfReferenceHolder2.Reference))
        {
          PdfReferenceHolder primitive = new PdfReferenceHolder(pdfObjects.GetObject(pdfReferenceHolder2.Reference.ObjectCollectionIndex));
          element.SetProperty("Data", (IPdfPrimitive) primitive);
        }
      }
      dic.Remove("ByteRange");
      PdfSignatureDictionary signatureDictionary = new PdfSignatureDictionary(this.CrossTable.Document, dic);
      this.Dictionary.Remove("Contents");
      this.Dictionary.Remove("ByteRange");
    }
    else if (this.CrossTable.Document is PdfLoadedDocument && this.m_signature != null && this.m_signature.Certificate == null && dic.ContainsKey("Contents"))
    {
      byte[] rawData = (byte[]) null;
      if (PdfCrossTable.Dereference(dic["Contents"]) is PdfString pdfString)
        rawData = pdfString.Bytes;
      if (rawData != null)
      {
        X509Certificate2 x509Certificate2 = (X509Certificate2) null;
        try
        {
          x509Certificate2 = new X509Certificate2(rawData);
        }
        catch (Exception ex)
        {
          PdfCmsSigner cmsSigner = this.CmsSigner;
          if (cmsSigner != null && cmsSigner.SignerCertificate != null)
            this.m_signature.Certificate = new PdfCertificate(cmsSigner);
          else
            x509Certificate2 = (X509Certificate2) null;
        }
        if (x509Certificate2 != null)
        {
          PdfCertificate pdfCertificate = new PdfCertificate(x509Certificate2);
          if (pdfCertificate != null)
            this.m_signature.Certificate = pdfCertificate;
        }
      }
    }
    if (dic.ContainsKey("M") && dic["M"] is PdfString)
      this.m_signature.m_signedDate = this.Dictionary.GetDateTime(dic["M"] as PdfString);
    if (dic.ContainsKey("Name") && dic["Name"] is PdfString)
      this.m_signature.m_signedName = (dic["Name"] as PdfString).Value;
    if (dic.ContainsKey("Reason") && PdfCrossTable.Dereference(dic["Reason"]) is PdfString pdfString1)
      this.m_signature.Reason = pdfString1.Value;
    if (dic.ContainsKey("Location") && PdfCrossTable.Dereference(dic["Location"]) is PdfString pdfString2)
      this.m_signature.LocationInfo = pdfString2.Value;
    if (dic.ContainsKey("ContactInfo") && PdfCrossTable.Dereference(dic["ContactInfo"]) is PdfString pdfString3)
      this.m_signature.ContactInfo = pdfString3.Value;
    if (!dic.ContainsKey("ByteRange"))
      return;
    this.m_signature.ByteRange = dic["ByteRange"] as PdfArray;
    if (this.CrossTable.DocumentCatalog != null)
    {
      PdfDictionary documentCatalog = this.CrossTable.DocumentCatalog;
      bool flag1 = false;
      if (documentCatalog.ContainsKey("Perms"))
      {
        IPdfPrimitive pdfPrimitive1 = documentCatalog["Perms"];
        if (((object) (pdfPrimitive1 as PdfReferenceHolder) != null ? (pdfPrimitive1 as PdfReferenceHolder).Object : pdfPrimitive1) is PdfDictionary pdfDictionary1 && pdfDictionary1.ContainsKey("DocMDP"))
        {
          IPdfPrimitive pdfPrimitive2 = pdfDictionary1["DocMDP"];
          if (((object) (pdfPrimitive2 as PdfReferenceHolder) != null ? (pdfPrimitive2 as PdfReferenceHolder).Object : pdfPrimitive2) is PdfDictionary pdfDictionary && pdfDictionary.ContainsKey("ByteRange"))
          {
            PdfArray pdfArray = PdfCrossTable.Dereference(pdfDictionary["ByteRange"]) as PdfArray;
            bool flag2 = true;
            if (pdfArray != null && this.m_signature != null && this.m_signature.ByteRange != null)
            {
              for (int index = 0; index < pdfArray.Count; ++index)
              {
                PdfNumber pdfNumber1 = pdfArray[index] as PdfNumber;
                PdfNumber pdfNumber2 = this.m_signature.ByteRange[index] as PdfNumber;
                if (pdfNumber1 != null && pdfNumber2 != null && pdfNumber1.LongValue != pdfNumber2.LongValue)
                {
                  flag2 = false;
                  break;
                }
              }
            }
            flag1 = flag2;
          }
        }
      }
      if (flag1 && dic.ContainsKey("Reference"))
      {
        IPdfPrimitive element = dic["Reference"];
        if (element is PdfArray)
          element = (element as PdfArray).Elements[0];
        if (((object) (element as PdfReferenceHolder) != null ? (element as PdfReferenceHolder).Object : element) is PdfDictionary pdfDictionary2 && pdfDictionary2.ContainsKey("TransformParams"))
        {
          IPdfPrimitive pdfPrimitive = pdfDictionary2["TransformParams"];
          if (((object) (pdfPrimitive as PdfReferenceHolder) != null ? (pdfPrimitive as PdfReferenceHolder).Object : pdfPrimitive) is PdfDictionary pdfDictionary && pdfDictionary.ContainsKey("P"))
          {
            this.m_signature.HasDocumentPermission = true;
            if (!(PdfCrossTable.Dereference(pdfDictionary["P"]) is PdfNumber pdfNumber))
              return;
            this.m_signature.DocumentPermissions = (PdfCertificationFlags) pdfNumber.IntValue;
          }
          else
            this.m_signature.HasDocumentPermission = false;
        }
        else
          this.m_signature.HasDocumentPermission = false;
      }
      else
        this.m_signature.HasDocumentPermission = false;
    }
    else
      this.m_signature.HasDocumentPermission = false;
  }

  private void InitializeSigner()
  {
    try
    {
      if (!this.Dictionary.ContainsKey("V"))
        return;
      PdfReferenceHolder pdfReferenceHolder = this.Dictionary["V"] as PdfReferenceHolder;
      if (!(pdfReferenceHolder != (PdfReferenceHolder) null) || !(pdfReferenceHolder.Object is PdfDictionary pdfDictionary))
        return;
      string empty = string.Empty;
      if (pdfDictionary.ContainsKey("SubFilter"))
      {
        PdfName pdfName = PdfCrossTable.Dereference(pdfDictionary["SubFilter"]) as PdfName;
        if (pdfName != (PdfName) null)
          empty = pdfName.Value;
      }
      if (!pdfDictionary.ContainsKey("Contents"))
        return;
      byte[] numArray = (byte[]) null;
      if (PdfCrossTable.Dereference(pdfDictionary["Contents"]) is PdfString pdfString1)
        numArray = pdfString1.Bytes;
      if (numArray == null)
        return;
      if (empty == "adbe.x509.rsa_sha1" && pdfDictionary.ContainsKey("Cert"))
      {
        if (!(PdfCrossTable.Dereference(pdfDictionary["Cert"]) is PdfString pdfString3))
        {
          if (PdfCrossTable.Dereference(pdfDictionary["Cert"]) is PdfArray pdfArray && pdfArray.Count > 0 && PdfCrossTable.Dereference(pdfArray[0]) is PdfString pdfString2)
            this.m_pdfCmsSigner = new PdfCmsSigner(numArray, pdfString2.Bytes);
        }
        else
          this.m_pdfCmsSigner = new PdfCmsSigner(numArray, pdfString3.Bytes);
      }
      else
        this.m_pdfCmsSigner = new PdfCmsSigner(numArray, empty);
      this.UpdateByteRange(this.m_pdfCmsSigner, pdfDictionary["ByteRange"] as PdfArray);
    }
    catch
    {
      this.m_message = "There are errors in the formatting or information contained in the signature";
    }
  }

  internal override void BeginSave()
  {
    base.BeginSave();
    if (this.m_signature == null || this.m_signature.Certificate == null || this.m_signature.Certificated)
      return;
    this.Dictionary["V"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) new PdfSignatureDictionary(this.CrossTable.Document, this.m_signature, this.m_signature.Certificate));
  }

  internal new PdfField Clone(PdfDictionary dictionary, PdfPage page)
  {
    PdfCrossTable crossTable = page.Section.ParentDocument.CrossTable;
    PdfLoadedSignatureField loadedSignatureField = new PdfLoadedSignatureField(dictionary, crossTable);
    loadedSignatureField.Page = (PdfPageBase) page;
    loadedSignatureField.SetName(this.GetFieldName());
    loadedSignatureField.Widget.Dictionary = this.Widget.Dictionary.Clone(crossTable) as PdfDictionary;
    return (PdfField) loadedSignatureField;
  }

  internal override PdfLoadedFieldItem CreateLoadedItem(PdfDictionary dictionary)
  {
    return base.CreateLoadedItem(dictionary);
  }

  internal override void Draw()
  {
    if (!this.Flatten)
      return;
    if (this.Dictionary["AP"] != null)
    {
      if (!(PdfCrossTable.Dereference(this.Dictionary["AP"]) is PdfDictionary pdfDictionary1) || !pdfDictionary1.ContainsKey("N") || !(PdfCrossTable.Dereference(pdfDictionary1["N"]) is PdfDictionary pdfDictionary2) || !(pdfDictionary2 is PdfStream template1))
        return;
      PdfTemplate template2 = new PdfTemplate(template1);
      if (template2 == null || this.Page == null)
        return;
      PdfGraphics graphics = this.Page.Graphics;
      PdfGraphicsState state = graphics.Save();
      if (this.Page.Rotation != PdfPageRotateAngle.RotateAngle0)
      {
        RectangleF templateBounds = this.CalculateTemplateBounds(this.Bounds, this.Page, template2, graphics);
        graphics.DrawPdfTemplate(template2, templateBounds.Location, templateBounds.Size);
      }
      else
        graphics.DrawPdfTemplate(template2, this.Bounds.Location, this.Bounds.Size);
      graphics.Restore(state);
    }
    else
    {
      PdfPageBase page = this.Page;
      PdfGraphicsState state = page.Graphics.Save();
      PdfPen pdfPen = new PdfPen(this.BorderColor, this.BorderWidth);
      PdfBrush backBrush = (PdfBrush) new PdfSolidBrush(this.GetBackColor());
      PdfLoadedStyledField.GraphicsProperties graphicsProperties = new PdfLoadedStyledField.GraphicsProperties((PdfLoadedStyledField) this);
      PaintParams paintParams = new PaintParams(graphicsProperties.Rect, backBrush, graphicsProperties.ForeBrush, graphicsProperties.Pen, graphicsProperties.Style, graphicsProperties.BorderWidth, graphicsProperties.ShadowBrush, graphicsProperties.RotationAngle);
      FieldPainter.DrawSignature(page.Graphics, paintParams);
      page.Graphics.Restore(state);
    }
  }

  private RectangleF CalculateTemplateBounds(
    RectangleF bounds,
    PdfPageBase page,
    PdfTemplate template,
    PdfGraphics graphics)
  {
    float x = bounds.X;
    float y = bounds.Y;
    float width = bounds.Width;
    float height = bounds.Height;
    if (page != null)
    {
      int graphicsRotation = this.ObtainGraphicsRotation(graphics.Matrix);
      if (page is PdfLoadedPage)
      {
        switch (graphicsRotation)
        {
          case 90:
            graphics.TranslateTransform(template.Height, 0.0f);
            graphics.RotateTransform(90f);
            x = bounds.X;
            y = (float) -((double) page.Size.Height - (double) bounds.Y - (double) bounds.Height);
            break;
          case 180:
            graphics.TranslateTransform(template.Width, template.Height);
            graphics.RotateTransform(180f);
            x = (float) -((double) page.Size.Width - ((double) bounds.X + (double) template.Width));
            y = (float) -((double) page.Size.Height - (double) bounds.Y - (double) template.Height);
            break;
          case 270:
            graphics.TranslateTransform(0.0f, template.Width);
            graphics.RotateTransform(270f);
            x = (float) -((double) page.Size.Width - (double) bounds.X - (double) bounds.Width);
            y = bounds.Y;
            break;
        }
      }
    }
    return new RectangleF(x, y, width, height);
  }

  internal int ObtainGraphicsRotation(PdfTransformationMatrix matrix)
  {
    int graphicsRotation = (int) Math.Round(Math.Atan2((double) matrix.Matrix.Elements[2], (double) matrix.Matrix.Elements[0]) * 180.0 / Math.PI);
    switch (graphicsRotation)
    {
      case -180:
        graphicsRotation = 180;
        break;
      case -90:
        graphicsRotation = 90;
        break;
      case 90:
        graphicsRotation = 270;
        break;
    }
    return graphicsRotation;
  }

  private bool CheckCertificateValidity(DateTime date, DateTime validFrom, DateTime validTo)
  {
    return date.CompareTo(validTo) < 0;
  }

  public PdfSignatureValidationResult ValidateSignature()
  {
    return this.ValidateSignature(new PdfSignatureValidationOptions());
  }

  public PdfSignatureValidationResult ValidateSignature(PdfSignatureValidationOptions options)
  {
    if (!this.m_isVerified)
    {
      this.m_isVerified = true;
      this.result = this.Validate(options);
      if (this.result != null)
      {
        if (this.result.IsDocumentModified)
        {
          this.result.SignatureStatus = SignatureStatus.Invalid;
        }
        else
        {
          bool flag1 = this.result.IsValidAtCurrentTime || this.result.IsValidAtSignedTime || this.result.IsValidAtTimeStampTime;
          bool flag2 = false;
          RevocationResult revocationResult = this.result.RevocationResult;
          if (revocationResult != null && (revocationResult.OcspRevocationStatus == RevocationStatus.Good || revocationResult.OcspRevocationStatus == RevocationStatus.None) && !revocationResult.IsRevokedCRL)
            flag2 = true;
          if (flag2 && flag1)
          {
            this.result.SignatureStatus = SignatureStatus.Unknown;
            this.result.IsSignatureValid = true;
          }
          else if (options != null && !options.ValidateRevocationStatus && flag1)
          {
            this.result.SignatureStatus = SignatureStatus.Unknown;
            this.result.IsSignatureValid = true;
          }
          else
            this.result.SignatureStatus = SignatureStatus.Invalid;
        }
      }
      else if (this.m_message != string.Empty)
      {
        this.result = new PdfSignatureValidationResult();
        this.result.SignatureStatus = SignatureStatus.Invalid;
        this.result.SignatureValidationErrors.Add(new PdfSignatureValidationException(this.m_message));
      }
    }
    return this.result;
  }

  private PdfSignatureValidationResult Validate(PdfSignatureValidationOptions options)
  {
    if (!this.IsSigned || this.CmsSigner == null)
      return (PdfSignatureValidationResult) null;
    PdfSignatureValidationResult result = new PdfSignatureValidationResult();
    result.SignatureName = this.GetFieldName();
    if (this.m_signature.Settings != null)
    {
      result.DigestAlgorithm = this.Signature.Settings.DigestAlgorithm;
      result.CryptographicStandard = this.Signature.Settings.CryptographicStandard;
    }
    result.IsCertificated = this.m_signature.Certificated;
    result.SignatureAlgorithm = this.m_pdfCmsSigner.EncryptionAlgorithm;
    result.Certificates = this.GetCertficates();
    if (!this.VerifyChecksum())
    {
      result.IsDocumentModified = true;
      result.SignatureValidationErrors.Add(new PdfSignatureValidationException("The document has been altered or corrupted since the signature was applied"));
      return result;
    }
    if (this.CheckIncrementUpdate())
    {
      result.IsDocumentModified = true;
      result.SignatureValidationErrors.Add(new PdfSignatureValidationException("The document has been altered or corrupted since the signature was applied"));
      return result;
    }
    result.IsDocumentModified = false;
    try
    {
      if (options != null)
      {
        if (options.ValidateRevocationStatus)
          result.RevocationResult = this.ValidateRevocation(result);
      }
    }
    catch (Exception ex)
    {
      result.SignatureValidationErrors.Add(new PdfSignatureValidationException(ex.Message));
      result.RevocationResult = (RevocationResult) null;
    }
    if (result.Certificates.Count > 0)
    {
      X509Certificate2 certificate = result.Certificates[0];
      DateTime notBefore = certificate.NotBefore;
      DateTime notAfter = certificate.NotAfter;
      if (this.CheckCertificateValidity(this.m_signature.SignedDate, notBefore, notAfter))
        result.IsValidAtSignedTime = true;
      else if (!result.m_isValidOCSPorCRLtimeValidation)
        result.SignatureValidationErrors.Add(new PdfSignatureValidationException("The signature is not valid at signing date. Signing time is from the clock on the signer's computer"));
      if (this.CheckCertificateValidity(DateTime.Now, notBefore, notAfter))
        result.IsValidAtCurrentTime = true;
      else if (!result.m_isValidOCSPorCRLtimeValidation)
        result.SignatureValidationErrors.Add(new PdfSignatureValidationException("The signature is not valid at current date."));
      result.TimeStampInformation = this.VerifyTimeStamp();
      if (result.TimeStampInformation != null && result.TimeStampInformation.IsValid)
      {
        if (this.CheckCertificateValidity(result.TimeStampInformation.Time, notBefore, notAfter))
          result.IsValidAtTimeStampTime = true;
        else
          result.SignatureValidationErrors.Add(new PdfSignatureValidationException("The signature includes an embedded timestamp but it could not be verified."));
      }
    }
    return result;
  }

  public PdfSignatureValidationResult ValidateSignature(X509CertificateCollection rootCertificates)
  {
    PdfSignatureValidationOptions options = new PdfSignatureValidationOptions();
    return this.ValidateSignature(rootCertificates, options);
  }

  public PdfSignatureValidationResult ValidateSignature(
    X509CertificateCollection rootCertificates,
    PdfSignatureValidationOptions options)
  {
    PdfSignatureValidationResult signatureResult = this.ValidateSignature(options);
    if (signatureResult != null)
    {
      List<Syncfusion.Pdf.Security.X509Certificate> collection = new List<Syncfusion.Pdf.Security.X509Certificate>();
      X509CertificateParser certificateParser = new X509CertificateParser();
      foreach (X509Certificate2 rootCertificate in rootCertificates)
        collection.Add(certificateParser.ReadCertificate(rootCertificate.RawData));
      if (this.m_pdfCmsSigner.ValidateCertificateWithCollection(collection, this.Signature.SignedDate, signatureResult) && signatureResult.SignatureStatus == SignatureStatus.Unknown)
        signatureResult.SignatureStatus = SignatureStatus.Valid;
    }
    return signatureResult;
  }

  private X509Certificate2Collection GetCertficates()
  {
    return this.m_pdfCmsSigner != null ? this.m_pdfCmsSigner.GetCertificates() : (X509Certificate2Collection) null;
  }

  private bool VerifyChecksum()
  {
    bool flag = false;
    if (this.m_pdfCmsSigner != null)
      flag = this.m_pdfCmsSigner.ValidateChecksum();
    return flag;
  }

  private bool CheckIncrementUpdate()
  {
    PdfDictionary trailer = this.CrossTable.Trailer;
    if (trailer != null && !trailer.ContainsKey("Prev"))
      return false;
    this.m_skipedObjects = new List<long>();
    System.Collections.Generic.Dictionary<long, List<ObjectInformation>> allTables = this.CrossTable.CrossTable.AllTables;
    PdfArray byteRange = this.Signature.ByteRange;
    long[] numArray = new long[4];
    if (byteRange != null && byteRange.Count > 3)
    {
      if (byteRange[0] is PdfNumber pdfNumber1)
        numArray[0] = pdfNumber1.LongValue;
      if (byteRange[1] is PdfNumber pdfNumber2)
        numArray[1] = pdfNumber2.LongValue;
      if (byteRange[2] is PdfNumber pdfNumber3)
        numArray[2] = pdfNumber3.LongValue;
      if (byteRange[3] is PdfNumber pdfNumber4)
        numArray[3] = pdfNumber4.LongValue;
    }
    bool flag1 = false;
    List<long> skipObjects = new List<long>();
    PdfDictionary documentCatalog = this.CrossTable.DocumentCatalog;
    if (documentCatalog != null)
    {
      System.Collections.Generic.Dictionary<PdfName, IPdfPrimitive> items = documentCatalog.Items;
      PdfName key1 = new PdfName("AcroForm");
      if (items.ContainsKey(key1))
      {
        IPdfPrimitive pdfPrimitive = items[key1];
        PdfReferenceHolder pdfReferenceHolder = pdfPrimitive as PdfReferenceHolder;
        if (pdfReferenceHolder != (PdfReferenceHolder) null)
          skipObjects.Add(pdfReferenceHolder.Reference.ObjNum);
        else if (pdfPrimitive is PdfDictionary formDictionary)
          this.ReadAllReferences(formDictionary);
      }
      PdfName key2 = new PdfName("DSS");
      if (items.ContainsKey(key2))
        this.ReadAllSubReferences(items[key2]);
    }
    if (trailer != null && trailer.ContainsKey("Info"))
      this.ReadAllSubReferences(trailer["Info"]);
    foreach (long key3 in allTables.Keys)
    {
      if (!flag1)
      {
        List<ObjectInformation> objectInformationList = allTables[key3];
        System.Collections.Generic.Dictionary<long, ObjectInformation> dictionary1 = new System.Collections.Generic.Dictionary<long, ObjectInformation>();
        System.Collections.Generic.Dictionary<long, ObjectInformation> dictionary2 = new System.Collections.Generic.Dictionary<long, ObjectInformation>();
        foreach (ObjectInformation objectInformation in objectInformationList)
        {
          long key4 = objectInformation.Archive == null ? objectInformation.Offset : (long) (objectInformation.Archive.Archive["First"] as PdfNumber).IntValue;
          if (numArray[0] <= key4 && numArray[1] >= key4 || numArray[2] <= key4 && numArray[2] + numArray[3] >= key4)
            dictionary1[key4] = objectInformation;
          else
            dictionary2[key4] = objectInformation;
        }
        if (this.Signature.HasDocumentPermission && this.Signature.DocumentPermissions == PdfCertificationFlags.ForbidChanges && dictionary2.Count > 0)
        {
          flag1 = true;
          break;
        }
        ObjectInformation objInfo1 = objectInformationList[0];
        bool flag2 = false;
        if (objInfo1 != null)
        {
          PdfDictionary dictionary3 = this.ReadDictionary(objInfo1);
          if (dictionary3 != null)
          {
            if (dictionary3.ContainsKey("Type") && (dictionary3["Type"] as PdfName).Value == "Catalog")
              flag2 = true;
            if (!flag2 && !this.m_skipedObjects.Contains(key3))
            {
              if (objectInformationList.Count == dictionary1.Count)
                flag1 = false;
              else if (objectInformationList.Count == dictionary2.Count)
              {
                if (dictionary3.ContainsKey("Type"))
                {
                  PdfName pdfName = dictionary3["Type"] as PdfName;
                  if (pdfName.Value == "Sig" || pdfName.Value == "DocTimeStamp")
                    flag1 = false;
                  else if (pdfName.Value == "Page")
                    flag1 = true;
                  else if (pdfName.Value == "Annot")
                    flag1 = !this.CheckSubType(dictionary3);
                }
                else if (dictionary3.ContainsKey("Subtype"))
                  flag1 = !this.CheckSubType(dictionary3);
                else if (dictionary3 is PdfStream)
                  flag1 = true;
              }
              else
              {
                ObjectInformation objInfo2 = (ObjectInformation) null;
                using (System.Collections.Generic.Dictionary<long, ObjectInformation>.KeyCollection.Enumerator enumerator = dictionary1.Keys.GetEnumerator())
                {
                  if (enumerator.MoveNext())
                  {
                    long current = enumerator.Current;
                    objInfo2 = dictionary1[current];
                  }
                }
                if (objInfo2 != null)
                {
                  PdfDictionary dictionaryObj = this.ReadDictionary(objInfo2);
                  if (dictionaryObj != null)
                  {
                    if (dictionaryObj.ContainsKey("Type"))
                    {
                      PdfName pdfName = dictionaryObj["Type"] as PdfName;
                      if (pdfName != (PdfName) null && pdfName.Value == "Page" && dictionaryObj.ContainsKey("Annots") && PdfCrossTable.Dereference(dictionaryObj["Annots"]) is PdfArray oldAnnots && dictionary3.ContainsKey("Annots") && PdfCrossTable.Dereference(dictionary3["Annots"]) is PdfArray newAnnots)
                        flag1 = this.CheckFormFieldRemoved(oldAnnots, newAnnots);
                    }
                    if (!flag1)
                      flag1 = this.CompareObjects(dictionaryObj, skipObjects, key3, dictionary3);
                  }
                }
              }
            }
          }
        }
      }
      else
        break;
    }
    return flag1;
  }

  private PdfDictionary ReadDictionary(ObjectInformation objInfo)
  {
    PdfDictionary pdfDictionary = (PdfDictionary) null;
    if (objInfo.Type == ObjectType.Normal)
    {
      if (objInfo.Obj != null)
      {
        pdfDictionary = objInfo.Obj as PdfDictionary;
      }
      else
      {
        PdfReader reader = this.CrossTable.CrossTable.Reader;
        long position = reader.Position;
        reader.Position = (long) objInfo;
        PdfParser pdfParser = new PdfParser(this.CrossTable.CrossTable, reader, this.CrossTable);
        for (int index = 0; index < 4; ++index)
          pdfParser.Advance();
        if (pdfParser.GetNext() == Syncfusion.Pdf.IO.TokenType.DictionaryStart)
          pdfDictionary = pdfParser.Dictionary() as PdfDictionary;
        reader.Position = position;
      }
    }
    else if (objInfo.Type == ObjectType.Packed)
    {
      if (objInfo.Obj != null)
        pdfDictionary = objInfo.Obj as PdfDictionary;
      else if (objInfo.Archive != null && objInfo.Parser != null)
      {
        PdfParser parser = objInfo.Parser;
        long position = parser.Position;
        parser.StartFrom(objInfo.Offset);
        if (parser.GetNext() == Syncfusion.Pdf.IO.TokenType.DictionaryStart)
          pdfDictionary = parser.Dictionary() as PdfDictionary;
        parser.SetOffset(position);
      }
    }
    return pdfDictionary;
  }

  private bool CheckFormFieldRemoved(PdfArray oldAnnots, PdfArray newAnnots)
  {
    bool flag = false;
    foreach (IPdfPrimitive element in oldAnnots.Elements)
    {
      if (!newAnnots.Contains(element))
      {
        PdfReferenceHolder pdfReferenceHolder = element as PdfReferenceHolder;
        if (pdfReferenceHolder != (PdfReferenceHolder) null)
        {
          IPdfPrimitive pdfPrimitive = pdfReferenceHolder.Object;
          if (((object) (pdfPrimitive as PdfReferenceHolder) != null ? (pdfPrimitive as PdfReferenceHolder).Object : pdfPrimitive) is PdfDictionary pdfDictionary && pdfDictionary.ContainsKey("Subtype") && (pdfDictionary["Subtype"] as PdfName).Value == "Widget")
          {
            flag = true;
            break;
          }
        }
      }
    }
    return flag;
  }

  private bool CheckSubType(PdfDictionary dictionary)
  {
    bool flag = false;
    PdfName pdfName1 = dictionary["Subtype"] as PdfName;
    if (pdfName1 != (PdfName) null && pdfName1.Value == "Widget" && dictionary.ContainsKey("FT"))
    {
      PdfName pdfName2 = PdfCrossTable.Dereference(dictionary["FT"]) as PdfName;
      flag = pdfName2 != (PdfName) null && pdfName2.Value == "Sig";
    }
    else if (pdfName1 != (PdfName) null && this.IsAnnotation(pdfName1.Value) && (!this.Signature.HasDocumentPermission || this.Signature.HasDocumentPermission && this.Signature.DocumentPermissions == PdfCertificationFlags.AllowComments))
      flag = true;
    else if (pdfName1 != (PdfName) null && pdfName1.Value == "Form")
      flag = true;
    return flag;
  }

  private bool CheckSubType(PdfDictionary newer, PdfDictionary older)
  {
    bool flag = false;
    PdfName pdfName = newer["Subtype"] as PdfName;
    if (pdfName != (PdfName) null && pdfName.Value == "Widget")
      flag = true;
    else if (pdfName != (PdfName) null && this.IsAnnotation(pdfName.Value))
      flag = !this.Signature.HasDocumentPermission || this.Signature.HasDocumentPermission && this.Signature.DocumentPermissions == PdfCertificationFlags.AllowComments || this.AreEqual(newer, older, false);
    return flag;
  }

  private bool CompareObjects(
    PdfDictionary dictionaryObj,
    List<long> skipObjects,
    long key,
    PdfDictionary dictionary)
  {
    bool flag = false;
    if (dictionaryObj != null)
      flag = skipObjects.Count <= 0 || !skipObjects.Contains(key) ? !this.AreEqual(dictionaryObj, dictionary, true) : this.ReadFormReferences(dictionaryObj, dictionary);
    return flag;
  }

  private bool ReadFormReferences(PdfDictionary dictionaryObj, PdfDictionary dictionary)
  {
    bool flag = false;
    if (dictionaryObj.ContainsKey("Fields") && dictionary.ContainsKey("Fields"))
    {
      PdfArray pdfArray1 = dictionaryObj["Fields"] as PdfArray;
      PdfArray pdfArray2 = dictionary["Fields"] as PdfArray;
      if (pdfArray1 != null && pdfArray2 != null)
      {
        if (pdfArray1.Count == pdfArray2.Count)
        {
          foreach (IPdfPrimitive element in pdfArray2)
          {
            if (!pdfArray1.Contains(element))
            {
              flag = true;
              break;
            }
            PdfReferenceHolder pdfReferenceHolder = element as PdfReferenceHolder;
            if (pdfReferenceHolder != (PdfReferenceHolder) null && pdfReferenceHolder.Object is PdfDictionary formDictionary)
              this.ReadAllReferences(formDictionary);
          }
        }
        else if (pdfArray2.Count > pdfArray1.Count)
        {
          foreach (IPdfPrimitive element in pdfArray2)
          {
            PdfReferenceHolder pdfReferenceHolder = element as PdfReferenceHolder;
            if (pdfReferenceHolder != (PdfReferenceHolder) null)
            {
              if (pdfReferenceHolder.Object is PdfDictionary formDictionary && !pdfArray1.Contains(element) && formDictionary.ContainsKey("FT"))
              {
                PdfName pdfName = PdfCrossTable.Dereference(formDictionary["FT"]) as PdfName;
                if (pdfName != (PdfName) null && pdfName.Value != "Sig")
                {
                  flag = true;
                  break;
                }
              }
              if (!flag && formDictionary != null)
                this.ReadAllReferences(formDictionary);
            }
          }
        }
        else
          flag = true;
      }
    }
    return flag;
  }

  private void ReadAllReferences(PdfDictionary formDictionary)
  {
    foreach (PdfName key in formDictionary.Keys)
    {
      if (key.Value != "P" && key.Value != "Parent")
        this.ReadAllSubReferences(formDictionary[key]);
    }
  }

  private void ReadAllSubReferences(IPdfPrimitive primitive)
  {
    if (primitive == null)
      return;
    if ((object) (primitive as PdfReferenceHolder) != null)
    {
      PdfReferenceHolder pdfReferenceHolder = primitive as PdfReferenceHolder;
      if (!(pdfReferenceHolder.Reference != (PdfReference) null) || this.m_skipedObjects.Contains(pdfReferenceHolder.Reference.ObjNum))
        return;
      this.m_skipedObjects.Add(pdfReferenceHolder.Reference.ObjNum);
      this.ReadAllSubReferences(pdfReferenceHolder.Object);
    }
    else
    {
      switch (primitive)
      {
        case PdfDictionary _:
          this.ReadAllReferences(primitive as PdfDictionary);
          break;
        case PdfArray _:
          PdfArray pdfArray = primitive as PdfArray;
          if (pdfArray.Count <= 0)
            break;
          using (List<IPdfPrimitive>.Enumerator enumerator = pdfArray.Elements.GetEnumerator())
          {
            while (enumerator.MoveNext())
              this.ReadAllSubReferences(enumerator.Current);
            break;
          }
      }
    }
  }

  private bool IsAnnotation(string subType)
  {
    bool flag = false;
    switch (subType)
    {
      case "Text":
      case "Link":
      case "FreeText":
      case "Line":
      case "Square":
      case "Circle":
      case "PolyLine":
      case "Polygon":
      case "Highlight":
      case "Underline":
      case "StrikeOut":
      case "Squiggly":
      case "Stamp":
      case "Caret":
      case "Ink":
      case "Popup":
      case "FileAttachment":
      case "Sound":
      case "Movie":
      case "Screen":
      case "PrinterMark":
      case "TrapNet":
      case "Watermark":
      case "U3D":
        flag = true;
        break;
    }
    return flag;
  }

  private bool AreEqual(PdfDictionary older, PdfDictionary newer, bool ignoreAnnotation)
  {
    bool flag1 = true;
    bool flag2 = false;
    if (newer != null && newer is PdfStream && newer.ContainsKey("Subtype") && (PdfCrossTable.Dereference(newer["Subtype"]) as PdfName).Value == "XML")
      flag2 = true;
    if (older != null && newer != null && !flag2)
    {
      PdfName pdfName1 = (PdfName) null;
      if (newer.ContainsKey("Type"))
        pdfName1 = PdfCrossTable.Dereference(newer["Type"]) as PdfName;
      if (ignoreAnnotation && pdfName1 != (PdfName) null && pdfName1.Value == "Annot")
        flag1 = this.CheckSubType(newer, older);
      else if (newer.ContainsKey("FT"))
      {
        PdfName pdfName2 = newer["FT"] as PdfName;
        if (pdfName2 != (PdfName) null)
          flag1 = pdfName2.Value == "Sig" || pdfName2.Value == "Tx" || pdfName2.Value == "Btn" || pdfName2.Value == "Ch";
      }
      else
      {
        foreach (PdfName key in older.Keys)
        {
          if (newer.ContainsKey(key))
          {
            if (key.Value != "Annots")
              flag1 = this.IsEqual(older[key], newer[key]);
            if (!flag1)
              break;
          }
          else
          {
            flag1 = false;
            break;
          }
        }
      }
    }
    return flag1;
  }

  private bool IsEqual(IPdfPrimitive older, IPdfPrimitive newer)
  {
    bool flag = true;
    if ((object) (older as PdfName) != null && (object) (newer as PdfName) != null)
      flag = (older as PdfName).Value == (newer as PdfName).Value;
    else if (older is PdfString && newer is PdfString)
      flag = (older as PdfString).Value == (newer as PdfString).Value;
    else if (older is PdfBoolean && newer is PdfBoolean)
      flag = (older as PdfBoolean).Value == (newer as PdfBoolean).Value;
    else if ((object) (older as PdfReferenceHolder) != null && (object) (newer as PdfReferenceHolder) != null)
    {
      PdfReference reference1 = (older as PdfReferenceHolder).Reference;
      PdfReference reference2 = (newer as PdfReferenceHolder).Reference;
      flag = reference1 == (PdfReference) null && reference2 == (PdfReference) null || reference1 != (PdfReference) null && reference2 != (PdfReference) null && reference1.ObjNum == reference2.ObjNum;
    }
    else if (older is PdfArray && newer is PdfArray)
    {
      PdfArray pdfArray1 = older as PdfArray;
      PdfArray pdfArray2 = newer as PdfArray;
      if (pdfArray1.Count == pdfArray2.Count)
      {
        for (int index = 0; index < pdfArray1.Count; ++index)
        {
          flag = this.IsEqual(pdfArray1[index], pdfArray2[index]);
          if (!flag)
            break;
        }
      }
      else
        flag = false;
    }
    else
      flag = !(older is PdfNumber) || !(newer is PdfNumber) ? older is PdfDictionary && newer is PdfDictionary && this.AreEqual(older as PdfDictionary, newer as PdfDictionary, true) : (double) (older as PdfNumber).FloatValue == (double) (newer as PdfNumber).FloatValue & (older as PdfNumber).IntValue == (newer as PdfNumber).IntValue;
    return flag;
  }

  private RevocationResult ValidateRevocation(PdfSignatureValidationResult result)
  {
    return this.m_pdfCmsSigner != null ? this.m_pdfCmsSigner.CheckRevocation(this.Signature.SignedDate, result) : (RevocationResult) null;
  }

  private TimeStampInformation VerifyTimeStamp()
  {
    if (this.m_signature == null)
      this.m_signature = this.Signature;
    return this.m_pdfCmsSigner != null ? this.m_pdfCmsSigner.ValidateTimestamp() : (TimeStampInformation) null;
  }

  private void UpdateByteRange(PdfCmsSigner pkcs7, PdfArray byteRange)
  {
    if (!(this.CrossTable.Document is PdfLoadedDocument document))
      return;
    Stream stream = document.m_stream;
    byte[] numArray1 = new byte[stream.Length];
    stream.Position = 0L;
    stream.Read(numArray1, 0, (int) stream.Length);
    IRandom source = (IRandom) new RandomArray(numArray1);
    long[] numArray2 = new long[4];
    if (byteRange != null && byteRange.Count > 3)
    {
      if (byteRange[0] is PdfNumber pdfNumber1)
        numArray2[0] = pdfNumber1.LongValue;
      if (byteRange[1] is PdfNumber pdfNumber2)
        numArray2[1] = pdfNumber2.LongValue;
      if (byteRange[2] is PdfNumber pdfNumber3)
        numArray2[2] = pdfNumber3.LongValue;
      if (byteRange[3] is PdfNumber pdfNumber4)
        numArray2[3] = pdfNumber4.LongValue;
    }
    IRandom[] sources = new IRandom[numArray2.Length / 2];
    for (int index = 0; index < numArray2.Length; index += 2)
      sources[index / 2] = (IRandom) new WindowRandom(source, numArray2[index], numArray2[index + 1]);
    RandomStream randomStream = (RandomStream) null;
    try
    {
      randomStream = new RandomStream((IRandom) new RandomGroup((ICollection<IRandom>) sources));
      byte[] numArray3 = new byte[8192 /*0x2000*/];
      int length;
      while ((length = randomStream.Read(numArray3, 0, numArray3.Length)) > 0)
        pkcs7.Update(numArray3, 0, length);
    }
    finally
    {
      randomStream?.Close();
    }
  }
}
