// Decompiled with JetBrains decompiler
// Type: NLog.Internal.FactoryHelper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;

#nullable disable
namespace NLog.Internal;

internal class FactoryHelper
{
  private FactoryHelper()
  {
  }

  internal static object CreateInstance(Type t)
  {
    try
    {
      return Activator.CreateInstance(t);
    }
    catch (MissingMethodException ex)
    {
      throw new NLogConfigurationException($"Cannot access the constructor of type: {t.FullName}. Is the required permission granted?", (Exception) ex);
    }
  }
}
