// Decompiled with JetBrains decompiler
// Type: QRCoder.ArtQRCode
// Assembly: QRCoder, Version=1.4.3.0, Culture=neutral, PublicKeyToken=c4ed5b9ae8358a28
// MVID: 5D0AD632-49D4-4E68-92A0-261964209740
// Assembly location: D:\PDFGear\bin\QRCoder.dll

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

#nullable disable
namespace QRCoder;

public class ArtQRCode : AbstractQRCode, IDisposable
{
  public ArtQRCode()
  {
  }

  public ArtQRCode(QRCodeData data)
    : base(data)
  {
  }

  public Bitmap GetGraphic(int pixelsPerModule)
  {
    return this.GetGraphic(pixelsPerModule, Color.Black, Color.White, Color.Transparent);
  }

  public Bitmap GetGraphic(Bitmap backgroundImage = null)
  {
    return this.GetGraphic(10, Color.Black, Color.White, Color.Transparent, backgroundImage);
  }

  public Bitmap GetGraphic(
    int pixelsPerModule,
    Color darkColor,
    Color lightColor,
    Color backgroundColor,
    Bitmap backgroundImage = null,
    double pixelSizeFactor = 0.8,
    bool drawQuietZones = true,
    ArtQRCode.QuietZoneStyle quietZoneRenderingStyle = ArtQRCode.QuietZoneStyle.Dotted,
    ArtQRCode.BackgroundImageStyle backgroundImageStyle = ArtQRCode.BackgroundImageStyle.DataAreaOnly,
    Bitmap finderPatternImage = null)
  {
    int pixelSize = pixelSizeFactor <= 1.0 ? (int) Math.Min((double) pixelsPerModule, Math.Floor((double) pixelsPerModule / pixelSizeFactor)) : throw new Exception("The parameter pixelSize must be between 0 and 1. (0-100%)");
    int numModules = this.QrCodeData.ModuleMatrix.Count - (drawQuietZones ? 0 : 8);
    int offset = drawQuietZones ? 0 : 4;
    int num1 = numModules * pixelsPerModule;
    Bitmap graphic = new Bitmap(num1, num1);
    using (Graphics graphics = Graphics.FromImage((Image) graphic))
    {
      using (SolidBrush brush1 = new SolidBrush(lightColor))
      {
        using (SolidBrush brush2 = new SolidBrush(darkColor))
        {
          using (SolidBrush solidBrush = new SolidBrush(backgroundColor))
            graphics.FillRectangle((Brush) solidBrush, new System.Drawing.Rectangle(0, 0, num1, num1));
          if (backgroundImage != null)
          {
            switch (backgroundImageStyle)
            {
              case ArtQRCode.BackgroundImageStyle.Fill:
                graphics.DrawImage((Image) this.Resize(backgroundImage, num1), 0, 0);
                break;
              case ArtQRCode.BackgroundImageStyle.DataAreaOnly:
                int num2 = 4 - offset;
                graphics.DrawImage((Image) this.Resize(backgroundImage, num1 - 2 * num2 * pixelsPerModule), num2 * pixelsPerModule, num2 * pixelsPerModule);
                break;
            }
          }
          Bitmap bitmap1 = this.MakeDotPixel(pixelsPerModule, pixelSize, brush2);
          Bitmap bitmap2 = this.MakeDotPixel(pixelsPerModule, pixelSize, brush1);
          for (int x = 0; x < numModules; ++x)
          {
            for (int y = 0; y < numModules; ++y)
            {
              System.Drawing.Rectangle rect = new System.Drawing.Rectangle(x * pixelsPerModule, y * pixelsPerModule, pixelsPerModule, pixelsPerModule);
              int num3 = this.QrCodeData.ModuleMatrix[offset + y][offset + x] ? 1 : 0;
              SolidBrush solidBrush = num3 != 0 ? brush2 : brush1;
              Bitmap bitmap3 = num3 != 0 ? bitmap1 : bitmap2;
              if (!this.IsPartOfFinderPattern(x, y, numModules, offset))
              {
                if (drawQuietZones && quietZoneRenderingStyle == ArtQRCode.QuietZoneStyle.Flat && this.IsPartOfQuietZone(x, y, numModules))
                  graphics.FillRectangle((Brush) solidBrush, rect);
                else
                  graphics.DrawImage((Image) bitmap3, rect);
              }
              else if (finderPatternImage == null)
                graphics.FillRectangle((Brush) solidBrush, rect);
            }
          }
          if (finderPatternImage != null)
          {
            int num4 = 7 * pixelsPerModule;
            graphics.DrawImage((Image) finderPatternImage, new System.Drawing.Rectangle(0, 0, num4, num4));
            graphics.DrawImage((Image) finderPatternImage, new System.Drawing.Rectangle(num1 - num4, 0, num4, num4));
            graphics.DrawImage((Image) finderPatternImage, new System.Drawing.Rectangle(0, num1 - num4, num4, num4));
          }
          graphics.Save();
        }
      }
    }
    return graphic;
  }

  private Bitmap MakeDotPixel(int pixelsPerModule, int pixelSize, SolidBrush brush)
  {
    Bitmap bitmap1 = new Bitmap(pixelSize, pixelSize);
    using (Graphics graphics = Graphics.FromImage((Image) bitmap1))
    {
      graphics.FillEllipse((Brush) brush, new System.Drawing.Rectangle(0, 0, pixelSize, pixelSize));
      graphics.Save();
    }
    int num1 = Math.Min(pixelsPerModule, pixelSize);
    int num2 = Math.Max((pixelsPerModule - num1) / 2, 0);
    Bitmap bitmap2 = new Bitmap(pixelsPerModule, pixelsPerModule);
    using (Graphics graphics = Graphics.FromImage((Image) bitmap2))
    {
      graphics.DrawImage((Image) bitmap1, (RectangleF) new System.Drawing.Rectangle(num2, num2, num1, num1), new RectangleF((float) (((double) pixelSize - (double) num1) / 2.0), (float) (((double) pixelSize - (double) num1) / 2.0), (float) num1, (float) num1), GraphicsUnit.Pixel);
      graphics.Save();
    }
    return bitmap2;
  }

  private bool IsPartOfQuietZone(int x, int y, int numModules)
  {
    return x < 4 || y < 4 || x > numModules - 5 || y > numModules - 5;
  }

  private bool IsPartOfFinderPattern(int x, int y, int numModules, int offset)
  {
    int num1 = 11 - offset;
    int num2 = numModules - num1 - 1;
    int num3 = num2 + 8;
    int num4 = 4 - offset;
    if (x >= num4 && x < num1 && y >= num4 && y < num1 || x > num2 && x < num3 && y >= num4 && y < num1)
      return true;
    return x >= num4 && x < num1 && y > num2 && y < num3;
  }

  private Bitmap Resize(Bitmap image, int newSize)
  {
    if (image == null)
      return (Bitmap) null;
    float num = Math.Min((float) newSize / (float) image.Width, (float) newSize / (float) image.Height);
    int width = (int) ((double) image.Width * (double) num);
    int height = (int) ((double) image.Height * (double) num);
    int x = (newSize - width) / 2;
    int y = (newSize - height) / 2;
    Bitmap bitmap1 = new Bitmap((Image) image, new Size(width, height));
    Bitmap bitmap2 = new Bitmap(newSize, newSize);
    using (Graphics graphics = Graphics.FromImage((Image) bitmap2))
    {
      using (SolidBrush solidBrush = new SolidBrush(Color.Transparent))
      {
        graphics.FillRectangle((Brush) solidBrush, new System.Drawing.Rectangle(0, 0, newSize, newSize));
        graphics.InterpolationMode = InterpolationMode.High;
        graphics.CompositingQuality = CompositingQuality.HighQuality;
        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        graphics.DrawImage((Image) bitmap1, new System.Drawing.Rectangle(x, y, width, height));
      }
    }
    return bitmap2;
  }

  public enum QuietZoneStyle
  {
    Dotted,
    Flat,
  }

  public enum BackgroundImageStyle
  {
    Fill,
    DataAreaOnly,
  }
}
