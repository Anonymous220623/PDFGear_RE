// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.TemplateMarkers.DirectionArgument
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.TemplateMarkers;

[TemplateMarker]
public class DirectionArgument : MarkerArgument
{
  private static readonly SortedList<string, MarkerDirection> s_lstStringToDirection = new SortedList<string, MarkerDirection>(2);
  private MarkerDirection m_direction;

  static DirectionArgument()
  {
    DirectionArgument.s_lstStringToDirection.Add("vertical", MarkerDirection.Vertical);
    DirectionArgument.s_lstStringToDirection.Add("horizontal", MarkerDirection.Horizontal);
  }

  public override MarkerArgument TryParse(string strArgument)
  {
    if (strArgument == null || strArgument.Length == 0)
      return (MarkerArgument) null;
    DirectionArgument directionArgument = (DirectionArgument) null;
    MarkerDirection markerDirection;
    if (DirectionArgument.s_lstStringToDirection.TryGetValue(strArgument.ToLower(), out markerDirection))
    {
      directionArgument = (DirectionArgument) this.Clone();
      directionArgument.m_direction = markerDirection;
    }
    return (MarkerArgument) directionArgument;
  }

  public override void PrepareOptions(MarkerOptionsImpl options)
  {
    if (options == null)
      throw new ArgumentNullException(nameof (options));
    options.Direction = this.m_direction;
  }

  public override bool IsPreparing => true;

  internal string TryGetDirection(string argument)
  {
    foreach (string key in (IEnumerable<string>) DirectionArgument.s_lstStringToDirection.Keys)
    {
      if (argument.Contains(key))
        return key;
    }
    return (string) null;
  }
}
