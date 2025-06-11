// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.MotionPath
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Animation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Presentation;

internal class MotionPath : IMotionPath, IEnumerable<IMotionCmdPath>, IEnumerable
{
  private List<IMotionCmdPath> pathValuesList = new List<IMotionCmdPath>();

  public int Count => this.pathValuesList.Count;

  public IMotionCmdPath this[int index]
  {
    get
    {
      if (this.pathValuesList.Count <= index)
        throw new IndexOutOfRangeException("Index was out of range,value should be greater than motion path list count");
      return this.pathValuesList[index];
    }
  }

  public IMotionCmdPath Add(
    MotionCommandPathType type,
    PointF[] points,
    MotionPathPointsType pointsType,
    bool isRelativeCoord)
  {
    points = AnimationConstant.CheckIsValidCmdPath(type, points);
    IMotionCmdPath motionCmdPath = (IMotionCmdPath) new MotionCmdPath();
    (motionCmdPath as MotionCmdPath).SetCommandType(type);
    if (points != null)
      motionCmdPath.Points = points.Clone() as PointF[];
    motionCmdPath.PointsType = pointsType;
    motionCmdPath.IsRelative = isRelativeCoord;
    this.pathValuesList.Add(motionCmdPath);
    return motionCmdPath;
  }

  public void Clear() => this.pathValuesList.Clear();

  public void Insert(
    int index,
    MotionCommandPathType type,
    PointF[] points,
    MotionPathPointsType pointsType,
    bool isRelativeCoord)
  {
    if (this.pathValuesList.Count <= index)
      throw new IndexOutOfRangeException("Index was out of range,value should be less than or equal to pathvalues count");
    points = AnimationConstant.CheckIsValidCmdPath(type, points);
    IMotionCmdPath motionCmdPath = (IMotionCmdPath) new MotionCmdPath();
    (motionCmdPath as MotionCmdPath).SetCommandType(type);
    if (points != null)
      motionCmdPath.Points = points.Clone() as PointF[];
    motionCmdPath.PointsType = pointsType;
    motionCmdPath.IsRelative = isRelativeCoord;
    this.pathValuesList.Insert(index, motionCmdPath);
  }

  public void Remove(IMotionCmdPath item)
  {
    if (item == null)
      return;
    this.pathValuesList.Remove(item);
  }

  public void RemoveAt(int index)
  {
    if (index >= this.pathValuesList.Count)
      throw new Exception("Index must be less than or equal to path values count");
    this.pathValuesList.RemoveAt(index);
  }

  public IEnumerator<IMotionCmdPath> GetEnumerator()
  {
    return (IEnumerator<IMotionCmdPath>) this.pathValuesList.GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.pathValuesList.GetEnumerator();

  internal void Add(IMotionCmdPath motionPath) => this.pathValuesList.Add(motionPath);

  internal MotionPath Clone()
  {
    MotionPath motionPath = (MotionPath) this.MemberwiseClone();
    if (this.pathValuesList != null)
      motionPath.pathValuesList = this.ClonePathValueList();
    return motionPath;
  }

  private List<IMotionCmdPath> ClonePathValueList()
  {
    List<IMotionCmdPath> motionCmdPathList = new List<IMotionCmdPath>();
    foreach (MotionCmdPath pathValues in this.pathValuesList)
    {
      MotionCmdPath motionCmdPath = pathValues.Clone();
      motionCmdPathList.Add((IMotionCmdPath) motionCmdPath);
    }
    return motionCmdPathList;
  }
}
