// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.StampEditWin
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using pdfeditor.ViewModels;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Controls;

public partial class StampEditWin : Window, IComponentConnector
{
  private pdfeditor.ViewModels.StampTextModel stampText;
  internal Grid root;
  internal TextBox txt_Text;
  internal StackPanel colorSelecters;
  internal RadioButton CustomColorRadioButton;
  internal Button btnCancel;
  internal Button btnOk;
  private bool _contentLoaded;

  public StampEditWin()
  {
    this.InitializeComponent();
    this.stampText = new pdfeditor.ViewModels.StampTextModel()
    {
      FontColor = "#FF20C48F"
    };
    this.Init();
  }

  public StampEditWin(pdfeditor.ViewModels.StampTextModel textModel)
  {
    this.InitializeComponent();
    this.Title = pdfeditor.Properties.Resources.WinCustomizeStampEditTiltle;
    this.stampText = textModel;
    this.Init();
  }

  public IStampTextModel StampTextModel { get; private set; }

  private void Init()
  {
    this.txt_Text.Text = this.stampText.TextContent ?? "";
    this.txt_Text.SelectAll();
    this.txt_Text.Focus();
    this.btnOk.Click += (RoutedEventHandler) ((o, e) =>
    {
      Color? nullable = this.colorSelecters.Children.OfType<RadioButton>().FirstOrDefault<RadioButton>((Func<RadioButton, bool>) (c => c.IsChecked.GetValueOrDefault())).Background is SolidColorBrush background2 ? new Color?(background2.Color) : new Color?();
      if (!nullable.HasValue)
        nullable = new Color?((Color) ColorConverter.ConvertFromString("#FF20C48F"));
      Color color = nullable.Value;
      this.stampText.TextContent = this.txt_Text.Text.Trim();
      this.stampText.FontColor = $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
      if (string.IsNullOrEmpty(this.stampText.TextContent))
      {
        int num1 = (int) ModernMessageBox.Show("Text cannot be empty.", UtilManager.GetProductName());
      }
      else if (this.stampText.TextContent.Trim().Length > 50)
      {
        int num2 = (int) ModernMessageBox.Show(pdfeditor.Properties.Resources.WinCustomizeStampMaxCharactersMsg, UtilManager.GetProductName());
      }
      else
      {
        this.StampTextModel = (IStampTextModel) this.stampText;
        this.DialogResult = new bool?(true);
      }
    });
    this.btnCancel.Click += (RoutedEventHandler) ((o, e) =>
    {
      this.StampTextModel = (IStampTextModel) null;
      this.DialogResult = new bool?(false);
      this.Close();
    });
    Color? nullable1 = new Color?();
    try
    {
      if (!string.IsNullOrEmpty(this.stampText.FontColor))
        nullable1 = new Color?((Color) ColorConverter.ConvertFromString(this.stampText.FontColor));
    }
    catch
    {
    }
    if (!nullable1.HasValue)
      nullable1 = new Color?((Color) ColorConverter.ConvertFromString("#FF20C48F"));
    IEnumerable<RadioButton> radioButtons = this.colorSelecters.Children.OfType<RadioButton>().Where<RadioButton>((Func<RadioButton, bool>) (c => c != this.CustomColorRadioButton));
    bool flag = false;
    foreach (RadioButton radioButton in radioButtons)
    {
      if (radioButton.Background is SolidColorBrush background3 && StampEditWin.IsSameColor(background3.Color, nullable1.Value))
      {
        radioButton.IsChecked = new bool?(true);
        flag = true;
        break;
      }
    }
    if (flag)
      return;
    this.CustomColorRadioButton.Background = (Brush) new SolidColorBrush(nullable1.Value);
    this.CustomColorRadioButton.Visibility = Visibility.Visible;
    this.CustomColorRadioButton.IsChecked = new bool?(true);
  }

  private static bool IsSameColor(Color left, Color right)
  {
    return Math.Abs((int) left.A - (int) right.A) < 2 && Math.Abs((int) left.R - (int) right.R) < 2 && Math.Abs((int) left.G - (int) right.G) < 2 && Math.Abs((int) left.B - (int) right.B) < 2;
  }

  private void txt_Text_TextChanged(object sender, TextChangedEventArgs e)
  {
    this.btnOk.IsEnabled = !string.IsNullOrEmpty(this.txt_Text.Text);
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/stampeditwin.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.root = (Grid) target;
        break;
      case 2:
        this.txt_Text = (TextBox) target;
        this.txt_Text.TextChanged += new TextChangedEventHandler(this.txt_Text_TextChanged);
        break;
      case 3:
        this.colorSelecters = (StackPanel) target;
        break;
      case 4:
        this.CustomColorRadioButton = (RadioButton) target;
        break;
      case 5:
        this.btnCancel = (Button) target;
        break;
      case 6:
        this.btnOk = (Button) target;
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
