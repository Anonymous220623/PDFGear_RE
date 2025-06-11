// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlReaders.Shapes.OptionButtonShapeParser
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

internal class OptionButtonShapeParser : VmlTextBoxBaseParser
{
  public override ShapeImpl ParseShapeType(XmlReader reader, ShapeCollectionBase shapes)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (shapes == null)
      throw new ArgumentNullException(nameof (shapes));
    reader.Skip();
    return (ShapeImpl) (shapes.Application as ApplicationImpl).CreateOptionButtonShapeImpl((object) (shapes as ShapesCollection));
  }

  protected override void ParseUnknownClientDataTag(XmlReader reader, TextBoxShapeBase textBox)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (textBox == null)
      throw new ArgumentNullException("shapes");
    if (reader.NodeType == XmlNodeType.Element)
    {
      OptionButtonShapeImpl optionButtonShapeImpl = textBox as OptionButtonShapeImpl;
      optionButtonShapeImpl.Display3DShading = true;
      switch (reader.LocalName)
      {
        case "Checked":
          optionButtonShapeImpl.CheckState = (ExcelCheckState) reader.ReadElementContentAsInt();
          break;
        case "FmlaLink":
          IWorksheet worksheet = optionButtonShapeImpl.Worksheet as IWorksheet;
          WorkbookImpl parentWorkbook = optionButtonShapeImpl.ParentWorkbook;
          string formula = reader.ReadElementContentAsString();
          if (formula.Contains(":"))
            formula = formula.Substring(0, formula.IndexOf(':'));
          optionButtonShapeImpl.LinkedCell = ChartParser.GetRange(parentWorkbook, formula, worksheet);
          break;
        case "FirstButton":
          optionButtonShapeImpl.IsFirstButton = true;
          break;
        case "NoThreeD":
          optionButtonShapeImpl.Display3DShading = false;
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
    if (textBox == null)
      throw new ArgumentNullException("shapes");
    base.RegisterShape(textBox);
    textBox.Worksheet.TypedOptionButtons.AddOptionButton(textBox as IOptionButtonShape);
  }

  internal override ShapeImpl CreateShape(ShapeCollectionBase shapes)
  {
    return (ShapeImpl) (shapes.Application as ApplicationImpl).CreateOptionButtonShapeImpl((object) (shapes as ShapesCollection));
  }
}
