// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontReadString
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontReadString : SystemFontPostScriptOperator
{
  public override void Execute(SystemFontInterpreter interpreter)
  {
    SystemFontPostScriptString lastAs = interpreter.Operands.GetLastAs<SystemFontPostScriptString>();
    interpreter.Operands.GetLast();
    int num = 0;
    while (!interpreter.Reader.EndOfFile && num < lastAs.Capacity)
      lastAs[num++] = (char) interpreter.Reader.Read();
    interpreter.Operands.AddLast((object) lastAs);
    interpreter.Operands.AddLast((object) true);
  }
}
