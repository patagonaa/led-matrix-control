import { Guid } from "./uuidv4";

export class RestDataService {
    public static async put<T>(endpoint: string, id: Guid, model: T): Promise<void> {
        let uri = `/api/${endpoint}/${id}`;

        await fetch(uri, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json; charset=utf-8"
            },
            body: JSON.stringify(model)
        });
    }

    public static async get<T>(endpoint: string, id: Guid): Promise<T> {
        const response = await fetch(`/api/${endpoint}/${id}`);
        return (await response.json()) as T;
    }

    public static async getList<T>(endpoint: string): Promise<T[]> {
        const response = await fetch(`/api/${endpoint}`);
        return (await response.json()) as T[];
    }
}