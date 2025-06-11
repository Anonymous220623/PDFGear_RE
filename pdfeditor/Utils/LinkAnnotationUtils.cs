// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.LinkAnnotationUtils
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Actions;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.Wrappers;
using pdfeditor.Controls;
using pdfeditor.Properties;
using pdfeditor.ViewModels;
using PDFKit.Services;
using PDFKit.Utils;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Utils;

internal static class LinkAnnotationUtils
{
  public static void LinkAnnotationop(
    PdfLinkAnnotation pdfLinkAnnotation,
    PdfDocument pdfDocument,
    PdfPage pdfPage,
    float Doczoom,
    MainViewModel vm)
  {
    if ((PdfWrapper) pdfLinkAnnotation == (PdfWrapper) null)
      return;
    LinkAnnotationModel linkModel = LinkAnnotationUtils.GetLinkModel(pdfLinkAnnotation, pdfDocument);
    if (linkModel == null)
      return;
    LinkEditWindows linkEditWindows = new LinkEditWindows(linkModel);
    linkEditWindows.Owner = App.Current.MainWindow;
    linkEditWindows.WindowStartupLocation = linkEditWindows.Owner != null ? WindowStartupLocation.CenterOwner : WindowStartupLocation.CenterScreen;
    bool? nullable = linkEditWindows.ShowDialog();
    if (pdfPage.Annots == null)
      pdfPage.CreateAnnotations();
    if (!nullable.GetValueOrDefault())
      return;
    using (vm.OperationManager.TraceAnnotationChange(pdfLinkAnnotation.Page))
    {
      if (linkEditWindows.SelectedType == LinkSelect.ToPage)
      {
        int num = linkEditWindows.Page - 1;
        PdfDestination xyz = PdfDestination.CreateXYZ(pdfDocument, num, top: new float?(pdfDocument.Pages[num].Height), zoom: new float?(Doczoom));
        pdfLinkAnnotation.Link.Action = (PdfAction) new PdfGoToAction(pdfDocument, xyz);
      }
      else if (linkEditWindows.SelectedType == LinkSelect.ToWeb)
        pdfLinkAnnotation.Link.Action = (PdfAction) new PdfUriAction(pdfDocument, linkEditWindows.UrlFilePath);
      else if (linkEditWindows.SelectedType == LinkSelect.ToFile)
        pdfLinkAnnotation.Link.Action = (PdfAction) new PdfLaunchAction(pdfDocument, new PdfFileSpecification(pdfDocument)
        {
          FileName = linkEditWindows.FileDiaoligFiePath
        });
      Color color = (Color) ColorConverter.ConvertFromString(linkEditWindows.SelectedFontground);
      FS_COLOR fsColor = new FS_COLOR((int) color.A, (int) color.R, (int) color.G, (int) color.B);
      float num1;
      if (!linkEditWindows.rectangleVis)
      {
        num1 = 0.0f;
      }
      else
      {
        pdfLinkAnnotation.Color = fsColor;
        num1 = linkEditWindows.BorderWidth;
      }
      PdfBorderStyle borderStyle = new PdfBorderStyle()
      {
        Width = num1,
        Style = linkEditWindows.BorderStyles
      };
      pdfLinkAnnotation.SetBorderStyle(borderStyle);
    }
    PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(pdfPage.Document);
    if (pdfControl != null)
    {
      int? pageIndex1 = PdfObjectExtensions.GetAnnotationHolderManager(pdfControl)?.CurrentHolder?.CurrentPage?.PageIndex;
      int pageIndex2 = pdfPage.PageIndex;
      int num = pageIndex1.GetValueOrDefault() == pageIndex2 & pageIndex1.HasValue ? 1 : 0;
    }
    for (int index = 0; index < 3; ++index)
    {
      bool flag = pdfPage.IsDisposed;
      if (!flag)
        flag = PdfDocumentStateService.CanDisposePage(pdfPage);
      ProgressiveStatus progressiveStatus;
      if (!flag && PdfObjectExtensions.TryGetProgressiveStatus(pdfPage, out progressiveStatus))
        flag = progressiveStatus != ProgressiveStatus.ToBeContinued && progressiveStatus != ProgressiveStatus.Failed;
      if (flag)
      {
        try
        {
          PageDisposeHelper.DisposePage(pdfPage);
          PdfDocumentStateService.TryRedrawViewerCurrentPage(pdfPage);
          break;
        }
        catch
        {
          break;
        }
      }
    }
  }

  internal static string GetLinkUrlOrFileName(PdfLink pdfLink)
  {
    if (pdfLink.Action is PdfUriAction action1 && !string.IsNullOrEmpty(action1.Uri))
      return action1.Uri;
    return pdfLink.Action is PdfLaunchAction action2 && !string.IsNullOrEmpty(action2.FileSpecification?.FileName) ? action2.FileSpecification.FileName : (string) null;
  }

  internal static LinkAnnotationModel GetLinkModel(
    PdfLinkAnnotation pdfLink,
    PdfDocument pdfDocument)
  {
    LinkAnnotationModel linkModel = new LinkAnnotationModel();
    linkModel.Title = Resources.LinkWinTitleEdit;
    linkModel.PdfDocument = pdfDocument;
    linkModel.BorderColor = pdfLink.Color.ToColor();
    if (pdfLink.Link.Action is PdfGoToAction action3 && action3.Destination != null)
    {
      linkModel.Action = LinkSelect.ToPage;
      linkModel.Page = action3.Destination.PageIndex + 1;
    }
    else if (pdfLink.Link.Destination != null)
    {
      linkModel.Action = LinkSelect.ToPage;
      linkModel.Page = pdfLink.Link.Destination.PageIndex + 1;
    }
    else if (pdfLink.Link.Action is PdfUriAction action2)
    {
      linkModel.Uri = action2.Uri;
      linkModel.Action = LinkSelect.ToWeb;
    }
    else if (pdfLink.Link.Action is PdfLaunchAction action1)
    {
      linkModel.Action = LinkSelect.ToFile;
      linkModel.FileName = action1.FileSpecification.FileName;
    }
    PdfBorderStyle pdfBorderStyle = new PdfBorderStyle();
    if (pdfLink.Dictionary.ContainsKey("BS"))
    {
      PdfBorderStyle borderStyle = pdfLink.GetBorderStyle();
      linkModel.Width = borderStyle.Width;
      linkModel.BorderStyle = borderStyle.Style;
    }
    else
    {
      linkModel.Width = 1f;
      linkModel.BorderStyle = BorderStyles.Solid;
    }
    return linkModel;
  }

  private static bool CheckUri(string uri)
  {
    return new Regex("^(http(s)?:\\/\\/)?(www\\.)?[\\w-]+(\\.\\w{2,4})?\\.\\w{2,4}?(\\/)?$").Match(uri.Trim()).Success;
  }
}
