// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontHHCurveTo
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontHHCurveTo : SystemFontOperator
{
  public override void Execute(SystemFontBuildChar interpreter)
  {
    SystemFontOperandsCollection operands = interpreter.Operands;
    int dya = 0;
    if (operands.Count % 2 != 0)
      dya = operands.GetFirstAsInt();
    while (operands.Count / 4 > 0)
    {
      int firstAsInt1 = operands.GetFirstAsInt();
      int firstAsInt2 = operands.GetFirstAsInt();
      int firstAsInt3 = operands.GetFirstAsInt();
      int firstAsInt4 = operands.GetFirstAsInt();
      SystemFontOperator.CurveTo(interpreter, firstAsInt1, dya, firstAsInt2, firstAsInt3, firstAsInt4, 0);
      dya = 0;
    }
    operands.Clear();
  }
}
