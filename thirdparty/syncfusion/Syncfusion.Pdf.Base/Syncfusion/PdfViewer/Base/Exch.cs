// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.Exch
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class Exch : PostScriptOperators
{
  public override void Execute(FontInterpreter interpreter)
  {
    object last1 = interpreter.Operands.GetLast();
    object last2 = interpreter.Operands.GetLast();
    interpreter.Operands.AddLast(last1);
    interpreter.Operands.AddLast(last2);
  }
}
