// Decompiled with JetBrains decompiler
// Type: NAudio.Utils.Decibels
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;

#nullable disable
namespace NAudio.Utils;

public class Decibels
{
  private const double LOG_2_DB = 8.6858896380650368;
  private const double DB_2_LOG = 0.11512925464970228;

  public static double LinearToDecibels(double lin) => Math.Log(lin) * 8.6858896380650368;

  public static double DecibelsToLinear(double dB) => Math.Exp(dB * 0.11512925464970228);
}
