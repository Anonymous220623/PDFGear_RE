// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Properties.SearchResource
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;
using System.Diagnostics;
using System.Reflection;

#nullable disable
namespace Patagames.Pdf.Properties;

internal class SearchResource
{
  public static Assembly FindResource(ref string name)
  {
    try
    {
      Patagames.Activation.Log.Info($"Search for {name} resource in assemblies");
      string str1 = (string) null;
      try
      {
        str1 = BitConverter.ToString(typeof (SearchResource).Assembly.GetName().GetPublicKeyToken());
      }
      catch (Exception ex)
      {
        Patagames.Activation.Log.Error("Cant't retrieve PublicToken for patagtames assembly: {0}", (object) ex.Message);
      }
      StackFrame[] frames = new StackTrace().GetFrames();
      if (frames != null)
      {
        Patagames.Activation.Log.Info("Found {0} frames", (object) frames.Length);
        for (int index = 0; index < frames.Length; ++index)
        {
          if (frames[index] == null)
          {
            Patagames.Activation.Log.Info("Frame {0} is null", (object) index);
          }
          else
          {
            MethodBase method = frames[index].GetMethod();
            if (method != (MethodBase) null)
            {
              Type declaringType = method.DeclaringType;
              if (declaringType != (Type) null)
              {
                Assembly assembly = declaringType.Assembly;
                try
                {
                  byte[] publicKeyToken = assembly.GetName().GetPublicKeyToken();
                  if (publicKeyToken != null)
                  {
                    if (str1 != null)
                    {
                      if (str1 == BitConverter.ToString(publicKeyToken))
                      {
                        Patagames.Activation.Log.Info("Skipped: {0}", (object) assembly.GetName().ToString());
                        continue;
                      }
                    }
                  }
                }
                catch (Exception ex)
                {
                  Patagames.Activation.Log.Error("Cant't compare public tokens: {0}", (object) ex.Message);
                }
                string[] manifestResourceNames = assembly.GetManifestResourceNames();
                if (manifestResourceNames != null)
                {
                  foreach (string str2 in manifestResourceNames)
                  {
                    if (str2.EndsWith(name + ".resources", StringComparison.OrdinalIgnoreCase))
                    {
                      name = str2.Substring(0, str2.Length - 10);
                      return assembly;
                    }
                  }
                }
              }
              else
                Patagames.Activation.Log.Info("Type is null", (object) index);
            }
            else
              Patagames.Activation.Log.Info("Method is null", (object) index);
          }
        }
      }
      else
        Patagames.Activation.Log.Info("There is no any frames");
      Patagames.Activation.Log.Info("The resource was not found");
      return (Assembly) null;
    }
    catch (Exception ex)
    {
      Patagames.Activation.Log.Error($"Search for {name} resource at RunTime Error: {{0}} {{1}}", (object) ex.Message, (object) ex.StackTrace);
    }
    return (Assembly) null;
  }
}
