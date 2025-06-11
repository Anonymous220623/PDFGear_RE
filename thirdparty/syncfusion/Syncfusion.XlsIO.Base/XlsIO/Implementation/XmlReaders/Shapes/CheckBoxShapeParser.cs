// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlReaders.Shapes.CheckBoxShapeParser
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Implementation.XmlSerialization.Charts;
using System;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlReaders.Shapes;

internal class CheckBoxShapeParser : VmlTextBoxBaseParser
{
  public override ShapeImpl ParseShapeType(XmlReader reader, ShapeCollectionBase shapes)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (shapes == null)
      throw new ArgumentNullException(nameof (shapes));
    reader.Skip();
    CheckBoxShapeImpl checkBoxShapeImpl = (shapes.Application as ApplicationImpl).CreateCheckBoxShapeImpl((object) (shapes as ShapesCollection));
    checkBoxShapeImpl.Display3DShading = true;
    return (ShapeImpl) checkBoxShapeImpl;
  }

  protected override void ParseUnknownClientDataTag(XmlReader reader, TextBoxShapeBase textBox)
  {
    if (reader.NodeType == XmlNodeType.Element)
    {
      CheckBoxShapeImpl checkBoxShapeImpl = textBox as CheckBoxShapeImpl;
      switch (reader.LocalName)
      {
        case "Checked":
          checkBoxShapeImpl.CheckState = (ExcelCheckState) reader.ReadElementContentAsInt();
          break;
        case "FmlaLink":
          IWorksheet worksheet = checkBoxShapeImpl.Worksheet as IWorksheet;
          WorkbookImpl parentWorkbook = checkBoxShapeImpl.ParentWorkbook;
          string formula = reader.ReadElementContentAsString();
          if (formula.Contains(":"))
            formula = formula.Substring(0, formula.IndexOf(':'));
          checkBoxShapeImpl.LinkedCell = ChartParser.GetRange(parentWorkbook, formula, worksheet);
          break;
        case "NoThreeD":
          checkBoxShapeImpl.Display3DShading = false;
          break;
        default:
          reader.Skip();
          break;
      }
    }
    else
      reader.Skip();
  }

  protected override void RegisterShape(TextBoxShapeBase textBox)
  {
    base.RegisterShape(textBox);
    textBox.Worksheet.TypedCheckBoxes.AddCheckBox(textBox as ICheckBoxShape);
  }

  internal override ShapeImpl CreateShape(ShapeCollectionBase shapes)
  {
    CheckBoxShapeImpl checkBoxShapeImpl = (shapes.Application as ApplicationImpl).CreateCheckBoxShapeImpl((object) (shapes as ShapesCollection));
    checkBoxShapeImpl.Display3DShading = true;
    return (ShapeImpl) checkBoxShapeImpl;
  }
}
