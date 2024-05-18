using SrtChanger.Models;
using System;
using System.Collections.Generic;

namespace SrtChanger.Controllers
{
    public interface ISrtService
    {
        List<SrtSubtitle> Parse(string content);
        void AddOffset(List<SrtSubtitle> subtitles, TimeSpan offset);
        string Save(List<SrtSubtitle> subtitles);
    }
}
