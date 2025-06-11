// Decompiled with JetBrains decompiler
// Type: HandyControl.Interactivity.NameResolver
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Windows;

#nullable disable
namespace HandyControl.Interactivity;

internal sealed class NameResolver
{
  private string _name;
  private FrameworkElement _nameScopeReferenceElement;

  private FrameworkElement ActualNameScopeReferenceElement
  {
    get
    {
      return this.NameScopeReferenceElement != null && Interaction.IsElementLoaded(this.NameScopeReferenceElement) ? this.GetActualNameScopeReference(this.NameScopeReferenceElement) : (FrameworkElement) null;
    }
  }

  private bool HasAttempedResolve { get; set; }

  public string Name
  {
    get => this._name;
    set
    {
      DependencyObject oldObject = this.Object;
      this._name = value;
      this.UpdateObjectFromName(oldObject);
    }
  }

  public FrameworkElement NameScopeReferenceElement
  {
    get => this._nameScopeReferenceElement;
    set
    {
      FrameworkElement referenceElement = this.NameScopeReferenceElement;
      this._nameScopeReferenceElement = value;
      this.OnNameScopeReferenceElementChanged(referenceElement);
    }
  }

  public DependencyObject Object
  {
    get
    {
      return string.IsNullOrEmpty(this.Name) && this.HasAttempedResolve ? (DependencyObject) this.NameScopeReferenceElement : this.ResolvedObject;
    }
  }

  private bool PendingReferenceElementLoad { get; set; }

  private DependencyObject ResolvedObject { get; set; }

  public event EventHandler<NameResolvedEventArgs> ResolvedElementChanged;

  private FrameworkElement GetActualNameScopeReference(FrameworkElement initialReferenceElement)
  {
    FrameworkElement frameworkElement = initialReferenceElement;
    return !this.IsNameScope(initialReferenceElement) || !(initialReferenceElement.Parent is FrameworkElement parent) ? frameworkElement : parent;
  }

  private bool IsNameScope(FrameworkElement frameworkElement)
  {
    return frameworkElement.Parent is FrameworkElement parent && parent.FindName(this.Name) != null;
  }

  private void OnNameScopeReferenceElementChanged(FrameworkElement oldNameScopeReference)
  {
    if (this.PendingReferenceElementLoad)
    {
      oldNameScopeReference.Loaded -= new RoutedEventHandler(this.OnNameScopeReferenceLoaded);
      this.PendingReferenceElementLoad = false;
    }
    this.HasAttempedResolve = false;
    this.UpdateObjectFromName(this.Object);
  }

  private void OnNameScopeReferenceLoaded(object sender, RoutedEventArgs e)
  {
    this.PendingReferenceElementLoad = false;
    this.NameScopeReferenceElement.Loaded -= new RoutedEventHandler(this.OnNameScopeReferenceLoaded);
    this.UpdateObjectFromName(this.Object);
  }

  private void OnObjectChanged(DependencyObject oldTarget, DependencyObject newTarget)
  {
    EventHandler<NameResolvedEventArgs> resolvedElementChanged = this.ResolvedElementChanged;
    if (resolvedElementChanged == null)
      return;
    resolvedElementChanged((object) this, new NameResolvedEventArgs((object) oldTarget, (object) newTarget));
  }

  private void UpdateObjectFromName(DependencyObject oldObject)
  {
    DependencyObject dependencyObject = (DependencyObject) null;
    this.ResolvedObject = (DependencyObject) null;
    if (this.NameScopeReferenceElement != null)
    {
      if (!Interaction.IsElementLoaded(this.NameScopeReferenceElement))
      {
        this.NameScopeReferenceElement.Loaded += new RoutedEventHandler(this.OnNameScopeReferenceLoaded);
        this.PendingReferenceElementLoad = true;
        return;
      }
      if (!string.IsNullOrEmpty(this.Name))
      {
        FrameworkElement referenceElement = this.ActualNameScopeReferenceElement;
        if (referenceElement != null)
          dependencyObject = referenceElement.FindName(this.Name) as DependencyObject;
      }
    }
    this.HasAttempedResolve = true;
    this.ResolvedObject = dependencyObject;
    if (object.Equals((object) oldObject, (object) this.Object))
      return;
    this.OnObjectChanged(oldObject, this.Object);
  }
}
