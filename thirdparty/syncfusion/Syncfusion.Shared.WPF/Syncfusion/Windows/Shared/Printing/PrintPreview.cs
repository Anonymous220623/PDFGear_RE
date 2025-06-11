// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.Printing.PrintPreview
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared.Printing;

public class PrintPreview : Control, IDisposable
{
  private bool isWired;
  private bool isdisposed;
  private PrintManager printManager;
  private TextBox Part_TextBox;
  private Slider PartZoomSlider;
  private Button PartZoomInButton;
  private Button PartZoomOutButton;
  private PrintOptionsControl PartPrintOptionsControl;
  private PrintPreviewAreaControl PartPrintPreviewAreaControl;

  public PrintPreview(PrintManager printManager)
  {
    this.DefaultStyleKey = (object) typeof (PrintPreview);
    this.printManager = printManager;
    PrintDialog printDialog = new PrintDialog();
    foreach (PrintQueueOption printer in this.printManager.Printers)
    {
      if (printDialog.PrintQueue == null && this.printManager.Printers.Count > 0 && this.printManager.NeedToChangeDefaultPrinter)
        this.printManager.SelectedPrinter = this.printManager.Printers[0];
      if (printDialog.PrintQueue != null && printDialog.PrintQueue.FullName != null && printDialog.PrintQueue.FullName.Equals(printer.PrintQueue.FullName))
      {
        if (this.printManager.NeedToChangeDefaultPrinter)
          this.printManager.SelectedPrinter = printer;
        printer.IsDefault = true;
      }
      else
        printer.IsDefault = false;
    }
    this.Loaded += new RoutedEventHandler(this.OnPrintPreviewControlLoaded);
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.UnWireEvents();
    this.PartPrintPreviewAreaControl = this.GetTemplateChild("PART_PrintPreviewAreaControl") as PrintPreviewAreaControl;
    this.Part_TextBox = this.GetTemplateChild("PART_TextBox") as TextBox;
    this.PartZoomSlider = this.GetTemplateChild("PART_ZoomSlider") as Slider;
    this.PartZoomInButton = this.GetTemplateChild("PART_MinusZoomButton") as Button;
    this.PartZoomOutButton = this.GetTemplateChild("PART_PlusZoomButton") as Button;
    this.PartPrintOptionsControl = this.GetTemplateChild("PART_PrintOptionsControl") as PrintOptionsControl;
    TextBox partTextBox = this.Part_TextBox;
    System.Windows.Controls.ToolTip toolTip1 = new System.Windows.Controls.ToolTip();
    toolTip1.Content = (object) SharedLocalizationResourceAccessor.Instance.GetString("PrintPreview_CurrentPage_ToolTip");
    System.Windows.Controls.ToolTip toolTip2 = toolTip1;
    partTextBox.ToolTip = (object) toolTip2;
    this.WireEvents();
  }

  private void WireEvents()
  {
    if (this.PartPrintPreviewAreaControl != null)
      this.PartPrintPreviewAreaControl.PrintManager = this.printManager;
    if (this.PartPrintOptionsControl != null)
    {
      this.PartPrintOptionsControl.printManager = this.printManager;
      this.PartPrintOptionsControl.printPreviewAreaControl = this.PartPrintPreviewAreaControl;
      this.PartPrintOptionsControl.printManager.PrintOptionsControl = this.PartPrintOptionsControl;
    }
    if (this.Part_TextBox != null)
    {
      this.Part_TextBox.LostFocus += new RoutedEventHandler(this.OnPartTextBoxLostFocus);
      this.Part_TextBox.KeyDown += new KeyEventHandler(this.Part_TextBox_KeyDown);
      this.Part_TextBox.MouseMove += new MouseEventHandler(this.Part_TextBox_MouseMove);
      this.Part_TextBox.PreviewKeyDown += new KeyEventHandler(this.TextBox_PreviewKeyDown);
      DataObject.AddPastingHandler((DependencyObject) this.Part_TextBox, new DataObjectPastingEventHandler(this.OnPasting));
    }
    if (this.PartZoomInButton != null)
      this.PartZoomInButton.Click += new RoutedEventHandler(this.OnZoomInClicked);
    if (this.PartZoomOutButton != null)
      this.PartZoomOutButton.Click += new RoutedEventHandler(this.OnZoomOutClicked);
    if (this.PartZoomSlider != null)
      this.PartZoomSlider.ValueChanged += new RoutedPropertyChangedEventHandler<double>(this.PartZoomSlider_ValueChanged);
    this.Unloaded += new RoutedEventHandler(this.OnPrintPreviewControlUnloaded);
    this.isWired = true;
  }

  private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
  {
    if (e.Key == Key.Space)
      e.Handled = true;
    this.OnPreviewKeyDown(e);
  }

  private void OnPasting(object sender, DataObjectPastingEventArgs e)
  {
    if (e.DataObject.GetDataPresent(typeof (string)))
    {
      string data = (string) e.DataObject.GetData(typeof (string));
      if (!this.IsNumber(data))
      {
        TextBox textBox = sender as TextBox;
        (textBox.ToolTip as System.Windows.Controls.ToolTip).Content = (object) SharedLocalizationResourceAccessor.Instance.GetString("PrintPreview_UpDown_NonNumericToopTip");
        this.ShowInvalidBorderandToolTip(textBox);
        e.CancelCommand();
      }
      else
      {
        int result;
        if (int.TryParse(this.Part_TextBox.Text.Insert(this.Part_TextBox.CaretIndex, data), out result))
        {
          bool flag = false;
          if (result > this.printManager.PageCount)
          {
            (this.Part_TextBox.ToolTip as System.Windows.Controls.ToolTip).Content = (object) $"{SharedLocalizationResourceAccessor.Instance.GetString("PrintPreview_UpDown_LessThanToolTip")} {(object) this.printManager.PageCount}";
            flag = true;
          }
          else if (result < 1)
          {
            (this.Part_TextBox.ToolTip as System.Windows.Controls.ToolTip).Content = (object) (SharedLocalizationResourceAccessor.Instance.GetString("PrintPreview_UpDown_GreaterThanToolTip") + " 1");
            flag = true;
          }
          if (!flag)
            return;
          this.ShowInvalidBorderandToolTip(this.Part_TextBox);
          e.CancelCommand();
        }
        else
          e.CancelCommand();
      }
    }
    else
      e.CancelCommand();
  }

  private bool IsNumber(string text)
  {
    foreach (char c in text.ToCharArray())
    {
      if (!char.IsDigit(c) && !char.IsControl(c))
        return false;
    }
    return true;
  }

  private void UnWireEvents()
  {
    if (this.Part_TextBox != null)
    {
      this.Part_TextBox.LostFocus -= new RoutedEventHandler(this.OnPartTextBoxLostFocus);
      this.Part_TextBox.KeyDown -= new KeyEventHandler(this.Part_TextBox_KeyDown);
      this.Part_TextBox.MouseMove -= new MouseEventHandler(this.Part_TextBox_MouseMove);
      DataObject.RemovePastingHandler((DependencyObject) this.Part_TextBox, new DataObjectPastingEventHandler(this.OnPasting));
    }
    if (this.PartZoomInButton != null)
      this.PartZoomInButton.Click -= new RoutedEventHandler(this.OnZoomInClicked);
    if (this.PartZoomOutButton != null)
      this.PartZoomOutButton.Click -= new RoutedEventHandler(this.OnZoomOutClicked);
    if (this.PartZoomSlider != null)
      this.PartZoomSlider.ValueChanged -= new RoutedPropertyChangedEventHandler<double>(this.PartZoomSlider_ValueChanged);
    this.isWired = false;
  }

  private void OnPrintPreviewControlLoaded(object sender, RoutedEventArgs e)
  {
    if (this.isWired)
      return;
    this.WireEvents();
  }

  private void OnPrintPreviewControlUnloaded(object sender, RoutedEventArgs e)
  {
    if (this.isWired)
      this.UnWireEvents();
    this.Unloaded -= new RoutedEventHandler(this.OnPrintPreviewControlUnloaded);
  }

  private void Part_TextBox_MouseMove(object sender, MouseEventArgs e)
  {
    TextBox textBox = sender as TextBox;
    if (!textBox.IsMouseOver || !(textBox.ToolTip as System.Windows.Controls.ToolTip).IsOpen || textBox.IsFocused)
      return;
    (this.Part_TextBox.ToolTip as System.Windows.Controls.ToolTip).Content = (object) SharedLocalizationResourceAccessor.Instance.GetString("PrintPreview_CurrentPage_ToolTip");
  }

  private void PartZoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
  {
    if (this.PartZoomInButton != null)
      this.PartZoomInButton.IsEnabled = e.NewValue != 10.0;
    if (this.PartZoomOutButton != null)
      this.PartZoomOutButton.IsEnabled = e.NewValue != 500.0;
    this.ClearInvalidBorderandToolTip(this.Part_TextBox);
  }

  private void Part_TextBox_KeyDown(object sender, KeyEventArgs e)
  {
    TextBox textBox = sender as TextBox;
    System.Windows.Controls.ToolTip toolTip = textBox.ToolTip as System.Windows.Controls.ToolTip;
    if (e.Key == Key.Return)
    {
      int result;
      if (int.TryParse(textBox.Text, out result) && result > 0 && result <= this.printManager.PageCount)
      {
        this.PartPrintPreviewAreaControl.PageIndex = result;
        this.ClearInvalidBorderandToolTip(textBox);
        if (this.PartPrintPreviewAreaControl.PartPrintWindowPanel == null)
          return;
        this.PartPrintPreviewAreaControl.PartPrintWindowPanel.InvalidateArrange();
      }
      else
      {
        if (!textBox.Text.Equals("") && !(textBox.Text == "0"))
          return;
        textBox.Text = Convert.ToString(this.PartPrintPreviewAreaControl.PageIndex);
      }
    }
    else if (e.Key == Key.OemMinus)
    {
      toolTip.Content = (object) SharedLocalizationResourceAccessor.Instance.GetString("PrintPreview_UpDown_NegativeToolTip");
      this.ShowInvalidBorderandToolTip(textBox);
      e.Handled = true;
    }
    else if (!Keyboard.Modifiers.Equals((object) ModifierKeys.None))
      e.Handled = true;
    else if (e.Key >= Key.D0 && e.Key <= Key.D9)
    {
      string str = string.IsNullOrEmpty(textBox.SelectedText) ? textBox.Text : textBox.Text.Remove(textBox.CaretIndex, textBox.SelectedText.Length);
      int result;
      if (int.TryParse(str.Insert(textBox.CaretIndex, Convert.ToString((int) (e.Key - 34))), out result))
      {
        if (result < 1 || result > this.printManager.PageCount)
        {
          if (result > this.printManager.PageCount)
            toolTip.Content = (object) (SharedLocalizationResourceAccessor.Instance.GetString("PrintPreview_UpDown_LessThanToolTip") + (object) this.printManager.PageCount);
          else if (result < 1)
            toolTip.Content = (object) (SharedLocalizationResourceAccessor.Instance.GetString("PrintPreview_UpDown_GreaterThanToolTip") + (object) this.printManager.PageCount);
          this.ShowInvalidBorderandToolTip(textBox);
          e.Handled = true;
        }
        else
        {
          int int32 = Convert.ToInt32(str.Trim() + Convert.ToString((int) (e.Key - 34)));
          if (int32 <= 0 || int32 > this.printManager.PageCount)
            return;
          this.ClearInvalidBorderandToolTip(textBox);
        }
      }
      else
        e.Handled = true;
    }
    else
    {
      if (this.IsNumberOrControlKey(e.Key))
        return;
      toolTip.Content = (object) SharedLocalizationResourceAccessor.Instance.GetString("PrintPreview_UpDown_NegativeToolTip");
      textBox.Text = Convert.ToString(this.PartPrintPreviewAreaControl.PageIndex);
      this.ShowInvalidBorderandToolTip(textBox);
      e.Handled = true;
    }
  }

  private bool IsNumberOrControlKey(Key inKey)
  {
    return inKey == Key.Delete || inKey == Key.Back || inKey == Key.Tab || inKey == Key.Return || Keyboard.Modifiers.HasFlag((Enum) ModifierKeys.Alt) || Keyboard.Modifiers.HasFlag((Enum) ModifierKeys.Control) || inKey >= Key.D0 && inKey <= Key.D9 || inKey >= Key.NumPad0 && inKey <= Key.NumPad9;
  }

  private void ShowInvalidBorderandToolTip(TextBox textBox)
  {
    textBox.BorderBrush = (Brush) Brushes.Red;
    if (!(textBox.ToolTip is System.Windows.Controls.ToolTip toolTip))
      return;
    toolTip.Placement = PlacementMode.Bottom;
    toolTip.PlacementTarget = (UIElement) textBox;
    if (toolTip.IsOpen)
      return;
    toolTip.IsOpen = true;
  }

  private void ClearInvalidBorderandToolTip(TextBox textBox)
  {
    if (textBox.ToolTip is System.Windows.Controls.ToolTip toolTip && toolTip.IsOpen)
      toolTip.IsOpen = false;
    if (textBox.BorderBrush != Brushes.Red)
      return;
    textBox.ClearValue(Border.BorderBrushProperty);
  }

  private void OnPartTextBoxLostFocus(object sender, RoutedEventArgs e)
  {
    TextBox textBox = sender as TextBox;
    int result;
    if (int.TryParse(textBox.Text, out result) && result > 0 && result <= this.printManager.PageCount)
    {
      this.PartPrintPreviewAreaControl.PageIndex = result;
      this.ClearInvalidBorderandToolTip(textBox);
    }
    else if (textBox.Text.Equals("") || textBox.Text == "0")
    {
      textBox.Text = this.PartPrintPreviewAreaControl.PageIndex.ToString();
      this.ClearInvalidBorderandToolTip(textBox);
    }
    else
      textBox.ClearValue(TextBox.TextProperty);
  }

  private void OnZoomInClicked(object obj, RoutedEventArgs routedEventArgs)
  {
    if (this.PartZoomSlider.Value - 10.0 < this.PartZoomSlider.Minimum)
      this.PartZoomSlider.Value = this.PartZoomSlider.Minimum;
    else
      this.PartZoomSlider.Value -= 10.0;
  }

  private void OnZoomOutClicked(object obj, RoutedEventArgs routedEventArgs)
  {
    if (this.PartZoomSlider.Value + 10.0 > this.PartZoomSlider.MaxHeight)
      this.PartZoomSlider.Value = this.PartZoomSlider.MaxHeight;
    else
      this.PartZoomSlider.Value += 10.0;
  }

  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }

  protected virtual void Dispose(bool isDisposing)
  {
    if (this.isdisposed)
      return;
    if (isDisposing)
    {
      this.printManager = (PrintManager) null;
      if (this.PartPrintPreviewAreaControl != null)
        this.PartPrintPreviewAreaControl.Dispose();
      this.PartPrintPreviewAreaControl = (PrintPreviewAreaControl) null;
      if (this.PartPrintOptionsControl != null)
        this.PartPrintOptionsControl = (PrintOptionsControl) null;
      this.Part_TextBox = (TextBox) null;
      this.PartZoomSlider = (Slider) null;
      this.PartZoomOutButton = (Button) null;
      this.PartZoomInButton = (Button) null;
    }
    this.isdisposed = true;
  }
}
