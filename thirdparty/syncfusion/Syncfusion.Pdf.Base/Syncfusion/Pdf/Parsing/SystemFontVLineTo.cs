// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontVLineTo
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontVLineTo : SystemFontOperator
{
  public override void Execute(SystemFontBuildChar interpreter)
  {
    if (interpreter.Operands.Count % 2 == 0)
    {
      while (interpreter.Operands.Count > 0)
      {
        int firstAsInt1 = interpreter.Operands.GetFirstAsInt();
        int firstAsInt2 = interpreter.Operands.GetFirstAsInt();
        SystemFontOperator.VLineTo(interpreter, firstAsInt1);
        SystemFontOperator.HLineTo(interpreter, firstAsInt2);
      }
    }
    else
    {
      int firstAsInt3 = interpreter.Operands.GetFirstAsInt();
      SystemFontOperator.VLineTo(interpreter, firstAsInt3);
      while (interpreter.Operands.Count > 0)
      {
        int firstAsInt4 = interpreter.Operands.GetFirstAsInt();
        int firstAsInt5 = interpreter.Operands.GetFirstAsInt();
        SystemFontOperator.HLineTo(interpreter, firstAsInt4);
        SystemFontOperator.VLineTo(interpreter, firstAsInt5);
      }
    }
    interpreter.Operands.Clear();
  }
}
