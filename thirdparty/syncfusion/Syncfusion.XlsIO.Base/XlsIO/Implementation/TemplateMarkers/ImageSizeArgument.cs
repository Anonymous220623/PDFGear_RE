// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.TemplateMarkers.ImageSizeArgument
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.TemplateMarkers;

[TemplateMarker]
public class ImageSizeArgument : MarkerArgument
{
  private const string DEF_IMAGE_SIZE_STRING = "size";
  private const int DEF_IMAGE_WIDTH = 50;
  private const int DEF_IMAGE_HEIGHT = 50;
  private const int DEF_PRIORITY = 5;
  private int m_imageWidth;
  private int m_imageHeight;
  private bool m_isAutoWidth;
  private bool m_isAutoHeight;

  public ImageSizeArgument()
  {
    this.m_imageHeight = 50;
    this.m_imageWidth = 50;
  }

  public int Width => this.m_imageWidth;

  public int Height => this.m_imageHeight;

  public override bool IsApplyable => true;

  public override int Priority => 5;

  internal bool IsAutoWidth => this.m_isAutoWidth;

  internal bool IsAutoHeight => this.m_isAutoHeight;

  public override MarkerArgument TryParse(string strArgument)
  {
    this.m_isAutoWidth = false;
    this.m_isAutoHeight = false;
    if (strArgument == null || strArgument.Length == 0)
      return (MarkerArgument) null;
    string[] strArray1 = strArgument.Split(':');
    if (strArray1[0].ToLower() != "size")
      return (MarkerArgument) null;
    if (strArray1[1].Contains(","))
    {
      Match match = new Regex("^([0-9]+|(\\b(auto)\\b)),{1}([0-9]+|(\\b(auto)\\b))$").Match(strArray1[1]);
      if (!match.Success)
        return (MarkerArgument) null;
      string[] strArray2 = match.Value.Split(',');
      int result1;
      if (int.TryParse(strArray2[0], out result1) && result1 > 50)
        this.m_imageWidth = result1;
      else if (strArray2[0] == "auto")
        this.m_isAutoWidth = true;
      int result2;
      if (int.TryParse(strArray2[1], out result2) && result2 > 50)
        this.m_imageHeight = result2;
      else if (strArray2[1] == "auto")
        this.m_isAutoHeight = true;
    }
    else
    {
      int result = 0;
      if (!int.TryParse(strArray1[1], out result))
        return (MarkerArgument) null;
      if (result > 50)
        this.m_imageWidth = result;
      if (result > 50)
        this.m_imageHeight = result;
    }
    MarkerArgument markerArgument = (MarkerArgument) this.Clone();
    this.m_imageHeight = 50;
    this.m_imageWidth = 50;
    return markerArgument;
  }
}
