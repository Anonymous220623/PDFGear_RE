// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontDefineFont
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontDefineFont : SystemFontPostScriptOperator
{
  public override void Execute(SystemFontInterpreter interpreter)
  {
    SystemFontPostScriptDictionary lastAs1 = interpreter.Operands.GetLastAs<SystemFontPostScriptDictionary>();
    string lastAs2 = interpreter.Operands.GetLastAs<string>();
    SystemFontType1Font systemFontType1Font = new SystemFontType1Font();
    systemFontType1Font.Load(lastAs1);
    interpreter.Fonts[lastAs2] = systemFontType1Font;
    interpreter.Operands.AddLast((object) systemFontType1Font);
  }
}
