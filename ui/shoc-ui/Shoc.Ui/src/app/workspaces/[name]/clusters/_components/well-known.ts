import { IntlMessageId } from "@/i18n/sources"

export const clusterNamePattern = /^[a-z\d](?:[a-z\d]|-(?=[a-z\d])){0,38}$/

export const clusterTypes: { key: string, display: IntlMessageId }[] = [ 
    {
        key: 'k8s', 
        display: 'workspaces.clusters.types.k8s'
    }
]

export const clusterTypesMap = Object.assign({}, ...clusterTypes.map((entry) => ({[entry.key]: entry.display})));;
