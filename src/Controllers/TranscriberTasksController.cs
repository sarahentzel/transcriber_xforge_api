using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIL.Transcriber.Models;
using SIL.Transcriber.Services;
using SIL.XForge.Controllers;
using JsonApiDotNetCore.Services;
using Microsoft.Extensions.Logging;

namespace SIL.Transcriber.Controllers
{
    [Route("tasks")]
    public class TranscriberTasksController : JsonApiControllerBase<TranscriberTaskResource>
    {
        public TranscriberTasksController(
                IJsonApiContext jsonApiContext, 
                IResourceService<TranscriberTaskResource, string> resourceService,
                ILoggerFactory loggerFactory) 
            : base(jsonApiContext, resourceService, loggerFactory)
        {
        }
    }
}