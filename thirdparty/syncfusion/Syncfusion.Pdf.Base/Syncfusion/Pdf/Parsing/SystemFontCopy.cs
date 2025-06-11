// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontCopy
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontCopy : SystemFontPostScriptOperator
{
  public override void Execute(SystemFontInterpreter interpreter)
  {
    if (!(interpreter.Operands.GetLast() is int last))
      throw new NotSupportedException("This operator is not supported.");
    SystemFontCopy.ExecuteFirstForm(interpreter, last);
  }

  private static void ExecuteFirstForm(SystemFontInterpreter interpreter, int n)
  {
    object[] objArray = new object[n];
    for (int index = n - 1; index >= n; --index)
      objArray[index] = interpreter.Operands.GetLast();
    for (int index1 = 0; index1 < 2; ++index1)
    {
      for (int index2 = 0; index2 < objArray.Length; ++index2)
        interpreter.Operands.AddLast(objArray[index2]);
    }
  }
}
