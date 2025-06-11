// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.HFlex
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class HFlex : Operator
{
  public override void Execute(CharacterBuilder buildChar)
  {
    int firstAsInt1 = buildChar.Operands.GetFirstAsInt();
    int firstAsInt2 = buildChar.Operands.GetFirstAsInt();
    int firstAsInt3 = buildChar.Operands.GetFirstAsInt();
    int firstAsInt4 = buildChar.Operands.GetFirstAsInt();
    int firstAsInt5 = buildChar.Operands.GetFirstAsInt();
    int firstAsInt6 = buildChar.Operands.GetFirstAsInt();
    int firstAsInt7 = buildChar.Operands.GetFirstAsInt();
    buildChar.Operands.Clear();
    Operator.CurveTo(buildChar, firstAsInt1, 0, firstAsInt2, firstAsInt3, firstAsInt4, 0);
    Operator.CurveTo(buildChar, firstAsInt5, 0, firstAsInt6, 0, firstAsInt7, 0);
  }
}
