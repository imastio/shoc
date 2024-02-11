/**
 * @description
 * Takes an Array<V>, and a grouping function,
 * and returns a Map of the array grouped by the grouping function.
 *
 * @param list An array of type V.
 * @param keyGetter A Function that takes the the Array type V as an input, and returns a value of type K.
 *                  K is generally intended to be a property key of V.
 *
 * @returns Map of the array grouped by the grouping function.
 */
//export function groupBy<K, V>(list: Array<V>, keyGetter: (input: V) => K): Map<K, Array<V>> {
//    const map = new Map<K, Array<V>>();
export function groupBy(list, keyGetter) {
    const map = new Map();
    list.forEach((item) => {
        const key = keyGetter(item);
        const collection = map.get(key);
        if (!collection) {
            map.set(key, [item]);
        } else {
            collection.push(item);
        }
    });
    return map;
}

/**
 * Inject paretns to the children by given root
 * 
 * @param {*} root The root node
 * @param {*} children The children of root
 * @param {*} childrenSelector The selector of children
 */
export const injectParents = (root, children, childrenSelector = 'children') => {

    // no children means nothing to do
    if (!children || children.length === 0) {
        return;
    }

    // process each child
    for (let current of children) {
        current.parent = root;
        injectParents(current, current[childrenSelector])
    }
}

/**
 * Deep find in tree by predicate
 * 
 * @param {*} root The root to start traverse
 * @param {*} predicate The predicate to apply
 * @returns Returns first matching element
 */
export const deepFind = (root, predicate) => {

    // try match the current element
    const found = predicate(root);

    // that's it
    if (found) {
        return root;
    }

    // take children
    const children = root.children || [];

    // try find in childs one by one
    for (let child of children) {

        // try find in current child
        const childFound = deepFind(child, predicate);

        // return if found
        if (childFound) {
            return childFound;
        }
    }

    // not found
    return null;
}

export function toTreeCollection(data, idSelector = 'id', parentSelector = 'parentId', rootParentId = null) {
    const treeCollection = [];
    const treeMap = {};

    data.forEach(entity => {
        treeMap[entity[idSelector]] = { ...entity, children: [] };
    });

    data.forEach(entity => {
        const parentID = entity[parentSelector];
        if (parentID === rootParentId) {
            treeCollection.push(treeMap[entity[idSelector]]);
        } else {
            if (treeMap[parentID]) {
                treeMap[parentID].children.push(treeMap[entity[idSelector]]);
            }
        }
    });

    return treeCollection;
}

export function filterTreeNode(input, node, predicate = (node) => node.label) {
    const nodeTitle = predicate(node).toLowerCase();
    const inputValue = input.toLowerCase();

    if (nodeTitle.includes(inputValue)) {
        return true;
    }

    if (node.children) {
        return node.children.some(child => filterTreeNode(input, child, predicate));
    }

    return false;
};
