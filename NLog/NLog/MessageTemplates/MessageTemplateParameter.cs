// Decompiled with JetBrains decompiler
// Type: NLog.MessageTemplates.MessageTemplateParameter
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using JetBrains.Annotations;
using System;

#nullable disable
namespace NLog.MessageTemplates;

public struct MessageTemplateParameter
{
  [NotNull]
  public string Name { get; }

  [CanBeNull]
  public object Value { get; }

  [CanBeNull]
  public string Format { get; }

  public CaptureType CaptureType { get; }

  public int? PositionalIndex
  {
    get
    {
      switch (this.Name)
      {
        case "0":
          return new int?(0);
        case "1":
          return new int?(1);
        case "2":
          return new int?(2);
        case "3":
          return new int?(3);
        case "4":
          return new int?(4);
        case "5":
          return new int?(5);
        case "6":
          return new int?(6);
        case "7":
          return new int?(7);
        case "8":
          return new int?(8);
        case "9":
          return new int?(9);
        default:
          string name = this.Name;
          int result;
          return (name != null ? (name.Length >= 1 ? 1 : 0) : 0) != 0 && this.Name[0] >= '0' && this.Name[0] <= '9' && int.TryParse(this.Name, out result) ? new int?(result) : new int?();
      }
    }
  }

  internal MessageTemplateParameter([NotNull] string name, object value, string format)
  {
    this.Name = name ?? throw new ArgumentNullException(nameof (name));
    this.Value = value;
    this.Format = format;
    this.CaptureType = CaptureType.Normal;
  }

  public MessageTemplateParameter(
    [NotNull] string name,
    object value,
    string format,
    CaptureType captureType)
  {
    this.Name = name ?? throw new ArgumentNullException(nameof (name));
    this.Value = value;
    this.Format = format;
    this.CaptureType = captureType;
  }
}
