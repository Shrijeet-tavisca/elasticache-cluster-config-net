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
using System.Linq;
using System.Text;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using Amazon.ElastiCacheCluster.Factories;
#if !CORE_CLR
using System.Configuration;
#endif

namespace Amazon.ElastiCacheCluster
{
#if CORE_CLR
    internal class ConfigurationPropertyAttribute : Attribute
    {
        internal ConfigurationPropertyAttribute(string name) { }
        public bool IsRequired { get; set; }
        public object DefaultValue { get; set; }
    }
#endif
    /// <summary>
    /// A config settings object used to configure the client config
    /// </summary>
    public class ClusterConfigSettings
#if !CORE_CLR
        : ConfigurationSection
#endif
    {
        /// <summary>
        /// An object that produces nodes for the Discovery Node, mainly used for testing
        /// </summary>
        public IConfigNodeFactory NodeFactory { get; set; }

#region Constructors

        /// <summary>
        /// For config manager
        /// </summary>
        public ClusterConfigSettings() { }

        /// <summary>
        /// Used to initialize a setup with a host and port
        /// </summary>
        /// <param name="hostname">Cluster hostname</param>
        /// <param name="port">Cluster port</param>
        public ClusterConfigSettings(string hostname, int port)
        {
            if (string.IsNullOrEmpty(hostname))
                throw new ArgumentNullException("hostname");
            if (port <= 0)
                throw new ArgumentException("Port cannot be less than or equal to zero");

            this.ClusterEndPoint.HostName = hostname;
            this.ClusterEndPoint.Port = port;
        }

#endregion

#region Config Settings

        /// <summary>
        /// Class containing information about the cluster host and port
        /// </summary>
        [ConfigurationProperty("endpoint", IsRequired = true)]
        public Endpoint ClusterEndPoint
#if CORE_CLR
        { get; set; }
#else
        {
            get { return (Endpoint)base["endpoint"]; }
            set { base["endpoint"] = value; }
        }
#endif

        /// <summary>
        /// Class containing information about the node configuration
        /// </summary>
        [ConfigurationProperty("node", IsRequired = false)]
        public NodeSettings ClusterNode
#if CORE_CLR
        { get; set; }
#else
        {
            get { return (NodeSettings)base["node"]; }
            set { base["node"] = value; }
        }
#endif

        /// <summary>
        /// Class containing information about the poller configuration
        /// </summary>
        [ConfigurationProperty("poller", IsRequired = false)]
        public PollerSettings ClusterPoller
#if CORE_CLR
        { get; set; }
#else
        {
            get { return (PollerSettings)base["poller"]; }
            set { base["poller"] = value; }
        }
#endif

        /// <summary>
        /// Endpoint that contains the hostname and port for auto discovery
        /// </summary>
        public class Endpoint
#if CORE_CLR
#else
            : ConfigurationElement
#endif
        {
            /// <summary>
            /// The hostname of the cluster containing ".cfg."
            /// </summary>
            [ConfigurationProperty("hostname", IsRequired = true)]
            public String HostName
#if CORE_CLR
            { get; set; }
#else
            {
                get
                {
                    return (String)this["hostname"];
                }
                set
                {
                    this["hostname"] = value;
                }
            }
#endif

            /// <summary>
            /// The port of the endpoint
            /// </summary>
            [ConfigurationProperty("port", IsRequired = true)]
            public int Port
#if CORE_CLR
            { get; set; }
#else
            {
                get
                {
                    return (int)this["port"];
                }
                set
                {
                    this["port"] = value;
                }
            }
#endif
        }

        /// <summary>
        /// Settings used for the discovery node
        /// </summary>
        public class NodeSettings
#if !CORE_CLR
            : ConfigurationElement
#endif
        {
            /// <summary>
            /// How many tries the node should use to get a config
            /// </summary>
            [ConfigurationProperty("nodeTries", DefaultValue = -1, IsRequired = false)]
            public int NodeTries
#if CORE_CLR
            { get; set; }
#else
            {
                get { return (int)base["nodeTries"]; }
                set { base["nodeTries"] = value; }
            }
#endif

            /// <summary>
            /// The delay between tries for the config in miliseconds
            /// </summary>
            [ConfigurationProperty("nodeDelay", DefaultValue = -1, IsRequired = false)]
            public int NodeDelay
#if CORE_CLR
            { get; set; }
#else
            {
                get { return (int)base["nodeDelay"]; }
                set { base["nodeDelay"] = value; }
            }
#endif
        }

        /// <summary>
        /// Settins used for the configuration poller
        /// </summary>
        public class PollerSettings
#if !CORE_CLR
            : ConfigurationElement
#endif
        {
            /// <summary>
            /// The delay between polls in miliseconds
            /// </summary>
            [ConfigurationProperty("intervalDelay", DefaultValue = -1, IsRequired = false)]
            public int IntervalDelay
#if CORE_CLR
            { get; set; }
#else
            {
                get { return (int)base["intervalDelay"]; }
                set { base["intervalDelay"] = value; }
            }
#endif
        }

#endregion

#region MemcachedConfig

        /// <summary>
        /// Gets or sets the configuration of the socket pool.
        /// </summary>
        [ConfigurationProperty("socketPool", IsRequired = false)]
#if CORE_CLR
        public ISocketPoolConfiguration SocketPool { get; set; }
#else
        public SocketPoolElement SocketPool
        {
            get { return (SocketPoolElement)base["socketPool"]; }
            set { base["socketPool"] = value; }
        }
#endif

        /// <summary>
        /// Gets or sets the configuration of the authenticator.
        /// </summary>
        [ConfigurationProperty("authentication", IsRequired = false)]
#if CORE_CLR
        public IAuthenticationConfiguration Authentication { get; set; }
#else
        public AuthenticationElement Authentication
        {
            get { return (AuthenticationElement)base["authentication"]; }
            set { base["authentication"] = value; }
        }
#endif

        /// <summary>
        /// Gets or sets the <see cref="T:Enyim.Caching.Memcached.IMemcachedNodeLocator"/> which will be used to assign items to Memcached nodes.
        /// </summary>
        [ConfigurationProperty("locator", IsRequired = false)]
#if CORE_CLR
        public Type NodeLocator { get; set; }
#else
        public ProviderElement<IMemcachedNodeLocator> NodeLocator
        {
            get { return (ProviderElement<IMemcachedNodeLocator>)base["locator"]; }
            set { base["locator"] = value; }
        }
#endif

        /// <summary>
        /// Gets or sets the <see cref="T:Enyim.Caching.Memcached.IMemcachedKeyTransformer"/> which will be used to convert item keys for Memcached.
        /// </summary>
        [ConfigurationProperty("keyTransformer", IsRequired = false)]
#if CORE_CLR
        public IMemcachedKeyTransformer KeyTransformer { get; set; }
#else
        public ProviderElement<IMemcachedKeyTransformer> KeyTransformer
        {
            get { return (ProviderElement<IMemcachedKeyTransformer>)base["keyTransformer"]; }
            set { base["keyTransformer"] = value; }
        }
#endif

        /// <summary>
        /// Gets or sets the <see cref="T:Enyim.Caching.Memcached.ITranscoder"/> which will be used serialzie or deserialize items.
        /// </summary>
        [ConfigurationProperty("transcoder", IsRequired = false)]
#if CORE_CLR
        public ITranscoder Transcoder { get; set; }
#else
        public ProviderElement<ITranscoder> Transcoder
        {
            get { return (ProviderElement<ITranscoder>)base["transcoder"]; }
            set { base["transcoder"] = value; }
        }
#endif

#if !CORE_CLR
        /// <summary>
        /// Gets or sets the <see cref="T:Enyim.Caching.Memcached.IPerformanceMonitor"/> which will be used monitor the performance of the client.
        /// </summary>
        [ConfigurationProperty("performanceMonitor", IsRequired = false)]
        public ProviderElement<IPerformanceMonitor> PerformanceMonitor
        {
            get { return (ProviderElement<IPerformanceMonitor>)base["performanceMonitor"]; }
            set { base["performanceMonitor"] = value; }
        }
#endif

        /// <summary>
        /// Gets or sets the type of the communication between client and server.
        /// </summary>
        [ConfigurationProperty("protocol", IsRequired = false, DefaultValue = MemcachedProtocol.Binary)]
        public MemcachedProtocol Protocol
#if CORE_CLR
        { get; set; }
#else
        {
            get { return (MemcachedProtocol)base["protocol"]; }
            set { base["protocol"] = value; }
        }
#endif

#endregion

    }
}