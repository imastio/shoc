package io.imast.shoc.containerize;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

/**
 * The registry reference
 * 
 * @author Davit.Petrosyan
 */
@Data
@Builder(toBuilder = true)
@AllArgsConstructor
@NoArgsConstructor
public class RegistryDefinition {
    
    /**
     * The docker registry
     */
    private String registry;
    
    /**
     * The repository to group images
     */
    private String repository;
    
    /**
     * The docker registry username
     */
    private String username;
    
    /**
     * The docker registry password
     */
    private String password;
    
    /**
     * The docker registry email
     */
    private String email;
}
