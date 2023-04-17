import Loader from "components/loader";
import Helmet from "react-helmet";
import { useNavigate, useSearchParams } from "react-router-dom";
import { Button, Modal } from "antd";
import { useCallback, useEffect, useState } from "react";
import { actions as userActions } from "redux/users/slice";
import { useDispatch } from "react-redux";
import { isEmpty } from "extensions/string";
import { CloseOutlined } from "@ant-design/icons";

const SignOutPage = () => {

    const navigate = useNavigate();
    const [searchParams] = useSearchParams();
    const dispatch = useDispatch();

    const logoutId = searchParams.get('logout_id') || searchParams.get('logoutId')

    const [manual, setManual] = useState(isEmpty(logoutId));
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

    }, [dispatch, manual, logoutId]);

    useEffect(() => {
        if(!isEmpty(logoutId)){
            signOutProcess();
        }  
    }, [signOutProcess, logoutId])

    return (
        <>
            <Helmet title="Sign out" />
            <Loader />
            <Modal
                title={<h5>Sign out</h5>}
                centered
                visible={manual}
                onOk={signOutProcess}
                onCancel={() => navigate(-1)}
                width={1000}
                footer={
                    <>
                        <Button onClick={() => navigate(-1)} type="text">
                        No
                        </Button>

                        <Button loading={progress} onClick={ signOutProcess } type="primary">
                        Yes
                        </Button>
                    </>
                }
                closeIcon={
                    <CloseOutlined />
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
                        navigate(redirectUri, { replace: true });
                    }
                    else{
                        window.location.href = redirectUri;
                    }
            }}></iframe> }
        </>
    )
}

export default SignOutPage;