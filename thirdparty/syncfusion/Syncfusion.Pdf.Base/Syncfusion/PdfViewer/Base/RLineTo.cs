// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.RLineTo
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class RLineTo : Operator
{
  public override void Execute(CharacterBuilder interpreter)
  {
    while (interpreter.Operands.Count / 2 > 0)
    {
      int firstAsInt1 = interpreter.Operands.GetFirstAsInt();
      int firstAsInt2 = interpreter.Operands.GetFirstAsInt();
      Operator.LineTo(interpreter, firstAsInt1, firstAsInt2);
    }
    interpreter.Operands.Clear();
  }
}
