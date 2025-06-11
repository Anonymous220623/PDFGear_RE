// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.CallOtherSubr
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class CallOtherSubr : Operator
{
  public override void Execute(CharacterBuilder interpreter)
  {
    int lastAsInt1 = interpreter.Operands.GetLastAsInt();
    int lastAsInt2 = interpreter.Operands.GetLastAsInt();
    switch (lastAsInt1)
    {
      case 0:
        interpreter.PostScriptStack.AddLast(interpreter.Operands.GetLast());
        interpreter.PostScriptStack.AddLast(interpreter.Operands.GetLast());
        break;
      case 3:
        interpreter.PostScriptStack.AddLast((object) 3);
        break;
      default:
        for (int index = 0; index < lastAsInt2; ++index)
          interpreter.PostScriptStack.AddLast(interpreter.Operands.GetLast());
        break;
    }
    interpreter.Operands.Clear();
  }
}
