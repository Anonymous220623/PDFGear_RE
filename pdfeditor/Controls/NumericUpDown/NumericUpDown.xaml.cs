// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.NumericUpDown.NumericUpDown
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Controls.NumericUpDown;

public partial class NumericUpDown : UserControl, IComponentConnector
{
  public static readonly DependencyProperty PrintCopiesProperty = DependencyProperty.Register(nameof (PrintCopies), typeof (int), typeof (pdfeditor.Controls.NumericUpDown.NumericUpDown), new PropertyMetadata((object) 1));
  internal TextBox tbox;
  internal Button UpBtn;
  internal Button DowbBtn;
  private bool _contentLoaded;

  public void Selection()
  {
    this.tbox.SelectAll();
    this.tbox.Focus();
  }

  public int PrintCopies
  {
    get => (int) this.GetValue(pdfeditor.Controls.NumericUpDown.NumericUpDown.PrintCopiesProperty);
    set => this.SetValue(pdfeditor.Controls.NumericUpDown.NumericUpDown.PrintCopiesProperty, (object) value);
  }

  public NumericUpDown()
  {
    this.InitializeComponent();
    this.DowbBtn.IsEnabled = false;
    this.DowbBtn.Background = (Brush) new SolidColorBrush((Color) ColorConverter.ConvertFromString("#D2D2D2"));
    this.UpBtn.Background = (Brush) new SolidColorBrush((Color) ColorConverter.ConvertFromString("#757575"));
  }

  private void UpBtn_Click(object sender, RoutedEventArgs e)
  {
    try
    {
      int num1 = int.Parse(this.tbox.Text);
      if (num1 >= 1000)
        return;
      int num2 = num1 + 1;
      if (num2 == 1000)
      {
        this.UpBtn.Background = (Brush) new SolidColorBrush((Color) ColorConverter.ConvertFromString("#D2D2D2"));
        this.UpBtn.IsEnabled = false;
      }
      this.PrintCopies = num2;
      this.tbox.Text = num2.ToString();
      this.DowbBtn.Background = (Brush) new SolidColorBrush((Color) ColorConverter.ConvertFromString("#757575"));
      this.DowbBtn.IsEnabled = true;
    }
    catch
    {
    }
  }

  private void DowbBtn_Click(object sender, RoutedEventArgs e)
  {
    try
    {
      int num1 = int.Parse(this.tbox.Text);
      if (num1 <= 1)
        return;
      int num2 = num1 - 1;
      if (num2 == 1)
      {
        this.DowbBtn.Background = (Brush) new SolidColorBrush((Color) ColorConverter.ConvertFromString("#D2D2D2"));
        this.DowbBtn.IsEnabled = false;
      }
      this.PrintCopies = num2;
      this.tbox.Text = num2.ToString();
      this.UpBtn.Background = (Brush) new SolidColorBrush((Color) ColorConverter.ConvertFromString("#757575"));
      this.UpBtn.IsEnabled = true;
    }
    catch
    {
    }
  }

  private void tbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
  {
    Regex regex = new Regex("[^0-9]+");
    e.Handled = regex.IsMatch(e.Text);
  }

  private void tbox_LostFocus(object sender, RoutedEventArgs e)
  {
    int result;
    int.TryParse(this.tbox.Text, out result);
    if (result == 0)
    {
      this.DowbBtn.Background = (Brush) new SolidColorBrush((Color) ColorConverter.ConvertFromString("#D2D2D2"));
      this.DowbBtn.IsEnabled = false;
    }
    this.PrintCopies = result;
    this.tbox.Text = result.ToString();
  }

  private void tbox_TextChanged(object sender, TextChangedEventArgs e)
  {
    if (!this.IsLoaded)
      return;
    int result;
    int.TryParse(this.tbox.Text, out result);
    if (result > 1)
    {
      this.DowbBtn.Background = (Brush) new SolidColorBrush((Color) ColorConverter.ConvertFromString("#757575"));
      this.DowbBtn.IsEnabled = true;
      if (result < 1000)
      {
        this.UpBtn.Background = (Brush) new SolidColorBrush((Color) ColorConverter.ConvertFromString("#757575"));
        this.UpBtn.IsEnabled = true;
      }
      else
      {
        this.UpBtn.Background = (Brush) new SolidColorBrush((Color) ColorConverter.ConvertFromString("#D2D2D2"));
        this.UpBtn.IsEnabled = false;
      }
    }
    else
    {
      this.DowbBtn.Background = (Brush) new SolidColorBrush((Color) ColorConverter.ConvertFromString("#D2D2D2"));
      this.DowbBtn.IsEnabled = false;
    }
  }

  private void UserControl_Loaded(object sender, RoutedEventArgs e)
  {
  }

  private void CommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = false;
    e.Handled = true;
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/numericupdown/numericupdown.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        ((FrameworkElement) target).Loaded += new RoutedEventHandler(this.UserControl_Loaded);
        break;
      case 2:
        this.tbox = (TextBox) target;
        this.tbox.LostFocus += new RoutedEventHandler(this.tbox_LostFocus);
        this.tbox.PreviewTextInput += new TextCompositionEventHandler(this.tbox_PreviewTextInput);
        this.tbox.TextChanged += new TextChangedEventHandler(this.tbox_TextChanged);
        break;
      case 3:
        ((CommandBinding) target).CanExecute += new CanExecuteRoutedEventHandler(this.CommandCanExecute);
        break;
      case 4:
        this.UpBtn = (Button) target;
        this.UpBtn.Click += new RoutedEventHandler(this.UpBtn_Click);
        break;
      case 5:
        this.DowbBtn = (Button) target;
        this.DowbBtn.Click += new RoutedEventHandler(this.DowbBtn_Click);
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
