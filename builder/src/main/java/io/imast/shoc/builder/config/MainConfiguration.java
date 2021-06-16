package io.imast.shoc.builder.config;

import com.mongodb.client.MongoClient;
import com.mongodb.client.MongoClients;
import com.mongodb.client.MongoDatabase;
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
}
