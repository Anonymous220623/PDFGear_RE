// Decompiled with JetBrains decompiler
// Type: Syncfusion.Drawing.Serializator
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Shapes;
using System.Xml;

#nullable disable
namespace Syncfusion.Drawing;

internal class Serializator
{
  internal Serializator()
  {
  }

  internal void AddShape(ShapeImplExt shape, XmlWriter xmlTextwriter)
  {
    new AutoShapeSerializator(shape).Write(xmlTextwriter);
  }

  internal void AddShape(AutoShapeImpl shape, XmlWriter xmlTextwriter)
  {
    new AutoShapeSerializator(shape).Write(xmlTextwriter);
  }

  private void WriteHeader(XmlWriter xmlTextWriter)
  {
    xmlTextWriter.WriteStartDocument(true);
    xmlTextWriter.WriteStartElement("xdr:wsDr");
    xmlTextWriter.WriteAttributeString("xmlns", "xdr", (string) null, "http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing");
    xmlTextWriter.WriteAttributeString("xmlns", "a", (string) null, "http://schemas.openxmlformats.org/drawingml/2006/main");
    xmlTextWriter.WriteAttributeString("xmlns", "r", (string) null, "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
  }
}
