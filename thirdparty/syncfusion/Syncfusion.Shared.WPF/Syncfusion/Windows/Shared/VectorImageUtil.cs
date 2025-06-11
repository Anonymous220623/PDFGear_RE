// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.VectorImageUtil
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class VectorImageUtil
{
  public static void SaveImage(string fileName, FrameworkElement element)
  {
    BitmapEncoder bitmapEncoder;
    switch (new FileInfo(fileName).Extension.ToLower(CultureInfo.InvariantCulture))
    {
      case ".bmp":
        bitmapEncoder = (BitmapEncoder) new BmpBitmapEncoder();
        break;
      case ".jpg":
      case ".jpeg":
        bitmapEncoder = (BitmapEncoder) new JpegBitmapEncoder();
        break;
      case ".png":
        bitmapEncoder = (BitmapEncoder) new PngBitmapEncoder();
        break;
      case ".gif":
        bitmapEncoder = (BitmapEncoder) new GifBitmapEncoder();
        break;
      case ".tif":
      case ".tiff":
        bitmapEncoder = (BitmapEncoder) new TiffBitmapEncoder();
        break;
      case ".wdp":
        bitmapEncoder = (BitmapEncoder) new WmpBitmapEncoder();
        break;
      default:
        bitmapEncoder = (BitmapEncoder) new BmpBitmapEncoder();
        break;
    }
    if (element == null)
      return;
    RenderTargetBitmap source = new RenderTargetBitmap((int) element.ActualWidth, (int) element.ActualHeight, 96.0, 96.0, PixelFormats.Pbgra32);
    source.Render((Visual) element);
    bitmapEncoder.Frames.Add(BitmapFrame.Create((BitmapSource) source));
    using (Stream stream = (Stream) File.Create(fileName))
    {
      bitmapEncoder.Save(stream);
      stream.Close();
    }
  }

  public static void SaveXAML(string fileName, ImageSource imageContent)
  {
    StringBuilder stringBuilder = new StringBuilder();
    XamlWriter.Save((object) imageContent, new XamlDesignerSerializationManager(XmlWriter.Create(fileName, new XmlWriterSettings()
    {
      Indent = true,
      OmitXmlDeclaration = true
    }))
    {
      XamlWriterMode = XamlWriterMode.Expression
    });
  }

  public static Visual ImportXaml(ImageSource imageSourceContent, FlowDocument document)
  {
    try
    {
      object obj = XamlReader.Load(XmlReader.Create((TextReader) new StringReader(new TextRange(document.ContentStart, document.ContentEnd).Text.Replace("\r\n", ""))));
      switch (obj)
      {
        case DrawingImage _:
          imageSourceContent = (ImageSource) (obj as DrawingImage);
          return (Visual) new Image()
          {
            Source = imageSourceContent
          };
        case UIElement _:
          if (obj is UIElement uiElement)
            return (Visual) uiElement;
          break;
      }
    }
    catch (Exception ex)
    {
    }
    return (Visual) null;
  }
}
