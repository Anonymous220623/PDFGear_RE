// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.DefineFont
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class DefineFont : PostScriptOperators
{
  public override void Execute(FontInterpreter interpreter)
  {
    PostScriptDict lastAs1 = interpreter.Operands.GetLastAs<PostScriptDict>();
    string lastAs2 = interpreter.Operands.GetLastAs<string>();
    BaseType1Font baseType1Font = new BaseType1Font();
    baseType1Font.Load(lastAs1);
    interpreter.Fonts[lastAs2] = baseType1Font;
    interpreter.Operands.AddLast((object) baseType1Font);
  }
}
