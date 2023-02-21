using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Core;
using Shoc.Executor.Model.Job;

namespace Shoc.Executor.Data.Sql
{
    /// <summary>
    /// The job repository implementation
    /// </summary>
    public class JobRepository : IJobRepository
    {
        /// <summary>
        /// The data operations instance
        /// </summary>
        private readonly DataOperations dataOps;

        /// <summary>
        /// Creates new instance of project repository implementation
        /// </summary>
        /// <param name="dataOps">A DataOps instance</param>
        public JobRepository(DataOperations dataOps)
        {
            this.dataOps = dataOps;
        }

        /// <summary>
        /// Gets job by id
        /// </summary>
        /// <param name="id">The job id</param>
        /// <returns></returns>
        public Task<JobModel> GetById(string id)
        {
            // get object from the database
            return this.dataOps.Connect().QueryFirst("Job", "GetById").ExecuteAsync<JobModel>(new 
            {
                JobId = id
            });
        }

        /// <summary>
        /// Creates a new package with given input
        /// </summary>
        /// <param name="input">The package creation input</param>
        /// <returns></returns>
        public Task<JobModel> Create(CreateJobInput input)
        {
            // generate id if necessary
            input.Id ??= StdIdGenerator.Next(ExecutorObjects.JOB)?.ToLowerInvariant();

            // add object to the database
            return this.dataOps.Connect().QueryFirst("Job", "Create").ExecuteAsync<JobModel>(input);
        }

        /// <summary>
        /// Updates the package status based on input
        /// </summary>
        /// <param name="input">The package status update model</param>
        /// <returns></returns>
        public Task<JobModel> UpdateStatus(JobStatusModel input)
        {
            // do update
            return this.dataOps.Connect().QueryFirst("Job", "UpdateStatus").ExecuteAsync<JobModel>(input);
        }
    }
}