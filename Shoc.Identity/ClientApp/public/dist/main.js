var mgr = new oidc.UserManager({});
mgr.signinSilentCallback().catch((err) => console.log(err));
