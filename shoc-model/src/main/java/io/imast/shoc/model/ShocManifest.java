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
     * The kind of application
     */
    private String appKind;
    
    
}
