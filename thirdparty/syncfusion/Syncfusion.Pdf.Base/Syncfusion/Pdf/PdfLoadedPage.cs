// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfLoadedPage
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Graphics.Fonts;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using Syncfusion.Pdf.Redaction;
using Syncfusion.Pdf.Security;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;

#nullable disable
namespace Syncfusion.Pdf;

public class PdfLoadedPage : PdfPageBase
{
  private PdfCrossTable m_crossTable;
  private bool m_bCheckResources;
  private PdfDocumentBase m_document;
  private List<PdfDictionary> m_terminalannots = new List<PdfDictionary>();
  private PdfLoadedAnnotationCollection m_annots;
  internal static bool m_annotChanged;
  private List<long> m_widgetReferences;
  private RectangleF m_mediaBox = RectangleF.Empty;
  private SizeF m_size = SizeF.Empty;
  private RectangleF m_cropBox = RectangleF.Empty;
  private RectangleF m_bleedBox = RectangleF.Empty;
  private RectangleF m_trimBox = RectangleF.Empty;
  private RectangleF m_artBox = RectangleF.Empty;
  private PdfResources m_resources;
  private PointF m_origin = PointF.Empty;
  private PdfArray m_annotsReference = new PdfArray();
  internal bool importAnnotation;
  private List<PdfRedaction> m_redactions;

  public new PdfLoadedAnnotationCollection Annotations
  {
    get
    {
      if (this.m_annots == null || this.importAnnotation)
        this.CreateAnnotations(this.GetWidgetReferences());
      return this.m_annots;
    }
    set => this.m_annots = value;
  }

  public List<PdfRedaction> Redactions
  {
    get
    {
      if (this.m_redactions == null)
      {
        (this.m_document as PdfLoadedDocument).m_redactionPages.Add(this);
        this.m_redactions = new List<PdfRedaction>();
      }
      return this.m_redactions;
    }
  }

  public RectangleF MediaBox
  {
    get
    {
      if (this.m_mediaBox.Equals((object) RectangleF.Empty) && this.Dictionary.ContainsKey(nameof (MediaBox)))
      {
        PdfArray pdfArray = this.Dictionary.GetValue(this.CrossTable, nameof (MediaBox), "Parent") as PdfArray;
        float floatValue = (pdfArray[2] as PdfNumber).FloatValue;
        float height = (double) (pdfArray[3] as PdfNumber).FloatValue != 0.0 ? (pdfArray[3] as PdfNumber).FloatValue : (pdfArray[1] as PdfNumber).FloatValue;
        this.m_mediaBox = this.CalculateBounds((pdfArray[0] as PdfNumber).FloatValue, (pdfArray[1] as PdfNumber).FloatValue, floatValue, height);
      }
      return this.m_mediaBox;
    }
  }

  public override SizeF Size
  {
    get
    {
      if (this.m_size == SizeF.Empty)
      {
        PdfArray pdfArray1 = (PdfArray) null;
        PdfArray pdfArray2 = (PdfArray) null;
        if (this.Dictionary != null)
        {
          pdfArray2 = this.Dictionary.GetValue(this.CrossTable, "MediaBox", "Parent") as PdfArray;
          pdfArray1 = this.Dictionary.GetValue(this.CrossTable, "CropBox", "Parent") as PdfArray;
        }
        PdfNumber pdfNumber = (PdfNumber) null;
        if (this.Dictionary.ContainsKey("Rotate"))
          pdfNumber = this.Dictionary.GetValue(this.CrossTable, "Rotate", "Parent") as PdfNumber;
        float width;
        float height;
        if (pdfArray1 != null && pdfNumber != null)
        {
          width = (pdfArray1[2] as PdfNumber).FloatValue - (pdfArray1[0] as PdfNumber).FloatValue;
          height = (pdfArray1[3] as PdfNumber).FloatValue - (pdfArray1[1] as PdfNumber).FloatValue;
          if (((double) pdfNumber.FloatValue == 0.0 || (double) pdfNumber.FloatValue == 180.0) && (double) width < (double) height || ((double) pdfNumber.FloatValue == 90.0 || (double) pdfNumber.FloatValue == 270.0) && (double) width > (double) height)
          {
            width = width;
            height = height;
          }
          else if (pdfArray2 != null)
          {
            width = (pdfArray2[2] as PdfNumber).FloatValue - (pdfArray2[0] as PdfNumber).FloatValue;
            height = (double) (pdfArray2[3] as PdfNumber).FloatValue != 0.0 ? (pdfArray2[3] as PdfNumber).FloatValue - (pdfArray2[1] as PdfNumber).FloatValue : (pdfArray2[1] as PdfNumber).FloatValue;
          }
        }
        else if (pdfArray2 != null)
        {
          width = (pdfArray2[2] as PdfNumber).FloatValue - (pdfArray2[0] as PdfNumber).FloatValue;
          height = (double) (pdfArray2[3] as PdfNumber).FloatValue != 0.0 ? (pdfArray2[3] as PdfNumber).FloatValue - (pdfArray2[1] as PdfNumber).FloatValue : (pdfArray2[1] as PdfNumber).FloatValue;
        }
        else
        {
          this.Dictionary["MediaBox"] = (IPdfPrimitive) new PdfArray(new float[4]
          {
            0.0f,
            0.0f,
            PdfPageSize.Letter.Width,
            PdfPageSize.Letter.Height
          });
          width = PdfPageSize.Letter.Width;
          height = PdfPageSize.Letter.Height;
        }
        if ((double) height < 0.0)
          height = -height;
        if ((double) width < 0.0)
          width = -width;
        this.m_size = new SizeF(width, height);
      }
      return this.m_size;
    }
  }

  public RectangleF CropBox
  {
    get
    {
      if (this.m_cropBox == RectangleF.Empty && this.Dictionary.ContainsKey(nameof (CropBox)))
      {
        PdfArray pdfArray = this.Dictionary.GetValue(this.CrossTable, nameof (CropBox), "Parent") as PdfArray;
        float floatValue = (pdfArray[2] as PdfNumber).FloatValue;
        float height = (double) (pdfArray[3] as PdfNumber).FloatValue != 0.0 ? (pdfArray[3] as PdfNumber).FloatValue : (pdfArray[1] as PdfNumber).FloatValue;
        this.m_cropBox = this.CalculateBounds((pdfArray[0] as PdfNumber).FloatValue, (pdfArray[1] as PdfNumber).FloatValue, floatValue, height);
      }
      return this.m_cropBox;
    }
  }

  public RectangleF BleedBox
  {
    get
    {
      if (this.m_bleedBox == RectangleF.Empty && this.Dictionary.ContainsKey(nameof (BleedBox)))
      {
        PdfArray pdfArray = this.Dictionary.GetValue(this.CrossTable, nameof (BleedBox), "Parent") as PdfArray;
        float floatValue = (pdfArray[2] as PdfNumber).FloatValue;
        float height = (double) (pdfArray[3] as PdfNumber).FloatValue != 0.0 ? (pdfArray[3] as PdfNumber).FloatValue : (pdfArray[1] as PdfNumber).FloatValue;
        this.m_bleedBox = this.CalculateBounds((pdfArray[0] as PdfNumber).FloatValue, (pdfArray[1] as PdfNumber).FloatValue, floatValue, height);
      }
      return this.m_bleedBox;
    }
  }

  public RectangleF TrimBox
  {
    get
    {
      if (this.m_trimBox == RectangleF.Empty && this.Dictionary.ContainsKey(nameof (TrimBox)))
      {
        PdfArray pdfArray = this.Dictionary.GetValue(this.CrossTable, nameof (TrimBox), "Parent") as PdfArray;
        float floatValue = (pdfArray[2] as PdfNumber).FloatValue;
        float height = (double) (pdfArray[3] as PdfNumber).FloatValue != 0.0 ? (pdfArray[3] as PdfNumber).FloatValue : (pdfArray[1] as PdfNumber).FloatValue;
        this.m_trimBox = this.CalculateBounds((pdfArray[0] as PdfNumber).FloatValue, (pdfArray[1] as PdfNumber).FloatValue, floatValue, height);
      }
      return this.m_trimBox;
    }
  }

  public RectangleF ArtBox
  {
    get
    {
      if (this.m_artBox == RectangleF.Empty && this.Dictionary.ContainsKey(nameof (ArtBox)))
      {
        PdfArray pdfArray = this.Dictionary.GetValue(this.CrossTable, nameof (ArtBox), "Parent") as PdfArray;
        float floatValue = (pdfArray[2] as PdfNumber).FloatValue;
        float height = (double) (pdfArray[3] as PdfNumber).FloatValue != 0.0 ? (pdfArray[3] as PdfNumber).FloatValue : (pdfArray[1] as PdfNumber).FloatValue;
        this.m_artBox = this.CalculateBounds((pdfArray[0] as PdfNumber).FloatValue, (pdfArray[1] as PdfNumber).FloatValue, floatValue, height);
      }
      return this.m_artBox;
    }
  }

  public PdfDocumentBase Document => this.m_document;

  public string[] ColorSpace => this.GetColorspace();

  internal PdfCrossTable CrossTable => this.m_crossTable;

  public void AddRedaction(PdfRedaction redaction) => this.Redactions.Add(redaction);

  internal override PdfResources GetResources()
  {
    if (this.m_resources == null)
    {
      if (!this.Dictionary.ContainsKey("Resources") || this.m_bCheckResources)
      {
        this.m_resources = base.GetResources();
        if ((this.m_resources.ObtainNames().Count == 0 || this.m_resources.Items.Count == 0) && this.Dictionary.ContainsKey("Parent"))
        {
          IPdfPrimitive pdfPrimitive = this.Dictionary["Parent"];
          PdfDictionary pdfDictionary1 = (object) (pdfPrimitive as PdfReferenceHolder) == null ? pdfPrimitive as PdfDictionary : (pdfPrimitive as PdfReferenceHolder).Object as PdfDictionary;
          if (pdfDictionary1.ContainsKey("Resources"))
          {
            IPdfPrimitive baseDictionary = pdfDictionary1["Resources"];
            if (baseDictionary is PdfDictionary && (baseDictionary as PdfDictionary).Items.Count > 0)
            {
              this.Dictionary["Resources"] = baseDictionary;
              this.m_resources = new PdfResources((PdfDictionary) baseDictionary);
              PdfDictionary pdfDictionary2 = new PdfDictionary();
              if (this.m_resources.ContainsKey("XObject") && this.m_resources["XObject"] is PdfDictionary resource)
              {
                if (PdfCrossTable.Dereference(this.Dictionary["Contents"]) is PdfArray pdfArray)
                {
                  for (int index = 0; index < pdfArray.Count; ++index)
                  {
                    PdfStream pageContent = PdfCrossTable.Dereference(pdfArray[index]) as PdfStream;
                    pageContent.Decompress();
                    this.ParseXobjectImages(pageContent, resource, pdfDictionary2);
                  }
                }
                else
                {
                  PdfStream pageContent = PdfCrossTable.Dereference(this.Dictionary["Contents"]) as PdfStream;
                  pageContent.Decompress();
                  this.ParseXobjectImages(pageContent, resource, pdfDictionary2);
                }
                this.m_resources.SetProperty("XObject", (IPdfPrimitive) pdfDictionary2);
                this.SetResources(this.m_resources);
              }
            }
            else if ((object) (baseDictionary as PdfReferenceHolder) != null)
            {
              bool flag = false;
              PdfReferenceHolder pdfReferenceHolder = baseDictionary as PdfReferenceHolder;
              if (pdfReferenceHolder != (PdfReferenceHolder) null)
              {
                PdfDictionary pdfDictionary3 = pdfReferenceHolder.Object as PdfDictionary;
                if (pdfDictionary3.Items.Count == this.m_resources.Items.Count || this.m_resources.Items.Count == 0)
                {
                  foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in this.m_resources.Items)
                  {
                    if (pdfDictionary3.Items.ContainsKey(keyValuePair.Key))
                    {
                      if (pdfDictionary3.Items.ContainsValue(this.m_resources[keyValuePair.Key]))
                        flag = true;
                    }
                    else
                    {
                      flag = false;
                      break;
                    }
                  }
                  if (flag || this.m_resources.Items.Count == 0)
                  {
                    this.Dictionary["Resources"] = baseDictionary;
                    this.m_resources = new PdfResources((PdfDictionary) (baseDictionary as PdfReferenceHolder).Object);
                  }
                  this.SetResources(this.m_resources);
                }
              }
            }
          }
        }
      }
      else
      {
        IPdfPrimitive pointer = this.Dictionary["Resources"];
        PdfDictionary pdfDictionary4 = this.m_crossTable.GetObject(pointer) as PdfDictionary;
        this.m_resources = new PdfResources(pdfDictionary4);
        if (pdfDictionary4 != pointer)
        {
          this.m_crossTable.Document.PdfObjects.ReregisterReference((IPdfPrimitive) pdfDictionary4, (IPdfPrimitive) this.m_resources);
          if (!this.m_crossTable.IsMerging)
            this.m_resources.Position = -1;
        }
        else
          this.Dictionary["Resources"] = (IPdfPrimitive) this.m_resources;
        if (this.Dictionary.ContainsKey("Parent") && PdfCrossTable.Dereference(this.Dictionary["Parent"]) is PdfDictionary pdfDictionary5 && pdfDictionary5.ContainsKey("Resources"))
        {
          PdfReferenceHolder pdfReferenceHolder1 = pdfDictionary5["Resources"] as PdfReferenceHolder;
          PdfReferenceHolder pdfReferenceHolder2 = pointer as PdfReferenceHolder;
          if (pdfReferenceHolder2 != (PdfReferenceHolder) null && pdfReferenceHolder1 != (PdfReferenceHolder) null && pdfReferenceHolder1.Reference == pdfReferenceHolder2.Reference && PdfCrossTable.Dereference(pointer) is PdfDictionary baseDictionary)
            this.m_resources = new PdfResources(baseDictionary);
        }
        this.SetResources(this.m_resources);
      }
      this.m_bCheckResources = true;
    }
    return this.m_resources;
  }

  private int CalculateHash(byte[] b)
  {
    int hash = 0;
    int length = b.Length;
    for (int index = 0; index < length; ++index)
      hash = hash * 31 /*0x1F*/ + (int) b[index];
    return hash;
  }

  private void ParseXobjectImages(
    PdfStream pageContent,
    PdfDictionary xObject,
    PdfDictionary xobjects)
  {
    PdfRecordCollection recordCollection = new ContentParser(pageContent.Data).ReadContent();
    if (recordCollection == null)
      return;
    for (int index = 0; index < recordCollection.RecordCollection.Count; ++index)
    {
      if (recordCollection.RecordCollection[index].OperatorName == "Do")
      {
        xObject.ContainsKey(recordCollection.RecordCollection[index].Operands[0]);
        PdfReferenceHolder pdfReferenceHolder = xObject[recordCollection.RecordCollection[index].Operands[0].TrimStart('/')] as PdfReferenceHolder;
        if (pdfReferenceHolder != (PdfReferenceHolder) null)
          xobjects[recordCollection.RecordCollection[index].Operands[0].TrimStart('/')] = (IPdfPrimitive) pdfReferenceHolder;
      }
    }
  }

  internal List<PdfDictionary> TerminalAnnotation
  {
    get => this.m_terminalannots;
    set => this.m_terminalannots = value;
  }

  internal override PointF Origin
  {
    get
    {
      if (this.m_origin == PointF.Empty)
      {
        PdfArray pdfArray = this.Dictionary.GetValue(this.CrossTable, "MediaBox", "Parent") as PdfArray;
        this.m_origin = new PointF((pdfArray[0] as PdfNumber).FloatValue, (pdfArray[1] as PdfNumber).FloatValue);
      }
      return this.m_origin;
    }
  }

  internal PdfArray AnnotsReference
  {
    get => this.m_annotsReference;
    set => this.m_annotsReference = value;
  }

  internal PdfLoadedPage(PdfDocumentBase document, PdfCrossTable cTable, PdfDictionary dictionary)
    : base(dictionary)
  {
    if (document == null)
      throw new ArgumentNullException(nameof (document));
    if (cTable == null)
      throw new ArgumentNullException(nameof (cTable));
    this.m_document = document;
    this.m_crossTable = cTable;
    if (!cTable.PageCorrespondance.ContainsKey((IPdfPrimitive) this.Dictionary))
      cTable.PageCorrespondance.Add((IPdfPrimitive) this.Dictionary, (object) null);
    if (!this.m_document.IsPdfViewerDocumentDisable)
      return;
    this.Dictionary.BeginSave += new SavePdfPrimitiveEventHandler(this.PageBeginSave);
    this.Dictionary.EndSave += new SavePdfPrimitiveEventHandler(this.PageEndSave);
  }

  public event EventHandler BeginSave;

  internal void CreateAnnotations(List<long> widgetReferences)
  {
    PdfLoadedPage.m_annotChanged = true;
    if (this.Dictionary.ContainsKey("Annots"))
    {
      PdfArray pdfArray1 = this.m_crossTable.GetObject(this.Dictionary["Annots"]) as PdfArray;
      PdfLoadedDocument document = this.Document as PdfLoadedDocument;
      if (pdfArray1 != null)
      {
        for (int index1 = 0; index1 < pdfArray1.Count; ++index1)
        {
          PdfDictionary pdfDictionary1 = this.m_crossTable.GetObject(pdfArray1[index1]) as PdfDictionary;
          PdfReferenceHolder pdfReferenceHolder1 = pdfArray1[index1] as PdfReferenceHolder;
          if (document.CrossTable != null && document.CrossTable.Encryptor != null && document.CrossTable.Encryptor.EncryptOnlyAttachment && pdfDictionary1 != null && pdfDictionary1.ContainsKey("Subtype"))
          {
            PdfName pdfName = pdfDictionary1.Items[new PdfName("Subtype")] as PdfName;
            if (pdfName != (PdfName) null && pdfName.Value == "FileAttachment")
            {
              PdfReferenceHolder pdfReferenceHolder2 = pdfDictionary1["FS"] as PdfReferenceHolder;
              if (pdfReferenceHolder2 != (PdfReferenceHolder) null)
              {
                PdfDictionary pdfDictionary2 = pdfReferenceHolder2.Object as PdfDictionary;
                PdfStream pdfStream1 = new PdfStream();
                PdfDictionary pdfDictionary3 = (PdfDictionary) null;
                if (pdfDictionary2 != null && pdfDictionary2.ContainsKey("EF"))
                {
                  if (pdfDictionary2["EF"] is PdfDictionary)
                    pdfDictionary3 = pdfDictionary2["EF"] as PdfDictionary;
                  else if ((object) (pdfDictionary2["EF"] as PdfReferenceHolder) != null)
                    pdfDictionary3 = (pdfDictionary2["EF"] as PdfReferenceHolder).Object as PdfDictionary;
                  if (pdfDictionary3 != null)
                  {
                    PdfReferenceHolder pdfReferenceHolder3 = pdfDictionary3["F"] as PdfReferenceHolder;
                    if (pdfReferenceHolder3 != (PdfReferenceHolder) null)
                    {
                      PdfReference reference = pdfReferenceHolder3.Reference;
                      PdfStream pdfStream2 = pdfReferenceHolder3.Object as PdfStream;
                      IPdfDecryptable pdfDecryptable = (IPdfDecryptable) pdfStream2;
                      if (pdfDecryptable != null)
                      {
                        if (document.RaiseUserPassword && document.m_password == string.Empty)
                        {
                          OnPdfPasswordEventArgs args = new OnPdfPasswordEventArgs();
                          document.PdfUserPassword(args);
                          document.m_password = args.UserPassword;
                        }
                        document.CheckEncryption(document.CrossTable.Encryptor.EncryptOnlyAttachment);
                        pdfDecryptable.Decrypt(document.CrossTable.Encryptor, reference.ObjNum);
                      }
                      pdfStream2.Decompress();
                    }
                  }
                }
              }
            }
          }
          if (pdfDictionary1 != null && pdfDictionary1.ContainsKey("Subtype"))
          {
            PdfName pdfName = pdfDictionary1.Items[new PdfName("Subtype")] as PdfName;
            if (pdfName != (PdfName) null && pdfName.Value.ToString() == "Widget" && document.Form != null)
            {
              if (pdfDictionary1.ContainsKey("Parent"))
              {
                if ((pdfDictionary1.Items[new PdfName("Parent")] as PdfReferenceHolder).Object is PdfDictionary pdfDictionary4)
                {
                  if (!pdfDictionary4.ContainsKey("Fields"))
                  {
                    if (pdfReferenceHolder1.Reference != (PdfReference) null && !widgetReferences.Contains(pdfReferenceHolder1.Reference.ObjNum))
                    {
                      if (!document.Form.TerminalFields.Contains(pdfDictionary4))
                        document.Form.TerminalFields.Add(pdfDictionary4);
                    }
                    else if (pdfDictionary4.ContainsKey("Kids") && pdfDictionary4.Count == 1)
                      pdfDictionary1.Remove("Parent");
                  }
                  else if (!pdfDictionary4.ContainsKey("Kids"))
                    pdfDictionary1.Remove("Parent");
                }
              }
              else if (!document.Form.TerminalFields.Contains(pdfDictionary1))
              {
                if (document.Form.m_widgetDictionary == null)
                  document.Form.m_widgetDictionary = new System.Collections.Generic.Dictionary<string, List<PdfDictionary>>();
                if (pdfDictionary1.ContainsKey("T"))
                {
                  string key = (pdfDictionary1.Items[new PdfName("T")] as PdfString).Value;
                  if (document.Form.m_widgetDictionary.ContainsKey(key))
                    document.Form.m_widgetDictionary[key].Add(pdfDictionary1);
                  else if (!document.Form.Fields.m_addedFieldNames.Contains(key))
                    document.Form.m_widgetDictionary.Add(key, new List<PdfDictionary>()
                    {
                      pdfDictionary1
                    });
                }
              }
            }
          }
          if (pdfReferenceHolder1 != (PdfReferenceHolder) null && pdfReferenceHolder1.Reference != (PdfReference) null)
          {
            if (!this.m_annotsReference.Contains((IPdfPrimitive) pdfReferenceHolder1.Reference))
              this.m_annotsReference.Add((IPdfPrimitive) pdfReferenceHolder1.Reference);
            bool flag = false;
            if (document != null && document.Form != null && widgetReferences.Contains(pdfReferenceHolder1.Reference.ObjNum))
            {
              foreach (object field in (PdfCollection) document.Form.Fields)
              {
                if (field is PdfLoadedField pdfLoadedField)
                {
                  PdfArray pdfArray2 = (PdfArray) null;
                  if (pdfLoadedField.Dictionary.ContainsKey("Kids"))
                    pdfArray2 = PdfCrossTable.Dereference(pdfLoadedField.Dictionary["Kids"]) as PdfArray;
                  if (pdfArray2 != null && pdfArray2.Count > 1)
                  {
                    for (int index2 = 0; index2 < pdfArray2.Count; ++index2)
                    {
                      IPdfPrimitive pdfPrimitive = (IPdfPrimitive) (PdfCrossTable.Dereference(pdfArray2[index2]) as PdfDictionary);
                      if (pdfPrimitive != null)
                      {
                        PdfReference reference = this.CrossTable.GetReference(pdfPrimitive);
                        if (pdfReferenceHolder1.Reference == reference)
                          flag = true;
                      }
                    }
                  }
                  else
                  {
                    IPdfPrimitive widgetAnnotation = (IPdfPrimitive) pdfLoadedField.GetWidgetAnnotation(pdfLoadedField.Dictionary, pdfLoadedField.CrossTable);
                    if (widgetAnnotation != null)
                    {
                      PdfReference reference = this.CrossTable.GetReference(widgetAnnotation);
                      if (pdfReferenceHolder1.Reference == reference)
                        flag = true;
                    }
                  }
                }
              }
            }
            if (!document.IsXFAForm)
            {
              if ((document.Form == null || widgetReferences != null) && !flag && !this.m_terminalannots.Contains(pdfDictionary1))
                this.m_terminalannots.Add(pdfDictionary1);
            }
            else if (pdfDictionary1 != null && pdfReferenceHolder1.Reference != (PdfReference) null && !flag && !this.m_terminalannots.Contains(pdfDictionary1))
              this.m_terminalannots.Add(pdfDictionary1);
          }
          else if (pdfDictionary1 != null && this.importAnnotation && !this.m_terminalannots.Contains(pdfDictionary1))
            this.m_terminalannots.Add(pdfDictionary1);
        }
      }
    }
    if (this.importAnnotation)
      this.importAnnotation = false;
    this.m_annots = new PdfLoadedAnnotationCollection(this);
    this.Annotations = this.m_annots;
  }

  private bool CheckFormField(IPdfPrimitive iPdfPrimitive)
  {
    PdfLoadedDocument document = this.Document as PdfLoadedDocument;
    if (this.m_widgetReferences == null && document.Form != null)
    {
      this.m_widgetReferences = new List<long>();
      foreach (object field in (PdfCollection) document.Form.Fields)
      {
        if (field is PdfLoadedField pdfLoadedField)
        {
          IPdfPrimitive widgetAnnotation = (IPdfPrimitive) pdfLoadedField.GetWidgetAnnotation(pdfLoadedField.Dictionary, pdfLoadedField.CrossTable);
          bool isNew;
          PdfReference reference = this.Document.PdfObjects.GetReference(widgetAnnotation, out isNew);
          if (isNew)
            reference = this.CrossTable.GetReference(widgetAnnotation);
          this.m_widgetReferences.Add(reference.ObjNum);
        }
      }
    }
    PdfReferenceHolder pdfReferenceHolder = iPdfPrimitive as PdfReferenceHolder;
    return pdfReferenceHolder != (PdfReferenceHolder) null && document.Form != null && this.m_widgetReferences.Count > 0 && pdfReferenceHolder.Reference != (PdfReference) null && this.m_widgetReferences.Contains(pdfReferenceHolder.Reference.ObjNum);
  }

  internal List<long> GetWidgetReferences()
  {
    PdfLoadedDocument document = this.Document as PdfLoadedDocument;
    if (this.m_widgetReferences == null && document.Form != null)
    {
      this.m_widgetReferences = new List<long>();
      foreach (object field in (PdfCollection) document.Form.Fields)
      {
        if (field is PdfLoadedField pdfLoadedField)
        {
          IPdfPrimitive widgetAnnotation = (IPdfPrimitive) pdfLoadedField.GetWidgetAnnotation(pdfLoadedField.Dictionary, pdfLoadedField.CrossTable);
          PdfArray pdfArray = (PdfArray) null;
          if (pdfLoadedField.Dictionary.ContainsKey("Kids"))
            pdfArray = PdfCrossTable.Dereference(pdfLoadedField.Dictionary["Kids"]) as PdfArray;
          bool isNew;
          if (pdfArray != null && pdfArray.Count > 1)
          {
            for (int index = 0; index < pdfArray.Count; ++index)
            {
              IPdfPrimitive pdfPrimitive = (IPdfPrimitive) (PdfCrossTable.Dereference(pdfArray[index]) as PdfDictionary);
              if (pdfPrimitive != null)
              {
                PdfReference reference = this.Document.PdfObjects.GetReference(pdfPrimitive, out isNew);
                if (isNew)
                  reference = this.CrossTable.GetReference(pdfPrimitive);
                this.m_widgetReferences.Add(reference.ObjNum);
              }
            }
          }
          else
          {
            PdfReference reference = this.Document.PdfObjects.GetReference(widgetAnnotation, out isNew);
            if (isNew)
              reference = this.CrossTable.GetReference(widgetAnnotation);
            this.m_widgetReferences.Add(reference.ObjNum);
          }
        }
      }
    }
    return this.m_widgetReferences;
  }

  internal void RemoveFromDictionaries(PdfAnnotation annot)
  {
    if (this.Dictionary.ContainsKey("Annots"))
    {
      PdfArray primitive = this.m_crossTable.GetObject(this.Dictionary["Annots"]) as PdfArray;
      PdfReferenceHolder element1 = new PdfReferenceHolder((IPdfPrimitive) annot.Dictionary);
      if (annot.Dictionary.ContainsKey("Popup"))
      {
        PdfDictionary pointer = (object) (annot.Dictionary[new PdfName("Popup")] as PdfReferenceHolder) == null ? annot.Dictionary[new PdfName("Popup")] as PdfDictionary : (annot.Dictionary[new PdfName("Popup")] as PdfReferenceHolder).Object as PdfDictionary;
        PdfReferenceHolder element2 = new PdfReferenceHolder((IPdfPrimitive) pointer);
        primitive.Remove((IPdfPrimitive) element2);
        IPdfPrimitive element3 = this.m_crossTable.GetObject((IPdfPrimitive) pointer);
        int index = this.m_crossTable.PdfObjects.IndexOf(element3);
        if (index != -1)
          this.m_crossTable.PdfObjects.Remove(index);
        this.RemoveAllReference(element3);
        this.TerminalAnnotation.Remove(pointer);
      }
      primitive.Remove((IPdfPrimitive) element1);
      primitive.MarkChanged();
      IPdfPrimitive element4 = this.m_crossTable.GetObject((IPdfPrimitive) annot.Dictionary);
      int index1 = this.m_crossTable.PdfObjects.IndexOf(element4);
      if (index1 != -1)
        this.m_crossTable.PdfObjects.Remove(index1);
      this.RemoveAllReference(element4);
      if (annot is PdfRubberStampAnnotation rubberStampAnnotation && rubberStampAnnotation.Appearance != null && rubberStampAnnotation.Appearance.Normal != null)
      {
        PdfTemplate normal = rubberStampAnnotation.Appearance.Normal;
        if (normal != null)
          this.RemoveAllReference((IPdfPrimitive) normal.m_content);
      }
      this.TerminalAnnotation.Remove(annot.Dictionary);
      this.Dictionary.SetProperty("Annots", (IPdfPrimitive) primitive);
    }
    PdfLoadedAnnotation loadedAnnotation = annot as PdfLoadedAnnotation;
  }

  private void RemoveAllReference(IPdfPrimitive obj)
  {
    if (!(obj is PdfDictionary pdfDictionary) && (object) (obj as PdfReferenceHolder) != null)
      pdfDictionary = (obj as PdfReferenceHolder).Object as PdfDictionary;
    if (pdfDictionary == null)
      return;
    foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in pdfDictionary.Items)
    {
      if (((object) (keyValuePair.Value as PdfReferenceHolder) != null || keyValuePair.Value is PdfDictionary) && keyValuePair.Key.Value != "P" && keyValuePair.Key.Value != "Parent")
      {
        int index = this.m_crossTable.PdfObjects.IndexOf(this.m_crossTable.GetObject(keyValuePair.Value));
        if (index != -1)
          this.m_crossTable.PdfObjects.Remove(index);
        this.RemoveAllReference(keyValuePair.Value);
        if (PdfCrossTable.Dereference(keyValuePair.Value) is PdfStream pdfStream)
        {
          if (pdfStream.InternalStream != null)
            pdfStream.InternalStream.Dispose();
          pdfStream.Dispose();
          pdfStream.Clear();
          pdfStream.isSkip = true;
        }
      }
    }
  }

  internal void RemoveHeadersFooters(List<RectangleF> bounds)
  {
    this.m_headerFooterBounds = bounds;
    if (bounds == null || bounds.Count <= 0)
      return;
    CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
    Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
    Matrix currentMatrix = (Matrix) null;
    this.m_pageResources = PageResourceLoader.Instance.GetPageResources((PdfPageBase) this);
    this.m_currentMatrix.Push(new GraphicsStateData()
    {
      m_drawing2dMatrixCTM = new Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f)
    });
    using (MemoryStream memoryStream = new MemoryStream())
    {
      this.Layers.CombineContent((Stream) memoryStream);
      memoryStream.Position = 0L;
      this.m_recordCollection = new ContentParser(memoryStream.ToArray()).ReadContent();
    }
    PdfStream stream1 = new PdfStream();
    List<string> keys = new List<string>();
    PdfImageRenderer pdfImageRenderer = new PdfImageRenderer(this.m_recordCollection.RecordCollection.Count);
    bool flag = false;
    for (int index = 0; index < this.m_recordCollection.RecordCollection.Count; ++index)
    {
      if (this.ParseContentStream(index, currentMatrix, keys))
        flag = true;
      else
        pdfImageRenderer.OptimizeContent(this.m_recordCollection, index, (string) null, stream1);
    }
    if (!flag)
      return;
    if (this.Dictionary.ContainsKey("Contents"))
    {
      PdfArray pdfArray = this.Dictionary["Contents"] as PdfArray;
      stream1.Compress = true;
      if (pdfArray != null)
      {
        foreach (IPdfPrimitive content in this.Contents)
        {
          PdfDictionary pdfDictionary = this.GetObject(content);
          if (pdfDictionary != null)
            pdfDictionary.isSkip = true;
        }
        pdfArray.Clear();
        pdfArray.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) stream1));
      }
      else
      {
        if (this.Dictionary["Contents"] is PdfStream stream2)
        {
          stream2.Clear();
          stream2.Items.Remove(new PdfName("Length"));
          stream2.Data = stream1.Data;
          stream2.Compress = true;
        }
        this.Graphics.StreamWriter = new PdfStreamWriter(stream2);
      }
    }
    PdfResources resources = this.GetResources();
    if (resources == null || !resources.ContainsKey("XObject") || !(resources["XObject"] is PdfDictionary pdfDictionary1))
      return;
    foreach (PdfName pdfName in new List<PdfName>((IEnumerable<PdfName>) pdfDictionary1.Keys))
    {
      if (!keys.Contains(pdfName.Value))
      {
        pdfDictionary1.Remove(pdfName.Value);
        pdfDictionary1.Modify();
      }
    }
  }

  private PdfDictionary GetObject(IPdfPrimitive primitive)
  {
    PdfDictionary pdfDictionary = (PdfDictionary) null;
    if (primitive is PdfDictionary)
      pdfDictionary = primitive as PdfDictionary;
    else if ((object) (primitive as PdfReferenceHolder) != null)
      pdfDictionary = (primitive as PdfReferenceHolder).Object as PdfDictionary;
    return pdfDictionary;
  }

  protected virtual void OnBeginSave(EventArgs e)
  {
    if (this.BeginSave == null)
      return;
    this.BeginSave((object) this, e);
  }

  private void PageBeginSave(object sender, SavePdfPrimitiveEventArgs args)
  {
    if (this.m_document.progressDelegate != null)
      this.m_document.OnPageSave((PdfPageBase) this);
    this.OnBeginSave(new EventArgs());
  }

  private void PageEndSave(object sender, SavePdfPrimitiveEventArgs args)
  {
  }

  internal PdfFont[] ExtractFonts()
  {
    List<PdfFont> pdfFontList = new List<PdfFont>();
    this.GetFontStream();
    if (this.m_fontReference == null)
      return pdfFontList.ToArray();
    bool signedPDF = false;
    if (this.Document is PdfLoadedDocument document && document.Catalog != null && document.Catalog.ContainsKey("AcroForm") && PdfCrossTable.Dereference(document.Catalog["AcroForm"]) is PdfDictionary pdfDictionary1 && pdfDictionary1.ContainsKey("SigFlags"))
      signedPDF = true;
    foreach (PdfReferenceHolder pdfReferenceHolder in this.m_fontReference)
    {
      if (!(this.Document as PdfLoadedDocument).font.ContainsKey(pdfReferenceHolder.Reference.ObjNum))
      {
        PdfDictionary fontDictionary1 = pdfReferenceHolder.Object as PdfDictionary;
        float num = 12f;
        PdfName pdfName1 = this.CrossTable.GetObject(fontDictionary1["BaseFont"]) as PdfName;
        string keyValue = "BaseFont";
        if (pdfName1 == (PdfName) null && fontDictionary1.ContainsKey("Name"))
        {
          pdfName1 = this.CrossTable.GetObject(fontDictionary1["Name"]) as PdfName;
          keyValue = "Name";
        }
        PdfFont pdfFont = PdfDocument.ConformanceLevel == PdfConformanceLevel.None ? (PdfFont) new PdfStandardFont((PdfStandardFont) PdfDocument.DefaultFont, num) : (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, 8f, PdfFontStyle.Regular, true);
        string key = this.GetKey(pdfName1, keyValue);
        if (key != null)
          num = this.GetContentHeight(key, signedPDF);
        if ((double) num < 0.0)
          num = 12f;
        if ((double) num == 0.0 && this.m_xObjectContentStream != null && this.m_xObjectContentStream.Count > 0)
        {
          foreach (PdfStream pdfStream in this.m_xObjectContentStream)
          {
            pdfStream.Decompress();
            MemoryStream data = new MemoryStream();
            pdfStream.InternalStream.WriteTo((Stream) data);
            num = this.GetContentHeight(key, data);
            data.Dispose();
            if ((double) num > 0.0)
              break;
          }
        }
        bool flag;
        if ((double) num == 0.0)
        {
          flag = false;
          num = 12f;
        }
        else
          flag = true;
        if (fontDictionary1.ContainsKey("Subtype"))
        {
          PdfName pdfName2 = this.CrossTable.GetObject(fontDictionary1["Subtype"]) as PdfName;
          if (pdfName2.Value == "Type1")
          {
            try
            {
              PdfFontFamily fontFamily = this.GetFontFamily(pdfName1.Value);
              PdfFontStyle fontStyle = this.GetFontStyle(pdfName1.Value);
              pdfFont = PdfDocument.ConformanceLevel == PdfConformanceLevel.None ? (PdfFont) new PdfStandardFont(fontFamily, num, fontStyle) : (PdfFont) new PdfStandardFont(fontFamily, num, fontStyle, true);
            }
            catch (ArgumentException ex)
            {
              PdfFontMetrics font = this.CreateFont(fontDictionary1, num, pdfName1);
              string str = pdfName1.Value.Substring(pdfName1.Value.IndexOf('+') + 1);
              PdfFontStyle style = PdfFontStyle.Regular;
              if (str.Contains("PSMT"))
                str = str.Remove(str.IndexOf("PSMT"));
              else if (str.Contains("Bold"))
                style = PdfFontStyle.Bold;
              else if (str.Contains("Italic"))
                style = PdfFontStyle.Italic;
              if (str.Contains("BoldItalic"))
                style = PdfFontStyle.Bold | PdfFontStyle.Italic;
              if (str.Contains("PS"))
                str = str.Remove(str.IndexOf("PS"));
              if (str.Contains("-"))
                str = str.Remove(str.IndexOf("-"));
              pdfFont = (PdfFont) new PdfStandardFont((PdfStandardFont) PdfDocument.DefaultFont, num, style);
              WidthTable widthTable = pdfFont.Metrics.WidthTable;
              pdfFont.Metrics = font;
              pdfFont.Metrics.Name = str;
              pdfFont.Metrics.WidthTable = widthTable;
            }
          }
          else if (pdfName2.Value == "TrueType")
          {
            PdfFontMetrics font = this.CreateFont(fontDictionary1, num, pdfName1);
            string familyName = pdfName1.Value.Substring(pdfName1.Value.IndexOf('+') + 1);
            FontStyle style = FontStyle.Regular;
            if (familyName.Contains("PSMT"))
              familyName = familyName.Remove(familyName.IndexOf("PSMT"));
            else if (familyName.Contains("Bold"))
              style = FontStyle.Bold;
            else if (familyName.Contains("Italic"))
              style = FontStyle.Italic;
            if (familyName.Contains("BoldItalic"))
              style = FontStyle.Bold | FontStyle.Italic;
            if (familyName.Contains("PS"))
              familyName = familyName.Remove(familyName.IndexOf("PS"));
            if (familyName.Contains("-"))
              familyName = familyName.Remove(familyName.IndexOf("-"));
            if (familyName.Contains("#20"))
              familyName = familyName.Replace("#20", " ");
            foreach (FontFamily family in FontFamily.Families)
            {
              string str = family.Name.Replace(" ", string.Empty);
              if (familyName.Equals(str))
              {
                familyName = family.Name;
                break;
              }
            }
            bool unicode = false;
            if (fontDictionary1.ContainsKey("ToUnicode"))
              unicode = true;
            pdfFont = (PdfFont) new PdfTrueTypeFont(new Font(familyName, num, style), unicode);
            WidthTable widthTable = pdfFont.Metrics.WidthTable;
            pdfFont.Metrics = font;
            pdfFont.Metrics.Name = familyName;
            pdfFont.Metrics.WidthTable = widthTable;
          }
          else if (pdfName2.Value == "Type0" && fontDictionary1.ContainsKey("ToUnicode"))
          {
            PdfArray pdfArray = (PdfArray) null;
            if (fontDictionary1.ContainsKey("DescendantFonts"))
              pdfArray = PdfCrossTable.Dereference(fontDictionary1["DescendantFonts"]) as PdfArray;
            PdfDictionary fontDictionary2 = (PdfDictionary) null;
            if (pdfArray != null && pdfArray.Count > 0)
              fontDictionary2 = PdfCrossTable.Dereference(pdfArray[0]) as PdfDictionary;
            PdfDictionary pdfDictionary2 = (PdfDictionary) null;
            if (fontDictionary2 != null && fontDictionary2.ContainsKey("FontDescriptor"))
              pdfDictionary2 = PdfCrossTable.Dereference(fontDictionary2["FontDescriptor"]) as PdfDictionary;
            else if (pdfArray == null && fontDictionary1.ContainsKey("FontDescriptor"))
              pdfDictionary2 = PdfCrossTable.Dereference(fontDictionary1["FontDescriptor"]) as PdfDictionary;
            if (pdfDictionary2 != null)
            {
              PdfName baseFont = PdfCrossTable.Dereference(pdfDictionary2["FontName"]) as PdfName;
              if (baseFont != (PdfName) null)
              {
                PdfFontMetrics font = this.CreateFont(fontDictionary2, num, baseFont);
                string familyName = baseFont.Value.Substring(baseFont.Value.IndexOf('+') + 1);
                FontStyle style = FontStyle.Regular;
                if (familyName.Contains("PSMT"))
                  familyName = familyName.Remove(familyName.IndexOf("PSMT"));
                else if (familyName.Contains("Bold"))
                  style = FontStyle.Bold;
                else if (familyName.Contains("Italic"))
                  style = FontStyle.Italic;
                if (familyName.Contains("BoldItalic"))
                  style = FontStyle.Bold | FontStyle.Italic;
                if (familyName.Contains("PS"))
                  familyName = familyName.Remove(familyName.IndexOf("PS"));
                if (familyName.Contains("-"))
                  familyName = familyName.Remove(familyName.IndexOf("-"));
                foreach (FontFamily family in FontFamily.Families)
                {
                  string str = family.Name.Replace(" ", string.Empty);
                  if (familyName.Equals(str))
                  {
                    if (!family.IsStyleAvailable(style))
                    {
                      if (family.IsStyleAvailable(FontStyle.Regular))
                        style = FontStyle.Regular;
                      else if (family.IsStyleAvailable(FontStyle.Italic))
                        style = FontStyle.Italic;
                      else if (family.IsStyleAvailable(FontStyle.Bold))
                        style = FontStyle.Bold;
                      else if (family.IsStyleAvailable(FontStyle.Strikeout))
                        style = FontStyle.Strikeout;
                      else if (family.IsStyleAvailable(FontStyle.Underline))
                        style = FontStyle.Underline;
                    }
                    familyName = family.Name;
                    break;
                  }
                }
                pdfFont = (PdfFont) new PdfTrueTypeFont(new Font(familyName, num, style), true);
                WidthTable widthTable = pdfFont.Metrics.WidthTable;
                pdfFont.Metrics = font;
                pdfFont.Metrics.Name = familyName;
                pdfFont.Metrics.WidthTable = widthTable;
              }
            }
          }
        }
        if (pdfFont != null && flag)
        {
          pdfFont.InternalFontName = pdfName1.Value;
          (this.Document as PdfLoadedDocument).font.Add(pdfReferenceHolder.Reference.ObjNum, pdfFont);
          pdfFontList.Add(pdfFont);
        }
      }
    }
    return pdfFontList.ToArray();
  }

  internal override void Clear()
  {
    if (this.m_annots != null)
      this.m_annots.Clear();
    base.Clear();
    if (this.m_terminalannots != null)
      this.m_terminalannots.Clear();
    if (this.m_widgetReferences != null)
      this.m_widgetReferences.Clear();
    if (this.m_fontReference == null)
      return;
    this.m_fontReference.Clear();
  }

  private float GetContentHeight(string key, bool signedPDF)
  {
    MemoryStream memoryStream = new MemoryStream();
    if (!signedPDF)
    {
      this.Layers.CombineContent((Stream) memoryStream);
    }
    else
    {
      PdfArray pdfArray = new PdfArray();
      bool flag = false;
      foreach (IPdfPrimitive content in this.Contents)
      {
        pdfArray.Add(content);
        if (PdfCrossTable.Dereference(content) is PdfStream pdfStream && pdfStream.Changed)
          flag = pdfStream.Changed;
      }
      this.Layers.CombineContent((Stream) memoryStream);
      this.Contents.Clear();
      foreach (IPdfPrimitive element in pdfArray)
      {
        this.Contents.Add(element);
        if (!flag && PdfCrossTable.Dereference(element) is PdfStream freezer && freezer.Changed)
          freezer.FreezeChanges((object) freezer);
      }
    }
    float result = 0.0f;
    string text = PdfString.ByteToString(memoryStream.ToArray());
    StringTokenizer stringTokenizer = new StringTokenizer(text);
    if (text.Contains(key))
    {
      int num = text.IndexOf(key);
      stringTokenizer.Position = num;
      string[] strArray = stringTokenizer.ReadLine().Split(' ');
      if (strArray.Length == 3)
        float.TryParse(strArray[1], NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result);
      if ((double) result == 0.0)
        result = 12f;
    }
    return result;
  }

  private float GetContentHeight(string key, MemoryStream data)
  {
    float contentHeight = 0.0f;
    string text = PdfString.ByteToString(data.ToArray());
    StringTokenizer stringTokenizer = new StringTokenizer(text);
    if (text.Contains(key))
    {
      int num = text.IndexOf(key);
      stringTokenizer.Position = num;
      string[] strArray = stringTokenizer.ReadLine().Split(' ');
      contentHeight = strArray.Length != 3 ? 12f : float.Parse(strArray[1], (IFormatProvider) CultureInfo.InvariantCulture);
    }
    return contentHeight;
  }

  internal string FontName(string fontString, out float height)
  {
    PdfReader pdfReader = new PdfReader((Stream) new MemoryStream(Encoding.ASCII.GetBytes(fontString)));
    pdfReader.Position = 0L;
    string s = pdfReader.GetNextToken();
    string nextToken = pdfReader.GetNextToken();
    string str = (string) null;
    height = 0.0f;
    while (nextToken != null && nextToken != string.Empty)
    {
      str = s;
      s = nextToken;
      nextToken = pdfReader.GetNextToken();
      if (nextToken == "Tf")
      {
        height = (float) double.Parse(s, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture);
        break;
      }
    }
    return str;
  }

  private PdfFontMetrics CreateFont(PdfDictionary fontDictionary, float height, PdfName baseFont)
  {
    PdfFontMetrics font = new PdfFontMetrics();
    if (fontDictionary.ContainsKey("FontDescriptor") && PdfCrossTable.Dereference(fontDictionary["FontDescriptor"]) is PdfDictionary pdfDictionary)
    {
      if (pdfDictionary.ContainsKey("Ascent") && PdfCrossTable.Dereference(pdfDictionary["Ascent"]) is PdfNumber pdfNumber1)
        font.Ascent = (float) pdfNumber1.IntValue;
      if (pdfDictionary.ContainsKey("Descent") && PdfCrossTable.Dereference(pdfDictionary["Descent"]) is PdfNumber pdfNumber2)
        font.Descent = (float) pdfNumber2.IntValue;
      font.Size = height;
      font.Height = font.Ascent - font.Descent;
      font.PostScriptName = baseFont.Value;
      if (fontDictionary.ContainsKey("Widths") && PdfCrossTable.Dereference(fontDictionary["Widths"]) is PdfArray pdfArray)
      {
        int[] widths = new int[pdfArray.Count];
        for (int index = 0; index < pdfArray.Count; ++index)
          widths[index] = (pdfArray[index] as PdfNumber).IntValue;
        font.WidthTable = (WidthTable) new StandardWidthTable(widths);
      }
      font.Name = baseFont.Value;
    }
    return font;
  }

  private PdfFontStyle GetFontStyle(string fontFamilyString)
  {
    int num = fontFamilyString.IndexOf("-");
    PdfFontStyle fontStyle = PdfFontStyle.Regular;
    if (num >= 0)
    {
      switch (fontFamilyString.Substring(num + 1, fontFamilyString.Length - num - 1))
      {
        case "Italic":
        case "Oblique":
          fontStyle = PdfFontStyle.Italic;
          break;
        case "Bold":
          fontStyle = PdfFontStyle.Bold;
          break;
        case "BoldItalic":
        case "BoldOblique":
          fontStyle = PdfFontStyle.Bold | PdfFontStyle.Italic;
          break;
      }
    }
    return fontStyle;
  }

  private PdfFontFamily GetFontFamily(string fontFamilyString)
  {
    int length = fontFamilyString.IndexOf("-");
    string str = fontFamilyString;
    if (length >= 0)
      str = fontFamilyString.Substring(0, length);
    return !(str == "Times") ? (PdfFontFamily) Enum.Parse(typeof (PdfFontFamily), str, true) : PdfFontFamily.TimesRoman;
  }

  private string GetKey(PdfName fontName, string keyValue)
  {
    PdfResources resources = this.GetResources();
    if (resources.ContainsKey("Font") && resources["Font"] is PdfDictionary)
    {
      System.Collections.Generic.Dictionary<PdfName, IPdfPrimitive> items = (resources["Font"] as PdfDictionary).Items;
      string empty = string.Empty;
      foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in items)
      {
        PdfDictionary pdfDictionary = (keyValuePair.Value as PdfReferenceHolder).Object as PdfDictionary;
        if (pdfDictionary.ContainsKey(keyValue) && (this.CrossTable.GetObject(pdfDictionary[keyValue]) as PdfName).Value == fontName.Value)
          return keyValuePair.Key.Value;
      }
    }
    return (string) null;
  }

  private RectangleF CalculateBounds(float x, float y, float width, float height)
  {
    width -= x;
    if ((double) height != (double) y)
      height -= y;
    return new RectangleF(x, y, width, height);
  }

  internal RectangleF GetCropBox()
  {
    if (!this.Dictionary.ContainsKey("CropBox"))
      return RectangleF.Empty;
    PdfArray pdfArray = this.Dictionary.GetValue(this.CrossTable, "CropBox", "Parent") as PdfArray;
    float floatValue = (pdfArray[2] as PdfNumber).FloatValue;
    float height = (double) (pdfArray[3] as PdfNumber).FloatValue != 0.0 ? (pdfArray[3] as PdfNumber).FloatValue : (pdfArray[1] as PdfNumber).FloatValue;
    return new RectangleF(new PointF((pdfArray[0] as PdfNumber).FloatValue, (pdfArray[1] as PdfNumber).FloatValue), new SizeF(floatValue, height));
  }
}
