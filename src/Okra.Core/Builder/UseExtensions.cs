using Okra.Lifetime;
using System;
using System.Threading.Tasks;

namespace Okra.Builder
{
    // Based upon https://github.com/aspnet/HttpAbstractions/blob/dev/src/Microsoft.AspNetCore.Http.Abstractions/Extensions/UseExtensions.cs

    public static class UseExtensions
    {
        public static IOkraAppBuilder Use(this IOkraAppBuilder app, Func<AppLaunchContext, Func<Task>, Task> middleware)
        {
            return app.Use(next =>
             {
                 return context =>
                 {
                     Func<Task> simpleNext = () => next(context);
                     return middleware(context, simpleNext);
                 };
             });

        }
    }
}
