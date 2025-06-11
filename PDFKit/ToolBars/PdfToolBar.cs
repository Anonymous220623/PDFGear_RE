// Decompiled with JetBrains decompiler
// Type: PDFKit.ToolBars.PdfToolBar
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace PDFKit.ToolBars;

public class PdfToolBar : ToolBar
{
  private PdfViewer _pdfViewer = (PdfViewer) null;
  public static readonly DependencyProperty PdfViewerProperty = DependencyProperty.Register(nameof (PdfViewer), typeof (PdfViewer), typeof (PdfToolBar), new PropertyMetadata((object) null, (PropertyChangedCallback) ((o, e) =>
  {
    bool isInDesignMode = DesignerProperties.GetIsInDesignMode((DependencyObject) (o as PdfToolBar));
    if (e.OldValue == e.NewValue || isInDesignMode)
      return;
    (o as PdfToolBar).OnPdfViewerChanging(e.OldValue as PdfViewer, e.NewValue as PdfViewer);
  })));

  public PdfViewer PdfViewer
  {
    get => (PdfViewer) this.GetValue(PdfToolBar.PdfViewerProperty);
    set => this.SetValue(PdfToolBar.PdfViewerProperty, (object) value);
  }

  public PdfToolBar()
  {
    this.InitializeButtons();
    this.UpdateButtons();
  }

  protected virtual void InitializeButtons()
  {
  }

  protected virtual void UpdateButtons()
  {
  }

  protected virtual void OnPdfViewerChanging(PdfViewer oldValue, PdfViewer newValue)
  {
    this._pdfViewer = newValue;
    this.UpdateButtons();
  }

  protected virtual Uri CreateUriToResource(string resName)
  {
    return new Uri("pack://application:,,,/PDFKit;component/Resources/" + resName, UriKind.Absolute);
  }

  protected virtual Button CreateButton(
    string name,
    string text,
    string toolTipText,
    Uri imgRes,
    RoutedEventHandler onClick,
    int imgWidth = 32 /*0x20*/,
    int imgHeight = 32 /*0x20*/,
    PdfToolBar.ImageTextType imageTextType = PdfToolBar.ImageTextType.ImageBeforeText)
  {
    Button button1 = new Button();
    button1.Name = name;
    Button button2 = button1;
    System.Windows.Controls.ToolTip toolTip = new System.Windows.Controls.ToolTip();
    toolTip.Content = (object) toolTipText;
    button2.ToolTip = (object) toolTip;
    StackPanel stackPanel = new StackPanel();
    Image element1 = (Image) null;
    TextBlock element2 = (TextBlock) null;
    if (imgRes != (Uri) null)
    {
      Image image = new Image();
      image.Source = (ImageSource) new BitmapImage(imgRes);
      image.Stretch = Stretch.Fill;
      image.Width = (double) imgWidth;
      image.Height = (double) imgHeight;
      element1 = image;
    }
    if (text != null)
      element2 = new TextBlock()
      {
        Text = text,
        TextAlignment = TextAlignment.Center
      };
    button1.Content = (object) stackPanel;
    button1.Click += onClick;
    button1.Padding = new Thickness(7.0, 2.0, 7.0, 2.0);
    if (imageTextType == PdfToolBar.ImageTextType.ImageBeforeText || imageTextType == PdfToolBar.ImageTextType.ImageOnly)
      stackPanel.Children.Add((UIElement) element1);
    if (imageTextType == PdfToolBar.ImageTextType.ImageBeforeText || imageTextType == PdfToolBar.ImageTextType.TextOnly)
      stackPanel.Children.Add((UIElement) element2);
    return button1;
  }

  protected virtual ToggleButton CreateToggleButton(
    string name,
    string text,
    string toolTipText,
    string imgResName,
    RoutedEventHandler onClick,
    int imgWidth = 32 /*0x20*/,
    int imgHeight = 32 /*0x20*/,
    PdfToolBar.ImageTextType imageTextType = PdfToolBar.ImageTextType.ImageBeforeText)
  {
    ToggleButton toggleButton1 = new ToggleButton();
    toggleButton1.Name = name;
    ToggleButton toggleButton2 = toggleButton1;
    System.Windows.Controls.ToolTip toolTip = new System.Windows.Controls.ToolTip();
    toolTip.Content = (object) toolTipText;
    toggleButton2.ToolTip = (object) toolTip;
    StackPanel stackPanel = new StackPanel();
    Image element1 = (Image) null;
    TextBlock element2 = (TextBlock) null;
    if (imgResName != null)
    {
      Image image = new Image();
      image.Source = (ImageSource) new BitmapImage(new Uri("pack://application:,,,/PDFKit;component/Resources/" + imgResName, UriKind.Absolute));
      image.Stretch = Stretch.Fill;
      image.Width = (double) imgWidth;
      image.Height = (double) imgHeight;
      element1 = image;
    }
    if (text != null)
      element2 = new TextBlock()
      {
        Text = text,
        TextAlignment = TextAlignment.Center
      };
    toggleButton1.Content = (object) stackPanel;
    toggleButton1.Click += onClick;
    toggleButton1.Padding = new Thickness(7.0, 2.0, 7.0, 2.0);
    if (imageTextType == PdfToolBar.ImageTextType.ImageBeforeText || imageTextType == PdfToolBar.ImageTextType.ImageOnly)
      stackPanel.Children.Add((UIElement) element1);
    if (imageTextType == PdfToolBar.ImageTextType.ImageBeforeText || imageTextType == PdfToolBar.ImageTextType.TextOnly)
      stackPanel.Children.Add((UIElement) element2);
    return toggleButton1;
  }

  protected enum ImageTextType
  {
    ImageOnly,
    TextOnly,
    ImageBeforeText,
  }
}
