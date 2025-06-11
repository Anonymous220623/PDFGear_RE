// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontSbw
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.PdfViewer.Base;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontSbw : SystemFontOperator
{
  public override void Execute(SystemFontBuildChar buildChar)
  {
    buildChar.Operands.GetLastAsInt();
    int lastAsInt1 = buildChar.Operands.GetLastAsInt();
    int lastAsInt2 = buildChar.Operands.GetLastAsInt();
    int lastAsInt3 = buildChar.Operands.GetLastAsInt();
    buildChar.Operands.Clear();
    buildChar.Width = new int?(lastAsInt1);
    buildChar.CurrentPoint = new Point((double) lastAsInt3, (double) lastAsInt2);
  }
}
