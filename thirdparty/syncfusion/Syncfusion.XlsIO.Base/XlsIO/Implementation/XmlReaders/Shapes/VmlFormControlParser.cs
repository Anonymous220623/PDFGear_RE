// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlReaders.Shapes.VmlFormControlParser
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Implementation.XmlSerialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlReaders.Shapes;

internal class VmlFormControlParser : ShapeParser
{
  private Dictionary<string, Stream> m_dictShapeTypes = new Dictionary<string, Stream>();
  private static Dictionary<string, ShapeParser> m_dictShapeParser = new Dictionary<string, ShapeParser>();

  static VmlFormControlParser()
  {
    VmlFormControlParser.m_dictShapeParser.Add("Checkbox", (ShapeParser) new CheckBoxShapeParser());
    VmlFormControlParser.m_dictShapeParser.Add("Radio", (ShapeParser) new OptionButtonShapeParser());
    VmlFormControlParser.m_dictShapeParser.Add("Drop", (ShapeParser) new ComboBoxShapeParser());
  }

  public override ShapeImpl ParseShapeType(XmlReader reader, ShapeCollectionBase shapes)
  {
    string key = reader.MoveToAttribute("id") ? reader.Value : throw new XmlException();
    reader.MoveToElement();
    Stream stream = ShapeParser.ReadNodeAsStream(reader);
    this.m_dictShapeTypes[key] = stream;
    return new ShapeImpl(shapes.Application, (object) shapes)
    {
      XmlTypeStream = stream
    };
  }

  public override bool ParseShape(
    XmlReader reader,
    ShapeImpl defaultShape,
    RelationCollection relations,
    string parentItemPath)
  {
    Stream data = reader != null ? ShapeParser.ReadNodeAsStream(reader) : throw new ArgumentNullException(nameof (reader));
    data.Position = 0L;
    reader = UtilityMethods.CreateReader(data);
    reader.MoveToAttribute("type");
    string str = reader.Value;
    reader.MoveToElement();
    reader.Read();
    string key1 = (string) null;
    while (reader.NodeType != XmlNodeType.EndElement && key1 == null)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "ClientData":
            if (reader.MoveToAttribute("ObjectType"))
            {
              key1 = reader.Value;
              continue;
            }
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Skip();
    }
    bool shape = false;
    ShapeParser shapeParser;
    if (key1 != null && VmlFormControlParser.m_dictShapeParser.TryGetValue(key1, out shapeParser))
    {
      string key2 = UtilityMethods.RemoveFirstCharUnsafe(str);
      if (this.m_dictShapeTypes.ContainsKey(key2))
      {
        Stream dictShapeType = this.m_dictShapeTypes[key2];
        dictShapeType.Position = 0L;
        UtilityMethods.CreateReader(dictShapeType);
        defaultShape = shapeParser.ParseShapeType(reader, defaultShape.ParentShapes);
        defaultShape.XmlTypeStream = dictShapeType;
      }
      else
        defaultShape = shapeParser.ParseShapeType(reader, defaultShape.ParentShapes);
      data.Position = 0L;
      reader = UtilityMethods.CreateReader(data);
      shape = shapeParser.ParseShape(reader, defaultShape, relations, parentItemPath);
    }
    return shape;
  }

  internal override ShapeImpl CreateShape(ShapeCollectionBase shapes)
  {
    return new ShapeImpl(shapes.Application, (object) shapes)
    {
      VmlShape = true
    };
  }
}
