/*
 * Copyright 2014 Amazon.com, Inc. or its affiliates. All Rights Reserved.
 * 
 * Portions copyright 2010 Attila Kiskó, enyim.com. Please see LICENSE.txt
 * for applicable license terms and NOTICE.txt for applicable notices.
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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;
using Enyim.Caching.Configuration;
using Enyim.Collections;
using System.Security;
using Enyim.Caching.Memcached;
using Enyim.Caching.Memcached.Protocol.Binary;

namespace Amazon.ElastiCacheCluster.Pools
{
    /// <summary>
    /// Server pool implementing the binary protocol.
    /// </summary>
    internal class AutoBinaryPool : AutoServerPool
    {
        ISaslAuthenticationProvider authenticationProvider;
        ElastiCacheClusterConfig configuration;

        public AutoBinaryPool(ElastiCacheClusterConfig configuration)
#if CORE_CLR
            : base(configuration, new BinaryOperationFactory(configuration.Logger))
#else
            : base(configuration, new BinaryOperationFactory())
#endif
        {
            this.authenticationProvider = GetProvider(configuration);
            this.configuration = configuration;
        }

#if CORE_CLR
        protected override IMemcachedNode CreateNode(EndPoint endpoint)
#else
        protected override IMemcachedNode CreateNode(IPEndPoint endpoint)
#endif

        {
            if (endpoint == null)
                throw new ArgumentNullException("endpoint");
#if CORE_CLR
            return new BinaryNode(endpoint, this.configuration.SocketPool, this.authenticationProvider, this.configuration.Logger);
#else
            return new BinaryNode(endpoint, this.configuration.SocketPool, this.authenticationProvider);
#endif
        }

        private static ISaslAuthenticationProvider GetProvider(IMemcachedClientConfiguration configuration)
        {
            // create&initialize the authenticator, if any
            // we'll use this single instance everywhere, so it must be thread safe
            IAuthenticationConfiguration auth = configuration.Authentication;
            if (auth != null)
            {
                Type t = auth.Type;
                var provider = (t == null) ? null : Enyim.Reflection.FastActivator.Create(t) as ISaslAuthenticationProvider;

                if (provider != null)
                {
                    provider.Initialize(auth.Parameters);
                    return provider;
                }
            }

            return null;
        }

    }
}
