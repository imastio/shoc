package io.imast.shoc.shocctl;

import io.imast.shoc.shocctl.cmd.ShocctlCommand;
import picocli.CommandLine;

/**
 * The CLI app for shocctl
 * 
 * @author Davit.Petrosyan
 */
public class ShocctlCli {

    /**
     * An entry point for shocctl 
     * 
     * @param args The command line arguments
     */
    public static void main(String[] args) {
        System.exit(new CommandLine(new ShocctlCommand()).execute(args)); 
    }
}