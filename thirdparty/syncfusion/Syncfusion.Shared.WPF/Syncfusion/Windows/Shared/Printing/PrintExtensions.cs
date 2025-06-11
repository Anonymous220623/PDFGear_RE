// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.Printing.PrintExtensions
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Windows.Shared.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

#nullable disable
namespace Syncfusion.Windows.Shared.Printing;

public static class PrintExtensions
{
  internal static readonly double CmConst = 4800.0 / (double) sbyte.MaxValue;

  public static Window GetParentWindow(DependencyObject child)
  {
    Window visualParent = VisualUtils.FindVisualParent<Window>(child);
    if (visualParent != null)
      return visualParent;
    return Application.Current != null ? Application.Current.MainWindow : (Window) null;
  }

  internal static double PixelToCm(double value) => value / PrintExtensions.CmConst;

  internal static double CmToPixel(double value) => value * PrintExtensions.CmConst;

  internal static Size PixelToCm(Size size)
  {
    return new Size(PrintExtensions.PixelToCm(size.Width), PrintExtensions.PixelToCm(size.Height));
  }

  internal static Size CmToPixel(Size size)
  {
    return new Size(PrintExtensions.CmToPixel(size.Width), PrintExtensions.CmToPixel(size.Height));
  }

  internal static Thickness CmToPixel(Thickness thickness)
  {
    return new Thickness(PrintExtensions.CmToPixel(thickness.Left), PrintExtensions.CmToPixel(thickness.Top), PrintExtensions.CmToPixel(thickness.Right), PrintExtensions.CmToPixel(thickness.Bottom));
  }

  internal static Thickness PixelToCm(Thickness thickness)
  {
    return new Thickness(PrintExtensions.PixelToCm(thickness.Left), PrintExtensions.PixelToCm(thickness.Top), PrintExtensions.PixelToCm(thickness.Right), PrintExtensions.PixelToCm(thickness.Bottom));
  }

  internal static List<PrintPageOrientation> GetOrientationList(ResourceWrapper resources)
  {
    return new List<PrintPageOrientation>()
    {
      new PrintPageOrientation()
      {
        Orientation = PrintOrientation.Portrait,
        OrientationName = SharedLocalizationResourceAccessor.Instance.GetString("Print_Orientation_Portrait"),
        ImagePath = "M12,1.7277191 L12,4 14.382444,4 z M1.5,0.99999994 C1.224,1 1,1.225 1,1.5 L1,18.5 C1,18.775 1.224,19 1.5,19 L14.5,19 C14.776,19 15,18.775 15,18.5 L15,5 11,5 11,0.99999994 z M1.5,0 L11.638,0 16,4.161 16,18.5 C16,19.327 15.327,20 14.5,20 L1.5,20 C0.67300034,20 0,19.327 0,18.5 L0,1.5 C0,0.67299999 0.67300034,0 1.5,0 z"
      },
      new PrintPageOrientation()
      {
        Orientation = PrintOrientation.Landscape,
        OrientationName = SharedLocalizationResourceAccessor.Instance.GetString("Print_Orientation_Landscape"),
        ImagePath = "M14,1.7070001 L14,4.0000001 16.293,4.0000001 z M1,1.0000002 L1,13 17,13 17,5.0000001 13,5.0000001 13,1.0000002 z M0,0 L13.707,0 18,4.2929999 18,14 0,14 z"
      }
    };
  }

  internal static List<PrintPageCollation> GetCollationList(ResourceWrapper resources)
  {
    return new List<PrintPageCollation>()
    {
      new PrintPageCollation()
      {
        Collation = Collation.Collated,
        CollationName = SharedLocalizationResourceAccessor.Instance.GetString("PrintPreview_Collated"),
        ImagePath = "M9.9999999,0.99999991 L9.9999999,11 17,11 17,0.99999991 z M1,0.99999991 L1,11 8.0000001,11 8.0000001,0.99999991 z M0,0 L9,0 18,0 18,12 9,12 0,12 z"
      },
      new PrintPageCollation()
      {
        Collation = Collation.Uncollated,
        CollationName = SharedLocalizationResourceAccessor.Instance.GetString("PrintPreview_UnCollated"),
        ImagePath = "M5,7 L5,11 5,12 5,17 13,17 13,12 13,11 13,7 10,7 8,7 z M10,0.99999994 L10,6 14,6 14,11 17,11 17,0.99999994 z M0.99999994,0.99999994 L0.99999994,11 4,11 4,6 8,6 8,0.99999994 z M0,0 L9,0 18,0 18,12 14,12 14,18 4,18 4,12 0,12 z"
      }
    };
  }

  internal static List<PrintPageRangeSelection> GetPageRangeSelectionList(ResourceWrapper resources)
  {
    return new List<PrintPageRangeSelection>()
    {
      new PrintPageRangeSelection()
      {
        PageRangeSelection = PageRangeSelection.AllPages,
        PageRangeSelectionName = SharedLocalizationResourceAccessor.Instance.GetString("PrintPreview_AllPages"),
        ImagePath = "M10,3.7070007 L10,6 12.292999,6 z M3,3 L3,15 13,15 13,7 9,7 9,3 z M2,2 L9.7070007,2 14,6.2929993 14,16 2,16 z M0,0 L8,0 8,1 1,1 1,13.5 0,13.5 z"
      },
      new PrintPageRangeSelection()
      {
        PageRangeSelection = PageRangeSelection.UserPages,
        PageRangeSelectionName = SharedLocalizationResourceAccessor.Instance.GetString("PrintPreview_UserPages"),
        ImagePath = "M8,6.7069998 L8,8 9.2930002,8 z M1,6 L1,17 10,17 10,9 7.0000001,9 7.0000001,6 z M0,5 L7.7070001,5 11,8.2930002 11,18 0,18 z M15,1.7070007 L15,3 16.292999,3 z M6.9999999,0 L14.707001,0 18,3.2929993 18,13 12,13 12,12 17,12 17,4 14,4 14,0.99999994 8,0.99999994 8,4 6.9999999,4 z"
      }
    };
  }

  internal static List<PrintPageMargin> GetMarginList(PrintOptionsControl optionsControl)
  {
    return new List<PrintPageMargin>()
    {
      new PrintPageMargin()
      {
        MarginName = SharedLocalizationResourceAccessor.Instance.GetString("Print_PageMargin_Normal"),
        ImagePath = "M12,16 L12,19 15,19 15,16 z M5.0000001,16 L5.0000001,19 11,19 11,16 z M1.0000002,16 L1.0000002,19 4.0000001,19 4.0000001,16 z M12,5 L12,15 15,15 15,5 z M5.0000001,5 L5.0000001,15 11,15 11,5 z M1.0000002,5 L1.0000002,15 4.0000001,15 4.0000001,5 z M12,1 L12,4 15,4 15,1 z M5.0000001,1 L5.0000001,4 11,4 11,1 z M1.0000002,1 L1.0000002,4 4.0000001,4 4.0000001,1 z M0,0 L16,0 16,4 16,5 16,15 16,16 16,20 0,20 z",
        Thickness = optionsControl.DefaultPageMargin
      },
      new PrintPageMargin()
      {
        MarginName = SharedLocalizationResourceAccessor.Instance.GetString("Print_PageMargin_Narrow"),
        ImagePath = "M14,18 L14,19 15,19 15,18 z M3.0000001,18 L3.0000001,19 13,19 13,18 z M1.0000002,18 L1.0000002,19 1.9999999,19 1.9999999,18 z M14,3.0000001 L14,17 15,17 15,3.0000001 z M3.0000001,3.0000001 L3.0000001,17 13,17 13,3.0000001 z M1.0000002,3.0000001 L1.0000002,17 1.9999999,17 1.9999999,3.0000001 z M14,1 L14,1.9999999 15,1.9999999 15,1 z M3.0000001,1 L3.0000001,1.9999999 13,1.9999999 13,1 z M1.0000002,1 L1.0000002,1.9999999 1.9999999,1.9999999 1.9999999,1 z M0,0 L16,0 16,1.9999999 16,3.0000001 16,17 16,18 16,20 14,20 13,20 3.0000001,20 1.9999999,20 0,20 z",
        Thickness = new Thickness(1.27, 1.27, 1.27, 1.27)
      },
      new PrintPageMargin()
      {
        MarginName = SharedLocalizationResourceAccessor.Instance.GetString("Print_PageMargin_Moderate"),
        ImagePath = "M13,17 L13,19 15,19 15,17 z M4.0000001,17 L4.0000001,19 12,19 12,17 z M1.0000002,17 L1.0000002,19 3.0000001,19 3.0000001,17 z M13,4 L13,16 15,16 15,4 z M4.0000001,4 L4.0000001,16 12,16 12,4 z M1.0000002,4 L1.0000002,16 3.0000001,16 3.0000001,4 z M13,1 L13,2.9999998 15,2.9999998 15,1 z M4.0000001,1 L4.0000001,2.9999998 12,2.9999998 12,1 z M1.0000002,1 L1.0000002,2.9999998 3.0000001,2.9999998 3.0000001,1 z M0,0 L16,0 16,2.9999998 16,4 16,16 16,17 16,20 0,20 z",
        Thickness = new Thickness(1.91, 2.54, 1.91, 2.54)
      },
      new PrintPageMargin()
      {
        MarginName = SharedLocalizationResourceAccessor.Instance.GetString("Print_PageMargin_Wide"),
        ImagePath = "M12,16 L12,19 15,19 15,16 z M5.0000002,16 L5.0000002,19 11,19 11,16 z M1.0000002,16 L1.0000002,19 4.0000002,19 4.0000002,16 z M12,5 L12,15 15,15 15,5 z M5.0000002,5 L5.0000002,15 11,15 11,5 z M1.0000002,5 L1.0000002,15 4.0000002,15 4.0000002,5 z M12,0.99999994 L12,4 15,4 15,0.99999994 z M5.0000002,0.99999994 L5.0000002,4 11,4 11,0.99999994 z M1.0000002,0.99999994 L1.0000002,4 4.0000002,4 4.0000002,0.99999994 z M0,0 L16,0 16,4 16,5 16,15 16,16 16,20 0,20 z",
        Thickness = new Thickness(5.08, 2.54, 5.08, 2.54)
      },
      new PrintPageMargin()
      {
        MarginName = SharedLocalizationResourceAccessor.Instance.GetString("Print_PageMargin_CustomMargin"),
        ImagePath = "F1M355.128,227.697L376.944,227.697L376.944,198.697L355.128,198.697z M377.945,228.697L354.129,228.697L354.129,197.697L377.945,197.697z",
        Thickness = optionsControl.DefaultPageMargin
      }
    };
  }

  internal static void SelectDefaultPageMediaSize(this PrintManager printManager)
  {
    try
    {
      XmlDocument xml = new XmlDocument();
      MemoryStream xmlStream = printManager.dialog.PrintTicket.GetXmlStream();
      xml.Load((Stream) xmlStream);
      Tuple<string, int, int> mediaSizeWithName = printManager.FindNearestPageMediaSizeWithName(xml, true);
      if (mediaSizeWithName == null)
        return;
      PrintPageSize printPageSize = (PrintPageSize) null;
      foreach (KeyValuePair<PrintPageSize, Tuple<string, int, int>> queueCapablitySiz in printManager.printQueueCapablitySizes)
      {
        Tuple<string, int, int> tuple = queueCapablitySiz.Value;
        if (Math.Abs(mediaSizeWithName.Item2 - tuple.Item2) <= 1 && Math.Abs(mediaSizeWithName.Item3 - tuple.Item3) <= 1)
        {
          printPageSize = queueCapablitySiz.Key;
          break;
        }
      }
      if (printManager.PrintOptionsControl == null || printPageSize == null)
        return;
      printManager.PrintOptionsControl.SelectedPageSize = printPageSize;
      printManager.SelectedPageMediaName = printPageSize.PageSizeName;
    }
    catch
    {
    }
  }

  internal static Dictionary<PrintPageSize, Tuple<string, int, int>> GetPrinterSupportedPaperSizes(
    this PrintManager printManager,
    out List<PrintPageSize> pageSizeOptions,
    PageSizeUnit unit)
  {
    XmlDocument xml = new XmlDocument();
    try
    {
      if (printManager.dialog != null)
      {
        if (printManager.dialog.PrintQueue != null)
        {
          MemoryStream capabilitiesAsXml = printManager.dialog.PrintQueue.GetPrintCapabilitiesAsXml(printManager.dialog.PrintQueue.UserPrintTicket);
          xml.Load((Stream) capabilitiesAsXml);
        }
      }
    }
    catch
    {
    }
    return PrintExtensions.GetSupportedPageSizes(xml, out pageSizeOptions, unit);
  }

  internal static PrintTicket GetCustomizedPrintTicket(
    this PrintManager printManager,
    Size? customSize,
    bool isPreview)
  {
    XmlDocument xml = new XmlDocument();
    try
    {
      if (customSize.HasValue)
      {
        double width = customSize.Value.Width;
        double height = customSize.Value.Height;
        printManager.dialog.PrintTicket.PageMediaSize = new PageMediaSize(PageMediaSizeName.Unknown, width, height);
      }
      MemoryStream xmlStream = printManager.dialog.PrintTicket.GetXmlStream();
      xml.Load((Stream) xmlStream);
      printManager.UpdateXMLDocumentWithPageMediaSize(ref xml, isPreview);
    }
    catch
    {
    }
    MemoryStream memoryStream = new MemoryStream();
    xml.Save((Stream) memoryStream);
    memoryStream.Flush();
    memoryStream.Position = 0L;
    return new PrintTicket((Stream) memoryStream);
  }

  internal static Tuple<string, int, int> FindNearestPageMediaSizeWithName(
    this PrintManager printManager,
    XmlDocument xml,
    bool isPreview = false)
  {
    Tuple<string, int, int> mediaSizeWithName = new Tuple<string, int, int>(string.Empty, 0, 0);
    Tuple<string, int, int> tuple = (Tuple<string, int, int>) null;
    try
    {
      if (printManager.PrintOptionsControl != null && printManager.PrintOptionsControl.SelectedPageSize != null)
        tuple = printManager.printQueueCapablitySizes[printManager.PrintOptionsControl.SelectedPageSize];
      if (tuple == null || tuple != null && tuple.Item2 == 0 && tuple.Item3 == 0)
      {
        foreach (XmlNode childNode1 in xml.ChildNodes)
        {
          foreach (XmlNode childNode2 in childNode1.ChildNodes)
          {
            if (childNode2.Attributes != null && childNode2.Attributes.Count > 0 && childNode2.Attributes[0].Value != null && childNode2.Attributes[0].Value.Equals("psk:PageMediaSize"))
            {
              foreach (XmlNode childNode3 in childNode2.ChildNodes)
              {
                int result1 = 0;
                int result2 = 0;
                foreach (XmlNode childNode4 in childNode3.ChildNodes)
                {
                  if (childNode4.Attributes != null && childNode4.Attributes.Count > 0 && childNode4.Attributes[0].Value != null && childNode4.Attributes[0].Value.Equals("psk:MediaSizeWidth"))
                  {
                    foreach (XmlNode childNode5 in childNode4.ChildNodes)
                    {
                      foreach (XmlNode childNode6 in childNode5.ChildNodes)
                        int.TryParse(childNode6.Value, out result1);
                    }
                  }
                  else if (childNode4.Attributes != null && childNode4.Attributes.Count > 0 && childNode4.Attributes[0].Value != null && childNode4.Attributes[0].Value.Equals("psk:MediaSizeHeight"))
                  {
                    foreach (XmlNode childNode7 in childNode4.ChildNodes)
                    {
                      foreach (XmlNode childNode8 in childNode7.ChildNodes)
                        int.TryParse(childNode8.Value, out result2);
                    }
                  }
                }
                mediaSizeWithName = printManager.FindNearestPageMediaSize(Math.Min(result1, result2), Math.Max(result1, result2), printManager.PageSizeDisplayUnit, isPreview);
              }
            }
          }
        }
      }
      else if (tuple != null)
      {
        if (tuple.Item2 > 0)
        {
          if (tuple.Item3 > 0)
            mediaSizeWithName = printManager.FindNearestPageMediaSize(Math.Min(tuple.Item2, tuple.Item3), Math.Max(tuple.Item2, tuple.Item3), printManager.PageSizeDisplayUnit, isPreview);
        }
      }
    }
    catch
    {
    }
    return mediaSizeWithName;
  }

  internal static Tuple<string, int, int> FindNearestPageMediaSize(
    this PrintManager printManager,
    int width,
    int height,
    PageSizeUnit units,
    bool isPreview = false)
  {
    Tuple<string, int, int> tuple1 = new Tuple<string, int, int>(string.Empty, 0, 0);
    Tuple<string, int, int> tuple2 = new Tuple<string, int, int>(string.Empty, 0, 0);
    int num1 = int.MaxValue;
    int num2 = int.MaxValue;
    if (!isPreview)
    {
      List<PrintPageSize> pageSizeOptions = new List<PrintPageSize>();
      printManager.printQueueCapablitySizes = printManager.GetPrinterSupportedPaperSizes(out pageSizeOptions, printManager.PageSizeDisplayUnit);
    }
    foreach (KeyValuePair<PrintPageSize, Tuple<string, int, int>> queueCapablitySiz in printManager.printQueueCapablitySizes)
    {
      Tuple<string, int, int> nearestPageMediaSize = queueCapablitySiz.Value;
      if (nearestPageMediaSize.Item2 == width && nearestPageMediaSize.Item3 == height)
        return nearestPageMediaSize;
      if (nearestPageMediaSize.Item2 >= width && nearestPageMediaSize.Item3 >= height)
      {
        int num3 = Math.Abs(nearestPageMediaSize.Item3 - height) + Math.Abs(nearestPageMediaSize.Item2 - width);
        if (num2 > num3)
        {
          num2 = num3;
          tuple2 = nearestPageMediaSize;
        }
      }
      else
      {
        int num4 = Math.Abs(nearestPageMediaSize.Item3 - height) + Math.Abs(nearestPageMediaSize.Item2 - width);
        if (num1 > num4)
        {
          num1 = num4;
          tuple1 = nearestPageMediaSize;
        }
      }
    }
    return (double) (Math.Abs(width - tuple1.Item2) + Math.Abs(height - tuple1.Item3)) < (double) (Math.Abs(width - tuple2.Item2) + Math.Abs(height - tuple2.Item3)) ? (!isPreview || isPreview && (double) Math.Abs(tuple1.Item2 - width) <= 264.584 && (double) Math.Abs(tuple1.Item3 - height) <= 264.584 ? tuple1 : new Tuple<string, int, int>(string.Empty, width, height)) : (!isPreview || isPreview && (double) Math.Abs(tuple2.Item2 - width) <= 264.584 && (double) Math.Abs(tuple2.Item3 - height) <= 264.584 ? tuple2 : new Tuple<string, int, int>(string.Empty, width, height));
  }

  internal static void UpdateXMLDocumentWithPageMediaSize(
    this PrintManager printManager,
    ref XmlDocument xml,
    bool isPreview)
  {
    try
    {
      foreach (XmlNode childNode1 in xml.ChildNodes)
      {
        foreach (XmlNode childNode2 in childNode1.ChildNodes)
        {
          if (childNode2.Attributes != null && childNode2.Attributes.Count > 0 && childNode2.Attributes[0].Value != null && childNode2.Attributes[0].Value.Equals("psk:PageMediaSize"))
          {
            foreach (XmlNode childNode3 in childNode2.ChildNodes)
            {
              int result1 = 0;
              int result2 = 0;
              foreach (XmlNode childNode4 in childNode3.ChildNodes)
              {
                if (childNode4.Attributes != null && childNode4.Attributes.Count > 0 && childNode4.Attributes[0].Value != null && childNode4.Attributes[0].Value.Equals("psk:MediaSizeWidth"))
                {
                  foreach (XmlNode childNode5 in childNode4.ChildNodes)
                  {
                    foreach (XmlNode childNode6 in childNode5.ChildNodes)
                      int.TryParse(childNode6.Value, out result1);
                  }
                }
                else if (childNode4.Attributes != null && childNode4.Attributes.Count > 0 && childNode4.Attributes[0].Value != null && childNode4.Attributes[0].Value.Equals("psk:MediaSizeHeight"))
                {
                  foreach (XmlNode childNode7 in childNode4.ChildNodes)
                  {
                    foreach (XmlNode childNode8 in childNode7.ChildNodes)
                      int.TryParse(childNode8.Value, out result2);
                  }
                }
              }
              Tuple<string, int, int> nearestPageMediaSize = printManager.FindNearestPageMediaSize(result1, result2, printManager.PageSizeDisplayUnit, isPreview);
              if (nearestPageMediaSize != null && (isPreview || !string.IsNullOrEmpty(nearestPageMediaSize.Item1) || string.IsNullOrEmpty(nearestPageMediaSize.Item1) && isPreview))
              {
                if (childNode3.Attributes != null && childNode3.Attributes.Count == 0 && !string.IsNullOrEmpty(nearestPageMediaSize.Item1) && !nearestPageMediaSize.Item1.Contains("Custom_"))
                {
                  XmlAttribute attribute = xml.CreateAttribute("name");
                  attribute.Value = nearestPageMediaSize.Item1;
                  childNode3.Attributes.SetNamedItem((XmlNode) attribute);
                }
                if (childNode3.Attributes != null && childNode3.Attributes.Count > 0 && childNode3.Attributes[0].Name.Equals("name"))
                  childNode3.Attributes[0].Value = nearestPageMediaSize.Item1;
                foreach (XmlNode childNode9 in childNode3.ChildNodes)
                {
                  if (childNode9.Attributes != null && childNode9.Attributes.Count > 0 && childNode9.Attributes[0].Value != null && childNode9.Attributes[0].Value.Equals("psk:MediaSizeWidth"))
                  {
                    foreach (XmlNode childNode10 in childNode9.ChildNodes)
                    {
                      foreach (XmlNode childNode11 in childNode10.ChildNodes)
                        childNode11.Value = nearestPageMediaSize.Item2.ToString();
                    }
                  }
                  else if (childNode9.Attributes != null && childNode9.Attributes.Count > 0 && childNode9.Attributes[0].Value != null && childNode9.Attributes[0].Value.Equals("psk:MediaSizeHeight"))
                  {
                    foreach (XmlNode childNode12 in childNode9.ChildNodes)
                    {
                      foreach (XmlNode childNode13 in childNode12.ChildNodes)
                        childNode13.Value = nearestPageMediaSize.Item3.ToString();
                    }
                  }
                }
              }
            }
          }
        }
      }
    }
    catch
    {
    }
  }

  private static Dictionary<PrintPageSize, Tuple<string, int, int>> GetSupportedPageSizes(
    XmlDocument xml,
    out List<PrintPageSize> pageSizeOptions,
    PageSizeUnit unit)
  {
    Dictionary<PrintPageSize, Tuple<string, int, int>> supportedPageSizes = new Dictionary<PrintPageSize, Tuple<string, int, int>>();
    pageSizeOptions = new List<PrintPageSize>();
    try
    {
      foreach (XmlNode childNode1 in xml.ChildNodes)
      {
        foreach (XmlNode childNode2 in childNode1.ChildNodes)
        {
          if (childNode2.Attributes != null && childNode2.Attributes.Count > 0 && childNode2.Attributes[0].Value != null && childNode2.Attributes[0].Value.Equals("psk:PageMediaSize"))
          {
            foreach (XmlNode childNode3 in childNode2.ChildNodes)
            {
              if (childNode3.Name != null && childNode3.Name.Contains("Option"))
              {
                string empty = string.Empty;
                Tuple<string, int, int> tuple = new Tuple<string, int, int>(childNode3.Attributes == null || childNode3.Attributes.Count <= 0 ? string.Empty : (childNode3.Attributes[0].Value != null ? childNode3.Attributes[0].Value : string.Empty), 0, 0);
                PrintPageSize key = new PrintPageSize();
                foreach (XmlNode childNode4 in childNode3.ChildNodes)
                {
                  if (childNode4.Attributes != null && childNode4.Attributes.Count > 0 && childNode4.Attributes[0].Value != null && childNode4.Attributes[0].Value.Equals("psk:DisplayName"))
                  {
                    foreach (XmlNode childNode5 in childNode4.ChildNodes)
                    {
                      foreach (XmlNode childNode6 in childNode5.ChildNodes)
                      {
                        string str = childNode6.Value;
                        key.PageSizeName = childNode6.Value;
                        key.PageSizeUnit = unit;
                      }
                    }
                  }
                  else if (childNode4.Attributes != null && childNode4.Attributes.Count > 0 && childNode4.Attributes[0].Value != null && childNode4.Attributes[0].Value.Equals("psk:MediaSizeWidth"))
                  {
                    foreach (XmlNode childNode7 in childNode4.ChildNodes)
                    {
                      foreach (XmlNode childNode8 in childNode7.ChildNodes)
                      {
                        int result;
                        if (int.TryParse(childNode8.Value, out result))
                        {
                          tuple = new Tuple<string, int, int>(tuple.Item1, result, tuple.Item3);
                          key.Size = new Size((double) result / 264.584, key.Size.Height);
                        }
                      }
                    }
                  }
                  else if (childNode4.Attributes != null && childNode4.Attributes.Count > 0 && childNode4.Attributes[0].Value != null && childNode4.Attributes[0].Value.Equals("psk:MediaSizeHeight"))
                  {
                    foreach (XmlNode childNode9 in childNode4.ChildNodes)
                    {
                      foreach (XmlNode childNode10 in childNode9.ChildNodes)
                      {
                        int result;
                        if (int.TryParse(childNode10.Value, out result))
                        {
                          tuple = new Tuple<string, int, int>(tuple.Item1, tuple.Item2, result);
                          key.Size = new Size(key.Size.Width, (double) result / 264.584);
                        }
                      }
                    }
                  }
                }
                if (key.Size.Width != 0.0 && key.Size.Height != 0.0)
                {
                  supportedPageSizes.Add(key, tuple);
                  pageSizeOptions.Add(key);
                }
              }
            }
          }
        }
      }
    }
    catch
    {
    }
    return supportedPageSizes;
  }
}
