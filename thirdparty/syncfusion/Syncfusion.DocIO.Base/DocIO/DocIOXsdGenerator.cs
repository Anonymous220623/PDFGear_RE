// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DocIOXsdGenerator
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using System.IO;
using System.Reflection;
using System.Xml.Schema;

#nullable disable
namespace Syncfusion.DocIO;

public class DocIOXsdGenerator : XsdGenerator
{
  protected const string DEF_DOCIO_RESOURCES = "Syncfusion.DocIO.Resources";

  public static XmlSchema GetDocIOLocalSchema()
  {
    return XmlSchema.Read(DocIOXsdGenerator.GetDocIOResourceStream("docio-schema.xsd"), new ValidationEventHandler(XsdGenerator.OnValidation));
  }

  public XmlSchema GenerateDocIOSchema()
  {
    return this.GenerateSchema(XsdGenerator.LoadXmlDocument(DocIOXsdGenerator.GetDocIOResourceStream("docio-meta-schema.xml")));
  }

  protected static Stream GetDocIOResourceStream(string resName)
  {
    return Assembly.GetExecutingAssembly().GetManifestResourceStream("Syncfusion.DocIO.Resources." + resName);
  }

  protected override Stream GetResourceStream(string resName, string resNamespace)
  {
    return resNamespace == "Syncfusion.DocIO" ? DocIOXsdGenerator.GetDocIOResourceStream(resName) : base.GetResourceStream(resName, resNamespace);
  }
}
