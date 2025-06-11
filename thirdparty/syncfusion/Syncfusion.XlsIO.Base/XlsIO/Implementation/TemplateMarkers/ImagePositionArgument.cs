// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.TemplateMarkers.ImagePositionArgument
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.TemplateMarkers;

[TemplateMarker]
internal class ImagePositionArgument : MarkerArgument
{
  private const string DEF_IMAGE_POSITION_STRING = "position";
  private const ImageVerticalPosition DEF_IMAGE_VPOSITION = ImageVerticalPosition.Top;
  private const ImageHorizontalPosition DEF_IMAGE_HPOSITION = ImageHorizontalPosition.Left;
  private const int DEF_PRIORITY = 6;
  private ImageHorizontalPosition m_hPosition;
  private ImageVerticalPosition m_vPosition;

  public ImagePositionArgument()
  {
    this.m_vPosition = ImageVerticalPosition.Top;
    this.m_hPosition = ImageHorizontalPosition.Left;
  }

  public ImageVerticalPosition VPosition => this.m_vPosition;

  public ImageHorizontalPosition HPosition => this.m_hPosition;

  public override bool IsApplyable => true;

  public override int Priority => 6;

  public override MarkerArgument TryParse(string strArgument)
  {
    if (strArgument == null || strArgument.Length == 0)
      return (MarkerArgument) null;
    string[] strArray = strArgument.Split(':');
    if (strArray[0].ToLower() != "position")
      return (MarkerArgument) null;
    if (strArray[1].Contains("-"))
    {
      Match match = new Regex("(?<vertical_position>[a-zA-Z]+)-(?<horizontal_position>[a-zA-Z]+);*").Match(strArray[1]);
      if (!match.Success)
        return (MarkerArgument) null;
      int positionFromEnum1 = this.GetPositionFromEnum((object) this.m_hPosition, match.Groups["horizontal_position"].ToString());
      if (positionFromEnum1 == 0)
        return (MarkerArgument) null;
      this.m_hPosition = (ImageHorizontalPosition) positionFromEnum1;
      int positionFromEnum2 = this.GetPositionFromEnum((object) this.m_vPosition, match.Groups["vertical_position"].ToString());
      if (positionFromEnum2 == 0)
        return (MarkerArgument) null;
      this.m_vPosition = (ImageVerticalPosition) positionFromEnum2;
    }
    else
    {
      Match match = Regex.Match(strArray[1], "^[a-z]+$");
      if (!match.Success)
        return (MarkerArgument) null;
      int positionFromEnum3 = this.GetPositionFromEnum((object) this.m_hPosition, match.Value);
      if (positionFromEnum3 > 0 && positionFromEnum3 < 4)
        this.m_hPosition = (ImageHorizontalPosition) positionFromEnum3;
      int positionFromEnum4 = this.GetPositionFromEnum((object) this.m_vPosition, match.Value);
      if (positionFromEnum4 > 0 && positionFromEnum4 < 4)
        this.m_vPosition = (ImageVerticalPosition) positionFromEnum4;
    }
    MarkerArgument markerArgument = (MarkerArgument) this.Clone();
    this.m_hPosition = ImageHorizontalPosition.Left;
    this.m_vPosition = ImageVerticalPosition.Top;
    return markerArgument;
  }

  private int GetPositionFromEnum(object enumObject, string value)
  {
    int positionFromEnum = 0;
    if (enumObject is ImageHorizontalPosition)
    {
      switch (value.ToLower())
      {
        case "left":
          positionFromEnum = 1;
          break;
        case "center":
          positionFromEnum = 2;
          break;
        case "right":
          positionFromEnum = 3;
          break;
      }
    }
    if (enumObject is ImageVerticalPosition)
    {
      switch (value.ToLower())
      {
        case "top":
          positionFromEnum = 1;
          break;
        case "middle":
          positionFromEnum = 2;
          break;
        case "bottom":
          positionFromEnum = 3;
          break;
      }
    }
    return positionFromEnum;
  }
}
