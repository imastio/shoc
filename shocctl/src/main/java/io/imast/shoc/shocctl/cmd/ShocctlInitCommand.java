package io.imast.shoc.shocctl.cmd;

import java.nio.file.Files;
import picocli.CommandLine.Command;

/**
 * The init command of shocctl
 * 
 * @author Davit.Petrosyan
 */
@Command(
        name = "init", 
        description = "Initialize shoc project"
)
public class ShocctlInitCommand extends ShocctlSubCommandBase {
    
    /**
     * Initialize shoc project with required files
     * 
     * @return Returns result of the command
     * @throws Exception 
     */
    @Override
    public Integer call() throws Exception {
        
        // validate
        this.exitOnFailure(this.shocctlCommand.validate());
        
        // the manifest file path
        var manifest = this.getShocManifest();
        
        // if manifest exists and is not a regular file 
        if(Files.exists(manifest) && !Files.isRegularFile(manifest)){
            System.err.println(String.format("A manifest file %s exists but invalid. Review and delete %s first.", SHOC_MANIFEST, SHOC_MANIFEST));
            return 1;
        }
        
        // if manifest exists and is a regular file 
        if(Files.isRegularFile(manifest)){
            System.out.println(String.format("A manifest file %s is already there.", SHOC_MANIFEST));
            return 0;
        }
        
        // create an empty manifest if does not exist
        if(!Files.exists(manifest)){
            Files.createFile(manifest);
        }
                
        return 0;
    }
}
