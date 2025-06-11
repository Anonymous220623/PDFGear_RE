// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontRRCurveTo
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontRRCurveTo : SystemFontOperator
{
  public override void Execute(SystemFontBuildChar interpreter)
  {
    SystemFontOperandsCollection operands = interpreter.Operands;
    while (operands.Count / 6 > 0)
    {
      int firstAsInt1 = operands.GetFirstAsInt();
      int firstAsInt2 = operands.GetFirstAsInt();
      int firstAsInt3 = operands.GetFirstAsInt();
      int firstAsInt4 = operands.GetFirstAsInt();
      int firstAsInt5 = operands.GetFirstAsInt();
      int firstAsInt6 = operands.GetFirstAsInt();
      SystemFontOperator.CurveTo(interpreter, firstAsInt1, firstAsInt2, firstAsInt3, firstAsInt4, firstAsInt5, firstAsInt6);
    }
    operands.Clear();
  }
}
