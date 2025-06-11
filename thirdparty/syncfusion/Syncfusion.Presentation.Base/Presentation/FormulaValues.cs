// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.FormulaValues
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Presentation;

internal class FormulaValues
{
  private RectangleF _rectBounds;
  private Dictionary<string, string> _shapeGuide;

  internal FormulaValues(RectangleF bounds, Dictionary<string, string> shapeGuide)
  {
    this._rectBounds = bounds;
    this._shapeGuide = shapeGuide;
  }

  internal Dictionary<string, float> GetFormulaValues(
    AutoShapeType shapeType,
    Dictionary<string, string> formulaColl,
    bool isAdjValue)
  {
    if (formulaColl.Count == 0)
      return (Dictionary<string, float>) null;
    Dictionary<string, float> formulaValues = new Dictionary<string, float>();
    foreach (KeyValuePair<string, string> keyValuePair in formulaColl)
    {
      string[] splitFormula = keyValuePair.Value.Split(new char[1]
      {
        ' '
      }, StringSplitOptions.RemoveEmptyEntries);
      if (splitFormula.Length > 1)
      {
        float[] operandValues = this.GetOperandValues(shapeType, ref formulaValues, splitFormula, isAdjValue);
        float resultValue = this.GetResultValue(splitFormula[0], operandValues);
        formulaValues.Add(keyValuePair.Key, resultValue);
      }
    }
    return formulaValues;
  }

  internal float[] GetOperandValues(
    AutoShapeType shapeType,
    ref Dictionary<string, float> formulaValues,
    string[] splitFormula,
    bool isAdjValue)
  {
    string[] array = new string[34]
    {
      "3cd4",
      "3cd8",
      "5cd8",
      "7cd8",
      "b",
      "cd2",
      "cd4",
      "cd8",
      "h",
      "hc",
      "hd2",
      "hd3",
      "hd4",
      "hd5",
      "hd6",
      "hd8",
      "l",
      "ls",
      "r",
      "ss",
      "ssd2",
      "ssd4",
      "ssd6",
      "ssd8",
      "t",
      "vc",
      "w",
      "wd2",
      "wd3",
      "wd4",
      "wd5",
      "wd6",
      "wd8",
      "wd10"
    };
    Dictionary<string, float> dictionary = this._shapeGuide.Count <= 0 || isAdjValue ? this.GetDefaultPathAdjValues(shapeType) : this.GetPathAdjustValue(shapeType);
    float[] operandValues = new float[splitFormula.Length - 1];
    int index1 = 0;
    for (int index2 = 1; index2 < splitFormula.Length; ++index2)
    {
      if (!float.TryParse(splitFormula[index2], out operandValues[index1]))
      {
        if (Array.IndexOf<string>(array, splitFormula[index2]) > -1)
          operandValues[index1] = this.GetPresetOperandValue(splitFormula[index2]);
        else if (!isAdjValue && dictionary.ContainsKey(splitFormula[index2]))
          operandValues[index1] = dictionary[splitFormula[index2]];
        else if (formulaValues.ContainsKey(splitFormula[index2]))
          operandValues[index1] = formulaValues[splitFormula[index2]];
      }
      ++index1;
    }
    return operandValues;
  }

  internal Dictionary<string, float> GetPathAdjustValue(AutoShapeType shapeType)
  {
    Dictionary<string, float> formulaValues = this.GetFormulaValues(AutoShapeType.Unknown, this._shapeGuide, true);
    List<string> stringList = new List<string>((IEnumerable<string>) formulaValues.Keys);
    if (shapeType == AutoShapeType.CircularArrow)
    {
      foreach (string key in stringList)
      {
        if (key != "adj1" && key != "adj5")
          formulaValues[key] /= 60000f;
      }
    }
    return formulaValues;
  }

  private float GetResultValue(string formula, float[] operandValues)
  {
    char[] array = new char[4]{ '*', '/', '+', '-' };
    float resultValue = operandValues[0];
    if (formula.Length > 1 && Array.IndexOf<char>(array, formula[0]) > -1)
    {
      int index1 = 0;
      for (int index2 = 0; index2 < formula.Length; ++index2)
      {
        ++index1;
        switch (formula[index2])
        {
          case '*':
            if ((double) operandValues[index1] != 0.0)
            {
              resultValue *= operandValues[index1];
              break;
            }
            break;
          case '+':
            resultValue += operandValues[index1];
            break;
          case '-':
            resultValue -= operandValues[index1];
            break;
          case '/':
            if ((double) operandValues[index1] != 0.0)
            {
              resultValue /= operandValues[index1];
              break;
            }
            break;
        }
      }
    }
    else
    {
      switch (formula)
      {
        case "?:":
          resultValue = (double) operandValues[0] > 0.0 ? operandValues[1] : operandValues[2];
          break;
        case "abs":
          resultValue = Math.Abs(operandValues[0]);
          break;
        case "at2":
          resultValue = (float) Math.Atan((double) operandValues[1] / (double) operandValues[0]);
          break;
        case "cat2":
          float d = (float) Math.Atan((double) operandValues[2] / (double) operandValues[1]);
          resultValue = operandValues[0] * (float) Math.Cos((double) d);
          break;
        case "cos":
          double num1 = Math.Cos((double) operandValues[1] * Math.PI / 180.0);
          resultValue = operandValues[0] * (float) num1;
          break;
        case "max":
          resultValue = Math.Max(operandValues[0], operandValues[1]);
          break;
        case "min":
          resultValue = Math.Min(operandValues[0], operandValues[1]);
          break;
        case "mod":
          resultValue = (float) Math.Sqrt(Math.Pow((double) operandValues[0], 2.0) + Math.Pow((double) operandValues[1], 2.0) + Math.Pow((double) operandValues[2], 2.0));
          break;
        case "pin":
          resultValue = (double) operandValues[1] >= (double) operandValues[0] ? ((double) operandValues[1] <= (double) operandValues[2] ? operandValues[1] : operandValues[2]) : operandValues[0];
          break;
        case "sat2":
          float a = (float) Math.Atan((double) operandValues[2] / (double) operandValues[1]);
          resultValue = operandValues[0] * (float) Math.Sin((double) a);
          break;
        case "sin":
          double num2 = Math.Sin((double) operandValues[1] * Math.PI / 180.0);
          resultValue = operandValues[0] * (float) num2;
          break;
        case "sqrt":
          resultValue = (float) Math.Sqrt((double) operandValues[0]);
          break;
        case "tan":
          double num3 = Math.Tan((double) operandValues[1] * Math.PI / 180.0);
          resultValue = operandValues[0] * (float) num3;
          break;
        case "val":
          resultValue = operandValues[0];
          break;
      }
    }
    return resultValue;
  }

  internal float GetPresetOperandValue(string operand)
  {
    switch (operand)
    {
      case "3cd4":
        return 270f;
      case "3cd8":
        return 135f;
      case "5cd8":
        return 225f;
      case "7cd8":
        return 315f;
      case "b":
        return this._rectBounds.Height;
      case "cd2":
        return 180f;
      case "cd4":
        return 90f;
      case "cd8":
        return 45f;
      case "h":
        return this._rectBounds.Height;
      case "hc":
        return this._rectBounds.Width / 2f;
      case "hd2":
        return this._rectBounds.Height / 2f;
      case "hd3":
        return this._rectBounds.Height / 3f;
      case "hd4":
        return this._rectBounds.Height / 4f;
      case "hd5":
        return this._rectBounds.Height / 5f;
      case "hd6":
        return this._rectBounds.Height / 6f;
      case "hd8":
        return this._rectBounds.Height / 8f;
      case "l":
        return 0.0f;
      case "ls":
        return Math.Max(this._rectBounds.Width, this._rectBounds.Height);
      case "r":
        return this._rectBounds.Width;
      case "ss":
        return Math.Min(this._rectBounds.Width, this._rectBounds.Height);
      case "ssd2":
        return Math.Min(this._rectBounds.Width, this._rectBounds.Height) / 2f;
      case "ssd4":
        return Math.Min(this._rectBounds.Width, this._rectBounds.Height) / 4f;
      case "ssd6":
        return Math.Min(this._rectBounds.Width, this._rectBounds.Height) / 6f;
      case "ssd8":
        return Math.Min(this._rectBounds.Width, this._rectBounds.Height) / 8f;
      case "t":
        return 0.0f;
      case "vc":
        return this._rectBounds.Height / 2f;
      case "w":
        return this._rectBounds.Width;
      case "wd2":
        return this._rectBounds.Width / 2f;
      case "wd3":
        return this._rectBounds.Width / 3f;
      case "wd4":
        return this._rectBounds.Width / 4f;
      case "wd5":
        return this._rectBounds.Width / 5f;
      case "wd6":
        return this._rectBounds.Width / 6f;
      case "wd8":
        return this._rectBounds.Width / 8f;
      case "wd10":
        return this._rectBounds.Width / 10f;
      default:
        return 0.0f;
    }
  }

  internal Dictionary<string, float> ParseShapeFormula(AutoShapeType shapeType)
  {
    return this.GetFormulaValues(shapeType, this.GetShapeFormula(shapeType), false);
  }

  private Dictionary<string, string> GetShapeFormula(AutoShapeType shapeType)
  {
    Dictionary<string, string> shapeFormula = new Dictionary<string, string>();
    switch (shapeType)
    {
      case AutoShapeType.Parallelogram:
        shapeFormula.Add("maxAdj", "*/ 100000 w ss");
        shapeFormula.Add("a", "pin 0 adj maxAdj");
        shapeFormula.Add("x1", "*/ ss a 200000");
        shapeFormula.Add("x2", "*/ ss a 100000");
        shapeFormula.Add("x6", "+- r 0 x1");
        shapeFormula.Add("x5", "+- r 0 x2");
        shapeFormula.Add("x3", "*/ x5 1 2");
        shapeFormula.Add("x4", "+- r 0 x3");
        shapeFormula.Add("il1", "*/ wd2 a maxAdj");
        shapeFormula.Add("q1", "*/ 5 a maxAdj");
        shapeFormula.Add("q2", "+/ 1 q1 12");
        shapeFormula.Add("il", "*/ q2 w 1");
        shapeFormula.Add("it", "*/ q2 h 1");
        shapeFormula.Add("ir", "+- r 0 il");
        shapeFormula.Add("ib", "+- b 0 it");
        shapeFormula.Add("q3", "*/ h hc x2");
        shapeFormula.Add("y1", "pin 0 q3 h");
        shapeFormula.Add("y2", "+- b 0 y1");
        break;
      case AutoShapeType.Trapezoid:
        shapeFormula.Add("maxAdj", "*/ 50000 w ss");
        shapeFormula.Add("a", "pin 0 adj maxAdj");
        shapeFormula.Add("x1", "*/ ss a 200000");
        shapeFormula.Add("x2", "*/ ss a 100000");
        shapeFormula.Add("x3", "+- r 0 x2");
        shapeFormula.Add("x4", "+- r 0 x1");
        shapeFormula.Add("il", "*/ wd3 a maxAdj");
        shapeFormula.Add("it", "*/ hd3 a maxAdj");
        shapeFormula.Add("ir", "+- r 0 il");
        break;
      case AutoShapeType.Diamond:
        shapeFormula.Add("ir", "*/ w 3 4");
        shapeFormula.Add("ib", "*/ h 3 4");
        break;
      case AutoShapeType.RoundedRectangle:
        shapeFormula.Add("a", "pin 0 adj 50000");
        shapeFormula.Add("x1", "*/ ss a 100000");
        shapeFormula.Add("x2", "+- r 0 x1");
        shapeFormula.Add("y2", "+- b 0 x1");
        shapeFormula.Add("il", "*/ x1 29289 100000");
        shapeFormula.Add("ir", "+- r 0 il");
        shapeFormula.Add("ib", "+- b 0 il");
        break;
      case AutoShapeType.Octagon:
        shapeFormula.Add("a", "pin 0 adj 50000");
        shapeFormula.Add("x1", "*/ ss a 100000");
        shapeFormula.Add("x2", "+- r 0 x1");
        shapeFormula.Add("y2", "+- b 0 x1");
        shapeFormula.Add("il", "*/ x1 1 2");
        shapeFormula.Add("ir", "+- r 0 il");
        shapeFormula.Add("ib", "+- b 0 il");
        break;
      case AutoShapeType.IsoscelesTriangle:
        shapeFormula.Add("a", "pin 0 adj 100000");
        shapeFormula.Add("x1", "*/ w a 200000");
        shapeFormula.Add("x2", "*/ w a 100000");
        shapeFormula.Add("x3", "+- x1 wd2 0");
        break;
      case AutoShapeType.RightTriangle:
        shapeFormula.Add("it", "*/ h 7 12");
        shapeFormula.Add("ir", "*/ w 7 12");
        shapeFormula.Add("ib", "*/ h 11 12");
        break;
      case AutoShapeType.Oval:
        shapeFormula.Add("idx", "cos wd2 45");
        shapeFormula.Add("idy", "sin hd2 45");
        shapeFormula.Add("il", "+- hc 0 idx");
        shapeFormula.Add("ir", "+- hc idx 0");
        shapeFormula.Add("it", "+- vc 0 idy");
        shapeFormula.Add("ib", "+- vc idy 0");
        break;
      case AutoShapeType.Hexagon:
        shapeFormula.Add("maxAdj", "*/ 50000 w ss");
        shapeFormula.Add("a", "pin 0 adj maxAdj");
        shapeFormula.Add("shd2", "*/ hd2 vf 100000");
        shapeFormula.Add("x1", "*/ ss a 100000");
        shapeFormula.Add("x2", "+- r 0 x1");
        shapeFormula.Add("dy1", "sin shd2 60");
        shapeFormula.Add("y1", "+- vc 0 dy1");
        shapeFormula.Add("y2", "+- vc dy1 0");
        shapeFormula.Add("q1", "*/ maxAdj -1 2");
        shapeFormula.Add("q2", "+- a q1 0");
        shapeFormula.Add("q3", "?: q2 4 2");
        shapeFormula.Add("q4", "?: q2 3 2");
        shapeFormula.Add("q5", "?: q2 q1 0");
        shapeFormula.Add("q6", "+/ a q5 q1");
        shapeFormula.Add("q7", "*/ q6 q4 -1");
        shapeFormula.Add("q8", "+- q3 q7 0");
        shapeFormula.Add("il", "*/ w q8 24");
        shapeFormula.Add("it", "*/ h q8 24");
        shapeFormula.Add("ir", "+- r 0 il");
        shapeFormula.Add("ib", "+- b 0 it");
        break;
      case AutoShapeType.Cross:
        shapeFormula.Add("a", "pin 0 adj 50000");
        shapeFormula.Add("x1", "*/ ss a 100000");
        shapeFormula.Add("x2", "+- r 0 x1");
        shapeFormula.Add("y2", "+- b 0 x1");
        shapeFormula.Add("d", "+- w 0 h");
        shapeFormula.Add("il", "?: d l x1");
        shapeFormula.Add("ir", "?: d r x2");
        shapeFormula.Add("it", "?: d x1 t");
        shapeFormula.Add("ib", "?: d y2 b");
        break;
      case AutoShapeType.RegularPentagon:
        shapeFormula.Add("swd2", "*/ wd2 hf 100000");
        shapeFormula.Add("shd2", "*/ hd2 vf 100000");
        shapeFormula.Add("svc", "*/ vc  vf 100000");
        shapeFormula.Add("dx1", "cos swd2 18");
        shapeFormula.Add("dx2", "cos swd2 306");
        shapeFormula.Add("dy1", "sin shd2 18");
        shapeFormula.Add("dy2", "sin shd2 306");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x2", "+- hc 0 dx2");
        shapeFormula.Add("x3", "+- hc dx2 0");
        shapeFormula.Add("x4", "+- hc dx1 0");
        shapeFormula.Add("y1", "+- svc 0 dy1");
        shapeFormula.Add("y2", "+- svc 0 dy2");
        shapeFormula.Add("it", "*/ y1 dx2 dx1");
        break;
      case AutoShapeType.Can:
        shapeFormula.Add("maxAdj", "*/ 50000 h ss");
        shapeFormula.Add("a", "pin 0 adj maxAdj");
        shapeFormula.Add("y1", "*/ ss a 200000");
        shapeFormula.Add("y2", "+- y1 y1 0");
        shapeFormula.Add("y3", "+- b 0 y1");
        break;
      case AutoShapeType.Cube:
        shapeFormula.Add("a", "pin 0 adj 100000");
        shapeFormula.Add("y1", "*/ ss a 100000");
        shapeFormula.Add("y4", "+- b 0 y1");
        shapeFormula.Add("y2", "*/ y4 1 2");
        shapeFormula.Add("y3", "+/ y1 b 2");
        shapeFormula.Add("x4", "+- r 0 y1");
        shapeFormula.Add("x2", "*/ x4 1 2");
        shapeFormula.Add("x3", "+/ y1 r 2");
        break;
      case AutoShapeType.Bevel:
        shapeFormula.Add("a", "pin 0 adj 50000");
        shapeFormula.Add("x1", "*/ ss a 100000");
        shapeFormula.Add("x2", "+- r 0 x1");
        shapeFormula.Add("y2", "+- b 0 x1");
        break;
      case AutoShapeType.FoldedCorner:
        shapeFormula.Add("a", "pin 0 adj 50000");
        shapeFormula.Add("dy2", "*/ ss a 100000");
        shapeFormula.Add("dy1", "*/ dy2 1 5");
        shapeFormula.Add("x1", "+- r 0 dy2");
        shapeFormula.Add("x2", "+- x1 dy1 0");
        shapeFormula.Add("y2", "+- b 0 dy2");
        shapeFormula.Add("y1", "+- y2 dy1 0");
        break;
      case AutoShapeType.SmileyFace:
        shapeFormula.Add("a", "pin -4653 adj 4653");
        shapeFormula.Add("x1", "*/ w 4969 21699");
        shapeFormula.Add("x2", "*/ w 6215 21600");
        shapeFormula.Add("x3", "*/ w 13135 21600");
        shapeFormula.Add("x4", "*/ w 16640 21600");
        shapeFormula.Add("y1", "*/ h 7570 21600");
        shapeFormula.Add("y3", "*/ h 16515 21600");
        shapeFormula.Add("dy2", "*/ h a 100000");
        shapeFormula.Add("y2", "+- y3 0 dy2");
        shapeFormula.Add("y4", "+- y3 dy2 0");
        shapeFormula.Add("dy3", "*/ h a 50000");
        shapeFormula.Add("y5", "+- y4 dy3 0");
        shapeFormula.Add("idx", "cos wd2 45");
        shapeFormula.Add("idy", "sin hd2 45");
        shapeFormula.Add("il", "+- hc 0 idx");
        shapeFormula.Add("ir", "+- hc idx 0");
        shapeFormula.Add("it", "+- vc 0 idy");
        shapeFormula.Add("ib", "+- vc idy 0");
        shapeFormula.Add("wR", "*/ w 1125 21600");
        shapeFormula.Add("hR", "*/ h 1125 21600");
        break;
      case AutoShapeType.Donut:
        shapeFormula.Add("a", "pin 0 adj 50000");
        shapeFormula.Add("dr", "*/ ss a 100000");
        shapeFormula.Add("iwd2", "+- wd2 0 dr");
        shapeFormula.Add("ihd2", "+- hd2 0 dr");
        shapeFormula.Add("idx", "cos wd2 2700000");
        shapeFormula.Add("idy", "sin hd2 2700000");
        shapeFormula.Add("il", "+- hc 0 idx");
        shapeFormula.Add("ir", "+- hc idx 0");
        shapeFormula.Add("it", "+- vc 0 idy");
        shapeFormula.Add("ib", "+- vc idy 0");
        break;
      case AutoShapeType.NoSymbol:
        shapeFormula.Add("a", "pin 0 adj 50000");
        shapeFormula.Add("dr", "*/ ss a 100000");
        shapeFormula.Add("iwd2", "+- wd2 0 dr");
        shapeFormula.Add("ihd2", "+- hd2 0 dr");
        shapeFormula.Add("ang3", "at2 w h");
        shapeFormula.Add("ang", "*/ ang3 180 " + Math.PI.ToString());
        shapeFormula.Add("ct", "cos ihd2 ang");
        shapeFormula.Add("st", "sin iwd2 ang");
        shapeFormula.Add("m", "mod ct st 0");
        shapeFormula.Add("n", "*/ iwd2 ihd2 m");
        shapeFormula.Add("drd2", "*/ dr 1 2");
        shapeFormula.Add("dang3", "at2 n drd2");
        shapeFormula.Add("dang", "*/ dang3 180 " + Math.PI.ToString());
        shapeFormula.Add("2dang", "*/ dang 2 1");
        shapeFormula.Add("swAng", "+- -180 2dang 0");
        shapeFormula.Add("t4", "at2 w h");
        shapeFormula.Add("t3", "*/ t4 180 " + Math.PI.ToString());
        shapeFormula.Add("stAng1", "+- t3 0 dang");
        shapeFormula.Add("stAng2", "+- stAng1 0 cd2");
        shapeFormula.Add("ct1", "cos ihd2 stAng1");
        shapeFormula.Add("st1", "sin iwd2 stAng1");
        shapeFormula.Add("m1", "mod ct1 st1 0");
        shapeFormula.Add("n1", "*/ iwd2 ihd2 m1");
        shapeFormula.Add("dx1", "cos n1 stAng1");
        shapeFormula.Add("dy1", "sin n1 stAng1");
        shapeFormula.Add("x1", "+- hc dx1 0");
        shapeFormula.Add("y1", "+- vc dy1 0");
        shapeFormula.Add("x2", "+- hc 0 dx1");
        shapeFormula.Add("y2", "+- vc 0 dy1");
        shapeFormula.Add("idx", "cos wd2 45");
        shapeFormula.Add("idy", "sin hd2 45");
        shapeFormula.Add("il", "+- hc 0 idx");
        shapeFormula.Add("ir", "+- hc idx 0");
        shapeFormula.Add("it", "+- vc 0 idy");
        shapeFormula.Add("ib", "+- vc idy 0");
        break;
      case AutoShapeType.BlockArc:
        shapeFormula.Add("stAng", "pin 0 adj1 21599999");
        shapeFormula.Add("istAng", "pin 0 adj2 21599999");
        shapeFormula.Add("a3", "pin 0 adj3 50000");
        shapeFormula.Add("sw11", "+- istAng 0 stAng");
        shapeFormula.Add("sw12", "+- sw11 21600000 0");
        shapeFormula.Add("swAng", "?: sw11 sw11 sw12");
        shapeFormula.Add("iswAng", "+- 0 0 swAng");
        shapeFormula.Add("wt1", "sin wd2 stAng");
        shapeFormula.Add("ht1", "cos hd2 stAng");
        shapeFormula.Add("wt3", "sin wd2 istAng");
        shapeFormula.Add("ht3", "cos hd2 istAng");
        shapeFormula.Add("dx1", "cat2 wd2 ht1 wt1");
        shapeFormula.Add("dy1", "sat2 hd2 ht1 wt1");
        shapeFormula.Add("dx3", "cat2 wd2 ht3 wt3");
        shapeFormula.Add("dy3", "sat2 hd2 ht3 wt3");
        shapeFormula.Add("x1", "+- hc dx1 0");
        shapeFormula.Add("y1", "+- vc dy1 0");
        shapeFormula.Add("x3", "+- hc dx3 0");
        shapeFormula.Add("y3", "+- vc dy3 0");
        shapeFormula.Add("dr", "*/ ss a3 100000");
        shapeFormula.Add("iwd2", "+- wd2 0 dr");
        shapeFormula.Add("ihd2", "+- hd2 0 dr");
        shapeFormula.Add("wt2", "sin iwd2 istAng");
        shapeFormula.Add("ht2", "cos ihd2 istAng");
        shapeFormula.Add("wt4", "sin iwd2 stAng");
        shapeFormula.Add("ht4", "cos ihd2 stAng");
        shapeFormula.Add("dx2", "cat2 iwd2 ht2 wt2");
        shapeFormula.Add("dy2", "sat2 ihd2 ht2 wt2");
        shapeFormula.Add("dx4", "cat2 iwd2 ht4 wt4");
        shapeFormula.Add("dy4", "sat2 ihd2 ht4 wt4");
        shapeFormula.Add("x2", "+- hc dx2 0");
        shapeFormula.Add("y2", "+- vc dy2 0");
        shapeFormula.Add("x4", "+- hc dx4 0");
        shapeFormula.Add("y4", "+- vc dy4 0");
        shapeFormula.Add("sw0", "+- 21600000 0 stAng");
        shapeFormula.Add("da1", "+- swAng 0 sw0");
        shapeFormula.Add("g1", "max x1 x2");
        shapeFormula.Add("g2", "max x3 x4");
        shapeFormula.Add("g3", "max g1 g2");
        shapeFormula.Add("ir", "?: da1 r g3");
        shapeFormula.Add("sw1", "+- cd4 0 stAng");
        shapeFormula.Add("sw2", "+- 27000000 0 stAng");
        shapeFormula.Add("sw3", "?: sw1 sw1 sw2");
        shapeFormula.Add("da2", "+- swAng 0 sw3");
        shapeFormula.Add("g5", "max y1 y2");
        shapeFormula.Add("g6", "max y3 y4");
        shapeFormula.Add("g7", "max g5 g6");
        shapeFormula.Add("ib", "?: da2 b g7");
        shapeFormula.Add("sw4", "+- cd2 0 stAng");
        shapeFormula.Add("sw5", "+- 32400000 0 stAng");
        shapeFormula.Add("sw6", "?: sw4 sw4 sw5");
        shapeFormula.Add("da3", "+- swAng 0 sw6");
        shapeFormula.Add("g9", "min x1 x2");
        shapeFormula.Add("g10", "min x3 x4");
        shapeFormula.Add("g11", "min g9 g10");
        shapeFormula.Add("il", "?: da3 l g11");
        shapeFormula.Add("sw7", "+- 3cd4 0 stAng");
        shapeFormula.Add("sw8", "+- 37800000 0 stAng");
        shapeFormula.Add("sw9", "?: sw7 sw7 sw8");
        shapeFormula.Add("da4", "+- swAng 0 sw9");
        shapeFormula.Add("g13", "min y1 y2");
        shapeFormula.Add("g14", "min y3 y4");
        shapeFormula.Add("g15", "min g13 g14");
        shapeFormula.Add("it", "?: da4 t g15");
        shapeFormula.Add("x5", "+/ x1 x4 2");
        shapeFormula.Add("y5", "+/ y1 y4 2");
        shapeFormula.Add("x6", "+/ x3 x2 2");
        shapeFormula.Add("y6", "+/ y3 y2 2");
        shapeFormula.Add("cang1", "+- stAng 0 cd4");
        shapeFormula.Add("cang2", "+- istAng cd4 0");
        shapeFormula.Add("cang3", "+/ cang1 cang2 2");
        break;
      case AutoShapeType.Heart:
        shapeFormula.Add("dx1", "*/ w 49 48");
        shapeFormula.Add("dx2", "*/ w 10 48");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x2", "+- hc 0 dx2");
        shapeFormula.Add("x3", "+- hc dx2 0");
        shapeFormula.Add("x4", "+- hc dx1 0");
        shapeFormula.Add("y1", "+- t 0 hd3");
        shapeFormula.Add("il", "*/ w 1 6");
        shapeFormula.Add("ir", "*/ w 5 6");
        shapeFormula.Add("ib", "*/ h 2 3");
        break;
      case AutoShapeType.LightningBolt:
        shapeFormula.Add("x1", "*/ w 5022 21600");
        shapeFormula.Add("x3", "*/ w 8472 21600");
        shapeFormula.Add("x4", "*/ w 8757 21600");
        shapeFormula.Add("x5", "*/ w 10012 21600");
        shapeFormula.Add("x8", "*/ w 12860 21600");
        shapeFormula.Add("x9", "*/ w 13917 21600");
        shapeFormula.Add("x11", "*/ w 16577 21600");
        shapeFormula.Add("y1", "*/ h 3890 21600");
        shapeFormula.Add("y2", "*/ h 6080 21600");
        shapeFormula.Add("y4", "*/ h 7437 21600");
        shapeFormula.Add("y6", "*/ h 9705 21600");
        shapeFormula.Add("y7", "*/ h 12007 21600");
        shapeFormula.Add("y10", "*/ h 14277 21600");
        shapeFormula.Add("y11", "*/ h 14915 21600");
        break;
      case AutoShapeType.Sun:
        shapeFormula.Add("a", "pin 12500 adj 46875");
        shapeFormula.Add("g0", "+- 50000 0 a");
        shapeFormula.Add("g1", "*/ g0 30274 32768");
        shapeFormula.Add("g2", "*/ g0 12540 32768");
        shapeFormula.Add("g3", "+- g1 50000 0");
        shapeFormula.Add("g4", "+- g2 50000 0");
        shapeFormula.Add("g5", "+- 50000 0 g1");
        shapeFormula.Add("g6", "+- 50000 0 g2");
        shapeFormula.Add("g7", "*/ g0 23170 32768");
        shapeFormula.Add("g8", "+- 50000 g7 0");
        shapeFormula.Add("g9", "+- 50000 0 g7");
        shapeFormula.Add("g10", "*/ g5 3 4");
        shapeFormula.Add("g11", "*/ g6 3 4");
        shapeFormula.Add("g12", "+- g10 3662 0");
        shapeFormula.Add("g13", "+- g11 3662 0");
        shapeFormula.Add("g14", "+- g11 12500 0");
        shapeFormula.Add("g15", "+- 100000 0 g10");
        shapeFormula.Add("g16", "+- 100000 0 g12");
        shapeFormula.Add("g17", "+- 100000 0 g13");
        shapeFormula.Add("g18", "+- 100000 0 g14");
        shapeFormula.Add("ox1", "*/ w 18436 21600");
        shapeFormula.Add("oy1", "*/ h 3163 21600");
        shapeFormula.Add("ox2", "*/ w 3163 21600");
        shapeFormula.Add("oy2", "*/ h 18436 21600");
        shapeFormula.Add("x8", "*/ w g8 100000");
        shapeFormula.Add("x9", "*/ w g9 100000");
        shapeFormula.Add("x10", "*/ w g10 100000");
        shapeFormula.Add("x12", "*/ w g12 100000");
        shapeFormula.Add("x13", "*/ w g13 100000");
        shapeFormula.Add("x14", "*/ w g14 100000");
        shapeFormula.Add("x15", "*/ w g15 100000");
        shapeFormula.Add("x16", "*/ w g16 100000");
        shapeFormula.Add("x17", "*/ w g17 100000");
        shapeFormula.Add("x18", "*/ w g18 100000");
        shapeFormula.Add("x19", "*/ w a 100000");
        shapeFormula.Add("wR", "*/ w g0 100000");
        shapeFormula.Add("hR", "*/ h g0 100000");
        shapeFormula.Add("y8", "*/ h g8 100000");
        shapeFormula.Add("y9", "*/ h g9 100000");
        shapeFormula.Add("y10", "*/ h g10 100000");
        shapeFormula.Add("y12", "*/ h g12 100000");
        shapeFormula.Add("y13", "*/ h g13 100000");
        shapeFormula.Add("y14", "*/ h g14 100000");
        shapeFormula.Add("y15", "*/ h g15 100000");
        shapeFormula.Add("y16", "*/ h g16 100000");
        shapeFormula.Add("y17", "*/ h g17 100000");
        shapeFormula.Add("y18", "*/ h g18 100000");
        break;
      case AutoShapeType.Moon:
        shapeFormula.Add("a", "pin 0 adj 87500");
        shapeFormula.Add("g0", "*/ ss a 100000");
        shapeFormula.Add("g0w", "*/ g0 w ss");
        shapeFormula.Add("g1", "+- ss 0 g0");
        shapeFormula.Add("g2", "*/ g0 g0 g1");
        shapeFormula.Add("g3", "*/ ss ss g1");
        shapeFormula.Add("g4", "*/ g3 2 1");
        shapeFormula.Add("g5", "+- g4 0 g2");
        shapeFormula.Add("g6", "+- g5 0 g0");
        shapeFormula.Add("g6w", "*/ g6 w ss");
        shapeFormula.Add("g7", "*/ g5 1 2");
        shapeFormula.Add("g8", "+- g7 0 g0");
        shapeFormula.Add("dy1", "*/ g8 hd2 ss");
        shapeFormula.Add("g10h", "+- vc 0 dy1");
        shapeFormula.Add("g11h", "+- vc dy1 0");
        shapeFormula.Add("g12", "*/ g0 9598 32768");
        shapeFormula.Add("g12w", "*/ g12 w ss");
        shapeFormula.Add("g13", "+- ss 0 g12");
        shapeFormula.Add("q1", "*/ ss ss 1");
        shapeFormula.Add("q2", "*/ g13 g13 1");
        shapeFormula.Add("q3", "+- q1 0 q2");
        shapeFormula.Add("q4", "sqrt q3");
        shapeFormula.Add("dy4", "*/ q4 hd2 ss");
        shapeFormula.Add("g15h", "+- vc 0 dy4");
        shapeFormula.Add("g16h", "+- vc dy4 0");
        shapeFormula.Add("g17w", "+- g6w 0 g0w");
        shapeFormula.Add("g18w", "*/ g17w 1 2");
        shapeFormula.Add("dx2p", "+- g0w g18w w");
        shapeFormula.Add("dx2", "*/ dx2p -1 1");
        shapeFormula.Add("dy2", "*/ hd2 -1 1");
        shapeFormula.Add("stAng", "at2 dx2 dy2");
        shapeFormula.Add("stAng1", "*/ stAng 180 " + Math.PI.ToString());
        shapeFormula.Add("enAngp", "at2 dx2 hd2");
        shapeFormula.Add("enAngp1", "*/ enAngp 180 " + Math.PI.ToString());
        shapeFormula.Add("enAng1", "+- enAngp1 0 360");
        shapeFormula.Add("swAng1", "+- enAng1 0 stAng1");
        break;
      case AutoShapeType.Arc:
        shapeFormula.Add("stAng", "pin 0 adj1 21599999");
        shapeFormula.Add("enAng", "pin 0 adj2 21599999");
        shapeFormula.Add("sw11", "+- enAng 0 stAng");
        shapeFormula.Add("sw12", "+- sw11 21600000 0");
        shapeFormula.Add("swAng", "?: sw11 sw11 sw12");
        shapeFormula.Add("wt1", "sin wd2 stAng");
        shapeFormula.Add("ht1", "cos hd2 stAng");
        shapeFormula.Add("dx1", "cat2 wd2 ht1 wt1");
        shapeFormula.Add("dy1", "sat2 hd2 ht1 wt1");
        shapeFormula.Add("wt2", "sin wd2 enAng");
        shapeFormula.Add("ht2", "cos hd2 enAng");
        shapeFormula.Add("dx2", "cat2 wd2 ht2 wt2");
        shapeFormula.Add("dy2", "sat2 hd2 ht2 wt2");
        shapeFormula.Add("x1", "+- hc dx1 0");
        shapeFormula.Add("y1", "+- vc dy1 0");
        shapeFormula.Add("x2", "+- hc dx2 0");
        shapeFormula.Add("y2", "+- vc dy2 0");
        shapeFormula.Add("sw0", "+- 21600000 0 stAng");
        shapeFormula.Add("da1", "+- swAng 0 sw0");
        shapeFormula.Add("g1", "max x1 x2");
        shapeFormula.Add("ir", "?: da1 r g1");
        shapeFormula.Add("sw1", "+- cd4 0 stAng");
        shapeFormula.Add("sw2", "+- 27000000 0 stAng");
        shapeFormula.Add("sw3", "?: sw1 sw1 sw2");
        shapeFormula.Add("da2", "+- swAng 0 sw3");
        shapeFormula.Add("g5", "max y1 y2");
        shapeFormula.Add("ib", "?: da2 b g5");
        shapeFormula.Add("sw4", "+- cd2 0 stAng");
        shapeFormula.Add("sw5", "+- 32400000 0 stAng");
        shapeFormula.Add("sw6", "?: sw4 sw4 sw5");
        shapeFormula.Add("da3", "+- swAng 0 sw6");
        shapeFormula.Add("g9", "min x1 x2");
        shapeFormula.Add("il", "?: da3 l g9");
        shapeFormula.Add("sw7", "+- 3cd4 0 stAng");
        shapeFormula.Add("sw8", "+- 37800000 0 stAng");
        shapeFormula.Add("sw9", "?: sw7 sw7 sw8");
        shapeFormula.Add("da4", "+- swAng 0 sw9");
        shapeFormula.Add("g13", "min y1 y2");
        shapeFormula.Add("it", "?: da4 t g13");
        shapeFormula.Add("cang1", "+- stAng 0 cd4");
        shapeFormula.Add("cang2", "+- enAng cd4 0");
        shapeFormula.Add("cang3", "+/ cang1 cang2 2");
        break;
      case AutoShapeType.DoubleBracket:
        shapeFormula.Add("a", "pin 0 adj 50000");
        shapeFormula.Add("x1", "*/ ss a 100000");
        shapeFormula.Add("x2", "+- r 0 x1");
        shapeFormula.Add("y2", "+- b 0 x1");
        shapeFormula.Add("il", "*/ x1 29289 100000");
        shapeFormula.Add("ir", "+- r 0 il");
        shapeFormula.Add("ib", "+- b 0 il");
        break;
      case AutoShapeType.DoubleBrace:
        shapeFormula.Add("a", "pin 0 adj 25000");
        shapeFormula.Add("x1", "*/ ss a 100000");
        shapeFormula.Add("x2", "*/ ss a 50000");
        shapeFormula.Add("x3", "+- r 0 x2");
        shapeFormula.Add("x4", "+- r 0 x1");
        shapeFormula.Add("y2", "+- vc 0 x1");
        shapeFormula.Add("y3", "+- vc x1 0");
        shapeFormula.Add("y4", "+- b 0 x1");
        shapeFormula.Add("it", "*/ x1 29289 100000");
        shapeFormula.Add("il", "+- x1 it 0");
        shapeFormula.Add("ir", "+- r 0 il");
        shapeFormula.Add("ib", "+- b 0 it");
        break;
      case AutoShapeType.Plaque:
        shapeFormula.Add("a", "pin 0 adj 50000");
        shapeFormula.Add("x1", "*/ ss a 100000");
        shapeFormula.Add("x2", "+- r 0 x1");
        shapeFormula.Add("y2", "+- b 0 x1");
        shapeFormula.Add("il", "*/ x1 70711 100000");
        shapeFormula.Add("ir", "+- r 0 il");
        shapeFormula.Add("ib", "+- b 0 il");
        break;
      case AutoShapeType.LeftBracket:
        shapeFormula.Add("maxAdj", "*/ 50000 h ss");
        shapeFormula.Add("a", "pin 0 adj maxAdj");
        shapeFormula.Add("y1", "*/ ss a 100000");
        shapeFormula.Add("y2", "+- b 0 y1");
        shapeFormula.Add("dx1", "cos w 2700000");
        shapeFormula.Add("dy1", "sin y1 2700000");
        shapeFormula.Add("il", "+- r 0 dx1");
        shapeFormula.Add("it", "+- y1 0 dy1");
        shapeFormula.Add("ib", "+- b dy1 y1");
        break;
      case AutoShapeType.RightBracket:
        shapeFormula.Add("maxAdj", "*/ 50000 h ss");
        shapeFormula.Add("a", "pin 0 adj maxAdj");
        shapeFormula.Add("y1", "*/ ss a 100000");
        shapeFormula.Add("y2", "+- b 0 y1");
        shapeFormula.Add("dx1", "cos w 2700000");
        shapeFormula.Add("dy1", "sin y1 2700000");
        shapeFormula.Add("ir", "+- l dx1 0");
        shapeFormula.Add("it", "+- y1 0 dy1");
        shapeFormula.Add("ib", "+- b dy1 y1");
        break;
      case AutoShapeType.LeftBrace:
        shapeFormula.Add("a2", "pin 0 adj2 100000");
        shapeFormula.Add("q1", "+- 100000 0 a2");
        shapeFormula.Add("q2", "min q1 a2");
        shapeFormula.Add("q3", "*/ q2 1 2");
        shapeFormula.Add("maxAdj1", "*/ q3 h ss");
        shapeFormula.Add("a1", "pin 0 adj1 maxAdj1");
        shapeFormula.Add("y1", "*/ ss a1 100000");
        shapeFormula.Add("y3", "*/ h a2 100000");
        shapeFormula.Add("y4", "+- y3 y1 0");
        shapeFormula.Add("dx1", "cos wd2 2700000");
        shapeFormula.Add("dy1", "sin y1 2700000");
        shapeFormula.Add("il", "+- r 0 dx1");
        shapeFormula.Add("it", "+- y1 0 dy1");
        shapeFormula.Add("ib", "+- b dy1 y1");
        break;
      case AutoShapeType.RightBrace:
        shapeFormula.Add("a2", "pin 0 adj2 100000");
        shapeFormula.Add("q1", "+- 100000 0 a2");
        shapeFormula.Add("q2", "min q1 a2");
        shapeFormula.Add("q3", "*/ q2 1 2");
        shapeFormula.Add("maxAdj1", "*/ q3 h ss");
        shapeFormula.Add("a1", "pin 0 adj1 maxAdj1");
        shapeFormula.Add("y1", "*/ ss a1 100000");
        shapeFormula.Add("y3", "*/ h a2 100000");
        shapeFormula.Add("y2", "+- y3 0 y1");
        shapeFormula.Add("y4", "+- b 0 y1");
        shapeFormula.Add("dx1", "cos wd2 2700000");
        shapeFormula.Add("dy1", "sin y1 2700000");
        shapeFormula.Add("ir", "+- l dx1 0");
        shapeFormula.Add("it", "+- y1 0 dy1");
        shapeFormula.Add("ib", "+- b dy1 y1");
        break;
      case AutoShapeType.RightArrow:
        shapeFormula.Add("maxAdj2", "*/ 100000 w ss");
        shapeFormula.Add("a1", "pin 0 adj1 100000");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("dx1", "*/ ss a2 100000");
        shapeFormula.Add("x1", "+- r 0 dx1");
        shapeFormula.Add("dy1", "*/ h a1 200000");
        shapeFormula.Add("y1", "+- vc 0 dy1");
        shapeFormula.Add("y2", "+- vc dy1 0");
        shapeFormula.Add("dx2", "*/ y1 dx1 hd2");
        shapeFormula.Add("x2", "+- x1 dx2 0");
        break;
      case AutoShapeType.LeftArrow:
        shapeFormula.Add("maxAdj2", "*/ 100000 w ss");
        shapeFormula.Add("a1", "pin 0 adj1 100000");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("dx2", "*/ ss a2 100000");
        shapeFormula.Add("x2", "+- l dx2 0");
        shapeFormula.Add("dy1", "*/ h a1 200000");
        shapeFormula.Add("y1", "+- vc 0 dy1");
        shapeFormula.Add("y2", "+- vc dy1 0");
        shapeFormula.Add("dx1", "*/ y1 dx2 hd2");
        shapeFormula.Add("x1", "+- x2  0 dx1");
        break;
      case AutoShapeType.UpArrow:
        shapeFormula.Add("maxAdj2", "*/ 100000 h ss");
        shapeFormula.Add("a1", "pin 0 adj1 100000");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("dy2", "*/ ss a2 100000");
        shapeFormula.Add("y2", "+- t dy2 0");
        shapeFormula.Add("dx1", "*/ w a1 200000");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x2", "+- hc dx1 0");
        shapeFormula.Add("dy1", "*/ x1 dy2 wd2");
        shapeFormula.Add("y1", "+- y2  0 dy1");
        break;
      case AutoShapeType.DownArrow:
        shapeFormula.Add("maxAdj2", "*/ 100000 h ss");
        shapeFormula.Add("a1", "pin 0 adj1 100000");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("dy1", "*/ ss a2 100000");
        shapeFormula.Add("y1", "+- b 0 dy1");
        shapeFormula.Add("dx1", "*/ w a1 200000");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x2", "+- hc dx1 0");
        shapeFormula.Add("dy2", "*/ x1 dy1 wd2");
        shapeFormula.Add("y2", "+- y1 dy2 0");
        break;
      case AutoShapeType.LeftRightArrow:
        shapeFormula.Add("maxAdj2", "*/ 50000 w ss");
        shapeFormula.Add("a1", "pin 0 adj1 100000");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("x2", "*/ ss a2 100000");
        shapeFormula.Add("x3", "+- r 0 x2");
        shapeFormula.Add("dy", "*/ h a1 200000");
        shapeFormula.Add("y1", "+- vc 0 dy");
        shapeFormula.Add("y2", "+- vc dy 0");
        shapeFormula.Add("dx1", "*/ y1 x2 hd2");
        shapeFormula.Add("x1", "+- x2 0 dx1");
        shapeFormula.Add("x4", "+- x3 dx1 0");
        break;
      case AutoShapeType.UpDownArrow:
        shapeFormula.Add("maxAdj2", "*/ 50000 h ss");
        shapeFormula.Add("a1", "pin 0 adj1 100000");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("y2", "*/ ss a2 100000");
        shapeFormula.Add("y3", "+- b 0 y2");
        shapeFormula.Add("dx1", "*/ w a1 200000");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x2", "+- hc dx1 0");
        shapeFormula.Add("dy1", "*/ x1 y2 wd2");
        shapeFormula.Add("y1", "+- y2 0 dy1");
        shapeFormula.Add("y4", "+- y3 dy1 0");
        break;
      case AutoShapeType.QuadArrow:
        shapeFormula.Add("a2", "pin 0 adj2 50000");
        shapeFormula.Add("maxAdj1", "*/ a2 2 1");
        shapeFormula.Add("a1", "pin 0 adj1 maxAdj1");
        shapeFormula.Add("q1", "+- 100000 0 maxAdj1");
        shapeFormula.Add("maxAdj3", "*/ q1 1 2");
        shapeFormula.Add("a3", "pin 0 adj3 maxAdj3");
        shapeFormula.Add("x1", "*/ ss a3 100000");
        shapeFormula.Add("dx2", "*/ ss a2 100000");
        shapeFormula.Add("x2", "+- hc 0 dx2");
        shapeFormula.Add("x5", "+- hc dx2 0");
        shapeFormula.Add("dx3", "*/ ss a1 200000");
        shapeFormula.Add("x3", "+- hc 0 dx3");
        shapeFormula.Add("x4", "+- hc dx3 0");
        shapeFormula.Add("x6", "+- r 0 x1");
        shapeFormula.Add("y2", "+- vc 0 dx2");
        shapeFormula.Add("y5", "+- vc dx2 0");
        shapeFormula.Add("y3", "+- vc 0 dx3");
        shapeFormula.Add("y4", "+- vc dx3 0");
        shapeFormula.Add("y6", "+- b 0 x1");
        shapeFormula.Add("il", "*/ dx3 x1 dx2");
        shapeFormula.Add("ir", "+- r 0 il");
        break;
      case AutoShapeType.LeftRightUpArrow:
        shapeFormula.Add("a2", "pin 0 adj2 50000");
        shapeFormula.Add("maxAdj1", "*/ a2 2 1");
        shapeFormula.Add("a1", "pin 0 adj1 maxAdj1");
        shapeFormula.Add("q1", "+- 100000 0 maxAdj1");
        shapeFormula.Add("maxAdj3", "*/ q1 1 2");
        shapeFormula.Add("a3", "pin 0 adj3 maxAdj3");
        shapeFormula.Add("x1", "*/ ss a3 100000");
        shapeFormula.Add("dx2", "*/ ss a2 100000");
        shapeFormula.Add("x2", "+- hc 0 dx2");
        shapeFormula.Add("x5", "+- hc dx2 0");
        shapeFormula.Add("dx3", "*/ ss a1 200000");
        shapeFormula.Add("x3", "+- hc 0 dx3");
        shapeFormula.Add("x4", "+- hc dx3 0");
        shapeFormula.Add("x6", "+- r 0 x1");
        shapeFormula.Add("dy2", "*/ ss a2 50000");
        shapeFormula.Add("y2", "+- b 0 dy2");
        shapeFormula.Add("y4", "+- b 0 dx2");
        shapeFormula.Add("y3", "+- y4 0 dx3");
        shapeFormula.Add("y5", "+- y4 dx3 0");
        shapeFormula.Add("il", "*/ dx3 x1 dx2");
        shapeFormula.Add("ir", "+- r 0 il");
        break;
      case AutoShapeType.BentArrow:
        shapeFormula.Add("a2", "pin 0 adj2 50000");
        shapeFormula.Add("maxAdj1", "*/ a2 2 1");
        shapeFormula.Add("a1", "pin 0 adj1 maxAdj1");
        shapeFormula.Add("a3", "pin 0 adj3 50000");
        shapeFormula.Add("th", "*/ ss a1 100000");
        shapeFormula.Add("aw2", "*/ ss a2 100000");
        shapeFormula.Add("th2", "*/ th 1 2");
        shapeFormula.Add("dh2", "+- aw2 0 th2");
        shapeFormula.Add("ah", "*/ ss a3 100000");
        shapeFormula.Add("bw", "+- r 0 ah");
        shapeFormula.Add("bh", "+- b 0 dh2");
        shapeFormula.Add("bs", "min bw bh");
        shapeFormula.Add("maxAdj4", "*/ 100000 bs ss");
        shapeFormula.Add("a4", "pin 0 adj4 maxAdj4");
        shapeFormula.Add("bd", "*/ ss a4 100000");
        shapeFormula.Add("bd3", "+- bd 0 th");
        shapeFormula.Add("bd2", "max bd3 0");
        shapeFormula.Add("x3", "+- th bd2 0");
        shapeFormula.Add("x4", "+- r 0 ah");
        shapeFormula.Add("y3", "+- dh2 th 0");
        shapeFormula.Add("y4", "+- y3 dh2 0");
        shapeFormula.Add("y5", "+- dh2 bd 0");
        shapeFormula.Add("y6", "+- y3 bd2 0");
        break;
      case AutoShapeType.UTurnArrow:
        shapeFormula.Add("a2", "pin 0 adj2 25000");
        shapeFormula.Add("maxAdj1", "*/ a2 2 1");
        shapeFormula.Add("a1", "pin 0 adj1 maxAdj1");
        shapeFormula.Add("q2", "*/ a1 ss h");
        shapeFormula.Add("q3", "+- 100000 0 q2");
        shapeFormula.Add("maxAdj3", "*/ q3 h ss");
        shapeFormula.Add("a3", "pin 0 adj3 maxAdj3");
        shapeFormula.Add("q1", "+- a3 a1 0");
        shapeFormula.Add("minAdj5", "*/ q1 ss h");
        shapeFormula.Add("a5", "pin minAdj5 adj5 100000");
        shapeFormula.Add("th", "*/ ss a1 100000");
        shapeFormula.Add("aw2", "*/ ss a2 100000");
        shapeFormula.Add("th2", "*/ th 1 2");
        shapeFormula.Add("dh2", "+- aw2 0 th2");
        shapeFormula.Add("y5", "*/ h a5 100000");
        shapeFormula.Add("ah", "*/ ss a3 100000");
        shapeFormula.Add("y4", "+- y5 0 ah");
        shapeFormula.Add("x9", "+- r 0 dh2");
        shapeFormula.Add("bw", "*/ x9 1 2");
        shapeFormula.Add("bs", "min bw y4");
        shapeFormula.Add("maxAdj4", "*/ bs 100000 ss");
        shapeFormula.Add("a4", "pin 0 adj4 maxAdj4");
        shapeFormula.Add("bd", "*/ ss a4 100000");
        shapeFormula.Add("bd3", "+- bd 0 th");
        shapeFormula.Add("bd2", "max bd3 0");
        shapeFormula.Add("x3", "+- th bd2 0");
        shapeFormula.Add("x8", "+- r 0 aw2");
        shapeFormula.Add("x6", "+- x8 0 aw2");
        shapeFormula.Add("x7", "+- x6 dh2 0");
        shapeFormula.Add("x4", "+- x9 0 bd");
        shapeFormula.Add("x5", "+- x7 0 bd2");
        shapeFormula.Add("cx", "+/ th x7 2");
        break;
      case AutoShapeType.LeftUpArrow:
        shapeFormula.Add("a2", "pin 0 adj2 50000");
        shapeFormula.Add("maxAdj1", "*/ a2 2 1");
        shapeFormula.Add("a1", "pin 0 adj1 maxAdj1");
        shapeFormula.Add("maxAdj3", "+- 100000 0 maxAdj1");
        shapeFormula.Add("a3", "pin 0 adj3 maxAdj3");
        shapeFormula.Add("x1", "*/ ss a3 100000");
        shapeFormula.Add("dx2", "*/ ss a2 50000");
        shapeFormula.Add("x2", "+- r 0 dx2");
        shapeFormula.Add("y2", "+- b 0 dx2");
        shapeFormula.Add("dx4", "*/ ss a2 100000");
        shapeFormula.Add("x4", "+- r 0 dx4");
        shapeFormula.Add("y4", "+- b 0 dx4");
        shapeFormula.Add("dx3", "*/ ss a1 200000");
        shapeFormula.Add("x3", "+- x4 0 dx3");
        shapeFormula.Add("x5", "+- x4 dx3 0");
        shapeFormula.Add("y3", "+- y4 0 dx3");
        shapeFormula.Add("y5", "+- y4 dx3 0");
        shapeFormula.Add("il", "*/ dx3 x1 dx4");
        shapeFormula.Add("cx1", "+/ x1 x5 2");
        shapeFormula.Add("cy1", "+/ x1 y5 2");
        break;
      case AutoShapeType.BentUpArrow:
        shapeFormula.Add("a1", "pin 0 adj1 50000");
        shapeFormula.Add("a2", "pin 0 adj2 50000");
        shapeFormula.Add("a3", "pin 0 adj3 50000");
        shapeFormula.Add("y1", "*/ ss a3 100000");
        shapeFormula.Add("dx1", "*/ ss a2 50000");
        shapeFormula.Add("x1", "+- r 0 dx1");
        shapeFormula.Add("dx3", "*/ ss a2 100000");
        shapeFormula.Add("x3", "+- r 0 dx3");
        shapeFormula.Add("dx2", "*/ ss a1 200000");
        shapeFormula.Add("x2", "+- x3 0 dx2");
        shapeFormula.Add("x4", "+- x3 dx2 0");
        shapeFormula.Add("dy2", "*/ ss a1 100000");
        shapeFormula.Add("y2", "+- b 0 dy2");
        shapeFormula.Add("x0", "*/ x4 1 2");
        shapeFormula.Add("y3", "+/ y2 b 2");
        shapeFormula.Add("y15", "+/ y1 b 2");
        break;
      case AutoShapeType.CurvedRightArrow:
        shapeFormula.Add("maxAdj2", "*/ 50000 h ss");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("a1", "pin 0 adj1 a2");
        shapeFormula.Add("th", "*/ ss a1 100000");
        shapeFormula.Add("aw", "*/ ss a2 100000");
        shapeFormula.Add("q1", "+/ th aw 4");
        shapeFormula.Add("hR", "+- hd2 0 q1");
        shapeFormula.Add("q7", "*/ hR 2 1");
        shapeFormula.Add("q8", "*/ q7 q7 1");
        shapeFormula.Add("q9", "*/ th th 1");
        shapeFormula.Add("q10", "+- q8 0 q9");
        shapeFormula.Add("q11", "sqrt q10");
        shapeFormula.Add("idx", "*/ q11 w q7");
        shapeFormula.Add("maxAdj3", "*/ 100000 idx ss");
        shapeFormula.Add("a3", "pin 0 adj3 maxAdj3");
        shapeFormula.Add("ah", "*/ ss a3 100000");
        shapeFormula.Add("y3", "+- hR th 0");
        shapeFormula.Add("q2", "*/ w w 1");
        shapeFormula.Add("q3", "*/ ah ah 1");
        shapeFormula.Add("q4", "+- q2 0 q3");
        shapeFormula.Add("q5", "sqrt q4");
        shapeFormula.Add("dy", "*/ q5 hR w");
        shapeFormula.Add("y5", "+- hR dy 0");
        shapeFormula.Add("y7", "+- y3 dy 0");
        shapeFormula.Add("q6", "+- aw 0 th");
        shapeFormula.Add("dh", "*/ q6 1 2");
        shapeFormula.Add("y4", "+- y5 0 dh");
        shapeFormula.Add("y8", "+- y7 dh 0");
        shapeFormula.Add("aw2", "*/ aw 1 2");
        shapeFormula.Add("y6", "+- b 0 aw2");
        shapeFormula.Add("x1", "+- r 0 ah");
        shapeFormula.Add("swAng0", "at2 ah dy");
        shapeFormula.Add("swAng", "*/ swAng0 180 " + Math.PI.ToString());
        shapeFormula.Add("stAng", "+- cd2 0 swAng");
        shapeFormula.Add("mswAng", "+- 0 0 swAng");
        shapeFormula.Add("ix", "+- r 0 idx");
        shapeFormula.Add("iy", "+/ hR y3 2");
        shapeFormula.Add("q12", "*/ th 1 2");
        shapeFormula.Add("dang0", "at2 idx q12");
        shapeFormula.Add("dang2", "*/ dang0 180 " + Math.PI.ToString());
        shapeFormula.Add("swAng2", "+- dang2 0 cd4");
        shapeFormula.Add("swAng3", "+- cd4 dang2 0");
        shapeFormula.Add("stAng3", "+- cd2 0 dang2");
        break;
      case AutoShapeType.CurvedLeftArrow:
        shapeFormula.Add("maxAdj2", "*/ 50000 h ss");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("a1", "pin 0 adj1 a2");
        shapeFormula.Add("th", "*/ ss a1 100000");
        shapeFormula.Add("aw", "*/ ss a2 100000");
        shapeFormula.Add("q1", "+/ th aw 4");
        shapeFormula.Add("hR", "+- hd2 0 q1");
        shapeFormula.Add("q7", "*/ hR 2 1");
        shapeFormula.Add("q8", "*/ q7 q7 1");
        shapeFormula.Add("q9", "*/ th th 1");
        shapeFormula.Add("q10", "+- q8 0 q9");
        shapeFormula.Add("q11", "sqrt q10");
        shapeFormula.Add("idx", "*/ q11 w q7");
        shapeFormula.Add("maxAdj3", "*/ 100000 idx ss");
        shapeFormula.Add("a3", "pin 0 adj3 maxAdj3");
        shapeFormula.Add("ah", "*/ ss a3 100000");
        shapeFormula.Add("y3", "+- hR th 0");
        shapeFormula.Add("q2", "*/ w w 1");
        shapeFormula.Add("q3", "*/ ah ah 1");
        shapeFormula.Add("q4", "+- q2 0 q3");
        shapeFormula.Add("q5", "sqrt q4");
        shapeFormula.Add("dy", "*/ q5 hR w");
        shapeFormula.Add("y5", "+- hR dy 0");
        shapeFormula.Add("y7", "+- y3 dy 0");
        shapeFormula.Add("q6", "+- aw 0 th");
        shapeFormula.Add("dh", "*/ q6 1 2");
        shapeFormula.Add("y4", "+- y5 0 dh");
        shapeFormula.Add("y8", "+- y7 dh 0");
        shapeFormula.Add("aw2", "*/ aw 1 2");
        shapeFormula.Add("y6", "+- b 0 aw2");
        shapeFormula.Add("x1", "+- l ah 0");
        shapeFormula.Add("swAng1", "at2 ah dy");
        shapeFormula.Add("swAng", "*/ swAng1 180 " + Math.PI.ToString());
        shapeFormula.Add("mswAng", "+- 0 0 swAng");
        shapeFormula.Add("ix", "+- l idx 0");
        shapeFormula.Add("iy", "+/ hR y3 2");
        shapeFormula.Add("q12", "*/ th 1 2");
        shapeFormula.Add("dang3", "at2 idx q12");
        shapeFormula.Add("dang2", "*/ dang3 180 " + Math.PI.ToString());
        shapeFormula.Add("swAng2", "+- dang2 0 swAng");
        shapeFormula.Add("swAng3", "+- swAng dang2 0");
        shapeFormula.Add("stAng3", "+- 0 0 dang2");
        break;
      case AutoShapeType.CurvedUpArrow:
        shapeFormula.Add("maxAdj2", "*/ 50000 w ss");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("a1", "pin 0 adj1 100000");
        shapeFormula.Add("th", "*/ ss a1 100000");
        shapeFormula.Add("aw", "*/ ss a2 100000");
        shapeFormula.Add("q1", "+/ th aw 4");
        shapeFormula.Add("wR", "+- wd2 0 q1");
        shapeFormula.Add("q7", "*/ wR 2 1");
        shapeFormula.Add("q8", "*/ q7 q7 1");
        shapeFormula.Add("q9", "*/ th th 1");
        shapeFormula.Add("q10", "+- q8 0 q9");
        shapeFormula.Add("q11", "sqrt q10");
        shapeFormula.Add("idy", "*/ q11 h q7");
        shapeFormula.Add("maxAdj3", "*/ 100000 idy ss");
        shapeFormula.Add("a3", "pin 0 adj3 maxAdj3");
        shapeFormula.Add("ah", "*/ ss adj3 100000");
        shapeFormula.Add("x3", "+- wR th 0");
        shapeFormula.Add("q2", "*/ h h 1");
        shapeFormula.Add("q3", "*/ ah ah 1");
        shapeFormula.Add("q4", "+- q2 0 q3");
        shapeFormula.Add("q5", "sqrt q4");
        shapeFormula.Add("dx", "*/ q5 wR h");
        shapeFormula.Add("x5", "+- wR dx 0");
        shapeFormula.Add("x7", "+- x3 dx 0");
        shapeFormula.Add("q6", "+- aw 0 th");
        shapeFormula.Add("dh", "*/ q6 1 2");
        shapeFormula.Add("x4", "+- x5 0 dh");
        shapeFormula.Add("x8", "+- x7 dh 0");
        shapeFormula.Add("aw2", "*/ aw 1 2");
        shapeFormula.Add("x6", "+- r 0 aw2");
        shapeFormula.Add("y1", "+- t ah 0");
        shapeFormula.Add("swAng0", "at2 ah dx");
        shapeFormula.Add("swAng", "*/ swAng0 180 " + Math.PI.ToString());
        shapeFormula.Add("mswAng", "+- 0 0 swAng");
        shapeFormula.Add("iy", "+- t idy 0");
        shapeFormula.Add("ix", "+/ wR x3 2");
        shapeFormula.Add("q12", "*/ th 1 2");
        shapeFormula.Add("dang0", "at2 idy q12");
        shapeFormula.Add("dang2", "*/ dang0 180 " + Math.PI.ToString());
        shapeFormula.Add("swAng2", "+- dang2 0 swAng");
        shapeFormula.Add("mswAng2", "+- 0 0 swAng2");
        shapeFormula.Add("stAng3", "+- cd4 0 swAng");
        shapeFormula.Add("swAng3", "+- swAng dang2 0");
        shapeFormula.Add("stAng2", "+- cd4 0 dang2");
        break;
      case AutoShapeType.CurvedDownArrow:
        shapeFormula.Add("maxAdj2", "*/ 50000 w ss");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("a1", "pin 0 adj1 100000");
        shapeFormula.Add("th", "*/ ss a1 100000");
        shapeFormula.Add("aw", "*/ ss a2 100000");
        shapeFormula.Add("q1", "+/ th aw 4");
        shapeFormula.Add("wR", "+- wd2 0 q1");
        shapeFormula.Add("q7", "*/ wR 2 1");
        shapeFormula.Add("q8", "*/ q7 q7 1");
        shapeFormula.Add("q9", "*/ th th 1");
        shapeFormula.Add("q10", "+- q8 0 q9");
        shapeFormula.Add("q11", "sqrt q10");
        shapeFormula.Add("idy", "*/ q11 h q7");
        shapeFormula.Add("maxAdj3", "*/ 100000 idy ss");
        shapeFormula.Add("a3", "pin 0 adj3 maxAdj3");
        shapeFormula.Add("ah", "*/ ss adj3 100000");
        shapeFormula.Add("x3", "+- wR th 0");
        shapeFormula.Add("q2", "*/ h h 1");
        shapeFormula.Add("q3", "*/ ah ah 1");
        shapeFormula.Add("q4", "+- q2 0 q3");
        shapeFormula.Add("q5", "sqrt q4");
        shapeFormula.Add("dx", "*/ q5 wR h");
        shapeFormula.Add("x5", "+- wR dx 0");
        shapeFormula.Add("x7", "+- x3 dx 0");
        shapeFormula.Add("q6", "+- aw 0 th");
        shapeFormula.Add("dh", "*/ q6 1 2");
        shapeFormula.Add("x4", "+- x5 0 dh");
        shapeFormula.Add("x8", "+- x7 dh 0");
        shapeFormula.Add("aw2", "*/ aw 1 2");
        shapeFormula.Add("x6", "+- r 0 aw2");
        shapeFormula.Add("y1", "+- b 0 ah");
        shapeFormula.Add("swAng0", "at2 ah dx");
        shapeFormula.Add("swAng", "*/ swAng0 180 " + Math.PI.ToString());
        shapeFormula.Add("mswAng", "+- 0 0 swAng");
        shapeFormula.Add("iy", "+- b 0 idy");
        shapeFormula.Add("ix", "+/ wR x3 2");
        shapeFormula.Add("q12", "*/ th 1 2");
        shapeFormula.Add("dang0", "at2 idy q12");
        shapeFormula.Add("dang2", "*/ dang0 180 " + Math.PI.ToString());
        shapeFormula.Add("stAng", "+- 3cd4 swAng 0");
        shapeFormula.Add("stAng2", "+- 3cd4 0 dang2");
        shapeFormula.Add("swAng2", "+- dang2 0 cd4");
        shapeFormula.Add("swAng3", "+- cd4 dang2 0");
        break;
      case AutoShapeType.StripedRightArrow:
        shapeFormula.Add("maxAdj2", "*/ 84375 w ss");
        shapeFormula.Add("a1", "pin 0 adj1 100000");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("x4", "*/ ss 5 32");
        shapeFormula.Add("dx5", "*/ ss a2 100000");
        shapeFormula.Add("x5", "+- r 0 dx5");
        shapeFormula.Add("dy1", "*/ h a1 200000");
        shapeFormula.Add("y1", "+- vc 0 dy1");
        shapeFormula.Add("y2", "+- vc dy1 0");
        shapeFormula.Add("dx6", "*/ dy1 dx5 hd2");
        shapeFormula.Add("x6", "+- r 0 dx6");
        break;
      case AutoShapeType.NotchedRightArrow:
        shapeFormula.Add("maxAdj2", "*/ 100000 w ss");
        shapeFormula.Add("a1", "pin 0 adj1 100000");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("dx2", "*/ ss a2 100000");
        shapeFormula.Add("x2", "+- r 0 dx2");
        shapeFormula.Add("dy1", "*/ h a1 200000");
        shapeFormula.Add("y1", "+- vc 0 dy1");
        shapeFormula.Add("y2", "+- vc dy1 0");
        shapeFormula.Add("x1", "*/ dy1 dx2 hd2");
        shapeFormula.Add("x3", "+- r 0 x1");
        break;
      case AutoShapeType.Pentagon:
        shapeFormula.Add("maxAdj", "*/ 100000 w ss");
        shapeFormula.Add("a", "pin 0 adj maxAdj");
        shapeFormula.Add("dx1", "*/ ss a 100000");
        shapeFormula.Add("x1", "+- r 0 dx1");
        shapeFormula.Add("ir", "+/ x1 r 2");
        shapeFormula.Add("x2", "*/ x1 1 2");
        break;
      case AutoShapeType.Chevron:
        shapeFormula.Add("maxAdj", "*/ 100000 w ss");
        shapeFormula.Add("a", "pin 0 adj maxAdj");
        shapeFormula.Add("x1", "*/ ss a 100000");
        shapeFormula.Add("x2", "+- r 0 x1");
        shapeFormula.Add("x3", "*/ x2 1 2");
        shapeFormula.Add("dx", "+- x2 0 x1");
        shapeFormula.Add("il", "?: dx x1 l");
        shapeFormula.Add("ir", "?: dx x2 r");
        break;
      case AutoShapeType.RightArrowCallout:
        shapeFormula.Add("maxAdj2", "*/ 50000 h ss");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("maxAdj1", "*/ a2 2 1");
        shapeFormula.Add("a1", "pin 0 adj1 maxAdj1");
        shapeFormula.Add("maxAdj3", "*/ 100000 w ss");
        shapeFormula.Add("a3", "pin 0 adj3 maxAdj3");
        shapeFormula.Add("q2", "*/ a3 ss w");
        shapeFormula.Add("maxAdj4", "+- 100000 0 q2");
        shapeFormula.Add("a4", "pin 0 adj4 maxAdj4");
        shapeFormula.Add("dy1", "*/ ss a2 100000");
        shapeFormula.Add("dy2", "*/ ss a1 200000");
        shapeFormula.Add("y1", "+- vc 0 dy1");
        shapeFormula.Add("y2", "+- vc 0 dy2");
        shapeFormula.Add("y3", "+- vc dy2 0");
        shapeFormula.Add("y4", "+- vc dy1 0");
        shapeFormula.Add("dx3", "*/ ss a3 100000");
        shapeFormula.Add("x3", "+- r 0 dx3");
        shapeFormula.Add("x2", "*/ w a4 100000");
        shapeFormula.Add("x1", "*/ x2 1 2");
        break;
      case AutoShapeType.LeftArrowCallout:
        shapeFormula.Add("maxAdj2", "*/ 50000 h ss");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("maxAdj1", "*/ a2 2 1");
        shapeFormula.Add("a1", "pin 0 adj1 maxAdj1");
        shapeFormula.Add("maxAdj3", "*/ 100000 w ss");
        shapeFormula.Add("a3", "pin 0 adj3 maxAdj3");
        shapeFormula.Add("q2", "*/ a3 ss w");
        shapeFormula.Add("maxAdj4", "+- 100000 0 q2");
        shapeFormula.Add("a4", "pin 0 adj4 maxAdj4");
        shapeFormula.Add("dy1", "*/ ss a2 100000");
        shapeFormula.Add("dy2", "*/ ss a1 200000");
        shapeFormula.Add("y1", "+- vc 0 dy1");
        shapeFormula.Add("y2", "+- vc 0 dy2");
        shapeFormula.Add("y3", "+- vc dy2 0");
        shapeFormula.Add("y4", "+- vc dy1 0");
        shapeFormula.Add("x1", "*/ ss a3 100000");
        shapeFormula.Add("dx2", "*/ w a4 100000");
        shapeFormula.Add("x2", "+- r 0 dx2");
        shapeFormula.Add("x3", "+/ x2 r 2");
        break;
      case AutoShapeType.UpArrowCallout:
        shapeFormula.Add("maxAdj2", "*/ 50000 w ss");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("maxAdj1", "*/ a2 2 1");
        shapeFormula.Add("a1", "pin 0 adj1 maxAdj1");
        shapeFormula.Add("maxAdj3", "*/ 100000 h ss");
        shapeFormula.Add("a3", "pin 0 adj3 maxAdj3");
        shapeFormula.Add("q2", "*/ a3 ss h");
        shapeFormula.Add("maxAdj4", "+- 100000 0 q2");
        shapeFormula.Add("a4", "pin 0 adj4 maxAdj4");
        shapeFormula.Add("dx1", "*/ ss a2 100000");
        shapeFormula.Add("dx2", "*/ ss a1 200000");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x2", "+- hc 0 dx2");
        shapeFormula.Add("x3", "+- hc dx2 0");
        shapeFormula.Add("x4", "+- hc dx1 0");
        shapeFormula.Add("y1", "*/ ss a3 100000");
        shapeFormula.Add("dy2", "*/ h a4 100000");
        shapeFormula.Add("y2", "+- b 0 dy2");
        shapeFormula.Add("y3", "+/ y2 b 2");
        break;
      case AutoShapeType.DownArrowCallout:
        shapeFormula.Add("maxAdj2", "*/ 50000 w ss");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("maxAdj1", "*/ a2 2 1");
        shapeFormula.Add("a1", "pin 0 adj1 maxAdj1");
        shapeFormula.Add("maxAdj3", "*/ 100000 h ss");
        shapeFormula.Add("a3", "pin 0 adj3 maxAdj3");
        shapeFormula.Add("q2", "*/ a3 ss h");
        shapeFormula.Add("maxAdj4", "+- 100000 0 q2");
        shapeFormula.Add("a4", "pin 0 adj4 maxAdj4");
        shapeFormula.Add("dx1", "*/ ss a2 100000");
        shapeFormula.Add("dx2", "*/ ss a1 200000");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x2", "+- hc 0 dx2");
        shapeFormula.Add("x3", "+- hc dx2 0");
        shapeFormula.Add("x4", "+- hc dx1 0");
        shapeFormula.Add("dy3", "*/ ss a3 100000");
        shapeFormula.Add("y3", "+- b 0 dy3");
        shapeFormula.Add("y2", "*/ h a4 100000");
        shapeFormula.Add("y1", "*/ y2 1 2");
        break;
      case AutoShapeType.LeftRightArrowCallout:
        shapeFormula.Add("maxAdj2", "*/ 50000 h ss");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("maxAdj1", "*/ a2 2 1");
        shapeFormula.Add("a1", "pin 0 adj1 maxAdj1");
        shapeFormula.Add("maxAdj3", "*/ 50000 w ss");
        shapeFormula.Add("a3", "pin 0 adj3 maxAdj3");
        shapeFormula.Add("q2", "*/ a3 ss wd2");
        shapeFormula.Add("maxAdj4", "+- 100000 0 q2");
        shapeFormula.Add("a4", "pin 0 adj4 maxAdj4");
        shapeFormula.Add("dy1", "*/ ss a2 100000");
        shapeFormula.Add("dy2", "*/ ss a1 200000");
        shapeFormula.Add("y1", "+- vc 0 dy1");
        shapeFormula.Add("y2", "+- vc 0 dy2");
        shapeFormula.Add("y3", "+- vc dy2 0");
        shapeFormula.Add("y4", "+- vc dy1 0");
        shapeFormula.Add("x1", "*/ ss a3 100000");
        shapeFormula.Add("x4", "+- r 0 x1");
        shapeFormula.Add("dx2", "*/ w a4 200000");
        shapeFormula.Add("x2", "+- hc 0 dx2");
        shapeFormula.Add("x3", "+- hc dx2 0");
        break;
      case AutoShapeType.UpDownArrowCallout:
        shapeFormula.Add("maxAdj2", "*/ 50000 w ss");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("maxAdj1", "*/ a2 2 1");
        shapeFormula.Add("a1", "pin 0 adj1 maxAdj1");
        shapeFormula.Add("maxAdj3", "*/ 50000 h ss");
        shapeFormula.Add("a3", "pin 0 adj3 maxAdj3");
        shapeFormula.Add("q2", "*/ a3 ss hd2");
        shapeFormula.Add("maxAdj4", "+- 100000 0 q2");
        shapeFormula.Add("a4", "pin 0 adj4 maxAdj4");
        shapeFormula.Add("dx1", "*/ ss a2 100000");
        shapeFormula.Add("dx2", "*/ ss a1 200000");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x2", "+- hc 0 dx2");
        shapeFormula.Add("x3", "+- hc dx2 0");
        shapeFormula.Add("x4", "+- hc dx1 0");
        shapeFormula.Add("y1", "*/ ss a3 100000");
        shapeFormula.Add("y4", "+- b 0 y1");
        shapeFormula.Add("dy2", "*/ h a4 200000");
        shapeFormula.Add("y2", "+- vc 0 dy2");
        shapeFormula.Add("y3", "+- vc dy2 0");
        break;
      case AutoShapeType.QuadArrowCallout:
        shapeFormula.Add("a2", "pin 0 adj2 50000");
        shapeFormula.Add("maxAdj1", "*/ a2 2 1");
        shapeFormula.Add("a1", "pin 0 adj1 maxAdj1");
        shapeFormula.Add("maxAdj3", "+- 50000 0 a2");
        shapeFormula.Add("a3", "pin 0 adj3 maxAdj3");
        shapeFormula.Add("q2", "*/ a3 2 1");
        shapeFormula.Add("maxAdj4", "+- 100000 0 q2");
        shapeFormula.Add("a4", "pin a1 adj4 maxAdj4");
        shapeFormula.Add("dx2", "*/ ss a2 100000");
        shapeFormula.Add("dx3", "*/ ss a1 200000");
        shapeFormula.Add("ah", "*/ ss a3 100000");
        shapeFormula.Add("dx1", "*/ w a4 200000");
        shapeFormula.Add("dy1", "*/ h a4 200000");
        shapeFormula.Add("x8", "+- r 0 ah");
        shapeFormula.Add("x2", "+- hc 0 dx1");
        shapeFormula.Add("x7", "+- hc dx1 0");
        shapeFormula.Add("x3", "+- hc 0 dx2");
        shapeFormula.Add("x6", "+- hc dx2 0");
        shapeFormula.Add("x4", "+- hc 0 dx3");
        shapeFormula.Add("x5", "+- hc dx3 0");
        shapeFormula.Add("y8", "+- b 0 ah");
        shapeFormula.Add("y2", "+- vc 0 dy1");
        shapeFormula.Add("y7", "+- vc dy1 0");
        shapeFormula.Add("y3", "+- vc 0 dx2");
        shapeFormula.Add("y6", "+- vc dx2 0");
        shapeFormula.Add("y4", "+- vc 0 dx3");
        shapeFormula.Add("y5", "+- vc dx3 0");
        break;
      case AutoShapeType.CircularArrow:
        shapeFormula.Add("a5", "pin 0 adj5 25000");
        shapeFormula.Add("maxAdj1", "*/ a5 2 1");
        shapeFormula.Add("a1", "pin 0 adj1 maxAdj1");
        shapeFormula.Add("enAng", "pin 1 adj3 360");
        shapeFormula.Add("stAng", "pin 0 adj4 360");
        shapeFormula.Add("th", "*/ ss a1 100000");
        shapeFormula.Add("thh", "*/ ss a5 100000");
        shapeFormula.Add("th2", "*/ th 1 2");
        shapeFormula.Add("rw1", "+- wd2 th2 thh");
        shapeFormula.Add("rh1", "+- hd2 th2 thh");
        shapeFormula.Add("rw2", "+- rw1 0 th");
        shapeFormula.Add("rh2", "+- rh1 0 th");
        shapeFormula.Add("rw3", "+- rw2 th2 0");
        shapeFormula.Add("rh3", "+- rh2 th2 0");
        shapeFormula.Add("wtH", "sin rw3 enAng");
        shapeFormula.Add("htH", "cos rh3 enAng");
        shapeFormula.Add("dxH", "cat2 rw3 htH wtH");
        shapeFormula.Add("dyH", "sat2 rh3 htH wtH");
        shapeFormula.Add("xH", "+- hc dxH 0");
        shapeFormula.Add("yH", "+- vc dyH 0");
        shapeFormula.Add("rI", "min rw2 rh2");
        shapeFormula.Add("u1", "*/ dxH dxH 1");
        shapeFormula.Add("u2", "*/ dyH dyH 1");
        shapeFormula.Add("u3", "*/ rI rI 1");
        shapeFormula.Add("u4", "+- u1 0 u3");
        shapeFormula.Add("u5", "+- u2 0 u3");
        shapeFormula.Add("u6", "*/ u4 u5 u1");
        shapeFormula.Add("u7", "*/ u6 1 u2");
        shapeFormula.Add("u8", "+- 1 0 u7");
        shapeFormula.Add("u9", "sqrt u8");
        shapeFormula.Add("u10", "*/ u4 1 dxH");
        shapeFormula.Add("u11", "*/ u10 1 dyH");
        shapeFormula.Add("u12", "+/ 1 u9 u11");
        shapeFormula.Add("u0", "at2 1 u12");
        shapeFormula.Add("u13", "*/ u0 180 " + Math.PI.ToString());
        shapeFormula.Add("u14", "+- u13 360 0");
        shapeFormula.Add("u15", "?: u13 u13 u14");
        shapeFormula.Add("u16", "+- u15 0 enAng");
        shapeFormula.Add("u17", "+- u16 360 0");
        shapeFormula.Add("u18", "?: u16 u16 u17");
        shapeFormula.Add("u19", "+- u18 0 cd2");
        shapeFormula.Add("u20", "+- u18 0 360");
        shapeFormula.Add("u21", "?: u19 u20 u18");
        shapeFormula.Add("maxAng", "abs u21");
        shapeFormula.Add("aAng", "pin 0 adj2 maxAng");
        shapeFormula.Add("ptAng", "+- enAng aAng 0");
        shapeFormula.Add("wtA", "sin rw3 ptAng");
        shapeFormula.Add("htA", "cos rh3 ptAng");
        shapeFormula.Add("dxA", "cat2 rw3 htA wtA");
        shapeFormula.Add("dyA", "sat2 rh3 htA wtA");
        shapeFormula.Add("xA", "+- hc dxA 0");
        shapeFormula.Add("yA", "+- vc dyA 0");
        shapeFormula.Add("wtE", "sin rw1 stAng");
        shapeFormula.Add("htE", "cos rh1 stAng");
        shapeFormula.Add("dxE", "cat2 rw1 htE wtE");
        shapeFormula.Add("dyE", "sat2 rh1 htE wtE");
        shapeFormula.Add("xE", "+- hc dxE 0");
        shapeFormula.Add("yE", "+- vc dyE 0");
        shapeFormula.Add("dxG", "cos thh ptAng");
        shapeFormula.Add("dyG", "sin thh ptAng");
        shapeFormula.Add("xG", "+- xH dxG 0");
        shapeFormula.Add("yG", "+- yH dyG 0");
        shapeFormula.Add("dxB", "cos thh ptAng");
        shapeFormula.Add("dyB", "sin thh ptAng");
        shapeFormula.Add("xB", "+- xH 0 dxB 0");
        shapeFormula.Add("yB", "+- yH 0 dyB 0");
        shapeFormula.Add("sx1", "+- xB 0 hc");
        shapeFormula.Add("sy1", "+- yB 0 vc");
        shapeFormula.Add("sx2", "+- xG 0 hc");
        shapeFormula.Add("sy2", "+- yG 0 vc");
        shapeFormula.Add("rO", "min rw1 rh1");
        shapeFormula.Add("x1O", "*/ sx1 rO rw1");
        shapeFormula.Add("y1O", "*/ sy1 rO rh1");
        shapeFormula.Add("x2O", "*/ sx2 rO rw1");
        shapeFormula.Add("y2O", "*/ sy2 rO rh1");
        shapeFormula.Add("dxO", "+- x2O 0 x1O");
        shapeFormula.Add("dyO", "+- y2O 0 y1O");
        shapeFormula.Add("dO", "mod dxO dyO 0");
        shapeFormula.Add("q1", "*/ x1O y2O 1");
        shapeFormula.Add("q2", "*/ x2O y1O 1");
        shapeFormula.Add("DO", "+- q1 0 q2");
        shapeFormula.Add("q3", "*/ rO rO 1");
        shapeFormula.Add("q4", "*/ dO dO 1");
        shapeFormula.Add("q5", "*/ q3 q4 1");
        shapeFormula.Add("q6", "*/ DO DO 1");
        shapeFormula.Add("q7", "+- q5 0 q6");
        shapeFormula.Add("q8", "max q7 0");
        shapeFormula.Add("sdelO", "sqrt q8");
        shapeFormula.Add("ndyO", "*/ dyO -1 1");
        shapeFormula.Add("sdyO", "?: ndyO -1 1");
        shapeFormula.Add("q9", "*/ sdyO dxO 1");
        shapeFormula.Add("q10", "*/ q9 sdelO 1");
        shapeFormula.Add("q11", "*/ DO dyO 1");
        shapeFormula.Add("dxF1", "+/ q11 q10 q4");
        shapeFormula.Add("q12", "+- q11 0 q10");
        shapeFormula.Add("dxF2", "*/ q12 1 q4");
        shapeFormula.Add("adyO", "abs dyO");
        shapeFormula.Add("q13", "*/ adyO sdelO 1");
        shapeFormula.Add("q14", "*/ DO dxO -1");
        shapeFormula.Add("dyF1", "+/ q14 q13 q4");
        shapeFormula.Add("q15", "+- q14 0 q13");
        shapeFormula.Add("dyF2", "*/ q15 1 q4");
        shapeFormula.Add("q16", "+- x2O 0 dxF1");
        shapeFormula.Add("q17", "+- x2O 0 dxF2");
        shapeFormula.Add("q18", "+- y2O 0 dyF1");
        shapeFormula.Add("q19", "+- y2O 0 dyF2");
        shapeFormula.Add("q20", "mod q16 q18 0");
        shapeFormula.Add("q21", "mod q17 q19 0");
        shapeFormula.Add("q22", "+- q21 0 q20");
        shapeFormula.Add("dxF", "?: q22 dxF1 dxF2");
        shapeFormula.Add("dyF", "?: q22 dyF1 dyF2");
        shapeFormula.Add("sdxF", "*/ dxF rw1 rO");
        shapeFormula.Add("sdyF", "*/ dyF rh1 rO");
        shapeFormula.Add("xF", "+- hc sdxF 0");
        shapeFormula.Add("yF", "+- vc sdyF 0");
        shapeFormula.Add("x1I", "*/ sx1 rI rw2");
        shapeFormula.Add("y1I", "*/ sy1 rI rh2");
        shapeFormula.Add("x2I", "*/ sx2 rI rw2");
        shapeFormula.Add("y2I", "*/ sy2 rI rh2");
        shapeFormula.Add("dxI1", "+- x2I 0 x1I");
        shapeFormula.Add("dyI1", "+- y2I 0 y1I");
        shapeFormula.Add("dI", "mod dxI1 dyI1 0");
        shapeFormula.Add("v1", "*/ x1I y2I 1");
        shapeFormula.Add("v2", "*/ x2I y1I 1");
        shapeFormula.Add("DI", "+- v1 0 v2");
        shapeFormula.Add("v3", "*/ rI rI 1");
        shapeFormula.Add("v4", "*/ dI dI 1");
        shapeFormula.Add("v5", "*/ v3 v4 1");
        shapeFormula.Add("v6", "*/ DI DI 1");
        shapeFormula.Add("v7", "+- v5 0 v6");
        shapeFormula.Add("v8", "max v7 0");
        shapeFormula.Add("sdelI", "sqrt v8");
        shapeFormula.Add("v9", "*/ sdyO dxI1 1");
        shapeFormula.Add("v10", "*/ v9 sdelI 1");
        shapeFormula.Add("v11", "*/ DI dyI1 1");
        shapeFormula.Add("dxC1", "+/ v11 v10 v4");
        shapeFormula.Add("v12", "+- v11 0 v10");
        shapeFormula.Add("dxC2", "*/ v12 1 v4");
        shapeFormula.Add("adyI", "abs dyI1");
        shapeFormula.Add("v13", "*/ adyI sdelI 1");
        shapeFormula.Add("v14", "*/ DI dxI1 -1");
        shapeFormula.Add("dyC1", "+/ v14 v13 v4");
        shapeFormula.Add("v15", "+- v14 0 v13");
        shapeFormula.Add("dyC2", "*/ v15 1 v4");
        shapeFormula.Add("v16", "+- x1I 0 dxC1");
        shapeFormula.Add("v17", "+- x1I 0 dxC2");
        shapeFormula.Add("v18", "+- y1I 0 dyC1");
        shapeFormula.Add("v19", "+- y1I 0 dyC2");
        shapeFormula.Add("v20", "mod v16 v18 0");
        shapeFormula.Add("v21", "mod v17 v19 0");
        shapeFormula.Add("v22", "+- v21 0 v20");
        shapeFormula.Add("dxC", "?: v22 dxC1 dxC2");
        shapeFormula.Add("dyC", "?: v22 dyC1 dyC2");
        shapeFormula.Add("sdxC", "*/ dxC rw2 rI");
        shapeFormula.Add("sdyC", "*/ dyC rh2 rI");
        shapeFormula.Add("xC", "+- hc sdxC 0");
        shapeFormula.Add("yC", "+- vc sdyC 0");
        shapeFormula.Add("ist00", "at2 sdxC sdyC");
        shapeFormula.Add("ist0", "*/ ist00 180 " + Math.PI.ToString());
        shapeFormula.Add("ist1", "+- ist0 360 0");
        shapeFormula.Add("istAng", "?: ist0 ist0 ist1");
        shapeFormula.Add("isw1", "+- stAng 0 istAng");
        shapeFormula.Add("isw2", "+- isw1 0 360");
        shapeFormula.Add("iswAng", "?: isw1 isw2 isw1");
        shapeFormula.Add("p1", "+- xF 0 xC");
        shapeFormula.Add("p2", "+- yF 0 yC");
        shapeFormula.Add("p3", "mod p1 p2 0");
        shapeFormula.Add("p4", "*/ p3 1 2");
        shapeFormula.Add("p5", "+- p4 0 thh");
        shapeFormula.Add("xGp", "?: p5 xF xG");
        shapeFormula.Add("yGp", "?: p5 yF yG");
        shapeFormula.Add("xBp", "?: p5 xC xB");
        shapeFormula.Add("yBp", "?: p5 yC yB");
        shapeFormula.Add("en00", "at2 sdxF sdyF");
        shapeFormula.Add("en0", "*/ en00 180 " + Math.PI.ToString());
        shapeFormula.Add("en1", "+- en0 360 0");
        shapeFormula.Add("en2", "?: en0 en0 en1");
        shapeFormula.Add("sw0", "+- en2 0 stAng");
        shapeFormula.Add("sw1", "+- sw0 360 0");
        shapeFormula.Add("swAng", "?: sw0 sw0 sw1");
        shapeFormula.Add("wtI", "sin rw3 stAng");
        shapeFormula.Add("htI", "cos rh3 stAng");
        shapeFormula.Add("dxI", "cat2 rw3 htI wtI");
        shapeFormula.Add("dyI", "sat2 rh3 htI wtI");
        shapeFormula.Add("xI", "+- hc dxI 0");
        shapeFormula.Add("yI", "+- vc dyI 0");
        shapeFormula.Add("aI", "+- stAng 0 cd4");
        shapeFormula.Add("aA", "+- ptAng cd4 0");
        shapeFormula.Add("aB", "+- ptAng cd2 0");
        shapeFormula.Add("idx", "cos rw1 45");
        shapeFormula.Add("idy", "sin rh1 45");
        shapeFormula.Add("il", "+- hc 0 idx");
        shapeFormula.Add("ir", "+- hc idx 0");
        shapeFormula.Add("it", "+- vc 0 idy");
        shapeFormula.Add("ib", "+- vc idy 0");
        break;
      case AutoShapeType.FlowChartAlternateProcess:
        shapeFormula.Add("x2", "+- r 0 ssd6");
        shapeFormula.Add("y2", "+- b 0 ssd6");
        shapeFormula.Add("il", "*/ ssd6 29289 100000");
        shapeFormula.Add("ir", "+- r 0 il");
        shapeFormula.Add("ib", "+- b 0 il");
        break;
      case AutoShapeType.FlowChartDecision:
        shapeFormula.Add("ir", "*/ w 3 4");
        shapeFormula.Add("ib", "*/ h 3 4");
        break;
      case AutoShapeType.FlowChartData:
        shapeFormula.Add("x3", "*/ w 2 5");
        shapeFormula.Add("x4", "*/ w 3 5");
        shapeFormula.Add("x5", "*/ w 4 5");
        shapeFormula.Add("x6", "*/ w 9 10");
        break;
      case AutoShapeType.FlowChartPredefinedProcess:
        shapeFormula.Add("x2", "*/ w 7 8");
        break;
      case AutoShapeType.FlowChartDocument:
        shapeFormula.Add("y1", "*/ h 17322 21600");
        shapeFormula.Add("y2", "*/ h 20172 21600");
        break;
      case AutoShapeType.FlowChartMultiDocument:
        shapeFormula.Add("y2", "*/ h 3675 21600");
        shapeFormula.Add("y8", "*/ h 20782 21600");
        shapeFormula.Add("x3", "*/ w 9298 21600");
        shapeFormula.Add("x4", "*/ w 12286 21600");
        shapeFormula.Add("x5", "*/ w 18595 21600");
        break;
      case AutoShapeType.FlowChartTerminator:
        shapeFormula.Add("il", "*/ w 1018 21600");
        shapeFormula.Add("ir", "*/ w 20582 21600");
        shapeFormula.Add("it", "*/ h 3163 21600");
        shapeFormula.Add("ib", "*/ h 18437 21600");
        break;
      case AutoShapeType.FlowChartPreparation:
        shapeFormula.Add("x2", "*/ w 4 5");
        break;
      case AutoShapeType.FlowChartManualOperation:
        shapeFormula.Add("x3", "*/ w 4 5");
        shapeFormula.Add("x4", "*/ w 9 10");
        break;
      case AutoShapeType.FlowChartConnector:
        shapeFormula.Add("idx", "cos wd2 45");
        shapeFormula.Add("idy", "sin hd2 45");
        shapeFormula.Add("il", "+- hc 0 idx");
        shapeFormula.Add("ir", "+- hc idx 0");
        shapeFormula.Add("it", "+- vc 0 idy");
        shapeFormula.Add("ib", "+- vc idy 0");
        break;
      case AutoShapeType.FlowChartOffPageConnector:
        shapeFormula.Add("y1", "*/ h 4 5");
        break;
      case AutoShapeType.FlowChartPunchedTape:
        shapeFormula.Add("y2", "*/ h 9 10");
        shapeFormula.Add("ib", "*/ h 4 5");
        break;
      case AutoShapeType.FlowChartSummingJunction:
        shapeFormula.Add("idx", "cos wd2 45");
        shapeFormula.Add("idy", "sin hd2 45");
        shapeFormula.Add("il", "+- hc 0 idx");
        shapeFormula.Add("ir", "+- hc idx 0");
        shapeFormula.Add("it", "+- vc 0 idy");
        shapeFormula.Add("ib", "+- vc idy 0");
        break;
      case AutoShapeType.FlowChartOr:
        shapeFormula.Add("idx", "cos wd2 45");
        shapeFormula.Add("idy", "sin hd2 45");
        shapeFormula.Add("il", "+- hc 0 idx");
        shapeFormula.Add("ir", "+- hc idx 0");
        shapeFormula.Add("it", "+- vc 0 idy");
        shapeFormula.Add("ib", "+- vc idy 0");
        break;
      case AutoShapeType.FlowChartCollate:
        shapeFormula.Add("ir", "*/ w 3 4");
        shapeFormula.Add("ib", "*/ h 3 4");
        break;
      case AutoShapeType.FlowChartSort:
        shapeFormula.Add("ir", "*/ w 3 4");
        shapeFormula.Add("ib", "*/ h 3 4");
        break;
      case AutoShapeType.FlowChartExtract:
        shapeFormula.Add("x2", "*/ w 3 4");
        break;
      case AutoShapeType.FlowChartMerge:
        shapeFormula.Add("x2", "*/ w 3 4");
        break;
      case AutoShapeType.FlowChartStoredData:
        shapeFormula.Add("x2", "*/ w 5 6");
        break;
      case AutoShapeType.FlowChartDelay:
        shapeFormula.Add("idx", "cos wd2 45");
        shapeFormula.Add("idy", "sin hd2 45");
        shapeFormula.Add("ir", "+- hc idx 0");
        shapeFormula.Add("it", "+- vc 0 idy");
        shapeFormula.Add("ib", "+- vc idy 0");
        break;
      case AutoShapeType.FlowChartSequentialAccessStorage:
        shapeFormula.Add("idx", "cos wd2 45");
        shapeFormula.Add("idy", "sin hd2 45");
        shapeFormula.Add("il", "+- hc 0 idx");
        shapeFormula.Add("ir", "+- hc idx 0");
        shapeFormula.Add("it", "+- vc 0 idy");
        shapeFormula.Add("ib", "+- vc idy 0");
        shapeFormula.Add("ang", "at2 w h");
        shapeFormula.Add("ang1", "*/ ang 180 " + Math.PI.ToString());
        break;
      case AutoShapeType.FlowChartMagneticDisk:
        shapeFormula.Add("y3", "*/ h 5 6");
        break;
      case AutoShapeType.FlowChartDirectAccessStorage:
        shapeFormula.Add("x2", "*/ w 2 3");
        break;
      case AutoShapeType.FlowChartDisplay:
        shapeFormula.Add("x2", "*/ w 5 6");
        break;
      case AutoShapeType.Explosion1:
        shapeFormula.Add("x5", "*/ w 4627 21600");
        shapeFormula.Add("x12", "*/ w 8485 21600");
        shapeFormula.Add("x21", "*/ w 16702 21600");
        shapeFormula.Add("x24", "*/ w 14522 21600");
        shapeFormula.Add("y3", "*/ h 6320 21600");
        shapeFormula.Add("y6", "*/ h 8615 21600");
        shapeFormula.Add("y9", "*/ h 13937 21600");
        shapeFormula.Add("y18", "*/ h 13290 21600");
        break;
      case AutoShapeType.Explosion2:
        shapeFormula.Add("x2", "*/ w 9722 21600");
        shapeFormula.Add("x5", "*/ w 5372 21600");
        shapeFormula.Add("x16", "*/ w 11612 21600");
        shapeFormula.Add("x19", "*/ w 14640 21600");
        shapeFormula.Add("y2", "*/ h 1887 21600");
        shapeFormula.Add("y3", "*/ h 6382 21600");
        shapeFormula.Add("y8", "*/ h 12877 21600");
        shapeFormula.Add("y14", "*/ h 19712 21600");
        shapeFormula.Add("y16", "*/ h 18842 21600");
        shapeFormula.Add("y17", "*/ h 15935 21600");
        shapeFormula.Add("y24", "*/ h 6645 21600");
        break;
      case AutoShapeType.Star4Point:
        shapeFormula.Add("a", "pin 0 adj 50000");
        shapeFormula.Add("iwd2", "*/ wd2 a 50000");
        shapeFormula.Add("ihd2", "*/ hd2 a 50000");
        shapeFormula.Add("sdx", "cos iwd2 45");
        shapeFormula.Add("sdy", "sin ihd2 45");
        shapeFormula.Add("sx1", "+- hc 0 sdx");
        shapeFormula.Add("sx2", "+- hc sdx 0");
        shapeFormula.Add("sy1", "+- vc 0 sdy");
        shapeFormula.Add("sy2", "+- vc sdy 0");
        shapeFormula.Add("yAdj", "+- vc 0 ihd2");
        break;
      case AutoShapeType.Star5Point:
        shapeFormula.Add("a", "pin 0 adj 50000");
        shapeFormula.Add("swd2", "*/ wd2 hf 100000");
        shapeFormula.Add("shd2", "*/ hd2 vf 100000");
        shapeFormula.Add("svc", "*/ vc  vf 100000");
        shapeFormula.Add("dx1", "cos swd2 18");
        shapeFormula.Add("dx2", "cos swd2 306");
        shapeFormula.Add("dy1", "sin shd2 18");
        shapeFormula.Add("dy2", "sin shd2 306");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x2", "+- hc 0 dx2");
        shapeFormula.Add("x3", "+- hc dx2 0");
        shapeFormula.Add("x4", "+- hc dx1 0");
        shapeFormula.Add("y1", "+- svc 0 dy1");
        shapeFormula.Add("y2", "+- svc 0 dy2");
        shapeFormula.Add("iwd2", "*/ swd2 a 50000");
        shapeFormula.Add("ihd2", "*/ shd2 a 50000");
        shapeFormula.Add("sdx1", "cos iwd2 342");
        shapeFormula.Add("sdx2", "cos iwd2 54");
        shapeFormula.Add("sdy1", "sin ihd2 54");
        shapeFormula.Add("sdy2", "sin ihd2 342");
        shapeFormula.Add("sx1", "+- hc 0 sdx1");
        shapeFormula.Add("sx2", "+- hc 0 sdx2");
        shapeFormula.Add("sx3", "+- hc sdx2 0");
        shapeFormula.Add("sx4", "+- hc sdx1 0");
        shapeFormula.Add("sy1", "+- svc 0 sdy1");
        shapeFormula.Add("sy2", "+- svc 0 sdy2");
        shapeFormula.Add("sy3", "+- svc ihd2 0");
        shapeFormula.Add("yAdj", "+- svc 0 ihd2");
        break;
      case AutoShapeType.Star8Point:
        shapeFormula.Add("a", "pin 0 adj 50000");
        shapeFormula.Add("dx1", "cos wd2 45");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x2", "+- hc dx1 0");
        shapeFormula.Add("dy1", "sin hd2 45");
        shapeFormula.Add("y1", "+- vc 0 dy1");
        shapeFormula.Add("y2", "+- vc dy1 0");
        shapeFormula.Add("iwd2", "*/ wd2 a 50000");
        shapeFormula.Add("ihd2", "*/ hd2 a 50000");
        shapeFormula.Add("sdx1", "*/ iwd2 92388 100000");
        shapeFormula.Add("sdx2", "*/ iwd2 38268 100000");
        shapeFormula.Add("sdy1", "*/ ihd2 92388 100000");
        shapeFormula.Add("sdy2", "*/ ihd2 38268 100000");
        shapeFormula.Add("sx1", "+- hc 0 sdx1");
        shapeFormula.Add("sx2", "+- hc 0 sdx2");
        shapeFormula.Add("sx3", "+- hc sdx2 0");
        shapeFormula.Add("sx4", "+- hc sdx1 0");
        shapeFormula.Add("sy1", "+- vc 0 sdy1");
        shapeFormula.Add("sy2", "+- vc 0 sdy2");
        shapeFormula.Add("sy3", "+- vc sdy2 0");
        shapeFormula.Add("sy4", "+- vc sdy1 0");
        shapeFormula.Add("yAdj", "+- vc 0 ihd2");
        break;
      case AutoShapeType.Star16Point:
        shapeFormula.Add("a", "pin 0 adj 50000");
        shapeFormula.Add("dx1", "*/ wd2 92388 100000");
        shapeFormula.Add("dx2", "*/ wd2 70711 100000");
        shapeFormula.Add("dx3", "*/ wd2 38268 100000");
        shapeFormula.Add("dy1", "*/ hd2 92388 100000");
        shapeFormula.Add("dy2", "*/ hd2 70711 100000");
        shapeFormula.Add("dy3", "*/ hd2 38268 100000");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x2", "+- hc 0 dx2");
        shapeFormula.Add("x3", "+- hc 0 dx3");
        shapeFormula.Add("x4", "+- hc dx3 0");
        shapeFormula.Add("x5", "+- hc dx2 0");
        shapeFormula.Add("x6", "+- hc dx1 0");
        shapeFormula.Add("y1", "+- vc 0 dy1");
        shapeFormula.Add("y2", "+- vc 0 dy2");
        shapeFormula.Add("y3", "+- vc 0 dy3");
        shapeFormula.Add("y4", "+- vc dy3 0");
        shapeFormula.Add("y5", "+- vc dy2 0");
        shapeFormula.Add("y6", "+- vc dy1 0");
        shapeFormula.Add("iwd2", "*/ wd2 a 50000");
        shapeFormula.Add("ihd2", "*/ hd2 a 50000");
        shapeFormula.Add("sdx1", "*/ iwd2 98079 100000");
        shapeFormula.Add("sdx2", "*/ iwd2 83147 100000");
        shapeFormula.Add("sdx3", "*/ iwd2 55557 100000");
        shapeFormula.Add("sdx4", "*/ iwd2 19509 100000");
        shapeFormula.Add("sdy1", "*/ ihd2 98079 100000");
        shapeFormula.Add("sdy2", "*/ ihd2 83147 100000");
        shapeFormula.Add("sdy3", "*/ ihd2 55557 100000");
        shapeFormula.Add("sdy4", "*/ ihd2 19509 100000");
        shapeFormula.Add("sx1", "+- hc 0 sdx1");
        shapeFormula.Add("sx2", "+- hc 0 sdx2");
        shapeFormula.Add("sx3", "+- hc 0 sdx3");
        shapeFormula.Add("sx4", "+- hc 0 sdx4");
        shapeFormula.Add("sx5", "+- hc sdx4 0");
        shapeFormula.Add("sx6", "+- hc sdx3 0");
        shapeFormula.Add("sx7", "+- hc sdx2 0");
        shapeFormula.Add("sx8", "+- hc sdx1 0");
        shapeFormula.Add("sy1", "+- vc 0 sdy1");
        shapeFormula.Add("sy2", "+- vc 0 sdy2");
        shapeFormula.Add("sy3", "+- vc 0 sdy3");
        shapeFormula.Add("sy4", "+- vc 0 sdy4");
        shapeFormula.Add("sy5", "+- vc sdy4 0");
        shapeFormula.Add("sy6", "+- vc sdy3 0");
        shapeFormula.Add("sy7", "+- vc sdy2 0");
        shapeFormula.Add("sy8", "+- vc sdy1 0");
        shapeFormula.Add("idx", "cos iwd2 45");
        shapeFormula.Add("idy", "sin ihd2 45");
        shapeFormula.Add("il", "+- hc 0 idx");
        shapeFormula.Add("it", "+- vc 0 idy");
        shapeFormula.Add("ir", "+- hc idx 0");
        shapeFormula.Add("ib", "+- vc idy 0");
        shapeFormula.Add("yAdj", "+- vc 0 ihd2");
        break;
      case AutoShapeType.Star24Point:
        shapeFormula.Add("a", "pin 0 adj 50000");
        shapeFormula.Add("dx1", "cos wd2 15");
        shapeFormula.Add("dx2", "cos wd2 30");
        shapeFormula.Add("dx3", "cos wd2 45");
        shapeFormula.Add("dx4", "val wd4");
        shapeFormula.Add("dx5", "cos wd2 75");
        shapeFormula.Add("dy1", "sin hd2 75");
        shapeFormula.Add("dy2", "sin hd2 60");
        shapeFormula.Add("dy3", "sin hd2 45");
        shapeFormula.Add("dy4", "val hd4");
        shapeFormula.Add("dy5", "sin hd2 15");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x2", "+- hc 0 dx2");
        shapeFormula.Add("x3", "+- hc 0 dx3");
        shapeFormula.Add("x4", "+- hc 0 dx4");
        shapeFormula.Add("x5", "+- hc 0 dx5");
        shapeFormula.Add("x6", "+- hc dx5 0");
        shapeFormula.Add("x7", "+- hc dx4 0");
        shapeFormula.Add("x8", "+- hc dx3 0");
        shapeFormula.Add("x9", "+- hc dx2 0");
        shapeFormula.Add("x10", "+- hc dx1 0");
        shapeFormula.Add("y1", "+- vc 0 dy1");
        shapeFormula.Add("y2", "+- vc 0 dy2");
        shapeFormula.Add("y3", "+- vc 0 dy3");
        shapeFormula.Add("y4", "+- vc 0 dy4");
        shapeFormula.Add("y5", "+- vc 0 dy5");
        shapeFormula.Add("y6", "+- vc dy5 0");
        shapeFormula.Add("y7", "+- vc dy4 0");
        shapeFormula.Add("y8", "+- vc dy3 0");
        shapeFormula.Add("y9", "+- vc dy2 0");
        shapeFormula.Add("y10", "+- vc dy1 0");
        shapeFormula.Add("iwd2", "*/ wd2 a 50000");
        shapeFormula.Add("ihd2", "*/ hd2 a 50000");
        shapeFormula.Add("sdx1", "*/ iwd2 99144 100000");
        shapeFormula.Add("sdx2", "*/ iwd2 92388 100000");
        shapeFormula.Add("sdx3", "*/ iwd2 79335 100000");
        shapeFormula.Add("sdx4", "*/ iwd2 60876 100000");
        shapeFormula.Add("sdx5", "*/ iwd2 38268 100000");
        shapeFormula.Add("sdx6", "*/ iwd2 13053 100000");
        shapeFormula.Add("sdy1", "*/ ihd2 99144 100000");
        shapeFormula.Add("sdy2", "*/ ihd2 92388 100000");
        shapeFormula.Add("sdy3", "*/ ihd2 79335 100000");
        shapeFormula.Add("sdy4", "*/ ihd2 60876 100000");
        shapeFormula.Add("sdy5", "*/ ihd2 38268 100000");
        shapeFormula.Add("sdy6", "*/ ihd2 13053 100000");
        shapeFormula.Add("sx1", "+- hc 0 sdx1");
        shapeFormula.Add("sx2", "+- hc 0 sdx2");
        shapeFormula.Add("sx3", "+- hc 0 sdx3");
        shapeFormula.Add("sx4", "+- hc 0 sdx4");
        shapeFormula.Add("sx5", "+- hc 0 sdx5");
        shapeFormula.Add("sx6", "+- hc 0 sdx6");
        shapeFormula.Add("sx7", "+- hc sdx6 0");
        shapeFormula.Add("sx8", "+- hc sdx5 0");
        shapeFormula.Add("sx9", "+- hc sdx4 0");
        shapeFormula.Add("sx10", "+- hc sdx3 0");
        shapeFormula.Add("sx11", "+- hc sdx2 0");
        shapeFormula.Add("sx12", "+- hc sdx1 0");
        shapeFormula.Add("sy1", "+- vc 0 sdy1");
        shapeFormula.Add("sy2", "+- vc 0 sdy2");
        shapeFormula.Add("sy3", "+- vc 0 sdy3");
        shapeFormula.Add("sy4", "+- vc 0 sdy4");
        shapeFormula.Add("sy5", "+- vc 0 sdy5");
        shapeFormula.Add("sy6", "+- vc 0 sdy6");
        shapeFormula.Add("sy7", "+- vc sdy6 0");
        shapeFormula.Add("sy8", "+- vc sdy5 0");
        shapeFormula.Add("sy9", "+- vc sdy4 0");
        shapeFormula.Add("sy10", "+- vc sdy3 0");
        shapeFormula.Add("sy11", "+- vc sdy2 0");
        shapeFormula.Add("sy12", "+- vc sdy1 0");
        shapeFormula.Add("idx", "cos iwd2 45");
        shapeFormula.Add("idy", "sin ihd2 45");
        shapeFormula.Add("il", "+- hc 0 idx");
        shapeFormula.Add("it", "+- vc 0 idy");
        shapeFormula.Add("ir", "+- hc idx 0");
        shapeFormula.Add("ib", "+- vc idy 0");
        shapeFormula.Add("yAdj", "+- vc 0 ihd2");
        break;
      case AutoShapeType.Star32Point:
        shapeFormula.Add("a", "pin 0 adj 50000");
        shapeFormula.Add("dx1", "*/ wd2 98079 100000");
        shapeFormula.Add("dx2", "*/ wd2 92388 100000");
        shapeFormula.Add("dx3", "*/ wd2 83147 100000");
        shapeFormula.Add("dx4", "cos wd2 45");
        shapeFormula.Add("dx5", "*/ wd2 55557 100000");
        shapeFormula.Add("dx6", "*/ wd2 38268 100000");
        shapeFormula.Add("dx7", "*/ wd2 19509 100000");
        shapeFormula.Add("dy1", "*/ hd2 98079 100000");
        shapeFormula.Add("dy2", "*/ hd2 92388 100000");
        shapeFormula.Add("dy3", "*/ hd2 83147 100000");
        shapeFormula.Add("dy4", "sin hd2 45");
        shapeFormula.Add("dy5", "*/ hd2 55557 100000");
        shapeFormula.Add("dy6", "*/ hd2 38268 100000");
        shapeFormula.Add("dy7", "*/ hd2 19509 100000");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x2", "+- hc 0 dx2");
        shapeFormula.Add("x3", "+- hc 0 dx3");
        shapeFormula.Add("x4", "+- hc 0 dx4");
        shapeFormula.Add("x5", "+- hc 0 dx5");
        shapeFormula.Add("x6", "+- hc 0 dx6");
        shapeFormula.Add("x7", "+- hc 0 dx7");
        shapeFormula.Add("x8", "+- hc dx7 0");
        shapeFormula.Add("x9", "+- hc dx6 0");
        shapeFormula.Add("x10", "+- hc dx5 0");
        shapeFormula.Add("x11", "+- hc dx4 0");
        shapeFormula.Add("x12", "+- hc dx3 0");
        shapeFormula.Add("x13", "+- hc dx2 0");
        shapeFormula.Add("x14", "+- hc dx1 0");
        shapeFormula.Add("y1", "+- vc 0 dy1");
        shapeFormula.Add("y2", "+- vc 0 dy2");
        shapeFormula.Add("y3", "+- vc 0 dy3");
        shapeFormula.Add("y4", "+- vc 0 dy4");
        shapeFormula.Add("y5", "+- vc 0 dy5");
        shapeFormula.Add("y6", "+- vc 0 dy6");
        shapeFormula.Add("y7", "+- vc 0 dy7");
        shapeFormula.Add("y8", "+- vc dy7 0");
        shapeFormula.Add("y9", "+- vc dy6 0");
        shapeFormula.Add("y10", "+- vc dy5 0");
        shapeFormula.Add("y11", "+- vc dy4 0");
        shapeFormula.Add("y12", "+- vc dy3 0");
        shapeFormula.Add("y13", "+- vc dy2 0");
        shapeFormula.Add("y14", "+- vc dy1 0");
        shapeFormula.Add("iwd2", "*/ wd2 a 50000");
        shapeFormula.Add("ihd2", "*/ hd2 a 50000");
        shapeFormula.Add("sdx1", "*/ iwd2 99518 100000");
        shapeFormula.Add("sdx2", "*/ iwd2 95694 100000");
        shapeFormula.Add("sdx3", "*/ iwd2 88192 100000");
        shapeFormula.Add("sdx4", "*/ iwd2 77301 100000");
        shapeFormula.Add("sdx5", "*/ iwd2 63439 100000");
        shapeFormula.Add("sdx6", "*/ iwd2 47140 100000");
        shapeFormula.Add("sdx7", "*/ iwd2 29028 100000");
        shapeFormula.Add("sdx8", "*/ iwd2 9802 100000");
        shapeFormula.Add("sdy1", "*/ ihd2 99518 100000");
        shapeFormula.Add("sdy2", "*/ ihd2 95694 100000");
        shapeFormula.Add("sdy3", "*/ ihd2 88192 100000");
        shapeFormula.Add("sdy4", "*/ ihd2 77301 100000");
        shapeFormula.Add("sdy5", "*/ ihd2 63439 100000");
        shapeFormula.Add("sdy6", "*/ ihd2 47140 100000");
        shapeFormula.Add("sdy7", "*/ ihd2 29028 100000");
        shapeFormula.Add("sdy8", "*/ ihd2 9802 100000");
        shapeFormula.Add("sx1", "+- hc 0 sdx1");
        shapeFormula.Add("sx2", "+- hc 0 sdx2");
        shapeFormula.Add("sx3", "+- hc 0 sdx3");
        shapeFormula.Add("sx4", "+- hc 0 sdx4");
        shapeFormula.Add("sx5", "+- hc 0 sdx5");
        shapeFormula.Add("sx6", "+- hc 0 sdx6");
        shapeFormula.Add("sx7", "+- hc 0 sdx7");
        shapeFormula.Add("sx8", "+- hc 0 sdx8");
        shapeFormula.Add("sx9", "+- hc sdx8 0");
        shapeFormula.Add("sx10", "+- hc sdx7 0");
        shapeFormula.Add("sx11", "+- hc sdx6 0");
        shapeFormula.Add("sx12", "+- hc sdx5 0");
        shapeFormula.Add("sx13", "+- hc sdx4 0");
        shapeFormula.Add("sx14", "+- hc sdx3 0");
        shapeFormula.Add("sx15", "+- hc sdx2 0");
        shapeFormula.Add("sx16", "+- hc sdx1 0");
        shapeFormula.Add("sy1", "+- vc 0 sdy1");
        shapeFormula.Add("sy2", "+- vc 0 sdy2");
        shapeFormula.Add("sy3", "+- vc 0 sdy3");
        shapeFormula.Add("sy4", "+- vc 0 sdy4");
        shapeFormula.Add("sy5", "+- vc 0 sdy5");
        shapeFormula.Add("sy6", "+- vc 0 sdy6");
        shapeFormula.Add("sy7", "+- vc 0 sdy7");
        shapeFormula.Add("sy8", "+- vc 0 sdy8");
        shapeFormula.Add("sy9", "+- vc sdy8 0");
        shapeFormula.Add("sy10", "+- vc sdy7 0");
        shapeFormula.Add("sy11", "+- vc sdy6 0");
        shapeFormula.Add("sy12", "+- vc sdy5 0");
        shapeFormula.Add("sy13", "+- vc sdy4 0");
        shapeFormula.Add("sy14", "+- vc sdy3 0");
        shapeFormula.Add("sy15", "+- vc sdy2 0");
        shapeFormula.Add("sy16", "+- vc sdy1 0");
        shapeFormula.Add("idx", "cos iwd2 45");
        shapeFormula.Add("idy", "sin ihd2 45");
        shapeFormula.Add("il", "+- hc 0 idx");
        shapeFormula.Add("it", "+- vc 0 idy");
        shapeFormula.Add("ir", "+- hc idx 0");
        shapeFormula.Add("ib", "+- vc idy 0");
        shapeFormula.Add("yAdj", "+- vc 0 ihd2");
        break;
      case AutoShapeType.UpRibbon:
        shapeFormula.Add("a1", "pin 0 adj1 33333");
        shapeFormula.Add("a2", "pin 25000 adj2 75000");
        shapeFormula.Add("x10", "+- r 0 wd8");
        shapeFormula.Add("dx2", "*/ w a2 200000");
        shapeFormula.Add("x2", "+- hc 0 dx2");
        shapeFormula.Add("x9", "+- hc dx2 0");
        shapeFormula.Add("x3", "+- x2 wd32 0");
        shapeFormula.Add("x8", "+- x9 0 wd32");
        shapeFormula.Add("x5", "+- x2 wd8 0");
        shapeFormula.Add("x6", "+- x9 0 wd8");
        shapeFormula.Add("x4", "+- x5 0 wd32");
        shapeFormula.Add("x7", "+- x6 wd32 0");
        shapeFormula.Add("dy1", "*/ h a1 200000");
        shapeFormula.Add("y1", "+- b 0 dy1");
        shapeFormula.Add("dy2", "*/ h a1 100000");
        shapeFormula.Add("y2", "+- b 0 dy2");
        shapeFormula.Add("y4", "+- t dy2 0");
        shapeFormula.Add("y3", "+/ y4 b 2");
        shapeFormula.Add("hR", "*/ h a1 400000");
        shapeFormula.Add("y6", "+- b 0 hR");
        shapeFormula.Add("y7", "+- y1 0 hR");
        break;
      case AutoShapeType.DownRibbon:
        shapeFormula.Add("a1", "pin 0 adj1 33333");
        shapeFormula.Add("a2", "pin 25000 adj2 75000");
        shapeFormula.Add("x10", "+- r 0 wd8");
        shapeFormula.Add("dx2", "*/ w a2 200000");
        shapeFormula.Add("x2", "+- hc 0 dx2");
        shapeFormula.Add("x9", "+- hc dx2 0");
        shapeFormula.Add("x3", "+- x2 wd32 0");
        shapeFormula.Add("x8", "+- x9 0 wd32");
        shapeFormula.Add("x5", "+- x2 wd8 0");
        shapeFormula.Add("x6", "+- x9 0 wd8");
        shapeFormula.Add("x4", "+- x5 0 wd32");
        shapeFormula.Add("x7", "+- x6 wd32 0");
        shapeFormula.Add("y1", "*/ h a1 200000");
        shapeFormula.Add("y2", "*/ h a1 100000");
        shapeFormula.Add("y4", "+- b 0 y2");
        shapeFormula.Add("y3", "*/ y4 1 2");
        shapeFormula.Add("hR", "*/ h a1 400000");
        shapeFormula.Add("y5", "+- b 0 hR");
        shapeFormula.Add("y6", "+- y2 0 hR");
        break;
      case AutoShapeType.CurvedUpRibbon:
        shapeFormula.Add("a1", "pin 0 adj1 100000");
        shapeFormula.Add("a2", "pin 25000 adj2 75000");
        shapeFormula.Add("q10", "+- 100000 0 a1");
        shapeFormula.Add("q11", "*/ q10 1 2");
        shapeFormula.Add("q12", "+- a1 0 q11");
        shapeFormula.Add("minAdj3", "max 0 q12");
        shapeFormula.Add("a3", "pin minAdj3 adj3 a1");
        shapeFormula.Add("dx2", "*/ w a2 200000");
        shapeFormula.Add("x2", "+- hc 0 dx2");
        shapeFormula.Add("x3", "+- x2 wd8 0");
        shapeFormula.Add("x4", "+- r 0 x3");
        shapeFormula.Add("x5", "+- r 0 x2");
        shapeFormula.Add("x6", "+- r 0 wd8");
        shapeFormula.Add("dy1", "*/ h a3 100000");
        shapeFormula.Add("f1", "*/ 4 dy1 w");
        shapeFormula.Add("q111", "*/ x3 x3 w");
        shapeFormula.Add("q2", "+- x3 0 q111");
        shapeFormula.Add("u1", "*/ f1 q2 1");
        shapeFormula.Add("y1", "+- b 0 u1");
        shapeFormula.Add("cx1", "*/ x3 1 2");
        shapeFormula.Add("cu1", "*/ f1 cx1 1");
        shapeFormula.Add("cy1", "+- b 0 cu1");
        shapeFormula.Add("cx2", "+- r 0 cx1");
        shapeFormula.Add("q1", "*/ h a1 100000");
        shapeFormula.Add("dy3", "+- q1 0 dy1");
        shapeFormula.Add("q3", "*/ x2 x2 w");
        shapeFormula.Add("q4", "+- x2 0 q3");
        shapeFormula.Add("q5", "*/ f1 q4 1");
        shapeFormula.Add("u3", "+- q5 dy3 0");
        shapeFormula.Add("y3", "+- b 0 u3");
        shapeFormula.Add("q6", "+- dy1 dy3 u3");
        shapeFormula.Add("q7", "+- q6 dy1 0");
        shapeFormula.Add("cu3", "+- q7 dy3 0");
        shapeFormula.Add("cy3", "+- b 0 cu3");
        shapeFormula.Add("rh", "+- b 0 q1");
        shapeFormula.Add("q8", "*/ dy1 14 16");
        shapeFormula.Add("u2", "+/ q8 rh 2");
        shapeFormula.Add("y2", "+- b 0 u2");
        shapeFormula.Add("u5", "+- q5 rh 0");
        shapeFormula.Add("y5", "+- b 0 u5");
        shapeFormula.Add("u6", "+- u3 rh 0");
        shapeFormula.Add("y6", "+- b 0 u6");
        shapeFormula.Add("cx4", "*/ x2 1 2");
        shapeFormula.Add("q9", "*/ f1 cx4 1");
        shapeFormula.Add("cu4", "+- q9 rh 0");
        shapeFormula.Add("cy4", "+- b 0 cu4");
        shapeFormula.Add("cx5", "+- r 0 cx4");
        shapeFormula.Add("cu6", "+- cu3 rh 0");
        shapeFormula.Add("cy6", "+- b 0 cu6");
        shapeFormula.Add("u7", "+- u1 dy3 0");
        shapeFormula.Add("y7", "+- b 0 u7");
        shapeFormula.Add("cu7", "+- q1 q1 u7");
        shapeFormula.Add("cy7", "+- b 0 cu7");
        break;
      case AutoShapeType.CurvedDownRibbon:
        shapeFormula.Add("a1", "pin 0 adj1 100000");
        shapeFormula.Add("a2", "pin 25000 adj2 75000");
        shapeFormula.Add("q10", "+- 100000 0 a1");
        shapeFormula.Add("q11", "*/ q10 1 2");
        shapeFormula.Add("q12", "+- a1 0 q11");
        shapeFormula.Add("minAdj3", "max 0 q12");
        shapeFormula.Add("a3", "pin minAdj3 adj3 a1");
        shapeFormula.Add("dx2", "*/ w a2 200000");
        shapeFormula.Add("x2", "+- hc 0 dx2");
        shapeFormula.Add("x3", "+- x2 wd8 0");
        shapeFormula.Add("x4", "+- r 0 x3");
        shapeFormula.Add("x5", "+- r 0 x2");
        shapeFormula.Add("x6", "+- r 0 wd8");
        shapeFormula.Add("dy1", "*/ h a3 100000");
        shapeFormula.Add("f1", "*/ 4 dy1 w");
        shapeFormula.Add("q111", "*/ x3 x3 w");
        shapeFormula.Add("q2", "+- x3 0 q111");
        shapeFormula.Add("y1", "*/ f1 q2 1");
        shapeFormula.Add("cx1", "*/ x3 1 2");
        shapeFormula.Add("cy1", "*/ f1 cx1 1");
        shapeFormula.Add("cx2", "+- r 0 cx1");
        shapeFormula.Add("q1", "*/ h a1 100000");
        shapeFormula.Add("dy3", "+- q1 0 dy1");
        shapeFormula.Add("q3", "*/ x2 x2 w");
        shapeFormula.Add("q4", "+- x2 0 q3");
        shapeFormula.Add("q5", "*/ f1 q4 1");
        shapeFormula.Add("y3", "+- q5 dy3 0");
        shapeFormula.Add("q6", "+- dy1 dy3 y3");
        shapeFormula.Add("q7", "+- q6 dy1 0");
        shapeFormula.Add("cy3", "+- q7 dy3 0");
        shapeFormula.Add("rh", "+- b 0 q1");
        shapeFormula.Add("q8", "*/ dy1 14 16");
        shapeFormula.Add("y2", "+/ q8 rh 2");
        shapeFormula.Add("y5", "+- q5 rh 0");
        shapeFormula.Add("y6", "+- y3 rh 0");
        shapeFormula.Add("cx4", "*/ x2 1 2");
        shapeFormula.Add("q9", "*/ f1 cx4 1");
        shapeFormula.Add("cy4", "+- q9 rh 0");
        shapeFormula.Add("cx5", "+- r 0 cx4");
        shapeFormula.Add("cy6", "+- cy3 rh 0");
        shapeFormula.Add("y7", "+- y1 dy3 0");
        shapeFormula.Add("cy7", "+- q1 q1 y7");
        shapeFormula.Add("y8", "+- b 0 dy1");
        break;
      case AutoShapeType.VerticalScroll:
        shapeFormula.Add("a", "pin 0 adj 25000");
        shapeFormula.Add("ch", "*/ ss a 100000");
        shapeFormula.Add("ch2", "*/ ch 1 2");
        shapeFormula.Add("ch4", "*/ ch 1 4");
        shapeFormula.Add("x3", "+- ch ch2 0");
        shapeFormula.Add("x4", "+- ch ch 0");
        shapeFormula.Add("x6", "+- r 0 ch");
        shapeFormula.Add("x7", "+- r 0 ch2");
        shapeFormula.Add("x5", "+- x6 0 ch2");
        shapeFormula.Add("y3", "+- b 0 ch");
        shapeFormula.Add("y4", "+- b 0 ch2");
        break;
      case AutoShapeType.HorizontalScroll:
        shapeFormula.Add("a", "pin 0 adj 25000");
        shapeFormula.Add("ch", "*/ ss a 100000");
        shapeFormula.Add("ch2", "*/ ch 1 2");
        shapeFormula.Add("ch4", "*/ ch 1 4");
        shapeFormula.Add("y3", "+- ch ch2 0");
        shapeFormula.Add("y4", "+- ch ch 0");
        shapeFormula.Add("y6", "+- b 0 ch");
        shapeFormula.Add("y7", "+- b 0 ch2");
        shapeFormula.Add("y5", "+- y6 0 ch2");
        shapeFormula.Add("x3", "+- r 0 ch");
        shapeFormula.Add("x4", "+- r 0 ch2");
        break;
      case AutoShapeType.Wave:
        shapeFormula.Add("a1", "pin 0 adj1 20000");
        shapeFormula.Add("a2", "pin -10000 adj2 10000");
        shapeFormula.Add("y1", "*/ h a1 100000");
        shapeFormula.Add("dy2", "*/ y1 10 3");
        shapeFormula.Add("y2", "+- y1 0 dy2");
        shapeFormula.Add("y3", "+- y1 dy2 0");
        shapeFormula.Add("y4", "+- b 0 y1");
        shapeFormula.Add("y5", "+- y4 0 dy2");
        shapeFormula.Add("y6", "+- y4 dy2 0");
        shapeFormula.Add("dx1", "*/ w a2 100000");
        shapeFormula.Add("of2", "*/ w a2 50000");
        shapeFormula.Add("x1", "abs dx1");
        shapeFormula.Add("dx2", "?: of2 0 of2");
        shapeFormula.Add("x2", "+- l 0 dx2");
        shapeFormula.Add("dx5", "?: of2 of2 0");
        shapeFormula.Add("x5", "+- r 0 dx5");
        shapeFormula.Add("dx3", "+/ dx2 x5 3");
        shapeFormula.Add("x3", "+- x2 dx3 0");
        shapeFormula.Add("x4", "+/ x3 x5 2");
        shapeFormula.Add("x6", "+- l dx5 0");
        shapeFormula.Add("x10", "+- r dx2 0");
        shapeFormula.Add("x7", "+- x6 dx3 0");
        shapeFormula.Add("x8", "+/ x7 x10 2");
        shapeFormula.Add("x9", "+- r 0 x1");
        shapeFormula.Add("xAdj", "+- hc dx1 0");
        shapeFormula.Add("xAdj2", "+- hc 0 dx1");
        shapeFormula.Add("il", "max x2 x6");
        shapeFormula.Add("ir", "min x5 x10");
        shapeFormula.Add("it", "*/ h a1 50000");
        shapeFormula.Add("ib", "+- b 0 it");
        break;
      case AutoShapeType.DoubleWave:
        shapeFormula.Add("a1", "pin 0 adj1 12500");
        shapeFormula.Add("a2", "pin -10000 adj2 10000");
        shapeFormula.Add("y1", "*/ h a1 100000");
        shapeFormula.Add("dy2", "*/ y1 10 3");
        shapeFormula.Add("y2", "+- y1 0 dy2");
        shapeFormula.Add("y3", "+- y1 dy2 0");
        shapeFormula.Add("y4", "+- b 0 y1");
        shapeFormula.Add("y5", "+- y4 0 dy2");
        shapeFormula.Add("y6", "+- y4 dy2 0");
        shapeFormula.Add("dx1", "*/ w a2 100000");
        shapeFormula.Add("of2", "*/ w a2 50000");
        shapeFormula.Add("x1", "abs dx1");
        shapeFormula.Add("dx2", "?: of2 0 of2");
        shapeFormula.Add("x2", "+- l 0 dx2");
        shapeFormula.Add("dx8", "?: of2 of2 0");
        shapeFormula.Add("x8", "+- r 0 dx8");
        shapeFormula.Add("dx3", "+/ dx2 x8 6");
        shapeFormula.Add("x3", "+- x2 dx3 0");
        shapeFormula.Add("dx4", "+/ dx2 x8 3");
        shapeFormula.Add("x4", "+- x2 dx4 0");
        shapeFormula.Add("x5", "+/ x2 x8 2");
        shapeFormula.Add("x6", "+- x5 dx3 0");
        shapeFormula.Add("x7", "+/ x6 x8 2");
        shapeFormula.Add("x9", "+- l dx8 0");
        shapeFormula.Add("x15", "+- r dx2 0");
        shapeFormula.Add("x10", "+- x9 dx3 0");
        shapeFormula.Add("x11", "+- x9 dx4 0");
        shapeFormula.Add("x12", "+/ x9 x15 2");
        shapeFormula.Add("x13", "+- x12 dx3 0");
        shapeFormula.Add("x14", "+/ x13 x15 2");
        shapeFormula.Add("x16", "+- r 0 x1");
        shapeFormula.Add("xAdj", "+- hc dx1 0");
        shapeFormula.Add("il", "max x2 x9");
        shapeFormula.Add("ir", "min x8 x15");
        shapeFormula.Add("it", "*/ h a1 50000");
        shapeFormula.Add("ib", "+- b 0 it");
        break;
      case AutoShapeType.RectangularCallout:
        shapeFormula.Add("dxPos", "*/ w adj1 100000");
        shapeFormula.Add("dyPos", "*/ h adj2 100000");
        shapeFormula.Add("xPos", "+- hc dxPos 0");
        shapeFormula.Add("yPos", "+- vc dyPos 0");
        shapeFormula.Add("dx", "+- xPos 0 hc");
        shapeFormula.Add("dy", "+- yPos 0 vc");
        shapeFormula.Add("dq", "*/ dxPos h w");
        shapeFormula.Add("ady", "abs dyPos");
        shapeFormula.Add("adq", "abs dq");
        shapeFormula.Add("dz", "+- ady 0 adq");
        shapeFormula.Add("xg1", "?: dxPos 7 2");
        shapeFormula.Add("xg2", "?: dxPos 10 5");
        shapeFormula.Add("x1", "*/ w xg1 12");
        shapeFormula.Add("x2", "*/ w xg2 12");
        shapeFormula.Add("yg1", "?: dyPos 7 2");
        shapeFormula.Add("yg2", "?: dyPos 10 5");
        shapeFormula.Add("y1", "*/ h yg1 12");
        shapeFormula.Add("y2", "*/ h yg2 12");
        shapeFormula.Add("t1", "?: dxPos l xPos");
        shapeFormula.Add("xl", "?: dz l t1");
        shapeFormula.Add("t2", "?: dyPos x1 xPos");
        shapeFormula.Add("xt", "?: dz t2 x1");
        shapeFormula.Add("t3", "?: dxPos xPos r");
        shapeFormula.Add("xr", "?: dz r t3");
        shapeFormula.Add("t4", "?: dyPos xPos x1");
        shapeFormula.Add("xb", "?: dz t4 x1");
        shapeFormula.Add("t5", "?: dxPos y1 yPos");
        shapeFormula.Add("yl", "?: dz y1 t5");
        shapeFormula.Add("t6", "?: dyPos t yPos");
        shapeFormula.Add("yt", "?: dz t6 t");
        shapeFormula.Add("t7", "?: dxPos yPos y1");
        shapeFormula.Add("yr", "?: dz y1 t7");
        shapeFormula.Add("t8", "?: dyPos yPos b");
        shapeFormula.Add("yb", "?: dz t8 b");
        break;
      case AutoShapeType.RoundedRectangularCallout:
        shapeFormula.Add("dxPos", "*/ w adj1 100000");
        shapeFormula.Add("dyPos", "*/ h adj2 100000");
        shapeFormula.Add("xPos", "+- hc dxPos 0");
        shapeFormula.Add("yPos", "+- vc dyPos 0");
        shapeFormula.Add("dq", "*/ dxPos h w");
        shapeFormula.Add("ady", "abs dyPos");
        shapeFormula.Add("adq", "abs dq");
        shapeFormula.Add("dz", "+- ady 0 adq");
        shapeFormula.Add("xg1", "?: dxPos 7 2");
        shapeFormula.Add("xg2", "?: dxPos 10 5");
        shapeFormula.Add("x1", "*/ w xg1 12");
        shapeFormula.Add("x2", "*/ w xg2 12");
        shapeFormula.Add("yg1", "?: dyPos 7 2");
        shapeFormula.Add("yg2", "?: dyPos 10 5");
        shapeFormula.Add("y1", "*/ h yg1 12");
        shapeFormula.Add("y2", "*/ h yg2 12");
        shapeFormula.Add("t1", "?: dxPos l xPos");
        shapeFormula.Add("xl", "?: dz l t1");
        shapeFormula.Add("t2", "?: dyPos x1 xPos");
        shapeFormula.Add("xt", "?: dz t2 x1");
        shapeFormula.Add("t3", "?: dxPos xPos r");
        shapeFormula.Add("xr", "?: dz r t3");
        shapeFormula.Add("t4", "?: dyPos xPos x1");
        shapeFormula.Add("xb", "?: dz t4 x1");
        shapeFormula.Add("t5", "?: dxPos y1 yPos");
        shapeFormula.Add("yl", "?: dz y1 t5");
        shapeFormula.Add("t6", "?: dyPos t yPos");
        shapeFormula.Add("yt", "?: dz t6 t");
        shapeFormula.Add("t7", "?: dxPos yPos y1");
        shapeFormula.Add("yr", "?: dz y1 t7");
        shapeFormula.Add("t8", "?: dyPos yPos b");
        shapeFormula.Add("yb", "?: dz t8 b");
        shapeFormula.Add("u1", "*/ ss adj3 100000");
        shapeFormula.Add("u2", "+- r 0 u1");
        shapeFormula.Add("v2", "+- b 0 u1");
        shapeFormula.Add("il", "*/ u1 29289 100000");
        shapeFormula.Add("ir", "+- r 0 il");
        shapeFormula.Add("ib", "+- b 0 il");
        break;
      case AutoShapeType.OvalCallout:
        shapeFormula.Add("dxPos", "*/ w adj1 100000");
        shapeFormula.Add("dyPos", "*/ h adj2 100000");
        shapeFormula.Add("xPos", "+- hc dxPos 0");
        shapeFormula.Add("yPos", "+- vc dyPos 0");
        shapeFormula.Add("sdx", "*/ dxPos h 1");
        shapeFormula.Add("sdy", "*/ dyPos w 1");
        shapeFormula.Add("pang1", "at2 sdx sdy");
        shapeFormula.Add("pang", "*/ pang1 180 " + Math.PI.ToString());
        shapeFormula.Add("stAng", "+- pang 11 0");
        shapeFormula.Add("enAng", "+- pang 0 11");
        shapeFormula.Add("dx1", "cos wd2 stAng");
        shapeFormula.Add("dy1", "sin hd2 stAng");
        shapeFormula.Add("x1", "+- hc dx1 0");
        shapeFormula.Add("y1", "+- vc dy1 0");
        shapeFormula.Add("dx2", "cos wd2 enAng");
        shapeFormula.Add("dy2", "sin hd2 enAng");
        shapeFormula.Add("x2", "+- hc dx2 0");
        shapeFormula.Add("y2", "+- vc dy2 0");
        shapeFormula.Add("stAng2", "at2 dx1 dy1");
        shapeFormula.Add("stAng1", "*/ stAng2 180 " + Math.PI.ToString());
        shapeFormula.Add("enAng2", "at2 dx2 dy2");
        shapeFormula.Add("enAng1", "*/ enAng2 180 " + Math.PI.ToString());
        shapeFormula.Add("swAng1", "+- enAng1 0 stAng1");
        shapeFormula.Add("swAng2", "+- swAng1 360 0");
        shapeFormula.Add("swAng", "?: swAng1 swAng1 swAng2");
        shapeFormula.Add("idx", "cos wd2 45");
        shapeFormula.Add("idy", "sin hd2 45");
        shapeFormula.Add("il", "+- hc 0 idx");
        shapeFormula.Add("ir", "+- hc idx 0");
        shapeFormula.Add("it", "+- vc 0 idy");
        shapeFormula.Add("ib", "+- vc idy 0");
        break;
      case AutoShapeType.CloudCallout:
        shapeFormula.Add("dxPos", "*/ w adj1 100000");
        shapeFormula.Add("dyPos", "*/ h adj2 100000");
        shapeFormula.Add("xPos", "+- hc dxPos 0");
        shapeFormula.Add("yPos", "+- vc dyPos 0");
        shapeFormula.Add("ht", "cat2 hd2 dxPos dyPos");
        shapeFormula.Add("wt", "sat2 wd2 dxPos dyPos");
        shapeFormula.Add("g2", "cat2 wd2 ht wt");
        shapeFormula.Add("g3", "sat2 hd2 ht wt");
        shapeFormula.Add("g4", "+- hc g2 0");
        shapeFormula.Add("g5", "+- vc g3 0");
        shapeFormula.Add("g6", "+- g4 0 xPos");
        shapeFormula.Add("g7", "+- g5 0 yPos");
        shapeFormula.Add("g8", "mod g6 g7 0");
        shapeFormula.Add("g9", "*/ ss 6600 21600");
        shapeFormula.Add("g10", "+- g8 0 g9");
        shapeFormula.Add("g11", "*/ g10 1 3");
        shapeFormula.Add("g12", "*/ ss 1800 21600");
        shapeFormula.Add("g13", "+- g11 g12 0");
        shapeFormula.Add("g14", "*/ g13 g6 g8");
        shapeFormula.Add("g15", "*/ g13 g7 g8");
        shapeFormula.Add("g16", "+- g14 xPos 0");
        shapeFormula.Add("g17", "+- g15 yPos 0");
        shapeFormula.Add("g18", "*/ ss 4800 21600");
        shapeFormula.Add("g19", "*/ g11 2 1");
        shapeFormula.Add("g20", "+- g18 g19 0");
        shapeFormula.Add("g21", "*/ g20 g6 g8");
        shapeFormula.Add("g22", "*/ g20 g7 g8");
        shapeFormula.Add("g23", "+- g21 xPos 0");
        shapeFormula.Add("g24", "+- g22 yPos 0");
        shapeFormula.Add("g25", "*/ ss 1200 21600");
        shapeFormula.Add("g26", "*/ ss 600 21600");
        shapeFormula.Add("x23", "+- xPos g26 0");
        shapeFormula.Add("x24", "+- g16 g25 0");
        shapeFormula.Add("x25", "+- g23 g12 0");
        shapeFormula.Add("il", "*/ w 2977 21600");
        shapeFormula.Add("it", "*/ h 3262 21600");
        shapeFormula.Add("ir", "*/ w 17087 21600");
        shapeFormula.Add("ib", "*/ h 17337 21600");
        shapeFormula.Add("g27", "*/ w 67 21600");
        shapeFormula.Add("g28", "*/ h 21577 21600");
        shapeFormula.Add("g29", "*/ w 21582 21600");
        shapeFormula.Add("g30", "*/ h 1235 21600");
        shapeFormula.Add("pang", "at2 dxPos dyPos");
        break;
      case AutoShapeType.LineCallout1:
        shapeFormula.Add("y1", "*/ h adj1 100000");
        shapeFormula.Add("x1", "*/ w adj2 100000");
        shapeFormula.Add("y2", "*/ h adj3 100000");
        shapeFormula.Add("x2", "*/ w adj4 100000");
        break;
      case AutoShapeType.LineCallout2:
        shapeFormula.Add("y1", "*/ h adj1 100000");
        shapeFormula.Add("x1", "*/ w adj2 100000");
        shapeFormula.Add("y2", "*/ h adj3 100000");
        shapeFormula.Add("x2", "*/ w adj4 100000");
        shapeFormula.Add("y3", "*/ h adj5 100000");
        shapeFormula.Add("x3", "*/ w adj6 100000");
        break;
      case AutoShapeType.LineCallout3:
        shapeFormula.Add("y1", "*/ h adj1 100000");
        shapeFormula.Add("x1", "*/ w adj2 100000");
        shapeFormula.Add("y2", "*/ h adj3 100000");
        shapeFormula.Add("x2", "*/ w adj4 100000");
        shapeFormula.Add("y3", "*/ h adj5 100000");
        shapeFormula.Add("x3", "*/ w adj6 100000");
        shapeFormula.Add("y4", "*/ h adj7 100000");
        shapeFormula.Add("x4", "*/ w adj8 100000");
        break;
      case AutoShapeType.LineCallout1NoBorder:
        shapeFormula.Add("y1", "*/ h adj1 100000");
        shapeFormula.Add("x1", "*/ w adj2 100000");
        shapeFormula.Add("y2", "*/ h adj3 100000");
        shapeFormula.Add("x2", "*/ w adj4 100000");
        break;
      case AutoShapeType.LineCallout1AccentBar:
        shapeFormula.Add("y1", "*/ h adj1 100000");
        shapeFormula.Add("x1", "*/ w adj2 100000");
        shapeFormula.Add("y2", "*/ h adj3 100000");
        shapeFormula.Add("x2", "*/ w adj4 100000");
        break;
      case AutoShapeType.LineCallout2AccentBar:
        shapeFormula.Add("y1", "*/ h adj1 100000");
        shapeFormula.Add("x1", "*/ w adj2 100000");
        shapeFormula.Add("y2", "*/ h adj3 100000");
        shapeFormula.Add("x2", "*/ w adj4 100000");
        shapeFormula.Add("y3", "*/ h adj5 100000");
        shapeFormula.Add("x3", "*/ w adj6 100000");
        break;
      case AutoShapeType.LineCallout3AccentBar:
        shapeFormula.Add("y1", "*/ h adj1 100000");
        shapeFormula.Add("x1", "*/ w adj2 100000");
        shapeFormula.Add("y2", "*/ h adj3 100000");
        shapeFormula.Add("x2", "*/ w adj4 100000");
        shapeFormula.Add("y3", "*/ h adj5 100000");
        shapeFormula.Add("x3", "*/ w adj6 100000");
        shapeFormula.Add("y4", "*/ h adj7 100000");
        shapeFormula.Add("x4", "*/ w adj8 100000");
        break;
      case AutoShapeType.LineCallout2NoBorder:
        shapeFormula.Add("y1", "*/ h adj1 100000");
        shapeFormula.Add("x1", "*/ w adj2 100000");
        shapeFormula.Add("y2", "*/ h adj3 100000");
        shapeFormula.Add("x2", "*/ w adj4 100000");
        shapeFormula.Add("y3", "*/ h adj5 100000");
        shapeFormula.Add("x3", "*/ w adj6 100000");
        break;
      case AutoShapeType.LineCallout3NoBorder:
        shapeFormula.Add("y1", "*/ h adj1 100000");
        shapeFormula.Add("x1", "*/ w adj2 100000");
        shapeFormula.Add("y2", "*/ h adj3 100000");
        shapeFormula.Add("x2", "*/ w adj4 100000");
        shapeFormula.Add("y3", "*/ h adj5 100000");
        shapeFormula.Add("x3", "*/ w adj6 100000");
        shapeFormula.Add("y4", "*/ h adj7 100000");
        shapeFormula.Add("x4", "*/ w adj8 100000");
        break;
      case AutoShapeType.LineCallout1BorderAndAccentBar:
        shapeFormula.Add("y1", "*/ h adj1 100000");
        shapeFormula.Add("x1", "*/ w adj2 100000");
        shapeFormula.Add("y2", "*/ h adj3 100000");
        shapeFormula.Add("x2", "*/ w adj4 100000");
        break;
      case AutoShapeType.LineCallout2BorderAndAccentBar:
        shapeFormula.Add("y1", "*/ h adj1 100000");
        shapeFormula.Add("x1", "*/ w adj2 100000");
        shapeFormula.Add("y2", "*/ h adj3 100000");
        shapeFormula.Add("x2", "*/ w adj4 100000");
        shapeFormula.Add("y3", "*/ h adj5 100000");
        shapeFormula.Add("x3", "*/ w adj6 100000");
        break;
      case AutoShapeType.LineCallout3BorderAndAccentBar:
        shapeFormula.Add("y1", "*/ h adj1 100000");
        shapeFormula.Add("x1", "*/ w adj2 100000");
        shapeFormula.Add("y2", "*/ h adj3 100000");
        shapeFormula.Add("x2", "*/ w adj4 100000");
        shapeFormula.Add("y3", "*/ h adj5 100000");
        shapeFormula.Add("x3", "*/ w adj6 100000");
        shapeFormula.Add("y4", "*/ h adj7 100000");
        shapeFormula.Add("x4", "*/ w adj8 100000");
        break;
      case AutoShapeType.LeftRightRibbon:
        shapeFormula.Add("a3", "pin 0 adj3 33333");
        shapeFormula.Add("maxAdj1", "+- 100000 0 a3");
        shapeFormula.Add("a1", "pin 0 adj1 maxAdj1");
        shapeFormula.Add("w1", "+- wd2 0 wd32");
        shapeFormula.Add("maxAdj2", "*/ 100000 w1 ss");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("x1", "*/ ss a2 100000");
        shapeFormula.Add("x4", "+- r 0 x1");
        shapeFormula.Add("dy1", "*/ h a1 200000");
        shapeFormula.Add("dy2", "*/ h a3 -200000");
        shapeFormula.Add("ly1", "+- vc dy2 dy1");
        shapeFormula.Add("ry4", "+- vc dy1 dy2");
        shapeFormula.Add("ly2", "+- ly1 dy1 0");
        shapeFormula.Add("ry3", "+- b 0 ly2");
        shapeFormula.Add("ly4", "*/ ly2 2 1");
        shapeFormula.Add("ry1", "+- b 0 ly4");
        shapeFormula.Add("ly3", "+- ly4 0 ly1");
        shapeFormula.Add("ry2", "+- b 0 ly3");
        shapeFormula.Add("hR", "*/ a3 ss 400000");
        shapeFormula.Add("x2", "+- hc 0 wd32");
        shapeFormula.Add("x3", "+- hc wd32 0");
        shapeFormula.Add("y1", "+- ly1 hR 0");
        shapeFormula.Add("y2", "+- ry2 0 hR");
        break;
      case AutoShapeType.DiagonalStripe:
        shapeFormula.Add("a", "pin 0 adj 100000");
        shapeFormula.Add("x2", "*/ w a 100000");
        shapeFormula.Add("x1", "*/ x2 1 2");
        shapeFormula.Add("x3", "+/ x2 r 2");
        shapeFormula.Add("y2", "*/ h a 100000");
        shapeFormula.Add("y1", "*/ y2 1 2");
        shapeFormula.Add("y3", "+/ y2 b 2");
        break;
      case AutoShapeType.Pie:
        shapeFormula.Add("stAng", "pin 0 adj1 21599999");
        shapeFormula.Add("enAng", "pin 0 adj2 21599999");
        shapeFormula.Add("sw1", "+- enAng 0 stAng");
        shapeFormula.Add("sw2", "+- sw1 21600000 0");
        shapeFormula.Add("swAng", "?: sw1 sw1 sw2");
        shapeFormula.Add("wt1", "sin wd2 stAng");
        shapeFormula.Add("ht1", "cos hd2 stAng");
        shapeFormula.Add("dx1", "cat2 wd2 ht1 wt1");
        shapeFormula.Add("dy1", "sat2 hd2 ht1 wt1");
        shapeFormula.Add("x1", "+- hc dx1 0");
        shapeFormula.Add("y1", "+- vc dy1 0");
        shapeFormula.Add("wt2", "sin wd2 enAng");
        shapeFormula.Add("ht2", "cos hd2 enAng");
        shapeFormula.Add("dx2", "cat2 wd2 ht2 wt2");
        shapeFormula.Add("dy2", "sat2 hd2 ht2 wt2");
        shapeFormula.Add("x2", "+- hc dx2 0");
        shapeFormula.Add("y2", "+- vc dy2 0");
        shapeFormula.Add("idx", "cos wd2 45");
        shapeFormula.Add("idy", "sin hd2 45");
        shapeFormula.Add("il", "+- hc 0 idx");
        shapeFormula.Add("ir", "+- hc idx 0");
        shapeFormula.Add("it", "+- vc 0 idy");
        shapeFormula.Add("ib", "+- vc idy 0");
        break;
      case AutoShapeType.Decagon:
        shapeFormula.Add("shd2", "*/ hd2 vf 100000");
        shapeFormula.Add("dx1", "cos wd2 36");
        shapeFormula.Add("dx2", "cos wd2 72");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x2", "+- hc 0 dx2");
        shapeFormula.Add("x3", "+- hc dx2 0");
        shapeFormula.Add("x4", "+- hc dx1 0");
        shapeFormula.Add("dy1", "sin shd2 72");
        shapeFormula.Add("dy2", "sin shd2 36");
        shapeFormula.Add("y1", "+- vc 0 dy1");
        shapeFormula.Add("y2", "+- vc 0 dy2");
        shapeFormula.Add("y3", "+- vc dy2 0");
        shapeFormula.Add("y4", "+- vc dy1 0");
        break;
      case AutoShapeType.Heptagon:
        shapeFormula.Add("swd2", "*/ wd2 hf 100000");
        shapeFormula.Add("shd2", "*/ hd2 vf 100000");
        shapeFormula.Add("svc", "*/ vc  vf 100000");
        shapeFormula.Add("dx1", "*/ swd2 97493 100000");
        shapeFormula.Add("dx2", "*/ swd2 78183 100000");
        shapeFormula.Add("dx3", "*/ swd2 43388 100000");
        shapeFormula.Add("dy1", "*/ shd2 62349 100000");
        shapeFormula.Add("dy2", "*/ shd2 22252 100000");
        shapeFormula.Add("dy3", "*/ shd2 90097 100000");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x2", "+- hc 0 dx2");
        shapeFormula.Add("x3", "+- hc 0 dx3");
        shapeFormula.Add("x4", "+- hc dx3 0");
        shapeFormula.Add("x5", "+- hc dx2 0");
        shapeFormula.Add("x6", "+- hc dx1 0");
        shapeFormula.Add("y1", "+- svc 0 dy1");
        shapeFormula.Add("y2", "+- svc dy2 0");
        shapeFormula.Add("y3", "+- svc dy3 0");
        shapeFormula.Add("ib", "+- b 0 y1");
        break;
      case AutoShapeType.Dodecagon:
        shapeFormula.Add("x1", "*/ w 2894 21600");
        shapeFormula.Add("x2", "*/ w 7906 21600");
        shapeFormula.Add("x3", "*/ w 13694 21600");
        shapeFormula.Add("x4", "*/ w 18706 21600");
        shapeFormula.Add("y1", "*/ h 2894 21600");
        shapeFormula.Add("y2", "*/ h 7906 21600");
        shapeFormula.Add("y3", "*/ h 13694 21600");
        shapeFormula.Add("y4", "*/ h 18706 21600");
        break;
      case AutoShapeType.Star6Point:
        shapeFormula.Add("a", "pin 0 adj 50000");
        shapeFormula.Add("swd2", "*/ wd2 hf 100000");
        shapeFormula.Add("dx1", "cos swd2 30");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x2", "+- hc dx1 0");
        shapeFormula.Add("y2", "+- vc hd4 0");
        shapeFormula.Add("iwd2", "*/ swd2 a 50000");
        shapeFormula.Add("ihd2", "*/ hd2 a 50000");
        shapeFormula.Add("sdx2", "*/ iwd2 1 2");
        shapeFormula.Add("sx1", "+- hc 0 iwd2");
        shapeFormula.Add("sx2", "+- hc 0 sdx2");
        shapeFormula.Add("sx3", "+- hc sdx2 0");
        shapeFormula.Add("sx4", "+- hc iwd2 0");
        shapeFormula.Add("sdy1", "sin ihd2 60");
        shapeFormula.Add("sy1", "+- vc 0 sdy1");
        shapeFormula.Add("sy2", "+- vc sdy1 0");
        shapeFormula.Add("yAdj", "+- vc 0 ihd2");
        break;
      case AutoShapeType.Star7Point:
        shapeFormula.Add("a", "pin 0 adj 50000");
        shapeFormula.Add("swd2", "*/ wd2 hf 100000");
        shapeFormula.Add("shd2", "*/ hd2 vf 100000");
        shapeFormula.Add("svc", "*/ vc  vf 100000");
        shapeFormula.Add("dx1", "*/ swd2 97493 100000");
        shapeFormula.Add("dx2", "*/ swd2 78183 100000");
        shapeFormula.Add("dx3", "*/ swd2 43388 100000");
        shapeFormula.Add("dy1", "*/ shd2 62349 100000");
        shapeFormula.Add("dy2", "*/ shd2 22252 100000");
        shapeFormula.Add("dy3", "*/ shd2 90097 100000");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x2", "+- hc 0 dx2");
        shapeFormula.Add("x3", "+- hc 0 dx3");
        shapeFormula.Add("x4", "+- hc dx3 0");
        shapeFormula.Add("x5", "+- hc dx2 0");
        shapeFormula.Add("x6", "+- hc dx1 0");
        shapeFormula.Add("y1", "+- svc 0 dy1");
        shapeFormula.Add("y2", "+- svc dy2 0");
        shapeFormula.Add("y3", "+- svc dy3 0");
        shapeFormula.Add("iwd2", "*/ swd2 a 50000");
        shapeFormula.Add("ihd2", "*/ shd2 a 50000");
        shapeFormula.Add("sdx1", "*/ iwd2 97493 100000");
        shapeFormula.Add("sdx2", "*/ iwd2 78183 100000");
        shapeFormula.Add("sdx3", "*/ iwd2 43388 100000");
        shapeFormula.Add("sx1", "+- hc 0 sdx1");
        shapeFormula.Add("sx2", "+- hc 0 sdx2");
        shapeFormula.Add("sx3", "+- hc 0 sdx3");
        shapeFormula.Add("sx4", "+- hc sdx3 0");
        shapeFormula.Add("sx5", "+- hc sdx2 0");
        shapeFormula.Add("sx6", "+- hc sdx1 0");
        shapeFormula.Add("sdy1", "*/ ihd2 90097 100000");
        shapeFormula.Add("sdy2", "*/ ihd2 22252 100000");
        shapeFormula.Add("sdy3", "*/ ihd2 62349 100000");
        shapeFormula.Add("sy1", "+- svc 0 sdy1");
        shapeFormula.Add("sy2", "+- svc 0 sdy2");
        shapeFormula.Add("sy3", "+- svc sdy3 0");
        shapeFormula.Add("sy4", "+- svc ihd2 0");
        shapeFormula.Add("yAdj", "+- svc 0 ihd2");
        break;
      case AutoShapeType.Star10Point:
        shapeFormula.Add("a", "pin 0 adj 50000");
        shapeFormula.Add("swd2", "*/ wd2 hf 100000");
        shapeFormula.Add("dx1", "*/ swd2 95106 100000");
        shapeFormula.Add("dx2", "*/ swd2 58779 100000");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x2", "+- hc 0 dx2");
        shapeFormula.Add("x3", "+- hc dx2 0");
        shapeFormula.Add("x4", "+- hc dx1 0");
        shapeFormula.Add("dy1", "*/ hd2 80902 100000");
        shapeFormula.Add("dy2", "*/ hd2 30902 100000");
        shapeFormula.Add("y1", "+- vc 0 dy1");
        shapeFormula.Add("y2", "+- vc 0 dy2");
        shapeFormula.Add("y3", "+- vc dy2 0");
        shapeFormula.Add("y4", "+- vc dy1 0");
        shapeFormula.Add("iwd2", "*/ swd2 a 50000");
        shapeFormula.Add("ihd2", "*/ hd2 a 50000");
        shapeFormula.Add("sdx1", "*/ iwd2 80902 100000");
        shapeFormula.Add("sdx2", "*/ iwd2 30902 100000");
        shapeFormula.Add("sdy1", "*/ ihd2 95106 100000");
        shapeFormula.Add("sdy2", "*/ ihd2 58779 100000");
        shapeFormula.Add("sx1", "+- hc 0 iwd2");
        shapeFormula.Add("sx2", "+- hc 0 sdx1");
        shapeFormula.Add("sx3", "+- hc 0 sdx2");
        shapeFormula.Add("sx4", "+- hc sdx2 0");
        shapeFormula.Add("sx5", "+- hc sdx1 0");
        shapeFormula.Add("sx6", "+- hc iwd2 0");
        shapeFormula.Add("sy1", "+- vc 0 sdy1");
        shapeFormula.Add("sy2", "+- vc 0 sdy2");
        shapeFormula.Add("sy3", "+- vc sdy2 0");
        shapeFormula.Add("sy4", "+- vc sdy1 0");
        shapeFormula.Add("yAdj", "+- vc 0 ihd2");
        break;
      case AutoShapeType.Star12Point:
        shapeFormula.Add("a", "pin 0 adj 50000");
        shapeFormula.Add("dx1", "cos wd2 30");
        shapeFormula.Add("dy1", "sin hd2 60");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x3", "*/ w 3 4");
        shapeFormula.Add("x4", "+- hc dx1 0");
        shapeFormula.Add("y1", "+- vc 0 dy1");
        shapeFormula.Add("y3", "*/ h 3 4");
        shapeFormula.Add("y4", "+- vc dy1 0");
        shapeFormula.Add("iwd2", "*/ wd2 a 50000");
        shapeFormula.Add("ihd2", "*/ hd2 a 50000");
        shapeFormula.Add("sdx1", "cos iwd2 15");
        shapeFormula.Add("sdx2", "cos iwd2 45");
        shapeFormula.Add("sdx3", "cos iwd2 75");
        shapeFormula.Add("sdy1", "sin ihd2 75");
        shapeFormula.Add("sdy2", "sin ihd2 45");
        shapeFormula.Add("sdy3", "sin ihd2 15");
        shapeFormula.Add("sx1", "+- hc 0 sdx1");
        shapeFormula.Add("sx2", "+- hc 0 sdx2");
        shapeFormula.Add("sx3", "+- hc 0 sdx3");
        shapeFormula.Add("sx4", "+- hc sdx3 0");
        shapeFormula.Add("sx5", "+- hc sdx2 0");
        shapeFormula.Add("sx6", "+- hc sdx1 0");
        shapeFormula.Add("sy1", "+- vc 0 sdy1");
        shapeFormula.Add("sy2", "+- vc 0 sdy2");
        shapeFormula.Add("sy3", "+- vc 0 sdy3");
        shapeFormula.Add("sy4", "+- vc sdy3 0");
        shapeFormula.Add("sy5", "+- vc sdy2 0");
        shapeFormula.Add("sy6", "+- vc sdy1 0");
        shapeFormula.Add("yAdj", "+- vc 0 ihd2");
        break;
      case AutoShapeType.RoundSingleCornerRectangle:
        shapeFormula.Add("a", "pin 0 adj 50000");
        shapeFormula.Add("dx1", "*/ ss a 100000");
        shapeFormula.Add("x1", "+- r 0 dx1");
        shapeFormula.Add("idx", "*/ dx1 29289 100000");
        shapeFormula.Add("ir", "+- r 0 idx");
        break;
      case AutoShapeType.RoundSameSideCornerRectangle:
        shapeFormula.Add("a1", "pin 0 adj1 50000");
        shapeFormula.Add("a2", "pin 0 adj2 50000");
        shapeFormula.Add("tx1", "*/ ss a1 100000");
        shapeFormula.Add("tx2", "+- r 0 tx1");
        shapeFormula.Add("bx1", "*/ ss a2 100000");
        shapeFormula.Add("bx2", "+- r 0 bx1");
        shapeFormula.Add("by1", "+- b 0 bx1");
        shapeFormula.Add("d", "+- tx1 0 bx1");
        shapeFormula.Add("tdx", "*/ tx1 29289 100000");
        shapeFormula.Add("bdx", "*/ bx1 29289 100000");
        shapeFormula.Add("il", "?: d tdx bdx");
        shapeFormula.Add("ir", "+- r 0 il");
        shapeFormula.Add("ib", "+- b 0 bdx");
        break;
      case AutoShapeType.RoundDiagonalCornerRectangle:
        shapeFormula.Add("a1", "pin 0 adj1 50000");
        shapeFormula.Add("a2", "pin 0 adj2 50000");
        shapeFormula.Add("x1", "*/ ss a1 100000");
        shapeFormula.Add("y1", "+- b 0 x1");
        shapeFormula.Add("a", "*/ ss a2 100000");
        shapeFormula.Add("x2", "+- r 0 a");
        shapeFormula.Add("y2", "+- b 0 a");
        shapeFormula.Add("dx1", "*/ x1 29289 100000");
        shapeFormula.Add("dx2", "*/ a 29289 100000");
        shapeFormula.Add("d", "+- dx1 0 dx2");
        shapeFormula.Add("dx", "?: d dx1 dx2");
        shapeFormula.Add("ir", "+- r 0 dx");
        shapeFormula.Add("ib", "+- b 0 dx");
        break;
      case AutoShapeType.SnipAndRoundSingleCornerRectangle:
        shapeFormula.Add("a1", "pin 0 adj1 50000");
        shapeFormula.Add("a2", "pin 0 adj2 50000");
        shapeFormula.Add("x1", "*/ ss a1 100000");
        shapeFormula.Add("dx2", "*/ ss a2 100000");
        shapeFormula.Add("x2", "+- r 0 dx2");
        shapeFormula.Add("il", "*/ x1 29289 100000");
        shapeFormula.Add("ir", "+/ x2 r 2");
        break;
      case AutoShapeType.SnipSingleCornerRectangle:
        shapeFormula.Add("a", "pin 0 adj 50000");
        shapeFormula.Add("dx1", "*/ ss a 100000");
        shapeFormula.Add("x1", "+- r 0 dx1");
        shapeFormula.Add("it", "*/ dx1 1 2");
        shapeFormula.Add("ir", "+/ x1 r 2");
        break;
      case AutoShapeType.SnipSameSideCornerRectangle:
        shapeFormula.Add("a1", "pin 0 adj1 50000");
        shapeFormula.Add("a2", "pin 0 adj2 50000");
        shapeFormula.Add("tx1", "*/ ss a1 100000");
        shapeFormula.Add("tx2", "+- r 0 tx1");
        shapeFormula.Add("bx1", "*/ ss a2 100000");
        shapeFormula.Add("bx2", "+- r 0 bx1");
        shapeFormula.Add("by1", "+- b 0 bx1");
        shapeFormula.Add("d", "+- tx1 0 bx1");
        shapeFormula.Add("dx", "?: d tx1 bx1");
        shapeFormula.Add("il", "*/ dx 1 2");
        shapeFormula.Add("ir", "+- r 0 il");
        shapeFormula.Add("it", "*/ tx1 1 2");
        shapeFormula.Add("ib", "+/ by1 b 2");
        break;
      case AutoShapeType.SnipDiagonalCornerRectangle:
        shapeFormula.Add("a1", "pin 0 adj1 50000");
        shapeFormula.Add("a2", "pin 0 adj2 50000");
        shapeFormula.Add("lx1", "*/ ss a1 100000");
        shapeFormula.Add("lx2", "+- r 0 lx1");
        shapeFormula.Add("ly1", "+- b 0 lx1");
        shapeFormula.Add("rx1", "*/ ss a2 100000");
        shapeFormula.Add("rx2", "+- r 0 rx1");
        shapeFormula.Add("ry1", "+- b 0 rx1");
        shapeFormula.Add("d", "+- lx1 0 rx1");
        shapeFormula.Add("dx", "?: d lx1 rx1");
        shapeFormula.Add("il", "*/ dx 1 2");
        shapeFormula.Add("ir", "+- r 0 il");
        shapeFormula.Add("ib", "+- b 0 il");
        break;
      case AutoShapeType.Frame:
        shapeFormula.Add("a1", "pin 0 adj1 50000");
        shapeFormula.Add("x1", "*/ ss a1 100000");
        shapeFormula.Add("x4", "+- r 0 x1");
        shapeFormula.Add("y4", "+- b 0 x1");
        break;
      case AutoShapeType.HalfFrame:
        shapeFormula.Add("maxAdj2", "*/ 100000 w ss");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("x1", "*/ ss a2 100000");
        shapeFormula.Add("g1", "*/ h x1 w");
        shapeFormula.Add("g2", "+- h 0 g1");
        shapeFormula.Add("maxAdj1", "*/ 100000 g2 ss");
        shapeFormula.Add("a1", "pin 0 adj1 maxAdj1");
        shapeFormula.Add("y1", "*/ ss a1 100000");
        shapeFormula.Add("dx2", "*/ y1 w h");
        shapeFormula.Add("x2", "+- r 0 dx2");
        shapeFormula.Add("dy2", "*/ x1 h w");
        shapeFormula.Add("y2", "+- b 0 dy2");
        shapeFormula.Add("cx1", "*/ x1 1 2");
        shapeFormula.Add("cy1", "+/ y2 b 2");
        shapeFormula.Add("cx2", "+/ x2 r 2");
        shapeFormula.Add("cy2", "*/ y1 1 2");
        break;
      case AutoShapeType.Teardrop:
        shapeFormula.Add("a", "pin 0 adj 200000");
        shapeFormula.Add("r2", "sqrt 2");
        shapeFormula.Add("tw", "*/ wd2 r2 1");
        shapeFormula.Add("th", "*/ hd2 r2 1");
        shapeFormula.Add("sw", "*/ tw a 100000");
        shapeFormula.Add("sh", "*/ th a 100000");
        shapeFormula.Add("dx1", "cos sw 45");
        shapeFormula.Add("dy1", "sin sh 45");
        shapeFormula.Add("x1", "+- hc dx1 0");
        shapeFormula.Add("y1", "+- vc 0 dy1");
        shapeFormula.Add("x2", "+/ hc x1 2");
        shapeFormula.Add("y2", "+/ vc y1 2");
        shapeFormula.Add("idx", "cos wd2 45");
        shapeFormula.Add("idy", "sin hd2 45");
        shapeFormula.Add("il", "+- hc 0 idx");
        shapeFormula.Add("ir", "+- hc idx 0");
        shapeFormula.Add("it", "+- vc 0 idy");
        shapeFormula.Add("ib", "+- vc idy 0");
        break;
      case AutoShapeType.Chord:
        shapeFormula.Add("stAng", "pin 0 adj1 21599999");
        shapeFormula.Add("enAng", "pin 0 adj2 21599999");
        shapeFormula.Add("sw1", "+- enAng 0 stAng");
        shapeFormula.Add("sw2", "+- sw1 21600000 0");
        shapeFormula.Add("swAng", "?: sw1 sw1 sw2");
        shapeFormula.Add("wt1", "sin wd2 stAng");
        shapeFormula.Add("ht1", "cos hd2 stAng");
        shapeFormula.Add("dx1", "cat2 wd2 ht1 wt1");
        shapeFormula.Add("dy1", "sat2 hd2 ht1 wt1");
        shapeFormula.Add("wt2", "sin wd2 enAng");
        shapeFormula.Add("ht2", "cos hd2 enAng");
        shapeFormula.Add("dx2", "cat2 wd2 ht2 wt2");
        shapeFormula.Add("dy2", "sat2 hd2 ht2 wt2");
        shapeFormula.Add("x1", "+- hc dx1 0");
        shapeFormula.Add("y1", "+- vc dy1 0");
        shapeFormula.Add("x2", "+- hc dx2 0");
        shapeFormula.Add("y2", "+- vc dy2 0");
        shapeFormula.Add("x3", "+/ x1 x2 2");
        shapeFormula.Add("y3", "+/ y1 y2 2");
        shapeFormula.Add("midAng0", "*/ swAng 1 2");
        shapeFormula.Add("midAng", "+- stAng midAng0 cd2");
        shapeFormula.Add("idx", "cos wd2 45");
        shapeFormula.Add("idy", "sin hd2 45");
        shapeFormula.Add("il", "+- hc 0 idx");
        shapeFormula.Add("ir", "+- hc idx 0");
        shapeFormula.Add("it", "+- vc 0 idy");
        shapeFormula.Add("ib", "+- vc idy 0");
        break;
      case AutoShapeType.Corner:
        shapeFormula.Add("maxAdj1", "*/ 100000 h ss");
        shapeFormula.Add("maxAdj2", "*/ 100000 w ss");
        shapeFormula.Add("a1", "pin 0 adj1 maxAdj1");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("x1", "*/ ss a2 100000");
        shapeFormula.Add("dy1", "*/ ss a1 100000");
        shapeFormula.Add("y1", "+- b 0 dy1");
        shapeFormula.Add("cx1", "*/ x1 1 2");
        shapeFormula.Add("cy1", "+/ y1 b 2");
        shapeFormula.Add("d", "+- w 0 h");
        shapeFormula.Add("it", "?: d y1 t");
        shapeFormula.Add("ir", "?: d r x1");
        break;
      case AutoShapeType.MathPlus:
        shapeFormula.Add("a1", "pin 0 adj1 73490");
        shapeFormula.Add("dx1", "*/ w 73490 200000");
        shapeFormula.Add("dy1", "*/ h 73490 200000");
        shapeFormula.Add("dx2", "*/ ss a1 200000");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x2", "+- hc 0 dx2");
        shapeFormula.Add("x3", "+- hc dx2 0");
        shapeFormula.Add("x4", "+- hc dx1 0");
        shapeFormula.Add("y1", "+- vc 0 dy1");
        shapeFormula.Add("y2", "+- vc 0 dx2");
        shapeFormula.Add("y3", "+- vc dx2 0");
        shapeFormula.Add("y4", "+- vc dy1 0");
        break;
      case AutoShapeType.MathMinus:
        shapeFormula.Add("a1", "pin 0 adj1 100000");
        shapeFormula.Add("dy1", "*/ h a1 200000");
        shapeFormula.Add("dx1", "*/ w 73490 200000");
        shapeFormula.Add("y1", "+- vc 0 dy1");
        shapeFormula.Add("y2", "+- vc dy1 0");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x2", "+- hc dx1 0");
        break;
      case AutoShapeType.MathMultiply:
        shapeFormula.Add("a1", "pin 0 adj1 51965");
        shapeFormula.Add("th", "*/ ss a1 100000");
        shapeFormula.Add("a0", "at2 w h");
        shapeFormula.Add("a", "*/ a0 180 " + Math.PI.ToString());
        shapeFormula.Add("sa", "sin 1 a");
        shapeFormula.Add("ca", "cos 1 a");
        shapeFormula.Add("ta", "tan 1 a");
        shapeFormula.Add("dl", "mod w h 0");
        shapeFormula.Add("rw", "*/ dl 51965 100000");
        shapeFormula.Add("lM", "+- dl 0 rw");
        shapeFormula.Add("xM", "*/ ca lM 2");
        shapeFormula.Add("yM", "*/ sa lM 2");
        shapeFormula.Add("dxAM", "*/ sa th 2");
        shapeFormula.Add("dyAM", "*/ ca th 2");
        shapeFormula.Add("xA", "+- xM 0 dxAM");
        shapeFormula.Add("yA", "+- yM dyAM 0");
        shapeFormula.Add("xB", "+- xM dxAM 0");
        shapeFormula.Add("yB", "+- yM 0 dyAM");
        shapeFormula.Add("xBC", "+- hc 0 xB");
        shapeFormula.Add("yBC", "*/ xBC ta 1");
        shapeFormula.Add("yC", "+- yBC yB 0");
        shapeFormula.Add("xD", "+- r 0 xB");
        shapeFormula.Add("xE", "+- r 0 xA");
        shapeFormula.Add("yFE", "+- vc 0 yA");
        shapeFormula.Add("xFE", "*/ yFE 1 ta");
        shapeFormula.Add("xF", "+- xE 0 xFE");
        shapeFormula.Add("xL", "+- xA xFE 0");
        shapeFormula.Add("yG", "+- b 0 yA");
        shapeFormula.Add("yH", "+- b 0 yB");
        shapeFormula.Add("yI", "+- b 0 yC");
        shapeFormula.Add("xC2", "+- r 0 xM");
        shapeFormula.Add("yC3", "+- b 0 yM");
        break;
      case AutoShapeType.MathDivision:
        shapeFormula.Add("a1", "pin 1000 adj1 36745");
        shapeFormula.Add("ma1", "+- 0 0 a1");
        shapeFormula.Add("ma3h", "+/ 73490 ma1 4");
        shapeFormula.Add("ma3w", "*/ 36745 w h");
        shapeFormula.Add("maxAdj3", "min ma3h ma3w");
        shapeFormula.Add("a3", "pin 1000 adj3 maxAdj3");
        shapeFormula.Add("m4a3", "*/ -4 a3 1");
        shapeFormula.Add("maxAdj2", "+- 73490 m4a3 a1");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("dy1", "*/ h a1 200000");
        shapeFormula.Add("yg", "*/ h a2 100000");
        shapeFormula.Add("rad", "*/ h a3 100000");
        shapeFormula.Add("dx1", "*/ w 73490 200000");
        shapeFormula.Add("y3", "+- vc 0 dy1");
        shapeFormula.Add("y4", "+- vc dy1 0");
        shapeFormula.Add("a", "+- yg rad 0");
        shapeFormula.Add("y2", "+- y3 0 a");
        shapeFormula.Add("y1", "+- y2 0 rad");
        shapeFormula.Add("y5", "+- b 0 y1");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x3", "+- hc dx1 0");
        shapeFormula.Add("x2", "+- hc 0 rad");
        break;
      case AutoShapeType.MathEqual:
        shapeFormula.Add("a1", "pin 0 adj1 36745");
        shapeFormula.Add("2a1", "*/ a1 2 1");
        shapeFormula.Add("mAdj2", "+- 100000 0 2a1");
        shapeFormula.Add("a2", "pin 0 adj2 mAdj2");
        shapeFormula.Add("dy1", "*/ h a1 100000");
        shapeFormula.Add("dy2", "*/ h a2 200000");
        shapeFormula.Add("dx1", "*/ w 73490 200000");
        shapeFormula.Add("y2", "+- vc 0 dy2");
        shapeFormula.Add("y3", "+- vc dy2 0");
        shapeFormula.Add("y1", "+- y2 0 dy1");
        shapeFormula.Add("y4", "+- y3 dy1 0");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x2", "+- hc dx1 0");
        shapeFormula.Add("yC1", "+/ y1 y2 2");
        shapeFormula.Add("yC2", "+/ y3 y4 2");
        break;
      case AutoShapeType.MathNotEqual:
        shapeFormula.Add("a1", "pin 0 adj1 50000");
        shapeFormula.Add("crAng", "pin 4200000 adj2 6600000");
        shapeFormula.Add("2a1", "*/ a1 2 1");
        shapeFormula.Add("maxAdj3", "+- 100000 0 2a1");
        shapeFormula.Add("a3", "pin 0 adj3 maxAdj3");
        shapeFormula.Add("dy1", "*/ h a1 100000");
        shapeFormula.Add("dy2", "*/ h a3 200000");
        shapeFormula.Add("dx1", "*/ w 73490 200000");
        shapeFormula.Add("x1", "+- hc 0 dx1");
        shapeFormula.Add("x8", "+- hc dx1 0");
        shapeFormula.Add("y2", "+- vc 0 dy2");
        shapeFormula.Add("y3", "+- vc dy2 0");
        shapeFormula.Add("y1", "+- y2 0 dy1");
        shapeFormula.Add("y4", "+- y3 dy1 0");
        shapeFormula.Add("cadj2", "+- crAng 0 cd4");
        shapeFormula.Add("xadj2", "tan hd2 cadj2");
        shapeFormula.Add("len", "mod xadj2 hd2 0");
        shapeFormula.Add("bhw", "*/ len dy1 hd2");
        shapeFormula.Add("bhw2", "*/ bhw 1 2");
        shapeFormula.Add("x7", "+- hc xadj2 bhw2");
        shapeFormula.Add("dx67", "*/ xadj2 y1 hd2");
        shapeFormula.Add("x6", "+- x7 0 dx67");
        shapeFormula.Add("dx57", "*/ xadj2 y2 hd2");
        shapeFormula.Add("x5", "+- x7 0 dx57");
        shapeFormula.Add("dx47", "*/ xadj2 y3 hd2");
        shapeFormula.Add("x4", "+- x7 0 dx47");
        shapeFormula.Add("dx37", "*/ xadj2 y4 hd2");
        shapeFormula.Add("x3", "+- x7 0 dx37");
        shapeFormula.Add("dx27", "*/ xadj2 2 1");
        shapeFormula.Add("x2", "+- x7 0 dx27");
        shapeFormula.Add("rx7", "+- x7 bhw 0");
        shapeFormula.Add("rx6", "+- x6 bhw 0");
        shapeFormula.Add("rx5", "+- x5 bhw 0");
        shapeFormula.Add("rx4", "+- x4 bhw 0");
        shapeFormula.Add("rx3", "+- x3 bhw 0");
        shapeFormula.Add("rx2", "+- x2 bhw 0");
        shapeFormula.Add("dx7", "*/ dy1 hd2 len");
        shapeFormula.Add("rxt", "+- x7 dx7 0");
        shapeFormula.Add("lxt", "+- rx7 0 dx7");
        shapeFormula.Add("rx", "?: cadj2 rxt rx7");
        shapeFormula.Add("lx", "?: cadj2 x7 lxt");
        shapeFormula.Add("dy3", "*/ dy1 xadj2 len");
        shapeFormula.Add("dy4", "+- 0 0 dy3");
        shapeFormula.Add("ry", "?: cadj2 dy3 t");
        shapeFormula.Add("ly", "?: cadj2 t dy4");
        shapeFormula.Add("dlx", "+- w 0 rx");
        shapeFormula.Add("drx", "+- w 0 lx");
        shapeFormula.Add("dly", "+- h 0 ry");
        shapeFormula.Add("dry", "+- h 0 ly");
        shapeFormula.Add("xC1", "+/ rx lx 2");
        shapeFormula.Add("xC2", "+/ drx dlx 2");
        shapeFormula.Add("yC1", "+/ ry ly 2");
        shapeFormula.Add("yC2", "+/ y1 y2 2");
        shapeFormula.Add("yC3", "+/ y3 y4 2");
        shapeFormula.Add("yC4", "+/ dry dly 2");
        break;
      case AutoShapeType.Gear6:
        shapeFormula.Add("a1", "pin 0 adj1 20000");
        shapeFormula.Add("a2", "pin 0 adj2 5358");
        shapeFormula.Add("th", "*/ ss a1 100000");
        shapeFormula.Add("lFD", "*/ ss a2 100000");
        shapeFormula.Add("th2", "*/ th 1 2");
        shapeFormula.Add("l2", "*/ lFD 1 2");
        shapeFormula.Add("l3", "+- th2 l2 0");
        shapeFormula.Add("rh", "+- hd2 0 th");
        shapeFormula.Add("rw", "+- wd2 0 th");
        shapeFormula.Add("dr", "+- rw 0 rh");
        shapeFormula.Add("maxr", "?: dr rh rw");
        shapeFormula.Add("ha", "at2 maxr l3");
        shapeFormula.Add("aA1", "+- 19800000 0 ha");
        shapeFormula.Add("aD1", "+- 19800000 ha 0");
        shapeFormula.Add("ta11", "cos rw aA1");
        shapeFormula.Add("ta12", "sin rh aA1");
        shapeFormula.Add("bA1", "at2 ta11 ta12");
        shapeFormula.Add("cta1", "cos rh bA1");
        shapeFormula.Add("sta1", "sin rw bA1");
        shapeFormula.Add("ma1", "mod cta1 sta1 0");
        shapeFormula.Add("na1", "*/ rw rh ma1");
        shapeFormula.Add("dxa1", "cos na1 bA1");
        shapeFormula.Add("dya1", "sin na1 bA1");
        shapeFormula.Add("xA1", "+- hc dxa1 0");
        shapeFormula.Add("yA1", "+- vc dya1 0");
        shapeFormula.Add("td11", "cos rw aD1");
        shapeFormula.Add("td12", "sin rh aD1");
        shapeFormula.Add("bD1", "at2 td11 td12");
        shapeFormula.Add("ctd1", "cos rh bD1");
        shapeFormula.Add("std1", "sin rw bD1");
        shapeFormula.Add("md1", "mod ctd1 std1 0");
        shapeFormula.Add("nd1", "*/ rw rh md1");
        shapeFormula.Add("dxd1", "cos nd1 bD1");
        shapeFormula.Add("dyd1", "sin nd1 bD1");
        shapeFormula.Add("xD1", "+- hc dxd1 0");
        shapeFormula.Add("yD1", "+- vc dyd1 0");
        shapeFormula.Add("xAD1", "+- xA1 0 xD1");
        shapeFormula.Add("yAD1", "+- yA1 0 yD1");
        shapeFormula.Add("lAD1", "mod xAD1 yAD1 0");
        shapeFormula.Add("a10", "at2 yAD1 xAD1");
        shapeFormula.Add("dxF1", "sin lFD a1");
        shapeFormula.Add("dyF1", "cos lFD a1");
        shapeFormula.Add("xF1", "+- xD1 dxF1 0");
        shapeFormula.Add("yF1", "+- yD1 dyF1 0");
        shapeFormula.Add("xE1", "+- xA1 0 dxF1");
        shapeFormula.Add("yE1", "+- yA1 0 dyF1");
        shapeFormula.Add("yC1t", "sin th a1");
        shapeFormula.Add("xC1t", "cos th a1");
        shapeFormula.Add("yC1", "+- yF1 yC1t 0");
        shapeFormula.Add("xC1", "+- xF1 0 xC1t");
        shapeFormula.Add("yB1", "+- yE1 yC1t 0");
        shapeFormula.Add("xB1", "+- xE1 0 xC1t");
        shapeFormula.Add("aD6", "+- 3cd4 ha 0");
        shapeFormula.Add("td61", "cos rw aD6");
        shapeFormula.Add("td62", "sin rh aD6");
        shapeFormula.Add("bD6", "at2 td61 td62");
        shapeFormula.Add("ctd6", "cos rh bD6");
        shapeFormula.Add("std6", "sin rw bD6");
        shapeFormula.Add("md6", "mod ctd6 std6 0");
        shapeFormula.Add("nd6", "*/ rw rh md6");
        shapeFormula.Add("dxd6", "cos nd6 bD6");
        shapeFormula.Add("dyd6", "sin nd6 bD6");
        shapeFormula.Add("xD6", "+- hc dxd6 0");
        shapeFormula.Add("yD6", "+- vc dyd6 0");
        shapeFormula.Add("xA6", "+- hc 0 dxd6");
        shapeFormula.Add("xF6", "+- xD6 0 lFD");
        shapeFormula.Add("xE6", "+- xA6 lFD 0");
        shapeFormula.Add("yC6", "+- yD6 0 th");
        shapeFormula.Add("swAng1", "+- bA1 0 bD6");
        shapeFormula.Add("aA2", "+- 1800000 0 ha");
        shapeFormula.Add("aD2", "+- 1800000 ha 0");
        shapeFormula.Add("ta21", "cos rw aA2");
        shapeFormula.Add("ta22", "sin rh aA2");
        shapeFormula.Add("bA2", "at2 ta21 ta22");
        shapeFormula.Add("yA2", "+- h 0 yD1");
        shapeFormula.Add("td21", "cos rw aD2");
        shapeFormula.Add("td22", "sin rh aD2");
        shapeFormula.Add("bD2", "at2 td21 td22");
        shapeFormula.Add("yD2", "+- h 0 yA1");
        shapeFormula.Add("yC2", "+- h 0 yB1");
        shapeFormula.Add("yB2", "+- h 0 yC1");
        shapeFormula.Add("xB2", "val xC1");
        shapeFormula.Add("swAng2", "+- bA2 0 bD1");
        shapeFormula.Add("aD3", "+- cd4 ha 0");
        shapeFormula.Add("td31", "cos rw aD3");
        shapeFormula.Add("td32", "sin rh aD3");
        shapeFormula.Add("bD3", "at2 td31 td32");
        shapeFormula.Add("yD3", "+- h 0 yD6");
        shapeFormula.Add("yB3", "+- h 0 yC6");
        shapeFormula.Add("aD4", "+- 9000000 ha 0");
        shapeFormula.Add("td41", "cos rw aD4");
        shapeFormula.Add("td42", "sin rh aD4");
        shapeFormula.Add("bD4", "at2 td41 td42");
        shapeFormula.Add("xD4", "+- w 0 xD1");
        shapeFormula.Add("xC4", "+- w 0 xC1");
        shapeFormula.Add("xB4", "+- w 0 xB1");
        shapeFormula.Add("aD5", "+- 12600000 ha 0");
        shapeFormula.Add("td51", "cos rw aD5");
        shapeFormula.Add("td52", "sin rh aD5");
        shapeFormula.Add("bD5", "at2 td51 td52");
        shapeFormula.Add("xD5", "+- w 0 xA1");
        shapeFormula.Add("xC5", "+- w 0 xB1");
        shapeFormula.Add("xB5", "+- w 0 xC1");
        shapeFormula.Add("xCxn1", "+/ xB1 xC1 2");
        shapeFormula.Add("yCxn1", "+/ yB1 yC1 2");
        shapeFormula.Add("yCxn2", "+- b 0 yCxn1");
        shapeFormula.Add("xCxn4", "+/ r 0 xCxn1");
        break;
      case AutoShapeType.Gear9:
        shapeFormula.Add("a1", "pin 0 adj1 20000");
        shapeFormula.Add("a2", "pin 0 adj2 2679");
        shapeFormula.Add("th", "*/ ss a1 100000");
        shapeFormula.Add("lFD", "*/ ss a2 100000");
        shapeFormula.Add("th2", "*/ th 1 2");
        shapeFormula.Add("l2", "*/ lFD 1 2");
        shapeFormula.Add("l3", "+- th2 l2 0");
        shapeFormula.Add("rh", "+- hd2 0 th");
        shapeFormula.Add("rw", "+- wd2 0 th");
        shapeFormula.Add("dr", "+- rw 0 rh");
        shapeFormula.Add("maxr", "?: dr rh rw");
        shapeFormula.Add("ha", "at2 maxr l3");
        shapeFormula.Add("aA1", "+- 18600000 0 ha");
        shapeFormula.Add("aD1", "+- 18600000 ha 0");
        shapeFormula.Add("ta11", "cos rw aA1");
        shapeFormula.Add("ta12", "sin rh aA1");
        shapeFormula.Add("bA1", "at2 ta11 ta12");
        shapeFormula.Add("cta1", "cos rh bA1");
        shapeFormula.Add("sta1", "sin rw bA1");
        shapeFormula.Add("ma1", "mod cta1 sta1 0");
        shapeFormula.Add("na1", "*/ rw rh ma1");
        shapeFormula.Add("dxa1", "cos na1 bA1");
        shapeFormula.Add("dya1", "sin na1 bA1");
        shapeFormula.Add("xA1", "+- hc dxa1 0");
        shapeFormula.Add("yA1", "+- vc dya1 0");
        shapeFormula.Add("td11", "cos rw aD1");
        shapeFormula.Add("td12", "sin rh aD1");
        shapeFormula.Add("bD1", "at2 td11 td12");
        shapeFormula.Add("ctd1", "cos rh bD1");
        shapeFormula.Add("std1", "sin rw bD1");
        shapeFormula.Add("md1", "mod ctd1 std1 0");
        shapeFormula.Add("nd1", "*/ rw rh md1");
        shapeFormula.Add("dxd1", "cos nd1 bD1");
        shapeFormula.Add("dyd1", "sin nd1 bD1");
        shapeFormula.Add("xD1", "+- hc dxd1 0");
        shapeFormula.Add("yD1", "+- vc dyd1 0");
        shapeFormula.Add("xAD1", "+- xA1 0 xD1");
        shapeFormula.Add("yAD1", "+- yA1 0 yD1");
        shapeFormula.Add("lAD1", "mod xAD1 yAD1 0");
        shapeFormula.Add("a10", "at2 yAD1 xAD1");
        shapeFormula.Add("dxF1", "sin lFD a1");
        shapeFormula.Add("dyF1", "cos lFD a1");
        shapeFormula.Add("xF1", "+- xD1 dxF1 0");
        shapeFormula.Add("yF1", "+- yD1 dyF1 0");
        shapeFormula.Add("xE1", "+- xA1 0 dxF1");
        shapeFormula.Add("yE1", "+- yA1 0 dyF1");
        shapeFormula.Add("yC1t", "sin th a1");
        shapeFormula.Add("xC1t", "cos th a1");
        shapeFormula.Add("yC1", "+- yF1 yC1t 0");
        shapeFormula.Add("xC1", "+- xF1 0 xC1t");
        shapeFormula.Add("yB1", "+- yE1 yC1t 0");
        shapeFormula.Add("xB1", "+- xE1 0 xC1t");
        shapeFormula.Add("aA2", "+- 21000000 0 ha");
        shapeFormula.Add("aD2", "+- 21000000 ha 0");
        shapeFormula.Add("ta21", "cos rw aA2");
        shapeFormula.Add("ta22", "sin rh aA2");
        shapeFormula.Add("bA2", "at2 ta21 ta22");
        shapeFormula.Add("cta2", "cos rh bA2");
        shapeFormula.Add("sta2", "sin rw bA2");
        shapeFormula.Add("ma2", "mod cta2 sta2 0");
        shapeFormula.Add("na2", "*/ rw rh ma2");
        shapeFormula.Add("dxa2", "cos na2 bA2");
        shapeFormula.Add("dya2", "sin na2 bA2");
        shapeFormula.Add("xA2", "+- hc dxa2 0");
        shapeFormula.Add("yA2", "+- vc dya2 0");
        shapeFormula.Add("td21", "cos rw aD2");
        shapeFormula.Add("td22", "sin rh aD2");
        shapeFormula.Add("bD2", "at2 td21 td22");
        shapeFormula.Add("ctd2", "cos rh bD2");
        shapeFormula.Add("std2", "sin rw bD2");
        shapeFormula.Add("md2", "mod ctd2 std2 0");
        shapeFormula.Add("nd2", "*/ rw rh md2");
        shapeFormula.Add("dxd2", "cos nd2 bD2");
        shapeFormula.Add("dyd2", "sin nd2 bD2");
        shapeFormula.Add("xD2", "+- hc dxd2 0");
        shapeFormula.Add("yD2", "+- vc dyd2 0");
        shapeFormula.Add("xAD2", "+- xA2 0 xD2");
        shapeFormula.Add("yAD2", "+- yA2 0 yD2");
        shapeFormula.Add("lAD2", "mod xAD2 yAD2 0");
        shapeFormula.Add("a20", "at2 yAD2 xAD2");
        shapeFormula.Add("dxF2", "sin lFD a2");
        shapeFormula.Add("dyF2", "cos lFD a2");
        shapeFormula.Add("xF2", "+- xD2 dxF2 0");
        shapeFormula.Add("yF2", "+- yD2 dyF2 0");
        shapeFormula.Add("xE2", "+- xA2 0 dxF2");
        shapeFormula.Add("yE2", "+- yA2 0 dyF2");
        shapeFormula.Add("yC2t", "sin th a2");
        shapeFormula.Add("xC2t", "cos th a2");
        shapeFormula.Add("yC2", "+- yF2 yC2t 0");
        shapeFormula.Add("xC2", "+- xF2 0 xC2t");
        shapeFormula.Add("yB2", "+- yE2 yC2t 0");
        shapeFormula.Add("xB2", "+- xE2 0 xC2t");
        shapeFormula.Add("swAng1", "+- bA2 0 bD1");
        shapeFormula.Add("aA3", "+- 1800000 0 ha");
        shapeFormula.Add("aD3", "+- 1800000 ha 0");
        shapeFormula.Add("ta31", "cos rw aA3");
        shapeFormula.Add("ta32", "sin rh aA3");
        shapeFormula.Add("bA3", "at2 ta31 ta32");
        shapeFormula.Add("cta3", "cos rh bA3");
        shapeFormula.Add("sta3", "sin rw bA3");
        shapeFormula.Add("ma3", "mod cta3 sta3 0");
        shapeFormula.Add("na3", "*/ rw rh ma3");
        shapeFormula.Add("dxa3", "cos na3 bA3");
        shapeFormula.Add("dya3", "sin na3 bA3");
        shapeFormula.Add("xA3", "+- hc dxa3 0");
        shapeFormula.Add("yA3", "+- vc dya3 0");
        shapeFormula.Add("td31", "cos rw aD3");
        shapeFormula.Add("td32", "sin rh aD3");
        shapeFormula.Add("bD3", "at2 td31 td32");
        shapeFormula.Add("ctd3", "cos rh bD3");
        shapeFormula.Add("std3", "sin rw bD3");
        shapeFormula.Add("md3", "mod ctd3 std3 0");
        shapeFormula.Add("nd3", "*/ rw rh md3");
        shapeFormula.Add("dxd3", "cos nd3 bD3");
        shapeFormula.Add("dyd3", "sin nd3 bD3");
        shapeFormula.Add("xD3", "+- hc dxd3 0");
        shapeFormula.Add("yD3", "+- vc dyd3 0");
        shapeFormula.Add("xAD3", "+- xA3 0 xD3");
        shapeFormula.Add("yAD3", "+- yA3 0 yD3");
        shapeFormula.Add("lAD3", "mod xAD3 yAD3 0");
        shapeFormula.Add("a3", "at2 yAD3 xAD3");
        shapeFormula.Add("dxF3", "sin lFD a3");
        shapeFormula.Add("dyF3", "cos lFD a3");
        shapeFormula.Add("xF3", "+- xD3 dxF3 0");
        shapeFormula.Add("yF3", "+- yD3 dyF3 0");
        shapeFormula.Add("xE3", "+- xA3 0 dxF3");
        shapeFormula.Add("yE3", "+- yA3 0 dyF3");
        shapeFormula.Add("yC3t", "sin th a3");
        shapeFormula.Add("xC3t", "cos th a3");
        shapeFormula.Add("yC3", "+- yF3 yC3t 0");
        shapeFormula.Add("xC3", "+- xF3 0 xC3t");
        shapeFormula.Add("yB3", "+- yE3 yC3t 0");
        shapeFormula.Add("xB3", "+- xE3 0 xC3t");
        shapeFormula.Add("swAng2", "+- bA3 0 bD2");
        shapeFormula.Add("aA4", "+- 4200000 0 ha");
        shapeFormula.Add("aD4", "+- 4200000 ha 0");
        shapeFormula.Add("ta41", "cos rw aA4");
        shapeFormula.Add("ta42", "sin rh aA4");
        shapeFormula.Add("bA4", "at2 ta41 ta42");
        shapeFormula.Add("cta4", "cos rh bA4");
        shapeFormula.Add("sta4", "sin rw bA4");
        shapeFormula.Add("ma4", "mod cta4 sta4 0");
        shapeFormula.Add("na4", "*/ rw rh ma4");
        shapeFormula.Add("dxa4", "cos na4 bA4");
        shapeFormula.Add("dya4", "sin na4 bA4");
        shapeFormula.Add("xA4", "+- hc dxa4 0");
        shapeFormula.Add("yA4", "+- vc dya4 0");
        shapeFormula.Add("td41", "cos rw aD4");
        shapeFormula.Add("td42", "sin rh aD4");
        shapeFormula.Add("bD4", "at2 td41 td42");
        shapeFormula.Add("ctd4", "cos rh bD4");
        shapeFormula.Add("std4", "sin rw bD4");
        shapeFormula.Add("md4", "mod ctd4 std4 0");
        shapeFormula.Add("nd4", "*/ rw rh md4");
        shapeFormula.Add("dxd4", "cos nd4 bD4");
        shapeFormula.Add("dyd4", "sin nd4 bD4");
        shapeFormula.Add("xD4", "+- hc dxd4 0");
        shapeFormula.Add("yD4", "+- vc dyd4 0");
        shapeFormula.Add("xAD4", "+- xA4 0 xD4");
        shapeFormula.Add("yAD4", "+- yA4 0 yD4");
        shapeFormula.Add("lAD4", "mod xAD4 yAD4 0");
        shapeFormula.Add("a4", "at2 yAD4 xAD4");
        shapeFormula.Add("dxF4", "sin lFD a4");
        shapeFormula.Add("dyF4", "cos lFD a4");
        shapeFormula.Add("xF4", "+- xD4 dxF4 0");
        shapeFormula.Add("yF4", "+- yD4 dyF4 0");
        shapeFormula.Add("xE4", "+- xA4 0 dxF4");
        shapeFormula.Add("yE4", "+- yA4 0 dyF4");
        shapeFormula.Add("yC4t", "sin th a4");
        shapeFormula.Add("xC4t", "cos th a4");
        shapeFormula.Add("yC4", "+- yF4 yC4t 0");
        shapeFormula.Add("xC4", "+- xF4 0 xC4t");
        shapeFormula.Add("yB4", "+- yE4 yC4t 0");
        shapeFormula.Add("xB4", "+- xE4 0 xC4t");
        shapeFormula.Add("swAng3", "+- bA4 0 bD3");
        shapeFormula.Add("aA5", "+- 6600000 0 ha");
        shapeFormula.Add("aD5", "+- 6600000 ha 0");
        shapeFormula.Add("ta51", "cos rw aA5");
        shapeFormula.Add("ta52", "sin rh aA5");
        shapeFormula.Add("bA5", "at2 ta51 ta52");
        shapeFormula.Add("td51", "cos rw aD5");
        shapeFormula.Add("td52", "sin rh aD5");
        shapeFormula.Add("bD5", "at2 td51 td52");
        shapeFormula.Add("xD5", "+- w 0 xA4");
        shapeFormula.Add("xC5", "+- w 0 xB4");
        shapeFormula.Add("xB5", "+- w 0 xC4");
        shapeFormula.Add("swAng4", "+- bA5 0 bD4");
        shapeFormula.Add("aD6", "+- 9000000 ha 0");
        shapeFormula.Add("td61", "cos rw aD6");
        shapeFormula.Add("td62", "sin rh aD6");
        shapeFormula.Add("bD6", "at2 td61 td62");
        shapeFormula.Add("xD6", "+- w 0 xA3");
        shapeFormula.Add("xC6", "+- w 0 xB3");
        shapeFormula.Add("xB6", "+- w 0 xC3");
        shapeFormula.Add("aD7", "+- 11400000 ha 0");
        shapeFormula.Add("td71", "cos rw aD7");
        shapeFormula.Add("td72", "sin rh aD7");
        shapeFormula.Add("bD7", "at2 td71 td72");
        shapeFormula.Add("xD7", "+- w 0 xA2");
        shapeFormula.Add("xC7", "+- w 0 xB2");
        shapeFormula.Add("xB7", "+- w 0 xC2");
        shapeFormula.Add("aD8", "+- 13800000 ha 0");
        shapeFormula.Add("td81", "cos rw aD8");
        shapeFormula.Add("td82", "sin rh aD8");
        shapeFormula.Add("bD8", "at2 td81 td82");
        shapeFormula.Add("xA8", "+- w 0 xD1");
        shapeFormula.Add("xD8", "+- w 0 xA1");
        shapeFormula.Add("xC8", "+- w 0 xB1");
        shapeFormula.Add("xB8", "+- w 0 xC1");
        shapeFormula.Add("aA9", "+- 3cd4 0 ha");
        shapeFormula.Add("aD9", "+- 3cd4 ha 0");
        shapeFormula.Add("td91", "cos rw aD9");
        shapeFormula.Add("td92", "sin rh aD9");
        shapeFormula.Add("bD9", "at2 td91 td92");
        shapeFormula.Add("ctd9", "cos rh bD9");
        shapeFormula.Add("std9", "sin rw bD9");
        shapeFormula.Add("md9", "mod ctd9 std9 0");
        shapeFormula.Add("nd9", "*/ rw rh md9");
        shapeFormula.Add("dxd9", "cos nd9 bD9");
        shapeFormula.Add("dyd9", "sin nd9 bD9");
        shapeFormula.Add("xD9", "+- hc dxd9 0");
        shapeFormula.Add("yD9", "+- vc dyd9 0");
        shapeFormula.Add("ta91", "cos rw aA9");
        shapeFormula.Add("ta92", "sin rh aA9");
        shapeFormula.Add("bA9", "at2 ta91 ta92");
        shapeFormula.Add("xA9", "+- hc 0 dxd9");
        shapeFormula.Add("xF9", "+- xD9 0 lFD");
        shapeFormula.Add("xE9", "+- xA9 lFD 0");
        shapeFormula.Add("yC9", "+- yD9 0 th");
        shapeFormula.Add("swAng5", "+- bA9 0 bD8");
        shapeFormula.Add("xCxn1", "+/ xB1 xC1 2");
        shapeFormula.Add("yCxn1", "+/ yB1 yC1 2");
        shapeFormula.Add("xCxn2", "+/ xB2 xC2 2");
        shapeFormula.Add("yCxn2", "+/ yB2 yC2 2");
        shapeFormula.Add("xCxn3", "+/ xB3 xC3 2");
        shapeFormula.Add("yCxn3", "+/ yB3 yC3 2");
        shapeFormula.Add("xCxn4", "+/ xB4 xC4 2");
        shapeFormula.Add("yCxn4", "+/ yB4 yC4 2");
        shapeFormula.Add("xCxn5", "+/ r 0 xCxn4");
        shapeFormula.Add("xCxn6", "+/ r 0 xCxn3");
        shapeFormula.Add("xCxn7", "+/ r 0 xCxn2");
        shapeFormula.Add("xCxn8", "+/ r 0 xCxn1");
        break;
      case AutoShapeType.Funnel:
        shapeFormula.Add("d", "*/ ss 1 20");
        shapeFormula.Add("rw2", "+- wd2 0 d");
        shapeFormula.Add("rh2", "+- hd4 0 d");
        shapeFormula.Add("t1", "cos wd2 480000");
        shapeFormula.Add("t2", "sin hd4 480000");
        shapeFormula.Add("da", "at2 t1 t2");
        shapeFormula.Add("2da", "*/ da 2 1");
        shapeFormula.Add("stAng1", "+- cd2 0 da");
        shapeFormula.Add("swAng1", "+- cd2 2da 0");
        shapeFormula.Add("swAng3", "+- cd2 0 2da");
        shapeFormula.Add("rw3", "*/ wd2 1 4");
        shapeFormula.Add("rh3", "*/ hd4 1 4");
        shapeFormula.Add("ct1", "cos hd4 stAng1");
        shapeFormula.Add("st1", "sin wd2 stAng1");
        shapeFormula.Add("m1", "mod ct1 st1 0");
        shapeFormula.Add("n1", "*/ wd2 hd4 m1");
        shapeFormula.Add("dx1", "cos n1 stAng1");
        shapeFormula.Add("dy1", "sin n1 stAng1");
        shapeFormula.Add("x1", "+- hc dx1 0");
        shapeFormula.Add("y1", "+- hd4 dy1 0");
        shapeFormula.Add("ct3", "cos rh3 da");
        shapeFormula.Add("st3", "sin rw3 da");
        shapeFormula.Add("m3", "mod ct3 st3 0");
        shapeFormula.Add("n3", "*/ rw3 rh3 m3");
        shapeFormula.Add("dx3", "cos n3 da");
        shapeFormula.Add("dy3", "sin n3 da");
        shapeFormula.Add("x3", "+- hc dx3 0");
        shapeFormula.Add("vc3", "+- b 0 rh3");
        shapeFormula.Add("y2", "+- vc3 dy3 0");
        shapeFormula.Add("x2", "+- wd2 0 rw2");
        shapeFormula.Add("cd", "*/ cd2 2 1");
        break;
      case AutoShapeType.LeftCircularArrow:
        shapeFormula.Add("a5", "pin 0 adj5 25000");
        shapeFormula.Add("maxAdj1", "*/ a5 2 1");
        shapeFormula.Add("a1", "pin 0 adj1 maxAdj1");
        shapeFormula.Add("enAng", "pin 1 adj3 21599999");
        shapeFormula.Add("stAng", "pin 0 adj4 21599999");
        shapeFormula.Add("th", "*/ ss a1 100000");
        shapeFormula.Add("thh", "*/ ss a5 100000");
        shapeFormula.Add("th2", "*/ th 1 2");
        shapeFormula.Add("rw1", "+- wd2 th2 thh");
        shapeFormula.Add("rh1", "+- hd2 th2 thh");
        shapeFormula.Add("rw2", "+- rw1 0 th");
        shapeFormula.Add("rh2", "+- rh1 0 th");
        shapeFormula.Add("rw3", "+- rw2 th2 0");
        shapeFormula.Add("rh3", "+- rh2 th2 0");
        shapeFormula.Add("wtH", "sin rw3 enAng");
        shapeFormula.Add("htH", "cos rh3 enAng");
        shapeFormula.Add("dxH", "cat2 rw3 htH wtH");
        shapeFormula.Add("dyH", "sat2 rh3 htH wtH");
        shapeFormula.Add("xH", "+- hc dxH 0");
        shapeFormula.Add("yH", "+- vc dyH 0");
        shapeFormula.Add("rI", "min rw2 rh2");
        shapeFormula.Add("u1", "*/ dxH dxH 1");
        shapeFormula.Add("u2", "*/ dyH dyH 1");
        shapeFormula.Add("u3", "*/ rI rI 1");
        shapeFormula.Add("u4", "+- u1 0 u3");
        shapeFormula.Add("u5", "+- u2 0 u3");
        shapeFormula.Add("u6", "*/ u4 u5 u1");
        shapeFormula.Add("u7", "*/ u6 1 u2");
        shapeFormula.Add("u8", "+- 1 0 u7");
        shapeFormula.Add("u9", "sqrt u8");
        shapeFormula.Add("u10", "*/ u4 1 dxH");
        shapeFormula.Add("u11", "*/ u10 1 dyH");
        shapeFormula.Add("u12", "+/ 1 u9 u11");
        shapeFormula.Add("u13", "at2 1 u12");
        shapeFormula.Add("u14", "+- u13 21600000 0");
        shapeFormula.Add("u15", "?: u13 u13 u14");
        shapeFormula.Add("u16", "+- u15 0 enAng");
        shapeFormula.Add("u17", "+- u16 21600000 0");
        shapeFormula.Add("u18", "?: u16 u16 u17");
        shapeFormula.Add("u19", "+- u18 0 cd2");
        shapeFormula.Add("u20", "+- u18 0 21600000");
        shapeFormula.Add("u21", "?: u19 u20 u18");
        shapeFormula.Add("u22", "abs u21");
        shapeFormula.Add("minAng", "*/ u22 -1 1");
        shapeFormula.Add("u23", "abs adj2");
        shapeFormula.Add("a2", "*/ u23 -1 1");
        shapeFormula.Add("aAng", "pin minAng a2 0");
        shapeFormula.Add("ptAng", "+- enAng aAng 0");
        shapeFormula.Add("wtA", "sin rw3 ptAng");
        shapeFormula.Add("htA", "cos rh3 ptAng");
        shapeFormula.Add("dxA", "cat2 rw3 htA wtA");
        shapeFormula.Add("dyA", "sat2 rh3 htA wtA");
        shapeFormula.Add("xA", "+- hc dxA 0");
        shapeFormula.Add("yA", "+- vc dyA 0");
        shapeFormula.Add("wtE", "sin rw1 stAng");
        shapeFormula.Add("htE", "cos rh1 stAng");
        shapeFormula.Add("dxE", "cat2 rw1 htE wtE");
        shapeFormula.Add("dyE", "sat2 rh1 htE wtE");
        shapeFormula.Add("xE", "+- hc dxE 0");
        shapeFormula.Add("yE", "+- vc dyE 0");
        shapeFormula.Add("wtD", "sin rw2 stAng");
        shapeFormula.Add("htD", "cos rh2 stAng");
        shapeFormula.Add("dxD", "cat2 rw2 htD wtD");
        shapeFormula.Add("dyD", "sat2 rh2 htD wtD");
        shapeFormula.Add("xD", "+- hc dxD 0");
        shapeFormula.Add("yD", "+- vc dyD 0");
        shapeFormula.Add("dxG", "cos thh ptAng");
        shapeFormula.Add("dyG", "sin thh ptAng");
        shapeFormula.Add("xG", "+- xH dxG 0");
        shapeFormula.Add("yG", "+- yH dyG 0");
        shapeFormula.Add("dxB", "cos thh ptAng");
        shapeFormula.Add("dyB", "sin thh ptAng");
        shapeFormula.Add("xB", "+- xH 0 dxB 0");
        shapeFormula.Add("yB", "+- yH 0 dyB 0");
        shapeFormula.Add("sx1", "+- xB 0 hc");
        shapeFormula.Add("sy1", "+- yB 0 vc");
        shapeFormula.Add("sx2", "+- xG 0 hc");
        shapeFormula.Add("sy2", "+- yG 0 vc");
        shapeFormula.Add("rO", "min rw1 rh1");
        shapeFormula.Add("x1O", "*/ sx1 rO rw1");
        shapeFormula.Add("y1O", "*/ sy1 rO rh1");
        shapeFormula.Add("x2O", "*/ sx2 rO rw1");
        shapeFormula.Add("y2O", "*/ sy2 rO rh1");
        shapeFormula.Add("dxO", "+- x2O 0 x1O");
        shapeFormula.Add("dyO", "+- y2O 0 y1O");
        shapeFormula.Add("dO", "mod dxO dyO 0");
        shapeFormula.Add("q1", "*/ x1O y2O 1");
        shapeFormula.Add("q2", "*/ x2O y1O 1");
        shapeFormula.Add("DO", "+- q1 0 q2");
        shapeFormula.Add("q3", "*/ rO rO 1");
        shapeFormula.Add("q4", "*/ dO dO 1");
        shapeFormula.Add("q5", "*/ q3 q4 1");
        shapeFormula.Add("q6", "*/ DO DO 1");
        shapeFormula.Add("q7", "+- q5 0 q6");
        shapeFormula.Add("q8", "max q7 0");
        shapeFormula.Add("sdelO", "sqrt q8");
        shapeFormula.Add("ndyO", "*/ dyO -1 1");
        shapeFormula.Add("sdyO", "?: ndyO -1 1");
        shapeFormula.Add("q9", "*/ sdyO dxO 1");
        shapeFormula.Add("q10", "*/ q9 sdelO 1");
        shapeFormula.Add("q11", "*/ DO dyO 1");
        shapeFormula.Add("dxF1", "+/ q11 q10 q4");
        shapeFormula.Add("q12", "+- q11 0 q10");
        shapeFormula.Add("dxF2", "*/ q12 1 q4");
        shapeFormula.Add("adyO", "abs dyO");
        shapeFormula.Add("q13", "*/ adyO sdelO 1");
        shapeFormula.Add("q14", "*/ DO dxO -1");
        shapeFormula.Add("dyF1", "+/ q14 q13 q4");
        shapeFormula.Add("q15", "+- q14 0 q13");
        shapeFormula.Add("dyF2", "*/ q15 1 q4");
        shapeFormula.Add("q16", "+- x2O 0 dxF1");
        shapeFormula.Add("q17", "+- x2O 0 dxF2");
        shapeFormula.Add("q18", "+- y2O 0 dyF1");
        shapeFormula.Add("q19", "+- y2O 0 dyF2");
        shapeFormula.Add("q20", "mod q16 q18 0");
        shapeFormula.Add("q21", "mod q17 q19 0");
        shapeFormula.Add("q22", "+- q21 0 q20");
        shapeFormula.Add("dxF", "?: q22 dxF1 dxF2");
        shapeFormula.Add("dyF", "?: q22 dyF1 dyF2");
        shapeFormula.Add("sdxF", "*/ dxF rw1 rO");
        shapeFormula.Add("sdyF", "*/ dyF rh1 rO");
        shapeFormula.Add("xF", "+- hc sdxF 0");
        shapeFormula.Add("yF", "+- vc sdyF 0");
        shapeFormula.Add("x1I", "*/ sx1 rI rw2");
        shapeFormula.Add("y1I", "*/ sy1 rI rh2");
        shapeFormula.Add("x2I", "*/ sx2 rI rw2");
        shapeFormula.Add("y2I", "*/ sy2 rI rh2");
        shapeFormula.Add("dxI", "+- x2I 0 x1I");
        shapeFormula.Add("dyI", "+- y2I 0 y1I");
        shapeFormula.Add("dI", "mod dxI dyI 0");
        shapeFormula.Add("v1", "*/ x1I y2I 1");
        shapeFormula.Add("v2", "*/ x2I y1I 1");
        shapeFormula.Add("DI", "+- v1 0 v2");
        shapeFormula.Add("v3", "*/ rI rI 1");
        shapeFormula.Add("v4", "*/ dI dI 1");
        shapeFormula.Add("v5", "*/ v3 v4 1");
        shapeFormula.Add("v6", "*/ DI DI 1");
        shapeFormula.Add("v7", "+- v5 0 v6");
        shapeFormula.Add("v8", "max v7 0");
        shapeFormula.Add("sdelI", "sqrt v8");
        shapeFormula.Add("v9", "*/ sdyO dxI 1");
        shapeFormula.Add("v10", "*/ v9 sdelI 1");
        shapeFormula.Add("v11", "*/ DI dyI 1");
        shapeFormula.Add("dxC1", "+/ v11 v10 v4");
        shapeFormula.Add("v12", "+- v11 0 v10");
        shapeFormula.Add("dxC2", "*/ v12 1 v4");
        shapeFormula.Add("adyI", "abs dyI");
        shapeFormula.Add("v13", "*/ adyI sdelI 1");
        shapeFormula.Add("v14", "*/ DI dxI -1");
        shapeFormula.Add("dyC1", "+/ v14 v13 v4");
        shapeFormula.Add("v15", "+- v14 0 v13");
        shapeFormula.Add("dyC2", "*/ v15 1 v4");
        shapeFormula.Add("v16", "+- x1I 0 dxC1");
        shapeFormula.Add("v17", "+- x1I 0 dxC2");
        shapeFormula.Add("v18", "+- y1I 0 dyC1");
        shapeFormula.Add("v19", "+- y1I 0 dyC2");
        shapeFormula.Add("v20", "mod v16 v18 0");
        shapeFormula.Add("v21", "mod v17 v19 0");
        shapeFormula.Add("v22", "+- v21 0 v20");
        shapeFormula.Add("dxC", "?: v22 dxC1 dxC2");
        shapeFormula.Add("dyC", "?: v22 dyC1 dyC2");
        shapeFormula.Add("sdxC", "*/ dxC rw2 rI");
        shapeFormula.Add("sdyC", "*/ dyC rh2 rI");
        shapeFormula.Add("xC", "+- hc sdxC 0");
        shapeFormula.Add("yC", "+- vc sdyC 0");
        shapeFormula.Add("ist0", "at2 sdxC sdyC");
        shapeFormula.Add("ist1", "+- ist0 21600000 0");
        shapeFormula.Add("istAng0", "?: ist0 ist0 ist1");
        shapeFormula.Add("isw1", "+- stAng 0 istAng0");
        shapeFormula.Add("isw2", "+- isw1 21600000 0");
        shapeFormula.Add("iswAng0", "?: isw1 isw1 isw2");
        shapeFormula.Add("istAng", "+- istAng0 iswAng0 0");
        shapeFormula.Add("iswAng", "+- 0 0 iswAng0");
        shapeFormula.Add("p1", "+- xF 0 xC");
        shapeFormula.Add("p2", "+- yF 0 yC");
        shapeFormula.Add("p3", "mod p1 p2 0");
        shapeFormula.Add("p4", "*/ p3 1 2");
        shapeFormula.Add("p5", "+- p4 0 thh");
        shapeFormula.Add("xGp", "?: p5 xF xG");
        shapeFormula.Add("yGp", "?: p5 yF yG");
        shapeFormula.Add("xBp", "?: p5 xC xB");
        shapeFormula.Add("yBp", "?: p5 yC yB");
        shapeFormula.Add("en0", "at2 sdxF sdyF");
        shapeFormula.Add("en1", "+- en0 21600000 0");
        shapeFormula.Add("en2", "?: en0 en0 en1");
        shapeFormula.Add("sw0", "+- en2 0 stAng");
        shapeFormula.Add("sw1", "+- sw0 0 21600000");
        shapeFormula.Add("swAng", "?: sw0 sw1 sw0");
        shapeFormula.Add("stAng0", "+- stAng swAng 0");
        shapeFormula.Add("swAng0", "+- 0 0 swAng");
        shapeFormula.Add("wtI", "sin rw3 stAng");
        shapeFormula.Add("htI", "cos rh3 stAng");
        shapeFormula.Add("dxI1", "cat2 rw3 htI wtI");
        shapeFormula.Add("dyI1", "sat2 rh3 htI wtI");
        shapeFormula.Add("xI", "+- hc dxI1 0");
        shapeFormula.Add("yI", "+- vc dyI1 0");
        shapeFormula.Add("aI", "+- stAng cd4 0");
        shapeFormula.Add("aA", "+- ptAng 0 cd4");
        shapeFormula.Add("aB", "+- ptAng cd2 0");
        shapeFormula.Add("idx", "cos rw1 2700000");
        shapeFormula.Add("idy", "sin rh1 2700000");
        shapeFormula.Add("il", "+- hc 0 idx");
        shapeFormula.Add("ir", "+- hc idx 0");
        shapeFormula.Add("it", "+- vc 0 idy");
        shapeFormula.Add("ib", "+- vc idy 0");
        break;
      case AutoShapeType.Cloud:
        shapeFormula.Add("il", "*/ w 2977 21600");
        shapeFormula.Add("it", "*/ h 3262 21600");
        shapeFormula.Add("ir", "*/ w 17087 21600");
        shapeFormula.Add("ib", "*/ h 17337 21600");
        shapeFormula.Add("g27", "*/ w 67 21600");
        shapeFormula.Add("g28", "*/ h 21577 21600");
        shapeFormula.Add("g29", "*/ w 21582 21600");
        shapeFormula.Add("g30", "*/ h 1235 21600");
        break;
      case AutoShapeType.SwooshArrow:
        shapeFormula.Add("a1", "pin 1 adj1 75000");
        shapeFormula.Add("maxAdj2", "*/ 70000 w ss");
        shapeFormula.Add("a2", "pin 0 adj2 maxAdj2");
        shapeFormula.Add("ad1", "*/ h a1 100000");
        shapeFormula.Add("ad2", "*/ ss a2 100000");
        shapeFormula.Add("xB", "+- r 0 ad2");
        shapeFormula.Add("yB", "+- t ssd8 0");
        shapeFormula.Add("alfa", "*/ cd4 1 14");
        shapeFormula.Add("dx0", "tan ssd8 alfa");
        shapeFormula.Add("xC", "+- xB 0 dx0");
        shapeFormula.Add("dx1", "tan ad1 alfa");
        shapeFormula.Add("yF", "+- yB ad1 0");
        shapeFormula.Add("xF", "+- xB dx1 0");
        shapeFormula.Add("xE", "+- xF dx0 0");
        shapeFormula.Add("yE", "+- yF ssd8 0");
        shapeFormula.Add("dy2", "+- yE 0 t");
        shapeFormula.Add("dy22", "*/ dy2 1 2");
        shapeFormula.Add("dy3", "*/ h 1 20");
        shapeFormula.Add("yD", "+- t dy22 dy3");
        shapeFormula.Add("dy4", "*/ hd6 1 1");
        shapeFormula.Add("yP1", "+- hd6 dy4 0");
        shapeFormula.Add("xP1", "val wd6");
        shapeFormula.Add("dy5", "*/ hd6 1 2");
        shapeFormula.Add("yP2", "+- yF dy5 0");
        shapeFormula.Add("xP2", "val wd4");
        break;
      case AutoShapeType.ElbowConnector:
        shapeFormula.Add("x1", "*/ w adj1 100000");
        break;
      case AutoShapeType.CurvedConnector:
        shapeFormula.Add("x2", "*/ w adj1 100000");
        shapeFormula.Add("x1", "+/ l x2 2");
        shapeFormula.Add("x3", "+/ r x2 2");
        shapeFormula.Add("y3", "*/ h 3 4");
        break;
      case AutoShapeType.BentConnector4:
        shapeFormula.Add("x1", "*/ w adj1 100000");
        shapeFormula.Add("x2", "+/ x1 r 2");
        shapeFormula.Add("y2", "*/ h adj2 100000");
        shapeFormula.Add("y1", "+/ t y2 2");
        break;
      case AutoShapeType.BentConnector5:
        shapeFormula.Add("x1", "*/ w adj1 100000");
        shapeFormula.Add("x3", "*/ w adj3 100000");
        shapeFormula.Add("x2", "+/ x1 x3 2");
        shapeFormula.Add("y2", "*/ h adj2 100000");
        shapeFormula.Add("y1", "+/ t y2 2");
        shapeFormula.Add("y3", "+/ b y2 2");
        break;
      case AutoShapeType.CurvedConnector4:
        shapeFormula.Add("x2", "*/ w adj1 100000");
        shapeFormula.Add("x1", "+/ l x2 2");
        shapeFormula.Add("x3", "+/ r x2 2");
        shapeFormula.Add("x4", "+/ x2 x3 2");
        shapeFormula.Add("x5", "+/ x3 r 2");
        shapeFormula.Add("y4", "*/ h adj2 100000");
        shapeFormula.Add("y1", "+/ t y4 2");
        shapeFormula.Add("y2", "+/ t y1 2");
        shapeFormula.Add("y3", "+/ y1 y4 2");
        shapeFormula.Add("y5", "+/ b y4 2");
        break;
      case AutoShapeType.CurvedConnector5:
        shapeFormula.Add("x3", "*/ w adj1 100000");
        shapeFormula.Add("x6", "*/ w adj3 100000");
        shapeFormula.Add("x1", "+/ x3 x6 2");
        shapeFormula.Add("x2", "+/ l x3 2");
        shapeFormula.Add("x4", "+/ x3 x1 2");
        shapeFormula.Add("x5", "+/ x6 x1 2");
        shapeFormula.Add("x7", "+/ x6 r 2");
        shapeFormula.Add("y4", "*/ h adj2 100000");
        shapeFormula.Add("y1", "+/ t y4 2");
        shapeFormula.Add("y2", "+/ t y1 2");
        shapeFormula.Add("y3", "+/ y1 y4 2");
        shapeFormula.Add("y5", "+/ b y4 2");
        shapeFormula.Add("y6", "+/ y5 y4 2");
        shapeFormula.Add("y7", "+/ y5 b 2");
        break;
    }
    return shapeFormula;
  }

  private Dictionary<string, float> GetDefaultPathAdjValues(AutoShapeType shapeType)
  {
    Dictionary<string, float> defaultPathAdjValues = new Dictionary<string, float>();
    switch (shapeType)
    {
      case AutoShapeType.Parallelogram:
      case AutoShapeType.Trapezoid:
        defaultPathAdjValues.Add("adj", 25000f);
        break;
      case AutoShapeType.RoundedRectangle:
      case AutoShapeType.FoldedCorner:
      case AutoShapeType.DoubleBracket:
      case AutoShapeType.Plaque:
      case AutoShapeType.RoundSingleCornerRectangle:
      case AutoShapeType.SnipSingleCornerRectangle:
        defaultPathAdjValues.Add("adj", 16667f);
        break;
      case AutoShapeType.Octagon:
        defaultPathAdjValues.Add("adj", 29289f);
        break;
      case AutoShapeType.IsoscelesTriangle:
      case AutoShapeType.Moon:
      case AutoShapeType.Pentagon:
      case AutoShapeType.Chevron:
      case AutoShapeType.DiagonalStripe:
        defaultPathAdjValues.Add("adj", 50000f);
        break;
      case AutoShapeType.Hexagon:
        defaultPathAdjValues.Add("adj", 25000f);
        defaultPathAdjValues.Add("vf", 115470f);
        break;
      case AutoShapeType.Cross:
      case AutoShapeType.Can:
      case AutoShapeType.Cube:
      case AutoShapeType.Donut:
      case AutoShapeType.Sun:
        defaultPathAdjValues.Add("adj", 25000f);
        break;
      case AutoShapeType.RegularPentagon:
        defaultPathAdjValues.Add("hf", 105146f);
        defaultPathAdjValues.Add("vf", 110557f);
        break;
      case AutoShapeType.Bevel:
      case AutoShapeType.Star4Point:
      case AutoShapeType.VerticalScroll:
      case AutoShapeType.HorizontalScroll:
        defaultPathAdjValues.Add("adj", 12500f);
        break;
      case AutoShapeType.SmileyFace:
        defaultPathAdjValues.Add("adj", 4653f);
        break;
      case AutoShapeType.NoSymbol:
        defaultPathAdjValues.Add("adj", 18750f);
        break;
      case AutoShapeType.BlockArc:
        defaultPathAdjValues.Add("adj1", 1.08E+07f);
        defaultPathAdjValues.Add("adj2", 0.0f);
        defaultPathAdjValues.Add("adj3", 25000f);
        break;
      case AutoShapeType.Arc:
        defaultPathAdjValues.Add("adj1", 1.62E+07f);
        defaultPathAdjValues.Add("adj2", 0.0f);
        break;
      case AutoShapeType.DoubleBrace:
      case AutoShapeType.LeftBracket:
      case AutoShapeType.RightBracket:
        defaultPathAdjValues.Add("adj", 8333f);
        break;
      case AutoShapeType.LeftBrace:
      case AutoShapeType.RightBrace:
        defaultPathAdjValues.Add("adj1", 8333f);
        defaultPathAdjValues.Add("adj2", 50000f);
        break;
      case AutoShapeType.RightArrow:
      case AutoShapeType.LeftArrow:
      case AutoShapeType.UpArrow:
      case AutoShapeType.DownArrow:
      case AutoShapeType.LeftRightArrow:
      case AutoShapeType.UpDownArrow:
      case AutoShapeType.StripedRightArrow:
      case AutoShapeType.NotchedRightArrow:
      case AutoShapeType.Corner:
      case AutoShapeType.BentConnector4:
      case AutoShapeType.CurvedConnector4:
        defaultPathAdjValues.Add("adj1", 50000f);
        defaultPathAdjValues.Add("adj2", 50000f);
        break;
      case AutoShapeType.QuadArrow:
      case AutoShapeType.LeftRightUpArrow:
        defaultPathAdjValues.Add("adj1", 22500f);
        defaultPathAdjValues.Add("adj2", 22500f);
        defaultPathAdjValues.Add("adj3", 22500f);
        break;
      case AutoShapeType.BentArrow:
        defaultPathAdjValues.Add("adj1", 25000f);
        defaultPathAdjValues.Add("adj2", 25000f);
        defaultPathAdjValues.Add("adj3", 25000f);
        defaultPathAdjValues.Add("adj4", 43750f);
        break;
      case AutoShapeType.UTurnArrow:
        defaultPathAdjValues.Add("adj1", 25000f);
        defaultPathAdjValues.Add("adj2", 25000f);
        defaultPathAdjValues.Add("adj3", 25000f);
        defaultPathAdjValues.Add("adj4", 43750f);
        defaultPathAdjValues.Add("adj5", 75000f);
        break;
      case AutoShapeType.LeftUpArrow:
      case AutoShapeType.BentUpArrow:
        defaultPathAdjValues.Add("adj1", 25000f);
        defaultPathAdjValues.Add("adj2", 25000f);
        defaultPathAdjValues.Add("adj3", 25000f);
        break;
      case AutoShapeType.CurvedRightArrow:
      case AutoShapeType.CurvedLeftArrow:
      case AutoShapeType.CurvedUpArrow:
      case AutoShapeType.CurvedDownArrow:
        defaultPathAdjValues.Add("adj1", 25000f);
        defaultPathAdjValues.Add("adj2", 50000f);
        defaultPathAdjValues.Add("adj3", 25000f);
        break;
      case AutoShapeType.RightArrowCallout:
      case AutoShapeType.LeftArrowCallout:
      case AutoShapeType.UpArrowCallout:
      case AutoShapeType.DownArrowCallout:
        defaultPathAdjValues.Add("adj1", 25000f);
        defaultPathAdjValues.Add("adj2", 25000f);
        defaultPathAdjValues.Add("adj3", 25000f);
        defaultPathAdjValues.Add("adj4", 64977f);
        break;
      case AutoShapeType.LeftRightArrowCallout:
      case AutoShapeType.UpDownArrowCallout:
        defaultPathAdjValues.Add("adj1", 25000f);
        defaultPathAdjValues.Add("adj2", 25000f);
        defaultPathAdjValues.Add("adj3", 25000f);
        defaultPathAdjValues.Add("adj4", 48123f);
        break;
      case AutoShapeType.QuadArrowCallout:
        defaultPathAdjValues.Add("adj1", 18515f);
        defaultPathAdjValues.Add("adj2", 18515f);
        defaultPathAdjValues.Add("adj3", 18515f);
        defaultPathAdjValues.Add("adj4", 48123f);
        break;
      case AutoShapeType.CircularArrow:
        defaultPathAdjValues.Add("adj1", 12500f);
        defaultPathAdjValues.Add("adj2", 19f);
        defaultPathAdjValues.Add("adj3", 341f);
        defaultPathAdjValues.Add("adj4", 180f);
        defaultPathAdjValues.Add("adj5", 12500f);
        break;
      case AutoShapeType.Star5Point:
        defaultPathAdjValues.Add("adj", 19098f);
        defaultPathAdjValues.Add("hf", 105146f);
        defaultPathAdjValues.Add("vf", 110557f);
        break;
      case AutoShapeType.Star8Point:
      case AutoShapeType.Star16Point:
      case AutoShapeType.Star24Point:
      case AutoShapeType.Star32Point:
      case AutoShapeType.Star12Point:
        defaultPathAdjValues.Add("adj", 37500f);
        break;
      case AutoShapeType.UpRibbon:
      case AutoShapeType.DownRibbon:
        defaultPathAdjValues.Add("adj1", 16667f);
        defaultPathAdjValues.Add("adj2", 50000f);
        break;
      case AutoShapeType.CurvedUpRibbon:
      case AutoShapeType.CurvedDownRibbon:
        defaultPathAdjValues.Add("adj1", 25000f);
        defaultPathAdjValues.Add("adj2", 50000f);
        defaultPathAdjValues.Add("adj3", 12500f);
        break;
      case AutoShapeType.Wave:
        defaultPathAdjValues.Add("adj1", 12500f);
        defaultPathAdjValues.Add("adj2", 0.0f);
        break;
      case AutoShapeType.DoubleWave:
        defaultPathAdjValues.Add("adj1", 6250f);
        defaultPathAdjValues.Add("adj2", 0.0f);
        break;
      case AutoShapeType.RectangularCallout:
      case AutoShapeType.OvalCallout:
      case AutoShapeType.CloudCallout:
        defaultPathAdjValues.Add("adj1", -20833f);
        defaultPathAdjValues.Add("adj2", 62500f);
        break;
      case AutoShapeType.RoundedRectangularCallout:
        defaultPathAdjValues.Add("adj1", -20833f);
        defaultPathAdjValues.Add("adj2", 62500f);
        defaultPathAdjValues.Add("adj3", 16667f);
        break;
      case AutoShapeType.LineCallout1:
      case AutoShapeType.LineCallout1NoBorder:
      case AutoShapeType.LineCallout1AccentBar:
      case AutoShapeType.LineCallout1BorderAndAccentBar:
        defaultPathAdjValues.Add("adj1", 18750f);
        defaultPathAdjValues.Add("adj2", -8333f);
        defaultPathAdjValues.Add("adj3", 112500f);
        defaultPathAdjValues.Add("adj4", -38333f);
        break;
      case AutoShapeType.LineCallout2:
      case AutoShapeType.LineCallout2AccentBar:
      case AutoShapeType.LineCallout2NoBorder:
      case AutoShapeType.LineCallout2BorderAndAccentBar:
        defaultPathAdjValues.Add("adj1", 18750f);
        defaultPathAdjValues.Add("adj2", -8333f);
        defaultPathAdjValues.Add("adj3", 18750f);
        defaultPathAdjValues.Add("adj4", -16667f);
        defaultPathAdjValues.Add("adj5", 112500f);
        defaultPathAdjValues.Add("adj6", -46667f);
        break;
      case AutoShapeType.LineCallout3:
      case AutoShapeType.LineCallout3AccentBar:
      case AutoShapeType.LineCallout3NoBorder:
      case AutoShapeType.LineCallout3BorderAndAccentBar:
        defaultPathAdjValues.Add("adj1", 18750f);
        defaultPathAdjValues.Add("adj2", -8333f);
        defaultPathAdjValues.Add("adj3", 18750f);
        defaultPathAdjValues.Add("adj4", -16667f);
        defaultPathAdjValues.Add("adj5", 100000f);
        defaultPathAdjValues.Add("adj6", -16667f);
        defaultPathAdjValues.Add("adj7", 112963f);
        defaultPathAdjValues.Add("adj8", -8333f);
        break;
      case AutoShapeType.LeftRightRibbon:
        defaultPathAdjValues.Add("adj1", 50000f);
        defaultPathAdjValues.Add("adj2", 50000f);
        defaultPathAdjValues.Add("adj3", 16667f);
        break;
      case AutoShapeType.Pie:
        defaultPathAdjValues.Add("adj1", 0.0f);
        defaultPathAdjValues.Add("adj2", 1.62E+07f);
        break;
      case AutoShapeType.Decagon:
        defaultPathAdjValues.Add("vf", 105146f);
        break;
      case AutoShapeType.Heptagon:
        defaultPathAdjValues.Add("hf", 102572f);
        defaultPathAdjValues.Add("vf", 105210f);
        break;
      case AutoShapeType.Star6Point:
        defaultPathAdjValues.Add("adj", 28868f);
        defaultPathAdjValues.Add("hf", 115470f);
        break;
      case AutoShapeType.Star7Point:
        defaultPathAdjValues.Add("adj", 34601f);
        defaultPathAdjValues.Add("hf", 102572f);
        defaultPathAdjValues.Add("vf", 105210f);
        break;
      case AutoShapeType.Star10Point:
        defaultPathAdjValues.Add("adj", 42533f);
        defaultPathAdjValues.Add("hf", 105146f);
        break;
      case AutoShapeType.RoundSameSideCornerRectangle:
      case AutoShapeType.RoundDiagonalCornerRectangle:
      case AutoShapeType.SnipSameSideCornerRectangle:
        defaultPathAdjValues.Add("adj1", 16667f);
        defaultPathAdjValues.Add("adj2", 0.0f);
        break;
      case AutoShapeType.SnipAndRoundSingleCornerRectangle:
        defaultPathAdjValues.Add("adj1", 16667f);
        defaultPathAdjValues.Add("adj2", 16667f);
        break;
      case AutoShapeType.SnipDiagonalCornerRectangle:
        defaultPathAdjValues.Add("adj2", 16667f);
        defaultPathAdjValues.Add("adj1", 0.0f);
        break;
      case AutoShapeType.Frame:
        defaultPathAdjValues.Add("adj1", 12500f);
        break;
      case AutoShapeType.HalfFrame:
        defaultPathAdjValues.Add("adj1", 33333f);
        defaultPathAdjValues.Add("adj2", 33333f);
        break;
      case AutoShapeType.Teardrop:
        defaultPathAdjValues.Add("adj", 100000f);
        break;
      case AutoShapeType.Chord:
        defaultPathAdjValues.Add("adj1", 2700000f);
        defaultPathAdjValues.Add("adj2", 1.62E+07f);
        break;
      case AutoShapeType.MathPlus:
      case AutoShapeType.MathMinus:
      case AutoShapeType.MathMultiply:
        defaultPathAdjValues.Add("adj1", 23520f);
        break;
      case AutoShapeType.MathDivision:
        defaultPathAdjValues.Add("adj1", 23520f);
        defaultPathAdjValues.Add("adj2", 5880f);
        defaultPathAdjValues.Add("adj3", 11760f);
        break;
      case AutoShapeType.MathEqual:
        defaultPathAdjValues.Add("adj1", 23520f);
        defaultPathAdjValues.Add("adj2", 11760f);
        break;
      case AutoShapeType.MathNotEqual:
        defaultPathAdjValues.Add("adj1", 23520f);
        defaultPathAdjValues.Add("adj2", 6600000f);
        defaultPathAdjValues.Add("adj3", 11760f);
        break;
      case AutoShapeType.Gear6:
        defaultPathAdjValues.Add("adj1", 15000f);
        defaultPathAdjValues.Add("adj2", 3526f);
        break;
      case AutoShapeType.Gear9:
        defaultPathAdjValues.Add("adj1", 10000f);
        defaultPathAdjValues.Add("adj2", 1763f);
        break;
      case AutoShapeType.LeftCircularArrow:
        defaultPathAdjValues.Add("adj1", 12500f);
        defaultPathAdjValues.Add("adj2", -1142319f);
        defaultPathAdjValues.Add("adj3", 1142319f);
        defaultPathAdjValues.Add("adj4", 1.08E+07f);
        defaultPathAdjValues.Add("adj5", 12500f);
        break;
      case AutoShapeType.SwooshArrow:
        defaultPathAdjValues.Add("adj1", 25000f);
        defaultPathAdjValues.Add("adj2", 16667f);
        break;
      case AutoShapeType.ElbowConnector:
      case AutoShapeType.CurvedConnector:
        defaultPathAdjValues.Add("adj1", 50000f);
        break;
      case AutoShapeType.BentConnector5:
      case AutoShapeType.CurvedConnector5:
        defaultPathAdjValues.Add("adj1", 50000f);
        defaultPathAdjValues.Add("adj2", 50000f);
        defaultPathAdjValues.Add("adj3", 50000f);
        break;
    }
    return defaultPathAdjValues;
  }

  internal void Close()
  {
    if (this._shapeGuide == null)
      return;
    this._shapeGuide.Clear();
    this._shapeGuide = (Dictionary<string, string>) null;
  }
}
