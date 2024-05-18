using SrtChanger.Controllers;
using SrtChanger.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace SrtChanger.Tests
{
    public class SrtServiceTests
    {
        private readonly SrtService _srtService;

        public SrtServiceTests()
        {
            _srtService = new SrtService();
        }

        [Fact]
        public void Parse_ValidContent_ReturnsSubtitles()
        {
            var content = "1\n00:00:01,000 --> 00:00:02,000\nHello\n\n2\n00:00:03,000 --> 00:00:04,000\nWorld\n";
            var subtitles = _srtService.Parse(content);

            Assert.Equal(2, subtitles.Count);
            Assert.Equal("Hello", subtitles[0].Text);
            Assert.Equal("World", subtitles[1].Text);
        }

        [Fact]
        public void Resync_ValidOffset_OffsetsTime()
        {
            var subtitles = new List<SrtSubtitle>
            {
                new SrtSubtitle { StartTime = TimeSpan.FromSeconds(1), EndTime = TimeSpan.FromSeconds(2), Text = "Hello\n" },
                new SrtSubtitle { StartTime = TimeSpan.FromSeconds(3), EndTime = TimeSpan.FromSeconds(4), Text = "World\n" }
            };

            _srtService.AddOffset(subtitles, TimeSpan.FromSeconds(1));

            Assert.Equal(TimeSpan.FromSeconds(2), subtitles[0].StartTime);
            Assert.Equal(TimeSpan.FromSeconds(3), subtitles[0].EndTime);
            Assert.Equal(TimeSpan.FromSeconds(4), subtitles[1].StartTime);
            Assert.Equal(TimeSpan.FromSeconds(5), subtitles[1].EndTime);
        }

        [Fact]
        public void Save_ValidSubtitles_ReturnsContent()
        {
            var subtitles = new List<SrtSubtitle>
            {
                new SrtSubtitle { Index = 1, StartTime = TimeSpan.FromSeconds(1), EndTime = TimeSpan.FromSeconds(2), Text = "Hello\n" },
                new SrtSubtitle { Index = 2, StartTime = TimeSpan.FromSeconds(3), EndTime = TimeSpan.FromSeconds(4), Text = "World\n" }
            };

            var content = _srtService.Save(subtitles);

            var expectedContent = "1\n00:00:01,000 --> 00:00:02,000\nHello\n\n2\n00:00:03,000 --> 00:00:04,000\nWorld\n\n";
            Assert.Equal(expectedContent, content);
        }
    }
}
