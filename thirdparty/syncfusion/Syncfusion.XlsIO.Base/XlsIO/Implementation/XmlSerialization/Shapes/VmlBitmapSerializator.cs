// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.Shapes.VmlBitmapSerializator
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Implementation.XmlReaders.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization.Shapes;

public class VmlBitmapSerializator : HFImageSerializator
{
  public override void Serialize(
    XmlWriter writer,
    ShapeImpl shape,
    WorksheetDataHolder holder,
    RelationCollection vmlRelations)
  {
    writer.WriteStartElement(nameof (shape), "urn:schemas-microsoft-com:vml");
    string str1 = '#'.ToString() + $"_x0000_t{shape.InnerSpRecord.Instance}";
    string str2 = $"_x0000_s{shape.ShapeId}";
    writer.WriteAttributeString("id", str2);
    writer.WriteAttributeString("type", str1);
    if (!string.IsNullOrEmpty(shape.AlternativeText))
      writer.WriteAttributeString("alt", shape.AlternativeText);
    List<string> styleProperties = new List<string>();
    this.PrepareStyleProperties(styleProperties, shape);
    VmlTextBoxBaseSerializator.SerializeStyle(writer, styleProperties);
    if (shape.HasFill)
      writer.WriteAttributeString("filled", "t");
    writer.WriteAttributeString("fillcolor", "window [65]");
    if (shape.HasBorder)
      writer.WriteAttributeString("stroked", "t");
    writer.WriteAttributeString("strokecolor", "windowText [64]");
    writer.WriteAttributeString("o", "insetmode", "urn:schemas-microsoft-com:vml", "auto");
    writer.WriteStartElement("v", "fill", (string) null);
    writer.WriteAttributeString("color2", "window [65]");
    writer.WriteEndElement();
    this.SerializeImageData(writer, shape, holder, string.Empty, true, vmlRelations);
    this.SerializeClientData(writer, shape, "Pict");
    writer.WriteEndElement();
  }

  protected override void SerializeClientDataAdditional(XmlWriter writer, ShapeImpl shape)
  {
    base.SerializeClientDataAdditional(writer, shape);
    if (shape.FormulaMacroStream != null)
      ShapeParser.WriteNodeFromStream(writer, shape.FormulaMacroStream);
    writer.WriteElementString("CF", "urn:schemas-microsoft-com:office:excel", "Pict");
    writer.WriteElementString("AutoPict", "urn:schemas-microsoft-com:office:excel", string.Empty);
    BitmapShapeImpl bitmapShapeImpl = shape as BitmapShapeImpl;
    if (bitmapShapeImpl.IsDDE)
      writer.WriteElementString("DDE", "urn:schemas-microsoft-com:office:excel", (string) null);
    if (!bitmapShapeImpl.IsCamera)
      return;
    writer.WriteElementString("Camera", "urn:schemas-microsoft-com:office:excel", (string) null);
  }

  private void PrepareStyleProperties(List<string> styleProperties, ShapeImpl shape)
  {
    styleProperties.Add("position:absolute");
    this.AddMeasurement(styleProperties, "margin-left", shape.Left);
    this.AddMeasurement(styleProperties, "margin-top", shape.Top);
    this.AddMeasurement(styleProperties, "width", shape.Width);
    this.AddMeasurement(styleProperties, "height", shape.Height);
  }

  private void AddMeasurement(List<string> styleProperties, string tagName, int size)
  {
    double num = Math.Round(ApplicationImpl.ConvertFromPixel((double) size, MeasureUnits.Point), 2);
    styleProperties.Add($"{tagName}:{num}pt");
  }

  protected override string SerializePicture(
    ShapeImpl shape,
    WorksheetDataHolder holder,
    bool useRawFormat,
    RelationCollection relations)
  {
    BitmapShapeImpl bitmapShapeImpl = shape != null ? shape as BitmapShapeImpl : throw new ArgumentNullException(nameof (shape));
    Image picture = bitmapShapeImpl.Picture;
    int blipId = (int) bitmapShapeImpl.BlipId;
    ImageFormat imageFormat = useRawFormat ? picture.RawFormat : ImageFormat.Png;
    FileDataHolder parentHolder = holder.ParentHolder;
    holder.ParentHolder.RegisterContentTypes(imageFormat);
    string imageItemName = parentHolder.GetImageItemName(blipId - 1);
    string relationId = relations.GenerateRelationId();
    relations[relationId] = new Relation('/'.ToString() + imageItemName, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image");
    return relationId;
  }
}
