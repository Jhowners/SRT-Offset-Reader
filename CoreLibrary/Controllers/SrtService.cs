using SrtChanger.Models;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SrtChanger.Controllers
{
    public class SrtService : ISrtService
    {
        public List<SrtSubtitle> Parse(string content)
        {
            var subtitles = new List<SrtSubtitle>();
            var lines = content.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            var index = 1; // Subtitles in SRT files are 1-based index
            SrtSubtitle? subtitle = null;

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    if (subtitle != null)
                    {
                        subtitles.Add(subtitle);
                        index++;
                    }
                    subtitle = null;
                }
                else if (subtitle == null)
                {
                    subtitle = new SrtSubtitle();
                    subtitle.Index = index;
                }
                else if (subtitle.StartTime == TimeSpan.Zero)
                {
                    var times = line.Split("-->");
                    if (times.Length == 2)
                    {
                        subtitle.StartTime = ParseTime(times[0].Trim());
                        subtitle.EndTime = ParseTime(times[1].Trim());

                    }

                }
                else
                {
                    if (!string.IsNullOrEmpty(subtitle.Text))
                    {
                        subtitle.Text += "\n";
                    }
                    subtitle.Text += line;
                }
            }

            if (subtitle != null)
            {
                subtitles.Add(subtitle);
            }

            return subtitles;
        }

        private TimeSpan ParseTime(string time)
        {
            var regex = new Regex(@"(\d{2}):(\d{2}):(\d{2}),(\d{3})");
            var match = regex.Match(time.Trim());

            if (!match.Success)
            {
                Console.WriteLine($"Format type '{time}' invalid.");
                return TimeSpan.Zero;
            }

            try
            {
                bool validHours = int.TryParse(match.Groups[1].Value, out int hours);
                bool validMinutes = int.TryParse(match.Groups[2].Value, out int minutes);
                bool validSeconds = int.TryParse(match.Groups[3].Value, out int seconds);
                bool validMilliseconds = int.TryParse(match.Groups[4].Value, out int milliseconds);

                if (!validHours || !validMinutes || !validSeconds || !validMilliseconds)
                {
                    Console.WriteLine($"One or plus '{time}' are invalid.");
                    return TimeSpan.Zero;
                }

                return new TimeSpan(0, hours, minutes, seconds, milliseconds);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing timeline '{time}': {ex.Message}");
                return TimeSpan.Zero;
            }
        }


        public void AddOffset(List<SrtSubtitle> subtitles, TimeSpan offset)
        {
            foreach (var subtitle in subtitles)
            {

                subtitle.StartTime = subtitle.StartTime.Add(offset);
                subtitle.EndTime = subtitle.EndTime.Add(offset);


            }
        }


        public string Save(List<SrtSubtitle> subtitles)
        {
            var content = string.Empty;
            foreach (var subtitle in subtitles)
            {
                content += subtitle.ToString();
            }
            return content;
        }
    }
}
