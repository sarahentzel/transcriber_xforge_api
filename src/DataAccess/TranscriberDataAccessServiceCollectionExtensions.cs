﻿using Microsoft.Extensions.Configuration;
using SIL.XForge.DataAccess;
using SIL.XForge.Models;
using SIL.Transcriber.Models;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class TranscriberDataAccessServiceCollectionExtensions
    {
        public static IServiceCollection AddTranscriberDataAccess(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDataAccess(configuration);
            DataAccessClassMap.RegisterConcreteClass<ProjectUserEntity, TranscriberProjectUserEntity>();

            services.AddMongoRepository<TranscriberTaskEntity>("tasks");

            return services;
        }
    }
}
