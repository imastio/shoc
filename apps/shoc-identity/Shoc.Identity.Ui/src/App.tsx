import { useEffect, useState } from 'react';
import './App.css';

function App() {
    const [data, setData] = useState<{}>();

    useEffect(() => {
        getData();
    }, []);

    const contents = data === undefined
        ? <p><em>Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationreact">https://aka.ms/jspsintegrationreact</a> for more details.</em></p>
        : <pre>
            {JSON.stringify(data, null, 4)}
        </pre>;

    return (
        <div>
            <h1 id="tabelLabel">Well Known Config</h1>
            <p>This component demonstrates fetching data from the server.</p>
            {contents}
        </div>
    );

    async function getData() {
        const response = await fetch('.well-known/openid-configuration');
        const data = await response.json();
        setData(data);
    }
}

export default App;