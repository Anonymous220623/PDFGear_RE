// Decompiled with JetBrains decompiler
// Type: pdfconverter.OCRSettingsWindow
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using CommomLib.Commom;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace pdfconverter;

public partial class OCRSettingsWindow : Window, IComponentConnector
{
  public bool ret;
  internal ListView languagesListView;
  internal Button okBtn;
  internal Button cancelBtn;
  private bool _contentLoaded;

  public OCRSettingsWindow()
  {
    this.InitializeComponent();
    this.InitLanguages();
  }

  private void InitLanguages()
  {
    foreach (KeyValuePair<OCRLanguageID, string> keyValuePair in ConvertManager.OCRLanguagesL10n)
      this.languagesListView.Items.Add((object) keyValuePair.Value);
  }

  private void updateSelectedLanguage()
  {
    this.Dispatcher.Invoke((Action) (() =>
    {
      OCRLanguageID ocrLanguageId = ConvertManager.GetOCRLanguageID();
      this.languagesListView.SelectedIndex = (int) ocrLanguageId;
      int index = (int) (ocrLanguageId + 5);
      if (index > this.languagesListView.Items.Count - 1)
        index = this.languagesListView.Items.Count - 1;
      this.languagesListView.ScrollIntoView(this.languagesListView.SelectedItem);
      ((UIElement) this.languagesListView.ItemContainerGenerator.ContainerFromIndex(this.languagesListView.SelectedIndex))?.Focus();
      this.languagesListView.ScrollIntoView(this.languagesListView.Items[index]);
    }));
  }

  private void Window_Loaded(object sender, RoutedEventArgs e)
  {
    try
    {
      this.updateSelectedLanguage();
    }
    catch
    {
    }
  }

  private void OkBtn_Click(object sender, RoutedEventArgs e)
  {
    try
    {
      int selectedIndex = this.languagesListView.SelectedIndex;
      if (selectedIndex < 0 || selectedIndex >= ConvertManager.OCRLanguages.Count || !ConvertManager.OCRLanguages.ContainsKey((OCRLanguageID) selectedIndex))
        return;
      ConfigManager.SetOCRLanguageID(selectedIndex);
    }
    catch
    {
    }
    this.ret = true;
    this.Close();
  }

  private void CancelBtn_Click(object sender, RoutedEventArgs e)
  {
    this.ret = false;
    this.Close();
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfconverter;component/ocrsettingswindow.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        ((FrameworkElement) target).Loaded += new RoutedEventHandler(this.Window_Loaded);
        break;
      case 2:
        this.languagesListView = (ListView) target;
        break;
      case 3:
        this.okBtn = (Button) target;
        this.okBtn.Click += new RoutedEventHandler(this.OkBtn_Click);
        break;
      case 4:
        this.cancelBtn = (Button) target;
        this.cancelBtn.Click += new RoutedEventHandler(this.CancelBtn_Click);
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
