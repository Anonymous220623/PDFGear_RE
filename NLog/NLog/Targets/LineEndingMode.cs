// Decompiled with JetBrains decompiler
// Type: NLog.Targets.LineEndingMode
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using JetBrains.Annotations;
using NLog.Internal;
using System;
using System.ComponentModel;
using System.Globalization;

#nullable disable
namespace NLog.Targets;

[TypeConverter(typeof (LineEndingMode.LineEndingModeConverter))]
public sealed class LineEndingMode : IEquatable<LineEndingMode>
{
  public static readonly LineEndingMode Default = new LineEndingMode(nameof (Default), EnvironmentHelper.NewLine);
  public static readonly LineEndingMode CRLF = new LineEndingMode(nameof (CRLF), "\r\n");
  public static readonly LineEndingMode CR = new LineEndingMode(nameof (CR), "\r");
  public static readonly LineEndingMode LF = new LineEndingMode(nameof (LF), "\n");
  public static readonly LineEndingMode Null = new LineEndingMode(nameof (Null), "\0");
  public static readonly LineEndingMode None = new LineEndingMode(nameof (None), string.Empty);
  private readonly string _name;
  private readonly string _newLineCharacters;

  public string Name => this._name;

  public string NewLineCharacters => this._newLineCharacters;

  private LineEndingMode()
  {
  }

  private LineEndingMode(string name, string newLineCharacters)
  {
    this._name = name;
    this._newLineCharacters = newLineCharacters;
  }

  public static LineEndingMode FromString([NotNull] string name)
  {
    if (name == null)
      throw new ArgumentNullException(nameof (name));
    if (name.Equals(LineEndingMode.CRLF.Name, StringComparison.OrdinalIgnoreCase))
      return LineEndingMode.CRLF;
    if (name.Equals(LineEndingMode.LF.Name, StringComparison.OrdinalIgnoreCase))
      return LineEndingMode.LF;
    if (name.Equals(LineEndingMode.CR.Name, StringComparison.OrdinalIgnoreCase))
      return LineEndingMode.CR;
    if (name.Equals(LineEndingMode.Default.Name, StringComparison.OrdinalIgnoreCase))
      return LineEndingMode.Default;
    if (name.Equals(LineEndingMode.Null.Name, StringComparison.OrdinalIgnoreCase))
      return LineEndingMode.Null;
    if (name.Equals(LineEndingMode.None.Name, StringComparison.OrdinalIgnoreCase))
      return LineEndingMode.None;
    throw new ArgumentOutOfRangeException(nameof (name), (object) name, "LineEndingMode is out of range");
  }

  public static bool operator ==(LineEndingMode mode1, LineEndingMode mode2)
  {
    if ((object) mode1 == null)
      return (object) mode2 == null;
    return (object) mode2 != null && mode1.NewLineCharacters == mode2.NewLineCharacters;
  }

  public static bool operator !=(LineEndingMode mode1, LineEndingMode mode2)
  {
    if ((object) mode1 == null)
      return mode2 != null;
    return (object) mode2 == null || mode1.NewLineCharacters != mode2.NewLineCharacters;
  }

  public override string ToString() => this.Name;

  public override int GetHashCode()
  {
    return this._newLineCharacters == null ? 0 : this._newLineCharacters.GetHashCode();
  }

  public override bool Equals(object obj)
  {
    if (obj == null)
      return false;
    if ((object) this == obj)
      return true;
    return obj is LineEndingMode other && this.Equals(other);
  }

  public bool Equals(LineEndingMode other)
  {
    if ((object) other == null)
      return false;
    return (object) this == (object) other || string.Equals(this._newLineCharacters, other._newLineCharacters);
  }

  public class LineEndingModeConverter : TypeConverter
  {
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {
      return sourceType == typeof (string) || base.CanConvertFrom(context, sourceType);
    }

    public override object ConvertFrom(
      ITypeDescriptorContext context,
      CultureInfo culture,
      object value)
    {
      return !(value is string name) ? base.ConvertFrom(context, culture, value) : (object) LineEndingMode.FromString(name);
    }
  }
}
