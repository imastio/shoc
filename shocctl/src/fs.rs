pub fn cwd() -> Option<String> {    
    std::env::current_dir()
        .ok()?
        .to_str()?
        .to_owned()
        .into()
}