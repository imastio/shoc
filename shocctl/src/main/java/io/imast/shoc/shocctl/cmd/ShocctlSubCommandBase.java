package io.imast.shoc.shocctl.cmd;

import java.nio.file.Path;
import picocli.CommandLine.Command;
import picocli.CommandLine.ParentCommand;

/**
 * The base interface for all shoc sub-commands
 * 
 * @author Davit.Petrosyan
 */
@Command
public abstract class ShocctlSubCommandBase extends ShocctlCommandBase {
    
    /**
     * The shocctl command 
     */
    @ParentCommand
    protected ShocctlCommand shocctlCommand;
    
    /**
     * Gets the context directory
     * 
     * @return Returns context
     */
    public String getContext(){
        return this.shocctlCommand.getContext();
    }
    
    /**
     * Gets the shoc directory
     * 
     * @return Returns shoc directory
     */
    public Path getShocManifest(){
        return Path.of(this.getContext(), SHOC_MANIFEST);
    }
}
