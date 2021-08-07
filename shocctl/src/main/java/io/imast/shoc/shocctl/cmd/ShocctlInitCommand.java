package io.imast.shoc.shocctl.cmd;

import io.imast.shoc.common.Yml;
import io.imast.shoc.model.ShocManifest;
import io.imast.shoc.shocctl.common.Models;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.Arrays;
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
        
        // the context directory
        var ctx = this.getContext();
        
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
        
        // get context path
        var ctxPath = Paths.get(ctx);
        
        // get project candidate name
        var projectName = ctxPath.getFileName().toString();
        
        // get dir name
        var projectDir = String.format("/%s/", ctxPath.getParent().getFileName().toString());
        
        var manifestObject = ShocManifest.builder()
                .name(projectName)
                .folder(projectDir)
                .labels(Arrays.asList())
                .technology("")
                .flavor("")
                .build();
        
        // dump to file
        Files.writeString(manifest, Yml.write(Models.toMap(manifestObject)));
        
        return 0;
    }
}
