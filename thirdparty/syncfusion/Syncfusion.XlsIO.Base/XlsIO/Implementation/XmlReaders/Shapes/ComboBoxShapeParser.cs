// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlReaders.Shapes.ComboBoxShapeParser
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Implementation.XmlSerialization;
using Syncfusion.XlsIO.Implementation.XmlSerialization.Charts;
using Syncfusion.XlsIO.Parser.Biff_Records.Formula;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlReaders.Shapes;

internal class ComboBoxShapeParser : ShapeParser
{
  private const string REF_ERROR = "#REF!";

  public override ShapeImpl ParseShapeType(XmlReader reader, ShapeCollectionBase shapes)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (shapes == null)
      throw new ArgumentNullException(nameof (shapes));
    reader.Skip();
    return (ShapeImpl) (shapes.Application as ApplicationImpl).CreateComboBoxShapeImpl((object) shapes);
  }

  public override bool ParseShape(
    XmlReader reader,
    ShapeImpl defaultShape,
    RelationCollection relations,
    string parentItemPath)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    ComboBoxShapeImpl comboBoxShapeImpl = defaultShape != null ? (ComboBoxShapeImpl) defaultShape.Clone(defaultShape.Parent, (Dictionary<string, string>) null, (Dictionary<int, int>) null, false) : throw new ArgumentNullException(nameof (defaultShape));
    VmlTextBoxBaseParser.ParseShapeId(reader, (ShapeImpl) comboBoxShapeImpl);
    if (reader.MoveToAttribute("style"))
    {
      this.ParseStyle(reader.Value, (ShapeImpl) comboBoxShapeImpl);
      reader.MoveToElement();
    }
    bool shape = false;
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      shape = true;
      while (reader.NodeType != XmlNodeType.EndElement && shape)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "ClientData":
              shape = this.ParseClientData(reader, comboBoxShapeImpl);
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
    if (shape)
    {
      if (comboBoxShapeImpl.Worksheet.InnerShapes.GetShapeById(comboBoxShapeImpl.ShapeId) is ShapeImpl shapeById && shapeById.Group != null)
      {
        int index = Array.IndexOf<IShape>(shapeById.Group.Items, (IShape) shapeById);
        shapeById.Group.Items[index] = (IShape) comboBoxShapeImpl;
      }
      else
        this.RegisterShape(comboBoxShapeImpl);
    }
    return shape;
  }

  private void ParseStyle(string strStyle, ShapeImpl shape)
  {
    Dictionary<string, string> dictStyles = this.SplitStyle(strStyle);
    shape.Left = (int) this.GetValue(dictStyles, "margin-left");
    shape.Top = (int) this.GetValue(dictStyles, "margin-top");
    shape.Width = (int) this.GetValue(dictStyles, "width");
    shape.Height = (int) this.GetValue(dictStyles, "height");
    if (!dictStyles.ContainsKey("visibility") || !(dictStyles["visibility"] == "hidden"))
      return;
    shape.IsShapeVisible = false;
  }

  private double GetValue(Dictionary<string, string> dictStyles, string tagName)
  {
    double num = 0.0;
    string s;
    if (dictStyles.TryGetValue(tagName, out s))
    {
      MeasureUnits from = MeasureUnits.Pixel;
      if (s.Length >= 2)
      {
        string str = s.Substring(s.Length - 2);
        if (!char.IsNumber(str[1]))
          s = s.Substring(0, s.Length - 2);
        switch (str)
        {
          case "mm":
            from = MeasureUnits.Millimeter;
            break;
        }
      }
      num = ApplicationImpl.ConvertToPixels(double.Parse(s, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture), from);
    }
    return num;
  }

  private bool ParseClientData(XmlReader reader, ComboBoxShapeImpl comboBox)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (comboBox == null)
      throw new ArgumentNullException(nameof (comboBox));
    if (reader.LocalName != "ClientData")
      throw new XmlException("Unexpected xml token");
    if (reader.MoveToAttribute("ObjectType") && reader.Value != "Drop")
      return false;
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      WorkbookImpl parentWorkbook = comboBox.ParentWorkbook;
      if (!(comboBox.Worksheet is IWorksheet worksheet1) && parentWorkbook.Worksheets.Count > 0)
        worksheet1 = parentWorkbook.Worksheets[0];
      comboBox.Display3DShading = true;
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "FmlaLink":
              IWorksheet worksheet2 = comboBox.Worksheet as IWorksheet;
              string formula = reader.ReadElementContentAsString();
              if (formula.Contains(":"))
                formula = formula.Substring(0, formula.IndexOf(':'));
              comboBox.LinkedCell = ChartParser.GetRange(parentWorkbook, formula, worksheet2);
              continue;
            case "FmlaRange":
              string strFormula = reader.ReadElementContentAsString();
              if (strFormula != "#REF!")
              {
                IRangeGetter rangeGetter = parentWorkbook.DataHolder.Parser.FormulaUtil.ParseString(strFormula)[0] as IRangeGetter;
                comboBox.ListFillRange = rangeGetter.GetRange((IWorkbook) parentWorkbook, worksheet1);
                continue;
              }
              continue;
            case "Sel":
              comboBox.SelectedIndex = reader.ReadElementContentAsInt();
              continue;
            case "Anchor":
              if (comboBox.Worksheet is WorksheetImpl)
              {
                this.ParseAnchor(reader, (ShapeImpl) comboBox);
                continue;
              }
              break;
            case "DropLines":
              comboBox.DropDownLines = reader.ReadElementContentAsInt();
              continue;
            case "NoThreeD2":
              comboBox.Display3DShading = false;
              reader.Read();
              continue;
            case "FmlaMacro":
              comboBox.FormulaMacro = reader.ReadElementContentAsString();
              comboBox.OnAction = comboBox.FormulaMacro;
              continue;
            case "PrintObject":
              comboBox.PrintWithSheet = false;
              reader.Read();
              continue;
          }
          reader.Skip();
        }
        else
          reader.Read();
      }
    }
    reader.Read();
    return true;
  }

  protected virtual void RegisterShape(ComboBoxShapeImpl comboBox)
  {
    WorksheetBaseImpl worksheetBaseImpl = comboBox != null ? comboBox.Worksheet : throw new ArgumentNullException(nameof (comboBox));
    worksheetBaseImpl.InnerShapes.AddShape((ShapeImpl) comboBox);
    worksheetBaseImpl.TypedComboBoxes.AddComboBox((IComboBoxShape) comboBox);
  }

  internal override ShapeImpl CreateShape(ShapeCollectionBase shapes)
  {
    return (ShapeImpl) (shapes.Application as ApplicationImpl).CreateComboBoxShapeImpl((object) shapes);
  }
}
