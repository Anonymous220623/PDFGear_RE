// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontEndChar
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontEndChar : SystemFontOperator
{
  public override void Execute(SystemFontBuildChar buildChar)
  {
    if (buildChar.Operands.Count >= 4)
    {
      int lastAsInt1 = buildChar.Operands.GetLastAsInt();
      int lastAsInt2 = buildChar.Operands.GetLastAsInt();
      int lastAsInt3 = buildChar.Operands.GetLastAsInt();
      int lastAsInt4 = buildChar.Operands.GetLastAsInt();
      string accentedChar = SystemFontPredefinedEncodings.StandardEncoding[lastAsInt1];
      string baseChar = SystemFontPredefinedEncodings.StandardEncoding[lastAsInt2];
      buildChar.CombineChars(accentedChar, baseChar, lastAsInt4, lastAsInt3);
    }
    SystemFontOperator.ReadWidth(buildChar, 0);
    buildChar.Operands.Clear();
  }
}
