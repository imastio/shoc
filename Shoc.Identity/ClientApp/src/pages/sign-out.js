import Loader from "components/loader";
import Helmet from "react-helmet";
import { useHistory } from "react-router";
import { Button, Modal } from "antd";
import { RiCloseFill } from "react-icons/ri";
import { useCallback, useEffect, useState } from "react";
import qs from "qs";
import _ from "lodash";
import { actions as userActions } from "redux/users/slice";
import { useDispatch } from "react-redux";

// get the query string
const query = qs.parse(document.location.search, { ignoreQueryPrefix: true });

// the logout id from query
const logoutId = query.logout_id || query.logoutId;

const SignOutPage = () => {

    const history = useHistory();
    const dispatch = useDispatch();

    const [manual, setManual] = useState(_.isEmpty(logoutId));
    const [progress, setProgress] = useState(false);
    const [iframeUrl, setIframeUrl] = useState(null);
    const [redirectUri, setRedirectUri] = useState("/");
    const [continueFlow, setContinueFlow] = useState(true);

    const signOutProcess = useCallback(async () => {
        setProgress(true)
        
        const result = await dispatch(userActions.signout({
          input: {
            requireValidContext: !manual,
            logoutId: logoutId
          }
        }))
        setProgress(false)
    
        if (result.error) {
          setManual(true);
          return;
        }
    
        // get payload
        const payload = result?.payload || {};
    
        setIframeUrl(payload.signOutIframeUrl);
        setRedirectUri(payload.postLogoutRedirectUri || "/");
        setContinueFlow(payload.continueFlow);   
        
        if(!payload.signOutIframeUrl && payload.postLogoutRedirectUri){
            window.location.href = payload.postLogoutRedirectUri;
        }

    }, [dispatch, manual]);

    useEffect(() => {
        if(!_.isEmpty(logoutId)){
            signOutProcess();
        }  
    }, [signOutProcess])

    return (
        <>
            <Helmet title="Sign out" />
            <Loader />
            <Modal
                title={<h5>Sign out</h5>}
                centered
                visible={manual}
                onOk={signOutProcess}
                onCancel={() => history.goBack()}
                width={1000}
                footer={
                    <>
                        <Button onClick={() => history.goBack()} type="text">
                        No
                        </Button>

                        <Button loading={progress} onClick={ signOutProcess } type="primary">
                        Yes
                        </Button>
                    </>
                }
                closeIcon={
                    <RiCloseFill
                        className="remix-icon"
                        size={24}
                    />
                }
            >
                <p>
                    Are you sure you want to sign out?
                </p>
            </Modal>
            { iframeUrl && <iframe key="signout-notification-iframe" 
                title="Sign Out Silently" 
                src={ iframeUrl } 
                style={{position: 'absolute', width: 0,height: 0, border:0}} 
                onLoad={() => {
                    if(redirectUri.startsWith("/") && !continueFlow){
                        history.replace(redirectUri);
                    }
                    else{
                        window.location.href = redirectUri;
                    }
            }}></iframe> }
        </>
    )
}

export default SignOutPage;