// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.ObservableUIElementCollection
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public class ObservableUIElementCollection(UIElement visualParent, FrameworkElement logicalParent) : 
  UIElementCollection(visualParent, logicalParent),
  INotifyCollectionChanged
{
  public event NotifyCollectionChangedEventHandler CollectionChanged;

  public override int Add(UIElement element)
  {
    int num = base.Add(element);
    int index = this.IndexOf(element);
    this.OnCollectionChanged(NotifyCollectionChangedAction.Add, (IList) new UIElement[1]
    {
      element
    }, (IList) null, index);
    return num;
  }

  public override void Clear()
  {
    IList removed = (IList) this;
    base.Clear();
    this.OnCollectionChanged(NotifyCollectionChangedAction.Reset, (IList) null, removed, -1);
  }

  public override void Insert(int index, UIElement element)
  {
    base.Insert(index, element);
    this.OnCollectionChanged(NotifyCollectionChangedAction.Add, (IList) new UIElement[1]
    {
      element
    }, (IList) null, index);
  }

  public override void Remove(UIElement element)
  {
    base.Remove(element);
    int index = this.IndexOf(element);
    this.OnCollectionChanged(NotifyCollectionChangedAction.Remove, (IList) null, (IList) new UIElement[1]
    {
      element
    }, index);
  }

  public override void RemoveAt(int index)
  {
    UIElement[] removed = new UIElement[1]{ this[index] };
    base.RemoveAt(index);
    this.OnCollectionChanged(NotifyCollectionChangedAction.Remove, (IList) null, (IList) removed, index);
  }

  public override void RemoveRange(int index, int count)
  {
    ArrayList removed = new ArrayList();
    int num = index + count;
    if (this.Count >= num)
    {
      for (int index1 = index; index1 < num; ++index1)
        removed.Add((object) this[index1]);
    }
    base.RemoveRange(index, count);
    this.OnCollectionChanged(NotifyCollectionChangedAction.Remove, (IList) null, (IList) removed, index);
  }

  private void OnCollectionChanged(
    NotifyCollectionChangedAction action,
    IList added,
    IList removed,
    int index)
  {
    if (this.CollectionChanged == null)
      return;
    this.CollectionChanged((object) this, new NotifyCollectionChangedEventArgs(action, added, removed, index));
  }
}
