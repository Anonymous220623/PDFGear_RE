// Decompiled with JetBrains decompiler
// Type: HandyControl.Media.Animation.GeometryAnimationUsingKeyFrames
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;

#nullable disable
namespace HandyControl.Media.Animation;

[ContentProperty("KeyFrames")]
public class GeometryAnimationUsingKeyFrames : GeometryAnimationBase, IKeyFrameAnimation, IAddChild
{
  private string[] _strings;
  private GeometryKeyFrameCollection _keyFrames;
  private ResolvedKeyFrameEntry[] _sortedResolvedKeyFrames;
  private bool _areKeyTimesValid;

  public string[] Strings
  {
    get
    {
      return this._keyFrames == null || this._keyFrames.Count == 0 ? (string[]) null : this._strings ?? (this._strings = Regex.Split(this._keyFrames[0].Value.ToString((IFormatProvider) CultureInfo.InvariantCulture), "[+-]?\\d*\\.?\\d+(?:\\.\\d+)?(?:[eE][+-]?\\d+)?"));
    }
  }

  public GeometryAnimationUsingKeyFrames() => this._areKeyTimesValid = true;

  public GeometryAnimationUsingKeyFrames Clone() => (GeometryAnimationUsingKeyFrames) base.Clone();

  public GeometryAnimationUsingKeyFrames CloneCurrentValue()
  {
    return (GeometryAnimationUsingKeyFrames) base.CloneCurrentValue();
  }

  protected override bool FreezeCore(bool isChecking)
  {
    int num = base.FreezeCore(isChecking) & Freezable.Freeze((Freezable) this._keyFrames, isChecking) ? 1 : 0;
    if ((num & (!this._areKeyTimesValid ? 1 : 0)) == 0)
      return num != 0;
    this.ResolveKeyTimes();
    return num != 0;
  }

  protected override void OnChanged()
  {
    this._areKeyTimesValid = false;
    base.OnChanged();
  }

  protected override Freezable CreateInstanceCore()
  {
    return (Freezable) new GeometryAnimationUsingKeyFrames();
  }

  protected override void CloneCore(Freezable sourceFreezable)
  {
    GeometryAnimationUsingKeyFrames sourceAnimation = (GeometryAnimationUsingKeyFrames) sourceFreezable;
    base.CloneCore(sourceFreezable);
    this.CopyCommon(sourceAnimation, false);
  }

  protected override void CloneCurrentValueCore(Freezable sourceFreezable)
  {
    GeometryAnimationUsingKeyFrames sourceAnimation = (GeometryAnimationUsingKeyFrames) sourceFreezable;
    base.CloneCurrentValueCore(sourceFreezable);
    this.CopyCommon(sourceAnimation, true);
  }

  protected override void GetAsFrozenCore(Freezable source)
  {
    GeometryAnimationUsingKeyFrames sourceAnimation = (GeometryAnimationUsingKeyFrames) source;
    base.GetAsFrozenCore(source);
    this.CopyCommon(sourceAnimation, false);
  }

  protected override void GetCurrentValueAsFrozenCore(Freezable source)
  {
    GeometryAnimationUsingKeyFrames sourceAnimation = (GeometryAnimationUsingKeyFrames) source;
    base.GetCurrentValueAsFrozenCore(source);
    this.CopyCommon(sourceAnimation, true);
  }

  private void CopyCommon(GeometryAnimationUsingKeyFrames sourceAnimation, bool isCurrentValueClone)
  {
    this._areKeyTimesValid = sourceAnimation._areKeyTimesValid;
    if (this._areKeyTimesValid && sourceAnimation._sortedResolvedKeyFrames != null)
      this._sortedResolvedKeyFrames = (ResolvedKeyFrameEntry[]) sourceAnimation._sortedResolvedKeyFrames.Clone();
    if (sourceAnimation._keyFrames == null)
      return;
    this._keyFrames = !isCurrentValueClone ? sourceAnimation._keyFrames.Clone() : (GeometryKeyFrameCollection) sourceAnimation._keyFrames.CloneCurrentValue();
    this.OnFreezablePropertyChanged((DependencyObject) null, (DependencyObject) this._keyFrames);
  }

  void IAddChild.AddChild(object child)
  {
    this.WritePreamble();
    if (child == null)
      throw new ArgumentNullException(nameof (child));
    this.AddChild(child);
    this.WritePostscript();
  }

  [EditorBrowsable(EditorBrowsableState.Advanced)]
  protected virtual void AddChild(object child)
  {
    if (!(child is GeometryKeyFrame keyFrame))
      throw new ArgumentException("Animation_ChildMustBeKeyFrame", nameof (child));
    this.KeyFrames.Add(keyFrame);
  }

  void IAddChild.AddText(string childText)
  {
    if (childText == null)
      throw new ArgumentNullException(nameof (childText));
    this.AddText(childText);
  }

  [EditorBrowsable(EditorBrowsableState.Advanced)]
  protected virtual void AddText(string childText)
  {
    throw new InvalidOperationException("Animation_NoTextChildren");
  }

  protected override Geometry GetCurrentValueCore(
    Geometry defaultOriginValue,
    Geometry defaultDestinationValue,
    AnimationClock animationClock)
  {
    if (this._keyFrames == null)
      return defaultDestinationValue;
    if (!this._areKeyTimesValid)
      this.ResolveKeyTimes();
    if (this._sortedResolvedKeyFrames == null)
      return defaultDestinationValue;
    TimeSpan timeSpan = animationClock.CurrentTime.Value;
    int length = this._sortedResolvedKeyFrames.Length;
    int resolvedKeyFrameIndex1 = length - 1;
    int resolvedKeyFrameIndex2 = 0;
    while (resolvedKeyFrameIndex2 < length && timeSpan > this._sortedResolvedKeyFrames[resolvedKeyFrameIndex2]._resolvedKeyTime)
      ++resolvedKeyFrameIndex2;
    while (resolvedKeyFrameIndex2 < resolvedKeyFrameIndex1 && timeSpan == this._sortedResolvedKeyFrames[resolvedKeyFrameIndex2 + 1]._resolvedKeyTime)
      ++resolvedKeyFrameIndex2;
    double[] arr1;
    if (resolvedKeyFrameIndex2 == length)
      arr1 = this.GetResolvedKeyFrameValue(resolvedKeyFrameIndex1);
    else if (timeSpan == this._sortedResolvedKeyFrames[resolvedKeyFrameIndex2]._resolvedKeyTime)
    {
      arr1 = this.GetResolvedKeyFrameValue(resolvedKeyFrameIndex2);
    }
    else
    {
      double[] arr2;
      double keyFrameProgress;
      if (resolvedKeyFrameIndex2 == 0)
      {
        AnimationHelper.DecomposeGeometryStr(defaultOriginValue.ToString((IFormatProvider) CultureInfo.InvariantCulture), out arr2);
        keyFrameProgress = timeSpan.TotalMilliseconds / this._sortedResolvedKeyFrames[0]._resolvedKeyTime.TotalMilliseconds;
      }
      else
      {
        int resolvedKeyFrameIndex3 = resolvedKeyFrameIndex2 - 1;
        TimeSpan resolvedKeyTime = this._sortedResolvedKeyFrames[resolvedKeyFrameIndex3]._resolvedKeyTime;
        arr2 = this.GetResolvedKeyFrameValue(resolvedKeyFrameIndex3);
        keyFrameProgress = (timeSpan - resolvedKeyTime).TotalMilliseconds / (this._sortedResolvedKeyFrames[resolvedKeyFrameIndex2]._resolvedKeyTime - resolvedKeyTime).TotalMilliseconds;
      }
      arr1 = this.GetResolvedKeyFrame(resolvedKeyFrameIndex2).InterpolateValue(arr2, keyFrameProgress);
    }
    return AnimationHelper.ComposeGeometry(this.Strings, arr1);
  }

  protected sealed override Duration GetNaturalDurationCore(Clock clock)
  {
    return new Duration(this.LargestTimeSpanKeyTime);
  }

  IList IKeyFrameAnimation.KeyFrames
  {
    get => (IList) this.KeyFrames;
    set => this.KeyFrames = (GeometryKeyFrameCollection) value;
  }

  public GeometryKeyFrameCollection KeyFrames
  {
    get
    {
      this.ReadPreamble();
      if (this._keyFrames == null)
      {
        if (this.IsFrozen)
        {
          this._keyFrames = GeometryKeyFrameCollection.Empty;
        }
        else
        {
          this.WritePreamble();
          this._keyFrames = new GeometryKeyFrameCollection();
          this.OnFreezablePropertyChanged((DependencyObject) null, (DependencyObject) this._keyFrames);
          this.WritePostscript();
        }
      }
      return this._keyFrames;
    }
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      this.WritePreamble();
      if (value == this._keyFrames)
        return;
      this.OnFreezablePropertyChanged((DependencyObject) this._keyFrames, (DependencyObject) value);
      this._keyFrames = value;
      this.WritePostscript();
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public bool ShouldSerializeKeyFrames()
  {
    this.ReadPreamble();
    return this._keyFrames != null && this._keyFrames.Count > 0;
  }

  private double[] GetResolvedKeyFrameValue(int resolvedKeyFrameIndex)
  {
    return this.GetResolvedKeyFrame(resolvedKeyFrameIndex).Numbers;
  }

  private GeometryKeyFrame GetResolvedKeyFrame(int resolvedKeyFrameIndex)
  {
    return this._keyFrames[this._sortedResolvedKeyFrames[resolvedKeyFrameIndex]._originalKeyFrameIndex];
  }

  private TimeSpan LargestTimeSpanKeyTime
  {
    get
    {
      bool flag = false;
      TimeSpan timeSpan = TimeSpan.Zero;
      if (this._keyFrames != null)
      {
        int count = this._keyFrames.Count;
        for (int index = 0; index < count; ++index)
        {
          KeyTime keyTime = this._keyFrames[index].KeyTime;
          if (keyTime.Type == KeyTimeType.TimeSpan)
          {
            flag = true;
            if (keyTime.TimeSpan > timeSpan)
              timeSpan = keyTime.TimeSpan;
          }
        }
      }
      return !flag ? TimeSpan.FromSeconds(1.0) : timeSpan;
    }
  }

  private void ResolveKeyTimes()
  {
    int length = 0;
    if (this._keyFrames != null)
      length = this._keyFrames.Count;
    if (length == 0)
    {
      this._sortedResolvedKeyFrames = (ResolvedKeyFrameEntry[]) null;
      this._areKeyTimesValid = true;
    }
    else
    {
      this._sortedResolvedKeyFrames = new ResolvedKeyFrameEntry[length];
      for (int index = 0; index < length; ++index)
        this._sortedResolvedKeyFrames[index]._originalKeyFrameIndex = index;
      Duration duration = this.Duration;
      TimeSpan timeSpan1 = duration.HasTimeSpan ? duration.TimeSpan : this.LargestTimeSpanKeyTime;
      int num1 = length - 1;
      List<GeometryAnimationUsingKeyFrames.KeyTimeBlock> keyTimeBlockList = new List<GeometryAnimationUsingKeyFrames.KeyTimeBlock>();
      bool flag = false;
      int index1 = 0;
      while (index1 < length)
      {
        KeyTime keyTime = this._keyFrames[index1].KeyTime;
        switch (keyTime.Type)
        {
          case KeyTimeType.Uniform:
          case KeyTimeType.Paced:
            if (index1 == num1)
            {
              this._sortedResolvedKeyFrames[index1]._resolvedKeyTime = timeSpan1;
              ++index1;
              continue;
            }
            if (index1 == 0 && keyTime.Type == KeyTimeType.Paced)
            {
              this._sortedResolvedKeyFrames[index1]._resolvedKeyTime = TimeSpan.Zero;
              ++index1;
              continue;
            }
            if (keyTime.Type == KeyTimeType.Paced)
              flag = true;
            GeometryAnimationUsingKeyFrames.KeyTimeBlock keyTimeBlock = new GeometryAnimationUsingKeyFrames.KeyTimeBlock()
            {
              BeginIndex = index1
            };
            while (++index1 < num1)
            {
              switch (this._keyFrames[index1].KeyTime.Type)
              {
                case KeyTimeType.Percent:
                case KeyTimeType.TimeSpan:
                  goto label_21;
                case KeyTimeType.Paced:
                  flag = true;
                  continue;
                default:
                  continue;
              }
            }
label_21:
            keyTimeBlock.EndIndex = index1;
            keyTimeBlockList.Add(keyTimeBlock);
            continue;
          case KeyTimeType.Percent:
            this._sortedResolvedKeyFrames[index1]._resolvedKeyTime = TimeSpan.FromMilliseconds(keyTime.Percent * timeSpan1.TotalMilliseconds);
            ++index1;
            continue;
          case KeyTimeType.TimeSpan:
            this._sortedResolvedKeyFrames[index1]._resolvedKeyTime = keyTime.TimeSpan;
            ++index1;
            continue;
          default:
            continue;
        }
      }
      foreach (GeometryAnimationUsingKeyFrames.KeyTimeBlock keyTimeBlock in keyTimeBlockList)
      {
        TimeSpan timeSpan2 = TimeSpan.Zero;
        if (keyTimeBlock.BeginIndex > 0)
          timeSpan2 = this._sortedResolvedKeyFrames[keyTimeBlock.BeginIndex - 1]._resolvedKeyTime;
        long num2 = (long) (keyTimeBlock.EndIndex - keyTimeBlock.BeginIndex + 1);
        TimeSpan timeSpan3 = TimeSpan.FromTicks((this._sortedResolvedKeyFrames[keyTimeBlock.EndIndex]._resolvedKeyTime - timeSpan2).Ticks / num2);
        int beginIndex = keyTimeBlock.BeginIndex;
        TimeSpan timeSpan4 = timeSpan2 + timeSpan3;
        for (; beginIndex < keyTimeBlock.EndIndex; ++beginIndex)
        {
          this._sortedResolvedKeyFrames[beginIndex]._resolvedKeyTime = timeSpan4;
          timeSpan4 += timeSpan3;
        }
      }
      if (flag)
        this.ResolvePacedKeyTimes();
      Array.Sort<ResolvedKeyFrameEntry>(this._sortedResolvedKeyFrames);
      this._areKeyTimesValid = true;
    }
  }

  private void ResolvePacedKeyTimes()
  {
    int index1 = 1;
    int num1 = this._sortedResolvedKeyFrames.Length - 1;
    do
    {
      if (this._keyFrames[index1].KeyTime.Type == KeyTimeType.Paced)
      {
        int num2 = index1;
        List<double> doubleList = new List<double>();
        TimeSpan resolvedKeyTime = this._sortedResolvedKeyFrames[index1 - 1]._resolvedKeyTime;
        double num3 = 0.0;
        double[] numArray = this._keyFrames[index1 - 1].Numbers;
        do
        {
          double[] numbers = this._keyFrames[index1].Numbers;
          num3 += Math.Abs(numbers[0] - numArray[0]);
          doubleList.Add(num3);
          numArray = numbers;
          ++index1;
        }
        while (index1 < num1 && this._keyFrames[index1].KeyTime.Type == KeyTimeType.Paced);
        double num4 = num3 + Math.Abs(this._keyFrames[index1].Numbers[0] - numArray[0]);
        TimeSpan timeSpan = this._sortedResolvedKeyFrames[index1]._resolvedKeyTime - resolvedKeyTime;
        int index2 = 0;
        int index3 = num2;
        while (index2 < doubleList.Count)
        {
          this._sortedResolvedKeyFrames[index3]._resolvedKeyTime = resolvedKeyTime + TimeSpan.FromMilliseconds(doubleList[index2] / num4 * timeSpan.TotalMilliseconds);
          ++index2;
          ++index3;
        }
      }
      else
        ++index1;
    }
    while (index1 < num1);
  }

  private struct KeyTimeBlock
  {
    public int BeginIndex;
    public int EndIndex;
  }
}
