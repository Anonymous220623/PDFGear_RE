// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.MathMLSerializer
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System;
using System.Globalization;
using System.Xml;

#nullable disable
namespace Syncfusion.Office;

internal class MathMLSerializer
{
  private const string M_namespace = "http://schemas.openxmlformats.org/officeDocument/2006/math";
  private DocumentSerializer m_documentSerializer;
  private XmlWriter m_writer;

  internal void SerializeMathPara(
    XmlWriter writer,
    IOfficeMathParagraph mathPara,
    DocumentSerializer documentSerializer)
  {
    this.m_writer = writer;
    this.m_documentSerializer = documentSerializer;
    writer.WriteStartElement("oMathPara", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    if (!(mathPara as OfficeMathParagraph).IsDefault)
      this.SerilaizeMathParaProperties(mathPara);
    for (int index = 0; index < mathPara.Maths.Count; ++index)
    {
      this.m_writer.WriteStartElement("oMath", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.SerializeMath(mathPara.Maths[index]);
      this.m_writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private void SerilaizeMathParaProperties(IOfficeMathParagraph mathPara)
  {
    this.m_writer.WriteStartElement("oMathParaPr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMathParaJustification(mathPara);
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathParaJustification(IOfficeMathParagraph mathPara)
  {
    this.m_writer.WriteStartElement("jc", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    switch (mathPara.Justification)
    {
      case MathJustification.Center:
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "center");
        break;
      case MathJustification.Left:
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "left");
        break;
      case MathJustification.Right:
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "right");
        break;
      default:
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "centerGroup");
        break;
    }
    this.m_writer.WriteEndElement();
  }

  internal void SerializeMath(
    XmlWriter writer,
    IOfficeMath officeMath,
    DocumentSerializer documentSerializer)
  {
    this.m_writer = writer;
    this.m_documentSerializer = documentSerializer;
    this.SerializeMath(officeMath);
  }

  private void SerializeMath(IOfficeMath officeMath)
  {
    if ((officeMath as OfficeMath).HasValue(74))
    {
      this.m_writer.WriteStartElement("argPr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.m_writer.WriteStartElement("argSz", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", this.ToString((float) officeMath.ArgumentSize));
      this.m_writer.WriteEndElement();
      this.m_writer.WriteEndElement();
    }
    for (int index = 0; index < officeMath.Functions.Count; ++index)
      this.SerializeMathFunction(officeMath.Functions[index]);
  }

  private void SerializeMathFunction(IOfficeMathFunctionBase officeMathFunction)
  {
    switch (officeMathFunction.Type)
    {
      case MathFunctionType.Accent:
        this.SerializeMathAccent(officeMathFunction as OfficeMathAccent);
        break;
      case MathFunctionType.Bar:
        this.SerializeMathBar(officeMathFunction as OfficeMathBar);
        break;
      case MathFunctionType.BorderBox:
        this.SerializeMathBorderBox(officeMathFunction as OfficeMathBorderBox);
        break;
      case MathFunctionType.Box:
        this.SerializeMathBox(officeMathFunction as OfficeMathBox);
        break;
      case MathFunctionType.Delimiter:
        this.SerializeMathDelimiter(officeMathFunction as OfficeMathDelimiter);
        break;
      case MathFunctionType.EquationArray:
        this.SerializeMathEqArray(officeMathFunction as OfficeMathEquationArray);
        break;
      case MathFunctionType.Fraction:
        this.SerializeMathFraction(officeMathFunction as OfficeMathFraction);
        break;
      case MathFunctionType.Function:
        this.SerializeMathFunc(officeMathFunction as OfficeMathFunction);
        break;
      case MathFunctionType.GroupCharacter:
        this.SerializeMathGroupChar(officeMathFunction as OfficeMathGroupCharacter);
        break;
      case MathFunctionType.Limit:
        OfficeMathLimit officeMathLimit = officeMathFunction as OfficeMathLimit;
        if (officeMathLimit.LimitType == MathLimitType.LowerLimit)
        {
          this.SerializeMathLowerLimit(officeMathLimit);
          break;
        }
        this.SerializeMathUpperLimit(officeMathLimit);
        break;
      case MathFunctionType.Matrix:
        this.SerializeMathMatrix(officeMathFunction as OfficeMathMatrix);
        break;
      case MathFunctionType.NArray:
        this.SerializeMathNAry(officeMathFunction as OfficeMathNArray);
        break;
      case MathFunctionType.Phantom:
        this.SerializeMathPhantom(officeMathFunction as OfficeMathPhantom);
        break;
      case MathFunctionType.Radical:
        this.SerializeMathRadical(officeMathFunction as OfficeMathRadical);
        break;
      case MathFunctionType.LeftSubSuperscript:
        this.SerializeMathLeftScript(officeMathFunction as OfficeMathLeftScript);
        break;
      case MathFunctionType.SubSuperscript:
        OfficeMathScript officeMathScript = officeMathFunction as OfficeMathScript;
        if (officeMathScript.ScriptType == MathScriptType.Subscript)
        {
          this.SerializeMathSubScript(officeMathScript);
          break;
        }
        this.SerializeMathSuperScript(officeMathScript);
        break;
      case MathFunctionType.RightSubSuperscript:
        this.SerializeMathRightScript(officeMathFunction as OfficeMathRightScript);
        break;
      case MathFunctionType.RunElement:
        this.SerializeMathText(officeMathFunction as OfficeMathRunElement);
        break;
    }
  }

  internal void SerializeMathProperties(XmlWriter writer, OfficeMathProperties mathProperties)
  {
    this.m_writer = writer;
    this.m_writer.WriteStartElement("mathPr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    if (mathProperties.HasValue(66))
    {
      this.m_writer.WriteStartElement("mathFont", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", mathProperties.MathFont);
      this.m_writer.WriteEndElement();
    }
    if (mathProperties.HasValue(58))
      this.SerializeBreakOnBinaryOperator(mathProperties);
    if (mathProperties.HasValue(59))
      this.SerializeBreakOnSubtractOperator(mathProperties);
    if (mathProperties.HasValue(71))
    {
      this.m_writer.WriteStartElement("smallFrac", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", !mathProperties.SmallFraction ? "0" : "1");
      this.m_writer.WriteEndElement();
    }
    if (mathProperties.HasValue(61))
    {
      this.m_writer.WriteStartElement("dispDef", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", !mathProperties.DisplayMathDefaults ? "0" : "1");
      this.m_writer.WriteEndElement();
    }
    if (mathProperties.HasValue(65))
    {
      this.m_writer.WriteStartElement("lMargin", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", this.ToString((float) (mathProperties.LeftMargin * 20)));
      this.m_writer.WriteEndElement();
    }
    if (mathProperties.HasValue(70))
    {
      this.m_writer.WriteStartElement("rMargin", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", this.ToString((float) (mathProperties.RightMargin * 20)));
      this.m_writer.WriteEndElement();
    }
    if (mathProperties.HasValue(60))
      this.SerializeDefaultJustification(mathProperties);
    if (mathProperties.HasValue(72))
    {
      this.m_writer.WriteStartElement("wrapIndent", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", this.ToString((float) (mathProperties.WrapIndent * 20)));
      this.m_writer.WriteEndElement();
    }
    if (mathProperties.HasValue(73))
      this.SerializeBoolProperty("wrapRight", mathProperties.WrapRight);
    if (mathProperties.HasValue(63 /*0x3F*/))
      this.SerializeIntergralLimitLocation(mathProperties);
    if (mathProperties.HasValue(67))
      this.SerializeNAryLimitLocation(mathProperties);
    this.m_writer.WriteEndElement();
  }

  private void SerializeNAryLimitLocation(OfficeMathProperties mathProperties)
  {
    this.m_writer.WriteStartElement("naryLim", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    if (mathProperties.NAryLimitLocation == LimitLocationType.SubSuperscript)
      this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "subSup");
    else
      this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "undOvr");
    this.m_writer.WriteEndElement();
  }

  private void SerializeIntergralLimitLocation(OfficeMathProperties mathProperties)
  {
    this.m_writer.WriteStartElement("intLim", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    if (mathProperties.IntegralLimitLocations == LimitLocationType.UnderOver)
      this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "undOvr");
    else
      this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "subSup");
    this.m_writer.WriteEndElement();
  }

  private void SerializeDefaultJustification(OfficeMathProperties mathProperties)
  {
    this.m_writer.WriteStartElement("defJc", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    switch (mathProperties.DefaultJustification)
    {
      case MathJustification.Center:
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "center");
        break;
      case MathJustification.Left:
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "left");
        break;
      case MathJustification.Right:
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "right");
        break;
      default:
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "centerGroup");
        break;
    }
    this.m_writer.WriteEndElement();
  }

  private void SerializeBreakOnSubtractOperator(OfficeMathProperties mathProperties)
  {
    this.m_writer.WriteStartElement("brkBinSub", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    switch (mathProperties.BreakOnBinarySubtraction)
    {
      case BreakOnBinarySubtractionType.PlusMinus:
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "++");
        break;
      case BreakOnBinarySubtractionType.MinusPlus:
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "-+");
        break;
      default:
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "--");
        break;
    }
    this.m_writer.WriteEndElement();
  }

  private void SerializeBreakOnBinaryOperator(OfficeMathProperties mathProperties)
  {
    this.m_writer.WriteStartElement("brkBin", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    switch (mathProperties.BreakOnBinaryOperators)
    {
      case BreakOnBinaryOperatorsType.After:
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "after");
        break;
      case BreakOnBinaryOperatorsType.Repeat:
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "repeat");
        break;
      default:
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "before");
        break;
    }
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathUpperLimit(OfficeMathLimit officeMathLimit)
  {
    this.m_writer.WriteStartElement("limUpp", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMathUpperLimitProperties(officeMathLimit);
    this.m_writer.WriteStartElement("e", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMath(officeMathLimit.Equation);
    this.m_writer.WriteEndElement();
    this.m_writer.WriteStartElement("lim", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMath(officeMathLimit.Limit);
    this.m_writer.WriteEndElement();
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathUpperLimitProperties(OfficeMathLimit officeMathLimit)
  {
    this.m_writer.WriteStartElement("limUppPr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    if (officeMathLimit.ControlProperties != null)
    {
      this.m_writer.WriteStartElement("ctrlPr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.m_documentSerializer.SerializeControlProperties(officeMathLimit.ControlProperties);
      this.m_writer.WriteEndElement();
    }
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathLowerLimit(OfficeMathLimit officeMathLimit)
  {
    this.m_writer.WriteStartElement("limLow", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMathLowerLimitProperties(officeMathLimit);
    this.m_writer.WriteStartElement("e", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMath(officeMathLimit.Equation);
    this.m_writer.WriteEndElement();
    this.m_writer.WriteStartElement("lim", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMath(officeMathLimit.Limit);
    this.m_writer.WriteEndElement();
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathLowerLimitProperties(OfficeMathLimit officeMathLimit)
  {
    this.m_writer.WriteStartElement("limLowPr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    if (officeMathLimit.ControlProperties != null)
    {
      this.m_writer.WriteStartElement("ctrlPr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.m_documentSerializer.SerializeControlProperties(officeMathLimit.ControlProperties);
      this.m_writer.WriteEndElement();
    }
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathRightScript(OfficeMathRightScript officeMathRightScript)
  {
    this.m_writer.WriteStartElement("sSubSup", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMathRightScriptProperties(officeMathRightScript);
    this.m_writer.WriteStartElement("e", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMath(officeMathRightScript.Equation);
    this.m_writer.WriteEndElement();
    this.m_writer.WriteStartElement("sub", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMath(officeMathRightScript.Subscript);
    this.m_writer.WriteEndElement();
    this.m_writer.WriteStartElement("sup", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMath(officeMathRightScript.Superscript);
    this.m_writer.WriteEndElement();
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathRightScriptProperties(OfficeMathRightScript officeMathRightScript)
  {
    this.m_writer.WriteStartElement("sSubSupPr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    if (officeMathRightScript.HasValue(52))
      this.SerializeBoolProperty("alnScr", officeMathRightScript.IsSkipAlign);
    if (officeMathRightScript.ControlProperties != null)
    {
      this.m_writer.WriteStartElement("ctrlPr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.m_documentSerializer.SerializeControlProperties(officeMathRightScript.ControlProperties);
      this.m_writer.WriteEndElement();
    }
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathLeftScript(OfficeMathLeftScript officeMathLeftScript)
  {
    this.m_writer.WriteStartElement("sPre", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMathLeftScriptProperties(officeMathLeftScript);
    this.m_writer.WriteStartElement("sub", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMath(officeMathLeftScript.Subscript);
    this.m_writer.WriteEndElement();
    this.m_writer.WriteStartElement("sup", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMath(officeMathLeftScript.Superscript);
    this.m_writer.WriteEndElement();
    this.m_writer.WriteStartElement("e", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMath(officeMathLeftScript.Equation);
    this.m_writer.WriteEndElement();
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathLeftScriptProperties(OfficeMathLeftScript officeMathLeftScript)
  {
    this.m_writer.WriteStartElement("sPrePr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    if (officeMathLeftScript.ControlProperties != null)
    {
      this.m_writer.WriteStartElement("ctrlPr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.m_documentSerializer.SerializeControlProperties(officeMathLeftScript.ControlProperties);
      this.m_writer.WriteEndElement();
    }
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathSuperScript(OfficeMathScript officeMathScript)
  {
    this.m_writer.WriteStartElement("sSup", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMathSuperScriptProperties(officeMathScript);
    this.m_writer.WriteStartElement("e", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMath(officeMathScript.Equation);
    this.m_writer.WriteEndElement();
    this.m_writer.WriteStartElement("sup", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMath(officeMathScript.Script);
    this.m_writer.WriteEndElement();
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathSuperScriptProperties(OfficeMathScript officeMathScript)
  {
    this.m_writer.WriteStartElement("sSupPr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    if (officeMathScript.ControlProperties != null)
    {
      this.m_writer.WriteStartElement("ctrlPr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.m_documentSerializer.SerializeControlProperties(officeMathScript.ControlProperties);
      this.m_writer.WriteEndElement();
    }
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathSubScript(OfficeMathScript officeMathScript)
  {
    this.m_writer.WriteStartElement("sSub", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMathSubScriptProperties(officeMathScript);
    this.m_writer.WriteStartElement("e", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMath(officeMathScript.Equation);
    this.m_writer.WriteEndElement();
    this.m_writer.WriteStartElement("sub", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMath(officeMathScript.Script);
    this.m_writer.WriteEndElement();
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathSubScriptProperties(OfficeMathScript officeMathScript)
  {
    this.m_writer.WriteStartElement("sSubPr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    if (officeMathScript.ControlProperties != null)
    {
      this.m_writer.WriteStartElement("ctrlPr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.m_documentSerializer.SerializeControlProperties(officeMathScript.ControlProperties);
      this.m_writer.WriteEndElement();
    }
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathText(OfficeMathRunElement officeMathParaItem)
  {
    this.m_writer.WriteStartElement("r", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.m_documentSerializer.SerializeMathRun((IOfficeMathRunElement) officeMathParaItem);
    this.m_writer.WriteEndElement();
  }

  internal void SerializeMathRunFormat(XmlWriter writer, OfficeMathFormat mathFormat)
  {
    this.m_writer = writer;
    if (mathFormat.IsDefault)
      return;
    this.m_writer.WriteStartElement("rPr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    if (mathFormat.HasValue(53))
      this.SerializeBoolProperty("aln", mathFormat.HasAlignment);
    this.SerializeMathBreak((OfficeMathBreak) mathFormat.Break);
    if (mathFormat.HasValue(54))
      this.SerializeBoolProperty("lit", mathFormat.HasLiteral);
    if (mathFormat.HasValue(55))
      this.SerializeBoolProperty("nor", mathFormat.HasNormalText);
    if (mathFormat.HasValue(56))
      this.SerializeMathRunFormatScript(mathFormat);
    if (mathFormat.HasValue(57))
      this.SerializeMathRunFormatStyle(mathFormat);
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathRunFormatScript(OfficeMathFormat mathFormat)
  {
    this.m_writer.WriteStartElement("scr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    switch (mathFormat.Font)
    {
      case MathFontType.DoubleStruck:
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "double-struck");
        break;
      case MathFontType.Fraktur:
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "fraktur");
        break;
      case MathFontType.Monospace:
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "monospace");
        break;
      case MathFontType.SansSerif:
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "sans-serif");
        break;
      case MathFontType.Script:
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "script");
        break;
      default:
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "roman");
        break;
    }
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathRunFormatStyle(OfficeMathFormat mathFormat)
  {
    this.m_writer.WriteStartElement("sty", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    switch (mathFormat.Style)
    {
      case MathStyleType.BoldItalic:
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "bi");
        break;
      case MathStyleType.Bold:
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "b");
        break;
      case MathStyleType.Regular:
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "p");
        break;
      default:
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "i");
        break;
    }
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathAccent(OfficeMathAccent mathAccent)
  {
    this.m_writer.WriteStartElement("acc", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMathAccentProperties(mathAccent);
    this.m_writer.WriteStartElement("e", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMath(mathAccent.Equation);
    this.m_writer.WriteEndElement();
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathAccentProperties(OfficeMathAccent mathAccent)
  {
    this.m_writer.WriteStartElement("accPr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    if (mathAccent.HasValue(1))
    {
      this.m_writer.WriteStartElement("chr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", mathAccent.AccentCharacter);
      this.m_writer.WriteEndElement();
    }
    if (mathAccent.ControlProperties != null)
    {
      this.m_writer.WriteStartElement("ctrlPr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.m_documentSerializer.SerializeControlProperties(mathAccent.ControlProperties);
      this.m_writer.WriteEndElement();
    }
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathBar(OfficeMathBar mathBar)
  {
    this.m_writer.WriteStartElement("bar", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMathBarProperties(mathBar);
    this.m_writer.WriteStartElement("e", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMath(mathBar.Equation);
    this.m_writer.WriteEndElement();
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathBarProperties(OfficeMathBar mathBar)
  {
    this.m_writer.WriteStartElement("barPr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    if (mathBar.HasValue(2))
    {
      this.m_writer.WriteStartElement("pos", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", !mathBar.BarTop ? "bot" : "top");
      this.m_writer.WriteEndElement();
    }
    if (mathBar.ControlProperties != null)
    {
      this.m_writer.WriteStartElement("ctrlPr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.m_documentSerializer.SerializeControlProperties(mathBar.ControlProperties);
      this.m_writer.WriteEndElement();
    }
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathBox(OfficeMathBox mathBox)
  {
    this.m_writer.WriteStartElement("box", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMathBoxProperties(mathBox);
    this.m_writer.WriteStartElement("e", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMath(mathBox.Equation);
    this.m_writer.WriteEndElement();
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathBoxProperties(OfficeMathBox mathBox)
  {
    this.m_writer.WriteStartElement("boxPr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    if (mathBox.HasValue(11))
      this.SerializeBoolProperty("aln", mathBox.Alignment);
    if (mathBox.HasValue(14))
      this.SerializeBoolProperty("diff", mathBox.EnableDifferential);
    if (mathBox.HasValue(15))
      this.SerializeBoolProperty("opEmu", mathBox.OperatorEmulator);
    if (mathBox.HasValue(12))
      this.SerializeBoolProperty("noBreak", mathBox.NoBreak);
    this.SerializeMathBreak((OfficeMathBreak) mathBox.Break);
    if (mathBox.ControlProperties != null)
    {
      this.m_writer.WriteStartElement("ctrlPr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.m_documentSerializer.SerializeControlProperties(mathBox.ControlProperties);
      this.m_writer.WriteEndElement();
    }
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathBreak(OfficeMathBreak mathBreak)
  {
    if (mathBreak == null)
      return;
    this.m_writer.WriteStartElement("brk", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    if (mathBreak.HasValue(16 /*0x10*/))
      this.m_writer.WriteAttributeString("alnAt", "http://schemas.openxmlformats.org/officeDocument/2006/math", this.ToString((float) mathBreak.AlignAt));
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathBorderBox(OfficeMathBorderBox mathBorderBox)
  {
    this.m_writer.WriteStartElement("borderBox", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMathBorderBoxProperties(mathBorderBox);
    this.m_writer.WriteStartElement("e", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMath(mathBorderBox.Equation);
    this.m_writer.WriteEndElement();
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathBorderBoxProperties(OfficeMathBorderBox mathBorderBox)
  {
    this.m_writer.WriteStartElement("borderBoxPr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    if (mathBorderBox.HasValue(4))
      this.SerializeBoolProperty("hideBot", mathBorderBox.HideBottom);
    if (mathBorderBox.HasValue(6))
      this.SerializeBoolProperty("hideLeft", mathBorderBox.HideLeft);
    if (mathBorderBox.HasValue(5))
      this.SerializeBoolProperty("hideRight", mathBorderBox.HideRight);
    if (mathBorderBox.HasValue(3))
      this.SerializeBoolProperty("hideTop", mathBorderBox.HideTop);
    if (mathBorderBox.HasValue(7))
      this.SerializeBoolProperty("strikeBLTR", mathBorderBox.StrikeDiagonalUp);
    if (mathBorderBox.HasValue(10))
      this.SerializeBoolProperty("strikeH", mathBorderBox.StrikeHorizontal);
    if (mathBorderBox.HasValue(8))
      this.SerializeBoolProperty("strikeTLBR", mathBorderBox.StrikeDiagonalDown);
    if (mathBorderBox.HasValue(9))
      this.SerializeBoolProperty("strikeV", mathBorderBox.StrikeVertical);
    if (mathBorderBox.ControlProperties != null)
    {
      this.m_writer.WriteStartElement("ctrlPr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.m_documentSerializer.SerializeControlProperties(mathBorderBox.ControlProperties);
      this.m_writer.WriteEndElement();
    }
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathDelimiter(OfficeMathDelimiter mathDelimiter)
  {
    this.m_writer.WriteStartElement("d", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMathDelimiterProperties(mathDelimiter);
    for (int index = 0; index < mathDelimiter.Equation.Count; ++index)
    {
      this.m_writer.WriteStartElement("e", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.SerializeMath(mathDelimiter.Equation[index]);
      this.m_writer.WriteEndElement();
    }
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathDelimiterProperties(OfficeMathDelimiter mathDelimiter)
  {
    this.m_writer.WriteStartElement("dPr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    if (mathDelimiter.HasValue(17))
    {
      this.m_writer.WriteStartElement("begChr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", mathDelimiter.BeginCharacter);
      this.m_writer.WriteEndElement();
    }
    if (mathDelimiter.HasValue(18))
    {
      this.m_writer.WriteStartElement("endChr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", mathDelimiter.EndCharacter);
      this.m_writer.WriteEndElement();
    }
    if (mathDelimiter.HasValue(21))
    {
      this.m_writer.WriteStartElement("sepChr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", mathDelimiter.Seperator);
      this.m_writer.WriteEndElement();
    }
    if (mathDelimiter.HasValue(22))
      this.SerializeBoolProperty("grow", mathDelimiter.IsGrow);
    if (mathDelimiter.HasValue(23))
      this.SerializeMathDelimiterShape(mathDelimiter);
    if (mathDelimiter.ControlProperties != null)
    {
      this.m_writer.WriteStartElement("ctrlPr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.m_documentSerializer.SerializeControlProperties(mathDelimiter.ControlProperties);
      this.m_writer.WriteEndElement();
    }
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathDelimiterShape(OfficeMathDelimiter mathDelimiter)
  {
    this.m_writer.WriteStartElement("shp", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    if (mathDelimiter.DelimiterShape == MathDelimiterShapeType.Match)
      this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "match");
    else
      this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "centered");
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathEqArray(OfficeMathEquationArray mathEqArray)
  {
    this.m_writer.WriteStartElement("eqArr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMathEqArrayProperties(mathEqArray);
    for (int index = 0; index < mathEqArray.Equation.Count; ++index)
    {
      this.m_writer.WriteStartElement("e", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.SerializeMath(mathEqArray.Equation[index]);
      this.m_writer.WriteEndElement();
    }
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathEqArrayProperties(OfficeMathEquationArray mathEqArray)
  {
    this.m_writer.WriteStartElement("eqArrPr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    if (mathEqArray.HasValue(24))
      this.SerializeMathEqArrayAlignment(mathEqArray);
    if (mathEqArray.HasValue(25))
      this.SerializeBoolProperty("maxDist", mathEqArray.ExpandEquationContainer);
    if (mathEqArray.HasValue(26))
      this.SerializeBoolProperty("objDist", mathEqArray.ExpandEquationContent);
    if (mathEqArray.HasValue(27) && (mathEqArray.RowSpacingRule == SpacingRule.Exactly || mathEqArray.RowSpacingRule == SpacingRule.Multiple))
      this.SerializeRowSpacing(mathEqArray.RowSpacing, mathEqArray.RowSpacingRule);
    if (mathEqArray.HasValue(28))
    {
      this.m_writer.WriteStartElement("rSpRule", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.SerializeSpacingRule(mathEqArray.RowSpacingRule);
      this.m_writer.WriteEndElement();
    }
    if (mathEqArray.ControlProperties != null)
    {
      this.m_writer.WriteStartElement("ctrlPr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.m_documentSerializer.SerializeControlProperties(mathEqArray.ControlProperties);
      this.m_writer.WriteEndElement();
    }
    this.m_writer.WriteEndElement();
  }

  private void SerializeSpacingRule(SpacingRule spacingRule)
  {
    switch (spacingRule)
    {
      case SpacingRule.OneAndHalf:
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "1");
        break;
      case SpacingRule.Double:
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "2");
        break;
      case SpacingRule.Exactly:
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "3");
        break;
      case SpacingRule.Multiple:
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "4");
        break;
      default:
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "0");
        break;
    }
  }

  private void SerializeMathEqArrayAlignment(OfficeMathEquationArray mathEqArray)
  {
    this.m_writer.WriteStartElement("baseJc", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    switch (mathEqArray.VerticalAlignment)
    {
      case MathVerticalAlignment.Top:
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "top");
        break;
      case MathVerticalAlignment.Bottom:
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "bottom");
        break;
      default:
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "centered");
        break;
    }
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathFraction(OfficeMathFraction mathFraction)
  {
    this.m_writer.WriteStartElement("f", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMathFractionProperties(mathFraction);
    this.m_writer.WriteStartElement("num", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMath(mathFraction.Numerator);
    this.m_writer.WriteEndElement();
    this.m_writer.WriteStartElement("den", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMath(mathFraction.Denominator);
    this.m_writer.WriteEndElement();
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathFractionProperties(OfficeMathFraction mathFraction)
  {
    this.m_writer.WriteStartElement("fPr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    if (mathFraction.HasValue(29))
      this.SerializeMathFractionType(mathFraction);
    if (mathFraction.ControlProperties != null)
    {
      this.m_writer.WriteStartElement("ctrlPr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.m_documentSerializer.SerializeControlProperties(mathFraction.ControlProperties);
      this.m_writer.WriteEndElement();
    }
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathFractionType(OfficeMathFraction mathFraction)
  {
    this.m_writer.WriteStartElement("type", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    switch (mathFraction.FractionType)
    {
      case MathFractionType.NoFractionBar:
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "noBar");
        break;
      case MathFractionType.SkewedFractionBar:
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "skw");
        break;
      case MathFractionType.FractionInline:
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "lin");
        break;
      default:
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "bar");
        break;
    }
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathFunc(OfficeMathFunction mathFunc)
  {
    this.m_writer.WriteStartElement("func", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMathFuncProperties(mathFunc);
    this.m_writer.WriteStartElement("fName", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMath(mathFunc.FunctionName);
    this.m_writer.WriteEndElement();
    this.m_writer.WriteStartElement("e", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMath(mathFunc.Equation);
    this.m_writer.WriteEndElement();
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathFuncProperties(OfficeMathFunction mathFunc)
  {
    this.m_writer.WriteStartElement("funcPr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    if (mathFunc.ControlProperties != null)
    {
      this.m_writer.WriteStartElement("ctrlPr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.m_documentSerializer.SerializeControlProperties(mathFunc.ControlProperties);
      this.m_writer.WriteEndElement();
    }
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathGroupChar(OfficeMathGroupCharacter mathGroupChar)
  {
    this.m_writer.WriteStartElement("groupChr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMathGroupCharProperties(mathGroupChar);
    this.m_writer.WriteStartElement("e", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMath(mathGroupChar.Equation);
    this.m_writer.WriteEndElement();
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathGroupCharProperties(OfficeMathGroupCharacter mathGroupChar)
  {
    this.m_writer.WriteStartElement("groupChrPr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    if (mathGroupChar.HasValue(30))
    {
      this.m_writer.WriteStartElement("vertJc", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      if (!mathGroupChar.HasAlignTop)
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "bot");
      else
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "top");
      this.m_writer.WriteEndElement();
    }
    if (mathGroupChar.HasValue(32 /*0x20*/))
    {
      this.m_writer.WriteStartElement("pos", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      if (!mathGroupChar.HasCharacterTop)
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "bot");
      else
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "top");
      this.m_writer.WriteEndElement();
    }
    if (mathGroupChar.HasValue(31 /*0x1F*/))
    {
      this.m_writer.WriteStartElement("chr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", mathGroupChar.GroupCharacter);
      this.m_writer.WriteEndElement();
    }
    if (mathGroupChar.ControlProperties != null)
    {
      this.m_writer.WriteStartElement("ctrlPr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.m_documentSerializer.SerializeControlProperties(mathGroupChar.ControlProperties);
      this.m_writer.WriteEndElement();
    }
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathMatrix(OfficeMathMatrix mathMatrix)
  {
    this.m_writer.WriteStartElement("m", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMathMatrixProperties(mathMatrix);
    for (int index = 0; index < mathMatrix.Rows.Count; ++index)
    {
      this.m_writer.WriteStartElement("mr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.SerializeMathMatrixRow(mathMatrix.Rows[index] as OfficeMathMatrixRow);
      this.m_writer.WriteEndElement();
    }
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathMatrixProperties(OfficeMathMatrix mathMatrix)
  {
    this.m_writer.WriteStartElement("mPr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    if (mathMatrix.HasValue(33))
      this.SerializeMathMatrixAlign(mathMatrix);
    if (mathMatrix.HasValue(37))
      this.SerializeBoolProperty("plcHide", mathMatrix.HidePlaceHolders);
    if (mathMatrix.HasValue(28))
    {
      this.m_writer.WriteStartElement("rSpRule", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.SerializeSpacingRule(mathMatrix.RowSpacingRule);
      this.m_writer.WriteEndElement();
    }
    if (mathMatrix.HasValue(34))
    {
      this.m_writer.WriteStartElement("cSp", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", this.ToString(mathMatrix.ColumnWidth * 20f));
      this.m_writer.WriteEndElement();
    }
    if (mathMatrix.HasValue(35))
    {
      this.m_writer.WriteStartElement("cGpRule", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.SerializeSpacingRule(mathMatrix.ColumnSpacingRule);
      this.m_writer.WriteEndElement();
    }
    if (mathMatrix.HasValue(27) && (mathMatrix.RowSpacingRule == SpacingRule.Exactly || mathMatrix.RowSpacingRule == SpacingRule.Multiple))
      this.SerializeRowSpacing(mathMatrix.RowSpacing, mathMatrix.RowSpacingRule);
    if (mathMatrix.HasValue(36) && (mathMatrix.ColumnSpacingRule == SpacingRule.Exactly || mathMatrix.ColumnSpacingRule == SpacingRule.Multiple))
    {
      string empty = string.Empty;
      string str = mathMatrix.ColumnSpacingRule != SpacingRule.Exactly ? this.ToString((float) Math.Floor((double) mathMatrix.ColumnSpacing / 6.0)) : this.ToString(mathMatrix.ColumnSpacing * 20f);
      this.m_writer.WriteStartElement("cGp", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", str);
      this.m_writer.WriteEndElement();
    }
    this.SerializeMathMatrixColumns(mathMatrix);
    if (mathMatrix.ControlProperties != null)
    {
      this.m_writer.WriteStartElement("ctrlPr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.m_documentSerializer.SerializeControlProperties(mathMatrix.ControlProperties);
      this.m_writer.WriteEndElement();
    }
    this.m_writer.WriteEndElement();
  }

  private void SerializeRowSpacing(float rowSpacing, SpacingRule spacingRule)
  {
    string empty = string.Empty;
    string str = spacingRule != SpacingRule.Exactly ? this.ToString(rowSpacing * 2f) : this.ToString(rowSpacing * 20f);
    this.m_writer.WriteStartElement("rSp", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", str);
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathMatrixAlign(OfficeMathMatrix mathMatrix)
  {
    this.m_writer.WriteStartElement("baseJc", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    switch (mathMatrix.VerticalAlignment)
    {
      case MathVerticalAlignment.Top:
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "top");
        break;
      case MathVerticalAlignment.Bottom:
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "bottom");
        break;
      default:
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "centered");
        break;
    }
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathMatrixColumns(OfficeMathMatrix mathMatrix)
  {
    this.m_writer.WriteStartElement("mcs", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    for (int index = 0; index < mathMatrix.ColumnProperties.Count; ++index)
    {
      this.m_writer.WriteStartElement("mc", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.SerializeMathMatrixColumnProperties(mathMatrix.ColumnProperties[index]);
      this.m_writer.WriteEndElement();
    }
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathMatrixColumnProperties(MatrixColumnProperties mathMatrixColumnProperties)
  {
    this.m_writer.WriteStartElement("mcPr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.m_writer.WriteStartElement("count", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", this.ToString((float) mathMatrixColumnProperties.Count));
    this.m_writer.WriteEndElement();
    this.SerializeMathMatrixColumnAlign(mathMatrixColumnProperties);
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathMatrixColumnAlign(MatrixColumnProperties mathMatrixColumnProperties)
  {
    this.m_writer.WriteStartElement("mcJc", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    switch (mathMatrixColumnProperties.Alignment)
    {
      case MathHorizontalAlignment.Left:
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "left");
        break;
      case MathHorizontalAlignment.Right:
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "right");
        break;
      default:
        this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "center");
        break;
    }
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathMatrixRow(OfficeMathMatrixRow mathMatrixRow)
  {
    for (int index = 0; index < mathMatrixRow.Arguments.Count; ++index)
    {
      this.m_writer.WriteStartElement("e", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.SerializeMath(mathMatrixRow.Arguments[index]);
      this.m_writer.WriteEndElement();
    }
  }

  private void SerializeMathNAry(OfficeMathNArray mathNAry)
  {
    this.m_writer.WriteStartElement("nary", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMathNAryProperties(mathNAry);
    this.m_writer.WriteStartElement("sub", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMath(mathNAry.Subscript);
    this.m_writer.WriteEndElement();
    this.m_writer.WriteStartElement("sup", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMath(mathNAry.Superscript);
    this.m_writer.WriteEndElement();
    this.m_writer.WriteStartElement("e", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMath(mathNAry.Equation);
    this.m_writer.WriteEndElement();
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathNAryProperties(OfficeMathNArray mathNAry)
  {
    this.m_writer.WriteStartElement("naryPr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    if (mathNAry.HasValue(41))
    {
      this.m_writer.WriteStartElement("chr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", mathNAry.NArrayCharacter);
      this.m_writer.WriteEndElement();
    }
    if (mathNAry.HasValue(42))
      this.SerializeBoolProperty("grow", mathNAry.HasGrow);
    if (mathNAry.HasValue(45))
    {
      this.m_writer.WriteStartElement("limLoc", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", !mathNAry.SubSuperscriptLimit ? "undOvr" : "subSup");
      this.m_writer.WriteEndElement();
    }
    if (mathNAry.HasValue(43))
      this.SerializeBoolProperty("subHide", mathNAry.HideLowerLimit);
    if (mathNAry.HasValue(44))
      this.SerializeBoolProperty("supHide", mathNAry.HideUpperLimit);
    if (mathNAry.ControlProperties != null)
    {
      this.m_writer.WriteStartElement("ctrlPr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.m_documentSerializer.SerializeControlProperties(mathNAry.ControlProperties);
      this.m_writer.WriteEndElement();
    }
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathRadical(OfficeMathRadical mathRadical)
  {
    this.m_writer.WriteStartElement("rad", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMathRadicalProperties(mathRadical);
    this.m_writer.WriteStartElement("deg", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMath(mathRadical.Degree);
    this.m_writer.WriteEndElement();
    this.m_writer.WriteStartElement("e", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMath(mathRadical.Equation);
    this.m_writer.WriteEndElement();
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathRadicalProperties(OfficeMathRadical mathRadical)
  {
    this.m_writer.WriteStartElement("radPr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    if (mathRadical.HasValue(51))
      this.SerializeBoolProperty("degHide", mathRadical.HideDegree);
    if (mathRadical.ControlProperties != null)
    {
      this.m_writer.WriteStartElement("ctrlPr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.m_documentSerializer.SerializeControlProperties(mathRadical.ControlProperties);
      this.m_writer.WriteEndElement();
    }
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathPhantom(OfficeMathPhantom mathPhantom)
  {
    this.m_writer.WriteStartElement("phant", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    if (!mathPhantom.IsDefault)
      this.SerializeMathPhantomProperties(mathPhantom);
    this.m_writer.WriteStartElement("e", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    this.SerializeMath(mathPhantom.Equation);
    this.m_writer.WriteEndElement();
    this.m_writer.WriteEndElement();
  }

  private void SerializeMathPhantomProperties(OfficeMathPhantom mathPhantom)
  {
    this.m_writer.WriteStartElement("phantPr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
    if (mathPhantom.HasValue(46))
      this.SerializeBoolProperty("show", mathPhantom.Show);
    if (mathPhantom.HasValue(49))
      this.SerializeBoolProperty("zeroAsc", mathPhantom.ZeroAscent);
    if (mathPhantom.HasValue(50))
      this.SerializeBoolProperty("zeroDesc", mathPhantom.ZeroDescent);
    if (mathPhantom.HasValue(48 /*0x30*/))
      this.SerializeBoolProperty("transp", mathPhantom.Transparent);
    if (mathPhantom.HasValue(51))
      this.SerializeBoolProperty("zeroWid", mathPhantom.ZeroWidth);
    if (mathPhantom.ControlProperties != null)
    {
      this.m_writer.WriteStartElement("ctrlPr", "http://schemas.openxmlformats.org/officeDocument/2006/math");
      this.m_documentSerializer.SerializeControlProperties(mathPhantom.ControlProperties);
      this.m_writer.WriteEndElement();
    }
    this.m_writer.WriteEndElement();
  }

  private void SerializeBoolProperty(string tag, bool value)
  {
    this.m_writer.WriteStartElement(tag, "http://schemas.openxmlformats.org/officeDocument/2006/math");
    if (!value)
      this.m_writer.WriteAttributeString("val", "http://schemas.openxmlformats.org/officeDocument/2006/math", "0");
    this.m_writer.WriteEndElement();
  }

  internal string ToString(float value)
  {
    return ((int) Math.Round((double) value)).ToString((IFormatProvider) CultureInfo.InvariantCulture);
  }
}
