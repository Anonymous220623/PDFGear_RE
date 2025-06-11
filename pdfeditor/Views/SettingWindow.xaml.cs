// Decompiled with JetBrains decompiler
// Type: pdfeditor.Views.SettingWindow
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using pdfeditor.ViewModels;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace pdfeditor.Views;

public partial class SettingWindow : Window, IComponentConnector
{
  public static readonly DependencyProperty IsAutoSaveProperty = DependencyProperty.Register(nameof (IsAutoSave), typeof (bool), typeof (SettingWindow), new PropertyMetadata((object) true));
  public static readonly DependencyProperty SpanMinutesProperty = DependencyProperty.Register(nameof (SpanMinutesProperty), typeof (string), typeof (SettingWindow), new PropertyMetadata((object) "5"));
  internal CheckBox ckbisAutoSave;
  internal TextBox txtFrequrymins;
  internal Button btnCancel;
  internal Button btnOk;
  private bool _contentLoaded;

  public bool IsAutoSave
  {
    get => (bool) this.GetValue(SettingWindow.IsAutoSaveProperty);
    set => this.SetValue(SettingWindow.IsAutoSaveProperty, (object) value);
  }

  public string SpanMinutes
  {
    get => (string) this.GetValue(SettingWindow.SpanMinutesProperty);
    set => this.SetValue(SettingWindow.SpanMinutesProperty, (object) value);
  }

  public SettingWindow()
  {
    this.InitializeComponent();
    this.GetConfig();
  }

  private void GetConfig()
  {
    CommomLib.Config.ConfigModels.AutoSaveModel result = ConfigManager.GetAutoSaveAsync(new CancellationToken()).GetAwaiter().GetResult();
    if (result == null)
      return;
    this.IsAutoSave = result.IsAutoSave;
    this.SpanMinutes = result.FrequencyMins.ToString();
  }

  private void btnCancel_Click(object sender, RoutedEventArgs e) => this.Close();

  private void btnOk_Click(object sender, RoutedEventArgs e)
  {
    bool isAutoSave = this.IsAutoSave;
    int result = 5;
    if (!int.TryParse(this.SpanMinutes, out result))
    {
      int num = (int) ModernMessageBox.Show("the number is not valid.", UtilManager.GetProductName());
      this.SpanMinutes = "5";
    }
    else if (isAutoSave && result == 0)
    {
      int num = (int) ModernMessageBox.Show("Set the numbers greater than 0.", UtilManager.GetProductName());
      this.SpanMinutes = "5";
    }
    else
    {
      ConfigManager.SetAutoSaveAsync(isAutoSave, result);
      MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
      if (isAutoSave)
      {
        requiredService.AutoSaveModel.IsAuto = isAutoSave;
        requiredService.AutoSaveModel.SpanMinutes = result;
        if (requiredService.CanSave)
          pdfeditor.AutoSaveRestore.AutoSaveManager.GetInstance().Start(result);
      }
      else
      {
        requiredService.AutoSaveModel.IsAuto = false;
        pdfeditor.AutoSaveRestore.AutoSaveManager.GetInstance().Stop();
      }
      this.Close();
    }
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/views/settingwindow.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.ckbisAutoSave = (CheckBox) target;
        break;
      case 2:
        this.txtFrequrymins = (TextBox) target;
        break;
      case 3:
        this.btnCancel = (Button) target;
        this.btnCancel.Click += new RoutedEventHandler(this.btnCancel_Click);
        break;
      case 4:
        this.btnOk = (Button) target;
        this.btnOk.Click += new RoutedEventHandler(this.btnOk_Click);
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
