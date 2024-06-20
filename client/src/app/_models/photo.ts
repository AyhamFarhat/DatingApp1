/*
    the export keyword is used to export functions, objects or primitive values from a given 
    file (or module) so they can be used by other files or modules with the import keyword.
*/
export interface Photo {
    id: number;
    url: string;
    isMain: boolean;
}
