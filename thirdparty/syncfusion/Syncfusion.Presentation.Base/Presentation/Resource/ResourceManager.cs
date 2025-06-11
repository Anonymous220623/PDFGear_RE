// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Resource.ResourceManager
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System.IO;
using System.Reflection;

#nullable disable
namespace Syncfusion.Presentation.Resource;

internal class ResourceManager
{
  internal static Stream GetStreamFromResource(string fileName)
  {
    return ResourceManager.GetStreamFromResource(fileName, ".zip");
  }

  internal static Stream GetStreamFromResource(string fileName, string extension)
  {
    string str = "";
    return ResourceManager.GetAssembly().GetManifestResourceStream($"Syncfusion.Presentation{str}.Resource.{fileName}{extension}");
  }

  private static Assembly GetAssembly() => Assembly.GetExecutingAssembly();
}
