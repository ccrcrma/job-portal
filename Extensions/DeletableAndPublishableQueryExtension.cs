using System;
using System.Linq.Expressions;
using System.Reflection;
using job_portal.Interfaces;
using job_portal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace job_portal.Extensions
{
    public static class DeletableAndPublishabelQueryExtension
    {
        public static void AddSoftDeleteAndPublishedQueryFilter(
        this IMutableEntityType entityData)
        {
            var methodToCall = typeof(DeletableAndPublishabelQueryExtension)
                .GetMethod(nameof(GetDeleteAndPublishedFilter),
                    BindingFlags.NonPublic | BindingFlags.Static)
                .MakeGenericMethod(entityData.ClrType);
            var filter = methodToCall.Invoke(null, new object[] { });
            entityData.SetQueryFilter((LambdaExpression)filter);
        }

        private static LambdaExpression GetDeleteAndPublishedFilter<TEntity>() where TEntity : PublishableEntity, ISoftDelete
        {

            Expression<Func<TEntity, bool>> filter = x => !x.IsSoftDeleted && x.Status == Types.PublishedStatus.Live;
            return filter;
        }
    }
}