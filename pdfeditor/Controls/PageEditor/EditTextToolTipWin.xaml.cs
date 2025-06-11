// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.PageEditor.EditTextToolTipWin
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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace pdfeditor.Controls.PageEditor;

public partial class EditTextToolTipWin : Window, IComponentConnector
{
  internal Button Btn_Converter;
  internal CheckBox ckb_ShowMsg;
  private bool _contentLoaded;

  public EditTextToolTipWin()
  {
    GAManager.SendEvent("PromoteWindow", "TEPDFtoWord", "Show", 1L);
    this.InitializeComponent();
  }

  private void Btn_Converter_Click(object sender, RoutedEventArgs e)
  {
    GAManager.SendEvent("PromoteWindow", "TEPDFtoWord", "BtnClick", 1L);
    Ioc.Default.GetRequiredService<MainViewModel>().ConverterCommands.DoPDFToWord("TEPDFtoWord");
  }

  private void ckb_ShowMsg_Checked(object sender, RoutedEventArgs e)
  {
    GAManager.SendEvent("PromoteWindow", "TEPDFtoWord", "Checked", 1L);
    ConfigManager.SetEditTextToolTipMsgFlag(true);
  }

  private void ckb_ShowMsg_Unchecked(object sender, RoutedEventArgs e)
  {
    GAManager.SendEvent("PromoteWindow", "TEPDFtoWord", "UnChecked", 1L);
    ConfigManager.SetEditTextToolTipMsgFlag(false);
  }

  private void Btn_EditAnyway_Click(object sender, RoutedEventArgs e) => this.Close();

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/pageeditor/edittexttooltipwin.xaml", UriKind.Relative));
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
        this.ckb_ShowMsg = (CheckBox) target;
        this.ckb_ShowMsg.Checked += new RoutedEventHandler(this.ckb_ShowMsg_Checked);
        this.ckb_ShowMsg.Unchecked += new RoutedEventHandler(this.ckb_ShowMsg_Unchecked);
      }
      else
        this._contentLoaded = true;
    }
    else
    {
      this.Btn_Converter = (Button) target;
      this.Btn_Converter.Click += new RoutedEventHandler(this.Btn_Converter_Click);
    }
  }
}
