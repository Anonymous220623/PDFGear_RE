// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.Get
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class Get : PostScriptOperators
{
  public override void Execute(FontInterpreter interpreter)
  {
    object last1 = interpreter.Operands.GetLast();
    object last2 = interpreter.Operands.GetLast();
    switch (last2)
    {
      case PostScriptArray _:
        PostScriptArray postScriptArray = (PostScriptArray) last2;
        int res1;
        Helper.ParseInteger(last1, out res1);
        interpreter.Operands.AddLast(postScriptArray[res1]);
        break;
      case PostScriptDict _:
        PostScriptDict postScriptDict = (PostScriptDict) last2;
        interpreter.Operands.AddLast(postScriptDict[(string) last1]);
        break;
      case PostScriptStrHelper _:
        PostScriptStrHelper postScriptStrHelper = (PostScriptStrHelper) last2;
        int res2;
        Helper.ParseInteger(last1, out res2);
        interpreter.Operands.AddLast((object) postScriptStrHelper[res2]);
        break;
    }
  }
}
