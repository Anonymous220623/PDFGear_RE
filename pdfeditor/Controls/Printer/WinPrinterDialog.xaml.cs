// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Printer.PrinterInfo
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.Utils.Printer;
using System.Text;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Controls.Printer;

public partial class PrinterInfo
{
  public FontFamily FontFamily { get; set; }

  public FontStyle FontStyle { get; set; }

  public FontWeight FontWeight { get; set; }

  public FontStretch FontStretch { get; set; }

  public FlowDirection FlowDirection { get; set; }

  public Brush Foreground { get; set; }

  public double FontSize { get; set; }

  public string PrinterName { get; set; }

  public PrintDevModeHandle PrintDevMode { get; set; }

  public override string ToString()
  {
    if (this.PrinterName.Length <= 40)
      return this.PrinterName;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(this.PrinterName.Substring(0, 18));
    stringBuilder.Append("...");
    stringBuilder.Append(this.PrinterName.Substring(this.PrinterName.Length - 18, 18));
    return stringBuilder.ToString();
  }
}
