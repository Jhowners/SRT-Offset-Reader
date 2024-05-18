using System;

namespace SrtChanger.Models
{
    public class SrtSubtitle
    {
        public int Index { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string? Text { get; set; }

        public override string ToString()
        {
            return $"{Index}\n{StartTime:hh\\:mm\\:ss\\,fff} --> {EndTime:hh\\:mm\\:ss\\,fff}\n{Text}\n";
        }
    }
}
