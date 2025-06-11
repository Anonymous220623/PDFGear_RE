// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.NotificationObject
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.ComponentModel;
using System.Linq.Expressions;

#nullable disable
namespace Syncfusion.Windows.Shared;

[Serializable]
public abstract class NotificationObject : INotifyPropertyChanged
{
  [field: NonSerialized]
  public event PropertyChangedEventHandler PropertyChanged;

  protected virtual void RaisePropertyChanged(string propertyName)
  {
    if (this.PropertyChanged == null)
      return;
    this.PropertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
  }

  protected void RaisePropertyChanged(params string[] propertyNames)
  {
    if (propertyNames == null)
      throw new ArgumentNullException(nameof (propertyNames));
    foreach (string propertyName in propertyNames)
      this.RaisePropertyChanged(propertyName);
  }

  protected void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
  {
    this.RaisePropertyChanged(PropertySupport.ExtractPropertyName<T>(propertyExpression));
  }
}
