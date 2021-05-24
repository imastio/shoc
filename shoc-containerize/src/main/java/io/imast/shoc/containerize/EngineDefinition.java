package io.imast.shoc.containerize;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

/**
 * The configuration options for containerization
 * 
 * @author Davit.Petrosyan
 */
@Data
@Builder(toBuilder = true)
@NoArgsConstructor
@AllArgsConstructor
public class EngineDefinition {
    
    /**
     * The host (and port) address of the engine API (sock file or port)
     */
    private String address;
    
    /**
     * Should TLS be verified on connection
     */
    private boolean tlsVerify;
    
    /**
     * The docker certificate path
     */
    private String certPath;
    
    /**
     * The version of API
     */
    private String version;
    
    /**
     * The type of engine
     */
    private EngineType type;
}
