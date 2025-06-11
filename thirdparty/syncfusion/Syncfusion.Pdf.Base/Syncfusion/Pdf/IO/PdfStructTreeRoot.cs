// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.IO.PdfStructTreeRoot
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.IO;

internal class PdfStructTreeRoot : PdfDictionary
{
  private PdfArray m_childSTR;
  private PdfPageBase m_pdfPage;
  [ThreadStatic]
  private static int m_id;
  private RectangleF m_BBoxBounds;
  private PdfArray m_nodeTree;
  private static object s_syncObject = new object();
  [ThreadStatic]
  private static int m_pageStructId;
  private PdfArray m_structTreeChild;
  private List<int> m_orderList = new List<int>();
  private Dictionary<string, IPdfPrimitive> m_ParentTagCollection;
  private PdfDictionary m_currentTreeRootChild;
  private List<PdfDictionary> m_structParentHierarchy;
  private PdfArray m_treeRootNode;
  private Dictionary<string, IPdfPrimitive> m_structChildCollection;
  private List<string> m_structElementTags;
  private PdfArray m_treeRootChilds;
  private PdfDictionary m_currentParent;
  internal bool isNewRow;
  internal bool m_SplitTable;
  private PdfDictionary m_structElementParent;
  internal bool isNewTable;
  private bool m_isNewListItem;
  private bool m_isNewList;
  internal bool m_isSubList;
  private PdfDictionary m_subListData;
  private PdfArray m_subListCollection;
  private bool m_isOrderAdded;
  internal bool m_isNestedGridRendered;
  private PdfArray m_prevRootNode;
  internal bool m_isChildGrid;
  private int m_orderIndex;
  private bool m_hasOrder;
  private PdfArray m_prevStructParent;
  internal bool m_isImage;
  private bool m_autoTag;
  internal bool m_WordtoPDFTaggedObject;

  internal List<int> OrderList
  {
    get => this.m_orderList;
    set => this.m_orderList = value;
  }

  internal PdfArray TreeRootNode
  {
    get => this.m_treeRootNode;
    set => this.m_treeRootNode = value;
  }

  internal bool IsNewList
  {
    get => this.m_isNewList;
    set => this.m_isNewList = value;
  }

  internal bool IsNewListItem
  {
    get => this.m_isNewListItem;
    set => this.m_isNewListItem = value;
  }

  internal bool HasOrder
  {
    get => this.m_hasOrder;
    set => this.m_hasOrder = true;
  }

  public PdfStructTreeRoot()
  {
    this["Type"] = (IPdfPrimitive) new PdfName("StructTreeRoot");
    PdfStructTreeRoot.m_id = 0;
    this.m_childSTR = new PdfArray();
    this.m_nodeTree = new PdfArray();
    this.m_structTreeChild = new PdfArray();
    this.m_ParentTagCollection = new Dictionary<string, IPdfPrimitive>();
    this.m_currentTreeRootChild = new PdfDictionary();
    this.m_treeRootNode = new PdfArray();
    this.m_structChildCollection = new Dictionary<string, IPdfPrimitive>();
    this.m_structElementTags = new List<string>();
    this.m_currentParent = new PdfDictionary();
    this.m_treeRootChilds = new PdfArray();
    this.m_structElementParent = new PdfDictionary();
    this.m_subListData = new PdfDictionary();
    this.m_subListCollection = new PdfArray();
    this.m_prevRootNode = new PdfArray();
    this.m_prevStructParent = new PdfArray();
  }

  internal int Add(string structType, string altText, PdfPageBase page, RectangleF bounds)
  {
    this.m_pdfPage = page;
    this.m_BBoxBounds = bounds;
    int num = this.Add(structType, altText, this.m_BBoxBounds);
    this.m_pdfPage = (PdfPageBase) null;
    return num;
  }

  internal int Add(string structType, string altText, RectangleF bounds)
  {
    lock (PdfStructTreeRoot.s_syncObject)
    {
      PdfDictionary pdfDictionary1 = new PdfDictionary();
      pdfDictionary1["S"] = (IPdfPrimitive) new PdfName(structType);
      pdfDictionary1["P"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) this);
      if (this.m_pdfPage != null)
        PdfStructTreeRoot.m_id = this.m_pdfPage.m_id;
      pdfDictionary1["K"] = (IPdfPrimitive) new PdfNumber(PdfStructTreeRoot.m_id++);
      pdfDictionary1["Lang"] = (IPdfPrimitive) new PdfString("English");
      if (structType != "P")
        pdfDictionary1["Alt"] = (IPdfPrimitive) new PdfString(altText);
      if (this.m_pdfPage != null)
        pdfDictionary1["Pg"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_pdfPage);
      PdfDictionary pdfDictionary2 = new PdfDictionary();
      pdfDictionary2["BBox"] = (IPdfPrimitive) new PdfArray(new float[4]
      {
        bounds.X,
        bounds.Y,
        bounds.Width,
        bounds.Height
      });
      if (structType == "P" && bounds != RectangleF.Empty)
        pdfDictionary1["A"] = (IPdfPrimitive) pdfDictionary2;
      PdfReferenceHolder element1 = new PdfReferenceHolder((IPdfPrimitive) pdfDictionary1);
      if (this.m_pdfPage != null && this.m_pdfPage.Dictionary.ContainsKey("StructParents"))
      {
        PdfNumber element2 = this.m_pdfPage.Dictionary["StructParents"] as PdfNumber;
        if (!this.m_nodeTree.Contains((IPdfPrimitive) element2))
        {
          this.m_pdfPage.m_childSTR = new PdfArray();
          this.m_pdfPage.m_childSTR.Add((IPdfPrimitive) element1);
          this.m_nodeTree.Add((IPdfPrimitive) element2);
          this.m_nodeTree.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) this.m_pdfPage.m_childSTR));
        }
        else
          this.m_pdfPage.m_childSTR.Add((IPdfPrimitive) element1);
      }
      this["ParentTree"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) new PdfDictionary()
      {
        ["Nums"] = (IPdfPrimitive) this.m_nodeTree
      });
      this["ParentTreeNextKey"] = (IPdfPrimitive) new PdfNumber(1);
      return PdfStructTreeRoot.m_id - 1;
    }
  }

  internal void Add(
    PdfStructureElement structElement,
    PdfPageBase page,
    PdfDictionary annotDictionary)
  {
    if (page is PdfPage)
    {
      PdfDocument document = (page as PdfPage).Document;
      if (document != null && document.AutoTag)
        this.m_autoTag = true;
    }
    PdfDictionary pdfDictionary1 = new PdfDictionary();
    pdfDictionary1["S"] = (IPdfPrimitive) new PdfName(this.ConvertToEquivalentTag(structElement.TagType));
    List<PdfStructureElement> rootParent = this.FindRootParent(structElement);
    bool flag1 = false;
    bool hasStructElement = false;
    if (structElement.Parent != null)
    {
      flag1 = this.IncludeParentForStructElement(structElement, rootParent, out hasStructElement);
      pdfDictionary1["P"] = this.m_currentParent["P"];
    }
    else
      pdfDictionary1["P"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) this);
    if (!this.m_autoTag)
      pdfDictionary1["Name"] = (IPdfPrimitive) new PdfName(structElement.m_name);
    PdfDictionary pdfDictionary2 = new PdfDictionary();
    if (page != null)
      pdfDictionary2["Pg"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) page);
    pdfDictionary2["Type"] = (IPdfPrimitive) new PdfName("OBJR");
    pdfDictionary2["Obj"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) annotDictionary);
    pdfDictionary1["K"] = (IPdfPrimitive) pdfDictionary2;
    if (structElement != null && page != null && !annotDictionary.ContainsKey("StructParent"))
      annotDictionary["StructParent"] = (IPdfPrimitive) new PdfNumber(PdfStructTreeRoot.m_pageStructId++);
    if (page != null && !page.Dictionary.ContainsKey("Tabs"))
      page.Dictionary["Tabs"] = (IPdfPrimitive) new PdfName("S");
    if (structElement.Order > 0 && !this.m_isOrderAdded)
    {
      this.m_orderList.Add(structElement.Order - 1);
      this.m_hasOrder = true;
    }
    else if (!this.m_isOrderAdded)
      this.m_orderList.Add(-1);
    PdfReferenceHolder element = new PdfReferenceHolder((IPdfPrimitive) pdfDictionary1);
    if (page != null && annotDictionary.ContainsKey("StructParent"))
    {
      PdfNumber annot = annotDictionary["StructParent"] as PdfNumber;
      if (!this.m_nodeTree.Contains((IPdfPrimitive) annot))
      {
        this.m_nodeTree.Add((IPdfPrimitive) annot);
        this.m_nodeTree.Add((IPdfPrimitive) element);
      }
    }
    this["ParentTree"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) new PdfDictionary()
    {
      ["Nums"] = (IPdfPrimitive) this.m_nodeTree
    });
    this["ParentTreeNextKey"] = (IPdfPrimitive) new PdfNumber(PdfStructTreeRoot.m_pageStructId);
    if (structElement.Parent != null)
    {
      this.m_treeRootNode.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary1));
      if (!this.m_isSubList && !this.m_isChildGrid)
      {
        bool flag2 = false;
        if (!this.m_autoTag && !flag1 && this.m_structTreeChild.Count != 0)
        {
          for (int index = 0; index < this.m_structTreeChild.Count; ++index)
          {
            flag2 = this.IncludeIdenticalParent(PdfCrossTable.Dereference(this.m_structTreeChild[index]) as PdfDictionary);
            if (flag2)
              break;
          }
        }
        if (!flag1 && !flag2)
          this.m_structTreeChild.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) this.m_currentTreeRootChild));
        this["K"] = (IPdfPrimitive) this.m_structTreeChild;
      }
    }
    else
      this.m_structTreeChild.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary1));
    this["K"] = (IPdfPrimitive) this.m_structTreeChild;
  }

  internal int Add(PdfStructureElement structElement, PdfPageBase page, RectangleF bounds)
  {
    if (page is PdfPage)
    {
      PdfDocument document = (page as PdfPage).Document;
      this.m_autoTag = document != null && document.AutoTag;
      if (document != null && !this.m_autoTag)
      {
        Dictionary<string, PdfDictionary> dicitionaryCollection = document.m_parnetTagDicitionaryCollection;
      }
      if (document != null)
        this.m_WordtoPDFTaggedObject = document.m_WordtoPDFTagged;
    }
    if (this.m_SplitTable)
      this.m_autoTag = false;
    PdfDictionary pdfDictionary1 = new PdfDictionary();
    pdfDictionary1["S"] = (IPdfPrimitive) new PdfName(this.ConvertToEquivalentTag(structElement.TagType));
    if (this.ConvertToEquivalentTag(structElement.TagType) == "TH")
    {
      if (structElement.Scope != ScopeType.None)
        page.Graphics.TableSpan.SetProperty("Scope", (IPdfPrimitive) new PdfName((Enum) structElement.Scope));
      pdfDictionary1.SetProperty("A", (IPdfPrimitive) page.Graphics.TableSpan);
    }
    if (this.ConvertToEquivalentTag(structElement.TagType) == "TD")
      pdfDictionary1.SetProperty("A", (IPdfPrimitive) page.Graphics.TableSpan);
    if (this.ConvertToEquivalentTag(structElement.TagType) == "Figure")
    {
      PdfMargins margins = (page as PdfPage).Document.PageSettings.Margins;
      RectangleF actualBounds = (page as PdfPage).Section.GetActualBounds(page as PdfPage, true);
      if ((double) bounds.Width <= 0.0)
        bounds.Width = 0.5f;
      if ((double) bounds.Height <= 0.0)
        bounds.Height = 0.5f;
      bounds = new RectangleF(bounds.X + actualBounds.X, page.Graphics.Size.Height - (bounds.Y + actualBounds.Y + bounds.Height), bounds.Width, bounds.Height);
      PdfDictionary pdfDictionary2 = new PdfDictionary();
      pdfDictionary2.SetName("O", "Layout");
      pdfDictionary2["BBox"] = (IPdfPrimitive) PdfArray.FromRectangle(bounds);
      pdfDictionary2.SetName("Placement", "Block");
      pdfDictionary1.SetProperty("A", (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary2));
    }
    bool flag1 = false;
    if (!page.Dictionary.ContainsKey("StructParents"))
    {
      this.m_structElementTags.Clear();
      this.m_ParentTagCollection.Clear();
      this.m_structChildCollection.Clear();
    }
    List<PdfStructureElement> rootParent = this.FindRootParent(structElement);
    bool hasStructElement = false;
    if (structElement.Parent != null)
    {
      flag1 = this.IncludeParentForStructElement(structElement, rootParent, out hasStructElement);
      pdfDictionary1["P"] = this.m_currentParent["P"];
    }
    else
      pdfDictionary1["P"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) this);
    if (!this.m_autoTag)
      pdfDictionary1["Name"] = (IPdfPrimitive) new PdfName(structElement.m_name);
    pdfDictionary1["Lang"] = structElement.Language == null ? (IPdfPrimitive) new PdfString("en-US") : (IPdfPrimitive) new PdfString(structElement.Language);
    if (structElement.Title != null)
      pdfDictionary1["T"] = (IPdfPrimitive) new PdfString(structElement.Title);
    if (structElement.ActualText != null)
      pdfDictionary1["ActualText"] = (IPdfPrimitive) new PdfString(structElement.ActualText);
    if (page != null)
      pdfDictionary1["Pg"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) page);
    if (structElement.AlternateText != null)
      pdfDictionary1["Alt"] = (IPdfPrimitive) new PdfString(structElement.AlternateText);
    else if (structElement.TagType == PdfTagType.Figure && this.m_isImage)
    {
      string str = Guid.NewGuid().ToString();
      pdfDictionary1["Alt"] = (IPdfPrimitive) new PdfString(str);
    }
    PdfReferenceHolder element1 = new PdfReferenceHolder((IPdfPrimitive) pdfDictionary1);
    if (structElement != null && page != null && !page.Dictionary.ContainsKey("StructParents"))
    {
      page.Dictionary["StructParents"] = (IPdfPrimitive) new PdfNumber(PdfStructTreeRoot.m_pageStructId++);
      page.m_id = 0;
    }
    pdfDictionary1["Type"] = (IPdfPrimitive) new PdfName("StructElem");
    pdfDictionary1["K"] = (IPdfPrimitive) new PdfNumber(page.m_id++);
    if (page != null && !page.Dictionary.ContainsKey("Tabs"))
      page.Dictionary["Tabs"] = (IPdfPrimitive) new PdfName("S");
    if (page != null && page.Dictionary.ContainsKey("StructParents"))
    {
      PdfNumber element2 = page.Dictionary["StructParents"] as PdfNumber;
      if (!this.m_nodeTree.Contains((IPdfPrimitive) element2))
      {
        page.m_childSTR = new PdfArray();
        page.m_childSTR.Add((IPdfPrimitive) element1);
        this.m_nodeTree.Add((IPdfPrimitive) element2);
        this.m_nodeTree.Add((IPdfPrimitive) page.m_childSTR);
      }
      else
        page.m_childSTR.Add((IPdfPrimitive) element1);
    }
    this["ParentTree"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) new PdfDictionary()
    {
      ["Nums"] = (IPdfPrimitive) this.m_nodeTree
    });
    this["ParentTreeNextKey"] = (IPdfPrimitive) new PdfNumber(PdfStructTreeRoot.m_pageStructId);
    if (structElement.Order > 0 && !this.m_isOrderAdded)
    {
      this.m_orderList.Add(structElement.Order - 1);
      this.m_hasOrder = true;
    }
    else if (!this.m_isOrderAdded && !hasStructElement)
      this.m_orderList.Add(-1);
    this.m_isOrderAdded = false;
    if (structElement.Parent != null)
    {
      this.m_treeRootNode.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary1));
      if (!this.m_isSubList && !this.m_isChildGrid || this.m_SplitTable)
      {
        bool flag2 = false;
        if (!this.m_autoTag && !flag1 && this.m_structTreeChild.Count != 0)
        {
          for (int index = 0; index < this.m_structTreeChild.Count; ++index)
          {
            flag2 = this.IncludeIdenticalParent(PdfCrossTable.Dereference(this.m_structTreeChild[index]) as PdfDictionary);
            if (flag2)
              break;
          }
        }
        if (!flag1 && !flag2)
          this.m_structTreeChild.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) this.m_currentTreeRootChild));
      }
      if (this.m_SplitTable && this.isNewTable)
      {
        this.isNewTable = false;
        this.isNewRow = false;
        this.m_autoTag = true;
      }
    }
    else
      this.m_structTreeChild.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary1));
    this["K"] = (IPdfPrimitive) this.m_structTreeChild;
    return page.m_id - 1;
  }

  private bool IncludeIdenticalParent(PdfDictionary currentElement)
  {
    bool flag = false;
    if (currentElement.ContainsKey("Name") && this.m_currentTreeRootChild.ContainsKey("Name"))
    {
      if (this.m_currentTreeRootChild["Name"] as PdfName == currentElement["Name"] as PdfName)
      {
        PdfArray pdfArray1 = PdfCrossTable.Dereference(currentElement["K"]) as PdfArray;
        PdfDictionary currentElement1 = PdfCrossTable.Dereference(currentElement["K"]) as PdfDictionary;
        PdfNumber pdfNumber = PdfCrossTable.Dereference(currentElement["K"]) as PdfNumber;
        if (!(PdfCrossTable.Dereference(this.m_currentTreeRootChild["K"]) is PdfArray pdfArray4))
        {
          if (PdfCrossTable.Dereference(this.m_currentTreeRootChild["K"]) is PdfDictionary pdfDictionary1)
          {
            if (pdfArray1 != null)
            {
              for (int index = 0; index < pdfArray1.Count; ++index)
              {
                if (PdfCrossTable.Dereference(pdfArray1[index]) is PdfDictionary currentElement2)
                {
                  if (pdfDictionary1["Name"] as PdfName == currentElement2["Name"] as PdfName)
                  {
                    this.m_currentTreeRootChild = pdfDictionary1;
                    flag = this.IncludeIdenticalParent(currentElement2);
                  }
                  if (flag)
                    return flag;
                }
              }
              pdfDictionary1["P"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) currentElement);
              pdfArray1.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary1));
              flag = true;
            }
            else
            {
              PdfArray pdfArray2 = new PdfArray();
              if (currentElement1 != null)
              {
                if (pdfDictionary1["Name"] as PdfName == currentElement1["Name"] as PdfName)
                {
                  this.m_currentTreeRootChild = pdfDictionary1;
                  flag = this.IncludeIdenticalParent(currentElement1);
                }
                if (flag)
                  return flag;
                pdfArray2.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) currentElement1));
              }
              if (pdfNumber != null)
                pdfArray2.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfNumber));
              pdfDictionary1["P"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) currentElement);
              pdfArray2.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary1));
              currentElement["K"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfArray2);
              flag = true;
            }
          }
        }
        else if (pdfArray4 != null && PdfCrossTable.Dereference(pdfArray4[0]) is PdfDictionary pdfDictionary2)
        {
          if (pdfArray1 != null)
          {
            for (int index = 0; index < pdfArray1.Count; ++index)
            {
              if (PdfCrossTable.Dereference(pdfArray1[index]) is PdfDictionary currentElement3)
              {
                if (pdfDictionary2["Name"] as PdfName == currentElement3["Name"] as PdfName)
                {
                  this.m_currentTreeRootChild = pdfDictionary2;
                  flag = this.IncludeIdenticalParent(currentElement3);
                }
                if (flag)
                  return flag;
              }
            }
            pdfDictionary2["P"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) currentElement);
            pdfArray1.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary2));
            flag = true;
          }
          else
          {
            PdfArray pdfArray3 = new PdfArray();
            if (currentElement1 != null)
            {
              if (pdfDictionary2["Name"] as PdfName == currentElement1["Name"] as PdfName)
              {
                this.m_currentTreeRootChild = pdfDictionary2;
                flag = this.IncludeIdenticalParent(currentElement1);
              }
              if (flag)
                return flag;
              pdfArray3.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) currentElement1));
            }
            if (pdfNumber != null)
              pdfArray3.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfNumber));
            pdfDictionary2["P"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) currentElement);
            pdfArray3.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary2));
            currentElement["K"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfArray3);
            flag = true;
          }
        }
        if (flag)
          return flag;
      }
      else
      {
        PdfArray pdfArray = PdfCrossTable.Dereference(currentElement["K"]) as PdfArray;
        PdfDictionary currentElement4 = PdfCrossTable.Dereference(currentElement["K"]) as PdfDictionary;
        if (pdfArray != null)
        {
          for (int index = 0; index < pdfArray.Count; ++index)
          {
            if (PdfCrossTable.Dereference(pdfArray[index]) is PdfDictionary currentElement5)
            {
              flag = this.IncludeIdenticalParent(currentElement5);
              if (flag)
                return flag;
            }
          }
        }
        else if (currentElement4 != null)
        {
          flag = this.IncludeIdenticalParent(currentElement4);
          if (flag)
            return flag;
        }
      }
    }
    return flag;
  }

  private bool IncludeParentForGroupElements(List<PdfStructureElement> elements)
  {
    this.m_structParentHierarchy = new List<PdfDictionary>();
    this.m_currentTreeRootChild = new PdfDictionary();
    return this.CheckTableChild(elements) || this.CheckSubList(elements) || this.AddEntriesForStructElement(elements);
  }

  private bool AddEntriesForStructElement(List<PdfStructureElement> elements)
  {
    if (this.isNewTable || this.IsNewList)
    {
      this.m_structElementParent = new PdfDictionary();
      this.m_treeRootChilds = new PdfArray();
      this.m_structElementParent["S"] = (IPdfPrimitive) new PdfName(this.ConvertToEquivalentTag(elements[0].TagType));
      this.m_structElementParent["P"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) this);
      if (!this.m_isOrderAdded && elements[0].Order >= 0)
        this.OrderList.Add(elements[0].Order - 1);
    }
    int index = 1;
    this.m_structParentHierarchy.Add(this.m_structElementParent);
    if (!this.m_autoTag)
      this.m_structElementParent["Name"] = (IPdfPrimitive) new PdfName(elements[0].m_name);
    bool flag = false;
    if (this.isNewTable || this.IsNewList)
    {
      this.m_treeRootNode = new PdfArray();
      this.SetParentForStructHierarchy(elements, this.m_structElementParent, index);
      this.SetChildForStructElement(this.m_structParentHierarchy, this.m_treeRootChilds);
      this.m_currentTreeRootChild = this.m_structParentHierarchy[0];
      this.m_currentParent = this.m_structParentHierarchy[this.m_structParentHierarchy.Count - 1];
      if (this.isNewTable)
        this.m_prevRootNode = this.m_treeRootNode;
      this.isNewTable = false;
      this.isNewRow = false;
      this.IsNewList = false;
      this.IsNewListItem = false;
    }
    else
    {
      if (this.isNewRow || this.IsNewListItem)
      {
        this.m_treeRootNode = new PdfArray();
        this.SetParentForStructHierarchy(elements, this.m_structElementParent, index);
        this.SetChildForStructElement(this.m_structParentHierarchy, this.m_treeRootChilds);
        this.m_currentParent = this.m_structParentHierarchy[this.m_structParentHierarchy.Count - 1];
        this.isNewRow = false;
        this.IsNewListItem = false;
      }
      this.m_currentTreeRootChild = this.m_structParentHierarchy[0];
      flag = true;
    }
    return flag;
  }

  private bool CheckTableChild(List<PdfStructureElement> elements)
  {
    if (this.m_isChildGrid)
    {
      if (this.isNewTable)
        this.m_prevRootNode = this.m_treeRootNode;
      this.SetSubEntries(elements, this.m_currentParent);
      return true;
    }
    if (this.m_isNestedGridRendered)
    {
      this.m_treeRootNode = this.m_prevRootNode;
      this.m_isNestedGridRendered = false;
    }
    return false;
  }

  private bool IncludeParentForStructElement(
    PdfStructureElement structElement,
    List<PdfStructureElement> elements,
    out bool hasStructElement)
  {
    if (this.m_autoTag && this.m_WordtoPDFTaggedObject && (this.AddTags(elements).Contains("Table") || this.AddTags(elements).Contains("List")))
      this.m_autoTag = false;
    if (this.m_autoTag && (this.AddTags(elements).Contains("Table") || this.AddTags(elements).Contains("List")))
    {
      hasStructElement = true;
      return this.IncludeParentForGroupElements(elements);
    }
    this.m_structParentHierarchy = new List<PdfDictionary>();
    this.m_currentTreeRootChild = new PdfDictionary();
    PdfDictionary structElementParent = new PdfDictionary();
    hasStructElement = false;
    structElementParent["S"] = (IPdfPrimitive) new PdfName(this.ConvertToEquivalentTag(elements[0].TagType));
    structElementParent["P"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) this);
    if (!this.m_autoTag)
      structElementParent["Name"] = (IPdfPrimitive) new PdfName(elements[0].m_name);
    int index = 1;
    this.m_structParentHierarchy.Add(structElementParent);
    bool flag = false;
    string str = this.AddTags(elements);
    if (this.IsIdenticalStructureElement(str) && this.m_autoTag)
    {
      flag = true;
      this.m_currentTreeRootChild = this.m_ParentTagCollection[str] as PdfDictionary;
      this.m_treeRootNode = this.m_structChildCollection[str] as PdfArray;
    }
    else
    {
      this.m_structElementTags.Add(this.AddTags(elements));
      this.m_treeRootNode = new PdfArray();
      this.SetParentForStructHierarchy(elements, structElementParent, index);
      this.SetTreeRootNodes(this.m_structParentHierarchy);
      this.m_currentTreeRootChild = this.m_structParentHierarchy[0];
      if (this.m_autoTag)
      {
        this.m_ParentTagCollection.Add(str, (IPdfPrimitive) this.m_structParentHierarchy[0]);
        this.m_structChildCollection.Add(str, (IPdfPrimitive) this.m_treeRootNode);
      }
      this.m_currentParent = this.m_structParentHierarchy[this.m_structParentHierarchy.Count - 1];
    }
    return flag;
  }

  private bool CheckSubList(List<PdfStructureElement> elements)
  {
    if (!this.m_isSubList)
      return false;
    this.SetSubEntries(elements, this.m_currentParent);
    return true;
  }

  private void SetSubEntries(List<PdfStructureElement> structElements, PdfDictionary currentParent)
  {
    if (this.IsNewList || this.isNewTable)
    {
      this.m_subListData = new PdfDictionary();
      this.m_treeRootNode.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) this.m_subListData));
      this.m_subListCollection = new PdfArray();
      this.m_subListData["S"] = (IPdfPrimitive) new PdfName(this.ConvertToEquivalentTag(structElements[0].TagType));
      this.m_subListData["P"] = (IPdfPrimitive) new PdfReferenceHolder(currentParent["P"]);
    }
    int index = 1;
    this.m_structParentHierarchy.Add(this.m_subListData);
    if (this.IsNewList || this.isNewTable)
    {
      this.m_treeRootNode = new PdfArray();
      this.SetParentForStructHierarchy(structElements, this.m_subListData, index);
      this.SetChildForStructElement(this.m_structParentHierarchy, this.m_subListCollection);
      this.m_currentTreeRootChild = this.m_structParentHierarchy[0];
      this.m_currentParent = this.m_structParentHierarchy[this.m_structParentHierarchy.Count - 1];
      this.IsNewList = false;
      this.IsNewListItem = false;
      this.isNewTable = false;
      this.isNewRow = false;
    }
    else
    {
      if (this.IsNewListItem || this.isNewRow)
      {
        this.m_treeRootNode = new PdfArray();
        this.SetParentForStructHierarchy(structElements, this.m_subListData, index);
        this.SetChildForStructElement(this.m_structParentHierarchy, this.m_subListCollection);
        this.m_currentParent = this.m_structParentHierarchy[this.m_structParentHierarchy.Count - 1];
        this.IsNewListItem = false;
        this.isNewRow = false;
      }
      this.m_currentTreeRootChild = this.m_structParentHierarchy[0];
    }
  }

  private string AddTags(List<PdfStructureElement> elements)
  {
    string str = "";
    foreach (PdfStructureElement element in elements)
      str += (string) (object) element.TagType;
    return str.TrimEnd();
  }

  private bool IsIdenticalStructureElement(string tag)
  {
    bool flag = false;
    if (this.m_structElementTags.Contains(tag))
      flag = true;
    return flag;
  }

  private void SetParentForStructHierarchy(
    List<PdfStructureElement> structElements,
    PdfDictionary structElementParent,
    int index)
  {
    if (index >= structElements.Count)
      return;
    PdfDictionary structElementParent1 = new PdfDictionary();
    structElementParent1["S"] = (IPdfPrimitive) new PdfName(this.ConvertToEquivalentTag(structElements[index].TagType));
    if (!this.m_autoTag)
      structElementParent1["Name"] = (IPdfPrimitive) new PdfName(structElements[index].m_name);
    structElementParent1["P"] = !((structElementParent["S"] as PdfName).Value == "Table") && !((structElementParent["S"] as PdfName).Value == "L") || this.m_prevStructParent.Count <= 0 ? (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) structElementParent) : this.m_prevStructParent[0];
    this.m_structParentHierarchy.Add(structElementParent1);
    this.SetParentForStructHierarchy(structElements, structElementParent1, ++index);
  }

  private void SetTreeRootNodes(List<PdfDictionary> treeRootItems)
  {
    PdfDictionary pdfDictionary = new PdfDictionary();
    for (int index = 0; index < treeRootItems.Count; ++index)
    {
      PdfDictionary treeRootItem = treeRootItems[index];
      if (index + 1 < treeRootItems.Count - 1)
        treeRootItem["K"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) treeRootItems[index + 1]);
      else if (index + 1 == treeRootItems.Count - 1)
        treeRootItem["K"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) this.m_treeRootNode);
      treeRootItems.RemoveAt(index);
      treeRootItems.Insert(index, treeRootItem);
    }
  }

  private void SetChildForStructElement(List<PdfDictionary> treeRootItems, PdfArray m_childs)
  {
    PdfDictionary pdfDictionary = new PdfDictionary();
    for (int index = 0; index < treeRootItems.Count; ++index)
    {
      PdfDictionary treeRootItem = treeRootItems[index];
      if ((treeRootItem["S"] as PdfName).Value == "Table" || (treeRootItem["S"] as PdfName).Value == "L")
      {
        m_childs.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) treeRootItems[index + 1]));
        treeRootItem["K"] = (IPdfPrimitive) m_childs;
      }
      else if ((treeRootItem["S"] as PdfName).Value == "TR" || (treeRootItem["S"] as PdfName).Value == "LI")
        treeRootItem["K"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) this.m_treeRootNode);
      else if (index < treeRootItems.Count - 1 && !treeRootItem.ContainsKey("K"))
      {
        treeRootItem["K"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) treeRootItems[index + 1]);
        if (this.isNewTable || this.IsNewList)
        {
          this.m_prevStructParent.Clear();
          this.m_prevStructParent.Insert(0, treeRootItem["K"]);
        }
      }
      treeRootItems.RemoveAt(index);
      treeRootItems.Insert(index, treeRootItem);
    }
  }

  internal void Dispose()
  {
    this.m_currentParent.Clear();
    this.m_currentTreeRootChild.Clear();
    this.m_orderList.Clear();
    this.m_structChildCollection.Clear();
    this.m_structElementParent.Clear();
    this.m_structElementTags.Clear();
    this.m_ParentTagCollection.Clear();
    this.m_subListCollection.Clear();
    this.m_subListData.Clear();
    this.m_treeRootChilds.Clear();
    this.m_treeRootNode.Clear();
  }

  private List<PdfStructureElement> FindRootParent(PdfStructureElement structElement)
  {
    List<PdfStructureElement> rootParent = new List<PdfStructureElement>();
    rootParent.Insert(0, structElement);
    this.AddOrderFromStructure(structElement);
    for (; structElement.Parent != null; structElement = structElement.Parent)
    {
      rootParent.Insert(0, structElement.Parent);
      this.AddOrderFromStructure(structElement.Parent);
    }
    return rootParent;
  }

  private void AddOrderFromStructure(PdfStructureElement structElement)
  {
    if (structElement.Order <= 0 || this.m_isOrderAdded || this.m_orderList.Contains(structElement.Order - 1))
      return;
    this.OrderList.Add(structElement.Order - 1);
    this.m_isOrderAdded = true;
    this.m_hasOrder = true;
  }

  internal string ConvertToEquivalentTag(PdfTagType tag)
  {
    string equivalentTag = "";
    switch (tag)
    {
      case PdfTagType.Annotation:
        equivalentTag = "Annot";
        break;
      case PdfTagType.Article:
        equivalentTag = "Art";
        break;
      case PdfTagType.BibliographyEntry:
        equivalentTag = "Bibentry";
        break;
      case PdfTagType.BlockQuotation:
        equivalentTag = "BlockQuote";
        break;
      case PdfTagType.Caption:
        equivalentTag = PdfTagType.Caption.ToString();
        break;
      case PdfTagType.Code:
        equivalentTag = PdfTagType.Code.ToString();
        break;
      case PdfTagType.Division:
        equivalentTag = "Div";
        break;
      case PdfTagType.Document:
        equivalentTag = "Document";
        break;
      case PdfTagType.Figure:
        equivalentTag = "Figure";
        break;
      case PdfTagType.Form:
        equivalentTag = "Form";
        break;
      case PdfTagType.Formula:
        equivalentTag = "Formula";
        break;
      case PdfTagType.Heading:
        equivalentTag = "H";
        break;
      case PdfTagType.HeadingLevel1:
        equivalentTag = "H1";
        break;
      case PdfTagType.HeadingLevel2:
        equivalentTag = "H2";
        break;
      case PdfTagType.HeadingLevel3:
        equivalentTag = "H3";
        break;
      case PdfTagType.HeadingLevel4:
        equivalentTag = "H4";
        break;
      case PdfTagType.HeadingLevel5:
        equivalentTag = "H5";
        break;
      case PdfTagType.HeadingLevel6:
        equivalentTag = "H6";
        break;
      case PdfTagType.Index:
        equivalentTag = "Index";
        break;
      case PdfTagType.Label:
        equivalentTag = "Lbl";
        break;
      case PdfTagType.Link:
        equivalentTag = "Link";
        break;
      case PdfTagType.List:
        equivalentTag = "L";
        break;
      case PdfTagType.ListBody:
        equivalentTag = "LBody";
        break;
      case PdfTagType.ListItem:
        equivalentTag = "LI";
        break;
      case PdfTagType.Note:
        equivalentTag = "Note";
        break;
      case PdfTagType.Paragraph:
        equivalentTag = "P";
        break;
      case PdfTagType.Part:
        equivalentTag = "Part";
        break;
      case PdfTagType.Quotation:
        equivalentTag = "Quote";
        break;
      case PdfTagType.Reference:
        equivalentTag = "Reference";
        break;
      case PdfTagType.Section:
        equivalentTag = "Sect";
        break;
      case PdfTagType.Span:
        equivalentTag = "Span";
        break;
      case PdfTagType.Table:
        equivalentTag = "Table";
        break;
      case PdfTagType.TableDataCell:
        equivalentTag = "TD";
        break;
      case PdfTagType.TableHeader:
        equivalentTag = "TH";
        break;
      case PdfTagType.TableOfContent:
        equivalentTag = "TOC";
        break;
      case PdfTagType.TableOfContentItem:
        equivalentTag = "TOCI";
        break;
      case PdfTagType.TableRow:
        equivalentTag = "TR";
        break;
    }
    return equivalentTag;
  }

  internal void ReOrderList(int maxLimit, List<int> orderList)
  {
    for (int index1 = 0; index1 < orderList.Count; ++index1)
    {
      if (orderList[index1] >= maxLimit)
      {
        int num1 = orderList[index1];
        int num2 = 0;
        List<int> orderList1 = new List<int>((IEnumerable<int>) orderList);
        do
        {
          int index2 = 0;
          for (int index3 = 0; index3 < orderList1.Count; ++index3)
          {
            if (orderList1[index3] >= 0 && orderList1[index3] < num1)
              index2 = index3;
          }
          if (index2 == 0)
            index2 = orderList1.IndexOf(num1);
          orderList1.RemoveAt(index2);
          orderList1.Insert(index2, -1);
          orderList[index2] = num2++;
          num1 = this.FindLowest(orderList1, maxLimit);
        }
        while (num1 >= 0 && orderList.Count > 0);
        break;
      }
    }
  }

  private int FindLowest(List<int> orderList, int maxLimit)
  {
    int lowest = -1;
    for (int index = 0; index < orderList.Count; ++index)
    {
      if (orderList[index] >= 0 && orderList[index] >= maxLimit)
        lowest = orderList[index];
    }
    return lowest;
  }

  internal void ReArrange(PdfArray childElements, List<int> orderList)
  {
    PdfReferenceHolder[] pdfReferenceHolderArray = new PdfReferenceHolder[childElements.Count];
    for (int index = 0; index < childElements.Count; ++index)
      pdfReferenceHolderArray[index] = childElements.Elements[index] as PdfReferenceHolder;
    if (childElements.Count == orderList.Count && !orderList.Contains(-1))
    {
      for (int index = 0; index < orderList.Count; ++index)
      {
        if (orderList[index] > childElements.Count)
          throw new ArgumentOutOfRangeException("Order value should not exceed the elements count.");
        if (orderList[index] < childElements.Count)
          childElements.Elements[orderList[index]] = (IPdfPrimitive) pdfReferenceHolderArray[index];
      }
    }
    else
    {
      for (int index1 = 0; index1 < orderList.Count; ++index1)
      {
        if (orderList[index1] >= 0)
        {
          PdfReferenceHolder element1 = pdfReferenceHolderArray[index1];
          int index2 = childElements.IndexOf((IPdfPrimitive) element1);
          PdfReferenceHolder element2 = childElements.Elements[orderList[index1]] as PdfReferenceHolder;
          int index3 = childElements.Elements.IndexOf((IPdfPrimitive) element2);
          childElements.Elements.RemoveAt(index2);
          childElements.Elements.Insert(index2, (IPdfPrimitive) element2);
          childElements.Elements.RemoveAt(index3);
          childElements.Elements.Insert(index3, (IPdfPrimitive) element1);
        }
      }
    }
  }

  private List<int> ArrangeChildWithOrder(int elementCount)
  {
    List<int> intList = new List<int>((IEnumerable<int>) this.OrderList.GetRange(this.m_orderIndex, elementCount));
    this.m_orderIndex += intList.Count;
    return intList;
  }

  internal void GetChildElements(PdfArray treeRootChild)
  {
    for (int index1 = 0; index1 < treeRootChild.Count; ++index1)
    {
      PdfDictionary pdfDictionary1 = (treeRootChild[index1] as PdfReferenceHolder).Object as PdfDictionary;
      PdfName pdfName = pdfDictionary1["S"] as PdfName;
      if (pdfName.Value != "Table" && pdfName.Value != "L" && pdfName.Value != "Form")
      {
        for (; pdfDictionary1.ContainsKey("K"); pdfDictionary1 = pdfDictionary2)
        {
          if (!(PdfCrossTable.Dereference(pdfDictionary1["K"]) is PdfDictionary pdfDictionary2))
          {
            if (PdfCrossTable.Dereference(pdfDictionary1["K"]) is PdfArray childElements)
            {
              List<int> orderList = this.ArrangeChildWithOrder(childElements.Count);
              this.ReOrderList(childElements.Count, orderList);
              this.ReArrange(childElements, orderList);
              break;
            }
            break;
          }
        }
      }
      else if (!this.m_WordtoPDFTaggedObject || !(pdfName.Value == "Table"))
      {
        List<int> orderList = this.ArrangeChildWithOrder(1);
        int num = index1;
        for (int index2 = 0; index2 < num; ++index2)
          orderList.Insert(index2, -1);
        this.ReArrange(treeRootChild, orderList);
      }
    }
  }
}
