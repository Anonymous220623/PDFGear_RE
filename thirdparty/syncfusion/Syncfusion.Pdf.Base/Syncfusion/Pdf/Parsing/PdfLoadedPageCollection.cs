// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.PdfLoadedPageCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

public class PdfLoadedPageCollection : IEnumerable
{
  private PdfDocumentBase m_document;
  private PdfCrossTable m_crossTable;
  private Dictionary<PdfDictionary, PdfPageBase> m_pagesCash;
  private PdfLoadedDocument m_loadedDocument;
  private int m_pageDuplicaton;
  internal static int m_repeatIndex;
  internal static int m_parentKidsCount;
  internal static int m_parentKidsCounttemp;
  internal static int m_nestedPages;
  private int m_sectionCount;
  private IPdfPrimitive m_pageCatalogObj;
  private PdfDictionary m_pageNodeDictionary;
  private int m_pageNodeCount;
  private PdfArray m_nodeKids;
  private int m_lastPageIndex;
  private int m_lastKidIndex;
  private PdfCrossTable m_lastCrossTable;
  private int m_pageIndex = -1;
  internal Dictionary<PdfDictionary, int> m_pageIndexCollection = new Dictionary<PdfDictionary, int>();
  private bool m_invalidPageNode;

  public int SectionCount
  {
    get
    {
      return ((this.m_crossTable.GetObject(this.m_document.Catalog["Pages"]) as PdfDictionary)["Kids"] as PdfArray).Count;
    }
  }

  private PdfLoadedDocument LoadedDocument => this.m_loadedDocument;

  public PdfPageBase this[int index] => this.GetPage(index);

  public int Count
  {
    get
    {
      int count = 0;
      if (PdfCrossTable.Dereference(this.m_document.Catalog["Pages"]) is PdfDictionary node)
        count = this.GetNodeCount(node);
      return count;
    }
  }

  private Dictionary<PdfDictionary, PdfPageBase> PageCache
  {
    get
    {
      if (this.m_pagesCash == null)
        this.m_pagesCash = new Dictionary<PdfDictionary, PdfPageBase>();
      return this.m_pagesCash;
    }
  }

  internal PdfLoadedPageCollection(PdfDocumentBase document, PdfCrossTable crossTable)
  {
    if (document == null)
      throw new ArgumentNullException(nameof (document));
    if (crossTable == null)
      throw new ArgumentNullException(nameof (crossTable));
    this.m_document = document;
    this.m_crossTable = crossTable;
  }

  public PdfPageBase Add() => this.Insert(this.Count);

  public PdfPageBase Add(SizeF size) => this.Insert(this.Count, size);

  public PdfPageBase Add(SizeF size, PdfMargins margins) => this.Insert(this.Count, size, margins);

  public PdfPageBase Add(SizeF size, PdfMargins margins, PdfPageRotateAngle rotation)
  {
    return this.Insert(this.Count, size, margins, rotation);
  }

  internal PdfPageBase Add(
    SizeF size,
    PdfMargins margins,
    PdfPageRotateAngle rotation,
    int location)
  {
    return this.Insert(location, size, margins, rotation);
  }

  internal PdfPageBase Add(PdfLoadedDocument ldDoc, PdfPageBase page, List<PdfArray> destinations)
  {
    if (ldDoc == null)
      throw new ArgumentNullException(nameof (ldDoc));
    PdfTemplate template = page != null ? page.ContentTemplate : throw new ArgumentNullException(nameof (page));
    PdfPage newPage = this.Add(page.Size, new PdfMargins(), page.Rotation) as PdfPage;
    if (page.Graphics.m_cropBox != null)
    {
      newPage.Graphics.m_cropBox = page.Graphics.m_cropBox;
      newPage.Dictionary["CropBox"] = (IPdfPrimitive) newPage.Graphics.m_cropBox;
    }
    PointF origin = page.Origin;
    if (template != null)
      newPage.Graphics.DrawPdfTemplate(template, origin);
    newPage.ImportAnnotations(ldDoc, page, destinations);
    if (page.Rotation == PdfPageRotateAngle.RotateAngle90)
    {
      newPage.Graphics.TranslateTransform(0.0f, newPage.Size.Height);
      newPage.Graphics.RotateTransform(-90f);
      newPage.Graphics.TranslateTransform(0.0f, 0.0f);
    }
    else if (page.Rotation == PdfPageRotateAngle.RotateAngle180)
    {
      newPage.Graphics.TranslateTransform(newPage.Size.Width, newPage.Size.Height);
      newPage.Graphics.RotateTransform(-180f);
      newPage.Graphics.TranslateTransform(0.0f, 0.0f);
    }
    else if (page.Rotation == PdfPageRotateAngle.RotateAngle270)
    {
      newPage.Graphics.TranslateTransform(page.Size.Width, 0.0f);
      newPage.Graphics.RotateTransform(-270f);
      newPage.Graphics.TranslateTransform(0.0f, 0.0f);
    }
    if (ldDoc.IsOptimizeIdentical && newPage.Dictionary.ContainsKey("Resources"))
    {
      newPage.DestinationDocument = ldDoc.DestinationDocument;
      PdfDictionary baseDictionary = PdfCrossTable.Dereference(newPage.Dictionary["Resources"]) as PdfDictionary;
      newPage.RemoveIdenticalResources(new PdfResources(baseDictionary), newPage);
    }
    return (PdfPageBase) newPage;
  }

  internal PdfPageBase Add(PdfLoadedDocument ldDoc, PdfPageBase page)
  {
    if (ldDoc == null)
      throw new ArgumentNullException(nameof (ldDoc));
    PdfTemplate template = page != null ? page.GetContent() : throw new ArgumentNullException(nameof (page));
    PdfPage pdfPage = this.Add(page.Size, new PdfMargins(), page.Rotation) as PdfPage;
    PointF origin = page.Origin;
    if (template != null)
      pdfPage.Graphics.DrawPdfTemplate(template, origin);
    if (pdfPage.Document != null && !pdfPage.Document.EnableMemoryOptimization)
      pdfPage.ImportAnnotations(ldDoc, page);
    return (PdfPageBase) pdfPage;
  }

  public PdfPageBase Insert(int index) => this.Insert(index, SizeF.Empty);

  public PdfPageBase Insert(int index, SizeF size) => this.Insert(index, size, (PdfMargins) null);

  public PdfPageBase Insert(int index, SizeF size, PdfMargins margins)
  {
    PdfPageRotateAngle rotation = PdfPageRotateAngle.RotateAngle0;
    return this.Insert(index, size, margins, rotation);
  }

  public PdfPageBase Insert(
    int index,
    SizeF size,
    PdfMargins margins,
    PdfPageRotateAngle rotation)
  {
    PdfPageOrientation orientation = (double) size.Width > (double) size.Height ? PdfPageOrientation.Landscape : PdfPageOrientation.Portrait;
    return this.Insert(index, size, margins, rotation, orientation);
  }

  public void RemoveAt(int index) => this.Remove(this.GetPage(index));

  public void Remove(PdfPageBase page)
  {
    int index1 = this.IndexOf(page);
    if (index1 <= -1)
      return;
    PdfLoadedDocument document = this.m_document as PdfLoadedDocument;
    Dictionary<PdfPageBase, object> destinationDictionary = document.CreateBookmarkDestinationDictionary();
    if (destinationDictionary != null)
    {
      List<object> objectList = (List<object>) null;
      if (destinationDictionary.ContainsKey(page))
        objectList = destinationDictionary[page] as List<object>;
      if (objectList != null)
      {
        for (int index2 = 0; index2 < objectList.Count; ++index2)
        {
          PdfBookmarkBase pdfBookmarkBase = objectList[index2] as PdfBookmarkBase;
          PdfDestination wrapper = (PdfDestination) null;
          if (pdfBookmarkBase.Dictionary["A"] != null)
            pdfBookmarkBase.Dictionary.SetProperty("A", (IPdfWrapper) wrapper);
          pdfBookmarkBase.Dictionary.SetProperty("Dest", (IPdfWrapper) wrapper);
        }
      }
    }
    page.m_removedPage = true;
    this.RemovePdfPageTemplates(document, page);
    PdfDictionary element = ((IPdfWrapper) page).Element as PdfDictionary;
    PdfDictionary parent = this.GetParent(index1, out int _, true);
    element["Parent"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) parent);
    PdfArray nodeKids = this.GetNodeKids(parent);
    if (index1 == 0)
    {
      PdfCrossTable crossTable = this.m_document.CrossTable;
      if (crossTable.DocumentCatalog != null)
      {
        if (crossTable.DocumentCatalog["OpenAction"] is PdfArray pdfArray1)
        {
          pdfArray1.Remove((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) element));
        }
        else
        {
          PdfReferenceHolder pdfReferenceHolder = crossTable.DocumentCatalog["OpenAction"] as PdfReferenceHolder;
          if (pdfReferenceHolder != (PdfReferenceHolder) null && (pdfReferenceHolder.Object as PdfDictionary)["D"] is PdfArray pdfArray)
            pdfArray.Remove((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) element));
        }
      }
    }
    PdfReferenceHolder pdfReferenceHolder1 = (PdfReferenceHolder) null;
    foreach (PdfReferenceHolder pdfReferenceHolder2 in nodeKids)
    {
      if (pdfReferenceHolder2.Object == element)
      {
        pdfReferenceHolder1 = pdfReferenceHolder2;
        break;
      }
    }
    if (pdfReferenceHolder1 != (PdfReferenceHolder) null)
    {
      this.RemoveFormFields(pdfReferenceHolder1);
      nodeKids.Remove((IPdfPrimitive) pdfReferenceHolder1);
    }
    this.UpdateCountDecrement(parent);
  }

  internal void RemoveFormFields(PdfReferenceHolder pageHolder)
  {
    if ((this.m_document as PdfLoadedDocument).Form == null)
      return;
    (this.m_document as PdfLoadedDocument).Form.Fields.RemoveContainingField(pageHolder);
  }

  private void RemovePdfPageTemplates(PdfLoadedDocument loadedDocument, PdfPageBase pdfPageBase)
  {
    if (PdfCrossTable.Dereference(loadedDocument.Catalog["Names"]) is PdfDictionary pdfDictionary1 && pdfDictionary1.ContainsKey("Pages") && PdfCrossTable.Dereference(pdfDictionary1["Pages"]) is PdfDictionary pdfDictionary2 && pdfDictionary2.ContainsKey("Names") && PdfCrossTable.Dereference(pdfDictionary2["Names"]) is PdfArray namedCollection1)
    {
      PdfArray pdfPagetemplates = this.GetUpdatedPdfPagetemplates(namedCollection1, loadedDocument, pdfPageBase);
      pdfDictionary1["Pages"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) new PdfDictionary()
      {
        ["Names"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfPagetemplates)
      });
    }
    if (pdfDictionary1 == null || !pdfDictionary1.ContainsKey("Templates") || !(PdfCrossTable.Dereference(pdfDictionary1["Templates"]) is PdfDictionary pdfDictionary3) || !pdfDictionary3.ContainsKey("Names") || !(PdfCrossTable.Dereference(pdfDictionary3["Names"]) is PdfArray namedCollection2))
      return;
    PdfArray pdfPagetemplates1 = this.GetUpdatedPdfPagetemplates(namedCollection2, loadedDocument, pdfPageBase);
    pdfDictionary1["Templates"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) new PdfDictionary()
    {
      ["Names"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfPagetemplates1)
    });
  }

  private PdfArray GetUpdatedPdfPagetemplates(
    PdfArray namedCollection,
    PdfLoadedDocument loadedDocument,
    PdfPageBase pdfPageBase)
  {
    PdfArray pdfPagetemplates = namedCollection;
    if (pdfPagetemplates.Count > 0)
    {
      for (int index = 1; index <= pdfPagetemplates.Count; index += 2)
      {
        if (PdfCrossTable.Dereference(pdfPagetemplates[index]) is PdfDictionary dic && loadedDocument.Pages.GetPage(dic) == pdfPageBase)
        {
          pdfPagetemplates.RemoveAt(index - 1);
          pdfPagetemplates.RemoveAt(index - 1);
          return pdfPagetemplates;
        }
      }
    }
    return pdfPagetemplates;
  }

  public void ReArrange(int[] orderArray)
  {
    int[] numArray1 = new int[orderArray.Length];
    int[] numArray2 = new int[orderArray.Length];
    int[] numArray3 = new int[orderArray.Length];
    int index1 = 0;
    int length = orderArray.Length;
    int num1 = length;
    int count1 = this.Count;
    int num2 = 0;
    for (int index2 = 0; index2 < orderArray.Length; ++index2)
    {
      if (orderArray[index2] >= this.Count)
        throw new ArgumentException("The page Index is not Valid");
    }
    PdfDictionary pdfDictionary1 = this.m_crossTable.GetObject(this.m_document.Catalog["Pages"]) as PdfDictionary;
    PdfArray pdfArray1 = pdfDictionary1["Kids"] as PdfArray;
    int count2 = pdfArray1.Count;
    this.m_loadedDocument = this.m_document as PdfLoadedDocument;
    PdfLoadedPageCollection.m_parentKidsCount = this.Count;
    for (int index3 = 0; index3 < length; ++index3)
    {
      for (int index4 = index3 + 1; index4 < length; ++index4)
      {
        if (orderArray[index3] == orderArray[index4])
        {
          this.m_pageDuplicaton = 1;
          if (numArray1[index4] == 0)
          {
            ++num2;
            numArray1[index4] = 1;
            numArray2[index1] = index4;
            ++index1;
          }
        }
      }
    }
    if (this.m_pageDuplicaton == 1)
    {
      for (int index5 = 0; index5 < numArray2.Length; ++index5)
      {
        for (int index6 = index5 + 1; index6 < numArray2.Length; ++index6)
        {
          if (numArray2[index6] != 0 && numArray2[index5] > numArray2[index6])
          {
            int num3 = numArray2[index5];
            numArray2[index5] = numArray2[index6];
            numArray2[index6] = num3;
          }
        }
      }
      int num4 = numArray2[0];
      PdfLoadedPageCollection.m_repeatIndex = num4;
      int num5 = num2;
      if (length > count1)
      {
        int index7 = num4;
        for (int index8 = 0; index8 < num5; ++index8)
        {
          int count3 = this.Count;
          this.m_loadedDocument.Pages.Add(this.m_loadedDocument, this.GetPage(orderArray[index7]));
          numArray3[index7] = 1;
          ++index7;
        }
      }
      else
      {
        int index9 = num4;
        for (int index10 = 0; index10 < num5; ++index10)
        {
          int count4 = this.Count;
          this.m_loadedDocument.Pages.Add(this.m_loadedDocument, this.GetPage(orderArray[index9]));
          numArray3[index9] = 1;
          ++index9;
          int count5 = this.Count;
        }
      }
      for (int index11 = num4; index11 < num1; ++index11)
      {
        if (numArray3[index11] == 0)
        {
          int count6 = this.Count;
          this.m_loadedDocument.Pages.Add(this.m_loadedDocument, this.GetPage(orderArray[index11]));
        }
      }
    }
    List<long> longList = new List<long>();
    int localIndex;
    PdfDictionary parent1 = this.GetParent(0, out localIndex, true);
    PdfArray nodeKids1 = this.GetNodeKids(parent1);
    PdfLoadedPageCollection.m_parentKidsCounttemp = nodeKids1.Count;
    int count7 = nodeKids1.Count;
    for (int index12 = 0; index12 < nodeKids1.Count; ++index12)
    {
      PdfReference reference = (nodeKids1[index12] as PdfReferenceHolder).Reference;
      longList.Add(reference.ObjNum);
    }
    if (count7 >= this.Count)
      PdfLoadedPageCollection.m_nestedPages = 0;
    int num6 = 0;
    while (count7 < this.Count)
    {
      PdfLoadedPageCollection.m_nestedPages = 1;
      PdfDictionary parent2 = this.GetParent(count7, out localIndex, true);
      PdfArray pdfArray2 = parent2["Kids"] as PdfArray;
      for (int index13 = 0; index13 < this.GetNodeKids(parent2).Count; ++index13)
      {
        PdfDictionary node = (pdfArray2[index13] as PdfReferenceHolder).Object as PdfDictionary;
        PdfReference reference = (pdfArray2[index13] as PdfReferenceHolder).Reference;
        if (node["Type"].ToString() == "/Pages")
        {
          PdfArray nodeKids2 = this.GetNodeKids(node);
          for (int index14 = 0; index14 < nodeKids2.Count; ++index14)
          {
            reference = (nodeKids2[index14] as PdfReferenceHolder).Reference;
            if (!longList.Contains(reference.ObjNum))
            {
              longList.Add(reference.ObjNum);
              nodeKids1.Insert(count7, this.GetNodeKids(parent2)[index13]);
              ++count7;
            }
          }
        }
        if (node["Type"].ToString() == "/Page" && !longList.Contains(reference.ObjNum))
        {
          longList.Add(reference.ObjNum);
          nodeKids1.Insert(count7, this.GetNodeKids(parent2)[index13]);
          ++count7;
        }
      }
      ++num6;
    }
    PdfLoadedPageCollection.m_parentKidsCounttemp = nodeKids1.Count;
    nodeKids1.ReArrange(orderArray);
    int num7 = count7 - orderArray.Length;
    if (num7 != 0)
    {
      for (int index15 = 0; index15 < num7; ++index15)
        this.UpdateCountDecrement(parent1);
    }
    if (PdfLoadedPageCollection.m_nestedPages != 1)
      return;
    PdfReferenceHolder pdfReferenceHolder = pdfArray1[0] as PdfReferenceHolder;
    long objNum = pdfReferenceHolder.Reference.ObjNum;
    PdfArray pdfArray3 = parent1["Kids"] as PdfArray;
    PdfReferenceHolder[] pdfReferenceHolderArray1 = new PdfReferenceHolder[pdfArray3.Count];
    PdfReferenceHolder[] pdfReferenceHolderArray2 = new PdfReferenceHolder[pdfArray3.Count];
    PdfReferenceHolder primitive1 = (pdfReferenceHolder.Object as PdfDictionary)["Parent"] as PdfReferenceHolder;
    PdfArray primitive2 = new PdfArray();
    for (int index16 = 0; index16 < pdfArray3.Count; ++index16)
    {
      PdfArray pdfArray4 = new PdfArray();
      PdfArray pdfArray5 = new PdfArray();
      pdfReferenceHolderArray1[index16] = pdfArray3[index16] as PdfReferenceHolder;
      PdfDictionary pdfDictionary2 = pdfReferenceHolderArray1[index16].Object as PdfDictionary;
      PdfDictionary pdfDictionary3 = (pdfDictionary2["Parent"] as PdfReferenceHolder).Object as PdfDictionary;
      if (pdfDictionary2["Type"].ToString() == "/Pages")
      {
        if (pdfDictionary2.ContainsKey("CropBox"))
        {
          PdfArray primitive3 = pdfDictionary2["CropBox"] as PdfArray;
          pdfDictionary2.SetProperty("CropBox", (IPdfPrimitive) primitive3);
        }
        if (pdfDictionary2.ContainsKey("MediaBox"))
        {
          PdfArray primitive4 = pdfDictionary2["MediaBox"] as PdfArray;
          pdfDictionary2.SetProperty("MediaBox", (IPdfPrimitive) primitive4);
        }
      }
      else
      {
        if (pdfDictionary3.ContainsKey("CropBox"))
        {
          PdfArray primitive5 = pdfDictionary3["CropBox"] as PdfArray;
          pdfDictionary2.SetProperty("CropBox", (IPdfPrimitive) primitive5);
        }
        if (pdfDictionary3.ContainsKey("MediaBox"))
        {
          PdfArray primitive6 = pdfDictionary3["MediaBox"] as PdfArray;
          if (!(pdfDictionary2["CropBox"] is PdfArray pdfArray6) || (double) (pdfArray6.Elements[0] as PdfNumber).FloatValue == 0.0 && (double) (pdfArray6.Elements[1] as PdfNumber).FloatValue == 0.0)
            pdfDictionary2.SetProperty("MediaBox", (IPdfPrimitive) primitive6);
        }
      }
      pdfDictionary2.SetProperty("Parent", (IPdfPrimitive) primitive1);
      pdfReferenceHolderArray2[index16] = pdfDictionary2["Parent"] as PdfReferenceHolder;
      primitive2.Add((IPdfPrimitive) pdfReferenceHolderArray1[index16]);
    }
    PdfLoadedPageCollection.m_parentKidsCounttemp = primitive2.Count;
    pdfDictionary1.SetProperty("Kids", (IPdfPrimitive) primitive2);
    pdfDictionary1.SetNumber("Count", orderArray.Length);
    if (primitive2.Count != 0)
      return;
    parent1.SetNumber("Count", 0);
  }

  private void UpdateCountDecrement(PdfDictionary parent)
  {
    for (; parent != null; parent = PdfCrossTable.Dereference(parent["Parent"]) as PdfDictionary)
    {
      if (this.GetNodeCount(parent) - 1 == 0)
      {
        PdfDictionary pdfDictionary1 = parent;
        if (PdfCrossTable.Dereference(parent["Parent"]) is PdfDictionary pdfDictionary2 && pdfDictionary2["Kids"] is PdfArray pdfArray)
          pdfArray.Remove((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary1));
      }
      int num = this.GetNodeCount(parent) - 1;
      parent.SetNumber("Count", num);
    }
  }

  public PdfPageBase Insert(
    int index,
    SizeF size,
    PdfMargins margins,
    PdfPageRotateAngle rotation,
    PdfPageOrientation orientation)
  {
    if (size == SizeF.Empty)
      size = PdfPageSize.A4;
    PdfPage page = new PdfPage();
    PdfPageSettings pageSettings = new PdfPageSettings(size, orientation, 0.0f);
    pageSettings.Size = size;
    if (margins == null)
    {
      margins = new PdfMargins();
      margins.All = 40f;
    }
    pageSettings.Margins = margins;
    pageSettings.Rotate = rotation;
    PdfSection pdfSection = new PdfSection(this.m_document, pageSettings);
    pdfSection.DropCropBox();
    pdfSection.Add(page);
    PdfDictionary element = ((IPdfWrapper) pdfSection).Element as PdfDictionary;
    int localIndex;
    PdfDictionary parent = this.GetParent(index, out localIndex, false);
    if (parent.ContainsKey("Rotate"))
    {
      int num1 = 90;
      int num2 = (int) page.Rotation * num1;
      if ((parent["Rotate"] as PdfNumber).IntValue != num2 && !element.ContainsKey("Rotate"))
        page.Dictionary["Rotate"] = (IPdfPrimitive) new PdfNumber(num2);
    }
    element["Parent"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) parent);
    this.GetNodeKids(parent).Insert(localIndex, (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) element));
    this.UpdateCount(parent);
    this.PageCache[((IPdfWrapper) page).Element as PdfDictionary] = (PdfPageBase) page;
    page.Graphics.ColorSpace = (this.m_document as PdfLoadedDocument).ColorSpace;
    page.Graphics.Layer.Colorspace = (this.m_document as PdfLoadedDocument).ColorSpace;
    return (PdfPageBase) page;
  }

  public void Insert(int index, PdfPageBase loadedPage)
  {
    if (loadedPage == null)
      throw new ArgumentNullException(nameof (loadedPage));
    if (index < 0)
      throw new ArgumentOutOfRangeException(nameof (index), "Value cannot be less than zero");
    if (index > this.Count)
      throw new ArgumentOutOfRangeException(nameof (index), "Value can not be more than number of pages in the document.");
    PdfDictionary dictionary = loadedPage.Dictionary;
    if (this.PageCache.ContainsKey(dictionary))
      throw new ArgumentException("The page already exists same page cannot be added twice", nameof (loadedPage));
    int localIndex;
    PdfDictionary parent = this.GetParent(index, out localIndex, false);
    if (dictionary.ContainsKey("Parent"))
    {
      if (PdfCrossTable.Dereference(dictionary["Parent"]) is PdfDictionary pdfDictionary1)
      {
        if (pdfDictionary1.ContainsKey("MediaBox") && !dictionary.ContainsKey("MediaBox"))
          dictionary.Items.Add((PdfName) "MediaBox", pdfDictionary1["MediaBox"]);
        if (pdfDictionary1.ContainsKey("Rotate") && !dictionary.ContainsKey("Rotate"))
          dictionary.Items.Add((PdfName) "Rotate", pdfDictionary1["Rotate"]);
      }
      if (!dictionary.ContainsKey("MediaBox") && PdfCrossTable.Dereference(pdfDictionary1["Parent"]) is PdfDictionary pdfDictionary2 && pdfDictionary2["MediaBox"] != null)
        dictionary.Items.Add((PdfName) "MediaBox", pdfDictionary2["MediaBox"]);
      dictionary["Parent"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) parent);
    }
    if (dictionary.ContainsKey("Contents"))
    {
      PdfArray pdfArray = loadedPage.ReInitializeContentReference();
      if (pdfArray.Elements.Count > 0)
        dictionary["Contents"] = (IPdfPrimitive) pdfArray;
    }
    if (dictionary.ContainsKey("Resources"))
    {
      PdfDictionary pdfDictionary = loadedPage.ReinitializePageResources();
      if (dictionary["Resources"] as PdfReferenceHolder != (PdfReferenceHolder) null)
        dictionary["Resources"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary);
    }
    if (dictionary.ContainsKey("Annots"))
    {
      PdfCatalog catalog1 = (loadedPage as PdfLoadedPage).Document.Catalog;
      PdfDictionary acroFormData = (PdfDictionary) null;
      if (catalog1.ContainsKey("AcroForm"))
        acroFormData = !(catalog1["AcroForm"] as PdfReferenceHolder != (PdfReferenceHolder) null) ? catalog1["AcroForm"] as PdfDictionary : PdfCrossTable.Dereference(catalog1["AcroForm"]) as PdfDictionary;
      loadedPage.ReInitializePageAnnotation(acroFormData);
      if (acroFormData != null)
      {
        PdfCatalog catalog2 = this.m_document.Catalog;
        catalog2.Items.Add((PdfName) "AcroForm", (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) acroFormData));
        catalog2.Modify();
      }
    }
    if (dictionary.ContainsKey("Thumb"))
      loadedPage.ReInitializeThumbnail();
    this.GetNodeKids(parent).Insert(localIndex, (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) dictionary));
    this.UpdateCount(parent);
    this.PageCache[dictionary] = loadedPage;
  }

  internal PdfPageBase GetPage(PdfDictionary dic)
  {
    Dictionary<PdfDictionary, PdfPageBase> pageCache = this.PageCache;
    PdfPageBase page = (PdfPageBase) null;
    if (pageCache.ContainsKey(dic))
      page = pageCache[dic];
    if (page == null)
    {
      page = (PdfPageBase) new PdfLoadedPage(this.m_document, this.m_crossTable, dic);
      pageCache[dic] = page;
    }
    return page;
  }

  internal void UpdateCount(PdfDictionary parent)
  {
    for (; parent != null; parent = PdfCrossTable.Dereference(parent["Parent"]) as PdfDictionary)
    {
      int num = this.GetNodeCount(parent) + 1;
      parent.SetNumber("Count", num);
    }
  }

  internal int IndexOf(PdfPageBase page)
  {
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    int num = -1;
    int index = 0;
    for (int count = this.Count; index < count; ++index)
    {
      if (this.GetPage(index) == page)
      {
        num = index;
        break;
      }
    }
    return num;
  }

  internal int GetIndex(PdfPageBase page)
  {
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    int index = -1;
    if (this.m_pageIndexCollection.Count == 0)
      this.ParsePageNodes(page);
    if (this.m_pageIndexCollection.ContainsKey(page.Dictionary))
      index = this.m_pageIndexCollection[page.Dictionary];
    return index;
  }

  private void ParsePageNodes(PdfPageBase page)
  {
    PdfDictionary pages = new PdfDictionary();
    if (this.m_crossTable != null && this.m_crossTable.DocumentCatalog != null)
    {
      pages = PdfCrossTable.Dereference(this.m_crossTable.DocumentCatalog["Pages"]) as PdfDictionary;
    }
    else
    {
      switch (page)
      {
        case PdfLoadedPage _:
          pages = PdfCrossTable.Dereference((page as PdfLoadedPage).Document.Catalog["Pages"]) as PdfDictionary;
          break;
        case PdfPage _:
          pages = PdfCrossTable.Dereference((page as PdfPage).Document.Catalog["Pages"]) as PdfDictionary;
          break;
      }
    }
    this.FindKids(pages);
  }

  private void FindKids(PdfDictionary pages)
  {
    if (!(PdfCrossTable.Dereference(pages["Kids"]) is PdfArray pdfArray))
      return;
    for (int index = 0; index < pdfArray.Count; ++index)
      this.FindPageNodes(PdfCrossTable.Dereference(pdfArray[index]) as PdfDictionary);
  }

  private void FindPageNodes(PdfDictionary pageNode)
  {
    if (pageNode == null || !pageNode.ContainsKey("Type"))
      return;
    switch ((PdfCrossTable.Dereference(pageNode["Type"]) as PdfName).Value)
    {
      case "Page":
        this.m_pageIndexCollection.Add(pageNode, ++this.m_pageIndex);
        break;
      case "Pages":
        this.FindKids(pageNode);
        break;
    }
  }

  private PdfPageBase GetPage(int index)
  {
    int localIndex;
    PdfArray nodeKids1 = this.GetNodeKids(this.GetParent(index, out localIndex, true, true));
    int index1 = localIndex;
    int index2 = 0;
    PdfDictionary pdfDictionary;
    PdfArray nodeKids2;
    do
    {
      pdfDictionary = this.m_crossTable.GetObject(nodeKids1[localIndex]) as PdfDictionary;
      string empty = string.Empty;
      if (pdfDictionary != null && pdfDictionary.ContainsKey("Type"))
        empty = (PdfCrossTable.Dereference(pdfDictionary["Type"]) as PdfName).Value;
      if (empty == "Pages")
      {
        ++index1;
        pdfDictionary = this.m_crossTable.GetObject(nodeKids1[index1]) as PdfDictionary;
        nodeKids2 = this.GetNodeKids(pdfDictionary);
        if (nodeKids2 == null)
          goto label_7;
      }
      else
        goto label_7;
    }
    while (nodeKids2.Count <= 0);
    pdfDictionary = this.m_crossTable.GetObject(nodeKids2[index2]) as PdfDictionary;
    int num = index2 + 1;
label_7:
    return this.GetPage(pdfDictionary);
  }

  private bool IsNodeLeaf(PdfDictionary node) => this.GetNodeCount(node) == 0;

  private PdfArray GetNodeKids(PdfDictionary node)
  {
    return this.m_crossTable.GetObject(node["Kids"]) as PdfArray;
  }

  private int GetNodeCount(PdfDictionary node)
  {
    return !(this.m_crossTable.GetObject(node["Count"]) is PdfNumber pdfNumber) ? 0 : pdfNumber.IntValue;
  }

  private PdfDictionary GetParent(int index, out int localIndex, bool zeroValid)
  {
    if (index < 0 && index > this.Count)
      throw new ArgumentOutOfRangeException(nameof (index), "The index should be within this range: [0; Count]");
    PdfDictionary node1 = this.m_crossTable.GetObject(this.m_document.Catalog["Pages"]) as PdfDictionary;
    int num = 0;
    localIndex = this.GetNodeCount(node1);
    if (index == 0 && !zeroValid)
      localIndex = 0;
    else if (index < this.Count)
    {
      PdfArray nodeKids = this.GetNodeKids(node1);
      for (int index1 = 0; index1 < nodeKids.Count; ++index1)
      {
        PdfReferenceHolder element = nodeKids.Elements[index1] as PdfReferenceHolder;
        if (element != (PdfReferenceHolder) null)
        {
          foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in (element.Object as PdfDictionary).Items)
          {
            if (keyValuePair.Key.Value == "Kids")
            {
              PdfArray pdfArray = (object) (keyValuePair.Value as PdfReferenceHolder) == null ? keyValuePair.Value as PdfArray : (keyValuePair.Value as PdfReferenceHolder).Object as PdfArray;
              if (pdfArray != null && pdfArray.Elements.Count == 0)
                nodeKids.RemoveAt(index1);
            }
          }
        }
      }
      int index2 = 0;
      for (int count = nodeKids.Count; index2 < count; ++index2)
      {
        PdfDictionary node2 = this.m_crossTable.GetObject(nodeKids[index2]) as PdfDictionary;
        string str = (node2["Type"] as PdfName).Value;
        if (this.IsNodeLeaf(node2) && !(str == "Pages"))
        {
          if (num + index2 == index)
          {
            localIndex = index2;
            break;
          }
        }
        else
        {
          int nodeCount = this.GetNodeCount(node2);
          if (index < num + nodeCount + index2)
          {
            num += index2;
            node1 = node2;
            nodeKids = this.GetNodeKids(node1);
            index2 = -1;
            count = nodeKids.Count;
          }
          else
            num += nodeCount - 1;
        }
      }
    }
    else
      localIndex = this.GetNodeKids(node1).Count;
    return node1;
  }

  private PdfDictionary GetParent(
    int index,
    out int localIndex,
    bool zeroValid,
    bool enableFastFetching)
  {
    if (index < 0 && index > this.Count)
      throw new ArgumentOutOfRangeException(nameof (index), "The index should be within this range: [0; Count]");
    if (!enableFastFetching)
      return this.GetParent(index, out localIndex, zeroValid);
    if (this.m_pageCatalogObj == null)
      this.m_pageCatalogObj = this.m_document.Catalog["Pages"];
    bool flag = false;
    PdfDictionary node1;
    if (this.m_pageNodeDictionary == null)
    {
      this.m_pageNodeDictionary = this.m_crossTable.GetObject(this.m_pageCatalogObj) as PdfDictionary;
      node1 = this.m_pageNodeDictionary;
      this.m_pageNodeCount = this.GetNodeCount(node1);
      this.m_lastCrossTable = this.m_crossTable;
      flag = true;
    }
    else if (this.m_lastCrossTable == this.m_crossTable)
    {
      node1 = this.m_pageNodeDictionary;
    }
    else
    {
      this.m_pageNodeDictionary = this.m_crossTable.GetObject(this.m_pageCatalogObj) as PdfDictionary;
      node1 = this.m_pageNodeDictionary;
      this.m_pageNodeCount = this.GetNodeCount(node1);
      this.m_lastCrossTable = this.m_crossTable;
      flag = true;
    }
    int lowIndex = 0;
    localIndex = this.m_pageNodeCount <= 0 ? this.GetNodeCount(node1) : this.m_pageNodeCount;
    if (index == 0 && !zeroValid)
      localIndex = 0;
    else if (index < this.Count)
    {
      PdfArray nodeKids;
      if (this.m_nodeKids == null || flag)
      {
        this.m_nodeKids = this.GetNodeKids(node1);
        nodeKids = this.m_nodeKids;
        for (int index1 = 0; index1 < nodeKids.Count; ++index1)
        {
          PdfReferenceHolder element = nodeKids.Elements[index1] as PdfReferenceHolder;
          if (element != (PdfReferenceHolder) null)
          {
            foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in (element.Object as PdfDictionary).Items)
            {
              if (keyValuePair.Key.Value == "Kids")
              {
                PdfArray pdfArray = (object) (keyValuePair.Value as PdfReferenceHolder) == null ? keyValuePair.Value as PdfArray : (keyValuePair.Value as PdfReferenceHolder).Object as PdfArray;
                if (pdfArray != null && pdfArray.Elements.Count == 0)
                  nodeKids.RemoveAt(index1);
              }
            }
          }
        }
      }
      else
        nodeKids = this.m_nodeKids;
      int kidStartIndex = 0;
      if ((this.m_lastPageIndex == index - 1 || this.m_lastPageIndex < index) && this.m_lastKidIndex < nodeKids.Count)
        kidStartIndex = this.m_lastKidIndex;
      bool isParentFetched = false;
      PdfDictionary node2 = (PdfDictionary) null;
      int localIndex1 = 0;
      if (nodeKids.Count == this.Count)
      {
        this.GetParentNode(kidStartIndex, nodeKids, lowIndex, index, out node2, out localIndex1, out isParentFetched);
        if (!isParentFetched)
          this.GetParentNode(0, nodeKids, lowIndex, index, out node2, out localIndex1, out isParentFetched);
      }
      else
        this.GetParentNode(0, nodeKids, lowIndex, index, out node2, out localIndex1, out isParentFetched);
      if (node2 != null)
        node1 = node2;
      if (localIndex1 != -1)
        localIndex = localIndex1;
      if (this.m_invalidPageNode)
        localIndex = this.GetNodeKids(node1).Count - 1;
    }
    else
      localIndex = this.GetNodeKids(node1).Count;
    this.m_lastPageIndex = index;
    return node1;
  }

  private void GetParentNode(
    int kidStartIndex,
    PdfArray kids,
    int lowIndex,
    int pageIndex,
    out PdfDictionary node,
    out int localIndex,
    out bool isParentFetched)
  {
    isParentFetched = false;
    node = (PdfDictionary) null;
    localIndex = -1;
    bool flag = false;
    int index = kidStartIndex;
    for (int count = kids.Count; index < count; ++index)
    {
      PdfDictionary node1 = this.m_crossTable.GetObject(kids[index]) as PdfDictionary;
      string empty = string.Empty;
      if (node1 != null && node1.ContainsKey("Type"))
        empty = (PdfCrossTable.Dereference(node1["Type"]) as PdfName).Value;
      if (node1 != null)
      {
        if (this.IsNodeLeaf(node1) && !(empty == "Pages"))
        {
          if (lowIndex + index == pageIndex)
          {
            localIndex = index;
            isParentFetched = true;
            if (flag)
              break;
            this.m_lastKidIndex = index;
            break;
          }
        }
        else
        {
          int nodeCount = this.GetNodeCount(node1);
          if (pageIndex < lowIndex + nodeCount + index)
          {
            flag = true;
            this.m_lastKidIndex = index;
            lowIndex += index;
            node = node1;
            kids = this.GetNodeKids(node);
            index = -1;
            count = kids.Count;
          }
          else
            lowIndex += nodeCount - 1;
        }
      }
      else
      {
        kids.RemoveAt(index);
        this.m_invalidPageNode = true;
        if (node != null && node.ContainsKey("Count") && PdfCrossTable.Dereference(node["Count"]) is PdfNumber pdfNumber2)
        {
          int num1 = pdfNumber2.IntValue - 1;
          node["Count"] = (IPdfPrimitive) new PdfNumber(num1);
          if (node.ContainsKey("Parent") && PdfCrossTable.Dereference(node["Parent"]) is PdfDictionary pdfDictionary1 && pdfDictionary1.ContainsKey("Count") && PdfCrossTable.Dereference(pdfDictionary1["Count"]) is PdfNumber pdfNumber1)
          {
            int num2 = pdfNumber1.IntValue - 1;
            pdfDictionary1["Count"] = (IPdfPrimitive) new PdfNumber(num2);
            if (this.m_crossTable != null && this.m_pageCatalogObj != null && this.m_crossTable.GetObject(this.m_pageCatalogObj) is PdfDictionary pdfDictionary && PdfCrossTable.Dereference(pdfDictionary["Count"]) is PdfNumber pdfNumber)
            {
              int num3 = pdfNumber.IntValue - 1;
              pdfDictionary["Count"] = (IPdfPrimitive) new PdfNumber(num3);
            }
          }
        }
      }
    }
  }

  internal void Clear(bool isCompletely)
  {
    if (this.m_pagesCash != null && this.m_pagesCash.Count > 0)
    {
      if (this.m_document != null && this.m_document.Catalog != null)
      {
        IPdfPrimitive pointer = this.m_document.Catalog["Pages"];
        if (pointer != null && this.m_crossTable != null && PdfCrossTable.Dereference(this.m_crossTable.GetObject(pointer)) is PdfDictionary pdfDictionary)
          pdfDictionary.Clear();
      }
      this.m_pagesCash.Clear();
    }
    if (isCompletely && this.m_crossTable != null)
    {
      this.m_crossTable.isCompletely = true;
      this.m_crossTable.Close(true);
      this.m_crossTable.isCompletely = false;
    }
    this.m_crossTable = (PdfCrossTable) null;
    this.m_document = (PdfDocumentBase) null;
    this.m_loadedDocument = (PdfLoadedDocument) null;
  }

  public IEnumerator GetEnumerator() => (IEnumerator) new PdfLoadedPageEnumerator(this);
}
