// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.MathMLParser
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml;

#nullable disable
namespace Syncfusion.Office;

internal class MathMLParser
{
  private DocumentParser m_documentParser;
  private Regex m_isFloatValue = new Regex("^.*\\d+(\\.*\\d+)*$");
  private Regex m_hasAlphabet = new Regex("^[^ A-Za-z_@/#&+-]*$");

  internal MathMLParser()
  {
  }

  internal void ParseMathPara(
    XmlReader reader,
    IOfficeMathParagraph mathPara,
    DocumentParser documentParser)
  {
    this.m_documentParser = documentParser;
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (reader.LocalName != localName)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "oMath":
            OfficeMath officeMath = mathPara.Maths.Add(mathPara.Maths.Count) as OfficeMath;
            this.ParseMath(reader, officeMath);
            break;
          case "oMathParaPr":
            this.ParseMathParaProperties(reader, mathPara);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  internal void ParseMath(XmlReader reader, OfficeMath officeMath, DocumentParser documentParser)
  {
    this.m_documentParser = documentParser;
    this.ParseMath(reader, officeMath);
  }

  internal void ParseMath(XmlReader reader, OfficeMath officeMath)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (reader.LocalName != localName)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "f":
            OfficeMathFraction mathFraction = officeMath.Functions.Add(officeMath.Functions.Count, MathFunctionType.Fraction) as OfficeMathFraction;
            this.ParseMathFraction(reader, mathFraction);
            break;
          case "func":
            OfficeMathFunction mathFunc = officeMath.Functions.Add(officeMath.Functions.Count, MathFunctionType.Function) as OfficeMathFunction;
            this.ParseMathFunc(reader, mathFunc);
            break;
          case "r":
            OfficeMathRunElement mathParaItem = officeMath.Functions.Add(officeMath.Functions.Count, MathFunctionType.RunElement) as OfficeMathRunElement;
            this.m_documentParser.ParseMathRun(reader, (IOfficeMathRunElement) mathParaItem);
            break;
          case "box":
            OfficeMathBox mathBox = officeMath.Functions.Add(officeMath.Functions.Count, MathFunctionType.Box) as OfficeMathBox;
            this.ParseMathBox(reader, mathBox);
            break;
          case "borderBox":
            OfficeMathBorderBox mathBorderBox = officeMath.Functions.Add(officeMath.Functions.Count, MathFunctionType.BorderBox) as OfficeMathBorderBox;
            this.ParseMathBorderBox(reader, mathBorderBox);
            break;
          case "d":
            OfficeMathDelimiter mathDelimiter = officeMath.Functions.Add(officeMath.Functions.Count, MathFunctionType.Delimiter) as OfficeMathDelimiter;
            this.ParseMathDelimiter(reader, mathDelimiter);
            break;
          case "acc":
            OfficeMathAccent mathAccent = officeMath.Functions.Add(officeMath.Functions.Count, MathFunctionType.Accent) as OfficeMathAccent;
            this.ParseMathAccent(reader, mathAccent);
            break;
          case "bar":
            OfficeMathBar mathBar = officeMath.Functions.Add(officeMath.Functions.Count, MathFunctionType.Bar) as OfficeMathBar;
            this.ParseMathBar(reader, mathBar);
            break;
          case "groupChr":
            OfficeMathGroupCharacter mathGroupChar = officeMath.Functions.Add(officeMath.Functions.Count, MathFunctionType.GroupCharacter) as OfficeMathGroupCharacter;
            this.ParseMathGroupChar(reader, mathGroupChar);
            break;
          case "eqArr":
            OfficeMathEquationArray mathEqArray = officeMath.Functions.Add(officeMath.Functions.Count, MathFunctionType.EquationArray) as OfficeMathEquationArray;
            this.ParseMathEqArray(reader, mathEqArray);
            break;
          case "sSub":
            OfficeMathScript mathScript1 = officeMath.Functions.Add(officeMath.Functions.Count, MathFunctionType.SubSuperscript) as OfficeMathScript;
            mathScript1.ScriptType = MathScriptType.Subscript;
            this.ParseMathScript(reader, mathScript1);
            break;
          case "sSup":
            OfficeMathScript mathScript2 = officeMath.Functions.Add(officeMath.Functions.Count, MathFunctionType.SubSuperscript) as OfficeMathScript;
            mathScript2.ScriptType = MathScriptType.Superscript;
            this.ParseMathScript(reader, mathScript2);
            break;
          case "sPre":
            OfficeMathLeftScript mathLeftScript = officeMath.Functions.Add(officeMath.Functions.Count, MathFunctionType.LeftSubSuperscript) as OfficeMathLeftScript;
            this.ParseMathLeftScript(reader, mathLeftScript);
            break;
          case "sSubSup":
            OfficeMathRightScript mathRightScript = officeMath.Functions.Add(officeMath.Functions.Count, MathFunctionType.RightSubSuperscript) as OfficeMathRightScript;
            this.ParseMathRightScript(reader, mathRightScript);
            break;
          case "rad":
            OfficeMathRadical mathRadical = officeMath.Functions.Add(officeMath.Functions.Count, MathFunctionType.Radical) as OfficeMathRadical;
            this.ParseMathRadical(reader, mathRadical);
            break;
          case "nary":
            OfficeMathNArray mathNAry = officeMath.Functions.Add(officeMath.Functions.Count, MathFunctionType.NArray) as OfficeMathNArray;
            this.ParseMathNAry(reader, mathNAry);
            break;
          case "m":
            OfficeMathMatrix mathMatrix = officeMath.Functions.Add(officeMath.Functions.Count, MathFunctionType.Matrix) as OfficeMathMatrix;
            this.ParseMathMatrix(reader, mathMatrix);
            if (mathMatrix != null)
            {
              mathMatrix.UpdateColumns();
              mathMatrix.ApplyColumnProperties();
              break;
            }
            break;
          case "phant":
            OfficeMathPhantom mathPhantom = officeMath.Functions.Add(officeMath.Functions.Count, MathFunctionType.Phantom) as OfficeMathPhantom;
            this.ParseMathPhantom(reader, mathPhantom);
            break;
          case "limLow":
            OfficeMathLimit mathLimit1 = officeMath.Functions.Add(officeMath.Functions.Count, MathFunctionType.Limit) as OfficeMathLimit;
            mathLimit1.LimitType = MathLimitType.LowerLimit;
            this.ParseMathLimit(reader, mathLimit1);
            break;
          case "limUpp":
            OfficeMathLimit mathLimit2 = officeMath.Functions.Add(officeMath.Functions.Count, MathFunctionType.Limit) as OfficeMathLimit;
            mathLimit2.LimitType = MathLimitType.UpperLimit;
            this.ParseMathLimit(reader, mathLimit2);
            break;
          case "argPr":
            this.ParseMathArgumentProperties(reader, officeMath);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  private void ParseMathArgumentProperties(XmlReader reader, OfficeMath officeMath)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (reader.LocalName != localName)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "argSz":
            string attribute = reader.GetAttribute("val", reader.NamespaceURI);
            if (!string.IsNullOrEmpty(attribute))
            {
              officeMath.ArgumentSize = (int) this.GetNumericValue(attribute);
              break;
            }
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  internal void ParseMathProperties(XmlReader reader, OfficeMathProperties mathProperties)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (reader.LocalName != localName)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "mathFont":
            string attribute1 = reader.GetAttribute("val", reader.NamespaceURI);
            if (!string.IsNullOrEmpty(attribute1))
            {
              mathProperties.MathFont = attribute1;
              break;
            }
            break;
          case "brkBin":
            this.ParseBreakOnBinaryOperator(reader, mathProperties);
            break;
          case "brkBinSub":
            this.ParseBreakOnSubtractOperator(reader, mathProperties);
            break;
          case "smallFrac":
            mathProperties.SmallFraction = this.GetBooleanValue(reader);
            break;
          case "dispDef":
            mathProperties.DisplayMathDefaults = this.GetBooleanValue(reader);
            break;
          case "lMargin":
            string attribute2 = reader.GetAttribute("val", reader.NamespaceURI);
            if (!string.IsNullOrEmpty(attribute2))
            {
              mathProperties.LeftMargin = (int) Math.Round((double) this.GetFloatValue(attribute2, reader.LocalName));
              break;
            }
            break;
          case "rMargin":
            string attribute3 = reader.GetAttribute("val", reader.NamespaceURI);
            if (!string.IsNullOrEmpty(attribute3))
            {
              mathProperties.RightMargin = (int) Math.Round((double) this.GetFloatValue(attribute3, reader.LocalName));
              break;
            }
            break;
          case "defJc":
            this.ParseDefaultJustification(reader, mathProperties);
            break;
          case "wrapIndent":
            string attribute4 = reader.GetAttribute("val", reader.NamespaceURI);
            if (!string.IsNullOrEmpty(attribute4))
            {
              mathProperties.WrapIndent = (int) Math.Round((double) this.GetFloatValue(attribute4, reader.LocalName));
              break;
            }
            break;
          case "wrapRight":
            mathProperties.WrapRight = this.GetBooleanValue(reader);
            break;
          case "intLim":
            this.ParseLimitLocationType(reader, mathProperties);
            break;
          case "naryLim":
            this.ParseNArtLimitLocationType(reader, mathProperties);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  private void ParseNArtLimitLocationType(XmlReader reader, OfficeMathProperties mathProperties)
  {
    switch (reader.GetAttribute("val", reader.NamespaceURI))
    {
      case "subSup":
        mathProperties.NAryLimitLocation = LimitLocationType.SubSuperscript;
        break;
      case "undOvr":
        mathProperties.NAryLimitLocation = LimitLocationType.UnderOver;
        break;
    }
  }

  private void ParseLimitLocationType(XmlReader reader, OfficeMathProperties mathProperties)
  {
    switch (reader.GetAttribute("val", reader.NamespaceURI))
    {
      case "undOvr":
        mathProperties.IntegralLimitLocations = LimitLocationType.UnderOver;
        break;
      case "subSup":
        mathProperties.IntegralLimitLocations = LimitLocationType.SubSuperscript;
        break;
    }
  }

  private void ParseDefaultJustification(XmlReader reader, OfficeMathProperties mathProperties)
  {
    switch (reader.GetAttribute("val", reader.NamespaceURI))
    {
      case "center":
        mathProperties.DefaultJustification = MathJustification.Center;
        break;
      case "left":
        mathProperties.DefaultJustification = MathJustification.Left;
        break;
      case "right":
        mathProperties.DefaultJustification = MathJustification.Right;
        break;
      case "centerGroup":
        mathProperties.DefaultJustification = MathJustification.CenterGroup;
        break;
    }
  }

  private void ParseBreakOnSubtractOperator(XmlReader reader, OfficeMathProperties mathProperties)
  {
    switch (reader.GetAttribute("val", reader.NamespaceURI))
    {
      case "+-":
        mathProperties.BreakOnBinarySubtraction = BreakOnBinarySubtractionType.PlusMinus;
        break;
      case "-+":
        mathProperties.BreakOnBinarySubtraction = BreakOnBinarySubtractionType.MinusPlus;
        break;
      case "--":
        mathProperties.BreakOnBinarySubtraction = BreakOnBinarySubtractionType.MinusMinus;
        break;
    }
  }

  private void ParseBreakOnBinaryOperator(XmlReader reader, OfficeMathProperties mathProperties)
  {
    switch (reader.GetAttribute("val", reader.NamespaceURI))
    {
      case "after":
        mathProperties.BreakOnBinaryOperators = BreakOnBinaryOperatorsType.After;
        break;
      case "repeat":
        mathProperties.BreakOnBinaryOperators = BreakOnBinaryOperatorsType.Repeat;
        break;
      case "before":
        mathProperties.BreakOnBinaryOperators = BreakOnBinaryOperatorsType.Before;
        break;
    }
  }

  private void ParseMathLimit(XmlReader reader, OfficeMathLimit mathLimit)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (reader.LocalName != localName)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "limLowPr":
          case "limUppPr":
            this.ParseMathLimitProperties(reader, mathLimit);
            break;
          case "e":
            this.ParseMath(reader, mathLimit.Equation as OfficeMath);
            break;
          case "lim":
            this.ParseMath(reader, mathLimit.Limit as OfficeMath);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  private void ParseMathLimitProperties(XmlReader reader, OfficeMathLimit mathLimit)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (!(reader.LocalName == localName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "ctrlPr":
            mathLimit.ControlProperties = this.ParseMathControlProperties(reader, (OfficeMathFunctionBase) mathLimit, mathLimit.ControlProperties);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  private void ParseMathPhantom(XmlReader reader, OfficeMathPhantom mathPhantom)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (reader.LocalName != localName)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "phantPr":
            this.ParseMathPhantomProperties(reader, mathPhantom);
            break;
          case "e":
            this.ParseMath(reader, mathPhantom.Equation as OfficeMath);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  private void ParseMathPhantomProperties(XmlReader reader, OfficeMathPhantom mathPhantom)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (!(reader.LocalName == localName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "transp":
            mathPhantom.Transparent = this.GetBooleanValue(reader);
            break;
          case "zeroDesc":
            mathPhantom.ZeroDescent = this.GetBooleanValue(reader);
            break;
          case "zeroAsc":
            mathPhantom.ZeroAscent = this.GetBooleanValue(reader);
            break;
          case "zeroWid":
            mathPhantom.ZeroWidth = this.GetBooleanValue(reader);
            break;
          case "show":
            mathPhantom.Show = this.GetBooleanValue(reader);
            break;
          case "ctrlPr":
            mathPhantom.ControlProperties = this.ParseMathControlProperties(reader, (OfficeMathFunctionBase) mathPhantom, mathPhantom.ControlProperties);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  private void ParseMathMatrix(XmlReader reader, OfficeMathMatrix mathMatrix)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (reader.LocalName != localName)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "mPr":
            this.ParseMathMatrixProperties(reader, mathMatrix);
            break;
          case "mr":
            OfficeMathMatrixRow mathMatrixRow = mathMatrix.Rows.Add(mathMatrix.Rows.Count) as OfficeMathMatrixRow;
            this.ParseMathMatrixRow(reader, mathMatrixRow);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  private void ParseMathMatrixProperties(XmlReader reader, OfficeMathMatrix mathMatrix)
  {
    string str1 = string.Empty;
    string str2 = string.Empty;
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (!(reader.LocalName == localName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "baseJc":
            this.ParseMathMatrixJustification(reader, mathMatrix);
            break;
          case "plcHide":
            mathMatrix.HidePlaceHolders = this.GetBooleanValue(reader);
            break;
          case "rSpRule":
            mathMatrix.RowSpacingRule = this.ParseSpacingRule(reader);
            break;
          case "cSp":
            string attribute = reader.GetAttribute("val", reader.NamespaceURI);
            if (!string.IsNullOrEmpty(attribute))
            {
              float num = (float) Math.Round((double) this.GetFloatValue(attribute, reader.LocalName), 2);
              mathMatrix.SetPropertyValue(34, (object) num);
              break;
            }
            break;
          case "cGpRule":
            mathMatrix.ColumnSpacingRule = this.ParseSpacingRule(reader);
            break;
          case "rSp":
            str1 = reader.GetAttribute("val", reader.NamespaceURI);
            break;
          case "cGp":
            str2 = reader.GetAttribute("val", reader.NamespaceURI);
            break;
          case "mcs":
            this.ParserMathColumns(reader, mathMatrix);
            break;
          case "ctrlPr":
            mathMatrix.ControlProperties = this.ParseMathControlProperties(reader, (OfficeMathFunctionBase) mathMatrix, mathMatrix.ControlProperties);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
    if (!string.IsNullOrEmpty(str1) && (mathMatrix.RowSpacingRule == SpacingRule.Exactly || mathMatrix.RowSpacingRule == SpacingRule.Multiple))
    {
      float num = (float) Math.Round((double) this.GetSpacingValue(str1, "rSp", mathMatrix.RowSpacingRule), 2);
      mathMatrix.SetPropertyValue(27, (object) num);
    }
    if (string.IsNullOrEmpty(str2) || mathMatrix.ColumnSpacingRule != SpacingRule.Exactly && mathMatrix.ColumnSpacingRule != SpacingRule.Multiple)
      return;
    float num1 = mathMatrix.ColumnSpacingRule != SpacingRule.Multiple ? (float) Math.Round((double) this.GetSpacingValue(str2, "cGp", mathMatrix.ColumnSpacingRule), 2) : (float) Math.Floor((double) this.GetSpacingValue(str2, "cGp", mathMatrix.ColumnSpacingRule));
    mathMatrix.SetPropertyValue(36, (object) num1);
  }

  private void ParserMathColumns(XmlReader reader, OfficeMathMatrix mathMatrix)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (reader.LocalName != localName)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "mc":
            MatrixColumnProperties mathMatrixColumnProperties = new MatrixColumnProperties((IOfficeMathEntity) mathMatrix);
            this.ParseMathMatrixColumn(reader, mathMatrixColumnProperties);
            mathMatrix.ColumnProperties.Add(mathMatrixColumnProperties);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  private void ParseMathMatrixColumn(
    XmlReader reader,
    MatrixColumnProperties mathMatrixColumnProperties)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (reader.LocalName != localName)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "mcPr":
            this.ParseMathMatrixColumnProperties(reader, mathMatrixColumnProperties);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  private void ParseMathMatrixColumnProperties(
    XmlReader reader,
    MatrixColumnProperties mathMatrixColumnProperties)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (!(reader.LocalName == localName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "mcJc":
            this.ParseMathMatrixColumnJustification(reader, mathMatrixColumnProperties);
            break;
          case "count":
            string attribute = reader.GetAttribute("val", reader.NamespaceURI);
            if (!string.IsNullOrEmpty(attribute))
            {
              mathMatrixColumnProperties.Count = (int) this.GetNumericValue(attribute);
              break;
            }
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  private void ParseMathMatrixColumnJustification(
    XmlReader reader,
    MatrixColumnProperties mathMatrixColumnProperties)
  {
    switch (reader.GetAttribute("val", reader.NamespaceURI))
    {
      case "left":
        mathMatrixColumnProperties.Alignment = MathHorizontalAlignment.Left;
        break;
      case "right":
        mathMatrixColumnProperties.Alignment = MathHorizontalAlignment.Right;
        break;
      case "center":
        mathMatrixColumnProperties.Alignment = MathHorizontalAlignment.Center;
        break;
    }
  }

  private SpacingRule ParseSpacingRule(XmlReader reader)
  {
    switch (reader.GetAttribute("val", reader.NamespaceURI))
    {
      case "0":
        return SpacingRule.Single;
      case "1":
        return SpacingRule.OneAndHalf;
      case "2":
        return SpacingRule.Double;
      case "3":
        return SpacingRule.Exactly;
      case "4":
        return SpacingRule.Multiple;
      default:
        return SpacingRule.Single;
    }
  }

  private void ParseMathMatrixJustification(XmlReader reader, OfficeMathMatrix mathMatrix)
  {
    switch (reader.GetAttribute("val", reader.NamespaceURI).ToLower())
    {
      case "top":
        mathMatrix.VerticalAlignment = MathVerticalAlignment.Top;
        break;
      case "bottom":
        mathMatrix.VerticalAlignment = MathVerticalAlignment.Bottom;
        break;
      case "center":
        mathMatrix.VerticalAlignment = MathVerticalAlignment.Center;
        break;
    }
  }

  private void ParseMathMatrixRow(XmlReader reader, OfficeMathMatrixRow mathMatrixRow)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (reader.LocalName != localName)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "e":
            OfficeMath officeMath = new OfficeMath((IOfficeMathEntity) mathMatrixRow);
            this.ParseMath(reader, officeMath);
            mathMatrixRow.m_args.InnerList.Add((object) officeMath);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  private void ParseMathNAry(XmlReader reader, OfficeMathNArray mathNAry)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (reader.LocalName != localName)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "naryPr":
            this.ParseMathNAryProperties(reader, mathNAry);
            break;
          case "e":
            this.ParseMath(reader, mathNAry.Equation as OfficeMath);
            break;
          case "sub":
            this.ParseMath(reader, mathNAry.Subscript as OfficeMath);
            break;
          case "sup":
            this.ParseMath(reader, mathNAry.Superscript as OfficeMath);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  private void ParseMathNAryProperties(XmlReader reader, OfficeMathNArray mathNAry)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (!(reader.LocalName == localName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "chr":
            string attribute1 = reader.GetAttribute("val", reader.NamespaceURI);
            if (!string.IsNullOrEmpty(attribute1))
            {
              mathNAry.NArrayCharacter = attribute1;
              break;
            }
            break;
          case "limLoc":
            string attribute2 = reader.GetAttribute("val", reader.NamespaceURI);
            if (!string.IsNullOrEmpty(attribute2))
            {
              mathNAry.SubSuperscriptLimit = attribute2.ToLower() == "subsup";
              break;
            }
            break;
          case "grow":
            mathNAry.HasGrow = this.GetBooleanValue(reader);
            break;
          case "subHide":
            mathNAry.HideLowerLimit = this.GetBooleanValue(reader);
            break;
          case "supHide":
            mathNAry.HideUpperLimit = this.GetBooleanValue(reader);
            break;
          case "ctrlPr":
            mathNAry.ControlProperties = this.ParseMathControlProperties(reader, (OfficeMathFunctionBase) mathNAry, mathNAry.ControlProperties);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  private void ParseMathRadical(XmlReader reader, OfficeMathRadical mathRadical)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (reader.LocalName != localName)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "radPr":
            this.ParseMathRadicalProperties(reader, mathRadical);
            break;
          case "e":
            this.ParseMath(reader, mathRadical.Equation as OfficeMath);
            break;
          case "deg":
            this.ParseMath(reader, mathRadical.Degree as OfficeMath);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  private void ParseMathRadicalProperties(XmlReader reader, OfficeMathRadical mathRadical)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (!(reader.LocalName == localName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "degHide":
            mathRadical.HideDegree = this.GetBooleanValue(reader);
            break;
          case "ctrlPr":
            mathRadical.ControlProperties = this.ParseMathControlProperties(reader, (OfficeMathFunctionBase) mathRadical, mathRadical.ControlProperties);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  private void ParseMathRightScript(XmlReader reader, OfficeMathRightScript mathRightScript)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (reader.LocalName != localName)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "sSubSupPr":
            this.ParseMathRightScriptProperties(reader, mathRightScript);
            break;
          case "e":
            this.ParseMath(reader, mathRightScript.Equation as OfficeMath);
            break;
          case "sup":
            this.ParseMath(reader, mathRightScript.Superscript as OfficeMath);
            break;
          case "sub":
            this.ParseMath(reader, mathRightScript.Subscript as OfficeMath);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  private void ParseMathRightScriptProperties(
    XmlReader reader,
    OfficeMathRightScript mathRightScript)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (!(reader.LocalName == localName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "alnScr":
            mathRightScript.IsSkipAlign = this.GetBooleanValue(reader);
            break;
          case "ctrlPr":
            mathRightScript.ControlProperties = this.ParseMathControlProperties(reader, (OfficeMathFunctionBase) mathRightScript, mathRightScript.ControlProperties);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  private void ParseMathLeftScript(XmlReader reader, OfficeMathLeftScript mathLeftScript)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (reader.LocalName != localName)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "sPrePr":
            this.ParseMathLeftScriptProperties(reader, mathLeftScript);
            break;
          case "e":
            this.ParseMath(reader, mathLeftScript.Equation as OfficeMath);
            break;
          case "sup":
            this.ParseMath(reader, mathLeftScript.Superscript as OfficeMath);
            break;
          case "sub":
            this.ParseMath(reader, mathLeftScript.Subscript as OfficeMath);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  private void ParseMathLeftScriptProperties(XmlReader reader, OfficeMathLeftScript mathLeftScript)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (!(reader.LocalName == localName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "ctrlPr":
            mathLeftScript.ControlProperties = this.ParseMathControlProperties(reader, (OfficeMathFunctionBase) mathLeftScript, mathLeftScript.ControlProperties);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  private void ParseMathScript(XmlReader reader, OfficeMathScript mathScript)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (reader.LocalName != localName)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "sSupPr":
          case "sSubPr":
            this.ParseMathScriptProperties(reader, mathScript);
            break;
          case "e":
            this.ParseMath(reader, mathScript.Equation as OfficeMath);
            break;
          case "sub":
          case "sup":
            this.ParseMath(reader, mathScript.Script as OfficeMath);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  private void ParseMathScriptProperties(XmlReader reader, OfficeMathScript mathScript)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (!(reader.LocalName == localName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "ctrlPr":
            mathScript.ControlProperties = this.ParseMathControlProperties(reader, (OfficeMathFunctionBase) mathScript, mathScript.ControlProperties);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  private void ParseMathEqArray(XmlReader reader, OfficeMathEquationArray mathEqArray)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (reader.LocalName != localName)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "eqArrPr":
            this.ParseMathEqArrayProperties(reader, mathEqArray);
            break;
          case "e":
            OfficeMath officeMath = new OfficeMath((IOfficeMathEntity) mathEqArray);
            this.ParseMath(reader, officeMath);
            mathEqArray.m_equation.InnerList.Add((object) officeMath);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  private void ParseMathEqArrayProperties(XmlReader reader, OfficeMathEquationArray mathEqArray)
  {
    string str = string.Empty;
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (!(reader.LocalName == localName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "baseJc":
            this.ParseMathEqArrayJustification(reader, mathEqArray);
            break;
          case "maxDist":
            mathEqArray.ExpandEquationContainer = this.GetBooleanValue(reader);
            break;
          case "objDist":
            mathEqArray.ExpandEquationContent = this.GetBooleanValue(reader);
            break;
          case "rSp":
            str = reader.GetAttribute("val", reader.NamespaceURI);
            break;
          case "rSpRule":
            mathEqArray.RowSpacingRule = this.ParseSpacingRule(reader);
            break;
          case "ctrlPr":
            mathEqArray.ControlProperties = this.ParseMathControlProperties(reader, (OfficeMathFunctionBase) mathEqArray, mathEqArray.ControlProperties);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
    if (string.IsNullOrEmpty(str) || mathEqArray.RowSpacingRule != SpacingRule.Exactly && mathEqArray.RowSpacingRule != SpacingRule.Multiple)
      return;
    float num = (float) Math.Round((double) this.GetSpacingValue(str, "rSp", mathEqArray.RowSpacingRule), 2);
    mathEqArray.SetPropertyValue(27, (object) num);
  }

  private void ParseMathEqArrayJustification(XmlReader reader, OfficeMathEquationArray mathEqArray)
  {
    switch (reader.GetAttribute("val", reader.NamespaceURI).ToLower())
    {
      case "top":
        mathEqArray.VerticalAlignment = MathVerticalAlignment.Top;
        break;
      case "bottom":
        mathEqArray.VerticalAlignment = MathVerticalAlignment.Bottom;
        break;
      case "center":
        mathEqArray.VerticalAlignment = MathVerticalAlignment.Center;
        break;
    }
  }

  private void ParseMathGroupChar(XmlReader reader, OfficeMathGroupCharacter mathGroupChar)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (reader.LocalName != localName)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "groupChrPr":
            this.ParseGroupCharProperties(reader, mathGroupChar);
            break;
          case "e":
            this.ParseMath(reader, mathGroupChar.Equation as OfficeMath);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  private void ParseGroupCharProperties(XmlReader reader, OfficeMathGroupCharacter mathGroupChar)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (!(reader.LocalName == localName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "chr":
            string attribute1 = reader.GetAttribute("val", reader.NamespaceURI);
            if (!string.IsNullOrEmpty(attribute1))
            {
              mathGroupChar.GroupCharacter = attribute1;
              break;
            }
            break;
          case "pos":
            string attribute2 = reader.GetAttribute("val", reader.NamespaceURI);
            if (!string.IsNullOrEmpty(attribute2) && attribute2.ToLower() == "top")
            {
              mathGroupChar.HasCharacterTop = true;
              break;
            }
            break;
          case "vertJc":
            string attribute3 = reader.GetAttribute("val", reader.NamespaceURI);
            if (!string.IsNullOrEmpty(attribute3) && attribute3.ToLower() == "bot")
            {
              mathGroupChar.HasAlignTop = false;
              break;
            }
            break;
          case "ctrlPr":
            mathGroupChar.ControlProperties = this.ParseMathControlProperties(reader, (OfficeMathFunctionBase) mathGroupChar, mathGroupChar.ControlProperties);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  private void ParseMathBar(XmlReader reader, OfficeMathBar mathBar)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (reader.LocalName != localName)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "barPr":
            this.ParseBarProperties(reader, mathBar);
            break;
          case "e":
            this.ParseMath(reader, mathBar.Equation as OfficeMath);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  private void ParseBarProperties(XmlReader reader, OfficeMathBar mathBar)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (!(reader.LocalName == localName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "pos":
            string attribute = reader.GetAttribute("val", reader.NamespaceURI);
            if (!string.IsNullOrEmpty(attribute) && attribute.ToLower() == "top")
            {
              mathBar.BarTop = true;
              break;
            }
            break;
          case "ctrlPr":
            mathBar.ControlProperties = this.ParseMathControlProperties(reader, (OfficeMathFunctionBase) mathBar, mathBar.ControlProperties);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  private void ParseMathAccent(XmlReader reader, OfficeMathAccent mathAccent)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (reader.LocalName != localName)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "accPr":
            this.ParseAccentProperties(reader, mathAccent);
            break;
          case "e":
            this.ParseMath(reader, mathAccent.Equation as OfficeMath);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  private void ParseAccentProperties(XmlReader reader, OfficeMathAccent mathAccent)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (!(reader.LocalName == localName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "chr":
            string attribute = reader.GetAttribute("val", reader.NamespaceURI);
            if (!string.IsNullOrEmpty(attribute))
            {
              mathAccent.AccentCharacter = attribute;
              break;
            }
            break;
          case "ctrlPr":
            mathAccent.ControlProperties = this.ParseMathControlProperties(reader, (OfficeMathFunctionBase) mathAccent, mathAccent.ControlProperties);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  private IOfficeRunFormat ParseMathControlProperties(
    XmlReader reader,
    OfficeMathFunctionBase mathFunction,
    IOfficeRunFormat controlProperties)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (!reader.IsEmptyElement)
    {
      string localName = reader.LocalName;
      reader.Read();
      if (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
      {
        this.SkipWhitespaces(reader);
        while (!(reader.LocalName == localName) || reader.NodeType != XmlNodeType.EndElement)
        {
          if (reader.NodeType == XmlNodeType.Element)
          {
            switch (reader.LocalName)
            {
              case "rPr":
                controlProperties = this.m_documentParser.ParseMathControlFormat(reader, (IOfficeMathFunctionBase) mathFunction);
                break;
            }
            reader.Read();
          }
          else
            reader.Read();
        }
      }
    }
    return controlProperties;
  }

  internal void ParseMathRunFormat(XmlReader reader, OfficeMathFormat mathFormat)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (!(reader.LocalName == localName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "aln":
            mathFormat.HasAlignment = this.GetBooleanValue(reader);
            break;
          case "brk":
            OfficeMathRunElement ownerMathEntity1 = mathFormat.OwnerMathEntity as OfficeMathRunElement;
            OfficeMath ownerMathEntity2 = ownerMathEntity1.OwnerMathEntity as OfficeMath;
            if (ownerMathEntity1 != null)
            {
              mathFormat.Break = (IOfficeMathBreak) (ownerMathEntity2.Breaks.Add(ownerMathEntity2.Breaks.Count) as OfficeMathBreak);
              this.ParseOfficeMathBreak(reader, (OfficeMathBreak) mathFormat.Break);
              break;
            }
            break;
          case "lit":
            mathFormat.HasLiteral = this.GetBooleanValue(reader);
            break;
          case "nor":
            mathFormat.HasNormalText = this.GetBooleanValue(reader);
            break;
          case "scr":
            this.ParseMathRunFormatScript(reader, mathFormat);
            break;
          case "sty":
            this.ParseMathRunFormatStyle(reader, mathFormat);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  private void ParseMathRunFormatScript(XmlReader reader, OfficeMathFormat mathFormat)
  {
    switch (reader.GetAttribute("val", reader.NamespaceURI).ToLower())
    {
      case "double-struck":
        mathFormat.Font = MathFontType.DoubleStruck;
        break;
      case "fraktur":
        mathFormat.Font = MathFontType.Fraktur;
        break;
      case "monospace":
        mathFormat.Font = MathFontType.Monospace;
        break;
      case "sans-serif":
        mathFormat.Font = MathFontType.SansSerif;
        break;
      case "script":
        mathFormat.Font = MathFontType.Script;
        break;
      case "roman":
        mathFormat.Font = MathFontType.Roman;
        break;
    }
  }

  private void ParseMathRunFormatStyle(XmlReader reader, OfficeMathFormat mathFormat)
  {
    switch (reader.GetAttribute("val", reader.NamespaceURI).ToLower())
    {
      case "b":
        mathFormat.Style = MathStyleType.Bold;
        break;
      case "bi":
        mathFormat.Style = MathStyleType.BoldItalic;
        break;
      case "p":
        mathFormat.Style = MathStyleType.Regular;
        break;
      case "i":
        mathFormat.Style = MathStyleType.Italic;
        break;
    }
  }

  private void ParseMathParaProperties(XmlReader reader, IOfficeMathParagraph mathPara)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (!(reader.LocalName == localName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "jc":
            this.ParseMathJustification(reader, mathPara);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  private void ParseMathJustification(XmlReader reader, IOfficeMathParagraph mathPara)
  {
    switch (reader.GetAttribute("val", reader.NamespaceURI).ToLower())
    {
      case "center":
        mathPara.Justification = MathJustification.Center;
        break;
      case "left":
        mathPara.Justification = MathJustification.Left;
        break;
      case "right":
        mathPara.Justification = MathJustification.Right;
        break;
      case "centergroup":
        mathPara.Justification = MathJustification.CenterGroup;
        break;
    }
  }

  private void ParseMathBox(XmlReader reader, OfficeMathBox mathBox)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (!(reader.LocalName == localName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "boxPr":
            this.ParseMathBoxProperties(reader, mathBox);
            break;
          case "e":
            this.ParseMath(reader, mathBox.Equation as OfficeMath);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  private void ParseMathBoxProperties(XmlReader reader, OfficeMathBox mathBox)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (!(reader.LocalName == localName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "aln":
            mathBox.Alignment = this.GetBooleanValue(reader);
            break;
          case "diff":
            mathBox.EnableDifferential = this.GetBooleanValue(reader);
            break;
          case "opEmu":
            mathBox.OperatorEmulator = this.GetBooleanValue(reader);
            break;
          case "noBreak":
            mathBox.NoBreak = this.GetBooleanValue(reader);
            break;
          case "brk":
            if (mathBox.OwnerMathEntity is OfficeMath ownerMathEntity)
            {
              mathBox.Break = (IOfficeMathBreak) (ownerMathEntity.Breaks.Add(ownerMathEntity.Breaks.Count) as OfficeMathBreak);
              this.ParseOfficeMathBreak(reader, (OfficeMathBreak) mathBox.Break);
              break;
            }
            break;
          case "ctrlPr":
            mathBox.ControlProperties = this.ParseMathControlProperties(reader, (OfficeMathFunctionBase) mathBox, mathBox.ControlProperties);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  private void ParseMathBorderBox(XmlReader reader, OfficeMathBorderBox mathBorderBox)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (!(reader.LocalName == localName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "borderBoxPr":
            this.ParseMathBorderBoxProperties(reader, mathBorderBox);
            break;
          case "e":
            this.ParseMath(reader, mathBorderBox.Equation as OfficeMath);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  private void ParseMathBorderBoxProperties(XmlReader reader, OfficeMathBorderBox mathBorderBox)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (!(reader.LocalName == localName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "hideBot":
            mathBorderBox.HideBottom = this.GetBooleanValue(reader);
            break;
          case "hideLeft":
            mathBorderBox.HideLeft = this.GetBooleanValue(reader);
            break;
          case "hideRight":
            mathBorderBox.HideRight = this.GetBooleanValue(reader);
            break;
          case "hideTop":
            mathBorderBox.HideTop = this.GetBooleanValue(reader);
            break;
          case "strikeBLTR":
            mathBorderBox.StrikeDiagonalUp = this.GetBooleanValue(reader);
            break;
          case "strikeH":
            mathBorderBox.StrikeHorizontal = this.GetBooleanValue(reader);
            break;
          case "strikeTLBR":
            mathBorderBox.StrikeDiagonalDown = this.GetBooleanValue(reader);
            break;
          case "strikeV":
            mathBorderBox.StrikeVertical = this.GetBooleanValue(reader);
            break;
          case "ctrlPr":
            mathBorderBox.ControlProperties = this.ParseMathControlProperties(reader, (OfficeMathFunctionBase) mathBorderBox, mathBorderBox.ControlProperties);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  private void ParseMathDelimiter(XmlReader reader, OfficeMathDelimiter mathDelimiter)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (!(reader.LocalName == localName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "dPr":
            this.ParseMathDelimiterProperties(reader, mathDelimiter);
            break;
          case "e":
            OfficeMath officeMath = new OfficeMath((IOfficeMathEntity) mathDelimiter);
            this.ParseMath(reader, officeMath);
            mathDelimiter.m_equation.InnerList.Add((object) officeMath);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  private void ParseMathDelimiterProperties(XmlReader reader, OfficeMathDelimiter mathDelimiter)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (!(reader.LocalName == localName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "begChr":
            string attribute1 = reader.GetAttribute("val", reader.NamespaceURI);
            if (attribute1 != null)
            {
              mathDelimiter.BeginCharacter = attribute1;
              break;
            }
            break;
          case "endChr":
            string attribute2 = reader.GetAttribute("val", reader.NamespaceURI);
            if (attribute2 != null)
            {
              mathDelimiter.EndCharacter = attribute2;
              break;
            }
            break;
          case "grow":
            mathDelimiter.IsGrow = this.GetBooleanValue(reader);
            break;
          case "sepChr":
            string attribute3 = reader.GetAttribute("val", reader.NamespaceURI);
            if (!string.IsNullOrEmpty(attribute3))
            {
              mathDelimiter.Seperator = attribute3;
              break;
            }
            break;
          case "shp":
            this.ParseMathDelimiterShape(reader, mathDelimiter);
            break;
          case "ctrlPr":
            mathDelimiter.ControlProperties = this.ParseMathControlProperties(reader, (OfficeMathFunctionBase) mathDelimiter, mathDelimiter.ControlProperties);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  private void ParseMathFraction(XmlReader reader, OfficeMathFraction mathFraction)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (!(reader.LocalName == localName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "fPr":
            this.ParseMathFractionProperties(reader, mathFraction);
            break;
          case "num":
            this.ParseMath(reader, mathFraction.Numerator as OfficeMath);
            break;
          case "den":
            this.ParseMath(reader, mathFraction.Denominator as OfficeMath);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  private void ParseMathFractionProperties(XmlReader reader, OfficeMathFraction mathFraction)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (!(reader.LocalName == localName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "type":
            this.ParseMathFractionType(reader, mathFraction);
            break;
          case "ctrlPr":
            mathFraction.ControlProperties = this.ParseMathControlProperties(reader, (OfficeMathFunctionBase) mathFraction, mathFraction.ControlProperties);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  private void ParseMathFractionType(XmlReader reader, OfficeMathFraction mathFraction)
  {
    switch (reader.GetAttribute("val", reader.NamespaceURI).ToLower())
    {
      case "nobar":
        mathFraction.FractionType = MathFractionType.NoFractionBar;
        break;
      case "skw":
        mathFraction.FractionType = MathFractionType.SkewedFractionBar;
        break;
      case "lin":
        mathFraction.FractionType = MathFractionType.FractionInline;
        break;
      case "bar":
        mathFraction.FractionType = MathFractionType.NormalFractionBar;
        break;
    }
  }

  private void ParseMathFunc(XmlReader reader, OfficeMathFunction mathFunc)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (!(reader.LocalName == localName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "funcPr":
            this.ParseMathFuncProperties(reader, mathFunc);
            break;
          case "fName":
            this.ParseMath(reader, mathFunc.FunctionName as OfficeMath);
            break;
          case "e":
            this.ParseMath(reader, mathFunc.Equation as OfficeMath);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  private void ParseMathFuncProperties(XmlReader reader, OfficeMathFunction mathFunc)
  {
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    if (localName == reader.LocalName && reader.NodeType == XmlNodeType.EndElement)
      return;
    this.SkipWhitespaces(reader);
    while (!(reader.LocalName == localName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "ctrlPr":
            mathFunc.ControlProperties = this.ParseMathControlProperties(reader, (OfficeMathFunctionBase) mathFunc, mathFunc.ControlProperties);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  private void ParseMathDelimiterShape(XmlReader reader, OfficeMathDelimiter mathDelimiter)
  {
    switch (reader.GetAttribute("val", reader.NamespaceURI))
    {
      case "match":
        mathDelimiter.DelimiterShape = MathDelimiterShapeType.Match;
        break;
      default:
        mathDelimiter.DelimiterShape = MathDelimiterShapeType.Centered;
        break;
    }
  }

  private void ParseOfficeMathBreak(XmlReader reader, OfficeMathBreak mathBreak)
  {
    string attribute = reader.GetAttribute("alnAt", reader.NamespaceURI);
    if (mathBreak == null || attribute == null)
      return;
    mathBreak.AlignAt = (int) this.GetNumericValue(attribute);
  }

  private bool GetBooleanValue(XmlReader reader)
  {
    bool booleanValue = true;
    if (reader.AttributeCount > 0)
    {
      string attribute = reader.GetAttribute("val", reader.NamespaceURI);
      if (attribute == null || attribute == "0" || attribute == "false" || attribute == "off")
        booleanValue = false;
    }
    return booleanValue;
  }

  private float GetNumericValue(string value)
  {
    float result = 0.0f;
    float.TryParse(value, NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result);
    if ((double) result == 0.0 && this.m_isFloatValue.IsMatch(value) && this.m_hasAlphabet.IsMatch(value))
    {
      string[] strArray = value.Split(new char[1]{ '.' }, StringSplitOptions.RemoveEmptyEntries);
      float.TryParse(value.StartsWith(".") ? "0." + strArray[0] : (strArray.Length > 1 ? $"{strArray[0]}.{strArray[1]}" : strArray[0]), NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result);
    }
    return result;
  }

  private float GetFloatValue(string value, string elementName)
  {
    switch (elementName)
    {
      case "cSp":
      case "lMargin":
      case "rMargin":
      case "wrapIndent":
        return this.GetNumericValue(value) / 20f;
      default:
        return 0.0f;
    }
  }

  private float GetSpacingValue(string value, string elementName, SpacingRule spacingRule)
  {
    if (spacingRule == SpacingRule.Exactly)
      return this.GetNumericValue(value) / 20f;
    switch (elementName)
    {
      case "rSp":
        return this.GetNumericValue(value) / 2f;
      case "cGp":
        return this.GetNumericValue(value) * 6f;
      default:
        return 0.0f;
    }
  }

  private void SkipWhitespaces(XmlReader reader)
  {
    if (reader.NodeType == XmlNodeType.Element)
      return;
    while (reader.NodeType == XmlNodeType.Whitespace)
      reader.Read();
  }
}
