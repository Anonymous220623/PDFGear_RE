// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.LinkEditWindows
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Win32;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using pdfeditor.Controls.ColorPickers;
using pdfeditor.Utils;
using pdfeditor.ViewModels;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Controls;

public partial class LinkEditWindows : Window, IComponentConnector
{
  internal string FileDiaoligFiePath = string.Empty;
  public bool RactVisible = true;
  public string UrlFilePath = string.Empty;
  public int Page = 1;
  private PdfDocument Document;
  public string SelectedFontground = "#FF000000";
  public bool rectangleVis = true;
  public float BorderWidth = 1f;
  public BorderStyles BorderStyles;
  internal ComboBox linkstylyCombobox;
  internal ComboBox LinestyleCombobox;
  internal ComboBox BorderstyleCombobox;
  internal ColorPickerButton WatermarkColorPicker;
  internal RadioButton ToPageRadioButton;
  internal TextBox pagecur;
  internal TextBlock pagenum;
  internal RadioButton ToWebRadioButton;
  internal TextBox urlcur;
  internal RadioButton ToFileRadioButton;
  internal TextBox localcur;
  internal Button openfolder;
  internal Button btnCancel;
  internal Button btnOk;
  private bool _contentLoaded;

  public LinkSelect SelectedType { get; set; }

  public LinkEditWindows(PdfDocument pdfDocument)
  {
    this.InitializeComponent();
    GAManager.SendEvent("PDFLink", "WindowShow", "Create", 1L);
    this.Loaded += new RoutedEventHandler(this.LinkEditWindows_Loaded);
    this.Document = pdfDocument;
    this.pagenum.Text = "/ " + pdfDocument.Pages.Count.ToString();
    this.SelectedType = LinkSelect.ToPage;
    this.InitMenu();
  }

  public LinkEditWindows(LinkAnnotationModel linkAnnotationModel)
  {
    this.InitializeComponent();
    GAManager.SendEvent("PDFLink", "WindowShow", "Edit", 1L);
    this.Editorloaded(linkAnnotationModel);
    this.InitMenu();
  }

  private void LinkEditWindows_Loaded(object sender, RoutedEventArgs e)
  {
    this.WatermarkColorPicker.SelectedColor = (System.Windows.Media.Color) ColorConverter.ConvertFromString(this.SelectedFontground);
  }

  private void InitMenu()
  {
    this.btnOk.Click += (RoutedEventHandler) ((o, e) =>
    {
      if (!this.CheckOk())
        return;
      Ioc.Default.GetRequiredService<MainViewModel>();
      if (this.SelectedType == LinkSelect.ToWeb && string.IsNullOrEmpty(this.UrlFilePath))
      {
        int num1 = (int) ModernMessageBox.Show(pdfeditor.Properties.Resources.LinkUriEmpty, UtilManager.GetProductName());
      }
      else if (this.SelectedType == LinkSelect.ToFile && string.IsNullOrEmpty(this.FileDiaoligFiePath))
      {
        int num2 = (int) ModernMessageBox.Show(pdfeditor.Properties.Resources.LinkUriEmpty, UtilManager.GetProductName());
      }
      else if (this.SelectedType == LinkSelect.ToWeb && !Uri.TryCreate(this.UrlFilePath, UriKind.Absolute, out Uri _))
      {
        int num3 = (int) ModernMessageBox.Show(pdfeditor.Properties.Resources.LinkUriError, UtilManager.GetProductName());
      }
      else
      {
        System.Windows.Media.Color selectedColor = this.WatermarkColorPicker.SelectedColor;
        this.SelectedFontground = $"#{selectedColor.A:X2}{selectedColor.R:X2}{selectedColor.G:X2}{selectedColor.B:X2}";
        ComboBoxItem selectedItem = this.linkstylyCombobox.SelectedItem as ComboBoxItem;
        float.TryParse((this.BorderstyleCombobox.SelectedItem as ComboBoxItem).Tag.ToString(), out this.BorderWidth);
        this.BorderStyles = LinkEditWindows.GetBorder(this.LinestyleCombobox.SelectedIndex);
        if (selectedItem.Content.ToString() == pdfeditor.Properties.Resources.LinkWinInvisibleRect)
          this.rectangleVis = false;
        GAManager.SendEvent("PDFLink", "WindowCreateEdit", this.SelectedType.ToString(), 1L);
        this.DialogResult = new bool?(true);
      }
    });
    this.btnCancel.Click += (RoutedEventHandler) ((o, e) =>
    {
      Ioc.Default.GetRequiredService<MainViewModel>();
      this.DialogResult = new bool?(false);
      this.Close();
    });
  }

  private static BorderStyles GetBorder(int Selectindex)
  {
    switch (Selectindex)
    {
      case 0:
        return BorderStyles.Solid;
      case 1:
        return BorderStyles.Dashed;
      case 2:
        return BorderStyles.Underline;
      default:
        return BorderStyles.Solid;
    }
  }

  private static int SetBorder(BorderStyles borderStyles)
  {
    switch (borderStyles)
    {
      case BorderStyles.Solid:
        return 0;
      case BorderStyles.Dashed:
        return 1;
      case BorderStyles.Underline:
        return 2;
      default:
        return 0;
    }
  }

  public static bool isInputNumber(KeyEventArgs e)
  {
    if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key == Key.Delete || e.Key == Key.Back || e.Key == Key.Left || e.Key == Key.Right)
    {
      if (e.KeyboardDevice.Modifiers == ModifierKeys.None)
        return true;
      e.Handled = true;
    }
    else
      e.Handled = true;
    return false;
  }

  private bool CheckOk()
  {
    if (this.SelectedType == LinkSelect.ToPage && !string.IsNullOrEmpty(this.pagecur.Text))
    {
      if (this.Page > 0 && this.Page <= this.Document.Pages.Count)
        return true;
      int num = (int) ModernMessageBox.Show(pdfeditor.Properties.Resources.LinkPageError, UtilManager.GetProductName());
      return false;
    }
    if (this.SelectedType == LinkSelect.ToWeb && !string.IsNullOrEmpty(this.urlcur.Text))
    {
      this.UrlFilePath = this.urlcur.Text;
      return true;
    }
    if (this.SelectedType == LinkSelect.ToFile && !string.IsNullOrEmpty(this.localcur.Text))
    {
      this.FileDiaoligFiePath = this.localcur.Text;
      return true;
    }
    int num1 = (int) ModernMessageBox.Show(pdfeditor.Properties.Resources.LinkUriEmpty, UtilManager.GetProductName());
    return false;
  }

  private void openfolder_Click(object sender, RoutedEventArgs e)
  {
    OpenFileDialog openFileDialog1 = new OpenFileDialog();
    openFileDialog1.Filter = "All Files(*.*)|*.*";
    openFileDialog1.ShowReadOnly = false;
    openFileDialog1.ReadOnlyChecked = true;
    OpenFileDialog openFileDialog2 = openFileDialog1;
    if (openFileDialog2.ShowDialog().GetValueOrDefault())
    {
      if (!string.IsNullOrEmpty(openFileDialog2.FileName))
      {
        try
        {
          this.FileDiaoligFiePath = openFileDialog2.FileName;
          this.localcur.Text = openFileDialog2.FileName;
          goto label_5;
        }
        catch
        {
          DrawUtils.ShowUnsupportedImageMessage();
          return;
        }
      }
    }
    this.FileDiaoligFiePath = string.Empty;
label_5:
    this.Activate();
  }

  private void CurrentPageRadioButton_Click(object sender, RoutedEventArgs e)
  {
    RadioButton radioButton = sender as RadioButton;
    if (!radioButton.IsChecked.Value)
      return;
    switch (radioButton.Name)
    {
      case "ToFileRadioButton":
        this.SelectedType = LinkSelect.ToFile;
        this.localcur.IsEnabled = true;
        this.urlcur.IsEnabled = false;
        this.pagecur.IsEnabled = false;
        this.openfolder.IsEnabled = true;
        this.pagecur.SetResourceReference(Control.BackgroundProperty, (object) "SignaturePickerBackground");
        break;
      case "ToWebRadioButton":
        this.SelectedType = LinkSelect.ToWeb;
        this.localcur.IsEnabled = false;
        this.urlcur.IsEnabled = true;
        this.pagecur.IsEnabled = false;
        this.openfolder.IsEnabled = false;
        this.pagecur.SetResourceReference(Control.BackgroundProperty, (object) "SignaturePickerBackground");
        break;
      case "ToPageRadioButton":
        this.SelectedType = LinkSelect.ToPage;
        this.localcur.IsEnabled = false;
        this.urlcur.IsEnabled = false;
        this.pagecur.IsEnabled = true;
        this.openfolder.IsEnabled = false;
        if (this.Page > 0 && this.Page <= this.Document.Pages.Count)
        {
          this.pagecur.SetResourceReference(Control.BackgroundProperty, (object) "SignaturePickerBackground");
          break;
        }
        this.pagecur.SetResourceReference(Control.BackgroundProperty, (object) "LinkEditWindowsErrorPage");
        break;
    }
  }

  private void textBox_PreviewKeyDown(object sender, KeyEventArgs e)
  {
    LinkEditWindows.isInputNumber(e);
  }

  private void pagecur_TextChanged(object sender, TextChangedEventArgs e)
  {
    try
    {
      if (this.pagecur.Text.Length <= 0)
        return;
      int num = int.Parse(this.pagecur.Text);
      if (num > 0 && num <= this.Document.Pages.Count)
        this.pagecur.SetResourceReference(Control.BackgroundProperty, (object) "SignaturePickerBackground");
      else if (num <= 0)
        this.pagecur.SetResourceReference(Control.BackgroundProperty, (object) "LinkEditWindowsErrorPage");
      else if (num > this.Document.Pages.Count)
        this.pagecur.SetResourceReference(Control.BackgroundProperty, (object) "LinkEditWindowsErrorPage");
      this.Page = int.Parse(this.pagecur.Text);
    }
    catch
    {
    }
  }

  private void Editorloaded(LinkAnnotationModel linkAnnotationModel)
  {
    if (linkAnnotationModel == null)
      return;
    this.Title = linkAnnotationModel.Title;
    this.Document = linkAnnotationModel.PdfDocument;
    this.pagenum.Text = "/ " + linkAnnotationModel.PdfDocument.Pages.Count.ToString();
    this.SelectedType = linkAnnotationModel.Action;
    this.WatermarkColorPicker.SelectedColor = linkAnnotationModel.BorderColor;
    if (this.SelectedType == LinkSelect.ToWeb)
    {
      this.urlcur.Text = linkAnnotationModel.Uri;
      this.ToWebRadioButton.IsChecked = new bool?(true);
      this.localcur.IsEnabled = false;
      this.urlcur.IsEnabled = true;
      this.pagecur.IsEnabled = false;
      this.openfolder.IsEnabled = false;
    }
    else if (this.SelectedType == LinkSelect.ToFile)
    {
      this.localcur.Text = linkAnnotationModel.FileName;
      this.ToFileRadioButton.IsChecked = new bool?(true);
      this.localcur.IsEnabled = true;
      this.urlcur.IsEnabled = false;
      this.pagecur.IsEnabled = false;
      this.openfolder.IsEnabled = true;
    }
    else
      this.pagecur.Text = linkAnnotationModel.Page.ToString();
    if ((double) linkAnnotationModel.Width == 0.0)
    {
      this.linkstylyCombobox.SelectedIndex = 1;
      this.BorderstyleCombobox.SelectedIndex = 2;
    }
    else if ((double) linkAnnotationModel.Width == 0.25)
    {
      this.linkstylyCombobox.SelectedIndex = 0;
      this.BorderstyleCombobox.SelectedIndex = 0;
    }
    else if ((double) linkAnnotationModel.Width == 0.5)
    {
      this.linkstylyCombobox.SelectedIndex = 0;
      this.BorderstyleCombobox.SelectedIndex = 1;
    }
    else
    {
      this.BorderstyleCombobox.SelectedIndex = (int) ((double) linkAnnotationModel.Width + 1.0);
      this.LinestyleCombobox.SelectedIndex = LinkEditWindows.SetBorder(linkAnnotationModel.BorderStyle);
    }
  }

  private void linkstylyCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    int selectedIndex = (sender as ComboBox).SelectedIndex;
    if (this.LinestyleCombobox == null || this.BorderstyleCombobox == null || this.WatermarkColorPicker == null)
      return;
    if (selectedIndex == 0)
    {
      this.LinestyleCombobox.IsEnabled = true;
      this.BorderstyleCombobox.IsEnabled = true;
      this.WatermarkColorPicker.IsEnabled = true;
    }
    else
    {
      this.LinestyleCombobox.IsEnabled = false;
      this.BorderstyleCombobox.IsEnabled = false;
      this.WatermarkColorPicker.IsEnabled = false;
    }
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/linkeditwindows.xaml", UriKind.Relative));
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
        this.linkstylyCombobox = (ComboBox) target;
        this.linkstylyCombobox.SelectionChanged += new SelectionChangedEventHandler(this.linkstylyCombobox_SelectionChanged);
        break;
      case 2:
        this.LinestyleCombobox = (ComboBox) target;
        break;
      case 3:
        this.BorderstyleCombobox = (ComboBox) target;
        break;
      case 4:
        this.WatermarkColorPicker = (ColorPickerButton) target;
        break;
      case 5:
        this.ToPageRadioButton = (RadioButton) target;
        this.ToPageRadioButton.Click += new RoutedEventHandler(this.CurrentPageRadioButton_Click);
        break;
      case 6:
        this.pagecur = (TextBox) target;
        this.pagecur.PreviewKeyDown += new KeyEventHandler(this.textBox_PreviewKeyDown);
        this.pagecur.TextChanged += new TextChangedEventHandler(this.pagecur_TextChanged);
        break;
      case 7:
        this.pagenum = (TextBlock) target;
        break;
      case 8:
        this.ToWebRadioButton = (RadioButton) target;
        this.ToWebRadioButton.Click += new RoutedEventHandler(this.CurrentPageRadioButton_Click);
        break;
      case 9:
        this.urlcur = (TextBox) target;
        break;
      case 10:
        this.ToFileRadioButton = (RadioButton) target;
        this.ToFileRadioButton.Click += new RoutedEventHandler(this.CurrentPageRadioButton_Click);
        break;
      case 11:
        this.localcur = (TextBox) target;
        break;
      case 12:
        this.openfolder = (Button) target;
        this.openfolder.Click += new RoutedEventHandler(this.openfolder_Click);
        break;
      case 13:
        this.btnCancel = (Button) target;
        break;
      case 14:
        this.btnOk = (Button) target;
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
