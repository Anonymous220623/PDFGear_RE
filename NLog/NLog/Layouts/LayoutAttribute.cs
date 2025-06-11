// Decompiled with JetBrains decompiler
// Type: NLog.Layouts.LayoutAttribute
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using System;

#nullable disable
namespace NLog.Layouts;

[AttributeUsage(AttributeTargets.Class)]
public sealed class LayoutAttribute(string name) : NameBaseAttribute(name)
{
}
