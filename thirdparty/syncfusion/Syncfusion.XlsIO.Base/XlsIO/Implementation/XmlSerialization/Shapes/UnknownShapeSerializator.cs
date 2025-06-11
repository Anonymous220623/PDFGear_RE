// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.Shapes.UnknownShapeSerializator
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Shapes;
using System;
using System.IO;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization.Shapes;

internal class UnknownShapeSerializator : ShapeSerializator
{
  private Stream m_shapeTypeStream;

  public UnknownShapeSerializator(Stream shapeTypeStream)
  {
    this.m_shapeTypeStream = shapeTypeStream != null && shapeTypeStream.Length != 0L ? shapeTypeStream : throw new ArgumentOutOfRangeException(nameof (shapeTypeStream));
  }

  public override void Serialize(
    XmlWriter writer,
    ShapeImpl shape,
    WorksheetDataHolder holder,
    RelationCollection vmlRelations)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (shape == null)
      throw new ArgumentNullException(nameof (shape));
    if (holder == null)
      throw new ArgumentNullException(nameof (holder));
    Stream xmlDataStream = shape.XmlDataStream;
    xmlDataStream.Position = 0L;
    XmlReader reader = UtilityMethods.CreateReader(xmlDataStream);
    writer.WriteNode(reader, false);
    writer.Flush();
  }

  public override void SerializeShapeType(XmlWriter writer, Type shapeType)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    this.m_shapeTypeStream.Position = 0L;
    XmlReader reader = UtilityMethods.CreateReader(this.m_shapeTypeStream);
    writer.WriteNode(reader, false);
    writer.Flush();
  }
}
