// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.ColorPickers.ColorPickerTemplateSettings
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Controls.ColorPickers;

public class ColorPickerTemplateSettings : DependencyObject
{
  private static object defaultColorFamiliesLocker = new object();
  private static IReadOnlyList<ColorFamily> defaultColorFamilies;
  public static readonly DependencyProperty RecentColorsProperty;
  private static readonly DependencyPropertyKey RecentColorsPropertyKey = DependencyProperty.RegisterReadOnly(nameof (RecentColors), typeof (IReadOnlyList<ColorValue>), typeof (ColorPickerTemplateSettings), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty RecentColorsKeyProperty = DependencyProperty.Register(nameof (RecentColorsKey), typeof (string), typeof (ColorPickerTemplateSettings), new PropertyMetadata((object) null, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is ColorPickerTemplateSettings templateSettings2) || object.Equals(a.NewValue, a.OldValue))
      return;
    templateSettings2.UpdateRecentColors();
  })));
  public static readonly DependencyProperty IsTransparentColorEnabledProperty = DependencyProperty.Register(nameof (IsTransparentColorEnabled), typeof (bool), typeof (ColorPickerTemplateSettings), new PropertyMetadata((object) false, new PropertyChangedCallback(ColorPickerTemplateSettings.OnTransparentColorEnabledChanged)));
  public static readonly DependencyProperty StandardColorsProperty = DependencyProperty.Register(nameof (StandardColors), typeof (IList<ColorValue>), typeof (ColorPickerTemplateSettings), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty ColorFamily1Property = DependencyProperty.Register(nameof (ColorFamily1), typeof (ColorFamily), typeof (ColorPickerTemplateSettings), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty ColorFamily2Property = DependencyProperty.Register(nameof (ColorFamily2), typeof (ColorFamily), typeof (ColorPickerTemplateSettings), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty ColorFamily3Property = DependencyProperty.Register(nameof (ColorFamily3), typeof (ColorFamily), typeof (ColorPickerTemplateSettings), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty ColorFamily4Property = DependencyProperty.Register(nameof (ColorFamily4), typeof (ColorFamily), typeof (ColorPickerTemplateSettings), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty ColorFamily5Property = DependencyProperty.Register(nameof (ColorFamily5), typeof (ColorFamily), typeof (ColorPickerTemplateSettings), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty ColorFamily6Property = DependencyProperty.Register(nameof (ColorFamily6), typeof (ColorFamily), typeof (ColorPickerTemplateSettings), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty ColorFamily7Property = DependencyProperty.Register(nameof (ColorFamily7), typeof (ColorFamily), typeof (ColorPickerTemplateSettings), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty ColorFamily8Property = DependencyProperty.Register(nameof (ColorFamily8), typeof (ColorFamily), typeof (ColorPickerTemplateSettings), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty ColorFamily9Property = DependencyProperty.Register(nameof (ColorFamily9), typeof (ColorFamily), typeof (ColorPickerTemplateSettings), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty ColorFamily10Property = DependencyProperty.Register(nameof (ColorFamily10), typeof (ColorFamily), typeof (ColorPickerTemplateSettings), new PropertyMetadata((PropertyChangedCallback) null));

  static ColorPickerTemplateSettings()
  {
    ColorPickerTemplateSettings.RecentColorsProperty = ColorPickerTemplateSettings.RecentColorsPropertyKey.DependencyProperty;
  }

  public ColorPickerTemplateSettings()
  {
    ColorPickerTemplateSettings.InitDefaultColors();
    this.ColorFamily1 = ColorPickerTemplateSettings.defaultColorFamilies[0];
    this.ColorFamily2 = ColorPickerTemplateSettings.defaultColorFamilies[1];
    this.ColorFamily3 = ColorPickerTemplateSettings.defaultColorFamilies[2];
    this.ColorFamily4 = ColorPickerTemplateSettings.defaultColorFamilies[3];
    this.ColorFamily5 = ColorPickerTemplateSettings.defaultColorFamilies[4];
    this.ColorFamily6 = ColorPickerTemplateSettings.defaultColorFamilies[5];
    this.ColorFamily7 = ColorPickerTemplateSettings.defaultColorFamilies[6];
    this.ColorFamily8 = ColorPickerTemplateSettings.defaultColorFamilies[7];
    this.ColorFamily9 = ColorPickerTemplateSettings.defaultColorFamilies[8];
    this.ColorFamily10 = ColorPickerTemplateSettings.defaultColorFamilies[9];
  }

  public IReadOnlyList<ColorValue> RecentColors
  {
    get
    {
      return (IReadOnlyList<ColorValue>) this.GetValue(ColorPickerTemplateSettings.RecentColorsProperty);
    }
    private set
    {
      this.SetValue(ColorPickerTemplateSettings.RecentColorsPropertyKey, (object) value);
    }
  }

  public string RecentColorsKey
  {
    get => (string) this.GetValue(ColorPickerTemplateSettings.RecentColorsKeyProperty);
    set => this.SetValue(ColorPickerTemplateSettings.RecentColorsKeyProperty, (object) value);
  }

  internal void UpdateRecentColors()
  {
    if (!string.IsNullOrEmpty(this.RecentColorsKey))
    {
      IReadOnlyList<string> result = ConfigManager.GetColorPickerRecentColorsAsync(this.RecentColorsKey, new CancellationToken()).GetAwaiter().GetResult();
      List<ColorValue> colorValueList = new List<ColorValue>();
      for (int index = 0; index < result.Count; ++index)
      {
        try
        {
          Color color = (Color) ColorConverter.ConvertFromString(result[index]);
          colorValueList.Add((ColorValue) color);
        }
        catch
        {
        }
      }
      if (colorValueList.Count > 0)
        this.RecentColors = (IReadOnlyList<ColorValue>) colorValueList;
      else
        this.RecentColors = (IReadOnlyList<ColorValue>) null;
    }
    else
      this.RecentColors = (IReadOnlyList<ColorValue>) null;
  }

  public bool IsTransparentColorEnabled
  {
    get => (bool) this.GetValue(ColorPickerTemplateSettings.IsTransparentColorEnabledProperty);
    set
    {
      this.SetValue(ColorPickerTemplateSettings.IsTransparentColorEnabledProperty, (object) value);
    }
  }

  public IList<ColorValue> StandardColors
  {
    get => (IList<ColorValue>) this.GetValue(ColorPickerTemplateSettings.StandardColorsProperty);
    set => this.SetValue(ColorPickerTemplateSettings.StandardColorsProperty, (object) value);
  }

  private static void OnTransparentColorEnabledChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ColorPickerTemplateSettings sender) || (bool) e.NewValue == (bool) e.OldValue)
      return;
    EventHandler colorEnabledChanged = sender.TransparentColorEnabledChanged;
    if (colorEnabledChanged == null)
      return;
    colorEnabledChanged((object) sender, EventArgs.Empty);
  }

  public event EventHandler TransparentColorEnabledChanged;

  public ColorFamily ColorFamily1
  {
    get => (ColorFamily) this.GetValue(ColorPickerTemplateSettings.ColorFamily1Property);
    set => this.SetValue(ColorPickerTemplateSettings.ColorFamily1Property, (object) value);
  }

  public ColorFamily ColorFamily2
  {
    get => (ColorFamily) this.GetValue(ColorPickerTemplateSettings.ColorFamily2Property);
    set => this.SetValue(ColorPickerTemplateSettings.ColorFamily2Property, (object) value);
  }

  public ColorFamily ColorFamily3
  {
    get => (ColorFamily) this.GetValue(ColorPickerTemplateSettings.ColorFamily3Property);
    set => this.SetValue(ColorPickerTemplateSettings.ColorFamily3Property, (object) value);
  }

  public ColorFamily ColorFamily4
  {
    get => (ColorFamily) this.GetValue(ColorPickerTemplateSettings.ColorFamily4Property);
    set => this.SetValue(ColorPickerTemplateSettings.ColorFamily4Property, (object) value);
  }

  public ColorFamily ColorFamily5
  {
    get => (ColorFamily) this.GetValue(ColorPickerTemplateSettings.ColorFamily5Property);
    set => this.SetValue(ColorPickerTemplateSettings.ColorFamily5Property, (object) value);
  }

  public ColorFamily ColorFamily6
  {
    get => (ColorFamily) this.GetValue(ColorPickerTemplateSettings.ColorFamily6Property);
    set => this.SetValue(ColorPickerTemplateSettings.ColorFamily6Property, (object) value);
  }

  public ColorFamily ColorFamily7
  {
    get => (ColorFamily) this.GetValue(ColorPickerTemplateSettings.ColorFamily7Property);
    set => this.SetValue(ColorPickerTemplateSettings.ColorFamily7Property, (object) value);
  }

  public ColorFamily ColorFamily8
  {
    get => (ColorFamily) this.GetValue(ColorPickerTemplateSettings.ColorFamily8Property);
    set => this.SetValue(ColorPickerTemplateSettings.ColorFamily8Property, (object) value);
  }

  public ColorFamily ColorFamily9
  {
    get => (ColorFamily) this.GetValue(ColorPickerTemplateSettings.ColorFamily9Property);
    set => this.SetValue(ColorPickerTemplateSettings.ColorFamily9Property, (object) value);
  }

  public ColorFamily ColorFamily10
  {
    get => (ColorFamily) this.GetValue(ColorPickerTemplateSettings.ColorFamily10Property);
    set => this.SetValue(ColorPickerTemplateSettings.ColorFamily10Property, (object) value);
  }

  private static void InitDefaultColors()
  {
    if (ColorPickerTemplateSettings.defaultColorFamilies != null)
      return;
    lock (ColorPickerTemplateSettings.defaultColorFamiliesLocker)
    {
      if (ColorPickerTemplateSettings.defaultColorFamilies != null)
        return;
      ColorFamily[] colorFamilyArray = new ColorFamily[10];
      ColorPickerTemplateSettings.defaultColorFamilies = (IReadOnlyList<ColorFamily>) colorFamilyArray;
      colorFamilyArray[0] = new ColorFamily((ColorValue) hex(16777215U /*0xFFFFFF*/), (IEnumerable<ColorValue>) new ColorValue[5]
      {
        (ColorValue) hex(16119285U /*0xF5F5F5*/),
        (ColorValue) hex(14803425U /*0xE1E1E1*/),
        (ColorValue) hex(13487565U /*0xCDCDCD*/),
        (ColorValue) hex(10855845U /*0xA5A5A5*/),
        (ColorValue) hex(8882055U /*0x878787*/)
      });
      colorFamilyArray[1] = new ColorFamily((ColorValue) hex(0U), (IEnumerable<ColorValue>) new ColorValue[5]
      {
        (ColorValue) hex(8553090U /*0x828282*/),
        (ColorValue) hex(5921370U /*0x5A5A5A*/),
        (ColorValue) hex(3947580U /*0x3C3C3C*/),
        (ColorValue) hex(2631720U /*0x282828*/),
        (ColorValue) hex(1315860U /*0x141414*/)
      });
      colorFamilyArray[2] = new ColorFamily((ColorValue) hex(15724002U), (IEnumerable<ColorValue>) new ColorValue[5]
      {
        (ColorValue) hex(15066072U),
        (ColorValue) hex(12958873U),
        (ColorValue) hex(9800279U),
        (ColorValue) hex(4867116U),
        (ColorValue) hex(1973264U)
      });
      colorFamilyArray[3] = new ColorFamily((ColorValue) hex(2115962U), (IEnumerable<ColorValue>) new ColorValue[5]
      {
        (ColorValue) hex(13162992U),
        (ColorValue) hex(9484001U),
        (ColorValue) hex(5738707U),
        (ColorValue) hex(1981534U),
        (ColorValue) hex(1123389U)
      });
      colorFamilyArray[4] = new ColorFamily((ColorValue) hex(5276347U), (IEnumerable<ColorValue>) new ColorValue[5]
      {
        (ColorValue) hex(11855615U),
        (ColorValue) hex(10539775U),
        (ColorValue) hex(8565997U),
        (ColorValue) hex(4618417U),
        (ColorValue) hex(1328767U)
      });
      colorFamilyArray[5] = new ColorFamily((ColorValue) hex(12603727U), (IEnumerable<ColorValue>) new ColorValue[5]
      {
        (ColorValue) hex(16765905U),
        (ColorValue) hex(16755625U),
        (ColorValue) hex(15893377U),
        (ColorValue) hex(11945797U),
        (ColorValue) hex(7998217U)
      });
      colorFamilyArray[6] = new ColorFamily((ColorValue) hex(10271326U), (IEnumerable<ColorValue>) new ColorValue[5]
      {
        (ColorValue) hex(15528672U),
        (ColorValue) hex(14083775U),
        (ColorValue) hex(12834718U),
        (ColorValue) hex(8955466U),
        (ColorValue) hex(5665816U)
      });
      colorFamilyArray[7] = new ColorFamily((ColorValue) hex(8413851U), (IEnumerable<ColorValue>) new ColorValue[5]
      {
        (ColorValue) hex(15393776U),
        (ColorValue) hex(13876455U),
        (ColorValue) hex(11045571U),
        (ColorValue) hex(5782131U),
        (ColorValue) hex(3150411U)
      });
      colorFamilyArray[8] = new ColorFamily((ColorValue) hex(5221829U), (IEnumerable<ColorValue>) new ColorValue[5]
      {
        (ColorValue) hex(12384255U),
        (ColorValue) hex(12050153U),
        (ColorValue) hex(6537689U),
        (ColorValue) hex(1932179U),
        (ColorValue) hex(23925U)
      });
      colorFamilyArray[9] = new ColorFamily((ColorValue) hex(16159825U), (IEnumerable<ColorValue>) new ColorValue[5]
      {
        (ColorValue) hex(16770209U),
        (ColorValue) hex(16762499U),
        (ColorValue) hex(16754789U),
        (ColorValue) hex(15164942U),
        (ColorValue) hex(12870175U)
      });
    }

    static Color hex(uint _rgb) => ColorHelper.FromRgb(_rgb);
  }
}
