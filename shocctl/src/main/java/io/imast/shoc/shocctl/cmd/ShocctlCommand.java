package io.imast.shoc.shocctl.cmd;

import java.util.concurrent.Callable;
import picocli.CommandLine.Command;

@Command(
        name = "shocctl", 
        mixinStandardHelpOptions = true, 
        version = "0.0.1", 
        description = "runs your HPC workloads serverless"
)
public class ShocctlCommand implements Callable<Integer> {
    
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
}
