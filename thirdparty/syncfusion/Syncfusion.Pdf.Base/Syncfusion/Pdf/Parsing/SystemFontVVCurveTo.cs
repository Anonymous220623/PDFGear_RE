// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontVVCurveTo
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontVVCurveTo : SystemFontOperator
{
  public override void Execute(SystemFontBuildChar interpreter)
  {
    SystemFontOperandsCollection operands = interpreter.Operands;
    int dxa = 0;
    if (operands.Count % 2 != 0)
      dxa = operands.GetFirstAsInt();
    int firstAsInt1 = operands.GetFirstAsInt();
    int firstAsInt2 = operands.GetFirstAsInt();
    int firstAsInt3 = operands.GetFirstAsInt();
    int firstAsInt4 = operands.GetFirstAsInt();
    SystemFontOperator.CurveTo(interpreter, dxa, firstAsInt1, firstAsInt2, firstAsInt3, 0, firstAsInt4);
    while (operands.Count / 4 > 0)
    {
      int firstAsInt5 = operands.GetFirstAsInt();
      int firstAsInt6 = operands.GetFirstAsInt();
      int firstAsInt7 = operands.GetFirstAsInt();
      int firstAsInt8 = operands.GetFirstAsInt();
      SystemFontOperator.CurveTo(interpreter, 0, firstAsInt5, firstAsInt6, firstAsInt7, 0, firstAsInt8);
    }
    operands.Clear();
  }
}
