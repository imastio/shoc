import { Component } from 'react';
import { Result } from 'antd';

class ErrorBoundary extends Component {
    
    constructor(props) {
        super(props);
        this.state = { error: null };
    }

    static getDerivedStateFromError(error) {  
        return { error } 
    }

    componentDidCatch(error, errorInfo) { 

    }

    render() {
        if (this.state.error) {
            return (<Result status="error" subTitle="Something went wrong!" />)
        }

        return this.props.children;
    }
}

export default ErrorBoundary;