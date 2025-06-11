// Decompiled with JetBrains decompiler
// Type: PDFLauncher.CustomControl.OpenFileTextBox
// Assembly: PDFLauncher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 94FE2CE7-EB91-4B8B-872D-74808A04C1E6
// Assembly location: C:\Program Files\PDFgear\PDFLauncher.exe

using Microsoft.Win32;
using System.Windows;

#nullable disable
namespace PDFLauncher.CustomControl;

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
