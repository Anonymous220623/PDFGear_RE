// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Watermark.WatermarkEditWin
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Win32;
using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.BasicTypes;
using pdfeditor.Controls.ColorPickers;
using pdfeditor.Controls.PageEditor;
using pdfeditor.Utils;
using pdfeditor.ViewModels;
using PDFKit.Utils;
using PDFKit.Utils.WatermarkUtils;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace pdfeditor.Controls.Watermark;

public partial class WatermarkEditWin : Window, IComponentConnector
{
  private WatermarkImageModel ImageModel;
  private WatermarkTextModel TextModel;
  private WatermarkParam watermarkParam;
  private const float HTileSpan = 50f;
  private const float VTileSpan = 50f;
  private string SelectedFontground = "#FF000000";
  public static readonly DependencyProperty CustomRotateProperty = DependencyProperty.Register(nameof (CustomRotate), typeof (string), typeof (WatermarkEditWin), new PropertyMetadata((object) "0"));
  private readonly List<string> _colorPresetList = new List<string>()
  {
    "#f44336",
    "#e91e63",
    "#9c27b0",
    "#673ab7",
    "#3f51b5",
    "#2196f3",
    "#03a9f4",
    "#00bcd4",
    "#009688",
    "#4caf50",
    "#8bc34a",
    "#cddc39",
    "#ffeb3b",
    "#ffc107",
    "#ff9800",
    "#ff5722",
    "#795548",
    "#000000"
  };
  internal TextBox txtWatermarkConent;
  internal ComboBox fontSizeComboBox;
  internal ColorPickerButton WatermarkColorPicker;
  internal Popup PopColor;
  internal WrapPanel PART_PanelColor;
  internal ComboBox rotateComboBox;
  internal ComboBox opacityCB;
  internal CheckBox checkTile;
  internal UIElementAligent alignmentSelector;
  internal RadioButton CurrentPageRadioButton;
  internal RadioButton AllPagesRadioButton;
  internal RadioButton SelectedPagesRadioButton;
  internal PageRangeTextBox RangeBox;
  internal ComboBox applyToComboBox;
  internal Button btnCancel;
  internal Button btnOk;
  internal RadioButton textWatermarkRadioButton;
  internal RadioButton fileWatermarkRadioButton;
  internal TextBlock txtWatermarkfile;
  internal Button btnOpenFile;
  private bool _contentLoaded;

  public string CustomRotate
  {
    get => (string) this.GetValue(WatermarkEditWin.CustomRotateProperty);
    set => this.SetValue(WatermarkEditWin.CustomRotateProperty, (object) value);
  }

  public WatermarkEditWin()
  {
    this.InitializeComponent();
    this.InitColorButton();
    this.Loaded += new RoutedEventHandler(this.WatermarkEditWin_Loaded);
  }

  private void InitColorButton()
  {
    this.PART_PanelColor.Children.Clear();
    foreach (string colorPreset in this._colorPresetList)
      this.PART_PanelColor.Children.Add((UIElement) this.CreateColorButton(colorPreset));
  }

  private Button CreateColorButton(string colorStr)
  {
    SolidColorBrush solidColorBrush = new SolidColorBrush((System.Windows.Media.Color) (System.Windows.Media.ColorConverter.ConvertFromString(colorStr) ?? (object) new System.Windows.Media.Color()));
    Button colorButton = new Button();
    colorButton.Background = (System.Windows.Media.Brush) solidColorBrush;
    colorButton.Margin = new Thickness(6.0);
    Border border = new Border();
    border.Background = (System.Windows.Media.Brush) solidColorBrush;
    border.Width = 12.0;
    border.Height = 12.0;
    border.CornerRadius = new CornerRadius(2.0);
    colorButton.Content = (object) border;
    colorButton.Click += (RoutedEventHandler) ((s, e) =>
    {
      this.SelectedFontground = (s as Button).Background.ToString();
      this.PopColor.IsOpen = false;
    });
    return colorButton;
  }

  private void WatermarkEditWin_Loaded(object sender, RoutedEventArgs e)
  {
    this.WatermarkColorPicker.SelectedColor = (System.Windows.Media.Color) System.Windows.Media.ColorConverter.ConvertFromString(this.SelectedFontground);
  }

  private async void btnOk_Click(object sender, RoutedEventArgs e)
  {
    WatermarkEditWin watermarkEditWin = this;
    System.Windows.Media.Color selectedColor = watermarkEditWin.WatermarkColorPicker.SelectedColor;
    watermarkEditWin.SelectedFontground = $"#{selectedColor.A:X2}{selectedColor.R:X2}{selectedColor.G:X2}{selectedColor.B:X2}";
    MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
    bool? isChecked = watermarkEditWin.textWatermarkRadioButton.IsChecked;
    if (isChecked.GetValueOrDefault())
    {
      requiredService.AnnotationToolbar.WatermarkModel = WatermarkAnnonationModel.Text;
      if (string.IsNullOrWhiteSpace(watermarkEditWin.txtWatermarkConent.Text.Trim()))
      {
        int num = (int) ModernMessageBox.Show(pdfeditor.Properties.Resources.WinWatermarkTextEmptyMsg, UtilManager.GetProductName());
        return;
      }
      watermarkEditWin.TextModel = new WatermarkTextModel();
      watermarkEditWin.TextModel.Content = watermarkEditWin.txtWatermarkConent.Text.Trim();
      watermarkEditWin.TextModel.Foreground = watermarkEditWin.SelectedFontground;
      float result;
      if (float.TryParse((watermarkEditWin.fontSizeComboBox.SelectedItem as ComboBoxItem).Content.ToString(), NumberStyles.Float | NumberStyles.AllowThousands, (IFormatProvider) CultureInfo.InvariantCulture, out result))
        watermarkEditWin.TextModel.FontSize = result;
      requiredService.AnnotationToolbar.TextWatermarkModel = watermarkEditWin.TextModel;
    }
    else
    {
      isChecked = watermarkEditWin.fileWatermarkRadioButton.IsChecked;
      if (isChecked.GetValueOrDefault())
      {
        requiredService.AnnotationToolbar.WatermarkModel = WatermarkAnnonationModel.Image;
        if (string.IsNullOrWhiteSpace(watermarkEditWin.txtWatermarkfile.Text.Trim()))
        {
          int num = (int) ModernMessageBox.Show(pdfeditor.Properties.Resources.WinWatermarkFilePathEmptyMsg, UtilManager.GetProductName());
          return;
        }
        if (watermarkEditWin.ImageModel == null)
          watermarkEditWin.ImageModel = new WatermarkImageModel();
        watermarkEditWin.ImageModel.ImageFilePath = watermarkEditWin.txtWatermarkfile.Text;
        WriteableBitmap writeableBitmap = (WriteableBitmap) null;
        try
        {
          using (FileStream fileStream = File.OpenRead(watermarkEditWin.ImageModel.ImageFilePath))
          {
            BitmapImage source = new BitmapImage();
            source.CacheOption = BitmapCacheOption.OnLoad;
            source.BeginInit();
            source.StreamSource = (Stream) fileStream;
            source.EndInit();
            writeableBitmap = new WriteableBitmap((BitmapSource) source);
          }
        }
        catch
        {
        }
        watermarkEditWin.ImageModel.WatermarkImageSource = (BitmapSource) writeableBitmap;
        requiredService.AnnotationToolbar.ImageWatermarkModel = watermarkEditWin.ImageModel;
      }
    }
    requiredService.AnnotationToolbar.WatermarkParam = watermarkEditWin.GetWatermarkParam(requiredService.Document);
    await watermarkEditWin.GenerateWaterMarkAsync();
    watermarkEditWin.DialogResult = new bool?(watermarkEditWin.TextModel != null || watermarkEditWin.ImageModel != null);
  }

  private async Task GenerateWaterMarkAsync()
  {
    MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
    this.watermarkParam = requiredService.AnnotationToolbar.WatermarkParam;
    if (this.watermarkParam.PageRange == null || this.watermarkParam.PageRange.Length == 0)
      return;
    CommomLib.Commom.GAManager.SendEvent("AnnotationAction", "PdfWatermarkAnnotation", "New", 1L);
    Action<PdfPage> action = (Action<PdfPage>) null;
    if (requiredService.AnnotationToolbar.WatermarkModel == WatermarkAnnonationModel.Text)
    {
      WatermarkTextModel textWatermarkModel = requiredService.AnnotationToolbar.TextWatermarkModel;
      action = this.CreateTextWatermarkFunc(requiredService.Document, this.watermarkParam, textWatermarkModel);
    }
    else if (requiredService.AnnotationToolbar.WatermarkModel == WatermarkAnnonationModel.Image)
      action = this.CreateImageWatermarkFunc(this.watermarkParam, requiredService.AnnotationToolbar.ImageWatermarkModel);
    PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(requiredService.Document);
    (int num1, int num2) = pdfControl != null ? pdfControl.GetVisiblePageRange() : (-1, -1);
    if (action != null)
    {
      for (int index = 0; index < this.watermarkParam.PageRange.Length; ++index)
      {
        int num3 = this.watermarkParam.PageRange[index];
        IntPtr num4 = IntPtr.Zero;
        PdfPage page = (PdfPage) null;
        try
        {
          num4 = Pdfium.FPDF_LoadPage(requiredService.Document.Handle, num3);
          if (num4 != IntPtr.Zero)
          {
            page = PdfPage.FromHandle(requiredService.Document, num4, num3);
            action(page);
          }
        }
        finally
        {
          if (page != null && (page.PageIndex > num2 || page.PageIndex < num1))
            PageDisposeHelper.DisposePage(page);
          if (num4 != IntPtr.Zero)
            Pdfium.FPDF_ClosePage(num4);
        }
      }
    }
    if (pdfControl == null)
      return;
    await pdfControl.TryRedrawVisiblePageAsync();
  }

  private Action<PdfPage> CreateTextWatermarkFunc(
    PdfDocument doc,
    WatermarkParam watermarkParam,
    WatermarkTextModel textModel)
  {
    System.Windows.Media.Color color = (System.Windows.Media.Color) System.Windows.Media.ColorConverter.ConvertFromString(textModel.Foreground);
    FS_COLOR fillColor = new FS_COLOR((int) (byte) ((double) color.A * (double) watermarkParam.Opacity), (int) color.R, (int) color.G, (int) color.B);
    bool isAuto = (double) textModel.FontSize == 0.0;
    float globalFontSize = isAuto ? 1f : textModel.FontSize;
    float globalBottomBaseline;
    FS_RECTF globalTextBounds;
    System.Collections.Generic.IReadOnlyList<TextWithFallbackFontFamily> fallbackFonts = WatermarkUtil.CreateWatermarkTextFonts(textModel.Content, fillColor, "Arial", globalFontSize, out globalTextBounds, out globalBottomBaseline);
    PdfFont[] fonts = fallbackFonts.Select<TextWithFallbackFontFamily, PdfFont>((Func<TextWithFallbackFontFamily, PdfFont>) (c => PdfFontUtils.CreateFont(doc, string.IsNullOrEmpty(c.FallbackFontFamily?.Source) ? "Arial" : c.FallbackFontFamily.Source, c.FontWeight, c.FontStyle, c.CharSet))).ToArray<PdfFont>();
    return watermarkParam.IsTile ? (Action<PdfPage>) (p =>
    {
      System.Collections.Generic.IReadOnlyList<PdfTextObject> pdfTextObjectList = CreateWatermarkTextObjects(fallbackFonts, globalBottomBaseline, globalFontSize, fillColor);
      float num1 = 1f;
      if (pdfTextObjectList.Count == 0)
        return;
      FS_SIZEF effectiveSize = p.GetEffectiveSize();
      FS_MATRIX fsMatrix1 = new FS_MATRIX();
      fsMatrix1.SetIdentity();
      fsMatrix1.Rotate((float) ((double) watermarkParam.Rotation * Math.PI / 180.0));
      if (isAuto)
        num1 = Math.Min(effectiveSize.Width / 2f / globalTextBounds.Width, effectiveSize.Height / 2f / globalTextBounds.Height);
      fsMatrix1.Scale(num1, num1);
      FS_RECTF rect = globalTextBounds;
      fsMatrix1.TransformRect(ref rect);
      int num2 = (int) Math.Ceiling(((double) effectiveSize.Width + 50.0) / ((double) rect.Width + 50.0));
      int num3 = (int) Math.Ceiling(((double) effectiveSize.Height + 50.0) / ((double) rect.Height + 50.0));
      PdfWatermarkAnnotation watermarkAnnotation = new PdfWatermarkAnnotation(p);
      watermarkAnnotation.Contents = textModel.Content;
      watermarkAnnotation.Flags |= AnnotationFlags.Print;
      watermarkAnnotation.ModificationDate = DateTimeOffset.Now.ToModificationDateString();
      watermarkAnnotation.CreateEmptyAppearance(AppearanceStreamModes.Normal);
      for (int index1 = 0; index1 < num3; ++index1)
      {
        float t = effectiveSize.Height - (rect.Height + 50f) * (float) index1;
        for (int index2 = 0; index2 < num2; ++index2)
        {
          float l = (rect.Width + 50f) * (float) index2;
          FS_RECTF fsRectf = new FS_RECTF(l, t, l + rect.Width, t - rect.Height);
          if (pdfTextObjectList == null)
          {
            pdfTextObjectList = CreateWatermarkTextObjects(fallbackFonts, globalBottomBaseline, globalFontSize, fillColor);
            if (pdfTextObjectList.Count == 0)
              continue;
          }
          for (int index3 = 0; index3 < pdfTextObjectList.Count; ++index3)
          {
            PdfTextObject pdfTextObject = pdfTextObjectList[index3];
            FS_POINTF location = pdfTextObject.Location;
            double angle = (double) watermarkParam.Rotation * Math.PI / 180.0;
            FS_MATRIX fsMatrix2 = pdfTextObject.Matrix ?? new FS_MATRIX();
            fsMatrix2.Translate((float) (-(double) globalTextBounds.Width / 2.0), (float) (-(double) globalTextBounds.Height / 2.0));
            fsMatrix2.Rotate((float) angle);
            fsMatrix2.Scale(num1, num1);
            fsMatrix2.Translate(fsRectf.left + fsRectf.Width / 2f, fsRectf.bottom + fsRectf.Height / 2f);
            pdfTextObject.Matrix = fsMatrix2;
            watermarkAnnotation.NormalAppearance.Add((PdfPageObject) pdfTextObject);
          }
          pdfTextObjectList = (System.Collections.Generic.IReadOnlyList<PdfTextObject>) null;
        }
      }
      watermarkAnnotation.GenerateAppearance(AppearanceStreamModes.Normal);
      if (p.Annots == null)
        p.CreateAnnotations();
      p.Annots.Add((PdfAnnotation) watermarkAnnotation);
      foreach (PdfTextObject pdfTextObject in watermarkAnnotation.NormalAppearance.OfType<PdfTextObject>())
        pdfTextObject.Font.Dictionary.Dispose();
      watermarkAnnotation.NormalAppearance.Dispose();
      watermarkAnnotation.Dispose();
    }) : (Action<PdfPage>) (p =>
    {
      float num = 1f;
      System.Collections.Generic.IReadOnlyList<PdfTextObject> watermarkTextObjects = CreateWatermarkTextObjects(fallbackFonts, globalBottomBaseline, globalFontSize, fillColor);
      if (watermarkTextObjects.Count == 0)
        return;
      FS_SIZEF effectiveSize = p.GetEffectiveSize();
      FS_MATRIX fsMatrix3 = new FS_MATRIX();
      fsMatrix3.SetIdentity();
      fsMatrix3.Rotate((float) ((double) watermarkParam.Rotation * Math.PI / 180.0));
      if (isAuto)
        num = Math.Min(effectiveSize.Width / 2f / globalTextBounds.Width, effectiveSize.Height / 2f / globalTextBounds.Height);
      fsMatrix3.Scale(num, num);
      FS_RECTF rect = globalTextBounds;
      fsMatrix3.TransformRect(ref rect);
      FS_RECTF pdfObjectBounds = this.GetPdfObjectBounds((double) rect.Width, (double) rect.Height, watermarkParam.Alignment, (double) effectiveSize.Width, (double) effectiveSize.Height);
      PdfWatermarkAnnotation watermarkAnnotation = new PdfWatermarkAnnotation(p);
      watermarkAnnotation.Contents = textModel.Content;
      watermarkAnnotation.ModificationDate = DateTimeOffset.Now.ToModificationDateString();
      watermarkAnnotation.Flags |= AnnotationFlags.Print;
      watermarkAnnotation.CreateEmptyAppearance(AppearanceStreamModes.Normal);
      for (int index = 0; index < watermarkTextObjects.Count; ++index)
      {
        PdfTextObject pdfTextObject = watermarkTextObjects[index];
        FS_POINTF location = pdfTextObject.Location;
        double angle = (double) watermarkParam.Rotation * Math.PI / 180.0;
        FS_MATRIX fsMatrix4 = pdfTextObject.Matrix ?? new FS_MATRIX();
        fsMatrix4.Translate((float) (-(double) globalTextBounds.Width / 2.0), (float) (-(double) globalTextBounds.Height / 2.0));
        fsMatrix4.Rotate((float) angle);
        fsMatrix4.Scale(num, num);
        fsMatrix4.Translate(pdfObjectBounds.left + pdfObjectBounds.Width / 2f, pdfObjectBounds.bottom + pdfObjectBounds.Height / 2f);
        pdfTextObject.Matrix = fsMatrix4;
        watermarkAnnotation.NormalAppearance.Add((PdfPageObject) pdfTextObject);
      }
      watermarkAnnotation.GenerateAppearance(AppearanceStreamModes.Normal);
      if (p.Annots == null)
        p.CreateAnnotations();
      p.Annots.Add((PdfAnnotation) watermarkAnnotation);
      watermarkAnnotation.NormalAppearance.Dispose();
      watermarkAnnotation.Dispose();
    });

    System.Collections.Generic.IReadOnlyList<PdfTextObject> CreateWatermarkTextObjects(
      System.Collections.Generic.IReadOnlyList<TextWithFallbackFontFamily> _fallbackFonts,
      float _bottomBaseline,
      float _fontSize,
      FS_COLOR _fillColor)
    {
      return (System.Collections.Generic.IReadOnlyList<PdfTextObject>) fallbackFonts.Select<TextWithFallbackFontFamily, PdfTextObject>((Func<TextWithFallbackFontFamily, int, PdfTextObject>) ((_c, _i) =>
      {
        PdfTextObject watermarkTextObjects = PdfTextObject.Create(_c.Text, (float) _c.Bounds.Left, _bottomBaseline, fonts[_i], _fontSize);
        watermarkTextObjects.FillColor = _fillColor;
        return watermarkTextObjects;
      })).ToList<PdfTextObject>();
    }
  }

  private Action<PdfPage> CreateImageWatermarkFunc(
    WatermarkParam watermarkParam,
    WatermarkImageModel imageModel)
  {
    WriteableBitmap bitmap = (WriteableBitmap) null;
    if (imageModel?.WatermarkImageSource != null)
    {
      if (imageModel.WatermarkImageSource.Format == PixelFormats.Bgra32)
        bitmap = new WriteableBitmap(imageModel.WatermarkImageSource);
      else
        bitmap = new WriteableBitmap((BitmapSource) new FormatConvertedBitmap(imageModel.WatermarkImageSource, PixelFormats.Bgra32, (BitmapPalette) null, 0.0));
    }
    Dictionary<(int, int, float), WriteableBitmap> bmpCache = new Dictionary<(int, int, float), WriteableBitmap>();
    return watermarkParam.IsTile ? (Action<PdfPage>) (p =>
    {
      (float width2, float height2) = p.GetEffectiveSize();
      System.Windows.Size _pageSize = new System.Windows.Size((double) width2, (double) height2);
      float pageDpi = GetPageDpi(p);
      WriteableBitmap scaledBitmap = GetScaledBitmap(_pageSize, pageDpi, bitmap, watermarkParam.Opacity);
      FS_MATRIX fsMatrix1 = new FS_MATRIX();
      fsMatrix1.SetIdentity();
      fsMatrix1.Rotate((float) ((double) watermarkParam.Rotation * Math.PI / 180.0));
      FS_RECTF rect = new FS_RECTF(0.0f, (float) scaledBitmap.PixelHeight, (float) scaledBitmap.PixelWidth, 0.0f);
      fsMatrix1.TransformRect(ref rect);
      int num1 = (int) Math.Ceiling((_pageSize.Width + 50.0) / ((double) rect.Width + 50.0));
      int num2 = (int) Math.Ceiling((_pageSize.Height + 50.0) / ((double) rect.Height + 50.0));
      PdfWatermarkAnnotation watermarkAnnotation = new PdfWatermarkAnnotation(p);
      watermarkAnnotation.ModificationDate = DateTimeOffset.Now.ToModificationDateString();
      watermarkAnnotation.Flags |= AnnotationFlags.Print;
      watermarkAnnotation.CreateEmptyAppearance(AppearanceStreamModes.Normal);
      for (int index1 = 0; index1 < num2; ++index1)
      {
        double t = _pageSize.Height - ((double) rect.Height + 50.0) * (double) index1;
        for (int index2 = 0; index2 < num1; ++index2)
        {
          float l = (rect.Width + 50f) * (float) index2;
          FS_RECTF fsRectf = new FS_RECTF((double) l, t, (double) l + (double) rect.Width, t - (double) rect.Height);
          using (PdfBitmap bitmap1 = new PdfBitmap(scaledBitmap.PixelWidth, scaledBitmap.PixelHeight, true))
          {
            int bufferSize = bitmap1.Stride * bitmap1.Height;
            scaledBitmap.CopyPixels(new Int32Rect(0, 0, scaledBitmap.PixelWidth, scaledBitmap.PixelHeight), bitmap1.Buffer, bufferSize, bitmap1.Stride);
            PdfImageObject pdfImageObject = PdfImageObject.Create(p.Document, bitmap1, 0.0f, 0.0f);
            FS_MATRIX fsMatrix2 = new FS_MATRIX();
            fsMatrix2.SetIdentity();
            fsMatrix2.Scale((float) bitmap1.Width, (float) bitmap1.Height);
            fsMatrix2.Translate(-bitmap1.Width / 2, -bitmap1.Height / 2);
            fsMatrix2.Rotate((float) ((double) watermarkParam.Rotation * Math.PI / 180.0));
            fsMatrix2.Translate(fsRectf.left + fsRectf.Width / 2f, fsRectf.bottom + fsRectf.Height / 2f);
            pdfImageObject.Matrix = fsMatrix2;
            watermarkAnnotation.NormalAppearance.Add((PdfPageObject) pdfImageObject);
          }
        }
      }
      watermarkAnnotation.GenerateAppearance(AppearanceStreamModes.Normal);
      if (p.Annots == null)
        p.CreateAnnotations();
      p.Annots.Add((PdfAnnotation) watermarkAnnotation);
      watermarkAnnotation.NormalAppearance.Dispose();
      watermarkAnnotation.Dispose();
    }) : (Action<PdfPage>) (p =>
    {
      (float width4, float height4) = p.GetEffectiveSize();
      System.Windows.Size _pageSize = new System.Windows.Size((double) width4, (double) height4);
      float pageDpi = GetPageDpi(p);
      WriteableBitmap scaledBitmap = GetScaledBitmap(_pageSize, pageDpi, bitmap, watermarkParam.Opacity);
      FS_MATRIX fsMatrix3 = new FS_MATRIX();
      fsMatrix3.SetIdentity();
      fsMatrix3.Rotate((float) ((double) watermarkParam.Rotation * Math.PI / 180.0));
      FS_RECTF rect = new FS_RECTF(0.0f, (float) scaledBitmap.PixelHeight, (float) scaledBitmap.PixelWidth, 0.0f);
      fsMatrix3.TransformRect(ref rect);
      FS_RECTF pdfObjectBounds = this.GetPdfObjectBounds((double) rect.Width, (double) rect.Height, watermarkParam.Alignment, _pageSize.Width, _pageSize.Height);
      using (PdfBitmap bitmap2 = new PdfBitmap(scaledBitmap.PixelWidth, scaledBitmap.PixelHeight, true))
      {
        int bufferSize = bitmap2.Stride * bitmap2.Height;
        scaledBitmap.CopyPixels(new Int32Rect(0, 0, scaledBitmap.PixelWidth, scaledBitmap.PixelHeight), bitmap2.Buffer, bufferSize, bitmap2.Stride);
        PdfWatermarkAnnotation watermarkAnnotation = new PdfWatermarkAnnotation(p);
        watermarkAnnotation.ModificationDate = DateTimeOffset.Now.ToModificationDateString();
        watermarkAnnotation.Flags |= AnnotationFlags.Print;
        watermarkAnnotation.CreateEmptyAppearance(AppearanceStreamModes.Normal);
        PdfImageObject pdfImageObject = PdfImageObject.Create(p.Document, bitmap2, 0.0f, 0.0f);
        FS_MATRIX fsMatrix4 = new FS_MATRIX();
        fsMatrix4.SetIdentity();
        fsMatrix4.Scale((float) bitmap2.Width, (float) bitmap2.Height);
        fsMatrix4.Translate(-bitmap2.Width / 2, -bitmap2.Height / 2);
        fsMatrix4.Rotate((float) ((double) watermarkParam.Rotation * Math.PI / 180.0));
        fsMatrix4.Translate(pdfObjectBounds.left + pdfObjectBounds.Width / 2f, pdfObjectBounds.bottom + pdfObjectBounds.Height / 2f);
        pdfImageObject.Matrix = fsMatrix4;
        watermarkAnnotation.NormalAppearance.Add((PdfPageObject) pdfImageObject);
        watermarkAnnotation.GenerateAppearance(AppearanceStreamModes.Normal);
        if (p.Annots == null)
          p.CreateAnnotations();
        p.Annots.Add((PdfAnnotation) watermarkAnnotation);
        watermarkAnnotation.NormalAppearance.Dispose();
        watermarkAnnotation.Dispose();
      }
    });

    static WriteableBitmap ResizeBitmap(
      WriteableBitmap _source,
      int _newWidth,
      int _newHeight,
      float _opacity)
    {
      if (_source.Width == (double) _newWidth && _source.Height == (double) _newHeight && (double) _opacity == 1.0)
        return _source;
      using (Bitmap bitmap1 = new Bitmap(_source.PixelWidth, _source.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
      {
        BitmapData bitmapdata = bitmap1.LockBits(new Rectangle(0, 0, _source.PixelWidth, _source.PixelHeight), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        _source.CopyPixels(new Int32Rect(0, 0, _source.PixelWidth, _source.PixelHeight), bitmapdata.Scan0, bitmapdata.Stride * bitmapdata.Height, bitmapdata.Stride);
        bitmap1.UnlockBits(bitmapdata);
        BitmapSizeOptions sizeOptions = BitmapSizeOptions.FromWidthAndHeight(_newWidth, _newHeight);
        Int32Rect sourceRect = new Int32Rect(0, 0, bitmap1.Width, bitmap1.Height);
        BitmapSource source = (BitmapSource) null;
        if ((double) _opacity != 1.0)
        {
          using (Bitmap bitmap2 = new Bitmap(bitmap1.Width, bitmap1.Height, bitmap1.PixelFormat))
          {
            float num = Math.Max(0.0f, Math.Min(1f, _opacity));
            ColorMatrix newColorMatrix = new ColorMatrix();
            newColorMatrix.Matrix33 = num;
            using (Graphics graphics = Graphics.FromImage((System.Drawing.Image) bitmap2))
            {
              using (ImageAttributes imageAttr = new ImageAttributes())
              {
                imageAttr.SetColorMatrix(newColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                graphics.DrawImage((System.Drawing.Image) bitmap1, new Rectangle(0, 0, bitmap1.Width, bitmap1.Height), 0, 0, bitmap1.Width, bitmap1.Height, GraphicsUnit.Pixel, imageAttr);
              }
            }
            IntPtr hbitmap = bitmap2.GetHbitmap();
            try
            {
              source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero, sourceRect, sizeOptions);
            }
            finally
            {
              try
              {
                if (hbitmap != IntPtr.Zero)
                  DrawUtils.DeleteObject(hbitmap);
              }
              catch
              {
              }
            }
          }
        }
        else
        {
          IntPtr hbitmap = bitmap1.GetHbitmap();
          try
          {
            source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero, sourceRect, sizeOptions);
          }
          finally
          {
            try
            {
              if (hbitmap != IntPtr.Zero)
                DrawUtils.DeleteObject(hbitmap);
            }
            catch
            {
            }
          }
        }
        return new WriteableBitmap(source);
      }
    }

    static float GetPageDpi(PdfPage page)
    {
      float pageDpi = 72f;
      if (page.Dictionary.ContainsKey("UserUnit"))
        pageDpi = page.Dictionary["UserUnit"].As<PdfTypeNumber>().FloatValue * 72f;
      return pageDpi;
    }

    static System.Windows.Size GetBitmapPageSize(
      double bitmapWidth,
      double bitmapHeight,
      FS_RECTF pageBounds)
    {
      System.Windows.Size bitmapPageSize = new System.Windows.Size(bitmapWidth * 96.0 / 72.0, bitmapHeight * 96.0 / 72.0);
      if (bitmapPageSize.Width > (double) pageBounds.Width / 2.0 || bitmapPageSize.Height > (double) pageBounds.Height / 2.0)
      {
        double num = Math.Max(bitmapPageSize.Width / ((double) pageBounds.Width / 2.0), bitmapPageSize.Height / ((double) pageBounds.Height / 2.0));
        bitmapPageSize = new System.Windows.Size(bitmapPageSize.Width / num, bitmapPageSize.Height / num);
      }
      else if (bitmapPageSize.Width < 10.0 && bitmapPageSize.Height < 10.0)
      {
        double num = Math.Min(bitmapPageSize.Width / 10.0, bitmapPageSize.Height / 10.0);
        bitmapPageSize = new System.Windows.Size(bitmapPageSize.Width / num, bitmapPageSize.Height / num);
      }
      return bitmapPageSize;
    }

    WriteableBitmap GetScaledBitmap(
      System.Windows.Size _pageSize,
      float pageDpi,
      WriteableBitmap _bitmap,
      float opacity)
    {
      System.Windows.Size size = new System.Windows.Size(_bitmap.Width * (double) pageDpi / 96.0, _bitmap.Height * (double) pageDpi / 96.0);
      System.Windows.Size bitmapPageSize = GetBitmapPageSize(size.Width, size.Height, new FS_RECTF(0.0, _pageSize.Height, _pageSize.Width, 0.0));
      (int, int, float) key = ((int) Math.Ceiling(bitmapPageSize.Width), (int) Math.Ceiling(bitmapPageSize.Height), opacity);
      WriteableBitmap scaledBitmap;
      if (!bmpCache.TryGetValue(key, out scaledBitmap))
      {
        scaledBitmap = ResizeBitmap(_bitmap, key.Item1, key.Item2, opacity);
        bmpCache[key] = scaledBitmap;
      }
      return scaledBitmap;
    }
  }

  private void SeTextObjectLocation(
    PdfTextObject text,
    PdfContentAlignment alignment,
    double pageWidth,
    double pageHeight)
  {
    float width = text.BoundingBox.Width;
    float height = text.BoundingBox.Height;
    switch (alignment)
    {
      case PdfContentAlignment.TopLeft:
        text.Location = new FS_POINTF(5.0, pageHeight - (double) height);
        break;
      case PdfContentAlignment.TopCenter:
        text.Location = new FS_POINTF(pageWidth / 2.0 - (double) width / 2.0, pageHeight - (double) height);
        break;
      case PdfContentAlignment.TopRight:
        text.Location = new FS_POINTF(pageWidth - (double) width - 5.0, pageHeight - (double) height);
        break;
      case PdfContentAlignment.MiddleLeft:
        text.Location = new FS_POINTF(5.0, pageHeight / 2.0 - (double) height / 2.0);
        break;
      case PdfContentAlignment.MiddleCenter:
        text.Location = new FS_POINTF(pageWidth / 2.0 - (double) width / 2.0, pageHeight / 2.0 - (double) height / 2.0);
        break;
      case PdfContentAlignment.MiddleRight:
        text.Location = new FS_POINTF(pageWidth - (double) width, pageHeight / 2.0 - (double) height / 2.0);
        break;
      case PdfContentAlignment.BottomLeft:
        text.Location = new FS_POINTF(5f, 0.0f);
        break;
      case PdfContentAlignment.BottomCenter:
        text.Location = new FS_POINTF(pageWidth / 2.0 - (double) width / 2.0, 0.0);
        break;
      case PdfContentAlignment.BottomRight:
        text.Location = new FS_POINTF(pageWidth - (double) width, 0.0);
        break;
    }
  }

  private FS_RECTF GetPdfObjectBounds(
    double textWidth,
    double textHeight,
    PdfContentAlignment alignment,
    double pageWidth,
    double pageHeight)
  {
    FS_POINTF fsPointf = new FS_POINTF();
    switch (alignment)
    {
      case PdfContentAlignment.TopLeft:
        fsPointf = new FS_POINTF(5.0, pageHeight - textHeight);
        break;
      case PdfContentAlignment.TopCenter:
        fsPointf = new FS_POINTF(pageWidth / 2.0 - textWidth / 2.0 + 8.0, pageHeight - textHeight);
        break;
      case PdfContentAlignment.TopRight:
        fsPointf = new FS_POINTF(pageWidth - textWidth - 5.0, pageHeight - textHeight);
        break;
      case PdfContentAlignment.MiddleLeft:
        fsPointf = new FS_POINTF(5.0, pageHeight / 2.0 - textHeight / 2.0);
        break;
      case PdfContentAlignment.MiddleCenter:
        fsPointf = new FS_POINTF(pageWidth / 2.0 - textWidth / 2.0 + 8.0, pageHeight / 2.0 - textHeight / 2.0);
        break;
      case PdfContentAlignment.MiddleRight:
        fsPointf = new FS_POINTF(pageWidth - textWidth, pageHeight / 2.0 - textHeight / 2.0);
        break;
      case PdfContentAlignment.BottomLeft:
        fsPointf = new FS_POINTF(5f, 0.0f);
        break;
      case PdfContentAlignment.BottomCenter:
        fsPointf = new FS_POINTF(pageWidth / 2.0 - textWidth / 2.0 + 8.0, 0.0);
        break;
      case PdfContentAlignment.BottomRight:
        fsPointf = new FS_POINTF(pageWidth - textWidth, 0.0);
        break;
    }
    return new FS_RECTF((double) fsPointf.X, (double) fsPointf.Y + textHeight, (double) fsPointf.X + textWidth, (double) fsPointf.Y);
  }

  private FS_POINTF SeImgObjectLocation(
    PdfImageObject img,
    PdfContentAlignment alignment,
    double pageWidth,
    double pageHeight,
    float scale)
  {
    float num1 = img.BoundingBox.Width * scale;
    float num2 = img.BoundingBox.Height * scale;
    FS_POINTF fsPointf = new FS_POINTF();
    switch (alignment)
    {
      case PdfContentAlignment.TopLeft:
        fsPointf = new FS_POINTF(5.0, pageHeight - (double) num2);
        break;
      case PdfContentAlignment.TopCenter:
        fsPointf = new FS_POINTF(pageWidth / 2.0 - (double) num1 / 2.0, pageHeight - (double) num2);
        break;
      case PdfContentAlignment.TopRight:
        fsPointf = new FS_POINTF(pageWidth - (double) num1 - 5.0, pageHeight - (double) num2);
        break;
      case PdfContentAlignment.MiddleLeft:
        fsPointf = new FS_POINTF(5.0, pageHeight / 2.0 - (double) num2 / 2.0);
        break;
      case PdfContentAlignment.MiddleCenter:
        fsPointf = new FS_POINTF(pageWidth / 2.0 - (double) num1 / 2.0, pageHeight / 2.0 - (double) num2 / 2.0);
        break;
      case PdfContentAlignment.MiddleRight:
        fsPointf = new FS_POINTF(pageWidth - (double) num1, pageHeight / 2.0 - (double) num2 / 2.0);
        break;
      case PdfContentAlignment.BottomLeft:
        fsPointf = new FS_POINTF(5f, num1 / 2f);
        break;
      case PdfContentAlignment.BottomCenter:
        fsPointf = new FS_POINTF(pageWidth / 2.0 - (double) num1 / 2.0, 0.0);
        break;
      case PdfContentAlignment.BottomRight:
        fsPointf = new FS_POINTF(pageWidth - (double) num1, 0.0);
        break;
    }
    return fsPointf;
  }

  private void SetImageRotate(
    PdfImageObject img,
    float rotation,
    float scale,
    bool isTile,
    float x,
    float y)
  {
    FS_MATRIX fsMatrix = img.Matrix ?? new FS_MATRIX();
    float sx = (float) img.Bitmap.Width * scale;
    float sy = (float) img.Bitmap.Height * scale;
    float angle = (float) ((double) rotation * 3.1400001049041748 / 180.0);
    fsMatrix.SetIdentity();
    fsMatrix.Scale(sx, sy);
    fsMatrix.Translate((float) (-(double) sx / 2.0), (float) (-(double) sy / 2.0));
    fsMatrix.Rotate(angle);
    fsMatrix.Translate(sx / 2f, sy / 2f);
    fsMatrix.Translate(x, y);
    img.Matrix = fsMatrix;
  }

  private void SetImageObjectSoftMask(PdfImageObject img, float Opacity)
  {
    PdfTypeStream pdfTypeStream = (PdfTypeStream) null;
    if (img.SoftMask != null && img.SoftMask.Is<PdfTypeStream>())
    {
      pdfTypeStream = img.SoftMask.As<PdfTypeStream>();
    }
    else
    {
      PdfTypeBase pdfTypeBase;
      if (img.Stream.Dictionary != null && img.Stream.Dictionary.TryGetValue("SMask", out pdfTypeBase) && pdfTypeBase != null && pdfTypeBase.Is<PdfTypeStream>())
        pdfTypeStream = pdfTypeBase.As<PdfTypeStream>();
    }
    byte[] data = new byte[pdfTypeStream.Content.Length];
    for (int index = 0; index < pdfTypeStream.Content.Length; ++index)
      data[index] = (byte) ((double) pdfTypeStream.Content[index] * (double) Opacity);
    pdfTypeStream.SetContent(data, false);
    img.SoftMask = (PdfTypeBase) pdfTypeStream;
  }

  private WatermarkParam GetWatermarkParam(PdfDocument doc)
  {
    WatermarkParam watermarkParam = new WatermarkParam();
    UIElementAligent alignmentSelector = this.alignmentSelector;
    watermarkParam.Alignment = alignmentSelector.Alignment;
    watermarkParam.Opacity = 0.5f;
    float result1;
    if (float.TryParse((this.opacityCB.SelectedItem as ComboBoxItem).Tag.ToString(), NumberStyles.Float | NumberStyles.AllowThousands, (IFormatProvider) CultureInfo.InvariantCulture, out result1))
      watermarkParam.Opacity = result1;
    ComboBoxItem selectedItem = this.rotateComboBox.SelectedItem as ComboBoxItem;
    watermarkParam.Rotation = 0.0f;
    float result2;
    if (float.TryParse(selectedItem.Tag.ToString(), NumberStyles.Float | NumberStyles.AllowThousands, (IFormatProvider) CultureInfo.InvariantCulture, out result2))
      watermarkParam.Rotation = result2;
    watermarkParam.Scale = 1f;
    watermarkParam.Vdistance = 0.0f;
    watermarkParam.Hdistance = 0.0f;
    watermarkParam.IsTile = this.checkTile.IsChecked.Value;
    watermarkParam.PageRange = this.GetImportPageRange(doc);
    return watermarkParam;
  }

  private void btnCancel_Click(object sender, RoutedEventArgs e)
  {
    this.DialogResult = new bool?(false);
    this.Close();
  }

  private void btnOpenFile_Click(object sender, RoutedEventArgs e)
  {
    MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
    OpenFileDialog openFileDialog1 = new OpenFileDialog();
    openFileDialog1.Filter = "All Image Files|*.bmp;*.ico;*.gif;*.jpeg;*.jpg;*.png;*.tif;*.tiff|Windows Bitmap(*.bmp)|*.bmp|Windows Icon(*.ico)|*.ico|Graphics Interchange Format (*.gif)|(*.gif)|JPEG File Interchange Format (*.jpg)|*.jpg;*.jpeg|Portable Network Graphics (*.png)|*.png|Tag Image File Format (*.tif)|*.tif;*.tiff";
    openFileDialog1.ShowReadOnly = false;
    openFileDialog1.ReadOnlyChecked = true;
    OpenFileDialog openFileDialog2 = openFileDialog1;
    bool? nullable = openFileDialog2.ShowDialog();
    this.ImageModel = new WatermarkImageModel();
    if (nullable.GetValueOrDefault() && !string.IsNullOrEmpty(openFileDialog2.FileName))
    {
      this.ImageModel.ImageFilePath = openFileDialog2.FileName;
      this.txtWatermarkfile.Text = openFileDialog2.FileName;
      requiredService.AnnotationToolbar.StampImgFileOkTime = DateTime.Now;
    }
    else
    {
      this.txtWatermarkfile.Text = string.Empty;
      this.ImageModel = (WatermarkImageModel) null;
    }
  }

  private int[] GetImportPageRange(PdfDocument doc)
  {
    if (this.CurrentPageRadioButton.IsChecked.GetValueOrDefault())
      return new int[1]
      {
        Ioc.Default.GetRequiredService<MainViewModel>().Document.Pages.CurrentIndex
      };
    int[] array;
    if (this.AllPagesRadioButton.IsChecked.GetValueOrDefault())
    {
      if (doc.Pages.Count == 0)
        return (int[]) null;
      array = Enumerable.Range(0, doc.Pages.Count).ToArray<int>();
    }
    else
      array = this.RangeBox.PageIndexes.ToArray<int>();
    if (((IEnumerable<int>) array).Any<int>((Func<int, bool>) (c => c < 0 || c >= doc.Pages.Count)))
      return (int[]) null;
    if (this.applyToComboBox.SelectedIndex == 1)
      array = ((IEnumerable<int>) array).Where<int>((Func<int, bool>) (c => c % 2 == 0)).ToArray<int>();
    else if (this.applyToComboBox.SelectedIndex == 2)
      array = ((IEnumerable<int>) array).Where<int>((Func<int, bool>) (c => c % 2 == 1)).ToArray<int>();
    return array.Length == 0 ? (int[]) null : array;
  }

  private void btn_PopColor_Click(object sender, RoutedEventArgs e) => this.PopColor.IsOpen = true;

  private void CurrentPageRadioButton_Checked(object sender, RoutedEventArgs e)
  {
    this.UpdateApplyToComboBoxEnabledState();
  }

  private void CurrentPageRadioButton_Unchecked(object sender, RoutedEventArgs e)
  {
    this.UpdateApplyToComboBoxEnabledState();
  }

  private void UpdateApplyToComboBoxEnabledState()
  {
    if (this.applyToComboBox == null || this.CurrentPageRadioButton == null)
      return;
    this.applyToComboBox.IsEnabled = !this.CurrentPageRadioButton.IsChecked.GetValueOrDefault();
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/watermark/watermarkeditwin.xaml", UriKind.Relative));
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
        this.txtWatermarkConent = (TextBox) target;
        break;
      case 2:
        this.fontSizeComboBox = (ComboBox) target;
        break;
      case 3:
        this.WatermarkColorPicker = (ColorPickerButton) target;
        break;
      case 4:
        this.PopColor = (Popup) target;
        break;
      case 5:
        this.PART_PanelColor = (WrapPanel) target;
        break;
      case 6:
        this.rotateComboBox = (ComboBox) target;
        break;
      case 7:
        this.opacityCB = (ComboBox) target;
        break;
      case 8:
        this.checkTile = (CheckBox) target;
        break;
      case 9:
        this.alignmentSelector = (UIElementAligent) target;
        break;
      case 10:
        this.CurrentPageRadioButton = (RadioButton) target;
        this.CurrentPageRadioButton.Checked += new RoutedEventHandler(this.CurrentPageRadioButton_Checked);
        this.CurrentPageRadioButton.Unchecked += new RoutedEventHandler(this.CurrentPageRadioButton_Unchecked);
        break;
      case 11:
        this.AllPagesRadioButton = (RadioButton) target;
        break;
      case 12:
        this.SelectedPagesRadioButton = (RadioButton) target;
        break;
      case 13:
        this.RangeBox = (PageRangeTextBox) target;
        break;
      case 14:
        this.applyToComboBox = (ComboBox) target;
        break;
      case 15:
        this.btnCancel = (Button) target;
        this.btnCancel.Click += new RoutedEventHandler(this.btnCancel_Click);
        break;
      case 16 /*0x10*/:
        this.btnOk = (Button) target;
        this.btnOk.Click += new RoutedEventHandler(this.btnOk_Click);
        break;
      case 17:
        this.textWatermarkRadioButton = (RadioButton) target;
        break;
      case 18:
        this.fileWatermarkRadioButton = (RadioButton) target;
        break;
      case 19:
        this.txtWatermarkfile = (TextBlock) target;
        break;
      case 20:
        this.btnOpenFile = (Button) target;
        this.btnOpenFile.Click += new RoutedEventHandler(this.btnOpenFile_Click);
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
