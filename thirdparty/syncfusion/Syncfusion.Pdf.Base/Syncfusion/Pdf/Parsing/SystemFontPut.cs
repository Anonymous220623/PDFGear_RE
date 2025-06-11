// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontPut
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontPut : SystemFontPostScriptOperator
{
  public override void Execute(SystemFontInterpreter interpreter)
  {
    object last1 = interpreter.Operands.GetLast();
    object last2 = interpreter.Operands.GetLast();
    object last3 = interpreter.Operands.GetLast();
    switch (last3)
    {
      case SystemFontPostScriptArray _:
        SystemFontPostScriptArray fontPostScriptArray = (SystemFontPostScriptArray) last3;
        int res1;
        SystemFontHelper.UnboxInteger(last2, out res1);
        fontPostScriptArray[res1] = last1;
        break;
      case SystemFontPostScriptDictionary _:
        ((SystemFontPostScriptDictionary) last3)[(string) last2] = last1;
        break;
      case SystemFontPostScriptString _:
        SystemFontPostScriptString postScriptString = (SystemFontPostScriptString) last3;
        int res2;
        SystemFontHelper.UnboxInteger(last2, out res2);
        int res3;
        SystemFontHelper.UnboxInteger(last1, out res3);
        postScriptString[res2] = (char) res3;
        break;
    }
  }
}
