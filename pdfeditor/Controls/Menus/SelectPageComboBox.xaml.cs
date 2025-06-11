// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Menus.SelectPageComboBox
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using pdfeditor.Models.Thumbnails;
using pdfeditor.Utils;
using pdfeditor.Utils.Behaviors;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Controls.Menus;

public partial class SelectPageComboBox : UserControl, IComponentConnector
{
  private bool innerSet;
  public static readonly DependencyProperty PagesProperty = DependencyProperty.Register(nameof (Pages), typeof (PdfPageEditList), typeof (SelectPageComboBox), new PropertyMetadata((object) null, new PropertyChangedCallback(SelectPageComboBox.OnPagesPropertyChanged)));
  public static readonly DependencyProperty PreviewGridViewProperty = DependencyProperty.Register(nameof (PreviewGridView), typeof (PdfPagePreviewGridView), typeof (SelectPageComboBox), new PropertyMetadata((PropertyChangedCallback) null));
  internal TextBox _TextBox;
  internal TextBoxEditBehavior _TextBoxEditBehavior;
  internal Button _ArrowButton;
  internal ComboBox _SelectPageComboBox;
  private bool _contentLoaded;

  public SelectPageComboBox() => this.InitializeComponent();

  private void _SelectPageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    this.ApplyComboBoxSelectedItem();
    this._SelectPageComboBox.SelectedIndex = -1;
  }

  private void _TextBoxEditBehavior_TextChanged(object sender, EventArgs e)
  {
    if (this.innerSet)
      return;
    this.ApplyInput();
  }

  private void _ArrowButton_Click(object sender, RoutedEventArgs e)
  {
    this._SelectPageComboBox.IsDropDownOpen = true;
  }

  public PdfPageEditList Pages
  {
    get => (PdfPageEditList) this.GetValue(SelectPageComboBox.PagesProperty);
    set => this.SetValue(SelectPageComboBox.PagesProperty, (object) value);
  }

  private static void OnPagesPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is SelectPageComboBox selectPageComboBox) || object.Equals(e.NewValue, e.OldValue))
      return;
    if (e.OldValue is INotifyPropertyChanged oldValue)
      WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>.RemoveHandler(oldValue, "PropertyChanged", new EventHandler<PropertyChangedEventArgs>(selectPageComboBox.OnPagesPropertyChanged));
    if (e.NewValue is INotifyPropertyChanged newValue)
      WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>.AddHandler(newValue, "PropertyChanged", new EventHandler<PropertyChangedEventArgs>(selectPageComboBox.OnPagesPropertyChanged));
    selectPageComboBox.UpdateDisplayText();
  }

  public PdfPagePreviewGridView PreviewGridView
  {
    get => (PdfPagePreviewGridView) this.GetValue(SelectPageComboBox.PreviewGridViewProperty);
    set => this.SetValue(SelectPageComboBox.PreviewGridViewProperty, (object) value);
  }

  private void OnPagesPropertyChanged(object sender, PropertyChangedEventArgs args)
  {
    if (this.innerSet)
      return;
    this.UpdateDisplayText();
  }

  private void UpdateDisplayText()
  {
    try
    {
      this.innerSet = true;
      if (this.Pages == null || this.Pages.Count == 0 || this.Pages.SelectedItems.Count == 0)
        this._TextBoxEditBehavior.Text = "";
      else if (this.Pages.AllItemSelected.GetValueOrDefault())
      {
        int displayPageIndex = this.Pages[this.Pages.Count - 1].DisplayPageIndex;
        if (displayPageIndex == 1)
          this._TextBoxEditBehavior.Text = "1";
        else
          this._TextBoxEditBehavior.Text = $"1-{displayPageIndex}";
      }
      else
        this._TextBoxEditBehavior.Text = this.Pages.SelectedItems.Select<PdfPageEditListModel, int>((Func<PdfPageEditListModel, int>) (c => c.PageIndex)).ConvertToRange();
    }
    finally
    {
      this.innerSet = false;
    }
  }

  private void ApplyInput()
  {
    if (this.Pages == null)
      return;
    if (this.Pages.Count == 0)
      return;
    try
    {
      this.innerSet = true;
      string range = this._TextBoxEditBehavior.Text.Replace("，", ",");
      if (string.IsNullOrEmpty(range))
      {
        this.Pages.AllItemSelected = new bool?(false);
      }
      else
      {
        int[] pageIndexes;
        if (new HashSet<string>(this._SelectPageComboBox.Items.OfType<ComboBoxItem>().Select<ComboBoxItem, object>((Func<ComboBoxItem, object>) (c => c.Tag)).OfType<string>().Where<string>((Func<string, bool>) (c => !string.IsNullOrEmpty(c))).Distinct<string>()).Contains(range) || !PdfObjectExtensions.TryParsePageRange(range, out pageIndexes, out int _))
          return;
        GAManager.SendEvent("PageView", "SelectPage", "CustomPages", 1L);
        this.Pages.AllItemSelected = new bool?(false);
        int count = this.Pages.Count;
        int index1 = -1;
        foreach (int index2 in pageIndexes)
        {
          if (index2 >= 0 && index2 < count)
          {
            if (index1 == -1)
              index1 = index2;
            if (this.IsItemInPreviewViewport(this.Pages[index2]) == SelectPageComboBox.ItemViewportRelationship.Contains)
              index1 = -2;
            this.Pages[index2].Selected = true;
          }
        }
        if (this.PreviewGridView == null || index1 < 0)
          return;
        this.BringIndexIntoGridViewViewport(index1);
      }
    }
    finally
    {
      this.innerSet = false;
    }
  }

  private void ApplyComboBoxSelectedItem()
  {
    if (this.Pages == null)
      return;
    if (this.Pages.Count == 0)
      return;
    try
    {
      this.innerSet = true;
      ComboBoxItem selectedItem = (ComboBoxItem) this._SelectPageComboBox.SelectedItem;
      if (selectedItem == null)
        return;
      switch (selectedItem.Tag as string)
      {
        case "UnselectAll":
          this._TextBoxEditBehavior.Text = pdfeditor.Properties.Resources.SelectPageUnselectAllItem;
          this.Pages.AllItemSelected = new bool?(false);
          GAManager.SendEvent("PageView", "SelectPage", "UnselectAll", 1L);
          break;
        case "AllPages":
          this._TextBoxEditBehavior.Text = pdfeditor.Properties.Resources.SelectPageAllPagesItem;
          this.Pages.AllItemSelected = new bool?(true);
          GAManager.SendEvent("PageView", "SelectPage", "AllPages", 1L);
          break;
        case "AllEvenPages":
          this._TextBoxEditBehavior.Text = pdfeditor.Properties.Resources.SelectPageAllEvenPagesItem;
          this.Pages.AllItemSelected = new bool?(false);
          int count1 = this.Pages.Count;
          for (int index = 0; index < count1; ++index)
          {
            if (index % 2 == 1)
              this.Pages[index].Selected = true;
          }
          GAManager.SendEvent("PageView", "SelectPage", "AllEvenPages", 1L);
          break;
        case "AllOddPages":
          this._TextBoxEditBehavior.Text = pdfeditor.Properties.Resources.SelectPageAllOddPagesItem;
          this.Pages.AllItemSelected = new bool?(false);
          int count2 = this.Pages.Count;
          for (int index = 0; index < count2; ++index)
          {
            if (index % 2 == 0)
              this.Pages[index].Selected = true;
          }
          GAManager.SendEvent("PageView", "SelectPage", "AllOddPages", 1L);
          break;
      }
    }
    finally
    {
      this.innerSet = false;
    }
  }

  private SelectPageComboBox.ItemViewportRelationship IsItemInPreviewViewport(
    PdfPageEditListModel item)
  {
    PdfPagePreviewGridView previewGridView = this.PreviewGridView;
    if (previewGridView == null || !previewGridView.IsLoaded)
      return SelectPageComboBox.ItemViewportRelationship.None;
    PdfPagePreviewGridViewItem previewGridViewItem = (PdfPagePreviewGridViewItem) previewGridView.ItemContainerGenerator.ContainerFromItem((object) item);
    if (previewGridViewItem == null)
      return SelectPageComboBox.ItemViewportRelationship.None;
    Rect rect1 = new Rect(0.0, 0.0, previewGridView.ActualWidth, previewGridView.ActualHeight);
    Rect rect2 = previewGridViewItem.TransformToVisual((Visual) previewGridView).TransformBounds(new Rect(0.0, 0.0, previewGridViewItem.ActualWidth, previewGridViewItem.ActualHeight));
    if (!rect1.IntersectsWith(rect2))
      return SelectPageComboBox.ItemViewportRelationship.None;
    return rect1.Contains(rect2) ? SelectPageComboBox.ItemViewportRelationship.Contains : SelectPageComboBox.ItemViewportRelationship.IntersectsWith;
  }

  private bool BringIndexIntoGridViewViewport(int index)
  {
    PdfPagePreviewGridView previewGridView = this.PreviewGridView;
    PdfPageEditList pages = this.Pages;
    if (previewGridView == null || pages == null || index < 0 || index >= pages.Count)
      return false;
    previewGridView.ScrollIntoView((object) pages[index]);
    return false;
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/menus/selectpagecombobox.xaml", UriKind.Relative));
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
        this._TextBox = (TextBox) target;
        break;
      case 2:
        this._TextBoxEditBehavior = (TextBoxEditBehavior) target;
        break;
      case 3:
        this._ArrowButton = (Button) target;
        this._ArrowButton.Click += new RoutedEventHandler(this._ArrowButton_Click);
        break;
      case 4:
        this._SelectPageComboBox = (ComboBox) target;
        this._SelectPageComboBox.SelectionChanged += new SelectionChangedEventHandler(this._SelectPageComboBox_SelectionChanged);
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }

  private enum ItemViewportRelationship
  {
    None,
    IntersectsWith,
    Contains,
  }
}
