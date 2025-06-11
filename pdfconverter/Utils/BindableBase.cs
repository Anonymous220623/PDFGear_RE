// Decompiled with JetBrains decompiler
// Type: pdfconverter.Utils.BindableBase
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using System.ComponentModel;
using System.Runtime.CompilerServices;

#nullable disable
namespace pdfconverter.Utils;

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
