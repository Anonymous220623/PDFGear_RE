// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.GradientSerializator
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Implementation.XmlSerialization.Charts;
using System;
using System.Drawing;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization;

internal class GradientSerializator
{
  public void Serialize(XmlWriter writer, GradientStops gradientStops, IWorkbook book)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (gradientStops == null)
      throw new ArgumentNullException(nameof (gradientStops));
    writer.WriteStartElement("gradFill", "http://schemas.openxmlformats.org/drawingml/2006/main");
    this.SerializeGradientStops(writer, gradientStops, book);
    writer.WriteEndElement();
  }

  private void SerializeGradientStops(
    XmlWriter writer,
    GradientStops gradientStops,
    IWorkbook book)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (gradientStops == null)
      throw new ArgumentNullException(nameof (gradientStops));
    writer.WriteStartElement("gsLst", "http://schemas.openxmlformats.org/drawingml/2006/main");
    int index = 0;
    for (int count = gradientStops.Count; index < count; ++index)
      this.SerializeGradientStop(writer, gradientStops[index], book);
    writer.WriteEndElement();
    if (gradientStops.GradientType == GradientType.Liniar)
    {
      writer.WriteStartElement("lin", "http://schemas.openxmlformats.org/drawingml/2006/main");
      writer.WriteAttributeString("ang", gradientStops.Angle.ToString());
      writer.WriteAttributeString("scaled", "1");
      writer.WriteEndElement();
    }
    else
    {
      writer.WriteStartElement("path", "http://schemas.openxmlformats.org/drawingml/2006/main");
      string lower = gradientStops.GradientType.ToString().ToLower();
      writer.WriteAttributeString("path", lower);
      Rectangle fillToRect = gradientStops.FillToRect;
      if (fillToRect.Left != 0 || fillToRect.Top != 0 || fillToRect.Right != 0 || fillToRect.Bottom != 0)
      {
        writer.WriteStartElement("fillToRect", "http://schemas.openxmlformats.org/drawingml/2006/main");
        int left = fillToRect.Left;
        if (left != 0)
          writer.WriteAttributeString("l", left.ToString());
        int top = fillToRect.Top;
        if (top != 0)
          writer.WriteAttributeString("t", top.ToString());
        if (fillToRect.Right != 0)
          writer.WriteAttributeString("r", fillToRect.Right.ToString());
        if (fillToRect.Bottom != 0)
          writer.WriteAttributeString("b", fillToRect.Bottom.ToString());
        writer.WriteEndElement();
      }
      writer.WriteEndElement();
    }
    Rectangle tileRect = gradientStops.TileRect;
    if (tileRect.Left == 0 && tileRect.Top == 0 && tileRect.Right == 0 && tileRect.Bottom == 0)
      return;
    writer.WriteStartElement("tileRect", "http://schemas.openxmlformats.org/drawingml/2006/main");
    int right = tileRect.Right;
    if (right != 0)
      writer.WriteAttributeString("r", right.ToString());
    int bottom = tileRect.Bottom;
    if (bottom != 0)
      writer.WriteAttributeString("b", bottom.ToString());
    int left1 = tileRect.Left;
    if (left1 != 0)
      writer.WriteAttributeString("l", left1.ToString());
    int top1 = tileRect.Top;
    if (top1 != 0)
      writer.WriteAttributeString("t", top1.ToString());
    writer.WriteEndElement();
  }

  private void SerializeGradientStop(
    XmlWriter writer,
    GradientStopImpl gradientStop,
    IWorkbook book)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (gradientStop == null)
      throw new ArgumentNullException(nameof (gradientStop));
    writer.WriteStartElement("gs", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("pos", gradientStop.Position.ToString());
    if (!gradientStop.ColorObject.IsSchemeColor)
      ChartSerializatorCommon.SerializeRgbColor(writer, gradientStop.ColorObject.GetRGB(book), gradientStop.Transparency, gradientStop.Tint, gradientStop.Shade);
    else
      this.SerializeSchemeColor(writer, gradientStop, book);
    writer.WriteEndElement();
  }

  private void SerializeSchemeColor(XmlWriter writer, GradientStopImpl gradienstop, IWorkbook book)
  {
    writer.WriteStartElement("schemeClr", "http://schemas.openxmlformats.org/drawingml/2006/main");
    writer.WriteAttributeString("val", gradienstop.ColorObject.SchemaName);
    ColorObject colorObject = gradienstop.ColorObject;
    if (colorObject.Tint > 0.0)
      ChartSerializatorCommon.SerializeDoubleValueTag(writer, "tint", "http://schemas.openxmlformats.org/drawingml/2006/main", colorObject.Tint);
    if (colorObject.Saturation > 0.0)
      ChartSerializatorCommon.SerializeDoubleValueTag(writer, "satMod", "http://schemas.openxmlformats.org/drawingml/2006/main", colorObject.Saturation);
    if (colorObject.Luminance > 0.0)
      ChartSerializatorCommon.SerializeDoubleValueTag(writer, "lumMod", "http://schemas.openxmlformats.org/drawingml/2006/main", colorObject.Luminance);
    if (colorObject.LuminanceOffSet > 0.0)
      ChartSerializatorCommon.SerializeDoubleValueTag(writer, "lumOff", "http://schemas.openxmlformats.org/drawingml/2006/main", colorObject.LuminanceOffSet);
    if (gradienstop.Transparency > 0)
      ChartSerializatorCommon.SerializeDoubleValueTag(writer, "alpha", "http://schemas.openxmlformats.org/drawingml/2006/main", (double) gradienstop.Transparency);
    if (gradienstop.Shade > 0)
      ChartSerializatorCommon.SerializeDoubleValueTag(writer, "shade", "http://schemas.openxmlformats.org/drawingml/2006/main", (double) gradienstop.Shade);
    writer.WriteEndElement();
  }
}
