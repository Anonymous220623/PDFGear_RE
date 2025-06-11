// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontVHCurveTo
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontVHCurveTo : SystemFontOperator
{
  public override void Execute(SystemFontBuildChar interpreter)
  {
    SystemFontOperandsCollection operands = interpreter.Operands;
    if (operands.Count % 8 != 0 && operands.Count % 8 != 1)
    {
      int firstAsInt1 = operands.GetFirstAsInt();
      int firstAsInt2 = operands.GetFirstAsInt();
      int firstAsInt3 = operands.GetFirstAsInt();
      int firstAsInt4 = operands.GetFirstAsInt();
      int firstAsInt5 = operands.Count == 1 ? operands.GetFirstAsInt() : 0;
      SystemFontOperator.CurveTo(interpreter, 0, firstAsInt1, firstAsInt2, firstAsInt3, firstAsInt4, firstAsInt5);
      while (operands.Count / 8 > 0)
      {
        int firstAsInt6 = operands.GetFirstAsInt();
        int firstAsInt7 = operands.GetFirstAsInt();
        int firstAsInt8 = operands.GetFirstAsInt();
        int firstAsInt9 = operands.GetFirstAsInt();
        int firstAsInt10 = operands.GetFirstAsInt();
        int firstAsInt11 = operands.GetFirstAsInt();
        int firstAsInt12 = operands.GetFirstAsInt();
        int firstAsInt13 = operands.GetFirstAsInt();
        int firstAsInt14 = operands.Count == 1 ? operands.GetFirstAsInt() : 0;
        SystemFontOperator.CurveTo(interpreter, firstAsInt6, 0, firstAsInt7, firstAsInt8, 0, firstAsInt9);
        SystemFontOperator.CurveTo(interpreter, 0, firstAsInt10, firstAsInt11, firstAsInt12, firstAsInt13, firstAsInt14);
      }
      operands.Clear();
    }
    else
    {
      while (operands.Count / 8 > 0)
      {
        int firstAsInt15 = operands.GetFirstAsInt();
        int firstAsInt16 = operands.GetFirstAsInt();
        int firstAsInt17 = operands.GetFirstAsInt();
        int firstAsInt18 = operands.GetFirstAsInt();
        int firstAsInt19 = operands.GetFirstAsInt();
        int firstAsInt20 = operands.GetFirstAsInt();
        int firstAsInt21 = operands.GetFirstAsInt();
        int firstAsInt22 = operands.GetFirstAsInt();
        int firstAsInt23 = operands.Count == 1 ? operands.GetFirstAsInt() : 0;
        SystemFontOperator.CurveTo(interpreter, 0, firstAsInt15, firstAsInt16, firstAsInt17, firstAsInt18, 0);
        SystemFontOperator.CurveTo(interpreter, firstAsInt19, 0, firstAsInt20, firstAsInt21, firstAsInt23, firstAsInt22);
      }
      operands.Clear();
    }
  }
}
