// Decompiled with JetBrains decompiler
// Type: CommomLib.Controls.ModernMessageBox
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using CommomLib.Commom.MessageBoxHelper;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace CommomLib.Controls;

internal partial class ModernMessageBox : Window, IComponentConnector
{
  public static readonly DependencyProperty HighlightPrimaryButtonProperty = DependencyProperty.RegisterAttached("HighlightPrimaryButton", typeof (bool), typeof (ModernMessageBox), new PropertyMetadata((object) true));
  internal ContentPresenter ContentPresenter;
  internal Grid ButtonContainer;
  internal Button PrimaryButton;
  internal Button SecondaryButton;
  internal Button CancelButton;
  private bool _contentLoaded;

  public ModernMessageBox(ModernMessageBoxOptions options)
  {
    if (options == null)
      throw new ArgumentNullException(nameof (options));
    this.InitializeComponent();
    this.InitOptions(options);
    this.Closed += (EventHandler) ((s, a) => this.Dispatcher.InvokeAsync((Action) (() =>
    {
      if (Keyboard.FocusedElement != null)
        return;
      Window element = Application.Current.Windows.OfType<Window>().FirstOrDefault<Window>((Func<Window, bool>) (c => c.IsActive));
      if (element == null)
        return;
      Keyboard.Focus((IInputElement) element);
    }), DispatcherPriority.Loaded));
  }

  internal bool? DialogResultInternal { get; private set; }

  private void InitOptions(ModernMessageBoxOptions options)
  {
    if (options.Owner != null)
    {
      try
      {
        this.Owner = options.Owner;
      }
      catch
      {
      }
    }
    this.WindowStartupLocation = this.Owner == null ? WindowStartupLocation.CenterScreen : WindowStartupLocation.CenterOwner;
    this.Title = options.Caption ?? "";
    this.ContentPresenter.Content = options.MessageBoxContent;
    if (options.UIOverrides.IsButtonsReversed)
    {
      Grid.SetColumn((UIElement) this.PrimaryButton, 2);
      Grid.SetColumn((UIElement) this.SecondaryButton, 1);
      Grid.SetColumn((UIElement) this.CancelButton, 0);
    }
    if (!options.UIOverrides.HighlightPrimaryButton)
    {
      ModernMessageBox.SetHighlightPrimaryButton(this.PrimaryButton, false);
      ModernMessageBox.SetHighlightPrimaryButton(this.SecondaryButton, false);
      ModernMessageBox.SetHighlightPrimaryButton(this.CancelButton, false);
    }
    if (options.Button == MessageBoxButton.OK)
    {
      this.SecondaryButton.Visibility = Visibility.Collapsed;
      this.CancelButton.Visibility = Visibility.Collapsed;
      this.PrimaryButton.IsCancel = true;
      this.PrimaryButton.Content = options.UIOverrides.OKButtonContent ?? (object) ButtonTextHelper.GetText(MessageBoxResult.OK, options.CultureInfo, true);
    }
    else if (options.Button == MessageBoxButton.OKCancel)
    {
      this.SecondaryButton.Visibility = Visibility.Collapsed;
      this.CancelButton.IsCancel = true;
      this.PrimaryButton.Content = options.UIOverrides.OKButtonContent ?? (object) ButtonTextHelper.GetText(MessageBoxResult.OK, options.CultureInfo, true);
      this.CancelButton.Content = options.UIOverrides.CancelButtonContent ?? (object) ButtonTextHelper.GetText(MessageBoxResult.Cancel, options.CultureInfo, true);
      if (options.DefaultResult == MessageBoxResult.Cancel)
      {
        this.PrimaryButton.IsDefault = false;
        this.CancelButton.IsDefault = true;
      }
    }
    else if (options.Button == MessageBoxButton.YesNo)
    {
      this.CancelButton.IsCancel = false;
      this.CancelButton.Visibility = Visibility.Collapsed;
      this.PrimaryButton.Content = options.UIOverrides.YesButtonContent ?? (object) ButtonTextHelper.GetText(MessageBoxResult.Yes, options.CultureInfo, true);
      this.SecondaryButton.Content = options.UIOverrides.NoButtonContent ?? (object) ButtonTextHelper.GetText(MessageBoxResult.No, options.CultureInfo, true);
      this.SecondaryButton.IsCancel = true;
      if (options.DefaultResult == MessageBoxResult.No)
      {
        this.PrimaryButton.IsDefault = false;
        this.SecondaryButton.IsDefault = true;
      }
    }
    else if (options.Button == MessageBoxButton.YesNoCancel)
    {
      this.PrimaryButton.Content = options.UIOverrides.YesButtonContent ?? (object) ButtonTextHelper.GetText(MessageBoxResult.Yes, options.CultureInfo, true);
      this.SecondaryButton.Content = options.UIOverrides.NoButtonContent ?? (object) ButtonTextHelper.GetText(MessageBoxResult.No, options.CultureInfo, true);
      this.CancelButton.Content = options.UIOverrides.CancelButtonContent ?? (object) ButtonTextHelper.GetText(MessageBoxResult.Cancel, options.CultureInfo, true);
      if (options.DefaultResult == MessageBoxResult.No)
      {
        this.PrimaryButton.IsDefault = false;
        this.SecondaryButton.IsDefault = true;
      }
      else if (options.DefaultResult == MessageBoxResult.Cancel)
      {
        this.PrimaryButton.IsDefault = false;
        this.CancelButton.IsDefault = true;
      }
    }
    if (!options.UIOverrides.HideButtons)
      return;
    this.ButtonContainer.Visibility = Visibility.Collapsed;
  }

  private void PrimaryButton_Click(object sender, RoutedEventArgs e)
  {
    this.DialogResultInternal = new bool?(true);
    this.DialogResult = new bool?(true);
  }

  private void SecondaryButton_Click(object sender, RoutedEventArgs e)
  {
    this.DialogResultInternal = new bool?(false);
    this.DialogResult = new bool?(false);
  }

  private void CancelButton_Click(object sender, RoutedEventArgs e)
  {
    this.DialogResultInternal = new bool?();
    this.DialogResult = new bool?();
  }

  protected override void OnPreviewKeyDown(KeyEventArgs e)
  {
    if (e.Key == Key.C)
    {
      if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.None)
      {
        try
        {
          if (this.ContentPresenter.Content is string content2 && content2.Length > 0)
            Clipboard.SetDataObject((object) content2);
          else if (this.ContentPresenter.Content is DependencyObject content1)
          {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (TextBlock allTextBlock in this.GetAllTextBlocks(content1))
              stringBuilder.AppendLine(allTextBlock.Text);
            Clipboard.SetDataObject((object) stringBuilder.ToString());
          }
          e.Handled = true;
        }
        catch
        {
        }
      }
    }
    base.OnPreviewKeyDown(e);
  }

  private IEnumerable<TextBlock> GetAllTextBlocks(DependencyObject obj)
  {
    if (obj != null)
    {
      int childCount = VisualTreeHelper.GetChildrenCount(obj);
      for (int i = 0; i < childCount; ++i)
      {
        DependencyObject child = VisualTreeHelper.GetChild(obj, i);
        if (child is TextBlock allTextBlock1)
        {
          yield return allTextBlock1;
        }
        else
        {
          foreach (TextBlock allTextBlock in this.GetAllTextBlocks(child))
            yield return allTextBlock;
        }
      }
    }
  }

  public static bool GetHighlightPrimaryButton(Button obj)
  {
    return (bool) obj.GetValue(ModernMessageBox.HighlightPrimaryButtonProperty);
  }

  public static void SetHighlightPrimaryButton(Button obj, bool value)
  {
    obj.SetValue(ModernMessageBox.HighlightPrimaryButtonProperty, (object) value);
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/CommomLib;component/controls/modernmessagebox.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.ContentPresenter = (ContentPresenter) target;
        break;
      case 2:
        this.ButtonContainer = (Grid) target;
        break;
      case 3:
        this.PrimaryButton = (Button) target;
        this.PrimaryButton.Click += new RoutedEventHandler(this.PrimaryButton_Click);
        break;
      case 4:
        this.SecondaryButton = (Button) target;
        this.SecondaryButton.Click += new RoutedEventHandler(this.SecondaryButton_Click);
        break;
      case 5:
        this.CancelButton = (Button) target;
        this.CancelButton.Click += new RoutedEventHandler(this.CancelButton_Click);
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
