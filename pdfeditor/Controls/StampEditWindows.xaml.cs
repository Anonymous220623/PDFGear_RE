// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.StampEditWindows
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Win32;
using pdfeditor.Controls.ColorPickers;
using pdfeditor.Controls.Signature;
using pdfeditor.Controls.Stamp;
using pdfeditor.Utils;
using pdfeditor.ViewModels;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace pdfeditor.Controls;

public partial class StampEditWindows : Window, IComponentConnector
{
  private ObservableCollection<MenuModel> MainMenus;
  private MenuModel SelectedMenuModel;
  internal string FileDiaoligFiePath = string.Empty;
  private SignatureCreateDialogResult resultModel = new SignatureCreateDialogResult();
  private pdfeditor.ViewModels.StampTextModel stampText;
  private pdfeditor.ViewModels.StampTextModel stampPreviewText;
  public bool isText = true;
  public bool isSave;
  public static readonly DependencyProperty ClearVisibleProperty = DependencyProperty.Register("StampClearVisible", typeof (bool), typeof (StampEditWindows), new PropertyMetadata((object) false));
  private double scal = 1.0;
  internal Grid LayoutRoot;
  internal ListBox Menus;
  internal StackPanel TextPanel;
  internal StampDefaultTextPreview PreviewImageContainer;
  internal Border colorSelecters;
  internal ListBox ForegroundPickerList;
  internal ColorPickerButton ForegroundPicker;
  internal TextBox TypeWriterCtrl;
  internal CheckBox DateCheck;
  internal ComboBox DateFormatComboBox;
  internal StackPanel ImagePanel;
  internal Border body;
  internal Button PictureCtrl;
  internal Image showPicture;
  internal Button btnClear;
  internal CheckBox ckbRemoveBg;
  internal Image imgHelp;
  internal CheckBox SaveCheck;
  internal Button btnCancel;
  internal Button btnOk;
  private bool _contentLoaded;

  public SignatureCreateDialogResult ResultModel => this.resultModel;

  public IStampTextModel StampTextModel { get; private set; }

  public bool ClearVisible
  {
    get => (bool) this.GetValue(StampEditWindows.ClearVisibleProperty);
    set => this.SetValue(StampEditWindows.ClearVisibleProperty, (object) value);
  }

  public StampEditWindows()
  {
    this.InitializeComponent();
    this.MainMenus = new ObservableCollection<MenuModel>();
    this.stampText = new pdfeditor.ViewModels.StampTextModel()
    {
      FontColor = "#FF20C48F"
    };
    this.stampPreviewText = new pdfeditor.ViewModels.StampTextModel()
    {
      FontColor = "#FF20C48F"
    };
    this.PreviewImageContainer.StampModel = (object) this.stampPreviewText;
    this.InitMenu();
    this.Menus.ItemsSource = (IEnumerable) this.MainMenus;
    this.Menus.SelectedIndex = 0;
    this.UpdatePreviewImage();
  }

  private void InitMenu()
  {
    this.MainMenus.Clear();
    this.MainMenus.Add(new MenuModel()
    {
      Title = pdfeditor.Properties.Resources.WinSignatureMenuInputContent,
      Tag = "Type"
    });
    this.MainMenus.Add(new MenuModel()
    {
      Title = pdfeditor.Properties.Resources.WinSignatureMenuPictureContent,
      Tag = "Picture"
    });
    this.btnOk.Click += (RoutedEventHandler) ((o, e) =>
    {
      if (!this.CheckOk())
        return;
      Ioc.Default.GetRequiredService<MainViewModel>();
      bool? isChecked;
      if (this.Menus.SelectedIndex == 1)
      {
        SignatureCreateDialogResult resultModel = this.ResultModel;
        isChecked = this.ckbRemoveBg.IsChecked;
        int num = isChecked.Value ? 1 : 0;
        resultModel.RemoveBackground = num != 0;
        isChecked = this.SaveCheck.IsChecked;
        if (isChecked.GetValueOrDefault())
        {
          this.SavePictureImg();
          if (this.ResultModel.RemoveBackground)
            this.SaveConfigRemoveBg(this.ResultModel.ImageFilePath);
        }
      }
      else if (this.Menus.SelectedIndex == 0)
      {
        this.stampText.TextContent = this.TypeWriterCtrl.Text.Trim();
        this.stampText.FontColor = ((System.Windows.Media.Color) this.ForegroundPickerList.SelectedItem).ToString();
        this.UpdatePreviewImage();
        if (string.IsNullOrEmpty(this.stampText.TextContent))
        {
          int num = (int) MessageBox.Show("Text cannot be empty.", UtilManager.GetProductName());
          return;
        }
        if (this.stampText.TextContent.Trim().Length > 50)
        {
          int num = (int) MessageBox.Show(pdfeditor.Properties.Resources.WinCustomizeStampMaxCharactersMsg, UtilManager.GetProductName());
          return;
        }
        this.StampTextModel = (IStampTextModel) this.stampText;
      }
      isChecked = this.SaveCheck.IsChecked;
      this.isSave = isChecked.Value;
      GAManager.SendEvent("PdfStampAnnotation", "CustomStampSet", (this.Menus.SelectedIndex == 0 ? "Type-" : "Picture-") + (this.isSave ? "Save" : "NoSave"), 1L);
      this.DialogResult = new bool?(true);
    });
    this.btnCancel.Click += (RoutedEventHandler) ((o, e) =>
    {
      Ioc.Default.GetRequiredService<MainViewModel>();
      this.showPicture.Source = (ImageSource) null;
      this.DialogResult = new bool?(false);
      this.Close();
    });
    this.DateFormatComboBox.ItemsSource = (IEnumerable) StampEditWindows.GetDateFormats();
  }

  private bool CheckOk()
  {
    return (!(this.SelectedMenuModel.Title == pdfeditor.Properties.Resources.WinSignatureMenuPictureContent) || this.showPicture.Source != null) && !(this.SelectedMenuModel.Title == pdfeditor.Properties.Resources.WinSignatureMenuWriteContent);
  }

  public void SavePictureImg()
  {
    if (string.IsNullOrEmpty(this.FileDiaoligFiePath))
      return;
    Ioc.Default.GetRequiredService<MainViewModel>();
    string str1 = DateTime.Now.ToString("yyyyMMddHHmmss");
    string str2 = Path.Combine(AppDataHelper.LocalFolder, "Stamp");
    if (!Directory.Exists(str2))
      Directory.CreateDirectory(str2);
    string destFileName = Path.Combine(str2, $"StampWrite{str1}.png");
    File.Copy(this.FileDiaoligFiePath, destFileName, true);
    this.ResultModel.ImageFilePath = destFileName;
    this.FileDiaoligFiePath = string.Empty;
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
        this.ImagePanel.Visibility = Visibility.Visible;
        this.TextPanel.Visibility = Visibility.Collapsed;
        this.isText = false;
        break;
      case "Type":
        this.ImagePanel.Visibility = Visibility.Collapsed;
        this.TextPanel.Visibility = Visibility.Visible;
        this.TypeWriterCtrl.Focus();
        this.isText = true;
        break;
    }
  }

  private void txt_Text_TextChanged(object sender, TextChangedEventArgs e)
  {
    this.UpdatePreviewImage();
    this.ShowClear();
  }

  private void ShowClear()
  {
    string title = this.SelectedMenuModel.Title;
    switch (this.SelectedMenuModel.Tag)
    {
      case "Picture":
        this.ClearVisible = this.showPicture.Source != null;
        break;
      case "Type":
        this.ClearVisible = !string.IsNullOrEmpty(this.TypeWriterCtrl.Text);
        break;
    }
  }

  private void ForegroundPickerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    if (!this.IsLoaded)
      return;
    if (e.AddedItems.Count == 0)
      this.ForegroundPickerList.SelectedIndex = 0;
    else
      this.UpdatePreviewImage();
  }

  private void ForegroundPicker_SelectedColorChanged(
    object sender,
    ColorPickerButtonSelectedColorChangedEventArgs e)
  {
    CompositeCollection resource = (CompositeCollection) this.colorSelecters.Resources[(object) "ColorCollection"];
    resource[resource.Count - 1] = (object) e.Color;
    this.ForegroundPickerList.SelectedIndex = resource.Count - 1;
  }

  private void DateCheck_Click(object sender, RoutedEventArgs e) => this.UpdatePreviewImage();

  private void DateFormatComboBox_TextChanged(object sender, TextChangedEventArgs e)
  {
    this.UpdatePreviewImage();
  }

  private void UpdatePreviewImage()
  {
    this.stampText.TextContent = this.TypeWriterCtrl.Text.Trim();
    this.stampText.FontColor = ((System.Windows.Media.Color) this.ForegroundPickerList.SelectedItem).ToString();
    this.stampPreviewText.TextContent = this.TypeWriterCtrl.Text.Trim();
    this.stampPreviewText.FontColor = ((System.Windows.Media.Color) this.ForegroundPickerList.SelectedItem).ToString();
    if (string.IsNullOrEmpty(this.stampPreviewText.TextContent))
      this.stampPreviewText.TextContent = pdfeditor.Properties.Resources.WinStampSampleText;
    this.PreviewImageContainer.ForceRender();
  }

  private void SaveConfigRemoveBg(string fileName) => ConfigManager.AddSignatureRemoveBg(fileName);

  private static string[] GetDateFormats()
  {
    Dictionary<string, string[]> dictionary1 = new Dictionary<string, string[]>();
    Dictionary<string, string[]> dictionary2 = new Dictionary<string, string[]>();
    List<string> source = new List<string>();
    dictionary1["US"] = new string[2]
    {
      "MM/dd/yyyy hh:mm tt",
      "MM/dd/yyyy hh:mm:ss tt"
    };
    dictionary1["GB"] = new string[2]
    {
      "dd/MM/yyyy HH:mm",
      "dd/MM/yyyy HH:mm:ss"
    };
    dictionary1["ES"] = dictionary1["GB"];
    dictionary1["IT"] = dictionary1["GB"];
    dictionary1["PT"] = dictionary1["GB"];
    dictionary1["FR"] = dictionary1["GB"];
    dictionary1["RU"] = new string[2]
    {
      "dd.MM.yyyy HH:mm",
      "dd.MM.yyyy HH:mm:ss"
    };
    dictionary1["DE"] = dictionary1["RU"];
    dictionary1["NL"] = new string[2]
    {
      "dd-MM-yyyy HH:mm",
      "dd-MM-yyyy HH:mm:ss"
    };
    dictionary2["CN"] = new string[2]
    {
      "yyyy/MM/dd HH:mm",
      "yyyy/MM/dd HH:mm:ss"
    };
    dictionary2["ZH-CN"] = dictionary2["CN"];
    dictionary2["JA"] = dictionary2["CN"];
    dictionary2["KO"] = new string[2]
    {
      "yyyy-MM-dd tt hh:mm",
      "yyyy-MM-dd tt hh:mm:ss"
    };
    dictionary2["SG"] = new string[2]
    {
      "dd/MM/yyyy tt hh:mm",
      "dd/MM/yyyy tt hh:mm:ss"
    };
    string str = "";
    if (CultureInfoUtils.SystemCultureInfo != null)
    {
      string name = CultureInfoUtils.SystemUICultureInfo.Name;
      string[] strArray1;
      if (name == null)
        strArray1 = (string[]) null;
      else
        strArray1 = name.Split('-');
      string[] strArray2 = strArray1;
      if (strArray2 != null && strArray2.Length == 2 && strArray2[1].Length == 2 && (dictionary1.ContainsKey(strArray2[1]) || dictionary2.ContainsKey(strArray2[1])))
        str = strArray2[1];
    }
    if (string.IsNullOrEmpty(str))
      str = CultureInfoUtils.ActualAppLanguage;
    string upperInvariant = str.ToUpperInvariant();
    string[] collection1;
    if (dictionary1.TryGetValue(upperInvariant, out collection1))
    {
      source.AddRange((IEnumerable<string>) collection1);
    }
    else
    {
      string[] collection2;
      if (dictionary2.TryGetValue(upperInvariant, out collection2))
        source.AddRange((IEnumerable<string>) collection2);
    }
    foreach (KeyValuePair<string, string[]> keyValuePair in dictionary1)
    {
      if (keyValuePair.Key != upperInvariant)
        source.AddRange((IEnumerable<string>) keyValuePair.Value);
    }
    foreach (KeyValuePair<string, string[]> keyValuePair in dictionary2)
    {
      if (keyValuePair.Key != upperInvariant)
        source.AddRange((IEnumerable<string>) keyValuePair.Value);
    }
    return source.Distinct<string>().ToArray<string>();
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/stampeditwindows.xaml", UriKind.Relative));
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
        this.LayoutRoot = (Grid) target;
        break;
      case 2:
        this.Menus = (ListBox) target;
        this.Menus.SelectionChanged += new SelectionChangedEventHandler(this.Menus_SelectionChanged);
        break;
      case 3:
        this.TextPanel = (StackPanel) target;
        break;
      case 4:
        this.PreviewImageContainer = (StampDefaultTextPreview) target;
        break;
      case 5:
        this.colorSelecters = (Border) target;
        break;
      case 6:
        this.ForegroundPickerList = (ListBox) target;
        this.ForegroundPickerList.SelectionChanged += new SelectionChangedEventHandler(this.ForegroundPickerList_SelectionChanged);
        break;
      case 7:
        this.ForegroundPicker = (ColorPickerButton) target;
        break;
      case 8:
        this.TypeWriterCtrl = (TextBox) target;
        this.TypeWriterCtrl.TextChanged += new TextChangedEventHandler(this.txt_Text_TextChanged);
        break;
      case 9:
        this.DateCheck = (CheckBox) target;
        this.DateCheck.Click += new RoutedEventHandler(this.DateCheck_Click);
        break;
      case 10:
        this.DateFormatComboBox = (ComboBox) target;
        this.DateFormatComboBox.AddHandler(TextBoxBase.TextChangedEvent, (Delegate) new TextChangedEventHandler(this.DateFormatComboBox_TextChanged));
        break;
      case 11:
        this.ImagePanel = (StackPanel) target;
        break;
      case 12:
        this.body = (Border) target;
        break;
      case 13:
        this.PictureCtrl = (Button) target;
        this.PictureCtrl.Click += new RoutedEventHandler(this.PictureCtrl_Click);
        break;
      case 14:
        this.showPicture = (Image) target;
        break;
      case 15:
        this.btnClear = (Button) target;
        this.btnClear.Click += new RoutedEventHandler(this.btnClear_Click);
        break;
      case 16 /*0x10*/:
        this.ckbRemoveBg = (CheckBox) target;
        break;
      case 17:
        this.imgHelp = (Image) target;
        break;
      case 18:
        this.SaveCheck = (CheckBox) target;
        break;
      case 19:
        this.btnCancel = (Button) target;
        break;
      case 20:
        this.btnOk = (Button) target;
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
