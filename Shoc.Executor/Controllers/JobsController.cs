using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.ApiCore.Protection;
using Shoc.Executor.Model.Job;
using Shoc.Executor.Services;

namespace Shoc.Executor.Controllers
{
    /// <summary>
    /// The jobs controller
    /// </summary>
    [Route("api/jobs")]
    [ApiController]
    [ShocExceptionHandler]
    [AuthorizedSubject]
    public class JobsController : ControllerBase
    {
        /// <summary>
        /// The job service
        /// </summary>
        private readonly JobService jobService;

        /// <summary>
        /// Creates new instance of jobs controller
        /// </summary>
        /// <param name="jobService">The projects service</param>
        public JobsController(JobService jobService)
        {
            this.jobService = jobService;
        }

        /// <summary>
        /// Creates new job
        /// </summary>
        /// <param name="input">The job create input</param>
        /// <returns></returns>
        [HttpPost]
        public Task<JobModel> Create([FromBody] CreateJobInput input)
        {
            return this.jobService.Create(this.HttpContext.GetShocPrincipal(), input);
        }

        /// <summary>
        /// Deploys the job
        /// </summary>
        /// <param name="id">The job identifier</param>
        /// <returns></returns>
        [HttpPost("{id}")]
        public Task<JobModel> Deploy(string id)
        {
            return this.jobService.Deploy(this.HttpContext.GetShocPrincipal(), id);
        }

        /// <summary>
        /// Watch for job logs
        /// </summary>
        /// <param name="id">The job identifier</param>
        /// <returns></returns>
        [HttpGet("{id}/watch")]
        public async Task<object> WatchJob(string id)
        {
            return new 
                { Logs = await this.jobService.WatchJob(this.HttpContext.GetShocPrincipal(), id) };
        }
    }
}
