package io.imast.shoc.builder.config;

import com.google.common.collect.Lists;
import java.security.Principal;
import java.util.List;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import springfox.documentation.builders.PathSelectors;
import static springfox.documentation.builders.PathSelectors.regex;
import springfox.documentation.service.ApiInfo;
import springfox.documentation.service.ApiKey;
import springfox.documentation.service.AuthorizationScope;
import springfox.documentation.service.SecurityReference;
import springfox.documentation.spi.DocumentationType;
import springfox.documentation.spi.service.contexts.SecurityContext;
import springfox.documentation.spring.web.plugins.Docket;
import springfox.documentation.swagger2.annotations.EnableSwagger2;

/**
 * The swagger configuration
 * 
 * @author davitp
 */
@Configuration
@EnableSwagger2
public class SwaggerConfig { 
    
    /**
     * The client ID
     */
    @Value("${spring.application.name}")
    private String applicationName;
    
    /**
     * The Auth header
     */
    public static final String AUTHORIZATION_HEADER = "Authorization";
    
    /**
     * Paths to include
     */
    public static final String DEFAULT_INCLUDE_PATTERN = "/api/.*";
    
    List<SecurityReference> defaultAuth() {
        return Lists.newArrayList(new SecurityReference("JWT", new AuthorizationScope[] {
            new AuthorizationScope("read", "The read scope"), 
            new AuthorizationScope("write", "The write scope")}));
    }
    
    /**
     * The swagger security context
     * 
     * @return The swagger security context
     */
    private SecurityContext securityContext() {
        return SecurityContext.builder()
            .securityReferences(this.defaultAuth())
            .forPaths(PathSelectors.regex(DEFAULT_INCLUDE_PATTERN))
            .build();
    }
     
    /**
     * The API Key descriptor
     * 
     * @return The API Key
     */
    private ApiKey apiKey() {
        return new ApiKey("JWT", AUTHORIZATION_HEADER, "header");
    }
     
    /**
     * Swagger configuration for API
     * 
     * @return Returns docket
     */
    @Bean
    public Docket api() { 
        return new Docket(DocumentationType.SWAGGER_2)
            .pathMapping("/")
            .apiInfo(this.getApiInfo())
            .forCodeGeneration(true)
            .ignoredParameterTypes(Principal.class)
            .securityContexts(Lists.newArrayList(securityContext()))
            .securitySchemes(Lists.newArrayList(apiKey()))
            .useDefaultResponseMessages(false)
            .select()
            .paths(regex(DEFAULT_INCLUDE_PATTERN))
            .build();
    }

    /**
     * Get the API Info 
     * 
     * @return The API Info
     */
    private ApiInfo getApiInfo() {

        // build API Info
        return new ApiInfo(
            String.format("%s: Swagger UI", this.applicationName),
            String.format("The API Reference for the %s service", this.applicationName),
            ApiInfo.DEFAULT.getVersion(),
            ApiInfo.DEFAULT.getTitle(),
            ApiInfo.DEFAULT.getContact(),
            ApiInfo.DEFAULT.getLicense(),
            ApiInfo.DEFAULT.getLicenseUrl(),
            ApiInfo.DEFAULT.getVendorExtensions()
        );
    }
}
