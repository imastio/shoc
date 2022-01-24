import React from 'react';
import { BrowserRouter, Switch } from 'react-router-dom';
import routes from 'routes'
import MaybeProtectedRoute from 'maybe-protected-route';

const PageRouter = (props) => {
    return (
        <BrowserRouter>
            <Switch>
                {routes.map(route => <MaybeProtectedRoute {...props} {...route} />)}
            </Switch>
        </BrowserRouter>
    )
}

export default PageRouter;
