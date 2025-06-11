// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfPageLayerCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf;

public class PdfPageLayerCollection : PdfCollection
{
  private PdfPageBase m_page;
  internal bool m_sublayer;
  private int parentLayerCount;
  internal PdfDictionary m_OptionalContent = new PdfDictionary();
  private System.Collections.Generic.List<string> m_layerCollection = new System.Collections.Generic.List<string>();
  private bool m_isLayerContainsResource;
  private static object s_syncLockLayer = new object();
  private bool isLayerPresent;
  private int m_bdcCount;
  private Dictionary<PdfReferenceHolder, PdfDictionary> documentLayers;
  private static object m_resourceLock = new object();
  internal bool isOptimizeContent;

  public PdfPageLayerCollection()
  {
  }

  public PdfPageLayer this[int index]
  {
    get => this.List[index] as PdfPageLayer;
    set
    {
      if (value == null)
        throw new ArgumentNullException("layer");
      if (value.Page != this.m_page)
        throw new ArgumentException("The layer belongs to another page");
      PdfPageLayer layer = this[index];
      if (layer != null)
        this.RemoveLayer(layer);
      this.List[index] = (object) value;
      this.InsertLayer(index, value);
    }
  }

  private bool IsSkip => this.m_bdcCount > 0;

  public PdfPageLayerCollection(PdfPageBase page)
  {
    this.m_page = page != null ? page : throw new ArgumentNullException(nameof (page));
    PdfPageBase loadedPage = page;
    if (loadedPage == null)
      return;
    this.ParseLayers(loadedPage);
  }

  public PdfPageLayer Add()
  {
    lock (PdfPageLayerCollection.s_syncLockLayer)
    {
      PdfPageLayer layer = new PdfPageLayer(this.m_page);
      layer.Name = string.Empty;
      this.Add(layer);
      return layer;
    }
  }

  public PdfPageLayer Add(string LayerName, bool Visible)
  {
    lock (PdfPageLayerCollection.s_syncLockLayer)
    {
      PdfPageLayer layer = new PdfPageLayer(this.m_page);
      layer.Name = LayerName;
      layer.Visible = Visible;
      layer.LayerId = "OCG_" + Guid.NewGuid().ToString();
      this.Add(layer);
      return layer;
    }
  }

  public PdfPageLayer Add(string LayerName)
  {
    lock (PdfPageLayerCollection.s_syncLockLayer)
    {
      PdfPageLayer layer = new PdfPageLayer(this.m_page);
      layer.Name = LayerName;
      layer.LayerId = "OCG_" + Guid.NewGuid().ToString();
      this.Add(layer);
      return layer;
    }
  }

  public int Add(PdfPageLayer layer)
  {
    lock (PdfPageLayerCollection.s_syncLockLayer)
    {
      if (layer == null)
        throw new ArgumentNullException(nameof (layer));
      if (layer.Page != this.m_page)
        throw new ArgumentException("The layer belongs to another page");
      this.List.Add((object) layer);
      int index = this.List.Count - 1;
      this.AddLayer(index, layer);
      if (layer.LayerId != null)
      {
        if (this.m_page is PdfPage)
          this.CreateLayer(layer);
        else
          this.CreateLayerLoadedPage(layer);
      }
      return index;
    }
  }

  private void CreateLayer(PdfPageLayer layer)
  {
    PdfPage page = this.m_page as PdfPage;
    PdfDocumentBase pdfDocumentBase = (PdfDocumentBase) page.Document ?? page.Section.ParentDocument;
    PdfDictionary primitive = new PdfDictionary();
    IPdfPrimitive contentDictionary = this.CreateOptionalContentDictionary(layer);
    primitive["OCGs"] = contentDictionary;
    primitive["D"] = this.CreateOptionalContentViews(layer);
    pdfDocumentBase?.Catalog.SetProperty("OCProperties", (IPdfPrimitive) primitive);
  }

  private PdfDictionary setPrintOption(PdfPageLayer layer)
  {
    PdfDictionary pdfDictionary = new PdfDictionary();
    layer.m_printOption = new PdfDictionary();
    layer.m_printOption.SetProperty("Subtype", (IPdfPrimitive) new PdfName("Print"));
    if (layer.PrintState.Equals((object) PdfPrintState.NeverPrint))
      layer.m_printOption.SetProperty("PrintState", (IPdfPrimitive) new PdfName("OFF"));
    else if (layer.PrintState.Equals((object) PdfPrintState.AlwaysPrint))
      layer.m_printOption.SetProperty("PrintState", (IPdfPrimitive) new PdfName("ON"));
    pdfDictionary.SetProperty("Print", (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) layer.m_printOption));
    return pdfDictionary;
  }

  private void CreateLayerLoadedPage(PdfPageLayer layer)
  {
    PdfLoadedPage page = this.m_page as PdfLoadedPage;
    PdfDictionary primitive = new PdfDictionary();
    IPdfPrimitive contentDictionary = this.CreateOptionalContentDictionary(layer, true);
    bool flag = false;
    if (page != null && page.Document != null && page.Document.Catalog != null && page.Document.Catalog.ContainsKey("OCProperties") && PdfCrossTable.Dereference(page.Document.Catalog["OCProperties"]) is PdfDictionary pdfDictionary2 && pdfDictionary2.ContainsKey("OCGs"))
    {
      if (PdfCrossTable.Dereference(pdfDictionary2["OCGs"]) is PdfArray pdfArray1)
      {
        flag = true;
        if (!pdfArray1.Contains((IPdfPrimitive) layer.ReferenceHolder))
          pdfArray1.Add((IPdfPrimitive) layer.ReferenceHolder);
      }
      if (pdfDictionary2.ContainsKey("D") && pdfDictionary2["D"] is PdfDictionary pdfDictionary1)
      {
        PdfArray pdfArray2 = PdfCrossTable.Dereference(pdfDictionary1["ON"]) as PdfArray;
        PdfArray pdfArray3 = PdfCrossTable.Dereference(pdfDictionary1["Order"]) as PdfArray;
        PdfArray pdfArray4 = PdfCrossTable.Dereference(pdfDictionary1["OFF"]) as PdfArray;
        PdfArray pdfArray5 = PdfCrossTable.Dereference(pdfDictionary1["AS"]) as PdfArray;
        if (pdfArray2 == null)
        {
          pdfArray2 = new PdfArray();
          pdfDictionary1["ON"] = (IPdfPrimitive) pdfArray2;
        }
        if (pdfArray3 != null && !pdfArray3.Contains((IPdfPrimitive) layer.ReferenceHolder))
          pdfArray3.Add((IPdfPrimitive) layer.ReferenceHolder);
        if (layer.Visible && !pdfArray2.Contains((IPdfPrimitive) layer.ReferenceHolder))
          pdfArray2.Add((IPdfPrimitive) layer.ReferenceHolder);
        if (!layer.Visible && pdfArray4 != null && !pdfArray4.Contains((IPdfPrimitive) layer.ReferenceHolder))
          pdfArray4.Add((IPdfPrimitive) layer.ReferenceHolder);
        if (pdfArray5 != null && pdfArray5.Count > 0 && PdfCrossTable.Dereference(pdfArray5[0]) is PdfDictionary pdfDictionary && pdfDictionary.ContainsKey("OCGs") && PdfCrossTable.Dereference(pdfDictionary["OCGs"]) is PdfArray pdfArray6 && !pdfArray6.Contains((IPdfPrimitive) layer.ReferenceHolder))
          pdfArray6.Add((IPdfPrimitive) layer.ReferenceHolder);
      }
    }
    if (flag || page == null || page.Document == null || page.Document.Catalog == null)
      return;
    primitive["OCGs"] = contentDictionary;
    primitive["D"] = this.CreateOptionalContentViews(layer, true);
    page.Document.Catalog.SetProperty("OCProperties", (IPdfPrimitive) primitive);
  }

  public void Insert(int index, PdfPageLayer layer)
  {
    if (index < 0)
      throw new ArgumentOutOfRangeException(nameof (index), "Value can not be less 0");
    if (layer == null)
      throw new ArgumentNullException(nameof (layer));
    if (layer.Page != this.m_page)
      throw new ArgumentException("The layer belongs to another page");
    if (!this.List.Contains((object) layer) && layer.LayerId != null)
      this.List.RemoveAt(this.Add(layer));
    this.List.Insert(index, (object) layer);
    this.InsertLayer(index, layer);
  }

  private IPdfPrimitive CreateOptionalContentDictionary(PdfPageLayer layer)
  {
    PdfPage page = this.m_page as PdfPage;
    PdfDocumentBase pdfDocumentBase = (PdfDocumentBase) page.Document ?? page.Section.ParentDocument;
    PdfDictionary pdfDictionary = new PdfDictionary();
    pdfDictionary["Name"] = (IPdfPrimitive) new PdfString(layer.Name);
    pdfDictionary["Type"] = (IPdfPrimitive) new PdfName("OCG");
    pdfDictionary["LayerID"] = (IPdfPrimitive) new PdfName(layer.LayerId);
    pdfDictionary["Visible"] = (IPdfPrimitive) new PdfBoolean(layer.Visible);
    if (layer.PrintState.Equals((object) PdfPrintState.AlwaysPrint) || layer.PrintState.Equals((object) PdfPrintState.NeverPrint) || layer.PrintState.Equals((object) PdfPrintState.PrintWhenVisible))
    {
      layer.m_usage = this.setPrintOption(layer);
      pdfDictionary["Usage"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) layer.m_usage);
      pdfDocumentBase?.m_printLayer.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary));
    }
    PdfReferenceHolder pdfReferenceHolder = new PdfReferenceHolder((IPdfPrimitive) pdfDictionary);
    pdfDocumentBase?.primitive.Insert(pdfDocumentBase.m_positon, (IPdfPrimitive) pdfReferenceHolder);
    layer.Dictionary = pdfDictionary;
    layer.ReferenceHolder = pdfReferenceHolder;
    if (pdfDocumentBase != null)
    {
      if (!this.m_sublayer)
      {
        layer.m_sublayer = false;
        if (pdfDocumentBase.m_sublayerposition > 0)
        {
          int count = this.m_page.Contents.Count;
          this.m_page.Contents.RemoveAt(this.parentLayerCount - 1);
          PdfStream pdfStream = new PdfStream();
          byte[] bytes = Encoding.ASCII.GetBytes("EMC\n");
          pdfStream.Write(bytes);
          this.m_page.Contents.Insert(count - 2, (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfStream));
        }
        pdfDocumentBase.m_sublayerposition = 0;
        pdfDocumentBase.m_sublayer = new PdfArray();
        pdfDocumentBase.m_order.Insert(pdfDocumentBase.m_orderposition, (IPdfPrimitive) pdfReferenceHolder);
        ++pdfDocumentBase.m_orderposition;
        this.parentLayerCount = this.m_page.Contents.Count;
      }
      else
      {
        layer.m_sublayer = true;
        pdfDocumentBase.m_sublayer.Insert(pdfDocumentBase.m_sublayerposition, (IPdfPrimitive) pdfReferenceHolder);
        if (pdfDocumentBase.m_sublayerposition != 0)
        {
          pdfDocumentBase.m_order.RemoveAt(pdfDocumentBase.m_orderposition - 1);
          --pdfDocumentBase.m_orderposition;
        }
        pdfDocumentBase.m_order.Insert(pdfDocumentBase.m_orderposition, (IPdfPrimitive) pdfDocumentBase.m_sublayer);
        ++pdfDocumentBase.m_sublayerposition;
        ++pdfDocumentBase.m_orderposition;
      }
      if (layer.Visible)
      {
        pdfDocumentBase.m_on.Insert(pdfDocumentBase.m_onpositon, (IPdfPrimitive) pdfReferenceHolder);
        ++pdfDocumentBase.m_onpositon;
      }
      else
      {
        pdfDocumentBase.m_off.Insert(pdfDocumentBase.m_offpositon, (IPdfPrimitive) pdfReferenceHolder);
        ++pdfDocumentBase.m_offpositon;
      }
      ++pdfDocumentBase.m_positon;
    }
    page.GetResources().AddProperties(layer.LayerId, pdfReferenceHolder);
    PdfArray contentDictionary = (PdfArray) null;
    if (pdfDocumentBase != null)
      contentDictionary = pdfDocumentBase.primitive;
    return (IPdfPrimitive) contentDictionary;
  }

  private IPdfPrimitive CreateOptionalContentDictionary(PdfPageLayer layer, bool isLoadedPage)
  {
    PdfLoadedPage page = this.m_page as PdfLoadedPage;
    PdfDictionary pdfDictionary1 = new PdfDictionary();
    pdfDictionary1["Name"] = (IPdfPrimitive) new PdfString(layer.Name);
    pdfDictionary1["Type"] = (IPdfPrimitive) new PdfName("OCG");
    pdfDictionary1["LayerID"] = (IPdfPrimitive) new PdfName(layer.LayerId);
    pdfDictionary1["Visible"] = (IPdfPrimitive) new PdfBoolean(layer.Visible);
    if (layer.PrintState.Equals((object) PdfPrintState.AlwaysPrint) || layer.PrintState.Equals((object) PdfPrintState.NeverPrint) || layer.PrintState.Equals((object) PdfPrintState.PrintWhenVisible))
    {
      layer.m_usage = this.setPrintOption(layer);
      pdfDictionary1["Usage"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) layer.m_usage);
      page.Document.m_printLayer.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary1));
    }
    PdfReferenceHolder pdfReferenceHolder = new PdfReferenceHolder((IPdfPrimitive) pdfDictionary1);
    page.Document.primitive.Insert(page.Document.m_positon, (IPdfPrimitive) pdfReferenceHolder);
    layer.Dictionary = pdfDictionary1;
    layer.ReferenceHolder = pdfReferenceHolder;
    if (!this.m_sublayer)
    {
      layer.m_sublayer = false;
      if (page.Document.m_sublayerposition > 0)
      {
        int count = this.m_page.Contents.Count;
        this.m_page.Contents.RemoveAt(this.parentLayerCount - 1);
        PdfStream pdfStream = new PdfStream();
        byte[] bytes = Encoding.ASCII.GetBytes("EMC\n");
        pdfStream.Write(bytes);
        this.m_page.Contents.Insert(count - 2, (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfStream));
      }
      page.Document.m_sublayerposition = 0;
      page.Document.m_sublayer = new PdfArray();
      page.Document.m_order.Insert(page.Document.m_orderposition, (IPdfPrimitive) pdfReferenceHolder);
      ++page.Document.m_orderposition;
      this.parentLayerCount = this.m_page.Contents.Count;
    }
    else
    {
      layer.m_sublayer = true;
      page.Document.m_sublayer.Insert(page.Document.m_sublayerposition, (IPdfPrimitive) pdfReferenceHolder);
      if (page.Document.m_sublayerposition != 0)
      {
        page.Document.m_order.RemoveAt(page.Document.m_orderposition - 1);
        --page.Document.m_orderposition;
      }
      page.Document.m_order.Insert(page.Document.m_orderposition, (IPdfPrimitive) page.Document.m_sublayer);
      ++page.Document.m_sublayerposition;
      ++page.Document.m_orderposition;
    }
    if (layer.Visible)
    {
      page.Document.m_on.Insert(page.Document.m_onpositon, (IPdfPrimitive) pdfReferenceHolder);
      ++page.Document.m_onpositon;
    }
    else
    {
      page.Document.m_off.Insert(page.Document.m_offpositon, (IPdfPrimitive) pdfReferenceHolder);
      ++page.Document.m_offpositon;
    }
    ++page.Document.m_positon;
    PdfResources resources = page.GetResources();
    if (resources != null && resources.ContainsKey("Properties"))
    {
      if (resources["Properties"] is PdfDictionary pdfDictionary2)
      {
        this.m_isLayerContainsResource = true;
        pdfDictionary2[layer.LayerId] = (IPdfPrimitive) pdfReferenceHolder;
      }
      else
        resources.AddProperties(layer.LayerId, pdfReferenceHolder);
    }
    else
      resources.AddProperties(layer.LayerId, pdfReferenceHolder);
    return (IPdfPrimitive) page.Document.primitive;
  }

  private void WriteEndMark()
  {
    PdfStream pdfStream = new PdfStream();
    byte[] bytes = Encoding.ASCII.GetBytes("EMC\n");
    pdfStream.Write(bytes);
    this.m_page.Contents.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfStream));
  }

  private IPdfPrimitive CreateOptionalContentViews(PdfPageLayer layer)
  {
    PdfPage page = this.m_page as PdfPage;
    PdfDocumentBase pdfDocumentBase = (PdfDocumentBase) page.Document ?? page.Section.ParentDocument;
    PdfArray pdfArray = new PdfArray();
    this.m_OptionalContent["Name"] = (IPdfPrimitive) new PdfString("Layers");
    if (pdfDocumentBase != null)
    {
      this.m_OptionalContent["Order"] = (IPdfPrimitive) pdfDocumentBase.m_order;
      this.m_OptionalContent["ON"] = (IPdfPrimitive) pdfDocumentBase.m_on;
      this.m_OptionalContent["OFF"] = (IPdfPrimitive) pdfDocumentBase.m_off;
    }
    PdfArray primitive = new PdfArray();
    primitive.Add((IPdfPrimitive) new PdfName("Print"));
    PdfDictionary pdfDictionary = new PdfDictionary();
    pdfDictionary.SetProperty("Category", (IPdfPrimitive) primitive);
    if (pdfDocumentBase != null)
      pdfDictionary.SetProperty("OCGs", (IPdfPrimitive) pdfDocumentBase.m_printLayer);
    pdfDictionary.SetProperty("Event", (IPdfPrimitive) new PdfName("Print"));
    pdfArray.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary));
    if (PdfDocument.ConformanceLevel != PdfConformanceLevel.Pdf_A3B && PdfDocument.ConformanceLevel != PdfConformanceLevel.Pdf_A3A && PdfDocument.ConformanceLevel != PdfConformanceLevel.Pdf_A3U && PdfDocument.ConformanceLevel != PdfConformanceLevel.Pdf_A2B && PdfDocument.ConformanceLevel != PdfConformanceLevel.Pdf_A2A && PdfDocument.ConformanceLevel != PdfConformanceLevel.Pdf_A2U)
      this.m_OptionalContent["AS"] = (IPdfPrimitive) pdfArray;
    return (IPdfPrimitive) this.m_OptionalContent;
  }

  private IPdfPrimitive CreateOptionalContentViews(PdfPageLayer layer, bool isLoadedPage)
  {
    PdfLoadedPage page = this.m_page as PdfLoadedPage;
    PdfArray pdfArray = new PdfArray();
    this.m_OptionalContent["Name"] = (IPdfPrimitive) new PdfString("Layers");
    this.m_OptionalContent["Order"] = (IPdfPrimitive) page.Document.m_order;
    this.m_OptionalContent["ON"] = (IPdfPrimitive) page.Document.m_on;
    this.m_OptionalContent["OFF"] = (IPdfPrimitive) page.Document.m_off;
    PdfArray primitive = new PdfArray();
    primitive.Add((IPdfPrimitive) new PdfName("Print"));
    PdfDictionary pdfDictionary = new PdfDictionary();
    pdfDictionary.SetProperty("Category", (IPdfPrimitive) primitive);
    pdfDictionary.SetProperty("OCGs", (IPdfPrimitive) page.Document.m_printLayer);
    pdfDictionary.SetProperty("Event", (IPdfPrimitive) new PdfName("Print"));
    pdfArray.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary));
    this.m_OptionalContent["AS"] = (IPdfPrimitive) pdfArray;
    return (IPdfPrimitive) this.m_OptionalContent;
  }

  public void Remove(PdfPageLayer layer)
  {
    if (layer == null)
      throw new ArgumentNullException(nameof (layer));
    this.List.Remove((object) layer);
    this.RemoveLayer(layer);
  }

  public void Remove(string name)
  {
    for (int index = 0; index < this.List.Count; ++index)
    {
      PdfPageLayer layer = this.List[index] as PdfPageLayer;
      if (layer.Name == name)
      {
        this.RemoveLayer(layer);
        this.List.Remove((object) layer);
        break;
      }
    }
  }

  public void RemoveAt(int index)
  {
    if (index < 0 || index > this.List.Count - 1)
      throw new ArgumentOutOfRangeException(nameof (index), "Value can not be less 0 and greater List.Count - 1");
    PdfPageLayer layer = this[index];
    this.List.RemoveAt(index);
    if (layer == null)
      return;
    this.RemoveLayer(layer);
  }

  public bool Contains(PdfPageLayer layer)
  {
    return layer != null ? this.List.Contains((object) layer) : throw new ArgumentNullException(nameof (layer));
  }

  public int IndexOf(PdfPageLayer layer)
  {
    return layer != null ? this.List.IndexOf((object) layer) : throw new ArgumentNullException(nameof (layer));
  }

  public void Clear()
  {
    int index = 0;
    for (int count = this.List.Count; index < count; ++index)
    {
      this.RemoveLayer(this[index]);
      this.m_page = (PdfPageBase) null;
    }
    this.List.Clear();
  }

  internal void CombineContent(Stream stream)
  {
    PdfLoadedPage page = this.m_page as PdfLoadedPage;
    bool decompress = false;
    bool flag = false;
    if (page != null)
    {
      decompress = true;
      if (page.Document != null && page.Document is PdfLoadedDocument document)
        flag = document.m_redactionPages.Contains(page);
    }
    else if ((this.m_page as PdfPage).Imported)
      decompress = true;
    byte[] numArray = PdfString.StringToByte("\r\n");
    if (page != null && (this.isLayerPresent || page.Contents.Count != this.Count + 2 || flag))
      this.CombineProcess(page, decompress, stream, numArray);
    else if (page == null && this.m_page != null && decompress && this.Count == 0)
    {
      int index = 0;
      for (int count = this.m_page.Contents.Count; index < count; ++index)
      {
        if (PdfCrossTable.Dereference(this.m_page.Contents[index]) is PdfStream pdfStream)
        {
          pdfStream.Decompress();
          pdfStream.InternalStream.WriteTo(stream);
          stream.Write(numArray, 0, numArray.Length);
        }
      }
    }
    else
    {
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        PdfPageLayer pdfPageLayer = this[index];
        if (pdfPageLayer.m_graphicsState != null && decompress && this.isOptimizeContent)
        {
          pdfPageLayer.Graphics.Restore(pdfPageLayer.m_graphicsState);
          pdfPageLayer.m_graphicsState = (PdfGraphicsState) null;
        }
        PdfStream element = ((IPdfWrapper) pdfPageLayer).Element as PdfStream;
        if (decompress)
          element.Decompress();
        element.InternalStream.WriteTo(stream);
        stream.Write(numArray, 0, numArray.Length);
      }
    }
  }

  private void CombineProcess(PdfLoadedPage lPage, bool decompress, Stream stream, byte[] endl)
  {
    for (int index = 0; index < lPage.Contents.Count; ++index)
    {
      PdfStream pdfStream = (PdfStream) null;
      IPdfPrimitive content = lPage.Contents[index];
      if ((object) (content as PdfReferenceHolder) != null)
        pdfStream = (lPage.Contents[index] as PdfReferenceHolder).Object as PdfStream;
      else if (content is PdfStream)
        pdfStream = content as PdfStream;
      if (pdfStream != null)
      {
        if (decompress)
          pdfStream.Decompress();
        pdfStream.InternalStream.WriteTo(stream);
        stream.Write(endl, 0, endl.Length);
      }
    }
  }

  private void AddLayer(int index, PdfPageLayer layer)
  {
    if (layer == null)
      throw new ArgumentNullException(nameof (layer));
    this.m_page.Contents.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) layer));
  }

  private void RemoveLayer(PdfPageLayer layer)
  {
    if (layer == null)
      throw new ArgumentNullException("layer not to be null");
    PdfDictionary pdfDictionary1 = (PdfDictionary) null;
    if (this.m_page == null)
      return;
    this.RemoveLayerContent(layer);
    if (PdfCrossTable.Dereference(this.m_page.Dictionary["Resources"]) is PdfDictionary pdfDictionary2 && PdfCrossTable.Dereference(pdfDictionary2["Properties"]) is PdfDictionary pdfDictionary3 && layer.LayerId != null)
    {
      if (pdfDictionary3.ContainsKey(layer.LayerId.TrimStart('/')))
        pdfDictionary3.Remove(layer.LayerId.TrimStart('/'));
    }
    PdfPage page1 = this.m_page as PdfPage;
    PdfLoadedPage page2 = this.m_page as PdfLoadedPage;
    if (page1 != null)
    {
      this.RemoveLayerReference(page1, layer);
      PdfDocumentBase pdfDocumentBase = (PdfDocumentBase) page1.Document ?? page1.Section.ParentDocument;
      if (pdfDocumentBase != null && pdfDocumentBase.Catalog.ContainsKey("OCProperties"))
        pdfDictionary1 = PdfCrossTable.Dereference(pdfDocumentBase.Catalog["OCProperties"]) as PdfDictionary;
    }
    else if (page2 != null)
    {
      this.RemoveLayerReference(page2, layer, true);
      if (page2.Document != null && page2.Document.Catalog.ContainsKey("OCProperties"))
        pdfDictionary1 = PdfCrossTable.Dereference(page2.Document.Catalog["OCProperties"]) as PdfDictionary;
    }
    if (pdfDictionary1 == null)
      return;
    if (PdfCrossTable.Dereference(pdfDictionary1["OCGs"]) is PdfArray pdfArray1 && pdfArray1.Contains((IPdfPrimitive) layer.ReferenceHolder))
      pdfArray1.Remove((IPdfPrimitive) layer.ReferenceHolder);
    if (!(PdfCrossTable.Dereference(pdfDictionary1["D"]) is PdfDictionary pdfDictionary4))
      return;
    PdfArray pdfArray2 = PdfCrossTable.Dereference(pdfDictionary4["Order"]) as PdfArray;
    PdfArray pdfArray3 = PdfCrossTable.Dereference(pdfDictionary4["OFF"]) as PdfArray;
    PdfArray pdfArray4 = PdfCrossTable.Dereference(pdfDictionary4["ON"]) as PdfArray;
    if (PdfCrossTable.Dereference(pdfDictionary4["AS"]) is PdfArray pdfArray6 && pdfArray6.Count > 0)
    {
      for (int index = 0; index < pdfArray6.Count; ++index)
      {
        if (PdfCrossTable.Dereference(pdfArray6[index]) is PdfDictionary pdfDictionary5 && pdfDictionary5.ContainsKey("OCGs") && pdfDictionary5["OCGs"] is PdfArray pdfArray5 && pdfArray5.Contains((IPdfPrimitive) layer.ReferenceHolder))
          pdfArray5.Remove((IPdfPrimitive) layer.ReferenceHolder);
      }
    }
    if (pdfArray2 != null && pdfArray2.Contains((IPdfPrimitive) layer.ReferenceHolder))
      pdfArray2.Remove((IPdfPrimitive) layer.ReferenceHolder);
    if (layer.Visible && pdfArray4 != null && pdfArray4.Contains((IPdfPrimitive) layer.ReferenceHolder))
    {
      pdfArray4.Remove((IPdfPrimitive) layer.ReferenceHolder);
    }
    else
    {
      if (pdfArray3 == null || !pdfArray3.Contains((IPdfPrimitive) layer.ReferenceHolder))
        return;
      pdfArray3.Remove((IPdfPrimitive) layer.ReferenceHolder);
    }
  }

  private void RemoveLayerReference(PdfLoadedPage page, PdfPageLayer layer, bool isloaded)
  {
    if (page.Document != null && page.Document.primitive != null && layer.ReferenceHolder != (PdfReferenceHolder) null && page.Document.primitive.Contains((IPdfPrimitive) layer.ReferenceHolder))
    {
      page.Document.primitive.Remove((IPdfPrimitive) layer.ReferenceHolder);
      --page.Document.m_positon;
    }
    if (page.Document != null && page.Document.m_order != null && layer.ReferenceHolder != (PdfReferenceHolder) null && page.Document.m_order.Contains((IPdfPrimitive) layer.ReferenceHolder))
    {
      page.Document.m_order.Remove((IPdfPrimitive) layer.ReferenceHolder);
      --page.Document.m_orderposition;
    }
    if (layer.Visible && page.Document != null && page.Document.m_on != null && layer.ReferenceHolder != (PdfReferenceHolder) null && page.Document.m_on.Contains((IPdfPrimitive) layer.ReferenceHolder))
    {
      page.Document.m_on.Remove((IPdfPrimitive) layer.ReferenceHolder);
      --page.Document.m_onpositon;
    }
    else
    {
      if (page.Document == null || page.Document.m_off == null || !(layer.ReferenceHolder != (PdfReferenceHolder) null) || !page.Document.m_off.Contains((IPdfPrimitive) layer.ReferenceHolder))
        return;
      page.Document.m_off.Remove((IPdfPrimitive) layer.ReferenceHolder);
      --page.Document.m_offpositon;
    }
  }

  private void RemoveLayerReference(PdfPage page, PdfPageLayer layer)
  {
    PdfDocumentBase pdfDocumentBase = (PdfDocumentBase) page.Document ?? page.Section.ParentDocument;
    if (pdfDocumentBase == null)
      return;
    if (pdfDocumentBase.primitive != null && layer.ReferenceHolder != (PdfReferenceHolder) null && pdfDocumentBase.primitive.Contains((IPdfPrimitive) layer.ReferenceHolder))
    {
      pdfDocumentBase.primitive.Remove((IPdfPrimitive) layer.ReferenceHolder);
      --pdfDocumentBase.m_positon;
    }
    if (pdfDocumentBase.m_order != null && layer.ReferenceHolder != (PdfReferenceHolder) null && pdfDocumentBase.m_order.Contains((IPdfPrimitive) layer.ReferenceHolder))
    {
      pdfDocumentBase.m_order.Remove((IPdfPrimitive) layer.ReferenceHolder);
      --pdfDocumentBase.m_orderposition;
    }
    if (layer.Visible && pdfDocumentBase.m_on != null && layer.ReferenceHolder != (PdfReferenceHolder) null && pdfDocumentBase.m_on.Contains((IPdfPrimitive) layer.ReferenceHolder))
    {
      pdfDocumentBase.m_on.Remove((IPdfPrimitive) layer.ReferenceHolder);
      --pdfDocumentBase.m_onpositon;
    }
    else
    {
      if (pdfDocumentBase.m_off == null || !(layer.ReferenceHolder != (PdfReferenceHolder) null) || !pdfDocumentBase.m_off.Contains((IPdfPrimitive) layer.ReferenceHolder))
        return;
      pdfDocumentBase.m_off.Remove((IPdfPrimitive) layer.ReferenceHolder);
      --pdfDocumentBase.m_offpositon;
    }
  }

  private void InsertLayer(int index, PdfPageLayer layer)
  {
    PdfReferenceHolder element = layer != null ? new PdfReferenceHolder((IPdfWrapper) layer) : throw new ArgumentNullException(nameof (layer));
    if (layer.ReferenceHolder != (PdfReferenceHolder) null && layer.LayerId != null)
    {
      if (this.m_page.Contents.Contains((IPdfPrimitive) element))
      {
        this.m_page.Contents.RemoveAt(this.m_page.Contents.IndexOf((IPdfPrimitive) element));
        this.m_page.Contents.Insert(index, (IPdfPrimitive) element);
      }
    }
    else
      this.m_page.Contents.Insert(index, (IPdfPrimitive) element);
    if (this.m_page is PdfPage page1)
    {
      PdfDocumentBase pdfDocumentBase = (PdfDocumentBase) page1.Document ?? page1.Section.ParentDocument;
      if (pdfDocumentBase == null)
        return;
      if (pdfDocumentBase.primitive != null && layer.ReferenceHolder != (PdfReferenceHolder) null && pdfDocumentBase.primitive.Contains((IPdfPrimitive) layer.ReferenceHolder))
      {
        int index1 = pdfDocumentBase.primitive.IndexOf((IPdfPrimitive) layer.ReferenceHolder);
        pdfDocumentBase.primitive.RemoveAt(index1);
        pdfDocumentBase.primitive.Insert(index, (IPdfPrimitive) layer.ReferenceHolder);
      }
      if (pdfDocumentBase.m_order == null || !(layer.ReferenceHolder != (PdfReferenceHolder) null) || !pdfDocumentBase.m_order.Contains((IPdfPrimitive) layer.ReferenceHolder))
        return;
      int index2 = pdfDocumentBase.m_order.IndexOf((IPdfPrimitive) layer.ReferenceHolder);
      pdfDocumentBase.m_order.RemoveAt(index2);
      pdfDocumentBase.m_order.Insert(index, (IPdfPrimitive) layer.ReferenceHolder);
    }
    else
    {
      if (!(this.m_page is PdfLoadedPage page) || page.Document == null || !page.Document.Catalog.ContainsKey("OCProperties") || !(PdfCrossTable.Dereference(page.Document.Catalog["OCProperties"]) is PdfDictionary pdfDictionary1))
        return;
      if (PdfCrossTable.Dereference(pdfDictionary1["OCGs"]) is PdfArray pdfArray1 && pdfArray1.Contains((IPdfPrimitive) layer.ReferenceHolder))
      {
        pdfArray1.Remove((IPdfPrimitive) layer.ReferenceHolder);
        pdfArray1.Insert(index, (IPdfPrimitive) layer.ReferenceHolder);
      }
      if (!(PdfCrossTable.Dereference(pdfDictionary1["D"]) is PdfDictionary pdfDictionary2) || !(PdfCrossTable.Dereference(pdfDictionary2["Order"]) is PdfArray pdfArray2) || !pdfArray2.Contains((IPdfPrimitive) layer.ReferenceHolder))
        return;
      pdfArray2.Remove((IPdfPrimitive) layer.ReferenceHolder);
      pdfArray2.Insert(index, (IPdfPrimitive) layer.ReferenceHolder);
    }
  }

  private void ParseLayers(PdfPageBase loadedPage)
  {
    if (loadedPage == null)
      throw new ArgumentNullException(nameof (loadedPage));
    if (this.m_page == null)
      return;
    PdfArray contents = this.m_page.Contents;
    PdfDictionary resource = (PdfDictionary) null;
    lock (PdfPageLayerCollection.m_resourceLock)
      resource = (PdfDictionary) this.m_page.GetResources();
    PdfDictionary pdfDictionary1 = (PdfDictionary) null;
    PdfDictionary propertie = (PdfDictionary) null;
    Dictionary<PdfReferenceHolder, PdfPageLayer> dictionary = new Dictionary<PdfReferenceHolder, PdfPageLayer>();
    if (loadedPage is PdfLoadedPage pdfLoadedPage)
    {
      propertie = PdfCrossTable.Dereference(resource["Properties"]) as PdfDictionary;
      if (pdfLoadedPage.Document != null)
      {
        pdfDictionary1 = PdfCrossTable.Dereference(pdfLoadedPage.Document.Catalog["OCProperties"]) as PdfDictionary;
        this.documentLayers = pdfLoadedPage.Document.documentLayerCollection;
      }
    }
    if (pdfDictionary1 != null && pdfDictionary1.ContainsKey("OCGs") && this.documentLayers == null)
    {
      this.documentLayers = new Dictionary<PdfReferenceHolder, PdfDictionary>();
      if (PdfCrossTable.Dereference(pdfDictionary1["OCGs"]) is PdfArray pdfArray && pdfArray.Count > 0)
      {
        for (int index = 0; index < pdfArray.Count; ++index)
        {
          PdfReferenceHolder key = pdfArray[index] as PdfReferenceHolder;
          if (key != (PdfReferenceHolder) null && key.Object is PdfDictionary pdfDictionary2 && !this.documentLayers.ContainsKey(key))
            this.documentLayers[key] = pdfDictionary2;
        }
        if (pdfLoadedPage != null && pdfLoadedPage.Document != null)
          pdfLoadedPage.Document.documentLayerCollection = this.documentLayers;
      }
    }
    bool isPropertieLayer = false;
    bool isResourceLayer = false;
    this.PageContainsLayer(propertie, resource, out isPropertieLayer, out isResourceLayer);
    if (pdfDictionary1 != null && (isPropertieLayer || isResourceLayer))
    {
      pdfDictionary5 = (PdfDictionary) null;
      PdfReferenceHolder pdfReferenceHolder = (PdfReferenceHolder) null;
      if (propertie != null && propertie.Items != null && isPropertieLayer)
      {
        foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in propertie.Items)
        {
          string key = keyValuePair.Key.Value;
          pdfReferenceHolder = keyValuePair.Value as PdfReferenceHolder;
          if (PdfCrossTable.Dereference(keyValuePair.Value) is PdfDictionary pdfDictionary5 && pdfReferenceHolder != (PdfReferenceHolder) null && this.documentLayers != null && !this.m_layerCollection.Contains(key) && (this.documentLayers.ContainsKey(pdfReferenceHolder) || pdfDictionary5.ContainsKey("OCGs")))
          {
            if (pdfDictionary5.ContainsKey("OCGs"))
            {
              if (!(PdfCrossTable.Dereference(pdfDictionary5["OCGs"]) is PdfArray pdfArray))
              {
                if (PdfCrossTable.Dereference(pdfDictionary5["OCGs"]) is PdfDictionary pdfDictionary5)
                {
                  pdfReferenceHolder = pdfDictionary5["OCGs"] as PdfReferenceHolder;
                  if (pdfReferenceHolder != (PdfReferenceHolder) null && this.documentLayers.ContainsKey(pdfReferenceHolder))
                    this.AddLayer(loadedPage, pdfDictionary5, pdfReferenceHolder, key, dictionary, false);
                }
              }
              else
              {
                for (int index = 0; index < pdfArray.Count; ++index)
                {
                  if ((object) (pdfArray[index] as PdfReferenceHolder) != null)
                  {
                    pdfReferenceHolder = pdfArray[index] as PdfReferenceHolder;
                    if (pdfReferenceHolder != (PdfReferenceHolder) null && (pdfArray[index] as PdfReferenceHolder).Object is PdfDictionary pdfDictionary5 && this.documentLayers.ContainsKey(pdfReferenceHolder))
                      this.AddLayer(loadedPage, pdfDictionary5, pdfReferenceHolder, key, dictionary, false);
                  }
                }
              }
            }
            else
              this.AddLayer(loadedPage, pdfDictionary5, pdfReferenceHolder, key, dictionary, false);
          }
        }
      }
      if (isResourceLayer)
        this.ParseResourceLayer(resource, pdfDictionary5, pdfReferenceHolder, loadedPage, dictionary, this.documentLayers);
    }
    PdfStream pdfStream1 = new PdfStream();
    PdfStream pdfStream2 = new PdfStream();
    byte num1 = 113;
    byte num2 = 10;
    byte num3 = 81;
    if (pdfDictionary1 != null && dictionary.Count > 0)
    {
      this.CheckVisible(pdfDictionary1, dictionary);
      this.SortLayerList(pdfDictionary1, dictionary);
      this.isLayerPresent = true;
    }
    if (loadedPage.Imported && contents.Count > 0)
    {
      for (int index = 0; index < contents.Count; ++index)
      {
        IPdfPrimitive pdfPrimitive = contents[index];
        if (pdfPrimitive != null && PdfCrossTable.Dereference(pdfPrimitive) is PdfStream pdfStream3)
          pdfStream3.Decompress();
      }
    }
    byte num4 = 32 /*0x20*/;
    byte[] numArray = new byte[4]{ num4, num1, num4, num2 };
    pdfStream1.Data = numArray;
    contents.Insert(0, (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfStream1));
    numArray[0] = num4;
    numArray[1] = num3;
    numArray[2] = num4;
    numArray[3] = num2;
    pdfStream2.Data = numArray;
    contents.Insert(contents.Count, (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfStream2));
  }

  private void CheckVisible(
    PdfDictionary ocproperties,
    Dictionary<PdfReferenceHolder, PdfPageLayer> m_layerDictionary)
  {
    if (ocproperties == null || !(PdfCrossTable.Dereference(ocproperties["D"]) is PdfDictionary pdfDictionary) || !(PdfCrossTable.Dereference(pdfDictionary["OFF"]) is PdfArray pdfArray) || m_layerDictionary.Count <= 0)
      return;
    for (int index = 0; index < pdfArray.Count; ++index)
    {
      if (m_layerDictionary.ContainsKey(pdfArray[index] as PdfReferenceHolder))
      {
        PdfPageLayer mLayer = m_layerDictionary[pdfArray[index] as PdfReferenceHolder];
        if (mLayer != null)
        {
          mLayer.m_visible = false;
          if (mLayer.Dictionary != null && mLayer.Dictionary.ContainsKey("Visible"))
            mLayer.Dictionary.SetProperty("Visible", (IPdfPrimitive) new PdfBoolean(false));
        }
      }
    }
  }

  private void SortLayerList(
    PdfDictionary ocPropertie,
    Dictionary<PdfReferenceHolder, PdfPageLayer> layerCollection)
  {
    if (ocPropertie == null || !(PdfCrossTable.Dereference(ocPropertie["OCGs"]) is PdfArray pdfArray))
      return;
    int count = this.List.Count;
    this.List.Clear();
    for (int index = 0; index < pdfArray.Count; ++index)
    {
      if (count > this.List.Count && layerCollection.ContainsKey(pdfArray[index] as PdfReferenceHolder))
        this.List.Add((object) layerCollection[pdfArray[index] as PdfReferenceHolder]);
    }
  }

  private void PageContainsLayer(
    PdfDictionary propertie,
    PdfDictionary resource,
    out bool isPropertieLayer,
    out bool isResourceLayer)
  {
    isPropertieLayer = false;
    isResourceLayer = false;
    if (propertie != null)
      isPropertieLayer = true;
    if (resource == null || !resource.ContainsKey("XObject") || !(PdfCrossTable.Dereference(resource["XObject"]) is PdfDictionary pdfDictionary1))
      return;
    foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in pdfDictionary1.Items)
    {
      if ((object) (keyValuePair.Value as PdfReferenceHolder) != null && PdfCrossTable.Dereference(keyValuePair.Value) is PdfDictionary pdfDictionary2 && pdfDictionary2.ContainsKey("OC"))
      {
        isResourceLayer = true;
        break;
      }
    }
  }

  private void AddLayer(
    PdfPageBase page,
    PdfDictionary dictionary,
    PdfReferenceHolder reference,
    string key,
    Dictionary<PdfReferenceHolder, PdfPageLayer> pageLayerCollection,
    bool isResourceLayer)
  {
    PdfPageLayer pdfPageLayer = new PdfPageLayer(page);
    pdfPageLayer.isResourceLayer = isResourceLayer;
    if (PdfCrossTable.Dereference(dictionary["Usage"]) is PdfDictionary pdfDictionary)
    {
      PdfDictionary pdfDictionary1 = PdfCrossTable.Dereference(pdfDictionary["Print"]) as PdfDictionary;
      PdfDictionary pdfDictionary2 = PdfCrossTable.Dereference(pdfDictionary["View"]) as PdfDictionary;
      if (pdfDictionary1 != null)
      {
        pdfPageLayer.m_printOption = pdfDictionary1;
        foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in pdfDictionary1.Items)
        {
          if (keyValuePair.Key.Value.Equals("PrintState"))
          {
            PdfName pdfName = keyValuePair.Value as PdfName;
            pdfPageLayer.PrintState = !(pdfName != (PdfName) null) || !pdfName.Value.Equals("ON") ? PdfPrintState.NeverPrint : PdfPrintState.AlwaysPrint;
            break;
          }
        }
      }
      if (pdfDictionary2 != null)
      {
        foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in pdfDictionary2.Items)
        {
          if (keyValuePair.Key.Value.Equals("ViewState"))
          {
            PdfName pdfName = keyValuePair.Value as PdfName;
            if (pdfName != (PdfName) null && pdfName.Value.Equals("OFF"))
            {
              pdfPageLayer.m_visible = false;
              break;
            }
          }
        }
      }
    }
    this.List.Add((object) pdfPageLayer);
    if (!pageLayerCollection.ContainsKey(reference))
      pageLayerCollection.Add(reference, pdfPageLayer);
    pdfPageLayer.Dictionary = dictionary;
    pdfPageLayer.ReferenceHolder = reference;
    this.m_layerCollection.Add(key.TrimStart('/'));
    pdfPageLayer.LayerId = key.TrimStart('/');
    if (!dictionary.ContainsKey("Name") || !(PdfCrossTable.Dereference(dictionary["Name"]) is PdfString pdfString))
      return;
    pdfPageLayer.Name = pdfString.Value;
  }

  private void ParseResourceLayer(
    PdfDictionary resource,
    PdfDictionary layerDictionary,
    PdfReferenceHolder layerReference,
    PdfPageBase loadedPage,
    Dictionary<PdfReferenceHolder, PdfPageLayer> pageLayerCollection,
    Dictionary<PdfReferenceHolder, PdfDictionary> documentLayers)
  {
    if (!resource.ContainsKey("XObject") || !(PdfCrossTable.Dereference(resource["XObject"]) is PdfDictionary pdfDictionary1))
      return;
    foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in pdfDictionary1.Items)
    {
      if ((object) (keyValuePair.Value as PdfReferenceHolder) != null)
      {
        string key = keyValuePair.Key.Value;
        if ((keyValuePair.Value as PdfReferenceHolder).Object is PdfDictionary pdfDictionary3 && pdfDictionary3.ContainsKey("OC"))
        {
          layerDictionary = PdfCrossTable.Dereference(pdfDictionary3["OC"]) as PdfDictionary;
          if (layerDictionary != null && documentLayers != null && (layerDictionary.ContainsKey("Name") || layerDictionary.ContainsKey("OCGs")))
          {
            if (!(PdfCrossTable.Dereference(layerDictionary["OCGs"]) is PdfArray pdfArray))
            {
              if (PdfCrossTable.Dereference(layerDictionary["OCGs"]) is PdfDictionary pdfDictionary2)
              {
                layerReference = layerDictionary["OCGs"] as PdfReferenceHolder;
                layerDictionary = pdfDictionary2;
                if (layerReference != (PdfReferenceHolder) null && documentLayers.ContainsKey(layerReference))
                  this.AddLayer(loadedPage, layerDictionary, layerReference, key, pageLayerCollection, true);
              }
              else
              {
                layerDictionary = PdfCrossTable.Dereference(pdfDictionary3["OC"]) as PdfDictionary;
                layerReference = pdfDictionary3["OC"] as PdfReferenceHolder;
                if (layerReference != (PdfReferenceHolder) null && documentLayers.ContainsKey(layerReference))
                  this.AddLayer(loadedPage, layerDictionary, layerReference, key, pageLayerCollection, true);
              }
            }
            else
            {
              for (int index = 0; index < pdfArray.Count; ++index)
              {
                layerDictionary = PdfCrossTable.Dereference(pdfArray[index]) as PdfDictionary;
                layerReference = pdfArray[index] as PdfReferenceHolder;
                if (layerReference != (PdfReferenceHolder) null && documentLayers.ContainsKey(layerReference))
                  this.AddLayer(loadedPage, layerDictionary, layerReference, key, pageLayerCollection, true);
              }
            }
          }
        }
      }
    }
  }

  private void RemoveLayerContent(PdfPageLayer layer)
  {
    bool isSkip = false;
    PdfArray pdfArray1 = new PdfArray();
    PdfArray pdfArray2 = new PdfArray();
    System.Collections.Generic.List<PdfStream> pdfStreamList = new System.Collections.Generic.List<PdfStream>();
    for (int index = 0; index < this.m_page.Contents.Count; ++index)
    {
      PdfReferenceHolder content = this.m_page.Contents[index] as PdfReferenceHolder;
      if (content != (PdfReferenceHolder) null && content.Reference == (PdfReference) null)
        pdfArray1.Add(this.m_page.Contents[index]);
      else
        pdfArray2.Add(this.m_page.Contents[index]);
    }
    ContentParser parser1 = (ContentParser) null;
    MemoryStream memoryStream = new MemoryStream();
    byte[] buffer = PdfString.StringToByte("\r\n");
    for (int index = 0; index < pdfArray2.Count; ++index)
    {
      PdfStream pdfStream = PdfCrossTable.Dereference(pdfArray2[index]) as PdfStream;
      pdfStream.Decompress();
      pdfStream.InternalStream.WriteTo((Stream) memoryStream);
      memoryStream.Write(buffer, 0, buffer.Length);
      parser1 = new ContentParser(memoryStream.ToArray());
    }
    PdfStream data1 = new PdfStream();
    if (parser1 != null)
    {
      PdfStream layersContent = this.FindLayersContent(layer, parser1, data1, isSkip);
      pdfStreamList.Add(layersContent);
    }
    PdfStream pdfStream1 = (PdfStream) null;
    if (layer.m_graphics != null && layer.m_graphics.StreamWriter != null)
      pdfStream1 = layer.m_graphics.StreamWriter.GetStream();
    for (int index = 0; index < pdfArray1.Count; ++index)
    {
      memoryStream = new MemoryStream();
      PdfStream pdfStream2 = PdfCrossTable.Dereference(pdfArray1[index]) as PdfStream;
      if (pdfStream2 != pdfStream1)
      {
        if (this.m_page is PdfLoadedPage)
          pdfStream2.Decompress();
        pdfStream2.InternalStream.WriteTo((Stream) memoryStream);
        ContentParser parser2 = new ContentParser(memoryStream.ToArray());
        PdfStream data2 = new PdfStream();
        PdfStream layersContent = this.FindLayersContent(layer, parser2, data2, isSkip);
        if (pdfStream2.Data.Length > 0 && layersContent.Data.Length > 0)
        {
          pdfStream2.Clear();
          pdfStream2.Write(layersContent.Data);
        }
        pdfStreamList.Add(pdfStream2);
      }
      memoryStream.Dispose();
    }
    this.m_page.Contents.Clear();
    for (int index = 0; index < pdfStreamList.Count; ++index)
      this.m_page.Contents.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfStreamList[index]));
    pdfStreamList.Clear();
    memoryStream?.Dispose();
  }

  private string FindOperator(int token)
  {
    return new string[79]
    {
      "b",
      "B",
      "bx",
      "Bx",
      "BDC",
      "BI",
      "BMC",
      "BT",
      "BX",
      "c",
      "cm",
      "CS",
      "cs",
      "d",
      "d0",
      "d1",
      "Do",
      "DP",
      "EI",
      "EMC",
      "ET",
      "EX",
      "f",
      "F",
      "fx",
      "G",
      "g",
      "gs",
      "h",
      "i",
      "ID",
      "j",
      "J",
      "K",
      "k",
      "l",
      "m",
      "M",
      "MP",
      "n",
      "q",
      "Q",
      "re",
      "RG",
      "rg",
      "ri",
      "s",
      "S",
      "SC",
      "sc",
      "SCN",
      "scn",
      "sh",
      "f*",
      "Tx",
      "Tc",
      "Td",
      "TD",
      "Tf",
      "Tj",
      "TJ",
      "TL",
      "Tm",
      "Tr",
      "Ts",
      "Tw",
      "Tz",
      "v",
      "w",
      "W",
      "W*",
      "Wx",
      "y",
      "T*",
      "b*",
      "B*",
      "'",
      "\"",
      "true"
    }.GetValue(token) as string;
  }

  private void StreamWrite(string[] operands, string mOperator, bool skip, PdfStream data)
  {
    if (skip && this.IsSkip)
      return;
    if (operands != null)
    {
      foreach (string operand in operands)
      {
        PdfString pdfString = new PdfString(operand);
        data.Write(pdfString.Bytes);
        data.Write(" ");
      }
    }
    PdfString pdfString1 = new PdfString(mOperator);
    data.Write(pdfString1.Bytes);
    data.Write("\r\n");
  }

  private void ProcessBeginMarkContent(
    PdfPageLayer parser,
    string mOperator,
    string[] operands,
    PdfStream data)
  {
    if ("BDC".Equals(mOperator.ToString()))
    {
      string str = (string) null;
      if (operands.Length > 1)
      {
        if (operands[0].TrimStart('/').Equals("OC"))
          str = operands[1].ToString().TrimStart('/');
      }
      if (this.m_bdcCount > 0)
      {
        ++this.m_bdcCount;
        return;
      }
      if (str != null && str.Equals(parser.LayerId))
        ++this.m_bdcCount;
    }
    this.StreamWrite(operands, mOperator, true, data);
    if (!"EMC".Equals(mOperator.ToString()) || this.m_bdcCount <= 0)
      return;
    --this.m_bdcCount;
  }

  private PdfStream FindLayersContent(
    PdfPageLayer layer,
    ContentParser parser,
    PdfStream data,
    bool isSkip)
  {
    PdfRecordCollection recordCollection = parser.ReadContent();
    for (int index = 0; index < recordCollection.RecordCollection.Count; ++index)
    {
      string operatorName = recordCollection.RecordCollection[index].OperatorName;
      if (operatorName == "BMC" || operatorName == "EMC" || operatorName == "BDC")
      {
        this.ProcessBeginMarkContent(layer, operatorName, recordCollection.RecordCollection[index].Operands, data);
        isSkip = true;
      }
      if (operatorName == "Do")
      {
        if (recordCollection.RecordCollection[index].Operands[0].TrimStart('/').Equals(layer.LayerId))
          isSkip = true;
      }
      if (operatorName == "q" || operatorName == "Q" || operatorName == "w" || operatorName == "J" || operatorName == "j" || operatorName == "M" || operatorName == "d" || operatorName == "ri" || operatorName == "i" || operatorName == "gs" || operatorName == "g" || operatorName == "cm" || operatorName == "G" || operatorName == "rg" || operatorName == "RG" || operatorName == "k" || operatorName == "K" || operatorName == "cs" || operatorName == "CS" || operatorName == "scn" || operatorName == "SCN" || operatorName == "sc" || operatorName == "SC")
      {
        if (!isSkip)
          this.StreamWrite(recordCollection.RecordCollection[index].Operands, operatorName, false, data);
      }
      else if (!isSkip)
        this.StreamWrite(recordCollection.RecordCollection[index].Operands, operatorName, true, data);
      isSkip = false;
    }
    data.Compress = true;
    return data;
  }
}
