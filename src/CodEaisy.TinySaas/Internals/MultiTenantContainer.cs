using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Autofac.Core.Lifetime;
using Autofac.Core.Resolving;
using CodEaisy.TinySaas.Interfaces;

namespace CodEaisy.TinySaas.Internals
{
    /// <summary>
    /// multitenant application container
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class MultitenantContainer<T> : IContainer where T : ITenant
    {
        //This is the base application container
        private readonly IContainer _applicationContainer;
        //This action configures a container builder
        private readonly Action<T, ContainerBuilder> _tenantContainerConfiguration;

        //This dictionary keeps track of all of the tenant scopes that we have created
        private readonly ConcurrentDictionary<string, Lazy<ILifetimeScope>> _tenantLifetimeScopes = new ConcurrentDictionary<string, Lazy<ILifetimeScope>>();

        private const string _multiTenantTag = "multitenantcontainer";

        public IDisposer Disposer => _applicationContainer.Disposer;

        public object Tag => _applicationContainer.Tag;

        public IComponentRegistry ComponentRegistry => _applicationContainer.ComponentRegistry;

        public DiagnosticListener DiagnosticSource => _applicationContainer.DiagnosticSource;

        /// <summary>
        /// create a new instance of multitenant container
        /// </summary>
        /// <param name="applicationContainer"></param>
        /// <param name="containerConfiguration"></param>
        public MultitenantContainer(IContainer applicationContainer, Action<T, ContainerBuilder> containerConfiguration)
        {
            _tenantContainerConfiguration = containerConfiguration;
            _applicationContainer = applicationContainer;
        }

        /// <summary>
        /// handle `LifetimeScopeBeginning` event
        /// </summary>
        public event EventHandler<LifetimeScopeBeginningEventArgs> ChildLifetimeScopeBeginning
        {
            add
            {
                _applicationContainer.ChildLifetimeScopeBeginning += value;
            }

            remove
            {
                _applicationContainer.ChildLifetimeScopeBeginning -= value;
            }
        }

        /// <summary>
        /// handle `LifetimeScopeEnding` event
        /// </summary>
        public event EventHandler<LifetimeScopeEndingEventArgs> CurrentScopeEnding
        {
            add
            {
                _applicationContainer.CurrentScopeEnding += value;
            }

            remove
            {
                _applicationContainer.CurrentScopeEnding -= value;
            }
        }

        /// <summary>
        /// handle `ResolveOperationBeginning` event
        /// </summary>
        public event EventHandler<ResolveOperationBeginningEventArgs> ResolveOperationBeginning
        {
            add
            {
                _applicationContainer.ResolveOperationBeginning += value;
            }

            remove
            {
                _applicationContainer.ResolveOperationBeginning -= value;
            }
        }

        /// <summary>
        /// Get the current tenant from the application container
        /// </summary>
        /// <returns>currently resolved tenant</returns>
        private T GetCurrentTenant() => _applicationContainer.Resolve<ITenantService<T>>()
            .GetTenant()
            .GetAwaiter()
            .GetResult();

        /// <summary>
        /// Get the scope of the current tenant
        /// </summary>
        /// <returns>litetime scope</returns>
        public ILifetimeScope GetCurrentTenantScope() =>
            GetTenantScope(GetCurrentTenant()?.Identifier);

        /// <summary>
        /// Get (configure on missing)
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns>lifetime scope</returns>
        public ILifetimeScope GetTenantScope(string tenantId)
        {
            //If no tenant (e.g. early on in the pipeline, we just use the application container)
            if (string.IsNullOrEmpty(tenantId))
                return _applicationContainer;

            // fetch existing tenant lifetime scope or create a new one
            return _tenantLifetimeScopes.GetOrAdd(tenantId, id => new Lazy<ILifetimeScope>(() => _applicationContainer.BeginLifetimeScope(_multiTenantTag, a => _tenantContainerConfiguration(GetCurrentTenant(), a)))).Value;
        }

        public ILifetimeScope BeginLifetimeScope() => _applicationContainer.BeginLifetimeScope();

        public ILifetimeScope BeginLifetimeScope(object tag) =>
            _applicationContainer.BeginLifetimeScope(tag);

        public ILifetimeScope BeginLifetimeScope(Action<ContainerBuilder> configurationAction) =>
            _applicationContainer.BeginLifetimeScope(configurationAction);

        public ILifetimeScope BeginLifetimeScope(object tag,
            Action<ContainerBuilder> configurationAction) =>
            _applicationContainer.BeginLifetimeScope(tag, configurationAction);

        public object ResolveComponent(ResolveRequest request) =>
            _applicationContainer.ResolveComponent(request);

        public ValueTask DisposeAsync() => _applicationContainer.DisposeAsync();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var scope in _tenantLifetimeScopes)
                    scope.Value?.Value?.Dispose();
                _applicationContainer.Dispose();
            }
        }
    }
}
