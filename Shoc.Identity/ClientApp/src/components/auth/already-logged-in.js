import React from "react";
import { Button, Col } from "antd";
import { useAuth } from "auth/useAuth";

const AlreadyLoggedIn = () => {

    const auth = useAuth();
    return (
        <>
        <Col>
                <span>
                  Hey, {auth?.user?.profile?.name || "User" }
                </span>
                <span className="enl-text-color-black-80 enl-caption enl-mr-4">
                  You are already signed in. Please sign-out first.
                </span>
                <Button type="link" onClick={ () => auth.getUserManager().signoutRedirect() }>
                    Sign out
                </Button>
            </Col>
              
        </>
    );
};

export default AlreadyLoggedIn;