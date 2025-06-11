// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.XmlReaders.Shapes.ShapeParser
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Collections;
using Syncfusion.OfficeChart.Implementation.Shapes;
using Syncfusion.OfficeChart.Implementation.XmlSerialization;
using Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.XmlReaders.Shapes;

internal abstract class ShapeParser
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
    int iPixels1 = int.Parse(strArray[1]);
    clientAnchor.LeftColumn = num1;
    clientAnchor.LeftOffset = shape.PixelsInOffset(num1 + 1, iPixels1, true);
    shape.CheckLeftOffset();
    int num2 = int.Parse(strArray[2]);
    int iPixels2 = int.Parse(strArray[3]);
    clientAnchor.TopRow = num2;
    clientAnchor.TopOffset = shape.PixelsInOffset(num2 + 1, iPixels2, false);
    int num3 = int.Parse(strArray[4]);
    int iPixels3 = int.Parse(strArray[5]);
    clientAnchor.RightColumn = num3;
    clientAnchor.RightOffset = shape.PixelsInOffset(num3 + 1, iPixels3, true);
    int num4 = int.Parse(strArray[6]);
    int iPixels4 = int.Parse(strArray[7]);
    clientAnchor.BottomRow = num4;
    clientAnchor.BottomOffset = shape.PixelsInOffset(num4 + 1, iPixels4, false);
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
}
