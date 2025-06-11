// Decompiled with JetBrains decompiler
// Type: NLog.LogReceiverService.NLogEvents
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Xml.Serialization;

#nullable disable
namespace NLog.LogReceiverService;

[DataContract(Name = "events", Namespace = "http://nlog-project.org/ws/")]
[XmlType(Namespace = "http://nlog-project.org/ws/")]
[XmlRoot("events", Namespace = "http://nlog-project.org/ws/")]
[DebuggerDisplay("Count = {Events.Length}")]
public class NLogEvents
{
  [DataMember(Name = "cli", Order = 0)]
  [XmlElement("cli", Order = 0)]
  public string ClientName { get; set; }

  [DataMember(Name = "bts", Order = 1)]
  [XmlElement("bts", Order = 1)]
  public long BaseTimeUtc { get; set; }

  [DataMember(Name = "lts", Order = 100)]
  [XmlArray("lts", Order = 100)]
  [XmlArrayItem("l")]
  public StringCollection LayoutNames { get; set; }

  [DataMember(Name = "str", Order = 200)]
  [XmlArray("str", Order = 200)]
  [XmlArrayItem("l")]
  public StringCollection Strings { get; set; }

  [DataMember(Name = "ev", Order = 1000)]
  [XmlArray("ev", Order = 1000)]
  [XmlArrayItem("e")]
  public NLogEvent[] Events { get; set; }

  public IList<LogEventInfo> ToEventInfo(string loggerNamePrefix)
  {
    LogEventInfo[] eventInfo = new LogEventInfo[this.Events.Length];
    for (int index = 0; index < eventInfo.Length; ++index)
      eventInfo[index] = this.Events[index].ToEventInfo(this, loggerNamePrefix);
    return (IList<LogEventInfo>) eventInfo;
  }

  public IList<LogEventInfo> ToEventInfo() => this.ToEventInfo(string.Empty);
}
