using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Dime.Multitenancy.Internal
{
    public class TenantPipelineMiddleware<TTenant>
    {
        private readonly RequestDelegate _next;
        private readonly IApplicationBuilder _rootApp;
        private readonly Action<TenantPipelineBuilderContext<TTenant>, IApplicationBuilder> _configuration;

        private readonly ConcurrentDictionary<TTenant, Lazy<RequestDelegate>> _pipelines
            = new ConcurrentDictionary<TTenant, Lazy<RequestDelegate>>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="rootApp"></param>
        /// <param name="configuration"></param>
        public TenantPipelineMiddleware(
            RequestDelegate next,
            IApplicationBuilder rootApp,
            Action<TenantPipelineBuilderContext<TTenant>, IApplicationBuilder> configuration)
        {
            Ensure.Argument.NotNull(next, nameof(next));
            Ensure.Argument.NotNull(rootApp, nameof(rootApp));
            Ensure.Argument.NotNull(configuration, nameof(configuration));

            this._next = next;
            this._rootApp = rootApp;
            this._configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            Ensure.Argument.NotNull(context, nameof(context));

            TenantContext<TTenant> tenantContext = context.GetTenantContext<TTenant>();

            if (tenantContext != null)
            {
                var tenantPipeline = _pipelines.GetOrAdd(
                    tenantContext.Tenant,
                    new Lazy<RequestDelegate>(() => BuildTenantPipeline(tenantContext)));

                await tenantPipeline.Value(context);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tenantContext"></param>
        /// <returns></returns>
        private RequestDelegate BuildTenantPipeline(TenantContext<TTenant> tenantContext)
        {
            IApplicationBuilder branchBuilder = _rootApp.New();

            TenantPipelineBuilderContext<TTenant> builderContext = new TenantPipelineBuilderContext<TTenant>
            {
                TenantContext = tenantContext,
                Tenant = tenantContext.Tenant
            };

            _configuration(builderContext, branchBuilder);

            // register root pipeline at the end of the tenant branch
            branchBuilder.Run(_next);

            return branchBuilder.Build();
        }
    }
}