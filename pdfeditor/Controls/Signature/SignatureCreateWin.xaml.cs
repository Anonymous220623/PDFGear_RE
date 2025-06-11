// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Signature.SignatureCreateWin
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Win32;
using pdfeditor.Utils;
using pdfeditor.ViewModels;
using PDFKit.Utils;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace pdfeditor.Controls.Signature;

public partial class SignatureCreateWin : Window, IComponentConnector
{
  private ObservableCollection<MenuModel> MainMenus;
  private ObservableCollection<FontItem> FontItemList;
  private string FileDiaoligFiePath = string.Empty;
  private SignatureCreateDialogResult resultModel = new SignatureCreateDialogResult();
  public static readonly DependencyProperty ClearVisibleProperty = DependencyProperty.Register(nameof (ClearVisible), typeof (bool), typeof (SignatureCreateWin), new PropertyMetadata((object) false));
  private MenuModel SelectedMenuModel;
  private double space = 1.0;
  internal Grid SingtruePanel;
  internal ListBox Menus;
  internal ComboBox WriteStrokeWidths;
  internal ComboBox FontFamilysCtrl;
  internal Border body;
  internal Canvas PathCtrl;
  internal InkCanvas inkCanvas;
  internal Button PictureCtrl;
  internal System.Windows.Controls.Image showPicture;
  internal TextBox TypeWriterCtrl;
  internal Button btnClear;
  internal CheckBox ckbRemoveBg;
  internal System.Windows.Controls.Image imgHelp;
  internal Button btnCancel;
  internal Button btnOk;
  private bool _contentLoaded;

  public bool ClearVisible
  {
    get => (bool) this.GetValue(SignatureCreateWin.ClearVisibleProperty);
    set => this.SetValue(SignatureCreateWin.ClearVisibleProperty, (object) value);
  }

  public SignatureCreateDialogResult ResultModel => this.resultModel;

  public SignatureCreateWin()
  {
    this.InitializeComponent();
    this.MainMenus = new ObservableCollection<MenuModel>();
    this.FontItemList = new ObservableCollection<FontItem>();
    this.InitMenu();
    this.Menus.ItemsSource = (IEnumerable) this.MainMenus;
    this.Menus.SelectedIndex = 0;
    this.WriteStrokeWidths.SelectedIndex = 0;
    this.inkCanvas.StrokeCollected += (InkCanvasStrokeCollectedEventHandler) ((o, e) => this.ShowClear());
    this.TypeWriterCtrl.TextChanged += (TextChangedEventHandler) ((o, e) =>
    {
      this.ShowClear();
      this.TypeWriterCtrl.FontSize = this.CalculateMaxWidthFontSize();
    });
    this.WriteStrokeWidths.SelectionChanged += (SelectionChangedEventHandler) ((o, e) =>
    {
      string str = e.AddedItems.Count > 0 ? (string) e.AddedItems[0] : "1pt";
      double result;
      if (!double.TryParse(str.Substring(0, str.Length - 2), out result))
        return;
      this.inkCanvas.DefaultDrawingAttributes.Width = result;
      this.inkCanvas.DefaultDrawingAttributes.Height = result;
    });
    this.btnOk.Click += (RoutedEventHandler) ((o, e) =>
    {
      if (!this.CheckOk())
        return;
      Ioc.Default.GetRequiredService<MainViewModel>();
      if (this.Menus.SelectedIndex == 0)
        this.SavePictureImg();
      else if (this.Menus.SelectedIndex == 2)
        this.SaveInkToImg();
      else if (this.Menus.SelectedIndex == 1 && !this.SaveTypeImg())
        return;
      this.showPicture.Source = (ImageSource) null;
      this.ResultModel.RemoveBackground = this.ckbRemoveBg.IsChecked.Value;
      if (this.ResultModel.RemoveBackground)
        this.SaveConfigRemoveBg(this.ResultModel.ImageFilePath);
      try
      {
        GAManager.SendEvent("PdfStampAnnotationSignature", "SaveSignature", this.GetTemplateCount().ToString(), 1L);
      }
      catch
      {
      }
      this.DialogResult = new bool?(true);
    });
    this.btnCancel.Click += (RoutedEventHandler) ((o, e) =>
    {
      Ioc.Default.GetRequiredService<MainViewModel>();
      this.showPicture.Source = (ImageSource) null;
      this.DialogResult = new bool?(false);
      this.Close();
    });
    this.inkCanvas.UseCustomCursor = true;
    this.inkCanvas.Cursor = CursorHelper.CreateCursor(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Style\\\\Resources\\\\Annonate\\\\signaturewrite.png"), 11U, 32U /*0x20*/);
  }

  private void SaveConfigRemoveBg(string fileName) => ConfigManager.AddSignatureRemoveBg(fileName);

  private int GetTemplateCount()
  {
    string path = Path.Combine(AppDataHelper.LocalFolder, "Signature");
    if (!Directory.Exists(path))
      Directory.CreateDirectory(path);
    return ((IEnumerable<string>) Directory.GetFiles(path)).ToList<string>().Count;
  }

  private bool CheckOk()
  {
    return (!(this.SelectedMenuModel.Title == pdfeditor.Properties.Resources.WinSignatureMenuPictureContent) || this.showPicture.Source != null) && (!(this.SelectedMenuModel.Title == pdfeditor.Properties.Resources.WinSignatureMenuWriteContent) || this.inkCanvas.Strokes != null && this.inkCanvas.Strokes.Count != 0);
  }

  private void ShowClear()
  {
    string title = this.SelectedMenuModel.Title;
    switch (this.SelectedMenuModel.Tag)
    {
      case "Write":
        this.ClearVisible = this.inkCanvas.Strokes.Count > 0;
        break;
      case "Picture":
        this.ClearVisible = this.showPicture.Source != null;
        break;
      case "Type":
        this.ClearVisible = !string.IsNullOrEmpty(this.TypeWriterCtrl.Text);
        break;
    }
  }

  private void InitMenu()
  {
    this.MainMenus.Clear();
    this.MainMenus.Add(new MenuModel()
    {
      Title = pdfeditor.Properties.Resources.WinSignatureMenuPictureContent,
      Tag = "Picture"
    });
    this.MainMenus.Add(new MenuModel()
    {
      Title = pdfeditor.Properties.Resources.WinSignatureMenuInputContent,
      Tag = "Type"
    });
    this.MainMenus.Add(new MenuModel()
    {
      Title = pdfeditor.Properties.Resources.WinSignatureMenuWriteContent,
      Tag = "Write"
    });
    this.GetLocaltem();
    this.FontFamilysCtrl.ItemsSource = (IEnumerable) this.FontItemList;
  }

  private void SaveInkToImg()
  {
    MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
    if (this.SelectedMenuModel == null || !(this.SelectedMenuModel.Tag == "Write") || this.inkCanvas.Strokes.Count <= 0)
      return;
    RenderTargetBitmap bitmap = new RenderTargetBitmap((int) this.PathCtrl.ActualWidth, (int) this.PathCtrl.ActualHeight, 96.0, 96.0, PixelFormats.Default);
    bitmap.Render((Visual) this.inkCanvas);
    PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
    BitmapSource source = this.ReplaceTransparency((BitmapSource) bitmap, System.Windows.Media.Brushes.Transparent.Color);
    pngBitmapEncoder.Frames.Add(BitmapFrame.Create(source));
    bitmap.Clear();
    string str1 = DateTime.Now.ToString("yyyyMMddHHmmss");
    string str2 = Path.Combine(AppDataHelper.LocalFolder, "Signature");
    if (!Directory.Exists(str2))
      Directory.CreateDirectory(str2);
    string path = Path.Combine(str2, $"SignatureWrite{str1}.png");
    try
    {
      using (Stream stream = (Stream) File.Create(path))
      {
        pngBitmapEncoder.Save(stream);
        this.ResultModel.ImageFilePath = path;
        requiredService.AnnotationToolbar.StampImgFileOkTime = DateTime.Now;
      }
    }
    catch (Exception ex)
    {
      this.ResultModel.ImageFilePath = string.Empty;
    }
    finally
    {
      bitmap.Freeze();
      GC.Collect();
    }
  }

  private bool SaveTypeImg()
  {
    MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
    if (this.TypeWriterCtrl.Text.Trim() == string.Empty)
    {
      int num = (int) ModernMessageBox.Show(pdfeditor.Properties.Resources.WinSignatureMenuInputIsEmptyMsg, UtilManager.GetProductName());
      return false;
    }
    if (this.SelectedMenuModel == null || !(this.SelectedMenuModel.Tag == "Type") || string.IsNullOrEmpty(this.TypeWriterCtrl.Text))
      return false;
    FormattedText formattedText = this.MeasureTextWidth(this.TypeWriterCtrl.FontSize);
    RenderTargetBitmap bitmap = new RenderTargetBitmap((int) formattedText.Width, (int) formattedText.Height, 96.0, 96.0, PixelFormats.Default);
    bitmap.Render((Visual) this.TypeWriterCtrl);
    PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
    BitmapSource textRender = this.CreateTextRender((BitmapSource) bitmap, System.Windows.Media.Brushes.Transparent.Color, this.TypeWriterCtrl.Text);
    pngBitmapEncoder.Frames.Add(BitmapFrame.Create(textRender));
    bitmap.Clear();
    string str1 = DateTime.Now.ToString("yyyyMMddHHmmss");
    string str2 = Path.Combine(AppDataHelper.LocalFolder, "Signature");
    if (!Directory.Exists(str2))
      Directory.CreateDirectory(str2);
    string path = Path.Combine(str2, $"SignatureWrite{str1}.png");
    try
    {
      using (Stream stream = (Stream) File.Create(path))
      {
        pngBitmapEncoder.Save(stream);
        this.ResultModel.ImageFilePath = path;
        requiredService.AnnotationToolbar.StampImgFileOkTime = DateTime.Now;
      }
    }
    catch (Exception ex)
    {
      this.ResultModel.ImageFilePath = string.Empty;
      return false;
    }
    finally
    {
      bitmap.Freeze();
      GC.Collect();
    }
    return true;
  }

  private void Menus_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    if (!((sender is Selector selector ? selector.SelectedItem : (object) null) is MenuModel selectedItem))
      return;
    this.SelectedMenuModel = selectedItem;
    this.ShowClear();
    string title = this.SelectedMenuModel.Title;
    switch (this.SelectedMenuModel.Tag)
    {
      case "Picture":
        this.PathCtrl.Visibility = Visibility.Collapsed;
        this.WriteStrokeWidths.Visibility = Visibility.Collapsed;
        this.showPicture.Visibility = Visibility.Visible;
        this.ckbRemoveBg.Visibility = Visibility.Visible;
        this.imgHelp.Visibility = Visibility.Visible;
        this.PictureCtrl.Visibility = this.showPicture.Source == null ? Visibility.Visible : Visibility.Collapsed;
        this.TypeWriterCtrl.Visibility = Visibility.Collapsed;
        this.FontFamilysCtrl.Visibility = Visibility.Collapsed;
        break;
      case "Write":
        this.PathCtrl.Visibility = Visibility.Visible;
        this.PictureCtrl.Visibility = Visibility.Collapsed;
        this.ckbRemoveBg.Visibility = Visibility.Collapsed;
        this.imgHelp.Visibility = Visibility.Collapsed;
        this.WriteStrokeWidths.Visibility = Visibility.Visible;
        this.showPicture.Visibility = Visibility.Collapsed;
        this.TypeWriterCtrl.Visibility = Visibility.Collapsed;
        this.FontFamilysCtrl.Visibility = Visibility.Collapsed;
        break;
      case "Type":
        this.PathCtrl.Visibility = Visibility.Collapsed;
        this.PictureCtrl.Visibility = Visibility.Collapsed;
        this.ckbRemoveBg.Visibility = Visibility.Collapsed;
        this.imgHelp.Visibility = Visibility.Collapsed;
        this.WriteStrokeWidths.Visibility = Visibility.Collapsed;
        this.showPicture.Visibility = Visibility.Collapsed;
        this.TypeWriterCtrl.Visibility = Visibility.Visible;
        this.FontFamilysCtrl.Visibility = Visibility.Visible;
        this.TypeWriterCtrl.Focus();
        break;
    }
  }

  private void btnClear_Click(object sender, RoutedEventArgs e)
  {
    if (this.SelectedMenuModel == null)
      return;
    string title = this.SelectedMenuModel.Title;
    string tag = this.SelectedMenuModel.Tag;
    if (tag == "Picture")
    {
      this.showPicture.Source = (ImageSource) null;
      this.PictureCtrl.Visibility = Visibility.Visible;
      this.ShowClear();
    }
    if (tag == "Write")
    {
      this.inkCanvas.Strokes.Clear();
      this.ShowClear();
    }
    if (!(tag == "Type"))
      return;
    this.TypeWriterCtrl.Text = string.Empty;
    this.ShowClear();
  }

  private void PictureCtrl_Click(object sender, RoutedEventArgs e)
  {
    Ioc.Default.GetRequiredService<MainViewModel>();
    OpenFileDialog openFileDialog1 = new OpenFileDialog();
    openFileDialog1.Filter = "All Image Files|*.bmp;*.ico;*.gif;*.jpeg;*.jpg;*.png;*.tif;*.tiff|Windows Bitmap(*.bmp)|*.bmp|Windows Icon(*.ico)|*.ico|Graphics Interchange Format (*.gif)|(*.gif)|JPEG File Interchange Format (*.jpg)|*.jpg;*.jpeg|Portable Network Graphics (*.png)|*.png|Tag Image File Format (*.tif)|*.tif;*.tiff";
    openFileDialog1.ShowReadOnly = false;
    openFileDialog1.ReadOnlyChecked = true;
    OpenFileDialog openFileDialog2 = openFileDialog1;
    if (openFileDialog2.ShowDialog().GetValueOrDefault())
    {
      if (!string.IsNullOrEmpty(openFileDialog2.FileName))
      {
        try
        {
          this.showPicture.Source = (ImageSource) new BitmapImage(new Uri(openFileDialog2.FileName, UriKind.Absolute));
          this.FileDiaoligFiePath = openFileDialog2.FileName;
          this.PictureCtrl.Visibility = Visibility.Collapsed;
          this.ShowClear();
          goto label_5;
        }
        catch
        {
          DrawUtils.ShowUnsupportedImageMessage();
          return;
        }
      }
    }
    this.ResultModel.ImageFilePath = string.Empty;
label_5:
    this.Activate();
  }

  public void SavePictureImg()
  {
    if (string.IsNullOrEmpty(this.FileDiaoligFiePath))
      return;
    Ioc.Default.GetRequiredService<MainViewModel>();
    string str1 = DateTime.Now.ToString("yyyyMMddHHmmss");
    string str2 = Path.Combine(AppDataHelper.LocalFolder, "Signature");
    if (!Directory.Exists(str2))
      Directory.CreateDirectory(str2);
    string destFileName = Path.Combine(str2, $"SignatureWrite{str1}.png");
    File.Copy(this.FileDiaoligFiePath, destFileName, true);
    this.ResultModel.ImageFilePath = destFileName;
    this.FileDiaoligFiePath = string.Empty;
  }

  public void FileStreamUseCopy(string source, string target)
  {
    using (FileStream fileStream1 = new FileStream(source, FileMode.OpenOrCreate, FileAccess.Read))
    {
      using (FileStream fileStream2 = new FileStream(target, FileMode.OpenOrCreate, FileAccess.Write))
      {
        byte[] buffer = new byte[2097152 /*0x200000*/];
        int count;
        while ((count = fileStream1.Read(buffer, 0, buffer.Length)) > 0)
          fileStream2.Write(buffer, 0, count);
      }
    }
  }

  public BitmapSource ReplaceTransparency(BitmapSource bitmap, System.Windows.Media.Color color)
  {
    DrawingVisual drawingVisual = new DrawingVisual();
    Rect rectangle = new Rect(0.0, 0.0, (double) bitmap.PixelWidth, (double) bitmap.PixelHeight);
    DrawingContext drawingContext = drawingVisual.RenderOpen();
    drawingContext.DrawRectangle((System.Windows.Media.Brush) new SolidColorBrush(color), (System.Windows.Media.Pen) null, rectangle);
    drawingContext.DrawImage((ImageSource) bitmap, rectangle);
    drawingContext.Close();
    RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(bitmap.PixelWidth, bitmap.PixelHeight, 96.0, 96.0, PixelFormats.Pbgra32);
    renderTargetBitmap.Render((Visual) drawingVisual);
    return (BitmapSource) renderTargetBitmap;
  }

  public BitmapSource CreateTextRender(BitmapSource bitmap, System.Windows.Media.Color color, string Text)
  {
    DrawingVisual drawingVisual = new DrawingVisual();
    Rect rectangle = new Rect(0.0, 0.0, (double) bitmap.PixelWidth, (double) bitmap.PixelHeight);
    DrawingContext drawingContext = drawingVisual.RenderOpen();
    drawingContext.DrawRectangle((System.Windows.Media.Brush) new SolidColorBrush(color), (System.Windows.Media.Pen) null, rectangle);
    System.Windows.Media.FontFamily fontFamily = this.TypeWriterCtrl.FontFamily;
    drawingContext.DrawText(new FormattedText(Text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(fontFamily, this.FontStyle, this.FontWeight, this.FontStretch), this.TypeWriterCtrl.FontSize, (System.Windows.Media.Brush) System.Windows.Media.Brushes.Black, VisualTreeHelper.GetDpi((Visual) this).PixelsPerDip), new System.Windows.Point(rectangle.Left, rectangle.Top));
    drawingContext.Close();
    RenderTargetBitmap textRender = new RenderTargetBitmap(bitmap.PixelWidth, bitmap.PixelHeight, 96.0, 96.0, PixelFormats.Pbgra32);
    textRender.Render((Visual) drawingVisual);
    return (BitmapSource) textRender;
  }

  private FormattedText MeasureTextWidth(double fontSize)
  {
    return new FormattedText(this.TypeWriterCtrl.Text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(this.TypeWriterCtrl.FontFamily, this.FontStyle, this.FontWeight, this.FontStretch), fontSize, (System.Windows.Media.Brush) System.Windows.Media.Brushes.Black, VisualTreeHelper.GetDpi((Visual) this).PixelsPerDip);
  }

  private double CalculateMaxWidthFontSize()
  {
    double width = this.TypeWriterCtrl.Width;
    double fontSize1 = 96.0;
    if (double.IsNaN(width) || this.MeasureTextWidth(fontSize1).WidthIncludingTrailingWhitespace < width)
      return fontSize1;
    double num = 0.0;
    while (fontSize1 > 0.0)
    {
      double fontSize2 = (fontSize1 + num) / 2.0;
      if (this.MeasureTextWidth(fontSize2).WidthIncludingTrailingWhitespace < width && this.MeasureTextWidth(fontSize2 + this.space).WidthIncludingTrailingWhitespace > width)
        return fontSize2;
      if (this.MeasureTextWidth(fontSize2).WidthIncludingTrailingWhitespace < width)
      {
        num = fontSize2 + this.space;
      }
      else
      {
        if (this.MeasureTextWidth(fontSize2).WidthIncludingTrailingWhitespace <= width)
          return fontSize2;
        fontSize1 = fontSize2 - this.space;
      }
    }
    return this.FontSize;
  }

  private void inkCanvas_Loaded(object sender, RoutedEventArgs e)
  {
    InkCanvas inkCanvas = (InkCanvas) sender;
    inkCanvas.DefaultDrawingAttributes.Width = 1.0;
    inkCanvas.DefaultDrawingAttributes.Height = 1.0;
    inkCanvas.DefaultDrawingAttributes.FitToCurve = true;
  }

  private void GetLocaltem()
  {
    System.Drawing.FontFamily[] source;
    try
    {
      source = new InstalledFontCollection().Families;
    }
    catch (Exception ex)
    {
      source = new System.Drawing.FontFamily[0];
    }
    this.FontItemList?.Clear();
    FontItem defaultItem = new FontItem();
    string defaultUiFont = PdfFontUtils.TryGetDefaultUIFont(CultureInfo.CurrentUICulture);
    if (!string.IsNullOrEmpty(defaultUiFont))
    {
      defaultItem.Name = defaultUiFont;
      defaultItem.DisplayName = defaultUiFont;
    }
    else
    {
      defaultItem.Name = this.FontFamily.Source;
      defaultItem.DisplayName = this.FontFamily.Source;
    }
    System.Drawing.FontFamily fontFamily1 = ((IEnumerable<System.Drawing.FontFamily>) source).FirstOrDefault<System.Drawing.FontFamily>((Func<System.Drawing.FontFamily, bool>) (x => x.Name == defaultItem.Name));
    if (fontFamily1 != null)
    {
      this.FontFamilysCtrl.SelectedIndex = ((IEnumerable<System.Drawing.FontFamily>) source).ToList<System.Drawing.FontFamily>().IndexOf(fontFamily1);
    }
    else
    {
      this.FontItemList.Add(defaultItem);
      this.FontFamilysCtrl.SelectedIndex = 0;
    }
    foreach (System.Drawing.FontFamily fontFamily2 in source)
      this.FontItemList.Add(new FontItem()
      {
        Name = fontFamily2.Name,
        DisplayName = fontFamily2.Name
      });
  }

  private void FontFamilysCtrl_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    if (!(this.FontFamilysCtrl.SelectedItem is FontItem selectedItem) || !(selectedItem.Name != ""))
      return;
    this.TypeWriterCtrl.FontFamily = new System.Windows.Media.FontFamily(selectedItem.Name);
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/signature/signaturecreatewin.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.SingtruePanel = (Grid) target;
        break;
      case 2:
        this.Menus = (ListBox) target;
        this.Menus.SelectionChanged += new SelectionChangedEventHandler(this.Menus_SelectionChanged);
        break;
      case 3:
        this.WriteStrokeWidths = (ComboBox) target;
        break;
      case 4:
        this.FontFamilysCtrl = (ComboBox) target;
        this.FontFamilysCtrl.SelectionChanged += new SelectionChangedEventHandler(this.FontFamilysCtrl_SelectionChanged);
        break;
      case 5:
        this.body = (Border) target;
        break;
      case 6:
        this.PathCtrl = (Canvas) target;
        break;
      case 7:
        this.inkCanvas = (InkCanvas) target;
        this.inkCanvas.Loaded += new RoutedEventHandler(this.inkCanvas_Loaded);
        break;
      case 8:
        this.PictureCtrl = (Button) target;
        this.PictureCtrl.Click += new RoutedEventHandler(this.PictureCtrl_Click);
        break;
      case 9:
        this.showPicture = (System.Windows.Controls.Image) target;
        break;
      case 10:
        this.TypeWriterCtrl = (TextBox) target;
        break;
      case 11:
        this.btnClear = (Button) target;
        this.btnClear.Click += new RoutedEventHandler(this.btnClear_Click);
        break;
      case 12:
        this.ckbRemoveBg = (CheckBox) target;
        break;
      case 13:
        this.imgHelp = (System.Windows.Controls.Image) target;
        break;
      case 14:
        this.btnCancel = (Button) target;
        break;
      case 15:
        this.btnOk = (Button) target;
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
