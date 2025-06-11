// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.FilterByAnnotationsControl
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Patagames.Pdf.Net;
using pdfeditor.Models;
using pdfeditor.ViewModels;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace pdfeditor.Controls;

public partial class FilterByAnnotationsControl : UserControl, IComponentConnector, IStyleConnector
{
  public static readonly DependencyProperty TextListProperty = DependencyProperty.Register(nameof (TextList), typeof (ObservableCollection<AnnotationsFilterModel>), typeof (FilterByAnnotationsControl), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty AllCountProperty = DependencyProperty.Register(nameof (AllCount), typeof (int), typeof (FilterByAnnotationsControl), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty AllCheckProperty = DependencyProperty.Register(nameof (AllCheck), typeof (bool?), typeof (FilterByAnnotationsControl), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty DocumentProperty = DependencyProperty.Register(nameof (Document), typeof (PdfDocument), typeof (FilterByAnnotationsControl), new PropertyMetadata((PropertyChangedCallback) null));
  internal ListView lsvFilter;
  internal GridView gView;
  internal GridViewColumn NumHeader;
  private bool _contentLoaded;

  public FilterByAnnotationsControl()
  {
    this.InitializeComponent();
    this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.FilterByAnnotationsControl_IsVisibleChanged);
  }

  private void FilterByAnnotationsControl_IsVisibleChanged(
    object sender,
    DependencyPropertyChangedEventArgs e)
  {
    ObservableCollection<AnnotationsFilterModel> observableCollection = (ObservableCollection<AnnotationsFilterModel>) this.GetValue(FilterByAnnotationsControl.TextListProperty);
    this.AllCount = 0;
    foreach (AnnotationsFilterModel annotationsFilterModel in (Collection<AnnotationsFilterModel>) observableCollection)
      this.AllCount += annotationsFilterModel.Count;
  }

  public ObservableCollection<AnnotationsFilterModel> TextList
  {
    get
    {
      ObservableCollection<AnnotationsFilterModel> textList = (ObservableCollection<AnnotationsFilterModel>) this.GetValue(FilterByAnnotationsControl.TextListProperty);
      this.AllCount = 0;
      foreach (AnnotationsFilterModel annotationsFilterModel in (Collection<AnnotationsFilterModel>) textList)
        this.AllCount += annotationsFilterModel.Count;
      return textList;
    }
    set => this.SetValue(FilterByAnnotationsControl.TextListProperty, (object) value);
  }

  public int AllCount
  {
    get => (int) this.GetValue(FilterByAnnotationsControl.AllCountProperty);
    set => this.SetValue(FilterByAnnotationsControl.AllCountProperty, (object) value);
  }

  public bool? AllCheck
  {
    get => (bool?) this.GetValue(FilterByAnnotationsControl.AllCheckProperty);
    set => this.SetValue(FilterByAnnotationsControl.AllCheckProperty, (object) value);
  }

  public PdfDocument Document
  {
    get => (PdfDocument) this.GetValue(FilterByAnnotationsControl.DocumentProperty);
    set => this.SetValue(FilterByAnnotationsControl.DocumentProperty, (object) value);
  }

  private void fileItemListSelectAll(object sender, RoutedEventArgs e)
  {
    if (this.TextList != null && this.TextList.Count > 0)
    {
      foreach (AnnotationsFilterModel text in (Collection<AnnotationsFilterModel>) this.TextList)
        text.IsSelect = true;
    }
    Ioc.Default.GetRequiredService<MainViewModel>().FilterAnnotations();
  }

  private void fileItemListSelectNone(object sender, RoutedEventArgs e)
  {
    if (this.TextList != null && this.TextList.Count > 0)
    {
      foreach (AnnotationsFilterModel text in (Collection<AnnotationsFilterModel>) this.TextList)
        text.IsSelect = false;
    }
    Ioc.Default.GetRequiredService<MainViewModel>().FilterAnnotations();
  }

  private void fileItemChecked(object sender, RoutedEventArgs e)
  {
    if (this.AllCheck.GetValueOrDefault())
      e.Handled = true;
    this.JudgeSelectall();
    Ioc.Default.GetRequiredService<MainViewModel>().FilterAnnotations();
  }

  private void JudgeSelectall()
  {
    List<AnnotationsFilterModel> list = this.TextList.Where<AnnotationsFilterModel>((Func<AnnotationsFilterModel, bool>) (x => x.IsSelect)).ToList<AnnotationsFilterModel>();
    if (list == null)
      this.AllCheck = new bool?(false);
    // ISSUE: explicit non-virtual call
    if (list != null && __nonvirtual (list.Count) == 0)
    {
      this.AllCheck = new bool?(false);
    }
    else
    {
      // ISSUE: explicit non-virtual call
      if (list == null || __nonvirtual (list.Count) <= 0)
        return;
      if (list.Count == this.TextList.Count)
        this.AllCheck = new bool?(true);
      else
        this.AllCheck = new bool?();
    }
  }

  private void fileItemUnchecked(object sender, RoutedEventArgs e)
  {
    bool? allCheck = this.AllCheck;
    bool flag = false;
    if (allCheck.GetValueOrDefault() == flag & allCheck.HasValue)
      e.Handled = true;
    this.JudgeSelectall();
    Ioc.Default.GetRequiredService<MainViewModel>().FilterAnnotations();
  }

  private void fileItemCB_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
  {
  }

  private void OnListViewItemDoubleClick(object sender, MouseButtonEventArgs e)
  {
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/filterbyannotationscontrol.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.lsvFilter = (ListView) target;
        break;
      case 3:
        this.gView = (GridView) target;
        break;
      case 6:
        this.NumHeader = (GridViewColumn) target;
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
      case 2:
        ((Style) target).Setters.Add((SetterBase) new EventSetter()
        {
          Event = Control.MouseDoubleClickEvent,
          Handler = (Delegate) new MouseButtonEventHandler(this.OnListViewItemDoubleClick)
        });
        break;
      case 4:
        ((ToggleButton) target).Checked += new RoutedEventHandler(this.fileItemListSelectAll);
        ((ToggleButton) target).Unchecked += new RoutedEventHandler(this.fileItemListSelectNone);
        break;
      case 5:
        ((ToggleButton) target).Checked += new RoutedEventHandler(this.fileItemChecked);
        ((ToggleButton) target).Unchecked += new RoutedEventHandler(this.fileItemUnchecked);
        ((Control) target).PreviewMouseDoubleClick += new MouseButtonEventHandler(this.fileItemCB_PreviewMouseDoubleClick);
        break;
    }
  }
}
