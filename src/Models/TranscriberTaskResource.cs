using SIL.XForge.Models;
using JsonApiDotNetCore.Models;

namespace SIL.Transcriber.Models
{
    public class TranscriberTaskResource :  Resource
    {
        [Attr]
        public string TaskID { get; set; }
        [Attr]
        public string Name { get; set; }
        [Attr]
        public string PassageSet { get; set; }
        [Attr]
        public string Book { get; set; }
        [Attr]
        public string Description { get; set; }
    }
}
