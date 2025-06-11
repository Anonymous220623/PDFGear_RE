// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlReaders.Shapes.ShapeParser
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.Drawing;
using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Implementation.XmlSerialization;
using Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlReaders.Shapes;

public abstract class ShapeParser
{
  public abstract ShapeImpl ParseShapeType(XmlReader reader, ShapeCollectionBase shapes);

  public abstract bool ParseShape(
    XmlReader reader,
    ShapeImpl defaultShape,
    RelationCollection relations,
    string parentItemPath);

  public static Stream ReadNodeAsStream(XmlReader reader)
  {
    return ShapeParser.ReadNodeAsStream(reader, false);
  }

  public static Stream ReadNodeAsStream(XmlReader reader, bool writeNamespaces)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    MemoryStream data = new MemoryStream();
    XmlWriter writer = UtilityMethods.CreateWriter((Stream) data, Encoding.UTF8);
    writer.WriteNode(reader, writeNamespaces);
    writer.Flush();
    return (Stream) data;
  }

  public static void WriteNodeFromStream(XmlWriter writer, Stream stream)
  {
    ShapeParser.WriteNodeFromStream(writer, stream, false);
  }

  public static void WriteNodeFromStream(XmlWriter writer, Stream stream, bool writeNamespaces)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    XmlReader reader = stream != null ? UtilityMethods.CreateReader(stream) : throw new ArgumentNullException(nameof (stream));
    writer.WriteNode(reader, writeNamespaces);
    writer.Flush();
  }

  protected void ParseAnchor(XmlReader reader, ShapeImpl shape)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (shape == null)
      throw new ArgumentNullException(nameof (shape));
    string[] strArray = reader.ReadElementContentAsString().Split(',');
    if (strArray.Length != 8)
      throw new XmlException("Wrong anchor format");
    int index = 0;
    for (int length = strArray.Length; index < length; ++index)
      strArray[index] = strArray[index].Trim();
    MsofbtClientAnchor clientAnchor = shape.ClientAnchor;
    int num1 = int.Parse(strArray[0]);
    int num2 = int.Parse(strArray[1]);
    int num3;
    clientAnchor.LeftColumn = num3 = Math.Abs(num1);
    clientAnchor.LeftOffset = shape.PixelsInOffset(num3 + 1, Math.Abs(num2), true);
    shape.CheckLeftOffset();
    int num4 = int.Parse(strArray[2]);
    int num5 = int.Parse(strArray[3]);
    int num6;
    clientAnchor.TopRow = num6 = Math.Abs(num4);
    clientAnchor.TopOffset = shape.PixelsInOffset(num6 + 1, Math.Abs(num5), false);
    int num7 = int.Parse(strArray[4]);
    int num8 = int.Parse(strArray[5]);
    int num9;
    clientAnchor.RightColumn = num9 = Math.Abs(num7);
    clientAnchor.RightOffset = shape.PixelsInOffset(num9 + 1, Math.Abs(num8), true);
    int num10 = int.Parse(strArray[6]);
    int num11 = int.Parse(strArray[7]);
    int num12;
    clientAnchor.BottomRow = num12 = Math.Abs(num10);
    clientAnchor.BottomOffset = shape.PixelsInOffset(num12 + 1, Math.Abs(num11), false);
    shape.EvaluateTopLeftPosition();
    shape.UpdateHeight();
    shape.UpdateWidth();
  }

  protected Dictionary<string, string> SplitStyle(string styleValue)
  {
    Dictionary<string, string> dictionary = new Dictionary<string, string>();
    if (styleValue != null)
    {
      string[] strArray = styleValue.Split(';');
      int index = 0;
      for (int length1 = strArray.Length; index < length1; ++index)
      {
        string str1 = strArray[index];
        int length2 = str1.IndexOf(':');
        if (length2 >= 0)
        {
          string key = str1.Substring(0, length2).Trim();
          string str2 = str1.Substring(length2 + 1, str1.Length - length2 - 1).Trim();
          if (!dictionary.ContainsKey(key))
            dictionary.Add(key, str2);
        }
      }
    }
    return dictionary;
  }

  internal abstract ShapeImpl CreateShape(ShapeCollectionBase shapes);

  internal static void ParsePath2D(XmlReader reader, List<Path2D> pathList)
  {
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (reader.LocalName == "pathLst")
    {
      localName = reader.LocalName;
      reader.Read();
    }
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        if (reader.LocalName == "path")
        {
          Path2D path = new Path2D();
          if (reader.MoveToAttribute("w"))
            path.Width = Helper.ToDouble(reader.Value);
          if (reader.MoveToAttribute("h"))
            path.Height = Helper.ToDouble(reader.Value);
          if (reader.MoveToAttribute("stroke"))
            path.IsStroke = reader.Value == "1";
          reader.MoveToElement();
          ShapeParser.Parse2DElements(reader, path);
          pathList.Add(path);
        }
        reader.Skip();
      }
      else
        reader.Skip();
    }
  }

  private static void Parse2DElements(XmlReader reader, Path2D path)
  {
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "close":
            path.PathElements.Add(1.0);
            path.PathElements.Add(0.0);
            reader.Skip();
            continue;
          case "moveTo":
            path.PathElements.Add(2.0);
            path.PathElements.Add(1.0);
            ShapeParser.ParsePath2DPoint(reader, path.PathElements);
            reader.Skip();
            continue;
          case "lnTo":
            path.PathElements.Add(3.0);
            path.PathElements.Add(1.0);
            ShapeParser.ParsePath2DPoint(reader, path.PathElements);
            reader.Skip();
            continue;
          case "quadBezTo":
            path.PathElements.Add(5.0);
            path.PathElements.Add(2.0);
            ShapeParser.ParsePath2DPoint(reader, path.PathElements);
            reader.Skip();
            continue;
          case "cubicBezTo":
            path.PathElements.Add(6.0);
            path.PathElements.Add(3.0);
            ShapeParser.ParsePath2DPoint(reader, path.PathElements);
            reader.Skip();
            continue;
          case "arcTo":
            path.PathElements.Add(4.0);
            path.PathElements.Add(4.0);
            if (reader.MoveToAttribute("wR"))
              path.PathElements.Add(Helper.ToDouble(reader.Value));
            if (reader.MoveToAttribute("hR"))
              path.PathElements.Add(Helper.ToDouble(reader.Value));
            if (reader.MoveToAttribute("stAng"))
              path.PathElements.Add(Helper.ToDouble(reader.Value));
            if (reader.MoveToAttribute("swAng"))
              path.PathElements.Add(Helper.ToDouble(reader.Value));
            reader.Skip();
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Skip();
    }
  }

  private static void ParsePath2DPoint(XmlReader reader, List<double> pathElements)
  {
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        if (!(reader.LocalName == "pt"))
          break;
        if (reader.MoveToAttribute("x"))
          pathElements.Add(Helper.ToDouble(reader.Value));
        if (reader.MoveToAttribute("y"))
          pathElements.Add(Helper.ToDouble(reader.Value));
        reader.Skip();
      }
      else
        reader.Skip();
    }
  }
}
