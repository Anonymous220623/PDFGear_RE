// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Speech.ImgaeFilePath
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System.ComponentModel;
using System.Runtime.CompilerServices;

#nullable disable
namespace pdfeditor.Controls.Speech;

public class ImgaeFilePath : INotifyPropertyChanged
{
  private string imagepath = "pack://application:,,,/Style/Resources/Speech/Play.png";

  public event PropertyChangedEventHandler PropertyChanged;

  private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
  {
    if (this.PropertyChanged == null)
      return;
    this.PropertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
  }

  public string ImagePath
  {
    get => this.imagepath;
    set => this.Set<string>(ref this.imagepath, value, nameof (ImagePath));
  }

  private void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
  {
    if (object.Equals((object) storage, (object) value))
      return;
    storage = value;
    this.NotifyPropertyChanged(propertyName);
  }
}
