// Decompiled with JetBrains decompiler
// Type: NLog.Targets.DatabaseParameterInfo
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Config;
using NLog.Internal;
using NLog.Layouts;
using System;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Reflection;

#nullable disable
namespace NLog.Targets;

[NLogConfigurationItem]
public class DatabaseParameterInfo
{
  private Type _parameterType;
  private DatabaseParameterInfo.DbTypeSetter _cachedDbTypeSetter;

  public DatabaseParameterInfo()
    : this((string) null, (Layout) null)
  {
  }

  public DatabaseParameterInfo(string parameterName, Layout parameterLayout)
  {
    this.Name = parameterName;
    this.Layout = parameterLayout;
  }

  [RequiredParameter]
  public string Name { get; set; }

  [RequiredParameter]
  public Layout Layout { get; set; }

  [DefaultValue(null)]
  public string DbType { get; set; }

  [DefaultValue(0)]
  public int Size { get; set; }

  [DefaultValue(0)]
  public byte Precision { get; set; }

  [DefaultValue(0)]
  public byte Scale { get; set; }

  [DefaultValue(typeof (string))]
  public Type ParameterType
  {
    get
    {
      Type parameterType1 = this._parameterType;
      if ((object) parameterType1 != null)
        return parameterType1;
      Type parameterType2 = this._cachedDbTypeSetter?.ParameterType;
      return (object) parameterType2 != null ? parameterType2 : typeof (string);
    }
    set => this._parameterType = value;
  }

  [DefaultValue(null)]
  public string Format { get; set; }

  [DefaultValue(null)]
  public CultureInfo Culture { get; set; }

  [DefaultValue(false)]
  public bool AllowDbNull { get; set; }

  internal bool SetDbType(IDbDataParameter dbParameter)
  {
    if (string.IsNullOrEmpty(this.DbType))
      return true;
    if (this._cachedDbTypeSetter == null || !this._cachedDbTypeSetter.IsValid(dbParameter.GetType(), this.DbType))
      this._cachedDbTypeSetter = new DatabaseParameterInfo.DbTypeSetter(dbParameter.GetType(), this.DbType);
    return this._cachedDbTypeSetter.SetDbType(dbParameter);
  }

  private class DbTypeSetter
  {
    private readonly Type _dbPropertyInfoType;
    private readonly string _dbTypeName;
    private readonly PropertyInfo _dbTypeSetter;
    private readonly Enum _dbTypeValue;
    private Action<IDbDataParameter> _dbTypeSetterFast;

    public Type ParameterType { get; }

    public DbTypeSetter(Type dbParameterType, string dbTypeName)
    {
      this._dbPropertyInfoType = dbParameterType;
      this._dbTypeName = dbTypeName;
      if (StringHelpers.IsNullOrWhiteSpace(dbTypeName))
        return;
      string[] strArray = dbTypeName.SplitAndTrimTokens('.');
      if (strArray.Length > 1 && !string.Equals(strArray[0], "DbType", StringComparison.OrdinalIgnoreCase))
      {
        PropertyInfo property = dbParameterType.GetProperty(strArray[0], BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
        Enum enumValue;
        if (!(property != (PropertyInfo) null) || !DatabaseParameterInfo.DbTypeSetter.TryParseEnum(strArray[1], property.PropertyType, out enumValue))
          return;
        this._dbTypeSetter = property;
        this._dbTypeValue = enumValue;
        this.ParameterType = this.TryParseParameterType(enumValue.ToString());
      }
      else
      {
        dbTypeName = strArray[strArray.Length - 1];
        System.Data.DbType dbType;
        if (string.IsNullOrEmpty(dbTypeName) || !ConversionHelpers.TryParseEnum<System.Data.DbType>(dbTypeName, out dbType))
          return;
        this._dbTypeValue = (Enum) dbType;
        this.ParameterType = DatabaseParameterInfo.DbTypeSetter.TryLookupParameterType(dbType);
        this._dbTypeSetterFast = (Action<IDbDataParameter>) (p => p.DbType = dbType);
      }
    }

    private static Type TryLookupParameterType(System.Data.DbType dbType)
    {
      switch (dbType)
      {
        case System.Data.DbType.AnsiString:
        case System.Data.DbType.String:
        case System.Data.DbType.AnsiStringFixedLength:
        case System.Data.DbType.StringFixedLength:
        case System.Data.DbType.Xml:
          return typeof (string);
        case System.Data.DbType.Byte:
          return typeof (byte);
        case System.Data.DbType.Boolean:
          return typeof (bool);
        case System.Data.DbType.Currency:
        case System.Data.DbType.Decimal:
        case System.Data.DbType.VarNumeric:
          return typeof (Decimal);
        case System.Data.DbType.Date:
        case System.Data.DbType.DateTime:
        case System.Data.DbType.DateTime2:
          return typeof (DateTime);
        case System.Data.DbType.Double:
          return typeof (double);
        case System.Data.DbType.Guid:
          return typeof (Guid);
        case System.Data.DbType.Int16:
          return typeof (short);
        case System.Data.DbType.Int32:
          return typeof (int);
        case System.Data.DbType.Int64:
          return typeof (long);
        case System.Data.DbType.Object:
          return typeof (object);
        case System.Data.DbType.SByte:
          return typeof (sbyte);
        case System.Data.DbType.Single:
          return typeof (float);
        case System.Data.DbType.Time:
          return typeof (TimeSpan);
        case System.Data.DbType.UInt16:
          return typeof (ushort);
        case System.Data.DbType.UInt32:
          return typeof (uint);
        case System.Data.DbType.UInt64:
          return typeof (ulong);
        case System.Data.DbType.DateTimeOffset:
          return typeof (DateTimeOffset);
        default:
          return (Type) null;
      }
    }

    private Type TryParseParameterType(string dbTypeString)
    {
      if (dbTypeString.IndexOf("Date", StringComparison.OrdinalIgnoreCase) >= 0)
        return typeof (DateTime);
      if (dbTypeString.IndexOf("Timestamp", StringComparison.OrdinalIgnoreCase) >= 0)
        return typeof (DateTime);
      if (dbTypeString.IndexOf("Double", StringComparison.OrdinalIgnoreCase) >= 0)
        return typeof (double);
      if (dbTypeString.IndexOf("Decimal", StringComparison.OrdinalIgnoreCase) >= 0)
        return typeof (Decimal);
      if (dbTypeString.IndexOf("Bool", StringComparison.OrdinalIgnoreCase) >= 0)
        return typeof (bool);
      return dbTypeString.IndexOf("Guid", StringComparison.OrdinalIgnoreCase) >= 0 ? typeof (Guid) : (Type) null;
    }

    public bool IsValid(Type dbParameterType, string dbTypeName)
    {
      if ((object) this._dbPropertyInfoType != (object) dbParameterType || (object) this._dbTypeName != (object) dbTypeName)
        return false;
      if (this._dbTypeSetterFast == null && this._dbTypeSetter != (PropertyInfo) null && this._dbTypeValue != null)
      {
        ReflectionHelpers.LateBoundMethodSingle dbTypeSetterLambda = ReflectionHelpers.CreateLateBoundMethodSingle(this._dbTypeSetter.GetSetMethod());
        object obj;
        this._dbTypeSetterFast = (Action<IDbDataParameter>) (p => obj = dbTypeSetterLambda((object) p, (object) this._dbTypeValue));
      }
      return true;
    }

    public bool SetDbType(IDbDataParameter dbParameter)
    {
      if (this._dbTypeSetterFast != null)
      {
        this._dbTypeSetterFast(dbParameter);
        return true;
      }
      if (!(this._dbTypeSetter != (PropertyInfo) null) || this._dbTypeValue == null)
        return false;
      this._dbTypeSetter.SetValue((object) dbParameter, (object) this._dbTypeValue, (object[]) null);
      return true;
    }

    private static bool TryParseEnum(string value, Type enumType, out Enum enumValue)
    {
      object resultValue;
      if (!string.IsNullOrEmpty(value) && ConversionHelpers.TryParseEnum(value, enumType, out resultValue))
      {
        enumValue = resultValue as Enum;
        return enumValue != null;
      }
      enumValue = (Enum) null;
      return false;
    }
  }
}
