// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.XyzDataSeries3D
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public abstract class XyzDataSeries3D : XyDataSeries3D
{
  public static readonly DependencyProperty ZBindingPathProperty = DependencyProperty.Register(nameof (ZBindingPath), typeof (string), typeof (ChartSeries3D), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(XyzDataSeries3D.OnBindingPathZChanged)));
  private ChartValueType zValueType;

  public XyzDataSeries3D() => this.ZValues = this.ActualZValues = (IEnumerable) new List<double>();

  public DoubleRange ZRange { get; internal set; }

  public string ZBindingPath
  {
    get => (string) this.GetValue(XyzDataSeries3D.ZBindingPathProperty);
    set => this.SetValue(XyzDataSeries3D.ZBindingPathProperty, (object) value);
  }

  internal string[] ZComplexPaths { get; set; }

  internal DoubleRange ZSideBySideInfoRangePad { get; set; }

  internal ChartValueType ZAxisValueType
  {
    get => this.zValueType;
    set => this.zValueType = value;
  }

  protected internal bool IsIndexedZAxis
  {
    get => this.ActualZAxis is CategoryAxis3D || this.ActualZAxis is DateTimeCategoryAxis;
  }

  protected internal ChartAxis ActualZAxis
  {
    get
    {
      return this.ActualArea == null || this == null ? (ChartAxis) null : this.ActualArea.InternalDepthAxis;
    }
  }

  protected internal IEnumerable ActualZValues { get; set; }

  protected internal IEnumerable ZValues { get; set; }

  internal override void GenerateDataTablePoints(string[] yPaths, IList<double>[] yLists)
  {
    XyzDataSeries3D xyzDataSeries3D = this;
    if (xyzDataSeries3D == null || xyzDataSeries3D != null && (xyzDataSeries3D.ZBindingPath == null || xyzDataSeries3D.ZBindingPath.Length == 0))
    {
      base.GenerateDataTablePoints(yPaths, yLists);
    }
    else
    {
      IEnumerator enumerator = (this.ItemsSource as DataTable).Rows.GetEnumerator();
      if (enumerator.MoveNext())
      {
        for (int index = 0; index < this.UpdateStartedIndex; ++index)
          enumerator.MoveNext();
        this.XAxisValueType = ChartSeriesBase.GetDataType((enumerator.Current as DataRow).Field<object>(this.XBindingPath));
        if (this.XAxisValueType == ChartValueType.DateTime || this.XAxisValueType == ChartValueType.Double || this.XAxisValueType == ChartValueType.Logarithmic || this.XAxisValueType == ChartValueType.TimeSpan)
        {
          if (!(this.XValues is List<double>))
            this.ActualXValues = this.XValues = (IEnumerable) new List<double>();
        }
        else if (!(this.XValues is List<string>))
          this.ActualXValues = this.XValues = (IEnumerable) new List<string>();
        if (this.ZAxisValueType == ChartValueType.DateTime || this.ZAxisValueType == ChartValueType.Double || this.ZAxisValueType == ChartValueType.Logarithmic || this.ZAxisValueType == ChartValueType.TimeSpan)
        {
          if (!(this.ZValues is List<double>))
            this.ActualZValues = this.ZValues = (IEnumerable) new List<double>();
        }
        else if (!(this.ZValues is List<string>))
          this.ActualZValues = this.ZValues = (IEnumerable) new List<string>();
        if (string.IsNullOrEmpty(yPaths[0]))
          return;
        IList<double> yList = yLists[0];
        List<double> zvalues1 = this.ZValues as List<double>;
        if (this.XAxisValueType == ChartValueType.String)
        {
          IList<string> xvalues = (IList<string>) (this.XValues as List<string>);
          switch (this.ZAxisValueType)
          {
            case ChartValueType.Double:
              do
              {
                object obj1 = (enumerator.Current as DataRow).Field<object>(this.XBindingPath);
                object obj2 = (enumerator.Current as DataRow).Field<object>(yPaths[0]);
                object obj3 = (enumerator.Current as DataRow).Field<object>(xyzDataSeries3D.ZBindingPath);
                xvalues.Add((string) obj1);
                yList.Add(Convert.ToDouble(obj2 ?? (object) double.NaN));
                zvalues1.Add(Convert.ToDouble(obj3));
                this.ActualData.Add(enumerator.Current);
              }
              while (enumerator.MoveNext());
              this.DataCount = xvalues.Count;
              break;
            case ChartValueType.DateTime:
              do
              {
                object obj4 = (enumerator.Current as DataRow).Field<object>(this.XBindingPath);
                object obj5 = (enumerator.Current as DataRow).Field<object>(yPaths[0]);
                object obj6 = (enumerator.Current as DataRow).Field<object>(xyzDataSeries3D.ZBindingPath);
                xvalues.Add((string) obj4);
                yList.Add(Convert.ToDouble(obj5 ?? (object) double.NaN));
                zvalues1.Add(((DateTime) obj6).ToOADate());
                this.ActualData.Add(enumerator.Current);
              }
              while (enumerator.MoveNext());
              this.DataCount = xvalues.Count;
              break;
            case ChartValueType.TimeSpan:
              do
              {
                object obj7 = (enumerator.Current as DataRow).Field<object>(this.XBindingPath);
                object obj8 = (enumerator.Current as DataRow).Field<object>(yPaths[0]);
                object obj9 = (enumerator.Current as DataRow).Field<object>(xyzDataSeries3D.ZBindingPath);
                xvalues.Add((string) obj7);
                yList.Add(Convert.ToDouble(obj8 ?? (object) double.NaN));
                zvalues1.Add(((TimeSpan) obj9).TotalMilliseconds);
                this.ActualData.Add(enumerator.Current);
              }
              while (enumerator.MoveNext());
              this.DataCount = xvalues.Count;
              break;
            default:
              object zvalues2 = (object) this.ZValues;
              do
              {
                object obj10 = (enumerator.Current as DataRow).Field<object>(this.XBindingPath);
                object obj11 = (enumerator.Current as DataRow).Field<object>(yPaths[0]);
                object obj12 = (enumerator.Current as DataRow).Field<object>(xyzDataSeries3D.ZBindingPath);
                xvalues.Add((string) obj10);
                yList.Add(Convert.ToDouble(obj11 ?? (object) double.NaN));
                (zvalues2 as List<string>).Add((string) obj12);
                this.ActualData.Add(enumerator.Current);
              }
              while (enumerator.MoveNext());
              this.DataCount = xvalues.Count;
              break;
          }
        }
        else if (this.XAxisValueType == ChartValueType.Double || this.XAxisValueType == ChartValueType.Logarithmic)
        {
          IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
          switch (this.ZAxisValueType)
          {
            case ChartValueType.Double:
              do
              {
                object obj13 = (enumerator.Current as DataRow).Field<object>(this.XBindingPath);
                object obj14 = (enumerator.Current as DataRow).Field<object>(yPaths[0]);
                object obj15 = (enumerator.Current as DataRow).Field<object>(xyzDataSeries3D.ZBindingPath);
                this.XData = Convert.ToDouble(obj13 ?? (object) double.NaN);
                if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                  this.isLinearData = false;
                xvalues.Add(this.XData);
                yList.Add(Convert.ToDouble(obj14 ?? (object) double.NaN));
                zvalues1.Add(Convert.ToDouble(obj15));
                this.ActualData.Add(enumerator.Current);
              }
              while (enumerator.MoveNext());
              this.DataCount = xvalues.Count;
              break;
            case ChartValueType.DateTime:
              do
              {
                object obj16 = (enumerator.Current as DataRow).Field<object>(this.XBindingPath);
                object obj17 = (enumerator.Current as DataRow).Field<object>(yPaths[0]);
                object obj18 = (enumerator.Current as DataRow).Field<object>(xyzDataSeries3D.ZBindingPath);
                this.XData = Convert.ToDouble(obj16 ?? (object) double.NaN);
                if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                  this.isLinearData = false;
                xvalues.Add(this.XData);
                yList.Add(Convert.ToDouble(obj17 ?? (object) double.NaN));
                zvalues1.Add(((DateTime) obj18).ToOADate());
                this.ActualData.Add(enumerator.Current);
              }
              while (enumerator.MoveNext());
              this.DataCount = xvalues.Count;
              break;
            case ChartValueType.TimeSpan:
              do
              {
                object obj19 = (enumerator.Current as DataRow).Field<object>(this.XBindingPath);
                object obj20 = (enumerator.Current as DataRow).Field<object>(yPaths[0]);
                object obj21 = (enumerator.Current as DataRow).Field<object>(xyzDataSeries3D.ZBindingPath);
                this.XData = Convert.ToDouble(obj19 ?? (object) double.NaN);
                if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                  this.isLinearData = false;
                xvalues.Add(this.XData);
                yList.Add(Convert.ToDouble(obj20 ?? (object) double.NaN));
                zvalues1.Add(((TimeSpan) obj21).TotalMilliseconds);
                this.ActualData.Add(enumerator.Current);
              }
              while (enumerator.MoveNext());
              this.DataCount = xvalues.Count;
              break;
            default:
              object zvalues3 = (object) this.ZValues;
              do
              {
                object obj22 = (enumerator.Current as DataRow).Field<object>(this.XBindingPath);
                object obj23 = (enumerator.Current as DataRow).Field<object>(yPaths[0]);
                object obj24 = (enumerator.Current as DataRow).Field<object>(xyzDataSeries3D.ZBindingPath);
                this.XData = Convert.ToDouble(obj22 ?? (object) double.NaN);
                if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                  this.isLinearData = false;
                xvalues.Add(this.XData);
                yList.Add(Convert.ToDouble(obj23 ?? (object) double.NaN));
                (zvalues3 as List<string>).Add((string) obj24);
                this.ActualData.Add(enumerator.Current);
              }
              while (enumerator.MoveNext());
              this.DataCount = xvalues.Count;
              break;
          }
        }
        else if (this.XAxisValueType == ChartValueType.DateTime)
        {
          IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
          switch (this.ZAxisValueType)
          {
            case ChartValueType.Double:
              do
              {
                object obj25 = (enumerator.Current as DataRow).Field<object>(this.XBindingPath);
                object obj26 = (enumerator.Current as DataRow).Field<object>(yPaths[0]);
                object obj27 = (enumerator.Current as DataRow).Field<object>(xyzDataSeries3D.ZBindingPath);
                this.XData = ((DateTime) obj25).ToOADate();
                if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                  this.isLinearData = false;
                xvalues.Add(this.XData);
                yList.Add(Convert.ToDouble(obj26 ?? (object) double.NaN));
                zvalues1.Add(Convert.ToDouble(obj27));
                this.ActualData.Add(enumerator.Current);
              }
              while (enumerator.MoveNext());
              this.DataCount = xvalues.Count;
              break;
            case ChartValueType.DateTime:
              do
              {
                object obj28 = (enumerator.Current as DataRow).Field<object>(this.XBindingPath);
                object obj29 = (enumerator.Current as DataRow).Field<object>(yPaths[0]);
                object obj30 = (enumerator.Current as DataRow).Field<object>(xyzDataSeries3D.ZBindingPath);
                this.XData = ((DateTime) obj28).ToOADate();
                if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                  this.isLinearData = false;
                xvalues.Add(this.XData);
                yList.Add(Convert.ToDouble(obj29 ?? (object) double.NaN));
                zvalues1.Add(((DateTime) obj30).ToOADate());
                this.ActualData.Add(enumerator.Current);
              }
              while (enumerator.MoveNext());
              this.DataCount = xvalues.Count;
              break;
            case ChartValueType.TimeSpan:
              do
              {
                object obj31 = (enumerator.Current as DataRow).Field<object>(this.XBindingPath);
                object obj32 = (enumerator.Current as DataRow).Field<object>(yPaths[0]);
                object obj33 = (enumerator.Current as DataRow).Field<object>(xyzDataSeries3D.ZBindingPath);
                this.XData = ((DateTime) obj31).ToOADate();
                if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                  this.isLinearData = false;
                xvalues.Add(this.XData);
                yList.Add(Convert.ToDouble(obj32 ?? (object) double.NaN));
                zvalues1.Add(((TimeSpan) obj33).TotalMilliseconds);
                this.ActualData.Add(enumerator.Current);
              }
              while (enumerator.MoveNext());
              this.DataCount = xvalues.Count;
              break;
            default:
              object zvalues4 = (object) this.ZValues;
              do
              {
                object obj34 = (enumerator.Current as DataRow).Field<object>(this.XBindingPath);
                object obj35 = (enumerator.Current as DataRow).Field<object>(yPaths[0]);
                object obj36 = (enumerator.Current as DataRow).Field<object>(xyzDataSeries3D.ZBindingPath);
                this.XData = ((DateTime) obj34).ToOADate();
                if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                  this.isLinearData = false;
                xvalues.Add(this.XData);
                yList.Add(Convert.ToDouble(obj35 ?? (object) double.NaN));
                (zvalues4 as List<string>).Add((string) obj36);
                this.ActualData.Add(enumerator.Current);
              }
              while (enumerator.MoveNext());
              this.DataCount = xvalues.Count;
              break;
          }
        }
        else if (this.XAxisValueType == ChartValueType.TimeSpan)
        {
          IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
          switch (this.ZAxisValueType)
          {
            case ChartValueType.Double:
              do
              {
                object obj37 = (enumerator.Current as DataRow).Field<object>(this.XBindingPath);
                object obj38 = (enumerator.Current as DataRow).Field<object>(yPaths[0]);
                object obj39 = (enumerator.Current as DataRow).Field<object>(xyzDataSeries3D.ZBindingPath);
                this.XData = ((TimeSpan) obj37).TotalMilliseconds;
                if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                  this.isLinearData = false;
                xvalues.Add(((TimeSpan) obj37).TotalMilliseconds);
                yList.Add(Convert.ToDouble(obj38 ?? (object) double.NaN));
                zvalues1.Add(Convert.ToDouble(obj39));
                this.ActualData.Add(enumerator.Current);
              }
              while (enumerator.MoveNext());
              this.DataCount = xvalues.Count;
              break;
            case ChartValueType.DateTime:
              do
              {
                object obj40 = (enumerator.Current as DataRow).Field<object>(this.XBindingPath);
                object obj41 = (enumerator.Current as DataRow).Field<object>(yPaths[0]);
                object obj42 = (enumerator.Current as DataRow).Field<object>(xyzDataSeries3D.ZBindingPath);
                this.XData = ((TimeSpan) obj40).TotalMilliseconds;
                if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                  this.isLinearData = false;
                xvalues.Add(((TimeSpan) obj40).TotalMilliseconds);
                yList.Add(Convert.ToDouble(obj41 ?? (object) double.NaN));
                zvalues1.Add(((DateTime) obj42).ToOADate());
                this.ActualData.Add(enumerator.Current);
              }
              while (enumerator.MoveNext());
              this.DataCount = xvalues.Count;
              break;
            case ChartValueType.TimeSpan:
              do
              {
                object obj43 = (enumerator.Current as DataRow).Field<object>(this.XBindingPath);
                object obj44 = (enumerator.Current as DataRow).Field<object>(yPaths[0]);
                object obj45 = (enumerator.Current as DataRow).Field<object>(xyzDataSeries3D.ZBindingPath);
                this.XData = ((TimeSpan) obj43).TotalMilliseconds;
                if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                  this.isLinearData = false;
                xvalues.Add(((TimeSpan) obj43).TotalMilliseconds);
                yList.Add(Convert.ToDouble(obj44 ?? (object) double.NaN));
                zvalues1.Add(((TimeSpan) obj45).TotalMilliseconds);
                this.ActualData.Add(enumerator.Current);
              }
              while (enumerator.MoveNext());
              this.DataCount = xvalues.Count;
              break;
            default:
              object zvalues5 = (object) this.ZValues;
              do
              {
                object obj46 = (enumerator.Current as DataRow).Field<object>(this.XBindingPath);
                object obj47 = (enumerator.Current as DataRow).Field<object>(yPaths[0]);
                object obj48 = (enumerator.Current as DataRow).Field<object>(xyzDataSeries3D.ZBindingPath);
                this.XData = ((TimeSpan) obj46).TotalMilliseconds;
                if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                  this.isLinearData = false;
                xvalues.Add(((TimeSpan) obj46).TotalMilliseconds);
                yList.Add(Convert.ToDouble(obj47 ?? (object) double.NaN));
                (zvalues5 as List<string>).Add((string) obj48);
                this.ActualData.Add(enumerator.Current);
              }
              while (enumerator.MoveNext());
              this.DataCount = xvalues.Count;
              break;
          }
        }
      }
      this.IsPointGenerated = true;
    }
  }

  internal override void GeneratePropertyPoints(string[] yPaths, IList<double>[] yLists)
  {
    XyzDataSeries3D xyzDataSeries3D = this;
    if (xyzDataSeries3D == null || xyzDataSeries3D != null && (xyzDataSeries3D.ZBindingPath == null || xyzDataSeries3D.ZBindingPath.Length == 0))
    {
      base.GeneratePropertyPoints(yPaths, yLists);
    }
    else
    {
      IEnumerator enumerator = (this.ItemsSource as IEnumerable).GetEnumerator();
      if (enumerator.MoveNext())
      {
        if (enumerator.Current is ICustomTypeDescriptor)
        {
          this.GenerateCustomTypeDescriptorPropertyPoints(yPaths, yLists, enumerator);
        }
        else
        {
          for (int index = 0; index < this.UpdateStartedIndex; ++index)
            enumerator.MoveNext();
          PropertyInfo propertyInfo1 = ChartDataUtils.GetPropertyInfo(enumerator.Current, this.XBindingPath);
          PropertyInfo propertyInfo2 = xyzDataSeries3D.ZBindingPath != null ? ChartDataUtils.GetPropertyInfo(enumerator.Current, xyzDataSeries3D.ZBindingPath) : (PropertyInfo) null;
          IPropertyAccessor propertyAccessor1 = (IPropertyAccessor) null;
          IPropertyAccessor propertyAccessor2 = (IPropertyAccessor) null;
          if (propertyInfo1 != (PropertyInfo) null)
            propertyAccessor1 = FastReflectionCaches.PropertyAccessorCache.Get(propertyInfo1);
          if (propertyInfo2 != (PropertyInfo) null)
            propertyAccessor2 = FastReflectionCaches.PropertyAccessorCache.Get(propertyInfo2);
          if (propertyAccessor1 == null || propertyAccessor2 == null)
            return;
          System.Func<object, object> getMethod1 = propertyAccessor1.GetMethod;
          System.Func<object, object> getMethod2 = propertyAccessor2.GetMethod;
          if (getMethod1(enumerator.Current) != null && getMethod1(enumerator.Current).GetType().IsArray)
            return;
          this.XAxisValueType = ChartSeriesBase.GetDataType(propertyAccessor1, this.ItemsSource as IEnumerable);
          if (propertyAccessor2 != null)
            this.ZAxisValueType = ChartSeriesBase.GetDataType(propertyAccessor2, this.ItemsSource as IEnumerable);
          if (this.XAxisValueType == ChartValueType.DateTime || this.XAxisValueType == ChartValueType.Double || this.XAxisValueType == ChartValueType.Logarithmic || this.XAxisValueType == ChartValueType.TimeSpan)
          {
            if (!(this.ActualXValues is List<double>))
              this.ActualXValues = this.XValues = (IEnumerable) new List<double>();
          }
          else if (!(this.ActualXValues is List<string>))
            this.ActualXValues = this.XValues = (IEnumerable) new List<string>();
          if (this.ZAxisValueType == ChartValueType.DateTime || this.ZAxisValueType == ChartValueType.Double || this.ZAxisValueType == ChartValueType.Logarithmic || this.ZAxisValueType == ChartValueType.TimeSpan)
          {
            if (!(this.ActualZValues is List<double>))
              this.ActualZValues = this.ZValues = (IEnumerable) new List<double>();
          }
          else if (!(this.ActualZValues is List<string>))
            this.ActualZValues = this.ZValues = (IEnumerable) new List<string>();
          if (string.IsNullOrEmpty(yPaths[0]))
            return;
          PropertyInfo propertyInfo3 = ChartDataUtils.GetPropertyInfo(enumerator.Current, yPaths[0]);
          IPropertyAccessor propertyAccessor3 = (IPropertyAccessor) null;
          if (propertyInfo3 != (PropertyInfo) null)
            propertyAccessor3 = FastReflectionCaches.PropertyAccessorCache.Get(propertyInfo3);
          if (propertyAccessor3 == null)
            return;
          IList<double> yList = yLists[0];
          if (propertyAccessor3 == null)
            return;
          System.Func<object, object> getMethod3 = propertyAccessor3.GetMethod;
          if (getMethod3(enumerator.Current) != null && getMethod3(enumerator.Current).GetType().IsArray)
            return;
          List<double> zvalues1 = this.ZValues as List<double>;
          if (this.XAxisValueType == ChartValueType.String)
          {
            IList<string> xvalues = (IList<string>) (this.XValues as List<string>);
            switch (this.ZAxisValueType)
            {
              case ChartValueType.Double:
                do
                {
                  object obj1 = getMethod1(enumerator.Current);
                  object obj2 = getMethod3(enumerator.Current);
                  object obj3 = getMethod2(enumerator.Current);
                  xvalues.Add(obj1 != null ? (string) obj1 : string.Empty);
                  yList.Add(Convert.ToDouble(obj2 ?? (object) double.NaN));
                  zvalues1.Add(Convert.ToDouble(obj3));
                  this.ActualData.Add(enumerator.Current);
                }
                while (enumerator.MoveNext());
                this.DataCount = xvalues.Count;
                break;
              case ChartValueType.DateTime:
                do
                {
                  object obj4 = getMethod1(enumerator.Current);
                  object obj5 = getMethod3(enumerator.Current);
                  object obj6 = getMethod2(enumerator.Current);
                  xvalues.Add(obj4 != null ? (string) obj4 : string.Empty);
                  yList.Add(Convert.ToDouble(obj5 ?? (object) double.NaN));
                  zvalues1.Add(((DateTime) obj6).ToOADate());
                  this.ActualData.Add(enumerator.Current);
                }
                while (enumerator.MoveNext());
                this.DataCount = xvalues.Count;
                break;
              case ChartValueType.TimeSpan:
                do
                {
                  object obj7 = getMethod1(enumerator.Current);
                  object obj8 = getMethod3(enumerator.Current);
                  object obj9 = getMethod2(enumerator.Current);
                  xvalues.Add(obj7 != null ? (string) obj7 : string.Empty);
                  yList.Add(Convert.ToDouble(obj8 ?? (object) double.NaN));
                  zvalues1.Add(((TimeSpan) obj9).TotalMilliseconds);
                  this.ActualData.Add(enumerator.Current);
                }
                while (enumerator.MoveNext());
                this.DataCount = xvalues.Count;
                break;
              default:
                object zvalues2 = (object) this.ZValues;
                do
                {
                  object obj10 = getMethod1(enumerator.Current);
                  object obj11 = getMethod3(enumerator.Current);
                  object obj12 = getMethod2(enumerator.Current);
                  xvalues.Add(obj10 != null ? (string) obj10 : string.Empty);
                  yList.Add(Convert.ToDouble(obj11 ?? (object) double.NaN));
                  (zvalues2 as List<string>).Add((string) obj12);
                  this.ActualData.Add(enumerator.Current);
                }
                while (enumerator.MoveNext());
                this.DataCount = xvalues.Count;
                break;
            }
          }
          else if (this.XAxisValueType == ChartValueType.Double || this.XAxisValueType == ChartValueType.Logarithmic)
          {
            IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
            switch (this.ZAxisValueType)
            {
              case ChartValueType.Double:
                do
                {
                  object obj13 = getMethod1(enumerator.Current);
                  object obj14 = getMethod3(enumerator.Current);
                  object obj15 = getMethod2(enumerator.Current);
                  this.XData = Convert.ToDouble(obj13 ?? (object) double.NaN);
                  if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                    this.isLinearData = false;
                  xvalues.Add(this.XData);
                  yList.Add(Convert.ToDouble(obj14 ?? (object) double.NaN));
                  zvalues1.Add(Convert.ToDouble(obj15));
                  this.ActualData.Add(enumerator.Current);
                }
                while (enumerator.MoveNext());
                this.DataCount = xvalues.Count;
                break;
              case ChartValueType.DateTime:
                do
                {
                  object obj16 = getMethod1(enumerator.Current);
                  object obj17 = getMethod3(enumerator.Current);
                  object obj18 = getMethod2(enumerator.Current);
                  this.XData = Convert.ToDouble(obj16 ?? (object) double.NaN);
                  if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                    this.isLinearData = false;
                  xvalues.Add(this.XData);
                  yList.Add(Convert.ToDouble(obj17 ?? (object) double.NaN));
                  zvalues1.Add(((DateTime) obj18).ToOADate());
                  this.ActualData.Add(enumerator.Current);
                }
                while (enumerator.MoveNext());
                this.DataCount = xvalues.Count;
                break;
              case ChartValueType.TimeSpan:
                do
                {
                  object obj19 = getMethod1(enumerator.Current);
                  object obj20 = getMethod3(enumerator.Current);
                  object obj21 = getMethod2(enumerator.Current);
                  this.XData = Convert.ToDouble(obj19 ?? (object) double.NaN);
                  if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                    this.isLinearData = false;
                  xvalues.Add(this.XData);
                  yList.Add(Convert.ToDouble(obj20 ?? (object) double.NaN));
                  zvalues1.Add(((TimeSpan) obj21).TotalMilliseconds);
                  this.ActualData.Add(enumerator.Current);
                }
                while (enumerator.MoveNext());
                this.DataCount = xvalues.Count;
                break;
              default:
                object zvalues3 = (object) this.ZValues;
                do
                {
                  object obj22 = getMethod1(enumerator.Current);
                  object obj23 = getMethod3(enumerator.Current);
                  object obj24 = getMethod2(enumerator.Current);
                  this.XData = Convert.ToDouble(obj22 ?? (object) double.NaN);
                  if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                    this.isLinearData = false;
                  xvalues.Add(this.XData);
                  yList.Add(Convert.ToDouble(obj23 ?? (object) double.NaN));
                  (zvalues3 as List<string>).Add((string) obj24);
                  this.ActualData.Add(enumerator.Current);
                }
                while (enumerator.MoveNext());
                this.DataCount = xvalues.Count;
                break;
            }
          }
          else if (this.XAxisValueType == ChartValueType.DateTime)
          {
            IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
            switch (this.ZAxisValueType)
            {
              case ChartValueType.Double:
                do
                {
                  object obj25 = getMethod1(enumerator.Current);
                  object obj26 = getMethod3(enumerator.Current);
                  object obj27 = getMethod2(enumerator.Current);
                  this.XData = ((DateTime) obj25).ToOADate();
                  if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                    this.isLinearData = false;
                  xvalues.Add(this.XData);
                  yList.Add(Convert.ToDouble(obj26 ?? (object) double.NaN));
                  zvalues1.Add(Convert.ToDouble(obj27));
                  this.ActualData.Add(enumerator.Current);
                }
                while (enumerator.MoveNext());
                this.DataCount = xvalues.Count;
                break;
              case ChartValueType.DateTime:
                do
                {
                  object obj28 = getMethod1(enumerator.Current);
                  object obj29 = getMethod3(enumerator.Current);
                  object obj30 = getMethod2(enumerator.Current);
                  this.XData = ((DateTime) obj28).ToOADate();
                  if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                    this.isLinearData = false;
                  xvalues.Add(this.XData);
                  yList.Add(Convert.ToDouble(obj29 ?? (object) double.NaN));
                  zvalues1.Add(((DateTime) obj30).ToOADate());
                  this.ActualData.Add(enumerator.Current);
                }
                while (enumerator.MoveNext());
                this.DataCount = xvalues.Count;
                break;
              case ChartValueType.TimeSpan:
                do
                {
                  object obj31 = getMethod1(enumerator.Current);
                  object obj32 = getMethod3(enumerator.Current);
                  object obj33 = getMethod2(enumerator.Current);
                  this.XData = ((DateTime) obj31).ToOADate();
                  if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                    this.isLinearData = false;
                  xvalues.Add(this.XData);
                  yList.Add(Convert.ToDouble(obj32 ?? (object) double.NaN));
                  zvalues1.Add(((TimeSpan) obj33).TotalMilliseconds);
                  this.ActualData.Add(enumerator.Current);
                }
                while (enumerator.MoveNext());
                this.DataCount = xvalues.Count;
                break;
              default:
                object zvalues4 = (object) this.ZValues;
                do
                {
                  object obj34 = getMethod1(enumerator.Current);
                  object obj35 = getMethod3(enumerator.Current);
                  object obj36 = getMethod2(enumerator.Current);
                  this.XData = ((DateTime) obj34).ToOADate();
                  if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                    this.isLinearData = false;
                  xvalues.Add(this.XData);
                  yList.Add(Convert.ToDouble(obj35 ?? (object) double.NaN));
                  (zvalues4 as List<string>).Add((string) obj36);
                  this.ActualData.Add(enumerator.Current);
                }
                while (enumerator.MoveNext());
                this.DataCount = xvalues.Count;
                break;
            }
          }
          else if (this.XAxisValueType == ChartValueType.TimeSpan)
          {
            IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
            switch (this.ZAxisValueType)
            {
              case ChartValueType.Double:
                do
                {
                  object obj37 = getMethod1(enumerator.Current);
                  object obj38 = getMethod3(enumerator.Current);
                  object obj39 = getMethod2(enumerator.Current);
                  this.XData = ((TimeSpan) obj37).TotalMilliseconds;
                  if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                    this.isLinearData = false;
                  xvalues.Add(this.XData);
                  yList.Add(Convert.ToDouble(obj38 ?? (object) double.NaN));
                  zvalues1.Add(Convert.ToDouble(obj39));
                  this.ActualData.Add(enumerator.Current);
                }
                while (enumerator.MoveNext());
                this.DataCount = xvalues.Count;
                break;
              case ChartValueType.DateTime:
                do
                {
                  object obj40 = getMethod1(enumerator.Current);
                  object obj41 = getMethod3(enumerator.Current);
                  object obj42 = getMethod2(enumerator.Current);
                  this.XData = ((TimeSpan) obj40).TotalMilliseconds;
                  if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                    this.isLinearData = false;
                  xvalues.Add(this.XData);
                  yList.Add(Convert.ToDouble(obj41 ?? (object) double.NaN));
                  zvalues1.Add(((DateTime) obj42).ToOADate());
                  this.ActualData.Add(enumerator.Current);
                }
                while (enumerator.MoveNext());
                this.DataCount = xvalues.Count;
                break;
              case ChartValueType.TimeSpan:
                do
                {
                  object obj43 = getMethod1(enumerator.Current);
                  object obj44 = getMethod3(enumerator.Current);
                  object obj45 = getMethod2(enumerator.Current);
                  this.XData = ((TimeSpan) obj43).TotalMilliseconds;
                  if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                    this.isLinearData = false;
                  xvalues.Add(this.XData);
                  yList.Add(Convert.ToDouble(obj44 ?? (object) double.NaN));
                  zvalues1.Add(((TimeSpan) obj45).TotalMilliseconds);
                  this.ActualData.Add(enumerator.Current);
                }
                while (enumerator.MoveNext());
                this.DataCount = xvalues.Count;
                break;
              default:
                object zvalues5 = (object) this.ZValues;
                do
                {
                  object obj46 = getMethod1(enumerator.Current);
                  object obj47 = getMethod3(enumerator.Current);
                  object obj48 = getMethod2(enumerator.Current);
                  this.XData = ((TimeSpan) obj46).TotalMilliseconds;
                  if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                    this.isLinearData = false;
                  xvalues.Add(this.XData);
                  yList.Add(Convert.ToDouble(obj47 ?? (object) double.NaN));
                  (zvalues5 as List<string>).Add((string) obj48);
                  this.ActualData.Add(enumerator.Current);
                }
                while (enumerator.MoveNext());
                this.DataCount = xvalues.Count;
                break;
            }
          }
          this.HookPropertyChangedEvent(this.ListenPropertyChange);
        }
      }
      this.IsPointGenerated = true;
    }
  }

  internal override void GenerateCustomTypeDescriptorPropertyPoints(
    string[] yPaths,
    IList<double>[] yLists,
    IEnumerator enumerator)
  {
    XyzDataSeries3D xyzDataSeries3D = this;
    if (xyzDataSeries3D == null || xyzDataSeries3D != null && (xyzDataSeries3D.ZBindingPath == null || xyzDataSeries3D.ZBindingPath.Length == 0))
    {
      base.GeneratePropertyPoints(yPaths, yLists);
    }
    else
    {
      PropertyDescriptorCollection properties1 = (enumerator.Current as ICustomTypeDescriptor).GetProperties();
      PropertyDescriptor propertyDescriptor = properties1.Find(this.XBindingPath, false);
      if (propertyDescriptor == null)
        return;
      for (int index = 0; index < ((IEnumerable<string>) yPaths).Count<string>(); ++index)
      {
        if (properties1.Find(yPaths[index], false) == null)
          return;
      }
      object obj1 = propertyDescriptor.GetValue((object) this.XBindingPath);
      double result1;
      DateTime result2;
      TimeSpan result3;
      if (double.TryParse(obj1.ToString(), out result1))
        this.XAxisValueType = ChartValueType.Double;
      else if (DateTime.TryParse(obj1.ToString(), out result2))
        this.XAxisValueType = ChartValueType.DateTime;
      else if (TimeSpan.TryParse(obj1.ToString(), out result3))
        this.XAxisValueType = ChartValueType.TimeSpan;
      else
        this.XAxisValueType = ChartValueType.String;
      this.ZAxisValueType = !double.TryParse(obj1.ToString(), out result1) ? (!DateTime.TryParse(obj1.ToString(), out result2) ? (!TimeSpan.TryParse(obj1.ToString(), out result3) ? ChartValueType.String : ChartValueType.TimeSpan) : ChartValueType.DateTime) : ChartValueType.Double;
      if (this.ZAxisValueType == ChartValueType.DateTime || this.ZAxisValueType == ChartValueType.Double || this.ZAxisValueType == ChartValueType.Logarithmic || this.ZAxisValueType == ChartValueType.TimeSpan)
      {
        if (!(this.ActualZValues is List<double>))
          this.ActualZValues = this.ZValues = (IEnumerable) new List<double>();
      }
      else if (!(this.ActualZValues is List<string>))
        this.ActualZValues = this.ZValues = (IEnumerable) new List<string>();
      List<double> zvalues1 = this.ZValues as List<double>;
      if (this.XAxisValueType == ChartValueType.Double || this.XAxisValueType == ChartValueType.Logarithmic)
      {
        if (!(this.ActualXValues is List<double>))
          this.ActualXValues = this.XValues = (IEnumerable) new List<double>();
        IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
        switch (this.ZAxisValueType)
        {
          case ChartValueType.Double:
            do
            {
              PropertyDescriptorCollection properties2 = (enumerator.Current as ICustomTypeDescriptor).GetProperties();
              this.XData = Convert.ToDouble(properties2.Find(this.XBindingPath, false).GetValue((object) this.XBindingPath) ?? (object) double.NaN);
              if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                this.isLinearData = false;
              xvalues.Add(this.XData);
              for (int index = 0; index < ((IEnumerable<string>) yPaths).Count<string>(); ++index)
              {
                object obj2 = properties2.Find(yPaths[index], false).GetValue((object) yPaths[index]);
                yLists[index].Add(Convert.ToDouble(obj2 ?? (object) double.NaN));
              }
              object obj3 = properties2.Find(xyzDataSeries3D.ZBindingPath, false).GetValue((object) xyzDataSeries3D.ZBindingPath);
              zvalues1.Add(Convert.ToDouble(obj3));
              this.ActualData.Add(enumerator.Current);
            }
            while (enumerator.MoveNext());
            this.DataCount = xvalues.Count;
            break;
          case ChartValueType.DateTime:
            do
            {
              PropertyDescriptorCollection properties3 = (enumerator.Current as ICustomTypeDescriptor).GetProperties();
              this.XData = Convert.ToDouble(properties3.Find(this.XBindingPath, false).GetValue((object) this.XBindingPath) ?? (object) double.NaN);
              if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                this.isLinearData = false;
              xvalues.Add(this.XData);
              for (int index = 0; index < ((IEnumerable<string>) yPaths).Count<string>(); ++index)
              {
                object obj4 = properties3.Find(yPaths[index], false).GetValue((object) yPaths[index]);
                yLists[index].Add(Convert.ToDouble(obj4 ?? (object) double.NaN));
              }
              object obj5 = properties3.Find(xyzDataSeries3D.ZBindingPath, false).GetValue((object) xyzDataSeries3D.ZBindingPath);
              zvalues1.Add(((DateTime) obj5).ToOADate());
              this.ActualData.Add(enumerator.Current);
            }
            while (enumerator.MoveNext());
            this.DataCount = xvalues.Count;
            break;
          case ChartValueType.TimeSpan:
            do
            {
              PropertyDescriptorCollection properties4 = (enumerator.Current as ICustomTypeDescriptor).GetProperties();
              this.XData = Convert.ToDouble(properties4.Find(this.XBindingPath, false).GetValue((object) this.XBindingPath) ?? (object) double.NaN);
              if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                this.isLinearData = false;
              xvalues.Add(this.XData);
              for (int index = 0; index < ((IEnumerable<string>) yPaths).Count<string>(); ++index)
              {
                object obj6 = properties4.Find(yPaths[index], false).GetValue((object) yPaths[index]);
                yLists[index].Add(Convert.ToDouble(obj6 ?? (object) double.NaN));
              }
              object obj7 = properties4.Find(xyzDataSeries3D.ZBindingPath, false).GetValue((object) xyzDataSeries3D.ZBindingPath);
              zvalues1.Add(((TimeSpan) obj7).TotalMilliseconds);
              this.ActualData.Add(enumerator.Current);
            }
            while (enumerator.MoveNext());
            this.DataCount = xvalues.Count;
            break;
          default:
            object zvalues2 = (object) this.ZValues;
            do
            {
              PropertyDescriptorCollection properties5 = (enumerator.Current as ICustomTypeDescriptor).GetProperties();
              this.XData = Convert.ToDouble(properties5.Find(this.XBindingPath, false).GetValue((object) this.XBindingPath) ?? (object) double.NaN);
              if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                this.isLinearData = false;
              xvalues.Add(this.XData);
              for (int index = 0; index < ((IEnumerable<string>) yPaths).Count<string>(); ++index)
              {
                object obj8 = properties5.Find(yPaths[index], false).GetValue((object) yPaths[index]);
                yLists[index].Add(Convert.ToDouble(obj8 ?? (object) double.NaN));
              }
              object obj9 = properties5.Find(xyzDataSeries3D.ZBindingPath, false).GetValue((object) xyzDataSeries3D.ZBindingPath);
              (zvalues2 as List<string>).Add((string) obj9);
              this.ActualData.Add(enumerator.Current);
            }
            while (enumerator.MoveNext());
            this.DataCount = xvalues.Count;
            break;
        }
      }
      else if (this.XAxisValueType == ChartValueType.DateTime)
      {
        if (!(this.ActualXValues is List<double>))
          this.ActualXValues = this.XValues = (IEnumerable) new List<double>();
        IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
        switch (this.ZAxisValueType)
        {
          case ChartValueType.Double:
            do
            {
              PropertyDescriptorCollection properties6 = (enumerator.Current as ICustomTypeDescriptor).GetProperties();
              this.XData = ((DateTime) properties6.Find(this.XBindingPath, false).GetValue((object) this.XBindingPath)).ToOADate();
              if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                this.isLinearData = false;
              xvalues.Add(this.XData);
              for (int index = 0; index < ((IEnumerable<string>) yPaths).Count<string>(); ++index)
              {
                object obj10 = properties6.Find(yPaths[index], false).GetValue((object) yPaths[index]);
                yLists[index].Add(Convert.ToDouble(obj10 ?? (object) double.NaN));
              }
              object obj11 = properties6.Find(xyzDataSeries3D.ZBindingPath, false).GetValue((object) xyzDataSeries3D.ZBindingPath);
              zvalues1.Add(Convert.ToDouble(obj11));
              this.ActualData.Add(enumerator.Current);
            }
            while (enumerator.MoveNext());
            this.DataCount = xvalues.Count;
            break;
          case ChartValueType.DateTime:
            do
            {
              PropertyDescriptorCollection properties7 = (enumerator.Current as ICustomTypeDescriptor).GetProperties();
              this.XData = ((DateTime) properties7.Find(this.XBindingPath, false).GetValue((object) this.XBindingPath)).ToOADate();
              if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                this.isLinearData = false;
              xvalues.Add(this.XData);
              for (int index = 0; index < ((IEnumerable<string>) yPaths).Count<string>(); ++index)
              {
                object obj12 = properties7.Find(yPaths[index], false).GetValue((object) yPaths[index]);
                yLists[index].Add(Convert.ToDouble(obj12 ?? (object) double.NaN));
              }
              object obj13 = properties7.Find(xyzDataSeries3D.ZBindingPath, false).GetValue((object) xyzDataSeries3D.ZBindingPath);
              zvalues1.Add(((DateTime) obj13).ToOADate());
              this.ActualData.Add(enumerator.Current);
            }
            while (enumerator.MoveNext());
            this.DataCount = xvalues.Count;
            break;
          case ChartValueType.TimeSpan:
            do
            {
              PropertyDescriptorCollection properties8 = (enumerator.Current as ICustomTypeDescriptor).GetProperties();
              this.XData = ((DateTime) properties8.Find(this.XBindingPath, false).GetValue((object) this.XBindingPath)).ToOADate();
              if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                this.isLinearData = false;
              xvalues.Add(this.XData);
              for (int index = 0; index < ((IEnumerable<string>) yPaths).Count<string>(); ++index)
              {
                object obj14 = properties8.Find(yPaths[index], false).GetValue((object) yPaths[index]);
                yLists[index].Add(Convert.ToDouble(obj14 ?? (object) double.NaN));
              }
              object obj15 = properties8.Find(xyzDataSeries3D.ZBindingPath, false).GetValue((object) xyzDataSeries3D.ZBindingPath);
              zvalues1.Add(((TimeSpan) obj15).TotalMilliseconds);
              this.ActualData.Add(enumerator.Current);
            }
            while (enumerator.MoveNext());
            this.DataCount = xvalues.Count;
            break;
          default:
            object zvalues3 = (object) this.ZValues;
            do
            {
              PropertyDescriptorCollection properties9 = (enumerator.Current as ICustomTypeDescriptor).GetProperties();
              this.XData = ((DateTime) properties9.Find(this.XBindingPath, false).GetValue((object) this.XBindingPath)).ToOADate();
              if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                this.isLinearData = false;
              xvalues.Add(this.XData);
              for (int index = 0; index < ((IEnumerable<string>) yPaths).Count<string>(); ++index)
              {
                object obj16 = properties9.Find(yPaths[index], false).GetValue((object) yPaths[index]);
                yLists[index].Add(Convert.ToDouble(obj16 ?? (object) double.NaN));
              }
              object obj17 = properties9.Find(xyzDataSeries3D.ZBindingPath, false).GetValue((object) xyzDataSeries3D.ZBindingPath);
              (zvalues3 as List<string>).Add((string) obj17);
              this.ActualData.Add(enumerator.Current);
            }
            while (enumerator.MoveNext());
            this.DataCount = xvalues.Count;
            break;
        }
      }
      else if (this.XAxisValueType == ChartValueType.TimeSpan)
      {
        if (!(this.ActualXValues is List<double>))
          this.ActualXValues = this.XValues = (IEnumerable) new List<double>();
        IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
        switch (this.ZAxisValueType)
        {
          case ChartValueType.Double:
            do
            {
              PropertyDescriptorCollection properties10 = (enumerator.Current as ICustomTypeDescriptor).GetProperties();
              this.XData = ((TimeSpan) properties10.Find(this.XBindingPath, false).GetValue((object) this.XBindingPath)).TotalMilliseconds;
              if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                this.isLinearData = false;
              xvalues.Add(this.XData);
              for (int index = 0; index < ((IEnumerable<string>) yPaths).Count<string>(); ++index)
              {
                object obj18 = properties10.Find(yPaths[index], false).GetValue((object) yPaths[index]);
                yLists[index].Add(Convert.ToDouble(obj18 ?? (object) double.NaN));
              }
              object obj19 = properties10.Find(xyzDataSeries3D.ZBindingPath, false).GetValue((object) xyzDataSeries3D.ZBindingPath);
              zvalues1.Add(Convert.ToDouble(obj19));
              this.ActualData.Add(enumerator.Current);
            }
            while (enumerator.MoveNext());
            this.DataCount = xvalues.Count;
            break;
          case ChartValueType.DateTime:
            do
            {
              PropertyDescriptorCollection properties11 = (enumerator.Current as ICustomTypeDescriptor).GetProperties();
              this.XData = ((TimeSpan) properties11.Find(this.XBindingPath, false).GetValue((object) this.XBindingPath)).TotalMilliseconds;
              if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                this.isLinearData = false;
              xvalues.Add(this.XData);
              for (int index = 0; index < ((IEnumerable<string>) yPaths).Count<string>(); ++index)
              {
                object obj20 = properties11.Find(yPaths[index], false).GetValue((object) yPaths[index]);
                yLists[index].Add(Convert.ToDouble(obj20 ?? (object) double.NaN));
              }
              object obj21 = properties11.Find(xyzDataSeries3D.ZBindingPath, false).GetValue((object) xyzDataSeries3D.ZBindingPath);
              zvalues1.Add(((DateTime) obj21).ToOADate());
              this.ActualData.Add(enumerator.Current);
            }
            while (enumerator.MoveNext());
            this.DataCount = xvalues.Count;
            break;
          case ChartValueType.TimeSpan:
            do
            {
              PropertyDescriptorCollection properties12 = (enumerator.Current as ICustomTypeDescriptor).GetProperties();
              this.XData = ((TimeSpan) properties12.Find(this.XBindingPath, false).GetValue((object) this.XBindingPath)).TotalMilliseconds;
              if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                this.isLinearData = false;
              xvalues.Add(this.XData);
              for (int index = 0; index < ((IEnumerable<string>) yPaths).Count<string>(); ++index)
              {
                object obj22 = properties12.Find(yPaths[index], false).GetValue((object) yPaths[index]);
                yLists[index].Add(Convert.ToDouble(obj22 ?? (object) double.NaN));
              }
              object obj23 = properties12.Find(xyzDataSeries3D.ZBindingPath, false).GetValue((object) xyzDataSeries3D.ZBindingPath);
              zvalues1.Add(((TimeSpan) obj23).TotalMilliseconds);
              this.ActualData.Add(enumerator.Current);
            }
            while (enumerator.MoveNext());
            this.DataCount = xvalues.Count;
            break;
          default:
            object zvalues4 = (object) this.ZValues;
            do
            {
              PropertyDescriptorCollection properties13 = (enumerator.Current as ICustomTypeDescriptor).GetProperties();
              this.XData = ((TimeSpan) properties13.Find(this.XBindingPath, false).GetValue((object) this.XBindingPath)).TotalMilliseconds;
              if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                this.isLinearData = false;
              xvalues.Add(this.XData);
              for (int index = 0; index < ((IEnumerable<string>) yPaths).Count<string>(); ++index)
              {
                object obj24 = properties13.Find(yPaths[index], false).GetValue((object) yPaths[index]);
                yLists[index].Add(Convert.ToDouble(obj24 ?? (object) double.NaN));
              }
              object obj25 = properties13.Find(xyzDataSeries3D.ZBindingPath, false).GetValue((object) xyzDataSeries3D.ZBindingPath);
              (zvalues4 as List<string>).Add((string) obj25);
              this.ActualData.Add(enumerator.Current);
            }
            while (enumerator.MoveNext());
            this.DataCount = xvalues.Count;
            break;
        }
      }
      else
      {
        if (this.XAxisValueType != ChartValueType.String)
          return;
        if (!(this.ActualXValues is List<string>))
          this.ActualXValues = this.XValues = (IEnumerable) new List<string>();
        IList<string> xvalues = (IList<string>) (this.XValues as List<string>);
        switch (this.ZAxisValueType)
        {
          case ChartValueType.Double:
            do
            {
              PropertyDescriptorCollection properties14 = (enumerator.Current as ICustomTypeDescriptor).GetProperties();
              object obj26 = properties14.Find(this.XBindingPath, false).GetValue((object) this.XBindingPath);
              xvalues.Add((string) obj26);
              for (int index = 0; index < ((IEnumerable<string>) yPaths).Count<string>(); ++index)
              {
                object obj27 = properties14.Find(yPaths[index], false).GetValue((object) yPaths[index]);
                yLists[index].Add(Convert.ToDouble(obj27 ?? (object) double.NaN));
              }
              object obj28 = properties14.Find(xyzDataSeries3D.ZBindingPath, false).GetValue((object) xyzDataSeries3D.ZBindingPath);
              zvalues1.Add(Convert.ToDouble(obj28));
              this.ActualData.Add(enumerator.Current);
            }
            while (enumerator.MoveNext());
            this.DataCount = xvalues.Count;
            break;
          case ChartValueType.DateTime:
            do
            {
              PropertyDescriptorCollection properties15 = (enumerator.Current as ICustomTypeDescriptor).GetProperties();
              object obj29 = properties15.Find(this.XBindingPath, false).GetValue((object) this.XBindingPath);
              xvalues.Add((string) obj29);
              for (int index = 0; index < ((IEnumerable<string>) yPaths).Count<string>(); ++index)
              {
                object obj30 = properties15.Find(yPaths[index], false).GetValue((object) yPaths[index]);
                yLists[index].Add(Convert.ToDouble(obj30 ?? (object) double.NaN));
              }
              object obj31 = properties15.Find(xyzDataSeries3D.ZBindingPath, false).GetValue((object) xyzDataSeries3D.ZBindingPath);
              zvalues1.Add(((DateTime) obj31).ToOADate());
              this.ActualData.Add(enumerator.Current);
            }
            while (enumerator.MoveNext());
            this.DataCount = xvalues.Count;
            break;
          case ChartValueType.TimeSpan:
            do
            {
              PropertyDescriptorCollection properties16 = (enumerator.Current as ICustomTypeDescriptor).GetProperties();
              object obj32 = properties16.Find(this.XBindingPath, false).GetValue((object) this.XBindingPath);
              xvalues.Add((string) obj32);
              for (int index = 0; index < ((IEnumerable<string>) yPaths).Count<string>(); ++index)
              {
                object obj33 = properties16.Find(yPaths[index], false).GetValue((object) yPaths[index]);
                yLists[index].Add(Convert.ToDouble(obj33 ?? (object) double.NaN));
              }
              object obj34 = properties16.Find(xyzDataSeries3D.ZBindingPath, false).GetValue((object) xyzDataSeries3D.ZBindingPath);
              zvalues1.Add(((TimeSpan) obj34).TotalMilliseconds);
              this.ActualData.Add(enumerator.Current);
            }
            while (enumerator.MoveNext());
            this.DataCount = xvalues.Count;
            break;
          default:
            object zvalues5 = (object) this.ZValues;
            do
            {
              PropertyDescriptorCollection properties17 = (enumerator.Current as ICustomTypeDescriptor).GetProperties();
              object obj35 = properties17.Find(this.XBindingPath, false).GetValue((object) this.XBindingPath);
              xvalues.Add((string) obj35);
              for (int index = 0; index < ((IEnumerable<string>) yPaths).Count<string>(); ++index)
              {
                object obj36 = properties17.Find(yPaths[index], false).GetValue((object) yPaths[index]);
                yLists[index].Add(Convert.ToDouble(obj36 ?? (object) double.NaN));
              }
              object obj37 = properties17.Find(xyzDataSeries3D.ZBindingPath, false).GetValue((object) xyzDataSeries3D.ZBindingPath);
              (zvalues5 as List<string>).Add((string) obj37);
              this.ActualData.Add(enumerator.Current);
            }
            while (enumerator.MoveNext());
            this.DataCount = xvalues.Count;
            break;
        }
      }
    }
  }

  internal override void GenerateComplexPropertyPoints(
    string[] yPaths,
    IList<double>[] yLists,
    ChartSeriesBase.GetReflectedProperty getPropertyValue)
  {
    XyzDataSeries3D xyzDataSeries3D = this;
    if (xyzDataSeries3D == null || xyzDataSeries3D != null && (xyzDataSeries3D.ZBindingPath == null || xyzDataSeries3D.ZBindingPath.Length == 0))
    {
      base.GenerateComplexPropertyPoints(yPaths, yLists, getPropertyValue);
    }
    else
    {
      IEnumerator enumerator = (this.ItemsSource as IEnumerable).GetEnumerator();
      if (enumerator.MoveNext())
      {
        for (int index = 0; index < this.UpdateStartedIndex; ++index)
          enumerator.MoveNext();
        this.XAxisValueType = this.GetDataType(this.ItemsSource as IEnumerable, this.XComplexPaths);
        if (this.XAxisValueType == ChartValueType.DateTime || this.XAxisValueType == ChartValueType.Double || this.XAxisValueType == ChartValueType.Logarithmic || this.XAxisValueType == ChartValueType.TimeSpan)
        {
          if (!(this.XValues is List<double>))
            this.ActualXValues = this.XValues = (IEnumerable) new List<double>();
        }
        else if (!(this.XValues is List<string>))
          this.ActualXValues = this.XValues = (IEnumerable) new List<string>();
        if (this.ZAxisValueType == ChartValueType.DateTime || this.ZAxisValueType == ChartValueType.Double || this.ZAxisValueType == ChartValueType.Logarithmic || this.ZAxisValueType == ChartValueType.TimeSpan)
        {
          if (!(this.ZValues is List<double>))
            this.ActualZValues = this.ZValues = (IEnumerable) new List<double>();
        }
        else if (!(this.ZValues is List<string>))
          this.ActualZValues = this.ZValues = (IEnumerable) new List<string>();
        string[] ycomplexPath = this.YComplexPaths[0];
        if (string.IsNullOrEmpty(yPaths[0]))
          return;
        IList<double> yList = yLists[0];
        List<double> zvalues1 = this.ZValues as List<double>;
        if (this.XAxisValueType == ChartValueType.String)
        {
          IList<string> xvalues = (IList<string>) (this.XValues as List<string>);
          switch (this.ZAxisValueType)
          {
            case ChartValueType.Double:
              do
              {
                object obj1 = getPropertyValue(enumerator.Current, this.XComplexPaths);
                object obj2 = getPropertyValue(enumerator.Current, ycomplexPath);
                object obj3 = getPropertyValue(enumerator.Current, this.ZComplexPaths);
                if (obj1 == null)
                  return;
                xvalues.Add((string) obj1);
                yList.Add(Convert.ToDouble(obj2 ?? (object) double.NaN));
                zvalues1.Add(Convert.ToDouble(obj3));
                this.ActualData.Add(enumerator.Current);
              }
              while (enumerator.MoveNext());
              this.DataCount = xvalues.Count;
              break;
            case ChartValueType.DateTime:
              do
              {
                object obj4 = getPropertyValue(enumerator.Current, this.XComplexPaths);
                object obj5 = getPropertyValue(enumerator.Current, ycomplexPath);
                object obj6 = getPropertyValue(enumerator.Current, this.ZComplexPaths);
                if (obj4 == null)
                  return;
                xvalues.Add((string) obj4);
                yList.Add(Convert.ToDouble(obj5 ?? (object) double.NaN));
                zvalues1.Add(((DateTime) obj6).ToOADate());
                this.ActualData.Add(enumerator.Current);
              }
              while (enumerator.MoveNext());
              this.DataCount = xvalues.Count;
              break;
            case ChartValueType.TimeSpan:
              do
              {
                object obj7 = getPropertyValue(enumerator.Current, this.XComplexPaths);
                object obj8 = getPropertyValue(enumerator.Current, ycomplexPath);
                object obj9 = getPropertyValue(enumerator.Current, this.ZComplexPaths);
                if (obj7 == null)
                  return;
                xvalues.Add((string) obj7);
                yList.Add(Convert.ToDouble(obj8 ?? (object) double.NaN));
                zvalues1.Add(((TimeSpan) obj9).TotalMilliseconds);
                this.ActualData.Add(enumerator.Current);
              }
              while (enumerator.MoveNext());
              this.DataCount = xvalues.Count;
              break;
            default:
              object zvalues2 = (object) this.ZValues;
              do
              {
                object obj10 = getPropertyValue(enumerator.Current, this.XComplexPaths);
                object obj11 = getPropertyValue(enumerator.Current, ycomplexPath);
                object obj12 = getPropertyValue(enumerator.Current, this.ZComplexPaths);
                if (obj10 == null)
                  return;
                xvalues.Add((string) obj10);
                yList.Add(Convert.ToDouble(obj11 ?? (object) double.NaN));
                (zvalues2 as List<string>).Add((string) obj12);
                this.ActualData.Add(enumerator.Current);
              }
              while (enumerator.MoveNext());
              this.DataCount = xvalues.Count;
              break;
          }
        }
        else if (this.XAxisValueType == ChartValueType.Double || this.XAxisValueType == ChartValueType.Logarithmic)
        {
          IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
          switch (this.ZAxisValueType)
          {
            case ChartValueType.Double:
              do
              {
                object obj13 = getPropertyValue(enumerator.Current, this.XComplexPaths);
                object obj14 = getPropertyValue(enumerator.Current, ycomplexPath);
                object obj15 = getPropertyValue(enumerator.Current, this.ZComplexPaths);
                if (obj13 == null)
                  return;
                this.XData = Convert.ToDouble(obj13);
                if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                  this.isLinearData = false;
                xvalues.Add(this.XData);
                yList.Add(Convert.ToDouble(obj14 ?? (object) double.NaN));
                zvalues1.Add(Convert.ToDouble(obj15));
                this.ActualData.Add(enumerator.Current);
              }
              while (enumerator.MoveNext());
              this.DataCount = xvalues.Count;
              break;
            case ChartValueType.DateTime:
              do
              {
                object obj16 = getPropertyValue(enumerator.Current, this.XComplexPaths);
                object obj17 = getPropertyValue(enumerator.Current, ycomplexPath);
                object obj18 = getPropertyValue(enumerator.Current, this.ZComplexPaths);
                if (obj16 == null)
                  return;
                this.XData = Convert.ToDouble(obj16);
                if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                  this.isLinearData = false;
                xvalues.Add(this.XData);
                yList.Add(Convert.ToDouble(obj17 ?? (object) double.NaN));
                zvalues1.Add(((DateTime) obj18).ToOADate());
                this.ActualData.Add(enumerator.Current);
              }
              while (enumerator.MoveNext());
              this.DataCount = xvalues.Count;
              break;
            case ChartValueType.TimeSpan:
              do
              {
                object obj19 = getPropertyValue(enumerator.Current, this.XComplexPaths);
                object obj20 = getPropertyValue(enumerator.Current, ycomplexPath);
                object obj21 = getPropertyValue(enumerator.Current, this.ZComplexPaths);
                if (obj19 == null)
                  return;
                this.XData = Convert.ToDouble(obj19);
                if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                  this.isLinearData = false;
                xvalues.Add(this.XData);
                yList.Add(Convert.ToDouble(obj20 ?? (object) double.NaN));
                zvalues1.Add(((TimeSpan) obj21).TotalMilliseconds);
                this.ActualData.Add(enumerator.Current);
              }
              while (enumerator.MoveNext());
              this.DataCount = xvalues.Count;
              break;
            default:
              object zvalues3 = (object) this.ZValues;
              do
              {
                object obj22 = getPropertyValue(enumerator.Current, this.XComplexPaths);
                object obj23 = getPropertyValue(enumerator.Current, ycomplexPath);
                object obj24 = getPropertyValue(enumerator.Current, this.ZComplexPaths);
                if (obj22 == null)
                  return;
                this.XData = Convert.ToDouble(obj22);
                if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                  this.isLinearData = false;
                xvalues.Add(this.XData);
                yList.Add(Convert.ToDouble(obj23 ?? (object) double.NaN));
                (zvalues3 as List<string>).Add((string) obj24);
                this.ActualData.Add(enumerator.Current);
              }
              while (enumerator.MoveNext());
              this.DataCount = xvalues.Count;
              break;
          }
        }
        else if (this.XAxisValueType == ChartValueType.DateTime)
        {
          IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
          switch (this.ZAxisValueType)
          {
            case ChartValueType.Double:
              do
              {
                object obj25 = getPropertyValue(enumerator.Current, this.XComplexPaths);
                object obj26 = getPropertyValue(enumerator.Current, ycomplexPath);
                object obj27 = getPropertyValue(enumerator.Current, this.ZComplexPaths);
                if (obj25 == null)
                  return;
                this.XData = ((DateTime) obj25).ToOADate();
                if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                  this.isLinearData = false;
                xvalues.Add(this.XData);
                yList.Add(Convert.ToDouble(obj26 ?? (object) double.NaN));
                zvalues1.Add(Convert.ToDouble(obj27));
                this.ActualData.Add(enumerator.Current);
              }
              while (enumerator.MoveNext());
              this.DataCount = xvalues.Count;
              break;
            case ChartValueType.DateTime:
              do
              {
                object obj28 = getPropertyValue(enumerator.Current, this.XComplexPaths);
                object obj29 = getPropertyValue(enumerator.Current, ycomplexPath);
                object obj30 = getPropertyValue(enumerator.Current, this.ZComplexPaths);
                if (obj28 == null)
                  return;
                this.XData = ((DateTime) obj28).ToOADate();
                if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                  this.isLinearData = false;
                xvalues.Add(this.XData);
                yList.Add(Convert.ToDouble(obj29 ?? (object) double.NaN));
                zvalues1.Add(((DateTime) obj30).ToOADate());
                this.ActualData.Add(enumerator.Current);
              }
              while (enumerator.MoveNext());
              this.DataCount = xvalues.Count;
              break;
            case ChartValueType.TimeSpan:
              do
              {
                object obj31 = getPropertyValue(enumerator.Current, this.XComplexPaths);
                object obj32 = getPropertyValue(enumerator.Current, ycomplexPath);
                object obj33 = getPropertyValue(enumerator.Current, this.ZComplexPaths);
                if (obj31 == null)
                  return;
                this.XData = ((DateTime) obj31).ToOADate();
                if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                  this.isLinearData = false;
                xvalues.Add(this.XData);
                yList.Add(Convert.ToDouble(obj32 ?? (object) double.NaN));
                zvalues1.Add(((TimeSpan) obj33).TotalMilliseconds);
                this.ActualData.Add(enumerator.Current);
              }
              while (enumerator.MoveNext());
              this.DataCount = xvalues.Count;
              break;
            default:
              object zvalues4 = (object) this.ZValues;
              do
              {
                object obj34 = getPropertyValue(enumerator.Current, this.XComplexPaths);
                object obj35 = getPropertyValue(enumerator.Current, ycomplexPath);
                object obj36 = getPropertyValue(enumerator.Current, this.ZComplexPaths);
                if (obj34 == null)
                  return;
                this.XData = ((DateTime) obj34).ToOADate();
                if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                  this.isLinearData = false;
                xvalues.Add(this.XData);
                yList.Add(Convert.ToDouble(obj35 ?? (object) double.NaN));
                (zvalues4 as List<string>).Add((string) obj36);
                this.ActualData.Add(enumerator.Current);
              }
              while (enumerator.MoveNext());
              this.DataCount = xvalues.Count;
              break;
          }
        }
        else if (this.XAxisValueType == ChartValueType.TimeSpan)
        {
          IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
          switch (this.ZAxisValueType)
          {
            case ChartValueType.Double:
              do
              {
                object obj37 = getPropertyValue(enumerator.Current, this.XComplexPaths);
                object obj38 = getPropertyValue(enumerator.Current, ycomplexPath);
                object obj39 = getPropertyValue(enumerator.Current, this.ZComplexPaths);
                if (obj37 == null)
                  return;
                this.XData = ((TimeSpan) obj37).TotalMilliseconds;
                if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                  this.isLinearData = false;
                xvalues.Add(this.XData);
                yList.Add(Convert.ToDouble(obj38 ?? (object) double.NaN));
                zvalues1.Add(Convert.ToDouble(obj39));
                this.ActualData.Add(enumerator.Current);
              }
              while (enumerator.MoveNext());
              this.DataCount = xvalues.Count;
              break;
            case ChartValueType.DateTime:
              do
              {
                object obj40 = getPropertyValue(enumerator.Current, this.XComplexPaths);
                object obj41 = getPropertyValue(enumerator.Current, ycomplexPath);
                object obj42 = getPropertyValue(enumerator.Current, this.ZComplexPaths);
                if (obj40 == null)
                  return;
                this.XData = ((TimeSpan) obj40).TotalMilliseconds;
                if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                  this.isLinearData = false;
                xvalues.Add(this.XData);
                yList.Add(Convert.ToDouble(obj41 ?? (object) double.NaN));
                zvalues1.Add(((DateTime) obj42).ToOADate());
                this.ActualData.Add(enumerator.Current);
              }
              while (enumerator.MoveNext());
              this.DataCount = xvalues.Count;
              break;
            case ChartValueType.TimeSpan:
              do
              {
                object obj43 = getPropertyValue(enumerator.Current, this.XComplexPaths);
                object obj44 = getPropertyValue(enumerator.Current, ycomplexPath);
                object obj45 = getPropertyValue(enumerator.Current, this.ZComplexPaths);
                if (obj43 == null)
                  return;
                this.XData = ((TimeSpan) obj43).TotalMilliseconds;
                if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                  this.isLinearData = false;
                xvalues.Add(this.XData);
                yList.Add(Convert.ToDouble(obj44 ?? (object) double.NaN));
                zvalues1.Add(((TimeSpan) obj45).TotalMilliseconds);
                this.ActualData.Add(enumerator.Current);
              }
              while (enumerator.MoveNext());
              this.DataCount = xvalues.Count;
              break;
            default:
              object zvalues5 = (object) this.ZValues;
              do
              {
                object obj46 = getPropertyValue(enumerator.Current, this.XComplexPaths);
                object obj47 = getPropertyValue(enumerator.Current, ycomplexPath);
                object obj48 = getPropertyValue(enumerator.Current, this.ZComplexPaths);
                if (obj46 == null)
                  return;
                this.XData = ((TimeSpan) obj46).TotalMilliseconds;
                if (this.isLinearData && xvalues.Count > 0 && this.XData < xvalues[xvalues.Count - 1])
                  this.isLinearData = false;
                xvalues.Add(this.XData);
                yList.Add(Convert.ToDouble(obj47 ?? (object) double.NaN));
                (zvalues5 as List<string>).Add((string) obj48);
                this.ActualData.Add(enumerator.Current);
              }
              while (enumerator.MoveNext());
              this.DataCount = xvalues.Count;
              break;
          }
        }
        this.HookPropertyChangedEvent(this.ListenPropertyChange);
      }
      this.IsPointGenerated = true;
    }
  }

  internal double GetZAdornmentAnglePosition(double start, double end)
  {
    double actualRotationAngle = this.Area.ActualRotationAngle;
    if (actualRotationAngle >= 0.0 && actualRotationAngle < 45.0 || actualRotationAngle >= 315.0 && actualRotationAngle < 360.0)
      return start;
    return actualRotationAngle >= 45.0 && actualRotationAngle < 135.0 || actualRotationAngle >= 225.0 && actualRotationAngle < 315.0 ? start + (end - start) * 0.5 : end + 2.0;
  }

  internal double GetZAdornmentAnglePosition(double z, DoubleRange zsbsInfo)
  {
    double actualRotationAngle = this.Area.ActualRotationAngle;
    if (actualRotationAngle >= 0.0 && actualRotationAngle < 45.0 || actualRotationAngle >= 315.0 && actualRotationAngle < 360.0)
      return z + zsbsInfo.Start;
    return actualRotationAngle >= 45.0 && actualRotationAngle < 135.0 || actualRotationAngle >= 225.0 && actualRotationAngle < 315.0 ? z + zsbsInfo.Median : z + zsbsInfo.End;
  }

  internal double GetXAdornmentAnglePosition(double x, DoubleRange sbsInfo)
  {
    double actualRotationAngle = this.Area.ActualRotationAngle;
    if (this.IsActualTransposed || actualRotationAngle >= 0.0 && actualRotationAngle < 45.0 || actualRotationAngle >= 315.0 && actualRotationAngle < 360.0 || actualRotationAngle >= 135.0 && actualRotationAngle < 225.0)
      return x + sbsInfo.Start;
    return actualRotationAngle >= 45.0 && actualRotationAngle < 135.0 ? x + sbsInfo.Median : x + sbsInfo.Start - Math.Abs(sbsInfo.Delta) / 2.0;
  }

  protected internal List<double> GetZValues()
  {
    double zIndexValues = 0.0;
    if (this.ActualZValues == null)
      return (List<double>) null;
    List<double> source = this.ActualZValues as List<double>;
    if (this.IsIndexedZAxis || source == null)
      source = source != null ? source.Select<double, double>((System.Func<double, double>) (val => zIndexValues++)).ToList<double>() : (this.ActualZValues as List<string>).Select<string, double>((System.Func<string, double>) (val => zIndexValues++)).ToList<double>();
    return source;
  }

  protected override void OnDataSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
    if (this.ActualZValues != null)
    {
      if (this.ActualZValues is IList<double>)
      {
        (this.ZValues as IList<double>).Clear();
        (this.ActualZValues as IList<double>).Clear();
      }
      else if (this.ActualZValues is IList<string>)
      {
        (this.ZValues as IList<string>).Clear();
        (this.ActualZValues as IList<string>).Clear();
      }
    }
    base.OnDataSourceChanged(oldValue, newValue);
  }

  protected override void SetIndividualDataTablePoint(int index, object obj, bool replace)
  {
    XyzDataSeries3D xyzDataSeries3D = this;
    if (xyzDataSeries3D == null || xyzDataSeries3D != null && (xyzDataSeries3D.ZBindingPath == null || xyzDataSeries3D.ZBindingPath.Length == 0))
    {
      base.SetIndividualDataTablePoint(index, obj, replace);
    }
    else
    {
      DataRow row = obj as DataRow;
      if (this.SeriesYValues == null || this.YPaths == null || this.ItemsSource == null)
        return;
      string[] ycomplexPath = this.YComplexPaths[0];
      IList<double> doubleList = index != 0 ? this.SeriesYValues[0] : (IList<double>) new List<double>();
      if (this.XAxisValueType == ChartValueType.String)
      {
        if (!(this.XValues is List<string>) || index == 0)
          this.XValues = this.ActualXValues = (IEnumerable) new List<string>();
        IList<string> xvalues = (IList<string>) (this.XValues as List<string>);
        object obj1 = row.Field<object>(this.XBindingPath);
        object obj2 = row.Field<object>(ycomplexPath[0]);
        object obj3 = row.Field<object>(xyzDataSeries3D.ZBindingPath);
        if (replace && xvalues.Count > index)
          xvalues[index] = Convert.ToString(obj1);
        else if (xvalues.Count == index)
          xvalues.Add(Convert.ToString(obj1));
        else
          xvalues.Insert(index, Convert.ToString(obj1));
        if (replace && doubleList.Count > index)
          doubleList[index] = Convert.ToDouble(obj2 ?? (object) double.NaN);
        else if (doubleList.Count == index)
          doubleList.Add(Convert.ToDouble(obj2 ?? (object) double.NaN));
        else
          doubleList.Insert(index, Convert.ToDouble(obj2 ?? (object) double.NaN));
        switch (this.ZAxisValueType)
        {
          case ChartValueType.Double:
            IList zvalues1 = (IList) (this.ZValues as List<double>);
            if (replace && zvalues1.Count > index)
            {
              zvalues1[index] = (object) Convert.ToDouble(obj3);
              break;
            }
            zvalues1.Insert(index, (object) Convert.ToDouble(obj3));
            break;
          case ChartValueType.DateTime:
            IList zvalues2 = (IList) (this.ZValues as List<double>);
            if (replace && zvalues2.Count > index)
            {
              zvalues2[index] = (object) ((DateTime) obj3).ToOADate();
              break;
            }
            zvalues2.Insert(index, (object) ((DateTime) obj3).ToOADate());
            break;
          case ChartValueType.TimeSpan:
            IList zvalues3 = (IList) (this.ZValues as List<double>);
            if (replace && zvalues3.Count > index)
            {
              zvalues3[index] = (object) ((TimeSpan) obj3).TotalMilliseconds;
              break;
            }
            zvalues3.Insert(index, (object) ((TimeSpan) obj3).TotalMilliseconds);
            break;
          default:
            IList zvalues4 = (IList) (this.ZValues as List<string>);
            string str = (string) obj3;
            if (replace && zvalues4.Count > index)
            {
              zvalues4[index] = (object) str;
              break;
            }
            zvalues4.Insert(index, (object) str);
            break;
        }
        this.DataCount = xvalues.Count;
      }
      else if (this.XAxisValueType == ChartValueType.Double || this.XAxisValueType == ChartValueType.Logarithmic)
      {
        if (!(this.XValues is List<double>) || index == 0)
          this.XValues = this.ActualXValues = (IEnumerable) new List<double>();
        IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
        object obj4 = row.Field<object>(this.XBindingPath);
        object obj5 = row.Field<object>(ycomplexPath[0]);
        object obj6 = row.Field<object>(xyzDataSeries3D.ZBindingPath);
        this.XData = Convert.ToDouble(obj4 ?? (object) double.NaN);
        if (this.isLinearData && index > 0 && this.XData < xvalues[index - 1])
          this.isLinearData = false;
        if (replace && xvalues.Count > index)
          xvalues[index] = this.XData;
        else if (xvalues.Count == index)
          xvalues.Add(this.XData);
        else
          xvalues.Insert(index, this.XData);
        if (replace && doubleList.Count > index)
          doubleList[index] = Convert.ToDouble(obj5 ?? (object) double.NaN);
        else if (doubleList.Count == index)
          doubleList.Add(Convert.ToDouble(obj5 ?? (object) double.NaN));
        else
          doubleList.Insert(index, Convert.ToDouble(obj5 ?? (object) double.NaN));
        switch (this.ZAxisValueType)
        {
          case ChartValueType.Double:
            IList zvalues5 = (IList) (this.ZValues as List<double>);
            if (replace && zvalues5.Count > index)
            {
              zvalues5[index] = (object) Convert.ToDouble(obj6);
              break;
            }
            zvalues5.Insert(index, (object) Convert.ToDouble(obj6));
            break;
          case ChartValueType.DateTime:
            IList zvalues6 = (IList) (this.ZValues as List<double>);
            if (replace && zvalues6.Count > index)
            {
              zvalues6[index] = (object) ((DateTime) obj6).ToOADate();
              break;
            }
            zvalues6.Insert(index, (object) ((DateTime) obj6).ToOADate());
            break;
          case ChartValueType.TimeSpan:
            IList zvalues7 = (IList) (this.ZValues as List<double>);
            if (replace && zvalues7.Count > index)
            {
              zvalues7[index] = (object) ((TimeSpan) obj6).TotalMilliseconds;
              break;
            }
            zvalues7.Insert(index, (object) ((TimeSpan) obj6).TotalMilliseconds);
            break;
          default:
            IList zvalues8 = (IList) (this.ZValues as List<string>);
            string str = (string) obj6;
            if (replace && zvalues8.Count > index)
            {
              zvalues8[index] = (object) str;
              break;
            }
            zvalues8.Insert(index, (object) str);
            break;
        }
        this.DataCount = xvalues.Count;
      }
      else if (this.XAxisValueType == ChartValueType.DateTime)
      {
        if (!(this.XValues is List<double>) || index == 0)
          this.XValues = this.ActualXValues = (IEnumerable) new List<double>();
        IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
        object obj7 = row.Field<object>(this.XBindingPath);
        object obj8 = row.Field<object>(ycomplexPath[0]);
        object obj9 = row.Field<object>(xyzDataSeries3D.ZBindingPath);
        this.XData = Convert.ToDateTime(obj7).ToOADate();
        if (this.isLinearData && index > 0 && this.XData < xvalues[index - 1])
          this.isLinearData = false;
        if (replace && xvalues.Count > index)
          xvalues[index] = this.XData;
        else if (xvalues.Count == index)
          xvalues.Add(this.XData);
        else
          xvalues.Insert(index, this.XData);
        if (replace && doubleList.Count > index)
          doubleList[index] = Convert.ToDouble(obj8 ?? (object) double.NaN);
        else if (doubleList.Count == index)
          doubleList.Add(Convert.ToDouble(obj8 ?? (object) double.NaN));
        else
          doubleList.Insert(index, Convert.ToDouble(obj8 ?? (object) double.NaN));
        switch (this.ZAxisValueType)
        {
          case ChartValueType.Double:
            IList zvalues9 = (IList) (this.ZValues as List<double>);
            if (replace && zvalues9.Count > index)
            {
              zvalues9[index] = (object) Convert.ToDouble(obj9);
              break;
            }
            zvalues9.Insert(index, (object) Convert.ToDouble(obj9));
            break;
          case ChartValueType.DateTime:
            IList zvalues10 = (IList) (this.ZValues as List<double>);
            if (replace && zvalues10.Count > index)
            {
              zvalues10[index] = (object) ((DateTime) obj9).ToOADate();
              break;
            }
            zvalues10.Insert(index, (object) ((DateTime) obj9).ToOADate());
            break;
          case ChartValueType.TimeSpan:
            IList zvalues11 = (IList) (this.ZValues as List<double>);
            if (replace && zvalues11.Count > index)
            {
              zvalues11[index] = (object) ((TimeSpan) obj9).TotalMilliseconds;
              break;
            }
            zvalues11.Insert(index, (object) ((TimeSpan) obj9).TotalMilliseconds);
            break;
          default:
            IList zvalues12 = (IList) (this.ZValues as List<string>);
            string str = (string) obj9;
            if (replace && zvalues12.Count > index)
            {
              zvalues12[index] = (object) str;
              break;
            }
            zvalues12.Insert(index, (object) str);
            break;
        }
        this.DataCount = xvalues.Count;
      }
      else if (this.XAxisValueType == ChartValueType.TimeSpan)
      {
        if (!(this.XValues is List<double>) || index == 0)
          this.XValues = this.ActualXValues = (IEnumerable) new List<double>();
        IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
        object obj10 = row.Field<object>(this.XBindingPath);
        object obj11 = row.Field<object>(ycomplexPath[0]);
        object obj12 = row.Field<object>(xyzDataSeries3D.ZBindingPath);
        this.XData = ((TimeSpan) obj10).TotalMilliseconds;
        if (this.isLinearData && index > 0 && this.XData < xvalues[index - 1])
          this.isLinearData = false;
        if (obj10 != null && replace && xvalues.Count > index)
          xvalues[index] = this.XData;
        else if (xvalues.Count == index)
          xvalues.Add(this.XData);
        else if (obj10 != null)
          xvalues.Insert(index, this.XData);
        if (obj11 != null && replace && doubleList.Count > index)
          doubleList[index] = Convert.ToDouble(obj11 ?? (object) double.NaN);
        else if (doubleList.Count == index)
          doubleList.Add(Convert.ToDouble(obj11 ?? (object) double.NaN));
        else
          doubleList.Insert(index, Convert.ToDouble(obj11 ?? (object) double.NaN));
        switch (this.ZAxisValueType)
        {
          case ChartValueType.Double:
            IList zvalues13 = (IList) (this.ZValues as List<double>);
            if (replace && zvalues13.Count > index)
            {
              zvalues13[index] = (object) Convert.ToDouble(obj12);
              break;
            }
            zvalues13.Insert(index, (object) Convert.ToDouble(obj12));
            break;
          case ChartValueType.DateTime:
            IList zvalues14 = (IList) (this.ZValues as List<double>);
            if (replace && zvalues14.Count > index)
            {
              zvalues14[index] = (object) ((DateTime) obj12).ToOADate();
              break;
            }
            zvalues14.Insert(index, (object) ((DateTime) obj12).ToOADate());
            break;
          case ChartValueType.TimeSpan:
            IList zvalues15 = (IList) (this.ZValues as List<double>);
            if (replace && zvalues15.Count > index)
            {
              zvalues15[index] = (object) ((TimeSpan) obj12).TotalMilliseconds;
              break;
            }
            zvalues15.Insert(index, (object) ((TimeSpan) obj12).TotalMilliseconds);
            break;
          default:
            IList zvalues16 = (IList) (this.ZValues as List<string>);
            string str = (string) obj12;
            if (replace && zvalues16.Count > index)
            {
              zvalues16[index] = (object) str;
              break;
            }
            zvalues16.Insert(index, (object) str);
            break;
        }
        this.DataCount = xvalues.Count;
      }
      if (replace && this.ActualData.Count > index)
        this.ActualData[index] = obj;
      else if (this.ActualData.Count == index)
        this.ActualData.Add(obj);
      else
        this.ActualData.Insert(index, obj);
    }
  }

  protected override void SetIndividualPoint(int index, object obj, bool replace)
  {
    XyzDataSeries3D xyzDataSeries3D = this;
    if (xyzDataSeries3D == null || xyzDataSeries3D != null && (xyzDataSeries3D.ZBindingPath == null || xyzDataSeries3D.ZBindingPath.Length == 0))
    {
      base.SetIndividualPoint(index, obj, replace);
    }
    else
    {
      if (this.SeriesYValues != null && this.YPaths != null && this.ItemsSource != null)
      {
        object arrayPropertyValue1 = this.GetArrayPropertyValue(obj, this.XComplexPaths);
        if (arrayPropertyValue1 != null)
          this.XAxisValueType = ChartSeriesBase.GetDataType(arrayPropertyValue1);
        string[] ycomplexPath = this.YComplexPaths[0];
        IList<double> seriesYvalue = this.SeriesYValues[0];
        if (this.XAxisValueType == ChartValueType.String)
        {
          if (!(this.XValues is List<string>))
            this.XValues = this.ActualXValues = (IEnumerable) new List<string>();
          IList<string> xvalues = (IList<string>) (this.XValues as List<string>);
          object arrayPropertyValue2 = this.GetArrayPropertyValue(obj, this.XComplexPaths);
          object arrayPropertyValue3 = this.GetArrayPropertyValue(obj, ycomplexPath);
          object arrayPropertyValue4 = this.GetArrayPropertyValue(obj, this.ZComplexPaths);
          if (replace && xvalues.Count > index)
            xvalues[index] = Convert.ToString(arrayPropertyValue2);
          else
            xvalues.Insert(index, Convert.ToString(arrayPropertyValue2));
          if (replace && seriesYvalue.Count > index)
            seriesYvalue[index] = Convert.ToDouble(arrayPropertyValue3 ?? (object) double.NaN);
          else
            seriesYvalue.Insert(index, Convert.ToDouble(arrayPropertyValue3 ?? (object) double.NaN));
          switch (this.ZAxisValueType)
          {
            case ChartValueType.Double:
              IList zvalues1 = (IList) (this.ZValues as List<double>);
              if (replace && zvalues1.Count > index)
              {
                zvalues1[index] = (object) Convert.ToDouble(arrayPropertyValue4);
                break;
              }
              zvalues1.Insert(index, (object) Convert.ToDouble(arrayPropertyValue4));
              break;
            case ChartValueType.DateTime:
              IList zvalues2 = (IList) (this.ZValues as List<double>);
              if (replace && zvalues2.Count > index)
              {
                zvalues2[index] = (object) ((DateTime) arrayPropertyValue4).ToOADate();
                break;
              }
              zvalues2.Insert(index, (object) ((DateTime) arrayPropertyValue4).ToOADate());
              break;
            case ChartValueType.TimeSpan:
              IList zvalues3 = (IList) (this.ZValues as List<double>);
              if (replace && zvalues3.Count > index)
              {
                zvalues3[index] = (object) ((TimeSpan) arrayPropertyValue4).TotalMilliseconds;
                break;
              }
              zvalues3.Insert(index, (object) ((TimeSpan) arrayPropertyValue4).TotalMilliseconds);
              break;
            default:
              IList zvalues4 = (IList) (this.ZValues as List<string>);
              string str = (string) arrayPropertyValue4;
              if (replace && zvalues4.Count > index)
              {
                zvalues4[index] = (object) str;
                break;
              }
              zvalues4.Insert(index, (object) str);
              break;
          }
          this.DataCount = xvalues.Count;
        }
        else if (this.XAxisValueType == ChartValueType.Double || this.XAxisValueType == ChartValueType.Logarithmic)
        {
          if (!(this.XValues is List<double>))
            this.XValues = this.ActualXValues = (IEnumerable) new List<double>();
          IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
          object arrayPropertyValue5 = this.GetArrayPropertyValue(obj, this.XComplexPaths);
          object arrayPropertyValue6 = this.GetArrayPropertyValue(obj, ycomplexPath);
          object arrayPropertyValue7 = this.GetArrayPropertyValue(obj, this.ZComplexPaths);
          this.XData = arrayPropertyValue5 != null ? Convert.ToDouble(arrayPropertyValue5) : double.NaN;
          if (this.isLinearData && index > 0 && this.XData < xvalues[index - 1])
            this.isLinearData = false;
          if (replace && xvalues.Count > index)
            xvalues[index] = this.XData;
          else
            xvalues.Insert(index, this.XData);
          if (replace && seriesYvalue.Count > index)
            seriesYvalue[index] = Convert.ToDouble(arrayPropertyValue6 ?? (object) double.NaN);
          else
            seriesYvalue.Insert(index, Convert.ToDouble(arrayPropertyValue6 ?? (object) double.NaN));
          switch (this.ZAxisValueType)
          {
            case ChartValueType.Double:
              IList zvalues5 = (IList) (this.ZValues as List<double>);
              if (replace && zvalues5.Count > index)
              {
                zvalues5[index] = (object) Convert.ToDouble(arrayPropertyValue7);
                break;
              }
              zvalues5.Insert(index, (object) Convert.ToDouble(arrayPropertyValue7));
              break;
            case ChartValueType.DateTime:
              IList zvalues6 = (IList) (this.ZValues as List<double>);
              if (replace && zvalues6.Count > index)
              {
                zvalues6[index] = (object) ((DateTime) arrayPropertyValue7).ToOADate();
                break;
              }
              zvalues6.Insert(index, (object) ((DateTime) arrayPropertyValue7).ToOADate());
              break;
            case ChartValueType.TimeSpan:
              IList zvalues7 = (IList) (this.ZValues as List<double>);
              if (replace && zvalues7.Count > index)
              {
                zvalues7[index] = (object) ((TimeSpan) arrayPropertyValue7).TotalMilliseconds;
                break;
              }
              zvalues7.Insert(index, (object) ((TimeSpan) arrayPropertyValue7).TotalMilliseconds);
              break;
            default:
              IList zvalues8 = (IList) (this.ZValues as List<string>);
              string str = (string) arrayPropertyValue7;
              if (replace && zvalues8.Count > index)
              {
                zvalues8[index] = (object) str;
                break;
              }
              zvalues8.Insert(index, (object) str);
              break;
          }
          this.DataCount = xvalues.Count;
        }
        else if (this.XAxisValueType == ChartValueType.DateTime)
        {
          if (!(this.XValues is List<double>))
            this.XValues = this.ActualXValues = (IEnumerable) new List<double>();
          IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
          object arrayPropertyValue8 = this.GetArrayPropertyValue(obj, this.XComplexPaths);
          object arrayPropertyValue9 = this.GetArrayPropertyValue(obj, ycomplexPath);
          object arrayPropertyValue10 = this.GetArrayPropertyValue(obj, this.ZComplexPaths);
          this.XData = Convert.ToDateTime(arrayPropertyValue8).ToOADate();
          if (this.isLinearData && index > 0 && this.XData < xvalues[index - 1])
            this.isLinearData = false;
          if (replace && xvalues.Count > index)
            xvalues[index] = this.XData;
          else
            xvalues.Insert(index, this.XData);
          if (replace && seriesYvalue.Count > index)
            seriesYvalue[index] = Convert.ToDouble(arrayPropertyValue9 ?? (object) double.NaN);
          else
            seriesYvalue.Insert(index, Convert.ToDouble(arrayPropertyValue9 ?? (object) double.NaN));
          switch (this.ZAxisValueType)
          {
            case ChartValueType.Double:
              IList zvalues9 = (IList) (this.ZValues as List<double>);
              if (replace && zvalues9.Count > index)
              {
                zvalues9[index] = (object) Convert.ToDouble(arrayPropertyValue10);
                break;
              }
              zvalues9.Insert(index, (object) Convert.ToDouble(arrayPropertyValue10));
              break;
            case ChartValueType.DateTime:
              IList zvalues10 = (IList) (this.ZValues as List<double>);
              if (replace && zvalues10.Count > index)
              {
                zvalues10[index] = (object) ((DateTime) arrayPropertyValue10).ToOADate();
                break;
              }
              zvalues10.Insert(index, (object) ((DateTime) arrayPropertyValue10).ToOADate());
              break;
            case ChartValueType.TimeSpan:
              IList zvalues11 = (IList) (this.ZValues as List<double>);
              if (replace && zvalues11.Count > index)
              {
                zvalues11[index] = (object) ((TimeSpan) arrayPropertyValue10).TotalMilliseconds;
                break;
              }
              zvalues11.Insert(index, (object) ((TimeSpan) arrayPropertyValue10).TotalMilliseconds);
              break;
            default:
              IList zvalues12 = (IList) (this.ZValues as List<string>);
              string str = (string) arrayPropertyValue10;
              if (replace && zvalues12.Count > index)
              {
                zvalues12[index] = (object) str;
                break;
              }
              zvalues12.Insert(index, (object) str);
              break;
          }
          this.DataCount = xvalues.Count;
        }
        else if (this.XAxisValueType == ChartValueType.TimeSpan)
        {
          if (!(this.XValues is List<double>))
            this.XValues = this.ActualXValues = (IEnumerable) new List<double>();
          IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
          object arrayPropertyValue11 = this.GetArrayPropertyValue(obj, this.XComplexPaths);
          object arrayPropertyValue12 = this.GetArrayPropertyValue(obj, ycomplexPath);
          object arrayPropertyValue13 = this.GetArrayPropertyValue(obj, this.ZComplexPaths);
          this.XData = ((TimeSpan) arrayPropertyValue11).TotalMilliseconds;
          if (this.isLinearData && index > 0 && this.XData < xvalues[index - 1])
            this.isLinearData = false;
          if (arrayPropertyValue11 != null && replace && xvalues.Count > index)
            xvalues[index] = this.XData;
          else if (arrayPropertyValue11 != null)
            xvalues.Insert(index, this.XData);
          if (replace && seriesYvalue.Count > index)
            seriesYvalue[index] = Convert.ToDouble(arrayPropertyValue12 ?? (object) double.NaN);
          else
            seriesYvalue.Insert(index, Convert.ToDouble(arrayPropertyValue12 ?? (object) double.NaN));
          switch (this.ZAxisValueType)
          {
            case ChartValueType.Double:
              IList zvalues13 = (IList) (this.ZValues as List<double>);
              if (replace && zvalues13.Count > index)
              {
                zvalues13[index] = (object) Convert.ToDouble(arrayPropertyValue13);
                break;
              }
              zvalues13.Insert(index, (object) Convert.ToDouble(arrayPropertyValue13));
              break;
            case ChartValueType.DateTime:
              IList zvalues14 = (IList) (this.ZValues as List<double>);
              if (replace && zvalues14.Count > index)
              {
                zvalues14[index] = (object) ((DateTime) arrayPropertyValue13).ToOADate();
                break;
              }
              zvalues14.Insert(index, (object) ((DateTime) arrayPropertyValue13).ToOADate());
              break;
            case ChartValueType.TimeSpan:
              IList zvalues15 = (IList) (this.ZValues as List<double>);
              if (replace && zvalues15.Count > index)
              {
                zvalues15[index] = (object) ((TimeSpan) arrayPropertyValue13).TotalMilliseconds;
                break;
              }
              zvalues15.Insert(index, (object) ((TimeSpan) arrayPropertyValue13).TotalMilliseconds);
              break;
            default:
              IList zvalues16 = (IList) (this.ZValues as List<string>);
              string str = (string) arrayPropertyValue13;
              if (replace && zvalues16.Count > index)
              {
                zvalues16[index] = (object) str;
                break;
              }
              zvalues16.Insert(index, (object) str);
              break;
          }
          this.DataCount = xvalues.Count;
        }
        if (replace && this.ActualData.Count > index)
          this.ActualData[index] = obj;
        else if (this.ActualData.Count == index)
          this.ActualData.Add(obj);
        else
          this.ActualData.Insert(index, obj);
        this.totalCalculated = false;
      }
      this.UpdateEmptyPoints(index);
      this.HookPropertyChangedEvent(this.ListenPropertyChange, obj);
    }
  }

  protected override void OnBindingPathChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.ActualZValues != null)
    {
      if (this.ActualZValues is IList<double>)
      {
        (this.ZValues as IList<double>).Clear();
        (this.ActualZValues as IList<double>).Clear();
      }
      else if (this.ActualZValues is IList<string>)
      {
        (this.ZValues as IList<string>).Clear();
        (this.ActualZValues as IList<string>).Clear();
      }
    }
    base.OnBindingPathChanged(args);
  }

  private static void OnBindingPathZChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    XyzDataSeries3D xyzDataSeries3D = obj as XyzDataSeries3D;
    if (args.NewValue != null)
      xyzDataSeries3D.ZComplexPaths = args.NewValue.ToString().Split('.');
    xyzDataSeries3D.OnBindingPathChanged(args);
  }
}
