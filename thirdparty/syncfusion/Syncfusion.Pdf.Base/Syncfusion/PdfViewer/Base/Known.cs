// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.Known
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class Known : PostScriptOperators
{
  public override void Execute(FontInterpreter interpreter)
  {
    string lastAs1 = interpreter.Operands.GetLastAs<string>();
    PostScriptDict lastAs2 = interpreter.Operands.GetLastAs<PostScriptDict>();
    interpreter.Operands.AddLast((object) lastAs2.ContainsKey(lastAs1));
  }
}
