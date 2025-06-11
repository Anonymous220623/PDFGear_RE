// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Screenshots.ExtractTextResultDialog
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using CommomLib.Controls;
using Microsoft.Win32;
using Patagames.Pdf;
using Patagames.Pdf.Net;
using PDFKit.Utils;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.Controls.Screenshots;

public partial class ExtractTextResultDialog : Window, IComponentConnector
{
  private readonly PdfDocument document;
  private readonly ScreenshotDialogResult result;
  private readonly Storyboard showToastAnimation;
  private string cultureInfoName;
  private string languageDisplayName;
  private string textResult;
  internal Grid LayoutRoot;
  internal ColumnDefinition ImageColumn;
  internal Image PagePreviewImage;
  internal Canvas GeoCanvas;
  internal RichTextBox rtb;
  internal CheckBox OcrCheckBox;
  internal HyperlinkButton LanguageButton;
  internal Button CloseBtn;
  internal Button CopyBtn;
  internal Button DownloadBtn;
  internal Grid Toast;
  internal TranslateTransform ToastTrans;
  internal TextBlock ToastContent;
  internal Border ProcessingDismissBorder;
  internal ProgressRing ProcessingRing;
  private bool _contentLoaded;

  public ExtractTextResultDialog(PdfDocument document, ScreenshotDialogResult result)
  {
    if (document == null)
      throw new ArgumentNullException(nameof (document));
    if (result == null)
      throw new ArgumentNullException(nameof (result));
    this.InitializeComponent();
    this.document = document;
    this.result = result;
    this.Loaded += new RoutedEventHandler(this.ExtractTextResultDialog_Loaded);
    this.showToastAnimation = this.LayoutRoot.Resources[(object) "ShowToastAnimation"] as Storyboard;
    this.LanguageButton.Opacity = 0.0;
    if (result.Mode == ScreenshotDialogMode.Ocr || string.IsNullOrEmpty(result.ExtractedText))
    {
      GAManager.SendEvent("ExtractText", "OCRInit", "Checked", 1L);
      this.OcrCheckBox.IsChecked = new bool?(true);
    }
    else
      GAManager.SendEvent("ExtractText", "OCRInit", "Unchecked", 1L);
  }

  public ExtractTextResultDialog(ScreenshotDialogResult result)
  {
    if (result == null)
      throw new ArgumentNullException(nameof (result));
    this.InitializeComponent();
    this.result = result;
    this.Loaded += new RoutedEventHandler(this.ExtractTextResultDialog_Loaded1);
    this.showToastAnimation = this.LayoutRoot.Resources[(object) "ShowToastAnimation"] as Storyboard;
    this.LanguageButton.Opacity = 0.0;
    this.OcrCheckBox.IsChecked = new bool?(true);
  }

  private async void ExtractTextResultDialog_Loaded1(object sender, RoutedEventArgs e)
  {
    this.OcrCheckBox.Visibility = Visibility.Collapsed;
    string str = await ConfigManager.GetScreenshotOcrLanguage((string) null);
    if (string.IsNullOrEmpty(str))
      str = OcrUtils.GetDefaultCultureInfoName();
    this.cultureInfoName = str;
    this.languageDisplayName = OcrUtils.GetLanguageDisplayName(CultureInfo.GetCultureInfo(this.cultureInfoName));
    this.LanguageButton.Content = (object) this.languageDisplayName;
    this.LanguageButton.Opacity = 1.0;
    byte[] array;
    using (MemoryStream memoryStream = new MemoryStream())
    {
      this.result.OcrImage.Save((Stream) memoryStream, ImageFormat.Bmp);
      array = memoryStream.ToArray();
    }
    BitmapImage bitmapImage = new BitmapImage();
    bitmapImage.BeginInit();
    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
    bitmapImage.StreamSource = (Stream) new MemoryStream(array);
    bitmapImage.EndInit();
    this.PagePreviewImage.Source = (ImageSource) bitmapImage;
    await this.UpdateResultAsync();
  }

  private async void ExtractTextResultDialog_Loaded(object sender, RoutedEventArgs e)
  {
    ExtractTextResultDialog textResultDialog = this;
    if (textResultDialog.OcrCheckBox.IsChecked.GetValueOrDefault())
      GAManager.SendEvent("ExtractText", "Show", textResultDialog.result.Mode.ToString() + "Checked", 1L);
    else
      GAManager.SendEvent("ExtractText", "Show", textResultDialog.result.Mode.ToString() + "Unchecked", 1L);
    string str = await ConfigManager.GetScreenshotOcrLanguage((string) null);
    if (string.IsNullOrEmpty(str))
      str = OcrUtils.GetDefaultCultureInfoName();
    textResultDialog.cultureInfoName = str;
    textResultDialog.languageDisplayName = OcrUtils.GetLanguageDisplayName(CultureInfo.GetCultureInfo(textResultDialog.cultureInfoName));
    textResultDialog.LanguageButton.Content = (object) textResultDialog.languageDisplayName;
    textResultDialog.LanguageButton.Opacity = 1.0;
    PdfPage page = textResultDialog.document.Pages[textResultDialog.result.PageIndex];
    FS_RECTF boundBox = page.GetEffectiveBox();
    textResultDialog.PagePreviewImage.Height = textResultDialog.PagePreviewImage.Width / (double) boundBox.Width * (double) boundBox.Height;
    double num1 = textResultDialog.PagePreviewImage.Height - ((FrameworkElement) textResultDialog.PagePreviewImage.Parent).ActualHeight;
    if (num1 > 0.0)
      textResultDialog.Height += num1;
    else
      textResultDialog.GeoCanvas.Margin = new Thickness(0.0, -num1 / 2.0, 0.0, 0.0);
    WriteableBitmap image = (WriteableBitmap) null;
    try
    {
      image = await ScreenshotDialog.GetPageImageAsync(textResultDialog.PagePreviewImage.Width * 4.0, textResultDialog.PagePreviewImage.Height * 4.0, page);
    }
    catch
    {
    }
    if (image == null)
    {
      textResultDialog.ImageColumn.Width = new GridLength(0.0, GridUnitType.Pixel);
    }
    else
    {
      textResultDialog.PagePreviewImage.Source = (ImageSource) image;
      double num2 = textResultDialog.PagePreviewImage.Width / (double) boundBox.Width;
      FS_RECTF selectedRect = textResultDialog.result.SelectedRect;
      Rect rect = new Rect(((double) selectedRect.left - (double) boundBox.left) * num2, ((double) boundBox.top - (double) selectedRect.top) * num2, ((double) selectedRect.right - (double) selectedRect.left) * num2, ((double) selectedRect.top - (double) selectedRect.bottom) * num2);
      Rectangle rectangle = new Rectangle();
      rectangle.Stroke = (Brush) Brushes.Red;
      rectangle.StrokeThickness = 1.0;
      rectangle.Width = rect.Width;
      rectangle.Height = rect.Height;
      Rectangle element = rectangle;
      Canvas.SetLeft((UIElement) element, rect.Left);
      Canvas.SetTop((UIElement) element, rect.Top);
      textResultDialog.GeoCanvas.Children.Add((UIElement) element);
    }
    await textResultDialog.UpdateResultAsync();
    image = (WriteableBitmap) null;
  }

  private void CloseBtn_Click(object sender, RoutedEventArgs e) => this.Close();

  private async void CopyBtn_Click(object sender, RoutedEventArgs e)
  {
    ((UIElement) sender).IsEnabled = false;
    try
    {
      Clipboard.SetDataObject((object) this.textResult);
      this.showToastAnimation.SkipToFill();
      this.showToastAnimation.Begin();
      await Task.Delay(300);
    }
    catch
    {
      this.showToastAnimation.SkipToFill();
    }
    ((UIElement) sender).IsEnabled = true;
    if (this.OcrCheckBox.IsChecked.GetValueOrDefault())
      GAManager.SendEvent("ExtractText", "Copy", this.result.Mode.ToString() + "Checked", 1L);
    else
      GAManager.SendEvent("ExtractText", "Copy", this.result.Mode.ToString() + "Unchecked", 1L);
  }

  private async void DownloadBtn_Click(object sender, RoutedEventArgs e)
  {
    ((UIElement) sender).IsEnabled = false;
    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
    saveFileDialog1.AddExtension = true;
    saveFileDialog1.Filter = "txt|*.txt";
    saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
    saveFileDialog1.FileName = "Extract.txt";
    SaveFileDialog saveFileDialog2 = saveFileDialog1;
    if (saveFileDialog2.ShowDialog().GetValueOrDefault())
    {
      string fileName = saveFileDialog2.FileName;
      try
      {
        File.WriteAllText(fileName, this.textResult, Encoding.UTF8);
        Mouse.OverrideCursor = Cursors.AppStarting;
        int num = await ExplorerUtils.SelectItemInExplorerAsync(fileName, new CancellationToken()) ? 1 : 0;
        Mouse.OverrideCursor = (Cursor) null;
      }
      catch
      {
      }
    }
    ((UIElement) sender).IsEnabled = true;
    if (this.OcrCheckBox.IsChecked.GetValueOrDefault())
      GAManager.SendEvent("ExtractText", "Save", this.result.Mode.ToString() + "Checked", 1L);
    else
      GAManager.SendEvent("ExtractText", "Save", this.result.Mode.ToString() + "Unchecked", 1L);
  }

  private void PagePreviewImage_SizeChanged(object sender, SizeChangedEventArgs e)
  {
  }

  private async void OcrCheckBox_Click(object sender, RoutedEventArgs e)
  {
    ExtractTextResultDialog textResultDialog = this;
    // ISSUE: reference to a compiler-generated method
    await textResultDialog.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) new Action(textResultDialog.\u003COcrCheckBox_Click\u003Eb__14_0));
  }

  private async void LanguageButton_Click(object sender, RoutedEventArgs e)
  {
    ExtractTextResultDialog textResultDialog = this;
    GAManager.SendEvent("ExtractText", "ChangeLanguage", "Count", 1L);
    OcrSelectLanguageDialog selectLanguageDialog = new OcrSelectLanguageDialog(textResultDialog.cultureInfoName);
    selectLanguageDialog.Owner = (Window) textResultDialog;
    selectLanguageDialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
    if (!selectLanguageDialog.ShowDialog().GetValueOrDefault() || !(textResultDialog.cultureInfoName != selectLanguageDialog.SelectedCultureInfoName))
      return;
    textResultDialog.cultureInfoName = selectLanguageDialog.SelectedCultureInfoName;
    textResultDialog.languageDisplayName = selectLanguageDialog.SelectedDisplayName;
    await ConfigManager.SetScreenshotOcrLanguageAsync(textResultDialog.cultureInfoName);
    textResultDialog.LanguageButton.Content = (object) textResultDialog.languageDisplayName;
    await textResultDialog.UpdateOcrResultAsync();
  }

  private void SetProcessingState(bool processing)
  {
    if (processing)
    {
      this.ProcessingDismissBorder.Visibility = Visibility.Visible;
      this.ProcessingRing.IsActive = true;
      this.CopyBtn.IsEnabled = false;
      this.DownloadBtn.IsEnabled = false;
    }
    else
    {
      this.ProcessingDismissBorder.Visibility = Visibility.Collapsed;
      this.ProcessingRing.IsActive = false;
      this.CopyBtn.IsEnabled = true;
      this.DownloadBtn.IsEnabled = true;
    }
  }

  private void SetResult(string text)
  {
    this.textResult = text;
    this.rtb.Document.Blocks.Clear();
    this.rtb.AppendText(text);
    this.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) (() =>
    {
      this.rtb.SelectAll();
      this.rtb.Focus();
      Keyboard.Focus((IInputElement) this.rtb);
    }));
    this.SetProcessingState(false);
  }

  private async Task UpdateResultAsync()
  {
    if (this.OcrCheckBox.IsChecked.GetValueOrDefault())
    {
      GAManager.SendEvent("ExtractText", "ExtractText", "OCRChecked", 1L);
      await this.UpdateOcrResultAsync();
    }
    else
    {
      GAManager.SendEvent("ExtractText", "ExtractText", "OCRUnChecked", 1L);
      this.SetResult(this.result.ExtractedText);
      this.SetProcessingState(false);
    }
  }

  private async Task UpdateOcrResultAsync()
  {
    this.SetProcessingState(true);
    try
    {
      if (this.result.RotatedImage != null)
        this.SetResult(await OcrUtils.GetStringAsync((BitmapSource) this.result.RotatedImage, CultureInfo.GetCultureInfo(this.cultureInfoName)));
      if (this.result.OcrImage == null)
        return;
      this.SetResult(await OcrUtils.GetStringAsync(this.result.OcrImage, CultureInfo.GetCultureInfo(this.cultureInfoName)));
    }
    catch (Exception ex)
    {
      GAManager.SendEvent("Exception", "OCR", ex.Message ?? "", 1L);
      CommomLib.Commom.Log.WriteLog($"OCR failed: {ex.Message}\n\r{ex.StackTrace}");
      this.SetProcessingState(false);
    }
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/screenshots/extracttextresultdialog.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.LayoutRoot = (Grid) target;
        break;
      case 2:
        this.ImageColumn = (ColumnDefinition) target;
        break;
      case 3:
        this.PagePreviewImage = (Image) target;
        break;
      case 4:
        this.GeoCanvas = (Canvas) target;
        break;
      case 5:
        this.rtb = (RichTextBox) target;
        break;
      case 6:
        this.OcrCheckBox = (CheckBox) target;
        this.OcrCheckBox.Click += new RoutedEventHandler(this.OcrCheckBox_Click);
        break;
      case 7:
        this.LanguageButton = (HyperlinkButton) target;
        this.LanguageButton.Click += new RoutedEventHandler(this.LanguageButton_Click);
        break;
      case 8:
        this.CloseBtn = (Button) target;
        this.CloseBtn.Click += new RoutedEventHandler(this.CloseBtn_Click);
        break;
      case 9:
        this.CopyBtn = (Button) target;
        this.CopyBtn.Click += new RoutedEventHandler(this.CopyBtn_Click);
        break;
      case 10:
        this.DownloadBtn = (Button) target;
        this.DownloadBtn.Click += new RoutedEventHandler(this.DownloadBtn_Click);
        break;
      case 11:
        this.Toast = (Grid) target;
        break;
      case 12:
        this.ToastTrans = (TranslateTransform) target;
        break;
      case 13:
        this.ToastContent = (TextBlock) target;
        break;
      case 14:
        this.ProcessingDismissBorder = (Border) target;
        break;
      case 15:
        this.ProcessingRing = (ProgressRing) target;
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
