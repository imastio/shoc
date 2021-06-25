package io.imast.shoc.containerize;

import com.github.dockerjava.api.DockerClient;
import com.github.dockerjava.core.DefaultDockerClientConfig;
import com.github.dockerjava.core.DockerClientImpl;
import com.github.dockerjava.transport.DockerHttpClient;
import com.github.dockerjava.zerodep.ZerodepDockerHttpClient;
import io.vavr.control.Try;
import lombok.extern.slf4j.Slf4j;

/**
 * The containerization module
 * 
 * @author Davit.Petrosyan
 */
@Slf4j
public class Containerize {
    
    /**
     * The engine definition
     */
    private final EngineDefinition engine;
    
    /**
     * The registry definition
     */
    private final RegistryDefinition registry;
    
    /**
     * The HTTP client instance for docker engine
     */
    private final DockerHttpClient httpClient;
    
    /**
     * The docker client
     */
    private final DockerClient client;
    
    /**
     * Creates new instance of containerization module
     * 
     * @param engine The engine reference
     * @param registry The registry reference
     */
    public Containerize(EngineDefinition engine, RegistryDefinition registry){
        this.engine = engine;
        this.registry = registry;
        
        // build docker client configuration
        var config = DefaultDockerClientConfig.createDefaultConfigBuilder()
            .withDockerHost(this.engine.getAddress())
            .withDockerTlsVerify(this.engine.isTlsVerify())
            .withDockerCertPath(this.engine.getCertPath())
            .withRegistryUsername(this.registry.getUsername())
            .withRegistryPassword(this.registry.getPassword())
            .withRegistryEmail(this.registry.getEmail())
            .withRegistryUrl(this.registry.getRegistry())
            .build();
        
        // build docker client
        this.httpClient = new ZerodepDockerHttpClient.Builder()
            .dockerHost(config.getDockerHost())
            .sslConfig(config.getSSLConfig())
            .maxConnections(100)
            .build();
        
        this.client = DockerClientImpl.getInstance(config, this.httpClient);
    }    
    
    /**
     * Get the engine instance info
     * 
     * @return Returns engine instance info
     */
    public EngineInstanceInfo getInfo(){
        
        // try get info or null
        var info = Try.of(() -> this.client.infoCmd().exec())
                .onFailure(ex -> log.error(String.format("Error while doing operation with message %s", ex.getMessage())))
                .getOrNull();
        
        // no info so not running
        if(info == null){
            return EngineInstanceInfo.builder().running(false).build();
        }
        
        
        return EngineInstanceInfo.builder()
                .running(true)
                .id(info.getId())
                .name(info.getName())
                .images(info.getImages())
                .containers(info.getContainers())
                .driver(info.getDriver())
                .build();
    }
}
