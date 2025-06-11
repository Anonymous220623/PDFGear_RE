// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.MotionCmdPath
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Animation;
using System.Drawing;

#nullable disable
namespace Syncfusion.Presentation;

internal class MotionCmdPath : IMotionCmdPath
{
  private MotionCommandPathType _commandType;
  private bool _isRelative;
  private PointF[] _points;
  private MotionPathPointsType _pointsType;

  public MotionCommandPathType CommandType
  {
    get => this._commandType;
    set
    {
      this._points = AnimationConstant.CheckIsValidCmdPath(value, this._points);
      this._commandType = value;
    }
  }

  public bool IsRelative
  {
    get => this._isRelative;
    set => this._isRelative = value;
  }

  public PointF[] Points
  {
    get => this._points;
    set => this._points = value;
  }

  public MotionPathPointsType PointsType
  {
    get => this._pointsType;
    set => this._pointsType = value;
  }

  internal void SetCommandType(MotionCommandPathType pathType) => this._commandType = pathType;

  internal MotionCmdPath Clone() => (MotionCmdPath) this.MemberwiseClone();
}
