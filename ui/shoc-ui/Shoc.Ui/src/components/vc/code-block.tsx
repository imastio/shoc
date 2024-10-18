"use client"

import { cn } from '@/lib/utils';
import { CheckIcon, ClipboardIcon } from '@radix-ui/react-icons';
import { useState } from 'react';
import { Light as SyntaxHighlighter } from 'react-syntax-highlighter';
import { atomOneDark } from 'react-syntax-highlighter/dist/esm/styles/hljs';

export default function CodeBlock({ code, language, className }: { code: string, language: string, className?: string }) {
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
        <div className={cn("relative bg-transparent text-white rounded-lg shadow-lg overflow-hidden", className)}>
            <button
                onClick={handleCopy}
                className="absolute top-2 right-2 bg-gray-700 hover:bg-gray-600 text-white p-1 rounded"
            >
                {isCopied ? <CheckIcon className="w-5 h-5" /> : <ClipboardIcon className="w-5 h-5" />}
            </button>

            <SyntaxHighlighter language={language} style={atomOneDark} customStyle={{ padding: 8 }}>
                {code}
            </SyntaxHighlighter>
        </div>
    );
}
