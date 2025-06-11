// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.Seac
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class Seac : Operator
{
  public override void Execute(CharacterBuilder buildChar)
  {
    int lastAsInt1 = buildChar.Operands.GetLastAsInt();
    int lastAsInt2 = buildChar.Operands.GetLastAsInt();
    int lastAsInt3 = buildChar.Operands.GetLastAsInt();
    int lastAsInt4 = buildChar.Operands.GetLastAsInt();
    buildChar.Operands.GetLastAsInt();
    string accentedChar = PresettedEncodings.StandardEncoding[lastAsInt1];
    string baseChar = PresettedEncodings.StandardEncoding[lastAsInt2];
    buildChar.CombineChars(accentedChar, baseChar, lastAsInt4, lastAsInt3);
    buildChar.Operands.Clear();
  }
}
