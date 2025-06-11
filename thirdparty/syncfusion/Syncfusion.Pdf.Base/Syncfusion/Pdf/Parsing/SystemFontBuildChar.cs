// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontBuildChar
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.PdfViewer.Base;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontBuildChar
{
  private static readonly Dictionary<SystemFontOperatorDescriptor, SystemFontOperator> operators = new Dictionary<SystemFontOperatorDescriptor, SystemFontOperator>();
  private static SystemFontOperatorDescriptor endChar;
  private static SystemFontHintOperator vStem;
  private readonly ISystemFontBuildCharHolder buildCharHolder;
  private SystemFontOperandsCollection postScriptStack;
  private SystemFontOperandsCollection operands;

  internal SystemFontOperandsCollection Operands => this.operands;

  internal SystemFontOperandsCollection PostScriptStack => this.postScriptStack;

  internal SystemFontPathFigure CurrentPathFigure { get; set; }

  internal SystemFontGlyphOutlinesCollection GlyphOutlines { get; private set; }

  internal Point CurrentPoint { get; set; }

  internal Point BottomLeft { get; set; }

  internal int? Width { get; set; }

  internal int Hints { get; set; }

  internal static Point CalculatePoint(SystemFontBuildChar interpreter, int dx, int dy)
  {
    interpreter.CurrentPoint = new Point(interpreter.CurrentPoint.X + (double) dx, interpreter.CurrentPoint.Y + (double) dy);
    return new Point(interpreter.CurrentPoint.X, -interpreter.CurrentPoint.Y);
  }

  private static void InitializePathConstructionOperators()
  {
    SystemFontBuildChar.operators[new SystemFontOperatorDescriptor((byte) 1)] = (SystemFontOperator) new SystemFontHintOperator();
    SystemFontBuildChar.vStem = new SystemFontHintOperator();
    SystemFontBuildChar.operators[new SystemFontOperatorDescriptor((byte) 3)] = (SystemFontOperator) SystemFontBuildChar.vStem;
    SystemFontBuildChar.operators[new SystemFontOperatorDescriptor((byte) 4)] = (SystemFontOperator) new SystemFontVMoveTo();
    SystemFontBuildChar.operators[new SystemFontOperatorDescriptor((byte) 5)] = (SystemFontOperator) new SystemFontRLineTo();
    SystemFontBuildChar.operators[new SystemFontOperatorDescriptor((byte) 6)] = (SystemFontOperator) new SystemFontHLineTo();
    SystemFontBuildChar.operators[new SystemFontOperatorDescriptor((byte) 7)] = (SystemFontOperator) new SystemFontVLineTo();
    SystemFontBuildChar.operators[new SystemFontOperatorDescriptor((byte) 8)] = (SystemFontOperator) new SystemFontRRCurveTo();
    SystemFontBuildChar.endChar = new SystemFontOperatorDescriptor((byte) 14);
    SystemFontBuildChar.operators[SystemFontBuildChar.endChar] = (SystemFontOperator) new SystemFontEndChar();
    SystemFontBuildChar.operators[new SystemFontOperatorDescriptor((byte) 18)] = (SystemFontOperator) new SystemFontHintOperator();
    SystemFontBuildChar.operators[new SystemFontOperatorDescriptor((byte) 19)] = (SystemFontOperator) new SystemFontHintMaskOperator();
    SystemFontBuildChar.operators[new SystemFontOperatorDescriptor((byte) 20)] = (SystemFontOperator) new SystemFontHintMaskOperator();
    SystemFontBuildChar.operators[new SystemFontOperatorDescriptor((byte) 21)] = (SystemFontOperator) new SystemFontRMoveTo();
    SystemFontBuildChar.operators[new SystemFontOperatorDescriptor((byte) 22)] = (SystemFontOperator) new SystemFontHMoveTo();
    SystemFontBuildChar.operators[new SystemFontOperatorDescriptor((byte) 23)] = (SystemFontOperator) new SystemFontHintOperator();
    SystemFontBuildChar.operators[new SystemFontOperatorDescriptor((byte) 24)] = (SystemFontOperator) new SystemFontRCurveLine();
    SystemFontBuildChar.operators[new SystemFontOperatorDescriptor((byte) 25)] = (SystemFontOperator) new SystemFontRLineCurve();
    SystemFontBuildChar.operators[new SystemFontOperatorDescriptor((byte) 26)] = (SystemFontOperator) new SystemFontVVCurveTo();
    SystemFontBuildChar.operators[new SystemFontOperatorDescriptor((byte) 27)] = (SystemFontOperator) new SystemFontHHCurveTo();
    SystemFontBuildChar.operators[new SystemFontOperatorDescriptor((byte) 30)] = (SystemFontOperator) new SystemFontVHCurveTo();
    SystemFontBuildChar.operators[new SystemFontOperatorDescriptor((byte) 31 /*0x1F*/)] = (SystemFontOperator) new SystemFontHVCurveTo();
    SystemFontBuildChar.operators[new SystemFontOperatorDescriptor((byte) 11)] = (SystemFontOperator) new SystemFontReturn();
    SystemFontBuildChar.operators[new SystemFontOperatorDescriptor((byte) 10)] = (SystemFontOperator) new SystemFontCallSubr();
    SystemFontBuildChar.operators[new SystemFontOperatorDescriptor((byte) 9)] = (SystemFontOperator) new SystemFontClosePath();
    SystemFontBuildChar.operators[new SystemFontOperatorDescriptor((byte) 13)] = (SystemFontOperator) new SystemFontHsbw();
    SystemFontBuildChar.operators[new SystemFontOperatorDescriptor((byte) 29)] = (SystemFontOperator) new SystemFontCallGSubr();
    SystemFontBuildChar.operators[new SystemFontOperatorDescriptor(SystemFontHelper.CreateByteArray((byte) 12, (byte) 12))] = (SystemFontOperator) new SystemFontDiv();
    SystemFontBuildChar.operators[new SystemFontOperatorDescriptor(SystemFontHelper.CreateByteArray((byte) 12, (byte) 35))] = (SystemFontOperator) new SystemFontFlex();
    SystemFontBuildChar.operators[new SystemFontOperatorDescriptor(SystemFontHelper.CreateByteArray((byte) 12, (byte) 37))] = (SystemFontOperator) new SystemFontFlex1();
    SystemFontBuildChar.operators[new SystemFontOperatorDescriptor(SystemFontHelper.CreateByteArray((byte) 12, (byte) 34))] = (SystemFontOperator) new SystemFontHFlex();
    SystemFontBuildChar.operators[new SystemFontOperatorDescriptor(SystemFontHelper.CreateByteArray((byte) 12, (byte) 36))] = (SystemFontOperator) new SystemFontHFlex1();
    SystemFontBuildChar.operators[new SystemFontOperatorDescriptor(SystemFontHelper.CreateByteArray((byte) 12, (byte) 16 /*0x10*/))] = (SystemFontOperator) new SystemFontCallOtherSubr();
    SystemFontBuildChar.operators[new SystemFontOperatorDescriptor(SystemFontHelper.CreateByteArray((byte) 12, (byte) 17))] = (SystemFontOperator) new SystemFontPop();
    SystemFontBuildChar.operators[new SystemFontOperatorDescriptor(SystemFontHelper.CreateByteArray((byte) 12, (byte) 6))] = (SystemFontOperator) new SystemFontSeac();
    SystemFontBuildChar.operators[new SystemFontOperatorDescriptor(SystemFontHelper.CreateByteArray((byte) 12, (byte) 7))] = (SystemFontOperator) new SystemFontSbw();
    SystemFontBuildChar.operators[new SystemFontOperatorDescriptor(SystemFontHelper.CreateByteArray((byte) 12, (byte) 33))] = (SystemFontOperator) new SystemFontSetCurrentPoint();
  }

  private static bool IsOperator(byte b)
  {
    return b != (byte) 28 && (byte) 0 <= b && b <= (byte) 31 /*0x1F*/;
  }

  private static bool IsTwoByteOperator(byte b) => b == (byte) 12;

  static SystemFontBuildChar() => SystemFontBuildChar.InitializePathConstructionOperators();

  public SystemFontBuildChar(ISystemFontBuildCharHolder subrsHodler)
  {
    this.buildCharHolder = subrsHodler;
  }

  public void ExecuteSubr(int index) => this.ExecuteInternal(this.buildCharHolder.GetSubr(index));

  public void ExecuteGlobalSubr(int index)
  {
    this.ExecuteInternal(this.buildCharHolder.GetGlobalSubr(index));
  }

  public void CombineChars(string accentedChar, string baseChar, int dx, int dy)
  {
    SystemFontGlyphOutlinesCollection collection = this.buildCharHolder.GetGlyphData(accentedChar).Oultlines.Clone();
    this.GlyphOutlines.AddRange((IEnumerable<SystemFontPathFigure>) this.buildCharHolder.GetGlyphData(baseChar).Oultlines.Clone());
    collection.Transform(new SystemFontMatrix(1.0, 0.0, 0.0, 1.0, (double) dx, (double) dy));
    this.GlyphOutlines.AddRange((IEnumerable<SystemFontPathFigure>) collection);
  }

  public void Execute(byte[] data)
  {
    this.postScriptStack = new SystemFontOperandsCollection();
    this.operands = new SystemFontOperandsCollection();
    this.GlyphOutlines = new SystemFontGlyphOutlinesCollection();
    this.CurrentPoint = new Point();
    this.Width = new int?();
    this.Hints = 0;
    this.ExecuteInternal(data);
  }

  private void ExecuteInternal(byte[] data)
  {
    SystemFontEncodedDataReader reader = new SystemFontEncodedDataReader(data, SystemFontByteEncoding.CharStringByteEncodings);
    while (!reader.EndOfFile)
    {
      byte b = reader.Peek(0);
      if (SystemFontBuildChar.IsOperator(b))
      {
        SystemFontOperatorDescriptor descr;
        if (SystemFontBuildChar.IsTwoByteOperator(b))
          descr = new SystemFontOperatorDescriptor(SystemFontHelper.CreateByteArray(reader.Read(), reader.Read()));
        else
          descr = new SystemFontOperatorDescriptor(reader.Read());
        this.ExecuteOperator(descr, reader);
        if (descr.Equals((object) SystemFontBuildChar.endChar))
          break;
      }
      else
        this.Operands.AddLast(reader.ReadOperand());
    }
  }

  private int GetMaskSize()
  {
    int maskSize = this.Hints / 8;
    if (this.Hints % 8 != 0)
      ++maskSize;
    return maskSize;
  }

  private void ExecuteOperator(
    SystemFontOperatorDescriptor descr,
    SystemFontEncodedDataReader reader)
  {
    SystemFontOperator systemFontOperator;
    if (!SystemFontBuildChar.operators.TryGetValue(descr, out systemFontOperator))
    {
      this.Operands.Clear();
    }
    else
    {
      switch (systemFontOperator)
      {
        case SystemFontHintMaskOperator _:
          int count1;
          SystemFontBuildChar.vStem.Execute(this, out count1);
          this.Hints += count1;
          byte[] numArray = new byte[this.GetMaskSize()];
          reader.Read(numArray, numArray.Length);
          ((SystemFontHintMaskOperator) systemFontOperator).Execute(this, numArray);
          break;
        case SystemFontHintOperator _:
          int count2;
          ((SystemFontHintOperator) systemFontOperator).Execute(this, out count2);
          this.Hints += count2;
          break;
        default:
          systemFontOperator.Execute(this);
          break;
      }
    }
  }
}
