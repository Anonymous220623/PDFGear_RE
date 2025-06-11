// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfDocumentLayerCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf;

public class PdfDocumentLayerCollection : PdfCollection
{
  internal bool m_sublayer;
  internal PdfDictionary m_OptionalContent = new PdfDictionary();
  private bool m_isLayerContainsResource;
  private static object s_syncLockLayer = new object();
  private PdfDocumentBase document;
  private PdfLayer m_parent;
  private Dictionary<PdfReferenceHolder, PdfLayer> layerDictionary = new Dictionary<PdfReferenceHolder, PdfLayer>();
  private int m_bdcCount;

  internal PdfDocumentLayerCollection()
  {
  }

  public PdfLayer this[int index]
  {
    get => this.List[index] as PdfLayer;
    set
    {
      if (value == null)
        throw new ArgumentNullException("layer");
      PdfLayer layer = this[index];
      if (layer != null)
        this.RemoveLayer(layer, true);
      this.List[index] = (object) value;
      this.InsertLayer(index, value);
    }
  }

  private bool IsSkip => this.m_bdcCount > 0;

  internal PdfDocumentLayerCollection(PdfDocumentBase document, PdfLayer layer)
  {
    this.document = document;
    this.m_parent = layer;
  }

  internal PdfDocumentLayerCollection(PdfDocumentBase document)
  {
    this.document = document != null ? document : throw new ArgumentNullException("document not to be null");
    if (!(document is PdfLoadedDocument))
      return;
    PdfLoadedDocument pdfLoadedDocument = document as PdfLoadedDocument;
    if (!pdfLoadedDocument.Catalog.ContainsKey("OCProperties") || !(PdfCrossTable.Dereference(pdfLoadedDocument.Catalog["OCProperties"]) is PdfDictionary ocProperties))
      return;
    if (ocProperties.ContainsKey("OCGs") && PdfCrossTable.Dereference(ocProperties["OCGs"]) is PdfArray)
    {
      PdfArray pdfArray = PdfCrossTable.Dereference(ocProperties["OCGs"]) as PdfArray;
      for (int index = 0; index < pdfArray.Count; ++index)
      {
        if ((object) (pdfArray[index] as PdfReferenceHolder) != null)
        {
          PdfReferenceHolder key = pdfArray[index] as PdfReferenceHolder;
          PdfDictionary pdfDictionary1 = key.Object as PdfDictionary;
          PdfLayer pdfLayer = new PdfLayer();
          if (pdfDictionary1 != null && pdfDictionary1.ContainsKey("Name"))
          {
            PdfString pdfString = PdfCrossTable.Dereference(pdfDictionary1["Name"]) as PdfString;
            pdfLayer.Name = pdfString.Value;
            pdfLayer.Dictionary = pdfDictionary1;
            pdfLayer.ReferenceHolder = key;
            IPdfPrimitive pdfPrimitive = PdfCrossTable.Dereference(pdfDictionary1["LayerID"]);
            if (pdfPrimitive != null)
              pdfLayer.LayerId = pdfPrimitive.ToString();
            if (PdfCrossTable.Dereference(pdfDictionary1["Usage"]) is PdfDictionary pdfDictionary4)
            {
              PdfDictionary pdfDictionary2 = PdfCrossTable.Dereference(pdfDictionary4["Print"]) as PdfDictionary;
              PdfDictionary pdfDictionary3 = PdfCrossTable.Dereference(pdfDictionary4["View"]) as PdfDictionary;
              if (pdfDictionary2 != null)
              {
                pdfLayer.m_printOption = pdfDictionary2;
                foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in pdfDictionary2.Items)
                {
                  if (keyValuePair.Key.Value.Equals("PrintState"))
                  {
                    pdfLayer.PrintState = !(PdfCrossTable.Dereference(keyValuePair.Value) as PdfName).Value.Equals("ON") ? PdfPrintState.NeverPrint : PdfPrintState.AlwaysPrint;
                    break;
                  }
                }
              }
              if (pdfDictionary3 != null)
              {
                foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in pdfDictionary3.Items)
                {
                  if (keyValuePair.Key.Value.Equals("ViewState") && (PdfCrossTable.Dereference(keyValuePair.Value) as PdfName).Value.Equals("OFF"))
                  {
                    pdfLayer.m_visible = false;
                    break;
                  }
                }
              }
            }
          }
          pdfLayer.Document = document;
          pdfLayer.Layer = pdfLayer;
          this.layerDictionary[key] = pdfLayer;
          this.List.Add((object) pdfLayer);
        }
      }
    }
    this.CheckLayerLock(ocProperties);
    this.CheckLayerVisible(ocProperties);
    this.CheckParentLayer(ocProperties);
    this.CreateLayerHierarchical(ocProperties);
  }

  public PdfLayer Add(string name, bool visible)
  {
    lock (PdfDocumentLayerCollection.s_syncLockLayer)
    {
      PdfLayer layer = new PdfLayer()
      {
        Name = name,
        Document = this.document,
        Visible = visible,
        LayerId = "OCG_" + Guid.NewGuid().ToString(),
        m_sublayerposition = 0
      };
      layer.Layer = layer;
      this.Add(layer);
      return layer;
    }
  }

  public PdfLayer Add(string name)
  {
    lock (PdfDocumentLayerCollection.s_syncLockLayer)
    {
      PdfLayer layer = new PdfLayer()
      {
        Document = this.document,
        Name = name,
        LayerId = "OCG_" + Guid.NewGuid().ToString(),
        m_sublayerposition = 0
      };
      layer.Layer = layer;
      this.Add(layer);
      return layer;
    }
  }

  private int Add(PdfLayer layer)
  {
    lock (PdfDocumentLayerCollection.s_syncLockLayer)
    {
      if (layer == null)
        throw new ArgumentNullException(nameof (layer));
      this.List.Add((object) layer);
      int num = this.List.Count - 1;
      if (this.document is PdfDocument)
        this.CreateLayer(layer);
      else
        this.CreateLayerLoadedDocument(layer);
      layer.Layer = layer;
      return num;
    }
  }

  private int AddNestedLayer(PdfLayer layer)
  {
    lock (PdfDocumentLayerCollection.s_syncLockLayer)
    {
      if (layer == null)
        throw new ArgumentNullException(nameof (layer));
      this.List.Add((object) layer);
      int num = this.List.Count - 1;
      layer.Layer = layer;
      return num;
    }
  }

  internal void CreateLayer(PdfLayer layer)
  {
    PdfDictionary primitive = new PdfDictionary();
    IPdfPrimitive contentDictionary = this.CreateOptionalContentDictionary(layer);
    primitive["OCGs"] = contentDictionary;
    primitive["D"] = this.CreateOptionalContentViews(layer);
    this.document.Catalog.SetProperty("OCProperties", (IPdfPrimitive) primitive);
  }

  private PdfDictionary setPrintOption(PdfLayer layer)
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

  internal void CreateLayerLoadedDocument(PdfLayer layer)
  {
    PdfDictionary primitive = new PdfDictionary();
    IPdfPrimitive contentDictionary = this.CreateOptionalContentDictionary(layer, true);
    bool flag = false;
    if (this.document != null && this.document.Catalog != null && this.document.Catalog.ContainsKey("OCProperties") && this.m_isLayerContainsResource && PdfCrossTable.Dereference(this.document.Catalog["OCProperties"]) is PdfDictionary pdfDictionary2 && pdfDictionary2.ContainsKey("OCGs"))
    {
      PdfArray pdfArray1 = PdfCrossTable.Dereference(pdfDictionary2["OCGs"]) as PdfArray;
      PdfArray pdfArray2 = contentDictionary as PdfArray;
      if (pdfArray1 != null && pdfArray2 != null)
      {
        flag = true;
        foreach (IPdfPrimitive element in pdfArray2)
        {
          if (!pdfArray1.Contains(element))
            pdfArray1.Add(element);
        }
      }
      if (pdfDictionary2.ContainsKey("D") && PdfCrossTable.Dereference(pdfDictionary2["D"]) is PdfDictionary pdfDictionary1)
      {
        PdfArray pdfArray3 = (PdfArray) null;
        PdfArray pdfArray4 = (PdfArray) null;
        PdfArray pdfArray5 = (PdfArray) null;
        if (!pdfDictionary1.ContainsKey("Order"))
          pdfDictionary1["Order"] = (IPdfPrimitive) this.document.m_order;
        if (pdfDictionary1.ContainsKey("OFF"))
          pdfArray4 = PdfCrossTable.Dereference(pdfDictionary1["OFF"]) as PdfArray;
        if (pdfDictionary1.ContainsKey("ON"))
          pdfArray3 = PdfCrossTable.Dereference(pdfDictionary1["ON"]) as PdfArray;
        if (pdfDictionary1.ContainsKey("AS"))
          pdfArray5 = PdfCrossTable.Dereference(pdfDictionary1["AS"]) as PdfArray;
        if (pdfArray5 != null)
        {
          for (int index = 0; index < pdfArray5.Count; ++index)
          {
            if ((object) (pdfArray5[index] as PdfReferenceHolder) != null || pdfArray5[index] is PdfDictionary)
            {
              PdfDictionary pdfDictionary = (object) (pdfArray5[index] as PdfReferenceHolder) == null ? pdfArray5[index] as PdfDictionary : (pdfArray5[index] as PdfReferenceHolder).Object as PdfDictionary;
              if (pdfDictionary != null && pdfDictionary["OCGs"] is PdfArray pdfArray6 && pdfArray2 != null && !pdfArray6.Contains((IPdfPrimitive) layer.ReferenceHolder))
                pdfArray6.Add((IPdfPrimitive) layer.ReferenceHolder);
            }
          }
        }
        if (layer.Visible)
        {
          if (pdfArray3 != null && pdfArray2 != null && !pdfArray3.Contains((IPdfPrimitive) layer.ReferenceHolder))
            pdfArray3.Add((IPdfPrimitive) layer.ReferenceHolder);
        }
        else if (pdfArray4 != null && pdfArray2 != null && !pdfArray4.Contains((IPdfPrimitive) layer.ReferenceHolder))
          pdfArray4.Add((IPdfPrimitive) layer.ReferenceHolder);
      }
    }
    if (flag)
      return;
    primitive["OCGs"] = contentDictionary;
    primitive["D"] = this.CreateOptionalContentViews(layer, true);
    this.document.Catalog.SetProperty("OCProperties", (IPdfPrimitive) primitive);
  }

  public void Move(int index, PdfLayer layer)
  {
    if (index < 0)
      throw new ArgumentOutOfRangeException(nameof (index), "Value can not be less 0");
    if (layer == null)
      throw new ArgumentNullException(nameof (layer));
    for (int index1 = 0; index1 < this.List.Count; ++index1)
    {
      if (this.List[index1].Equals((object) layer))
        this.List.RemoveAt(this.IndexOf(this.List[index1] as PdfLayer));
    }
    this.List.Insert(index, (object) layer);
    this.InsertLayer(index, layer);
  }

  private IPdfPrimitive CreateOptionalContentDictionary(PdfLayer layer)
  {
    PdfDictionary pdfDictionary = new PdfDictionary();
    pdfDictionary["Name"] = (IPdfPrimitive) new PdfString(layer.Name);
    pdfDictionary["Type"] = (IPdfPrimitive) new PdfName("OCG");
    pdfDictionary["LayerID"] = (IPdfPrimitive) new PdfName(layer.LayerId);
    pdfDictionary["Visible"] = (IPdfPrimitive) new PdfBoolean(layer.Visible);
    if (layer.PrintState.Equals((object) PdfPrintState.AlwaysPrint) || layer.PrintState.Equals((object) PdfPrintState.NeverPrint) || layer.PrintState.Equals((object) PdfPrintState.PrintWhenVisible))
    {
      layer.m_usage = this.setPrintOption(layer);
      pdfDictionary["Usage"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) layer.m_usage);
      this.document.m_printLayer.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary));
    }
    PdfReferenceHolder pdfReferenceHolder = new PdfReferenceHolder((IPdfPrimitive) pdfDictionary);
    this.document.primitive.Add((IPdfPrimitive) pdfReferenceHolder);
    layer.ReferenceHolder = pdfReferenceHolder;
    layer.Dictionary = pdfDictionary;
    this.CreateSublayer(PdfCrossTable.Dereference(this.document.Catalog["OCProperties"]) as PdfDictionary, pdfReferenceHolder, layer);
    if (layer.Visible)
      this.document.m_on.Add((IPdfPrimitive) pdfReferenceHolder);
    else
      this.document.m_off.Add((IPdfPrimitive) pdfReferenceHolder);
    return (IPdfPrimitive) this.document.primitive;
  }

  private IPdfPrimitive CreateOptionalContentDictionary(PdfLayer layer, bool isLoadedDocument)
  {
    PdfDictionary pdfDictionary = new PdfDictionary();
    pdfDictionary["Name"] = (IPdfPrimitive) new PdfString(layer.Name);
    pdfDictionary["Type"] = (IPdfPrimitive) new PdfName("OCG");
    pdfDictionary["LayerID"] = (IPdfPrimitive) new PdfName(layer.LayerId);
    pdfDictionary["Visible"] = (IPdfPrimitive) new PdfBoolean(layer.Visible);
    if (layer.PrintState.Equals((object) PdfPrintState.AlwaysPrint) || layer.PrintState.Equals((object) PdfPrintState.NeverPrint) || layer.PrintState.Equals((object) PdfPrintState.PrintWhenVisible))
    {
      layer.m_usage = this.setPrintOption(layer);
      pdfDictionary["Usage"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) layer.m_usage);
      this.document.m_printLayer.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary));
    }
    PdfReferenceHolder pdfReferenceHolder = new PdfReferenceHolder((IPdfPrimitive) pdfDictionary);
    this.document.primitive.Add((IPdfPrimitive) pdfReferenceHolder);
    layer.Dictionary = pdfDictionary;
    layer.ReferenceHolder = pdfReferenceHolder;
    this.CreateSublayer(PdfCrossTable.Dereference(this.document.Catalog["OCProperties"]) as PdfDictionary, pdfReferenceHolder, layer);
    if (layer.Visible)
      this.document.m_on.Add((IPdfPrimitive) pdfReferenceHolder);
    else
      this.document.m_off.Add((IPdfPrimitive) pdfReferenceHolder);
    this.m_isLayerContainsResource = true;
    return (IPdfPrimitive) this.document.primitive;
  }

  private void CreateSublayer(
    PdfDictionary ocProperties,
    PdfReferenceHolder reference,
    PdfLayer layer)
  {
    if (!this.m_sublayer)
    {
      if (ocProperties != null)
      {
        PdfArray pdfArray = (PdfArray) null;
        if (PdfCrossTable.Dereference(ocProperties["D"]) is PdfDictionary pdfDictionary)
          pdfArray = PdfCrossTable.Dereference(pdfDictionary["Order"]) as PdfArray;
        if (this.document.m_order != null && pdfArray != null)
          this.document.m_order = pdfArray;
        this.document.m_order.Add((IPdfPrimitive) reference);
      }
      else
        this.document.m_order.Add((IPdfPrimitive) reference);
    }
    else
    {
      layer.parent = this.m_parent;
      if (ocProperties != null)
      {
        PdfArray pdfArray = (PdfArray) null;
        if (PdfCrossTable.Dereference(ocProperties["D"]) is PdfDictionary pdfDictionary)
          pdfArray = PdfCrossTable.Dereference(pdfDictionary["Order"]) as PdfArray;
        if (this.document.m_order != null && pdfArray != null)
          this.document.m_order = pdfArray;
      }
      if (this.m_parent.m_child.Count == 0)
        this.m_parent.sublayer.Add((IPdfPrimitive) reference);
      else if (this.document.m_order.Contains((IPdfPrimitive) this.m_parent.ReferenceHolder))
      {
        this.document.m_order.RemoveAt(this.document.m_order.IndexOf((IPdfPrimitive) this.m_parent.ReferenceHolder) + 1);
        this.m_parent.sublayer.Add((IPdfPrimitive) reference);
      }
      else
        this.m_parent.sublayer.Add((IPdfPrimitive) reference);
      if (this.document.m_order.Contains((IPdfPrimitive) this.m_parent.ReferenceHolder))
        this.document.m_order.Insert(this.document.m_order.IndexOf((IPdfPrimitive) this.m_parent.ReferenceHolder) + 1, (IPdfPrimitive) this.m_parent.sublayer);
      else if (this.m_parent.parent != null)
      {
        this.m_parent.parent.sublayer.Contains((IPdfPrimitive) this.m_parent.ReferenceHolder);
        int num1 = this.m_parent.parent.sublayer.IndexOf((IPdfPrimitive) this.m_parent.ReferenceHolder);
        if (this.m_parent.sublayer.Count == 1)
          this.m_parent.parent.sublayer.Insert(num1 + 1, (IPdfPrimitive) this.m_parent.sublayer);
        if (this.document.m_order.Contains((IPdfPrimitive) this.m_parent.parent.ReferenceHolder))
        {
          int num2 = this.document.m_order.IndexOf((IPdfPrimitive) this.m_parent.parent.ReferenceHolder);
          this.document.m_order.RemoveAt(num2 + 1);
          this.document.m_order.Insert(num2 + 1, (IPdfPrimitive) this.m_parent.parent.sublayer);
        }
      }
      else if (this.document is PdfLoadedDocument)
      {
        for (int index = 0; index < this.document.m_order.Count; ++index)
        {
          if (this.document.m_order[index] is PdfArray && (this.document.m_order[index] as PdfArray).Contains((IPdfPrimitive) this.m_parent.ReferenceHolder))
          {
            int num = (this.document.m_order[index] as PdfArray).IndexOf((IPdfPrimitive) this.m_parent.ReferenceHolder);
            if (this.m_parent.sublayer.Count == 1)
            {
              (this.document.m_order[index] as PdfArray).Insert(num + 1, (IPdfPrimitive) this.m_parent.sublayer);
              break;
            }
          }
        }
      }
      if (!this.m_parent.m_child.Contains(layer))
        this.m_parent.m_child.Add(layer);
      if (this.m_parent.m_parentLayer.Count == 0)
      {
        layer.m_parentLayer.Add(this.m_parent);
      }
      else
      {
        for (int index = 0; index < this.m_parent.m_parentLayer.Count; ++index)
        {
          if (!layer.m_parentLayer.Contains(this.m_parent.m_parentLayer[index]))
            layer.m_parentLayer.Add(this.m_parent.m_parentLayer[index]);
        }
        if (layer.m_parentLayer.Contains(this.m_parent))
          return;
        layer.m_parentLayer.Add(this.m_parent);
      }
    }
  }

  private IPdfPrimitive CreateOptionalContentViews(PdfLayer layer)
  {
    PdfArray pdfArray = new PdfArray();
    this.m_OptionalContent["Name"] = (IPdfPrimitive) new PdfString("Layers");
    this.m_OptionalContent["Order"] = (IPdfPrimitive) this.document.m_order;
    this.m_OptionalContent["ON"] = (IPdfPrimitive) this.document.m_on;
    this.m_OptionalContent["OFF"] = (IPdfPrimitive) this.document.m_off;
    PdfArray primitive = new PdfArray();
    primitive.Add((IPdfPrimitive) new PdfName("Print"));
    PdfDictionary pdfDictionary = new PdfDictionary();
    pdfDictionary.SetProperty("Category", (IPdfPrimitive) primitive);
    pdfDictionary.SetProperty("OCGs", (IPdfPrimitive) this.document.m_printLayer);
    pdfDictionary.SetProperty("Event", (IPdfPrimitive) new PdfName("Print"));
    pdfArray.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary));
    this.m_OptionalContent["AS"] = (IPdfPrimitive) pdfArray;
    return (IPdfPrimitive) this.m_OptionalContent;
  }

  private IPdfPrimitive CreateOptionalContentViews(PdfLayer layer, bool isLoadedDocument)
  {
    PdfArray pdfArray = new PdfArray();
    this.m_OptionalContent["Name"] = (IPdfPrimitive) new PdfString("Layers");
    this.m_OptionalContent["Order"] = (IPdfPrimitive) this.document.m_order;
    this.m_OptionalContent["ON"] = (IPdfPrimitive) this.document.m_on;
    this.m_OptionalContent["OFF"] = (IPdfPrimitive) this.document.m_off;
    PdfArray primitive = new PdfArray();
    primitive.Add((IPdfPrimitive) new PdfName("Print"));
    PdfDictionary pdfDictionary = new PdfDictionary();
    pdfDictionary.SetProperty("Category", (IPdfPrimitive) primitive);
    pdfDictionary.SetProperty("OCGs", (IPdfPrimitive) this.document.m_printLayer);
    pdfDictionary.SetProperty("Event", (IPdfPrimitive) new PdfName("Print"));
    pdfArray.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary));
    this.m_OptionalContent["AS"] = (IPdfPrimitive) pdfArray;
    return (IPdfPrimitive) this.m_OptionalContent;
  }

  public void Remove(PdfLayer layer)
  {
    if (layer == null)
      throw new ArgumentNullException("layer not to be null");
    this.List.Remove((object) layer);
    this.RemoveLayer(layer, false);
  }

  public void Remove(PdfLayer layer, bool removeGraphicalContent)
  {
    if (layer == null)
      throw new ArgumentNullException("layer not to be null");
    this.List.Remove((object) layer);
    this.RemoveLayer(layer, removeGraphicalContent);
  }

  public void Remove(string name)
  {
    for (int index = 0; index < this.List.Count; ++index)
    {
      PdfLayer layer = this.List[index] as PdfLayer;
      if (layer.Name == name)
      {
        this.RemoveLayer(layer, false);
        this.List.Remove((object) layer);
        --index;
      }
    }
  }

  public void Remove(string name, bool removeGraphicalContent)
  {
    for (int index = 0; index < this.List.Count; ++index)
    {
      PdfLayer layer = this.List[index] as PdfLayer;
      if (layer.Name == name)
      {
        this.RemoveLayer(layer, removeGraphicalContent);
        this.List.Remove((object) layer);
        --index;
      }
    }
  }

  public void RemoveAt(int index)
  {
    if (index < 0 || index > this.List.Count - 1)
      throw new ArgumentOutOfRangeException(nameof (index), "Value can not be less 0 and greater List.Count - 1");
    PdfLayer layer = this[index];
    this.List.RemoveAt(index);
    if (layer == null)
      return;
    this.RemoveLayer(layer, false);
  }

  public void RemoveAt(int index, bool removeGraphicalContent)
  {
    if (index < 0 || index > this.List.Count - 1)
      throw new ArgumentOutOfRangeException(nameof (index), "Value can not be less 0 and greater List.Count - 1");
    PdfLayer layer = this[index];
    this.List.RemoveAt(index);
    if (layer == null)
      return;
    this.RemoveLayer(layer, removeGraphicalContent);
  }

  public bool Contains(PdfLayer layer)
  {
    return layer != null ? this.List.Contains((object) layer) : throw new ArgumentNullException("layer not to be null");
  }

  public bool Contains(string name)
  {
    if (name == null)
      throw new ArgumentNullException("layerName not to null");
    bool flag = false;
    for (int index = 0; index < this.List.Count; ++index)
    {
      PdfLayer pdfLayer = this.List[index] as PdfLayer;
      if (pdfLayer.Name == name)
      {
        flag = this.List.Contains((object) pdfLayer);
        break;
      }
    }
    return flag;
  }

  public int IndexOf(PdfLayer layer)
  {
    return layer != null ? this.List.IndexOf((object) layer) : throw new ArgumentNullException("layer not to be null");
  }

  public void Clear()
  {
    int index = 0;
    for (int count = this.List.Count; index < count; ++index)
      this.RemoveLayer(this[index], true);
    this.List.Clear();
  }

  private void RemoveLayer(PdfLayer layer, bool isRemoveContent)
  {
    if (layer == null)
      throw new ArgumentNullException("layer not to be null");
    if (layer == null && this.document == null)
      return;
    if (this.document != null)
    {
      PdfDictionary catalog = (PdfDictionary) this.document.Catalog;
      if (catalog.ContainsKey("OCProperties") && PdfCrossTable.Dereference(catalog["OCProperties"]) is PdfDictionary pdfDictionary1)
      {
        if (PdfCrossTable.Dereference(pdfDictionary1["OCGs"]) is PdfArray ocGroup)
          this.RemoveOCG(layer, ocGroup);
        if (pdfDictionary1.ContainsKey("D") && PdfCrossTable.Dereference(pdfDictionary1["D"]) is PdfDictionary pdfDictionary)
        {
          PdfArray on = (PdfArray) null;
          PdfArray off = (PdfArray) null;
          if (pdfDictionary.ContainsKey("Order") && PdfCrossTable.Dereference(pdfDictionary["Order"]) is PdfArray order)
          {
            System.Collections.Generic.List<PdfArray> arrayList = new System.Collections.Generic.List<PdfArray>();
            this.RemoveOrder(layer, order, arrayList);
          }
          if (pdfDictionary.ContainsKey("Locked") && PdfCrossTable.Dereference(pdfDictionary["Locked"]) is PdfArray locked)
            this.RemoveLocked(layer, locked);
          if (pdfDictionary.ContainsKey("OFF"))
            off = PdfCrossTable.Dereference(pdfDictionary["OFF"]) as PdfArray;
          if (pdfDictionary.ContainsKey("ON"))
            on = PdfCrossTable.Dereference(pdfDictionary["ON"]) as PdfArray;
          if (pdfDictionary.ContainsKey("AS") && PdfCrossTable.Dereference(pdfDictionary["AS"]) is PdfArray m_usage)
            this.RemoveUsage(layer, m_usage);
          this.RemoveVisible(layer, on, off);
        }
      }
    }
    if (!isRemoveContent)
      return;
    this.RemoveLayerContent(layer);
  }

  private void InsertLayer(int index, PdfLayer layer)
  {
    PdfReferenceHolder element = layer != null ? new PdfReferenceHolder((IPdfWrapper) layer) : throw new ArgumentNullException("layer not to be null");
    if (layer.ReferenceHolder != (PdfReferenceHolder) null && layer.LayerId != null)
    {
      if (layer.Page != null && layer.Page.Contents.Contains((IPdfPrimitive) element))
      {
        int index1 = layer.Page.Contents.IndexOf((IPdfPrimitive) element);
        layer.Page.Contents.RemoveAt(index1);
        layer.Page.Contents.Insert(index, (IPdfPrimitive) element);
      }
    }
    else if (layer.Page != null)
      layer.Page.Contents.Insert(index, (IPdfPrimitive) element);
    if (this.document == null || !this.document.Catalog.ContainsKey("OCProperties") || !(PdfCrossTable.Dereference(this.document.Catalog["OCProperties"]) is PdfDictionary pdfDictionary1))
      return;
    PdfArray pdfArray1 = PdfCrossTable.Dereference(pdfDictionary1["OCGs"]) as PdfArray;
    if (!pdfDictionary1.ContainsKey("D") || !(PdfCrossTable.Dereference(pdfDictionary1["D"]) is PdfDictionary pdfDictionary2) || !(PdfCrossTable.Dereference(pdfDictionary2["Order"]) is PdfArray pdfArray2) || pdfArray1 == null || !pdfArray2.Contains((IPdfPrimitive) layer.ReferenceHolder) || index >= pdfArray2.Count || (object) (pdfArray2[index] as PdfReferenceHolder) == null || index + 1 >= pdfArray2.Count || index + 2 >= pdfArray2.Count || (object) (pdfArray2[index + 1] as PdfReferenceHolder) == null || (object) (pdfArray2[index + 2] as PdfReferenceHolder) == null)
      return;
    pdfArray2.Remove((IPdfPrimitive) layer.ReferenceHolder);
    pdfArray2.Insert(index, (IPdfPrimitive) layer.ReferenceHolder);
    if (!pdfArray1.Contains((IPdfPrimitive) layer.ReferenceHolder))
      return;
    pdfArray1.Remove((IPdfPrimitive) layer.ReferenceHolder);
    pdfArray1.Insert(index, (IPdfPrimitive) layer.ReferenceHolder);
  }

  private void CheckLayerVisible(PdfDictionary ocProperties)
  {
    PdfLoadedDocument document = this.document as PdfLoadedDocument;
    PdfArray pdfArray = (PdfArray) null;
    if (!document.Catalog.ContainsKey("OCProperties"))
      return;
    if (PdfCrossTable.Dereference(ocProperties["D"]) is PdfDictionary pdfDictionary && pdfDictionary.ContainsKey("OFF"))
      pdfArray = PdfCrossTable.Dereference(pdfDictionary["OFF"]) as PdfArray;
    if (pdfArray == null)
      return;
    for (int index = 0; index < pdfArray.Count; ++index)
    {
      PdfLayer layer = this.layerDictionary[pdfArray[index] as PdfReferenceHolder];
      if (layer != null)
      {
        layer.m_visible = false;
        if (layer.Dictionary != null && layer.Dictionary.ContainsKey("Visible"))
          layer.Dictionary.SetProperty("Visible", (IPdfPrimitive) new PdfBoolean(false));
      }
    }
  }

  private void CheckLayerLock(PdfDictionary ocProperties)
  {
    PdfArray pdfArray = (PdfArray) null;
    if (PdfCrossTable.Dereference(ocProperties["D"]) is PdfDictionary pdfDictionary && pdfDictionary.ContainsKey("Locked"))
      pdfArray = PdfCrossTable.Dereference(pdfDictionary["Locked"]) as PdfArray;
    if (pdfArray == null)
      return;
    for (int index = 0; index < pdfArray.Count; ++index)
    {
      PdfLayer layer = this.layerDictionary[pdfArray[index] as PdfReferenceHolder];
      if (layer != null)
        layer.m_locked = true;
    }
  }

  private void RemoveOCG(PdfLayer layer, PdfArray ocGroup)
  {
    if (ocGroup == null || !ocGroup.Contains((IPdfPrimitive) layer.ReferenceHolder))
      return;
    ocGroup.Remove((IPdfPrimitive) layer.ReferenceHolder);
  }

  private void RemoveUsage(PdfLayer layer, PdfArray m_usage)
  {
    if (m_usage == null)
      return;
    bool flag = false;
    for (int index = 0; index < m_usage.Count; ++index)
    {
      if ((object) (m_usage[index] as PdfReferenceHolder) != null || m_usage[index] is PdfDictionary)
      {
        PdfDictionary pdfDictionary = (object) (m_usage[index] as PdfReferenceHolder) == null ? m_usage[index] as PdfDictionary : (m_usage[index] as PdfReferenceHolder).Object as PdfDictionary;
        if (pdfDictionary != null && pdfDictionary["OCGs"] is PdfArray pdfArray)
        {
          if (pdfArray.Contains((IPdfPrimitive) layer.ReferenceHolder))
          {
            pdfArray.Remove((IPdfPrimitive) layer.ReferenceHolder);
            flag = true;
          }
          if (flag)
            break;
        }
      }
    }
  }

  private void RemoveOrder(PdfLayer layer, PdfArray order, System.Collections.Generic.List<PdfArray> arrayList)
  {
    bool flag = false;
    if (order != null)
    {
      for (int index = 0; index < order.Count; ++index)
      {
        if ((object) (order[index] as PdfReferenceHolder) != null)
        {
          if (order[index].Equals((object) layer.ReferenceHolder))
          {
            if (index != order.Count - 1)
            {
              if (order[index + 1] is PdfArray)
              {
                order.RemoveAt(index);
                order.RemoveAt(index);
                flag = true;
                break;
              }
              order.RemoveAt(index);
              flag = true;
              break;
            }
            order.RemoveAt(index);
            flag = true;
            break;
          }
        }
        else if (order[index] is PdfArray)
          arrayList.Add(order[index] as PdfArray);
      }
    }
    if (flag || arrayList == null)
      return;
    int num;
    for (int index = 0; index < arrayList.Count; index = num + 1)
    {
      order = arrayList[index];
      arrayList.RemoveAt(index);
      num = index - 1;
      this.RemoveOrder(layer, order, arrayList);
    }
  }

  private void RemoveVisible(PdfLayer layer, PdfArray on, PdfArray off)
  {
    if (layer.Visible)
    {
      if (on == null || !on.Contains((IPdfPrimitive) layer.ReferenceHolder))
        return;
      on.Remove((IPdfPrimitive) layer.ReferenceHolder);
    }
    else
    {
      if (off == null || !off.Contains((IPdfPrimitive) layer.ReferenceHolder))
        return;
      off.Remove((IPdfPrimitive) layer.ReferenceHolder);
    }
  }

  private void RemoveLocked(PdfLayer layer, PdfArray locked)
  {
    if (locked == null || !locked.Contains((IPdfPrimitive) layer.ReferenceHolder))
      return;
    locked.Remove((IPdfPrimitive) layer.ReferenceHolder);
  }

  private void RemoveLayerContent(PdfLayer layer)
  {
    bool flag = false;
    if (layer.Page == null)
      return;
    for (int index1 = 0; index1 < layer.pages.Count; ++index1)
    {
      if (PdfCrossTable.Dereference(layer.pages[index1].Dictionary["Resources"]) is PdfDictionary pdfDictionary)
      {
        PdfDictionary pdfDictionary1 = PdfCrossTable.Dereference(pdfDictionary["Properties"]) as PdfDictionary;
        PdfDictionary pdfDictionary2 = PdfCrossTable.Dereference(pdfDictionary["XObject"]) as PdfDictionary;
        if (pdfDictionary1 != null && !string.IsNullOrEmpty(layer.LayerId))
        {
          if (pdfDictionary1.ContainsKey(layer.LayerId.TrimStart('/')))
            pdfDictionary1.Remove(layer.LayerId.TrimStart('/'));
        }
        if (pdfDictionary2 != null && layer.xobject.Count > 0)
        {
          foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in pdfDictionary2.Items)
          {
            if (layer.xobject.Contains(keyValuePair.Key.Value.TrimStart('/')))
            {
              pdfDictionary2.Remove(keyValuePair.Key);
              break;
            }
          }
        }
      }
      PdfArray contents = layer.pages[index1].Contents;
      for (int index2 = 0; index2 < contents.Count; ++index2)
      {
        MemoryStream memoryStream = new MemoryStream();
        PdfStream data = new PdfStream();
        if (layer.pages[index1] is PdfLoadedPage)
          (PdfCrossTable.Dereference(contents[index2]) as PdfStream).Decompress();
        (PdfCrossTable.Dereference(contents[index2]) as PdfStream).InternalStream.WriteTo((Stream) memoryStream);
        PdfRecordCollection recordCollection = new ContentParser(memoryStream.ToArray()).ReadContent();
        for (int index3 = 0; index3 < recordCollection.RecordCollection.Count; ++index3)
        {
          string operatorName = recordCollection.RecordCollection[index3].OperatorName;
          if (operatorName == "BMC" || operatorName == "EMC" || operatorName == "BDC")
          {
            this.ProcessBeginMarkContent(layer, operatorName, recordCollection.RecordCollection[index3].Operands, data);
            flag = true;
          }
          if (operatorName == "Do")
          {
            if (layer.xobject.Contains(recordCollection.RecordCollection[index3].Operands[0].TrimStart('/')))
              flag = true;
          }
          if (operatorName == "q" || operatorName == "Q" || operatorName == "w" || operatorName == "J" || operatorName == "j" || operatorName == "M" || operatorName == "d" || operatorName == "ri" || operatorName == "i" || operatorName == "gs" || operatorName == "g" || operatorName == "cm" || operatorName == "G" || operatorName == "rg" || operatorName == "RG" || operatorName == "k" || operatorName == "K" || operatorName == "cs" || operatorName == "CS" || operatorName == "scn" || operatorName == "SCN" || operatorName == "sc" || operatorName == "SC")
          {
            if (!flag)
              this.StreamWrite(recordCollection.RecordCollection[index3].Operands, operatorName, false, data);
            flag = false;
          }
          else
          {
            if (!flag)
              this.StreamWrite(recordCollection.RecordCollection[index3].Operands, operatorName, true, data);
            flag = false;
          }
        }
        if (data.Data.Length > 0)
        {
          (PdfCrossTable.Dereference(layer.pages[index1].Contents[index2]) as PdfStream).Clear();
          (PdfCrossTable.Dereference(layer.pages[index1].Contents[index2]) as PdfStream).Write(data.Data);
        }
        memoryStream.Dispose();
      }
    }
  }

  private void CheckParentLayer(PdfDictionary ocProperties)
  {
    if (!(PdfCrossTable.Dereference(ocProperties["D"]) is PdfDictionary pdfDictionary) || !(PdfCrossTable.Dereference(pdfDictionary["Order"]) is PdfArray array))
      return;
    this.ParsingLayerOrder((PdfLayer) null, array, this.layerDictionary);
  }

  private void ParsingLayerOrder(
    PdfLayer parent,
    PdfArray array,
    Dictionary<PdfReferenceHolder, PdfLayer> layerDictionary)
  {
    PdfLayer parent1 = (PdfLayer) null;
    for (int index1 = 0; index1 < array.Count; ++index1)
    {
      PdfReferenceHolder key = array[index1] as PdfReferenceHolder;
      if ((object) (array[index1] as PdfReferenceHolder) != null)
      {
        if (layerDictionary.ContainsKey(key))
          parent1 = layerDictionary[key];
        if (parent1 != null)
        {
          if (parent != null)
          {
            if (!parent.m_child.Contains(parent1))
              parent.m_child.Add(parent1);
            if (parent.m_parentLayer.Count == 0)
            {
              parent1.m_parentLayer.Add(parent);
              parent1.parent = parent;
            }
            else
            {
              for (int index2 = 0; index2 < parent.m_parentLayer.Count; ++index2)
              {
                if (!parent1.m_parentLayer.Contains(parent.m_parentLayer[index2]))
                  parent1.m_parentLayer.Add(parent.m_parentLayer[index2]);
              }
              parent1.m_parentLayer.Add(parent);
              parent1.parent = parent;
            }
          }
          if (array.Count > index1 + 1 && PdfCrossTable.Dereference(array[index1 + 1]) is PdfArray)
          {
            ++index1;
            PdfArray array1 = PdfCrossTable.Dereference(array[index1]) as PdfArray;
            parent1.sublayer = array1;
            this.ParsingLayerOrder(parent1, array1, layerDictionary);
          }
        }
      }
      else if (PdfCrossTable.Dereference(array[index1]) is PdfArray)
      {
        if (!(PdfCrossTable.Dereference(array[index1]) is PdfArray array2))
          break;
        PdfCrossTable.Dereference(array2[0]);
        if (array2[0] is PdfString)
        {
          parent = (PdfLayer) null;
          this.ParsingLayerOrder(parent, array2, layerDictionary);
        }
        else
        {
          parent = (PdfLayer) null;
          this.ParsingLayerOrder(parent, PdfCrossTable.Dereference(array[index1]) as PdfArray, layerDictionary);
        }
      }
    }
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
    PdfLayer parser,
    string m_operator,
    string[] operands,
    PdfStream data)
  {
    if ("BDC".Equals(m_operator.ToString()))
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
    this.StreamWrite(operands, m_operator, true, data);
    if (!"EMC".Equals(m_operator.ToString()) || this.m_bdcCount <= 0)
      return;
    --this.m_bdcCount;
  }

  private void CreateLayerHierarchical(PdfDictionary ocProperties)
  {
    if (!(PdfCrossTable.Dereference(ocProperties["D"]) is PdfDictionary pdfDictionary) || !pdfDictionary.ContainsKey("Order") || this.layerDictionary == null || this.layerDictionary.Count <= 0)
      return;
    this.List.Clear();
    foreach (KeyValuePair<PdfReferenceHolder, PdfLayer> layer1 in this.layerDictionary)
    {
      PdfLayer layer2 = layer1.Value;
      if (layer2 != null)
      {
        if (layer2.parent == null && !this.List.Contains((object) layer2))
          this.List.Add((object) layer2);
        else if (layer2.m_child.Count > 0)
          this.AddChildlayer(layer2.parent);
        else if (layer2.parent != null && layer2.m_child.Count == 0 && !layer2.parent.Layers.Contains(layer2))
          layer2.parent.Layers.AddNestedLayer(layer2);
      }
    }
  }

  private void AddChildlayer(PdfLayer pdflayer)
  {
    for (int index = 0; index < pdflayer.m_child.Count; ++index)
    {
      PdfLayer layer = pdflayer.m_child[index];
      if (!pdflayer.Layers.Contains(layer))
        pdflayer.Layers.AddNestedLayer(layer);
    }
  }
}
