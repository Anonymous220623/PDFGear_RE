// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Screenshots.ScreenshotOcrToolbar
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace pdfeditor.Controls.Screenshots;

public partial class ScreenshotOcrToolbar : UserControl, IComponentConnector
{
  public static readonly DependencyProperty ScreenshotDialogProperty = DependencyProperty.Register(nameof (ScreenshotDialog), typeof (ScreenshotDialog), typeof (ScreenshotOcrToolbar), new PropertyMetadata((PropertyChangedCallback) null));
  internal Button CancelButton;
  internal Button AcceptButton;
  private bool _contentLoaded;

  public ScreenshotOcrToolbar() => this.InitializeComponent();

  public ScreenshotDialog ScreenshotDialog
  {
    get => (ScreenshotDialog) this.GetValue(ScreenshotOcrToolbar.ScreenshotDialogProperty);
    set => this.SetValue(ScreenshotOcrToolbar.ScreenshotDialogProperty, (object) value);
  }

  private void AcceptButton_Click(object sender, RoutedEventArgs e)
  {
    this.ScreenshotDialog?.CompleteExtractTextAsync(true);
  }

  private void CancelButton_Click(object sender, RoutedEventArgs e)
  {
    this.ScreenshotDialog?.Close();
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/screenshots/screenshotocrtoolbar.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    if (connectionId != 1)
    {
      if (connectionId == 2)
      {
        this.AcceptButton = (Button) target;
        this.AcceptButton.Click += new RoutedEventHandler(this.AcceptButton_Click);
      }
      else
        this._contentLoaded = true;
    }
    else
    {
      this.CancelButton = (Button) target;
      this.CancelButton.Click += new RoutedEventHandler(this.CancelButton_Click);
    }
  }
}
