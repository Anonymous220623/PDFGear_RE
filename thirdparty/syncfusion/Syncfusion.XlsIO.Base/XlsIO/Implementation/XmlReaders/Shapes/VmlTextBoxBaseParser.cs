// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlReaders.Shapes.VmlTextBoxBaseParser
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.Compression;
using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Implementation.XmlSerialization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlReaders.Shapes;

internal abstract class VmlTextBoxBaseParser : ShapeParser
{
  internal const int GradientHorizontal = 0;
  internal const int GradientVertical = -90;
  internal const int GradientDiagonalUp = -135;
  internal const int GradientDiagonalDownCornerCenter = -45;
  private const byte IndexedColor = 1;
  private const byte NamedColor = 2;
  private const byte HexColor = 3;
  internal const int GradientVariant_1 = 100;
  internal const int GradientVariant_3 = 50;
  internal const int GradientVariant_4 = -50;
  internal const string PatternPrefix = "pat_";
  private const string LineStylePrefix = "LINE_";
  private const string ResourcePatternPrefix = "Patt";
  private const string DefaultFillStyle = "solid";
  public static Dictionary<string, ExcelShapeLineStyle> m_excelShapeLineStyle;
  public static Dictionary<string, ExcelShapeDashLineStyle> m_excelDashLineStyle;
  private bool m_isGradientShadingRadial;

  public static void InitShapeLineStyle()
  {
    if (VmlTextBoxBaseParser.m_excelShapeLineStyle != null)
      return;
    VmlTextBoxBaseParser.m_excelShapeLineStyle = new Dictionary<string, ExcelShapeLineStyle>();
    lock (VmlTextBoxBaseParser.m_excelShapeLineStyle)
    {
      VmlTextBoxBaseParser.m_excelShapeLineStyle.Add("single", ExcelShapeLineStyle.Line_Single);
      VmlTextBoxBaseParser.m_excelShapeLineStyle.Add("thinThin", ExcelShapeLineStyle.Line_Thin_Thin);
      VmlTextBoxBaseParser.m_excelShapeLineStyle.Add("thinThick", ExcelShapeLineStyle.Line_Thin_Thick);
      VmlTextBoxBaseParser.m_excelShapeLineStyle.Add("thickThin", ExcelShapeLineStyle.Line_Thick_Thin);
      VmlTextBoxBaseParser.m_excelShapeLineStyle.Add("thickBetweenThin", ExcelShapeLineStyle.Line_Thick_Between_Thin);
    }
  }

  public static void InitDashLineStyle()
  {
    VmlTextBoxBaseParser.m_excelDashLineStyle = new Dictionary<string, ExcelShapeDashLineStyle>();
    VmlTextBoxBaseParser.m_excelDashLineStyle.Add("solid", ExcelShapeDashLineStyle.Solid);
    VmlTextBoxBaseParser.m_excelDashLineStyle.Add("roundDot", ExcelShapeDashLineStyle.Dotted_Round);
    VmlTextBoxBaseParser.m_excelDashLineStyle.Add("1 1", ExcelShapeDashLineStyle.Dotted);
    VmlTextBoxBaseParser.m_excelDashLineStyle.Add("dash", ExcelShapeDashLineStyle.Dashed);
    VmlTextBoxBaseParser.m_excelDashLineStyle.Add("dashDot", ExcelShapeDashLineStyle.Dash_Dot);
    VmlTextBoxBaseParser.m_excelDashLineStyle.Add("longDash", ExcelShapeDashLineStyle.Medium_Dashed);
    VmlTextBoxBaseParser.m_excelDashLineStyle.Add("longDashDot", ExcelShapeDashLineStyle.Medium_Dash_Dot);
    VmlTextBoxBaseParser.m_excelDashLineStyle.Add("longDashDotDot", ExcelShapeDashLineStyle.Dash_Dot_Dot);
  }

  private bool IsGradientShadingRadial
  {
    get => this.m_isGradientShadingRadial;
    set => this.m_isGradientShadingRadial = value;
  }

  public override bool ParseShape(
    XmlReader reader,
    ShapeImpl defaultShape,
    RelationCollection relations,
    string parentItemPath)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    TextBoxShapeBase textBoxShapeBase = defaultShape != null ? (TextBoxShapeBase) defaultShape.Clone(defaultShape.Parent, (Dictionary<string, string>) null, (Dictionary<int, int>) null, false) : throw new ArgumentNullException(nameof (defaultShape));
    VmlTextBoxBaseParser.ParseShapeId(reader, (ShapeImpl) textBoxShapeBase);
    if (reader.MoveToAttribute("style"))
      this.ParseStyle(reader, textBoxShapeBase);
    if (reader.MoveToAttribute("filled"))
    {
      if (textBoxShapeBase is OptionButtonShapeImpl)
        textBoxShapeBase.Fill.Visible = false;
      else
        textBoxShapeBase.HasFill = false;
    }
    else
      textBoxShapeBase.HasFill = true;
    if (reader.MoveToAttribute("stroked"))
      textBoxShapeBase.HasLineFormat = reader.Value != "f";
    if (textBoxShapeBase.HasLineFormat)
    {
      if (reader.MoveToAttribute("strokecolor"))
      {
        ColorObject color = this.ExtractColor(reader.Value);
        textBoxShapeBase.Line.BackColor = color.GetRGB(textBoxShapeBase.Workbook);
        if (defaultShape is CommentShapeImpl)
          textBoxShapeBase.Line.ForeColor = textBoxShapeBase.Line.ForeColor;
        else
          textBoxShapeBase.Line.ForeColor = color.GetRGB(textBoxShapeBase.Workbook);
        textBoxShapeBase.Line.DashStyle = ExcelShapeDashLineStyle.Solid;
        textBoxShapeBase.Line.HasPattern = false;
        textBoxShapeBase.Line.Style = ExcelShapeLineStyle.Line_Single;
        textBoxShapeBase.Line.Weight = 0.5;
      }
      if (reader.MoveToAttribute("strokeweight"))
      {
        string str = reader.Value;
        if (str.Contains("pt"))
        {
          double result;
          if (double.TryParse(str.Split('p')[0], NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result))
            textBoxShapeBase.Line.Weight = result;
        }
        else if (str.Contains("mm"))
        {
          double result;
          if (double.TryParse(str.Split('m')[0], NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result))
            textBoxShapeBase.Line.Weight = result;
        }
      }
    }
    if (textBoxShapeBase.HasFill && reader.MoveToAttribute("fillcolor"))
    {
      ColorObject color = this.ExtractColor(reader.Value);
      textBoxShapeBase.ColorObject = color;
      textBoxShapeBase.FillColor = color.GetRGB(textBoxShapeBase.Workbook);
      textBoxShapeBase.Fill.ForeColor = textBoxShapeBase.FillColor;
    }
    if (reader.MoveToAttribute("alt"))
      textBoxShapeBase.AlternativeText = reader.Value.Split('#')[0];
    if (reader.MoveToAttribute("spid", "urn:schemas-microsoft-com:office:office") && reader.MoveToAttribute("id"))
      textBoxShapeBase.Name = reader.Value;
    reader.MoveToElement();
    reader.Read();
    bool shape = true;
    bool flag1 = false;
    bool flag2 = false;
    string shapeType = (string) null;
    bool hasRowColumn = false;
    while (reader.NodeType != XmlNodeType.EndElement && shape)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "ClientData":
            flag1 = true;
            shape = this.ParseClientData(reader, textBoxShapeBase, out shapeType, ref hasRowColumn);
            continue;
          case "textbox":
            flag2 = true;
            this.ParseTextBox(reader, textBoxShapeBase);
            continue;
          case "fill":
            this.ParseFillStyle(reader, textBoxShapeBase, relations, parentItemPath);
            continue;
          case "stroke":
            if (textBoxShapeBase.HasLineFormat)
            {
              this.ParseLine(reader, textBoxShapeBase, relations, parentItemPath);
              continue;
            }
            break;
        }
        reader.Skip();
      }
      else
        reader.Skip();
    }
    reader.Read();
    if (textBoxShapeBase.Worksheet.InnerShapes.GetShapeById(textBoxShapeBase.ShapeId) is ShapeImpl shapeById && !shapeById.EnableAlternateContent && shapeById.ClientAnchor.Equals((object) textBoxShapeBase.ClientAnchor))
      shape = false;
    if (!hasRowColumn)
      textBoxShapeBase.ValidComment = false;
    if (shape && (flag1 || flag2) && shapeType != "Shape")
    {
      if (shapeById != null && shapeById.Group != null)
      {
        int index = Array.IndexOf<IShape>(shapeById.Group.Items, (IShape) shapeById);
        shapeById.Group.Items[index] = (IShape) textBoxShapeBase;
      }
      else
        this.RegisterShape(textBoxShapeBase);
    }
    return shape;
  }

  public static void ParseShapeId(XmlReader reader, ShapeImpl shape)
  {
    string str1 = (string) null;
    int num = 0;
    if (reader.MoveToAttribute("spid", "urn:schemas-microsoft-com:office:office"))
    {
      string str2 = reader.Value;
      num = str2.IndexOf("_s");
      if (num >= 0)
        str1 = str2;
    }
    if (str1 == null && reader.MoveToAttribute("id"))
    {
      string str3 = reader.Value;
      num = str3.IndexOf("_s");
      if (num >= 0)
        str1 = str3;
    }
    int result;
    if (str1 == null || !int.TryParse(str1.Substring(num + 2), out result))
      return;
    ShapesCollection innerShapes = shape.Worksheet.InnerShapes;
    if (innerShapes.StartId == 0)
      innerShapes.StartId = result;
    ShapeImpl shapeById = innerShapes.GetShapeById(result) as ShapeImpl;
    shape.ShapeId = result;
    if (shapeById == null || !shapeById.EnableAlternateContent)
      return;
    shape.EnableAlternateContent = true;
    shape.XmlDataStream = shapeById.XmlDataStream;
    if (shapeById.Name != null && shapeById.Name.Length > 0)
      shape.Name = shapeById.Name;
    shapeById.Remove();
  }

  private void ParseTextBox(XmlReader reader, TextBoxShapeBase textBox)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (textBox == null)
      throw new ArgumentNullException(nameof (textBox));
    if (reader.LocalName != "textbox")
      throw new XmlException("Unexcpected xml tag.");
    if (reader.MoveToAttribute("style"))
    {
      Dictionary<string, string> dictProperties = this.SplitStyle(reader.Value);
      this.ParseTextDirection(textBox, dictProperties);
    }
    reader.MoveToElement();
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "div":
              this.ParseDiv(reader, textBox);
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
    reader.Skip();
  }

  private void ParseDiv(XmlReader reader, TextBoxShapeBase textBox)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (textBox == null)
      throw new ArgumentNullException(nameof (textBox));
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "font":
              this.ParseFormattingRun(reader, textBox);
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
    reader.Read();
  }

  private void ParseFormattingRun(XmlReader reader, TextBoxShapeBase textBox)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (textBox == null)
      throw new ArgumentNullException(nameof (textBox));
    Stream data = ShapeParser.ReadNodeAsStream(reader);
    data.Position = 0L;
    bool flag = this.CheckFontElement(UtilityMethods.CreateReader(data));
    data.Position = 0L;
    XmlReader reader1 = UtilityMethods.CreateReader(data);
    IFont font = textBox.Workbook.CreateFont();
    if (reader1.MoveToAttribute("face"))
      font.FontName = reader1.Value;
    if (reader1.MoveToAttribute("size"))
      font.Size = (double) XmlConvertExtension.ToInt32(reader1.Value) / 20.0;
    reader1.MoveToElement();
    string empty = string.Empty;
    if (flag)
    {
      string text = reader1.ReadElementContentAsString().Replace("\r\n", "");
      for (int index = 0; index < text.Length; ++index)
      {
        int num1 = 0;
        if (num1 == 0)
        {
          text = text.Replace("  ", "");
          int num2 = num1 + 1;
        }
      }
      IRichTextString richText = textBox.RichText;
      int length = richText.Text.Length;
      richText.Append(text, font);
    }
    else
    {
      if (reader1.IsEmptyElement)
        return;
      reader1.Read();
      while (reader1.NodeType != XmlNodeType.EndElement)
      {
        if (reader1.NodeType == XmlNodeType.Element)
        {
          switch (reader1.LocalName)
          {
            case "span":
              empty += this.ParseSpanTag(reader1, textBox);
              continue;
            default:
              reader1.Skip();
              continue;
          }
        }
        else if (reader1.NodeType == XmlNodeType.Text)
        {
          empty += reader1.Value;
          reader1.Skip();
        }
      }
      string text = empty.Replace("\r\n  ", "");
      IRichTextString richText = textBox.RichText;
      int length = richText.Text.Length;
      richText.Append(text, font);
    }
  }

  private string ParseSpanTag(XmlReader reader, TextBoxShapeBase textBox)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (textBox == null)
      throw new ArgumentNullException(nameof (textBox));
    return reader.ReadElementContentAsString();
  }

  private bool CheckFontElement(XmlReader fontReader)
  {
    if (fontReader == null)
      throw new ArgumentNullException();
    if (fontReader.LocalName != "font")
      throw new XmlException();
    fontReader.Read();
    fontReader.Read();
    return fontReader.LocalName == "font" && fontReader.NodeType == XmlNodeType.EndElement;
  }

  protected virtual void RegisterShape(TextBoxShapeBase textBox)
  {
    if (textBox == null)
      throw new ArgumentNullException("comment");
    textBox.Worksheet.InnerShapes.AddShape((ShapeImpl) textBox);
  }

  private bool ParseClientData(
    XmlReader reader,
    TextBoxShapeBase textBox,
    out string shapeType,
    ref bool hasRowColumn)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (textBox == null)
      throw new ArgumentNullException(nameof (textBox));
    if (reader.LocalName != "ClientData")
      throw new XmlException("Unexpected xml token");
    shapeType = (string) null;
    if (reader.MoveToAttribute("ObjectType"))
    {
      shapeType = reader.Value;
      if (shapeType == "Pict")
        return false;
    }
    reader.Read();
    textBox.IsMoveWithCell = true;
    textBox.IsSizeWithCell = true;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "MoveWithCells":
            textBox.IsMoveWithCell = !VmlTextBoxBaseParser.ParseBoolOrEmpty(reader, true);
            break;
          case "SizeWithCells":
            textBox.IsSizeWithCell = !VmlTextBoxBaseParser.ParseBoolOrEmpty(reader, true);
            break;
          case "Anchor":
            this.ParseAnchor(reader, (ShapeImpl) textBox);
            break;
          case "TextHAlign":
            textBox.HAlignment = (ExcelCommentHAlign) Enum.Parse(typeof (ExcelCommentHAlign), reader.ReadElementContentAsString(), false);
            break;
          case "TextVAlign":
            textBox.VAlignment = (ExcelCommentVAlign) Enum.Parse(typeof (ExcelCommentVAlign), reader.ReadElementContentAsString(), false);
            break;
          case "FmlaMacro":
            textBox.OnAction = reader.ReadElementContentAsString();
            break;
          case "LockText":
            string str = reader.ReadElementContentAsString();
            textBox.IsTextLocked = XmlConvertExtension.ToBoolean(str.ToLower());
            break;
          case "PrintObject":
            textBox.PrintWithSheet = false;
            reader.Skip();
            break;
          default:
            this.ParseUnknownClientDataTag(reader, textBox);
            hasRowColumn = true;
            break;
        }
      }
      reader.Read();
    }
    reader.Read();
    return true;
  }

  protected virtual void ParseUnknownClientDataTag(XmlReader reader, TextBoxShapeBase textBox)
  {
    reader.Skip();
  }

  public static bool ParseBoolOrEmpty(XmlReader reader, bool defaultValue)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    bool boolOrEmpty = defaultValue;
    if (!reader.IsEmptyElement)
    {
      string str = reader.ReadElementContentAsString();
      if (str.Length != 0)
        boolOrEmpty = bool.Parse(str);
    }
    else
      reader.Read();
    return boolOrEmpty;
  }

  private void ParseFillStyle(
    XmlReader reader,
    TextBoxShapeBase textBox,
    RelationCollection relations,
    string parentItemPath)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (textBox == null)
      throw new ArgumentNullException(nameof (textBox));
    ExcelFillType excelFillType = reader.MoveToAttribute("type") ? this.GetExcelFillType(reader.Value) : this.GetExcelFillType("solid");
    if (reader.MoveToAttribute("opacity"))
      textBox.Fill.Transparency = 1.0 - this.ExtractOpacity(reader.Value);
    if (reader.MoveToAttribute("color"))
    {
      ColorObject color = this.ExtractColor(reader.Value);
      textBox.FillColor = color.GetRGB(textBox.Workbook);
      textBox.ColorObject = color;
      if (textBox.FillColor == ColorExtension.Empty)
        textBox.HasFill = false;
      textBox.Fill.ForeColor = textBox.FillColor;
    }
    switch (excelFillType)
    {
      case ExcelFillType.SolidColor:
        this.ParseSolidFill(reader, textBox);
        break;
      case ExcelFillType.Pattern:
        this.ParsePatternFill(reader, textBox, relations);
        break;
      case ExcelFillType.Texture:
        this.ParseTextureFill(reader, textBox, relations);
        break;
      case ExcelFillType.Picture:
        this.ParsePictureFill(reader, textBox, relations);
        break;
      case ExcelFillType.Gradient:
        this.ParseGradientFill(reader, textBox);
        break;
    }
  }

  private void ParseSolidFill(XmlReader reader, TextBoxShapeBase textBox)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (textBox == null)
      throw new ArgumentNullException(nameof (textBox));
    textBox.Fill.FillType = ExcelFillType.SolidColor;
    if (reader.MoveToAttribute("opacity"))
      textBox.Fill.Transparency = 1.0 - this.ExtractOpacity(reader.Value);
    reader.MoveToElement();
    reader.Skip();
  }

  private void ParseGradientFill(XmlReader reader, TextBoxShapeBase textBox)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (textBox == null)
      throw new ArgumentNullException(nameof (textBox));
    textBox.Fill.FillType = ExcelFillType.Gradient;
    string color1 = (string) null;
    textBox.Fill.GradientColorType = this.ExtractGradientColorType(reader, out color1);
    switch (textBox.Fill.GradientColorType)
    {
      case ExcelGradientColor.OneColor:
        textBox.Fill.BackColor = textBox.FillColor;
        textBox.Fill.GradientDegree = this.ExtractDegree(color1);
        break;
      case ExcelGradientColor.TwoColor:
        ColorObject color2 = this.ExtractColor(color1);
        textBox.Fill.BackColor = textBox.FillColor;
        textBox.Fill.ForeColor = color2.GetRGB(textBox.Workbook);
        break;
      case ExcelGradientColor.Preset:
        textBox.Fill.PresetGradient(this.ExtractPreset(color1));
        break;
    }
    this.ParseGradientCommon(reader, textBox);
  }

  private void ParseTextureFill(
    XmlReader reader,
    TextBoxShapeBase textBox,
    RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (textBox == null)
      throw new ArgumentNullException(nameof (textBox));
    if (relations == null)
      throw new ArgumentNullException("relation collection");
    string str1 = "image";
    if (reader.MoveToAttribute("title", "urn:schemas-microsoft-com:office:office"))
      str1 = reader.Value;
    textBox.Fill.FillType = ExcelFillType.Texture;
    textBox.Fill.Texture = this.ExtractTexture(str1);
    if (textBox.Fill.Texture == ExcelTexture.User_Defined && reader.MoveToAttribute("relid", "urn:schemas-microsoft-com:office:office"))
    {
      FileDataHolder dataHolder = textBox.ParentWorkbook.DataHolder;
      string id = reader.Value;
      Relation relation = relations[id];
      string itemPath = relations.ItemPath;
      int length1 = itemPath.LastIndexOf('/');
      string str2 = itemPath.Substring(0, length1);
      int length2 = str2.LastIndexOf('/');
      string strFullPath = FileDataHolder.CombinePath(str2.Substring(0, length2), relation.Target);
      Image image = dataHolder.GetImage(strFullPath);
      textBox.Fill.UserTexture(image, str1);
    }
    else
      textBox.Fill.PresetTextured(this.ExtractTexture(str1));
  }

  private void ParsePatternFill(
    XmlReader reader,
    TextBoxShapeBase textBox,
    RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (textBox == null)
      throw new ArgumentNullException(nameof (textBox));
    if (relations == null)
      throw new ArgumentNullException("relation collection");
    string title = "image";
    if (reader.MoveToAttribute("title", "urn:schemas-microsoft-com:office:office"))
      title = reader.Value;
    if (reader.MoveToAttribute("color2"))
    {
      ColorObject color = this.ExtractColor(reader.Value);
      textBox.Fill.BackColor = textBox.FillColor;
      textBox.Fill.ForeColor = color.GetRGB(textBox.Workbook);
      textBox.Fill.FillType = ExcelFillType.Pattern;
      textBox.Fill.Pattern = this.ExtractPattern(title);
    }
    if (!reader.MoveToAttribute("relid", "urn:schemas-microsoft-com:office:office"))
      return;
    FileDataHolder dataHolder = textBox.ParentWorkbook.DataHolder;
    string id = reader.Value;
    Relation relation = relations[id];
    string itemPath = relations.ItemPath;
    int length1 = itemPath.LastIndexOf('/');
    string str = itemPath.Substring(0, length1);
    int length2 = str.LastIndexOf('/');
    string strFullPath = FileDataHolder.CombinePath(str.Substring(0, length2), relation.Target);
    dataHolder.GetImage(strFullPath);
    textBox.Fill.Patterned(textBox.Fill.Pattern);
  }

  private void ParsePictureFill(
    XmlReader reader,
    TextBoxShapeBase textBox,
    RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (textBox == null)
      throw new ArgumentNullException(nameof (textBox));
    if (relations == null)
      throw new ArgumentNullException("relation collection");
    string name = "image";
    if (reader.MoveToAttribute("title", "urn:schemas-microsoft-com:office:office"))
      name = reader.Value;
    if (!reader.MoveToAttribute("relid", "urn:schemas-microsoft-com:office:office"))
      return;
    FileDataHolder dataHolder = textBox.ParentWorkbook.DataHolder;
    string id = reader.Value;
    Relation relation = relations[id];
    string itemPath = relations.ItemPath;
    int length1 = itemPath.LastIndexOf('/');
    string str = itemPath.Substring(0, length1);
    int length2 = str.LastIndexOf('/');
    string strFullPath = FileDataHolder.CombinePath(str.Substring(0, length2), relation.Target);
    Image image = dataHolder.GetImage(strFullPath);
    textBox.Fill.UserPicture(image, name);
  }

  private void ParseGradientCommon(XmlReader reader, TextBoxShapeBase textBox)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (textBox == null)
      throw new ArgumentNullException(nameof (textBox));
    int num = 0;
    if (reader.MoveToAttribute("opacity"))
      textBox.Fill.TransparencyFrom = this.ExtractOpacity(reader.Value);
    if (reader.MoveToAttribute("o:opacity2"))
      textBox.Fill.TransparencyTo = this.ExtractOpacity(reader.Value);
    if (reader.MoveToAttribute("focus"))
      textBox.Fill.GradientVariant = this.ExtractShadingVariant(reader.Value);
    if (reader.MoveToAttribute("angle"))
      num = reader.ReadContentAsInt();
    switch (num)
    {
      case -135:
        textBox.Fill.GradientStyle = ExcelGradientStyle.Diagonl_Up;
        break;
      case -90:
        textBox.Fill.GradientStyle = ExcelGradientStyle.Vertical;
        break;
      case -45:
        if (this.IsGradientShadingRadial)
        {
          reader.Read();
          if (reader.NodeType == XmlNodeType.EndElement || reader.NodeType == XmlNodeType.Whitespace)
          {
            textBox.Fill.GradientStyle = ExcelGradientStyle.From_Center;
            break;
          }
          textBox.Fill.GradientStyle = ExcelGradientStyle.From_Corner;
          break;
        }
        textBox.Fill.GradientStyle = ExcelGradientStyle.Diagonl_Down;
        break;
      case 0:
        textBox.Fill.GradientStyle = ExcelGradientStyle.Horizontal;
        break;
    }
  }

  private void ParseLine(
    XmlReader reader,
    TextBoxShapeBase textBox,
    RelationCollection relations,
    string parentItemPath)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (textBox == null)
      throw new ArgumentNullException(nameof (textBox));
    if (reader.MoveToAttribute("filltype"))
    {
      textBox.Line.HasPattern = true;
      this.ParsePatternLine(reader, textBox, relations, parentItemPath);
    }
    else
    {
      if (reader.MoveToAttribute("dashstyle"))
        textBox.Line.DashStyle = this.ExtractDashStyle(reader.Value);
      if (reader.MoveToAttribute("linestyle"))
        textBox.Line.Style = this.ExtractLineStyle(reader.Value);
      if (reader.MoveToAttribute("endcap"))
        (textBox.Line as ShapeLineFormatImpl).EndCapType = reader.Value;
      if (textBox.Line.DashStyle == ExcelShapeDashLineStyle.Dotted && (textBox.Line as ShapeLineFormatImpl).EndCapType == "round")
        textBox.Line.DashStyle = ExcelShapeDashLineStyle.Dotted_Round;
      reader.MoveToElement();
      reader.Skip();
    }
  }

  private void ParsePatternLine(
    XmlReader reader,
    TextBoxShapeBase textBox,
    RelationCollection relations,
    string parentItemPath)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (textBox == null)
      throw new ArgumentNullException(nameof (textBox));
    if (relations == null)
      throw new ArgumentNullException("relation collection");
    if (parentItemPath == null)
      throw new ArgumentNullException("resource path");
    if (!reader.MoveToAttribute("relid", "urn:schemas-microsoft-com:office:office"))
      return;
    FileDataHolder dataHolder = textBox.ParentWorkbook.DataHolder;
    string id = reader.Value;
    Relation relation = relations[id];
    string itemPath = relations.ItemPath;
    int length1 = itemPath.LastIndexOf('/');
    string str = itemPath.Substring(0, length1);
    int length2 = str.LastIndexOf('/');
    string strFullPath = FileDataHolder.CombinePath(str.Substring(0, length2), relation.Target);
    dataHolder.GetImage(strFullPath);
    int linePattern = (int) this.ExtractLinePattern((long) dataHolder.GetData(relation, parentItemPath, false).Length);
  }

  private void ParseStyle(XmlReader reader, TextBoxShapeBase textBox)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (textBox == null)
      throw new ArgumentNullException(nameof (textBox));
    string styleValue = reader.Value;
    textBox.StyleProperties = this.SplitStyle(styleValue);
    this.ParseStyle(textBox, textBox.StyleProperties);
  }

  protected virtual void ParseStyle(
    TextBoxShapeBase textBox,
    Dictionary<string, string> styleProperties)
  {
    string str;
    if (!styleProperties.TryGetValue("visibility", out str) || !(str == "hidden"))
      return;
    textBox.IsShapeVisible = false;
  }

  private void ParseTextDirection(
    TextBoxShapeBase textBox,
    Dictionary<string, string> dictProperties)
  {
    if (textBox == null)
      throw new ArgumentNullException(nameof (textBox));
    if (dictProperties == null)
      throw new ArgumentNullException(nameof (dictProperties));
    string str1;
    dictProperties.TryGetValue("mso-layout-flow-alt", out str1);
    string str2;
    dictProperties.TryGetValue("layout-flow", out str2);
    if (str1 == null && !(str2 == "vertical"))
      return;
    ExcelTextRotation excelTextRotation = !(str1 == "top-to-bottom") ? (!(str1 == "bottom-to-top") ? ExcelTextRotation.Clockwise : ExcelTextRotation.CounterClockwise) : ExcelTextRotation.TopToBottom;
    textBox.TextRotation = excelTextRotation;
  }

  private ExcelFillType GetExcelFillType(string excelFillType)
  {
    Dictionary<string, ExcelFillType> dictionary = new Dictionary<string, ExcelFillType>();
    dictionary.Add("gradient", ExcelFillType.Gradient);
    dictionary.Add("gradientRadial", ExcelFillType.Gradient);
    dictionary.Add("pattern", ExcelFillType.Pattern);
    dictionary.Add("frame", ExcelFillType.Picture);
    dictionary.Add("tile", ExcelFillType.Texture);
    dictionary.Add("solid", ExcelFillType.SolidColor);
    if (excelFillType.Equals("gradientRadial"))
      this.IsGradientShadingRadial = true;
    return dictionary[excelFillType];
  }

  private byte GetColorType(string color)
  {
    if (color.Contains('['.ToString()))
      return 1;
    return color.Contains('#'.ToString()) ? (byte) 3 : (byte) 2;
  }

  private ColorObject ExtractColor(string color)
  {
    switch (this.GetColorType(color))
    {
      case 1:
        return new ColorObject(ColorType.Indexed, Convert.ToInt32(color.Split('[')[1].Split(']')[0]));
      case 2:
        return new ColorObject(ColorExtension.FromName(color));
      case 3:
        if (this.IsHexString(color))
          return new ColorObject(ColorType.RGB, int.Parse(this.RemoveCharUnSafeAt(color, false), NumberStyles.HexNumber, (IFormatProvider) null));
        return new ColorObject(ShapeFillImpl.DEF_COMENT_PARSE_COLOR)
        {
          HexColor = color
        };
      default:
        return new ColorObject(ColorType.RGB, 1);
    }
  }

  private bool IsHexString(string color)
  {
    color = color.Trim('#');
    foreach (char ch in color.ToCharArray())
    {
      if ((ch < '0' || ch > '9') && (ch < 'a' || ch > 'f') && (ch < 'A' || ch > 'F'))
        return false;
    }
    return true;
  }

  private double ExtractOpacity(string opacity)
  {
    if (!opacity.EndsWith("f"))
      return Convert.ToDouble(opacity);
    opacity = this.RemoveCharUnSafeAt(opacity, true);
    return Convert.ToDouble(opacity) / 65536.0;
  }

  private string RemoveCharUnSafeAt(string source, bool isLast)
  {
    return !isLast ? source.Remove(0, 1) : source.Remove(source.Length - 1);
  }

  private ExcelGradientColor ExtractGradientColorType(XmlReader reader, out string color)
  {
    if (reader.MoveToAttribute("colors"))
    {
      color = reader.Value;
      return ExcelGradientColor.Preset;
    }
    if (reader.MoveToAttribute("color2"))
    {
      color = reader.Value;
      return color.StartsWith("fill") ? ExcelGradientColor.OneColor : ExcelGradientColor.TwoColor;
    }
    color = "fill darken(0)";
    return ExcelGradientColor.OneColor;
  }

  private double ExtractDegree(string degree)
  {
    double num = degree != null ? XmlConvertExtension.ToDouble(degree.Split('(')[1].Split(')')[0]) : throw new ArgumentNullException(nameof (degree));
    return !degree.Contains("fill lighten") ? (num - 0.5) / (double) byte.MaxValue : num / (double) byte.MaxValue;
  }

  private ExcelGradientPreset ExtractPreset(string preset)
  {
    ExcelGradientPreset[] excelGradientPresetArray = new ExcelGradientPreset[24]
    {
      ExcelGradientPreset.Grad_Early_Sunset,
      ExcelGradientPreset.Grad_Late_Sunset,
      ExcelGradientPreset.Grad_Nightfall,
      ExcelGradientPreset.Grad_Daybreak,
      ExcelGradientPreset.Grad_Horizon,
      ExcelGradientPreset.Grad_Desert,
      ExcelGradientPreset.Grad_Ocean,
      ExcelGradientPreset.Grad_Calm_Water,
      ExcelGradientPreset.Grad_Fire,
      ExcelGradientPreset.Grad_Fog,
      ExcelGradientPreset.Grad_Moss,
      ExcelGradientPreset.Grad_Peacock,
      ExcelGradientPreset.Grad_Wheat,
      ExcelGradientPreset.Grad_Parchment,
      ExcelGradientPreset.Grad_Mahogany,
      ExcelGradientPreset.Grad_Rainbow,
      ExcelGradientPreset.Grad_RainbowII,
      ExcelGradientPreset.Grad_Gold,
      ExcelGradientPreset.Grad_GoldII,
      ExcelGradientPreset.Grad_Brass,
      ExcelGradientPreset.Grad_Chrome,
      ExcelGradientPreset.Grad_ChromeII,
      ExcelGradientPreset.Grad_Silver,
      ExcelGradientPreset.Grad_Sapphire
    };
    ResourceManager resourceManager = new ResourceManager("Syncfusion.XlsIO.VMLPresetGradientFills", typeof (VmlTextBoxBaseParser).Assembly);
    int index = 0;
    for (int length = excelGradientPresetArray.Length; index < length; ++index)
    {
      if (resourceManager.GetString(excelGradientPresetArray[index].ToString()).Equals(preset))
        return excelGradientPresetArray[index];
    }
    throw new IndexOutOfRangeException("Presets");
  }

  private ExcelPattern ExtractLinePattern(long length)
  {
    throw new NotImplementedException("pattern");
  }

  private ExcelGradientVariants ExtractShadingVariant(string focus)
  {
    switch (Convert.ToInt32(this.RemoveCharUnSafeAt(focus, true)))
    {
      case -50:
        return ExcelGradientVariants.ShadingVariants_4;
      case 50:
        return ExcelGradientVariants.ShadingVariants_3;
      case 100:
        return ExcelGradientVariants.ShadingVariants_1;
      default:
        return ExcelGradientVariants.ShadingVariants_2;
    }
  }

  private ExcelTexture ExtractTexture(string title)
  {
    title = title.Replace(' ', '_');
    try
    {
      return (ExcelTexture) Enum.Parse(typeof (ExcelTexture), title, true);
    }
    catch (Exception ex)
    {
      return ExcelTexture.User_Defined;
    }
  }

  private ExcelGradientPattern ExtractPattern(string title)
  {
    title = "pat_" + title.Replace(' ', '_');
    try
    {
      return (ExcelGradientPattern) Enum.Parse(typeof (ExcelGradientPattern), title, true);
    }
    catch (Exception ex)
    {
      return ExcelGradientPattern.Pat_10_Percent;
    }
  }

  private ExcelShapeDashLineStyle ExtractDashStyle(string dashStyle)
  {
    if (VmlTextBoxBaseParser.m_excelDashLineStyle == null)
      VmlTextBoxBaseParser.InitDashLineStyle();
    if (dashStyle.Length == 3 && dashStyle.Trim().Equals("1 1"))
      return VmlTextBoxBaseParser.m_excelDashLineStyle[dashStyle];
    if (char.IsDigit(dashStyle[0]))
      return ExcelShapeDashLineStyle.Dotted_Round;
    return dashStyle == "roundDot" || !VmlTextBoxBaseParser.m_excelDashLineStyle.ContainsKey(dashStyle) ? ExcelShapeDashLineStyle.Solid : VmlTextBoxBaseParser.m_excelDashLineStyle[dashStyle];
  }

  private ExcelShapeLineStyle ExtractLineStyle(string lineStyle)
  {
    if (VmlTextBoxBaseParser.m_excelShapeLineStyle == null)
      VmlTextBoxBaseParser.InitShapeLineStyle();
    return VmlTextBoxBaseParser.m_excelShapeLineStyle[lineStyle];
  }
}
