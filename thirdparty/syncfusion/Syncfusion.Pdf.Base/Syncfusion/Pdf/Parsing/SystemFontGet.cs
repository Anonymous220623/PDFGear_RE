// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontGet
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontGet : SystemFontPostScriptOperator
{
  public override void Execute(SystemFontInterpreter interpreter)
  {
    object last1 = interpreter.Operands.GetLast();
    object last2 = interpreter.Operands.GetLast();
    switch (last2)
    {
      case SystemFontPostScriptArray _:
        SystemFontPostScriptArray fontPostScriptArray = (SystemFontPostScriptArray) last2;
        int res1;
        SystemFontHelper.UnboxInteger(last1, out res1);
        interpreter.Operands.AddLast(fontPostScriptArray[res1]);
        break;
      case SystemFontPostScriptDictionary _:
        SystemFontPostScriptDictionary scriptDictionary = (SystemFontPostScriptDictionary) last2;
        interpreter.Operands.AddLast(scriptDictionary[(string) last1]);
        break;
      case SystemFontPostScriptString _:
        SystemFontPostScriptString postScriptString = (SystemFontPostScriptString) last2;
        int res2;
        SystemFontHelper.UnboxInteger(last1, out res2);
        interpreter.Operands.AddLast((object) postScriptString[res2]);
        break;
    }
  }
}
