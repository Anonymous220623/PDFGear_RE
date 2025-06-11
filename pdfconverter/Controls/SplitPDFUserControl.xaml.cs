// Decompiled with JetBrains decompiler
// Type: pdfconverter.Controls.SplitPDFUserControl
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using CommomLib.Commom;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using pdfconverter.ViewModels;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

#nullable disable
namespace pdfconverter.Controls;

public partial class SplitPDFUserControl : UserControl, IComponentConnector, IStyleConnector
{
  internal Button addFilesSplitBtn;
  internal Button clearFilesSplitBtn;
  internal MListView lsvFiles;
  internal Button splitDestPathBtn;
  internal ButtonEx SplitBtn;
  private bool _contentLoaded;

  public SplitPDFUserControl()
  {
    this.InitializeComponent();
    this.DataContext = (object) Ioc.Default.GetRequiredService<SplitPDFUCViewModel>();
  }

  private void UserControl_Loaded(object sender, RoutedEventArgs e)
  {
    GridView view = this.lsvFiles.View as GridView;
    double num = 0.0;
    for (int index = view.Columns.Count - 1; index > 0; --index)
      num += view.Columns[index].ActualWidth;
    view.Columns[0].Width = this.ActualWidth - num - 10.0;
  }

  private void splitModeHelpBtn_Click(object sender, RoutedEventArgs e)
  {
    int num = (int) ModernMessageBox.Show($"{pdfconverter.Properties.Resources.WinMergeSplitSplitModeCustomRangeHelptipMsg}\r\n{pdfconverter.Properties.Resources.WinMergeSplitSplitModeFixedRangeHelptipMsg}", UtilManager.GetProductName());
  }

  private void lsvFiles_Drop(object sender, DragEventArgs e)
  {
    if (e.Data.GetDataPresent(DataFormats.FileDrop))
    {
      string[] data = (string[]) e.Data.GetData(DataFormats.FileDrop);
      SplitPDFUCViewModel dataContext = this.DataContext as SplitPDFUCViewModel;
      foreach (string fileName in data)
      {
        if (!string.IsNullOrWhiteSpace(fileName))
        {
          DocsPathUtils.WriteFilesPathJson("unknow", (object) fileName);
          dataContext.AddOneFileToSplitList(fileName);
        }
        else
          break;
      }
    }
    e.Handled = true;
  }

  private void lsvFiles_DragEnter(object sender, DragEventArgs e)
  {
    e.Effects = !e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.None : DragDropEffects.Copy;
    e.Handled = true;
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfconverter;component/controls/splitpdfusercontrol.xaml", UriKind.Relative));
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
        ((FrameworkElement) target).Loaded += new RoutedEventHandler(this.UserControl_Loaded);
        break;
      case 2:
        this.addFilesSplitBtn = (Button) target;
        break;
      case 3:
        this.clearFilesSplitBtn = (Button) target;
        break;
      case 4:
        this.lsvFiles = (MListView) target;
        break;
      case 6:
        this.splitDestPathBtn = (Button) target;
        break;
      case 7:
        this.SplitBtn = (ButtonEx) target;
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IStyleConnector.Connect(int connectionId, object target)
  {
    if (connectionId != 5)
      return;
    ((ButtonBase) target).Click += new RoutedEventHandler(this.splitModeHelpBtn_Click);
  }
}
