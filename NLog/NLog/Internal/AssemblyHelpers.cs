// Decompiled with JetBrains decompiler
// Type: NLog.Internal.AssemblyHelpers
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;

#nullable disable
namespace NLog.Internal;

internal static class AssemblyHelpers
{
  public static Assembly LoadFromPath(string assemblyFileName, string baseDirectory = null)
  {
    string assemblyFile = baseDirectory == null ? assemblyFileName : Path.Combine(baseDirectory, assemblyFileName);
    InternalLogger.Info<string>("Loading assembly file: {0}", assemblyFile);
    return Assembly.LoadFrom(assemblyFile);
  }

  public static Assembly LoadFromName(string assemblyName)
  {
    InternalLogger.Info<string>("Loading assembly: {0}", assemblyName);
    try
    {
      return Assembly.Load(assemblyName);
    }
    catch (FileNotFoundException ex)
    {
      AssemblyName name = new AssemblyName(assemblyName);
      InternalLogger.Trace<string>("Try find '{0}' in current domain", assemblyName);
      Assembly assembly = ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).FirstOrDefault<Assembly>((Func<Assembly, bool>) (domainAssembly => AssemblyHelpers.IsAssemblyMatch(name, domainAssembly.GetName())));
      if (assembly != (Assembly) null)
      {
        InternalLogger.Trace<string>("Found '{0}' in current domain", assemblyName);
        return assembly;
      }
      InternalLogger.Trace<string>("Haven't found' '{0}' in current domain", assemblyName);
      throw;
    }
  }

  private static bool IsAssemblyMatch(AssemblyName expected, AssemblyName actual)
  {
    if (expected.Name != actual.Name || expected.Version != (Version) null && expected.Version != actual.Version || expected.CultureInfo != null && expected.CultureInfo.Name != actual.CultureInfo.Name)
      return false;
    byte[] publicKeyToken = expected.GetPublicKeyToken();
    return publicKeyToken == null || ((IEnumerable<byte>) publicKeyToken).SequenceEqual<byte>((IEnumerable<byte>) actual.GetPublicKeyToken());
  }

  public static string GetAssemblyFileLocation(Assembly assembly)
  {
    string str = string.Empty;
    try
    {
      if (assembly == (Assembly) null)
        return string.Empty;
      str = assembly.FullName;
      Uri result;
      if (!Uri.TryCreate(assembly.CodeBase, UriKind.RelativeOrAbsolute, out result))
      {
        InternalLogger.Warn<string, string>("Ignoring assembly location because code base is unknown: '{0}' ({1})", assembly.CodeBase, str);
        return string.Empty;
      }
      string directoryName = Path.GetDirectoryName(result.LocalPath);
      if (string.IsNullOrEmpty(directoryName))
      {
        InternalLogger.Warn<string, string>("Ignoring assembly location because it is not a valid directory: '{0}' ({1})", result.LocalPath, str);
        return string.Empty;
      }
      DirectoryInfo directoryInfo = new DirectoryInfo(directoryName);
      if (!directoryInfo.Exists)
      {
        InternalLogger.Warn<string, string>("Ignoring assembly location because directory doesn't exists: '{0}' ({1})", directoryName, str);
        return string.Empty;
      }
      InternalLogger.Debug<string, string>("Found assembly location directory: '{0}' ({1})", directoryInfo.FullName, str);
      return directoryInfo.FullName;
    }
    catch (PlatformNotSupportedException ex)
    {
      InternalLogger.Warn((Exception) ex, "Ignoring assembly location because assembly lookup is not supported: {0}", (object) str);
      if (!ex.MustBeRethrown())
        return string.Empty;
      throw;
    }
    catch (SecurityException ex)
    {
      InternalLogger.Warn((Exception) ex, "Ignoring assembly location because assembly lookup is not allowed: {0}", (object) str);
      if (!ex.MustBeRethrown())
        return string.Empty;
      throw;
    }
    catch (UnauthorizedAccessException ex)
    {
      InternalLogger.Warn((Exception) ex, "Ignoring assembly location because assembly lookup is not allowed: {0}", (object) str);
      if (!ex.MustBeRethrown())
        return string.Empty;
      throw;
    }
  }
}
