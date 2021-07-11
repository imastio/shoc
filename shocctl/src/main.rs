mod fs;
use clap::{ crate_version, App, Arg };

fn main() {

    let matches = App::new("shocctl")
        .about("When HPC goes Serlverless")
        .version(crate_version!())
        .author("Davit Petrosyan <davit.petrosyan@imast.io>")
        .arg(
            Arg::new("context")
                .long("context")
                .about("the path to the project folder")
                .value_name("FOLDER")
                .default_value(&fs::cwd().unwrap())
                .required(true)
        )
        .subcommand(
            App::new("init") 
                .about("Initialize the folder as a shoc project")
                .arg(
                    Arg::new("name")
                        .about("the name of project")
                        .index(1)
                        .required(false),
                ),
        )
        .get_matches();

    println!("Hello, world! Context {}", matches.value_of("context").unwrap());
}
