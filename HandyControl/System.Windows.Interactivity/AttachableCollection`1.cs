// Decompiled with JetBrains decompiler
// Type: HandyControl.Interactivity.AttachableCollection`1
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Windows;

#nullable disable
namespace HandyControl.Interactivity;

public abstract class AttachableCollection<T> : FreezableCollection<T>, IAttachedObject where T : DependencyObject, IAttachedObject
{
  private DependencyObject _associatedObject;
  private Collection<T> _snapshot;

  internal AttachableCollection()
  {
    ((INotifyCollectionChanged) this).CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnCollectionChanged);
    this._snapshot = new Collection<T>();
  }

  public DependencyObject AssociatedObject
  {
    get
    {
      this.ReadPreamble();
      return this._associatedObject;
    }
  }

  DependencyObject IAttachedObject.AssociatedObject => this.AssociatedObject;

  public void Attach(DependencyObject dependencyObject)
  {
    if (object.Equals((object) dependencyObject, (object) this.AssociatedObject))
      return;
    if (this.AssociatedObject != null)
      throw new InvalidOperationException();
    if (Interaction.ShouldRunInDesignMode || !(bool) this.GetValue(DesignerProperties.IsInDesignModeProperty))
    {
      this.WritePreamble();
      this._associatedObject = dependencyObject;
      this.WritePostscript();
    }
    this.OnAttached();
  }

  public void Detach()
  {
    this.OnDetaching();
    this.WritePreamble();
    this._associatedObject = (DependencyObject) null;
    this.WritePostscript();
  }

  protected abstract void OnAttached();

  protected abstract void OnDetaching();

  internal abstract void ItemAdded(T item);

  internal abstract void ItemRemoved(T item);

  private void VerifyAdd(T item)
  {
    if ((object) item == null)
      throw new ArgumentNullException(nameof (item));
    if (this._snapshot.Contains(item))
      throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, ExceptionStringTable.DuplicateItemInCollectionExceptionMessage, new object[2]
      {
        (object) typeof (T).Name,
        (object) this.GetType().Name
      }));
  }

  private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
    switch (e.Action)
    {
      case NotifyCollectionChangedAction.Add:
        IEnumerator enumerator1 = e.NewItems.GetEnumerator();
        try
        {
          while (enumerator1.MoveNext())
          {
            T current = (T) enumerator1.Current;
            try
            {
              this.VerifyAdd(current);
              this.ItemAdded(current);
            }
            finally
            {
              this._snapshot.Insert(this.IndexOf(current), current);
            }
          }
          break;
        }
        finally
        {
          if (enumerator1 is IDisposable disposable)
            disposable.Dispose();
        }
      case NotifyCollectionChangedAction.Remove:
        IEnumerator enumerator2 = e.OldItems.GetEnumerator();
        try
        {
          while (enumerator2.MoveNext())
          {
            T current = (T) enumerator2.Current;
            this.ItemRemoved(current);
            this._snapshot.Remove(current);
          }
          break;
        }
        finally
        {
          if (enumerator2 is IDisposable disposable)
            disposable.Dispose();
        }
      case NotifyCollectionChangedAction.Replace:
        foreach (T oldItem in (IEnumerable) e.OldItems)
        {
          this.ItemRemoved(oldItem);
          this._snapshot.Remove(oldItem);
        }
        IEnumerator enumerator3 = e.NewItems.GetEnumerator();
        try
        {
          while (enumerator3.MoveNext())
          {
            T current = (T) enumerator3.Current;
            try
            {
              this.VerifyAdd(current);
              this.ItemAdded(current);
            }
            finally
            {
              this._snapshot.Insert(this.IndexOf(current), current);
            }
          }
          break;
        }
        finally
        {
          if (enumerator3 is IDisposable disposable)
            disposable.Dispose();
        }
      case NotifyCollectionChangedAction.Reset:
        foreach (T obj in this._snapshot)
          this.ItemRemoved(obj);
        this._snapshot = new Collection<T>();
        using (FreezableCollection<T>.Enumerator enumerator4 = this.GetEnumerator())
        {
          while (enumerator4.MoveNext())
          {
            T current = enumerator4.Current;
            this.VerifyAdd(current);
            this.ItemAdded(current);
          }
          break;
        }
    }
  }
}
