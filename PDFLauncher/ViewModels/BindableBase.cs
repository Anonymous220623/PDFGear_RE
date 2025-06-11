// Decompiled with JetBrains decompiler
// Type: PDFLauncher.ViewModels.BindableBase
// Assembly: PDFLauncher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 94FE2CE7-EB91-4B8B-872D-74808A04C1E6
// Assembly location: C:\Program Files\PDFgear\PDFLauncher.exe

using System.ComponentModel;
using System.Runtime.CompilerServices;

#nullable disable
namespace PDFLauncher.ViewModels;

public abstract class BindableBase : INotifyPropertyChanged
{
  public event PropertyChangedEventHandler PropertyChanged = (_param1, _param2) => { };

  protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
  {
    if (object.Equals((object) storage, (object) value))
      return false;
    storage = value;
    this.OnPropertyChanged(propertyName);
    return true;
  }

  public void OnPropertyChanged([CallerMemberName] string propertyName = null)
  {
    if (this.PropertyChanged == null)
      return;
    this.PropertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
  }
}
