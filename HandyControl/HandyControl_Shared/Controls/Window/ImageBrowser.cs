// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.ImageBrowser
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Properties.Langs;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

#nullable disable
namespace HandyControl.Controls;

[TemplatePart(Name = "PART_PanelTop", Type = typeof (Panel))]
[TemplatePart(Name = "PART_ImageViewer", Type = typeof (ImageViewer))]
public class ImageBrowser : Window
{
  private const string ElementPanelTop = "PART_PanelTop";
  private const string ElementImageViewer = "PART_ImageViewer";
  private Panel _panelTop;
  private ImageViewer _imageViewer;

  static ImageBrowser()
  {
    Window.IsFullScreenProperty.AddOwner(typeof (ImageBrowser), new PropertyMetadata(ValueBoxes.FalseBox));
  }

  public ImageBrowser()
  {
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Close, new ExecutedRoutedEventHandler(this.ButtonClose_OnClick)));
    this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
    this.WindowStyle = WindowStyle.None;
    this.Topmost = true;
    this.AllowsTransparency = true;
  }

  public ImageBrowser(Uri uri)
    : this()
  {
    ImageBrowser imageBrowser = this;
    this.Loaded += (RoutedEventHandler) ((s, e) =>
    {
      try
      {
        imageBrowser._imageViewer.ImageSource = BitmapFrame.Create(uri);
        imageBrowser._imageViewer.ImgPath = uri.AbsolutePath;
        if (!File.Exists(imageBrowser._imageViewer.ImgPath))
          return;
        imageBrowser._imageViewer.ImgSize = new FileInfo(imageBrowser._imageViewer.ImgPath).Length;
      }
      catch
      {
        int num = (int) MessageBox.Show(Lang.ErrorImgPath);
      }
    });
  }

  public ImageBrowser(string path)
    : this(new Uri(path))
  {
  }

  public override void OnApplyTemplate()
  {
    if (this._panelTop != null)
      this._panelTop.MouseLeftButtonDown -= new MouseButtonEventHandler(this.PanelTopOnMouseLeftButtonDown);
    if (this._imageViewer != null)
      this._imageViewer.MouseLeftButtonDown -= new MouseButtonEventHandler(this.ImageViewer_MouseLeftButtonDown);
    base.OnApplyTemplate();
    this._panelTop = this.GetTemplateChild("PART_PanelTop") as Panel;
    this._imageViewer = this.GetTemplateChild("PART_ImageViewer") as ImageViewer;
    if (this._panelTop != null)
      this._panelTop.MouseLeftButtonDown += new MouseButtonEventHandler(this.PanelTopOnMouseLeftButtonDown);
    if (this._imageViewer == null)
      return;
    this._imageViewer.MouseLeftButtonDown += new MouseButtonEventHandler(this.ImageViewer_MouseLeftButtonDown);
  }

  private void ButtonClose_OnClick(object sender, RoutedEventArgs e) => this.Close();

  private void PanelTopOnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    if (e.LeftButton != MouseButtonState.Pressed)
      return;
    this.DragMove();
  }

  private void ImageViewer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    if (e.LeftButton != MouseButtonState.Pressed || this._imageViewer.ImageWidth > this.ActualWidth || this._imageViewer.ImageHeight > this.ActualHeight)
      return;
    this.DragMove();
  }
}
