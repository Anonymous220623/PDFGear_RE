// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.XmlParagraphItem
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.Convertors;
using Syncfusion.Layouting;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class XmlParagraphItem : ParagraphItem
{
  private Stream m_xmlNode;
  private Dictionary<string, DictionaryEntry> m_relations;
  internal string m_shapeHyperlink;
  private Dictionary<string, ImageRecord> m_imageRelations;
  private int m_zOrderPosition;
  private bool m_hasNestedImageRelations;
  internal ParagraphItemCollection MathParaItemsCollection;

  internal Dictionary<string, ImageRecord> ImageRelations
  {
    get
    {
      if (this.m_imageRelations == null)
        this.m_imageRelations = new Dictionary<string, ImageRecord>();
      return this.m_imageRelations;
    }
  }

  internal Dictionary<string, DictionaryEntry> Relations
  {
    get
    {
      if (this.m_relations == null)
        this.m_relations = new Dictionary<string, DictionaryEntry>();
      return this.m_relations;
    }
  }

  internal Stream DataNode
  {
    get => this.m_xmlNode;
    set => this.m_xmlNode = value;
  }

  internal WCharacterFormat CharacterFormat
  {
    get => this.m_charFormat;
    set => this.m_charFormat = value;
  }

  internal int ZOrderIndex
  {
    get => this.m_zOrderPosition;
    set => this.m_zOrderPosition = value;
  }

  public override EntityType EntityType => EntityType.XmlParaItem;

  internal bool HasNestedImageRelations
  {
    get => this.m_hasNestedImageRelations;
    set => this.m_hasNestedImageRelations = value;
  }

  public XmlParagraphItem(Stream xmlNode, IWordDocument wordDocument)
    : base(wordDocument as WordDocument)
  {
    this.m_xmlNode = xmlNode;
    this.m_charFormat = new WCharacterFormat(wordDocument);
    this.m_charFormat.SetOwner((OwnerHolder) this);
  }

  internal void ApplyCharacterFormat(WCharacterFormat charFormat)
  {
    if (charFormat == null)
      return;
    this.m_charFormat = charFormat.CloneInt() as WCharacterFormat;
  }

  protected override object CloneImpl()
  {
    XmlParagraphItem xmlParagraphItem = base.CloneImpl() as XmlParagraphItem;
    if (this.m_charFormat != null)
      xmlParagraphItem.CharacterFormat.ImportContainer((FormatBase) this.m_charFormat);
    if (xmlParagraphItem.m_xmlNode != null)
      xmlParagraphItem.m_xmlNode = UtilityMethods.CloneStream((Stream) (this.m_xmlNode as MemoryStream));
    xmlParagraphItem.m_relations = new Dictionary<string, DictionaryEntry>();
    if (this.m_relations != null)
    {
      foreach (string key in this.m_relations.Keys)
      {
        DictionaryEntry relation = this.m_relations[key];
        DictionaryEntry dictionaryEntry = new DictionaryEntry((object) (string) relation.Key, (object) (string) relation.Value);
        xmlParagraphItem.Relations.Add(key, dictionaryEntry);
      }
    }
    xmlParagraphItem.m_imageRelations = new Dictionary<string, ImageRecord>();
    if (this.m_imageRelations != null)
    {
      foreach (string key in this.m_imageRelations.Keys)
        xmlParagraphItem.ImageRelations.Add(key, this.m_imageRelations[key]);
    }
    return (object) xmlParagraphItem;
  }

  protected override void CreateLayoutInfo() => this.m_layoutInfo = (ILayoutInfo) new LayoutInfo();

  internal override void InitLayoutInfo(Entity entity, ref bool isLastTOCEntry)
  {
    this.m_layoutInfo = (ILayoutInfo) null;
    if (this != entity)
      return;
    isLastTOCEntry = true;
  }

  internal override void CloneRelationsTo(WordDocument doc, OwnerHolder nextOwner)
  {
    base.CloneRelationsTo(doc, nextOwner);
    if (this.ImageRelations.Count > 0)
    {
      string[] array = new string[this.ImageRelations.Count];
      this.ImageRelations.Keys.CopyTo(array, 0);
      for (int index = 0; index < array.Length; ++index)
      {
        ImageRecord imageRelation = this.ImageRelations[array[index]];
        ImageRecord imageRecord = !imageRelation.IsMetafile ? doc.Images.LoadImage(imageRelation.ImageBytes) : doc.Images.LoadMetaFileImage(imageRelation.m_imageBytes, true);
        this.ImageRelations[array[index]] = imageRecord;
      }
    }
    if (doc.DocxPackage == null && this.Document.DocxPackage != null)
    {
      doc.DocxPackage = this.Document.DocxPackage.Clone();
    }
    else
    {
      if (doc.DocxPackage == null || this.Document.DocxPackage == null)
        return;
      this.UpdateXmlParts(doc);
    }
  }

  internal override void AttachToParagraph(WParagraph owner, int itemPos)
  {
    base.AttachToParagraph(owner, itemPos);
    this.Document.FloatingItems.Add((Entity) this);
  }

  internal override void Detach()
  {
    base.Detach();
    this.Document.FloatingItems.Remove((Entity) this);
  }

  private void UpdateXmlParts(WordDocument destination)
  {
    if (this.Relations.Count == 0)
      return;
    string[] array = new string[this.Relations.Count];
    this.Relations.Keys.CopyTo(array, 0);
    for (int index = 0; index < array.Length; ++index)
    {
      DictionaryEntry relation = this.Relations[array[index]];
      if (relation.Key == null || !relation.Key.ToString().EndsWith("hyperlink"))
      {
        string[] parts = relation.Value.ToString().Split('/');
        string newValue = this.UpdateXmlPartContainer(this.Document.DocxPackage, this.Document.DocxPackage.FindPartContainer("word/"), destination.DocxPackage.FindPartContainer("word/"), parts, 0);
        if (newValue != string.Empty)
        {
          relation.Value = (object) relation.Value.ToString().Replace(parts[parts.Length - 1], newValue);
          this.Relations[array[index]] = relation;
        }
      }
    }
  }

  private string UpdateXmlPartContainer(
    Package srcPackage,
    PartContainer srcContainer,
    PartContainer destContainer,
    string[] parts,
    int index)
  {
    string str = string.Empty;
    for (int index1 = index; index1 < parts.Length; ++index1)
    {
      if (index1 < parts.Length - 1)
      {
        string key = parts[index1] + "/";
        if (destContainer.XmlPartContainers.ContainsKey(key))
        {
          destContainer = destContainer.XmlPartContainers[key];
          srcContainer = srcContainer.XmlPartContainers[key];
          str = this.UpdateXmlPartContainer(srcPackage, srcContainer, destContainer, parts, index1 + 1);
          break;
        }
        PartContainer newContainer = new PartContainer();
        newContainer.Name = key;
        str = srcContainer.XmlPartContainers[key].CopyXmlPartContainer(newContainer, srcPackage, parts, index1 + 1);
        destContainer.XmlPartContainers.Add(key, newContainer);
        break;
      }
      str = srcContainer.CopyXmlPartItems(destContainer, srcPackage, parts[index1]);
    }
    return str;
  }

  internal override void Close()
  {
    base.Close();
    if (this.m_xmlNode != null)
      this.m_xmlNode.Close();
    if (this.m_relations != null)
    {
      this.m_relations.Clear();
      this.m_relations = (Dictionary<string, DictionaryEntry>) null;
    }
    if (this.m_imageRelations == null)
      return;
    foreach (ImageRecord imageRecord in this.m_imageRelations.Values)
      imageRecord.Close();
    this.m_imageRelations.Clear();
    this.m_imageRelations = (Dictionary<string, ImageRecord>) null;
  }
}
