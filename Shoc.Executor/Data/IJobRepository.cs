using System.Threading.Tasks;
using Shoc.Executor.Model.Job;

namespace Shoc.Executor.Data
{
    /// <summary>
    /// The job repository interface
    /// </summary>
    public interface IJobRepository
    {
        /// <summary>
        /// Gets job by id
        /// </summary>
        /// <param name="id">The job id</param>
        /// <returns></returns>
        Task<JobModel> GetById(string id);

        /// <summary>
        /// Creates the job by given input
        /// </summary>
        /// <param name="input">The job creation input</param>
        /// <returns></returns>
        Task<JobModel> Create(CreateJobInput input);

        /// <summary>
        /// Updates the package status based on input
        /// </summary>
        /// <param name="input">The package status update model</param>
        /// <returns></returns>
        Task<JobModel> UpdateStatus(JobStatusModel input);
    }
}