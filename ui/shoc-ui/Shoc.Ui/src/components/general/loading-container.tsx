import React, { ReactNode } from 'react';
import SpinnerIcon from '@/components/icons/spinner-icon';
import { cn } from '@/lib/utils';

export default function LoadingContainer({ loading, children, className }: { loading?: boolean, children: ReactNode, className?: string }) {
  return (
    <div className={cn("relative", className)}>
      <div className={`${loading ? 'opacity-50 pointer-events-none' : ''}`}>
        {children}
      </div>
      {loading && (
        <div className="absolute inset-0 flex items-center justify-center bg-white bg-opacity-50">
            <SpinnerIcon className="animate-spin h-5 w-5" />
        </div>
      )}
    </div>
  );
};
