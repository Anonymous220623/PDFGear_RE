// Decompiled with JetBrains decompiler
// Type: pdfconverter.Controls.MergePDFUserControl
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using CommomLib.Commom;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using pdfconverter.Models;
using pdfconverter.ViewModels;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

#nullable disable
namespace pdfconverter.Controls;

public partial class MergePDFUserControl : UserControl, IComponentConnector, IStyleConnector
{
  private MergeFileItem DragData;
  private ListViewItem viewItem;
  private AdornerLayer adornerlaryer;
  private int MouseOverIndex;
  internal Button addFilesMergeBtn;
  internal Button clearFilesMergeBtn;
  internal MListView lsvFiles;
  internal Button mergeOpenFile;
  internal Button mergeOpenFileInExploreBtn;
  internal Button mergeDestPathBtn;
  internal ButtonEx mergeBtn;
  private bool _contentLoaded;

  public MergePDFUserControl()
  {
    this.InitializeComponent();
    this.DataContext = (object) Ioc.Default.GetRequiredService<MergePDFUCViewModel>();
  }

  private void UserControl_Loaded(object sender, RoutedEventArgs e)
  {
    GridView view = this.lsvFiles.View as GridView;
    double num = 0.0;
    for (int index = view.Columns.Count - 1; index > 0; --index)
      num += view.Columns[index].ActualWidth;
    view.Columns[0].Width = this.ActualWidth - num - 10.0;
  }

  private void lsvFiles_Drop(object sender, DragEventArgs e)
  {
    if (e.Data.GetDataPresent(DataFormats.FileDrop))
    {
      string[] data = (string[]) e.Data.GetData(DataFormats.FileDrop);
      MergePDFUCViewModel dataContext = this.DataContext as MergePDFUCViewModel;
      foreach (string fileName in data)
      {
        if (!string.IsNullOrWhiteSpace(fileName))
        {
          DocsPathUtils.WriteFilesPathJson("unknow", (object) fileName);
          dataContext.AddOneFileToMergeList(fileName);
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

  private void FrameworkElement_Drop(object sender, DragEventArgs e)
  {
    if (e.Effects != DragDropEffects.Move)
      return;
    MergePDFUCViewModel dataContext = this.DataContext as MergePDFUCViewModel;
    MergeFileItem mergeFileItem = (MergeFileItem) null;
    if (this.DragData != null)
      mergeFileItem = this.DragData;
    MergeFileItem content = (sender as ListViewItem).Content as MergeFileItem;
    MergeFileItem dropedFileItem = mergeFileItem;
    MergeFileItem oriFileItem = content;
    dataContext.changeMergeFileItem(dropedFileItem, oriFileItem);
    this.lsvFiles.SelectedItem = (object) mergeFileItem;
    int num1 = this.lsvFiles.Items.IndexOf((object) mergeFileItem);
    int num2 = this.lsvFiles.Items.IndexOf((object) content);
    if (num1 > num2)
    {
      if (num1 >= this.lsvFiles.Items.Count - 1)
        return;
      this.lsvFiles.ScrollIntoView(this.lsvFiles.Items[num1 + 1]);
    }
    else
    {
      if (num1 <= 0)
        return;
      this.lsvFiles.ScrollIntoView(this.lsvFiles.Items[num1 - 1]);
    }
  }

  private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    this.DragData = (MergeFileItem) null;
    if (!(sender is ListViewItem listViewItem))
      return;
    this.DragData = listViewItem.Content as MergeFileItem;
    this.viewItem = listViewItem;
    this.MouseOverIndex = this.lsvFiles.Items.IndexOf((object) this.DragData);
  }

  private void lsvFiles_MouseMove(object sender, MouseEventArgs e)
  {
    if (this.DragData != null && e.LeftButton == MouseButtonState.Pressed)
    {
      this.Dispatcher.BeginInvoke((Delegate) (() =>
      {
        DragDropAdorner dragDropAdorner = new DragDropAdorner((UIElement) this.viewItem);
        this.adornerlaryer = AdornerLayer.GetAdornerLayer((Visual) this.lsvFiles);
        this.adornerlaryer.Add((Adorner) dragDropAdorner);
        int num = (int) DragDrop.DoDragDrop((DependencyObject) this.lsvFiles, (object) this.DragData, DragDropEffects.Move);
        this.adornerlaryer?.Remove((Adorner) dragDropAdorner);
        this.adornerlaryer = (AdornerLayer) null;
        this.MouseOverIndex = 0;
      }));
    }
    else
    {
      this.DragData = (MergeFileItem) null;
      this.viewItem = (ListViewItem) null;
    }
  }

  private void ListViewItem_DragOver(object sender, DragEventArgs e)
  {
  }

  private void TextBox_MouseMove(object sender, MouseEventArgs e)
  {
    this.OnMouseMove(e);
    if (this.DragData == null || e.LeftButton != MouseButtonState.Pressed)
      return;
    e.Handled = true;
  }

  private void TextBox_TextInput(object sender, TextCompositionEventArgs e)
  {
    Regex regex = new Regex("[^0-9.-]+");
    e.Handled = regex.IsMatch(e.Text);
  }

  private void TextBox_Error(object sender, ValidationErrorEventArgs e)
  {
    TextBox element = sender as TextBox;
    if (Validation.GetErrors((DependencyObject) element).Count<ValidationError>() <= 0)
      return;
    int num = (int) ModernMessageBox.Show(pdfconverter.Properties.Resources.FileConvertMsgInvaildPageNum, UtilManager.GetProductName());
    element.Text = (element.DataContext as MergeFileItem).PageFrom.ToString();
    element.Focus();
  }

  private void TextBox_Error_1(object sender, ValidationErrorEventArgs e)
  {
    TextBox element = sender as TextBox;
    if (Validation.GetErrors((DependencyObject) element).Count<ValidationError>() <= 0)
      return;
    int num = (int) ModernMessageBox.Show(pdfconverter.Properties.Resources.FileConvertMsgInvaildPageNum, UtilManager.GetProductName());
    element.Text = (element.DataContext as MergeFileItem).PageTo.ToString();
    element.Focus();
  }

  private void lsvFiles_PreviewQueryContinueDrag(object sender, QueryContinueDragEventArgs e)
  {
    this.adornerlaryer?.Update();
  }

  private void ListViewItem_DragEnter(object sender, DragEventArgs e)
  {
    if (!(sender is ListViewItem listViewItem))
      return;
    MergeFileItem mergeFileItem = (MergeFileItem) null;
    if (this.DragData != null)
      mergeFileItem = this.DragData;
    MergeFileItem content = listViewItem.Content as MergeFileItem;
    this.lsvFiles.SelectedItem = (object) content;
    this.lsvFiles.Items.IndexOf((object) mergeFileItem);
    int num = this.lsvFiles.Items.IndexOf((object) content);
    if (num > this.MouseOverIndex && num < this.lsvFiles.Items.Count - 1)
    {
      this.lsvFiles.ScrollIntoView(this.lsvFiles.Items[num + 1]);
      this.MouseOverIndex = num;
    }
    else
    {
      if (num >= this.MouseOverIndex || num <= 0)
        return;
      this.lsvFiles.ScrollIntoView(this.lsvFiles.Items[num - 1]);
      this.MouseOverIndex = num;
    }
  }

  private void ListViewItem_DragLeave(object sender, DragEventArgs e)
  {
    if (!(sender is ListViewItem))
      return;
    this.lsvFiles.SelectedItem = (object) null;
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfconverter;component/controls/mergepdfusercontrol.xaml", UriKind.Relative));
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
        this.addFilesMergeBtn = (Button) target;
        break;
      case 3:
        this.clearFilesMergeBtn = (Button) target;
        break;
      case 4:
        this.lsvFiles = (MListView) target;
        break;
      case 8:
        this.mergeOpenFile = (Button) target;
        break;
      case 9:
        this.mergeOpenFileInExploreBtn = (Button) target;
        break;
      case 10:
        this.mergeDestPathBtn = (Button) target;
        break;
      case 11:
        this.mergeBtn = (ButtonEx) target;
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
    switch (connectionId)
    {
      case 5:
        ((Style) target).Setters.Add((SetterBase) new EventSetter()
        {
          Event = UIElement.DropEvent,
          Handler = (Delegate) new DragEventHandler(this.FrameworkElement_Drop)
        });
        ((Style) target).Setters.Add((SetterBase) new EventSetter()
        {
          Event = UIElement.PreviewMouseLeftButtonDownEvent,
          Handler = (Delegate) new MouseButtonEventHandler(this.ListViewItem_PreviewMouseLeftButtonDown)
        });
        ((Style) target).Setters.Add((SetterBase) new EventSetter()
        {
          Event = UIElement.DragOverEvent,
          Handler = (Delegate) new DragEventHandler(this.ListViewItem_DragOver)
        });
        ((Style) target).Setters.Add((SetterBase) new EventSetter()
        {
          Event = UIElement.DragEnterEvent,
          Handler = (Delegate) new DragEventHandler(this.ListViewItem_DragEnter)
        });
        ((Style) target).Setters.Add((SetterBase) new EventSetter()
        {
          Event = UIElement.DragLeaveEvent,
          Handler = (Delegate) new DragEventHandler(this.ListViewItem_DragLeave)
        });
        break;
      case 6:
        ((UIElement) target).MouseMove += new MouseEventHandler(this.TextBox_MouseMove);
        ((UIElement) target).PreviewTextInput += new TextCompositionEventHandler(this.TextBox_TextInput);
        ((UIElement) target).AddHandler(Validation.ErrorEvent, (Delegate) new EventHandler<ValidationErrorEventArgs>(this.TextBox_Error));
        break;
      case 7:
        ((UIElement) target).PreviewTextInput += new TextCompositionEventHandler(this.TextBox_TextInput);
        ((UIElement) target).AddHandler(Validation.ErrorEvent, (Delegate) new EventHandler<ValidationErrorEventArgs>(this.TextBox_Error_1));
        ((UIElement) target).MouseMove += new MouseEventHandler(this.TextBox_MouseMove);
        break;
    }
  }
}
