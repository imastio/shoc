package io.imast.shoc.builder.resources;

import io.imast.shoc.builder.services.EngineService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

/**
 * The container engines controller
 * 
 * @author davitp
 */
@RestController
@RequestMapping("/api/v1/engines")
public class EngineController {
       
    /**
     * The engine service
     */
    @Autowired
    private EngineService engineService;
    
    /**
     * Gets all engine instances
     * 
     * @return Returns set engines
     */
    @GetMapping(path = "")
    public ResponseEntity<?> getAll(){
        return ResponseEntity.ok(this.engineService.getAll());
    }
}
