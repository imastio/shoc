package io.imast.shoc.containerize;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

/**
 * The engine instance
 * 
 * @author davitp
 */
@Data
@Builder(toBuilder = true)
@NoArgsConstructor
@AllArgsConstructor
public class EngineInstanceInfo {
 
    /**
     * The engine id
     */
    private String id;
    
    /**
     * The engine name
     */
    private String name;
    
    /**
     * The engine driver
     */
    private String driver;
    
    /**
     * Indicates if engine is in running state
     */
    private boolean running;
    
    /**
     * The number of images in engine
     */
    private Integer images;
    
    /**
     * The number of containers in engine
     */
    private Integer containers;
    
}
