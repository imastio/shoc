package io.imast.samples.scheduler.resources;

import io.imast.work4j.controller.SchedulerController;
import io.imast.work4j.model.JobDefinitionInput;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

/**
 * The remote controller for jobs
 * 
 * @author davitp
 */
@RestController
@RequestMapping("/api/v1/scheduler/jobs")
public class JobsController {
    
    /**
     * The job scheduler controller
     */
    @Autowired
    private SchedulerController schedulerController;
    
    /**
     * Gets all the jobs in the system
     * 
     * @param cluster The target cluster 
     * @param type The optional type parameter
     * @return Returns set of jobs
     */
    @GetMapping(path = "")
    public ResponseEntity<?> getAll( @RequestParam(required = false) String cluster, @RequestParam(required = false) String type){
        return ResponseEntity.ok(this.schedulerController.getAllJobs(cluster, type));
    }
        
    /**
     * Gets all job definitions by page 
     * 
     * @param cluster The target cluster 
     * @param type The optional type parameter 
     * @param page The page
     * @param size The page size
     * @return Returns set of job definitions
     */
    @GetMapping(path = "", params = {"page", "size"})
    public ResponseEntity<?> getPage(@RequestParam(required = false) String cluster, @RequestParam(required = false) String type, @RequestParam(required = true) Integer page, @RequestParam(required = true) Integer size){
        return ResponseEntity.ok(this.schedulerController.getJobPage(cluster, type, page, size));
    }
    
    /**
     * Find the job by id
     * 
     * @param id The ID of requested job
     * @return Returns found job
     */
    @GetMapping(path = "{id}")
    public ResponseEntity<?> getById(@PathVariable String id){
        return ResponseEntity.of(this.schedulerController.getJobById(id));
    }
    
    /**
     * Add a job definition to controller
     * 
     * @param definition The job definition to add
     * @param replace The flag indicates if job should be replaced
     * @return Returns added entity
     */
    @PostMapping(path = "")
    public ResponseEntity<?> postOne(@RequestBody JobDefinitionInput definition, @RequestParam(required = false) boolean replace){
        return ResponseEntity.ok(this.schedulerController.insertJob(definition, replace));
    }
    
    /**
     * Update a job definition to controller
     * 
     * @param id The ID of job definition to update
     * @param definition The job definition to add
     * @return Returns added entity
     */
    @PutMapping(path = "{id}")
    public ResponseEntity<?> updateOne(@PathVariable String id, @RequestBody JobDefinitionInput definition){
        return ResponseEntity.ok(this.schedulerController.updateJob(id, definition));
    }
    
    /**
     * Delete job from scheduler
     * 
     * @param id The job id to remove
     * @return Returns removed job
     */
    @DeleteMapping(path = "{id}")
    public ResponseEntity<?> delete(@PathVariable String id){
        return ResponseEntity.of(this.schedulerController.deleteJobById(id));
    }
    
    /**
     * Delete job from scheduler by path
     * 
     * @param folder The target folder of entry
     * @param name The name of entry
     * @return Returns removed job
     */
    @DeleteMapping(path = "", params = { "folder", "name"})
    public ResponseEntity<?> deleteByPath(@RequestParam(required = true) String folder, @RequestParam(required = true) String name){
        return ResponseEntity.of(this.schedulerController.deleteJobByPath(folder, name));
    }
    
    /**
     * Delete all the jobs from scheduler
     * 
     * @param all The safety flag to confirm
     * @return Returns removed job
     */
    @DeleteMapping(path = "", params = { "all"})
    public ResponseEntity<?> deleteByPath(@RequestParam(required = true) boolean all){
        return ResponseEntity.ok(this.schedulerController.deleteAllJobs());
    }
}
