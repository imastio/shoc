package io.imast.shoc.shocctl;

import io.imast.shoc.shocctl.cmd.ShocctlBuildCommand;
import io.imast.shoc.shocctl.cmd.ShocctlCommand;
import io.imast.shoc.shocctl.cmd.ShocctlInitCommand;
import picocli.CommandLine;
import picocli.CommandLine.HelpCommand;

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
        System.exit(new CommandLine(new ShocctlCommand())
                .addSubcommand(new ShocctlInitCommand())
                .addSubcommand(new ShocctlBuildCommand())
                .addSubcommand(new HelpCommand())
                .execute(args)); 
    }
}