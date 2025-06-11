// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.UIElementsRecycler`1
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class UIElementsRecycler<T> : IEnumerable<T>, IEnumerable where T : FrameworkElement
{
  private Panel panel;
  private Dictionary<DependencyProperty, Binding> bindingsProvider;

  internal List<T> generatedElements { get; set; }

  public Panel Panel => this.panel;

  public int Count => this.generatedElements.Count;

  public Dictionary<DependencyProperty, Binding> BindingProvider => this.bindingsProvider;

  public UIElementsRecycler(Panel panel)
  {
    this.generatedElements = new List<T>();
    this.bindingsProvider = new Dictionary<DependencyProperty, Binding>();
    this.panel = panel;
  }

  public UIElementsRecycler()
  {
    this.generatedElements = new List<T>();
    this.bindingsProvider = new Dictionary<DependencyProperty, Binding>();
  }

  public void GenerateElements(int count)
  {
    if (count > this.generatedElements.Count)
    {
      count -= this.generatedElements.Count;
      for (int index = 0; index < count; ++index)
      {
        T instance = Activator.CreateInstance<T>();
        foreach (KeyValuePair<DependencyProperty, Binding> keyValuePair in this.bindingsProvider)
          instance.SetBinding(keyValuePair.Key, (BindingBase) keyValuePair.Value);
        this.generatedElements.Add(instance);
        if (this.panel != null)
          this.panel.Children.Add((UIElement) instance);
      }
    }
    else
    {
      if (count >= this.generatedElements.Count)
        return;
      count = this.generatedElements.Count - count;
      for (int index = 0; index < count; ++index)
      {
        T element = this.generatedElements.ElementAt<T>(0);
        this.generatedElements.Remove(element);
        if (this.panel != null && this.panel.Children.Contains((UIElement) element))
          this.panel.Children.Remove((UIElement) element);
      }
    }
  }

  internal void GenerateElementsOfType(int count, Type type)
  {
    if (count > this.generatedElements.Count)
    {
      count -= this.generatedElements.Count;
      for (int index = 0; index < count; ++index)
      {
        T instance = Activator.CreateInstance(type) as T;
        foreach (KeyValuePair<DependencyProperty, Binding> keyValuePair in this.bindingsProvider)
          instance.SetBinding(keyValuePair.Key, (BindingBase) keyValuePair.Value);
        this.generatedElements.Add(instance);
        if (this.panel != null)
          this.panel.Children.Add((UIElement) instance);
      }
    }
    else
    {
      if (count >= this.generatedElements.Count)
        return;
      count = this.generatedElements.Count - count;
      for (int index = 0; index < count; ++index)
      {
        T element = this.generatedElements.ElementAt<T>(0);
        this.generatedElements.Remove(element);
        if (this.panel != null && this.panel.Children.Contains((UIElement) element))
          this.panel.Children.Remove((UIElement) element);
      }
    }
  }

  public void Add(T element)
  {
    foreach (KeyValuePair<DependencyProperty, Binding> keyValuePair in this.bindingsProvider)
      element.SetBinding(keyValuePair.Key, (BindingBase) keyValuePair.Value);
    if (this.panel == null || this.panel.Children.Contains((UIElement) element))
      return;
    this.generatedElements.Add(element);
    this.panel.Children.Add((UIElement) element);
  }

  public int IndexOf(T element) => this.generatedElements.IndexOf(element);

  public void Remove(T element)
  {
    if (this.panel == null || !this.panel.Children.Contains((UIElement) element))
      return;
    this.generatedElements.Remove(element);
    this.panel.Children.Remove((UIElement) element);
  }

  public T CreateNewInstance()
  {
    T instance = Activator.CreateInstance<T>();
    foreach (KeyValuePair<DependencyProperty, Binding> keyValuePair in this.bindingsProvider)
      instance.SetBinding(keyValuePair.Key, (BindingBase) keyValuePair.Value);
    this.generatedElements.Add(instance);
    if (this.panel != null)
      this.panel.Children.Add((UIElement) instance);
    return instance;
  }

  public void RemoveBinding(DependencyProperty property)
  {
    this.BindingProvider.Remove(property);
    foreach (T generatedElement in this.generatedElements)
      BindingOperations.ClearBinding((DependencyObject) generatedElement, property);
  }

  public void Clear()
  {
    if (this.panel != null)
    {
      foreach (T generatedElement in this.generatedElements)
      {
        if (this.panel.Children.Contains((UIElement) generatedElement))
          this.panel.Children.Remove((UIElement) generatedElement);
      }
    }
    this.generatedElements.Clear();
  }

  public T this[int index]
  {
    get => this.generatedElements.Count <= index ? default (T) : this.generatedElements[index];
  }

  public IEnumerator<T> GetEnumerator() => (IEnumerator<T>) this.generatedElements.GetEnumerator();

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.generatedElements.GetEnumerator();
}
