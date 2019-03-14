using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using JsonApiDotNetCore.Services;
using JsonApiDotNetCore.Internal.Query;
using SIL.Transcriber.Models;
using SIL.XForge.DataAccess;
using SIL.XForge.Models;
using SIL.XForge.Services;
using SIL.XForge.Utils;
using Microsoft.Extensions.Options;
using SIL.XForge.Configuration;

namespace SIL.Transcriber.Services
{
    public class TranscriberTaskService : RepositoryResourceServiceBase<TranscriberTaskResource, TranscriberTaskEntity>
    {
        private readonly IOptions<SiteOptions> _siteOptions;

        public TranscriberTaskService(IJsonApiContext jsonApiContext, IMapper mapper, IUserAccessor userAccessor,
            IRepository<TranscriberTaskEntity> tasks, IOptions<SiteOptions> siteOptions)
            : base(jsonApiContext, mapper, userAccessor, tasks)
        {
            _siteOptions = siteOptions;
        }

        protected override IRelationship<TranscriberTaskEntity> GetRelationship(string relationshipName)
        {
            /*
            switch (relationshipName)
            {
                case nameof(TranscriberTaskResource.Projects):
                    return OneToMany(ProjectUserMapper, u => u.UserRef);
            }
            */
            return base.GetRelationship(relationshipName);
        }

        
        protected override IQueryable<TranscriberTaskEntity> ApplyFilter(IQueryable<TranscriberTaskEntity> entities,
            FilterQuery filter)
        {
            if (filter.Attribute == "search")
            {
                string value = filter.Value.ToLowerInvariant();
                return entities.Where(u => u.Name.ToLowerInvariant().Contains(value) || u.Description.Contains(value));
            }
            return base.ApplyFilter(entities, filter);
        }

        protected override Task<TranscriberTaskEntity> InsertEntityAsync(TranscriberTaskEntity entity)
        {
            /*if (!string.IsNullOrEmpty(entity.Username))
                entity.Username = UserEntity.NormalizeUsername(entity.Username);
            if (!string.IsNullOrEmpty(entity.Password))
                entity.Password = UserEntity.HashPassword(entity.Password);
            entity.CanonicalEmail = UserEntity.CanonicalizeEmail(entity.Email);
            */
            return base.InsertEntityAsync(entity);
        }

        protected override void UpdateAttribute(IUpdateBuilder<TranscriberTaskEntity> update, string name, object value)
        {
            switch (name)
            {
                case nameof(TranscriberTaskResource.Book):
                    if (value == null)
                        update.Unset(u => u.Book);
                    else
                        update.Set(u => u.Book, (string)value);
                    break;
                case nameof(TranscriberTaskResource.Description):
                    if (value == null)
                        update.Unset(u => u.Description);
                    else
                        update.Set(u => u.Description, (string)value);
                    break;
                default:
                    base.UpdateAttribute(update, name, value);
                    break;
            }
        }

        protected override Task CheckCanCreateAsync(TranscriberTaskResource resource)
        {
            if (SystemRole == SystemRoles.User)
                throw ForbiddenException();
            return Task.CompletedTask;
        }

        protected override Task CheckCanUpdateAsync(string id, IDictionary<string, object> attrs,
            IDictionary<string, string> relationships)
        {
            return CheckCanUpdateDeleteAsync(id);
        }

        protected override Task CheckCanUpdateRelationshipAsync(string id)
        {
            return CheckCanUpdateDeleteAsync(id);
        }

        protected override Task CheckCanDeleteAsync(string id)
        {
            return CheckCanUpdateDeleteAsync(id);
        }

        protected override Task<IQueryable<TranscriberTaskEntity>> ApplyPermissionFilterAsync(IQueryable<TranscriberTaskEntity> query)
        {
            if (SystemRole == SystemRoles.User)
                query = query.Where(u => u.Id == UserId);
            return Task.FromResult(query);
        }

        private Task CheckCanUpdateDeleteAsync(string id)
        {
            if (SystemRole == SystemRoles.User && id != UserId)
                throw ForbiddenException();
            return Task.CompletedTask;
        }
    }


}
