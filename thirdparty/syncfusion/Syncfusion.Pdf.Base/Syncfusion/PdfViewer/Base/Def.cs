// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.Def
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class Def : PostScriptOperators
{
  public override void Execute(FontInterpreter interpreter)
  {
    object last = interpreter.Operands.GetLast();
    string lastAs = interpreter.Operands.GetLastAs<string>();
    interpreter.CurrentDictionary[lastAs] = last;
    if (Syncfusion.PdfViewer.Base.RD.IsRDOperator(lastAs))
      interpreter.RD = (PostScriptArray) last;
    else if (Syncfusion.PdfViewer.Base.ND.IsNDOperator(lastAs))
    {
      interpreter.ND = (PostScriptArray) last;
    }
    else
    {
      if (!Syncfusion.PdfViewer.Base.NP.IsNPOperator(lastAs))
        return;
      interpreter.NP = (PostScriptArray) last;
    }
  }
}
