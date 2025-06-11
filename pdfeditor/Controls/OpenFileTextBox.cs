// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.OpenFileTextBox
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Win32;
using System.Windows;

#nullable disable
namespace pdfeditor.Controls;

public class OpenFileTextBox : PathTextBox
{
  public static readonly DependencyProperty FilterProperty = DependencyProperty.Register(nameof (Filter), typeof (string), typeof (OpenFileTextBox), new PropertyMetadata((object) string.Empty));

  protected override object CreateDialog(string initialDirectory, string filename)
  {
    OpenFileDialog dialog = new OpenFileDialog();
    dialog.InitialDirectory = initialDirectory;
    dialog.FileName = filename;
    dialog.Filter = this.Filter;
    return (object) dialog;
  }

  public string Filter
  {
    get => (string) this.GetValue(OpenFileTextBox.FilterProperty);
    set => this.SetValue(OpenFileTextBox.FilterProperty, (object) value);
  }
}
