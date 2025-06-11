// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.PartContainer
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.Compression.Zip;
using Syncfusion.DocIO.DLS.Convertors;
using System.Collections.Generic;
using System.IO;
using System.Xml;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class PartContainer
{
  protected string m_name;
  protected Dictionary<string, Part> m_xmlParts;
  protected Dictionary<string, PartContainer> m_xmlPartContainers;
  protected Dictionary<string, Syncfusion.DocIO.DLS.Relations> m_relations;

  internal Dictionary<string, Part> XmlParts
  {
    get
    {
      if (this.m_xmlParts == null)
        this.m_xmlParts = new Dictionary<string, Part>();
      return this.m_xmlParts;
    }
  }

  internal Dictionary<string, PartContainer> XmlPartContainers
  {
    get
    {
      if (this.m_xmlPartContainers == null)
        this.m_xmlPartContainers = new Dictionary<string, PartContainer>();
      return this.m_xmlPartContainers;
    }
  }

  internal Dictionary<string, Syncfusion.DocIO.DLS.Relations> Relations
  {
    get
    {
      if (this.m_relations == null)
        this.m_relations = new Dictionary<string, Syncfusion.DocIO.DLS.Relations>();
      return this.m_relations;
    }
  }

  internal string Name
  {
    get => this.m_name;
    set => this.m_name = value;
  }

  internal void AddPart(ZipArchiveItem item)
  {
    this.AddPart(new Part(item.DataStream)
    {
      Name = this.GetPartName(item.ItemName)
    });
  }

  internal void AddPart(Part xmlPart) => this.XmlParts.Add(xmlPart.Name, xmlPart);

  internal void AddPartContainer(PartContainer container)
  {
    this.XmlPartContainers.Add(container.Name, container);
  }

  internal PartContainer EnsurePartContainer(string[] nameParts, int startNameIndex)
  {
    if (nameParts.Length == 1)
      return this;
    string key = nameParts[startNameIndex] + "/";
    PartContainer container = (PartContainer) null;
    if (this.XmlPartContainers.ContainsKey(key))
      container = this.XmlPartContainers[key];
    if (container == null)
    {
      if (key.EndsWith("_rels/"))
        return this;
      container = new PartContainer();
      container.Name = key;
      this.AddPartContainer(container);
    }
    return startNameIndex < nameParts.Length - 2 ? container.EnsurePartContainer(nameParts, ++startNameIndex) : container;
  }

  internal void LoadRelations(ZipArchiveItem item)
  {
    Syncfusion.DocIO.DLS.Relations relations = new Syncfusion.DocIO.DLS.Relations(item);
    this.Relations.Add(item.ItemName, relations);
  }

  private string GetPartName(string fullPath)
  {
    int startIndex = fullPath.LastIndexOf('/') + 1;
    int length = fullPath.Length - startIndex;
    return fullPath.Substring(startIndex, length);
  }

  internal PartContainer Clone()
  {
    PartContainer partContainer = new PartContainer();
    partContainer.Name = this.m_name;
    if (this.m_xmlParts != null && this.m_xmlParts.Count > 0)
    {
      foreach (string key in this.m_xmlParts.Keys)
        partContainer.XmlParts.Add(key, this.m_xmlParts[key].Clone());
    }
    if (this.m_relations != null && this.m_relations.Count > 0)
    {
      foreach (string key in this.m_relations.Keys)
        partContainer.Relations.Add(key, this.m_relations[key].Clone() as Syncfusion.DocIO.DLS.Relations);
    }
    if (this.m_xmlPartContainers != null && this.m_xmlPartContainers.Count > 0)
    {
      foreach (string key in this.m_xmlPartContainers.Keys)
        partContainer.XmlPartContainers.Add(key, this.m_xmlPartContainers[key].Clone());
    }
    return partContainer;
  }

  internal string CopyXmlPartContainer(
    PartContainer newContainer,
    Package srcPackage,
    string[] parts,
    int index)
  {
    string str = "";
    for (int index1 = 0; index1 < parts.Length; ++index1)
    {
      if (index1 >= index)
      {
        if (index1 == parts.Length - 1)
        {
          str = this.CopyXmlPartItems(newContainer, srcPackage, parts[index1]);
        }
        else
        {
          string key = parts[index1] + "/";
          PartContainer newContainer1 = new PartContainer();
          str = this.m_xmlPartContainers[key].CopyXmlPartContainer(newContainer1, srcPackage, parts, index1 + 1);
          newContainer.XmlPartContainers.Add(key, newContainer1);
          break;
        }
      }
    }
    return str;
  }

  internal string CopyXmlPartItems(PartContainer newContainer, Package srcPackage, string partName)
  {
    string key = "";
    if (newContainer.XmlParts.ContainsKey(partName))
    {
      string extension = Path.GetExtension(partName);
      string str = partName.Replace(extension, "");
      string oldValue = str.TrimEnd('0', '1', '2', '3', '4', '5', '6', '7', '8', '9');
      int result = 0;
      int.TryParse(str.Replace(oldValue, ""), out result);
      for (key = oldValue + result.ToString() + extension; newContainer.XmlParts.ContainsKey(key); key = oldValue + result.ToString() + extension)
        ++result;
    }
    if (this.m_xmlParts != null && this.m_xmlParts.ContainsKey(partName))
    {
      Part part = this.m_xmlParts[partName].Clone();
      if (!string.IsNullOrEmpty(key))
        part.Name = key;
      newContainer.XmlParts.Add(part.Name, part);
    }
    if (this.m_relations != null)
    {
      string xmlPartRelationKey = this.GetXmlPartRelationKey(partName);
      if (this.m_relations.ContainsKey(xmlPartRelationKey))
      {
        Syncfusion.DocIO.DLS.Relations relation = this.m_relations[xmlPartRelationKey].Clone() as Syncfusion.DocIO.DLS.Relations;
        if (!string.IsNullOrEmpty(key))
          relation.Name = xmlPartRelationKey.Replace(partName + ".rels", key + ".rels");
        if (newContainer.Relations.ContainsKey(relation.Name))
          newContainer.Relations.Remove(relation.Name);
        newContainer.Relations.Add(relation.Name, relation);
        Dictionary<string, string> innerRelationTarget = this.CopyInnerRelatedXmlParts(newContainer, srcPackage, relation, partName);
        this.UpdateInnerRelationTarget(relation, innerRelationTarget);
      }
    }
    return key;
  }

  internal string GetXmlPartRelationKey(string partName)
  {
    string xmlPartRelationKey = "";
    if (this.m_relations == null)
      return xmlPartRelationKey;
    foreach (string key in this.m_relations.Keys)
    {
      if (key.EndsWith(partName + ".rels"))
      {
        xmlPartRelationKey = key;
        break;
      }
    }
    return xmlPartRelationKey;
  }

  private void UpdateInnerRelationTarget(
    Syncfusion.DocIO.DLS.Relations relation,
    Dictionary<string, string> innerRelationTarget)
  {
    Stream dataStream = relation.DataStream;
    dataStream.Position = 0L;
    XmlReader reader = UtilityMethods.CreateReader(dataStream);
    MemoryStream output = new MemoryStream();
    XmlWriterSettings settings = new XmlWriterSettings();
    XmlWriter xmlWriter = XmlWriter.Create((Stream) output, settings);
    xmlWriter.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"");
    bool eof;
    do
    {
      bool flag = false;
      switch (reader.NodeType)
      {
        case XmlNodeType.Element:
          xmlWriter.WriteStartElement(reader.Prefix, reader.LocalName, reader.NamespaceURI);
          for (int i = 0; i < reader.AttributeCount; ++i)
          {
            reader.MoveToAttribute(i);
            switch (reader.LocalName)
            {
              case "target":
              case "Target":
                xmlWriter.WriteAttributeString(reader.Prefix, reader.LocalName, reader.NamespaceURI, innerRelationTarget.ContainsKey(reader.Value) ? innerRelationTarget[reader.Value] : reader.Value);
                break;
              default:
                xmlWriter.WriteAttributeString(reader.Prefix, reader.LocalName, reader.NamespaceURI, reader.Value);
                break;
            }
          }
          reader.MoveToElement();
          if (!reader.IsEmptyElement)
          {
            string localName = reader.LocalName;
            reader.Read();
            flag = true;
            if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
            {
              xmlWriter.WriteEndElement();
              break;
            }
            break;
          }
          xmlWriter.WriteEndElement();
          break;
        case XmlNodeType.Text:
          xmlWriter.WriteString(reader.Value);
          break;
        case XmlNodeType.SignificantWhitespace:
          xmlWriter.WriteWhitespace(reader.Value);
          break;
        case XmlNodeType.EndElement:
          xmlWriter.WriteEndElement();
          break;
      }
      eof = reader.EOF;
      if (!flag && !eof)
        reader.Read();
    }
    while (!eof);
    xmlWriter.Flush();
    output.Flush();
    output.Position = 0L;
    relation.SetDataStream((Stream) output);
  }

  private Dictionary<string, string> CopyInnerRelatedXmlParts(
    PartContainer newContainer,
    Package srcPackage,
    Syncfusion.DocIO.DLS.Relations relation,
    string curPartName)
  {
    Dictionary<string, string> dictionary = new Dictionary<string, string>();
    Stream dataStream = relation.DataStream;
    dataStream.Position = 0L;
    XmlReader reader = UtilityMethods.CreateReader(dataStream);
    int content = (int) reader.MoveToContent();
    if (reader.LocalName != "Relationships")
    {
      reader.ReadInnerXml();
      return dictionary;
    }
    if (!reader.IsEmptyElement)
    {
      string localName = reader.LocalName;
      reader.Read();
      bool flag = true;
      if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
        return dictionary;
      do
      {
        if (!flag)
          reader.Read();
        if (!(reader.GetAttribute("TargetMode") == "External"))
        {
          string attribute = reader.GetAttribute("Target");
          if (!string.IsNullOrEmpty(attribute))
          {
            if (attribute.StartsWithExt("../"))
            {
              PartContainer xmlPartContainer = srcPackage.GetXmlPartContainer(this, attribute);
              string partName = attribute.Substring(attribute.LastIndexOf('/') + 1);
              PartContainer newContainer1;
              if (newContainer.XmlPartContainers.ContainsKey("embeddings/"))
              {
                newContainer1 = newContainer.XmlPartContainers["embeddings/"];
              }
              else
              {
                newContainer1 = new PartContainer();
                newContainer1.Name = "embeddings/";
                newContainer.XmlPartContainers.Add(newContainer1.Name, newContainer1);
              }
              string str = xmlPartContainer.CopyXmlPartItems(newContainer1, srcPackage, partName);
              if (!string.IsNullOrEmpty(str))
                dictionary.Add(attribute, newContainer1.Name + str);
              else
                dictionary.Add(attribute, newContainer1.Name + partName);
            }
            else
            {
              string[] parts = attribute.Split('/');
              string newValue = this.CopyXmlPartContainer(newContainer, srcPackage, parts, 0);
              if (!string.IsNullOrEmpty(newValue))
                dictionary.Add(attribute, attribute.Replace(parts[parts.Length - 1], newValue));
            }
          }
        }
        flag = false;
      }
      while (reader.LocalName != "Relationships");
    }
    return dictionary;
  }

  internal PartContainer GetXmlPartContainer(PartContainer srcContainer, string target)
  {
    return this.EnsurePartContainer(this.GetXmlPartContainerPath(srcContainer, target).Split('/'), 0);
  }

  internal string GetXmlPartContainerPath(PartContainer container, string target)
  {
    string[] strArray = target.Split('/');
    int num = 0;
    for (int index = 0; index < strArray.Length; ++index)
    {
      if (strArray[index].Trim('.') == "")
        ++num;
      else
        break;
    }
    string str1 = this.GetParentPartPath(container);
    for (; num > 1; --num)
    {
      string str2 = str1.TrimEnd('/');
      str1 = !str2.Contains("/") ? "" : str2.Remove(str2.LastIndexOf('/'));
    }
    return str1 + target.TrimStart('.', '/');
  }

  private string GetParentPartPath(PartContainer srcContainer)
  {
    string parentPartPath1 = "";
    if (this.m_xmlPartContainers == null)
      return parentPartPath1;
    if (this.m_xmlPartContainers.ContainsValue(srcContainer))
      return parentPartPath1 + this.Name;
    foreach (KeyValuePair<string, PartContainer> xmlPartContainer in this.m_xmlPartContainers)
    {
      if (xmlPartContainer.Value.m_xmlPartContainers != null && xmlPartContainer.Value.m_xmlPartContainers.Count != 0)
      {
        string parentPartPath2 = xmlPartContainer.Value.GetParentPartPath(srcContainer);
        if (!string.IsNullOrEmpty(parentPartPath2))
        {
          parentPartPath1 = parentPartPath1 + this.Name + parentPartPath2;
          break;
        }
      }
    }
    return parentPartPath1;
  }

  internal virtual void Close()
  {
    if (this.m_xmlParts != null)
    {
      foreach (Part part in this.m_xmlParts.Values)
        part.Close();
      this.m_xmlParts.Clear();
      this.m_xmlParts = (Dictionary<string, Part>) null;
    }
    if (this.m_xmlPartContainers != null)
    {
      foreach (PartContainer partContainer in this.m_xmlPartContainers.Values)
        partContainer.Close();
      this.m_xmlPartContainers.Clear();
      this.m_xmlPartContainers = (Dictionary<string, PartContainer>) null;
    }
    if (this.m_relations == null)
      return;
    foreach (Syncfusion.DocIO.DLS.Relations relations in this.m_relations.Values)
      relations.Close();
    this.m_relations.Clear();
    this.m_relations = (Dictionary<string, Syncfusion.DocIO.DLS.Relations>) null;
  }
}
