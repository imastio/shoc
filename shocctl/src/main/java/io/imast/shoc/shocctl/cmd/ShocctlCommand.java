package io.imast.shoc.shocctl.cmd;

import io.vavr.control.Try;
import java.io.File;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.concurrent.Callable;
import picocli.CommandLine.Command;
import picocli.CommandLine.Option;

@Command(
        name = "shocctl", 
        version = "0.0.1", 
        description = "Runs your HPC workloads serverless",
        mixinStandardHelpOptions = true
)
public class ShocctlCommand implements Callable<Integer> {
    
    /**
     * The context directory
     */
    @Option(names = { "-c", "--context" }, defaultValue = ".", description = "The context directory of project")
    private File context;
    
    /**
     * Handles shocctl global command 
     * 
     * @return Returns result integer
     * @throws Exception 
     */
    @Override
    public Integer call() throws Exception { 
        return 0;
    }
    
    /**
     * Gets the context directory
     * 
     * @return Returns context
     */
    public String getContext(){
        return Try.of(() -> this.context.getCanonicalPath()).getOrNull();
    }
    
    /**
     * Validates the command base 
     * 
     * @return Returns validation result
     */
    public Integer validate(){
        
        var ctx = this.getContext();
        
        if(ctx == null){
            System.err.println(String.format("No context is available"));
            return 1;
        }
        
        if(!Files.isDirectory(Path.of(ctx))){
            System.out.println(String.format("Given context is not directory"));
            return 1;
        }
        
        return 0;
    }
}
