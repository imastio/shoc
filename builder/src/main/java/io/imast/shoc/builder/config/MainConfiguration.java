package io.imast.shoc.builder.config;

import com.mongodb.client.MongoClient;
import com.mongodb.client.MongoClients;
import com.mongodb.client.MongoDatabase;
import io.imast.shoc.containerize.Containerize;
import io.imast.shoc.containerize.EngineDefinition;
import io.imast.shoc.containerize.EngineType;
import io.imast.shoc.containerize.RegistryDefinition;
import lombok.extern.slf4j.Slf4j;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.context.annotation.Lazy;

/**
 * The main configuration beans
 * 
 * @author davitp
 */
@Configuration
@Slf4j
public class MainConfiguration {
    
    /**
     * The mongo URI
     */
    @Value("${imast.shoc.database.uri}")
    private String mongoUri;
    
    /**
     * The mongo database
     */
    @Value("${imast.shoc.database.db}")
    private String databaseName;
    
    /**
     * The docker engine address
     */
    @Value("${imast.shoc.engine.address}")
    private String dockerEngineAddress;
    
    /**
     * The docker engine version
     */
    @Value("${imast.shoc.engine.version}")
    private String dockerEngineVersion;
    
    /**
     * The docker engine TLS-verify
     */
    @Value("${imast.shoc.engine.tlsVerify}")
    private boolean dockerEngineTlsVerify;
    
    /**
     * The docker engine cert path
     */
    @Value("${imast.shoc.engine.certPath}")
    private String dockerEngineCertPath;
    
    /**
     * The docker registry address
     */
    @Value("${imast.shoc.registry.registry}")
    private String dockerRegistryAddress;
    
    /**
     * The docker registry repository
     */
    @Value("${imast.shoc.registry.repository}")
    private String dockerRegistryRepository;
    
    /**
     * The docker registry user name
     */
    @Value("${imast.shoc.registry.username}")
    private String dockerRegistryUsername;
    
    /**
     * The docker registry password
     */
    @Value("${imast.shoc.registry.password}")
    private String dockerRegistryPassword;
    
    /**
     * The docker registry email
     */
    @Value("${imast.shoc.registry.email}")
    private String dockerRegistryEmail;
    
    /**
     * The mongo client for communication
     * 
     * @return Returns mongo client
     */
    @Lazy
    @Bean
    public MongoClient mongoClient(){
        return MongoClients.create(this.mongoUri);
    }
    
    /**
     * The mongo database for communication
     * 
     * @return Returns mongo database
     */
    @Lazy
    @Bean
    public MongoDatabase mongoDatabase(){
        return this.mongoClient().getDatabase(this.databaseName);
    }
    
    /**
     * The docker engine definition
     * 
     * @return An instance to docker engine
     */
    @Bean
    public EngineDefinition dockerEngine(){
        return EngineDefinition.builder()
                .type(EngineType.DOCKER)
                .version(this.dockerEngineVersion)
                .address(this.dockerEngineAddress)
                .tlsVerify(this.dockerEngineTlsVerify)
                .certPath(this.dockerEngineCertPath)
                .build();
    }
    
    /**
     * The docker registry definition 
     * 
     * @return An instance of docker registry
     */
    @Bean
    public RegistryDefinition dockerRegistry(){
        return RegistryDefinition.builder()
                .registry(this.dockerRegistryAddress)
                .repository(this.dockerRegistryRepository)
                .username(this.dockerRegistryUsername)
                .password(this.dockerRegistryPassword)
                .email(this.dockerRegistryEmail)
                .build();
    }
    
    /**
     * An instance to containerization module
     * 
     * @return Returns instance to containerize
     */
    @Bean
    public Containerize dockerize(){
        return new Containerize(this.dockerEngine(), this.dockerRegistry());
    }
}
