// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.Shapes.HFImageSerializator
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization.Shapes;

public class HFImageSerializator : ShapeSerializator
{
  private Dictionary<int, string> m_pictureItems;
  private static string[] s_arrFormulas = new string[12]
  {
    "if lineDrawn pixelLineWidth 0",
    "sum @0 1 0",
    "sum 0 0 @1",
    "prod @2 1 2",
    "prod @3 21600 pixelWidth",
    "prod @3 21600 pixelHeight",
    "sum @0 0 1",
    "prod @6 1 2",
    "prod @7 21600 pixelWidth",
    "sum @8 21600 0",
    "prod @7 21600 pixelHeight",
    "sum @10 21600 0"
  };

  public HFImageSerializator() => this.m_pictureItems = new Dictionary<int, string>();

  internal new void Clear()
  {
    if (this.m_pictureItems == null)
      return;
    this.m_pictureItems.Clear();
    this.m_pictureItems = (Dictionary<int, string>) null;
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
    string str1 = '#'.ToString() + $"_x0000_t{shape.InnerSpRecord.Instance}";
    writer.WriteStartElement(nameof (shape), "urn:schemas-microsoft-com:vml");
    writer.WriteAttributeString("id", shape.Name);
    writer.WriteAttributeString("type", str1);
    BitmapShapeImpl bitmap = (BitmapShapeImpl) shape;
    PageSetupBaseImpl pageSetupBase = bitmap.Worksheet.PageSetupBase;
    int width = this.GetWidth(bitmap);
    int height = this.GetHeight(bitmap);
    string str2 = bitmap.PreserveStyleString == null || bitmap.PreserveStyleString.Length <= 0 ? string.Format((IFormatProvider) CultureInfo.InvariantCulture.NumberFormat, "width:{0}pt;height:{1}pt", (object) ApplicationImpl.ConvertFromPixel((double) width, MeasureUnits.Point), (object) ApplicationImpl.ConvertFromPixel((double) height, MeasureUnits.Point)) : bitmap.PreserveStyleString;
    writer.WriteAttributeString("style", str2);
    this.SerializeImageData(writer, shape, holder, (string) null, false, holder.HFDrawingsRelations);
    writer.WriteEndElement();
  }

  protected virtual int GetWidth(BitmapShapeImpl bitmap) => bitmap.LeftColumn;

  protected virtual int GetHeight(BitmapShapeImpl bitmap) => bitmap.TopRow;

  public override void SerializeShapeType(XmlWriter writer, Type shapeType)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.WriteStartElement("shapetype", "urn:schemas-microsoft-com:vml");
    string str = $"_x0000_t{75}";
    writer.WriteAttributeString("id", str);
    writer.WriteAttributeString("coordsize", "21600,21600");
    writer.WriteAttributeString("spt", "urn:schemas-microsoft-com:office:office", 75.ToString());
    writer.WriteAttributeString("preferrelative", "urn:schemas-microsoft-com:office:office", "t");
    writer.WriteAttributeString("path", "m@4@5l@4@11@9@11@9@5xe");
    writer.WriteAttributeString("filled", "f");
    writer.WriteAttributeString("stroked", "f");
    writer.WriteStartElement("stroke", "urn:schemas-microsoft-com:vml");
    writer.WriteAttributeString("joinstyle", "miter");
    writer.WriteEndElement();
    writer.WriteStartElement("formulas", "urn:schemas-microsoft-com:vml");
    int index = 0;
    for (int length = HFImageSerializator.s_arrFormulas.Length; index < length; ++index)
    {
      writer.WriteStartElement("f", "urn:schemas-microsoft-com:vml");
      writer.WriteAttributeString("eqn", HFImageSerializator.s_arrFormulas[index]);
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
    writer.WriteStartElement("path", "urn:schemas-microsoft-com:vml");
    writer.WriteAttributeString("extrusionok", "urn:schemas-microsoft-com:office:office", "f");
    writer.WriteAttributeString("gradientshapeok", "t");
    writer.WriteAttributeString("connecttype", "urn:schemas-microsoft-com:office:office", "rect");
    writer.WriteEndElement();
    writer.WriteStartElement("lock", "urn:schemas-microsoft-com:office:office");
    writer.WriteAttributeString("ext", "urn:schemas-microsoft-com:vml", "edit");
    writer.WriteAttributeString("aspectratio", "t");
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  protected void SerializeImageData(
    XmlWriter writer,
    ShapeImpl shape,
    WorksheetDataHolder holder,
    string title,
    bool useRawFormat,
    RelationCollection relations)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (shape == null)
      throw new ArgumentNullException(nameof (shape));
    writer.WriteStartElement("imagedata", "urn:schemas-microsoft-com:vml");
    string str = this.SerializePicture(shape, holder, useRawFormat, relations);
    writer.WriteAttributeString("relid", "urn:schemas-microsoft-com:office:office", str);
    if (title != null)
      writer.WriteAttributeString(nameof (title), "urn:schemas-microsoft-com:office:office", title);
    writer.WriteEndElement();
  }

  protected virtual string SerializePicture(
    ShapeImpl shape,
    WorksheetDataHolder holder,
    bool useRawFormat,
    RelationCollection relations)
  {
    BitmapShapeImpl bitmapShapeImpl = shape != null ? shape as BitmapShapeImpl : throw new ArgumentNullException(nameof (shape));
    Image picture = bitmapShapeImpl.Picture;
    ImageFormat imageFormat = useRawFormat ? picture.RawFormat : ImageFormat.Png;
    FileDataHolder parentHolder = holder.ParentHolder;
    holder.ParentHolder.RegisterContentTypes(imageFormat);
    string empty = string.Empty;
    string str;
    if (!this.m_pictureItems.ContainsKey((int) bitmapShapeImpl.BlipId))
    {
      str = parentHolder.SaveImage(picture, imageFormat, (string) null);
      this.m_pictureItems.Add((int) bitmapShapeImpl.BlipId, str);
    }
    else
      str = this.m_pictureItems[(int) bitmapShapeImpl.BlipId];
    string relationId = relations.GenerateRelationId();
    relations[relationId] = new Relation('/'.ToString() + str, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image");
    return relationId;
  }

  private string SerializePicture(ShapeImpl shape, WorksheetDataHolder holder)
  {
    if (shape == null)
      throw new ArgumentNullException(nameof (shape));
    return this.SerializePicture(shape, holder, false, holder.HFDrawingsRelations);
  }
}
