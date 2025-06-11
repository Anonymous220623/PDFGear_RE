// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.PageHeaderFooters.PageNumberDialog
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Controls;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Patagames.Pdf;
using Patagames.Pdf.Net;
using pdfeditor.Controls.ColorPickers;
using pdfeditor.Services;
using PDFKit.Utils;
using PDFKit.Utils.PageHeaderFooters;
using PDFKit.Utils.XObjects;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.Controls.PageHeaderFooters;

public partial class PageNumberDialog : Window, IComponentConnector
{
  private PdfDocument document;
  public static readonly DependencyProperty ResultProperty;
  private static readonly DependencyPropertyKey ResultPropertyKey = DependencyProperty.RegisterReadOnly("Texts", typeof (ResultModel), typeof (PageNumberDialog), new PropertyMetadata((object) null, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is PageNumberDialog pageNumberDialog2) || a.NewValue == a.OldValue)
      return;
    pageNumberDialog2.UpdateSubset();
    pageNumberDialog2.UpdateFontSize();
  })));
  private CancellationTokenSource cts;
  internal ComboBox StyleComboBox;
  internal NumberBox PageNumberOffsetBox;
  internal ComboBox FontSizeComboBox;
  internal ColorPickerButton TextColorButton;
  internal RadioButton PositionHeaderRadioButton;
  internal RadioButton PositionFooterRadioButton;
  internal RadioButton AlignmentLeftRadioButton;
  internal RadioButton AlignmentCenterRadioButton;
  internal RadioButton AlignmentRightRadioButton;
  internal RadioButton AllPageRadioButton;
  internal RadioButton SelectedPagesRadioButton;
  internal NumberBox SelectedPageStartBox;
  internal NumberBox SelectedPageEndBox;
  internal ComboBox SubsetComboBox;
  internal NumberBox PreviewPageIndexBox;
  internal TorePaperControl Tore1;
  internal MarginControl MarginControl1;
  internal TorePaperControl Tore2;
  internal MarginControl MarginControl2;
  private bool _contentLoaded;

  static PageNumberDialog()
  {
    PageNumberDialog.ResultProperty = PageNumberDialog.ResultPropertyKey.DependencyProperty;
  }

  public PageNumberDialog(PdfDocument doc, HeaderFooterSettings settings, int[] selectedIndexes = null)
  {
    this.InitializeComponent();
    this.document = doc ?? throw new ArgumentException(nameof (doc));
    ResultModel resultModel = settings == null ? new ResultModel() : ResultModel.FromSettings(doc, settings);
    this.StyleComboBox.ItemsSource = (IEnumerable) PagePlaceholderFormatter.AllSupportedPageNumberFormats.ToList<string>();
    if (!PagePlaceholderFormatter.AllSupportedPageNumberFormats.Contains<string>(resultModel.PageNumberFormat))
      resultModel.PageNumberFormat = PagePlaceholderFormatter.AllSupportedPageNumberFormats[0];
    this.StyleComboBox.SelectedItem = (object) resultModel.PageNumberFormat;
    this.PageNumberOffsetBox.Maximum = (double) int.MaxValue;
    this.PageNumberOffsetBox.Value = (double) resultModel.PageNumberOffset;
    this.UpdateSelectedPageRange();
    if (resultModel.PageRange == ResultModel.PageRangeEnum.None)
    {
      if (selectedIndexes != null && selectedIndexes.Length != 0)
      {
        resultModel.PageRange = ResultModel.PageRangeEnum.SelectedPages;
        resultModel.SelectedPagesStart = selectedIndexes[0] + 1;
        resultModel.SelectedPagesEnd = selectedIndexes[selectedIndexes.Length - 1] + 1;
      }
      else
      {
        resultModel.PageRange = ResultModel.PageRangeEnum.AllPages;
        resultModel.SelectedPagesStart = 1;
        resultModel.SelectedPagesEnd = this.document.Pages.Count;
      }
    }
    if (resultModel.PageRange == ResultModel.PageRangeEnum.AllPages)
      this.AllPageRadioButton.IsChecked = new bool?(true);
    else if (resultModel.PageRange == ResultModel.PageRangeEnum.SelectedPages)
      this.SelectedPagesRadioButton.IsChecked = new bool?(true);
    this.SubsetComboBox.SelectedIndex = (int) resultModel.Subset;
    string fontSizeStr = $"{resultModel.FontSize:0.#}pt";
    ComboBoxItem comboBoxItem1 = this.FontSizeComboBox.Items.OfType<ComboBoxItem>().FirstOrDefault<ComboBoxItem>((Func<ComboBoxItem, bool>) (c => object.Equals((object) fontSizeStr, c.Content)));
    if (comboBoxItem1 != null)
    {
      comboBoxItem1.IsSelected = true;
    }
    else
    {
      fontSizeStr = "10pt";
      ComboBoxItem comboBoxItem2 = this.FontSizeComboBox.Items.OfType<ComboBoxItem>().FirstOrDefault<ComboBoxItem>((Func<ComboBoxItem, bool>) (c => object.Equals((object) fontSizeStr, (object) c)));
      if (comboBoxItem2 != null)
        comboBoxItem2.IsSelected = true;
    }
    this.Result = resultModel;
    this.UpdateResultText();
    this.UpdatePagePreviewAsync(0);
  }

  private void UpdateSelectedPageRange()
  {
    this.SelectedPageEndBox.Maximum = (double) this.document.Pages.Count;
    this.PreviewPageIndexBox.Maximum = (double) this.document.Pages.Count;
  }

  public ResultModel Result
  {
    get => (ResultModel) this.GetValue(PageNumberDialog.ResultProperty);
    private set => this.SetValue(PageNumberDialog.ResultPropertyKey, (object) value);
  }

  private void CancelButton_Click(object sender, RoutedEventArgs e) => e.Handled = true;

  private void OKButton_Click(object sender, RoutedEventArgs e)
  {
    Keyboard.ClearFocus();
    this.IsHitTestVisible = false;
    this.UpdateResultText();
    this.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) (() => this.DialogResult = new bool?(true)));
  }

  private void TextColorButton_SelectedColorChanged(
    object sender,
    ColorPickerButtonSelectedColorChangedEventArgs e)
  {
    this.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) (() => this.UpdatePreviewTextContent()));
  }

  private void FontSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    this.UpdateFontSize();
  }

  private void HeaderFooterDialogPageRange_Checked(object sender, RoutedEventArgs e)
  {
    if (this.Result == null)
      return;
    if (e.Source == this.AllPageRadioButton)
    {
      this.Result.PageRange = ResultModel.PageRangeEnum.AllPages;
    }
    else
    {
      if (e.Source != this.SelectedPagesRadioButton)
        return;
      this.Result.PageRange = ResultModel.PageRangeEnum.SelectedPages;
    }
  }

  private void SubsetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    this.UpdateSubset();
  }

  private async void PreviewPageIndexBox_ValueChanged(
    object sender,
    RoutedPropertyChangedEventArgs<double> e)
  {
    int num = (int) ((RangeBase) sender).Value;
    if (this.document == null || num <= 0 || num > this.document.Pages.Count)
      return;
    await this.UpdatePagePreviewAsync(num - 1);
  }

  private void StyleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    if (this.Result != null)
      this.Result.PageNumberFormat = (string) e.AddedItems[0];
    this.UpdateResultText();
    this.UpdatePreviewTextContent();
  }

  private void PageNumberOffsetBox_ValueChanged(
    object sender,
    RoutedPropertyChangedEventArgs<double> e)
  {
    if (this.Result != null)
      this.Result.PageNumberOffset = (int) (e.NewValue + 0.5);
    this.UpdateResultText();
    this.UpdatePreviewTextContent();
  }

  private void PositionRadioButton_Checked(object sender, RoutedEventArgs e)
  {
    this.UpdateResultText();
    this.UpdatePreviewTextContent();
  }

  private void AlignmentRadioButton_Checked(object sender, RoutedEventArgs e)
  {
    this.UpdateResultText();
    this.UpdatePreviewTextContent();
  }

  protected override void OnMouseDown(MouseButtonEventArgs e)
  {
    base.OnMouseDown(e);
    if (Keyboard.FocusedElement is FrameworkElement focusedElement)
    {
      TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
      focusedElement.MoveFocus(request);
    }
    Keyboard.ClearFocus();
    this.UpdateResultText();
    this.UpdatePreviewTextContent();
  }

  private void UpdateResultText()
  {
    if (this.Result == null)
      return;
    this.Result.Text.LeftHeaderText = "";
    this.Result.Text.CenterHeaderText = "";
    this.Result.Text.RightHeaderText = "";
    this.Result.Text.LeftFooterText = "";
    this.Result.Text.CenterFooterText = "";
    this.Result.Text.RightFooterText = "";
    string str = $"<<{this.Result.PageNumberFormat}>>";
    bool? isChecked = this.PositionHeaderRadioButton.IsChecked;
    if (isChecked.GetValueOrDefault())
    {
      isChecked = this.AlignmentLeftRadioButton.IsChecked;
      if (isChecked.GetValueOrDefault())
        this.Result.Text.LeftHeaderText = str;
      isChecked = this.AlignmentCenterRadioButton.IsChecked;
      if (isChecked.GetValueOrDefault())
        this.Result.Text.CenterHeaderText = str;
      isChecked = this.AlignmentRightRadioButton.IsChecked;
      if (!isChecked.GetValueOrDefault())
        return;
      this.Result.Text.RightHeaderText = str;
    }
    else
    {
      isChecked = this.AlignmentLeftRadioButton.IsChecked;
      if (isChecked.GetValueOrDefault())
        this.Result.Text.LeftFooterText = str;
      isChecked = this.AlignmentCenterRadioButton.IsChecked;
      if (isChecked.GetValueOrDefault())
        this.Result.Text.CenterFooterText = str;
      isChecked = this.AlignmentRightRadioButton.IsChecked;
      if (!isChecked.GetValueOrDefault())
        return;
      this.Result.Text.RightFooterText = str;
    }
  }

  private void UpdateSubset()
  {
    if (this.Result == null)
      return;
    switch (this.SubsetComboBox.SelectedIndex)
    {
      case 0:
        this.Result.Subset = ResultModel.SubsetEnum.AllPages;
        break;
      case 1:
        this.Result.Subset = ResultModel.SubsetEnum.Odd;
        break;
      case 2:
        this.Result.Subset = ResultModel.SubsetEnum.Even;
        break;
      default:
        this.Result.Subset = ResultModel.SubsetEnum.AllPages;
        break;
    }
  }

  private void UpdateFontSize()
  {
    if (this.Result == null)
      return;
    string s = string.Empty;
    if (this.FontSizeComboBox.SelectedItem is string selectedItem2)
      s = selectedItem2;
    else if (this.FontSizeComboBox.SelectedItem is ComboBoxItem selectedItem1 && selectedItem1.Content is string content)
      s = content;
    float result = 14f;
    if (!string.IsNullOrEmpty(s))
    {
      if (s.Length >= 2 && s[s.Length - 2] == 'p' && s[s.Length - 1] == 't')
        s = s.Substring(0, s.Length - 2);
      if (!float.TryParse(s, out result))
        result = 14f;
    }
    this.Result.FontSize = result;
    this.UpdatePreviewTextContent();
  }

  private async Task UpdatePagePreviewAsync(int pageIndex)
  {
    this.cts?.Cancel();
    this.cts?.Dispose();
    this.cts = new CancellationTokenSource();
    PdfThumbnailService service = Ioc.Default.GetService<PdfThumbnailService>();
    try
    {
      WriteableBitmap pdfBitmapAsync = await service.TryGetPdfBitmapAsync(this.document.Pages[pageIndex], Colors.White, this.document.Pages[pageIndex].Rotation, 600, 0, this.cts.Token);
      ImageBrush imageBrush1 = new ImageBrush((ImageSource) pdfBitmapAsync);
      imageBrush1.AlignmentY = AlignmentY.Top;
      imageBrush1.AlignmentX = AlignmentX.Left;
      imageBrush1.Stretch = Stretch.UniformToFill;
      this.Tore1.ContentBrush = (Brush) imageBrush1;
      ImageBrush imageBrush2 = new ImageBrush((ImageSource) pdfBitmapAsync);
      imageBrush2.AlignmentY = AlignmentY.Bottom;
      imageBrush2.AlignmentX = AlignmentX.Left;
      imageBrush2.Stretch = Stretch.UniformToFill;
      this.Tore2.ContentBrush = (Brush) imageBrush2;
      FS_SIZEF effectiveSize = this.document.Pages[pageIndex].GetEffectiveSize();
      this.MarginControl1.PageOriginalWidth = (double) effectiveSize.Width;
      this.MarginControl2.PageOriginalWidth = (double) effectiveSize.Width;
      this.UpdatePreviewTextContentCore(this.document.Pages[pageIndex]);
    }
    catch (OperationCanceledException ex)
    {
    }
  }

  private void UpdatePreviewTextContentCore(PdfPage page)
  {
    if (this.Result == null)
      return;
    HeaderFooterSettings settings = this.Result.ToSettings(this.document, page.PageIndex);
    int pageIndex = page.PageIndex;
    int count = page.Document.Pages.Count;
    this.MarginControl1.LeftString = PageHeaderFooterUtils.GetContent(page, pageIndex, count, settings, PageHeaderFooterUtils.LocationEnum.HeaderLeft, DateTimeOffset.Now);
    this.MarginControl1.CenterString = PageHeaderFooterUtils.GetContent(page, pageIndex, count, settings, PageHeaderFooterUtils.LocationEnum.HeaderCenter, DateTimeOffset.Now);
    this.MarginControl1.RightString = PageHeaderFooterUtils.GetContent(page, pageIndex, count, settings, PageHeaderFooterUtils.LocationEnum.HeaderRight, DateTimeOffset.Now);
    this.MarginControl2.LeftString = PageHeaderFooterUtils.GetContent(page, pageIndex, count, settings, PageHeaderFooterUtils.LocationEnum.FooterLeft, DateTimeOffset.Now);
    this.MarginControl2.CenterString = PageHeaderFooterUtils.GetContent(page, pageIndex, count, settings, PageHeaderFooterUtils.LocationEnum.FooterCenter, DateTimeOffset.Now);
    this.MarginControl2.RightString = PageHeaderFooterUtils.GetContent(page, pageIndex, count, settings, PageHeaderFooterUtils.LocationEnum.FooterRight, DateTimeOffset.Now);
    this.MarginControl1.PreviewFontSize = settings.Font.Size;
    this.MarginControl1.FontFamily = new FontFamily("Arial, Microsoft Yahei");
    this.MarginControl1.Foreground = (Brush) new SolidColorBrush(this.Result.Color);
    this.MarginControl2.PreviewFontSize = settings.Font.Size;
    this.MarginControl2.FontFamily = new FontFamily("Arial, Microsoft Yahei");
    this.MarginControl2.Foreground = (Brush) new SolidColorBrush(this.Result.Color);
  }

  private void UpdatePreviewTextContent()
  {
    if (this.PreviewPageIndexBox == null)
      return;
    int num = (int) this.PreviewPageIndexBox.Value;
    if (this.document == null || num <= 0 || num > this.document.Pages.Count)
      return;
    this.UpdatePreviewTextContentCore(this.document.Pages[num - 1]);
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/pageheaderfooters/pagenumberdialog.xaml", UriKind.Relative));
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
        this.StyleComboBox = (ComboBox) target;
        this.StyleComboBox.SelectionChanged += new SelectionChangedEventHandler(this.StyleComboBox_SelectionChanged);
        break;
      case 2:
        this.PageNumberOffsetBox = (NumberBox) target;
        this.PageNumberOffsetBox.ValueChanged += new RoutedPropertyChangedEventHandler<double>(this.PageNumberOffsetBox_ValueChanged);
        break;
      case 3:
        this.FontSizeComboBox = (ComboBox) target;
        this.FontSizeComboBox.SelectionChanged += new SelectionChangedEventHandler(this.FontSizeComboBox_SelectionChanged);
        break;
      case 4:
        this.TextColorButton = (ColorPickerButton) target;
        break;
      case 5:
        ((UIElement) target).AddHandler(ToggleButton.CheckedEvent, (Delegate) new RoutedEventHandler(this.PositionRadioButton_Checked));
        break;
      case 6:
        this.PositionHeaderRadioButton = (RadioButton) target;
        break;
      case 7:
        this.PositionFooterRadioButton = (RadioButton) target;
        break;
      case 8:
        ((UIElement) target).AddHandler(ToggleButton.CheckedEvent, (Delegate) new RoutedEventHandler(this.AlignmentRadioButton_Checked));
        break;
      case 9:
        this.AlignmentLeftRadioButton = (RadioButton) target;
        break;
      case 10:
        this.AlignmentCenterRadioButton = (RadioButton) target;
        break;
      case 11:
        this.AlignmentRightRadioButton = (RadioButton) target;
        break;
      case 12:
        ((UIElement) target).AddHandler(ToggleButton.CheckedEvent, (Delegate) new RoutedEventHandler(this.HeaderFooterDialogPageRange_Checked));
        break;
      case 13:
        this.AllPageRadioButton = (RadioButton) target;
        break;
      case 14:
        this.SelectedPagesRadioButton = (RadioButton) target;
        break;
      case 15:
        this.SelectedPageStartBox = (NumberBox) target;
        break;
      case 16 /*0x10*/:
        this.SelectedPageEndBox = (NumberBox) target;
        break;
      case 17:
        this.SubsetComboBox = (ComboBox) target;
        this.SubsetComboBox.SelectionChanged += new SelectionChangedEventHandler(this.SubsetComboBox_SelectionChanged);
        break;
      case 18:
        this.PreviewPageIndexBox = (NumberBox) target;
        this.PreviewPageIndexBox.ValueChanged += new RoutedPropertyChangedEventHandler<double>(this.PreviewPageIndexBox_ValueChanged);
        break;
      case 19:
        this.Tore1 = (TorePaperControl) target;
        break;
      case 20:
        this.MarginControl1 = (MarginControl) target;
        break;
      case 21:
        this.Tore2 = (TorePaperControl) target;
        break;
      case 22:
        this.MarginControl2 = (MarginControl) target;
        break;
      case 23:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.CancelButton_Click);
        break;
      case 24:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.OKButton_Click);
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
