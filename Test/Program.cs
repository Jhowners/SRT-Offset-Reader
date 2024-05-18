using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using SrtChanger;
using SrtChanger.Controllers;
using SrtChanger.Models;

namespace SRTOffsetChanger.ExecutionControl
{
    public class Execute
    {
        static void Main(string[] args)
        {

            string filePath = "Insert the path here";

            string newFilePath = "Insert ur path here";

            // Load SRT File
            string content = File.ReadAllText(filePath);

            // Parsing
            var srtService = new SrtService();
            var subtitles = srtService.Parse(content);

            // Adding Offset
            TimeSpan offset = TimeSpan.FromSeconds(5);
            srtService.AddOffset(subtitles, offset);

            // Saving a new SRT with selected offset
            File.WriteAllText(newFilePath, srtService.Save(subtitles));

            Console.WriteLine("New SRT file with updated offset saved in: " + newFilePath);

        }

    }
}
