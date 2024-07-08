import React, { ReactNode } from 'react';
import SpinnerIcon from '@/components/icons/spinner-icon';

export default function LoadingContainer({ loading, children }: { loading?: boolean, children: ReactNode }) {
  return (
    <div className="relative">
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
