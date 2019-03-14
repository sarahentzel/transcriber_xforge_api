using JsonApiDotNetCore.Models;
using SIL.XForge.Models;

namespace SIL.Transcriber.Models
{
    public class TranscriberProjectUserResource : ProjectUserResource
    {
        [Attr]
        public string SelectedTask { get; set; }
    }
}
