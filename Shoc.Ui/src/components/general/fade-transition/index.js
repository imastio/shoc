import { useEffect } from "react";
import { useState } from "react"
import { useLocation } from "react-router-dom";
import './styles.css';

export default function FadeTransition({children}){

    const location = useLocation();
    const [stage, setStage] = useState('fadeIn');
    const [displayLocation, setDisplayLocation] = useState(location);

    useEffect(() => {

        if(location !== displayLocation){
            setStage('fadeOut');
        }

    }, [location, displayLocation])

    const handleAnimationEnd = () => {

        if(stage === 'fadeOut'){
            setStage('fadeIn');
            setDisplayLocation(location);
        }
    }

    return <div className={stage} onAnimationEnd={handleAnimationEnd}>
        {children}
    </div>

}