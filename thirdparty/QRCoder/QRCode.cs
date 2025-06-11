// Decompiled with JetBrains decompiler
// Type: QRCoder.QRCode
// Assembly: QRCoder, Version=1.4.3.0, Culture=neutral, PublicKeyToken=c4ed5b9ae8358a28
// MVID: 5D0AD632-49D4-4E68-92A0-261964209740
// Assembly location: D:\PDFGear\bin\QRCoder.dll

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

#nullable disable
namespace QRCoder;

public class QRCode : AbstractQRCode, IDisposable
{
  public QRCode()
  {
  }

  public QRCode(QRCodeData data)
    : base(data)
  {
  }

  public Bitmap GetGraphic(int pixelsPerModule)
  {
    return this.GetGraphic(pixelsPerModule, Color.Black, Color.White, true);
  }

  public Bitmap GetGraphic(
    int pixelsPerModule,
    string darkColorHtmlHex,
    string lightColorHtmlHex,
    bool drawQuietZones = true)
  {
    return this.GetGraphic(pixelsPerModule, ColorTranslator.FromHtml(darkColorHtmlHex), ColorTranslator.FromHtml(lightColorHtmlHex), drawQuietZones);
  }

  public Bitmap GetGraphic(
    int pixelsPerModule,
    Color darkColor,
    Color lightColor,
    bool drawQuietZones = true)
  {
    int num1 = (this.QrCodeData.ModuleMatrix.Count - (drawQuietZones ? 0 : 8)) * pixelsPerModule;
    int num2 = drawQuietZones ? 0 : 4 * pixelsPerModule;
    Bitmap graphic = new Bitmap(num1, num1);
    using (Graphics graphics = Graphics.FromImage((Image) graphic))
    {
      using (SolidBrush solidBrush1 = new SolidBrush(lightColor))
      {
        using (SolidBrush solidBrush2 = new SolidBrush(darkColor))
        {
          for (int index1 = 0; index1 < num1 + num2; index1 += pixelsPerModule)
          {
            for (int index2 = 0; index2 < num1 + num2; index2 += pixelsPerModule)
            {
              if (this.QrCodeData.ModuleMatrix[(index2 + pixelsPerModule) / pixelsPerModule - 1][(index1 + pixelsPerModule) / pixelsPerModule - 1])
                graphics.FillRectangle((Brush) solidBrush2, new System.Drawing.Rectangle(index1 - num2, index2 - num2, pixelsPerModule, pixelsPerModule));
              else
                graphics.FillRectangle((Brush) solidBrush1, new System.Drawing.Rectangle(index1 - num2, index2 - num2, pixelsPerModule, pixelsPerModule));
            }
          }
          graphics.Save();
        }
      }
    }
    return graphic;
  }

  public Bitmap GetGraphic(
    int pixelsPerModule,
    Color darkColor,
    Color lightColor,
    Bitmap icon = null,
    int iconSizePercent = 15,
    int iconBorderWidth = 0,
    bool drawQuietZones = true,
    Color? iconBackgroundColor = null)
  {
    int num1 = (this.QrCodeData.ModuleMatrix.Count - (drawQuietZones ? 0 : 8)) * pixelsPerModule;
    int num2 = drawQuietZones ? 0 : 4 * pixelsPerModule;
    Bitmap graphic = new Bitmap(num1, num1, PixelFormat.Format32bppArgb);
    using (Graphics graphics = Graphics.FromImage((Image) graphic))
    {
      using (SolidBrush solidBrush1 = new SolidBrush(lightColor))
      {
        using (SolidBrush solidBrush2 = new SolidBrush(darkColor))
        {
          graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
          graphics.CompositingQuality = CompositingQuality.HighQuality;
          graphics.Clear(lightColor);
          bool flag = icon != null && iconSizePercent > 0 && iconSizePercent <= 100;
          for (int index1 = 0; index1 < num1 + num2; index1 += pixelsPerModule)
          {
            for (int index2 = 0; index2 < num1 + num2; index2 += pixelsPerModule)
            {
              SolidBrush solidBrush3 = this.QrCodeData.ModuleMatrix[(index2 + pixelsPerModule) / pixelsPerModule - 1][(index1 + pixelsPerModule) / pixelsPerModule - 1] ? solidBrush2 : solidBrush1;
              graphics.FillRectangle((Brush) solidBrush3, new System.Drawing.Rectangle(index1 - num2, index2 - num2, pixelsPerModule, pixelsPerModule));
            }
          }
          if (flag)
          {
            float width = (float) (iconSizePercent * graphic.Width) / 100f;
            float height = flag ? width * (float) icon.Height / (float) icon.Width : 0.0f;
            float x = (float) (((double) graphic.Width - (double) width) / 2.0);
            float y = (float) (((double) graphic.Height - (double) height) / 2.0);
            RectangleF rect = new RectangleF(x - (float) iconBorderWidth, y - (float) iconBorderWidth, width + (float) (iconBorderWidth * 2), height + (float) (iconBorderWidth * 2));
            RectangleF destRect = new RectangleF(x, y, width, height);
            SolidBrush solidBrush4 = iconBackgroundColor.HasValue ? new SolidBrush(iconBackgroundColor.Value) : solidBrush1;
            if (iconBorderWidth > 0)
            {
              using (GraphicsPath roundedRectanglePath = this.CreateRoundedRectanglePath(rect, iconBorderWidth * 2))
                graphics.FillPath((Brush) solidBrush4, roundedRectanglePath);
            }
            graphics.DrawImage((Image) icon, destRect, new RectangleF(0.0f, 0.0f, (float) icon.Width, (float) icon.Height), GraphicsUnit.Pixel);
          }
          graphics.Save();
        }
      }
    }
    return graphic;
  }

  internal GraphicsPath CreateRoundedRectanglePath(RectangleF rect, int cornerRadius)
  {
    GraphicsPath roundedRectanglePath = new GraphicsPath();
    roundedRectanglePath.AddArc(rect.X, rect.Y, (float) (cornerRadius * 2), (float) (cornerRadius * 2), 180f, 90f);
    roundedRectanglePath.AddLine(rect.X + (float) cornerRadius, rect.Y, rect.Right - (float) (cornerRadius * 2), rect.Y);
    roundedRectanglePath.AddArc(rect.X + rect.Width - (float) (cornerRadius * 2), rect.Y, (float) (cornerRadius * 2), (float) (cornerRadius * 2), 270f, 90f);
    roundedRectanglePath.AddLine(rect.Right, rect.Y + (float) (cornerRadius * 2), rect.Right, rect.Y + rect.Height - (float) (cornerRadius * 2));
    roundedRectanglePath.AddArc(rect.X + rect.Width - (float) (cornerRadius * 2), rect.Y + rect.Height - (float) (cornerRadius * 2), (float) (cornerRadius * 2), (float) (cornerRadius * 2), 0.0f, 90f);
    roundedRectanglePath.AddLine(rect.Right - (float) (cornerRadius * 2), rect.Bottom, rect.X + (float) (cornerRadius * 2), rect.Bottom);
    roundedRectanglePath.AddArc(rect.X, rect.Bottom - (float) (cornerRadius * 2), (float) (cornerRadius * 2), (float) (cornerRadius * 2), 90f, 90f);
    roundedRectanglePath.AddLine(rect.X, rect.Bottom - (float) (cornerRadius * 2), rect.X, rect.Y + (float) (cornerRadius * 2));
    roundedRectanglePath.CloseFigure();
    return roundedRectanglePath;
  }
}
