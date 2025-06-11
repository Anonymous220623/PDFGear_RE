// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.IfElse
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class IfElse : PostScriptOperators
{
  public override void Execute(FontInterpreter interpreter)
  {
    PostScriptArray lastAs1 = interpreter.Operands.GetLastAs<PostScriptArray>();
    PostScriptArray lastAs2 = interpreter.Operands.GetLastAs<PostScriptArray>();
    if (interpreter.Operands.GetLastAs<bool>())
      interpreter.ExecuteProcedure(lastAs2);
    else
      interpreter.ExecuteProcedure(lastAs1);
  }
}
