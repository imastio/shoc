package io.imast.shoc.shocctl.common;

import io.imast.shoc.model.ShocManifest;
import java.util.LinkedHashMap;

/**
 * The set of shoc utilities
 * 
 * @author Davit.Petrosyan
 */
public class Models {
   
    /**
     * Maps shoc manifest into a ordered map
     * 
     * @param source The source object
     * @return Returns map representation
     */
    public static LinkedHashMap<String, Object> toMap(ShocManifest source){
        
        // resulting object
        var result = new LinkedHashMap<String, Object>();
        
        result.put("name", source.getName());
        result.put("folder", source.getFolder());
        result.put("labels", source.getLabels());
        result.put("technology", source.getTechnology());
        result.put("flavor", source.getFlavor());
        result.put("packages", source.getPackages());
        result.put("includes", source.getIncludes());
        result.put("excludes", source.getExcludes());
        result.put("entrypoint", source.getEntrypoint());
        result.put("args", source.getArgs());
   
        return result;
    }
    
}
