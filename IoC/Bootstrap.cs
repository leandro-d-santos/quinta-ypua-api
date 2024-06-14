﻿using Data.Common;
using Data.Context;
using Domain.Common.Data;
using IoC.Users;
using Microsoft.Extensions.DependencyInjection;

namespace IoC
{
    public static class Bootstrap
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddDbContext<DbConnection>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddUsers();
        }
    }
}