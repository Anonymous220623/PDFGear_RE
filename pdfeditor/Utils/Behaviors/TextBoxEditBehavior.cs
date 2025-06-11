// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.Behaviors.TextBoxEditBehavior
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Utils.Behaviors;

public class TextBoxEditBehavior : Behavior<TextBox>
{
  private bool innerSet;
  private UIElement rootVisual;
  public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof (Text), typeof (string), typeof (TextBoxEditBehavior), new PropertyMetadata((object) "", new PropertyChangedCallback(TextBoxEditBehavior.OnTextPropertyChanged)));
  public static readonly DependencyProperty ApplyWhenClickEmptyProperty = DependencyProperty.Register(nameof (ApplyWhenClickEmpty), typeof (bool), typeof (TextBoxEditBehavior), new PropertyMetadata((object) false, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is TextBoxEditBehavior textBoxEditBehavior2) || object.Equals(a.NewValue, a.OldValue))
      return;
    textBoxEditBehavior2.UpdateRootVisualEvent();
  })));

  protected override void OnAttached()
  {
    base.OnAttached();
    this.innerSet = true;
    this.Text = this.AssociatedObject.Text;
    this.innerSet = false;
    this.AssociatedObject.Loaded += new RoutedEventHandler(this.AssociatedObject_Loaded);
    this.AssociatedObject.LostFocus += new RoutedEventHandler(this.AssociatedObject_LostFocus);
    this.AssociatedObject.PreviewKeyDown += new KeyEventHandler(this.AssociatedObject_PreviewKeyDown);
    this.UpdateRootVisualEvent();
  }

  protected override void OnDetaching()
  {
    if (this.rootVisual != null)
    {
      this.rootVisual.PreviewMouseDown -= new MouseButtonEventHandler(this.RootVisual_PreviewMouseDown);
      this.rootVisual = (UIElement) null;
    }
    this.AssociatedObject.Loaded -= new RoutedEventHandler(this.AssociatedObject_Loaded);
    this.AssociatedObject.LostFocus -= new RoutedEventHandler(this.AssociatedObject_LostFocus);
    this.AssociatedObject.PreviewKeyDown -= new KeyEventHandler(this.AssociatedObject_PreviewKeyDown);
    base.OnDetaching();
  }

  private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
  {
    this.UpdateRootVisualEvent();
  }

  private void AssociatedObject_PreviewKeyDown(object sender, KeyEventArgs e)
  {
    bool flag = e.Key == Key.Return;
    TextBox associatedObject = this.AssociatedObject;
    if ((associatedObject != null ? (associatedObject.AcceptsReturn ? 1 : 0) : 0) != 0)
      flag = false;
    if (!(e.Key == Key.Escape | flag))
      return;
    e.Handled = true;
    this.Apply();
  }

  public void Apply()
  {
    if (this.AssociatedObject.IsMouseCaptured)
      this.AssociatedObject.ReleaseMouseCapture();
    this.Text = this.AssociatedObject.Text;
    if (!this.AssociatedObject.IsFocused || this.AssociatedObject.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next)))
      return;
    Keyboard.ClearFocus();
  }

  private void AssociatedObject_LostFocus(object sender, RoutedEventArgs e) => this.Apply();

  private void RootVisual_PreviewMouseDown(object sender, MouseButtonEventArgs e)
  {
    TextBox associatedObject = this.AssociatedObject;
    if (associatedObject == null)
      return;
    Point position = e.GetPosition((IInputElement) associatedObject);
    if (VisualTreeHelper.HitTest((Visual) associatedObject, position)?.VisualHit != null)
      return;
    this.Apply();
  }

  private void UpdateRootVisualEvent()
  {
    if (this.rootVisual != null)
    {
      this.rootVisual.PreviewMouseDown -= new MouseButtonEventHandler(this.RootVisual_PreviewMouseDown);
      this.rootVisual = (UIElement) null;
    }
    TextBox associatedObject = this.AssociatedObject;
    if (associatedObject == null || !this.ApplyWhenClickEmpty)
      return;
    PresentationSource presentationSource = PresentationSource.FromVisual((Visual) associatedObject);
    if (presentationSource != null)
      this.rootVisual = presentationSource.RootVisual as UIElement;
    if (this.rootVisual == null)
      return;
    this.rootVisual.PreviewMouseDown += new MouseButtonEventHandler(this.RootVisual_PreviewMouseDown);
  }

  public string Text
  {
    get => (string) this.GetValue(TextBoxEditBehavior.TextProperty);
    set => this.SetValue(TextBoxEditBehavior.TextProperty, (object) value);
  }

  private static void OnTextPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is TextBoxEditBehavior sender))
      return;
    if (sender.AssociatedObject == null)
      throw new ArgumentException("AssociatedObject");
    if (sender.AssociatedObject.Text != (string) e.NewValue)
      sender.AssociatedObject.Text = (string) e.NewValue;
    if (sender.innerSet)
      return;
    EventHandler textChanged = sender.TextChanged;
    if (textChanged == null)
      return;
    textChanged((object) sender, EventArgs.Empty);
  }

  public bool ApplyWhenClickEmpty
  {
    get => (bool) this.GetValue(TextBoxEditBehavior.ApplyWhenClickEmptyProperty);
    set => this.SetValue(TextBoxEditBehavior.ApplyWhenClickEmptyProperty, (object) value);
  }

  public event EventHandler TextChanged;
}
