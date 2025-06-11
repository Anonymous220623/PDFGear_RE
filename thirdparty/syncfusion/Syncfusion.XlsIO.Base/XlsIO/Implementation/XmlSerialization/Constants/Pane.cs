// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.Constants.Pane
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization.Constants;

internal sealed class Pane
{
  public const string TagName = "pane";
  public const string XSplit = "xSplit";
  public const string YSplit = "ySplit";
  public const string TopLeftCell = "topLeftCell";
  public const string Active = "activePane";
  public const string State = "state";
  public const string StateFrozen = "frozen";
  public const string StateFrozenSplit = "frozenSplit";
  public const string StateSplit = "split";
  public const string Selection = "selection";
  public const string ActiveCell = "activeCell";
  public const string Sqref = "sqref";
  public static readonly Dictionary<string, Pane.ActivePane> PaneStrings = new Dictionary<string, Pane.ActivePane>();

  static Pane()
  {
    Pane.PaneStrings.Add("bottomLeft", Pane.ActivePane.bottomLeft);
    Pane.PaneStrings.Add("bottomRight", Pane.ActivePane.bottomRight);
    Pane.PaneStrings.Add("topLeft", Pane.ActivePane.topLeft);
    Pane.PaneStrings.Add("topRight", Pane.ActivePane.topRight);
  }

  public enum ActivePane
  {
    bottomRight,
    topRight,
    bottomLeft,
    topLeft,
  }
}
