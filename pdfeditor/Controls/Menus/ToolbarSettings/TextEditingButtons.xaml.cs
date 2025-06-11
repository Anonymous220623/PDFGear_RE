// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Menus.ToolbarSettings.TextEditingButtons
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Views;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using pdfeditor.Models.Menus.ToolbarSettings;
using pdfeditor.ViewModels;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace pdfeditor.Controls.Menus.ToolbarSettings;

public partial class TextEditingButtons : UserControl, IComponentConnector
{
  public static readonly DependencyProperty ModelProperty = DependencyProperty.Register(nameof (Model), typeof (ToolbarSettingItemTextEditingButtonsModel), typeof (TextEditingButtons), new PropertyMetadata((PropertyChangedCallback) null));
  internal Button btnExit;
  internal Button btnSupport;
  private bool _contentLoaded;

  public TextEditingButtons() => this.InitializeComponent();

  public ToolbarSettingItemTextEditingButtonsModel Model
  {
    get
    {
      return (ToolbarSettingItemTextEditingButtonsModel) this.GetValue(TextEditingButtons.ModelProperty);
    }
    set => this.SetValue(TextEditingButtons.ModelProperty, (object) value);
  }

  private void btnExit_Click(object sender, RoutedEventArgs e) => this.Model?.ExecuteCommand();

  private void btnSupport_Click(object sender, RoutedEventArgs e)
  {
    string documentPath = Ioc.Default.GetRequiredService<MainViewModel>().DocumentWrapper.DocumentPath;
    FeedbackWindow feedbackWindow = new FeedbackWindow();
    feedbackWindow.Owner = App.Current.MainWindow;
    feedbackWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
    feedbackWindow.source = "EditText";
    if (!string.IsNullOrEmpty(documentPath))
    {
      feedbackWindow.flist.Add(documentPath);
      feedbackWindow.showAttachmentCB(true);
    }
    feedbackWindow.ShowDialog();
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/menus/toolbarsettings/texteditingbuttons.xaml", UriKind.Relative));
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
        this.btnSupport = (Button) target;
        this.btnSupport.Click += new RoutedEventHandler(this.btnSupport_Click);
      }
      else
        this._contentLoaded = true;
    }
    else
    {
      this.btnExit = (Button) target;
      this.btnExit.Click += new RoutedEventHandler(this.btnExit_Click);
    }
  }
}
