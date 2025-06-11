// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Screenshots.FontSizeControl
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace pdfeditor.Controls.Screenshots;

public partial class FontSizeControl : UserControl, IComponentConnector
{
  public static readonly DependencyProperty SelectedFontSizeProperty = DependencyProperty.Register(nameof (SelectedFontSize), typeof (double), typeof (FontSizeControl), new PropertyMetadata((object) 12.0));
  public static readonly DependencyProperty FontSizeUnitProperty = DependencyProperty.Register(nameof (FontSizeUnit), typeof (string), typeof (FontSizeControl), new PropertyMetadata((object) ""));
  public static readonly DependencyProperty IsShowAutoProperty = DependencyProperty.Register(nameof (IsShowAuto), typeof (bool), typeof (FontSizeControl), new PropertyMetadata((object) false));
  public static readonly DependencyProperty IsAutoProperty = DependencyProperty.Register(nameof (IsAuto), typeof (bool), typeof (FontSizeControl), new PropertyMetadata((object) false));
  internal FontSizeControl _this;
  internal KSPopupButton popupBtn_fontSize;
  internal CheckBox ck_isShoAuto;
  internal TextBlock txt_auto;
  internal KSSelectableButton btn_auto;
  internal ItemsControl listbox_fontSize;
  private bool _contentLoaded;

  public FontSizeControl()
  {
    this.InitializeComponent();
    this.popupBtn_fontSize.Loaded += new RoutedEventHandler(this.PopupBtn_fontSize_Loaded);
  }

  public double SelectedFontSize
  {
    get => (double) this.GetValue(FontSizeControl.SelectedFontSizeProperty);
    set => this.SetValue(FontSizeControl.SelectedFontSizeProperty, (object) value);
  }

  public string FontSizeUnit
  {
    get => (string) this.GetValue(FontSizeControl.FontSizeUnitProperty);
    set => this.SetValue(FontSizeControl.FontSizeUnitProperty, (object) value);
  }

  public bool IsShowAuto
  {
    get => (bool) this.GetValue(FontSizeControl.IsShowAutoProperty);
    set => this.SetValue(FontSizeControl.IsShowAutoProperty, (object) value);
  }

  public bool IsAuto
  {
    get => (bool) this.GetValue(FontSizeControl.IsAutoProperty);
    set => this.SetValue(FontSizeControl.IsAutoProperty, (object) value);
  }

  private void PopupBtn_fontSize_Loaded(object sender, RoutedEventArgs e)
  {
    if (this.popupBtn_fontSize.InnerPopup == null)
      return;
    this.popupBtn_fontSize.InnerPopup.Opened += new EventHandler(this.InnerPopup_Opened);
  }

  private void InnerPopup_Opened(object sender, EventArgs e)
  {
  }

  public IReadOnlyList<double> FontSizeList { get; } = (IReadOnlyList<double>) new List<double>()
  {
    8.0,
    10.0,
    12.0,
    14.0,
    16.0,
    18.0,
    20.0,
    22.0,
    24.0,
    26.0,
    28.0,
    36.0,
    48.0,
    56.0,
    72.0,
    96.0
  };

  private void btn_fontSize_item_Click(object sender, RoutedEventArgs e)
  {
    this.IsAuto = false;
    this.SelectedFontSize = (double) (sender as FrameworkElement).DataContext;
    this.popupBtn_fontSize.ClosePopup();
  }

  private void btn_auto_Click(object sender, RoutedEventArgs e)
  {
    this.IsAuto = true;
    this.popupBtn_fontSize.ClosePopup();
  }

  private void btn_fontSize_item_Loaded(object sender, RoutedEventArgs e)
  {
    if (this.IsAuto)
      return;
    FrameworkElement frameworkElement = sender as FrameworkElement;
    if ((double) frameworkElement.DataContext != this.SelectedFontSize)
      return;
    frameworkElement.BringIntoView();
  }

  private void btn_auto_Loaded(object sender, RoutedEventArgs e)
  {
    if (!this.IsAuto)
      return;
    this.btn_auto.BringIntoView();
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/screenshots/fontsizecontrol.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  internal Delegate _CreateDelegate(Type delegateType, string handler)
  {
    return Delegate.CreateDelegate(delegateType, (object) this, handler);
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this._this = (FontSizeControl) target;
        break;
      case 2:
        this.popupBtn_fontSize = (KSPopupButton) target;
        break;
      case 3:
        this.ck_isShoAuto = (CheckBox) target;
        break;
      case 4:
        this.txt_auto = (TextBlock) target;
        break;
      case 5:
        this.btn_auto = (KSSelectableButton) target;
        break;
      case 6:
        this.listbox_fontSize = (ItemsControl) target;
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
