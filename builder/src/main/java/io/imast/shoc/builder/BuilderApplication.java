package io.imast.shoc.builder;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.autoconfigure.security.servlet.SecurityAutoConfiguration;

/**
 * The schedule management application
 * 
 * @author davitp
 */
@SpringBootApplication(exclude = { SecurityAutoConfiguration.class })
public class BuilderApplication {

    /**
     * The entry point for "Builder Application"
     * 
     * @param args The arguments
     */
    public static void main(String[] args) {
        // create an application
        SpringApplication.run(BuilderApplication.class, args);
    }
}
