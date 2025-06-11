// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontDef
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontDef : SystemFontPostScriptOperator
{
  public override void Execute(SystemFontInterpreter interpreter)
  {
    object last = interpreter.Operands.GetLast();
    string lastAs = interpreter.Operands.GetLastAs<string>();
    interpreter.CurrentDictionary[lastAs] = last;
    if (SystemFontRD.IsRDOperator(lastAs))
      interpreter.RD = (SystemFontPostScriptArray) last;
    else if (SystemFontND.IsNDOperator(lastAs))
    {
      interpreter.ND = (SystemFontPostScriptArray) last;
    }
    else
    {
      if (!SystemFontNP.IsNPOperator(lastAs))
        return;
      interpreter.NP = (SystemFontPostScriptArray) last;
    }
  }
}
