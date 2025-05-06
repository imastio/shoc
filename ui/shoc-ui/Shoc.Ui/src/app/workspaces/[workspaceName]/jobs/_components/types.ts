export type Job = {
    id: string,
    workspaceId: string,
    workspaceName: string
    localId: number,
    clusterId: number,
    clusterName: string,    
    userId: string,
    userFullName: string,
    name: string,
    description: string,
    scope: JobScope,
    manifest: string,
    namespace: string,
    totalTasks: number,
    succeededTasks: number,
    failedTasks: number,
    cancelledTasks: number,
    completedTasks: number,
    status: JobStatus,
    message: string,
    pendingAt?: string | null,
    runningAt?: string | null,
    completedAt?: string | null,
    created: string,
    updated: string
}

export type JobStatus = 'created' | 'pending' | 'running' | 'partially_succeeded' | 'succeeded' | 'failed' | 'cancelled';

export type JobScope = 'user' | 'workspace';

export interface FilterOptions {
    scope?: JobScope
    status?: JobStatus
    userId?: string
    page: number
    size: number
}