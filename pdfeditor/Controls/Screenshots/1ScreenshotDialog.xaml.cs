// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Screenshots.ScreenshotDialogResult
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf;
using System.Windows;
using System.Windows.Media.Imaging;

#nullable disable
namespace pdfeditor.Controls.Screenshots;

public class ScreenshotDialogResult
{
  private ScreenshotDialogResult()
  {
  }

  public ScreenshotDialogMode Mode { get; private set; }

  public bool Completed { get; private set; }

  public string ExtractedText { get; private set; } = "";

  public FS_RECTF? BeforeRect { get; private set; }

  public FS_RECTF SelectedRect { get; private set; }

  public Rect SelectedClientRect { get; private set; }

  public int PageIndex { get; private set; } = -1;

  public WriteableBitmap Image { get; private set; }

  public WriteableBitmap RotatedImage { get; private set; }

  public System.Drawing.Image OcrImage { get; private set; }

  public int[] ApplyPageIndex { get; private set; }

  public static ScreenshotDialogResult CreateCancel()
  {
    return new ScreenshotDialogResult()
    {
      Completed = false
    };
  }

  public static ScreenshotDialogResult CreateExtractedText(
    int pageIndex,
    string text,
    WriteableBitmap image,
    WriteableBitmap rotatedImage,
    FS_RECTF rect,
    Rect selectedClientRect,
    bool ocr)
  {
    return new ScreenshotDialogResult()
    {
      Completed = image != null,
      Mode = ocr ? ScreenshotDialogMode.Ocr : ScreenshotDialogMode.ExtractText,
      PageIndex = pageIndex,
      Image = image,
      RotatedImage = rotatedImage,
      ExtractedText = text ?? "",
      SelectedRect = rect,
      SelectedClientRect = selectedClientRect
    };
  }

  public static ScreenshotDialogResult CreateExtractImageText(
    int pageIndex,
    string text,
    WriteableBitmap image,
    System.Drawing.Image OcrImage,
    FS_RECTF rect,
    Rect selectedClientRect,
    bool ocr)
  {
    return new ScreenshotDialogResult()
    {
      Completed = image != null,
      Mode = ocr ? ScreenshotDialogMode.Ocr : ScreenshotDialogMode.ExtractText,
      PageIndex = pageIndex,
      Image = image,
      OcrImage = OcrImage,
      ExtractedText = text ?? "",
      SelectedRect = rect,
      SelectedClientRect = selectedClientRect
    };
  }

  public static ScreenshotDialogResult CreateImage(
    int pageIndex,
    WriteableBitmap image,
    WriteableBitmap rotatedImage,
    FS_RECTF rect,
    Rect selectedClientRect)
  {
    return new ScreenshotDialogResult()
    {
      Completed = image != null,
      Mode = ScreenshotDialogMode.Screenshot,
      PageIndex = pageIndex,
      Image = image,
      RotatedImage = rotatedImage,
      SelectedRect = rect,
      SelectedClientRect = selectedClientRect
    };
  }

  public static ScreenshotDialogResult GetCropBox(
    int pageIndex,
    int[] pageIndexs,
    FS_RECTF beforeRect,
    FS_RECTF rect,
    Rect selectedClientRect)
  {
    return new ScreenshotDialogResult()
    {
      Completed = true,
      Mode = ScreenshotDialogMode.CropPage,
      PageIndex = pageIndex,
      SelectedRect = rect,
      BeforeRect = new FS_RECTF?(beforeRect),
      SelectedClientRect = selectedClientRect,
      ApplyPageIndex = pageIndexs
    };
  }
}
