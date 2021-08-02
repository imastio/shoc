package io.imast.shoc.builder.services;

import io.imast.shoc.containerize.Containerize;
import io.imast.shoc.containerize.EngineInstanceInfo;
import java.util.Arrays;
import java.util.List;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

/**
 * The container engine service
 * 
 * @author davitp
 */
@Service
public class EngineService {
    
    /**
     * Get the dockerize instance
     */
    @Autowired
    private Containerize dockerize;
    
    /**
     * Gets all engine healths
     * 
     * @return Returns set of engine health instances
     */
    public List<EngineInstanceInfo> getAll(){
        return Arrays.asList(this.dockerize.getInfo());
    }
}
