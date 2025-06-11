// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.PageHeaderFooters.PageHeaderFooterDialog
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
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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

public partial class PageHeaderFooterDialog : Window, IComponentConnector
{
  private PdfDocument document;
  private readonly string fileName;
  private TextBox lastLostFocusTextbox;
  public static readonly DependencyProperty ResultProperty;
  private static readonly DependencyPropertyKey ResultPropertyKey = DependencyProperty.RegisterReadOnly("Texts", typeof (ResultModel), typeof (PageHeaderFooterDialog), new PropertyMetadata((object) null, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is PageHeaderFooterDialog headerFooterDialog2) || a.NewValue == a.OldValue)
      return;
    headerFooterDialog2.UpdateSubset();
    headerFooterDialog2.UpdateFontSize();
  })));
  private CancellationTokenSource cts;
  internal Grid ContentContainer;
  internal TextBox LeftHeaderTextBox;
  internal TextBox CenterHeaderTextBox;
  internal TextBox RightHeaderTextBox;
  internal TextBox LeftFooterTextBox;
  internal TextBox CenterFooterTextBox;
  internal TextBox RightFooterTextBox;
  internal ComboBox FontSizeComboBox;
  internal ColorPickerButton TextColorButton;
  internal Button DateButton;
  internal ContextMenu DateButtonContextMenu;
  internal Button InsertFileNameButton;
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

  static PageHeaderFooterDialog()
  {
    PageHeaderFooterDialog.ResultProperty = PageHeaderFooterDialog.ResultPropertyKey.DependencyProperty;
  }

  public PageHeaderFooterDialog(
    PdfDocument doc,
    string fileName,
    HeaderFooterSettings settings,
    int[] selectedIndexes = null)
  {
    this.InitializeComponent();
    this.document = doc ?? throw new ArgumentException(nameof (doc));
    this.fileName = fileName;
    ResultModel resultModel = settings == null ? new ResultModel() : ResultModel.FromSettings(doc, settings);
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
    this.DateButtonContextMenu.ItemsSource = (IEnumerable) PagePlaceholderFormatter.AllSupportedDateFormats.ToList<string>();
    if (string.IsNullOrEmpty(fileName))
      this.InsertFileNameButton.Visibility = Visibility.Collapsed;
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
    this.UpdatePagePreviewAsync(0);
  }

  public PageHeaderFooterDialog(PdfDocument doc, string fileName, int[] selectedIndexes = null)
    : this(doc, fileName, (HeaderFooterSettings) null, selectedIndexes)
  {
  }

  private void UpdateSelectedPageRange()
  {
    this.SelectedPageEndBox.Maximum = (double) this.document.Pages.Count;
    this.PreviewPageIndexBox.Maximum = (double) this.document.Pages.Count;
  }

  private void CancelButton_Click(object sender, RoutedEventArgs e) => e.Handled = true;

  private void OKButton_Click(object sender, RoutedEventArgs e)
  {
    Keyboard.ClearFocus();
    this.IsHitTestVisible = false;
    this.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) (() => this.DialogResult = new bool?(true)));
  }

  public ResultModel Result
  {
    get => (ResultModel) this.GetValue(PageHeaderFooterDialog.ResultProperty);
    private set => this.SetValue(PageHeaderFooterDialog.ResultPropertyKey, (object) value);
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

  private void FontSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    this.UpdateFontSize();
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

  private void DateButton_Click(object sender, RoutedEventArgs e)
  {
    this.DateButtonContextMenu.PlacementTarget = (UIElement) this.DateButton;
    this.DateButtonContextMenu.Placement = PlacementMode.Bottom;
    this.DateButtonContextMenu.IsOpen = true;
  }

  private void DateButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    if (e.ChangedButton != MouseButton.Right)
      return;
    e.Handled = true;
  }

  private void DateButtonContextMenu_Click(object sender, RoutedEventArgs e)
  {
    if (!(((FrameworkElement) e.OriginalSource)?.DataContext is string dataContext))
      return;
    bool adjusted;
    Action<string> setTextFunc1;
    TextBox lastFocusedTextBox = this.GetLastFocusedTextBox(out adjusted, out setTextFunc1);
    this.Result.DateFormat = dataContext;
    string insertText = $"<<{dataContext}>>";
    int num = adjusted ? 1 : 0;
    Action<string> setTextFunc2 = setTextFunc1;
    PageHeaderFooterDialog.InsertString(lastFocusedTextBox, insertText, num != 0, true, setTextFunc2);
    this.UpdatePreviewTextContent();
  }

  private void InsertFileNameButton_Click(object sender, RoutedEventArgs e)
  {
    if (string.IsNullOrEmpty(this.fileName))
      return;
    string insertText = this.fileName;
    try
    {
      insertText = Path.GetFileNameWithoutExtension(this.fileName);
    }
    catch
    {
    }
    if (string.IsNullOrEmpty(insertText))
      insertText = this.fileName;
    bool adjusted;
    Action<string> setTextFunc;
    PageHeaderFooterDialog.InsertString(this.GetLastFocusedTextBox(out adjusted, out setTextFunc), insertText, adjusted, true, setTextFunc);
    this.UpdatePreviewTextContent();
  }

  private void InsertPageNumberButton_Click(object sender, RoutedEventArgs e)
  {
    AddPageNumberDialog pageNumberDialog = new AddPageNumberDialog(this.document, this.Result.PageNumberFormat, this.Result.PageNumberOffset);
    pageNumberDialog.Owner = (Window) this;
    pageNumberDialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
    if (!pageNumberDialog.ShowDialog().GetValueOrDefault())
      return;
    this.Result.PageNumberFormat = pageNumberDialog.PageNumberFormats;
    this.Result.PageNumberOffset = pageNumberDialog.PageNumberOffset;
    bool adjusted;
    Action<string> setTextFunc;
    PageHeaderFooterDialog.InsertString(this.GetLastFocusedTextBox(out adjusted, out setTextFunc), $"<<{pageNumberDialog.PageNumberFormats}>>", adjusted, true, setTextFunc);
    this.UpdatePreviewTextContent();
  }

  private void ContentContainer_LostFocus(object sender, RoutedEventArgs e)
  {
    if (!(e.OriginalSource is TextBox originalSource))
      return;
    this.lastLostFocusTextbox = originalSource;
    this.UpdatePreviewTextContent();
  }

  private TextBox GetLastFocusedTextBox(out bool adjusted, out Action<string> setTextFunc)
  {
    adjusted = false;
    setTextFunc = (Action<string>) null;
    TextBox lastFocusedTextBox = this.lastLostFocusTextbox;
    if (lastFocusedTextBox == this.LeftHeaderTextBox)
      setTextFunc = (Action<string>) (s => this.Result.Text.LeftHeaderText = s);
    else if (lastFocusedTextBox == this.CenterHeaderTextBox)
      setTextFunc = (Action<string>) (s => this.Result.Text.CenterHeaderText = s);
    else if (lastFocusedTextBox == this.RightHeaderTextBox)
      setTextFunc = (Action<string>) (s => this.Result.Text.RightHeaderText = s);
    else if (lastFocusedTextBox == this.LeftFooterTextBox)
      setTextFunc = (Action<string>) (s => this.Result.Text.LeftFooterText = s);
    else if (lastFocusedTextBox == this.CenterFooterTextBox)
      setTextFunc = (Action<string>) (s => this.Result.Text.CenterFooterText = s);
    else if (lastFocusedTextBox == this.RightFooterTextBox)
    {
      setTextFunc = (Action<string>) (s => this.Result.Text.RightFooterText = s);
    }
    else
    {
      adjusted = true;
      lastFocusedTextBox = this.LeftHeaderTextBox;
      setTextFunc = (Action<string>) (s => this.Result.Text.LeftHeaderText = s);
    }
    return lastFocusedTextBox;
  }

  private static void InsertString(
    TextBox textBox,
    string insertText,
    bool adjusted,
    bool insertSpace,
    Action<string> setTextFunc)
  {
    if (textBox == null || string.IsNullOrEmpty(insertText))
      return;
    if (setTextFunc == null)
      setTextFunc = (Action<string>) (s => textBox.Text = s);
    if (!string.IsNullOrEmpty(textBox.Text))
      insertText = " " + insertText;
    int length = textBox.SelectionStart;
    int num = textBox.SelectionLength;
    if (adjusted)
    {
      length = textBox.Text.Length - 1;
      num = 0;
    }
    if (length == -1)
      length = 0;
    setTextFunc(textBox.Text.Substring(0, length) + insertText + textBox.Text.Substring(length + num));
    textBox.SelectionLength = 0;
    textBox.SelectionStart += insertText.Length;
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
      FS_SIZEF effectiveSize = this.document.Pages[pageIndex].GetEffectiveSize(this.document.Pages[pageIndex].Rotation);
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
    int num = (int) this.PreviewPageIndexBox.Value;
    if (this.document == null || num <= 0 || num > this.document.Pages.Count)
      return;
    this.UpdatePreviewTextContentCore(this.document.Pages[num - 1]);
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

  private void TextColorButton_SelectedColorChanged(
    object sender,
    ColorPickerButtonSelectedColorChangedEventArgs e)
  {
    this.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) (() => this.UpdatePreviewTextContent()));
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
    this.UpdatePreviewTextContent();
  }

  private void LeftHeaderTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
  {
  }

  private void LeftHeaderTextBox_TextChanged(object sender, TextChangedEventArgs e)
  {
    TextBox textBox = sender as TextBox;
    if (textBox.Text.Length <= 0)
      return;
    textBox.Text = textBox.Text.TrimStart('\r', '\n');
    textBox.Text = Regex.Replace(textBox.Text, "(\\r\\n)+", "\r\n");
    textBox.SelectionStart = textBox.Text.Length;
    textBox.SelectionLength = 0;
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/pageheaderfooters/pageheaderfooterdialog.xaml", UriKind.Relative));
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
        this.ContentContainer = (Grid) target;
        this.ContentContainer.LostFocus += new RoutedEventHandler(this.ContentContainer_LostFocus);
        break;
      case 2:
        this.LeftHeaderTextBox = (TextBox) target;
        this.LeftHeaderTextBox.TextChanged += new TextChangedEventHandler(this.LeftHeaderTextBox_TextChanged);
        break;
      case 3:
        this.CenterHeaderTextBox = (TextBox) target;
        this.CenterHeaderTextBox.TextChanged += new TextChangedEventHandler(this.LeftHeaderTextBox_TextChanged);
        break;
      case 4:
        this.RightHeaderTextBox = (TextBox) target;
        this.RightHeaderTextBox.TextChanged += new TextChangedEventHandler(this.LeftHeaderTextBox_TextChanged);
        break;
      case 5:
        this.LeftFooterTextBox = (TextBox) target;
        this.LeftFooterTextBox.TextChanged += new TextChangedEventHandler(this.LeftHeaderTextBox_TextChanged);
        break;
      case 6:
        this.CenterFooterTextBox = (TextBox) target;
        this.CenterFooterTextBox.TextChanged += new TextChangedEventHandler(this.LeftHeaderTextBox_TextChanged);
        break;
      case 7:
        this.RightFooterTextBox = (TextBox) target;
        this.RightFooterTextBox.TextChanged += new TextChangedEventHandler(this.LeftHeaderTextBox_TextChanged);
        break;
      case 8:
        this.FontSizeComboBox = (ComboBox) target;
        this.FontSizeComboBox.SelectionChanged += new SelectionChangedEventHandler(this.FontSizeComboBox_SelectionChanged);
        break;
      case 9:
        this.TextColorButton = (ColorPickerButton) target;
        break;
      case 10:
        this.DateButton = (Button) target;
        this.DateButton.Click += new RoutedEventHandler(this.DateButton_Click);
        this.DateButton.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(this.DateButton_PreviewMouseLeftButtonDown);
        break;
      case 11:
        this.DateButtonContextMenu = (ContextMenu) target;
        this.DateButtonContextMenu.AddHandler(MenuItem.ClickEvent, (Delegate) new RoutedEventHandler(this.DateButtonContextMenu_Click));
        break;
      case 12:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.InsertPageNumberButton_Click);
        break;
      case 13:
        this.InsertFileNameButton = (Button) target;
        this.InsertFileNameButton.Click += new RoutedEventHandler(this.InsertFileNameButton_Click);
        break;
      case 14:
        ((UIElement) target).AddHandler(ToggleButton.CheckedEvent, (Delegate) new RoutedEventHandler(this.HeaderFooterDialogPageRange_Checked));
        break;
      case 15:
        this.AllPageRadioButton = (RadioButton) target;
        break;
      case 16 /*0x10*/:
        this.SelectedPagesRadioButton = (RadioButton) target;
        break;
      case 17:
        this.SelectedPageStartBox = (NumberBox) target;
        break;
      case 18:
        this.SelectedPageEndBox = (NumberBox) target;
        break;
      case 19:
        this.SubsetComboBox = (ComboBox) target;
        this.SubsetComboBox.SelectionChanged += new SelectionChangedEventHandler(this.SubsetComboBox_SelectionChanged);
        break;
      case 20:
        this.PreviewPageIndexBox = (NumberBox) target;
        this.PreviewPageIndexBox.ValueChanged += new RoutedPropertyChangedEventHandler<double>(this.PreviewPageIndexBox_ValueChanged);
        break;
      case 21:
        this.Tore1 = (TorePaperControl) target;
        break;
      case 22:
        this.MarginControl1 = (MarginControl) target;
        break;
      case 23:
        this.Tore2 = (TorePaperControl) target;
        break;
      case 24:
        this.MarginControl2 = (MarginControl) target;
        break;
      case 25:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.CancelButton_Click);
        break;
      case 26:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.OKButton_Click);
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
