// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.Controls.PageSetupUI
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Windows.Shared;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace Syncfusion.Windows.Tools.Controls;

internal class PageSetupUI : ChromelessWindow, IComponentConnector
{
  private Dictionary<string, PaperSize> pages = new Dictionary<string, PaperSize>();
  private double _pageWidth;
  private double _pageHeight;
  private double _leftMargin;
  private double _topMargin;
  private double _bottomMargin;
  private double _rightMargin;
  private PrintPageSettings pageSetupSettings;
  internal Grid grd_General;
  internal UpDown top;
  internal UpDown bottom;
  internal UpDown left;
  internal UpDown right;
  internal RadioButton portrait;
  internal RadioButton landscape;
  internal ComboBox pagesize;
  internal UpDown pageWidth;
  internal UpDown pageHeight;
  internal Button Cancel_button;
  internal Button Ok_button;
  internal Button Default;
  private bool _contentLoaded;

  public bool IsInternalChange { get; set; }

  public double PageWidth => this._pageWidth;

  public double PageHeight => this._pageHeight;

  public double LeftMargin => this._leftMargin;

  public double TopMargin => this._topMargin;

  public double BottomMargin => this._bottomMargin;

  public double RightMargin => this._rightMargin;

  private PrintPageSettings PageSetupSettings
  {
    get => this.pageSetupSettings;
    set => this.pageSetupSettings = value;
  }

  public PageSetupUI(PrintPageSettings pageSettings)
  {
    this.InitializeComponent();
    this.PageSetupSettings = pageSettings;
    this.LoadPageDimention();
    this.Ok_button.Click += new RoutedEventHandler(this.Ok_button_Click);
    this.pagesize.SelectionChanged += new SelectionChangedEventHandler(this.pagesize_SelectionChanged);
    this.landscape.Checked += new RoutedEventHandler(this.Orientation_Checked);
    this.portrait.Checked += new RoutedEventHandler(this.Orientation_Checked);
    this.Default.Click += new RoutedEventHandler(this.Default_Click);
    foreach (object key in this.pages.Keys)
      this.pagesize.Items.Add(key);
    this.pagesize.SelectedIndex = 53;
    this.GetPageSettings();
  }

  private void GetPageSettings()
  {
    this.portrait.IsChecked = new bool?(this.PageSetupSettings.PageOrientation == PrintPageSettings.Orientation.Portrait);
    this.landscape.IsChecked = new bool?(this.PageSetupSettings.PageOrientation == PrintPageSettings.Orientation.Landscape);
    this.top.Value = new double?(this.PageSetupSettings.PageMargin.Top);
    this.bottom.Value = new double?(this.PageSetupSettings.PageMargin.Bottom);
    this.left.Value = new double?(this.PageSetupSettings.PageMargin.Left);
    this.right.Value = new double?(this.PageSetupSettings.PageMargin.Right);
    this.pageHeight.Value = new double?(this.PageSetupSettings.PageHeight);
    this.pageWidth.Value = new double?(this.PageSetupSettings.PageWidth);
    this.pagesize.SelectedItem = (object) this.PageSetupSettings.PageType;
  }

  private void SetPageSettings()
  {
    this.PageSetupSettings.PageOrientation = this.portrait.IsChecked.Value ? PrintPageSettings.Orientation.Portrait : PrintPageSettings.Orientation.Landscape;
    this.PageSetupSettings.PageMargin = new Thickness(this.left.Value.Value * 96.0, this.top.Value.Value * 96.0, this.right.Value.Value * 96.0, this.bottom.Value.Value * 96.0);
    this.PageSetupSettings.PageHeight = this.pageHeight.Value.Value * 96.0;
    this.PageSetupSettings.PageWidth = this.pageWidth.Value.Value * 96.0;
    this.PageSetupSettings.PageType = this.pagesize.SelectedItem.ToString();
  }

  private void Default_Click(object sender, RoutedEventArgs e)
  {
    this.portrait.IsChecked = new bool?(true);
    this.pagesize.SelectedIndex = 53;
    this.left.Value = new double?(1.0);
    this.right.Value = new double?(1.0);
    this.top.Value = new double?(1.0);
    this.bottom.Value = new double?(1.0);
  }

  private void Orientation_Checked(object sender, RoutedEventArgs e)
  {
    double num1 = this.pageWidth.Value.Value;
    double num2 = this.pageHeight.Value.Value;
    double num3 = num1;
    double num4 = num2;
    this.pageHeight.Value = new double?(num3);
    this.pageWidth.Value = new double?(num4);
  }

  private void pagesize_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    if (this.IsInternalChange)
      return;
    double height = this.pages[this.pagesize.SelectedItem.ToString()].Height;
    double width = this.pages[this.pagesize.SelectedItem.ToString()].Width;
    this.pageHeight.Value = new double?(this.portrait.IsChecked.Value ? height : width);
    this.pageWidth.Value = new double?(this.portrait.IsChecked.Value ? width : height);
  }

  private void Ok_button_Click(object sender, RoutedEventArgs e)
  {
    try
    {
      this.SetPageSettings();
      if (this.PageSetupSettings.PageMargin.Left + this.PageSetupSettings.PageMargin.Right >= this.PageSetupSettings.PageWidth || this.PageSetupSettings.PageMargin.Top + this.PageSetupSettings.PageMargin.Bottom >= this.PageSetupSettings.PageHeight)
      {
        int num = (int) MessageBox.Show(SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "msgBoxMargin"), SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "PageSetup"), MessageBoxButton.OK, MessageBoxImage.Hand);
      }
      else
      {
        this.DialogResult = new bool?(true);
        this.Close();
      }
    }
    catch (Exception ex)
    {
      int num = (int) MessageBox.Show(ex.Message);
    }
  }

  private void LoadPageDimention()
  {
    this.pages.Add("10 x 11", new PaperSize(10.0, 11.0));
    this.pages.Add("10 x 14", new PaperSize(10.0, 14.0));
    this.pages.Add("11 x 17", new PaperSize(11.0, 17.0));
    this.pages.Add("12 x 11", new PaperSize(15.0, 11.0));
    this.pages.Add("6 3/4 Envelop", new PaperSize(3.63, 6.5));
    this.pages.Add("9 x 11", new PaperSize(9.0, 11.0));
    this.pages.Add("A2", new PaperSize(16.5, 23.4));
    this.pages.Add("A3", new PaperSize(11.7, 16.5));
    this.pages.Add("A3 Extra", new PaperSize(12.68, 17.52));
    this.pages.Add("A3 Rotated", new PaperSize(16.54, 11.69));
    this.pages.Add("A4", new PaperSize(8.27, 11.69));
    this.pages.Add("A4 Extra", new PaperSize(9.27, 12.69));
    this.pages.Add("A4 Plus", new PaperSize(8.27, 12.99));
    this.pages.Add("A4 Rotated", new PaperSize(11.69, 8.27));
    this.pages.Add("A5", new PaperSize(5.83, 8.27));
    this.pages.Add("A5 Extra", new PaperSize(6.85, 9.25));
    this.pages.Add("A5 Rotated", new PaperSize(8.27, 5.83));
    this.pages.Add("A6", new PaperSize(4.13, 5.83));
    this.pages.Add("B4(ISO)", new PaperSize(9.84, 13.9));
    this.pages.Add("B4(JIS)", new PaperSize(10.12, 14.33));
    this.pages.Add("B4(JIS)Rotated", new PaperSize(14.33, 10.12));
    this.pages.Add("B5(JIS)", new PaperSize(7.17, 10.12));
    this.pages.Add("B5(JIS)Rotated", new PaperSize(10.12, 7.17));
    this.pages.Add("B6(JIS)", new PaperSize(5.04, 7.17));
    this.pages.Add("B6(JIS)Rotated", new PaperSize(7.17, 5.04));
    this.pages.Add("C size sheet", new PaperSize(17.0, 22.0));
    this.pages.Add("D size sheet", new PaperSize(22.0, 34.0));
    this.pages.Add("E size sheet", new PaperSize(34.0, 44.0));
    this.pages.Add("Envolope", new PaperSize(4.33, 9.06));
    this.pages.Add("Envelope #9", new PaperSize(3.875, 8.875));
    this.pages.Add("Envelope #10", new PaperSize(4.125, 9.5));
    this.pages.Add("Envelope #11", new PaperSize(4.5, 10.375));
    this.pages.Add("Envelope #12", new PaperSize(4.75, 11.0));
    this.pages.Add("Envelope #14", new PaperSize(5.0, 11.5));
    this.pages.Add("Envelope DL", new PaperSize(4.33, 8.66));
    this.pages.Add("Envelope C3", new PaperSize(12.75, 18.0));
    this.pages.Add("Envelope C4", new PaperSize(9.0, 12.75));
    this.pages.Add("Envelope C5", new PaperSize(6.38, 9.02));
    this.pages.Add("Envelope C6", new PaperSize(4.5, 6.4));
    this.pages.Add("Envelope C65", new PaperSize(4.5, 9.0));
    this.pages.Add("Envelope B4", new PaperSize(9.85, 13.9));
    this.pages.Add("Envelope B5", new PaperSize(6.9, 9.85));
    this.pages.Add("Envelope B6", new PaperSize(6.9, 4.9));
    this.pages.Add("Envelope Monarch", new PaperSize(3.875, 7.5));
    this.pages.Add("Envelope Invite", new PaperSize(8.66, 8.66));
    this.pages.Add("Executive", new PaperSize(7.25, 10.5));
    this.pages.Add("Folio", new PaperSize(8.5, 13.0));
    this.pages.Add("German LegalFanfold", new PaperSize(8.5, 13.0));
    this.pages.Add("Japanese Postcard", new PaperSize(3.94, 5.83));
    this.pages.Add("Japanese Double Postcard", new PaperSize(7.87, 5.83));
    this.pages.Add("Ledger", new PaperSize(17.0, 11.0));
    this.pages.Add("Legal", new PaperSize(8.5, 14.0));
    this.pages.Add("Legal Extra", new PaperSize(9.5, 15.0));
    this.pages.Add("Letter", new PaperSize(8.5, 11.0));
    this.pages.Add("Letter Plus", new PaperSize(8.5, 12.69));
    this.pages.Add("Letter Extra", new PaperSize(9.5, 12.0));
    this.pages.Add("Note", new PaperSize(8.5, 11.0));
    this.pages.Add("Quarto", new PaperSize(8.46, 10.83));
    this.pages.Add("Statement", new PaperSize(5.5, 8.5));
    this.pages.Add("SuperA", new PaperSize(8.94, 14.02));
    this.pages.Add("SuperB", new PaperSize(8.94, 14.02));
    this.pages.Add("Tabloid", new PaperSize(11.0, 17.0));
    this.pages.Add("TabloidExtra", new PaperSize(12.0, 18.0));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/Syncfusion.Shared.Wpf;component/controls/printpreview/pagesetupui.xaml", UriKind.Relative));
  }

  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [DebuggerNonUserCode]
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
        this.grd_General = (Grid) target;
        break;
      case 2:
        this.top = (UpDown) target;
        break;
      case 3:
        this.bottom = (UpDown) target;
        break;
      case 4:
        this.left = (UpDown) target;
        break;
      case 5:
        this.right = (UpDown) target;
        break;
      case 6:
        this.portrait = (RadioButton) target;
        break;
      case 7:
        this.landscape = (RadioButton) target;
        break;
      case 8:
        this.pagesize = (ComboBox) target;
        break;
      case 9:
        this.pageWidth = (UpDown) target;
        break;
      case 10:
        this.pageHeight = (UpDown) target;
        break;
      case 11:
        this.Cancel_button = (Button) target;
        break;
      case 12:
        this.Ok_button = (Button) target;
        break;
      case 13:
        this.Default = (Button) target;
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
