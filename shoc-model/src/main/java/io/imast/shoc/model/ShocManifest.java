package io.imast.shoc.model;

import java.util.List;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

/**
 * The shoc manifest model
 * 
 * @author Davit.Petrosyan
 */
@Data
@Builder(toBuilder = true)
@AllArgsConstructor
@NoArgsConstructor
public class ShocManifest {
    
    /**
     * The name of project
     */
    private String name;
    
    /**
     * The folder name
     */
    private String folder;
    
    /**
     * Labels of project
     */
    private List<String> labels;
    
    /**
     * The technology of application
     */
    private String technology;
    
    /**
     * Get technology flavor of application
     */
    private String flavor;
    
    /**
     * The set of packages to install additionally
     */
    private List<String> packages;
    
    /**
     * The paths to include in the final package
     */
    private List<String> includes;
    
    /**
     * The paths to exclude in the final package
     */
    private List<String> excludes;
    
    /**
     * The entry point of application
     */
    private String entrypoint;
    
    /**
     * The arguments to entry point
     */
    private String args;
}
