using System.IO;
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
        /// Gets job by identifier
        /// </summary>
        /// <param name="id">The job identifier</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public Task<JobModel> Get(string id)
        {
            return this.jobService.GetById(this.HttpContext.GetShocPrincipal(), id);
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
        /// <param name="input">The job deploy input</param>
        /// <returns></returns>
        [HttpPost("deploy")]
        public Task<JobModel> Deploy([FromBody] DeployJobInput input)
        {
            return this.jobService.Deploy(this.HttpContext.GetShocPrincipal(), input);
        }

        /// <summary>
        /// Watch for job logs
        /// </summary>
        /// <param name="id">The job identifier</param>
        /// <returns></returns>
        [HttpGet("{id}/watch")]
        public async Task<Stream> WatchJob(string id)
        {
            return await this.jobService.WatchJob(this.HttpContext.GetShocPrincipal(), id);
        }
    }
}
