using System;
using SIL.XForge.Models;

namespace SIL.Transcriber.Models
{
    public class TranscriberProjectUserEntity : ProjectUserEntity
    {
        public string SelectedTask { get; set; }
        public DateTime LastSyncedDate { get; set; } = DateTimeOffset.FromUnixTimeSeconds(0).UtcDateTime;
    }
}