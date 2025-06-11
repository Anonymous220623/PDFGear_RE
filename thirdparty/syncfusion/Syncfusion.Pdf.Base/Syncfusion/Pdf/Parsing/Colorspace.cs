// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.Colorspace
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal abstract class Colorspace
{
  private Brush m_defaultBrush = Brushes.Black;

  internal abstract int Components { get; }

  internal virtual Brush DefaultBrush => this.m_defaultBrush;

  internal abstract Color GetColor(string[] pars);

  internal abstract Color GetColor(byte[] bytes, int offset);

  internal virtual void SetOperatorValues(bool IsRectangle, bool IsCircle, string RectangleWidth)
  {
  }

  internal abstract Brush GetBrush(string[] pars, PdfPageResources resource);

  internal static bool IsColorSpace(string name)
  {
    name = Colorspace.GetColorSpaceName(name);
    switch (name)
    {
      case "DeviceGray":
      case "DeviceRGB":
      case "DeviceCMYK":
      case "CalCMYK":
      case "CalGray":
      case "CalRGB":
      case "ICCBased":
      case "Indexed":
      case "Pattern":
      case "Separation":
      case "DeviceN":
        return true;
      default:
        return false;
    }
  }

  private static string GetColorSpaceName(string name)
  {
    switch (name)
    {
      case "G":
        return "DeviceGray";
      case "RGB":
        return "DeviceRGB";
      case "CMYK":
        return "DeviceCMYK";
      case "I":
        return "Indexed";
      default:
        return name;
    }
  }

  internal static Colorspace CreateColorSpace(string name)
  {
    name = Colorspace.GetColorSpaceName(name);
    switch (name)
    {
      case "DeviceGray":
        return (Colorspace) new DeviceGray();
      case "DeviceRGB":
        return (Colorspace) new DeviceRGB();
      case "DeviceCMYK":
        return (Colorspace) new DeviceCMYK();
      case "ICCBased":
        return (Colorspace) new ICCBased();
      case "CalRGB":
        return (Colorspace) new DeviceRGB();
      case "CalGray":
        return (Colorspace) new CalGray();
      case "Lab":
        return (Colorspace) new LabColor();
      case "Indexed":
        return (Colorspace) new Indexed();
      case "Separation":
        return (Colorspace) new Separation();
      case "DeviceN":
        return (Colorspace) new DeviceN();
      case "Pattern":
        return (Colorspace) new Pattern();
      case "Shading":
        return (Colorspace) new ShadingPattern();
      default:
        throw new NotSupportedException("Color space is not supported.");
    }
  }

  internal static Colorspace CreateColorSpace(string value, IPdfPrimitive array)
  {
    Colorspace colorSpace1 = Colorspace.CreateColorSpace(value);
    switch (colorSpace1)
    {
      case CalRGB colorSpace2:
        colorSpace2.SetValue(array as PdfArray);
        return (Colorspace) colorSpace2;
      case CalGray colorSpace3:
        colorSpace3.SetValue(array as PdfArray);
        return (Colorspace) colorSpace3;
      case LabColor colorSpace4:
        colorSpace4.SetValue(array as PdfArray);
        return (Colorspace) colorSpace4;
      case ICCBased colorSpace5:
        colorSpace5.Profile = new ICCProfile(array as PdfArray);
        return (Colorspace) colorSpace5;
      case Indexed colorSpace6:
        colorSpace6.SetValue(array as PdfArray);
        return (Colorspace) colorSpace6;
      case Separation colorSpace7:
        colorSpace7.SetValue(array as PdfArray);
        return (Colorspace) colorSpace7;
      case DeviceN colorSpace8:
        colorSpace8.SetValue(array as PdfArray);
        return (Colorspace) colorSpace8;
      case ShadingPattern colorSpace9:
        colorSpace9.SetShadingValue(array);
        return (Colorspace) colorSpace9;
      case Pattern colorSpace10:
        colorSpace10.SetValue(array);
        return (Colorspace) colorSpace10;
      default:
        return colorSpace1;
    }
  }

  protected static Color GetRgbColor(string[] pars)
  {
    float result1;
    float.TryParse(pars[0], out result1);
    float result2;
    float.TryParse(pars[1], out result2);
    float result3;
    float.TryParse(pars[2], out result3);
    return Color.FromArgb((int) byte.MaxValue, (int) (byte) ((double) result1 * (double) byte.MaxValue), (int) (byte) ((double) result2 * (double) byte.MaxValue), (int) (byte) ((double) result3 * (double) byte.MaxValue));
  }

  protected static Color GetRgbColor(float[] values)
  {
    return Color.FromArgb((int) byte.MaxValue, (int) (byte) ((double) values[0] * (double) byte.MaxValue), (int) (byte) ((double) values[1] * (double) byte.MaxValue), (int) (byte) ((double) values[2] * (double) byte.MaxValue));
  }

  protected static Color GetRgbColor(double[] values)
  {
    return Color.FromArgb((int) byte.MaxValue, (int) (byte) (values[0] * (double) byte.MaxValue), (int) (byte) (values[1] * (double) byte.MaxValue), (int) (byte) (values[2] * (double) byte.MaxValue));
  }

  protected static Color GetCmykColor(string[] pars)
  {
    float result1;
    float.TryParse(pars[0], out result1);
    float result2;
    float.TryParse(pars[1], out result2);
    float result3;
    float.TryParse(pars[2], out result3);
    float result4;
    float.TryParse(pars[3], out result4);
    return Colorspace.ConvertCMYKtoRGB(result1, result2, result3, result4);
  }

  protected static Color GetGrayColor(string[] pars)
  {
    float result;
    float.TryParse(pars[0], out result);
    return Color.FromArgb((int) byte.MaxValue, (int) (byte) ((double) result * (double) byte.MaxValue), (int) (byte) ((double) result * (double) byte.MaxValue), (int) (byte) ((double) result * (double) byte.MaxValue));
  }

  private static Color ConvertCMYKtoRGB(float c, float m, float y, float k)
  {
    float num1 = (float) ((double) c * (-4.3873323846099881 * (double) c + 54.486151941891762 * (double) m + 18.822905021653021 * (double) y + 212.25662451639585 * (double) k - 285.2331026137004) + (double) m * (1.7149763477362134 * (double) m - 5.6096736904047315 * (double) y + -17.873870861415444 * (double) k - 5.4970064271963661) + (double) y * (-2.5217340131683033 * (double) y - 21.248923337353073 * (double) k + 17.5119270841813) + (double) k * (-21.86122147463605 * (double) k - 189.48180835922747) + (double) byte.MaxValue);
    float num2 = (float) ((double) c * (8.8410414220361488 * (double) c + 60.118027045597366 * (double) m + 6.8714255920490066 * (double) y + 31.159100130055922 * (double) k - 79.2970844816548) + (double) m * (-15.310361306967817 * (double) m + 17.575251261109482 * (double) y + 131.35250912493976 * (double) k - 190.9453302588951) + (double) y * (4.444339102852739 * (double) y + 9.8632861493405 * (double) k - 24.86741582555878) + (double) k * (-20.737325471181034 * (double) k - 187.80453709719578) + (double) byte.MaxValue);
    float num3 = (float) ((double) c * (0.88425224300032956 * (double) c + 8.0786775031129281 * (double) m + 30.89978309703729 * (double) y - 0.23883238689178934 * (double) k - 14.183576799673286) + (double) m * (10.49593273432072 * (double) m + 63.02378494754052 * (double) y + 50.606957656360734 * (double) k - 112.23884253719248) + (double) y * (0.032960411148732167 * (double) y + 115.60384449646641 * (double) k - 193.58209356861505) + (double) k * (-22.33816807309886 * (double) k - 180.12613974708367) + (double) byte.MaxValue);
    return Color.FromArgb((int) byte.MaxValue, (double) num1 > (double) byte.MaxValue ? (int) byte.MaxValue : ((double) num1 < 0.0 ? 0 : (int) num1), (double) num2 > (double) byte.MaxValue ? (int) byte.MaxValue : ((double) num2 < 0.0 ? 0 : (int) num2), (double) num3 > (double) byte.MaxValue ? (int) byte.MaxValue : ((double) num3 < 0.0 ? 0 : (int) num3));
  }

  protected static Color GetRgbColor(byte[] bytes, int offset)
  {
    return Color.FromArgb((int) byte.MaxValue, (int) bytes[offset], (int) bytes[offset + 1], (int) bytes[offset + 2]);
  }

  protected static Color GetGrayColor(byte[] bytes, int offset)
  {
    return Color.FromArgb((int) byte.MaxValue, (int) bytes[offset] * (int) byte.MaxValue, (int) bytes[offset] * (int) byte.MaxValue, (int) bytes[offset] * (int) byte.MaxValue);
  }

  protected static Color GetCmykColor(byte[] bytes, int offset) => Color.Empty;

  internal string[] ToParams(double[] values)
  {
    string[] strArray = new string[values.Length];
    for (int index = 0; index < values.Length; ++index)
      strArray[index] = values[index].ToString();
    return strArray;
  }
}
