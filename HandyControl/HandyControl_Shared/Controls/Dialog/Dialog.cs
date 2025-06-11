// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.Dialog
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Controls;

[TemplatePart(Name = "PART_BackElement", Type = typeof (Border))]
public class Dialog : ContentControl
{
  private const string BackElement = "PART_BackElement";
  private string _token;
  private Border _backElement;
  private AdornerContainer _container;
  private static readonly Dictionary<string, FrameworkElement> ContainerDict = new Dictionary<string, FrameworkElement>();
  private static readonly Dictionary<string, Dialog> DialogDict = new Dictionary<string, Dialog>();
  public static readonly DependencyProperty IsClosedProperty = DependencyProperty.Register(nameof (IsClosed), typeof (bool), typeof (Dialog), new PropertyMetadata(ValueBoxes.FalseBox));
  public static readonly DependencyProperty MaskCanCloseProperty = DependencyProperty.RegisterAttached("MaskCanClose", typeof (bool), typeof (Dialog), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits));
  public static readonly DependencyProperty MaskBrushProperty = DependencyProperty.Register(nameof (MaskBrush), typeof (Brush), typeof (Dialog), new PropertyMetadata((object) null));
  public static readonly DependencyProperty TokenProperty = DependencyProperty.RegisterAttached("Token", typeof (string), typeof (Dialog), new PropertyMetadata((object) null, new PropertyChangedCallback(Dialog.OnTokenChanged)));

  public bool IsClosed
  {
    get => (bool) this.GetValue(Dialog.IsClosedProperty);
    internal set => this.SetValue(Dialog.IsClosedProperty, ValueBoxes.BooleanBox(value));
  }

  public static void SetMaskCanClose(DependencyObject element, bool value)
  {
    element.SetValue(Dialog.MaskCanCloseProperty, ValueBoxes.BooleanBox(value));
  }

  public static bool GetMaskCanClose(DependencyObject element)
  {
    return (bool) element.GetValue(Dialog.MaskCanCloseProperty);
  }

  public Brush MaskBrush
  {
    get => (Brush) this.GetValue(Dialog.MaskBrushProperty);
    set => this.SetValue(Dialog.MaskBrushProperty, (object) value);
  }

  private static void OnTokenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is FrameworkElement element))
      return;
    if (e.NewValue == null)
      Dialog.Unregister(element);
    else
      Dialog.Register(e.NewValue.ToString(), element);
  }

  public static void SetToken(DependencyObject element, string value)
  {
    element.SetValue(Dialog.TokenProperty, (object) value);
  }

  public static string GetToken(DependencyObject element)
  {
    return (string) element.GetValue(Dialog.TokenProperty);
  }

  public Dialog()
  {
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Close, (ExecutedRoutedEventHandler) ((s, e) => this.Close())));
  }

  public static void Register(string token, FrameworkElement element)
  {
    if (string.IsNullOrEmpty(token) || element == null)
      return;
    Dialog.ContainerDict[token] = element;
  }

  public static void Unregister(string token, FrameworkElement element)
  {
    if (string.IsNullOrEmpty(token) || element == null || !Dialog.ContainerDict.ContainsKey(token) || Dialog.ContainerDict[token] != element)
      return;
    Dialog.ContainerDict.Remove(token);
  }

  public static void Unregister(FrameworkElement element)
  {
    if (element == null)
      return;
    KeyValuePair<string, FrameworkElement> keyValuePair = Dialog.ContainerDict.FirstOrDefault<KeyValuePair<string, FrameworkElement>>((Func<KeyValuePair<string, FrameworkElement>, bool>) (item => element == item.Value));
    if (string.IsNullOrEmpty(keyValuePair.Key))
      return;
    Dialog.ContainerDict.Remove(keyValuePair.Key);
  }

  public static void Unregister(string token)
  {
    if (string.IsNullOrEmpty(token) || !Dialog.ContainerDict.ContainsKey(token))
      return;
    Dialog.ContainerDict.Remove(token);
  }

  public static Dialog Show<T>(string token = "") where T : new()
  {
    return Dialog.Show((object) new T(), token);
  }

  public static Dialog Show(object content, string token = "")
  {
    Dialog dialog1 = new Dialog();
    dialog1._token = token;
    dialog1.Content = content;
    Dialog dialog2 = dialog1;
    AdornerDecorator adornerDecorator;
    if (string.IsNullOrEmpty(token))
    {
      adornerDecorator = VisualHelper.GetChild<AdornerDecorator>((DependencyObject) WindowHelper.GetActiveWindow());
    }
    else
    {
      Dialog.Close(token);
      Dialog.DialogDict[token] = dialog2;
      FrameworkElement d;
      Dialog.ContainerDict.TryGetValue(token, out d);
      adornerDecorator = d is System.Windows.Window ? VisualHelper.GetChild<AdornerDecorator>((DependencyObject) d) : (AdornerDecorator) VisualHelper.GetChild<DialogContainer>((DependencyObject) d);
    }
    if (adornerDecorator != null)
    {
      if (adornerDecorator.Child != null)
        adornerDecorator.Child.IsEnabled = false;
      AdornerLayer adornerLayer = adornerDecorator.AdornerLayer;
      if (adornerLayer != null)
      {
        AdornerContainer adornerContainer = new AdornerContainer((UIElement) adornerLayer)
        {
          Child = (UIElement) dialog2
        };
        dialog2._container = adornerContainer;
        dialog2.IsClosed = false;
        adornerLayer.Add((Adorner) adornerContainer);
      }
    }
    return dialog2;
  }

  public static void Close(string token)
  {
    Dialog dialog;
    if (!Dialog.DialogDict.TryGetValue(token, out dialog))
      return;
    dialog.Close();
  }

  public void Close()
  {
    if (string.IsNullOrEmpty(this._token))
    {
      this.Close((DependencyObject) WindowHelper.GetActiveWindow());
    }
    else
    {
      FrameworkElement element;
      if (!Dialog.ContainerDict.TryGetValue(this._token, out element))
        return;
      this.Close((DependencyObject) element);
      Dialog.DialogDict.Remove(this._token);
    }
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this._backElement = this.GetTemplateChild("PART_BackElement") as Border;
  }

  protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
  {
    if (!Dialog.GetMaskCanClose((DependencyObject) this))
      return;
    Border backElement = this._backElement;
    if (backElement == null || !backElement.IsMouseDirectlyOver)
      return;
    this.Close();
  }

  private void Close(DependencyObject element)
  {
    if (element == null || this._container == null)
      return;
    AdornerDecorator child = VisualHelper.GetChild<AdornerDecorator>(element);
    if (child == null)
      return;
    if (child.Child != null)
      child.Child.IsEnabled = true;
    child.AdornerLayer?.Remove((Adorner) this._container);
    this.IsClosed = true;
  }
}
