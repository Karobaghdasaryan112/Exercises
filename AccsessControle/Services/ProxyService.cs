using AccsessControl.Attributes;
using AccsessControl.Enums;
using System.Reflection;


namespace AccsessControl.Services
{
    public class ProxyService<T> : DispatchProxy
    {
        private T? _service;
        private Roles _roles;
        public void SetService(T? service)
        {
            _service = service;
        }
        public void SetCurrentRole(Roles roles)
        {
            _roles = roles;
        }
        protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
        {
            if (targetMethod == null)
                throw new ArgumentNullException(nameof(targetMethod));

            var AttributesAuth = targetMethod.GetCustomAttributes<CustomAuthorize>();
            if (AttributesAuth != null)
            {
                if (AttributesAuth.Where(Attr => Attr.Role == _roles)?.FirstOrDefault() == null)
                {
                        throw new InvalidOperationException($"you can't have succsess on {targetMethod.Name} Method");
                }
                Console.WriteLine("Succsessfully Authorize");
                return targetMethod.Invoke(_service, args);
            }
            throw new InvalidOperationException($"you haven't Any Role to Succsess {targetMethod.Name} method");

        }

    }
}
