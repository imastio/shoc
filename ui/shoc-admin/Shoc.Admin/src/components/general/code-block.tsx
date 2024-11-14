"use client"

import { CheckOutlined, CopyFilled } from '@ant-design/icons';
import { Button } from 'antd';
import { useState } from 'react';
import { Light as SyntaxHighlighter } from 'react-syntax-highlighter';
import { atomOneDark } from 'react-syntax-highlighter/dist/esm/styles/hljs';

export default function CodeBlock({ code, language }: { code: string, language: string}) {
    const [isCopied, setIsCopied] = useState(false);

    const handleCopy = async () => {
        try {
            await navigator.clipboard.writeText(code);
            setIsCopied(true);
            setTimeout(() => setIsCopied(false), 2000); // Reset after 2 seconds
        } catch (error) {
            console.error('Failed to copy text:', error);
        }
    };

    return (
        <div style={{
            overflow: 'hidden',
            position: 'relative',
            color: 'white',
            borderRadius: '8px'
        }} >
            <Button
                type="text"
                onClick={handleCopy}
                style={{
                    position: 'absolute',
                    padding: '4px',
                    top: '2px',
                    right: '2px',
                    color: 'white',
                }}
            >
                {isCopied ? <CheckOutlined /> : <CopyFilled />}
            </Button>

            <SyntaxHighlighter language={language} style={atomOneDark} customStyle={{ padding: 8, margin: 0  }}>
                {code}
            </SyntaxHighlighter>
        </div>
    );
}
