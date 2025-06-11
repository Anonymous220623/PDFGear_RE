// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.Shapes.VmlFormControlsSerializator
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization.Shapes;

internal class VmlFormControlsSerializator : ShapeSerializator
{
  private Dictionary<Type, ShapeSerializator> m_dictShapeSerializators = new Dictionary<Type, ShapeSerializator>();

  public VmlFormControlsSerializator()
  {
    this.m_dictShapeSerializators.Add(typeof (CheckBoxShapeImpl), (ShapeSerializator) new CheckBoxShapeSerializator());
    this.m_dictShapeSerializators.Add(typeof (ComboBoxShapeImpl), (ShapeSerializator) new ComboBoxShapeSerializator());
    this.m_dictShapeSerializators.Add(typeof (OptionButtonShapeImpl), (ShapeSerializator) new OptionButtonShapeSerializator());
  }

  internal void ClearAll()
  {
    this.m_dictShapeSerializators.Clear();
    this.m_dictShapeSerializators = (Dictionary<Type, ShapeSerializator>) null;
  }

  public override void Serialize(
    XmlWriter writer,
    ShapeImpl shape,
    WorksheetDataHolder holder,
    RelationCollection vmlRelations)
  {
    ShapeSerializator shapeSerializator;
    if (!this.m_dictShapeSerializators.TryGetValue(shape.GetType(), out shapeSerializator))
      shapeSerializator = (ShapeSerializator) new UnknownShapeSerializator((Stream) null);
    shapeSerializator.Serialize(writer, shape, holder, vmlRelations);
  }

  public override void SerializeShapeType(XmlWriter writer, Type shapeType)
  {
    ShapeSerializator shapeSerializator;
    if (!this.m_dictShapeSerializators.TryGetValue(shapeType, out shapeSerializator))
      shapeSerializator = (ShapeSerializator) new UnknownShapeSerializator((Stream) null);
    if (!(shapeType.Name != "ComboBoxShapeImpl"))
      return;
    shapeSerializator.SerializeShapeType(writer, shapeType);
  }
}
