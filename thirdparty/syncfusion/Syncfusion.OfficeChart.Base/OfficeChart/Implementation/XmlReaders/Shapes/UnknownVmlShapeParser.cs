// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.XmlReaders.Shapes.UnknownVmlShapeParser
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Collections;
using Syncfusion.OfficeChart.Implementation.Shapes;
using Syncfusion.OfficeChart.Implementation.XmlSerialization;
using Syncfusion.OfficeChart.Implementation.XmlSerialization.Shapes;
using System;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.XmlReaders.Shapes;

internal class UnknownVmlShapeParser : ShapeParser
{
  public override ShapeImpl ParseShapeType(XmlReader reader, ShapeCollectionBase shapes)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (shapes == null)
      throw new ArgumentNullException(nameof (shapes));
    int shapeInstance = reader.MoveToAttribute("spt", "urn:schemas-microsoft-com:office:office") ? int.Parse(reader.Value) : throw new XmlException();
    reader.MoveToElement();
    this.AddNewSerializator(shapeInstance, reader, shapes);
    return new ShapeImpl(shapes.Application, (object) shapes);
  }

  public override bool ParseShape(
    XmlReader reader,
    ShapeImpl defaultShape,
    RelationCollection relations,
    string parentItemPath)
  {
    ShapeImpl shapeImpl = (ShapeImpl) defaultShape.Clone(defaultShape.Parent);
    MemoryStream data = new MemoryStream();
    XmlWriter writer = UtilityMethods.CreateWriter((Stream) data, Encoding.UTF8);
    writer.WriteNode(reader, false);
    writer.Flush();
    shapeImpl.XmlDataStream = (Stream) data;
    data.Position = 0L;
    reader = UtilityMethods.CreateReader((Stream) data);
    reader.Read();
    while (reader.NodeType != XmlNodeType.None)
    {
      if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "imagedata")
      {
        if (reader.MoveToAttribute("relid", "urn:schemas-microsoft-com:office:office"))
        {
          string id = reader.Value;
          shapeImpl.ImageRelation = (Relation) relations[id].Clone();
          shapeImpl.ImageRelationId = id;
          break;
        }
        break;
      }
      reader.Read();
    }
    data.Position = 0L;
    return true;
  }

  private void AddNewSerializator(int shapeInstance, XmlReader reader, ShapeCollectionBase shapes)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (shapes == null)
      throw new ArgumentNullException(nameof (shapes));
    MemoryStream memoryStream = new MemoryStream();
    XmlWriter writer = UtilityMethods.CreateWriter((Stream) memoryStream, Encoding.UTF8);
    writer.WriteNode(reader, false);
    writer.Flush();
    WorksheetBaseImpl worksheetBase = shapes.WorksheetBase;
    UnknownShapeSerializator shapeSerializator = new UnknownShapeSerializator((Stream) memoryStream);
    worksheetBase.DataHolder.ParentHolder.Serializator.VmlSerializators[shapeInstance] = (ShapeSerializator) shapeSerializator;
    worksheetBase.UnknownVmlShapes = true;
  }
}
