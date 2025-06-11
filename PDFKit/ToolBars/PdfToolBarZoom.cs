// Decompiled with JetBrains decompiler
// Type: PDFKit.ToolBars.PdfToolBarZoom
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace PDFKit.ToolBars;

public class PdfToolBarZoom : PdfToolBarZoomEx
{
  private ComboBox CreateZoomCombo()
  {
    ComboBox zoomCombo = new ComboBox();
    zoomCombo.Name = "btnComboBox";
    ComboBox comboBox = zoomCombo;
    System.Windows.Controls.ToolTip toolTip = new System.Windows.Controls.ToolTip();
    toolTip.Content = (object) PDFKit.Properties.Resources.btnZoomComboToolTipText;
    comboBox.ToolTip = (object) toolTip;
    zoomCombo.KeyDown += new KeyEventHandler(this.ComboBox_KeyDown);
    zoomCombo.SelectionChanged += new SelectionChangedEventHandler(this.ComboBox_SelectionChanged);
    zoomCombo.Width = 70.0;
    for (int index = 0; index < this.ZoomLevel.Length; ++index)
      zoomCombo.Items.Add((object) $"{this.ZoomLevel[index]:.00}%");
    return zoomCombo;
  }

  protected override void InitializeButtons()
  {
    this.Items.Add((object) this.CreateButton("btnZoomOut", PDFKit.Properties.Resources.btnZoomOutText, PDFKit.Properties.Resources.btnZoomOutToolTipText, this.CreateUriToResource("zoomOut.png"), new RoutedEventHandler(this.btn_ZoomOutClick)));
    this.Items.Add((object) this.CreateZoomCombo());
    this.Items.Add((object) this.CreateButton("btnZoomIn", PDFKit.Properties.Resources.btnZoomInText, PDFKit.Properties.Resources.btnZoomInToolTipText, this.CreateUriToResource("zoomIn.png"), new RoutedEventHandler(this.btn_ZoomInClick)));
  }

  protected override void UpdateButtons()
  {
    if (this.Items[1] is ComboBox comboBox)
      comboBox.IsEnabled = this.PdfViewer != null && this.PdfViewer.Document != null;
    if (this.Items[0] is Button button1)
    {
      int num = this.PdfViewer == null ? 0 : (this.PdfViewer.Document != null ? 1 : 0);
      button1.IsEnabled = num != 0;
    }
    if (this.Items[2] is Button button2)
    {
      int num = this.PdfViewer == null ? 0 : (this.PdfViewer.Document != null ? 1 : 0);
      button2.IsEnabled = num != 0;
    }
    if (this.PdfViewer == null || this.PdfViewer.Document == null)
      return;
    this.CalcCurrentZoomLevel();
    if (comboBox == null)
      return;
    comboBox.Text = $"{this.Zoom:.00}%";
  }

  private void btn_ZoomOutClick(object sender, EventArgs e)
  {
    this.OnZoomExOutClick(this.Items[0] as Button);
  }

  private void btn_ZoomInClick(object sender, EventArgs e)
  {
    this.OnZoomExInClick(this.Items[2] as Button);
  }

  private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    if ((sender as ComboBox).SelectedIndex < 0)
      return;
    this.OnComboBoxSelectionChanged(sender as ComboBox, (sender as ComboBox).SelectedIndex);
  }

  private void ComboBox_KeyDown(object sender, KeyEventArgs e)
  {
    this.OnComboBoxKeyDown(this.Items[1] as ComboBox, e);
  }

  protected virtual void OnComboBoxSelectionChanged(ComboBox item, int selectedIndex)
  {
    this.SetZoom(selectedIndex);
  }

  protected virtual void OnComboBoxKeyDown(ComboBox item, KeyEventArgs e)
  {
    if (item == null || e.Key != Key.Return)
      return;
    double result = 0.0;
    string s = item.Text.Replace("%", "").Replace(" ", "");
    if (!double.TryParse(s, out result) && !double.TryParse(s.Replace(".", ","), out result) && !double.TryParse(s.Replace(",", "."), out result))
      return;
    if (result < this.ZoomLevel[0])
      result = this.ZoomLevel[0];
    else if (result > this.ZoomLevel[this.ZoomLevel.Length - 1])
      result = this.ZoomLevel[this.ZoomLevel.Length - 1];
    this.SetZoom(result / 100.0);
    item.Text = $"{result:.00}%";
  }
}
