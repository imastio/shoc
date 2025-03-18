import ErrorDefinitions from "@/addons/error-handling/error-definitions";
import { selfClient } from "@/clients/shoc";
import JobTasksClient from "@/clients/shoc/job/job-tasks-client";
import { ObjectContainer } from "@/components/general/object-container";
import { Input } from "antd";
import { useCallback, useEffect, useRef, useState } from "react";

export default function LogsArea({ workspaceId, jobId, taskId, loading, onUpdate = () => { } }: any) {
    const [progress, setProgress] = useState(true);
    const [data, setData] = useState<any>('');
    const [fatalError, setFatalError] = useState<any>(null);
    const logContainerRef = useRef<any>(null);

    const { url } = selfClient(JobTasksClient).getLogsByIdUrl(workspaceId, jobId, taskId);

    const load = useCallback(async () => {
        setFatalError(null);
        setData('');
        setProgress(true);
    
        try {
            const response = await fetch(url, {
                headers: {
                    'x-shoc-sse': 'yes'
                },
            });
            setProgress(false);
    
            if (!response.ok) {
                const errorData = await response.json();
                setFatalError({ statusCode: response.status, errors: [...(errorData.errors || [])] });
                return;
            }
    
    
            if(!response.body){
                return;
            }

            const reader = response.body.getReader();
            const decoder = new TextDecoder();
    
            let logs = '';
    
            while (true) {
                const { done, value } = await reader.read();
    
                if (done) {
                    break;
                }
    
                const chunk = decoder.decode(value, { stream: true });
    
                const events = chunk.split('\n\n');
                for (const event of events) {
                    if (event.startsWith('data: ')) {
                        const eventData = event.slice(6);
                        logs += `${eventData}\n`;
                        setData(logs);
                    }
                }
            }
        } catch (error: any) {
            setProgress(false);
            setFatalError({ statusCode: 500, errors: [ErrorDefinitions.unknown(error?.message || 'Unknown error')] });
        }
    }, [url]);

    useEffect(() => {

        if (!workspaceId || !jobId || !taskId) {
            return;
        }

        load();
    }, [url]);

    useEffect(() => {
        if (logContainerRef.current) {
            const textareaElement = logContainerRef.current.resizableTextArea.textArea;
            textareaElement.scrollTop = textareaElement.scrollHeight;
        }
    }, [data]); 

    return <ObjectContainer loading={loading || progress} fatalError={fatalError}>
            <Input.TextArea 
                ref={logContainerRef} 
                placeholder="Logs will appear here..." 
                style={{ width: '100%' }}
                autoSize={{maxRows: 20}}
                value={data} />
    </ObjectContainer>

}