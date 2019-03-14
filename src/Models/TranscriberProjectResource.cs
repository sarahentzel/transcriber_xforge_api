using System;
using System.Collections.Generic;
using JsonApiDotNetCore.Models;
using SIL.XForge.Models;

namespace SIL.Transcriber.Models
{
    public class TranscriberProjectResource : ProjectResource
    {
        [Attr(isImmutable: true)]
        public string ParatextId { get; set; }
        public DateTime LastSyncedDate { get; set; }
    }
}