// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Screenshots.OcrSelectLanguageDialog
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace pdfeditor.Controls.Screenshots;

public partial class OcrSelectLanguageDialog : Window, IComponentConnector
{
  internal ListBox LanguageListBox;
  internal Button OKButton;
  private bool _contentLoaded;

  public OcrSelectLanguageDialog(string cultureInfoName)
  {
    this.InitializeComponent();
    OcrSelectLanguageDialog.LanguageInfo[] array = OcrUtils.GetLanguageList().Select<(string, string), OcrSelectLanguageDialog.LanguageInfo>((Func<(string, string), OcrSelectLanguageDialog.LanguageInfo>) (c => new OcrSelectLanguageDialog.LanguageInfo(c.cultureInfoName, c.displayName))).ToArray<OcrSelectLanguageDialog.LanguageInfo>();
    this.LanguageListBox.ItemsSource = (IEnumerable) array;
    this.LanguageListBox.SelectedItem = (object) ((IEnumerable<OcrSelectLanguageDialog.LanguageInfo>) array).FirstOrDefault<OcrSelectLanguageDialog.LanguageInfo>((Func<OcrSelectLanguageDialog.LanguageInfo, bool>) (c => c.CultureInfoName == cultureInfoName));
    this.OKButton.IsEnabled = this.LanguageListBox.SelectedItem is OcrSelectLanguageDialog.LanguageInfo;
    this.Loaded += new RoutedEventHandler(this.OcrSelectLanguageDialog_Loaded);
  }

  public string SelectedCultureInfoName { get; private set; }

  public string SelectedDisplayName { get; private set; }

  private void OcrSelectLanguageDialog_Loaded(object sender, RoutedEventArgs e)
  {
    if (!(this.LanguageListBox.SelectedItem is OcrSelectLanguageDialog.LanguageInfo))
      return;
    this.LanguageListBox.ScrollIntoView(this.LanguageListBox.SelectedItem);
  }

  private void LanguageListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    this.OKButton.IsEnabled = this.LanguageListBox.SelectedItem is OcrSelectLanguageDialog.LanguageInfo;
  }

  private async void LanguageListBox_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
  {
    await Task.Delay(10);
    this.OKButton_Click((object) this.OKButton, (RoutedEventArgs) null);
  }

  private void OKButton_Click(object sender, RoutedEventArgs e)
  {
    if (!(this.LanguageListBox.SelectedItem is OcrSelectLanguageDialog.LanguageInfo selectedItem))
      return;
    this.SelectedCultureInfoName = selectedItem.CultureInfoName;
    this.SelectedDisplayName = selectedItem.DisplayName;
    this.DialogResult = new bool?(true);
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/screenshots/ocrselectlanguagedialog.xaml", UriKind.Relative));
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
        this.OKButton = (Button) target;
        this.OKButton.Click += new RoutedEventHandler(this.OKButton_Click);
      }
      else
        this._contentLoaded = true;
    }
    else
    {
      this.LanguageListBox = (ListBox) target;
      this.LanguageListBox.SelectionChanged += new SelectionChangedEventHandler(this.LanguageListBox_SelectionChanged);
      this.LanguageListBox.PreviewMouseDoubleClick += new MouseButtonEventHandler(this.LanguageListBox_PreviewMouseDoubleClick);
    }
  }

  private class LanguageInfo
  {
    public LanguageInfo(string cultureInfoName, string displayName)
    {
      this.CultureInfoName = cultureInfoName;
      this.DisplayName = displayName;
    }

    public string CultureInfoName { get; }

    public string DisplayName { get; }
  }
}
