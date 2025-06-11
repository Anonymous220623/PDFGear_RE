// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.BindingUtils
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public static class BindingUtils
{
  public static readonly DependencyProperty EnableBindingErrorsProperty = DependencyProperty.RegisterAttached("EnableBindingErrors", typeof (bool), typeof (BindingUtils), (PropertyMetadata) new FrameworkPropertyMetadata((object) true, new PropertyChangedCallback(BindingUtils.OnEnableBindingErrorsChanged)));

  public static bool GetEnableBindingErrors(DependencyObject obj)
  {
    return obj == null || (bool) obj.GetValue(BindingUtils.EnableBindingErrorsProperty);
  }

  public static void SetEnableBindingErrors(DependencyObject obj, bool value)
  {
    obj.SetValue(BindingUtils.EnableBindingErrorsProperty, (object) value);
  }

  private static void OnEnableBindingErrorsChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs e)
  {
    BindingUtils.SetEnableBindingErrors(obj, (bool) e.NewValue);
  }

  public static BindingExpressionBase SetBinding(
    DependencyObject target,
    object source,
    DependencyProperty dp,
    object propertyPath)
  {
    return BindingUtils.SetBinding(target, source, dp, propertyPath, BindingMode.Default, (IValueConverter) null);
  }

  public static BindingExpressionBase SetBinding(
    DependencyObject target,
    object source,
    DependencyProperty dp,
    object propertyPath,
    BindingMode mode)
  {
    return BindingUtils.SetBinding(target, source, dp, propertyPath, mode, (IValueConverter) null);
  }

  public static BindingExpressionBase SetBinding(
    DependencyObject target,
    object source,
    DependencyProperty dp,
    object propertyPath,
    BindingMode mode,
    IValueConverter converter)
  {
    if (source == null)
      throw new ArgumentNullException(nameof (source));
    if (target == null)
      throw new ArgumentNullException(nameof (target));
    if (dp == null)
      throw new ArgumentNullException(nameof (dp));
    if (propertyPath == null)
      throw new ArgumentNullException("sourcePropertyName");
    return BindingOperations.SetBinding(target, dp, (BindingBase) new Binding()
    {
      Source = source,
      Path = new PropertyPath(propertyPath),
      Mode = mode,
      Converter = converter
    });
  }

  public static BindingExpressionBase SetRelativeBinding(
    DependencyObject target,
    DependencyProperty dp,
    Type sourceType,
    object sourceProperty)
  {
    return BindingUtils.SetRelativeBinding(target, dp, sourceType, sourceProperty, BindingMode.Default, 1);
  }

  public static BindingExpressionBase SetRelativeBinding(
    DependencyObject target,
    DependencyProperty dp,
    Type sourceType,
    object sourceProperty,
    BindingMode mode)
  {
    return BindingUtils.SetRelativeBinding(target, dp, sourceType, sourceProperty, mode, 1);
  }

  public static BindingExpressionBase SetRelativeBinding(
    DependencyObject target,
    DependencyProperty dp,
    Type sourceType,
    object sourceProperty,
    BindingMode mode,
    int ancestorLevel)
  {
    if (sourceType == (Type) null)
      throw new ArgumentNullException(nameof (sourceType));
    if (target == null)
      throw new ArgumentNullException(nameof (target));
    if (dp == null)
      throw new ArgumentNullException(nameof (dp));
    if (sourceProperty == null)
      throw new ArgumentNullException("sourcePropertyName");
    return BindingOperations.SetBinding(target, dp, (BindingBase) new Binding()
    {
      RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, sourceType, ancestorLevel),
      Path = new PropertyPath(sourceProperty),
      Mode = mode
    });
  }

  public static BindingExpressionBase SetTemplatedParentBinding(
    DependencyObject target,
    DependencyProperty dp,
    object sourceProperty,
    BindingMode mode)
  {
    if (target == null)
      throw new ArgumentNullException(nameof (target));
    if (dp == null)
      throw new ArgumentNullException(nameof (dp));
    if (sourceProperty == null)
      throw new ArgumentNullException("sourcePropertyName");
    return BindingOperations.SetBinding(target, dp, (BindingBase) new Binding()
    {
      RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent),
      Path = new PropertyPath(sourceProperty),
      Mode = mode
    });
  }

  public static void SetBindingToVisualChild(
    FrameworkElement rootelement,
    Type typeChild,
    object source,
    DependencyProperty dp,
    object sourcePropertyName)
  {
    if (rootelement == null)
      throw new ArgumentNullException(nameof (rootelement));
    if (typeChild == (Type) null)
      throw new ArgumentNullException(nameof (typeChild));
    if (source == null)
      throw new ArgumentNullException(nameof (source));
    if (dp == null)
      throw new ArgumentNullException(nameof (dp));
    switch (sourcePropertyName)
    {
      case null:
        throw new ArgumentNullException(nameof (sourcePropertyName));
      case string _:
      case DependencyProperty _:
        Binding binding = (Binding) null;
        using (IEnumerator<Visual> enumerator = VisualUtils.EnumChildrenOfType((Visual) rootelement, typeChild).GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            Visual current = enumerator.Current;
            if (binding != null)
            {
              binding = new Binding();
              binding.Source = source;
              binding.Path = new PropertyPath(sourcePropertyName);
              binding.Mode = BindingMode.Default;
            }
            BindingOperations.SetBinding((DependencyObject) current, dp, (BindingBase) binding);
          }
          break;
        }
      default:
        throw new ArgumentException("Value should be specified as string or as dependency property instance.", nameof (sourcePropertyName));
    }
  }
}
