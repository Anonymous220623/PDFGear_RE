// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.CharacterBuilder
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class CharacterBuilder
{
  private static readonly Dictionary<OperatorDescriptor, Operator> operators = new Dictionary<OperatorDescriptor, Operator>();
  private static OperatorDescriptor endChar;
  private static HintOperator vStem;
  private readonly IBuildCharacterOwner buildCharHolder;
  private OperandCollector postScriptStack;
  private OperandCollector operands;

  internal OperandCollector Operands => this.operands;

  internal OperandCollector PostScriptStack => this.postScriptStack;

  internal PathFigure CurrentPathFigure { get; set; }

  internal GlyphOutlinesCollection GlyphOutlines { get; private set; }

  internal Point CurrentPoint { get; set; }

  internal Point BottomLeft { get; set; }

  internal int? Width { get; set; }

  internal int Hints { get; set; }

  internal static Point CalculatePoint(CharacterBuilder interpreter, int dx, int dy)
  {
    interpreter.CurrentPoint = new Point(interpreter.CurrentPoint.X + (double) dx, interpreter.CurrentPoint.Y + (double) dy);
    return new Point(interpreter.CurrentPoint.X, -interpreter.CurrentPoint.Y);
  }

  private static void InitializePathConstructionOperators()
  {
    CharacterBuilder.operators[new OperatorDescriptor((byte) 1)] = (Operator) new HintOperator();
    CharacterBuilder.vStem = new HintOperator();
    CharacterBuilder.operators[new OperatorDescriptor((byte) 3)] = (Operator) CharacterBuilder.vStem;
    CharacterBuilder.operators[new OperatorDescriptor((byte) 4)] = (Operator) new VMoveTo();
    CharacterBuilder.operators[new OperatorDescriptor((byte) 5)] = (Operator) new RLineTo();
    CharacterBuilder.operators[new OperatorDescriptor((byte) 6)] = (Operator) new HLineTo();
    CharacterBuilder.operators[new OperatorDescriptor((byte) 7)] = (Operator) new VLineTo();
    CharacterBuilder.operators[new OperatorDescriptor((byte) 8)] = (Operator) new RRCurveTo();
    CharacterBuilder.endChar = new OperatorDescriptor((byte) 14);
    CharacterBuilder.operators[CharacterBuilder.endChar] = (Operator) new EndChar();
    CharacterBuilder.operators[new OperatorDescriptor((byte) 18)] = (Operator) new HintOperator();
    CharacterBuilder.operators[new OperatorDescriptor((byte) 19)] = (Operator) new HintMaskOperator();
    CharacterBuilder.operators[new OperatorDescriptor((byte) 20)] = (Operator) new HintMaskOperator();
    CharacterBuilder.operators[new OperatorDescriptor((byte) 21)] = (Operator) new RMoveTo();
    CharacterBuilder.operators[new OperatorDescriptor((byte) 22)] = (Operator) new HMoveTo();
    CharacterBuilder.operators[new OperatorDescriptor((byte) 23)] = (Operator) new HintOperator();
    CharacterBuilder.operators[new OperatorDescriptor((byte) 24)] = (Operator) new RCurveLine();
    CharacterBuilder.operators[new OperatorDescriptor((byte) 25)] = (Operator) new RLineCurve();
    CharacterBuilder.operators[new OperatorDescriptor((byte) 26)] = (Operator) new VVCurveTo();
    CharacterBuilder.operators[new OperatorDescriptor((byte) 27)] = (Operator) new HHCurveTo();
    CharacterBuilder.operators[new OperatorDescriptor((byte) 30)] = (Operator) new VHCurveTo();
    CharacterBuilder.operators[new OperatorDescriptor((byte) 31 /*0x1F*/)] = (Operator) new HVCurveTo();
    CharacterBuilder.operators[new OperatorDescriptor((byte) 11)] = (Operator) new Return();
    CharacterBuilder.operators[new OperatorDescriptor((byte) 10)] = (Operator) new CallSubr();
    CharacterBuilder.operators[new OperatorDescriptor((byte) 9)] = (Operator) new ClosePath();
    CharacterBuilder.operators[new OperatorDescriptor((byte) 13)] = (Operator) new Hsbw();
    CharacterBuilder.operators[new OperatorDescriptor((byte) 29)] = (Operator) new CallGSubr();
    CharacterBuilder.operators[new OperatorDescriptor(Helper.CreateByteArray((byte) 12, (byte) 12))] = (Operator) new Div();
    CharacterBuilder.operators[new OperatorDescriptor(Helper.CreateByteArray((byte) 12, (byte) 35))] = (Operator) new Flex();
    CharacterBuilder.operators[new OperatorDescriptor(Helper.CreateByteArray((byte) 12, (byte) 37))] = (Operator) new Flex1();
    CharacterBuilder.operators[new OperatorDescriptor(Helper.CreateByteArray((byte) 12, (byte) 34))] = (Operator) new HFlex();
    CharacterBuilder.operators[new OperatorDescriptor(Helper.CreateByteArray((byte) 12, (byte) 36))] = (Operator) new HFlex1();
    CharacterBuilder.operators[new OperatorDescriptor(Helper.CreateByteArray((byte) 12, (byte) 16 /*0x10*/))] = (Operator) new CallOtherSubr();
    CharacterBuilder.operators[new OperatorDescriptor(Helper.CreateByteArray((byte) 12, (byte) 17))] = (Operator) new Pop();
    CharacterBuilder.operators[new OperatorDescriptor(Helper.CreateByteArray((byte) 12, (byte) 6))] = (Operator) new Seac();
    CharacterBuilder.operators[new OperatorDescriptor(Helper.CreateByteArray((byte) 12, (byte) 7))] = (Operator) new Sbw();
    CharacterBuilder.operators[new OperatorDescriptor(Helper.CreateByteArray((byte) 12, (byte) 33))] = (Operator) new SetCurrentPoint();
  }

  private static bool IsOperator(byte b)
  {
    return b != (byte) 28 && (byte) 0 <= b && b <= (byte) 31 /*0x1F*/;
  }

  private static bool IsTwoByteOperator(byte b) => b == (byte) 12;

  static CharacterBuilder() => CharacterBuilder.InitializePathConstructionOperators();

  public CharacterBuilder(IBuildCharacterOwner subrsHodler) => this.buildCharHolder = subrsHodler;

  public void ExecuteSubr(int index) => this.ExecuteInternal(this.buildCharHolder.GetSubr(index));

  public void ExecuteGlobalSubr(int index)
  {
    this.ExecuteInternal(this.buildCharHolder.GetGlobalSubr(index));
  }

  public void CombineChars(string accentedChar, string baseChar, int dx, int dy)
  {
    GlyphOutlinesCollection collection = this.buildCharHolder.GetGlyphData(accentedChar).Oultlines.Clone();
    this.GlyphOutlines.AddRange((IEnumerable<PathFigure>) this.buildCharHolder.GetGlyphData(baseChar).Oultlines.Clone());
    collection.Transform(new Matrix(1.0, 0.0, 0.0, 1.0, (double) dx, (double) dy));
    this.GlyphOutlines.AddRange((IEnumerable<PathFigure>) collection);
  }

  public void Execute(byte[] data)
  {
    this.postScriptStack = new OperandCollector();
    this.operands = new OperandCollector();
    this.GlyphOutlines = new GlyphOutlinesCollection();
    this.CurrentPoint = new Point();
    this.Width = new int?();
    this.Hints = 0;
    this.ExecuteInternal(data);
  }

  private void ExecuteInternal(byte[] data)
  {
    EncodedDataParser reader = new EncodedDataParser(data, ByteEncodingBase.CharStringByteEncodings);
    while (!reader.EndOfFile)
    {
      byte b = reader.Peek(0);
      if (CharacterBuilder.IsOperator(b))
      {
        OperatorDescriptor descr;
        if (CharacterBuilder.IsTwoByteOperator(b))
          descr = new OperatorDescriptor(Helper.CreateByteArray(reader.Read(), reader.Read()));
        else
          descr = new OperatorDescriptor(reader.Read());
        this.ExecuteOperator(descr, reader);
        if (descr.Equals((object) CharacterBuilder.endChar))
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

  private void ExecuteOperator(OperatorDescriptor descr, EncodedDataParser reader)
  {
    Operator @operator;
    if (!CharacterBuilder.operators.TryGetValue(descr, out @operator))
    {
      this.Operands.Clear();
    }
    else
    {
      switch (@operator)
      {
        case HintMaskOperator _:
          int count1;
          CharacterBuilder.vStem.Execute(this, out count1);
          this.Hints += count1;
          byte[] numArray = new byte[this.GetMaskSize()];
          reader.Read(numArray, numArray.Length);
          ((HintMaskOperator) @operator).Execute(this, numArray);
          break;
        case HintOperator _:
          int count2;
          ((HintOperator) @operator).Execute(this, out count2);
          this.Hints += count2;
          break;
        default:
          @operator.Execute(this);
          break;
      }
    }
  }
}
