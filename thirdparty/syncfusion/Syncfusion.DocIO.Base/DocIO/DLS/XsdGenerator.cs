// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.XsdGenerator
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class XsdGenerator
{
  protected const string DEF_DLS_RESOURCES = "Syncfusion.DocIO.DLS.Resources";
  private const string DEF_SCHEME_NS = "http://www.w3.org/2001/XMLSchema";
  private const string DEF_META_NS = "http://tempuri.org/DLSMetaSchema.xsd";
  private readonly string[] DEF_STANDARD_TYPES = new string[6]
  {
    "string",
    "float",
    "boolean",
    "int",
    "datetime",
    "base64Binary"
  };
  private XmlSchema m_schema;
  private XmlNamespaceManager m_mngr;
  private XmlElement m_metaElement;

  public static XmlSchema GetDLSLocalSchema()
  {
    return XmlSchema.Read(XsdGenerator.GetDLSResourceStream("dls-schema.xsd"), new ValidationEventHandler(XsdGenerator.OnValidation));
  }

  public XmlSchema GenerateDLSSchema()
  {
    return this.GenerateSchema(XsdGenerator.LoadXmlDocument(XsdGenerator.GetDLSResourceStream("dls-meta-schema.xml")));
  }

  public XmlSchema GenerateSchema(XmlDocument metaSchema)
  {
    this.m_mngr = new XmlNamespaceManager(metaSchema.NameTable);
    this.m_mngr.AddNamespace("m", "http://tempuri.org/DLSMetaSchema.xsd");
    this.m_metaElement = metaSchema.DocumentElement;
    foreach (XmlNode selectNode in this.m_metaElement.SelectNodes("m:include", this.m_mngr))
    {
      XmlDocument includeSchema = XsdGenerator.LoadXmlDocument(this.GetResourceStream(selectNode.Attributes["name"].Value, selectNode.Attributes["namespace"].Value));
      this.MergeWithInclude(metaSchema, includeSchema);
    }
    this.m_schema = new XmlSchema();
    XmlElement xmlElement = this.m_metaElement["m:root"];
    this.m_schema.Items.Add((XmlSchemaObject) new XmlSchemaElement()
    {
      Name = xmlElement.Attributes["name"].Value,
      SchemaTypeName = new XmlQualifiedName(xmlElement.Attributes["type"].Value)
    });
    foreach (XmlNode selectNode in this.m_metaElement.SelectNodes("m:type", this.m_mngr))
      this.ParseType(selectNode);
    return this.m_schema;
  }

  protected virtual Stream GetResourceStream(string resName, string resNamespace)
  {
    return resNamespace == "Syncfusion.DocIO.DLS" ? XsdGenerator.GetDLSResourceStream(resName) : (Stream) null;
  }

  protected static Stream GetDLSResourceStream(string resName)
  {
    return Assembly.GetExecutingAssembly().GetManifestResourceStream("Syncfusion.DocIO.DLS.Resources." + resName);
  }

  protected static XmlDocument LoadXmlDocument(Stream stream)
  {
    string end = new StreamReader(stream).ReadToEnd();
    XmlDocument xmlDocument = new XmlDocument();
    xmlDocument.LoadXml(end);
    return xmlDocument;
  }

  protected static void OnValidation(object sender, ValidationEventArgs args)
  {
    throw new DLSException(args.Message);
  }

  private void ParseType(XmlNode typeNode)
  {
    XmlAttribute attribute = typeNode.Attributes["mode"];
    if (attribute != null)
    {
      switch (attribute.Value)
      {
        case "choice":
          this.ParseComplexType((XmlSchemaGroupBase) new XmlSchemaChoice(), typeNode, false);
          break;
        case "grouping":
          this.ParseComplexType((XmlSchemaGroupBase) new XmlSchemaSequence(), typeNode, true);
          break;
        case "enum":
          this.ParseSimpleType(typeNode, ModeType.Enum);
          break;
        case "pattern":
          this.ParseSimpleType(typeNode, ModeType.Pattern);
          break;
        case "space":
          this.ParseSimpleType(typeNode, ModeType.Space);
          break;
      }
    }
    else
      this.ParseComplexType((XmlSchemaGroupBase) new XmlSchemaChoice(), typeNode, false);
  }

  private void ParseSimpleType(XmlNode typeNode, ModeType mode)
  {
    XmlSchemaSimpleType schemaSimpleType = new XmlSchemaSimpleType();
    schemaSimpleType.Name = typeNode.Attributes["name"].Value;
    XmlSchemaSimpleTypeRestriction simpleTypeRestriction = new XmlSchemaSimpleTypeRestriction();
    simpleTypeRestriction.BaseTypeName = new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema");
    schemaSimpleType.Content = (XmlSchemaSimpleTypeContent) simpleTypeRestriction;
    switch (mode)
    {
      case ModeType.Enum:
        IEnumerator enumerator = typeNode.SelectNodes("m:enum", this.m_mngr).GetEnumerator();
        try
        {
          while (enumerator.MoveNext())
          {
            XmlNode current = (XmlNode) enumerator.Current;
            XmlSchemaEnumerationFacet enumerationFacet = new XmlSchemaEnumerationFacet();
            enumerationFacet.Value = current.Attributes["value"].Value;
            simpleTypeRestriction.Facets.Add((XmlSchemaObject) enumerationFacet);
          }
          break;
        }
        finally
        {
          if (enumerator is IDisposable disposable)
            disposable.Dispose();
        }
      case ModeType.Pattern:
        XmlNode xmlNode1 = typeNode.SelectSingleNode("m:pattern", this.m_mngr);
        XmlSchemaPatternFacet schemaPatternFacet = new XmlSchemaPatternFacet();
        schemaPatternFacet.Value = xmlNode1.Attributes["value"].Value;
        simpleTypeRestriction.Facets.Add((XmlSchemaObject) schemaPatternFacet);
        break;
      case ModeType.Space:
        XmlNode xmlNode2 = typeNode.SelectSingleNode("m:whitespace", this.m_mngr);
        XmlSchemaWhiteSpaceFacet schemaWhiteSpaceFacet = new XmlSchemaWhiteSpaceFacet();
        schemaWhiteSpaceFacet.Value = xmlNode2.Attributes["value"].Value;
        simpleTypeRestriction.Facets.Add((XmlSchemaObject) schemaWhiteSpaceFacet);
        break;
    }
    this.m_schema.Items.Add((XmlSchemaObject) schemaSimpleType);
  }

  private void ParseComplexType(XmlSchemaGroupBase group, XmlNode typeNode, bool isGrouping)
  {
    XmlSchemaComplexType schemaComplexType = new XmlSchemaComplexType();
    schemaComplexType.Name = typeNode.Attributes["name"].Value;
    if (isGrouping)
    {
      string name = schemaComplexType.Name + "Group";
      schemaComplexType.Particle = (XmlSchemaParticle) new XmlSchemaGroupRef()
      {
        RefName = new XmlQualifiedName(name)
      };
      this.m_schema.Items.Add((XmlSchemaObject) new XmlSchemaGroup()
      {
        Name = name,
        Particle = group
      });
    }
    else
    {
      schemaComplexType.Particle = (XmlSchemaParticle) group;
      schemaComplexType.Particle.MaxOccursString = "unbounded";
      schemaComplexType.Particle.MinOccurs = 0M;
    }
    this.m_schema.Items.Add((XmlSchemaObject) schemaComplexType);
    foreach (XmlNode selectNode in typeNode.SelectNodes("m:element", this.m_mngr))
      this.ParseElement(selectNode, group);
    foreach (XmlNode selectNode in typeNode.SelectNodes("m:group", this.m_mngr))
    {
      XmlAttribute attribute = selectNode.Attributes["ref"];
      if (attribute != null)
        this.ParseGroup(this.m_metaElement.SelectSingleNode($"m:group[@name='{attribute.Value}']", this.m_mngr), group);
      else
        this.ParseGroup(selectNode, group);
    }
    XmlSchemaObjectCollection attributes = schemaComplexType.Attributes;
    if (isGrouping)
    {
      string name = schemaComplexType.Name + "AttrGroup";
      schemaComplexType.Attributes.Add((XmlSchemaObject) new XmlSchemaAttributeGroupRef()
      {
        RefName = new XmlQualifiedName(name)
      });
      XmlSchemaAttributeGroup schemaAttributeGroup = new XmlSchemaAttributeGroup();
      schemaAttributeGroup.Name = name;
      this.m_schema.Items.Add((XmlSchemaObject) schemaAttributeGroup);
      attributes = schemaAttributeGroup.Attributes;
    }
    foreach (XmlNode selectNode in typeNode.SelectNodes("m:attribute", this.m_mngr))
      this.ParseAttribute(selectNode, attributes);
    schemaComplexType.Attributes.Add((XmlSchemaObject) new XmlSchemaAttribute()
    {
      Name = "id",
      SchemaTypeName = new XmlQualifiedName("int", "http://www.w3.org/2001/XMLSchema")
    });
  }

  private void ParseElement(XmlNode elementNode, XmlSchemaGroupBase group)
  {
    XmlSchemaElement xmlSchemaElement = new XmlSchemaElement();
    xmlSchemaElement.Name = elementNode.Attributes["name"].Value;
    XmlAttribute attribute = elementNode.Attributes["type"];
    xmlSchemaElement.SchemaTypeName = !this.DEF_STANDARD_TYPES.Contains((object) attribute.Value) ? new XmlQualifiedName(attribute.Value) : new XmlQualifiedName(attribute.Value, "http://www.w3.org/2001/XMLSchema");
    group.Items.Add((XmlSchemaObject) xmlSchemaElement);
  }

  private void ParseGroup(XmlNode groupNode, XmlSchemaGroupBase group)
  {
    XmlSchemaElement xmlSchemaElement = new XmlSchemaElement();
    xmlSchemaElement.Name = groupNode.Attributes["name"].Value;
    group.Items.Add((XmlSchemaObject) xmlSchemaElement);
    XmlSchemaComplexType schemaComplexType = new XmlSchemaComplexType();
    xmlSchemaElement.SchemaType = (XmlSchemaType) schemaComplexType;
    XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
    schemaComplexType.Particle = (XmlSchemaParticle) xmlSchemaSequence;
    schemaComplexType.Particle.MaxOccursString = "unbounded";
    schemaComplexType.Particle.MinOccurs = 0M;
    xmlSchemaSequence.Items.Add((XmlSchemaObject) new XmlSchemaElement()
    {
      Name = groupNode.Attributes["item"].Value,
      SchemaTypeName = new XmlQualifiedName(groupNode.Attributes["type"].Value)
    });
  }

  private void ParseAttribute(XmlNode attrNode, XmlSchemaObjectCollection attributes)
  {
    XmlSchemaAttribute xmlSchemaAttribute = new XmlSchemaAttribute();
    xmlSchemaAttribute.Name = attrNode.Attributes["name"].Value;
    XmlAttribute attribute1 = attrNode.Attributes["type"];
    XmlAttribute attribute2 = attrNode.Attributes["fixed"];
    xmlSchemaAttribute.SchemaTypeName = !this.DEF_STANDARD_TYPES.Contains((object) attribute1.Value) ? new XmlQualifiedName(attribute1.Value) : new XmlQualifiedName(attribute1.Value, "http://www.w3.org/2001/XMLSchema");
    if (attribute2 != null)
      xmlSchemaAttribute.FixedValue = attribute2.Value;
    attributes.Add((XmlSchemaObject) xmlSchemaAttribute);
  }

  private void MergeWithInclude(XmlDocument metaSchema, XmlDocument includeSchema)
  {
    this.MergeWithInclude(includeSchema, metaSchema, "m:type");
    this.MergeWithInclude(includeSchema, metaSchema, "m:group");
  }

  private void MergeWithInclude(XmlDocument includeSchema, XmlDocument metaSchema, string tagsName)
  {
    foreach (XmlNode selectNode in includeSchema.DocumentElement.SelectNodes(tagsName, this.m_mngr))
    {
      string str = selectNode.Attributes["name"].Value;
      XmlNode xmlNode = metaSchema.DocumentElement.SelectSingleNode($"{tagsName}[@name='{str}']", this.m_mngr);
      if (xmlNode == null)
      {
        XmlNode node = metaSchema.CreateNode(XmlNodeType.Element, "temp", string.Empty);
        node.InnerXml = selectNode.OuterXml;
        metaSchema.DocumentElement.AppendChild(node.FirstChild);
      }
      else
      {
        string innerXml = xmlNode.InnerXml;
        xmlNode.InnerXml = innerXml + selectNode.InnerXml;
      }
    }
  }
}
