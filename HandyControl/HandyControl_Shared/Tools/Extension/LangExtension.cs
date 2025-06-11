// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.Extension.LangExtension
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Properties.Langs;
using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

#nullable disable
namespace HandyControl.Tools.Extension;

public class LangExtension : MarkupExtension
{
  private readonly DependencyObject _proxy;
  public static readonly DependencyProperty KeyProperty = DependencyProperty.RegisterAttached(nameof (Key), typeof (object), typeof (LangExtension), new PropertyMetadata((PropertyChangedCallback) null));
  private static readonly DependencyProperty TargetPropertyProperty = DependencyProperty.RegisterAttached("TargetProperty", typeof (DependencyProperty), typeof (LangExtension), new PropertyMetadata((object) null));

  public LangExtension()
  {
    this._proxy = new DependencyObject();
    this.Source = (object) LangProvider.Instance;
  }

  public LangExtension(string key)
    : this()
  {
    this.Key = (object) key;
  }

  public object Key
  {
    get => this._proxy.GetValue(LangExtension.KeyProperty);
    set => this._proxy.SetValue(LangExtension.KeyProperty, value);
  }

  private static void SetTargetProperty(DependencyObject element, DependencyProperty value)
  {
    element.SetValue(LangExtension.TargetPropertyProperty, (object) value);
  }

  private static DependencyProperty GetTargetProperty(DependencyObject element)
  {
    return (DependencyProperty) element.GetValue(LangExtension.TargetPropertyProperty);
  }

  public BindingMode Mode { get; set; }

  public IValueConverter Converter { get; set; }

  public object ConverterParameter { get; set; }

  public object Source { get; set; }

  public override object ProvideValue(IServiceProvider serviceProvider)
  {
    if (!(serviceProvider.GetService(typeof (IProvideValueTarget)) is IProvideValueTarget service))
      return (object) this;
    if (service.TargetObject.GetType().FullName == "System.Windows.SharedDp")
      return (object) this;
    if (!(service.TargetObject is DependencyObject targetObject))
      return (object) this;
    if (!(service.TargetProperty is DependencyProperty targetProperty))
      return (object) this;
    switch (this.Key)
    {
      case string key:
        BindingBase langBinding = this.CreateLangBinding(key);
        BindingOperations.SetBinding(targetObject, targetProperty, langBinding);
        return langBinding.ProvideValue(serviceProvider);
      case Binding binding2:
        if (targetObject is FrameworkElement frameworkElement)
        {
          if (frameworkElement.DataContext != null)
            return this.SetLangBinding((DependencyObject) frameworkElement, targetProperty, binding2.Path, frameworkElement.DataContext).ProvideValue(serviceProvider);
          LangExtension.SetTargetProperty((DependencyObject) frameworkElement, targetProperty);
          frameworkElement.DataContextChanged += new DependencyPropertyChangedEventHandler(this.LangExtension_DataContextChanged);
          break;
        }
        Binding binding1 = binding2;
        if (targetObject is FrameworkContentElement frameworkContentElement)
        {
          if (frameworkContentElement.DataContext != null)
            return this.SetLangBinding((DependencyObject) frameworkContentElement, targetProperty, binding1.Path, frameworkContentElement.DataContext).ProvideValue(serviceProvider);
          LangExtension.SetTargetProperty((DependencyObject) frameworkContentElement, targetProperty);
          frameworkContentElement.DataContextChanged += new DependencyPropertyChangedEventHandler(this.LangExtension_DataContextChanged);
          break;
        }
        break;
    }
    return (object) string.Empty;
  }

  private void LangExtension_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
  {
    switch (sender)
    {
      case FrameworkElement frameworkElement:
        frameworkElement.DataContextChanged -= new DependencyPropertyChangedEventHandler(this.LangExtension_DataContextChanged);
        if (!(this.Key is Binding key1))
          break;
        DependencyProperty targetProperty1 = LangExtension.GetTargetProperty((DependencyObject) frameworkElement);
        LangExtension.SetTargetProperty((DependencyObject) frameworkElement, (DependencyProperty) null);
        this.SetLangBinding((DependencyObject) frameworkElement, targetProperty1, key1.Path, frameworkElement.DataContext);
        break;
      case FrameworkContentElement frameworkContentElement:
        frameworkContentElement.DataContextChanged -= new DependencyPropertyChangedEventHandler(this.LangExtension_DataContextChanged);
        if (!(this.Key is Binding key2))
          break;
        DependencyProperty targetProperty2 = LangExtension.GetTargetProperty((DependencyObject) frameworkContentElement);
        LangExtension.SetTargetProperty((DependencyObject) frameworkContentElement, (DependencyProperty) null);
        this.SetLangBinding((DependencyObject) frameworkContentElement, targetProperty2, key2.Path, frameworkContentElement.DataContext);
        break;
    }
  }

  private BindingBase SetLangBinding(
    DependencyObject targetObject,
    DependencyProperty targetProperty,
    PropertyPath path,
    object dataContext)
  {
    if (targetProperty == null)
      return (BindingBase) null;
    BindingOperations.SetBinding(targetObject, targetProperty, (BindingBase) new Binding()
    {
      Path = path,
      Source = dataContext,
      Mode = BindingMode.OneWay
    });
    string key = targetObject.GetValue(targetProperty) as string;
    if (string.IsNullOrEmpty(key))
      return (BindingBase) null;
    BindingBase langBinding = this.CreateLangBinding(key);
    BindingOperations.SetBinding(targetObject, targetProperty, langBinding);
    return langBinding;
  }

  private BindingBase CreateLangBinding(string key)
  {
    return (BindingBase) new Binding(key)
    {
      Converter = this.Converter,
      ConverterParameter = this.ConverterParameter,
      UpdateSourceTrigger = UpdateSourceTrigger.Explicit,
      Source = this.Source,
      Mode = BindingMode.OneWay
    };
  }
}
