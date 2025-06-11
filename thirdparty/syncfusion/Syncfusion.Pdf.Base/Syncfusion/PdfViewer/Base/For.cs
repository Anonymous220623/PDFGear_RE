// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.For
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class For : PostScriptOperators
{
  public override void Execute(FontInterpreter interpreter)
  {
    PostScriptArray lastAs = interpreter.Operands.GetLastAs<PostScriptArray>();
    double lastAsReal1 = interpreter.Operands.GetLastAsReal();
    double lastAsReal2 = interpreter.Operands.GetLastAsReal();
    for (double lastAsReal3 = interpreter.Operands.GetLastAsReal(); (lastAsReal1 > 0.0 ? (lastAsReal3 < lastAsReal1 ? 1 : 0) : (lastAsReal3 > lastAsReal1 ? 1 : 0)) != 0; lastAsReal3 += lastAsReal2)
    {
      interpreter.Operands.AddLast((object) lastAsReal3);
      interpreter.ExecuteProcedure(lastAs);
    }
  }
}
