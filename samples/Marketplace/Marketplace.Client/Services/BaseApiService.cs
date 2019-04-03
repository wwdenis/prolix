﻿using Marketplace.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prolix.Core.Domain;
using Prolix.Client.Services;

namespace Marketplace.Client.Services
{
    public abstract class BaseApiService<ModelType> : ApiService<ModelType>
        where ModelType : Model, new()
    {
        public BaseApiService(ApplicationContext context)
        {
            Context = context;
            BaseUrl = context.BaseUrl;
        }

        ApplicationContext Context { get; }
    }
}
