// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.Converter.ColLayoutConverter
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Tools.Extension;
using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Security;
using System.Text;

#nullable disable
namespace HandyControl.Tools.Converter;

public class ColLayoutConverter : TypeConverter
{
  public override bool CanConvertFrom(ITypeDescriptorContext typeDescriptorContext, Type sourceType)
  {
    switch (Type.GetTypeCode(sourceType))
    {
      case TypeCode.Int16:
      case TypeCode.UInt16:
      case TypeCode.Int32:
      case TypeCode.UInt32:
      case TypeCode.Int64:
      case TypeCode.UInt64:
      case TypeCode.Single:
      case TypeCode.Double:
      case TypeCode.Decimal:
      case TypeCode.String:
        return true;
      default:
        return false;
    }
  }

  public override bool CanConvertTo(
    ITypeDescriptorContext typeDescriptorContext,
    Type destinationType)
  {
    return destinationType == typeof (InstanceDescriptor) || destinationType == typeof (string);
  }

  public override object ConvertFrom(
    ITypeDescriptorContext typeDescriptorContext,
    CultureInfo cultureInfo,
    object source)
  {
    ColLayout colLayout;
    switch (source)
    {
      case null:
        throw this.GetConvertFromException((object) null);
      case string s:
        colLayout = ColLayoutConverter.FromString(s, cultureInfo);
        break;
      case double uniformWidth:
        colLayout = new ColLayout((int) uniformWidth);
        break;
      default:
        colLayout = new ColLayout(Convert.ToInt32(source, (IFormatProvider) cultureInfo));
        break;
    }
    return (object) colLayout;
  }

  [SecurityCritical]
  public override object ConvertTo(
    ITypeDescriptorContext typeDescriptorContext,
    CultureInfo cultureInfo,
    object value,
    Type destinationType)
  {
    if (value == null)
      throw new ArgumentNullException(nameof (value));
    if (destinationType == (Type) null)
      throw new ArgumentNullException(nameof (destinationType));
    if (!(value is ColLayout th))
      throw new ArgumentException("UnexpectedParameterType");
    if (destinationType == typeof (string))
      return (object) ColLayoutConverter.ToString(th, cultureInfo);
    if (!(destinationType == typeof (InstanceDescriptor)))
      throw new ArgumentException("CannotConvertType");
    return (object) new InstanceDescriptor((MemberInfo) typeof (ColLayout).GetConstructor(new Type[6]
    {
      typeof (int),
      typeof (int),
      typeof (int),
      typeof (int),
      typeof (int),
      typeof (int)
    }), (ICollection) new object[6]
    {
      (object) th.Xs,
      (object) th.Sm,
      (object) th.Md,
      (object) th.Lg,
      (object) th.Xl,
      (object) th.Xxl
    });
  }

  private static string ToString(ColLayout th, CultureInfo cultureInfo)
  {
    char numericListSeparator = TokenizerHelper.GetNumericListSeparator((IFormatProvider) cultureInfo);
    StringBuilder stringBuilder = new StringBuilder(128 /*0x80*/);
    stringBuilder.Append(th.Xs.ToString((IFormatProvider) cultureInfo));
    stringBuilder.Append(numericListSeparator);
    stringBuilder.Append(th.Sm.ToString((IFormatProvider) cultureInfo));
    stringBuilder.Append(numericListSeparator);
    stringBuilder.Append(th.Md.ToString((IFormatProvider) cultureInfo));
    stringBuilder.Append(numericListSeparator);
    stringBuilder.Append(th.Lg.ToString((IFormatProvider) cultureInfo));
    stringBuilder.Append(numericListSeparator);
    stringBuilder.Append(th.Xl.ToString((IFormatProvider) cultureInfo));
    stringBuilder.Append(numericListSeparator);
    stringBuilder.Append(th.Xxl.ToString((IFormatProvider) cultureInfo));
    return th.ToString();
  }

  private static ColLayout FromString(string s, CultureInfo cultureInfo)
  {
    TokenizerHelper tokenizerHelper = new TokenizerHelper(s, (IFormatProvider) cultureInfo);
    int[] numArray = new int[6];
    int index = 0;
    while (tokenizerHelper.NextToken())
    {
      if (index >= 6)
      {
        index = 7;
        break;
      }
      numArray[index] = tokenizerHelper.GetCurrentToken().Value<int>();
      ++index;
    }
    switch (index)
    {
      case 1:
        return new ColLayout(numArray[0]);
      case 2:
        return new ColLayout()
        {
          Xs = numArray[0],
          Sm = numArray[1]
        };
      case 3:
        return new ColLayout()
        {
          Xs = numArray[0],
          Sm = numArray[1],
          Md = numArray[2]
        };
      case 4:
        return new ColLayout()
        {
          Xs = numArray[0],
          Sm = numArray[1],
          Md = numArray[2],
          Lg = numArray[3]
        };
      case 5:
        return new ColLayout()
        {
          Xs = numArray[0],
          Sm = numArray[1],
          Md = numArray[2],
          Lg = numArray[3],
          Xl = numArray[4]
        };
      case 6:
        return new ColLayout()
        {
          Xs = numArray[0],
          Sm = numArray[1],
          Md = numArray[2],
          Lg = numArray[3],
          Xl = numArray[4],
          Xxl = numArray[5]
        };
      default:
        throw new FormatException("InvalidStringColLayout");
    }
  }
}
