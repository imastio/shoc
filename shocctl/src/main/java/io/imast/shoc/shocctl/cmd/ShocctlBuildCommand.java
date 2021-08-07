package io.imast.shoc.shocctl.cmd;

import java.nio.file.Files;
import picocli.CommandLine.Command;

/**
 * The build command of shocctl
 * 
 * @author Davit.Petrosyan
 */
@Command(
        name = "build", 
        description = "Build shoc project"
)
public class ShocctlBuildCommand extends ShocctlSubCommandBase {
    
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
        
        // the context directory
        var ctx = this.getContext();
        
        // the manifest file path
        var manifest = this.getShocManifest();
        
        // if manifest exists and is not a regular file 
        if(!Files.isRegularFile(manifest)){
            System.err.println(String.format("A manifest file %s does not exist or invalid", SHOC_MANIFEST));
            return 1;
        }
        
        
        return 0;
    }
}
