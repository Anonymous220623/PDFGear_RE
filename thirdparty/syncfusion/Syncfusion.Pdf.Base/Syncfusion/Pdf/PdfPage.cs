// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfPage
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf;

public class PdfPage : PdfPageBase
{
  internal PdfSection m_section;
  private PdfAnnotationCollection m_annotations;
  internal bool IsNewPage;
  private PdfDocumentBase m_documentBase;
  internal bool templateResource;

  public PdfSection Section
  {
    get
    {
      return this.m_section != null ? this.m_section : throw new PdfException("Page must be added to some section before using.");
    }
    internal set => this.m_section = value;
  }

  public override SizeF Size => this.Section.PageSettings.Size;

  internal override PointF Origin => this.Section.PageSettings.Origin;

  public PdfAnnotationCollection Annotations
  {
    get
    {
      if (this.m_annotations == null)
      {
        this.m_annotations = new PdfAnnotationCollection(this);
        if (!this.Dictionary.ContainsKey("Annots"))
          this.Dictionary["Annots"] = ((IPdfWrapper) this.m_annotations).Element;
        this.m_annotations.Annotations = this.Dictionary["Annots"] as PdfArray;
      }
      return this.m_annotations;
    }
  }

  internal PdfDocument Document
  {
    get
    {
      return this.m_section != null && this.m_section.Parent != null ? this.m_section.Parent.Document : (PdfDocument) null;
    }
  }

  internal PdfCrossTable CrossTable
  {
    get
    {
      if (this.m_section == null)
        throw new PdfDocumentException("Page is not created");
      return this.m_section.Parent != null ? this.m_section.Parent.Document.CrossTable : this.m_section.ParentDocument.CrossTable;
    }
  }

  internal PdfDocumentBase DestinationDocument
  {
    get => this.m_documentBase;
    set => this.m_documentBase = value;
  }

  public event EventHandler BeginSave;

  public PdfPage()
    : base(new PdfDictionary())
  {
    this.Initialize();
  }

  public SizeF GetClientSize() => this.Section.GetActualBounds(this, true).Size;

  protected virtual void OnBeginSave(EventArgs e)
  {
    if (this.BeginSave == null)
      return;
    this.BeginSave((object) this, e);
  }

  internal override void Clear()
  {
    base.Clear();
    if (this.m_annotations != null)
      this.m_annotations.Clear();
    this.m_section = (PdfSection) null;
  }

  internal void AssignSection(PdfSection section)
  {
    this.m_section = this.m_section == null ? section : throw new PdfException("The page already exists in some section, it can't be contained by several sections");
    this.Dictionary["Parent"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) section);
  }

  private void Initialize()
  {
    this.Dictionary["Type"] = (IPdfPrimitive) new PdfName("Page");
    this.Dictionary.BeginSave += new SavePdfPrimitiveEventHandler(this.PageBeginSave);
    this.Dictionary.EndSave += new SavePdfPrimitiveEventHandler(this.PageEndSave);
  }

  private void DrawPageTemplates(PdfDocument document)
  {
    if (document == null)
      return;
    if (this.Section.ContainsTemplates(document, this, false))
    {
      PdfPageLayer layer = new PdfPageLayer((PdfPageBase) this, false);
      this.Layers.Insert(0, layer);
      this.Section.DrawTemplates(this, layer, document, false);
    }
    if (!this.Section.ContainsTemplates(document, this, true))
      return;
    PdfPageLayer layer1 = new PdfPageLayer((PdfPageBase) this, false);
    this.Layers.Add(layer1);
    this.Section.DrawTemplates(this, layer1, document, true);
  }

  private void RemoveTemplateLayers(PdfDocument document)
  {
    bool flag1 = document != null ? this.Section.ContainsTemplates(document, this, false) : throw new ArgumentNullException(nameof (document));
    bool flag2 = this.Section.ContainsTemplates(document, this, true);
    if (flag1)
      this.Layers.RemoveAt(0);
    if (!flag2)
      return;
    this.Layers.RemoveAt(this.Layers.Count - 1);
  }

  private void RemoveFromDocument(PdfDictionary dictionary)
  {
    dictionary.isSkip = true;
    foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in dictionary.Items)
    {
      IPdfPrimitive dictionary1 = PdfCrossTable.Dereference(keyValuePair.Value);
      if (dictionary1 is PdfDictionary)
        this.RemoveFromDocument(dictionary1 as PdfDictionary);
    }
  }

  private void AddToDocument(PdfDictionary dictionary)
  {
    dictionary.isSkip = false;
    foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in dictionary.Items)
    {
      IPdfPrimitive dictionary1 = PdfCrossTable.Dereference(keyValuePair.Value);
      if (dictionary1 is PdfDictionary)
        this.AddToDocument(dictionary1 as PdfDictionary);
    }
  }

  internal void RemoveIdenticalResources(PdfResources resources, PdfPage newPage)
  {
    PdfDictionary xObjectDictionary = PdfCrossTable.Dereference(resources["XObject"]) as PdfDictionary;
    PdfDictionary fontDictionary = PdfCrossTable.Dereference(resources["Font"]) as PdfDictionary;
    if (xObjectDictionary != null)
    {
      System.Collections.Generic.Dictionary<PdfName, IPdfPrimitive> dictionary = new System.Collections.Generic.Dictionary<PdfName, IPdfPrimitive>();
      foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in xObjectDictionary.Items)
        dictionary.Add(keyValuePair.Key, keyValuePair.Value);
      foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in dictionary)
      {
        if (PdfCrossTable.Dereference(keyValuePair.Value) is PdfDictionary xObject && xObject.ContainsKey("Subtype"))
        {
          PdfName pdfName = PdfCrossTable.Dereference(xObject["Subtype"]) as PdfName;
          if (pdfName != (PdfName) null && pdfName.Value == "Image")
          {
            if (xObject is PdfStream stream1)
              this.AddResourceCollection(stream1, newPage, xObject, xObjectDictionary, keyValuePair.Key);
          }
          else if (pdfName != (PdfName) null && pdfName.Value == "Form" && xObject.ContainsKey("Resources"))
          {
            if (PdfCrossTable.Dereference(xObject["Resources"]) is PdfDictionary baseDictionary && (baseDictionary.ContainsKey("XObject") || baseDictionary.ContainsKey("Font")))
            {
              this.RemoveIdenticalResources(new PdfResources(baseDictionary), newPage);
              if (xObject is PdfStream stream2 && newPage.DestinationDocument != null && !newPage.DestinationDocument.m_isImported && newPage.templateResource)
                this.AddResourceCollection(stream2, newPage, xObject, xObjectDictionary, keyValuePair.Key);
            }
            else if (xObject is PdfStream stream3)
              this.AddResourceCollection(stream3, newPage, xObject, xObjectDictionary, keyValuePair.Key);
          }
        }
      }
    }
    if (this.templateResource)
      return;
    this.FontOptimization(fontDictionary, newPage);
  }

  private void FontOptimization(PdfDictionary fontDictionary, PdfPage newPage)
  {
    if (fontDictionary == null)
      return;
    foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in fontDictionary.Items)
    {
      if (PdfCrossTable.Dereference(keyValuePair.Value) is PdfDictionary xObject && xObject.ContainsKey("Subtype"))
      {
        PdfName pdfName = PdfCrossTable.Dereference(xObject["Subtype"]) as PdfName;
        if (pdfName != (PdfName) null && xObject.ContainsKey("FontDescriptor"))
        {
          if (PdfCrossTable.Dereference(xObject["FontDescriptor"]) is PdfDictionary dictionary && (dictionary.ContainsKey("FontFile2") || dictionary.ContainsKey("FontFile3")))
          {
            PdfStream pdfStream = (PdfStream) null;
            if (!(PdfCrossTable.Dereference(dictionary["FontFile2"]) is PdfDictionary pdfDictionary1))
              pdfDictionary1 = PdfCrossTable.Dereference(dictionary["FontFile3"]) as PdfDictionary;
            if (pdfDictionary1 != null)
              pdfStream = pdfDictionary1 as PdfStream;
            if (pdfStream != null)
            {
              string hashValue = string.Empty;
              if (this.CompareStream(pdfStream.InternalStream, newPage, out hashValue))
              {
                if (newPage.DestinationDocument.ResourceCollection[hashValue] is PdfDictionary)
                {
                  PdfDictionary pdfDictionary2 = dictionary;
                  PdfDictionary resource = newPage.DestinationDocument.ResourceCollection[hashValue] as PdfDictionary;
                  if (pdfDictionary2.ObjectCollectionIndex != resource.ObjectCollectionIndex)
                    this.RemoveFromDocument(dictionary);
                  if (dictionary != null)
                  {
                    xObject.Items.Remove(new PdfName("FontDescriptor"));
                    xObject.Items.Add(new PdfName("FontDescriptor"), (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) resource));
                  }
                }
              }
              else if (hashValue != string.Empty)
                newPage.DestinationDocument.ResourceCollection.Add(hashValue, (IPdfPrimitive) dictionary);
            }
          }
        }
        else if (pdfName != (PdfName) null && xObject.ContainsKey("DescendantFonts"))
          this.OptimizeDescendantFonts(xObject, newPage);
      }
    }
  }

  private bool CompareStream(MemoryStream stream, PdfPage page, out string hashValue)
  {
    bool flag = false;
    hashValue = string.Empty;
    if (page == null || page.DestinationDocument == null)
      return flag;
    stream.Position = 0L;
    byte[] numArray = new byte[(int) stream.Length];
    stream.Read(numArray, 0, numArray.Length);
    stream.Position = 0L;
    hashValue = page.DestinationDocument.CreateHashFromStream(numArray);
    return page.DestinationDocument.ResourceCollection.ContainsKey(hashValue);
  }

  private void AddResourceCollection(
    PdfStream stream,
    PdfPage newPage,
    PdfDictionary xObject,
    PdfDictionary xObjectDictionary,
    PdfName xObjectkey)
  {
    string hashValue = string.Empty;
    if (this.CompareStream(stream.InternalStream, newPage, out hashValue))
    {
      PdfDictionary pdfDictionary = xObject;
      PdfDictionary resource = newPage.DestinationDocument.ResourceCollection[hashValue] as PdfDictionary;
      if (pdfDictionary.ObjectCollectionIndex != resource.ObjectCollectionIndex)
        this.RemoveFromDocument(xObject);
      if (xObject == null)
        return;
      if (xObject != null && pdfDictionary.ContainsKey("SMask") && resource.ContainsKey("SMask"))
      {
        PdfStream pdfStream1 = PdfCrossTable.Dereference(pdfDictionary["SMask"]) as PdfStream;
        PdfStream pdfStream2 = PdfCrossTable.Dereference(resource["SMask"]) as PdfStream;
        if (pdfStream1 == null || pdfStream2 == null)
          return;
        pdfStream1.InternalStream.Position = 0L;
        byte[] numArray1 = new byte[(int) pdfStream1.InternalStream.Length];
        pdfStream1.InternalStream.Read(numArray1, 0, numArray1.Length);
        pdfStream1.InternalStream.Position = 0L;
        string hashFromStream1 = newPage.DestinationDocument.CreateHashFromStream(numArray1);
        pdfStream2.InternalStream.Position = 0L;
        byte[] numArray2 = new byte[(int) pdfStream2.InternalStream.Length];
        pdfStream2.InternalStream.Read(numArray2, 0, numArray2.Length);
        pdfStream2.InternalStream.Position = 0L;
        string hashFromStream2 = newPage.DestinationDocument.CreateHashFromStream(numArray2);
        if (string.IsNullOrEmpty(hashFromStream2) || string.IsNullOrEmpty(hashFromStream1) || !(hashFromStream1 == hashFromStream2))
          return;
        xObjectDictionary.Items.Remove(xObjectkey);
        xObjectDictionary.Items.Add(xObjectkey, (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) resource));
      }
      else
      {
        if (xObject == null || stream.ContainsKey("SMask") || xObject.ContainsKey("SMask"))
          return;
        xObjectDictionary.Items.Remove(xObjectkey);
        this.AddToDocument(resource);
        xObjectDictionary.Items.Add(xObjectkey, (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) resource));
      }
    }
    else
    {
      if (!(hashValue != string.Empty))
        return;
      newPage.DestinationDocument.ResourceCollection.Add(hashValue, (IPdfPrimitive) xObject);
    }
  }

  private void OptimizeDescendantFonts(PdfDictionary xObject, PdfPage newPage)
  {
    if (!(PdfCrossTable.Dereference(xObject["DescendantFonts"]) is PdfArray pdfArray) || pdfArray.Count <= 0 || !(PdfCrossTable.Dereference(pdfArray[0]) is PdfDictionary dictionary) || !dictionary.ContainsKey("FontDescriptor") || !(PdfCrossTable.Dereference(dictionary["FontDescriptor"]) is PdfDictionary pdfDictionary1) || !pdfDictionary1.ContainsKey("FontFile2") && !pdfDictionary1.ContainsKey("FontFile3"))
      return;
    PdfStream pdfStream = (PdfStream) null;
    if (!(PdfCrossTable.Dereference(pdfDictionary1["FontFile2"]) is PdfDictionary pdfDictionary2))
      pdfDictionary2 = PdfCrossTable.Dereference(pdfDictionary1["FontFile3"]) as PdfDictionary;
    if (pdfDictionary2 != null)
      pdfStream = pdfDictionary2 as PdfStream;
    if (pdfStream == null)
      return;
    string hashValue = string.Empty;
    if (this.CompareStream(pdfStream.InternalStream, newPage, out hashValue))
    {
      if (!(newPage.DestinationDocument.ResourceCollection[hashValue] is PdfArray))
        return;
      PdfDictionary pdfDictionary3 = dictionary;
      PdfArray resource = newPage.DestinationDocument.ResourceCollection[hashValue] as PdfArray;
      PdfDictionary pdfDictionary4 = PdfCrossTable.Dereference(resource[0]) as PdfDictionary;
      if (pdfDictionary3.ObjectCollectionIndex != pdfDictionary4.ObjectCollectionIndex)
        this.RemoveFromDocument(dictionary);
      if (resource == null)
        return;
      xObject.Items.Remove(new PdfName("DescendantFonts"));
      xObject.Items.Add(new PdfName("DescendantFonts"), (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) resource));
    }
    else
    {
      if (!(hashValue != string.Empty))
        return;
      newPage.DestinationDocument.ResourceCollection.Add(hashValue, (IPdfPrimitive) pdfArray);
    }
  }

  private void PageBeginSave(object sender, SavePdfPrimitiveEventArgs args)
  {
    if (args.Writer.Document is PdfDocument document2 && this.Document != null)
    {
      this.DrawPageTemplates(document2);
      if (this.m_isProgressOn)
        this.Section.OnPageSaving(this);
      if (PdfDocument.ConformanceLevel == PdfConformanceLevel.Pdf_X1A2001)
      {
        this.Dictionary["MediaBox"] = (IPdfPrimitive) PdfArray.FromRectangle(new RectangleF(PointF.Empty, this.Size));
        this.Dictionary["TrimBox"] = (IPdfPrimitive) PdfArray.FromRectangle(new RectangleF(PointF.Empty, this.Size));
      }
      PdfPageTransition transitionSettings = this.Section.GetTransitionSettings();
      if (transitionSettings != null)
      {
        this.Dictionary.SetProperty("Dur", (IPdfPrimitive) new PdfNumber(transitionSettings.PageDuration));
        this.Dictionary.SetProperty("Trans", ((IPdfWrapper) transitionSettings).Element);
      }
    }
    else if (args.Writer.Document is PdfLoadedDocument document1 && document1.progressDelegate != null)
      document1.OnPageSave((PdfPageBase) this);
    this.OnBeginSave(new EventArgs());
  }

  private void PageEndSave(object sender, SavePdfPrimitiveEventArgs args)
  {
    if (!(args.Writer.Document is PdfDocument document) || this.Document == null)
      return;
    this.RemoveTemplateLayers(document);
  }
}
