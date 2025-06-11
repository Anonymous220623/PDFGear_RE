// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontHintOperator
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontHintOperator : SystemFontOperator
{
  private static void ReadWidth(SystemFontBuildChar interpreter)
  {
    if (interpreter.Width.HasValue)
      return;
    if (interpreter.Operands.Count % 2 == 1)
      interpreter.Width = new int?(interpreter.Operands.GetFirstAsInt());
    else
      interpreter.Width = new int?(0);
  }

  public override void Execute(SystemFontBuildChar interpreter)
  {
    SystemFontHintOperator.ReadWidth(interpreter);
    interpreter.Operands.Clear();
  }

  internal void Execute(SystemFontBuildChar interpreter, out int count)
  {
    count = interpreter.Operands.Count / 2;
    this.Execute(interpreter);
  }
}
