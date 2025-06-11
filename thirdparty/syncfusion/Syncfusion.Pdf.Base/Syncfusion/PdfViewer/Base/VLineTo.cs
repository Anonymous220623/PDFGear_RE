// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.VLineTo
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class VLineTo : Operator
{
  public override void Execute(CharacterBuilder interpreter)
  {
    if (interpreter.Operands.Count % 2 == 0)
    {
      while (interpreter.Operands.Count > 0)
      {
        int firstAsInt1 = interpreter.Operands.GetFirstAsInt();
        int firstAsInt2 = interpreter.Operands.GetFirstAsInt();
        Operator.VLineTo(interpreter, firstAsInt1);
        Operator.HLineTo(interpreter, firstAsInt2);
      }
    }
    else
    {
      int firstAsInt3 = interpreter.Operands.GetFirstAsInt();
      Operator.VLineTo(interpreter, firstAsInt3);
      while (interpreter.Operands.Count > 0)
      {
        int firstAsInt4 = interpreter.Operands.GetFirstAsInt();
        int firstAsInt5 = interpreter.Operands.GetFirstAsInt();
        Operator.HLineTo(interpreter, firstAsInt4);
        Operator.VLineTo(interpreter, firstAsInt5);
      }
    }
    interpreter.Operands.Clear();
  }
}
