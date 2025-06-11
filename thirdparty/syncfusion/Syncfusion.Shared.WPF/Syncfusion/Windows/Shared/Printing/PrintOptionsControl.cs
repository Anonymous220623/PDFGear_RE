// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.Printing.PrintOptionsControl
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Windows.Shared.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

#nullable disable
namespace Syncfusion.Windows.Shared.Printing;

public class PrintOptionsControl : Control, IDisposable, INotifyPropertyChanged
{
  private bool isWired;
  private object previousMargin;
  private bool isdisposed;
  private bool IsInSuspend;
  private object previousPageSize;
  private List<PrintPageOrientation> orientationOptions;
  private List<PrintPageRangeSelection> pageRangeSelectionOptions;
  private List<PrintPageCollation> pageCollationOptions;
  private List<PrintPageMargin> marginOptions;
  private int _copiesCount = 1;
  private int _frompage = 1;
  private int _topage = 1;
  private List<PrintPageSize> pageSizeOptions;
  private PrintPageSize selectedPageSize;
  private PrintQueueOption selectedPrinter;
  private Collation selectedCollation = Collation.Collated;
  private PageRangeSelection selectedPageRange;
  private List<PrintQueueOption> printers;
  internal PrintManager printManager;
  internal PrintPreviewAreaControl printPreviewAreaControl;
  internal Thickness DefaultPageMargin = new Thickness(2.5, 2.5, 2.5, 2.5);
  internal ResourceWrapper resources = new ResourceWrapper();
  private Button Part_QuickPrintButton;
  private ComboBox Part_PapersComboBox;
  private ComboBox Part_MarginComboBox;
  private ComboBox Part_ScaleOptionComboBox;
  private ComboBox Part_OrientationCombombBox;
  private ComboBox PART_PrintersComboBox;
  private ComboBox PART_CollatedComboBox;
  private ComboBox PART_PageRangeSelectionComboBox;
  private TextBox Part_CopyCount_TextBox;
  private RepeatButton Part_CopyCount_UpButton;
  private RepeatButton Part_CopyCount_DownButton;
  private TextBox Part_FromPage_TextBox;
  private RepeatButton Part_FromPage_UpButton;
  private RepeatButton Part_FromPage_DownButton;
  private TextBox Part_ToPage_TextBox;
  private RepeatButton Part_ToPage_UpButton;
  private RepeatButton Part_ToPage_DownButton;
  public static readonly DependencyProperty SelectedScaleIndexProperty = DependencyProperty.Register(nameof (SelectedScaleIndex), typeof (int), typeof (PrintOptionsControl), new PropertyMetadata((object) 0, new PropertyChangedCallback(PrintOptionsControl.OnPrintScaleOptionChanged)));
  internal static readonly DependencyProperty PageMarginProperty = DependencyProperty.Register(nameof (PageMargin), typeof (Thickness), typeof (PrintOptionsControl), new PropertyMetadata((object) new Thickness(94.49), new PropertyChangedCallback(PrintOptionsControl.OnPrintMarginChanged)));
  internal static readonly DependencyProperty PageSizeProperty = DependencyProperty.Register(nameof (PageSize), typeof (Size), typeof (PrintOptionsControl), new PropertyMetadata((object) new Size(), new PropertyChangedCallback(PrintOptionsControl.OnPageSizeChanged)));
  public static readonly DependencyProperty PrintOrientationProperty = DependencyProperty.Register(nameof (PrintOrientation), typeof (PrintOrientation), typeof (PrintOptionsControl), new PropertyMetadata((object) PrintOrientation.Portrait, new PropertyChangedCallback(PrintOptionsControl.OnPrintOrientationChanged)));
  private ICommand printCommand;
  private ICommand quickPrintCommand;

  public PrintOptionsControl()
  {
    this.DefaultStyleKey = (object) typeof (PrintOptionsControl);
    this.Loaded += new RoutedEventHandler(this.OnPrintOptionsControlLoaded);
  }

  public List<PrintPageRangeSelection> PrintPageRangeSelectionOptions
  {
    get => this.UpdatePrintPageRangeSelectionValues();
  }

  public List<PrintPageCollation> PrintPageCollationOptions
  {
    get => this.UpdatePrintPageCollationValues();
  }

  public List<PrintPageOrientation> OrientationOptions => this.UpdateOrientationValues();

  public List<PrintScaleInfo> ScaleOptions => this.printManager.GetScaleOptions();

  public List<PrintPageMargin> MarginOptions
  {
    get => this.marginOptions;
    set
    {
      this.marginOptions = value;
      this.OnPropertyChanged(nameof (MarginOptions));
    }
  }

  public int CopiesCount
  {
    get => this._copiesCount;
    set
    {
      if (this._copiesCount == value)
        return;
      this._copiesCount = value;
      this.printManager.CopiesCount = value;
      this.OnPropertyChanged(nameof (CopiesCount));
    }
  }

  public int FromPage
  {
    get => this._frompage;
    set
    {
      if (this._frompage == value)
        return;
      this._frompage = value;
      this.UpdatePageRangeSelection();
      this.OnPropertyChanged(nameof (FromPage));
    }
  }

  public int ToPage
  {
    get => this._topage;
    set
    {
      if (this._topage == value)
        return;
      this._topage = value;
      this.UpdatePageRangeSelection();
      this.OnPropertyChanged(nameof (ToPage));
    }
  }

  public List<PrintPageSize> PageSizeOptions
  {
    get => this.pageSizeOptions;
    set
    {
      this.pageSizeOptions = value;
      this.OnPropertyChanged(nameof (PageSizeOptions));
    }
  }

  public List<PrintQueueOption> Printers
  {
    get => this.printers;
    internal set
    {
      if (this.printers == value)
        return;
      this.printers = value;
      this.OnPropertyChanged(nameof (Printers));
    }
  }

  public PrintQueueOption SelectedPrinter
  {
    get => this.selectedPrinter;
    set
    {
      if (this.selectedPrinter == value)
        return;
      this.selectedPrinter = value;
      this.OnPropertyChanged(nameof (SelectedPrinter));
    }
  }

  public Collation SelectedCollation
  {
    get => this.selectedCollation;
    set
    {
      if (this.selectedCollation == value)
        return;
      this.selectedCollation = value;
      this.OnPropertyChanged(nameof (SelectedCollation));
    }
  }

  public PageRangeSelection SelectedPrintPageRange
  {
    get => this.selectedPageRange;
    set
    {
      if (this.selectedPageRange == value)
        return;
      this.selectedPageRange = value;
      this.OnPropertyChanged(nameof (SelectedPrintPageRange));
    }
  }

  public PrintPageSize SelectedPageSize
  {
    get => this.selectedPageSize;
    set
    {
      if (this.selectedPageSize == value)
        return;
      this.selectedPageSize = value;
      this.OnPropertyChanged(nameof (SelectedPageSize));
    }
  }

  public int SelectedScaleIndex
  {
    get => (int) this.GetValue(PrintOptionsControl.SelectedScaleIndexProperty);
    set => this.SetValue(PrintOptionsControl.SelectedScaleIndexProperty, (object) value);
  }

  private static void OnPrintScaleOptionChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    PrintOptionsControl printOptionsControl = d as PrintOptionsControl;
    if (printOptionsControl.IsInSuspend || printOptionsControl.printManager == null)
      return;
    printOptionsControl.printManager.OnPrintScaleOptionChanged((int) e.NewValue);
  }

  internal Thickness PageMargin
  {
    get => (Thickness) this.GetValue(PrintOptionsControl.PageMarginProperty);
    set => this.SetValue(PrintOptionsControl.PageMarginProperty, (object) value);
  }

  private static void OnPrintMarginChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    PrintOptionsControl printOptionsControl = d as PrintOptionsControl;
    if (printOptionsControl.IsInSuspend || printOptionsControl.printManager == null)
      return;
    printOptionsControl.printManager.OnPrintMarginChanged((Thickness) e.NewValue);
  }

  internal Size PageSize
  {
    get => (Size) this.GetValue(PrintOptionsControl.PageSizeProperty);
    set => this.SetValue(PrintOptionsControl.PageSizeProperty, (object) value);
  }

  private static void OnPageSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    int num = (d as PrintOptionsControl).IsInSuspend ? 1 : 0;
  }

  public PrintOrientation PrintOrientation
  {
    get => (PrintOrientation) this.GetValue(PrintOptionsControl.PrintOrientationProperty);
    set => this.SetValue(PrintOptionsControl.PrintOrientationProperty, (object) value);
  }

  private static void OnPrintOrientationChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    PrintOptionsControl printOptionsControl = d as PrintOptionsControl;
    if (printOptionsControl.IsInSuspend || printOptionsControl.printManager == null || printOptionsControl.printManager.isSuspended || printOptionsControl.printManager.PageOrientation == (PrintOrientation) e.NewValue)
      return;
    printOptionsControl.printManager.OnPrintOrientationChanged((PrintOrientation) e.NewValue);
    printOptionsControl.printPreviewAreaControl.UpdateZoomFactor();
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.UnWireEvents();
    this.Part_QuickPrintButton = this.GetTemplateChild("Part_QuickPrintButton") as Button;
    this.Part_PapersComboBox = this.GetTemplateChild("PART_PapersComboBox") as ComboBox;
    this.Part_ScaleOptionComboBox = this.GetTemplateChild("PART_ScaleOptionComboBox") as ComboBox;
    this.Part_MarginComboBox = this.GetTemplateChild("PART_MarginComboBox") as ComboBox;
    this.Part_OrientationCombombBox = this.GetTemplateChild("PART_OrientationComboBox") as ComboBox;
    this.PART_PrintersComboBox = this.GetTemplateChild("PART_PrintersComboBox") as ComboBox;
    this.PART_CollatedComboBox = this.GetTemplateChild("PART_Collated") as ComboBox;
    this.PART_PageRangeSelectionComboBox = this.GetTemplateChild("PART_PageRangeSelection") as ComboBox;
    this.Part_CopyCount_TextBox = this.GetTemplateChild("Part_CopyTextBox") as TextBox;
    this.Part_CopyCount_UpButton = this.GetTemplateChild("Copy_upbutton") as RepeatButton;
    this.Part_CopyCount_DownButton = this.GetTemplateChild("Copy_downbutton") as RepeatButton;
    this.Part_CopyCount_TextBox.ToolTip = (object) new System.Windows.Controls.ToolTip();
    this.Part_FromPage_TextBox = this.GetTemplateChild("Part_FromPageTextBox") as TextBox;
    this.Part_FromPage_TextBox.ToolTip = (object) new System.Windows.Controls.ToolTip();
    this.Part_FromPage_UpButton = this.GetTemplateChild("FromPage_upbutton") as RepeatButton;
    this.Part_FromPage_DownButton = this.GetTemplateChild("FromPage_downbutton") as RepeatButton;
    this.Part_ToPage_TextBox = this.GetTemplateChild("Part_ToPageTextBox") as TextBox;
    this.Part_ToPage_TextBox.ToolTip = (object) new System.Windows.Controls.ToolTip();
    this.Part_ToPage_UpButton = this.GetTemplateChild("ToPage_upbutton") as RepeatButton;
    this.Part_ToPage_DownButton = this.GetTemplateChild("ToPage_downbutton") as RepeatButton;
    if (this.ScaleOptions == null || this.ScaleOptions.Count == 0)
      this.Part_ScaleOptionComboBox.Visibility = Visibility.Collapsed;
    else
      this.Part_ScaleOptionComboBox.Visibility = Visibility.Visible;
    this.WireEvents();
    this.UpdatePageMarginComboBox(PrintExtensions.PixelToCm(this.PageMargin));
  }

  private void UpdatePageRangeSelection()
  {
    if (this.FromPage != 0 && this.ToPage != 0 && this.FromPage != 1 || this.ToPage != this.printPreviewAreaControl.TotalPages)
    {
      this.SelectedPrintPageRange = PageRangeSelection.UserPages;
      this.printManager.PageRangeSelection = this.SelectedPrintPageRange;
    }
    else
    {
      this.SelectedPrintPageRange = PageRangeSelection.AllPages;
      this.printManager.PageRangeSelection = this.SelectedPrintPageRange;
    }
  }

  private List<PrintPageCollation> UpdatePrintPageCollationValues()
  {
    if (this.pageCollationOptions == null)
      this.pageCollationOptions = PrintExtensions.GetCollationList(this.resources);
    return this.pageCollationOptions;
  }

  private List<PrintPageRangeSelection> UpdatePrintPageRangeSelectionValues()
  {
    if (this.pageRangeSelectionOptions == null)
      this.pageRangeSelectionOptions = PrintExtensions.GetPageRangeSelectionList(this.resources);
    return this.pageRangeSelectionOptions;
  }

  private List<PrintPageOrientation> UpdateOrientationValues()
  {
    if (this.orientationOptions == null)
      this.orientationOptions = PrintExtensions.GetOrientationList(this.resources);
    return this.orientationOptions;
  }

  private List<PrintPageMargin> UpdateMarginValues()
  {
    if (this.marginOptions == null)
      this.marginOptions = PrintExtensions.GetMarginList(this);
    return this.marginOptions;
  }

  private void UpdatePaperSizeComboBox(Size size)
  {
    if (this.PageSizeOptions == null || this.Part_PapersComboBox == null)
      return;
    PrintPageSize printPageSize = this.PageSizeOptions.FirstOrDefault<PrintPageSize>((Func<PrintPageSize, bool>) (x => x.Size == new Size(Math.Round(size.Width, 2), Math.Round(size.Height, 2))));
    if (printPageSize == null)
    {
      this.Part_PapersComboBox.SelectedIndex = this.Part_PapersComboBox.Items.Count - 1;
      this.PageSizeOptions[this.Part_PapersComboBox.SelectedIndex].Size = new Size(Math.Round(size.Width, 2), Math.Round(size.Height, 2));
      this.printManager.PageSizeOptions[this.Part_PapersComboBox.SelectedIndex].Size = this.PageSizeOptions[this.Part_PapersComboBox.SelectedIndex].Size;
    }
    else
      this.Part_PapersComboBox.SelectedIndex = this.PageSizeOptions.IndexOf(printPageSize);
  }

  private void UpdatePageMarginComboBox(Thickness pageMargin)
  {
    if (this.MarginOptions == null || this.Part_MarginComboBox == null)
      return;
    pageMargin = new Thickness(Math.Round(pageMargin.Left, 2), Math.Round(pageMargin.Top, 2), Math.Round(pageMargin.Right, 2), Math.Round(pageMargin.Bottom, 2));
    PrintPageMargin printPageMargin = this.MarginOptions.FirstOrDefault<PrintPageMargin>((Func<PrintPageMargin, bool>) (x => x.Thickness == pageMargin));
    if (printPageMargin == null)
    {
      this.Part_MarginComboBox.SelectedIndex = this.Part_MarginComboBox.Items.Count - 1;
      this.MarginOptions[this.Part_MarginComboBox.Items.Count - 1].Thickness = pageMargin;
    }
    else
      this.Part_MarginComboBox.SelectedIndex = this.MarginOptions.IndexOf(printPageMargin);
  }

  internal void InitializePrintOptionsWindow()
  {
    this.IsInSuspend = true;
    this.MarginOptions = this.UpdateMarginValues();
    this.PageMargin = this.PageMargin != this.printManager.PageMargin ? this.printManager.PageMargin : this.PageMargin;
    this.PrintOrientation = this.PrintOrientation != this.printManager.PageOrientation ? this.printManager.PageOrientation : this.PrintOrientation;
    this.PageSize = this.PrintOrientation != PrintOrientation.Portrait ? new Size(this.printManager.GetPageHeight(), this.printManager.GetPageWidth()) : new Size(this.printManager.GetPageWidth(), this.printManager.GetPageHeight());
    this.SelectedScaleIndex = this.SelectedScaleIndex != this.printManager.SelectedScaleIndex ? this.printManager.SelectedScaleIndex : this.SelectedScaleIndex;
    this.SelectedCollation = this.SelectedCollation != this.printManager.Collation ? this.printManager.Collation : this.SelectedCollation;
    this.SelectedPrintPageRange = this.SelectedPrintPageRange != this.printManager.PageRangeSelection ? this.printManager.PageRangeSelection : this.SelectedPrintPageRange;
    this.FromPage = this.FromPage != this.printManager.FromPage ? this.printManager.FromPage : this.FromPage;
    this.ToPage = this.ToPage != this.printManager.ToPage ? this.printManager.ToPage : this.ToPage;
    this.CopiesCount = this.CopiesCount != this.printManager.CopiesCount ? this.printManager.CopiesCount : this.CopiesCount;
    if (this.ScaleOptions == null || this.ScaleOptions.Count == 0)
      this.Part_ScaleOptionComboBox.Visibility = Visibility.Collapsed;
    else
      this.Part_ScaleOptionComboBox.Visibility = Visibility.Visible;
    this.UpdatePageMarginComboBox(PrintExtensions.PixelToCm(this.PageMargin));
    this.IsInSuspend = false;
  }

  private void WireEvents()
  {
    this.MouseMove += new MouseEventHandler(this.PrintOptionsControl_MouseMove);
    if (this.Part_PapersComboBox != null)
    {
      this.Part_PapersComboBox.SelectionChanged += new SelectionChangedEventHandler(this.Part_PapersComboBox_SelectionChanged);
      this.Part_PapersComboBox.DropDownOpened += new EventHandler(this.Part_PapersComboBox_DropDownOpened);
      this.Part_PapersComboBox.DropDownClosed += new EventHandler(this.Part_PapersComboBox_DropDownClosed);
      if (this.Part_PapersComboBox.SelectedItem == null)
        this.Part_QuickPrintButton.IsEnabled = false;
    }
    if (this.Part_MarginComboBox != null)
    {
      this.Part_MarginComboBox.SelectionChanged += new SelectionChangedEventHandler(this.Part_MarginComboBoxSelectionChanged);
      this.Part_MarginComboBox.DropDownOpened += new EventHandler(this.Part_MarginComboBox_DropDownOpened);
      this.Part_MarginComboBox.DropDownClosed += new EventHandler(this.Part_MarginCombox_DropDownClosed);
    }
    if (this.Part_OrientationCombombBox != null)
      this.Part_OrientationCombombBox.SelectionChanged += new SelectionChangedEventHandler(this.Part_OrientationComboBox_SelectionChanged);
    if (this.PART_PrintersComboBox != null)
      this.PART_PrintersComboBox.SelectionChanged += new SelectionChangedEventHandler(this.PART_PrintersComboBox_SelectionChanged);
    if (this.PART_CollatedComboBox != null)
      this.PART_CollatedComboBox.SelectionChanged += new SelectionChangedEventHandler(this.PART_CollatedComboBox_SelectionChanged);
    if (this.PART_PageRangeSelectionComboBox != null)
      this.PART_PageRangeSelectionComboBox.SelectionChanged += new SelectionChangedEventHandler(this.PART_PageRangeSelectionComboBox_SelectionChanged);
    if (this.Part_CopyCount_TextBox != null)
    {
      this.Part_CopyCount_TextBox.LostFocus += new RoutedEventHandler(this.OnPartTextBoxLostFocus);
      this.Part_CopyCount_TextBox.MouseMove += new MouseEventHandler(this.Part_TextBox_MouseMove);
      this.Part_CopyCount_TextBox.KeyDown += new KeyEventHandler(this.Part_TextBox_KeyDown);
      this.Part_CopyCount_TextBox.PreviewKeyDown += new KeyEventHandler(this.TextBox_PreviewKeyDown);
      DataObject.AddPastingHandler((DependencyObject) this.Part_CopyCount_TextBox, new DataObjectPastingEventHandler(this.OnPasting));
    }
    if (this.Part_CopyCount_UpButton != null)
      this.Part_CopyCount_UpButton.Click += new RoutedEventHandler(this.Part_CopyCount_UpButton_Click);
    if (this.Part_CopyCount_DownButton != null)
      this.Part_CopyCount_DownButton.Click += new RoutedEventHandler(this.Part_CopyCount_DownButton_Click);
    if (this.Part_FromPage_TextBox != null)
    {
      this.Part_FromPage_TextBox.LostFocus += new RoutedEventHandler(this.OnPartTextBoxLostFocus);
      this.Part_FromPage_TextBox.MouseMove += new MouseEventHandler(this.Part_TextBox_MouseMove);
      this.Part_FromPage_TextBox.KeyDown += new KeyEventHandler(this.Part_TextBox_KeyDown);
      this.Part_FromPage_TextBox.PreviewKeyDown += new KeyEventHandler(this.TextBox_PreviewKeyDown);
      DataObject.AddPastingHandler((DependencyObject) this.Part_FromPage_TextBox, new DataObjectPastingEventHandler(this.OnPasting));
    }
    if (this.Part_FromPage_UpButton != null)
      this.Part_FromPage_UpButton.Click += new RoutedEventHandler(this.Part_FromPage_UpButton_Click);
    if (this.Part_FromPage_DownButton != null)
      this.Part_FromPage_DownButton.Click += new RoutedEventHandler(this.Part_FromPage_DownButton_Click);
    if (this.Part_ToPage_TextBox != null)
    {
      this.Part_ToPage_TextBox.LostFocus += new RoutedEventHandler(this.OnPartTextBoxLostFocus);
      this.Part_ToPage_TextBox.MouseMove += new MouseEventHandler(this.Part_TextBox_MouseMove);
      this.Part_ToPage_TextBox.KeyDown += new KeyEventHandler(this.Part_TextBox_KeyDown);
      this.Part_ToPage_TextBox.PreviewKeyDown += new KeyEventHandler(this.TextBox_PreviewKeyDown);
      DataObject.AddPastingHandler((DependencyObject) this.Part_ToPage_TextBox, new DataObjectPastingEventHandler(this.OnPasting));
    }
    if (this.Part_ToPage_UpButton != null)
      this.Part_ToPage_UpButton.Click += new RoutedEventHandler(this.Part_ToPage_UpButton_Click);
    if (this.Part_ToPage_DownButton != null)
      this.Part_ToPage_DownButton.Click += new RoutedEventHandler(this.Part_ToPage_DownButton_Click);
    this.Unloaded += new RoutedEventHandler(this.OnPrintOptionsControlUnloaded);
    this.isWired = true;
  }

  private void PART_PageRangeSelectionComboBox_SelectionChanged(
    object sender,
    SelectionChangedEventArgs e)
  {
    if (e.AddedItems == null || e.AddedItems.Count <= 0 || !(e.AddedItems[0] is PrintPageRangeSelection))
      return;
    this.printManager.PageRangeSelection = (e.AddedItems[0] as PrintPageRangeSelection).PageRangeSelection;
    if (this.printManager.PageRangeSelection != PageRangeSelection.AllPages || this.printPreviewAreaControl.TotalPages == 0)
      return;
    this.Part_FromPage_TextBox.Text = "1";
    this.FromPage = 1;
    this.printManager.FromPage = this.FromPage;
    this.Part_ToPage_TextBox.Text = this.printPreviewAreaControl.TotalPages.ToString();
    this.ToPage = this.printPreviewAreaControl.TotalPages;
    this.printManager.ToPage = this.ToPage;
  }

  private void PART_CollatedComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    if (e.AddedItems == null || e.AddedItems.Count <= 0 || !(e.AddedItems[0] is PrintPageCollation))
      return;
    this.printManager.Collation = (e.AddedItems[0] as PrintPageCollation).Collation;
  }

  private void Part_ToPage_DownButton_Click(object sender, RoutedEventArgs e)
  {
    this.ChangeToPageValue(false, this.Part_ToPage_TextBox);
  }

  private void Part_ToPage_UpButton_Click(object sender, RoutedEventArgs e)
  {
    this.ChangeToPageValue(true, this.Part_ToPage_TextBox);
  }

  private void Part_FromPage_DownButton_Click(object sender, RoutedEventArgs e)
  {
    this.ChangefromPageValue(false, this.Part_FromPage_TextBox);
  }

  private void Part_FromPage_UpButton_Click(object sender, RoutedEventArgs e)
  {
    this.ChangefromPageValue(true, this.Part_FromPage_TextBox);
  }

  private void ChangefromPageValue(bool IsUp, TextBox textbox)
  {
    int result;
    int.TryParse(textbox.Text, out result);
    if (IsUp)
    {
      if (result < this.ToPage)
      {
        this.FromPage = result + 1;
        this.printManager.FromPage = this.FromPage;
        textbox.Text = this.FromPage.ToString();
      }
      else
      {
        textbox.Text = result == 0 ? this.FromPage.ToString() : result.ToString();
        (textbox.ToolTip as System.Windows.Controls.ToolTip).Content = (object) $"{SharedLocalizationResourceAccessor.Instance.GetString("PrintPreview_UpDown_LessThanToolTip")} {(object) result}";
        this.ShowInvalidBorderandToolTip(textbox);
      }
    }
    else if (result > 1 && this.FromPage <= this.ToPage)
    {
      this.FromPage = result - 1;
      this.printManager.FromPage = this.FromPage;
      textbox.Text = this.FromPage.ToString();
    }
    else
    {
      textbox.Text = result == 0 ? this.FromPage.ToString() : result.ToString();
      (textbox.ToolTip as System.Windows.Controls.ToolTip).Content = (object) (SharedLocalizationResourceAccessor.Instance.GetString("PrintPreview_UpDown_GreaterThanToolTip") + " 1");
      this.ShowInvalidBorderandToolTip(textbox);
    }
  }

  private void ChangeToPageValue(bool IsUp, TextBox textbox)
  {
    int result;
    int.TryParse(textbox.Text, out result);
    if (IsUp)
    {
      if (result < this.printPreviewAreaControl.TotalPages)
      {
        this.ToPage = result + 1;
        this.printManager.ToPage = this.ToPage;
        textbox.Text = this.ToPage.ToString();
      }
      else
      {
        textbox.Text = result == 0 ? this.ToPage.ToString() : result.ToString();
        (textbox.ToolTip as System.Windows.Controls.ToolTip).Content = (object) $"{SharedLocalizationResourceAccessor.Instance.GetString("PrintPreview_UpDown_LessThanToolTip")} {(object) result}";
        this.ShowInvalidBorderandToolTip(textbox);
      }
    }
    else if (result > 1 && this.FromPage < result)
    {
      this.ToPage = result - 1;
      this.printManager.ToPage = this.ToPage;
      textbox.Text = this.ToPage.ToString();
    }
    else
    {
      textbox.Text = result == 0 ? this.ToPage.ToString() : result.ToString();
      (textbox.ToolTip as System.Windows.Controls.ToolTip).Content = (object) $"{SharedLocalizationResourceAccessor.Instance.GetString("PrintPreview_UpDown_GreaterThanToolTip")} {this.FromPage.ToString()}";
      this.ShowInvalidBorderandToolTip(textbox);
    }
  }

  private void Part_PapersComboBox_DropDownOpened(object sender, EventArgs e)
  {
    this.previousPageSize = this.Part_PapersComboBox.SelectedItem;
  }

  private void Part_PapersComboBox_DropDownClosed(object sender, EventArgs e)
  {
    if (this.Part_PapersComboBox.SelectedItem != null)
      return;
    this.IsInSuspend = true;
    this.Part_PapersComboBox.SelectedItem = this.previousPageSize;
    this.Part_PapersComboBox.IsDropDownOpen = false;
    this.IsInSuspend = false;
  }

  private void Part_MarginComboBox_DropDownOpened(object sender, EventArgs e)
  {
    this.previousMargin = this.Part_MarginComboBox.SelectedItem;
    this.MarginOptions[this.MarginOptions.Count - 1].Thickness = (this.previousMargin as PrintPageMargin).Thickness;
    this.Part_MarginComboBox.SelectedItem = (object) null;
  }

  private void Part_MarginCombox_DropDownClosed(object sender, EventArgs e)
  {
    if (this.Part_MarginComboBox.SelectedItem != null)
      return;
    this.IsInSuspend = true;
    this.Part_MarginComboBox.SelectedItem = this.previousMargin;
    this.Part_MarginComboBox.IsDropDownOpen = false;
    this.IsInSuspend = false;
  }

  private void UnWireEvents()
  {
    if (this.Part_PapersComboBox != null)
    {
      this.Part_PapersComboBox.SelectionChanged -= new SelectionChangedEventHandler(this.Part_PapersComboBox_SelectionChanged);
      this.Part_PapersComboBox.DropDownOpened -= new EventHandler(this.Part_PapersComboBox_DropDownOpened);
      this.Part_PapersComboBox.DropDownClosed -= new EventHandler(this.Part_PapersComboBox_DropDownClosed);
    }
    if (this.Part_MarginComboBox != null)
    {
      this.Part_MarginComboBox.SelectionChanged -= new SelectionChangedEventHandler(this.Part_MarginComboBoxSelectionChanged);
      this.Part_MarginComboBox.DropDownOpened -= new EventHandler(this.Part_MarginComboBox_DropDownOpened);
      this.Part_MarginComboBox.DropDownClosed -= new EventHandler(this.Part_MarginCombox_DropDownClosed);
    }
    if (this.Part_OrientationCombombBox != null)
      this.Part_OrientationCombombBox.SelectionChanged -= new SelectionChangedEventHandler(this.Part_OrientationComboBox_SelectionChanged);
    if (this.PART_PrintersComboBox != null)
      this.PART_PrintersComboBox.SelectionChanged -= new SelectionChangedEventHandler(this.PART_PrintersComboBox_SelectionChanged);
    if (this.Part_CopyCount_TextBox != null)
    {
      this.Part_CopyCount_TextBox.LostFocus -= new RoutedEventHandler(this.OnPartTextBoxLostFocus);
      this.Part_CopyCount_TextBox.MouseMove -= new MouseEventHandler(this.Part_TextBox_MouseMove);
      this.Part_CopyCount_TextBox.KeyDown -= new KeyEventHandler(this.Part_TextBox_KeyDown);
      DataObject.RemovePastingHandler((DependencyObject) this.Part_CopyCount_TextBox, new DataObjectPastingEventHandler(this.OnPasting));
    }
    if (this.Part_CopyCount_UpButton != null)
      this.Part_CopyCount_UpButton.Click -= new RoutedEventHandler(this.Part_CopyCount_UpButton_Click);
    if (this.Part_CopyCount_DownButton != null)
      this.Part_CopyCount_DownButton.Click -= new RoutedEventHandler(this.Part_CopyCount_DownButton_Click);
    if (this.Part_FromPage_TextBox != null)
    {
      this.Part_FromPage_TextBox.LostFocus -= new RoutedEventHandler(this.OnPartTextBoxLostFocus);
      this.Part_FromPage_TextBox.MouseMove -= new MouseEventHandler(this.Part_TextBox_MouseMove);
      this.Part_FromPage_TextBox.KeyDown -= new KeyEventHandler(this.Part_TextBox_KeyDown);
      DataObject.RemovePastingHandler((DependencyObject) this.Part_FromPage_TextBox, new DataObjectPastingEventHandler(this.OnPasting));
    }
    if (this.Part_FromPage_UpButton != null)
      this.Part_FromPage_UpButton.Click -= new RoutedEventHandler(this.Part_FromPage_UpButton_Click);
    if (this.Part_FromPage_DownButton != null)
      this.Part_FromPage_DownButton.Click -= new RoutedEventHandler(this.Part_FromPage_DownButton_Click);
    if (this.Part_ToPage_TextBox != null)
    {
      this.Part_ToPage_TextBox.LostFocus -= new RoutedEventHandler(this.OnPartTextBoxLostFocus);
      this.Part_ToPage_TextBox.MouseMove -= new MouseEventHandler(this.Part_TextBox_MouseMove);
      this.Part_ToPage_TextBox.KeyDown -= new KeyEventHandler(this.Part_TextBox_KeyDown);
      DataObject.RemovePastingHandler((DependencyObject) this.Part_ToPage_TextBox, new DataObjectPastingEventHandler(this.OnPasting));
    }
    if (this.Part_ToPage_UpButton != null)
      this.Part_ToPage_UpButton.Click -= new RoutedEventHandler(this.Part_ToPage_UpButton_Click);
    if (this.Part_ToPage_DownButton != null)
      this.Part_ToPage_DownButton.Click -= new RoutedEventHandler(this.Part_ToPage_DownButton_Click);
    this.isWired = false;
  }

  private void OnPrintOptionsControlLoaded(object sender, RoutedEventArgs e)
  {
    if (this.isWired)
      return;
    this.WireEvents();
  }

  private void OnPrintOptionsControlUnloaded(object sender, RoutedEventArgs e)
  {
    if (this.isWired)
      this.UnWireEvents();
    this.Unloaded -= new RoutedEventHandler(this.OnPrintOptionsControlUnloaded);
  }

  private void Part_OrientationComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    if (this.IsInSuspend || this.Part_MarginComboBox == null)
      return;
    this.UpdatePage(this.Part_MarginComboBox.SelectedItem is PrintPageMargin selectedItem ? selectedItem.Thickness : this.PageMargin, false);
  }

  private void PART_PrintersComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    if (e.AddedItems != null && e.AddedItems.Count > 0 && e.AddedItems[0] is PrintQueueOption)
    {
      this.printManager.NeedToChangeDefaultPrinter = false;
      this.printManager.SelectedPrinter = e.AddedItems[0] as PrintQueueOption;
    }
    if (this.Part_PapersComboBox == null || this.Part_QuickPrintButton == null)
      return;
    if (this.Part_PapersComboBox.SelectedItem != null)
      this.Part_QuickPrintButton.IsEnabled = true;
    else
      this.Part_QuickPrintButton.IsEnabled = false;
  }

  private void Part_PapersComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    PrintPageSize addedItem = e.AddedItems.Count == 0 ? (PrintPageSize) null : e.AddedItems[0] as PrintPageSize;
    if (addedItem == null || this.IsInSuspend)
      return;
    if (this.Part_PapersComboBox.SelectedItem != null)
      this.Part_QuickPrintButton.IsEnabled = true;
    if (addedItem.Size.Width == 0.0 && addedItem.Size.Height == 0.0)
    {
      Window parentWindow = PrintExtensions.GetParentWindow((DependencyObject) this);
      CustomPageSizeDialog customPageSizeDialog = new CustomPageSizeDialog(this, (this.previousPageSize != null ? this.previousPageSize as PrintPageSize : addedItem).Size);
      customPageSizeDialog.Owner = parentWindow;
      customPageSizeDialog.ShowDialog();
      this.printManager.dialog.PrintTicket = this.printManager.GetCustomizedPrintTicket(new Size?(PrintExtensions.CmToPixel(new Size(customPageSizeDialog.PageWidth, customPageSizeDialog.PageHeight))), true);
    }
    else if (this.previousPageSize is PrintPageSize previousPageSize && addedItem.Size == previousPageSize.Size)
    {
      this.ResetPageSizeToPreviousValue();
    }
    else
    {
      if (this.Part_MarginComboBox != null && this.Part_MarginComboBox.SelectedItem != null)
        this.UpdatePage((this.Part_MarginComboBox.SelectedItem as PrintPageMargin).Thickness, false);
      this.printManager.SelectedPageMediaName = addedItem.PageSizeName;
      this.printManager.OnPageSizeChanged(addedItem.Size.Width, addedItem.Size.Height);
      this.printPreviewAreaControl.UpdateZoomFactor();
      Tuple<string, int, int> queueCapablitySiz = this.printManager.printQueueCapablitySizes[this.SelectedPageSize];
      if (queueCapablitySiz == null || queueCapablitySiz.Item2 <= 0 || queueCapablitySiz.Item3 <= 0)
        return;
      this.printManager.dialog.PrintTicket = this.printManager.GetCustomizedPrintTicket(new Size?(new Size((double) queueCapablitySiz.Item2 / 264.584, (double) queueCapablitySiz.Item3 / 264.584)), true);
    }
  }

  internal void SetPageSize(Size pagesize)
  {
    if (this.previousPageSize is PrintPageSize previousPageSize && pagesize == previousPageSize.Size)
    {
      this.ResetPageSizeToPreviousValue();
    }
    else
    {
      double pixel1 = PrintExtensions.CmToPixel(pagesize.Height);
      double pixel2 = PrintExtensions.CmToPixel(pagesize.Width);
      if (pixel1 < this.PageMargin.Top + this.PageMargin.Bottom || pixel2 < this.PageMargin.Left + this.PageMargin.Right)
        return;
      this.printManager.OnPageSizeChanged(pixel2, pixel1);
      if (this.Part_OrientationCombombBox != null)
        this.Part_OrientationCombombBox.SelectedIndex = pixel1 > pixel2 ? 0 : 1;
      this.UpdatePaperSizeComboBox(pagesize);
      if (this.Part_MarginComboBox != null)
        this.UpdatePage((this.Part_MarginComboBox.SelectedItem as PrintPageMargin).Thickness, false);
      this.printPreviewAreaControl.UpdateZoomFactor();
    }
  }

  internal void ResetPageSizeToPreviousValue()
  {
    this.IsInSuspend = true;
    this.Part_PapersComboBox.SelectedItem = this.previousPageSize;
    this.IsInSuspend = false;
  }

  internal void ResetPageMaringToPreviousValue()
  {
    this.IsInSuspend = true;
    this.Part_MarginComboBox.SelectedItem = this.previousMargin;
    this.IsInSuspend = false;
  }

  internal void SetCustomMargin(Thickness pageMargin)
  {
    if (this.previousMargin is PrintPageMargin previousMargin && pageMargin == previousMargin.Thickness)
      this.ResetPageMaringToPreviousValue();
    else
      this.UpdatePage(pageMargin, true);
  }

  private void Part_MarginComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    PrintPageMargin addedItem = e.AddedItems.Count == 0 ? (PrintPageMargin) null : e.AddedItems[0] as PrintPageMargin;
    if (addedItem == null || this.IsInSuspend)
      return;
    if (addedItem.MarginName == SharedLocalizationResourceAccessor.Instance.GetString("Print_PageMargin_CustomMargin"))
    {
      PrintPageMargin printPageMargin = this.previousMargin != null ? this.previousMargin as PrintPageMargin : addedItem;
      Window parentWindow = PrintExtensions.GetParentWindow((DependencyObject) this);
      CustomMarginDialog childobject = new CustomMarginDialog(this, printPageMargin.Thickness);
      childobject.Owner = parentWindow;
      try
      {
        SfSkinManagerExtension.SetTheme((DependencyObject) this, (DependencyObject) childobject);
      }
      catch
      {
      }
      childobject.ShowDialog();
    }
    else if (this.previousMargin is PrintPageMargin previousMargin && addedItem.Thickness == previousMargin.Thickness)
    {
      this.ResetPageMaringToPreviousValue();
    }
    else
    {
      Thickness thickness = addedItem.Thickness;
      if (this.UpdatePage((this.Part_MarginComboBox.SelectedItem as PrintPageMargin).Thickness, false))
        return;
      this.PageMargin = PrintExtensions.CmToPixel(thickness);
      this.printPreviewAreaControl.OnZoomFactorChanged(this.printPreviewAreaControl.ZoomFactor);
    }
  }

  private bool UpdatePage(Thickness selectedMargin, bool canupdate)
  {
    bool flag = false;
    double num1 = 0.5;
    double num2 = 0.9;
    if (this.Part_PapersComboBox.SelectedItem is PrintPageSize selectedItem)
    {
      if (this.PrintOrientation == PrintOrientation.Portrait)
      {
        if (selectedMargin.Left + selectedMargin.Right > selectedItem.Size.Width - num1)
          flag = true;
        if (selectedMargin.Bottom + selectedMargin.Top > selectedItem.Size.Height - num2)
          flag = true;
      }
      if (this.PrintOrientation == PrintOrientation.Landscape)
      {
        if (selectedMargin.Left + selectedMargin.Right > selectedItem.Size.Height - num1)
          flag = true;
        if (selectedMargin.Bottom + selectedMargin.Top > selectedItem.Size.Width - num2)
          flag = true;
      }
      if (flag)
        selectedMargin = this.DefaultPageMargin;
      if (flag || canupdate)
      {
        this.PageMargin = PrintExtensions.CmToPixel(selectedMargin);
        this.UpdatePageMarginComboBox(selectedMargin);
        this.printPreviewAreaControl.OnZoomFactorChanged(this.printPreviewAreaControl.ZoomFactor);
        return true;
      }
    }
    return false;
  }

  public ICommand PrintCommand
  {
    get
    {
      return this.printCommand ?? (this.printCommand = (ICommand) new BaseCommand(new Action<object>(this.OnprintCommandClicked), (Predicate<object>) (o => (this.printManager != null ? this.printManager.PageCount : 0) > 0)));
    }
  }

  private void OnprintCommandClicked(object obj) => this.printManager.PrintWithDialog();

  public ICommand QuickPrintCommand
  {
    get
    {
      return this.quickPrintCommand ?? (this.quickPrintCommand = (ICommand) new BaseCommand(new Action<object>(this.OnQuickprintCommandClicked), (Predicate<object>) (o => (this.printManager != null ? this.printManager.PageCount : 0) > 0)));
    }
  }

  private void OnQuickprintCommandClicked(object obj) => this.printManager.Print();

  public event PropertyChangedEventHandler PropertyChanged;

  protected void OnPropertyChanged(string propertyName)
  {
    if (this.PropertyChanged == null)
      return;
    this.PropertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
  }

  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }

  protected virtual void Dispose(bool isDisposing)
  {
    if (this.isdisposed)
      return;
    if (isDisposing)
    {
      this.Unloaded -= new RoutedEventHandler(this.OnPrintOptionsControlUnloaded);
      this.printManager = (PrintManager) null;
      this.printPreviewAreaControl = (PrintPreviewAreaControl) null;
      this.Part_ScaleOptionComboBox = (ComboBox) null;
      this.Part_PapersComboBox = (ComboBox) null;
      this.Part_MarginComboBox = (ComboBox) null;
      this.Part_OrientationCombombBox = (ComboBox) null;
    }
    this.isdisposed = true;
  }

  private void Part_TextBox_MouseMove(object sender, MouseEventArgs e)
  {
    TextBox textBox = sender as TextBox;
    if (!textBox.IsMouseOver || textBox.IsFocused)
      return;
    (textBox.ToolTip as System.Windows.Controls.ToolTip).Visibility = Visibility.Collapsed;
  }

  private void Part_CopyCount_DownButton_Click(object sender, RoutedEventArgs e)
  {
    this.ChangeValue(false, this.Part_CopyCount_TextBox);
  }

  private void Part_CopyCount_UpButton_Click(object sender, RoutedEventArgs e)
  {
    this.ChangeValue(true, this.Part_CopyCount_TextBox);
  }

  private void ChangeValue(bool IsUp, TextBox textbox)
  {
    int result;
    int.TryParse(textbox.Text, out result);
    if (IsUp)
    {
      if (result < 32000)
      {
        this.CopiesCount = result + 1;
        this.printManager.CopiesCount = this.CopiesCount;
        textbox.Text = this.CopiesCount.ToString();
      }
      else
      {
        (textbox.ToolTip as System.Windows.Controls.ToolTip).Content = (object) $"{SharedLocalizationResourceAccessor.Instance.GetString("PrintPreview_UpDown_LessThanToolTip")} {(object) result}";
        this.ShowInvalidBorderandToolTip(textbox);
      }
    }
    else if (result > 1)
    {
      this.CopiesCount = result - 1;
      this.printManager.CopiesCount = this.CopiesCount;
      textbox.Text = this.CopiesCount.ToString();
    }
    else
    {
      (textbox.ToolTip as System.Windows.Controls.ToolTip).Content = (object) (SharedLocalizationResourceAccessor.Instance.GetString("PrintPreview_UpDown_GreaterThanToolTip") + " 1");
      this.ShowInvalidBorderandToolTip(textbox);
    }
  }

  private void Part_TextBox_KeyDown(object sender, KeyEventArgs e)
  {
    TextBox textBox = sender as TextBox;
    int num1 = 32000;
    int num2 = 0;
    int result;
    if (textBox != this.Part_CopyCount_TextBox)
    {
      if (textBox == this.Part_FromPage_TextBox)
      {
        int.TryParse(this.Part_FromPage_TextBox.Text, out result);
        num2 = result;
        int.TryParse(this.Part_ToPage_TextBox.Text, out result);
        num1 = result;
      }
      else
      {
        int.TryParse(this.Part_FromPage_TextBox.Text, out result);
        num2 = result;
        num1 = this.printPreviewAreaControl.TotalPages;
      }
    }
    System.Windows.Controls.ToolTip toolTip = textBox.ToolTip as System.Windows.Controls.ToolTip;
    if (e.Key == Key.Return)
    {
      if (int.TryParse(textBox.Text, out result) && result >= num2 && result <= num1)
      {
        if (textBox == this.Part_CopyCount_TextBox)
        {
          this.CopiesCount = result;
          this.printManager.CopiesCount = this.CopiesCount;
        }
        else if (textBox == this.Part_FromPage_TextBox)
        {
          this.FromPage = result;
          this.printManager.FromPage = this.FromPage;
        }
        else
        {
          this.ToPage = result;
          this.printManager.ToPage = this.ToPage;
        }
        this.ClearInvalidBorderandToolTip(textBox);
      }
      else
      {
        if (!textBox.Text.Equals("") && !(textBox.Text == "0") && (!int.TryParse(textBox.Text, out result) || result >= this.ToPage))
          return;
        if (textBox == this.Part_CopyCount_TextBox)
          textBox.Text = Convert.ToString(this.CopiesCount);
        else if (textBox == this.Part_FromPage_TextBox)
          textBox.Text = Convert.ToString(this.FromPage);
        else
          textBox.Text = Convert.ToString(this.ToPage);
      }
    }
    else if (e.Key == Key.OemMinus)
    {
      toolTip.Content = (object) SharedLocalizationResourceAccessor.Instance.GetString("PrintPreview_UpDown_NegativeToolTip");
      this.ShowInvalidBorderandToolTip(textBox);
      e.Handled = true;
    }
    else if (!Keyboard.Modifiers.Equals((object) ModifierKeys.None))
      e.Handled = true;
    else if (e.Key >= Key.D0 && e.Key <= Key.D9)
    {
      string str = string.IsNullOrEmpty(textBox.SelectedText) ? textBox.Text : textBox.Text.Remove(textBox.CaretIndex, textBox.SelectedText.Length);
      if (int.TryParse(str.Insert(textBox.CaretIndex, Convert.ToString((int) (e.Key - 34))), out result))
      {
        bool flag = false;
        if (this.Part_ToPage_TextBox == textBox && num2 > result)
          flag = true;
        if (result < 1 || result > num1 || flag)
        {
          if (result > num1)
            toolTip.Content = (object) $"{SharedLocalizationResourceAccessor.Instance.GetString("PrintPreview_UpDown_LessThanToolTip")} {(object) num1}";
          else if (num2 > result)
            toolTip.Content = (object) $"{SharedLocalizationResourceAccessor.Instance.GetString("PrintPreview_UpDown_LessThanToolTip")} {(object) num2}";
          else if (result < 1)
            toolTip.Content = (object) (SharedLocalizationResourceAccessor.Instance.GetString("PrintPreview_UpDown_GreaterThanToolTip") + " 1");
          this.ShowInvalidBorderandToolTip(textBox);
          e.Handled = true;
        }
        else
        {
          int int32 = Convert.ToInt32(str.Trim() + Convert.ToString((int) (e.Key - 34)));
          if (int32 <= num2 || int32 > num1)
            return;
          this.ClearInvalidBorderandToolTip(textBox);
        }
      }
      else
        e.Handled = true;
    }
    else
    {
      if (this.IsNumberOrControlKey(e.Key))
        return;
      toolTip.Content = (object) SharedLocalizationResourceAccessor.Instance.GetString("PrintPreview_UpDown_NonNumericToopTip");
      this.ShowInvalidBorderandToolTip(textBox);
      e.Handled = true;
    }
  }

  private bool IsNumberOrControlKey(Key inKey)
  {
    return inKey == Key.Delete || inKey == Key.Back || inKey == Key.Tab || inKey == Key.Return || Keyboard.Modifiers.HasFlag((Enum) ModifierKeys.Alt) || Keyboard.Modifiers.HasFlag((Enum) ModifierKeys.Control) || Keyboard.Modifiers.HasFlag((Enum) ModifierKeys.Shift) || inKey >= Key.D0 && inKey <= Key.D9 || inKey >= Key.NumPad0 && inKey <= Key.NumPad9;
  }

  private void ShowInvalidBorderandToolTip(TextBox textBox)
  {
    System.Windows.Controls.ToolTip toolTip = textBox.ToolTip as System.Windows.Controls.ToolTip;
    toolTip.Visibility = Visibility.Visible;
    if (toolTip == null)
      return;
    toolTip.Placement = PlacementMode.Bottom;
    toolTip.PlacementTarget = (UIElement) textBox;
    if (toolTip.IsOpen)
      return;
    toolTip.IsOpen = true;
  }

  private void ClearInvalidBorderandToolTip(TextBox textBox)
  {
    if (!(textBox.ToolTip is System.Windows.Controls.ToolTip toolTip) || !toolTip.IsOpen)
      return;
    toolTip.IsOpen = false;
  }

  private void OnPartTextBoxLostFocus(object sender, RoutedEventArgs e)
  {
    TextBox textBox = sender as TextBox;
    int num = this.printPreviewAreaControl.TotalPages;
    if (textBox == this.Part_CopyCount_TextBox)
      num = 32000;
    int result;
    if (int.TryParse(textBox.Text, out result) && result > 0 && result <= num)
    {
      if (textBox == this.Part_CopyCount_TextBox)
      {
        this.CopiesCount = result;
        this.printManager.CopiesCount = this.CopiesCount;
      }
      else if (textBox == this.Part_FromPage_TextBox)
      {
        this.FromPage = result;
        this.printManager.FromPage = this.FromPage;
      }
      else
      {
        this.ToPage = result;
        this.printManager.ToPage = this.ToPage;
      }
      this.ClearInvalidBorderandToolTip(textBox);
    }
    else if (textBox.Text.Equals("") || textBox.Text == "0")
    {
      if (textBox == this.Part_CopyCount_TextBox)
        this.Part_CopyCount_TextBox.Text = this.CopiesCount.ToString();
      else if (textBox == this.Part_FromPage_TextBox)
        this.Part_FromPage_TextBox.Text = this.FromPage.ToString();
      else
        this.Part_ToPage_TextBox.Text = this.ToPage.ToString();
      this.ClearInvalidBorderandToolTip(textBox);
    }
    else
      textBox.ClearValue(TextBox.TextProperty);
  }

  private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
  {
    if (e.Key == Key.Space)
      e.Handled = true;
    this.OnPreviewKeyDown(e);
  }

  private void OnPasting(object sender, DataObjectPastingEventArgs e)
  {
    if (e.DataObject.GetDataPresent(typeof (string)))
    {
      string data = (string) e.DataObject.GetData(typeof (string));
      if (!this.IsNumber(data))
      {
        TextBox textBox = sender as TextBox;
        (textBox.ToolTip as System.Windows.Controls.ToolTip).Content = (object) SharedLocalizationResourceAccessor.Instance.GetString("PrintPreview_UpDown_NonNumericToopTip");
        this.ShowInvalidBorderandToolTip(textBox);
        e.CancelCommand();
      }
      else
      {
        int num1;
        int num2;
        TextBox textBox;
        if (sender == this.Part_CopyCount_TextBox)
        {
          num1 = 1;
          num2 = 32000;
          textBox = this.Part_CopyCount_TextBox;
        }
        else if (sender == this.Part_FromPage_TextBox)
        {
          num1 = this.FromPage;
          num2 = this.ToPage;
          textBox = this.Part_FromPage_TextBox;
        }
        else
        {
          num1 = this.FromPage;
          num2 = this.printPreviewAreaControl.TotalPages;
          textBox = this.Part_ToPage_TextBox;
        }
        int result;
        if (int.TryParse((string.IsNullOrEmpty(textBox.SelectedText) ? textBox.Text : textBox.Text.Remove(textBox.CaretIndex, textBox.SelectedText.Length)).Insert(textBox.CaretIndex, data), out result))
        {
          bool flag = false;
          if (result > num2)
          {
            (textBox.ToolTip as System.Windows.Controls.ToolTip).Content = (object) $"{SharedLocalizationResourceAccessor.Instance.GetString("PrintPreview_UpDown_LessThanToolTip")} {(object) num2}";
            flag = true;
          }
          else if (result < num1)
          {
            (textBox.ToolTip as System.Windows.Controls.ToolTip).Content = (object) $"{SharedLocalizationResourceAccessor.Instance.GetString("PrintPreview_UpDown_GreaterThanToolTip")} {(object) num1}";
            flag = true;
          }
          if (!flag)
            return;
          this.ShowInvalidBorderandToolTip(textBox);
          e.CancelCommand();
        }
        else
          e.CancelCommand();
      }
    }
    else
      e.CancelCommand();
  }

  private bool IsNumber(string text)
  {
    foreach (char c in text.ToCharArray())
    {
      if (!char.IsDigit(c))
        return false;
    }
    return true;
  }

  private void PrintOptionsControl_MouseMove(object sender, MouseEventArgs e)
  {
    if (this.Part_CopyCount_TextBox != null)
      this.ClearInvalidBorderandToolTip(this.Part_CopyCount_TextBox);
    if (this.Part_FromPage_TextBox != null)
      this.ClearInvalidBorderandToolTip(this.Part_FromPage_TextBox);
    if (this.Part_ToPage_TextBox == null)
      return;
    this.ClearInvalidBorderandToolTip(this.Part_ToPage_TextBox);
  }
}
