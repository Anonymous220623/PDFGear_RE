// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.FilterItemElement
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.ComponentModel;
using System.Linq.Expressions;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

public class FilterItemElement : IComparable, INotifyPropertyChanged
{
  private bool suspendPropertyChangedTrigger;
  private bool? isSelected;

  public FilterItemElement()
  {
    this.isSelected = new bool?(true);
    this.SelectedState = new bool?(true);
    this.IsChanged = new bool?(true);
  }

  public event PropertyChangedEventHandler PropertyChanged;

  public string Key { get; set; }

  public bool? IsSelected
  {
    get => this.isSelected;
    set
    {
      this.isSelected = value;
      if (this.suspendPropertyChangedTrigger)
        return;
      this.OnPropertyChanged<bool?>((Expression<Func<FilterItemElement, bool?>>) (pc => pc.IsSelected));
    }
  }

  public bool IsAllFilter { get; set; }

  public bool? SelectedState { get; set; }

  public bool? IsChanged { get; set; }

  public void AcceptChanges()
  {
    this.suspendPropertyChangedTrigger = true;
    this.SelectedState = this.IsSelected;
    this.suspendPropertyChangedTrigger = false;
  }

  public void RejectChanges()
  {
    this.IsSelected = this.SelectedState;
    this.suspendPropertyChangedTrigger = false;
  }

  public int CompareTo(object obj)
  {
    return obj is FilterItemElement filterItemElement && filterItemElement.Key != null ? filterItemElement.Key.CompareTo(this.Key) : 0;
  }

  public override string ToString() => this.Key;

  private void OnPropertyChanged<R>(Expression<Func<FilterItemElement, R>> expr)
  {
    this.OnPropertyChanged(((MemberExpression) expr.Body).Member.Name);
  }

  private void OnPropertyChanged(string propertyName)
  {
    if (this.PropertyChanged == null)
      return;
    this.PropertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
  }
}
