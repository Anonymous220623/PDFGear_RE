// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.Shapes.ShapeSerializator
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Implementation.XmlReaders.Shapes;
using Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization.Shapes;

public abstract class ShapeSerializator
{
  public const string FalseAttributeValue = "f";
  public const string TrueAttributeValue = "t";
  private static object m_lockObject = new object();

  public abstract void Serialize(
    XmlWriter writer,
    ShapeImpl shape,
    WorksheetDataHolder holder,
    RelationCollection relations);

  public abstract void SerializeShapeType(XmlWriter writer, Type shapeType);

  protected static string GetAnchorValue(ShapeImpl shape)
  {
    MsofbtClientAnchor clientAnchor = shape.ClientAnchor;
    int leftColumn = clientAnchor.LeftColumn;
    int num1 = shape.OffsetInPixels(leftColumn + 1, clientAnchor.LeftOffset, true);
    string str1 = leftColumn.ToString();
    string str2 = num1.ToString();
    int rightColumn = clientAnchor.RightColumn;
    int num2 = shape.OffsetInPixels(rightColumn + 1, clientAnchor.RightOffset, true);
    string str3 = rightColumn.ToString();
    string str4 = num2.ToString();
    int topRow = clientAnchor.TopRow;
    int num3 = shape.OffsetInPixels(topRow + 1, clientAnchor.TopOffset, false);
    string str5 = topRow.ToString();
    string str6 = num3.ToString();
    int bottomRow = clientAnchor.BottomRow;
    int num4 = shape.OffsetInPixels(bottomRow + 1, clientAnchor.BottomOffset, false);
    string str7 = bottomRow.ToString();
    string str8 = num4.ToString();
    return string.Join(", ", str1, str2, str5, str6, str3, str4, str7, str8);
  }

  protected void SerializeClientData(XmlWriter writer, ShapeImpl shape, string shapeType)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (shape == null)
      throw new ArgumentNullException(nameof (shape));
    writer.WriteStartElement("ClientData", "urn:schemas-microsoft-com:office:excel");
    if (shape.ValidComment)
      writer.WriteAttributeString("ObjectType", shapeType);
    else
      writer.WriteAttributeString("ObjectType", "Text");
    if (!shape.IsMoveWithCell)
      writer.WriteElementString("MoveWithCells", "urn:schemas-microsoft-com:office:excel", (string) null);
    if (!shape.IsSizeWithCell)
    {
      writer.WriteStartElement("SizeWithCells", "urn:schemas-microsoft-com:office:excel");
      writer.WriteEndElement();
    }
    string anchorValue = ShapeSerializator.GetAnchorValue(shape);
    writer.WriteElementString("Anchor", "urn:schemas-microsoft-com:office:excel", anchorValue);
    if (shape.ValidComment)
      this.SerializeClientDataAdditional(writer, shape);
    writer.WriteEndElement();
  }

  protected virtual void SerializeClientDataAdditional(XmlWriter writer, ShapeImpl shape)
  {
  }

  protected virtual void SerializeFill(
    XmlWriter writer,
    ShapeImpl shape,
    WorksheetDataHolder holder,
    RelationCollection vmlRelations)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (shape == null)
      throw new ArgumentNullException("textBox");
    if (!shape.HasFill)
      return;
    TextBoxShapeBase textBox = shape as TextBoxShapeBase;
    writer.WriteStartElement("fill", "urn:schemas-microsoft-com:vml");
    switch (textBox.Fill.FillType)
    {
      case ExcelFillType.SolidColor:
        this.SerializeSolidFill(writer, textBox);
        break;
      case ExcelFillType.Pattern:
        FileDataHolder parentHolder1 = holder.ParentHolder;
        writer.WriteAttributeString("type", "pattern");
        writer.WriteAttributeString("color", this.GenerateHexColor(textBox.Fill.BackColor));
        writer.WriteAttributeString("color2", this.GenerateHexColor(textBox.Fill.ForeColor));
        this.SerializePatternFill(writer, textBox, parentHolder1, vmlRelations);
        this.SerializeFillCommon(writer, textBox);
        break;
      case ExcelFillType.Texture:
        writer.WriteAttributeString("type", "tile");
        FileDataHolder parentHolder2 = holder.ParentHolder;
        this.SerializeTextureFill(writer, textBox, parentHolder2, vmlRelations);
        this.SerializeFillCommon(writer, textBox);
        break;
      case ExcelFillType.Picture:
        writer.WriteAttributeString("type", "frame");
        FileDataHolder parentHolder3 = holder.ParentHolder;
        this.SerializePictureFill(writer, textBox, parentHolder3, vmlRelations);
        break;
      case ExcelFillType.Gradient:
        this.SerializeGradientFill(writer, textBox);
        break;
    }
    writer.WriteEndElement();
  }

  protected virtual void SerializeSolidFill(XmlWriter writer, TextBoxShapeBase textBox)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (textBox == null)
      throw new ArgumentNullException(nameof (textBox));
    if (!ShapeSerializator.IsEmptyColor(textBox.Fill.ForeColor))
    {
      if (textBox.ColorObject == (ColorObject) null)
        return;
      string str = textBox.ColorObject.HexColor == null || !(textBox.Fill.ForeColor == ShapeFillImpl.DEF_COMENT_PARSE_COLOR) ? this.GenerateHexColor(textBox.Fill.ForeColor) : textBox.ColorObject.HexColor;
      writer.WriteAttributeString("color", str);
    }
    this.SerializeFillCommon(writer, textBox);
  }

  protected virtual void SerializeGradientFill(XmlWriter writer, TextBoxShapeBase textBox)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (textBox == null)
      throw new ArgumentNullException(nameof (textBox));
    switch (textBox.Fill.GradientColorType)
    {
      case ExcelGradientColor.OneColor:
        writer.WriteAttributeString("color", this.GenerateHexColor(textBox.Fill.BackColor));
        writer.WriteAttributeString("color2", this.PrepareGradientDegree(textBox.Fill.GradientDegree));
        break;
      case ExcelGradientColor.TwoColor:
        writer.WriteAttributeString("color", this.GenerateHexColor(textBox.Fill.BackColor));
        writer.WriteAttributeString("color2", this.GenerateHexColor(textBox.Fill.ForeColor));
        break;
      case ExcelGradientColor.Preset:
        writer.WriteAttributeString("method", "none");
        writer.WriteAttributeString("colors", this.GetPresetString(textBox.Fill.PresetGradientType));
        break;
    }
    this.SerializeGradientFillCommon(writer, textBox);
  }

  protected virtual void SerializeTextureFill(
    XmlWriter writer,
    TextBoxShapeBase textBox,
    FileDataHolder holder,
    RelationCollection vmlRelations)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (textBox == null)
      throw new ArgumentNullException(nameof (textBox));
    if (holder == null)
      throw new ArgumentNullException(nameof (holder));
    if (vmlRelations == null)
      throw new ArgumentNullException("relations");
    ExcelTexture texture = textBox.Fill.Texture;
    if (texture != ExcelTexture.User_Defined)
    {
      byte[] resData = ShapeFillImpl.GetResData("Text" + ((int) texture).ToString());
      byte[] numArray = new byte[resData.Length - 25];
      Array.Copy((Array) resData, 25, (Array) numArray, 0, numArray.Length);
      MemoryStream ms = new MemoryStream();
      ShapeFillImpl.UpdateBitMapHederToStream(ms, resData);
      ms.Write(numArray, 0, numArray.Length);
      Image image = Image.FromStream((Stream) ms, true, true);
      string str1 = texture.ToString().Replace('_', ' ').Trim();
      string str2 = holder.SaveImage(image, (string) null);
      string relationId = vmlRelations.GenerateRelationId();
      vmlRelations[relationId] = new Relation('/'.ToString() + str2, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image");
      writer.WriteAttributeString("relid", "urn:schemas-microsoft-com:office:office", relationId);
      writer.WriteAttributeString("title", "urn:schemas-microsoft-com:office:office", str1);
    }
    else
    {
      Image picture = textBox.Fill.Picture;
      string pictureName = textBox.Fill.PictureName;
      textBox.Fill.UserTexture(picture, pictureName);
      this.SerializeUserPicture(writer, textBox, holder, vmlRelations);
    }
  }

  protected virtual void SerializePatternFill(
    XmlWriter writer,
    TextBoxShapeBase textBox,
    FileDataHolder holder,
    RelationCollection vmlRelations)
  {
  }

  protected virtual void SerializePictureFill(
    XmlWriter writer,
    TextBoxShapeBase textBox,
    FileDataHolder holder,
    RelationCollection relations)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (holder == null)
      throw new ArgumentNullException(nameof (holder));
    if (relations == null)
      throw new ArgumentNullException(nameof (relations));
    if (textBox == null)
      throw new ArgumentNullException(nameof (textBox));
    Image picture = textBox.Fill.Picture;
    string pictureName = textBox.Fill.PictureName;
    int fillType = (int) textBox.Fill.FillType;
    textBox.Fill.FillType = ExcelFillType.SolidColor;
    writer.WriteAttributeString("opacity", this.GetOpacityFormat(textBox.Fill.Transparency));
    textBox.Fill.UserPicture(picture, pictureName);
    this.SerializeUserPicture(writer, textBox, holder, relations);
  }

  protected virtual void SerializeUserPicture(
    XmlWriter writer,
    TextBoxShapeBase textBox,
    FileDataHolder holder,
    RelationCollection relations)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (holder == null)
      throw new ArgumentNullException(nameof (holder));
    if (relations == null)
      throw new ArgumentNullException(nameof (relations));
    if (textBox == null)
      throw new ArgumentNullException(nameof (textBox));
    Image picture = textBox.Fill.Picture;
    string pictureName = textBox.Fill.PictureName;
    string str = holder.SaveImage(picture, (string) null);
    string relationId = relations.GenerateRelationId();
    relations[relationId] = new Relation('/'.ToString() + str, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image");
    writer.WriteAttributeString("relid", "urn:schemas-microsoft-com:office:office", relationId);
    writer.WriteAttributeString("title", "urn:schemas-microsoft-com:office:office", pictureName);
  }

  protected virtual void SerializeGradientFillCommon(XmlWriter writer, TextBoxShapeBase textBox)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (textBox == null)
      throw new ArgumentNullException(nameof (textBox));
    writer.WriteAttributeString("opacity", this.GetOpacityFormat(textBox.Fill.TransparencyFrom));
    writer.WriteAttributeString("recolor", "t");
    writer.WriteAttributeString("rotate", "t");
    writer.WriteAttributeString("opacity2", "urn:schemas-microsoft-com:office:office", this.GetOpacityFormat(textBox.Fill.TransparencyTo));
    double num1 = 0.0;
    switch (textBox.Fill.GradientStyle)
    {
      case ExcelGradientStyle.Horizontal:
        num1 = 0.0;
        writer.WriteAttributeString("type", "gradient");
        break;
      case ExcelGradientStyle.Vertical:
        double num2 = -90.0;
        writer.WriteAttributeString("type", "gradient");
        writer.WriteAttributeString("angle", num2.ToString());
        break;
      case ExcelGradientStyle.Diagonl_Up:
        double num3 = -135.0;
        writer.WriteAttributeString("angle", num3.ToString());
        writer.WriteAttributeString("type", "gradient");
        break;
      case ExcelGradientStyle.Diagonl_Down:
        double num4 = -45.0;
        writer.WriteAttributeString("angle", num4.ToString());
        writer.WriteAttributeString("type", "gradient");
        break;
      case ExcelGradientStyle.From_Corner:
        double num5 = -45.0;
        writer.WriteAttributeString("angle", num5.ToString());
        writer.WriteAttributeString("type", "gradientRadial");
        writer.WriteStartElement("fill", "urn:schemas-microsoft-com:office:office");
        writer.WriteAttributeString("ext", "urn:schemas-microsoft-com:vml", "view");
        writer.WriteAttributeString("type", "gradientCenter");
        writer.WriteEndElement();
        break;
      case ExcelGradientStyle.From_Center:
        double num6 = -45.0;
        writer.WriteAttributeString("angle", num6.ToString());
        writer.WriteAttributeString("type", "gradientRadial");
        break;
    }
    switch (textBox.Fill.GradientVariant)
    {
      case ExcelGradientVariants.ShadingVariants_1:
        writer.WriteAttributeString("focus", 100.ToString() + "%");
        break;
      case ExcelGradientVariants.ShadingVariants_3:
        writer.WriteAttributeString("focus", 50.ToString() + "%");
        break;
      case ExcelGradientVariants.ShadingVariants_4:
        writer.WriteAttributeString("focus", -50.ToString() + "%");
        break;
    }
  }

  protected virtual void SerializeFillCommon(XmlWriter writer, TextBoxShapeBase textBox)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (textBox == null)
      throw new ArgumentNullException(nameof (textBox));
    ExcelFillType fillType = textBox.Fill.FillType;
    textBox.Fill.FillType = ExcelFillType.SolidColor;
    writer.WriteAttributeString("opacity", this.GetOpacityFormat(textBox.Fill.Transparency));
    textBox.Fill.FillType = fillType;
    writer.WriteAttributeString("recolor", "t");
    writer.WriteAttributeString("rotate", "t");
  }

  protected virtual void SerializeLine(
    XmlWriter writer,
    TextBoxShapeBase textBox,
    FileDataHolder holder,
    RelationCollection relations)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (textBox == null)
      throw new ArgumentNullException(nameof (textBox));
    if (!textBox.HasLineFormat)
      return;
    ShapeLineFormatImpl line = textBox.Line as ShapeLineFormatImpl;
    writer.WriteStartElement("stroke", "urn:schemas-microsoft-com:vml");
    if (textBox.Line.HasPattern)
    {
      this.SerializePatternLine(writer, textBox, holder, relations);
      writer.WriteAttributeString("color2", '#'.ToString() + this.GenerateHexColor(textBox.Line.ForeColor));
    }
    else
    {
      string str = this.GetDashStyle(textBox.Line.DashStyle);
      if (str == "roundDot")
        str = "1 1";
      writer.WriteAttributeString("dashstyle", str);
      writer.WriteAttributeString("linestyle", this.GetLineStyle(textBox.Line.Style));
      writer.WriteAttributeString("color2", '#'.ToString() + this.GenerateHexColor(textBox.Line.ForeColor));
      if (line != null && line.EndCapType != null)
        writer.WriteAttributeString("endcap", line.EndCapType);
    }
    writer.WriteEndElement();
  }

  protected virtual void SerializePatternLine(
    XmlWriter writer,
    TextBoxShapeBase textBox,
    FileDataHolder holder,
    RelationCollection relations)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (textBox == null)
      throw new ArgumentNullException(nameof (textBox));
    if (holder == null)
      throw new ArgumentNullException(nameof (holder));
    if (relations == null)
      throw new ArgumentNullException(nameof (relations));
    ExcelGradientPattern pattern = textBox.Line.Pattern;
    byte[] resData = ShapeFillImpl.GetResData("Patt" + ((int) pattern).ToString());
    byte[] numArray = new byte[resData.Length - 25];
    Array.Copy((Array) resData, 25, (Array) numArray, 0, numArray.Length);
    MemoryStream ms = new MemoryStream();
    ShapeFillImpl.UpdateBitMapHederToStream(ms, resData);
    ms.Write(numArray, 0, numArray.Length);
    Image image = ApplicationImpl.CreateImage((Stream) ms);
    pattern.ToString();
    string patternName = this.GeneratePatternName(pattern);
    string str = holder.SaveImage(image, (string) null);
    string relationId = relations.GenerateRelationId();
    relations[relationId] = new Relation('/'.ToString() + str, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image");
    writer.WriteAttributeString("relid", "urn:schemas-microsoft-com:office:office", relationId);
    writer.WriteAttributeString("title", "urn:schemas-microsoft-com:office:office", patternName);
  }

  protected string PrepareGradientDegree(double degree)
  {
    return degree <= 0.5 ? "fill lighten(" + (object) (int) (degree * (double) byte.MaxValue) + ")" : "fill darken(" + (object) (int) (degree * (double) byte.MaxValue + 0.5) + ")";
  }

  protected string GenerateHexColor(Color color)
  {
    return '#'.ToString() + this.RemovePrecedingZeroes((color.ToArgb() & 16777215 /*0xFFFFFF*/).ToString("X6"));
  }

  protected string GetPresetString(ExcelGradientPreset excelGradientPreset)
  {
    return new ResourceManager("Syncfusion.XlsIO.VMLPresetGradientFills", typeof (VmlTextBoxBaseSerializator).Assembly).GetString(excelGradientPreset.ToString(), CultureInfo.CurrentCulture);
  }

  protected string GeneratePatternName(ExcelGradientPattern pattern)
  {
    return pattern.ToString().Remove(0, "pat_".ToString().Length - 1).Replace('_', ' ').Trim();
  }

  protected string GetOpacityFormat(double opacity)
  {
    opacity = 1.0 - opacity;
    return ((int) (opacity * 65536.0)).ToString() + "f";
  }

  protected string RemovePrecedingZeroes(string color)
  {
    for (int index = 0; index < color.Length && color.StartsWith("0"); ++index)
      color = color.Remove(0, 1);
    return color;
  }

  protected string GetDashStyle(ExcelShapeDashLineStyle dashStyle)
  {
    if (VmlTextBoxBaseParser.m_excelDashLineStyle == null)
      VmlTextBoxBaseParser.InitDashLineStyle();
    Dictionary<string, ExcelShapeDashLineStyle> excelDashLineStyle = VmlTextBoxBaseParser.m_excelDashLineStyle;
    IEnumerator<string> enumerator1 = (IEnumerator<string>) excelDashLineStyle.Keys.GetEnumerator();
    IEnumerator<ExcelShapeDashLineStyle> enumerator2 = (IEnumerator<ExcelShapeDashLineStyle>) excelDashLineStyle.Values.GetEnumerator();
    while (enumerator1.MoveNext())
    {
      enumerator2.MoveNext();
      if (enumerator2.Current == dashStyle)
        break;
    }
    return enumerator1.Current;
  }

  protected string GetLineStyle(ExcelShapeLineStyle lineStyle)
  {
    lock (ShapeSerializator.m_lockObject)
    {
      if (VmlTextBoxBaseParser.m_excelShapeLineStyle == null)
        VmlTextBoxBaseParser.InitShapeLineStyle();
      Dictionary<string, ExcelShapeLineStyle> excelShapeLineStyle = VmlTextBoxBaseParser.m_excelShapeLineStyle;
      IEnumerator<string> enumerator1 = (IEnumerator<string>) excelShapeLineStyle.Keys.GetEnumerator();
      IEnumerator<ExcelShapeLineStyle> enumerator2 = (IEnumerator<ExcelShapeLineStyle>) excelShapeLineStyle.Values.GetEnumerator();
      while (enumerator1.MoveNext())
      {
        enumerator2.MoveNext();
        if (enumerator2.Current == lineStyle)
          break;
      }
      return enumerator1.Current;
    }
  }

  public static bool IsEmptyColor(Color color) => color == ColorExtension.Empty;

  internal virtual void Clear()
  {
  }
}
