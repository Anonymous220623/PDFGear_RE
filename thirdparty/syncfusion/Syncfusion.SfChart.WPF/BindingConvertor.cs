// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.BindingConvertor
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class BindingConvertor : ExpressionConverter
{
  public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
  {
    return destinationType == typeof (MarkupExtension);
  }

  public override object ConvertTo(
    ITypeDescriptorContext context,
    CultureInfo culture,
    object value,
    Type destinationType)
  {
    if (destinationType == typeof (MarkupExtension))
    {
      switch (value)
      {
        case MultiBindingExpression _:
          return value is MultiBindingExpression bindingExpression1 ? (object) bindingExpression1.ParentMultiBinding : throw new Exception();
        case BindingExpression _:
          return value is BindingExpression bindingExpression2 ? (object) bindingExpression2.ParentBinding : throw new Exception();
      }
    }
    return base.ConvertTo(context, culture, value, destinationType);
  }
}
