// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Annotations.TextStampModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System.Windows;

#nullable disable
namespace pdfeditor.Controls.Annotations;

public class TextStampModel
{
  public double TextFontSize { get; set; }

  public string TextFontFamily { get; set; }

  public FontWeight TextFontWeight { get; set; }

  public FontStyle TextFontStyle { get; set; }

  public string Text { get; set; }

  public double TextWidth { get; set; }

  public double TextHeight { get; set; }

  public string BorderBrush { get; set; }

  public string Foreground { get; set; }

  public double PageScale { get; set; }

  public string TimeFormat { get; set; }
}
