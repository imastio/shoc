﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Engine.Client;
using Shoc.Engine.Model;

namespace Shoc.Builder.Services
{
    /// <summary>
    /// The container engine interaction service
    /// </summary>
    public class EngineService
    {
        /// <summary>
        /// The engine client reference
        /// </summary>
        private readonly EngineClient engineClient;

        /// <summary>
        /// Creates new instance of engine interaction service
        /// </summary>
        /// <param name="engineClient">The engine client</param>
        public EngineService(EngineClient engineClient)
        {
            this.engineClient = engineClient;
        }

        /// <summary>
        /// Gets all the known engines in the system
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<EngineInstanceInfo>> GetAll()
        {
            return new[] { await this.engineClient.GetInfo()};
        }
    }
}