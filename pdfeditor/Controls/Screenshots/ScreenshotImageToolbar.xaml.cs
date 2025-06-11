// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Screenshots.ScreenshotImageToolbar
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Controls.Screenshots;

public partial class ScreenshotImageToolbar : UserControl, IComponentConnector
{
  public static readonly DependencyProperty ScreenshotDialogProperty = DependencyProperty.Register(nameof (ScreenshotDialog), typeof (ScreenshotDialog), typeof (ScreenshotImageToolbar), new PropertyMetadata((PropertyChangedCallback) null));
  internal ScreenshotImageToolbar _this;
  internal CheckBox DrawRectangle;
  internal CheckBox DrawCircle;
  internal CheckBox DrawArrow;
  internal CheckBox DrawInk;
  internal CheckBox DrawText;
  internal Button UndoButton;
  internal Button ExtractTextBtn;
  internal Button CopyBtn;
  internal Button DownloadBtn;
  internal Button CancelButton;
  internal Button AcceptButton;
  internal DrawSettingToolbar DrawSettingToolbar;
  private bool _contentLoaded;

  public ScreenshotImageToolbar()
  {
    this.InitializeComponent();
    this.DrawSettingToolbar.PropertyChanged += new PropertyChangedEventHandler(this.DrawSettingToolbar_PropertyChanged);
  }

  public ScreenshotDialog ScreenshotDialog
  {
    get => (ScreenshotDialog) this.GetValue(ScreenshotImageToolbar.ScreenshotDialogProperty);
    set
    {
      if (this.ScreenshotDialog != null)
        this.ScreenshotDialog.PropertyChanged -= new PropertyChangedEventHandler(this.ScreenshotDialog_PropertyChanged);
      this.SetValue(ScreenshotImageToolbar.ScreenshotDialogProperty, (object) value);
      this.ScreenshotDialog.PropertyChanged += new PropertyChangedEventHandler(this.ScreenshotDialog_PropertyChanged);
    }
  }

  private void ScreenshotDialog_PropertyChanged(object sender, PropertyChangedEventArgs e)
  {
    if (e.PropertyName == "DrawControlMode")
    {
      DrawControlMode drawControlMode = this.ScreenshotDialog.DrawControlMode;
      this.DrawSettingToolbar.DrawControlMode = drawControlMode;
      if (drawControlMode == DrawControlMode.None)
        this.DrawSettingToolbar.Visibility = Visibility.Collapsed;
      else
        this.DrawSettingToolbar.Visibility = Visibility.Visible;
    }
    else if (e.PropertyName == "CurrentBrush")
      this.DrawSettingToolbar.Color = ((SolidColorBrush) this.ScreenshotDialog.CurrentBrush).Color;
    else if (e.PropertyName == "CurrentThickness")
    {
      this.DrawSettingToolbar.Thickness = this.ScreenshotDialog.CurrentThickness;
    }
    else
    {
      if (!(e.PropertyName == "CurrentFontSize"))
        return;
      this.DrawSettingToolbar.DrawFontSize = this.ScreenshotDialog.CurrentFontSize;
    }
  }

  private void DrawSettingToolbar_PropertyChanged(object sender, PropertyChangedEventArgs e)
  {
    if (!(sender is DrawSettingToolbar drawSettingToolbar))
      return;
    if (e.PropertyName == "Color")
    {
      SolidColorBrush solidColorBrush = new SolidColorBrush(drawSettingToolbar.Color);
      this.ScreenshotDialog.CurrentBrush = (Brush) solidColorBrush;
      this.ScreenshotDialog.SetDrawControlBrush(this.ScreenshotDialog.SelectedDrawControl, (Brush) solidColorBrush, true);
    }
    else if (e.PropertyName == "Thickness")
    {
      this.ScreenshotDialog.CurrentThickness = drawSettingToolbar.Thickness;
      this.ScreenshotDialog.SetDrawControlThickness(this.ScreenshotDialog.SelectedDrawControl, drawSettingToolbar.Thickness, true);
    }
    else
    {
      if (!(e.PropertyName == "DrawFontSize"))
        return;
      this.ScreenshotDialog.CurrentFontSize = drawSettingToolbar.DrawFontSize;
      this.ScreenshotDialog.SetDrawTextFontSize(this.ScreenshotDialog.SelectedDrawControl, drawSettingToolbar.DrawFontSize, true);
    }
  }

  private async void DownloadButton_Click(object sender, RoutedEventArgs e)
  {
    ((UIElement) sender).IsEnabled = false;
    await this.ScreenshotDialog?.SaveImageAsync();
    ((UIElement) sender).IsEnabled = true;
    GAManager.SendEvent("Screenshot", "Save", "Count", 1L);
  }

  private async void CopyButton_Click(object sender, RoutedEventArgs e)
  {
    ((UIElement) sender).IsEnabled = false;
    await this.ScreenshotDialog?.CopyToClipboardAsync();
    ((UIElement) sender).IsEnabled = true;
    GAManager.SendEvent("Screenshot", "Copy", "Count", 1L);
  }

  private async void AcceptButton_Click(object sender, RoutedEventArgs e)
  {
    ((UIElement) sender).IsEnabled = false;
    await this.ScreenshotDialog?.CompleteImageAsync();
    ((UIElement) sender).IsEnabled = true;
    GAManager.SendEvent("Screenshot", "Ok", "Count", 1L);
  }

  private void CancelButton_Click(object sender, RoutedEventArgs e)
  {
    this.ScreenshotDialog?.Close();
    GAManager.SendEvent("Screenshot", "Cancel", "Count", 1L);
  }

  private void UndoButton_Click(object sender, RoutedEventArgs e)
  {
    this.ScreenshotDialog?.UndoDrawControl();
  }

  private async void ExtractTextBtn_Click(object sender, RoutedEventArgs e)
  {
    this.ScreenshotDialog?.ExtractText();
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/screenshots/screenshotimagetoolbar.xaml", UriKind.Relative));
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
        this._this = (ScreenshotImageToolbar) target;
        break;
      case 2:
        this.DrawRectangle = (CheckBox) target;
        break;
      case 3:
        this.DrawCircle = (CheckBox) target;
        break;
      case 4:
        this.DrawArrow = (CheckBox) target;
        break;
      case 5:
        this.DrawInk = (CheckBox) target;
        break;
      case 6:
        this.DrawText = (CheckBox) target;
        break;
      case 7:
        this.UndoButton = (Button) target;
        this.UndoButton.Click += new RoutedEventHandler(this.UndoButton_Click);
        break;
      case 8:
        this.ExtractTextBtn = (Button) target;
        this.ExtractTextBtn.Click += new RoutedEventHandler(this.ExtractTextBtn_Click);
        break;
      case 9:
        this.CopyBtn = (Button) target;
        this.CopyBtn.Click += new RoutedEventHandler(this.CopyButton_Click);
        break;
      case 10:
        this.DownloadBtn = (Button) target;
        this.DownloadBtn.Click += new RoutedEventHandler(this.DownloadButton_Click);
        break;
      case 11:
        this.CancelButton = (Button) target;
        this.CancelButton.Click += new RoutedEventHandler(this.CancelButton_Click);
        break;
      case 12:
        this.AcceptButton = (Button) target;
        this.AcceptButton.Click += new RoutedEventHandler(this.AcceptButton_Click);
        break;
      case 13:
        this.DrawSettingToolbar = (DrawSettingToolbar) target;
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
