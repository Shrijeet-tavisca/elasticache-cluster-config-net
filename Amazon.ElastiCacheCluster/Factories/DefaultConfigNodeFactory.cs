/*
 * Copyright 2014 Amazon.com, Inc. or its affiliates. All Rights Reserved.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License").
 * You may not use this file except in compliance with the License.
 * A copy of the License is located at
 * 
 *  http://aws.amazon.com/apache2.0
 * 
 * or in the "license" file accompanying this file. This file is distributed
 * on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
 * express or implied. See the License for the specific language governing
 * permissions and limitations under the License.
 */
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Enyim.Caching;
#if NETSTANDARD
using Microsoft.Extensions.Logging;
#endif

namespace Amazon.ElastiCacheCluster.Factories
{
    internal class DefaultConfigNodeFactory : IConfigNodeFactory
    {
#if NETSTANDARD
        readonly ILogger logger;
        public DefaultConfigNodeFactory(ILogger logger)
        {
            this.logger = logger;
        }
#endif
        public IMemcachedNode CreateNode(IPEndPoint endpoint, ISocketPoolConfiguration config)
        {
#if NETSTANDARD
            return new MemcachedNode(endpoint, config, logger);
#else
            return new MemcachedNode(endpoint, config);
#endif
        }
    }
}
