package io.imast.shoc.shocctl.cmd;

import java.util.concurrent.Callable;

/**
 * The base interface for all shoc commands
 * 
 * @author Davit.Petrosyan
 */
public abstract class ShocctlCommandBase implements Callable<Integer> {
    
    /**
     * The manifest file name
     */
    protected static final String SHOC_MANIFEST = "manifest.yml";
    
    /**
     * Override this to put logic of command
     * 
     * @return Returns error code
     * @throws Exception 
     */
    @Override
    public abstract Integer call() throws Exception;
        
    /**
     * Exit on failure 
     * 
     * @param code The exit code
     */
    protected void exitOnFailure(int code){
        if(code != 0){
            System.exit(code);
        }
    }
}
