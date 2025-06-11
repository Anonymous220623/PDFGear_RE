// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontFlex1
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontFlex1 : SystemFontOperator
{
  public override void Execute(SystemFontBuildChar buildChar)
  {
    int firstAsInt1 = buildChar.Operands.GetFirstAsInt();
    int firstAsInt2 = buildChar.Operands.GetFirstAsInt();
    int firstAsInt3 = buildChar.Operands.GetFirstAsInt();
    int firstAsInt4 = buildChar.Operands.GetFirstAsInt();
    int firstAsInt5 = buildChar.Operands.GetFirstAsInt();
    int firstAsInt6 = buildChar.Operands.GetFirstAsInt();
    int firstAsInt7 = buildChar.Operands.GetFirstAsInt();
    int firstAsInt8 = buildChar.Operands.GetFirstAsInt();
    int firstAsInt9 = buildChar.Operands.GetFirstAsInt();
    int firstAsInt10 = buildChar.Operands.GetFirstAsInt();
    int firstAsInt11 = buildChar.Operands.GetFirstAsInt();
    int num1 = firstAsInt1 + firstAsInt3 + firstAsInt5 + firstAsInt7 + firstAsInt9;
    int num2 = firstAsInt1 + firstAsInt3 + firstAsInt5 + firstAsInt7 + firstAsInt9;
    int dxc = 0;
    int dyc = 0;
    if (Math.Abs(num1) > Math.Abs(num2))
      dxc = firstAsInt11;
    else
      dyc = firstAsInt11;
    buildChar.Operands.Clear();
    SystemFontOperator.CurveTo(buildChar, firstAsInt1, firstAsInt2, firstAsInt3, firstAsInt4, firstAsInt5, firstAsInt6);
    SystemFontOperator.CurveTo(buildChar, firstAsInt7, firstAsInt8, firstAsInt9, firstAsInt10, dxc, dyc);
  }
}
