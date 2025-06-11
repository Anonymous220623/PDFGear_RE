// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Screenshots.DrawSettingToolbar
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

public partial class DrawSettingToolbar : UserControl, INotifyPropertyChanged, IComponentConnector
{
  public static readonly DependencyProperty DrawControlModeProperty = DependencyProperty.Register(nameof (DrawControlMode), typeof (DrawControlMode), typeof (DrawSettingToolbar), new PropertyMetadata((PropertyChangedCallback) null));
  private double thickness;
  private Color color;
  private double drawFontSize;
  internal DrawSettingToolbar _this;
  private bool _contentLoaded;

  public DrawControlMode DrawControlMode
  {
    get => (DrawControlMode) this.GetValue(DrawSettingToolbar.DrawControlModeProperty);
    set
    {
      this.SetValue(DrawSettingToolbar.DrawControlModeProperty, (object) value);
      this.RaisePropertyChanged(nameof (DrawControlMode));
    }
  }

  public double Thickness
  {
    get => this.thickness;
    set
    {
      this.thickness = value;
      this.RaisePropertyChanged(nameof (Thickness));
      ConfigManager.SetScreenshotThickness(this.thickness);
    }
  }

  public Color Color
  {
    get => this.color;
    set
    {
      this.color = value;
      this.RaisePropertyChanged(nameof (Color));
      ConfigManager.SetScreenshotColor(this.color);
    }
  }

  public double DrawFontSize
  {
    get => this.drawFontSize;
    set
    {
      this.drawFontSize = value;
      this.RaisePropertyChanged(nameof (DrawFontSize));
      ConfigManager.SetScreenshotFontSize(this.drawFontSize);
    }
  }

  public DrawSettingToolbar()
  {
    this.InitializeComponent();
    this.Thickness = ConfigManager.GetScreenshotThickness();
    this.Color = ConfigManager.GetScreenshotColor();
    this.DrawFontSize = ConfigManager.GetScreenshotFontSize();
  }

  public event PropertyChangedEventHandler PropertyChanged;

  private void RaisePropertyChanged(string propertyName)
  {
    PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
    if (propertyChanged == null)
      return;
    propertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/screenshots/drawsettingtoolbar.xaml", UriKind.Relative));
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
    if (connectionId == 1)
      this._this = (DrawSettingToolbar) target;
    else
      this._contentLoaded = true;
  }
}
