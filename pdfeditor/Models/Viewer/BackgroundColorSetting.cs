// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Viewer.BackgroundColorSetting
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.Properties;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Models.Viewer;

public class BackgroundColorSetting
{
  public BackgroundColorSetting(
    string name,
    string displayNameResourceKey,
    string defaultDisplayName,
    Color backgroundColor,
    Color pageMaskColor)
  {
    this.Name = name;
    try
    {
      this.DisplayName = Resources.ResourceManager.GetString(displayNameResourceKey, Resources.Culture);
    }
    catch
    {
    }
    if (this.DisplayName == null)
      this.DisplayName = defaultDisplayName;
    this.BackgroundColor = backgroundColor;
    this.PageMaskColor = pageMaskColor;
  }

  public BackgroundColorSetting(
    string name,
    string displayNameResourceKey,
    string defaultDisplayName,
    string backgroundColor,
    string pageMaskColor)
    : this(name, displayNameResourceKey, defaultDisplayName, (Color) ColorConverter.ConvertFromString(backgroundColor), (Color) ColorConverter.ConvertFromString(pageMaskColor))
  {
  }

  public string Name { get; }

  public string DisplayName { get; }

  public Color BackgroundColor { get; set; }

  public Color PageMaskColor { get; }
}
