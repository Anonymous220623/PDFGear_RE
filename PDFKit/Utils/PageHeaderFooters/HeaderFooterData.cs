// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PageHeaderFooters.HeaderFooterData
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using PDFKit.Utils.XObjects;
using System;
using System.Collections.Generic;

#nullable disable
namespace PDFKit.Utils.PageHeaderFooters;

public class HeaderFooterData
{
  public HeaderFooterData.HeaderFooterSettingsData SettingsData { get; internal set; }

  public Dictionary<int, IReadOnlyList<int>> FormObjects { get; internal set; }

  public class HeaderFooterSettingsData : IEquatable<HeaderFooterData.HeaderFooterSettingsData>
  {
    private static HeaderFooterData.HeaderFooterSettingsData emptyData = new HeaderFooterData.HeaderFooterSettingsData()
    {
      isEmpty = true
    };
    private bool isEmpty = false;
    private HeaderFooterSettings settings;
    private string settingsXml;

    public static HeaderFooterData.HeaderFooterSettingsData EmptyData
    {
      get => HeaderFooterData.HeaderFooterSettingsData.emptyData;
    }

    public HeaderFooterSettings Settings
    {
      get => this.settings;
      internal set
      {
        if (this.isEmpty)
          throw new ArgumentException("isEmpty");
        this.settings = value;
      }
    }

    public string SettingsXml
    {
      get => !this.isEmpty ? this.settingsXml : "";
      internal set
      {
        if (this.isEmpty)
          throw new ArgumentException("isEmpty");
        this.settingsXml = value;
      }
    }

    public override int GetHashCode()
    {
      HeaderFooterSettings settings = this.Settings;
      return settings != null ? settings.GetHashCode() : base.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      return obj is HeaderFooterData.HeaderFooterSettingsData footerSettingsData && footerSettingsData.Equals(this);
    }

    public bool Equals(HeaderFooterData.HeaderFooterSettingsData other)
    {
      if (other == null)
        return false;
      return this.Settings == null && other.Settings == null || this.isEmpty && other.isEmpty || object.Equals((object) this.Settings, (object) other.Settings);
    }
  }
}
