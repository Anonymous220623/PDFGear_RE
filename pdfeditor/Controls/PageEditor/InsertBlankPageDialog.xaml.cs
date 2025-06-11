// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.PageEditor.InsertBlankPageDialog
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using CommomLib.Controls;
using Patagames.Pdf;
using Patagames.Pdf.Net;
using pdfeditor.Utils;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace pdfeditor.Controls.PageEditor;

public partial class InsertBlankPageDialog : Window, IComponentConnector
{
  private readonly PdfDocument doc;
  private int firstSelectedPage = -1;
  private int lastSelectedPage = -1;
  private int selectedFirstIndex = -1;
  private SizeF DefaultSize = new SizeF(210f, 297f);
  private SizeF A4SizeF = new SizeF(210f, 297f);
  private SizeF A3SizeF = new SizeF(297f, 420f);
  private Dictionary<string, SizeF> dicItem = new Dictionary<string, SizeF>();
  internal ComboBox cbPageSize;
  internal HeaderedContentControl tbPageWidth;
  internal NumberBox tboxPageWidth;
  internal HeaderedContentControl tbPageHeight;
  internal NumberBox tboxPageHeight;
  internal RadioButton PortraitRadioButton;
  internal RadioButton LandscapeRadioButton;
  internal NumberBox PageCountNumberBox;
  internal RadioButton BeginingRadioButton;
  internal RadioButton EndRadioButton;
  internal RadioButton PageRadioButton;
  internal TextBox PageindexNumbox;
  internal TextBlock PageNumber;
  internal ComboBox InsertPosition;
  private bool _contentLoaded;

  public InsertBlankPageDialog(
    IEnumerable<int> selectedPages,
    PdfDocument doc,
    bool fromSinglePageCmd = false)
  {
    this.InitializeComponent();
    this.doc = doc;
    List<int> list = selectedPages.ToList<int>();
    list.Sort();
    if (list.Count > 0)
    {
      this.selectedFirstIndex = list[0];
      this.PageindexNumbox.Text = selectedPages.ConvertToRange();
    }
    else
    {
      this.selectedFirstIndex = 0;
      this.PageindexNumbox.Text = "";
    }
    if (doc != null)
      this.PageNumber.Text = doc.Pages.Count.ToString();
    this.InitInsertPositionRadioButtons(selectedPages, fromSinglePageCmd);
    this.InitControls();
  }

  private void InitControls()
  {
    try
    {
      if (this.selectedFirstIndex != -1)
      {
        PdfPage page = this.doc.Pages[this.selectedFirstIndex];
        FS_SIZEF pageSizeByIndex = this.doc.GetPageSizeByIndex(this.selectedFirstIndex);
        this.DefaultSize.Width = (float) InsertBlankPageDialog.GetMMValueFromPix(pageSizeByIndex.Width);
        this.DefaultSize.Height = (float) InsertBlankPageDialog.GetMMValueFromPix(pageSizeByIndex.Height);
      }
      this.dicItem.Add($"{pdfeditor.Properties.Resources.WinPageInsertSettingPageSizeItemDefault} ({this.DefaultSize.Width}mm x {this.DefaultSize.Height}mm)", this.A4SizeF);
      this.dicItem.Add("A4 (210mm x 297mm)", this.A4SizeF);
      this.dicItem.Add("A3 (297mm x 420mm)", this.A3SizeF);
      this.dicItem.Add(pdfeditor.Properties.Resources.WinPageInsertSettingPageSizeItemCustomize, this.DefaultSize);
      this.cbPageSize.ItemsSource = (IEnumerable) this.dicItem;
      this.cbPageSize.SelectedValuePath = "Value";
      this.cbPageSize.SelectedIndex = 0;
      this.tboxPageWidth.Value = (double) this.DefaultSize.Width;
      this.tboxPageHeight.Value = (double) this.DefaultSize.Height;
    }
    catch
    {
    }
  }

  public int PageCount { get; private set; }

  public int InsertPageIndex { get; private set; }

  public bool InsertBefore { get; private set; }

  public SizeF PageSize { get; private set; }

  private static float GetPixValueFromMM(float mm) => (float) ((double) mm * 7200.0 / 2540.0);

  private static int GetMMValueFromPix(float pix)
  {
    return (int) Math.Round((double) pix * 2540.0 / 7200.0, 0);
  }

  private void InitInsertPositionRadioButtons(
    IEnumerable<int> selectedPages,
    bool fromSinglePageCmd)
  {
    if (fromSinglePageCmd)
      this.InitInsertPositionRadioButtonsFromSinglePage(selectedPages);
    else
      this.InitInsertPositionRadioButtonsFromMultiPages(selectedPages);
  }

  private void InitInsertPositionRadioButtonsFromSinglePage(IEnumerable<int> selectedPages)
  {
    int count = this.doc.Pages.Count;
    int[] array = selectedPages != null ? selectedPages.ToArray<int>() : (int[]) null;
    if (array != null && array.Length == 1 && array[0] >= 0 && array[0] < count)
    {
      this.firstSelectedPage = array[0];
      this.lastSelectedPage = array[0];
      this.PageRadioButton.IsEnabled = true;
      this.PageRadioButton.IsChecked = new bool?(true);
      this.PageNumber.Text = count.ToString();
    }
    else
    {
      this.EndRadioButton.IsChecked = new bool?(true);
      this.InitInsertPositionRadioButtonsFromMultiPages(selectedPages);
    }
  }

  private void InitInsertPositionRadioButtonsFromMultiPages(IEnumerable<int> selectedPages)
  {
    int count = this.doc.Pages.Count;
    int[] array = selectedPages != null ? selectedPages.ToArray<int>() : (int[]) null;
    if (array != null)
    {
      int val2_1 = int.MaxValue;
      int val2_2 = int.MinValue;
      for (int index = 0; index < array.Length; ++index)
      {
        if (array[index] >= 0 && array[index] < count)
        {
          val2_1 = Math.Min(array[index], val2_1);
          val2_2 = Math.Max(array[index], val2_2);
        }
      }
      if (val2_1 != int.MaxValue && val2_1 >= 0)
        this.firstSelectedPage = val2_1;
      if (val2_2 != int.MinValue && val2_2 <= count - 1)
        this.lastSelectedPage = val2_2;
    }
    if (this.firstSelectedPage != -1)
    {
      this.PageRadioButton.IsEnabled = true;
      this.PageRadioButton.IsChecked = new bool?(true);
      this.InsertPosition.SelectedIndex = 1;
      this.PageNumber.Text = count.ToString();
    }
    if (this.lastSelectedPage == -1)
      return;
    this.PageRadioButton.IsEnabled = true;
    this.PageRadioButton.IsChecked = new bool?(true);
    this.InsertPosition.SelectedIndex = 0;
    this.PageNumber.Text = count.ToString();
  }

  private void cbPageSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    try
    {
      if (this.cbPageSize.SelectedIndex == 3)
      {
        this.tbPageHeight.Visibility = Visibility.Visible;
        this.tbPageWidth.Visibility = Visibility.Visible;
        this.PortraitRadioButton.Visibility = Visibility.Collapsed;
        this.LandscapeRadioButton.Visibility = Visibility.Collapsed;
      }
      else
      {
        this.tbPageHeight.Visibility = Visibility.Collapsed;
        this.tbPageWidth.Visibility = Visibility.Collapsed;
        this.PortraitRadioButton.Visibility = Visibility.Visible;
        this.LandscapeRadioButton.Visibility = Visibility.Visible;
      }
    }
    catch
    {
    }
  }

  private void OKButton_Click(object sender, RoutedEventArgs e)
  {
    try
    {
      if (this.PageCountNumberBox.IsKeyboardFocusWithin)
        Keyboard.ClearFocus();
      if (!this.PageCountNumberBox.IsValid)
        return;
      this.PageCount = (int) this.PageCountNumberBox.Value;
      bool? isChecked;
      float pixValueFromMm1;
      float pixValueFromMm2;
      if (this.cbPageSize.SelectedIndex == 0)
      {
        isChecked = this.PortraitRadioButton.IsChecked;
        bool flag = false;
        if (isChecked.GetValueOrDefault() == flag & isChecked.HasValue)
        {
          pixValueFromMm1 = InsertBlankPageDialog.GetPixValueFromMM(this.DefaultSize.Height);
          pixValueFromMm2 = InsertBlankPageDialog.GetPixValueFromMM(this.DefaultSize.Width);
        }
        else
        {
          pixValueFromMm1 = InsertBlankPageDialog.GetPixValueFromMM(this.DefaultSize.Width);
          pixValueFromMm2 = InsertBlankPageDialog.GetPixValueFromMM(this.DefaultSize.Height);
        }
      }
      else if (this.cbPageSize.SelectedIndex == 1)
      {
        isChecked = this.PortraitRadioButton.IsChecked;
        bool flag = false;
        if (isChecked.GetValueOrDefault() == flag & isChecked.HasValue)
        {
          pixValueFromMm1 = InsertBlankPageDialog.GetPixValueFromMM(this.A4SizeF.Height);
          pixValueFromMm2 = InsertBlankPageDialog.GetPixValueFromMM(this.A4SizeF.Width);
        }
        else
        {
          pixValueFromMm1 = InsertBlankPageDialog.GetPixValueFromMM(this.A4SizeF.Width);
          pixValueFromMm2 = InsertBlankPageDialog.GetPixValueFromMM(this.A4SizeF.Height);
        }
      }
      else if (this.cbPageSize.SelectedIndex == 2)
      {
        isChecked = this.PortraitRadioButton.IsChecked;
        bool flag = false;
        if (isChecked.GetValueOrDefault() == flag & isChecked.HasValue)
        {
          pixValueFromMm1 = InsertBlankPageDialog.GetPixValueFromMM(this.A3SizeF.Height);
          pixValueFromMm2 = InsertBlankPageDialog.GetPixValueFromMM(this.A3SizeF.Width);
        }
        else
        {
          pixValueFromMm1 = InsertBlankPageDialog.GetPixValueFromMM(this.A3SizeF.Width);
          pixValueFromMm2 = InsertBlankPageDialog.GetPixValueFromMM(this.A3SizeF.Height);
        }
      }
      else
      {
        float mm1 = (float) this.tboxPageWidth.Value;
        double mm2 = this.tboxPageHeight.Value;
        pixValueFromMm1 = InsertBlankPageDialog.GetPixValueFromMM(mm1);
        pixValueFromMm2 = InsertBlankPageDialog.GetPixValueFromMM((float) mm2);
      }
      this.PageSize = new SizeF(pixValueFromMm1, pixValueFromMm2);
      isChecked = this.BeginingRadioButton.IsChecked;
      if (isChecked.GetValueOrDefault())
      {
        this.InsertPageIndex = 0;
        this.InsertBefore = true;
      }
      else
      {
        isChecked = this.EndRadioButton.IsChecked;
        if (isChecked.GetValueOrDefault())
        {
          this.InsertPageIndex = this.doc.Pages.Count - 1;
          this.InsertBefore = false;
        }
      }
      isChecked = this.PageRadioButton.IsChecked;
      if (isChecked.GetValueOrDefault())
      {
        this.InsertBefore = this.InsertPosition.SelectedIndex != 0;
        this.InsertPageIndex = this.GetInsertPageIndex(this.InsertBefore);
        if (this.InsertPageIndex == -1)
        {
          int num = (int) CommomLib.Commom.ModernMessageBox.Show(pdfeditor.Properties.Resources.LinkPageError, UtilManager.GetProductName());
          return;
        }
      }
      this.DialogResult = new bool?(true);
      this.Close();
    }
    catch
    {
      this.DialogResult = new bool?(false);
    }
  }

  private int GetInsertPageIndex(bool InsertBefore)
  {
    int count = this.doc.Pages.Count;
    int[] importPageRange = this.GetImportPageRange();
    if (importPageRange == null)
      return -1;
    int val2_1 = int.MaxValue;
    int val2_2 = int.MinValue;
    for (int index = 0; index < importPageRange.Length; ++index)
    {
      if (importPageRange[index] >= 0 && importPageRange[index] < count)
      {
        val2_1 = Math.Min(importPageRange[index], val2_1);
        val2_2 = Math.Max(importPageRange[index], val2_2);
      }
    }
    if (!InsertBefore)
    {
      if (val2_2 != int.MinValue && val2_2 <= count - 1)
        return val2_2;
    }
    else if (val2_1 != int.MaxValue && val2_1 >= 0)
      return val2_1;
    return this.InsertPageIndex;
  }

  private int[] GetImportPageRange()
  {
    int[] source = (int[]) null;
    if (string.IsNullOrEmpty(this.PageindexNumbox.Text))
      return (int[]) null;
    int[] pageIndexes;
    PdfObjectExtensions.TryParsePageRange(this.PageindexNumbox.Text, out pageIndexes, out int _);
    if (pageIndexes == null)
      return (int[]) null;
    if (pageIndexes.Length != 0)
      source = pageIndexes;
    if (((IEnumerable<int>) source).Any<int>((Func<int, bool>) (c => c < 0 || c >= this.doc.Pages.Count)))
      return (int[]) null;
    return source.Length == 0 ? (int[]) null : source;
  }

  private void CancelButton_Click(object sender, RoutedEventArgs e) => this.Close();

  private void PortraitRadioButton_Checked(object sender, RoutedEventArgs e)
  {
  }

  private void PageindexNumbox_TextChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
  {
    if (this.doc == null)
      return;
    int num = (int) (e.NewValue - 1.0);
    if (num < 0)
    {
      this.PageindexNumbox.Text = "0";
      num = 0;
    }
    if (num > this.doc.Pages.Count - 1)
    {
      this.PageindexNumbox.Text = $"{this.doc.Pages.Count - 1}";
      num = this.doc.Pages.Count - 1;
    }
    this.InsertPageIndex = num;
  }

  private void CustomTextBox_LostFocus(object sender, RoutedEventArgs e)
  {
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/pageeditor/insertblankpagedialog.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.cbPageSize = (ComboBox) target;
        this.cbPageSize.SelectionChanged += new SelectionChangedEventHandler(this.cbPageSize_SelectionChanged);
        break;
      case 2:
        this.tbPageWidth = (HeaderedContentControl) target;
        break;
      case 3:
        this.tboxPageWidth = (NumberBox) target;
        break;
      case 4:
        this.tbPageHeight = (HeaderedContentControl) target;
        break;
      case 5:
        this.tboxPageHeight = (NumberBox) target;
        break;
      case 6:
        this.PortraitRadioButton = (RadioButton) target;
        break;
      case 7:
        this.LandscapeRadioButton = (RadioButton) target;
        break;
      case 8:
        this.PageCountNumberBox = (NumberBox) target;
        break;
      case 9:
        this.BeginingRadioButton = (RadioButton) target;
        break;
      case 10:
        this.EndRadioButton = (RadioButton) target;
        break;
      case 11:
        this.PageRadioButton = (RadioButton) target;
        break;
      case 12:
        this.PageindexNumbox = (TextBox) target;
        this.PageindexNumbox.LostFocus += new RoutedEventHandler(this.CustomTextBox_LostFocus);
        break;
      case 13:
        this.PageNumber = (TextBlock) target;
        break;
      case 14:
        this.InsertPosition = (ComboBox) target;
        break;
      case 15:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.CancelButton_Click);
        break;
      case 16 /*0x10*/:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.OKButton_Click);
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
