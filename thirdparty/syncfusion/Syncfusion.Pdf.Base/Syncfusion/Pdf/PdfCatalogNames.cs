// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfCatalogNames
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf;

internal class PdfCatalogNames : IPdfWrapper
{
  private PdfAttachmentCollection m_attachments;
  internal PdfDictionary m_dictionary = new PdfDictionary();

  public PdfCatalogNames()
  {
  }

  public PdfCatalogNames(PdfDictionary root)
  {
    this.m_dictionary = root != null ? root : throw new ArgumentNullException(nameof (root));
  }

  public PdfAttachmentCollection EmbeddedFiles
  {
    get => this.m_attachments;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (EmbeddedFiles));
      if (this.m_attachments == value)
        return;
      this.m_attachments = value;
      this.m_dictionary.SetProperty(nameof (EmbeddedFiles), (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_attachments));
    }
  }

  internal PdfDictionary Destinations
  {
    get => PdfCrossTable.Dereference(this.m_dictionary["Dests"]) as PdfDictionary;
  }

  internal IPdfPrimitive GetNamedObjectFromTree(PdfDictionary root, PdfString name)
  {
    bool flag = false;
    PdfDictionary current = root;
    IPdfPrimitive namedObjectFromTree = (IPdfPrimitive) null;
    while (!flag && current != null && current.Items.Count > 0)
    {
      if (current.ContainsKey("Kids"))
        current = this.GetProperKid(current, name);
      else if (current.ContainsKey("Names"))
      {
        namedObjectFromTree = this.FindName(current, name);
        flag = true;
      }
    }
    return namedObjectFromTree;
  }

  private IPdfPrimitive FindName(PdfDictionary current, PdfString name)
  {
    PdfArray pdfArray = PdfCrossTable.Dereference(current["Names"]) as PdfArray;
    int num1 = pdfArray.Count / 2;
    int num2 = 0;
    int num3 = num1 - 1;
    int num4 = 0;
    bool flag = false;
    while (!flag)
    {
      num4 = (num2 + num3) / 2;
      if (num2 <= num3)
      {
        PdfString str2 = PdfCrossTable.Dereference(pdfArray[num4 * 2]) as PdfString;
        int num5 = PdfString.ByteCompare(name, str2);
        if (num5 > 0)
          num2 = num4 + 1;
        else if (num5 < 0)
        {
          num3 = num4 - 1;
        }
        else
        {
          flag = true;
          break;
        }
      }
      else
        break;
    }
    IPdfPrimitive name1 = (IPdfPrimitive) null;
    if (flag)
      name1 = PdfCrossTable.Dereference(pdfArray[num4 * 2 + 1]);
    return name1;
  }

  private PdfDictionary GetProperKid(PdfDictionary current, PdfString name)
  {
    PdfArray pdfArray = PdfCrossTable.Dereference(current["Kids"]) as PdfArray;
    PdfDictionary kid = (PdfDictionary) null;
    foreach (IPdfPrimitive pdfPrimitive in pdfArray)
    {
      kid = PdfCrossTable.Dereference(pdfPrimitive) as PdfDictionary;
      if (!this.CheckLimits(kid, name))
        kid = (PdfDictionary) null;
      else
        break;
    }
    return kid;
  }

  private bool CheckLimits(PdfDictionary kid, PdfString name)
  {
    PdfArray pdfArray = kid["Limits"] as PdfArray;
    bool flag = false;
    if (pdfArray != null && pdfArray.Count >= 2)
    {
      PdfString str1_1 = pdfArray[0] as PdfString;
      PdfString str1_2 = pdfArray[1] as PdfString;
      int num1 = PdfString.ByteCompare(str1_1, name);
      int num2 = PdfString.ByteCompare(str1_2, name);
      if (num1 == 0 || num2 == 0)
        flag = true;
      else if (num1 < 0 && num2 > 0)
        flag = true;
    }
    return flag;
  }

  public IPdfPrimitive Element => (IPdfPrimitive) this.m_dictionary;

  internal void MergeEmbedded(PdfCatalogNames names, PdfCrossTable crossTable)
  {
    this.AppendEmbedded(names.GetEmbedded(), crossTable);
  }

  private void AppendEmbedded(List<IPdfPrimitive> embedded, PdfCrossTable crossTable)
  {
    if (embedded == null || embedded.Count <= 0)
      return;
    if (!(PdfCrossTable.Dereference(this.m_dictionary["EmbeddedFiles"]) is PdfDictionary pdfDictionary))
    {
      pdfDictionary = new PdfDictionary();
      pdfDictionary["Names"] = (IPdfPrimitive) new PdfArray();
      this.m_dictionary["EmbeddedFiles"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary);
    }
    string baseName = string.Empty;
    PdfDictionary node = (PdfDictionary) null;
    if (pdfDictionary.ContainsKey("Names"))
    {
      node = pdfDictionary;
      baseName = this.GetNodeRightLimit(node);
    }
    else if (pdfDictionary.ContainsKey("Kids"))
    {
      PdfArray pdfArray = PdfCrossTable.Dereference(pdfDictionary["Kids"]) as PdfArray;
      baseName = this.GetNodeRightLimit(PdfCrossTable.Dereference(pdfArray[pdfArray.Count - 1]) as PdfDictionary);
      node = new PdfDictionary();
      node["Names"] = (IPdfPrimitive) new PdfArray();
      pdfArray.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) node));
    }
    this.AppendObjects(baseName, node, embedded, node != pdfDictionary, crossTable);
  }

  private string GetNodeRightLimit(PdfDictionary node)
  {
    PdfArray pdfArray1 = PdfCrossTable.Dereference(node["Limits"]) as PdfArray;
    PdfString pdfString = (PdfString) null;
    if (pdfArray1 != null)
      pdfString = PdfCrossTable.Dereference(pdfArray1[1]) as PdfString;
    string empty = string.Empty;
    if (pdfString != null)
      empty = pdfString.Value;
    else if (node.ContainsKey("Names"))
    {
      PdfArray pdfArray2 = PdfCrossTable.Dereference(node["Names"]) as PdfArray;
      if (pdfArray2.Count > 1)
        empty = (PdfCrossTable.Dereference(pdfArray2[pdfArray2.Count - 2]) as PdfString).Value;
    }
    return empty;
  }

  private void AppendObjects(
    string baseName,
    PdfDictionary node,
    List<IPdfPrimitive> embedded,
    bool updateLimits,
    PdfCrossTable crossTable)
  {
    PdfArray loadedAttachmentNames = PdfCrossTable.Dereference(node["Names"]) as PdfArray;
    int count = embedded.Count;
    int num = 0;
    for (int index = 0; index < count; ++index)
    {
      if (crossTable == null)
        loadedAttachmentNames.Add(embedded[index]);
      else
        loadedAttachmentNames.Add(embedded[index].Clone(crossTable));
    }
    this.SortAttachmentNames(loadedAttachmentNames, node);
    if (!updateLimits)
      return;
    PdfString element1 = new PdfString(baseName + count.ToString("X"));
    PdfString element2 = new PdfString(baseName + num.ToString("X"));
    PdfArray pdfArray = new PdfArray();
    node["Limits"] = (IPdfPrimitive) pdfArray;
    pdfArray.Add((IPdfPrimitive) element2);
    pdfArray.Add((IPdfPrimitive) element1);
  }

  private void SortAttachmentNames(PdfArray loadedAttachmentNames, PdfDictionary catalogNames)
  {
    Dictionary<string, IPdfPrimitive> attachmentCollection = new Dictionary<string, IPdfPrimitive>();
    if (loadedAttachmentNames == null || loadedAttachmentNames.Count <= 0)
      return;
    for (int index = 0; index < loadedAttachmentNames.Count; index += 2)
    {
      string str = (string) null;
      if (loadedAttachmentNames[index] is PdfString loadedAttachmentName1)
      {
        str = loadedAttachmentName1.Value;
      }
      else
      {
        PdfName loadedAttachmentName = loadedAttachmentNames[index] as PdfName;
        if (loadedAttachmentName != (PdfName) null)
          str = loadedAttachmentName.Value;
      }
      if (!string.IsNullOrEmpty(str))
      {
        if (!attachmentCollection.ContainsKey(str))
        {
          IPdfPrimitive loadedAttachmentName2 = loadedAttachmentNames[index + 1];
          if (loadedAttachmentName2 != null)
            attachmentCollection[str] = loadedAttachmentName2;
        }
        else
        {
          string uniqueName = this.GetUniqueName(str, attachmentCollection);
          IPdfPrimitive loadedAttachmentName3 = loadedAttachmentNames[index + 1];
          if (loadedAttachmentName3 != null)
            attachmentCollection[uniqueName] = loadedAttachmentName3;
        }
      }
    }
    System.StringComparer ordinal = System.StringComparer.Ordinal;
    List<string> stringList = new List<string>((IEnumerable<string>) attachmentCollection.Keys);
    stringList.Sort((IComparer<string>) ordinal);
    PdfArray primitive = new PdfArray();
    foreach (string key in stringList)
    {
      primitive.Add((IPdfPrimitive) new PdfString(key));
      primitive.Add(attachmentCollection[key]);
    }
    catalogNames.SetProperty("Names", (IPdfPrimitive) primitive);
    attachmentCollection.Clear();
    stringList.Clear();
  }

  private string GetUniqueName(
    string attachmentName,
    Dictionary<string, IPdfPrimitive> attachmentCollection)
  {
    int num1 = 0;
    string str1 = attachmentName;
    int num2 = num1;
    int num3 = num2 + 1;
    string str2 = num2.ToString();
    string key = str1 + str2;
    while (attachmentCollection.ContainsKey(key))
      key = attachmentName + num3++.ToString();
    return key;
  }

  private List<IPdfPrimitive> GetEmbedded()
  {
    PdfDictionary pdfDictionary1 = PdfCrossTable.Dereference(this.m_dictionary["EmbeddedFiles"]) as PdfDictionary;
    List<IPdfPrimitive> array = (List<IPdfPrimitive>) null;
    if (pdfDictionary1 != null && this.m_dictionary.ContainsKey("EmbeddedFiles"))
    {
      array = new List<IPdfPrimitive>();
      Stack<PdfCatalogNames.NodeInfo> nodeInfoStack = new Stack<PdfCatalogNames.NodeInfo>();
      PdfDictionary pdfDictionary2 = pdfDictionary1;
      PdfCatalogNames.NodeInfo nodeInfo = new PdfCatalogNames.NodeInfo(0, 1);
      while (true)
      {
        do
        {
          if (nodeInfo.Index < nodeInfo.Count)
          {
            if (nodeInfo.Kids != null)
              pdfDictionary2 = PdfCrossTable.Dereference(nodeInfo.Kids[nodeInfo.Index]) as PdfDictionary;
            if (pdfDictionary2.ContainsKey("Kids"))
            {
              nodeInfoStack.Push(nodeInfo);
              nodeInfo = new PdfCatalogNames.NodeInfo(pdfDictionary2);
            }
            else
              goto label_6;
          }
        }
        while (nodeInfoStack.Count > 0);
        break;
label_6:
        if (pdfDictionary2.ContainsKey("Names"))
        {
          this.CollectObjects(pdfDictionary2, array);
          if (nodeInfoStack.Count > 0)
            nodeInfo = nodeInfoStack.Pop();
        }
        ++nodeInfo.Index;
      }
    }
    return array;
  }

  private void CollectObjects(PdfDictionary leafNode, List<IPdfPrimitive> array)
  {
    PdfArray pdfArray = PdfCrossTable.Dereference(leafNode["Names"]) as PdfArray;
    int index = 0;
    for (int count = pdfArray.Count; index < count; ++index)
      array.Add(pdfArray[index]);
  }

  internal void Clear()
  {
    if (this.m_attachments != null)
      this.m_attachments.Clear();
    if (this.m_dictionary == null)
      return;
    this.m_dictionary.Clear();
  }

  private class NodeInfo
  {
    public PdfDictionary Node;
    public int Index;
    public int Count;
    public PdfArray Kids;

    public NodeInfo(PdfDictionary node)
    {
      this.Node = node != null ? node : throw new ArgumentNullException(nameof (node));
      this.Kids = PdfCrossTable.Dereference(node[nameof (Kids)]) as PdfArray;
      this.Count = this.Kids.Count;
    }

    public NodeInfo(int index, int count)
    {
      this.Index = index;
      this.Count = count;
    }
  }
}
