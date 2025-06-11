// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Images.RtfToImage
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Native;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Images;

internal class RtfToImage
{
  private const int EM_FORMATRANGE = 1081;
  private const int EM_DISPLAYBAND = 1075;
  private const int EM_SETEDITSTYLE = 1228;
  private const int SES_EXTENDBACKCOLOR = 4;
  private static readonly Type _type = typeof (RtfToImage);
  private static Rectangle s_virtualRect;

  private RtfToImage()
  {
  }

  public static Image ConvertToImage(string text, float width, float height, PdfImageType type)
  {
    if (text == null)
      throw new ArgumentNullException(nameof (text));
    Image image;
    lock (RtfToImage._type)
    {
      width = (float) Math.Ceiling((double) width);
      height = (float) Math.Ceiling((double) height);
      RtfToImage.s_virtualRect = Rectangle.Empty;
      RichTextBox richTextBox = new RichTextBox();
      richTextBox.AutoSize = true;
      richTextBox.AcceptsTab = true;
      richTextBox.DetectUrls = false;
      richTextBox.BorderStyle = BorderStyle.FixedSingle;
      richTextBox.BackColor = Color.FromArgb((int) byte.MaxValue, Color.White);
      richTextBox.Rtf = string.Empty;
      richTextBox.ScrollBars = RichTextBoxScrollBars.None;
      richTextBox.ContentsResized += new ContentsResizedEventHandler(RtfToImage.ContentsResized);
      richTextBox.Width = (int) width;
      RtfToImage.SetText(richTextBox, text);
      if (RtfToImage.s_virtualRect.IsEmpty)
      {
        RtfToImage.SetText(richTextBox, string.Empty);
        RtfToImage.SetText(richTextBox, text);
      }
      width = (double) width == 0.0 ? 1f : width;
      height = (double) height <= 0.0 ? (float) RtfToImage.s_virtualRect.Height : height;
      GdiApi.SendMessage(richTextBox.Handle, 1228, new IntPtr(4), new IntPtr(4));
      image = RtfToImage.ToImage(richTextBox, width, height, type);
      richTextBox.Dispose();
    }
    return image;
  }

  private static Image ToImage(
    RichTextBox richTextBox,
    float width,
    float height,
    PdfImageType type)
  {
    if (richTextBox == null)
      throw new ArgumentNullException(nameof (richTextBox));
    return type != PdfImageType.Metafile ? RtfToImage.ConvertToBitmap(richTextBox, width, height) : RtfToImage.ConvertToMetafile(richTextBox, width, height);
  }

  private static Image ConvertToMetafile(RichTextBox richTextBox, float width, float height)
  {
    if (richTextBox == null)
      throw new ArgumentNullException(nameof (richTextBox));
    MemoryStream memoryStream = new MemoryStream();
    IntPtr dc = GdiApi.CreateDC("DISPLAY", (string) null, (string) null, IntPtr.Zero);
    RectangleF rectangleF = new RectangleF(0.0f, 0.0f, width, height);
    Metafile metafile = new Metafile((Stream) memoryStream, dc, rectangleF, MetafileFrameUnit.Pixel, EmfType.EmfOnly);
    System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage((Image) metafile);
    graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
    IntPtr hdc = graphics.GetHdc();
    RtfToImage.DrawRtf(richTextBox, hdc, rectangleF);
    graphics.ReleaseHdc(hdc);
    graphics.Dispose();
    GdiApi.DeleteDC(dc);
    memoryStream.Close();
    return (Image) metafile;
  }

  internal static Image ConvertToMetafile(RichTextBoxExt richTextBox, float width, float height)
  {
    if (richTextBox == null)
      throw new ArgumentNullException(nameof (richTextBox));
    MemoryStream memoryStream = new MemoryStream();
    IntPtr dc = GdiApi.CreateDC("DISPLAY", (string) null, (string) null, IntPtr.Zero);
    richTextBox.RightMargin = (int) width - 5;
    RectangleF rectangleF = new RectangleF(0.0f, 0.0f, width, height);
    Metafile metafile = new Metafile((Stream) memoryStream, dc, rectangleF, MetafileFrameUnit.Pixel, EmfType.EmfOnly);
    System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage((Image) metafile);
    graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
    IntPtr hdc = graphics.GetHdc();
    RtfToImage.DrawRtf((RichTextBox) richTextBox, hdc, rectangleF);
    graphics.ReleaseHdc(hdc);
    graphics.Dispose();
    GdiApi.DeleteDC(dc);
    memoryStream.Close();
    return (Image) metafile;
  }

  private static Image ConvertToBitmap(RichTextBox richTextBox, float width, float height)
  {
    if (richTextBox == null)
      throw new ArgumentNullException(nameof (richTextBox));
    RectangleF rect = new RectangleF(0.0f, 0.0f, width, height);
    Bitmap bitmap = new Bitmap((int) width, (int) height);
    System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage((Image) bitmap);
    graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
    IntPtr hdc = graphics.GetHdc();
    RtfToImage.DrawRtf(richTextBox, hdc, rect);
    graphics.ReleaseHdc(hdc);
    graphics.Dispose();
    return (Image) bitmap;
  }

  private static void DrawRtf(RichTextBox richTextBox, IntPtr hdc, RectangleF rect)
  {
    if (richTextBox == null)
      throw new ArgumentNullException(nameof (richTextBox));
    CHARRANGE charrange;
    charrange.cpMin = 0;
    charrange.cpMax = richTextBox.TextLength;
    float deviceCaps1 = (float) GdiApi.GetDeviceCaps(hdc, 88);
    float deviceCaps2 = (float) GdiApi.GetDeviceCaps(hdc, 90);
    RectangleF inches = RtfToImage.ConvertPixelsToInches(rect, deviceCaps1, deviceCaps2);
    RECT rect1;
    rect1.left = RtfToImage.ConvertInchesToTwips(inches.Left);
    rect1.top = RtfToImage.ConvertInchesToTwips(inches.Top);
    rect1.right = RtfToImage.ConvertInchesToTwips(inches.Width);
    rect1.bottom = RtfToImage.ConvertInchesToTwips(inches.Height);
    FORMATRANGE structure1 = new FORMATRANGE();
    structure1.chrg = charrange;
    structure1.hdc = hdc;
    structure1.hdcTarget = hdc;
    structure1.rc = rect1;
    structure1.rcPage = rect1;
    IntPtr wParam = new IntPtr(0);
    IntPtr lParam = new IntPtr(0);
    GdiApi.SendMessage(richTextBox.Handle, 1081, wParam, lParam);
    IntPtr num1 = Marshal.AllocCoTaskMem(Marshal.SizeOf<FORMATRANGE>(structure1));
    Marshal.StructureToPtr<FORMATRANGE>(structure1, num1, false);
    GdiApi.SendMessage(richTextBox.Handle, 1081, wParam, num1);
    Marshal.FreeCoTaskMem(num1);
    RECT structure2 = new RECT(rect1.left, rect1.top, rect1.right, rect1.bottom);
    IntPtr num2 = Marshal.AllocCoTaskMem(Marshal.SizeOf<RECT>(structure2));
    Marshal.StructureToPtr<RECT>(structure2, num2, false);
    GdiApi.SendMessage(richTextBox.Handle, 1075, new IntPtr(0), num2);
    Marshal.FreeCoTaskMem(num2);
    wParam = new IntPtr(0);
    num2 = new IntPtr(0);
    GdiApi.SendMessage(richTextBox.Handle, 1081, wParam, num2);
  }

  private static int ConvertInchesToTwips(int n) => (int) ((double) n * 1440.0);

  private static int ConvertInchesToTwips(float f) => (int) ((double) f * 1440.0);

  private static RectangleF ConvertPixelsToInches(RectangleF rcpixels, float dpix, float dpiy)
  {
    return new RectangleF(rcpixels.X / dpix, rcpixels.Y / dpiy, rcpixels.Width / dpix, rcpixels.Height / dpiy);
  }

  public static bool IsValidRtf(string rtf)
  {
    rtf = rtf.Trim(char.MinValue, '\r', '\n', ' ', '\t');
    return rtf.StartsWith("{\\rtf") && rtf.EndsWith("}");
  }

  private static void SetText(RichTextBox richTextBox, string text)
  {
    if (richTextBox == null)
      throw new ArgumentNullException(nameof (richTextBox));
    if (text == null)
      throw new ArgumentNullException(nameof (text));
    if (RtfToImage.IsValidRtf(text))
      richTextBox.Rtf = text;
    else
      richTextBox.Text = text;
  }

  private static void ContentsResized(object sender, ContentsResizedEventArgs e)
  {
    lock (RtfToImage._type)
      RtfToImage.s_virtualRect = e.NewRectangle;
  }
}
