using System;

namespace Okra.Builder
{
    public static class UseMiddlewareExtensions
    {
        public static IOkraAppBuilder UseMiddleware<TMiddleware, TOptions>(this IOkraAppBuilder app, TOptions options)
                where TMiddleware : IMiddleware<TOptions>
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));

            var applicationServices = app.ApplicationServices;

            return app.Use(next =>
            {
                IMiddleware<TOptions> middleware = applicationServices.GetService(typeof(TMiddleware)) as IMiddleware<TOptions>;
                middleware.Configure(next, options);

                return context =>
                {
                    return middleware.Invoke(context);
                };
            });
        }
    }
}
