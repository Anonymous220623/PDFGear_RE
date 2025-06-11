// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.Put
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class Put : PostScriptOperators
{
  public override void Execute(FontInterpreter interpreter)
  {
    object last1 = interpreter.Operands.GetLast();
    object last2 = interpreter.Operands.GetLast();
    object last3 = interpreter.Operands.GetLast();
    switch (last3)
    {
      case PostScriptArray _:
        PostScriptArray postScriptArray = (PostScriptArray) last3;
        int res1;
        Helper.ParseInteger(last2, out res1);
        postScriptArray[res1] = last1;
        break;
      case PostScriptDict _:
        ((PostScriptDict) last3)[(string) last2] = last1;
        break;
      case PostScriptStrHelper _:
        PostScriptStrHelper postScriptStrHelper = (PostScriptStrHelper) last3;
        int res2;
        Helper.ParseInteger(last2, out res2);
        int res3;
        Helper.ParseInteger(last1, out res3);
        postScriptStrHelper[res2] = (char) res3;
        break;
    }
  }
}
